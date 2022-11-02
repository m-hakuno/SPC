Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class WATREP005

    Private Sub PageHeader1_Format(sender As Object, e As EventArgs) Handles PageHeader1.Format

    End Sub

    Private Sub GroupHeader1_Format(sender As Object, e As EventArgs) Handles GroupHeader1.Format
        Dim ci As New System.Globalization.CultureInfo("ja-JP")
        Dim jp As New System.Globalization.JapaneseCalendar
        Dim dt As System.DateTime
        ' 使用する暦を、和暦に設定
        ci.DateTimeFormat.Calendar = jp

        '西暦を和暦に変換する
        If Date.TryParse(txtMAKE_DT_1.Text, dt) = True Then
            dt = DateTime.Parse(txtMAKE_DT_1.Text)
            'txtMAKE_DT_1.Text = dt.ToString("ggyy年MM月dd日", ci)
            txtMAKE_DT_1.Text = dt.ToString("yyyy年MM月dd日")
        End If
    End Sub
End Class
