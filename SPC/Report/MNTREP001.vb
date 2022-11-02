Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class MNTREP001

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        Dim ci As New System.Globalization.CultureInfo("ja-JP")
        ' 和暦を表すクラスです。
        Dim jp As New System.Globalization.JapaneseCalendar
        Dim dt As System.DateTime

        ' 現在のカルチャで使用する暦を、和暦に設定します。
        ci.DateTimeFormat.Calendar = jp

        ' TextBoxのデータを、DateTime型に変換します。
        'dt = Now
        dt = Date.Now

        ' 「書式」「カルチャの書式情報」を使用し、文字列に変換します。
        'Me.TxtDate.Text = dt.ToString("gg  yy年 M月 d日", ci)
        Me.TxtDate.Text = dt.ToString("  yyyy年 M月 d日")

    End Sub
End Class
