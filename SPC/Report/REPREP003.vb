Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class REPREP003
    Dim intCounter As Integer = 0
    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format
        intCounter = intCounter + 1
        'NO
        Me.txtNo.Text = intCounter

    End Sub

End Class
