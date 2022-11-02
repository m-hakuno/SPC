'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　保守担当者マスタ
'*　ＰＧＭＩＤ：　COMUPDM41
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.02.18　：　武
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'COMUPDM41-001     2015/08/21      栗原　　　入力チェック・業者マスタとの整合性チェック・他　ストアド　COMUPDM44_S1,_S2,_S3,_S4,_U1,_D1

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive
Imports SQL_DBCLS_LIB

#End Region

Public Class COMUPDM41

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
    Const DispCode As String = "COMUPDM41"                  '画面ID
    Const MasterName As String = "保守担当者マスタ"           '画面名
    Const DBSName As String = "SPCDB.dbo."                  'DB名.スキーマ名
    Const TableName As String = "M41_MAINTE_CHARGE"        'テーブル名

    '業者マスタ画面パス
    Private Const M_MST_DISP_PATH_M44 = "~/" & P_MST & "/" &
            P_FUN_COM & P_SCR_UPD & "M" & "44" & "/" &
            P_FUN_COM & P_SCR_UPD & "M" & "44.aspx"
    Private Const M_MST_DISP_PATH_MA1 = "~/" & P_MST & "/" &
                P_FUN_COM & P_SCR_UPD & "M" & "A1" & "/" &
                P_FUN_COM & P_SCR_UPD & "M" & "A1.aspx"

#End Region

#Region "変数定義"
    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
    Dim clsDataConnect As New ClsCMDataConnect
    Dim mclsDB As New ClsSQLSvrDB
    Dim ClsSQL As New ClsSQL
    Dim clsExc As New ClsCMExclusive
    Dim objStack As StackFrame
    Dim strMode As String = Nothing
    Dim clsMst As New ClsMSTCommon

#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page Init
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        'グリッドXML定義ファイル取得
        'ヘッダ修正パラメータ
        Dim intHeadCol As Integer() = New Integer() {1, 5, 7, 9}
        Dim intColSpan As Integer() = New Integer() {2, 2, 2, 2}
        pfSet_GridView(Me.grvList, DispCode, intHeadCol, intColSpan)
    End Sub

    ''' <summary>
    ''' Page Load
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim btnFtM44 As Button = DirectCast(Master.FindControl("btnLeft1"), Button) '業者マスタ遷移ボタン
        btnFtM44.Visible = True
        btnFtM44.Text = "業者マスタ"
        Dim btnFtM41 As Button = DirectCast(Master.FindControl("btnLeft2"), Button) '業者マスタ遷移ボタン
        btnFtM41.Visible = True
        btnFtM41.Text = "業者基本マスタ"

        'ボタンアクション設定
        AddHandler Master.ppBtnSearch.Click, AddressOf btnSearch_Click          '検索
        AddHandler Master.ppBtnSrcClear.Click, AddressOf btnSearchClear_Click   '検索条件クリア
        AddHandler Master.ppBtnInsert.Command, AddressOf btn_Click              '登録
        AddHandler Master.ppBtnUpdate.Command, AddressOf btn_Click              '更新
        AddHandler Master.ppBtnDelete.Command, AddressOf btn_Click              '削除
        AddHandler Master.ppBtnClear.Click, AddressOf btnClear_Click            'クリア
        AddHandler btnFtM44.Click, AddressOf btnFtM44_Click                     '画面遷移(業者マスタ)
        AddHandler btnFtM41.Click, AddressOf btnFtM41_Click                     '画面遷移(業者基本マスタ)

        'テキストチェンジ
        AddHandler Me.txtMainteCd.ppTextBox.TextChanged, AddressOf txtMainteCd_TextChanged
        AddHandler Me.txtBranchCd1.ppTextBox.TextChanged, AddressOf GetComp
        'AddHandler Me.ddlBranchCd.ppDropDownList.SelectedIndexChanged, AddressOf ddlBranchCd_IndexChanged

        Me.txtMainteCd.ppTextBox.AutoPostBack = True
        Me.txtBranchCd1.ppTextBox.AutoPostBack = True
        ' Me.ddlBranchCd.ppDropDownList.AutoPostBack = True

        '文言のイベント設定
        Master.ppBtnDmy.Attributes.Add("onfocus", "")
        Master.ppBtnDmy.Visible = False

        '検索項目の入力チェックの為にバリデーターのインスタンスと検証イベント生成
        Dim cuv_Mainte As CustomValidator
        Dim cuv_General As CustomValidator
        cuv_Mainte = tftMainteCd.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv_General = tftGeneralCd.FindControl("pnlErr").FindControl("cuvTextBox")
        AddHandler cuv_Mainte.ServerValidate, AddressOf cuvMainte_ServerValidate
        AddHandler cuv_General.ServerValidate, AddressOf cuvGeneral_ServerValidate

        If Not IsPostBack Then
            'プログラムＩＤ、画面名設定
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"

            'ビューステートの初期化
            ViewState("strBranchCd") = String.Empty
            ViewState("strGeneralCdF") = String.Empty
            ViewState("strGeneralCdT") = String.Empty
            ViewState("strMainteCdF") = String.Empty
            ViewState("strMainteCdT") = String.Empty
            ViewState("strDeleteFlg") = String.Empty

            'Master.ppBtnDelete.Visible = False

            'パンくずリスト設定
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            'ボタン押下時のメッセージ設定
            Master.ppBtnInsert.OnClientClick = pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '追加
            Master.ppBtnUpdate.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '更新

            'グリッドの初期化
            Me.grvList.DataSource = New DataTable
            grvList.DataBind()

            msClearddl()
            msSetddlEmp()

            strMode = "Default"
            Me.txtBranchCd1.ppTextBox.Focus()
        End If

        Dim pnl As UpdatePanel
        pnl = Master.FindControl("UpdPanelMain")
        pnl.UpdateMode = UpdatePanelUpdateMode.Always
        pnl = Master.FindControl("UpdPanelSearch")
        pnl.UpdateMode = UpdatePanelUpdateMode.Always
    End Sub

    ''' <summary>
    ''' Page PreRender
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        Select Case strMode
            Case "Default"
                Master.ppBtnInsert.Enabled = False      '追加
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnClear.Enabled = True       'クリア
                Me.txtMainteCd.ppEnabled = True
                Master.ppBtnDelete.Text = "削除"
                Master.ppMainEnabled = True
                'Me.ddlBranchCd.ppEnabled = False
                Me.ddlGeneralCd.ppEnabled = False
                Me.ddlMaterialCd.ppEnabled = False
                Me.txtSpTelNo.ppEnabled = False
                Me.txtEmTelNo.ppEnabled = False
                Me.txtRemark1.ppEnabled = False
            Case "Select"
                Master.ppBtnInsert.Enabled = False     '追加
                If Master.ppBtnDelete.Text = "削除取消" Then
                    Master.ppBtnUpdate.Enabled = False
                    Master.ppMainEnabled = False
                Else
                    Master.ppBtnUpdate.Enabled = True      '更新
                    Master.ppMainEnabled = True
                End If
                Master.ppBtnDelete.Enabled = True      '削除
                Master.ppBtnClear.Enabled = True       'クリア
                Me.txtMainteCd.ppEnabled = False
                'Me.ddlBranchCd.ppEnabled = True
                Me.ddlGeneralCd.ppEnabled = True
                Me.ddlMaterialCd.ppEnabled = True
                Me.txtSpTelNo.ppEnabled = True
                Me.txtEmTelNo.ppEnabled = True
                Me.txtRemark1.ppEnabled = True
            Case "Insert"
                Master.ppBtnInsert.Enabled = True     '追加
                Master.ppBtnDelete.Text = "削除"
                Master.ppBtnDelete.Enabled = False      '削除
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnClear.Enabled = True       'クリア
                Me.txtMainteCd.ppEnabled = False
                'Me.ddlBranchCd.ppEnabled = True
                Me.ddlGeneralCd.ppEnabled = True
                Me.ddlMaterialCd.ppEnabled = True
                Me.txtSpTelNo.ppEnabled = True
                Me.txtEmTelNo.ppEnabled = True
                Me.txtRemark1.ppEnabled = True
        End Select
    End Sub

    ''' <summary>
    '''検索ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        Page.Validate("search")
        'データ取得
        If (Page.IsValid) And checkEmployee("Search") Then
            'If checkEmployee("Search") Then
            ViewState("strBranchCd") = Me.txtBranchCd1.ppText.Trim
            ViewState("strGeneralCdF") = Me.tftGeneralCd.ppFromText.Trim
            ViewState("strGeneralCdT") = Me.tftGeneralCd.ppToText.Trim
            ViewState("strMainteCdF") = Me.tftMainteCd.ppFromText.Trim
            ViewState("strMainteCdT") = Me.tftMainteCd.ppToText.Trim
            ViewState("strDeleteFlg") = Me.ddldel.ppSelectedValue                 '削除

            msGet_Data()
        End If

        '保担コード入力と競合するとダミーボタンが表示されてしまう事がある為
        Master.ppBtnDmy.Visible = False

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

        txtBranchCd1.ppText = String.Empty
        tftGeneralCd.ppFromText = String.Empty
        tftGeneralCd.ppToText = String.Empty
        tftMainteCd.ppFromText = String.Empty
        tftMainteCd.ppToText = String.Empty
        lblBranchNm.Text = String.Empty
        ddldel.ppDropDownList.SelectedIndex = 0

        '保担コード入力と競合するとダミーボタンが表示されてしまう事がある為
        Master.ppBtnDmy.Visible = False
        'フォーカス移動
        txtBranchCd1.ppTextBox.Focus()
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

        If (Page.IsValid) AndAlso checkEmployee(e.CommandName) Then
            If lblBranch.Text = String.Empty Then
                Dim strErrMsg As String = String.Empty
                Select Case e.CommandName
                    Case "INSERT"
                        strErrMsg = "登録"
                    Case "UPDATE"
                        strErrMsg = "更新"
                    Case "DELETE"
                        strErrMsg = Master.ppBtnDelete.Text
                End Select
                clsMst.psMesBox(Me, "00000", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "支店情報が業者マスタに存在しない為、" & strErrMsg & "出来ません。\n業者マスタを確認して下さい。")
            Else
                msEditData(e.CommandName)
            End If
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

        'ddlBranchCd.ppSelectedValue = String.Empty
        lblBranch.Text = String.Empty
        msClearddl()
        txtMainteCd.ppText = String.Empty
        lblMainteNm.Text = String.Empty
        lblZipNo.Text = String.Empty
        lblAddr.Text = String.Empty
        lblTelNo.Text = String.Empty
        lblFaxNo.Text = String.Empty
        lblTelNoS.Text = String.Empty
        txtEmTelNo.ppText = String.Empty
        txtSpTelNo.ppText = String.Empty
        txtRemark1.ppText = String.Empty

        strMode = "Default"

        '保担コード入力と競合するとダミーボタンが表示されてしまう事がある為
        Master.ppBtnDmy.Visible = False
        txtMainteCd.ppTextBox.Focus()

        'ログ出力終了
        psLogEnd(Me)
        setVal("OFF")
    End Sub

    ''' <summary>
    ''' 画面遷移処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnFtM44_Click()

        '開始ログ出力
        psLogStart(Me)

        'パンクズリストの設定
        Session(P_SESSION_BCLIST) = Master.ppBCList

        '業者マスタに遷移
        psOpen_Window(Me, M_MST_DISP_PATH_M44)

        '終了ログ出力
        psLogEnd(Me)

    End Sub
    Protected Sub btnFtM41_Click()

        '開始ログ出力
        psLogStart(Me)

        'パンクズリストの設定
        Session(P_SESSION_BCLIST) = Master.ppBCList

        '業者マスタに遷移
        psOpen_Window(Me, M_MST_DISP_PATH_MA1)

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 保守担当コード（キー項目）入力
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtMainteCd_TextChanged(sender As Object, e As EventArgs)
        Page.Validate("key")
        If Page.IsValid Then
            GetEmployee(sender, e)
        End If
    End Sub

    ''' <summary>
    ''' 会社（支店）変更
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlBranchCd_IndexChanged(sender As Object, e As EventArgs)

        '営業所/部署情報の再取得
        msSetddlEmp()
        SetFocus(ddlGeneralCd.ppDropDownList.ClientID)

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
                arKey.Insert(0, CType(rowData.FindControl("保担コード"), TextBox).Text)

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


                Dim strTrader_Base As String = mfGet_BaseData(CType(rowData.FindControl("保担コード"), TextBox).Text)
                If strTrader_Base = "False" Then
                    clsMst.psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "保守担当コード「" & _
                                    CType(rowData.FindControl("保担コード"), TextBox).Text & "」" & "は業者基本マスタに存在しません。\n業者基本マスタを確認してください。")
                    SetFocus(Me.txtMainteCd.ppTextBox.ClientID)
                    Exit Sub
                ElseIf strTrader_Base = "Delete" Then
                    clsMst.psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "保守担当コード「" & _
                                     CType(rowData.FindControl("保担コード"), TextBox).Text & "」" & "は業者基本マスタで削除されています。\n業者基本マスタを確認してください。")
                    SetFocus(Me.txtMainteCd.ppTextBox.ClientID)
                    Exit Sub
                ElseIf strTrader_Base = "Many" Then
                    clsMst.psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "保守担当コード「" & _
                                    CType(rowData.FindControl("保担コード"), TextBox).Text & "」" & "は業者基本マスタに複数登録されている為、編集出来ません。\n業者基本マスタを確認してください。")
                    Exit Sub
                Else
                    If mfIsExist_TraderData(CType(rowData.FindControl("保担コード"), TextBox).Text) = False Then
                        clsMst.psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "保守担当コード「" & _
                                    CType(rowData.FindControl("保担コード"), TextBox).Text & "」" & "は業者マスタに存在しません。\n業者マスタを確認してください。")
                        Exit Sub
                    End If
                End If

                '編集エリアに値を設定
                txtMainteCd.ppText = CType(rowData.FindControl("保担コード"), TextBox).Text
                lblMainteNm.Text = CType(rowData.FindControl("保担名"), TextBox).Text
                lblBranch.Text = CType(rowData.FindControl("支店コード"), TextBox).Text & ":" & CType(rowData.FindControl("支店名"), TextBox).Text
                msSetddlEmp()
                Dim lstItem As ListItem = ddlGeneralCd.ppDropDownList.Items.FindByValue(CType(rowData.FindControl("統括保担コード"), TextBox).Text)
                If CType(rowData.FindControl("統括保担コード"), TextBox).Text = "" Then
                    ddlGeneralCd.ppDropDownList.SelectedIndex = -1
                Else
                    If lstItem Is Nothing Then
                        ddlGeneralCd.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                        ddlGeneralCd.ppDropDownList.Items(0).Value = CType(rowData.FindControl("統括保担コード"), TextBox).Text
                        ddlGeneralCd.ppDropDownList.Items(0).Text = CType(rowData.FindControl("統括保担コード"), TextBox).Text & ":" & CType(rowData.FindControl("統括保担名"), TextBox).Text
                    End If
                    ddlGeneralCd.ppSelectedValue = CType(rowData.FindControl("統括保担コード"), TextBox).Text
                End If

                lstItem = ddlMaterialCd.ppDropDownList.Items.FindByValue(CType(rowData.FindControl("部材配備"), TextBox).Text)
                If CType(rowData.FindControl("部材配備"), TextBox).Text = "" Then
                    ddlMaterialCd.ppDropDownList.SelectedIndex = -1
                Else
                    If lstItem Is Nothing Then
                        ddlMaterialCd.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                        ddlMaterialCd.ppDropDownList.Items(0).Value = CType(rowData.FindControl("部材配備"), TextBox).Text
                        ddlMaterialCd.ppDropDownList.Items(0).Text = CType(rowData.FindControl("部材配備"), TextBox).Text & ":" & CType(rowData.FindControl("部材配備名"), TextBox).Text
                    End If
                    ddlMaterialCd.ppSelectedValue = CType(rowData.FindControl("部材配備"), TextBox).Text
                End If

                txtRemark1.ppText = CType(rowData.FindControl("備考"), TextBox).Text
                lblZipNo.Text = CType(rowData.FindControl("郵便番号"), TextBox).Text
                lblAddr.Text = CType(rowData.FindControl("住所"), TextBox).Text
                txtEmTelNo.ppText = CType(rowData.FindControl("緊急連絡番号"), TextBox).Text
                txtSpTelNo.ppText = CType(rowData.FindControl("システムセンタ番号"), TextBox).Text
                lblTelNo.Text = CType(rowData.FindControl("電話番号"), TextBox).Text
                lblFaxNo.Text = CType(rowData.FindControl("FAX番号"), TextBox).Text

                If CType(rowData.FindControl("削除"), TextBox).Text = "" Then
                    Master.ppBtnDelete.Text = "削除"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '削除
                Else
                    Master.ppBtnDelete.Text = "削除取消"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
                End If

                strMode = "Select"
            End If

            '既存データ入力とグリッド選択を同時に行うと、画面表示に不具合が出る為調整
            setVal("OFF")
            Master.ppBtnDmy.Visible = False
            SetFocus(ddlGeneralCd.ppDropDownList.ClientID)

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
    Private Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        'ヘッダテキスト設定

        Dim strHeader As String() = New String() {"選択", "保守担当", "保担名", "電話番号", "FAX番号", "支店", "支店名", "統括保守担当", "統括保担名",
                                                  "部材配備拠点", "部材配備名", "備考", "削除", "緊急連絡番号", "システムセンタ番号",
                                                  "郵便番号", "住所"}

        Try
            If Not IsPostBack Then
                For clm As Integer = 1 To 16
                    grvList.Columns(clm).HeaderText = strHeader(clm)
                Next
            End If

            For Each rowData As GridViewRow In grvList.Rows
                '削除フラグ１の時該当行赤字化
                If CType(rowData.FindControl("削除"), TextBox).Text.Trim = "●" Then
                    CType(rowData.FindControl("保担コード"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("保担名"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("電話番号"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("FAX番号"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("支店コード"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("支店名"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("統括保担コード"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("統括保担名"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("部材配備"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("部材配備名"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("備考"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("緊急連絡番号"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("システムセンタ番号"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("郵便番号"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("FAX番号"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("住所"), TextBox).ForeColor = Drawing.Color.Red
                End If
            Next

        Catch ex As Exception
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

#End Region

#Region "Validation"
    ''' <summary>
    ''' 検索欄桁数チェック(保守担当)
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuvMainte_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim cuv As CustomValidator
        Dim intRsltFrom As Integer
        Dim intRsltTo As Integer

        If tftMainteCd.ppFromText.Trim <> String.Empty AndAlso tftMainteCd.ppToText.Trim <> String.Empty _
            AndAlso tftMainteCd.ppFromText.Length <> tftMainteCd.ppToText.Length _
            AndAlso Integer.TryParse(tftMainteCd.ppFromText, intRsltFrom) AndAlso Integer.TryParse(tftMainteCd.ppToText, intRsltTo) Then

            cuv = tftMainteCd.FindControl("pnlErr").FindControl("cuvTextBox")
            cuv.Text = ClsCMCommon.pfGet_ValMes("3005", tftMainteCd.ppName).Item(P_VALMES_SMES).ToString
            cuv.ErrorMessage = ClsCMCommon.pfGet_ValMes("3005", tftMainteCd.ppName).Item(P_VALMES_MES).ToString
            args.IsValid = False
        End If
    End Sub
    ''' <summary>
    ''' 検索欄桁数チェック(統括保守担当)
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuvGeneral_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim cuv As CustomValidator
        Dim intRsltFrom As Integer
        Dim intRsltTo As Integer

        If tftGeneralCd.ppFromText.Trim <> String.Empty AndAlso tftGeneralCd.ppToText.Trim <> String.Empty _
            AndAlso tftGeneralCd.ppFromText.Length <> tftGeneralCd.ppToText.Length _
            AndAlso Integer.TryParse(tftGeneralCd.ppFromText, intRsltFrom) AndAlso Integer.TryParse(tftGeneralCd.ppToText, intRsltTo) Then

            cuv = tftGeneralCd.FindControl("pnlErr").FindControl("cuvTextBox")
            cuv.Text = ClsCMCommon.pfGet_ValMes("3005", tftGeneralCd.ppName).Item(P_VALMES_SMES).ToString
            cuv.ErrorMessage = ClsCMCommon.pfGet_ValMes("3005", tftGeneralCd.ppName).Item(P_VALMES_MES).ToString
            args.IsValid = False
        End If
    End Sub
#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 検索処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data()

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet
        objStack = New StackFrame

        Try
            '画面ページ表示初期化
            Master.ppCount = "0"
            Me.grvList.DataSource = Nothing
            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Throw New Exception("")
            End If

            'パラメータ設定
            cmdDB = New SqlCommand(DispCode & "_S1", conDB)
            With cmdDB.Parameters
                .Add(pfSet_Param("BranchCd", SqlDbType.NVarChar, ViewState("strBranchCd")))               '支社コード
                .Add(pfSet_Param("GeneralCd_From", SqlDbType.NVarChar, ViewState("strGeneralCdF")))     '統括保担
                .Add(pfSet_Param("GeneralCd_To", SqlDbType.NVarChar, ViewState("strGeneralCdT")))
                .Add(pfSet_Param("MainteCd_From", SqlDbType.NVarChar, ViewState("strMainteCdF")))       '保担コード
                .Add(pfSet_Param("MainteCd_To", SqlDbType.NVarChar, ViewState("strMainteCdT")))
                .Add(pfSet_Param("DeleteFlg", SqlDbType.NVarChar, ViewState("strDeleteFlg")))
                .Add(pfSet_Param("Progid", SqlDbType.NVarChar, DispCode))                               '画面ID
            End With
            'データ取得およびデータをリストに設定
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.grvList.DataSource = dstOrders
            '件数を設定
            Master.ppCount = dstOrders.Tables(0).Rows.Count

            '変更を反映
            Me.grvList.DataBind()

            If dstOrders.Tables(0).Rows.Count = 0 Then
                '0件
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                Master.ppCount = dstOrders.Tables(0).Rows.Count.ToString
            Else
                '閾値を超えた場合はメッセージを表示
                If dstOrders.Tables(0).Rows(0)("件数") > dstOrders.Tables(0).Rows.Count Then
                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                        dstOrders.Tables(0).Rows(0)("件数").ToString, dstOrders.Tables(0).Rows.Count.ToString)
                End If
                Master.ppCount = dstOrders.Tables(0).Rows.Count.ToString()
            End If

            'フォーカス移動
            txtBranchCd1.ppTextBox.Focus()

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守担当者マスタ")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            mclsDB.psDB_Close()
        End Try

    End Sub

    ''' <summary>
    ''' 追加/更新/削除処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msEditData(ByVal ipstrMode As String)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim MesCode As String = String.Empty
        Dim dttGrid As New DataTable
        Dim strStored As String = String.Empty
        Dim getFlg As Boolean = True
        Dim intRtn As Integer
        Dim trans As SqlClient.SqlTransaction             'トランザクション
        objStack = New StackFrame

        Select Case ipstrMode
            Case "INSERT"
                MesCode = "00003"
                strStored = DispCode & "_U1"
            Case "UPDATE"
                MesCode = "00001"
                strStored = DispCode & "_U1"
            Case "DELETE"
                Select Case Master.ppBtnDelete.Text
                    Case "削除"
                        MesCode = "00002"
                        strStored = DispCode & "_D1"
                        getFlg = False
                    Case "削除取消"
                        MesCode = "00001"
                        strStored = DispCode & "_D1"
                End Select
        End Select

        Try
            If clsDataConnect.pfOpen_Database(conDB) Then

                'トランザクションの設定
                trans = conDB.BeginTransaction
                cmdDB = New SqlCommand(strStored, conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.Transaction = trans
                Select Case ipstrMode
                    Case "DELETE"
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("MainteCd", SqlDbType.NVarChar, Me.txtMainteCd.ppText))            '保守担当コード
                            .Add(pfSet_Param("UserId", SqlDbType.NVarChar, Session(P_SESSION_USERID)))          'ユーザーID
                        End With
                        If getFlg = True Then
                            ViewState("strBranchCd") = mfSepColon(Me.lblBranch.Text, 0)
                            ViewState("strGeneralCdF") = Me.ddlGeneralCd.ppSelectedValue
                            ViewState("strGeneralCdT") = ""
                            ViewState("strMainteCdF") = Me.txtMainteCd.ppText.Trim
                            ViewState("strMainteCdT") = ""
                            ViewState("strDeleteFlg") = "0"
                        End If
                    Case Else
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("MainteCd", SqlDbType.NVarChar, Me.txtMainteCd.ppText.Trim))            '保守担当コード
                            '.Add(pfSet_Param("MainteNm", SqlDbType.NVarChar, Me.lblMainteNm.Text.Trim))            '保守担当名
                            '.Add(pfSet_Param("ZipNo", SqlDbType.NVarChar, Me.lblZipNo.Text.Trim))                  '郵便番号
                            '.Add(pfSet_Param("Addr", SqlDbType.NVarChar, Me.lblAddr.Text.Trim))                   '住所
                            '.Add(pfSet_Param("TelNo", SqlDbType.NVarChar, Me.lblTelNo.Text.Trim))                  '電話番号
                            '.Add(pfSet_Param("FaxNo", SqlDbType.NVarChar, Me.lblFaxNo.Text.Trim))                  'FAX番号
                            '.Add(pfSet_Param("BranchCd", SqlDbType.NVarChar, mfSepColon(Me.lblBranch.Text, 0)))            '支店
                            '.Add(pfSet_Param("BranchNm", SqlDbType.NVarChar, mfSepColon(Me.lblBranch.Text, 1)))            '支店名
                            .Add(pfSet_Param("GeneralCd", SqlDbType.NVarChar, mfSepColon(Me.ddlGeneralCd.ppSelectedText.ToString, 0)))         '統括保守担当コード
                            .Add(pfSet_Param("GeneralNm", SqlDbType.NVarChar, mfSepColon(Me.ddlGeneralCd.ppSelectedText.ToString, 1)))         '統括保守担当名
                            .Add(pfSet_Param("MaterialCd", SqlDbType.NVarChar, mfSepColon(Me.ddlMaterialCd.ppSelectedText.ToString, 0)))        '部材配備拠点コード
                            .Add(pfSet_Param("MaterialNm", SqlDbType.NVarChar, mfSepColon(Me.ddlMaterialCd.ppSelectedText.ToString, 1)))        '部材配備拠点名
                            .Add(pfSet_Param("EmeTelNo", SqlDbType.NVarChar, Me.txtEmTelNo.ppText.Trim))            '緊急連絡番号
                            .Add(pfSet_Param("SupportTel", SqlDbType.NVarChar, Me.txtSpTelNo.ppText.Trim))         'サポートセンタ番号
                            .Add(pfSet_Param("Remark", SqlDbType.NVarChar, Me.txtRemark1.ppText.Trim))               '備考
                            .Add(pfSet_Param("UserId", SqlDbType.NVarChar, Session(P_SESSION_USERID)))               'ユーザーID
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                        '戻り値
                        End With
                        If getFlg = True Then
                            ViewState("strBranchCd") = mfSepColon(Me.lblBranch.Text, 0)
                            ViewState("strGeneralCdF") = Me.ddlGeneralCd.ppSelectedValue
                            ViewState("strGeneralCdT") = ""
                            ViewState("strMainteCdF") = Me.txtMainteCd.ppText.Trim
                            ViewState("strMainteCdT") = ""
                            ViewState("strDeleteFlg") = "0"
                        End If
                End Select

                cmdDB.ExecuteNonQuery()

                Select Case ipstrMode
                    Case "DELETE"
                    Case Else
                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn = 1 Then '業者マスタの登録がない場合
                            psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "保守担当コード「" & Me.txtMainteCd.ppText & "」" & _
                                     "は業者マスタに登録がありません。\n業者マスタの登録")
                            mclsDB.psDB_Rollback()
                        ElseIf intRtn = 2 Then
                            '業者マスタに複数件登録が見つかった場合
                            psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "保守担当コード「" & Me.txtMainteCd.ppText & "」" & _
                                     "は業者マスタで複数件見つかりました。\n業者マスタの確認")
                        ElseIf intRtn = 3 Then
                            '業者マスタと統括保守担当が違った場合
                            psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "保守担当コード「" & Me.txtMainteCd.ppText & "」" & _
                                     "の統括保守担当が変更されています。\n業者マスタの更新")
                        End If
                End Select
                trans.Commit()
                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName)
                If getFlg = True Then
                    msGet_Data()
                Else
                    'グリッドの初期化
                    Me.grvList.DataSource = New DataTable
                    grvList.DataBind()
                End If
                btnClear_Click()
            Else
                'ロールバック
                mclsDB.psDB_Rollback()
                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
            End If

        Catch ex As Exception
            psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            mclsDB.psDB_Close()
        End Try
    End Sub

    ''' <summary>
    ''' 業者基本マスタ取得＆保守担当者マスタ取得
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub GetEmployee(sender As Object, e As EventArgs)
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim dsData As DataSet
        Dim strTrader_Base As String '業者基本マスタに情報が存在しているかのフラグ

        System.Threading.Thread.Sleep(50)      'クリアボタンによるキャンセル猶予

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                '保守担当取得
                setVal("ON")
                If Not txtMainteCd.ppText.Trim = String.Empty Then
                    '業者基本マスタから内容を取得
                    strTrader_Base = mfGet_BaseData(Me.txtMainteCd.ppText.Trim)

                    'パラメータ設定
                    '新規か更新かチェック
                    objCmd = New SqlCommand(DispCode & "_S1", objCn)
                    With objCmd.Parameters
                        .Add(pfSet_Param("BranchCd", SqlDbType.NVarChar, String.Empty))                 '支社コード
                        .Add(pfSet_Param("GeneralCd_From", SqlDbType.NVarChar, String.Empty))           '統括保担
                        .Add(pfSet_Param("GeneralCd_To", SqlDbType.NVarChar, String.Empty))
                        .Add(pfSet_Param("MainteCd_From", SqlDbType.NVarChar, txtMainteCd.ppText.Trim)) '保担コード
                        .Add(pfSet_Param("MainteCd_To", SqlDbType.NVarChar, String.Empty))
                        .Add(pfSet_Param("DeleteFlg", SqlDbType.NVarChar, ""))
                        .Add(pfSet_Param("Progid", SqlDbType.NVarChar, DispCode))                       '画面ID
                    End With
                    'データ取得およびデータをリストに設定
                    dsData = clsDataConnect.pfGet_DataSet(objCmd)

                    If dsData.Tables(0).Rows.Count > 0 Then
                        If strTrader_Base = "False" Then
                            Me.txtMainteCd.psSet_ErrorNo("2013", "保守担当コード「" & Me.txtMainteCd.ppText & "」", "業者基本マスタ")
                            SetFocus(Me.txtMainteCd.ppTextBox.ClientID)
                            Exit Sub
                        ElseIf strTrader_Base = "Delete" Then
                            Me.txtMainteCd.psSet_ErrorNo("2013", "保守担当コード「" & Me.txtMainteCd.ppText & "」", "業者基本マスタで削除されています。また、保守担当者マスタ")
                            SetFocus(Me.txtMainteCd.ppTextBox.ClientID)
                            Exit Sub
                        ElseIf strTrader_Base = "Many" Then
                            Me.txtMainteCd.psSet_ErrorNo("2022", "保守担当コード「" & Me.txtMainteCd.ppText & "」", "業者基本マスタに複数登録されています。業者基本マスタ")
                            SetFocus(Me.txtMainteCd.ppTextBox.ClientID)
                            Exit Sub

                        Else
                            If mfIsExist_TraderData(txtMainteCd.ppText.Trim) = False Then
                                '業者マスタに存在無し
                                clsMst.psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "保守担当コード「" & _
                                            txtMainteCd.ppText & "」" & "は業者マスタに存在しません。\n業者マスタを確認してください。")
                            End If
                        End If


                        'ログ出力開始
                        psLogStart(Me)

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
                        arKey.Insert(0, dsData.Tables(0).Rows(0).Item("保担コード").ToString)

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
                        If lblMainteNm.Text.Trim = String.Empty Then
                            lblMainteNm.Text = dsData.Tables(0).Rows(0).Item("保担名").ToString
                        End If
                        lblBranch.Text = dsData.Tables(0).Rows(0).Item("支店コード").ToString & ":" & dsData.Tables(0).Rows(0).Item("支店名").ToString
                        If lblBranch.Text = ":" Then
                            lblBranch.Text = String.Empty
                        End If
                        msSetddlEmp()
                        Dim lstItem As ListItem = ddlGeneralCd.ppDropDownList.Items.FindByValue(dsData.Tables(0).Rows(0).Item("統括保担コード").ToString)
                        If dsData.Tables(0).Rows(0).Item("統括保担コード").ToString = "" Then
                            ddlGeneralCd.ppDropDownList.SelectedIndex = -1
                        Else
                            If lstItem Is Nothing Then
                                ddlGeneralCd.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                                ddlGeneralCd.ppDropDownList.Items(0).Value = dsData.Tables(0).Rows(0).Item("統括保担コード").ToString
                                ddlGeneralCd.ppDropDownList.Items(0).Text = dsData.Tables(0).Rows(0).Item("統括保担コード").ToString & ":" & dsData.Tables(0).Rows(0).Item("統括保担名").ToString
                            End If
                            ddlGeneralCd.ppSelectedValue = dsData.Tables(0).Rows(0).Item("統括保担コード").ToString
                        End If

                        lstItem = ddlMaterialCd.ppDropDownList.Items.FindByValue(dsData.Tables(0).Rows(0).Item("部材配備").ToString)
                        If dsData.Tables(0).Rows(0).Item("部材配備").ToString = "" Then
                            ddlMaterialCd.ppDropDownList.SelectedIndex = -1
                        Else
                            If lstItem Is Nothing Then
                                ddlMaterialCd.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                                ddlMaterialCd.ppDropDownList.Items(0).Value = dsData.Tables(0).Rows(0).Item("部材配備").ToString
                                ddlMaterialCd.ppDropDownList.Items(0).Text = dsData.Tables(0).Rows(0).Item("部材配備").ToString & ":" & dsData.Tables(0).Rows(0).Item("部材配備名").ToString
                            End If
                            ddlMaterialCd.ppSelectedValue = dsData.Tables(0).Rows(0).Item("部材配備").ToString
                        End If

                        txtRemark1.ppText = dsData.Tables(0).Rows(0).Item("備考").ToString
                        txtEmTelNo.ppText = dsData.Tables(0).Rows(0).Item("緊急連絡番号").ToString
                        txtSpTelNo.ppText = dsData.Tables(0).Rows(0).Item("システムセンタ番号").ToString

                        lblZipNo.Text = dsData.Tables(0).Rows(0).Item("郵便番号").ToString
                        lblAddr.Text = dsData.Tables(0).Rows(0).Item("住所").ToString
                        lblTelNo.Text = dsData.Tables(0).Rows(0).Item("電話番号").ToString
                        lblFaxNo.Text = dsData.Tables(0).Rows(0).Item("FAX番号").ToString

                        If dsData.Tables(0).Rows(0).Item("削除").ToString = "" Then
                            Master.ppBtnDelete.Text = "削除"
                            Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '削除
                        Else
                            Master.ppBtnDelete.Text = "削除取消"
                            Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
                        End If
                        strMode = "Select"
                    Else

                        If strTrader_Base = "False" Then
                            Me.txtMainteCd.psSet_ErrorNo("2013", "保守担当コード「" & Me.txtMainteCd.ppText & "」", "業者基本マスタ")
                            SetFocus(Me.txtMainteCd.ppTextBox.ClientID)
                            Exit Sub
                        ElseIf strTrader_Base = "Delete" Then
                            Me.txtMainteCd.psSet_ErrorNo("2013", "保守担当コード「" & Me.txtMainteCd.ppText & "」", "業者基本マスタで削除されています。また、保守担当者マスタ")
                            SetFocus(Me.txtMainteCd.ppTextBox.ClientID)
                            Exit Sub
                        ElseIf strTrader_Base = "Many" Then
                            Me.txtMainteCd.psSet_ErrorNo("2022", "保守担当コード「" & Me.txtMainteCd.ppText & "」", "業者基本マスタに複数登録されています。業者基本マスタ")
                            SetFocus(Me.txtMainteCd.ppTextBox.ClientID)
                            Exit Sub
                        End If

                        If mfIsExist_TraderData(txtMainteCd.ppText.Trim) = False Then
                            '業者マスタに存在無し
                            Me.txtMainteCd.psSet_ErrorNo("2013", "保守担当コード「" & Me.txtMainteCd.ppText & "」", "業者マスタ")
                            lblBranch.Text = String.Empty
                            msClearddl()
                            lblMainteNm.Text = String.Empty
                            lblZipNo.Text = String.Empty
                            lblAddr.Text = String.Empty
                            lblTelNo.Text = String.Empty
                            lblFaxNo.Text = String.Empty
                            lblTelNoS.Text = String.Empty
                            txtEmTelNo.ppText = String.Empty
                            txtSpTelNo.ppText = String.Empty
                            txtRemark1.ppText = String.Empty
                            SetFocus(Me.txtMainteCd.ppTextBox.ClientID)
                            Exit Sub
                        Else
                            lblBranch.Text = mfGetBranchCd(txtMainteCd.ppText)
                            mfGet_BaseData(txtMainteCd.ppText, True)
                        End If
                        msSetddlEmp()
                        strMode = "Insert"

                    End If
                End If
                SetFocus(ddlGeneralCd.ppDropDownList)

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle)
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
    ''' 業者基本マスタから情報取得、表示
    ''' </summary>
    ''' <param name="strOfcCd"></param>
    ''' <returns>True:データ有　False:データ無</returns>
    ''' <remarks></remarks>
    Private Function mfGet_BaseData(ByVal strOfcCd As String, Optional ByVal blnGetData As Boolean = False) As String
        mfGet_BaseData = "False"
        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet
        objStack = New StackFrame

        Try
            '画面初期化
            msClearInfo()

            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Throw New Exception("")
            End If

            'パラメータ設定
            cmdDB = New SqlCommand(DispCode & "_S2", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("Cmp_Dvs", SqlDbType.NVarChar, "1"))
                .Add(pfSet_Param("Ofc_Cod", SqlDbType.NVarChar, strOfcCd))
                .Add(pfSet_Param("DelFlg", SqlDbType.NVarChar, ""))
            End With

            'データ取得
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            If dstOrders.Tables(0).Rows.Count = 0 Then
            ElseIf dstOrders.Tables(0).Rows.Count > 1 Then
                '業者基本マスタ（2:営業所、3:保守担当）内のコード重複により、
                '一意のデータを抽出出来なかった際のエラー
                'psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "入力された保守担当コードに該当する業者基本マスタが" & dstOrders.Tables(0).Rows.Count & "件存在します。\n業者基本マスタの確認")

                If dstOrders.Tables(0).Select("削除 = 0").Count > 1 Then
                    mfGet_BaseData = "Many"

                ElseIf dstOrders.Tables(0).Select("削除 = 0").Count = 0 Then
                    mfGet_BaseData = "Delete"

                ElseIf dstOrders.Tables(0).Select("削除 = 0").Count = 1 Then
                    Dim dtr As DataRow() = dstOrders.Tables(0).Select("削除 = 0").Clone
                    mfGet_BaseData = "True"
                    If blnGetData = True Then
                        '編集エリアに値を設定
                        lblMainteNm.Text = dtr(0).Item("会社名").ToString
                        lblZipNo.Text = dtr(0).Item("郵便番号1").ToString & "-" & dtr(0).Item("郵便番号2").ToString
                        If lblZipNo.Text = "-" Then
                            lblZipNo.Text = String.Empty
                        End If
                        lblAddr.Text = dtr(0).Item("住所1").ToString & dtr(0).Item("住所2").ToString & dtr(0).Item("住所3").ToString
                        lblTelNo.Text = dtr(0).Item("代表TEL1").ToString & "-" & dtr(0).Item("代表TEL2").ToString & "-" & dtr(0).Item("代表TEL3").ToString
                        If lblTelNo.Text = "--" Then
                            lblTelNo.Text = String.Empty
                        End If
                        lblFaxNo.Text = dtr(0).Item("FAX1").ToString & "-" & dtr(0).Item("FAX2").ToString & "-" & dtr(0).Item("FAX3").ToString
                        If lblFaxNo.Text = "--" Then
                            lblFaxNo.Text = String.Empty
                        End If
                    End If

                    lblTelNoS.Text = dtr(0).Item("連絡TEL1").ToString & "-" & dtr(0).Item("連絡TEL2").ToString & "-" & dtr(0).Item("連絡TEL3").ToString
                    If lblTelNoS.Text = "--" Then
                        lblTelNoS.Text = String.Empty
                    End If

                End If

            Else
                If dstOrders.Tables(0).Rows(0).Item("削除").ToString = "1" Then
                    mfGet_BaseData = "Delete"
                Else
                    mfGet_BaseData = "True"
                    If blnGetData = True Then
                        '編集エリアに値を設定
                        lblMainteNm.Text = dstOrders.Tables(0).Rows(0).Item("会社名").ToString
                        lblZipNo.Text = dstOrders.Tables(0).Rows(0).Item("郵便番号1").ToString & "-" & dstOrders.Tables(0).Rows(0).Item("郵便番号2").ToString
                        If lblZipNo.Text = "-" Then
                            lblZipNo.Text = String.Empty
                        End If
                        lblAddr.Text = dstOrders.Tables(0).Rows(0).Item("住所1").ToString & dstOrders.Tables(0).Rows(0).Item("住所2").ToString & dstOrders.Tables(0).Rows(0).Item("住所3").ToString
                        lblTelNo.Text = dstOrders.Tables(0).Rows(0).Item("代表TEL1").ToString & "-" & dstOrders.Tables(0).Rows(0).Item("代表TEL2").ToString & "-" & dstOrders.Tables(0).Rows(0).Item("代表TEL3").ToString
                        If lblTelNo.Text = "--" Then
                            lblTelNo.Text = String.Empty
                        End If
                        lblFaxNo.Text = dstOrders.Tables(0).Rows(0).Item("FAX1").ToString & "-" & dstOrders.Tables(0).Rows(0).Item("FAX2").ToString & "-" & dstOrders.Tables(0).Rows(0).Item("FAX3").ToString
                        If lblFaxNo.Text = "--" Then
                            lblFaxNo.Text = String.Empty
                        End If
                    End If
                    lblTelNoS.Text = dstOrders.Tables(0).Rows(0).Item("連絡TEL1").ToString & "-" & dstOrders.Tables(0).Rows(0).Item("連絡TEL2").ToString & "-" & dstOrders.Tables(0).Rows(0).Item("連絡TEL3").ToString
                    If lblTelNoS.Text = "--" Then
                        lblTelNoS.Text = String.Empty
                    End If
                End If
            End If

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle.ToString)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            mclsDB.psDB_Close()
        End Try

    End Function

    Private Function mfIsExist_TraderData(ByVal strMainteCd As String) As Boolean
        mfIsExist_TraderData = False
        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet
        objStack = New StackFrame

        Try
            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Throw New Exception("")
            End If

            'パラメータ設定
            cmdDB = New SqlCommand("ZCMPSEL029", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("trader_cd", SqlDbType.NVarChar, "2,3"))
                .Add(pfSet_Param("office_cd", SqlDbType.NVarChar, strMainteCd))
            End With

            'データ取得
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            If dstOrders.Tables(0).Rows.Count = 0 Then
                mfIsExist_TraderData = False
            ElseIf dstOrders.Tables(0).Rows.Count >= 1 Then
                'mfGetBranchName(dstOrders.Tables(0).Rows(0).Item("会社コード").ToString)
                mfIsExist_TraderData = True
            End If

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle.ToString)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            mclsDB.psDB_Close()
        End Try
    End Function
    Private Function mfGetBranchCd(ByVal strOfficeCd As String) As String
        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet
        objStack = New StackFrame
        mfGetBranchCd = Nothing '加賀追加
        Try
            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Throw New Exception("")
            End If

            'パラメータ設定
            cmdDB = New SqlCommand("ZCMPSEL029", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("trader_cd", SqlDbType.NVarChar, "2,3"))
                .Add(pfSet_Param("office_cd", SqlDbType.NVarChar, strOfficeCd))
            End With

            'データ取得
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            If dstOrders.Tables(0).Rows.Count = 0 Then
                mfGetBranchCd = ""
            ElseIf dstOrders.Tables(0).Rows.Count >= 1 Then
                mfGetBranchCd = dstOrders.Tables(0).Rows(0).Item("会社コード").ToString & ":" & dstOrders.Tables(0).Rows(0).Item("会社名").ToString
            End If

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle.ToString)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            mclsDB.psDB_Close()
        End Try
    End Function
    Private Function mfGetBranchName(ByVal strBranchCd As String) As Boolean
        mfGetBranchName = False
        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet
        objStack = New StackFrame

        Try
            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Throw New Exception("")
            End If

            'パラメータ設定
            cmdDB = New SqlCommand(DispCode & "_S2", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("Cmp_Dvs", SqlDbType.NVarChar, "0"))
                .Add(pfSet_Param("Ofc_Cod", SqlDbType.NVarChar, strBranchCd))
                .Add(pfSet_Param("DelFlg", SqlDbType.NVarChar, "0"))
            End With

            'データ取得
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            If dstOrders.Tables(0).Rows.Count = 0 Then
                mfGetBranchName = False
            ElseIf dstOrders.Tables(0).Rows.Count >= 1 Then
                lblBranch.Text = dstOrders.Tables(0).Rows(0).Item("会社コード").ToString & ":" & dstOrders.Tables(0).Rows(0).Item("会社名").ToString
                If lblBranch.Text = ":" Then
                    lblBranch.Text = ""
                End If
                mfGetBranchName = True
            End If

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle.ToString)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            mclsDB.psDB_Close()
        End Try
    End Function

    ''' <summary>
    ''' 住所、TEL等初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearInfo()
        'lblBranch.Text = String.Empty
        'lblZipNo.Text = String.Empty
        'lblAddr.Text = String.Empty
        'lblTelNo.Text = String.Empty
        'lblFaxNo.Text = String.Empty
        lblTelNoS.Text = String.Empty
    End Sub

    ''' <summary>
    ''' ドロップダウンリスト初期化(支店・統括保守担当・部材配備拠点)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearddl()
        'Me.ddlBranchCd.ppDropDownList.Items.Clear()
        Me.ddlGeneralCd.ppDropDownList.Items.Clear()
        Me.ddlMaterialCd.ppDropDownList.Items.Clear()
    End Sub

    ''' <summary>
    ''' ドロップダウンリスト　営業所（統括保守担当・部材配備拠点）取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlEmp()
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim dsData As DataSet
        Dim strOfcFlg As String = Nothing
        objStack = New StackFrame

        If lblBranch.Text = String.Empty Then
            Me.ddlGeneralCd.ppDropDownList.Items.Clear()
            Me.ddlMaterialCd.ppDropDownList.Items.Clear()
            SetFocus(ddlGeneralCd.ppDropDownList.ClientID)
            Exit Sub
        End If

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            '業者基本マスタから内容を取得
            Try
                '会社が決まってから営業所を表示する。
                If lblBranch.Text <> String.Empty Then

                    objCmd = New SqlCommand(DispCode & "_S5", objCn)
                    With objCmd.Parameters
                        '--パラメータ設定
                        .Add(pfSet_Param("Cmp_Dvs", SqlDbType.NVarChar, "1"))
                        .Add(pfSet_Param("Cmp_Cod", SqlDbType.NVarChar, mfSepColon(lblBranch.Text, 0)))
                    End With

                    'データ取得
                    dsData = clsDataConnect.pfGet_DataSet(objCmd)

                    Me.ddlGeneralCd.ppDropDownList.Items.Clear()
                    Me.ddlGeneralCd.ppDropDownList.DataSource = dsData.Tables(0)
                    Me.ddlGeneralCd.ppDropDownList.DataTextField = "項目名"
                    Me.ddlGeneralCd.ppDropDownList.DataValueField = "コード"
                    Me.ddlGeneralCd.ppDropDownList.DataBind()
                    Me.ddlGeneralCd.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

                    Me.ddlMaterialCd.ppDropDownList.Items.Clear()
                    Me.ddlMaterialCd.ppDropDownList.DataSource = dsData.Tables(0)
                    Me.ddlMaterialCd.ppDropDownList.DataTextField = "項目名"
                    Me.ddlMaterialCd.ppDropDownList.DataValueField = "コード"
                    Me.ddlMaterialCd.ppDropDownList.DataBind()
                    Me.ddlMaterialCd.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                    SetFocus(txtSpTelNo.ppTextBox.ClientID)
                End If

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle)
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
    ''' 業者基本マスタから会社名取得
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub GetComp(sender As Object, e As EventArgs)
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim dsData As DataSet
        objStack = New StackFrame
        If Not txtBranchCd1.ppText.Trim = String.Empty Then
            'DB接続
            If Not clsDataConnect.pfOpen_Database(objCn) Then
                clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else
                '業者基本マスタから会社名を取得
                Try
                    objCmd = New SqlCommand(DispCode & "_S4", objCn)
                    With objCmd.Parameters
                        '--パラメータ設定
                        .Add(pfSet_Param("BranchCd", SqlDbType.NVarChar, Me.txtBranchCd1.ppText.Trim))
                    End With
                    'データ取得
                    dsData = clsDataConnect.pfGet_DataSet(objCmd)
                    If dsData.Tables(0).Rows.Count > 0 Then
                        With dsData.Tables(0).Rows(0)
                            If Not Microsoft.VisualBasic.StrConv(.Item("会社名").ToString, Microsoft.VisualBasic.VbStrConv.Narrow) = ":" Then
                                lblBranchNm.Text = .Item("会社名").ToString
                            Else
                                lblBranch.Text = ""
                            End If
                        End With
                        'フォーカスを移動
                        SetFocus(tftMainteCd.ppTextBoxFrom.ClientID)
                    Else
                        lblBranchNm.Text = ""
                        Me.txtBranchCd1.psSet_ErrorNo("2002", "支店コード「" & Me.txtBranchCd1.ppText & "」")
                        'フォーカスを移動()
                        SetFocus(txtBranchCd1.ppTextBox.ClientID)
                    End If

                Catch ex As Exception
                    clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle)
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
        Else
            lblBranchNm.Text = String.Empty
            SetFocus(tftMainteCd.ppTextBoxFrom.ClientID)
        End If
    End Sub

    ''' <summary>
    ''' 業者基本マスタの存在チェック
    ''' </summary>
    ''' <param name="strCommandName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function checkEmployee(ByVal strCommandName As String) As Boolean
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim dsData As DataSet
        Dim blnGeneral_Trader As Boolean = False
        Dim blnGeneral_Mainte As Boolean = False
        Dim blnMaterial As Boolean = False
        objStack = New StackFrame

        Select Case strCommandName
            Case "DELETE"
                checkEmployee = True
                Exit Function

            Case "Search"
                If txtBranchCd1.ppText.Trim <> String.Empty AndAlso lblBranchNm.Text = String.Empty Then
                    checkEmployee = False
                Else
                    checkEmployee = True
                End If

            Case Else
                If Not clsDataConnect.pfOpen_Database(objCn) Then
                    clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Else
                    Try

                        '統括保守担当
                        If Not ddlGeneralCd.ppSelectedValue = String.Empty Then
                            objCmd = New SqlCommand(DispCode & "_S2", objCn)
                            With objCmd.Parameters
                                '--パラメータ設定
                                .Add(pfSet_Param("Cmp_Dvs", SqlDbType.NVarChar, "1"))
                                .Add(pfSet_Param("Ofc_Cod", SqlDbType.NVarChar, Me.ddlGeneralCd.ppSelectedValue))
                                .Add(pfSet_Param("DelFlg", SqlDbType.NVarChar, "0"))
                            End With

                            'データ取得
                            dsData = clsDataConnect.pfGet_DataSet(objCmd)

                            If dsData.Tables(0).Rows.Count > 0 Then
                                blnGeneral_Trader = True
                            Else
                                Me.ddlGeneralCd.psSet_ErrorNo("2002", "統括保守担当コード「" & Me.ddlGeneralCd.ppSelectedValue & "」")
                                blnGeneral_Trader = False
                            End If
                        Else
                            blnGeneral_Trader = True
                        End If

                        '統括保守担当保守担当者マスタ
                        If Not ddlGeneralCd.ppSelectedValue = String.Empty Then
                            objCmd = New SqlCommand(DispCode & "_S1", objCn)
                            With objCmd.Parameters
                                '--パラメータ設定
                                .Add(pfSet_Param("BranchCd", SqlDbType.NVarChar, ""))               '支社コード
                                .Add(pfSet_Param("GeneralCd_From", SqlDbType.NVarChar, ""))     '統括保担
                                .Add(pfSet_Param("GeneralCd_To", SqlDbType.NVarChar, ""))
                                .Add(pfSet_Param("MainteCd_From", SqlDbType.NVarChar, ddlGeneralCd.ppSelectedValue))       '保担コード
                                .Add(pfSet_Param("MainteCd_To", SqlDbType.NVarChar, ""))
                                .Add(pfSet_Param("DeleteFlg", SqlDbType.NVarChar, ""))
                                .Add(pfSet_Param("Progid", SqlDbType.NVarChar, DispCode))                               '画面ID
                            End With
                            'データ取得
                            dsData = clsDataConnect.pfGet_DataSet(objCmd)

                            If dsData.Tables(0).Rows.Count > 0 Then
                                blnGeneral_Mainte = True
                            Else
                                Me.ddlGeneralCd.psSet_ErrorNo("2013", "統括保守担当コード「" & Me.ddlGeneralCd.ppSelectedValue & "」", "保守担当者マスタ")
                                blnGeneral_Mainte = False
                            End If
                        Else
                            blnGeneral_Mainte = True
                        End If
                        '部材配備拠点
                        If Not ddlMaterialCd.ppSelectedValue = String.Empty Then
                            objCmd = New SqlCommand(DispCode & "_S2", objCn)
                            With objCmd.Parameters
                                '--パラメータ設定
                                .Add(pfSet_Param("Cmp_Dvs", SqlDbType.NVarChar, "1"))
                                .Add(pfSet_Param("Ofc_Cod", SqlDbType.NVarChar, Me.ddlMaterialCd.ppSelectedValue))
                                .Add(pfSet_Param("DelFlg", SqlDbType.NVarChar, "0"))
                            End With

                            'データ取得
                            dsData = clsDataConnect.pfGet_DataSet(objCmd)

                            If dsData.Tables(0).Rows.Count > 0 Then
                                blnMaterial = True
                            Else
                                Me.ddlMaterialCd.psSet_ErrorNo("2002", "部材配備拠点コード「" & Me.ddlMaterialCd.ppSelectedValue & "」")
                                blnMaterial = False
                            End If
                        Else
                            blnMaterial = True
                        End If


                    Catch ex As Exception
                        clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle)
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

                If blnGeneral_Trader AndAlso blnGeneral_Mainte AndAlso blnMaterial Then
                    checkEmployee = True
                Else
                    checkEmployee = False
                    Dim strErrCode As String = String.Empty
                    If strCommandName = "INSERT" Then
                        strErrCode = "00003"
                    ElseIf strCommandName = "UPDATE" Then
                        strErrCode = "00001"
                    End If
                    If blnGeneral_Trader = False AndAlso blnMaterial = False Then   '業者基本の存在有無が最優先（基本マスタに存在有で初めて保担マスタの存在有無を表示する。）
                        clsMst.psMesBox(Me, strErrCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "統括保守拠点、部材配備拠点が業者基本マスタから削除された為、保守担当者マスタ")
                    ElseIf blnGeneral_Trader = False Then
                        clsMst.psMesBox(Me, strErrCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "統括保守拠点が業者基本マスタから削除された為、保守担当者マスタ")
                    ElseIf blnMaterial = False Then
                        clsMst.psMesBox(Me, strErrCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "部材配備拠点が業者基本マスタから削除された為、保守担当者マスタ")
                    ElseIf blnGeneral_Mainte = False Then
                        clsMst.psMesBox(Me, strErrCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "統括保守拠点が保守担当者マスタに存在しない為、保守担当者マスタ")
                    Else
                        clsMst.psMesBox(Me, strErrCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "業者マスタとの整合性エラーの為、保守担当者マスタ")
                    End If

                End If
        End Select
    End Function

    ''' <summary>
    '''フォーカス移動 
    ''' </summary>
    ''' <param name="TxtBoxF"></param>
    ''' <param name="TxtBoxT"></param>
    ''' <remarks></remarks>
    Private Sub FocusChange(ByVal TxtBoxF As TextBox, ByVal TxtBoxT As TextBox)

        Dim strBtnDmy As String = Master.ppBtnDmy.ClientID
        Master.ppBtnDmy.Visible = True

        Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + TxtBoxT.ClientID + ");"
        SetFocus(Master.ppBtnDmy.ClientID)

    End Sub

    ''' <summary>
    ''' エラーサマリーの表示制御
    ''' </summary>
    ''' <param name="ONorOFF"></param>
    ''' <remarks></remarks>
    Private Sub setVal(ByVal ONorOFF As String)
        Dim cuv_Mainte As CustomValidator
        Dim sum_Mainte As ValidationSummary
        cuv_Mainte = txtMainteCd.FindControl("pnlErr").FindControl("cuvTextBox")
        sum_Mainte = Master.FindControl("UpdPanelMain").FindControl("ValidSumKey")

        '不正入力と他の処理が競合した時に、エラーサマリーだけが残るのを防ぐ
        Select Case ONorOFF
            Case "ON"
                cuv_Mainte.Visible = True
                sum_Mainte.Visible = True

            Case "OFF"
                cuv_Mainte.Visible = False
                sum_Mainte.Visible = False

        End Select
    End Sub

    ''' <summary>
    ''' "："を区切り文字としてテキストを分割する(空欄なら空文字を返す)
    ''' </summary>
    ''' <param name="strText">"："入りのテキスト</param>
    ''' <param name="intSeparator">"："区切りの0:前　1:後</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfSepColon(ByVal strText As String, ByVal intSeparator As Integer) As String
        If strText.Trim <> String.Empty Then
            If strText.Contains(":") Then
                mfSepColon = strText.Split(":")(intSeparator)
            ElseIf strText.Contains("：") Then
                mfSepColon = strText.Split("：")(intSeparator)
            Else
                mfSepColon = strText
            End If
        Else
            mfSepColon = ""
        End If
    End Function

#End Region

#Region "終了処理プロシージャ"

#End Region

End Class
