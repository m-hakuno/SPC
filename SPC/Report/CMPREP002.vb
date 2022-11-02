Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class CMPREP002
    Dim mintNo As Integer = 0

    Private Sub PageHeader1_BeforePrint(sender As Object, e As EventArgs) Handles PageHeader1.BeforePrint
        Dim ci As New System.Globalization.CultureInfo("ja-JP")
        Dim jp As New System.Globalization.JapaneseCalendar
        Dim dt As System.DateTime

        ' 使用する暦を、和暦に設定
        ci.DateTimeFormat.Calendar = jp

        '西暦表示
        'If IsDate(TxtYY.Text) = True Then
        If Date.TryParse(TxtYY.Text, dt) = True Then
            dt = DateTime.Parse(TxtYY.Text)
            TxtYY.Text = dt.ToString("yyyy年")
        End If
        ''西暦を和暦に変換する
        ''If IsDate(TxtYY.Text) = True Then
        'If Date.TryParse(TxtYY.Text, dt) = True Then
        '    dt = DateTime.Parse(TxtYY.Text)
        '    TxtYY.Text = dt.ToString("ggyy年", ci)
        'End If

        '西暦を和暦に変換する
        'If IsDate(TxtMM.Text) = True Then
        If Date.TryParse(TxtMM.Text, dt) = True Then
            dt = DateTime.Parse(TxtMM.Text)
            TxtMM.Text = dt.ToString("MM月", ci)
        End If
    End Sub

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format
        Dim dt As System.DateTime

        mintNo += 1
        Me.TxtNo.Text = mintNo.ToString

        'If IsDate(TxtTRANS_DT_MM.Text) = True Then
        If Date.TryParse(TxtTRANS_DT_MM.Text, dt) = True Then
            dt = DateTime.Parse(TxtTRANS_DT_MM.Text)
            TxtTRANS_DT_MM.Text = dt.ToString("MM")
        End If

        'If IsDate(TxtTRANS_DT_DD.Text) = True Then
        If Date.TryParse(TxtTRANS_DT_DD.Text, dt) = True Then
            dt = DateTime.Parse(TxtTRANS_DT_DD.Text)
            TxtTRANS_DT_DD.Text = dt.ToString("dd")
        End If
    End Sub
End Class
