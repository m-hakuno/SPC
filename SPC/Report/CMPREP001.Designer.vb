<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class CMPREP001
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
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(CMPREP001))
        Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
        Me.Label9 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblTitle = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TxtDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label4 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label7 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TxtOthTtl = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtClctDt = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblClctDt = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblSystemCls = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TxtSystemCls = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtCln = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtCln2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail()
        Me.PREFECTURE = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
        Me.ReportHeader1 = New GrapeCity.ActiveReports.SectionReportModel.ReportHeader()
        Me.ReportFooter1 = New GrapeCity.ActiveReports.SectionReportModel.ReportFooter()
        Me.Label10 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.GroupHeader1 = New GrapeCity.ActiveReports.SectionReportModel.GroupHeader()
        Me.GroupFooter1 = New GrapeCity.ActiveReports.SectionReportModel.GroupFooter()
        CType(Me.Label9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTitle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtOthTtl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtClctDt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblClctDt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblSystemCls, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtSystemCls, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtCln, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtCln2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PREFECTURE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'PageHeader1
        '
        Me.PageHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.Label9, Me.lblTitle, Me.TxtDate, Me.TextBox2, Me.Label4, Me.Label7, Me.TxtOthTtl, Me.TxtClctDt, Me.lblClctDt, Me.lblSystemCls, Me.TxtSystemCls, Me.txtCln, Me.txtCln2})
        Me.PageHeader1.Height = 1.456693!
        Me.PageHeader1.Name = "PageHeader1"
        '
        'Label9
        '
        Me.Label9.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label9.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label9.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label9.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label9.Height = 0.1968504!
        Me.Label9.HyperLink = Nothing
        Me.Label9.Left = 0.1968504!
        Me.Label9.Name = "Label9"
        Me.Label9.Padding = New GrapeCity.ActiveReports.PaddingEx(3, 0, 0, 0)
        Me.Label9.Style = "font-size: 8pt; text-align: center; vertical-align: middle; ddo-char-set: 1"
        Me.Label9.Text = "都道府県名"
        Me.Label9.Top = 1.259843!
        Me.Label9.Width = 0.7086614!
        '
        'lblTitle
        '
        Me.lblTitle.DataField = "タイトル"
        Me.lblTitle.Height = 0.2362205!
        Me.lblTitle.HyperLink = Nothing
        Me.lblTitle.Left = 0.1968504!
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Style = "font-size: 13pt; font-weight: bold; text-align: center; ddo-char-set: 1"
        Me.lblTitle.Text = "保守料金明細一覧表"
        Me.lblTitle.Top = 0.1968504!
        Me.lblTitle.Width = 10.82677!
        '
        'TxtDate
        '
        Me.TxtDate.DataField = "=System.DateTime.Now.ToString(""yyyy年MM月dd日"")"
        Me.TxtDate.Height = 0.1574803!
        Me.TxtDate.Left = 9.448819!
        Me.TxtDate.Name = "TxtDate"
        Me.TxtDate.Padding = New GrapeCity.ActiveReports.PaddingEx(0, 0, 3, 0)
        Me.TxtDate.Style = "font-size: 8pt; text-align: right; vertical-align: bottom; ddo-char-set: 1"
        Me.TxtDate.Text = "2014年01月23日"
        Me.TxtDate.Top = 0.2755906!
        Me.TxtDate.Width = 1.574803!
        '
        'TextBox2
        '
        Me.TextBox2.Height = 0.1574803!
        Me.TextBox2.Left = 8.54252!
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Padding = New GrapeCity.ActiveReports.PaddingEx(0, 0, 3, 0)
        Me.TextBox2.Style = "font-size: 8pt; text-align: right; vertical-align: bottom; ddo-char-set: 1"
        Me.TextBox2.Text = "エフ・エス㈱　サポートセンタ"
        Me.TextBox2.Top = 0.4330709!
        Me.TextBox2.Width = 2.481102!
        '
        'Label4
        '
        Me.Label4.Height = 0.1574803!
        Me.Label4.HyperLink = Nothing
        Me.Label4.Left = 10.82677!
        Me.Label4.Name = "Label4"
        Me.Label4.Padding = New GrapeCity.ActiveReports.PaddingEx(0, 0, 3, 0)
        Me.Label4.Style = "font-size: 8pt; text-align: right; ddo-char-set: 1"
        Me.Label4.Text = "件"
        Me.Label4.Top = 1.023622!
        Me.Label4.Width = 0.1968504!
        '
        'Label7
        '
        Me.Label7.Height = 0.1574803!
        Me.Label7.HyperLink = Nothing
        Me.Label7.Left = 8.858269!
        Me.Label7.Name = "Label7"
        Me.Label7.Padding = New GrapeCity.ActiveReports.PaddingEx(0, 0, 3, 0)
        Me.Label7.Style = "font-size: 8pt; text-align: right; ddo-char-set: 1"
        Me.Label7.Text = "総合計"
        Me.Label7.Top = 1.023622!
        Me.Label7.Width = 1.102362!
        '
        'TxtOthTtl
        '
        Me.TxtOthTtl.Height = 0.1574803!
        Me.TxtOthTtl.Left = 9.96063!
        Me.TxtOthTtl.Name = "TxtOthTtl"
        Me.TxtOthTtl.Padding = New GrapeCity.ActiveReports.PaddingEx(0, 0, 3, 0)
        Me.TxtOthTtl.Style = "font-size: 8pt; text-align: right; vertical-align: top; ddo-char-set: 1"
        Me.TxtOthTtl.Text = "9999999999"
        Me.TxtOthTtl.Top = 1.023622!
        Me.TxtOthTtl.Width = 0.8661423!
        '
        'TxtClctDt
        '
        Me.TxtClctDt.Height = 0.1574803!
        Me.TxtClctDt.Left = 1.108662!
        Me.TxtClctDt.Name = "TxtClctDt"
        Me.TxtClctDt.OutputFormat = resources.GetString("TxtClctDt.OutputFormat")
        Me.TxtClctDt.Padding = New GrapeCity.ActiveReports.PaddingEx(0, 0, 3, 0)
        Me.TxtClctDt.Style = "font-size: 10pt; text-align: left; vertical-align: top; ddo-char-set: 1"
        Me.TxtClctDt.Text = "2014年01月23日"
        Me.TxtClctDt.Top = 1.023622!
        Me.TxtClctDt.Width = 1.161417!
        '
        'lblClctDt
        '
        Me.lblClctDt.Height = 0.1574803!
        Me.lblClctDt.HyperLink = Nothing
        Me.lblClctDt.Left = 0.1968504!
        Me.lblClctDt.Name = "lblClctDt"
        Me.lblClctDt.Padding = New GrapeCity.ActiveReports.PaddingEx(3, 0, 0, 0)
        Me.lblClctDt.Style = "font-size: 10pt; text-align: left; ddo-char-set: 1"
        Me.lblClctDt.Text = "集　計　日"
        Me.lblClctDt.Top = 1.023622!
        Me.lblClctDt.Width = 0.8129922!
        '
        'lblSystemCls
        '
        Me.lblSystemCls.Height = 0.1574803!
        Me.lblSystemCls.HyperLink = Nothing
        Me.lblSystemCls.Left = 0.1968503!
        Me.lblSystemCls.Name = "lblSystemCls"
        Me.lblSystemCls.Padding = New GrapeCity.ActiveReports.PaddingEx(3, 0, 0, 0)
        Me.lblSystemCls.Style = "font-size: 10pt; text-align: left; ddo-char-set: 1"
        Me.lblSystemCls.Text = "システム分類"
        Me.lblSystemCls.Top = 0.8661418!
        Me.lblSystemCls.Width = 0.8129923!
        '
        'TxtSystemCls
        '
        Me.TxtSystemCls.Height = 0.1574803!
        Me.TxtSystemCls.Left = 1.108662!
        Me.TxtSystemCls.Name = "TxtSystemCls"
        Me.TxtSystemCls.OutputFormat = resources.GetString("TxtSystemCls.OutputFormat")
        Me.TxtSystemCls.Padding = New GrapeCity.ActiveReports.PaddingEx(0, 0, 3, 0)
        Me.TxtSystemCls.Style = "font-size: 10pt; text-align: left; vertical-align: top; ddo-char-set: 1"
        Me.TxtSystemCls.Text = "IDorICorLUTERNA"
        Me.TxtSystemCls.Top = 0.8661418!
        Me.TxtSystemCls.Width = 0.9842521!
        '
        'txtCln
        '
        Me.txtCln.Height = 0.1574803!
        Me.txtCln.Left = 1.009843!
        Me.txtCln.Name = "txtCln"
        Me.txtCln.OutputFormat = resources.GetString("txtCln.OutputFormat")
        Me.txtCln.Padding = New GrapeCity.ActiveReports.PaddingEx(0, 0, 3, 0)
        Me.txtCln.Style = "font-size: 10pt; text-align: left; vertical-align: top; ddo-char-set: 1"
        Me.txtCln.Text = ":"
        Me.txtCln.Top = 0.8661418!
        Me.txtCln.Width = 0.0988189!
        '
        'txtCln2
        '
        Me.txtCln2.Height = 0.1574803!
        Me.txtCln2.Left = 1.009843!
        Me.txtCln2.Name = "txtCln2"
        Me.txtCln2.OutputFormat = resources.GetString("txtCln2.OutputFormat")
        Me.txtCln2.Padding = New GrapeCity.ActiveReports.PaddingEx(0, 0, 3, 0)
        Me.txtCln2.Style = "font-size: 10pt; text-align: left; vertical-align: top; ddo-char-set: 1"
        Me.txtCln2.Text = ":"
        Me.txtCln2.Top = 1.023622!
        Me.txtCln2.Width = 0.09881888!
        '
        'Detail1
        '
        Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.PREFECTURE})
        Me.Detail1.Height = 0.1968504!
        Me.Detail1.Name = "Detail1"
        '
        'PREFECTURE
        '
        Me.PREFECTURE.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.PREFECTURE.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.PREFECTURE.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.PREFECTURE.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.PREFECTURE.DataField = "都道府県名"
        Me.PREFECTURE.Height = 0.1968504!
        Me.PREFECTURE.Left = 0.1968503!
        Me.PREFECTURE.Name = "PREFECTURE"
        Me.PREFECTURE.OutputFormat = resources.GetString("PREFECTURE.OutputFormat")
        Me.PREFECTURE.Style = "font-size: 8pt; text-align: center; vertical-align: middle; ddo-char-set: 1"
        Me.PREFECTURE.Text = "北海道"
        Me.PREFECTURE.Top = 0.0!
        Me.PREFECTURE.Width = 0.7086616!
        '
        'PageFooter1
        '
        Me.PageFooter1.Height = 0.3937008!
        Me.PageFooter1.Name = "PageFooter1"
        '
        'ReportHeader1
        '
        Me.ReportHeader1.Height = 0.0!
        Me.ReportHeader1.Name = "ReportHeader1"
        '
        'ReportFooter1
        '
        Me.ReportFooter1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.Label10})
        Me.ReportFooter1.Height = 0.3937008!
        Me.ReportFooter1.Name = "ReportFooter1"
        '
        'Label10
        '
        Me.Label10.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label10.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label10.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label10.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label10.Height = 0.1968504!
        Me.Label10.HyperLink = Nothing
        Me.Label10.Left = 0.1968504!
        Me.Label10.Name = "Label10"
        Me.Label10.Padding = New GrapeCity.ActiveReports.PaddingEx(3, 0, 0, 0)
        Me.Label10.Style = "font-size: 8pt; text-align: center; vertical-align: middle; ddo-char-set: 1"
        Me.Label10.Text = "総合計"
        Me.Label10.Top = 0.07874016!
        Me.Label10.Width = 0.7086611!
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
        'CMPREP001
        '
        Me.MasterReport = False
        Me.PageSettings.Margins.Bottom = 0.1968504!
        Me.PageSettings.Margins.Left = 0.1968504!
        Me.PageSettings.Margins.Right = 0.1968504!
        Me.PageSettings.Margins.Top = 0.1968504!
        Me.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape
        Me.PageSettings.PaperHeight = 11.0!
        Me.PageSettings.PaperWidth = 8.5!
        Me.PrintWidth = 11.02362!
        Me.ScriptLanguage = "VB.NET"
        Me.Sections.Add(Me.ReportHeader1)
        Me.Sections.Add(Me.PageHeader1)
        Me.Sections.Add(Me.GroupHeader1)
        Me.Sections.Add(Me.Detail1)
        Me.Sections.Add(Me.GroupFooter1)
        Me.Sections.Add(Me.PageFooter1)
        Me.Sections.Add(Me.ReportFooter1)
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; " & _
            "color: Black; font-family: ""MS UI Gothic""; ddo-char-set: 128", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: ""MS UI Gothic""; ddo-char-set: 12" & _
            "8", "Heading1", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: ""MS UI Goth" & _
            "ic""; ddo-char-set: 128", "Heading2", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"))
        CType(Me.Label9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTitle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtOthTtl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtClctDt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblClctDt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblSystemCls, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtSystemCls, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtCln, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtCln2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PREFECTURE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Private WithEvents ReportHeader1 As GrapeCity.ActiveReports.SectionReportModel.ReportHeader
    Private WithEvents ReportFooter1 As GrapeCity.ActiveReports.SectionReportModel.ReportFooter
    Private WithEvents lblTitle As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TxtDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label4 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label7 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TxtOthTtl As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label9 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TxtClctDt As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblClctDt As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents PREFECTURE As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label10 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents GroupHeader1 As GrapeCity.ActiveReports.SectionReportModel.GroupHeader
    Private WithEvents GroupFooter1 As GrapeCity.ActiveReports.SectionReportModel.GroupFooter
    Private WithEvents lblSystemCls As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TxtSystemCls As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtCln As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtCln2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
End Class
