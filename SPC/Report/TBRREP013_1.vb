Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document
Imports GrapeCity.ActiveReports.SectionReportModel

Public Class TBRREP013_1

    Private intRowCount As Integer  '行カウント

    Public Sub New(ByVal dtSub_1 As DataTable)

        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。

        Dim rpt_1 As New TBRREP013_2

        rpt_1.DataSource = dtSub_1

        Me.SubReport1.Report = rpt_1

    End Sub

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        intRowCount += 1
        If intRowCount = 20 Then
            '20行目でカウントリセット、改ページ
            intRowCount = 1
            Me.Detail1.NewPage = NewPage.Before

        Else
            Me.Detail1.NewPage = NewPage.None

        End If

    End Sub

End Class
