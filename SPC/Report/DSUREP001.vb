Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class DSUREP001

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

    End Sub

    Private Sub PageHeader1_BeforePrint(sender As Object, e As EventArgs) Handles PageHeader1.BeforePrint
        txtNTT_BRANCH_1.Text = txtNTT_BRANCH_1.Text.Replace("　", Environment.NewLine) & "　御中"
    End Sub
End Class
