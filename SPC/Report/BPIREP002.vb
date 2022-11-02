Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document


Public Class BPIREP002

    Dim intDetCnter As Integer = 0 '行番号集計用の変数。
    Dim intDetPrint As Integer = 46 '１ページの行数

    Private Sub BPIREP002_ReportStart(sender As Object, e As EventArgs) Handles MyBase.ReportStart

    End Sub

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format
        intDetCnter += 1
        If intDetCnter >= 46 Then
            Me.Line5.Visible = True
            Me.Line6.Visible = True
            intDetCnter = 0
        Else
            Me.Line5.Visible = False
            Me.Line6.Visible = False
        End If

    End Sub

End Class
