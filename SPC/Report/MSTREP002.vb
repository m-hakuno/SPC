Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class MSTREP002

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        Dim ds As DataTable = Me.DataSource
        If ds.Rows(0).Item("撤去区分").ToString = "2" Then
            '※一時撤去

            '再開日
            Me.TxtWrkEndDT.Text = "未定"
            Label3.Visible = False
            Label4.Visible = False

        ElseIf ds.Rows(0).Item("撤去区分").ToString = "1" Then
            '※全撤去

            'テキスト変更
            TxtStopReq.Text = "廃止依頼"
            TxtStopMng.Text = "廃止処理"
            TextBox31.Text = "最終集信日"
            TextBox32.Text = "運用終了日"
            TextBox46.Text = "廃止日"

            '罫線変更
            TextBox31.Border.Style = BorderLineStyle.Solid
            TextBox31.Border.BottomStyle = BorderLineStyle.None
            TxtLstClctDT.Border.Style = BorderLineStyle.Solid
            TxtLstClctDT.Border.LeftStyle = BorderLineStyle.None
            TxtLstClctDT.Border.BottomStyle = BorderLineStyle.None
            TextBox37.Border.Style = BorderLineStyle.Solid
            TextBox37.Border.BottomStyle = BorderLineStyle.None
            TextBox32.Border.Style = BorderLineStyle.ThickSolid
            TxtWrkEndDT.Border.Style = BorderLineStyle.ThickSolid
            TxtWrkEndDT.Border.LeftStyle = BorderLineStyle.None
            TxtWrkEndDT.Border.RightStyle = BorderLineStyle.Solid
            TextBox38.Border.Style = BorderLineStyle.ThickSolid
            TextBox38.Border.LeftStyle = BorderLineStyle.Solid

            TxtLstClctDT.Font = New System.Drawing.Font("MS UI Gothic", 19)
            TxtWrkEndDT.Font = New System.Drawing.Font("MS UI Gothic", 24)

            TxtRunCls.Text = TxtRunCls.Text.Replace("【03：一時停止】", "【09：廃止】")

        End If

    End Sub

End Class
