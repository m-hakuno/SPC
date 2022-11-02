Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class REPREP058

    Private Sub PageHeader1_BeforePrint(sender As Object, e As EventArgs) Handles PageHeader1.BeforePrint
        Dim ci As New System.Globalization.CultureInfo("ja-JP")
        Dim jp As New System.Globalization.JapaneseCalendar
        Dim dt As System.DateTime

        ' 使用する暦を、和暦に設定
        ci.DateTimeFormat.Calendar = jp

        '西暦を和暦に変換する
        'If IsDate(TxtDate.Text) = True Then
        If Date.TryParse(TxtDate.Text, dt) = True Then
            dt = DateTime.Parse(TxtDate.Text)
            'TxtDate.Text = dt.ToString("ggyy年MM月分", ci)
            TxtDate.Text = dt.ToString("yyyy年MM月分")
        End If
    End Sub

    Private Sub Detail1_BeforePrint(sender As Object, e As EventArgs) Handles Detail1.BeforePrint
        Dim intMoney As Integer
        Dim intCount As Integer
        Dim decVal As Decimal = 0

        '合計の算出
        intCount = 0
        'If IsNumeric(TxtID_N_COUNT_R.Text) = True Then intCount += TxtID_N_COUNT_R.Value
        If Decimal.TryParse(TxtID_N_COUNT_R.Text, decVal) = True Then intCount += TxtID_N_COUNT_R.Value
        'If IsNumeric(TxtID_L_COUNT_R.Text) = True Then intCount += TxtID_L_COUNT_R.Value
        If Decimal.TryParse(TxtID_L_COUNT_R.Text, decVal) = True Then intCount += TxtID_L_COUNT_R.Value
        'If IsNumeric(TxtIC_COUNT_R.Text) = True Then intCount += TxtIC_COUNT_R.Value
        If Decimal.TryParse(TxtIC_COUNT_R.Text, decVal) = True Then intCount += TxtIC_COUNT_R.Value
        'If IsNumeric(TxtLUTERNA_COUNT_R.Text) = True Then intCount += TxtLUTERNA_COUNT_R.Value
        If Decimal.TryParse(TxtLUTERNA_COUNT_R.Text, decVal) = True Then intCount += TxtLUTERNA_COUNT_R.Value
        TxtTOTAL_COUNT_R.Value = intCount

        intCount = 0
        'If IsNumeric(TxtID_N_COUNT_P.Text) = True Then intCount += TxtID_N_COUNT_P.Value
        If Decimal.TryParse(TxtID_N_COUNT_P.Text, decVal) = True Then intCount += TxtID_N_COUNT_P.Value
        'If IsNumeric(TxtID_L_COUNT_P.Text) = True Then intCount += TxtID_L_COUNT_P.Value
        If Decimal.TryParse(TxtID_L_COUNT_P.Text, decVal) = True Then intCount += TxtID_L_COUNT_P.Value
        'If IsNumeric(TxtIC_COUNT_P.Text) = True Then intCount += TxtIC_COUNT_P.Value
        If Decimal.TryParse(TxtIC_COUNT_P.Text, decVal) = True Then intCount += TxtIC_COUNT_P.Value
        'If IsNumeric(TxtLUTERNA_COUNT_P.Text) = True Then intCount += TxtLUTERNA_COUNT_P.Value
        If Decimal.TryParse(TxtLUTERNA_COUNT_P.Text, decVal) = True Then intCount += TxtLUTERNA_COUNT_P.Value
        TxtTOTAL_COUNT_P.Value = intCount

        intMoney = 0
        'If IsNumeric(TxtID_N_MONEY_R.Text) = True Then intMoney += TxtID_N_MONEY_R.Value
        If Decimal.TryParse(TxtID_N_MONEY_R.Text, decVal) = True Then intMoney += TxtID_N_MONEY_R.Value
        'If IsNumeric(TxtID_L_MONEY_R.Text) = True Then intMoney += TxtID_L_MONEY_R.Value
        If Decimal.TryParse(TxtID_L_MONEY_R.Text, decVal) = True Then intMoney += TxtID_L_MONEY_R.Value
        'If IsNumeric(TxtIC_MONEY_R.Text) = True Then intMoney += TxtIC_MONEY_R.Value
        If Decimal.TryParse(TxtIC_MONEY_R.Text, decVal) = True Then intMoney += TxtIC_MONEY_R.Value
        'If IsNumeric(TxtLUTERNA_MONEY_R.Text) = True Then intMoney += TxtLUTERNA_MONEY_R.Value
        If Decimal.TryParse(TxtLUTERNA_MONEY_R.Text, decVal) = True Then intMoney += TxtLUTERNA_MONEY_R.Value
        TxtTOTAL_MONEY_R.Value = intMoney

        intMoney = 0
        'If IsNumeric(TxtID_N_MONEY_P.Text) = True Then intMoney += TxtID_N_MONEY_P.Value
        If Decimal.TryParse(TxtID_N_MONEY_P.Text, decVal) = True Then intMoney += TxtID_N_MONEY_P.Value
        'If IsNumeric(TxtID_L_MONEY_P.Text) = True Then intMoney += TxtID_L_MONEY_P.Value
        If Decimal.TryParse(TxtID_L_MONEY_P.Text, decVal) = True Then intMoney += TxtID_L_MONEY_P.Value
        'If IsNumeric(TxtIC_MONEY_P.Text) = True Then intMoney += TxtIC_MONEY_P.Value
        If Decimal.TryParse(TxtIC_MONEY_P.Text, decVal) = True Then intMoney += TxtIC_MONEY_P.Value
        'If IsNumeric(TxtLUTERNA_MONEY_P.Text) = True Then intMoney += TxtLUTERNA_MONEY_P.Value
        If Decimal.TryParse(TxtLUTERNA_MONEY_P.Text, decVal) = True Then intMoney += TxtLUTERNA_MONEY_P.Value
        TxtTOTAL_MONEY_P.Value = intMoney

        intMoney = 0
        'If IsNumeric(TxtTOTAL_MONEY_R.Text) = True Then intMoney += TxtTOTAL_MONEY_R.Value
        If Decimal.TryParse(TxtTOTAL_MONEY_R.Text, decVal) = True Then intMoney += TxtTOTAL_MONEY_R.Value
        'If IsNumeric(TxtTOTAL_MONEY_P.Text) = True Then intMoney += TxtTOTAL_MONEY_P.Value
        If Decimal.TryParse(TxtTOTAL_MONEY_P.Text, decVal) = True Then intMoney += TxtTOTAL_MONEY_P.Value
        TxtTOTAL_MONEY.Value = intMoney
    End Sub

End Class
