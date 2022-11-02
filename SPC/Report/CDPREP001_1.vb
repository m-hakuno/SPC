Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class CDPREP001_1

    '2017/02/15 伯野
    'サポートセンタ検収書　集計方法変更につき仕様変更
    '　請求対象が無い場合は、２．料金算出を非表示（レポートフッタの高さを０として実行）
    Private Sub CDPREP001_1_ReportStart(sender As Object, e As EventArgs) Handles Me.ReportStart
        If Me.DataSource.rows.count > 0 Then
            If Me.DataSource.rows(0).item("請求件数").ToString() <= 0 Then
                ReportFooter1.Height = 0
            End If
        Else
            ReportFooter1.Height = 0
        End If
    End Sub

    'Private Sub GroupHeader1_BeforePrint(sender As Object, e As EventArgs) Handles GroupHeader1.BeforePrint

    '    Try
    '        Select Case txtSubtotTTL.Value
    '            Case "1"
    '                txtSubtotTTL.Text = "１．使用中ＤＢ吸上げ作業件数"
    '            Case "2"
    '                txtSubtotTTL.Text = "２．入金ログ吸上げ作業件数"
    '            Case Else
    '                txtSubtotTTL.Text = "３．その他"
    '        End Select

    '    Catch ex As Exception
    '        txtSubtotTTL.Text = ""
    '    End Try

    'End Sub
    '送付先表示変更(8/5　栗原)ここから
    Private Sub PageHeader1_Format(sender As Object, e As EventArgs) Handles PageHeader.Format
        If txtPostNM.Text <> "" Then
            txtPostNM.Text = txtPostNM.Text & " 殿"
        End If
    End Sub

    Private Sub Detail_Format(sender As Object, e As EventArgs) Handles Detail.Format
        Dim dtVal As DateTime = Nothing
        If DateTime.TryParse(txtProcDay.Text, dtVal) Then
            txtProcDay.Value = dtVal
        End If

    End Sub

    Private Sub ReportFooter1_Format(sender As Object, e As EventArgs) Handles ReportFooter1.Format
    End Sub

End Class
