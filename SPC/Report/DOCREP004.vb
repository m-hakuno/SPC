Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class DOCREP004
    Dim mintNo As Integer = 0
    Dim intID_N_Count As Integer = 0
    Dim intID_L_Count As Integer = 0
    Dim intIC_Count As Integer = 0
    Dim intLUTERNA_Count As Integer = 0

    Dim intALL_Count As Integer = 0
    Dim intID_N_Money As Integer = 0
    Dim intID_L_Money As Integer = 0
    Dim intIC_Money As Integer = 0
    Dim intLUTERNA_Money As Integer = 0
    Dim intAllMoney As Integer = 0

    Private Sub PageHeader1_Format(sender As Object, e As EventArgs) Handles PageHeader1.BeforePrint
        Dim ci As New System.Globalization.CultureInfo("ja-JP")
        Dim jp As New System.Globalization.JapaneseCalendar
        Dim dt As System.DateTime

        ' 使用する暦を、和暦に設定
        ci.DateTimeFormat.Calendar = jp

        '西暦を和暦に変換する
        'If IsDate(TxtDate.Text) = True Then
        If Date.TryParse(TxtDate.Text, dt) = True Then
            dt = DateTime.Parse(TxtDate.Text)
            '8/7書式変更　栗原ここから
            'TxtDate.Text = dt.ToString("ggyy年MM月dd日", ci)
            'TxtDate.Text = dt.ToString("ggyy年M月d日", ci)
            TxtDate.Text = dt.ToString("yyyy年M月d日")
            'ここまで
        End If

        '西暦を和暦に変換する
        'If IsDate(TxtDate.Text) = True Then
        If Date.TryParse(TxtDate.Text, dt) = True Then
            dt = DateTime.Parse(TxtYear.Text)
            'TxtYear.Text = dt.ToString("ggyy年M月", ci)
            If dt.Month >= 10 Then
                'TxtYear.Text = dt.ToString("ggyy年", ci) & dt.ToString("MMMM", ci)
                TxtYear.Text = dt.ToString("yyyy年") & dt.ToString("MM月")
            Else
                'TxtYear.Text = dt.ToString("ggyy年", ci) & " " & dt.ToString("MMMM", ci)
                TxtYear.Text = dt.ToString("yyyy年") & dt.ToString("MM月")
            End If
        End If

        If Date.TryParse(TxtMM.Text, dt) = True Then
            dt = DateTime.Parse(TxtMM.Text)
            TxtMM.Text = dt.ToString("MMMM度", ci)
        End If
    End Sub

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        'Dim ci As New System.Globalization.CultureInfo("ja-JP")
        'Dim jp As New System.Globalization.JapaneseCalendar
        'Dim dt As System.DateTime

        '' 使用する暦を、和暦に設定
        'ci.DateTimeFormat.Calendar = jp

        ''西暦を和暦に変換する
        ''If IsDate(TxtDate.Text) = True Then
        'If Date.TryParse(TxtDate.Text, dt) = True Then
        '    dt = DateTime.Parse(TxtDate.Text)
        '    TxtDate.Text = dt.ToString("ggyy年MM月dd日", ci)
        'End If

        ''西暦を和暦に変換する
        ''If IsDate(TxtDate.Text) = True Then
        'If Date.TryParse(TxtDate.Text, dt) = True Then
        '    dt = DateTime.Parse(TxtYear.Text)
        '    TxtYear.Text = dt.ToString("ggyy年MM月", ci)
        'End If

        mintNo += 1
        Me.TxtNo.Text = mintNo.ToString
    End Sub

    Private Sub Detail1_BeforePrint(sender As Object, e As EventArgs) Handles Detail1.BeforePrint
        'TxtID_N_MONEY.Text = ""
        'TxtID_L_MONEY.Text = ""
        'TxtIC_MONEY.Text = ""
        'TxtLUTERNA_MONEY.Text = ""
        'TxtALL_MONEY.Text = ""

        TxtALL_COUNT.Text = 0

        Dim int As Integer = 0
        Dim dec As Decimal = 0

        If TxtTBOXID.Text = "特別保守費用" Then
            TxtALL_MONEY.Text = TxtMAINTE_RATE.Text
            TxtALL_COUNT.Text = 1
        End If

        If TxtTBOXID.Text = "緊急運営輸送費" Then
            TxtALL_MONEY.Text = TxtMAINTE_RATE.Text
            TxtALL_COUNT.Text = 1
        End If

        If TxtTBOXID.Text = "修理・有償部品費" Then
            TxtALL_MONEY.Text = TxtMAINTE_RATE.Text
            TxtALL_COUNT.Text = 1
        End If



        'If IsNumeric(TxtID_N_COUNT.Text) = True Then
        If Integer.TryParse(TxtID_N_COUNT.Text.Replace(",", ""), int) = True Then
            If TxtID_N_COUNT.Text = String.Empty Then
                TxtID_N_COUNT.Text = 0
            End If
            TxtALL_COUNT.Text = Integer.Parse(TxtALL_COUNT.Text) + Integer.Parse(TxtID_N_COUNT.Text.Replace(",", ""))
            intID_N_Count += Integer.Parse(TxtID_N_COUNT.Text)
        End If
        'If IsNumeric(TxtID_L_COUNT.Text) = True Then
        If Integer.TryParse(TxtID_L_COUNT.Text.Replace(",", ""), int) = True Then
            If TxtID_L_COUNT.Text = String.Empty Then
                TxtID_L_COUNT.Text = 0
            End If
            TxtALL_COUNT.Text = Integer.Parse(TxtALL_COUNT.Text) + Integer.Parse(TxtID_L_COUNT.Text.Replace(",", ""))
            intID_L_Count += Integer.Parse(TxtID_L_COUNT.Text.Replace(",", ""))
        End If
        'If IsNumeric(TxtIC_COUNT.Text) = True Then
        If Integer.TryParse(TxtIC_COUNT.Text.Replace(",", ""), int) = True Then
            If TxtIC_COUNT.Text = String.Empty Then
                TxtIC_COUNT.Text = 0
            End If
            TxtALL_COUNT.Text = Integer.Parse(TxtALL_COUNT.Text) + Integer.Parse(TxtIC_COUNT.Text.Replace(",", ""))
            intIC_Count += Integer.Parse(TxtIC_COUNT.Text.Replace(",", ""))
        End If
        'If IsNumeric(TxtLUTERNA_COUNT.Text) = True Then
        If Integer.TryParse(TxtLUTERNA_COUNT.Text.Replace(",", ""), int) = True Then
            If TxtLUTERNA_COUNT.Text = String.Empty Then
                TxtLUTERNA_COUNT.Text = 0
            End If
            TxtALL_COUNT.Text = Integer.Parse(TxtALL_COUNT.Text) + Integer.Parse(TxtLUTERNA_COUNT.Text.Replace(",", ""))
            intLUTERNA_Count += Integer.Parse(TxtLUTERNA_COUNT.Text.Replace(",", ""))
        End If
        If TxtALL_COUNT.Text = 0 And TxtTBOXID.Text = "" Then
            TxtALL_COUNT.Text = ""
        End If

        If Not TxtTBOXID.Text = String.Empty Then
            If TxtID_N_COUNT.Text = String.Empty Then
                TxtID_N_COUNT.Text = 0
            End If
            If TxtID_L_COUNT.Text = String.Empty Then
                TxtID_L_COUNT.Text = 0
            End If
            If TxtIC_COUNT.Text = String.Empty Then
                TxtIC_COUNT.Text = 0
            End If
            If TxtLUTERNA_COUNT.Text = String.Empty Then
                TxtLUTERNA_COUNT.Text = 0
            End If
            TxtALL_COUNT.Text = Integer.Parse(TxtID_N_COUNT.Text.Replace(",", "")) + Integer.Parse(TxtID_L_COUNT.Text.Replace(",", "")) _
                + Integer.Parse(TxtIC_COUNT.Text.Replace(",", "")) + Integer.Parse(TxtLUTERNA_COUNT.Text.Replace(",", ""))
        End If

        If Integer.TryParse(TxtMAINTE_RATE.Text.Replace(",", ""), int) = True Then
            'If IsNumeric(TxtID_N_COUNT.Text) = True Then
            If Integer.TryParse(TxtID_N_COUNT.Text.Replace(",", ""), int) = True Then
                'TxtID_N_MONEY_TTL.Text = (Integer.Parse(TxtMAINTE_RATE.Text.Replace(",", "")) * Integer.Parse(TxtID_N_COUNT.Text.Replace(",", ""))).ToString
                If TxtID_N_MONEY.Text = String.Empty Then
                    TxtID_N_MONEY.Text = 0
                End If
                intID_N_Money += Integer.Parse(TxtID_N_MONEY.Text.Replace(",", ""))
            End If
            'If IsNumeric(TxtID_L_COUNT.Text) = True Then
            If Integer.TryParse(TxtID_L_COUNT.Text.Replace(",", ""), int) = True Then
                'TxtID_L_MONEY_TTL.Text = (Integer.Parse(TxtMAINTE_RATE.Text.Replace(",", "")) * Integer.Parse(TxtID_L_COUNT.Text.Replace(",", ""))).ToString
                If TxtID_L_MONEY.Text = String.Empty Then
                    TxtID_L_MONEY.Text = 0
                End If
                intID_L_Money += Integer.Parse(TxtID_L_MONEY.Text.Replace(",", ""))
            End If
            'If IsNumeric(TxtIC_COUNT.Text) = True Then
            If Integer.TryParse(TxtIC_COUNT.Text.Replace(",", ""), int) = True Then
                'TxtIC_MONEY_TTL.Text = (Integer.Parse(TxtMAINTE_RATE.Text.Replace(",", "")) * Integer.Parse(TxtIC_COUNT.Text.Replace(",", ""))).ToString
                If TxtIC_MONEY.Text = String.Empty Then
                    TxtIC_MONEY.Text = 0
                End If
                intIC_Money += Integer.Parse(TxtIC_MONEY.Text.Replace(",", ""))
            End If
            'If IsNumeric(TxtLUTERNA_COUNT.Text) = True Then
            If Integer.TryParse(TxtLUTERNA_COUNT.Text.Replace(",", ""), int) = True Then
                'TxtLUTERNA_MONEY_TTL.Text = (Integer.Parse(TxtMAINTE_RATE.Text.Replace(",", "")) * Integer.Parse(TxtLUTERNA_COUNT.Text.Replace(",", ""))).ToString
                If TxtLUTERNA_MONEY.Text = String.Empty Then
                    TxtLUTERNA_MONEY.Text = 0
                End If
                intLUTERNA_Money += Integer.Parse(TxtLUTERNA_MONEY.Text.Replace(",", ""))
            End If
            'If IsNumeric(TxtAllCount.Text) = True Then
            If Integer.TryParse(TxtALL_COUNT.Text.Replace(",", ""), int) = True Then
                'TxtALL_MONEY_TTL.Text = (Integer.Parse(TxtMAINTE_RATE.Text.Replace(",", "")) * Integer.Parse(TxtALL_COUNT.Text.Replace(",", ""))).ToString
                If TxtALL_MONEY.Text = String.Empty Then
                    TxtALL_MONEY.Text = 0
                End If
                intAllMoney += Integer.Parse(TxtALL_MONEY.Text.Replace(",", ""))
            End If
        End If


        If Decimal.TryParse(TxtMAINTE_RATE.Text.Replace(",", ""), dec) = True Then
            Me.TxtMAINTE_RATE.Text = dec.ToString("#,##0")
        End If
        If Decimal.TryParse(TxtID_N_MONEY.Text.Replace(",", ""), dec) = True Then
            Me.TxtID_N_MONEY.Text = dec.ToString("#,##0")
        End If
        If Decimal.TryParse(TxtID_L_MONEY.Text.Replace(",", ""), dec) = True Then
            Me.TxtID_L_MONEY.Text = dec.ToString("#,##0")
        End If
        If Decimal.TryParse(TxtIC_MONEY.Text.Replace(",", ""), dec) = True Then
            Me.TxtIC_MONEY.Text = dec.ToString("#,##0")
        End If
        If Decimal.TryParse(TxtLUTERNA_MONEY.Text.Replace(",", ""), dec) = True Then
            Me.TxtLUTERNA_MONEY.Text = dec.ToString("#,##0")
        End If
        If Decimal.TryParse(TxtLUTERNA_MONEY.Text.Replace(",", ""), dec) = True Then
            Me.TxtLUTERNA_MONEY.Text = dec.ToString("#,##0")
        End If
        If Decimal.TryParse(TxtALL_MONEY.Text.Replace(",", ""), dec) = True Then
            Me.TxtALL_MONEY.Text = dec.ToString("#,##0")
        End If

        If Decimal.TryParse(TxtALL_COUNT.Text.Replace(",", ""), dec) = True Then
            Me.TxtALL_COUNT.Text = dec.ToString("#,##0")
        End If

        If TxtTBOXID.Text = "特別保守費用" OrElse
            TxtTBOXID.Text = "緊急運営輸送費" OrElse
                TxtTBOXID.Text = "修理・有償部品費" Then
            TxtMAINTE_RATE.Text = String.Empty
            '    TxtALL_COUNT.Text = String.Empty
            '    TxtID_N_MONEY.Text = ""
            '    TxtID_L_MONEY.Text = ""
            '    TxtIC_MONEY.Text = ""
            '    TxtLUTERNA_MONEY.Text = ""
            '    TxtID_N_COUNT.Text = ""
            '    TxtID_L_COUNT.Text = ""
            '    TxtIC_COUNT.Text = ""
            '    TxtLUTERNA_COUNT.Text = ""
        End If

        If TxtTBOXID.Text = "緊急運営輸送費" Then
            TxtMAINTE_RATE.Text = String.Empty
            '    TxtALL_COUNT.Text = String.Empty
            '    TxtID_N_MONEY.Text = ""
            '    TxtID_L_MONEY.Text = ""
            '    TxtIC_MONEY.Text = ""
            '    TxtLUTERNA_MONEY.Text = ""
            '    TxtID_N_COUNT.Text = ""
            '    TxtID_L_COUNT.Text = ""
            '    TxtIC_COUNT.Text = ""
            '    TxtLUTERNA_COUNT.Text = ""
        End If

        If TxtTBOXID.Text = "修理・有償部品費" Then
            TxtMAINTE_RATE.Text = String.Empty
            '    TxtALL_COUNT.Text = String.Empty
            '    TxtID_N_MONEY.Text = ""
            '    TxtID_L_MONEY.Text = ""
            '    TxtIC_MONEY.Text = ""
            '    TxtLUTERNA_MONEY.Text = ""
            '    TxtID_N_COUNT.Text = ""
            '    TxtID_L_COUNT.Text = ""
            '    TxtIC_COUNT.Text = ""
            '    TxtLUTERNA_COUNT.Text = ""
        End If



        'If Decimal.TryParse(TxtMAINTE_RATE.Text, dec) = True Then
        '    Me.TxtMAINTE_RATE.Text = dec.ToString("#,##0")
        'End If

        'If Decimal.TryParse(TxtID_N_MONEY.Text, dec) = True Then
        '    Me.TxtID_N_MONEY.Text = dec.ToString("#,##0")
        'End If

        'If Decimal.TryParse(TxtID_L_MONEY.Text, dec) = True Then
        '    Me.TxtID_L_MONEY.Text = dec.ToString("#,##0")
        'End If

        'If Decimal.TryParse(TxtIC_MONEY.Text, dec) = True Then
        '    Me.TxtIC_MONEY.Text = dec.ToString("#,##0")
        'End If

        'If Decimal.TryParse(TxtLUTERNA_MONEY.Text, dec) = True Then
        '    Me.TxtLUTERNA_MONEY.Text = dec.ToString("#,##0")
        'End If

        'If Decimal.TryParse(TxtLUTERNA_MONEY.Text, dec) = True Then
        '    Me.TxtLUTERNA_MONEY.Text = dec.ToString("#,##0")
        'End If

        '個別件数
        If Integer.TryParse(TxtALL_COUNT.Text, int) = True Then
            intALL_Count += int
        End If
    End Sub

    Private Sub GroupFooter1_Format(sender As Object, e As EventArgs) Handles GroupFooter1.Format
        TxtALL_COUNT_TTL.Text = intID_N_Count + intID_L_Count + intIC_Count + intLUTERNA_Count
        'TxtALL_COUNT_TTL.Text = intALL_Count
        TxtID_N_MONEY_TTL.Text = (intID_N_Money).ToString
        TxtID_L_MONEY_TTL.Text = (intID_L_Money).ToString
        TxtIC_MONEY_TTL.Text = (intIC_Money).ToString
        TxtLUTERNA_MONEY_TTL.Text = (intLUTERNA_Money).ToString
        TxtALL_MONEY_TTL.Text = (intAllMoney).ToString

        'TxtDISCOUNT.Text = Math.Floor(Decimal.Parse(TxtALL_MONEY_TTL.Text) * 0.02).ToString
        'TxtTAX_TARGET.Text = Math.Floor(Decimal.Parse(TxtALL_MONEY_TTL.Text) - Decimal.Parse(TxtDISCOUNT.Text)).ToString

        Dim int As Integer = 0
        Dim dec As Decimal = 0

        'If IsNumeric(TxtTax.Text) = True Then
        'If Integer.TryParse(TxtTAX.Text, int) = True Then
        '    TxtTAX.Text = Math.Floor(Decimal.Parse(TxtTAX_TARGET.Text) * Decimal.Parse(TxtTAX.Text)).ToString
        'Else
        '    TxtTAX.Text = 0
        'End If


        'TxtTOTAL.Text = Math.Floor(Decimal.Parse(TxtTAX_TARGET.Text) + Decimal.Parse(TxtTAX.Text)).ToString
    End Sub



    Private Sub GroupFooter1_BeforePrint(sender As Object, e As EventArgs) Handles GroupFooter1.BeforePrint

        Dim dec As Decimal = 0

        If Not TxtINSAPP_CHARGE.Text = String.Empty Then
            TxtINSAPP_CHARGE.Text = TxtINSAPP_CHARGE.Text + " 殿"
        End If

        If Decimal.TryParse(TxtID_N_MONEY_TTL.Text, dec) = True Then
            Me.TxtID_N_MONEY_TTL.Text = dec.ToString("#,##0")
        End If

        If Decimal.TryParse(TxtID_L_MONEY_TTL.Text, dec) = True Then
            Me.TxtID_L_MONEY_TTL.Text = dec.ToString("#,##0")
        End If

        If Decimal.TryParse(TxtIC_MONEY_TTL.Text, dec) = True Then
            Me.TxtIC_MONEY_TTL.Text = dec.ToString("#,##0")
        End If

        If Decimal.TryParse(TxtLUTERNA_MONEY_TTL.Text, dec) = True Then
            Me.TxtLUTERNA_MONEY_TTL.Text = dec.ToString("#,##0")
        End If

        If Decimal.TryParse(TxtALL_MONEY_TTL.Text, dec) = True Then
            Me.TxtALL_MONEY_TTL.Text = dec.ToString("#,##0")
        End If


        If Decimal.TryParse(TxtDISCOUNT.Text, dec) = True Then
            Me.TxtDISCOUNT.Text = dec.ToString("#,##0")
        End If




        If Decimal.TryParse(TxtTAX_TARGET.Text, dec) = True Then
            Me.TxtTAX_TARGET.Text = dec.ToString("#,##0")
        End If

        If Decimal.TryParse(TxtLUTERNA_MONEY_TTL.Text, dec) = True Then
            Me.TxtLUTERNA_MONEY_TTL.Text = dec.ToString("#,##0")
        End If

        If Decimal.TryParse(TxtALL_MONEY_TTL.Text, dec) = True Then
            Me.TxtALL_MONEY_TTL.Text = dec.ToString("#,##0")
        End If


        If Decimal.TryParse(TxtTOTAL.Text, dec) = True Then
            Me.TxtTOTAL.Text = dec.ToString("#,##0")
        End If
        If Decimal.TryParse(TxtALL_COUNT_TTL.Text, dec) = True Then
            Me.TxtALL_COUNT_TTL.Text = dec.ToString("#,##0")
        End If

        TxtDISCOUNT_RATE.Text = Decimal.Parse(TxtDISCOUNT_RATE.Text).ToString("f")


    End Sub
End Class
