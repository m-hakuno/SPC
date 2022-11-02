Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document
Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.LayoutAction

Public Class TBRREP004

    Private intRowCount As Integer  '行カウント

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        Me.LayoutAction = 7

        intRowCount += 1
        If intRowCount = 43 Then
            '43行目で改ページ
            Me.Detail1.NewPage = NewPage.After
            intRowCount = 0
        Else
            Me.Detail1.NewPage = NewPage.None

            '10行毎に空白を挿入する
            If (intRowCount Mod 11) = 0 Then
                Me.LayoutAction = 2
            End If

        End If

        Dim dt As System.DateTime

        'If IsDate(TextBox10.Text) = False Then
        If Date.TryParse(TextBox10.Text, dt) = False Then
            TextBox10.Text = "0000/00/00 00:00:00"
        End If
    End Sub

End Class
