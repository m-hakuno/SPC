<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class BBPREP001
    Inherits GrapeCity.ActiveReports.SectionReport

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
        End If
        MyBase.Dispose(disposing)
    End Sub

    'メモ: 以下のプロシージャは ActiveReports デザイナーで必要です。
    'ActiveReports デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    Private WithEvents PageHeader As GrapeCity.ActiveReports.SectionReportModel.PageHeader
    Private WithEvents Detail As GrapeCity.ActiveReports.SectionReportModel.Detail
    Private WithEvents PageFooter As GrapeCity.ActiveReports.SectionReportModel.PageFooter
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(BBPREP001))
        Me.PageHeader = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
        Me.txtCompNM = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtPostNM = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.ReportInfo1 = New GrapeCity.ActiveReports.SectionReportModel.ReportInfo()
        Me.txtMyCompNM = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtMyPostNM = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label2 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label3 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label4 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label5 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label6 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label7 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label8 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label9 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label10 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label11 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
        Me.txtGrpNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtNLCLS = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtProcDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtCntlNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtTBOXID = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtHallNM = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtBBDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtBBNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.PageFooter = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
        Me.ReportInfo2 = New GrapeCity.ActiveReports.SectionReportModel.ReportInfo()
        Me.GroupHeader1 = New GrapeCity.ActiveReports.SectionReportModel.GroupHeader()
        Me.GroupFooter1 = New GrapeCity.ActiveReports.SectionReportModel.GroupFooter()
        CType(Me.txtCompNM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPostNM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ReportInfo1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtMyCompNM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtMyPostNM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtGrpNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtNLCLS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtProcDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtCntlNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTBOXID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtHallNM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtBBDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtBBNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ReportInfo2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'PageHeader
        '
        Me.PageHeader.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtCompNM, Me.txtPostNM, Me.ReportInfo1, Me.txtMyCompNM, Me.txtMyPostNM, Me.Label2, Me.Label3, Me.Label4, Me.Label5, Me.Label6, Me.Label7, Me.Label8, Me.Label9, Me.Label10, Me.Label11})
        Me.PageHeader.Height = 1.5625!
        Me.PageHeader.Name = "PageHeader"
        '
        'txtCompNM
        '
        Me.txtCompNM.DataField = "送付先名"
        Me.txtCompNM.Height = 0.2!
        Me.txtCompNM.Left = 0.1779528!
        Me.txtCompNM.Name = "txtCompNM"
        Me.txtCompNM.Text = "マミヤ・オーピー株式会社"
        Me.txtCompNM.Top = 0.06259841!
        Me.txtCompNM.Width = 2.307087!
        '
        'txtPostNM
        '
        Me.txtPostNM.DataField = "送付先営業所"
        Me.txtPostNM.Height = 0.2!
        Me.txtPostNM.Left = 0.1779528!
        Me.txtPostNM.Name = "txtPostNM"
        Me.txtPostNM.Text = "電子事業総括本部 殿"
        Me.txtPostNM.Top = 0.2625985!
        Me.txtPostNM.Width = 5.770866!
        '
        'ReportInfo1
        '
        Me.ReportInfo1.DataField = "帳票出力日"
        Me.ReportInfo1.FormatString = "{RunDateTime:yyyy年M月d日}"
        Me.ReportInfo1.Height = 0.2!
        Me.ReportInfo1.Left = 5.875985!
        Me.ReportInfo1.MultiLine = False
        Me.ReportInfo1.Name = "ReportInfo1"
        Me.ReportInfo1.Style = "text-align: right"
        Me.ReportInfo1.Top = 0.0!
        Me.ReportInfo1.Width = 1.229134!
        '
        'txtMyCompNM
        '
        Me.txtMyCompNM.DataField = "自社名"
        Me.txtMyCompNM.Height = 0.2!
        Me.txtMyCompNM.Left = 4.146851!
        Me.txtMyCompNM.Name = "txtMyCompNM"
        Me.txtMyCompNM.Style = "text-align: right"
        Me.txtMyCompNM.Text = "エフ・エス株式会社"
        Me.txtMyCompNM.Top = 0.4165355!
        Me.txtMyCompNM.Width = 2.958269!
        '
        'txtMyPostNM
        '
        Me.txtMyPostNM.DataField = "自部署名"
        Me.txtMyPostNM.Height = 0.2!
        Me.txtMyPostNM.Left = 4.146851!
        Me.txtMyPostNM.Name = "txtMyPostNM"
        Me.txtMyPostNM.Style = "text-align: right"
        Me.txtMyPostNM.Text = "サポートセンタ"
        Me.txtMyPostNM.Top = 0.6165355!
        Me.txtMyPostNM.Width = 2.940944!
        '
        'Label2
        '
        Me.Label2.Height = 0.2283465!
        Me.Label2.HyperLink = Nothing
        Me.Label2.Left = 0.0!
        Me.Label2.Name = "Label2"
        Me.Label2.Style = "font-family: ＭＳ ゴシック; font-size: 15.75pt; font-weight: bold; text-align: center"
        Me.Label2.Text = "券売機ＢＢ１読出し　処理件数明細"
        Me.Label2.Top = 0.7653544!
        Me.Label2.Width = 7.283465!
        '
        'Label3
        '
        Me.Label3.Height = 0.21875!
        Me.Label3.HyperLink = Nothing
        Me.Label3.Left = 0.0!
        Me.Label3.Name = "Label3"
        Me.Label3.Style = "font-family: ＭＳ ゴシック; font-size: 12pt; font-weight: bold; text-align: center"
        Me.Label3.Text = "ブラックボックス調査報告書（券売機）"
        Me.Label3.Top = 1.023622!
        Me.Label3.Width = 7.283465!
        '
        'Label4
        '
        Me.Label4.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label4.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label4.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label4.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label4.Height = 0.1968504!
        Me.Label4.HyperLink = Nothing
        Me.Label4.Left = 0.0!
        Me.Label4.Name = "Label4"
        Me.Label4.Style = "background-color: LightGrey; text-align: center"
        Me.Label4.Text = "No."
        Me.Label4.Top = 1.377953!
        Me.Label4.Width = 0.2755905!
        '
        'Label5
        '
        Me.Label5.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label5.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label5.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label5.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label5.Height = 0.1968504!
        Me.Label5.HyperLink = Nothing
        Me.Label5.Left = 0.2755905!
        Me.Label5.Name = "Label5"
        Me.Label5.Style = "background-color: LightGrey; text-align: center"
        Me.Label5.Text = "N/L"
        Me.Label5.Top = 1.377953!
        Me.Label5.Width = 0.3149606!
        '
        'Label6
        '
        Me.Label6.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label6.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label6.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label6.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label6.Height = 0.1968504!
        Me.Label6.HyperLink = Nothing
        Me.Label6.Left = 1.771654!
        Me.Label6.Name = "Label6"
        Me.Label6.Style = "background-color: LightGrey; text-align: center"
        Me.Label6.Text = "ＢＢ調報No."
        Me.Label6.Top = 1.377953!
        Me.Label6.Width = 0.7874014!
        '
        'Label7
        '
        Me.Label7.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label7.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label7.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label7.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label7.Height = 0.1968504!
        Me.Label7.HyperLink = Nothing
        Me.Label7.Left = 2.559055!
        Me.Label7.Name = "Label7"
        Me.Label7.Style = "background-color: LightGrey; text-align: center"
        Me.Label7.Text = "TBOX-ID"
        Me.Label7.Top = 1.377953!
        Me.Label7.Width = 0.7874014!
        '
        'Label8
        '
        Me.Label8.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label8.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label8.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label8.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label8.Height = 0.1968504!
        Me.Label8.HyperLink = Nothing
        Me.Label8.Left = 3.346457!
        Me.Label8.Name = "Label8"
        Me.Label8.Style = "background-color: LightGrey; text-align: center"
        Me.Label8.Text = "店舗名"
        Me.Label8.Top = 1.377953!
        Me.Label8.Width = 1.968504!
        '
        'Label9
        '
        Me.Label9.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label9.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label9.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label9.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label9.Height = 0.1968504!
        Me.Label9.HyperLink = Nothing
        Me.Label9.Left = 5.31496!
        Me.Label9.Name = "Label9"
        Me.Label9.Style = "background-color: LightGrey; text-align: center"
        Me.Label9.Text = "ＢＢ日付"
        Me.Label9.Top = 1.377953!
        Me.Label9.Width = 1.181102!
        '
        'Label10
        '
        Me.Label10.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label10.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label10.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label10.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label10.Height = 0.1968504!
        Me.Label10.HyperLink = Nothing
        Me.Label10.Left = 0.5905512!
        Me.Label10.Name = "Label10"
        Me.Label10.Style = "background-color: LightGrey; text-align: center"
        Me.Label10.Text = "処理日"
        Me.Label10.Top = 1.377953!
        Me.Label10.Width = 1.181102!
        '
        'Label11
        '
        Me.Label11.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label11.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label11.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label11.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label11.Height = 0.1968504!
        Me.Label11.HyperLink = Nothing
        Me.Label11.Left = 6.496063!
        Me.Label11.Name = "Label11"
        Me.Label11.Style = "background-color: LightGrey; text-align: center"
        Me.Label11.Text = "ＢＢＮｏ．"
        Me.Label11.Top = 1.377953!
        Me.Label11.Width = 0.7874014!
        '
        'Detail
        '
        Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtGrpNo, Me.txtNLCLS, Me.txtProcDate, Me.txtCntlNo, Me.txtTBOXID, Me.txtHallNM, Me.txtBBDate, Me.txtBBNo})
        Me.Detail.Height = 0.1968504!
        Me.Detail.Name = "Detail"
        '
        'txtGrpNo
        '
        Me.txtGrpNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtGrpNo.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtGrpNo.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtGrpNo.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtGrpNo.CountNullValues = True
        Me.txtGrpNo.DataField = "Ｎｏ"
        Me.txtGrpNo.Height = 0.1968504!
        Me.txtGrpNo.Left = 0.0!
        Me.txtGrpNo.Name = "txtGrpNo"
        Me.txtGrpNo.Style = "text-align: center; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: n" & _
    "owrap"
        Me.txtGrpNo.SummaryFunc = GrapeCity.ActiveReports.SectionReportModel.SummaryFunc.Count
        Me.txtGrpNo.SummaryGroup = "GroupHeader1"
        Me.txtGrpNo.SummaryRunning = GrapeCity.ActiveReports.SectionReportModel.SummaryRunning.Group
        Me.txtGrpNo.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal
        Me.txtGrpNo.Text = "1"
        Me.txtGrpNo.Top = 0.0!
        Me.txtGrpNo.Width = 0.2755905!
        '
        'txtNLCLS
        '
        Me.txtNLCLS.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtNLCLS.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtNLCLS.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtNLCLS.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtNLCLS.DataField = "Ｎ_Ｌ"
        Me.txtNLCLS.Height = 0.1968504!
        Me.txtNLCLS.Left = 0.2755905!
        Me.txtNLCLS.Name = "txtNLCLS"
        Me.txtNLCLS.Style = "text-align: center; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: n" & _
    "owrap"
        Me.txtNLCLS.Text = "L"
        Me.txtNLCLS.Top = 0.0!
        Me.txtNLCLS.Width = 0.3149606!
        '
        'txtProcDate
        '
        Me.txtProcDate.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtProcDate.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtProcDate.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtProcDate.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtProcDate.DataField = "処理日"
        Me.txtProcDate.Height = 0.1968504!
        Me.txtProcDate.Left = 0.5905511!
        Me.txtProcDate.Name = "txtProcDate"
        Me.txtProcDate.OutputFormat = resources.GetString("txtProcDate.OutputFormat")
        Me.txtProcDate.Style = "text-align: center; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: n" & _
    "owrap"
        Me.txtProcDate.Text = "123456789"
        Me.txtProcDate.Top = 0.0!
        Me.txtProcDate.Width = 1.181102!
        '
        'txtCntlNo
        '
        Me.txtCntlNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtCntlNo.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtCntlNo.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtCntlNo.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtCntlNo.DataField = "ＢＢ調報Ｎｏ"
        Me.txtCntlNo.Height = 0.1968504!
        Me.txtCntlNo.Left = 1.771654!
        Me.txtCntlNo.Name = "txtCntlNo"
        Me.txtCntlNo.Style = "text-align: center; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: n" & _
    "owrap"
        Me.txtCntlNo.Text = "00020371"
        Me.txtCntlNo.Top = 0.0!
        Me.txtCntlNo.Width = 0.7874014!
        '
        'txtTBOXID
        '
        Me.txtTBOXID.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTBOXID.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTBOXID.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTBOXID.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTBOXID.DataField = "ＴＢＯＸＩＤ"
        Me.txtTBOXID.Height = 0.1968504!
        Me.txtTBOXID.Left = 2.559055!
        Me.txtTBOXID.Name = "txtTBOXID"
        Me.txtTBOXID.Padding = New GrapeCity.ActiveReports.PaddingEx(3, 0, 0, 0)
        Me.txtTBOXID.Style = "text-align: center; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: n" & _
    "owrap"
        Me.txtTBOXID.Text = "テストホール"
        Me.txtTBOXID.Top = 0.0!
        Me.txtTBOXID.Width = 0.7874016!
        '
        'txtHallNM
        '
        Me.txtHallNM.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallNM.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallNM.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallNM.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallNM.DataField = "店舗名"
        Me.txtHallNM.Height = 0.1968504!
        Me.txtHallNM.Left = 3.346457!
        Me.txtHallNM.Name = "txtHallNM"
        Me.txtHallNM.ShrinkToFit = True
        Me.txtHallNM.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left; vertical-align: middle" & _
    "; white-space: inherit; ddo-shrink-to-fit: true; ddo-wrap-mode: inherit"
        Me.txtHallNM.Text = "1234567890ABCDEFGHIJ"
        Me.txtHallNM.Top = 0.0!
        Me.txtHallNM.Width = 1.968504!
        '
        'txtBBDate
        '
        Me.txtBBDate.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtBBDate.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtBBDate.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtBBDate.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtBBDate.DataField = "ＢＢ日付"
        Me.txtBBDate.Height = 0.1968504!
        Me.txtBBDate.Left = 5.31496!
        Me.txtBBDate.Name = "txtBBDate"
        Me.txtBBDate.OutputFormat = resources.GetString("txtBBDate.OutputFormat")
        Me.txtBBDate.Style = "text-align: center; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: n" & _
    "owrap"
        Me.txtBBDate.Text = "2013年04月02日"
        Me.txtBBDate.Top = 0.0!
        Me.txtBBDate.Width = 1.181102!
        '
        'txtBBNo
        '
        Me.txtBBNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtBBNo.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtBBNo.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtBBNo.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtBBNo.DataField = "ＢＢＮｏ"
        Me.txtBBNo.Height = 0.1968504!
        Me.txtBBNo.Left = 6.496063!
        Me.txtBBNo.Name = "txtBBNo"
        Me.txtBBNo.Style = "text-align: center; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: n" & _
    "owrap"
        Me.txtBBNo.Text = "OK"
        Me.txtBBNo.Top = 0.0!
        Me.txtBBNo.Width = 0.7874014!
        '
        'PageFooter
        '
        Me.PageFooter.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.ReportInfo2})
        Me.PageFooter.Height = 0.1968504!
        Me.PageFooter.Name = "PageFooter"
        '
        'ReportInfo2
        '
        Me.ReportInfo2.FormatString = "{PageNumber} / {PageCount} ページ"
        Me.ReportInfo2.Height = 0.1968504!
        Me.ReportInfo2.Left = 2.755906!
        Me.ReportInfo2.Name = "ReportInfo2"
        Me.ReportInfo2.Style = "text-align: center; white-space: nowrap; ddo-wrap-mode: nowrap"
        Me.ReportInfo2.Top = 0.0!
        Me.ReportInfo2.Width = 1.771654!
        '
        'GroupHeader1
        '
        Me.GroupHeader1.Height = 0.0!
        Me.GroupHeader1.Name = "GroupHeader1"
        '
        'GroupFooter1
        '
        Me.GroupFooter1.Height = 0.0!
        Me.GroupFooter1.Name = "GroupFooter1"
        '
        'BBPREP001
        '
        Me.MasterReport = False
        Me.PageSettings.DefaultPaperSize = False
        Me.PageSettings.Margins.Bottom = 0.3937007!
        Me.PageSettings.Margins.Left = 0.5905512!
        Me.PageSettings.Margins.Right = 0.3937008!
        Me.PageSettings.Margins.Top = 0.5905512!
        Me.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Portrait
        Me.PageSettings.PaperHeight = 11.69291!
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4
        Me.PageSettings.PaperWidth = 8.267716!
        Me.PrintWidth = 7.283465!
        Me.Sections.Add(Me.PageHeader)
        Me.Sections.Add(Me.GroupHeader1)
        Me.Sections.Add(Me.Detail)
        Me.Sections.Add(Me.GroupFooter1)
        Me.Sections.Add(Me.PageFooter)
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; " & _
            "color: Black; font-family: ""MS UI Gothic""; ddo-char-set: 128", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: ""MS UI Gothic""; ddo-char-set: 12" & _
            "8", "Heading1", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: ""MS UI Goth" & _
            "ic""; ddo-char-set: 128", "Heading2", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"))
        CType(Me.txtCompNM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPostNM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ReportInfo1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtMyCompNM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtMyPostNM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtGrpNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtNLCLS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtProcDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtCntlNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTBOXID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtHallNM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtBBDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtBBNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ReportInfo2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Private WithEvents txtCompNM As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtPostNM As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents ReportInfo1 As GrapeCity.ActiveReports.SectionReportModel.ReportInfo
    Private WithEvents txtMyCompNM As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtMyPostNM As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label2 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label3 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label4 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label5 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label6 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label7 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label8 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label9 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label10 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label11 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents ReportInfo2 As GrapeCity.ActiveReports.SectionReportModel.ReportInfo
    Private WithEvents txtGrpNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtNLCLS As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtProcDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtCntlNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTBOXID As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtHallNM As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtBBDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtBBNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents GroupHeader1 As GrapeCity.ActiveReports.SectionReportModel.GroupHeader
    Private WithEvents GroupFooter1 As GrapeCity.ActiveReports.SectionReportModel.GroupFooter
End Class
