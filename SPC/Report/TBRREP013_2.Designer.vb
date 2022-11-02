<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class TBRREP013_2
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
    Private WithEvents Detail1 As GrapeCity.ActiveReports.SectionReportModel.Detail
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(TBRREP013_2))
        Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail()
        Me.txtTG_TIMESHOUH = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtTG_TIMEPTAX = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label6 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label8 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtKG_TIMEPREMIUM = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtKG_TIMESHOUH = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label5 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtTG_TIMEPREMIUM = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtTG_TIMEPMONEY = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.SUM_KG_TIMEPMONEY = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label9 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtKS_TIMESHOUH = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtTG_TIMENYUKIN = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtKG_TIMETAX = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtKG_TIMEPTAX = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Line1 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.txtKG_TIMENYUKIN = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtTS_TIMESHOUH = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtTG_TIMETAX = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtTG_PPREMIUMTOTAL = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtKG_KURIPPREMIUMTOTAL = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtHALL_NAME = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label2 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label3 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtTENPO = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        CType(Me.txtTG_TIMESHOUH, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTG_TIMEPTAX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKG_TIMEPREMIUM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKG_TIMESHOUH, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTG_TIMEPREMIUM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTG_TIMEPMONEY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SUM_KG_TIMEPMONEY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKS_TIMESHOUH, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTG_TIMENYUKIN, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKG_TIMETAX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKG_TIMEPTAX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKG_TIMENYUKIN, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTS_TIMESHOUH, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTG_TIMETAX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTG_PPREMIUMTOTAL, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtKG_KURIPPREMIUMTOTAL, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtHALL_NAME, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTENPO, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'Detail1
        '
        Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtTG_TIMESHOUH, Me.txtTG_TIMEPTAX, Me.Label6, Me.Label8, Me.txtKG_TIMEPREMIUM, Me.txtKG_TIMESHOUH, Me.Label5, Me.txtTG_TIMEPREMIUM, Me.txtTG_TIMEPMONEY, Me.SUM_KG_TIMEPMONEY, Me.Label9, Me.txtKS_TIMESHOUH, Me.txtTG_TIMENYUKIN, Me.txtKG_TIMETAX, Me.txtKG_TIMEPTAX, Me.Line1, Me.txtKG_TIMENYUKIN, Me.txtTS_TIMESHOUH, Me.txtTG_TIMETAX, Me.txtTG_PPREMIUMTOTAL, Me.txtKG_KURIPPREMIUMTOTAL, Me.txtHALL_NAME, Me.Label2, Me.Label3, Me.txtTENPO})
        Me.Detail1.Height = 0.8971457!
        Me.Detail1.KeepTogether = True
        Me.Detail1.Name = "Detail1"
        '
        'txtTG_TIMESHOUH
        '
        Me.txtTG_TIMESHOUH.DataField = "当日消費精算額"
        Me.txtTG_TIMESHOUH.Height = 0.1377953!
        Me.txtTG_TIMESHOUH.Left = 5.708662!
        Me.txtTG_TIMESHOUH.Name = "txtTG_TIMESHOUH"
        Me.txtTG_TIMESHOUH.OutputFormat = resources.GetString("txtTG_TIMESHOUH.OutputFormat")
        Me.txtTG_TIMESHOUH.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: right"
        Me.txtTG_TIMESHOUH.Text = "9,999,999,999"
        Me.txtTG_TIMESHOUH.Top = 0.1968504!
        Me.txtTG_TIMESHOUH.Width = 0.984252!
        '
        'txtTG_TIMEPTAX
        '
        Me.txtTG_TIMEPTAX.DataField = "当日Ｅマネー消費税額"
        Me.txtTG_TIMEPTAX.Height = 0.1377953!
        Me.txtTG_TIMEPTAX.Left = 7.874016!
        Me.txtTG_TIMEPTAX.Name = "txtTG_TIMEPTAX"
        Me.txtTG_TIMEPTAX.OutputFormat = resources.GetString("txtTG_TIMEPTAX.OutputFormat")
        Me.txtTG_TIMEPTAX.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: right"
        Me.txtTG_TIMEPTAX.Text = "9,999,999,999"
        Me.txtTG_TIMEPTAX.Top = 0.1968504!
        Me.txtTG_TIMEPTAX.Width = 0.984252!
        '
        'Label6
        '
        Me.Label6.Height = 0.1377953!
        Me.Label6.HyperLink = Nothing
        Me.Label6.Left = 2.161026!
        Me.Label6.Name = "Label6"
        Me.Label6.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: right"
        Me.Label6.Text = "（精算）"
        Me.Label6.Top = 0.6692914!
        Me.Label6.Width = 1.279528!
        '
        'Label8
        '
        Me.Label8.Height = 0.1377953!
        Me.Label8.HyperLink = Nothing
        Me.Label8.Left = 1.51536!
        Me.Label8.Name = "Label8"
        Me.Label8.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: right"
        Me.Label8.Text = "繰越合計"
        Me.Label8.Top = 0.511811!
        Me.Label8.Width = 1.446063!
        '
        'txtKG_TIMEPREMIUM
        '
        Me.txtKG_TIMEPREMIUM.DataField = "繰越プレミアム金額"
        Me.txtKG_TIMEPREMIUM.Height = 0.1377953!
        Me.txtKG_TIMEPREMIUM.Left = 9.055119!
        Me.txtKG_TIMEPREMIUM.Name = "txtKG_TIMEPREMIUM"
        Me.txtKG_TIMEPREMIUM.OutputFormat = resources.GetString("txtKG_TIMEPREMIUM.OutputFormat")
        Me.txtKG_TIMEPREMIUM.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: right"
        Me.txtKG_TIMEPREMIUM.Text = "9,999,999,999"
        Me.txtKG_TIMEPREMIUM.Top = 0.511811!
        Me.txtKG_TIMEPREMIUM.Width = 0.984252!
        '
        'txtKG_TIMESHOUH
        '
        Me.txtKG_TIMESHOUH.DataField = "繰越消費精算額"
        Me.txtKG_TIMESHOUH.Height = 0.1377953!
        Me.txtKG_TIMESHOUH.Left = 5.708662!
        Me.txtKG_TIMESHOUH.Name = "txtKG_TIMESHOUH"
        Me.txtKG_TIMESHOUH.OutputFormat = resources.GetString("txtKG_TIMESHOUH.OutputFormat")
        Me.txtKG_TIMESHOUH.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: right"
        Me.txtKG_TIMESHOUH.Text = "9,999,999,999"
        Me.txtKG_TIMESHOUH.Top = 0.511811!
        Me.txtKG_TIMESHOUH.Width = 0.984252!
        '
        'Label5
        '
        Me.Label5.Height = 0.1377953!
        Me.Label5.HyperLink = Nothing
        Me.Label5.Left = 2.161026!
        Me.Label5.Name = "Label5"
        Me.Label5.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: right"
        Me.Label5.Text = "（精算）"
        Me.Label5.Top = 0.3543307!
        Me.Label5.Width = 1.279528!
        '
        'txtTG_TIMEPREMIUM
        '
        Me.txtTG_TIMEPREMIUM.DataField = "当日プレミアム金額"
        Me.txtTG_TIMEPREMIUM.Height = 0.1377953!
        Me.txtTG_TIMEPREMIUM.Left = 9.055119!
        Me.txtTG_TIMEPREMIUM.Name = "txtTG_TIMEPREMIUM"
        Me.txtTG_TIMEPREMIUM.OutputFormat = resources.GetString("txtTG_TIMEPREMIUM.OutputFormat")
        Me.txtTG_TIMEPREMIUM.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: right"
        Me.txtTG_TIMEPREMIUM.Text = "9,999,999,999"
        Me.txtTG_TIMEPREMIUM.Top = 0.1968504!
        Me.txtTG_TIMEPREMIUM.Width = 0.984252!
        '
        'txtTG_TIMEPMONEY
        '
        Me.txtTG_TIMEPMONEY.DataField = "当日Ｅマネー入金額"
        Me.txtTG_TIMEPMONEY.Height = 0.1377953!
        Me.txtTG_TIMEPMONEY.Left = 4.527559!
        Me.txtTG_TIMEPMONEY.Name = "txtTG_TIMEPMONEY"
        Me.txtTG_TIMEPMONEY.OutputFormat = resources.GetString("txtTG_TIMEPMONEY.OutputFormat")
        Me.txtTG_TIMEPMONEY.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: right"
        Me.txtTG_TIMEPMONEY.Text = "9,999,999,999"
        Me.txtTG_TIMEPMONEY.Top = 0.1968504!
        Me.txtTG_TIMEPMONEY.Width = 0.984252!
        '
        'SUM_KG_TIMEPMONEY
        '
        Me.SUM_KG_TIMEPMONEY.DataField = "繰越Ｅマネー入金額"
        Me.SUM_KG_TIMEPMONEY.Height = 0.1377953!
        Me.SUM_KG_TIMEPMONEY.Left = 4.527559!
        Me.SUM_KG_TIMEPMONEY.Name = "SUM_KG_TIMEPMONEY"
        Me.SUM_KG_TIMEPMONEY.OutputFormat = resources.GetString("SUM_KG_TIMEPMONEY.OutputFormat")
        Me.SUM_KG_TIMEPMONEY.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: right"
        Me.SUM_KG_TIMEPMONEY.Text = "9,999,999,999"
        Me.SUM_KG_TIMEPMONEY.Top = 0.511811!
        Me.SUM_KG_TIMEPMONEY.Width = 0.984252!
        '
        'Label9
        '
        Me.Label9.Height = 0.1377953!
        Me.Label9.HyperLink = Nothing
        Me.Label9.Left = 1.515357!
        Me.Label9.Name = "Label9"
        Me.Label9.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: right"
        Me.Label9.Text = "当日合計"
        Me.Label9.Top = 0.1968504!
        Me.Label9.Width = 1.446063!
        '
        'txtKS_TIMESHOUH
        '
        Me.txtKS_TIMESHOUH.DataField = "精算機_繰越清算金額"
        Me.txtKS_TIMESHOUH.Height = 0.1377953!
        Me.txtKS_TIMESHOUH.Left = 5.708662!
        Me.txtKS_TIMESHOUH.Name = "txtKS_TIMESHOUH"
        Me.txtKS_TIMESHOUH.OutputFormat = resources.GetString("txtKS_TIMESHOUH.OutputFormat")
        Me.txtKS_TIMESHOUH.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: right"
        Me.txtKS_TIMESHOUH.Text = "9,999,999,999"
        Me.txtKS_TIMESHOUH.Top = 0.6692914!
        Me.txtKS_TIMESHOUH.Width = 0.984252!
        '
        'txtTG_TIMENYUKIN
        '
        Me.txtTG_TIMENYUKIN.DataField = "当日現金入金金額"
        Me.txtTG_TIMENYUKIN.Height = 0.1377953!
        Me.txtTG_TIMENYUKIN.Left = 3.543307!
        Me.txtTG_TIMENYUKIN.Name = "txtTG_TIMENYUKIN"
        Me.txtTG_TIMENYUKIN.OutputFormat = resources.GetString("txtTG_TIMENYUKIN.OutputFormat")
        Me.txtTG_TIMENYUKIN.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: right"
        Me.txtTG_TIMENYUKIN.Text = "9,999,999,999"
        Me.txtTG_TIMENYUKIN.Top = 0.1968504!
        Me.txtTG_TIMENYUKIN.Width = 0.984252!
        '
        'txtKG_TIMETAX
        '
        Me.txtKG_TIMETAX.DataField = "繰越消費税額"
        Me.txtKG_TIMETAX.Height = 0.1377953!
        Me.txtKG_TIMETAX.Left = 6.889764!
        Me.txtKG_TIMETAX.Name = "txtKG_TIMETAX"
        Me.txtKG_TIMETAX.OutputFormat = resources.GetString("txtKG_TIMETAX.OutputFormat")
        Me.txtKG_TIMETAX.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: right"
        Me.txtKG_TIMETAX.Text = "9,999,999,999"
        Me.txtKG_TIMETAX.Top = 0.511811!
        Me.txtKG_TIMETAX.Width = 0.984252!
        '
        'txtKG_TIMEPTAX
        '
        Me.txtKG_TIMEPTAX.DataField = "繰越Ｅマネー消費税額"
        Me.txtKG_TIMEPTAX.Height = 0.1377953!
        Me.txtKG_TIMEPTAX.Left = 7.874016!
        Me.txtKG_TIMEPTAX.Name = "txtKG_TIMEPTAX"
        Me.txtKG_TIMEPTAX.OutputFormat = resources.GetString("txtKG_TIMEPTAX.OutputFormat")
        Me.txtKG_TIMEPTAX.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: right"
        Me.txtKG_TIMEPTAX.Text = "9,999,999,999"
        Me.txtKG_TIMEPTAX.Top = 0.511811!
        Me.txtKG_TIMEPTAX.Width = 0.984252!
        '
        'Line1
        '
        Me.Line1.Height = 0.0!
        Me.Line1.Left = 0.0!
        Me.Line1.LineWeight = 1.0!
        Me.Line1.Name = "Line1"
        Me.Line1.Top = 0.0!
        Me.Line1.Width = 11.02363!
        Me.Line1.X1 = 0.0!
        Me.Line1.X2 = 11.02363!
        Me.Line1.Y1 = 0.0!
        Me.Line1.Y2 = 0.0!
        '
        'txtKG_TIMENYUKIN
        '
        Me.txtKG_TIMENYUKIN.DataField = "繰越現金入金金額"
        Me.txtKG_TIMENYUKIN.Height = 0.1377953!
        Me.txtKG_TIMENYUKIN.Left = 3.543307!
        Me.txtKG_TIMENYUKIN.Name = "txtKG_TIMENYUKIN"
        Me.txtKG_TIMENYUKIN.OutputFormat = resources.GetString("txtKG_TIMENYUKIN.OutputFormat")
        Me.txtKG_TIMENYUKIN.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: right"
        Me.txtKG_TIMENYUKIN.Text = "9,999,999,999"
        Me.txtKG_TIMENYUKIN.Top = 0.511811!
        Me.txtKG_TIMENYUKIN.Width = 0.984252!
        '
        'txtTS_TIMESHOUH
        '
        Me.txtTS_TIMESHOUH.DataField = "精算機_当日清算金額"
        Me.txtTS_TIMESHOUH.Height = 0.1377953!
        Me.txtTS_TIMESHOUH.Left = 5.708662!
        Me.txtTS_TIMESHOUH.Name = "txtTS_TIMESHOUH"
        Me.txtTS_TIMESHOUH.OutputFormat = resources.GetString("txtTS_TIMESHOUH.OutputFormat")
        Me.txtTS_TIMESHOUH.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: right"
        Me.txtTS_TIMESHOUH.Text = "9,999,999,999"
        Me.txtTS_TIMESHOUH.Top = 0.3543307!
        Me.txtTS_TIMESHOUH.Width = 0.984252!
        '
        'txtTG_TIMETAX
        '
        Me.txtTG_TIMETAX.DataField = "当日消費税額"
        Me.txtTG_TIMETAX.Height = 0.1377953!
        Me.txtTG_TIMETAX.Left = 6.889764!
        Me.txtTG_TIMETAX.Name = "txtTG_TIMETAX"
        Me.txtTG_TIMETAX.OutputFormat = resources.GetString("txtTG_TIMETAX.OutputFormat")
        Me.txtTG_TIMETAX.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: right"
        Me.txtTG_TIMETAX.Text = "9,999,999,999"
        Me.txtTG_TIMETAX.Top = 0.1968504!
        Me.txtTG_TIMETAX.Width = 0.984252!
        '
        'txtTG_PPREMIUMTOTAL
        '
        Me.txtTG_PPREMIUMTOTAL.DataField = "当日Ｅマネープレミアム金額"
        Me.txtTG_PPREMIUMTOTAL.Height = 0.1377953!
        Me.txtTG_PPREMIUMTOTAL.Left = 10.03937!
        Me.txtTG_PPREMIUMTOTAL.Name = "txtTG_PPREMIUMTOTAL"
        Me.txtTG_PPREMIUMTOTAL.OutputFormat = resources.GetString("txtTG_PPREMIUMTOTAL.OutputFormat")
        Me.txtTG_PPREMIUMTOTAL.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: right"
        Me.txtTG_PPREMIUMTOTAL.Text = "9,999,999,999"
        Me.txtTG_PPREMIUMTOTAL.Top = 0.1968504!
        Me.txtTG_PPREMIUMTOTAL.Width = 0.984252!
        '
        'txtKG_KURIPPREMIUMTOTAL
        '
        Me.txtKG_KURIPPREMIUMTOTAL.DataField = "繰越Ｅマネープレミアム金額"
        Me.txtKG_KURIPPREMIUMTOTAL.Height = 0.1377953!
        Me.txtKG_KURIPPREMIUMTOTAL.Left = 10.03937!
        Me.txtKG_KURIPPREMIUMTOTAL.Name = "txtKG_KURIPPREMIUMTOTAL"
        Me.txtKG_KURIPPREMIUMTOTAL.OutputFormat = resources.GetString("txtKG_KURIPPREMIUMTOTAL.OutputFormat")
        Me.txtKG_KURIPPREMIUMTOTAL.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: right"
        Me.txtKG_KURIPPREMIUMTOTAL.Text = "9,999,999,999"
        Me.txtKG_KURIPPREMIUMTOTAL.Top = 0.511811!
        Me.txtKG_KURIPPREMIUMTOTAL.Width = 0.984252!
        '
        'txtHALL_NAME
        '
        Me.txtHALL_NAME.DataField = "店舗名"
        Me.txtHALL_NAME.Height = 0.1377953!
        Me.txtHALL_NAME.Left = 1.181102!
        Me.txtHALL_NAME.MultiLine = False
        Me.txtHALL_NAME.Name = "txtHALL_NAME"
        Me.txtHALL_NAME.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt"
        Me.txtHALL_NAME.Text = "１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０"
        Me.txtHALL_NAME.Top = 0.03937008!
        Me.txtHALL_NAME.Width = 3.937008!
        '
        'Label2
        '
        Me.Label2.Height = 0.1377953!
        Me.Label2.HyperLink = Nothing
        Me.Label2.Left = 5.11811!
        Me.Label2.Name = "Label2"
        Me.Label2.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: left"
        Me.Label2.Text = "】"
        Me.Label2.Top = 0.03937008!
        Me.Label2.Width = 0.1574803!
        '
        'Label3
        '
        Me.Label3.Height = 0.1377953!
        Me.Label3.HyperLink = Nothing
        Me.Label3.Left = 0.984252!
        Me.Label3.Name = "Label3"
        Me.Label3.Style = "font-family: ＭＳ ゴシック; font-size: 8.25pt; text-align: right"
        Me.Label3.Text = "【"
        Me.Label3.Top = 0.03937009!
        Me.Label3.Width = 0.1968504!
        '
        'txtTENPO
        '
        Me.txtTENPO.DataField = "小計名"
        Me.txtTENPO.Height = 0.1574803!
        Me.txtTENPO.Left = 0.1968504!
        Me.txtTENPO.Name = "txtTENPO"
        Me.txtTENPO.Text = "店舗99計"
        Me.txtTENPO.Top = 0.03937006!
        Me.txtTENPO.Width = 0.7874016!
        '
        'TBRREP013_2
        '
        Me.MasterReport = False
        Me.PageSettings.DefaultPaperSize = False
        Me.PageSettings.Margins.Bottom = 0.1968504!
        Me.PageSettings.Margins.Left = 0.1968504!
        Me.PageSettings.Margins.Right = 0.1968504!
        Me.PageSettings.Margins.Top = 0.1968504!
        Me.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape
        Me.PageSettings.PaperHeight = 11.41732!
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Custom
        Me.PageSettings.PaperName = "ユーザー定義のサイズ"
        Me.PageSettings.PaperWidth = 8.661417!
        Me.PrintWidth = 11.22047!
        Me.ScriptLanguage = "VB.NET"
        Me.Sections.Add(Me.Detail1)
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; " & _
            "color: Black; font-family: ""MS UI Gothic""; ddo-char-set: 128", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: ""MS UI Gothic""; ddo-char-set: 12" & _
            "8", "Heading1", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: ""MS UI Goth" & _
            "ic""; ddo-char-set: 128", "Heading2", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"))
        CType(Me.txtTG_TIMESHOUH, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTG_TIMEPTAX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKG_TIMEPREMIUM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKG_TIMESHOUH, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTG_TIMEPREMIUM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTG_TIMEPMONEY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SUM_KG_TIMEPMONEY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKS_TIMESHOUH, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTG_TIMENYUKIN, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKG_TIMETAX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKG_TIMEPTAX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKG_TIMENYUKIN, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTS_TIMESHOUH, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTG_TIMETAX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTG_PPREMIUMTOTAL, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtKG_KURIPPREMIUMTOTAL, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtHALL_NAME, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTENPO, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Private WithEvents txtTG_TIMEPREMIUM As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTG_TIMEPTAX As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label6 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label8 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtKG_TIMEPREMIUM As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtKG_TIMESHOUH As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label5 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtTG_TIMEPMONEY As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents SUM_KG_TIMEPMONEY As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label9 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtKS_TIMESHOUH As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTG_TIMESHOUH As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtKG_TIMETAX As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtKG_TIMEPTAX As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Line1 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents txtTG_TIMENYUKIN As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtKG_TIMENYUKIN As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTS_TIMESHOUH As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTG_TIMETAX As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtKG_KURIPPREMIUMTOTAL As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTG_PPREMIUMTOTAL As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtHALL_NAME As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label2 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label3 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtTENPO As GrapeCity.ActiveReports.SectionReportModel.TextBox
End Class
