Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class ETCREP020

    Private Sub PageHeader1_Format(sender As Object, e As EventArgs) Handles PageHeader1.BeforePrint
        Dim ci As New System.Globalization.CultureInfo("ja-JP")
        Dim jp As New System.Globalization.JapaneseCalendar
        Dim dt As System.DateTime

        ' 使用する暦を、和暦に設定
        ci.DateTimeFormat.Calendar = jp

        '西暦を和暦に変換する
        'If IsDate(TxtDate.Text) = True Then
        If Date.TryParse(TxtDATE.Text, dt) = True Then
            dt = DateTime.Parse(TxtDATE.Text)
            'TxtDATE.Text = dt.ToString("ggyy年MM月dd日", ci)
            TxtDATE.Text = dt.ToString("yyyy年MM月dd日")
        End If

        '西暦を和暦に変換する
        'If IsDate(TxtDate.Text) = True Then
        If Date.TryParse(TxtGYM.Text, dt) = True Then
            dt = DateTime.Parse(TxtGYM.Text)
            'TxtGYM.Text = dt.ToString("ggyy年MM月", ci)
            TxtGYM.Text = dt.ToString("yyyy年MM月")
        End If

    End Sub

    Private Sub Detail1_BeforePrint(sender As Object, e As EventArgs) Handles Detail1.BeforePrint
        If TxtTITLE.Text = "NVC" Then
            TxtTITLE.Text = "LUTERNA"
        End If
        If Me.TxtCNT_NM.Text.Trim = "設置台数" Then
            Me.LblColor.Visible = True
        Else
            Me.LblColor.Visible = False
        End If
    End Sub

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        'Dim tryCNT As Integer
        'Dim CNT As Integer = 0
        'Dim text() As String = {TxtCNT1.Text, TxtCNT2.Text, TxtCNT3.Text, TxtCNT4.Text, TxtCNT5.Text, TxtCNT6.Text}
        'For n = 0 To 5
        '    If Integer.TryParse(text(n), tryCNT) = True Then
        '        CNT += Integer.Parse(text(n))
        '    Else
        '    End If
        'Next n
        'TxtCNT7.Text = CNT

    End Sub

End Class
