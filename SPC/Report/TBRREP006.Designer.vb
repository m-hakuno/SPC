<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class TBRREP006
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
    Private WithEvents PageHeader1 As GrapeCity.ActiveReports.SectionReportModel.PageHeader
    Private WithEvents Detail1 As GrapeCity.ActiveReports.SectionReportModel.Detail
    Private WithEvents PageFooter1 As GrapeCity.ActiveReports.SectionReportModel.PageFooter
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(TBRREP006))
        Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
        Me.lblTitle = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblDt = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblPage = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblRefDt = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblTboxId = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblLogHsDt = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblTbox = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblTboxMode = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblTboxSts = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblKbInf = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblKeyLct = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblApDiv = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblLogDiv = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtPage = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtRefDt = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtDt = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtTboxId = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtHallNm = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblHall1 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblTboxVer = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtTboxVer = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblHall2 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Line1 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line2 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.lblScnNo = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblUnyDt = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail()
        Me.txtLogHsDt = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtLogHsTm = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtUnyDt = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtTboxMode = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtScnNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtTboxSts = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtKbInf = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtKeyLct = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtApDiv = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtLogDiv = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
        CType(Me.lblTitle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblDt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblPage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblRefDt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTboxId, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblLogHsDt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTbox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTboxMode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTboxSts, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblKbInf, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblKeyLct, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblApDiv, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblLogDiv, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtRefDt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTboxId, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtHallNm, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblHall1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTboxVer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTboxVer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblHall2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblScnNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblUnyDt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtLogHsDt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtLogHsTm, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtUnyDt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTboxMode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtScnNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTboxSts, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKbInf, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKeyLct, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtApDiv, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtLogDiv, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'PageHeader1
        '
        Me.PageHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.lblTitle, Me.lblDt, Me.lblPage, Me.lblRefDt, Me.lblTboxId, Me.lblLogHsDt, Me.lblTbox, Me.lblTboxMode, Me.lblTboxSts, Me.lblKbInf, Me.lblKeyLct, Me.lblApDiv, Me.lblLogDiv, Me.txtPage, Me.txtRefDt, Me.txtDt, Me.txtTboxId, Me.txtHallNm, Me.lblHall1, Me.lblTboxVer, Me.txtTboxVer, Me.lblHall2, Me.Line1, Me.Line2, Me.lblScnNo, Me.lblUnyDt})
        Me.PageHeader1.Height = 1.72646!
        Me.PageHeader1.Name = "PageHeader1"
        '
        'lblTitle
        '
        Me.lblTitle.Height = 0.1582677!
        Me.lblTitle.HyperLink = Nothing
        Me.lblTitle.Left = 4.030315!
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: center"
        Me.lblTitle.Text = "＊＊　　ＴＢＯＸ操作ログ　　＊＊"
        Me.lblTitle.Top = 0.7874016!
        Me.lblTitle.Width = 2.416535!
        '
        'lblDt
        '
        Me.lblDt.Height = 0.1582677!
        Me.lblDt.HyperLink = Nothing
        Me.lblDt.Left = 8.67992!
        Me.lblDt.MultiLine = False
        Me.lblDt.Name = "lblDt"
        Me.lblDt.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left"
        Me.lblDt.Text = "日時"
        Me.lblDt.Top = 0.7874016!
        Me.lblDt.Width = 0.7708664!
        '
        'lblPage
        '
        Me.lblPage.Height = 0.1586614!
        Me.lblPage.HyperLink = Nothing
        Me.lblPage.Left = 8.679922!
        Me.lblPage.MultiLine = False
        Me.lblPage.Name = "lblPage"
        Me.lblPage.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left"
        Me.lblPage.Text = "ページ数"
        Me.lblPage.Top = 0.9456694!
        Me.lblPage.Width = 0.8224411!
        '
        'lblRefDt
        '
        Me.lblRefDt.Height = 0.1582678!
        Me.lblRefDt.HyperLink = Nothing
        Me.lblRefDt.Left = 8.679922!
        Me.lblRefDt.MultiLine = False
        Me.lblRefDt.Name = "lblRefDt"
        Me.lblRefDt.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left"
        Me.lblRefDt.Text = "照会日時："
        Me.lblRefDt.Top = 1.114567!
        Me.lblRefDt.Width = 0.7606297!
        '
        'lblTboxId
        '
        Me.lblTboxId.Height = 0.1480316!
        Me.lblTboxId.HyperLink = Nothing
        Me.lblTboxId.Left = 0.219685!
        Me.lblTboxId.MultiLine = False
        Me.lblTboxId.Name = "lblTboxId"
        Me.lblTboxId.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left"
        Me.lblTboxId.Text = "ＴＢＯＸＩＤ："
        Me.lblTboxId.Top = 1.114567!
        Me.lblTboxId.Width = 1.072835!
        '
        'lblLogHsDt
        '
        Me.lblLogHsDt.Height = 0.2!
        Me.lblLogHsDt.HyperLink = Nothing
        Me.lblLogHsDt.Left = 0.250394!
        Me.lblLogHsDt.MultiLine = False
        Me.lblLogHsDt.Name = "lblLogHsDt"
        Me.lblLogHsDt.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left"
        Me.lblLogHsDt.Text = "発生日時"
        Me.lblLogHsDt.Top = 1.359449!
        Me.lblLogHsDt.Width = 0.6354332!
        '
        'lblTbox
        '
        Me.lblTbox.Height = 0.2!
        Me.lblTbox.HyperLink = Nothing
        Me.lblTbox.Left = 1.051575!
        Me.lblTbox.MultiLine = False
        Me.lblTbox.Name = "lblTbox"
        Me.lblTbox.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: center"
        Me.lblTbox.Text = "ＴＢＯＸ"
        Me.lblTbox.Top = 1.351575!
        Me.lblTbox.Width = 0.6145663!
        '
        'lblTboxMode
        '
        Me.lblTboxMode.Height = 0.2!
        Me.lblTboxMode.HyperLink = Nothing
        Me.lblTboxMode.Left = 1.935826!
        Me.lblTboxMode.MultiLine = False
        Me.lblTboxMode.Name = "lblTboxMode"
        Me.lblTboxMode.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left"
        Me.lblTboxMode.Text = "ＴＢＯＸモード"
        Me.lblTboxMode.Top = 1.351575!
        Me.lblTboxMode.Width = 1.072835!
        '
        'lblTboxSts
        '
        Me.lblTboxSts.Height = 0.2!
        Me.lblTboxSts.HyperLink = Nothing
        Me.lblTboxSts.Left = 3.459056!
        Me.lblTboxSts.MultiLine = False
        Me.lblTboxSts.Name = "lblTboxSts"
        Me.lblTboxSts.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left"
        Me.lblTboxSts.Text = "ＴＢＯＸ状態"
        Me.lblTboxSts.Top = 1.351575!
        Me.lblTboxSts.Width = 0.9271653!
        '
        'lblKbInf
        '
        Me.lblKbInf.Height = 0.2!
        Me.lblKbInf.HyperLink = Nothing
        Me.lblKbInf.Left = 4.982281!
        Me.lblKbInf.MultiLine = False
        Me.lblKbInf.Name = "lblKbInf"
        Me.lblKbInf.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left"
        Me.lblKbInf.Text = "キーボード情報"
        Me.lblKbInf.Top = 1.351575!
        Me.lblKbInf.Width = 1.072835!
        '
        'lblKeyLct
        '
        Me.lblKeyLct.Height = 0.2!
        Me.lblKeyLct.HyperLink = Nothing
        Me.lblKeyLct.Left = 6.223228!
        Me.lblKeyLct.MultiLine = False
        Me.lblKeyLct.Name = "lblKeyLct"
        Me.lblKeyLct.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: justify"
        Me.lblKeyLct.Text = "鍵位置"
        Me.lblKeyLct.Top = 1.351575!
        Me.lblKeyLct.Width = 0.4893703!
        '
        'lblApDiv
        '
        Me.lblApDiv.Height = 0.2!
        Me.lblApDiv.HyperLink = Nothing
        Me.lblApDiv.Left = 6.915746!
        Me.lblApDiv.MultiLine = False
        Me.lblApDiv.Name = "lblApDiv"
        Me.lblApDiv.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left"
        Me.lblApDiv.Text = "ＡＰ種別"
        Me.lblApDiv.Top = 1.351575!
        Me.lblApDiv.Width = 0.6665354!
        '
        'lblLogDiv
        '
        Me.lblLogDiv.Height = 0.2!
        Me.lblLogDiv.HyperLink = Nothing
        Me.lblLogDiv.Left = 9.108269!
        Me.lblLogDiv.MultiLine = False
        Me.lblLogDiv.Name = "lblLogDiv"
        Me.lblLogDiv.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left"
        Me.lblLogDiv.Text = "操作ログ種別"
        Me.lblLogDiv.Top = 1.359449!
        Me.lblLogDiv.Width = 1.010237!
        '
        'txtPage
        '
        Me.txtPage.Height = 0.1586614!
        Me.txtPage.Left = 10.04528!
        Me.txtPage.MultiLine = False
        Me.txtPage.Name = "txtPage"
        Me.txtPage.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.txtPage.SummaryRunning = GrapeCity.ActiveReports.SectionReportModel.SummaryRunning.All
        Me.txtPage.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.PageCount
        Me.txtPage.Text = "XXX"
        Me.txtPage.Top = 0.9456694!
        Me.txtPage.Width = 0.4582682!
        '
        'txtRefDt
        '
        Me.txtRefDt.DataField = "照会日時"
        Me.txtRefDt.Height = 0.1582678!
        Me.txtRefDt.Left = 9.388584!
        Me.txtRefDt.MultiLine = False
        Me.txtRefDt.Name = "txtRefDt"
        Me.txtRefDt.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtRefDt.Text = "YYYY/MM/DD HH:MM"
        Me.txtRefDt.Top = 1.114567!
        Me.txtRefDt.Width = 1.198425!
        '
        'txtDt
        '
        Me.txtDt.DataField = "出力日時"
        Me.txtDt.Height = 0.1582677!
        Me.txtDt.Left = 9.398817!
        Me.txtDt.MultiLine = False
        Me.txtDt.Name = "txtDt"
        Me.txtDt.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtDt.Text = "YYYY/MM/DD HH:MM"
        Me.txtDt.Top = 0.7874016!
        Me.txtDt.Width = 1.198425!
        '
        'txtTboxId
        '
        Me.txtTboxId.DataField = "ＴＢＯＸＩＤ"
        Me.txtTboxId.Height = 0.1480315!
        Me.txtTboxId.Left = 1.176771!
        Me.txtTboxId.MultiLine = False
        Me.txtTboxId.Name = "txtTboxId"
        Me.txtTboxId.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtTboxId.Text = "XXXXXXXX"
        Me.txtTboxId.Top = 1.114567!
        Me.txtTboxId.Width = 0.6149607!
        '
        'txtHallNm
        '
        Me.txtHallNm.DataField = "ホール名"
        Me.txtHallNm.Height = 0.1480315!
        Me.txtHallNm.Left = 1.905118!
        Me.txtHallNm.MultiLine = False
        Me.txtHallNm.Name = "txtHallNm"
        Me.txtHallNm.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtHallNm.Text = "ＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸ１２３４５６７８９０"
        Me.txtHallNm.Top = 1.114567!
        Me.txtHallNm.Width = 4.80748!
        '
        'lblHall1
        '
        Me.lblHall1.Height = 0.1480315!
        Me.lblHall1.HyperLink = Nothing
        Me.lblHall1.Left = 1.729134!
        Me.lblHall1.MultiLine = False
        Me.lblHall1.Name = "lblHall1"
        Me.lblHall1.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left"
        Me.lblHall1.Text = "【"
        Me.lblHall1.Top = 1.114567!
        Me.lblHall1.Width = 0.23937!
        '
        'lblTboxVer
        '
        Me.lblTboxVer.Height = 0.1480315!
        Me.lblTboxVer.HyperLink = Nothing
        Me.lblTboxVer.Left = 6.885038!
        Me.lblTboxVer.MultiLine = False
        Me.lblTboxVer.Name = "lblTboxVer"
        Me.lblTboxVer.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left"
        Me.lblTboxVer.Text = "ＴＢＯＸ ＶＥＲ："
        Me.lblTboxVer.Top = 1.114567!
        Me.lblTboxVer.Width = 1.229134!
        '
        'txtTboxVer
        '
        Me.txtTboxVer.DataField = "ＴＢＯＸＶＥＲ"
        Me.txtTboxVer.Height = 0.1480315!
        Me.txtTboxVer.Left = 8.114174!
        Me.txtTboxVer.MultiLine = False
        Me.txtTboxVer.Name = "txtTboxVer"
        Me.txtTboxVer.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtTboxVer.Text = "XXXX"
        Me.txtTboxVer.Top = 1.114567!
        Me.txtTboxVer.Width = 0.3937008!
        '
        'lblHall2
        '
        Me.lblHall2.Height = 0.1480315!
        Me.lblHall2.HyperLink = Nothing
        Me.lblHall2.Left = 6.712597!
        Me.lblHall2.MultiLine = False
        Me.lblHall2.Name = "lblHall2"
        Me.lblHall2.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left"
        Me.lblHall2.Text = "】"
        Me.lblHall2.Top = 1.114567!
        Me.lblHall2.Width = 0.23937!
        '
        'Line1
        '
        Me.Line1.Height = 0.0!
        Me.Line1.Left = 0.07440957!
        Me.Line1.LineWeight = 2.0!
        Me.Line1.Name = "Line1"
        Me.Line1.Top = 1.337796!
        Me.Line1.Width = 10.85866!
        Me.Line1.X1 = 0.07440957!
        Me.Line1.X2 = 10.93307!
        Me.Line1.Y1 = 1.337796!
        Me.Line1.Y2 = 1.337796!
        '
        'Line2
        '
        Me.Line2.Height = 0.0!
        Me.Line2.Left = 0.07440957!
        Me.Line2.LineWeight = 2.0!
        Me.Line2.Name = "Line2"
        Me.Line2.Top = 1.693307!
        Me.Line2.Width = 10.85866!
        Me.Line2.X1 = 0.07440957!
        Me.Line2.X2 = 10.93307!
        Me.Line2.Y1 = 1.693307!
        Me.Line2.Y2 = 1.693307!
        '
        'lblScnNo
        '
        Me.lblScnNo.Height = 0.1850399!
        Me.lblScnNo.HyperLink = Nothing
        Me.lblScnNo.Left = 1.935826!
        Me.lblScnNo.MultiLine = False
        Me.lblScnNo.Name = "lblScnNo"
        Me.lblScnNo.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left"
        Me.lblScnNo.Text = "画面番号"
        Me.lblScnNo.Top = 1.512204!
        Me.lblScnNo.Width = 1.072835!
        '
        'lblUnyDt
        '
        Me.lblUnyDt.Height = 0.1850394!
        Me.lblUnyDt.HyperLink = Nothing
        Me.lblUnyDt.Left = 1.051575!
        Me.lblUnyDt.MultiLine = False
        Me.lblUnyDt.Name = "lblUnyDt"
        Me.lblUnyDt.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: center"
        Me.lblUnyDt.Text = "運用日"
        Me.lblUnyDt.Top = 1.512205!
        Me.lblUnyDt.Width = 0.6145669!
        '
        'Detail1
        '
        Me.Detail1.CanGrow = False
        Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtLogHsDt, Me.txtLogHsTm, Me.txtUnyDt, Me.txtTboxMode, Me.txtScnNo, Me.txtTboxSts, Me.txtKbInf, Me.txtKeyLct, Me.txtApDiv, Me.txtLogDiv})
        Me.Detail1.Height = 0.3149606!
        Me.Detail1.KeepTogether = True
        Me.Detail1.Name = "Detail1"
        Me.Detail1.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before
        '
        'txtLogHsDt
        '
        Me.txtLogHsDt.DataField = "発生日付"
        Me.txtLogHsDt.Height = 0.1377953!
        Me.txtLogHsDt.Left = 0.2503937!
        Me.txtLogHsDt.MultiLine = False
        Me.txtLogHsDt.Name = "txtLogHsDt"
        Me.txtLogHsDt.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtLogHsDt.Text = "YYYY/MM/DD"
        Me.txtLogHsDt.Top = 0.0!
        Me.txtLogHsDt.Width = 0.7401575!
        '
        'txtLogHsTm
        '
        Me.txtLogHsTm.DataField = "発生時刻"
        Me.txtLogHsTm.Height = 0.1377953!
        Me.txtLogHsTm.Left = 0.2503937!
        Me.txtLogHsTm.MultiLine = False
        Me.txtLogHsTm.Name = "txtLogHsTm"
        Me.txtLogHsTm.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtLogHsTm.Text = "HH:MM:SS"
        Me.txtLogHsTm.Top = 0.1299213!
        Me.txtLogHsTm.Width = 0.7401577!
        '
        'txtUnyDt
        '
        Me.txtUnyDt.DataField = "ＴＢＯＸ運用日"
        Me.txtUnyDt.Height = 0.1377953!
        Me.txtUnyDt.Left = 1.051575!
        Me.txtUnyDt.MultiLine = False
        Me.txtUnyDt.Name = "txtUnyDt"
        Me.txtUnyDt.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtUnyDt.Text = "YYYY/MM/DD"
        Me.txtUnyDt.Top = 0.0!
        Me.txtUnyDt.Width = 0.7401577!
        '
        'txtTboxMode
        '
        Me.txtTboxMode.DataField = "ＴＢＯＸモード"
        Me.txtTboxMode.Height = 0.1377953!
        Me.txtTboxMode.Left = 1.935827!
        Me.txtTboxMode.MultiLine = False
        Me.txtTboxMode.Name = "txtTboxMode"
        Me.txtTboxMode.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtTboxMode.Text = "ＸＸＸＸＸＸＸＸＸＸ"
        Me.txtTboxMode.Top = 0.0!
        Me.txtTboxMode.Width = 1.395669!
        '
        'txtScnNo
        '
        Me.txtScnNo.DataField = "画面番号"
        Me.txtScnNo.Height = 0.1377953!
        Me.txtScnNo.Left = 1.935827!
        Me.txtScnNo.MultiLine = False
        Me.txtScnNo.Name = "txtScnNo"
        Me.txtScnNo.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtScnNo.Text = "ＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸ"
        Me.txtScnNo.Top = 0.1299213!
        Me.txtScnNo.Width = 4.146063!
        '
        'txtTboxSts
        '
        Me.txtTboxSts.DataField = "ＴＢＯＸ状態"
        Me.txtTboxSts.Height = 0.1377953!
        Me.txtTboxSts.Left = 3.459056!
        Me.txtTboxSts.MultiLine = False
        Me.txtTboxSts.Name = "txtTboxSts"
        Me.txtTboxSts.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtTboxSts.Text = "ＸＸＸＸＸＸＸＸＸＸ"
        Me.txtTboxSts.Top = 0.0!
        Me.txtTboxSts.Width = 1.427166!
        '
        'txtKbInf
        '
        Me.txtKbInf.CanGrow = False
        Me.txtKbInf.DataField = "キーボード情報"
        Me.txtKbInf.Height = 0.1377953!
        Me.txtKbInf.Left = 4.982284!
        Me.txtKbInf.Name = "txtKbInf"
        Me.txtKbInf.ShrinkToFit = True
        Me.txtKbInf.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; ddo-shrink-to-fit: true"
        Me.txtKbInf.Text = "ＸＸＸＸＸＸＸＸ"
        Me.txtKbInf.Top = 0.0!
        Me.txtKbInf.Width = 1.166929!
        '
        'txtKeyLct
        '
        Me.txtKeyLct.DataField = "鍵位置"
        Me.txtKeyLct.Height = 0.1377953!
        Me.txtKeyLct.Left = 6.223229!
        Me.txtKeyLct.MultiLine = False
        Me.txtKeyLct.Name = "txtKeyLct"
        Me.txtKeyLct.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtKeyLct.Text = "ＸＸＸＸ"
        Me.txtKeyLct.Top = 0.0!
        Me.txtKeyLct.Width = 0.5940943!
        '
        'txtApDiv
        '
        Me.txtApDiv.DataField = "ＡＰ種別"
        Me.txtApDiv.Height = 0.2755905!
        Me.txtApDiv.Left = 6.915749!
        Me.txtApDiv.Name = "txtApDiv"
        Me.txtApDiv.ShrinkToFit = True
        Me.txtApDiv.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; ddo-shrink-to-fit: true"
        Me.txtApDiv.Text = "ＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸ"
        Me.txtApDiv.Top = 0.0!
        Me.txtApDiv.Width = 2.090551!
        '
        'txtLogDiv
        '
        Me.txtLogDiv.DataField = "操作ログ種別"
        Me.txtLogDiv.Height = 0.2755905!
        Me.txtLogDiv.Left = 9.108268!
        Me.txtLogDiv.Name = "txtLogDiv"
        Me.txtLogDiv.ShrinkToFit = True
        Me.txtLogDiv.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; ddo-shrink-to-fit: true"
        Me.txtLogDiv.Text = "ＸＸＸＸＸＸＸＸＸＸ"
        Me.txtLogDiv.Top = 0.0!
        Me.txtLogDiv.Width = 1.465354!
        '
        'PageFooter1
        '
        Me.PageFooter1.Height = 0.0!
        Me.PageFooter1.Name = "PageFooter1"
        '
        'TBRREP006
        '
        Me.MasterReport = False
        Me.PageSettings.DefaultPaperSize = False
        Me.PageSettings.Margins.Bottom = 0.1968504!
        Me.PageSettings.Margins.Left = 0.1968504!
        Me.PageSettings.Margins.Right = 0.1968504!
        Me.PageSettings.Margins.Top = 0.1968504!
        Me.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape
        Me.PageSettings.PaperHeight = 11.42!
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Custom
        Me.PageSettings.PaperName = "ユーザー定義のサイズ"
        Me.PageSettings.PaperWidth = 8.660001!
        Me.PrintWidth = 11.03544!
        Me.Sections.Add(Me.PageHeader1)
        Me.Sections.Add(Me.Detail1)
        Me.Sections.Add(Me.PageFooter1)
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; " & _
            "color: Black; font-family: ""ＭＳ ゴシック""; ddo-char-set: 186", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: ""MS UI Gothic""; ddo-char-set: 12" & _
            "8", "Heading1", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: ""MS UI Goth" & _
            "ic""; ddo-char-set: 128", "Heading2", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"))
        CType(Me.lblTitle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblDt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblPage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblRefDt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTboxId, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblLogHsDt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTbox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTboxMode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTboxSts, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblKbInf, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblKeyLct, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblApDiv, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblLogDiv, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtRefDt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTboxId, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtHallNm, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblHall1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTboxVer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTboxVer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblHall2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblScnNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblUnyDt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtLogHsDt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtLogHsTm, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtUnyDt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTboxMode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtScnNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTboxSts, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKbInf, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKeyLct, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtApDiv, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtLogDiv, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Private WithEvents lblTitle As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblDt As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblPage As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblRefDt As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblTboxId As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblTboxVer As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Line1 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents lblLogHsDt As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblTbox As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblUnyDt As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblTboxMode As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblScnNo As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Line2 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents lblTboxSts As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblKbInf As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblKeyLct As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblApDiv As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblLogDiv As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtPage As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtRefDt As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtDt As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTboxId As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtHallNm As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblHall1 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblHall2 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtTboxVer As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtLogHsDt As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtLogHsTm As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtUnyDt As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTboxMode As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtScnNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTboxSts As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtKbInf As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtKeyLct As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtApDiv As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtLogDiv As GrapeCity.ActiveReports.SectionReportModel.TextBox
End Class
