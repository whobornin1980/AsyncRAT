Public Class Messages

    Public Shared F As Form1
    Delegate Sub _Read(ByVal C As Client, ByVal b() As Byte)

    Public Shared Sub Read(ByVal C As Client, ByVal b() As Byte)
        Try
            Dim A As String() = Split(BS(b).Replace(Settings.EOF, Nothing), Settings.SPL)

            Select Case A(0)

                Case "INFO"
                    If F.InvokeRequired Then : F.Invoke(New _Read(AddressOf Read), New Object() {C, b}) : Exit Sub : Else

                        C.L = F.LV1.Items.Insert(0, C.IP.Split(":")(0))
                        C.L.Tag = C
                        For i As Integer = 1 To A.Length - 1
                            C.L.SubItems.Add(A(i))
                        Next
                        F.LV1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
                    End If
            End Select
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

End Class
