Module Helper

    Function SB(ByVal s As String) As Byte()
        Return Text.Encoding.Default.GetBytes(s)
    End Function

    Function BS(ByVal b As Byte()) As String
        Return Text.Encoding.Default.GetString(b)
    End Function

End Module
