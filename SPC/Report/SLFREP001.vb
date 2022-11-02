Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class SLFREP001
    Dim mintNo As Integer = 0
    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format
        mintNo += 1
        Me.TxtNo.Text = mintNo.ToString
    End Sub
End Class
