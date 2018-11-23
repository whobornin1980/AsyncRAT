Imports System, System.IO, System.Net, System.Net.Sockets

Public Class ClientSocket

    Public Shared S As Socket

    Public Shared isConnected As Boolean = False

    Public Shared MS As MemoryStream

    Public Shared EOF As String = "<EOF>"
    Public Shared SPL As String = "<N>"


    Public Shared Sub Connect()

        Threading.Thread.Sleep(1000)

        'InterNetworkV6
        S = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)

        Dim ipAddress As IPAddress = IPAddress.Parse("127.0.0.1")
        Dim ipEndPoint As IPEndPoint = New IPEndPoint(ipAddress, 2020)

        S.ReceiveBufferSize = 1024 * 5000
        S.SendBufferSize = S.ReceiveBufferSize

        Try
            S.Connect(ipEndPoint)
            isConnected = True
            Diagnostics.Debug.WriteLine("Connected")

            MS = New MemoryStream

            Dim OS As New Devices.ComputerInfo
            Send(String.Concat("INFO", SPL, "SYNC", SPL, OS.OSFullName, Environment.OSVersion.ServicePack, " ", Environment.Is64BitOperatingSystem.ToString.Replace("False", "32bit").Replace("True", "64bit")))

            Read()

        Catch ex As Exception
            Disconnected()
        End Try

    End Sub

    Private Shared Sub Read()

        Dim i As Integer = 0

        While isConnected = True

            Try
                Threading.Thread.Sleep(1)

                i += 1

                If i > 300 Then
                    i = 0
                    If S.Poll(0, SelectMode.SelectRead) And S.Available <= 0 Then
                        Exit While
                    End If
                End If

                If isConnected = False OrElse Not S.Connected Then
                    Exit While
                End If

                If S.Available > 0 Then

                    Dim Buffer(S.Available - 1) As Byte
                    Diagnostics.Debug.WriteLine("Received " + Buffer.Length.ToString)

                    S.Receive(Buffer, 0, Buffer.Length, SocketFlags.None)
                    MS.Write(Buffer, 0, Buffer.Length)

re:

                    If BS(MS.ToArray).Contains(EOF) Then

                        Dim A As Array = SplitWord(MS.ToArray, EOF)
                        Dim T As New Threading.Thread(AddressOf Messages.Read)
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
            Catch ex As Exception
                Exit While
            End Try
        End While

        Disconnected()

    End Sub

    Private Shared Sub Disconnected()
        isConnected = False
        Diagnostics.Debug.WriteLine("Disconnected")

        Try
            If S IsNot Nothing Then
                S.Close()
                S.Dispose()
            End If
        Catch ex As Exception
        End Try

        Try
            If MS IsNot Nothing Then
                MS.Flush()
                MS.Dispose()
            End If
        Catch ex As Exception
        End Try

        Connect()

    End Sub

    Private Shared Sub Send(ByVal b As Byte())
        Dim MS As MemoryStream = New IO.MemoryStream
        Try
            MS.Write(b, 0, b.Length)
            MS.Write(SB(EOF), 0, EOF.Length)

            SyncLock S
                S.Poll(-1, SelectMode.SelectWrite)
                S.Send(MS.ToArray, 0, MS.Length, SocketFlags.None)
            End SyncLock

            MS.Dispose()
        Catch ex As Exception
            isConnected = False
            MS.Dispose()
        End Try
    End Sub

    Public Shared Sub Send(ByVal S As String)
        Send(SB(S))
    End Sub

    Private Shared Function SplitWord(ByVal b As Byte(), ByVal Word As String) As Array 'credit to njq8
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
    End Function

End Class
