Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class ETCREP021

    Private Sub ETCREP021_ReportStart(sender As Object, e As EventArgs) Handles Me.ReportStart

        'TxtFROM_DT.Text = HttpContext.Current.Session("SupportFrom")
        'TxtTO_DT.Text = HttpContext.Current.Session("SupportTo")

        'If TxtTO_DT.Text = String.Empty Then
        '    TxtKARA.Text = String.Empty
        'End If

    End Sub

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        'カッコで囲む
        '-- TBOXID
        TxtTBOXID.Text = "(" + TxtTBOXID.Text + ")"
        '-- 管理番号
        TxtMEETING_NO.Text = "(" + TxtMEETING_NO.Text + ")"

    End Sub

End Class