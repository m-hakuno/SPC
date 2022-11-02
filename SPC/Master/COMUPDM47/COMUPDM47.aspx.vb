'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　梱包材マスタ
'*　ＰＧＭＩＤ：　COMUPDM47
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.11.26　：　武
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

Public Class COMUPDM47

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
    Const DispCode As String = "COMUPDM47"                   '画面ID

    'マスタ情報
    Const MasterName1 As String = "梱包材マスタ"            'マスタ名
    Const TableName1 As String = "M47_PACKING"            'テーブル名

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

        AddHandler txtProdCd.ppTextBox.TextChanged, AddressOf txtprodcd_TextChanged
        txtProdCd.ppTextBox.AutoPostBack = True

        If Not IsPostBack Then

            'プログラムＩＤ、画面名、件数
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName1
            Master.ppCount = "0"

            '画面制御
            'Master.ppBtnInsert.Visible = False
            'Master.ppBtnDelete.Visible = False

            'パンくずリスト(ページ階級)設定
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            'グリッドの初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

            '初期化
            Clear()

            'データ取得
            msGet_Data()

            'フォーカス設定
            SetFocus(txtProdCd.ClientID)

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
                txtProdCd.ppEnabled = True
                txtProdNm.ppEnabled = False
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnInsert.Enabled = False      '登録
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnDelete.Text = "削除"
                Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName1)
            Case "Insert"
                txtProdCd.ppEnabled = False
                txtProdNm.ppEnabled = True
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnInsert.Enabled = True      '登録
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnDelete.Text = "削除"
                Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName1)
            Case "Select"
                txtProdCd.ppEnabled = False
                txtProdNm.ppEnabled = True
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnInsert.Enabled = False     '登録
                Master.ppBtnUpdate.Enabled = True      '更新
                Master.ppBtnDelete.Enabled = True      '削除
                Master.ppBtnDelete.Text = "削除"
                Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName1)
            Case "Delete"
                txtProdCd.ppEnabled = False
                txtProdNm.ppEnabled = False
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppBtnInsert.Enabled = False     '登録
                Master.ppBtnUpdate.Enabled = False       '更新
                Master.ppBtnDelete.Enabled = True      '削除
                Master.ppBtnDelete.Text = "削除取消"
                Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName1 & "の削除取消")
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

        '初期化
        Clear()

        'フォーカス
        SetFocus(txtProdCd.ppTextBox.ClientID)
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
            SetFocus(txtProdCd.ppTextBox.ClientID)

            '初期化
            Clear()
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
            'グリッドの選択ボタン
            If e.CommandName = "Select" Then
                'ログ出力開始
                psLogStart(Me)
                '排他制御処理
                Dim strExclusiveDate As String = Nothing
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList
                Dim rowData As GridViewRow = grvList.Rows(Convert.ToInt32(e.CommandArgument))
                Dim strLmpFlg As String = "0"

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
                arTable_Name.Insert(0, TableName1)

                'ロックテーブルキー項目の登録
                arKey.Insert(0, CType(rowData.FindControl("梱包材コード"), TextBox).Text)

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

                ''ランプフラグの設定
                'If CType(rowData.FindControl("ランプフラグ"), TextBox).Text = "●" Then
                '    strLmpFlg = "1"
                'Else
                '    strLmpFlg = "0"
                'End If

                '登録エリアに値を入力
                txtProdCd.ppText = CType(rowData.FindControl("梱包材コード"), TextBox).Text
                txtProdNm.ppText = CType(rowData.FindControl("梱包材名称"), TextBox).Text

                'フォーカスをtxtprodnmへ
                SetFocus(txtProdNm.ppTextBox.ClientID)

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

        '削除データは赤文字で表示()
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
                    .Add(pfSet_Param("Prod_Cd", SqlDbType.NVarChar, ""))
                    .Add(pfSet_Param("SearchCls", SqlDbType.NVarChar, "0"))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                Master.ppCount = dstOrders.Tables(0).Rows(0).Item("件数")

                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                'エラーメッセージ表示
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName1)
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
                            .Add(pfSet_Param("Prod_Cd", SqlDbType.NVarChar, Me.txtProdCd.ppText))
                            .Add(pfSet_Param("Prod_Nm", SqlDbType.NVarChar, Me.txtProdNm.ppText))
                            .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
                        End With

                    Case "DELETE"               '削除
                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("Prod_Cd", SqlDbType.NVarChar, Me.txtProdCd.ppText))
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
                        psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName1)
                        Exit Sub
                    End If
                    'コミット
                    cntrn.Commit()
                End Using

                msGet_Data()

                btnClear_Click()

                '完了メッセージ
                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName1)

            Catch ex As Exception

                'エラー
                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName1)
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
    ''' 機器種別コード変更時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub txtprodcd_TextChanged(sender As Object, e As EventArgs)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(DispCode & "_S1", conDB)
                With cmdDB.Parameters
                    'データグリッドパラメータ設定
                    .Add(pfSet_Param("Prod_Cd", SqlDbType.NVarChar, txtProdCd.ppText.ToString))
                    .Add(pfSet_Param("SearchCls", SqlDbType.NVarChar, "1"))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'ログ出力開始
                psLogStart(Me)

                If dstOrders.Tables(0).Rows.Count > 0 Then

                    '排他制御処理
                    Dim strExclusiveDate As String = Nothing
                    Dim arTable_Name As New ArrayList
                    Dim arKey As New ArrayList
                    Dim strLmpFlg As String = "0"

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
                    arTable_Name.Insert(0, TableName1)

                    'ロックテーブルキー項目の登録
                    arKey.Insert(0, dstOrders.Tables(0).Rows(0).Item("梱包材コード"))

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


                    '登録エリアに値を入力
                    txtProdCd.ppText = dstOrders.Tables(0).Rows(0).Item("梱包材コード")
                    txtProdNm.ppText = dstOrders.Tables(0).Rows(0).Item("梱包材名称")

                    'フォーカスをtxtPerへ
                    'SetFocus(txtErrNm.ppTextBox.ClientID)
                    'フォーカスを移動

                    If dstOrders.Tables(0).Rows(0).Item("削除フラグ").ToString = "1" Then
                        strMode = "Delete"
                    Else
                        strMode = "Select"
                    End If
                Else
                    strMode = "Insert"
                End If

                'クリアボタンで表示されたダミーボタンを消す
                Master.ppBtnDmy.Visible = False
                Master.ppBtnDmy.Visible = True
                Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtProdNm.ppTextBox.ClientID + ");"
                SetFocus(Master.ppBtnDmy.ClientID)

                Page.Validate("key")


            Catch ex As Exception
                'エラーメッセージ表示
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName1)
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


        'フォーカスをtxtErrNmへ

    End Sub

    ''' <summary>
    ''' 初期化
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Clear()

        '初期化
        txtProdCd.ppText = String.Empty
        txtProdNm.ppText = String.Empty

        'ページの状態=Defaultに
        strMode = "Default"

    End Sub

#End Region

#Region "終了処理プロシージャ"

#End Region

End Class
