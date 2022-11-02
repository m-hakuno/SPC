Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class CNSREP009_2

    Dim counter As Integer = 0

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        counter = counter + 1
        If counter = 12 Then
            Me.Detail1.NewColumn = SectionReportModel.NewColumn.After
        Else
            Me.Detail1.NewColumn = SectionReportModel.NewColumn.None
        End If

        'データが"0"の場合は表示しない
        If Me.Fields(3).Value.ToString = "0" Then
            Me.TxtBEF_CONST.Text = String.Empty
        End If
        If Me.Fields(4).Value.ToString = "0" Then
            Me.TxtRELOCATE.Text = String.Empty
        End If
        If Me.Fields(5).Value.ToString = "0" Then
            Me.TxtREMOVE.Text = String.Empty
        End If
        If Me.Fields(6).Value.ToString = "0" Then
            Me.TxtNEW.Text = String.Empty
        End If
        If Me.Fields(7).Value.ToString = "0" Then
            Me.TxtREMOVE2.Text = String.Empty
        End If
        If Me.Fields(8).Value.ToString = "0" Then
            Me.TxtAFTER_CONST.Text = String.Empty
        End If

        If counter = 25 Then
            '空行の表示制御
            Me.TxtNAME.Visible = False
            Me.Label125.Visible = False
            Me.TxtBEF_CONST.Visible = False
            Me.Label128.Visible = False
            Me.Label133.Visible = False
            Me.TxtRELOCATE.Visible = False
            Me.Label136.Visible = False
            Me.Label143.Visible = False
            Me.TxtREMOVE.Visible = False
            Me.Label146.Visible = False
            Me.Label147.Visible = False
            Me.TxtNEW.Visible = False
            Me.Label153.Visible = False
            Me.Label154.Visible = False
            Me.TxtREMOVE2.Visible = False
            Me.Label325.Visible = False
            Me.Label326.Visible = False
            Me.TxtAFTER_CONST.Visible = False
            Me.Label327.Visible = False
        Else
            Me.TxtNAME.Visible = True
            Me.Label125.Visible = True
            Me.TxtBEF_CONST.Visible = True
            Me.Label128.Visible = True
            Me.Label133.Visible = True
            Me.TxtRELOCATE.Visible = True
            Me.Label136.Visible = True
            Me.Label143.Visible = True
            Me.TxtREMOVE.Visible = True
            Me.Label146.Visible = True
            Me.Label147.Visible = True
            Me.TxtNEW.Visible = True
            Me.Label153.Visible = True
            Me.Label154.Visible = True
            Me.TxtREMOVE2.Visible = True
            Me.Label325.Visible = True
            Me.Label326.Visible = True
            Me.TxtAFTER_CONST.Visible = True
            Me.Label327.Visible = True
        End If

        'データを表示しない項目の値処理
        Select Case counter
            Case 19, 20, 22, 24
                Me.TxtBEF_CONST.Text = "-"
                Me.TxtRELOCATE.Text = "-"
                Me.TxtREMOVE.Text = "-"
                Me.TxtREMOVE2.Text = "-"
                Me.TxtAFTER_CONST.Text = "-"
                Me.TxtBEF_CONST.Alignment = Section.TextAlignment.Center
                Me.TxtRELOCATE.Alignment = Section.TextAlignment.Center
                Me.TxtREMOVE.Alignment = Section.TextAlignment.Center
                Me.TxtNEW.Alignment = Section.TextAlignment.Right
                Me.TxtREMOVE2.Alignment = Section.TextAlignment.Center
                Me.TxtAFTER_CONST.Alignment = Section.TextAlignment.Center
            Case 26, 27, 28, 29, 30
                Me.TxtNAME.Alignment = Section.TextAlignment.Right
                Me.TxtREMOVE2.Text = "-"
                Me.TxtREMOVE2.Alignment = Section.TextAlignment.Center
            Case Else
                Me.TxtBEF_CONST.Alignment = Section.TextAlignment.Right
                Me.TxtRELOCATE.Alignment = Section.TextAlignment.Right
                Me.TxtREMOVE.Alignment = Section.TextAlignment.Right
                Me.TxtNEW.Alignment = Section.TextAlignment.Right
                Me.TxtREMOVE2.Alignment = Section.TextAlignment.Right
                Me.TxtAFTER_CONST.Alignment = Section.TextAlignment.Right
        End Select

        '2) の処理
        If Me.Fields(0).Value.ToString = "2" Then
            If Me.Fields(10).Value.ToString = "1" Then
                'システム分類がIDのときそのまま
                Me.TxtNAME.Text = Me.Fields(2).Value.ToString
            Else
                'IDじゃなければTBOX機種名に置き換える
                Me.TxtNAME.Text = Me.Fields(2).Value.ToString.Replace("TBOX7XX", Me.Fields(9).Value.ToString)
            End If
        End If

        '5) の処理
        If Me.Fields(0).Value.ToString = "5" Then
            'TBOX機種名が"IT100"のときの処理
            If Me.Fields(9).Value.ToString = "IT100" Then
                Me.TxtNAME.Text.Replace("卓上", Me.Fields(9).Value.ToString)
            End If
        End If

    End Sub
End Class
