'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　機器備考マスタ
'*　ＰＧＭＩＤ：　COMUPDM97
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2016.06.13　：　稲葉
'********************************************************************************************************************************

#Region "インポート定義"

Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive
Imports SQL_DBCLS_LIB

#End Region

Public Class COMUPDM97

#Region "継承定義"

    Inherits System.Web.UI.Page

#End Region

#Region "定数定義"

    Const DispCode As String = "COMUPDM97"                  '画面ID
    Const MasterName As String = "シリアル特殊条件マスタ"           '画面名
    Const TableName As String = "M97_BRING_CONDITION"       'テーブル名
    Const ApaCls_Ctr As String = "01"   '機器種別：制御部のコード
    Const ApaCls_Hdd As String = "09"   '機器種別：HDDのコード

    '業者マスタ画面パス
    Const M_MST_DISP_PATH = "~/" & P_MST & "/" &
            P_FUN_COM & P_SCR_UPD & "M" & "97" & "/" &
            P_FUN_COM & P_SCR_UPD & "M" & "97.aspx"

#End Region

#Region "変数定義"

    Dim clsDataConnect As New ClsCMDataConnect
    Dim mclsDB As New ClsSQLSvrDB
    Dim clsExc As New ClsCMExclusive
    Dim objStack As StackFrame
    Dim strMode As String = Nothing
    Dim strMode_other As String = Nothing
    Dim clsMst As New ClsMSTCommon

#End Region

#Region "プロパティ定義"

    ''' <summary>
    ''' 入力欄の活性/非活性
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property mpMainInpEnable() As Boolean
        Get
            Return mpMainInpEnable
        End Get
        Set(value As Boolean)
            'Me.txtEmTel3.ppEnabled = value
        End Set
    End Property

    ''' <summary>
    ''' キー項目の活性/非活性
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property mpKeyInpEnable() As Boolean
        Get
            Return mpKeyInpEnable
        End Get
        Set(value As Boolean)
            Me.ddlAppaDvs.ppDropDownList.Enabled = value
            Me.ddlAppaCls.ppDropDownList.Enabled = value
            'Me.txtCd.ppEnabled = value
        End Set
    End Property

#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page Init
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'グリッドXML定義ファイル取得
        'ヘッダ修正パラメータ
        Dim intHeadCol As Integer() = New Integer() {1, 3}
        Dim intColSpan As Integer() = New Integer() {2, 2}
        pfSet_GridView(Me.grvList, DispCode, intHeadCol, intColSpan)

    End Sub

    ''' <summary>
    ''' Page Load
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        'ボタンアクション設定
        AddHandler Master.ppBtnSearch.Click, AddressOf btnSearch_Click          '検索
        AddHandler Master.ppBtnSrcClear.Click, AddressOf btnSearchClear_Click   '検索条件クリア
        AddHandler Master.ppBtnInsert.Command, AddressOf btn_Click              '登録
        AddHandler Master.ppBtnUpdate.Command, AddressOf btn_Click              '更新
        AddHandler Master.ppBtnDelete.Command, AddressOf btn_Click              '削除
        AddHandler Master.ppBtnClear.Click, AddressOf btnClear_Click            'クリア

        'テキスト・選択リスト　チェンジイベント
        AddHandler Me.ddlsAppaDvs.ppDropDownList.SelectedIndexChanged, AddressOf ddlsAppaDvs_SelectedIndexChanged
        AddHandler Me.ddlsAppaCls.ppDropDownList.SelectedIndexChanged, AddressOf ddlsAppaCls_SelectedIndexChanged
        AddHandler Me.ddlAppaDvs.ppDropDownList.SelectedIndexChanged, AddressOf ddlAppaDvs_SelectedIndexChanged
        AddHandler Me.ddlAppaCls.ppDropDownList.SelectedIndexChanged, AddressOf ddlAppaCls_SelectedIndexChanged
        AddHandler Me.ddlKijun.ppDropDownList.SelectedIndexChanged, AddressOf ddlKijun_SelectedIndexChanged
        AddHandler Me.txtSeq.ppTextBox.TextChanged, AddressOf txtSeq_TextChanged
        AddHandler Me.cbxFlg.CheckedChanged, AddressOf cbxFlg_CheckedChanged

        Dim cuv_Hdd_No As CustomValidator = ddlHddNo.FindControl("pnlErr").FindControl("cuvDropDownList")
        Dim cuv_Hdd_Cls As CustomValidator = ddlHddCls.FindControl("pnlErr").FindControl("cuvDropDownList")
        AddHandler cuv_Hdd_No.ServerValidate, AddressOf cuv_Hdd_No_ServerValidate
        AddHandler cuv_Hdd_Cls.ServerValidate, AddressOf cuv_Hdd_Cls_ServerValidate

        Me.ddlsAppaDvs.ppDropDownList.AutoPostBack = True
        Me.ddlsAppaCls.ppDropDownList.AutoPostBack = True
        Me.ddlAppaDvs.ppDropDownList.AutoPostBack = True
        Me.ddlAppaCls.ppDropDownList.AutoPostBack = True
        Me.ddlKijun.ppDropDownList.AutoPostBack = True
        Me.txtSeq.ppTextBox.AutoPostBack = True
        Me.cbxFlg.AutoPostBack = True

        Dim pnl As UpdatePanel
        pnl = Master.FindControl("UpdPanelMain")
        pnl.UpdateMode = UpdatePanelUpdateMode.Always
        pnl = Master.FindControl("UpdPanelSearch")
        pnl.UpdateMode = UpdatePanelUpdateMode.Always

        If Not IsPostBack Then

            'プログラムＩＤ、画面名設定
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"

            'パンくずリスト設定
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            'ボタン押下時のメッセージ設定
            Master.ppBtnInsert.OnClientClick = pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '追加
            Master.ppBtnUpdate.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '更新

            'ドロップダウンリスト設定
            msSetddlAppaDvs("Search")
            msSetddlAppaDvs("Input")
            msSetddlAppaCls("Search")
            'msSetddlAppaCls("Input")
            msSetddlHDD()

            Me.ddlKijun.ppDropDownList.Items.Add(New ListItem("", ""))
            Me.ddlKijun.ppDropDownList.Items.Add(New ListItem("優先度", "0"))
            Me.ddlKijun.ppDropDownList.Items.Add(New ListItem("NL区分", "1"))

            'チェックボックスリスト設定
            msSetcklTboxType()

            ''活性制御
            'Me.pnlOther.Visible = True
            'Me.pnlSeigyo.Visible = False

            'Me.ddlAppaDvs.ppEnabled = True
            'Me.ddlAppaCls.ppEnabled = True
            'Me.pnlOther.Enabled = False
            'Me.pnlSeigyo.Enabled = False
            'Me.cklTboxType.Enabled = False

            'グリッドの初期化
            Me.grvList.DataSource = New DataTable
            grvList.DataBind()

            strMode = "Default"
            ddlsAppaDvs.ppDropDownList.Focus()

        End If

    End Sub

    ''' <summary>
    ''' Page PreRender
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        '活性制御
        msEnableCtrl(strMode)
        msEnableBtn(strMode)

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
        If (Page.IsValid) Then
            msGet_Data()
        End If

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

        ddlsAppaDvs.ppDropDownList.SelectedIndex = -1
        ddlsAppaCls.ppDropDownList.SelectedIndex = -1
        ddldel.ppDropDownList.SelectedIndex = -1

        ddlsAppaDvs.ppDropDownList.Focus()

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

        '削除ボタン押下時は検証しない
        If sender.Equals(Master.ppBtnDelete) Then
            msEditData(e.CommandName)
        Else

            msSetValidate()
            Page.Validate("val")
            If Page.IsValid Then
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

        '入力項目クリア
        msClearInput()

        strMode = "Default"
        SetFocus(ddlAppaDvs.ppDropDownList.ClientID)

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 機器分類変更(検索)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlsAppaDvs_SelectedIndexChanged(sender As Object, e As System.EventArgs)

        If ddlsAppaDvs.ppSelectedValue <> String.Empty Then
            '機器種別取得
            'msSetddlAppaCls("Search")
            SetFocus(ddlsAppaCls.ppDropDownList.ClientID)
        Else
            'Me.ddlsAppaCls.ppDropDownList.Items.Clear()
            SetFocus(ddlsAppaDvs.ppDropDownList.ClientID)
        End If

    End Sub

    ''' <summary>
    ''' 機器種別変更(検索)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlsAppaCls_SelectedIndexChanged(sender As Object, e As System.EventArgs)

        If ddlsAppaCls.ppSelectedValue <> String.Empty Then
            If ddlsAppaDvs.ppSelectedValue <> String.Empty Then
                'Page.Validate("key")
                'If Page.IsValid Then
                '    msGet_ExistData()
                'End If
            Else
                'Master.ppBtnDmy.Visible = True
                'SetFocus(Master.ppBtnDmy.ClientID)
            End If
        End If

    End Sub

    ''' <summary>
    ''' 機器分類変更
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlAppaDvs_SelectedIndexChanged(sender As Object, e As System.EventArgs)

        If ddlAppaDvs.ppSelectedValue <> String.Empty Then
            txtSeq.ppText = String.Empty

            '機器種別取得
            msSetddlAppaCls("Input")
            SetFocus(ddlAppaCls.ppDropDownList.ClientID)
        Else
            Me.ddlAppaCls.ppDropDownList.Items.Clear()
            txtSeq.ppText = String.Empty
            ddlAppaCls.ppDropDownList.SelectedIndex = -1
            SetFocus(ddlAppaDvs.ppDropDownList.ClientID)
        End If

    End Sub

    ''' <summary>
    ''' 機器種別変更
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlAppaCls_SelectedIndexChanged(sender As Object, e As System.EventArgs)

        If ddlAppaCls.ppSelectedValue <> String.Empty Then
            If ddlAppaDvs.ppSelectedValue <> String.Empty Then
                SetFocus(txtSeq.ppTextBox.ClientID)

                If ddlAppaCls.ppSelectedValue <> ApaCls_Ctr Then
                    pnlOther.Visible = True
                    pnlSeigyo.Visible = False
                Else
                    pnlOther.Visible = False
                    pnlSeigyo.Visible = True
                End If
                'Page.Validate("key")
                'If Page.IsValid Then
                '    'msGet_ExistData()
                'End If
            Else
                'Master.ppBtnDmy.Visible = True
                'SetFocus(Master.ppBtnDmy.ClientID)
            End If
        End If

    End Sub

    ''' <summary>
    ''' 基準変更
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlKijun_SelectedIndexChanged(sender As Object, e As System.EventArgs)

        Me.ddlHante.ppDropDownList.Items.Clear()

        If ddlKijun.ppSelectedValue <> String.Empty Then
            If ddlHante.ppDropDownList.Items.Count = 0 Then
                If ddlKijun.ppDropDownList.SelectedValue = "0" Then
                    Me.ddlHante.ppDropDownList.Items.Add(New ListItem("", ""))
                    Me.ddlHante.ppDropDownList.Items.Add(New ListItem("1", "1"))
                    Me.ddlHante.ppDropDownList.Items.Add(New ListItem("2", "2"))
                Else
                    Me.ddlHante.ppDropDownList.Items.Add(New ListItem("", ""))
                    Me.ddlHante.ppDropDownList.Items.Add(New ListItem("N", "N"))
                    Me.ddlHante.ppDropDownList.Items.Add(New ListItem("L", "L"))
                End If
            End If
        End If

    End Sub

    ''' <summary>
    ''' コード変更
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtSeq_TextChanged(sender As Object, e As System.EventArgs)
        If Master.ppBtnInsert.Enabled = True Then
            Master.ppBtnDmy.Visible = True
            If pnlOther.Enabled = True Then
                Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + ddlHddNo.ppDropDownList.ClientID + ");"
            Else
                Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + ddlKijun.ppDropDownList.ClientID + ");"
            End If

            SetFocus(Master.ppBtnDmy.ClientID)
            Exit Sub
        End If
        If txtSeq.ppText.Trim <> String.Empty Then
            txtSeq.ppValidationGroup = "key"
            txtSeq.ppText = mfGetNarrow(txtSeq.ppText.Trim)
            Dim i As Integer
            If Integer.TryParse(txtSeq.ppText, i) Then
                txtSeq.ppText = i.ToString
            End If

            If ddlAppaDvs.ppSelectedValue <> String.Empty AndAlso ddlAppaCls.ppSelectedValue <> String.Empty Then
                Page.Validate("key")
                If Page.IsValid Then
                    If Master.ppBtnInsert.Enabled = False Then
                        msGet_ExistData()
                    End If
                Else
                    Master.ppBtnDmy.Visible = True
                    Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtSeq.ppTextBox.ClientID + ");"
                    SetFocus(Master.ppBtnDmy.ClientID)
                End If
            Else
                If ddlAppaDvs.ppSelectedValue = String.Empty Then
                    ddlAppaDvs.ppDropDownList.Focus()
                Else
                    ddlAppaCls.ppDropDownList.Focus()
                End If
            End If
        Else
            Master.ppBtnDmy.Visible = True
            Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtSeq.ppTextBox.ClientID + ");"
            SetFocus(Master.ppBtnDmy.ClientID)
        End If
    End Sub

    ''' <summary>
    ''' チェックボックス変更
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cbxFlg_CheckedChanged(sender As Object, e As System.EventArgs)
        Dim objCtrl As CheckBox = DirectCast(sender, CheckBox)

        If objCtrl.Checked = False Then
            ddlHddNo.ppDropDownList.SelectedIndex = -1
            ddlHddCls.ppDropDownList.SelectedIndex = -1
            ddlKijun.ppDropDownList.SelectedIndex = -1
            ddlHante.ppDropDownList.SelectedIndex = -1
            txtSerStt.ppText = String.Empty
            txtSerStr.ppText = String.Empty
            cklTboxType.ClearSelection()
        End If

        strMode_other = ddlAppaCls.ppSelectedValue
        '元の状態(登録or更新)を[登録]ボタンの活性で判断する。
        If Master.ppBtnInsert.Enabled Then
            strMode = "Insert"
        Else
            strMode = "Select"
        End If

    End Sub

#End Region

#Region "Validation関係"

    'Private Function CodeLengthCheck() As Boolean
    '    CodeLengthCheck = False
    '    If txtCd.ppText.Length = 5 Then
    '        If ddlAppaDvs.ppDropDownList.SelectedValue = "4" AndAlso (ddlAppaDvs.ppSelectedValue = "1" OrElse ddlAppaDvs.ppSelectedValue = "9") Then
    '            CodeLengthCheck = True
    '        Else
    '            Me.txtCd.psSet_ErrorNo("3002", "コードは", "4")
    '            SetFocus(Me.txtCd.ppTextBox.ClientID)
    '            Exit Function
    '        End If
    '    ElseIf txtCd.ppText.Trim.Length = 0 Then
    '        Me.txtCd.psSet_ErrorNo("5001", "コード")
    '        SetFocus(Me.txtCd.ppTextBox.ClientID)
    '        Exit Function

    '    Else
    '        If ddlAppaDvs.ppDropDownList.SelectedValue = "4" AndAlso (ddlAppaDvs.ppSelectedValue = "1" OrElse ddlAppaDvs.ppSelectedValue = "9") Then
    '            Me.txtCd.psSet_ErrorNo("3001", "コードは", "5")
    '            SetFocus(Me.txtCd.ppTextBox.ClientID)
    '            Exit Function
    '        Else
    '            CodeLengthCheck = True
    '        End If
    '    End If
    'End Function

#End Region

#Region "GRID関係"

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
                arKey.Insert(0, CType(rowData.FindControl("機器分類コード"), TextBox).Text)
                arKey.Insert(0, CType(rowData.FindControl("機器種別コード"), TextBox).Text)
                arKey.Insert(0, CType(rowData.FindControl("連番"), TextBox).Text)

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

                '初期化処理
                btnClear_Click()

                '編集エリアに値を設定
                ddlAppaDvs.ppDropDownList.SelectedValue = DirectCast(rowData.FindControl("機器分類コード"), TextBox).Text
                '機器種別リスト生成
                msSetddlAppaCls("Input")
                ddlAppaCls.ppDropDownList.SelectedValue = DirectCast(rowData.FindControl("機器種別コード"), TextBox).Text
                txtSeq.ppText = DirectCast(rowData.FindControl("連番"), TextBox).Text
                ddlHddNo.ppDropDownList.SelectedValue = DirectCast(rowData.FindControl("HDDNo"), TextBox).Text
                ddlHddCls.ppDropDownList.SelectedValue = DirectCast(rowData.FindControl("HDD種別"), TextBox).Text
                txtAppaNm.ppText = DirectCast(rowData.FindControl("機器備考"), TextBox).Text
                If DirectCast(rowData.FindControl("優先順位"), TextBox).Text <> String.Empty Then
                    ddlKijun.ppDropDownList.SelectedValue = "0"
                    ddlHante.ppDropDownList.Items.Clear()
                    If ddlHante.ppDropDownList.Items.Count = 0 Then
                        Me.ddlHante.ppDropDownList.Items.Add(New ListItem("", ""))
                        Me.ddlHante.ppDropDownList.Items.Add(New ListItem("1", "1"))
                        Me.ddlHante.ppDropDownList.Items.Add(New ListItem("2", "2"))
                    End If
                    ddlHante.ppDropDownList.SelectedValue = DirectCast(rowData.FindControl("優先順位"), TextBox).Text
                Else
                    ddlKijun.ppDropDownList.SelectedValue = "1"
                    ddlHante.ppDropDownList.Items.Clear()
                    If ddlHante.ppDropDownList.Items.Count = 0 Then
                        Me.ddlHante.ppDropDownList.Items.Add(New ListItem("", ""))
                        Me.ddlHante.ppDropDownList.Items.Add(New ListItem("N", "N"))
                        Me.ddlHante.ppDropDownList.Items.Add(New ListItem("L", "L"))
                    End If
                    ddlHante.ppDropDownList.SelectedValue = DirectCast(rowData.FindControl("ＮＬ区分"), TextBox).Text
                End If
                txtSerStt.ppText = DirectCast(rowData.FindControl("シリアル開始位置"), TextBox).Text
                txtSerStr.ppText = DirectCast(rowData.FindControl("シリアル文字"), TextBox).Text

                Dim strData As String() = DirectCast(rowData.FindControl("TBOXタイプ"), TextBox).Text.Split(",")
                For zz As Integer = 0 To cklTboxType.Items.Count - 1
                    For yy As Integer = 0 To strData.Length - 1
                        If cklTboxType.Items.Item(zz).Text = strData(yy) Then
                            cklTboxType.Items.Item(zz).Selected = True
                        End If
                    Next
                Next
                If DirectCast(rowData.FindControl("管理区分"), TextBox).Text = "0" Then
                    cbxFlg.Checked = True
                Else
                    '判定基準を空に再設定します
                    ddlKijun.ppSelectedValue = String.Empty
                    cbxFlg.Checked = False
                End If
                If DirectCast(rowData.FindControl("削除"), TextBox).Text = "0" Then
                    Master.ppBtnDelete.Text = "削除"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '削除
                Else
                    Master.ppBtnDelete.Text = "削除取消"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
                End If

                strMode = "Select"
                strMode_other = DirectCast(rowData.FindControl("機器種別コード"), TextBox).Text

                '不正入力との競合時、エラーサマリーが残る事象を防ぐ
                Page.Validate("key")
                '既存データ取得処理と競合時、ダミーボタンが残る事象を防ぐ
                Master.ppBtnDmy.Visible = False
                'フォーカス設定
                Master.ppBtnDmy.Visible = True
                Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtAppaNm.ppTextBox.ClientID + ");"
                SetFocus(Master.ppBtnDmy.ClientID)
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
    ''' GRID DataBound
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        'ヘッダテキスト設定
        'Dim strHeader As String() = New String() {"選択", "業者区分", "業者区分", "分類区分", "分類区分", "業者名", "業者名", "郵便番号", "都道府県",
        '                                          "都道府県", "住所", "代表電話番号", "連絡電話番号", "FAX番号", "削除", "住所1", "住所2", "住所3",
        '                                          "代表TEL1", "代表TEL2", "代表TEL3", "FAX1", "FAX2", "FAX3", "連絡TEL1", "連絡TEL2", "連絡TEL3"}

        Try
            'If Not IsPostBack Then
            '    For clm As Integer = 1 To strHeader.Count - 1
            '        grvList.Columns(clm).HeaderText = strHeader(clm)
            '    Next
            'End If

            For Each rowData As GridViewRow In grvList.Rows
                '削除フラグ１の時該当行赤字化
                If CType(rowData.FindControl("削除"), TextBox).Text.Trim = "1" Then
                    CType(rowData.FindControl("機器分類コード"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("機器分類名称"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("機器種別コード"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("機器種別名称"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("連番"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("機器備考"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("持参物品管理"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("優先順位"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("ＮＬ区分"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("シリアル開始位置"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("シリアル文字"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("HDDNo"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("HDD種別"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("TBOXタイプ"), TextBox).ForeColor = Drawing.Color.Red
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
                .Add(pfSet_Param("AppaDvs", SqlDbType.NVarChar, ddlsAppaDvs.ppSelectedValue))
                .Add(pfSet_Param("AppaCls", SqlDbType.NVarChar, ddlsAppaCls.ppSelectedValue))
                .Add(pfSet_Param("DeleteFlg", SqlDbType.NVarChar, ddldel.ppSelectedValue))
                .Add(pfSet_Param("Prog_id", SqlDbType.NVarChar, DispCode))
                .Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, "0"))
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

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle.ToString)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            mclsDB.psDB_Close()
        End Try

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
                    .Add(pfSet_Param("AppaDvs", SqlDbType.NVarChar, ddlAppaDvs.ppSelectedValue))
                    .Add(pfSet_Param("AppaCls", SqlDbType.NVarChar, ddlAppaCls.ppSelectedValue))
                    .Add(pfSet_Param("Seq", SqlDbType.NVarChar, txtSeq.ppText.Trim))
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
                    arKey.Insert(0, ddlAppaDvs.ppSelectedValue)
                    arKey.Insert(0, ddlAppaCls.ppSelectedValue)
                    'arKey.Insert(0, txtCd.ppText.Trim)

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

                    '初期化処理
                    btnClear_Click()

                    '編集エリアに値を設定
                    ddlAppaDvs.ppDropDownList.SelectedValue = dtsDB.Tables(0).Rows(0).Item("機器分類コード").ToString
                    '機器種別リスト生成
                    msSetddlAppaCls("Input")
                    ddlAppaCls.ppDropDownList.SelectedValue = dtsDB.Tables(0).Rows(0).Item("機器種別コード").ToString
                    txtSeq.ppText = dtsDB.Tables(0).Rows(0).Item("連番").ToString
                    ddlHddNo.ppDropDownList.SelectedValue = dtsDB.Tables(0).Rows(0).Item("HDDNo").ToString
                    ddlHddCls.ppDropDownList.SelectedValue = dtsDB.Tables(0).Rows(0).Item("HDD種別").ToString
                    txtAppaNm.ppText = dtsDB.Tables(0).Rows(0).Item("機器備考").ToString

                    If dtsDB.Tables(0).Rows(0).Item("優先順位").ToString <> String.Empty Then
                        ddlKijun.ppDropDownList.SelectedValue = "0"
                        ddlHante.ppDropDownList.Items.Clear()
                        If ddlHante.ppDropDownList.Items.Count = 0 Then
                            Me.ddlHante.ppDropDownList.Items.Add(New ListItem("", ""))
                            Me.ddlHante.ppDropDownList.Items.Add(New ListItem("1", "1"))
                            Me.ddlHante.ppDropDownList.Items.Add(New ListItem("2", "2"))
                        End If
                        ddlHante.ppDropDownList.SelectedValue = dtsDB.Tables(0).Rows(0).Item("優先順位").ToString
                    Else
                        ddlKijun.ppDropDownList.SelectedValue = "1"
                        ddlHante.ppDropDownList.Items.Clear()
                        If ddlHante.ppDropDownList.Items.Count = 0 Then
                            Me.ddlHante.ppDropDownList.Items.Add(New ListItem("", ""))
                            Me.ddlHante.ppDropDownList.Items.Add(New ListItem("N", "N"))
                            Me.ddlHante.ppDropDownList.Items.Add(New ListItem("L", "L"))
                        End If
                        ddlHante.ppDropDownList.SelectedValue = dtsDB.Tables(0).Rows(0).Item("ＮＬ区分").ToString
                    End If
                    txtSerStt.ppText = dtsDB.Tables(0).Rows(0).Item("シリアル開始位置").ToString
                    txtSerStr.ppText = dtsDB.Tables(0).Rows(0).Item("シリアル文字").ToString

                    'チェックを付ける
                    For zz As Integer = 0 To cklTboxType.Items.Count - 1
                        For yy As Integer = 0 To dtsDB.Tables(0).Rows.Count - 1
                            If cklTboxType.Items.Item(zz).Value = dtsDB.Tables(0).Rows(yy).Item("システムコード").ToString Then
                                cklTboxType.Items.Item(zz).Selected = True
                            End If
                        Next
                    Next

                    '持参物品管理区分の確認
                    If dtsDB.Tables(0).Rows(0).Item("管理区分").ToString.Trim = "0" Then '管理する
                        cbxFlg.Checked = True
                    Else
                        cbxFlg.Checked = False
                        '判定基準を空に再設定します
                        ddlKijun.ppSelectedValue = String.Empty
                    End If

                    '削除フラグ確認
                    If dtsDB.Tables(0).Rows(0).Item("削除").ToString = "0" Then
                        Master.ppBtnDelete.Text = "削除"
                        Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)
                    Else
                        Master.ppBtnDelete.Text = "削除取消"
                        Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")
                    End If

                    '状態設定
                    strMode = "Update"
                    strMode_other = dtsDB.Tables(0).Rows(0).Item("機器種別コード").ToString
                Else
                    '状態設定
                    strMode = "Insert"
                    strMode_other = ddlAppaCls.ppDropDownList.SelectedValue
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
        If strMode = "Insert" OrElse strMode = "Update" Then
            'If strMode_other = ApaCls_Ctr Then
            '    afFocusID = ddlKijun.ppDropDownList.ClientID
            'Else
            '    If strMode_other = ApaCls_Hdd Then
            '        afFocusID = ddlHddNo.ppDropDownList.ClientID
            '    Else
            '        afFocusID = txtAppaNm.ppTextBox.ClientID
            '    End If
            'End If
            afFocusID = txtAppaNm.ppTextBox.ClientID
        Else
            afFocusID = ddlAppaDvs.ppDropDownList.ClientID
        End If

        Master.ppBtnDmy.Visible = True
        Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + afFocusID + ");"
        SetFocus(Master.ppBtnDmy.ClientID)

    End Sub

    ''' <summary>
    ''' 既存データ存在確認
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfIsExistData() As Boolean

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
                    .Add(pfSet_Param("AppaDvs", SqlDbType.NVarChar, ddlAppaDvs.ppSelectedValue))
                    .Add(pfSet_Param("AppaCls", SqlDbType.NVarChar, ddlAppaCls.ppSelectedValue))
                    .Add(pfSet_Param("Seq", SqlDbType.NVarChar, txtSeq.ppText.Trim))
                End With

                'リストデータ取得
                dtsDB = clsDataConnect.pfGet_DataSet(cmdDB)

                '既にデータが存在していた場合は各項目に設定
                If dtsDB.Tables(0).Rows.Count > 0 Then
                    mfIsExistData = True
                Else
                    mfIsExistData = False
                End If

            Catch ex As Exception
                mfIsExistData = True 'キャッチに入ったら存在するものとして扱う
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
            mfIsExistData = True '接続エラーは存在するものとして扱う
            'エラー
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If
    End Function

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
        Dim strStored_Detail As String = String.Empty
        Dim getFlg As String = "0"
        Dim trans As SqlClient.SqlTransaction             'トランザクション
        objStack = New StackFrame
        Dim intRtn As Integer
        Dim intRtn2 As Integer
        Dim intRtn3 As Integer
        Dim drData As DataRow

        Select Case ipstrMode
            Case "INSERT"
                MesCode = "00003"
                strStored = DispCode & "_U1"
                strStored_Detail = DispCode & "_U2"
            Case "UPDATE"
                MesCode = "00001"
                strStored = DispCode & "_U1"
                strStored_Detail = DispCode & "_U2"
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

        Try
            If clsDataConnect.pfOpen_Database(conDB) Then

                Dim dtsDB As New DataSet
                cmdDB = New SqlCommand(DispCode & "_S5", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("apa_dvs", SqlDbType.NVarChar, ddlAppaDvs.ppSelectedValue))
                    .Add(pfSet_Param("apa_cls", SqlDbType.NVarChar, ddlAppaCls.ppSelectedValue))
                    .Add(pfSet_Param("seq_num", SqlDbType.NVarChar, txtSeq.ppText.Trim))
                End With
                'リストデータ取得
                dtsDB = clsDataConnect.pfGet_DataSet(cmdDB)


                'トランザクションの設定
                trans = conDB.BeginTransaction
                cmdDB = New SqlCommand(strStored, conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.Transaction = trans
                Select Case ipstrMode
                    Case "DELETE"
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("apa_dvs", SqlDbType.NVarChar, ddlAppaDvs.ppSelectedValue))
                            .Add(pfSet_Param("apa_cls", SqlDbType.NVarChar, ddlAppaCls.ppSelectedValue))
                            .Add(pfSet_Param("seq_num", SqlDbType.NVarChar, txtSeq.ppText.Trim))
                            .Add(pfSet_Param("user_id", SqlDbType.NVarChar, Session(P_SESSION_USERID)))  'ユーザーID
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
                        End With

                    Case Else
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("apa_dvs", SqlDbType.NVarChar, ddlAppaDvs.ppSelectedValue))
                            .Add(pfSet_Param("apa_cls", SqlDbType.NVarChar, ddlAppaCls.ppSelectedValue))
                            .Add(pfSet_Param("seq_num", SqlDbType.NVarChar, txtSeq.ppText.Trim))
                            .Add(pfSet_Param("cnd_nam", SqlDbType.NVarChar, txtAppaNm.ppText))
                            If ddlKijun.ppDropDownList.SelectedValue = "0" Then
                                '優先度
                                .Add(pfSet_Param("ord_pri", SqlDbType.NVarChar, ddlHante.ppDropDownList.SelectedValue))
                                .Add(pfSet_Param("nlj_dvs", SqlDbType.NVarChar, String.Empty))
                            Else
                                'NL区分
                                .Add(pfSet_Param("ord_pri", SqlDbType.NVarChar, String.Empty))
                                .Add(pfSet_Param("nlj_dvs", SqlDbType.NVarChar, ddlHante.ppDropDownList.SelectedValue))
                            End If
                            .Add(pfSet_Param("ser_stt", SqlDbType.NVarChar, txtSerStt.ppText))
                            .Add(pfSet_Param("ser_str", SqlDbType.NVarChar, txtSerStr.ppText))
                            .Add(pfSet_Param("hdd_no", SqlDbType.NVarChar, ddlHddNo.ppDropDownList.SelectedValue))
                            .Add(pfSet_Param("hdd_cls", SqlDbType.NVarChar, ddlHddCls.ppDropDownList.SelectedValue))
                            If cbxFlg.Checked = True Then
                                .Add(pfSet_Param("brg_flg", SqlDbType.NVarChar, "0"))
                            Else
                                .Add(pfSet_Param("brg_flg", SqlDbType.NVarChar, "1"))
                            End If
                            .Add(pfSet_Param("del_dvs", SqlDbType.NVarChar, "0"))
                            If ipstrMode = "INSERT" Then
                                .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, "0"))
                            Else
                                .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, "1"))
                            End If
                            .Add(pfSet_Param("user_id", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
                        End With

                End Select
                cmdDB.ExecuteNonQuery()
                'ストアド戻り値チェック
                intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)


                '更新時、明細DELETE
                Select Case ipstrMode
                    Case "UPDATE"
                        cmdDB = New SqlCommand(DispCode & "_D2", conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = trans
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("apa_dvs", SqlDbType.NVarChar, ddlAppaDvs.ppSelectedValue))
                            .Add(pfSet_Param("apa_cls", SqlDbType.NVarChar, ddlAppaCls.ppSelectedValue))
                            .Add(pfSet_Param("seq_num", SqlDbType.NVarChar, txtSeq.ppText.Trim))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With
                        cmdDB.ExecuteNonQuery()
                        'ストアド戻り値チェック
                        intRtn2 = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                End Select

                '登録/更新時、明細INSERT
                Select Case ipstrMode
                    Case "INSERT", "UPDATE"
                        cmdDB = New SqlCommand(strStored_Detail, conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = trans
                        For zz As Integer = 0 To cklTboxType.Items.Count - 1
                            If cklTboxType.Items.Item(zz).Selected = True Then
                                With cmdDB.Parameters
                                    .Add(pfSet_Param("apa_dvs", SqlDbType.NVarChar, ddlAppaDvs.ppSelectedValue))
                                    .Add(pfSet_Param("apa_cls", SqlDbType.NVarChar, ddlAppaCls.ppSelectedValue))
                                    .Add(pfSet_Param("seq_num", SqlDbType.NVarChar, txtSeq.ppText.Trim))
                                    .Add(pfSet_Param("sys_cod", SqlDbType.NVarChar, cklTboxType.Items.Item(zz).Value))
                                    .Add(pfSet_Param("del_dvs", SqlDbType.NVarChar, "0"))
                                    If ipstrMode = "INSERT" OrElse dtsDB.Tables(0).Rows.Count = 0 Then
                                        .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, "0"))
                                        .Add(pfSet_Param("ins_usr", SqlDbType.NVarChar, String.Empty))
                                        .Add(pfSet_Param("ins_ymd", SqlDbType.NVarChar, DBNull.Value))
                                    Else
                                        .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, "1"))
                                        .Add(pfSet_Param("ins_usr", SqlDbType.NVarChar, dtsDB.Tables(0).Rows(0).Item("登録者").ToString))
                                        .Add(pfSet_Param("ins_ymd", SqlDbType.NVarChar, dtsDB.Tables(0).Rows(0).Item("登録日").ToString))
                                    End If
                                    .Add(pfSet_Param("user_id", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
                                End With
                                cmdDB.ExecuteNonQuery()
                                'ストアド戻り値チェック
                                intRtn3 = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                                cmdDB.Parameters.Clear()
                            End If
                            If intRtn3 <> 0 Then
                                Exit For
                            End If
                        Next
                End Select


                '成功・失敗判定
                If intRtn <> 0 Or intRtn2 <> 0 Or intRtn3 <> 0 Then
                    trans.Rollback()
                    psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                    Exit Sub
                End If
                trans.Commit()

                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName)
                Try
                    If ipstrMode = "INSERT" OrElse ipstrMode = "UPDATE" OrElse Master.ppBtnDelete.Text = "削除取消" Then

                        '登録/更新の場合は対象レコードのみ表示
                        '表示用のデータテーブルを作成
                        dttGrid.Columns.Add("機器分類コード")
                        dttGrid.Columns.Add("機器分類名称")
                        dttGrid.Columns.Add("機器種別コード")
                        dttGrid.Columns.Add("機器種別名称")
                        dttGrid.Columns.Add("連番")
                        dttGrid.Columns.Add("機器備考")
                        dttGrid.Columns.Add("持参物品管理")
                        dttGrid.Columns.Add("管理区分")
                        dttGrid.Columns.Add("優先順位")
                        dttGrid.Columns.Add("ＮＬ区分")
                        dttGrid.Columns.Add("シリアル開始位置")
                        dttGrid.Columns.Add("シリアル文字")
                        dttGrid.Columns.Add("HDDNo")
                        dttGrid.Columns.Add("HDD種別")
                        dttGrid.Columns.Add("TBOXタイプ")
                        dttGrid.Columns.Add("削除")
                        '表示用の行データを作成、データテーブルに登録
                        drData = dttGrid.NewRow()
                        drData.Item("機器分類コード") = ddlAppaDvs.ppDropDownList.SelectedItem.ToString.Split(":")(0)
                        drData.Item("機器分類名称") = ddlAppaDvs.ppDropDownList.SelectedItem.ToString.Split(":")(1)
                        drData.Item("機器種別コード") = ddlAppaCls.ppDropDownList.SelectedItem.ToString.Split(":")(0)
                        drData.Item("機器種別名称") = ddlAppaCls.ppDropDownList.SelectedItem.ToString.Split(":")(1)
                        drData.Item("連番") = txtSeq.ppText.Trim
                        drData.Item("機器備考") = txtAppaNm.ppText.Trim
                        If cbxFlg.Checked Then
                            drData.Item("持参物品管理") = "持参物品管理する"
                            drData.Item("管理区分") = "0"
                        Else
                            drData.Item("持参物品管理") = String.Empty
                            drData.Item("管理区分") = "1"
                        End If

                        If ddlKijun.ppDropDownList.SelectedValue = "0" Then
                            '優先度
                            drData.Item("優先順位") = ddlHante.ppDropDownList.SelectedValue
                            drData.Item("ＮＬ区分") = String.Empty
                        Else
                            'NL区分
                            drData.Item("優先順位") = String.Empty
                            drData.Item("ＮＬ区分") = ddlHante.ppDropDownList.SelectedValue
                        End If
                        drData.Item("シリアル開始位置") = txtSerStt.ppText.Trim
                        drData.Item("シリアル文字") = txtSerStr.ppText
                        drData.Item("HDDNo") = ddlHddNo.ppDropDownList.SelectedValue
                        drData.Item("HDD種別") = ddlHddCls.ppDropDownList.SelectedValue

                        'チェックボックスの値を設定
                        Dim strData As String = String.Empty
                        For zz As Integer = 0 To cklTboxType.Items.Count - 1
                            If cklTboxType.Items.Item(zz).Selected = True Then
                                If strData = String.Empty Then
                                    strData = cklTboxType.Items.Item(zz).Text
                                Else
                                    strData = strData & "," & cklTboxType.Items.Item(zz).Text
                                End If
                            End If
                        Next
                        drData.Item("TBOXタイプ") = strData

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

                    '初期化処理
                    btnClear_Click()

                Catch ex As Exception
                    psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                End Try
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
    ''' ドロップダウンリスト機器分類取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlAppaDvs(ByVal strSwitchFlg As String)
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

                ''01：TBOXのみ表示する
                'Dim objDv = clsDataConnect.pfGet_DataSet(objCmd).Tables(0).DefaultView
                'objDv.RowFilter = "機器分類コード = 01"
                'objDt = objDv.ToTable
                objDt = clsDataConnect.pfGet_DataSet(objCmd).Tables(0)

                'ドロップダウンリスト設定
                Select Case strSwitchFlg
                    Case "Search"
                        '検索
                        Me.ddlsAppaDvs.ppDropDownList.Items.Clear()
                        Me.ddlsAppaDvs.ppDropDownList.DataSource = objDt
                        Me.ddlsAppaDvs.ppDropDownList.DataTextField = "名称"
                        Me.ddlsAppaDvs.ppDropDownList.DataValueField = "機器分類コード"
                        Me.ddlsAppaDvs.ppDropDownList.DataBind()
                        Me.ddlsAppaDvs.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

                    Case "Input"
                        Me.ddlAppaDvs.ppDropDownList.Items.Clear()
                        Me.ddlAppaDvs.ppDropDownList.DataSource = objDt
                        Me.ddlAppaDvs.ppDropDownList.DataTextField = "名称"
                        Me.ddlAppaDvs.ppDropDownList.DataValueField = "機器分類コード"
                        Me.ddlAppaDvs.ppDropDownList.DataBind()
                        Me.ddlAppaDvs.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

                End Select

                'msSetddlAppaCls(strSwitchFlg)

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
    Private Sub msSetddlAppaCls(ByVal strSwitchFlg As String)
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
                    .Add(pfSet_Param("appadivcd", SqlDbType.NVarChar, Me.ddlAppaDvs.ppSelectedValue))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                'For zz As Integer = 0 To objDs.Tables(0).Rows.Count - 1
                '    Select Case objDs.Tables(0).Rows(zz).Item("機器種別コード").ToString
                '        Case ApaCls_Ctr, "02", "03", "04", "05", ApaCls_Hdd
                '        Case Else
                '            objDs.Tables(0).Rows(zz).Delete()
                '    End Select
                'Next
                'objDs.Tables(0).AcceptChanges()

                'ドロップダウンリスト設定
                Select Case strSwitchFlg
                    Case "Search"
                        '検索
                        Me.ddlsAppaCls.ppDropDownList.Items.Clear()
                        Me.ddlsAppaCls.ppDropDownList.DataSource = objDs.Tables(0)
                        Me.ddlsAppaCls.ppDropDownList.DataTextField = "機器種別名"
                        Me.ddlsAppaCls.ppDropDownList.DataValueField = "機器種別コード"
                        Me.ddlsAppaCls.ppDropDownList.DataBind()
                        Me.ddlsAppaCls.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

                    Case "Input"
                        Me.ddlAppaCls.ppDropDownList.Items.Clear()
                        Me.ddlAppaCls.ppDropDownList.DataSource = objDs.Tables(0)
                        Me.ddlAppaCls.ppDropDownList.DataTextField = "機器種別名"
                        Me.ddlAppaCls.ppDropDownList.DataValueField = "機器種別コード"
                        Me.ddlAppaCls.ppDropDownList.DataBind()
                        Me.ddlAppaCls.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

                End Select

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
                    '.Add(pfSet_Param("Sys_Cod", SqlDbType.NVarChar, ddlSys.ppSelectedValue))
                End With
                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                '■HDDNo
                Me.ddlHddNo.ppDropDownList.Items.Clear()
                Me.ddlHddNo.ppDropDownList.DataSource = objDs.Tables(0)
                Me.ddlHddNo.ppDropDownList.DataTextField = "HDDNo"
                Me.ddlHddNo.ppDropDownList.DataValueField = "HDDNo"
                Me.ddlHddNo.ppDropDownList.DataBind()
                Me.ddlHddNo.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                '■HDD種別
                Me.ddlHddCls.ppDropDownList.Items.Clear()
                Me.ddlHddCls.ppDropDownList.DataSource = objDs.Tables(1)
                Me.ddlHddCls.ppDropDownList.DataTextField = "HDD種別"
                Me.ddlHddCls.ppDropDownList.DataValueField = "HDD種別"
                Me.ddlHddCls.ppDropDownList.DataBind()
                If Me.ddlHddCls.ppDropDownList.Items.FindByValue(String.Empty) Is Nothing Then
                    Me.ddlHddCls.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing))
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
    ''' TBOXタイプチェックボックスリストの作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetcklTboxType()
        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        Dim intCkl As Integer = 0
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(DispCode & "_S4", objCn)
                'objCmd.Parameters.Add(pfSet_Param("system", SqlDbType.NVarChar, ddlSystem.SelectedValue))
                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                Me.cklTboxType.Items.Clear()
                Me.cklTboxType.DataSource = objDs.Tables(0)
                Me.cklTboxType.DataTextField = "システム名称"
                Me.cklTboxType.DataValueField = "システムコード"
                Me.cklTboxType.DataBind()
                objCmd.Dispose()
                objDs.Dispose()

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "許容バージョン一覧取得")
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
    ''' 入力項目クリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearInput()

        ddlAppaDvs.ppDropDownList.SelectedIndex = -1

        Me.ddlAppaCls.ppDropDownList.Items.Clear()
        ddlAppaCls.ppDropDownList.SelectedIndex = -1
        txtSeq.ppText = String.Empty
        txtAppaNm.ppText = String.Empty
        cbxFlg.Checked = False

        ddlHddNo.ppDropDownList.SelectedIndex = -1
        ddlHddCls.ppDropDownList.SelectedIndex = -1
        ddlKijun.ppDropDownList.SelectedIndex = -1
        ddlHante.ppDropDownList.SelectedIndex = -1
        txtSerStt.ppText = String.Empty
        txtSerStr.ppText = String.Empty
        cklTboxType.ClearSelection()

    End Sub

    ''' <summary>
    ''' 入力エリアボタン活性制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msEnableBtn(ByVal strMode As String)

        '活性制御を指定していない場合は処理終了する
        If strMode = Nothing Then
            Exit Sub
        End If

        Master.ppBtnInsert.Enabled = False
        Master.ppBtnUpdate.Enabled = False
        Master.ppBtnDelete.Enabled = False
        Master.ppBtnClear.Enabled = True
        Master.ppMainEnabled = True

        Select Case strMode
            Case "Default"
                Master.ppBtnDelete.Text = "削除"
                mpKeyInpEnable = True
                mpMainInpEnable = False

            Case "Select", "Update"
                Master.ppBtnUpdate.Enabled = True
                Master.ppBtnDelete.Enabled = True
                If Master.ppBtnDelete.Text = "削除取消" Then
                    Master.ppBtnUpdate.Enabled = False
                    Master.ppMainEnabled = False
                Else
                    Master.ppBtnUpdate.Enabled = True
                End If
                mpKeyInpEnable = False
                mpMainInpEnable = True

            Case "Insert"
                Master.ppBtnInsert.Enabled = True
                Master.ppBtnDelete.Text = "削除"
                mpKeyInpEnable = False
                mpMainInpEnable = True
        End Select

    End Sub

    ''' <summary>
    ''' 入力エリア活性制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msEnableCtrl(ByVal strMode As String)

        '活性制御を指定していない場合は処理終了する
        If strMode = Nothing Then
            Exit Sub
        End If

        Master.ppBtnInsert.Enabled = False     '追加
        Master.ppBtnUpdate.Enabled = False     '更新
        Master.ppBtnDelete.Enabled = False     '削除
        Master.ppBtnClear.Enabled = True       'クリア
        Master.ppMainEnabled = True

        Select Case strMode
            Case "Default"
                ddlAppaDvs.ppDropDownList.Enabled = True
                ddlAppaCls.ppDropDownList.Enabled = True
                txtSeq.ppTextBox.Enabled = True

                pnlName.Visible = True
                txtAppaNm.ppTextBox.Enabled = False
                cbxFlg.Enabled = False

                pnlOther.Visible = True
                ddlHddNo.ppEnabled = False
                ddlHddCls.ppEnabled = False

                pnlSeigyo.Visible = False
                ddlKijun.ppDropDownList.Enabled = False
                ddlHante.ppDropDownList.Enabled = False
                txtSerStt.ppTextBox.Enabled = False
                txtSerStr.ppTextBox.Enabled = False

                cklTboxType.Enabled = False

            Case "Select", "Update", "Insert"
                ddlAppaDvs.ppDropDownList.Enabled = False
                ddlAppaCls.ppDropDownList.Enabled = False
                txtSeq.ppTextBox.Enabled = False

                pnlName.Visible = True
                txtAppaNm.ppTextBox.Enabled = True
                cbxFlg.Enabled = True

                If strMode_other <> ApaCls_Ctr Then
                    '制御部以外の場合
                    pnlOther.Visible = True
                    If cbxFlg.Checked = True Then
                        If strMode_other = ApaCls_Hdd Then
                            ddlHddNo.ppEnabled = True
                            ddlHddCls.ppEnabled = True
                        Else
                            ddlHddNo.ppEnabled = False
                            ddlHddCls.ppEnabled = False
                        End If
                    Else
                        ddlHddNo.ppEnabled = False
                        ddlHddCls.ppEnabled = False
                    End If

                    pnlSeigyo.Visible = False
                    ddlKijun.ppDropDownList.Enabled = False
                    ddlHante.ppDropDownList.Enabled = False
                    txtSerStt.ppTextBox.Enabled = False
                    txtSerStr.ppTextBox.Enabled = False
                Else
                    '制御部の場合
                    pnlOther.Visible = False
                    ddlHddNo.ppEnabled = False
                    ddlHddCls.ppEnabled = False

                    pnlSeigyo.Visible = True
                    If cbxFlg.Checked = True Then
                        ddlKijun.ppDropDownList.Enabled = True
                        ddlHante.ppDropDownList.Enabled = True
                        txtSerStt.ppTextBox.Enabled = True
                        txtSerStr.ppTextBox.Enabled = True
                    Else
                        ddlKijun.ppDropDownList.Enabled = False
                        ddlHante.ppDropDownList.Enabled = False
                        txtSerStt.ppTextBox.Enabled = False
                        txtSerStr.ppTextBox.Enabled = False
                    End If

                End If

                'If cbxFlg.Checked = True Then
                '    cklTboxType.Enabled = True
                'Else
                '    cklTboxType.Enabled = False
                'End If
                cklTboxType.Enabled = True

                'Case "Insert"
                '    ddlAppaDvs.ppDropDownList.Enabled = False
                '    ddlAppaCls.ppDropDownList.Enabled = False
                '    txtSeq.ppTextBox.Enabled = False

                '    pnlOther.Visible = True
                '    ddlHddNo.ppEnabled = True
                '    ddlHddCls.ppEnabled = True
                '    txtAppaNm.ppTextBox.Enabled = True

                '    pnlSeigyo.Visible = False
                '    ddlKijun.ppDropDownList.Enabled = False
                '    ddlHante.ppDropDownList.Enabled = False
                '    txtSerStt.ppTextBox.Enabled = False
                '    txtSerStr.ppTextBox.Enabled = False

                '    cklTboxType.Enabled = True
        End Select

    End Sub

    ''' <summary>
    ''' 番号がオールゼロか空欄なら空文字を返す(ﾊｲﾌﾝ許容)
    ''' </summary>
    ''' <param name="strTelNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfTelNo_Check(ByVal strTelNo As String) As String
        Dim intTemp As Integer
        mfTelNo_Check = strTelNo
        strTelNo = strTelNo.Trim("-")

        If Integer.TryParse(strTelNo.Trim, intTemp) AndAlso intTemp = 0 Then
            mfTelNo_Check = String.Empty
        ElseIf strTelNo = String.Empty Then
            mfTelNo_Check = String.Empty
        End If
    End Function

    ''' <summary>
    ''' 文字列を半角に変換する
    ''' </summary>
    ''' <param name="strCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetNarrow(ByVal strCode) As String
        mfGetNarrow = Microsoft.VisualBasic.StrConv(strCode, Microsoft.VisualBasic.VbStrConv.Narrow)
    End Function

    ''' <summary>
    ''' 空欄登録の許容項目を設定する(入力チェック準備)
    ''' </summary>
    ''' <remarks>Page.Validete("val")の呼び出し前に実行して下さい。
    ''' このメソッド実行後は、持参管理しない機器備考の登録時に各項目の空欄登録が許可されます。</remarks>
    Private Sub msSetValidate()
        '入力必須有無を保存する変数
        '既定値をTrue(入力必須)として、持参管理しない場合にFalse(空欄許容)にする
        Dim blnReq As Boolean = True

        '機器備考名称のみ別条件
        '制御部かつ持参管理時の場合、空欄登録／更新を許容
        If ddlAppaCls.ppSelectedValue = "01" And cbxFlg.Checked Then
            txtAppaNm.ppRequiredField = False
        End If

        'その他入力項目
        '持参物品管理しない場合、入力項目の空欄を許容
        If cbxFlg.Checked = False Then
            blnReq = False
        End If
        ddlHddNo.ppRequiredField = blnReq
        'HDD種別は常に未入力許可
        'ddlHddCls.ppRequiredField = blnReq
        ddlKijun.ppRequiredField = blnReq
        ddlHante.ppRequiredField = blnReq
        txtSerStt.ppRequiredField = blnReq
        txtSerStr.ppRequiredField = blnReq
        cus_TboxType.Enabled = blnReq

    End Sub

    ''' <summary>
    ''' ＨＤＤマスタ存在チェック
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfExistHDDData() As Boolean
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim dtRow As DataRow = Nothing          'DataRow

        objStack = New StackFrame

        mfExistHDDData = False

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(DispCode + "_S6", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    'HDD No.
                    .Add(pfSet_Param("hdd_no", SqlDbType.NVarChar, ddlHddNo.ppSelectedValue))
                    'HDD 種別
                    .Add(pfSet_Param("hdd_cls", SqlDbType.NVarChar, ddlHddCls.ppSelectedValue))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables(0).Rows.Count = 0 Then
                    Exit Function
                End If

                mfExistHDDData = True

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＨＤＤマスタ情報取得")
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

    End Function

    ''' <summary>
    ''' HDD No Validation
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks>セットNoもしくは台数が"0"か"00"なら
    ''' 　　　　 検証エラーとしてエラーサマリーを表示する。</remarks>
    Private Sub cuv_Hdd_No_ServerValidate(source As Object, args As ServerValidateEventArgs)

        'Const Name As String = "HDD No"
        If ddlHddNo.ppDropDownList.SelectedIndex <> 0 Then
            If mfExistHDDData() = False Then
                source.ErrorMessage = pfGet_ValMes("2002", "選択したHDD No.とHDD種別の組み合わせ").Item("Message").ToString
                source.Text = "整合性エラー"
                args.IsValid = False
                Exit Sub
            End If
        End If

    End Sub

    ''' <summary>
    ''' HDD種別 Validation
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks>セットNoもしくは台数が"0"か"00"なら
    ''' 　　　　 検証エラーとしてエラーサマリーを表示する。</remarks>
    Private Sub cuv_Hdd_Cls_ServerValidate(source As Object, args As ServerValidateEventArgs)

        'Const Name As String = "HDD種別"

        'If tmbStartTm1.ppHourText = String.Empty AndAlso tmbStartTm1.ppMinText = String.Empty Then
        '    If (txtPrice1_b.Text <> String.Empty AndAlso txtPrice1_b.Text <> "0") Or (tmbEndTm1.ppHourText <> String.Empty AndAlso tmbEndTm1.ppMinText <> String.Empty) Then
        '        source.ErrorMessage = "" & Name & "に値が設定されていません。"
        '        source.Text = "未入力エラー"
        '        args.IsValid = False
        '        Exit Sub
        '    End If
        'Else
        '    If tmbEndTm2.ppHourText <> String.Empty AndAlso tmbEndTm2.ppMinText <> String.Empty Then
        '        If tmbStartTm1.ppHourText & tmbStartTm1.ppMinText <> tmbEndTm2.ppHourText & tmbEndTm2.ppMinText Then
        '            source.ErrorMessage = "特別料金 終了時刻と同じ値を設定してください。"
        '            source.Text = "整合性エラー"
        '            args.IsValid = False
        '            Exit Sub
        '        End If
        '    End If
        'End If
    End Sub

    ''' <summary>
    ''' TBOXタイプチェックボックス Validation
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub cus_TboxType_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cus_TboxType.ServerValidate

        'Dim ckl As CheckBoxList = CType(Master.FindControl("MainContent").FindControl(source.ControlToValidate), CheckBoxList)
        Const Name As String = "TBOXタイプ"

        Dim blnError As Boolean = True 'True:エラー有り、False:エラー無し

        '必須チェック
        For zz As Integer = 0 To cklTboxType.Items.Count - 1
            If cklTboxType.Items.Item(zz).Selected = True Then
                blnError = False
                Exit For
            End If
        Next
        If blnError = True Then
            source.ErrorMessage = "" & Name & "が選択されていません。"
            source.Text = "未入力エラー"
            args.IsValid = False
            Exit Sub
        End If


        'HDDNoとHDD種別の整合性チェック
        If ddlAppaCls.ppDropDownList.SelectedValue = ApaCls_Hdd Then
            Dim conDB As SqlConnection = Nothing
            Dim cmdDB As SqlCommand = Nothing
            Dim dtsDB As New DataSet
            objStack = New StackFrame
            Try
                If clsDataConnect.pfOpen_Database(conDB) Then
                    cmdDB = New SqlCommand(DispCode & "_S6", conDB)
                    'パラメータ設定
                    With cmdDB.Parameters
                        .Add(pfSet_Param("hdd_no", SqlDbType.NVarChar, ddlHddNo.ppSelectedValue))
                        .Add(pfSet_Param("hdd_cls", SqlDbType.NVarChar, ddlHddCls.ppSelectedValue))
                    End With
                    'リストデータ取得
                    dtsDB = clsDataConnect.pfGet_DataSet(cmdDB)
                End If

            Catch ex As Exception
                'エラー
                psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "エラーチェック")
                objStack = New StackFrame
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            End Try

            If dtsDB.Tables(0).Rows.Count <> 0 Then
                For zz As Integer = 0 To cklTboxType.Items.Count - 1
                    'チェックボックスにチェックしてあるTBOXタイプがHDDマスタになかったらアウト
                    If cklTboxType.Items.Item(zz).Selected = False Then
                        Continue For
                    End If

                    Dim blnExistsFlg As Boolean = False
                    For yy As Integer = 0 To dtsDB.Tables(0).Rows.Count - 1
                        If cklTboxType.Items.Item(zz).Value = dtsDB.Tables(0).Rows(yy).Item("TBOXタイプコード").ToString Then
                            blnExistsFlg = True
                            Continue For
                        End If
                    Next
                    If blnExistsFlg = False Then
                        source.ErrorMessage = "HDDと整合性の合わないTBOXタイプが選択されています。"
                        source.Text = "整合性エラー"
                        args.IsValid = False
                        Exit Sub
                    End If
                Next
            End If
        End If

    End Sub

#End Region

End Class
