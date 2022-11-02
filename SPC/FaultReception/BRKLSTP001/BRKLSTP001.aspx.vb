'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜故障受付＞
'*　処理名　　：　ミニ処理票一覧
'*　ＰＧＭＩＤ：　BRKLSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.04.07　：　浜本
'*  更　新　　：　2014.06.17　：　間瀬　レイアウト変更
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'BRKLSTP001-001     2016/03/29      栗原　　　検索欄と一覧のレイアウト上下反転、csv発行機能追加

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

Public Class BRKLSTP001

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
    ''' 登録ボタン表示名称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strBTN_SEARCH_NM As String = "検索"
    Private Const strBTN_SEARCH_CLEAR_NM As String = "検索条件クリア"
    Private Const strBTN_CSV_NM As String = "ＣＳＶ"
    Private Const strBTN_CSV_DTL_NM As String = "ミニ処理票ＣＳＶ"
    ''' <summary>
    ''' 画面ＩＤ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const M_DISP_ID As String = "BRKLSTP001"
    Private Const M_UPD_DISP_ID As String = P_FUN_BRK & P_SCR_UPD & P_PAGE & "001"

    ''' <summary>
    ''' ＴＢＯＸタイプ用グリッドビュー名前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strTBOXTYPE_GRID_NM As String = "BRKLSTP002"

    ''' <summary>
    ''' ミニ処理票画面のパス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strNEXTPAGE_PATH As String = "~/" & P_FLR & "/" & P_FUN_BRK & P_SCR_UPD & P_PAGE & "001/" &
                                         P_FUN_BRK & P_SCR_UPD & P_PAGE & "001.aspx"

    ''' <summary>
    ''' 参照ボタン名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strSANSYO_BTN_NAME As String = "btnReference"

    ''' <summary>
    ''' 更新ボタン名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strUPDATE_BTN_NAME As String = "btnUpdate"

    ''' <summary>
    ''' ViewStateのリロードフラグ管理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IsReloadClick As String = "Reload_Flg_MN"

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

#Region "イベントプロシージャ"

#Region "■ ページ初期処理"

    ''' <summary>
    ''' ページ初期処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        '表設定
        pfSet_GridView(grvList, M_DISP_ID, 36, 11)
        'pfSet_GridView(grvListTboxcls, strTBOXTYPE_GRID_NM, , 11)

    End Sub

#End Region

#Region "■ Page_Load"

    ''' <summary>
    ''' Page_Load処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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

            'ボタン機能の設定
            msSetBtnKmk()

            'スクロールポジションの設定
            MaintainScrollPositionOnPostBack = True

            If Not IsPostBack Then

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                'セッション情報をViewStateに格納
                ViewState(P_SESSION_BCLIST) = Session(P_SESSION_BCLIST)
                ViewState(P_SESSION_USERID) = Session(P_SESSION_USERID)
                ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

                'プログラムＩＤ、画面名設定
                Master.Master.ppProgramID = M_DISP_ID
                'Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)
                Master.Master.ppTitle = "コール処理票一覧"

                'パンくずリスト設定
                Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                'ページクリア
                If mfPageClear() = False Then
                    '(Tbox機種情報取得エラー時)はエラー
                    Throw New Exception
                End If

                '条件検索取得
                msGet_Data("1")

                ViewState(IsReloadClick) = True

                'Master.Master.ppLeftButton1.Enabled = False
                'Master.Master.ppLeftButton2.Enabled = False

                'Dim strScript As New System.Text.StringBuilder
                'strScript.Append("pageonload(" & hdnIsPostBack.ClientID & ");")
                'ClientScript.RegisterStartupScript(Page.GetType(), "", strScript.ToString(), True)
                SetFocus(txtTboxID.ppTextBoxFrom.ClientID)
            Else
                hdnScrollTop.Value = "1"
            End If
        Catch ex As Exception
            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            psClose_Window(Me)
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub

#End Region

#Region "■ユーザー権限"

    '---------------------------
    '2014/04/18 武 ここから
    '---------------------------
    ''' <summary>
    ''' ユーザー権限 CSVボタン制御
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
                If grvList.Rows.Count > 0 Then
                    Master.Master.ppLeftButton1.Enabled = True
                    Master.Master.ppLeftButton2.Enabled = True
                Else
                    Master.Master.ppLeftButton1.Enabled = False
                    Master.Master.ppLeftButton2.Enabled = False
                End If
            Case "SPC"
                Master.Master.ppLeftButton1.Enabled = False
                Master.Master.ppLeftButton2.Enabled = False
            Case "営業所"
                Master.Master.ppLeftButton1.Enabled = False
                Master.Master.ppLeftButton2.Enabled = False
            Case "NGC"
                Master.Master.ppLeftButton1.Enabled = False
                Master.Master.ppLeftButton2.Enabled = False
        End Select



    End Sub
    '---------------------------
    '2014/04/18 武 ここまで
    '---------------------------

#End Region

#Region "■ ボタン押下処理"

    ''' <summary>
    ''' 検索ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
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

            '画面項目チェック
            Page.Validate()
            If Not Page.IsValid Then
                Return
            End If

            '条件検索取得
            msGet_Data("2")
            ViewState(IsReloadClick) = False
            If grvList.Rows.Count > 0 Then
                Master.Master.ppLeftButton1.Enabled = True
                Master.Master.ppLeftButton2.Enabled = True
                hdnScrollTop.Value = "0"
            Else
                Master.Master.ppLeftButton1.Enabled = False
                Master.Master.ppLeftButton2.Enabled = False
            End If

            SetFocus(txtTboxID.ppTextBoxFrom.ClientID)

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
            Return
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub

    ''' <summary>
    ''' 検索条件クリアボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
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

            '初期化
            msClearBtnMethod()

        Catch ex As Exception
            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
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
    ''' 登録ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnInsert_Click(ByVal sender As Object, ByVal e As System.EventArgs)
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

            Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
            Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録
            Session(P_SESSION_OLDDISP) = M_DISP_ID
            Session(P_SESSION_USERID) = ViewState(P_SESSION_USERID)         'ユーザＩＤ

            '--------------------------------
            '2014/04/16 星野　ここから
            '--------------------------------
            '■□■□結合試験時のみ使用予定□■□■
            Dim objStack As New StackFrame
            Dim strPrm As String = ""
            strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
            Dim tmp As Object() = Session(P_KEY)
            If Not tmp Is Nothing Then
                For zz = 0 To tmp.Length - 1
                    If zz <> tmp.Length - 1 Then
                        strPrm &= tmp(zz).ToString & ","
                    Else
                        strPrm &= tmp(zz).ToString
                    End If
                Next
            End If

            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, strNEXTPAGE_PATH, strPrm, "TRANS")
            '■□■□結合試験時のみ使用予定□■□■
            '--------------------------------
            '2014/04/16 星野　ここまで
            '--------------------------------
            'ミニ処理票表示(登録モード)
            psOpen_Window(Me, strNEXTPAGE_PATH)

        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ミニ処理票画面の読み込み")
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
    ''' ミニ処理票ＣＳＶボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCSV_Dtl_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim objCon As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim fileName As String = String.Empty
        objStack = New StackFrame

        'ログ出力開始
        psLogStart(Me)

        If Page.IsValid Then
            'ＤＢ接続
            If clsDataConnect.pfOpen_Database(objCon) Then
                Try
                    'ストアド設定
                    If sender.text = strBTN_CSV_DTL_NM Then
                        fileName = "ミニ処理票_総合"
                        objCmd = New SqlCommand("BRKLSTP001_S12")
                        'BRKLSTP001
                        'ElseIf sender.text = "対応内容ＣＳＶ" Then
                        '    fileName = "対応内容"
                        '    objCmd = New SqlCommand("BRKLSTP001_S13")
                        'BRKLSTP001 END
                    End If
                    objCmd.Connection = objCon
                    objCmd.CommandType = CommandType.StoredProcedure

                    Dim strReloadFlg As String
                    Select Case ViewState(IsReloadClick)
                        Case True
                            strReloadFlg = "1"
                        Case Else
                            strReloadFlg = "0"
                    End Select

                    'パラメータ設定
                    With objCmd.Parameters
                        .Add(pfSet_Param("prmD77_TBOXIDFm", SqlDbType.NVarChar, mfGetDBNull(txtTboxID.ppFromText))) 'TBOXID From
                        .Add(pfSet_Param("prmD77_TBOXIDTo", SqlDbType.NVarChar, mfGetDBNull(txtTboxID.ppToText))) ' TBOXID To
                        .Add(pfSet_Param("prmD77_RCPT_DTFm", SqlDbType.NVarChar, mfGetDBNull(txtUketukeDtFmTo.ppFromText))) '受付日 From
                        .Add(pfSet_Param("prmD77_RCPT_DTTo", SqlDbType.NVarChar, mfGetDBNull(txtUketukeDtFmTo.ppToText))) '受付日 To
                        .Add(pfSet_Param("prmD77_MNG_NOFm", SqlDbType.NVarChar, mfGetDBNull(txtKanriNo.ppFromText))) '管理番号 From
                        .Add(pfSet_Param("prmD77_MNG_NOTo", SqlDbType.NVarChar, mfGetDBNull(txtKanriNo.ppToText))) '管理番号 To
                        '.Add(pfSet_Param("prmD77_NL_CLS", SqlDbType.NVarChar, mfGetDBNull(txtNlKbn.ppText))) 'NL区分
                        '.Add(pfSet_Param("prmD77_EW_CLS", SqlDbType.NVarChar, mfGetDBNull(txtEwKbn.ppText))) 'EW区分
                        .Add(pfSet_Param("prmT01_STATE_CDFm", SqlDbType.NVarChar, mfGetDBNull(ddlPrefectureFm.SelectedValue))) '都道府県 From
                        .Add(pfSet_Param("prmT01_STATE_CDTo", SqlDbType.NVarChar, mfGetDBNull(ddlPrefectureTo.SelectedValue))) '都道府県 To
                        '.Add(pfSet_Param("prmD77_APPA_CLS", SqlDbType.NVarChar, mfGetDBNull(ddlAppaCls.ppSelectedValue))) '機種区分
                        '.Add(pfSet_Param("prmD77_BLNG_CLS", SqlDbType.NVarChar, mfGetDBNull(ddlBlngCls.ppSelectedValue))) '所属区分
                        .Add(pfSet_Param("prmD77_CALL_CLS", SqlDbType.NVarChar, mfGetDBNull(ddlCallCls.ppSelectedValue))) 'コール区分
                        .Add(pfSet_Param("prmD77_RCPT_CHARGE", SqlDbType.NVarChar, mfGetDBNull(txtUketukeNm.ppText))) '受付者
                        '.Add(pfSet_Param("prmD77_STATUS_CDFm", SqlDbType.NVarChar, mfGetDBNull(ddlSagyoJokyoFm.SelectedValue))) '作業状況 From
                        '.Add(pfSet_Param("prmD77_STATUS_CDTo", SqlDbType.NVarChar, mfGetDBNull(ddlSagyoJokyoTo.SelectedValue))) '作業状況 To
                        .Add(pfSet_Param("prmD77_TBOX_VER", SqlDbType.NVarChar, mfGetDBNull(txtVersion.ppText))) 'Ｖｅｒ
                        .Add(pfSet_Param("prmD77_RPT_CD", SqlDbType.NVarChar, mfGetDBNull(txtShinsei.ppText))) '申告内容
                        .Add(pfSet_Param("prmD77_RPT_DTL", SqlDbType.NVarChar, mfGetDBNull(txtShinseiDtl.ppText))) '申告内容詳細
                        .Add(pfSet_Param("prmD78_DEAL_DTL", SqlDbType.NVarChar, mfGetDBNull(txtTaioShinchokuJk.ppText))) '対応進捗状況
                        .Add(pfSet_Param("prmD77_DEAL_CD", SqlDbType.NVarChar, mfGetDBNull(txtSyochi.ppText))) '処置内容(ドロップダウンリスト)
                        .Add(pfSet_Param("prmD77_DEAL_DTL", SqlDbType.NVarChar, mfGetDBNull(txtSyochiDtl.ppText))) '処置内容詳細
                        'BRKLSTP001-001
                        .Add(pfSet_Param("prmD77_TBOX_TYPE", SqlDbType.NVarChar, mfGetDBNull(mfGetTboxType()))) 'TBOXタイプ
                        .Add(pfSet_Param("prmIsReload", SqlDbType.NVarChar, mfGetDBNull(strReloadFlg))) 'リロードフラグ
                        'BRKLSTP001-001 END
                    End With

                    'ＳＱＬ実行
                    Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

                    '取得エラー？
                    If objDs Is Nothing OrElse objDs.Tables.Count <= 0 Then
                        Master.Master.ppLeftButton2.Enabled = False
                        psMesBox(Me, "00005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    End If

                    Dim objWkTbl As DataTable = Nothing

                    'BRKLSTP001-001
                    ''TBOXタイプ取得
                    'Dim strSysCd As String = mfGetTboxType()

                    'If strSysCd <> "" Then
                    '    'TBOXタイプの条件をかける
                    '    Dim objRow As DataRow() = objDs.Tables(0).Select("ＴＢＯＸタイプ IN (" & strSysCd & ") ")

                    '    objWkTbl = objDs.Tables(0).Clone()

                    '    For Each objWkRow As DataRow In objRow
                    '        objWkTbl.ImportRow(objWkRow)
                    '    Next

                    'Else
                    '    objWkTbl = objDs.Tables(0)
                    'End If
                    objWkTbl = objDs.Tables(0)
                    'BRKLSTP001-001 END

                    '該当データが存在するか
                    If objWkTbl.Rows.Count = 0 Then
                        Master.Master.ppLeftButton1.Enabled = False
                        Master.Master.ppLeftButton2.Enabled = False
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        Return
                    End If

                    'CSVファイルダウンロード
                    If pfDLCsvFile(fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv", objWkTbl, True, Me) <> 0 Then
                        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, fileName)
                    End If

                Catch ex As Threading.ThreadAbortException

                Catch ex As Exception
                    psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, fileName)
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
            Else
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub
    'BRKLSTP001-001
    ''' <summary>
    ''' ＣＳＶボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCSV_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim objCon As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim fileName As String = String.Empty
        objStack = New StackFrame

        'ログ出力開始
        psLogStart(Me)

        If Page.IsValid Then
            'ＤＢ接続
            If clsDataConnect.pfOpen_Database(objCon) Then
                Try
                    'ストアド設定
                    If sender.text = strBTN_CSV_NM Then
                        fileName = "ミニ処理票"
                        objCmd = New SqlCommand("BRKLSTP001_S13")
                    End If
                    objCmd.Connection = objCon
                    objCmd.CommandType = CommandType.StoredProcedure

                    Dim strReloadFlg As String
                    Select Case ViewState(IsReloadClick)
                        Case True
                            strReloadFlg = "1"
                        Case Else
                            strReloadFlg = "0"
                    End Select


                    'パラメータ設定
                    With objCmd.Parameters
                        .Add(pfSet_Param("prmD77_TBOXIDFm", SqlDbType.NVarChar, mfGetDBNull(txtTboxID.ppFromText)))             'TBOXID From
                        .Add(pfSet_Param("prmD77_TBOXIDTo", SqlDbType.NVarChar, mfGetDBNull(txtTboxID.ppToText)))               'TBOXID To
                        .Add(pfSet_Param("prmD77_RCPT_DTFm", SqlDbType.NVarChar, mfGetDBNull(txtUketukeDtFmTo.ppFromText)))     '受付日 From
                        .Add(pfSet_Param("prmD77_RCPT_DTTo", SqlDbType.NVarChar, mfGetDBNull(txtUketukeDtFmTo.ppToText)))       '受付日 To
                        .Add(pfSet_Param("prmD77_MNG_NOFm", SqlDbType.NVarChar, mfGetDBNull(txtKanriNo.ppFromText)))            '管理番号 From
                        .Add(pfSet_Param("prmD77_MNG_NOTo", SqlDbType.NVarChar, mfGetDBNull(txtKanriNo.ppToText)))              '管理番号 To
                        '.Add(pfSet_Param("prmD77_NL_CLS", SqlDbType.NVarChar, mfGetDBNull(txtNlKbn.ppText)))                    'NL区分
                        '.Add(pfSet_Param("prmD77_EW_CLS", SqlDbType.NVarChar, mfGetDBNull(txtEwKbn.ppText)))                    'EW区分
                        .Add(pfSet_Param("prmT01_STATE_CDFm", SqlDbType.NVarChar, mfGetDBNull(ddlPrefectureFm.SelectedValue)))  '都道府県 From
                        .Add(pfSet_Param("prmT01_STATE_CDTo", SqlDbType.NVarChar, mfGetDBNull(ddlPrefectureTo.SelectedValue)))  '都道府県 To
                        '.Add(pfSet_Param("prmD77_APPA_CLS", SqlDbType.NVarChar, mfGetDBNull(ddlAppaCls.ppSelectedValue)))       '機種区分
                        '.Add(pfSet_Param("prmD77_BLNG_CLS", SqlDbType.NVarChar, mfGetDBNull(ddlBlngCls.ppSelectedValue)))       '所属区分
                        .Add(pfSet_Param("prmD77_CALL_CLS", SqlDbType.NVarChar, mfGetDBNull(ddlCallCls.ppSelectedValue)))       'コール区分
                        .Add(pfSet_Param("prmD77_RCPT_CHARGE", SqlDbType.NVarChar, mfGetDBNull(txtUketukeNm.ppText)))           '受付者
                        '.Add(pfSet_Param("prmD77_STATUS_CDFm", SqlDbType.NVarChar, mfGetDBNull(ddlSagyoJokyoFm.SelectedValue))) '作業状況 From
                        '.Add(pfSet_Param("prmD77_STATUS_CDTo", SqlDbType.NVarChar, mfGetDBNull(ddlSagyoJokyoTo.SelectedValue))) '作業状況 To
                        .Add(pfSet_Param("prmD77_TBOX_VER", SqlDbType.NVarChar, mfGetDBNull(txtVersion.ppText)))                'Ｖｅｒ
                        .Add(pfSet_Param("prmD77_RPT_CD", SqlDbType.NVarChar, mfGetDBNull(txtShinsei.ppText)))                  '申告内容
                        .Add(pfSet_Param("prmD77_RPT_DTL", SqlDbType.NVarChar, mfGetDBNull(txtShinseiDtl.ppText)))              '申告内容詳細
                        .Add(pfSet_Param("prmD78_DEAL_DTL", SqlDbType.NVarChar, mfGetDBNull(txtTaioShinchokuJk.ppText)))        '対応進捗状況
                        .Add(pfSet_Param("prmD77_DEAL_CD", SqlDbType.NVarChar, mfGetDBNull(txtSyochi.ppText)))                  '処置内容(ドロップダウンリスト)
                        .Add(pfSet_Param("prmD77_DEAL_DTL", SqlDbType.NVarChar, mfGetDBNull(txtSyochiDtl.ppText)))              '処置内容詳細
                        .Add(pfSet_Param("prmD77_TBOX_TYPE", SqlDbType.NVarChar, mfGetDBNull(mfGetTboxType())))                                 'TBOXタイプ
                        .Add(pfSet_Param("prmIsReload", SqlDbType.NVarChar, mfGetDBNull(strReloadFlg))) 'リロードフラグ
                    End With

                    'ＳＱＬ実行
                    Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

                    '取得エラー？
                    If objDs Is Nothing OrElse objDs.Tables.Count <= 0 Then
                        Master.Master.ppLeftButton1.Enabled = False
                        psMesBox(Me, "00005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    End If

                    '該当データが存在するか
                    If objDs.Tables(0).Rows.Count = 0 Then
                        Master.Master.ppLeftButton1.Enabled = False
                        Master.Master.ppLeftButton2.Enabled = False
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        Return
                    End If

                    'CSVファイルダウンロード
                    If pfDLCsvFile(fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv", objDs.Tables(0), True, Me) <> 0 Then
                        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, fileName)
                    End If

                Catch ex As Exception
                    psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, fileName)
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Finally
                    'DB切断
                    If Not clsDataConnect.pfClose_Database(objCon) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                End Try
            Else
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub
    'BRKLSTP001-001 END
#End Region

#Region "■ Validation"

    ''' <summary>
    ''' 県Validator作動
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvPrefecture_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvPrefecture.ServerValidate

        '都道府県FROMTO
        If ddlPrefectureFm.SelectedIndex <> 0 AndAlso ddlPrefectureTo.SelectedIndex <> 0 Then
            If ddlPrefectureFm.SelectedValue > ddlPrefectureTo.SelectedValue Then
                cuvPrefecture.Text = pfGet_ValMes("2001", "都道府県").Item(P_VALMES_SMES)
                cuvPrefecture.ErrorMessage = pfGet_ValMes("2001", "都道府県").Item(P_VALMES_MES)
                cuvPrefecture.Visible = True
                args.IsValid = False

            Else
                cuvPrefecture.Text = ""
                cuvPrefecture.ErrorMessage = ""
                cuvPrefecture.Visible = False
                args.IsValid = True
            End If
        End If

    End Sub

    ''' <summary>
    ''' 作業状況Validator作動
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    'Protected Sub cuvddlSagyoJokyo_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvddlSagyoJokyo.ServerValidate

    '    '作業状況FROMTO
    '    If ddlSagyoJokyoFm.SelectedIndex <> 0 AndAlso ddlSagyoJokyoTo.SelectedIndex <> 0 Then

    '        If ddlSagyoJokyoFm.SelectedValue > ddlSagyoJokyoTo.SelectedValue Then
    '            cuvddlSagyoJokyo.Text = pfGet_ValMes("2001", "作業状況").Item(P_VALMES_SMES)
    '            cuvddlSagyoJokyo.ErrorMessage = pfGet_ValMes("2001", "作業状況").Item(P_VALMES_MES)
    '            cuvddlSagyoJokyo.Visible = True
    '            args.IsValid = False
    '        Else
    '            cuvddlSagyoJokyo.Text = ""
    '            cuvddlSagyoJokyo.ErrorMessage = ""
    '            cuvddlSagyoJokyo.Visible = False
    '            args.IsValid = True
    '        End If
    '    End If

    'End Sub

#End Region

#Region "■ 一覧の更新／参照／進捗画面ボタン押下処理"

    ''' <summary>
    ''' 一覧の更新／参照／進捗画面ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        Dim strExclusiveDate As String = Nothing
        Dim arTable_Name As New ArrayList
        Dim arKey As New ArrayList
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim strErrBtn As String = String.Empty

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            '参照、更新ボタンでないときは処理を抜ける
            If e.CommandName <> strSANSYO_BTN_NAME AndAlso e.CommandName <> strUPDATE_BTN_NAME Then
                Return
            End If

            Select Case e.CommandName
                Case strSANSYO_BTN_NAME
                    strErrBtn = "参照"
                Case strUPDATE_BTN_NAME
                    strErrBtn = "更新"
            End Select

            '開始ログ出力
            psLogStart(Me)

            Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    ' ボタン押下行のIndex
            Dim rowData As GridViewRow = grvList.Rows(intIndex)             ' ボタン押下行

            'セッション情報設定
            Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
            Session(P_SESSION_OLDDISP) = M_DISP_ID
            Session(P_SESSION_USERID) = ViewState(P_SESSION_USERID)         'ユーザＩＤ

            '受渡し用管理番号取得
            Session(P_KEY) = {CType(rowData.FindControl("管理番号"), TextBox).Text.ToString.Trim}

            '--------------------------------
            '2014/06/06 間瀬　ここから
            '--------------------------------
            '存在チェック
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    'ストアド設定
                    cmdDB = New SqlCommand("BRKUPDP001_S1", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    'パラメータ設定
                    With cmdDB.Parameters
                        .Add(pfSet_Param("prmD77_MNG_NO", SqlDbType.NVarChar, CType(rowData.FindControl("管理番号"), TextBox).Text.ToString.Trim)) 'ミニ管理番号
                    End With
                    Dim dstOrders As DataSet = Nothing
                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                    If dstOrders.Tables(0).Rows.Count <= 0 Then
                        psMesBox(Me, "30005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "選択したミニ処理票は、削除処理", strErrBtn)
                        Return
                    End If
                Catch ex As Exception
                    psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ミニ処理票画面の読み込み")
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Finally
                    'DB切断
                    If Not conDB Is Nothing Then
                        If Not clsDataConnect.pfClose_Database(conDB) Then
                            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        End If
                    End If
                End Try
            End If
            '--------------------------------
            '2014/06/06 間瀬　ここまで
            '--------------------------------


            Select Case e.CommandName
                Case strSANSYO_BTN_NAME     '参照
                    'モード設定
                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照

                    '--------------------------------
                    '2014/04/16 星野　ここから
                    '--------------------------------
                    '■□■□結合試験時のみ使用予定□■□■
                    Dim objStack As New StackFrame
                    Dim strPrm As String = ""
                    strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
                    Dim tmp As Object() = Session(P_KEY)
                    If Not tmp Is Nothing Then
                        For zz = 0 To tmp.Length - 1
                            If zz <> tmp.Length - 1 Then
                                strPrm &= tmp(zz).ToString & ","
                            Else
                                strPrm &= tmp(zz).ToString
                            End If
                        Next
                    End If

                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, strNEXTPAGE_PATH, strPrm, "TRANS")
                    '■□■□結合試験時のみ使用予定□■□■
                    '--------------------------------
                    '2014/04/16 星野　ここまで
                    '--------------------------------
                    'ミニ処理票表示(参照モード)
                    psOpen_Window(Me, strNEXTPAGE_PATH)

                Case strUPDATE_BTN_NAME        '更新

                    '★ロック対象テーブル名の登録
                    arTable_Name.Insert(0, "D77_MINI_MANAGE")

                    'ロックテーブルキー項目の登録.
                    arKey.Insert(0, CType(rowData.FindControl("管理番号"), TextBox).Text)

                    '★排他情報確認処理(更新処理の実行)
                    If clsExc.pfSel_Exclusive(strExclusiveDate _
                                     , Me _
                                     , Session(P_SESSION_IP) _
                                     , Session(P_SESSION_PLACE) _
                                     , Session(P_SESSION_USERID) _
                                     , Session(P_SESSION_SESSTION_ID) _
                                     , ViewState(P_SESSION_GROUP_NUM) _
                                     , M_UPD_DISP_ID _
                                     , arTable_Name _
                                     , arKey) = 0 Then

                        '★登録年月日時刻
                        Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate
                    Else
                        '排他ロック中
                        Exit Sub

                    End If

                    'モード設定
                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新

                    '--------------------------------
                    '2014/04/16 星野　ここから
                    '--------------------------------
                    '■□■□結合試験時のみ使用予定□■□■
                    Dim objStack As New StackFrame
                    Dim strPrm As String = ""
                    strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
                    Dim tmp As Object() = Session(P_KEY)
                    If Not tmp Is Nothing Then
                        For zz = 0 To tmp.Length - 1
                            If zz <> tmp.Length - 1 Then
                                strPrm &= tmp(zz).ToString & ","
                            Else
                                strPrm &= tmp(zz).ToString
                            End If
                        Next
                    End If

                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, strNEXTPAGE_PATH, strPrm, "TRANS")
                    '■□■□結合試験時のみ使用予定□■□■
                    '--------------------------------
                    '2014/04/16 星野　ここまで
                    '--------------------------------
                    'ミニ処理票表示(更新モード)
                    psOpen_Window(Me, strNEXTPAGE_PATH)

            End Select

        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ミニ処理票画面の読み込み")
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
    ''' データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound
        Dim strCancelCls As String = Nothing    'キャンセル区分

        For Each rowData As GridViewRow In grvList.Rows
            'グリッド内容取得
            strCancelCls = CType(rowData.FindControl("取消"), TextBox).Text

            If strCancelCls = "●" Then
                'キャンセル区分が１：取消の場合、更新ボタンを非活性にする
                rowData.Cells(1).Enabled = False
            End If

            If (rowData.RowIndex Mod 2) = 1 Then
                CType(rowData.FindControl("管理番号"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("ホール名"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("ＮＬ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("ＥＷ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("ＴＢＯＸタイプ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("コール区分"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("所属区分"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("機種区分"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("申告内容"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("都道府県"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("作業状況"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("受付日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("更新日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("取消"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
            Else
                CType(rowData.FindControl("管理番号"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("ホール名"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("ＮＬ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("ＥＷ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("ＴＢＯＸタイプ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("コール区分"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("所属区分"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("機種区分"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("申告内容"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("都道府県"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("作業状況"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("受付日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("更新日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("取消"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
            End If

            '文字色変更
            If CType(rowData.FindControl("作業状況"), TextBox).Text <> "" Then
                If "01" = CType(rowData.FindControl("作業状況"), TextBox).Text.Substring(0, 2) Then
                    CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).ForeColor = Drawing.Color.FromArgb(255, 255, 0, 0)
                    'If CType(rowData.FindControl("故障区分"), TextBox).Text > "0" Then
                    '    CType(rowData.FindControl("申告内容"), TextBox).ForeColor = Drawing.Color.FromArgb(255, 255, 0, 0)
                    'End If
                Else
                    If CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text.IndexOf("引継ぎ有り") > 0 Then
                        CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).ForeColor = Drawing.Color.FromArgb(255, 0, 128, 0)
                    End If
                    'If CType(rowData.FindControl("故障区分"), TextBox).Text > "0" Then
                    '    CType(rowData.FindControl("申告内容"), TextBox).ForeColor = Drawing.Color.FromArgb(255, 255, 0, 0)
                    'End If
                End If
            Else
                If CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text.IndexOf("引継ぎ有り") > 0 Then
                    CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).ForeColor = Drawing.Color.FromArgb(255, 0, 128, 0)
                End If
                'If CType(rowData.FindControl("故障区分"), TextBox).Text > "0" Then
                '    CType(rowData.FindControl("申告内容"), TextBox).ForeColor = Drawing.Color.FromArgb(255, 255, 0, 0)
                'End If
            End If
        Next
    End Sub

#End Region

#End Region

#Region "そのほかのプロシージャ"

#Region "■ ハンドラ付加"

    ''' <summary>
    ''' ボタン制御設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetBtnKmk()
        'BRKLSTP001-001
        'AddHandler Master.ppRightButton1.Click, AddressOf btnSearch_Click   '検索
        'AddHandler Master.ppRightButton2.Click, AddressOf btnClear_Click   'クリア
        'AddHandler Master.ppRightButton3.Click, AddressOf btnInsert_Click   '登録

        AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnSearch_Click   '検索
        AddHandler Master.Master.ppRigthButton2.Click, AddressOf btnClear_Click   'クリア
        AddHandler Master.Master.ppRigthButton3.Click, AddressOf btnInsert_Click   '登録

        AddHandler Master.Master.ppLeftButton1.Click, AddressOf btnCSV_Click  'CSV
        AddHandler Master.Master.ppLeftButton2.Click, AddressOf btnCSV_Dtl_Click  'ミニ処理票CSV

        '「登録」「検索条件クリア」ボタン押下時の検証を無効
        Master.Master.ppRigthButton2.CausesValidation = False
        Master.Master.ppRigthButton3.CausesValidation = False
        btnReload.CausesValidation = False

        '検索ボタンにValidationGroup設定
        Master.Master.ppRigthButton1.ValidationGroup = "Detail1"
        Master.Master.ppLeftButton1.ValidationGroup = "Detail1"
        Master.Master.ppLeftButton2.ValidationGroup = "Detail1"

        'ボタンの表示設定
        Master.Master.ppRigthButton1.Visible = True
        Master.Master.ppRigthButton2.Visible = True
        Master.Master.ppRigthButton3.Visible = True

        Master.Master.ppLeftButton1.Visible = True
        Master.Master.ppLeftButton2.Visible = True

        'ボタンの名称設定
        Master.Master.ppRigthButton1.Text = strBTN_SEARCH_NM
        Master.Master.ppRigthButton2.Text = strBTN_SEARCH_CLEAR_NM
        Master.Master.ppRigthButton3.Text = P_BTN_NM_ADD

        Master.Master.ppLeftButton1.Text = strBTN_CSV_NM
        Master.Master.ppLeftButton2.Text = strBTN_CSV_DTL_NM
        'BRKLSTP001-001 END

    End Sub

#End Region

#Region "■ ページ初期化"

    ''' <summary>
    ''' ページ初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfPageClear() As Boolean

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        mfPageClear = False

        Try
            'クリアボタンの処理
            msClearBtnMethod()

            '一覧クリア
            Master.ppCount = "0" '件数を初期設定
            grvList.DataSource = New DataTable()
            grvList.DataBind()

            mfPageClear = True

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
            mfPageClear = False
        End Try

    End Function

    ''' <summary>
    ''' クリアボタンの処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearBtnMethod()
        'テキスト項目クリア
        msTextKmkClear()

        'ドロップダウンリスト設定
        msSetDropDownList()

        'TBOXタイプ一覧設定
        'BRKLSTP001-001 
        msSetcklTboxClass()
        'If mfSetTboxType() = False Then
        '    Throw New Exception()
        'End If
        'BRKLSTP001-001 END
        '初期項目フォーカス設定
        If Me.IsPostBack = True Then
            txtTboxID.ppTextBoxFrom.Focus()
        End If
    End Sub

    ''' <summary>
    ''' テキスト項目クリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msTextKmkClear()
        txtTboxID.ppFromText = "" 'TBOXID From
        txtTboxID.ppToText = "" 'TBOXID To
        txtUketukeDtFmTo.ppFromText = "" '受付日 From
        txtUketukeDtFmTo.ppToText = "" '受付日 To
        txtKanriNo.ppFromText = "" '管理番号 From
        txtKanriNo.ppToText = "" '管理番号 To
        'txtNlKbn.ppText = "" 'NL区分
        'txtEwKbn.ppText = "" 'EW区分
        txtUketukeNm.ppText = "" '受付者
        txtVersion.ppText = "" 'Ｖｅｒ
        txtShinsei.ppText = "" '申請内容
        txtShinseiDtl.ppText = "" '申請内容詳細
        txtTaioShinchokuJk.ppText = "" '対応進捗状況
        txtSyochi.ppText = "" '処置内容
        txtSyochiDtl.ppText = "" '処置内容詳細
    End Sub

#End Region

#Region "■ ドロップダウンリスト設定"

    ''' <summary>
    ''' ドロップダウンリスト設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetDropDownList()
        '区分マスタを用いない各ドロップダウンリスト項目にデータ設定
        msSetDropDownList(ddlPrefectureFm, "BRKLSTP001_S11", "M12_STATE_NM", "M12_STATE_CD", "都道府県 From")        '都道府県 From        
        msSetDropDownList(ddlPrefectureTo, "BRKLSTP001_S11", "M12_STATE_NM", "M12_STATE_CD", "都道府県 To")          '都道府県 To        
        'msSetDropDownList(ddlSyotiDtl, "BRKLSTP001_S9", "M71_CONTENT", "M71_CODE", "処置内容")                 '処置内容
        'msSetDropDownList(ddlShinkokuDtl, "BRKLSTP001_S7", "M70_CONTENT", "M70_CODE", "申告内容")              '申告内容
        'msSetDropDownList(ddlSagyoJokyoFm, "BRKLSTP001_S8", "M27_STATUS_NM", "M27_STATUS_CD", "作業状況 From") '作業状況 From
        'msSetDropDownList(ddlSagyoJokyoTo, "BRKLSTP001_S8", "M27_STATUS_NM", "M27_STATUS_CD", "作業状況 To")   '作業状況 To

        '各ドロップダウンリストを未選択に
        msNotSelectDdl()

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定
    ''' </summary>
    ''' <param name="ddlCtrl">ドロップダウンリスト</param>
    ''' <param name="strStoredNm">ストアド名称</param>
    ''' <param name="strDataTxtKmkNm">DB表示項目名</param>
    ''' <param name="strValKmkNm">DB値項目名</param>
    ''' <param name="strMstNm">マスタ名称(エラーメッセージ用)</param>
    ''' <remarks></remarks>
    Private Sub msSetDropDownList(ByRef ddlCtrl As DropDownList, ByVal strStoredNm As String, ByVal strDataTxtKmkNm As String, ByVal strValKmkNm As String, ByVal strMstNm As String)

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
        If clsDataConnect.pfOpen_Database(objCn) Then
            Try
                objCmd = New SqlCommand(strStoredNm, objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                ddlCtrl.Items.Clear()
                ddlCtrl.DataSource = objDs.Tables(0)
                ddlCtrl.DataTextField = strDataTxtKmkNm
                ddlCtrl.DataValueField = strValKmkNm
                ddlCtrl.DataBind()
                ddlCtrl.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加
                ddlCtrl.SelectedIndex = 0

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMstNm & "マスタ一覧取得")
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

    End Sub

    ''' <summary>
    ''' ドロップダウンリストを未選択に設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msNotSelectDdl()
        '各ドロップダウンリストを未選択に

        ddlCallCls.ppDropDownList.SelectedIndex = 0
        'ddlBlngCls.ppDropDownList.SelectedIndex = 0
        'ddlAppaCls.ppDropDownList.SelectedIndex = 0

        'ddlSagyoJokyoFm.SelectedIndex = 0
        'ddlSagyoJokyoTo.SelectedIndex = 0
        ddlPrefectureFm.SelectedIndex = 0
        ddlPrefectureTo.SelectedIndex = 0

    End Sub

#End Region

    'BRKLSTP001-001 未使用なのでコメントアウト
#Region "■ TBOXタイプ一覧設定"
    ' <summary>
    ' TBOXタイプ設定
    ' </summary>
    ' <remarks></remarks>
    'Private Function mfSetTboxType() As Boolean
    '    Dim objCmd As SqlCommand = Nothing
    '    Dim objCon As SqlConnection = Nothing
    '    '--------------------------------
    '    '2014/04/14 星野　ここから
    '    '--------------------------------
    '    objStack = New StackFrame
    '    '--------------------------------
    '    '2014/04/14 星野　ここまで
    '    '--------------------------------

    '    mfSetTboxType = False

    '    '一覧初期化
    '    grvListTboxcls.DataSource = New DataTable()
    '    grvListTboxcls.DataBind()

    '    'DB接続
    '    If clsDataConnect.pfOpen_Database(objCon) Then
    '        Try
    '            'ストアド設定
    '            objCmd = New SqlCommand("BRKLSTP001_S10")
    '            objCmd.Connection = objCon
    '            objCmd.CommandType = CommandType.StoredProcedure

    '            'SQL実行
    '            Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

    '            '取得エラー
    '            If objDs Is Nothing OrElse objDs.Tables.Count <= 0 Then
    '                mfSetTboxType = False
    '                Exit Function
    '            End If

    '            'データ設定
    '            grvListTboxcls.DataSource = objDs.Tables(0)
    '            grvListTboxcls.DataBind()

    '            mfSetTboxType = True

    '        Catch ex As Exception
    '            '--------------------------------
    '            '2014/04/14 星野　ここから
    '            '--------------------------------
    '            'ログ出力
    '            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
    '                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
    '            '--------------------------------
    '            '2014/04/14 星野　ここまで
    '            '--------------------------------
    '            mfSetTboxType = False
    '        Finally
    '            'DB切断
    '            If Not clsDataConnect.pfClose_Database(objCon) Then
    '                mfSetTboxType = False
    '            End If
    '        End Try
    '    Else
    '        mfSetTboxType = False
    '    End If

    'End Function
#End Region
    'BRKLSTP001-001 END

#Region "■ 検索"

    ''' <summary>
    ''' 検索処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data(ByVal SchCd As String, Optional ByVal blnIsReloadFlg As Boolean = False)
        Dim objCon As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'ＤＢ接続
        If clsDataConnect.pfOpen_Database(objCon) Then
            Try
                'ストアド設定
                objCmd = New SqlCommand("BRKLSTP001_S2")
                objCmd.Connection = objCon
                objCmd.CommandType = CommandType.StoredProcedure

                'パラメータ設定
                If blnIsReloadFlg = True Then
                    With objCmd.Parameters
                        .Add(pfSet_Param("prmD77_TBOXIDFm", SqlDbType.NVarChar, DBNull.Value)) 'TBOXID From
                        .Add(pfSet_Param("prmD77_TBOXIDTo", SqlDbType.NVarChar, DBNull.Value)) ' TBOXID To
                        .Add(pfSet_Param("prmD77_RCPT_DTFm", SqlDbType.NVarChar, DBNull.Value)) '受付日 From
                        .Add(pfSet_Param("prmD77_RCPT_DTTo", SqlDbType.NVarChar, DBNull.Value)) '受付日 To
                        .Add(pfSet_Param("prmD77_MNG_NOFm", SqlDbType.NVarChar, DBNull.Value)) '管理番号 From
                        .Add(pfSet_Param("prmD77_MNG_NOTo", SqlDbType.NVarChar, DBNull.Value)) '管理番号 To
                        .Add(pfSet_Param("prmD77_NL_CLS", SqlDbType.NVarChar, DBNull.Value)) 'NL区分
                        .Add(pfSet_Param("prmD77_EW_CLS", SqlDbType.NVarChar, DBNull.Value)) 'EW区分
                        .Add(pfSet_Param("prmT01_STATE_CDFm", SqlDbType.NVarChar, DBNull.Value)) '都道府県 From
                        .Add(pfSet_Param("prmT01_STATE_CDTo", SqlDbType.NVarChar, DBNull.Value)) '都道府県 To
                        .Add(pfSet_Param("prmD77_RPT_CD", SqlDbType.NVarChar, DBNull.Value)) '申告内容
                        .Add(pfSet_Param("prmD77_APPA_CLS", SqlDbType.NVarChar, DBNull.Value)) '機種区分
                        .Add(pfSet_Param("prmD77_BLNG_CLS", SqlDbType.NVarChar, DBNull.Value)) '所属区分
                        .Add(pfSet_Param("prmD77_CALL_CLS", SqlDbType.NVarChar, DBNull.Value)) 'コール区分
                        .Add(pfSet_Param("prmD77_RCPT_CHARGE", SqlDbType.NVarChar, DBNull.Value)) '受付者
                        .Add(pfSet_Param("prmD77_STATUS_CDFm", SqlDbType.NVarChar, DBNull.Value)) '作業状況 From
                        .Add(pfSet_Param("prmD77_STATUS_CDTo", SqlDbType.NVarChar, DBNull.Value)) '作業状況 To
                        .Add(pfSet_Param("prmD77_TBOX_VER", SqlDbType.NVarChar, DBNull.Value)) 'Ｖｅｒ
                        .Add(pfSet_Param("prmD77_RPT_DTL", SqlDbType.NVarChar, DBNull.Value)) '申告内容
                        .Add(pfSet_Param("prmD78_DEAL_DTL", SqlDbType.NVarChar, DBNull.Value)) '対応進捗状況
                        .Add(pfSet_Param("prmD77_DEAL_CD", SqlDbType.NVarChar, DBNull.Value)) '処置内容(ドロップダウンリスト)
                        .Add(pfSet_Param("prmD77_DEAL_DTL", SqlDbType.NVarChar, DBNull.Value)) '処置内容詳細
                        'BRKLSTP001-001
                        .Add(pfSet_Param("prmD77_TBOX_TYPE", SqlDbType.NVarChar, DBNull.Value)) 'TBOXタイプ
                        'BRKLSTP001-001 END
                        .Add(pfSet_Param("search_cls", SqlDbType.NVarChar, SchCd)) '初期処理区分
                    End With
                Else
                    With objCmd.Parameters
                        .Add(pfSet_Param("prmD77_TBOXIDFm", SqlDbType.NVarChar, mfGetDBNull(txtTboxID.ppFromText))) 'TBOXID From
                        .Add(pfSet_Param("prmD77_TBOXIDTo", SqlDbType.NVarChar, mfGetDBNull(txtTboxID.ppToText))) ' TBOXID To
                        .Add(pfSet_Param("prmD77_RCPT_DTFm", SqlDbType.NVarChar, mfGetDBNull(txtUketukeDtFmTo.ppFromText))) '受付日 From
                        .Add(pfSet_Param("prmD77_RCPT_DTTo", SqlDbType.NVarChar, mfGetDBNull(txtUketukeDtFmTo.ppToText))) '受付日 To
                        .Add(pfSet_Param("prmD77_MNG_NOFm", SqlDbType.NVarChar, mfGetDBNull(txtKanriNo.ppFromText))) '管理番号 From
                        .Add(pfSet_Param("prmD77_MNG_NOTo", SqlDbType.NVarChar, mfGetDBNull(txtKanriNo.ppToText))) '管理番号 To
                        '.Add(pfSet_Param("prmD77_NL_CLS", SqlDbType.NVarChar, mfGetDBNull(txtNlKbn.ppText))) 'NL区分
                        '.Add(pfSet_Param("prmD77_EW_CLS", SqlDbType.NVarChar, mfGetDBNull(txtEwKbn.ppText))) 'EW区分
                        .Add(pfSet_Param("prmT01_STATE_CDFm", SqlDbType.NVarChar, mfGetDBNull(ddlPrefectureFm.SelectedValue))) '都道府県 From
                        .Add(pfSet_Param("prmT01_STATE_CDTo", SqlDbType.NVarChar, mfGetDBNull(ddlPrefectureTo.SelectedValue))) '都道府県 To
                        '.Add(pfSet_Param("prmD77_APPA_CLS", SqlDbType.NVarChar, mfGetDBNull(ddlAppaCls.ppSelectedValue))) '機種区分
                        '.Add(pfSet_Param("prmD77_BLNG_CLS", SqlDbType.NVarChar, mfGetDBNull(ddlBlngCls.ppSelectedValue))) '所属区分
                        .Add(pfSet_Param("prmD77_CALL_CLS", SqlDbType.NVarChar, mfGetDBNull(ddlCallCls.ppSelectedValue))) 'コール区分
                        .Add(pfSet_Param("prmD77_RCPT_CHARGE", SqlDbType.NVarChar, mfGetDBNull(txtUketukeNm.ppText))) '受付者
                        '.Add(pfSet_Param("prmD77_STATUS_CDFm", SqlDbType.NVarChar, mfGetDBNull(ddlSagyoJokyoFm.SelectedValue))) '作業状況 From
                        '.Add(pfSet_Param("prmD77_STATUS_CDTo", SqlDbType.NVarChar, mfGetDBNull(ddlSagyoJokyoTo.SelectedValue))) '作業状況 To
                        .Add(pfSet_Param("prmD77_TBOX_VER", SqlDbType.NVarChar, mfGetDBNull(txtVersion.ppText))) 'Ｖｅｒ
                        .Add(pfSet_Param("prmD77_RPT_CD", SqlDbType.NVarChar, mfGetDBNull(txtShinsei.ppText))) '申告内容
                        .Add(pfSet_Param("prmD77_RPT_DTL", SqlDbType.NVarChar, mfGetDBNull(txtShinseiDtl.ppText))) '申告内容詳細
                        .Add(pfSet_Param("prmD78_DEAL_DTL", SqlDbType.NVarChar, mfGetDBNull(txtTaioShinchokuJk.ppText))) '対応進捗状況
                        .Add(pfSet_Param("prmD77_DEAL_CD", SqlDbType.NVarChar, mfGetDBNull(txtSyochi.ppText))) '処置内容(ドロップダウンリスト)
                        .Add(pfSet_Param("prmD77_DEAL_DTL", SqlDbType.NVarChar, mfGetDBNull(txtSyochiDtl.ppText))) '処置内容詳細
                        'BRKLSTP001-001
                        .Add(pfSet_Param("prmD77_TBOX_TYPE", SqlDbType.NVarChar, mfGetDBNull(mfGetTboxType()))) '処置内容詳細
                        'BRKLSTP001-001 END
                        .Add(pfSet_Param("search_cls", SqlDbType.NVarChar, SchCd)) '初期処理区分
                    End With
                End If


                'ＳＱＬ実行
                Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

                '取得エラー？
                If objDs Is Nothing OrElse objDs.Tables.Count <= 0 Then
                    Master.Master.ppLeftButton1.Enabled = False
                    Throw New Exception
                End If

                Dim objWkTbl As DataTable = Nothing

                'TBOXタイプ取得
                'Dim strSysCd As String = mfGetTboxType()

                'If strSysCd <> "" Then
                '    'TBOXタイプの条件をかける
                '    Dim objRow As DataRow() = objDs.Tables(0).Select("ＴＢＯＸタイプＣＤ IN (" & strSysCd & ") ")

                '    objWkTbl = objDs.Tables(0).Clone()

                '    For Each objWkRow As DataRow In objRow
                '        objWkTbl.ImportRow(objWkRow)
                '    Next

                'Else
                '    objWkTbl = objDs.Tables(0)
                'End If
                objWkTbl = objDs.Tables(0)
                '該当データが存在するか
                If objWkTbl.Rows.Count = 0 Then
                    Master.Master.ppLeftButton1.Enabled = False
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                End If

                Master.ppCount = objWkTbl.Rows.Count
                Master.ppCount_Visible = False
                lblCount.Text = objWkTbl.Rows.Count

                '閾値の制御
                objWkTbl = mfSetShikiichi(objWkTbl)

                '不要な行の削除
                objWkTbl.Columns.Remove("ＴＢＯＸタイプＣＤ")

                'グリッドにデータ設定
                grvList.DataSource = objWkTbl
                grvList.DataBind()

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ミニ処理票データ")
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
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Sub

#End Region

#Region "■ DBNULL取得"

    ''' <summary>
    ''' DBNull取得
    ''' </summary>
    ''' <param name="strVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDBNull(ByVal strVal As String) As Object
        If strVal Is Nothing OrElse strVal.Trim() = "" Then
            Return DBNull.Value
        Else
            Return strVal.Trim()
        End If

    End Function

#End Region

#Region "■ TBOXタイプ取得"

    ''' <summary>
    ''' TBOXタイプの取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetTboxType() As String
        'Dim strVal As String = ""
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            '一覧のチェックがついている行について、コードを取得

            'BRKLSTP001-001 

            Dim strTemp As String = String.Empty
            '許容バージョンを配列に挿入
            For i As Integer = 0 To cklTboxClass.Items.Count - 1
                If cklTboxClass.Items(i).Selected Then
                    strTemp &= "," + cklTboxClass.Items(i).Value
                End If
            Next
            Return strTemp.TrimStart(",")

            'For Each objRow As GridViewRow In grvListTboxcls.Rows
            '    If CType(objRow.Cells(2).Controls(0), CheckBox).Checked Then
            '        strVal &= "'" & CType(objRow.Cells(0).Controls(0), TextBox).Text & "',"
            '    End If
            'Next

            'If strVal.Trim = "" Then
            '    Return ""
            'Else
            '    '最後のカンマを除去
            '    strVal = strVal.Trim.Substring(0, strVal.Trim.Length - 1)
            'End If

            'Return strVal
            'BRKLSTP001-001 END
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
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' ■　チェックボックスの生成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetcklTboxClass()
        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        Dim intCkl As Integer = 0
        Dim strErrMsg As String = "TBOXﾀｲﾌﾟ"
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                strErrMsg = "TBOXﾀｲﾌﾟ"
                objCmd = New SqlCommand("ZCMPSEL038", objCn)
                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                ''Me.cklTboxClass.Items.Clear()
                ''Me.cklTboxClass.DataSource = objDs.Tables(0)
                ''Me.cklTboxClass.DataTextField = "ＴＢＯＸシステム"
                ''Me.cklTboxClass.DataValueField = "ＴＢＯＸシステムコード"
                ''Me.cklTboxClass.DataBind()

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strErrMsg & "一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If
    End Sub
#End Region

#Region "■ 閾値制御"

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
            If objTbl.Rows.Count > objTbl.Rows(0)("閾値") Then

                '行を最大件数に絞込み
                For i As Integer = 0 To objTbl.Rows(0)("閾値") - 1
                    objWkTbl.ImportRow(objTbl.Rows(i))
                Next

                '閾値超過をメッセで知らせる
                psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, objTbl.Rows.Count, objTbl.Rows(0)("閾値"))

            Else
                objWkTbl = objTbl
            End If
        Else
            objWkTbl = objTbl
        End If

        objWkTbl.Columns.Remove("閾値")

        Return objWkTbl

    End Function

#End Region

    Protected Sub btnReload_Click(sender As Object, e As EventArgs) Handles btnReload.Click
        'ページクリア
        'mfPageClear()

        '条件検索取得
        msGet_Data("1", True)

        ViewState(IsReloadClick) = True
    End Sub

#End Region

End Class
