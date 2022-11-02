Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class TBRREP010

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format
        If TxtREG_DEQ.Text = 0 Then
            TxtREG_DEQ.Text = "正エリア"
        ElseIf TxtREG_DEQ.Text = 1 Then
            TxtREG_DEQ.Text = "副エリア"
        Else
            TxtREG_DEQ.Text = ""
        End If
        If TxtTODAY_BROU.Text = 0 Then
            TxtTODAY_BROU.Text = "当日"
        ElseIf TxtTODAY_BROU.Text = 1 Then
            TxtTODAY_BROU.Text = "繰越"
        Else
            TxtTODAY_BROU.Text = ""
        End If
    End Sub

End Class