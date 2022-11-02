Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class CNSREP004

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        Dim ts As New TimeSpan(0, 30, 0)
        If TxtCnstTM.Text = String.Empty Then
            TxtArriveTM.Text = String.Empty
        Else
            TxtArriveTM.Text = String.Format("{0:HH : mm}", DateTime.Parse(TxtCnstTM.Text) - ts)
        End If

    End Sub
End Class