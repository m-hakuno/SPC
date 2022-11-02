'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　電話番号用途マスタ
'*　ＰＧＭＩＤ：　COMUPDM31
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.04.10　：　林
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'COMUPDM31-001     2015/09/24      栗原　　　自動採番追加、バグ修正等
'********************************************注意*****************************************************
'コード99はホールマスタ管理で自由入力に使う為、ストアドと合わせてハードコーディングで除外してあります。
'******************************************************************************************************

#Region "インポート定義"

Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive

#End Region

Public Class COMUPDM31

#Region "継承定義"

    Inherits System.Web.UI.Page

#End Region

#Region "定数定義"

    Const DispCode As String = "COMUPDM31"
    Const MasterName As String = "電話番号別文言マスタ"
    Const TableName As String = "M31_TELNOUSE"

#End Region

#Region "変数定義"

    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsExc As New ClsCMExclusive
    Dim clsMst As New ClsMSTCommon
    Dim objStack As StackFrame
    Dim strMode As String = Nothing

#End Region

#Region "イベントプロージャ"

    ''' <summary>
    ''' Page_Init
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, ByVal e As EventArgs) Handles Me.Init

        pfSet_GridView(Me.grvList, DispCode)

    End Sub

    ''' <summary>
    ''' Page_Load
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンアクション設定
        AddHandler Master.ppBtnSrcClear.Click, AddressOf btnSrcClear_Click
        AddHandler Master.ppBtnSearch.Click, AddressOf btnSearch_Click
        AddHandler Master.ppBtnClear.Click, AddressOf btnClear_Click
        AddHandler Master.ppBtnInsert.Command, AddressOf btn_Click
        AddHandler Master.ppBtnUpdate.Command, AddressOf btn_Click
        AddHandler Master.ppBtnDelete.Command, AddressOf btn_Click
        AddHandler btnGetSeq.Click, AddressOf btnGetSeq_Click

        'コードイベント設定
        AddHandler txtMcode.ppTextBox.TextChanged, AddressOf txtMcode_TextChanged
        txtMcode.ppTextBox.AutoPostBack = True
        AddHandler txtScode.ppTextBox.TextChanged, AddressOf txtScode_TextChanged
        txtScode.ppTextBox.AutoPostBack = True

        Dim scm As ScriptManager
        scm = Master.FindControl("tsmManager")
        scm.RegisterPostBackControl(txtMcode)
        scm.RegisterPostBackControl(btnGetSeq)

        '用途名称イベント設定
        Me.txtMcontent.ppTextBox.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txtMcontent.ppMaxLength & """);")
        Me.txtMcontent.ppTextBox.Attributes.Add("onChange", "lenCheck(this,""" & Me.txtMcontent.ppMaxLength & """);")

        Dim pnl As UpdatePanel
        pnl = Master.FindControl("UpdPanelMain")
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

            msGet_Data()

            '状態設定
            strMode = "Default"

            'フォーカス設定
            SetFocus(txtScode.ppTextBox.ClientID)

        End If

    End Sub

    ''' <summary>
    ''' Page_PreRender
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        '画面項目制御処理
        Select Case strMode

            Case "Default"
                txtMcode.ppTextBox.Enabled = True
                txtMcontent.ppTextBox.Enabled = False
                Master.ppBtnClear.Enabled = True
                Master.ppBtnInsert.Enabled = False
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnDelete.Enabled = False
                Master.ppBtnDelete.Text = "削除"

            Case "Insert"
                txtMcode.ppTextBox.Enabled = False
                txtMcontent.ppTextBox.Enabled = True
                Master.ppBtnClear.Enabled = True
                Master.ppBtnInsert.Enabled = True
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnDelete.Enabled = False
                Master.ppBtnDelete.Text = "削除"

            Case "Update"
                txtMcode.ppTextBox.Enabled = False
                txtMcontent.ppTextBox.Enabled = True
                Master.ppBtnClear.Enabled = True
                Master.ppBtnInsert.Enabled = False
                Master.ppBtnUpdate.Enabled = True
                Master.ppBtnDelete.Enabled = True
                Master.ppBtnDelete.Text = "削除"

            Case "Delete"
                txtMcode.ppTextBox.Enabled = False
                txtMcontent.ppTextBox.Enabled = False
                Master.ppBtnClear.Enabled = True
                Master.ppBtnInsert.Enabled = False
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnDelete.Enabled = True

        End Select

    End Sub

    ''' <summary>
    ''' 検索条件クリア
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnSrcClear_Click(sender As Object, ByVal e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        '初期化処理
        txtScode.ppTextBox.Text = String.Empty
        lblScontent.Text = String.Empty
        ddldel.ppDropDownList.SelectedIndex = -1

        'フォーカス設定
        SetFocus(txtScode.ppTextBox.ClientID)

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 検索ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, ByVal e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        'データ出力
        Page.Validate("search")
        If (Page.IsValid) Then

            '検索処理
            msGet_Data()

            'フォーカス設定
            SetFocus(txtScode.ppTextBox.ClientID)

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

        '初期化処理
        txtMcode.ppTextBox.Text = String.Empty
        txtMcontent.ppTextBox.Text = String.Empty

        '状態設定
        strMode = "Default"

        'フォーカス設定
        SetFocus(txtMcode.ppTextBox.ClientID)

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 登録/更新/削除 ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btn_Click(sender As Object, ByVal e As CommandEventArgs)

        'ログ出力開始
        psLogStart(Me)

        'データ取得
        If (Page.IsValid) Then

            '登録/更新/削除 処理
            msEdit_Data(e.CommandName)

            '初期化処理
            txtMcode.ppTextBox.Text = String.Empty
            txtMcontent.ppTextBox.Text = String.Empty

            '状態設定
            strMode = "Default"

            'フォーカス設定
            SetFocus(txtScode.ppTextBox.ClientID)

        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' コード変更時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub txtMcode_TextChanged(sender As Object, ByVal e As EventArgs)

        'ログ出力開始
        psLogStart(Me)
        System.Threading.Thread.Sleep(50)
        setVal("ON")

        txtMcode.ppText = edit_num(txtMcode.ppText)
        If txtMcode.ppText = "99" Then
            psMesBox(Me, "30028", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "コード：99は自由入力用", "編集")
            Exit Sub
        End If

        If Not txtMcode.ppText.Trim = String.Empty Then
            '入力チェック
            Page.Validate("key")
            If (Page.IsValid) Then
                '既存データ取得処理
                msGet_ExistData(txtMcode.ppText, txtMcontent.ppText)
            End If
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' コード変更時(検索欄)
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub txtScode_TextChanged(sender As Object, ByVal e As EventArgs)

        'ログ出力開始
        psLogStart(Me)
        txtScode.ppText = edit_num(txtScode.ppText)
        '既存データ取得処理
        msGet_SearchData(txtScode.ppText, lblScontent.Text)

        SetFocus(ddldel.ppDropDownList)
        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' RowCommand
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, ByVal e As CommandEventArgs) Handles grvList.RowCommand

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
                txtMcode.ppText = CType(rowData.FindControl("コード"), TextBox).Text
                txtMcontent.ppText = CType(rowData.FindControl("用途名称"), TextBox).Text

                '削除フラグ確認
                If CType(rowData.FindControl("削除"), TextBox).Text = "0" Then

                    Master.ppBtnDelete.Text = "削除"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)

                    '状態設定
                    strMode = "Update"

                    'フォーカス設定
                    SetFocus(txtMcontent.ppTextBox.ClientID)

                Else

                    Master.ppBtnDelete.Text = "削除取消"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")

                    '状態設定
                    strMode = "Delete"

                    'フォーカス設定
                    SetFocus(Master.ppBtnClear.ClientID)

                End If
                setVal("OFF")

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
    Protected Sub grvList_DataBound(sender As Object, ByVal e As EventArgs) Handles grvList.DataBound

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

#Region "その他のプロージャ"

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
                    .Add(pfSet_Param("code", SqlDbType.NVarChar, txtScode.ppText.Trim))
                    .Add(pfSet_Param("del_flg", SqlDbType.NVarChar, ddldel.ppSelectedValue))
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
                grvList.DataSource = dtsDB.Tables(0)
                '変更を反映
                grvList.DataBind()

            Catch ex As Exception

                'エラー表示
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                grvList.DataSource = New DataTable
                grvList.DataBind()
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
            grvList.DataSource = New DataTable
            grvList.DataBind()
            Master.ppCount = "0"

        End If

    End Sub

    ''' <summary>
    ''' 登録/更新/削除 処理
    ''' </summary>
    ''' <param name="ipstrMode"></param>
    ''' <remarks></remarks>
    Private Sub msEdit_Data(ByVal ipstrMode As String)

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
                            .Add(pfSet_Param("code", SqlDbType.NVarChar, txtMcode.ppText.Trim))
                            .Add(pfSet_Param("content", SqlDbType.NVarChar, txtMcontent.ppText.Trim))
                            .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))
                            .Add(pfSet_Param("user_id", SqlDbType.NVarChar, User.Identity.Name))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With

                    Case "DELETE"               '削除/削除取消
                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("code", SqlDbType.NVarChar, txtMcode.ppText.Trim))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                            .Add(pfSet_Param("user_id", SqlDbType.NVarChar, User.Identity.Name))
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
                    dttGrid.Columns.Add("用途名称")
                    dttGrid.Columns.Add("削除")
                    drData = dttGrid.NewRow()
                    drData.Item("コード") = txtMcode.ppText.Trim
                    drData.Item("用途名称") = txtMcontent.ppText.Trim
                    drData.Item("削除") = "0"
                    dttGrid.Rows.Add(drData)

                ElseIf Master.ppBtnDelete.Text = "削除取消" Then

                    '削除取消の場合は対象レコードのみ表示
                    'msGet_Data()

                    dttGrid.Columns.Add("コード")
                    dttGrid.Columns.Add("用途名称")
                    dttGrid.Columns.Add("削除")
                    drData = dttGrid.NewRow()
                    drData.Item("コード") = txtMcode.ppText.Trim
                    drData.Item("用途名称") = txtMcontent.ppText.Trim
                    drData.Item("削除") = "0"
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
    ''' 既存データ取得処理(コード変更時)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_ExistData(ByRef strCode As String, ByRef strContent As String)

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
                    '.Add(pfSet_Param("code", SqlDbType.NVarChar, txtMcode.ppText.Trim))
                    .Add(pfSet_Param("code", SqlDbType.NVarChar, strCode))
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

                    '取得したデータを各項目に設定
                    strCode = dtsDB.Tables(0).Rows(0).Item("コード").ToString
                    strContent = dtsDB.Tables(0).Rows(0).Item("用途名称").ToString

                    '削除フラグ確認
                    If dtsDB.Tables(0).Rows(0).Item("削除").ToString = "0" Then

                        Master.ppBtnDelete.Text = "削除"
                        Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)

                        '状態設定
                        strMode = "Update"

                    Else

                        Master.ppBtnDelete.Text = "削除取消"
                        Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")

                        '状態設定
                        strMode = "Delete"

                    End If

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

        'フォーカス設定
        Dim afFocusID As String = String.Empty
        Select Case strMode
            Case "Insert"
                afFocusID = txtMcontent.ppTextBox.ClientID

            Case "Update"
                afFocusID = txtMcontent.ppTextBox.ClientID

            Case "Delete"
                afFocusID = Master.ppBtnClear.ClientID

        End Select

        Master.ppBtnDmy.Visible = True
        Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + afFocusID + ");"
        SetFocus(Master.ppBtnDmy.ClientID)

    End Sub

    ''' <summary>
    ''' 既存データ取得処理(コード変更時)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_SearchData(ByRef strCode As String, ByRef strContent As String)

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
                    .Add(pfSet_Param("code", SqlDbType.NVarChar, strCode))
                End With

                'リストデータ取得
                dtsDB = clsDataConnect.pfGet_DataSet(cmdDB)

                '既にデータが存在していた場合は各項目に設定
                If dtsDB.Tables(0).Rows.Count > 0 Then

                    '取得したデータを各項目に設定
                    strCode = dtsDB.Tables(0).Rows(0).Item("コード").ToString
                    strContent = dtsDB.Tables(0).Rows(0).Item("用途名称").ToString

                    '削除フラグ確認
                    If dtsDB.Tables(0).Rows(0).Item("削除").ToString = "0" Then
                    Else
                    End If
                Else
                    strContent = String.Empty
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
    End Sub

    ''' <summary>
    ''' 採番処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub btnGetSeq_Click()
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim dsData As DataSet
        Dim intRtn As Integer

        '連番テキストチェンジとの競合を避ける
        btnClear_Click()

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            '番号を採番する
            Try

                objCmd = New SqlCommand(DispCode & "_S3", objCn)
                With objCmd.Parameters
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                End With
                'データ取得
                dsData = clsDataConnect.pfGet_DataSet(objCmd)
                'ストアド戻り値チェック
                intRtn = Integer.Parse(objCmd.Parameters("retvalue").Value.ToString)

                If intRtn <> 0 Then
                    'エラー(登録件数MAX) 
                    psMesBox(Me, "30023", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "登録件数が最大値に達しています。")
                    Exit Sub
                End If

                If dsData.Tables(0).Rows.Count > 0 Then
                    With dsData.Tables(0).Rows(0)
                        Me.txtMcode.ppText = edit_num(.Item("NUM").ToString())
                    End With
                End If
                'フォーカスを移動
                'クリアボタンで表示されたダミーボタンを消す
                Master.ppBtnDmy.Visible = False
                Master.ppBtnDmy.Visible = True
                Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtMcontent.ppTextBox.ClientID + ");"
                SetFocus(Master.ppBtnDmy.ClientID)
                strMode = "Insert"

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle)
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If
        setVal("OFF")
    End Sub

    ''' <summary>
    ''' バリデータ操作
    ''' </summary>
    ''' <param name="OnOff"></param>
    ''' <remarks></remarks>
    Private Sub setVal(ByVal OnOff As String)
        Dim cuv_Seq As CustomValidator
        Dim sum_Seq As ValidationSummary
        cuv_Seq = txtMcode.FindControl("pnlErr").FindControl("cuvTextBox")
        sum_Seq = Master.FindControl("UpdPanelMain").FindControl("ValidSumKey")

        Select Case OnOff
            Case "ON"
                cuv_Seq.Visible = True
                sum_Seq.Visible = True

            Case "OFF"
                cuv_Seq.Visible = False
                sum_Seq.Visible = False

        End Select
    End Sub

    ''' <summary>
    ''' 連番１桁時先頭に０を追加
    ''' </summary>
    ''' <param name="num">連番</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Private Function edit_num(ByVal num As String) As String
        If Regex.IsMatch(num, "([0-9])", RegexOptions.ECMAScript) AndAlso num.Length = 1 Then
            num = "0" & num
        End If
        Return num
    End Function

#End Region

End Class
