'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　作業内容マスタ
'*　ＰＧＭＩＤ：　COMUPDM89
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.11.26　：　栗原
'********************************************************************************************************************************


#Region "インポート定義"

Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive

#End Region

Public Class COMUPDM89
#Region "継承定義"

    Inherits System.Web.UI.Page

#End Region

#Region "定数定義"

    Const DispCode As String = "COMUPDM89"
    Const MasterName As String = "作業内容マスタ"
    Const TableName As String = "M89_WORK_DTIL"

#End Region
#Region "変数定義"

    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsExc As New ClsCMExclusive
    Dim clsMst As New ClsMSTCommon
    Dim objStack As StackFrame
    Dim strMode As String = Nothing

#End Region

#Region "プロパティ定義"

    ''' <summary>
    ''' 入力欄の活性/非活性
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property mpMainInpEnable() As Boolean
        Get
            Return mpMainInpEnable
        End Get
        Set(value As Boolean)
            Me.txtContent.ppEnabled = value
            Me.txtWorkTime.ppEnabled = value
            Me.txtGoRtnTime.ppEnabled = value
            Me.txtPeopleCnt.ppEnabled = value
        End Set
    End Property

    ''' <summary>
    ''' キー項目の活性/非活性
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property mpKeyInpEnable() As Boolean
        Get
            Return mpKeyInpEnable
        End Get
        Set(value As Boolean)
            Me.ddlSystem.Enabled = value
            Me.txtCode.ppEnabled = value
        End Set
    End Property

#End Region

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
        AddHandler ddlSystem.SelectedIndexChanged, AddressOf ddlSystem_SelectedIndexChanged
        ddlSystem.AutoPostBack = True

        'テキストボックスのイベント設定
        AddHandler txtCode.ppTextBox.TextChanged, AddressOf txtCode_TextChanged
        txtCode.ppTextBox.AutoPostBack = True
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
        AddHandler cuv.ServerValidate, AddressOf cuv_stxtCode
        cuv = txtCode.FindControl("pnlErr").FindControl("cuvTextBox")
        AddHandler cuv.ServerValidate, AddressOf cuv_txtCode

        '画面初期化処理
        If Not IsPostBack Then

            'プログラムID,画面名,該当件数設定
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

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
        Select Case strMode
            Case "Default"
                mpKeyInpEnable = True
                mpMainInpEnable = False
                Master.ppBtnClear.Enabled = True
                Master.ppBtnInsert.Enabled = False
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnDelete.Text = "削除"
                Master.ppBtnDelete.Enabled = False

            Case "Insert"
                mpKeyInpEnable = False
                mpMainInpEnable = True
                Master.ppBtnClear.Enabled = True
                Master.ppBtnInsert.Enabled = True
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnDelete.Text = "削除"
                Master.ppBtnDelete.Enabled = False

            Case "Update"
                mpKeyInpEnable = False
                Master.ppBtnClear.Enabled = True
                Master.ppBtnInsert.Enabled = False
                Master.ppBtnDelete.Enabled = True
                '削除データを選択した場合
                If Master.ppBtnDelete.Text = "削除取消" Then
                    Master.ppBtnUpdate.Enabled = False
                    mpMainInpEnable = False
                Else
                    Master.ppBtnUpdate.Enabled = True
                    mpMainInpEnable = True
                End If
        End Select
    End Sub

    ''' <summary>
    '''検索条件クリア
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnSearchClear_Click(sender As Object, e As EventArgs)

        'ログ出力
        psLogStart(Me)

        ddlSSystem.SelectedIndex = -1
        txtScode.ppFromText = String.Empty
        txtScode.ppToText = String.Empty
        txtscontent.ppText = String.Empty
        ddlDel.ppDropDownList.SelectedIndex = -1

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
            SetFocus(ddlSSystem.ClientID)
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' クリアボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnclear_click()

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
        ddlSystem.SelectedIndex = -1
        txtCode.ppText = String.Empty
        txtContent.ppText = String.Empty
        txtWorkTime.ppText = String.Empty
        txtGoRtnTime.ppText = String.Empty
        txtPeopleCnt.ppText = String.Empty

        Page.Validate("key") 'エラーサマリーだけ残る事象を防ぐ
        strMode = "Default"

        SetFocus(ddlSystem.ClientID)

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

        If sender.Equals(Master.ppBtnDelete) OrElse Page.IsValid Then
            '登録/更新/削除 処理
            msEditData(e.CommandName)
            'フォーカス設定
            SetFocus(ddlSystem.ClientID)
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' システム変更時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlSystem_SelectedIndexChanged()
        'ログ出力
        psLogStart(Me)
        If ddlSystem.SelectedValue <> String.Empty AndAlso txtCode.ppText <> String.Empty Then
            Page.Validate("key")
            If Page.IsValid Then
                msGet_ExistData()
            End If
        ElseIf ddlSystem.SelectedValue <> String.Empty Then
            SetFocus(txtCode.ppTextBox.ClientID)
        Else
            SetFocus(ddlSystem.ClientID)
        End If
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' コード変更時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub txtCode_TextChanged()
        'ログ出力
        psLogStart(Me)
        If ddlSystem.SelectedValue <> String.Empty AndAlso txtCode.ppText <> String.Empty Then
            Page.Validate("key")
            If Page.IsValid Then
                zerofill(txtCode.ppText)
                msGet_ExistData()
            End If
        ElseIf txtCode.ppText.Trim <> String.Empty Then
            zerofill(txtCode.ppText)
            SetFocus(ddlSystem.ClientID)
        Else
            SetFocus(ddlSystem.ClientID)
        End If
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

                Dim strDB As String = CType(rowData.FindControl("システムコード"), TextBox).Text

                If Not (ddlSystem.Items.FindByValue(strDB) Is Nothing) Then

                    ddlSystem.SelectedValue = CType(rowData.FindControl("システムコード"), TextBox).Text
                    txtCode.ppText = CType(rowData.FindControl("コード"), TextBox).Text
                    txtContent.ppText = CType(rowData.FindControl("作業内容"), TextBox).Text
                    txtWorkTime.ppText = CType(rowData.FindControl("作業時間"), TextBox).Text
                    txtGoRtnTime.ppText = CType(rowData.FindControl("往復時間"), TextBox).Text
                    txtPeopleCnt.ppText = CType(rowData.FindControl("人数"), TextBox).Text

                    '削除フラグ確認
                    If CType(rowData.FindControl("削除"), TextBox).Text = "0" Then
                        Master.ppBtnDelete.Text = "削除"
                        Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)
                    Else
                        Master.ppBtnDelete.Text = "削除取消"
                        Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")
                    End If

                    SetFocus(txtContent.ppTextBox.ClientID)
                    strMode = "Update"
                    Page.Validate("key")
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
        For i = 0 To grvList.Rows.Count - 1
            For z = 1 To grvList.Columns.Count - 1
                If CType(grvList.Rows(i).FindControl("削除"), TextBox).Text = 1 Then
                    CType(grvList.Rows(i).FindControl(grvList.Columns(z).HeaderText), TextBox).ForeColor = Drawing.Color.Red
                    CType(grvList.Rows(i).FindControl("システムコード"), TextBox).ForeColor = Drawing.Color.Red
                    CType(grvList.Rows(i).FindControl("コード"), TextBox).ForeColor = Drawing.Color.Red
                End If
            Next
        Next
    End Sub

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

        If clsDataConnect.pfOpen_Database(cnDB) Then
            Try
                cmdDB = New SqlCommand(DispCode & "_S1", cnDB)
                With cmdDB.Parameters
                    .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ddlSSystem.SelectedValue))
                    .Add(pfSet_Param("code_from", SqlDbType.NVarChar, txtScode.ppFromText.Trim))
                    .Add(pfSet_Param("code_to", SqlDbType.NVarChar, txtScode.ppToText.Trim))
                    .Add(pfSet_Param("content", SqlDbType.NVarChar, txtscontent.ppText.Trim))
                    .Add(pfSet_Param("prog_id", SqlDbType.NVarChar, DispCode))
                    .Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, dispflg))
                    .Add(pfSet_Param("del_flg", SqlDbType.NVarChar, ddlDel.ppSelectedValue))
                End With

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

                Me.grvList.DataSource = dtsDB.Tables(0)
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
                cmdDB = New SqlCommand(DispCode & "_S2", cnDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ddlSystem.SelectedValue))
                    .Add(pfSet_Param("code", SqlDbType.NVarChar, txtCode.ppText.Trim))
                End With

                dtsDB = clsDataConnect.pfGet_DataSet(cmdDB)

                If dtsDB.Tables(0).Rows.Count > 0 Then
                    '★排他制御削除
                    If Not Me.Master.ppExclusiveDate = String.Empty Then
                        If clsExc.pfDel_Exclusive(Me _
                                           , Session(P_SESSION_SESSTION_ID) _
                                           , Me.Master.ppExclusiveDate) = 0 Then
                            Me.Master.ppExclusiveDate = String.Empty
                        Else
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
                    arKey.Insert(0, ddlSystem.SelectedValue)
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
                        psLogEnd(Me)
                        Exit Sub
                    End If

                    '編集エリアの各項目に値を設定
                    ddlSystem.SelectedValue = dtsDB.Tables(0).Rows(0).Item("システムコード").ToString
                    txtCode.ppText = dtsDB.Tables(0).Rows(0).Item("コード").ToString
                    txtContent.ppText = dtsDB.Tables(0).Rows(0).Item("作業内容").ToString
                    txtWorkTime.ppText = dtsDB.Tables(0).Rows(0).Item("作業時間").ToString
                    txtGoRtnTime.ppText = dtsDB.Tables(0).Rows(0).Item("往復時間").ToString
                    txtPeopleCnt.ppText = dtsDB.Tables(0).Rows(0).Item("人数").ToString

                    '削除フラグ確認
                    If dtsDB.Tables(0).Rows(0).Item("削除").ToString = "0" Then
                        Master.ppBtnDelete.Text = "削除"
                        Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)
                    Else
                        Master.ppBtnDelete.Text = "削除取消"
                        Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")
                    End If

                    strMode = "Update"
                Else
                    strMode = "Insert"
                End If
                Master.ppBtnDmy.Visible = True
                Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtContent.ppTextBox.ClientID + ");"
                SetFocus(Master.ppBtnDmy.ClientID)
            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(cnDB) Then
                    clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If



    End Sub

    ''' <summary>
    ''' 追加/更新/削除処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msEditData(ByVal ipstrMode As String)

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
                    Case Else
                        MesCode = "00001"
                        strStored = DispCode & "_D1"
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
                            .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ddlSystem.SelectedValue))
                            .Add(pfSet_Param("code", SqlDbType.NVarChar, txtCode.ppText.Trim))
                            .Add(pfSet_Param("content", SqlDbType.NVarChar, txtContent.ppText.Trim))
                            .Add(pfSet_Param("worktime", SqlDbType.NVarChar, txtWorkTime.ppText.Trim))
                            .Add(pfSet_Param("gortntime", SqlDbType.NVarChar, txtGoRtnTime.ppText.Trim))
                            .Add(pfSet_Param("peoplecnt", SqlDbType.NVarChar, txtPeopleCnt.ppText.Trim))
                            .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With

                    Case "DELETE"               '削除
                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ddlSystem.SelectedValue))
                            .Add(pfSet_Param("code", SqlDbType.NVarChar, txtCode.ppText.Trim))
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With

                End Select

                '登録/更新/削除
                Using cntrn = cnDB.BeginTransaction
                    cmdDB.Transaction = cntrn
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()

                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        cntrn.Rollback()
                        psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                        Exit Sub
                    End If

                    cntrn.Commit()
                End Using

                If ipstrMode = "INSERT" OrElse ipstrMode = "UPDATE" OrElse Master.ppBtnDelete.Text = "削除取消" Then
                    '登録/更新の場合は対象レコードのみ表示
                    dttGrid.Columns.Add("システムコード")
                    dttGrid.Columns.Add("システム")
                    dttGrid.Columns.Add("コード")
                    dttGrid.Columns.Add("作業内容")
                    dttGrid.Columns.Add("作業時間")
                    dttGrid.Columns.Add("往復時間")
                    dttGrid.Columns.Add("人数")
                    dttGrid.Columns.Add("削除")
                    drData = dttGrid.NewRow()
                    drData.Item("システムコード") = ddlSystem.SelectedItem.ToString.Split(":")(0)
                    drData.Item("システム") = ddlSystem.SelectedItem.ToString.Split(":")(1)
                    drData.Item("コード") = txtCode.ppText.Trim
                    drData.Item("作業内容") = txtContent.ppText.Trim
                    drData.Item("作業時間") = txtWorkTime.ppText.Trim
                    drData.Item("往復時間") = txtGoRtnTime.ppText.Trim
                    drData.Item("人数") = txtPeopleCnt.ppText.Trim
                    drData.Item("削除") = "0"
                    dttGrid.Rows.Add(drData)
                Else
                    dttGrid = New DataTable
                End If

                dttGrid.AcceptChanges()
                grvList.DataSource = dttGrid
                grvList.DataBind()

                Master.ppCount = dttGrid.Rows.Count

                btnclear_click()

                strMode = "Default"
                SetFocus(ddlSystem.ClientID)

                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName)

            Catch ex As Exception
                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(cnDB) Then
                    psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
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
                Me.ddlSystem.Items.Clear()
                Me.ddlSystem.DataSource = objdts.Tables(1)
                Me.ddlSystem.DataTextField = "ＴＢＯＸリスト"
                Me.ddlSystem.DataValueField = "ＴＢＯＸシステムコード"
                Me.ddlSystem.DataBind()
                Me.ddlSystem.Items.Insert(0, "**:ダミー") 'ダミーを追加
                Me.ddlSystem.Items(0).Value = "**"
                Me.ddlSystem.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ機種マスタ一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objcn) Then
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
        If intI = 0 Then
            Exit Sub
        End If
        While strNum.Trim.Length < 2
            strNum = "0" & strNum
        End While
    End Sub

    ''' <summary>
    ''' コードゼロチェック
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuv_txtCode(source As Object, args As ServerValidateEventArgs)
        Select Case txtCode.ppText.Trim
            Case "0", "00"
                source.text = "形式エラー"
                source.ErrorMessage = "コード は０以上の半角数値で入力してください。"
                args.IsValid = False
                Exit Sub
        End Select
    End Sub

    ''' <summary>
    ''' コードゼロチェック
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuv_stxtCode(source As Object, args As ServerValidateEventArgs)
        Select Case txtScode.ppFromText.Trim
            Case "0", "00"
                source.text = "形式エラー"
                source.ErrorMessage = "コード は０以上の半角数値で入力してください。"
                args.IsValid = False
                Exit Sub
        End Select
        Select Case txtScode.ppToText.Trim
            Case "0", "00"
                source.text = "形式エラー"
                source.ErrorMessage = "コード は０以上の半角数値で入力してください。"
                args.IsValid = False
                Exit Sub
        End Select
    End Sub

#End Region


End Class