Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class CNSREP010
    Dim mintNo As Integer = 0
    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format
        'mintNo += 1
        'Me.TxtNO.Text = mintNo.ToString
        'If TxtCORRES_D.Text = "" And TxtCODE.Text = "" And TxtPARS.Text = "" And TxtCONTENT.Text = "" Then
        '    TxtNO.Text = ""
        'End If

    End Sub

    Private Sub PageHeader1_Format(sender As Object, e As EventArgs) Handles PageHeader1.Format
        mintNo = 0
    End Sub

    Private Sub GroupHeader1_Format(sender As Object, e As EventArgs) Handles GroupHeader1.Format
        'If TxtNEW.Text = "1" Then
        '    TxtNEW.Text = "*"
        'Else
        '    TxtNEW.Text = ""
        'End If
        'If TxtEXP.Text = "1" Then
        '    TxtEXP.Text = "*"
        'Else
        '    TxtEXP.Text = ""
        'End If
        'If TxtSOM_REMOVE.Text = "1" Then
        '    TxtSOM_REMOVE.Text = "*"
        'Else
        '    TxtSOM_REMOVE.Text = ""
        'End If
        'If TxtSTORE_RELO.Text = "1" Then
        '    TxtSTORE_RELO.Text = "*"
        'Else
        '    TxtSTORE_RELO.Text = ""
        'End If
        'If TxtREMOVE.Text = "1" Then
        '    TxtREMOVE.Text = "*"
        'Else
        '    TxtREMOVE.Text = ""
        'End If
        'If TxtONE_REMOVE.Text = "1" Then
        '    TxtONE_REMOVE.Text = "*"
        'Else
        '    TxtONE_REMOVE.Text = ""
        'End If
        'If TxtCONF_CHANGE.Text = "1" Then
        '    TxtCONF_CHANGE.Text = "*"
        'Else
        '    TxtCONF_CHANGE.Text = ""
        'End If
        'If TxtCONF_DELI.Text = "1" Then
        '    TxtCONF_DELI.Text = "*"
        'Else
        '    TxtCONF_DELI.Text = ""
        'End If
        'If TxtREINS.Text = "1" Then
        '    TxtREINS.Text = "*"
        'Else
        '    TxtREINS.Text = ""
        'End If
        'If TxtOTHER.Text = "1" Then
        '    TxtOTHER.Text = "*"
        'Else
        '    TxtOTHER.Text = ""
        'End If
    End Sub
End Class
