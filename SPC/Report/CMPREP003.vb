Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document 
Imports GrapeCity.ActiveReports.SectionReportModel

Public Class CMPREP003

    Private intRowCount As Integer  '行カウント
    Private intPageCount As Integer 'ページカウント
    Private objDt As DataTable      'データテーブルクラス
    Private intTotalPage As Integer '総ページ数
    Const C_LINENUM As Integer = 10

    Private Sub CMPREP003_ReportStart(sender As Object, e As EventArgs) Handles Me.ReportStart

        'データテーブル設定
        objDt = Me.DataSource

        '総ページ数算出
        If objDt.Rows.Count Mod C_LINENUM = 0 Then
            intTotalPage = objDt.Rows.Count / C_LINENUM
        Else
            intTotalPage = objDt.Rows.Count / C_LINENUM + 1
        End If

        '出力項目初期化処理
        msInitPrintitem()

        '和暦を表すクラス
        Dim ci As New System.Globalization.CultureInfo("ja-JP")
        Dim jp As New System.Globalization.JapaneseCalendar
        ci.DateTimeFormat.Calendar = jp

        '対応年月を表示
        If (Me.Fields("対応年月").Value).ToString = "1900/01/01 0:00:00" Then
            Me.txtYear.Text = ""
            Me.txtMonth.Text = ""
        Else
            Me.txtYear.Text = CDate(Me.Fields("対応年月").Value).ToString("yyyy年")
            '            Me.txtYear.Text = CDate(Me.Fields("対応年月").Value).ToString("ggy年", ci)
            Me.txtMonth.Text = CDate(Me.Fields("対応年月").Value).ToString("M月")
        End If

    End Sub

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        'If Me.Fields("ホール名TBOXID").Value.ToString.Trim = "/" Then
        'Me.txtTboxID.Text = String.Empty
        'End If

        If Me.Fields("故障状況対応内容").Value.ToString.Trim = "/" Then
            Me.txtRptDeal.Text = String.Empty
        End If

        'If Me.txtStatusCD.Text <> "" Then
        '    Me.txtTotalGb.Text = (Integer.Parse(Me.txtTotalGb.Text) + Integer.Parse(Me.txtStatusCD.Text)).ToString
        'End If
        'If Me.txtGbTM.Text <> "" Then
        '    Me.txtTotalGb.Text = (Integer.Parse(Me.txtTotalGb.Text) + Integer.Parse(Me.txtGbTM.Text)).ToString
        'End If

        intRowCount += 1

        '改ページ判定
        If intRowCount = C_LINENUM Then
            '最終ページ判定
            If intTotalPage <> intPageCount Then
                '改ページ
                Me.Detail1.NewPage = NewPage.After
                intRowCount = 0
                intPageCount += 1
            Else
                Me.Detail1.NewPage = NewPage.None
            End If
        Else
            Me.Detail1.NewPage = NewPage.None
        End If
        If txtMntNo.Text = String.Empty And txtReqPrice.Text = 0 Then
            Me.txtTotalPsn.Text = 0
        Else
            Me.txtTotalPsn.Text = objDt.Rows.Count
        End If

    End Sub

    Private Sub msInitPrintitem()

        'レポートヘッダー
        Me.txtYear.Text = String.Empty
        Me.txtMonth.Text = String.Empty

        '明細
        Me.txtDistrictNM.Text = String.Empty
        Me.txtReqDay.Text = String.Empty
        Me.txtReqHour.Text = String.Empty
        Me.txtReqMinute.Text = String.Empty
        Me.txtStartDt.Text = String.Empty
        Me.txtDeptHour.Text = String.Empty
        Me.txtDeptMinute.Text = String.Empty
        Me.txtStartHour.Text = String.Empty
        Me.txtStartMinute.Text = String.Empty
        Me.txtEndHour.Text = String.Empty
        Me.txtEndMinute.Text = String.Empty
        Me.txtOfficeNM.Text = String.Empty
        Me.txtMntNo.Text = String.Empty
        Me.txtTboxID.Text = String.Empty
        Me.txtRptDeal.Text = String.Empty
        Me.txtGbTM.Text = String.Empty
        Me.txtStatusCD.Text = String.Empty
        Me.txtGbTM.Text = String.Empty
        Me.txtPsnNum.Text = String.Empty
        Me.txtReqPrice.Text = String.Empty
        Me.txtSubmitDt.Text = String.Empty

        'グループヘッダー
        Me.txtTotalGb.Text = ""
        Me.txtTotalPsn.Text = String.Empty
        Me.txtTotalPrice.Text = String.Empty
        Me.txtOutputDt.Text = String.Empty

        '行カウント
        intRowCount = 0

        'ページカウント
        intPageCount = 1

    End Sub

End Class
