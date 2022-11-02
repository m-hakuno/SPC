Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class CNSREP008
    Private Sub PageHeader1_Format(sender As Object, e As EventArgs) Handles PageHeader1.Format
        Dim ci As New System.Globalization.CultureInfo("ja-JP")
        Dim jp As New System.Globalization.JapaneseCalendar
        Dim dt As System.DateTime


        ci.DateTimeFormat.Calendar = jp
        dt = DateTime.Parse(System.DateTime.Now)
        TxtDate.Text = dt.ToString("yyyy年M月d日") & "  第１版"
    End Sub

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format
        Dim strTboxid As String = TBOXID1.Text
        Dim strSerial As String = シリアル番号1.Text
        Dim intcnt As Integer = 0
        '各項目を初期化する
        TBOXID1.Text = String.Empty
        TBOXID2.Text = String.Empty
        TBOXID3.Text = String.Empty
        TBOXID4.Text = String.Empty
        TBOXID5.Text = String.Empty
        TBOXID6.Text = String.Empty
        TBOXID7.Text = String.Empty
        TBOXID8.Text = String.Empty
        シリアル番号1.Text = String.Empty
        シリアル番号2.Text = String.Empty
        シリアル番号3.Text = String.Empty
        シリアル番号4.Text = String.Empty
        シリアル番号5.Text = String.Empty
        シリアル番号6.Text = String.Empty
        シリアル番号7.Text = String.Empty
        シリアル番号8.Text = String.Empty
        シリアル番号9.Text = String.Empty
        シリアル番号10.Text = String.Empty
        シリアル番号11.Text = String.Empty
        シリアル番号12.Text = String.Empty
        シリアル番号13.Text = String.Empty
        シリアル番号14.Text = String.Empty
        シリアル番号15.Text = String.Empty
        シリアル番号16.Text = String.Empty

        'TBOXIDを各テキストボックスに割り当てる
        If strTboxid.Length > 0 Then
            For intcnt = 0 To strTboxid.Length
                Select Case intcnt
                    Case 0
                        TBOXID1.Text = strTboxid.Substring(intcnt, 1)
                    Case 1
                        TBOXID2.Text = strTboxid.Substring(intcnt, 1)
                    Case 2
                        TBOXID3.Text = strTboxid.Substring(intcnt, 1)
                    Case 3
                        TBOXID4.Text = strTboxid.Substring(intcnt, 1)
                    Case 4
                        TBOXID5.Text = strTboxid.Substring(intcnt, 1)
                    Case 5
                        TBOXID6.Text = strTboxid.Substring(intcnt, 1)
                    Case 6
                        TBOXID7.Text = strTboxid.Substring(intcnt, 1)
                    Case 7
                        TBOXID8.Text = strTboxid.Substring(intcnt, 1)
                End Select
            Next
        Else
            TBOXID1.Text = String.Empty
            TBOXID2.Text = String.Empty
            TBOXID3.Text = String.Empty
            TBOXID4.Text = String.Empty
            TBOXID5.Text = String.Empty
            TBOXID6.Text = String.Empty
            TBOXID7.Text = String.Empty
            TBOXID8.Text = String.Empty
        End If
        'シリアル番号を各テキストボックスに割り当てる
        If strSerial.Length > 0 Then
            For intcnt = 0 To strSerial.Length - 1
                Select Case intcnt
                    Case 0
                        シリアル番号1.Text = strSerial.Substring(intcnt, 1)
                    Case 1
                        シリアル番号2.Text = strSerial.Substring(intcnt, 1)
                    Case 2
                        シリアル番号3.Text = strSerial.Substring(intcnt, 1)
                    Case 3
                        シリアル番号4.Text = strSerial.Substring(intcnt, 1)
                    Case 4
                        シリアル番号5.Text = strSerial.Substring(intcnt, 1)
                    Case 5
                        シリアル番号6.Text = strSerial.Substring(intcnt, 1)
                    Case 6
                        シリアル番号7.Text = strSerial.Substring(intcnt, 1)
                    Case 7
                        シリアル番号8.Text = strSerial.Substring(intcnt, 1)
                    Case 8
                        シリアル番号9.Text = strSerial.Substring(intcnt, 1)
                    Case 9
                        シリアル番号10.Text = strSerial.Substring(intcnt, 1)
                    Case 10
                        シリアル番号11.Text = strSerial.Substring(intcnt, 1)
                    Case 11
                        シリアル番号12.Text = strSerial.Substring(intcnt, 1)
                    Case 12
                        シリアル番号13.Text = strSerial.Substring(intcnt, 1)
                    Case 13
                        シリアル番号14.Text = strSerial.Substring(intcnt, 1)
                    Case 14
                        シリアル番号15.Text = strSerial.Substring(intcnt, 1)
                    Case 15
                        シリアル番号16.Text = strSerial.Substring(intcnt, 1)
                End Select
            Next
        Else
            シリアル番号1.Text = String.Empty
            シリアル番号2.Text = String.Empty
            シリアル番号3.Text = String.Empty
            シリアル番号4.Text = String.Empty
            シリアル番号5.Text = String.Empty
            シリアル番号6.Text = String.Empty
            シリアル番号7.Text = String.Empty
            シリアル番号8.Text = String.Empty
            シリアル番号9.Text = String.Empty
            シリアル番号10.Text = String.Empty
            シリアル番号11.Text = String.Empty
            シリアル番号12.Text = String.Empty
            シリアル番号13.Text = String.Empty
            シリアル番号14.Text = String.Empty
            シリアル番号15.Text = String.Empty
            シリアル番号16.Text = String.Empty
        End If
    End Sub
End Class
