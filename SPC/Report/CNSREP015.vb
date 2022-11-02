Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document
Imports System.Drawing

Public Class CNSREP015

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        '★その他内容の文字カット処理
        Dim g As Graphics = Graphics.FromImage(New Bitmap(100, 100))    'グラフィックオブジェクト
        Dim sglTxtBoxWidth As Single = TxtOthDtl.Width * 96 - 0.01      'テキストボックスの幅
        Dim sglTxtWidth As Single                                       'テキストの表示幅
        Dim strTexts() As String = TxtOthDtl.Text.Split(Microsoft.VisualBasic.vbLf)

        '工事/保守 判別
        Select Case TxtCnstNo.Text.Substring(0, 5)
            Case "N0010", "N0090"
                '工事
                sglTxtBoxWidth *= 2 'テキストボックスの幅を x2
                If TxtDLLCls.Text <> "" Then
                    sglTxtBoxWidth -= 40 '「特運無し」表示用の幅を確保
                End If
            Case Else
                '保守
                TxtOthDtl.Text = String.Empty 'テキストクリア
        End Select

        '行数分ループ
        For i As Integer = 0 To (strTexts.Count - 1)

            'テキストの表示幅取得
            sglTxtWidth = g.MeasureString(strTexts(i), TxtOthDtl.Font).Width

            '印字範囲に収まるまで文字カット
            While sglTxtWidth > sglTxtBoxWidth
                '1文字カット & 末尾「…」表示
                strTexts(i) = strTexts(i).Substring(0, strTexts(i).Length - 2) & "…"
                'テキストの表示幅取得
                sglTxtWidth = g.MeasureString(strTexts(i), TxtOthDtl.Font).Width
            End While
        Next

        '編集結果を反映
        Select Case TxtCnstNo.Text.Substring(0, 5)
            Case "N0010", "N0090"
                '工事
                TxtOthDtl.Text = (strTexts(0) & TxtDLLCls.Text).Trim
                '自動改行有効化
                TxtOthDtl.WrapMode = Section.WrapMode.CharWrap
            Case Else
                '保守
                For Each strText As String In strTexts
                    TxtOthDtl.Text &= (strText & Environment.NewLine)
                Next
                '自動改行無効化
                TxtOthDtl.WrapMode = Section.WrapMode.NoWrap
        End Select

    End Sub

End Class
