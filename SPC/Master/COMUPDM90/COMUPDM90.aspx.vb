'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　移動理由マスタ
'*　ＰＧＭＩＤ：　COMUPDM90
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.11.25　：　栗原
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

Public Class COMUPDM90

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
    Const DispCode As String = "COMUPDM90"                  '画面ID
    Const MasterName As String = "移動理由マスタ"               '画面名
    Const TableName As String = "M90_MOVE_REASON"            'テーブル名
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
    ''' Page Init
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        pfSet_GridView(Me.grvList, DispCode)
    End Sub

    ''' <summary>
    ''' Page Load
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        AddHandler Master.ppBtnInsert.Command, AddressOf btn_Click
        AddHandler Master.ppBtnUpdate.Command, AddressOf btn_Click
        AddHandler Master.ppBtnDelete.Command, AddressOf btn_Click
        AddHandler Master.ppBtnClear.Click, AddressOf btnClear_Click
        AddHandler txtCode.ppTextBox.TextChanged, AddressOf txtCode_TextChanged
        txtCode.ppTextBox.AutoPostBack = True

        Dim cuv As CustomValidator
        cuv = txtCode.FindControl("pnlErr").FindControl("cuvTextBox")
        AddHandler cuv.ServerValidate, AddressOf cuv_txtCode

        If Not IsPostBack Then
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            msGet_Data()

            SetFocus(txtCode.ppTextBox.ClientID)
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
                txtCode.ppEnabled = True
                txtName.ppEnabled = False
                Master.ppBtnClear.Enabled = True
                Master.ppBtnInsert.Enabled = False
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnDelete.Enabled = False

            Case "Insert"
                txtCode.ppEnabled = False
                txtName.ppEnabled = True
                Master.ppBtnClear.Enabled = True
                Master.ppBtnInsert.Enabled = True
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnDelete.Enabled = False

            Case "Select"
                txtCode.ppEnabled = False
                txtName.ppEnabled = True
                Master.ppBtnClear.Enabled = True
                Master.ppBtnInsert.Enabled = False
                Master.ppBtnDelete.Enabled = True
                If Master.ppBtnDelete.Text = "削除" Then
                    txtName.ppEnabled = True
                    Master.ppBtnUpdate.Enabled = True
                Else
                    txtName.ppEnabled = False
                    Master.ppBtnUpdate.Enabled = False
                End If
        End Select

    End Sub

    ''' <summary>
    ''' クリアボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click()
        psLogStart(Me)
        '排他情報削除処理
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

        txtCode.ppText = String.Empty
        txtName.ppText = String.Empty
        Master.ppBtnDelete.Text = "削除"
        Page.Validate("key")
        Master.ppBtnDmy.Visible = False
        strMode = "Default"
        SetFocus(txtCode.ppTextBox.ClientID)

        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 登録/更新/削除ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btn_Click(sender As Object, e As CommandEventArgs)
        'ログ出力開始
        psLogStart(Me)
        txtName.ppText = txtName.ppText.Trim
        If (Page.IsValid) Then
            msEditData(e.CommandName)
        End If
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' コード変更時の設定
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub txtCode_TextChanged(sender As Object, e As EventArgs)
        If txtCode.ppText.Trim <> String.Empty Then
            Page.Validate("key")
            If Page.IsValid Then
                txtCode.ppText = msZero_fill(txtCode.ppText.Trim)
                msExistData()
            End If
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
                        psLogEnd(Me)
                        Exit Sub
                    End If
                End If
                'ロック対象テーブル名の登録
                arTable_Name.Insert(0, TableName)
                'ロックテーブルキー項目の登録
                arKey.Insert(0, CType(rowData.FindControl("コード"), TextBox).Text)
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

                txtCode.ppText = CType(rowData.FindControl("コード"), TextBox).Text
                txtName.ppText = CType(rowData.FindControl("移動理由"), TextBox).Text
                If CType(rowData.FindControl("削除"), TextBox).Text = "1" Then
                    Master.ppBtnDelete.Text = "削除取消"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")
                Else
                    Master.ppBtnDelete.Text = "削除"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)
                End If
                Page.Validate("key")
                Master.ppBtnDmy.Visible = False
                SetFocus(txtName.ppTextBox.ClientID)
                strMode = "Select"

            End If
        Catch ex As Exception
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
            objStack = New StackFrame
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
        psLogEnd(Me)
    End Sub
    ''' <summary>
    ''' GRID DataBound
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound
        '削除データを赤文字で表示
        For i = 0 To grvList.Rows.Count - 1
            For z = 1 To grvList.Columns.Count - 1
                If CType(grvList.Rows(i).FindControl("削除"), TextBox).Text = 1 Then
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
        Dim delflg As String = String.Empty
        objStack = New StackFrame

        If Me.IsPostBack Then
            dispFlg = "0"
        Else
            dispFlg = "1"
        End If

        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(DispCode & "_S1", conDB)
                With cmdDB.Parameters
                    .Add(pfSet_Param("prog_id", SqlDbType.NVarChar, DispCode))
                    .Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, dispFlg))
                End With

                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    Master.ppCount = "0"
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                ElseIf CType(dstOrders.Tables(0).Rows(0).Item("総件数").ToString, Integer) > dstOrders.Tables(0).Rows.Count Then
                    Master.ppCount = dstOrders.Tables(0).Rows(0).Item("総件数").ToString
                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                                        dstOrders.Tables(0).Rows(0).Item("総件数").ToString, dstOrders.Tables(0).Rows.Count)
                Else
                    Master.ppCount = dstOrders.Tables(0).Rows(0).Item("総件数").ToString
                End If
                Me.grvList.DataSource = dstOrders.Tables(0)
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
        Dim getFlg As String = "0"

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
                        getFlg = "1"
                    Case "削除取消"
                        MesCode = "00001"
                        strStored = DispCode & "_D1"
                        getFlg = "0"
                End Select
        End Select

        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(strStored, conDB)
                Select Case ipstrMode
                    Case "INSERT", "UPDATE"
                        With cmdDB.Parameters
                            .Add(pfSet_Param("code", SqlDbType.NVarChar, txtCode.ppText.Trim))          'コード
                            .Add(pfSet_Param("name", SqlDbType.NVarChar, txtName.ppText.Trim))          '移動理由
                            .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))                  '処理区分
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))         'ユーザーＩＤ
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
                        End With
                    Case "DELETE"
                        With cmdDB.Parameters
                            .Add(pfSet_Param("code", SqlDbType.NVarChar, txtCode.ppText.Trim))          'コード
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))         'ユーザーＩＤ
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
                        End With
                End Select
                Using conTrn = conDB.BeginTransaction
                    cmdDB.Transaction = conTrn
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()

                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        conTrn.Rollback()
                        psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                        Exit Sub
                    End If
                    conTrn.Commit()
                End Using

                msGet_Data()
                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName)
                btnClear_Click()

            Catch ex As Exception
                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If
    End Sub

    ''' <summary>
    ''' 既存データ取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msExistData()
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(DispCode & "_S2", conDB)
                With cmdDB.Parameters
                    .Add(pfSet_Param("code", SqlDbType.NVarChar, txtCode.ppText.Trim))
                End With
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
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
                            psLogEnd(Me)
                            Exit Sub
                        End If
                    End If
                    'ロック対象テーブル名の登録
                    arTable_Name.Insert(0, TableName)
                    'ロックテーブルキー項目の登録
                    arKey.Insert(0, txtCode.ppText.Trim)
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
                    txtCode.ppText = dstOrders.Tables(0).Rows(0).Item("コード").ToString
                    txtName.ppText = dstOrders.Tables(0).Rows(0).Item("移動理由").ToString
                    If dstOrders.Tables(0).Rows(0).Item("削除").ToString = "1" Then
                        Master.ppBtnDelete.Text = "削除取消"
                        Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")
                    Else
                        Master.ppBtnDelete.Text = "削除"
                        Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)
                    End If
                    strMode = "Select"
                Else
                    strMode = "Insert"
                End If

                Master.ppBtnDmy.Visible = True
                Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtName.ppTextBox.ClientID + ");"
                SetFocus(Master.ppBtnDmy.ClientID)

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
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
    Private Sub cuv_txtCode(source As Object, args As ServerValidateEventArgs)
        Select Case txtCode.ppText.Trim
            Case "0", "00"
                source.text = "形式エラー"
                source.ErrorMessage = "コード は１以上の半角数値で入力してください。"
                args.IsValid = False
                Exit Sub
        End Select
    End Sub

#End Region

#Region "終了処理プロシージャ"

#End Region

End Class
