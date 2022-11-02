'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　事象マスタ
'*　ＰＧＭＩＤ：　COMUPDM68
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.11.20　：　栗原
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

Public Class COMUPDM68

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
    Const DispCode As String = "COMUPDM68"                  '画面ID
    Const MasterName As String = "事象マスタ"               '画面名
    Const TableName As String = "M68_PHENOMENON"            'テーブル名

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
    Dim strDeleteFlg As String = String.Empty

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

        'テキストボックスイベント設定
        AddHandler txtCode.ppTextBox.TextChanged, AddressOf txtCode_TextChanged
        txtCode.ppTextBox.AutoPostBack = True
        'テキストボックスイベント設定
        AddHandler tftSCode.ppTextBoxFrom.TextChanged, AddressOf tftSCode_TextChanged
        AddHandler tftSCode.ppTextBoxTo.TextChanged, AddressOf tftSCode_TextChanged
        tftSCode.ppTextBoxFrom.AutoPostBack = True
        tftSCode.ppTextBoxTo.AutoPostBack = True

        'エラーサマリー活性制御の為のUpdatePanelのUPdateMode調整
        Dim pnl As UpdatePanel
        'pnl = Master.FindControl("UpdPanelMain")
        'pnl.UpdateMode = UpdatePanelUpdateMode.Always
        pnl = Master.FindControl("UpdPanelSearch")
        pnl.UpdateMode = UpdatePanelUpdateMode.Always


        If Not IsPostBack Then
            'プログラムＩＤ、画面名、件数設定
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"

            'パンくずリスト設定
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            'グリッドの初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

            'データ取得
            msGet_Data()

            'フォーカス設定
            SetFocus(tftSCode.ppTextBoxFrom.ClientID)

            strMode = "Default"

        End If

    End Sub

    ''' <summary>
    ''' Page PreRender
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        Select Case strMode
            Case "Default"
                txtCode.ppEnabled = True
                txtName.ppEnabled = False
                Master.ppBtnInsert.Enabled = False      '追加
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnDelete.Text = "削除"
                Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)
            Case "Insert"
                txtCode.ppEnabled = False
                txtName.ppEnabled = True
                Master.ppBtnInsert.Enabled = True      '追加
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnDelete.Text = "削除"
                Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)
            Case "Select"
                txtCode.ppEnabled = False
                txtName.ppEnabled = True
                Master.ppBtnInsert.Enabled = False     '追加
                Master.ppBtnUpdate.Enabled = True      '更新
                Master.ppBtnDelete.Enabled = True      '削除
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnDelete.Text = "削除"
                Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)
            Case "Delete"
                txtCode.ppEnabled = False
                txtName.ppEnabled = False
                Master.ppBtnInsert.Enabled = False     '追加
                Master.ppBtnUpdate.Enabled = False      '更新
                Master.ppBtnDelete.Enabled = True      '削除
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnDelete.Text = "削除取消"
                Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")
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
            SetFocus(tftSCode.ppTextBoxFrom.ClientID)
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

        tftSCode.ppFromText = String.Empty
        tftSCode.ppToText = String.Empty
        txtSName.ppText = String.Empty
        ddlDel.ppDropDownList.SelectedIndex = -1

        'フォーカス設定
        SetFocus(tftSCode.ppTextBoxFrom.ClientID)

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
            SetFocus(tftSCode.ppTextBoxFrom.ClientID)
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

        txtCode.ppText = String.Empty
        txtName.ppText = String.Empty
        Page.Validate("key")

        'フォーカス設定
        SetFocus(txtCode.ppTextBox.ClientID)

        strMode = "Default"

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' コード変更時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub tftSCode_TextChanged(sender As Object, e As EventArgs)

        If sender.Text.Trim <> String.Empty Then
            If msIsNum(sender.Text.Trim) Then
                sender.Text = msZero_fill(sender.Text.Trim)
            End If
        End If
        Select Case sender.ClientID
            Case tftSCode.ppTextBoxFrom.ClientID
                SetFocus(tftSCode.ppTextBoxTo.ClientID)
            Case tftSCode.ppTextBoxTo.ClientID
                SetFocus(ddlDel.ppDropDownList.ClientID)
        End Select
    End Sub
    Protected Sub txtCode_TextChanged()

        If txtCode.ppText.Trim <> String.Empty Then
            Page.Validate("key")
            If (Page.IsValid) Then
                txtCode.ppText = msZero_fill(txtCode.ppText.Trim)
                msExistData()
            Else
                strMode = "Default"
                SetFocus(txtCode.ppTextBox.ClientID)
            End If
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
                arKey.Insert(0, CType(rowData.FindControl("コード"), TextBox).Text)

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
                txtCode.ppText = CType(rowData.FindControl("コード"), TextBox).Text
                txtName.ppText = CType(rowData.FindControl("名称"), TextBox).Text
                If CType(rowData.FindControl("削除"), TextBox).Text = "1" Then
                    strMode = "Delete"
                Else
                    strMode = "Select"
                End If
                Page.Validate("key")
                Master.ppBtnDmy.Visible = False

                'フォーカス設定
                SetFocus(txtName.ppTextBox.ClientID)
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
    ''' <summary>
    ''' DataBound
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        '削除データは赤文字で表示
        For i = 0 To grvList.Rows.Count - 1
            For z = 1 To grvList.Columns.Count - 1
                If CType(grvList.Rows(i).FindControl("削除"), TextBox).Text = 1 Then
                    CType(grvList.Rows(i).FindControl(grvList.Columns(z).HeaderText), TextBox).ForeColor = Drawing.Color.Red
                End If
            Next
        Next

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
        objStack = New StackFrame

        If Me.IsPostBack Then
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
                    .Add(pfSet_Param("code_from", SqlDbType.NVarChar, tftSCode.ppFromText.Trim))
                    .Add(pfSet_Param("code_to", SqlDbType.NVarChar, tftSCode.ppToText.Trim))
                    .Add(pfSet_Param("name", SqlDbType.NVarChar, txtSName.ppText.Trim))
                    .Add(pfSet_Param("prog_id", SqlDbType.NVarChar, DispCode))
                    .Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, dispFlg))
                    .Add(pfSet_Param("del_flg", SqlDbType.NVarChar, ddlDel.ppSelectedValue))
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
                            .Add(pfSet_Param("code", SqlDbType.NVarChar, txtCode.ppText.Trim))          'コード
                            .Add(pfSet_Param("name", SqlDbType.NVarChar, txtName.ppText.Trim))          '名称
                            .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))                  '処理区分
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))         'ユーザーＩＤ
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
                        End With
                    Case "DELETE"
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("code", SqlDbType.NVarChar, txtCode.ppText.Trim))          'コード
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))         'ユーザーＩＤ
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
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
                If ipstrMode = "INSERT" OrElse ipstrMode = "UPDATE" OrElse Master.ppBtnDelete.Text = "削除取消" Then
                    dttGrid.Columns.Add("コード")
                    dttGrid.Columns.Add("名称")
                    dttGrid.Columns.Add("削除")
                    drData = dttGrid.NewRow()
                    drData.Item("コード") = txtCode.ppText.Trim
                    drData.Item("名称") = txtName.ppText.Trim
                    drData.Item("削除") = "0"
                    dttGrid.Rows.Add(drData)
                Else
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
    ''' 既存データ取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msExistData()
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        objStack = New StackFrame

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(DispCode & "_S2", conDB)

                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("code", SqlDbType.NVarChar, txtCode.ppText.Trim))
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
                    arKey.Insert(0, txtCode.ppText.Trim)

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
                    txtCode.ppText = dstOrders.Tables(0).Rows(0).Item("コード").ToString
                    txtName.ppText = dstOrders.Tables(0).Rows(0).Item("名称").ToString

                    If dstOrders.Tables(0).Rows(0).Item("削除").ToString = "1" Then
                        strMode = "Delete"
                    Else
                        strMode = "Select"
                    End If
                Else
                    strMode = "Insert"
                End If

                Master.ppBtnDmy.Visible = True
                Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtName.ppTextBox.ClientID + ");"
                SetFocus(Master.ppBtnDmy.ClientID)

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
    End Sub

    ''' <summary>
    ''' ゼロ埋め
    ''' </summary>
    ''' <param name="strNum">ゼロ埋め対象の数字</param>
    ''' <param name="intLength">文字列長</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function msZero_fill(ByVal strNum As String, Optional ByVal intLength As Integer = 2) As String
        Dim intCount As Integer = strNum.Length
        While intCount < intLength
            strNum = "0" & strNum
            intCount += 1
        End While
        Return strNum
    End Function

    ''' <summary>
    ''' 数値チェック
    ''' </summary>
    ''' <param name="strNum"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function msIsNum(ByVal strNum As String) As Boolean
        Dim intTemp As Integer
        If Integer.TryParse(strNum, intTemp) Then
            msIsNum = True
        Else
            msIsNum = False
        End If
    End Function

#End Region

#Region "終了処理プロシージャ"

#End Region

End Class
