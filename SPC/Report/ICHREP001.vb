Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class ICHREP001

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        'If TxtREG_DEP.Text = 0 Then
        '    TxtREG_DEP.Text = "正"
        'ElseIf TxtREG_DEP.Text = 1 Then
        '    TxtREG_DEP.Text = "副"
        'Else
        '    TxtREG_DEP.Text = ""
        'End If

        If TxtCARD_CLS.Text = "C" Then
            TxtCARD_CLS.Text = "一般(I)"
        Else
            TxtCARD_CLS.Text = ""
        End If
    End Sub

End Class
