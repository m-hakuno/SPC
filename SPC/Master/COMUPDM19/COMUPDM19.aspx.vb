'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜マスタ＞
'*　処理名　　：　電話番号マスタ
'*　ＰＧＭＩＤ：　COMUPDPM19
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2016.11.04　：　伯野
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SQL_DBCLS_LIB


Public Class COMUPDM19

    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
#Region "継承定義"

    Inherits System.Web.UI.Page

#End Region

    '============================================================================================================================
    '=　定数定義
    '============================================================================================================================
#Region "定数定義"

    Const DispCode As String = "COMUPDM19"                  '画面ID
    Const MasterName As String = "電話番号マスタ"           '画面名
    Const DBSName As String = "SPCDB.dbo."                  'DB名.スキーマ名
    Const TableName As String = "M19_TELNO"        'テーブル名

#End Region

    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
#Region "変数定義"

    Dim clsDataConnect As New ClsCMDataConnect
    Dim mclsDB As New ClsSQLSvrDB
    Dim ClsSQL As New ClsSQL
    Dim clsExc As New ClsCMExclusive
    Dim clsMst As New ClsMSTCommon
    Dim mclsCMC As New ClsCMCommon
    Dim objStack As StackFrame

    Dim mstrDispMode As String = "DEFAULT"
    Dim strDeleteFlg As String = String.Empty
    Dim strFocusFlg As String = String.Empty

#End Region

    '============================================================================================================================
    '=　イベントプロシージャ
    '============================================================================================================================
#Region "イベントプロシージャ"

    '----------------------------------------------------------------------------------------------------------------------------
    '=　ページ制御
    '----------------------------------------------------------------------------------------------------------------------------
#Region "ページ制御"

    ''' <summary>
    ''' ページ初期化
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, DispCode, 33, 11)

    End Sub

    ''' <summary>
    ''' ページロード
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        '        Master.ppBtnSrcClear.Enabled = False
        '        Master.ppBtnSearch.Enabled = False


        Dim ctrlname As String = Page.Request.Params.Get("__EVENTTARGET")
        Dim a As String = ""
        If String.IsNullOrEmpty(ctrlname) And ctrlname <> String.Empty Then
            a = ctrlname
            'Else
            '    For Each ctl As String In Page.Request.Form
            '        Dim c As Control = Page.FindControl(ctl)
            ''        If ctl = "__EVENTTARGET" Then
            ''            Dim d As String = ""
            ''        End If
            ''        a = c.ClientID
            '    Next
        End If

        'ＤＢ接続
        mclsDB.mstrConString = ConfigurationManager.ConnectionStrings("SPCDB").ToString
        If mclsDB.pfDB_Connect() = False Then
            'ＤＢ接続失敗
        End If

        Master.ppchksDel.Visible = False
        '        ddlDel.ppEnabled = False
        '        ddlDel.ppDropDownList.AutoPostBack = False
        ViewState("txtSrch_TelNo") = txtSrch_TelNo.ppText
        ViewState("ddlSrch_JudgeCls") = ddlSrch_JudgeCls.ppSelectedValue
        ViewState("txtSrch_tboxidFrom") = txtSrch_tboxidFromTo.ppFromText
        ViewState("txtSrch_tboxidTo") = txtSrch_tboxidFromTo.ppToText
        ViewState("txtSrch_Hall") = txtSrch_Hall.ppText
        ViewState("ddlSrchOperate") = ddlSrchOperate.ppSelectedValue
        '        ViewState("ddlSrch_compcd") = ddlSrch_compcd.ppSelectedValue
        '        ViewState("txtSrch_employee") = txtSrch_employee.ppText
        ViewState("ddlDel") = "" 'ddlDel.ppSelectedValue
        ViewState("txtTelNo") = txtTelNo.ppText
        ViewState("txtOldTelNo") = txtOldTelNo.Text
        ViewState("ddlJudge_Cls") = ddlJudge_Cls.ppSelectedValue
        ViewState("txtName") = txtName.ppText
        '        ViewState("ddlHall") = Me.ddlHall.SelectedValue
        ViewState("lblHall") = Me.lblHall.Text
        ViewState("txtOldHallCD") = Me.txtOldHallCD.Text
        ViewState("ddlCompCD") = ddlCompCD.ppSelectedValue
        ViewState("ddlEmployee") = ddlEmployee.ppSelectedValue

        If ViewState("mstrDispMode") Is Nothing Then
        Else
            mstrDispMode = ViewState("mstrDispMode")
        End If

        'ボタンアクション設定
        AddHandler Master.ppBtnSearch.Click, AddressOf btnSearch_Click          '検索
        AddHandler Master.ppBtnSrcClear.Click, AddressOf btnSearchClear_Click   '検索条件クリア
        AddHandler Master.ppBtnInsert.Command, AddressOf btn_Click              '登録
        AddHandler Master.ppBtnUpdate.Command, AddressOf btn_Click              '更新
        AddHandler Master.ppBtnDelete.Command, AddressOf btn_Click              '削除
        AddHandler Master.ppBtnClear.Click, AddressOf btnClear_Click            'クリア

        '        Me.txtSrch_TelNo.ppTextBox.AutoPostBack = True
        Me.ddlSrch_JudgeCls.ppDropDownList.AutoPostBack = True
        '        Me.txtSrch_tboxidFromTo.ppTextBoxFrom.AutoPostBack = True
        '        Me.txtSrch_tboxidFromTo.ppTextBoxTo.AutoPostBack = True
        '        Me.txtSrch_tboxid.ppTextBox.AutoPostBack = False
        '        Me.txtSrch_Hall.ppTextBox.AutoPostBack = True
        '        Me.ddlSrchOperate.ppDropDownList.AutoPostBack = False
        '        Me.ddlSrch_compcd.ppDropDownList.AutoPostBack = 
        '        Me.txtSrch_employee.ppTextBox.AutoPostBack = False
        '        Me.ddlDel.ppDropDownList.AutoPostBack = False
        Me.txtTelNo.ppTextBox.AutoPostBack = True
        Me.txtOldTelNo.AutoPostBack = False
        Me.ddlJudge_Cls.ppDropDownList.AutoPostBack = True
        Me.txtName.ppTextBox.AutoPostBack = True
        Me.ddlCompCD.ppDropDownList.AutoPostBack = True
        Me.txtOldHallCD.AutoPostBack = False

        AddHandler txtSrch_tboxidFromTo.ppTextBoxFrom.TextChanged, AddressOf txtSrch_TBOXID_Changed
        AddHandler txtSrch_tboxidFromTo.ppTextBoxFrom.TextChanged, AddressOf txtSrch_TBOXID_Changed
        AddHandler txtSrch_tboxidFromTo.ppTextBoxTo.TextChanged, AddressOf txtSrch_TBOXID_Changed
        AddHandler ddlSrch_JudgeCls.ppDropDownList.TextChanged, AddressOf ddlSrch_JudgeCls_TextChanged
        AddHandler txtTelNo.ppTextBox.TextChanged, AddressOf txtTelNo_TextChanged
        AddHandler ddlJudge_Cls.ppDropDownList.TextChanged, AddressOf ddlJudge_Cls_TextChanged
        AddHandler txtName.ppTextBox.TextChanged, AddressOf txtName_TextChanged
        AddHandler ddlCompCD.ppDropDownList.TextChanged, AddressOf ddlCompCD_TextChanged
        '        AddHandler ddlHall.TextChanged, AddressOf ddlHall_TextChanged

        '        Master.ppBtnDmy.Visible = True
        '        Master.ppBtnDmy.Enabled = True

        'Dim scm As ScriptManager
        'scm = Master.FindControl("tsmManager")
        'scm.RegisterPostBackControl(txtName)

        'Dim pnl As UpdatePanel
        'pnl = Master.FindControl("UpdatePanelTBOX")
        'pnl.UpdateMode = UpdatePanelUpdateMode.Always

        If Not IsPostBack Then
            'プログラムＩＤ、画面名設定
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"

            '検索条件クリアボタン押下時の検証を無効
            Master.ppBtnSrcClear.CausesValidation = False

            '削除データを含むを活性化
            '            Master.ppchksDel.Visible = True

            'パンくずリスト設定
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            'ボタン押下時のメッセージ設定
            Master.ppBtnInsert.OnClientClick = pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '追加
            Master.ppBtnUpdate.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '更新
            '            Master.ppBtnUpdate.Attributes("onchange") = "textChange();"
            '            Master.ppBtnUpdate.Attributes("onpropertychange") = "textChange();"
            '            Me.txtName.ppTextBox.Attributes("onblur") = "exec_postback();"
            'ValidationGroup設定
            Master.ppBtnInsert.ValidationGroup = "key"
            Master.ppBtnUpdate.ValidationGroup = "key"

            'ドロップダウンリスト設定
            Call msSetddlLineCls()
            Call msSetddlCompCD()
            Call msSetddlOparate()
            Call msSetddlEMPLOYEE()
            '            Call sSetddlAPPACLASS()
            '            Call sSetddlSEARIAL_CLS()


            Call ddlSrch_JudgeCls_TextChanged()
            Call ddlJudge_Cls_TextChanged()

            'グリッドの初期化
            '            Me.grvList.DataSource = New DataTable
            '            grvList.DataBind()

            'データ取得
            msGet_Data("DEF")

            Master.ppMainEnabled = True
            '            mstrDispMode = "DEFAULT"
            ViewState("mstrDispMode") = mstrDispMode

            SetFocus(Me.txtSrch_TelNo.ppTextBox.ClientID)

        Else
            '            If ctrlname.IndexOf("txtSrch_TelNo") <> -1 Then
            '                SetFocus(Me.ddlSrch_JudgeCls.ppDropDownList.ClientID)
            '            End If
            '            If ctrlname.IndexOf("txtSrch_tboxidFromTo$txtTextBoxFrom") <> -1 Then
            '                SetFocus(Me.txtSrch_tboxidFromTo.ppTextBoxTo.ClientID)
            '            End If
            '            If ctrlname.IndexOf("txtSrch_tboxidFromTo$txtTextBoxTo") <> -1 Then
            '                FocusChange(txtSrch_tboxidFromTo.ppTextBoxTo, txtSrch_Hall.ppTextBox)
            ''                SetFocus(Me.txtSrch_Hall.ppTextBox.ClientID)
            '            End If
            '            If ctrlname.IndexOf("txtSrch_Hall") <> -1 Then
            '                If ViewState("ddlSrch_JudgeCls") = "1" Then
            '                    SetFocus(Master.ppBtnSearch.ClientID)
            '                End If
            '                If ViewState("ddlSrch_JudgeCls") = "0" Then
            '                    SetFocus(Me.ddlSrchOperate.ppDropDownList.ClientID)
            '                End If
            '            End If
            '            If ctrlname = "" Then
            '                If sender.Equals(Master.ppBtnSearch) Then
            '                    Call btnSearch_Click(sender, e)
            '                End If
            '                'If sender.Equals(Master.ppBtnSrcClear) Then
            '                '    Call btnSearchClear_Click(sender, e)
            '                'End If
            '                'If sender.Equals(Master.ppBtnClear) Then
            '                '    Call btnClear_Click()
            '                'End If
            '                'If sender.Equals(Master.ppBtnInsert) Then
            '                '    btn_Click(sender, e)
            '                'End If
            '            End If
            ''            Call msGet_Data()
        End If

    End Sub

    ''' <summary>
    ''' ページ成形前
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        If ViewState("txtSrch_TelNo") Is Nothing Then
            txtSrch_TelNo.ppText = ""
        Else
            txtSrch_TelNo.ppText = ViewState("txtSrch_TelNo")
        End If
        If ViewState("ddlSrch_JudgeCls") Is Nothing Then
            ddlSrch_JudgeCls.ppSelectedValue = ""
        Else
            ddlSrch_JudgeCls.ppSelectedValue = ViewState("ddlSrch_JudgeCls")
        End If
        If ViewState("txtSrch_tboxidFrom") Is Nothing Then
            txtSrch_tboxidFromTo.ppTextBoxFrom.Text = ""
        Else
            txtSrch_tboxidFromTo.ppTextBoxFrom.Text = ViewState("txtSrch_tboxidFrom")
        End If
        If ViewState("txtSrch_tboxidTo") Is Nothing Then
            txtSrch_tboxidFromTo.ppTextBoxTo.Text = ""
        Else
            txtSrch_tboxidFromTo.ppTextBoxTo.Text = ViewState("txtSrch_tboxidTo")
        End If
        If ViewState("txtSrch_Hall") Is Nothing Then
            txtSrch_Hall.ppText = ""
        Else
            txtSrch_Hall.ppText = ViewState("txtSrch_Hall")
        End If
        If ViewState("ddlSrchOperate") Is Nothing Then
            ddlSrchOperate.ppSelectedValue = ""
        Else
            ddlSrchOperate.ppSelectedValue = ViewState("ddlSrchOperate")
        End If
        '        If ViewState("ddlSrch_compcd") Is Nothing Then
        '            ddlSrch_compcd.ppSelectedValue = ""
        '        Else
        '            ddlSrch_compcd.ppSelectedValue = ViewState("ddlSrch_compcd")
        '        End If
        '        If ViewState("txtSrch_employee") Is Nothing Then
        '            txtSrch_employee.ppText = ""
        '        Else
        '            txtSrch_employee.ppText = ViewState("txtSrch_employee")
        '        End If
        '        If ViewState("ddlDel") Is Nothing Then
        '            Me.ddlDel.ppSelectedValue = ""
        '        Else
        '            Me.ddlDel.ppSelectedValue = ViewState("ddlDel")
        '        End If
        If ViewState("txtTelNo") Is Nothing Then
            txtTelNo.ppText = ""
        Else
            txtTelNo.ppText = ViewState("txtTelNo")
        End If
        If ViewState("txtOldTelNo") Is Nothing Then
            txtOldTelNo.Text = ""
        Else
            txtOldTelNo.Text = ViewState("txtOldTelNo")
        End If
        If ViewState("ddlJudge_Cls") Is Nothing Then
            ddlJudge_Cls.ppSelectedValue = ""
        Else
            ddlJudge_Cls.ppSelectedValue = ViewState("ddlJudge_Cls")
        End If
        If ViewState("txtName") Is Nothing Then
            txtName.ppText = ""
        Else
            txtName.ppText = ViewState("txtName")
        End If
        'If ViewState("ddlHall") Is Nothing Then
        '    Me.ddlHall.SelectedValue = ""
        'Else
        '    Me.ddlHall.SelectedValue = ViewState("ddlHall")
        'End If
        If ViewState("lblHall") Is Nothing Then
            Me.lblHall.Text = ""
        Else
            Me.lblHall.Text = ViewState("lblHall")
        End If
        If ViewState("txtOldHallCD") Is Nothing Then
            txtOldHallCD.Text = ""
        Else
            txtOldHallCD.Text = ViewState("txtOldHallCD")
        End If
        If ViewState("ddlCompCD") Is Nothing Then
            ddlCompCD.ppSelectedValue = ""
        Else
            ddlCompCD.ppSelectedValue = ViewState("ddlCompCD")
        End If
        If ViewState("ddlEmployee") Is Nothing Then
            ddlEmployee.ppSelectedValue = ""
        Else
            ddlEmployee.ppSelectedValue = ViewState("ddlEmployee")
        End If

        Me.txtTelNo.ppTextBox.Enabled = True
        Me.ddlJudge_Cls.ppDropDownList.Enabled = True
        '        Me.txtName.ppTextBox.Enabled = True
        '        ddlDel.Visible = False
        Me.txtTelNo.ppTextBox.Enabled = True
        Select Case mstrDispMode
            Case "DEFAULT"
                Master.ppBtnInsert.Enabled = False      '追加
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnClear.Enabled = True       'クリア

                Me.txtSrch_TelNo.ppEnabled = True
                Me.ddlSrch_JudgeCls.ppEnabled = True
                '                Me.txtSrch_tboxid.ppEnabled = True
                '                Me.txtSrch_Hall.ppEnabled = True
                '                Me.ddlSrch_compcd.ppEnabled = True
                '                Me.txtSrch_employee.ppEnabled = True
                Me.txtTelNo.ppEnabled = True
                Me.ddlJudge_Cls.ppEnabled = True
                '                Me.txtName.ppEnabled = True
                Master.ppBtnDelete.Text = "削除"
                Master.ppMainEnabled = True
            Case "SELECT", "UPDATE"
                Master.ppBtnInsert.Enabled = False     '追加
                '                If Master.ppBtnDelete.Text = "削除取消" Then
                '                    Master.ppBtnUpdate.Enabled = False
                '                    Master.ppMainEnabled = False
                '                Else
                Master.ppBtnUpdate.Enabled = True      '更新
                Master.ppMainEnabled = True
                '                End If
                Master.ppBtnDelete.Enabled = True      '削除
                Master.ppBtnClear.Enabled = True       'クリア
                Me.txtSrch_TelNo.ppEnabled = True
                Me.ddlSrch_JudgeCls.ppEnabled = True
                '                Me.txtSrch_tboxid.ppEnabled = True
                '                Me.txtSrch_Hall.ppEnabled = True
                '                Me.ddlSrch_compcd.ppEnabled = True
                '                Me.txtSrch_employee.ppEnabled = True
                Me.txtTelNo.ppTextBox.Enabled = False
                Me.ddlJudge_Cls.ppEnabled = False
'                Me.txtName.ppEnabled = True
'                Me.txtAPPA_CLS.ppTextBox.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC")
            Case "INSERT"
                Master.ppBtnInsert.Enabled = True     '追加
                Master.ppBtnDelete.Text = "削除"
                Master.ppBtnDelete.Enabled = False      '削除
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnClear.Enabled = True       'クリア
                Me.txtSrch_TelNo.ppEnabled = True
                Me.ddlSrch_JudgeCls.ppEnabled = True
                '                Me.txtSrch_tboxid.ppEnabled = True
                '                Me.txtSrch_Hall.ppEnabled = True
                '                Me.ddlSrch_compcd.ppEnabled = True
                '                Me.txtSrch_employee.ppEnabled = True

                '                If Me.txtTelNo.ppText <> "" And ddlJudge_Cls.ppDropDownList.SelectedValue <> "" Then
                '                    Page.Validate("key")
                '                    If Page.IsValid Then
                '                        Me.txtTelNo.ppEnabled = False
                '                        Me.ddlJudge_Cls.ppEnabled = False
                '                    End If
                '                Else
                '                    Me.txtTelNo.ppEnabled = True
                '                    Me.ddlJudge_Cls.ppEnabled = True
                '                End If

                '                Me.txtName.ppEnabled = True
        End Select

        '        If Me.ddlJudge_Cls.ppSelectedValue <> "" Then
        '            Me.ddlJudge_Cls.ppEnabled = False
        '        Else
        '            Me.ddlJudge_Cls.ppEnabled = True
        '        End If

        ViewState("mstrDispMode") = mstrDispMode

    End Sub

    ''' <summary>
    ''' ページ作成終了
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Unload(sender As Object, e As EventArgs) Handles Me.Unload

        'ＤＢクローズ
        Call mclsDB.psDB_Close()

    End Sub

#End Region

    '----------------------------------------------------------------------------------------------------------------------------
    '-　ボタン制御
    '----------------------------------------------------------------------------------------------------------------------------
#Region "ボタン制御"

    ''' <summary>
    ''' 検索ボタン　押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        If sender.Equals(Master.ppBtnSearch) Then
        Else
            Exit Sub
        End If

        'ログ出力開始
        psLogStart(Me)

        Page.Validate("search")

        If Me.txtSrch_TelNo.ppText <> "" Or Me.ddlSrch_JudgeCls.ppSelectedValue <> "" Or Me.txtSrch_tboxidFromTo.ppTextBoxFrom.Text <> "" Or Me.txtSrch_tboxidFromTo.ppTextBoxTo.Text <> "" Or Me.txtSrch_Hall.ppText <> "" Then

            Call msSrchCheck()
            'データ取得
            If (Page.IsValid) Then

                'コントロールの制御
                Master.ppMainEnabled = True
                If mstrDispMode <> "DEFAULT" And mstrDispMode <> "DELETE" Then
                Else
                    mstrDispMode = "DEFAULT"
                End If
                ViewState("mstrDispMode") = mstrDispMode

                msGet_Data()

            End If
        Else
            clsMst.psMesBox(Me, "30016", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "検索条件")
        End If

        'フォーカス移動
        SetFocus(Me.txtSrch_TelNo.ppTextBox.ClientID)
        '        Me.txtTelNo.ppTextBox.Focus()

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 検索条件クリア　押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearchClear_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        '画面項目クリア
        Me.txtSrch_TelNo.ppText = ""
        Me.ddlSrch_JudgeCls.ppSelectedValue = ""
        Me.txtSrch_tboxidFromTo.ppTextBoxFrom.Text = ""
        Me.txtSrch_tboxidFromTo.ppTextBoxTo.Text = ""
        Me.txtSrch_Hall.ppText = ""
        Me.ddlSrchOperate.ppSelectedValue = ""
        '        Me.ddlSrch_compcd.ppSelectedValue = ""
        '        Me.txtSrch_employee.ppText = ""
        '        Me.ddlDel.ppSelectedValue = ""

        'ＶＩＥＷＳＴＡＴＥクリア
        ViewState("txtSrch_TelNo") = txtSrch_TelNo.ppText
        ViewState("ddlSrch_JudgeCls") = ddlSrch_JudgeCls.ppSelectedValue
        ViewState("txtSrch_tboxidFrom") = txtSrch_tboxidFromTo.ppTextBoxFrom.Text
        ViewState("txtSrch_tboxidTo") = txtSrch_tboxidFromTo.ppTextBoxTo.Text
        ViewState("txtSrch_Hall") = txtSrch_Hall.ppText
        ViewState("ddlSrchOperate") = ddlSrchOperate.ppSelectedValue
        '        ViewState("ddlSrch_compcd") = ddlSrch_compcd.ppSelectedValue
        '        ViewState("txtSrch_employee") = txtSrch_employee.ppText
        ViewState("ddlDel") = "" 'ddlDel.ppSelectedValue

        '        Me.txtSrch_tboxid.Visible = False
        '        Me.txtSrch_Hall.Visible = False
        '        Me.ddlSrch_compcd.Visible = False
        '        Me.txtSrch_employee.Visible = False
        Me.txtSrch_tboxidFromTo.ppEnabled = False
        Me.txtSrch_Hall.ppEnabled = False
        Me.ddlSrchOperate.ppEnabled = False
        Master.ppchksDel.Checked = False

        'フォーカス移動
        SetFocus(Me.txtSrch_TelNo.ppTextBox.ClientID)
        '        Me.txtSrch_TelNo.ppTextBox.Focus()

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 追加／更新／削除ボタン　押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_Click(sender As Object, e As CommandEventArgs)

        'ログ出力開始
        psLogStart(Me)

        If e.CommandName <> "DELETE" Then
            Page.Validate("key")
            Call msCheck()
        End If
        If (Page.IsValid) Then
            msEditData(e.CommandName)
        End If

        SetFocus(Me.txtTelNo.ppTextBox.ClientID)

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' クリアボタン　押下
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

        '画面項目クリア
        'Me.txtSrch_TelNo.ppText = ""
        'Me.ddlSrch_JudgeCls.ppSelectedValue = ""
        'Me.txtSrch_tboxid.ppText = ""
        'Me.txtSrch_Hall.ppText = ""
        'Me.ddlSrch_compcd.ppSelectedValue = ""
        'Me.txtSrch_employee.ppText = ""
        Me.txtTelNo.ppText = ""
        Me.txtOldTelNo.Text = ""
        Me.ddlJudge_Cls.ppSelectedValue = ""
        Me.txtName.ppText = ""
        Me.txtName.ppEnabled = False
        'Me.ddlHall.SelectedValue = ""
        'Me.ddlHall.Enabled = False
        Me.lblHall.Text = ""
        Me.txtOldHallCD.Text = ""
        Me.ddlCompCD.ppSelectedValue = ""
        Me.ddlCompCD.ppEnabled = False
        Me.ddlEmployee.ppSelectedValue = ""
        Me.ddlEmployee.ppEnabled = False

        'ＶＩＥＷＳＴＡＴＥクリア
        '        ViewState("txtSrch_TelNo") = txtSrch_TelNo.ppText
        '        ViewState("ddlSrch_JudgeCls") = ddlSrch_JudgeCls.ppSelectedValue
        '        ViewState("txtSrch_tboxid") = txtSrch_tboxid.ppText
        '        ViewState("txtSrch_Hall") = txtSrch_Hall.ppText
        '        ViewState("ddlSrch_compcd") = ddlSrch_compcd.ppSelectedValue
        '        ViewState("txtSrch_employee") = txtSrch_employee.ppText
        ViewState("txtTelNo") = txtTelNo.ppText
        ViewState("txtOldTelNo") = txtOldTelNo.Text
        ViewState("ddlJudge_Cls") = ddlJudge_Cls.ppSelectedValue
        ViewState("txtName") = txtName.ppText
        '        ViewState("ddlHall") = ddlHall.SelectedValue
        ViewState("lblHall") = lblHall.Text
        ViewState("txtOldHallCD") = txtOldHallCD.Text
        ViewState("ddlCompCD") = ddlCompCD.ppSelectedValue
        ViewState("ddlEmployee") = ddlEmployee.ppSelectedValue
        mstrDispMode = "DEFAULT"
        ViewState("mstrDispMode") = mstrDispMode

        SetFocus(Me.txtTelNo.ppTextBox.ClientID)

        'ログ出力終了
        psLogEnd(Me)

    End Sub

#End Region

    '----------------------------------------------------------------------------------------------------------------------------
    '-　ボタン以外のコントロール制御
    '----------------------------------------------------------------------------------------------------------------------------
#Region "ボタン以外のコントロール制御"

    Private Sub ddlHall_TextChanged()

        If lblHall.Text = "" Then
            ViewState("lblHall") = ""
        Else
            ViewState("lblHall") = lblHall.Text
        End If

    End Sub

    Private Sub txtTelNo_TextChanged()

        'If mstrDispMode = "SELECT" Then
        'ElseIf mstrDispMode = "INSERT" Then
        'ElseIf mstrDispMode = "DELETE" Then
        'ElseIf mstrDispMode = "UPDATE" Then
        'Else
        '    mstrDispMode = "INSERT"
        '    ViewState("mstrDispMode") = mstrDispMode
        'End If

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        objStack = New StackFrame

        If txtTelNo.ppText.Trim <> String.Empty Then
            '入力チェック
            Page.Validate("key")
            Call msKeyCheck()
            If (Page.IsValid) Then
                '接続
                If clsDataConnect.pfOpen_Database(conDB) Then
                    Try
                        'ドロップダウン設定
                        Call msSetddlLineCls()
                        Call msSetddlCompCD()
                        Call msSetddlOparate()
                        Call msSetddlEMPLOYEE()

                        cmdDB = New SqlCommand(DispCode & "_S1", conDB)

                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("prmTELNO", SqlDbType.NVarChar, txtTelNo.ppText.Trim))
                            .Add(pfSet_Param("prmDECISION_CLS", SqlDbType.NVarChar, ""))
                            .Add(pfSet_Param("prmTBOXIDFm", SqlDbType.NVarChar, ""))
                            .Add(pfSet_Param("prmTBOXIDTo", SqlDbType.NVarChar, ""))
                            .Add(pfSet_Param("prmHALL", SqlDbType.NVarChar, ""))
                            .Add(pfSet_Param("prmOPERATE", SqlDbType.NVarChar, ""))
                            .Add(pfSet_Param("prmCOMPCD", SqlDbType.NVarChar, ""))
                            .Add(pfSet_Param("prmprog_id", SqlDbType.NVarChar, DispCode))
                            .Add(pfSet_Param("prmPROCFLG", SqlDbType.NVarChar, "2"))
                        End With

                        'リストデータ取得
                        dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                        '既に存在している場合は各項目に値を設定
                        If dstOrders.Tables(0).Rows.Count > 0 Then

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
                            arKey.Insert(0, txtTelNo.ppText.Trim)

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

                            '編集エリアに値を設定
                            With dstOrders.Tables(0).Rows(0)
                                Me.txtOldTelNo.Text = Me.txtTelNo.ppText
                                ddlJudge_Cls.ppSelectedValue = .Item("判定区分").ToString
                                ViewState("txtTelNo") = txtTelNo.ppText
                                ViewState("txtOldTelNo") = txtOldTelNo.Text
                                ViewState("ddlJudge_Cls") = ddlJudge_Cls.ppSelectedValue
                                If .Item("判定区分").ToString = "0" Then
                                    Me.ddlEmployee.ppDropDownList.SelectedIndex = -1
                                    Me.ddlEmployee.ppEnabled = False
                                    Me.txtName.ppEnabled = True
                                    Me.lblHall.Enabled = True
                                    Me.txtName.ppText = .Item("TBOXID").ToString
                                    Me.lblHall.Text = .Item("ホール").ToString.Replace(Environment.NewLine, "")
                                    Me.txtOldHallCD.Text = .Item("ホール").ToString
                                    ViewState("txtName") = txtName.ppText
                                    ViewState("lblHall") = lblHall.Text
                                    ViewState("txtOldHallCD") = txtOldHallCD.Text
                                    SetFocus(Me.txtName.ppTextBox.ClientID)
                                ElseIf .Item("判定区分").ToString = "1" Then
                                    Me.txtName.ppTextBox.Text = ""
                                    Me.txtName.ppEnabled = False
                                    Me.ddlCompCD.ppEnabled = True
                                    ''Me.ddlEmployee.ppEnabled = True
                                    Call msSetddlCompCD()
                                    Me.ddlCompCD.ppSelectedValue = .Item("会社コード").ToString
                                    ViewState("ddlCompCD") = ddlCompCD.ppSelectedValue
                                    Call msSetddlEMPLOYEE()
                                    Me.ddlEmployee.ppSelectedValue = .Item("社員コード").ToString
                                    ViewState("ddlEmployee") = ddlEmployee.ppSelectedValue
                                    SetFocus(Me.ddlEmployee.ppDropDownList.ClientID)
                                End If
                            End With

                            cmdDB.Dispose()
                            Me.txtTelNo.ppEnabled = False
                            Me.ddlJudge_Cls.ppEnabled = False

                            mstrDispMode = "SELECT"

                        Else
                            mstrDispMode = "INSERT"
                        End If

                        'If Master.ppBtnDelete.Text = "削除" Then
                        '    Master.ppBtnDmy.Visible = True
                        '    Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtTelNo.ppTextBox.ClientID + ");"
                        '    SetFocus(Master.ppBtnDmy.ClientID)
                        'Else
                        '    SetFocus(Master.ppBtnClear.ClientID)
                        'End If

                        If ViewState("txtTelNo") <> "" And ViewState("ddlJudge_Cls") <> "" Then ' txtTelNo.ppTextBox.Text <> "" And Me.ddlJudge_Cls.ppSelectedValue <> "" Then
                            Me.txtTelNo.ppEnabled = False
                            Me.ddlJudge_Cls.ppEnabled = False
                        Else
                            Me.txtTelNo.ppEnabled = True
                            Me.ddlJudge_Cls.ppEnabled = True
                        End If

                        Page.Validate("key")
                        Call msKeyCheck()
                        If Page.IsValid = True Then
                            If ViewState("ddlJudge_Cls") = "0" Then
                                Me.txtName.ppEnabled = True
                                Me.ddlEmployee.ppEnabled = False
                                ViewState("ddlEmployee") = ""
                                Me.ddlEmployee.ppDropDownList.SelectedIndex = -1
                                SetFocus(Me.txtName.ppTextBox.ClientID)
                            ElseIf ViewState("ddlJudge_Cls") = "1" Then
                                Me.txtName.ppEnabled = False
                                ViewState("txtName") = ""
                                Me.txtName.ppTextBox.Text = ""
                                Me.ddlEmployee.ppEnabled = True
                                SetFocus(Me.ddlEmployee.ppDropDownList.ClientID)
                            End If
                        End If

                    Catch ex As Exception
                        clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, ViewState(MasterName))
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    Finally
                        'DB切断
                        If Not clsDataConnect.pfClose_Database(conDB) Then
                            clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        End If
                    End Try
                Else
                    clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End If
        Else
            mstrDispMode = "DEFAULT"
            SetFocus(Master.ppBtnClear.ClientID)
        End If

        SetFocus(Me.ddlJudge_Cls.ppDropDownList.ClientID)
        '        Me.ddlJudge_Cls.ppDropDownList.Focus()

    End Sub

    Private Sub ddlJudge_Cls_TextChanged()

        'If mstrDispMode = "SELECT" Then
        'ElseIf mstrDispMode = "INSERT" Then
        'ElseIf mstrDispMode = "DELETE" Then
        'ElseIf mstrDispMode = "UPDATE" Then
        'Else
        '    mstrDispMode = "INSERT"
        '    ViewState("mstrDispMode") = mstrDispMode
        'End If
        '        Me.txtName.ppTextBox.Focus()

        Me.txtName.ppEnabled = False
        '        Me.ddlHall.Enabled = True
        Me.ddlCompCD.ppEnabled = False
        Me.ddlEmployee.ppEnabled = False

        '        If Me.ddlJudge_Cls.ppSelectedValue = "" Then
        If ViewState("ddlJudge_Cls") Is Nothing Then
        ElseIf ViewState("ddlJudge_Cls") = "" Then
            Me.txtName.ppText = ""
            '            Me.ddlHall.SelectedIndex = -1
            Me.lblHall.Text = ""
            Me.ddlCompCD.ppDropDownList.SelectedIndex = -1
            Me.ddlEmployee.ppDropDownList.SelectedIndex = -1
            ViewState("txtName") = ""
            '            ViewState("ddlHall") = ""
            ViewState("lblHall") = ""
            ViewState("ddlCompCD") = ""
            ViewState("ddlEmployee") = ""
            Me.txtName.ppEnabled = False
            '            Me.ddlHall.Enabled = False
            Me.ddlCompCD.ppEnabled = False
            Me.ddlEmployee.ppEnabled = False
        ElseIf ViewState("ddlJudge_Cls") = "0" Then
            Me.ddlCompCD.ppDropDownList.SelectedIndex = -1
            Me.ddlEmployee.ppDropDownList.SelectedIndex = -1
            Me.txtName.ppText = ""
            '            Me.ddlHall.SelectedIndex = -1
            Me.lblHall.Text = ""
            ViewState("txtName") = ""
            '            ViewState("ddlHall") = ""
            ViewState("lblHall") = ""
            ViewState("ddlCompCD") = ""
            ViewState("ddlEmployee") = ""
            Me.ddlCompCD.ppEnabled = False
            Me.ddlEmployee.ppEnabled = False
            Me.txtName.ppTextBox.Focus()
        ElseIf ViewState("ddlJudge_Cls") = "1" Then
            Me.ddlCompCD.ppDropDownList.SelectedIndex = -1
            Me.ddlEmployee.ppDropDownList.SelectedIndex = -1
            Me.txtName.ppText = ""
            '            Me.ddlHall.SelectedIndex = -1
            Me.lblHall.Text = ""
            ViewState("txtName") = ""
            '            ViewState("ddlHall") = ""
            ViewState("lblHall") = ""
            ViewState("ddlCompCD") = ""
            ViewState("ddlEmployee") = ""
            Me.txtName.ppEnabled = False
            '            Me.ddlHall.Enabled = False
            '            Me.ddlCompCD.ppSelectedValue = "701"
            '            ViewState("ddlCompCD") = "701"
            '            Call ddlCompCD_TextChanged()
            ddlEmployee.ppDropDownList.Focus()
        End If

        '        If Me.txtTelNo.ppText <> "" And Me.ddlJudge_Cls.ppSelectedValue <> "" Then
        If ViewState("txtTelNo") <> "" And ViewState("ddlJudge_Cls") <> "" Then
            Page.Validate("key")
            Call msKeyCheck()
            If Page.IsValid = True Then
                Me.txtTelNo.ppEnabled = False
                Me.ddlJudge_Cls.ppEnabled = False
                If ViewState("ddlJudge_Cls") = "0" Then
                    Me.txtName.ppEnabled = True
                    Me.ddlEmployee.ppEnabled = False
                    ViewState("ddlEmployee") = ""
                    Me.ddlJudge_Cls.ppDropDownList.SelectedIndex = -1
                    SetFocus(Me.txtName.ppTextBox.ClientID)
                ElseIf ViewState("ddlJudge_Cls") = "1" Then
                    Me.txtName.ppEnabled = False
                    ViewState("txtName") = ""
                    Me.txtName.ppTextBox.Text = ""
                    Me.ddlEmployee.ppEnabled = True
                    SetFocus(Me.ddlEmployee.ppDropDownList.ClientID)
                End If
                ''mstrDispMode = "DEFAULT"
            End If
        End If

        If ViewState("ddlJudge_Cls") = "1" Then
            Call msSetddlEMPLOYEE()
        End If
    End Sub

    Private Sub ddlSrch_JudgeCls_TextChanged()

        'If mstrDispMode = "SELECT" Then
        'ElseIf mstrDispMode = "INSERT" Then
        'ElseIf mstrDispMode = "DELETE" Then
        'ElseIf mstrDispMode = "UPDATE" Then
        'Else
        '    mstrDispMode = "INSERT"
        '    ViewState("mstrDispMode") = mstrDispMode
        'End If
        '        Me.txtName.ppTextBox.Focus()

        Me.txtSrch_tboxidFromTo.ppEnabled = True
        Me.txtSrch_Hall.ppEnabled = True
        '        Me.ddlSrch_compcd.Visible = True
        '        Me.txtSrch_employee.Visible = True

        ViewState("ddlSrch_JudgeCls") = Me.ddlSrch_JudgeCls.ppSelectedValue
        If Me.ddlSrch_JudgeCls.ppSelectedValue = "" Then
            Me.txtSrch_tboxidFromTo.ppFromText = ""
            Me.txtSrch_tboxidFromTo.ppToText = ""
            Me.txtSrch_Hall.ppText = ""
            Me.ddlSrchOperate.ppDropDownList.SelectedIndex = -1
            ViewState("txtSrch_tboxidFrom") = ""
            ViewState("txtSrch_tboxidTo") = ""
            ViewState("txtSrch_Hall") = ""
            ViewState("ddlSrchOperate") = ""
            Me.txtSrch_tboxidFromTo.ppEnabled = False
            Me.txtSrch_Hall.ppEnabled = False
            Me.ddlSrchOperate.ppEnabled = False
        ElseIf Me.ddlSrch_JudgeCls.ppSelectedValue = "0" Then
            Me.txtSrch_tboxidFromTo.ppFromText = ""
            Me.txtSrch_tboxidFromTo.ppToText = ""
            Me.txtSrch_Hall.ppText = ""
            ViewState("txtSrch_tboxidFrom") = ""
            ViewState("txtSrch_tboxidTo") = ""
            ViewState("txtSrch_Hall") = ""
            ViewState("ddlSrchOperate") = ""
            Me.ddlSrchOperate.ppDropDownList.SelectedIndex = -1
            Me.txtSrch_tboxidFromTo.ppEnabled = True
            Me.txtSrch_Hall.ppEnabled = True
            Me.ddlSrchOperate.ppEnabled = True
        ElseIf Me.ddlSrch_JudgeCls.ppSelectedValue = "1" Then
            Me.txtSrch_tboxidFromTo.ppFromText = ""
            Me.txtSrch_tboxidFromTo.ppToText = ""
            Me.txtSrch_Hall.ppText = ""
            Me.ddlSrchOperate.ppDropDownList.SelectedIndex = -1
            ViewState("txtSrch_tboxidFrom") = ""
            ViewState("txtSrch_tboxidTo") = ""
            ViewState("txtSrch_Hall") = ""
            ViewState("ddlSrchOperate") = ""
            Me.txtSrch_tboxidFromTo.ppEnabled = True
            Me.txtSrch_Hall.ppEnabled = True
            Me.ddlSrchOperate.ppEnabled = False
        End If

        If ViewState("ddlSrch_JudgeCls") = "0" Or ViewState("ddlSrch_JudgeCls") = "1" Then
            '            Me.txtSrch_tboxidFromTo.ppTextBoxFrom.Focus()
        Else
            Me.txtSrch_TelNo.ppTextBox.Focus()
        End If

    End Sub

    Private Sub txtSrch_TBOXID_Changed(sender As Object, e As EventArgs)

        '        If sender.Equals(txtSrch_tboxidFromTo.ppTextBoxFrom) Then
        ''            SetFocus(txtSrch_tboxidFromTo.ppTextBoxTo.ClientID)
        '        ElseIf sender.Equals(txtSrch_tboxidFromTo.ppTextBoxTo) Then
        ''            SetFocus(txtSrch_Hall.ppTextBox.ClientID)
        '            FocusChange(txtSrch_tboxidFromTo.ppTextBoxTo, txtSrch_Hall.ppTextBox)
        '        End If

        '        Call msSetHallName("SRCH")

    End Sub

    Private Sub txtName_TextChanged()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        Dim blnExistence As Boolean = False

        'If mstrDispMode = "SELECT" Then
        'ElseIf mstrDispMode = "INSERT" Then
        'ElseIf mstrDispMode = "DELETE" Then
        'ElseIf mstrDispMode = "UPDATE" Then
        'Else
        '    mstrDispMode = "INSERT"
        '    ViewState("mstrDispMode") = mstrDispMode
        'End If
        '        Me.txtTelNo.ppTextBox.Focus()

        Page.Validate("key")
        Call msCheck()

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                '                ddlHall.SelectedIndex = -1
                lblHall.Text = ""
                objCmd = New SqlCommand("COMUPDM19_S5", objCn)
                objCmd.Parameters.Add(pfSet_Param("prmTBOXID", SqlDbType.NVarChar, txtName.ppText))

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                '                Me.ddlHall.Items.Clear()
                '                Me.ddlHall.DataSource = objDs.Tables(0)
                '                Me.ddlHall.DataTextField = "T01_HALL_NAME"
                '                Me.ddlHall.DataValueField = "T01_HALL_CD"
                '                Me.ddlHall.DataBind()
                '                Me.ddlHall.Items.Insert(0, "")
                ''                Me.ddlHall.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                '                ddlHall.SelectedValue = ""
                '                ViewState("ddlHall") = ""
                Me.lblHall.Text = ""
                If objDs.Tables.Count > 0 Then
                    If objDs.Tables(0).Rows.Count > 0 Then
                        lblHall.Text = objDs.Tables(0).Rows(0).Item("T01_HALL_NAME").ToString.Replace(Environment.NewLine, "")
                        ViewState("lblHall") = lblHall.Text
                        blnExistence = True
                    End If
                End If
                '                Me.txtOldHallCD.Text = ""
                '                ViewState("txtOldHallCD") = ""
                If blnExistence = False Then
                    Me.lblHall.Text = ""
                    ViewState("lblHall") = ""
                    '                    Me.txtName.psSet_ErrorNo("2002", "TBOXID")
                End If

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ホール情報取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                Call mclsDB.psDisposeDataSet(objDs)
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If


        '        Me.txtName.ppTextBox.Focus()

        '        Call msSetHallName("EDIT")

    End Sub

    Private Sub ddlCompCD_TextChanged()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("COMUPDM19_S3", objCn)

                With objCmd.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prmCOMPCD", SqlDbType.NVarChar, ViewState("ddlCompCD")))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                Me.ddlEmployee.ppDropDownList.Items.Clear()
                Me.ddlEmployee.ppDropDownList.DataSource = objDs.Tables(0)
                Me.ddlEmployee.ppDropDownList.DataTextField = "M02_EMPLOYEE_NAME"
                Me.ddlEmployee.ppDropDownList.DataValueField = "M02_EMPLOYEE_CD"
                Me.ddlEmployee.ppDropDownList.DataBind()
                Me.ddlEmployee.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

                Me.ddlEmployee.ppSelectedValue = ""
                ViewState("ddlEmployee") = ""

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "社員リスト取得")
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

    '----------------------------------------------------------------------------------------------------------------------------
    '-　グリッドビュー制御
    '----------------------------------------------------------------------------------------------------------------------------
#Region "グリッドビュー制御"

    Private Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        'For Each rowData As GridViewRow In grvList.Rows
        '    If CType(rowData.FindControl("削除フラグ"), TextBox).Text = "1" Then
        '        If rowData.HasControls = True Then
        '            Dim zz As Integer = 0
        '            For zz = 0 To rowData.Cells.Count - 1
        '                If rowData.Cells(zz).HasControls Then
        '                    For yy = 0 To rowData.Cells(zz).Controls.Count - 1
        '                        If rowData.Cells(zz).Controls(yy).GetType.ToString = "System.Web.UI.WebControls.TextBox" Then
        '                            DirectCast(rowData.Cells(zz).Controls(0), TextBox).ForeColor = Drawing.Color.Red
        '                        End If
        '                    Next
        '                End If
        '            Next
        '        End If
        '    End If
        'Next

    End Sub

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
                arKey.Insert(0, CType(rowData.FindControl("電話番号"), TextBox).Text)

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
                Dim strJudgeCls As String = ""

                Me.txtTelNo.ppText = CType(rowData.FindControl("電話番号"), TextBox).Text.Replace("-", "").Replace(Environment.NewLine, "")
                Me.txtOldTelNo.Text = CType(rowData.FindControl("電話番号"), TextBox).Text.Replace("-", "").Replace(Environment.NewLine, "")

                strJudgeCls = CType(rowData.FindControl("判定区分"), TextBox).Text
                ViewState("ddlJudge_Cls") = CType(rowData.FindControl("判定区分"), TextBox).Text

                Me.ddlJudge_Cls.ppDropDownList.SelectedValue = strJudgeCls
                '                Call ddlJudge_Cls_TextChanged()

                If strJudgeCls = "0" Then

                    Me.txtName.ppText = CType(rowData.FindControl("TBOXID"), TextBox).Text
                    ViewState("txtName") = CType(rowData.FindControl("TBOXID"), TextBox).Text

                    Dim objCn As SqlConnection = Nothing
                    Dim objCmd As SqlCommand = Nothing
                    Dim objDs As DataSet = Nothing
                    Try
                        If Not clsDataConnect.pfOpen_Database(objCn) Then
                            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        Else
                            'ddlHall.SelectedIndex = -1
                            lblHall.Text = ""
                            objCmd = New SqlCommand("COMUPDM19_S5", objCn)
                            objCmd.Parameters.Add(pfSet_Param("prmTBOXID", SqlDbType.NVarChar, ViewState("txtName")))
                            'データ取得
                            objDs = clsDataConnect.pfGet_DataSet(objCmd)
                            'ドロップダウンリスト設定
                            'Me.ddlHall.Items.Clear()
                            'Me.ddlHall.DataSource = objDs.Tables(0)
                            'Me.ddlHall.DataTextField = "T01_HALL_NAME"
                            'Me.ddlHall.DataValueField = "T01_HALL_CD"
                            'Me.ddlHall.DataBind()
                            'Me.ddlHall.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                            If objDs.Tables.Count > 0 Then
                                If objDs.Tables(0).Rows.Count > 0 Then
                                    lblHall.Text = objDs.Tables(0).Rows(0).Item("T01_HALL_NAME").ToString.Replace(Environment.NewLine, "")
                                    ViewState("lblHall") = lblHall.Text
                                End If
                            End If
                        End If
                    Catch ex As Exception
                        clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ホールリスト取得")
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    Finally
                        Call mclsDB.psDisposeDataSet(objDs)
                        'DB切断
                        If Not clsDataConnect.pfClose_Database(objCn) Then
                            clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        End If
                    End Try
                    'Me.ddlHall.SelectedValue = CType(rowData.FindControl("ホールコード"), TextBox).Text
                    'ViewState("ddlHall") = CType(rowData.FindControl("ホールコード"), TextBox).Text
                    Me.lblHall.Text = CType(rowData.FindControl("ホール"), TextBox).Text.Replace(Environment.NewLine, "")
                    ViewState("lblHall") = CType(rowData.FindControl("ホール"), TextBox).Text.Replace(Environment.NewLine, "")
                    'Me.txtOldHallCD.Text = CType(rowData.FindControl("ホールコード"), TextBox).Text
                    'ViewState("txtOldHallCD") = CType(rowData.FindControl("ホールコード"), TextBox).Text
                    Me.txtOldHallCD.Text = CType(rowData.FindControl("ホール"), TextBox).Text
                    ViewState("txtOldHallCD") = CType(rowData.FindControl("ホール"), TextBox).Text
                    Me.txtName.ppEnabled = True
                    ViewState("ddlEmployee") = ""
                    Me.ddlEmployee.ppDropDownList.SelectedIndex = -1
                    Me.ddlEmployee.ppEnabled = False
                ElseIf strJudgeCls = "1" Then
                    Call msSetddlCompCD()
                    Me.ddlCompCD.ppDropDownList.SelectedValue = CType(rowData.FindControl("会社コード"), TextBox).Text
                    ViewState("ddlCompCD") = CType(rowData.FindControl("会社コード"), TextBox).Text
                    Call ddlCompCD_TextChanged()
                    Me.ddlEmployee.ppDropDownList.SelectedValue = CType(rowData.FindControl("社員コード"), TextBox).Text
                    ViewState("ddlEmployee") = CType(rowData.FindControl("社員コード"), TextBox).Text
                    Me.ddlEmployee.ppEnabled = True
                    ViewState("txtName") = ""
                    ViewState("lblHall") = ""
                    Me.txtName.ppTextBox.Text = ""
                    Me.lblHall.Text = ""
                    Me.txtName.ppEnabled = False
                End If
                If CType(rowData.FindControl("削除フラグ"), TextBox).Text = "0" Then
                    Master.ppBtnDelete.Text = "削除"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '削除
                Else
                    Master.ppBtnDelete.Text = "削除取消"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
                    'Master.ppMainEnabled = False
                End If

                ViewState("txtTelNo") = CType(rowData.FindControl("電話番号"), TextBox).Text.Replace(Environment.NewLine, "")
                ViewState("txtOldTelNo") = CType(rowData.FindControl("電話番号"), TextBox).Text.Replace(Environment.NewLine, "")
                ViewState("htxtDELETE") = CType(rowData.FindControl("削除フラグ"), TextBox).Text

                mstrDispMode = "SELECT"
                ViewState("mstrDispMode") = mstrDispMode
            End If

            SetFocus(Me.txtTelNo.ClientID)
            '            Me.txtTelNo.Focus()

        Catch ex As Exception
            'エラー
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
            objStack = New StackFrame
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
        'ログ出力終了
        psLogEnd(Me)
    End Sub


#End Region

#End Region

    '============================================================================================================================
    '=　プロシージャ
    '============================================================================================================================
#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 入力チェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheck()

        Dim arySPTELNO As String() = Nothing
        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        Dim blnExistence As Boolean = False

        'TEL
        If txtTelNo.ppText.Trim <> String.Empty Then
            '数字のみでチェック
            If Not Regex.IsMatch(txtTelNo.ppText.Trim, "^[0-9]+$") Then
                '特殊電話番号の取得
                If mfGet_SPTELNOXml(arySPTELNO) = True Then
                    '特殊電話番号取得成功
                    Dim index1 As Integer = Array.IndexOf(arySPTELNO, txtTelNo.ppText.Trim)
                    '特殊電話番号を一致しなければエラー
                    If index1 = -1 Then
                        txtTelNo.psSet_ErrorNo("4001", "電話番号", "半角数字")
                    Else
                        '特殊電話番号と一致して数字、ハイフン以外以外があったらエラー
                        If Not Regex.IsMatch(txtTelNo.ppText.Trim, "^[-.0-9]+$") Then
                            txtTelNo.psSet_ErrorNo("4001", "電話番号", "半角数字と半角ハイフン")
                        End If
                    End If
                    '特殊電話番号の取得に失敗し、数字以外が使われていたらエラー
                Else
                    txtTelNo.psSet_ErrorNo("4001", "電話番号", "半角数字")
                End If
            End If
        End If
        '区分
        If ddlJudge_Cls.ppDropDownList.SelectedValue = "0" Or ddlJudge_Cls.ppDropDownList.SelectedValue = "1" Then
        Else
            ddlJudge_Cls.psSet_ErrorNo("5003", "区分")
        End If
        'TBOXID
        If txtName.ppText.Trim <> String.Empty Then
            '数字のみでチェック
            If Not Regex.IsMatch(txtName.ppText.Trim, "^[0-9]+$") Then
                txtName.psSet_ErrorNo("4001", "TBOXID", "半角数字")
            End If
            '桁数チェック
            If txtName.ppText.Trim.Length <> 8 Then
                txtName.psSet_ErrorNo("3001", "TBOXID", "8")
            End If
        End If
        '区分
        If ddlJudge_Cls.ppDropDownList.SelectedValue = "0" And txtName.ppTextBox.Text = "" Then
            txtName.psSet_ErrorNo("5003", "TBOXID")
        End If
        '社員
        If ddlJudge_Cls.ppDropDownList.SelectedValue = "1" And ddlEmployee.ppDropDownList.SelectedValue = "" Then
            ddlEmployee.psSet_ErrorNo("5003", "社員")
        End If

        'TBOXID
        If ddlJudge_Cls.ppSelectedValue = "0" Then
            If Not clsDataConnect.pfOpen_Database(objCn) Then
                clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else
                Try
                    '                ddlHall.SelectedIndex = -1
                    lblHall.Text = ""
                    objCmd = New SqlCommand("COMUPDM19_S5", objCn)
                    objCmd.Parameters.Add(pfSet_Param("prmTBOXID", SqlDbType.NVarChar, txtName.ppText))

                    'データ取得
                    objDs = clsDataConnect.pfGet_DataSet(objCmd)

                    Me.lblHall.Text = ""
                    If objDs.Tables.Count > 0 Then
                        If objDs.Tables(0).Rows.Count > 0 Then
                            blnExistence = True
                        End If
                    End If
                    If blnExistence = False Then
                        Me.txtName.psSet_ErrorNo("2002", "TBOXID")
                    End If

                Catch ex As Exception
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Finally
                    Call mclsDB.psDisposeDataSet(objDs)
                    'DB切断
                    If Not clsDataConnect.pfClose_Database(objCn) Then
                    End If
                End Try
            End If
        End If


    End Sub

    ''' <summary>
    ''' 入力チェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msKeyCheck()

        Dim arySPTELNO As String() = Nothing
        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        Dim blnExistence As Boolean = False

        'TEL
        If txtTelNo.ppText.Trim <> String.Empty Then
            '数字のみでチェック
            If Not Regex.IsMatch(txtTelNo.ppText.Trim, "^[0-9]+$") Then
                '特殊電話番号の取得
                If mfGet_SPTELNOXml(arySPTELNO) = True Then
                    '特殊電話番号取得成功
                    Dim index1 As Integer = Array.IndexOf(arySPTELNO, txtTelNo.ppText.Trim)
                    '特殊電話番号を一致しなければエラー
                    If index1 = -1 Then
                        txtTelNo.psSet_ErrorNo("4001", "電話番号", "半角数字")
                    Else
                        '特殊電話番号と一致して数字、ハイフン以外以外があったらエラー
                        If Not Regex.IsMatch(txtTelNo.ppText.Trim, "^[-.0-9]+$") Then
                            txtTelNo.psSet_ErrorNo("4001", "電話番号", "半角数字と半角ハイフン")
                        End If
                    End If
                    '特殊電話番号の取得に失敗し、数字以外が使われていたらエラー
                Else
                    txtTelNo.psSet_ErrorNo("4001", "電話番号", "半角数字")
                End If
            End If
        End If
        '区分
        If ddlJudge_Cls.ppSelectedValue = "" Then
        Else
            If ddlJudge_Cls.ppDropDownList.SelectedValue = "0" Or ddlJudge_Cls.ppDropDownList.SelectedValue = "1" Then
            Else
                ddlJudge_Cls.psSet_ErrorNo("5003", "区分")
            End If
        End If

    End Sub


    ''' <summary>
    ''' 検索項目入力チェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSrchCheck()

        Dim arySPTELNO As String() = Nothing

        'TEL
        'If txtSrch_TelNo.ppText.Trim <> String.Empty Then
        '    If Not Regex.IsMatch(txtSrch_TelNo.ppText.Trim, "^[0-9]+$") Then
        '        txtSrch_TelNo.psSet_ErrorNo("4001", "電話番号", "正しい形式")
        '    End If
        'End If

        'TEL
        If txtSrch_TelNo.ppText.Trim <> String.Empty Then
            '数字のみでチェック
            If Not Regex.IsMatch(txtSrch_TelNo.ppText.Trim, "^[0-9]+$") Then
                '特殊電話番号の取得
                If mfGet_SPTELNOXml(arySPTELNO) = True Then
                    '特殊電話番号取得成功
                    Dim index1 As Integer = Array.IndexOf(arySPTELNO, txtSrch_TelNo.ppText.Trim)
                    '特殊電話番号と一致しなければエラー
                    If index1 = -1 Then
                        txtSrch_TelNo.psSet_ErrorNo("4001", "電話番号", "半角数字")
                    Else
                        '特殊電話番号と一致して数字、ハイフン以外以外があったらエラー
                        If Not Regex.IsMatch(txtSrch_TelNo.ppText.Trim, "^[-.0-9]+$") Then
                            txtSrch_TelNo.psSet_ErrorNo("4001", "電話番号", "正しい形式")
                        End If
                    End If
                    '特殊電話番号の取得に失敗し、数字以外が使われていたらエラー
                Else
                    txtSrch_TelNo.psSet_ErrorNo("4001", "電話番号", "半角数字")
                End If
            End If
        End If

        'TBOXFROM
        If ddlSrch_JudgeCls.ppDropDownList.SelectedValue = "0" And txtSrch_tboxidFromTo.ppTextBoxFrom.Text.Trim <> String.Empty Then
            '数字のみでチェック
            If Not Regex.IsMatch(txtSrch_tboxidFromTo.ppTextBoxFrom.Text.Trim, "^[0-9]+$") Then
                txtSrch_tboxidFromTo.psSet_ErrorNo("4001", "TBOXID", "半角数字")
            End If
        End If
        If ddlSrch_JudgeCls.ppDropDownList.SelectedValue = "0" And txtSrch_tboxidFromTo.ppTextBoxTo.Text.Trim <> String.Empty Then
            '数字のみでチェック
            If Not Regex.IsMatch(txtSrch_tboxidFromTo.ppTextBoxTo.Text.Trim, "^[0-9]+$") Then
                txtSrch_tboxidFromTo.psSet_ErrorNo("4001", "TBOXID", "半角数字")
            End If
        End If
        'TBOXTO
        If ddlSrch_JudgeCls.ppDropDownList.SelectedValue = "0" And txtSrch_tboxidFromTo.ppTextBoxFrom.Text.Trim <> "" And txtSrch_tboxidFromTo.ppTextBoxFrom.Text.Trim.Length <> 8 Then
            '数字のみでチェック
            txtSrch_tboxidFromTo.psSet_ErrorNo("3001", "TBOXID", "8")
        End If

        If ddlSrch_JudgeCls.ppDropDownList.SelectedValue = "0" And txtSrch_tboxidFromTo.ppTextBoxTo.Text.Trim <> "" And txtSrch_tboxidFromTo.ppTextBoxTo.Text.Trim.Length <> 8 Then
            '数字のみでチェック
            txtSrch_tboxidFromTo.psSet_ErrorNo("3001", "TBOXID", "8")
        End If
        '社員FROM
        If ddlSrch_JudgeCls.ppDropDownList.SelectedValue = "1" And txtSrch_tboxidFromTo.ppTextBoxFrom.Text.Trim <> String.Empty Then
            '数字のみでチェック
            If Not Regex.IsMatch(txtSrch_tboxidFromTo.ppTextBoxFrom.Text.Trim, "^[0-9]+$") Then
                txtSrch_tboxidFromTo.psSet_ErrorNo("4001", "社員コード", "半角数字")
            End If
        End If
        '社員TO
        If ddlSrch_JudgeCls.ppDropDownList.SelectedValue = "1" And txtSrch_tboxidFromTo.ppTextBoxTo.Text.Trim <> String.Empty Then
            '数字のみでチェック
            If Not Regex.IsMatch(txtSrch_tboxidFromTo.ppTextBoxTo.Text.Trim, "^[0-9]+$") Then
                txtSrch_tboxidFromTo.psSet_ErrorNo("4001", "社員コード", "半角数字")
            End If
        End If
        'FROMTO
        If ddlSrch_JudgeCls.ppDropDownList.SelectedValue = "0" Then
            If txtSrch_tboxidFromTo.ppTextBoxFrom.Text <> "" And txtSrch_tboxidFromTo.ppTextBoxTo.Text <> "" Then
                If txtSrch_tboxidFromTo.ppTextBoxFrom.Text > txtSrch_tboxidFromTo.ppTextBoxTo.Text Then
                    txtSrch_tboxidFromTo.psSet_ErrorNo("2021", "TBOXID")
                End If
            End If
        ElseIf ddlSrch_JudgeCls.ppDropDownList.SelectedValue = "1" Then
            If txtSrch_tboxidFromTo.ppTextBoxFrom.Text <> "" And txtSrch_tboxidFromTo.ppTextBoxTo.Text <> "" Then
                If txtSrch_tboxidFromTo.ppTextBoxFrom.Text > txtSrch_tboxidFromTo.ppTextBoxTo.Text Then
                    txtSrch_tboxidFromTo.psSet_ErrorNo("2021", "社員コード")
                End If
            End If
        End If

    End Sub


    ''' <summary>
    ''' データ取得およびバインド
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data(Optional ByVal ipstrFlg As String = "NML")

        Dim objSQLCmd As New SqlClient.SqlCommand
        Dim objwkDS As New DataSet
        objStack = New StackFrame

        Try
            '画面ページ表示初期化
            Master.ppCount = "0"
            '            Me.grvList.DataSource = Nothing
            '            grvList.DataBind()

            objSQLCmd.Connection = mclsDB.mobjDB
            objSQLCmd.CommandText = "COMUPDM19_S1"
            objSQLCmd.Parameters.Add("prmTELNO", SqlDbType.NVarChar)
            objSQLCmd.Parameters.Add("prmDECISION_CLS", SqlDbType.NVarChar)
            objSQLCmd.Parameters.Add("prmTBOXIDFm", SqlDbType.NVarChar)
            objSQLCmd.Parameters.Add("prmTBOXIDTo", SqlDbType.NVarChar)
            objSQLCmd.Parameters.Add("prmHALL", SqlDbType.NVarChar)
            objSQLCmd.Parameters.Add("prmOPERATE", SqlDbType.NVarChar)
            objSQLCmd.Parameters.Add("prmCOMPCD", SqlDbType.NVarChar)
            objSQLCmd.Parameters.Add("prmprog_id", SqlDbType.NVarChar)
            'Dim strprmValue As String = ""
            'If ViewState("ddlSrchAPPACLASS_CD") Is Nothing Then
            '    strprmValue = ""
            'ElseIf ViewState("ddlSrchAPPACLASS_CD") = "" Then
            '    strprmValue = ""
            'Else
            '    strprmValue = ViewState("ddlSrchAPPACLASS_CD")
            'End If
            'objSQLCmd.Parameters("prmAPPACLASS_CD").Value = strprmValue
            'strprmValue = ""
            'If ViewState("txtSrchAPPACLS_NM") Is Nothing Then
            '    strprmValue = ""
            'ElseIf ViewState("txtSrchAPPACLS_NM") = "" Then
            '    strprmValue = ""
            'Else
            '    strprmValue = ViewState("txtSrchAPPACLS_NM")
            'End If
            'objSQLCmd.Parameters("prmAPPACLS_NM").Value = strprmValue
            'If mstrDispMode = "FIRST" Then
            '    objSQLCmd.Parameters("prmAPPACLASS_CD").Value = ""
            '    objSQLCmd.Parameters("prmAPPACLS_NM").Value = ""
            'End If
            'If ViewState("ppchksDel") = True Then
            '    objSQLCmd.Parameters("prmppchksDel").Value = "1"
            'ElseIf ViewState("ppchksDel") = False Then
            '    objSQLCmd.Parameters("prmppchksDel").Value = "0"
            'Else
            '    objSQLCmd.Parameters("prmppchksDel").Value = "0"
            'End If

            If ViewState("txtSrch_TelNo") Is Nothing Then
                objSQLCmd.Parameters("prmTELNO").Value = ""
            Else
                objSQLCmd.Parameters("prmTELNO").Value = ViewState("txtSrch_TelNo")
            End If
            If ViewState("ddlSrch_JudgeCls") Is Nothing Then
                objSQLCmd.Parameters("prmDECISION_CLS").Value = ""
            Else
                objSQLCmd.Parameters("prmDECISION_CLS").Value = ViewState("ddlSrch_JudgeCls")
            End If
            If ViewState("txtSrch_tboxidFrom") Is Nothing Then
                objSQLCmd.Parameters("prmTBOXIDFm").Value = ""
            Else
                objSQLCmd.Parameters("prmTBOXIDFm").Value = ViewState("txtSrch_tboxidFrom")
            End If
            If ViewState("txtSrch_tboxidTo") Is Nothing Then
                objSQLCmd.Parameters("prmTBOXIDTo").Value = ""
            Else
                objSQLCmd.Parameters("prmTBOXIDTo").Value = ViewState("txtSrch_tboxidTo")
            End If
            If ViewState("txtSrch_Hall") Is Nothing Then
                objSQLCmd.Parameters("prmHALL").Value = ""
            Else
                objSQLCmd.Parameters("prmHALL").Value = ViewState("txtSrch_Hall")
            End If
            If ViewState("ddlSrchOperate") Is Nothing Then
                objSQLCmd.Parameters("prmOPERATE").Value = ""
            Else
                objSQLCmd.Parameters("prmOPERATE").Value = ViewState("ddlSrchOperate")
            End If
            If ViewState("ddlSrch_compcd") Is Nothing Then
                objSQLCmd.Parameters("prmCOMPCD").Value = ""
            Else
                objSQLCmd.Parameters("prmCOMPCD").Value = ViewState("ddlSrch_compcd")
            End If

            'If ViewState("ddlDel") = "1" Then
            '    objSQLCmd.Parameters("prmppchksDel").Value = "1"
            'ElseIf ViewState("ddlDel") = "0" Then
            '    objSQLCmd.Parameters("prmppchksDel").Value = "0"
            'Else
            '    objSQLCmd.Parameters("prmppchksDel").Value = ""
            'End If
            objSQLCmd.Parameters("prmprog_id").Value = DispCode
            If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
                'エラー
                clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "リスト作成")
                objStack = New StackFrame
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "リスト情報取得失敗", "Catch")
                Exit Sub
            End If

            Master.ppCount = 0

            If ipstrFlg = "NML" Then
                If objwkDS.Tables.Count > 0 Then
                    If objwkDS.Tables(0).Rows.Count > 0 Then
                        '件数を設定
                        Master.ppCount = objwkDS.Tables(0).Rows(0).Item("総件数").ToString
                        '閾値を超えた場合はメッセージを表示
                        If objwkDS.Tables(0).Rows(0).Item("総件数") > objwkDS.Tables(0).Rows(0).Item("最大表示") Then
                            psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, objwkDS.Tables(0).Rows(0).Item("最大表示").ToString.Trim, objwkDS.Tables(0).Rows(0).Item("総件数").ToString.Trim)
                        End If
                    Else
                        '0件
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        Master.ppCount = 0
                    End If
                    Me.grvList.DataSource = objwkDS.Tables(0)
                    Me.grvList.DataBind()
                Else
                    Dim objWKDT As New DataTable
                    Call DumTbl_Create(objWKDT)
                    Me.grvList.DataSource = objWKDT
                    '                    Me.grvList.DataSource = New DataTable
                    Me.grvList.DataBind()
                    Master.ppCount = 0
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "電話場号マスタ")
                End If
            Else
                Master.ppCount = 0
                Dim objWKDT As New DataTable
                Call DumTbl_Create(objWKDT)
                Me.grvList.DataSource = objWKDT
                Me.grvList.DataBind()
            End If

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "電話場号マスタ")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ipmstrDispMode"></param>
    ''' <remarks></remarks>
    Private Sub msEditData(ByVal ipmstrDispMode As String)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim MesCode As String = String.Empty
        Dim dttGrid As New DataTable
        Dim strStored As String = String.Empty
        Dim getFlg As String = "0"
        Dim objDBTran As SqlClient.SqlTransaction = mclsDB.mobjDB.BeginTransaction
        objStack = New StackFrame

        Select Case ipmstrDispMode
            Case "INSERT"
                MesCode = "00003"
                strStored = "COMUPDM19_I1"
            Case "UPDATE"
                MesCode = "00001"
                strStored = "COMUPDM19_U1"
            Case "DELETE"
                Select Case Master.ppBtnDelete.Text
                    Case "削除"
                        MesCode = "00002"
                        strStored = "COMUPDM19_U2"
                        getFlg = "1"
                    Case "削除取消"
                        MesCode = "00001"
                        strStored = "COMUPDM19_U2"
                        getFlg = "0"
                End Select
        End Select

        Try
            ''INSERTの場合、更新するかのチェックを行う
            'If ipmstrDispMode = "INSERT" Then
            '    cmdDB = New SqlCommand(DispCode & "_S3", conDB)
            '    cmdDB.Parameters.Add(pfSet_Param("MainteCd", SqlDbType.NVarChar, Me.txtMainteCd.ppText))       '保担コード
            '    'データ取得およびデータをリストに設定
            '    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            '    If dstOrders.Tables(0).Rows.Count > 0 Then
            '        'psMesBox(Me, "30008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
            '        'Exit Sub
            '    End If
            'End If

            Dim objSQLCmd As New SqlClient.SqlCommand
            Dim intRetCD As Integer = 0
            Dim strMesType As String = "00001"
            Dim strwkBuff As String = ""
            Dim strJudgeCls As String = ""

            objSQLCmd.Connection = mclsDB.mobjDB
            objSQLCmd.CommandText = strStored

            Select Case ipmstrDispMode
                Case "INSERT"
                    objSQLCmd.Parameters.Add("prmM19_TELNO", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM19_COMP_CD", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM19_HALL_CD", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM19_EMPLOYEE_CD", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM19_DECISION_CLS", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM19_DELETE_FLG", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM19_INSERT_USR", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM19_OLDTELNO", SqlDbType.NVarChar)

                    strwkBuff = ""
                    If ViewState("txtTelNo") Is Nothing Then
                        strwkBuff = ""
                    Else
                        strwkBuff = ViewState("txtTelNo").ToString.Trim
                    End If
                    objSQLCmd.Parameters("prmM19_TELNO").Value = strwkBuff
                    strwkBuff = ""
                    If ViewState("txtOldTelNo") Is Nothing Then
                        strwkBuff = ""
                    Else
                        strwkBuff = ViewState("txtOldTelNo").ToString.Trim
                    End If
                    objSQLCmd.Parameters("prmM19_OLDTELNO").Value = strwkBuff
                    If ViewState("ddlJudge_Cls") Is Nothing Then
                        '社員、ホールの区別がない場合は処理不能の為エラー
                        '失敗メッセージ表示
                        psMesBox(Me, strMesType, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                        objSQLCmd.Transaction.Rollback()
                        Exit Sub
                    ElseIf ViewState("ddlJudge_Cls").ToString = "" Then
                        '社員、ホールの区別がない場合は処理不能の為エラー
                        '失敗メッセージ表示
                        psMesBox(Me, strMesType, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                        objSQLCmd.Transaction.Rollback()
                        Exit Sub
                    Else
                        strJudgeCls = ViewState("ddlJudge_Cls").ToString
                    End If
                    objSQLCmd.Parameters("prmM19_DECISION_CLS").Value = strJudgeCls

                    If strJudgeCls = "0" Then 'ホール
                        strwkBuff = ""
                        'If ViewState("ddlHall") Is Nothing Then
                        '    strwkBuff = ""
                        'Else
                        '    strwkBuff = ViewState("ddlHall").ToString.Trim
                        'End If
                        If ViewState("lblHall") Is Nothing Then
                            strwkBuff = ""
                        Else
                            strwkBuff = ViewState("lblHall").ToString.Trim
                            If strwkBuff = "" Then
                            Else
                                Dim aryBuff As String()
                                aryBuff = strwkBuff.Trim.Split(":")
                                strwkBuff = aryBuff(0)
                            End If
                        End If
                        objSQLCmd.Parameters("prmM19_HALL_CD").Value = strwkBuff
                        objSQLCmd.Parameters("prmM19_COMP_CD").Value = ""
                        objSQLCmd.Parameters("prmM19_EMPLOYEE_CD").Value = ""
                    ElseIf strJudgeCls = "1" Then '社員
                        '会社コード
                        strwkBuff = ""
                        If ViewState("ddlCompCD") Is Nothing Then
                            strwkBuff = ""
                        Else
                            strwkBuff = ViewState("ddlCompCD").ToString.Trim
                        End If
                        '                        objSQLCmd.Parameters("prmM19_COMP_CD").Value = strwkBuff
                        objSQLCmd.Parameters("prmM19_COMP_CD").Value = "701" 'strwkBuff
                        '社員コード
                        strwkBuff = ""
                        If ViewState("ddlEmployee") Is Nothing Then
                            strwkBuff = ""
                        Else
                            strwkBuff = ViewState("ddlEmployee").ToString.Trim
                        End If
                        objSQLCmd.Parameters("prmM19_EMPLOYEE_CD").Value = strwkBuff
                        objSQLCmd.Parameters("prmM19_HALL_CD").Value = ""
                    Else 'ホール、社員以外
                        strwkBuff = ""
                        'If ViewState("ddlHall") Is Nothing Then
                        '    strwkBuff = ""
                        'Else
                        '    strwkBuff = ViewState("ddlHall").ToString.Trim
                        'End If
                        If ViewState("lblHall") Is Nothing Then
                            strwkBuff = ""
                        Else
                            strwkBuff = ViewState("lblHall").ToString.Trim
                            If strwkBuff = "" Then
                            Else
                                Dim aryBuff As String()
                                aryBuff = strwkBuff.Trim.Split(":")
                                strwkBuff = aryBuff(0)
                            End If
                        End If
                        objSQLCmd.Parameters("prmM19_HALL_CD").Value = strwkBuff
                        strwkBuff = ""
                        If ViewState("txtComp") Is Nothing Then
                            strwkBuff = ""
                        Else
                            strwkBuff = ViewState("txtComp").ToString.Trim
                        End If
                        '                        objSQLCmd.Parameters("prmM19_COMP_CD").Value = strwkBuff
                        objSQLCmd.Parameters("prmM19_COMP_CD").Value = "701" 'strwkBuff
                        strwkBuff = ""
                        If ViewState("ddlEmployee") Is Nothing Then
                            strwkBuff = ""
                        Else
                            strwkBuff = ViewState("ddlEmployee").ToString.Trim
                        End If
                        objSQLCmd.Parameters("prmM19_EMPLOYEE_CD").Value = strwkBuff
                    End If
                    objSQLCmd.Parameters("prmM19_DELETE_FLG").Value = "0"
                    objSQLCmd.Parameters("prmM19_INSERT_USR").Value = User.Identity.Name

                Case "UPDATE"
                    objSQLCmd.Parameters.Add("prmM19_TELNO", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM19_COMP_CD", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM19_HALL_CD", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM19_EMPLOYEE_CD", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM19_DECISION_CLS", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM19_DELETE_FLG", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM19_UPDATE_USR", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM19_OLDTELNO", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM19_OLDHALL_CD", SqlDbType.NVarChar)

                    strwkBuff = ""
                    If ViewState("txtTelNo") Is Nothing Then
                        strwkBuff = ""
                    Else
                        strwkBuff = ViewState("txtTelNo").ToString.Trim
                    End If
                    objSQLCmd.Parameters("prmM19_TELNO").Value = strwkBuff
                    strwkBuff = ""
                    If ViewState("txtOldTelNo") Is Nothing Then
                        strwkBuff = ""
                    Else
                        strwkBuff = ViewState("txtOldTelNo").ToString.Trim
                    End If
                    objSQLCmd.Parameters("prmM19_OLDTELNO").Value = strwkBuff

                    If ViewState("ddlJudge_Cls") Is Nothing Then
                        '社員、ホールの区別がない場合は処理不能の為エラー
                        '失敗メッセージ表示
                        psMesBox(Me, strMesType, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                        objSQLCmd.Transaction.Rollback()
                        Exit Sub
                    ElseIf ViewState("ddlJudge_Cls").ToString = "" Then
                        '社員、ホールの区別がない場合は処理不能の為エラー
                        '失敗メッセージ表示
                        psMesBox(Me, strMesType, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                        objSQLCmd.Transaction.Rollback()
                        Exit Sub
                    Else
                        strJudgeCls = ViewState("ddlJudge_Cls").ToString
                    End If
                    objSQLCmd.Parameters("prmM19_DECISION_CLS").Value = strJudgeCls

                    If strJudgeCls = "0" Then 'ホール
                        strwkBuff = ""
                        'If ViewState("ddlHall") Is Nothing Then
                        '    strwkBuff = ""
                        'Else
                        '    strwkBuff = ViewState("ddlHall").ToString.Trim
                        'End If
                        If ViewState("lblHall") Is Nothing Then
                            strwkBuff = ""
                        Else
                            strwkBuff = ViewState("lblHall").ToString.Trim
                            If strwkBuff = "" Then
                            Else
                                Dim aryBuff As String()
                                aryBuff = strwkBuff.Trim.Split(":")
                                strwkBuff = aryBuff(0)
                            End If
                        End If
                        objSQLCmd.Parameters("prmM19_HALL_CD").Value = strwkBuff
                        strwkBuff = ""
                        If ViewState("txtOldHallCD") Is Nothing Then
                            strwkBuff = ""
                        Else
                            strwkBuff = ViewState("txtOldHallCD").ToString.Trim
                            If strwkBuff = "" Then
                            Else
                                Dim aryBuff As String()
                                aryBuff = strwkBuff.Trim.Split(":")
                                strwkBuff = aryBuff(0)
                            End If
                        End If
                        objSQLCmd.Parameters("prmM19_OLDHALL_CD").Value = strwkBuff
                        objSQLCmd.Parameters("prmM19_COMP_CD").Value = ""
                        objSQLCmd.Parameters("prmM19_EMPLOYEE_CD").Value = ""
                    ElseIf strJudgeCls = "1" Then '社員
                        '会社コード
                        strwkBuff = ""
                        If ViewState("ddlCompCD") Is Nothing Then
                            strwkBuff = ""
                        Else
                            strwkBuff = ViewState("ddlCompCD").ToString.Trim
                        End If
                        '                        objSQLCmd.Parameters("prmM19_COMP_CD").Value = strwkBuff
                        objSQLCmd.Parameters("prmM19_COMP_CD").Value = "701" 'strwkBuff
                        '社員コード
                        strwkBuff = ""
                        If ViewState("ddlEmployee") Is Nothing Then
                            strwkBuff = ""
                        Else
                            strwkBuff = ViewState("ddlEmployee").ToString.Trim
                        End If
                        objSQLCmd.Parameters("prmM19_EMPLOYEE_CD").Value = strwkBuff
                        objSQLCmd.Parameters("prmM19_HALL_CD").Value = ""
                        objSQLCmd.Parameters("prmM19_OLDHALL_CD").Value = ""
                    Else 'ホール、社員以外
                        strwkBuff = ""
                        If ViewState("ddlHall") Is Nothing Then
                            strwkBuff = ""
                        Else
                            strwkBuff = ViewState("ddlHall").ToString.Trim
                        End If
                        objSQLCmd.Parameters("prmM19_HALL_CD").Value = strwkBuff
                        strwkBuff = ""
                        If ViewState("txtOldHallCD") Is Nothing Then
                            strwkBuff = ""
                        Else
                            strwkBuff = ViewState("txtOldHallCD").ToString.Trim
                            If strwkBuff = "" Then
                            Else
                                Dim aryBuff As String()
                                aryBuff = strwkBuff.Trim.Split(":")
                                strwkBuff = aryBuff(0)
                            End If
                        End If
                        objSQLCmd.Parameters("prmM19_OLDHALL_CD").Value = strwkBuff
                        strwkBuff = ""
                        If ViewState("txtComp") Is Nothing Then
                            strwkBuff = ""
                        Else
                            strwkBuff = ViewState("txtComp").ToString.Trim
                        End If
                        '                        objSQLCmd.Parameters("prmM19_COMP_CD").Value = strwkBuff
                        objSQLCmd.Parameters("prmM19_COMP_CD").Value = "701" 'strwkBuff
                        strwkBuff = ""
                        If ViewState("ddlEmployee") Is Nothing Then
                            strwkBuff = ""
                        Else
                            strwkBuff = ViewState("ddlEmployee").ToString.Trim
                        End If
                        objSQLCmd.Parameters("prmM19_EMPLOYEE_CD").Value = strwkBuff
                    End If
                    objSQLCmd.Parameters("prmM19_DELETE_FLG").Value = "0"
                    objSQLCmd.Parameters("prmM19_UPDATE_USR").Value = User.Identity.Name

                Case "DELETE"
                    Select Case Master.ppBtnDelete.Text
                        Case "削除"
                            objSQLCmd.Parameters.Add("prmM19_TELNO", SqlDbType.NVarChar)
                            objSQLCmd.Parameters.Add("prmM19_DELETE_FLG", SqlDbType.NVarChar)
                            objSQLCmd.Parameters.Add("prmM19_UPDATE_USR", SqlDbType.NVarChar)
                            objSQLCmd.Parameters.Add("prmM19_OLDTELNO", SqlDbType.NVarChar)
                            objSQLCmd.Parameters.Add("prmM19_OLDHALL_CD", SqlDbType.NVarChar)

                            strwkBuff = ""
                            If ViewState("txtTelNo") Is Nothing Then
                                strwkBuff = ""
                            Else
                                strwkBuff = ViewState("txtTelNo").ToString.Trim
                            End If
                            objSQLCmd.Parameters("prmM19_TELNO").Value = strwkBuff
                            strwkBuff = ""
                            If ViewState("txtOldTelNo") Is Nothing Then
                                strwkBuff = ""
                            Else
                                strwkBuff = ViewState("txtOldTelNo").ToString.Trim
                            End If
                            objSQLCmd.Parameters("prmM19_OLDTELNO").Value = strwkBuff
                            strwkBuff = ""
                            If ViewState("txtOldHallCD") Is Nothing Then
                                strwkBuff = ""
                            Else
                                strwkBuff = ViewState("txtOldHallCD").ToString.Trim
                                If strwkBuff = "" Then
                                Else
                                    Dim aryBuff As String()
                                    aryBuff = strwkBuff.Trim.Split(":")
                                    strwkBuff = aryBuff(0)
                                End If
                            End If
                            objSQLCmd.Parameters("prmM19_OLDHALL_CD").Value = strwkBuff
                            objSQLCmd.Parameters("prmM19_DELETE_FLG").Value = "1"
                            objSQLCmd.Parameters("prmM19_UPDATE_USR").Value = User.Identity.Name

                        Case "削除取消"
                            objSQLCmd.Parameters.Add("prmM19_TELNO", SqlDbType.NVarChar)
                            objSQLCmd.Parameters.Add("prmM19_DELETE_FLG", SqlDbType.NVarChar)
                            objSQLCmd.Parameters.Add("prmM19_UPDATE_USR", SqlDbType.NVarChar)
                            objSQLCmd.Parameters.Add("prmM19_OLDTELNO", SqlDbType.NVarChar)

                            strwkBuff = ""
                            If ViewState("txtTelNo") Is Nothing Then
                                strwkBuff = ""
                            Else
                                strwkBuff = ViewState("txtTelNo").ToString.Trim
                            End If
                            objSQLCmd.Parameters("prmM19_TELNO").Value = strwkBuff
                            strwkBuff = ""
                            If ViewState("txtOldTelNo") Is Nothing Then
                                strwkBuff = ""
                            Else
                                strwkBuff = ViewState("txtOldTelNo").ToString.Trim
                            End If
                            objSQLCmd.Parameters("prmM19_OLDTELNO").Value = strwkBuff
                            objSQLCmd.Parameters("prmM19_DELETE_FLG").Value = "0"
                            objSQLCmd.Parameters("prmM19_UPDATE_USR").Value = User.Identity.Name

                    End Select
            End Select

            Try
                objSQLCmd.Transaction = objDBTran

                'SQL実行
                If mclsDB.pfDB_ProcStored(objSQLCmd) Then
                Else
                    '失敗メッセージ表示
                    psMesBox(Me, strMesType, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                    objSQLCmd.Transaction.Rollback()
                    Exit Sub
                End If

                'コミット
                objSQLCmd.Transaction.Commit()

                Dim objwkDS As New DataSet
                objSQLCmd = Nothing
                objSQLCmd = New SqlClient.SqlCommand

                '追加、更新の場合は対象のレコードのみグリッドに表示
                'If ipstrMode = "INSERT" OrElse ipstrMode = "UPDATE" Then
                If ipmstrDispMode = "SELECT" OrElse ipmstrDispMode = "INSERT" OrElse ipmstrDispMode = "UPDATE" OrElse ipmstrDispMode = "GENERAL" OrElse Master.ppBtnDelete.Text = "削除取消" Then
                    objSQLCmd.Connection = mclsDB.mobjDB
                    objSQLCmd.CommandText = "COMUPDM19_S1"
                    With objSQLCmd.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("prmTELNO", SqlDbType.NVarChar, txtTelNo.ppText.Trim))
                        .Add(pfSet_Param("prmDECISION_CLS", SqlDbType.NVarChar, ""))
                        .Add(pfSet_Param("prmTBOXIDFm", SqlDbType.NVarChar, ""))
                        .Add(pfSet_Param("prmTBOXIDTo", SqlDbType.NVarChar, ""))
                        .Add(pfSet_Param("prmHALL", SqlDbType.NVarChar, ""))
                        .Add(pfSet_Param("prmOPERATE", SqlDbType.NVarChar, ""))
                        .Add(pfSet_Param("prmCOMPCD", SqlDbType.NVarChar, ""))
                        .Add(pfSet_Param("prmprog_id", SqlDbType.NVarChar, DispCode))
                        .Add(pfSet_Param("@prmPROCFLG", SqlDbType.NVarChar, "2"))
                    End With
                    'リストデータ取得
                    If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
                        'エラー
                        clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "リスト作成")
                        objStack = New StackFrame
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "リスト情報取得失敗", "Catch")
                        Exit Sub
                    End If
                    grvList.DataSource = objwkDS.Tables(0)
                    grvList.DataBind()
                    Master.ppCount = objwkDS.Tables(0).Rows.Count
                Else
                    Dim objWKDT As New DataTable
                    Call DumTbl_Create(objWKDT)
                    grvList.DataSource = objWKDT
                    '                    dttGrid = New DataTable
                    '                    grvList.DataSource = dttGrid
                    grvList.DataBind()
                    Master.ppCount = dttGrid.Rows.Count
                End If

                '                Call msGet_Data()

                'Try

            Catch ex As Exception

                '失敗メッセージ表示
                psMesBox(Me, strMesType, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)

            End Try

            'トランザクションが残ってたらロールバック
            If objSQLCmd.Transaction Is Nothing Then
            Else
                objSQLCmd.Transaction.Rollback()
            End If

            psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName)
            Call btnClear_Click()

        Catch ex As Exception
            psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
        End Try

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（表示用判定分類）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlLineCls()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZMSTSEL005", objCn)

                With objCmd.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("Class", SqlDbType.NVarChar, "0148"))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                Me.ddlSrch_JudgeCls.ppDropDownList.Items.Clear()
                Me.ddlSrch_JudgeCls.ppDropDownList.DataSource = objDs.Tables(0)
                Me.ddlSrch_JudgeCls.ppDropDownList.DataTextField = "リスト用"
                Me.ddlSrch_JudgeCls.ppDropDownList.DataValueField = "コード"
                Me.ddlSrch_JudgeCls.ppDropDownList.DataBind()
                Me.ddlSrch_JudgeCls.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

                Me.ddlJudge_Cls.ppDropDownList.Items.Clear()
                Me.ddlJudge_Cls.ppDropDownList.DataSource = objDs.Tables(0)
                Me.ddlJudge_Cls.ppDropDownList.DataTextField = "リスト用"
                Me.ddlJudge_Cls.ppDropDownList.DataValueField = "コード"
                Me.ddlJudge_Cls.ppDropDownList.DataBind()
                Me.ddlJudge_Cls.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "表示用判定分類取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                Call mclsDB.psDisposeDataSet(objDs)
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（表示用会社）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlCompCD()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("COMUPDM19_S2", objCn)
                '                objCmd.Parameters.Add(pfSet_Param("disp_cls", SqlDbType.NVarChar, Request.QueryString.Get("page").ToString))         '画面区分（1:社員マスタ 2:他社マスタ）
                objCmd.Parameters.Add(pfSet_Param("disp_cls", SqlDbType.NVarChar, "1")) '画面区分（1:社員マスタ 2:他社マスタ）
                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                'Me.ddlSrch_compcd.ppDropDownList.Items.Clear()
                'Me.ddlSrch_compcd.ppDropDownList.DataSource = objDs.Tables(0)
                'Me.ddlSrch_compcd.ppDropDownList.DataTextField = "M44_COMP_NM"
                'Me.ddlSrch_compcd.ppDropDownList.DataValueField = "M44_COMP_CD"
                'Me.ddlSrch_compcd.ppDropDownList.DataBind()
                'Me.ddlSrch_compcd.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

                Me.ddlCompCD.ppDropDownList.Items.Clear()
                Me.ddlCompCD.ppDropDownList.DataSource = objDs.Tables(0)
                Me.ddlCompCD.ppDropDownList.DataTextField = "M44_COMP_NM"
                Me.ddlCompCD.ppDropDownList.DataValueField = "M44_COMP_CD"
                Me.ddlCompCD.ppDropDownList.DataBind()
                Me.ddlCompCD.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "表示用会社リスト取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                Call mclsDB.psDisposeDataSet(objDs)
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（表示用社員）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlSrchEMPLOYEE()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("COMUPDM19_S3", objCn)
                '                objCmd.Parameters.Add(pfSet_Param("prmCOMPCD", SqlDbType.NVarChar, Me.ddlSrch_compcd.ppSelectedValue))

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                'Me.ddlSrch_compcd.ppDropDownList.Items.Clear()
                'Me.ddlSrch_compcd.ppDropDownList.DataSource = objDs.Tables(0)
                'Me.ddlSrch_compcd.ppDropDownList.DataTextField = "M02_EMPLOYEE_NAME"
                'Me.ddlSrch_compcd.ppDropDownList.DataValueField = "M02_EMPLOYEE_CD"
                'Me.ddlSrch_compcd.ppDropDownList.DataBind()
                'Me.ddlSrch_compcd.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "表示用会社リスト取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                Call mclsDB.psDisposeDataSet(objDs)
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（表示用社員）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlEMPLOYEE()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("COMUPDM19_S3", objCn)
                objCmd.Parameters.Add(pfSet_Param("prmCOMPCD", SqlDbType.NVarChar, ViewState("ddlCompCD")))

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                Me.ddlEmployee.ppDropDownList.Items.Clear()
                Me.ddlEmployee.ppDropDownList.DataSource = objDs.Tables(0)
                Me.ddlEmployee.ppDropDownList.DataTextField = "M02_EMPLOYEE_NAME"
                Me.ddlEmployee.ppDropDownList.DataValueField = "M02_EMPLOYEE_CD"
                Me.ddlEmployee.ppDropDownList.DataBind()
                Me.ddlEmployee.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "表示用会社リスト取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                Call mclsDB.psDisposeDataSet(objDs)
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（運用状況）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlOparate()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("COMUPDM19_S6", objCn)
                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                Me.ddlSrchOperate.ppDropDownList.Items.Clear()
                Me.ddlSrchOperate.ppDropDownList.DataSource = objDs.Tables(0)
                Me.ddlSrchOperate.ppDropDownList.DataTextField = "M29_NAME"
                Me.ddlSrchOperate.ppDropDownList.DataValueField = "M29_CODE"
                Me.ddlSrchOperate.ppDropDownList.DataBind()
                Me.ddlSrchOperate.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "運用状況リスト取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                Call mclsDB.psDisposeDataSet(objDs)
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ホール名取得
    ''' </summary>
    ''' <param name="ipstrProc">書き換え対象　SRCH:検索／EDIT:編集</param>
    ''' <remarks></remarks>
    Private Sub msSetHallName(ByVal ipstrProc As String)

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        Dim strHallName As String = ""

        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("COMUPDM19_S4", objCn)
                objCmd.Parameters.Add(pfSet_Param("prmHALLCD", SqlDbType.NVarChar, ViewState("txtName")))

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                If objDs.Tables.Count > 0 Then
                    If objDs.Tables(0).Rows.Count > 0 Then
                        If objDs.Tables(0).Rows(0).Item("") Is DBNull.Value Then
                        Else
                            If ipstrProc = "SRCH" Then
                                strHallName = objDs.Tables(0).Rows(0).Item("T01_HALL_NAME").ToString
                            ElseIf ipstrProc = "EDIT" Then
                                '                                strHallName = objDs.Tables(0).Rows(0).Item("T01_HALL_CD").ToString
                                strHallName = objDs.Tables(0).Rows(0).Item("T01_HALL_NAME").ToString
                            End If
                        End If
                    End If
                End If

                If ipstrProc = "SRCH" Then
                    Me.txtSrch_Hall.ppText = strHallName
                ElseIf ipstrProc = "EDIT" Then
                    '                    Me.ddlHall.SelectedValue = strHallName
                    Me.lblHall.Text = strHallName.Replace(Environment.NewLine, "")
                    ViewState("lblHall") = lblHall.Text
                End If

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "表示用会社リスト取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                Call mclsDB.psDisposeDataSet(objDs)
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub


    ' ''' <summary>
    ' ''' ドロップダウンリスト設定　シリアル管理区分
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Sub sSetddlSEARIAL_CLS()

    '    Dim objSQLCmd As New SqlClient.SqlCommand
    '    Dim objwkDS As New DataSet

    '    objSQLCmd.Connection = mclsDB.mobjDB
    '    objSQLCmd.CommandText = "COMUPDM06_S3"
    '    If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
    '        'エラー
    '        clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "リスト作成")
    '        objStack = New StackFrame
    '        'ログ出力
    '        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "リスト情報取得失敗", "Catch")
    '        Exit Sub
    '    End If

    '    If objwkDS.Tables.Count > 0 Then
    '        '入力エリア項目
    '        Me.ddlSEARIAL_CLS.ppDropDownList.DataSource = objwkDS.Tables(0)
    '        Me.ddlSEARIAL_CLS.ppDropDownList.DataTextField = "M29_NAME_DISP"
    '        Me.ddlSEARIAL_CLS.ppDropDownList.DataValueField = "M29_CODE"
    '        Me.ddlSEARIAL_CLS.DataBind()
    '        Me.ddlSEARIAL_CLS.ppDropDownList.Items.Insert(0, "")
    '    End If

    '    Call mclsDB.psDisposeDataSet(objwkDS)

    'End Sub

    'フォーカス移動
    Private Sub FocusChange(ByVal TxtBoxF As TextBox, ByVal TxtBoxT As TextBox)

        Dim strBtnDmy As String = Master.ppBtnDmy.ClientID
        Master.ppBtnDmy.Visible = True

        Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + TxtBoxT.ClientID + ");"
        SetFocus(Master.ppBtnDmy.ClientID)

    End Sub
    'フォーカス移動
    Private Sub FocusChange(ByVal TxtBoxF As TextBox, ByVal DropDownT As DropDownList)

        Dim strBtnDmy As String = Master.ppBtnDmy.ClientID
        Master.ppBtnDmy.Visible = True

        Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + DropDownT.ClientID + ");"
        SetFocus(Master.ppBtnDmy.ClientID)

    End Sub

    Private Function mfGet_SPTELNOXml(ByRef oparySPTelNo As String()) As Boolean

        Dim dtsXml As DataSet = New DataSet()
        Dim dtrSelect() As DataRow
        Dim intTelNOCnt As Integer = 0
        Dim zz As Integer = 0
        Dim arySPTelNo As String() = Nothing

        mfGet_SPTELNOXml = False
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            dtsXml.ReadXml(HttpContext.Current.Server.MapPath("~/XML/COMUPDM19_SPTEL.xml"))

            With dtsXml.Tables(0)
                dtrSelect = .Select("No = 0")
                If dtrSelect.Length > 0 Then
                    intTelNOCnt = dtrSelect(0).Item(1)
                Else
                    intTelNOCnt = 0
                End If

                ReDim oparySPTelNo(intTelNOCnt - 1)
                ReDim arySPTelNo(intTelNOCnt - 1)

                For zz = 1 To intTelNOCnt
                    oparySPTelNo(zz - 1) = .Rows(zz).Item(2)
                    '                    arySPTelNo(zz - 1) = .Rows(zz).Item(2)
                Next

            End With

            '            System.Array.Copy(arySPTelNo, oparySPTelNo, arySPTelNo.Length)

            mfGet_SPTELNOXml = True

        Catch ex As Exception
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
                            objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
        End Try

    End Function

    Private Sub DumTbl_Create(ByRef cpobjWKDT As DataTable)

        cpobjWKDT = New DataTable("WKDT")

        cpobjWKDT.Columns.Add("連番  ", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("電話番号 ", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("会社コード", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("会社", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("ホール", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("ホールコード", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("ホール名", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("TBOXID", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("社員", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("社員名", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("社員コード", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("判定", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("判定区分", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("削除フラグ", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("削除", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("登録日時", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("M19_INSERT_USR", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("更新日時", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("M19_UPDATE_USR", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("運用状況コード", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("運用状況", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("件数 ", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("最大表示", Type.GetType("System.String"))
        cpobjWKDT.Columns.Add("総件数", Type.GetType("System.String"))

        cpobjWKDT.AcceptChanges()

    End Sub

    Private Sub msSerch_Proc()

        If Me.txtSrch_TelNo.ppText <> "" Or Me.ddlSrch_JudgeCls.ppSelectedValue <> "" Or Me.txtSrch_tboxidFromTo.ppTextBoxFrom.Text <> "" Or Me.txtSrch_tboxidFromTo.ppTextBoxTo.Text <> "" Or Me.txtSrch_Hall.ppText <> "" Then

            Call msSrchCheck()

            'データ取得
            If (Page.IsValid) Then

                'コントロールの制御
                Master.ppMainEnabled = True
                mstrDispMode = "DEFAULT"
                ViewState("mstrDispMode") = mstrDispMode

                msGet_Data()

            End If
        Else
            clsMst.psMesBox(Me, "30016", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "検索条件")
        End If

        'フォーカス移動
        SetFocus(Me.txtTelNo.ppTextBox.ClientID)

    End Sub

#End Region

#Region "終了処理プロシージャ"

#End Region

End Class