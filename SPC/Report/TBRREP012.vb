Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document
Imports GrapeCity.ActiveReports.SectionReportModel

Public Class TBRREP012

    Private intRowCount As Integer  '行カウント

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        intRowCount += 1
        If intRowCount = 30 Then
            '30行目でカウントリセット、次から改行
            intRowCount = 0
            Me.Detail1.NewColumn = NewColumn.After

        Else
            Me.Detail1.NewColumn = NewColumn.None

        End If

    End Sub

End Class
