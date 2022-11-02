<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class BBPREP003
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
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(BBPREP003))
        Me.PageHeader = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
        Me.ReportInfo1 = New GrapeCity.ActiveReports.SectionReportModel.ReportInfo()
        Me.txtMyCompNM = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtMyPostNM = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label2 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
        Me.BBPREP003_2 = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
        Me.PageFooter = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
        Me.Label18 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.PageCount = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.PageMax = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label3 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.GroupHeader1 = New GrapeCity.ActiveReports.SectionReportModel.GroupHeader()
        Me.lblWest = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label9 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblEast = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblDetTTL = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtYear = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtMonth = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Line1 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.TextBox25 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox26 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox33 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox34 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox35 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox36 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox37 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox38 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox39 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox40 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox41 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label1 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label6 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label7 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label8 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Line2 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Label10 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label11 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label12 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label13 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Line3 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Label14 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label16 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label17 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label19 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Line4 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Label20 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label21 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label22 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label23 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Line5 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Label24 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label25 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label26 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label27 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Line6 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Label28 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label29 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label30 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label31 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Line7 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Label32 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label33 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label34 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label35 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Line8 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Label36 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label37 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label38 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label39 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Line9 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Label40 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label41 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label42 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label43 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Line10 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Label44 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label45 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label46 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label47 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Line11 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Label48 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label49 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label50 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label51 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Line12 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line13 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line14 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line15 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line16 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line17 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line18 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line19 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line20 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line21 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line22 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line23 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.txtNextYear = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Shape2 = New GrapeCity.ActiveReports.SectionReportModel.Shape()
        Me.GroupFooter1 = New GrapeCity.ActiveReports.SectionReportModel.GroupFooter()
        CType(Me.ReportInfo1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtMyCompNM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtMyPostNM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label18, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PageCount, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PageMax, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblWest, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblEast, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblDetTTL, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtYear, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtMonth, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox25, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox26, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox33, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox34, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox35, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox36, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox37, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox38, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox39, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox40, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox41, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label12, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label13, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label14, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label16, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label17, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label19, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label20, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label21, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label22, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label23, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label24, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label25, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label26, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label27, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label28, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label29, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label30, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label31, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label32, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label33, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label34, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label35, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label36, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label37, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label38, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label39, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label40, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label41, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label42, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label43, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label44, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label45, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label46, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label47, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label48, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label49, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label50, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label51, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtNextYear, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'PageHeader
        '
        Me.PageHeader.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.ReportInfo1, Me.txtMyCompNM, Me.txtMyPostNM, Me.Label2})
        Me.PageHeader.Height = 0.5905512!
        Me.PageHeader.Name = "PageHeader"
        '
        'ReportInfo1
        '
        Me.ReportInfo1.FormatString = "{RunDateTime:yyyy年M月d日}"
        Me.ReportInfo1.Height = 0.2!
        Me.ReportInfo1.Left = 5.875985!
        Me.ReportInfo1.MultiLine = False
        Me.ReportInfo1.Name = "ReportInfo1"
        Me.ReportInfo1.Style = "text-align: right"
        Me.ReportInfo1.Top = 0.0!
        Me.ReportInfo1.Width = 1.210629!
        '
        'txtMyCompNM
        '
        Me.txtMyCompNM.DataField = "自社名"
        Me.txtMyCompNM.Height = 0.1968504!
        Me.txtMyCompNM.Left = 5.636615!
        Me.txtMyCompNM.Name = "txtMyCompNM"
        Me.txtMyCompNM.Style = "text-align: right"
        Me.txtMyCompNM.Text = "エフ・エス株式会社"
        Me.txtMyCompNM.Top = 0.1968504!
        Me.txtMyCompNM.Width = 1.45!
        '
        'txtMyPostNM
        '
        Me.txtMyPostNM.DataField = "自部署名"
        Me.txtMyPostNM.Height = 0.2!
        Me.txtMyPostNM.Left = 5.636615!
        Me.txtMyPostNM.Name = "txtMyPostNM"
        Me.txtMyPostNM.Style = "text-align: right"
        Me.txtMyPostNM.Text = "サポートセンタ"
        Me.txtMyPostNM.Top = 0.3937008!
        Me.txtMyPostNM.Width = 1.45!
        '
        'Label2
        '
        Me.Label2.Height = 0.2362205!
        Me.Label2.HyperLink = Nothing
        Me.Label2.Left = 0.042126!
        Me.Label2.Name = "Label2"
        Me.Label2.Style = "font-family: ＭＳ ゴシック; font-size: 15.75pt; font-weight: bold; text-align: center"
        Me.Label2.Text = "券売機・サンドＢＢ１読出し　処理件数"
        Me.Label2.Top = 0.1574803!
        Me.Label2.Width = 7.044488!
        '
        'Detail
        '
        Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.BBPREP003_2})
        Me.Detail.Height = 9.645669!
        Me.Detail.Name = "Detail"
        '
        'BBPREP003_2
        '
        Me.BBPREP003_2.CloseBorder = False
        Me.BBPREP003_2.Height = 9.645669!
        Me.BBPREP003_2.Left = 1.732283!
        Me.BBPREP003_2.Name = "BBPREP003_2"
        Me.BBPREP003_2.Report = Nothing
        Me.BBPREP003_2.ReportName = "BBPREP003_2"
        Me.BBPREP003_2.Top = 0.0!
        Me.BBPREP003_2.Width = 5.433071!
        '
        'PageFooter
        '
        Me.PageFooter.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.Label18, Me.PageCount, Me.PageMax, Me.Label3})
        Me.PageFooter.Height = 0.3937008!
        Me.PageFooter.Name = "PageFooter"
        '
        'Label18
        '
        Me.Label18.Height = 0.1968504!
        Me.Label18.HyperLink = Nothing
        Me.Label18.Left = 0.3937008!
        Me.Label18.Name = "Label18"
        Me.Label18.Style = "text-justify: auto"
        Me.Label18.Text = "日付はブラックボックス調査報告書（券売機）の受領日"
        Me.Label18.Top = 0.0!
        Me.Label18.Width = 5.905512!
        '
        'PageCount
        '
        Me.PageCount.Height = 0.2!
        Me.PageCount.Left = 2.719685!
        Me.PageCount.Name = "PageCount"
        Me.PageCount.Style = "text-align: right"
        Me.PageCount.SummaryRunning = GrapeCity.ActiveReports.SectionReportModel.SummaryRunning.All
        Me.PageCount.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.PageCount
        Me.PageCount.Text = "PageCount"
        Me.PageCount.Top = 0.1968504!
        Me.PageCount.Width = 0.7397637!
        '
        'PageMax
        '
        Me.PageMax.Height = 0.2!
        Me.PageMax.Left = 3.605118!
        Me.PageMax.Name = "PageMax"
        Me.PageMax.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.PageCount
        Me.PageMax.Text = "PageMax"
        Me.PageMax.Top = 0.1968504!
        Me.PageMax.Width = 0.7397639!
        '
        'Label3
        '
        Me.Label3.Height = 0.2!
        Me.Label3.HyperLink = Nothing
        Me.Label3.Left = 3.459449!
        Me.Label3.Name = "Label3"
        Me.Label3.Style = "text-align: center"
        Me.Label3.Text = "/"
        Me.Label3.Top = 0.1968504!
        Me.Label3.Width = 0.145669!
        '
        'GroupHeader1
        '
        Me.GroupHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.lblWest, Me.Label9, Me.lblEast, Me.lblDetTTL, Me.txtYear, Me.txtMonth, Me.Line1, Me.TextBox25, Me.TextBox26, Me.TextBox33, Me.TextBox34, Me.TextBox35, Me.TextBox36, Me.TextBox37, Me.TextBox38, Me.TextBox39, Me.TextBox40, Me.TextBox41, Me.Label1, Me.Label6, Me.Label7, Me.Label8, Me.Line2, Me.Label10, Me.Label11, Me.Label12, Me.Label13, Me.Line3, Me.Label14, Me.Label16, Me.Label17, Me.Label19, Me.Line4, Me.Label20, Me.Label21, Me.Label22, Me.Label23, Me.Line5, Me.Label24, Me.Label25, Me.Label26, Me.Label27, Me.Line6, Me.Label28, Me.Label29, Me.Label30, Me.Label31, Me.Line7, Me.Label32, Me.Label33, Me.Label34, Me.Label35, Me.Line8, Me.Label36, Me.Label37, Me.Label38, Me.Label39, Me.Line9, Me.Label40, Me.Label41, Me.Label42, Me.Label43, Me.Line10, Me.Label44, Me.Label45, Me.Label46, Me.Label47, Me.Line11, Me.Label48, Me.Label49, Me.Label50, Me.Label51, Me.Line12, Me.Line13, Me.Line14, Me.Line15, Me.Line16, Me.Line17, Me.Line18, Me.Line19, Me.Line20, Me.Line21, Me.Line22, Me.Line23, Me.txtNextYear, Me.Shape2})
        Me.GroupHeader1.Height = 9.645672!
        Me.GroupHeader1.Name = "GroupHeader1"
        Me.GroupHeader1.RepeatStyle = GrapeCity.ActiveReports.SectionReportModel.RepeatStyle.All
        Me.GroupHeader1.UnderlayNext = True
        '
        'lblWest
        '
        Me.lblWest.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.lblWest.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.lblWest.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.lblWest.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.lblWest.Height = 0.1968504!
        Me.lblWest.HyperLink = Nothing
        Me.lblWest.Left = 1.062992!
        Me.lblWest.Name = "lblWest"
        Me.lblWest.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.lblWest.Text = "西日本"
        Me.lblWest.Top = 0.3937007!
        Me.lblWest.Width = 0.6692914!
        '
        'Label9
        '
        Me.Label9.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label9.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label9.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label9.Height = 0.3937007!
        Me.Label9.HyperLink = Nothing
        Me.Label9.Left = 1.062992!
        Me.Label9.Name = "Label9"
        Me.Label9.Style = "background-color: LightGrey"
        Me.Label9.Text = ""
        Me.Label9.Top = 0.0!
        Me.Label9.Width = 0.6692914!
        '
        'lblEast
        '
        Me.lblEast.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.lblEast.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.lblEast.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.lblEast.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.lblEast.Height = 0.1968504!
        Me.lblEast.HyperLink = Nothing
        Me.lblEast.Left = 1.062992!
        Me.lblEast.Name = "lblEast"
        Me.lblEast.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.lblEast.Text = "東日本"
        Me.lblEast.Top = 0.5905511!
        Me.lblEast.Width = 0.6692914!
        '
        'lblDetTTL
        '
        Me.lblDetTTL.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.lblDetTTL.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.lblDetTTL.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.lblDetTTL.Height = 0.1968504!
        Me.lblDetTTL.HyperLink = Nothing
        Me.lblDetTTL.Left = 1.062992!
        Me.lblDetTTL.Name = "lblDetTTL"
        Me.lblDetTTL.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.lblDetTTL.Text = "計"
        Me.lblDetTTL.Top = 0.7874014!
        Me.lblDetTTL.Width = 0.6692914!
        '
        'txtYear
        '
        Me.txtYear.DataField = "検収年"
        Me.txtYear.Height = 0.1968504!
        Me.txtYear.Left = 0.03937008!
        Me.txtYear.Name = "txtYear"
        Me.txtYear.Style = "text-align: center; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: n" & _
    "owrap"
        Me.txtYear.Text = "平成25年"
        Me.txtYear.Top = 0.3937008!
        Me.txtYear.Width = 0.6299213!
        '
        'txtMonth
        '
        Me.txtMonth.Height = 0.1968504!
        Me.txtMonth.Left = 0.6692914!
        Me.txtMonth.Name = "txtMonth"
        Me.txtMonth.Style = "text-align: center; vertical-align: middle"
        Me.txtMonth.Text = "04月"
        Me.txtMonth.Top = 0.3937007!
        Me.txtMonth.Width = 0.3149607!
        '
        'Line1
        '
        Me.Line1.Height = 0.3937007!
        Me.Line1.Left = 1.062992!
        Me.Line1.LineWeight = 1.0!
        Me.Line1.Name = "Line1"
        Me.Line1.Top = 0.0!
        Me.Line1.Width = 0.669291!
        Me.Line1.X1 = 1.062992!
        Me.Line1.X2 = 1.732283!
        Me.Line1.Y1 = 0.0!
        Me.Line1.Y2 = 0.3937007!
        '
        'TextBox25
        '
        Me.TextBox25.Height = 0.1968504!
        Me.TextBox25.Left = 0.6692914!
        Me.TextBox25.Name = "TextBox25"
        Me.TextBox25.Style = "text-align: center; vertical-align: middle"
        Me.TextBox25.Text = "05月"
        Me.TextBox25.Top = 1.279528!
        Me.TextBox25.Width = 0.3149607!
        '
        'TextBox26
        '
        Me.TextBox26.Height = 0.1968504!
        Me.TextBox26.Left = 0.6692914!
        Me.TextBox26.Name = "TextBox26"
        Me.TextBox26.Style = "text-align: center; vertical-align: middle"
        Me.TextBox26.Text = "06月"
        Me.TextBox26.Top = 2.066929!
        Me.TextBox26.Width = 0.3149607!
        '
        'TextBox33
        '
        Me.TextBox33.Height = 0.1968504!
        Me.TextBox33.Left = 0.6692914!
        Me.TextBox33.Name = "TextBox33"
        Me.TextBox33.Style = "text-align: center; vertical-align: middle"
        Me.TextBox33.Text = "07月"
        Me.TextBox33.Top = 2.854331!
        Me.TextBox33.Width = 0.3149607!
        '
        'TextBox34
        '
        Me.TextBox34.Height = 0.1968504!
        Me.TextBox34.Left = 0.6692914!
        Me.TextBox34.Name = "TextBox34"
        Me.TextBox34.Style = "text-align: center; vertical-align: middle"
        Me.TextBox34.Text = "08月"
        Me.TextBox34.Top = 3.641732!
        Me.TextBox34.Width = 0.3149607!
        '
        'TextBox35
        '
        Me.TextBox35.Height = 0.1968504!
        Me.TextBox35.Left = 0.6692914!
        Me.TextBox35.Name = "TextBox35"
        Me.TextBox35.Style = "text-align: center; vertical-align: middle"
        Me.TextBox35.Text = "09月"
        Me.TextBox35.Top = 4.429134!
        Me.TextBox35.Width = 0.3149607!
        '
        'TextBox36
        '
        Me.TextBox36.Height = 0.1968504!
        Me.TextBox36.Left = 0.6692914!
        Me.TextBox36.Name = "TextBox36"
        Me.TextBox36.Style = "text-align: center; vertical-align: middle"
        Me.TextBox36.Text = "10月"
        Me.TextBox36.Top = 5.216536!
        Me.TextBox36.Width = 0.3149607!
        '
        'TextBox37
        '
        Me.TextBox37.Height = 0.1968504!
        Me.TextBox37.Left = 0.6692914!
        Me.TextBox37.Name = "TextBox37"
        Me.TextBox37.Style = "text-align: center; vertical-align: middle"
        Me.TextBox37.Text = "11月"
        Me.TextBox37.Top = 6.003937!
        Me.TextBox37.Width = 0.3149607!
        '
        'TextBox38
        '
        Me.TextBox38.Height = 0.1968504!
        Me.TextBox38.Left = 0.6692914!
        Me.TextBox38.Name = "TextBox38"
        Me.TextBox38.Style = "text-align: center; vertical-align: middle"
        Me.TextBox38.Text = "12月"
        Me.TextBox38.Top = 6.791339!
        Me.TextBox38.Width = 0.3149607!
        '
        'TextBox39
        '
        Me.TextBox39.Height = 0.1968504!
        Me.TextBox39.Left = 0.6692914!
        Me.TextBox39.Name = "TextBox39"
        Me.TextBox39.Style = "text-align: center; vertical-align: middle"
        Me.TextBox39.Text = "01月"
        Me.TextBox39.Top = 7.578743!
        Me.TextBox39.Width = 0.3149607!
        '
        'TextBox40
        '
        Me.TextBox40.Height = 0.1968504!
        Me.TextBox40.Left = 0.6692914!
        Me.TextBox40.Name = "TextBox40"
        Me.TextBox40.Style = "text-align: center; vertical-align: middle"
        Me.TextBox40.Text = "02月"
        Me.TextBox40.Top = 8.366146!
        Me.TextBox40.Width = 0.3149607!
        '
        'TextBox41
        '
        Me.TextBox41.Height = 0.1968504!
        Me.TextBox41.Left = 0.6692914!
        Me.TextBox41.Name = "TextBox41"
        Me.TextBox41.Style = "text-align: center; vertical-align: middle"
        Me.TextBox41.Text = "03月"
        Me.TextBox41.Top = 9.153542!
        Me.TextBox41.Width = 0.3149607!
        '
        'Label1
        '
        Me.Label1.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label1.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label1.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label1.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label1.Height = 0.1968504!
        Me.Label1.HyperLink = Nothing
        Me.Label1.Left = 1.062992!
        Me.Label1.Name = "Label1"
        Me.Label1.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label1.Text = "西日本"
        Me.Label1.Top = 1.181102!
        Me.Label1.Width = 0.6692914!
        '
        'Label6
        '
        Me.Label6.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label6.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label6.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label6.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label6.Height = 0.1968504!
        Me.Label6.HyperLink = Nothing
        Me.Label6.Left = 1.062992!
        Me.Label6.Name = "Label6"
        Me.Label6.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label6.Text = "東日本"
        Me.Label6.Top = 1.377953!
        Me.Label6.Width = 0.6692914!
        '
        'Label7
        '
        Me.Label7.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label7.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label7.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label7.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label7.Height = 0.1968504!
        Me.Label7.HyperLink = Nothing
        Me.Label7.Left = 1.062992!
        Me.Label7.Name = "Label7"
        Me.Label7.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label7.Text = "計"
        Me.Label7.Top = 1.574803!
        Me.Label7.Width = 0.6692914!
        '
        'Label8
        '
        Me.Label8.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label8.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label8.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label8.Height = 0.1968504!
        Me.Label8.HyperLink = Nothing
        Me.Label8.Left = 1.062992!
        Me.Label8.Name = "Label8"
        Me.Label8.Style = "background-color: LightGrey"
        Me.Label8.Text = ""
        Me.Label8.Top = 0.9842521!
        Me.Label8.Width = 0.6692914!
        '
        'Line2
        '
        Me.Line2.Height = 0.1968497!
        Me.Line2.Left = 1.062992!
        Me.Line2.LineWeight = 1.0!
        Me.Line2.Name = "Line2"
        Me.Line2.Top = 0.9803153!
        Me.Line2.Width = 0.669291!
        Me.Line2.X1 = 1.062992!
        Me.Line2.X2 = 1.732283!
        Me.Line2.Y1 = 0.9803153!
        Me.Line2.Y2 = 1.177165!
        '
        'Label10
        '
        Me.Label10.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label10.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label10.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label10.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label10.Height = 0.1968504!
        Me.Label10.HyperLink = Nothing
        Me.Label10.Left = 1.062992!
        Me.Label10.Name = "Label10"
        Me.Label10.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label10.Text = "西日本"
        Me.Label10.Top = 1.968504!
        Me.Label10.Width = 0.6692914!
        '
        'Label11
        '
        Me.Label11.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label11.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label11.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label11.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label11.Height = 0.1968504!
        Me.Label11.HyperLink = Nothing
        Me.Label11.Left = 1.062992!
        Me.Label11.Name = "Label11"
        Me.Label11.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label11.Text = "東日本"
        Me.Label11.Top = 2.165354!
        Me.Label11.Width = 0.6692914!
        '
        'Label12
        '
        Me.Label12.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label12.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label12.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label12.Height = 0.1968504!
        Me.Label12.HyperLink = Nothing
        Me.Label12.Left = 1.062992!
        Me.Label12.Name = "Label12"
        Me.Label12.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label12.Text = "計"
        Me.Label12.Top = 2.362205!
        Me.Label12.Width = 0.6692914!
        '
        'Label13
        '
        Me.Label13.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label13.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label13.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label13.Height = 0.1968504!
        Me.Label13.HyperLink = Nothing
        Me.Label13.Left = 1.062992!
        Me.Label13.Name = "Label13"
        Me.Label13.Style = "background-color: LightGrey"
        Me.Label13.Text = ""
        Me.Label13.Top = 1.771654!
        Me.Label13.Width = 0.6692914!
        '
        'Line3
        '
        Me.Line3.Height = 0.1968499!
        Me.Line3.Left = 1.062992!
        Me.Line3.LineWeight = 1.0!
        Me.Line3.Name = "Line3"
        Me.Line3.Top = 1.771654!
        Me.Line3.Width = 0.669291!
        Me.Line3.X1 = 1.062992!
        Me.Line3.X2 = 1.732283!
        Me.Line3.Y1 = 1.771654!
        Me.Line3.Y2 = 1.968504!
        '
        'Label14
        '
        Me.Label14.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label14.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label14.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label14.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label14.Height = 0.1968504!
        Me.Label14.HyperLink = Nothing
        Me.Label14.Left = 1.062992!
        Me.Label14.Name = "Label14"
        Me.Label14.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label14.Text = "西日本"
        Me.Label14.Top = 2.755906!
        Me.Label14.Width = 0.6692914!
        '
        'Label16
        '
        Me.Label16.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label16.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label16.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label16.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label16.Height = 0.1968504!
        Me.Label16.HyperLink = Nothing
        Me.Label16.Left = 1.062992!
        Me.Label16.Name = "Label16"
        Me.Label16.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label16.Text = "東日本"
        Me.Label16.Top = 2.952756!
        Me.Label16.Width = 0.6692914!
        '
        'Label17
        '
        Me.Label17.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label17.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label17.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label17.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label17.Height = 0.1968504!
        Me.Label17.HyperLink = Nothing
        Me.Label17.Left = 1.062992!
        Me.Label17.Name = "Label17"
        Me.Label17.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label17.Text = "計"
        Me.Label17.Top = 3.149606!
        Me.Label17.Width = 0.6692914!
        '
        'Label19
        '
        Me.Label19.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label19.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label19.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label19.Height = 0.1968504!
        Me.Label19.HyperLink = Nothing
        Me.Label19.Left = 1.062992!
        Me.Label19.Name = "Label19"
        Me.Label19.Style = "background-color: LightGrey"
        Me.Label19.Text = ""
        Me.Label19.Top = 2.559055!
        Me.Label19.Width = 0.6692914!
        '
        'Line4
        '
        Me.Line4.Height = 0.196851!
        Me.Line4.Left = 1.062992!
        Me.Line4.LineWeight = 1.0!
        Me.Line4.Name = "Line4"
        Me.Line4.Top = 2.559055!
        Me.Line4.Width = 0.669291!
        Me.Line4.X1 = 1.062992!
        Me.Line4.X2 = 1.732283!
        Me.Line4.Y1 = 2.559055!
        Me.Line4.Y2 = 2.755906!
        '
        'Label20
        '
        Me.Label20.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label20.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label20.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label20.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label20.Height = 0.1968504!
        Me.Label20.HyperLink = Nothing
        Me.Label20.Left = 1.062992!
        Me.Label20.Name = "Label20"
        Me.Label20.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label20.Text = "西日本"
        Me.Label20.Top = 3.543307!
        Me.Label20.Width = 0.6692914!
        '
        'Label21
        '
        Me.Label21.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label21.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label21.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label21.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label21.Height = 0.1968504!
        Me.Label21.HyperLink = Nothing
        Me.Label21.Left = 1.062992!
        Me.Label21.Name = "Label21"
        Me.Label21.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label21.Text = "東日本"
        Me.Label21.Top = 3.740158!
        Me.Label21.Width = 0.6692914!
        '
        'Label22
        '
        Me.Label22.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label22.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label22.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label22.Height = 0.1968504!
        Me.Label22.HyperLink = Nothing
        Me.Label22.Left = 1.062992!
        Me.Label22.Name = "Label22"
        Me.Label22.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label22.Text = "計"
        Me.Label22.Top = 3.937008!
        Me.Label22.Width = 0.6692914!
        '
        'Label23
        '
        Me.Label23.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label23.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label23.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label23.Height = 0.1968504!
        Me.Label23.HyperLink = Nothing
        Me.Label23.Left = 1.062992!
        Me.Label23.Name = "Label23"
        Me.Label23.Style = "background-color: LightGrey"
        Me.Label23.Text = ""
        Me.Label23.Top = 3.346457!
        Me.Label23.Width = 0.6692914!
        '
        'Line5
        '
        Me.Line5.Height = 0.196851!
        Me.Line5.Left = 1.062992!
        Me.Line5.LineWeight = 1.0!
        Me.Line5.Name = "Line5"
        Me.Line5.Top = 3.346457!
        Me.Line5.Width = 0.669291!
        Me.Line5.X1 = 1.062992!
        Me.Line5.X2 = 1.732283!
        Me.Line5.Y1 = 3.346457!
        Me.Line5.Y2 = 3.543308!
        '
        'Label24
        '
        Me.Label24.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label24.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label24.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label24.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label24.Height = 0.1968504!
        Me.Label24.HyperLink = Nothing
        Me.Label24.Left = 1.062992!
        Me.Label24.Name = "Label24"
        Me.Label24.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label24.Text = "西日本"
        Me.Label24.Top = 4.330709!
        Me.Label24.Width = 0.6692914!
        '
        'Label25
        '
        Me.Label25.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label25.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label25.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label25.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label25.Height = 0.1968504!
        Me.Label25.HyperLink = Nothing
        Me.Label25.Left = 1.062992!
        Me.Label25.Name = "Label25"
        Me.Label25.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label25.Text = "東日本"
        Me.Label25.Top = 4.527559!
        Me.Label25.Width = 0.6692914!
        '
        'Label26
        '
        Me.Label26.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label26.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label26.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label26.Height = 0.1968504!
        Me.Label26.HyperLink = Nothing
        Me.Label26.Left = 1.062992!
        Me.Label26.Name = "Label26"
        Me.Label26.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label26.Text = "計"
        Me.Label26.Top = 4.72441!
        Me.Label26.Width = 0.6692914!
        '
        'Label27
        '
        Me.Label27.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label27.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label27.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label27.Height = 0.1968504!
        Me.Label27.HyperLink = Nothing
        Me.Label27.Left = 1.062992!
        Me.Label27.Name = "Label27"
        Me.Label27.Style = "background-color: LightGrey"
        Me.Label27.Text = ""
        Me.Label27.Top = 4.133859!
        Me.Label27.Width = 0.6692914!
        '
        'Line6
        '
        Me.Line6.Height = 0.1968498!
        Me.Line6.Left = 1.062992!
        Me.Line6.LineWeight = 1.0!
        Me.Line6.Name = "Line6"
        Me.Line6.Top = 4.133859!
        Me.Line6.Width = 0.669291!
        Me.Line6.X1 = 1.062992!
        Me.Line6.X2 = 1.732283!
        Me.Line6.Y1 = 4.133859!
        Me.Line6.Y2 = 4.330709!
        '
        'Label28
        '
        Me.Label28.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label28.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label28.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label28.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label28.Height = 0.1968504!
        Me.Label28.HyperLink = Nothing
        Me.Label28.Left = 1.062992!
        Me.Label28.Name = "Label28"
        Me.Label28.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label28.Text = "西日本"
        Me.Label28.Top = 6.692914!
        Me.Label28.Width = 0.6692914!
        '
        'Label29
        '
        Me.Label29.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label29.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label29.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label29.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label29.Height = 0.1968504!
        Me.Label29.HyperLink = Nothing
        Me.Label29.Left = 1.062992!
        Me.Label29.Name = "Label29"
        Me.Label29.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label29.Text = "東日本"
        Me.Label29.Top = 6.889764!
        Me.Label29.Width = 0.6692914!
        '
        'Label30
        '
        Me.Label30.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label30.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label30.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label30.Height = 0.1968504!
        Me.Label30.HyperLink = Nothing
        Me.Label30.Left = 1.062992!
        Me.Label30.Name = "Label30"
        Me.Label30.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label30.Text = "計"
        Me.Label30.Top = 7.086618!
        Me.Label30.Width = 0.6692914!
        '
        'Label31
        '
        Me.Label31.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label31.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label31.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label31.Height = 0.1968504!
        Me.Label31.HyperLink = Nothing
        Me.Label31.Left = 1.062992!
        Me.Label31.Name = "Label31"
        Me.Label31.Style = "background-color: LightGrey"
        Me.Label31.Text = ""
        Me.Label31.Top = 6.496063!
        Me.Label31.Width = 0.6692914!
        '
        'Line7
        '
        Me.Line7.Height = 0.1968498!
        Me.Line7.Left = 1.062992!
        Me.Line7.LineWeight = 1.0!
        Me.Line7.Name = "Line7"
        Me.Line7.Top = 6.496063!
        Me.Line7.Width = 0.669291!
        Me.Line7.X1 = 1.062992!
        Me.Line7.X2 = 1.732283!
        Me.Line7.Y1 = 6.496063!
        Me.Line7.Y2 = 6.692913!
        '
        'Label32
        '
        Me.Label32.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label32.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label32.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label32.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label32.Height = 0.1968504!
        Me.Label32.HyperLink = Nothing
        Me.Label32.Left = 1.062992!
        Me.Label32.Name = "Label32"
        Me.Label32.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label32.Text = "西日本"
        Me.Label32.Top = 7.480319!
        Me.Label32.Width = 0.6692914!
        '
        'Label33
        '
        Me.Label33.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label33.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label33.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label33.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label33.Height = 0.1968504!
        Me.Label33.HyperLink = Nothing
        Me.Label33.Left = 1.062992!
        Me.Label33.Name = "Label33"
        Me.Label33.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label33.Text = "東日本"
        Me.Label33.Top = 7.677167!
        Me.Label33.Width = 0.6692914!
        '
        'Label34
        '
        Me.Label34.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label34.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label34.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label34.Height = 0.1968504!
        Me.Label34.HyperLink = Nothing
        Me.Label34.Left = 1.062992!
        Me.Label34.Name = "Label34"
        Me.Label34.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label34.Text = "計"
        Me.Label34.Top = 7.874014!
        Me.Label34.Width = 0.6692914!
        '
        'Label35
        '
        Me.Label35.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label35.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label35.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label35.Height = 0.1968504!
        Me.Label35.HyperLink = Nothing
        Me.Label35.Left = 1.062992!
        Me.Label35.Name = "Label35"
        Me.Label35.Style = "background-color: LightGrey"
        Me.Label35.Text = ""
        Me.Label35.Top = 7.283465!
        Me.Label35.Width = 0.6692914!
        '
        'Line8
        '
        Me.Line8.Height = 0.1968541!
        Me.Line8.Left = 1.062992!
        Me.Line8.LineWeight = 1.0!
        Me.Line8.Name = "Line8"
        Me.Line8.Top = 7.283465!
        Me.Line8.Width = 0.669291!
        Me.Line8.X1 = 1.062992!
        Me.Line8.X2 = 1.732283!
        Me.Line8.Y1 = 7.283465!
        Me.Line8.Y2 = 7.480319!
        '
        'Label36
        '
        Me.Label36.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label36.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label36.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label36.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label36.Height = 0.1968504!
        Me.Label36.HyperLink = Nothing
        Me.Label36.Left = 1.062992!
        Me.Label36.Name = "Label36"
        Me.Label36.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label36.Text = "西日本"
        Me.Label36.Top = 8.267715!
        Me.Label36.Width = 0.6692914!
        '
        'Label37
        '
        Me.Label37.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label37.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label37.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label37.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label37.Height = 0.1968504!
        Me.Label37.HyperLink = Nothing
        Me.Label37.Left = 1.062992!
        Me.Label37.Name = "Label37"
        Me.Label37.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label37.Text = "東日本"
        Me.Label37.Top = 8.46457!
        Me.Label37.Width = 0.6692914!
        '
        'Label38
        '
        Me.Label38.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label38.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label38.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label38.Height = 0.1968504!
        Me.Label38.HyperLink = Nothing
        Me.Label38.Left = 1.062992!
        Me.Label38.Name = "Label38"
        Me.Label38.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label38.Text = "計"
        Me.Label38.Top = 8.661417!
        Me.Label38.Width = 0.6692914!
        '
        'Label39
        '
        Me.Label39.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label39.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label39.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label39.Height = 0.1968504!
        Me.Label39.HyperLink = Nothing
        Me.Label39.Left = 1.062992!
        Me.Label39.Name = "Label39"
        Me.Label39.Style = "background-color: LightGrey"
        Me.Label39.Text = ""
        Me.Label39.Top = 8.070868!
        Me.Label39.Width = 0.6692914!
        '
        'Line9
        '
        Me.Line9.Height = 0.1968479!
        Me.Line9.Left = 1.062992!
        Me.Line9.LineWeight = 1.0!
        Me.Line9.Name = "Line9"
        Me.Line9.Top = 8.070868!
        Me.Line9.Width = 0.669291!
        Me.Line9.X1 = 1.062992!
        Me.Line9.X2 = 1.732283!
        Me.Line9.Y1 = 8.070868!
        Me.Line9.Y2 = 8.267715!
        '
        'Label40
        '
        Me.Label40.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label40.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label40.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label40.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label40.Height = 0.1968504!
        Me.Label40.HyperLink = Nothing
        Me.Label40.Left = 1.062992!
        Me.Label40.Name = "Label40"
        Me.Label40.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label40.Text = "西日本"
        Me.Label40.Top = 5.118111!
        Me.Label40.Width = 0.6692914!
        '
        'Label41
        '
        Me.Label41.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label41.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label41.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label41.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label41.Height = 0.1968504!
        Me.Label41.HyperLink = Nothing
        Me.Label41.Left = 1.062992!
        Me.Label41.Name = "Label41"
        Me.Label41.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label41.Text = "東日本"
        Me.Label41.Top = 5.314961!
        Me.Label41.Width = 0.6692914!
        '
        'Label42
        '
        Me.Label42.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label42.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label42.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label42.Height = 0.1968504!
        Me.Label42.HyperLink = Nothing
        Me.Label42.Left = 1.062992!
        Me.Label42.Name = "Label42"
        Me.Label42.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label42.Text = "計"
        Me.Label42.Top = 5.51181!
        Me.Label42.Width = 0.6692914!
        '
        'Label43
        '
        Me.Label43.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label43.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label43.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label43.Height = 0.1968504!
        Me.Label43.HyperLink = Nothing
        Me.Label43.Left = 1.062992!
        Me.Label43.Name = "Label43"
        Me.Label43.Style = "background-color: LightGrey"
        Me.Label43.Text = ""
        Me.Label43.Top = 4.921259!
        Me.Label43.Width = 0.6692914!
        '
        'Line10
        '
        Me.Line10.Height = 0.1968508!
        Me.Line10.Left = 1.062992!
        Me.Line10.LineWeight = 1.0!
        Me.Line10.Name = "Line10"
        Me.Line10.Top = 4.917324!
        Me.Line10.Width = 0.669291!
        Me.Line10.X1 = 1.062992!
        Me.Line10.X2 = 1.732283!
        Me.Line10.Y1 = 4.917324!
        Me.Line10.Y2 = 5.114175!
        '
        'Label44
        '
        Me.Label44.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label44.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label44.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label44.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label44.Height = 0.1968504!
        Me.Label44.HyperLink = Nothing
        Me.Label44.Left = 1.062992!
        Me.Label44.Name = "Label44"
        Me.Label44.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label44.Text = "西日本"
        Me.Label44.Top = 5.905513!
        Me.Label44.Width = 0.6692914!
        '
        'Label45
        '
        Me.Label45.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label45.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label45.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label45.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label45.Height = 0.1968504!
        Me.Label45.HyperLink = Nothing
        Me.Label45.Left = 1.062992!
        Me.Label45.Name = "Label45"
        Me.Label45.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label45.Text = "東日本"
        Me.Label45.Top = 6.102364!
        Me.Label45.Width = 0.6692914!
        '
        'Label46
        '
        Me.Label46.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label46.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label46.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label46.Height = 0.1968504!
        Me.Label46.HyperLink = Nothing
        Me.Label46.Left = 1.062992!
        Me.Label46.Name = "Label46"
        Me.Label46.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label46.Text = "計"
        Me.Label46.Top = 6.299212!
        Me.Label46.Width = 0.6692914!
        '
        'Label47
        '
        Me.Label47.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label47.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label47.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label47.Height = 0.1968504!
        Me.Label47.HyperLink = Nothing
        Me.Label47.Left = 1.062992!
        Me.Label47.Name = "Label47"
        Me.Label47.Style = "background-color: LightGrey"
        Me.Label47.Text = ""
        Me.Label47.Top = 5.708662!
        Me.Label47.Width = 0.6692914!
        '
        'Line11
        '
        Me.Line11.Height = 0.1968498!
        Me.Line11.Left = 1.062992!
        Me.Line11.LineWeight = 1.0!
        Me.Line11.Name = "Line11"
        Me.Line11.Top = 5.708663!
        Me.Line11.Width = 0.669291!
        Me.Line11.X1 = 1.062992!
        Me.Line11.X2 = 1.732283!
        Me.Line11.Y1 = 5.708663!
        Me.Line11.Y2 = 5.905513!
        '
        'Label48
        '
        Me.Label48.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label48.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label48.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label48.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label48.Height = 0.1968504!
        Me.Label48.HyperLink = Nothing
        Me.Label48.Left = 1.062992!
        Me.Label48.Name = "Label48"
        Me.Label48.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label48.Text = "西日本"
        Me.Label48.Top = 9.055119!
        Me.Label48.Width = 0.6692914!
        '
        'Label49
        '
        Me.Label49.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label49.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label49.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label49.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label49.Height = 0.1968504!
        Me.Label49.HyperLink = Nothing
        Me.Label49.Left = 1.062992!
        Me.Label49.Name = "Label49"
        Me.Label49.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label49.Text = "東日本"
        Me.Label49.Top = 9.251972!
        Me.Label49.Width = 0.6692914!
        '
        'Label50
        '
        Me.Label50.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label50.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label50.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label50.Height = 0.1968504!
        Me.Label50.HyperLink = Nothing
        Me.Label50.Left = 1.062992!
        Me.Label50.Name = "Label50"
        Me.Label50.Style = "background-color: LightGrey; text-align: center; vertical-align: middle"
        Me.Label50.Text = "計"
        Me.Label50.Top = 9.448819!
        Me.Label50.Width = 0.6692914!
        '
        'Label51
        '
        Me.Label51.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label51.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label51.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.Label51.Height = 0.1968504!
        Me.Label51.HyperLink = Nothing
        Me.Label51.Left = 1.062992!
        Me.Label51.Name = "Label51"
        Me.Label51.Style = "background-color: LightGrey"
        Me.Label51.Text = ""
        Me.Label51.Top = 8.858271!
        Me.Label51.Width = 0.6692914!
        '
        'Line12
        '
        Me.Line12.Height = 0.1968546!
        Me.Line12.Left = 1.062992!
        Me.Line12.LineWeight = 1.0!
        Me.Line12.Name = "Line12"
        Me.Line12.Top = 8.854334!
        Me.Line12.Width = 0.669291!
        Me.Line12.X1 = 1.062992!
        Me.Line12.X2 = 1.732283!
        Me.Line12.Y1 = 8.854334!
        Me.Line12.Y2 = 9.051188!
        '
        'Line13
        '
        Me.Line13.Height = 0.0!
        Me.Line13.Left = 0.0!
        Me.Line13.LineWeight = 2.0!
        Me.Line13.Name = "Line13"
        Me.Line13.Top = 0.984252!
        Me.Line13.Width = 7.086615!
        Me.Line13.X1 = 0.0!
        Me.Line13.X2 = 7.086615!
        Me.Line13.Y1 = 0.984252!
        Me.Line13.Y2 = 0.984252!
        '
        'Line14
        '
        Me.Line14.Height = 0.0!
        Me.Line14.Left = 0.0!
        Me.Line14.LineWeight = 2.0!
        Me.Line14.Name = "Line14"
        Me.Line14.Top = 1.771654!
        Me.Line14.Width = 7.086611!
        Me.Line14.X1 = 0.0!
        Me.Line14.X2 = 7.086611!
        Me.Line14.Y1 = 1.771654!
        Me.Line14.Y2 = 1.771654!
        '
        'Line15
        '
        Me.Line15.Height = 0.0!
        Me.Line15.Left = 0.0!
        Me.Line15.LineWeight = 2.0!
        Me.Line15.Name = "Line15"
        Me.Line15.Top = 2.559055!
        Me.Line15.Width = 7.086611!
        Me.Line15.X1 = 0.0!
        Me.Line15.X2 = 7.086611!
        Me.Line15.Y1 = 2.559055!
        Me.Line15.Y2 = 2.559055!
        '
        'Line16
        '
        Me.Line16.Height = 0.0!
        Me.Line16.Left = 0.0!
        Me.Line16.LineWeight = 2.0!
        Me.Line16.Name = "Line16"
        Me.Line16.Top = 3.346457!
        Me.Line16.Width = 7.086611!
        Me.Line16.X1 = 0.0!
        Me.Line16.X2 = 7.086611!
        Me.Line16.Y1 = 3.346457!
        Me.Line16.Y2 = 3.346457!
        '
        'Line17
        '
        Me.Line17.Height = 0.0!
        Me.Line17.Left = 0.0!
        Me.Line17.LineWeight = 2.0!
        Me.Line17.Name = "Line17"
        Me.Line17.Top = 4.133859!
        Me.Line17.Width = 7.086611!
        Me.Line17.X1 = 0.0!
        Me.Line17.X2 = 7.086611!
        Me.Line17.Y1 = 4.133859!
        Me.Line17.Y2 = 4.133859!
        '
        'Line18
        '
        Me.Line18.Height = 0.0!
        Me.Line18.Left = 0.0!
        Me.Line18.LineWeight = 2.0!
        Me.Line18.Name = "Line18"
        Me.Line18.Top = 4.92126!
        Me.Line18.Width = 7.086611!
        Me.Line18.X1 = 0.0!
        Me.Line18.X2 = 7.086611!
        Me.Line18.Y1 = 4.92126!
        Me.Line18.Y2 = 4.92126!
        '
        'Line19
        '
        Me.Line19.Height = 0.0!
        Me.Line19.Left = 0.0!
        Me.Line19.LineWeight = 2.0!
        Me.Line19.Name = "Line19"
        Me.Line19.Top = 5.708662!
        Me.Line19.Width = 7.086611!
        Me.Line19.X1 = 0.0!
        Me.Line19.X2 = 7.086611!
        Me.Line19.Y1 = 5.708662!
        Me.Line19.Y2 = 5.708662!
        '
        'Line20
        '
        Me.Line20.Height = 0.0!
        Me.Line20.Left = 0.0!
        Me.Line20.LineWeight = 2.0!
        Me.Line20.Name = "Line20"
        Me.Line20.Top = 6.496063!
        Me.Line20.Width = 7.086611!
        Me.Line20.X1 = 0.0!
        Me.Line20.X2 = 7.086611!
        Me.Line20.Y1 = 6.496063!
        Me.Line20.Y2 = 6.496063!
        '
        'Line21
        '
        Me.Line21.Height = 0.0!
        Me.Line21.Left = 0.0!
        Me.Line21.LineWeight = 2.0!
        Me.Line21.Name = "Line21"
        Me.Line21.Top = 7.283465!
        Me.Line21.Width = 7.086611!
        Me.Line21.X1 = 0.0!
        Me.Line21.X2 = 7.086611!
        Me.Line21.Y1 = 7.283465!
        Me.Line21.Y2 = 7.283465!
        '
        'Line22
        '
        Me.Line22.Height = 0.0!
        Me.Line22.Left = 0.0!
        Me.Line22.LineWeight = 2.0!
        Me.Line22.Name = "Line22"
        Me.Line22.Top = 8.070867!
        Me.Line22.Width = 7.086611!
        Me.Line22.X1 = 0.0!
        Me.Line22.X2 = 7.086611!
        Me.Line22.Y1 = 8.070867!
        Me.Line22.Y2 = 8.070867!
        '
        'Line23
        '
        Me.Line23.Height = 0.0!
        Me.Line23.Left = 0.0!
        Me.Line23.LineWeight = 2.0!
        Me.Line23.Name = "Line23"
        Me.Line23.Top = 8.858269!
        Me.Line23.Width = 7.086611!
        Me.Line23.X1 = 0.0!
        Me.Line23.X2 = 7.086611!
        Me.Line23.Y1 = 8.858269!
        Me.Line23.Y2 = 8.858269!
        '
        'txtNextYear
        '
        Me.txtNextYear.Height = 0.1968504!
        Me.txtNextYear.Left = 0.03937006!
        Me.txtNextYear.Name = "txtNextYear"
        Me.txtNextYear.Style = "text-align: center; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: n" & _
    "owrap"
        Me.txtNextYear.Text = "平成26年"
        Me.txtNextYear.Top = 7.578741!
        Me.txtNextYear.Width = 0.6299213!
        '
        'Shape2
        '
        Me.Shape2.Height = 9.645669!
        Me.Shape2.Left = 0.0!
        Me.Shape2.LineWeight = 2.0!
        Me.Shape2.Name = "Shape2"
        Me.Shape2.RoundingRadius = New GrapeCity.ActiveReports.Controls.CornersRadius(9.999999!)
        Me.Shape2.Top = 0.0!
        Me.Shape2.Width = 7.102363!
        '
        'GroupFooter1
        '
        Me.GroupFooter1.Height = 0.0!
        Me.GroupFooter1.Name = "GroupFooter1"
        '
        'BBPREP003
        '
        Me.MasterReport = False
        Me.PageSettings.DefaultPaperSize = False
        Me.PageSettings.Margins.Bottom = 0.3937008!
        Me.PageSettings.Margins.Left = 0.3937008!
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
        CType(Me.ReportInfo1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtMyCompNM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtMyPostNM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label18, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PageCount, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PageMax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblWest, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblEast, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblDetTTL, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtYear, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtMonth, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox25, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox26, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox33, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox34, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox35, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox36, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox37, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox38, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox39, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox40, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox41, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label12, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label13, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label14, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label16, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label17, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label19, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label20, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label21, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label22, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label23, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label24, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label25, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label26, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label27, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label28, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label29, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label30, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label31, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label32, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label33, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label34, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label35, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label36, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label37, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label38, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label39, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label40, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label41, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label42, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label43, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label44, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label45, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label46, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label47, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label48, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label49, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label50, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label51, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtNextYear, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Private WithEvents ReportInfo1 As GrapeCity.ActiveReports.SectionReportModel.ReportInfo
    Private WithEvents txtMyCompNM As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtMyPostNM As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label2 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label18 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents PageCount As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents PageMax As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label3 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents BBPREP003_2 As GrapeCity.ActiveReports.SectionReportModel.SubReport
    Private WithEvents GroupHeader1 As GrapeCity.ActiveReports.SectionReportModel.GroupHeader
    Private WithEvents lblEast As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblDetTTL As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtYear As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtMonth As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label9 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Line1 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents TextBox25 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox26 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox33 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox34 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox35 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox36 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox37 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox38 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox39 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox40 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox41 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label1 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label6 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label7 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label8 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Line2 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Label10 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label11 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label12 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label13 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Line3 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Label14 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label16 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label17 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label19 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Line4 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Label20 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label21 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label22 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label23 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Line5 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Label24 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label25 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label26 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label27 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Line6 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Label28 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label29 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label30 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label31 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Line7 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Label32 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label33 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label34 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label35 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Line8 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Label36 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label37 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label38 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label39 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Line9 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Label40 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label41 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label42 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label43 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Line10 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Label44 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label45 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label46 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label47 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Line11 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Label48 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label49 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label50 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label51 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Line12 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents lblWest As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents GroupFooter1 As GrapeCity.ActiveReports.SectionReportModel.GroupFooter
    Private WithEvents Shape2 As GrapeCity.ActiveReports.SectionReportModel.Shape
    Private WithEvents Line13 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line14 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line15 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line16 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line17 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line18 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line19 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line20 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line21 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line22 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line23 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents txtNextYear As GrapeCity.ActiveReports.SectionReportModel.TextBox
End Class
