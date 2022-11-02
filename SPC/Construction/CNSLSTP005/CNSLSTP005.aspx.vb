'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　工事連絡票一覧
'*　ＰＧＭＩＤ：　CNSLSTP005
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.2　：　ＸＸＸ
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'CNSLSTP005-001     2015/12/07      加賀　　　該当件数0件の場合、該当件数とグリッドを更新するように修正
'CNSLSTP005-002     2016/07/05      栗原　　　NGC側の初期表示条件を03:回答済み以外に変更
'CNSLSTP005-003     2016/07/28      栗原　　　一覧にソート機能を実装、レイアウトの調整

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive

#End Region

Public Class CNSLSTP005
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
    Const M_MY_DISP_ID = P_FUN_CNS & P_SCR_LST & P_PAGE & "005"

    '次画面ファイルパス
    Const M_NEXT_DISP_PATH = "~/" & P_CNS & "/" &
            P_FUN_CNS & P_SCR_UPD & P_PAGE & "004" & "/" &
            P_FUN_CNS & P_SCR_UPD & P_PAGE & "004.aspx"

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
    '接続場所
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
        'CNSLSTP005-003
        'pfSet_GridView(Me.grvList, M_MY_DISP_ID, M_MY_DISP_ID + "_Header", Me.DivOut)
        pfSet_GridView(Me.grvList, M_MY_DISP_ID)
        'CNSLSTP005-003 END

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
        AddHandler Master.ppRigthButton5.Click, AddressOf btnNew_Click
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            If Not IsPostBack Then  '初回表示のみ
                '新規作成ボタンの作成
                Master.ppRigthButton5.Visible = True
                Master.ppRigthButton5.Text = "新規作成"

                '接続場所がＳＰＣの場合のみ新規作成ボタンを活性化
                If strAdd <> P_ADD_SPC Then
                    Master.ppRigthButton5.Enabled = False
                End If

                '検索条件クリアボタンと新規作成ボタン押下時の検証を無効
                Master.ppRigthButton2.CausesValidation = False
                Master.ppRigthButton5.CausesValidation = False

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

                'ドロップダウンリスト設定
                msSet_ddlNGCStatus()

                '画面クリア
                msClear_Screen()

                '引継ぎ項目を検索条件に設定
                Dim key As String() = Session(P_KEY)
                If Not key Is Nothing AndAlso key.GetLength(0) = 3 Then
                    Me.txtCnstNo.ppText = Session(P_KEY)(0)
                    Me.tftTboxId.ppTextBoxFrom.Text = Session(P_KEY)(1)
                    Me.txtHallNm.ppText = Session(P_KEY)(2)
                End If

                If Session(P_SESSION_AUTH) = "NGC" Then

                    '初期時のドロップダウンを未回答に設定
                    Me.ddlNGCStatus.SelectedIndex = 1
                    If IsPostBack Then
                        'データ取得
                        msSet_Data()
                    Else
                        '初期表示
                        msSet_Data_First()
                    End If


                End If

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
    '2014/04/23 武 ここから
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
    '2014/04/23 武 ここまで
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

        If (Page.IsValid) Then
            'データ取得
            msSet_Data()
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

        '検索項目クリア
        msClearSearchKmk()

        'TBOXIDにフォーカス設定
        Me.tftTboxId.ppTextBoxFrom.Focus()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 新規作成ボタンクリック時処理
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
        strList.Add(Me.txtCnstNo.ppText)

        'セッション情報設定																	
        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録
        Session(P_KEY) = strList.ToArray
        Session(P_SESSION_OLDDISP) = M_MY_DISP_ID

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
        '工事連絡票　参照／更新　画面起動
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

        If mfGet_Comm(CType(rowData.FindControl("連絡票管理番号"), TextBox).Text) = False Then
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
            Exit Sub
        End If
        '--------------------------------
        '2014/05/30 後藤　ここから
        '--------------------------------
        '排他制御用の変数定義.
        Dim strExclusiveDate As String = String.Empty
        Dim arTable_Name As New ArrayList
        Dim arKey As New ArrayList
        '--------------------------------
        '2014/05/30 後藤　ここまで
        '--------------------------------

        '次画面引継ぎ用キー情報設定
        strList = New List(Of String)
        strList.Add(CType(rowData.FindControl("連絡票管理番号"), TextBox).Text)

        'セッション情報設定																	
        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
        Session(P_KEY) = strList.ToArray
        Session(P_SESSION_OLDDISP) = M_MY_DISP_ID
        Select Case e.CommandName
            Case "btnReference"     '参照
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
            Case "btnUpdate"        '更新
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新

                'ロック対象テーブル名の登録.
                arTable_Name.Insert(0, "D25_CNST_COMM")

                'ロックテーブルキー項目の登録.
                arKey.Insert(0, CType(rowData.FindControl("連絡票管理番号"), TextBox).Text)

                '排他情報確認処理(更新処理の実行).
                If clsExc.pfSel_Exclusive(strExclusiveDate _
                                 , Me _
                                 , Session(P_SESSION_IP) _
                                 , Session(P_SESSION_PLACE) _
                                 , Session(P_SESSION_USERID) _
                                 , Session(P_SESSION_SESSTION_ID) _
                                 , ViewState(P_SESSION_GROUP_NUM) _
                                 , "CNSUPDP004" _
                                 , arTable_Name _
                                 , arKey) = 0 Then

                    '登録年月日時刻.
                    Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate
                Else

                    '排他ロック中.
                    Exit Sub

                End If

                '排他情報のグループ番号をセッション変数に設定.
                Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

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
        '工事連絡票画面起動
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
        '接続先が作業拠点の場合、更新を非活性にする
        For Each rowData As GridViewRow In grvList.Rows
            If strAdd = P_ADD_WKB Then
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
        '検索項目クリア
        Me.msClearSearchKmk()

        '一覧クリア
        Me.msClearList()

        'TBOXIDにフォーカス設定
        Me.tftTboxId.ppTextBoxFrom.Focus()

    End Sub

    ''' <summary>
    ''' 検索項目クリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearSearchKmk()
        Me.tftTboxId.ppFromText = String.Empty              'ＴＢＯＸＩＤ From
        Me.tftTboxId.ppToText = String.Empty                'ＴＢＯＸＩＤ To
        Me.txtCnstNo.ppText = String.Empty                  '工事依頼番号
        Me.dftConstD.ppFromText = String.Empty              '工事日 From
        Me.dftConstD.ppToText = String.Empty                '工事日 To
        Me.txtHallNm.ppText = String.Empty                  'ホール
        Me.txtCommNo.ppText = String.Empty                  '工事連絡票番号
        Me.ddlNGCStatus.SelectedValue = String.Empty        'ＮＧＣ進捗状況
    End Sub

    ''' <summary>
    ''' 一覧初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearList()
        Master.ppCount = "0"
        Me.grvList.DataSource = New DataTable
        Me.grvList.DataBind()
    End Sub

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <param name="ipstrTboxIdF">ＴＢＯＸＩＤFrom</param>
    ''' <param name="ipstrTboxIdT">ＴＢＯＸＩＤTo</param>
    ''' <param name="ipstrCnstNo">工事依頼番号</param>
    ''' <param name="ipstrConstDF">工事日From</param>
    ''' <param name="ipstrConstDT">工事日To</param>
    ''' <param name="ipstrHallNm">ホール名</param>
    ''' <param name="ipstrCommNo">工事連絡票番号</param>
    ''' <param name="ipstrNGCStatusCd">ＮＧＣ進捗状況</param>
    ''' <param name="opdstData">工事連絡票一覧項目</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_Data(ByVal ipstrTboxIdF As String _
                              , ByVal ipstrTboxIdT As String _
                              , ByVal ipstrCnstNo As String _
                              , ByVal ipstrConstDF As String _
                              , ByVal ipstrConstDT As String _
                              , ByVal ipstrHallNm As String _
                              , ByVal ipstrCommNo As String _
                              , ByVal ipstrNGCStatusCd As String _
                              , ByRef opdstData As DataSet _
                              ) As Boolean
        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGet_Data = False

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then

            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_S1", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    'ＴＢＯＸＩＤFrom
                    If Me.tftTboxId.ppToText = String.Empty Then
                        'ＴＢＯＸＩＤToが空白の場合は「あいまい検索」なのでエスケープする
                        .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar _
                                         , ipstrTboxIdF.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    Else
                        'ＴＢＯＸＩＤToが空白でないの場合は「範囲検索」なのでエスケープしない
                        .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, ipstrTboxIdF))
                    End If
                    'ＴＢＯＸＩＤTo
                    .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, ipstrTboxIdT))
                    '工事依頼番号
                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar _
                                     , ipstrCnstNo.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    '工事日From
                    .Add(pfSet_Param("const_d_f", SqlDbType.NVarChar, ipstrConstDF.Replace("/", "")))
                    '工事日To
                    .Add(pfSet_Param("const_d_t", SqlDbType.NVarChar, ipstrConstDT.Replace("/", "")))
                    'ホール名
                    .Add(pfSet_Param("hall_nm", SqlDbType.NVarChar _
                                     , ipstrHallNm.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    '工事連絡票番号
                    .Add(pfSet_Param("comm_no", SqlDbType.NVarChar, ipstrCommNo))
                    'ＮＧＣ進捗状況
                    .Add(pfSet_Param("ngc_status_cd", SqlDbType.NVarChar, ipstrNGCStatusCd))
                    '場所区分
                    .Add(pfSet_Param("Auth_cls", SqlDbType.NVarChar, Session(P_SESSION_AUTH)))
                End With

                'データ取得
                opdstData = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ有無確認
                If opdstData.Tables(0).Rows.Count = 0 Then
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    'CNSLSTP005-001
                    Me.grvList.DataSource = New DataTable
                    Me.grvList.DataBind()
                    Master.ppCount = "0"
                    'CNSLSTP005-001 END
                    Exit Function
                End If

                mfGet_Data = True

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事連絡票")
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
    End Function

    ''' <summary>
    ''' 工事連絡票存在確認
    ''' </summary>
    ''' <param name="ipstrCommNo">工事連絡票番号</param>
    ''' <remarks></remarks>
    Private Function mfGet_Comm(ByVal ipstrCommNo As String) As Boolean
        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGet_Comm = False

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then

            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_S2", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    '工事連絡票番号
                    .Add(pfSet_Param("comm_no", SqlDbType.NVarChar, ipstrCommNo))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ有無確認
                If dstOrders.Tables.Count <> 0 Then
                    If dstOrders.Tables(0).Rows.Count = 0 Then
                        'psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        Exit Function
                    End If
                End If

                mfGet_Comm = True

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事連絡票")
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
    End Function

    ''' <summary>
    ''' データ表示処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_Data()
        Dim dtsCommData As DataSet = Nothing

        '工事依頼番号情報取得
        If mfGet_Data(Me.tftTboxId.ppFromText _
                    , Me.tftTboxId.ppToText _
                    , Me.txtCnstNo.ppText _
                    , Me.dftConstD.ppFromText _
                    , Me.dftConstD.ppToText _
                    , Me.txtHallNm.ppText _
                    , Me.txtCommNo.ppText _
                    , Me.ddlNGCStatus.SelectedValue _
                    , dtsCommData _
                     ) Then

            '件数を設定
            Master.ppCount = dtsCommData.Tables(0).Rows.Count.ToString

            '取得したデータをグリッドに設定
            Me.grvList.DataSource = dtsCommData.Tables(0)

            '変更を反映
            Me.grvList.DataBind()

        End If
    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（進捗ステータスマスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_ddlNGCStatus()
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
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("duty_cd", SqlDbType.NVarChar, "23"))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'ドロップダウンリスト設定
                Me.ddlNGCStatus.Items.Clear()
                Me.ddlNGCStatus.DataSource = dstOrders.Tables(0)
                Me.ddlNGCStatus.DataTextField = "進捗ステータス名"
                Me.ddlNGCStatus.DataValueField = "進捗ステータス"
                Me.ddlNGCStatus.DataBind()
                Me.ddlNGCStatus.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "進捗ステータスマスタ一覧")
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
    ''' 初期データ表示処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_Data_First()
        Dim dtsCommData As DataSet = Nothing
        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        objStack = New StackFrame

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then

            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_S1", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    'ＴＢＯＸＩＤFrom
                    'ＴＢＯＸＩＤToが空白の場合は「あいまい検索」なのでエスケープする
                    .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, ""))
                    'ＴＢＯＸＩＤTo
                    .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, ""))
                    '工事依頼番号
                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ""))
                    '工事日From
                    .Add(pfSet_Param("const_d_f", SqlDbType.NVarChar, ""))
                    '工事日To
                    .Add(pfSet_Param("const_d_t", SqlDbType.NVarChar, ""))
                    'ホール名
                    .Add(pfSet_Param("hall_nm", SqlDbType.NVarChar, ""))
                    '工事連絡票番号
                    .Add(pfSet_Param("comm_no", SqlDbType.NVarChar, ""))
                    'ＮＧＣ進捗状況
                    .Add(pfSet_Param("ngc_status_cd", SqlDbType.NVarChar, ""))
                    '場所区分
                    .Add(pfSet_Param("Auth_cls", SqlDbType.NVarChar, Session(P_SESSION_AUTH)))
                    '初期表示フラグ
                    .Add(pfSet_Param("First_flg", SqlDbType.NVarChar, "1"))
                End With

                'データ取得
                dtsCommData = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ有無確認
                If dtsCommData.Tables(0).Rows.Count = 0 Then
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    'CNSLSTP005-001
                    Me.grvList.DataSource = New DataTable
                    Me.grvList.DataBind()
                    Master.ppCount = "0"
                    'CNSLSTP005-001 END
                    Exit Sub
                End If


            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事連絡票")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
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

        '件数を設定
        Master.ppCount = dtsCommData.Tables(0).Rows.Count.ToString
        '取得したデータをグリッドに設定
        Me.grvList.DataSource = dtsCommData.Tables(0)
        '変更を反映
        Me.grvList.DataBind()

    End Sub

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
