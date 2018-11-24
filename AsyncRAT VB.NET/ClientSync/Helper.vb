Module Helper

    Function SB(ByVal s As String) As Byte()
        Return Text.Encoding.Default.GetBytes(s)
    End Function

    Function BS(ByVal b As Byte()) As String
        Return Text.Encoding.Default.GetString(b)
    End Function

    Function ID() As String
        Dim S As String = Nothing

        S += Environment.UserDomainName
        S += Environment.UserName
        S += Environment.MachineName

        Return S
    End Function

    Function GetHash(strToHash As String) As String

        Dim md5Obj As New Security.Cryptography.MD5CryptoServiceProvider
        Dim bytesToHash() As Byte = Text.Encoding.ASCII.GetBytes(strToHash)

        bytesToHash = md5Obj.ComputeHash(bytesToHash)
        Dim strResult As New Text.StringBuilder

        For Each b As Byte In bytesToHash
            strResult.Append(b.ToString("x2"))
        Next

        Return strResult.ToString.Substring(0, 12).ToUpper

    End Function
End Module
