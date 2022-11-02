<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class CNSREP009_2
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
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(CNSREP009_2))
        Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
        Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail()
        Me.TxtNAME = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtBEF_CONST = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label125 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label128 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TxtRELOCATE = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label133 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label136 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TxtREMOVE = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label143 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label146 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TxtNEW = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label147 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label153 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TxtREMOVE2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label154 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label325 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TxtAFTER_CONST = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label326 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label327 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
        CType(Me.TxtNAME, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtBEF_CONST, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label125, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label128, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtRELOCATE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label133, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label136, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtREMOVE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label143, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label146, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtNEW, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label147, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label153, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtREMOVE2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label154, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label325, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtAFTER_CONST, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label326, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label327, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TxtNAME, Me.TxtBEF_CONST, Me.Label125, Me.Label128, Me.TxtRELOCATE, Me.Label133, Me.Label136, Me.TxtREMOVE, Me.Label143, Me.Label146, Me.TxtNEW, Me.Label147, Me.Label153, Me.TxtREMOVE2, Me.Label154, Me.Label325, Me.TxtAFTER_CONST, Me.Label326, Me.Label327})
        Me.Detail1.Height = 0.1749671!
        Me.Detail1.Name = "Detail1"
        '
        'TxtNAME
        '
        Me.TxtNAME.DataField = "名称"
        Me.TxtNAME.Height = 0.1665356!
        Me.TxtNAME.Left = 0.03858268!
        Me.TxtNAME.Name = "TxtNAME"
        Me.TxtNAME.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.TxtNAME.Text = "24)LANｹｰﾌﾞﾙﾒｰﾄﾙ本数"
        Me.TxtNAME.Top = 0.0!
        Me.TxtNAME.Width = 1.315748!
        '
        'TxtBEF_CONST
        '
        Me.TxtBEF_CONST.DataField = "工事前"
        Me.TxtBEF_CONST.Height = 0.1661417!
        Me.TxtBEF_CONST.Left = 1.437402!
        Me.TxtBEF_CONST.Name = "TxtBEF_CONST"
        Me.TxtBEF_CONST.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right; vertical-align: middl" & _
    "e"
        Me.TxtBEF_CONST.Text = "9999"
        Me.TxtBEF_CONST.Top = 0.0!
        Me.TxtBEF_CONST.Width = 0.3055118!
        '
        'Label125
        '
        Me.Label125.Height = 0.166519!
        Me.Label125.HyperLink = Nothing
        Me.Label125.Left = 1.354331!
        Me.Label125.Name = "Label125"
        Me.Label125.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; vertical-align: middle"
        Me.Label125.Text = "("
        Me.Label125.Top = 0.0!
        Me.Label125.Width = 0.08306354!
        '
        'Label128
        '
        Me.Label128.Height = 0.166519!
        Me.Label128.HyperLink = Nothing
        Me.Label128.Left = 1.742911!
        Me.Label128.Name = "Label128"
        Me.Label128.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left; vertical-align: middle" & _
    ""
        Me.Label128.Text = ")"
        Me.Label128.Top = 0.0!
        Me.Label128.Width = 0.08306354!
        '
        'TxtRELOCATE
        '
        Me.TxtRELOCATE.DataField = "移設"
        Me.TxtRELOCATE.Height = 0.1661417!
        Me.TxtRELOCATE.Left = 1.865748!
        Me.TxtRELOCATE.Name = "TxtRELOCATE"
        Me.TxtRELOCATE.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right; vertical-align: middl" & _
    "e"
        Me.TxtRELOCATE.Text = "9999"
        Me.TxtRELOCATE.Top = 0.0!
        Me.TxtRELOCATE.Width = 0.3055118!
        '
        'Label133
        '
        Me.Label133.Height = 0.166519!
        Me.Label133.HyperLink = Nothing
        Me.Label133.Left = 1.782677!
        Me.Label133.Name = "Label133"
        Me.Label133.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; vertical-align: middle"
        Me.Label133.Text = "("
        Me.Label133.Top = 0.001181102!
        Me.Label133.Width = 0.08306354!
        '
        'Label136
        '
        Me.Label136.Height = 0.166519!
        Me.Label136.HyperLink = Nothing
        Me.Label136.Left = 2.17126!
        Me.Label136.Name = "Label136"
        Me.Label136.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left; vertical-align: middle" & _
    ""
        Me.Label136.Text = ")"
        Me.Label136.Top = 0.005118111!
        Me.Label136.Width = 0.08306354!
        '
        'TxtREMOVE
        '
        Me.TxtREMOVE.DataField = "撤去"
        Me.TxtREMOVE.Height = 0.1661417!
        Me.TxtREMOVE.Left = 2.290158!
        Me.TxtREMOVE.Name = "TxtREMOVE"
        Me.TxtREMOVE.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right; vertical-align: middl" & _
    "e"
        Me.TxtREMOVE.Text = "9999"
        Me.TxtREMOVE.Top = 0.0!
        Me.TxtREMOVE.Width = 0.3055118!
        '
        'Label143
        '
        Me.Label143.Height = 0.166519!
        Me.Label143.HyperLink = Nothing
        Me.Label143.Left = 2.207087!
        Me.Label143.Name = "Label143"
        Me.Label143.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; vertical-align: middle"
        Me.Label143.Text = "("
        Me.Label143.Top = 0.0!
        Me.Label143.Width = 0.08306354!
        '
        'Label146
        '
        Me.Label146.Height = 0.166519!
        Me.Label146.HyperLink = Nothing
        Me.Label146.Left = 2.589764!
        Me.Label146.Name = "Label146"
        Me.Label146.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left; vertical-align: middle" & _
    ""
        Me.Label146.Text = ")"
        Me.Label146.Top = 0.0!
        Me.Label146.Width = 0.08306354!
        '
        'TxtNEW
        '
        Me.TxtNEW.DataField = "新設"
        Me.TxtNEW.Height = 0.1661417!
        Me.TxtNEW.Left = 2.71063!
        Me.TxtNEW.Name = "TxtNEW"
        Me.TxtNEW.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right; vertical-align: middl" & _
    "e"
        Me.TxtNEW.Text = "9999"
        Me.TxtNEW.Top = 0.0!
        Me.TxtNEW.Width = 0.3055118!
        '
        'Label147
        '
        Me.Label147.Height = 0.166519!
        Me.Label147.HyperLink = Nothing
        Me.Label147.Left = 2.627559!
        Me.Label147.Name = "Label147"
        Me.Label147.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; vertical-align: middle"
        Me.Label147.Text = "("
        Me.Label147.Top = 0.001181102!
        Me.Label147.Width = 0.08306354!
        '
        'Label153
        '
        Me.Label153.Height = 0.166519!
        Me.Label153.HyperLink = Nothing
        Me.Label153.Left = 3.016142!
        Me.Label153.Name = "Label153"
        Me.Label153.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left; vertical-align: middle" & _
    ""
        Me.Label153.Text = ")"
        Me.Label153.Top = 0.005118111!
        Me.Label153.Width = 0.08306354!
        '
        'TxtREMOVE2
        '
        Me.TxtREMOVE2.DataField = "撤去設"
        Me.TxtREMOVE2.Height = 0.1661417!
        Me.TxtREMOVE2.Left = 3.137795!
        Me.TxtREMOVE2.Name = "TxtREMOVE2"
        Me.TxtREMOVE2.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right; vertical-align: middl" & _
    "e"
        Me.TxtREMOVE2.Text = "9999"
        Me.TxtREMOVE2.Top = 0.0!
        Me.TxtREMOVE2.Width = 0.3055118!
        '
        'Label154
        '
        Me.Label154.Height = 0.166519!
        Me.Label154.HyperLink = Nothing
        Me.Label154.Left = 3.054725!
        Me.Label154.Name = "Label154"
        Me.Label154.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; vertical-align: middle"
        Me.Label154.Text = "("
        Me.Label154.Top = 0.005118111!
        Me.Label154.Width = 0.08306354!
        '
        'Label325
        '
        Me.Label325.Height = 0.166519!
        Me.Label325.HyperLink = Nothing
        Me.Label325.Left = 3.443307!
        Me.Label325.Name = "Label325"
        Me.Label325.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left; vertical-align: middle" & _
    ""
        Me.Label325.Text = ")"
        Me.Label325.Top = 0.0!
        Me.Label325.Width = 0.08306354!
        '
        'TxtAFTER_CONST
        '
        Me.TxtAFTER_CONST.DataField = "工事後"
        Me.TxtAFTER_CONST.Height = 0.1661417!
        Me.TxtAFTER_CONST.Left = 3.567323!
        Me.TxtAFTER_CONST.Name = "TxtAFTER_CONST"
        Me.TxtAFTER_CONST.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right; vertical-align: middl" & _
    "e"
        Me.TxtAFTER_CONST.Text = "9999"
        Me.TxtAFTER_CONST.Top = 0.0!
        Me.TxtAFTER_CONST.Width = 0.3055118!
        '
        'Label326
        '
        Me.Label326.Height = 0.166519!
        Me.Label326.HyperLink = Nothing
        Me.Label326.Left = 3.484252!
        Me.Label326.Name = "Label326"
        Me.Label326.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; vertical-align: middle"
        Me.Label326.Text = "("
        Me.Label326.Top = 0.0!
        Me.Label326.Width = 0.08306354!
        '
        'Label327
        '
        Me.Label327.Height = 0.166519!
        Me.Label327.HyperLink = Nothing
        Me.Label327.Left = 3.864961!
        Me.Label327.Name = "Label327"
        Me.Label327.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: left; vertical-align: middle" & _
    ""
        Me.Label327.Text = ")"
        Me.Label327.Top = 0.0!
        Me.Label327.Width = 0.08306354!
        '
        'PageFooter1
        '
        Me.PageFooter1.Height = 0.0!
        Me.PageFooter1.Name = "PageFooter1"
        '
        'CNSREP009_2
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
        Me.PrintWidth = 7.954176!
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
        CType(Me.TxtNAME, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtBEF_CONST, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label125, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label128, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtRELOCATE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label133, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label136, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtREMOVE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label143, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label146, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtNEW, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label147, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label153, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtREMOVE2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label154, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label325, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtAFTER_CONST, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label326, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label327, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Private WithEvents TxtNAME As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtBEF_CONST As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label125 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label128 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TxtRELOCATE As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label133 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label136 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TxtREMOVE As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label143 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label146 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TxtNEW As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label147 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label153 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TxtREMOVE2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label154 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label325 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TxtAFTER_CONST As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label326 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label327 As GrapeCity.ActiveReports.SectionReportModel.Label
End Class
