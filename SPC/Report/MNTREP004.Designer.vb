<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class MNTREP004
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
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MNTREP004))
        Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
        Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail()
        Me.Line27 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line29 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line30 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line31 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line32 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line33 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line34 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line44 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.TxtNO = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtPRODUCT_NM = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtCLS = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtSELIAL_NO = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtREMARKS = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtQUANTITY = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblKugiri = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
        Me.GroupHeader1 = New GrapeCity.ActiveReports.SectionReportModel.GroupHeader()
        Me.Line25 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label1 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label2 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Line1 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line2 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line3 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line4 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line5 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line6 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line7 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line9 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line10 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line12 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line13 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line15 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line16 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line8 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line14 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Label3 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label4 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label5 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label6 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TxtDESTINATION = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtDELIVERY_D = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtSENDER = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtCONTENT = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox14 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtORDER_NO = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Line19 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line20 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line21 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line22 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line23 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line24 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line41 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line42 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line43 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Label7 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label8 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label9 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label10 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label11 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.GroupFooter1 = New GrapeCity.ActiveReports.SectionReportModel.GroupFooter()
        Me.Line35 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line37 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line38 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line39 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Line40 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Label12 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TxtTOTAL = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        CType(Me.TxtNO, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtPRODUCT_NM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtCLS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtSELIAL_NO, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtREMARKS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtQUANTITY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblKugiri, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtDESTINATION, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtDELIVERY_D, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtSENDER, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtCONTENT, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox14, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtORDER_NO, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label12, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtTOTAL, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'PageHeader1
        '
        Me.PageHeader1.Height = 0.0!
        Me.PageHeader1.Name = "PageHeader1"
        '
        'Detail1
        '
        Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.Line27, Me.Line29, Me.Line30, Me.Line31, Me.Line32, Me.Line33, Me.Line34, Me.Line44, Me.TxtNO, Me.TxtPRODUCT_NM, Me.TxtCLS, Me.TxtSELIAL_NO, Me.TxtREMARKS, Me.TxtQUANTITY, Me.lblKugiri})
        Me.Detail1.Height = 0.3385827!
        Me.Detail1.Name = "Detail1"
        '
        'Line27
        '
        Me.Line27.Height = 0.0!
        Me.Line27.Left = 0.4944882!
        Me.Line27.LineWeight = 1.0!
        Me.Line27.Name = "Line27"
        Me.Line27.Top = 0.3381891!
        Me.Line27.Width = 6.744882!
        Me.Line27.X1 = 7.23937!
        Me.Line27.X2 = 0.4944882!
        Me.Line27.Y1 = 0.3381891!
        Me.Line27.Y2 = 0.3381891!
        '
        'Line29
        '
        Me.Line29.Height = 0.338189!
        Me.Line29.Left = 0.4944882!
        Me.Line29.LineWeight = 2.0!
        Me.Line29.Name = "Line29"
        Me.Line29.Top = 0.0!
        Me.Line29.Width = 0.0!
        Me.Line29.X1 = 0.4944882!
        Me.Line29.X2 = 0.4944882!
        Me.Line29.Y1 = 0.0!
        Me.Line29.Y2 = 0.338189!
        '
        'Line30
        '
        Me.Line30.Height = 0.3381889!
        Me.Line30.Left = 7.239371!
        Me.Line30.LineWeight = 2.0!
        Me.Line30.Name = "Line30"
        Me.Line30.Top = 0.0!
        Me.Line30.Width = 0.0!
        Me.Line30.X1 = 7.239371!
        Me.Line30.X2 = 7.239371!
        Me.Line30.Y1 = 0.0!
        Me.Line30.Y2 = 0.3381889!
        '
        'Line31
        '
        Me.Line31.Height = 0.3381889!
        Me.Line31.Left = 3.525985!
        Me.Line31.LineWeight = 1.0!
        Me.Line31.Name = "Line31"
        Me.Line31.Top = 0.0!
        Me.Line31.Width = 0.0!
        Me.Line31.X1 = 3.525985!
        Me.Line31.X2 = 3.525985!
        Me.Line31.Y1 = 0.0!
        Me.Line31.Y2 = 0.3381889!
        '
        'Line32
        '
        Me.Line32.Height = 0.3381889!
        Me.Line32.Left = 2.672047!
        Me.Line32.LineWeight = 1.0!
        Me.Line32.Name = "Line32"
        Me.Line32.Top = 0.0!
        Me.Line32.Width = 0.0!
        Me.Line32.X1 = 2.672047!
        Me.Line32.X2 = 2.672047!
        Me.Line32.Y1 = 0.0!
        Me.Line32.Y2 = 0.3381889!
        '
        'Line33
        '
        Me.Line33.Height = 0.3381889!
        Me.Line33.Left = 5.442914!
        Me.Line33.LineWeight = 1.0!
        Me.Line33.Name = "Line33"
        Me.Line33.Top = 0.0!
        Me.Line33.Width = 0.0!
        Me.Line33.X1 = 5.442914!
        Me.Line33.X2 = 5.442914!
        Me.Line33.Y1 = 0.0!
        Me.Line33.Y2 = 0.3381889!
        '
        'Line34
        '
        Me.Line34.Height = 0.3381889!
        Me.Line34.Left = 6.515749!
        Me.Line34.LineWeight = 1.0!
        Me.Line34.Name = "Line34"
        Me.Line34.Top = 0.0!
        Me.Line34.Width = 0.0!
        Me.Line34.X1 = 6.515749!
        Me.Line34.X2 = 6.515749!
        Me.Line34.Y1 = 0.0!
        Me.Line34.Y2 = 0.3381889!
        '
        'Line44
        '
        Me.Line44.Height = 0.338189!
        Me.Line44.Left = 0.8748031!
        Me.Line44.LineWeight = 1.0!
        Me.Line44.Name = "Line44"
        Me.Line44.Top = 0.0!
        Me.Line44.Width = 0.00000005960464!
        Me.Line44.X1 = 0.8748032!
        Me.Line44.X2 = 0.8748031!
        Me.Line44.Y1 = 0.0!
        Me.Line44.Y2 = 0.338189!
        '
        'TxtNO
        '
        Me.TxtNO.Height = 0.2417322!
        Me.TxtNO.Left = 0.5574803!
        Me.TxtNO.Name = "TxtNO"
        Me.TxtNO.ShrinkToFit = True
        Me.TxtNO.Style = "text-align: center; vertical-align: middle; ddo-font-vertical: none; ddo-shrink-t" & _
    "o-fit: true"
        Me.TxtNO.Text = "XX"
        Me.TxtNO.Top = 0.06732284!
        Me.TxtNO.Width = 0.2653543!
        '
        'TxtPRODUCT_NM
        '
        Me.TxtPRODUCT_NM.DataField = "品名"
        Me.TxtPRODUCT_NM.Height = 0.2417322!
        Me.TxtPRODUCT_NM.Left = 0.9141734!
        Me.TxtPRODUCT_NM.Name = "TxtPRODUCT_NM"
        Me.TxtPRODUCT_NM.ShrinkToFit = True
        Me.TxtPRODUCT_NM.Style = "text-align: center; vertical-align: middle; ddo-font-vertical: none; ddo-shrink-t" & _
    "o-fit: true"
        Me.TxtPRODUCT_NM.Text = "NNNNNNNNNNNNNNNNNN"
        Me.TxtPRODUCT_NM.Top = 0.06732284!
        Me.TxtPRODUCT_NM.Width = 1.723622!
        '
        'TxtCLS
        '
        Me.TxtCLS.DataField = "区分"
        Me.TxtCLS.Height = 0.2417322!
        Me.TxtCLS.Left = 2.770866!
        Me.TxtCLS.Name = "TxtCLS"
        Me.TxtCLS.Style = "text-align: center; vertical-align: middle; ddo-font-vertical: none"
        Me.TxtCLS.Text = "XX"
        Me.TxtCLS.Top = 0.06732284!
        Me.TxtCLS.Width = 0.6767716!
        '
        'TxtSELIAL_NO
        '
        Me.TxtSELIAL_NO.DataField = "シリアルNo"
        Me.TxtSELIAL_NO.Height = 0.2417322!
        Me.TxtSELIAL_NO.Left = 3.599213!
        Me.TxtSELIAL_NO.Name = "TxtSELIAL_NO"
        Me.TxtSELIAL_NO.Style = "text-align: center; vertical-align: middle; ddo-font-vertical: none"
        Me.TxtSELIAL_NO.Text = "NNNNNNNNNNNNNNNNNN"
        Me.TxtSELIAL_NO.Top = 0.06732284!
        Me.TxtSELIAL_NO.Width = 1.754725!
        '
        'TxtREMARKS
        '
        Me.TxtREMARKS.DataField = "備考"
        Me.TxtREMARKS.Height = 0.2417322!
        Me.TxtREMARKS.Left = 5.479922!
        Me.TxtREMARKS.Name = "TxtREMARKS"
        Me.TxtREMARKS.ShrinkToFit = True
        Me.TxtREMARKS.Style = "text-align: center; vertical-align: middle; ddo-font-vertical: none; ddo-shrink-t" & _
    "o-fit: true"
        Me.TxtREMARKS.Text = "NNNNNNNNNN"
        Me.TxtREMARKS.Top = 0.06732284!
        Me.TxtREMARKS.Width = 0.9944885!
        '
        'TxtQUANTITY
        '
        Me.TxtQUANTITY.DataField = "数量"
        Me.TxtQUANTITY.Height = 0.2417322!
        Me.TxtQUANTITY.Left = 6.567323!
        Me.TxtQUANTITY.Name = "TxtQUANTITY"
        Me.TxtQUANTITY.Style = "text-align: center; vertical-align: middle; ddo-font-vertical: none"
        Me.TxtQUANTITY.Text = "NNNN"
        Me.TxtQUANTITY.Top = 0.06732284!
        Me.TxtQUANTITY.Width = 0.6094489!
        '
        'lblKugiri
        '
        Me.lblKugiri.DataField = "区切り"
        Me.lblKugiri.Height = 0.2!
        Me.lblKugiri.HyperLink = Nothing
        Me.lblKugiri.Left = 6.078347!
        Me.lblKugiri.Name = "lblKugiri"
        Me.lblKugiri.Style = ""
        Me.lblKugiri.Text = "Label14"
        Me.lblKugiri.Top = 0.1090551!
        Me.lblKugiri.Visible = False
        Me.lblKugiri.Width = 1.0!
        '
        'PageFooter1
        '
        Me.PageFooter1.Height = 0.0!
        Me.PageFooter1.Name = "PageFooter1"
        '
        'GroupHeader1
        '
        Me.GroupHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.Line25, Me.TextBox1, Me.Label1, Me.Label2, Me.Line1, Me.Line2, Me.Line3, Me.Line4, Me.Line5, Me.Line6, Me.Line7, Me.Line9, Me.Line10, Me.Line12, Me.Line13, Me.Line15, Me.Line16, Me.Line8, Me.Line14, Me.Label3, Me.Label4, Me.Label5, Me.Label6, Me.TxtDESTINATION, Me.TxtDELIVERY_D, Me.TxtSENDER, Me.TxtCONTENT, Me.TextBox14, Me.TxtORDER_NO, Me.Line19, Me.Line20, Me.Line21, Me.Line22, Me.Line23, Me.Line24, Me.Line41, Me.Line42, Me.Line43, Me.Label7, Me.Label8, Me.Label9, Me.Label10, Me.Label11})
        Me.GroupHeader1.DataField = "注文番号"
        Me.GroupHeader1.Height = 2.662375!
        Me.GroupHeader1.Name = "GroupHeader1"
        Me.GroupHeader1.RepeatStyle = GrapeCity.ActiveReports.SectionReportModel.RepeatStyle.All
        '
        'Line25
        '
        Me.Line25.Height = 0.3381889!
        Me.Line25.Left = 6.515749!
        Me.Line25.LineWeight = 1.0!
        Me.Line25.Name = "Line25"
        Me.Line25.Top = 2.337795!
        Me.Line25.Width = 0.0!
        Me.Line25.X1 = 6.515749!
        Me.Line25.X2 = 6.515749!
        Me.Line25.Y1 = 2.337795!
        Me.Line25.Y2 = 2.675984!
        '
        'TextBox1
        '
        Me.TextBox1.DataField = "帳票出力日"
        Me.TextBox1.Height = 0.2!
        Me.TextBox1.Left = 6.0!
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Style = "text-align: right"
        Me.TextBox1.Text = "平成XX年XX月XX日"
        Me.TextBox1.Top = 0.9232287!
        Me.TextBox1.Width = 1.23937!
        '
        'Label1
        '
        Me.Label1.Height = 0.3562993!
        Me.Label1.HyperLink = Nothing
        Me.Label1.Left = 0.0!
        Me.Label1.Name = "Label1"
        Me.Label1.Style = "font-family: メイリオ; font-size: 15pt; font-weight: bold; text-align: center; vertic" & _
    "al-align: middle; ddo-char-set: 1"
        Me.Label1.Text = "整備完了品一覧表"
        Me.Label1.Top = 0.4834646!
        Me.Label1.Width = 7.63543!
        '
        'Label2
        '
        Me.Label2.DataField = "自社名"
        Me.Label2.Height = 0.2!
        Me.Label2.HyperLink = Nothing
        Me.Label2.Left = 5.666536!
        Me.Label2.Name = "Label2"
        Me.Label2.Style = "text-align: right"
        Me.Label2.Text = "エフ・エス㈱　サポートセンタ"
        Me.Label2.Top = 1.123228!
        Me.Label2.Width = 1.572835!
        '
        'Line1
        '
        Me.Line1.Height = 0.0!
        Me.Line1.Left = 0.4944882!
        Me.Line1.LineWeight = 2.0!
        Me.Line1.Name = "Line1"
        Me.Line1.Top = 1.323228!
        Me.Line1.Width = 6.74488!
        Me.Line1.X1 = 7.239368!
        Me.Line1.X2 = 0.4944882!
        Me.Line1.Y1 = 1.323228!
        Me.Line1.Y2 = 1.323228!
        '
        'Line2
        '
        Me.Line2.Height = 0.0!
        Me.Line2.Left = 0.4943099!
        Me.Line2.LineWeight = 1.0!
        Me.Line2.Name = "Line2"
        Me.Line2.Top = 1.661417!
        Me.Line2.Width = 6.745058!
        Me.Line2.X1 = 7.239368!
        Me.Line2.X2 = 0.4943099!
        Me.Line2.Y1 = 1.661417!
        Me.Line2.Y2 = 1.661417!
        '
        'Line3
        '
        Me.Line3.Height = 0.338189!
        Me.Line3.Left = 0.4944882!
        Me.Line3.LineWeight = 2.0!
        Me.Line3.Name = "Line3"
        Me.Line3.Top = 1.323228!
        Me.Line3.Width = 0.0!
        Me.Line3.X1 = 0.4944882!
        Me.Line3.X2 = 0.4944882!
        Me.Line3.Y1 = 1.323228!
        Me.Line3.Y2 = 1.661417!
        '
        'Line4
        '
        Me.Line4.Height = 0.3381889!
        Me.Line4.Left = 7.239371!
        Me.Line4.LineWeight = 2.0!
        Me.Line4.Name = "Line4"
        Me.Line4.Top = 2.337795!
        Me.Line4.Width = 0.0!
        Me.Line4.X1 = 7.239371!
        Me.Line4.X2 = 7.239371!
        Me.Line4.Y1 = 2.337795!
        Me.Line4.Y2 = 2.675984!
        '
        'Line5
        '
        Me.Line5.Height = 0.338189!
        Me.Line5.Left = 1.672047!
        Me.Line5.LineWeight = 1.0!
        Me.Line5.Name = "Line5"
        Me.Line5.Top = 1.323228!
        Me.Line5.Width = 0.0!
        Me.Line5.X1 = 1.672047!
        Me.Line5.X2 = 1.672047!
        Me.Line5.Y1 = 1.323228!
        Me.Line5.Y2 = 1.661417!
        '
        'Line6
        '
        Me.Line6.Height = 0.0!
        Me.Line6.Left = 0.4944882!
        Me.Line6.LineWeight = 1.0!
        Me.Line6.Name = "Line6"
        Me.Line6.Top = 1.999606!
        Me.Line6.Width = 6.74488!
        Me.Line6.X1 = 7.239368!
        Me.Line6.X2 = 0.4944882!
        Me.Line6.Y1 = 1.999606!
        Me.Line6.Y2 = 1.999606!
        '
        'Line7
        '
        Me.Line7.Height = 0.338189!
        Me.Line7.Left = 0.4944882!
        Me.Line7.LineWeight = 2.0!
        Me.Line7.Name = "Line7"
        Me.Line7.Top = 1.661417!
        Me.Line7.Width = 0.0!
        Me.Line7.X1 = 0.4944882!
        Me.Line7.X2 = 0.4944882!
        Me.Line7.Y1 = 1.661417!
        Me.Line7.Y2 = 1.999606!
        '
        'Line9
        '
        Me.Line9.Height = 0.338189!
        Me.Line9.Left = 7.239371!
        Me.Line9.LineWeight = 2.0!
        Me.Line9.Name = "Line9"
        Me.Line9.Top = 1.323228!
        Me.Line9.Width = 0.0!
        Me.Line9.X1 = 7.239371!
        Me.Line9.X2 = 7.239371!
        Me.Line9.Y1 = 1.323228!
        Me.Line9.Y2 = 1.661417!
        '
        'Line10
        '
        Me.Line10.Height = 0.338189!
        Me.Line10.Left = 1.672047!
        Me.Line10.LineWeight = 1.0!
        Me.Line10.Name = "Line10"
        Me.Line10.Top = 1.661417!
        Me.Line10.Width = 0.0!
        Me.Line10.X1 = 1.672047!
        Me.Line10.X2 = 1.672047!
        Me.Line10.Y1 = 1.661417!
        Me.Line10.Y2 = 1.999606!
        '
        'Line12
        '
        Me.Line12.Height = 0.0!
        Me.Line12.Left = 0.49449!
        Me.Line12.LineWeight = 2.0!
        Me.Line12.Name = "Line12"
        Me.Line12.Top = 2.337795!
        Me.Line12.Width = 6.744878!
        Me.Line12.X1 = 7.239368!
        Me.Line12.X2 = 0.49449!
        Me.Line12.Y1 = 2.337795!
        Me.Line12.Y2 = 2.337795!
        '
        'Line13
        '
        Me.Line13.Height = 0.338189!
        Me.Line13.Left = 0.4944883!
        Me.Line13.LineWeight = 2.0!
        Me.Line13.Name = "Line13"
        Me.Line13.Top = 1.999606!
        Me.Line13.Width = 0.0!
        Me.Line13.X1 = 0.4944883!
        Me.Line13.X2 = 0.4944883!
        Me.Line13.Y1 = 1.999606!
        Me.Line13.Y2 = 2.337795!
        '
        'Line15
        '
        Me.Line15.Height = 0.33819!
        Me.Line15.Left = 7.239371!
        Me.Line15.LineWeight = 2.0!
        Me.Line15.Name = "Line15"
        Me.Line15.Top = 1.661417!
        Me.Line15.Width = 0.0!
        Me.Line15.X1 = 7.239371!
        Me.Line15.X2 = 7.239371!
        Me.Line15.Y1 = 1.661417!
        Me.Line15.Y2 = 1.999607!
        '
        'Line16
        '
        Me.Line16.Height = 0.338189!
        Me.Line16.Left = 1.672047!
        Me.Line16.LineWeight = 1.0!
        Me.Line16.Name = "Line16"
        Me.Line16.Top = 1.999606!
        Me.Line16.Width = 0.0!
        Me.Line16.X1 = 1.672047!
        Me.Line16.X2 = 1.672047!
        Me.Line16.Y1 = 1.999606!
        Me.Line16.Y2 = 2.337795!
        '
        'Line8
        '
        Me.Line8.Height = 0.338189!
        Me.Line8.Left = 3.525985!
        Me.Line8.LineWeight = 1.0!
        Me.Line8.Name = "Line8"
        Me.Line8.Top = 1.661417!
        Me.Line8.Width = 0.0!
        Me.Line8.X1 = 3.525985!
        Me.Line8.X2 = 3.525985!
        Me.Line8.Y1 = 1.661417!
        Me.Line8.Y2 = 1.999606!
        '
        'Line14
        '
        Me.Line14.Height = 0.338189!
        Me.Line14.Left = 4.338583!
        Me.Line14.LineWeight = 1.0!
        Me.Line14.Name = "Line14"
        Me.Line14.Top = 1.661417!
        Me.Line14.Width = 0.0!
        Me.Line14.X1 = 4.338583!
        Me.Line14.X2 = 4.338583!
        Me.Line14.Y1 = 1.661417!
        Me.Line14.Y2 = 1.999606!
        '
        'Label3
        '
        Me.Label3.Height = 0.2937008!
        Me.Label3.HyperLink = Nothing
        Me.Label3.Left = 0.6043308!
        Me.Label3.Name = "Label3"
        Me.Label3.Style = "font-size: 12pt; font-weight: bold; text-align: center; vertical-align: middle"
        Me.Label3.Text = "送付先"
        Me.Label3.Top = 1.367717!
        Me.Label3.Width = 1.0!
        '
        'Label4
        '
        Me.Label4.Height = 0.2937008!
        Me.Label4.HyperLink = Nothing
        Me.Label4.Left = 0.6043308!
        Me.Label4.Name = "Label4"
        Me.Label4.Style = "font-size: 12pt; font-weight: bold; text-align: center; vertical-align: middle"
        Me.Label4.Text = "納入日"
        Me.Label4.Top = 1.705906!
        Me.Label4.Width = 1.0!
        '
        'Label5
        '
        Me.Label5.Height = 0.2937008!
        Me.Label5.HyperLink = Nothing
        Me.Label5.Left = 0.6043308!
        Me.Label5.Name = "Label5"
        Me.Label5.Style = "font-size: 12pt; font-weight: bold; text-align: center; vertical-align: middle"
        Me.Label5.Text = "作業内容"
        Me.Label5.Top = 2.044096!
        Me.Label5.Width = 1.0!
        '
        'Label6
        '
        Me.Label6.Height = 0.2937008!
        Me.Label6.HyperLink = Nothing
        Me.Label6.Left = 3.599213!
        Me.Label6.Name = "Label6"
        Me.Label6.Style = "font-size: 12pt; font-weight: bold; text-align: center; vertical-align: middle"
        Me.Label6.Text = "送付元"
        Me.Label6.Top = 1.705906!
        Me.Label6.Width = 0.6767716!
        '
        'TxtDESTINATION
        '
        Me.TxtDESTINATION.DataField = "送付先名"
        Me.TxtDESTINATION.Height = 0.2417322!
        Me.TxtDESTINATION.Left = 1.916535!
        Me.TxtDESTINATION.Name = "TxtDESTINATION"
        Me.TxtDESTINATION.Style = "font-weight: bold; vertical-align: middle; ddo-font-vertical: none"
        Me.TxtDESTINATION.Text = "NNNNNNNNNNNNNNNNNNN"
        Me.TxtDESTINATION.Top = 1.367717!
        Me.TxtDESTINATION.Width = 5.161812!
        '
        'TxtDELIVERY_D
        '
        Me.TxtDELIVERY_D.DataField = "納入日"
        Me.TxtDELIVERY_D.Height = 0.2417322!
        Me.TxtDELIVERY_D.Left = 1.916535!
        Me.TxtDELIVERY_D.Name = "TxtDELIVERY_D"
        Me.TxtDELIVERY_D.Style = "font-weight: bold; text-align: left; vertical-align: middle; ddo-font-vertical: n" & _
    "one"
        Me.TxtDELIVERY_D.Text = "平成yy年MM月dd日"
        Me.TxtDELIVERY_D.Top = 1.705906!
        Me.TxtDELIVERY_D.Width = 1.531103!
        '
        'TxtSENDER
        '
        Me.TxtSENDER.DataField = "送付元名"
        Me.TxtSENDER.Height = 0.2417322!
        Me.TxtSENDER.Left = 4.541733!
        Me.TxtSENDER.Name = "TxtSENDER"
        Me.TxtSENDER.Style = "font-weight: bold; vertical-align: middle; ddo-font-vertical: none"
        Me.TxtSENDER.Text = "NNNNNNNNNNNNNNNNNNN"
        Me.TxtSENDER.Top = 1.705906!
        Me.TxtSENDER.Width = 2.63504!
        '
        'TxtCONTENT
        '
        Me.TxtCONTENT.DataField = "作業内容"
        Me.TxtCONTENT.Height = 0.2417322!
        Me.TxtCONTENT.Left = 1.916535!
        Me.TxtCONTENT.Name = "TxtCONTENT"
        Me.TxtCONTENT.Style = "font-weight: bold; vertical-align: middle; ddo-font-vertical: none"
        Me.TxtCONTENT.Text = "項目"
        Me.TxtCONTENT.Top = 2.044096!
        Me.TxtCONTENT.Width = 1.609449!
        '
        'TextBox14
        '
        Me.TextBox14.DataField = "バージョン"
        Me.TextBox14.Height = 0.2417322!
        Me.TextBox14.Left = 3.525985!
        Me.TextBox14.Name = "TextBox14"
        Me.TextBox14.Style = "font-weight: bold; vertical-align: middle; ddo-font-vertical: none"
        Me.TextBox14.Text = "バージョン"
        Me.TextBox14.Top = 2.044096!
        Me.TextBox14.Width = 1.015749!
        '
        'TxtORDER_NO
        '
        Me.TxtORDER_NO.DataField = "注文番号"
        Me.TxtORDER_NO.Height = 0.2417322!
        Me.TxtORDER_NO.Left = 4.541733!
        Me.TxtORDER_NO.Name = "TxtORDER_NO"
        Me.TxtORDER_NO.Style = "font-weight: bold; vertical-align: middle; ddo-font-vertical: none"
        Me.TxtORDER_NO.Text = "注文番号"
        Me.TxtORDER_NO.Top = 2.044096!
        Me.TxtORDER_NO.Width = 1.885039!
        '
        'Line19
        '
        Me.Line19.Height = 0.0!
        Me.Line19.Left = 0.4944915!
        Me.Line19.LineWeight = 1.0!
        Me.Line19.Name = "Line19"
        Me.Line19.Top = 2.675984!
        Me.Line19.Width = 6.744876!
        Me.Line19.X1 = 7.239368!
        Me.Line19.X2 = 0.4944915!
        Me.Line19.Y1 = 2.675984!
        Me.Line19.Y2 = 2.675984!
        '
        'Line20
        '
        Me.Line20.Height = 0.3381889!
        Me.Line20.Left = 0.4944904!
        Me.Line20.LineWeight = 2.0!
        Me.Line20.Name = "Line20"
        Me.Line20.Top = 2.337795!
        Me.Line20.Width = 0.0!
        Me.Line20.X1 = 0.4944904!
        Me.Line20.X2 = 0.4944904!
        Me.Line20.Y1 = 2.337795!
        Me.Line20.Y2 = 2.675984!
        '
        'Line21
        '
        Me.Line21.Height = 0.338189!
        Me.Line21.Left = 7.239371!
        Me.Line21.LineWeight = 2.0!
        Me.Line21.Name = "Line21"
        Me.Line21.Top = 1.999606!
        Me.Line21.Width = 0.0!
        Me.Line21.X1 = 7.239371!
        Me.Line21.X2 = 7.239371!
        Me.Line21.Y1 = 1.999606!
        Me.Line21.Y2 = 2.337795!
        '
        'Line22
        '
        Me.Line22.Height = 0.3381889!
        Me.Line22.Left = 3.525987!
        Me.Line22.LineWeight = 1.0!
        Me.Line22.Name = "Line22"
        Me.Line22.Top = 2.337795!
        Me.Line22.Width = 0.0!
        Me.Line22.X1 = 3.525987!
        Me.Line22.X2 = 3.525987!
        Me.Line22.Y1 = 2.337795!
        Me.Line22.Y2 = 2.675984!
        '
        'Line23
        '
        Me.Line23.Height = 0.3381889!
        Me.Line23.Left = 2.672049!
        Me.Line23.LineWeight = 1.0!
        Me.Line23.Name = "Line23"
        Me.Line23.Top = 2.337795!
        Me.Line23.Width = 0.0!
        Me.Line23.X1 = 2.672049!
        Me.Line23.X2 = 2.672049!
        Me.Line23.Y1 = 2.337795!
        Me.Line23.Y2 = 2.675984!
        '
        'Line24
        '
        Me.Line24.Height = 0.3381889!
        Me.Line24.Left = 5.442916!
        Me.Line24.LineWeight = 1.0!
        Me.Line24.Name = "Line24"
        Me.Line24.Top = 2.337795!
        Me.Line24.Width = 0.0!
        Me.Line24.X1 = 5.442916!
        Me.Line24.X2 = 5.442916!
        Me.Line24.Y1 = 2.337795!
        Me.Line24.Y2 = 2.675984!
        '
        'Line41
        '
        Me.Line41.Height = 0.0!
        Me.Line41.Left = 0.4944915!
        Me.Line41.LineWeight = 1.0!
        Me.Line41.Name = "Line41"
        Me.Line41.Top = 2.711812!
        Me.Line41.Width = 6.744876!
        Me.Line41.X1 = 7.239368!
        Me.Line41.X2 = 0.4944915!
        Me.Line41.Y1 = 2.711812!
        Me.Line41.Y2 = 2.711812!
        '
        'Line42
        '
        Me.Line42.Height = 0.03582811!
        Me.Line42.Left = 0.4944904!
        Me.Line42.LineWeight = 2.0!
        Me.Line42.Name = "Line42"
        Me.Line42.Top = 2.675984!
        Me.Line42.Width = 0.0!
        Me.Line42.X1 = 0.4944904!
        Me.Line42.X2 = 0.4944904!
        Me.Line42.Y1 = 2.675984!
        Me.Line42.Y2 = 2.711812!
        '
        'Line43
        '
        Me.Line43.Height = 0.03582716!
        Me.Line43.Left = 7.239369!
        Me.Line43.LineWeight = 2.0!
        Me.Line43.Name = "Line43"
        Me.Line43.Top = 2.19252!
        Me.Line43.Width = 0.0!
        Me.Line43.X1 = 7.239369!
        Me.Line43.X2 = 7.239369!
        Me.Line43.Y1 = 2.19252!
        Me.Line43.Y2 = 2.228347!
        '
        'Label7
        '
        Me.Label7.Height = 0.2937005!
        Me.Label7.HyperLink = Nothing
        Me.Label7.Left = 1.239764!
        Me.Label7.Name = "Label7"
        Me.Label7.Style = "font-size: 12pt; font-weight: bold; text-align: center; vertical-align: middle"
        Me.Label7.Text = "品名"
        Me.Label7.Top = 2.34567!
        Me.Label7.Width = 0.6767716!
        '
        'Label8
        '
        Me.Label8.Height = 0.2937005!
        Me.Label8.HyperLink = Nothing
        Me.Label8.Left = 2.770866!
        Me.Label8.Name = "Label8"
        Me.Label8.Style = "font-size: 12pt; font-weight: bold; text-align: center; vertical-align: middle"
        Me.Label8.Text = "区分"
        Me.Label8.Top = 2.34567!
        Me.Label8.Width = 0.6767716!
        '
        'Label9
        '
        Me.Label9.Height = 0.2937005!
        Me.Label9.HyperLink = Nothing
        Me.Label9.Left = 4.046851!
        Me.Label9.Name = "Label9"
        Me.Label9.Style = "font-size: 12pt; font-weight: bold; text-align: center; vertical-align: middle"
        Me.Label9.Text = "シリアルNo."
        Me.Label9.Top = 2.34567!
        Me.Label9.Width = 1.0!
        '
        'Label10
        '
        Me.Label10.Height = 0.2937005!
        Me.Label10.HyperLink = Nothing
        Me.Label10.Left = 5.666537!
        Me.Label10.Name = "Label10"
        Me.Label10.Style = "font-size: 12pt; font-weight: bold; text-align: center; vertical-align: middle"
        Me.Label10.Text = "備考"
        Me.Label10.Top = 2.34567!
        Me.Label10.Width = 0.6767716!
        '
        'Label11
        '
        Me.Label11.Height = 0.2937005!
        Me.Label11.HyperLink = Nothing
        Me.Label11.Left = 6.567324!
        Me.Label11.Name = "Label11"
        Me.Label11.Style = "font-size: 12pt; font-weight: bold; text-align: center; vertical-align: middle"
        Me.Label11.Text = "数量"
        Me.Label11.Top = 2.34567!
        Me.Label11.Width = 0.6094489!
        '
        'GroupFooter1
        '
        Me.GroupFooter1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.Line35, Me.Line37, Me.Line38, Me.Line39, Me.Line40, Me.Label12, Me.TxtTOTAL})
        Me.GroupFooter1.Height = 1.345134!
        Me.GroupFooter1.Name = "GroupFooter1"
        Me.GroupFooter1.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.After
        '
        'Line35
        '
        Me.Line35.Height = 0.0!
        Me.Line35.Left = 0.4943093!
        Me.Line35.LineWeight = 2.0!
        Me.Line35.Name = "Line35"
        Me.Line35.Top = 0.0!
        Me.Line35.Width = 6.74506!
        Me.Line35.X1 = 7.23937!
        Me.Line35.X2 = 0.4943093!
        Me.Line35.Y1 = 0.0!
        Me.Line35.Y2 = 0.0!
        '
        'Line37
        '
        Me.Line37.Height = 0.0!
        Me.Line37.Left = 0.494311!
        Me.Line37.LineWeight = 2.0!
        Me.Line37.Name = "Line37"
        Me.Line37.Top = 0.338189!
        Me.Line37.Width = 6.74506!
        Me.Line37.X1 = 7.239371!
        Me.Line37.X2 = 0.494311!
        Me.Line37.Y1 = 0.338189!
        Me.Line37.Y2 = 0.338189!
        '
        'Line38
        '
        Me.Line38.Height = 0.3381889!
        Me.Line38.Left = 0.4944882!
        Me.Line38.LineWeight = 2.0!
        Me.Line38.Name = "Line38"
        Me.Line38.Top = 0.0000001192093!
        Me.Line38.Width = 0.0!
        Me.Line38.X1 = 0.4944882!
        Me.Line38.X2 = 0.4944882!
        Me.Line38.Y1 = 0.0000001192093!
        Me.Line38.Y2 = 0.338189!
        '
        'Line39
        '
        Me.Line39.Height = 0.3381889!
        Me.Line39.Left = 7.239368!
        Me.Line39.LineWeight = 2.0!
        Me.Line39.Name = "Line39"
        Me.Line39.Top = 0.0000001192093!
        Me.Line39.Width = 0.0!
        Me.Line39.X1 = 7.239368!
        Me.Line39.X2 = 7.239368!
        Me.Line39.Y1 = 0.0000001192093!
        Me.Line39.Y2 = 0.338189!
        '
        'Line40
        '
        Me.Line40.Height = 0.3381889!
        Me.Line40.Left = 6.515749!
        Me.Line40.LineWeight = 2.0!
        Me.Line40.Name = "Line40"
        Me.Line40.Top = 0.0!
        Me.Line40.Width = 0.0!
        Me.Line40.X1 = 6.515749!
        Me.Line40.X2 = 6.515749!
        Me.Line40.Y1 = 0.0!
        Me.Line40.Y2 = 0.3381889!
        '
        'Label12
        '
        Me.Label12.Height = 0.2937008!
        Me.Label12.HyperLink = Nothing
        Me.Label12.Left = 0.6043308!
        Me.Label12.Name = "Label12"
        Me.Label12.Style = "font-size: 12pt; font-weight: bold; text-align: center; vertical-align: middle"
        Me.Label12.Text = "合  計"
        Me.Label12.Top = 0.04448819!
        Me.Label12.Width = 5.838583!
        '
        'TxtTOTAL
        '
        Me.TxtTOTAL.CountNullValues = True
        Me.TxtTOTAL.DataField = "数量"
        Me.TxtTOTAL.Height = 0.2417322!
        Me.TxtTOTAL.Left = 6.567323!
        Me.TxtTOTAL.Name = "TxtTOTAL"
        Me.TxtTOTAL.Style = "text-align: center; vertical-align: middle; ddo-font-vertical: none"
        Me.TxtTOTAL.SummaryGroup = "GroupHeader1"
        Me.TxtTOTAL.SummaryRunning = GrapeCity.ActiveReports.SectionReportModel.SummaryRunning.Group
        Me.TxtTOTAL.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal
        Me.TxtTOTAL.Text = "NNNN"
        Me.TxtTOTAL.Top = 0.04448819!
        Me.TxtTOTAL.Width = 0.6094489!
        '
        'MNTREP004
        '
        Me.MasterReport = False
        Me.PageSettings.DefaultPaperSize = False
        Me.PageSettings.Margins.Bottom = 0.3937008!
        Me.PageSettings.Margins.Left = 0.3937008!
        Me.PageSettings.Margins.Right = 0.3937008!
        Me.PageSettings.Margins.Top = 0.3937008!
        Me.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Portrait
        Me.PageSettings.PaperHeight = 11.69291!
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4
        Me.PageSettings.PaperWidth = 8.267716!
        Me.PrintWidth = 7.635434!
        Me.Sections.Add(Me.PageHeader1)
        Me.Sections.Add(Me.GroupHeader1)
        Me.Sections.Add(Me.Detail1)
        Me.Sections.Add(Me.GroupFooter1)
        Me.Sections.Add(Me.PageFooter1)
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; " & _
            "color: Black; font-family: ""MS UI Gothic""; ddo-char-set: 128", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: ""MS UI Gothic""; ddo-char-set: 12" & _
            "8", "Heading1", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: ""MS UI Goth" & _
            "ic""; ddo-char-set: 128", "Heading2", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"))
        CType(Me.TxtNO, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtPRODUCT_NM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtCLS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtSELIAL_NO, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtREMARKS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtQUANTITY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblKugiri, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtDESTINATION, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtDELIVERY_D, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtSENDER, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtCONTENT, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox14, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtORDER_NO, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label12, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtTOTAL, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Private WithEvents Line27 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line29 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line30 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line31 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line32 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line33 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line34 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line44 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents TxtNO As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtPRODUCT_NM As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtCLS As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtSELIAL_NO As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtREMARKS As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtQUANTITY As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents GroupHeader1 As GrapeCity.ActiveReports.SectionReportModel.GroupHeader
    Private WithEvents GroupFooter1 As GrapeCity.ActiveReports.SectionReportModel.GroupFooter
    Private WithEvents Line35 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line37 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line38 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line39 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line40 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Label12 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TxtTOTAL As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Line25 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label1 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label2 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Line1 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line2 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line3 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line4 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line5 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line6 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line7 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line9 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line10 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line12 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line13 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line15 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line16 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line8 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line14 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Label3 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label4 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label5 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label6 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TxtDESTINATION As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtDELIVERY_D As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtSENDER As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtCONTENT As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox14 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtORDER_NO As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Line19 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line20 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line21 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line22 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line23 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line24 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line41 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line42 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Line43 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Label7 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label8 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label9 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label10 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label11 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblKugiri As GrapeCity.ActiveReports.SectionReportModel.Label
End Class
