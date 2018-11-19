Imports System.Net.Sockets
Imports System.Net

Public Class ServerSocket
    Public S As Socket

    Sub Start(ByVal Port As Integer)
        S = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        Dim IpEndPoint As IPEndPoint = New IPEndPoint(IPAddress.Any, Port)
        S.ReceiveBufferSize = 1024 * 100
        S.SendBufferSize = 1024 * 100
        S.ReceiveTimeout = -1
        S.SendTimeout = -1
        S.Bind(IpEndPoint)
        S.Listen(999)
        BegingAccpet()
    End Sub

    Sub BegingAccpet()
        S.BeginAccept(New AsyncCallback(AddressOf EndAccept), Nothing)
    End Sub

    Sub EndAccept(ByVal ar As IAsyncResult)
        Try
            Dim C As New Client(S.EndAccept(ar))
            C.C.BeginReceive(C.Buffer, 0, C.Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf C.BeginReceive), C)
        Catch ex As Exception
        End Try
        BegingAccpet()
    End Sub


End Class
