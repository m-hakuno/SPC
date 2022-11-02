Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class CNSREP019

    Private Sub Detail1_BeforePrint(sender As Object, e As EventArgs) Handles Detail1.BeforePrint
        If Me.txtIND_CNST_CLS.Text = "" Then Me.txtIND_CNST_CLS2.Text = "＊" Else Me.txtIND_CNST_CLS2.Text = ""
        If Me.txtSYSSTK_CLS.Text = "" Then Me.txtSYSSTK_CLS2.Text = "＊" Else Me.txtSYSSTK_CLS2.Text = ""
        If Me.txtPC_FLG.Text = "" Then Me.txtPC_FLG2.Text = "＊" Else Me.txtPC_FLG2.Text = ""

        'CNSUPDP001_019
        '[印刷区分]フラグ有無の確認
        If HttpContext.Current.Session("印刷区分") = "1" Then
            txtPrtSetting.Text = "帳票設定要"
        Else
            txtPrtSetting.Text = "帳票設定不要"
        End If
        'CNSUPDP001_019 END
    End Sub
End Class
