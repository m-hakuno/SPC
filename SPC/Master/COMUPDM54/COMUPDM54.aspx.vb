'********************************************************************************************************************************
'*　システム　：　サポートセンタシステム　＜共通＞
'*　処理名　　：　持参物品マスタ一覧
'*　ＰＧＭＩＤ：　COMUPDM54
'*                                                                                                  CopyRight SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.04.27  ：　栗原
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive
#End Region

Public Class COMUPDM54
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
    Const DispCode As String = "COMUPDM54"                  '画面ID
    Const DispCodeTo As String = "COMUPDM55"
    Const MasterName As String = "持参物品マスタ一覧"           '画面名
    Const TableName As String = "M54_BRING_ARTICLE"        'テーブル名

    '次画面ファイルパス
    Const M_MST_DISP_PATH = "~/" & P_MST & "/" &
            P_FUN_COM & P_SCR_UPD & "M" & "55" & "/" &
            P_FUN_COM & P_SCR_UPD & "M" & "55.aspx"

#End Region

#Region "変数定義"
    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsExc As New ClsCMExclusive
    Dim clsMst As New ClsMSTCommon
    Dim objStack As StackFrame
    Dim strMode As String = Nothing
    Dim stb As New StringBuilder
#End Region

#Region "イベントプロシージャ"
    ''' <summary>
    ''' Page_Init
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        'Dim intHeadCol As Integer() = New Integer() {1} 'グリッドXML定義ファイル取得　ヘッダ修正パラメータ
        'Dim intColSpan As Integer() = New Integer() {2}
        'pfSet_GridView(Me.grvList, DispCode, intHeadCol, intColSpan)
        pfSet_GridView(Me.grvList, DispCode)
    End Sub

    ''' <summary>
    ''' Page_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim btnNew As Button = DirectCast(Master.FindControl("btnLeft1"), Button)
        'ボタンアクション設定
        AddHandler Master.ppBtnSearch.Click, AddressOf btnSearch_Click          '検索
        AddHandler Master.ppBtnSrcClear.Click, AddressOf btnSearchClear_Click   '検索条件クリア
        AddHandler btnNew.Click, AddressOf btn_New_Click                        '新規
        btnNew.Visible = True
        btnNew.Text = "新規"


        'ドロップダウンリストのイベント設定
        AddHandler ddlSystem.SelectedIndexChanged, AddressOf ddlSystem_SelectedIndexChanged '編集エリア分類コード
        ddlSystem.AutoPostBack = True
        AddHandler ddlMachine.SelectedIndexChanged, AddressOf ddlMachine_SelectedIndexChanged  '編集エリア分類コード
        ddlMachine.AutoPostBack = True
        AddHandler ddlVer.SelectedIndexChanged, AddressOf ddlVer_SelectedIndexChanged '編集エリア分類コード
        ddlVer.AutoPostBack = True


        If Not IsPostBack Then

            'プログラムＩＤ、画面名、件数設定
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"
            ViewState("strDeleteFlg") = String.Empty
            'パンくずリスト設定
            Master.ppBCList = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

            'ドロップダウンリスト設定
            msSetddlSystem()
            msSetddlMachine()

            'グリッドの初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

            'フォーカス設定
            SetFocus(ddlSystem.ClientID)
            strMode = "Default"

            '初期表示はバージョン非活性
            ddlVer.Enabled = False
        End If
    End Sub

    ''' <summary>
    ''' Page_PreRender
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

    End Sub

    ''' <summary>
    ''' 検索ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        'データ取得
        If (Page.IsValid) Then
            msGet_Data()

        End If
        'フォーカス設定
        SetFocus(ddlSystem.ClientID)
        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 検索条件クリアボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearchClear_Click(sender As Object, e As EventArgs)
        'ログ出力開始
        psLogStart(Me)

        '検索項目をリセット
        ddlSystem.SelectedIndex = -1
        ddlMachine.SelectedIndex = -1
        ddlVer.Items.Clear()
        ddlVer.SelectedIndex = -1
        ddlVer.Enabled = False

        'フォーカス設定
        SetFocus(ddlSystem.ClientID)

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 新規ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_New_Click(sender As Object, e As EventArgs)
        'TODO新規ボタン押下処理作る
        dis_change("New")
    End Sub

    ''' <summary>
    '''システム項目変更
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlSystem_SelectedIndexChanged()

        'ログ出力開始
        psLogStart(Me)

        msGet_ExistData("System")

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    '''機器項目変更
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlMachine_SelectedIndexChanged()

        'ログ出力開始
        psLogStart(Me)

        msGet_ExistData("Machine")

        'ログ出力終了
        psLogEnd(Me)

    End Sub
    ''' <summary>
    '''バージョン変更
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlVer_SelectedIndexChanged()

        'ログ出力開始
        psLogStart(Me)

        msGet_ExistData("Ver")

        'ログ出力終了
        psLogEnd(Me)

    End Sub
    ''' <summary>
    '''許容バージョン変更
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlMVer_SelectedIndexChanged()

        'ログ出力開始
        psLogStart(Me)

        msGet_ExistData("MVer")

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    '============================================================================================================================
    '==   GRID関係
    '============================================================================================================================
    ''' <summary>
    ''' Grid_Rowコマンド
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_RowCommand(sender As Object, e As CommandEventArgs) Handles grvList.RowCommand

        If e.CommandName = "Select" Then
            '******************************
            '*          排他処理          *
            '******************************
            Dim rowData As GridViewRow = grvList.Rows(Convert.ToInt32(e.CommandArgument))
            '★排他制御処理
            Dim strExclusiveDate As String = Nothing
            Dim arTable_Name As New ArrayList
            Dim arKey As New ArrayList

            '★排他情報削除
            If Not Me.Master.ppExclusiveDate = String.Empty Then
                If clsExc.pfDel_Exclusive(Me _
                                   , Session(P_SESSION_SESSTION_ID) _
                                   , Me.Master.ppExclusiveDate) = 0 Then
                    Me.Master.ppExclusiveDate = String.Empty
                Else
                    'ログ出力終了
                    psLogEnd(Me)
                    Exit Sub
                End If
            End If

            '★ロック対象テーブル名の登録
            arTable_Name.Insert(0, TableName)

            '★ロックテーブルキー項目の登録
            arKey.Insert(0, CType(rowData.FindControl("システムコード"), TextBox).Text)
            arKey.Insert(0, CType(rowData.FindControl("バージョン"), TextBox).Text)
            arKey.Insert(0, CType(rowData.FindControl("機器コード"), TextBox).Text)

            '★排他情報確認処理(更新画面へ遷移)
            If clsExc.pfSel_Exclusive(strExclusiveDate _
                             , Me _
                             , Session(P_SESSION_IP) _
                             , Session(P_SESSION_PLACE) _
                             , Session(P_SESSION_USERID) _
                             , Session(P_SESSION_SESSTION_ID) _
                             , ViewState(P_SESSION_GROUP_NUM) _
                             , DispCodeTo _
                             , arTable_Name _
                             , arKey) = 0 Then

                '★登録年月日時刻(明細)
                Session("ExclusiveDate") = strExclusiveDate

            Else
                '排他ロック中
                'ログ出力終了
                psLogEnd(Me)
                Exit Sub
            End If

            Session("prm_SysCod") = String.Empty
            Session("prm_AppCod") = String.Empty
            Session("prm_TbxVer") = String.Empty

            Session("prm_SysCod") = CType(rowData.FindControl("システムコード"), TextBox).Text
            Session("prm_AppCod") = CType(rowData.FindControl("機器コード"), TextBox).Text
            Session("prm_TbxVer") = CType(rowData.FindControl("バージョン"), TextBox).Text
            dis_change("Select")
        End If
    End Sub
#End Region

#Region "そのほかのプロシージャ"
    ''' <summary>
    ''' 検索処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim dispFlg As String = String.Empty
        Dim mver As String = String.Empty

        objStack = New StackFrame

        If Me.IsPostBack Then 'ポストバックかどうか判定
            dispFlg = "0"
        Else
            dispFlg = "1"
        End If

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(DispCode & "_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("system", SqlDbType.NVarChar, ddlSystem.SelectedValue))
                    .Add(pfSet_Param("machine", SqlDbType.NVarChar, ddlMachine.SelectedValue))
                    .Add(pfSet_Param("ver", SqlDbType.NVarChar, ddlVer.SelectedValue))
                    .Add(pfSet_Param("prog_id", SqlDbType.NVarChar, DispCode))
                    .Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, dispFlg))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    Master.ppCount = "0"
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)

                    '総件数とデータセット内の件数(閾値制限)の比較
                ElseIf CType(dstOrders.Tables(0).Rows(0).Item("総件数").ToString, Integer) > dstOrders.Tables(0).Rows.Count Then
                    '閾値オーバー
                    Master.ppCount = dstOrders.Tables(0).Rows(0).Item("総件数").ToString
                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                                        dstOrders.Tables(0).Rows(0).Item("総件数").ToString, dstOrders.Tables(0).Rows.Count)
                Else
                    Master.ppCount = dstOrders.Tables(0).Rows(0).Item("総件数").ToString
                End If


                '取得したデータをリストに設定
                Me.grvList.DataSource = dstOrders.Tables(0)
                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
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
    End Sub

    ''' <summary>
    ''' ドロップダウンリスト変更時
    ''' </summary>
    ''' <param name="strKey"></param>
    ''' <remarks></remarks>
    Private Sub msGet_ExistData(ByVal strKey As String)

        'システム変更時はバージョン設定
        If strKey = "System" Then
            If ddlSystem.SelectedValue <> "" Then
                msGetVer()
            Else
                Me.ddlVer.Items.Clear()
                Me.ddlVer.Enabled = False
            End If
        End If

        'フォーカス処理
        Dim afFocusID As String = String.Empty

        Select Case strKey
            Case "System"
                afFocusID = ddlSystem.ClientID
            Case "Machine"
                afFocusID = ddlMachine.ClientID
            Case "Ver"
                afFocusID = ddlVer.ClientID
        End Select
        SetFocus(afFocusID)
    End Sub

    ''' <summary>
    ''' 画面遷移処理
    ''' </summary>
    ''' <param name="CommandName"></param>
    ''' <remarks></remarks>
    Protected Sub dis_change(CommandName As String)

        Dim rowData As GridViewRow = Nothing        'ボタン押下行
        Dim strKeyList As List(Of String) = Nothing '次画面引継ぎ用キー情報

        'セッション情報にバージョンデータを入力
        If CommandName = "New" Then
            Session("prm_SysCod") = String.Empty
            Session("prm_AppCod") = String.Empty
            Session("prm_TbxVer") = String.Empty
        ElseIf CommandName = "Select" Then
        End If

        '開始ログ出力
        psLogStart(Me)

        'パンクズリストの設定
        Session(P_SESSION_BCLIST) = Master.ppBCList

        '持参物品マスタ登録に遷移
        psOpen_Window(Me, M_MST_DISP_PATH)

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' ドロップダウンリストシステム取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlSystem()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZMSTSEL004", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                '検索結果
                Me.ddlSystem.Items.Clear()
                Me.ddlSystem.DataSource = objDs.Tables(0)
                Me.ddlSystem.DataTextField = "ＴＢＯＸリスト"
                Me.ddlSystem.DataValueField = "ＴＢＯＸシステムコード"
                Me.ddlSystem.DataBind()
                'Me.ddlSystem.Items.Insert(0, "**:ダミー") 'ダミーを追加
                'Me.ddlSystem.Items(0).Value = "**"
                Me.ddlSystem.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ機種マスタ一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト機器取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlMachine()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(DispCode & "_S3", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                '検索結果
                Me.ddlMachine.Items.Clear()
                Me.ddlMachine.DataSource = objDs.Tables(0)
                Me.ddlMachine.DataTextField = "機器"
                Me.ddlMachine.DataValueField = "機器コード"
                Me.ddlMachine.DataBind()
                Me.ddlMachine.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' システム変更時のバージョン設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGetVer()
        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        If Not ddlSystem.SelectedIndex = 0 Then
            'DB接続
            If Not clsDataConnect.pfOpen_Database(objCn) Then
                clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else
                Try
                    objCmd = New SqlCommand(DispCode & "_S2", objCn)
                    objCmd.Parameters.Add(pfSet_Param("system", SqlDbType.NVarChar, ddlSystem.SelectedValue))
                    'データ取得
                    objDs = clsDataConnect.pfGet_DataSet(objCmd)

                    If objDs.Tables(0).Rows.Count > 0 Then
                        'ドロップダウンリスト設定
                        Me.ddlVer.Items.Clear()
                        Me.ddlVer.DataSource = objDs.Tables(0)
                        Me.ddlVer.DataTextField = "バージョン"
                        Me.ddlVer.DataBind()
                        Me.ddlVer.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                        'ドロップダウンリスト活性化
                        Me.ddlVer.Enabled = True
                    Else
                        clsMst.psMesBox(Me, "10004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "選択されたＴＢＯＸタイプには、バージョン情報")
                        Me.ddlVer.Items.Clear()
                        Me.ddlVer.Enabled = False
                    End If

                Catch ex As Exception
                    clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "バージョン一覧取得")
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Finally
                    'DB切断
                    If Not clsDataConnect.pfClose_Database(objCn) Then
                        clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                End Try
            End If
        Else
            Me.ddlVer.Items.Clear()
            Me.ddlVer.Enabled = False
        End If

    End Sub

#End Region

End Class
