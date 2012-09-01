Imports System.IO
Imports System.Drawing
Imports Ceptara.SystemInterface

Public Class MySkype

#Region "Constants"

    Private Const SD_DTMF_TIME_BETWEEN_KEY_PRESSES As Integer = 300 ' milliseconds

#End Region

#Region "Fields"

    Public WithEvents oSkype As SKYPE4COMLib.Skype

    ' - Call Object Properties
    ' Target Identity = Skype Users External PSTN number
    ' PSTN Number = Caller ID number for incoming call, e.g. +14253184600
    ' Partner Handle = PSTN: Caller ID, P2P: Calling user's Skype Handle
    ' Partner DisplayName = PSTN: Caller ID, P2P: Calling User's Skype Display Name
    Private _CurrentCall As SKYPE4COMLib.Call = Nothing

    ' - SMS Object Properties
    Private _CurrentSMS As SKYPE4COMLib.SmsMessage = Nothing

    Private _AttachmentStatus As SKYPE4COMLib.TAttachmentStatus
    Private _CurrentCallStatus As String = "Unknown"
    Private _UserSearchInProgress As Boolean = False

    Private _SearchDelay As Integer = 100 ' in milliseconds
    Private _SearchUserCollection As SKYPE4COMLib.UserCollection = Nothing
    Private _LastSearchUser As SKYPE4COMLib.User = Nothing

#End Region

#Region "Properties"

    Public ReadOnly Property IsConnected() As Boolean
        Get
            If IsNothing(oSkype) Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property

    Public ReadOnly Property AttachmentStatus() As SKYPE4COMLib.TAttachmentStatus
        Get
            Return _AttachmentStatus
        End Get
    End Property

    Public ReadOnly Property CurrentCall() As SKYPE4COMLib.Call
        Get
            Return _CurrentCall
        End Get
    End Property
    Public ReadOnly Property CurrentCallStatus() As String
        Get
            Return _CurrentCallStatus
        End Get
    End Property
    Public ReadOnly Property CurrentCreditBalance() As String
        Get
            Return oSkype.CurrentUserProfile.BalanceToText
        End Get
    End Property
    Public ReadOnly Property UserCountryCode() As String
        Get
            Return oSkype.User.CountryCode
        End Get
    End Property
    Public ReadOnly Property MySkypeHandle() As String
        Get
            Return oSkype.CurrentUser.Handle
        End Get
    End Property
    Public ReadOnly Property UserSearchInProgress() As Boolean
        Get
            Return _UserSearchInProgress
        End Get
    End Property
    Public ReadOnly Property InSilentMode() As Boolean
        Get
            Return oSkype.SilentMode
        End Get
    End Property

    Public ReadOnly Property CurrentSMSMessage As SKYPE4COMLib.SmsMessage
        Get
            Return _CurrentSMS
        End Get
    End Property

    ' Chats

    ''' <summary>
    ''' Current / Active Chat (last one to be statused)
    ''' </summary>
    ''' <value></value>
    ''' <returns>Skype4ComLib.Chat</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property CurrentChat As SKYPE4COMLib.Chat
        Get
            Return _currentChat
        End Get
    End Property
    Dim _currentChat As SKYPE4COMLib.Chat = Nothing
    ''' <summary>
    ''' ArrayList of ongoing Chats
    ''' </summary>
    ''' <value></value>
    ''' <returns>List(Of SKYPE4COMLib.Chat)</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ActiveChats As List(Of SKYPE4COMLib.Chat)
        Get
            Return _activeChats
        End Get
    End Property
    Dim _activeChats As List(Of SKYPE4COMLib.Chat) = Nothing


#End Region

#Region "Events"

    Public Event IncomingCall(ByVal pCall As SKYPE4COMLib.Call)
    Public Event ChatMessageStatus(ByVal Message As SKYPE4COMLib.ChatMessage, _
                                     ByVal Status As SKYPE4COMLib.TChatMessageStatus)

#End Region

#Region "Methods"

    Public Sub New()

        Dim strTrace As String = "General Fault."
        Try
            ' Create Object Instance
            strTrace = "Creating new Skype instance."
            oSkype = New SKYPE4COMLib.Skype

            ' Create other objects
            _activeChats = New List(Of SKYPE4COMLib.Chat)

            ' Make sure Skype is running
            strTrace = "Checking if Skype is running."
            If Not oSkype.Client.IsRunning Then
                strTrace = "Starting Skype application."
                oSkype.Client.Start()
                Threading.Thread.Sleep(1500)
            End If
            strTrace = "Attaching to Skype COM Interface."
            oSkype.Attach(, True)

        Catch ex As Exception
            Errorlogger(ex.Message & ". " & strTrace, Err.Number, "MySkype:New")
        End Try

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        Try
            ' Reset Skype app
            If InSilentMode Then oSkype.SilentMode = False

        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "MySkype:Finalize")
        End Try

    End Sub

    Public Overloads Function EndChat(ByVal ChatName As String) As Boolean
        Return False
    End Function

    Public Overloads Function EndChat(ByVal xChat As SKYPE4COMLib.Chat) As Boolean

        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "clsMySkype:EndChat_2"
        Try
            strTrace = "Remove chat from the ActiveChats list."
            _activeChats.Remove(xChat)
            strTrace = "Leave the chat."
            xChat.Leave()

            Return True
        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(strTrace, ex, Err, strRoutine)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Creates a new Chat session with the named Skype user
    ''' </summary>
    ''' <param name="SkypeHandle">String: Skype user specified by their handle</param>
    ''' <returns>Skype4ComLib.Chat</returns>
    ''' <remarks></remarks>
    Public Function CreateChatWith(ByVal SkypeHandle As String) As SKYPE4COMLib.Chat
        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "clsMySkype:CreateChatWith"
        Try
            If String.IsNullOrEmpty(SkypeHandle) Then
                strTrace = "No Skype handle was specified."
                Throw New Exception("Unable to create a chat session.")
            End If

            strTrace = "Creating new chat session with '" & SkypeHandle & "'."
            Dim newChat As SKYPE4COMLib.Chat = oSkype.CreateChatWith(SkypeHandle)
            If Not IsNothing(newChat) Then
                strTrace = "Adding chat session to the Active Chats list, Session Name: '" & newChat.Name & "'."
                TraceLogger(strTrace, strRoutine)

                _activeChats.Add(newChat)
                _currentChat = newChat
            End If

            Return newChat
        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(strTrace, ex, Err, strRoutine)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Sends an SMS message to a well formed phone number, e.g. +14255551212
    ''' </summary>
    ''' <param name="PhoneNumber">String: Well formed phone number</param>
    ''' <param name="Message">Text message to send</param>
    ''' <param name="ReplyNumber">ReplyToNumber - if different than default (user's own mobile number)</param>
    ''' <remarks></remarks>
    Public Sub SendSMSUsingPhoneNumber(ByVal PhoneNumber As String, _
                                       ByVal Message As String, _
                                Optional ByVal ReplyNumber As String = "")

        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "clsMySkype:SendSMSUsingPhoneNumber"
        Try
            If String.IsNullOrEmpty(PhoneNumber) Then
                strTrace = "Phone number was not specified."
                Throw New Exception("Unable to send the SMS message.")
            End If
            If String.IsNullOrEmpty(Message) Then
                strTrace = "Text message was not specified."
                Throw New Exception("Unable to send the SMS message.")
            End If

            strTrace = "Creating SMS message."
            _CurrentSMS = oSkype.CreateSms(SKYPE4COMLib.TSmsMessageType.smsMessageTypeOutgoing, PhoneNumber)

            Dim myName As String = oSkype.Profile("FullName")

            strTrace = "Adding the text body."
            _CurrentSMS.Body = Message & vbCrLf & vbCrLf & "From " & myName & " via Ceptara/Skype"

            ' 2011-12-11: ReplyTo Number no longer supported and causes an exception
            'strTrace = "Adding the ReplyTo number: '" & ReplyNumber & "'."
            'If ReplyNumber.Length > 0 Then _CurrentSMS.ReplyToNumber = ReplyNumber

            strTrace = "Sending SMS message '" & Message & "' to '" & PhoneNumber & "'."
            _CurrentSMS.Send()

            If Not IsNothing(_CurrentSMS) Then
                strTrace = "Sent SMS message to " & PhoneNumber
                SyncLogger("SK_SendSMS", strTrace, strRoutine)
            Else
                strTrace = "Failed to initiate the SMS message."
                Throw New Exception("Unable to send the SMS message.")
            End If

        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(ex.Message & " " & strTrace, Err.Number, strRoutine)
        End Try
    End Sub

    ''' <summary>
    ''' Send an SMS message to the user specified by the Skype handle.
    ''' </summary>
    ''' <param name="SkypeUserHandle">String: Skype handle</param>
    ''' <param name="Message">String: Message to send</param>
    ''' <param name="ReplyNumber">String: Reply to number</param>
    ''' <remarks></remarks>
    Public Sub SendSMSUsingUserHandle(ByVal SkypeUserHandle As String, _
                                      ByVal Message As String, _
                                Optional ByVal ReplyNumber As String = "")

        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "clsMySkype:SendSMSUsingPhoneNumber"
        Try
            If String.IsNullOrEmpty(SkypeUserHandle) Then
                strTrace = "Skype handle was not specified."
                Throw New Exception("Unable to send the SMS message.")
            End If
            If String.IsNullOrEmpty(Message) Then
                strTrace = "Text message was not specified."
                Throw New Exception("Unable to send the SMS message.")
            End If

            Dim oUser As SKYPE4COMLib.User = GetUserFromSkypeHandle(SkypeUserHandle)
            If Not IsNothing(oUser) Then
                Dim PhoneNumber As String = oUser.PhoneMobile

                _CurrentSMS = oSkype.CreateSms(SKYPE4COMLib.TSmsMessageType.smsMessageTypeOutgoing, PhoneNumber)
                _CurrentSMS.Body = Message

                ' 2011-12-11: ReplyTo Number no longer supported and causes an exception
                'strTrace = "Adding the ReplyTo number: '" & ReplyNumber & "'."
                'If ReplyNumber.Length > 0 Then _CurrentSMS.ReplyToNumber = ReplyNumber

                strTrace = "Sending SMS message '" & Message & "' to '" & PhoneNumber & "'."
                ' _CurrentSMS = oSkype.SendSms(PhoneNumber, Message)
                _CurrentSMS.Send()

                If Not IsNothing(_CurrentSMS) Then
                    strTrace = "Sent SMS message to " & PhoneNumber
                    SyncLogger("SK_SendSMS", strTrace, strRoutine)
                Else
                    strTrace = "Failed to initiate the SMS message."
                    Throw New Exception("Unable to send the SMS message.")
                End If
            Else
                strTrace = "Unable to locate the specified user's record."
                Throw New Exception("Unable to send the SMS message.")
            End If

        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(ex.Message & " " & strTrace, Err.Number, strRoutine)
        End Try
    End Sub

    ''' <summary>
    ''' Initiate a voice call using a well formed phone number, e.g. +14255551212
    ''' </summary>
    ''' <param name="PhoneNumber">String: Well formed phone number</param>
    ''' <remarks></remarks>
    Public Sub MakeCallFromPhoneNumber(ByVal PhoneNumber As String)
        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "clsMySkype:MakeCallFromPhoneNumber"
        Try
            If String.IsNullOrEmpty(PhoneNumber) Then
                strTrace = "No phone number was provided."
                Throw New Exception("Unable to make a call.")
            End If

            Dim strTarget As String = PhoneNumber

            strTrace = "Placing call to: '" & strTarget & "'"
            _CurrentCall = oSkype.PlaceCall(strTarget)

            If Not IsNothing(_CurrentCall) Then
                strTrace = "Placed call to " & strTarget
                SyncLogger("SK_PlaceCall", strTrace, strRoutine)
            Else
                strTrace = "Failed to initiate the call."
                Throw New Exception("Unable to place call.")
            End If

        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(ex.Message & ". " & strTrace, Err.Number, strRoutine)
        End Try
    End Sub

    ''' <summary>
    ''' Initiate a voice call using a user's Skype handle
    ''' </summary>
    ''' <param name="SkypeUserHandle">String: Skype User Handle</param>
    ''' <remarks></remarks>
    Public Sub MakeCallFromUserHandle(ByVal SkypeUserHandle As String)
        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "clsMySkype:MakeCallFromUserHandle"
        Try
            If String.IsNullOrEmpty(SkypeUserHandle) Then
                strTrace = "No Handle was provided."
                Throw New Exception("Unable to make a call.")
            End If

            'strSkypeUserHandle = "echo123"
            strTrace = "Placing call to: '" & SkypeUserHandle & "'"
            _CurrentCall = oSkype.PlaceCall(SkypeUserHandle)

            If Not IsNothing(_CurrentCall) Then
                strTrace = "Placed call to " & _CurrentCall.PartnerHandle
                SyncLogger("SK_PlaceCall", strTrace, strRoutine)
            Else
                strTrace = "Failed to initiate the call."
                Throw New Exception("Unable to place call.")
            End If

        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(ex.Message & ". " & strTrace, Err.Number, strRoutine)
        End Try
    End Sub

    Public Sub MuteCurrentCall()
        Dim strTrace As String = "General Fault."
        Try
            If _CurrentCall.Status = SKYPE4COMLib.TCallStatus.clsInProgress Then
                strTrace = "Muting the call."
                oSkype.Mute = True
            End If
        Catch ex As Exception
            Errorlogger(ex.Message & ". " & strTrace, Err.Number, "MySkype:MuteCurrentCall")
        End Try
    End Sub

    Public Sub UnMuteCurrentCall()
        Dim strTrace As String = "General Fault."
        Try
            If _CurrentCall.Status = SKYPE4COMLib.TCallStatus.clsInProgress Then
                strTrace = "Un-muting the call."
                oSkype.Mute = False
            End If
        Catch ex As Exception
            Errorlogger(ex.Message & ". " & strTrace, Err.Number, "MySkype:UnmuteCurrentCall")
        End Try
    End Sub

    Public Sub EndCurrentCall()
        Dim strTrace As String = "General Fault."
        Try
            strTrace = "Finishing current call."
            _CurrentCall.Finish()

            _CurrentCall = Nothing

        Catch ex As Exception
            Errorlogger(ex.Message & ". " & strTrace, Err.Number, "MySkype:EndCurrentCall")
        End Try
    End Sub

    Public Sub AnswerCurrentCall()
        Dim strTrace As String = "General Fault."
        Try
            If Not IsNothing(_CurrentCall) Then _CurrentCall.Answer()
        Catch ex As Exception
            Errorlogger(ex.Message & ". " & strTrace, Err.Number, "MySkype:AnswerCurrentCall")
        End Try
    End Sub

    Public Sub SendDTMFString(ByVal strDTMFCodes As String)
        Dim strTrace As String = "General Fault."
        Try
            Dim strProperCharacters As String = "0123456789*#"

            If _CurrentCall.Status = SKYPE4COMLib.TCallStatus.clsInProgress Then
                ' Only send codes if a call is in progress
                strTrace = "Sending string: " & strDTMFCodes
                Dim i As Integer = 0
                For i = 0 To strDTMFCodes.Length - 1
                    ' Make sure sending a proper code
                    Dim c As Char = strDTMFCodes.Substring(i, 1)
                    If strProperCharacters.IndexOf(c) >= 0 Then
                        ' Send the code
                        strTrace = "Sending character: " & c & " of code string: " & strDTMFCodes
                        _CurrentCall.DTMF = c
                        Threading.Thread.Sleep(SD_DTMF_TIME_BETWEEN_KEY_PRESSES)
                    End If
                Next
            End If
        Catch ex As Exception
            Errorlogger(ex.Message & ". " & strTrace, Err.Number, "MySkype:SendDTMFString")
        End Try
    End Sub

    Public Sub SendWAVFile(ByVal strFilename As String)
        Try

            Dim strFullPath As String = "c:\users\chris\music\dtmfg.wav"
            Dim strNum3 As String = "c:\users\chris\music\dtmf3.wav"

            If Ceptara.SystemInterface.FileExists(strFullPath) Then
                'oSkype.Mute = True
                _CurrentCall.InputDevice(SKYPE4COMLib.TCallIoDeviceType.callIoDeviceTypeFile) = strNum3
                Threading.Thread.Sleep(250)
                _CurrentCall.InputDevice(SKYPE4COMLib.TCallIoDeviceType.callIoDeviceTypeFile) = strNum3
                Threading.Thread.Sleep(250)
                _CurrentCall.InputDevice(SKYPE4COMLib.TCallIoDeviceType.callIoDeviceTypeFile) = strNum3
                Threading.Thread.Sleep(250)
                _CurrentCall.InputDevice(SKYPE4COMLib.TCallIoDeviceType.callIoDeviceTypeFile) = strNum3
                Threading.Thread.Sleep(250)
                _CurrentCall.InputDevice(SKYPE4COMLib.TCallIoDeviceType.callIoDeviceTypeFile) = strNum3
                Threading.Thread.Sleep(250)
                _CurrentCall.InputDevice(SKYPE4COMLib.TCallIoDeviceType.callIoDeviceTypeFile) = strNum3
                Threading.Thread.Sleep(250)
                _CurrentCall.InputDevice(SKYPE4COMLib.TCallIoDeviceType.callIoDeviceTypeFile) = strNum3
                Threading.Thread.Sleep(250)
                _CurrentCall.InputDevice(SKYPE4COMLib.TCallIoDeviceType.callIoDeviceTypeFile) = strFullPath
                'oSkype.Mute = False
            End If

        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "SendWAVFile")
        End Try
    End Sub

    ''' <summary>
    ''' Launches the Skype Desktop client
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LaunchSkypeDesktopApplication()
        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "clsMySkype:LaunchSkypeDesktopApplication"
        Try
            Dim strSkypeKey As String = "Software\Skype\Phone"
            Dim keySkype As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(strSkypeKey)

            If Not IsNothing(keySkype) Then
                Dim strFullPath As String = keySkype.GetValue("SkypePath")
                If Not IsNothing(strFullPath) Then
                    Dim i As Integer = Shell(strFullPath)
                Else
                    strTrace = "Unable to retrieve Skype installation path from registry."
                    Throw New Exception("Unable to launch the Skype application.")
                End If
            Else
                strTrace = "Unable to retrieve Skype registry key."
                Throw New Exception("Unable to launch the Skype application.")
            End If

        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(strTrace, ex, Err, strRoutine)
        End Try
    End Sub

    Public Sub ViewAccount()
        Try

        Catch ex As Exception

        End Try
    End Sub

    Public Overloads Function FindContactFromUser(ByVal User As SKYPE4COMLib.User) As Outlook.ContactItem
        Return Nothing
    End Function

    Public Overloads Function FindContactFromCall(ByVal iCall As SKYPE4COMLib.Call) As Outlook.ContactItem

        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "clsMySkype:FindContactFromCall"
        Try
            Dim oContact As Outlook.ContactItem = Nothing
            If iCall.Type = SKYPE4COMLib.TCallType.cltIncomingPSTN Then
                oContact = FindContactByPhoneNumber(iCall.PstnNumber)
            ElseIf iCall.Type = SKYPE4COMLib.TCallType.cltIncomingP2P Then

            End If

            Return oContact
        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(strTrace, ex, Err, strRoutine)
            Return Nothing
        End Try
    End Function

    Public Function FindUserHandle(ByVal oContact As Outlook.ContactItem) As String
        Try
            Dim strReturn As String = String.Empty

            If Not IsNothing(_LastSearchUser) Then
                strReturn = _LastSearchUser.Handle
            End If

            Return strReturn
        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "MySkype:FindUserHandle")
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Get the Skype's User record from a SkypeHandle from the authenticated user's friend's list
    ''' </summary>
    ''' <param name="SkypeHandle">String: Skype user's handle</param>
    ''' <returns>Skype4COMLib.User</returns>
    ''' <remarks></remarks>
    Public Function GetUserFromSkypeHandle(ByVal SkypeHandle As String) As SKYPE4COMLib.User
        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "clsMySkype:GetUserFromSkypeHandle"
        Try
            Dim retUser As SKYPE4COMLib.User = Nothing

            If String.IsNullOrEmpty(SkypeHandle) Then
                strTrace = "No Skype handle provided."
                Throw New Exception("Unable to retrieve the Skype User record.")
            End If

            For Each oUser As SKYPE4COMLib.User In oSkype.Friends
                If oUser.Handle.ToUpper = SkypeHandle.ToUpper Then
                    retUser = oUser
                    Exit For
                End If
            Next

            Return retUser
        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(strTrace, ex, Err, strRoutine)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Returns the Contact's Skype Handle if found.
    ''' </summary>
    ''' <param name="oContact">Outlook.ContactItem</param>
    ''' <returns>String: Skype handle if found otherwise, empty string</returns>
    ''' <remarks></remarks>
    Public Function GetUserHandleFromContactItem(ByVal oContact As Outlook.ContactItem) As String
        Try
            Dim retHandle As String = String.Empty

            Dim User As SKYPE4COMLib.User = GetUserFromContactItem(oContact)
            If Not IsNothing(User) Then
                retHandle = User.Handle
            End If

            Return retHandle
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Find contact's Skype User record in the authenticated user's friend list.
    ''' </summary>
    ''' <param name="oContact">Outlook.ContactItem</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUserFromContactItem(ByVal oContact As Outlook.ContactItem) As SKYPE4COMLib.User
        Dim strTrace As String = "General Fault."
        Try
            Dim oUser As SKYPE4COMLib.User = Nothing
            Dim retUser As SKYPE4COMLib.User = Nothing
            Dim strPhone As String = String.Empty

            For Each oUser In oSkype.Friends
                Dim strName As String = oUser.FullName & " | " & oUser.DisplayName
                strTrace = "Evaluating '" & strName & "' friend full name & display name"
                If Not IsNothing(oContact.HomeTelephoneNumber) Then
                    strPhone = GetSkypePhoneNumber(oContact, oContact.HomeTelephoneNumber)
                    If oUser.PhoneHome = strPhone Then
                        retUser = oUser
                        Exit For
                    End If
                End If
                If Not IsNothing(oContact.Home2TelephoneNumber) Then
                    strPhone = GetSkypePhoneNumber(oContact, oContact.Home2TelephoneNumber)
                    If oUser.PhoneHome = strPhone Then
                        retUser = oUser
                        Exit For
                    End If
                End If
                If Not IsNothing(oContact.BusinessTelephoneNumber) Then
                    strPhone = GetSkypePhoneNumber(oContact, oContact.BusinessTelephoneNumber)
                    If oUser.PhoneOffice = strPhone Then
                        retUser = oUser
                        Exit For
                    End If
                End If
                If Not IsNothing(oContact.Business2TelephoneNumber) Then
                    strPhone = GetSkypePhoneNumber(oContact, oContact.Business2TelephoneNumber)
                    If oUser.PhoneOffice = strPhone Then
                        retUser = oUser
                        Exit For
                    End If
                End If
                If Not IsNothing(oContact.MobileTelephoneNumber) Then
                    strPhone = GetSkypePhoneNumber(oContact, oContact.MobileTelephoneNumber)
                    If oUser.PhoneMobile = strPhone Then
                        retUser = oUser
                        Exit For
                    End If
                End If
                If oUser.FullName.ToUpper = oContact.FullName.ToUpper Then
                    retUser = oUser
                    Exit For
                End If
                If oUser.DisplayName.ToUpper = oContact.FullName.ToUpper Then
                    retUser = oUser
                    Exit For
                End If
            Next
            _LastSearchUser = retUser
            Return retUser
        Catch ex As Exception
            Errorlogger(ex.Message & " " & strTrace, Err.Number, "MySkype:FindUserFromFriends")
            Return Nothing
        End Try
    End Function

    Public Function FindUserOnSkype(ByVal oContact As Outlook.ContactItem) As SKYPE4COMLib.User
        ' Doesn't work...
        Try

            Dim oUser As SKYPE4COMLib.User = Nothing

            If Not IsNothing(oContact.Email1Address) Then
                If oContact.Email1Address.Length > 0 Then
                    _UserSearchInProgress = True
                    oSkype.AsyncSearchUsers(oContact.Email1Address)

                    While _UserSearchInProgress
                        Threading.Thread.Sleep(100)
                    End While

                    If _SearchUserCollection.Count > 0 Then
                        For Each oUser In _SearchUserCollection
                            MsgBox(oUser.Handle & " " & oUser.FullName)
                        Next
                    End If
                End If
            End If

            Return oUser
        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "MySkype:FindUser")
            Return Nothing
        End Try
    End Function

    Public Shared Function GetSkypePhoneNumber(ByVal oContact As Outlook.ContactItem, _
                                     ByVal PhoneNumber As String) As String

        Dim strTrace As String = "General Fault."
        Try
            Dim strReturn As String = String.Empty

            'Dim strCountry As String = "United States of America"
            Dim strCC As String = My.Settings.CountryDialingCode

            strReturn = CleanPhoneNumber(PhoneNumber)

            ' Need to add default country code
            If strReturn.IndexOf("+") < 0 Then
                ' No country code found - adding default
                strReturn = "+" & strCC & strReturn
            End If

            Return strReturn
        Catch ex As Exception
            Errorlogger(ex.Message & " " & strTrace, Err.Number, "Common:GetSkypePhoneNumber")
            Return String.Empty
        End Try

    End Function

    Private Shared Function CleanPhoneNumber(ByVal strPhoneNumber As String) As String
        Try
            ' Clean non-numerics
            Dim vTemp As String

            vTemp = Replace(strPhoneNumber, "(", "")
            vTemp = Replace(vTemp, ")", "")
            vTemp = Replace(vTemp, " ", "")
            vTemp = Replace(vTemp, "-", "")
            vTemp = Replace(vTemp, ".", "")

            Return vTemp

        Catch ex As Exception
            Return strPhoneNumber
        End Try
    End Function

    ''' <summary>
    ''' Returns an image of the Contact's business card.
    ''' </summary>
    ''' <param name="oContact">Outlook.ContactItem</param>
    ''' <returns>Drawing.Image</returns>
    ''' <remarks></remarks>
    Public Shared Function GetBusinessCardImage(ByVal oContact As Outlook.ContactItem) As Drawing.Image
        Try
            Dim iPic As Drawing.Image = Nothing
            Dim c As New Ceptara.SystemInterface

            Dim strFullPathName As String = GetApplicationDataPath() & "\Card.bmp"

            oContact.SaveBusinessCardImage(strFullPathName)

            Dim oFile As FileStream
            oFile = File.OpenRead(strFullPathName)
            iPic = Image.FromStream(oFile)
            oFile.Close()

            DeleteFile(strFullPathName)

            Return iPic

        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "clsMySkype:GetBusinessCardImage")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Handlers"

    Private Sub oSkype_ApplicationConnecting(ByVal pApp As SKYPE4COMLib.Application, _
                                            ByVal pUsers As SKYPE4COMLib.UserCollection) Handles oSkype.ApplicationConnecting
        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "clsMySkype:oSkype_ApplicationConnecting"
        Try
            strTrace = "ApplicationConnecting Event Fired."
            TraceLogger(strTrace, strRoutine)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub oSkype_AttachmentStatus(ByVal Status As SKYPE4COMLib.TAttachmentStatus) _
                                                        Handles oSkype.AttachmentStatus
        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "clsMySkype:oSkype_AttachmentStatus"
        Try

            strTrace = "Checking attachment status."
            ' Set property tracking variable
            _AttachmentStatus = oSkype.AttachmentStatus
            Select Case oSkype.AttachmentStatus
                Case SKYPE4COMLib.TAttachmentStatus.apiAttachSuccess
                    ' Set up Skype behaviors specific to this add-in here...
                    If My.Settings.OpenSkypeInSilentMode Then oSkype.SilentMode = True
                    strTrace = "Attachment successful."
                    ' strTrace = "Making default startup call to 'echo123'"
                    ' oSkype.PlaceCall("echo123")
                Case SKYPE4COMLib.TAttachmentStatus.apiAttachAvailable
                    strTrace = "Attempting to reattach."
                    oSkype.Attach(8, False)
                Case SKYPE4COMLib.TAttachmentStatus.apiAttachRefused

                Case SKYPE4COMLib.TAttachmentStatus.apiAttachNotAvailable
                Case SKYPE4COMLib.TAttachmentStatus.apiAttachPendingAuthorization
                Case SKYPE4COMLib.TAttachmentStatus.apiAttachUnknown
                    strTrace = "Attachment status is unknown."
                Case Else
                    strTrace = "Attachment status was not listed."
            End Select

            'If oSkype.AttachmentStatus = SKYPE4COMLib.TAttachmentStatus.apiAttachSuccess Then
            '    If My.Settings.OpenSkypeInSilentMode Then oSkype.SilentMode = True
            '    strTrace = "Making default startup call to 'echo123'"
            '    _AttachmentStatus = SKYPE4COMLib.TAttachmentStatus.apiAttachSuccess
            '    'oSkype.PlaceCall("echo123")
            'ElseIf oSkype.AttachmentStatus = SKYPE4COMLib.TAttachmentStatus.apiAttachAvailable Then
            '    strTrace = "Attempting to reattach."
            '    _AttachmentStatus = SKYPE4COMLib.TAttachmentStatus.apiAttachAvailable
            '    oSkype.Attach(8, False)
            'End If
        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(ex.Message & ". " & strTrace, Err.Number, strRoutine)
        End Try
    End Sub

    Private Sub oSkype_CallStatus(ByVal pCall As SKYPE4COMLib.ICall, _
                                  ByVal Status As SKYPE4COMLib.TCallStatus) Handles oSkype.CallStatus

        Dim strTrace As String = ""
        Dim strRoutine As String = "clsMySkype:oSkype_CallStatus"
        Try
            _CurrentCallStatus = oSkype.Convert.CallStatusToText(Status)

            Select Case pCall.Status
                Case SKYPE4COMLib.TCallStatus.clsRinging
                    If IsNothing(_CurrentCall) Then
                        If pCall.Type = SKYPE4COMLib.TCallType.cltIncomingP2P Or _
                                pCall.Type = SKYPE4COMLib.TCallType.cltIncomingPSTN Then
                            ' A new call is coming in
                            If pCall.TargetIdentity <> "" Then
                                strTrace = strTrace & ".  Call target identity: '" & pCall.TargetIdentity & "'."
                                TraceLogger(strTrace, strRoutine)
                            End If
                            strTrace = "Incoming call from " & pCall.PartnerHandle

                            ' Assign CurrentCall to incoming call.
                            _CurrentCall = pCall
                            'pCall.Answer()
                            RaiseEvent IncomingCall(pCall)

                        End If
                    Else
                        If _CurrentCall.Status = SKYPE4COMLib.TCallStatus.clsInProgress Or _
                            _CurrentCall.Status = SKYPE4COMLib.TCallStatus.clsLocalHold Or _
                            _CurrentCall.Status = SKYPE4COMLib.TCallStatus.clsOnHold Or _
                            _CurrentCall.Status = SKYPE4COMLib.TCallStatus.clsRouting Then
                            ' Call in progress and another coming in
                        Else
                            ' Current Call not in progress
                            If pCall.Id <> _CurrentCall.Id Then
                                ' Make sure not handling the same call
                                If pCall.Type = SKYPE4COMLib.TCallType.cltIncomingP2P Or _
                                        pCall.Type = SKYPE4COMLib.TCallType.cltIncomingPSTN Then
                                    ' A new call is coming in
                                    If pCall.TargetIdentity <> "" Then
                                        strTrace = strTrace & ".  Call target identity: '" & pCall.TargetIdentity & "'."
                                        TraceLogger(strTrace, strRoutine)
                                    End If
                                    strTrace = "Incoming call from " & pCall.PartnerHandle

                                    ' Assign CurrentCall to incoming call.
                                    _CurrentCall = pCall
                                    'pCall.Answer()
                                    RaiseEvent IncomingCall(pCall)

                                End If

                            End If
                        End If
                    End If

                Case SKYPE4COMLib.TCallStatus.clsCancelled
                    strTrace = "Call was cancelled."
                    _CurrentCall = Nothing
                Case SKYPE4COMLib.TCallStatus.clsInProgress
                    strTrace = "Call is in progress."
                Case SKYPE4COMLib.TCallStatus.clsLocalHold
                    strTrace = "Call put on hold."
                Case SKYPE4COMLib.TCallStatus.clsFinished
                    strTrace = "Call completed."
                    _CurrentCall = Nothing
                Case SKYPE4COMLib.TCallStatus.clsMissed
                    strTrace = "Missed call from " & pCall.PartnerHandle
                Case SKYPE4COMLib.TCallStatus.clsBusy
                    strTrace = "Busy signal encountered."
                Case Else
                    strTrace = "Call status: '" & oSkype.Convert.CallStatusToText(pCall.Status) & "'."
            End Select
            If strTrace.Length > 0 Then SyncLogger("SK_CallStatus", strTrace, strRoutine)

            ' Set property tracking field
            'strTrace = "Querying call status."
            'If Status = SKYPE4COMLib.TCallStatus.clsInProgress Then
            '    strTrace = "Failed to make a call."
            '    strTrace = strTrace & " " & pCall.PartnerHandle & " " & oSkype.Convert.CallStatusToText(pCall.Status)
            'End If
        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(ex.Message & " " & strTrace, Err.Number, strRoutine)
        End Try

    End Sub

    ''' <summary>
    ''' Handles Skype's Chat message Status event
    ''' </summary>
    ''' <param name="pMessage">SKYPE4COMLIB.ChatMessage</param>
    ''' <param name="Status">SKYPE4COMLIB.TChatMessageStatus</param>
    ''' <remarks></remarks>
    Private Sub oSkype_MessageStatus(ByVal pMessage As SKYPE4COMLib.ChatMessage, _
                                     ByVal Status As SKYPE4COMLib.TChatMessageStatus) Handles oSkype.MessageStatus
        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "clsMySkype:oSkype_MessageStatus"
        Try
            'Select Case pMessage.Status
            '    Case SKYPE4COMLib.TChatMessageStatus.cmsRead
            '    Case SKYPE4COMLib.TChatMessageStatus.cmsReceived
            '    Case SKYPE4COMLib.TChatMessageStatus.cmsSending
            '    Case SKYPE4COMLib.TChatMessageStatus.cmsSent
            '    Case SKYPE4COMLib.TChatMessageStatus.cmsUnknown
            'End Select

            strTrace = "Assign current chat to latest status updating chat."
            _currentChat = pMessage.Chat

            strTrace = "Check ActiveChats to make sure this chat is 'logged'."
            strTrace = "Activate appropriate Chat window."

            strTrace = "Raise the message status event."
            RaiseEvent ChatMessageStatus(pMessage, Status)

        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(strTrace, ex, Err, strRoutine)
        End Try
    End Sub

    Private Sub oSkype_SmsMessageStatusChanged(ByVal pMessage As SKYPE4COMLib.SmsMessage, _
                                               ByVal Status As SKYPE4COMLib.TSmsMessageStatus) _
                                                                Handles oSkype.SmsMessageStatusChanged
        Dim strTrace As String = ""
        Dim strRoutine As String = "clsMySkype:oSkype_SmsMessageStatusChanged"
        Try
            strTrace = "Message status: '" & oSkype.Convert.SmsMessageStatusToText(pMessage.Status) & "'."
            SyncLogger("SK_SMSStatus", strTrace, strRoutine)
        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(ex.Message & " " & strTrace, Err.Number, strRoutine)
        End Try
    End Sub

    Private Sub oSkype_AsychSearchUsersFinished(ByVal cookie As Integer, _
                                        ByVal pUsers As SKYPE4COMLib.UserCollection) Handles oSkype.AsyncSearchUsersFinished
        Try
            _UserSearchInProgress = False
            _SearchUserCollection = pUsers

        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "MySkype:oSkype_AsynchSearchUsersFinished")
        End Try
    End Sub

#End Region

#Region "Supporting Methods"

#End Region

End Class

