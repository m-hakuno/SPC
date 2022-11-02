'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　業者基本マスタ
'*　ＰＧＭＩＤ：　COMUPDMA1
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.10.22　：　栗原
'********************************************************************************************************************************

#Region "インポート定義"

Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive
Imports SQL_DBCLS_LIB

#End Region

Public Class COMUPDMA1

#Region "継承定義"

    Inherits System.Web.UI.Page

#End Region

#Region "定数定義"

    Const DispCode As String = "COMUPDMA1"                  '画面ID
    Const MasterName As String = "業者基本マスタ"           '画面名
    Const TableName As String = "MA1_TRADER_BASE"        'テーブル名

    '業者マスタ画面パス
    Const M_MST_DISP_PATH = "~/" & P_MST & "/" &
            P_FUN_COM & P_SCR_UPD & "M" & "44" & "/" &
            P_FUN_COM & P_SCR_UPD & "M" & "44.aspx"

#End Region

#Region "変数定義"

    Dim clsDataConnect As New ClsCMDataConnect
    Dim mclsDB As New ClsSQLSvrDB
    Dim clsExc As New ClsCMExclusive
    Dim objStack As StackFrame
    Dim strMode As String = Nothing
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
            Me.txtName.ppEnabled = value
            Me.txtZipNo1.ppEnabled = value
            Me.txtZipNo2.ppEnabled = value
            Me.ddlState.Enabled = value
            Me.txtAddr1.ppEnabled = value
            Me.txtAddr2.ppEnabled = value
            Me.txtAddr3.ppEnabled = value
            Me.txtTel1.ppEnabled = value
            Me.txtTel2.ppEnabled = value
            Me.txtTel3.ppEnabled = value
            Me.txtFax1.ppEnabled = value
            Me.txtFax2.ppEnabled = value
            Me.txtFax3.ppEnabled = value
            Me.txtEmTel1.ppEnabled = value
            Me.txtEmTel2.ppEnabled = value
            Me.txtEmTel3.ppEnabled = value
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
            Me.ddlTrd.ppDropDownList.Enabled = value
            Me.ddlPair.ppDropDownList.Enabled = value
            Me.txtCd.ppEnabled = value
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
        Dim intHeadCol As Integer() = New Integer() {1, 3, 5, 8}
        Dim intColSpan As Integer() = New Integer() {2, 2, 2, 2}
        pfSet_GridView(Me.grvList, DispCode, intHeadCol, intColSpan)

    End Sub

    ''' <summary>
    ''' Page Load
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        '遷移ボタン(業者マスタ)設定
        Dim btnFtM44 As Button = DirectCast(Master.FindControl("btnLeft1"), Button)
        btnFtM44.Visible = True
        btnFtM44.Text = "業者マスタ"
        '*** TODO 業者マスタ導入時Trueにする ***
        btnFtM44.Enabled = True
        '***************************************

        'ボタンアクション設定
        AddHandler Master.ppBtnSearch.Click, AddressOf btnSearch_Click          '検索
        AddHandler Master.ppBtnSrcClear.Click, AddressOf btnSearchClear_Click   '検索条件クリア
        AddHandler Master.ppBtnInsert.Command, AddressOf btn_Click              '登録
        AddHandler Master.ppBtnUpdate.Command, AddressOf btn_Click              '更新
        AddHandler Master.ppBtnDelete.Command, AddressOf btn_Click              '削除
        AddHandler Master.ppBtnClear.Click, AddressOf btnClear_Click            'クリア
        AddHandler btnFtM44.Click, AddressOf btnM44_Click            '画面遷移(業者マスタ)

        'テキスト・選択リスト　チェンジイベント
        AddHandler Me.ddlTrd.ppDropDownList.SelectedIndexChanged, AddressOf ddlTrd_SelectedIndexChanged
        AddHandler Me.ddlPair.ppDropDownList.SelectedIndexChanged, AddressOf ddlPair_SelectedIndexChanged
        AddHandler Me.txtCd.ppTextBox.TextChanged, AddressOf txtCd_TextChanged
        AddHandler Me.tftsCd.ppTextBoxFrom.TextChanged, AddressOf tftsCd_TextChanged
        AddHandler Me.tftsCd.ppTextBoxTo.TextChanged, AddressOf tftsCd_TextChanged

        Me.ddlTrd.ppDropDownList.AutoPostBack = True
        Me.ddlPair.ppDropDownList.AutoPostBack = True
        Me.txtCd.ppTextBox.AutoPostBack = True
        Me.tftsCd.ppTextBoxFrom.AutoPostBack = True
        Me.tftsCd.ppTextBoxTo.AutoPostBack = True

        '電話番号、郵便番号入力チェック用バリデーターのインスタンス・イベント生成
        'エラーサマリー表示は原則最後のテキストボックスに紐付けて行う(例：住所txtAddr3、郵便番号：txtzip2)
        '表示文言調整の為、txtNameオブジェクトもppRequiredFieldプロパティを使用せず直接設定

        Dim cuv_Zip As CustomValidator
        Dim cuv_Tel As CustomValidator
        Dim cuv_Fax As CustomValidator
        Dim cuv_EmTel As CustomValidator
        Dim cuv_Name As CustomValidator
        Dim cuv_Cod As CustomValidator
        cuv_Zip = txtZipNo2.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv_Tel = txtTel3.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv_Fax = txtFax3.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv_EmTel = txtEmTel3.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv_Name = txtName.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv_Cod = txtCd.FindControl("pnlErr").FindControl("cuvTextBox")

        AddHandler cuv_Zip.ServerValidate, AddressOf cuvZip_ServerValidate
        AddHandler cuv_Tel.ServerValidate, AddressOf cuvTEL_ServerValidate
        AddHandler cuv_Fax.ServerValidate, AddressOf cuvFax_ServerValidate
        AddHandler cuv_EmTel.ServerValidate, AddressOf cuvEmTEL_ServerValidate
        AddHandler cuv_Name.ServerValidate, AddressOf cuvName_ServerValidate
        AddHandler cuv_Cod.ServerValidate, AddressOf cuvCod_ServerValidate

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
            msSetddlState()

            'グリッドの初期化
            Me.grvList.DataSource = New DataTable
            grvList.DataBind()

            strMode = "Default"
            ddlsTrd.ppDropDownList.Focus()

        End If

    End Sub

    ''' <summary>
    ''' Page PreRender
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        '活性制御
        msEnableCtrl(strMode)


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

        ddlsTrd.ppDropDownList.SelectedIndex = -1
        ddlsPair.ppDropDownList.SelectedIndex = -1
        tftsCd.ppTextBoxFrom.Text = String.Empty
        tftsCd.ppTextBoxTo.Text = String.Empty
        ddldel.ppDropDownList.SelectedIndex = -1

        ddlsTrd.ppDropDownList.Focus()

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
            If e.CommandName = "INSERT" Then
                txtCd.ppValidationGroup = "val"
                Page.Validate("val")
                If Page.IsValid AndAlso CodeLengthCheck() Then
                    If mfIsExistData() = True Then 'False = 存在しないならOK
                        Me.txtCd.psSet_ErrorNo("2006", "入力した業者コード")
                    Else
                        msEditData(e.CommandName)
                    End If
                End If
            Else
                Page.Validate("val")
                If Page.IsValid AndAlso CodeLengthCheck() Then
                    If e.CommandName = "INSERT" Then
                        If mfIsExistData() = True Then 'False = 存在しないならOK
                            Me.txtCd.psSet_ErrorNo("2006", "入力した業者コード")
                            Exit Sub
                        End If
                    End If
                    msEditData(e.CommandName)
                End If
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

        ddlTrd.ppDropDownList.SelectedIndex = -1
        ddlPair.ppDropDownList.SelectedIndex = -1
        txtCd.ppText = String.Empty
        txtName.ppText = String.Empty
        txtZipNo1.ppText = String.Empty
        txtZipNo2.ppText = String.Empty
        ddlState.SelectedIndex = -1
        txtAddr1.ppText = String.Empty
        txtAddr2.ppText = String.Empty
        txtAddr3.ppText = String.Empty
        txtTel1.ppText = String.Empty
        txtTel2.ppText = String.Empty
        txtTel3.ppText = String.Empty
        txtFax1.ppText = String.Empty
        txtFax2.ppText = String.Empty
        txtFax3.ppText = String.Empty
        txtEmTel1.ppText = String.Empty
        txtEmTel2.ppText = String.Empty
        txtEmTel3.ppText = String.Empty

        strMode = "Default"

        ddlTrd.ppDropDownList.Focus()

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 業者区分変更
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlTrd_SelectedIndexChanged(sender As Object, e As System.EventArgs)

        If ddlTrd.ppSelectedValue <> String.Empty Then
            If ddlPair.ppSelectedValue <> String.Empty AndAlso txtCd.ppText.Trim <> String.Empty Then
                Page.Validate("key")
                If Page.IsValid Then
                    msGet_ExistData()
                End If
            Else
                ddlPair.ppDropDownList.Focus()
            End If
        Else
            ddlTrd.ppDropDownList.Focus()
        End If

    End Sub

    ''' <summary>
    ''' 企業区分変更
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlPair_SelectedIndexChanged(sender As Object, e As System.EventArgs)

        If ddlPair.ppSelectedValue <> String.Empty Then
            If ddlTrd.ppSelectedValue <> String.Empty AndAlso txtCd.ppText.Trim <> String.Empty Then
                Page.Validate("key")
                If Page.IsValid Then
                    msGet_ExistData()
                End If
            Else
                Master.ppBtnDmy.Visible = True
                Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtCd.ppTextBox.ClientID + ");"
                SetFocus(Master.ppBtnDmy.ClientID)
            End If
        Else
            ddlPair.ppDropDownList.Focus()
        End If

    End Sub

    ''' <summary>
    ''' コード変更
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtCd_TextChanged(sender As Object, e As System.EventArgs)
        If Master.ppBtnInsert.Enabled = True Then
            Master.ppBtnDmy.Visible = True
            Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtName.ppTextBox.ClientID + ");"
            SetFocus(Master.ppBtnDmy.ClientID)
            Exit Sub
        End If
        If txtCd.ppText.Trim <> String.Empty Then
            txtCd.ppValidationGroup = "key"
            txtCd.ppText = mfGetNarrow(txtCd.ppText.Trim)
            If ddlTrd.ppSelectedValue <> String.Empty AndAlso ddlPair.ppSelectedValue <> String.Empty Then
                Page.Validate("key")
                If Page.IsValid Then
                    If Master.ppBtnInsert.Enabled = False Then
                        msGet_ExistData()
                    End If
                Else
                    Master.ppBtnDmy.Visible = True
                    Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtCd.ppTextBox.ClientID + ");"
                    SetFocus(Master.ppBtnDmy.ClientID)
                End If
            Else
                If ddlTrd.ppSelectedValue = String.Empty Then
                    ddlTrd.ppDropDownList.Focus()
                Else
                    ddlPair.ppDropDownList.Focus()
                End If
            End If
        Else
            Master.ppBtnDmy.Visible = True
            Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtCd.ppTextBox.ClientID + ");"
            SetFocus(Master.ppBtnDmy.ClientID)
        End If
    End Sub
    Private Sub tftsCd_TextChanged(sender As Object, e As System.EventArgs)

        sender.text = mfGetNarrow(sender.text)

        If sender.Equals(tftsCd.ppTextBoxFrom) Then
            Master.ppBtnDmy.Visible = True
            Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + tftsCd.ppTextBoxTo.ClientID + ");"
            SetFocus(Master.ppBtnDmy.ClientID)
        Else
            ddldel.ppDropDownList.Focus()
        End If

    End Sub

    ''' <summary>
    ''' 画面遷移処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnM44_Click()

        '開始ログ出力
        psLogStart(Me)

        'パンクズリストの設定
        Session(P_SESSION_BCLIST) = Master.ppBCList

        '業者マスタに遷移
        psOpen_Window(Me, M_MST_DISP_PATH)

        '終了ログ出力
        psLogEnd(Me)

    End Sub

#End Region

#Region "Validation関係"

    ''' <summary>
    ''' 郵便番号チェック
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuvZip_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim cuv As CustomValidator
        Dim strErrNo As String
        cuv = txtZipNo2.FindControl("pnlErr").FindControl("cuvTextBox")
        Try
            If (txtZipNo1.ppText.Trim = String.Empty AndAlso txtZipNo2.ppText.Trim = String.Empty) Then
            ElseIf (txtZipNo1.ppText.Trim <> String.Empty AndAlso txtZipNo2.ppText.Trim <> String.Empty) Then
                strErrNo = ClsCMCommon.pfCheck_TxtErr(txtZipNo1.ppText & txtZipNo2.ppText,
                                                      False,
                                                      True,
                                                      True,
                                                      True,
                                                      txtZipNo1.ppMaxLength + txtZipNo2.ppMaxLength,
                                                      "",
                                                      False)

                If strErrNo <> String.Empty Then
                    txtZipNo2.psSet_ErrorNo(strErrNo, txtZipNo1.ppName, txtZipNo1.ppMaxLength + txtZipNo2.ppMaxLength)
                    args.IsValid = cuv.IsValid
                    Exit Sub
                End If

            Else
                cuv.Text = ClsCMCommon.pfGet_ValMes("5005", txtZipNo1.ppName).Item(P_VALMES_SMES).ToString
                cuv.ErrorMessage = ClsCMCommon.pfGet_ValMes("5005", txtZipNo1.ppName).Item(P_VALMES_MES).ToString
                args.IsValid = False
            End If
        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, txtZipNo1.ppName & "の数値への変換")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
    End Sub

    ''' <summary>
    ''' 代表電話番号チェック
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuvTEL_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim cuv As CustomValidator
        Dim strErrNo As String
        cuv = txtTel3.FindControl("pnlErr").FindControl("cuvTextBox")
        Try
            If (txtTel1.ppText.Trim = String.Empty AndAlso txtTel2.ppText.Trim = String.Empty AndAlso txtTel3.ppText.Trim = String.Empty) Then
            ElseIf (txtTel1.ppText.Trim <> String.Empty AndAlso txtTel2.ppText.Trim <> String.Empty AndAlso txtTel3.ppText.Trim <> String.Empty) Then
                strErrNo = ClsCMCommon.pfCheck_TxtErr(txtTel1.ppText & txtTel2.ppText & txtTel3.ppText,
                                                     False,
                                                     True,
                                                     True,
                                                     False,
                                                     txtTel1.ppMaxLength + txtTel2.ppMaxLength + txtTel3.ppMaxLength,
                                                     "",
                                                     False)

                If strErrNo <> String.Empty Then
                    txtTel3.psSet_ErrorNo(strErrNo, txtTel1.ppName, txtTel1.ppMaxLength + txtTel2.ppMaxLength + txtTel3.ppMaxLength)
                    args.IsValid = cuv.IsValid
                    Exit Sub
                End If
            Else

                cuv.Text = ClsCMCommon.pfGet_ValMes("5005", txtTel1.ppName).Item(P_VALMES_SMES).ToString
                cuv.ErrorMessage = ClsCMCommon.pfGet_ValMes("5005", txtTel1.ppName).Item(P_VALMES_MES).ToString
                args.IsValid = False
            End If
        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, txtTel1.ppName & "の数値への変換")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
    End Sub

    ''' <summary>
    ''' FAX番号チェック
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuvFax_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim cuv As CustomValidator
        Dim strErrNo As String
        cuv = txtFax3.FindControl("pnlErr").FindControl("cuvTextBox")
        Try
            If (txtFax1.ppText.Trim = String.Empty AndAlso txtFax2.ppText.Trim = String.Empty AndAlso txtFax3.ppText.Trim = String.Empty) Then
            ElseIf (txtFax1.ppText.Trim <> String.Empty AndAlso txtFax2.ppText.Trim <> String.Empty AndAlso txtFax3.ppText.Trim <> String.Empty) Then
                strErrNo = ClsCMCommon.pfCheck_TxtErr(txtFax1.ppText & txtFax2.ppText & txtFax3.ppText,
                                                     False,
                                                     True,
                                                     True,
                                                     False,
                                                     txtFax1.ppMaxLength + txtFax2.ppMaxLength + txtFax3.ppMaxLength,
                                                     "",
                                                     False)

                If strErrNo <> String.Empty Then
                    txtFax3.psSet_ErrorNo(strErrNo, txtFax1.ppName, txtFax1.ppMaxLength + txtFax2.ppMaxLength + txtFax3.ppMaxLength)
                    args.IsValid = cuv.IsValid
                    Exit Sub
                End If
            Else

                cuv.Text = ClsCMCommon.pfGet_ValMes("5005", txtFax1.ppName).Item(P_VALMES_SMES).ToString
                cuv.ErrorMessage = ClsCMCommon.pfGet_ValMes("5005", txtFax1.ppName).Item(P_VALMES_MES).ToString
                args.IsValid = False
            End If
        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, txtFax1.ppName & "の数値への変換")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' 連絡電話番号チェック
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuvEmTEL_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim cuv As CustomValidator
        Dim strErrNo As String
        cuv = txtEmTel3.FindControl("pnlErr").FindControl("cuvTextBox")
        Try

            If (txtEmTel1.ppText.Trim = String.Empty AndAlso txtEmTel2.ppText.Trim = String.Empty AndAlso txtEmTel3.ppText.Trim = String.Empty) Then
            ElseIf (txtEmTel1.ppText.Trim <> String.Empty AndAlso txtEmTel2.ppText.Trim <> String.Empty AndAlso txtEmTel3.ppText.Trim <> String.Empty) Then
                strErrNo = ClsCMCommon.pfCheck_TxtErr(txtEmTel1.ppText & txtEmTel2.ppText & txtEmTel3.ppText,
                                                     False,
                                                     True,
                                                     True,
                                                     False,
                                                     txtEmTel1.ppMaxLength + txtEmTel2.ppMaxLength + txtEmTel3.ppMaxLength,
                                                     "",
                                                     False)

                If strErrNo <> String.Empty Then
                    txtEmTel3.psSet_ErrorNo(strErrNo, txtEmTel1.ppName, txtEmTel1.ppMaxLength + txtEmTel2.ppMaxLength + txtEmTel3.ppMaxLength)
                    args.IsValid = cuv.IsValid
                    Exit Sub
                End If

            Else
                cuv.Text = ClsCMCommon.pfGet_ValMes("5005", txtEmTel1.ppName).Item(P_VALMES_SMES).ToString
                cuv.ErrorMessage = ClsCMCommon.pfGet_ValMes("5005", txtEmTel1.ppName).Item(P_VALMES_MES).ToString
                args.IsValid = False
            End If
        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, txtEmTel1.ppName & "の数値への変換")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
    End Sub

    ''' <summary>
    ''' 業者名称チェック(未入力エラー)
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuvName_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim cuv As CustomValidator
        cuv = txtName.FindControl("pnlErr").FindControl("cuvTextBox")
        If txtName.ppText.Trim = String.Empty Then
            txtName.psSet_ErrorNo("5006", "業者名")
            args.IsValid = cuv.IsValid
            Exit Sub
        End If
    End Sub

    ''' <summary>
    ''' コードチェック
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuvCod_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim cuv As CustomValidator
        cuv = txtCd.FindControl("pnlErr").FindControl("cuvTextBox")
        Dim strErrNo As String
        Try
            If ddlTrd.ppDropDownList.SelectedValue = "4" AndAlso (ddlPair.ppSelectedValue = "1" OrElse ddlPair.ppSelectedValue = "9") Then
                strErrNo = ClsCMCommon.pfCheck_TxtErr(txtCd.ppText,
                                                  True,
                                                  False,
                                                  True,
                                                  True,
                                                  5,
                                                  "^[0-9a-zA-Z]+$",
                                                  False)

                If strErrNo <> String.Empty Then
                    txtCd.psSet_ErrorNo(strErrNo, txtCd.ppName, "5")
                    args.IsValid = cuv.IsValid
                    Exit Sub
                End If
            Else
                strErrNo = ClsCMCommon.pfCheck_TxtErr(txtCd.ppText,
                                                  True,
                                                  True,
                                                  True,
                                                  False,
                                                  4,
                                                  "",
                                                  False)

                If strErrNo <> String.Empty Then
                    txtCd.psSet_ErrorNo(strErrNo, txtCd.ppName, "4")
                    args.IsValid = cuv.IsValid
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, txtCd.ppName & "の数値への変換")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
    End Sub


    Private Function CodeLengthCheck() As Boolean
        CodeLengthCheck = False
        If txtCd.ppText.Length = 5 Then
            If ddlTrd.ppDropDownList.SelectedValue = "4" AndAlso (ddlPair.ppSelectedValue = "1" OrElse ddlPair.ppSelectedValue = "9") Then
                CodeLengthCheck = True
            Else
                Me.txtCd.psSet_ErrorNo("3002", "コードは", "4")
                SetFocus(Me.txtCd.ppTextBox.ClientID)
                Exit Function
            End If
        ElseIf txtCd.ppText.Trim.Length = 0 Then
            Me.txtCd.psSet_ErrorNo("5001", "コード")
            SetFocus(Me.txtCd.ppTextBox.ClientID)
            Exit Function

        Else
            If ddlTrd.ppDropDownList.SelectedValue = "4" AndAlso (ddlPair.ppSelectedValue = "1" OrElse ddlPair.ppSelectedValue = "9") Then
                Me.txtCd.psSet_ErrorNo("3001", "コードは", "5")
                SetFocus(Me.txtCd.ppTextBox.ClientID)
                Exit Function
            Else
                CodeLengthCheck = True
            End If
        End If
    End Function

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
                arKey.Insert(0, CType(rowData.FindControl("業者コード"), TextBox).Text)
                arKey.Insert(0, CType(rowData.FindControl("企業コード"), TextBox).Text)
                arKey.Insert(0, CType(rowData.FindControl("会社コード"), TextBox).Text)

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
                ddlTrd.ppDropDownList.SelectedValue = CType(rowData.FindControl("業者コード"), TextBox).Text
                ddlPair.ppDropDownList.SelectedValue = CType(rowData.FindControl("企業コード"), TextBox).Text
                txtCd.ppText = CType(rowData.FindControl("会社コード"), TextBox).Text
                txtName.ppText = CType(rowData.FindControl("会社名"), TextBox).Text
                txtZipNo1.ppText = CType(rowData.FindControl("郵便番号1"), TextBox).Text
                txtZipNo2.ppText = CType(rowData.FindControl("郵便番号2"), TextBox).Text
                ddlState.SelectedValue = CType(rowData.FindControl("都道府県コード"), TextBox).Text
                txtAddr1.ppText = CType(rowData.FindControl("住所1"), TextBox).Text
                txtAddr2.ppText = CType(rowData.FindControl("住所2"), TextBox).Text
                txtAddr3.ppText = CType(rowData.FindControl("住所3"), TextBox).Text
                txtTel1.ppText = CType(rowData.FindControl("代表TEL1"), TextBox).Text
                txtTel2.ppText = CType(rowData.FindControl("代表TEL2"), TextBox).Text
                txtTel3.ppText = CType(rowData.FindControl("代表TEL3"), TextBox).Text
                txtFax1.ppText = CType(rowData.FindControl("FAX1"), TextBox).Text
                txtFax2.ppText = CType(rowData.FindControl("FAX2"), TextBox).Text
                txtFax3.ppText = CType(rowData.FindControl("FAX3"), TextBox).Text
                txtEmTel1.ppText = CType(rowData.FindControl("連絡TEL1"), TextBox).Text
                txtEmTel2.ppText = CType(rowData.FindControl("連絡TEL2"), TextBox).Text
                txtEmTel3.ppText = CType(rowData.FindControl("連絡TEL3"), TextBox).Text

                If CType(rowData.FindControl("削除"), TextBox).Text = "0" Then
                    Master.ppBtnDelete.Text = "削除"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '削除
                Else
                    Master.ppBtnDelete.Text = "削除取消"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
                End If
                strMode = "Select"
            End If

            '不正入力との競合時、エラーサマリーが残る事象を防ぐ
            Page.Validate("key")
            '既存データ取得処理と競合時、ダミーボタンが残る事象を防ぐ
            Master.ppBtnDmy.Visible = False

            txtName.ppTextBox.Focus()

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
        Dim strHeader As String() = New String() {"選択", "業者区分", "業者区分", "分類区分", "分類区分", "業者名", "業者名", "郵便番号", "都道府県",
                                                  "都道府県", "住所", "代表電話番号", "連絡電話番号", "FAX番号", "削除", "住所1", "住所2", "住所3",
                                                  "代表TEL1", "代表TEL2", "代表TEL3", "FAX1", "FAX2", "FAX3", "連絡TEL1", "連絡TEL2", "連絡TEL3"}

        Try
            If Not IsPostBack Then
                For clm As Integer = 1 To strHeader.Count - 1
                    grvList.Columns(clm).HeaderText = strHeader(clm)
                Next
            End If

            For Each rowData As GridViewRow In grvList.Rows
                '削除フラグ１の時該当行赤字化
                If CType(rowData.FindControl("削除"), TextBox).Text.Trim = "1" Then
                    CType(rowData.FindControl("業者コード"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("業者区分"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("企業コード"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("企業区分"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("会社コード"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("会社名"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("郵便番号"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("都道府県コード"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("都道府県"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("住所1"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("住所2"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("住所3"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("住所"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("代表電話番号"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("連絡電話番号"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("FAX番号"), TextBox).ForeColor = Drawing.Color.Red
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
                .Add(pfSet_Param("TrdCd", SqlDbType.NVarChar, ddlsTrd.ppSelectedValue))
                .Add(pfSet_Param("CmpCd", SqlDbType.NVarChar, ddlsPair.ppSelectedValue))
                .Add(pfSet_Param("CdF", SqlDbType.NVarChar, tftsCd.ppFromText.Trim))
                .Add(pfSet_Param("CdT", SqlDbType.NVarChar, tftsCd.ppToText.Trim))
                .Add(pfSet_Param("DeleteFlg", SqlDbType.NVarChar, ddldel.ppSelectedValue))
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
                    .Add(pfSet_Param("TrdCd", SqlDbType.NVarChar, ddlTrd.ppSelectedValue))
                    .Add(pfSet_Param("CmpCd", SqlDbType.NVarChar, ddlPair.ppSelectedValue))
                    .Add(pfSet_Param("Cd", SqlDbType.NVarChar, txtCd.ppText.Trim))
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
                    arKey.Insert(0, ddlTrd.ppSelectedValue)
                    arKey.Insert(0, ddlPair.ppSelectedValue)
                    arKey.Insert(0, txtCd.ppText.Trim)

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
                    ddlTrd.ppDropDownList.SelectedValue = dtsDB.Tables(0).Rows(0).Item("業者コード").ToString
                    ddlPair.ppDropDownList.SelectedValue = dtsDB.Tables(0).Rows(0).Item("企業コード").ToString
                    txtCd.ppText = dtsDB.Tables(0).Rows(0).Item("会社コード").ToString
                    txtName.ppText = dtsDB.Tables(0).Rows(0).Item("会社名").ToString
                    txtZipNo1.ppText = dtsDB.Tables(0).Rows(0).Item("郵便番号1").ToString
                    txtZipNo2.ppText = dtsDB.Tables(0).Rows(0).Item("郵便番号2").ToString
                    ddlState.SelectedValue = dtsDB.Tables(0).Rows(0).Item("都道府県コード").ToString
                    txtAddr1.ppText = dtsDB.Tables(0).Rows(0).Item("住所1").ToString
                    txtAddr2.ppText = dtsDB.Tables(0).Rows(0).Item("住所2").ToString
                    txtAddr3.ppText = dtsDB.Tables(0).Rows(0).Item("住所3").ToString
                    txtTel1.ppText = dtsDB.Tables(0).Rows(0).Item("代表TEL1").ToString
                    txtTel2.ppText = dtsDB.Tables(0).Rows(0).Item("代表TEL2").ToString
                    txtTel3.ppText = dtsDB.Tables(0).Rows(0).Item("代表TEL3").ToString
                    txtFax1.ppText = dtsDB.Tables(0).Rows(0).Item("FAX1").ToString
                    txtFax2.ppText = dtsDB.Tables(0).Rows(0).Item("FAX2").ToString
                    txtFax3.ppText = dtsDB.Tables(0).Rows(0).Item("FAX3").ToString
                    txtEmTel1.ppText = dtsDB.Tables(0).Rows(0).Item("連絡TEL1").ToString
                    txtEmTel2.ppText = dtsDB.Tables(0).Rows(0).Item("連絡TEL2").ToString
                    txtEmTel3.ppText = dtsDB.Tables(0).Rows(0).Item("連絡TEL3").ToString

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
        Dim afFocusID As String = String.Empty
        If strMode = "Insert" OrElse strMode = "Update" Then
            afFocusID = txtName.ppTextBox.ClientID
        Else
            afFocusID = ddlTrd.ppDropDownList.ClientID
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
                    .Add(pfSet_Param("TrdCd", SqlDbType.NVarChar, ddlTrd.ppSelectedValue))
                    .Add(pfSet_Param("CmpCd", SqlDbType.NVarChar, ddlPair.ppSelectedValue))
                    .Add(pfSet_Param("Cd", SqlDbType.NVarChar, txtCd.ppText.Trim))
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
        Dim getFlg As String = "0"
        Dim trans As SqlClient.SqlTransaction             'トランザクション
        objStack = New StackFrame
        Dim intRtn As Integer
        Dim drData As DataRow

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
                        getFlg = "1"
                    Case "削除取消"
                        MesCode = "00001"
                        strStored = DispCode & "_D1"
                        getFlg = "0"
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
                            .Add(pfSet_Param("TrdCd", SqlDbType.NVarChar, ddlTrd.ppSelectedValue))
                            .Add(pfSet_Param("CmpCd", SqlDbType.NVarChar, ddlPair.ppSelectedValue))
                            .Add(pfSet_Param("Cd", SqlDbType.NVarChar, txtCd.ppText))
                            .Add(pfSet_Param("UserId", SqlDbType.NVarChar, Session(P_SESSION_USERID)))  'ユーザーID
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
                        End With

                    Case Else
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("TrdCd", SqlDbType.NVarChar, ddlTrd.ppSelectedValue))
                            .Add(pfSet_Param("TrdNm", SqlDbType.NVarChar, ddlTrd.ppSelectedText.Split(":")(1)))
                            .Add(pfSet_Param("CmpCd", SqlDbType.NVarChar, ddlPair.ppSelectedValue))
                            .Add(pfSet_Param("CmpNm", SqlDbType.NVarChar, ddlPair.ppSelectedText.Split(":")(1)))
                            .Add(pfSet_Param("Cd", SqlDbType.NVarChar, txtCd.ppText.Trim))
                            .Add(pfSet_Param("Name", SqlDbType.NVarChar, txtName.ppText.Trim))
                            .Add(pfSet_Param("Zip1", SqlDbType.NVarChar, txtZipNo1.ppText.Trim))
                            .Add(pfSet_Param("Zip2", SqlDbType.NVarChar, txtZipNo2.ppText.Trim))
                            .Add(pfSet_Param("StateCd", SqlDbType.NVarChar, ddlState.SelectedValue))
                            .Add(pfSet_Param("StateNm", SqlDbType.NVarChar, mfStateNm()))
                            .Add(pfSet_Param("Addr1", SqlDbType.NVarChar, txtAddr1.ppText.Trim))
                            .Add(pfSet_Param("Addr2", SqlDbType.NVarChar, txtAddr2.ppText.Trim))
                            .Add(pfSet_Param("Addr3", SqlDbType.NVarChar, txtAddr3.ppText.Trim))
                            .Add(pfSet_Param("Tel1", SqlDbType.NVarChar, txtTel1.ppText.Trim))
                            .Add(pfSet_Param("Tel2", SqlDbType.NVarChar, txtTel2.ppText.Trim))
                            .Add(pfSet_Param("Tel3", SqlDbType.NVarChar, txtTel3.ppText.Trim))
                            .Add(pfSet_Param("Fax1", SqlDbType.NVarChar, txtFax1.ppText.Trim))
                            .Add(pfSet_Param("Fax2", SqlDbType.NVarChar, txtFax2.ppText.Trim))
                            .Add(pfSet_Param("Fax3", SqlDbType.NVarChar, txtFax3.ppText.Trim))
                            .Add(pfSet_Param("EmTel1", SqlDbType.NVarChar, txtEmTel1.ppText.Trim))
                            .Add(pfSet_Param("EmTel2", SqlDbType.NVarChar, txtEmTel2.ppText.Trim))
                            .Add(pfSet_Param("EmTel3", SqlDbType.NVarChar, txtEmTel3.ppText.Trim))
                            .Add(pfSet_Param("UserId", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
                        End With

                End Select

                cmdDB.ExecuteNonQuery()

                'ストアド戻り値チェック
                intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)

                If intRtn <> 0 Then
                    trans.Rollback()
                    psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                    Exit Sub
                End If
                trans.Commit()

                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName)

                If ipstrMode = "INSERT" OrElse ipstrMode = "UPDATE" OrElse Master.ppBtnDelete.Text = "削除取消" Then

                    '登録/更新の場合は対象レコードのみ表示
                    '表示用のデータテーブルを作成
                    dttGrid.Columns.Add("業者コード")
                    dttGrid.Columns.Add("業者区分")
                    dttGrid.Columns.Add("企業コード")
                    dttGrid.Columns.Add("企業区分")
                    dttGrid.Columns.Add("会社コード")
                    dttGrid.Columns.Add("会社名")
                    dttGrid.Columns.Add("郵便番号")
                    dttGrid.Columns.Add("都道府県コード")
                    dttGrid.Columns.Add("都道府県")
                    dttGrid.Columns.Add("住所")
                    dttGrid.Columns.Add("代表電話番号")
                    dttGrid.Columns.Add("FAX番号")
                    dttGrid.Columns.Add("連絡電話番号")
                    dttGrid.Columns.Add("削除")
                    dttGrid.Columns.Add("郵便番号1")
                    dttGrid.Columns.Add("郵便番号2")
                    dttGrid.Columns.Add("住所1")
                    dttGrid.Columns.Add("住所2")
                    dttGrid.Columns.Add("住所3")
                    dttGrid.Columns.Add("代表TEL1")
                    dttGrid.Columns.Add("代表TEL2")
                    dttGrid.Columns.Add("代表TEL3")
                    dttGrid.Columns.Add("FAX1")
                    dttGrid.Columns.Add("FAX2")
                    dttGrid.Columns.Add("FAX3")
                    dttGrid.Columns.Add("連絡TEL1")
                    dttGrid.Columns.Add("連絡TEL2")
                    dttGrid.Columns.Add("連絡TEL3")
                    '表示用の行データを作成、データテーブルに登録
                    drData = dttGrid.NewRow()
                    drData.Item("業者コード") = ddlTrd.ppDropDownList.SelectedItem.ToString.Split(":")(0)
                    drData.Item("業者区分") = ddlTrd.ppDropDownList.SelectedItem.ToString.Split(":")(1)
                    drData.Item("企業コード") = ddlPair.ppDropDownList.SelectedItem.ToString.Split(":")(0)
                    drData.Item("企業区分") = ddlPair.ppDropDownList.SelectedItem.ToString.Split(":")(1)
                    drData.Item("会社コード") = txtCd.ppText.Trim
                    drData.Item("会社名") = txtName.ppText.Trim
                    drData.Item("郵便番号") = mfTelNo_Check(txtZipNo1.ppText.Trim & "-" & txtZipNo2.ppText.Trim)
                    drData.Item("都道府県コード") = ddlState.SelectedValue
                    drData.Item("都道府県") = mfStateNm()
                    drData.Item("住所") = txtAddr1.ppText.Trim & txtAddr2.ppText.Trim & txtAddr3.ppText.Trim
                    drData.Item("代表電話番号") = mfTelNo_Check(txtTel1.ppText.Trim & "-" & txtTel2.ppText.Trim & "-" & txtTel3.ppText.Trim)
                    drData.Item("FAX番号") = mfTelNo_Check(txtFax1.ppText.Trim & "-" & txtFax2.ppText.Trim & "-" & txtFax3.ppText.Trim)
                    drData.Item("連絡電話番号") = mfTelNo_Check(txtEmTel1.ppText.Trim & "-" & txtEmTel2.ppText.Trim & "-" & txtEmTel3.ppText.Trim)
                    drData.Item("削除") = "0"
                    drData.Item("郵便番号1") = txtZipNo1.ppText.Trim
                    drData.Item("郵便番号2") = txtZipNo2.ppText.Trim
                    drData.Item("住所1") = txtAddr1.ppText.Trim
                    drData.Item("住所2") = txtAddr2.ppText.Trim
                    drData.Item("住所3") = txtAddr3.ppText.Trim
                    drData.Item("代表TEL1") = txtTel1.ppText.Trim
                    drData.Item("代表TEL2") = txtTel2.ppText.Trim
                    drData.Item("代表TEL3") = txtTel3.ppText.Trim
                    drData.Item("FAX1") = txtFax1.ppText.Trim
                    drData.Item("FAX2") = txtFax2.ppText.Trim
                    drData.Item("FAX3") = txtFax3.ppText.Trim
                    drData.Item("連絡TEL1") = txtEmTel1.ppText.Trim
                    drData.Item("連絡TEL2") = txtEmTel2.ppText.Trim
                    drData.Item("連絡TEL3") = txtEmTel3.ppText.Trim
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
    ''' 都道府県DropDownList設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlState()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try

                '都道府県設定
                objCmd = New SqlCommand("ZMSTSEL002", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                Me.ddlState.Items.Clear()
                Me.ddlState.DataSource = objDs.Tables(0)
                Me.ddlState.DataTextField = "項目名"
                Me.ddlState.DataValueField = "都道府県コード"
                Me.ddlState.DataBind()
                Me.ddlState.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

                Me.ddlState.Items.Clear()
                Me.ddlState.DataSource = objDs.Tables(0)
                Me.ddlState.DataTextField = "項目名"
                Me.ddlState.DataValueField = "都道府県コード"
                Me.ddlState.DataBind()
                Me.ddlState.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "都道府県の一覧取得")
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
                Master.ppBtnDelete.Text = "削除"
                mpKeyInpEnable = True
                mpMainInpEnable = False

            Case "Select", "Update"
                Master.ppBtnUpdate.Enabled = True      '更新
                Master.ppBtnDelete.Enabled = True      '削除
                If Master.ppBtnDelete.Text = "削除取消" Then
                    Master.ppBtnUpdate.Enabled = False
                    Master.ppMainEnabled = False
                Else
                    Master.ppBtnUpdate.Enabled = True      '更新
                End If
                mpKeyInpEnable = False
                mpMainInpEnable = True

            Case "Insert"
                Master.ppBtnInsert.Enabled = True     '追加
                Master.ppBtnDelete.Text = "削除"
                mpKeyInpEnable = False
                mpMainInpEnable = True
                txtCd.ppEnabled = True
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
    ''' 選択されている都道府県名を返す(未選択時は空文字を返す)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfStateNm() As String
        If ddlState.SelectedValue <> String.Empty Then
            mfStateNm = ddlState.SelectedItem.Text.Split(":")(1)
        Else
            mfStateNm = ""
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

#End Region

End Class
