<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCallManager
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCallManager))
        Me.ss_StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.lbl_Status = New System.Windows.Forms.ToolStripStatusLabel()
        Me.chkbx_StartJournalEntry = New System.Windows.Forms.CheckBox()
        Me.btn_Phone1 = New System.Windows.Forms.Button()
        Me.btn_EndCall = New System.Windows.Forms.Button()
        Me.btn_Phone2 = New System.Windows.Forms.Button()
        Me.btn_Phone3 = New System.Windows.Forms.Button()
        Me.btn_Phone4 = New System.Windows.Forms.Button()
        Me.myTimer = New System.Windows.Forms.Timer(Me.components)
        Me.btn_SkypeCall = New System.Windows.Forms.Button()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbtn_StartJournaling = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbtn_LaunchDialPad = New System.Windows.Forms.ToolStripButton()
        Me.tsbtn_SendDTMFCode = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbtn_Mute = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbtn_LaunchSkype = New System.Windows.Forms.ToolStripButton()
        Me.tsbtn_Options = New System.Windows.Forms.ToolStripButton()
        Me.pb_BusinessCard = New System.Windows.Forms.PictureBox()
        Me.btn_Answer = New System.Windows.Forms.Button()
        Me.ss_StatusStrip.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.pb_BusinessCard, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ss_StatusStrip
        '
        Me.ss_StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lbl_Status})
        Me.ss_StatusStrip.Location = New System.Drawing.Point(0, 211)
        Me.ss_StatusStrip.Name = "ss_StatusStrip"
        Me.ss_StatusStrip.Size = New System.Drawing.Size(392, 22)
        Me.ss_StatusStrip.TabIndex = 1
        Me.ss_StatusStrip.Text = "StatusStrip1"
        '
        'lbl_Status
        '
        Me.lbl_Status.Name = "lbl_Status"
        Me.lbl_Status.Size = New System.Drawing.Size(57, 17)
        Me.lbl_Status.Text = "lbl_Status"
        '
        'chkbx_StartJournalEntry
        '
        Me.chkbx_StartJournalEntry.AutoSize = True
        Me.chkbx_StartJournalEntry.Location = New System.Drawing.Point(12, 189)
        Me.chkbx_StartJournalEntry.Name = "chkbx_StartJournalEntry"
        Me.chkbx_StartJournalEntry.Size = New System.Drawing.Size(286, 17)
        Me.chkbx_StartJournalEntry.TabIndex = 2
        Me.chkbx_StartJournalEntry.Text = "Create new journal item and start timer for the new call.."
        Me.chkbx_StartJournalEntry.UseVisualStyleBackColor = True
        '
        'btn_Phone1
        '
        Me.btn_Phone1.Location = New System.Drawing.Point(273, 28)
        Me.btn_Phone1.Name = "btn_Phone1"
        Me.btn_Phone1.Size = New System.Drawing.Size(50, 35)
        Me.btn_Phone1.TabIndex = 3
        Me.btn_Phone1.Text = "Office"
        Me.btn_Phone1.UseVisualStyleBackColor = True
        '
        'btn_EndCall
        '
        Me.btn_EndCall.Location = New System.Drawing.Point(328, 143)
        Me.btn_EndCall.Name = "btn_EndCall"
        Me.btn_EndCall.Size = New System.Drawing.Size(52, 35)
        Me.btn_EndCall.TabIndex = 6
        Me.btn_EndCall.Text = "Ignore"
        Me.btn_EndCall.UseVisualStyleBackColor = True
        '
        'btn_Phone2
        '
        Me.btn_Phone2.Location = New System.Drawing.Point(331, 28)
        Me.btn_Phone2.Name = "btn_Phone2"
        Me.btn_Phone2.Size = New System.Drawing.Size(50, 35)
        Me.btn_Phone2.TabIndex = 7
        Me.btn_Phone2.Text = "Ph2"
        Me.btn_Phone2.UseVisualStyleBackColor = True
        '
        'btn_Phone3
        '
        Me.btn_Phone3.Location = New System.Drawing.Point(273, 69)
        Me.btn_Phone3.Name = "btn_Phone3"
        Me.btn_Phone3.Size = New System.Drawing.Size(50, 35)
        Me.btn_Phone3.TabIndex = 8
        Me.btn_Phone3.Text = "Ph3"
        Me.btn_Phone3.UseVisualStyleBackColor = True
        '
        'btn_Phone4
        '
        Me.btn_Phone4.Location = New System.Drawing.Point(330, 69)
        Me.btn_Phone4.Name = "btn_Phone4"
        Me.btn_Phone4.Size = New System.Drawing.Size(50, 35)
        Me.btn_Phone4.TabIndex = 9
        Me.btn_Phone4.Text = "Ph4"
        Me.btn_Phone4.UseVisualStyleBackColor = True
        '
        'myTimer
        '
        '
        'btn_SkypeCall
        '
        Me.btn_SkypeCall.Location = New System.Drawing.Point(273, 110)
        Me.btn_SkypeCall.Name = "btn_SkypeCall"
        Me.btn_SkypeCall.Size = New System.Drawing.Size(107, 27)
        Me.btn_SkypeCall.TabIndex = 10
        Me.btn_SkypeCall.Text = "Skype to Skype"
        Me.btn_SkypeCall.UseVisualStyleBackColor = True
        '
        'ToolStrip1
        '
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtn_StartJournaling, Me.ToolStripSeparator1, Me.tsbtn_LaunchDialPad, Me.tsbtn_SendDTMFCode, Me.ToolStripSeparator2, Me.tsbtn_Mute, Me.ToolStripSeparator3, Me.tsbtn_LaunchSkype, Me.tsbtn_Options})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(392, 25)
        Me.ToolStrip1.TabIndex = 11
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbtn_StartJournaling
        '
        Me.tsbtn_StartJournaling.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtn_StartJournaling.Image = CType(resources.GetObject("tsbtn_StartJournaling.Image"), System.Drawing.Image)
        Me.tsbtn_StartJournaling.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtn_StartJournaling.Name = "tsbtn_StartJournaling"
        Me.tsbtn_StartJournaling.Size = New System.Drawing.Size(23, 22)
        Me.tsbtn_StartJournaling.Text = "Create Journal Item"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'tsbtn_LaunchDialPad
        '
        Me.tsbtn_LaunchDialPad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtn_LaunchDialPad.Image = CType(resources.GetObject("tsbtn_LaunchDialPad.Image"), System.Drawing.Image)
        Me.tsbtn_LaunchDialPad.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtn_LaunchDialPad.Name = "tsbtn_LaunchDialPad"
        Me.tsbtn_LaunchDialPad.Size = New System.Drawing.Size(23, 22)
        Me.tsbtn_LaunchDialPad.Text = "Launch Dialpad"
        '
        'tsbtn_SendDTMFCode
        '
        Me.tsbtn_SendDTMFCode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtn_SendDTMFCode.Image = CType(resources.GetObject("tsbtn_SendDTMFCode.Image"), System.Drawing.Image)
        Me.tsbtn_SendDTMFCode.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtn_SendDTMFCode.Name = "tsbtn_SendDTMFCode"
        Me.tsbtn_SendDTMFCode.Size = New System.Drawing.Size(23, 22)
        Me.tsbtn_SendDTMFCode.Text = "Send Conference Call Code"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'tsbtn_Mute
        '
        Me.tsbtn_Mute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtn_Mute.Image = Global.SkypeDialer2.My.Resources.Resources.MutedCall
        Me.tsbtn_Mute.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtn_Mute.Name = "tsbtn_Mute"
        Me.tsbtn_Mute.Size = New System.Drawing.Size(23, 22)
        Me.tsbtn_Mute.Text = "Mute"
        Me.tsbtn_Mute.ToolTipText = "Mute"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'tsbtn_LaunchSkype
        '
        Me.tsbtn_LaunchSkype.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtn_LaunchSkype.Image = Global.SkypeDialer2.My.Resources.Resources.Skype24bit
        Me.tsbtn_LaunchSkype.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtn_LaunchSkype.Name = "tsbtn_LaunchSkype"
        Me.tsbtn_LaunchSkype.Size = New System.Drawing.Size(23, 22)
        Me.tsbtn_LaunchSkype.Text = "Launch Skype App"
        '
        'tsbtn_Options
        '
        Me.tsbtn_Options.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtn_Options.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtn_Options.Image = Global.SkypeDialer2.My.Resources.Resources.Options24bit
        Me.tsbtn_Options.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtn_Options.Name = "tsbtn_Options"
        Me.tsbtn_Options.Size = New System.Drawing.Size(23, 22)
        Me.tsbtn_Options.Text = "Options"
        '
        'pb_BusinessCard
        '
        Me.pb_BusinessCard.Location = New System.Drawing.Point(12, 28)
        Me.pb_BusinessCard.Name = "pb_BusinessCard"
        Me.pb_BusinessCard.Size = New System.Drawing.Size(250, 150)
        Me.pb_BusinessCard.TabIndex = 0
        Me.pb_BusinessCard.TabStop = False
        '
        'btn_Answer
        '
        Me.btn_Answer.Location = New System.Drawing.Point(273, 143)
        Me.btn_Answer.Name = "btn_Answer"
        Me.btn_Answer.Size = New System.Drawing.Size(52, 35)
        Me.btn_Answer.TabIndex = 12
        Me.btn_Answer.Text = "Answer"
        Me.btn_Answer.UseVisualStyleBackColor = True
        '
        'frmCallManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(392, 233)
        Me.Controls.Add(Me.btn_Answer)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.btn_SkypeCall)
        Me.Controls.Add(Me.btn_Phone4)
        Me.Controls.Add(Me.btn_Phone3)
        Me.Controls.Add(Me.btn_Phone2)
        Me.Controls.Add(Me.btn_EndCall)
        Me.Controls.Add(Me.btn_Phone1)
        Me.Controls.Add(Me.chkbx_StartJournalEntry)
        Me.Controls.Add(Me.ss_StatusStrip)
        Me.Controls.Add(Me.pb_BusinessCard)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmCallManager"
        Me.Text = "frmCallManager"
        Me.ss_StatusStrip.ResumeLayout(False)
        Me.ss_StatusStrip.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.pb_BusinessCard, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pb_BusinessCard As System.Windows.Forms.PictureBox
    Friend WithEvents ss_StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents chkbx_StartJournalEntry As System.Windows.Forms.CheckBox
    Friend WithEvents btn_Phone1 As System.Windows.Forms.Button
    Friend WithEvents btn_EndCall As System.Windows.Forms.Button
    Friend WithEvents btn_Phone2 As System.Windows.Forms.Button
    Friend WithEvents btn_Phone3 As System.Windows.Forms.Button
    Friend WithEvents btn_Phone4 As System.Windows.Forms.Button
    Friend WithEvents lbl_Status As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents myTimer As System.Windows.Forms.Timer
    Friend WithEvents btn_SkypeCall As System.Windows.Forms.Button
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbtn_StartJournaling As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbtn_LaunchDialPad As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtn_SendDTMFCode As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbtn_Mute As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtn_LaunchSkype As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbtn_Options As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_Answer As System.Windows.Forms.Button
End Class
