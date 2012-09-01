Imports System.Windows.Forms

Public Class dlgOptions

    Dim _bSaved As Boolean = True

    Private Sub dlgOptions_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.lbl_CCDescription.Text = "When a plus (+) sign is not included in the requested " & _
                "phone number, the default country code is inserted with a plus sign."

            Me.txtbx_DefaultCountryCode.Text = My.Settings.CountryDialingCode
            Me.chkbx_StartInSilentMode.Checked = My.Settings.OpenSkypeInSilentMode
            Me.chkbx_CloseOnHangUp.Checked = My.Settings.CloseCallManagerOnHangUp

            Me.btn_Apply.Enabled = False

        Catch ex As Exception
            ErrorLogger(ex.Message, Err.Number, "dlgOptions:Load")
        End Try
    End Sub

    Private Sub btn_Apply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Apply.Click
        Try
            Me.btn_Apply.Enabled = False
            SaveSkypeDialerSettings()
        Catch ex As Exception
            ErrorLogger(ex.Message, Err.Number, "dlgOptions:btn_Apply_Click")
        End Try
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            If Not _bSaved Then
                SaveSkypeDialerSettings()
            End If
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        Catch ex As Exception
            ErrorLogger(ex.Message, Err.Number, "dlgOptions:OK_Button_Click")
        End Try
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

#Region "Control Handlers"

    Private Sub txtbx_DefaultCountryCode_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtbx_DefaultCountryCode.LostFocus
        Try
            Dim strText As String = Me.txtbx_DefaultCountryCode.Text
            If Not IsNumeric(strText) Or strText.IndexOf("+") >= 0 Then
                ' Error dialog
                Dim strMsg As String = "The country code must be a numeric value, " & _
                    "please enter a simple integer such as 1, 44, 963, etc."
                MsgBox(strMsg, MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, _
                       My.Application.Info.ProductName & " - Default Country Code")
                ' Reset entry
                Me.txtbx_DefaultCountryCode.Text = My.Settings.CountryDialingCode
            End If
        Catch ex As Exception
            ErrorLogger(ex.Message, Err.Number, "dlgOptions:txtbx_DefaultCountryCode_LostFocus")
        End Try
    End Sub

    Private Sub txtbx_DefaultCountryCode_ModifiedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtbx_DefaultCountryCode.ModifiedChanged
        Try
            _bSaved = False
            Me.btn_Apply.Enabled = True
        Catch ex As Exception
            ErrorLogger(ex.Message, Err.Number, "dlgOptions:txtbx_DefaultCountryCode_ModifiedChanged")
        End Try
    End Sub

    Private Sub chkbx_StartInSilentMode_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkbx_StartInSilentMode.CheckStateChanged
        Try
            _bSaved = False
            Me.btn_Apply.Enabled = True
        Catch ex As Exception
            ErrorLogger(ex.Message, Err.Number, "dlgOptions:chkbx_StartInSilentMode_CheckStateChanged")
        End Try
    End Sub

#End Region

#Region "Supporting Methods"

    Private Sub SaveSkypeDialerSettings()
        Try
            My.Settings.OpenSkypeInSilentMode = Me.chkbx_StartInSilentMode.Checked
            My.Settings.CountryDialingCode = Me.txtbx_DefaultCountryCode.Text
            My.Settings.CloseCallManagerOnHangUp = Me.chkbx_CloseOnHangUp.Checked
            My.Settings.Save()
        Catch ex As Exception
            ErrorLogger(ex.Message, Err.Number, "dlgOptions:SaveSkypeDialerSettings")
        End Try
    End Sub

#End Region


End Class
