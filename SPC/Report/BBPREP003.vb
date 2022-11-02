Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class BBPREP003

    Dim SbRep As New BBPREP003_2
    Dim intDataCnt As Integer = 0
    Dim intCounter As Integer = 3 '行番号集計用の変数。
    Dim strBuff As String = ""
    Dim intMonth As Integer = 4

    Private Sub Detail_Format(ByVal sender As Object, ByVal e As System.EventArgs) Handles Detail.Format

        intDataCnt += 1

        intCounter += 1
        If intCounter = 13 Then intCounter = 1
        txtMonth.Value = intCounter.ToString("00") & "月"

        lblEast.Value = "東日本"
        lblWest.Value = "西日本"
        lblDetTTL.Value = "計"

    End Sub

    Private Sub msSetDate(ByVal objTxt As GrapeCity.ActiveReports.SectionReportModel.TextBox)
        Dim dtVal As DateTime = Nothing

        If DateTime.TryParse(objTxt.Text, dtVal) Then
            objTxt.Value = dtVal
        Else
            objTxt.Text = ""
        End If
    End Sub

    Private Sub BBPREP003_ReportStart(sender As Object, e As EventArgs) Handles MyBase.ReportStart
        Dim dtData As DataTable
        Dim dtData2 As DataTable
        Dim intIndex As Integer

        dtData = Me.DataSource
        dtData2 = dtData.Copy
        For intIndex = dtData.Rows.Count - 1 To 1 Step -1
            dtData.Rows(intIndex).Delete()
        Next

        SbRep.DataSource = dtData2
        Me.BBPREP003_2.Report = SbRep

    End Sub

    Private Sub GroupHeader1_Format(sender As Object, e As EventArgs) Handles GroupHeader1.Format
        Dim ci As New System.Globalization.CultureInfo("ja-JP")
        Dim jp As New System.Globalization.JapaneseCalendar
        Dim dt As System.DateTime
        ' 使用する暦を、和暦に設定
        ci.DateTimeFormat.Calendar = jp

        '西暦表示
        If Date.TryParse(txtYear.Text & "/01/01", dt) = True Then
            dt = DateTime.Parse(txtYear.Text & "/01/01")
            txtYear.Text = dt.ToString("yyyy年")
            txtNextYear.Text = dt.AddYears(1).ToString("yyyy年")
        End If
        ''西暦を和暦に変換する
        'If Date.TryParse(txtYear.Text & "/01/01", dt) = True Then
        '    dt = DateTime.Parse(txtYear.Text & "/01/01")
        '    txtYear.Text = dt.ToString("gg yy年", ci)
        '    txtNextYear.Text = dt.AddYears(1).ToString("gg yy年", ci)
        'End If

    End Sub
End Class

