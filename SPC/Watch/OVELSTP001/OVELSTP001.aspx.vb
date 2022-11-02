'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜監視＞
'*　処理名　　：　時間外消費ホール詳細
'*　ＰＧＭＩＤ：　OVELSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.01.14　：　ＮＫＣ
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax

'排他制御用
Imports SPC.ClsCMExclusive

#End Region


Public Class OVELSTP001
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
    Private Const M_DISP_ID = P_FUN_OVE & P_SCR_LST & P_PAGE & "001"

    '次画面ファイルパス
    Const M_NEXT_DISP_PATH = "~/" & P_WAT & "/" &
            P_FUN_OVE & P_SCR_LST & P_PAGE & "002" & "/" &
            P_FUN_OVE & P_SCR_LST & P_PAGE & "002.aspx"

    '次画面ID
    Const M_DISP_OVELST002_ID = P_FUN_OVE & P_SCR_LST & P_PAGE & "002"

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
    ''' Page_Init
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        '表設定
        pfSet_GridView(Me.grvList, M_DISP_ID)

    End Sub

    ''' <summary>
    ''' Page_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btn_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btn_Click
        AddHandler Master.Master.ppRigthButton1.Click, AddressOf btn_Click
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        If Not IsPostBack Then  '初回表示

            Try

                '不正なアクセス
                If Session(P_SESSION_BCLIST) Is Nothing Then

                    Throw New Exception

                End If


                '排他情報用のグループ番号保管
                If Not Session(P_SESSION_GROUP_NUM) Is Nothing Then

                    ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

                End If

                'プログラムＩＤ、画面名設定
                Master.Master.ppProgramID = M_DISP_ID
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

                'パンくずリスト設定
                Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                '画面初期化処理
                msClearScreen()

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
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
                Exit Sub

            End Try


        End If

    End Sub

#End Region

#Region "ユーザー権限"
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

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
            Case "NGC"
        End Select

    End Sub
    '---------------------------
    '2014/04/16 武 ここまで
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

        Dim strView_st(2 - 1) As String
        Dim Result As Boolean = False
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            Select Case sender.text

                Case "検索条件クリア"

                    Me.txtTyousaFromTo.ppFromText = Nothing
                    Me.txtTyousaFromTo.ppToText = Nothing
                    '--------------------------------
                    '2014/06/09 後藤　ここから
                    '--------------------------------
                    Me.grvList.DataSource = New DataTable
                    Master.ppCount = "0"
                    Me.grvList.DataBind()
                    Master.Master.ppRigthButton1.Enabled = False
                    '--------------------------------
                    '2014/06/09 後藤　ここまで
                    '--------------------------------
                Case "検索"

                    '検証チェック結果
                    If (Page.IsValid) Then

                        Try

                            '画面更新ボタン押下時の情報を設定
                            If String.IsNullOrEmpty(Me.txtTyousaFromTo.ppFromText) Then
                                strView_st(0) = "1"
                            Else
                                strView_st(0) = Me.txtTyousaFromTo.ppFromText.Replace("/", "")
                            End If
                            If String.IsNullOrEmpty(Me.txtTyousaFromTo.ppToText) Then
                                strView_st(1) = "1"
                            Else
                                strView_st(1) = Me.txtTyousaFromTo.ppToText.Replace("/", "")
                            End If

                            '時間外消費テーブル検索/グリッド表示
                            ms_GetSearchData(strView_st(0) _
                                           , strView_st(1) _
                                           , Result)

                            '検索結果が0以上
                            If Result Then

                                '画面更新ボタン押下時の情報を設定
                                Me.ViewState.Add("画面更新", strView_st)
                                Master.Master.ppRigthButton1.Enabled = True

                            Else
                                Master.Master.ppRigthButton1.Enabled = False


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
                            '処理終了
                            Exit Sub

                        End Try

                    End If

                Case "画面更新"

                    Try

                        strView_st = Me.ViewState("画面更新")

                        '時間外消費テーブル検索/グリッド表示
                        ms_GetSearchData(strView_st(0) _
                                       , strView_st(1) _
                                       , Result)

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

                    End Try

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

        End Try

    End Sub

    ''' <summary>
    ''' グリッドボタン操作
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        Select Case e.CommandName

            Case "btnReference"
            Case "btnUpdate"
            Case Else
                Exit Sub

        End Select

        Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    'ボタン押下行のIndex
        Dim rowData As GridViewRow = grvList.Rows(intIndex)             'ボタン押下行
        Dim strKeyList As List(Of String) = Nothing                     '次画面引継ぎ用キー情報

        '開始ログ出力
        psLogStart(Me)

        Select Case e.CommandName
            Case "btnReference" '参照
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
            Case "btnUpdate"    '更新

                '排他制御開始
                'Dim strExclusiveDate As String = Nothing
                'Dim arTable_Name As New ArrayList
                'Dim arKey As New ArrayList

                ''★ロック対象テーブル名の登録
                'arTable_Name.Insert(0, "D168_JIKANGAI")

                ''★ロックテーブルキー項目の登録(D168_JIKANGAI)
                ''-----------------------------------------
                '' 6/08 後藤　ここから
                ''-----------------------------------------
                ''arKey.Insert(0, CType(rowData.FindControl("調査年月日"), TextBox).Text)
                'arKey.Insert(0, CType(rowData.FindControl("管理番号"), TextBox).Text)
                ''arKey.Insert(0, CType(rowData.FindControl("調査日時"), TextBox).Text)
                ''-----------------------------------------
                '' 6/08 後藤　ここまで
                ''-----------------------------------------

                ''★排他情報確認処理(更新画面へ遷移)
                'If clsexc.pfSel_Exclusive(strExclusiveDate _
                '                 , Me _
                '                 , Session(P_SESSION_IP) _
                '                 , Session(P_SESSION_PLACE) _
                '                 , Session(P_SESSION_USERID) _
                '                 , Session(P_SESSION_SESSTION_ID) _
                '                 , ViewState(P_SESSION_GROUP_NUM) _
                '                 , M_DISP_OVELST002_ID _
                '                 , arTable_Name _
                '                 , arKey) = 0 Then

                '遷移先へ受け渡す変数の設定
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新

                ''★登録年月日時刻
                'Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate

                'Else

                ''排他ロック中
                'Exit Sub

                'End If

        End Select

        '次画面引継ぎ用キー情報設定
        strKeyList = New List(Of String)
        strKeyList.Add(CType(rowData.FindControl("調査年月日"), TextBox).Text)
        strKeyList.Add(CType(rowData.FindControl("管理番号"), TextBox).Text)
        strKeyList.Add(CType(rowData.FindControl("調査日時"), TextBox).Text)

        'セッション情報設定																	
        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
        Session(P_KEY) = strKeyList.ToArray

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
        '時間外消費ログ発生調査詳細起動
        psOpen_Window(Me, M_NEXT_DISP_PATH)

        '終了ログ出力
        psLogEnd(Me)

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()

        '入力項目の初期化
        Me.txtTyousaFromTo.ppFromText = Nothing
        Me.txtTyousaFromTo.ppToText = Nothing
        Me.grvList.DataSource = New DataTable
        Me.grvList.DataBind()

        '件数の初期化
        Me.Master.ppCount = "0"

        'ボタン項目の設定
        Master.ppRigthButton1.CausesValidation = True
        Master.ppRigthButton2.CausesValidation = False
        Master.Master.ppRigthButton1.CausesValidation = False
        'Master.ppRigthButton1.ValidationGroup = "1"

        'ボタンの表示
        Master.Master.ppRigthButton1.Text = "画面更新"
        Master.Master.ppRigthButton1.Visible = True
        Master.Master.ppRigthButton1.Enabled = False

    End Sub

    ''' <summary>
    ''' 時間外消費テーブル検索/グリッド表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_GetSearchData(ByVal DateFrom As String _
                               , ByVal DateTo As String _
                               , ByRef Result As Boolean)

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

                cmdDB = New SqlCommand(M_DISP_ID & "_S1", conDB)

                'BB構成ファイル
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_exe_dt_f", SqlDbType.NVarChar, DateFrom))                      'FROM
                    .Add(pfSet_Param("prm_exe_dt_t", SqlDbType.NVarChar, DateTo))                        'TO
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))   '結果取得用
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '結果情報を取得
                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                '取得したデータをグリッドに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

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

                        '結果を返す
                        Result = False

                    Case Else        'データ有り

                        If dstOrders.Tables(0).Rows(0)("データ件数") > dstOrders.Tables(0).Rows.Count Then
                            psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                                dstOrders.Tables(0).Rows(0)("データ件数").ToString, dstOrders.Tables(0).Rows.Count.ToString)
                        End If
                        Master.ppCount = dstOrders.Tables(0).Rows(0)("データ件数").ToString

                        '変更を反映
                        Me.grvList.DataBind()
                        '結果を返す
                        Result = True

                End Select

            Catch ex As SqlException

                '検索失敗
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "時間外消費")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                '結果を返す
                Result = False
                Throw ex

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "時間外消費")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                '結果を返す
                Result = False
                Throw ex

            Finally

                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Master.ppCount = "0"

        End If

    End Sub

#End Region

#Region "終了処理プロシージャ"

#End Region

End Class
