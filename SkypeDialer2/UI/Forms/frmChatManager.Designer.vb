<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChatManager
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmChatManager))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbtn_JournalChat = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbtn_AddMember = New System.Windows.Forms.ToolStripButton()
        Me.tsbtn_RemoveUser = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbtn_LaunchSkype = New System.Windows.Forms.ToolStripButton()
        Me.tsbtn_Options = New System.Windows.Forms.ToolStripButton()
        Me.pnl_UserPanel = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.tsStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.txtbx_UserMessage = New System.Windows.Forms.TextBox()
        Me.btn_Send = New System.Windows.Forms.Button()
        Me.txtbx_Body = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ToolStrip1.SuspendLayout()
        Me.pnl_UserPanel.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtn_JournalChat, Me.ToolStripSeparator1, Me.tsbtn_AddMember, Me.tsbtn_RemoveUser, Me.ToolStripSeparator2, Me.tsbtn_LaunchSkype, Me.tsbtn_Options})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(410, 25)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbtn_JournalChat
        '
        Me.tsbtn_JournalChat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtn_JournalChat.Image = CType(resources.GetObject("tsbtn_JournalChat.Image"), System.Drawing.Image)
        Me.tsbtn_JournalChat.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtn_JournalChat.Name = "tsbtn_JournalChat"
        Me.tsbtn_JournalChat.Size = New System.Drawing.Size(23, 22)
        Me.tsbtn_JournalChat.Text = "Journal"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'tsbtn_AddMember
        '
        Me.tsbtn_AddMember.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtn_AddMember.Image = CType(resources.GetObject("tsbtn_AddMember.Image"), System.Drawing.Image)
        Me.tsbtn_AddMember.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtn_AddMember.Name = "tsbtn_AddMember"
        Me.tsbtn_AddMember.Size = New System.Drawing.Size(23, 22)
        Me.tsbtn_AddMember.Text = "Add User"
        '
        'tsbtn_RemoveUser
        '
        Me.tsbtn_RemoveUser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtn_RemoveUser.Image = CType(resources.GetObject("tsbtn_RemoveUser.Image"), System.Drawing.Image)
        Me.tsbtn_RemoveUser.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtn_RemoveUser.Name = "tsbtn_RemoveUser"
        Me.tsbtn_RemoveUser.Size = New System.Drawing.Size(23, 22)
        Me.tsbtn_RemoveUser.Text = "Remove User"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'tsbtn_LaunchSkype
        '
        Me.tsbtn_LaunchSkype.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtn_LaunchSkype.Image = Global.SkypeDialer2.My.Resources.Resources.Skype24bit
        Me.tsbtn_LaunchSkype.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtn_LaunchSkype.Name = "tsbtn_LaunchSkype"
        Me.tsbtn_LaunchSkype.Size = New System.Drawing.Size(23, 22)
        Me.tsbtn_LaunchSkype.Text = "Launch Skype"
        '
        'tsbtn_Options
        '
        Me.tsbtn_Options.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtn_Options.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtn_Options.Image = Global.SkypeDialer2.My.Resources.Resources.Options24bit
        Me.tsbtn_Options.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtn_Options.Name = "tsbtn_Options"
        Me.tsbtn_Options.Size = New System.Drawing.Size(23, 22)
        Me.tsbtn_Options.Text = "ToolStripButton1"
        '
        'pnl_UserPanel
        '
        Me.pnl_UserPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnl_UserPanel.Controls.Add(Me.Label1)
        Me.pnl_UserPanel.Location = New System.Drawing.Point(0, 24)
        Me.pnl_UserPanel.Name = "pnl_UserPanel"
        Me.pnl_UserPanel.Size = New System.Drawing.Size(410, 60)
        Me.pnl_UserPanel.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(111, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "User Photos Go Here!"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsStatus})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 430)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(410, 22)
        Me.StatusStrip1.TabIndex = 3
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'tsStatus
        '
        Me.tsStatus.Name = "tsStatus"
        Me.tsStatus.Size = New System.Drawing.Size(70, 17)
        Me.tsStatus.Text = "Chat Status:"
        '
        'txtbx_UserMessage
        '
        Me.txtbx_UserMessage.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbx_UserMessage.Location = New System.Drawing.Point(12, 352)
        Me.txtbx_UserMessage.Multiline = True
        Me.txtbx_UserMessage.Name = "txtbx_UserMessage"
        Me.txtbx_UserMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtbx_UserMessage.Size = New System.Drawing.Size(306, 65)
        Me.txtbx_UserMessage.TabIndex = 4
        '
        'btn_Send
        '
        Me.btn_Send.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_Send.Location = New System.Drawing.Point(329, 352)
        Me.btn_Send.Name = "btn_Send"
        Me.btn_Send.Size = New System.Drawing.Size(69, 65)
        Me.btn_Send.TabIndex = 5
        Me.btn_Send.Text = "Send Message"
        Me.btn_Send.UseVisualStyleBackColor = True
        '
        'txtbx_Body
        '
        Me.txtbx_Body.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbx_Body.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.txtbx_Body.Location = New System.Drawing.Point(12, 109)
        Me.txtbx_Body.Multiline = True
        Me.txtbx_Body.Name = "txtbx_Body"
        Me.txtbx_Body.ReadOnly = True
        Me.txtbx_Body.Size = New System.Drawing.Size(386, 218)
        Me.txtbx_Body.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 90)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(50, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Chat Log"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(16, 333)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(50, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Message"
        '
        'frmChatManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(410, 452)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtbx_Body)
        Me.Controls.Add(Me.btn_Send)
        Me.Controls.Add(Me.txtbx_UserMessage)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.pnl_UserPanel)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmChatManager"
        Me.Text = "frmChatManager"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.pnl_UserPanel.ResumeLayout(False)
        Me.pnl_UserPanel.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbtn_JournalChat As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbtn_AddMember As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtn_RemoveUser As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbtn_LaunchSkype As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtn_Options As System.Windows.Forms.ToolStripButton
    Friend WithEvents pnl_UserPanel As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents tsStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents txtbx_UserMessage As System.Windows.Forms.TextBox
    Friend WithEvents btn_Send As System.Windows.Forms.Button
    Friend WithEvents txtbx_Body As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
