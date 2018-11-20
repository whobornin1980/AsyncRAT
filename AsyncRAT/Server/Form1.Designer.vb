<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.LV1 = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ClientMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CloseClientToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DownloadAndExecuteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Timer_Ping = New System.Windows.Forms.Timer(Me.components)
        Me.ClientMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'LV1
        '
        Me.LV1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.LV1.ContextMenuStrip = Me.ClientMenu
        Me.LV1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LV1.FullRowSelect = True
        Me.LV1.GridLines = True
        Me.LV1.Location = New System.Drawing.Point(0, 0)
        Me.LV1.Name = "LV1"
        Me.LV1.Size = New System.Drawing.Size(931, 314)
        Me.LV1.TabIndex = 0
        Me.LV1.UseCompatibleStateImageBehavior = False
        Me.LV1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "IP"
        Me.ColumnHeader1.Width = 197
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Username"
        Me.ColumnHeader2.Width = 182
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Operating System"
        Me.ColumnHeader3.Width = 308
        '
        'ClientMenu
        '
        Me.ClientMenu.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ClientMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CloseClientToolStripMenuItem, Me.DownloadAndExecuteToolStripMenuItem})
        Me.ClientMenu.Name = "ClientMenu"
        Me.ClientMenu.Size = New System.Drawing.Size(282, 64)
        '
        'CloseClientToolStripMenuItem
        '
        Me.CloseClientToolStripMenuItem.Name = "CloseClientToolStripMenuItem"
        Me.CloseClientToolStripMenuItem.Size = New System.Drawing.Size(281, 30)
        Me.CloseClientToolStripMenuItem.Text = "Close Client"
        '
        'DownloadAndExecuteToolStripMenuItem
        '
        Me.DownloadAndExecuteToolStripMenuItem.Name = "DownloadAndExecuteToolStripMenuItem"
        Me.DownloadAndExecuteToolStripMenuItem.Size = New System.Drawing.Size(281, 30)
        Me.DownloadAndExecuteToolStripMenuItem.Text = "Download And Execute"
        '
        'Timer_Ping
        '
        Me.Timer_Ping.Enabled = True
        Me.Timer_Ping.Interval = 5000
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(931, 314)
        Me.Controls.Add(Me.LV1)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "AsyncRAT  // NYAN CAT"
        Me.ClientMenu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LV1 As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents ClientMenu As ContextMenuStrip
    Friend WithEvents CloseClientToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DownloadAndExecuteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Timer_Ping As Timer
End Class
