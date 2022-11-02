Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class DLCREP001

    Dim intNo As Integer = 0

    Private Sub Detail1_BeforePrint(sender As Object, e As EventArgs) Handles Detail1.BeforePrint

        '項番の設定
        intNo += 1
        txtNo.Text = intNo.ToString()

        If intNo <= Me.DataSource.rows.count Then
            'NLの区別
            If Me.DataSource.rows(intNo - 1).item("区分").ToString() = "L" Then
                TxtD47_NL_FLG.BackColor = System.Drawing.Color.FromArgb(255, 200, 200, 200)
                TxtD47_NL_FLG.Font = New System.Drawing.Font("MS UI Gothic", 8, System.Drawing.FontStyle.Bold)
            Else
                TxtD47_NL_FLG.BackColor = System.Drawing.Color.White
                TxtD47_NL_FLG.Font = New System.Drawing.Font("MS UI Gothic", 8, System.Drawing.FontStyle.Regular)
            End If

            '作業内容の区別
            Select Case Me.DataSource.rows(intNo - 1).item("作業内容コード").ToString()
                Case "04" '玉単価
                    TxtD47_SET_NM.BackColor = System.Drawing.Color.FromArgb(255, 200, 200, 200)
                    TxtD47_SET_NM.Font = New System.Drawing.Font("MS UI Gothic", 8, System.Drawing.FontStyle.Underline)
                Case "12" '中古機器設置他店取込設定
                    TxtD47_SET_NM.BackColor = System.Drawing.Color.FromArgb(255, 200, 200, 200)
                    TxtD47_SET_NM.Font = New System.Drawing.Font("MS UI Gothic", 8, System.Drawing.FontStyle.Regular)
                Case "14" '【IC】追加入金許可設定
                    TxtD47_SET_NM.BackColor = System.Drawing.Color.FromArgb(255, 200, 200, 200)
                    TxtD47_SET_NM.Font = New System.Drawing.Font("MS UI Gothic", 8, System.Drawing.FontStyle.Italic Or System.Drawing.FontStyle.Bold)
                Case "15" '【LUT】追加入金許可設定
                    TxtD47_SET_NM.BackColor = System.Drawing.Color.FromArgb(255, 200, 200, 200)
                    TxtD47_SET_NM.Font = New System.Drawing.Font("MS UI Gothic", 8, System.Drawing.FontStyle.Italic Or System.Drawing.FontStyle.Bold)
                Case Else
                    TxtD47_SET_NM.BackColor = System.Drawing.Color.White
                    TxtD47_SET_NM.Font = New System.Drawing.Font("MS UI Gothic", 8, System.Drawing.FontStyle.Regular)
            End Select

            'システムの区別
            If Me.DataSource.rows(intNo - 1).item("種別").ToString() = "NVC" Then
                TxtT03_TBOXCLASS_CD.BackColor = System.Drawing.Color.FromArgb(255, 200, 200, 200)
            Else
                TxtT03_TBOXCLASS_CD.BackColor = System.Drawing.Color.White
            End If

            '設定状況の区別y
            Select Case Me.DataSource.rows(intNo - 1).item("設定状況").ToString()
                Case "未作業"
                    TxtChgSts.BackColor = System.Drawing.Color.White
                Case "作業なし"
                    TxtChgSts.BackColor = System.Drawing.Color.White
                Case Else
                    TxtChgSts.BackColor = System.Drawing.Color.FromArgb(255, 200, 200, 200)
            End Select

            '戻し状況の区別
            Select Case Me.DataSource.rows(intNo - 1).item("戻し状況").ToString()
                Case "未作業"
                    TxtRtnSts.BackColor = System.Drawing.Color.White
                Case "作業なし"
                    TxtRtnSts.BackColor = System.Drawing.Color.White
                Case Else
                    TxtRtnSts.BackColor = System.Drawing.Color.FromArgb(255, 200, 200, 200)
            End Select
        Else
            TxtD47_NL_FLG.BackColor = System.Drawing.Color.White
            TxtD47_SET_NM.BackColor = System.Drawing.Color.White
            TxtT03_TBOXCLASS_CD.BackColor = System.Drawing.Color.White
            TxtRtnSts.BackColor = System.Drawing.Color.White
            TxtChgSts.BackColor = System.Drawing.Color.White
        End If
    End Sub

    Private Sub PageFooter1_Format(sender As Object, e As EventArgs) Handles PageFooter1.Format
        TextBox37.Font = New System.Drawing.Font("MS UI Gothic", 9, System.Drawing.FontStyle.Underline)
    End Sub
End Class
