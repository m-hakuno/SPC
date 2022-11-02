Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class BBPREP003_2

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format
        If Me.TxtTitle.Text.Trim = "券売機" Or Me.TxtTitle.Text.Trim = "" Then
            Me.LineSep1.Visible = False
            Me.LineSep2.Visible = False
            If Me.TxtTitle.Text.Trim = "券売機" Then
                Me.LineSep2.Visible = True
            End If
        Else
            Me.LineSep1.Visible = True
            Me.LineSep2.Visible = True
        End If
    End Sub

    Private Sub BBPREP003_2_ReportStart(sender As Object, e As EventArgs) Handles MyBase.ReportStart

    End Sub
End Class
