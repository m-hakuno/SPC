Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document
Imports GrapeCity.ActiveReports.SectionReportModel

Public Class TBRREP005

    Private intRowCount As Integer  '行カウント

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        intRowCount += 1
        If intRowCount = 40 Then
            '40行目で改ページ
            Me.Detail1.NewPage = NewPage.After
            intRowCount = 0
        Else
            Me.Detail1.NewPage = NewPage.None

        End If

    End Sub

End Class
