Imports System.Windows.Forms

Public Class dlgDialPad

    'Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Me.DialogResult = System.Windows.Forms.DialogResult.OK
    '    Me.Close()
    'End Sub

    'Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    '    Me.Close()
    'End Sub

#Region "Methods"

    Private Sub dlgDialPad_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.txtbx_DialString.ReadOnly = True
        Catch ex As Exception
            ErrorLogger(ex.Message, Err.Number, "dlgDialPad:Load")
        End Try
    End Sub

#Region "Dial 1"
    ' 1
    Private Sub btn_Dial1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Dial1.Click
        DialCode("1")
        Me.txtbx_DialString.Focus() ' so key presses work after button presses
    End Sub
    'Private Sub btn_Dial1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles btn_Dial1.KeyPress
    '    Try
    '        If e.KeyChar = "1" Then
    '            DialCode("1")
    '        End If
    '        Me.txtbx_DialString.Focus()
    '    Catch ex As Exception
    '        ErrorLogger(ex.Message, Err.Number, "dlgDialPad:btn_Dial1_KeyPress")
    '    End Try
    'End Sub
#End Region

    ' 2
    Private Sub btn_Dial2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Dial2.Click
        'sdSkype.SendWAVFile("")

        DialCode("2")
        Me.txtbx_DialString.Focus() ' so key presses work after button presses
    End Sub

    ' 3
    Private Sub btn_Dial3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Dial3.Click
        DialCode("3")
        Me.txtbx_DialString.Focus() ' so key presses work after button presses
    End Sub

    ' 4
    Private Sub btn_Dial4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Dial4.Click
        DialCode("4")
        Me.txtbx_DialString.Focus() ' so key presses work after button presses
    End Sub

    ' 5
    Private Sub btn_Dial5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Dial5.Click
        DialCode("5")
        Me.txtbx_DialString.Focus() ' so key presses work after button presses
    End Sub

    ' 6
    Private Sub btn_Dial6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Dial6.Click
        DialCode("6")
        Me.txtbx_DialString.Focus() ' so key presses work after button presses
    End Sub

    ' 7
    Private Sub btn_Dial7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Dial7.Click
        DialCode("7")
        Me.txtbx_DialString.Focus() ' so key presses work after button presses
    End Sub

    ' 8
    Private Sub btn_Dial8_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Dial8.Click
        DialCode("8")
        Me.txtbx_DialString.Focus() ' so key presses work after button presses
    End Sub

    ' 9
    Private Sub btn_Dial9_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Dial9.Click
        DialCode("9")
        Me.txtbx_DialString.Focus() ' so key presses work after button presses
    End Sub

    ' 0
    Private Sub btn_Dial0_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Dial0.Click
        DialCode("0")
        Me.txtbx_DialString.Focus() ' so key presses work after button presses
    End Sub

    ' #
    Private Sub btn_DialPound_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_DialPound.Click
        DialCode("#")
        Me.txtbx_DialString.Focus() ' so key presses work after button presses
    End Sub

    ' *
    Private Sub btn_DialStar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_DialStar.Click
        DialCode("*")
        Me.txtbx_DialString.Focus() ' so key presses work after button presses
    End Sub

    ' Text Box
    Private Sub txtbx_DialString_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtbx_DialString.KeyPress
        If "0123456789*#".IndexOf(e.KeyChar) >= 0 Then
            DialCode(e.KeyChar)
        End If
    End Sub

    ' Key Pad
    Private Sub dlgDialPad_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Me.txtbx_DialString.Focus()
    End Sub

    Private Sub dlgDialPad_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Me.txtbx_DialString.Focus()
    End Sub

#End Region

#Region "Supporting Methods"

    Private Sub DialCode(ByVal strDialCode As String)
        Try
            Dim strStatus As String = sdSkype.CurrentCallStatus
            If strStatus.ToUpper.IndexOf("PROGRESS") >= 0 Then
                ' Update Textbox
                Me.txtbx_DialString.Text = Me.txtbx_DialString.Text & strDialCode
                ' Dial Code
                sdSkype.SendDTMFString(strDialCode)
            Else
                MsgBox("No call in progress.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, _
                       My.Application.Info.ProductName & " - Dial Error")
                Me.txtbx_DialString.Text = String.Empty
            End If
        Catch ex As Exception
            ErrorLogger(ex.Message, Err.Number, "dlgDialPad:DialCode")
        End Try
    End Sub

#End Region


End Class
