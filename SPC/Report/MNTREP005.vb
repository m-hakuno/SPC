Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class MNTREP005

    Private Sub PageHeader1_BeforePrint(sender As Object, e As EventArgs) Handles PageHeader1.BeforePrint
        Dim dt As New DateTime
        dt = DateTime.Now

        lblPrtdate.Text = dt.ToString("yyyy/MM/dd HH:mm")
    End Sub
End Class
