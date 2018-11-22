Imports System.IO
Imports System.Net.Sockets

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
        Me.MS = New IO.MemoryStream
        Me.IP = CL.RemoteEndPoint.ToString
    End Sub

    Async Sub BeginReceive(ByVal ar As IAsyncResult)
        If Me.IsConnected = False Then Me.isDisconnected()

        Try
            Dim Received As Integer = Me.C.EndReceive(ar)
            If Received > 0 Then
                Await Me.MS.WriteAsync(Me.Buffer, 0, Received)

                If BS(Me.MS.ToArray).Contains(Settings.EOF) Then
                    RaiseEvent Read(Me, Me.MS.ToArray)
                    Await Me.MS.FlushAsync
                    Me.MS.Dispose()
                    Me.MS = New MemoryStream
                End If
            End If
            Me.C.BeginReceive(Me.Buffer, 0, Me.Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf BeginReceive), Me)
        Catch ex As Exception
            Me.isDisconnected()
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
                Me.L.Remove()
                L = Nothing
            End If
        Catch ex As Exception
        End Try

        Try
            C.Close()
            C.Dispose()
            C = Nothing
        Catch ex As Exception
        End Try

        Try
            MS.Dispose()
            MS = Nothing
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
