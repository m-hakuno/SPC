Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class RETREP001

    Private Sub GroupHeader1_Format(sender As Object, e As EventArgs) Handles GroupHeader1.Format
        Dim ci As New System.Globalization.CultureInfo("ja-JP")
        Dim jp As New System.Globalization.JapaneseCalendar
        Dim dt As System.DateTime


        ci.DateTimeFormat.Calendar = jp
        dt = DateTime.Parse(System.DateTime.Now)
        TxtDate.Text = dt.ToString("yyyy年M月d日")
        ' 使用する暦を、和暦に設定
        '西暦を和暦に変換する
        If Date.TryParse(TxtDlvPlnDT.Text, dt) = True Then
            dt = DateTime.Parse(TxtDlvPlnDT.Text)
            'TxtDlvPlnDT.Text = dt.ToString("ggyy年M月d日", ci)
            TxtDlvPlnDT.Text = dt.ToString("yyyy年M月d日")
        End If
    End Sub

    Private Sub Detail_Format(sender As Object, e As EventArgs) Handles Detail1.Format
        TxtTotal.Text = CType(TxtRmvSubTotal.Text, Integer) + CType(TxtUnuseSubTotal.Text, Integer)
    End Sub

End Class
