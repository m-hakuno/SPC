Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class CDPREP001

    'Private Sub GroupHeader1_BeforePrint(sender As Object, e As EventArgs) Handles GroupHeader1.BeforePrint

    '    Try
    '        Select Case txtSubtotTTL.Value
    '            Case "1"
    '                txtSubtotTTL.Text = "１．使用中ＤＢ吸上げ作業件数"
    '            Case "2"
    '                txtSubtotTTL.Text = "２．入金ログ吸上げ作業件数"
    '            Case Else
    '                txtSubtotTTL.Text = "３．その他"
    '        End Select

    '    Catch ex As Exception
    '        txtSubtotTTL.Text = ""
    '    End Try

    'End Sub
    '送付先表示変更(8/5　栗原)ここから
    Private Sub PageHeader1_Format(sender As Object, e As EventArgs) Handles PageHeader.Format
        If txtPostNM.Text <> "" Then
            txtPostNM.Text = txtPostNM.Text & " 殿"
        End If
    End Sub

    Private Sub Detail_Format(sender As Object, e As EventArgs) Handles Detail.Format
        Dim dtVal As DateTime = Nothing
        If DateTime.TryParse(txtProcDay.Text, dtVal) Then
            txtProcDay.Value = dtVal
        End If

    End Sub
End Class
