Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document
Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.LayoutAction

Public Class TBRREP006

    Private intRowCount As Integer  '行カウント

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        Me.LayoutAction = 7

        intRowCount += 1
        If intRowCount = 19 Then
            '        If intRowCount = 20 Then
            '20行目でカウントリセット
            intRowCount = 0
        Else
            Me.Detail1.NewPage = NewPage.None

            '2行毎に空白を挿入する
            '            If (intRowCount Mod 3) = 0 Then
            '                Me.LayoutAction = 2
            '            End If

        End If

    End Sub

End Class
