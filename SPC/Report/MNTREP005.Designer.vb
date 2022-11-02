<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class MNTREP005
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MNTREP005))
        Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
        Me.lblTitle = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label35 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail()
        Me.Label1 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblHallName = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblJyusyo = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblHallTel1 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblHallTel2 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblHallTel4 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblAttention = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtTBOXID = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtHallName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtJyusyo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtHallTel1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtHallTel2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtHallTel4 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtSystem = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblSystem = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtVer = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblVer = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtTboxTel = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblTboxTel = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtUnyobi = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblUnyobi = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtHallCD = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblHallCD = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.lblSyusinbi = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox8 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label8 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label2 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtYoto1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtYoto3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtYoto4 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblHallTel3 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtHallTel3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtYoto2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtHallTel5 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblHallTel5 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtTeikei1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtTeikei2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtTeikei3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtTeikei4 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtTenpo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtMDNSet = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblMDNSet = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtOyaMDNCnt = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblOyaMDNCnt = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtMDNCnt = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblMDNCnt = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtChild1MDNCnt = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblChild1MDNCnt = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtChild2MDNCnt = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblChild2MDNCnt = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtAttention = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.lblPanel = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label3 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label4 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label5 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label6 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox4 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox5 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox6 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox7 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label7 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox9 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox10 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label9 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label10 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label11 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox11 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox12 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label12 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox13 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label13 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox14 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label14 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label15 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox15 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label16 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox16 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label17 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox17 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox18 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label18 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox19 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label19 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox20 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label20 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label21 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox21 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label22 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox22 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label23 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox23 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox24 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label24 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox25 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox26 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label25 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox27 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label26 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox28 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label27 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox29 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label28 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox30 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox31 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label29 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label30 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label31 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox32 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TextBox33 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label32 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label33 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TextBox34 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtSyusinbi = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label34 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.txtMDN = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
        Me.lblPrtdate = New GrapeCity.ActiveReports.SectionReportModel.Label()
        CType(Me.lblTitle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label35, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblHallName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblJyusyo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblHallTel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblHallTel2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblHallTel4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblAttention, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTBOXID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtHallName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtJyusyo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtHallTel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtHallTel2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtHallTel4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSystem, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblSystem, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtVer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblVer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTboxTel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTboxTel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtUnyobi, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblUnyobi, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtHallCD, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblHallCD, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblSyusinbi, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtYoto1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtYoto3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtYoto4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblHallTel3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtHallTel3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtYoto2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtHallTel5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblHallTel5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTeikei1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTeikei2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTeikei3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTeikei4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtTenpo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtMDNSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblMDNSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtOyaMDNCnt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblOyaMDNCnt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtMDNCnt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblMDNCnt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtChild1MDNCnt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblChild1MDNCnt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtChild2MDNCnt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblChild2MDNCnt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtAttention, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox12, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label12, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox13, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label13, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox14, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label14, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label15, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox15, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label16, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox16, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label17, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox17, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox18, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label18, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox19, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label19, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox20, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label20, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label21, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox21, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label22, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox22, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label23, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox23, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox24, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label24, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox25, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox26, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label25, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox27, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label26, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox28, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label27, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox29, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label28, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox30, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox31, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label29, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label30, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label31, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox32, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox33, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label32, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label33, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextBox34, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSyusinbi, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label34, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtMDN, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblPrtdate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'PageHeader1
        '
        Me.PageHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.lblTitle, Me.Label35, Me.lblPrtdate})
        Me.PageHeader1.Height = 0.9895833!
        Me.PageHeader1.Name = "PageHeader1"
        '
        'lblTitle
        '
        Me.lblTitle.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.lblTitle.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.lblTitle.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.lblTitle.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.lblTitle.Height = 0.314567!
        Me.lblTitle.HyperLink = Nothing
        Me.lblTitle.Left = 0.1358268!
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 20.25pt"
        Me.lblTitle.Text = "ホール詳細データ"
        Me.lblTitle.Top = 0.3728347!
        Me.lblTitle.Width = 2.781103!
        '
        'Label35
        '
        Me.Label35.Height = 0.1582677!
        Me.Label35.HyperLink = Nothing
        Me.Label35.Left = 8.728347!
        Me.Label35.Name = "Label35"
        Me.Label35.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.Label35.Text = "印刷日時："
        Me.Label35.Top = 0.5291339!
        Me.Label35.Width = 0.7515755!
        '
        'Detail1
        '
        Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.Label1, Me.lblHallName, Me.lblJyusyo, Me.lblHallTel1, Me.lblHallTel2, Me.lblHallTel4, Me.lblAttention, Me.txtTBOXID, Me.txtHallName, Me.txtJyusyo, Me.txtHallTel1, Me.txtHallTel2, Me.txtHallTel4, Me.TextBox1, Me.txtSystem, Me.lblSystem, Me.txtVer, Me.lblVer, Me.txtTboxTel, Me.lblTboxTel, Me.txtUnyobi, Me.lblUnyobi, Me.txtHallCD, Me.lblHallCD, Me.lblSyusinbi, Me.TextBox8, Me.Label8, Me.TextBox2, Me.Label2, Me.txtYoto1, Me.txtYoto3, Me.txtYoto4, Me.lblHallTel3, Me.txtHallTel3, Me.txtYoto2, Me.txtHallTel5, Me.lblHallTel5, Me.txtTeikei1, Me.txtTeikei2, Me.txtTeikei3, Me.txtTeikei4, Me.txtTenpo, Me.txtMDNSet, Me.lblMDNSet, Me.txtOyaMDNCnt, Me.lblOyaMDNCnt, Me.txtMDNCnt, Me.lblMDNCnt, Me.txtChild1MDNCnt, Me.lblChild1MDNCnt, Me.txtChild2MDNCnt, Me.lblChild2MDNCnt, Me.txtAttention, Me.lblPanel, Me.Label3, Me.Label4, Me.Label5, Me.Label6, Me.TextBox3, Me.TextBox4, Me.TextBox5, Me.TextBox6, Me.TextBox7, Me.Label7, Me.TextBox9, Me.TextBox10, Me.Label9, Me.Label10, Me.Label11, Me.TextBox11, Me.TextBox12, Me.Label12, Me.TextBox13, Me.Label13, Me.TextBox14, Me.Label14, Me.Label15, Me.TextBox15, Me.Label16, Me.TextBox16, Me.Label17, Me.TextBox17, Me.TextBox18, Me.Label18, Me.TextBox19, Me.Label19, Me.TextBox20, Me.Label20, Me.Label21, Me.TextBox21, Me.Label22, Me.TextBox22, Me.Label23, Me.TextBox23, Me.TextBox24, Me.Label24, Me.TextBox25, Me.TextBox26, Me.Label25, Me.TextBox27, Me.Label26, Me.TextBox28, Me.Label27, Me.TextBox29, Me.Label28, Me.TextBox30, Me.TextBox31, Me.Label29, Me.Label30, Me.Label31, Me.TextBox32, Me.TextBox33, Me.Label32, Me.Label33, Me.TextBox34, Me.txtSyusinbi, Me.Label34, Me.txtMDN})
        Me.Detail1.Height = 6.312601!
        Me.Detail1.Name = "Detail1"
        '
        'Label1
        '
        Me.Label1.Height = 0.1582677!
        Me.Label1.HyperLink = Nothing
        Me.Label1.Left = 0.1358261!
        Me.Label1.Name = "Label1"
        Me.Label1.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.Label1.Text = "ＴＢＯＸＩＤ"
        Me.Label1.Top = 0.06259855!
        Me.Label1.Width = 1.083465!
        '
        'lblHallName
        '
        Me.lblHallName.Height = 0.1582677!
        Me.lblHallName.HyperLink = Nothing
        Me.lblHallName.Left = 0.1358261!
        Me.lblHallName.Name = "lblHallName"
        Me.lblHallName.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.lblHallName.Text = "ホール名"
        Me.lblHallName.Top = 0.2917324!
        Me.lblHallName.Width = 1.083465!
        '
        'lblJyusyo
        '
        Me.lblJyusyo.Height = 0.1582677!
        Me.lblJyusyo.HyperLink = Nothing
        Me.lblJyusyo.Left = 0.1358261!
        Me.lblJyusyo.Name = "lblJyusyo"
        Me.lblJyusyo.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.lblJyusyo.Text = "住所"
        Me.lblJyusyo.Top = 0.5417324!
        Me.lblJyusyo.Width = 1.083465!
        '
        'lblHallTel1
        '
        Me.lblHallTel1.Height = 0.1582677!
        Me.lblHallTel1.HyperLink = Nothing
        Me.lblHallTel1.Left = 0.1358261!
        Me.lblHallTel1.Name = "lblHallTel1"
        Me.lblHallTel1.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.lblHallTel1.Text = "ホールＴＥＬ１"
        Me.lblHallTel1.Top = 0.7917324!
        Me.lblHallTel1.Width = 1.083465!
        '
        'lblHallTel2
        '
        Me.lblHallTel2.Height = 0.1582677!
        Me.lblHallTel2.HyperLink = Nothing
        Me.lblHallTel2.Left = 0.1358261!
        Me.lblHallTel2.Name = "lblHallTel2"
        Me.lblHallTel2.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.lblHallTel2.Text = "ホールＴＥＬ２"
        Me.lblHallTel2.Top = 1.020866!
        Me.lblHallTel2.Width = 1.083465!
        '
        'lblHallTel4
        '
        Me.lblHallTel4.Height = 0.1582677!
        Me.lblHallTel4.HyperLink = Nothing
        Me.lblHallTel4.Left = 0.1358261!
        Me.lblHallTel4.Name = "lblHallTel4"
        Me.lblHallTel4.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.lblHallTel4.Text = "ホールＴＥＬ４"
        Me.lblHallTel4.Top = 1.23937!
        Me.lblHallTel4.Width = 1.083465!
        '
        'lblAttention
        '
        Me.lblAttention.Height = 0.1582677!
        Me.lblAttention.HyperLink = Nothing
        Me.lblAttention.Left = 0.1358261!
        Me.lblAttention.Name = "lblAttention"
        Me.lblAttention.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.lblAttention.Text = "注意事項"
        Me.lblAttention.Top = 1.457874!
        Me.lblAttention.Width = 0.7291339!
        '
        'txtTBOXID
        '
        Me.txtTBOXID.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTBOXID.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTBOXID.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTBOXID.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTBOXID.DataField = "ＴＢＯＸＩＤ"
        Me.txtTBOXID.Height = 0.1771654!
        Me.txtTBOXID.Left = 1.146457!
        Me.txtTBOXID.Name = "txtTBOXID"
        Me.txtTBOXID.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtTBOXID.Text = "12345678901234567890" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.txtTBOXID.Top = 0.06259855!
        Me.txtTBOXID.Width = 1.427165!
        '
        'txtHallName
        '
        Me.txtHallName.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallName.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallName.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallName.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallName.DataField = "ホール名"
        Me.txtHallName.Height = 0.1771654!
        Me.txtHallName.Left = 1.146457!
        Me.txtHallName.Name = "txtHallName"
        Me.txtHallName.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtHallName.Text = "１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０"
        Me.txtHallName.Top = 0.2917324!
        Me.txtHallName.Width = 5.812205!
        '
        'txtJyusyo
        '
        Me.txtJyusyo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtJyusyo.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtJyusyo.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtJyusyo.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtJyusyo.CanGrow = False
        Me.txtJyusyo.DataField = "住所"
        Me.txtJyusyo.Height = 0.1771654!
        Me.txtJyusyo.Left = 1.146457!
        Me.txtJyusyo.Name = "txtJyusyo"
        Me.txtJyusyo.ShrinkToFit = True
        Me.txtJyusyo.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; vertical-align:" & _
    " middle; ddo-char-set: 1; ddo-shrink-to-fit: true"
        Me.txtJyusyo.Text = "１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０１２３４５６７８９００" & _
    "１２３４５６７８９００１２３４５６７８９０"
        Me.txtJyusyo.Top = 0.5417324!
        Me.txtJyusyo.Width = 9.551969!
        '
        'txtHallTel1
        '
        Me.txtHallTel1.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallTel1.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallTel1.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallTel1.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallTel1.DataField = "ホールＴＥＬ1"
        Me.txtHallTel1.Height = 0.1771654!
        Me.txtHallTel1.Left = 1.146457!
        Me.txtHallTel1.Name = "txtHallTel1"
        Me.txtHallTel1.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtHallTel1.Text = "123456789012345"
        Me.txtHallTel1.Top = 0.7917324!
        Me.txtHallTel1.Width = 1.281102!
        '
        'txtHallTel2
        '
        Me.txtHallTel2.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallTel2.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallTel2.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallTel2.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallTel2.DataField = "ホールＴＥＬ2"
        Me.txtHallTel2.Height = 0.1771654!
        Me.txtHallTel2.Left = 1.146457!
        Me.txtHallTel2.Name = "txtHallTel2"
        Me.txtHallTel2.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtHallTel2.Text = "123456789012345"
        Me.txtHallTel2.Top = 1.020866!
        Me.txtHallTel2.Width = 1.281102!
        '
        'txtHallTel4
        '
        Me.txtHallTel4.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallTel4.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallTel4.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallTel4.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallTel4.DataField = "ホールＴＥＬ4"
        Me.txtHallTel4.Height = 0.1771654!
        Me.txtHallTel4.Left = 1.146457!
        Me.txtHallTel4.Name = "txtHallTel4"
        Me.txtHallTel4.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtHallTel4.Text = "123456789012345"
        Me.txtHallTel4.Top = 1.23937!
        Me.txtHallTel4.Width = 1.281102!
        '
        'TextBox1
        '
        Me.TextBox1.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox1.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox1.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox1.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox1.DataField = "ＮＬ区分"
        Me.TextBox1.Height = 0.1771654!
        Me.TextBox1.Left = 2.627165!
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: cen" & _
    "ter"
        Me.TextBox1.Text = "X"
        Me.TextBox1.Top = 0.06259855!
        Me.TextBox1.Width = 0.2397638!
        '
        'txtSystem
        '
        Me.txtSystem.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtSystem.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtSystem.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtSystem.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtSystem.DataField = "システム"
        Me.txtSystem.Height = 0.1779528!
        Me.txtSystem.Left = 3.65748!
        Me.txtSystem.Name = "txtSystem"
        Me.txtSystem.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtSystem.Text = "12345678901234567890" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.txtSystem.Top = 0.06259855!
        Me.txtSystem.Width = 1.490157!
        '
        'lblSystem
        '
        Me.lblSystem.Height = 0.1582677!
        Me.lblSystem.HyperLink = Nothing
        Me.lblSystem.Left = 2.927953!
        Me.lblSystem.Name = "lblSystem"
        Me.lblSystem.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.lblSystem.Text = "システム"
        Me.lblSystem.Top = 0.06259855!
        Me.lblSystem.Width = 0.6775589!
        '
        'txtVer
        '
        Me.txtVer.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtVer.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtVer.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtVer.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtVer.DataField = "ＶＥＲ"
        Me.txtVer.Height = 0.1771654!
        Me.txtVer.Left = 5.814568!
        Me.txtVer.Name = "txtVer"
        Me.txtVer.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtVer.Text = "1234567890"
        Me.txtVer.Top = 0.06259855!
        Me.txtVer.Width = 0.8011811!
        '
        'lblVer
        '
        Me.lblVer.Height = 0.1582677!
        Me.lblVer.HyperLink = Nothing
        Me.lblVer.Left = 5.272835!
        Me.lblVer.Name = "lblVer"
        Me.lblVer.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.lblVer.Text = "ＶＥＲ"
        Me.lblVer.Top = 0.06259855!
        Me.lblVer.Width = 0.468504!
        '
        'txtTboxTel
        '
        Me.txtTboxTel.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTboxTel.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTboxTel.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTboxTel.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTboxTel.DataField = "ＴＢＯＸＴＥＬ"
        Me.txtTboxTel.Height = 0.1771654!
        Me.txtTboxTel.Left = 7.823623!
        Me.txtTboxTel.Name = "txtTboxTel"
        Me.txtTboxTel.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtTboxTel.Text = "123456789012345"
        Me.txtTboxTel.Top = 0.06259855!
        Me.txtTboxTel.Width = 1.103543!
        '
        'lblTboxTel
        '
        Me.lblTboxTel.Height = 0.1582677!
        Me.lblTboxTel.HyperLink = Nothing
        Me.lblTboxTel.Left = 6.687795!
        Me.lblTboxTel.Name = "lblTboxTel"
        Me.lblTboxTel.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.lblTboxTel.Text = "ＴＢＯＸＴＥＬ"
        Me.lblTboxTel.Top = 0.06259855!
        Me.lblTboxTel.Width = 1.083857!
        '
        'txtUnyobi
        '
        Me.txtUnyobi.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtUnyobi.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtUnyobi.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtUnyobi.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtUnyobi.DataField = "運用開始日"
        Me.txtUnyobi.Height = 0.1771654!
        Me.txtUnyobi.Left = 9.864962!
        Me.txtUnyobi.Name = "txtUnyobi"
        Me.txtUnyobi.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtUnyobi.Text = "9999/99/99"
        Me.txtUnyobi.Top = 0.06259855!
        Me.txtUnyobi.Width = 0.8334646!
        '
        'lblUnyobi
        '
        Me.lblUnyobi.Height = 0.1582677!
        Me.lblUnyobi.HyperLink = Nothing
        Me.lblUnyobi.Left = 8.99016!
        Me.lblUnyobi.Name = "lblUnyobi"
        Me.lblUnyobi.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.lblUnyobi.Text = "運用開始日"
        Me.lblUnyobi.Top = 0.06259855!
        Me.lblUnyobi.Width = 0.8019657!
        '
        'txtHallCD
        '
        Me.txtHallCD.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallCD.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallCD.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallCD.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallCD.DataField = "ホールコード"
        Me.txtHallCD.Height = 0.1771654!
        Me.txtHallCD.Left = 8.156693!
        Me.txtHallCD.Name = "txtHallCD"
        Me.txtHallCD.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtHallCD.Text = "12345678"
        Me.txtHallCD.Top = 0.2917324!
        Me.txtHallCD.Width = 0.6874016!
        '
        'lblHallCD
        '
        Me.lblHallCD.Height = 0.1582677!
        Me.lblHallCD.HyperLink = Nothing
        Me.lblHallCD.Left = 7.135432!
        Me.lblHallCD.Name = "lblHallCD"
        Me.lblHallCD.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.lblHallCD.Text = "ホールコード"
        Me.lblHallCD.Top = 0.2917324!
        Me.lblHallCD.Width = 0.9484252!
        '
        'lblSyusinbi
        '
        Me.lblSyusinbi.Height = 0.1582677!
        Me.lblSyusinbi.HyperLink = Nothing
        Me.lblSyusinbi.Left = 8.990158!
        Me.lblSyusinbi.Name = "lblSyusinbi"
        Me.lblSyusinbi.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.lblSyusinbi.Text = "集信日"
        Me.lblSyusinbi.Top = 0.2917324!
        Me.lblSyusinbi.Width = 0.8019657!
        '
        'TextBox8
        '
        Me.TextBox8.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox8.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox8.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox8.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox8.DataField = "ＥＷ区分"
        Me.TextBox8.Height = 0.1771654!
        Me.TextBox8.Left = 3.21929!
        Me.TextBox8.Name = "TextBox8"
        Me.TextBox8.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: cen" & _
    "ter"
        Me.TextBox8.Text = "E"
        Me.TextBox8.Top = 0.7917324!
        Me.TextBox8.Width = 0.2602362!
        '
        'Label8
        '
        Me.Label8.Height = 0.1582677!
        Me.Label8.HyperLink = Nothing
        Me.Label8.Left = 2.489763!
        Me.Label8.Name = "Label8"
        Me.Label8.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.Label8.Text = "ＥＷ区分"
        Me.Label8.Top = 0.7917321!
        Me.Label8.Width = 0.6566929!
        '
        'TextBox2
        '
        Me.TextBox2.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox2.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox2.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox2.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox2.DataField = "ＦＡＸ"
        Me.TextBox2.Height = 0.1771654!
        Me.TextBox2.Left = 4.510236!
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: lef" & _
    "t"
        Me.TextBox2.Text = "123456789012345"
        Me.TextBox2.Top = 0.7917324!
        Me.TextBox2.Width = 1.198425!
        '
        'Label2
        '
        Me.Label2.Height = 0.1582677!
        Me.Label2.HyperLink = Nothing
        Me.Label2.Left = 3.780708!
        Me.Label2.Name = "Label2"
        Me.Label2.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.Label2.Text = "ＦＡＸ"
        Me.Label2.Top = 0.7917324!
        Me.Label2.Width = 0.6566929!
        '
        'txtYoto1
        '
        Me.txtYoto1.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtYoto1.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtYoto1.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtYoto1.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtYoto1.DataField = "用途内容1"
        Me.txtYoto1.Height = 0.1771654!
        Me.txtYoto1.Left = 2.489763!
        Me.txtYoto1.Name = "txtYoto1"
        Me.txtYoto1.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtYoto1.Text = "１２３４５６７８９０１２３４５６７８９０"
        Me.txtYoto1.Top = 1.020866!
        Me.txtYoto1.Width = 2.874803!
        '
        'txtYoto3
        '
        Me.txtYoto3.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtYoto3.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtYoto3.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtYoto3.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtYoto3.DataField = "用途内容3"
        Me.txtYoto3.Height = 0.1771654!
        Me.txtYoto3.Left = 2.489763!
        Me.txtYoto3.Name = "txtYoto3"
        Me.txtYoto3.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtYoto3.Text = "１２３４５６７８９０１２３４５６７８９０"
        Me.txtYoto3.Top = 1.23937!
        Me.txtYoto3.Width = 2.874803!
        '
        'txtYoto4
        '
        Me.txtYoto4.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtYoto4.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtYoto4.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtYoto4.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtYoto4.DataField = "用途内容4"
        Me.txtYoto4.Height = 0.1771654!
        Me.txtYoto4.Left = 7.823622!
        Me.txtYoto4.Name = "txtYoto4"
        Me.txtYoto4.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtYoto4.Text = "１２３４５６７８９０１２３４５６７８９０"
        Me.txtYoto4.Top = 1.23937!
        Me.txtYoto4.Width = 2.874803!
        '
        'lblHallTel3
        '
        Me.lblHallTel3.Height = 0.1582677!
        Me.lblHallTel3.HyperLink = Nothing
        Me.lblHallTel3.Left = 5.416928!
        Me.lblHallTel3.Name = "lblHallTel3"
        Me.lblHallTel3.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.lblHallTel3.Text = "ホールＴＥＬ３"
        Me.lblHallTel3.Top = 1.020866!
        Me.lblHallTel3.Width = 1.146063!
        '
        'txtHallTel3
        '
        Me.txtHallTel3.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallTel3.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallTel3.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallTel3.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallTel3.DataField = "ホールＴＥＬ3"
        Me.txtHallTel3.Height = 0.1771654!
        Me.txtHallTel3.Left = 6.615747!
        Me.txtHallTel3.Name = "txtHallTel3"
        Me.txtHallTel3.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtHallTel3.Text = "123456789012345"
        Me.txtHallTel3.Top = 1.020866!
        Me.txtHallTel3.Width = 1.155906!
        '
        'txtYoto2
        '
        Me.txtYoto2.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtYoto2.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtYoto2.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtYoto2.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtYoto2.DataField = "用途内容2"
        Me.txtYoto2.Height = 0.1771654!
        Me.txtYoto2.Left = 7.823622!
        Me.txtYoto2.Name = "txtYoto2"
        Me.txtYoto2.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtYoto2.Text = "１２３４５６７８９０１２３４５６７８９０"
        Me.txtYoto2.Top = 1.020866!
        Me.txtYoto2.Width = 2.874803!
        '
        'txtHallTel5
        '
        Me.txtHallTel5.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallTel5.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallTel5.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallTel5.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtHallTel5.DataField = "ホールＴＥＬ5"
        Me.txtHallTel5.Height = 0.1771654!
        Me.txtHallTel5.Left = 6.615747!
        Me.txtHallTel5.Name = "txtHallTel5"
        Me.txtHallTel5.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtHallTel5.Text = "123456789012345"
        Me.txtHallTel5.Top = 1.23937!
        Me.txtHallTel5.Width = 1.155906!
        '
        'lblHallTel5
        '
        Me.lblHallTel5.Height = 0.1582677!
        Me.lblHallTel5.HyperLink = Nothing
        Me.lblHallTel5.Left = 5.416928!
        Me.lblHallTel5.Name = "lblHallTel5"
        Me.lblHallTel5.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.lblHallTel5.Text = "ホールＴＥＬ５"
        Me.lblHallTel5.Top = 1.23937!
        Me.lblHallTel5.Width = 1.146063!
        '
        'txtTeikei1
        '
        Me.txtTeikei1.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTeikei1.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTeikei1.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTeikei1.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTeikei1.DataField = "定型文1"
        Me.txtTeikei1.Height = 0.1771654!
        Me.txtTeikei1.Left = 0.9059056!
        Me.txtTeikei1.Name = "txtTeikei1"
        Me.txtTeikei1.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtTeikei1.Text = "１２３４５６７８９０１２３４５６７８９０"
        Me.txtTeikei1.Top = 1.457874!
        Me.txtTeikei1.Width = 2.874803!
        '
        'txtTeikei2
        '
        Me.txtTeikei2.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTeikei2.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTeikei2.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTeikei2.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTeikei2.DataField = "定型文2"
        Me.txtTeikei2.Height = 0.1771654!
        Me.txtTeikei2.Left = 3.864173!
        Me.txtTeikei2.Name = "txtTeikei2"
        Me.txtTeikei2.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtTeikei2.Text = "１２３４５６７８９０１２３４５６７８９０"
        Me.txtTeikei2.Top = 1.457874!
        Me.txtTeikei2.Width = 2.874803!
        '
        'txtTeikei3
        '
        Me.txtTeikei3.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTeikei3.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTeikei3.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTeikei3.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTeikei3.DataField = "定型文3"
        Me.txtTeikei3.Height = 0.1771654!
        Me.txtTeikei3.Left = 0.9059047!
        Me.txtTeikei3.Name = "txtTeikei3"
        Me.txtTeikei3.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtTeikei3.Text = "１２３４５６７８９０１２３４５６７８９０"
        Me.txtTeikei3.Top = 1.687008!
        Me.txtTeikei3.Width = 2.874803!
        '
        'txtTeikei4
        '
        Me.txtTeikei4.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTeikei4.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTeikei4.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTeikei4.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTeikei4.DataField = "定型文4"
        Me.txtTeikei4.Height = 0.1771654!
        Me.txtTeikei4.Left = 3.864172!
        Me.txtTeikei4.Name = "txtTeikei4"
        Me.txtTeikei4.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtTeikei4.Text = "１２３４５６７８９０１２３４５６７８９０"
        Me.txtTeikei4.Top = 1.687008!
        Me.txtTeikei4.Width = 2.874803!
        '
        'txtTenpo
        '
        Me.txtTenpo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTenpo.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTenpo.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTenpo.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtTenpo.DataField = "店舗種別"
        Me.txtTenpo.Height = 0.1771654!
        Me.txtTenpo.Left = 6.79252!
        Me.txtTenpo.Name = "txtTenpo"
        Me.txtTenpo.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: cen" & _
    "ter"
        Me.txtTenpo.Text = "単独店"
        Me.txtTenpo.Top = 1.457874!
        Me.txtTenpo.Width = 0.5417323!
        '
        'txtMDNSet
        '
        Me.txtMDNSet.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtMDNSet.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtMDNSet.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtMDNSet.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtMDNSet.DataField = "ＭＤＮ設置"
        Me.txtMDNSet.Height = 0.1771654!
        Me.txtMDNSet.Left = 8.573229!
        Me.txtMDNSet.Name = "txtMDNSet"
        Me.txtMDNSet.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: cen" & _
    "ter"
        Me.txtMDNSet.Text = "無"
        Me.txtMDNSet.Top = 1.457874!
        Me.txtMDNSet.Width = 0.4169292!
        '
        'lblMDNSet
        '
        Me.lblMDNSet.Height = 0.1582677!
        Me.lblMDNSet.HyperLink = Nothing
        Me.lblMDNSet.Left = 7.541732!
        Me.lblMDNSet.Name = "lblMDNSet"
        Me.lblMDNSet.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.lblMDNSet.Text = "ＭＤＮ設置"
        Me.lblMDNSet.Top = 1.457874!
        Me.lblMDNSet.Width = 0.9476376!
        '
        'txtOyaMDNCnt
        '
        Me.txtOyaMDNCnt.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtOyaMDNCnt.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtOyaMDNCnt.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtOyaMDNCnt.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtOyaMDNCnt.DataField = "親ＭＤＮ台数"
        Me.txtOyaMDNCnt.Height = 0.1771654!
        Me.txtOyaMDNCnt.Left = 8.573232!
        Me.txtOyaMDNCnt.Name = "txtOyaMDNCnt"
        Me.txtOyaMDNCnt.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: rig" & _
    "ht"
        Me.txtOyaMDNCnt.Text = "0"
        Me.txtOyaMDNCnt.Top = 1.687008!
        Me.txtOyaMDNCnt.Width = 0.4169292!
        '
        'lblOyaMDNCnt
        '
        Me.lblOyaMDNCnt.Height = 0.1582677!
        Me.lblOyaMDNCnt.HyperLink = Nothing
        Me.lblOyaMDNCnt.Left = 7.541732!
        Me.lblOyaMDNCnt.Name = "lblOyaMDNCnt"
        Me.lblOyaMDNCnt.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.lblOyaMDNCnt.Text = "親ＭＤＮ台数"
        Me.lblOyaMDNCnt.Top = 1.687008!
        Me.lblOyaMDNCnt.Width = 0.9476375!
        '
        'txtMDNCnt
        '
        Me.txtMDNCnt.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtMDNCnt.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtMDNCnt.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtMDNCnt.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtMDNCnt.DataField = "ＭＤＮ台数"
        Me.txtMDNCnt.Height = 0.1771654!
        Me.txtMDNCnt.Left = 10.28149!
        Me.txtMDNCnt.Name = "txtMDNCnt"
        Me.txtMDNCnt.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: rig" & _
    "ht"
        Me.txtMDNCnt.Text = "0"
        Me.txtMDNCnt.Top = 1.457874!
        Me.txtMDNCnt.Width = 0.4169292!
        '
        'lblMDNCnt
        '
        Me.lblMDNCnt.Height = 0.1582677!
        Me.lblMDNCnt.HyperLink = Nothing
        Me.lblMDNCnt.Left = 9.072835!
        Me.lblMDNCnt.Name = "lblMDNCnt"
        Me.lblMDNCnt.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.lblMDNCnt.Text = "ＭＤＮ台数"
        Me.lblMDNCnt.Top = 1.457874!
        Me.lblMDNCnt.Width = 1.124799!
        '
        'txtChild1MDNCnt
        '
        Me.txtChild1MDNCnt.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtChild1MDNCnt.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtChild1MDNCnt.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtChild1MDNCnt.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtChild1MDNCnt.DataField = "子1ＭＤＮ台数"
        Me.txtChild1MDNCnt.Height = 0.1771654!
        Me.txtChild1MDNCnt.Left = 10.28149!
        Me.txtChild1MDNCnt.Name = "txtChild1MDNCnt"
        Me.txtChild1MDNCnt.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: rig" & _
    "ht"
        Me.txtChild1MDNCnt.Text = "0"
        Me.txtChild1MDNCnt.Top = 1.687008!
        Me.txtChild1MDNCnt.Width = 0.4169292!
        '
        'lblChild1MDNCnt
        '
        Me.lblChild1MDNCnt.Height = 0.1582677!
        Me.lblChild1MDNCnt.HyperLink = Nothing
        Me.lblChild1MDNCnt.Left = 9.072835!
        Me.lblChild1MDNCnt.Name = "lblChild1MDNCnt"
        Me.lblChild1MDNCnt.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.lblChild1MDNCnt.Text = "子１ＭＤＮ台数"
        Me.lblChild1MDNCnt.Top = 1.687008!
        Me.lblChild1MDNCnt.Width = 1.124799!
        '
        'txtChild2MDNCnt
        '
        Me.txtChild2MDNCnt.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtChild2MDNCnt.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtChild2MDNCnt.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtChild2MDNCnt.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtChild2MDNCnt.DataField = "子2ＭＤＮ台数"
        Me.txtChild2MDNCnt.Height = 0.1771654!
        Me.txtChild2MDNCnt.Left = 10.28149!
        Me.txtChild2MDNCnt.Name = "txtChild2MDNCnt"
        Me.txtChild2MDNCnt.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: rig" & _
    "ht"
        Me.txtChild2MDNCnt.Text = "0"
        Me.txtChild2MDNCnt.Top = 1.916142!
        Me.txtChild2MDNCnt.Width = 0.4169292!
        '
        'lblChild2MDNCnt
        '
        Me.lblChild2MDNCnt.Height = 0.1582677!
        Me.lblChild2MDNCnt.HyperLink = Nothing
        Me.lblChild2MDNCnt.Left = 9.072835!
        Me.lblChild2MDNCnt.Name = "lblChild2MDNCnt"
        Me.lblChild2MDNCnt.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.lblChild2MDNCnt.Text = "子２ＭＤＮ台数"
        Me.lblChild2MDNCnt.Top = 1.916142!
        Me.lblChild2MDNCnt.Width = 1.1248!
        '
        'txtAttention
        '
        Me.txtAttention.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtAttention.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtAttention.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtAttention.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtAttention.DataField = "注意事項"
        Me.txtAttention.Height = 1.68937!
        Me.txtAttention.Left = 0.9059047!
        Me.txtAttention.Name = "txtAttention"
        Me.txtAttention.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtAttention.Text = resources.GetString("txtAttention.Text")
        Me.txtAttention.Top = 1.916142!
        Me.txtAttention.Width = 5.833072!
        '
        'lblPanel
        '
        Me.lblPanel.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.lblPanel.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.lblPanel.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.lblPanel.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.lblPanel.Height = 0.9917326!
        Me.lblPanel.HyperLink = Nothing
        Me.lblPanel.Left = 0.1358268!
        Me.lblPanel.Name = "lblPanel"
        Me.lblPanel.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9pt"
        Me.lblPanel.Text = ""
        Me.lblPanel.Top = 3.709449!
        Me.lblPanel.Width = 10.5626!
        '
        'Label3
        '
        Me.Label3.Height = 0.1582677!
        Me.Label3.HyperLink = Nothing
        Me.Label3.Left = 0.2503937!
        Me.Label3.Name = "Label3"
        Me.Label3.Style = "font-family: ＭＳ ゴシック; font-size: 9pt"
        Me.Label3.Text = "ＴＢＯＸシリアル"
        Me.Label3.Top = 3.792127!
        Me.Label3.Width = 1.177559!
        '
        'Label4
        '
        Me.Label4.Height = 0.1582677!
        Me.Label4.HyperLink = Nothing
        Me.Label4.Left = 0.2503937!
        Me.Label4.Name = "Label4"
        Me.Label4.Style = "font-family: ＭＳ ゴシック; font-size: 9pt"
        Me.Label4.Text = "プリンタシリアル"
        Me.Label4.Top = 4.02126!
        Me.Label4.Width = 1.177559!
        '
        'Label5
        '
        Me.Label5.Height = 0.1582677!
        Me.Label5.HyperLink = Nothing
        Me.Label5.Left = 0.2503937!
        Me.Label5.Name = "Label5"
        Me.Label5.Style = "font-family: ＭＳ ゴシック; font-size: 9pt"
        Me.Label5.Text = "ＨＤＤ１シリアル"
        Me.Label5.Top = 4.229527!
        Me.Label5.Width = 1.177559!
        '
        'Label6
        '
        Me.Label6.Height = 0.1582677!
        Me.Label6.HyperLink = Nothing
        Me.Label6.Left = 0.2503937!
        Me.Label6.Name = "Label6"
        Me.Label6.Style = "font-family: ＭＳ ゴシック; font-size: 9pt"
        Me.Label6.Text = "ＨＤＤ３シリアル"
        Me.Label6.Top = 4.448426!
        Me.Label6.Width = 1.177559!
        '
        'TextBox3
        '
        Me.TextBox3.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox3.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox3.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox3.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox3.CanGrow = False
        Me.TextBox3.DataField = "ＴＢＯＸシリアル"
        Me.TextBox3.Height = 0.1771654!
        Me.TextBox3.Left = 1.469686!
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ShrinkToFit = True
        Me.TextBox3.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; ddo-char-set: 1" & _
    "; ddo-shrink-to-fit: true"
        Me.TextBox3.Text = "123456789012345678901234567890"
        Me.TextBox3.Top = 3.792126!
        Me.TextBox3.Width = 2.009843!
        '
        'TextBox4
        '
        Me.TextBox4.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox4.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox4.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox4.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox4.CanGrow = False
        Me.TextBox4.DataField = "プリンタシリアル"
        Me.TextBox4.Height = 0.1771654!
        Me.TextBox4.Left = 1.469686!
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.ShrinkToFit = True
        Me.TextBox4.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; ddo-char-set: 1" & _
    "; ddo-shrink-to-fit: true"
        Me.TextBox4.Text = "123456789012345678901234567890"
        Me.TextBox4.Top = 4.02126!
        Me.TextBox4.Width = 2.009843!
        '
        'TextBox5
        '
        Me.TextBox5.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox5.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox5.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox5.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox5.CanGrow = False
        Me.TextBox5.DataField = "ＨＤＤ1シリアル"
        Me.TextBox5.Height = 0.1771654!
        Me.TextBox5.Left = 1.469686!
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.ShrinkToFit = True
        Me.TextBox5.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; ddo-char-set: 1" & _
    "; ddo-shrink-to-fit: true"
        Me.TextBox5.Text = "123456789012345678901234567890"
        Me.TextBox5.Top = 4.229527!
        Me.TextBox5.Width = 2.009843!
        '
        'TextBox6
        '
        Me.TextBox6.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox6.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox6.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox6.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox6.CanGrow = False
        Me.TextBox6.DataField = "ＨＤＤ3シリアル"
        Me.TextBox6.Height = 0.1771654!
        Me.TextBox6.Left = 1.469686!
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.ShrinkToFit = True
        Me.TextBox6.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; ddo-char-set: 1" & _
    "; ddo-shrink-to-fit: true"
        Me.TextBox6.Text = "123456789012345678901234567890"
        Me.TextBox6.Top = 4.448425!
        Me.TextBox6.Width = 2.009843!
        '
        'TextBox7
        '
        Me.TextBox7.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox7.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox7.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox7.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox7.CanGrow = False
        Me.TextBox7.DataField = "ＣＲＴシリアル"
        Me.TextBox7.Height = 0.1771654!
        Me.TextBox7.Left = 4.896851!
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.ShrinkToFit = True
        Me.TextBox7.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; ddo-char-set: 1" & _
    "; ddo-shrink-to-fit: true"
        Me.TextBox7.Text = "123456789012345678901234567890"
        Me.TextBox7.Top = 4.02126!
        Me.TextBox7.Width = 2.009843!
        '
        'Label7
        '
        Me.Label7.Height = 0.1582677!
        Me.Label7.HyperLink = Nothing
        Me.Label7.Left = 3.657481!
        Me.Label7.Name = "Label7"
        Me.Label7.Style = "font-family: ＭＳ ゴシック; font-size: 9pt"
        Me.Label7.Text = "ＨＤＤ４シリアル"
        Me.Label7.Top = 4.448426!
        Me.Label7.Width = 1.177559!
        '
        'TextBox9
        '
        Me.TextBox9.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox9.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox9.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox9.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox9.CanGrow = False
        Me.TextBox9.DataField = "ＨＤＤ4シリアル"
        Me.TextBox9.Height = 0.1771654!
        Me.TextBox9.Left = 4.896851!
        Me.TextBox9.Name = "TextBox9"
        Me.TextBox9.ShrinkToFit = True
        Me.TextBox9.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; ddo-char-set: 1" & _
    "; ddo-shrink-to-fit: true"
        Me.TextBox9.Text = "123456789012345678901234567890"
        Me.TextBox9.Top = 4.448425!
        Me.TextBox9.Width = 2.009843!
        '
        'TextBox10
        '
        Me.TextBox10.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox10.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox10.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox10.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox10.CanGrow = False
        Me.TextBox10.DataField = "ＨＤＤ2シリアル"
        Me.TextBox10.Height = 0.1771654!
        Me.TextBox10.Left = 4.896851!
        Me.TextBox10.Name = "TextBox10"
        Me.TextBox10.ShrinkToFit = True
        Me.TextBox10.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; ddo-char-set: 1" & _
    "; ddo-shrink-to-fit: true"
        Me.TextBox10.Text = "123456789012345678901234567890"
        Me.TextBox10.Top = 4.229527!
        Me.TextBox10.Width = 2.009843!
        '
        'Label9
        '
        Me.Label9.Height = 0.1582677!
        Me.Label9.HyperLink = Nothing
        Me.Label9.Left = 3.657481!
        Me.Label9.Name = "Label9"
        Me.Label9.Style = "font-family: ＭＳ ゴシック; font-size: 9pt"
        Me.Label9.Text = "ＨＤＤ２シリアル"
        Me.Label9.Top = 4.229529!
        Me.Label9.Width = 1.177559!
        '
        'Label10
        '
        Me.Label10.Height = 0.1582677!
        Me.Label10.HyperLink = Nothing
        Me.Label10.Left = 3.657481!
        Me.Label10.Name = "Label10"
        Me.Label10.Style = "font-family: ＭＳ ゴシック; font-size: 9pt"
        Me.Label10.Text = "ＣＲＴシリアル"
        Me.Label10.Top = 4.021261!
        Me.Label10.Width = 1.177559!
        '
        'Label11
        '
        Me.Label11.Height = 0.1582677!
        Me.Label11.HyperLink = Nothing
        Me.Label11.Left = 3.657481!
        Me.Label11.Name = "Label11"
        Me.Label11.Style = "font-family: ＭＳ ゴシック; font-size: 9pt"
        Me.Label11.Text = "操作盤シリアル"
        Me.Label11.Top = 3.792127!
        Me.Label11.Width = 1.177559!
        '
        'TextBox11
        '
        Me.TextBox11.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox11.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox11.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox11.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox11.CanGrow = False
        Me.TextBox11.DataField = "操作盤シリアル"
        Me.TextBox11.Height = 0.1771654!
        Me.TextBox11.Left = 4.896851!
        Me.TextBox11.Name = "TextBox11"
        Me.TextBox11.ShrinkToFit = True
        Me.TextBox11.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; ddo-char-set: 1" & _
    "; ddo-shrink-to-fit: true"
        Me.TextBox11.Text = "123456789012345678901234567890"
        Me.TextBox11.Top = 3.792126!
        Me.TextBox11.Width = 2.009843!
        '
        'TextBox12
        '
        Me.TextBox12.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox12.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox12.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox12.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox12.CanGrow = False
        Me.TextBox12.DataField = "ＵＰＳシリアル"
        Me.TextBox12.Height = 0.1771654!
        Me.TextBox12.Left = 8.322836!
        Me.TextBox12.Name = "TextBox12"
        Me.TextBox12.ShrinkToFit = True
        Me.TextBox12.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; ddo-char-set: 1" & _
    "; ddo-shrink-to-fit: true"
        Me.TextBox12.Text = "123456789012345678901234567890"
        Me.TextBox12.Top = 3.792127!
        Me.TextBox12.Width = 2.009843!
        '
        'Label12
        '
        Me.Label12.Height = 0.1582677!
        Me.Label12.HyperLink = Nothing
        Me.Label12.Left = 7.010632!
        Me.Label12.Name = "Label12"
        Me.Label12.Style = "font-family: ＭＳ ゴシック; font-size: 9pt; text-align: right"
        Me.Label12.Text = "ＵＰＳシリアル"
        Me.Label12.Top = 3.792127!
        Me.Label12.Width = 1.23937!
        '
        'TextBox13
        '
        Me.TextBox13.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox13.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox13.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox13.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox13.CanGrow = False
        Me.TextBox13.DataField = "ＳＣ"
        Me.TextBox13.Height = 0.1771654!
        Me.TextBox13.Left = 8.322837!
        Me.TextBox13.Name = "TextBox13"
        Me.TextBox13.ShrinkToFit = True
        Me.TextBox13.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: rig" & _
    "ht; ddo-char-set: 1; ddo-shrink-to-fit: true"
        Me.TextBox13.Text = "999"
        Me.TextBox13.Top = 4.021261!
        Me.TextBox13.Width = 0.3326772!
        '
        'Label13
        '
        Me.Label13.Height = 0.1582677!
        Me.Label13.HyperLink = Nothing
        Me.Label13.Left = 7.823623!
        Me.Label13.Name = "Label13"
        Me.Label13.Style = "font-family: ＭＳ ゴシック; font-size: 9pt; text-align: right"
        Me.Label13.Text = "ＳＣ"
        Me.Label13.Top = 4.02126!
        Me.Label13.Width = 0.427561!
        '
        'TextBox14
        '
        Me.TextBox14.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox14.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox14.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox14.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox14.CanGrow = False
        Me.TextBox14.DataField = "券売機"
        Me.TextBox14.Height = 0.1771654!
        Me.TextBox14.Left = 10.0!
        Me.TextBox14.Name = "TextBox14"
        Me.TextBox14.ShrinkToFit = True
        Me.TextBox14.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: rig" & _
    "ht; ddo-char-set: 1; ddo-shrink-to-fit: true"
        Me.TextBox14.Text = "999"
        Me.TextBox14.Top = 4.021261!
        Me.TextBox14.Width = 0.3326772!
        '
        'Label14
        '
        Me.Label14.Height = 0.1582677!
        Me.Label14.HyperLink = Nothing
        Me.Label14.Left = 9.281891!
        Me.Label14.Name = "Label14"
        Me.Label14.Style = "font-family: ＭＳ ゴシック; font-size: 9pt; text-align: right"
        Me.Label14.Text = "券売機"
        Me.Label14.Top = 4.02126!
        Me.Label14.Width = 0.6464558!
        '
        'Label15
        '
        Me.Label15.Height = 0.1582677!
        Me.Label15.HyperLink = Nothing
        Me.Label15.Left = 7.823621!
        Me.Label15.Name = "Label15"
        Me.Label15.Style = "font-family: ＭＳ ゴシック; font-size: 9pt; text-align: right"
        Me.Label15.Text = "ＣＣ"
        Me.Label15.Top = 4.229527!
        Me.Label15.Width = 0.4275611!
        '
        'TextBox15
        '
        Me.TextBox15.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox15.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox15.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox15.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox15.CanGrow = False
        Me.TextBox15.DataField = "精算機"
        Me.TextBox15.Height = 0.1771654!
        Me.TextBox15.Left = 10.0!
        Me.TextBox15.Name = "TextBox15"
        Me.TextBox15.ShrinkToFit = True
        Me.TextBox15.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: rig" & _
    "ht; ddo-char-set: 1; ddo-shrink-to-fit: true"
        Me.TextBox15.Text = "999"
        Me.TextBox15.Top = 4.229529!
        Me.TextBox15.Width = 0.3326772!
        '
        'Label16
        '
        Me.Label16.Height = 0.1582677!
        Me.Label16.HyperLink = Nothing
        Me.Label16.Left = 9.281893!
        Me.Label16.Name = "Label16"
        Me.Label16.Style = "font-family: ＭＳ ゴシック; font-size: 9pt; text-align: right"
        Me.Label16.Text = "精算機"
        Me.Label16.Top = 4.229527!
        Me.Label16.Width = 0.6464558!
        '
        'TextBox16
        '
        Me.TextBox16.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox16.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox16.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox16.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox16.CanGrow = False
        Me.TextBox16.DataField = "ＣＣ"
        Me.TextBox16.Height = 0.1771654!
        Me.TextBox16.Left = 8.322837!
        Me.TextBox16.Name = "TextBox16"
        Me.TextBox16.ShrinkToFit = True
        Me.TextBox16.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: rig" & _
    "ht; ddo-char-set: 1; ddo-shrink-to-fit: true"
        Me.TextBox16.Text = "999"
        Me.TextBox16.Top = 4.229529!
        Me.TextBox16.Width = 0.3326772!
        '
        'Label17
        '
        Me.Label17.Height = 0.1582677!
        Me.Label17.HyperLink = Nothing
        Me.Label17.Left = 7.62559!
        Me.Label17.Name = "Label17"
        Me.Label17.Style = "font-family: ＭＳ ゴシック; font-size: 9pt; text-align: right"
        Me.Label17.Text = "サンド"
        Me.Label17.Top = 4.448426!
        Me.Label17.Width = 0.6255918!
        '
        'TextBox17
        '
        Me.TextBox17.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox17.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox17.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox17.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox17.CanGrow = False
        Me.TextBox17.DataField = "サンド"
        Me.TextBox17.Height = 0.1771654!
        Me.TextBox17.Left = 8.322838!
        Me.TextBox17.Name = "TextBox17"
        Me.TextBox17.ShrinkToFit = True
        Me.TextBox17.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: rig" & _
    "ht; ddo-char-set: 1; ddo-shrink-to-fit: true"
        Me.TextBox17.Text = "999"
        Me.TextBox17.Top = 4.448427!
        Me.TextBox17.Width = 0.3326772!
        '
        'TextBox18
        '
        Me.TextBox18.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox18.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox18.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox18.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox18.DataField = "担当営業部"
        Me.TextBox18.Height = 0.1771654!
        Me.TextBox18.Left = 4.875985!
        Me.TextBox18.Name = "TextBox18"
        Me.TextBox18.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.TextBox18.Text = "１２３４５６７８９０１２３４５６７８９０"
        Me.TextBox18.Top = 4.792127!
        Me.TextBox18.Width = 3.697244!
        '
        'Label18
        '
        Me.Label18.Height = 0.1582677!
        Me.Label18.HyperLink = Nothing
        Me.Label18.Left = 4.105906!
        Me.Label18.Name = "Label18"
        Me.Label18.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.Label18.Text = "担当営業部"
        Me.Label18.Top = 4.792127!
        Me.Label18.Width = 0.729134!
        '
        'TextBox19
        '
        Me.TextBox19.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox19.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox19.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox19.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox19.DataField = "ＴＥＬ(担当営業部)"
        Me.TextBox19.Height = 0.1771654!
        Me.TextBox19.Left = 9.520867!
        Me.TextBox19.Name = "TextBox19"
        Me.TextBox19.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.TextBox19.Text = "123456789012345"
        Me.TextBox19.Top = 4.792127!
        Me.TextBox19.Width = 1.177559!
        '
        'Label19
        '
        Me.Label19.Height = 0.1582677!
        Me.Label19.HyperLink = Nothing
        Me.Label19.Left = 8.750788!
        Me.Label19.Name = "Label19"
        Me.Label19.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.Label19.Text = "ＴＥＬ"
        Me.Label19.Top = 4.792127!
        Me.Label19.Width = 0.729134!
        '
        'TextBox20
        '
        Me.TextBox20.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox20.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox20.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox20.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox20.CanGrow = False
        Me.TextBox20.DataField = "保担ＳＣ"
        Me.TextBox20.Height = 0.1771654!
        Me.TextBox20.Left = 0.9059054!
        Me.TextBox20.Name = "TextBox20"
        Me.TextBox20.ShrinkToFit = True
        Me.TextBox20.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; vertical-align:" & _
    " middle; ddo-char-set: 1; ddo-shrink-to-fit: true"
        Me.TextBox20.Text = "１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０"
        Me.TextBox20.Top = 5.010631!
        Me.TextBox20.Width = 2.958268!
        '
        'Label20
        '
        Me.Label20.Height = 0.1582677!
        Me.Label20.HyperLink = Nothing
        Me.Label20.Left = 0.1358279!
        Me.Label20.Name = "Label20"
        Me.Label20.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.Label20.Text = "保担ＳＣ"
        Me.Label20.Top = 5.010631!
        Me.Label20.Width = 0.729134!
        '
        'Label21
        '
        Me.Label21.Height = 0.1582677!
        Me.Label21.HyperLink = Nothing
        Me.Label21.Left = 4.105907!
        Me.Label21.Name = "Label21"
        Me.Label21.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.Label21.Text = "ＬＡＮ担当"
        Me.Label21.Top = 5.01063!
        Me.Label21.Width = 0.729134!
        '
        'TextBox21
        '
        Me.TextBox21.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox21.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox21.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox21.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox21.DataField = "ＬＡＮ担当ＩＤ"
        Me.TextBox21.Height = 0.1771654!
        Me.TextBox21.Left = 4.875985!
        Me.TextBox21.Name = "TextBox21"
        Me.TextBox21.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: lef" & _
    "t"
        Me.TextBox21.Text = "12345678"
        Me.TextBox21.Top = 5.010631!
        Me.TextBox21.Width = 0.6665354!
        '
        'Label22
        '
        Me.Label22.Height = 0.1582677!
        Me.Label22.HyperLink = Nothing
        Me.Label22.Left = 8.75079!
        Me.Label22.Name = "Label22"
        Me.Label22.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.Label22.Text = "ＴＥＬ"
        Me.Label22.Top = 5.010631!
        Me.Label22.Width = 0.729134!
        '
        'TextBox22
        '
        Me.TextBox22.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox22.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox22.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox22.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox22.DataField = "ＴＥＬ(保担ＳＣ)"
        Me.TextBox22.Height = 0.1771654!
        Me.TextBox22.Left = 9.520864!
        Me.TextBox22.Name = "TextBox22"
        Me.TextBox22.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.TextBox22.Text = "123456789012345"
        Me.TextBox22.Top = 5.010631!
        Me.TextBox22.Width = 1.177559!
        '
        'Label23
        '
        Me.Label23.Height = 0.1582677!
        Me.Label23.HyperLink = Nothing
        Me.Label23.Left = 8.750792!
        Me.Label23.Name = "Label23"
        Me.Label23.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.Label23.Text = "ＦＡＸ"
        Me.Label23.Top = 5.229134!
        Me.Label23.Width = 0.729134!
        '
        'TextBox23
        '
        Me.TextBox23.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox23.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox23.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox23.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox23.DataField = "ＦＡＸ(保担ＳＣ)"
        Me.TextBox23.Height = 0.1771654!
        Me.TextBox23.Left = 9.520867!
        Me.TextBox23.Name = "TextBox23"
        Me.TextBox23.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.TextBox23.Text = "123456789012345"
        Me.TextBox23.Top = 5.229134!
        Me.TextBox23.Width = 1.177559!
        '
        'TextBox24
        '
        Me.TextBox24.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox24.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox24.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox24.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox24.CanGrow = False
        Me.TextBox24.DataField = "住所(保担ＳＣ)"
        Me.TextBox24.Height = 0.1771654!
        Me.TextBox24.Left = 0.9059063!
        Me.TextBox24.Name = "TextBox24"
        Me.TextBox24.ShrinkToFit = True
        Me.TextBox24.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; vertical-align:" & _
    " middle; ddo-char-set: 1; ddo-shrink-to-fit: true"
        Me.TextBox24.Text = "１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０"
        Me.TextBox24.Top = 5.229135!
        Me.TextBox24.Width = 7.667323!
        '
        'Label24
        '
        Me.Label24.Height = 0.1582677!
        Me.Label24.HyperLink = Nothing
        Me.Label24.Left = 0.1358287!
        Me.Label24.Name = "Label24"
        Me.Label24.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.Label24.Text = "住所"
        Me.Label24.Top = 5.229135!
        Me.Label24.Width = 0.729134!
        '
        'TextBox25
        '
        Me.TextBox25.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox25.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox25.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox25.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox25.CanGrow = False
        Me.TextBox25.DataField = "ＬＡＮ担当情報"
        Me.TextBox25.Height = 0.1771654!
        Me.TextBox25.Left = 5.614963!
        Me.TextBox25.Name = "TextBox25"
        Me.TextBox25.ShrinkToFit = True
        Me.TextBox25.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; vertical-align:" & _
    " middle; ddo-char-set: 1; ddo-shrink-to-fit: true"
        Me.TextBox25.Text = "１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０"
        Me.TextBox25.Top = 5.010631!
        Me.TextBox25.Width = 2.958268!
        '
        'TextBox26
        '
        Me.TextBox26.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox26.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox26.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox26.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox26.DataField = "ＦＡＸ(統括ＳＣ)"
        Me.TextBox26.Height = 0.1771654!
        Me.TextBox26.Left = 9.520864!
        Me.TextBox26.Name = "TextBox26"
        Me.TextBox26.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.TextBox26.Text = "123456789012345"
        Me.TextBox26.Top = 5.677166!
        Me.TextBox26.Width = 1.177559!
        '
        'Label25
        '
        Me.Label25.Height = 0.1582677!
        Me.Label25.HyperLink = Nothing
        Me.Label25.Left = 8.75079!
        Me.Label25.Name = "Label25"
        Me.Label25.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.Label25.Text = "ＴＥＬ"
        Me.Label25.Top = 5.458663!
        Me.Label25.Width = 0.729134!
        '
        'TextBox27
        '
        Me.TextBox27.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox27.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox27.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox27.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox27.DataField = "ＴＥＬ(統括ＳＣ)"
        Me.TextBox27.Height = 0.1771654!
        Me.TextBox27.Left = 9.520864!
        Me.TextBox27.Name = "TextBox27"
        Me.TextBox27.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.TextBox27.Text = "123456789012345"
        Me.TextBox27.Top = 5.458663!
        Me.TextBox27.Width = 1.177559!
        '
        'Label26
        '
        Me.Label26.Height = 0.1582677!
        Me.Label26.HyperLink = Nothing
        Me.Label26.Left = 8.75079!
        Me.Label26.Name = "Label26"
        Me.Label26.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.Label26.Text = "ＦＡＸ"
        Me.Label26.Top = 5.677166!
        Me.Label26.Width = 0.729134!
        '
        'TextBox28
        '
        Me.TextBox28.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox28.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox28.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox28.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox28.DataField = "ＦＡＸ(代理店)"
        Me.TextBox28.Height = 0.1771654!
        Me.TextBox28.Left = 9.520867!
        Me.TextBox28.Name = "TextBox28"
        Me.TextBox28.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.TextBox28.Text = "123456789012345"
        Me.TextBox28.Top = 6.135435!
        Me.TextBox28.Width = 1.177559!
        '
        'Label27
        '
        Me.Label27.Height = 0.1582677!
        Me.Label27.HyperLink = Nothing
        Me.Label27.Left = 8.750791!
        Me.Label27.Name = "Label27"
        Me.Label27.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.Label27.Text = "ＴＥＬ"
        Me.Label27.Top = 5.916931!
        Me.Label27.Width = 0.729134!
        '
        'TextBox29
        '
        Me.TextBox29.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox29.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox29.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox29.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox29.DataField = "ＴＥＬ(代理店)"
        Me.TextBox29.Height = 0.1771654!
        Me.TextBox29.Left = 9.520867!
        Me.TextBox29.Name = "TextBox29"
        Me.TextBox29.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.TextBox29.Text = "123456789012345"
        Me.TextBox29.Top = 5.916931!
        Me.TextBox29.Width = 1.177559!
        '
        'Label28
        '
        Me.Label28.Height = 0.1582677!
        Me.Label28.HyperLink = Nothing
        Me.Label28.Left = 8.750791!
        Me.Label28.Name = "Label28"
        Me.Label28.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.Label28.Text = "ＦＡＸ"
        Me.Label28.Top = 6.135435!
        Me.Label28.Width = 0.729134!
        '
        'TextBox30
        '
        Me.TextBox30.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox30.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox30.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox30.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox30.CanGrow = False
        Me.TextBox30.DataField = "住所(統括ＳＣ)"
        Me.TextBox30.Height = 0.1771654!
        Me.TextBox30.Left = 0.9059063!
        Me.TextBox30.Name = "TextBox30"
        Me.TextBox30.ShrinkToFit = True
        Me.TextBox30.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; vertical-align:" & _
    " middle; ddo-char-set: 1; ddo-shrink-to-fit: true"
        Me.TextBox30.Text = "１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０"
        Me.TextBox30.Top = 5.677166!
        Me.TextBox30.Width = 7.667323!
        '
        'TextBox31
        '
        Me.TextBox31.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox31.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox31.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox31.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox31.CanGrow = False
        Me.TextBox31.DataField = "支店名(統括ＳＣ)"
        Me.TextBox31.Height = 0.1771654!
        Me.TextBox31.Left = 4.875986!
        Me.TextBox31.Name = "TextBox31"
        Me.TextBox31.ShrinkToFit = True
        Me.TextBox31.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; vertical-align:" & _
    " middle; ddo-char-set: 1; ddo-shrink-to-fit: true"
        Me.TextBox31.Text = "１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０"
        Me.TextBox31.Top = 5.458663!
        Me.TextBox31.Width = 3.697244!
        '
        'Label29
        '
        Me.Label29.Height = 0.1582677!
        Me.Label29.HyperLink = Nothing
        Me.Label29.Left = 0.1358282!
        Me.Label29.Name = "Label29"
        Me.Label29.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.Label29.Text = "統括ＳＣ"
        Me.Label29.Top = 5.458663!
        Me.Label29.Width = 0.729134!
        '
        'Label30
        '
        Me.Label30.Height = 0.1582677!
        Me.Label30.HyperLink = Nothing
        Me.Label30.Left = 0.1358289!
        Me.Label30.Name = "Label30"
        Me.Label30.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.Label30.Text = "住所"
        Me.Label30.Top = 5.677166!
        Me.Label30.Width = 0.729134!
        '
        'Label31
        '
        Me.Label31.Height = 0.1582677!
        Me.Label31.HyperLink = Nothing
        Me.Label31.Left = 4.105907!
        Me.Label31.Name = "Label31"
        Me.Label31.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.Label31.Text = "支店名"
        Me.Label31.Top = 5.458662!
        Me.Label31.Width = 0.729134!
        '
        'TextBox32
        '
        Me.TextBox32.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox32.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox32.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox32.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox32.CanGrow = False
        Me.TextBox32.DataField = "統括ＳＣ"
        Me.TextBox32.Height = 0.1771654!
        Me.TextBox32.Left = 0.9059063!
        Me.TextBox32.Name = "TextBox32"
        Me.TextBox32.ShrinkToFit = True
        Me.TextBox32.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; vertical-align:" & _
    " middle; ddo-char-set: 1; ddo-shrink-to-fit: true"
        Me.TextBox32.Text = "１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０"
        Me.TextBox32.Top = 5.458663!
        Me.TextBox32.Width = 2.958268!
        '
        'TextBox33
        '
        Me.TextBox33.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox33.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox33.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox33.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox33.CanGrow = False
        Me.TextBox33.DataField = "住所(代理店)"
        Me.TextBox33.Height = 0.1771654!
        Me.TextBox33.Left = 0.9059063!
        Me.TextBox33.Name = "TextBox33"
        Me.TextBox33.ShrinkToFit = True
        Me.TextBox33.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; vertical-align:" & _
    " middle; ddo-char-set: 1; ddo-shrink-to-fit: true"
        Me.TextBox33.Text = "１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０"
        Me.TextBox33.Top = 6.135435!
        Me.TextBox33.Width = 7.667323!
        '
        'Label32
        '
        Me.Label32.Height = 0.1582677!
        Me.Label32.HyperLink = Nothing
        Me.Label32.Left = 0.1358283!
        Me.Label32.Name = "Label32"
        Me.Label32.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.Label32.Text = "代理店"
        Me.Label32.Top = 5.916931!
        Me.Label32.Width = 0.729134!
        '
        'Label33
        '
        Me.Label33.Height = 0.1582677!
        Me.Label33.HyperLink = Nothing
        Me.Label33.Left = 0.135829!
        Me.Label33.Name = "Label33"
        Me.Label33.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.Label33.Text = "住所"
        Me.Label33.Top = 6.135435!
        Me.Label33.Width = 0.729134!
        '
        'TextBox34
        '
        Me.TextBox34.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox34.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox34.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox34.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TextBox34.CanGrow = False
        Me.TextBox34.DataField = "代理店"
        Me.TextBox34.Height = 0.1771654!
        Me.TextBox34.Left = 0.9059063!
        Me.TextBox34.Name = "TextBox34"
        Me.TextBox34.ShrinkToFit = True
        Me.TextBox34.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; vertical-align:" & _
    " middle; ddo-char-set: 1; ddo-shrink-to-fit: true"
        Me.TextBox34.Text = "１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０"
        Me.TextBox34.Top = 5.916931!
        Me.TextBox34.Width = 2.958268!
        '
        'txtSyusinbi
        '
        Me.txtSyusinbi.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtSyusinbi.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtSyusinbi.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtSyusinbi.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtSyusinbi.DataField = "集信日"
        Me.txtSyusinbi.Height = 0.1771654!
        Me.txtSyusinbi.Left = 9.864962!
        Me.txtSyusinbi.Name = "txtSyusinbi"
        Me.txtSyusinbi.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt"
        Me.txtSyusinbi.Text = "9999/99/99"
        Me.txtSyusinbi.Top = 0.2917324!
        Me.txtSyusinbi.Width = 0.8334646!
        '
        'Label34
        '
        Me.Label34.Height = 0.2!
        Me.Label34.HyperLink = Nothing
        Me.Label34.Left = 7.541732!
        Me.Label34.Name = "Label34"
        Me.Label34.Style = "font-family: ＭＳ ゴシック; text-align: right"
        Me.Label34.Text = "ＭＤＮ機器名"
        Me.Label34.Top = 2.135433!
        Me.Label34.Width = 0.9476385!
        '
        'txtMDN
        '
        Me.txtMDN.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtMDN.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtMDN.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtMDN.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.txtMDN.DataField = "ＭＤＮ機器名"
        Me.txtMDN.Height = 1.033465!
        Me.txtMDN.Left = 8.573225!
        Me.txtMDN.Name = "txtMDN"
        Me.txtMDN.Style = "background-color: White; font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: lef" & _
    "t"
        Me.txtMDN.Text = "0" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.txtMDN.Top = 2.135433!
        Me.txtMDN.Width = 2.125201!
        '
        'PageFooter1
        '
        Me.PageFooter1.Height = 0.0!
        Me.PageFooter1.Name = "PageFooter1"
        '
        'lblPrtdate
        '
        Me.lblPrtdate.Height = 0.1582677!
        Me.lblPrtdate.HyperLink = Nothing
        Me.lblPrtdate.Left = 9.479922!
        Me.lblPrtdate.Name = "lblPrtdate"
        Me.lblPrtdate.Style = "font-family: ＭＳ ゴシック; font-size: 9.75pt; text-align: right"
        Me.lblPrtdate.Text = "9999/99/99 12:10"
        Me.lblPrtdate.Top = 0.5291339!
        Me.lblPrtdate.Width = 1.218504!
        '
        'MNTREP005
        '
        Me.MasterReport = False
        Me.PageSettings.DefaultPaperSize = False
        Me.PageSettings.Gutter = 0.1968504!
        Me.PageSettings.Margins.Bottom = 0.1968504!
        Me.PageSettings.Margins.Left = 0.1968504!
        Me.PageSettings.Margins.Right = 0.1968504!
        Me.PageSettings.Margins.Top = 0.1968504!
        Me.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape
        Me.PageSettings.PaperHeight = 11.41732!
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Custom
        Me.PageSettings.PaperName = "ユーザー定義のサイズ"
        Me.PageSettings.PaperWidth = 8.267716!
        Me.PrintWidth = 10.77165!
        Me.ScriptLanguage = "VB.NET"
        Me.Sections.Add(Me.PageHeader1)
        Me.Sections.Add(Me.Detail1)
        Me.Sections.Add(Me.PageFooter1)
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; " & _
            "color: Black; font-family: ""MS UI Gothic""; text-align: left; vertical-align: top" & _
            "; ddo-char-set: 128", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: ""MS UI Gothic""; ddo-char-set: 12" & _
            "8", "Heading1", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: ""MS UI Goth" & _
            "ic""; ddo-char-set: 128", "Heading2", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"))
        CType(Me.lblTitle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label35, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblHallName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblJyusyo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblHallTel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblHallTel2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblHallTel4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblAttention, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTBOXID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtHallName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtJyusyo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtHallTel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtHallTel2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtHallTel4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSystem, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblSystem, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtVer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblVer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTboxTel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTboxTel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtUnyobi, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblUnyobi, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtHallCD, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblHallCD, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblSyusinbi, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtYoto1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtYoto3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtYoto4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblHallTel3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtHallTel3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtYoto2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtHallTel5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblHallTel5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTeikei1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTeikei2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTeikei3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTeikei4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtTenpo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtMDNSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblMDNSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtOyaMDNCnt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblOyaMDNCnt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtMDNCnt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblMDNCnt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtChild1MDNCnt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblChild1MDNCnt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtChild2MDNCnt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblChild2MDNCnt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtAttention, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox12, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label12, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox13, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label13, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox14, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label14, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label15, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox15, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label16, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox16, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label17, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox17, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox18, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label18, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox19, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label19, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox20, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label20, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label21, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox21, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label22, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox22, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label23, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox23, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox24, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label24, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox25, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox26, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label25, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox27, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label26, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox28, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label27, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox29, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label28, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox30, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox31, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label29, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label30, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label31, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox32, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox33, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label32, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label33, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextBox34, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSyusinbi, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label34, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtMDN, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblPrtdate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Private WithEvents lblTitle As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label1 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblHallName As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblJyusyo As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblHallTel1 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblHallTel2 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblHallTel4 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblAttention As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtTBOXID As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtHallName As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtJyusyo As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtHallTel1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtHallTel2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtHallTel4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtSystem As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblSystem As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtVer As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblVer As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtTboxTel As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblTboxTel As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtUnyobi As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblUnyobi As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtHallCD As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblHallCD As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblSyusinbi As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox8 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label8 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label2 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtYoto1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtYoto3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtYoto4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblHallTel3 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtHallTel3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtYoto2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtHallTel5 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblHallTel5 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtTeikei1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTeikei2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTeikei3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTeikei4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtTenpo As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtMDNSet As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblMDNSet As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtOyaMDNCnt As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblOyaMDNCnt As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtMDNCnt As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblMDNCnt As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtChild1MDNCnt As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblChild1MDNCnt As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtChild2MDNCnt As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblChild2MDNCnt As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtAttention As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents lblPanel As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label3 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label4 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label5 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label6 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox5 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox6 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox7 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label7 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox9 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox10 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label9 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label10 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label11 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox11 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox12 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label12 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox13 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label13 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox14 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label14 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label15 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox15 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label16 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox16 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label17 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox17 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox18 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label18 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox19 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label19 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox20 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label20 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label21 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox21 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label22 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox22 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label23 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox23 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox24 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label24 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox25 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox26 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label25 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox27 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label26 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox28 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label27 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox29 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label28 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox30 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox31 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label29 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label30 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label31 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox32 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TextBox33 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label32 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label33 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TextBox34 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents txtSyusinbi As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label34 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents txtMDN As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label35 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents lblPrtdate As GrapeCity.ActiveReports.SectionReportModel.Label
End Class
