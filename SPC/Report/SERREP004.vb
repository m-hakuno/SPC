Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class SERREP004

    '機器種別コード
    Const M_APPACLS_CTL = "01" '本体
    Const M_APPACLS_CRT = "02" 'ＬＣＤ
    Const M_APPACLS_KBD = "03" 'キーボード
    Const M_APPACLS_PRT = "04" 'プリンタ
    Const M_APPACLS_UPS = "06" 'ＵＰＳ

    Private Sub PageHeader1_Format(sender As Object, e As EventArgs) Handles PageHeader1.Format
        Dim ci As New System.Globalization.CultureInfo("ja-JP")
        Dim jp As New System.Globalization.JapaneseCalendar
        Dim dtime As System.DateTime


        ci.DateTimeFormat.Calendar = jp
        dtime = DateTime.Parse(System.DateTime.Now)
        lblHAN.Text = "作成日：" & dtime.ToString("yyyy/MM/dd")

        Dim dt As DataTable = Me.DataSource
        Dim strNotetext(4) As String    '備考配列（ 0:本体、1:ＬＣＤ、2:プリンタ、3:キーボード、4:ＵＰＳ ）

        '出力コントロールの初期化
        Me.msClearControl()

        'TxtSERIAL1.Text = dt.Rows(N).Item("シリアル").ToString.Substring(0, 1)
        'TxtSERIAL2.Text = dt.Rows(N).Item("シリアル").ToString.Substring(1, 1)
        'TxtSERIAL3.Text = dt.Rows(N).Item("シリアル").ToString.Substring(2, 1)
        'TxtSERIAL4.Text = dt.Rows(N).Item("シリアル").ToString.Substring(3, 1)
        'TxtSERIAL5.Text = dt.Rows(N).Item("シリアル").ToString.Substring(4, 1)
        'TxtSERIAL6.Text = dt.Rows(N).Item("シリアル").ToString.Substring(5, 1)
        'TxtSERIAL7.Text = dt.Rows(N).Item("シリアル").ToString.Substring(6, 1)
        'TxtSERIAL8.Text = dt.Rows(N).Item("シリアル").ToString.Substring(7, 1)
        'TxtSERIAL9.Text = dt.Rows(N).Item("シリアル").ToString.Substring(8, 1)
        'TxtSERIAL10.Text = dt.Rows(N).Item("シリアル").ToString.Substring(9, 1)
        'TxtSERIAL11.Text = dt.Rows(N).Item("シリアル").ToString.Substring(10, 1)
        'TxtSERIAL12.Text = dt.Rows(N).Item("シリアル").ToString.Substring(11, 1)
        'TxtSERIAL13.Text = dt.Rows(N).Item("シリアル").ToString.Substring(12, 1)
        'TxtSERIAL14.Text = dt.Rows(N).Item("シリアル").ToString.Substring(13, 1)
        'TxtSERIAL15.Text = dt.Rows(N).Item("シリアル").ToString.Substring(14, 1)
        'TxtSERIAL16.Text = dt.Rows(N).Item("シリアル").ToString.Substring(15, 1)
        'txtVERSION.Text = dt.Rows(N).Item("バージョン")
        'txtSYSTEM.Text = dt.Rows(N).Item("機種")
        'strNotetext(0) = dt.Rows(N).Item("備考")

        'TxtLCD1.Text = dt.Rows(N).Item("シリアル").ToString.Substring(0, 1)
        'TxtLCD2.Text = dt.Rows(N).Item("シリアル").ToString.Substring(1, 1)
        'TxtLCD3.Text = dt.Rows(N).Item("シリアル").ToString.Substring(2, 1)
        'TxtLCD4.Text = dt.Rows(N).Item("シリアル").ToString.Substring(3, 1)
        'TxtLCD5.Text = dt.Rows(N).Item("シリアル").ToString.Substring(4, 1)
        'TxtLCD6.Text = dt.Rows(N).Item("シリアル").ToString.Substring(5, 1)
        'TxtLCD7.Text = dt.Rows(N).Item("シリアル").ToString.Substring(6, 1)
        'TxtLCD8.Text = dt.Rows(N).Item("シリアル").ToString.Substring(7, 1)
        'TxtLCD9.Text = dt.Rows(N).Item("シリアル").ToString.Substring(8, 1)
        'strNotetext(1) = dt.Rows(N).Item("備考")

        'TxtPRI1.Text = dt.Rows(N).Item("シリアル").ToString.Substring(0, 1)
        'TxtPRI2.Text = dt.Rows(N).Item("シリアル").ToString.Substring(1, 1)
        'TxtPRI3.Text = dt.Rows(N).Item("シリアル").ToString.Substring(2, 1)
        'TxtPRI4.Text = dt.Rows(N).Item("シリアル").ToString.Substring(3, 1)
        'TxtPRI5.Text = dt.Rows(N).Item("シリアル").ToString.Substring(4, 1)
        'TxtPRI6.Text = dt.Rows(N).Item("シリアル").ToString.Substring(5, 1)
        'TxtPRI7.Text = dt.Rows(N).Item("シリアル").ToString.Substring(6, 1)
        'TxtPRI8.Text = dt.Rows(N).Item("シリアル").ToString.Substring(7, 1)
        'TxtPRI9.Text = dt.Rows(N).Item("シリアル").ToString.Substring(8, 1)
        'TxtPRI10.Text = dt.Rows(N).Item("シリアル").ToString.Substring(9, 1)
        'TxtPRI11.Text = dt.Rows(N).Item("シリアル").ToString.Substring(10, 1)
        'TxtPRI12.Text = dt.Rows(N).Item("シリアル").ToString.Substring(11, 1)
        'TxtPRI13.Text = dt.Rows(N).Item("シリアル").ToString.Substring(12, 1)
        'TxtPRI14.Text = dt.Rows(N).Item("シリアル").ToString.Substring(13, 1)
        'TxtPRI15.Text = dt.Rows(N).Item("シリアル").ToString.Substring(14, 1)
        'TxtPRI16.Text = dt.Rows(N).Item("シリアル").ToString.Substring(15, 1)
        'txtAPPA_NM.Text = dt.Rows(N).Item("型式")
        'strNotetext(2) = dt.Rows(N).Item("備考")

        'TxtKEY1.Text = dt.Rows(N).Item("シリアル").ToString.Substring(0, 1)
        'TxtKEY2.Text = dt.Rows(N).Item("シリアル").ToString.Substring(1, 1)
        'TxtKEY3.Text = dt.Rows(N).Item("シリアル").ToString.Substring(2, 1)
        'TxtKEY4.Text = dt.Rows(N).Item("シリアル").ToString.Substring(3, 1)
        'TxtKEY5.Text = dt.Rows(N).Item("シリアル").ToString.Substring(4, 1)
        'TxtKEY6.Text = dt.Rows(N).Item("シリアル").ToString.Substring(5, 1)
        'TxtKEY7.Text = dt.Rows(N).Item("シリアル").ToString.Substring(6, 1)
        'TxtKEY8.Text = dt.Rows(N).Item("シリアル").ToString.Substring(7, 1)
        'TxtKEY9.Text = dt.Rows(N).Item("シリアル").ToString.Substring(8, 1)
        'strNotetext(3) = dt.Rows(N).Item("備考")

        'TxtUPS1.Text = dt.Rows(N).Item("シリアル").ToString.Substring(0, 1)
        'TxtUPS2.Text = dt.Rows(N).Item("シリアル").ToString.Substring(1, 1)
        'TxtUPS3.Text = dt.Rows(N).Item("シリアル").ToString.Substring(2, 1)
        'TxtUPS4.Text = dt.Rows(N).Item("シリアル").ToString.Substring(3, 1)
        'TxtUPS5.Text = dt.Rows(N).Item("シリアル").ToString.Substring(4, 1)
        'TxtUPS6.Text = dt.Rows(N).Item("シリアル").ToString.Substring(5, 1)
        'TxtUPS7.Text = dt.Rows(N).Item("シリアル").ToString.Substring(6, 1)
        'TxtUPS8.Text = dt.Rows(N).Item("シリアル").ToString.Substring(7, 1)
        'TxtUPS9.Text = dt.Rows(N).Item("シリアル").ToString.Substring(8, 1)
        'TxtUPS10.Text = dt.Rows(N).Item("シリアル").ToString.Substring(9, 1)
        'strNotetext(4) = dt.Rows(N).Item("備考")

        '備考の出力
        'If strNotetext(0) <> String.Empty Then
        '    txtNOTETEXT.Text += strNotetext(0) + Environment.NewLine + Environment.NewLine
        'End If
        'If strNotetext(1) <> String.Empty Then
        '    txtNOTETEXT.Text += strNotetext(1) + Environment.NewLine + Environment.NewLine
        'End If
        'If strNotetext(2) <> String.Empty Then
        '    txtNOTETEXT.Text += strNotetext(2) + Environment.NewLine + Environment.NewLine
        'End If
        'If strNotetext(3) <> String.Empty Then
        '    txtNOTETEXT.Text += strNotetext(3) + Environment.NewLine + Environment.NewLine
        'End If
        'If strNotetext(4) <> String.Empty Then
        '    txtNOTETEXT.Text += strNotetext(4) + Environment.NewLine + Environment.NewLine
        'End If

    End Sub

    Private Sub msClearControl()

        Me.txtNL_CLS.Text = String.Empty
        Me.txtMOVE_REASON.Text = String.Empty
        Me.txtSET_DT.Text = String.Empty
        Me.lblCOMP_NM.Text = String.Empty
        Me.txtSTRAGE_NM.Text = String.Empty
        Me.TxtTBOXID1.Text = String.Empty
        Me.TxtTBOXID2.Text = String.Empty
        Me.TxtTBOXID3.Text = String.Empty
        Me.TxtTBOXID4.Text = String.Empty
        Me.TxtTBOXID5.Text = String.Empty
        Me.TxtTBOXID6.Text = String.Empty
        Me.TxtTBOXID7.Text = String.Empty
        Me.TxtTBOXID8.Text = String.Empty
        Me.txtVERSION.Text = String.Empty
        Me.txtSYSTEM.Text = String.Empty
        Me.TxtSERIAL1.Text = String.Empty
        Me.TxtSERIAL2.Text = String.Empty
        Me.TxtSERIAL3.Text = String.Empty
        Me.TxtSERIAL4.Text = String.Empty
        Me.TxtSERIAL5.Text = String.Empty
        Me.TxtSERIAL6.Text = String.Empty
        Me.TxtSERIAL7.Text = String.Empty
        Me.TxtSERIAL8.Text = String.Empty
        Me.TxtSERIAL9.Text = String.Empty
        Me.TxtSERIAL10.Text = String.Empty
        Me.TxtSERIAL11.Text = String.Empty
        Me.TxtSERIAL12.Text = String.Empty
        Me.TxtSERIAL13.Text = String.Empty
        Me.TxtSERIAL14.Text = String.Empty
        Me.TxtSERIAL15.Text = String.Empty
        Me.TxtSERIAL16.Text = String.Empty
        Me.TxtLCD1.Text = String.Empty
        Me.TxtLCD2.Text = String.Empty
        Me.TxtLCD3.Text = String.Empty
        Me.TxtLCD4.Text = String.Empty
        Me.TxtLCD5.Text = String.Empty
        Me.TxtLCD6.Text = String.Empty
        Me.TxtLCD7.Text = String.Empty
        Me.TxtLCD8.Text = String.Empty
        Me.TxtLCD9.Text = String.Empty
        Me.TxtPRI1.Text = String.Empty
        Me.TxtPRI2.Text = String.Empty
        Me.TxtPRI3.Text = String.Empty
        Me.TxtPRI4.Text = String.Empty
        Me.TxtPRI5.Text = String.Empty
        Me.TxtPRI6.Text = String.Empty
        Me.TxtPRI7.Text = String.Empty
        Me.TxtPRI8.Text = String.Empty
        Me.TxtPRI9.Text = String.Empty
        Me.TxtPRI10.Text = String.Empty
        Me.TxtPRI11.Text = String.Empty
        Me.TxtPRI12.Text = String.Empty
        Me.TxtPRI13.Text = String.Empty
        Me.TxtPRI14.Text = String.Empty
        Me.TxtPRI15.Text = String.Empty
        Me.TxtPRI16.Text = String.Empty
        Me.txtAPPA_NM.Text = String.Empty
        Me.TxtKEY1.Text = String.Empty
        Me.TxtKEY2.Text = String.Empty
        Me.TxtKEY3.Text = String.Empty
        Me.TxtKEY4.Text = String.Empty
        Me.TxtKEY5.Text = String.Empty
        Me.TxtKEY6.Text = String.Empty
        Me.TxtKEY7.Text = String.Empty
        Me.TxtKEY8.Text = String.Empty
        Me.TxtKEY9.Text = String.Empty
        Me.TxtUPS1.Text = String.Empty
        Me.TxtUPS2.Text = String.Empty
        Me.TxtUPS3.Text = String.Empty
        Me.TxtUPS4.Text = String.Empty
        Me.TxtUPS5.Text = String.Empty
        Me.TxtUPS6.Text = String.Empty
        Me.TxtUPS7.Text = String.Empty
        Me.TxtUPS8.Text = String.Empty
        Me.TxtUPS9.Text = String.Empty
        Me.TxtUPS10.Text = String.Empty
        Me.lblCOMP_NM2.Text = String.Empty
        Me.txtTEL_NO.Text = String.Empty
        Me.txtFAX_NO.Text = String.Empty
        Me.txtADDR.Text = String.Empty
        Me.txtNOTETEXT.Text = String.Empty

    End Sub

End Class
