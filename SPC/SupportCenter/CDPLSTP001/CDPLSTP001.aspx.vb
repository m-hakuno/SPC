'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　使用中カードＤＢ吸上一覧
'*　ＰＧＭＩＤ：　CDPLSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　14.01.24　：　(NKC)浜本
'*  修　正　　：　16.08.17　：　伯野　　　登録／更新／削除ボタン押下時に検索条件をクリアしているバグを修正
'********************************************************************************************************************************
'CDPLSTP001-001　2016/08/17　登録／更新／削除ボタン押下時に検索条件をクリアしているバグを修正
'                          　検索結果が０件の場合、"対象データがありません"というメッセージを表示する

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax
Imports SPC.ClsCMDataConnect
Imports System.Data.SqlClient
Imports SPC.ClsCMExclusive
#End Region

Public Class CDPLSTP001

#Region "継承定義"
    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
    Inherits System.Web.UI.Page
#End Region

#Region "定数定義"
    '============================================================================================================================
    '=　定数定義
    '============================================================================================================================

    ''' <summary>
    ''' 帳票名(日本語箇所)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strCHOHYO_NM_PART_JP As String = "カード情報吸い上げTBOX作業報告書兼受領書"

#End Region

#Region "構造体・列挙体定義"
    '============================================================================================================================
    '=　構造体・列挙体定義
    '============================================================================================================================
#End Region

#Region "変数定義"
    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================

    ''' <summary>
    ''' プログラムＩＤ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const M_MY_DISP_ID As String = P_FUN_CDP & P_SCR_LST & P_PAGE & "001"
    '--------------------------------
    '2014/04/14 星野　ここから
    '--------------------------------
    Dim objStack As StackFrame
    '--------------------------------
    '2014/04/14 星野　ここまで
    '--------------------------------
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom
    Dim clsExc As New ClsCMExclusive

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' 初期化イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, M_MY_DISP_ID)

    End Sub

    ''' <summary>
    ''' ページロード
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'マスタページのボタン項目に対し、イベントの付加
        msAddEventHandlerParentKmk()

        'CDPLSTP001-001
        ViewState("txtSrcTBoxFrToFrom") = txtSrcTBoxFrTo.ppFromText               'TBOXID_FROM
        ViewState("txtSrcTBoxFrToTo") = txtSrcTBoxFrTo.ppToText                   'TBOXID_TO
        ViewState("txtSrcKanriNoFrToFrom") = txtSrcKanriNoFrTo.ppFromText         '管理番号_FROM
        ViewState("txtSrcKanriNoFrToTo") = txtSrcKanriNoFrTo.ppToText             '管理番号_TO
        ViewState("txtSrcNoFrToFrom") = txtSrcNoFrTo.ppFromText                   'NO_FROM
        ViewState("txtSrcNoFrToTo") = txtSrcNoFrTo.ppToText                       'NO_TO
        ViewState("txtSrcJuryoFrToFrom") = txtSrcJuryoFrTo.ppFromText             '受領_FROM
        ViewState("txtSrcJuryoFrToTo") = txtSrcJuryoFrTo.ppToText                 '受領_TO
        ViewState("txtSrcKensyuMonthFrToFrom") = txtSrcKensyuMonthFrTo.ppFromText '検収月_FROM
        ViewState("txtSrcKensyuMonthFrToTo") = txtSrcKensyuMonthFrTo.ppToText     '検収月_TO
        'CDPLSTP001-001

        txtKensyuMonth.ppTextBox.AutoPostBack = True

        'ポストバック
        If Not Me.IsPostBack Then

            ViewState(P_KEY) = Session(P_KEY)
            ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)
            ViewState(P_SESSION_USERID) = Session(P_SESSION_USERID)
            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            '開始ログ出力
            psLogStart(Me)

            '検索ボタンに対しチェックグループ設定
            Master.ppRigthButton1.ValidationGroup = "Detail"

            '「検索条件クリア」ボタン押下時の検証を無効
            Master.ppRigthButton2.CausesValidation = False

            '「印刷」ボタン押下時の検証を無効
            Master.Master.ppRigthButton1.CausesValidation = False

            '「ＣＳＶ」ボタン押下時の検証を無効
            Master.Master.ppRigthButton2.CausesValidation = False

            'プログラムID、画面名設定
            Master.Master.ppProgramID = M_MY_DISP_ID
            Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)

            'パンくずリスト設定
            Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

            '「印刷」、「CSV」ボタン活性
            Master.Master.ppRigthButton1.Text = "ＣＳＶ"
            Master.Master.ppRigthButton1.Visible = True
            Master.Master.ppRigthButton2.Text = P_BTN_NM_PRI
            Master.Master.ppRigthButton2.Visible = True

            Master.Master.ppRigthButton1.Enabled = False
            Master.Master.ppRigthButton2.Enabled = False
            btnDetailUpdate.Enabled = False
            btnDetailDelete.Enabled = False
            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            '画面項目初期化
            Me.msPageClear()

            '★排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
            If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then

                Me.Master.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)

            End If

            'モードによる、項目のEnable制御をおこなう
            Me.msPageEnableSet(ViewState(P_SESSION_TERMS))

            '終了ログ出力
            psLogEnd(Me)

        End If

    End Sub

    '---------------------------
    '2014/04/16 武 ここから
    '---------------------------
    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        'CDPLSTP001-001
        'TBOXID_FROM
        If ViewState("txtSrcTBoxFrToFrom") Is Nothing Then
            txtSrcTBoxFrTo.ppFromText = ""
        Else
            txtSrcTBoxFrTo.ppFromText = ViewState("txtSrcTBoxFrToFrom")
        End If
        'TBOXID_TO
        If ViewState("txtSrcTBoxFrToTo") Is Nothing Then
            txtSrcTBoxFrTo.ppToText = ""
        Else
            txtSrcTBoxFrTo.ppToText = ViewState("txtSrcTBoxFrToTo")
        End If
        '管理番号_FROM
        If ViewState("txtSrcKanriNoFrToFrom") Is Nothing Then
            txtSrcKanriNoFrTo.ppFromText = ""
        Else
            txtSrcKanriNoFrTo.ppFromText = ViewState("txtSrcKanriNoFrToFrom")
        End If
        '管理番号_TO
        If ViewState("txtSrcKanriNoFrToTo") Is Nothing Then
            txtSrcKanriNoFrTo.ppToText = ""
        Else
            txtSrcKanriNoFrTo.ppToText = ViewState("txtSrcKanriNoFrToTo")
        End If
        'NO_FROM
        If ViewState("txtSrcNoFrToFrom") Is Nothing Then
            txtSrcNoFrTo.ppFromText = ""
        Else
            txtSrcNoFrTo.ppFromText = ViewState("txtSrcNoFrToFrom")
        End If
        'NO_TO
        If ViewState("txtSrcNoFrToTo") Is Nothing Then
            txtSrcNoFrTo.ppToText = ""
        Else
            txtSrcNoFrTo.ppToText = ViewState("txtSrcNoFrToTo")
        End If
        '受領_FROM
        If ViewState("txtSrcJuryoFrToFrom") Is Nothing Then
            txtSrcJuryoFrTo.ppFromText = ""
        Else
            txtSrcJuryoFrTo.ppFromText = ViewState("txtSrcJuryoFrToFrom")
        End If
        '受領_TO
        If ViewState("txtSrcJuryoFrToTo") Is Nothing Then
            txtSrcJuryoFrTo.ppToText = ""
        Else
            txtSrcJuryoFrTo.ppToText = ViewState("txtSrcJuryoFrToTo")
        End If
        '検収月_FROM
        If ViewState("txtSrcKensyuMonthFrToFrom") Is Nothing Then
            txtSrcKensyuMonthFrTo.ppFromText = ""
        Else
            txtSrcKensyuMonthFrTo.ppFromText = ViewState("txtSrcKensyuMonthFrToFrom")
        End If
        '検収月_TO
        If ViewState("txtSrcKensyuMonthFrToTo") Is Nothing Then
            txtSrcKensyuMonthFrTo.ppToText = ""
        Else
            txtSrcKensyuMonthFrTo.ppToText = ViewState("txtSrcKensyuMonthFrToTo")
        End If
        'CDPLSTP001-001

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
                btnDetailInsert.Enabled = False
                btnDetailUpdate.Enabled = False
                btnDetailDelete.Enabled = False
            Case "NGC"
        End Select

    End Sub
    '---------------------------
    '2014/04/16 武 ここまで
    '---------------------------

    ''' <summary>
    ''' 検索ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.Object)

        '開始ログ出力
        psLogStart(Me)

        'データ検索
        msDataRead()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 検索条件クリアボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.Object)
        '開始ログ出力
        psLogStart(Me)

        '検索エリアクリア
        msPageClear_Search()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 登録ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDetailInsert_Click(ByVal sender As Object, ByVal e As System.Object) Handles btnDetailInsert.Click

        '新規でない場合は処理を抜ける
        If lblNo.Text.Trim() <> "" Then
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
            Return
        End If

        '開始ログ出力
        psLogStart(Me)

        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        Dim objTran As SqlTransaction = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '項目チェック
        If Page.IsValid() = False Then
            Return
        End If

        Try
            'DB接続
            If clsDataConnect.pfOpen_Database(objCon) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Return
            End If

            objCmd = New SqlCommand("CDPLSTP001_S2")
            objCmd.Connection = objCon
            objCmd.CommandType = CommandType.StoredProcedure

            Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

            If objDs Is Nothing OrElse objDs.Tables.Count = 0 OrElse objDs.Tables(0).Rows.Count = 0 Then
                Throw New Exception()
            End If

            objTran = objCon.BeginTransaction()

            objCmd = New SqlCommand("CDPLSTP001_U1")

            objCmd.Connection = objCon

            objCmd.CommandType = CommandType.StoredProcedure

            objCmd.Transaction = objTran

            'パラメータ設定
            With objCmd.Parameters
                .Add(pfSet_Param("prmD22_REGIST_SEQ", SqlDbType.Int, objDs.Tables(0).Rows(0)("MAXNO")))
                .Add(pfSet_Param("prmD22_NL_CLS", SqlDbType.NVarChar, hdnNLKbn.Value))
                .Add(pfSet_Param("prmD22_CNTL_NO", SqlDbType.Int, txtKanriNo.ppText))
                .Add(pfSet_Param("prmD22_TBOXID", SqlDbType.NVarChar, txtTBoxId.ppText))
                .Add(pfSet_Param("prmD22_HALL_NM", SqlDbType.NVarChar, lblHallNm.Text))
                .Add(pfSet_Param("prmD22_CLASS", SqlDbType.NVarChar, lblSyubetu.Text))
                .Add(pfSet_Param("prmD22_VERSION", SqlDbType.NVarChar, lblVersion.Text))
                .Add(pfSet_Param("prmD22_SERIAL_NO", SqlDbType.NVarChar, txtSerialNo.ppText))
                .Add(pfSet_Param("prmD22_WRK_RSLT", SqlDbType.NVarChar, ddlSagyoKekka.ppSelectedValue))
                .Add(pfSet_Param("prmD22_RCV_DT", SqlDbType.DateTime, mfGetDBNull(txtJuryo.ppText)))
                .Add(pfSet_Param("prmD22_WRK_DT", SqlDbType.DateTime, mfGetDBNull(txtSagyoJissi.ppText)))
                .Add(pfSet_Param("prmD22_SEND_DT", SqlDbType.DateTime, mfGetDBNull(txtBaitaiSofu.ppText)))
                .Add(pfSet_Param("prmD22_SNDRTN_DT", SqlDbType.DateTime, mfGetDBNull(txtHenkyakuHasso.ppText)))
                .Add(pfSet_Param("prmD22_DLVRTN_DT", SqlDbType.DateTime, mfGetDBNull(txtHenkyakuNonyu.ppText)))
                .Add(pfSet_Param("prmD22_INSPECT_MNT", SqlDbType.NVarChar, txtKensyuMonth.ppText))
                .Add(pfSet_Param("prmD22_DELETE_FLG", SqlDbType.NVarChar, 0))
                .Add(pfSet_Param("prmD22_INSERT_DT", SqlDbType.DateTime, DateTime.Now))
                .Add(pfSet_Param("prmD22_INSERT_USR", SqlDbType.NVarChar, ViewState(P_SESSION_USERID)))
                If chkRequest.Checked = True Then
                    .Add(pfSet_Param("prmD22_REQUEST", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("prmD22_REQUEST", SqlDbType.NVarChar, "0"))
                End If
            End With

            'SQL実行
            Dim intRet As Integer = objCmd.ExecuteNonQuery()

            '登録エラー？
            If intRet <= 0 Then
                Throw New Exception()
            End If

            'コミット
            objTran.Commit()

            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "画面内容")

            msPageClear()

            '再読み込み
            msDataRead()

        Catch ex As Exception
            If Not objTran Is Nothing Then
                'ロールバック
                objTran.Rollback()
            End If

            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "画面内容")
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------

        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(objCon) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 更新ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDetailUpdate_Click(ByVal sender As Object, ByVal e As System.Object) Handles btnDetailUpdate.Click

        '新規データならばなにもしない
        If lblNo.Text.Trim() = "" Then
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
            Return
        End If


        '開始ログ出力
        psLogStart(Me)

        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        Dim objTran As SqlTransaction = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '項目チェック
        If Page.IsValid() = False Then
            Return
        End If

        Try
            'DB接続
            If clsDataConnect.pfOpen_Database(objCon) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Return
            End If
            objTran = objCon.BeginTransaction()

            objCmd = New SqlCommand("CDPLSTP001_U2")

            objCmd.Connection = objCon

            objCmd.CommandType = CommandType.StoredProcedure

            objCmd.Transaction = objTran

            'パラメータ設定
            With objCmd.Parameters
                .Add(pfSet_Param("prmD22_REGIST_SEQ", SqlDbType.Int, lblNo.Text))
                .Add(pfSet_Param("prmD22_NL_CLS", SqlDbType.NVarChar, hdnNLKbn.Value))
                .Add(pfSet_Param("prmD22_CNTL_NO", SqlDbType.Int, txtKanriNo.ppText))
                .Add(pfSet_Param("prmD22_TBOXID", SqlDbType.NVarChar, txtTBoxId.ppText))
                .Add(pfSet_Param("prmD22_HALL_NM", SqlDbType.NVarChar, lblHallNm.Text))
                .Add(pfSet_Param("prmD22_CLASS", SqlDbType.NVarChar, lblSyubetu.Text))
                .Add(pfSet_Param("prmD22_VERSION", SqlDbType.NVarChar, lblVersion.Text))
                .Add(pfSet_Param("prmD22_SERIAL_NO", SqlDbType.NVarChar, txtSerialNo.ppText))
                .Add(pfSet_Param("prmD22_WRK_RSLT", SqlDbType.NVarChar, ddlSagyoKekka.ppSelectedValue))
                .Add(pfSet_Param("prmD22_RCV_DT", SqlDbType.DateTime, mfGetDBNull(txtJuryo.ppText)))
                .Add(pfSet_Param("prmD22_WRK_DT", SqlDbType.DateTime, mfGetDBNull(txtSagyoJissi.ppText)))
                .Add(pfSet_Param("prmD22_SEND_DT", SqlDbType.DateTime, mfGetDBNull(txtBaitaiSofu.ppText)))
                .Add(pfSet_Param("prmD22_SNDRTN_DT", SqlDbType.DateTime, mfGetDBNull(txtHenkyakuHasso.ppText)))
                .Add(pfSet_Param("prmD22_DLVRTN_DT", SqlDbType.DateTime, mfGetDBNull(txtHenkyakuNonyu.ppText)))
                .Add(pfSet_Param("prmD22_INSPECT_MNT", SqlDbType.NVarChar, txtKensyuMonth.ppText))
                .Add(pfSet_Param("prmD22_UPDATE_DT", SqlDbType.DateTime, DateTime.Now))
                .Add(pfSet_Param("prmD22_UPDATE_USR", SqlDbType.NVarChar, ViewState(P_SESSION_USERID)))
                If chkRequest.Checked = True Then
                    .Add(pfSet_Param("prmD22_REQUEST", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("prmD22_REQUEST", SqlDbType.NVarChar, "0"))
                End If

            End With

            'SQL実行
            Dim intRet As Integer = objCmd.ExecuteNonQuery()

            '更新エラー？
            If intRet <= 0 Then
                Throw New Exception()
            End If

            'コミット
            objTran.Commit()

            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "画面内容")

            '★排他情報削除
            If Not Me.Master.Master.ppExclusiveDateDtl = String.Empty Then

                clsExc.pfDel_Exclusive(Me _
                              , Session(P_SESSION_SESSTION_ID) _
                              , Me.Master.Master.ppExclusiveDateDtl)

                Me.Master.Master.ppExclusiveDateDtl = String.Empty

            End If

            msPageClear()

            '再読み込み
            msDataRead()

        Catch ex As Exception
            If Not objTran Is Nothing Then
                'ロールバック
                objTran.Rollback()
            End If

            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "画面内容")
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------

        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(objCon) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub

    ''' <summary>
    ''' 削除ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDetailDelete_Click(ByVal sender As Object, ByVal e As System.Object) Handles btnDetailDelete.Click

        '新規データならばなにもしない
        If lblNo.Text.Trim() = "" Then
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
            Return
        End If

        '開始ログ出力
        psLogStart(Me)

        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        Dim objTran As SqlTransaction = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'DB接続
            If clsDataConnect.pfOpen_Database(objCon) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Return
            End If

            objTran = objCon.BeginTransaction()

            objCmd = New SqlCommand("CDPLSTP001_U3")

            objCmd.Connection = objCon

            objCmd.CommandType = CommandType.StoredProcedure

            objCmd.Transaction = objTran

            'パラメータ設定
            With objCmd.Parameters
                .Add(pfSet_Param("prmD22_REGIST_SEQ", SqlDbType.NVarChar, lblNo.Text)) 'No
            End With

            'SQL実行
            Dim intRet As Integer = objCmd.ExecuteNonQuery()

            '削除エラー？
            If intRet <= 0 Then
                Throw New Exception()
            End If

            'コミット
            objTran.Commit()

            psMesBox(Me, "00002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "画面内容")

            '★排他情報削除
            If Not Me.Master.Master.ppExclusiveDateDtl = String.Empty Then

                clsExc.pfDel_Exclusive(Me _
                              , Session(P_SESSION_SESSTION_ID) _
                              , Me.Master.Master.ppExclusiveDateDtl)

                Me.Master.Master.ppExclusiveDateDtl = String.Empty

            End If

            msPageClear()

            '再読み込み
            msDataRead()

        Catch ex As Exception
            If Not objTran Is Nothing Then
                'ロールバック
                objTran.Rollback()
            End If

            psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "画面内容")
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------

        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(objCon) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub

    Protected Sub btnDetailClear_Click(sender As Object, e As EventArgs) Handles btnDetailClear.Click
        lblNo.Text = ""                         'NO
        txtKanriNo.ppText = ""                  '管理番号
        txtTBoxId.ppText = ""                   'TBOXID
        lblNLKbn.Text = ""                      'NL区分
        lblSyubetu.Text = ""                    '種別
        lblVersion.Text = ""                    'ＶＥＲ
        lblHallNm.Text = ""                     'ホール名
        txtSerialNo.ppText = ""                 'シリアルＮｏ
        ddlSagyoKekka.ppDropDownList.SelectedIndex = 0      '作業結果
        txtJuryo.ppText = ""                    '受領
        txtSagyoJissi.ppText = ""               '作業実施
        txtBaitaiSofu.ppText = ""               '媒体送付
        txtHenkyakuHasso.ppText = ""            '返却(発送)
        txtHenkyakuNonyu.ppText = ""            '返却(納入)
        txtKensyuMonth.ppText = ""              '検収月
        chkRequest.Checked = False              '請求対象
        chkRequest.Enabled = False
        hdnNLKbn.Value = ""                     'NL区分コード

        btnDetailInsert.Enabled = True
        btnDetailUpdate.Enabled = False
        btnDetailDelete.Enabled = False
    End Sub

    ''' <summary>
    ''' 選択ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            Dim strExclusiveDate As String = Nothing
            Dim arTable_Name As New ArrayList
            Dim arKye As New ArrayList

            '開始ログ出力
            psLogStart(Me)

            '選択ボタンでなければ、なにもしない
            If e.CommandName.Trim() <> "btnSelect" Then
                Return
            End If

            Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    'ボタン押下行のIndex
            Dim rowData As GridViewRow = grvList.Rows(intIndex)             'ボタン押下行

            '初期化
            lblNo.Text = ""
            txtKanriNo.ppText = ""
            txtTBoxId.ppText = ""
            lblNLKbn.Text = ""
            hdnNLKbn.Value = ""
            lblSyubetu.Text = ""
            lblVersion.Text = ""
            lblHallNm.Text = ""
            txtSerialNo.ppText = ""
            ddlSagyoKekka.ppDropDownList.SelectedIndex = 0
            txtJuryo.ppText = ""
            txtSagyoJissi.ppText = ""
            txtBaitaiSofu.ppText = ""
            txtHenkyakuHasso.ppText = ""
            txtHenkyakuNonyu.ppText = ""
            txtKensyuMonth.ppText = ""
            chkRequest.Checked = False
            chkRequest.Enabled = False

            '★排他情報削除
            If Not Me.Master.Master.ppExclusiveDateDtl = String.Empty Then

                clsExc.pfDel_Exclusive(Me _
                              , Session(P_SESSION_SESSTION_ID) _
                              , Me.Master.Master.ppExclusiveDateDtl)

                Me.Master.Master.ppExclusiveDateDtl = String.Empty

            End If
            Select Case Session(P_SESSION_AUTH)
                Case "管理者", "SPC", "NGC"

                    '★ロック対象テーブル名の登録
                    arTable_Name.Insert(0, "D22_USINGDB")

                    '★ロックテーブルキー項目の登録(D22_USINGDB)
                    arKye.Insert(0, CType(grvList.Rows(intIndex).FindControl("KMKNO"), TextBox).Text)

                    '★排他情報確認処理
                    If clsExc.pfSel_Exclusive(strExclusiveDate _
                                     , Me _
                                     , Session(P_SESSION_IP) _
                                     , Session(P_SESSION_PLACE) _
                                     , Session(P_SESSION_USERID) _
                                     , Session(P_SESSION_SESSTION_ID) _
                                     , ViewState(P_SESSION_GROUP_NUM) _
                                     , M_MY_DISP_ID _
                                     , arTable_Name _
                                     , arKye) = 0 Then

                        '登録項目の各項目に選択行の値を貼り付け
                        lblNo.Text = CType(rowData.FindControl("KMKNO"), TextBox).Text                          'NO
                        txtKanriNo.ppText = CType(rowData.FindControl("管理番号"), TextBox).Text                 '管理番号
                        txtTBoxId.ppText = CType(rowData.FindControl("TBOXID"), TextBox).Text                   'TBOXID
                        lblNLKbn.Text = CType(rowData.FindControl("KMKNL区分"), TextBox).Text                    'NL区分
                        hdnNLKbn.Value = CType(rowData.FindControl("KMKNL区分コード"), TextBox).Text                    'NL区分コード
                        lblSyubetu.Text = CType(rowData.FindControl("種別"), TextBox).Text                   '種別
                        lblVersion.Text = CType(rowData.FindControl("VER"), TextBox).Text                   'バージョン
                        lblHallNm.Text = CType(rowData.FindControl("ホール名"), TextBox).Text                     'ホール名
                        txtSerialNo.ppText = CType(rowData.FindControl("シリアルNO"), TextBox).Text               'シリアルNo
                        ddlSagyoKekka.ppSelectedValue = CType(rowData.FindControl("作業結果コード"), TextBox).Text  '作業結果
                        txtJuryo.ppText = CType(rowData.FindControl("受領"), TextBox).Text                     '受領
                        txtSagyoJissi.ppText = CType(rowData.FindControl("作業実施"), TextBox).Text           '作業実施
                        txtBaitaiSofu.ppText = CType(rowData.FindControl("媒体送付"), TextBox).Text           '媒体送付
                        txtHenkyakuHasso.ppText = CType(rowData.FindControl("返却_発送"), TextBox).Text     '返却(発送)
                        txtHenkyakuNonyu.ppText = CType(rowData.FindControl("返却_納入"), TextBox).Text     '返却(納入)
                        txtKensyuMonth.ppText = CType(rowData.FindControl("検収月"), TextBox).Text.Replace("/", "")          '検収月
                        '                        If CType(rowData.FindControl("検収月"), TextBox).Text >= "1610" Then
                        If txtKensyuMonth.ppText >= "1610" Then
                            If CType(rowData.FindControl("請求対象"), TextBox).Text = "1" Then
                                chkRequest.Enabled = True
                                chkRequest.Checked = True
                            Else
                                chkRequest.Enabled = True
                                chkRequest.Checked = False
                            End If
                        Else
                            chkRequest.Enabled = False
                            chkRequest.Checked = False
                        End If

                        '登録年月日時刻(明細)に登録.
                        Me.Master.Master.ppExclusiveDateDtl = strExclusiveDate

                        btnDetailInsert.Enabled = False
                        btnDetailUpdate.Enabled = True
                        btnDetailDelete.Enabled = True
                    Else
                        Return
                    End If
            End Select

        Catch ex As Exception
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub

    ''' <summary>
    ''' 印刷ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.Object)
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            '開始ログ出力
            psLogStart(Me)

            Dim strFileNm As String = ""
            Dim strHiduke As String = ""

            'データ取得
            Dim objTbl As DataTable = mfGetPrintData(strHiduke)

            If objTbl.Rows.Count = 0 Then
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                '終了ログ出力
                psLogEnd(Me)
                Return
            End If

            'ファイル名作成
            strFileNm = strCHOHYO_NM_PART_JP & "_" & strHiduke

            'PDF作成
            Dim objPrint As New CDPREP002()
            psPrintPDF(Me, objPrint, objTbl, strFileNm)

        Catch ex As Exception
            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "カード情報吸上げＴＢＯＸ作業報告書兼受領書")
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub

    ''' <summary>
    ''' ＣＳＶボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCsv_Click(ByVal sender As Object, ByVal e As System.Object)
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            '開始ログ出力
            psLogStart(Me)

            Dim objTbl As DataTable = mfGetNewDataTableLikeList()

            'データテーブルとして一覧を取得
            For i As Integer = 0 To grvList.Rows.Count - 1
                Dim objRow As DataRow = objTbl.NewRow()
                objRow("KMKNO") = CType(grvList.Rows(i).FindControl("KMKNO"), TextBox).Text
                objRow("管理番号") = CType(grvList.Rows(i).FindControl("管理番号"), TextBox).Text
                objRow("TBOXID") = CType(grvList.Rows(i).FindControl("TBOXID"), TextBox).Text
                objRow("KMKNL区分") = CType(grvList.Rows(i).FindControl("KMKNL区分"), TextBox).Text
                objRow("種別") = CType(grvList.Rows(i).FindControl("種別"), TextBox).Text
                objRow("VER") = CType(grvList.Rows(i).FindControl("VER"), TextBox).Text
                objRow("ホール名") = CType(grvList.Rows(i).FindControl("ホール名"), TextBox).Text
                objRow("シリアルNO") = CType(grvList.Rows(i).FindControl("シリアルNO"), TextBox).Text
                objRow("作業結果") = CType(grvList.Rows(i).FindControl("作業結果"), TextBox).Text
                objRow("受領") = CType(grvList.Rows(i).FindControl("受領"), TextBox).Text
                objRow("作業実施") = CType(grvList.Rows(i).FindControl("作業実施"), TextBox).Text
                objRow("媒体送付") = CType(grvList.Rows(i).FindControl("媒体送付"), TextBox).Text
                objRow("返却_発送") = CType(grvList.Rows(i).FindControl("返却_発送"), TextBox).Text
                objRow("返却_納入") = CType(grvList.Rows(i).FindControl("返却_納入"), TextBox).Text
                objRow("検収月") = CType(grvList.Rows(i).FindControl("検収月"), TextBox).Text
                objTbl.Rows.Add(objRow)
            Next

            If objTbl.Rows.Count = 0 Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                '終了ログ出力
                psLogEnd(Me)
                Return
            End If

            'データセットに変換
            Dim objDs As New DataSet()
            objDs.Tables.Add(objTbl)

            Dim dteDate As DateTime = DateTime.Now()
            Dim strHiduke As String = ""
            Dim strFilePath As String = ""

            strHiduke = String.Format("{0:yyyyMMddHHmmss}", dteDate)
            strFilePath = Server.MapPath("~/" & P_WORK_NM & "/" & Session.SessionID)

            'CSVファイルを作成
            If pfDLCsvFile(strCHOHYO_NM_PART_JP & "_" & strHiduke + ".csv", objDs.Tables(0), True, Me) Then
                psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSVファイル")
                Return
            End If

            '完了メッセージ
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "CSVファイルの作成")

        Catch ex As Threading.ThreadAbortException
            '完了メッセージ
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "CSVファイルの作成")

        Catch ex As Exception
            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "使用中カードDB吸上げ一覧(CSV)")
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try


    End Sub

    ''' <summary>
    ''' TBOXID入力時の動き
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ctl_Change(ByVal sender As Object, ByVal e As System.Object)
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            '開始ログ出力
            psLogStart(Me)

            'DB接続
            If clsDataConnect.pfOpen_Database(objCon) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Return
            End If

            'プロシージャ設定(TBOXデータ検索)
            objCmd = New SqlCommand("CDPLSTP001_S3")
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Connection = objCon

            'パラメータ設定
            With objCmd.Parameters
                .Add(pfSet_Param("prmT03_TBOXID", SqlDbType.NVarChar, txtTBoxId.ppText))
            End With

            'SQL実行
            Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

            '取得エラー
            If objDs Is Nothing OrElse objDs.Tables.Count = 0 Then
                Throw New Exception()
            End If

            Dim objTbl As DataTable = objDs.Tables(0)
            If objTbl.Rows.Count > 0 Then
                lblNLKbn.Text = CType(objTbl.Rows(0)("M29_NAME"), String)
                lblSyubetu.Text = CType(objTbl.Rows(0)("T03_TBOXCLASS_CD"), String)
                lblVersion.Text = CType(objTbl.Rows(0)("T03_VERSION"), String)
                lblHallNm.Text = CType(objTbl.Rows(0)("T01_HALL_NAME"), String)
                hdnNLKbn.Value = CType(objTbl.Rows(0)("T03_NL_CLS"), String)
            Else
                lblNLKbn.Text = ""
                lblSyubetu.Text = ""
                lblVersion.Text = ""
                lblHallNm.Text = ""
                hdnNLKbn.Value = ""
            End If

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "TBOX情報")
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Return
        Finally

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面項目初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msPageClear()
        '★排他情報用コントロールの初期化
        Me.Master.Master.ppExclusiveDate = String.Empty
        Me.Master.Master.ppExclusiveDateDtl = String.Empty

        'CDPLSTP001-001
        '        '検索条件
        '        msPageClear_Search()
        'CDPLSTP001-001

        '登録項目
        msPageClear_Update()

        '一覧
        msPageClear_List()

    End Sub

    ''' <summary>
    ''' 初期化(検索条件項目)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msPageClear_Search()

        'CDPLSTP001-001
        'ViewStateのクリア
        ViewState("txtSrcTBoxFrToFrom") = Nothing           'TBOXID_FROM
        ViewState("txtSrcTBoxFrToTo") = Nothing             'TBOXID_TO
        ViewState("txtSrcKanriNoFrToFrom") = Nothing        '管理番号_FROM
        ViewState("txtSrcKanriNoFrToTo") = Nothing          '管理番号_TO
        ViewState("txtSrcNoFrToFrom") = Nothing             'NO_FROM
        ViewState("txtSrcNoFrToTo") = Nothing               'NO_TO
        ViewState("txtSrcJuryoFrToFrom") = Nothing          '受領_FROM
        ViewState("txtSrcJuryoFrToTo") = Nothing            '受領_TO
        ViewState("txtSrcKensyuMonthFrToFrom") = Nothing    '検収月_FROM
        ViewState("txtSrcKensyuMonthFrToTo") = Nothing      '検収月_TO
        'CDPLSTP001-001

        'コントロールのクリア
        txtSrcTBoxFrTo.ppFromText = ""          'TBOXID_FROM
        txtSrcTBoxFrTo.ppToText = ""            'TBOXID_TO
        txtSrcKanriNoFrTo.ppFromText = ""       '管理番号_FROM
        txtSrcKanriNoFrTo.ppToText = ""         '管理番号_TO
        txtSrcNoFrTo.ppFromText = ""            'NO_FROM
        txtSrcNoFrTo.ppToText = ""              'NO_TO
        txtSrcJuryoFrTo.ppFromText = ""         '受領_FROM
        txtSrcJuryoFrTo.ppToText = ""           '受領_TO
        txtSrcKensyuMonthFrTo.ppFromText = ""   '検収月_FROM
        txtSrcKensyuMonthFrTo.ppToText = ""     '検収月_TO

        'ＴＢＯＸＩＤ項目にフォーカスを設定
        txtSrcTBoxFrTo.ppTextBoxFrom.Focus()

    End Sub

    ''' <summary>
    ''' 初期化(登録項目)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msPageClear_Update()
        lblNo.Text = ""                         'NO
        txtKanriNo.ppText = ""                  '管理番号
        txtTBoxId.ppText = ""                   'TBOXID
        lblNLKbn.Text = ""                      'NL区分
        lblSyubetu.Text = ""                    '種別
        lblVersion.Text = ""                    'ＶＥＲ
        lblHallNm.Text = ""                     'ホール名
        txtSerialNo.ppText = ""                 'シリアルＮｏ
        ddlSagyoKekka.ppDropDownList.SelectedIndex = 0      '作業結果
        txtJuryo.ppText = ""                    '受領
        txtSagyoJissi.ppText = ""               '作業実施
        txtBaitaiSofu.ppText = ""               '媒体送付
        txtHenkyakuHasso.ppText = ""            '返却(発送)
        txtHenkyakuNonyu.ppText = ""            '返却(納入)
        txtKensyuMonth.ppText = ""              '検収月
        hdnNLKbn.Value = ""                     'NL区分コード
        chkRequest.Checked = False              '請求区分
        chkRequest.Enabled = False
        btnDetailUpdate.Enabled = False         'ボタン
        btnDetailDelete.Enabled = False
    End Sub

    ''' <summary>
    ''' Enable制御
    ''' </summary>
    ''' <param name="shrOpenMode"></param>
    ''' <remarks></remarks>
    Private Sub msPageEnableSet(ByVal shrOpenMode As Short)
        txtSrcTBoxFrTo.ppEnabled = True 'TBOXID
        txtSrcKanriNoFrTo.ppEnabled = True '管理番号
        txtSrcNoFrTo.ppEnabled = True 'NO
        txtSrcJuryoFrTo.ppEnabled = True '受領
        txtSrcKensyuMonthFrTo.ppEnabled = True '検収月
        Master.ppRigthButton1.Enabled = True '検索
        Master.ppRigthButton2.Enabled = True '検索条件クリア

        If shrOpenMode = ClsComVer.E_遷移条件.参照 Then
            '参照モード
            txtKanriNo.ppEnabled = False                   '管理番号
            txtTBoxId.ppEnabled = False                    'TBOXID
            txtSerialNo.ppEnabled = False                  'シリアルNO
            ddlSagyoKekka.ppEnabled = False                '作業結果
            txtJuryo.ppEnabled = False                     '受領
            txtSagyoJissi.ppEnabled = False                '作業実施
            txtBaitaiSofu.ppEnabled = False                '媒体送付
            txtHenkyakuHasso.ppEnabled = False             '返却(発送)
            txtHenkyakuNonyu.ppEnabled = False             '返却(納入)
            txtKensyuMonth.ppEnabled = False               '検収月
            chkRequest.Enabled = False
            btnDetailInsert.Enabled = False                '登録
            btnDetailUpdate.Enabled = False                '更新
            btnDetailDelete.Enabled = False                '削除
            btnDetailClear.Enabled = True                 'クリア
            grvList.Enabled = False                        '一覧
            Master.Master.ppRigthButton1.Enabled = True    'CSV
            Master.Master.ppRigthButton2.Enabled = True    '印刷
        ElseIf shrOpenMode = ClsComVer.E_遷移条件.更新 Then
            '更新モード
            txtKanriNo.ppEnabled = False                   '管理番号
            txtTBoxId.ppEnabled = False                    'TBOXID
            txtSerialNo.ppEnabled = False                  'シリアルNO
            ddlSagyoKekka.ppEnabled = True                 '作業結果
            txtJuryo.ppEnabled = True                      '受領
            txtSagyoJissi.ppEnabled = True                 '作業実施
            txtBaitaiSofu.ppEnabled = True                 '媒体送付
            txtHenkyakuHasso.ppEnabled = True              '返却(発送)
            txtHenkyakuNonyu.ppEnabled = True              '返却(納入)
            txtKensyuMonth.ppEnabled = True                '検収月
            chkRequest.Enabled = False
            btnDetailInsert.Enabled = True                 '登録
            btnDetailUpdate.Enabled = True                 '更新
            btnDetailDelete.Enabled = True                 '削除
            btnDetailClear.Enabled = True                 'クリア
            grvList.Enabled = True                         '一覧
            Master.Master.ppRigthButton1.Enabled = True    'CSV
            Master.Master.ppRigthButton2.Enabled = True    '印刷
        ElseIf shrOpenMode = ClsComVer.E_遷移条件.登録 Then
            '追加モード
            txtKanriNo.ppEnabled = True                    '管理番号
            txtTBoxId.ppEnabled = True                     'TBOXID
            txtSerialNo.ppEnabled = True                   'シリアルNO
            ddlSagyoKekka.ppEnabled = True                 '作業結果
            txtJuryo.ppEnabled = True                      '受領
            txtSagyoJissi.ppEnabled = True                 '作業実施
            txtBaitaiSofu.ppEnabled = True                 '媒体送付
            txtHenkyakuHasso.ppEnabled = True              '返却(発送)
            txtHenkyakuNonyu.ppEnabled = True              '返却(納入)
            txtKensyuMonth.ppEnabled = True                '検収月
            chkRequest.Enabled = False
            btnDetailInsert.Enabled = True                 '登録
            btnDetailUpdate.Enabled = False                 '更新
            btnDetailDelete.Enabled = False                 '削除
            btnDetailClear.Enabled = True                 'クリア
            grvList.Enabled = True                         '一覧
            Master.Master.ppRigthButton1.Enabled = True    'CSV
            Master.Master.ppRigthButton2.Enabled = True    '印刷
        End If

    End Sub

    ''' <summary>
    ''' テーブルの型取得(CSV用)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetNewDataTableLikeList() As DataTable
        'グリッド、該当件数
        Dim objTbl As New DataTable
        objTbl.Columns.Add("KMKNO")
        objTbl.Columns.Add("KMKNL区分")
        objTbl.Columns.Add("管理番号")
        objTbl.Columns.Add("TBOXID")
        objTbl.Columns.Add("ホール名")
        objTbl.Columns.Add("種別")
        objTbl.Columns.Add("VER")
        objTbl.Columns.Add("シリアルNO")
        objTbl.Columns.Add("作業結果")
        objTbl.Columns.Add("受領")
        objTbl.Columns.Add("作業実施")
        objTbl.Columns.Add("媒体送付")
        objTbl.Columns.Add("返却_発送")
        objTbl.Columns.Add("返却_納入")
        objTbl.Columns.Add("検収月")
        Return objTbl
    End Function

    ''' <summary>
    ''' 帳票表示用テーブル型取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetPrintDataNewTable() As DataTable
        'グリッド、該当件数
        Dim objTbl As New DataTable
        objTbl.Columns.Add("管理")
        objTbl.Columns.Add("和暦")
        objTbl.Columns.Add("TBOX種別")
        objTbl.Columns.Add("ホール名")
        objTbl.Columns.Add("TBOXID")
        objTbl.Columns.Add("TBOXVer")
        objTbl.Columns.Add("シリアルNo")
        objTbl.Columns.Add("処理日")
        objTbl.Columns.Add("吸上結果")
        '--------------------------------
        '2014/06/11 後藤　ここから
        '--------------------------------
        objTbl.Columns.Add("宛先会社名")
        objTbl.Columns.Add("宛先部署名")
        objTbl.Columns.Add("宛先")
        objTbl.Columns.Add("宛先会社名２")
        objTbl.Columns.Add("宛先部署名２")
        objTbl.Columns.Add("宛先２")
        '--------------------------------
        '2014/06/11 後藤　ここまで
        '--------------------------------

        Return objTbl
    End Function

    ''' <summary>
    ''' 初期化(一覧)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msPageClear_List()
        'グリッド、該当件数
        Me.grvList.DataSource = New DataTable()
        Master.ppCount = "0"
        Me.grvList.DataBind()

    End Sub

    ''' <summary>
    ''' イベントハンドラ付加
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msAddEventHandlerParentKmk()
        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click           '検索ボタン
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click            '検索条件クリアボタン
        AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnCsv_Click       'ＣＳＶボタン
        AddHandler Master.Master.ppRigthButton2.Click, AddressOf btnPrint_Click     '印刷ボタン
        'TBOXIDを入力した場合
        AddHandler Me.txtTBoxId.ppTextBox.TextChanged, AddressOf ctl_Change
        Me.txtTBoxId.ppTextBox.AutoPostBack = True

        AddHandler txtKensyuMonth.ppTextBox.TextChanged, AddressOf KensyuMonth_Change
        txtKensyuMonth.ppTextBox.AutoPostBack = True

        '追加ボタンの確認メッセージ設定
        Me.btnDetailInsert.OnClientClick =
            pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "画面の入力内容")

        '更新ボタンの確認メッセージ設定
        Me.btnDetailUpdate.OnClientClick =
            pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "画面の入力内容")

        '削除ボタンの確認メッセージ設定
        Me.btnDetailDelete.OnClientClick =
            pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "画面の入力内容")

        'CSVボタンの確認メッセージ設定
        Master.Master.ppRigthButton1.OnClientClick =
            pfGet_OCClickMes("10001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "一覧の内容でCSV")

        '印刷ボタンの確認メッセージ設定
        Master.Master.ppRigthButton2.OnClientClick =
            pfGet_OCClickMes("10002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "一覧の内容")

    End Sub

    ''' <summary>
    ''' 一覧検索
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msDataRead()
        Dim objCon As SqlConnection = Nothing   'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '項目チェック
        If Page.IsValid() = False Then
            Return
        End If

        Try
            'DB接続
            If clsDataConnect.pfOpen_Database(objCon) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Me.grvList.DataSource = New DataTable()
                Me.grvList.DataBind()
                Master.ppCount = "0"
                Return
            End If

            '実行ストアドプロシージャの設定
            objCmd = New SqlCommand("CDPLSTP001_S1")
            objCmd.Connection = objCon
            objCmd.CommandType = CommandType.StoredProcedure

            'パラメータ設定
            With objCmd.Parameters
                .Add(pfSet_Param("prmTboxIdFr", SqlDbType.NVarChar, mfGetDBNull(txtSrcTBoxFrTo.ppFromText)))
                .Add(pfSet_Param("prmTboxIdTo", SqlDbType.NVarChar, mfGetDBNull(txtSrcTBoxFrTo.ppToText)))
                .Add(pfSet_Param("prmKanriNoFr", SqlDbType.NVarChar, mfGetDBNull(txtSrcKanriNoFrTo.ppFromText)))
                .Add(pfSet_Param("prmKanriNoTo", SqlDbType.NVarChar, mfGetDBNull(txtSrcKanriNoFrTo.ppToText)))
                .Add(pfSet_Param("prmKmkNoFr", SqlDbType.NVarChar, mfGetDBNull(txtSrcNoFrTo.ppFromText)))
                .Add(pfSet_Param("prmKmkNoTo", SqlDbType.NVarChar, mfGetDBNull(txtSrcNoFrTo.ppToText)))
                .Add(pfSet_Param("prmJuryoFr", SqlDbType.NVarChar, mfGetDBNull(txtSrcJuryoFrTo.ppFromText)))
                .Add(pfSet_Param("prmJuryoTo", SqlDbType.NVarChar, mfGetDBNull(txtSrcJuryoFrTo.ppToText)))
                .Add(pfSet_Param("prmKensyuFr", SqlDbType.NVarChar, mfGetDBNull(txtSrcKensyuMonthFrTo.ppFromText)))
                .Add(pfSet_Param("prmKensyuTo", SqlDbType.NVarChar, mfGetDBNull(txtSrcKensyuMonthFrTo.ppToText)))
            End With

            'SQL実行
            objDs = clsDataConnect.pfGet_DataSet(objCmd)

            '取得エラー
            If objDs Is Nothing OrElse objDs.Tables.Count = 0 Then
                Throw New Exception()
            End If

            'CDPLSTP001-001
            '検索結果が0件の場合.
            If objDs.Tables(0).Rows.Count = 0 Then
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
            End If

            '取得異常
            If objDs Is Nothing Then
                Throw New Exception()
            End If
            'CDPLSTP001-001

            '件数を設定
            Master.ppCount = objDs.Tables(0).Rows.Count.ToString

            If Master.ppCount > 0 Then
                Master.Master.ppRigthButton1.Enabled = True
                Master.Master.ppRigthButton2.Enabled = True
            Else
                Master.Master.ppRigthButton1.Enabled = False
                Master.Master.ppRigthButton2.Enabled = False
            End If

            '閾値制御
            Dim objTbl As DataTable = mfSetShikiichi(objDs.Tables(0))

            '取得したデータをグリッドに設定
            Me.grvList.DataSource = objTbl

            '変更を反映
            Me.grvList.DataBind()

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "使用中カードＤＢ吸上一覧")
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Me.grvList.DataSource = New DataTable()
            Me.grvList.DataBind()
            Master.ppCount = "0"
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(objCon) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

        End Try
    End Sub

    ''' <summary>
    ''' 印刷データ作成
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetPrintData(ByRef strHiduke As String) As DataTable
        Dim objTbl As DataTable = mfGetPrintDataNewTable()

        'データテーブルとして一覧を取得
        For i As Integer = 0 To grvList.Rows.Count - 1
            Dim objRow As DataRow = objTbl.NewRow()
            Dim objCal As New System.Globalization.JapaneseCalendar()
            Dim strEra As String = ""

            '和暦設定
            Dim strWarekiDate As String = CType(grvList.Rows(i).FindControl("媒体送付"), TextBox).Text
            Dim dteWarekiDate As Date = Nothing

            If Date.TryParse(strWarekiDate, dteWarekiDate) Then
                Dim intEra As Integer = objCal.GetEra(dteWarekiDate)
                '年号取得
                Select Case intEra
                    Case 1
                        strEra = "明治"
                    Case 2
                        strEra = "大正"
                    Case 3
                        strEra = "昭和"
                    Case 4
                        strEra = "平成"
                    Case Else
                        strEra = ""
                End Select

                'objRow("和暦") = strEra & " " & objCal.GetYear(dteWarekiDate) & " 年 " & objCal.GetMonth(dteWarekiDate) & " 月 " & objCal.GetDayOfMonth(dteWarekiDate) & " 日" 'レポート日付
                objRow("和暦") = dteWarekiDate.ToString("yyyy") & " 年 " & objCal.GetMonth(dteWarekiDate) & " 月 " & objCal.GetDayOfMonth(dteWarekiDate) & " 日" 'レポート日付
            Else
                objRow("和暦") = ""
            End If

            objRow("管理") = CType(grvList.Rows(i).FindControl("管理番号"), TextBox).Text '管理番号
            objRow("TBOX種別") = CType(grvList.Rows(i).FindControl("種別"), TextBox).Text '種別
            objRow("ホール名") = CType(grvList.Rows(i).FindControl("ホール名"), TextBox).Text 'ホール名
            objRow("TBOXID") = CType(grvList.Rows(i).FindControl("TBOXID"), TextBox).Text 'TBOXID
            objRow("TBOXVer") = CType(grvList.Rows(i).FindControl("VER"), TextBox).Text 'VER
            objRow("シリアルNo") = CType(grvList.Rows(i).FindControl("シリアルNO"), TextBox).Text 'シリアルNO
            objRow("処理日") = CType(grvList.Rows(i).FindControl("作業実施"), TextBox).Text '作業実施
            objRow("吸上結果") = CType(grvList.Rows(i).FindControl("作業結果"), TextBox).Text '作業結果
            '--------------------------------
            '2014/06/11 後藤　ここから
            '--------------------------------
            objRow("宛先会社名") = CType(grvList.Rows(i).FindControl("宛先会社名"), TextBox).Text '宛先会社名
            objRow("宛先部署名") = CType(grvList.Rows(i).FindControl("宛先部署名"), TextBox).Text '宛先部署名
            objRow("宛先") = CType(grvList.Rows(i).FindControl("宛先"), TextBox).Text '宛先
            objRow("宛先会社名２") = CType(grvList.Rows(i).FindControl("宛先会社名２"), TextBox).Text '宛先会社名２
            objRow("宛先部署名２") = CType(grvList.Rows(i).FindControl("宛先部署名２"), TextBox).Text '宛先部署名２
            objRow("宛先２") = CType(grvList.Rows(i).FindControl("宛先２"), TextBox).Text '宛先２
            '--------------------------------
            '2014/06/11 後藤　ここまで
            '--------------------------------
            objTbl.Rows.Add(objRow)
        Next

        Return objTbl

    End Function

    ''' <summary>
    ''' 空白に対しDBNULLを返却
    ''' </summary>
    ''' <param name="strVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDBNull(ByVal strVal As String) As Object
        If strVal.Trim() = "" Then
            Return DBNull.Value
        End If
        Return strVal
    End Function

    ''' <summary>
    ''' 閾値制御
    ''' </summary>
    ''' <param name="objTbl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfSetShikiichi(ByVal objTbl As DataTable) As DataTable
        Dim objWkTbl As DataTable = objTbl.Clone()

        'テーブルに行はあるか？
        If Not objTbl Is Nothing AndAlso objTbl.Rows.Count > 0 Then

            'テーブルの行が最大件数を超えているか
            If objTbl.Rows.Count > objTbl.Rows(0)("最大件数") Then

                '行を最大件数に絞込み
                For i As Integer = 0 To objTbl.Rows(0)("最大件数") - 1
                    objWkTbl.ImportRow(objTbl.Rows(i))
                Next

                '閾値超過をメッセで知らせる
                psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, objTbl.Rows.Count, objTbl.Rows(0)("最大件数"))

            Else
                objWkTbl = objTbl
            End If

        Else
            objWkTbl = objTbl
        End If

        objTbl.Columns.Remove("最大件数")

        Return objWkTbl

    End Function

    Private Sub KensyuMonth_Change()

        If txtKensyuMonth.ppTextBox.Text >= "1610" Then
            Me.chkRequest.Enabled = True
        Else
            Me.chkRequest.Enabled = False
            Me.chkRequest.Checked = False
        End If

    End Sub

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
