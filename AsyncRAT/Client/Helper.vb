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

    Function AES_Encryptor(ByVal input As Byte()) As Byte()
        Dim AES As New Security.Cryptography.RijndaelManaged
        Dim SHA256 As New Security.Cryptography.SHA256Cng
        Dim ciphertext As String = ""
        Try
            AES.Key = SHA256.ComputeHash(SB(Settings.KEY))
            AES.Mode = Security.Cryptography.CipherMode.ECB
            Dim DESEncrypter As Security.Cryptography.ICryptoTransform = AES.CreateEncryptor
            Dim Buffer As Byte() = input
            Return DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length)
        Catch ex As Exception
        End Try
    End Function

    Function AES_Decryptor(ByVal input As Byte()) As Byte()
        Dim AES As New Security.Cryptography.RijndaelManaged
        Dim SHA256 As New Security.Cryptography.SHA256Cng
        Try
            AES.Key = SHA256.ComputeHash(SB(Settings.KEY))
            AES.Mode = Security.Cryptography.CipherMode.ECB
            Dim DESDecrypter As Security.Cryptography.ICryptoTransform = AES.CreateDecryptor
            Dim Buffer As Byte() = input
            Return DESDecrypter.TransformFinalBlock(Buffer, 0, Buffer.Length)
        Catch ex As Exception
        End Try
    End Function
End Module
