'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜監視＞
'*　処理名　　：　監視報告書兼依頼書一覧
'*　ＰＧＭＩＤ：　WATLSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.06　：　ＸＸＸ
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive      '排他制御用

#End Region

Public Class WATLSTP001
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
    Const M_MY_DISP_ID = P_FUN_WAT & P_SCR_LST & P_PAGE & "001"
    Const M_UPD_DISP_ID = P_FUN_WAT & P_SCR_UPD & P_PAGE & "001"
    '次画面ファイルパス
    Const M_NEXT_DISP_PATH = "~/" & P_WAT & "/" &
            P_FUN_WAT & P_SCR_UPD & P_PAGE & "001" & "/" &
            P_FUN_WAT & P_SCR_UPD & P_PAGE & "001.aspx"

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
    Dim strAdd As String = ConfigurationManager.AppSettings("Address")
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
        AddHandler Master.ppRigthButton3.Click, AddressOf btnNew_Click
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            If Not IsPostBack Then  '初回表示のみ

                Master.ppRigthButton3.Visible = True
                Master.ppRigthButton3.Text = P_BTN_NM_ADD

                '接続場所がＳＰＣの場合のみ登録ボタンを活性化
                If strAdd <> P_ADD_SPC Then
                    Master.ppRigthButton3.Enabled = False
                End If

                '検索条件クリアボタンと新規作成ボタン押下時の検証を無効
                Master.ppRigthButton2.CausesValidation = False
                Master.ppRigthButton3.CausesValidation = False

                '画面設定
                Master.Master.ppProgramID = M_MY_DISP_ID
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)
                Master.Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる

                'パンくずリスト設定
                Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                'セッション変数「グループナンバー」が存在しない場合、画面を閉じる
                If Session(P_SESSION_GROUP_NUM) Is Nothing Then
                    psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    psClose_Window(Me)
                    Return
                End If

                'ViewStateに「グループナンバー」を保存
                ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                '状況ドロップダウンリスト設定
                msSet_ddlStatus()           '進捗状況
                msSet_ddlDealStatus()       'ＮＧＣ対応状況

                '画面クリア
                msClear_Screen()

                'データ取得
                msGet_Data("1")

            Else
                'ViewState「グループナンバー」が存在しない場合、画面を閉じる
                If ViewState(P_SESSION_GROUP_NUM) Is Nothing Then
                    psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
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
    '2014/04/18 武 ここから
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
        End Select

    End Sub
    '---------------------------
    '2014/04/18 武 ここまで
    '---------------------------

    ''' <summary>
    ''' 検索ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)
        '開始ログ出力
        psLogStart(Me)

        'データ取得
        If (Page.IsValid) Then
            msGet_Data("0")
        End If

        '終了ログ出力
        psLogEnd(Me)

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
        msClear_Screen()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 登録ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnNew_Click(sender As Object, e As EventArgs)
        Dim strList As List(Of String) = Nothing                        '次画面引継ぎ用キー情報

        '開始ログ出力
        psLogStart(Me)

        '次画面引継ぎ用キー情報設定
        strList = New List(Of String)
        strList.Add(String.Empty)

        'セッション情報設定																	
        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録
        Session(P_KEY) = strList.ToArray
        Session(P_SESSION_OLDDISP) = M_MY_DISP_ID
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

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
                        objStack.GetMethod.Name, M_NEXT_DISP_PATH, strPrm, "TRANS")
        '■□■□結合試験時のみ使用予定□■□■
        '--------------------------------
        '2014/04/16 星野　ここまで
        '--------------------------------
        '監視報告書兼依頼表参照／更新　画面起動
        psOpen_Window(Me, M_NEXT_DISP_PATH)

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
        Dim intIndex As Integer = Nothing
        Dim rowData As GridViewRow = Nothing
        Dim strList As List(Of String) = Nothing                        '次画面引継ぎ用キー情報

        If e.CommandName <> "btnReference" And e.CommandName <> "btnUpdate" Then
            Exit Sub
        End If

        '開始ログ出力
        psLogStart(Me)

        intIndex = Convert.ToInt32(e.CommandArgument)       'ボタン押下行のIndex
        rowData = grvList.Rows(intIndex)                    'ボタン押下行

        '次画面引継ぎ用キー情報設定
        strList = New List(Of String)
        strList.Add(CType(rowData.FindControl("管理番号"), TextBox).Text)

        'セッション情報設定																	
        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
        Session(P_KEY) = strList.ToArray
        Session(P_SESSION_OLDDISP) = M_MY_DISP_ID
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

        Select Case e.CommandName
            Case "btnReference" '参照
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
            Case "btnUpdate"    '更新

                '排他制御用変数
                Dim strExclusiveDate As String = Nothing
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList

                'ロック対象テーブル名の登録
                arTable_Name.Insert(0, "D36_MNTRREPORT")

                'ロックテーブルキー項目の登録
                arKey.Insert(0, CType(rowData.FindControl("管理番号"), TextBox).Text)

                '排他情報確認処理
                If clsExc.pfSel_Exclusive(strExclusiveDate,
                                   Me,
                                   Session(P_SESSION_IP),
                                   Session(P_SESSION_PLACE),
                                   Session(P_SESSION_USERID),
                                   Session(P_SESSION_SESSTION_ID),
                                   ViewState(P_SESSION_GROUP_NUM),
                                   M_UPD_DISP_ID,
                                   arTable_Name,
                                   arKey) = 0 Then

                    'セッション情報設定
                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
                    Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate

                Else
                    '排他ロック中
                    Exit Sub

                End If

        End Select

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
                        objStack.GetMethod.Name, M_NEXT_DISP_PATH, strPrm, "TRANS")
        '■□■□結合試験時のみ使用予定□■□■
        '--------------------------------
        '2014/04/16 星野　ここまで
        '--------------------------------
        '監視報告書兼依頼表参照／更新　画面起動
        psOpen_Window(Me, M_NEXT_DISP_PATH)

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
        '削除フラグがある行の更新ボタンを非活性にする
        Dim strDelF As String
        For Each rowData As GridViewRow In grvList.Rows
            strDelF = CType(rowData.FindControl("削除フラグ"), TextBox).Text
            If strDelF = "●" Then   '削除フラグあり
                rowData.Cells(1).Enabled = False
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

        Me.tftTboxId.ppFromText = String.Empty          'ＴＢＯＸＩＤ From
        Me.tftTboxId.ppToText = String.Empty            'ＴＢＯＸＩＤ To
        Me.tftMntrReportNo.ppFromText = String.Empty    '管理番号 From
        Me.tftMntrReportNo.ppToText = String.Empty      '管理番号 To
        Me.dftOBreakD.ppFromText = String.Empty         '発生日 From
        Me.dftOBreakD.ppToText = String.Empty           '発生日 To
        Me.cbxMntrEnd.Checked = False                   '案件終了
        Me.ddlHpnStts.ppSelectedValue = String.Empty    '発生状況
        Me.ddlStatus.SelectedValue = String.Empty       '進捗状況
        Me.ddlDealStatus.SelectedValue = String.Empty   'ＮＧＣ対応状況
        Me.grvList.DataSource = New DataTable
        Master.ppCount = "0"
        Me.grvList.DataBind()
        Me.tftTboxId.ppTextBoxFrom.Focus()

    End Sub

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <param name="ipstrDefault">初期表示フラグ（1：初期　1以外：初期以外）</param>
    ''' <remarks></remarks>
    Private Sub msGet_Data(ByVal ipstrDefault As String)
        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット
        Dim strMntrEnd As String
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '値取得
        Select Case Me.cbxMntrEnd.Checked
            Case False
                strMntrEnd = "0"
            Case Else
                strMntrEnd = String.Empty
        End Select

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_S1", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    'ＴＢＯＸＩＤFrom
                    .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, Me.tftTboxId.ppFromText))
                    'ＴＢＯＸＩＤTo
                    .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, Me.tftTboxId.ppToText))
                    '管理番号From
                    .Add(pfSet_Param("mntrreport_no_f", SqlDbType.NVarChar, Me.tftMntrReportNo.ppFromText))
                    '管理番号To
                    .Add(pfSet_Param("mntrreport_no_t", SqlDbType.NVarChar, Me.tftMntrReportNo.ppToText))
                    '発生日From
                    .Add(pfSet_Param("obreak_d_f", SqlDbType.NVarChar, Me.dftOBreakD.ppFromText))
                    '発生日To
                    .Add(pfSet_Param("obreak_d_t", SqlDbType.NVarChar, Me.dftOBreakD.ppToText))
                    '案件終了
                    .Add(pfSet_Param("mntr_end_flg", SqlDbType.NVarChar, strMntrEnd))
                    '発生状況
                    .Add(pfSet_Param("hpnstts_cd", SqlDbType.NVarChar, Me.ddlHpnStts.ppSelectedValue))
                    '進捗状況
                    .Add(pfSet_Param("status_cd", SqlDbType.NVarChar, Me.ddlStatus.SelectedValue))
                    'ＮＧＣ対応状況
                    .Add(pfSet_Param("deal_status", SqlDbType.NVarChar, Me.ddlDealStatus.SelectedValue))
                    'ユーザＩＤ
                    .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))
                    '画面ＩＤ
                    .Add(pfSet_Param("disp_id", SqlDbType.NVarChar, M_MY_DISP_ID))
                    '初期表示フラグ
                    .Add(pfSet_Param("default", SqlDbType.NVarChar, ipstrDefault))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ有無確認
                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    Master.ppCount = dstOrders.Tables(0).Rows.Count.ToString
                Else
                    '閾値を超えた場合はメッセージを表示
                    If dstOrders.Tables(0).Rows(0)("データ件数") > dstOrders.Tables(0).Rows.Count Then
                        psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                            dstOrders.Tables(0).Rows(0)("データ件数").ToString, dstOrders.Tables(0).Rows.Count.ToString)
                    End If
                    Master.ppCount = dstOrders.Tables(0).Rows(0)("データ件数").ToString
                End If

                '取得したデータをグリッドに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "監視報告書兼依頼票一覧")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()
                Master.ppCount = "0"
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"
        End If
    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（進捗状況）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_ddlStatus()
        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("ZCMPSEL015", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("duty_cd", SqlDbType.NVarChar, "35A"))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'ドロップダウンリスト設定
                Me.ddlStatus.Items.Clear()
                Me.ddlStatus.DataSource = dstOrders.Tables(0)
                Me.ddlStatus.DataTextField = "進捗ステータス名"
                Me.ddlStatus.DataValueField = "進捗ステータス"
                Me.ddlStatus.DataBind()
                Me.ddlStatus.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "進捗状況一覧")
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
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If
    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（ＮＧＣ対応状況）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_ddlDealStatus()
        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("ZCMPSEL015", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("duty_cd", SqlDbType.NVarChar, "35B"))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'ドロップダウンリスト設定
                Me.ddlDealStatus.Items.Clear()
                Me.ddlDealStatus.DataSource = dstOrders.Tables(0)
                Me.ddlDealStatus.DataTextField = "進捗ステータス名"
                Me.ddlDealStatus.DataValueField = "進捗ステータス"
                Me.ddlDealStatus.DataBind()
                Me.ddlDealStatus.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＮＧＣ対応状況一覧")
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
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If
    End Sub
#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
