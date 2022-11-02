Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document
Imports SPC.ClsCMReport

Public Class CMPREP001
    Dim TypeStart As Integer = 0
    Dim TypeCnt As Integer = 0
    Dim Title As String
    Public Property ppTitle As String
        Get
            Return Title
        End Get
        Set(value As String)
            Title = value
        End Set
    End Property

    Private Sub CMPREP001_ReportStart(sender As Object, e As EventArgs) Handles Me.ReportStart
        'セッション情報の取得
        'Dim FsTtl As Integer = HttpContext.Current.Session("FsTtlCnt")
        'Dim CsTtl As Integer = HttpContext.Current.Session("CsTtlCnt")
        'Dim OthTtl As Integer = HttpContext.Current.Session("OthTtlCnt")
        Dim dtbAggDt As Date = HttpContext.Current.Session("dtbAggDt")  '集計日
        Dim FsCnt() As Integer = HttpContext.Current.Session("FsCnt")
        Dim FsPrice() As Integer = HttpContext.Current.Session("FsPrice")
        Dim CsCnt() As Integer = HttpContext.Current.Session("CsCnt")
        Dim CsPrice() As Integer = HttpContext.Current.Session("CsPrice")
        Dim OthCnt() As Integer = HttpContext.Current.Session("OthCnt")
        Dim OthPrice() As Integer = HttpContext.Current.Session("OthPrice")

        'システム区分の分類分け
        Select Case Title
            Case "保守料金明細作成一覧表　ID"
                TypeStart = 0
                TypeCnt = HttpContext.Current.Session("IDTypeCnt")
                TxtSystemCls.Text = "ID"
            Case "保守料金明細作成一覧表　IC"
                TypeStart = HttpContext.Current.Session("IDTypeCnt")
                TypeCnt = HttpContext.Current.Session("ICTypeCnt")
                TxtSystemCls.Text = "IC"
            Case "保守料金明細作成一覧表　LUTERNA"
                TypeStart = HttpContext.Current.Session("IDTypeCnt") + HttpContext.Current.Session("ICTypeCnt")
                TypeCnt = HttpContext.Current.Session("LTTypeCnt")
                TxtSystemCls.Text = "LUTERNA"
            Case "保守料金明細作成一覧表"
                '過去帳票（システム分類での帳票分割未実施）の印刷

                '全システムをIDとして集計する
                TypeStart = 0
                TypeCnt = HttpContext.Current.Session("IDTypeCnt")

                '印字項目「システム分類」を消す
                lblSystemCls.Visible = False
                TxtSystemCls.Visible = False
                txtCln.Visible = False 'コロン（：）

                'フォントスタイル（サイズ）の再設定（10pt → 8pt）
                lblClctDt.Style = "font-size: 8pt; text-align: left; vertical-align: top; ddo-char-set: 1"
                TxtClctDt.Style = "font-size: 8pt; text-align: left; vertical-align: top; ddo-char-set: 1"
                txtCln2.Style = "font-size: 8pt; text-align: left; vertical-align: top; ddo-char-set: 1"

                'フォントサイズを小さくした分、印字文字を調整する。
                lblClctDt.Text = "集計日："           '「集　計　日」→「集計日：」
                lblClctDt.Width = CmToInch(1.2)       '1.2cmは修正前の同コントロール幅
                txtCln2.Visible = False               'コロン（：）は集計日のラベルに直書きしたので消す
                '「集計日」の印字位置を調整
                'コントロールの左端を「集計日ラベル」の右端に合わせる
                TxtClctDt.Left = lblClctDt.Left + lblClctDt.Width
            Case Else


        End Select

        '○保守料金明細一覧表     
        '×保守料金明細作成一覧表
        lblTitle.Text = "保守料金明細一覧表"

        '集計日の設定
        TxtClctDt.Text = dtbAggDt.ToString("yyyy年MM月dd日")

        'ページ上部「総合計」計算
        TxtOthTtl.Text = 0
        'For intcnt As Integer = 0 To OthCnt.Count - 1
        For intcnt As Integer = TypeStart To TypeStart + TypeCnt - 1
            TxtOthTtl.Text += OthCnt(intcnt)
        Next
        'TxtOthTtl.Text = String.Format("{0:#,##0}", TxtOthTtl.Text)
        TxtOthTtl.Text = CType(TxtOthTtl.Text, Integer).ToString("#,##0")

        '列幅の計算（1枚目のみ）
        If bln = False Then
            dt = Me.DataSource
            clmCnt = (dt.Columns.Count - 1) / 2
            'If clmCnt > 9 Then
            '    clmW = 25.7 / 9
            'Else
            '    clmW = 25.7 / clmCnt
            'End If
            '固定幅に変更　2016.02.20
            clmW = 25.7 / 9
            numW = clmW / 5 * 2
            prcW = clmW / 5 * 3
            Dim tmp As Integer = clmCnt Mod 9
            If tmp = 0 Then
                pgCnt = clmCnt / 9
            Else
                pgCnt = Math.Truncate(clmCnt / 9) + 1
            End If
        End If

        If pgCnt <> 0 Then
            Dim tmp As Integer = 0
            If pgCnt <> 1 Then
                For zz = (pg - 1) * 9 To pg * 9 - 1
                    'PageHeader追加
                    Dim lbl As New GrapeCity.ActiveReports.SectionReportModel.Label
                    PageHeader1.Controls.Add(lbl)
                    lbl.Text = dt.Columns(zz * 2 + 1).ToString.Trim.Replace("_件数", "")
                    lbl.Height = GrapeCity.ActiveReports.SectionReport.CmToInch(0.5F)
                    lbl.Width = GrapeCity.ActiveReports.SectionReport.CmToInch(clmW)
                    lbl.Alignment = Section.TextAlignment.Center
                    lbl.Alignment = Section.TextAlignment.Center
                    lbl.VerticalAlignment = Section.VerticalTextAlignment.Middle
                    lbl.Font = New System.Drawing.Font("", 8)
                    locX = 2.3 + clmW * tmp
                    lbl.Location = New System.Drawing.PointF(GrapeCity.ActiveReports.SectionReport.CmToInch(locX), GrapeCity.ActiveReports.SectionReport.CmToInch(3.2F))
                    lbl.Border.Style = BorderLineStyle.Solid
                    'Detail追加
                    Dim txtCnt As New GrapeCity.ActiveReports.SectionReportModel.TextBox
                    Detail1.Controls.Add(txtCnt)
                    txtCnt.OutputFormat = "#,##0"
                    txtCnt.DataField = dt.Columns(zz * 2 + 1).ToString
                    txtCnt.Height = GrapeCity.ActiveReports.SectionReport.CmToInch(0.5F)
                    txtCnt.Width = GrapeCity.ActiveReports.SectionReport.CmToInch(numW)
                    txtCnt.Alignment = Section.TextAlignment.Right
                    txtCnt.VerticalAlignment = Section.VerticalTextAlignment.Middle
                    txtCnt.Font = New System.Drawing.Font("", 8)
                    locX = 2.3 + numW * tmp + prcW * tmp
                    txtCnt.Location = New System.Drawing.PointF(GrapeCity.ActiveReports.SectionReport.CmToInch(locX), "0")
                    txtCnt.Border.Style = BorderLineStyle.Solid

                    Dim txtPrc As New GrapeCity.ActiveReports.SectionReportModel.TextBox
                    Detail1.Controls.Add(txtPrc)
                    txtPrc.OutputFormat = "#,##0"
                    txtPrc.DataField = dt.Columns(zz * 2 + 2).ToString
                    txtPrc.Height = GrapeCity.ActiveReports.SectionReport.CmToInch(0.5F)
                    txtPrc.Width = GrapeCity.ActiveReports.SectionReport.CmToInch(prcW)
                    txtPrc.Alignment = Section.TextAlignment.Right
                    txtPrc.VerticalAlignment = Section.VerticalTextAlignment.Middle
                    txtPrc.Font = New System.Drawing.Font("", 8)
                    locX = 2.3 + numW * (tmp + 1) + prcW * tmp
                    txtPrc.Location = New System.Drawing.PointF(GrapeCity.ActiveReports.SectionReport.CmToInch(locX), "0")
                    txtPrc.Border.Style = BorderLineStyle.Solid

                    'ReportFooter追加
                    For yy = 0 To 0
                        '件数
                        Dim txtTNum As New GrapeCity.ActiveReports.SectionReportModel.TextBox
                        ReportFooter1.Controls.Add(txtTNum)
                        Select Case yy
                            Case "0"
                                txtTNum.Text = String.Format("{0:#,##0}", FsCnt(zz + TypeStart) + CsCnt(zz + TypeStart) + OthCnt(zz + TypeStart))
                            Case "1"
                                txtTNum.Text = String.Format("{0:#,##0}", FsCnt(zz))
                            Case "2"
                                txtTNum.Text = String.Format("{0:#,##0}", CsCnt(zz))
                            Case "3"
                                txtTNum.Text = String.Format("{0:#,##0}", OthCnt(zz))
                        End Select
                        txtTNum.OutputFormat = "#,##0"
                        txtTNum.Height = GrapeCity.ActiveReports.SectionReport.CmToInch(0.5F)
                        txtTNum.Width = GrapeCity.ActiveReports.SectionReport.CmToInch(numW)
                        txtTNum.Alignment = Section.TextAlignment.Right
                        txtTNum.VerticalAlignment = Section.VerticalTextAlignment.Middle
                        txtTNum.Font = New System.Drawing.Font("", 8)
                        locX = 2.3 + numW * tmp + prcW * tmp
                        txtTNum.Location = New System.Drawing.PointF(GrapeCity.ActiveReports.SectionReport.CmToInch(locX), GrapeCity.ActiveReports.SectionReport.CmToInch(locY))
                        txtTNum.Border.Style = BorderLineStyle.Solid

                        '金額
                        Dim txtTPrc As New GrapeCity.ActiveReports.SectionReportModel.TextBox
                        ReportFooter1.Controls.Add(txtTPrc)
                        Select Case yy
                            Case "0"
                                txtTPrc.Text = String.Format("{0:#,##0}", FsPrice(zz + TypeStart) + CsPrice(zz + TypeStart) + OthPrice(zz + TypeStart))
                            Case "1"
                                txtTPrc.Text = String.Format("{0:#,##0}", FsPrice(zz))
                            Case "2"
                                txtTPrc.Text = String.Format("{0:#,##0}", CsPrice(zz))
                            Case "3"
                                txtTPrc.Text = String.Format("{0:#,##0}", OthPrice(zz))
                        End Select
                        txtTPrc.OutputFormat = "#,##0"
                        txtTPrc.Height = GrapeCity.ActiveReports.SectionReport.CmToInch(0.5F)
                        txtTPrc.Width = GrapeCity.ActiveReports.SectionReport.CmToInch(prcW)
                        txtTPrc.Alignment = Section.TextAlignment.Right
                        txtTPrc.VerticalAlignment = Section.VerticalTextAlignment.Middle
                        txtTPrc.Font = New System.Drawing.Font("", 8)
                        locX = 2.3 + numW * (tmp + 1) + prcW * tmp
                        txtTPrc.Location = New System.Drawing.PointF(GrapeCity.ActiveReports.SectionReport.CmToInch(locX), GrapeCity.ActiveReports.SectionReport.CmToInch(locY))
                        txtTPrc.Border.Style = BorderLineStyle.Solid
                        'If yy <> 3 Then
                        '    locY += 0.5
                        'Else
                        '    locY = 0.2
                        'End If
                    Next
                    tmp += 1
                Next
                pg += 1
                pgCnt -= 1
                bln = True
            Else
                For zz = (pg - 1) * 9 To clmCnt - 1
                    'PageHeader追加
                    Dim lbl As New GrapeCity.ActiveReports.SectionReportModel.Label
                    PageHeader1.Controls.Add(lbl)
                    lbl.Text = dt.Columns(zz * 2 + 1).ToString.Trim.Replace("_件数", "")
                    lbl.Height = GrapeCity.ActiveReports.SectionReport.CmToInch(0.5F)
                    lbl.Width = GrapeCity.ActiveReports.SectionReport.CmToInch(clmW)
                    lbl.Alignment = Section.TextAlignment.Center
                    lbl.Alignment = Section.TextAlignment.Center
                    lbl.VerticalAlignment = Section.VerticalTextAlignment.Middle
                    lbl.Font = New System.Drawing.Font("", 8)
                    locX = 2.3 + clmW * tmp
                    lbl.Location = New System.Drawing.PointF(GrapeCity.ActiveReports.SectionReport.CmToInch(locX), GrapeCity.ActiveReports.SectionReport.CmToInch(3.2F))
                    lbl.Border.Style = BorderLineStyle.Solid

                    'Detail追加
                    Dim txtCnt As New GrapeCity.ActiveReports.SectionReportModel.TextBox
                    Detail1.Controls.Add(txtCnt)
                    txtCnt.OutputFormat = "#,##0"
                    txtCnt.DataField = dt.Columns(zz * 2 + 1).ToString
                    txtCnt.Height = GrapeCity.ActiveReports.SectionReport.CmToInch(0.5F)
                    txtCnt.Width = GrapeCity.ActiveReports.SectionReport.CmToInch(numW)
                    txtCnt.Alignment = Section.TextAlignment.Right
                    txtCnt.VerticalAlignment = Section.VerticalTextAlignment.Middle
                    txtCnt.Font = New System.Drawing.Font("", 8)
                    locX = 2.3 + numW * tmp + prcW * tmp
                    txtCnt.Location = New System.Drawing.PointF(GrapeCity.ActiveReports.SectionReport.CmToInch(locX), "0")
                    txtCnt.Border.Style = BorderLineStyle.Solid

                    Dim txtPrc As New GrapeCity.ActiveReports.SectionReportModel.TextBox
                    Detail1.Controls.Add(txtPrc)
                    txtPrc.OutputFormat = "#,##0"
                    txtPrc.DataField = dt.Columns(zz * 2 + 2).ToString
                    txtPrc.Height = GrapeCity.ActiveReports.SectionReport.CmToInch(0.5F)
                    txtPrc.Width = GrapeCity.ActiveReports.SectionReport.CmToInch(prcW)
                    txtPrc.Alignment = Section.TextAlignment.Right
                    txtPrc.VerticalAlignment = Section.VerticalTextAlignment.Middle
                    txtPrc.Font = New System.Drawing.Font("", 8)
                    locX = 2.3 + numW * (tmp + 1) + prcW * tmp
                    txtPrc.Location = New System.Drawing.PointF(GrapeCity.ActiveReports.SectionReport.CmToInch(locX), "0")
                    txtPrc.Border.Style = BorderLineStyle.Solid
                    'ReportFooter追加
                    For yy = 0 To 0
                        '件数
                        Dim txtTNum As New GrapeCity.ActiveReports.SectionReportModel.TextBox
                        ReportFooter1.Controls.Add(txtTNum)
                        Select Case yy
                            Case "0"
                                txtTNum.Text = String.Format("{0:#,##0}", FsCnt(zz + TypeStart) + CsCnt(zz + TypeStart) + OthCnt(zz + TypeStart))
                            Case "1"
                                txtTNum.Text = String.Format("{0:#,##0}", FsCnt(zz))
                            Case "2"
                                txtTNum.Text = String.Format("{0:#,##0}", CsCnt(zz))
                            Case "3"
                                txtTNum.Text = String.Format("{0:#,##0}", OthCnt(zz))
                        End Select
                        txtTNum.OutputFormat = "#,##0"
                        txtTNum.Height = GrapeCity.ActiveReports.SectionReport.CmToInch(0.5F)
                        txtTNum.Width = GrapeCity.ActiveReports.SectionReport.CmToInch(numW)
                        txtTNum.Alignment = Section.TextAlignment.Right
                        txtTNum.VerticalAlignment = Section.VerticalTextAlignment.Middle
                        txtTNum.Font = New System.Drawing.Font("", 8)
                        locX = 2.3 + numW * tmp + prcW * tmp
                        txtTNum.Location = New System.Drawing.PointF(GrapeCity.ActiveReports.SectionReport.CmToInch(locX), GrapeCity.ActiveReports.SectionReport.CmToInch(locY))
                        txtTNum.Border.Style = BorderLineStyle.Solid

                        '金額
                        Dim txtTPrc As New GrapeCity.ActiveReports.SectionReportModel.TextBox
                        ReportFooter1.Controls.Add(txtTPrc)
                        Select Case yy
                            Case "0"
                                txtTPrc.Text = String.Format("{0:#,##0}", FsPrice(zz + TypeStart) + CsPrice(zz + TypeStart) + OthPrice(zz + TypeStart))
                            Case "1"
                                txtTPrc.Text = String.Format("{0:#,##0}", FsPrice(zz))
                            Case "2"
                                txtTPrc.Text = String.Format("{0:#,##0}", CsPrice(zz))
                            Case "3"
                                txtTPrc.Text = String.Format("{0:#,##0}", OthPrice(zz))
                        End Select
                        txtTPrc.OutputFormat = "#,##0"
                        txtTPrc.Height = GrapeCity.ActiveReports.SectionReport.CmToInch(0.5F)
                        txtTPrc.Width = GrapeCity.ActiveReports.SectionReport.CmToInch(prcW)
                        txtTPrc.Alignment = Section.TextAlignment.Right
                        txtTPrc.VerticalAlignment = Section.VerticalTextAlignment.Middle
                        txtTPrc.Font = New System.Drawing.Font("", 8)
                        locX = 2.3 + numW * (tmp + 1) + prcW * tmp
                        txtTPrc.Location = New System.Drawing.PointF(GrapeCity.ActiveReports.SectionReport.CmToInch(locX), GrapeCity.ActiveReports.SectionReport.CmToInch(locY))
                        txtTPrc.Border.Style = BorderLineStyle.Solid
                        'If yy <> 3 Then
                        '    locY += 0.5
                        'Else
                        '    locY = 0.2
                        'End If
                    Next
                    tmp += 1
                Next
                pg += 1
                pgCnt -= 1
            End If
        End If

    End Sub

    Private Sub CMPREP001_ReportEnd(sender As Object, e As EventArgs) Handles Me.ReportEnd

        If pgCnt <> 0 Then
            '2枚目のレポートを生成
            Dim report As New CMPREP001
            report.DataSource = Me.DataSource
            report.Title = Me.Title     'タイトルを再設定
            report.Run()

            '上で作成したレポートを現在のレポートと合成
            '（作成したページを下につける）
            For zz = 0 To report.Document.Pages.Count - 1
                Me.Document.Pages.Insert(Me.Document.Pages.Count, report.Document.Pages(zz))
            Next

        End If

        '※後処理
        pg = 1
        pgCnt = 1
        dt.Clear()
        clmW = 0
        bln = False
        locX = 0
        locY = 0.2

    End Sub

End Class
