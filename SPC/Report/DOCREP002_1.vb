Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class DOCREP002_1


    Private Sub PageHeader1_Format(sender As Object, e As EventArgs) Handles PageHeader1.Format
        Dim ci As New System.Globalization.CultureInfo("ja-JP")
        Dim jp As New System.Globalization.JapaneseCalendar
        Dim dt As System.DateTime
        ' 使用する暦を、和暦に設定
        ci.DateTimeFormat.Calendar = jp

        '西暦表示
        If Date.TryParse(TxtDATE.Text, dt) = True Then
            dt = DateTime.Parse(TxtDATE.Text)
            TxtDATE.Text = dt.ToString("yyyy年MM月dd日")
        End If
        If Date.TryParse(TxtYM.Text, dt) = True Then
            dt = DateTime.Parse(TxtYM.Text)
            TxtYM.Text = dt.ToString("yyyy年度MM月")
        End If
        '西暦を和暦に変換する
        'If Date.TryParse(TxtDATE.Text, dt) = True Then
        '    dt = DateTime.Parse(TxtDATE.Text)
        '    TxtDATE.Text = dt.ToString("gg yy年MM月dd日", ci)
        'End If
        'If Date.TryParse(TxtYM.Text, dt) = True Then
        '    dt = DateTime.Parse(TxtYM.Text)
        '    TxtYM.Text = dt.ToString("ggyy年度MM月", ci)
        'End If
        '  TextBox7.Font = New System.Drawing.Font("メイリオ", 15, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Point, 1)
    End Sub

    Private Sub ReportHeader1_Format(sender As Object, e As EventArgs) Handles ReportHeader1.Format
        Dim ci As New System.Globalization.CultureInfo("ja-JP")
        Dim jp As New System.Globalization.JapaneseCalendar
        Dim dt As System.DateTime
        ' 使用する暦を、和暦に設定
        ci.DateTimeFormat.Calendar = jp

        '西暦を和暦に変換する
        If Date.TryParse(TxtDATE.Text, dt) = True Then
            dt = DateTime.Parse(TxtDATE.Text)
            '8/7書式変更　栗原ここから
            'TxtDATE.Text = dt.ToString("gg yy年MM月dd日", ci)
            'TxtDATE.Text = dt.ToString("ggyy年M月d日", ci)
            TxtDATE.Text = dt.ToString("yyyy年M月d日")
        End If
        If Date.TryParse(TxtYM.Text, dt) = True Then
            dt = DateTime.Parse(TxtYM.Text)
            If dt.Month >= 10 Then
                'TxtYM.Text = dt.ToString("ggyy年", ci) & dt.ToString("MMMM", ci)
                TxtYM.Text = dt.ToString("yyyy年") & dt.ToString("MM月")
            Else
                'TxtYM.Text = dt.ToString("ggyy年", ci) & " " & dt.ToString("MMMM", ci)
                TxtYM.Text = dt.ToString("yyyy年") & dt.ToString("MM月")
            End If
        End If
        'ここまで

        If Date.TryParse(TxtMONTH.Text, dt) = True Then
            dt = DateTime.Parse(TxtMONTH.Text)
            TxtMONTH.Text = dt.ToString("MMMM度", ci)
        End If

        If TxtORG.Text <> "" Then
            TxtORG.Text = TxtORG.Text & " 殿"
        End If

    End Sub

    Private Sub Detail1_BeforePrint(sender As Object, e As EventArgs) Handles Detail1.BeforePrint

        Dim dec As Decimal = CDec(TxtTAX.Text)

        '出精値引き
        'TxtDISCOUNT_TTL.Text = Math.Round(Decimal.Parse(TxtTTL_AMO.Text) * Decimal.Parse(TextBox27.Text), 1).ToString("###,###,##0")

        '課税対象料金計
        TxtTAX_TARGET.Text = Math.Floor(Decimal.Parse(TxtTTL_AMO.Text) - Decimal.Parse(TxtDISCOUNT_TTL.Text)).ToString("###,###,##0")

        '合計
        If Decimal.TryParse(TxtTOTAL.Text, dec) = True Then
            TxtTOTAL.Text = Math.Floor(Decimal.Parse(TxtTAX.Text) + Decimal.Parse(TxtTAX_TARGET.Text)).ToString("###,###,##0")
        Else
            TxtTOTAL.Text = 0
        End If

        TextBox27.Text = Decimal.Parse(TextBox27.Text).ToString("f")

        TextBox10.Text = "< " & TextBox10.Text & " 件 >"

        TextBox27.Text = "(× " & TextBox27.Text & " )"

    End Sub

    Private Sub ReportFooter1_Format(sender As Object, e As EventArgs) Handles PageFooter1.Format
        TxtFS_CHARGE.Text = "　" & TxtFS_CHARGE.Text & "　宛"
    End Sub
End Class
