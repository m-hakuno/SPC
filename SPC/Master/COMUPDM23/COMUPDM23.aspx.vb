'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　ＴＢＯＸ機種マスタ
'*　ＰＧＭＩＤ：　COMUPDM23
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.03.13　：　星野
'*  変　更　　：  2015.05.11  ：　栗原
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

Public Class COMUPDM23

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
    Const DispCode As String = "COMUPDM23"                      '画面ID
    Const MasterName As String = "ＴＢＯＸタイプマスタ"           '画面名
    Const TableName As String = "M23_TBOXCLASS"                 'テーブル名

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
        AddHandler txtSystemCd.TextChanged, AddressOf txtSystemCd_TextChanged
        txtSystemCd.AutoPostBack = True
        tftSSystemCd.ppTextBoxFrom.AutoPostBack = True
        tftSSystemCd.ppTextBoxTo.AutoPostBack = True
        AddHandler tftSSystemCd.ppTextBoxFrom.TextChanged, AddressOf tftSystemCd_TextChanged
        AddHandler tftSSystemCd.ppTextBoxTo.TextChanged, AddressOf tftSystemCd_TextChanged

        'エラーサマリー活性制御の為のUpdatePanelのUPdateMode調整
        Dim pnl As UpdatePanel
        pnl = Master.FindControl("UpdPanelMain")
        pnl.UpdateMode = UpdatePanelUpdateMode.Always
        pnl = Master.FindControl("UpdPanelSearch")
        pnl.UpdateMode = UpdatePanelUpdateMode.Always
        Dim scm As ScriptManager
        scm = Master.FindControl("tsmManager")
        scm.RegisterPostBackControl(txtSystemCd)

        '検索項目｢システム｣の桁数チェックの為にバリデーターのインスタンスと検証イベント生成
        Dim cuv As CustomValidator
        cuv = tftSSystemCd.FindControl("pnlErr").FindControl("cuvTextBox")
        AddHandler cuv.ServerValidate, AddressOf cuv_val

        '削除ボタン非表示
        Master.ppBtnDelete.Visible = False

        If Not IsPostBack Then
            'プログラムＩＤ、画面名、件数設定
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"

            'パンくずリスト設定
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            'ドロップダウンリスト設定
            msSetddlSystemCls()
            msSetddlSumCls()

            'グリッドの初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

            'データ取得
            msGet_Data()

            'フォーカス設定
            SetFocus(tftSSystemCd.ppTextBoxFrom.ClientID)

            strMode = "Default"

        End If

        'グリッド調整
        grvList.Columns(2).HeaderStyle.CssClass = "GridNoDisp"
        grvList.Columns(1).HeaderStyle.Width = 173
        grvList.Columns(5).HeaderStyle.CssClass = "GridNoDisp"
        grvList.Columns(4).HeaderStyle.Width = 143
        grvList.Columns(7).HeaderStyle.CssClass = "GridNoDisp"
        grvList.Columns(6).HeaderStyle.Width = 103


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
                txtSystemCd.Enabled = True
                Master.ppBtnInsert.Enabled = False     '追加
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnDelete.Text = "削除"
            Case "Insert"
                Master.ppMainEnabled = True
                msSet_Enabled(True)
                txtSystemCd.Enabled = False
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
                txtSystemCd.Enabled = False
                Master.ppBtnInsert.Enabled = False     '追加
                Master.ppBtnDelete.Enabled = True      '削除
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
            SetFocus(tftSSystemCd.ppTextBoxFrom.ClientID)
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

        tftSSystemCd.ppFromText = String.Empty
        tftSSystemCd.ppToText = String.Empty
        txtSSystemNm.ppText = String.Empty
        ddlSSystemCls.SelectedIndex = -1
        ddlSSumCls.ppDropDownList.SelectedIndex = -1
        ddlDel.ppDropDownList.SelectedIndex = -1
        'フォーカス設定
        SetFocus(tftSSystemCd.ppTextBoxFrom.ClientID)

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

        'Page.Validate("val")
        If (Page.IsValid) Or sender.Equals(Master.ppBtnDelete) Then
            msEditData(e.CommandName)
            'フォーカス設定
            SetFocus(tftSSystemCd.ppTextBoxFrom.ClientID)
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

        txtSystemCd.Text = String.Empty
        txtSystemNm.ppText = String.Empty
        txtShortNm.ppText = String.Empty
        ddlSystemCls.SelectedIndex = -1
        ddlSumCls.SelectedIndex = -1


        'フォーカス設定
        SetFocus(txtSystemCd.ClientID)

        strMode = "Default"

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' システムコード変更時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub txtSystemCd_TextChanged()

        'ログ出力開始
        psLogStart(Me)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        objStack = New StackFrame

        If txtSystemCd.Text.Trim <> String.Empty Then
            '入力チェック
            Page.Validate("key")
            If (Page.IsValid) Then
                '接続
                If clsDataConnect.pfOpen_Database(conDB) Then
                    Try
                        cmdDB = New SqlCommand(DispCode & "_S2", conDB)

                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, edit_num(txtSystemCd.Text.Trim)))
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
                            arKey.Insert(0, edit_num(txtSystemCd.Text.Trim))

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
                            msSetddlSystemCls("textchange")
                            msSetddlSumCls()

                            '編集エリアに値を設定
                            With dstOrders.Tables(0).Rows(0)
                                txtSystemCd.Text = .Item("システム").ToString
                                txtSystemNm.ppText = .Item("システム名").ToString
                                txtShortNm.ppText = .Item("システム略称").ToString
                                Dim lstItem As ListItem = ddlSystemCls.Items.FindByValue(.Item("システム分類").ToString)
                                If lstItem Is Nothing Then
                                    ddlSystemCls.Items(0).Value = .Item("システム分類").ToString
                                    ddlSystemCls.Items(0).Text = .Item("システム分類").ToString & ":" & .Item("システム分類名").ToString
                                Else
                                    ddlSystemCls.SelectedValue = .Item("システム分類").ToString
                                End If
                                lstItem = ddlSumCls.Items.FindByValue(.Item("集計用区分").ToString)
                                If lstItem Is Nothing Then
                                    ddlSumCls.Items(0).Value = .Item("集計用区分").ToString
                                    ddlSumCls.Items(0).Text = .Item("集計用区分").ToString & ":" & .Item("集計用区分名").ToString
                                Else
                                    ddlSumCls.SelectedValue = .Item("集計用区分").ToString
                                End If


                                '削除フラグによってボタンの文言を変更
                                If .Item("削除フラグ").ToString <> "1" Then
                                    Master.ppBtnDelete.Text = "削除"
                                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '削除
                                Else
                                    Master.ppBtnDelete.Text = "削除取消"
                                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
                                End If
                            End With

                            strMode = "Select"
                        Else
                            strMode = "Insert"
                            txtSystemCd.Text = Microsoft.VisualBasic.StrConv(edit_num(txtSystemCd.Text.Trim), Microsoft.VisualBasic.VbStrConv.Narrow)
                        End If

                        If Master.ppBtnDelete.Text = "削除" Then
                            Master.ppBtnDmy.Visible = True
                            Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtSystemNm.ppTextBox.ClientID + ");"
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

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 検索エリアのシステムコード編集
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub tftSystemCd_TextChanged(sender As Object, e As EventArgs)
        If sender.Equals(tftSSystemCd.ppTextBoxFrom) AndAlso Regex.IsMatch(tftSSystemCd.ppTextBoxFrom.Text, "^\d{0,2}$") Then
            If tftSSystemCd.ppTextBoxFrom.Text <> "0" Then
                tftSSystemCd.ppTextBoxFrom.Text = edit_num(tftSSystemCd.ppTextBoxFrom.Text)
            End If
        ElseIf sender.Equals(tftSSystemCd.ppTextBoxTo) AndAlso Regex.IsMatch(tftSSystemCd.ppTextBoxTo.Text, "^\d{0,2}$") Then
            If tftSSystemCd.ppTextBoxTo.Text <> "0" Then
                tftSSystemCd.ppTextBoxTo.Text = edit_num(tftSSystemCd.ppTextBoxTo.Text)
            End If
        End If
        If sender.Equals(tftSSystemCd.ppTextBoxFrom) Then
            SetFocus(tftSSystemCd.ppTextBoxTo.ClientID)
        ElseIf sender.Equals(tftSSystemCd.ppTextBoxTo) Then
            SetFocus(txtSSystemNm.ppTextBox.ClientID)
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

                'ドロップダウンリスト設定
                'msSetddlSystemCls()
                'msSetddlSumCls()

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
                txtSystemCd.Text = CType(rowData.FindControl("システム"), TextBox).Text
                txtSystemNm.ppText = CType(rowData.FindControl("システム名"), TextBox).Text
                txtShortNm.ppText = CType(rowData.FindControl("システム略称"), TextBox).Text
                Dim lstItem As ListItem = ddlSystemCls.Items.FindByValue(CType(rowData.FindControl("システム分類"), TextBox).Text)
                If lstItem Is Nothing Then
                    ddlSystemCls.Items(0).Value = CType(rowData.FindControl("システム分類"), TextBox).Text
                    ddlSystemCls.Items(0).Text = CType(rowData.FindControl("システム分類"), TextBox).Text & ":" & CType(rowData.FindControl("システム分類名"), TextBox).Text
                Else
                    ddlSystemCls.SelectedValue = CType(rowData.FindControl("システム分類"), TextBox).Text
                End If
                lstItem = ddlSumCls.Items.FindByValue(CType(rowData.FindControl("集計用区分"), TextBox).Text)
                If lstItem Is Nothing Then
                    ddlSumCls.Items(0).Value = CType(rowData.FindControl("集計用区分"), TextBox).Text
                    ddlSumCls.Items(0).Text = CType(rowData.FindControl("集計用区分"), TextBox).Text & ":" & CType(rowData.FindControl("集計用区分名"), TextBox).Text
                Else
                    ddlSumCls.SelectedValue = CType(rowData.FindControl("集計用区分"), TextBox).Text
                End If

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
                    SetFocus(txtSystemNm.ppTextBox.ClientID)
                Else
                    SetFocus(Master.ppBtnClear.ClientID)
                End If

                strMode = "Select"
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
    ''' GRID_DataBound
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
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

        txtSystemCd.Enabled = bool
        txtSystemNm.ppEnabled = bool
        txtShortNm.ppEnabled = bool
        ddlSystemCls.Enabled = bool
        ddlSumCls.Enabled = bool


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
        Dim delflg As String = String.Empty

        If Me.IsPostBack Then
            dispFlg = "0"
            '2015/07/22　修正:削除区分非表示
            'delflg = ddlDel.ppDropDownList.SelectedValue
            delflg = "0"
        Else
            dispFlg = "1"
            delflg = "0"
        End If



        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(DispCode & "_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("system_cd_from", SqlDbType.NVarChar, tftSSystemCd.ppFromText.Trim))
                    .Add(pfSet_Param("system_cd_to", SqlDbType.NVarChar, tftSSystemCd.ppToText.Trim))
                    .Add(pfSet_Param("system_nm", SqlDbType.NVarChar, txtSSystemNm.ppText.Trim))
                    .Add(pfSet_Param("system_cls", SqlDbType.NVarChar, ddlSSystemCls.SelectedValue))
                    .Add(pfSet_Param("sum_cls", SqlDbType.NVarChar, ddlSSumCls.ppDropDownList.SelectedValue))
                    .Add(pfSet_Param("delete", SqlDbType.NVarChar, delflg))
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
                            .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, txtSystemCd.Text.Trim))                               'システムコード
                            .Add(pfSet_Param("system_nm", SqlDbType.NVarChar, txtSystemNm.ppText.Trim))                             'システム名
                            .Add(pfSet_Param("short_nm", SqlDbType.NVarChar, txtShortNm.ppText.Trim))                               'システム略称
                            .Add(pfSet_Param("system_cls", SqlDbType.NVarChar, ddlSystemCls.SelectedValue))                         'システム分類
                            .Add(pfSet_Param("sum_cls", SqlDbType.NVarChar, ddlSumCls.SelectedValue))                               '集計用区分
                            .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))                                              '処理区分
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                                     'ユーザーＩＤ
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                                       '戻り値
                        End With

                    Case "DELETE"
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, txtSystemCd.Text.Trim))          'システムコード
                            .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))                           '処理区分
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                  'ユーザーＩＤ
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                    '戻り値
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
                    dttGrid.Columns.Add("システム")
                    dttGrid.Columns.Add("システム名")
                    dttGrid.Columns.Add("システム略称")
                    dttGrid.Columns.Add("システム分類")
                    dttGrid.Columns.Add("システム分類名")
                    dttGrid.Columns.Add("集計用区分")
                    dttGrid.Columns.Add("集計用区分名")
                    dttGrid.Columns.Add("削除フラグ")
                    drData = dttGrid.NewRow()
                    drData.Item("システム") = txtSystemCd.Text.Trim
                    drData.Item("システム名") = txtSystemNm.ppText.Trim
                    drData.Item("システム略称") = txtShortNm.ppText.Trim
                    drData.Item("システム分類") = ddlSystemCls.SelectedValue
                    drData.Item("システム分類名") = ddlSystemCls.SelectedItem.ToString.Split(":")(1)
                    drData.Item("集計用区分") = ddlSumCls.SelectedValue
                    drData.Item("集計用区分名") = ddlSumCls.SelectedItem.ToString.Split(":")(1)
                    drData.Item("削除フラグ") = "0"
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
    ''' ドロップダウンリスト設定（システム分類）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlSystemCls(Optional ByVal strMode As String = "")

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
                    .Add(pfSet_Param("Class", SqlDbType.NVarChar, "0006"))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '既存データ取得から呼び出された場合、検索欄のドロップダウンリストは再生成しない。
                If strMode = "" Then
                    'ドロップダウンリスト設定
                    Me.ddlSSystemCls.Items.Clear()
                    Me.ddlSSystemCls.DataSource = objDs.Tables(0)
                    Me.ddlSSystemCls.DataTextField = "リスト用"
                    Me.ddlSSystemCls.DataValueField = "コード"
                    Me.ddlSSystemCls.DataBind()
                    Me.ddlSSystemCls.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                    Me.ddlSSystemCls.SelectedIndex = -1
                End If

                'ドロップダウンリスト設定
                Me.ddlSystemCls.Items.Clear()
                Me.ddlSystemCls.DataSource = objDs.Tables(0)
                Me.ddlSystemCls.DataTextField = "リスト用"
                Me.ddlSystemCls.DataValueField = "コード"
                Me.ddlSystemCls.DataBind()
                Me.ddlSystemCls.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                Me.ddlSystemCls.SelectedIndex = -1
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
    ''' ドロップダウンリスト設定（集計用区分）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlSumCls()

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
                    .Add(pfSet_Param("Class", SqlDbType.NVarChar, "0095"))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                Me.ddlSumCls.Items.Clear()
                Me.ddlSumCls.DataSource = objDs.Tables(0)
                Me.ddlSumCls.DataTextField = "リスト用"
                Me.ddlSumCls.DataValueField = "コード"
                Me.ddlSumCls.DataBind()
                Me.ddlSumCls.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "集計用区分取得")
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
    ''' 入力チェック（システム分類）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub cuvSystemCls_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvSystemCls.ServerValidate

        Dim strErrNo As String
        Dim dtrMes As DataRow
        'エラーチェック
        strErrNo = pfCheck_ListErr(args.Value, True)

        If strErrNo <> String.Empty Then
            'エラー
            dtrMes = pfGet_ValMes(strErrNo, "システム分類")
            cuvSystemCls.Text = dtrMes.Item(P_VALMES_SMES)
            cuvSystemCls.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            args.IsValid = False
            Exit Sub
        End If

    End Sub

    ''' <summary>
    ''' 入力チェック（集計用区分）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub cuvSumCls_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvSumCls.ServerValidate

        Dim strErrNo As String
        Dim dtrMes As DataRow
        'エラーチェック
        strErrNo = pfCheck_ListErr(args.Value, True)

        If strErrNo <> String.Empty Then
            'エラー
            dtrMes = pfGet_ValMes(strErrNo, "集計用区分")
            cuvSumCls.Text = dtrMes.Item(P_VALMES_SMES)
            cuvSumCls.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            args.IsValid = False
            Exit Sub
        End If

    End Sub

    ''' <summary>
    ''' 入力チェック(システムコード)
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuvSyscode_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstSystemCd.ServerValidate
        Dim tb As TextBox = CType(Master.FindControl("MainContent").FindControl(source.ControlToValidate), TextBox)
        'txtSystemCd.Text = Microsoft.VisualBasic.StrConv(txtSystemCd.Text.Trim, Microsoft.VisualBasic.VbStrConv.Narrow)

        If txtSystemCd.Enabled = False Then
            args.IsValid = True
            Exit Sub
        End If

        '0～9かチェック
        If Not Regex.IsMatch(tb.Text, "^\d+[0-9]{0,2}$") Then
            source.ErrorMessage = "システムは、半角数字で入力して下さい。"
            source.Text = "形式エラー"
            args.IsValid = False
            Exit Sub
        ElseIf tb.Text <> "0" AndAlso tb.Text <> "０" Then
            txtSystemCd.Text = edit_num(tb.Text)
        End If

        If txtSystemCd.Text.Length = 1 Then
            source.ErrorMessage = "システムは、2桁の数字で入力して下さい。"
            source.Text = "形式エラー"
            args.IsValid = False
        End If

        Select Case tb.Text
            Case "00", "００", "0０", "０0"
                source.ErrorMessage = "システムの「00」は、許容出来ません。入力値を確認して下さい。"
                source.Text = "形式エラー"
                args.IsValid = False
        End Select

    End Sub

    ''' <summary>
    ''' 検索欄桁数チェック(検索欄システムコード)
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuv_val(source As Object, args As ServerValidateEventArgs)
        If tftSSystemCd.ppFromText.Length = 1 OrElse tftSSystemCd.ppToText.Length = 1 Then
            source.text = "桁数エラー"
            source.ErrorMessage = "システムは、半角数字２桁で入力してください。"
            args.IsValid = False
            Exit Sub
        End If
    End Sub

    ''' <summary>
    ''' 連番１桁時先頭に０を追加
    ''' </summary>
    ''' <param name="num">連番</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Private Function edit_num(ByVal num) As String
        If num.ToString.Length = 1 Then
            num = "0" & num
        End If
        Return num
    End Function

#End Region

#Region "終了処理プロシージャ"

#End Region

End Class
