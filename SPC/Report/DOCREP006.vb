Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class DOCREP006

    Dim intDataCnt As Integer = 0
    Dim mintNo As Integer = 0
    Dim intCnt As Integer = 0

    '-----------------------------
    '2014/04/11 Hamamoto　ここから
    '-----------------------------
    Public Property RepDataCount() As Integer
        Get
            Return intDataCnt
        End Get
        Set(value As Integer)
            intDataCnt = value
        End Set
    End Property

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format
        mintNo += 1
        intCnt += 1
        Me.TxtNo.Text = mintNo.ToString

        If intCnt < 20 OrElse mintNo = intDataCnt Then
            Detail1.NewPage = SectionReportModel.NewPage.None
        Else
            Detail1.NewPage = SectionReportModel.NewPage.After
            intCnt = 0
        End If

        '空行は非表示
        If lblEmpty.Text = "1" Then
            TxtPART_CD.Visible = False
            TxtPART_NM.Visible = False
            TxtPRICE.Visible = False
            TxtQUANTITY.Visible = False
            TxtSUBTOTAL.Visible = False
        Else
            TxtPART_CD.Visible = True
            TxtPART_NM.Visible = True
            TxtPRICE.Visible = True
            TxtQUANTITY.Visible = True
            TxtSUBTOTAL.Visible = True
        End If
        '-----------------------------
        '2014/04/11 Hamamoto　ここまで
        '-----------------------------
    End Sub

    Private Sub Detail1_biforeprint(sender As Object, e As EventArgs) Handles Detail1.BeforePrint

        'TextBox21.Text = Math.Floor(Decimal.Parse(TextBox20.Text) * Decimal.Parse(TextBox19.Text)).ToString("#,#")
        'TextBox19.Text = Math.Floor(Decimal.Parse(TextBox19.Text) * 1).ToString
        'TextBox15.Text = Math.Floor(Decimal.Parse(TextBox15.Text) * 1).ToString
        'TextBox16.Text = Math.Floor(Decimal.Parse(TextBox16.Text) * 1).ToString
    End Sub

    Private Sub GroupFooter1_BeforePrint(sender As Object, e As EventArgs) Handles GroupFooter1.BeforePrint

        TxtTOTAL.Text = Math.Floor(Decimal.Parse(TxtTOTAL.Text) * 1).ToString("#,#")

    End Sub

    Private Sub GroupHeader1_Format(sender As Object, e As EventArgs) Handles GroupHeader1.Format
        Dim ci As New System.Globalization.CultureInfo("ja-JP")
        Dim jp As New System.Globalization.JapaneseCalendar
        Dim dt As System.DateTime
        ' 使用する暦を、和暦に設定
        ci.DateTimeFormat.Calendar = jp
        '-----------------------------
        '2014/04/11 Hamamoto　ここから
        '-----------------------------
        '西暦を和暦に変換する
        If Date.TryParse(TextBox17.Text, dt) = True Then
            dt = DateTime.Parse(TextBox17.Text)
            '8/7書式変更　栗原ここから
            'TextBox17.Text = dt.ToString("gg yy年 MMMM d日", ci)
            'TextBox17.Text = dt.ToString("ggyy年M月d日", ci)
            TextBox17.Text = dt.ToString("yyyy年M月d日")
            'ここまで
        Else
            TextBox17.Text = ""
        End If
        If Date.TryParse(TextBox4.Text, dt) = True Then
            dt = DateTime.Parse(TextBox4.Text)
            'TextBox4.Text = dt.ToString("ggyy年", ci) & dt.ToString("MMMM", ci)
            TextBox4.Text = dt.ToString("yyyy年") & dt.ToString("MM月")
        Else
            TextBox4.Text = ""
        End If

        Dim strVal As String = lblNendo.Text

        If DateTime.TryParse(strVal, dt) Then
            If dt.Month < 10 Then
                TxtMONTH.Text = " " & String.Format("{0:MMMM}", dt) & "度"
            Else
                TxtMONTH.Text = String.Format("{0:MMMM}", dt) & "度"
            End If
        Else
            TxtMONTH.Text = ""
        End If

        strVal = TxtVERSION.Text
        TxtSYSTEM.Text = "T-BOX(" & strVal & ")"

        If TxtUNIT.Text <> "" Then
            TxtUNIT.Text = TxtUNIT.Text & " 殿"
        End If

        '-----------------------------
        '2014/04/11 Hamamoto　ここまで
        '-----------------------------
    End Sub
End Class
