Imports GrapeCity.ActiveReports
Imports GrapeCity.ActiveReports.Document
Imports System.Drawing

Public Class TBRREP001
    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        '検知フラグ
        If txtCID_HCKENTIFLAG.Text = "0" Then
            txtCID_HCKENTIFLAG.Text = "未検知"
        ElseIf txtCID_HCKENTIFLAG.Text = "1" Then
            txtCID_HCKENTIFLAG.Text = "検知"
        Else
            txtCID_HCKENTIFLAG.Text = ""
        End If
        '正副(照会)
        If txtK_ERIACLASS.Text = "0" Then
            txtK_ERIACLASS.Text = "正"
        ElseIf txtK_ERIACLASS.Text = "1" Then
            txtK_ERIACLASS.Text = "副"
        Else
            txtK_ERIACLASS.Text = ""
        End If
        '正副(正)
        If txtS_ERIACLASS.Text = "0" Then
            txtS_ERIACLASS.Text = "正"
        ElseIf txtS_ERIACLASS.Text = "1" Then
            txtS_ERIACLASS.Text = "副"
        Else
            txtS_ERIACLASS.Text = ""
        End If
        '正副(副)
        If txtF_ERIACLASS.Text = "0" Then
            txtF_ERIACLASS.Text = "正"
        ElseIf txtF_ERIACLASS.Text = "1" Then
            txtF_ERIACLASS.Text = "副"
        Else
            txtF_ERIACLASS.Text = ""
        End If

    End Sub

End Class
