'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　出精値引マスタ
'*　ＰＧＭＩＤ：　COMUPDM75
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.11.18　：　栗原
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================

Imports System.Data.SqlClient
Imports SPC.ClsCMExclusive
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect

#End Region

Public Class COMUPDM75

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
    '画面情報
    Const DispCode As String = "COMUPDM75"                   '画面ID

    'マスタ情報
    Const MasterName As String = "出精値引マスタ"            'マスタ名
    Const TableName As String = "M75_REDUCTION"            'テーブル名

    '定数
    'Const MasterName As String = "出精値引マスタ"        'マスタ名
    'Const TableName As String = "M75_REDUCTION"     'テーブル名
    'Const DispCode As String = "COMUPDM75"           '画面コード
    'Const SortKey As String = "M75_START_DT"             'ソート

#End Region

#Region "変数定義"
    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================

    Dim clsDataConnect As New ClsCMDataConnect
    Dim objStack As StackFrame
    Dim clsExc As New ClsCMExclusive
    Dim strMode As String = Nothing
    Dim clsMst As New ClsMSTCommon

#End Region


#Region "イベントプロシージャ"

    ''' <summary>
    ''' PAGE Init
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, DispCode)

    End Sub

    ''' <summary>
    ''' PAGE Load
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        AddHandler Master.ppBtnClear.Click, AddressOf btnClear_Click            'クリアボタン
        AddHandler Master.ppBtnInsert.Command, AddressOf btn_Click              '登録ボタン
        AddHandler Master.ppBtnUpdate.Command, AddressOf btn_Click              '更新ボタン
        AddHandler Master.ppBtnDelete.Command, AddressOf btn_Click              '削除ボタン

        AddHandler dtbStartDt.ppDateBox.TextChanged, AddressOf dtbStartDt_TextChanged
        dtbStartDt.ppDateBox.AutoPostBack = True
        'AddHandler txtPer.ppTextBox.TextChanged, AddressOf txtPer_TextChanged
        'txtPer.ppTextBox.AutoPostBack = True                             '値引率テキストボックス

        '桁数チェック用
        Dim cuv_per As CustomValidator
        Dim cuv_dt As CustomValidator
        cuv_per = txtPer.FindControl("pnlErr").FindControl("cuvTextBox")
        AddHandler cuv_per.ServerValidate, AddressOf cuv_Per_validate
        cuv_dt = dtbStartDt.FindControl("cuvDateBox")
        AddHandler cuv_dt.ServerValidate, AddressOf cuv_Date_validate
        If Not IsPostBack Then

            'プログラムＩＤ、画面名、件数
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"

            'パンくずリスト(ページ階級)設定
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            'グリッドの初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

            'データ取得
            msGet_Data()

            'フォーカス設定
            SetFocus(dtbStartDt.ppDateBox.ClientID)

            'ページの状態=Defaultに
            strMode = "Default"

        End If
    End Sub

    ''' <summary>
    ''' Page PreRender
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        '各テキストボックス、ボタンの活性、非活性
        Select Case strMode

            Case "Default"
                dtbStartDt.ppEnabled = True
                txtPer.ppEnabled = False
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnInsert.Enabled = False      '登録
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnDelete.Text = "削除"
                Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)
            Case "Insert"
                dtbStartDt.ppEnabled = False
                txtPer.ppEnabled = True
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnInsert.Enabled = True      '登録
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnDelete.Text = "削除"
                Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)
            Case "Select"
                dtbStartDt.ppEnabled = False
                txtPer.ppEnabled = True
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnInsert.Enabled = False     '登録
                Master.ppBtnUpdate.Enabled = True      '更新
                Master.ppBtnDelete.Enabled = True      '削除
                Master.ppBtnDelete.Text = "削除"
                Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)
            Case "Delete"
                dtbStartDt.ppEnabled = False
                txtPer.ppEnabled = False
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnInsert.Enabled = False     '登録
                Master.ppBtnUpdate.Enabled = False       '更新
                Master.ppBtnDelete.Enabled = True      '削除
                Master.ppBtnDelete.Text = "削除取消"
                Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")
        End Select

    End Sub

    ''' <summary>
    ''' クリアボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click()

        'ログ出力開始
        psLogStart(Me)

        '排他情報削除処理
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

        'txtCodeとtxtNameの初期化
        dtbStartDt.ppText = String.Empty
        txtPer.ppText = String.Empty

        'ページの状態=Defaultに
        strMode = "Default"

        'フォーカス
        SetFocus(dtbStartDt.ppDateBox.ClientID)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 登録/更新/削除ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btn_Click(sender As Object, e As CommandEventArgs)

        'ログ出力開始
        psLogStart(Me)

        If sender.Equals(Master.ppBtnDelete) OrElse (Page.IsValid) Then
            '登録更新削除
            msEditData(e.CommandName)
            'フォーカス
            SetFocus(dtbStartDt.ppDateBox.ClientID)
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 年月変更時の設定
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub dtbStartDt_TextChanged(sender As Object, e As EventArgs)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        objStack = New StackFrame

        If dtbStartDt.ppText.Trim <> String.Empty Then
            Page.Validate("key")
            If (Page.IsValid) Then
                'DB接続
                If clsDataConnect.pfOpen_Database(conDB) Then
                    Try
                        cmdDB = New SqlCommand(DispCode & "_S2", conDB)
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("start_dt", SqlDbType.NVarChar, dtbStartDt.ppText.Trim))
                        End With
                        'ストアドよりデータ取得
                        dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                        'データが存在していた場合
                        If dstOrders.Tables(0).Rows.Count > 0 Then
                            '排他制御処理
                            Dim strExclusiveDate As String = Nothing
                            Dim arTable_Name As New ArrayList
                            Dim arKey As New ArrayList
                            '排他情報削除
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
                            'ロック対象テーブル名の登録
                            arTable_Name.Insert(0, TableName)
                            'ロックテーブルキー項目の登録
                            arKey.Insert(0, dtbStartDt.ppText.Trim)
                            '排他情報確認処理(更新画面へ)
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
                                '登録年月日時刻(明細)
                                Me.Master.ppExclusiveDate = strExclusiveDate
                            Else
                                '排他ロック中メッセージ
                                clsMst.psMesBox(Me, "30010", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)
                                'ログ出力終了
                                psLogEnd(Me)
                                Exit Sub
                            End If
                            ' txtCodeとtxtNameに値を入力
                            dtbStartDt.ppText = dstOrders.Tables(0).Rows(0).Item("開始年月").ToString
                            txtPer.ppText = dstOrders.Tables(0).Rows(0).Item("値引率").ToString

                            If dstOrders.Tables(0).Rows(0).Item("削除フラグ").ToString = 1 Then
                                strMode = "Delete"
                            Else
                                strMode = "Select"
                            End If
                        Else
                            strMode = "Insert"
                        End If
                        'フォーカス設定
                        Master.ppBtnDmy.Visible = True
                        Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtPer.ppTextBox.ClientID + ");"
                        SetFocus(Master.ppBtnDmy.ClientID)

                    Catch ex As Exception
                        'エラーメッセージ表示
                        clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    Finally
                        'DB切断
                        If Not clsDataConnect.pfClose_Database(conDB) Then
                            'エラーメッセージ表示
                            clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        End If
                    End Try
                Else
                    'エラーメッセージ表示
                    clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End If
        Else
            'ページの状態=Defaultに
            strMode = "Default"

            'フォーカスをtxtPerへ
            SetFocus(txtPer.ppTextBox.ClientID)
        End If

    End Sub

    ''' <summary>
    ''' 値引率変更時の設定（小数点の処理）
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub txtPer_TextChanged(sender As Object, e As EventArgs)

        msSetpoint(txtPer.ppText)

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
            'グリッドの選択ボタン
            If e.CommandName = "Select" Then
                'ログ出力開始
                psLogStart(Me)
                '排他制御処理
                Dim strExclusiveDate As String = Nothing
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList
                Dim rowData As GridViewRow = grvList.Rows(Convert.ToInt32(e.CommandArgument))

                '排他情報削除
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
                'ロック対象テーブル名の登録
                arTable_Name.Insert(0, TableName)

                'ロックテーブルキー項目の登録
                arKey.Insert(0, CType(rowData.FindControl("開始年月"), TextBox).Text)

                '排他情報確認処理(更新画面へ)
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

                    '登録年月日時刻(明細)
                    Me.Master.ppExclusiveDate = strExclusiveDate
                Else
                    '排他ロック中メッセージ
                    clsMst.psMesBox(Me, "30010", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)
                    'ログ出力終了
                    psLogEnd(Me)
                    Exit Sub
                End If
                'dtbStartDtとtxtPerに値を入力
                dtbStartDt.ppText = CType(rowData.FindControl("開始年月"), TextBox).Text
                txtPer.ppText = CType(rowData.FindControl("値引率"), TextBox).Text

                'フォーカスをtxtPerへ
                SetFocus(txtPer.ppTextBox.ClientID)

                If CType(rowData.FindControl("削除フラグ"), TextBox).Text = "1" Then

                    strMode = "Delete"
                Else

                    strMode = "Select"
                End If
                Page.Validate("key")

            End If

        Catch ex As Exception
            'エラーメッセージ表示
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
                If CType(grvList.Rows(i).FindControl("削除フラグ"), TextBox).Text = 1 Then
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
        Dim dttGrid As New DataTable
        Dim daydata As DateTime = DateTime.Now

        If Me.IsPostBack Then
            dispFlg = "0"
        Else
            dispFlg = "1"
        End If

        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(DispCode & "_S1", conDB)
                With cmdDB.Parameters
                    'データグリッドパラメータ設定
                    .Add(pfSet_Param("start_dt", SqlDbType.NVarChar, ""))
                    .Add(pfSet_Param("end_dt", SqlDbType.NVarChar, ""))
                    .Add(pfSet_Param("prog_id", SqlDbType.NVarChar, DispCode))
                    .Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, dispFlg))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then

                    '0件の時
                    Master.ppCount = "0"
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)

                ElseIf CType(dstOrders.Tables(0).Rows(0).Item("総件数").ToString, Integer) > dstOrders.Tables(0).Rows.Count Then

                    '上限オーバーの時
                    Master.ppCount = dstOrders.Tables(0).Rows(0).Item("総件数").ToString
                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                                        dstOrders.Tables(0).Rows(0).Item("総件数").ToString, dstOrders.Tables(0).Rows.Count)
                Else
                    'その他
                    Master.ppCount = dstOrders.Tables(0).Rows(0).Item("総件数").ToString
                End If
                Me.grvList.DataSource = dstOrders.Tables(0)
                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                'エラーメッセージ表示
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()
                '件数=0
                Master.ppCount = "0"
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    'エラーメッセージ表示
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            'エラーメッセージ表示
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"
        End If
    End Sub

    ''' <summary>
    ''' 登録/更新/削除処理
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
        objStack = New StackFrame

        Dim dispFlg As String = String.Empty
        Dim daydata As DateTime = DateTime.Now

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
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(strStored, conDB)
                Select Case ipstrMode
                    Case "INSERT", "UPDATE"      '登録/更新
                        'パラメータ設定
                        With cmdDB.Parameters
                            'データグリッドパラメータ設定
                            .Add(pfSet_Param("start_dt", SqlDbType.NVarChar, dtbStartDt.ppText.Trim))
                            .Add(pfSet_Param("discount_rt", SqlDbType.NVarChar, txtPer.ppText.Trim))
                            .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
                        End With

                    Case "DELETE"               '削除
                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("start_dt", SqlDbType.NVarChar, dtbStartDt.ppText.Trim))
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))         'ユーザーＩＤ
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
                        End With

                End Select

                '登録/更新/削除
                Using cntrn = conDB.BeginTransaction
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

                msGet_Data()

                btnClear_Click()

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
                If Not clsDataConnect.pfClose_Database(conDB) Then

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
    ''' 数値を#.###に変更
    ''' </summary>
    ''' <param name="strNum"></param>
    ''' <remarks></remarks>
    Private Function msSetpoint(ByRef strNum As String) As Boolean
        msSetpoint = False
        '数値かチェック
        If Not mfCheckpoint(strNum) Then
            Exit Function
        End If

        If strNum.StartsWith(".") Then
            '"."始まり
            '文字数５文字未満なら０を先頭に追加
            If strNum.Length < 5 Then
                strNum = "0" & strNum
            Else
                Exit Function
            End If
        End If

        '０始まりなら０を消去
        While strNum.StartsWith("0")
            If strNum.StartsWith("0.") Then
                '0.###ならOK
                Exit While
            End If
            strNum = strNum.Remove(0, 1)
        End While

        '０埋め
        Select Case strNum.Length
            Case 1      '１桁の整数(０含む)
                strNum = strNum & ".000"

            Case Else   '小数点の有無をチェック

                If strNum.Contains(".") Then
                    strNum = strNum.PadRight(5, "0"c)
                Else
                    '２桁の整数なので処理無し
                End If
        End Select
        msSetpoint = True
    End Function

    ''' <summary>
    ''' 数値チェック
    ''' </summary>
    ''' <param name="strNum"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfCheckpoint(ByVal strNum As String) As Boolean

        If Not Regex.IsMatch(strNum, "^([0-9]|[.])+$", RegexOptions.ECMAScript) OrElse strNum.StartsWith(".") Then
            mfCheckpoint = False
        Else
            mfCheckpoint = True
        End If

    End Function

    ''' <summary>
    ''' 編集欄入力チェック
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuv_Per_validate(source As Object, args As ServerValidateEventArgs)
        If mfCheckpoint(txtPer.ppText) = False AndAlso txtPer.ppTextBox.Text.Trim <> String.Empty Then
            source.text = "形式エラー"
            source.ErrorMessage = "値引率 は半角数値で入力してください。"
            args.IsValid = False
            Exit Sub
        End If

        If Not Regex.IsMatch(txtPer.ppText.Trim, "\d[.]\d\d\d", RegexOptions.ECMAScript) AndAlso txtPer.ppTextBox.Text.Trim <> String.Empty Then
            source.text = "形式エラー"
            source.ErrorMessage = "値引率 の入力形式に誤りがあります。"
            args.IsValid = False
            Exit Sub
        End If

        If txtPer.ppText.Trim = "0.000" Then
            source.text = "形式エラー"
            source.ErrorMessage = "値引率 は0.001　以上の数値を入力してください。"
            args.IsValid = False
            Exit Sub
        End If
    End Sub
    Private Sub cuv_Date_validate(source As Object, args As ServerValidateEventArgs)


        If dtbStartDt.ppText.Trim <> String.Empty AndAlso Regex.IsMatch(txtPer.ppText.Trim, "\d\d\d\d[/]\d\d", RegexOptions.ECMAScript) Then
            source.text = "形式エラー"
            source.ErrorMessage = "開始年月 の入力形式に誤りがあります。"
            args.IsValid = False
            Exit Sub
        End If

    End Sub
#End Region

#Region "終了処理プロシージャ"

#End Region

End Class
