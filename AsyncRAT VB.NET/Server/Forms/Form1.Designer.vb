<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.LV1 = New System.Windows.Forms.ListView()
        Me._IP = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._ID = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._Username = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._OS = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._VER = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ClientMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DownloadAndExecuteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoteDesktopToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CLIENTToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CLOSEToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UPDATEToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Timer_Ping = New System.Windows.Forms.Timer(Me.components)
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Timer_Status = New System.Windows.Forms.Timer(Me.components)
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClientMenu.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'LV1
        '
        Me.LV1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._IP, Me._ID, Me._Username, Me._OS, Me._VER})
        Me.LV1.ContextMenuStrip = Me.ClientMenu
        Me.LV1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LV1.FullRowSelect = True
        Me.LV1.GridLines = True
        Me.LV1.Location = New System.Drawing.Point(0, 0)
        Me.LV1.Name = "LV1"
        Me.LV1.Size = New System.Drawing.Size(943, 340)
        Me.LV1.TabIndex = 0
        Me.LV1.UseCompatibleStateImageBehavior = False
        Me.LV1.View = System.Windows.Forms.View.Details
        '
        '_IP
        '
        Me._IP.Text = "IP"
        Me._IP.Width = 147
        '
        '_ID
        '
        Me._ID.Text = "ID"
        Me._ID.Width = 154
        '
        '_Username
        '
        Me._Username.Text = "Username"
        Me._Username.Width = 159
        '
        '_OS
        '
        Me._OS.Text = "Operating System"
        Me._OS.Width = 308
        '
        '_VER
        '
        Me._VER.Text = "Version"
        Me._VER.Width = 148
        '
        'ClientMenu
        '
        Me.ClientMenu.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ClientMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DownloadAndExecuteToolStripMenuItem, Me.RemoteDesktopToolStripMenuItem, Me.CLIENTToolStripMenuItem, Me.ToolStripSeparator1, Me.AboutToolStripMenuItem})
        Me.ClientMenu.Name = "ClientMenu"
        Me.ClientMenu.Size = New System.Drawing.Size(241, 147)
        '
        'DownloadAndExecuteToolStripMenuItem
        '
        Me.DownloadAndExecuteToolStripMenuItem.Name = "DownloadAndExecuteToolStripMenuItem"
        Me.DownloadAndExecuteToolStripMenuItem.Size = New System.Drawing.Size(240, 26)
        Me.DownloadAndExecuteToolStripMenuItem.Text = "UPLOAD FILE"
        '
        'RemoteDesktopToolStripMenuItem
        '
        Me.RemoteDesktopToolStripMenuItem.Name = "RemoteDesktopToolStripMenuItem"
        Me.RemoteDesktopToolStripMenuItem.Size = New System.Drawing.Size(240, 26)
        Me.RemoteDesktopToolStripMenuItem.Text = "REMOTE DESKTOP"
        '
        'CLIENTToolStripMenuItem
        '
        Me.CLIENTToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CLOSEToolStripMenuItem, Me.UPDATEToolStripMenuItem})
        Me.CLIENTToolStripMenuItem.Name = "CLIENTToolStripMenuItem"
        Me.CLIENTToolStripMenuItem.Size = New System.Drawing.Size(240, 26)
        Me.CLIENTToolStripMenuItem.Text = "CLIENT"
        '
        'CLOSEToolStripMenuItem
        '
        Me.CLOSEToolStripMenuItem.Name = "CLOSEToolStripMenuItem"
        Me.CLOSEToolStripMenuItem.Size = New System.Drawing.Size(155, 30)
        Me.CLOSEToolStripMenuItem.Text = "CLOSE"
        '
        'UPDATEToolStripMenuItem
        '
        Me.UPDATEToolStripMenuItem.Name = "UPDATEToolStripMenuItem"
        Me.UPDATEToolStripMenuItem.Size = New System.Drawing.Size(155, 30)
        Me.UPDATEToolStripMenuItem.Text = "UPDATE"
        '
        'Timer_Ping
        '
        Me.Timer_Ping.Enabled = True
        Me.Timer_Ping.Interval = 30000
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 314)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(943, 26)
        Me.StatusStrip1.TabIndex = 1
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(20, 21)
        Me.ToolStripStatusLabel1.Text = ".."
        '
        'Timer_Status
        '
        Me.Timer_Status.Enabled = True
        Me.Timer_Status.Interval = 1000
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(237, 6)
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(240, 26)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(943, 340)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.LV1)
        Me.MinimumSize = New System.Drawing.Size(953, 378)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "AsyncRAT  // NYAN CAT"
        Me.ClientMenu.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LV1 As ListView
    Friend WithEvents _IP As ColumnHeader
    Friend WithEvents _Username As ColumnHeader
    Friend WithEvents _OS As ColumnHeader
    Friend WithEvents ClientMenu As ContextMenuStrip
    Friend WithEvents DownloadAndExecuteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Timer_Ping As Timer
    Friend WithEvents RemoteDesktopToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents Timer_Status As Timer
    Friend WithEvents _VER As ColumnHeader
    Friend WithEvents _ID As ColumnHeader
    Friend WithEvents CLIENTToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CLOSEToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UPDATEToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
End Class
