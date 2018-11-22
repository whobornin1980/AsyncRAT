Imports System.ComponentModel

Public Class RemoteDesktop
    Public F As Form1
    Public C As Client

    Private Sub RemoteDesktop_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Button1.Text = "OFF"
    End Sub

    Private Sub RemoteDesktop_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

    End Sub

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If Button1.Text = "OFF" Then
                Button1.Text = "Capturing..."

                Dim M As New IO.MemoryStream
                Dim S As Byte() = SB("RD+" + Settings.SPL + PictureBox1.Width.ToString + Settings.SPL + PictureBox1.Height.ToString)
                Await M.WriteAsync(S, 0, S.Length)
                Await M.WriteAsync(SB(Settings.EOF), 0, Settings.EOF.Length)

                Dim x As Client = CType(C, Client)
                Try
                    x.C.BeginSend(M.ToArray, 0, M.Length, Net.Sockets.SocketFlags.None, New AsyncCallback(AddressOf x.EndSend), C)
                Catch ex As Exception
                    x.isDisconnected()
                End Try

                Try
                    Await M.FlushAsync()
                    M.Dispose()
                    S = Nothing
                Catch ex As Exception
                End Try
            Else
                Button1.Text = "OFF"
            End If

        Catch ex As Exception
        End Try
    End Sub
End Class