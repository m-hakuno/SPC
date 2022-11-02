'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　持参物品セットマスタ
'*　ＰＧＭＩＤ：　COMUPDM95
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.11.27　：　栗原
'********************************************************************************************************************************

#Region "インポート定義"

Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive

#End Region

Public Class COMUPDM95

#Region "継承定義"

    Inherits System.Web.UI.Page

#End Region

#Region "定数定義"

    Const DispCode As String = "COMUPDM95"
    Const MasterName As String = "持参物品セットマスタ"
    Const TableName As String = "M95_BRINGSET"

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
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        Dim intHeadCol As Integer() = New Integer() {1, 3, 5} 'グリッドXML定義ファイル取得　ヘッダ修正パラメータ
        Dim intColSpan As Integer() = New Integer() {2, 2, 2}
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
        AddHandler Master.ppBtnDelete.Command, AddressOf btn_Click

        'コードイベント設定
        AddHandler ddlSys.ppDropDownList.SelectedIndexChanged, AddressOf ddlSys_SelectedIndexChanged
        AddHandler ddlDvs.ppDropDownList.SelectedIndexChanged, AddressOf ddlDvs_SelectedIndexChanged
        AddHandler ddlCls.ppDropDownList.SelectedIndexChanged, AddressOf ddlCls_SelectedIndexChanged

        ddlSys.ppDropDownList.AutoPostBack = True
        ddlDvs.ppDropDownList.AutoPostBack = True
        ddlCls.ppDropDownList.AutoPostBack = True

        '入力検証用
        Dim cuv_HDDCls As CustomValidator
        Dim cuv_SetNo As CustomValidator
        Dim cuv_MaxCnt As CustomValidator
        cuv_HDDCls = ddlHDDCls.FindControl("pnlErr").FindControl("cuvDropDownList")
        AddHandler cuv_HDDCls.ServerValidate, AddressOf cuv_HDDCls_validate
        cuv_SetNo = txtSetNo.FindControl("pnlErr").FindControl("cuvTextBox")
        AddHandler cuv_SetNo.ServerValidate, AddressOf cuv_ZeroCheck
        cuv_MaxCnt = txtMaxCnt.FindControl("pnlErr").FindControl("cuvTextBox")
        AddHandler cuv_MaxCnt.ServerValidate, AddressOf cuv_ZeroCheck

        'UpdatePanel(グリッド用)のポストバックの為に、
        'スクリプトマネージャーに登録する。
        Dim scm As ScriptManager
        scm = Master.FindControl("tsmManager")
        scm.RegisterPostBackControl(grvList)
        scm.RegisterPostBackControl(ddlCls)

        If Not IsPostBack Then

            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

            msGet_Data()
            msSetddlSystem()
            ddlDvs.ppDropDownList.Items.Clear()
            ddlCls.ppDropDownList.Items.Clear()
            ddlHDDNo.ppDropDownList.Items.Clear()
            ddlHDDCls.ppDropDownList.Items.Clear()

            strMode = "Default"
            SetFocus(ddlSys.ppDropDownList.ClientID)

        End If
    End Sub

    ''' <summary>
    ''' Page_PreRender
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        msEnableCtrl_btn(strMode)
        msEnableCtrl_obj(strMode)

        '非同期ポストバック時、グリッドが初期化される事がある為
        If grvList.Rows.Count = 0 Then
            msGet_Data()
        End If
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
        ddlSys.ppSelectedValue = String.Empty
        ddlDvs.ppSelectedValue = String.Empty
        ddlCls.ppSelectedValue = String.Empty
        ddlHDDNo.ppSelectedValue = String.Empty
        ddlHDDCls.ppSelectedValue = String.Empty
        ddlInsUpd.SelectedValue = "0"   '新規モードに戻す
        txtSetNo.ppText = String.Empty
        txtMaxCnt.ppText = String.Empty

        ddlDvs.ppDropDownList.Items.Clear()
        ddlCls.ppDropDownList.Items.Clear()

        strMode = "Default"
        SetFocus(ddlSys.ppDropDownList.ClientID)

        If hdnDtl_SelectFLG.Value = "1" Then
            msGet_Data()
            'Response.Redirect(Request.Url.OriginalString)
            updPanelGrv.Update()
            hdnDtl_SelectFLG.Value = "0"
        End If

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
            If mfIsCheckData(sender) Then
                msEdit_Data(e.CommandName)
                btnClear_Click()
                strMode = "Default"
            End If
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' システムコード変更時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlSys_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

        'ログ出力開始
        psLogStart(Me)
        If ddlSys.ppSelectedValue <> String.Empty AndAlso ddlDvs.ppSelectedValue <> String.Empty AndAlso ddlCls.ppSelectedValue <> String.Empty Then
            Page.Validate("key")
            If (Page.IsValid) Then
                '既存データ取得処理
                msGet_ExistData()
                SetFocus(ddlCls.ppDropDownList.ClientID)
            End If
        ElseIf ddlSys.ppSelectedValue <> String.Empty Then
            msSetddlDvs()
            SetFocus(ddlCls.ppDropDownList.ClientID)
        Else
            ddlDvs.ppDropDownList.Items.Clear()
            ddlCls.ppDropDownList.Items.Clear()
            SetFocus(ddlSys.ppDropDownList.ClientID)
        End If

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 分類変更時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlDvs_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        If ddlSys.ppSelectedValue <> String.Empty AndAlso ddlDvs.ppSelectedValue <> String.Empty AndAlso ddlCls.ppSelectedValue <> String.Empty Then
            Page.Validate("key")
            If (Page.IsValid) Then
                msGet_ExistData()
            End If
        ElseIf ddlDvs.ppSelectedValue <> String.Empty Then
            msSetddlCls()
            SetFocus(ddlCls.ppDropDownList.ClientID)
        Else
            Me.ddlCls.ppDropDownList.Items.Clear()
            SetFocus(ddlDvs.ppDropDownList.ClientID)
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 種別変更時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlCls_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        If ddlSys.ppSelectedValue <> String.Empty AndAlso ddlDvs.ppSelectedValue <> String.Empty AndAlso ddlCls.ppSelectedValue <> String.Empty Then
            Page.Validate("key")
            If (Page.IsValid) Then
                '既存データ取得処理
                msGet_ExistData()
                SetFocus(ddlInsUpd.ClientID)
            End If
        ElseIf ddlDvs.ppSelectedValue <> String.Empty Then
            SetFocus(ddlSys.ppDropDownList.ClientID)
            strMode = "Default"
        Else
            SetFocus(ddlDvs.ppDropDownList.ClientID)
            strMode = "Default"
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
                arKey.Insert(0, CType(rowData.FindControl("機器分類コード"), TextBox).Text)
                arKey.Insert(0, CType(rowData.FindControl("機器種別コード"), TextBox).Text)
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
                ddlSys.ppSelectedValue = CType(rowData.FindControl("システムコード"), TextBox).Text
                msSetddlDvs()
                ddlDvs.ppSelectedValue = CType(rowData.FindControl("機器分類コード"), TextBox).Text
                ddlCls.ppSelectedValue = CType(rowData.FindControl("機器種別コード"), TextBox).Text
                txtSetNo.ppText = CType(rowData.FindControl("セットNo"), TextBox).Text
                txtMaxCnt.ppText = CType(rowData.FindControl("台数"), TextBox).Text
                ddlInsUpd.SelectedValue = "1"
                If CType(rowData.FindControl("HDDNo"), TextBox).Text.Trim <> String.Empty OrElse CType(rowData.FindControl("HDD種別"), TextBox).Text.Trim <> String.Empty Then
                    msSetddlHDD()
                    ddlHDDNo.ppSelectedValue = CType(rowData.FindControl("HDDNo"), TextBox).Text
                    ddlHDDCls.ppSelectedValue = CType(rowData.FindControl("HDD種別"), TextBox).Text
                Else
                    ddlHDDNo.ppSelectedValue = String.Empty
                    ddlHDDCls.ppSelectedValue = String.Empty
                End If
                strMode = "Edit"
                If CType(rowData.FindControl("削除"), TextBox).Text = "0" Then
                    Master.ppBtnDelete.Text = "削除"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)
                Else
                    Master.ppBtnDelete.Text = "削除取消"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")
                End If
            End If
            SetFocus(txtSetNo.ppTextBox.ClientID)
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
        '削除済みデータを赤文字表示に
        For zz = 0 To grvList.Rows.Count - 1
            If CType(grvList.Rows(zz).FindControl("削除"), TextBox).Text = "1" Then
                CType(grvList.Rows(zz).FindControl("システムコード"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(zz).FindControl("システム"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(zz).FindControl("機器分類コード"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(zz).FindControl("機器分類"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(zz).FindControl("機器種別コード"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(zz).FindControl("機器種別"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(zz).FindControl("HDDNo"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(zz).FindControl("HDD種別"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(zz).FindControl("セットNo"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(zz).FindControl("台数"), TextBox).ForeColor = Drawing.Color.Red
            End If
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

                With cmdDB.Parameters
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
    ''' 条件付き検索
    ''' </summary>
    ''' <returns>True:１件以上の検索結果　False：それ以外</returns>
    ''' <remarks></remarks>
    Private Function mfGet_Data_Dtl() As Boolean
        Dim cnDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dtsDB As DataSet = Nothing
        Dim dispflg As String = String.Empty
        objStack = New StackFrame
        'DB接続
        If clsDataConnect.pfOpen_Database(cnDB) Then
            Try
                cmdDB = New SqlCommand(DispCode & "_S2", cnDB)
                With cmdDB.Parameters
                    With cmdDB.Parameters
                        .Add(pfSet_Param("Sys_Cod", SqlDbType.NVarChar, ddlSys.ppSelectedValue))
                        .Add(pfSet_Param("Dvs_Cod", SqlDbType.NVarChar, ddlDvs.ppSelectedValue))
                        .Add(pfSet_Param("Cls_Cod", SqlDbType.NVarChar, ddlCls.ppSelectedValue))
                    End With
                End With
                dtsDB = clsDataConnect.pfGet_DataSet(cmdDB)
                If dtsDB.Tables(0).Rows.Count = 0 Then
                    'エラー表示
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                Else
                    Master.ppCount = dtsDB.Tables(0).Rows.Count
                    grvList.DataSource = dtsDB.Tables(0)
                    grvList.DataBind()
                End If

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
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
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            grvList.DataSource = New DataTable
            grvList.DataBind()
            Master.ppCount = "0"
        End If

        If dtsDB.Tables(0).Rows.Count <> 0 Then
            mfGet_Data_Dtl = True
        Else
            mfGet_Data_Dtl = False
        End If

    End Function

    ''' <summary>
    ''' 検索結果を入力欄にセット
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_Data()
        Dim cnDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dtsDB As DataSet = Nothing
        Dim dispflg As String = String.Empty
        objStack = New StackFrame
        'DB接続
        If clsDataConnect.pfOpen_Database(cnDB) Then
            Try
                cmdDB = New SqlCommand(DispCode & "_S2", cnDB)
                With cmdDB.Parameters
                    With cmdDB.Parameters
                        .Add(pfSet_Param("Sys_Cod", SqlDbType.NVarChar, ddlSys.ppSelectedValue))
                        .Add(pfSet_Param("Dvs_Cod", SqlDbType.NVarChar, ddlDvs.ppSelectedValue))
                        .Add(pfSet_Param("Cls_Cod", SqlDbType.NVarChar, ddlCls.ppSelectedValue))
                        .Add(pfSet_Param("Seq", SqlDbType.NVarChar, txtSetNo.ppText.Trim))
                    End With
                End With
                dtsDB = clsDataConnect.pfGet_DataSet(cmdDB)
                If dtsDB.Tables(0).Rows.Count = 0 Then
                    '既存データ無し
                    txtMaxCnt.ppText = "2"
                    If ddlCls.ppSelectedValue = "09" Then
                        msSetddlHDD()
                    Else
                        ddlHDDNo.ppDropDownList.Items.Clear()
                        ddlHDDCls.ppDropDownList.Items.Clear()
                    End If
                Else
                    txtMaxCnt.ppText = dtsDB.Tables(0).Rows(0).Item("台数").ToString
                    If ddlCls.ppSelectedValue = "09" Then
                        msSetddlHDD()
                        ddlHDDNo.ppSelectedValue = dtsDB.Tables(0).Rows(0).Item("HDDNo").ToString
                        ddlHDDCls.ppSelectedValue = dtsDB.Tables(0).Rows(0).Item("HDD種別").ToString
                    Else
                        ddlHDDNo.ppDropDownList.Items.Clear()
                        ddlHDDCls.ppDropDownList.Items.Clear()
                    End If
                End If
                If dtsDB.Tables(0).Rows(0).Item("削除").ToString = "0" Then
                    Master.ppBtnDelete.Text = "削除"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)
                Else
                    Master.ppBtnDelete.Text = "削除取消"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")
                End If
                strMode = "Edit"
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
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
                        With cmdDB.Parameters
                            .Add(pfSet_Param("Sys_Cod", SqlDbType.NVarChar, ddlSys.ppSelectedValue))
                            .Add(pfSet_Param("Dvs_Cod", SqlDbType.NVarChar, ddlDvs.ppSelectedValue))
                            .Add(pfSet_Param("Cls_Cod", SqlDbType.NVarChar, ddlCls.ppSelectedValue))
                            .Add(pfSet_Param("HDD_No", SqlDbType.NVarChar, ddlHDDNo.ppSelectedValue))
                            .Add(pfSet_Param("HDD_Cls", SqlDbType.NVarChar, ddlHDDCls.ppSelectedValue))
                            .Add(pfSet_Param("Seq", SqlDbType.NVarChar, txtSetNo.ppText.Trim))
                            .Add(pfSet_Param("Max_Cnt", SqlDbType.NVarChar, txtMaxCnt.ppText.Trim))
                            .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))
                            .Add(pfSet_Param("user_id", SqlDbType.NVarChar, User.Identity.Name))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With
                    Case "DELETE"               '削除
                        With cmdDB.Parameters
                            .Add(pfSet_Param("Sys_Cod", SqlDbType.NVarChar, ddlSys.ppSelectedValue))
                            .Add(pfSet_Param("Dvs_Cod", SqlDbType.NVarChar, ddlDvs.ppSelectedValue))
                            .Add(pfSet_Param("Cls_Cod", SqlDbType.NVarChar, ddlCls.ppSelectedValue))
                            .Add(pfSet_Param("seq", SqlDbType.NVarChar, txtSetNo.ppText.Trim))
                            .Add(pfSet_Param("user_id", SqlDbType.NVarChar, User.Identity.Name))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With
                End Select

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
                    .Add(pfSet_Param("Sys_Cod", SqlDbType.NVarChar, ddlSys.ppSelectedValue))
                    .Add(pfSet_Param("Dvs_Cod", SqlDbType.NVarChar, ddlDvs.ppSelectedValue))
                    .Add(pfSet_Param("Cls_Cod", SqlDbType.NVarChar, ddlCls.ppSelectedValue))
                End With
                dtsDB = clsDataConnect.pfGet_DataSet(cmdDB)

                If ddlInsUpd.SelectedValue = "0" Then
                    txtSetNo.ppText = mfGet_SetNo()
                    If txtSetNo.ppText = "" Then
                        psMesBox(Me, "30023", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "登録件数が最大値に達しています。")
                        ddlCls.ppSelectedValue = String.Empty
                        Exit Sub
                    End If
                    txtMaxCnt.ppText = "2"
                    strMode = "Edit"
                    If ddlCls.ppSelectedValue = "09" Then
                        msSetddlHDD()
                    Else
                        ddlHDDNo.ppDropDownList.Items.Clear()
                        ddlHDDCls.ppDropDownList.Items.Clear()
                    End If
                Else
                    If dtsDB.Tables(0).Rows.Count = 0 Then
                        psMesBox(Me, "30023", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "更新データが存在しません。")
                        ddlCls.ppSelectedValue = String.Empty
                        SetFocus(ddlCls.ppDropDownList.ClientID)
                        Exit Sub
                    End If
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
                        arKey.Insert(0, ddlSys.ppSelectedValue)
                        arKey.Insert(0, ddlDvs.ppSelectedValue)
                        arKey.Insert(0, ddlCls.ppSelectedValue)
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
                        txtSetNo.ppText = dtsDB.Tables(0).Rows(0).Item("セットNo").ToString
                        txtMaxCnt.ppText = dtsDB.Tables(0).Rows(0).Item("台数").ToString
                        If ddlCls.ppSelectedValue = "09" Then
                            msSetddlHDD()
                            ddlHDDNo.ppSelectedValue = dtsDB.Tables(0).Rows(0).Item("HDDNo").ToString
                            ddlHDDCls.ppSelectedValue = dtsDB.Tables(0).Rows(0).Item("HDD種別").ToString
                        Else
                            ddlHDDNo.ppDropDownList.Items.Clear()
                            ddlHDDCls.ppDropDownList.Items.Clear()
                        End If
                        strMode = "Edit"

                        If dtsDB.Tables(0).Rows(0).Item("削除").ToString = "0" Then
                            Master.ppBtnDelete.Text = "削除"
                            Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)
                        Else
                            Master.ppBtnDelete.Text = "削除取消"
                            Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")
                        End If
                    ElseIf dtsDB.Tables(0).Rows.Count > 1 Then
                        psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "該当データが複数存在します。\n編集するデータを、一覧から選択してください。")
                        mfGet_Data_Dtl()
                        strMode = "UnCtrl"
                        hdnDtl_SelectFLG.Value = "1"
                    Else
                        strMode = "Edit"
                        txtSetNo.ppText = "1"
                        txtMaxCnt.ppText = "2"
                        If ddlCls.ppSelectedValue = "09" Then
                            msSetddlHDD()
                        End If
                    End If

                End If

                SetFocus(txtSetNo.ppTextBox.ClientID)

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
        'フォーカス設定
        SetFocus(ddlSys.ppDropDownList.ClientID)
    End Sub

    ''' <summary>
    ''' ドロップダウンリストシステム取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub msSetddlSystem()
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
                Me.ddlSys.ppDropDownList.Items.Clear()
                Me.ddlSys.ppDropDownList.DataSource = objDs.Tables(0)
                Me.ddlSys.ppDropDownList.DataTextField = "ＴＢＯＸリスト"
                Me.ddlSys.ppDropDownList.DataValueField = "ＴＢＯＸシステムコード"
                Me.ddlSys.ppDropDownList.DataBind()
                Me.ddlSys.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
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

    ''' <summary>
    ''' ドロップダウンリスト機器分類取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlDvs()
        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDt As DataTable = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL012", objCn)

                '01：TBOXのみ表示する
                Dim objDv = clsDataConnect.pfGet_DataSet(objCmd).Tables(0).DefaultView
                objDv.RowFilter = "機器分類コード = 01"
                objDt = objDv.ToTable

                'ドロップダウンリスト設定
                '検索結果
                Me.ddlDvs.ppDropDownList.Items.Clear()
                Me.ddlDvs.ppDropDownList.DataSource = objDt
                Me.ddlDvs.ppDropDownList.DataTextField = "名称"
                Me.ddlDvs.ppDropDownList.DataValueField = "機器分類コード"
                Me.ddlDvs.ppDropDownList.DataBind()
                Me.ddlDvs.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                Me.ddlDvs.ppDropDownList.SelectedValue = "01"
                msSetddlCls()

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器分類マスタ一覧取得")
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
    ''' ドロップダウンリスト機器種別取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlCls()
        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL013", objCn)
                With objCmd.Parameters
                    .Add(pfSet_Param("appadivcd", SqlDbType.NVarChar, "01"))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                '検索結果
                Me.ddlCls.ppDropDownList.Items.Clear()
                Me.ddlCls.ppDropDownList.DataSource = objDs.Tables(0)
                Me.ddlCls.ppDropDownList.DataTextField = "機器種別名"
                Me.ddlCls.ppDropDownList.DataValueField = "機器種別コード"
                Me.ddlCls.ppDropDownList.DataBind()
                Me.ddlCls.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器種別マスタ一覧取得")
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
    ''' ドロップダウンリストＨＤＤ取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlHDD()

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(DispCode & "_S3", objCn)
                With objCmd.Parameters
                    .Add(pfSet_Param("Sys_Cod", SqlDbType.NVarChar, ddlSys.ppSelectedValue))
                End With
                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                '■HDDNo
                Me.ddlHDDNo.ppDropDownList.Items.Clear()
                Me.ddlHDDNo.ppDropDownList.DataSource = objDs.Tables(0)
                Me.ddlHDDNo.ppDropDownList.DataTextField = "HDDNo"
                Me.ddlHDDNo.ppDropDownList.DataValueField = "HDDNo"
                Me.ddlHDDNo.ppDropDownList.DataBind()
                Me.ddlHDDNo.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                '■HDD種別
                Me.ddlHDDCls.ppDropDownList.Items.Clear()
                Me.ddlHDDCls.ppDropDownList.DataSource = objDs.Tables(1)
                Me.ddlHDDCls.ppDropDownList.DataTextField = "HDD種別"
                Me.ddlHDDCls.ppDropDownList.DataValueField = "HDD種別"
                Me.ddlHDDCls.ppDropDownList.DataBind()
                If Me.ddlHDDCls.ppDropDownList.Items.FindByValue(String.Empty) Is Nothing Then
                    Me.ddlHDDCls.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing))
                End If
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＨＤＤマスタ一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
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
                ddlInsUpd.Enabled = True
                ddlSys.ppEnabled = True
                ddlDvs.ppEnabled = True
                ddlCls.ppEnabled = True
                ddlHDDNo.ppEnabled = False
                ddlHDDCls.ppEnabled = False
                txtMaxCnt.ppEnabled = False
                txtSetNo.ppEnabled = False

            Case "Choice"
                ddlInsUpd.Enabled = True
                ddlSys.ppEnabled = True
                ddlDvs.ppEnabled = True
                ddlCls.ppEnabled = True
                ddlHDDNo.ppEnabled = False
                ddlHDDCls.ppEnabled = False
                txtMaxCnt.ppEnabled = False
                txtSetNo.ppEnabled = False

            Case "UnCtrl"
                ddlSys.ppEnabled = False
                ddlDvs.ppEnabled = False
                ddlCls.ppEnabled = False
                ddlInsUpd.Enabled = False
                ddlHDDNo.ppEnabled = False
                ddlHDDCls.ppEnabled = False
                txtMaxCnt.ppEnabled = False
                txtSetNo.ppEnabled = False

            Case "Edit"
                ddlSys.ppEnabled = False
                ddlDvs.ppEnabled = False
                ddlInsUpd.Enabled = False
                ddlCls.ppEnabled = False
                If Master.ppBtnDelete.Text = "削除取消" Then
                    txtMaxCnt.ppEnabled = False
                    txtSetNo.ppEnabled = False
                    ddlHDDNo.ppEnabled = False
                    ddlHDDCls.ppEnabled = False
                Else
                    txtMaxCnt.ppEnabled = True
                    txtSetNo.ppEnabled = True
                    If ddlCls.ppSelectedValue = "09" Then
                        ddlHDDNo.ppEnabled = True
                        ddlHDDCls.ppEnabled = True
                    Else
                        ddlHDDNo.ppEnabled = False
                        ddlHDDCls.ppEnabled = False
                    End If
                End If
        End Select
    End Sub

    ''' <summary>
    ''' ボタン活性制御
    ''' </summary>
    ''' <param name="strMode"></param>
    ''' <remarks></remarks>
    Private Sub msEnableCtrl_btn(ByVal strMode As String)
        Master.ppBtnClear.Enabled = True
        Select Case strMode

            Case "Default", "Choice", "UnCtrl"
                Master.ppBtnInsert.Enabled = False
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnDelete.Enabled = False
                Master.ppBtnDelete.Text = "削除"
            Case "Edit"
                '削除データを選択した場合
                If Master.ppBtnDelete.Text = "削除取消" Then
                    Master.ppBtnInsert.Enabled = False
                    Master.ppBtnUpdate.Enabled = False
                    If ddlInsUpd.SelectedValue = "0" Then
                        Master.ppBtnDelete.Enabled = False
                    Else
                        Master.ppBtnDelete.Enabled = True
                    End If
                Else
                    If ddlInsUpd.SelectedValue = "0" Then
                        Master.ppBtnInsert.Enabled = True
                        Master.ppBtnUpdate.Enabled = False
                        Master.ppBtnDelete.Enabled = False
                    Else
                        Master.ppBtnInsert.Enabled = False
                        Master.ppBtnUpdate.Enabled = True
                        Master.ppBtnDelete.Enabled = True
                    End If
                End If
        End Select
    End Sub

    ''' <summary>
    ''' 整合性チェック
    ''' </summary>
    ''' <param name="sender">ボタン種別　(【削除】と【登録/更新】で処理を分ける為）</param>
    ''' <returns>True:検証OK　False:検証NG</returns>
    ''' <remarks>データの存在と削除フラグ、
    ''' 選択中のモードと押下ボタンの整合性を検証する</remarks>
    Private Function mfIsCheckData(ByVal sender As Object) As Boolean

        'パターン別エラーメッセージ
        '① 削除処理時、対象データが既に削除済み
        '② 削除処理時、データが存在しない
        '③ 登録時、既にデータが存在している
        '④ 更新時、データが存在しない
        '⑤ 登録時、既にデータが存在し、削除されている
        '⑥ 更新時、対象データが削除済み
        Dim strExistDt As String
        strExistDt = mfIsDataExist()
        If sender.Equals(Master.ppBtnDelete) Then
            '削除取消を許容する為、sender.Text = "削除" とする
            If strExistDt = "Delete" AndAlso sender.Text = "削除" Then
                '①
                psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "既に削除されたデータです。\n※削除取消は一覧の選択ボタン押下")
                mfIsCheckData = False
            ElseIf strExistDt = "False" AndAlso sender.Text = "削除" Then
                '②
                psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "削除可能なデータが存在しません。\nセットNoを確認してください。")
                mfIsCheckData = False
            Else
                mfIsCheckData = True
            End If
        Else
            If strExistDt = "True" AndAlso ddlInsUpd.SelectedValue = "0" Then
                '③
                psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "既にデータが存在しています。\nセットNoを確認してください。\n※データの変更は、更新モードで編集")
                mfIsCheckData = False
            ElseIf strExistDt = "False" AndAlso ddlInsUpd.SelectedValue = "1" Then
                '④
                psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "編集可能なデータが存在しません。\nセットNoを確認してください。\n※新規登録は新規モードで登録")
                mfIsCheckData = False
            ElseIf strExistDt = "Delete" AndAlso ddlInsUpd.SelectedValue = "0" Then
                '⑤
                psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "削除されたデータが存在している為、登録出来ません。\nセットNoを確認してください。\n※削除取消は一覧の選択ボタン押下")
                mfIsCheckData = False
            ElseIf strExistDt = "Delete" Then
                '⑥
                psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "削除されたデータの為、更新出来ません。\nセットNoを確認してください。\n※削除取消は一覧の選択ボタン押下")
                mfIsCheckData = False
            Else
                mfIsCheckData = True
            End If
        End If
    End Function

    ''' <summary>
    ''' データ存在チェック
    ''' </summary>
    ''' <returns>"True":データ有　"False":データ無　"Delete":削除済みデータ有</returns>
    ''' <remarks>データの存在と削除フラグをチェック　戻り値はString型</remarks>
    Private Function mfIsDataExist() As String
        Dim cnDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dtsDB As DataSet = Nothing
        Dim dispflg As String = String.Empty
        objStack = New StackFrame
        'DB接続
        If clsDataConnect.pfOpen_Database(cnDB) Then
            Try
                cmdDB = New SqlCommand(DispCode & "_S5", cnDB)
                With cmdDB.Parameters
                    With cmdDB.Parameters
                        .Add(pfSet_Param("Sys_Cod", SqlDbType.NVarChar, ddlSys.ppSelectedValue))
                        .Add(pfSet_Param("Dvs_Cod", SqlDbType.NVarChar, ddlDvs.ppSelectedValue))
                        .Add(pfSet_Param("Cls_Cod", SqlDbType.NVarChar, ddlCls.ppSelectedValue))
                        .Add(pfSet_Param("Seq", SqlDbType.NVarChar, txtSetNo.ppText.Trim))
                    End With
                End With
                dtsDB = clsDataConnect.pfGet_DataSet(cmdDB)

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
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
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            grvList.DataSource = New DataTable
            grvList.DataBind()
            Master.ppCount = "0"
        End If
        If dtsDB.Tables(1).Rows.Count > 0 Then
            mfIsDataExist = "Delete"
        ElseIf dtsDB.Tables(0).Rows.Count > 0 Then
            mfIsDataExist = "True"
        Else
            mfIsDataExist = "False"
        End If
    End Function

    ''' <summary>
    ''' 連番採番処理
    ''' </summary>
    ''' <returns>登録データの無い、新規の連番を返す※String型</returns>
    ''' <remarks>登録済み連番の最大値＋１を返す。
    '''          ９９が登録済みなら歯抜けの最小値
    '''          登録データ無しなら１
    ''' 　　　　 空き連番無しは空文字を返す。(注)接続エラー時も空文字を返す
    ''' </remarks>
    Private Function mfGet_SetNo() As String
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim dsData As DataSet
        Dim intRtn As Integer
        mfGet_SetNo = ""

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            '番号を採番する
            Try

                objCmd = New SqlCommand(DispCode & "_S6", objCn)
                With objCmd.Parameters
                    .Add(pfSet_Param("Sys_Cod", SqlDbType.NVarChar, ddlSys.ppSelectedValue))
                    .Add(pfSet_Param("Dvs_Cod", SqlDbType.NVarChar, ddlDvs.ppSelectedValue))
                    .Add(pfSet_Param("Cls_Cod", SqlDbType.NVarChar, ddlCls.ppSelectedValue))
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                End With
                'データ取得
                dsData = clsDataConnect.pfGet_DataSet(objCmd)
                'ストアド戻り値チェック
                intRtn = Integer.Parse(objCmd.Parameters("retvalue").Value.ToString)

                If intRtn <> 0 Then
                    mfGet_SetNo = ""
                    Exit Function
                End If

                If dsData.Tables(0).Rows.Count > 0 Then
                    With dsData.Tables(0).Rows(0)
                        mfGet_SetNo = .Item("NUM").ToString()
                    End With
                End If
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle)
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Function

    ''' <summary>
    ''' HDDマスタ存在チェック
    ''' </summary>
    ''' <returns>該当HDD有：True　該当HDD無：False</returns>
    ''' <remarks></remarks>
    Private Function mfIsHDDExist() As Boolean
        Dim cnDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dtsDB As DataSet = Nothing
        objStack = New StackFrame

        mfIsHDDExist = False
        'DB接続
        If clsDataConnect.pfOpen_Database(cnDB) Then
            Try
                cmdDB = New SqlCommand(DispCode & "_S4", cnDB)
                With cmdDB.Parameters
                    .Add(pfSet_Param("Sys_Cod", SqlDbType.NVarChar, ddlSys.ppSelectedValue))
                    .Add(pfSet_Param("HDDNo", SqlDbType.NVarChar, ddlHDDNo.ppSelectedValue))
                    .Add(pfSet_Param("HDDCls", SqlDbType.NVarChar, ddlHDDCls.ppSelectedValue))
                End With
                dtsDB = clsDataConnect.pfGet_DataSet(cmdDB)
                '件数チェック（１行以上でTrueを返す）
                If CType(dtsDB.Tables(0).Rows(0).Item("件数").ToString, Integer) > 0 Then
                    mfIsHDDExist = True
                Else
                    mfIsHDDExist = False
                End If

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "HDDマスタ")
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
    End Function

    ''' <summary>
    ''' HDD_Validationイベント
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuv_HDDCls_validate(source As Object, args As ServerValidateEventArgs)
        Dim cuv_HDDNo As CustomValidator
        cuv_HDDNo = ddlHDDNo.FindControl("pnlErr").FindControl("cuvDropDownList")
        If ddlCls.ppSelectedValue = "09" Then
            If Not mfIsHDDExist() Then
                'エラーテキストは両方出す
                source.text = "整合性エラー"
                cuv_HDDNo.Text = "整合性エラー"
                cuv_HDDNo.ErrorMessage = String.Empty   'エラーサマリーが二重に表示されるのを防ぐ
                source.ErrorMessage = "選択したＨＤＤがＨＤＤマスタに存在しません。再度確認し、入力してください。"
                cuv_HDDNo.IsValid = False
                args.IsValid = False
                Exit Sub
            End If
        End If
    End Sub

    ''' <summary>
    ''' ゼロチェック
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks>セットNoもしくは台数が"0"か"00"なら
    ''' 　　　　 検証エラーとしてエラーサマリーを表示する。</remarks>
    Private Sub cuv_ZeroCheck(source As Object, args As ServerValidateEventArgs)
        Dim TmpText As String '検証対象のテキスト
        Dim TmpName As String 'エラーメッセージ用
        Select Case source.ClientID
            Case txtSetNo.FindControl("pnlErr").FindControl("cuvTextBox").ClientID
                TmpText = txtSetNo.ppText.Trim
                TmpName = txtSetNo.ppName
            Case txtMaxCnt.FindControl("pnlErr").FindControl("cuvTextBox").ClientID
                TmpText = txtMaxCnt.ppText.Trim
                TmpName = txtMaxCnt.ppName
            Case Else
                Exit Sub
        End Select

        Select Case TmpText
            Case "0", "00"
                source.text = "形式エラー"
                source.ErrorMessage = TmpName & " は１以上の半角数値で入力してください。"
                args.IsValid = False
                Exit Sub
        End Select
    End Sub

#End Region
End Class
