Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class TBRREP008

    Private Sub PageHeader1_Format(sender As Object, e As EventArgs) Handles PageHeader1.Format
        ''ページ数
        'Me.TextBox26.Text = Integer.Parse(Me.TextBox26.Text).ToString("000")

    End Sub

    Private Sub PageHeader1_BeforePrint(sender As Object, e As EventArgs) Handles PageHeader1.BeforePrint
        'ホール名
        'Me.TxtHALL.Text = "【" & Me.TxtHALL.Text & "】"
        '照会日時
        'Me.TextBox23.Text = Left(Me.TextBox23.Text, 16)
        'If Me.TxtINQ_D.Text.ToString.Length >= 16 Then
        '    Me.TxtINQ_D.Text = Me.TxtINQ_D.Text.Substring(0, 16)
        'End If
        'TBOXバージョン
        'Me.TextBox29.Text = Left(Me.TextBox29.Text, 2) & "." & Right(Me.TextBox29.Text, 2)
        'If Me.TxtTBOX_VER.Text.ToString.Length >= 4 Then
        '    Me.TxtTBOX_VER.Text = Me.TxtTBOX_VER.Text.Substring(0, 2) & "." & Me.TxtTBOX_VER.Text.Substring(Me.TxtTBOX_VER.Text.ToString.Length - 2, 2)
        'End If
    End Sub

End Class
