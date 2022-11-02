'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　回復内容マスタ
'*　ＰＧＭＩＤ：　COMUPDM80
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.03.27　：　林
'********************************************************************************************************************************

'---------------------------------------------------------------------------------------------------------------------------------
'番号　　　　　｜　日付　　　　　｜　名前　　｜　備考
'---------------------------------------------------------------------------------------------------------------------------------
'COMUPDM80-001　　2015/09/14　　　　栗原　　　　ゼロ埋め処理　入力チェック　削除ドロップダウンリスト追加

#Region "インポート定義"

Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive

#End Region

Public Class COMUPDM80

#Region "継承定義"

    Inherits System.Web.UI.Page

#End Region

#Region "定数定義"

    Const DispCode As String = "COMUPDM80"
    Const MasterName As String = "回復内容マスタ"
    Const TableName As String = "M80_REPAIR_DTIL"

#End Region

#Region "変数定義"

    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsExc As New ClsCMExclusive
    Dim clsMst As New ClsMSTCommon
    Dim objStack As StackFrame
    Dim strMode As String = Nothing
    'Dim strDelflg As String = Nothing

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

        'ドロップダウンリストのイベント設定
        AddHandler ddlMSystem.SelectedIndexChanged, AddressOf ddlMSystem_SelectedIndexChanged
        ddlMSystem.AutoPostBack = True

        'テキストボックスのイベント設定
        AddHandler txtMcode.ppTextBox.TextChanged, AddressOf txtMCode_TextChanged
        txtMcode.ppTextBox.AutoPostBack = True
        AddHandler txtScode.ppTextBoxFrom.TextChanged, AddressOf txtSCode_TextChanged
        AddHandler txtScode.ppTextBoxTo.TextChanged, AddressOf txtSCode_TextChanged
        txtScode.ppTextBoxFrom.AutoPostBack = True
        txtScode.ppTextBoxTo.AutoPostBack = True

        'エラーサマリー活性制御の為のUpdatePanelのUPdateMode調整
        Dim pnl As UpdatePanel
        pnl = Master.FindControl("UpdPanelMain")
        pnl.UpdateMode = UpdatePanelUpdateMode.Always
        pnl = Master.FindControl("UpdPanelSearch")
        pnl.UpdateMode = UpdatePanelUpdateMode.Always

        '桁数チェック用
        Dim cuv As CustomValidator
        cuv = txtScode.FindControl("pnlErr").FindControl("cuvTextBox")
        AddHandler cuv.ServerValidate, AddressOf cuv_s_val
        cuv = txtMcode.FindControl("pnlErr").FindControl("cuvTextBox")
        AddHandler cuv.ServerValidate, AddressOf cuv_m_val


        ''文言のイベント設定
        'Me.txtMcontent.ppTextBox.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txtMcontent.ppMaxLength & """);")
        'Me.txtMcontent.ppTextBox.Attributes.Add("onChange", "lenCheck(this,""" & Me.txtMcontent.ppMaxLength & """);")

        '画面初期化処理
        If Not IsPostBack Then

            '削除データも含む(CheckBox)を活性化
            'Master.ppchksDel.Visible = True

            'プログラムID,画面名,該当件数設定
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"

            'パンくずリストの設定
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            'ドロップダウンリストの設定
            msSetddlSystem()

            'グリッドの初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

            'フォーカス設定
            SetFocus(ddlSSystem.ClientID)

            '状態設定
            strMode = "Default"

        End If
    End Sub

    ''' <summary>
    ''' Page PreRender
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        '画面項目制御
        Select Case strMode

            Case "Default"                                     '状態：デフォルト
                ddlMSystem.Enabled = True                      'システムリスト
                txtMcode.ppEnabled = True                      'コードテキスト
                txtMcontent.ppEnabled = False                  '文言テキスト
                Master.ppBtnClear.Enabled = True               'クリアボタン
                Master.ppBtnInsert.Enabled = False             '登録ボタン
                Master.ppBtnUpdate.Enabled = False             '更新ボタン
                Master.ppBtnDelete.Text = "削除"               '削除ボタンテキスト変更
                Master.ppBtnDelete.Enabled = False             '削除ボタン

            Case "Insert"                                      '状態：新規
                ddlMSystem.Enabled = False                     'システムリスト
                txtMcode.ppEnabled = False                     'コードテキスト
                txtMcontent.ppEnabled = True                   '文言テキスト
                Master.ppBtnClear.Enabled = True               'クリアボタン
                Master.ppBtnInsert.Enabled = True              '登録ボタン
                Master.ppBtnUpdate.Enabled = False             '更新ボタン
                Master.ppBtnDelete.Text = "削除"               '削除ボタンテキスト変更
                Master.ppBtnDelete.Enabled = False             '削除ボタン

            Case "Update"                                      '状態：更新
                ddlMSystem.Enabled = False                     'システムリスト
                txtMcode.ppEnabled = False                     'コードテキスト
                Master.ppBtnClear.Enabled = True               'クリアボタン
                Master.ppBtnInsert.Enabled = False             '登録ボタン
                '削除データを選択した場合
                If Master.ppBtnDelete.Text = "削除取消" Then
                    Master.ppBtnUpdate.Enabled = False         '更新ボタン
                    txtMcontent.ppEnabled = False              '文言テキスト
                Else
                    Master.ppBtnUpdate.Enabled = True          '更新ボタン
                    txtMcontent.ppEnabled = True               '文言テキスト
                End If
                Master.ppBtnDelete.Enabled = True              '削除ボタン

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
        ddlSSystem.SelectedIndex = -1
        txtScode.ppFromText = String.Empty
        txtScode.ppToText = String.Empty
        txtscontent.ppText = String.Empty
        'Master.ppchksDel.Checked = False
        ddlDel.ppDropDownList.SelectedIndex = -1

        'フォーカス設定
        SetFocus(ddlSSystem)

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

        'データ取得
        Page.Validate("search")
        If (Page.IsValid) Then

            msGet_Data()

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

        'ドロップダウンリスト再取得(編集エリア)
        msSetddlMSystem()

        '初期化
        ddlMSystem.SelectedIndex = -1
        txtMcode.ppText = String.Empty
        txtMcontent.ppText = String.Empty

        '状態設定
        strMode = "Default"

        'フォーカス設定
        SetFocus(ddlMSystem)

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
            SetFocus(ddlMSystem)
        Else
            Page.Validate("val")
            If (Page.IsValid) Then
                '登録/更新/削除 処理
                msEditData(e.CommandName)
                'フォーカス設定
                SetFocus(ddlMSystem)
            End If
        End If

        'データ取得

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' システム変更時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlMSystem_SelectedIndexChanged()

        'ログ出力
        psLogStart(Me)

        msGet_ExistData("System")

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

        'ゼロ埋め
        zerofill(txtMcode.ppText)

        msGet_ExistData("Code")

        'ログ出力終了
        psLogEnd(Me)

    End Sub
    Private Sub txtSCode_TextChanged(sender As Object, e As System.EventArgs)

        'ゼロ埋め
        zerofill(sender.text)

        'フォーカス設定
        Dim afFocusID As String = String.Empty
        Select Case sender.ClientID
            Case txtScode.ppTextBoxFrom.ClientID
                afFocusID = txtScode.ppTextBoxTo.ClientID
            Case Else
                afFocusID = ddlDel.ppDropDownList.ClientID
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

        'ドロップダウンリスト再取得(編集エリア)
        msSetddlMSystem()

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
                arKey.Insert(0, CType(rowData.FindControl("システムコード"), TextBox).Text)

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

                '編集エリアのドロップダウンリスト内検索
                Dim strDB As String = CType(rowData.FindControl("システムコード"), TextBox).Text

                If Not (ddlMSystem.Items.FindByValue(strDB) Is Nothing) Then

                    '値があった場合
                    '編集エリアに値を設定
                    ddlMSystem.SelectedValue = CType(rowData.FindControl("システムコード"), TextBox).Text
                    txtMcode.ppText = CType(rowData.FindControl("コード"), TextBox).Text
                    txtMcontent.ppText = CType(rowData.FindControl("文言"), TextBox).Text
                    '削除フラグ確認
                    If CType(rowData.FindControl("削除"), TextBox).Text = "" Then

                        Master.ppBtnDelete.Text = "削除"
                        Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)

                    Else

                        Master.ppBtnDelete.Text = "削除取消"
                        Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")

                    End If

                    'フォーカス設定
                    SetFocus(txtMcontent.ppTextBox.ClientID)
                    '状態設定
                    strMode = "Update"

                Else

                    '値が無い場合システムの先頭にコード表示
                    Me.ddlMSystem.Items(0).Text = (CType(rowData.FindControl("システムコード"), TextBox).Text) & ":" & (CType(rowData.FindControl("システム"), TextBox).Text)
                    txtMcode.ppText = CType(rowData.FindControl("コード"), TextBox).Text
                    txtMcontent.ppText = CType(rowData.FindControl("文言"), TextBox).Text
                    '削除フラグ確認
                    If CType(rowData.FindControl("削除"), TextBox).Text = "" Then

                        Master.ppBtnDelete.Text = "削除"
                        Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)

                    Else

                        Master.ppBtnDelete.Text = "削除取消"
                        Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")

                    End If

                    'フォーカス設定
                    SetFocus(txtMcontent.ppTextBox.ClientID)
                    '状態設定
                    strMode = "Update"

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
    ''' <summary>
    ''' DataBound
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound
        '削除行の赤字化
        For Each rowData As GridViewRow In grvList.Rows
            If CType(rowData.FindControl("削除"), TextBox).Text = "●" Then
                CType(rowData.FindControl("システムコード"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("システム"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("コード"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("文言"), TextBox).ForeColor = Drawing.Color.Red
            End If
        Next
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

        'If Master.ppchksDel.Checked.Equals(True) Then

        '    strDelflg = "1"

        'Else

        '    strDelflg = "0"

        'End If

        'DB接続
        If clsDataConnect.pfOpen_Database(cnDB) Then

            Try

                cmdDB = New SqlCommand(DispCode & "_S1", cnDB)

                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ddlSSystem.SelectedValue))
                    .Add(pfSet_Param("code_from", SqlDbType.NVarChar, txtScode.ppFromText.Trim))
                    .Add(pfSet_Param("code_to", SqlDbType.NVarChar, txtScode.ppToText.Trim))
                    .Add(pfSet_Param("content", SqlDbType.NVarChar, txtscontent.ppText.Trim))
                    .Add(pfSet_Param("prog_id", SqlDbType.NVarChar, DispCode))
                    .Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, dispflg))
                    .Add(pfSet_Param("del_flg", SqlDbType.NVarChar, ddlDel.ppSelectedValue))
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
    Private Sub msGet_ExistData(ByVal strkey As String)

        Dim cnDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dtsDB As DataSet = Nothing
        objStack = New StackFrame

        If ddlMSystem.SelectedIndex > 0 AndAlso txtMcode.ppText.Trim <> String.Empty Then

            '入力チェック
            Page.Validate("key")
            If (Page.IsValid) Then

                'DB接続
                If clsDataConnect.pfOpen_Database(cnDB) Then

                    Try

                        cmdDB = New SqlCommand(DispCode & "_S2", cnDB)
                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ddlMSystem.SelectedValue))
                            .Add(pfSet_Param("code", SqlDbType.NVarChar, txtMcode.ppText.Trim))
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
                            arKey.Insert(0, ddlMSystem.SelectedValue)
                            arKey.Insert(0, txtMcode.ppText.Trim)

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

                            'ドロップダウンリスト再取得(編集エリア)
                            msSetddlMSystem()

                            '編集エリアの各項目に値を設定
                            ddlMSystem.SelectedValue = dtsDB.Tables(0).Rows(0).Item("システムコード").ToString
                            txtMcode.ppText = dtsDB.Tables(0).Rows(0).Item("コード").ToString
                            txtMcontent.ppText = dtsDB.Tables(0).Rows(0).Item("文言").ToString

                            '削除フラグ確認
                            If dtsDB.Tables(0).Rows(0).Item("削除").ToString = "" Then

                                Master.ppBtnDelete.Text = "削除"
                                Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)

                            Else

                                Master.ppBtnDelete.Text = "削除取消"
                                Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")

                            End If

                            '状態設定
                            strMode = "Update"

                        Else

                            '状態設定
                            strMode = "Insert"

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

            Else

                '処理終了
                Exit Sub

            End If

        End If

        'フォーカス設定
        Dim afFocusID As String = String.Empty
        If strMode = "Update" Then
            afFocusID = txtMcontent.ppTextBox.ClientID
        Else
            If ddlMSystem.SelectedValue <> "" AndAlso txtMcode.ppText.Trim <> String.Empty Then
                afFocusID = txtMcontent.ppTextBox.ClientID
            Else
                If strkey = "Code" Then
                    afFocusID = ddlMSystem.ClientID
                ElseIf strkey = "System" Then
                    afFocusID = ddlMSystem.ClientID
                End If
            End If
        End If

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
                Select Case Master.ppBtnDelete.Text

                    Case "削除"
                        MesCode = "00002"
                        strStored = DispCode & "_D1"
                        'strDelflg = "1"

                    Case Else
                        MesCode = "00001"
                        strStored = DispCode & "_D1"
                        'strDelflg = "0"

                End Select

        End Select

        'DB接続
        If clsDataConnect.pfOpen_Database(cnDB) Then

            Try

                cmdDB = New SqlCommand(strStored, cnDB)

                Select Case ipstrMode

                    Case "INSERT", "UPDATE"      '登録/更新
                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ddlMSystem.SelectedValue))
                            .Add(pfSet_Param("code", SqlDbType.NVarChar, txtMcode.ppText.Trim))
                            .Add(pfSet_Param("content", SqlDbType.NVarChar, txtMcontent.ppText.Trim))
                            .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With

                    Case "DELETE"               '削除
                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ddlMSystem.SelectedValue))
                            .Add(pfSet_Param("code", SqlDbType.NVarChar, txtMcode.ppText.Trim))
                            .Add(pfSet_Param("UserId", SqlDbType.NVarChar, User.Identity.Name))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                            '.Add(pfSet_Param("del_flg", SqlDbType.NVarChar, strDelflg))
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
                    dttGrid.Columns.Add("システムコード")
                    dttGrid.Columns.Add("システム")
                    dttGrid.Columns.Add("コード")
                    dttGrid.Columns.Add("文言")
                    dttGrid.Columns.Add("削除")
                    drData = dttGrid.NewRow()
                    drData.Item("システムコード") = ddlMSystem.SelectedItem.ToString.Split(":")(0)
                    drData.Item("システム") = ddlMSystem.SelectedItem.ToString.Split(":")(1)
                    drData.Item("コード") = txtMcode.ppText.Trim
                    drData.Item("文言") = txtMcontent.ppText.Trim
                    drData.Item("削除") = ""
                    dttGrid.Rows.Add(drData)

                    'ドロップダウンリスト再取得(編集エリア)
                    msSetddlMSystem()

                ElseIf Master.ppBtnDelete.Text = "削除取消" Then

                    '削除取消の場合は対象レコードのみ表示
                    'msGet_Data()

                    dttGrid.Columns.Add("システムコード")
                    dttGrid.Columns.Add("システム")
                    dttGrid.Columns.Add("コード")
                    dttGrid.Columns.Add("文言")
                    dttGrid.Columns.Add("削除")
                    drData = dttGrid.NewRow()
                    drData.Item("システムコード") = ddlMSystem.SelectedValue
                    drData.Item("システム") = ddlMSystem.SelectedItem.ToString.Split(":")(1)
                    drData.Item("コード") = txtMcode.ppText.Trim
                    drData.Item("文言") = txtMcontent.ppText.Trim
                    drData.Item("削除") = ""
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
                ddlMSystem.SelectedIndex = -1
                txtMcode.ppText = String.Empty
                txtMcontent.ppText = String.Empty

                '状態設定
                strMode = "Default"

                'フォーカス設定
                SetFocus(ddlMSystem)

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
    ''' ドロップダウンリスト設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlSystem()

        Dim objcn As SqlConnection = Nothing
        Dim objcmd As SqlCommand = Nothing
        Dim objdts As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objcn) Then

            'エラー
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

        Else

            Try

                objcmd = New SqlCommand("ZMSTSEL004", objcn)
                'データ取得
                objdts = clsDataConnect.pfGet_DataSet(objcmd)

                'ドロップダウンリスト設定
                '検索条件エリア
                Me.ddlSSystem.Items.Clear()
                Me.ddlSSystem.DataSource = objdts.Tables(0)
                Me.ddlSSystem.DataTextField = "ＴＢＯＸリスト"
                Me.ddlSSystem.DataValueField = "ＴＢＯＸシステムコード"
                Me.ddlSSystem.DataBind()
                Me.ddlSSystem.Items.Insert(0, "**:ダミー") 'ダミーを追加
                Me.ddlSSystem.Items(0).Value = "**"
                Me.ddlSSystem.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                '編集エリア
                Me.ddlMSystem.Items.Clear()
                Me.ddlMSystem.DataSource = objdts.Tables(1)
                Me.ddlMSystem.DataTextField = "ＴＢＯＸリスト"
                Me.ddlMSystem.DataValueField = "ＴＢＯＸシステムコード"
                Me.ddlMSystem.DataBind()
                Me.ddlMSystem.Items.Insert(0, "**:ダミー") 'ダミーを追加
                Me.ddlMSystem.Items(0).Value = "**"
                Me.ddlMSystem.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception

                'エラー
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ機種マスタ一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")

            Finally

                'DB切断
                If Not clsDataConnect.pfClose_Database(objcn) Then

                    'エラー
                    clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

                End If

            End Try

        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定(編集エリア)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlMSystem()

        Dim objcn As SqlConnection = Nothing
        Dim objcmd As SqlCommand = Nothing
        Dim objdts As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objcn) Then

            'エラー
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

        Else

            Try

                objcmd = New SqlCommand("ZMSTSEL004", objcn)
                'データ取得
                objdts = clsDataConnect.pfGet_DataSet(objcmd)

                'ドロップダウンリスト設定
                '編集エリア
                Me.ddlMSystem.Items.Clear()
                Me.ddlMSystem.DataSource = objdts.Tables(1)
                Me.ddlMSystem.DataTextField = "ＴＢＯＸリスト"
                Me.ddlMSystem.DataValueField = "ＴＢＯＸシステムコード"
                Me.ddlMSystem.DataBind()
                Me.ddlMSystem.Items.Insert(0, "**:ダミー") 'ダミーを追加
                Me.ddlMSystem.Items(0).Value = "**"
                Me.ddlMSystem.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception

                'エラー
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ機種マスタ一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")

            Finally

                'DB切断
                If Not clsDataConnect.pfClose_Database(objcn) Then

                    'エラー
                    clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

                End If

            End Try

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

        While strNum.Trim.Length < 4
            strNum = "0" & strNum
        End While
    End Sub

    ''' <summary>
    ''' 検索欄入力チェック
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuv_s_val(source As Object, args As ServerValidateEventArgs)
        Dim intI As Integer
        If (Integer.TryParse(txtScode.ppTextBoxFrom.Text.Trim, intI) = False AndAlso txtScode.ppTextBoxFrom.Text.Trim <> String.Empty) _
            OrElse (Integer.TryParse(txtScode.ppTextBoxTo.Text.Trim, intI) = False AndAlso txtScode.ppTextBoxTo.Text.Trim <> String.Empty) Then
            source.text = "形式エラー"
            source.ErrorMessage = "コード は、半角数字 で入力してください。"
            args.IsValid = False
            Exit Sub
        End If


    End Sub
    ''' <summary>
    ''' 編集欄入力チェック
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuv_m_val(source As Object, args As ServerValidateEventArgs)
        Dim intI As Integer
        If (Integer.TryParse(txtMcode.ppTextBox.Text.Trim, intI) = False AndAlso txtMcode.ppTextBox.Text.Trim <> String.Empty) Then
            source.text = "形式エラー"
            source.ErrorMessage = "コード は、半角数字 で入力してください。"
            args.IsValid = False
            Exit Sub
        End If
    End Sub

#End Region

End Class
