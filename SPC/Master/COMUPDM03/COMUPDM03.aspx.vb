'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　ＴＢＯＸマスタ
'*　ＰＧＭＩＤ：　COMUPDM03
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.03.09　：　星野
'*  変　更    ：　2015.05.22　：　栗原
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive

Public Class COMUPDM03

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
    Const DispCode As String = "COMUPDM03"                  '画面ID
    Const MasterName As String = "ＴＢＯＸバージョンマスタ" '画面名
    Const TableName As String = "M03_TBOX"                  'テーブル名

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

#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page Init
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, DispCode)
    End Sub

    ''' <summary>
    ''' Page Load
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        'ボタンアクション設定
        AddHandler Master.ppBtnSearch.Click, AddressOf btnSearch_Click          '検索
        AddHandler Master.ppBtnSrcClear.Click, AddressOf btnSearchClear_Click   '検索条件クリア
        AddHandler Master.ppBtnInsert.Command, AddressOf btn_Click              '登録
        AddHandler Master.ppBtnUpdate.Command, AddressOf btn_Click              '更新
        AddHandler Master.ppBtnDelete.Command, AddressOf btn_Click              '削除
        AddHandler Master.ppBtnClear.Click, AddressOf btnClear_Click            'クリア

        'テキストボックスのイベント設定
        AddHandler txtVer.ppTextBox.TextChanged, AddressOf txtVer_TextChanged
        txtVer.ppTextBox.AutoPostBack = True
        'エラーサマリー活性制御の為のUpdatePanelのUPdateMode調整
        Dim pnl As UpdatePanel
        pnl = Master.FindControl("UpdPanelMain")
        pnl.UpdateMode = UpdatePanelUpdateMode.Always

        If Not IsPostBack Then
            'プログラムＩＤ、画面名、件数設定
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"

            '削除ボタン非表示
            Master.ppBtnDelete.Visible = False


            'パンくずリスト設定
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            'ドロップダウンリスト設定
            msSetddlSystem()
            msSetddlDispSys()
            msSetddlLineCls()

            'グリッドの初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

            'データ取得
            msGet_Data()

            'フォーカス設定
            SetFocus(ddlSSystem.ClientID)

            strMode = "Default"

        End If

        'グリッド調整
        grvList.Columns(2).HeaderStyle.CssClass = "GridNoDisp"
        grvList.Columns(1).HeaderStyle.Width = 117
        grvList.Columns(7).HeaderStyle.CssClass = "GridNoDisp"
        grvList.Columns(6).HeaderStyle.Width = 153
        grvList.Columns(9).HeaderStyle.CssClass = "GridNoDisp"
        grvList.Columns(8).HeaderStyle.Width = 103


    End Sub

    ''' <summary>
    ''' Page PreRender
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        msSetVal()
        Select Case strMode
            Case "Default"
                Master.ppMainEnabled = True
                msSet_Enabled(False)
                ddlSystem.Enabled = True
                txtVer.ppEnabled = True
                Master.ppBtnInsert.Enabled = False     '追加
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnDelete.Text = "削除"
            Case "Insert"
                Master.ppMainEnabled = True
                msSet_Enabled(True)
                ddlSystem.Enabled = False
                txtVer.ppEnabled = False
                Master.ppBtnInsert.Enabled = True      '追加
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnDelete.Text = "削除"
            Case "Select"
                msSet_Enabled(False)
                If Master.ppBtnDelete.Text = "削除" Then
                    Master.ppMainEnabled = True
                    Master.ppBtnUpdate.Enabled = True  '更新
                    txtTboxVer.ppEnabled = True
                    txtType.ppEnabled = True
                Else
                    Master.ppMainEnabled = False
                    Master.ppBtnUpdate.Enabled = False
                End If

                ddlDispSysCls.Enabled = True
                ddlLineCls.Enabled = True
                Master.ppBtnInsert.Enabled = False     '追加
                Master.ppBtnDelete.Enabled = True      '削除
                Master.ppBtnClear.Enabled = True       'クリア
            Case "Disable"
                Master.ppMainEnabled = False
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnInsert.Enabled = False     '追加
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnClear.Enabled = True       'クリア
        End Select

    End Sub

    ''' <summary>
    '''検索ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        'データ取得
        If (Page.IsValid) Then
            msGet_Data()
            'フォーカス設定
            SetFocus(ddlSSystem.ClientID)
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    '''検索条件クリア
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnSearchClear_Click(sender As Object, e As EventArgs)
        'ログ出力開始
        psLogStart(Me)

        ddlSSystem.SelectedIndex = -1
        ddlSSystemCls.ppDropDownList.SelectedIndex = -1
        ddldel.ppDropDownList.SelectedIndex = -1

        'フォーカス設定
        SetFocus(ddlSSystem.ClientID)

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 追加/更新/削除ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btn_Click(sender As Object, e As CommandEventArgs)

        'ログ出力開始
        psLogStart(Me)

        If (Page.IsValid) Then
            msEditData(e.CommandName)
            'フォーカス設定
            SetFocus(ddlSSystem.ClientID)
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' クリアボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click()

        'ログ出力開始
        psLogStart(Me)

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

        'ドロップダウンリスト設定
        msSetddlSystem(True)
        msSetddlDispSys()
        msSetddlLineCls()

        ddlSystem.SelectedIndex = -1
        txtVer.ppText = String.Empty
        lblSystemCls.Text = String.Empty
        ddlDispSysCls.SelectedIndex = -1
        txtShortVer.ppText = String.Empty
        ddlLineCls.SelectedIndex = -1
        txtTboxVer.ppText = String.Empty
        txtType.ppText = String.Empty

        'フォーカス設定
        SetFocus(ddlSystem.ClientID)

        strMode = "Default"

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' システム変更時
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ddlSystem_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles ddlSystem.SelectedIndexChanged

        'ログ出力開始
        psLogStart(Me)

        msGet_ExistData("System")
        msGet_SysCls()

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 正式バージョン変更時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub txtVer_TextChanged()

        'ログ出力開始
        psLogStart(Me)

        msGet_ExistData("Ver")

        'ログ出力終了
        psLogEnd(Me)

    End Sub


    '============================================================================================================================
    '==   GRID関係
    '============================================================================================================================

    ''' <summary>
    ''' GRID RowCommand
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub grvList_RowCommand(sender As Object, e As CommandEventArgs) Handles grvList.RowCommand

        Try
            If e.CommandName = "Select" Then

                'ログ出力開始
                psLogStart(Me)

                'ドロップダウンリスト設定
                msSetddlSystem(True)
                msSetddlDispSys()
                msSetddlLineCls()

                '★排他制御処理
                Dim strExclusiveDate As String = Nothing
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList
                Dim rowData As GridViewRow = grvList.Rows(Convert.ToInt32(e.CommandArgument))

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
                arKey.Insert(0, CType(rowData.FindControl("正式バージョン"), TextBox).Text)
                arKey.Insert(0, CType(rowData.FindControl("システム"), TextBox).Text)

                '★排他情報確認処理(更新画面へ遷移)
                If clsExc.pfSel_Exclusive(strExclusiveDate _
                                 , Me _
                                 , Session(P_SESSION_IP) _
                                 , Session(P_SESSION_PLACE) _
                                 , Session(P_SESSION_USERID) _
                                 , Session(P_SESSION_SESSTION_ID) _
                                 , ViewState(P_SESSION_GROUP_NUM) _
                                 , DispCode _
                                 , arTable_Name _
                                 , arKey) = 0 Then

                    '★登録年月日時刻(明細)
                    Me.Master.ppExclusiveDate = strExclusiveDate

                Else
                    '排他ロック中
                    'ログ出力終了
                    psLogEnd(Me)
                    Exit Sub
                End If

                '編集エリアに値を設定
                Dim lstItem As ListItem = ddlSystem.Items.FindByValue(CType(rowData.FindControl("システム"), TextBox).Text)
                If lstItem Is Nothing Then
                    ddlSystem.Items(0).Value = CType(rowData.FindControl("システム"), TextBox).Text
                    ddlSystem.Items(0).Text = CType(rowData.FindControl("システム"), TextBox).Text & ":" & CType(rowData.FindControl("システム名"), TextBox).Text
                    clsMst.psMesBox(Me, "30011", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "システム " & CType(rowData.FindControl("システム"), TextBox).Text & ":" & CType(rowData.FindControl("システム名"), TextBox).Text)
                    strMode = "Disable"
                Else
                    ddlSystem.SelectedValue = CType(rowData.FindControl("システム"), TextBox).Text
                End If
                txtVer.ppText = CType(rowData.FindControl("正式バージョン"), TextBox).Text
                lblSystemCls.Text = CType(rowData.FindControl("システム分類"), TextBox).Text
                lstItem = ddlDispSysCls.Items.FindByValue(CType(rowData.FindControl("表示用システム分類"), TextBox).Text)
                If lstItem Is Nothing Then
                    ddlDispSysCls.Items(0).Value = CType(rowData.FindControl("表示用システム分類"), TextBox).Text
                    ddlDispSysCls.Items(0).Text = CType(rowData.FindControl("表示用システム分類"), TextBox).Text & ":" & CType(rowData.FindControl("表示用システム分類名"), TextBox).Text
                Else
                    ddlDispSysCls.SelectedValue = CType(rowData.FindControl("表示用システム分類"), TextBox).Text
                End If
                txtShortVer.ppText = CType(rowData.FindControl("略式バージョン"), TextBox).Text
                lstItem = ddlLineCls.Items.FindByValue(CType(rowData.FindControl("有線無線区分"), TextBox).Text)
                If lstItem Is Nothing Then
                    ddlLineCls.Items(0).Value = CType(rowData.FindControl("有線無線区分"), TextBox).Text
                    ddlLineCls.Items(0).Text = CType(rowData.FindControl("有線無線区分"), TextBox).Text & ":" & CType(rowData.FindControl("有線無線区分名"), TextBox).Text
                Else
                    ddlLineCls.SelectedValue = CType(rowData.FindControl("有線無線区分"), TextBox).Text
                End If
                txtTboxVer.ppText = CType(rowData.FindControl("TBOXタイプ(Ver込み)"), TextBox).Text
                txtType.ppText = CType(rowData.FindControl("現行システム"), TextBox).Text

                '削除フラグによってボタンの文言を変更
                If CType(rowData.FindControl("削除フラグ"), TextBox).Text <> "1" Then
                    Master.ppBtnDelete.Text = "削除"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '削除
                Else
                    Master.ppBtnDelete.Text = "削除取消"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
                End If

                'フォーカス設定
                If Master.ppBtnDelete.Text = "削除" Then
                    SetFocus(ddlDispSysCls.ClientID)
                Else
                    SetFocus(Master.ppBtnClear.ClientID)
                End If

                If strMode <> "Disable" Then
                    strMode = "Select"
                End If
            End If

        Catch ex As Exception
            'エラー
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
            objStack = New StackFrame
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Private Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        '削除済みデータを赤文字表示に
        For zz = 0 To grvList.Rows.Count - 1
            If CType(grvList.Rows(zz).FindControl("削除フラグ"), TextBox).Text = "1" Then
                For yy = 1 To grvList.Columns.Count - 1
                    CType(grvList.Rows(zz).FindControl(grvList.Columns(yy).HeaderText), TextBox).ForeColor = Drawing.Color.Red
                Next
            End If
        Next

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 編集エリア活性制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_Enabled(ByVal bool As Boolean)

        ddlSystem.Enabled = bool
        txtVer.ppEnabled = bool
        ddlDispSysCls.Enabled = bool
        txtShortVer.ppEnabled = bool
        ddlLineCls.Enabled = bool
        txtTboxVer.ppEnabled = bool
        txtType.ppEnabled = bool


    End Sub

    ''' <summary>
    ''' 検索処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim dispFlg As String = String.Empty
        Dim dltFlg As String = String.Empty
        objStack = New StackFrame

        If Me.IsPostBack Then
            dispFlg = "0"
        Else
            dispFlg = "1"
        End If

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                'If ddldel.ppDropDownList.SelectedValue = "1" Then
                '    dltFlg = "1"
                'ElseIf Me.IsPostBack = False OrElse ddldel.ppDropDownList.SelectedValue = "0" Then
                '    dltFlg = "0"
                'Else
                '    dltFlg = ""
                'End If
                '2015/07/22　修正:削除区分非表示
                dltFlg = "0"

                cmdDB = New SqlCommand(DispCode & "_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ddlSSystem.SelectedValue))
                    .Add(pfSet_Param("system_cls", SqlDbType.NVarChar, ddlSSystemCls.ppDropDownList.SelectedValue))
                    .Add(pfSet_Param("delete_flg", SqlDbType.NVarChar, dltFlg))
                    .Add(pfSet_Param("prog_id", SqlDbType.NVarChar, DispCode))
                    .Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, dispFlg))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    Master.ppCount = "0"
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                ElseIf CType(dstOrders.Tables(0).Rows(0).Item("総件数").ToString, Integer) > dstOrders.Tables(0).Rows.Count Then
                    '上限オーバー
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
    ''' 既存データ取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_ExistData(ByVal strKey As String)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        objStack = New StackFrame

        If ddlSystem.SelectedIndex > 0 AndAlso txtVer.ppText.Trim <> String.Empty Then
            '入力チェック
            Page.Validate("key")
            If (Page.IsValid) Then
                '接続
                If clsDataConnect.pfOpen_Database(conDB) Then
                    Try

                        cmdDB = New SqlCommand(DispCode & "_S2", conDB)

                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ddlSystem.SelectedValue))
                            .Add(pfSet_Param("ver", SqlDbType.NVarChar, txtVer.ppText.Trim))
                        End With

                        'リストデータ取得
                        dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                        '既に存在している場合は各項目に値を設定
                        If dstOrders.Tables(0).Rows.Count > 0 Then

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
                            arKey.Insert(0, txtVer.ppText.Trim)
                            arKey.Insert(0, ddlSystem.SelectedValue)

                            '★排他情報確認処理(更新画面へ遷移)
                            If clsExc.pfSel_Exclusive(strExclusiveDate _
                                             , Me _
                                             , Session(P_SESSION_IP) _
                                             , Session(P_SESSION_PLACE) _
                                             , Session(P_SESSION_USERID) _
                                             , Session(P_SESSION_SESSTION_ID) _
                                             , ViewState(P_SESSION_GROUP_NUM) _
                                             , DispCode _
                                             , arTable_Name _
                                             , arKey) = 0 Then

                                '★登録年月日時刻(明細)
                                Me.Master.ppExclusiveDate = strExclusiveDate

                            Else
                                '排他ロック中
                                clsMst.psMesBox(Me, "30010", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)
                                'ログ出力終了
                                psLogEnd(Me)
                                Exit Sub
                            End If

                            'ドロップダウンリスト設定
                            msSetddlSystem(True)
                            msSetddlDispSys()
                            msSetddlLineCls()

                            '編集エリアに値を設定
                            With dstOrders.Tables(0).Rows(0)
                                Dim lstItem As ListItem = ddlSystem.Items.FindByValue(.Item("システム").ToString)
                                If lstItem Is Nothing Then
                                    ddlSystem.Items(0).Value = .Item("システム").ToString
                                    ddlSystem.Items(0).Text = .Item("システム").ToString & ":" & .Item("システム名").ToString
                                    clsMst.psMesBox(Me, "30011", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "システム " & .Item("システム").ToString & ":" & .Item("システム名").ToString)
                                    strMode = "Disable"
                                Else
                                    ddlSystem.SelectedValue = .Item("システム").ToString
                                End If
                                txtVer.ppText = .Item("正式バージョン").ToString
                                lblSystemCls.Text = .Item("システム分類").ToString
                                lstItem = ddlDispSysCls.Items.FindByValue(.Item("表示用システム分類").ToString)
                                If lstItem Is Nothing Then
                                    ddlDispSysCls.Items(0).Value = .Item("表示用システム分類").ToString
                                    ddlDispSysCls.Items(0).Text = .Item("表示用システム分類").ToString & ":" & .Item("表示用システム分類名").ToString
                                Else
                                    ddlDispSysCls.SelectedValue = .Item("表示用システム分類").ToString
                                End If
                                txtShortVer.ppText = .Item("略式バージョン").ToString
                                lstItem = ddlLineCls.Items.FindByValue(.Item("有線無線区分").ToString)
                                If lstItem Is Nothing Then
                                    ddlLineCls.Items(0).Value = .Item("有線無線区分").ToString
                                    ddlLineCls.Items(0).Text = .Item("有線無線区分").ToString & ":" & .Item("有線無線区分名").ToString
                                Else
                                    ddlLineCls.SelectedValue = .Item("有線無線区分").ToString
                                End If
                                txtTboxVer.ppText = .Item("TBOXタイプ(Ver込み)").ToString
                                txtType.ppText = .Item("現行システム").ToString

                                '削除フラグによってボタンの文言を変更
                                If .Item("削除フラグ").ToString <> "1" Then
                                    Master.ppBtnDelete.Text = "削除"
                                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '削除
                                Else
                                    Master.ppBtnDelete.Text = "削除取消"
                                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
                                End If

                                If strMode <> "Disable" Then
                                    strMode = "Select"
                                End If
                            End With
                        Else
                            strMode = "Insert"
                        End If

                    Catch ex As Exception
                        clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    Finally
                        'DB切断
                        If Not clsDataConnect.pfClose_Database(conDB) Then
                            clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        End If
                    End Try
                Else
                    clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End If
        Else
            strMode = "Default"
        End If

        Dim afFocusID As String = String.Empty
        If Master.ppBtnDelete.Text = "削除" Then
            If strMode = "Select" Then
                afFocusID = ddlDispSysCls.ClientID
            Else
                If ddlSystem.SelectedValue <> "" AndAlso txtVer.ppText.Trim <> String.Empty Then
                    afFocusID = txtShortVer.ppTextBox.ClientID
                Else
                    If strKey = "System" Then
                        afFocusID = ddlSystem.ClientID
                    Else
                        afFocusID = Master.ppBtnClear.ClientID
                    End If
                End If
            End If
            Master.ppBtnDmy.Visible = True
            Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + afFocusID + ");"
            SetFocus(Master.ppBtnDmy.ClientID)
        Else
            SetFocus(Master.ppBtnClear.ClientID)
        End If

    End Sub

    ''' <summary>
    ''' 追加/更新/削除処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msEditData(ByVal ipstrMode As String)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim intRtn As Integer
        Dim MesCode As String = String.Empty
        Dim procCls As String = String.Empty
        Dim strStored As String = String.Empty
        Dim dttGrid As New DataTable
        Dim drData As DataRow
        objStack = New StackFrame

        Select Case ipstrMode
            Case "INSERT"
                MesCode = "00003"
                procCls = "0"
                strStored = DispCode & "_U1"
            Case "UPDATE"
                MesCode = "00001"
                procCls = "1"
                strStored = DispCode & "_U1"
            Case "DELETE"
                Select Case Master.ppBtnDelete.Text
                    Case "削除"
                        MesCode = "00002"
                        strStored = DispCode & "_D1"
                    Case "削除取消"
                        MesCode = "00001"
                        strStored = DispCode & "_D1"
                End Select
        End Select

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(strStored, conDB)
                Select Case ipstrMode
                    Case "INSERT", "UPDATE"
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ddlSystem.SelectedValue))                             'システムコード
                            .Add(pfSet_Param("system_nm", SqlDbType.NVarChar, ddlSystem.SelectedItem.ToString.Split(":")(1)))       'システム名
                            .Add(pfSet_Param("version", SqlDbType.NVarChar, txtVer.ppText.Trim))                                    '正式バージョン
                            .Add(pfSet_Param("short_ver", SqlDbType.NVarChar, txtShortVer.ppText.Trim))                             '略式バージョン
                            .Add(pfSet_Param("disp_sysCls", SqlDbType.NVarChar, ddlDispSysCls.SelectedValue))        '表示用システム分類
                            .Add(pfSet_Param("line_cls", SqlDbType.NVarChar, ddlLineCls.SelectedValue))              '有線無線区分
                            .Add(pfSet_Param("tbox_ver", SqlDbType.NVarChar, txtTboxVer.ppText.Trim))                               'TBOX種別(Ver込)
                            .Add(pfSet_Param("type", SqlDbType.NVarChar, txtType.ppText.Trim))                                      'タイプ
                            .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))                                              '処理区分
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                                     'ユーザーＩＤ
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                                       '戻り値
                        End With
                    Case "DELETE"
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ddlSystem.SelectedValue))          'システムコード
                            .Add(pfSet_Param("version", SqlDbType.NVarChar, txtVer.ppText.Trim))                 '正式バージョン
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                  'ユーザーＩＤ
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                    '戻り値
                        End With
                End Select

                'データ登録/更新/削除
                Using conTrn = conDB.BeginTransaction
                    cmdDB.Transaction = conTrn

                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        conTrn.Rollback()
                        If intRtn = 1 Then
                            psMesBox(Me, "30010", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "登録最大件数までデータ", "データを整理するまで新規登録は")
                        Else
                            psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                        End If
                        Exit Sub
                    End If

                    'コミット
                    conTrn.Commit()
                End Using

                '追加、更新の場合は対象のレコードのみグリッドに表示
                'If ipstrMode = "INSERT" OrElse ipstrMode = "UPDATE" Then
                If ipstrMode = "INSERT" OrElse ipstrMode = "UPDATE" OrElse Master.ppBtnDelete.Text = "削除取消" Then
                    dttGrid.Columns.Add("システム")
                    dttGrid.Columns.Add("システム名")
                    dttGrid.Columns.Add("正式バージョン")
                    dttGrid.Columns.Add("略式バージョン")
                    dttGrid.Columns.Add("システム分類")
                    dttGrid.Columns.Add("表示用システム分類")
                    dttGrid.Columns.Add("表示用システム分類名")
                    dttGrid.Columns.Add("有線無線区分")
                    dttGrid.Columns.Add("有線無線区分名")
                    dttGrid.Columns.Add("TBOXタイプ(Ver込み)")
                    dttGrid.Columns.Add("現行システム")
                    dttGrid.Columns.Add("削除フラグ")
                    drData = dttGrid.NewRow()
                    drData.Item("システム") = ddlSystem.SelectedValue
                    drData.Item("システム名") = ddlSystem.SelectedItem.ToString.Split(":")(1)
                    drData.Item("正式バージョン") = txtVer.ppText.Trim
                    drData.Item("略式バージョン") = txtShortVer.ppText.Trim
                    drData.Item("システム分類") = lblSystemCls.Text.Trim
                    If ddlDispSysCls.SelectedValue <> "" Then
                        drData.Item("表示用システム分類") = ddlDispSysCls.SelectedValue
                        drData.Item("表示用システム分類名") = ddlDispSysCls.SelectedItem.ToString.Split(":")(1)
                    Else
                        drData.Item("表示用システム分類") = String.Empty
                        drData.Item("表示用システム分類名") = String.Empty
                    End If
                    If ddlLineCls.SelectedValue <> "" Then
                        drData.Item("有線無線区分") = ddlLineCls.SelectedValue
                        drData.Item("有線無線区分名") = ddlLineCls.SelectedItem.ToString.Split(":")(1)
                    Else
                        drData.Item("有線無線区分") = String.Empty
                        drData.Item("有線無線区分名") = String.Empty
                    End If
                    drData.Item("TBOXタイプ(Ver込み)") = txtTboxVer.ppText.Trim
                    drData.Item("現行システム") = txtType.ppText.Trim
                    drData.Item("削除フラグ") = String.Empty
                    dttGrid.Rows.Add(drData)
                Else
                    'dttGrid = pfParse_DataTable(Me.grvList)
                    'With dttGrid
                    '    For zz = 0 To .Rows.Count - 1
                    '        If ddlSystem.SelectedValue = .Rows(zz).Item("システムコード").ToString AndAlso txtVer.ppText = .Rows(zz).Item("正式バージョン") Then
                    '            If Master.ppchksDel.Checked AndAlso Master.ppBtnDelete.Text = "削除" Then
                    '                .Rows(zz).Item("削除") = "●"
                    '            ElseIf Master.ppBtnDelete.Text = "削除取消" Then
                    '                .Rows(zz).Item("削除") = String.Empty
                    '            Else
                    '                .Rows(zz).Delete()
                    '            End If
                    '            Exit For
                    '        End If
                    '    Next
                    'End With
                    dttGrid = New DataTable
                End If

                'データをセット
                dttGrid.AcceptChanges()
                grvList.DataSource = dttGrid
                grvList.DataBind()

                Master.ppCount = dttGrid.Rows.Count

                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName)
                btnClear_Click()

            Catch ex As Exception
                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Sub

    ''' <summary>
    ''' システム分類取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_SysCls()

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

                With objCmd.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ddlSystem.SelectedValue))          'システムコード
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '項目に値設定
                If Not objDs Is Nothing AndAlso objDs.Tables(0).Rows.Count > 0 Then
                    lblSystemCls.Text = objDs.Tables(0).Rows(0).Item("システム分類").ToString
                Else
                    lblSystemCls.Text = String.Empty
                End If

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "システム分類取得")
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
    ''' ドロップダウンリスト設定（システム）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlSystem(Optional ByVal blnEdit As Boolean = False)

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
                '検索条件
                If blnEdit = False Then
                    Me.ddlSSystem.Items.Clear()
                    Me.ddlSSystem.DataSource = objDs.Tables(0)
                    Me.ddlSSystem.DataTextField = "ＴＢＯＸリスト"
                    Me.ddlSSystem.DataValueField = "ＴＢＯＸシステムコード"
                    Me.ddlSSystem.DataBind()
                    Me.ddlSSystem.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                End If
                '検索結果（明細）
                Me.ddlSystem.Items.Clear()
                Me.ddlSystem.DataSource = objDs.Tables(1)
                Me.ddlSystem.DataTextField = "ＴＢＯＸリスト"
                Me.ddlSystem.DataValueField = "ＴＢＯＸシステムコード"
                Me.ddlSystem.DataBind()
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
    ''' ドロップダウンリスト設定（表示用システム分類）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlLineCls()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZMSTSEL005", objCn)

                With objCmd.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("Class", SqlDbType.NVarChar, "0014"))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                Me.ddlDispSysCls.Items.Clear()
                Me.ddlDispSysCls.DataSource = objDs.Tables(0)
                Me.ddlDispSysCls.DataTextField = "リスト用"
                Me.ddlDispSysCls.DataValueField = "コード"
                Me.ddlDispSysCls.DataBind()
                Me.ddlDispSysCls.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "表示用システム分類取得")
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
    ''' ドロップダウンリスト設定（有線無線区分）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlDispSys()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZMSTSEL005", objCn)

                With objCmd.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("Class", SqlDbType.NVarChar, "0010"))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                Me.ddlLineCls.Items.Clear()
                Me.ddlLineCls.DataSource = objDs.Tables(0)
                Me.ddlLineCls.DataTextField = "リスト用"
                Me.ddlLineCls.DataValueField = "コード"
                Me.ddlLineCls.DataBind()
                Me.ddlLineCls.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "有線無線区分取得")
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
    ''' バリデーションサマリーテキスト編集
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetVal()
        Dim cuv As CustomValidator
        cuv = txtVer.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = ClsCMCommon.pfGet_ValMes("4015", txtVer.ppName).Item(P_VALMES_MES).ToString
        'cuv.ErrorMessage = Microsoft.VisualBasic.Replace(cuv.ErrorMessage, "半角文字", " 半角英数 ")
        'cuv.ErrorMessage = Microsoft.VisualBasic.Replace(cuv.ErrorMessage, "正しい形式", "半角英数")

        cuv = txtTboxVer.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = ClsCMCommon.pfGet_ValMes("4015", txtTboxVer.ppName).Item(P_VALMES_MES).ToString
        'cuv.ErrorMessage = Microsoft.VisualBasic.Replace(cuv.ErrorMessage, "半角文字", " 半角英数 ")
        'cuv.ErrorMessage = Microsoft.VisualBasic.Replace(cuv.ErrorMessage, "正しい形式", "半角英数")
        cuv = txtType.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = ClsCMCommon.pfGet_ValMes("4015", txtType.ppName).Item(P_VALMES_MES).ToString
        'cuv.ErrorMessage = Microsoft.VisualBasic.Replace(cuv.ErrorMessage, "半角文字", " 半角英数 ")
        'cuv.ErrorMessage = Microsoft.VisualBasic.Replace(cuv.ErrorMessage, "正しい形式", "半角英数")

        cuv = txtShortVer.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = ClsCMCommon.pfGet_ValMes("4015", txtShortVer.ppName).Item(P_VALMES_MES).ToString
        'cuv.ErrorMessage = Microsoft.VisualBasic.Replace(cuv.ErrorMessage, "半角文字", " 半角英数 ")
        'cuv.ErrorMessage = Microsoft.VisualBasic.Replace(cuv.ErrorMessage, "正しい形式", "半角英数")
    End Sub
#End Region

#Region "終了処理プロシージャ"

#End Region

End Class
