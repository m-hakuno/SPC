Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class TBRREP009

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        If TxtREG_DEP.Text = 0 Then
            TxtREG_DEP.Text = "正エリア"
        ElseIf TxtREG_DEP.Text = 1 Then
            TxtREG_DEP.Text = "副エリア"
        Else
            TxtREG_DEP.Text = ""
        End If

        If TxtTODAY_BRO.Text = 0 Then
            TxtTODAY_BRO.Text = "当日"
        ElseIf TxtTODAY_BRO.Text = 1 Then
            TxtTODAY_BRO.Text = "繰越"
        Else
            TxtTODAY_BRO.Text = ""
        End If
    End Sub
End Class
