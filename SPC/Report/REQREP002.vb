Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class REQREP002

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        'トラブル処理票の場合、「状況」は出力しない
        If Me.Fields("タイトル").Value = "トラブル処理票" Then
            Me.TxtStatusNM.Text = String.Empty
        End If

        '空行読み飛ばし
        If Me.Fields("日時").Value Is DBNull.Value AndAlso
           Me.Fields("処置").Value Is DBNull.Value AndAlso
           Me.Fields("対処者").Value Is DBNull.Value Then
            Me.LayoutAction = 4
        End If

    End Sub

    Private Sub PageHeader1_Format(sender As Object, e As EventArgs) Handles PageHeader1.Format
        'トラブル処理票の場合、「状況」は出力しない
        If Me.Fields("タイトル").Value = "トラブル処理票" Then
            Me.Label11.Text = "保守対応依頼No."
        Else
            Me.Label11.Text = "トラブル管理No."
        End If
    End Sub
End Class
