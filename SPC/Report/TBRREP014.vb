Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document
Imports GrapeCity.ActiveReports.SectionReportModel

Public Class TBRREP014

    Private intRowCount As Integer  '行カウント

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        intRowCount += 1
        If intRowCount = 13 Then
            '13行目でカウントリセット、改ページ
            intRowCount = 1
            Me.Detail1.NewPage = NewPage.Before

        Else
            Me.Detail1.NewPage = NewPage.None

        End If

    End Sub


End Class
