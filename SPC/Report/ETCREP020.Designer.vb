<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ETCREP020
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
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ETCREP020))
        Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
        Me.TxtTITLE = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtDATE = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtCOMP_NM = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label1 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label2 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TxtGYM = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail()
        Me.LblColor = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TxtCNT_NM = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtCNT1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtCNT2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtCNT3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtCNT4 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtCNT5 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtCNT6 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtCNT7 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Line15 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line16 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line17 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line18 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line19 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line20 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line21 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line22 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line23 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line24 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line25 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line26 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
        Me.GroupHeader1 = New GrapeCity.ActiveReports.SectionReportModel.GroupHeader()
        Me.TextBox5 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox6 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox7 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtITEM_TITLE1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtITEM_TITLE2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtITEM_TITLE3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtITEM_TITLE4 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtITEM_TITLE5 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtITEM_TITLE6 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtITEM_TITLE7 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Line1 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line2 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line3 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line4 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line5 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line6 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line7 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line8 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line9 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line10 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line11 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line12 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line13 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.GroupFooter1 = New GrapeCity.ActiveReports.SectionReportModel.GroupFooter()
        Me.GroupHeader2 = New GrapeCity.ActiveReports.SectionReportModel.GroupHeader()
        Me.TxtCLASS = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtTARGET = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Line14 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.GroupFooter2 = New GrapeCity.ActiveReports.SectionReportModel.GroupFooter()
        Me.Line27 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        CType(Me.TxtTITLE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtDATE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtCOMP_NM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtGYM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LblColor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtCNT_NM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtCNT1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtCNT2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtCNT3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtCNT4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtCNT5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtCNT6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtCNT7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtITEM_TITLE1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtITEM_TITLE2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtITEM_TITLE3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtITEM_TITLE4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtITEM_TITLE5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtITEM_TITLE6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtITEM_TITLE7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtCLASS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtTARGET, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'PageHeader1
        '
        Me.PageHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TxtTITLE, Me.TextBox2, Me.TxtDATE, Me.TxtCOMP_NM, Me.Label1, Me.Label2, Me.TxtGYM})
        Me.PageHeader1.Height = 0.6692914!
        Me.PageHeader1.Name = "PageHeader1"
        '
        'TxtTITLE
        '
        Me.TxtTITLE.DataField = "タイトル"
        Me.TxtTITLE.Height = 0.2362205!
        Me.TxtTITLE.Left = 0.06614174!
        Me.TxtTITLE.Name = "TxtTITLE"
        Me.TxtTITLE.Style = "font-size: 15.75pt; text-align: right"
        Me.TxtTITLE.Text = "XXXXX"
        Me.TxtTITLE.Top = 0.1181102!
        Me.TxtTITLE.Width = 1.588977!
        '
        'TextBox2
        '
        Me.TextBox2.Height = 0.2362205!
        Me.TextBox2.Left = 1.655118!
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Style = "font-size: 15.75pt"
        Me.TextBox2.Text = "ホール機器品質管理表"
        Me.TextBox2.Top = 0.1181102!
        Me.TextBox2.Width = 2.132677!
        '
        'TxtDATE
        '
        Me.TxtDATE.DataField = "日付"
        Me.TxtDATE.Height = 0.2362206!
        Me.TxtDATE.Left = 5.708662!
        Me.TxtDATE.Name = "TxtDATE"
        Me.TxtDATE.OutputFormat = resources.GetString("TxtDATE.OutputFormat")
        Me.TxtDATE.Style = "text-align: right"
        Me.TxtDATE.Text = "ggyy年MM月dd日"
        Me.TxtDATE.Top = 0.1181102!
        Me.TxtDATE.Width = 1.259842!
        '
        'TxtCOMP_NM
        '
        Me.TxtCOMP_NM.DataField = "会社名"
        Me.TxtCOMP_NM.Height = 0.2!
        Me.TxtCOMP_NM.Left = 4.099607!
        Me.TxtCOMP_NM.Name = "TxtCOMP_NM"
        Me.TxtCOMP_NM.OutputFormat = resources.GetString("TxtCOMP_NM.OutputFormat")
        Me.TxtCOMP_NM.Style = "text-align: right"
        Me.TxtCOMP_NM.Text = "XXXXXXXXXXXXXXXX"
        Me.TxtCOMP_NM.Top = 0.3543307!
        Me.TxtCOMP_NM.Width = 2.868897!
        '
        'Label1
        '
        Me.Label1.Height = 0.2362205!
        Me.Label1.HyperLink = Nothing
        Me.Label1.Left = 3.787796!
        Me.Label1.Name = "Label1"
        Me.Label1.Style = "font-size: 15pt; text-align: right; ddo-char-set: 1"
        Me.Label1.Text = "("
        Me.Label1.Top = 0.1181102!
        Me.Label1.Width = 0.1354332!
        '
        'Label2
        '
        Me.Label2.Height = 0.2362205!
        Me.Label2.HyperLink = Nothing
        Me.Label2.Left = 5.297638!
        Me.Label2.Name = "Label2"
        Me.Label2.Style = "font-size: 15pt; text-align: left; ddo-char-set: 1"
        Me.Label2.Text = "度)"
        Me.Label2.Top = 0.1181102!
        Me.Label2.Width = 0.4165354!
        '
        'TxtGYM
        '
        Me.TxtGYM.DataField = "タイトル年月"
        Me.TxtGYM.Height = 0.2362205!
        Me.TxtGYM.Left = 3.923229!
        Me.TxtGYM.Name = "TxtGYM"
        Me.TxtGYM.Style = "font-size: 15.75pt"
        Me.TxtGYM.Text = "yyyy年MM月"
        Me.TxtGYM.Top = 0.1181102!
        Me.TxtGYM.Width = 1.374408!
        '
        'Detail1
        '
        Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.LblColor, Me.TxtCNT_NM, Me.TxtCNT1, Me.TxtCNT2, Me.TxtCNT3, Me.TxtCNT4, Me.TxtCNT5, Me.TxtCNT6, Me.TxtCNT7, Me.Line15, Me.Line16, Me.Line17, Me.Line18, Me.Line19, Me.Line20, Me.Line21, Me.Line22, Me.Line23, Me.Line24, Me.Line25, Me.Line26})
        Me.Detail1.Height = 0.1864338!
        Me.Detail1.Name = "Detail1"
        '
        'LblColor
        '
        Me.LblColor.Height = 0.1968504!
        Me.LblColor.HyperLink = Nothing
        Me.LblColor.Left = 1.377953!
        Me.LblColor.Name = "LblColor"
        Me.LblColor.Style = "background-color: LightGrey"
        Me.LblColor.Text = ""
        Me.LblColor.Top = 0.0!
        Me.LblColor.Visible = False
        Me.LblColor.Width = 5.590551!
        '
        'TxtCNT_NM
        '
        Me.TxtCNT_NM.DataField = "数量名称"
        Me.TxtCNT_NM.Height = 0.1968504!
        Me.TxtCNT_NM.Left = 1.377953!
        Me.TxtCNT_NM.Name = "TxtCNT_NM"
        Me.TxtCNT_NM.Style = "text-align: center; vertical-align: middle"
        Me.TxtCNT_NM.Text = "XXXXXXXXXX"
        Me.TxtCNT_NM.Top = 0.0!
        Me.TxtCNT_NM.Width = 1.181102!
        '
        'TxtCNT1
        '
        Me.TxtCNT1.DataField = "数量1"
        Me.TxtCNT1.Height = 0.1968504!
        Me.TxtCNT1.Left = 2.559055!
        Me.TxtCNT1.Name = "TxtCNT1"
        Me.TxtCNT1.OutputFormat = resources.GetString("TxtCNT1.OutputFormat")
        Me.TxtCNT1.Style = "text-align: right; vertical-align: middle"
        Me.TxtCNT1.Text = "000,000"
        Me.TxtCNT1.Top = 0.0!
        Me.TxtCNT1.Width = 0.6299213!
        '
        'TxtCNT2
        '
        Me.TxtCNT2.DataField = "数量2"
        Me.TxtCNT2.Height = 0.1968504!
        Me.TxtCNT2.Left = 3.188976!
        Me.TxtCNT2.Name = "TxtCNT2"
        Me.TxtCNT2.Style = "text-align: right; vertical-align: middle"
        Me.TxtCNT2.Text = "000,000"
        Me.TxtCNT2.Top = 0.0!
        Me.TxtCNT2.Width = 0.6299213!
        '
        'TxtCNT3
        '
        Me.TxtCNT3.DataField = "数量3"
        Me.TxtCNT3.Height = 0.1968504!
        Me.TxtCNT3.Left = 3.818897!
        Me.TxtCNT3.Name = "TxtCNT3"
        Me.TxtCNT3.Style = "text-align: right; vertical-align: middle"
        Me.TxtCNT3.Text = "000,000"
        Me.TxtCNT3.Top = 0.0!
        Me.TxtCNT3.Width = 0.6299213!
        '
        'TxtCNT4
        '
        Me.TxtCNT4.DataField = "数量4"
        Me.TxtCNT4.Height = 0.1968504!
        Me.TxtCNT4.Left = 4.448819!
        Me.TxtCNT4.Name = "TxtCNT4"
        Me.TxtCNT4.Style = "text-align: right; vertical-align: middle"
        Me.TxtCNT4.Text = "000,000"
        Me.TxtCNT4.Top = 0.0!
        Me.TxtCNT4.Width = 0.6299213!
        '
        'TxtCNT5
        '
        Me.TxtCNT5.DataField = "数量5"
        Me.TxtCNT5.Height = 0.1968504!
        Me.TxtCNT5.Left = 5.07874!
        Me.TxtCNT5.Name = "TxtCNT5"
        Me.TxtCNT5.Style = "text-align: right; vertical-align: middle"
        Me.TxtCNT5.Text = "000,000"
        Me.TxtCNT5.Top = 0.0!
        Me.TxtCNT5.Width = 0.6299213!
        '
        'TxtCNT6
        '
        Me.TxtCNT6.DataField = "数量6"
        Me.TxtCNT6.Height = 0.1968504!
        Me.TxtCNT6.Left = 5.708661!
        Me.TxtCNT6.Name = "TxtCNT6"
        Me.TxtCNT6.Style = "text-align: right; vertical-align: middle"
        Me.TxtCNT6.Text = "000,000"
        Me.TxtCNT6.Top = 0.0!
        Me.TxtCNT6.Width = 0.6299213!
        '
        'TxtCNT7
        '
        Me.TxtCNT7.DataField = "数量7"
        Me.TxtCNT7.Height = 0.1968504!
        Me.TxtCNT7.Left = 6.338583!
        Me.TxtCNT7.Name = "TxtCNT7"
        Me.TxtCNT7.Style = "text-align: right; vertical-align: middle"
        Me.TxtCNT7.Text = "000,000"
        Me.TxtCNT7.Top = 0.0!
        Me.TxtCNT7.Width = 0.6299213!
        '
        'Line15
        '
        Me.Line15.Height = 0.0!
        Me.Line15.Left = 1.377953!
        Me.Line15.LineWeight = 1.0!
        Me.Line15.Name = "Line15"
        Me.Line15.Top = 0.0!
        Me.Line15.Width = 5.590555!
        Me.Line15.X1 = 1.377953!
        Me.Line15.X2 = 6.968508!
        Me.Line15.Y1 = 0.0!
        Me.Line15.Y2 = 0.0!
        '
        'Line16
        '
        Me.Line16.Height = 0.1968504!
        Me.Line16.Left = 0.06614164!
        Me.Line16.LineWeight = 1.0!
        Me.Line16.Name = "Line16"
        Me.Line16.Top = 0.0!
        Me.Line16.Width = 0.00000009685755!
        Me.Line16.X1 = 0.06614174!
        Me.Line16.X2 = 0.06614164!
        Me.Line16.Y1 = 0.0!
        Me.Line16.Y2 = 0.1968504!
        '
        'Line17
        '
        Me.Line17.Height = 0.1968504!
        Me.Line17.Left = 0.5905512!
        Me.Line17.LineWeight = 1.0!
        Me.Line17.Name = "Line17"
        Me.Line17.Top = 0.0!
        Me.Line17.Width = 0.0!
        Me.Line17.X1 = 0.5905512!
        Me.Line17.X2 = 0.5905512!
        Me.Line17.Y1 = 0.0!
        Me.Line17.Y2 = 0.1968504!
        '
        'Line18
        '
        Me.Line18.Height = 0.1968504!
        Me.Line18.Left = 1.377952!
        Me.Line18.LineWeight = 1.0!
        Me.Line18.Name = "Line18"
        Me.Line18.Top = 0.0!
        Me.Line18.Width = 0.0!
        Me.Line18.X1 = 1.377952!
        Me.Line18.X2 = 1.377952!
        Me.Line18.Y1 = 0.0!
        Me.Line18.Y2 = 0.1968504!
        '
        'Line19
        '
        Me.Line19.Height = 0.1968504!
        Me.Line19.Left = 2.559055!
        Me.Line19.LineWeight = 1.0!
        Me.Line19.Name = "Line19"
        Me.Line19.Top = 0.0!
        Me.Line19.Width = 0.0!
        Me.Line19.X1 = 2.559055!
        Me.Line19.X2 = 2.559055!
        Me.Line19.Y1 = 0.0!
        Me.Line19.Y2 = 0.1968504!
        '
        'Line20
        '
        Me.Line20.Height = 0.1968504!
        Me.Line20.Left = 3.188976!
        Me.Line20.LineWeight = 1.0!
        Me.Line20.Name = "Line20"
        Me.Line20.Top = 0.0!
        Me.Line20.Width = 0.0!
        Me.Line20.X1 = 3.188976!
        Me.Line20.X2 = 3.188976!
        Me.Line20.Y1 = 0.0!
        Me.Line20.Y2 = 0.1968504!
        '
        'Line21
        '
        Me.Line21.Height = 0.1968504!
        Me.Line21.Left = 3.818897!
        Me.Line21.LineWeight = 1.0!
        Me.Line21.Name = "Line21"
        Me.Line21.Top = 0.0!
        Me.Line21.Width = 0.0!
        Me.Line21.X1 = 3.818897!
        Me.Line21.X2 = 3.818897!
        Me.Line21.Y1 = 0.0!
        Me.Line21.Y2 = 0.1968504!
        '
        'Line22
        '
        Me.Line22.Height = 0.1968504!
        Me.Line22.Left = 4.448819!
        Me.Line22.LineWeight = 1.0!
        Me.Line22.Name = "Line22"
        Me.Line22.Top = 0.0!
        Me.Line22.Width = 0.0!
        Me.Line22.X1 = 4.448819!
        Me.Line22.X2 = 4.448819!
        Me.Line22.Y1 = 0.0!
        Me.Line22.Y2 = 0.1968504!
        '
        'Line23
        '
        Me.Line23.Height = 0.1968504!
        Me.Line23.Left = 5.07874!
        Me.Line23.LineWeight = 1.0!
        Me.Line23.Name = "Line23"
        Me.Line23.Top = 0.0!
        Me.Line23.Width = 0.0!
        Me.Line23.X1 = 5.07874!
        Me.Line23.X2 = 5.07874!
        Me.Line23.Y1 = 0.0!
        Me.Line23.Y2 = 0.1968504!
        '
        'Line24
        '
        Me.Line24.Height = 0.1968504!
        Me.Line24.Left = 5.708662!
        Me.Line24.LineWeight = 1.0!
        Me.Line24.Name = "Line24"
        Me.Line24.Top = 0.0!
        Me.Line24.Width = 0.0!
        Me.Line24.X1 = 5.708662!
        Me.Line24.X2 = 5.708662!
        Me.Line24.Y1 = 0.0!
        Me.Line24.Y2 = 0.1968504!
        '
        'Line25
        '
        Me.Line25.Height = 0.1968504!
        Me.Line25.Left = 6.338583!
        Me.Line25.LineWeight = 1.0!
        Me.Line25.Name = "Line25"
        Me.Line25.Top = 0.0!
        Me.Line25.Width = 0.0!
        Me.Line25.X1 = 6.338583!
        Me.Line25.X2 = 6.338583!
        Me.Line25.Y1 = 0.0!
        Me.Line25.Y2 = 0.1968504!
        '
        'Line26
        '
        Me.Line26.Height = 0.1968504!
        Me.Line26.Left = 6.968507!
        Me.Line26.LineWeight = 1.0!
        Me.Line26.Name = "Line26"
        Me.Line26.Top = 0.0!
        Me.Line26.Width = 0.0!
        Me.Line26.X1 = 6.968507!
        Me.Line26.X2 = 6.968507!
        Me.Line26.Y1 = 0.0!
        Me.Line26.Y2 = 0.1968504!
        '
        'PageFooter1
        '
        Me.PageFooter1.Height = 0.0!
        Me.PageFooter1.Name = "PageFooter1"
        '
        'GroupHeader1
        '
        Me.GroupHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TextBox5, Me.TextBox6, Me.TextBox7, Me.TxtITEM_TITLE1, Me.TxtITEM_TITLE2, Me.TxtITEM_TITLE3, Me.TxtITEM_TITLE4, Me.TxtITEM_TITLE5, Me.TxtITEM_TITLE6, Me.TxtITEM_TITLE7, Me.Line1, Me.Line2, Me.Line3, Me.Line4, Me.Line5, Me.Line6, Me.Line7, Me.Line8, Me.Line9, Me.Line10, Me.Line11, Me.Line12, Me.Line13})
        Me.GroupHeader1.DataField = "タイトル"
        Me.GroupHeader1.Height = 0.304544!
        Me.GroupHeader1.Name = "GroupHeader1"
        Me.GroupHeader1.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before
        Me.GroupHeader1.RepeatStyle = GrapeCity.ActiveReports.SectionReportModel.RepeatStyle.OnPageIncludeNoDetail
        '
        'TextBox5
        '
        Me.TextBox5.Height = 0.3149606!
        Me.TextBox5.Left = 0.06614174!
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Style = "text-align: center; vertical-align: middle"
        Me.TextBox5.Text = "部位"
        Me.TextBox5.Top = 0.0!
        Me.TextBox5.Width = 0.5244095!
        '
        'TextBox6
        '
        Me.TextBox6.Height = 0.3149606!
        Me.TextBox6.Left = 0.5905512!
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.Style = "text-align: center; vertical-align: middle"
        Me.TextBox6.Text = "目標値"
        Me.TextBox6.Top = 0.0!
        Me.TextBox6.Width = 0.7874016!
        '
        'TextBox7
        '
        Me.TextBox7.Height = 0.3149606!
        Me.TextBox7.Left = 1.377953!
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.Style = "text-align: center; vertical-align: middle"
        Me.TextBox7.Text = "数量"
        Me.TextBox7.Top = 0.0!
        Me.TextBox7.Width = 1.181102!
        '
        'TxtITEM_TITLE1
        '
        Me.TxtITEM_TITLE1.DataField = "項目タイトル1"
        Me.TxtITEM_TITLE1.Height = 0.3149606!
        Me.TxtITEM_TITLE1.Left = 2.559055!
        Me.TxtITEM_TITLE1.Name = "TxtITEM_TITLE1"
        Me.TxtITEM_TITLE1.Style = "text-align: center; vertical-align: middle"
        Me.TxtITEM_TITLE1.Text = "XX月"
        Me.TxtITEM_TITLE1.Top = 0.0!
        Me.TxtITEM_TITLE1.Width = 0.6299213!
        '
        'TxtITEM_TITLE2
        '
        Me.TxtITEM_TITLE2.DataField = "項目タイトル2"
        Me.TxtITEM_TITLE2.Height = 0.3149606!
        Me.TxtITEM_TITLE2.Left = 3.188977!
        Me.TxtITEM_TITLE2.Name = "TxtITEM_TITLE2"
        Me.TxtITEM_TITLE2.Style = "text-align: center; vertical-align: middle"
        Me.TxtITEM_TITLE2.Text = "XX月"
        Me.TxtITEM_TITLE2.Top = 0.0!
        Me.TxtITEM_TITLE2.Width = 0.6299213!
        '
        'TxtITEM_TITLE3
        '
        Me.TxtITEM_TITLE3.DataField = "項目タイトル3"
        Me.TxtITEM_TITLE3.Height = 0.3149606!
        Me.TxtITEM_TITLE3.Left = 3.818898!
        Me.TxtITEM_TITLE3.Name = "TxtITEM_TITLE3"
        Me.TxtITEM_TITLE3.Style = "text-align: center; vertical-align: middle"
        Me.TxtITEM_TITLE3.Text = "XX月"
        Me.TxtITEM_TITLE3.Top = 0.0!
        Me.TxtITEM_TITLE3.Width = 0.6299213!
        '
        'TxtITEM_TITLE4
        '
        Me.TxtITEM_TITLE4.DataField = "項目タイトル4"
        Me.TxtITEM_TITLE4.Height = 0.3149606!
        Me.TxtITEM_TITLE4.Left = 4.448819!
        Me.TxtITEM_TITLE4.Name = "TxtITEM_TITLE4"
        Me.TxtITEM_TITLE4.Style = "text-align: center; vertical-align: middle"
        Me.TxtITEM_TITLE4.Text = "XX月"
        Me.TxtITEM_TITLE4.Top = 0.0!
        Me.TxtITEM_TITLE4.Width = 0.6299213!
        '
        'TxtITEM_TITLE5
        '
        Me.TxtITEM_TITLE5.DataField = "項目タイトル5"
        Me.TxtITEM_TITLE5.Height = 0.3149606!
        Me.TxtITEM_TITLE5.Left = 5.078741!
        Me.TxtITEM_TITLE5.Name = "TxtITEM_TITLE5"
        Me.TxtITEM_TITLE5.Style = "text-align: center; vertical-align: middle"
        Me.TxtITEM_TITLE5.Text = "XX月"
        Me.TxtITEM_TITLE5.Top = 0.0!
        Me.TxtITEM_TITLE5.Width = 0.6299213!
        '
        'TxtITEM_TITLE6
        '
        Me.TxtITEM_TITLE6.DataField = "項目タイトル6"
        Me.TxtITEM_TITLE6.Height = 0.3149606!
        Me.TxtITEM_TITLE6.Left = 5.708662!
        Me.TxtITEM_TITLE6.Name = "TxtITEM_TITLE6"
        Me.TxtITEM_TITLE6.Style = "text-align: center; vertical-align: middle"
        Me.TxtITEM_TITLE6.Text = "XX月"
        Me.TxtITEM_TITLE6.Top = 0.0!
        Me.TxtITEM_TITLE6.Width = 0.6299213!
        '
        'TxtITEM_TITLE7
        '
        Me.TxtITEM_TITLE7.DataField = "項目タイトル7"
        Me.TxtITEM_TITLE7.Height = 0.3149606!
        Me.TxtITEM_TITLE7.Left = 6.338583!
        Me.TxtITEM_TITLE7.Name = "TxtITEM_TITLE7"
        Me.TxtITEM_TITLE7.Style = "text-align: center; vertical-align: middle"
        Me.TxtITEM_TITLE7.Text = "合計"
        Me.TxtITEM_TITLE7.Top = 0.0!
        Me.TxtITEM_TITLE7.Width = 0.6299213!
        '
        'Line1
        '
        Me.Line1.Height = 0.0!
        Me.Line1.Left = 0.06614174!
        Me.Line1.LineWeight = 1.0!
        Me.Line1.Name = "Line1"
        Me.Line1.Top = 0.3149607!
        Me.Line1.Width = 6.902362!
        Me.Line1.X1 = 0.06614174!
        Me.Line1.X2 = 6.968504!
        Me.Line1.Y1 = 0.3149607!
        Me.Line1.Y2 = 0.3149607!
        '
        'Line2
        '
        Me.Line2.Height = 0.0!
        Me.Line2.Left = 0.06614174!
        Me.Line2.LineWeight = 1.0!
        Me.Line2.Name = "Line2"
        Me.Line2.Top = 0.0!
        Me.Line2.Width = 6.902366!
        Me.Line2.X1 = 0.06614174!
        Me.Line2.X2 = 6.968508!
        Me.Line2.Y1 = 0.0!
        Me.Line2.Y2 = 0.0!
        '
        'Line3
        '
        Me.Line3.Height = 0.3149607!
        Me.Line3.Left = 0.06614174!
        Me.Line3.LineWeight = 1.0!
        Me.Line3.Name = "Line3"
        Me.Line3.Top = 0.0!
        Me.Line3.Width = 0.0!
        Me.Line3.X1 = 0.06614174!
        Me.Line3.X2 = 0.06614174!
        Me.Line3.Y1 = 0.0!
        Me.Line3.Y2 = 0.3149607!
        '
        'Line4
        '
        Me.Line4.Height = 0.3149607!
        Me.Line4.Left = 0.5905511!
        Me.Line4.LineWeight = 1.0!
        Me.Line4.Name = "Line4"
        Me.Line4.Top = 0.0!
        Me.Line4.Width = 0.0!
        Me.Line4.X1 = 0.5905511!
        Me.Line4.X2 = 0.5905511!
        Me.Line4.Y1 = 0.0!
        Me.Line4.Y2 = 0.3149607!
        '
        'Line5
        '
        Me.Line5.Height = 0.3149607!
        Me.Line5.Left = 1.377953!
        Me.Line5.LineWeight = 1.0!
        Me.Line5.Name = "Line5"
        Me.Line5.Top = 0.0!
        Me.Line5.Width = 0.0!
        Me.Line5.X1 = 1.377953!
        Me.Line5.X2 = 1.377953!
        Me.Line5.Y1 = 0.0!
        Me.Line5.Y2 = 0.3149607!
        '
        'Line6
        '
        Me.Line6.Height = 0.3149607!
        Me.Line6.Left = 2.559055!
        Me.Line6.LineWeight = 1.0!
        Me.Line6.Name = "Line6"
        Me.Line6.Top = 0.0!
        Me.Line6.Width = 0.0!
        Me.Line6.X1 = 2.559055!
        Me.Line6.X2 = 2.559055!
        Me.Line6.Y1 = 0.0!
        Me.Line6.Y2 = 0.3149607!
        '
        'Line7
        '
        Me.Line7.Height = 0.3149607!
        Me.Line7.Left = 3.188977!
        Me.Line7.LineWeight = 1.0!
        Me.Line7.Name = "Line7"
        Me.Line7.Top = 0.0!
        Me.Line7.Width = 0.0!
        Me.Line7.X1 = 3.188977!
        Me.Line7.X2 = 3.188977!
        Me.Line7.Y1 = 0.0!
        Me.Line7.Y2 = 0.3149607!
        '
        'Line8
        '
        Me.Line8.Height = 0.3149607!
        Me.Line8.Left = 3.818898!
        Me.Line8.LineWeight = 1.0!
        Me.Line8.Name = "Line8"
        Me.Line8.Top = 0.0!
        Me.Line8.Width = 0.0!
        Me.Line8.X1 = 3.818898!
        Me.Line8.X2 = 3.818898!
        Me.Line8.Y1 = 0.0!
        Me.Line8.Y2 = 0.3149607!
        '
        'Line9
        '
        Me.Line9.Height = 0.3149607!
        Me.Line9.Left = 4.448819!
        Me.Line9.LineWeight = 1.0!
        Me.Line9.Name = "Line9"
        Me.Line9.Top = 0.0!
        Me.Line9.Width = 0.0!
        Me.Line9.X1 = 4.448819!
        Me.Line9.X2 = 4.448819!
        Me.Line9.Y1 = 0.0!
        Me.Line9.Y2 = 0.3149607!
        '
        'Line10
        '
        Me.Line10.Height = 0.3149607!
        Me.Line10.Left = 5.078741!
        Me.Line10.LineWeight = 1.0!
        Me.Line10.Name = "Line10"
        Me.Line10.Top = 0.0!
        Me.Line10.Width = 0.0!
        Me.Line10.X1 = 5.078741!
        Me.Line10.X2 = 5.078741!
        Me.Line10.Y1 = 0.0!
        Me.Line10.Y2 = 0.3149607!
        '
        'Line11
        '
        Me.Line11.Height = 0.3149607!
        Me.Line11.Left = 5.708662!
        Me.Line11.LineWeight = 1.0!
        Me.Line11.Name = "Line11"
        Me.Line11.Top = 0.0!
        Me.Line11.Width = 0.0!
        Me.Line11.X1 = 5.708662!
        Me.Line11.X2 = 5.708662!
        Me.Line11.Y1 = 0.0!
        Me.Line11.Y2 = 0.3149607!
        '
        'Line12
        '
        Me.Line12.Height = 0.3149607!
        Me.Line12.Left = 6.338583!
        Me.Line12.LineWeight = 1.0!
        Me.Line12.Name = "Line12"
        Me.Line12.Top = 0.0!
        Me.Line12.Width = 0.0!
        Me.Line12.X1 = 6.338583!
        Me.Line12.X2 = 6.338583!
        Me.Line12.Y1 = 0.0!
        Me.Line12.Y2 = 0.3149607!
        '
        'Line13
        '
        Me.Line13.Height = 0.3149607!
        Me.Line13.Left = 6.968504!
        Me.Line13.LineWeight = 1.0!
        Me.Line13.Name = "Line13"
        Me.Line13.Top = 0.0!
        Me.Line13.Width = 0.0!
        Me.Line13.X1 = 6.968504!
        Me.Line13.X2 = 6.968504!
        Me.Line13.Y1 = 0.0!
        Me.Line13.Y2 = 0.3149607!
        '
        'GroupFooter1
        '
        Me.GroupFooter1.Height = 0.0!
        Me.GroupFooter1.Name = "GroupFooter1"
        '
        'GroupHeader2
        '
        Me.GroupHeader2.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TxtCLASS, Me.TxtTARGET, Me.Line14})
        Me.GroupHeader2.DataField = "部位"
        Me.GroupHeader2.Height = 0.1968504!
        Me.GroupHeader2.Name = "GroupHeader2"
        Me.GroupHeader2.UnderlayNext = True
        '
        'TxtCLASS
        '
        Me.TxtCLASS.CanGrow = False
        Me.TxtCLASS.DataField = "部位"
        Me.TxtCLASS.Height = 0.1968504!
        Me.TxtCLASS.Left = 0.06614174!
        Me.TxtCLASS.Name = "TxtCLASS"
        Me.TxtCLASS.ShrinkToFit = True
        Me.TxtCLASS.Style = "text-align: center; vertical-align: middle; ddo-shrink-to-fit: true"
        Me.TxtCLASS.Text = "XXX"
        Me.TxtCLASS.Top = 0.0!
        Me.TxtCLASS.Width = 0.5244095!
        '
        'TxtTARGET
        '
        Me.TxtTARGET.DataField = "目標値"
        Me.TxtTARGET.Height = 0.1968504!
        Me.TxtTARGET.Left = 0.5905512!
        Me.TxtTARGET.Name = "TxtTARGET"
        Me.TxtTARGET.Style = "text-align: right; vertical-align: middle"
        Me.TxtTARGET.Text = "000,000,000"
        Me.TxtTARGET.Top = 0.0!
        Me.TxtTARGET.Width = 0.7874016!
        '
        'Line14
        '
        Me.Line14.Height = 0.0!
        Me.Line14.Left = 0.06417323!
        Me.Line14.LineWeight = 1.0!
        Me.Line14.Name = "Line14"
        Me.Line14.Top = 0.0!
        Me.Line14.Width = 6.905512!
        Me.Line14.X1 = 0.06417323!
        Me.Line14.X2 = 6.969685!
        Me.Line14.Y1 = 0.0!
        Me.Line14.Y2 = 0.0!
        '
        'GroupFooter2
        '
        Me.GroupFooter2.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.Line27})
        Me.GroupFooter2.Height = 0.0!
        Me.GroupFooter2.Name = "GroupFooter2"
        '
        'Line27
        '
        Me.Line27.Height = 0.0!
        Me.Line27.Left = 0.06299213!
        Me.Line27.LineWeight = 1.0!
        Me.Line27.Name = "Line27"
        Me.Line27.Top = 0.0!
        Me.Line27.Width = 6.90748!
        Me.Line27.X1 = 0.06299213!
        Me.Line27.X2 = 6.970472!
        Me.Line27.Y1 = 0.0!
        Me.Line27.Y2 = 0.0!
        '
        'ETCREP020
        '
        Me.MasterReport = False
        Me.PageSettings.DefaultPaperSize = False
        Me.PageSettings.Margins.Bottom = 0.3937008!
        Me.PageSettings.Margins.Left = 0.7874016!
        Me.PageSettings.Margins.Right = 0.3937008!
        Me.PageSettings.Margins.Top = 0.3937008!
        Me.PageSettings.PaperHeight = 11.69291!
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4
        Me.PageSettings.PaperWidth = 8.267716!
        Me.PrintWidth = 7.086614!
        Me.Sections.Add(Me.PageHeader1)
        Me.Sections.Add(Me.GroupHeader1)
        Me.Sections.Add(Me.GroupHeader2)
        Me.Sections.Add(Me.Detail1)
        Me.Sections.Add(Me.GroupFooter2)
        Me.Sections.Add(Me.GroupFooter1)
        Me.Sections.Add(Me.PageFooter1)
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; " & _
            "color: Black; font-family: ""MS UI Gothic""; ddo-char-set: 128", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: ""MS UI Gothic""; ddo-char-set: 12" & _
            "8", "Heading1", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: ""MS UI Goth" & _
            "ic""; ddo-char-set: 128", "Heading2", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"))
        CType(Me.TxtTITLE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtDATE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtCOMP_NM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtGYM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LblColor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtCNT_NM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtCNT1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtCNT2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtCNT3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtCNT4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtCNT5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtCNT6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtCNT7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtITEM_TITLE1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtITEM_TITLE2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtITEM_TITLE3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtITEM_TITLE4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtITEM_TITLE5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtITEM_TITLE6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtITEM_TITLE7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtCLASS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtTARGET, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Private WithEvents TxtTITLE As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtDATE As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtCOMP_NM As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents GroupHeader1 As GrapeCity.ActiveReports.SectionReportModel.GroupHeader
    Private WithEvents TextBox5 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox6 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox7 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtITEM_TITLE1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtITEM_TITLE2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtITEM_TITLE3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtITEM_TITLE4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtITEM_TITLE5 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtITEM_TITLE6 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtITEM_TITLE7 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Line1 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line2 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line3 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line4 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line5 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line6 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line7 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line8 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line9 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line10 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line11 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line12 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line13 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents GroupFooter1 As GrapeCity.ActiveReports.SectionReportModel.GroupFooter
    Private WithEvents TxtCNT_NM As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtCNT1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtCNT2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtCNT3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtCNT4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtCNT5 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtCNT6 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtCNT7 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Line15 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line16 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line17 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line18 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line19 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line20 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line21 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line22 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line23 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line24 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line25 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line26 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents GroupHeader2 As GrapeCity.ActiveReports.SectionReportModel.GroupHeader
    Private WithEvents GroupFooter2 As GrapeCity.ActiveReports.SectionReportModel.GroupFooter
    Private WithEvents LblColor As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Line27 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents TxtCLASS As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtTARGET As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Line14 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Label1 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label2 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TxtGYM As GrapeCity.ActiveReports.SectionReportModel.TextBox
End Class
