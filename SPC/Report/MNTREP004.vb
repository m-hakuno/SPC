Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class MNTREP004
    Dim mintNo As Integer = 0
    Dim intDataCnt As Integer = 0
    Dim intRepDataCount As Integer = 0

    Public Property RepDataCount()
        Get
            Return intRepDataCount
        End Get
        Set(value)
            intRepDataCount = value
        End Set
    End Property

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        mintNo += 1
        Me.TxtNO.Text = mintNo.ToString
        '-----------------------------
        '2014/04/11 Hamamoto　ここから
        '-----------------------------
        intDataCnt += 1

        '小文字に設定
        If Not TxtCLS.Text Is Nothing OrElse TxtCLS.Text.Trim() <> "" Then
            TxtCLS.Text = Microsoft.VisualBasic.StrConv(TxtCLS.Text, Microsoft.VisualBasic.VbStrConv.Narrow)
        Else
            TxtCLS.Text = ""
        End If

        '区切りの時、数値を見せない。
        If lblKugiri.Text = "1" Then
            TxtQUANTITY.Visible = False
        Else
            TxtQUANTITY.Visible = True
        End If

        If intDataCnt < 20 OrElse intRepDataCount = mintNo Then
            Detail1.NewPage = SectionReportModel.NewPage.None
            Line27.LineWeight = 1
        Else
            Detail1.NewPage = SectionReportModel.NewPage.After
            Line27.LineWeight = 2
            intDataCnt = 0
        End If

        'If TxtCLS.Text = "1" Then
        '    TxtCLS.Text = "OK"
        'ElseIf TxtCLS.Text = "2" Then
        '    TxtCLS.Text = "NG"
        'Else
        '    TxtCLS.Text = ""
        'End If
        '-----------------------------
        '2014/04/11 Hamamoto　ここまで
        '-----------------------------

    End Sub

    '-----------------------------
    '2014/04/11 Hamamoto　ここから
    '-----------------------------
    'Private Sub ReportHeader1_Format(sender As Object, e As EventArgs)
    '    Dim ci As New System.Globalization.CultureInfo("ja-JP")
    '    Dim jp As New System.Globalization.JapaneseCalendar
    '    Dim dt As System.DateTime
    '    ' 使用する暦を、和暦に設定
    '    ci.DateTimeFormat.Calendar = jp

    '    '西暦を和暦に変換する
    '    If Date.TryParse(TextBox1.Text, dt) = True Then
    '        dt = DateTime.Parse(TextBox1.Text)
    '        TextBox1.Text = dt.ToString("ggyy年MM月dd日", ci)
    '    End If
    '    If Date.TryParse(TxtDELIVERY_D.Text, dt) = True Then
    '        dt = DateTime.Parse(TxtDELIVERY_D.Text)
    '        TxtDELIVERY_D.Text = dt.ToString("ggyy年MM月dd日", ci)
    '    End If
    'End Sub

    Private Sub GroupHeader1_Format(sender As Object, e As EventArgs) Handles GroupHeader1.Format
        Dim ci As New System.Globalization.CultureInfo("ja-JP")
        Dim jp As New System.Globalization.JapaneseCalendar
        Dim dt As System.DateTime
        ' 使用する暦を、和暦に設定
        ci.DateTimeFormat.Calendar = jp

        '西暦を和暦に変換する
        If Date.TryParse(TextBox1.Text, dt) = True Then
            dt = DateTime.Parse(TextBox1.Text)
            'TextBox1.Text = dt.ToString("ggyy年MMMMd日", ci)
            TextBox1.Text = dt.ToString("yyyy年MMMMd日")
        Else
            TextBox1.Text = ""
        End If
        If Date.TryParse(TxtDELIVERY_D.Text, dt) = True Then
            dt = DateTime.Parse(TxtDELIVERY_D.Text)
            'TxtDELIVERY_D.Text = dt.ToString("ggyy年MM月dd日", ci)
            TxtDELIVERY_D.Text = dt.ToString("yyyy年MM月dd日")
        Else
            TxtDELIVERY_D.Text = ""
        End If

    End Sub
    '-----------------------------
    '2014/04/11 Hamamoto　ここまで
    '-----------------------------

    'Private Sub Detail1_AfterPrint(sender As Object, e As EventArgs) Handles Detail1.AfterPrint
    '    Try
    '        If mintNo Mod 20 = 0 Then
    '            Detail1.NewPage = SectionReportModel.NewPage.After
    '        Else
    '            Detail1.NewPage = SectionReportModel.NewPage.None
    '        End If
    '    Catch ex As Exception
    '        Dim strErr As String = ""
    '    End Try

    'End Sub
End Class
