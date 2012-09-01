Public Class frmChatManager

#Region "Fields"

    Dim WithEvents cm_Skype As MySkype = sdSkype

    Dim WithEvents myTimer As New Windows.Forms.Timer

#End Region

#Region "Properties"

    ''' <summary>
    ''' Initial person targeted for the chat
    ''' </summary>
    ''' <value>Outlook.ContactItem</value>
    ''' <returns>Outlook.ContactItem</returns>
    ''' <remarks></remarks>
    Public Property Contact As Outlook.ContactItem
        Get
            Return _contact
        End Get
        Set(ByVal value As Outlook.ContactItem)
            _contact = value
        End Set
    End Property
    Dim _contact As Outlook.ContactItem = Nothing

    ''' <summary>
    ''' Pointer to the current Chat sessions
    ''' </summary>
    ''' <value></value>
    ''' <returns>SKYPE4COMLib.Chat</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ChatSession As SKYPE4COMLib.Chat
        Get
            Return _chatSession
        End Get
    End Property
    Dim _chatSession As SKYPE4COMLib.Chat = Nothing

    ''' <summary>
    ''' Message status of the last Chat Message received for this specific Chat
    ''' </summary>
    ''' <value></value>
    ''' <returns>SKYPE4COMLib.TChatMessageStatus</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MessageStatus As SKYPE4COMLib.TChatMessageStatus
        Get
            Return _messageStatus
        End Get
    End Property
    Dim _messageStatus As SKYPE4COMLib.TChatMessageStatus = Nothing

#End Region

#Region "Event Handlers"

    Private Sub frmChatManager_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            ' Stop the timer
            StopSessionTimer()
            ' Close the chat session.
            sdSkype.EndChat(_chatSession)
            cm_Skype = Nothing
        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmChatManager_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ' Set up the dialog buttons
            ConfigureButtons()
            ' Start the session timer
            StartSessionTimer()
            '
            Me.tsStatus.Text = "Chat status: Ready"

            If Not IsNothing(_contact) Then StartNewChatWith(_contact)

        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' Handles the Skype Chat Message Status change event
    ''' </summary>
    ''' <param name="Message"></param>
    ''' <param name="Status"></param>
    ''' <remarks></remarks>
    Private Sub cm_Skype_ChatMessageStatus(ByVal Message As SKYPE4COMLib.ChatMessage, _
                                           ByVal Status As SKYPE4COMLib.TChatMessageStatus) Handles cm_Skype.ChatMessageStatus

        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "frmChatManager:cm_Skype_ChatMessageStatus"
        Try
            If Not IsNothing(ChatSession) Then
                If ChatSession.Name.ToUpper = Message.ChatName.ToUpper Then
                    If Status = SKYPE4COMLib.TChatMessageStatus.cmsReceived Then
                        strTrace = "Add message status to the UI."
                        AppendMessageToChatBody(Message.FromDisplayName, Message.Body)
                    End If

                    _messageStatus = Status
                    'Dim strStatus As String = sdSkype.oSkype.Convert.ChatMessageStatusToText(Status)
                    'Me.tsStatus.Text = "Chat status: " & strStatus
                Else
                    strTrace = "Status for another active chat - not this one."
                End If
            Else
                Me.tsStatus.Text = ""
            End If
        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(strTrace, ex, Err, strRoutine)
        End Try
    End Sub

    ''' <summary>
    ''' Evaluates characters entered inthe UserMessage textbox and processes a CR if needed.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtbx_UserMessage_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtbx_UserMessage.KeyPress
        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "frmChatManager:txtbx_UserMessage_KeyPress"
        Try
            If e.KeyChar = Chr(13) Then
                e.Handled = True
                SendMessage()
            End If
        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(strTrace, ex, Err, strRoutine)
        End Try
    End Sub

    ''' <summary>
    ''' Handles the Send Button Click event
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btn_Send_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Send.Click
        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "frmChatManager:btn_Send_Click"
        Try
            SendMessage()
        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(strTrace, ex, Err, strRoutine)
        End Try
    End Sub

    ''' <summary>
    ''' Handles the Launch Skype application toolstrip button
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tsbtn_LaunchSkype_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtn_LaunchSkype.Click
        sdSkype.LaunchSkypeDesktopApplication()
    End Sub

#End Region

#Region "Supporting Methods"

    ''' <summary>
    ''' Send the contents of the text box to the chat stream.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SendMessage()
        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "frmChatManager:SendMessage"
        Try
            Dim strMsg As String = String.Empty
            If Not IsNothing(ChatSession) Then
                If Me.txtbx_UserMessage.Text.Length > 0 Then
                    strTrace = "Send the status message to Skype."
                    ChatSession.SendMessage(Me.txtbx_UserMessage.Text)
                    strTrace = "Update the UI."
                    AppendMessageToChatBody("Me", Me.txtbx_UserMessage.Text)
                    Me.txtbx_UserMessage.Text = String.Empty
                Else
                    strMsg = "Please type a message in the text box."
                    MsgBox(strMsg, vbInformation Or vbOKOnly, "Chat Message - " & My.Application.Info.ProductName)
                End If
            Else
                strMsg = "Please add at least one member to the chat before sending a message."
                MsgBox(strMsg, vbCritical Or vbOKOnly, "Chat Message - " & My.Application.Info.ProductName)
            End If
        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(strTrace, ex, Err, strRoutine)
        End Try
    End Sub

    ''' <summary>
    ''' Configure the buttons at load time
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ConfigureButtons()
        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "frmChatManager:ConfigureButtons"
        Try
            ' Journal button
            Me.tsbtn_JournalChat.Image = GetImageFromOfficeMSOName("Organizer")
            Me.tsbtn_JournalChat.ToolTipText = "Create Journal Item from chat comments."

            ' Add button
            Me.tsbtn_AddMember.Image = GetImageFromOfficeMSOName("DistributionListAddNewMember")
            Me.tsbtn_AddMember.ToolTipText = "Add a user to this chat session."

            ' Remove button
            Me.tsbtn_RemoveUser.Image = GetImageFromOfficeMSOName("DistributionListRemoveMember")
            Me.tsbtn_RemoveUser.ToolTipText = "Remove a user from this chat session."

            ' Skype Launcher
            Me.tsbtn_LaunchSkype.ToolTipText = "Launch the Skype Application."

            ' Options
            Me.tsbtn_Options.ToolTipText = "Configure program options."

        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(strTrace, ex, Err, strRoutine)
        End Try
    End Sub

    ''' <summary>
    ''' Initiate a new chat with the specified Outlook Contact
    ''' </summary>
    ''' <param name="Contact">Outlook.ContactItem</param>
    ''' <remarks></remarks>
    Private Sub StartNewChatWith(ByVal Contact As Outlook.ContactItem)
        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "frmChatManager:StartNewChatWith"
        Try
            If IsNothing(Contact) Then
                strTrace = "Contact instance was not specified."
                Throw New Exception("Unable to start a new chat.")
            End If

            Dim UserHandle As String = sdSkype.GetUserHandleFromContactItem(Contact)
            If String.IsNullOrEmpty(UserHandle) Then UserHandle = Contact.IMAddress

            If String.IsNullOrEmpty(UserHandle) Then
                Dim strTemp As String = InputBox("Please provide the user's Skype handle.", _
                                                 "User Skype Handle - " & My.Application.Info.ProductName)
                If strTemp.Length > 0 Then UserHandle = strTemp
            End If

            If Not String.IsNullOrEmpty(UserHandle) Then
                Dim sUser As SKYPE4COMLib.User = sdSkype.GetUserFromSkypeHandle(UserHandle)
                If IsNothing(sUser) Then
                    ' user not in the friend list
                End If

                Dim ChatX As SKYPE4COMLib.Chat = Nothing
                If sUser.OnlineStatus = SKYPE4COMLib.TOnlineStatus.olsOnline Then
                    ChatX = sdSkype.CreateChatWith(UserHandle)
                    _chatSession = ChatX
                Else
                    Dim strStatus As String = sdSkype.oSkype.Convert.OnlineStatusToText(sUser.OnlineStatus)
                    Dim strMsg As String = "The user's online status is '" & strStatus & "', " & _
                        "please contact the user via another method to set up a Chat session." & vbCrLf & vbCrLf & _
                        "Would you like to create a new chat session anyway?"

                    Dim dResult As MsgBoxResult = MsgBox(strMsg, MsgBoxStyle.Information Or MsgBoxStyle.YesNo, _
                                        "Chat Request - " & My.Application.Info.ProductName)
                    If dResult = MsgBoxResult.Yes Then
                        ChatX = sdSkype.CreateChatWith(UserHandle)
                        _chatSession = ChatX
                    Else
                        Me.Close()
                    End If
                End If
            Else
                ' No user handle provided
                Me.Close()
            End If

        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(strTrace, ex, Err, strRoutine)
        End Try
    End Sub

    ''' <summary>
    ''' Append a Message to the body of the Chat
    ''' </summary>
    ''' <param name="Author">String: Message Author</param>
    ''' <param name="Message">String: Message to Append</param>
    ''' <remarks></remarks>
    Private Sub AppendMessageToChatBody(ByVal Author As String, ByVal Message As String)
        Try
            Dim sb As New StringBuilder
            sb.Append(Me.txtbx_Body.Text)
            If Me.txtbx_Body.Text.Length > 0 Then
                sb.Append(vbCrLf & vbCrLf)
            End If
            sb.Append(Author & vbCrLf)
            sb.Append(Message)

            Me.txtbx_Body.Text = sb.ToString

            Me.txtbx_Body.SelectionStart = Me.txtbx_Body.Text.Length
            Me.txtbx_Body.ScrollToCaret()

        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Timer Methods"

    Dim dtTimerStart As Date        ' Timer start datetime

    Dim lTimerInterval As Long = 2 ' in seconds
    Dim lElapsedTime As Long = 0
    Dim dblTimerBias As Double = 0  ' in seconds - allows starting and stopping and the ability
    '                                   to start the timer at a non-zero value

    Dim lMessageTimeoutCounter As Long = 0 ' if a message persists too long, changes to Ready status, e.g. Sending
    Dim lMessageTimeout As Long = 2 ' in intervals, e.g. 2 intervals x 2 s/interval = 4 seconds - upper limit on persistent message

    Private Sub StartSessionTimer()
        Try
            ' Check to see if timer is already running
            If Not Me.myTimer.Enabled Then
                dtTimerStart = Now
                dblTimerBias = 0 ' DecodeSessionTimerText() * 3600
                If dblTimerBias < 1 Then dblTimerBias = 0

                ' Update UI
                'Me.ts_StatusLabel.Text = "Timer is running..."
                'SetSessionTimerText(dblTimerBias)

                Me.myTimer.Interval = 1000
                Me.myTimer.Start()
            End If

        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "frmChatManager:StartSessionTimer")
        End Try
    End Sub

    Private Sub StopSessionTimer()
        Try
            If Me.myTimer.Enabled Then
                Me.myTimer.Stop()

                lElapsedTime = DateDiff(DateInterval.Second, dtTimerStart, Now) + dblTimerBias
                'SetSessionTimerText(lElapsedTime)
            End If
        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "frmChatManager:StopSessionTimer")
        End Try

    End Sub

    Dim _lastMessageStatus As String = String.Empty
    Private Sub myTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles myTimer.Tick
        Try
            lElapsedTime = DateDiff(DateInterval.Second, dtTimerStart, Now) + dblTimerBias
            If lElapsedTime Mod lTimerInterval = 0 Then
                Dim strStatus As String = sdSkype.oSkype.Convert.ChatMessageStatusToText(_messageStatus)

                If _lastMessageStatus.Length = 0 Then
                    _lastMessageStatus = strStatus
                Else
                    If _lastMessageStatus.ToUpper = strStatus.ToUpper Then
                        ' Same status as last time through the Tick routine
                        lMessageTimeoutCounter += 1
                        If lMessageTimeoutCounter > lMessageTimeout Then
                            strStatus = "Ready..."
                        End If
                    Else
                        ' New status since last time, reset the timeout counter
                        lMessageTimeoutCounter = 0
                    End If
                End If
                Me.tsStatus.Text = "Chat status: " & strStatus
            End If

        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "frmChatManager:myTimer_Tick")
        End Try
    End Sub

    Private Sub myTimer_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles myTimer.Disposed
        'MsgBox(" Disposed")
    End Sub

#End Region

End Class