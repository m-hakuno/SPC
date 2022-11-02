'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　機器分類マスタ
'*　ＰＧＭＩＤ：　COMUPDM73
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.11.20　：　栗原
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

Public Class COMUPDM73

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
    Const DispCode As String = "COMUPDM73"                   '画面ID

    'マスタ情報
    Const MasterName As String = "機器分類マスタ"            'マスタ名
    Const TableName As String = "M73_APPA_GROUPING"            'テーブル名

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
        Dim intHeadCol As Integer() = New Integer() {1}
        Dim intColSpan As Integer() = New Integer() {2}
        pfSet_GridView(Me.grvList, DispCode, intHeadCol, intColSpan)
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
        Master.ppBtnDelete.Visible = False

        AddHandler txtAppaCd.ppTextBox.TextChanged, AddressOf txtAppaCd_TextChanged
        txtAppaCd.ppTextBox.AutoPostBack = True

        Dim pnl As UpdatePanel
        pnl = Master.FindControl("UpdPanelMain")
        pnl.UpdateMode = UpdatePanelUpdateMode.Always

        '桁数チェック用
        Dim cuv As CustomValidator
        cuv = txtAppaCd.FindControl("pnlErr").FindControl("cuvTextBox")
        AddHandler cuv.ServerValidate, AddressOf cuv_AppaCode

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
            SetFocus(txtAppaCd.ppTextBox.ClientID)

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
                txtAppaCd.ppEnabled = True
                txtDvsCd.ppEnabled = False
                txtName.ppEnabled = False
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnInsert.Enabled = False      '登録
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnDelete.Text = "削除"

            Case "Insert"
                txtAppaCd.ppEnabled = False
                txtDvsCd.ppEnabled = True
                txtName.ppEnabled = True
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnInsert.Enabled = True      '登録
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnDelete.Text = "削除"

            Case "Select"
                txtAppaCd.ppEnabled = False
                txtDvsCd.ppEnabled = True
                txtName.ppEnabled = True
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnInsert.Enabled = False     '登録
                Master.ppBtnUpdate.Enabled = True      '更新
                Master.ppBtnDelete.Enabled = True      '削除
                Master.ppBtnDelete.Text = "削除"

            Case "Delete"
                txtAppaCd.ppEnabled = False
                txtDvsCd.ppEnabled = True
                txtName.ppEnabled = True
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnInsert.Enabled = False     '登録
                Master.ppBtnUpdate.Enabled = False       '更新
                Master.ppBtnDelete.Enabled = True      '削除
                Master.ppBtnDelete.Text = "削除取消"
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

        txtAppaCd.ppText = String.Empty
        txtDvsCd.ppText = String.Empty
        txtName.ppText = String.Empty

        Page.Validate("key") 'エラーサマリーだけ残る事象を防ぐ
        strMode = "Default"

        'フォーカス
        SetFocus(txtAppaCd.ppTextBox.ClientID)

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
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' キー項目変更時の設定
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub txtAppaCd_TextChanged(sender As Object, e As EventArgs)

        If txtAppaCd.ppText.Trim <> String.Empty Then
            Page.Validate("key")
            If Page.IsValid Then
                txtAppaCd.ppText = msZero_fill(txtAppaCd.ppText.Trim)
                msExist_Data()
            End If
        Else
            'フォーカス
            SetFocus(txtAppaCd.ppTextBox.ClientID)
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
                arKey.Insert(0, CType(rowData.FindControl("機器分類コード"), TextBox).Text)

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

                txtAppaCd.ppText = CType(rowData.FindControl("機器分類コード"), TextBox).Text
                txtDvsCd.ppText = CType(rowData.FindControl("区分"), TextBox).Text
                txtName.ppText = CType(rowData.FindControl("名称"), TextBox).Text

                SetFocus(txtName.ppTextBox.ClientID)
                Page.Validate("key")
                strMode = "Select"

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
                    .Add(pfSet_Param("Appa_Cd", SqlDbType.NVarChar, ""))
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
    ''' 既存データ取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msExist_Data()
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(DispCode & "_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("Appa_Cd", SqlDbType.NVarChar, txtAppaCd.ppText.Trim))
                    .Add(pfSet_Param("prog_id", SqlDbType.NVarChar, DispCode))
                    .Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, "0"))
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
                    arKey.Insert(0, txtAppaCd.ppText.Trim)
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
                    txtDvsCd.ppText = dstOrders.Tables(0).Rows(0).Item("区分").ToString
                    txtName.ppText = dstOrders.Tables(0).Rows(0).Item("名称").ToString

                    strMode = "Select"
                Else
                    strMode = "Insert"
                End If

                Master.ppBtnDmy.Visible = True
                Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtName.ppTextBox.ClientID + ");"
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
                            .Add(pfSet_Param("Appa_Cd", SqlDbType.NVarChar, txtAppaCd.ppText.Trim))
                            .Add(pfSet_Param("Dvs_Cd", SqlDbType.NVarChar, txtDvsCd.ppText.Trim))
                            .Add(pfSet_Param("Name", SqlDbType.NVarChar, txtName.ppText.Trim))
                            .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
                        End With

                    Case "DELETE"               '削除
                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("start_dt", SqlDbType.NVarChar, txtAppaCd.ppText.Trim))
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
    ''' コードゼロチェック
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuv_AppaCode(source As Object, args As ServerValidateEventArgs)
        Select Case txtAppaCd.ppText.Trim
            Case "0", "00"
                source.text = "形式エラー"
                source.ErrorMessage = "コード は０以上の半角数値で入力してください。"
                args.IsValid = False
                Exit Sub
        End Select
    End Sub
#End Region

#Region "終了処理プロシージャ"

#End Region

End Class