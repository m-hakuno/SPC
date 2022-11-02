Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class CNSREP009_1

    Dim counter As Integer = 0


    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        '段組改列
        counter = counter + 1
        If counter = 26 Then
            Me.Detail1.NewColumn = SectionReportModel.NewColumn.After
        Else
            Me.Detail1.NewColumn = SectionReportModel.NewColumn.None
        End If

        'メインタイトル部と項目部での表示制御
        Select Case Me.Fields(1).Value.ToString
            Case "99" 'メイン
                Me.TxtNAME.Visible = True
                Me.TextBox294.Visible = False
                Me.Label60.Visible = False
                Me.TxtQUANT.Visible = False
                Me.Label61.Visible = False
                Me.Label68.Visible = False
                Me.TxtAMOUNT.Visible = False
                Me.Label59.Visible = False
                Me.Label67.Visible = False
            Case "XX" '空行
                Me.TxtNAME.Visible = False
                Me.TextBox294.Visible = False
                Me.Label60.Visible = False
                Me.TxtQUANT.Visible = False
                Me.Label61.Visible = False
                Me.Label68.Visible = False
                Me.TxtAMOUNT.Visible = False
                Me.Label59.Visible = False
                Me.Label67.Visible = False
            Case Else '項目
                Me.TxtNAME.Visible = False
                Me.TextBox294.Visible = True
                Me.Label60.Visible = True
                Me.TxtQUANT.Visible = True
                Me.Label61.Visible = True
                Me.Label68.Visible = True
                Me.TxtAMOUNT.Visible = True
                Me.Label59.Visible = True
                Me.Label67.Visible = True
        End Select

        '-----------------------------
        '2014/04/17 土岐　ここから
        '-----------------------------
        ''303構成配信費の処理
        'If Me.Fields(2).Value.ToString = "303" Then
        '    If Me.Fields(4).Value.ToString <> String.Empty Then
        '        Me.TxtQUANT.Text = Math.Ceiling(Integer.Parse(Me.Fields(4).Value.ToString) / 10)
        '    End If
        'End If

        ''305派遣費の処理
        'If Me.Fields(2).Value.ToString = "305" Then
        '    If Me.Fields(4).Value.ToString <> String.Empty Then
        '        Me.TxtQUANT.Text = Math.Ceiling(Integer.Parse(Me.Fields(4).Value.ToString) / 250)
        '    End If
        'End If
        '-----------------------------
        '2014/04/17 土岐　ここまで
        '-----------------------------

    End Sub
End Class
