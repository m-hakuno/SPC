'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜修理・整備業務＞
'*　処理名　　：　修理依頼書
'*　ＰＧＭＩＤ：　REPUPDP002
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.17　：　後藤
'*  更　新　　：　2017.08.01　：　間瀬　新規登録時の初期表示の内容や、エラーチェックの修正
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'                   2017.08.01　  　間瀬　   新規登録時の初期表示の内容や、エラーチェックの修正
'REPUPDP002-001     2016/01/22      栗原     帳票の印字項目連絡先をマスタ参照するよう変更
'REPUPDP002-002     2017/08/01      伯野     保守機ＴＢＯＸＩＤに対応　ＴＢＯＸＩＤに英字を許可

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive
#End Region

Public Class REPUPDP002
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
    '画面ID.
    Private Const M_DISP_ID = P_FUN_REP & P_SCR_UPD & P_PAGE & "002"

    Private Const sCnsClass As String = "CLASSCD" '機器種別コード.
    Private Const sCnsHall As String = "HALLCD"   'ホールコード.
    Private Const sCnsSys As String = "SYSTEM"    'システムコード.
    Private Const sCnsCompFlg As String = "TERMS" '会社名フラグ(0:非活性、1:活性).
    Private Const sCnsReqDt As String = "REQDT"   '請求年月.
    Private Const sCnsSeqNo As String = "SEQNO"   '連番.

    ''' <summary>引継情報(枝番)</summary>
    Private Const P_KEY2 As String = "ＫＥＹ２"

    ''' <summary>共有情報(枝番)</summary>
    Private Const P_BRANCH As String = "BRANCH"

    ''' <summary>進捗ステータスマスタ情報取得</summary>
    Private Const sCnsSqlid_015 As String = "ZCMPSEL015"
    ''' <summary>業者マスタ情報取得</summary>
    Private Const sCnsSqlid_040 As String = "ZCMPSEL040"
    ''' <summary>申告内容マスタ情報取得</summary>
    Private Const sCnsSqlid_018 As String = "ZCMPSEL018"
    ''' <summary>倉庫マスタ情報取得</summary>
    Private Const sCnsSqlid_039 As String = "ZCMPSEL039"
    ''' <summary>部品マスタ情報取得</summary>
    Private Const sCnsSqlid_027 As String = "ZCMPSEL027"
    ''' <summary>ホール情報取得</summary>
    Private Const sCnsSqlid_028 As String = "ZCMPSEL028"
    ''' <summary>完了送付先情報取得</summary>
    Private Const sCnsSqlid_032 As String = "ZCMPSEL032"
    Private Const sCnsSqlid_062 As String = "ZCMPSEL062"

    'REPUPDP002-001 過去帳票の納入先とマスタ情報結合用
    Private Const OLD_FS_COD As String = "90-01"
    Private Const NEW_FS_COD As String = "00-02"
    Private Const OLD_BW_COD As String = "90-02"
    Private Const NEW_BW_COD As String = "00-06"
    'REPUPDP002-001 END

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
    ''' Page_Init.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '表設定
        pfSet_GridView(Me.grvList, M_DISP_ID)
    End Sub

    ''' <summary>
    ''' ロード処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet

        Dim strKey As String = String.Empty
        Dim strKey2 As String = String.Empty
        Dim strId As String = String.Empty
        Dim strTerms As String = String.Empty
        Dim strData As String

        If Not IsPostBack Then '初回表示.

            If Session(P_KEY) Is Nothing Then
                psMesBox(Me, "20004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画前)
                '--------------------------------
                '2014/06/11 後藤　ここから
                '--------------------------------
                '排他削除
                clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                '--------------------------------
                '2014/06/11 後藤　ここまで
                '--------------------------------
                psClose_Window(Me)
                Return
            End If

            'セッション項目取得
            ViewState(P_KEY) = DirectCast(Session(P_KEY), String())(0)
            ViewState(P_SESSION_OLDDISP) = Session(P_SESSION_OLDDISP)

            'ビューステート項目取得
            strKey = ViewState(P_KEY)
            strId = ViewState(P_SESSION_OLDDISP)

            'プログラムＩＤ、画面名設定.
            Master.ppProgramID = M_DISP_ID
            Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

            'パンくずリスト設定.
            Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

            '--------------------------------
            '2014/05/14 後藤　ここから
            '--------------------------------
            'ボタン活性
            Master.ppRigthButton1.Text = "登録"
            Master.ppRigthButton1.Visible = True
            Master.ppRigthButton2.Text = "クリア"
            Master.ppRigthButton2.Visible = True
            Master.ppLeftButton1.Text = "印刷"
            Master.ppLeftButton1.Visible = True
            '--------------------------------
            '2014/05/14 後藤　ここまで
            '--------------------------------

            ddlStatus.Items.Add("")
            ddlStatus.Items.Add("1:故障")
            ddlStatus.Items.Add("2:調査中")
            ddlStatus.Items.Add("3:TOK")

            '排他情報用のグループ番号保管.
            If Not Session(P_SESSION_GROUP_NUM) Is Nothing Then
                ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)
            End If

            '排他情報削除用の登録年月日時刻をマスタページのコントロールに登録.
            If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then
                Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)
            End If

            '進捗一覧からの場合は枝番を取得、進捗以外は１を指定.
            If Session(P_SESSION_OLDDISP) = "REPLSTP001" Then
                ViewState(P_KEY2) = DirectCast(Session(P_KEY), String())(1)
                strKey2 = ViewState(P_KEY2)
                ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)
                strTerms = ViewState(P_SESSION_TERMS)
                '画面初期化処理.
                Call msInitScreen()

                '修理依頼データ取得処理.
                Call msGet_Data(strKey, strKey2, "")

                '修理依頼明細データ取得処理.
                Call msSetList_Data(strKey, strKey2)

                '活性制御
                Call msEnableScreen(strTerms.Trim)
            Else

                '画面初期化処理.
                Call msInitScreen()

                '●●● 稲葉 Add：ここから---------------------------------------------------
                'strKey2 = "1"

                '接続
                If Not clsDataConnect.pfOpen_Database(conDB) Then
                    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    Exit Sub
                Else
                    Try
                        '修理依頼データ取得.
                        cmdDB = New SqlCommand(M_DISP_ID & "_S10", conDB)
                        cmdDB.Parameters.Add(pfSet_Param("repair_no", SqlDbType.NVarChar, strKey))
                        dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                        strKey2 = dstOrders.Tables(0).Rows(0).Item("最大枝番").ToString
                        If strKey2 = "0" Then
                            strKey2 = "1"
                        End If
                    Catch ex As Exception
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    Finally
                        'DB切断
                        If Not clsDataConnect.pfClose_Database(conDB) Then
                            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        End If
                    End Try
                End If
                '●●● 稲葉 Add：ここまで---------------------------------------------------

                '修理依頼データ取得処理.
                Call msGet_Data(strKey, strKey2, strTerms)

                '修理依頼明細データ取得処理.
                Call msSetList_Data(strKey, strKey2)

                '接続
                If Not clsDataConnect.pfOpen_Database(conDB) Then
                    psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                Else
                    Try

                        '修理依頼データ取得.
                        cmdDB = New SqlCommand(M_DISP_ID & "_S10", conDB)
                        cmdDB.Parameters.Add(pfSet_Param("repair_no", SqlDbType.NVarChar, strKey))
                        dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                        strData = (Integer.Parse(dstOrders.Tables(0).Rows(0).Item("最大枝番").ToString) + 1).ToString

                        If Integer.Parse(strData) <> Integer.Parse(strKey2) Then
                            Me.txtBranch.ppText = strData
                            cmdDB = New SqlCommand(M_DISP_ID & "_S8", conDB)
                            cmdDB.Parameters.Add(pfSet_Param("repair_no", SqlDbType.NVarChar, Me.lblRepair_No.Text))
                            cmdDB.Parameters.Add(pfSet_Param("branch", SqlDbType.NVarChar, Me.txtBranch.ppText))

                            'リストデータ取得
                            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                            '0件の場合は登録.
                            If dstOrders.Tables(0).Rows.Count <> 0 Then

                                'コントロールにセット.
                                Call msSetDisp_Data(dstOrders)

                                '交換部品リスト再検索.
                                Call msSetList_Data(Me.lblRepair_No.Text, Me.txtBranch.ppText)

                                '更新で表示.
                                Call msEnableScreen(ClsComVer.E_遷移条件.更新)

                            Else
                                '通知完了登録、交換部品クリア.
                                Call msInitScreen(False)

                                '登録で表示.
                                Call msEnableScreen(ClsComVer.E_遷移条件.登録)
                            End If

                            '枝番保管.
                            ViewState(P_BRANCH) = Me.lblRepair_No.Text

                            If Master.ppRigthButton1.Text = "更新" Then
                                Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("00004", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "修理依頼書")
                            Else
                                Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("00008", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "修理依頼書")
                            End If
                        Else
                            '活性制御
                            If strTerms.Trim = "登録" Then
                                Call msEnableScreen(ClsComVer.E_遷移条件.登録)
                            Else
                                Call msEnableScreen(ClsComVer.E_遷移条件.更新)
                            End If
                        End If

                    Catch ex As Exception
                        psMesBox(Me, "00004", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後)
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    Finally
                        'DB切断
                        If Not clsDataConnect.pfClose_Database(conDB) Then
                            psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                        End If

                    End Try

                End If

            End If

        End If

        'ボタンアクションの設定.
        Call msSet_ButtonAction()

    End Sub

    '---------------------------
    '2014/04/24 武 ここから
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
            Case "営業所", "NGC"
                txtBranch.ppEnabled = False
                dtbTroubleDt.ppEnabled = False
                txtTboxid.ppEnabled = False
                ddlProductNm.Enabled = False
                txtHallNm.ppEnabled = False
                txtOldVer.ppEnabled = False
                txtNewVer.ppEnabled = False
                txtSerial.ppEnabled = False
                ddlTrouble.Enabled = False
                txtTrouble.Enabled = False
                cuvTraderCd.Enabled = False
                ddlTraderCd.Enabled = False
                txtZipNo.Enabled = False
                txtAddr.Enabled = False
                txtTel.Enabled = False
                txtCharge.Enabled = False
                ddlCompNm.Enabled = False
                txtRepairContent.Enabled = False
                txtRepairCharge.ppEnabled = False
                dtbAppasendDt.ppEnabled = False
                txtVer1.ppEnabled = False
                txtHddNo1.ppEnabled = False
                txtHddCls1.ppEnabled = False
                dtbAppaarvDt.ppEnabled = False
                ddlWrkNo11.Enabled = False
                ddlWrkNo12.Enabled = False
                ddlWrkNo13.Enabled = False
                ddlWrkNo14.Enabled = False
                ddlWrkNo21.Enabled = False
                ddlPartsNo1.Enabled = False
                ddlTmpResult.ppEnabled = False
                dtbRsltSndDt.ppEnabled = False
                ddlCmprtnCd.Enabled = False
                dtbCmpSndDt.ppEnabled = False
                txtVer2.ppEnabled = False
                txtHddNo2.ppEnabled = False
                txtHddCls2.ppEnabled = False
                dtbArrivalDt.ppEnabled = False
                txtNewSerial.ppEnabled = False
                ddlInsResult.ppEnabled = False
                txtSubNo.ppEnabled = False
                ddlTokCls.ppEnabled = False
                ddlStatusCd.Enabled = False
                txtReqDt.ppEnabled = False
                ddlPartsNm.Enabled = False
                txtQuantity.ppEnabled = False
                'btnClear.Enabled = False
                btnInsert.Enabled = False
                btnChange.Enabled = False
                btnDelete.Enabled = False
                Master.ppLeftButton1.Enabled = False
                '--------------------------------
                '2014/05/19 後藤　ここから
                '--------------------------------
                'btnUpdate.Enabled = False
                Master.ppRigthButton1.Enabled = False
                'btnAllClear.Enabled = False
                Master.ppRigthButton2.Enabled = False
                '--------------------------------
                '2014/05/19 後藤　ここまで
                '--------------------------------
                'Case "NGC"
                '    txtBranch.ppEnabled = False
                '    dtbTroubleDt.ppEnabled = False
                '    txtTboxid.ppEnabled = False
                '    ddlProductNm.Enabled = False
                '    txtHallNm.ppEnabled = False
                '    txtOldVer.ppEnabled = False
                '    txtNewVer.ppEnabled = False
                '    txtSerial.ppEnabled = False
                '    ddlTrouble.Enabled = False
                '    txtTrouble.Enabled = False
                '    cuvTraderCd.Enabled = False
                '    ddlTraderCd.Enabled = False
                '    txtZipNo.Enabled = False
                '    txtAddr.Enabled = False
                '    txtTel.Enabled = False
                '    txtCharge.Enabled = False
                '    ddlCompNm.Enabled = False
                '    txtRepairContent.Enabled = False
                '    txtRepairCharge.ppEnabled = False
                '    dtbAppasendDt.ppEnabled = False
                '    txtVer1.ppEnabled = False
                '    txtHddNo1.ppEnabled = False
                '    txtHddCls1.ppEnabled = False
                '    dtbAppaarvDt.ppEnabled = False
                '    ddlWrkNo11.Enabled = False
                '    ddlWrkNo12.Enabled = False
                '    ddlWrkNo13.Enabled = False
                '    ddlWrkNo14.Enabled = False
                '    ddlWrkNo21.Enabled = False
                '    ddlPartsNo1.Enabled = False
                '    ddlTmpResult.ppEnabled = False
                '    dtbRsltSndDt.ppEnabled = False
                '    ddlCmprtnCd.Enabled = False
                '    dtbCmpSndDt.ppEnabled = False
                '    txtVer2.ppEnabled = False
                '    txtHddNo2.ppEnabled = False
                '    txtHddCls2.ppEnabled = False
                '    dtbArrivalDt.ppEnabled = False
                '    txtNewSerial.ppEnabled = False
                '    ddlInsResult.ppEnabled = False
                '    txtSubNo.ppEnabled = False
                '    ddlTokCls.ppEnabled = False
                '    ddlStatusCd.Enabled = False
                '    txtReqDt.ppEnabled = False
                '    ddlPartsNm.Enabled = False
                '    txtQuantity.ppEnabled = False
                '    btnClear.Enabled = False
                '    btnInsert.Enabled = False
                '    btnChange.Enabled = False
                '    btnDelete.Enabled = False
                '    '--------------------------------
                '    '2014/05/19 後藤　ここから
                '    '--------------------------------
                '    'btnUpdate.Enabled = False
                '    Master.ppRigthButton1.Enabled = False
                '    'btnAllClear.Enabled = False
                '    Master.ppRigthButton2.Enabled = False
                '    '--------------------------------
                '    '2014/05/19 後藤　ここまで
                '    '--------------------------------
                Me.pnlRepair.Enabled = True
                Me.pnlComplete.Enabled = True
        End Select

    End Sub
    '---------------------------
    '2014/04/24 武 ここまで
    '---------------------------

    ''' <summary>
    ''' 枝番変更時処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtBranch_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet
        Dim intCount As Integer = 0
        Dim strErr As String
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '枝番形式チェック.
        strErr = pfCheck_TxtErr(Me.txtBranch.ppText, True, True, True, False, 2, "", False)
        If strErr <> String.Empty Then
            Me.txtBranch.psSet_ErrorNo(strErr, "枝番")
            Exit Sub
        End If

        Try

            '開始ログ出力.
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            Else

                cmdDB = New SqlCommand(M_DISP_ID & "_S8", conDB)
                cmdDB.Parameters.Add(pfSet_Param("repair_no", SqlDbType.NVarChar, Me.lblRepair_No.Text))
                cmdDB.Parameters.Add(pfSet_Param("branch", SqlDbType.NVarChar, Me.txtBranch.ppText))

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '0件の場合は登録.
                If dstOrders.Tables(0).Rows.Count <> 0 Then

                    'ドロップダウンリスト再生成.
                    Call msGet_CompDropListData_Sel(dstOrders.Tables(0).Rows(0).Item("メーカーコード").ToString())

                    'コントロールにセット.
                    Call msSetDisp_Data(dstOrders)

                    '交換部品リスト再検索.
                    Call msSetList_Data(Me.lblRepair_No.Text, Me.txtBranch.ppText)

                    '更新で表示.
                    Call msEnableScreen(ClsComVer.E_遷移条件.更新)

                Else
                    '通知完了登録、交換部品クリア.
                    Call msInitScreen(False)

                    '●●● 稲葉 Add：ここから-------------------------------------------------
                    ViewState(sCnsCompFlg) = "1"
                    '●●● 稲葉 Add：ここまで-------------------------------------------------

                    '登録で表示.
                    Call msEnableScreen(ClsComVer.E_遷移条件.登録)
                End If

                '枝番保管.
                ViewState(P_BRANCH) = Me.lblRepair_No.Text

                'ボタンアクション再設定.
                'If Me.btnUpdate.Text = "更新" Then
                '    Me.btnUpdate.OnClientClick = pfGet_OCClickMes("00004", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "修理依頼書")
                'Else
                '    Me.btnUpdate.OnClientClick = pfGet_OCClickMes("00008", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "修理依頼書")
                'End If

                If Master.ppRigthButton1.Text = "更新" Then
                    Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("00004", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "修理依頼書")
                Else
                    Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("00008", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "修理依頼書")
                End If


            End If

        Catch ex As Exception
            '--------------------------------
            '2014/05/15 後藤　ここから
            '--------------------------------
            'psMesBox(Me, "00004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "修理依頼情報取得")
            psMesBox(Me, "30015", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "修理依頼情報取得")
            '--------------------------------
            '2014/05/15 後藤　ここあｍで
            '--------------------------------
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
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub
    ''' <summary>
    ''' TBOXID変更時処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtTboxId_TextChanged(sender As Object, e As EventArgs)

        '空白の場合は検索しない.
        If Me.txtTboxid.ppTextBox.Text.Trim <> String.Empty Then
            '--------------------------------
            '2014/05/20 後藤　ここから
            '--------------------------------
            'ホール名取得.
            'Call mfGet_HallData_Sel(Me.txtTboxid.ppTextBox.Text)

            If mfGet_HallData_Sel(Me.txtTboxid.ppText) = False Then
                txtTboxid.psSet_ErrorNo("2002", "入力されたTBOXID")
                Me.txtTboxid.ppTextBox.Focus()
                Me.txtHallNm.ppText = String.Empty
            End If
            '--------------------------------
            '2014/05/20 後藤　ここまで
            '--------------------------------
        End If

    End Sub
    ''' <summary>
    ''' 完了品の送付先リスト変更時処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlTraderCd_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '空白の場合は検索しない.
        If Me.ddlTraderCd.SelectedIndex = "0" Then
            Exit Sub
        End If

        Try

            '開始ログ出力.
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            Else

                cmdDB = New SqlCommand(sCnsSqlid_032, conDB)
                cmdDB.Parameters.Add(pfSet_Param("code", SqlDbType.NVarChar, Me.ddlTraderCd.SelectedValue))

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得したデータをコントロールに設定
                Me.txtZipNo.Text = dstOrders.Tables(0).Rows(0).Item("郵便番号").ToString()
                Me.txtAddr.Text = dstOrders.Tables(0).Rows(0).Item("住所").ToString()
                Me.txtTel.Text = dstOrders.Tables(0).Rows(0).Item("TEL").ToString()
            End If

        Catch ex As Exception
            psMesBox(Me, "00004", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後)
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
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub
    ''' <summary>
    ''' 会社名ドロップダウンリスト変更時処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlCompNm_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        '空白時は対象コントロールを非活性.
        If Me.ddlCompNm.SelectedIndex = "0" Then
            Me.ddlWrkNo11.Enabled = False
            Me.ddlWrkNo12.Enabled = False
            Me.ddlWrkNo13.Enabled = False
            Me.ddlWrkNo14.Enabled = False
            Me.ddlWrkNo21.Enabled = False
            Me.ddlPartsNo1.Enabled = False
            Me.ddlPartsNm.Enabled = False
            '-----------------------------
            '2014/05/02 土岐　ここから
            '-----------------------------
            'Me.ddlWrkNo11.SelectedIndex = "0"
            'Me.ddlWrkNo12.SelectedIndex = "0"
            'Me.ddlWrkNo13.SelectedIndex = "0"
            'Me.ddlWrkNo14.SelectedIndex = "0"
            'Me.ddlWrkNo21.SelectedIndex = "0"
            'Me.ddlPartsNo1.SelectedIndex = "0"
            'Me.ddlPartsNm.SelectedIndex = "0"
            '-----------------------------
            '2014/05/02 土岐　ここまで
            '-----------------------------
        Else
            Me.ddlWrkNo11.Enabled = True
            Me.ddlWrkNo12.Enabled = True
            Me.ddlWrkNo13.Enabled = True
            Me.ddlWrkNo14.Enabled = True
            Me.ddlWrkNo21.Enabled = True
            Me.ddlPartsNo1.Enabled = True
            Me.ddlPartsNm.Enabled = True
            Me.txtQuantity.ppEnabled = True
        End If

        'ドロップダウンリスト再生成.
        Call msGet_CompDropListData_Sel(Me.ddlCompNm.SelectedValue)

    End Sub
    ''' <summary>
    ''' 交換部品・クリアボタン押下処理．
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.ddlPartsNm.SelectedIndex = 0
        Me.txtQuantity.ppText = String.Empty

        '活性制御.
        Call msEnableChangeScreen(sender)

    End Sub
    ''' <summary>
    ''' 交換部品・追加ボタン押下処理．
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim objDt As DataTable = Nothing
        Dim strBrach As String = String.Empty '枝番
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim intRtn As Integer = 0
        Dim intMaxValue As Integer = 0
        Dim dstOrders As New DataSet
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '検証チェック.
        Me.ddlCompNm.ValidationGroup = 2
        Me.cuvCompNm.ValidationGroup = 2
        msCheck_AddError()
        If (Page.IsValid) Then

            Try

                '開始ログ出力.
                psLogStart(Me)

                'グリッドの情報取得.
                objDt = pfParse_DataTable(Me.grvList)

                '0件の場合,連番=1.
                'If objDt.Rows.Count <> 0 Then
                '枝番の最大値取得.
                If Not clsDataConnect.pfOpen_Database(conDB) Then
                    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Else
                    cmdDB = New SqlCommand(M_DISP_ID & "_S9", conDB)
                    cmdDB.Parameters.Add(pfSet_Param("repair_no", SqlDbType.NVarChar, Me.lblRepair_No.Text))
                    cmdDB.Parameters.Add(pfSet_Param("branch", SqlDbType.NVarChar, Me.txtBranch.ppText))
                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                End If

                intMaxValue = (Integer.Parse(dstOrders.Tables(0).Rows(0).Item("連番").ToString()) + 1)
                'Else
                '    intMaxValue = 1
                'End If

                '接続
                If Not clsDataConnect.pfOpen_Database(conDB) Then
                    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Else
                    'トランザクション.
                    Using conTrn = conDB.BeginTransaction
                        cmdDB = New SqlCommand(M_DISP_ID & "_I1", conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = conTrn
                        With cmdDB.Parameters
                            .Add(pfSet_Param("repair_no", SqlDbType.NVarChar, Me.lblRepair_No.Text))
                            .Add(pfSet_Param("branch", SqlDbType.NVarChar, Me.txtBranch.ppText))
                            .Add(pfSet_Param("seq_no", SqlDbType.NVarChar, intMaxValue))
                            .Add(pfSet_Param("maker_cd", SqlDbType.NVarChar, Me.ddlCompNm.SelectedValue))
                            .Add(pfSet_Param("parts_cd", SqlDbType.NVarChar, Me.ddlPartsNm.SelectedValue))
                            .Add(pfSet_Param("quantity", SqlDbType.NVarChar, Me.txtQuantity.ppText))
                            .Add(pfSet_Param("insert_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With

                        '実行
                        cmdDB.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "交換部品")

                            'ロールバック
                            conTrn.Rollback()

                            Exit Sub
                        End If

                        'コミット
                        conTrn.Commit()

                    End Using


                    '完了メッセージ
                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "交換部品")

                    '連番保持.
                    ViewState(sCnsSeqNo) = intMaxValue

                    '交換部品リスト再取得.
                    Call msSetList_Data(Me.lblRepair_No.Text, Me.txtBranch.ppText)

                    '活性制御.
                    Call msEnableChangeScreen(sender)
                End If

            Catch ex As Exception
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "交換部品")
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
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

                '検証チェック.
                Me.ddlCompNm.ValidationGroup = 1
                Me.cuvCompNm.ValidationGroup = 1

                '終了ログ出力
                psLogEnd(Me)

            End Try
        End If

    End Sub
    ''' <summary>
    ''' 交換部品・変更ボタン押下処理．
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnChange_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim objDt As DataTable = Nothing
        Dim strBrach As String = String.Empty '枝番
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim intRtn As Integer = 0
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '検証チェック.
        If (Page.IsValid) Then

            Try

                '開始ログ出力.
                psLogStart(Me)

                '接続
                If Not clsDataConnect.pfOpen_Database(conDB) Then
                    psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                Else
                    'トランザクション.
                    Using conTrn = conDB.BeginTransaction
                        cmdDB = New SqlCommand(M_DISP_ID & "_U1", conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = conTrn
                        With cmdDB.Parameters
                            .Add(pfSet_Param("repair_no", SqlDbType.NVarChar, Me.lblRepair_No.Text))
                            .Add(pfSet_Param("branch", SqlDbType.NVarChar, Me.txtBranch.ppText))
                            .Add(pfSet_Param("seq_no", SqlDbType.NVarChar, ViewState(sCnsSeqNo)))
                            .Add(pfSet_Param("maker_cd", SqlDbType.NVarChar, Me.ddlCompNm.SelectedValue))
                            .Add(pfSet_Param("parts_cd", SqlDbType.NVarChar, Me.ddlPartsNm.SelectedValue))
                            .Add(pfSet_Param("quantity", SqlDbType.NVarChar, Me.txtQuantity.ppText))
                            .Add(pfSet_Param("update_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With

                        '実行
                        cmdDB.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, "00001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "交換部品")

                            'ロールバック
                            conTrn.Rollback()

                            Exit Sub
                        End If

                        'コミット
                        conTrn.Commit()

                    End Using

                    '完了メッセージ
                    psMesBox(Me, "00001", clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後, "交換部品")

                    '交換部品リスト再取得.
                    Call msSetList_Data(Me.lblRepair_No.Text, Me.txtBranch.ppText)

                    '活性制御.
                    Call msEnableChangeScreen(sender)
                End If

            Catch ex As Exception
                psMesBox(Me, "00001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "交換部品")
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
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                End If

                '終了ログ出力
                psLogEnd(Me)

            End Try
        End If

    End Sub
    ''' <summary>
    ''' 交換部品・削除ボタン押下処理．
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim objDt As DataTable = Nothing
        Dim strBrach As String = String.Empty '枝番
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim intRtn As Integer = 0
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '検証チェック.
        If (Page.IsValid) Then

            Try

                '開始ログ出力.
                psLogStart(Me)

                '接続
                If Not clsDataConnect.pfOpen_Database(conDB) Then
                    psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                Else
                    'トランザクション.
                    Using conTrn = conDB.BeginTransaction
                        cmdDB = New SqlCommand(M_DISP_ID & "_D1", conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = conTrn
                        With cmdDB.Parameters
                            .Add(pfSet_Param("repair_no", SqlDbType.NVarChar, Me.lblRepair_No.Text))
                            .Add(pfSet_Param("branch", SqlDbType.NVarChar, Me.txtBranch.ppText))
                            .Add(pfSet_Param("seq_no", SqlDbType.NVarChar, ViewState(sCnsSeqNo)))
                            .Add(pfSet_Param("update_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With

                        '実行
                        cmdDB.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, "00002", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "交換部品")

                            'ロールバック
                            conTrn.Rollback()

                            Exit Sub
                        End If

                        'コミット
                        conTrn.Commit()

                    End Using

                    '完了メッセージ
                    psMesBox(Me, "00002", clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後, "交換部品")

                    '交換部品リスト再取得.
                    Call msSetList_Data(Me.lblRepair_No.Text, Me.txtBranch.ppText)

                    '活性制御.
                    Call msEnableChangeScreen(sender)
                End If

            Catch ex As Exception
                psMesBox(Me, "00002", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "交換部品")
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
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                End If

                '終了ログ出力
                psLogEnd(Me)

            End Try

        End If

    End Sub
    ''' <summary>
    ''' 一覧の選択ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        '選択ボタン以外は処理しない.
        If e.CommandName.Trim <> "btnSelect" Then
            Exit Sub
        End If

        Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    ' ボタン押下行のIndex
        Dim rowData As GridViewRow = grvList.Rows(intIndex)             ' ボタン押下行

        '排他制御用の変数.
        Dim strExclusiveDate As String = String.Empty
        Dim arTable_Name As New ArrayList
        Dim arKey As New ArrayList

        '排他情報削除.
        If Not Me.Master.ppExclusiveDateDtl = String.Empty Then
            clsExc.pfDel_Exclusive(Me _
                          , Session(P_SESSION_SESSTION_ID) _
                          , Me.Master.ppExclusiveDateDtl)
            Me.Master.ppExclusiveDateDtl = String.Empty
        End If

        Select Case Session(P_SESSION_AUTH)
            Case "管理者", "SPC"
                'ロック対象テーブル名の登録.
                arTable_Name.Insert(0, "D80_REPAIR_DTIL")

                'ロックテーブルキー項目の登録(D80_REPAIR_DTIL).
                arKey.Insert(0, Me.lblRepair_No.Text)                               'D80_REPAIR_NO.
                arKey.Insert(1, Me.txtBranch.ppText)                                'D80_BRANCH.
                'arKey.Insert(2, CType(rowData.FindControl("連番"), TextBox).Text)   'D80_SEQNO.

                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                objStack = New StackFrame
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------

                '排他情報確認処理(更新処理の実行).
                If clsExc.pfSel_Exclusive(strExclusiveDate _
                                                  , Me _
                                                  , Session(P_SESSION_IP) _
                                                  , Session(P_SESSION_PLACE) _
                                                  , Session(P_SESSION_USERID) _
                                                  , Session(P_SESSION_SESSTION_ID) _
                                                  , ViewState(P_SESSION_GROUP_NUM) _
                                                  , M_DISP_ID _
                                                  , arTable_Name _
                                                  , arKey) = 0 Then

                    Try

                    Catch ex As Exception

                        'マスタ情報が存在しない.
                        psMesBox(Me, "30001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                        '--------------------------------
                        '2014/04/14 星野　ここから
                        '--------------------------------
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
                        '--------------------------------
                        '2014/04/14 星野　ここまで
                        '--------------------------------
                        '排他情報解除.
                        clsExc.pfDel_Exclusive(Me _
                                      , Session(P_SESSION_SESSTION_ID) _
                                      , Me.Master.ppExclusiveDateDtl)
                        Me.Master.ppExclusiveDateDtl = String.Empty
                        Exit Sub
                    End Try

                    '画面に表示.
                    Me.ddlPartsNm.SelectedValue = CType(rowData.FindControl("交換部品コード"), TextBox).Text
                    Me.txtQuantity.ppText = CType(rowData.FindControl("個数"), TextBox).Text
                    ViewState(sCnsSeqNo) = CType(rowData.FindControl("連番"), TextBox).Text


                    '活性制御.
                    Call msEnableChangeScreen(sender)

                    '登録年月日時刻(明細)に登録.
                    Me.Master.ppExclusiveDateDtl = strExclusiveDate

                Else
                    '排他ロック中
                    Exit Sub
                End If
            Case "NGC"
                '画面に表示.
                Me.ddlPartsNm.SelectedValue = CType(rowData.FindControl("交換部品コード"), TextBox).Text
                Me.txtQuantity.ppText = CType(rowData.FindControl("個数"), TextBox).Text
                ViewState(sCnsSeqNo) = CType(rowData.FindControl("連番"), TextBox).Text


                '活性制御.
                Call msEnableChangeScreen(sender)

                '登録年月日時刻(明細)に登録.
                Me.Master.ppExclusiveDateDtl = strExclusiveDate

        End Select

    End Sub
    ''' <summary>
    ''' 印刷ボタン押下処理．
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim dstTrader As New DataSet
        Dim dtPrint As New DataTable                     '帳票用データセット
        Dim dtRow As DataRow = dtPrint.NewRow()          'データテーブルの行定義
        Dim strFNm As String = "修理依頼書_" & Me.lblRepair_No.Text & "_" & Date.Today.ToString("yyyyMMddHHmmss")  '帳票名称
        Dim rpt As New REPREP002
        Dim strParts As String = String.Empty
        Dim intCnt As Integer = 0
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


            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            Else

                '修理依頼テーブル情報取得.
                cmdDB = New SqlCommand(M_DISP_ID & "_S2", conDB)
                cmdDB.Parameters.Add(pfSet_Param("repair_no", SqlDbType.NVarChar, Me.lblRepair_No.Text))
                cmdDB.Parameters.Add(pfSet_Param("branch", SqlDbType.NVarChar, Me.txtBranch.ppText))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '業者マスタ情報取得.
                cmdDB = New SqlCommand(M_DISP_ID & "_S3", conDB)
                dstTrader = clsDataConnect.pfGet_DataSet(cmdDB)

            End If

            'データテーブルの項目名セット       
            dtPrint.Columns.Add("システム種別")
            dtPrint.Columns.Add("管理番号")
            dtPrint.Columns.Add("故障発生日")
            dtPrint.Columns.Add("TBOXID")
            dtPrint.Columns.Add("部品名")
            dtPrint.Columns.Add("TEL")
            dtPrint.Columns.Add("FAX")
            dtPrint.Columns.Add("ホール名")
            dtPrint.Columns.Add("旧Ver")
            dtPrint.Columns.Add("新Ver")
            dtPrint.Columns.Add("故障機シリアル")
            dtPrint.Columns.Add("障害内容")
            dtPrint.Columns.Add("送付先郵便番号")
            dtPrint.Columns.Add("送付先住所")
            dtPrint.Columns.Add("送付先担当者")
            dtPrint.Columns.Add("連絡先電話番号")
            dtPrint.Columns.Add("自社名")
            dtPrint.Columns.Add("会社名")
            dtPrint.Columns.Add("故障原因及び修理内容等")
            dtPrint.Columns.Add("使用部品等品名数量")
            dtPrint.Columns.Add("受領日")
            dtPrint.Columns.Add("発送日")
            dtPrint.Columns.Add("責任者")
            'REPUPDP002-001
            dtPrint.Columns.Add("連絡先会社名")
            dtPrint.Columns.Add("連絡先営業所名")
            dtPrint.Columns.Add("連絡先担当者")
            'REPUPDP002-001 END


            For i As Integer = 0 To 15
                intCnt = i + 1
                dtPrint.Columns.Add("故障機シリアル" & intCnt)

                If dstOrders.Tables(0).Rows(0).Item("故障機シリアル").ToString().Length >= intCnt Then
                    dtRow("故障機シリアル" & intCnt) = dstOrders.Tables(0).Rows(0).Item("故障機シリアル").ToString().Substring(i, 1)
                Else
                    dtRow("故障機シリアル" & intCnt) = String.Empty
                End If

            Next

            'データテーブルの列に値を設定
            dtRow("システム種別") = dstOrders.Tables(0).Rows(0).Item("システム種別").ToString()
            dtRow("管理番号") = dstOrders.Tables(0).Rows(0).Item("管理番号").ToString()
            dtRow("故障発生日") = dstOrders.Tables(0).Rows(0).Item("故障発生日").ToString()
            dtRow("TBOXID") = dstOrders.Tables(0).Rows(0).Item("TBOXID").ToString()
            dtRow("部品名") = dstOrders.Tables(0).Rows(0).Item("部品名").ToString()
            dtRow("TEL") = dstTrader.Tables(0).Rows(0).Item("電話番号").ToString()
            dtRow("FAX") = dstTrader.Tables(0).Rows(0).Item("FAX").ToString()
            dtRow("ホール名") = dstOrders.Tables(0).Rows(0).Item("ホール名").ToString()
            dtRow("旧Ver") = dstOrders.Tables(0).Rows(0).Item("旧Ver").ToString()
            dtRow("新Ver") = dstOrders.Tables(0).Rows(0).Item("新Ver").ToString()
            dtRow("障害内容") = dstOrders.Tables(0).Rows(0).Item("障害内容").ToString()
            dtRow("送付先郵便番号") = dstOrders.Tables(0).Rows(0).Item("郵便番号").ToString()
            dtRow("送付先住所") = dstOrders.Tables(0).Rows(0).Item("住所").ToString()
            dtRow("送付先担当者") = dstOrders.Tables(0).Rows(0).Item("担当者").ToString()
            dtRow("連絡先電話番号") = dstOrders.Tables(0).Rows(0).Item("電話番号").ToString()
            dtRow("自社名") = dstOrders.Tables(0).Rows(0).Item("自社名").ToString()
            dtRow("会社名") = dstOrders.Tables(0).Rows(0).Item("会社名").ToString()
            dtRow("故障原因及び修理内容等") = dstOrders.Tables(0).Rows(0).Item("故障原因及び修理内容").ToString
            'REPUPDP002-001
            dtRow("連絡先会社名") = dstTrader.Tables(0).Rows(0).Item("会社名").ToString
            dtRow("連絡先営業所名") = dstTrader.Tables(0).Rows(0).Item("営業所名").ToString
            dtRow("連絡先担当者") = dstTrader.Tables(0).Rows(0).Item("担当者").ToString
            'REPUPDP002-001 END
            '--------------------------------
            '2014/05/21 後藤　ここから
            '--------------------------------
            'strParts = dstOrders.Tables(0).Rows(0).Item("作業項番11").ToString() _
            '                       & "," & dstOrders.Tables(0).Rows(0).Item("作業項番12").ToString() _
            '                       & "," & dstOrders.Tables(0).Rows(0).Item("作業項番13").ToString() _
            '                       & "," & dstOrders.Tables(0).Rows(0).Item("作業項番14").ToString() _
            '                       & "," & dstOrders.Tables(0).Rows(0).Item("作業項番21").ToString() _
            '                       & "," & dstOrders.Tables(0).Rows(0).Item("部品項番1").ToString()

            Dim strParts2 As String() = {dstOrders.Tables(0).Rows(0).Item("作業項番11").ToString(),
                                         dstOrders.Tables(0).Rows(0).Item("作業項番12").ToString(),
                                         dstOrders.Tables(0).Rows(0).Item("作業項番13").ToString(),
                                         dstOrders.Tables(0).Rows(0).Item("作業項番14").ToString(),
                                         dstOrders.Tables(0).Rows(0).Item("作業項番21").ToString(),
                                         dstOrders.Tables(0).Rows(0).Item("部品項番1").ToString()}

            For i As Integer = 0 To strParts2.Length - 1

                If strParts2(i).ToString <> String.Empty Then

                    If i = strParts2.Length - 1 Then
                        strParts += strParts2(i).ToString
                    Else
                        strParts += strParts2(i).ToString & ","
                    End If

                End If
            Next

            If strParts.Trim.Replace(",", "") = String.Empty Then
                dtRow("使用部品等品名数量") = String.Empty
            Else
                'dtRow("使用部品等品名数量") = strParts.Replace(",,", " ,")
                If Microsoft.VisualBasic.Strings.Right(strParts, 1) = "," Then
                    dtRow("使用部品等品名数量") = Microsoft.VisualBasic.Strings.Left(strParts, Microsoft.VisualBasic.Strings.Len(strParts) - 1)
                Else
                    dtRow("使用部品等品名数量") = strParts
                End If
            End If
            '--------------------------------
            '2014/05/21 後藤　ここまで
            '--------------------------------

            dtRow("受領日") = dstOrders.Tables(0).Rows(0).Item("受領日").ToString()
            dtRow("発送日") = dstOrders.Tables(0).Rows(0).Item("発送日").ToString()
            dtRow("責任者") = dstOrders.Tables(0).Rows(0).Item("修理責任者").ToString()

            'データテーブルに作成したデータを設定
            dtPrint.Rows.Add(dtRow)

            'Active Reports帳票の起動
            Try
                psPrintPDF(Me, rpt, dtPrint, strFNm)

            Catch ex As Exception
                '帳票の出力に失敗
                psMesBox(Me, "10001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
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

        Catch ex As SqlException
            '帳票情報の取得に失敗
            psMesBox(Me, "00004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "帳票情報")
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
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            End If
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub
    ''' <summary>
    ''' クリアボタン押下処理．
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAllClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        '--------------------------------
        '2014/05/19 後藤　ここから
        '--------------------------------
        'If Me.btnAllClear.Text = "クリア" Then
        If Master.ppRigthButton2.Text = "クリア" Then
            '--------------------------------
            '2014/05/19 後藤　ここまで
            '--------------------------------

            'クリア処理.
            Call msInitScreen()
        Else
            '再検索.
            Call msSearch_Data(Me.lblRepair_No.Text, Me.txtBranch.ppText)
        End If


    End Sub
    ''' <summary>
    ''' 登録／更新ボタン押下処理．
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim objDt As DataTable = Nothing
        Dim strBrach As String = String.Empty '枝番
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim strRepairEndFlg As String = String.Empty
        Dim strMessage As String = String.Empty
        Dim strErrorMessage As String = String.Empty
        Dim intRtn As Integer = 0
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '検証グループ設定.
        '--------------------------------
        '2014/05/19 後藤　ここから
        '--------------------------------
        'If Me.btnUpdate.Text = "更新" Then
        If Master.ppRigthButton1.Text = "更新" Then
            '--------------------------------
            '2014/05/19 後藤　ここまで
            '--------------------------------
            Me.txtRepairContent.ValidationGroup = 1
            cuvRepairContent.ValidationGroup = 1
            Me.txtRepairCharge.ppValidationGroup = 1
            Me.dtbAppasendDt.ppValidationGroup = 1
            Me.txtVer1.ppValidationGroup = 1
            Me.txtHddNo1.ppValidationGroup = 1
            Me.txtHddCls1.ppValidationGroup = 1
            Me.dtbAppaarvDt.ppValidationGroup = 1
            Me.ddlWrkNo11.ValidationGroup = 1
            Me.ddlWrkNo12.ValidationGroup = 1
            Me.ddlWrkNo13.ValidationGroup = 1
            Me.ddlWrkNo14.ValidationGroup = 1
            Me.ddlWrkNo21.ValidationGroup = 1
            Me.ddlPartsNo1.ValidationGroup = 1
            Me.ddlTmpResult.ppValidationGroup = 1
            Me.dtbRsltSndDt.ppValidationGroup = 1
            Me.ddlCmprtnCd.ValidationGroup = 1
            Me.dtbCmpSndDt.ppValidationGroup = 1
            Me.txtVer2.ppValidationGroup = 1
            Me.txtHddNo2.ppValidationGroup = 1
            Me.txtHddCls2.ppValidationGroup = 1
            Me.dtbArrivalDt.ppValidationGroup = 1
            Me.txtNewSerial.ppValidationGroup = 1
            Me.ddlInsResult.ppValidationGroup = 1
            Me.txtSubNo.ppValidationGroup = 1
            Me.ddlTokCls.ppValidationGroup = 1
            Me.ddlStatusCd.ValidationGroup = 1
            Me.txtReqDt.ppValidationGroup = 1

            '通知完了登録入力チェック.
            Call msCheck_CompleteError()

        End If

        '修理依頼書入力チェック.
        Call msCheck_RepairError()

        '検証チェック.
        If (Page.IsValid) Then

            Try

                '開始ログ出力.
                psLogStart(Me)

                '接続
                If Not clsDataConnect.pfOpen_Database(conDB) Then
                    psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                Else
                    'モード設定.
                    '--------------------------------
                    '2014/05/19 後藤　ここから
                    '--------------------------------
                    'If btnUpdate.Text = "登録" Then
                    '    cmdDB = New SqlCommand(M_DISP_ID & "_I2", conDB)
                    '    strErrorMessage = "00003"
                    '    strMessage = "00009"
                    'ElseIf btnUpdate.Text = "更新" Then
                    '    cmdDB = New SqlCommand(M_DISP_ID & "_U2", conDB)
                    '    strErrorMessage = "00001"
                    '    strMessage = "00001"
                    'End If

                    If Master.ppRigthButton1.Text = "登録" Then
                        cmdDB = New SqlCommand(M_DISP_ID & "_I2", conDB)
                        strErrorMessage = "00003"
                        strMessage = "00009"
                    ElseIf Master.ppRigthButton1.Text = "更新" Then
                        cmdDB = New SqlCommand(M_DISP_ID & "_U2", conDB)
                        strErrorMessage = "00001"
                        strMessage = "00001"
                    End If
                    '--------------------------------
                    '2014/05/19 後藤　ここまで
                    '--------------------------------

                    '案件終了フラグ.
                    If Me.ddlStatusCd.SelectedValue = "04" Then
                        strRepairEndFlg = "1"
                    Else
                        strRepairEndFlg = "0"
                    End If


                    'トランザクション.
                    Using conTrn = conDB.BeginTransaction
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = conTrn
                        With cmdDB.Parameters
                            .Add(pfSet_Param("repair_no", SqlDbType.NVarChar, Me.lblRepair_No.Text))
                            .Add(pfSet_Param("branch", SqlDbType.NVarChar, Me.txtBranch.ppText))
                            .Add(pfSet_Param("tboxclass_cd", SqlDbType.NVarChar, ViewState(sCnsSys)))
                            .Add(pfSet_Param("trouble_d", SqlDbType.NVarChar, mfGetDBNull(Me.dtbTroubleDt.ppText.Trim)))
                            .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxid.ppText))
                            .Add(pfSet_Param("appaclass_cd", SqlDbType.NVarChar, Me.ddlProductNm.SelectedValue))
                            .Add(pfSet_Param("hall_cd", SqlDbType.NVarChar, ViewState(sCnsHall)))
                            .Add(pfSet_Param("hall_nm", SqlDbType.NVarChar, Me.txtHallNm.ppText))
                            .Add(pfSet_Param("old_version", SqlDbType.NVarChar, Me.txtOldVer.ppText))
                            .Add(pfSet_Param("new_version", SqlDbType.NVarChar, Me.txtNewVer.ppText))
                            .Add(pfSet_Param("serial", SqlDbType.NVarChar, Me.txtSerial.ppText))
                            .Add(pfSet_Param("content", SqlDbType.NVarChar, txtTrouble.Text))
                            .Add(pfSet_Param("send_cd", SqlDbType.NVarChar, Me.ddlTraderCd.SelectedValue))
                            .Add(pfSet_Param("zipno", SqlDbType.NVarChar, Me.txtZipNo.Text))
                            .Add(pfSet_Param("addr", SqlDbType.NVarChar, Me.txtAddr.Text))
                            .Add(pfSet_Param("telno", SqlDbType.NVarChar, Me.txtTel.Text))
                            .Add(pfSet_Param("charge", SqlDbType.NVarChar, txtCharge.Text))
                            .Add(pfSet_Param("trouble_cd", SqlDbType.NVarChar, Me.ddlTrouble.SelectedValue))
                            .Add(pfSet_Param("repair_content", SqlDbType.NVarChar, Me.txtRepairContent.Text))
                            .Add(pfSet_Param("arrival_d", SqlDbType.NVarChar, mfGetDBNull(Me.dtbArrivalDt.ppText.Trim)))
                            .Add(pfSet_Param("repair_charge", SqlDbType.NVarChar, Me.txtRepairCharge.ppText.Trim))
                            .Add(pfSet_Param("repair_end_flg", SqlDbType.NVarChar, strRepairEndFlg))
                            .Add(pfSet_Param("delete_flg", SqlDbType.NVarChar, "0"))
                            .Add(pfSet_Param("appasend_dt", SqlDbType.NVarChar, mfGetDBNull(Me.dtbAppasendDt.ppText.Trim)))
                            .Add(pfSet_Param("appaarv_dt", SqlDbType.NVarChar, mfGetDBNull(Me.dtbAppaarvDt.ppText.Trim)))
                            .Add(pfSet_Param("version", SqlDbType.NVarChar, Me.txtVer1.ppText))
                            .Add(pfSet_Param("hdd_no", SqlDbType.NVarChar, Me.txtHddNo1.ppText))
                            .Add(pfSet_Param("hdd_cls", SqlDbType.NVarChar, Me.txtHddCls1.ppText))
                            .Add(pfSet_Param("wrk_no11", SqlDbType.NVarChar, Me.ddlWrkNo11.SelectedValue))
                            .Add(pfSet_Param("wrk_no12", SqlDbType.NVarChar, Me.ddlWrkNo12.SelectedValue))
                            .Add(pfSet_Param("wrk_no13", SqlDbType.NVarChar, Me.ddlWrkNo13.SelectedValue))
                            .Add(pfSet_Param("wrk_no14", SqlDbType.NVarChar, Me.ddlWrkNo14.SelectedValue))
                            .Add(pfSet_Param("tmp_result", SqlDbType.NVarChar, Me.ddlTmpResult.ppSelectedValue))
                            .Add(pfSet_Param("rsltsnd_dt", SqlDbType.NVarChar, mfGetDBNull(Me.dtbRsltSndDt.ppText.Trim)))
                            .Add(pfSet_Param("cmprtn_cd", SqlDbType.NVarChar, Me.ddlCmprtnCd.SelectedValue))
                            .Add(pfSet_Param("cmpsnd_dt", SqlDbType.NVarChar, mfGetDBNull(Me.dtbCmpSndDt.ppText.Trim)))
                            .Add(pfSet_Param("cmp_version", SqlDbType.NVarChar, Me.txtVer2.ppText))
                            .Add(pfSet_Param("cmp_hddno", SqlDbType.NVarChar, Me.txtHddNo2.ppText))
                            .Add(pfSet_Param("cmp_hddcls", SqlDbType.NVarChar, Me.txtHddCls2.ppText))
                            .Add(pfSet_Param("new_serial", SqlDbType.NVarChar, Me.txtNewSerial.ppText))
                            .Add(pfSet_Param("ins_result", SqlDbType.NVarChar, Me.ddlInsResult.ppSelectedValue))
                            .Add(pfSet_Param("sub_no", SqlDbType.NVarChar, Me.txtSubNo.ppText))
                            If ddlStatusCd.SelectedIndex = 0 Then
                                ddlStatusCd.SelectedIndex = 1
                            End If
                            .Add(pfSet_Param("status_cd", SqlDbType.NVarChar, Me.ddlStatusCd.SelectedValue))
                            .Add(pfSet_Param("req_dt", SqlDbType.NVarChar, mfGetDBNull(ViewState(sCnsReqDt))))
                            .Add(pfSet_Param("mkr_cd", SqlDbType.NVarChar, Me.ddlCompNm.SelectedValue))
                            .Add(pfSet_Param("wrk_no21", SqlDbType.NVarChar, Me.ddlWrkNo21.SelectedValue))
                            .Add(pfSet_Param("parts_no1", SqlDbType.NVarChar, Me.ddlPartsNo1.SelectedValue))
                            .Add(pfSet_Param("tok_cls", SqlDbType.NVarChar, Me.ddlTokCls.ppSelectedValue))
                            .Add(pfSet_Param("user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                            If Me.ddlStatus.Text.Trim <> "" Then
                                .Add(pfSet_Param("sys_dvs", SqlDbType.NVarChar, Me.ddlStatus.Text.Split(":"c)(0)))
                            Else
                                .Add(pfSet_Param("sys_dvs", SqlDbType.NVarChar, ""))
                            End If
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With

                        '実行
                        cmdDB.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, strErrorMessage, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "修理依頼書")

                            'ロールバック
                            conTrn.Rollback()

                            Exit Sub
                        End If

                        'コミット
                        conTrn.Commit()

                    End Using

                    '--------------------------------
                    '2014/05/20 後藤　ここから
                    '--------------------------------
                    '画面状態更新
                    'If Master.ppRigthButton1.Text = "登録" Then
                    msEnableScreen(ClsComVer.E_遷移条件.更新)
                    'End If

                    msSet_ButtonAction()
                    '--------------------------------
                    '2014/05/20 後藤　ここまで
                    '--------------------------------

                    '完了メッセージ
                    psMesBox(Me, strMessage, clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後, "修理依頼書")

                End If

            Catch ex As Exception
                psMesBox(Me, strErrorMessage, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "修理依頼書")
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
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                End If

                '終了ログ出力
                psLogEnd(Me)

            End Try

        End If

    End Sub
    ''' <summary>
    ''' 製品名の検証.
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvProductNm_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvProductNm.ServerValidate
        Dim dtrMes As DataRow
        args.IsValid = True
        If ddlProductNm.SelectedIndex = 0 Then '未選択
            dtrMes = pfGet_ValMes("5001", "製品名")
            cuvProductNm.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            cuvProductNm.Text = dtrMes.Item(P_VALMES_SMES)
            args.IsValid = False
        End If
    End Sub
    ''' <summary>
    ''' 障害内容の検証.
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvTrouble_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvTrouble.ServerValidate
        Dim dtrMes As DataRow
        args.IsValid = True
        If ddlTrouble.SelectedIndex = 0 Then '未選択
            dtrMes = pfGet_ValMes("5001", "障害内容")
            cuvTrouble.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            cuvTrouble.Text = dtrMes.Item(P_VALMES_SMES)
            args.IsValid = False
        End If
    End Sub
    ''' <summary>
    ''' 障害内容の検証.
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvTrouble_t_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvTrouble_t.ServerValidate
        Dim dtrMes As DataRow
        args.IsValid = True
        If Not Me.txtTrouble.Text.Length <= 100 Then
            dtrMes = pfGet_ValMes("3002", "障害内容", "100")
            cuvTrouble_t.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            cuvTrouble_t.Text = dtrMes.Item(P_VALMES_SMES)
            args.IsValid = False
        End If
    End Sub
    ''' <summary>
    ''' 完了品の送付先の検証.
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvTraderCd_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvTraderCd.ServerValidate
        Dim dtrMes As DataRow
        args.IsValid = True
        If ddlTraderCd.SelectedIndex = 0 Then '未選択
            dtrMes = pfGet_ValMes("5001", "完了品の送付先")
            cuvTraderCd.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            cuvTraderCd.Text = dtrMes.Item(P_VALMES_SMES)
            args.IsValid = False
        End If
    End Sub
    ''' <summary>
    ''' 故障原因及び修理内容の検証.
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvRepairContent_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvRepairContent.ServerValidate
        Dim dtrMes As DataRow
        args.IsValid = True
        If Not Me.txtRepairContent.Text.Length <= 200 Then
            dtrMes = pfGet_ValMes("3002", "故障原因及び修理内容", "200")
            cuvRepairContent.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            cuvRepairContent.Text = dtrMes.Item(P_VALMES_SMES)
            args.IsValid = False
        End If
    End Sub
    ''' <summary>
    ''' 部品名の検証.
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvPartsNm_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvPartsNm.ServerValidate
        Dim dtrMes As DataRow
        args.IsValid = True
        If ddlPartsNm.SelectedIndex = 0 Then '未選択
            dtrMes = pfGet_ValMes("5001", "部品名")
            cuvPartsNm.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            cuvPartsNm.Text = dtrMes.Item(P_VALMES_SMES)
            args.IsValid = False
        End If
    End Sub
    ''' <summary>
    ''' 会社名の検証.
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvCompNm_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvCompNm.ServerValidate
        Dim dtrMes As DataRow
        args.IsValid = True
        If pnlComplete.Enabled = False Then
            Exit Sub
        End If
        If ddlCompNm.SelectedIndex = 0 Then '未選択
            dtrMes = pfGet_ValMes("5001", "会社名")
            cuvCompNm.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            cuvCompNm.Text = dtrMes.Item(P_VALMES_SMES)
            args.IsValid = False
        End If

        '    Dim conDB As SqlConnection = Nothing
        '    Dim cmdDB As SqlCommand
        '    Dim dstOrders As New DataSet

        '    '接続.
        '    If Not clsDataConnect.pfOpen_Database(conDB) Then
        '        psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        '    End If
        '    Try
        '        '修理依頼データ取得.
        '        cmdDB = New SqlCommand(M_DISP_ID & "_S11", conDB)
        '        cmdDB.Parameters.Add(pfSet_Param("repair_no", SqlDbType.NVarChar, Me.lblRepair_No.Text))
        '        dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
        '        If dstOrders.Tables(0).Rows(0).Item("会社コード").ToString = "" Then
        '            dtrMes = pfGet_ValMes("5001", "会社名")
        '            cuvCompNm.ErrorMessage = dtrMes.Item(P_VALMES_MES)
        '            cuvCompNm.Text = dtrMes.Item(P_VALMES_SMES)
        '            args.IsValid = False
        '        Else
        '        End If

        '    Catch ex As Exception
        '        psMesBox(Me, "30001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        '    Finally
        '        If Not clsDataConnect.pfClose_Database(conDB) Then
        '            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        '        End If
        '    End Try


    End Sub



#End Region

#Region "そのほかのプロシージャ"
    ''' <summary>
    ''' 画面初期化処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msInitScreen(Optional ByVal ipstrClearflg As Boolean = True)

        If ipstrClearflg = True Then

            '修理依頼書.
            Me.txtBranch.ppText = String.Empty
            Me.txtTboxid.ppText = String.Empty
            Me.txtHallNm.ppText = String.Empty
            Me.txtOldVer.ppText = String.Empty
            Me.ddlTrouble.SelectedIndex = 0


        End If

        Me.dtbTroubleDt.ppText = String.Empty
        Me.lblSystem.Text = String.Empty
        Me.ddlProductNm.SelectedIndex = 0
        Me.txtNewVer.ppText = String.Empty
        Me.txtSerial.ppText = String.Empty
        Me.txtTrouble.Text = String.Empty
        Me.ddlTraderCd.SelectedIndex = 0
        Me.txtZipNo.Text = String.Empty
        Me.txtAddr.Text = String.Empty
        Me.txtTel.Text = String.Empty
        Me.txtCharge.Text = String.Empty

        '通知完了登録.
        Me.ddlCompNm.SelectedIndex = 0
        Me.txtRepairContent.Text = String.Empty
        Me.txtRepairCharge.ppText = String.Empty
        Me.dtbAppasendDt.ppText = String.Empty
        Me.txtVer1.ppText = String.Empty
        Me.txtHddNo1.ppText = String.Empty
        Me.txtHddCls1.ppText = String.Empty
        Me.dtbAppaarvDt.ppText = String.Empty
        Me.ddlWrkNo11.SelectedIndex = 0
        Me.ddlWrkNo12.SelectedIndex = 0
        Me.ddlWrkNo13.SelectedIndex = 0
        Me.ddlWrkNo14.SelectedIndex = 0
        Me.ddlWrkNo21.SelectedIndex = 0
        Me.ddlPartsNo1.SelectedIndex = 0
        Me.ddlTmpResult.ppDropDownList.SelectedIndex = 0
        Me.dtbRsltSndDt.ppText = String.Empty
        Me.ddlCmprtnCd.SelectedIndex = 0
        Me.dtbCmpSndDt.ppText = String.Empty
        Me.txtVer2.ppText = String.Empty
        Me.txtHddNo2.ppText = String.Empty
        Me.txtHddCls2.ppText = String.Empty
        Me.dtbArrivalDt.ppText = String.Empty
        Me.txtNewSerial.ppText = String.Empty
        Me.ddlInsResult.ppDropDownList.SelectedIndex = 0
        Me.txtSubNo.ppText = String.Empty
        Me.ddlTokCls.ppDropDownList.SelectedIndex = 0
        Me.ddlStatusCd.SelectedIndex = 0
        Me.txtReqDt.ppText = String.Empty
        Me.ddlStatus.SelectedIndex = 0

        '交換部品.
        Me.ddlPartsNm.SelectedIndex = 0
        Me.txtQuantity.ppText = String.Empty
        Me.grvList.DataSource = New Object() {}
        Me.grvList.DataBind()

        ''排他情報用コントロールの初期化
        'Me.Master.ppExclusiveDate = String.Empty
        'Me.Master.ppExclusiveDateDtl = String.Empty


    End Sub
    ''' <summary>
    ''' 修理依頼データ取得処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data(ByVal ipstrRepairNo As String, ByVal ipstrBranch As String, ByRef ipstrTerms As String)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet
        Dim cmdDBList As New SqlCommand
        Dim dstOrdersList As New DataSet
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            '開始ログ出力.
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else

                '修理依頼データ取得.
                cmdDB = New SqlCommand(M_DISP_ID & "_S1", conDB)
                cmdDB.Parameters.Add(pfSet_Param("repair_no", SqlDbType.NVarChar, ipstrRepairNo))
                cmdDB.Parameters.Add(pfSet_Param("branch", SqlDbType.NVarChar, ipstrBranch))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得したデータをコントロールに設定.
                Call msSet_Data(dstOrders)

                '遷移条件取得.
                ipstrTerms = dstOrders.Tables(0).Rows(0).Item("条件").ToString()

            End If

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "修理依頼データ")
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            '--------------------------------
            '2014/06/11 後藤　ここから
            '--------------------------------
            '排他削除
            clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
            '--------------------------------
            '2014/06/11 後藤　ここまで
            '--------------------------------
            psClose_Window(Me)
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub
    ''' <summary>
    ''' 修理依頼データ設定処理.
    ''' </summary>
    ''' <param name="dstOrders"></param>
    ''' <remarks></remarks>
    Private Sub msSet_Data(ByVal dstOrders As DataSet)

        'ドロップダウンリスト生成.
        If Not mfGet_DropListData_Sel(dstOrders.Tables(0).Rows(0).Item("メーカーコード").ToString(), dstOrders.Tables(0).Rows(0).Item("システムコード").ToString()) Then
            '画面を終了
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            '--------------------------------
            '2014/06/11 後藤　ここから
            '--------------------------------
            '排他削除
            clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
            '--------------------------------
            '2014/06/11 後藤　ここまで
            '--------------------------------
            psClose_Window(Me)
        End If

        '取得したデータをコントロールに設定.
        Call msSetDisp_Data(dstOrders)

    End Sub

    ''' <summary>
    ''' 修理依頼データ取得処理(再検索).
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSearch_Data(ByVal ipstrRepairNo As String, ByVal ipstrBranch As String)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet
        Dim cmdDBList As New SqlCommand
        Dim dstOrdersList As New DataSet
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            '開始ログ出力.
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else

                Dim a As Integer = 0

                '修理依頼データ取得.
                cmdDB = New SqlCommand(M_DISP_ID & "_S8", conDB)
                cmdDB.Parameters.Add(pfSet_Param("repair_no", SqlDbType.NVarChar, ipstrRepairNo))
                cmdDB.Parameters.Add(pfSet_Param("branch", SqlDbType.NVarChar, ipstrBranch))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得したデータをコントロールに設定.
                Call msSetDisp_Data(dstOrders)

            End If

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "修理依頼データ")
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
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub
    ''' <summary>
    ''' 修理依頼データ表示処理.
    ''' </summary>
    ''' <param name="dstOrders"></param>
    ''' <remarks></remarks>
    Private Sub msSetDisp_Data(ByVal dstOrders As DataSet)
        Dim liData As ListItem

        Me.lblRepair_No.Text = dstOrders.Tables(0).Rows(0).Item("修理管理番号").ToString()
        Me.txtBranch.ppText = dstOrders.Tables(0).Rows(0).Item("枝番").ToString()
        Me.dtbTroubleDt.ppText = dstOrders.Tables(0).Rows(0).Item("故障発生日").ToString()
        Me.txtTboxid.ppText = dstOrders.Tables(0).Rows(0).Item("TBOXID").ToString()
        ViewState(sCnsSys) = dstOrders.Tables(0).Rows(0).Item("システムコード").ToString()
        Me.lblSystem.Text = dstOrders.Tables(0).Rows(0).Item("システム").ToString()
        liData = Nothing
        liData = Me.ddlProductNm.Items.FindByValue(dstOrders.Tables(0).Rows(0).Item("機器種別コード").ToString())
        If liData Is Nothing Then
            Me.ddlProductNm.SelectedIndex = 0
        Else
            Me.ddlProductNm.SelectedValue = dstOrders.Tables(0).Rows(0).Item("機器種別コード").ToString()
        End If
        ViewState(sCnsHall) = dstOrders.Tables(0).Rows(0).Item("ホールコード").ToString()
        Me.txtHallNm.ppText = dstOrders.Tables(0).Rows(0).Item("ホール名").ToString()
        Me.txtOldVer.ppText = dstOrders.Tables(0).Rows(0).Item("VER旧").ToString()
        Me.txtNewVer.ppText = dstOrders.Tables(0).Rows(0).Item("VER新").ToString()
        Me.txtSerial.ppText = dstOrders.Tables(0).Rows(0).Item("故障機シリアル").ToString()
        liData = Nothing
        liData = Me.ddlTrouble.Items.FindByValue(dstOrders.Tables(0).Rows(0).Item("故障内容コード").ToString())
        If liData Is Nothing Then
            Me.ddlTrouble.SelectedIndex = 0
        Else
            Me.ddlTrouble.SelectedValue = dstOrders.Tables(0).Rows(0).Item("故障内容コード").ToString()
        End If
        Me.txtTrouble.Text = dstOrders.Tables(0).Rows(0).Item("障害内容").ToString()
        liData = Nothing
        liData = Me.ddlTraderCd.Items.FindByValue(dstOrders.Tables(0).Rows(0).Item("送付先コード").ToString())
        If liData Is Nothing Then
            'REPUPDP002-001
            Select Case dstOrders.Tables(0).Rows(0).Item("送付先コード").ToString()
                Case OLD_BW_COD
                    Me.ddlTraderCd.SelectedValue = NEW_BW_COD
                Case OLD_FS_COD
                    Me.ddlTraderCd.SelectedValue = NEW_FS_COD
                Case Else
                    Me.ddlTraderCd.SelectedIndex = -1
            End Select
            'REPUPDP002-001
        Else
            Me.ddlTraderCd.SelectedValue = dstOrders.Tables(0).Rows(0).Item("送付先コード").ToString()
        End If
        Me.txtZipNo.Text = dstOrders.Tables(0).Rows(0).Item("郵便番号").ToString()
        Me.txtAddr.Text = dstOrders.Tables(0).Rows(0).Item("住所").ToString()
        Me.txtTel.Text = dstOrders.Tables(0).Rows(0).Item("電話番号").ToString()
        Me.txtCharge.Text = dstOrders.Tables(0).Rows(0).Item("担当者").ToString()
        liData = Nothing
        liData = Me.ddlCompNm.Items.FindByValue(dstOrders.Tables(0).Rows(0).Item("メーカーコード").ToString())
        If liData Is Nothing Then
            Me.ddlCompNm.SelectedIndex = 0
        Else
            Me.ddlCompNm.SelectedValue = dstOrders.Tables(0).Rows(0).Item("メーカーコード").ToString()
        End If
        Me.txtRepairContent.Text = dstOrders.Tables(0).Rows(0).Item("故障原因及び修理内容").ToString()
        Me.txtRepairCharge.ppText = dstOrders.Tables(0).Rows(0).Item("修理責任者").ToString()
        Me.dtbAppasendDt.ppText = dstOrders.Tables(0).Rows(0).Item("機器発送日").ToString()
        Me.txtVer1.ppText = dstOrders.Tables(0).Rows(0).Item("VER").ToString()
        Me.txtHddNo1.ppText = dstOrders.Tables(0).Rows(0).Item("HDDNo").ToString()
        Me.txtHddCls1.ppText = dstOrders.Tables(0).Rows(0).Item("HDD種別").ToString()
        Me.dtbAppaarvDt.ppText = dstOrders.Tables(0).Rows(0).Item("機器到着日").ToString()
        liData = Nothing
        liData = Me.ddlWrkNo11.Items.FindByValue(dstOrders.Tables(0).Rows(0).Item("作業項番11").ToString())
        If liData Is Nothing Then
            Me.ddlWrkNo11.SelectedIndex = 0
        Else
            Me.ddlWrkNo11.SelectedValue = dstOrders.Tables(0).Rows(0).Item("作業項番11").ToString()
        End If
        liData = Nothing
        liData = Me.ddlWrkNo12.Items.FindByValue(dstOrders.Tables(0).Rows(0).Item("作業項番12").ToString())
        If liData Is Nothing Then
            Me.ddlWrkNo12.SelectedIndex = 0
        Else
            Me.ddlWrkNo12.SelectedValue = dstOrders.Tables(0).Rows(0).Item("作業項番12").ToString()
        End If
        liData = Nothing
        liData = Me.ddlWrkNo13.Items.FindByValue(dstOrders.Tables(0).Rows(0).Item("作業項番13").ToString())
        If liData Is Nothing Then
            Me.ddlWrkNo13.SelectedIndex = 0
        Else
            Me.ddlWrkNo13.SelectedValue = dstOrders.Tables(0).Rows(0).Item("作業項番13").ToString()
        End If
        liData = Nothing
        liData = Me.ddlWrkNo14.Items.FindByValue(dstOrders.Tables(0).Rows(0).Item("作業項番14").ToString())
        If liData Is Nothing Then
            Me.ddlWrkNo14.SelectedIndex = 0
        Else
            Me.ddlWrkNo14.SelectedValue = dstOrders.Tables(0).Rows(0).Item("作業項番14").ToString()
        End If
        liData = Nothing
        liData = Me.ddlWrkNo21.Items.FindByValue(dstOrders.Tables(0).Rows(0).Item("作業項番21").ToString())
        If liData Is Nothing Then
            Me.ddlWrkNo21.SelectedIndex = 0
        Else
            Me.ddlWrkNo21.SelectedValue = dstOrders.Tables(0).Rows(0).Item("作業項番21").ToString()
        End If
        liData = Nothing
        liData = Me.ddlPartsNo1.Items.FindByValue(dstOrders.Tables(0).Rows(0).Item("部品項番1").ToString())
        If liData Is Nothing Then
            Me.ddlPartsNo1.SelectedIndex = 0
        Else
            Me.ddlPartsNo1.SelectedValue = dstOrders.Tables(0).Rows(0).Item("部品項番1").ToString()
        End If
        liData = Nothing
        liData = Me.ddlTmpResult.ppDropDownList.Items.FindByValue(dstOrders.Tables(0).Rows(0).Item("一時診断結果").ToString())
        If liData Is Nothing Then
            Me.ddlTmpResult.ppDropDownList.SelectedIndex = 0
        Else
            Me.ddlTmpResult.ppSelectedValue = dstOrders.Tables(0).Rows(0).Item("一時診断結果").ToString()
        End If
        Me.dtbRsltSndDt.ppText = dstOrders.Tables(0).Rows(0).Item("診断結果送付日").ToString()
        liData = Nothing
        liData = Me.ddlCmprtnCd.Items.FindByValue(dstOrders.Tables(0).Rows(0).Item("完了返却先").ToString())
        If liData Is Nothing Then
            Me.ddlCmprtnCd.SelectedIndex = 0
        Else
            Me.ddlCmprtnCd.SelectedValue = dstOrders.Tables(0).Rows(0).Item("完了返却先").ToString()
        End If
        Me.dtbCmpSndDt.ppText = dstOrders.Tables(0).Rows(0).Item("完了発送日").ToString()
        Me.txtVer2.ppText = dstOrders.Tables(0).Rows(0).Item("完了後VER").ToString()
        Me.txtHddNo2.ppText = dstOrders.Tables(0).Rows(0).Item("完了後HDDNo").ToString()
        Me.txtHddCls2.ppText = dstOrders.Tables(0).Rows(0).Item("完了後HDD種別").ToString()
        Me.dtbArrivalDt.ppText = dstOrders.Tables(0).Rows(0).Item("受領日").ToString()
        Me.txtNewSerial.ppText = dstOrders.Tables(0).Rows(0).Item("新シリアル").ToString()
        liData = Nothing
        liData = Me.ddlInsResult.ppDropDownList.Items.FindByValue(dstOrders.Tables(0).Rows(0).Item("検品結果").ToString())
        If liData Is Nothing Then
            Me.ddlInsResult.ppDropDownList.SelectedIndex = 0
        Else
            Me.ddlInsResult.ppSelectedValue = dstOrders.Tables(0).Rows(0).Item("検品結果").ToString()
        End If
        Me.txtSubNo.ppText = dstOrders.Tables(0).Rows(0).Item("代替製造番号").ToString()
        liData = Nothing
        liData = Me.ddlStatusCd.Items.FindByValue(dstOrders.Tables(0).Rows(0).Item("ステータス").ToString())
        If liData Is Nothing Then
            Me.ddlStatusCd.SelectedIndex = 0
        Else
            Me.ddlStatusCd.SelectedValue = dstOrders.Tables(0).Rows(0).Item("ステータス").ToString()
        End If
        Me.txtReqDt.ppText = dstOrders.Tables(0).Rows(0).Item("請求年月").ToString()
        liData = Nothing
        liData = Me.ddlTokCls.ppDropDownList.Items.FindByValue(dstOrders.Tables(0).Rows(0).Item("TOK区分").ToString())
        If liData Is Nothing Then
            Me.ddlTokCls.ppDropDownList.SelectedIndex = 0
        Else
            Me.ddlTokCls.ppSelectedValue = dstOrders.Tables(0).Rows(0).Item("TOK区分").ToString()
        End If
        If dstOrders.Tables(0).Rows(0).Item("運用状況").ToString() <> "" Then
            Me.ddlStatus.SelectedIndex = Integer.Parse(dstOrders.Tables(0).Rows(0).Item("運用状況").ToString())
        Else
            Me.ddlStatus.SelectedIndex = 0
        End If


    End Sub
    ''' <summary>
    ''' '修理依頼明細データ表示処理.
    ''' </summary>
    ''' <param name="ipstrRepairNo"></param>
    ''' <remarks></remarks>
    Private Sub msSetList_Data(ByVal ipstrRepairNo As String, ByVal ipstrBranch As String)

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

        Try

            '開始ログ出力.
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(objCn) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else

                '修理依頼明細データ取得.
                objCmd = New SqlCommand(M_DISP_ID & "_S4", objCn)
                objCmd.Parameters.Add(pfSet_Param("repair_no", SqlDbType.NVarChar, ipstrRepairNo))
                objCmd.Parameters.Add(pfSet_Param("branch", SqlDbType.NVarChar, ipstrBranch))
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'レコードがある場合、会社名を非活性.
                If objDs.Tables(0).Rows.Count = 0 Then
                    ViewState(sCnsCompFlg) = "1"
                    Me.ddlCompNm.Enabled = True
                Else
                    ViewState(sCnsCompFlg) = "0"
                    Me.ddlCompNm.Enabled = False
                End If

                '取得したデータをリストに設定
                Me.grvList.DataSource = objDs.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            End If

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "修理依頼明細データ")
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

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub
    ''' <summary>
    ''' 活性制御.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msEnableScreen(ByVal strTerms As String)

        Select Case strTerms.ToString.Trim

            Case ClsComVer.E_遷移条件.参照
                '--------------------------------
                '2014/05/19 後藤　ここから
                '--------------------------------
                'Me.btnPrint.Enabled = True
                Master.ppLeftButton1.Enabled = True
                'Me.btnUpdate.Text = "更新"
                'Me.btnUpdate.Enabled = False
                Master.ppRigthButton1.Text = "更新"
                Master.ppRigthButton1.Enabled = False
                'Me.btnAllClear.Text = "元に戻す"
                'Me.btnAllClear.Enabled = False
                Master.ppRigthButton2.Text = "元に戻す"
                Master.ppRigthButton2.Enabled = False
                '--------------------------------
                '2014/05/19 後藤　ここまで
                '--------------------------------
                '--------------------------------
                '2014/06/24 武　ここから
                '--------------------------------
                Me.ddlPartsNm.Enabled = False
                Me.txtQuantity.ppEnabled = False
                Me.txtBranch.ppEnabled = False
                Me.dtbTroubleDt.ppEnabled = False
                Me.txtTboxid.ppEnabled = False
                Me.ddlProductNm.Enabled = False
                Me.txtHallNm.ppEnabled = False
                Me.txtOldVer.ppEnabled = False
                Me.txtNewVer.ppEnabled = False
                Me.txtSerial.ppEnabled = False
                Me.ddlTrouble.Enabled = False
                Me.txtTrouble.Enabled = False
                Me.ddlTraderCd.Enabled = False
                Me.txtZipNo.Enabled = False
                Me.txtAddr.Enabled = False
                Me.txtTel.Enabled = False
                Me.txtCharge.Enabled = False
                Me.ddlCompNm.Enabled = False
                Me.txtRepairContent.Enabled = False
                Me.txtRepairCharge.ppEnabled = False
                Me.dtbAppasendDt.ppEnabled = False
                Me.txtVer1.ppEnabled = False
                Me.txtHddNo1.ppEnabled = False
                Me.txtHddCls1.ppEnabled = False
                Me.dtbAppaarvDt.ppEnabled = False
                Me.ddlWrkNo11.Enabled = False
                Me.ddlWrkNo12.Enabled = False
                Me.ddlWrkNo13.Enabled = False
                Me.ddlWrkNo14.Enabled = False
                Me.ddlWrkNo21.Enabled = False
                Me.ddlPartsNo1.Enabled = False
                Me.ddlTmpResult.ppEnabled = False
                Me.dtbRsltSndDt.ppEnabled = False
                Me.ddlCmprtnCd.Enabled = False
                Me.ddlCmprtnCd.Enabled = False
                Me.txtVer2.ppEnabled = False
                Me.txtHddNo2.ppEnabled = False
                Me.txtHddCls2.ppEnabled = False
                Me.dtbArrivalDt.ppEnabled = False
                Me.txtNewSerial.ppEnabled = False
                Me.ddlInsResult.ppEnabled = False
                Me.txtSubNo.ppEnabled = False
                Me.ddlTokCls.ppEnabled = False
                Me.ddlStatusCd.Enabled = False
                Me.txtReqDt.ppEnabled = False
                Me.ddlPartsNm.Enabled = False
                Me.txtQuantity.ppEnabled = False
                Me.dtbCmpSndDt.ppEnabled = False
                Me.ddlStatus.Enabled = False
                '--------------------------------
                '2014/06/24 武　ここまで
                '--------------------------------
                Me.pnlRepair.Enabled = False
                '--------------------------------
                '2014/06/24 武　ここから
                '--------------------------------
                'Me.pnlComplete.Enabled = False
                Me.btnInsert.Enabled = False
                Me.btnDelete.Enabled = False
                Me.btnChange.Enabled = False
                Me.btnClear.Enabled = False
                '--------------------------------
                '2014/06/24 武　ここまで
                '--------------------------------
                Return

            Case ClsComVer.E_遷移条件.更新
                '--------------------------------
                '2014/05/19 後藤　ここから
                '--------------------------------
                Me.pnlRepair.Enabled = True
                Me.pnlComplete.Enabled = True
                Me.grvList.Enabled = True
                'Me.btnPrint.Enabled = True
                Master.ppLeftButton1.Enabled = True
                'Me.btnUpdate.Text = "更新"
                'Me.btnUpdate.Enabled = True
                Master.ppRigthButton1.Text = "更新"
                Master.ppRigthButton1.Enabled = True
                'Me.btnAllClear.Enabled = True
                'Me.btnAllClear.Text = "元に戻す"
                Master.ppRigthButton2.Enabled = True
                Master.ppRigthButton2.Text = "元に戻す"
                '--------------------------------
                '2014/05/19 後藤　ここまで
                '--------------------------------
                btnInsert.Enabled = True
                btnChange.Enabled = False
                btnDelete.Enabled = False
                '会社名活性制御.
                If ViewState(sCnsCompFlg) = "0" Then
                    Me.ddlCompNm.Enabled = False
                Else
                    Me.ddlCompNm.Enabled = True
                End If
                cuvCompNm.ValidationGroup = 1

            Case ClsComVer.E_遷移条件.登録
                '--------------------------------
                '2014/05/19 後藤　ここから
                '--------------------------------
                Me.pnlRepair.Enabled = True
                Me.pnlComplete.Enabled = False
                'Me.btnPrint.Enabled = False
                Master.ppLeftButton1.Enabled = False
                'Me.btnUpdate.Text = "登録"
                'Me.btnUpdate.Enabled = True
                Master.ppRigthButton1.Text = "登録"
                Master.ppRigthButton1.Enabled = True

                'Me.btnAllClear.Enabled = True
                'Me.btnAllClear.Text = "クリア"
                Master.ppRigthButton2.Enabled = True
                Master.ppRigthButton2.Text = "クリア"
                cuvCompNm.ValidationGroup = ""
                '--------------------------------
                '2014/05/19 後藤　ここまで
                '--------------------------------
        End Select

        '空白時は対象コントロールを非活性.
        If Me.ddlCompNm.SelectedIndex = "0" Then
            Me.ddlWrkNo11.Enabled = False
            Me.ddlWrkNo12.Enabled = False
            Me.ddlWrkNo13.Enabled = False
            Me.ddlWrkNo14.Enabled = False
            Me.ddlWrkNo21.Enabled = False
            Me.ddlPartsNo1.Enabled = False
            Me.ddlPartsNm.Enabled = False
            Me.ddlWrkNo11.SelectedIndex = "0"
            Me.ddlWrkNo12.SelectedIndex = "0"
            Me.ddlWrkNo13.SelectedIndex = "0"
            Me.ddlWrkNo14.SelectedIndex = "0"
            Me.ddlWrkNo21.SelectedIndex = "0"
            Me.ddlPartsNo1.SelectedIndex = "0"
            Me.ddlPartsNm.SelectedIndex = "0"
        Else
            Me.ddlWrkNo11.Enabled = True
            Me.ddlWrkNo12.Enabled = True
            Me.ddlWrkNo13.Enabled = True
            Me.ddlWrkNo14.Enabled = True
            Me.ddlWrkNo21.Enabled = True
            Me.ddlPartsNo1.Enabled = True
            Me.ddlPartsNm.Enabled = True
            Me.txtQuantity.ppEnabled = True
        End If

        If ddlStatusCd.SelectedValue = "04" And txtReqDt.ppText <> "" And txtReqDt.ppText < System.DateTime.Now.ToString("yyyyMM") Then
            ddlPartsNm.Enabled = False
            txtQuantity.ppEnabled = False
            btnInsert.Enabled = False
            btnChange.Enabled = False
            btnDelete.Enabled = False
        End If

    End Sub
    ''' <summary>
    ''' ボタンアクションの設定.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_ButtonAction()

        AddHandler Me.txtBranch.ppTextBox.TextChanged, AddressOf txtBranch_TextChanged
        AddHandler Me.ddlTraderCd.TextChanged, AddressOf ddlTraderCd_TextChanged
        AddHandler Me.ddlCompNm.TextChanged, AddressOf ddlCompNm_TextChanged
        AddHandler Me.btnClear.Click, AddressOf btnClear_Click
        AddHandler Me.btnInsert.Click, AddressOf btnInsert_Click
        AddHandler Me.btnChange.Click, AddressOf btnChange_Click
        AddHandler Me.btnDelete.Click, AddressOf btnDelete_Click
        '--------------------------------
        '2014/05/19 後藤　ここから
        '--------------------------------
        'AddHandler Me.btnPrint.Click, AddressOf btnPrint_Click
        'AddHandler Me.btnAllClear.Click, AddressOf btnAllClear_Click
        'AddHandler Me.btnUpdate.Click, AddressOf btnUpdate_Click

        AddHandler Master.ppLeftButton1.Click, AddressOf btnPrint_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnAllClear_Click
        AddHandler Master.ppRigthButton1.Click, AddressOf btnUpdate_Click

        AddHandler Me.txtTboxid.ppTextBox.TextChanged, AddressOf txtTboxId_TextChanged
        Me.txtBranch.ppTextBox.AutoPostBack = True
        Me.txtTboxid.ppTextBox.AutoPostBack = True

        '確認メッセージ設定.
        Me.btnInsert.OnClientClick = pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "交換部品")
        Me.btnChange.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "交換部品")
        Me.btnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "交換部品")
        'Me.btnPrint.OnClientClick = pfGet_OCClickMes("10002", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "修理依頼書")
        Master.ppLeftButton1.OnClientClick = pfGet_OCClickMes("10002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "修理依頼書")

        'If Me.btnUpdate.Text = "更新" Then
        '    Me.btnUpdate.OnClientClick = pfGet_OCClickMes("00004", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "修理依頼書")
        'Else
        '    Me.btnUpdate.OnClientClick = pfGet_OCClickMes("00008", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "修理依頼書")
        'End If

        If Master.ppRigthButton1.Text = "更新" Then
            Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "修理依頼書")
        Else
            Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("00008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "修理依頼書")
        End If

        Master.ppRigthButton1.ValidationGroup = "1"
        '--------------------------------
        '2014/05/19 後藤　ここまで
        '--------------------------------
    End Sub
    ''' <summary>
    ''' 交換部品活性制御.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Private Sub msEnableChangeScreen(ByVal sender As Object)

        Select Case sender.ID
            Case "btnClear"        'クリア
                '--------------------------------
                '2014/06/24 武　ここから
                '--------------------------------
                If ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.参照 Then
                    '--------------------------------
                    '2014/06/24 武　ここまで
                    '--------------------------------
                    Me.btnInsert.Enabled = True
                    Me.btnDelete.Enabled = False
                    Me.btnChange.Enabled = False
                    '--------------------------------
                    '2014/06/24 武　ここから
                    '--------------------------------
                Else
                    Me.btnInsert.Enabled = False
                    Me.btnDelete.Enabled = False
                    Me.btnChange.Enabled = False
                    Me.btnClear.Enabled = False
                End If
                '--------------------------------
                '2014/06/24 武　ここまで
                '--------------------------------
            Case "btnInsert"       '追加.
                Me.btnInsert.Enabled = True
                Me.btnDelete.Enabled = False
                Me.btnChange.Enabled = False
            Case "btnChange"       '更新.
                Me.btnInsert.Enabled = False
                Me.btnDelete.Enabled = True
                Me.btnChange.Enabled = True
            Case "btnDelete"       '削除.
                Me.btnInsert.Enabled = True
                Me.btnDelete.Enabled = False
                Me.btnChange.Enabled = False
            Case "grvList"          '選択.
                If ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.参照 Then
                    Me.btnInsert.Enabled = False
                    Me.btnDelete.Enabled = True
                    Me.btnChange.Enabled = True
                Else
                    Me.btnInsert.Enabled = False
                    Me.btnDelete.Enabled = False
                    Me.btnChange.Enabled = False
                    Me.btnClear.Enabled = True
                End If
        End Select


        If ddlStatusCd.SelectedValue = "04" And txtReqDt.ppText <> "" And txtReqDt.ppText < System.DateTime.Now.ToString("yyyyMM") Then
            ddlPartsNm.Enabled = False
            txtQuantity.ppEnabled = False
            btnInsert.Enabled = False
            btnChange.Enabled = False
            btnDelete.Enabled = False
        End If
    End Sub
    ''' <summary>
    ''' 修理依頼書入力チェック.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheck_RepairError()

        Dim strErr As String
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet

        '故障発生日形式チェック.
        strErr = pfCheck_DateErr(Me.dtbTroubleDt.ppText, False, ClsComVer.E_日付形式.年月日)
        If strErr <> String.Empty Then
            Me.dtbTroubleDt.psSet_ErrorNo(strErr)
        End If

        '故障発生日下限上限チェック.
        If Not Me.dtbTroubleDt.ppText <= DateTime.Now.ToString("yyyy/MM/dd") Then
            Me.dtbTroubleDt.psSet_ErrorNo("6001", "故障発生日", "システム日付")
        End If

        '--------------------------------
        '2014/05/20 後藤　ここから
        '--------------------------------
        '空白の場合は検索しない.
        If Me.txtTboxid.ppTextBox.Text.Trim <> String.Empty Then
            If mfGet_HallData_Sel(Me.txtTboxid.ppText) = False Then
                txtTboxid.psSet_ErrorNo("2002", "入力されたTBOXID")
                Me.txtTboxid.ppTextBox.Focus()
                Me.txtHallNm.ppText = String.Empty
            End If
        End If

        '接続.
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If
        Try
            '●●● 稲葉 CommentOut & Add：ここから-------------------------------------------------
            ''修理依頼データ取得.
            'cmdDB = New SqlCommand(M_DISP_ID & "_S10", conDB)
            'cmdDB.Parameters.Add(pfSet_Param("repair_no", SqlDbType.NVarChar, Me.lblRepair_No.Text))
            'dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            'If Integer.Parse(Me.txtBranch.ppText) > (Integer.Parse(dstOrders.Tables(0).Rows(0).Item("最大枝番").ToString) + 1) Then
            '    txtBranch.psSet_ErrorNo("4001", "新規登録を行う場合", "枝番を " & (Integer.Parse(dstOrders.Tables(0).Rows(0).Item("最大枝番").ToString) + 1).ToString)
            '    Me.txtBranch.ppTextBox.Focus()
            '    Me.txtHallNm.ppText = String.Empty
            'End If

            '修理依頼データ取得.
            cmdDB = New SqlCommand(M_DISP_ID & "_S10", conDB)
            cmdDB.Parameters.Add(pfSet_Param("repair_no", SqlDbType.NVarChar, Me.lblRepair_No.Text))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            If Me.txtBranch.ppText = "0" Then
                txtBranch.psSet_ErrorNo("4001", "新規登録を行う場合", "枝番を " & (Integer.Parse(dstOrders.Tables(0).Rows(0).Item("最大枝番").ToString) + 1).ToString)
                Me.txtBranch.ppTextBox.Focus()
            Else
                If Master.ppRigthButton1.Text <> "更新" Then
                    If Me.txtBranch.ppText <> dstOrders.Tables(0).Rows(0).Item("最大枝番").ToString Then
                        If txtBranch.ppText.Trim = String.Empty OrElse Integer.Parse(Me.txtBranch.ppText) <> (Integer.Parse(dstOrders.Tables(0).Rows(0).Item("最大枝番").ToString) + 1) Then
                            txtBranch.psSet_ErrorNo("4001", "新規登録を行う場合", "枝番を " & (Integer.Parse(dstOrders.Tables(0).Rows(0).Item("最大枝番").ToString) + 1).ToString)
                            Me.txtBranch.ppTextBox.Focus()
                        End If
                    End If
                End If
            End If
            '●●● 稲葉 CommentOut & Add：ここまで---------------------------------------------------

        Catch ex As Exception
            psMesBox(Me, "30001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Finally
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End Try

    End Sub

    ''' <summary>
    ''' 修理依頼書データ追加チェック.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheck_AddError()

        Dim dtrMes As DataRow
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet

        '接続.
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If
        Try
            '修理依頼データ取得.
            cmdDB = New SqlCommand(M_DISP_ID & "_S11", conDB)
            cmdDB.Parameters.Add(pfSet_Param("repair_no", SqlDbType.NVarChar, Me.lblRepair_No.Text))
            cmdDB.Parameters.Add(pfSet_Param("branch_no", SqlDbType.NVarChar, Me.txtBranch.ppText))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            If Not dstOrders.Tables(0).Rows(0).Item("会社コード").ToString = ddlCompNm.SelectedValue Then
                dtrMes = pfGet_ValMes("5001", "会社名")
                If dstOrders.Tables(0).Rows(0).Item("会社コード").ToString = "" Then
                    cuvCompNm.ErrorMessage = "会社名が登録されていません。更新処理を行ってください。"
                Else
                    cuvCompNm.ErrorMessage = "会社名が変更されています。更新処理を行ってください。"
                End If
                cuvCompNm.Text = "未登録エラー"
                cuvCompNm.IsValid = False
            Else
            End If

        Catch ex As Exception
            psMesBox(Me, "30001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Finally
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End Try


    End Sub

    ''' <summary>
    ''' 通知完了登録入力チェック.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheck_CompleteError()

        Dim strErr As String
        Dim strReqDt As String

        '--------------------------------
        '2014/05/08 後藤　ここから
        '--------------------------------
        '診断結果送付日下限上限チェック.
        If Me.dtbRsltSndDt.ppText <> String.Empty AndAlso Me.dtbTroubleDt.ppText <> String.Empty Then
            'If Not Me.dtbRsltSndDt.ppText <= DateTime.Now.ToString("yyyy/MM/dd") Then
            '    Me.dtbRsltSndDt.psSet_ErrorNo("6001", "診断結果送付日", "システム日付")
            'End If

            '診断結果送付日下限上限チェック.
            If Not Me.dtbRsltSndDt.ppText >= Me.dtbTroubleDt.ppText Then
                Me.dtbRsltSndDt.psSet_ErrorNo("6003", "診断結果送付日", "故障発生日")
            End If
        End If

        If Me.dtbRsltSndDt.ppText <> String.Empty Then
            If Not Me.dtbRsltSndDt.ppText <= DateTime.Now.ToString("yyyy/MM/dd") Then
                Me.dtbRsltSndDt.psSet_ErrorNo("6001", "診断結果送付日", "システム日付")
            End If
        End If

        '完了発送日下限上限チェック.
        If Me.dtbCmpSndDt.ppText <> String.Empty AndAlso Me.dtbTroubleDt.ppText <> String.Empty Then
            'If Not Me.dtbCmpSndDt.ppText <= DateTime.Now.ToString("yyyy/MM/dd") Then
            '    Me.dtbCmpSndDt.psSet_ErrorNo("6001", "完了発送日", "システム日付")
            'End If

            '完了発送日下限上限チェック.
            If Not Me.dtbCmpSndDt.ppText >= Me.dtbTroubleDt.ppText Then
                Me.dtbCmpSndDt.psSet_ErrorNo("6003", "完了発送日", "故障発生日")
            End If
        End If

        If Me.dtbCmpSndDt.ppText <> String.Empty Then
            If Not Me.dtbCmpSndDt.ppText <= DateTime.Now.ToString("yyyy/MM/dd") Then
                Me.dtbCmpSndDt.psSet_ErrorNo("6001", "完了発送日", "システム日付")
            End If
        End If

        '受領日下限上限チェック.
        If Me.dtbArrivalDt.ppText <> String.Empty AndAlso Me.dtbTroubleDt.ppText <> String.Empty Then
            'If Not Me.dtbArrivalDt.ppText <= DateTime.Now.ToString("yyyy/MM/dd") Then
            '    Me.dtbArrivalDt.psSet_ErrorNo("6001", "受領日", "システム日付")
            'End If

            '受領日下限上限チェック.
            If Not Me.dtbArrivalDt.ppText >= Me.dtbTroubleDt.ppText Then
                Me.dtbArrivalDt.psSet_ErrorNo("6003", "受領日", "故障発生日")
            End If
        End If

        If Me.dtbArrivalDt.ppText <> String.Empty Then
            If Not Me.dtbArrivalDt.ppText <= DateTime.Now.ToString("yyyy/MM/dd") Then
                Me.dtbArrivalDt.psSet_ErrorNo("6001", "完了発送日", "システム日付")
            End If
        End If

        '--------------------------------
        '2014/05/08 後藤　ここまで
        '--------------------------------

        '空白時はチェックしない.
        If Me.txtReqDt.ppText <> String.Empty Then

            '請求年月桁数チェック.
            strErr = pfCheck_TxtErr(Me.txtReqDt.ppText, False, True, False, True, 6, String.Empty, False)
            If strErr <> String.Empty Then
                Me.txtReqDt.psSet_ErrorNo(strErr, "請求年月", "6")
                Exit Sub
            End If

            '請求年月整合性チェック.
            strErr = pfCheck_TxtErr(Me.txtReqDt.ppText, False, True, True, True, 6, "[0-9][0-9][0-9][0-9]([0][1-9]|[1][0-2])", False)
            If strErr <> "" Then
                Me.txtReqDt.psSet_ErrorNo(strErr, "請求年月", "年月")
                Exit Sub
            End If

            '請求年月下限上限チェック.
            strReqDt = Me.txtReqDt.ppText.Substring(0, 4) & "/" & Me.txtReqDt.ppText.Substring(4, 2) & "/" & "01"
            If Not strReqDt >= "2000/01/01" Then
                Me.txtReqDt.psSet_ErrorNo("6003", "請求年月", "2000年")
                Exit Sub
            End If

            '更新用.
            ViewState(sCnsReqDt) = strReqDt
        End If


    End Sub
    ''' <summary>
    ''' ホールのデータ取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfGet_HallData_Sel(ByVal ipstrtbox_id As String) As Boolean
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet        '表示情報のデータセット
        Dim maxVal As Integer = 0

        '--------------------------------
        '2014/05/20 後藤　ここから
        '--------------------------------
        mfGet_HallData_Sel = False
        '--------------------------------
        '2014/05/20 後藤　ここまで
        '--------------------------------

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

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                mfGet_HallData_Sel = False
            End If

            'リストデータ取得
            'REPUPDP002-002
            '            cmdDB = New SqlCommand(sCnsSqlid_028, conDB)
            cmdDB = New SqlCommand(sCnsSqlid_062, conDB)
            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, ipstrtbox_id))
            End With
            'REPUPDP002-002
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            '--------------------------------
            '2014/05/20 後藤　ここから
            '--------------------------------
            If dstOrders.Tables(0).Rows.Count = 0 Then
                Exit Function
            End If
            '--------------------------------
            '2014/05/20 後藤　ここまで
            '--------------------------------

            Me.txtHallNm.ppText = dstOrders.Tables(0).Rows(0).Item("ホール名").ToString()
            ViewState(sCnsHall) = dstOrders.Tables(0).Rows(0).Item("ホールコード").ToString()

            '正常終了
            '--------------------------------
            '2014/05/20 後藤　ここから
            '--------------------------------
            'msGet_HallData_Sel = True
            mfGet_HallData_Sel = True
            '--------------------------------
            '2014/05/20 後藤　ここまで
            '--------------------------------

        Catch ex As Exception
            '--------------------------------
            '2014/05/20 後藤　ここから
            '--------------------------------
            'psMesBox(Me, "00004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "ホール情報取得")
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ情報取得")
            '--------------------------------
            '2014/05/20 後藤　ここまで
            '--------------------------------

            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------

            '--------------------------------
            '2014/05/20 後藤　ここから
            '--------------------------------
            'msGet_HallData_Sel = False
            mfGet_HallData_Sel = False
            '--------------------------------
            '2014/05/20 後藤　ここまで
            '--------------------------------
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                '--------------------------------
                '2014/05/20 後藤　ここから
                '--------------------------------
                'msGet_HallData_Sel = False
                mfGet_HallData_Sel = False
                '--------------------------------
                '2014/05/20 後藤　ここまで
                '--------------------------------
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function
    ''' <summary>
    ''' ドロップダウンリスト生成処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfGet_DropListData_Sel(ByVal ipstrMkr_cd As String, ByVal ipstrSys_cd As String) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet        '表示情報のデータセット
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            '開始ログ出力.
            psLogStart(Me)

            '接続.
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                mfGet_DropListData_Sel = False
            End If

            '製品名ドロップダウンリスト生成.
            cmdDB = New SqlCommand(M_DISP_ID & "_S7", conDB)
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlProductNm.DataSource = dstOrders.Tables(0)
            Me.ddlProductNm.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlProductNm.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlProductNm.DataBind()
            '--------------------------------
            '2014/05/19 後藤　ここから
            '--------------------------------
            'Me.ddlProductNm.Items.Insert(0, " ")
            Me.ddlProductNm.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/19 後藤　ここまで
            '--------------------------------
            Me.ddlProductNm.SelectedIndex = 0

            '障害内容ドロップダウンリスト生成.
            cmdDB = New SqlCommand(sCnsSqlid_018, conDB)
            cmdDB.Parameters.Add(pfSet_Param("sys_code", SqlDbType.NVarChar, ipstrSys_cd))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlTrouble.DataSource = dstOrders.Tables(0)
            Me.ddlTrouble.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlTrouble.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlTrouble.DataBind()
            '--------------------------------
            '2014/05/19 後藤　ここから
            '--------------------------------
            'Me.ddlTrouble.Items.Insert(0, " ")
            Me.ddlTrouble.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/19 後藤　ここまで
            '--------------------------------
            Me.ddlTrouble.SelectedIndex = 0

            '完了品の返却先ドロップダウンリスト生成.
            cmdDB = New SqlCommand(sCnsSqlid_039, conDB)
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlCmprtnCd.DataSource = dstOrders.Tables(0)
            Me.ddlCmprtnCd.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlCmprtnCd.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlCmprtnCd.DataBind()
            '--------------------------------
            '2014/05/19 後藤　ここから
            '--------------------------------
            'Me.ddlCmprtnCd.Items.Insert(0, " ")
            Me.ddlCmprtnCd.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/19 後藤　ここまで
            '--------------------------------
            Me.ddlCmprtnCd.SelectedIndex = 0

            '完了品の送付先ドロップダウンリスト生成.
            cmdDB = New SqlCommand(sCnsSqlid_039, conDB)
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlTraderCd.DataSource = dstOrders.Tables(0)
            Me.ddlTraderCd.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlTraderCd.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlTraderCd.DataBind()
            '--------------------------------
            '2014/05/19 後藤　ここから
            '--------------------------------
            'Me.ddlTraderCd.Items.Insert(0, " ")
            Me.ddlTraderCd.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/19 後藤　ここまで
            '--------------------------------
            Me.ddlTraderCd.SelectedIndex = 0

            '会社名ドロップダウンリスト生成.
            cmdDB = New SqlCommand(sCnsSqlid_040, conDB)
            cmdDB.Parameters.Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, "1"))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlCompNm.DataSource = dstOrders.Tables(0)
            Me.ddlCompNm.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlCompNm.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlCompNm.DataBind()
            '--------------------------------
            '2014/05/19 後藤　ここから
            '--------------------------------
            'Me.ddlCompNm.Items.Insert(0, " ")
            Me.ddlCompNm.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/19 後藤　ここまで
            '--------------------------------
            Me.ddlCompNm.SelectedIndex = 0

            '作業項番1ドロップダウンリスト生成.
            cmdDB = New SqlCommand(sCnsSqlid_027, conDB)
            cmdDB.Parameters.Add(pfSet_Param("maker_cd", SqlDbType.NVarChar, ipstrMkr_cd))
            cmdDB.Parameters.Add(pfSet_Param("wrk_cls", SqlDbType.NVarChar, "1"))
            cmdDB.Parameters.Add(pfSet_Param("part_cd", SqlDbType.NVarChar, ""))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlWrkNo11.DataSource = dstOrders.Tables(0)
            Me.ddlWrkNo12.DataSource = dstOrders.Tables(0)
            Me.ddlWrkNo13.DataSource = dstOrders.Tables(0)
            Me.ddlWrkNo14.DataSource = dstOrders.Tables(0)
            Me.ddlWrkNo11.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlWrkNo11.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlWrkNo12.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlWrkNo12.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlWrkNo13.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlWrkNo13.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlWrkNo14.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlWrkNo14.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlWrkNo11.DataBind()
            Me.ddlWrkNo12.DataBind()
            Me.ddlWrkNo13.DataBind()
            Me.ddlWrkNo14.DataBind()
            '--------------------------------
            '2014/05/19 後藤　ここから
            '--------------------------------
            'Me.ddlWrkNo11.Items.Insert(0, " ")
            'Me.ddlWrkNo12.Items.Insert(0, " ")
            'Me.ddlWrkNo13.Items.Insert(0, " ")
            'Me.ddlWrkNo14.Items.Insert(0, " ")
            Me.ddlWrkNo11.Items.Insert(0, New ListItem(Nothing, Nothing))
            Me.ddlWrkNo12.Items.Insert(0, New ListItem(Nothing, Nothing))
            Me.ddlWrkNo13.Items.Insert(0, New ListItem(Nothing, Nothing))
            Me.ddlWrkNo14.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/19 後藤　ここまで
            '--------------------------------
            Me.ddlWrkNo11.SelectedIndex = 0
            Me.ddlWrkNo12.SelectedIndex = 0
            Me.ddlWrkNo13.SelectedIndex = 0
            Me.ddlWrkNo14.SelectedIndex = 0

            '作業項番2ドロップダウンリスト生成.
            cmdDB = New SqlCommand(sCnsSqlid_027, conDB)
            cmdDB.Parameters.Add(pfSet_Param("maker_cd", SqlDbType.NVarChar, ipstrMkr_cd))
            cmdDB.Parameters.Add(pfSet_Param("wrk_cls", SqlDbType.NVarChar, "2"))
            cmdDB.Parameters.Add(pfSet_Param("part_cd", SqlDbType.NVarChar, ""))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlWrkNo21.DataSource = dstOrders.Tables(0)
            Me.ddlWrkNo21.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlWrkNo21.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlWrkNo21.DataBind()
            '--------------------------------
            '2014/05/19 後藤　ここから
            '--------------------------------
            'Me.ddlWrkNo21.Items.Insert(0, " ")
            Me.ddlWrkNo21.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/19 後藤　ここまで
            '--------------------------------
            Me.ddlWrkNo21.SelectedIndex = 0

            '部品項番1ドロップダウンリスト生成.
            cmdDB = New SqlCommand(sCnsSqlid_027, conDB)
            cmdDB.Parameters.Add(pfSet_Param("maker_cd", SqlDbType.NVarChar, ipstrMkr_cd))
            cmdDB.Parameters.Add(pfSet_Param("wrk_cls", SqlDbType.NVarChar, "3"))
            cmdDB.Parameters.Add(pfSet_Param("part_cd", SqlDbType.NVarChar, ""))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlPartsNo1.DataSource = dstOrders.Tables(0)
            Me.ddlPartsNo1.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlPartsNo1.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlPartsNo1.DataBind()
            '--------------------------------
            '2014/05/19 後藤　ここから
            '--------------------------------
            'Me.ddlPartsNo1.Items.Insert(0, " ")
            Me.ddlPartsNo1.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/19 後藤　ここまで
            '--------------------------------
            Me.ddlPartsNo1.SelectedIndex = 0

            'ステータスドロップダウンリスト生成.
            cmdDB = New SqlCommand(sCnsSqlid_015, conDB)
            cmdDB.Parameters.Add(pfSet_Param("duty_cd", SqlDbType.NVarChar, "86"))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlStatusCd.DataSource = dstOrders.Tables(0)
            Me.ddlStatusCd.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlStatusCd.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlStatusCd.DataBind()
            '--------------------------------
            '2014/05/19 後藤　ここから
            '--------------------------------
            'Me.ddlStatusCd.Items.Insert(0, " ")
            Me.ddlStatusCd.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/19 後藤　ここまで
            '--------------------------------
            Me.ddlStatusCd.SelectedIndex = 0

            '部品名ドロップダウンリスト生成.
            cmdDB = New SqlCommand(sCnsSqlid_027, conDB)
            cmdDB.Parameters.Add(pfSet_Param("maker_cd", SqlDbType.NVarChar, ipstrMkr_cd))
            cmdDB.Parameters.Add(pfSet_Param("wrk_cls", SqlDbType.NVarChar, "4"))
            cmdDB.Parameters.Add(pfSet_Param("part_cd", SqlDbType.NVarChar, ""))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlPartsNm.DataSource = dstOrders.Tables(0)
            Me.ddlPartsNm.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlPartsNm.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlPartsNm.DataBind()
            '--------------------------------
            '2014/05/19 後藤　ここから
            '--------------------------------
            'Me.ddlPartsNm.Items.Insert(0, " ")
            Me.ddlPartsNm.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/19 後藤　ここまで
            '--------------------------------
            Me.ddlPartsNm.SelectedIndex = 0

            '正常終了.
            mfGet_DropListData_Sel = True

        Catch ex As Exception
            psMesBox(Me, "30001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            mfGet_DropListData_Sel = False
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                mfGet_DropListData_Sel = False
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function
    ''' <summary>
    ''' ドロップダウンリスト生成処理(会社名ドロップダウンリスト変更時）
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msGet_CompDropListData_Sel(ByVal ipstrMkr_cd As String) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet        '表示情報のデータセット
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            '開始ログ出力.
            psLogStart(Me)

            '接続.
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                msGet_CompDropListData_Sel = False
            End If

            '作業項番1ドロップダウンリスト生成.
            cmdDB = New SqlCommand(sCnsSqlid_027, conDB)
            cmdDB.Parameters.Add(pfSet_Param("maker_cd", SqlDbType.NVarChar, ipstrMkr_cd))
            cmdDB.Parameters.Add(pfSet_Param("wrk_cls", SqlDbType.NVarChar, "1"))
            cmdDB.Parameters.Add(pfSet_Param("part_cd", SqlDbType.NVarChar, ""))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlWrkNo11.DataSource = dstOrders.Tables(0)
            Me.ddlWrkNo12.DataSource = dstOrders.Tables(0)
            Me.ddlWrkNo13.DataSource = dstOrders.Tables(0)
            Me.ddlWrkNo14.DataSource = dstOrders.Tables(0)
            Me.ddlWrkNo11.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlWrkNo11.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlWrkNo12.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlWrkNo12.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlWrkNo13.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlWrkNo13.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlWrkNo14.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlWrkNo14.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlWrkNo11.DataBind()
            Me.ddlWrkNo12.DataBind()
            Me.ddlWrkNo13.DataBind()
            Me.ddlWrkNo14.DataBind()
            '--------------------------------
            '2014/05/19 後藤　ここから
            '--------------------------------
            'Me.ddlWrkNo11.Items.Insert(0, " ")
            'Me.ddlWrkNo12.Items.Insert(0, " ")
            'Me.ddlWrkNo13.Items.Insert(0, " ")
            'Me.ddlWrkNo14.Items.Insert(0, " ")
            Me.ddlWrkNo11.Items.Insert(0, New ListItem(Nothing, Nothing))
            Me.ddlWrkNo12.Items.Insert(0, New ListItem(Nothing, Nothing))
            Me.ddlWrkNo13.Items.Insert(0, New ListItem(Nothing, Nothing))
            Me.ddlWrkNo14.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/19 後藤　ここまで
            '--------------------------------
            Me.ddlWrkNo11.SelectedIndex = 0
            Me.ddlWrkNo12.SelectedIndex = 0
            Me.ddlWrkNo13.SelectedIndex = 0
            Me.ddlWrkNo14.SelectedIndex = 0

            '作業項番2ドロップダウンリスト生成.
            cmdDB = New SqlCommand(sCnsSqlid_027, conDB)
            cmdDB.Parameters.Add(pfSet_Param("maker_cd", SqlDbType.NVarChar, ipstrMkr_cd))
            cmdDB.Parameters.Add(pfSet_Param("wrk_cls", SqlDbType.NVarChar, "2"))
            cmdDB.Parameters.Add(pfSet_Param("part_cd", SqlDbType.NVarChar, ""))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlWrkNo21.DataSource = dstOrders.Tables(0)
            Me.ddlWrkNo21.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlWrkNo21.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlWrkNo21.DataBind()
            '--------------------------------
            '2014/05/19 後藤　ここから
            '--------------------------------
            'Me.ddlWrkNo21.Items.Insert(0, " ")
            Me.ddlWrkNo21.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/19 後藤　ここまで
            '--------------------------------
            Me.ddlWrkNo21.SelectedIndex = 0

            '部品項番1ドロップダウンリスト生成.
            cmdDB = New SqlCommand(sCnsSqlid_027, conDB)
            cmdDB.Parameters.Add(pfSet_Param("maker_cd", SqlDbType.NVarChar, ipstrMkr_cd))
            cmdDB.Parameters.Add(pfSet_Param("wrk_cls", SqlDbType.NVarChar, "3"))
            cmdDB.Parameters.Add(pfSet_Param("part_cd", SqlDbType.NVarChar, ""))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlPartsNo1.DataSource = dstOrders.Tables(0)
            Me.ddlPartsNo1.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlPartsNo1.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlPartsNo1.DataBind()
            '--------------------------------
            '2014/05/19 後藤　ここから
            '--------------------------------
            'Me.ddlPartsNo1.Items.Insert(0, " ")
            Me.ddlPartsNo1.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/19 後藤　ここまで
            '--------------------------------
            Me.ddlPartsNo1.SelectedIndex = 0

            '部品名ドロップダウンリスト生成.
            cmdDB = New SqlCommand(sCnsSqlid_027, conDB)
            cmdDB.Parameters.Add(pfSet_Param("maker_cd", SqlDbType.NVarChar, ipstrMkr_cd))
            cmdDB.Parameters.Add(pfSet_Param("wrk_cls", SqlDbType.NVarChar, "4"))
            cmdDB.Parameters.Add(pfSet_Param("part_cd", SqlDbType.NVarChar, ""))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlPartsNm.DataSource = dstOrders.Tables(0)
            Me.ddlPartsNm.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlPartsNm.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlPartsNm.DataBind()
            '--------------------------------
            '2014/05/19 後藤　ここから
            '--------------------------------
            'Me.ddlPartsNm.Items.Insert(0, " ")
            Me.ddlPartsNm.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/19 後藤　ここまで
            '--------------------------------
            Me.ddlPartsNm.SelectedIndex = 0

            '正常終了.
            msGet_CompDropListData_Sel = True

        Catch ex As Exception
            '--------------------------------
            '2014/05/15 後藤　ここから
            '--------------------------------
            'psMesBox(Me, "00004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "ドロップダウンリスト生成処理")
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ドロップダウンリスト生成処理")
            '--------------------------------
            '2014/05/15 後藤　ここまで
            '--------------------------------

            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            msGet_CompDropListData_Sel = False
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                msGet_CompDropListData_Sel = False
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function
    ''' <summary>
    ''' オブジェクト取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDBNull(ByVal strVal As String) As Object
        If String.IsNullOrEmpty(strVal) = True Then
            Return DBNull.Value
        End If
        Return strVal

    End Function

#End Region

End Class