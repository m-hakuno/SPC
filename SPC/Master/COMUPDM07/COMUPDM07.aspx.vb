'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　機器マスタ管理
'*　ＰＧＭＩＤ：　COMUPDM07
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.01.20　：　加賀
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'COMUPDM07-001      2016/07/07      栗原      入力項目[印刷区分]を追加(分類03:店内装置、種別21:サンドの時のみ表示)
'COMUPDM07-002      2016/10/19      稲葉      エラーチェック追加
'                                             (機器バージョンにシステムのバージョンが入力された場合は、バージョンも同様のバージョンを選択する)

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports SPC.ClsCMDataConnect
Imports SQL_DBCLS_LIB
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon

#End Region

Public Class COMUPDM07

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
    Const DispCode As String = "COMUPDM07"             '画面ID
    'マスタ情報
    Const MasterName As String = "工事機器マスタ"           'マスタ名
    Const TableName As String = "M07_APPA"              'テーブル名
    'キー情報
    Const KeyName1 As String = "M07_APPA_CD"
    Const KeyName2 As String = ""
    Const KeyName3 As String = ""
    Const KeyName4 As String = ""
    Const KeyName5 As String = ""
    'Const intDelCls As Integer = 1         '削除種別 0:削除なし  1:削除フラグ  2:DELETE
    'ソート情報
    Const SortKey As String = " ORDER BY " & KeyName1 & "   "

#End Region

#Region "変数定義"
    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
    Dim mclsDB As New ClsSQLSvrDB
    Dim ClsSQL As New ClsSQL
    Dim clsExc As New ClsCMExclusive
    Dim clsMst As New ClsMSTCommon
    Dim objStack As StackFrame
    Dim stb As New StringBuilder
    Dim dv As New DataView
    Dim dt As New DataTable
    Dim DispMode As String
    Dim RcdCase As String = ""
    Dim clsDataConnect As New ClsCMDataConnect

#End Region

#Region "イベントプロシージャ"
    ''' <summary>
    ''' PAGE Init
    ''' </summary>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, DispCode)
    End Sub
    ''' <summary>
    ''' PAGE Load
    ''' </summary>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        ViewState("AppaCdFrom") = txtsAPPACDfrom.Text
        ViewState("AppaCdTo") = txtsAPPACDto.Text
        ViewState("AppaNm") = txtsAPPANM.Text
        ViewState("AppaNmShort") = txtsSHORT.Text
        ViewState("AppaGroup") = ddlsAPPAGroup.SelectedValue
        ViewState("AppaCls") = ddlsAPPACLS.SelectedValue
        ViewState("SystemCd") = ddlsSYS.SelectedValue
        ViewState("DeleteFlg") = ddldel.ppDropDownList.SelectedValue

        '初回実行
        If Not IsPostBack Then
            DispMode = "First"
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            'キー情報保存用ViewState初期化
            Me.ViewState.Add("KeyName1", "")
            Me.ViewState.Add("KeyName2", "")
            Me.ViewState.Add("KeyName3", "")
            Me.ViewState.Add("KeyName4", "")
            Me.ViewState.Add("KeyName5", "")
            '削除データ検索フラグ初期化
            Me.ViewState.Add("SrchDelFlg", "")

            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"

            'ドロップダウンデータ取得
            setDropDownList()

            'コントロールの活性制御
            setControl("Default")
            setControl("Search")

            'フォーカス設定
            txtsAPPACDfrom.Focus()
        End If

        'ボタンイベント設定
        AddHandler Master.ppBtnSrcClear.Click, AddressOf BtnSearchClear_Click
        AddHandler Master.ppBtnSearch.Click, AddressOf BtnSearch_Click
        AddHandler Master.ppBtnClear.Click, AddressOf BtnClear_Click
        AddHandler Master.ppBtnInsert.Click, AddressOf BtnInsert_Click
        AddHandler Master.ppBtnUpdate.Click, AddressOf BtnUpdate_Click
        AddHandler Master.ppBtnDelete.Click, AddressOf BtnDelete_Click

        'キー入力イベント設定
        AddHandler txtAPPACD.TextChanged, AddressOf inputSelectData

        'フォーカス用スクリプト設定
        Master.ppBtnDmy.Attributes.Add("onfocus", "")

        'エラーサマリー活性制御の為のUpdatePanelのUPdateMode調整
        Dim pnl As UpdatePanel
        pnl = Master.FindControl("UpdPanelMain")
        pnl.UpdateMode = UpdatePanelUpdateMode.Always
        pnl = Master.FindControl("UpdPanelSearch")
        pnl.UpdateMode = UpdatePanelUpdateMode.Always

    End Sub
    ''' <summary>
    ''' PAGE PreRender
    ''' </summary>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        '閉じるボタンでEXIT
        If Master.ppCount = "close" OrElse DispMode = "EXIT" Then
            Exit Sub
        End If

        If ViewState("AppaCdFrom") Is Nothing Then
        Else
            txtsAPPACDfrom.Text = ViewState("AppaCdFrom")
        End If
        If ViewState("AppaCdTo") Is Nothing Then
        Else
            txtsAPPACDto.Text = ViewState("AppaCdTo")
        End If
        If ViewState("AppaNm") Is Nothing Then
        Else
            txtsAPPANM.Text = ViewState("AppaNm")
        End If
        If ViewState("AppaNmShort") Is Nothing Then
        Else
            txtsSHORT.Text = ViewState("AppaNmShort")
        End If
        If ViewState("AppaGroup") Is Nothing Then
        Else
            ddlsAPPAGroup.SelectedValue = ViewState("AppaGroup")
        End If
        If ViewState("AppaCls") Is Nothing Then
        Else
            ddlsAPPACLS.SelectedValue = ViewState("AppaCls")
        End If
        If ViewState("SystemCd") Is Nothing Then
        Else
            ddlsSYS.SelectedValue = ViewState("SystemCd")
        End If
        If ViewState("DeleteFlg") Is Nothing Then
        Else
            ddldel.ppDropDownList.SelectedValue = ViewState("DeleteFlg")
        End If

        'ボタンの使用制限
        Select Case DispMode
            Case "DEF", "First"
                '初期　追加
                Master.ppBtnInsert.Enabled = False
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnDelete.Enabled = False
                txtAPPACD.Enabled = True
            Case "ADD"
                '初期　追加
                Master.ppBtnInsert.Enabled = True
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnDelete.Enabled = False
                Master.ppBtnDelete.Text = "削除"
                txtAPPACD.Enabled = False
            Case "UPD"
                '選択後　更新　削除
                Master.ppBtnInsert.Enabled = False
                Master.ppBtnUpdate.Enabled = True
                Master.ppBtnDelete.Enabled = True
                txtAPPACD.Enabled = False
                ddlAPPACLS.Enabled = True
                ddlAPPAGroup.Enabled = True
                'TOMAS名称変更禁止
                '                txtTOMAS.Enabled = True
                txtTOMAS.ReadOnly = False
                txtTOMAS.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF")
            Case "UPD_TMS"
                'TOMASからのデータだった場合、機器分類と機器種別を編集可能とする。
                Master.ppBtnInsert.Enabled = False
                Master.ppBtnUpdate.Enabled = True
                Master.ppBtnDelete.Enabled = True
                txtAPPACD.Enabled = False
                'TOMAS名称変更禁止
                '                txtTOMAS.Enabled = False
                txtTOMAS.ReadOnly = True
                txtTOMAS.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC")
            Case "DEL"
                '削除データ選択時
                Master.ppBtnInsert.Enabled = False
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnDelete.Enabled = True
                Master.ppBtnDmy.Visible = False
        End Select
        'COMUPDM07-001 [印刷区分]の表示／非表示
        msCheckBoxControl()
        'COMUPDM07-001
    End Sub

    '============================================================================================================================
    '==   データ操作
    '============================================================================================================================
    ''' <summary>
    ''' クリアボタン
    ''' </summary>
    Protected Sub BtnClear_Click()

        'ログ出力開始
        psLogStart(Me)

        '★排他情報削除
        Me.Master.ppExclusiveDate = ClsSQL.pfExclDel(Me, DispCode, Me.Master.ppExclusiveDate)

        '選択したキー情報の削除
        ViewState("KeyName1") = ""
        ViewState("KeyName2") = ""
        ViewState("KeyName3") = ""
        ViewState("KeyName4") = ""
        ViewState("KeyName5") = ""

        '項目のクリア
        ClsSQL.ClearControls(Master.ppPlaceHolderMain)

        'コントロールの制御
        setControl("Default")
        Master.ppMainEnabled = True
        Master.ppBtnUpdate.Enabled = True

        'ドロップダウンの初期化
        ddlSYS.Items.Item(0).Text = ""
        ddlSYS.Items.Item(0).Value = ""
        ddlVer.Items.Item(0).Text = ""
        ddlVer.Items.Item(0).Value = ""
        ddlAPPAGroup.Items.Item(0).Text = ""
        ddlAPPAGroup.Items.Item(0).Value = ""
        ddlAPPACLS.Items.Item(0).Text = ""
        ddlAPPACLS.Items.Item(0).Value = ""
        ddlCNSTDET.Items.Item(0).Text = ""
        ddlCNSTDET.Items.Item(0).Value = ""
        ddlHDDCLS.Items.Item(0).Text = ""
        ddlHDDCLS.Items.Item(0).Value = ""
        ddlHDDNo.Items.Item(0).Text = ""
        ddlHDDNo.Items.Item(0).Value = ""
        Master.ppBtnDelete.Text = "削除"
        'COMUPDM07-001
        chkPrtcls.Checked = False
        'COMUPDM07-001 END

        setVal("OFF")
        'フォーカス設定
        FocusChange(txtAPPACD, txtAPPACD)
        'txtAPPACD.Focus()

        'ボタンの使用制限
        DispMode = "DEF"

        'ログ出力終了
        psLogEnd(Me)
    End Sub
    ''' <summary>
    ''' 追加ボタン
    ''' </summary>
    Protected Sub BtnInsert_Click()

        'ログ出力開始
        psLogStart(Me)

        Me.Validate("val")
        If Me.IsValid Then
            msEditData("INSERT")
        End If

        ''実行
        'If ClsSQL.Btn_EditDBData(Me, "追加", MasterName, GetStr("追加")) = True Then

        '    '更新をグリッドに反映
        '    GetData_and_GridBind(GetStr("EditDataKey"))

        '    '項目の初期化
        '    BtnClear_Click()

        '    'フォーカス設定
        '    txtsAPPACDfrom.Focus()
        'End If
        'ログ出力終了
        psLogEnd(Me)
    End Sub
    ''' <summary>
    ''' 更新ボタン
    ''' </summary>
    Protected Sub BtnUpdate_Click()

        'ログ出力開始
        psLogStart(Me)

        Me.Validate("val")
        If Me.IsValid Then
            msEditData("UPDATE")
        End If

        ''実行
        'If ClsSQL.Btn_EditDBData(Me, "更新", MasterName, GetStr("更新")) = True Then

        '    '更新をグリッドに反映
        '    GetData_and_GridBind(GetStr("EditDataKey"))

        '    '項目の初期化
        '    BtnClear_Click()

        '    'フォーカス設定
        '    txtsAPPACDfrom.Focus()
        'End If


        'ログ出力終了
        psLogEnd(Me)
    End Sub
    ''' <summary>
    ''' 削除ボタン
    ''' </summary>
    Protected Sub BtnDelete_Click()

        'ログ出力開始
        psLogStart(Me)

        msEditData("DELETE")

        BtnClear_Click()
        'フォーカス設定
        'txtAPPACD.Focus()
        'ログ出力終了
        psLogEnd(Me)
    End Sub


    '============================================================================================================================
    '==   GRID関係
    '============================================================================================================================
    ''' <summary>
    ''' GRID RowCommand
    ''' </summary>
    Private Sub grvList_RowCommand(sender As Object, e As CommandEventArgs) Handles grvList.RowCommand

        'ログ出力開始
        psLogStart(Me)

        Select Case e.CommandName
            Case "Sort"   'ヘッダー押下でソート
                Exit Sub
            Case "Select"  '選択
                Dim conDB As SqlConnection = Nothing
                Dim cmdDB As SqlCommand = Nothing
                Dim objWKDS As New DataSet

                '接続
                If clsDataConnect.pfOpen_Database(conDB) Then
                    Try
                        Dim rowData As GridViewRow = grvList.Rows(Convert.ToInt32(e.CommandArgument))
                        ''stb.Clear()
                        ''stb.Append("SELECT * FROM " & TableName & " ")
                        'stb.Clear()
                        'stb.Append(" SELECT [M07_APPA_CD] ")
                        'stb.Append("       ,[M07_APPA_NM] ")
                        'stb.Append("       ,[M07_SHORT_NM] ")
                        'stb.Append("       ,[M07_APPA_CLS] ")
                        'stb.Append("       ,[M07_DELETE_FLG] ")
                        'stb.Append("       ,[M07_VERSION] ")
                        'stb.Append("       ,[M07_MODEL_NO] ")
                        'stb.Append("       ,[M07_APPACLASS_CD] ")
                        'stb.Append("       ,[M07_SYSTEM_CD] ")
                        'stb.Append("       ,[M07_TBOX_VER] ")
                        'stb.Append("       ,[M07_HDD_NO] ")
                        'stb.Append("       ,[M07_HDD_CLS] ")
                        'stb.Append("       ,[M07_CNSTDET_CLS] ")
                        'stb.Append("       ,[M07_TOMASUSE_NM] ")
                        'stb.Append("       ,[M07_INSERT_USR] ")
                        'stb.Append("       ,[M07_APPA_CLS] + ' : ' + [M06_APPACLS_NM]		AS 機器種別 ")
                        'stb.Append(" 	   ,CASE [M07_SYSTEM_CD] ")
                        'stb.Append(" 	      WHEN '11' THEN '11 : 磁気無線' ")
                        'stb.Append(" 	      WHEN '12' THEN '12 : 磁気有線' ")
                        'stb.Append(" 	      ELSE ISNULL([M07_SYSTEM_CD] + ' : ' + [M23_TBOXCLS_NM], [M07_SYSTEM_CD] ) ")
                        'stb.Append(" 	    END					AS  システム ")

                        'stb.Append("   FROM [SPCDB].[dbo].[M07_APPA] ")
                        'stb.Append("   LEFT JOIN [SPCDB].[dbo].[M06_APPACLASS] ")
                        'stb.Append("        ON   [M07_APPACLASS_CD] = [M06_APPACLASS_CD] ")
                        'stb.Append("        AND  [M07_APPA_CLS]     = [M06_APPA_CLS] ")
                        'stb.Append("   LEFT JOIN [SPCDB].[dbo].[M23_TBOXCLASS] ")
                        'stb.Append("        ON   M07_SYSTEM_CD = M23_TBOXCLS  ")

                        'stb.Append(" WHERE ")
                        'stb.Append("" & KeyName1 & " = '" & CType(rowData.FindControl("機器コード"), TextBox).Text & "' ")
                        'dt = ClsSQL.getDataSetTable(stb.ToString, "SelectedRow")
                        cmdDB = New SqlCommand("COMUPDM07_S2", conDB)
                        'パラメータ設定
                        cmdDB.Parameters.Add(pfSet_Param("prm_AppaCD", SqlDbType.NVarChar, CType(rowData.FindControl("機器コード"), TextBox).Text))
                        objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)
                        dt = objWKDS.Tables(0).Copy
                        '選択したデータのキーの登録&退避
                        Dim arKey As String() = {KeyName1, KeyName2, KeyName3, KeyName4, KeyName5}
                        Dim strKeyValue As String() = {"", "", "", "", ""}
                        For i As Integer = 0 To arKey.Count - 1
                            If arKey(i) = "" Then
                                Exit For
                            Else
                                Me.ViewState.Add("KeyName" & i + 1 & "", dt(0)(arKey(i)).ToString)
                                strKeyValue(i) = dt(0)(arKey(i)).ToString
                            End If
                        Next

                        '排他制御処理
                        If fExclusive(strKeyValue) = False Then
                            Exit Sub
                        End If

                        '編集エリアに値を反映
                        setControl("UPD")

                        txtAPPACD.Text = dt(0)("M07_APPA_CD").ToString
                        txtAPPANM.Text = dt(0)("M07_APPA_NM").ToString
                        txtSHORTNM.Text = dt(0)("M07_SHORT_NM").ToString

                        setDropdownValue(ddlAPPAGroup, dt(0)("M07_APPACLASS_CD").ToString.Trim)
                        ddlAPPAGroup_SelectedIndexChanged()

                        setDropdownValue(ddlSYS, dt(0)("M07_SYSTEM_CD").ToString, dt(0)("システム").ToString)
                        ddlSYS_SelectedIndexChanged()

                        setDropdownValue(ddlAPPACLS, dt(0)("M07_APPA_CLS").ToString, dt(0)("機器種別").ToString)
                        ddlAPPACLS_SelectedIndexChanged()

                        txtVERSION.Text = dt(0)("M07_VERSION").ToString
                        txtMODELNo.Text = dt(0)("M07_MODEL_NO").ToString

                        setDropdownValue(ddlVer, dt(0)("M07_TBOX_VER").ToString.Trim)
                        setDropdownValue(ddlHDDNo, dt(0)("M07_HDD_NO").ToString.Trim)
                        setDropdownValue(ddlHDDCLS, dt(0)("M07_HDD_CLS").ToString.Trim)

                        txtTOMAS.Text = dt(0)("M07_TOMASUSE_NM").ToString

                        Dim M29CLASS As String
                        Select Case hdnSYS.Value
                            Case "1"
                                M29CLASS = "0118"
                            Case Else
                                M29CLASS = "0120"
                        End Select
                        If ClsSQL.GetRecordCount("SELECT M29_CODE FROM M29_CLASS WHERE M29_CLASS_CD = '" & M29CLASS & "' AND M29_CODE = '" & dt(0)("M07_CNSTDET_CLS").ToString & "' ") <= 0 Then
                            ddlCNSTDET.SelectedIndex = 0
                            ddlCNSTDET.SelectedItem.Text = dt(0)("M07_CNSTDET_CLS").ToString
                            ddlCNSTDET.Items.Item(0).Value = dt(0)("M07_CNSTDET_CLS").ToString
                        Else
                            ddlCNSTDET.SelectedValue = dt(0)("M07_CNSTDET_CLS").ToString
                            ddlCNSTDET.Items.Item(0).Text = ""
                            ddlCNSTDET.Items.Item(0).Value = ""
                        End If

                        'COMUPDM07-001印刷区分の設定
                        If dt(0)("M07_PRT_CLS").ToString.Trim = "0" Then
                            chkPrtcls.Checked = False
                        Else
                            chkPrtcls.Checked = True
                        End If
                        'COMUPDM07-001 END

                        'Select Case intDelCls
                        '    Case 1
                        Select Case dt(0)("" & TableName.Substring(0, 4) & "DELETE_FLG").ToString
                            Case "0"
                                Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '削除
                                Master.ppBtnDelete.Text = "削除"
                                Master.ppMainEnabled = True
                            Case "1"
                                Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
                                Master.ppBtnDelete.Text = "削除取消"
                                Master.ppMainEnabled = False
                        End Select
                        '    Case Else
                        'End Select


                        'ボタンの使用制限
                        Select Case Master.ppBtnDelete.Text
                            Case "削除"
                                If dt(0)("M07_INSERT_USR").ToString = "TOMAS" Then
                                    DispMode = "UPD_TMS"
                                Else
                                    DispMode = "UPD"
                                End If
                                'フォーカス設定
                                txtAPPANM.Focus()
                            Case Else
                                DispMode = "DEL"
                                'フォーカス設定
                                Master.ppBtnClear.Focus()
                        End Select

                    Catch ex As Exception
                        'エラー
                        clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
                        objStack = New StackFrame
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    Finally
                        Call mclsDB.psClearDataSet(objWKDS)
                        '                        dt.Clear()
                        '                        dt.Dispose()
                        'DB切断
                        If Not clsDataConnect.pfClose_Database(conDB) Then
                            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        End If
                    End Try
                End If
        End Select
        setVal("OFF")
        'ログ出力終了
        psLogEnd(Me)
    End Sub
    ''' <summary>
    ''' GRID DataBound
    ''' </summary>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        'ヘッダテキスト設定

        '        Dim strHeader As String() = New String() {"選択", "商品/機器コード", "型式/機器", "機器略称", "機器分類", "機器種別", "システム", "登録日時", "削除"}

        Try
            '            If Not IsPostBack Then
            '                For clm As Integer = 1 To 8
            '                    grvList.Columns(clm).HeaderText = strHeader(clm)
            '                Next
            '            End If

            For Each rowData As GridViewRow In grvList.Rows
                '削除フラグ１の時該当行赤字化
                If CType(rowData.FindControl("削除"), TextBox).Text.Trim = "●" Then
                    CType(rowData.FindControl("機器コード"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("機器名称"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("機器略称"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("機器分類"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("機器種別"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("システム"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("登録日時"), TextBox).ForeColor = Drawing.Color.Red
                End If
            Next
        Catch ex As Exception
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
    End Sub


    '============================================================================================================================
    '==   他
    '============================================================================================================================
    ''' <summary>
    '''編集 機器分類に対応した機器種別をバインド
    ''' </summary>
    Protected Sub ddlAPPAGroup_SelectedIndexChanged() Handles ddlAPPAGroup.SelectedIndexChanged

        If ddlAPPAGroup.SelectedIndex = 0 Then
            ddlAPPACLS.SelectedIndex = 0
            ddlAPPACLS.Enabled = False
        Else
            ddlAPPACLS.Enabled = True
            'stb.Clear()
            'stb.Append(" SELECT [M06_APPA_CLS]                           AS CLS ")
            'stb.Append("       ,[M06_APPA_CLS] + ' : ' + [M06_APPACLS_NM]  AS CLSNM ")
            'stb.Append("   FROM [SPCDB].[dbo].[M06_APPACLASS] ")
            'stb.Append("   WHERE M06_APPACLASS_CD = '" & ddlAPPAGroup.SelectedValue & "' ")
            'stb.Append("     AND M06_DELETE_FLG   = '0' ")
            'ClsSQL.psDropDownDataBind(ddlsAPPACLS, stb.ToString, "CLSNM", "CLS")

            Dim conDB As SqlConnection = Nothing
            Dim cmdDB As SqlCommand = Nothing
            Dim objWKDS As New DataSet

            If clsDataConnect.pfOpen_Database(conDB) Then

                Try
                    cmdDB = New SqlCommand("COMUPDM07_S3", conDB)
                    'パラメータ設定
                    cmdDB.Parameters.Add(pfSet_Param("prm_APPACLASS_CD", SqlDbType.NVarChar, ddlAPPAGroup.SelectedValue))
                    objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)

                    Me.ddlAPPACLS.DataSource = objWKDS.Tables(0)
                    Me.ddlAPPACLS.DataTextField = "CLSNM"
                    Me.ddlAPPACLS.DataValueField = "CLS"
                    Me.ddlAPPACLS.DataBind()
                    Me.ddlAPPACLS.Items.Insert(0, "")
                Catch ex As Exception
                    'エラー
                    clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器種別選択")
                    objStack = New StackFrame
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Finally
                    Call mclsDB.psClearDataSet(objWKDS)
                    '                        dt.Clear()
                    '                        dt.Dispose()
                    'DB切断
                    If Not clsDataConnect.pfClose_Database(conDB) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                End Try
            End If


        End If

        'HDDドロップダウン活性制御
        ddlAPPACLS_SelectedIndexChanged()

        'フォーカス設定
        ddlAPPAGroup.Focus()

    End Sub
    ''' <summary>
    '''検索 機器分類に対応した機器種別をバインド
    ''' </summary>
    Protected Sub ddlsAPPAGroup_SelectedIndexChanged() Handles ddlsAPPAGroup.SelectedIndexChanged

        If ddlsAPPAGroup.SelectedIndex = 0 Then
            ViewState("AppaCls") = Nothing
            ddlsAPPACLS.SelectedIndex = 0
            ddlsAPPACLS.Enabled = False
        Else

            ddlsAPPACLS.SelectedIndex = -1
            ViewState("AppaCls") = Nothing
            ddlsAPPACLS.Enabled = True
            'stb.Clear()
            'stb.Append(" SELECT [M06_APPA_CLS]                           AS CLS ")
            'stb.Append("       ,[M06_APPA_CLS] + ':' + [M06_APPACLS_NM]  AS CLSNM ")
            'stb.Append("   FROM [SPCDB].[dbo].[M06_APPACLASS] ")
            'stb.Append("   WHERE M06_APPACLASS_CD = '" & ddlsAPPAGroup.SelectedValue & "' ")
            ''stb.Append("     AND M06_DELETE_FLG   = '0' ")
            ''ClsSQL.psDropDownDataBind(ddlsAPPACLS, stb.ToString, "CLSNM", "CLS")
            'ClsSQL.psDropDownDataBind(ddlsAPPACLS, stb.ToString, "CLSNM", "CLS")
            Dim conDB As SqlConnection = Nothing
            Dim cmdDB As SqlCommand = Nothing
            Dim objWKDS As New DataSet

            If clsDataConnect.pfOpen_Database(conDB) Then

                Try
                    cmdDB = New SqlCommand("COMUPDM07_S4", conDB)
                    'パラメータ設定
                    cmdDB.Parameters.Add(pfSet_Param("@prm_APPAGroup", SqlDbType.NVarChar, ddlsAPPAGroup.SelectedValue))
                    objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)

                    Me.ddlsAPPACLS.DataSource = objWKDS.Tables(0)
                    Me.ddlsAPPACLS.DataTextField = "CLSNM"
                    Me.ddlsAPPACLS.DataValueField = "CLS"
                    Me.ddlsAPPACLS.DataBind()
                    Me.ddlsAPPACLS.Items.Insert(0, "")
                Catch ex As Exception
                    'エラー
                    clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器分類選択")
                    objStack = New StackFrame
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Finally
                    Call mclsDB.psClearDataSet(objWKDS)
                    '                        dt.Clear()
                    '                        dt.Dispose()
                    'DB切断
                    If Not clsDataConnect.pfClose_Database(conDB) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                End Try
            End If
        End If

        'フォーカス設定
        ddlsAPPAGroup.Focus()

    End Sub
    ''' <summary>
    '''編集 機器種別：ＨＤＤの時にＨＤＤドロップダウン活性
    ''' </summary>
    Protected Sub ddlAPPACLS_SelectedIndexChanged() Handles ddlAPPACLS.SelectedIndexChanged

        If ddlAPPACLS.SelectedValue = "09" Then
            ddlHDDCLS.Enabled = True
            ddlHDDNo.Enabled = True
        Else
            ddlHDDCLS.Enabled = False
            ddlHDDNo.Enabled = False
            ddlHDDCLS.SelectedIndex = 0
            ddlHDDNo.SelectedIndex = 0
        End If

        'フォーカス設定
        ddlAPPACLS.Focus()

    End Sub
    ''' <summary>
    '''編集 SYSTEM_CDに対応した取得先バージョンをバインド
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlSYS_SelectedIndexChanged() Handles ddlSYS.SelectedIndexChanged

        Select Case ddlSYS.SelectedIndex
            Case 0
                If ddlVer.Enabled = True Then
                    ddlVer.SelectedIndex = 0
                    ClsSQL.ControlClear(ddlVer, False)
                End If

                hdnSYS.Value = ""
                '工事依頼書用区分
                ClsSQL.psSetDropDownClassData(ddlCNSTDET, "0120")
            Case Else


                ddlVer.Enabled = True
                stb.Clear()
                stb.Append("SELECT M03_TBOX_VER FROM M03_TBOX WHERE M03_DELETE_FLG = 0 AND LEN(M03_TBOX_VER) = 5 AND M03_TBOX_CD = '" & ddlSYS.SelectedValue.ToString & "' ")
                Dim Ver As New DataTable
                Ver = ClsSQL.getDataSetTable(stb.ToString, "Ver")
                'Ver.Rows.Add(Ver.NewRow)
                Ver.Rows.InsertAt(Ver.NewRow, 0)
                ddlVer.DataSource = Ver
                ddlVer.DataTextField = "M03_TBOX_VER"
                ddlVer.DataValueField = "M03_TBOX_VER"
                ddlVer.DataBind()
                'Select Case ddlSYS.SelectedValue
                '    Case "11", "12"
                '    Case Else
                '        ddlVer.Items.Insert(0, "-")
                'End Select
                'ddlVer.Items.Insert(0, "")

                hdnSYS.Value = ClsSQL.GetRecord("SELECT [M23_SYSTEM_CD]  FROM [SPCDB].[dbo].[M23_TBOXCLASS]  WHERE [M23_TBOXCLS] = '" & ddlSYS.SelectedValue & "' ")


                '工事依頼書用区分
                'If hdnSYS.Value = "1" Or ddlSYS.SelectedValue = "11" Or ddlSYS.SelectedValue = "12" Then
                If hdnSYS.Value = "1" Then
                    'ID/磁気無線/磁気有線
                    ClsSQL.psSetDropDownClassData(ddlCNSTDET, "0118")
                Else
                    'IC/LUTERUNA
                    ClsSQL.psSetDropDownClassData(ddlCNSTDET, "0120")
                End If

        End Select

        'フォーカス設定
        ddlSYS.Focus()

    End Sub


    '============================================================================================================================
    '==   Validation
    '============================================================================================================================

#Region "                             --- Validation ---                                "

    ''' <summary>
    ''' 検索 機器コード
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_s01_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_s01.ServerValidate

        Dim tb As TextBox = txtsAPPACDfrom
        Const Name As String = "機器コード"
        source.ControlToValidate = tb.ID

        If txtsAPPACDfrom.Text = String.Empty And txtsAPPACDto.Text = String.Empty Then
            Exit Sub
        End If

        If (Regex.IsMatch(tb.Text, "^[0-9]{8}$") = True OrElse tb.Text.Trim = "") And (Regex.IsMatch(txtsAPPACDto.Text, "^[0-9]{8}$") = True _
             OrElse txtsAPPACDto.Text.Trim = "") Then
        Else
            source.ErrorMessage = "" & Name & "は半角数字8桁で入力して下さい。"
            source.Text = "形式エラー"
            args.IsValid = False
            Exit Sub
        End If

        Dim intTgt As Int32
        If Int32.TryParse(txtsAPPACDfrom.Text, intTgt) = True AndAlso Int32.TryParse(txtsAPPACDto.Text, intTgt) = True Then
            If Integer.Parse(txtsAPPACDfrom.Text) > Integer.Parse(txtsAPPACDto.Text) Then
                source.ErrorMessage = "" & Name & " は開始が終了以下となるよう入力してください。"
                source.Text = "整合性エラー"
                args.IsValid = False
                Exit Sub
            End If
        End If
    End Sub

    ''' <summary>
    ''' 機器コード
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_APPACD_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_APPACD.ServerValidate

        Dim tb As TextBox = DirectCast(Master.FindControl("MainContent").FindControl(source.ControlToValidate), TextBox)
        Const Name As String = "機器コード"

        If tb.Text = String.Empty Then
            source.ErrorMessage = "" & Name & "に値が設定されていません。"
            source.Text = "未入力エラー"
            args.IsValid = False
            Exit Sub
        End If

        If Regex.IsMatch(tb.Text, "^[0-9]{8}$") Then
        Else
            source.ErrorMessage = "" & Name & "は半角数字 8 桁で入力して下さい。"
            source.Text = "形式エラー"
            args.IsValid = False
            Exit Sub
        End If

    End Sub

    ''' <summary>
    ''' 型式/機器
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_NM_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_NM.ServerValidate

        Dim tb As TextBox = txtAPPANM
        Const Name As String = "型式/機器"
        source.ControlToValidate = tb.ID

        If tb.Text = String.Empty Then
            source.ErrorMessage = "" & Name & "に値が設定されていません。"
            source.Text = "未入力エラー"
            args.IsValid = False
            Exit Sub
        End If

        If tb.Text.ToUpper = "NULL" Then
            source.ErrorMessage = "" & Name & "は不正な値です。"
            source.Text = "形式エラー"
            args.IsValid = False
            Exit Sub
        End If

    End Sub

    ''' <summary>
    ''' 機器略称
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_Short_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_Short.ServerValidate

        Dim tb As TextBox = DirectCast(Master.FindControl("MainContent").FindControl(source.ControlToValidate), TextBox)
        Const Name As String = "機器略称"

        If ddlCNSTDET.SelectedValue <> String.Empty AndAlso tb.Text = String.Empty Then
            source.ErrorMessage = "" & Name & "に値が設定されていません。"
            source.Text = "未入力エラー"
            args.IsValid = False
            Exit Sub
        End If

        If tb.Text.ToUpper = "NULL" Then
            source.ErrorMessage = "" & Name & "は不正な値です。"
            source.Text = "形式エラー"
            args.IsValid = False
            Exit Sub
        End If

    End Sub

    ''' <summary>
    ''' 機器分類
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_APPAGroup_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_APPAGroup.ServerValidate

        Const Name As String = "機器分類"
        Dim ddl As DropDownList = ddlAPPAGroup
        source.ControlToValidate = ddl.ID

        If ddl.SelectedValue = "" Then
            source.ErrorMessage = "" & Name & "に値が設定されていません。"
            source.Text = "未入力エラー"
            args.IsValid = False
            Exit Sub
        End If

    End Sub

    ''' <summary>
    ''' 型番
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_Model_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_Model.ServerValidate

        Const Name As String = "型番"
        Dim tb As TextBox = txtMODELNo
        source.ControlToValidate = tb.ID

        'If tb.Text = String.Empty Then
        '    source.ErrorMessage = "" & Name & "に値が設定されていません。"
        '    source.Text = "未入力エラー"
        '    args.IsValid = False
        '    Exit Sub
        'End If

        If Regex.IsMatch(tb.Text, "^[a-zA-Z0-9 -/:-@\[-\`\{-\~]{0,20}$") = False Then
            source.ErrorMessage = "" & Name & "は半角英数字記号で入力して下さい。"
            source.Text = "形式エラー"
            args.IsValid = False
            Exit Sub
        End If

    End Sub

    ''' <summary>
    ''' 機器種別
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_APPACLS_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_APPACLS.ServerValidate

        Const Name As String = "機器種別"
        Dim ddl As DropDownList = ddlAPPACLS
        source.ControlToValidate = ddl.ID

        If ddl.SelectedValue = "" Then
            source.ErrorMessage = "" & Name & "に値が設定されていません。"
            source.Text = "未入力エラー"
            args.IsValid = False
            Exit Sub
        End If

    End Sub

    ''' <summary>
    ''' システムコード
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_SYS_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_SYS.ServerValidate

        'Const Name As String = "システムコード"
        Dim ddl As DropDownList = DirectCast(Master.FindControl("MainContent").FindControl(source.ControlToValidate), DropDownList)

        If ddl.Enabled = False Then
            Exit Sub
        End If

    End Sub

    ''' <summary>
    ''' TBOXバージョン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_Ver_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_Ver.ServerValidate

        'COMUPDM07-002
        'バージョン
        Dim ddl_ver As DropDownList = DirectCast(Master.FindControl("MainContent").FindControl(source.ControlToValidate), DropDownList)
        '機器バージョン
        Dim txt_ver As TextBox = DirectCast(Master.FindControl("MainContent").FindControl("txtVERSION"), TextBox)
        'システム
        Dim ddl_sys As DropDownList = DirectCast(Master.FindControl("MainContent").FindControl("ddlSYS"), DropDownList)

        If txt_ver.Text = "" Then
            Exit Sub
        End If
        If ddl_sys.Enabled = False Then
            Exit Sub
        End If


        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim dispFlg As String = String.Empty
        objStack = New StackFrame

        If Me.IsPostBack Then 'ポストバックかどうか判定
            dispFlg = "0"
        Else
            dispFlg = "1"
        End If

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(DispCode & "_S13", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_sys", SqlDbType.NVarChar, ddl_sys.SelectedValue))
                    .Add(pfSet_Param("prm_ver", SqlDbType.NVarChar, txt_ver.Text.Trim.ToString))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables.Count = 0 Or dstOrders.Tables(0).Rows.Count = 0 Then
                    Exit Sub
                End If

                If ddl_ver.SelectedValue <> dstOrders.Tables(0).Rows(0).Item("M03_TBOX_VER").ToString Then
                    If ddl_ver.SelectedValue = "" Then
                        source.ErrorMessage = "システムのバージョンは、バージョンから選択してください。"
                        source.Text = "整合性エラー"
                        args.IsValid = False
                        Exit Sub
                    Else
                        source.ErrorMessage = "機器バージョンとバージョンは、同様の値を設定してください。"
                        source.Text = "整合性エラー"
                        args.IsValid = False
                        Exit Sub
                    End If

                End If

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
        End If
        'COMUPDM07-002 END

        'Const Name As String = "TBOXバージョン"
        'Dim ddl As DropDownList = DirectCast(Master.FindControl("MainContent").FindControl(source.ControlToValidate), DropDownList)

        'If ddl.Enabled = False Then
        '    Exit Sub
        'End If

        'If ddl.SelectedIndex = 0 Then
        '    source.ErrorMessage = "" & Name & "に値が設定されていません。"
        '    source.Text = "未入力エラー"
        '    args.IsValid = False
        '    Exit Sub
        'End If

    End Sub

    ''' <summary>
    ''' HDD No
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_HDDNo_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_HDDNo.ServerValidate

        Const Name As String = "HDD No"
        Dim ddl As DropDownList = DirectCast(Master.FindControl("MainContent").FindControl(source.ControlToValidate), DropDownList)

        If ddl.Enabled = False Then
            Exit Sub
        End If

        If ddl.SelectedIndex = 0 And ddlHDDCLS.SelectedIndex <> 0 Then
            source.ErrorMessage = "" & Name & "に値が設定されていません。"
            source.Text = "未入力エラー"
            args.IsValid = False
            Exit Sub
        End If

    End Sub

    ''' <summary>
    ''' HDD 種別
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_HDDCLS_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_HDDCLS.ServerValidate

        'Const Name As String = "HDD種別"
        Dim ddl As DropDownList = DirectCast(Master.FindControl("MainContent").FindControl(source.ControlToValidate), DropDownList)

        If ddl.Enabled = False Then
            Exit Sub
        End If

        If ddl.SelectedIndex = 0 Then
            'source.ErrorMessage = "" & Name & "に値が設定されていません。"
            'source.Text = "未入力エラー"
            'args.IsValid = False
            Exit Sub
        End If

        '重複チェック
        'Dim strKey1 As String = ddlSYS.SelectedValue.ToString
        'Dim strKey2 As String = ddlAPPACLS.SelectedValue.ToString
        'Dim strKey3 As String = txtSEQ.Text
        'Dim strKey4 As String = txtSEQ.Text
        'Dim strKey5 As String = ddlHDDCLS.SelectedValue.ToString

        'stb.Clear()
        'stb.Append("SELECT COUNT(*) FROM " & TableName & " ")
        'stb.Append("WHERE  ")
        'stb.Append("     " & KeyName1 & " = '" & strKey1 & "' ")
        'stb.Append(" AND " & KeyName2 & " = '" & strKey2 & "' ")
        ''stb.Append(" AND " & KeyName3 & " = '" & strKey3 & "' ")
        'stb.Append(" AND  M97_HDD_NO      = '" & ddlHDDNo.SelectedValue.ToString & "' ")
        'stb.Append(" AND  M97_HDD_CLS     = '" & ddlHDDCLS.SelectedValue.ToString & "' ")
        'stb.Append(" AND  NOT (  ")
        'stb.Append(ClsSQL.GeneStrSearch(KeyName1, ViewState("KeyName1"), "完全", ""))
        'stb.Append(ClsSQL.GeneStrSearch(KeyName2, ViewState("KeyName2"), "完全"))
        'stb.Append(ClsSQL.GeneStrSearch(KeyName3, ViewState("KeyName3"), "完全"))
        'stb.Append(ClsSQL.GeneStrSearch(KeyName4, ViewState("KeyName4"), "完全"))
        'stb.Append(ClsSQL.GeneStrSearch(KeyName5, ViewState("KeyName5"), "完全"))

        'stb.Append("     " & KeyName1 & " = '" & ViewState("KeyName1") & "' ")
        'stb.Append(" AND " & KeyName2 & " = '" & ViewState("KeyName2") & "' ")
        'stb.Append(" AND " & KeyName3 & " = '" & ViewState("KeyName3") & "' ")
        'stb.Append(" AND  " & KeyName4 & " = '" & ViewState("KeyName4") & "' ")
        'stb.Append(" AND  " & KeyName5 & " = '" & ViewState("KeyName5") & "' ")
        stb.Append("  ) ")

        '変更・追加する場合
        Select Case ClsSQL.GetRecord(stb.ToString)
            Case -1    'エラー

            Case 0     '存在しない

            Case Else  '存在する
                stb.Clear()
                'stb.Append("システムコード:" & strKey1 & " ")
                'stb.Append("機器種別:" & strKey2 & " ")
                'stb.Append("機器種別:" & strKey3 & " ")
                'stb.Append("連番:" & strKey4 & " ")
                'stb.Append("に　" & Name & "： " & strKey4 & " は既に登録されています。")
                stb.Append("に　HDDNo：" & ddlHDDNo.SelectedValue.ToString & "")
                stb.Append("  HDD種別：" & ddlHDDCLS.SelectedValue.ToString & "  は既に登録されています。")

                source.ErrorMessage = stb.ToString
                source.Text = "整合性エラー"
                args.IsValid = False
        End Select

    End Sub

    ''' <summary>
    ''' 機器バージョン
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub CstmVal_MachineVer_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_MachineVer.ServerValidate
        Dim tb As TextBox = DirectCast(Master.FindControl("MainContent").FindControl(source.ControlToValidate), TextBox)
        If Regex.IsMatch(tb.Text, "^[0-9a-zA-Z\.]{0,10}$") Then
        Else
            source.ErrorMessage = "機器バージョンは半角英数で入力して下さい。"
            source.Text = "形式エラー"
            args.IsValid = False
            Exit Sub
        End If
    End Sub

    Private Sub setVal(ByVal OnOff As String)

        Dim sum_Seq As ValidationSummary

        sum_Seq = Master.FindControl("UpdPanelMain").FindControl("ValidSumKey")

        Select Case OnOff
            Case "ON"
                CstmVal_APPACD.Visible = True
                sum_Seq.Visible = True

            Case "OFF"
                CstmVal_APPACD.Visible = False
                sum_Seq.Visible = False

        End Select
    End Sub

#End Region


#End Region

#Region "そのほかのプロシージャ"
    ''' <summary>
    ''' 検索処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetData()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim dispFlg As String = String.Empty
        objStack = New StackFrame

        If Me.IsPostBack Then 'ポストバックかどうか判定
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
                    .Add(pfSet_Param("AppaCdFrom", SqlDbType.NVarChar, ViewState("AppaCdFrom")))
                    .Add(pfSet_Param("AppaCdTo", SqlDbType.NVarChar, ViewState("AppaCdTo")))
                    .Add(pfSet_Param("AppaNm", SqlDbType.NVarChar, ViewState("AppaNm")))
                    .Add(pfSet_Param("AppaNmShort", SqlDbType.NVarChar, ViewState("AppaNmShort")))
                    .Add(pfSet_Param("AppaGroup", SqlDbType.NVarChar, ViewState("AppaGroup")))
                    .Add(pfSet_Param("AppaCls", SqlDbType.NVarChar, ViewState("AppaCls")))
                    .Add(pfSet_Param("SystemCd", SqlDbType.NVarChar, ViewState("SystemCd")))
                    .Add(pfSet_Param("prog_id", SqlDbType.NVarChar, DispCode))
                    .Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, dispFlg))
                    .Add(pfSet_Param("DeleteFlg", SqlDbType.NVarChar, ViewState("DeleteFlg")))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    Master.ppCount = "0"
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)

                    '総件数とデータセット内の件数(閾値制限)の比較
                ElseIf CType(dstOrders.Tables(0).Rows(0).Item("総件数").ToString, Integer) > dstOrders.Tables(0).Rows.Count Then
                    '閾値オーバー
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
    ''' <param name="ipstrMode"></param>
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
        'Dim drData As DataRow
        objStack = New StackFrame
        Dim getFlg As String = "0"
        Dim strErrorMsg As String = String.Empty
        Dim DtTime As DateTime = DateTime.Now

        Select Case ipstrMode
            Case "INSERT"
                MesCode = "00003"
                procCls = "0"
                strStored = DispCode & "_U1"
                strErrorMsg = "登録"

            Case "UPDATE"
                MesCode = "00001"
                procCls = "1"
                strStored = DispCode & "_U1"
                strErrorMsg = "更新"

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
                        strErrorMsg = "削除取消"
                End Select

        End Select
        '入力チェック

        If (Page.IsValid) OrElse ipstrMode = "DELETE" Then
            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    cmdDB = New SqlCommand(strStored, conDB)
                    Select Case ipstrMode
                        Case "INSERT", "UPDATE"
                            With cmdDB.Parameters
                                'パラメータ設定
                                .Add(pfSet_Param("AppaCd", SqlDbType.NVarChar, txtAPPACD.Text.Trim))
                                .Add(pfSet_Param("AppaNm", SqlDbType.NVarChar, txtAPPANM.Text.Trim))
                                .Add(pfSet_Param("AppaNmShort", SqlDbType.NVarChar, txtSHORTNM.Text.Trim))
                                .Add(pfSet_Param("AppaCls", SqlDbType.NVarChar, ddlAPPACLS.SelectedValue))
                                .Add(pfSet_Param("Ver", SqlDbType.NVarChar, txtVERSION.Text.Trim))
                                .Add(pfSet_Param("Model", SqlDbType.NVarChar, txtMODELNo.Text.Trim))
                                .Add(pfSet_Param("AppaGroup", SqlDbType.NVarChar, ddlAPPAGroup.SelectedValue))
                                .Add(pfSet_Param("SystemCd", SqlDbType.NVarChar, ddlSYS.SelectedValue))
                                .Add(pfSet_Param("TboxVer", SqlDbType.NVarChar, ddlVer.SelectedValue))
                                .Add(pfSet_Param("HDDNo", SqlDbType.NVarChar, ddlHDDNo.SelectedValue))
                                .Add(pfSet_Param("HDDCls", SqlDbType.NVarChar, ddlHDDCLS.SelectedValue))
                                .Add(pfSet_Param("CnstDet", SqlDbType.NVarChar, ddlCNSTDET.SelectedValue))
                                .Add(pfSet_Param("TomasNm", SqlDbType.NVarChar, txtTOMAS.Text.Trim))
                                'COMUPDM07-001
                                .Add(pfSet_Param("Prtcls", SqlDbType.NVarChar, mfIsChecked(chkPrtcls)))
                                'COMUPDM07-001 END
                                .Add(pfSet_Param("now", SqlDbType.DateTime, DtTime))
                                .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))                                      '処理区分
                                .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                             'ユーザーＩＤ
                                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                               '戻り値
                            End With

                        Case "DELETE"
                            With cmdDB.Parameters
                                'パラメータ設定
                                .Add(pfSet_Param("AppaCd", SqlDbType.NVarChar, txtAPPACD.Text.Trim))
                                .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                             'ユーザーＩＤ
                                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                               '戻り値
                            End With

                    End Select

                    'データ登録/更新/削除
                    'トランザクションの開始
                    Using conTrn = conDB.BeginTransaction
                        cmdDB.Transaction = conTrn
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)

                        If intRtn <> 0 Then
                            'ロールバック
                            conTrn.Rollback()
                            psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                            Exit Sub
                        End If
                        'コミット
                        conTrn.Commit()
                    End Using

                    '追加、更新の場合は対象のレコードのみグリッドに表示
                    If ipstrMode = "INSERT" OrElse ipstrMode = "UPDATE" Or getFlg = "0" Then

                        ViewState("AppaCdFrom") = txtAPPACD.Text.Trim
                        ViewState("AppaCdTo") = String.Empty
                        ViewState("AppaNm") = String.Empty
                        ViewState("AppaNmShort") = String.Empty
                        ViewState("AppaGroup") = String.Empty
                        ViewState("AppaCls") = String.Empty
                        ViewState("SystemCd") = String.Empty
                        ViewState("DeleteFlg") = String.Empty
                        GetData()

                    Else
                        '削除の場合はテーブルを初期化
                        dttGrid = New DataTable

                        'データをセット
                        dttGrid.AcceptChanges()
                        grvList.DataSource = dttGrid
                        grvList.DataBind()

                        '件数表示の設定
                        Master.ppCount = dttGrid.Rows.Count

                    End If
                    psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName)
                    'フォーカスチェンジと編集エリア初期化
                    BtnClear_Click()

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
        End If
    End Sub

    ''' <summary>
    '''排他制御処理
    ''' </summary>
    Function fExclusive(ByVal arKeyValue As String()) As Boolean

        fExclusive = False

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
                Exit Function
            End If
        End If

        '★ロック対象テーブル名の登録
        arTable_Name.Insert(0, TableName)

        '★ロックテーブルキー項目の登録
        For i As Integer = 0 To arKeyValue.Count - 1
            If arKeyValue(i) = "" Then
                Exit For
            Else
                arKey.Insert(i, arKeyValue(i))
            End If
        Next

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
            Exit Function
        End If

        fExclusive = True

    End Function
    ''' <summary>
    '''検索条件クリア
    ''' </summary>
    Sub BtnSearchClear_Click()
        'ログ出力開始
        psLogStart(Me)

        '項目のクリア
        ClsSQL.ClearControls(Me.Master.ppPlaceHolderSearch)
        ddldel.ppDropDownList.SelectedIndex = 0

        ViewState("AppaCdFrom") = ""
        ViewState("AppaCdTo") = ""
        ViewState("AppaNm") = ""
        ViewState("AppaNmShort") = ""
        ViewState("AppaGroup") = ""
        ViewState("AppaCls") = ""
        ViewState("SystemCd") = ""
        ViewState("DeleteFlg") = ""

        txtsAPPACDfrom.Text = ""
        txtsAPPACDto.Text = ""
        txtsAPPANM.Text = ""
        txtsSHORT.Text = ""
        ddlsAPPAGroup.SelectedValue = ""
        ddlsAPPACLS.SelectedValue = ""
        ddlsSYS.SelectedValue = ""
        ddldel.ppDropDownList.SelectedValue = ""

        ' Master.ppchksDel.Checked = False
        'コントロールの活性制御
        setControl("Search")

        'フォーカス設定
        ' FocusChange(txtsAPPACDfrom, txtsAPPACDfrom)
        txtsAPPACDfrom.Focus()

        'ログ出力終了
        psLogEnd(Me)
    End Sub
    ''' <summary>
    '''検索ボタン
    ''' </summary>
    Sub BtnSearch_Click()

        'ログ出力開始
        psLogStart(Me)

        If Me.IsValid Then
            ViewState("AppaCdFrom") = txtsAPPACDfrom.Text.Trim
            ViewState("AppaCdTo") = txtsAPPACDto.Text.Trim
            ViewState("AppaNm") = txtsAPPANM.Text.Trim
            ViewState("AppaNmShort") = txtsSHORT.Text.Trim
            ViewState("AppaGroup") = ddlsAPPAGroup.SelectedValue
            ViewState("AppaCls") = ddlsAPPACLS.SelectedValue
            ViewState("SystemCd") = ddlsSYS.SelectedValue
            ViewState("DeleteFlg") = ddldel.ppDropDownList.SelectedValue
            GetData()
        End If

        txtsAPPACDfrom.Focus()

        'ログ出力終了
        psLogEnd(Me)

    End Sub
    ''' <summary>
    ''' DropDownList データ取得＆バインド
    ''' </summary>
    Private Sub setDropDownList()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim objWKDS As New DataSet

        If clsDataConnect.pfOpen_Database(conDB) Then

            Try
                'TBOX機種コード
                cmdDB = New SqlCommand("COMUPDM07_S5", conDB)
                'パラメータ設定
                '                cmdDB.Parameters.Add(pfSet_Param("@prm_APPAGroup", SqlDbType.NVarChar, ddlsAPPAGroup.SelectedValue))
                objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)
                Me.ddlSYS.DataSource = objWKDS.Tables(0)
                Me.ddlSYS.DataTextField = "CLSNM"
                Me.ddlSYS.DataValueField = "M23_TBOXCLS"
                Me.ddlSYS.DataBind()
                Me.ddlSYS.Items.Insert(0, "")
                Call mclsDB.psClearDataSet(objWKDS)

                objWKDS = New DataSet
                cmdDB = New SqlCommand("COMUPDM07_S6", conDB)
                'パラメータ設定
                '                cmdDB.Parameters.Add(pfSet_Param("@prm_APPAGroup", SqlDbType.NVarChar, ddlsAPPAGroup.SelectedValue))
                objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)
                Me.ddlsSYS.DataSource = objWKDS.Tables(0)
                Me.ddlsSYS.DataTextField = "CLSNM"
                Me.ddlsSYS.DataValueField = "M23_TBOXCLS"
                Me.ddlsSYS.DataBind()
                Me.ddlsSYS.Items.Insert(0, "")
                Call mclsDB.psClearDataSet(objWKDS)

                '機器分類
                objWKDS = New DataSet
                cmdDB = New SqlCommand("COMUPDM07_S7", conDB)
                'パラメータ設定
                '                cmdDB.Parameters.Add(pfSet_Param("@prm_APPAGroup", SqlDbType.NVarChar, ddlsAPPAGroup.SelectedValue))
                objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)
                Me.ddlAPPAGroup.DataSource = objWKDS.Tables(0)
                Me.ddlAPPAGroup.DataTextField = "TEXT"
                Me.ddlAPPAGroup.DataValueField = "VALUE"
                Me.ddlAPPAGroup.DataBind()
                Me.ddlAPPAGroup.Items.Insert(0, "")

                Me.ddlsAPPAGroup.DataSource = objWKDS.Tables(0)
                Me.ddlsAPPAGroup.DataTextField = "TEXT"
                Me.ddlsAPPAGroup.DataValueField = "VALUE"
                Me.ddlsAPPAGroup.DataBind()
                Me.ddlsAPPAGroup.Items.Insert(0, "")
                Call mclsDB.psClearDataSet(objWKDS)

                '機器種別
                objWKDS = New DataSet
                cmdDB = New SqlCommand("COMUPDM07_S8", conDB)
                'パラメータ設定
                '                cmdDB.Parameters.Add(pfSet_Param("@prm_APPAGroup", SqlDbType.NVarChar, ddlsAPPAGroup.SelectedValue))
                objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)
                Me.ddlsAPPACLS.DataSource = objWKDS.Tables(0)
                Me.ddlsAPPACLS.DataTextField = "CLSNM"
                Me.ddlsAPPACLS.DataValueField = "CLS"
                Me.ddlsAPPACLS.DataBind()
                Me.ddlsAPPACLS.Items.Insert(0, "")

                Me.ddlAPPACLS.DataSource = objWKDS.Tables(0)
                Me.ddlAPPACLS.DataTextField = "CLSNM"
                Me.ddlAPPACLS.DataValueField = "CLS"
                Me.ddlAPPACLS.DataBind()
                Me.ddlAPPACLS.Items.Insert(0, "")
                Call mclsDB.psClearDataSet(objWKDS)

                'HDDNo
                objWKDS = New DataSet
                cmdDB = New SqlCommand("COMUPDM07_S9", conDB)
                'パラメータ設定
                '                cmdDB.Parameters.Add(pfSet_Param("@prm_APPAGroup", SqlDbType.NVarChar, ddlsAPPAGroup.SelectedValue))
                objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)
                Me.ddlHDDNo.DataSource = objWKDS.Tables(0)
                Me.ddlHDDNo.DataTextField = "HDDNo"
                Me.ddlHDDNo.DataValueField = "HDDNo"
                Me.ddlHDDNo.DataBind()
                Me.ddlHDDNo.Items.Insert(0, "")

                'HDD種別
                objWKDS = New DataSet
                cmdDB = New SqlCommand("COMUPDM07_S10", conDB)
                'パラメータ設定
                '                cmdDB.Parameters.Add(pfSet_Param("@prm_APPAGroup", SqlDbType.NVarChar, ddlsAPPAGroup.SelectedValue))
                objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)
                Me.ddlHDDCLS.DataSource = objWKDS.Tables(0)
                Me.ddlHDDCLS.DataTextField = "HDDCLS"
                Me.ddlHDDCLS.DataValueField = "HDDCLS"
                Me.ddlHDDCLS.DataBind()
                Me.ddlHDDCLS.Items.Insert(0, "")

                '工事依頼書用区分
                objWKDS = New DataSet
                cmdDB = New SqlCommand("COMUPDM07_S11", conDB)
                'パラメータ設定
                cmdDB.Parameters.Add(pfSet_Param("prm_CLASS_CD", SqlDbType.NVarChar, "0120"))
                objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)
                Me.ddlCNSTDET.DataSource = objWKDS.Tables(0)
                Me.ddlCNSTDET.DataTextField = "TEXT"
                Me.ddlCNSTDET.DataValueField = "VALUE"
                Me.ddlCNSTDET.DataBind()
                Me.ddlCNSTDET.Items.Insert(0, "")

            Catch ex As Exception
                'エラー
                clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器分類選択")
                objStack = New StackFrame
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Exit Sub
            Finally
                Call mclsDB.psClearDataSet(objWKDS)
                '                dt.Clear()
                '                dt.Dispose()
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

        ''TBOX機種コード
        'stb.Clear()
        'stb.Append(" SELECT [M23_TBOXCLS] ")
        'stb.Append("       ,(M23_TBOXCLS + ' : ' + M23_TBOXCLS_NM) AS CLSNM ")
        'stb.Append("   FROM [SPCDB].[dbo].[M23_TBOXCLASS]")
        'stb.Append("  WHERE M23_DELETE_FLG ='0' ")
        ''stb.Append(" UNION  ")
        ''stb.Append(" SELECT '11', '11 : 磁気無線' ")
        ''stb.Append(" UNION  ")
        ''stb.Append(" SELECT '12', '12 : 磁気有線' ")
        'ClsSQL.psDropDownDataBind(ddlSYS, stb.ToString, "CLSNM", "M23_TBOXCLS")

        'stb.Clear()
        'stb.Append(" SELECT [M23_TBOXCLS] ")
        'stb.Append("       ,(M23_TBOXCLS + ' : ' + M23_TBOXCLS_NM) AS CLSNM ")
        'stb.Append("   FROM [SPCDB].[dbo].[M23_TBOXCLASS] ")
        ''stb.Append(" UNION  ")
        ''stb.Append(" SELECT '11', '11 : 磁気無線' ")
        ''stb.Append(" UNION  ")
        ''stb.Append(" SELECT '12', '12 : 磁気有線' ")
        'ClsSQL.psDropDownDataBind(ddlsSYS, stb.ToString, "CLSNM", "M23_TBOXCLS")

        'ddlSYS.Items.Item(0).Attributes.Add("style", "color:Red;")
        'ddlSYS.Items.Item(8).Attributes.Add("style", "color:Blue;")

        ''機器分類
        'stb.Clear()
        'stb.Append(" SELECT [M73_APPACLASS_CD] AS VALUE ")
        'stb.Append("       ,[M73_APPACLASS_CD] + ' : ' + [M73_APPACLASS_NM] AS TEXT ")
        'stb.Append("   FROM [SPCDB].[dbo].[M73_APPA_GROUPING] ")
        'stb.Append(" UNION  ")
        'stb.Append(" SELECT '99', '99 : その他' ")
        'ClsSQL.psDropDownDataBind(ddlAPPAGroup, stb.ToString, "TEXT", "VALUE")
        'ClsSQL.psDropDownDataBind(ddlsAPPAGroup, stb.ToString, "TEXT", "VALUE")

        ''機器種別
        'stb.Clear()
        'stb.Append(" SELECT [M06_APPA_CLS]                           AS CLS ")
        'stb.Append("       ,[M06_APPA_CLS] + ':' + [M06_APPACLS_NM]  AS CLSNM ")
        'stb.Append("   FROM [SPCDB].[dbo].[M06_APPACLASS] ")
        'stb.Append("   WHERE M06_APPACLASS_CD = '01' ")
        'stb.Append("     AND M06_DELETE_FLG   = '0' ")
        'ClsSQL.psDropDownDataBind(ddlsAPPACLS, stb.ToString, "CLSNM", "CLS")
        'ClsSQL.psDropDownDataBind(ddlAPPACLS, stb.ToString, "CLSNM", "CLS")

        ''HDDNo
        'stb.Clear()
        'stb.Append(" SELECT DISTINCT [M57_HDD_NO] AS HDDNo ")
        'stb.Append("   FROM [SPCDB].[dbo].[M57_HDD] ")
        'ClsSQL.psDropDownDataBind(ddlHDDNo, stb.ToString, "HDDNo", "HDDNo")

        ''HDD種別
        'stb.Clear()
        'stb.Append(" SELECT DISTINCT [M57_HDD_CLS] AS HDDCLS ")
        'stb.Append("   FROM [SPCDB].[dbo].[M57_HDD] ")
        'ClsSQL.psDropDownDataBind(ddlHDDCLS, stb.ToString, "HDDCLS", "HDDCLS", False)

        ''工事依頼書用区分
        'ClsSQL.psSetDropDownClassData(ddlCNSTDET, "0120")

    End Sub
    ''' <summary>
    ''' コントロール制御
    ''' </summary>
    Private Sub setControl(strCase As String)

        Select Case strCase
            Case "Default"
                ClsSQL.ControlClear(txtAPPANM, False)
                ClsSQL.ControlClear(txtSHORTNM, False)
                ClsSQL.ControlClear(ddlAPPAGroup, False)
                ClsSQL.ControlClear(ddlAPPACLS, False)
                ClsSQL.ControlClear(txtVERSION, False)
                ClsSQL.ControlClear(txtMODELNo, False)
                ClsSQL.ControlClear(ddlHDDNo, False)
                ClsSQL.ControlClear(ddlHDDCLS, False)
                ClsSQL.ControlClear(ddlSYS, False)
                ClsSQL.ControlClear(ddlVer, False)
                ClsSQL.ControlClear(ddlCNSTDET, False)
                '                ClsSQL.ControlClear(txtTOMAS, False)
                txtTOMAS.ReadOnly = True
                txtTOMAS.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC")

                'ddlAPPACLS.Enabled = True
                'ddlSYS.Enabled = True
                'ddlVer.Enabled = True
                'ddlHDDNo.Enabled = True
                'ddlHDDCLS.Enabled = True
            Case "UPD"
                ClsSQL.ControlClear(txtAPPANM, True)
                ClsSQL.ControlClear(txtSHORTNM, True)
                ClsSQL.ControlClear(ddlAPPAGroup, True)
                ClsSQL.ControlClear(ddlAPPACLS, False)
                ClsSQL.ControlClear(txtVERSION, True)
                ClsSQL.ControlClear(txtMODELNo, True)
                ClsSQL.ControlClear(ddlHDDNo, False)
                ClsSQL.ControlClear(ddlHDDCLS, False)
                ClsSQL.ControlClear(ddlSYS, True)
                ClsSQL.ControlClear(ddlVer, False)
                ClsSQL.ControlClear(ddlCNSTDET, True)
                '                ClsSQL.ControlClear(txtTOMAS, True)
                txtTOMAS.ReadOnly = False
                txtTOMAS.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF")
            Case "UPD_TMS"
                ClsSQL.ControlClear(txtAPPANM, True)
                ClsSQL.ControlClear(txtSHORTNM, True)
                ClsSQL.ControlClear(ddlAPPAGroup, True)
                ClsSQL.ControlClear(ddlAPPACLS, True)
                ClsSQL.ControlClear(txtVERSION, True)
                ClsSQL.ControlClear(txtMODELNo, True)
                ClsSQL.ControlClear(ddlHDDNo, False)
                ClsSQL.ControlClear(ddlHDDCLS, False)
                ClsSQL.ControlClear(ddlSYS, True)
                ClsSQL.ControlClear(ddlVer, False)
                ClsSQL.ControlClear(ddlCNSTDET, True)
                '                ClsSQL.ControlClear(txtTOMAS, True)
                txtTOMAS.ReadOnly = False
                txtTOMAS.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF")
            Case "Search"

                ddlsAPPACLS.Enabled = False

        End Select

    End Sub

    ''' <summary>
    ''' キー入力時制御
    ''' </summary>
    Private Sub inputSelectData(ByVal sender As Control, ByVal e As EventArgs)
        System.Threading.Thread.Sleep(250)
        setVal("ON")
        Try
            'フォーカス設定
            sender.Focus()

            'キー項目入力制御
            If txtAPPACD.Text = "" Then
                '未入力キー項目がある場合、キー項目以外を非活性
                setControl("Default")
            Else
                'キー入力値検証
                Me.Validate("key")
                If Me.IsValid = False Then
                    Exit Sub
                End If

                Dim conDB As SqlConnection = Nothing
                Dim cmdDB As SqlCommand = Nothing
                Dim objWKDS As New DataSet

                If clsDataConnect.pfOpen_Database(conDB) Then

                    Try

                        cmdDB = New SqlCommand("COMUPDM07_S12", conDB)
                        'パラメータ設定
                        cmdDB.Parameters.Add(pfSet_Param("prm_APPACD", SqlDbType.NVarChar, txtAPPACD.Text))
                        objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)

                        dt = objWKDS.Tables(0).Copy

                    Catch ex As Exception
                        'エラー
                        clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器分類選択")
                        objStack = New StackFrame
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
                        Exit Sub
                    Finally
                        Call mclsDB.psClearDataSet(objWKDS)
                        '                dt.Clear()
                        '                dt.Dispose()
                        'DB切断
                        If Not clsDataConnect.pfClose_Database(conDB) Then
                            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        End If
                    End Try
                End If

                'stb.Clear()
                'stb.Append(" SELECT [M07_APPA_CD] ")
                'stb.Append("       ,[M07_APPA_NM] ")
                'stb.Append("       ,[M07_SHORT_NM] ")
                'stb.Append("       ,[M07_APPA_CLS] ")
                'stb.Append("       ,[M07_DELETE_FLG] ")
                'stb.Append("       ,[M07_VERSION] ")
                'stb.Append("       ,[M07_MODEL_NO] ")
                'stb.Append("       ,[M07_APPACLASS_CD] ")
                'stb.Append("       ,[M07_SYSTEM_CD] ")
                'stb.Append("       ,[M07_TBOX_VER] ")
                'stb.Append("       ,[M07_HDD_NO] ")
                'stb.Append("       ,[M07_HDD_CLS] ")
                'stb.Append("       ,[M07_CNSTDET_CLS] ")
                'stb.Append("       ,[M07_TOMASUSE_NM] ")
                'stb.Append("       ,[M07_INSERT_USR] ")
                'stb.Append("       ,[M07_APPA_CLS] + ' : ' + [M06_APPACLS_NM]		AS 機器種別 ")
                ''stb.Append(" 	   ,CASE [M07_SYSTEM_CD] ")
                ''stb.Append(" 	      WHEN '11' THEN '11 : 磁気無線' ")
                ''stb.Append(" 	      WHEN '12' THEN '12 : 磁気有線' ")
                ''stb.Append(" 	      ELSE ISNULL([M07_SYSTEM_CD] + ' : ' + [M23_TBOXCLS_NM], [M07_SYSTEM_CD] ) ")
                ''stb.Append(" 	    END					AS  システム ")
                'stb.Append(" 	  ,ISNULL([M07_SYSTEM_CD] + ' : ' + [M23_TBOXCLS_NM], [M07_SYSTEM_CD] )				AS  システム ")
                'stb.Append("   FROM [SPCDB].[dbo].[M07_APPA] ")
                'stb.Append("   LEFT JOIN [SPCDB].[dbo].[M06_APPACLASS] ")
                'stb.Append("        ON   [M07_APPACLASS_CD] = [M06_APPACLASS_CD] ")
                'stb.Append("        AND  [M07_APPA_CLS]     = [M06_APPA_CLS] ")
                'stb.Append("   LEFT JOIN [SPCDB].[dbo].[M23_TBOXCLASS] ")
                'stb.Append("        ON   M07_SYSTEM_CD = M23_TBOXCLS  ")

                'stb.Append("  WHERE [M07_APPA_CD] = '" & txtAPPACD.Text & "' ")
                'dt = ClsSQL.getDataSetTable(stb.ToString, "SelectedRow")

                If dt.Rows.Count = 0 Then
                    'BtnClear_Click()
                    txtAPPANM.Enabled = True
                    txtSHORTNM.Enabled = True
                    ddlAPPAGroup.Enabled = True
                    ddlAPPACLS.Enabled = False
                    txtVERSION.Enabled = True
                    txtMODELNo.Enabled = True
                    ddlHDDNo.Enabled = False
                    ddlHDDCLS.Enabled = False
                    ddlSYS.Enabled = True
                    ddlVer.Enabled = False
                    ddlCNSTDET.Enabled = True
                    '                    txtTOMAS.Enabled = True
                    txtTOMAS.ReadOnly = False
                    txtTOMAS.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF")

                    DispMode = "ADD"
                    'フォーカス設定
                    FocusChange(sender, txtAPPANM)
                Else

                    '選択したデータのキーの登録&退避
                    Dim arKey As String() = {KeyName1, KeyName2, KeyName3, KeyName4, KeyName5}
                    Dim strKeyValue As String() = {"", "", "", "", ""}
                    For i As Integer = 0 To arKey.Count - 1
                        If arKey(i) = "" Then
                            Exit For
                        Else
                            Me.ViewState.Add("KeyName" & i + 1 & "", dt(0)(arKey(i)).ToString)
                            strKeyValue(i) = dt(0)(arKey(i)).ToString
                        End If
                    Next

                    '排他制御処理
                    If fExclusive(strKeyValue) = False Then
                        clsMst.psMesBox(Me, "30010", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)
                        DispMode = "DEF"
                        'BtnClear_Click()
                        Exit Sub
                    End If
                    If dt(0)("M07_INSERT_USR").ToString = "TOMAS" Then
                        setControl("UPD_TMS")
                    Else
                        setControl("UPD")
                    End If

                    '編集エリアに値を反映
                    txtAPPACD.Text = dt(0)("M07_APPA_CD").ToString
                    txtAPPANM.Text = dt(0)("M07_APPA_NM").ToString
                    txtSHORTNM.Text = dt(0)("M07_SHORT_NM").ToString

                    setDropdownValue(ddlAPPAGroup, dt(0)("M07_APPACLASS_CD").ToString.Trim)
                    ddlAPPAGroup_SelectedIndexChanged()

                    setDropdownValue(ddlSYS, dt(0)("M07_SYSTEM_CD").ToString, dt(0)("システム").ToString)
                    ddlSYS_SelectedIndexChanged()

                    setDropdownValue(ddlAPPACLS, dt(0)("M07_APPA_CLS").ToString, dt(0)("機器種別").ToString)
                    ddlAPPACLS_SelectedIndexChanged()

                    txtVERSION.Text = dt(0)("M07_VERSION").ToString
                    txtMODELNo.Text = dt(0)("M07_MODEL_NO").ToString

                    setDropdownValue(ddlVer, dt(0)("M07_TBOX_VER").ToString.Trim)
                    setDropdownValue(ddlHDDNo, dt(0)("M07_HDD_NO").ToString.Trim)
                    setDropdownValue(ddlHDDCLS, dt(0)("M07_HDD_CLS").ToString.Trim)

                    txtTOMAS.Text = dt(0)("M07_TOMASUSE_NM").ToString

                    Dim M29CLASS As String
                    Select Case hdnSYS.Value
                        Case "1"
                            M29CLASS = "0118"
                        Case Else
                            M29CLASS = "0120"
                    End Select
                    If ClsSQL.GetRecordCount("SELECT M29_CODE FROM M29_CLASS WHERE M29_CLASS_CD = '" & M29CLASS & "' AND M29_CODE = '" & dt(0)("M07_CNSTDET_CLS").ToString & "' ") <= 0 Then
                        ddlCNSTDET.SelectedIndex = 0
                        ddlCNSTDET.SelectedItem.Text = dt(0)("M07_CNSTDET_CLS").ToString
                        ddlCNSTDET.Items.Item(0).Value = dt(0)("M07_CNSTDET_CLS").ToString
                    Else
                        ddlCNSTDET.SelectedValue = dt(0)("M07_CNSTDET_CLS").ToString
                        ddlCNSTDET.Items.Item(0).Text = ""
                        ddlCNSTDET.Items.Item(0).Value = ""
                    End If


                    'Select Case intDelCls
                    '    Case 1
                    Select Case dt(0)("" & TableName.Substring(0, 4) & "DELETE_FLG").ToString
                        Case "0"
                            Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '削除
                            Master.ppBtnDelete.Text = "削除"
                            Master.ppMainEnabled = True
                        Case "1"
                            Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
                            Master.ppBtnDelete.Text = "削除取消"
                            Master.ppMainEnabled = False
                    End Select
                    '    Case Else
                    'End Select
                    'COMUPDM07-001印刷区分の設定
                    If dt(0)("M07_PRT_CLS").ToString.Trim = "0" Then
                        chkPrtcls.Checked = False
                    Else
                        chkPrtcls.Checked = True
                    End If
                    'COMUPDM07-001 END
                    'ボタンの使用制限
                    Select Case Master.ppBtnDelete.Text
                        Case "削除"
                            If dt(0)("M07_INSERT_USR").ToString = "TOMAS" Then
                                DispMode = "UPD_TMS"
                            Else
                                DispMode = "UPD"
                            End If

                            'フォーカス設定
                            FocusChange(sender, txtAPPANM)
                        Case Else
                            DispMode = "DEL"
                            'フォーカス設定
                            Master.ppBtnClear.Focus()
                    End Select
                End If
            End If

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try


    End Sub
    ''' <summary>
    ''' フォーカス移動
    ''' </summary>
    Private Sub FocusChange(ByVal objCtrlF As Control, ByVal objCtrlT As Control)

        Dim strBtnDmy As String = Master.ppBtnDmy.ClientID
        Master.ppBtnDmy.Visible = True

        Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + objCtrlT.ClientID + ");"
        SetFocus(Master.ppBtnDmy.ClientID)

    End Sub
    ''' <summary>
    ''' ドロップダウン選択制御
    ''' </summary>
    Private Sub setDropdownValue(ByVal objddl As DropDownList, ByVal strValue As String, Optional strText As String = Nothing)

        '未指定でValue=Text
        If strText Is Nothing Then
            strText = strValue
        End If

        objddl.Items.Item(0).Value = ""
        objddl.Items.Item(0).Text = ""

        '項目が存在しない場合、先頭に値入力
        If objddl.Items.FindByValue(strValue) Is Nothing Then
            objddl.Items.Item(0).Value = strValue
            objddl.Items.Item(0).Text = strText
            objddl.SelectedIndex = 0
        Else
            objddl.Items.Item(0).Value = ""
            objddl.Items.Item(0).Text = ""
            objddl.SelectedValue = strValue
        End If

    End Sub

    'COMUPDM07-001
    ''' <summary>
    ''' チェックボックスの表示／非表示制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheckBoxControl()
        '分類03:店内装置　種別21:サンド
        If ddlAPPAGroup.SelectedValue = "03" AndAlso ddlAPPACLS.SelectedValue = "21" Then
            prtclsName.Visible = True
            chkPrtcls.Visible = True
        Else
            prtclsName.Visible = False
            chkPrtcls.Visible = False
        End If
    End Sub

    ''' <summary>
    ''' 登録／更新時用にチェックボックスの状態を返します
    ''' </summary>
    ''' <param name="chkbox"></param>
    ''' <returns>店内装置かつサンドかつチェック有：1
    ''' 上記以外：0</returns>
    ''' <remarks></remarks>
    Private Function mfIsChecked(ByVal chkbox As CheckBox) As String
        '念の為分類と種別のチェックも入れておきます。
        '分類03:店内装置　種別21:サンド
        If ddlAPPAGroup.SelectedValue = "03" AndAlso ddlAPPACLS.SelectedValue = "21" Then
        Else
            Return "0"
        End If
        If chkbox.Visible = True AndAlso chkbox.Checked = True Then
            Return "1"
        Else
            Return "0"
        End If
    End Function
    'COMUPDM07-001 END

#End Region

#Region "終了処理プロシージャ"

#End Region

End Class
