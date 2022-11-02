Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class CNSREP017
    Dim pgCnt As Integer = 0

    Private Sub PageHeader1_Format(sender As Object, e As EventArgs) Handles GroupHeader1.Format
        pgCnt += 1
        TxtPAGE_NOW.Text = pgCnt

        If Not TxtNewPage.Text Is Nothing AndAlso TxtNewPage.Text.Substring(0, 1) = "E" Then
            TxtEW_FLG.Text = "東日本"
        ElseIf Not TxtNewPage.Text Is Nothing AndAlso TxtNewPage.Text.Substring(0, 1) = "W" Then
            TxtEW_FLG.Text = "西日本"
        Else
            TxtEW_FLG.Text = "日本"
        End If

        If Not TxtNewPage.Text Is Nothing AndAlso TxtNewPage.Text.Substring(1, 1) = "0" Then
            TxtIDIC.Text = "IC"
        Else
            TxtIDIC.Text = "ID"
        End If

        If Not TxtNewPage.Text Is Nothing AndAlso TxtNewPage.Text.Substring(2, 1) = "0" Then
            TxtCONST_CLS.Text = "新規工事"
        Else
            TxtCONST_CLS.Text = "通常工事"
        End If
    End Sub

End Class
