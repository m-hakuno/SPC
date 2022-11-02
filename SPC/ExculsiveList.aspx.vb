'********************************************************************************************************************************
'*　システム　：　サポートセンタステム <排他>
'*　処理名　　：　排他制御削除処理実行画面
'*　ＰＧＭＩＤ：　ExculsiveList
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.06.19　：　間瀬
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports System.IO
Imports System.String
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive

Public Class ExculsiveList
    Inherits System.Web.UI.Page

    Const M_MY_DISP_ID = "ExculsiveList"
    Const M_MY_DISP_TITLE = "排他制御一覧"

    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsExc As New ClsCMExclusive

    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, M_MY_DISP_ID)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim flag As String = 0                           'フラグ(0:排他情報 1:排他情報と明細情報あり)
        Dim ExculsiveDate As String = String.Empty       '排他情報
        Dim ExculsiveDateDtl As String = String.Empty    '排他情報(明細)


        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click

        AddHandler Master.Master.ppLeftButton1.Click, AddressOf btnAllSelect_Click
        AddHandler Master.Master.ppLeftButton2.Click, AddressOf btnSelectedDel_Click

        If Not IsPostBack Then  '初回表示
            '「クリア」「当日」ボタン押下時の検証を無効
            Master.ppRigthButton2.CausesValidation = False
            Master.ppRigthButton3.CausesValidation = False

            '「削除」のボタン活性
            Master.Master.ppLeftButton1.Visible = True
            Master.Master.ppLeftButton1.Text = "すべて選択"
            Master.Master.ppLeftButton2.Visible = True
            Master.Master.ppLeftButton2.Text = "選択行削除"

            Master.Master.ppProgramID = M_MY_DISP_ID
            Master.Master.ppTitle = M_MY_DISP_TITLE

            'パンくずリスト設定
            Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            'グリッドの初期化
            Me.grvList.DataSource = New DataTable

            Master.ppCount = "0"

            'ヘッダ表示
            Me.grvList.DataBind()

            'ドロップダウンリスト生成.
            Me.msGet_DropListData_Sel()

        End If

    End Sub

#Region "ドロップダウンリスト設定"
    ''' <summary>
    ''' ドロップダウンリスト設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_DropListData_Sel(Optional ByVal blnDisp As Boolean = True)

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim intIndex As Integer
        Dim intCount As Integer
        Dim strData As String

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ClsCMExclusive_S5", objCn)

                objCmd.Parameters.Add(pfSet_Param("prm_D85_USER_ID", SqlDbType.NVarChar, ""))
                objCmd.Parameters.Add(pfSet_Param("prm_D85_TERM_ID", SqlDbType.NVarChar, ""))
                objCmd.Parameters.Add(pfSet_Param("prm_D85_DISP_ID", SqlDbType.NVarChar, ""))
                objCmd.Parameters.Add(pfSet_Param("prm_D85_INSERT_DT_From", SqlDbType.NVarChar, ""))
                objCmd.Parameters.Add(pfSet_Param("prm_D85_INSERT_DT_To", SqlDbType.NVarChar, ""))
                objCmd.Parameters.Add(pfSet_Param("prm_D85_PLACE_CD", SqlDbType.NVarChar, ""))

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                ViewState("dataset") = objDs

                'ドロップダウンリスト設定（画面名）
                Me.ddlDisp.Items.Clear()
                Me.ddlDisp.Items.Insert(0, New ListItem(Nothing, Nothing))  '先頭に空白行を追加
                intCount = 1
                strData = ""
                For intIndex = 0 To objDs.Tables(0).Rows.Count - 1
                    With objDs.Tables(0).Rows(intIndex)
                        If strData <> .Item("画面ID").ToString Then
                            Me.ddlDisp.Items.Insert(intCount, New ListItem(.Item("画面名").ToString, .Item("画面ID").ToString))  '先頭に空白行を追加
                            strData = .Item("画面ID").ToString
                            intCount += 1
                        End If
                    End With
                Next

                'ドロップダウンリスト設定（IPアドレス）
                Me.ddlIp.Items.Clear()
                Me.ddlIp.Items.Insert(0, New ListItem(Nothing, Nothing))  '先頭に空白行を追加
                intCount = 1
                strData = ""
                For intIndex = 0 To objDs.Tables(0).Rows.Count - 1
                    With objDs.Tables(0).Rows(intIndex)
                        If strData <> .Item("IPアドレス").ToString Then
                            Me.ddlIp.Items.Insert(intCount, New ListItem(.Item("IPアドレス").ToString, .Item("IPアドレス").ToString))  '先頭に空白行を追加
                            strData = .Item("IPアドレス").ToString
                            intCount += 1
                        End If
                    End With
                Next

                If blnDisp = True Then
                    Call msDispData(objDs)
                End If

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "排他制御一覧取得")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub


    ''' <summary>
    ''' データ表示処理
    ''' </summary>
    ''' <param name="objDs"></param>
    ''' <remarks></remarks>
    Private Sub msDispData(objDs As DataSet)
        Dim intIndex As Integer

        Try
            For intIndex = 0 To objDs.Tables(0).Rows.Count - 1
                With objDs.Tables(0).Rows(intIndex)
                    Select Case .Item("場所コード").ToString
                        Case "1"
                            .Item("場所コード") = "ＳＰＣ"
                        Case "2"
                            .Item("場所コード") = "営業所"
                        Case "3"
                            .Item("場所コード") = "ＮＧＣ"
                    End Select
                End With

            Next


            'グリッド初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

            '件数を設定
            If objDs.Tables(0).Rows.Count = 0 Then
                psMesBox(Me, "00004", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後)

                '該当件数初期化
                Master.ppCount = "0"

                '検索結果CSVを非活性
                Master.Master.ppLeftButton2.Enabled = False
            Else
                Master.ppCount = objDs.Tables(0).Rows.Count.ToString

                '検索結果CSVを活性
                Master.Master.ppLeftButton2.Enabled = True
            End If

            '取得したデータをグリッドに設定
            Me.grvList.DataSource = objDs.Tables(0)

            '変更を反映
            Me.grvList.DataBind()

        Catch ex As Exception
            psMesBox(Me, "30010", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後,
                     ex.ToString.Replace(Environment.NewLine, "").Replace("'", ""))
        Finally
        End Try

    End Sub


    ''' <summary>
    ''' 検索ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
        Else
            Try

                '開始ログ出力
                psLogStart(Me)

                '画面ページ表示初期化
                Master.ppCount = "0"
                Me.grvList.DataSource = Nothing

                'データ取得
                If (Page.IsValid) Then
                    objCmd = New SqlCommand("ClsCMExclusive_S5", objCn)

                    objCmd.Parameters.Add(pfSet_Param("prm_D85_USER_ID", SqlDbType.NVarChar, Me.txtUserID.ppText))
                    objCmd.Parameters.Add(pfSet_Param("prm_D85_TERM_ID", SqlDbType.NVarChar, Me.ddlIp.SelectedValue))
                    objCmd.Parameters.Add(pfSet_Param("prm_D85_DISP_ID", SqlDbType.NVarChar, Me.ddlDisp.SelectedValue))
                    objCmd.Parameters.Add(pfSet_Param("prm_D85_INSERT_DT_From", SqlDbType.NVarChar, Me.dftSetDt.ppFromText))
                    objCmd.Parameters.Add(pfSet_Param("prm_D85_INSERT_DT_To", SqlDbType.NVarChar, Me.dftSetDt.ppToText))
                    objCmd.Parameters.Add(pfSet_Param("prm_D85_PLACE_CD", SqlDbType.NVarChar, Me.ddlPlace.SelectedValue))

                    'データ取得
                    objDs = clsDataConnect.pfGet_DataSet(objCmd)

                    Dim strDispValue As String = ddlDisp.SelectedValue
                    Dim strDisp As String = ddlDisp.SelectedItem.Text
                    Dim strIPValue As String = ddlIp.SelectedValue
                    Dim strIP As String = ddlIp.SelectedItem.Text

                    Call msGet_DropListData_Sel(False)
                    Call msDispData(objDs)
                    ViewState("dataset") = objDs

                    Try
                        ddlDisp.SelectedValue = strDispValue
                    Catch ex As Exception
                        psMesBox(Me, "00003", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "画面名：" & strDisp)
                    End Try
                    Try
                        ddlIp.SelectedValue = strIPValue
                    Catch ex As Exception
                        psMesBox(Me, "00003", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "IPアドレス：" & strIP)
                    End Try

                End If

                '終了ログ出力
                psLogEnd(Me)

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "排他制御一覧取得")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If
    End Sub

    ''' <summary>
    ''' 検索条件クリアボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        '画面クリア
        Me.txtUserID.ppText = ""
        Me.ddlDisp.SelectedIndex = 0
        Me.dftSetDt.ppFromText = ""
        Me.dftSetDt.ppToText = ""
        Me.ddlIp.SelectedIndex = 0
        Me.ddlPlace.SelectedIndex = 0

        '終了ログ出力
        psLogEnd(Me)

    End Sub


    ''' <summary>
    ''' 全選択ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAllSelect_Click(sender As Object, e As EventArgs)
        '開始ログ出力
        psLogStart(Me)

        For Each rowData As GridViewRow In grvList.Rows
            '検収チェックボックスON
            CType(rowData.FindControl("選択"), CheckBox).Checked = True
        Next

    End Sub


    ''' <summary>
    ''' 選択行削除ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSelectedDel_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        Dim ds As New DataSet
        ds = ViewState("dataset")

        For Each rowData As GridViewRow In grvList.Rows
            If CType(rowData.FindControl("選択"), CheckBox).Checked = True Then
                clsExc.pfDel_Exclusive_master(CType(rowData.FindControl("セッション"), TextBox).Text, CType(rowData.FindControl("排他日時"), TextBox).Text)
                For zz = 0 To grvList.Rows.Count - 1
                    If CType(grvList.Rows(zz).FindControl("排他日時"), TextBox).Text = CType(rowData.FindControl("排他日時"), TextBox).Text Then
                        ds.Tables(0).Rows(zz).Delete()
                    End If
                Next
            End If
        Next
        ds.AcceptChanges()
        grvList.DataSource = ds
        grvList.DataBind()

        Master.ppCount = grvList.Rows.Count

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 一覧の更新／参照／進捗画面ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        Try
            Select Case e.CommandName
                Case "btnDelete"
                    clsExc.pfDel_Exclusive_master(CType(grvList.Rows(Convert.ToInt32(e.CommandArgument)).FindControl("セッション"), TextBox).Text, CType(grvList.Rows(Convert.ToInt32(e.CommandArgument)).FindControl("排他日時"), TextBox).Text)
                    Dim ds As New DataSet
                    ds = ViewState("dataset")
                    For zz = 0 To grvList.Rows.Count - 1
                        If CType(grvList.Rows(zz).FindControl("排他日時"), TextBox).Text = CType(grvList.Rows(Convert.ToInt32(e.CommandArgument)).FindControl("排他日時"), TextBox).Text Then
                            ds.Tables(0).Rows(zz).Delete()
                        End If
                    Next
                    ds.AcceptChanges()
                    grvList.DataSource = ds
                    grvList.DataBind()

                    Master.ppCount = grvList.Rows.Count

            End Select
        Catch ex As Exception
            'ログ出力
            psMesBox(Me, "00004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "排他制御情報削除")
        End Try
    End Sub


#End Region
End Class