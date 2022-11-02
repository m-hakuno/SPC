'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　申告元マスタ
'*　ＰＧＭＩＤ：　COMUPDM69
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2016.08.24　：　栗原
'********************************************************************************************************************************

#Region "インポート定義"

Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive

#End Region


Public Class COMUPDM69

#Region "継承定義"
    Inherits System.Web.UI.Page

#End Region

#Region "定数定義"

    Const DispCode As String = "COMUPDM69"
    Const MasterName As String = "申告元マスタ"
    Const TableName As String = "M69_REPORT_SOURCE"

    'コードの入力可能桁数（ゼロ埋めで使用）
    Const CodeLength As Integer = 3

#End Region

#Region "変数定義"

    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsExc As New ClsCMExclusive
    Dim clsMst As New ClsMSTCommon
    Dim objStack As StackFrame

    ''' <summary>
    ''' 画面制御用
    ''' </summary>
    ''' <remarks></remarks>
    Enum DispMode
        何もしない
        デフォルト
        登録
        更新
    End Enum
    Dim Mode As DispMode

#End Region

#Region "イベントプロージャ"

    ''' <summary>
    ''' Page Init
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'ヘッダ修正
        Dim intHeadCol As Integer() = New Integer() {1, 3}
        Dim intColSpan As Integer() = New Integer() {2, 2}
        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, DispCode, intHeadCol, intColSpan)

    End Sub

    ''' <summary>
    ''' Page Load
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンアクション設定
        AddHandler Master.ppBtnSrcClear.Click, AddressOf btnSearchClear_Click         '検索条件クリア
        AddHandler Master.ppBtnSearch.Click, AddressOf btnSearch_click                '検索
        AddHandler Master.ppBtnClear.Click, AddressOf btnclear_click                  'クリア
        AddHandler Master.ppBtnUpdate.Command, AddressOf btn_click                    '更新
        AddHandler Master.ppBtnInsert.Command, AddressOf btn_click                    '登録
        AddHandler Master.ppBtnDelete.Command, AddressOf btn_click                    '削除

        'テキストボックスのイベント設定
        AddHandler txtMCode.ppTextBox.TextChanged, AddressOf txtMCode_TextChanged
        txtMCode.ppTextBox.AutoPostBack = True

        AddHandler tftSCode.ppTextBoxFrom.TextChanged, AddressOf tftSCode_TextChanged
        AddHandler tftSCode.ppTextBoxTo.TextChanged, AddressOf tftSCode_TextChanged
        tftSCode.ppTextBoxFrom.AutoPostBack = True
        tftSCode.ppTextBoxTo.AutoPostBack = True

        'エラーサマリー活性制御の為のUpdatePanelのUPdateMode調整
        Dim pnl As UpdatePanel
        pnl = Master.FindControl("UpdPanelMain")
        pnl.UpdateMode = UpdatePanelUpdateMode.Always
        pnl = Master.FindControl("UpdPanelSearch")
        pnl.UpdateMode = UpdatePanelUpdateMode.Always

        '画面初期化処理
        If Not IsPostBack Then

            'プログラムID,画面名,該当件数設定
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"

            'パンくずリストの設定
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            'グリッドの初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

            '一覧取得
            msGet_Data()

            msSetddlPrintName(ddlSPrint.ppDropDownList)
            msSetddlPrintName(ddlMPrint.ppDropDownList)

            'フォーカス設定
            SetFocus(tftSCode.ppTextBoxFrom.ClientID)

            '状態設定
            Mode = DispMode.デフォルト

        End If
    End Sub

    ''' <summary>
    ''' Page PreRender
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        Master.ppBtnClear.Enabled = True               'クリアボタン
        '*** 削除ボタンは常時非活性とする ***
        Master.ppBtnDelete.Enabled = False             '削除ボタン

        '画面項目制御
        Select Case Mode

            Case DispMode.デフォルト
                '状態：デフォルト
                txtMCode.ppEnabled = True                   'コード
                txtMName.ppEnabled = False                  '名称
                ddlMPrint.ppEnabled = False                 '印刷名

                Master.ppBtnInsert.Enabled = False             '登録ボタン
                Master.ppBtnUpdate.Enabled = False             '更新ボタン

            Case DispMode.登録
                '状態：新規
                txtMCode.ppEnabled = False                 'コード
                txtMName.ppEnabled = True                  '名称
                ddlMPrint.ppEnabled = True                 '印刷名

                Master.ppBtnInsert.Enabled = True              '登録ボタン
                Master.ppBtnUpdate.Enabled = False             '更新ボタン

            Case DispMode.更新
                '状態：更新
                txtMCode.ppEnabled = False                 'コード
                txtMName.ppEnabled = True                  '名称
                ddlMPrint.ppEnabled = True                 '印刷名

                Master.ppBtnInsert.Enabled = False             '登録ボタン
                Master.ppBtnUpdate.Enabled = True              '更新ボタン
        End Select

    End Sub

    ''' <summary>
    '''検索条件クリア
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnSearchClear_Click(sender As Object, e As EventArgs)

        'ログ出力
        psLogStart(Me)

        '初期化処理
        tftSCode.ppFromText = String.Empty
        tftSCode.ppToText = String.Empty
        txtSName.ppText = String.Empty
        ddlSPrint.ppDropDownList.SelectedIndex = -1

        'フォーカス設定
        SetFocus(tftSCode.ppTextBoxFrom.ClientID)

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 検索ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnSearch_click(sender As Object, e As EventArgs)

        'ログ出力
        psLogStart(Me)

        '入力検証OFF
        msIsValidKey(False)

        'データ取得
        Page.Validate("search")
        If (Page.IsValid) Then
            msGet_Data()
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
    Protected Sub btnclear_click(sender As Object, e As EventArgs)

        'ログ出力
        psLogStart(Me)

        '排他制御削除
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

        '初期化
        txtMCode.ppText = String.Empty
        txtMName.ppText = String.Empty
        ddlMPrint.ppSelectedValue = String.Empty

        '状態設定
        Mode = DispMode.デフォルト

        '不正入力との競合時、エラーサマリーが残る事象を防ぐ
        Page.Validate("key")
        '既存データ取得処理と競合時、ダミーボタンが残る事象を防ぐ
        Master.ppBtnDmy.Visible = False

        'フォーカス設定
        SetFocus(txtMCode.ppTextBox.ClientID)

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 追加/更新/削除ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btn_click(sender As Object, e As CommandEventArgs)

        'ログ出力
        psLogStart(Me)

        If sender.Equals(Master.ppBtnDelete) Then
            '登録/更新/削除 処理
            msEditData(e.CommandName)
            'フォーカス設定
            SetFocus(txtMCode.ppTextBox.ClientID)
        Else
            txtMName.ppText = txtMName.ppText.Trim
            Page.Validate("val")
            If (Page.IsValid) Then
                '登録/更新/削除 処理
                msEditData(e.CommandName)
                'フォーカス設定
                SetFocus(txtMCode.ppTextBox.ClientID)
            End If
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' コード変更時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub txtMCode_TextChanged()

        'ログ出力
        psLogStart(Me)

        '入力検証ON
        msIsValidKey(True)

        'ゼロ埋め
        zerofill(txtMCode.ppText)
        If txtMCode.ppText.Trim <> String.Empty Then
            '入力チェック
            Page.Validate("key")
            If Page.IsValid Then
                msGet_ExistData()
            End If
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub
    Private Sub tftSCode_TextChanged(sender As Object, e As System.EventArgs)

        'ゼロ埋め
        zerofill(sender.text)

        'フォーカス設定
        Dim afFocusID As String = String.Empty
        Select Case sender.ClientID
            Case tftSCode.ppTextBoxFrom.ClientID
                afFocusID = tftSCode.ppTextBoxTo.ClientID
            Case Else
                afFocusID = txtSName.ppTextBox.ClientID
        End Select
        Master.ppBtnDmy.Visible = True
        Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + afFocusID + ");"
        SetFocus(Master.ppBtnDmy.ClientID)
    End Sub

    ''' <summary>
    ''' Row Command
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
                arKey.Insert(0, CType(rowData.FindControl("コード"), TextBox).Text.Trim)

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
                txtMCode.ppText = CType(rowData.FindControl("コード"), TextBox).Text
                txtMName.ppText = CType(rowData.FindControl("名称"), TextBox).Text
                ddlMPrint.ppSelectedValue = CType(rowData.FindControl("印刷名"), TextBox).Text.Trim

                '不正入力との競合時、エラーサマリーが残る事象を防ぐ
                Page.Validate("key")
                '既存データ取得処理と競合時、ダミーボタンが残る事象を防ぐ
                Master.ppBtnDmy.Visible = False

                'フォーカス設定
                SetFocus(txtMName.ppTextBox.ClientID)

                '状態設定
                Mode = DispMode.更新

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

#Region "そのほかのプロージャ"

    ''' <summary>
    ''' 検索処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data()

        Dim cnDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dtsDB As DataSet = Nothing
        Dim dispflg As String = String.Empty
        objStack = New StackFrame

        If Me.IsPostBack Then
            dispflg = "0"
        Else
            dispflg = "1"
        End If

        'DB接続
        If clsDataConnect.pfOpen_Database(cnDB) Then
            Try
                cmdDB = New SqlCommand(DispCode & "_S1", cnDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("code_from", SqlDbType.NVarChar, tftSCode.ppFromText.Trim))
                    .Add(pfSet_Param("code_to", SqlDbType.NVarChar, tftSCode.ppToText.Trim))
                    .Add(pfSet_Param("name", SqlDbType.NVarChar, txtSName.ppText.Trim))
                    .Add(pfSet_Param("printname", SqlDbType.NVarChar, ddlSPrint.ppSelectedValue))
                    .Add(pfSet_Param("prog_id", SqlDbType.NVarChar, DispCode))
                    .Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, dispflg))
                End With

                'リストデータ取得
                dtsDB = clsDataConnect.pfGet_DataSet(cmdDB)

                If dtsDB.Tables(0).Rows.Count = 0 Then
                    '0件
                    Master.ppCount = "0"
                    'エラー表示
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                ElseIf CType(dtsDB.Tables(0).Rows(0).Item("総件数").ToString, Integer) > dtsDB.Tables(0).Rows.Count Then
                    '上限オーバー
                    Master.ppCount = dtsDB.Tables(0).Rows(0).Item("総件数").ToString
                    'エラー表示
                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                                        dtsDB.Tables(0).Rows(0).Item("総件数").ToString, dtsDB.Tables(0).Rows.Count)
                Else
                    Master.ppCount = dtsDB.Tables(0).Rows(0).Item("総件数").ToString
                End If

                '取得したデータをリストに反映
                Me.grvList.DataSource = dtsDB.Tables(0)
                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                'エラー表示
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()
                Master.ppCount = "0"
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(cnDB) Then
                    'エラー表示
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            'エラー表示
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"
        End If
    End Sub

    ''' <summary>
    ''' 既存データ取得処理(システム,コード変更時)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_ExistData()

        Dim cnDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dtsDB As DataSet = Nothing
        objStack = New StackFrame


        'DB接続
        If clsDataConnect.pfOpen_Database(cnDB) Then

            Try

                cmdDB = New SqlCommand(DispCode & "_S3", cnDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("code", SqlDbType.NVarChar, txtMCode.ppText.Trim))
                End With

                'リストデータ取得
                dtsDB = clsDataConnect.pfGet_DataSet(cmdDB)

                '既にデータが存在していた場合は各項目に設定
                If dtsDB.Tables(0).Rows.Count > 0 Then
                    '★排他制御削除
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

                    '★排他制御処理
                    Dim strExclusiveDate As String = String.Empty
                    Dim arTable_Name As New ArrayList
                    Dim arKey As New ArrayList

                    '★ロック対象のテーブル名登録
                    arTable_Name.Insert(0, TableName)

                    '★ロック対象のキー情報登録
                    arKey.Insert(0, txtMCode.ppText.Trim)

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

                    '編集エリアの各項目に値を設定
                    txtMCode.ppText = dtsDB.Tables(0).Rows(0).Item("コード").ToString
                    txtMName.ppText = dtsDB.Tables(0).Rows(0).Item("名称").ToString
                    ddlMPrint.ppSelectedValue = dtsDB.Tables(0).Rows(0).Item("印刷名").ToString

                    '状態設定
                    Mode = DispMode.更新
                Else
                    '状態設定
                    Mode = DispMode.登録
                End If

            Catch ex As Exception
                'エラー
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(cnDB) Then
                    'エラー
                    clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            'エラー
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

        'フォーカス設定
        Dim afFocusID As String = String.Empty
        afFocusID = txtMName.ppTextBox.ClientID
        Master.ppBtnDmy.Visible = True
        Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + afFocusID + ");"
        SetFocus(Master.ppBtnDmy.ClientID)

    End Sub

    ''' <summary>
    ''' 追加/更新/削除処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msEditData(ByVal ipstrMode As String)

        '変数宣言
        Dim cnDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dtsDB As DataSet = Nothing
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
                MesCode = "00002"
                strStored = DispCode & "_D1"
        End Select

        'DB接続
        If clsDataConnect.pfOpen_Database(cnDB) Then
            Try
                cmdDB = New SqlCommand(strStored, cnDB)

                Select Case ipstrMode

                    Case "INSERT", "UPDATE"      '登録/更新
                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("code", SqlDbType.NVarChar, txtMCode.ppText.Trim))
                            .Add(pfSet_Param("name", SqlDbType.NVarChar, txtMName.ppText.Trim))
                            .Add(pfSet_Param("printname", SqlDbType.NVarChar, ddlMPrint.ppSelectedValue))
                            .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With

                    Case "DELETE"               '削除
                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("code", SqlDbType.NVarChar, txtMCode.ppText.Trim))
                            .Add(pfSet_Param("UserId", SqlDbType.NVarChar, User.Identity.Name))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With

                End Select

                '登録/更新/削除
                Using cntrn = cnDB.BeginTransaction

                    cmdDB.Transaction = cntrn
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)

                    If intRtn <> 0 Then
                        cntrn.Rollback()
                        'エラー
                        psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                        Exit Sub
                    End If

                    'コミット
                    cntrn.Commit()

                End Using

                If ipstrMode = "INSERT" OrElse ipstrMode = "UPDATE" Then
                    '登録/更新の場合は対象レコードのみ表示
                    dttGrid.Columns.Add("コード")
                    dttGrid.Columns.Add("名称")
                    dttGrid.Columns.Add("印刷名")
                    drData = dttGrid.NewRow()
                    drData.Item("コード") = txtMCode.ppText.Trim
                    drData.Item("名称") = txtMName.ppText.Trim
                    drData.Item("印刷名") = ddlMPrint.ppSelectedValue
                    dttGrid.Rows.Add(drData)
                Else
                    '削除の場合は何も表示しない
                    dttGrid = New DataTable
                End If

                'データセット
                dttGrid.AcceptChanges()
                grvList.DataSource = dttGrid
                grvList.DataBind()

                Master.ppCount = dttGrid.Rows.Count

                '初期化処理
                btnclear_click(Nothing, Nothing)

                '完了メッセージ
                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName)

            Catch ex As Exception
                'エラー
                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(cnDB) Then
                    'エラー
                    psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            'エラー
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Sub

    ''' <summary>
    ''' ゼロ埋め
    ''' </summary>
    ''' <param name="strNum"></param>
    ''' <remarks></remarks>
    Private Sub zerofill(ByRef strNum As String)
        Dim intI As Integer

        If strNum.Trim = String.Empty OrElse Integer.TryParse(strNum, intI) = False Then
            Exit Sub
        End If

        While strNum.Trim.Length < CodeLength
            strNum = "0" & strNum
        End While
    End Sub

    Private Sub msSetddlPrintName(ByVal ddlList As DropDownList)
        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim dstDB As New DataSet
        objStack = New StackFrame

        If Not IsPostBack Then
            '初期化
            conDB = Nothing

            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    cmdDB = New SqlCommand(DispCode & "_S2", conDB)
                    With cmdDB
                        'コマンドタイプ設定(ストアド)
                        .CommandType = CommandType.StoredProcedure
                        'パラメータ設定
                        .Parameters.Add(pfSet_Param("classcd", SqlDbType.VarChar, "0145"))
                    End With

                    '区分リストデータ取得
                    dstDB = clsDataConnect.pfGet_DataSet(cmdDB)
                    ddlList.DataSource = dstDB
                    ddlList.DataTextField = "M29_NAME"
                    ddlList.DataValueField = "M29_NAME"

                    ddlList.DataBind()
                    ddlList.Items.Insert(0, New ListItem(Nothing, Nothing))


                Catch ex As ArgumentOutOfRangeException
                    psMesBox(Me.Page, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "印刷名")
                    'ログ出力
                    psLogWrite("", USER_NAME, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Catch ex As Exception
                    psMesBox(Me.Page, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "印刷名")
                End Try
            Else
                psMesBox(Me.Page, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End If
    End Sub

    Private Sub msIsValidKey(ByVal _bool As Boolean)
        Dim cuv As CustomValidator
        Dim valSumKey As ValidationSummary = Master.FindControl("ValidSumKey")
        cuv = txtMCode.FindControl("pnlErr").FindControl("cuvTextBox")
        If _bool Then
            cuv.Visible = True
            valSumKey.Visible = True
        Else
            cuv.Visible = False
            valSumKey.Visible = False
        End If

    End Sub

#End Region

End Class