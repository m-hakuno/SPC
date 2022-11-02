'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　持参物品選択マスタ
'*　ＰＧＭＩＤ：　COMUPDM77
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.11.17　：　栗原
'********************************************************************************************************************************

#Region "インポート定義"

Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive

#End Region

Public Class COMUPDM77

#Region "継承定義"

    Inherits System.Web.UI.Page

#End Region

#Region "定数定義"

    Const DispCode As String = "COMUPDM77"
    Const MasterName As String = "持参物品選択マスタ"
    Const TableName As String = "M77_PRINT"

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
        Dim intHeadCol As Integer() = New Integer() {1, 3} 'グリッドXML定義ファイル取得　ヘッダ修正パラメータ
        Dim intColSpan As Integer() = New Integer() {2, 2}
        pfSet_GridView(Me.grvList, DispCode, intHeadCol, intColSpan)

    End Sub

    ''' <summary>
    ''' Page_Load
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンアクション設定
        AddHandler Master.ppBtnClear.Click, AddressOf btnClear_Click
        AddHandler Master.ppBtnInsert.Command, AddressOf btn_Click
        AddHandler Master.ppBtnUpdate.Command, AddressOf btn_Click

        'コードイベント設定
        AddHandler ddlSys.SelectedIndexChanged, AddressOf ddlSys_SelectedIndexChanged
        AddHandler ddlDvs.ppDropDownList.SelectedIndexChanged, AddressOf ddlDvs_SelectedIndexChanged

        ddlSys.AutoPostBack = True
        ddlDvs.ppDropDownList.AutoPostBack = True

        Master.ppBtnDelete.Visible = False

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

            msSetddlSystem()

            '状態設定
            strMode = "Default"

            'フォーカス設定
            SetFocus(ddlSys.ClientID)

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
                Master.ppBtnClear.Enabled = True
                Master.ppBtnInsert.Enabled = False
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnDelete.Enabled = False
                ddlSys.Enabled = True
                ddlDvs.ppEnabled = True
                txtName.ppEnabled = False

            Case "Insert"
                Master.ppBtnClear.Enabled = True
                Master.ppBtnInsert.Enabled = True
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnDelete.Enabled = False
                ddlSys.Enabled = False
                ddlDvs.ppEnabled = False
                txtName.ppEnabled = True

            Case "Update"
                Master.ppBtnClear.Enabled = True
                Master.ppBtnInsert.Enabled = False
                Master.ppBtnUpdate.Enabled = True
                Master.ppBtnDelete.Enabled = True
                Master.ppBtnDelete.Text = "削除"
                ddlSys.Enabled = False
                ddlDvs.ppEnabled = False
                txtName.ppEnabled = True

        End Select

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
        ddlSys.SelectedValue = String.Empty
        ddlDvs.ppSelectedValue = String.Empty
        txtName.ppText = String.Empty

        '状態設定
        strMode = "Default"

        'フォーカス設定
        SetFocus(ddlSys.ClientID)

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 登録/更新 ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btn_Click(sender As Object, ByVal e As CommandEventArgs)

        'ログ出力開始
        psLogStart(Me)

        'データ取得
        If (Page.IsValid) Then

            '登録/更新/削除 処理
            msEdit_Data(e.CommandName)

            msGet_Data()

            btnClear_Click()

            '状態設定
            strMode = "Default"

        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' システムコード変更時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlSys_SelectedIndexChanged(sender As Object, ByVal e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        If ddlSys.SelectedValue <> String.Empty AndAlso ddlDvs.ppSelectedValue <> String.Empty Then
            '入力チェック
            Page.Validate("key")
            If (Page.IsValid) Then
                '既存データ取得処理
                msGet_ExistData()
            End If
        ElseIf ddlSys.SelectedValue <> String.Empty Then
            SetFocus(ddlDvs.ppDropDownList.ClientID)
        Else
            SetFocus(ddlSys.ClientID)
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 分類変更時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlDvs_SelectedIndexChanged(sender As Object, ByVal e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        If ddlSys.SelectedValue <> String.Empty AndAlso ddlDvs.ppSelectedValue <> String.Empty Then
            '入力チェック
            Page.Validate("key")
            If (Page.IsValid) Then
                '既存データ取得処理
                msGet_ExistData()
            End If
        ElseIf ddlDvs.ppSelectedValue <> String.Empty Then
            SetFocus(ddlSys.ClientID)
        Else
            SetFocus(ddlDvs.ppDropDownList.ClientID)
        End If

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
                arKey.Insert(0, CType(rowData.FindControl("システムコード"), TextBox).Text)
                arKey.Insert(0, CType(rowData.FindControl("分類コード"), TextBox).Text)

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
                ddlSys.SelectedValue = CType(rowData.FindControl("システムコード"), TextBox).Text
                ddlDvs.ppSelectedValue = CType(rowData.FindControl("分類コード"), TextBox).Text
                txtName.ppText = CType(rowData.FindControl("文言"), TextBox).Text

                '状態設定
                strMode = "Update"

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
    Protected Sub grvList_DataBound(sender As Object, ByVal e As EventArgs) Handles grvList.DataBound

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
    ''' 登録/更新 処理
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

        End Select

        'DB接続
        If clsDataConnect.pfOpen_Database(cnDB) Then

            Try

                cmdDB = New SqlCommand(strStored, cnDB)

                Select Case ipstrMode

                    Case "INSERT", "UPDATE"      '登録/更新
                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("Sys_Cod", SqlDbType.NVarChar, ddlSys.SelectedValue))
                            .Add(pfSet_Param("Dvs_Cod", SqlDbType.NVarChar, ddlDvs.ppSelectedValue))
                            .Add(pfSet_Param("Txt_Nam", SqlDbType.NVarChar, txtName.ppText.Trim))
                            .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))
                            .Add(pfSet_Param("user_id", SqlDbType.NVarChar, User.Identity.Name))
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

                '一覧更新
                msGet_Data()

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
    ''' 既存データ取得処理
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
                    .Add(pfSet_Param("Sys_Cod", SqlDbType.NVarChar, ddlSys.SelectedValue))
                    .Add(pfSet_Param("Dvs_Cod", SqlDbType.NVarChar, ddlDvs.ppSelectedValue))
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
                    arKey.Insert(0, ddlSys.SelectedValue)
                    arKey.Insert(0, ddlDvs.ppSelectedValue)

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
                    ddlSys.SelectedValue = dtsDB.Tables(0).Rows(0).Item("システムコード").ToString
                    ddlDvs.ppSelectedValue = dtsDB.Tables(0).Rows(0).Item("分類コード").ToString
                    txtName.ppText = dtsDB.Tables(0).Rows(0).Item("文言").ToString

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

        'フォーカス設定
        Master.ppBtnDmy.Visible = True
        Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtName.ppTextBox.ClientID + ");"
        SetFocus(Master.ppBtnDmy.ClientID)

    End Sub

    ''' <summary>
    ''' ドロップダウンリストシステム取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlSystem()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZMSTSEL004", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                '検索結果
                Me.ddlSys.Items.Clear()
                Me.ddlSys.DataSource = objDs.Tables(0)
                Me.ddlSys.DataTextField = "ＴＢＯＸリスト"
                Me.ddlSys.DataValueField = "ＴＢＯＸシステムコード"
                Me.ddlSys.DataBind()
                Me.ddlSys.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ機種マスタ一覧取得")
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

#End Region

End Class
