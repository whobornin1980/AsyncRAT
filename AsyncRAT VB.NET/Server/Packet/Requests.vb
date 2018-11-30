Public Class Requests
    Public C As Client
    Public B As Byte()

    Sub New(ByVal C_ As Client, B_ As Byte())
        C = C_
        B = B_
    End Sub

End Class



Public Class Pending
    Public Shared Req As List(Of Requests)
    Public Shared Sub Pen()
        While True
            Try
                Threading.Thread.Sleep(1)
                Dim ClientReq As Requests = Nothing
                If Req.Count > 0 Then
                    ClientReq = Req.Item(0)
                    Req.Remove(ClientReq)
                    Messages.Read(ClientReq.C, ClientReq.B)
                End If
            Catch ex As Exception
            End Try
        End While
    End Sub
End Class
