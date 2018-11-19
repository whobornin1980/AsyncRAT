Public Class Main

    Public Shared Sub Main()
        Dim T As New Threading.Thread(AddressOf ClientSocket.BeginConnect)
        T.Start()
    End Sub

End Class
