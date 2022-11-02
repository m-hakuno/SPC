'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　ユーザーマスタ
'*　ＰＧＭＩＤ：　COMUPDM01
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.02.24　：　星野
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

Public Class COMUPDM01

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
    Const DispCode As String = "COMUPDM01"                  '画面ID
    Const MasterName As String = "ユーザーマスタ"           '画面名
    Const TableName As String = "M01_USER"                  'テーブル名

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
        AddHandler txtUserID.ppTextBox.TextChanged, AddressOf txtUserID_TextChanged
        txtUserID.ppTextBox.AutoPostBack = True

        If Not IsPostBack Then
            'プログラムＩＤ、画面名、件数設定
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"

            'パンくずリスト設定
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            '削除データを含むを表示
            Master.ppchksDel.Visible = True

            'ドロップダウンリスト設定
            msSetddlComp()
            msSetddlEmp()

            'グリッドの初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

            'データ取得
            'msGet_Data()

            'フォーカス設定
            SetFocus(txtSUserID.ppTextBox.ClientID)

            strMode = "Default"

        End If

        'グリッドの調整
        grvList.Columns(2).HeaderStyle.CssClass = "GridNoDisp"
        grvList.Columns(1).HeaderStyle.Width = 113
        grvList.Columns(4).HeaderStyle.CssClass = "GridNoDisp"
        grvList.Columns(3).HeaderStyle.Width = 218

    End Sub

    ''' <summary>
    ''' Page PreRender
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        Select Case strMode
            Case "Default"
                Master.ppMainEnabled = True
                msSet_Enabled(False)
                txtUserID.ppEnabled = True
                Master.ppBtnInsert.Enabled = False     '追加
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnDelete.Text = "削除"
            Case "Insert"
                Master.ppMainEnabled = True
                msSet_Enabled(True)
                txtUserID.ppEnabled = False
                Master.ppBtnInsert.Enabled = True      '追加
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnDelete.Text = "削除"
            Case "Select"
                If Master.ppBtnDelete.Text = "削除" Then
                    Master.ppMainEnabled = True
                    Master.ppBtnUpdate.Enabled = True  '更新
                Else
                    Master.ppMainEnabled = False
                    Master.ppBtnUpdate.Enabled = False
                End If
                msSet_Enabled(True)
                txtUserID.ppEnabled = False
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
        End If

        'フォーカス設定
        SetFocus(txtSUserID.ppTextBox.ClientID)

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

        ddlSComp.SelectedIndex = -1
        ddlSEmp.SelectedIndex = -1
        txtSUserID.ppText = String.Empty
        Master.ppchksDel.Checked = False

        'フォーカス設定
        SetFocus(txtSUserID.ppTextBox.ClientID)

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
            SetFocus(txtSUserID.ppTextBox.ClientID)
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

        txtUserID.ppText = String.Empty
        txtPassword.ppText = String.Empty
        ddlComp.SelectedIndex = -1
        ddlEmp.Items.Clear()

        'フォーカス設定
        SetFocus(txtUserID.ppTextBox.ClientID)

        strMode = "Default"

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' ユーザーＩＤ変更時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub txtUserID_TextChanged()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        objStack = New StackFrame

        If txtUserID.ppText.Trim <> String.Empty Then
            '入力チェック
            Page.Validate("key")
            If (Page.IsValid) Then
                '接続
                If clsDataConnect.pfOpen_Database(conDB) Then
                    Try
                        'ドロップダウンリスト（社員）設定
                        msSetddlEmp(True)

                        cmdDB = New SqlCommand(DispCode & "_S2", conDB)

                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("user_id", SqlDbType.NVarChar, txtUserID.ppText.Trim))
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
                            arKey.Insert(0, txtUserID.ppText.Trim)

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

                            '編集エリアに値を設定
                            txtUserID.ppText = dstOrders.Tables(0).Rows(0).Item("ユーザーＩＤ").ToString
                            txtPassword.ppText = dstOrders.Tables(0).Rows(0).Item("パスワード").ToString
                            ddlComp.SelectedValue = dstOrders.Tables(0).Rows(0).Item("会社コード").ToString
                            Dim lstItem As ListItem = ddlEmp.Items.FindByText(dstOrders.Tables(0).Rows(0).Item("社員").ToString)
                            If lstItem Is Nothing Then
                                Me.ddlEmp.Items.Clear()
                                Me.ddlEmp.Items.Insert(0, New ListItem(dstOrders.Tables(0).Rows(0).Item("社員").ToString))
                                clsMst.psMesBox(Me, "30011", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "社員 " & dstOrders.Tables(0).Rows(0).Item("社員").ToString)
                                strMode = "Disable"
                            Else
                                ddlEmp.SelectedValue = dstOrders.Tables(0).Rows(0).Item("社員コード").ToString
                            End If

                            '削除フラグによってボタンの文言を変更
                            If dstOrders.Tables(0).Rows(0).Item("削除フラグ").ToString <> "1" Then
                                Master.ppBtnDelete.Text = "削除"
                                Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '削除
                            Else
                                Master.ppBtnDelete.Text = "削除取消"
                                Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
                            End If

                            If strMode <> "Disable" Then
                                strMode = "Select"
                            End If
                        Else
                            strMode = "Insert"
                        End If

                        If Master.ppBtnDelete.Text = "削除" AndAlso strMode <> "Disable" Then
                            Master.ppBtnDmy.Visible = True
                            Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtPassword.ppTextBox.ClientID + ");"
                            SetFocus(Master.ppBtnDmy.ClientID)
                        Else
                            SetFocus(Master.ppBtnClear.ClientID)
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
            SetFocus(Master.ppBtnClear.ClientID)
        End If

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

                'ドロップダウンリスト（社員）設定
                msSetddlEmp(True)

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
                arKey.Insert(0, CType(rowData.FindControl("ユーザーＩＤ"), TextBox).Text)

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
                txtUserID.ppText = CType(rowData.FindControl("ユーザーＩＤ"), TextBox).Text
                txtPassword.ppText = CType(rowData.FindControl("パスワード"), TextBox).Text
                ddlComp.SelectedValue = CType(rowData.FindControl("会社コード"), TextBox).Text
                Dim lstItem As ListItem = ddlEmp.Items.FindByValue(CType(rowData.FindControl("社員コード"), TextBox).Text)
                If lstItem Is Nothing Then
                    Me.ddlEmp.Items.Clear()
                    Me.ddlEmp.Items.Insert(0, New ListItem(CType(rowData.FindControl("社員コード"), TextBox).Text & ":" & CType(rowData.FindControl("社員"), TextBox).Text))
                    clsMst.psMesBox(Me, "30011", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "社員 " & CType(rowData.FindControl("社員コード"), TextBox).Text & ":" & CType(rowData.FindControl("社員"), TextBox).Text)
                    strMode = "Disable"
                Else
                    ddlEmp.SelectedValue = CType(rowData.FindControl("社員コード"), TextBox).Text
                End If

                '削除フラグによってボタンの文言を変更
                If CType(rowData.FindControl("削除"), TextBox).Text = "" Then
                    Master.ppBtnDelete.Text = "削除"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '削除
                Else
                    Master.ppBtnDelete.Text = "削除取消"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
                End If

                'フォーカス設定
                If Master.ppBtnDelete.Text = "削除" AndAlso strMode <> "Disable" Then
                    SetFocus(txtPassword.ppTextBox.ClientID)
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

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 編集エリア活性制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_Enabled(ByVal bool As Boolean)

        txtUserID.ppEnabled = bool
        txtPassword.ppEnabled = bool
        ddlComp.Enabled = bool
        ddlEmp.Enabled = bool

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
                If Master.ppchksDel.Checked Then
                    dltFlg = "1"
                Else
                    dltFlg = "0"
                End If
                cmdDB = New SqlCommand(DispCode & "_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("user_id", SqlDbType.NVarChar, txtSUserID.ppText.Trim))
                    .Add(pfSet_Param("comp_cd", SqlDbType.NVarChar, ddlSComp.SelectedValue))
                    .Add(pfSet_Param("emply_cd", SqlDbType.NVarChar, ddlSEmp.SelectedValue))
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
                        procCls = "0"
                        strStored = DispCode & "_D1"
                    Case "削除取消"
                        MesCode = "00001"
                        procCls = "1"
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
                            .Add(pfSet_Param("user_id", SqlDbType.NVarChar, txtUserID.ppText.Trim))              'ユーザーＩＤ
                            .Add(pfSet_Param("password", SqlDbType.NVarChar, txtPassword.ppText.Trim))           'パスワード
                            .Add(pfSet_Param("comp_cd", SqlDbType.NVarChar, ddlComp.SelectedValue))              '会社コード
                            .Add(pfSet_Param("emply_cd", SqlDbType.NVarChar, ddlEmp.SelectedValue))              '社員コード
                            .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))                           '処理区分
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                  'ユーザーＩＤ
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                    '戻り値
                        End With
                    Case "DELETE"
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("user_id", SqlDbType.NVarChar, txtUserID.ppText.Trim))              'ユーザーＩＤ
                            .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))                           '処理区分
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
                        psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                        Exit Sub
                    End If

                    'コミット
                    conTrn.Commit()
                End Using

                '追加、更新の場合は対象のレコードのみグリッドに表示
                'If ipstrMode = "INSERT" OrElse ipstrMode = "UPDATE" Then
                If ipstrMode = "INSERT" OrElse ipstrMode = "UPDATE" OrElse Master.ppBtnDelete.Text = "削除取消" Then
                    dttGrid.Columns.Add("ユーザーＩＤ")
                    dttGrid.Columns.Add("パスワード")
                    dttGrid.Columns.Add("会社コード")
                    dttGrid.Columns.Add("会社")
                    dttGrid.Columns.Add("社員コード")
                    dttGrid.Columns.Add("社員")
                    dttGrid.Columns.Add("削除")
                    drData = dttGrid.NewRow()
                    drData.Item("ユーザーＩＤ") = txtUserID.ppText.Trim
                    drData.Item("パスワード") = txtPassword.ppText.Trim
                    drData.Item("会社コード") = ddlComp.SelectedValue
                    drData.Item("会社") = ddlComp.SelectedItem.ToString.Split(":")(1)
                    drData.Item("社員コード") = ddlEmp.SelectedValue
                    drData.Item("社員") = ddlEmp.SelectedItem.ToString.Split(":")(1)
                    drData.Item("削除") = String.Empty
                    dttGrid.Rows.Add(drData)
                Else
                    'dttGrid = pfParse_DataTable(Me.grvList)
                    'With dttGrid
                    '    For zz = 0 To .Rows.Count - 1
                    '        If txtUserID.ppText.Trim = .Rows(zz).Item("ユーザーＩＤ") Then
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
    ''' ドロップダウンリスト設定（会社）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlComp()

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
                '検索条件
                Me.ddlSComp.Items.Clear()
                Me.ddlSComp.DataSource = objDs.Tables(0)
                Me.ddlSComp.DataTextField = "会社名"
                Me.ddlSComp.DataValueField = "連番"
                Me.ddlSComp.DataBind()
                Me.ddlSComp.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                '検索結果（明細）
                Me.ddlComp.Items.Clear()
                Me.ddlComp.DataSource = objDs.Tables(0)
                Me.ddlComp.DataTextField = "会社名"
                Me.ddlComp.DataValueField = "連番"
                Me.ddlComp.DataBind()
                Me.ddlComp.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "業者マスタ取得")
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
    ''' ドロップダウンリスト設定（社員）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlEmp(Optional ByVal blnEdit As Boolean = False)

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(DispCode & "_S4", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                '検索条件
                If blnEdit = False Then
                    Me.ddlSEmp.Items.Clear()
                    Me.ddlSEmp.DataSource = objDs.Tables(0)
                    Me.ddlSEmp.DataTextField = "社員名"
                    Me.ddlSEmp.DataValueField = "社員コード"
                    Me.ddlSEmp.DataBind()
                    Me.ddlSEmp.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                End If
                '検索結果（明細）
                Me.ddlEmp.Items.Clear()
                Me.ddlEmp.DataSource = objDs.Tables(1)
                Me.ddlEmp.DataTextField = "社員名"
                Me.ddlEmp.DataValueField = "社員コード"
                Me.ddlEmp.DataBind()
                Me.ddlEmp.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "社員マスタ一覧取得")
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
    ''' 入力チェック（会社）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub cuvComp_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvComp.ServerValidate
        Dim strErrNo As String
        Dim dtrMes As DataRow
        'エラーチェック
        strErrNo = pfCheck_ListErr(args.Value, True)

        If strErrNo <> String.Empty Then
            'エラー
            dtrMes = pfGet_ValMes(strErrNo, "会社")
            cuvComp.Text = dtrMes.Item(P_VALMES_SMES)
            cuvComp.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            args.IsValid = False
            Exit Sub
        End If

    End Sub

    ''' <summary>
    ''' 入力チェック（社員）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub cuvEmp_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvEmp.ServerValidate
        Dim strErrNo As String
        Dim dtrMes As DataRow
        'エラーチェック
        strErrNo = pfCheck_ListErr(args.Value, True)

        If strErrNo <> String.Empty Then
            'エラー
            dtrMes = pfGet_ValMes(strErrNo, "社員")
            cuvEmp.Text = dtrMes.Item(P_VALMES_SMES)
            cuvEmp.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            args.IsValid = False
            Exit Sub
        End If

    End Sub

#End Region

#Region "終了処理プロシージャ"

#End Region

End Class
