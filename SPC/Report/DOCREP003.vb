'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'DOCREP003-001      2016/04/08      加賀　　　「出精値引」の算出を修正(ROUNDDOWN→ROUND)


Imports GrapeCity.ActiveReports
Imports GrapeCity.ActiveReports.Document

Public Class DOCREP003

    Private Sub Detail1_BeforePrint(sender As Object, e As EventArgs) Handles Detail1.BeforePrint
        If TxtNormalPrice.Value = "0" Then
            TxtNormalPrice.Text = ""
        End If
        If TxtNewPrice.Value = "0" Then
            TxtNewPrice.Text = ""
        End If
    End Sub

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        Dim ci As New System.Globalization.CultureInfo("ja-JP")
        ' 和暦を表すクラスです。
        Dim jp As New System.Globalization.JapaneseCalendar
        Dim dt As System.DateTime
        Dim intMonth As Integer = 0

        TextBox1.Visible = False

        ' 現在のカルチャで使用する暦を、和暦に設定します。
        ci.DateTimeFormat.Calendar = jp

        ' TextBoxのデータを、DateTime型に変換します。
        dt = Date.Now

        ' 「書式」「カルチャの書式情報」を使用し、文字列に変換します。
        '8/7書式変更　栗原ここから
        'Me.TxtDate.Text = dt.ToString("ggyy年M月d日", ci)
        'Me.TxtDate.Text = dt.ToString("ggyy年M月d日", ci)
        Me.TxtDate.Text = dt.ToString("yyyy年M月d日")
        'Me.TxtYM.Text = CType(Me.TxtYM.Text, Date).ToString("ggyy年M月", ci)
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
        If TxtMonth.Text.Trim = "99月度" Then
            intMonth = MONTH.Text
            TxtMonth.Text = intMonth.ToString("0") + "月度"
        End If

    End Sub

    Private Sub GroupFooter2_Format(sender As Object, e As EventArgs) Handles GroupFooter2.Format

        If TxtGroupCD.Text = "1" Then
            TxtKindNM.Text = "磁気無線計"
        ElseIf TxtGroupCD.Text = "2" Then
            TxtKindNM.Text = "磁気有線計"
        ElseIf TxtGroupCD.Text = "3" Then
            TxtKindNM.Text = "IT計"
        End If

    End Sub

    Private Sub GroupFooter1_Format(sender As Object, e As EventArgs) Handles GroupFooter1.Format

        Dim dec As Decimal = 0
        Dim decRate As Decimal = 0

        '出精値引き
        If Decimal.TryParse(TxtReductSubTotal.Text, dec) = True Then
            'TxtReductSubTotal.Text = Math.Floor(Decimal.Parse(TxtSubTotal.Text) * Decimal.Parse(TxtReductRate.Text)).ToString("#,##0") 'DOCREP003-001
            TxtReductSubTotal.Text = Math.Round(Decimal.Parse(TxtSubTotal.Text) * Decimal.Parse(TxtReductRate.Text), MidpointRounding.AwayFromZero).ToString("#,##0")
        End If
        '課税対象額
        If Decimal.TryParse(TxtTaxSubTotal.Text, dec) = True Then
            TxtTaxSubTotal.Text = (Decimal.Parse(TxtSubTotal.Text) - Decimal.Parse(TxtReductSubTotal.Text)).ToString("#,##0")
        End If
        '消費税相当額
        If Decimal.TryParse(TxtTax.Text, dec) = True Then
            If Decimal.TryParse(txtTaxrate.Text, decRate) = True Then
            End If
            TxtTax.Text = Math.Floor(Decimal.Parse(TxtTaxSubTotal.Text) * decRate).ToString("#,##0")
        End If
        '合計
        If Decimal.TryParse(TxtTotal.Text, dec) = True Then
            TxtTotal.Text = (Decimal.Parse(TxtTaxSubTotal.Text) + Decimal.Parse(TxtTax.Text)).ToString("#,##0")
        End If

        TxtReductRate.Text = Decimal.Parse(TxtReductRate.Text).ToString("f")
    End Sub

    Private Sub PageHeader1_BeforePrint(sender As Object, e As EventArgs) Handles PageHeader1.BeforePrint

        If TxtDestination.Text <> "" Then
            TxtDestination.Text = TxtDestination.Text & " 殿"
        End If
    End Sub
End Class
