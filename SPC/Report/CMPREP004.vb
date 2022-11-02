Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class CMPREP004

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        '和暦を表すクラス
        Dim ci As New System.Globalization.CultureInfo("ja-JP")
        Dim jp As New System.Globalization.JapaneseCalendar
        ci.DateTimeFormat.Calendar = jp

        '対応年月を表示()
        If (Me.Fields("対応年月").Value).ToString = "1900/01/01 0:00:00" Then
            TxtYearMonth.Text = ""
        Else
            '            TxtYearMonth.Text = CDate(Me.Fields("対応年月").Value).ToString("ggy年M月", ci)
            TxtYearMonth.Text = CDate(Me.Fields("対応年月").Value).ToString("yyyy年M月")
        End If



    End Sub

End Class
