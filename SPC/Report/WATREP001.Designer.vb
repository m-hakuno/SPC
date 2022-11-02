<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class WATREP001
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
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(WATREP001))
        Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
        Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail()
        Me.TxtRowNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtBB1InvstRepNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtRepairNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtTboxID = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtModel = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtJBNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtChecker = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtLedFlg = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtCollRslt = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtDestFlg = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtEndDT = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.TxtNoteText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
        Me.Line1 = New GrapeCity.ActiveReports.SectionReportModel.Line()
        Me.Label15 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.ReportHeader1 = New GrapeCity.ActiveReports.SectionReportModel.ReportHeader()
        Me.ReportFooter1 = New GrapeCity.ActiveReports.SectionReportModel.ReportFooter()
        Me.GroupHeader1 = New GrapeCity.ActiveReports.SectionReportModel.GroupHeader()
        Me.Label3 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label4 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label5 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label6 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label7 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label8 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label9 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label10 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label11 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label12 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label13 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label14 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TxtAppaClsNM = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label2 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label1 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.TxtDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.Label16 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.Label17 = New GrapeCity.ActiveReports.SectionReportModel.Label()
        Me.GroupFooter1 = New GrapeCity.ActiveReports.SectionReportModel.GroupFooter()
        CType(Me.TxtRowNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtBB1InvstRepNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtRepairNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtTboxID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtModel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtJBNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtChecker, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtLedFlg, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtCollRslt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtDestFlg, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtEndDT, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtNoteText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label15, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label12, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label13, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label14, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtAppaClsNM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label16, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Label17, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'PageHeader1
        '
        Me.PageHeader1.Height = 0.0!
        Me.PageHeader1.Name = "PageHeader1"
        '
        'Detail1
        '
        Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TxtRowNo, Me.TxtBB1InvstRepNo, Me.TxtRepairNo, Me.TxtTboxID, Me.TxtModel, Me.TxtJBNo, Me.TxtChecker, Me.TxtLedFlg, Me.TxtCollRslt, Me.TxtDestFlg, Me.TxtEndDT, Me.TxtNoteText})
        Me.Detail1.Height = 0.2952756!
        Me.Detail1.Name = "Detail1"
        '
        'TxtRowNo
        '
        Me.TxtRowNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtRowNo.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtRowNo.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtRowNo.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtRowNo.DataField = "No"
        Me.TxtRowNo.Height = 0.2952756!
        Me.TxtRowNo.Left = 0.1968503!
        Me.TxtRowNo.Name = "TxtRowNo"
        Me.TxtRowNo.Style = "text-align: center; vertical-align: middle"
        Me.TxtRowNo.Text = Nothing
        Me.TxtRowNo.Top = 0.0!
        Me.TxtRowNo.Width = 0.5905512!
        '
        'TxtBB1InvstRepNo
        '
        Me.TxtBB1InvstRepNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtBB1InvstRepNo.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtBB1InvstRepNo.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtBB1InvstRepNo.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtBB1InvstRepNo.CanGrow = False
        Me.TxtBB1InvstRepNo.DataField = "BB1調報No"
        Me.TxtBB1InvstRepNo.Height = 0.2952756!
        Me.TxtBB1InvstRepNo.Left = 0.7874016!
        Me.TxtBB1InvstRepNo.MultiLine = False
        Me.TxtBB1InvstRepNo.Name = "TxtBB1InvstRepNo"
        Me.TxtBB1InvstRepNo.Style = "text-align: center; vertical-align: middle"
        Me.TxtBB1InvstRepNo.Text = Nothing
        Me.TxtBB1InvstRepNo.Top = 0.0!
        Me.TxtBB1InvstRepNo.Width = 0.984252!
        '
        'TxtRepairNo
        '
        Me.TxtRepairNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtRepairNo.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtRepairNo.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtRepairNo.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtRepairNo.CanGrow = False
        Me.TxtRepairNo.DataField = "修理依頼No"
        Me.TxtRepairNo.Height = 0.2952756!
        Me.TxtRepairNo.Left = 1.771654!
        Me.TxtRepairNo.MultiLine = False
        Me.TxtRepairNo.Name = "TxtRepairNo"
        Me.TxtRepairNo.Style = "text-align: center; vertical-align: middle"
        Me.TxtRepairNo.Text = Nothing
        Me.TxtRepairNo.Top = 0.0!
        Me.TxtRepairNo.Width = 0.9842519!
        '
        'TxtTboxID
        '
        Me.TxtTboxID.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtTboxID.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtTboxID.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtTboxID.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtTboxID.CanGrow = False
        Me.TxtTboxID.DataField = "TBOXID"
        Me.TxtTboxID.Height = 0.2952756!
        Me.TxtTboxID.Left = 2.755906!
        Me.TxtTboxID.MultiLine = False
        Me.TxtTboxID.Name = "TxtTboxID"
        Me.TxtTboxID.Style = "text-align: center; vertical-align: middle"
        Me.TxtTboxID.Text = Nothing
        Me.TxtTboxID.Top = 0.0!
        Me.TxtTboxID.Width = 0.9448819!
        '
        'TxtModel
        '
        Me.TxtModel.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtModel.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtModel.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtModel.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtModel.CanGrow = False
        Me.TxtModel.DataField = "型式番号"
        Me.TxtModel.Height = 0.2952756!
        Me.TxtModel.Left = 3.700788!
        Me.TxtModel.MultiLine = False
        Me.TxtModel.Name = "TxtModel"
        Me.TxtModel.Style = "text-align: center; vertical-align: middle"
        Me.TxtModel.Text = Nothing
        Me.TxtModel.Top = 0.0!
        Me.TxtModel.Width = 1.181102!
        '
        'TxtJBNo
        '
        Me.TxtJBNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtJBNo.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtJBNo.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtJBNo.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtJBNo.Height = 0.2952756!
        Me.TxtJBNo.Left = 4.88189!
        Me.TxtJBNo.Name = "TxtJBNo"
        Me.TxtJBNo.Style = "text-align: center; vertical-align: middle"
        Me.TxtJBNo.Text = Nothing
        Me.TxtJBNo.Top = 0.0!
        Me.TxtJBNo.Width = 0.8661417!
        '
        'TxtChecker
        '
        Me.TxtChecker.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtChecker.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtChecker.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtChecker.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtChecker.Height = 0.2952756!
        Me.TxtChecker.Left = 5.748032!
        Me.TxtChecker.Name = "TxtChecker"
        Me.TxtChecker.Style = "text-align: center; vertical-align: middle"
        Me.TxtChecker.Text = Nothing
        Me.TxtChecker.Top = 0.0!
        Me.TxtChecker.Width = 0.8661417!
        '
        'TxtLedFlg
        '
        Me.TxtLedFlg.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtLedFlg.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtLedFlg.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtLedFlg.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtLedFlg.Height = 0.2952756!
        Me.TxtLedFlg.Left = 6.614174!
        Me.TxtLedFlg.Name = "TxtLedFlg"
        Me.TxtLedFlg.Style = "text-align: center; vertical-align: middle"
        Me.TxtLedFlg.Text = "○ ・ ×"
        Me.TxtLedFlg.Top = 0.0!
        Me.TxtLedFlg.Width = 0.8661417!
        '
        'TxtCollRslt
        '
        Me.TxtCollRslt.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtCollRslt.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtCollRslt.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtCollRslt.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtCollRslt.Height = 0.2952756!
        Me.TxtCollRslt.Left = 7.480316!
        Me.TxtCollRslt.Name = "TxtCollRslt"
        Me.TxtCollRslt.Style = "text-align: center; vertical-align: middle"
        Me.TxtCollRslt.Text = Nothing
        Me.TxtCollRslt.Top = 0.0!
        Me.TxtCollRslt.Width = 0.9842528!
        '
        'TxtDestFlg
        '
        Me.TxtDestFlg.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtDestFlg.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtDestFlg.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtDestFlg.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtDestFlg.Height = 0.2952756!
        Me.TxtDestFlg.Left = 8.464567!
        Me.TxtDestFlg.Name = "TxtDestFlg"
        Me.TxtDestFlg.Style = "text-align: center; vertical-align: middle"
        Me.TxtDestFlg.Text = "○ ・ ×"
        Me.TxtDestFlg.Top = 0.0!
        Me.TxtDestFlg.Width = 0.8661412!
        '
        'TxtEndDT
        '
        Me.TxtEndDT.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtEndDT.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtEndDT.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtEndDT.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtEndDT.Height = 0.2952756!
        Me.TxtEndDT.Left = 9.330709!
        Me.TxtEndDT.Name = "TxtEndDT"
        Me.TxtEndDT.OutputFormat = resources.GetString("TxtEndDT.OutputFormat")
        Me.TxtEndDT.Style = "text-align: center; vertical-align: middle"
        Me.TxtEndDT.Text = "："
        Me.TxtEndDT.Top = 0.0!
        Me.TxtEndDT.Width = 0.7086614!
        '
        'TxtNoteText
        '
        Me.TxtNoteText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtNoteText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtNoteText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.ThickSolid
        Me.TxtNoteText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.TxtNoteText.CanGrow = False
        Me.TxtNoteText.Height = 0.2952756!
        Me.TxtNoteText.Left = 10.03937!
        Me.TxtNoteText.MultiLine = False
        Me.TxtNoteText.Name = "TxtNoteText"
        Me.TxtNoteText.Style = "text-align: center; vertical-align: middle"
        Me.TxtNoteText.Text = Nothing
        Me.TxtNoteText.Top = 0.0!
        Me.TxtNoteText.Width = 0.984252!
        '
        'PageFooter1
        '
        Me.PageFooter1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.Line1, Me.Label15})
        Me.PageFooter1.Height = 0.9320047!
        Me.PageFooter1.Name = "PageFooter1"
        '
        'Line1
        '
        Me.Line1.Height = 0.0!
        Me.Line1.Left = 0.1968503!
        Me.Line1.LineWeight = 2.0!
        Me.Line1.Name = "Line1"
        Me.Line1.Top = 0.07874016!
        Me.Line1.Width = 10.82677!
        Me.Line1.X1 = 0.1968503!
        Me.Line1.X2 = 11.02362!
        Me.Line1.Y1 = 0.07874016!
        Me.Line1.Y2 = 0.07874016!
        '
        'Label15
        '
        Me.Label15.Height = 0.5622048!
        Me.Label15.HyperLink = Nothing
        Me.Label15.Left = 0.1968504!
        Me.Label15.Name = "Label15"
        Me.Label15.Style = "font-size: 8.2pt; ddo-char-set: 1"
        Me.Label15.Text = "【店内集信結果フラグID】 １：他店舗/正常終了　２：他店舗/BB異常　３：機種異常/BB異常　４：他店舗/通信異常　５：認証異常/通信異常　６：未接続/無応答　" & _
    "７：工場出荷/正常終了　８：工場出荷/通信異常　９：自店舗/正常終了" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "【店内集信結果フラグIC】 １：他店装置検出　２：運用世代超過検出　３：店内装置種別" & _
    "不一致　４：店内装置異常　５：店内装置異常電文　６：無表示（通信ＮＧ）"
        Me.Label15.Top = 0.1574803!
        Me.Label15.Width = 11.01417!
        '
        'ReportHeader1
        '
        Me.ReportHeader1.Height = 0.0!
        Me.ReportHeader1.Name = "ReportHeader1"
        '
        'ReportFooter1
        '
        Me.ReportFooter1.BackColor = System.Drawing.Color.Red
        Me.ReportFooter1.Height = 0.0!
        Me.ReportFooter1.Name = "ReportFooter1"
        '
        'GroupHeader1
        '
        Me.GroupHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.Label3, Me.Label4, Me.Label5, Me.Label6, Me.Label7, Me.Label8, Me.Label9, Me.Label10, Me.Label11, Me.Label12, Me.Label13, Me.Label14, Me.TxtAppaClsNM, Me.Label2, Me.Label1, Me.TxtDate, Me.Label16, Me.Label17})
        Me.GroupHeader1.DataField = "TBOX種別CD"
        Me.GroupHeader1.GroupKeepTogether = GrapeCity.ActiveReports.SectionReportModel.GroupKeepTogether.All
        Me.GroupHeader1.Height = 1.771654!
        Me.GroupHeader1.Name = "GroupHeader1"
        Me.GroupHeader1.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before
        Me.GroupHeader1.RepeatStyle = GrapeCity.ActiveReports.SectionReportModel.RepeatStyle.OnPage
        '
        'Label3
        '
        Me.Label3.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label3.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label3.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label3.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label3.Height = 0.1980315!
        Me.Label3.HyperLink = Nothing
        Me.Label3.Left = 0.1968504!
        Me.Label3.Name = "Label3"
        Me.Label3.Style = "background-color: Gainsboro; text-align: center"
        Me.Label3.Text = "Ｎｏ．"
        Me.Label3.Top = 1.573622!
        Me.Label3.Width = 0.5905511!
        '
        'Label4
        '
        Me.Label4.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label4.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label4.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label4.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label4.Height = 0.1980315!
        Me.Label4.HyperLink = Nothing
        Me.Label4.Left = 0.787402!
        Me.Label4.Name = "Label4"
        Me.Label4.Style = "background-color: Gainsboro; text-align: center"
        Me.Label4.Text = "BB1調報Ｎｏ．"
        Me.Label4.Top = 1.573622!
        Me.Label4.Width = 0.9842521!
        '
        'Label5
        '
        Me.Label5.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label5.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label5.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label5.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label5.Height = 0.1980315!
        Me.Label5.HyperLink = Nothing
        Me.Label5.Left = 1.771654!
        Me.Label5.Name = "Label5"
        Me.Label5.Style = "background-color: Gainsboro; text-align: center"
        Me.Label5.Text = "修理依頼Ｎｏ．"
        Me.Label5.Top = 1.573622!
        Me.Label5.Width = 0.9842521!
        '
        'Label6
        '
        Me.Label6.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label6.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label6.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label6.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label6.Height = 0.1980315!
        Me.Label6.HyperLink = Nothing
        Me.Label6.Left = 2.755906!
        Me.Label6.Name = "Label6"
        Me.Label6.Style = "background-color: Gainsboro; text-align: center"
        Me.Label6.Text = "TBOXID"
        Me.Label6.Top = 1.573622!
        Me.Label6.Width = 0.944882!
        '
        'Label7
        '
        Me.Label7.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label7.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label7.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label7.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label7.Height = 0.1980315!
        Me.Label7.HyperLink = Nothing
        Me.Label7.Left = 3.700788!
        Me.Label7.Name = "Label7"
        Me.Label7.Style = "background-color: Gainsboro; text-align: center"
        Me.Label7.Text = "型式番号"
        Me.Label7.Top = 1.573622!
        Me.Label7.Width = 1.181103!
        '
        'Label8
        '
        Me.Label8.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label8.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label8.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label8.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label8.Height = 0.1980315!
        Me.Label8.HyperLink = Nothing
        Me.Label8.Left = 4.881889!
        Me.Label8.Name = "Label8"
        Me.Label8.Style = "background-color: Gainsboro; text-align: center"
        Me.Label8.Text = "JB番号"
        Me.Label8.Top = 1.573622!
        Me.Label8.Width = 0.8661417!
        '
        'Label9
        '
        Me.Label9.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label9.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label9.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label9.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label9.Height = 0.1980315!
        Me.Label9.HyperLink = Nothing
        Me.Label9.Left = 5.748033!
        Me.Label9.Name = "Label9"
        Me.Label9.Style = "background-color: Gainsboro; text-align: center"
        Me.Label9.Text = "CHECKER"
        Me.Label9.Top = 1.573622!
        Me.Label9.Width = 0.8661417!
        '
        'Label10
        '
        Me.Label10.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label10.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label10.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label10.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label10.Height = 0.1980315!
        Me.Label10.HyperLink = Nothing
        Me.Label10.Left = 6.614174!
        Me.Label10.Name = "Label10"
        Me.Label10.Style = "background-color: Gainsboro; text-align: center"
        Me.Label10.Text = "LED点灯"
        Me.Label10.Top = 1.573622!
        Me.Label10.Width = 0.8661417!
        '
        'Label11
        '
        Me.Label11.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label11.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label11.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label11.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label11.Height = 0.1980315!
        Me.Label11.HyperLink = Nothing
        Me.Label11.Left = 7.48032!
        Me.Label11.Name = "Label11"
        Me.Label11.Style = "background-color: Gainsboro; text-align: center"
        Me.Label11.Text = "店内集信結果"
        Me.Label11.Top = 1.573622!
        Me.Label11.Width = 0.9842521!
        '
        'Label12
        '
        Me.Label12.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label12.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label12.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label12.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label12.Height = 0.1980315!
        Me.Label12.HyperLink = Nothing
        Me.Label12.Left = 8.464569!
        Me.Label12.Name = "Label12"
        Me.Label12.Style = "background-color: Gainsboro; text-align: center"
        Me.Label12.Text = "破棄完了"
        Me.Label12.Top = 1.573622!
        Me.Label12.Width = 0.8661417!
        '
        'Label13
        '
        Me.Label13.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label13.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label13.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label13.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label13.Height = 0.1980315!
        Me.Label13.HyperLink = Nothing
        Me.Label13.Left = 9.330709!
        Me.Label13.Name = "Label13"
        Me.Label13.Style = "background-color: Gainsboro; text-align: center"
        Me.Label13.Text = "終了時刻"
        Me.Label13.Top = 1.573622!
        Me.Label13.Width = 0.7086611!
        '
        'Label14
        '
        Me.Label14.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label14.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label14.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label14.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
        Me.Label14.Height = 0.1980315!
        Me.Label14.HyperLink = Nothing
        Me.Label14.Left = 10.03937!
        Me.Label14.Name = "Label14"
        Me.Label14.Style = "background-color: Gainsboro; text-align: center"
        Me.Label14.Text = "備考"
        Me.Label14.Top = 1.573622!
        Me.Label14.Width = 0.9842521!
        '
        'TxtAppaClsNM
        '
        Me.TxtAppaClsNM.DataField = "TBOX種別"
        Me.TxtAppaClsNM.Height = 0.2755905!
        Me.TxtAppaClsNM.Left = 3.228346!
        Me.TxtAppaClsNM.Name = "TxtAppaClsNM"
        Me.TxtAppaClsNM.OutputFormat = resources.GetString("TxtAppaClsNM.OutputFormat")
        Me.TxtAppaClsNM.Style = "font-size: 17.5pt; font-style: italic; font-weight: bold; text-decoration: underl" & _
    "ine; vertical-align: bottom; ddo-char-set: 1"
        Me.TxtAppaClsNM.Text = "(IT100)"
        Me.TxtAppaClsNM.Top = 0.7874014!
        Me.TxtAppaClsNM.Width = 3.779528!
        '
        'Label2
        '
        Me.Label2.Height = 0.1488191!
        Me.Label2.HyperLink = Nothing
        Me.Label2.Left = 0.1968513!
        Me.Label2.Name = "Label2"
        Me.Label2.Padding = New GrapeCity.ActiveReports.PaddingEx(6, 0, 0, 0)
        Me.Label2.Style = "font-weight: bold; text-align: justify"
        Me.Label2.Text = "【LEC】"
        Me.Label2.Top = 1.174803!
        Me.Label2.Width = 0.5905495!
        '
        'Label1
        '
        Me.Label1.Height = 0.2755905!
        Me.Label1.HyperLink = Nothing
        Me.Label1.Left = 0.1968513!
        Me.Label1.Name = "Label1"
        Me.Label1.Padding = New GrapeCity.ActiveReports.PaddingEx(3, 0, 0, 0)
        Me.Label1.Style = "font-size: 17.5pt; font-style: italic; font-weight: bold; text-align: left; text-" & _
    "decoration: underline; vertical-align: bottom; ddo-char-set: 1"
        Me.Label1.Text = "ブラックボックス調査チェックリスト"
        Me.Label1.Top = 0.7874014!
        Me.Label1.Width = 3.153543!
        '
        'TxtDate
        '
        Me.TxtDate.DataField = "=System.DateTime.Now.ToString(""yyyy年MM月dd日 HH時mm分"")"
        Me.TxtDate.Height = 0.1574803!
        Me.TxtDate.Left = 9.031104!
        Me.TxtDate.Name = "TxtDate"
        Me.TxtDate.Padding = New GrapeCity.ActiveReports.PaddingEx(0, 0, 3, 0)
        Me.TxtDate.Style = "text-align: right; vertical-align: bottom"
        Me.TxtDate.Text = "2014年1月20日 10時30分"
        Me.TxtDate.Top = 0.9055117!
        Me.TxtDate.Width = 1.992519!
        '
        'Label16
        '
        Me.Label16.Height = 0.3988191!
        Me.Label16.HyperLink = Nothing
        Me.Label16.Left = 0.7874017!
        Me.Label16.Name = "Label16"
        Me.Label16.Padding = New GrapeCity.ActiveReports.PaddingEx(6, 0, 0, 0)
        Me.Label16.Style = "font-weight: bold"
        Me.Label16.Text = "リアル系TBOXID：99400011、準リアル系TBOXID：99400021、IT1XX：99450011" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "リアル系TBOXID：91000091、準リアル" & _
    "系TBOXID：91000101、IT1XX：98500011、NVC：98600012"
        Me.Label16.Top = 1.174803!
        Me.Label16.Width = 7.920471!
        '
        'Label17
        '
        Me.Label17.Height = 0.1673229!
        Me.Label17.HyperLink = Nothing
        Me.Label17.Left = 0.1968504!
        Me.Label17.Name = "Label17"
        Me.Label17.Padding = New GrapeCity.ActiveReports.PaddingEx(6, 0, 0, 0)
        Me.Label17.Style = "font-weight: bold; text-align: justify"
        Me.Label17.Text = "【NGC】"
        Me.Label17.Top = 1.323622!
        Me.Label17.Width = 0.5905494!
        '
        'GroupFooter1
        '
        Me.GroupFooter1.Height = 0.0!
        Me.GroupFooter1.Name = "GroupFooter1"
        '
        'WATREP001
        '
        Me.MasterReport = False
        Me.PageSettings.DefaultPaperSize = False
        Me.PageSettings.Margins.Bottom = 0.1968504!
        Me.PageSettings.Margins.Left = 0.1968504!
        Me.PageSettings.Margins.Right = 0.1968504!
        Me.PageSettings.Margins.Top = 0.1968504!
        Me.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape
        Me.PageSettings.PaperHeight = 11.69291!
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4
        Me.PageSettings.PaperWidth = 8.267716!
        Me.PrintWidth = 11.55!
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
        CType(Me.TxtRowNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtBB1InvstRepNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtRepairNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtTboxID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtModel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtJBNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtChecker, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtLedFlg, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtCollRslt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtDestFlg, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtEndDT, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtNoteText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label15, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label12, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label13, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label14, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtAppaClsNM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label16, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Label17, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Private WithEvents ReportHeader1 As GrapeCity.ActiveReports.SectionReportModel.ReportHeader
    Private WithEvents ReportFooter1 As GrapeCity.ActiveReports.SectionReportModel.ReportFooter
    Private WithEvents TxtRowNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtBB1InvstRepNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtRepairNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtTboxID As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtModel As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtJBNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtChecker As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtLedFlg As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtCollRslt As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtDestFlg As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtEndDT As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents TxtNoteText As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Line1 As GrapeCity.ActiveReports.SectionReportModel.Line
    Private WithEvents Label15 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents GroupHeader1 As GrapeCity.ActiveReports.SectionReportModel.GroupHeader
    Private WithEvents Label3 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label4 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label5 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label6 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label7 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label8 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label9 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label10 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label11 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label12 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label13 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label14 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TxtAppaClsNM As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents Label2 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label1 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents TxtDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Private WithEvents GroupFooter1 As GrapeCity.ActiveReports.SectionReportModel.GroupFooter
    Private WithEvents Label16 As GrapeCity.ActiveReports.SectionReportModel.Label
    Private WithEvents Label17 As GrapeCity.ActiveReports.SectionReportModel.Label
End Class
