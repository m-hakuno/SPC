Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class MNTREP003

    '明細の項目[NO]用
    Dim strMkrCd As String = String.Empty
    Dim intCounter As Integer = 0

    '集計用
    Dim decWorkP As Decimal = 0
    Dim decSendP As Decimal = 0
    Dim decPartP As Decimal = 0
    Dim decPartP_Tanka As Decimal = 0
    Dim decPartP_Count As Decimal = 0
    Dim decSum As Decimal = 0

    Private Sub GroupHeader1_Format(sender As Object, e As EventArgs) Handles GroupHeader1.Format

        'If strMkrCd <> Me.Fields(0).Value.ToString And Me.Fields(0).Value.ToString <> "" Then
        '    intCounter = 0
        'End If
        'strMkrCd = Me.Fields(0).Value.ToString

        If Me.TextBox3.Text = String.Empty AndAlso Me.TextBox5.Text = String.Empty Then
            Me.Label4.Visible = False
        Else
            Me.Label4.Visible = True
        End If

    End Sub

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format
        intCounter = intCounter + 1
        'NO
        Me.TxtNO.Text = intCounter

        '---------------------------
        '2014/4/11 小守　ここから
        '---------------------------
        '作業費
        If Me.Fields(9).Value.ToString <> String.Empty Then
            decWorkP = decWorkP + Decimal.Parse(Me.Fields(9).Value.ToString)
        End If
        '運送費
        If Me.Fields(10).Value.ToString <> String.Empty Then
            decSendP = decSendP + Decimal.Parse(Me.Fields(10).Value.ToString)
        End If
        '部品単価
        If Me.Fields(12).Value.ToString <> String.Empty Then
            decPartP_Tanka = decPartP_Tanka + Decimal.Parse(Me.Fields(12).Value.ToString)
        End If
        '部品数量
        If Me.Fields(13).Value.ToString <> String.Empty Then
            decPartP_Count = decPartP_Count + Decimal.Parse(Me.Fields(13).Value.ToString)
        End If
        '部品費
        If Me.Fields(14).Value.ToString <> String.Empty Then
            decPartP = decPartP + Decimal.Parse(Me.Fields(14).Value.ToString)
        End If
        '合計
        If Me.Fields(15).Value.ToString <> String.Empty Then
            decSum = decSum + Decimal.Parse(Me.Fields(15).Value.ToString)
        End If

        ''作業費
        'If Me.Fields(18).Value.ToString <> String.Empty Then
        '    decWorkP = decWorkP + Decimal.Parse(Me.Fields(18).Value.ToString)
        'End If
        ''運送費
        'If Me.Fields(19).Value.ToString <> String.Empty Then
        '    decSendP = decSendP + Decimal.Parse(Me.Fields(19).Value.ToString)
        'End If
        ''部品単価
        'If Me.Fields(21).Value.ToString <> String.Empty Then
        '    decPartP_Tanka = decPartP_Tanka + Decimal.Parse(Me.Fields(21).Value.ToString)
        'End If
        ''部品数量
        'If Me.Fields(22).Value.ToString <> String.Empty Then
        '    decPartP_Count = decPartP_Count + Decimal.Parse(Me.Fields(22).Value.ToString)
        'End If
        ''部品費
        'If Me.Fields(23).Value.ToString <> String.Empty Then
        '    decPartP = decPartP + Decimal.Parse(Me.Fields(23).Value.ToString)
        'End If
        ''合計
        'If Me.Fields(24).Value.ToString <> String.Empty Then
        '    decSum = decSum + Decimal.Parse(Me.Fields(24).Value.ToString)
        'End If

        '---------------------------
        '2014/4/11 小守　ここまで
        '---------------------------

    End Sub

    Private Sub GroupFooter1_Format(sender As Object, e As EventArgs) Handles GroupFooter1.Format
        Me.TxtWORK_TTL.Text = decWorkP.ToString("#,##0")
        Me.TxtTRANS_TTL.Text = decSendP.ToString("#,##0")
        'Me.TxtPRICE_TTL.Text = decPartP_Tanka.ToString("#,##0")
        'Me.TxtQUANTITY_TTL.Text = decPartP_Count.ToString("#,##0")
        Me.TxtPART_PRICE_TTL.Text = decPartP.ToString("#,##0")
        Me.TxtTOTAL.Text = decSum.ToString("#,##0")

        decWorkP = 0
        decSendP = 0
        decPartP = 0
        decPartP_Tanka = 0
        decPartP_Count = 0
        decSum = 0
    End Sub

    Private Sub MNTREP003_ReportStart(sender As Object, e As EventArgs) Handles MyBase.ReportStart

    End Sub
End Class
