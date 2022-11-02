Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class REQREP001

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format
        '帳票最下部の備考
        'If Me.TxtREMARK.Text <> String.Empty Then
        '    Me.TxtREMARK.Text = "※ " & Me.TxtREMARK.Text
        'End If

        'TxtSEND_NM.Text = StrConv(TxtSEND_NM.Text, vbNarrow)

        '送付先１
        If Me.Fields("送付先１郵便番号").Value.ToString = "" _
                AndAlso Me.Fields("送付先１住所").Value.ToString = "" _
                AndAlso Me.Fields("送付先１名").Value.ToString = "" _
                AndAlso Me.Fields("送付先１気付").Value.ToString = "" _
                AndAlso Me.Fields("送付先１TEL").Value.ToString = "" _
                AndAlso Me.Fields("送付先１FAX").Value.ToString = "" _
                AndAlso Me.Fields("送付先１機器情報１").Value.ToString = "" _
                AndAlso Me.Fields("送付先１機器情報２").Value.ToString = "" Then
            Me.Label53.Visible = False '「故障機器送付先」
            Me.Label41.Visible = False '「〒」
            Me.Label54.Visible = False '「TEL」
            Me.Label55.Visible = False '「FAX」
        End If

        '送付先２
        If Me.Fields("送付先２郵便番号").Value.ToString = "" _
                AndAlso Me.Fields("送付先２住所").Value.ToString = "" _
                AndAlso Me.Fields("送付先２名").Value.ToString = "" _
                AndAlso Me.Fields("送付先２気付").Value.ToString = "" _
                AndAlso Me.Fields("送付先２TEL").Value.ToString = "" _
                AndAlso Me.Fields("送付先２FAX").Value.ToString = "" _
                AndAlso Me.Fields("送付先２機器情報１").Value.ToString = "" _
                AndAlso Me.Fields("送付先２機器情報２").Value.ToString = "" Then
            Me.Label56.Visible = False '「故障機器送付先」
            Me.Label57.Visible = False '「〒」
            Me.Label58.Visible = False '「TEL」
            Me.Label62.Visible = False '「FAX」
        End If

        '送付先３
        If Me.Fields("送付先３郵便番号").Value.ToString = "" _
                AndAlso Me.Fields("送付先３住所").Value.ToString = "" _
                AndAlso Me.Fields("送付先３名").Value.ToString = "" _
                AndAlso Me.Fields("送付先３気付").Value.ToString = "" _
                AndAlso Me.Fields("送付先３TEL").Value.ToString = "" _
                AndAlso Me.Fields("送付先３FAX").Value.ToString = "" _
                AndAlso Me.Fields("送付先３機器情報１").Value.ToString = "" _
                AndAlso Me.Fields("送付先３機器情報２").Value.ToString = "" Then
            Me.Label70.Visible = False '「故障機器送付先」
            Me.Label71.Visible = False '「〒」
            Me.Label72.Visible = False '「TEL」
            Me.Label73.Visible = False '「FAX」
        End If
    End Sub

    Private Sub REQREP001_ReportStart(sender As Object, e As EventArgs) Handles Me.ReportStart

        '和暦を表すクラス
        Dim ci As New System.Globalization.CultureInfo("ja-JP")
        Dim jp As New System.Globalization.JapaneseCalendar
        ci.DateTimeFormat.Calendar = jp

        'Me.TxtDATE.Text = System.DateTime.Now.ToString("ggyy年MM月dd日", ci)
        Me.TxtDATE.Text = System.DateTime.Now.ToString("yyyy年MM月dd日 ")

    End Sub
End Class
