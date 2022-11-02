<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class CNSREP009_1
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
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(CNSREP009_1))
        Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
        Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail()
        Me.Label67 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox294 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtAMOUNT = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label61 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label59 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label60 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TxtNAME = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label68 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TxtQUANT = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
        CType(Me.Label67, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox294, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtAMOUNT, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label61, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label59, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label60, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtNAME, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label68, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtQUANT, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'PageHeader1
        '
        Me.PageHeader1.Height = 0.0!
        Me.PageHeader1.Name = "PageHeader1"
        '
        'Detail1
        '
        Me.Detail1.ColumnCount = 2
        Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.Label67, Me.TextBox294, Me.TxtAMOUNT, Me.Label61, Me.Label59, Me.Label60, Me.TxtNAME, Me.Label68, Me.TxtQUANT})
        Me.Detail1.Height = 0.1645503!
        Me.Detail1.Name = "Detail1"
        '
        'Label67
        '
        Me.Label67.Height = 0.166519!
        Me.Label67.HyperLink = Nothing
        Me.Label67.Left = 3.399608!
        Me.Label67.Name = "Label67"
        Me.Label67.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; vertical-align: middle"
        Me.Label67.Text = "円"
        Me.Label67.Top = 0.0007876009!
        Me.Label67.Width = 0.1665351!
        '
        'TextBox294
        '
        Me.TextBox294.DataField = "名称"
        Me.TextBox294.Height = 0.1665356!
        Me.TextBox294.Left = 0.2185056!
        Me.TextBox294.Name = "TextBox294"
        Me.TextBox294.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; vertical-align: middle"
        Me.TextBox294.Text = "(1)TBOX T500 本体"
        Me.TextBox294.Top = 0.0003940016!
        Me.TextBox294.Width = 1.776378!
        '
        'TxtAMOUNT
        '
        Me.TxtAMOUNT.DataField = "金額"
        Me.TxtAMOUNT.Height = 0.1661417!
        Me.TxtAMOUNT.Left = 2.596458!
        Me.TxtAMOUNT.Name = "TxtAMOUNT"
        Me.TxtAMOUNT.OutputFormat = resources.GetString("TxtAMOUNT.OutputFormat")
        Me.TxtAMOUNT.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right; vertical-align: middl" & _
    "e"
        Me.TxtAMOUNT.Text = "99,999,999"
        Me.TxtAMOUNT.Top = 0.0011812!
        Me.TxtAMOUNT.Width = 0.722441!
        '
        'Label61
        '
        Me.Label61.Height = 0.166519!
        Me.Label61.HyperLink = Nothing
        Me.Label61.Left = 2.408663!
        Me.Label61.Name = "Label61"
        Me.Label61.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left; vertical-align: middle" & _
    ""
        Me.Label61.Text = ")"
        Me.Label61.Top = 0.0007876009!
        Me.Label61.Width = 0.08306354!
        '
        'Label59
        '
        Me.Label59.Height = 0.166519!
        Me.Label59.HyperLink = Nothing
        Me.Label59.Left = 3.316537!
        Me.Label59.Name = "Label59"
        Me.Label59.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left; vertical-align: middle" & _
    ""
        Me.Label59.Text = ")"
        Me.Label59.Top = 0.0007881969!
        Me.Label59.Width = 0.08306354!
        '
        'Label60
        '
        Me.Label60.Height = 0.166519!
        Me.Label60.HyperLink = Nothing
        Me.Label60.Left = 1.994883!
        Me.Label60.Name = "Label60"
        Me.Label60.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; vertical-align: middle"
        Me.Label60.Text = "("
        Me.Label60.Top = 0.0007876009!
        Me.Label60.Width = 0.08306354!
        '
        'TxtNAME
        '
        Me.TxtNAME.DataField = "名称"
        Me.TxtNAME.Height = 0.1665356!
        Me.TxtNAME.Left = 0.03858268!
        Me.TxtNAME.Name = "TxtNAME"
        Me.TxtNAME.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; vertical-align: middle"
        Me.TxtNAME.Text = "１. TBOX機器費"
        Me.TxtNAME.Top = 0.0!
        Me.TxtNAME.Width = 1.583071!
        '
        'Label68
        '
        Me.Label68.Height = 0.166519!
        Me.Label68.HyperLink = Nothing
        Me.Label68.Left = 2.509057!
        Me.Label68.Name = "Label68"
        Me.Label68.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; vertical-align: middle"
        Me.Label68.Text = "("
        Me.Label68.Top = 0.0007876009!
        Me.Label68.Width = 0.08306354!
        '
        'TxtQUANT
        '
        Me.TxtQUANT.DataField = "数量"
        Me.TxtQUANT.Height = 0.1661417!
        Me.TxtQUANT.Left = 2.081104!
        Me.TxtQUANT.Name = "TxtQUANT"
        Me.TxtQUANT.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right; vertical-align: middl" & _
    "e"
        Me.TxtQUANT.Text = "9999"
        Me.TxtQUANT.Top = 0.0003940016!
        Me.TxtQUANT.Width = 0.327559!
        '
        'PageFooter1
        '
        Me.PageFooter1.Height = 0.0!
        Me.PageFooter1.Name = "PageFooter1"
        '
        'CNSREP009_1
        '
        Me.MasterReport = False
        Me.PageSettings.DefaultPaperSize = False
        Me.PageSettings.Margins.Bottom = 0.07874016!
        Me.PageSettings.Margins.Left = 0.1574803!
        Me.PageSettings.Margins.Right = 0.1574803!
        Me.PageSettings.Margins.Top = 0.07874016!
        Me.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Portrait
        Me.PageSettings.PaperHeight = 11.69291!
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4
        Me.PageSettings.PaperWidth = 8.267716!
        Me.PrintWidth = 7.9375!
        Me.Sections.Add(Me.PageHeader1)
        Me.Sections.Add(Me.Detail1)
        Me.Sections.Add(Me.PageFooter1)
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; " & _
            "color: Black; font-family: ""MS UI Gothic""; ddo-char-set: 128", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: ""MS UI Gothic""; ddo-char-set: 12" & _
            "8", "Heading1", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: ""MS UI Goth" & _
            "ic""; ddo-char-set: 128", "Heading2", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"))
        CType(Me.Label67, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox294, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtAMOUNT, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label61, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label59, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label60, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtNAME, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label68, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtQUANT, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Private WithEvents Label67 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox294 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtAMOUNT As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label61 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label59 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label60 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TxtNAME As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label68 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TxtQUANT As GrapeCity.ActiveReports.SectionReportModel.TextBox
End Class
