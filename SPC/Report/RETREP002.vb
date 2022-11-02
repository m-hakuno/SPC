Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class RETREP002

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format
        工事区分明細.Text = 工事区分明細.Text.Replace("（", " ").Replace("）", "")
    End Sub

    Private Sub PageHeader1_Format(sender As Object, e As EventArgs) Handles PageHeader1.Format
        Dim ci As New System.Globalization.CultureInfo("ja-JP")
        Dim jp As New System.Globalization.JapaneseCalendar
        Dim dt As System.DateTime


        ci.DateTimeFormat.Calendar = jp
        dt = DateTime.Parse(System.DateTime.Now)
        出力日付.Text = dt.ToString("yyyy年M月d日")
    End Sub

    'Private Sub PageFooter1_Format(sender As Object, e As EventArgs) Handles PageFooter1.Format
    '    台数総計.Text = TxtTotalAmnt.Text
    'End Sub
End Class
