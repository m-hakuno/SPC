'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　監視対象外ホール一覧
'*　ＰＧＭＩＤ：　WATLSTP002
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.14　：　ＸＸＸ
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive      '排他制御用

Public Class WATLSTP002
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
    'プログラムID
    Const M_MY_DISP_ID = P_FUN_WAT & P_SCR_LST & P_PAGE & "002"
    '--------------------------------
    '2014/05/02 高松　ここから
    '--------------------------------
    Const M_MSG_CSV = "監視対象外ホール一覧ＣＳＶファイル"
    '--------------------------------
    '2014/05/02 高松　ここまで
    '--------------------------------

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
    ''' Page_Initイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, M_MY_DISP_ID)

    End Sub

    ''' <summary>
    ''' Page_Loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click
        AddHandler Me.btnAdd.Click, AddressOf btnAdd_Click
        AddHandler Me.btnUpdate.Click, AddressOf btnUpdate_Click
        AddHandler Me.btnDelete.Click, AddressOf btnDelete_Click
        '--------------------------------
        '2014/05/02 高松　ここから
        '--------------------------------
        AddHandler Me.btnCSV.Click, AddressOf btnCSV_Click
        '--------------------------------
        '2014/05/02 高松　ここまで
        '--------------------------------

        'ＴＢＯＸ情報取得設定
        AddHandler Me.txtTboxId.ppTextBox.TextChanged, AddressOf txtTboxId_TextChanged
        Me.txtTboxId.ppTextBox.AutoPostBack = True

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            If Not IsPostBack Then  '初回表示のみ

                '検索条件クリアボタンと新規作成ボタン押下時の検証を無効
                Master.ppRigthButton2.CausesValidation = False

                '検証グループ設定


                '画面設定
                Master.Master.ppProgramID = M_MY_DISP_ID
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)
                Master.Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる

                'パンくずリスト設定
                Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                'セッション変数「グループナンバー」が存在しない場合、画面を閉じる
                If Session(P_SESSION_GROUP_NUM) Is Nothing Then
                    psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    '--------------------------------
                    '2014/06/11 後藤　ここから
                    '--------------------------------
                    '排他削除
                    clsExc.psExclusive_Del_Chk(Me, Me.Master.Master.ppExclusiveDate, Me.Master.Master.ppExclusiveDateDtl)
                    '--------------------------------
                    '2014/06/11 後藤　ここまで
                    '--------------------------------
                    psClose_Window(Me)
                    Return
                End If

                'ViewStateに「グループナンバー」を保存
                ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

                '--2014/04/15 ここから
                '排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
                If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then

                    Me.Master.Master.ppExclusiveDateDtl = Session(P_SESSION_EXCLUSIV_DATE)

                End If
                '--2014/04/15 ここまで

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                'ドロップダウンリスト設定
                msSet_ddlSystem()       'システム
                msSet_ddlInpUsr()       '入力者

                '画面クリア
                msClear_Screen()

            Else
                'ViewState「グループナンバー」が存在しない場合、画面を閉じる
                If ViewState(P_SESSION_GROUP_NUM) Is Nothing Then
                    psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    '--------------------------------
                    '2014/06/11 後藤　ここから
                    '--------------------------------
                    '排他削除
                    clsExc.psExclusive_Del_Chk(Me, Me.Master.Master.ppExclusiveDate, Me.Master.Master.ppExclusiveDateDtl)
                    '--------------------------------
                    '2014/06/11 後藤　ここまで
                    '--------------------------------
                    psClose_Window(Me)
                    Return
                End If

            End If
        Catch ex As Exception
            '画面初期化エラー
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------

        End Try
    End Sub

    '---------------------------
    '2014/04/21 武 ここから
    '---------------------------
    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
            Case "NGC"
                txtTboxId.ppEnabled = False
                txtReason2.ppEnabled = False
                txtCnfm.ppEnabled = False
                txtMntrReportNo.ppEnabled = False
                ddlInpUsr.Enabled = False
                txtNoteText.ppEnabled = False
                btnAdd.Enabled = False
                btnUpdate.Enabled = False
                btnDelete.Enabled = False
        End Select

    End Sub
    '---------------------------
    '2014/06/18 武 ここから
    '---------------------------
    'Private Sub grvList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grvList.RowDataBound

    '    Select Case Session(P_SESSION_AUTH)
    '        Case "管理者"
    '        Case "SPC"
    '        Case "営業所"
    '        Case "NGC"
    '            e.Row.Cells(0).Enabled = False
    '    End Select

    'End Sub
    '---------------------------
    '2014/06/18 武 ここまで
    '---------------------------
    '---------------------------
    '2014/04/21 武 ここまで
    '---------------------------

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
        msClear_SearchArea()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 検索ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)
        '-----------------------------
        '2014/06/20 武　ここから
        '-----------------------------
        Me.txtTboxId.ppText = String.Empty              'ＴＢＯＸＩＤ
        Me.lblNLCls2.Text = String.Empty                'ＮＬ区分
        Me.lblSystem2.Text = String.Empty               'システム
        Me.lblHallNm2.Text = String.Empty               'ホール名
        Me.txtReason2.ppText = String.Empty             '監視対象外理由
        Me.txtCnfm.ppText = String.Empty                '確認者
        Me.txtMntrReportNo.ppText = String.Empty        '管理番号
        Me.ddlInpUsr.SelectedValue = String.Empty       '入力者
        Me.txtNoteText.ppText = String.Empty            '備考

        msSet_DtlMode("1")
        '-----------------------------
        '2014/06/20 武　ここまで
        '-----------------------------
        'データ取得
        If (Page.IsValid) Then
            msGet_Data()
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 登録ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs)

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '開始ログ出力
        psLogStart(Me)

        '----- 追加
        Dim dtrError As DataRow
        If ddlInpUsr.SelectedIndex = 0 Then
            dtrError = ClsCMCommon.pfGet_ValMes("5003", lblInpUsr.Text)
            cuvDropDownList.Text = "未入力エラー"
            cuvDropDownList.ErrorMessage = dtrError.Item(P_VALMES_MES)
            cuvDropDownList.Enabled = True
            cuvDropDownList.IsValid = False
            cuvDropDownList.SetFocusOnError = True
        End If
        '----- 追加

        '入力チェック
        If (Page.IsValid) Then
            Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
            Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
            Dim objDs As DataSet = Nothing          'データセット
            Dim intRtn As Integer
            Dim intCnt As Integer = 0

            '確認処理付与
            Me.btnUpdate.OnClientClick =
                pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "監視対象外ホール情報")

            '接続
            If clsDataConnect.pfOpen_Database(objCn) Then
                Try
                    '確認処理
                    objCmd = New SqlCommand(M_MY_DISP_ID + "_S4", objCn)
                    'パラメータ設定
                    With objCmd.Parameters
                        .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxId.ppText))
                    End With
                    'データ取得
                    objDs = clsDataConnect.pfGet_DataSet(objCmd)

                    '件数を設定
                    intCnt = objDs.Tables(0).Rows.Count.ToString

                    '削除フラグにより、登録または更新する
                    If intCnt = 0 Then
                        objCmd = New SqlCommand(M_MY_DISP_ID + "_I1", objCn)
                        'パラメータ設定
                        With objCmd.Parameters
                            .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxId.ppText))                    'ＴＢＯＸＩＤ
                            .Add(pfSet_Param("reason", SqlDbType.NVarChar, Me.txtReason2.ppText))                   '監視対象外理由
                            .Add(pfSet_Param("cnfm", SqlDbType.NVarChar, Me.txtCnfm.ppText))                        '確認者
                            .Add(pfSet_Param("mntrreport_no", SqlDbType.NVarChar, Me.txtMntrReportNo.ppText))       '管理番号
                            .Add(pfSet_Param("inp_usr_cd", SqlDbType.NVarChar, Me.ddlInpUsr.SelectedValue))         '入力者
                            .Add(pfSet_Param("notetext", SqlDbType.NVarChar, Me.txtNoteText.ppText))                '備考
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                     'ユーザーＩＤ
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                       '戻り値
                        End With
                    ElseIf intCnt <> 0 Then
                        If objDs.Tables(0).Rows(0).Item("削除フラグ").ToString = 1 Then
                            objCmd = New SqlCommand(M_MY_DISP_ID + "_U1", objCn)
                            'パラメータ設定
                            With objCmd.Parameters
                                .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxId.ppText))                    'ＴＢＯＸＩＤ
                                .Add(pfSet_Param("reason", SqlDbType.NVarChar, Me.txtReason2.ppText))                   '監視対象外理由
                                .Add(pfSet_Param("cnfm", SqlDbType.NVarChar, Me.txtCnfm.ppText))                        '確認者
                                .Add(pfSet_Param("mntrreport_no", SqlDbType.NVarChar, Me.txtMntrReportNo.ppText))       '管理番号
                                .Add(pfSet_Param("inp_usr_cd", SqlDbType.NVarChar, Me.ddlInpUsr.SelectedValue))         '入力者
                                .Add(pfSet_Param("notetext", SqlDbType.NVarChar, Me.txtNoteText.ppText))                '備考
                                .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                     'ユーザーＩＤ
                                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                       '戻り値
                            End With
                        Else
                            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, intRtn.ToString)
                            Exit Sub
                        End If
                    End If

                    'データ追加
                    Using conTrn = objCn.BeginTransaction
                        objCmd.Transaction = conTrn
                        objDs = clsDataConnect.pfGet_DataSet(objCmd)

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(objCmd.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, "00008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, intRtn.ToString)
                            Exit Sub
                        End If

                        'コミット
                        conTrn.Commit()

                    End Using

                    '完了メッセージ
                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "監視対象外ホール情報")

                Catch ex As SqlException
                    'データ登録処理エラー
                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "監視対象外ホール情報")
                    '--------------------------------
                    '2014/04/14 星野　ここから
                    '--------------------------------
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    '--------------------------------
                    '2014/04/14 星野　ここまで
                    '--------------------------------

                Catch ex As Exception
                    'データ登録処理エラー
                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "監視対象外ホール情報")
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
                    If Not clsDataConnect.pfClose_Database(objCn) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                End Try
            Else
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

        End If

        '終了ログ出力
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 更新処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs)

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '開始ログ出力
        psLogStart(Me)

        '----- 追加
        Dim dtrError As DataRow
        If ddlInpUsr.SelectedIndex = 0 Then
            dtrError = ClsCMCommon.pfGet_ValMes("5003", lblInpUsr.Text)
            cuvDropDownList.Text = "未入力エラー"
            cuvDropDownList.ErrorMessage = dtrError.Item(P_VALMES_MES)
            cuvDropDownList.Enabled = True
            cuvDropDownList.IsValid = False
            cuvDropDownList.SetFocusOnError = True
        End If
        '----- 追加

        '入力チェック
        If (Page.IsValid) Then
            Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
            Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
            Dim objDs As DataSet = Nothing          'データセット
            Dim intRtn As Integer

            '確認処理付与
            Me.btnUpdate.OnClientClick =
                pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "監視対象外ホール情報")

            '接続
            If clsDataConnect.pfOpen_Database(objCn) Then
                Try
                    objCmd = New SqlCommand(M_MY_DISP_ID + "_U1", objCn)
                    'パラメータ設定
                    With objCmd.Parameters
                        .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxId.ppText))                    'ＴＢＯＸＩＤ
                        .Add(pfSet_Param("reason", SqlDbType.NVarChar, Me.txtReason2.ppText))                   '監視対象外理由
                        .Add(pfSet_Param("cnfm", SqlDbType.NVarChar, Me.txtCnfm.ppText))                        '確認者
                        .Add(pfSet_Param("mntrreport_no", SqlDbType.NVarChar, Me.txtMntrReportNo.ppText))       '管理番号
                        .Add(pfSet_Param("inp_usr_cd", SqlDbType.NVarChar, Me.ddlInpUsr.SelectedValue))         '入力者
                        .Add(pfSet_Param("notetext", SqlDbType.NVarChar, Me.txtNoteText.ppText))                '備考
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                     'ユーザーＩＤ
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                       '戻り値
                    End With

                    'データ更新
                    Using conTrn = objCn.BeginTransaction
                        objCmd.Transaction = conTrn

                        'コマンドタイプ設定(ストアド)
                        objCmd.CommandType = CommandType.StoredProcedure
                        objCmd.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(objCmd.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, "00007", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, intRtn.ToString)
                            Exit Sub
                        End If

                        'コミット
                        conTrn.Commit()

                    End Using

                    '完了メッセージ
                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "監視対象外ホール情報")

                    'データ再取得
                    msGet_Data()

                Catch ex As SqlException
                    'データ更新処理エラー
                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "監視対象外ホール情報")
                    '--------------------------------
                    '2014/04/14 星野　ここから
                    '--------------------------------
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    '--------------------------------
                    '2014/04/14 星野　ここまで
                    '--------------------------------

                Catch ex As Exception
                    'データ更新処理エラー
                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "監視対象外ホール情報")
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
                    If Not clsDataConnect.pfClose_Database(objCn) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                End Try
            Else
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 削除ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDelete_Click(sender As Object, e As EventArgs)

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '開始ログ出力
        psLogStart(Me)

        '入力チェック
        If (Page.IsValid) Then
            Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
            Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
            Dim intRtn As Integer

            '確認処理付与
            Me.btnDelete.OnClientClick =
                pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "監視対象外ホール情報")

            '接続
            If clsDataConnect.pfOpen_Database(objCn) Then
                Try
                    objCmd = New SqlCommand(M_MY_DISP_ID + "_D1", objCn)
                    'パラメータ設定
                    With objCmd.Parameters
                        .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxId.ppText))        'ＴＢＯＸＩＤ
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))         'ユーザーＩＤ
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
                    End With

                    'データ更新
                    Using conTrn = objCn.BeginTransaction
                        objCmd.Transaction = conTrn

                        'コマンドタイプ設定(ストアド)
                        objCmd.CommandType = CommandType.StoredProcedure
                        objCmd.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(objCmd.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, "00009", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, intRtn.ToString)
                            Exit Sub
                        End If

                        'コミット
                        conTrn.Commit()

                    End Using

                    '完了メッセージ
                    psMesBox(Me, "00002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "監視対象外ホール情報")

                    '登録エリアクリア
                    msClear_RegArea()

                    '--2014/04/15 中川　ここから
                    '排他情報削除
                    If Not Me.Master.Master.ppExclusiveDateDtl = String.Empty Then

                        clsExc.pfDel_Exclusive(Me,
                                        Session(P_SESSION_SESSTION_ID),
                                        Me.Master.Master.ppExclusiveDateDtl)

                        Me.Master.Master.ppExclusiveDateDtl = String.Empty

                    End If
                    '--2014/04/15 中川　ここまで

                    'データ再取得
                    msGet_Data()

                Catch ex As Exception
                    'データ削除処理エラー
                    psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "監視対象外ホール情報")
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
                    If Not clsDataConnect.pfClose_Database(objCn) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                End Try
            Else
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' CSVボタン押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCSV_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        Dim fileName As String = "監視対象外ホール一覧" 'ファイル名
        Dim dt As New DataTable
        Try
            'ヘッダ作成
            dt.Columns.Add("「出力日」")
            dt.Columns.Add("「NL区分」")
            dt.Columns.Add("「TBOXID」")
            dt.Columns.Add("「ホール名」")
            dt.Columns.Add("「システム」")
            dt.Columns.Add("「監視対象外理由」")
            dt.Columns.Add("「確認先」")
            dt.Columns.Add("「管理番号」")
            dt.Columns.Add("「入力者」")
            dt.Columns.Add("「入力日時」")
            dt.Columns.Add("「備考」")
            dt.Columns.Add("「削除」")
            '行作成
            'グリッドビューの件数分ループ
            For Each rowData As GridViewRow In grvList.Rows
                Dim dtRow As DataRow = dt.NewRow()
                'データ行に設定
                dtRow("「出力日」") = DateTime.Now.ToString("yyyy/MM/dd")
                dtRow("「NL区分」") = CType(rowData.FindControl("ＮＬ区分"), TextBox).Text
                dtRow("「TBOXID」") = CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text
                dtRow("「ホール名」") = CType(rowData.FindControl("ホール名"), TextBox).Text
                dtRow("「システム」") = CType(rowData.FindControl("システム"), TextBox).Text
                dtRow("「監視対象外理由」") = CType(rowData.FindControl("監視対象外理由"), TextBox).Text
                dtRow("「確認先」") = CType(rowData.FindControl("確認先"), TextBox).Text
                dtRow("「管理番号」") = CType(rowData.FindControl("管理番号"), TextBox).Text
                dtRow("「入力者」") = CType(rowData.FindControl("入力者"), TextBox).Text
                dtRow("「入力日時」") = CType(rowData.FindControl("入力日時"), TextBox).Text
                dtRow("「備考」") = CType(rowData.FindControl("備考"), TextBox).Text
                dtRow("「削除」") = CType(rowData.FindControl("削除フラグ"), TextBox).Text
                'データテーブルにセット
                dt.Rows.Add(dtRow)
            Next
            'pfDLCSV(ファイル名,管理番号,データテーブル,ヘッダ有無,出力ページ)
            If pfDLCSV(fileName, Nothing, dt, True, Me) <> 0 Then
                psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_MSG_CSV) 'ＣＳＶの出力に失敗
            End If

        Catch ex As Threading.ThreadAbortException

        Catch ex As Exception

            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_MSG_CSV) 'ＣＳＶの出力に失敗

        Finally

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub

    ''' <summary>
    ''' ＴＢＯＸＩＤ変更時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtTboxId_TextChanged(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        Dim strbuff As String = String.Empty    'ホール名取得用

        '開始ログ出力
        psLogStart(Me)

        If mfGet_TboxInfo() Then
            Me.txtReason2.ppTextBox.Focus()
        Else
            Me.txtTboxId.ppText = String.Empty
            Me.txtTboxId.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
            Me.txtTboxId.ppTextBox.Focus()
        End If

        '-----------------------------
        '2014/06/20 武　ここから
        '-----------------------------
        msSet_DtlMode("1")
        '-----------------------------
        '2014/06/20 武　ここまで
        '-----------------------------
        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' グリッドのボタン押下時処理
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

        '開始ログ出力
        psLogStart(Me)

        Try
            Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    'ボタン押下行のIndex
            Dim rowData As GridViewRow = grvList.Rows(intIndex)             'ボタン押下行

            Select Case e.CommandName
                Case "btnSelect"

                    '--2014/04/15 中川　ここから
                    '排他制御用変数
                    Dim strExclusiveDate As String = Nothing
                    Dim arTable_Name As New ArrayList
                    Dim arKey As New ArrayList

                    '排他情報削除
                    If Not Me.Master.Master.ppExclusiveDateDtl = String.Empty Then

                        clsExc.pfDel_Exclusive(Me,
                                        Session(P_SESSION_SESSTION_ID),
                                        Me.Master.Master.ppExclusiveDateDtl)

                        Me.Master.Master.ppExclusiveDateDtl = String.Empty

                    End If

                    'NGCは排他対象外
                    '--------------------------------
                    '2014/06/19 星野　ここから
                    '--------------------------------
                    'Select Case Session(P_SESSION_AUTH)
                    '    '--------------------------------
                    '    '2014/06/18 武　ここから
                    '    '--------------------------------
                    '    'Case "管理者", "SPC", "営業所"
                    '    Case "管理者", "SPC", "NGC"
                    '        '--------------------------------
                    '        '2014/06/18 武　ここまで
                    '        '--------------------------------

                    '        'ロック対象テーブル名の登録
                    '        arTable_Name.Insert(0, "T02_HALL_EXCLD")

                    '        'ロックテーブルキー項目の登録
                    '        '--- 変更 2014/05/27 START ---
                    '        arKey.Insert(0, CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text)     'ＴＢＯＸＩＤ
                    '        arKey.Insert(1, CType(rowData.FindControl("ＮＬ"), TextBox).Text)             'ＮＬ区分
                    '        arKey.Insert(2, CType(rowData.FindControl("システム分類"), TextBox).Text)     'システム分類
                    '        '--- 変更 2014/05/27 END   ---

                    '        '排他情報確認処理
                    '        If clsExc.pfSel_Exclusive(strExclusiveDate,
                    '                           Me,
                    '                           Session(P_SESSION_IP),
                    '                           Session(P_SESSION_PLACE),
                    '                           Session(P_SESSION_USERID),
                    '                           Session(P_SESSION_SESSTION_ID),
                    '                           ViewState(P_SESSION_GROUP_NUM),
                    '                           M_MY_DISP_ID,
                    '                           arTable_Name,
                    '                           arKey) = 0 Then

                    '            Me.lblNLCls2.Text = DirectCast(rowData.FindControl("ＮＬ区分"), TextBox).Text
                    '            Me.txtTboxId.ppText = DirectCast(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text
                    '            Me.lblHallNm2.Text = DirectCast(rowData.FindControl("ホール名"), TextBox).Text
                    '            Me.lblSystem2.Text = DirectCast(rowData.FindControl("システム"), TextBox).Text
                    '            Me.txtReason2.ppText = DirectCast(rowData.FindControl("監視対象外理由"), TextBox).Text
                    '            Me.txtCnfm.ppText = DirectCast(rowData.FindControl("確認先"), TextBox).Text
                    '            Me.txtMntrReportNo.ppText = DirectCast(rowData.FindControl("管理番号"), TextBox).Text
                    '            Me.txtNoteText.ppText = DirectCast(rowData.FindControl("備考"), TextBox).Text
                    '            Me.ddlInpUsr.SelectedValue = DirectCast(rowData.FindControl("入力者"), TextBox).Text

                    '            Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate
                    '            Me.Master.Master.ppExclusiveDateDtl = Session(P_SESSION_EXCLUSIV_DATE)

                    '        Else
                    '            '排他ロック中
                    '            Exit Sub

                    '        End If
                    '        '--2014/04/15 中川　ここまで
                    'End Select

                    Dim intExFlg As Integer = 0     '排他判断用フラグ

                    Select Case Session(P_SESSION_AUTH)
                        Case "管理者", "SPC"

                            'ロック対象テーブル名の登録
                            arTable_Name.Insert(0, "T02_HALL_EXCLD")

                            'ロックテーブルキー項目の登録
                            arKey.Insert(0, CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text)     'ＴＢＯＸＩＤ
                            arKey.Insert(1, CType(rowData.FindControl("ＮＬ"), TextBox).Text)             'ＮＬ区分
                            arKey.Insert(2, CType(rowData.FindControl("システム分類"), TextBox).Text)     'システム分類

                            '排他情報確認処理
                            If clsExc.pfSel_Exclusive(strExclusiveDate,
                                               Me,
                                               Session(P_SESSION_IP),
                                               Session(P_SESSION_PLACE),
                                               Session(P_SESSION_USERID),
                                               Session(P_SESSION_SESSTION_ID),
                                               ViewState(P_SESSION_GROUP_NUM),
                                               M_MY_DISP_ID,
                                               arTable_Name,
                                               arKey) = 0 Then

                                Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate
                                Me.Master.Master.ppExclusiveDateDtl = Session(P_SESSION_EXCLUSIV_DATE)

                            Else
                                '排他ロック中
                                intExFlg = 1
                            End If
                    End Select

                    If intExFlg = 0 Then
                        Me.lblNLCls2.Text = DirectCast(rowData.FindControl("ＮＬ区分"), TextBox).Text
                        Me.txtTboxId.ppText = DirectCast(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text
                        Me.lblHallNm2.Text = DirectCast(rowData.FindControl("ホール名"), TextBox).Text
                        Me.lblSystem2.Text = DirectCast(rowData.FindControl("システム"), TextBox).Text
                        Me.txtReason2.ppText = DirectCast(rowData.FindControl("監視対象外理由"), TextBox).Text
                        Me.txtCnfm.ppText = DirectCast(rowData.FindControl("確認先"), TextBox).Text
                        Me.txtMntrReportNo.ppText = DirectCast(rowData.FindControl("管理番号"), TextBox).Text
                        Me.txtNoteText.ppText = DirectCast(rowData.FindControl("備考"), TextBox).Text
                        Me.ddlInpUsr.SelectedValue = DirectCast(rowData.FindControl("入力者"), TextBox).Text
                    End If

                    '--------------------------------
                    '2014/06/19 星野　ここまで
                    '--------------------------------

                    '--------------------------------
                    '2014/06/20　武　ここから
                    '--------------------------------
                    msSet_DtlMode("2")
                    '--------------------------------
                    '2014/06/20　武　ここまで
                    '--------------------------------

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
        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        '削除フラグがある行の更新を非活性にする
        Dim strDelF As String
        For Each rowData As GridViewRow In grvList.Rows
            strDelF = CType(rowData.FindControl("削除フラグ"), TextBox).Text
            If strDelF = "●" Then   '削除フラグあり
                rowData.Cells(0).Enabled = False
            End If
        Next

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClear_Screen()

        Me.tftTboxId.ppFromText = String.Empty          'ＴＢＯＸＩＤFrom
        Me.tftTboxId.ppToText = String.Empty            'ＴＢＯＸＩＤTo
        Me.ddlNLCls.ppSelectedValue = String.Empty      'ＮＬ区分
        Me.ddlSystem.SelectedValue = String.Empty       'システム
        Me.txtReason1.ppText = String.Empty             '監視対象外理由
        Me.cbxDeleteFlgY.Checked = False                '削除フラグ（有効）
        Me.cbxDeleteFlgN.Checked = False                '削除フラグ（削除）
        Me.txtTboxId.ppText = String.Empty              'ＴＢＯＸＩＤ
        Me.lblNLCls2.Text = String.Empty                'ＮＬ区分
        Me.lblSystem2.Text = String.Empty               'システム
        Me.lblHallNm2.Text = String.Empty               'ホール名
        Me.txtReason2.ppText = String.Empty             '監視対象外理由
        Me.txtCnfm.ppText = String.Empty                '確認者
        Me.txtMntrReportNo.ppText = String.Empty        '管理番号
        Me.ddlInpUsr.SelectedValue = String.Empty       '入力者
        Me.txtNoteText.ppText = String.Empty            '備考
        Me.grvList.DataSource = New DataTable
        Master.ppCount = "0"
        Me.grvList.DataBind()
        Me.tftTboxId.ppTextBoxFrom.Focus()

        '--------------------------------
        '2014/05/02 高松　ここから
        '--------------------------------
        Me.btnCSV.CausesValidation = False              'CSVボタン検証チェック無し
        Me.btnCSV.Enabled = False                       'CSVボタン初期は非活性
        Me.btnCSV.OnClientClick = pfGet_OCClickMes("10001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, M_MSG_CSV)
        '--------------------------------
        '2014/05/02 高松　ここまで
        '--------------------------------
        '-----------------------------
        '2014/06/17 武　ここから
        '-----------------------------
        msSet_DtlMode("1")
        '-----------------------------
        '2014/06/17 武　ここまで
        '-----------------------------

    End Sub

    '--------------------------------
    '2014/06/20 武　ここから
    '--------------------------------
    ''' <summary>
    ''' 物品機材登録一覧のボタン制御
    ''' </summary>
    ''' <param name="ipstrMode"></param>
    ''' <remarks></remarks>
    Private Sub msSet_DtlMode(ByVal ipstrMode As String)
        If ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.参照 Then
            Select Case ipstrMode

                Case "1"
                    Me.btnAdd.Enabled = True                    '追加(明細)ボタン
                    Me.btnUpdate.Enabled = False                '更新(明細)ボタン
                    Me.btnDelete.Enabled = False                '削除(明細)ボタン
                Case "2"
                    Me.btnAdd.Enabled = False                   '追加(明細)ボタン
                    Me.btnUpdate.Enabled = True                 '更新(明細)ボタン
                    Me.btnDelete.Enabled = True                 '削除(明細)ボタン
            End Select
        Else
            Me.btnAdd.Enabled = False                   '追加(明細)ボタン
            Me.btnUpdate.Enabled = False                 '更新(明細)ボタン
            Me.btnDelete.Enabled = False                 '削除(明細)ボタン
        End If
    End Sub
    '--------------------------------
    '2014/06/20　武　ここまで
    '--------------------------------

    ''' <summary>
    ''' 検索画面クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClear_SearchArea()

        Me.tftTboxId.ppFromText = String.Empty          'ＴＢＯＸＩＤFrom
        Me.tftTboxId.ppToText = String.Empty            'ＴＢＯＸＩＤTo
        Me.ddlNLCls.ppSelectedValue = String.Empty      'ＮＬ区分
        Me.ddlSystem.SelectedValue = String.Empty       'システム
        Me.txtReason1.ppText = String.Empty             '監視対象外理由
        Me.cbxDeleteFlgY.Checked = False                '削除フラグ（有効）
        Me.cbxDeleteFlgN.Checked = False                '削除フラグ（削除）
        '--------------------------------
        '2014/05/02 高松　ここから
        '--------------------------------
        'Me.btnCSV.Enabled = False                       'CSVボタン非活性
        '--------------------------------
        '2014/05/02 高松　ここまで
        '--------------------------------

    End Sub
    ''' <summary>
    ''' 画面登録エリアクリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClear_RegArea()

        Me.txtTboxId.ppText = String.Empty              'ＴＢＯＸＩＤ
        Me.lblNLCls2.Text = String.Empty                'ＮＬ区分
        Me.lblSystem2.Text = String.Empty               'システム
        Me.lblHallNm2.Text = String.Empty               'ホール名
        Me.txtReason2.ppText = String.Empty             '監視対象外理由
        Me.txtCnfm.ppText = String.Empty                '確認者
        Me.txtMntrReportNo.ppText = String.Empty        '管理番号
        Me.ddlInpUsr.SelectedValue = String.Empty       '入力者
        Me.txtNoteText.ppText = String.Empty            '備考
        Me.txtTboxId.ppTextBox.Focus()

    End Sub

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data()

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As New DataSet                'データセット
        Dim strDeleteFlg As String = Nothing

        If cbxDeleteFlgY.Checked = True And cbxDeleteFlgN.Checked = False Then
            strDeleteFlg = "0"
        ElseIf cbxDeleteFlgY.Checked = False And cbxDeleteFlgN.Checked = True Then
            strDeleteFlg = "1"
        Else
            strDeleteFlg = String.Empty
        End If

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'DB接続
        If clsDataConnect.pfOpen_Database(objCn) Then

            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S1", objCn)
                'パラメータ設定
                With objCmd.Parameters
                    'ＴＢＯＸＩＤFrom
                    If Me.tftTboxId.ppToText = String.Empty Then
                        'ＴＢＯＸＩＤToが空白の場合は「あいまい検索」なのでエスケープする
                        .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar _
                                         , Me.tftTboxId.ppFromText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    Else
                        'ＴＢＯＸＩＤToが空白でないの場合は「範囲検索」なのでエスケープしない
                        .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, Me.tftTboxId.ppFromText))
                    End If
                    'ＴＢＯＸＩＤTo
                    .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, Me.tftTboxId.ppToText))
                    'ＮＬ区分
                    .Add(pfSet_Param("nl_cls_cd", SqlDbType.NVarChar, Me.ddlNLCls.ppSelectedValue))
                    'システム
                    .Add(pfSet_Param("tbox_cd", SqlDbType.NVarChar, Me.ddlSystem.SelectedValue))
                    '監視対象外理由
                    .Add(pfSet_Param("reason", SqlDbType.NVarChar _
                                     , Me.txtReason1.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    '削除フラグ
                    .Add(pfSet_Param("delete_flg", SqlDbType.NVarChar, strDeleteFlg))
                    '画面ＩＤ
                    .Add(pfSet_Param("disp_id", SqlDbType.NVarChar, M_MY_DISP_ID))
                    '初期表示フラグ
                    .Add(pfSet_Param("default", SqlDbType.NVarChar, "0"))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '件数を設定
                Master.ppCount = objDs.Tables(0).Rows.Count.ToString

                '検索結果データなし判定
                If objDs.Tables(0).Rows.Count = 0 Then
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    Me.btnCSV.Enabled = False
                Else
                    '--------------------------------
                    '2014/05/02 高松　ここから
                    '--------------------------------
                    Me.btnCSV.Enabled = True                       'CSVボタン活性
                    '--------------------------------
                    '2014/05/02 高松　ここまで
                    '--------------------------------
                End If

                '取得したデータをグリッドに設定
                Me.grvList.DataSource = objDs.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            Catch ex As SqlException
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "監視対象外ホール情報")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Me.grvList.DataSource = Nothing
                Me.grvList.DataBind()
                Master.ppCount = "0"

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "監視対象外ホール情報")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Me.grvList.DataSource = Nothing
                Me.grvList.DataBind()
                Master.ppCount = "0"
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Me.grvList.DataSource = Nothing
            Me.grvList.DataBind()
            Master.ppCount = "0"
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（システム）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_ddlSystem()

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL011", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ＴＢＯＸリスト、ＴＢＯＸ機種コードでDistinctする。（重複データをカット）
                Dim dv As DataView = objDs.Tables(0).DefaultView
                Dim dt As DataTable = dv.ToTable("ＴＢＯＸ", True, "ＴＢＯＸリスト", "ＴＢＯＸ機種コード")

                'ドロップダウンリスト設定
                Me.ddlSystem.Items.Clear()
                Me.ddlSystem.DataSource = dt
                Me.ddlSystem.DataTextField = "ＴＢＯＸリスト"
                Me.ddlSystem.DataValueField = "ＴＢＯＸ機種コード"
                Me.ddlSystem.DataBind()

                '先頭に空白行を追加
                Me.ddlSystem.Items.Insert(0, New ListItem(Nothing, Nothing))

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸマスタ一覧取得")
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
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（入力者）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_ddlInpUsr()

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S3", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '社員リスト、社員コードでDistinctする。（重複データをカット）
                Dim dv As DataView = objDs.Tables(0).DefaultView
                Dim dt As DataTable = dv.ToTable("ＳＰＣ社員", True, "社員リスト", "社員コード")

                'ドロップダウンリスト設定
                Me.ddlInpUsr.Items.Clear()
                Me.ddlInpUsr.DataSource = dt
                Me.ddlInpUsr.DataTextField = "社員リスト"
                Me.ddlInpUsr.DataValueField = "社員コード"
                Me.ddlInpUsr.DataBind()

                '先頭に空白行を追加
                Me.ddlInpUsr.Items.Insert(0, New ListItem(Nothing, Nothing))

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "社員マスタ一覧取得")
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
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ＴＢＯＸ情報取得処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGet_TboxInfo() As Boolean

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim dtRow As DataRow = Nothing          'DataRow
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGet_TboxInfo = False
        '--------------------------------
        '2014/06/20 武　ここから
        '--------------------------------
        Me.lblNLCls2.Text = Nothing
        Me.lblSystem2.Text = Nothing
        Me.lblHallNm2.Text = Nothing
        Me.txtReason2.ppText = Nothing             '監視対象外理由
        Me.txtCnfm.ppText = Nothing                '確認者
        Me.txtMntrReportNo.ppText = Nothing        '管理番号
        Me.ddlInpUsr.SelectedValue = Nothing       '入力者
        Me.txtNoteText.ppText = Nothing

        '未入力チェック
        'If Me.txtTboxId.ppText = Nothing Then
        '    'ＮＬ区分、システム、ホール名設定
        '    'Me.lblNLCls2.Text = Nothing
        '    'Me.lblSystem2.Text = Nothing
        '    'Me.lblHallNm2.Text = Nothing

        '    mfGet_TboxInfo = True
        'End If

        '--------------------------------
        '2014/06/20 武　ここまで
        '--------------------------------

        'DB接続
        If clsDataConnect.pfOpen_Database(objCn) Then
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S2", objCn)
                'パラメータ設定
                With objCmd.Parameters
                    'ＴＢＯＸＩＤ
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxId.ppText))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables(0).Rows.Count = 0 Then
                    Exit Function
                End If

                dtRow = objDs.Tables(0).Rows(0)

                'ＮＬ区分、システム、ホール名設定
                Me.lblNLCls2.Text = dtRow("ＮＬ区分").ToString
                Me.lblSystem2.Text = dtRow("システム").ToString
                Me.lblHallNm2.Text = dtRow("ホール名").ToString

                mfGet_TboxInfo = True

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ホール情報取得")
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
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Function

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
