Public Class frmCallManager

#Region "Fields"

    Dim _Contact As Outlook.ContactItem

    Dim _PhoneList As New Dictionary(Of String, String)
    Dim _Muted As Boolean = False

    Friend WithEvents _DPad As dlgDialPad = Nothing

#End Region

#Region "Properties"

    Public Property oContact() As Outlook.ContactItem
        Get
            Return _Contact
        End Get
        Set(ByVal value As Outlook.ContactItem)
            _Contact = value
        End Set
    End Property

#End Region

#Region "Methods"

    Public Sub StartSession()
        StartSessionTimer()
    End Sub

#End Region

#Region "Events"

    Private Sub frmCallManager_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim strMsg As String = String.Empty

            ' Stop if no contact specified
            If IsNothing(oContact) Then
                Me.Close()
            End If

            If IsNothing(sdSkype) Then
                Errorlogger("Error intializing Skype object class, ignoring request.", -99, "frmCallManager:Load")

                strMsg = "Unable to create a Skype class instance.  Check to make sure Skype " & _
                        "has been installed.  Download the application from " & _
                        "Skype's website, www.skype.com.  Installing the latest version will load " & _
                        "all necessary objects."
                MsgBox(strMsg, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, _
                       My.Application.Info.ProductName & " - No Skype Class")
                Me.Close()
            End If

            ' Check to make sure a connection to the Skype4COM library has been established
            If Not sdSkype.IsConnected Then

                strMsg = "Unable to connect to the Skype4COM library."
                ErrorLogger(strMsg, -99, "frmCallManager:Load")

                strMsg = strMsg & "  Check to make sure Skype " & _
                        "has been installed.  Download the application from " & _
                        "Skype's website, www.skype.com.  Installing the latest version will load " & _
                        "all necessary objects."

                MsgBox(strMsg, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, _
                       My.Application.Info.ProductName & " - No Skype Object")
                Me.Close()
            End If

            ' Evaluate Skype's connection status
            If sdSkype.AttachmentStatus <> SKYPE4COMLib.TAttachmentStatus.apiAttachSuccess Then
                If sdSkype.AttachmentStatus = SKYPE4COMLib.TAttachmentStatus.apiAttachRefused Then
                    ' Check to make sure Outlook is allowed to connect to Skype message
                    strMsg = "Connection to Skype was refused - Outlook not allowed to access."
                    ErrorLogger(strMsg, -99, "frmCallManager:Load")

                    strMsg = "Connection to Skype was refused, check to make sure Outlook is allowed to access " & _
                        "Skype." & vbCrLf & vbCrLf & "Open Skype, select Tools - Options, Advanced Settings tab.  Click on " & _
                        "'Manage other programs access to Skype' and assure OUTLOOK.EXE is 'Allowed'."
                Else
                    ' Skype is unavailable to connect to - not sure why
                    strMsg = "Connection to Skype was not established, AttachmentStatus: '" & _
                        sdSkype.AttachmentStatus.ToString & "'."
                    ErrorLogger(strMsg, -99, "frmCallManager:Load")

                    strMsg = "A connection to Skype was not established.  Check to make sure the latest version " & _
                        "of Skype has been installed and Outlook is allowed to access it."
                End If
                MsgBox(strMsg, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, _
                       My.Application.Info.ProductName & " - No Connection to Skype")
                Me.Close()
            End If

            ' Set up the Picture Box
            Dim img As Drawing.Image = MySkype.GetBusinessCardImage(oContact)
            Me.pb_BusinessCard.Image = img

            ' Set up buttons
            SetupButtons()
            Me.tsbtn_Mute.Image = My.Resources.UnmutedCall
            Me.tsbtn_Mute.ToolTipText = "Press to Mute the Call"

            ConfigureAnswerButtons()

            ' Set Status Line
            If _PhoneList.Count = 0 Then
                Me.lbl_Status.Text = "No Phone Numbers Available for the Contact"
            Else
                Me.lbl_Status.Text = "Call Status: Ready..."
            End If

            Dim strCC As String = oContact.BusinessAddressCountry

            ' Find the users Skype handle if in Skype address book, Call via Skype Handle button
            Dim oUser As SKYPE4COMLib.User = sdSkype.GetUserFromContactItem(_Contact)
            If Not IsNothing(oUser) Then
                With Me.btn_SkypeCall
                    .Visible = True
                    .Tag = oUser.Handle
                End With
                Dim tt As New Windows.Forms.ToolTip
                tt.SetToolTip(Me.btn_SkypeCall, oUser.Handle)
            Else
                Me.btn_SkypeCall.Enabled = False
            End If

        Catch ex As Exception
            ErrorLogger(ex.Message, Err.Number, "frmCallManager:Load")
        End Try
    End Sub

    Private Sub tsbtn_Options_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtn_Options.Click
        Try
            Dim dlg As New dlgOptions
            dlg.Text = "Options - " & My.Application.Info.ProductName
            dlg.ShowDialog()
        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "frmCallManager:tsbtn_Options_Click")
        End Try
    End Sub

    Private Sub btn_Phone1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Phone1.Click
        Try
            Dim btn As Windows.Forms.Button = CType(sender, Windows.Forms.Button)

            Dim strContactPhoneNumber As String = MySkype.GetSkypePhoneNumber(_Contact, btn.Tag)

            Me.lbl_Status.Text = "Call Status: " & "Dialing " & strContactPhoneNumber
            sdSkype.MakeCallFromPhoneNumber(strContactPhoneNumber)
            StartSessionTimer()
            If Me.chkbx_StartJournalEntry.Checked Then StartJournalEntry()

        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "frmCallManager:btn_Phone1_Click")
        End Try
    End Sub

    Private Sub btn_Phone2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Phone2.Click
        Try
            Dim btn As Windows.Forms.Button = CType(sender, Windows.Forms.Button)

            Dim strContactPhoneNumber As String = MySkype.GetSkypePhoneNumber(_Contact, btn.Tag)

            Me.lbl_Status.Text = "Call Status: " & "Dialing " & strContactPhoneNumber
            sdSkype.MakeCallFromPhoneNumber(strContactPhoneNumber)
            StartSessionTimer()
            If Me.chkbx_StartJournalEntry.Checked Then StartJournalEntry()

        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "frmCallManager:btn_Phone2_Click")
        End Try
    End Sub

    Private Sub btn_Phone3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Phone3.Click
        Try
            Dim btn As Windows.Forms.Button = CType(sender, Windows.Forms.Button)

            Dim strContactPhoneNumber As String = MySkype.GetSkypePhoneNumber(_Contact, btn.Tag)

            Me.lbl_Status.Text = "Call Status: " & "Dialing " & strContactPhoneNumber
            sdSkype.MakeCallFromPhoneNumber(strContactPhoneNumber)
            StartSessionTimer()
            If Me.chkbx_StartJournalEntry.Checked Then StartJournalEntry()

        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "frmCallManager:btn_Phone3_Click")
        End Try
    End Sub

    Private Sub btn_Phone4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Phone4.Click
        Try
            Dim btn As Windows.Forms.Button = CType(sender, Windows.Forms.Button)

            Dim strContactPhoneNumber As String = MySkype.GetSkypePhoneNumber(_Contact, btn.Tag)

            Me.lbl_Status.Text = "Call Status: " & "Dialing " & strContactPhoneNumber
            sdSkype.MakeCallFromPhoneNumber(strContactPhoneNumber)
            StartSessionTimer()
            If Me.chkbx_StartJournalEntry.Checked Then StartJournalEntry()

        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "frmCallManager:btn_Phone4_Click")
        End Try
    End Sub

    Private Sub btn_SkypeCall_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SkypeCall.Click
        Try
            Dim btn As Windows.Forms.Button = CType(sender, Windows.Forms.Button)
            If Not IsNothing(btn) Then
                Me.lbl_Status.Text = "Call Status: " & "Dialing " & btn.Tag
                sdSkype.MakeCallFromUserHandle(btn.Tag)
                StartSessionTimer()
                If Me.chkbx_StartJournalEntry.Checked Then StartJournalEntry()
            End If
        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "frmCallManager:btn_SkypeCall_Click")
        End Try
    End Sub

    Private Sub btn_EndCall_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_EndCall.Click
        Try
            sdSkype.EndCurrentCall()
            'StopSessionTimer()
            'Me.lbl_Status.Text = "Call Status: " & "Finished"
        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "frmCallManager:btn_EndCall_Click")
        End Try
    End Sub

    Private Sub btn_Answer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Answer.Click
        Try
            sdSkype.AnswerCurrentCall()
        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "frmCallManager:btn_Answer_Click")
        End Try
    End Sub

    Private Sub tsbtn_StartJournaling_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtn_StartJournaling.Click
        Try
            StartJournalEntry()
        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "frmCallManager:tsbtn_StartJournaling_Click")
        End Try
    End Sub

    Private Sub tsbtn_SendDTMFCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtn_SendDTMFCode.Click
        Try
            Dim strStatus As String = sdSkype.CurrentCallStatus
            If strStatus.ToUpper.IndexOf("PROGRESS") >= 0 Then
                Dim strInput As String = InputBox("Conference Code:", _
                                        My.Application.Info.ProductName & " - Enter Code", "")
                If strInput.Length <> 0 Then
                    sdSkype.SendDTMFString(strInput)
                End If
            Else
                MsgBox("No call in progress.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, _
                       My.Application.Info.ProductName & " - Call Error")
            End If
        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "frmCallManager:tsbtn_SendDTMFCode_Click")
        End Try
    End Sub

    Private Sub tsbtn_LaunchDialPad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtn_LaunchDialPad.Click
        Try
            Dim cPoint As Drawing.Point = Me.Location

            If Not IsNothing(_DPad) Then
                ' Window exists
                _DPad.TopMost = True
            Else
                ' Need to create the dial pad
                _DPad = New dlgDialPad

                _DPad.Text = "Dial Pad - " & My.Application.Info.ProductName
                _DPad.StartPosition = Windows.Forms.FormStartPosition.Manual
                _DPad.Location = New Drawing.Point(cPoint.X + Me.Width + 5, cPoint.Y)

                _DPad.Show()
            End If
        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "frmCallManager:tsbtn_LaunchDialPad_Click")
        End Try
    End Sub

    Private Sub tsbtn_Mute_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsbtn_Mute.Click
        Try
            Dim strStatus As String = sdSkype.CurrentCallStatus
            If strStatus.ToUpper.IndexOf("PROGRESS") >= 0 Then
                If _Muted Then
                    ' Tracking variable
                    _Muted = False
                    ' Unmute the call
                    sdSkype.UnMuteCurrentCall()
                    ' Update UI
                    Me.tsbtn_Mute.ToolTipText = "Press to Mute the Call"
                    Me.tsbtn_Mute.Image = My.Resources.UnmutedCall
                Else
                    ' Tracking variable
                    _Muted = True
                    ' Mute the call
                    sdSkype.MuteCurrentCall()
                    ' Update UI
                    Me.tsbtn_Mute.ToolTipText = "Press to Unmute the Call"
                    Me.tsbtn_Mute.Image = My.Resources.MutedCall
                End If
            Else
                MsgBox("No call in progress.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, _
                       My.Application.Info.ProductName & " - Call Error")
            End If

        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "frmCallManager:tsbtn_Mute_Click")
        End Try
    End Sub

    Private Sub tsbtn_LaunchSkype_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtn_LaunchSkype.Click
        Try
            sdSkype.LaunchSkypeDesktopApplication()
        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "frmCallManager:tsbtn_LaunchSkype_Click")
        End Try
    End Sub

    Private Sub frmCallManager_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            StopSessionTimer() '
            If Not IsNothing(_DPad) Then _DPad.Close()
        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "frmCallManager:FormClosing")
        End Try
    End Sub

    Private Sub _DPad_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles _DPad.FormClosed
        Try
            _DPad = Nothing
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Supporting Methods"

    Private Sub GetPhoneNumbers()
        Try
            _PhoneList.Clear()

            If Not IsNothing(oContact.BusinessTelephoneNumber) Then
                If oContact.BusinessTelephoneNumber.Length > 0 Then
                    _PhoneList.Add("Office", oContact.BusinessTelephoneNumber)
                End If
            End If
            If Not IsNothing(oContact.Business2TelephoneNumber) Then
                If oContact.Business2TelephoneNumber.Length > 0 Then
                    _PhoneList.Add("Office 2", oContact.Business2TelephoneNumber)
                End If
            End If
            If Not IsNothing(oContact.MobileTelephoneNumber) Then
                If oContact.MobileTelephoneNumber.Length > 0 Then
                    _PhoneList.Add("Mobile", oContact.MobileTelephoneNumber)
                End If
            End If
            If Not IsNothing(oContact.HomeTelephoneNumber) Then
                If oContact.HomeTelephoneNumber.Length > 0 Then
                    _PhoneList.Add("Home", oContact.HomeTelephoneNumber)
                End If
            End If
            If Not IsNothing(oContact.Home2TelephoneNumber) Then
                If oContact.Home2TelephoneNumber.Length > 0 Then
                    _PhoneList.Add("Home 2", oContact.Home2TelephoneNumber)
                End If
            End If
        Catch ex As Exception
            ErrorLogger(ex.Message, Err.Number, "frmCallManager:GetPhoneNumbers")
        End Try
    End Sub

    Private Sub SetupButtons()
        Try
            GetPhoneNumbers()

            'Me.btn_Phone1.Visible = False
            'Me.btn_Phone2.Visible = False
            'Me.btn_Phone3.Visible = False
            'Me.btn_Phone4.Visible = False

            Me.btn_Phone1.Enabled = False
            Me.btn_Phone2.Enabled = False
            Me.btn_Phone3.Enabled = False
            Me.btn_Phone4.Enabled = False

            If _PhoneList.Count > 0 Then
                Dim i As Integer = 1
                Dim dict As KeyValuePair(Of String, String)
                For Each dict In _PhoneList
                    Select Case i
                        Case 1
                            With Me.btn_Phone1
                                .Visible = True
                                .Enabled = True
                                .Text = dict.Key
                                .Tag = dict.Value
                            End With
                            Dim tt As New Windows.Forms.ToolTip
                            tt.SetToolTip(Me.btn_Phone1, dict.Value)
                        Case 2
                            With Me.btn_Phone2
                                .Visible = True
                                .Enabled = True
                                .Text = dict.Key
                                .Tag = dict.Value
                            End With
                            Dim tt As New Windows.Forms.ToolTip
                            tt.SetToolTip(Me.btn_Phone2, dict.Value)
                        Case 3
                            With Me.btn_Phone3
                                .Visible = True
                                .Enabled = True
                                .Text = dict.Key
                                .Tag = dict.Value
                            End With
                            Dim tt As New Windows.Forms.ToolTip
                            tt.SetToolTip(Me.btn_Phone3, dict.Value)
                        Case 4
                            With Me.btn_Phone4
                                .Visible = True
                                .Enabled = True
                                .Text = dict.Key
                                .Tag = dict.Value
                            End With
                            Dim tt As New Windows.Forms.ToolTip
                            tt.SetToolTip(Me.btn_Phone4, dict.Value)

                    End Select
                    i += 1
                Next
            Else
                ' Warn and show form
            End If

        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "frmCallManager:SetupButtons")
        End Try
    End Sub

    Private Sub ConfigureAnswerButtons()
        Try
            ' Setup Answer / Ignore buttons

            Dim strStatus As String = sdSkype.CurrentCallStatus
            If strStatus.ToUpper.IndexOf("CALLING") >= 0 Then
                Me.btn_Answer.Visible = True
                Me.btn_EndCall.Location = New Drawing.Point(328, 143)
                Me.btn_EndCall.Size = New Drawing.Size(52, 35)
                Me.btn_EndCall.Text = "Ignore"
            Else
                Me.btn_Answer.Visible = False
                Me.btn_EndCall.Location = New Drawing.Point(273, 143)
                Me.btn_EndCall.Size = New Drawing.Size(108, 35)
                Me.btn_EndCall.Text = "End Call"
            End If

        Catch ex As Exception
            Errorlogger(ex.Message, Err.Number, "frmCallManager:ConfigureAnswerButtons")
        End Try
    End Sub

    Private Sub StartJournalEntry()
        Try
            Dim olApp As Outlook.Application = SkypeDialer2.Globals.ThisAddIn.Application
            
            Dim oJournal As Outlook.JournalItem = olApp.CreateItem(Outlook.OlItemType.olJournalItem)

            oJournal.Subject = "Call with " & _Contact.FullName
            oJournal.Categories = _Contact.Categories
            oJournal.Type = "Phone call"
            oJournal.StartTimer()

            oJournal.Display()

            olApp = Nothing
        Catch ex As Exception
            ErrorLogger(ex.Message, Err.Number, "frmCallManager:StartJournalEntry")
        End Try
    End Sub

#End Region

#Region "Timer Methods"

    Dim dtTimerStart As Date        ' Timer start datetime

    Dim lTimerInterval As Long = 2 ' in seconds
    Dim lElapsedTime As Long = 0
    Dim dblTimerBias As Double = 0  ' in seconds - allows starting and stopping and the ability
    '                                   to start the timer at a non-zero value

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
            ErrorLogger(ex.Message, Err.Number, "frmCallManager:StartSessionTimer")
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
            ErrorLogger(ex.Message, Err.Number, "frmCallManager:StopSessionTimer")
        End Try

    End Sub

    Private Sub myTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles myTimer.Tick
        Try
            lElapsedTime = DateDiff(DateInterval.Second, dtTimerStart, Now) + dblTimerBias
            If lElapsedTime Mod lTimerInterval = 0 Then
                'SetSessionTimerText(lElapsedTime)
                ' Get current call status
                Dim strStatus As String = sdSkype.CurrentCallStatus
                ' Check muting
                Dim strMuted As String = String.Empty
                If _Muted Then strMuted = " - Call is muted."
                ' Update UI
                Me.lbl_Status.Text = "Call Status: " & strStatus & strMuted
                ' If done clean up timer and close dialog (if appropriate)
                If strStatus.ToUpper.IndexOf("FIN") >= 0 Then
                    StopSessionTimer()
                    If My.Settings.CloseCallManagerOnHangUp Then
                        Me.Close()
                    End If
                End If

                ConfigureAnswerButtons()

            End If

        Catch ex As Exception
            ErrorLogger(ex.Message, Err.Number, "frmCallManager:myTimer_Tick")
        End Try
    End Sub

    Private Sub myTimer_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles myTimer.Disposed
        'MsgBox(" Disposed")
    End Sub

#End Region


End Class