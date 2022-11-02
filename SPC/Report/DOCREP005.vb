Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class DOCREP005
    Dim mintNo As Integer = 0
    Dim strDiscount As String = String.Empty
    Dim strTaxTarget As String = String.Empty
    Dim strTotal As String = String.Empty

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format
        mintNo += 1
        Me.TxtNo.Text = mintNo.ToString

        '空行の非表示
        If lblEmpty.Text = "1" Then
            TxtITEM.Visible = False
            TxtORDER_NO.Visible = False
            TxtPRICE.Visible = False
            TxtNOTETEXT.Visible = False
        Else
            TxtITEM.Visible = True
            TxtORDER_NO.Visible = True
            TxtPRICE.Visible = True
            TxtNOTETEXT.Visible = True
        End If
        'If TxtITEM.Text = "" Then
        '    TxtPRICE.Text = ""
        'End If

        If Me.Fields("空行フラグ").Value.ToString = "0" Then
            strDiscount = Me.Fields("出精値引き").Value.ToString
            strTaxTarget = Me.Fields("課税対象料金金額").Value.ToString
            strTotal = Me.Fields("合計金額").Value.ToString
        End If


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
            'TxtDATE.Text = dt.ToString("ggyy年MMMMd日", ci)
            TxtDATE.Text = dt.ToString("yyyy年MMMMd日")
        Else
            TxtDATE.Text = ""
        End If
        If Date.TryParse(TxtYM.Text, dt) = True Then
            dt = DateTime.Parse(TxtYM.Text)
            If dt.Month >= 10 Then
                '                TxtYM.Text = dt.ToString("ggyy年", ci) & dt.ToString("MMMM", ci)
                TxtYM.Text = dt.ToString("yyyy年") & dt.ToString("MM月")
            Else
                'TxtYM.Text = dt.ToString("ggyy年", ci) & " " & dt.ToString("MMMM", ci)
                TxtYM.Text = dt.ToString("yyyy年") & dt.ToString("MM月")
            End If
        Else
            TxtYM.Text = ""
        End If
        If Date.TryParse(TxtMONTH.Text, dt) = True Then
            dt = DateTime.Parse(TxtMONTH.Text)
            TxtMONTH.Text = dt.ToString("MMMM度", ci)
        Else
            TxtMONTH.Text = ""
        End If

        If TxtORG.Text <> "" Then
            TxtORG.Text = TxtORG.Text & " 殿"
        End If

    End Sub

    Private Sub GroupFooter1_BeforePrint(sender As Object, e As EventArgs) Handles GroupFooter1.BeforePrint

        Dim dec As Decimal = CDec(TxtTAX.Text)

        'TextBox22.Text = (intAllMoney).ToString
        'TxtDISCOUNT.Text = Math.Floor(Decimal.Parse(TxtSUBTOTAL.Text) * Decimal.Parse(TextBox27.Text)).ToString("###,###,##0")
        'TxtTAX_TARGET.Text = Math.Floor(Decimal.Parse(TxtSUBTOTAL.Text) - Decimal.Parse(TxtDISCOUNT.Text)).ToString("###,###,##0")
        TxtDISCOUNT.Text = Decimal.Parse(strDiscount).ToString("###,###,##0")
        TxtTAX_TARGET.Text = Decimal.Parse(strTaxTarget).ToString("###,###,##0")

        'If IsNumeric(TextBox25.Text) = True Then
        '    TextBox25.Text = Math.Floor(Decimal.Parse(TextBox24.Text) * Decimal.Parse(TextBox25.Text)).ToString
        'Else
        '    TextBox25.Text = 0
        'End If
        'If Decimal.TryParse(TxtTOTAL.Text, dec) = True Then
        '    TxtTOTAL.Text = Math.Floor(Decimal.Parse(TxtTAX.Text) + Decimal.Parse(TxtTAX_TARGET.Text)).ToString("###,###,##0")
        'Else
        '    TxtTOTAL.Text = 0
        'End If
        If Decimal.TryParse(strTotal, dec) = True Then
            TxtTOTAL.Text = Decimal.Parse(strTotal).ToString("###,###,##0")
        Else
            TxtTOTAL.Text = 0
        End If


        TextBox27.Text = Decimal.Parse(TextBox27.Text).ToString("f")
    End Sub

    'MNTOUTP001-001
    Private Sub GroupFooter1_Format(sender As Object, e As EventArgs) Handles GroupFooter1.Format
        TxtFS_CHARGE.Text = TxtFS_CHARGE.Text & "　宛"
    End Sub
End Class
