Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document
Imports GrapeCity.ActiveReports.SectionReportModel

Public Class TBRREP013
    '改ページ毎に区切り記号が追加で挿入されるのでコメントアウト
    'Private Sub GroupHeader1_Format(sender As Object, e As EventArgs) Handles GroupHeader1.Format

    '    Dim strTmp As String = Me.txtRECVDATETIME.Text

    '    If strTmp <> "" Then
    '        Me.txtRECVDATETIME.Text = strTmp.Substring(0, 4) & "/" & strTmp.Substring(4, 2) & "/" & strTmp.Substring(6, 2)
    '        Me.txtRECVDATETIME.Text &= " " & strTmp.Substring(8, 2) & ":" & strTmp.Substring(10, 2)
    '    End If

    'End Sub

    Private intRowCount As Integer  '行カウント

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
