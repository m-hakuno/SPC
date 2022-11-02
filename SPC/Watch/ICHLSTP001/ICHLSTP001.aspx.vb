'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜監視＞
'*　処理名　　：　ICカード履歴調査一覧
'*　ＰＧＭＩＤ：　ICHLSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.01.21　：　ＮＫＣ
'*  更　新　　：　2014.07.03　：　間瀬      NL区分でNを指定しているものに対してJも参照するように変更
'********************************************************************************************************************************

#Region "インポート定義"

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax

#End Region


Public Class ICHLSTP001
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
    Private Const M_DISP_ID = P_FUN_ICH & P_SCR_LST & P_PAGE & "001"

    '次画面ファイルパス
    Const M_NEXT_DISP_PATH = "~/" & P_WAT & "/" &
            P_FUN_ICH & P_SCR_LST & P_PAGE & "002" & "/" &
            P_FUN_ICH & P_SCR_LST & P_PAGE & "002.aspx"

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

#End Region

#Region "プロパティ定義"

    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================

#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page_Init
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        '表設定
        pfSet_GridView_S_Off(Me.grvList, M_DISP_ID)

    End Sub

    ''' <summary>
    ''' Page_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btn_Click     '更新
        AddHandler Master.ppRigthButton2.Click, AddressOf btn_Click     '照会
        AddHandler Master.ppRigthButton3.Click, AddressOf btn_Click     '照会条件クリア
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        If Not IsPostBack Then  '初回表示

            Try
                '開始ログ出力
                psLogStart(Me)

                'プログラムＩＤ、画面名設定
                Master.Master.ppProgramID = M_DISP_ID
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

                'パンくずリスト設定
                Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                '画面初期化処理
                msClearScreen()

                'セッション変数の取得/明細情報検索
                If Master.Master.ppBcList_Text = String.Empty Then

                    'システムエラー
                    psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    '処理終了(画面終了)
                    psClose_Window(Me)
                    Exit Sub

                End If

                '一覧の初期表示
                me_GetICRireki()

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

        End If

    End Sub

#End Region

#Region "ユーザー権限"
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
#End Region

#Region "ボタンクリックイベント"

    ''' <summary>
    ''' ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btn_Click(sender As Object, e As EventArgs)

        Dim Result As Boolean
        Dim dts As DataTable = pfParse_DataTable(Me.grvList)
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

            Select Case sender.text

                Case "照会条件クリア"

                    'コントロールの初期化
                    Me.txtTboxID.ppText = Nothing
                    Me.txtDataDT.ppText = Nothing

                    'グリッドの再描画
                    Me.grvList.DataSource = dts
                    Me.grvList.DataBind()

                Case "照会"

                    Try

                        If (Page.IsValid) Then

                            'TBOXIDの存在確認_データ日付チェック
                            ms_ChkTboxid(Result)

                            If Result Then

                                '照会中情報の確認
                                ms_GetICSyoukai(Result)

                                If Result Then

                                    '照会データの作成
                                    ms_InsICdata(True, Nothing, 1, "照会依頼開始")
                                Else

                                    '処理終了
                                    Throw New Exception

                                End If

                            Else

                                '処理終了
                                Throw New Exception

                            End If

                            '画面の更新
                            me_GetICRireki()

                        End If

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

                        'グリッドの再描画
                        Me.grvList.DataSource = dts
                        Me.grvList.DataBind()

                        '処理終了
                        Exit Sub

                    End Try

                Case "更新"

                    '一覧の更新
                    me_GetICRireki()

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
            '処理終了
            Exit Sub

        Finally

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub

    ''' <summary>
    ''' グリッドボタン操作
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    'ボタン押下行のIndex
        Dim rowData As GridViewRow = grvList.Rows(intIndex)             'ボタン押下行
        Dim result As Boolean = False
        Dim strKeyList As List(Of String) = Nothing                     '次画面引継ぎ用キー情報

        Dim dts As DataTable = pfParse_DataTable(Me.grvList)

        'グリッドの再描画
        Me.grvList.DataSource = dts
        Me.grvList.DataBind()

        Select Case e.CommandName

            Case "btnDetail"     '詳細ボタン押下

                '開始ログ出力
                psLogStart(Me)

                '次画面引継ぎ用キー情報設定
                strKeyList = New List(Of String)
                strKeyList.Add(CType(rowData.FindControl("要求通番"), TextBox).Text)
                strKeyList.Add(CType(rowData.FindControl("要求日付"), TextBox).Text)
                strKeyList.Add(CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text)
                strKeyList.Add(CType(rowData.FindControl("ＮＬ区分"), TextBox).Text)
                strKeyList.Add(CType(rowData.FindControl("ホール名"), TextBox).Text)
                strKeyList.Add(CType(rowData.FindControl("ホールコード"), TextBox).Text)
                strKeyList.Add(CType(rowData.FindControl("ＶＥＲ"), TextBox).Text)
                strKeyList.Add(CType(rowData.FindControl("システム"), TextBox).Text)
                strKeyList.Add(CType(rowData.FindControl("システムコード"), TextBox).Text)
                strKeyList.Add(CType(rowData.FindControl("システム分類"), TextBox).Text)
                strKeyList.Add(CType(rowData.FindControl("データ日付_キー"), TextBox).Text)
                strKeyList.Add(CType(rowData.FindControl("管理番号"), TextBox).Text)
                strKeyList.Add(CType(rowData.FindControl("運用監視サーバ運用日付"), TextBox).Text)

                'セッション情報設定																	
                Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
                Session(P_KEY) = strKeyList.ToArray
                'Session(P_KEY) = strKeyList
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
                                objStack.GetMethod.Name, M_NEXT_DISP_PATH, strPrm, "TRANS")
                '■□■□結合試験時のみ使用予定□■□■
                '--------------------------------
                '2014/04/16 星野　ここまで
                '--------------------------------
                'ＩＣカード履歴調査一覧(詳細)へ遷移
                psOpen_Window(Me, M_NEXT_DISP_PATH)

                '終了ログ出力
                psLogEnd(Me)

            Case "btnReport"     '印刷ボタン押下

                Dim ReportFlag As String = CType(rowData.FindControl("データ種別"), TextBox).Text

                '開始ログ出力
                psLogStart(Me)

                Dim conDB As SqlConnection = Nothing
                Dim cmdDB As SqlCommand = Nothing
                Dim dstOrders As New DataSet
                Dim strOKNG As String = Nothing
                Dim strFNm As String = Nothing
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                objStack = New StackFrame
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------

                If clsDataConnect.pfOpen_Database(conDB) Then

                    Try
                        Dim yokyu_seq As String = Nothing
                        Dim eda_num As String = Nothing
                        Dim rpt As Object = Nothing

                        yokyu_seq = CType(rowData.FindControl("要求通番"), TextBox).Text.Substring(1, 5)     '要求通番

                        Select Case ReportFlag

                            Case "使用中カードDB"

                                strFNm = "使用中カードDB_" & Date.Now.ToString("yyyyMMddHHmmss")  '帳票名称

                                rpt = New TBRREP001
                                cmdDB = New SqlCommand("TBRREP001_S1", conDB)
                                eda_num = "01"

                            Case "使用中カード2DB"

                                strFNm = "使用中カード2DB_" & Date.Now.ToString("yyyyMMddHHmmss")  '帳票名称

                                rpt = New TBRREP011
                                cmdDB = New SqlCommand("TBRREP011_S1", conDB)
                                eda_num = "01"

                            Case "店内装置構成表"

                                rpt = New TBRREP003
                                strFNm = "店内装置構成票_" & Date.Now.ToString("yyyyMMddHHmmss")  '帳票名称

                                cmdDB = New SqlCommand("TBRREP003_S1", conDB)
                                eda_num = "01"

                            Case "債権明細情報"

                                rpt = New TBRREP008
                                strFNm = "債権明細情報_" & Date.Now.ToString("yyyyMMddHHmmss")  '帳票名称

                                cmdDB = New SqlCommand("TBRREP008_S1", conDB)
                                eda_num = "02"

                            Case "債務明細情報"

                                rpt = New TBRREP009
                                strFNm = "債務明細情報_" & Date.Now.ToString("yyyyMMddHHmmss")  '帳票名称

                                cmdDB = New SqlCommand("TBRREP009_S1", conDB)
                                eda_num = "03"

                            Case "精算明細情報"

                                rpt = New TBRREP010
                                strFNm = "精算明細情報_" & Date.Now.ToString("yyyyMMddHHmmss")  '帳票名称

                                cmdDB = New SqlCommand("TBRREP010_S1", conDB)
                                eda_num = "04"

                        End Select

                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("ctrl_no", SqlDbType.NVarChar, CType(rowData.FindControl("管理番号"), TextBox).Text))                   '管理番号
                            .Add(pfSet_Param("seq", SqlDbType.NVarChar, "1"))                                                                       '連番
                            .Add(pfSet_Param("nl_cls", SqlDbType.NVarChar, CType(rowData.FindControl("ＮＬ区分"), TextBox).Text))                       'NL区分
                            .Add(pfSet_Param("idic_cls", SqlDbType.NVarChar, "3"))                                                                     'ID/IC区分
                            .Add(pfSet_Param("unyo_date", SqlDbType.NVarChar, CType(rowData.FindControl("運用監視サーバ運用日付"), TextBox).Text))  '運用監視サーバ運用日付
                            .Add(pfSet_Param("yokyu_seq", SqlDbType.NVarChar, yokyu_seq))                                                           '要求通番
                            .Add(pfSet_Param("eda_seq", SqlDbType.NVarChar, eda_num))                                                           '枝番
                        End With

                        'データ取得
                        dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                        psPrintPDF(Me, rpt, dstOrders.Tables(0), strFNm)

                    Catch ex As SqlException

                        'データ取得失敗
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, ReportFlag)
                        '--------------------------------
                        '2014/04/14 星野　ここから
                        '--------------------------------
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
                        '--------------------------------
                        '2014/04/14 星野　ここまで
                        '--------------------------------
                        Exit Sub

                    Catch ex As Exception

                        '帳票印刷失敗
                        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, ReportFlag)
                        '--------------------------------
                        '2014/04/14 星野　ここから
                        '--------------------------------
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
                        '--------------------------------
                        '2014/04/14 星野　ここまで
                        '--------------------------------
                        Exit Sub

                    Finally

                        'DB切断
                        If Not clsDataConnect.pfClose_Database(conDB) Then
                            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        End If

                        '終了ログ出力
                        psLogEnd(Me)

                    End Try
                Else
                    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    Exit Sub
                End If

        End Select

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面項目の初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()

        'コントロールの初期化
        Me.txtTboxID.ppText = Nothing
        Me.txtDataDT.ppText = Nothing
        Me.Master.ppCount = "0"

        'ボタン項目の設定
        Master.ppRigthButton1.CausesValidation = False
        Master.ppRigthButton2.CausesValidation = True
        Master.ppRigthButton3.CausesValidation = False

        Master.ppRigthButton1.Visible = True
        Master.ppRigthButton2.Visible = True
        Master.ppRigthButton3.Visible = True

        Master.ppRigthButton1.Text = "更新"
        Master.ppRigthButton2.Text = "照会"
        Master.ppRigthButton3.Text = "照会条件クリア"
        Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("10001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "ICカード履歴調査一覧")

        'フォーカス設定
        Me.txtTboxID.ppTextBox.Focus()

    End Sub

    ''' <summary>
    ''' ＴＢＯＸＩＤ存在確認
    ''' </summary>
    ''' <param name="Result"></param>
    ''' <remarks></remarks>
    Private Sub ms_ChkTboxid(ByRef Result As Boolean)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
        Dim Viewst(11 - 1) As String
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Result = True

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then

            Try

                '開始ログ出力
                psLogStart(Me)

                cmdDB = New SqlCommand(M_DISP_ID & "_S2", conDB)

                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_tbox_id", SqlDbType.NVarChar, Me.txtTboxID.ppText))                'TBOXID
                    .Add(pfSet_Param("prm_nl", SqlDbType.NVarChar, 20, ParameterDirection.Output))           'NLコード
                    .Add(pfSet_Param("prm_ew", SqlDbType.NVarChar, 20, ParameterDirection.Output))           'EWコード
                    .Add(pfSet_Param("prm_system", SqlDbType.NVarChar, 20, ParameterDirection.Output))       'システム
                    .Add(pfSet_Param("prm_systemCD", SqlDbType.NVarChar, 20, ParameterDirection.Output))     'システムコード
                    .Add(pfSet_Param("prm_hallCD", SqlDbType.NVarChar, 8, ParameterDirection.Output))        'ホールコード
                    .Add(pfSet_Param("prm_hallname", SqlDbType.NVarChar, 40, ParameterDirection.Output))     'ホール名
                    .Add(pfSet_Param("prm_halltel", SqlDbType.NVarChar, 15, ParameterDirection.Output))      'ホールTEL
                    .Add(pfSet_Param("prm_halladd", SqlDbType.NVarChar, 100, ParameterDirection.Output))     'ホール住所
                    .Add(pfSet_Param("prm_TboxType", SqlDbType.NVarChar, 20, ParameterDirection.Output))     'TBOXタイプ
                    .Add(pfSet_Param("prm_ver", SqlDbType.NVarChar, 20, ParameterDirection.Output))          'バージョン
                    .Add(pfSet_Param("prm_ic_id_class", SqlDbType.NVarChar, 20, ParameterDirection.Output))  'IC/IDコード
                    .Add(pfSet_Param("prm_data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))   '結果取得用
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '結果情報を取得
                strOKNG = cmdDB.Parameters("prm_data_exist").Value.ToString

                '実行日<入力日の場合エラー
                If Me.txtDataDT.ppDate > Date.Now Then

                    Me.txtDataDT.psSet_ErrorNo("1004", Me.txtDataDT.ppName, Date.Now.ToString("yyyy/MM/dd"))
                    Me.txtDataDT.ppDateBox.Focus()

                    '処理終了
                    Result = False

                End If

                Select Case strOKNG
                    Case "0"         'データ無し

                        '対象データ無し
                        Me.txtTboxID.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
                        Me.txtTboxID.ppTextBox.Focus()

                        Result = False

                    Case Else        'データ有り

                        'ViewStateに値を設定する
                        Viewst(0) = cmdDB.Parameters("prm_nl").Value.ToString
                        Viewst(1) = cmdDB.Parameters("prm_ew").Value.ToString
                        Viewst(2) = cmdDB.Parameters("prm_system").Value.ToString
                        Viewst(3) = cmdDB.Parameters("prm_systemCD").Value.ToString
                        Viewst(4) = cmdDB.Parameters("prm_hallCD").Value.ToString
                        Viewst(5) = cmdDB.Parameters("prm_hallname").Value.ToString
                        Viewst(6) = cmdDB.Parameters("prm_halltel").Value.ToString
                        Viewst(7) = cmdDB.Parameters("prm_halladd").Value.ToString
                        Viewst(8) = cmdDB.Parameters("prm_TboxType").Value.ToString
                        Viewst(9) = cmdDB.Parameters("prm_ver").Value.ToString
                        Viewst(10) = cmdDB.Parameters("prm_ic_id_class").Value.ToString
                        Me.ViewState.Add("TBOX情報", Viewst)

                End Select

            Catch ex As SqlException

                '検索失敗
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＩＣカード履歴")
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

                'システムエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＩＣカード履歴")
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
        Else

            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            Master.ppCount = "0"

        End If

    End Sub

    ''' <summary>
    ''' 照会状況確認
    ''' </summary>
    ''' <param name="Result"></param>
    ''' <remarks></remarks>
    Private Sub ms_GetICSyoukai(ByRef Result As Boolean)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Result = False

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then

            Try

                '開始ログ出力
                psLogStart(Me)

                cmdDB = New SqlCommand(M_DISP_ID & "_S3", conDB)

                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_term_nm", SqlDbType.NVarChar, Session(P_SESSION_IP)))                    '端末情報
                    .Add(pfSet_Param("prm_user_id", SqlDbType.NVarChar, Session(P_SESSION_USERID)))                'ユーザID
                    .Add(pfSet_Param("prm_tbox_id", SqlDbType.NVarChar, Me.txtTboxID.ppText))                      'TBOXID
                    .Add(pfSet_Param("prm_proc_cd", SqlDbType.NVarChar, "301"))                                    '処理コード
                    .Add(pfSet_Param("prm_data_dt", SqlDbType.NVarChar, txtDataDT.ppText.Replace("/", "")))        'データ日
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))             '結果取得用
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '結果情報を取得
                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "0"         'データ無し

                        Result = True

                    Case Else        'データ有り

                        psMesBox(Me, "20002", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)

                End Select

            Catch ex As SqlException

                '検索失敗
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＩＣカード履歴")
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

                'システムエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＩＣカード履歴")
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
        Else

            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            Master.ppCount = "0"

        End If

    End Sub

    ''' <summary>
    ''' 照会依頼データ作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_InsICdata(ByVal Upd_Ins_flag As Boolean _
                           , ByVal cntrol As String _
                           , ByVal seq As Integer _
                           , ByVal joukyou As String)

        Dim conDB As SqlConnection = Nothing                           'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing                              'SqlCommand
        Dim trans As SqlClient.SqlTransaction                          'トランザクション
        Dim objDs As DataSet = Nothing                                 'データセット
        Dim strOKNG As String = Nothing                                '検索結果
        Dim cntrol_num As String = cntrol                              '管理番号
        Dim Viewst() As String = Me.ViewState("TBOX情報")
        Dim errMsg As String = "照会処理"
        Dim dtDate As Date = Date.Now
        Dim maxSeq As String = Nothing
        Dim dstOrders As New DataSet
        Dim strKKTboxId As String = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then

            Try

                '開始ログ出力
                psLogStart(Me)

                '管理番号の作成
                cntrol_num = dtDate.ToString("yyyyMMdd")

                '管理番号採番
                'パラメータ設定
                cmdDB = New SqlCommand("ZCMPSEL022", conDB)
                With cmdDB.Parameters
                    'パラメータ設定 
                    .Add(pfSet_Param("MNGNO_CLS", SqlDbType.Int, 1))                                    '管理番号
                    .Add(pfSet_Param("YMD", SqlDbType.NVarChar, dtDate.ToString("yyyyMMdd")))           '年月日
                    .Add(pfSet_Param("SalesYTD", SqlDbType.Int, 20, ParameterDirection.Output))         '結果取得用
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '戻り値確認
                If cmdDB.Parameters("SalesYTD").Value.ToString Is Nothing Then

                    '処理終了
                    Throw New Exception

                End If

                '管理番号を作成する
                'cntrol_num = cntrol_num.Substring(2, 2) _
                '             + "_" _
                '             + cntrol_num.Substring(2, 2) _
                '             + "_" _
                '             + cmdDB.Parameters("SalesYTD").Value.ToString("0000")

                'cntrol_num = Date.Now.ToString("yyyyMMdd") _
                '             + "_" _
                '             + CInt(cmdDB.Parameters("SalesYTD").Value).ToString("0000")

                cntrol_num = Date.Now.ToString("yyyyMMdd") _
                             + CInt(cmdDB.Parameters("SalesYTD").Value).ToString("0000")

                If Viewst(0) = "N" Or Viewst(0) = "J" Then

                    'cntrol_num = "GI" + "_" + cntrol_num
                    strKKTboxId = "20"

                Else

                    'cntrol_num = "LI" + "_" + cntrol_num
                    strKKTboxId = "10"

                End If

                '照会要求データ連番最大値取得
                'パラメータ設定
                cmdDB = New SqlCommand(M_DISP_ID + "_S4", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("ymd", SqlDbType.NVarChar, dtDate.ToString("yyyyMMdd")))              '要求日付
                    .Add(pfSet_Param("maxseq", SqlDbType.Int, 20, ParameterDirection.Output))              '結果取得用
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                maxSeq = String.Format("{0:000000}", cmdDB.Parameters("maxseq").Value.ToString).ToString

                'トランザクションの設定
                trans = conDB.BeginTransaction

                Try

                    '照会要求データ
                    cmdDB = New SqlCommand(M_DISP_ID + "_I1", conDB)
                    With cmdDB.Parameters
                        .Add(pfSet_Param("prm_req_dt", SqlDbType.NVarChar, dtDate.ToString("yyyyMMdd")))            '要求日付
                        .Add(pfSet_Param("prm_req_seq", SqlDbType.NVarChar, CInt(maxSeq).ToString("000000")))       '要求通番
                        .Add(pfSet_Param("prm_cond_flg", SqlDbType.NVarChar, "0"))                                  '状態フラグ
                        .Add(pfSet_Param("prm_term_nm", SqlDbType.NVarChar, Session(P_SESSION_IP)))                 '端末情報
                        .Add(pfSet_Param("prm_user_id", SqlDbType.NVarChar, Session(P_SESSION_USERID)))             'ユーザID
                        .Add(pfSet_Param("prm_init_dt", SqlDbType.NVarChar, Date.Now.ToString("yyyyMMddHHmmss")))   '作成日
                        .Add(pfSet_Param("prm_tboxid", SqlDbType.NVarChar, Me.txtTboxID.ppText))                    'TBOXID
                        .Add(pfSet_Param("prm_proc_cd", SqlDbType.NVarChar, "301"))                                 '処理コード
                        .Add(pfSet_Param("prm_ftp_snd", SqlDbType.NVarChar, "0"))                                   'FTP送信
                        .Add(pfSet_Param("prm_ftp_rcv", SqlDbType.NVarChar, "0"))                                   'FTP受信
                        .Add(pfSet_Param("prm_reqmng_no", SqlDbType.NVarChar, cntrol_num))                          '新管理番号
                    End With
                    'コマンドタイプ設定(ストアド)
                    errMsg = "照会要求データ"
                    'トランザクションの設定
                    cmdDB.Transaction = trans
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()

                    '照会依頼データ
                    cmdDB = New SqlCommand(M_DISP_ID + "_I2", conDB)
                    With cmdDB.Parameters
                        .Add(pfSet_Param("prm_ctrl_no", SqlDbType.NVarChar, cntrol_num))
                        .Add(pfSet_Param("prm_seq", SqlDbType.Int, 1))
                        .Add(pfSet_Param("prm_nl_cls", SqlDbType.NVarChar, Viewst(0)))
                        .Add(pfSet_Param("prm_id_ic_cls", SqlDbType.NVarChar, Viewst(10)))
                        .Add(pfSet_Param("prm_reqseq_spc", SqlDbType.NChar, "000001"))
                        .Add(pfSet_Param("prm_reqdate_spc", SqlDbType.NChar, Date.Now.ToString("yyyyMMddHHmmss")))
                        .Add(pfSet_Param("prm_kkchutbxid", SqlDbType.NChar, strKKTboxId + Me.txtTboxID.ppText))
                        .Add(pfSet_Param("prm_dataclass_1", SqlDbType.NChar, "27"))
                        .Add(pfSet_Param("prm_datadate_1", SqlDbType.NChar, txtDataDT.ppText.Replace("/", "")))
                        .Add(pfSet_Param("prm_bbserialno_1", SqlDbType.NChar, ""))
                        .Add(pfSet_Param("prm_cid_1", SqlDbType.NChar, ""))
                        .Add(pfSet_Param("prm_denpyono_1", SqlDbType.NChar, ""))
                        .Add(pfSet_Param("prm_dataclass_2", SqlDbType.NChar, "39"))
                        .Add(pfSet_Param("prm_datadate_2", SqlDbType.NChar, txtDataDT.ppText.Replace("/", "")))
                        .Add(pfSet_Param("prm_bbserialno_2", SqlDbType.NChar, ""))
                        .Add(pfSet_Param("prm_cid_2", SqlDbType.NChar, ""))
                        .Add(pfSet_Param("prm_denpyono_2", SqlDbType.NChar, ""))
                        .Add(pfSet_Param("prm_dataclass_3", SqlDbType.NChar, "40"))
                        .Add(pfSet_Param("prm_datadate_3", SqlDbType.NChar, txtDataDT.ppText.Replace("/", "")))
                        .Add(pfSet_Param("prm_bbserialno_3", SqlDbType.NChar, ""))
                        .Add(pfSet_Param("prm_cid_3", SqlDbType.NChar, ""))
                        .Add(pfSet_Param("prm_denpyono_3", SqlDbType.NChar, ""))
                        .Add(pfSet_Param("prm_dataclass_4", SqlDbType.NChar, "42"))
                        .Add(pfSet_Param("prm_datadate_4", SqlDbType.NChar, txtDataDT.ppText.Replace("/", "")))
                        .Add(pfSet_Param("prm_bbserialno_4", SqlDbType.NChar, ""))
                        .Add(pfSet_Param("prm_cid_4", SqlDbType.NChar, ""))
                        .Add(pfSet_Param("prm_denpyono_4", SqlDbType.NChar, ""))
                        .Add(pfSet_Param("prm_insert_usr", SqlDbType.NChar, Session(P_SESSION_USERID)))
                    End With
                    'コマンドタイプ設定(ストアド)
                    errMsg = "照会依頼データ"
                    'トランザクションの設定
                    cmdDB.Transaction = trans
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()

                    'ICカード履歴調査一覧
                    cmdDB = New SqlCommand(M_DISP_ID + "_I3", conDB)
                    With cmdDB.Parameters
                        .Add(pfSet_Param("prm_response_no", SqlDbType.NVarChar, cntrol_num))                 '管理番号
                        .Add(pfSet_Param("prm_seqno", SqlDbType.Int, 1))                                     '連番
                        .Add(pfSet_Param("prm_tboxid", SqlDbType.NVarChar, Me.txtTboxID.ppText))             'ＴＢＯＸＩＤ
                        .Add(pfSet_Param("prm_hall_cd", SqlDbType.NVarChar, Viewst(4)))                      'ホールコード
                        .Add(pfSet_Param("prm_hall_nm", SqlDbType.NVarChar, Viewst(5)))                      'ホール名
                        .Add(pfSet_Param("prm_system_cd", SqlDbType.NVarChar, Viewst(3)))                    'システムコード
                        .Add(pfSet_Param("prm_system_nm", SqlDbType.NVarChar, Viewst(2)))                    'システム
                        .Add(pfSet_Param("prm_version", SqlDbType.NVarChar, Viewst(9)))                      'バージョン
                        .Add(pfSet_Param("prm_proc_cd", SqlDbType.NVarChar, "301"))                          '処理コード
                        .Add(pfSet_Param("prm_cardid", SqlDbType.NVarChar, ""))                              'カードＩＤ
                        .Add(pfSet_Param("prm_data_dt", SqlDbType.DateTime, Me.txtDataDT.ppText))            'データ日付
                        .Add(pfSet_Param("prm_insert_usr", SqlDbType.NVarChar, Session(P_SESSION_USERID)))   '登録者
                    End With
                    'コマンドタイプ設定(ストアド)
                    errMsg = "ICカード履歴調査一覧"
                    'トランザクションの設定
                    cmdDB.Transaction = trans
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()

                    'コミット
                    trans.Commit()

                Catch ex As Exception

                    'システムエラー
                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, errMsg)
                    '--------------------------------
                    '2014/04/14 星野　ここから
                    '--------------------------------
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    '--------------------------------
                    '2014/04/14 星野　ここまで
                    '--------------------------------

                    'ロールバック
                    trans.Rollback()

                    Throw ex

                End Try

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, errMsg)
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Throw ex

            Finally

                '終了ログ出力
                psLogEnd(Me)

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Throw New Exception
        End If

    End Sub

    ''' <summary>
    ''' ICカード履歴調査一覧の情報取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub me_GetICRireki()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then

            Try

                '開始ログ出力
                psLogStart(Me)

                cmdDB = New SqlCommand(M_DISP_ID & "_S1", conDB)

                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("term_nm", SqlDbType.NVarChar, Session(P_SESSION_IP)))            '端末情報
                    .Add(pfSet_Param("user_id", SqlDbType.NVarChar, Session(P_SESSION_USERID)))        'ユーザID
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output)) '結果取得用
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '結果情報を取得
                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "0"         'データ無し

                        '件数を設定
                        Master.ppCount = "0"

                        'グリッドの初期化
                        Me.grvList.DataSource = New DataTable
                        '変更を反映
                        Me.grvList.DataBind()

                        '0件
                        '--------------------------------
                        '2014/05/12 後藤　ここから
                        '--------------------------------
                        'psMesBox(Me, "00007", clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後)
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        '--------------------------------
                        '2014/05/12 後藤　ここまで
                        '--------------------------------

                    Case Else        'データ有り

                        '件数を設定
                        Master.ppCount = dstOrders.Tables(1).Rows(0).Item("該当件数").ToString

                        '取得したデータをグリッドに設定
                        Me.grvList.DataSource = dstOrders.Tables(0)

                        '変更を反映
                        Me.grvList.DataBind()

                End Select

            Catch ex As SqlException

                '検索失敗
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＩＣカード履歴")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Throw ex

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＩＣカード履歴")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Throw ex

            Finally

                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

                '終了ログ出力
                psLogEnd(Me)

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            Master.ppCount = "0"

        End If

    End Sub

    ''' <summary>
    ''' データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        Dim strSyokai As String = Nothing        '照会状況
        Dim strData As String = Nothing          'データ種別
        Dim strCondFlag As String = Nothing      '照会状況
        Dim strResultCD As String = Nothing      '照会結果
        Dim strTrublMake As String = Nothing     '照会結果
        Dim strVs() As String = ViewState(P_KEY)

        For Each rowData As GridViewRow In grvList.Rows

            strSyokai = CType(rowData.FindControl("照会状況"), TextBox).Text
            strData = CType(rowData.FindControl("データ種別"), TextBox).Text
            strCondFlag = CType(rowData.FindControl("照会状況"), TextBox).Text
            strResultCD = CType(rowData.FindControl("照会結果"), TextBox).Text

            'ボタンの非活性(デフォルト)
            rowData.Cells(0).Enabled = False
            rowData.Cells(10).Enabled = False

            Select Case strSyokai
                Case "集計完了"

                    '文字の色を変更
                    CType(rowData.FindControl("照会状況"), TextBox).ForeColor = Drawing.Color.Blue
                    rowData.Cells(0).Enabled = True

                Case "集計失敗"

                    '文字の色を変更
                    CType(rowData.FindControl("照会状況"), TextBox).ForeColor = Drawing.Color.Red

            End Select

            Select Case strResultCD
                Case "正常終了"

                    '文字の色を変更
                    CType(rowData.FindControl("照会状況"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("照会結果"), TextBox).ForeColor = Drawing.Color.Blue

                Case "異常終了"

                    '文字の色を変更
                    CType(rowData.FindControl("照会状況"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("照会結果"), TextBox).ForeColor = Drawing.Color.Red

            End Select

            '詳細・印刷ボタンの表示/非表示制御
            Select Case strData
                Case Nothing

                    '印刷ボタンの非表示
                    rowData.Cells(10).Text = ""

                Case Else

                    '詳細ボタンの非表示
                    rowData.Cells(0).Text = ""

                    '印刷ボタンの活性/非活性制御
                    If strResultCD = "正常終了" And strCondFlag = "受信完了" Then
                        Select Case strData
                            Case "店内装置構成表"
                                rowData.Cells(10).Enabled = False
                                If CType(rowData.FindControl("装置構成有無"), TextBox).Text = "有" Then rowData.Cells(10).Enabled = True
                            Case "債権明細情報"
                                rowData.Cells(10).Enabled = False
                                If CType(rowData.FindControl("債権明細有無"), TextBox).Text = "有" Then rowData.Cells(10).Enabled = True
                            Case "債務明細情報"
                                rowData.Cells(10).Enabled = False
                                If CType(rowData.FindControl("債務明細有無"), TextBox).Text = "有" Then rowData.Cells(10).Enabled = True
                            Case "精算明細情報"
                                rowData.Cells(10).Enabled = False
                                If CType(rowData.FindControl("精算明細有無"), TextBox).Text = "有" Then rowData.Cells(10).Enabled = True
                        End Select

                    End If

            End Select

        Next

    End Sub

#End Region

#Region "終了処理プロシージャ"

#End Region

End Class
