Imports GrapeCity.ActiveReports
Imports GrapeCity.ActiveReports.Document
'Imports SPC.Global_asax
Imports GrapeCity.ActiveReports.SectionReportModel

Public Class CNSREP002

    Private Sub PageHeader1_BeforePrint(sender As Object, e As EventArgs) Handles PageHeader1.BeforePrint
        txtOffice_Nm.Text = txtOffice_Nm.Text & "　" & TextBox2.Text + "　様　宛"
    End Sub

    Private Sub PageHeader1_Format(sender As Object, e As EventArgs) Handles PageHeader1.Format
        '2枚目の時にNothingが渡ってくるので空文字を設定する
        If txtCnst_Cls1_val.Text Is Nothing Then txtCnst_Cls1_val.Text = ""
        If txtCnst_Cls2_val.Text Is Nothing Then txtCnst_Cls2_val.Text = ""
        If txtCnst_Cls3_val.Text Is Nothing Then txtCnst_Cls3_val.Text = ""
        If txtCnst_Cls4_val.Text Is Nothing Then txtCnst_Cls4_val.Text = ""
        If txtCnst_Cls5_val.Text Is Nothing Then txtCnst_Cls5_val.Text = ""
        If txtCnst_Cls6_val.Text Is Nothing Then txtCnst_Cls6_val.Text = ""
        txtCnst_Cls1_val.Text = mfGet_CONSTClsValue(txtCnst_Cls1_val.Text, ClsComVer.E_工事種別.新規)
        txtCnst_Cls2_val.Text = mfGet_CONSTClsValue(txtCnst_Cls2_val.Text, ClsComVer.E_工事種別.増設)
        txtCnst_Cls3_val.Text = mfGet_CONSTClsValue(txtCnst_Cls3_val.Text, ClsComVer.E_工事種別.再配置)
        txtCnst_Cls4_val.Text = mfGet_CONSTClsValue(txtCnst_Cls4_val.Text, ClsComVer.E_工事種別.移設)
        txtCnst_Cls5_val.Text = mfGet_CONSTClsValue(txtCnst_Cls5_val.Text, ClsComVer.E_工事種別.機種変更)
        txtCnst_Cls6_val.Text = mfGet_CONSTClsValue(txtCnst_Cls6_val.Text, ClsComVer.E_工事種別.その他)
    End Sub

    ''' <summary>
    ''' 工事種別の表示する値を返す。
    ''' </summary>
    ''' <param name="ipstrData">判別するデータ</param>
    ''' <param name="ipshtCount">判定する工事種別</param>
    ''' <returns>0以外:●, 他:空白</returns>
    ''' <remarks></remarks>
    Private Function mfGet_CONSTClsValue(ByVal ipstrData As String, ByVal ipshtCount As ClsComVer.E_工事種別) As String

        If txtBaseduty.Text = "工事用出荷" Then
            If (ipstrData.Length >= ipshtCount) Then
                If (ipstrData.Substring(ipshtCount - 1, 1) <> "0") Then
                    mfGet_CONSTClsValue = "●"
                Else
                    mfGet_CONSTClsValue = String.Empty
                End If
            Else
                mfGet_CONSTClsValue = String.Empty
            End If
        Else
            mfGet_CONSTClsValue = String.Empty
        End If

    End Function

    Private Sub PageFooter1_BeforePrint(sender As Object, e As EventArgs) Handles PageFooter1.BeforePrint
        If TextBox28.Text = "共通ユーザー 　" Then
            TextBox28.Text = String.Empty
        End If
    End Sub
End Class
