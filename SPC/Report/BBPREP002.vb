Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class BBPREP002
    Private Sub Detail_Format(sender As Object, e As EventArgs) Handles Detail.Format
        Dim dtVal As DateTime = Nothing
        If DateTime.TryParse(txtProcDate.Text, dtVal) Then
            txtProcDate.Value = dtVal
        Else
            txtProcDate.Text = ""
        End If
        If DateTime.TryParse(txtBBDate.Text, dtVal) Then
            txtBBDate.Value = dtVal
        Else
            txtBBDate.Text = ""
        End If
    End Sub
    '送付先表示変更(8/5　栗原)ここから
    Private Sub PageHeader1_Format(sender As Object, e As EventArgs) Handles PageHeader.Format
        If txtPostNM.Text <> "" Then
            txtPostNM.Text = txtPostNM.Text & " 殿"
        End If
    End Sub
End Class 
