Public Class Incoming_Requests
    Public C As Client
    Public B As Byte()

    Sub New(ByVal C_ As Client, B_ As Byte())
        C = C_
        B = B_
    End Sub

End Class

Public Class Outcoming_Requests
    Public C As Client
    Public B As Byte()

    Sub New(ByVal C_ As Client, B_ As Byte())
        C = C_
        B = B_
    End Sub

End Class



Public Class Pending
    Public Shared Req_In As List(Of Incoming_Requests)
    Public Shared Sub Incoming()
        While True
            Try
                Threading.Thread.Sleep(1)
                Dim ClientReq As Incoming_Requests = Nothing
                If Req_In.Count > 0 Then
                    ClientReq = Req_In.Item(0)
                    Req_In.Remove(ClientReq)
                    Messages.Read(ClientReq.C, ClientReq.B)
                End If
            Catch ex As Exception
            End Try
        End While
    End Sub

    Public Shared Req_Out As List(Of Outcoming_Requests)
    Public Shared Sub OutComing()
        While True
            Try
                Threading.Thread.Sleep(1)
                Dim ClientReq As Outcoming_Requests = Nothing
                If Req_Out.Count > 0 Then
                    ClientReq = Req_Out.Item(0)
                    Req_Out.Remove(ClientReq)
                    ClientReq.C.BeginSend(ClientReq.B)
                End If
            Catch ex As Exception
            End Try
        End While
    End Sub
End Class