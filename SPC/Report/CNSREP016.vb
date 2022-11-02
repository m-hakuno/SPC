Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class CNSREP016

    Private Sub PageHeader1_Format(sender As Object, e As EventArgs) Handles PageHeader1.Format

        If TxtCONST_CLS.Text = "1" Then
            TxtCONST_CLS.Text = "新規工事"
        Else
            TxtCONST_CLS.Text = "通常工事"
        End If
        If TxtEW_FLG.Text = "E" Then
            TxtEW_FLG.Text = "東日本"
        ElseIf TxtEW_FLG.Text = "W" Then
            TxtEW_FLG.Text = "西日本"
        ElseIf TxtEW_FLG.Text = "Z" Then
            TxtEW_FLG.Text = ""
        Else
            TxtEW_FLG.Text = "日本"
        End If

        If TxtIDIC.Text = "1" Then
            TxtIDIC.Text = "ID"
        Else
            TxtIDIC.Text = "IC"
        End If

    End Sub

End Class
