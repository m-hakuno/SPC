'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　特別保守料金マスタ
'*　ＰＧＭＩＤ：　COMUPDM96
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2016.08.25　：　稲葉
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'COMUPDM96-001

#Region "インポート定義"

Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive

#End Region

Public Class COMUPDM96

#Region "継承定義"

    Inherits System.Web.UI.Page

#End Region

#Region "定数定義"

    Const DispCode As String = "COMUPDM96"
    Const MasterName As String = "特別保守料金マスタ"
    Const TableName As String = "M96_SP_MNT_RATE"

#End Region

#Region "変数定義"

    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsExc As New ClsCMExclusive
    Dim clsMst As New ClsMSTCommon
    Dim objStack As StackFrame
    Dim strMode As String = Nothing
    Dim strBtnMode As String = Nothing

#End Region

#Region "イベントプロージャ"

    ''' <summary>
    ''' Page_Init
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init

        Dim intHeadCol As Integer() = New Integer() {1} 'グリッドXML定義ファイル取得　ヘッダ修正パラメータ
        Dim intColSpan As Integer() = New Integer() {2}
        pfSet_GridView(Me.grvList, DispCode, intHeadCol, intColSpan)

    End Sub

    ''' <summary>
    ''' Page_Load
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンアクション設定
        'AddHandler Master.ppBtnClear.Click, AddressOf btnClear_Click
        AddHandler Master.ppBtnClear.Command, AddressOf btnClear_Click
        AddHandler Master.ppBtnInsert.Command, AddressOf btn_Click
        AddHandler Master.ppBtnUpdate.Command, AddressOf btn_Click
        AddHandler Master.ppBtnDelete.Command, AddressOf btn_Click

        'コードイベント設定
        AddHandler txtSpPriceCd.ppTextBox.TextChanged, AddressOf txtSpPriceCd_TextChanged
        txtSpPriceCd.ppTextBox.AutoPostBack = True

        '入力検証用
        Dim cuv_Price1_Start As CustomValidator
        Dim cuv_Price1_End As CustomValidator
        Dim cuv_Price2_Start As CustomValidator
        Dim cuv_Price2_End As CustomValidator
        cuv_Price1_Start = tmbStartTm1.FindControl("pnlErr").FindControl("cuvTimeBox")
        AddHandler cuv_Price1_Start.ServerValidate, AddressOf CstmVal_StartN_ServerValidate

        cuv_Price1_End = tmbEndTm1.FindControl("pnlErr").FindControl("cuvTimeBox")
        AddHandler cuv_Price1_End.ServerValidate, AddressOf CstmVal_EndN_ServerValidate

        cuv_Price2_Start = tmbStartTm2.FindControl("pnlErr").FindControl("cuvTimeBox")
        AddHandler cuv_Price2_Start.ServerValidate, AddressOf CstmVal_StartS_ServerValidate

        cuv_Price2_End = tmbEndTm2.FindControl("pnlErr").FindControl("cuvTimeBox")
        AddHandler cuv_Price2_End.ServerValidate, AddressOf CstmVal_EndS_ServerValidate

        'エラーサマリー活性制御の為のUpdatePanelのUPdateMode調整
        Dim pnl As UpdatePanel
        pnl = Master.FindControl("UpdPanelMain")
        pnl.UpdateMode = UpdatePanelUpdateMode.Always
        pnl = Master.FindControl("UpdPanelSearch")
        pnl.UpdateMode = UpdatePanelUpdateMode.Always

        'スクリプト埋め込み
        txtPrice1_b.Attributes.Add("onfocus", "setdeletezero(" & txtPrice1_b.ClientID & ");")
        txtPrice2_b.Attributes.Add("onfocus", "setdeletezero(" & txtPrice2_b.ClientID & ");")
        txtPrice1_b.Attributes.Add("onblur", "settotaljippi(" & txtPrice1_b.ClientID & ",0,0);")
        txtPrice2_b.Attributes.Add("onblur", "settotaljippi(" & txtPrice2_b.ClientID & ",0,0);")


        Master.ppBtnClear.CausesValidation = False
        Master.ppBtnInsert.Enabled = False
        Master.ppBtnDelete.Enabled = False
        Master.ppBtnSearch.Visible = False
        Master.ppBtnSrcClear.Visible = False

        txtSpPriceNm.ppTextBox.AutoPostBack = True
        txtPrice1_b.AutoPostBack = True
        tmbStartTm1.ppHourBox.AutoPostBack = True
        tmbStartTm1.ppMinBox.AutoPostBack = True
        tmbEndTm1.ppHourBox.AutoPostBack = True
        tmbEndTm1.ppMinBox.AutoPostBack = True
        txtPrice2_b.AutoPostBack = True
        tmbStartTm2.ppHourBox.AutoPostBack = True
        tmbStartTm2.ppMinBox.AutoPostBack = True
        tmbEndTm2.ppHourBox.AutoPostBack = True
        tmbEndTm2.ppMinBox.AutoPostBack = True

        If Not IsPostBack Then

            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

            msGet_Data()

            strMode = "Default"
            strBtnMode = "Default"
            SetFocus(txtSpPriceCd.ppTextBox.ClientID)

        End If
    End Sub

    ''' <summary>
    ''' Page_PreRender
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        msEnableCtrl_btn(strMode, strBtnMode)
        msEnableCtrl_obj(strMode)
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
                psLogEnd(Me)
                Exit Sub
            End If
        End If

        '初期化処理
        Me.txtSpPriceCd.ppText = String.Empty
        Me.txtSpPriceNm.ppText = String.Empty
        Me.txtPrice1_b.Text = "0"
        Me.tmbStartTm1.ppHourText = String.Empty
        Me.tmbStartTm1.ppMinText = String.Empty
        Me.tmbEndTm1.ppHourText = String.Empty
        Me.tmbEndTm1.ppMinText = String.Empty
        Me.txtPrice2_b.Text = "0"
        Me.tmbStartTm2.ppHourText = String.Empty
        Me.tmbStartTm2.ppMinText = String.Empty
        Me.tmbEndTm2.ppHourText = String.Empty
        Me.tmbEndTm2.ppMinText = String.Empty

        strMode = "Default"
        strBtnMode = "Normal"

        If Me.txtSpPriceCd.ppText <> String.Empty Then
            '不正入力との競合時、エラーサマリーが残る事象を防ぐ
            Page.Validate("key")
            '既存データ取得処理と競合時、ダミーボタンが残る事象を防ぐ
            Master.ppBtnDmy.Visible = False
        End If

        SetFocus(txtSpPriceCd.ppTextBox.ClientID)

        'If hdnDtl_SelectFLG.Value = "1" Then
        '    msGet_Data()
        '    hdnDtl_SelectFLG.Value = "0"
        'End If

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 登録/更新/削除　ボタン押下時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btn_Click(ByVal sender As Object, ByVal e As CommandEventArgs)

        'ログ出力開始
        psLogStart(Me)

        'データ取得
        If (Page.IsValid) Then

            msEdit_Data(e.CommandName)
            btnClear_Click()
            strMode = "Default"
            strBtnMode = "Normal"
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 特別保守料金コード変更時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub txtSpPriceCd_TextChanged(ByVal sender As Object, ByVal e As EventArgs)

        'ログ出力開始
        psLogStart(Me)
        If txtSpPriceCd.ppText <> String.Empty Then
            Page.Validate("key")
            If (Page.IsValid) Then
                Dim intCd As Integer
                If Integer.TryParse(Me.txtSpPriceCd.ppText, intCd) = True Then
                    Me.txtSpPriceCd.ppText = intCd
                End If

                '既存データ取得処理
                msGet_ExistData()
            End If

        Else
            Me.txtSpPriceCd.ppText = String.Empty
            Me.txtSpPriceNm.ppText = String.Empty
            Me.txtPrice1_b.Text = "0"
            Me.tmbStartTm1.ppHourText = String.Empty
            Me.tmbStartTm1.ppMinText = String.Empty
            Me.tmbEndTm1.ppHourText = String.Empty
            Me.tmbEndTm1.ppMinText = String.Empty
            Me.txtPrice2_b.Text = "0"
            Me.tmbStartTm2.ppHourText = String.Empty
            Me.tmbStartTm2.ppMinText = String.Empty
            Me.tmbEndTm2.ppHourText = String.Empty
            Me.tmbEndTm2.ppMinText = String.Empty
            strMode = "Default"
            strBtnMode = "Normal"
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
                '初期化
                'btnClear_Click()

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
                arKey.Insert(0, CType(rowData.FindControl("特別保守料金コード"), TextBox).Text)
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
                txtSpPriceCd.ppText = DirectCast(rowData.FindControl("特別保守料金コード"), TextBox).Text
                txtSpPriceNm.ppText = DirectCast(rowData.FindControl("特別保守料金名称"), TextBox).Text

                If DirectCast(rowData.FindControl("料金1"), TextBox).Text <> String.Empty Then
                    txtPrice1_b.Text = DirectCast(rowData.FindControl("料金1"), TextBox).Text.Replace(",", "")
                End If
                If DirectCast(rowData.FindControl("開始時刻1"), TextBox).Text <> String.Empty Then
                    tmbStartTm1.ppHourText = DirectCast(rowData.FindControl("開始時刻1"), TextBox).Text.Split(":")(0)
                    tmbStartTm1.ppMinText = DirectCast(rowData.FindControl("開始時刻1"), TextBox).Text.Split(":")(1)
                End If
                If DirectCast(rowData.FindControl("終了時刻1"), TextBox).Text <> String.Empty Then
                    tmbEndTm1.ppHourText = DirectCast(rowData.FindControl("終了時刻1"), TextBox).Text.Split(":")(0)
                    tmbEndTm1.ppMinText = DirectCast(rowData.FindControl("終了時刻1"), TextBox).Text.Split(":")(1)
                End If
                If DirectCast(rowData.FindControl("料金2"), TextBox).Text <> String.Empty Then
                    txtPrice2_b.Text = DirectCast(rowData.FindControl("料金2"), TextBox).Text.Replace(",", "")
                End If
                If DirectCast(rowData.FindControl("開始時刻2"), TextBox).Text <> String.Empty Then
                    tmbStartTm2.ppHourText = DirectCast(rowData.FindControl("開始時刻2"), TextBox).Text.Split(":")(0)
                    tmbStartTm2.ppMinText = DirectCast(rowData.FindControl("開始時刻2"), TextBox).Text.Split(":")(1)
                End If
                If DirectCast(rowData.FindControl("終了時刻2"), TextBox).Text <> String.Empty Then
                    tmbEndTm2.ppHourText = DirectCast(rowData.FindControl("終了時刻2"), TextBox).Text.Split(":")(0)
                    tmbEndTm2.ppMinText = DirectCast(rowData.FindControl("終了時刻2"), TextBox).Text.Split(":")(1)
                End If

                strMode = "Edit"
                strBtnMode = "Upd"

                '不正入力との競合時、エラーサマリーが残る事象を防ぐ
                Page.Validate("key")
                '既存データ取得処理と競合時、ダミーボタンが残る事象を防ぐ
                Master.ppBtnDmy.Visible = False

                SetFocus(txtSpPriceNm.ppTextBox.ClientID)

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

                With cmdDB.Parameters
                    .Add(pfSet_Param("SCdFrom", SqlDbType.NVarChar, String.Empty))
                    .Add(pfSet_Param("SCdTo", SqlDbType.NVarChar, String.Empty))
                    .Add(pfSet_Param("SNm", SqlDbType.NVarChar, String.Empty))
                    .Add(pfSet_Param("prog_id", SqlDbType.NVarChar, DispCode))
                    .Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, dispflg))
                End With
                dtsDB = clsDataConnect.pfGet_DataSet(cmdDB)

                If dtsDB.Tables(0).Rows.Count = 0 Then
                    '0件
                    Master.ppCount = "0"
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)

                ElseIf CType(dtsDB.Tables(0).Rows(0).Item("総件数").ToString, Integer) > dtsDB.Tables(0).Rows.Count Then
                    '上限オーバー
                    Master.ppCount = dtsDB.Tables(0).Rows(0).Item("総件数").ToString
                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                                        dtsDB.Tables(0).Rows(0).Item("総件数").ToString, dtsDB.Tables(0).Rows.Count)
                Else
                    Master.ppCount = dtsDB.Tables(0).Rows(0).Item("総件数").ToString
                End If
                grvList.DataSource = dtsDB.Tables(0)
                grvList.DataBind()

            Catch ex As Exception
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
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
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

                With cmdDB.Parameters
                    .Add(pfSet_Param("SpPriceCd", SqlDbType.NVarChar, txtSpPriceCd.ppText))
                    .Add(pfSet_Param("SpPriceNm", SqlDbType.NVarChar, txtSpPriceNm.ppText))
                    If txtPrice1_b.Text = String.Empty Then
                        .Add(pfSet_Param("Price1", SqlDbType.NVarChar, 0))
                    Else
                        .Add(pfSet_Param("Price1", SqlDbType.NVarChar, txtPrice1_b.Text))
                    End If
                    If tmbStartTm1.ppHourText <> String.Empty AndAlso tmbStartTm1.ppMinText <> String.Empty Then
                        .Add(pfSet_Param("StartTm1", SqlDbType.NVarChar, tmbStartTm1.ppHourText & ":" & tmbStartTm1.ppMinText))
                    Else
                        .Add(pfSet_Param("StartTm1", SqlDbType.NVarChar, String.Empty))
                    End If
                    If tmbEndTm1.ppHourText <> String.Empty AndAlso tmbEndTm1.ppMinText <> String.Empty Then
                        .Add(pfSet_Param("EndTm1", SqlDbType.NVarChar, tmbEndTm1.ppHourText & ":" & tmbEndTm1.ppMinText))
                    Else
                        .Add(pfSet_Param("EndTm1", SqlDbType.NVarChar, String.Empty))
                    End If
                    If txtPrice2_b.Text = String.Empty Then
                        .Add(pfSet_Param("Price2", SqlDbType.NVarChar, 0))
                    Else
                        .Add(pfSet_Param("Price2", SqlDbType.NVarChar, txtPrice2_b.Text))
                    End If
                    If tmbStartTm2.ppHourText <> String.Empty AndAlso tmbStartTm2.ppMinText <> String.Empty Then
                        .Add(pfSet_Param("StartTm2", SqlDbType.NVarChar, tmbStartTm2.ppHourText & ":" & tmbStartTm2.ppMinText))
                    Else
                        .Add(pfSet_Param("StartTm2", SqlDbType.NVarChar, String.Empty))
                    End If
                    If tmbEndTm2.ppHourText <> String.Empty AndAlso tmbEndTm2.ppMinText <> String.Empty Then
                        .Add(pfSet_Param("EndTm2", SqlDbType.NVarChar, tmbEndTm2.ppHourText & ":" & tmbEndTm2.ppMinText))
                    Else
                        .Add(pfSet_Param("EndTm2", SqlDbType.NVarChar, String.Empty))
                    End If
                    .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))
                    .Add(pfSet_Param("user_id", SqlDbType.NVarChar, User.Identity.Name))
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                End With

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

                msGet_Data()
                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName)

            Catch ex As Exception
                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                'ログ出力
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
                With cmdDB.Parameters
                    .Add(pfSet_Param("SpPriceCd", SqlDbType.NVarChar, txtSpPriceCd.ppText))
                End With
                dtsDB = clsDataConnect.pfGet_DataSet(cmdDB)

                If dtsDB.Tables(0).Rows.Count = 1 Then
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
                    arKey.Insert(0, txtSpPriceCd.ppText)
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
                    txtSpPriceCd.ppText = dtsDB.Tables(0).Rows(0).Item("特別保守料金コード").ToString
                    txtSpPriceNm.ppText = dtsDB.Tables(0).Rows(0).Item("特別保守料金名称").ToString
                    txtPrice1_b.Text = dtsDB.Tables(0).Rows(0).Item("料金1").ToString.Split(".")(0)

                    If dtsDB.Tables(0).Rows(0).Item("開始時刻1").ToString <> String.Empty Then
                        tmbStartTm1.ppHourText = dtsDB.Tables(0).Rows(0).Item("開始時刻1").ToString.Split(":")(0)
                        tmbStartTm1.ppMinText = dtsDB.Tables(0).Rows(0).Item("開始時刻1").ToString.Split(":")(1)
                    End If
                    If dtsDB.Tables(0).Rows(0).Item("終了時刻1").ToString <> String.Empty Then
                        tmbEndTm1.ppHourText = dtsDB.Tables(0).Rows(0).Item("終了時刻1").ToString.Split(":")(0)
                        tmbEndTm1.ppMinText = dtsDB.Tables(0).Rows(0).Item("終了時刻1").ToString.Split(":")(1)
                    End If
                    txtPrice2_b.Text = dtsDB.Tables(0).Rows(0).Item("料金2").ToString.Split(".")(0)

                    If dtsDB.Tables(0).Rows(0).Item("開始時刻2").ToString <> String.Empty Then
                        tmbStartTm2.ppHourText = dtsDB.Tables(0).Rows(0).Item("開始時刻2").ToString.Split(":")(0)
                        tmbStartTm2.ppMinText = dtsDB.Tables(0).Rows(0).Item("開始時刻2").ToString.Split(":")(1)
                    End If
                    If dtsDB.Tables(0).Rows(0).Item("終了時刻2").ToString <> String.Empty Then
                        tmbEndTm2.ppHourText = dtsDB.Tables(0).Rows(0).Item("終了時刻2").ToString.Split(":")(0)
                        tmbEndTm2.ppMinText = dtsDB.Tables(0).Rows(0).Item("終了時刻2").ToString.Split(":")(1)
                    End If

                    strMode = "Edit"
                    strBtnMode = "Upd"

                    SetFocus(txtSpPriceNm.ppTextBox.ClientID)
                Else
                    clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "")
                    strMode = "Default"
                    strBtnMode = "Normal"

                    SetFocus(txtSpPriceCd.ppTextBox.ClientID)
                End If


            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                'ログ出力
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
    ''' 入力欄活性制御
    ''' </summary>
    ''' <param name="strMode"></param>
    ''' <remarks>2015/12/09　栗原
    ''' ｢モード｣は機器種別選択時まで無効化しないよう修正</remarks>
    Private Sub msEnableCtrl_obj(ByVal strMode As String)
        Select Case strMode
            Case "Default"
                Me.txtSpPriceCd.ppEnabled = True
                Me.txtSpPriceNm.ppEnabled = False
                Me.txtPrice1_b.Enabled = False
                Me.tmbStartTm1.ppEnabled = False
                Me.tmbEndTm1.ppEnabled = False
                Me.txtPrice2_b.Enabled = False
                Me.tmbStartTm2.ppEnabled = False
                Me.tmbEndTm2.ppEnabled = False

                SetFocus(txtSpPriceCd.ppTextBox.ClientID)

            Case "Edit"
                Me.txtSpPriceCd.ppEnabled = False
                Me.txtSpPriceNm.ppEnabled = True
                Me.txtPrice1_b.Enabled = True
                Me.tmbStartTm1.ppEnabled = True
                Me.tmbEndTm1.ppEnabled = True
                Me.txtPrice2_b.Enabled = True
                Me.tmbStartTm2.ppEnabled = True
                Me.tmbEndTm2.ppEnabled = True

                'フォーカス設定
                Dim afFocusID As String = String.Empty
                afFocusID = txtSpPriceNm.ppTextBox.ClientID
                Master.ppBtnDmy.Visible = True
                Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + afFocusID + ");"
                SetFocus(Master.ppBtnDmy.ClientID)

        End Select
    End Sub

    ''' <summary>
    ''' ボタン活性制御
    ''' </summary>
    ''' <param name="strMode"></param>
    ''' <remarks></remarks>
    Private Sub msEnableCtrl_btn(ByVal strMode As String, ByVal strBtnMode As String)
        Master.ppBtnClear.Enabled = True
        Select Case strMode

            Case "Default"
                Master.ppBtnInsert.Enabled = False
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnDelete.Enabled = False

            Case "Edit"
                Select Case strBtnMode
                    Case "Normal"
                        Master.ppBtnInsert.Enabled = False
                        Master.ppBtnUpdate.Enabled = False
                        Master.ppBtnDelete.Enabled = False

                    Case "Upd"
                        Master.ppBtnInsert.Enabled = False
                        Master.ppBtnUpdate.Enabled = True
                        Master.ppBtnDelete.Enabled = False

                End Select
        End Select
    End Sub

    ''' <summary>
    ''' 通常料金
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_PriceN_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_PriceN.ServerValidate

        Dim tb As TextBox = CType(Master.FindControl("MainContent").FindControl(source.ControlToValidate), TextBox)
        Const Name As String = "通常料金"

        If txtPrice1_b.Text = String.Empty Or txtPrice1_b.Text = "0" Then
            If (tmbStartTm1.ppHourText <> String.Empty AndAlso tmbStartTm1.ppMinText <> String.Empty) Or (tmbEndTm1.ppHourText <> String.Empty AndAlso tmbEndTm1.ppMinText <> String.Empty) Then
                source.ErrorMessage = "" & Name & "に値が設定されていません。"
                source.Text = "未入力エラー"
                args.IsValid = False
                Exit Sub
            End If
        Else
            If Regex.IsMatch(tb.Text, "^[0-9]{1}$|^[1-9]{1}[0-9]{0,7}$") Then
            Else
                source.ErrorMessage = "" & Name & "は 8 桁以内の数値で入力して下さい。"
                source.Text = "形式エラー"
                args.IsValid = False
                Exit Sub
            End If
        End If

    End Sub

    ''' <summary>
    ''' 通常料金開始時刻
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks>セットNoもしくは台数が"0"か"00"なら
    ''' 　　　　 検証エラーとしてエラーサマリーを表示する。</remarks>
    Private Sub CstmVal_StartN_ServerValidate(source As Object, args As ServerValidateEventArgs)

        Const Name As String = "通常料金 開始時刻"

        If tmbStartTm1.ppHourText = String.Empty AndAlso tmbStartTm1.ppMinText = String.Empty Then
            If (txtPrice1_b.Text <> String.Empty AndAlso txtPrice1_b.Text <> "0") Or (tmbEndTm1.ppHourText <> String.Empty AndAlso tmbEndTm1.ppMinText <> String.Empty) Then
                source.ErrorMessage = "" & Name & "に値が設定されていません。"
                source.Text = "未入力エラー"
                args.IsValid = False
                Exit Sub
            End If
        Else
            If tmbEndTm2.ppHourText <> String.Empty AndAlso tmbEndTm2.ppMinText <> String.Empty Then
                If tmbStartTm1.ppHourText & tmbStartTm1.ppMinText <> tmbEndTm2.ppHourText & tmbEndTm2.ppMinText Then
                    source.ErrorMessage = "特別料金 終了時刻と同じ値を設定してください。"
                    source.Text = "整合性エラー"
                    args.IsValid = False
                    Exit Sub
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' 通常料金終了時刻
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks>セットNoもしくは台数が"0"か"00"なら
    ''' 　　　　 検証エラーとしてエラーサマリーを表示する。</remarks>
    Private Sub CstmVal_EndN_ServerValidate(source As Object, args As ServerValidateEventArgs)

        Const Name As String = "通常料金 終了時刻"

        If tmbEndTm1.ppHourText = String.Empty AndAlso tmbEndTm1.ppMinText = String.Empty Then
            If (txtPrice1_b.Text <> String.Empty AndAlso txtPrice1_b.Text <> "0") Or (tmbStartTm1.ppHourText <> String.Empty AndAlso tmbStartTm1.ppMinText <> String.Empty) Then
                source.ErrorMessage = "" & Name & "に値が設定されていません。"
                source.Text = "未入力エラー"
                args.IsValid = False
                Exit Sub
            End If
        Else
            If tmbStartTm2.ppHourText <> String.Empty AndAlso tmbStartTm2.ppMinText <> String.Empty Then
                If tmbEndTm1.ppHourText & tmbEndTm1.ppMinText <> tmbStartTm2.ppHourText & tmbStartTm2.ppMinText Then
                    source.ErrorMessage = "特別料金 開始時刻と同じ値を設定してください。"
                    source.Text = "整合性エラー"
                    args.IsValid = False
                    Exit Sub
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' 特別料金
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_PriceS_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_PriceS.ServerValidate

        Dim tb As TextBox = CType(Master.FindControl("MainContent").FindControl(source.ControlToValidate), TextBox)
        Const Name As String = "特別料金"

        If txtPrice2_b.Text = String.Empty Or txtPrice2_b.Text = "0" Then
            If (tmbStartTm2.ppHourText <> String.Empty AndAlso tmbStartTm2.ppMinText <> String.Empty) Or (tmbEndTm2.ppHourText <> String.Empty AndAlso tmbEndTm2.ppMinText <> String.Empty) Then
                source.ErrorMessage = "" & Name & "に値が設定されていません。"
                source.Text = "未入力エラー"
                args.IsValid = False
                Exit Sub
            End If
        Else
            If Regex.IsMatch(tb.Text, "^[0-9]{1}$|^[1-9]{1}[0-9]{0,7}$") Then
            Else
                source.ErrorMessage = "" & Name & "は 8 桁以内の数値で入力して下さい。"
                source.Text = "形式エラー"
                args.IsValid = False
                Exit Sub
            End If
        End If

    End Sub

    ''' <summary>
    ''' 特別料金開始時刻
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks>セットNoもしくは台数が"0"か"00"なら
    ''' 　　　　 検証エラーとしてエラーサマリーを表示する。</remarks>
    Private Sub CstmVal_StartS_ServerValidate(source As Object, args As ServerValidateEventArgs)

        Const Name As String = "特別料金 開始時刻"

        If tmbStartTm2.ppHourText = String.Empty AndAlso tmbStartTm2.ppMinText = String.Empty Then
            If (txtPrice2_b.Text <> String.Empty AndAlso txtPrice2_b.Text <> "0") Or (tmbEndTm2.ppHourText <> String.Empty AndAlso tmbEndTm2.ppMinText <> String.Empty) Then
                source.ErrorMessage = "" & Name & "に値が設定されていません。"
                source.Text = "未入力エラー"
                args.IsValid = False
                Exit Sub
            End If
        Else
            If tmbEndTm1.ppHourText <> String.Empty AndAlso tmbEndTm1.ppMinText <> String.Empty Then
                If tmbStartTm2.ppHourText & tmbStartTm2.ppMinText <> tmbEndTm1.ppHourText & tmbEndTm1.ppMinText Then
                    source.ErrorMessage = "通常料金 終了時刻と同じ値を設定してください。"
                    source.Text = "整合性エラー"
                    args.IsValid = False
                    Exit Sub
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' 特別料金終了時刻
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks>セットNoもしくは台数が"0"か"00"なら
    ''' 　　　　 検証エラーとしてエラーサマリーを表示する。</remarks>
    Private Sub CstmVal_EndS_ServerValidate(source As Object, args As ServerValidateEventArgs)

        Const Name As String = "特別料金 終了時刻"

        If tmbEndTm2.ppHourText = String.Empty AndAlso tmbEndTm2.ppMinText = String.Empty Then
            If (txtPrice2_b.Text <> String.Empty AndAlso txtPrice2_b.Text <> "0") Or (tmbStartTm2.ppHourText <> String.Empty AndAlso tmbStartTm2.ppMinText <> String.Empty) Then
                source.ErrorMessage = "" & Name & "に値が設定されていません。"
                source.Text = "未入力エラー"
                args.IsValid = False
                Exit Sub
            End If
        Else
            If tmbStartTm1.ppHourText <> String.Empty AndAlso tmbStartTm1.ppMinText <> String.Empty Then
                If tmbEndTm2.ppHourText & tmbEndTm2.ppMinText <> tmbStartTm1.ppHourText & tmbStartTm1.ppMinText Then
                    source.ErrorMessage = "通常料金 開始時刻と同じ値を設定してください。"
                    source.Text = "整合性エラー"
                    args.IsValid = False
                    Exit Sub
                End If
            End If
        End If
    End Sub

#End Region
End Class
