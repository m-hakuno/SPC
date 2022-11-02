Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class CNSREP009

    Sub New(ByVal dtSub_1 As DataTable, ByVal dtSub_2 As DataTable)

        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。

        Dim rpt_1 As New CNSREP009_1
        Dim rpt_2 As New CNSREP009_2

        rpt_1.DataSource = dtSub_1
        rpt_2.DataSource = dtSub_2

        Me.SubReport1.Report = rpt_1
        Me.SubReport2.Report = rpt_2

    End Sub

    Private Sub CNSREP009_ReportStart(sender As Object, e As EventArgs) Handles MyBase.ReportStart

    End Sub

End Class
