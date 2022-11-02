Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document 
Imports System
Imports System.Globalization

Public Class CDPREP002
    Dim rowNum As Integer = 1

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format
        Dim ci As New System.Globalization.CultureInfo("ja-JP")
        ' 和暦を表すクラス
        Dim jp As New System.Globalization.JapaneseCalendar
        Dim dt As System.DateTime

        ' 現在のカルチャで使用する暦を、和暦に設定
        ci.DateTimeFormat.Calendar = jp

        If Me.TxtDate.Text <> "" Then
            ' TextBoxのデータを、DateTime型に変換
            dt = TxtDate.Text

            ' 「書式」「カルチャの書式情報」を使用し、文字列に変換
            'Me.TxtDate.Text = dt.ToString("gg yy年 M月 d日", ci)
            Me.TxtDate.Text = dt.ToString(" yyyy年 M月 d日")
        End If

        '行番号設定
        Me.TxtRowNum.Text = rowNum
        rowNum += 1

        If Me.TxtWrkRslt.Text = "1" Then
            Me.TxtWrkRslt.Text = "ＯＫ"
        ElseIf Me.TxtWrkRslt.Text = "2" Then
            Me.TxtWrkRslt.Text = "ＮＧ"
        End If

    End Sub

    Private Sub PageHeader1_BeforePrint(sender As Object, e As EventArgs) Handles PageHeader1.BeforePrint
        Me.TxtCharge.Text = Me.TxtCharge.Text & "　様"
    End Sub

    Private Sub PageHeader1_Format(sender As Object, e As EventArgs) Handles PageHeader1.Format
        rowNum = 1
    End Sub
End Class
