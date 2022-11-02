'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　型式マスタ
'*　ＰＧＭＩＤ：　COMUPDM66
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.06.08　：　栗原
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

Public Class COMUPDM66

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
    Const DispCode As String = "COMUPDM66"                      '画面ID
    Const MasterName As String = "型式マスタ"           '画面名
    Const TableName As String = "M66_MODEL"                 'テーブル名

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
    Dim ValCheck As Integer = 0
#End Region

    ''' <summary>
    ''' Page Init
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Dim intHeadCol As Integer() = New Integer() {2, 4} 'グリッドXML定義ファイル取得　ヘッダ修正パラメータ
        Dim intColSpan As Integer() = New Integer() {2, 2}

        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, DispCode, intHeadCol, intColSpan)
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
        AddHandler txtModelCd.ppTextBox.TextChanged, AddressOf txtModelCd_TextChanged
        txtModelCd.ppTextBox.AutoPostBack = True

        Dim pnl As UpdatePanel
        pnl = Master.FindControl("UpdPanelMain")
        pnl.UpdateMode = UpdatePanelUpdateMode.Always
        pnl = Master.FindControl("UpdPanelSearch")
        pnl.UpdateMode = UpdatePanelUpdateMode.Always
        Dim scm As ScriptManager
        scm = Master.FindControl("tsmManager")
        scm.RegisterPostBackControl(txtModelCd)

        If Not IsPostBack Then

            'プログラムＩＤ、画面名、件数設定
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"

            'パンくずリスト設定
            Master.ppBCList = pfGet_BCList("マスタ一覧", Master.ppTitle)

            'グリッドの初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

            'データ取得
            msGet_Data()

            'フォーカス設定
            SetFocus(txtSModelCd.ppTextBox.ClientID)

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
                Master.ppMainEnabled = True
                msSet_Enabled(False)
                txtModelCd.ppEnabled = True
                Master.ppBtnInsert.Enabled = False     '追加
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnDelete.Text = "削除"
            Case "Insert"
                Master.ppMainEnabled = True
                msSet_Enabled(True)
                txtModelCd.ppEnabled = False
                Master.ppBtnInsert.Enabled = True      '追加
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnDelete.Text = "削除"
            Case "Select"
                If Master.ppBtnDelete.Text = "削除" Then
                    msSet_Enabled(True)
                    txtModelCd.ppEnabled = False
                    Master.ppBtnUpdate.Enabled = True      '更新
                Else
                    msSet_Enabled(False)
                    Master.ppBtnUpdate.Enabled = False
                End If
                Master.ppBtnInsert.Enabled = False     '追加
                Master.ppBtnDelete.Enabled = True      '削除
                Master.ppBtnClear.Enabled = True       'クリア
        End Select
        gridedit()
        msSetVal()
    End Sub

    ''' <summary>
    '''検索ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)
        'データ取得
        Page.Validate("search")
        If (Page.IsValid) Then
            msGet_Data()
            'フォーカス設定
            '  SetFocus(txtModelCd.ppTextBox.ClientID)
            Master.ppBtnDmy.Visible = True
            Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtSModelCd.ppTextBox.ClientID + ");"
            SetFocus(Master.ppBtnDmy.ClientID)
        End If
        'ログ出力終了
        psLogEnd(Me)
        ValCheck += 1
    End Sub

    ''' <summary>
    '''検索条件クリア
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnSearchClear_Click(sender As Object, e As EventArgs)
        'ログ出力開始
        psLogStart(Me)
        txtSModelCd.ppText = String.Empty
        ddlSSystem.ppDropDownList.SelectedIndex = -1
        ddlSSum.ppDropDownList.SelectedIndex = -1
        ddlDel.ppDropDownList.SelectedIndex = -1

        'フォーカス設定
        'SetFocus(txtSModelCd.ppTextBox.ClientID)
        Master.ppBtnDmy.Visible = True
        Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtSModelCd.ppTextBox.ClientID + ");"
        SetFocus(Master.ppBtnDmy.ClientID)

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

        If (Page.IsValid) Or sender.Equals(Master.ppBtnDelete) Then
            msEditData(e.CommandName)
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


        txtModelCd.ppText = String.Empty
        ddlSystem.ppDropDownList.SelectedIndex = -1
        ddlSum.ppDropDownList.SelectedIndex = -1

        'フォーカス設定
        SetFocus(txtModelCd.ppTextBox.ClientID)
        'Master.ppBtnDmy.Visible = True
        'Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtModelCd.ppTextBox.ClientID + ");"
        'SetFocus(Master.ppBtnDmy.ClientID)
        strMode = "Default"

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 故障機型式番号変更時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub txtModelCd_TextChanged()

        'ログ出力開始
        psLogStart(Me)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        objStack = New StackFrame

        If txtModelCd.ppText.Trim <> String.Empty Then
            '入力チェック
            msSetVal()
            ValCheck += 1
            Page.Validate("key")
            If (Page.IsValid) Then
                '接続
                If clsDataConnect.pfOpen_Database(conDB) Then
                    Try
                        cmdDB = New SqlCommand(DispCode & "_S2", conDB)

                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("model_cd", SqlDbType.NVarChar, txtModelCd.ppTextBox.Text.Trim))
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
                            arKey.Insert(0, txtModelCd.ppTextBox.Text.Trim)

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
                            With dstOrders.Tables(0).Rows(0)
                                txtModelCd.ppTextBox.Text = .Item("型式番号").ToString

                                Dim lstItem As ListItem = ddlSystem.ppDropDownList.Items.FindByValue(.Item("システム分類").ToString)
                                If lstItem Is Nothing Then
                                    ddlSystem.ppDropDownList.Items(0).Value = .Item("システム分類").ToString
                                    ddlSystem.ppDropDownList.Items(0).Text = .Item("システム分類").ToString
                                Else
                                    ddlSystem.ppDropDownList.SelectedValue = .Item("システム分類").ToString
                                End If
                                lstItem = ddlSum.ppDropDownList.Items.FindByValue(.Item("集計用区分").ToString)
                                If lstItem Is Nothing Then
                                    ddlSum.ppDropDownList.Items(0).Value = .Item("集計用区分").ToString
                                    ddlSum.ppDropDownList.Items(0).Text = .Item("集計用区分").ToString
                                Else
                                    ddlSum.ppDropDownList.SelectedValue = .Item("集計用区分").ToString
                                End If
                                If .Item("削除区分").ToString = "0" Then
                                    Master.ppBtnDelete.Text = "削除"
                                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '削除
                                Else
                                    Master.ppBtnDelete.Text = "削除取消"
                                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
                                End If
                            End With
                            strMode = "Select"
                            SetFocus(ddlSystem.ppDropDownList.ClientID)
                        Else
                            strMode = "Insert"
                            SetFocus(ddlSystem.ppDropDownList.ClientID)
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
                arKey.Insert(0, CType(rowData.FindControl("型式番号"), TextBox).Text)

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
                txtModelCd.ppTextBox.Text = CType(rowData.FindControl("型式番号"), TextBox).Text
                Dim lstItem As ListItem = ddlSystem.ppDropDownList.Items.FindByValue(CType(rowData.FindControl("システム分類"), TextBox).Text)
                If lstItem Is Nothing Then
                    ddlSystem.ppDropDownList.Items(0).Value = CType(rowData.FindControl("システム分類"), TextBox).Text
                    ddlSystem.ppDropDownList.Items(0).Text = CType(rowData.FindControl("システム分類"), TextBox).Text
                Else
                    ddlSystem.ppDropDownList.SelectedValue = CType(rowData.FindControl("システム分類"), TextBox).Text
                End If
                lstItem = ddlSum.ppDropDownList.Items.FindByValue(CType(rowData.FindControl("集計用区分"), TextBox).Text)
                If lstItem Is Nothing Then
                    ddlSum.ppDropDownList.Items(0).Value = CType(rowData.FindControl("集計用区分"), TextBox).Text
                    ddlSum.ppDropDownList.Items(0).Text = CType(rowData.FindControl("集計用区分"), TextBox).Text
                Else
                    ddlSum.ppDropDownList.SelectedValue = CType(rowData.FindControl("集計用区分"), TextBox).Text
                End If

                If CType(rowData.FindControl("削除区分"), TextBox).Text <> "1" Then
                    Master.ppBtnDelete.Text = "削除"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '削除
                    strMode = "Select"
                Else
                    Master.ppBtnDelete.Text = "削除取消"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
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
                    .Add(pfSet_Param("model_cd", SqlDbType.NVarChar, txtSModelCd.ppText.Trim))
                    .Add(pfSet_Param("system_cls", SqlDbType.NVarChar, ddlSSystem.ppDropDownList.SelectedValue))
                    .Add(pfSet_Param("sum_cls", SqlDbType.NVarChar, ddlSSum.ppDropDownList.SelectedValue))
                    .Add(pfSet_Param("prog_id", SqlDbType.NVarChar, DispCode))
                    .Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, dispFlg))
                    .Add(pfSet_Param("delflg", SqlDbType.NVarChar, ddlDel.ppDropDownList.SelectedValue))
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
                            .Add(pfSet_Param("model_cd", SqlDbType.NVarChar, txtModelCd.ppText.Trim))
                            .Add(pfSet_Param("system_cls", SqlDbType.NVarChar, ddlSystem.ppDropDownList.SelectedValue))
                            .Add(pfSet_Param("sum_cls", SqlDbType.NVarChar, ddlSum.ppDropDownList.SelectedValue))
                            .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))                                              '処理区分
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                                     'ユーザーＩＤ
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                                       '戻り値
                        End With

                    Case "DELETE"
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("model_cd", SqlDbType.NVarChar, txtModelCd.ppTextBox.Text.Trim))           '故障機器型式番号
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                           '戻り値
                        End With
                End Select

                'データ登録
                Using conTrn = conDB.BeginTransaction

                    cmdDB.Transaction = conTrn
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()
                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn = 1 Then
                        conTrn.Rollback()
                        psMesBox(Me, "30010", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "登録最大件数までデータ", "データを整理するまで新規登録は")
                        Exit Sub
                    ElseIf intRtn <> 0 Then
                        conTrn.Rollback()
                        psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                        Exit Sub
                    End If
                    'コミット
                    conTrn.Commit()
                End Using

                '追加、更新の場合は対象のレコードのみグリッドに表示
                If ipstrMode = "INSERT" OrElse ipstrMode = "UPDATE" OrElse Master.ppBtnDelete.Text = "削除取消" Then
                    dttGrid.Columns.Add("型式番号")
                    dttGrid.Columns.Add("システム分類")
                    dttGrid.Columns.Add("システム分類名")
                    dttGrid.Columns.Add("集計用区分")
                    dttGrid.Columns.Add("集計用区分名")
                    dttGrid.Columns.Add("削除区分")
                    drData = dttGrid.NewRow()
                    drData.Item("型式番号") = txtModelCd.ppTextBox.Text.Trim
                    drData.Item("システム分類") = ddlSystem.ppDropDownList.SelectedValue
                    If Not drData.Item("システム分類") = "" Then
                        drData.Item("システム分類名") = ddlSystem.ppDropDownList.SelectedItem.ToString.Split(":")(1)
                    End If
                    drData.Item("集計用区分") = ddlSum.ppDropDownList.SelectedValue
                    If Not drData.Item("集計用区分") = "" Then
                        drData.Item("集計用区分名") = ddlSum.ppDropDownList.SelectedItem.ToString.Split(":")(1)
                    End If
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
    ''' 編集エリア活性制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_Enabled(ByVal bool As Boolean)
        txtModelCd.ppEnabled = bool
        ddlSystem.ppEnabled = bool
        ddlSum.ppEnabled = bool
    End Sub

    ''' <summary>
    ''' グリッド編集
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function gridedit()
        'ヘッダテキスト設定
        Dim strHeader As String() = New String() {"選択", "型式番号", "システム分類", "システム分類名", "集計用区分", "集計用区分名", "削除区分"}
        Try
            If Not IsPostBack Then
                For clm As Integer = 1 To 6
                    grvList.Columns(clm).HeaderText = strHeader(clm)
                Next
            End If
            For Each rowData As GridViewRow In grvList.Rows
                '削除フラグ１の時該当行赤字化
                If CType(rowData.FindControl(strHeader(6)), TextBox).Text.Trim = "1" Then
                    CType(rowData.FindControl("型式番号"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("システム分類"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("システム分類名"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("集計用区分"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("集計用区分名"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("削除区分"), TextBox).ForeColor = Drawing.Color.Red
                End If
            Next
        Catch ex As Exception
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
        Return True
    End Function

    ''' <summary>
    ''' バリデーションサマリーテキスト編集
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetVal()
        Dim cuv As CustomValidator
        Dim valSumKey As ValidationSummary = Master.FindControl("ValidSumKey")

        cuv = txtModelCd.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = Microsoft.VisualBasic.Replace(cuv.ErrorMessage, "半角文字", " 半角英数 ")
        cuv.ErrorMessage = Microsoft.VisualBasic.Replace(cuv.ErrorMessage, "正しい形式", "半角英数")
        cuv.EnableClientScript = True

        cuv = txtSModelCd.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = Microsoft.VisualBasic.Replace(cuv.ErrorMessage, "半角文字", " 半角英数 ")
        cuv.ErrorMessage = Microsoft.VisualBasic.Replace(cuv.ErrorMessage, "正しい形式", "半角英数")
        cuv.EnableClientScript = True

        If txtModelCd.ppEnabled = False OrElse ValCheck = 2 Then
            cuv = txtModelCd.FindControl("pnlErr").FindControl("cuvTextBox")
            cuv.Visible = False
            valSumKey.Visible = False
        Else
            cuv = txtModelCd.FindControl("pnlErr").FindControl("cuvTextBox")
            cuv.Visible = True
            valSumKey.Visible = True
        End If
        ValCheck = 0
    End Sub

#Region "終了処理プロシージャ"

#End Region

End Class
