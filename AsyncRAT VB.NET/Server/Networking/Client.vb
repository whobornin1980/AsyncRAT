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
    ' Public Shared Event Read(ByVal C As Client, ByVal b() As Byte)

    Sub New(ByVal CL As Socket)
        Me.C = CL

        Me.Buffer = New Byte(1024 * 100) {}
        Me.MS = New MemoryStream
        Me.IP = CL.RemoteEndPoint.ToString

        C.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf BeginReceive), C)
    End Sub

    Async Sub BeginReceive(ByVal ar As IAsyncResult)
        If IsConnected = False Then isDisconnected()
        Try
            Dim Received As Integer = C.EndReceive(ar)
            If Received > 0 Then
                Await MS.WriteAsync(Buffer, 0, Received)
re:
                If BS(MS.ToArray).Contains(Settings.EOF) Then
                    Dim A As Array = Await fx(MS.ToArray, Settings.EOF)
                    '  RaiseEvent Read(Me, A(0))
                    Threading.ThreadPool.QueueUserWorkItem(New Threading.WaitCallback(Sub() Messages.Read(Me, A(0))))
                    Await MS.FlushAsync
                    MS.Dispose()
                    MS = New MemoryStream

                    If A.Length = 2 Then
                        MS.Write(A(1), 0, A(1).length)
                        GoTo re
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

    'Credit to njq8 / better wat to split
    Async Function fx(ByVal b As Byte(), ByVal Word As String) As Threading.Tasks.Task(Of Array)
        Dim a As New Collections.Generic.List(Of Byte())
        Dim M As New MemoryStream
        Dim MM As New MemoryStream
        Dim T As String() = Split(BS(b), Word)
        Await M.WriteAsync(b, 0, T(0).Length)
        Await MM.WriteAsync(b, T(0).Length + Word.Length, b.Length - (T(0).Length + Word.Length))
        a.Add(M.ToArray)
        a.Add(MM.ToArray)
        M.Dispose()
        MM.Dispose()
        Return a.ToArray
    End Function

End Class
