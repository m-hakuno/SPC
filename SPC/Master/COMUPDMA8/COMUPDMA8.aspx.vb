'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　DLL設定依頼内容マスタ
'*　ＰＧＭＩＤ：　COMUPDMA8
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2016.10.XX　：　栗原
'********************************************************************************************************************************

#Region "インポート定義"

Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive

#End Region

Public Class COMUPDMA8

#Region "継承定義"

    Inherits System.Web.UI.Page

#End Region

#Region "定数定義"

    Const DispCode As String = "COMUPDMA8"
    Const MasterName As String = "DLL設定依頼内容マスタ"
    Const TableName_M62 As String = "M62_DLL_WORK"     'テーブル名
    Const TableName_MA7 As String = "MA7_DLL_TIME"
    Const TableName_MA8 As String = "MA8_DEN_NAME"

    Const WRKCODE_SINYA As String = "02"
    Const WRKCODE_SYUYA As String = "03"
    Const WRKCODE_TMTNK As String = "04"
    Const WRKCODE_KNINK As String = "13"
    Const WRKCODE_CARDK As String = "16"

    Const SYSCODE_IC As String = "3"
    Const SYSCODE_LT As String = "5"

#End Region

#Region "変数定義"

    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsExc As New ClsCMExclusive
    Dim clsMst As New ClsMSTCommon
    Dim objStack As StackFrame
    Enum DispMode As Integer
        DEF
        SelectMode
        UpdateMode
    End Enum
    Dim enmMode As DispMode

#End Region

#Region "プロパティ定義"

    ''' <summary>
    ''' 検索欄の活性/非活性
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property mpSerEnable() As Boolean
        Get
            Return mpSerEnable
        End Get
        Set(value As Boolean)
            Me.ddlSWork.ppEnabled = value
            Me.ddlSSystemCls.ppEnabled = value
        End Set
    End Property

    ''' <summary>
    ''' 入力欄の活性/非活性
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property mpInpEnable() As Boolean
        Get
            Return mpInpEnable
        End Get
        Set(value As Boolean)
            Me.txtWork.ppEnabled = value
            Me.txtName1.ppEnabled = value
            Me.txtName2.ppEnabled = value
            Me.txtName3.ppEnabled = value
            Me.txtName4.ppEnabled = value
            Me.txtCngTime1.ppEnabled = value
            Me.txtCngTime2.ppEnabled = value
            Me.txtCngTime3.ppEnabled = value
            Me.txtCngTime4.ppEnabled = value
            Me.txtCngDayRevision1.ppEnabled = value
            Me.txtCngDayRevision2.ppEnabled = value
            Me.txtCngDayRevision3.ppEnabled = value
            Me.txtCngDayRevision4.ppEnabled = value
            Me.txtCngTimeRevision1.ppEnabled = value
            Me.txtCngTimeRevision2.ppEnabled = value
            Me.txtCngTimeRevision3.ppEnabled = value
            Me.txtCngTimeRevision4.ppEnabled = value
            Me.txtRtnTime1.ppEnabled = value
            Me.txtRtnTime2.ppEnabled = value
            Me.txtRtnTime3.ppEnabled = value
            Me.txtRtnTime4.ppEnabled = value
            Me.txtRtnDayRevision1.ppEnabled = value
            Me.txtRtnDayRevision2.ppEnabled = value
            Me.txtRtnDayRevision3.ppEnabled = value
            Me.txtRtnDayRevision4.ppEnabled = value
            Me.txtRtnTimeRevision1.ppEnabled = value
            Me.txtRtnTimeRevision2.ppEnabled = value
            Me.txtRtnTimeRevision3.ppEnabled = value
            Me.txtRtnTimeRevision4.ppEnabled = value
        End Set
    End Property

#End Region

#Region "イベントプロージャ"

    ''' <summary>
    ''' Page Init
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

    End Sub

    ''' <summary>
    ''' Page Load
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '不要なのでグリッドエリアを消します
        DirectCast(Me.Master.FindControl("GridContent"), ContentPlaceHolder).Visible = False

        '同様にMasterページの登録ボタン等を消します。
        Me.Master.ppBtnDelete.Visible = False
        Me.Master.ppBtnInsert.Visible = False
        Me.Master.ppBtnSrcClear.Visible = False

        Me.Master.ppBtnSearch.Text = "選択"

        'ボタンアクション設定
        AddHandler Master.ppBtnClear.Click, AddressOf btnClear_Click
        AddHandler Master.ppBtnSearch.Click, AddressOf btnSearch_click
        AddHandler Me.Master.ppBtnUpdate.Click, AddressOf btn_click

        'ドロップダウンリストのイベント設定
        AddHandler ddlSWork.ppDropDownList.SelectedIndexChanged, AddressOf ddlSWork_SelectedIndexChanged
        ddlSWork.ppDropDownList.AutoPostBack = True

        'テキストボックスのイベント設定
        AddHandler txtCngTime1.ppHourBox.TextChanged, AddressOf msTimeChange
        AddHandler txtCngTime1.ppMinBox.TextChanged, AddressOf msTimeChange
        AddHandler txtCngTime2.ppHourBox.TextChanged, AddressOf msTimeChange
        AddHandler txtCngTime2.ppMinBox.TextChanged, AddressOf msTimeChange
        AddHandler txtCngTime3.ppHourBox.TextChanged, AddressOf msTimeChange
        AddHandler txtCngTime3.ppMinBox.TextChanged, AddressOf msTimeChange
        AddHandler txtCngTime4.ppHourBox.TextChanged, AddressOf msTimeChange
        AddHandler txtCngTime4.ppMinBox.TextChanged, AddressOf msTimeChange
        AddHandler txtRtnTime1.ppHourBox.TextChanged, AddressOf msTimeChange
        AddHandler txtRtnTime1.ppMinBox.TextChanged, AddressOf msTimeChange
        AddHandler txtRtnTime2.ppHourBox.TextChanged, AddressOf msTimeChange
        AddHandler txtRtnTime2.ppMinBox.TextChanged, AddressOf msTimeChange
        AddHandler txtRtnTime3.ppHourBox.TextChanged, AddressOf msTimeChange
        AddHandler txtRtnTime3.ppMinBox.TextChanged, AddressOf msTimeChange
        AddHandler txtRtnTime4.ppHourBox.TextChanged, AddressOf msTimeChange
        AddHandler txtRtnTime4.ppMinBox.TextChanged, AddressOf msTimeChange
        AddHandler txtCngTimeRevision1.ppTextBox.TextChanged, AddressOf msTimeChange
        AddHandler txtCngTimeRevision2.ppTextBox.TextChanged, AddressOf msTimeChange
        AddHandler txtCngTimeRevision3.ppTextBox.TextChanged, AddressOf msTimeChange
        AddHandler txtCngTimeRevision4.ppTextBox.TextChanged, AddressOf msTimeChange
        AddHandler txtRtnTimeRevision1.ppTextBox.TextChanged, AddressOf msTimeChange
        AddHandler txtRtnTimeRevision2.ppTextBox.TextChanged, AddressOf msTimeChange
        AddHandler txtRtnTimeRevision3.ppTextBox.TextChanged, AddressOf msTimeChange
        AddHandler txtRtnTimeRevision4.ppTextBox.TextChanged, AddressOf msTimeChange
        AddHandler txtCngDayRevision1.ppTextBox.TextChanged, AddressOf msTimeChange
        AddHandler txtCngDayRevision2.ppTextBox.TextChanged, AddressOf msTimeChange
        AddHandler txtCngDayRevision3.ppTextBox.TextChanged, AddressOf msTimeChange
        AddHandler txtCngDayRevision4.ppTextBox.TextChanged, AddressOf msTimeChange
        AddHandler txtRtnDayRevision1.ppTextBox.TextChanged, AddressOf msTimeChange
        AddHandler txtRtnDayRevision2.ppTextBox.TextChanged, AddressOf msTimeChange
        AddHandler txtRtnDayRevision3.ppTextBox.TextChanged, AddressOf msTimeChange
        AddHandler txtRtnDayRevision4.ppTextBox.TextChanged, AddressOf msTimeChange

        AddHandler txtWork.ppTextBox.TextChanged, AddressOf msNameChange
        AddHandler txtName1.ppTextBox.TextChanged, AddressOf msNameChange
        AddHandler txtName2.ppTextBox.TextChanged, AddressOf msNameChange
        AddHandler txtName3.ppTextBox.TextChanged, AddressOf msNameChange
        AddHandler txtName4.ppTextBox.TextChanged, AddressOf msNameChange

        txtCngTime1.ppHourBox.AutoPostBack = True
        txtCngTime1.ppMinBox.AutoPostBack = True
        txtCngTime2.ppHourBox.AutoPostBack = True
        txtCngTime2.ppMinBox.AutoPostBack = True
        txtCngTime3.ppHourBox.AutoPostBack = True
        txtCngTime3.ppMinBox.AutoPostBack = True
        txtCngTime4.ppHourBox.AutoPostBack = True
        txtCngTime4.ppMinBox.AutoPostBack = True
        txtRtnTime1.ppHourBox.AutoPostBack = True
        txtRtnTime1.ppMinBox.AutoPostBack = True
        txtRtnTime2.ppHourBox.AutoPostBack = True
        txtRtnTime2.ppMinBox.AutoPostBack = True
        txtRtnTime3.ppHourBox.AutoPostBack = True
        txtRtnTime3.ppMinBox.AutoPostBack = True
        txtRtnTime4.ppHourBox.AutoPostBack = True
        txtRtnTime4.ppMinBox.AutoPostBack = True
        txtCngDayRevision1.ppTextBox.AutoPostBack = True
        txtCngDayRevision2.ppTextBox.AutoPostBack = True
        txtCngDayRevision3.ppTextBox.AutoPostBack = True
        txtCngDayRevision4.ppTextBox.AutoPostBack = True
        txtRtnDayRevision1.ppTextBox.AutoPostBack = True
        txtRtnDayRevision2.ppTextBox.AutoPostBack = True
        txtRtnDayRevision3.ppTextBox.AutoPostBack = True
        txtRtnDayRevision4.ppTextBox.AutoPostBack = True
        txtCngTimeRevision1.ppTextBox.AutoPostBack = True
        txtCngTimeRevision2.ppTextBox.AutoPostBack = True
        txtCngTimeRevision3.ppTextBox.AutoPostBack = True
        txtCngTimeRevision4.ppTextBox.AutoPostBack = True
        txtRtnTimeRevision1.ppTextBox.AutoPostBack = True
        txtRtnTimeRevision2.ppTextBox.AutoPostBack = True
        txtRtnTimeRevision3.ppTextBox.AutoPostBack = True
        txtRtnTimeRevision4.ppTextBox.AutoPostBack = True
        txtWork.ppTextBox.AutoPostBack = True
        txtName1.ppTextBox.AutoPostBack = True
        txtName2.ppTextBox.AutoPostBack = True
        txtName3.ppTextBox.AutoPostBack = True
        txtName4.ppTextBox.AutoPostBack = True

        'スクリプトの埋め込み
        msSetScript()

        '画面初期化処理
        If Not IsPostBack Then
            'プログラムID,画面名設定
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            '該当件数部分を消す
            Master.FindControl("Label4").Visible = False
            Master.FindControl("lblcount").Visible = False
            Master.FindControl("Label6").Visible = False

            'パンくずリストの設定
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            '設定項目２～４を非表示
            For i As Integer = 2 To 4
                msSetInpTableVisible(i, False)
            Next

            'ドロップダウンリストの設定
            msSetddlWork()
            msSetddlSystemCls()



            'フォーカス設定
            SetFocus(ddlSWork.ppDropDownList.ClientID)
            '状態設定
            enmMode = DispMode.SelectMode
        End If
    End Sub

    ''' <summary>
    ''' Page PreRender
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        '画面項目制御
        Select Case enmMode

            Case DispMode.SelectMode
                '入力系
                mpInpEnable = False

                'ボタン系
                Me.Master.ppBtnUpdate.Enabled = False

            Case DispMode.UpdateMode
                '入力系
                mpInpEnable = True
                'ボタン系
                Me.Master.ppBtnUpdate.Enabled = True

        End Select

        '依頼内容毎の特殊画面制御処理
        msDispSetting()

        'エラーメッセージの編集
        msEditValid()
    End Sub

    ''' <summary>
    '''クリア
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)

        'ログ出力
        psLogStart(Me)

        '初期化処理
        msInpClear()

        'フォーカス設定
        SetFocus(ddlSWork.ppDropDownList.ClientID)

        '画面状態設定
        enmMode = DispMode.SelectMode
        If Master.ppBtnSearch.OnClientClick <> String.Empty Then
            Master.ppBtnSearch.OnClientClick = String.Empty
            Master.ppBtnClear.OnClientClick = String.Empty
            DirectCast(Master.FindControl("UpdPanelSearch"), UpdatePanel).Update()
        End If
        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 検索ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnSearch_click(sender As Object, e As EventArgs)
        Dim dtWrk As DataTable '検索結果保持用
        Dim i As Integer = 1 'データ表示時に使用

        'ログ出力
        psLogStart(Me)
        'データ取得
        Page.Validate("search")
        If (Page.IsValid) Then
            '画面初期化
            msInpClear()
            If ddlSWork.ppSelectedValue = WRKCODE_CARDK Then
                'Ⅴ.【LUT】カード共用化設定　e-basカスタマイズ案件から設定不可項目の為、マスタ管理項目から除外
                psMesBox(Me, "00000", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "マスタ管理外項目の為、編集出来ません。")
            End If

            'データ取得
            dtWrk = mfGet_Data()

            '明細無し、または表示数(4)オーバー
            If dtWrk.Rows.Count = 0 Then
                psMesBox(Me, "30028", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "依頼内容がみつかりません", "マスタデータ")
                Exit Sub
            ElseIf dtWrk.Rows.Count > 4 Then
                psMesBox(Me, "30028", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "項目数が4を超えています", "マスタデータ")
                Exit Sub
            End If

            '入力内容表示
            txtWorkCd.ppText = dtWrk.Rows(0).Item("設定依頼内容コード").ToString
            txtWork.ppText = dtWrk.Rows(0).Item("設定依頼内容").ToString
            hdnSyscls.Value = ddlSSystemCls.ppSelectedValue
            lblSyscls.Text = ddlSSystemCls.ppSelectedText

            For Each dr As DataRow In dtWrk.Rows
                msSetInp(i, dr)
                msSetInpTableVisible(i, True)
                i += 1
            Next

            '合計値計算
            msDispDiffTime()

            'フォーカス設定
            SetFocus(txtWork.ppTextBox.ClientID)

            '状態設定
            enmMode = DispMode.UpdateMode

            If Master.ppBtnSearch.OnClientClick <> String.Empty Then
                Master.ppBtnSearch.OnClientClick = String.Empty
                Master.ppBtnClear.OnClientClick = String.Empty
                DirectCast(Master.FindControl("UpdPanelSearch"), UpdatePanel).Update()
            End If
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 追加/更新/削除ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btn_click(sender As Object, e As EventArgs)

        'ログ出力
        psLogStart(Me)

        Page.Validate("val")
        If (Page.IsValid) Then
            '登録/更新/削除 処理
            msEditData()
            'フォーカス設定
            SetFocus(txtWork.ppTextBox.ClientID)

            If Master.ppBtnSearch.OnClientClick <> String.Empty Then
                Master.ppBtnSearch.OnClientClick = String.Empty
                Master.ppBtnClear.OnClientClick = String.Empty
                DirectCast(Master.FindControl("UpdPanelSearch"), UpdatePanel).Update()
            End If

        End If

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 検索欄依頼内容変更時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlSWork_SelectedIndexChanged()

        'ログ出力
        psLogStart(Me)

        '依頼内容に沿ったシステム分類に変更
        Dim tmp As String = ddlSSystemCls.ppSelectedValue
        msSetddlSystemCls()
        If Not ddlSSystemCls.ppDropDownList.Items.FindByValue(tmp) Is Nothing Then
            ddlSSystemCls.ppSelectedValue = tmp
            SetFocus(ddlSSystemCls.ppDropDownList.ClientID)
        End If


        ddlSSystemCls.ppEnabled = True

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 設定時間(変更・戻し)変更時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub msTimeChange(sender As Object, e As EventArgs)
        If Master.ppBtnSearch.OnClientClick = String.Empty Then
            Master.ppBtnSearch.OnClientClick = pfGet_OCClickMes("00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "入力内容が変更されています。\n入力内容を破棄してよろしいですか？")
            Master.ppBtnClear.OnClientClick = pfGet_OCClickMes("00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "入力内容が変更されています。\n入力内容を破棄してよろしいですか？")
            DirectCast(Master.FindControl("UpdPanelSearch"), UpdatePanel).Update()
        End If

        '取得引数(sender)はHourBoxまたはMinBoxなので、親(ClsCMTimeBoxオブジェクト)を取得して渡します。
        msLinkFieldEnable(sender.ClientID)

        msDispDiffTime()

        'フォーカス設定
        Dim nextbox As New TextBox
        Dim sendbox As New TextBox
        Dim emptxt As String = "0"
        msNextCtrl(sender.ClientID, sendbox, nextbox)
        Select Case sender.ClientID
            Case txtCngTime1.ppHourBox.ClientID, txtCngTime2.ppHourBox.ClientID, txtCngTime3.ppHourBox.ClientID, txtCngTime4.ppHourBox.ClientID _
                , txtRtnTime1.ppHourBox.ClientID, txtRtnTime2.ppHourBox.ClientID, txtRtnTime3.ppHourBox.ClientID, txtRtnTime4.ppHourBox.ClientID _
                , txtCngTime1.ppMinBox.ClientID, txtCngTime2.ppMinBox.ClientID, txtCngTime3.ppMinBox.ClientID, txtCngTime4.ppMinBox.ClientID _
                , txtRtnTime1.ppMinBox.ClientID, txtRtnTime2.ppMinBox.ClientID, txtRtnTime3.ppMinBox.ClientID, txtRtnTime4.ppMinBox.ClientID
                emptxt = "00"
            Case txtCngDayRevision1.ppTextBox.ClientID
            Case txtCngDayRevision2.ppTextBox.ClientID
            Case txtCngDayRevision3.ppTextBox.ClientID
            Case txtCngDayRevision4.ppTextBox.ClientID
            Case txtRtnDayRevision1.ppTextBox.ClientID
            Case txtRtnDayRevision2.ppTextBox.ClientID
            Case txtRtnDayRevision3.ppTextBox.ClientID
            Case txtRtnDayRevision4.ppTextBox.ClientID
            Case txtCngTimeRevision1.ppTextBox.ClientID
            Case txtCngTimeRevision2.ppTextBox.ClientID
            Case txtCngTimeRevision3.ppTextBox.ClientID
            Case txtCngTimeRevision4.ppTextBox.ClientID
            Case txtRtnTimeRevision1.ppTextBox.ClientID
            Case txtRtnTimeRevision2.ppTextBox.ClientID
            Case txtRtnTimeRevision3.ppTextBox.ClientID
            Case txtRtnTimeRevision4.ppTextBox.ClientID
            Case Else
        End Select

        If sendbox.Text.Trim = String.Empty OrElse sendbox.Text.Trim = "-0" Then
            sendbox.Text = emptxt
        End If
        Dim afFocusID As String = String.Empty
        If nextbox.Visible = True Then
            afFocusID = nextbox.ClientID
        Else
            afFocusID = txtWork.ppTextBox.ClientID
        End If

        Master.ppBtnDmy.Visible = True
        Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + afFocusID + ");"
        SetFocus(Master.ppBtnDmy.ClientID)

    End Sub
    Private Sub msNameChange(sender As Object, e As EventArgs)

        If Master.ppBtnSearch.OnClientClick = String.Empty Then
            Master.ppBtnSearch.OnClientClick = pfGet_OCClickMes("00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "入力内容が変更されています。\n入力内容を破棄してよろしいですか？")
            Master.ppBtnClear.OnClientClick = pfGet_OCClickMes("00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "入力内容が変更されています。\n入力内容を破棄してよろしいですか？")
            DirectCast(Master.FindControl("UpdPanelSearch"), UpdatePanel).Update()
        End If

        Dim txtbox As TextBox
        Select Case sender.ClientID
            Case txtWork.ppTextBox.ClientID
                txtbox = txtName1.ppTextBox
            Case txtName1.ppTextBox.ClientID
                If txtWorkCd.ppText <> WRKCODE_SINYA AndAlso txtWorkCd.ppText <> WRKCODE_SYUYA Then
                    txtbox = txtCngDayRevision1.ppTextBox
                Else
                    txtbox = txtCngTime1.ppHourBox
                End If
            Case txtName2.ppTextBox.ClientID
                If txtWorkCd.ppText <> WRKCODE_SINYA AndAlso txtWorkCd.ppText <> WRKCODE_SYUYA Then
                    txtbox = txtCngDayRevision2.ppTextBox
                Else
                    txtbox = txtCngTime2.ppHourBox
                End If
            Case txtName3.ppTextBox.ClientID
                If txtWorkCd.ppText <> WRKCODE_SINYA AndAlso txtWorkCd.ppText <> WRKCODE_SYUYA Then
                    txtbox = txtCngDayRevision3.ppTextBox
                Else
                    txtbox = txtCngTime3.ppHourBox
                End If
            Case txtName4.ppTextBox.ClientID
                If txtWorkCd.ppText <> WRKCODE_SINYA AndAlso txtWorkCd.ppText <> WRKCODE_SYUYA Then
                    txtbox = txtCngDayRevision4.ppTextBox
                Else
                    txtbox = txtCngTime4.ppHourBox
                End If
            Case Else
                txtbox = txtName1.ppTextBox
        End Select

        Dim afFocusID As String = String.Empty
        afFocusID = txtbox.ClientID
        Master.ppBtnDmy.Visible = True
        Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + afFocusID + ");"
        SetFocus(Master.ppBtnDmy.ClientID)

    End Sub


#End Region

#Region "そのほかのプロージャ"

    ''' <summary>
    ''' 検索処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfGet_Data() As DataTable
        mfGet_Data = Nothing
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
                    .Add(pfSet_Param("workcode", SqlDbType.NVarChar, ddlSWork.ppSelectedValue))
                    .Add(pfSet_Param("systemcls", SqlDbType.NVarChar, ddlSSystemCls.ppSelectedValue))
                    .Add(pfSet_Param("prog_id", SqlDbType.NVarChar, DispCode))
                    .Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, dispflg))
                End With

                'リストデータ取得
                dtsDB = clsDataConnect.pfGet_DataSet(cmdDB)
                mfGet_Data = dtsDB.Tables(0)

            Catch ex As Exception

                'エラー表示
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")

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
            Master.ppCount = "0"
        End If
    End Function

    ''' <summary>
    ''' 更新処理
    ''' </summary>
    ''' <remarks>項目数の増減が仕様外の為、更新処理のみ実施する。</remarks>
    Private Sub msEditData()

        '変数宣言
        Dim cnDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim intRtn As Integer
        Dim MesCode As String = String.Empty
        Dim strStored As String = String.Empty
        Dim dtmNow As DateTime = DateTime.Now   '更新日時統一の為

        objStack = New StackFrame

        MesCode = "00001"
        strStored = DispCode & "_U1"

        'DB接続
        If clsDataConnect.pfOpen_Database(cnDB) Then
            Try
                cmdDB = New SqlCommand(strStored, cnDB)

                '更新処理
                Using cntrn = cnDB.BeginTransaction
                    cmdDB.Transaction = cntrn
                    cmdDB.CommandType = CommandType.StoredProcedure
                    '項目数分処理する
                    For i As Integer = 1 To mfGetNameRecordCount()
                        'パラメータ設定
                        msSetParameters(cmdDB, i)
                        cmdDB.Parameters.Add(pfSet_Param("nowdatetime", SqlDbType.DateTime, dtmNow)) '更新日時の設定
                        cmdDB.ExecuteNonQuery()
                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            cntrn.Rollback()
                            'エラー
                            psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                            Exit Sub
                        End If
                    Next
                    'コミット
                    cntrn.Commit()
                End Using

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
    ''' 入力欄初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msInpClear()

        Me.txtWorkCd.ppText = String.Empty
        Me.txtWork.ppText = String.Empty
        Me.lblSyscls.Text = String.Empty
        Me.txtNameCd1.ppText = String.Empty
        Me.txtNameCd2.ppText = String.Empty
        Me.txtNameCd3.ppText = String.Empty
        Me.txtNameCd4.ppText = String.Empty
        Me.txtName1.ppText = String.Empty
        Me.txtName2.ppText = String.Empty
        Me.txtName3.ppText = String.Empty
        Me.txtName4.ppText = String.Empty
        Me.txtCngTime1.ppHourText = String.Empty
        Me.txtCngTime2.ppHourText = String.Empty
        Me.txtCngTime3.ppHourText = String.Empty
        Me.txtCngTime4.ppHourText = String.Empty
        Me.txtCngTime1.ppMinText = String.Empty
        Me.txtCngTime2.ppMinText = String.Empty
        Me.txtCngTime3.ppMinText = String.Empty
        Me.txtCngTime4.ppMinText = String.Empty
        Me.txtCngDayRevision1.ppText = String.Empty
        Me.txtCngDayRevision2.ppText = String.Empty
        Me.txtCngDayRevision3.ppText = String.Empty
        Me.txtCngDayRevision4.ppText = String.Empty
        Me.txtCngTimeRevision1.ppText = String.Empty
        Me.txtCngTimeRevision2.ppText = String.Empty
        Me.txtCngTimeRevision3.ppText = String.Empty
        Me.txtCngTimeRevision4.ppText = String.Empty
        Me.txtRtnTime1.ppHourText = String.Empty
        Me.txtRtnTime2.ppHourText = String.Empty
        Me.txtRtnTime3.ppHourText = String.Empty
        Me.txtRtnTime4.ppHourText = String.Empty
        Me.txtRtnTime1.ppMinText = String.Empty
        Me.txtRtnTime2.ppMinText = String.Empty
        Me.txtRtnTime3.ppMinText = String.Empty
        Me.txtRtnTime4.ppMinText = String.Empty
        Me.txtRtnDayRevision1.ppText = String.Empty
        Me.txtRtnDayRevision2.ppText = String.Empty
        Me.txtRtnDayRevision3.ppText = String.Empty
        Me.txtRtnDayRevision4.ppText = String.Empty
        Me.txtRtnTimeRevision1.ppText = String.Empty
        Me.txtRtnTimeRevision2.ppText = String.Empty
        Me.txtRtnTimeRevision3.ppText = String.Empty
        Me.txtRtnTimeRevision4.ppText = String.Empty
        Me.lblCng1.Text = String.Empty
        Me.lblCng2.Text = String.Empty
        Me.lblCng3.Text = String.Empty
        Me.lblCng4.Text = String.Empty
        Me.lblRtn1.Text = String.Empty
        Me.lblRtn2.Text = String.Empty
        Me.lblRtn3.Text = String.Empty
        Me.lblRtn4.Text = String.Empty

        '設定項目２～４を非表示
        For i As Integer = 2 To 4
            msSetInpTableVisible(i, False)
        Next

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト[設定依頼内容]
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlWork()

        Dim objcn As SqlConnection = Nothing
        Dim objcmd As SqlCommand = Nothing
        Dim objdts As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objcn) Then
            'エラー
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objcmd = New SqlCommand(DispCode & "_S2", objcn)
                'データ取得
                objdts = clsDataConnect.pfGet_DataSet(objcmd)

                'ドロップダウンリスト設定
                '検索条件エリア
                Me.ddlSWork.ppDropDownList.Items.Clear()
                Me.ddlSWork.ppDropDownList.DataSource = objdts.Tables(0)
                Me.ddlSWork.ppDropDownList.DataTextField = "設定依頼内容"
                Me.ddlSWork.ppDropDownList.DataValueField = "設定依頼内容コード"
                Me.ddlSWork.ppDropDownList.DataBind()
                Me.ddlSWork.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                'エラー
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "設定依頼内容マスタ一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objcn) Then
                    'エラー
                    clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If
    End Sub

    ''' <summary>
    ''' ドロップダウンリスト[システム分類]
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlSystemCls()
        Dim objcn As SqlConnection = Nothing
        Dim objcmd As SqlCommand = Nothing
        Dim objdts As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objcn) Then
            'エラー
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objcmd = New SqlCommand(DispCode & "_S3", objcn)
                'パラメータ設定
                With objcmd.Parameters
                    .Add(pfSet_Param("workcode", SqlDbType.NVarChar, ddlSWork.ppSelectedValue))
                End With

                'データ取得
                objdts = clsDataConnect.pfGet_DataSet(objcmd)

                'ドロップダウンリスト設定
                '検索条件エリア
                Me.ddlSSystemCls.ppDropDownList.Items.Clear()
                Me.ddlSSystemCls.ppDropDownList.DataSource = objdts.Tables(0)
                Me.ddlSSystemCls.ppDropDownList.DataTextField = "システム分類"
                Me.ddlSSystemCls.ppDropDownList.DataValueField = "システム分類コード"
                Me.ddlSSystemCls.ppDropDownList.DataBind()
                Me.ddlSSystemCls.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                'エラー
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "システム分類一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objcn) Then
                    'エラー
                    clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' 時刻を00:00形式で返します
    ''' </summary>
    ''' <param name="_timebox"></param>
    ''' <returns></returns>
    ''' <remarks>時刻は一桁になります(01:05⇒1:05)</remarks>
    Private Function mfTimeText(ByVal _timebox As ClsCMTimeBox) As String
        If _timebox.ppHourText.Trim = String.Empty OrElse _timebox.ppMinText.Trim = String.Empty Then
            Return String.Empty
        Else
            Dim tmp As Integer = Integer.Parse(_timebox.ppHourText.Trim)
            Return tmp & ":" & _timebox.ppMinText.Trim
        End If
    End Function

    ''' <summary>
    ''' 入力エリアにデータをセットする。
    ''' </summary>
    ''' <param name="_intSetindex"></param>
    ''' <param name="_dr"></param>
    ''' <remarks></remarks>
    Private Sub msSetInp(ByVal _intSetindex As Integer, ByVal _dr As DataRow)

        Try
            Select Case _intSetindex
                Case 1
                    txtNameCd1.ppText = _dr("設定項目名コード").ToString
                    txtName1.ppText = _dr("設定項目名").ToString

                    If _dr("変更設定時間").ToString.Contains(":") Then
                        txtCngTime1.ppHourText = _dr("変更設定時間").ToString.Split(":")(0).PadLeft(2, "0")
                        txtCngTime1.ppMinText = _dr("変更設定時間").ToString.Split(":")(1)
                    Else
                        txtCngTime1.ppHourText = String.Empty
                        txtCngTime1.ppMinText = String.Empty
                    End If

                    txtCngDayRevision1.ppText = _dr("変更日付補正").ToString
                    txtCngTimeRevision1.ppText = _dr("変更時間補正").ToString

                    If _dr("戻し設定時間").ToString.Contains(":") Then
                        txtRtnTime1.ppHourText = _dr("戻し設定時間").ToString.Split(":")(0).PadLeft(2, "0")
                        txtRtnTime1.ppMinText = _dr("戻し設定時間").ToString.Split(":")(1)
                    Else
                        txtRtnTime1.ppHourText = String.Empty
                        txtRtnTime1.ppMinText = String.Empty
                    End If

                    txtRtnDayRevision1.ppText = _dr("戻し日付補正").ToString
                    txtRtnTimeRevision1.ppText = _dr("戻し時間補正").ToString

                Case 2
                    txtNameCd2.ppText = _dr("設定項目名コード").ToString
                    txtName2.ppText = _dr("設定項目名").ToString
                    If _dr("変更設定時間").ToString.Contains(":") Then
                        txtCngTime2.ppHourText = _dr("変更設定時間").ToString.Split(":")(0).PadLeft(2, "0")
                        txtCngTime2.ppMinText = _dr("変更設定時間").ToString.Split(":")(1)
                    Else
                        txtCngTime2.ppHourText = String.Empty
                        txtCngTime2.ppMinText = String.Empty
                    End If

                    txtCngDayRevision2.ppText = _dr("変更日付補正").ToString
                    txtCngTimeRevision2.ppText = _dr("変更時間補正").ToString

                    If _dr("戻し設定時間").ToString.Contains(":") Then
                        txtRtnTime2.ppHourText = _dr("戻し設定時間").ToString.Split(":")(0).PadLeft(2, "0")
                        txtRtnTime2.ppMinText = _dr("戻し設定時間").ToString.Split(":")(1)
                    Else
                        txtRtnTime2.ppHourText = String.Empty
                        txtRtnTime2.ppMinText = String.Empty
                    End If

                    txtRtnDayRevision2.ppText = _dr("戻し日付補正").ToString
                    txtRtnTimeRevision2.ppText = _dr("戻し時間補正").ToString

                Case 3
                    txtNameCd3.ppText = _dr("設定項目名コード").ToString
                    txtName3.ppText = _dr("設定項目名").ToString

                    If _dr("変更設定時間").ToString.Contains(":") Then
                        txtCngTime3.ppHourText = _dr("変更設定時間").ToString.Split(":")(0).PadLeft(2, "0")
                        txtCngTime3.ppMinText = _dr("変更設定時間").ToString.Split(":")(1)
                    Else
                        txtCngTime3.ppHourText = String.Empty
                        txtCngTime3.ppMinText = String.Empty
                    End If

                    txtCngDayRevision3.ppText = _dr("変更日付補正").ToString
                    txtCngTimeRevision3.ppText = _dr("変更時間補正").ToString
                    If _dr("戻し設定時間").ToString.Contains(":") Then
                        txtRtnTime3.ppHourText = _dr("戻し設定時間").ToString.Split(":")(0).PadLeft(2, "0")
                        txtRtnTime3.ppMinText = _dr("戻し設定時間").ToString.Split(":")(1)
                    Else
                        txtRtnTime3.ppHourText = String.Empty
                        txtRtnTime3.ppMinText = String.Empty
                    End If

                    txtRtnDayRevision3.ppText = _dr("戻し日付補正").ToString
                    txtRtnTimeRevision3.ppText = _dr("戻し時間補正").ToString

                Case 4
                    txtNameCd4.ppText = _dr("設定項目名コード").ToString
                    txtName4.ppText = _dr("設定項目名").ToString
                    If _dr("変更設定時間").ToString.Contains(":") Then
                        txtCngTime4.ppHourText = _dr("変更設定時間").ToString.Split(":")(0).PadLeft(2, "0")
                        txtCngTime4.ppMinText = _dr("変更設定時間").ToString.Split(":")(1)
                    Else
                        txtCngTime4.ppHourText = String.Empty
                        txtCngTime4.ppMinText = String.Empty
                    End If

                    txtCngDayRevision4.ppText = _dr("変更日付補正").ToString
                    txtCngTimeRevision4.ppText = _dr("変更時間補正").ToString
                    If _dr("戻し設定時間").ToString.Contains(":") Then
                        txtRtnTime4.ppHourText = _dr("戻し設定時間").ToString.Split(":")(0).PadLeft(2, "0")
                        txtRtnTime4.ppMinText = _dr("戻し設定時間").ToString.Split(":")(1)
                    Else
                        txtRtnTime4.ppHourText = String.Empty
                        txtRtnTime4.ppMinText = String.Empty
                    End If

                    txtRtnDayRevision4.ppText = _dr("戻し日付補正").ToString
                    txtRtnTimeRevision4.ppText = _dr("戻し時間補正").ToString
            End Select

        Catch ex As Exception
            'エラー
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "データ項目の表示")
            'btnclear_click(Nothing, Nothing)
        End Try
    End Sub

    ''' <summary>
    ''' 依頼内容毎に異なる特殊な画面制御を行います
    ''' </summary>
    ''' <remarks>各処理にコメント推奨</remarks>
    Private Sub msDispSetting()
        '特殊処理
        '(1)システム分類が共通のみの場合は、共通をセットし選択不可にします。
        '(2)深夜／終夜の設定以外は変更時間(設定・戻し)の入力不可とします。
        '(3)【LUT】カード共用化設定　e-basカスタマイズ案件から設定不可項目の為、編集不可とします。

        '(1)
        'カウントが1ではなく2なのは[0:共通]と[Nothing:Nothing](空欄)が有る為
        If ddlSWork.ppSelectedValue <> String.Empty AndAlso _
                (Not ddlSSystemCls.ppDropDownList.Items.FindByValue("0") Is Nothing) AndAlso _
                (ddlSSystemCls.ppDropDownList.Items.Count = 2) Then
            ddlSSystemCls.ppSelectedValue = "0" '0:共通をセット
        End If

        '(2)
        If txtWorkCd.ppText <> WRKCODE_SINYA AndAlso txtWorkCd.ppText <> WRKCODE_SYUYA Then
            txtCngTime1.ppEnabled = False
            txtCngTime2.ppEnabled = False
            txtCngTime3.ppEnabled = False
            txtCngTime4.ppEnabled = False
            txtRtnTime1.ppEnabled = False
            txtRtnTime2.ppEnabled = False
            txtRtnTime3.ppEnabled = False
            txtRtnTime4.ppEnabled = False
        End If

        '(3)
        '処理は検索後(mpInpEnable = True)のみです。選択時(検索実施前)の処理は他の依頼と同様です。
        If ddlSWork.ppSelectedValue = WRKCODE_CARDK AndAlso mpInpEnable = True Then
            mpInpEnable = False
        End If

    End Sub

    ''' <summary>
    ''' 設定項目毎の表示／非表示
    ''' </summary>
    ''' <param name="_intSetindex"></param>
    ''' <param name="_bool"></param>
    ''' <remarks></remarks>
    Private Sub msSetInpTableVisible(ByVal _intSetindex As Integer, ByVal _bool As Boolean)
        Select Case _intSetindex
            Case 1
                txtNameCd1.Visible = _bool
                txtName1.Visible = _bool
                txtCngTime1.Visible = _bool
                txtCngDayRevision1.Visible = _bool
                txtCngTimeRevision1.Visible = _bool
                txtRtnTime1.Visible = _bool
                txtRtnDayRevision1.Visible = _bool
                txtRtnTimeRevision1.Visible = _bool
                lbl1_1.Visible = _bool
                lbl1_2.Visible = _bool
                lbl1_3.Visible = _bool
                lblCng1.Visible = _bool
                lblRtn1.Visible = _bool

            Case 2
                txtNameCd2.Visible = _bool
                txtName2.Visible = _bool
                txtCngTime2.Visible = _bool
                txtCngDayRevision2.Visible = _bool
                txtCngTimeRevision2.Visible = _bool
                txtRtnTime2.Visible = _bool
                txtRtnDayRevision2.Visible = _bool
                txtRtnTimeRevision2.Visible = _bool
                lbl2_1.Visible = _bool
                lbl2_2.Visible = _bool
                lbl2_3.Visible = _bool
                lblCng2.Visible = _bool
                lblRtn2.Visible = _bool

            Case 3
                txtNameCd3.Visible = _bool
                txtName3.Visible = _bool
                txtCngTime3.Visible = _bool
                txtCngDayRevision3.Visible = _bool
                txtCngTimeRevision3.Visible = _bool
                txtRtnTime3.Visible = _bool
                txtRtnDayRevision3.Visible = _bool
                txtRtnTimeRevision3.Visible = _bool
                lbl3_1.Visible = _bool
                lbl3_2.Visible = _bool
                lbl3_3.Visible = _bool
                lblCng3.Visible = _bool
                lblRtn3.Visible = _bool

            Case 4
                txtNameCd4.Visible = _bool
                txtName4.Visible = _bool
                txtCngTime4.Visible = _bool
                txtCngDayRevision4.Visible = _bool
                txtCngTimeRevision4.Visible = _bool
                txtRtnTime4.Visible = _bool
                txtRtnDayRevision4.Visible = _bool
                txtRtnTimeRevision4.Visible = _bool
                lbl4_1.Visible = _bool
                lbl4_2.Visible = _bool
                lbl4_3.Visible = _bool
                lblCng4.Visible = _bool
                lblRtn4.Visible = _bool

        End Select
    End Sub

    ''' <summary>
    ''' パラメータの設定
    ''' </summary>
    ''' <param name="_intSetindex"></param>
    ''' <remarks></remarks>
    Private Sub msSetParameters(ByRef _cmd As SqlCommand, ByVal _intSetindex As Integer)
        Dim workcode As String = Me.txtWorkCd.ppText.Trim
        Dim workname As String = Me.txtWork.ppText.Trim
        Dim systemcls As String = Me.hdnSyscls.Value
        Dim namecode As String = String.Empty
        Dim name As String = String.Empty
        Dim cngtime As String = String.Empty
        Dim cngdayrev As Integer = 0
        Dim cngtimerev As Integer = 0
        Dim rtntime As String = String.Empty
        Dim rtndayrev As Integer = 0
        Dim rtntimerev As Integer = 0

        Select Case _intSetindex
            Case 1
                namecode = Me.txtNameCd1.ppText.Trim
                name = Me.txtName1.ppText
                cngtime = mfTimeText(Me.txtCngTime1)
                cngdayrev = Me.txtCngDayRevision1.ppText.Trim
                cngtimerev = Me.txtCngTimeRevision1.ppText.Trim
                rtntime = mfTimeText(Me.txtRtnTime1)
                rtndayrev = Me.txtRtnDayRevision1.ppText.Trim
                rtntimerev = Me.txtRtnTimeRevision1.ppText.Trim
            Case 2
                namecode = Me.txtNameCd2.ppText.Trim
                name = Me.txtName2.ppText
                cngtime = mfTimeText(Me.txtCngTime2)
                cngdayrev = Me.txtCngDayRevision2.ppText.Trim
                cngtimerev = Me.txtCngTimeRevision2.ppText.Trim
                rtntime = mfTimeText(Me.txtRtnTime2)
                rtndayrev = Me.txtRtnDayRevision2.ppText.Trim
                rtntimerev = Me.txtRtnTimeRevision2.ppText.Trim
            Case 3
                namecode = Me.txtNameCd3.ppText.Trim
                name = Me.txtName3.ppText
                cngtime = mfTimeText(Me.txtCngTime3)
                cngdayrev = Me.txtCngDayRevision3.ppText.Trim
                cngtimerev = Me.txtCngTimeRevision3.ppText.Trim
                rtntime = mfTimeText(Me.txtRtnTime3)
                rtndayrev = Me.txtRtnDayRevision3.ppText.Trim
                rtntimerev = Me.txtRtnTimeRevision3.ppText.Trim
            Case 4
                namecode = Me.txtNameCd4.ppText.Trim
                name = Me.txtName4.ppText
                cngtime = mfTimeText(Me.txtCngTime4)
                cngdayrev = Me.txtCngDayRevision4.ppText.Trim
                cngtimerev = Me.txtCngTimeRevision4.ppText.Trim
                rtntime = mfTimeText(Me.txtRtnTime4)
                rtndayrev = Me.txtRtnDayRevision4.ppText.Trim
                rtntimerev = Me.txtRtnTimeRevision4.ppText.Trim
        End Select

        With _cmd.Parameters
            .Clear()
            .Add(pfSet_Param("workcode", SqlDbType.NVarChar, workcode))
            .Add(pfSet_Param("workname", SqlDbType.NVarChar, workname))
            .Add(pfSet_Param("systemcls", SqlDbType.NVarChar, systemcls))
            .Add(pfSet_Param("namecode", SqlDbType.NVarChar, namecode))
            .Add(pfSet_Param("name", SqlDbType.NVarChar, name))
            .Add(pfSet_Param("cngtime", SqlDbType.NVarChar, cngtime))
            .Add(pfSet_Param("cngdayrev", SqlDbType.Int, cngdayrev))
            .Add(pfSet_Param("cngtimerev", SqlDbType.Int, cngtimerev))
            .Add(pfSet_Param("rtntime", SqlDbType.NVarChar, rtntime))
            .Add(pfSet_Param("rtndayrev", SqlDbType.Int, rtndayrev))
            .Add(pfSet_Param("rtntimerev", SqlDbType.Int, rtntimerev))
            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))
            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
        End With
    End Sub

    ''' <summary>
    ''' 項目数を返します
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>有効化された項目数をカウントして返します</remarks>
    Private Function mfGetNameRecordCount() As Integer
        Dim intRtn As Integer = 0
        If lbl1_1.Visible = True Then
            intRtn += 1
        End If
        If lbl2_1.Visible = True Then
            intRtn += 1
        End If
        If lbl3_1.Visible = True Then
            intRtn += 1
        End If
        If lbl4_1.Visible = True Then
            intRtn += 1
        End If
        Return intRtn
    End Function

    ''' <summary>
    ''' スクリプトの埋め込み
    ''' </summary>
    ''' <remarks>設定時刻のテキストボックスはサーバー側イベントで処理しますので補正値のテキストボックスのみ埋め込みます
    ''' 補正値でのポストバックが必要になったので、スクリプトは不要になりました</remarks>
    Private Sub msSetScript()
        Dim cng1 As String = "calconblur(" & txtCngTime1.ppHourBox.ClientID & _
                                                    "," & txtCngTime1.ppMinBox.ClientID & _
                                                    "," & txtCngDayRevision1.ppTextBox.ClientID & _
                                                    "," & txtCngTimeRevision1.ppTextBox.ClientID & _
                                                    "," & lblCng1.ClientID & ");"
        Dim cng2 As String = "calconblur(" & txtCngTime2.ppHourBox.ClientID & _
                                                    "," & txtCngTime2.ppMinBox.ClientID & _
                                                    "," & txtCngDayRevision2.ppTextBox.ClientID & _
                                                    "," & txtCngTimeRevision2.ppTextBox.ClientID & _
                                                    "," & lblCng2.ClientID & ");"
        Dim cng3 As String = "calconblur(" & txtCngTime3.ppHourBox.ClientID & _
                                                    "," & txtCngTime3.ppMinBox.ClientID & _
                                                    "," & txtCngDayRevision3.ppTextBox.ClientID & _
                                                    "," & txtCngTimeRevision3.ppTextBox.ClientID & _
                                                    "," & lblCng3.ClientID & ");"
        Dim cng4 As String = "calconblur(" & txtCngTime4.ppHourBox.ClientID & _
                                                    "," & txtCngTime4.ppMinBox.ClientID & _
                                                    "," & txtCngDayRevision4.ppTextBox.ClientID & _
                                                    "," & txtCngTimeRevision4.ppTextBox.ClientID & _
                                                    "," & lblCng4.ClientID & ");"

        Dim rtn1 As String = "calconblur(" & txtRtnTime1.ppHourBox.ClientID & _
                                                    "," & txtRtnTime1.ppMinBox.ClientID & _
                                                    "," & txtRtnDayRevision1.ppTextBox.ClientID & _
                                                    "," & txtRtnTimeRevision1.ppTextBox.ClientID & _
                                                    "," & lblRtn1.ClientID & ");"

        Dim rtn2 As String = "calconblur(" & txtRtnTime2.ppHourBox.ClientID & _
                                                    "," & txtRtnTime2.ppMinBox.ClientID & _
                                                    "," & txtRtnDayRevision2.ppTextBox.ClientID & _
                                                    "," & txtRtnTimeRevision2.ppTextBox.ClientID & _
                                                    "," & lblRtn2.ClientID & ");"

        Dim rtn3 As String = "calconblur(" & txtRtnTime3.ppHourBox.ClientID & _
                                                    "," & txtRtnTime3.ppMinBox.ClientID & _
                                                    "," & txtRtnDayRevision3.ppTextBox.ClientID & _
                                                    "," & txtRtnTimeRevision3.ppTextBox.ClientID & _
                                                    "," & lblRtn3.ClientID & ");"

        Dim rtn4 As String = "calconblur(" & txtRtnTime4.ppHourBox.ClientID & _
                                                    "," & txtRtnTime4.ppMinBox.ClientID & _
                                                    "," & txtRtnDayRevision4.ppTextBox.ClientID & _
                                                    "," & txtRtnTimeRevision4.ppTextBox.ClientID & _
                                                    "," & lblRtn4.ClientID & ");"

        'txtCngTime1.ppHourBox.Attributes.Add("onblur", cng1)
        'txtCngTime2.ppHourBox.Attributes.Add("onblur", cng2)
        'txtCngTime3.ppHourBox.Attributes.Add("onblur", cng3)
        'txtCngTime4.ppHourBox.Attributes.Add("onblur", cng4)
        'txtCngTime1.ppMinBox.Attributes.Add("onblur", cng1)
        'txtCngTime2.ppMinBox.Attributes.Add("onblur", cng2)
        'txtCngTime3.ppMinBox.Attributes.Add("onblur", cng3)
        'txtCngTime4.ppMinBox.Attributes.Add("onblur", cng4)

        'txtRtnTime1.ppHourBox.Attributes.Add("onblur", cng1)
        'txtRtnTime2.ppHourBox.Attributes.Add("onblur", cng2)
        'txtRtnTime3.ppHourBox.Attributes.Add("onblur", cng3)
        'txtRtnTime4.ppHourBox.Attributes.Add("onblur", cng4)
        'txtRtnTime1.ppMinBox.Attributes.Add("onblur", cng1)
        'txtRtnTime2.ppMinBox.Attributes.Add("onblur", cng2)
        'txtRtnTime3.ppMinBox.Attributes.Add("onblur", cng3)
        'txtRtnTime4.ppMinBox.Attributes.Add("onblur", cng4)

        'txtCngDayRevision1.ppTextBox.Attributes.Add("onblur", cng1)
        'txtCngDayRevision2.ppTextBox.Attributes.Add("onblur", cng2)
        'txtCngDayRevision3.ppTextBox.Attributes.Add("onblur", cng3)
        'txtCngDayRevision4.ppTextBox.Attributes.Add("onblur", cng4)

        'txtCngTimeRevision1.ppTextBox.Attributes.Add("onblur", cng1)
        'txtCngTimeRevision2.ppTextBox.Attributes.Add("onblur", cng2)
        'txtCngTimeRevision3.ppTextBox.Attributes.Add("onblur", cng3)
        'txtCngTimeRevision4.ppTextBox.Attributes.Add("onblur", cng4)

        'txtRtnDayRevision1.ppTextBox.Attributes.Add("onblur", rtn1)
        'txtRtnDayRevision2.ppTextBox.Attributes.Add("onblur", rtn2)
        'txtRtnDayRevision3.ppTextBox.Attributes.Add("onblur", rtn3)
        'txtRtnDayRevision4.ppTextBox.Attributes.Add("onblur", rtn4)

        'txtRtnTimeRevision1.ppTextBox.Attributes.Add("onblur", rtn1)
        'txtRtnTimeRevision2.ppTextBox.Attributes.Add("onblur", rtn2)
        'txtRtnTimeRevision3.ppTextBox.Attributes.Add("onblur", rtn3)
        'txtRtnTimeRevision4.ppTextBox.Attributes.Add("onblur", rtn4)

        'txtCngDayRevision1.ppTextBox.Attributes.Add("onfocus", "deletezero(" & txtCngDayRevision1.ppTextBox.ClientID & ");")
        'txtCngDayRevision2.ppTextBox.Attributes.Add("onfocus", "deletezero(" & txtCngDayRevision2.ppTextBox.ClientID & ");")
        'txtCngDayRevision3.ppTextBox.Attributes.Add("onfocus", "deletezero(" & txtCngDayRevision3.ppTextBox.ClientID & ");")
        'txtCngDayRevision4.ppTextBox.Attributes.Add("onfocus", "deletezero(" & txtCngDayRevision4.ppTextBox.ClientID & ");")

        'txtCngTimeRevision1.ppTextBox.Attributes.Add("onfocus", "deletezero(" & txtCngTimeRevision1.ppTextBox.ClientID & ");")
        'txtCngTimeRevision2.ppTextBox.Attributes.Add("onfocus", "deletezero(" & txtCngTimeRevision2.ppTextBox.ClientID & ");")
        'txtCngTimeRevision3.ppTextBox.Attributes.Add("onfocus", "deletezero(" & txtCngTimeRevision3.ppTextBox.ClientID & ");")
        'txtCngTimeRevision4.ppTextBox.Attributes.Add("onfocus", "deletezero(" & txtCngTimeRevision4.ppTextBox.ClientID & ");")

        'txtRtnDayRevision1.ppTextBox.Attributes.Add("onfocus", "deletezero(" & txtRtnDayRevision1.ppTextBox.ClientID & ");")
        'txtRtnDayRevision2.ppTextBox.Attributes.Add("onfocus", "deletezero(" & txtRtnDayRevision2.ppTextBox.ClientID & ");")
        'txtRtnDayRevision3.ppTextBox.Attributes.Add("onfocus", "deletezero(" & txtRtnDayRevision3.ppTextBox.ClientID & ");")
        'txtRtnDayRevision4.ppTextBox.Attributes.Add("onfocus", "deletezero(" & txtRtnDayRevision4.ppTextBox.ClientID & ");")

        'txtRtnTimeRevision1.ppTextBox.Attributes.Add("onfocus", "deletezero(" & txtRtnTimeRevision1.ppTextBox.ClientID & ");")
        'txtRtnTimeRevision2.ppTextBox.Attributes.Add("onfocus", "deletezero(" & txtRtnTimeRevision2.ppTextBox.ClientID & ");")
        'txtRtnTimeRevision3.ppTextBox.Attributes.Add("onfocus", "deletezero(" & txtRtnTimeRevision3.ppTextBox.ClientID & ");")
        'txtRtnTimeRevision4.ppTextBox.Attributes.Add("onfocus", "deletezero(" & txtRtnTimeRevision4.ppTextBox.ClientID & ");")

    End Sub

    ''' <summary>
    ''' 変更時間活性制御
    ''' </summary>
    ''' <param name="_strClientID"></param>
    ''' <remarks>同一日時登録のみ許容する変更時間の活性制御</remarks>
    Private Sub msLinkFieldEnable(ByVal _strClientID As String)
        Dim isCng = False
        Dim timebox As ClsCMTimeBox
        Dim rdaybox As ClsCMTextBox
        Dim rtimbox As ClsCMTextBox

        Select Case _strClientID
            Case txtCngTime1.ppHourBox.ClientID, txtCngTime1.ppMinBox.ClientID, txtCngDayRevision1.ppTextBox.ClientID, txtCngTimeRevision1.ppTextBox.ClientID
                timebox = txtCngTime1
                rdaybox = txtCngDayRevision1
                rtimbox = txtCngTimeRevision1
                isCng = True
            Case txtCngTime2.ppHourBox.ClientID, txtCngTime2.ppMinBox.ClientID, txtCngDayRevision2.ppTextBox.ClientID, txtCngTimeRevision2.ppTextBox.ClientID
                timebox = txtCngTime2
                rdaybox = txtCngDayRevision2
                rtimbox = txtCngTimeRevision2
                isCng = True
            Case txtCngTime3.ppHourBox.ClientID, txtCngTime3.ppMinBox.ClientID, txtCngDayRevision3.ppTextBox.ClientID, txtCngTimeRevision3.ppTextBox.ClientID
                timebox = txtCngTime3
                rdaybox = txtCngDayRevision3
                rtimbox = txtCngTimeRevision3
                isCng = True
            Case txtCngTime4.ppHourBox.ClientID, txtCngTime4.ppMinBox.ClientID, txtCngDayRevision4.ppTextBox.ClientID, txtCngTimeRevision4.ppTextBox.ClientID
                timebox = txtCngTime4
                rdaybox = txtCngDayRevision4
                rtimbox = txtCngTimeRevision4
                isCng = True
            Case txtRtnTime1.ppHourBox.ClientID, txtRtnTime1.ppMinBox.ClientID, txtRtnDayRevision1.ppTextBox.ClientID, txtRtnTimeRevision1.ppTextBox.ClientID
                timebox = txtRtnTime1
                rdaybox = txtRtnDayRevision1
                rtimbox = txtRtnTimeRevision1
                isCng = False
            Case txtRtnTime2.ppHourBox.ClientID, txtRtnTime2.ppMinBox.ClientID, txtRtnDayRevision2.ppTextBox.ClientID, txtRtnTimeRevision2.ppTextBox.ClientID
                timebox = txtRtnTime2
                rdaybox = txtRtnDayRevision2
                rtimbox = txtRtnTimeRevision2
                isCng = False
            Case txtRtnTime3.ppHourBox.ClientID, txtRtnTime3.ppMinBox.ClientID, txtRtnDayRevision3.ppTextBox.ClientID, txtRtnTimeRevision3.ppTextBox.ClientID
                timebox = txtRtnTime3
                rdaybox = txtRtnDayRevision3
                rtimbox = txtRtnTimeRevision3
                isCng = False
            Case txtRtnTime4.ppHourBox.ClientID, txtRtnTime4.ppMinBox.ClientID, txtRtnDayRevision4.ppTextBox.ClientID, txtRtnTimeRevision4.ppTextBox.ClientID
                timebox = txtRtnTime4
                rdaybox = txtRtnDayRevision4
                rtimbox = txtRtnTimeRevision4
                isCng = False
            Case Else
                timebox = Nothing
                rdaybox = Nothing
                rtimbox = Nothing

        End Select

        If timebox Is Nothing OrElse rdaybox Is Nothing OrElse rtimbox Is Nothing Then
            Return
        End If

        Dim hour As String = timebox.ppHourText.Trim
        If hour = String.Empty Then
            hour = "00"
        End If
        Dim min As String = timebox.ppMinText.Trim
        If min = String.Empty Then
            min = "00"
        End If
        Dim day As String = rdaybox.ppText.Trim
        If day = String.Empty Then
            day = "0"
        End If
        Dim time As String = rtimbox.ppText.Trim
        If time = String.Empty Then
            time = "0"
        End If

        Select Case ddlSWork.ppSelectedValue

            Case WRKCODE_SINYA
                'Ⅰ.特別営業設定(深夜)　　IC ： ユーザの任意登録を許容する。
                '                   　　　LUT： 変更時間　①運用モード・②精算可能日数、③精算機オフライン時間
                '　　　　　                        上記変更を1回の電文処理で行う為、同一の変更時間を登録とする。
                '                               戻し時間　②精算可能日数、③精算機オフライン時間
                '　　　　                          上記変更を1回の電文処理で行う為、同一の戻し時間を登録とする。
                Select Case ddlSSystemCls.ppSelectedValue
                    Case SYSCODE_IC
                    Case SYSCODE_LT
                        If isCng Then
                            Select Case timebox.ClientID
                                'Case txtCngTime1.ClientID, txtCngTime2.ClientID, txtCngTime3.ClientID
                                Case txtCngTime1.ClientID
                                    txtCngTime1.ppHourText = hour
                                    'txtCngTime2.ppHourText = hour
                                    'txtCngTime3.ppHourText = hour
                                    txtCngTime1.ppMinText = min
                                    'txtCngTime2.ppMinText = min
                                    'txtCngTime3.ppMinText = min
                                    txtCngDayRevision1.ppText = day
                                    'txtCngDayRevision2.ppText = day
                                    'txtCngDayRevision3.ppText = day
                                    txtCngTimeRevision1.ppText = time
                                    'txtCngTimeRevision2.ppText = time
                                    'txtCngTimeRevision3.ppText = time

                                    txtCngTime1.ppEnabled = False
                                    'txtCngTime2.ppEnabled = False
                                    'txtCngTime3.ppEnabled = False
                                    txtCngDayRevision1.ppEnabled = False
                                    'txtCngDayRevision2.ppEnabled = False
                                    'txtCngDayRevision3.ppEnabled = False
                                    txtCngTimeRevision1.ppEnabled = False
                                    'txtCngTimeRevision2.ppEnabled = False
                                    'txtCngTimeRevision3.ppEnabled = False
                                Case txtCngTime2.ClientID
                                    'txtCngTime1.ppHourText = hour
                                    txtCngTime2.ppHourText = hour
                                    'txtCngTime3.ppHourText = hour
                                    'txtCngTime1.ppMinText = min
                                    txtCngTime2.ppMinText = min
                                    'txtCngTime3.ppMinText = min
                                    'txtCngDayRevision1.ppText = day
                                    txtCngDayRevision2.ppText = day
                                    'txtCngDayRevision3.ppText = day
                                    'txtCngTimeRevision1.ppText = time
                                    txtCngTimeRevision2.ppText = time
                                    'txtCngTimeRevision3.ppText = time

                                    'txtCngTime1.ppEnabled = False
                                    txtCngTime2.ppEnabled = False
                                    'txtCngTime3.ppEnabled = False
                                    'txtCngDayRevision1.ppEnabled = False
                                    txtCngDayRevision2.ppEnabled = False
                                    'txtCngDayRevision3.ppEnabled = False
                                    'txtCngTimeRevision1.ppEnabled = False
                                    txtCngTimeRevision2.ppEnabled = False
                                    'txtCngTimeRevision3.ppEnabled = False
                                Case txtCngTime3.ClientID
                                    'txtCngTime1.ppHourText = hour
                                    'txtCngTime2.ppHourText = hour
                                    txtCngTime3.ppHourText = hour
                                    'txtCngTime1.ppMinText = min
                                    'txtCngTime2.ppMinText = min
                                    txtCngTime3.ppMinText = min
                                    'txtCngDayRevision1.ppText = day
                                    'txtCngDayRevision2.ppText = day
                                    txtCngDayRevision3.ppText = day
                                    'txtCngTimeRevision1.ppText = time
                                    'txtCngTimeRevision2.ppText = time
                                    txtCngTimeRevision3.ppText = time

                                    txtCngTime1.ppEnabled = False
                                    'txtCngTime2.ppEnabled = False
                                    'txtCngTime3.ppEnabled = False
                                    txtCngDayRevision1.ppEnabled = False
                                    'txtCngDayRevision2.ppEnabled = False
                                    'txtCngDayRevision3.ppEnabled = False
                                    txtCngTimeRevision1.ppEnabled = False
                                    'txtCngTimeRevision2.ppEnabled = False
                                    'txtCngTimeRevision3.ppEnabled = False

                            End Select
                        Else
                            Select Case timebox.ClientID
                                'Case txtRtnTime2.ClientID, txtRtnTime3.ClientID
                                Case txtRtnTime2.ClientID
                                    txtRtnTime2.ppHourText = hour
                                    'txtRtnTime3.ppHourText = hour
                                    txtRtnTime2.ppMinText = min
                                    'txtRtnTime3.ppMinText = min
                                    txtRtnDayRevision2.ppText = day
                                    'txtRtnDayRevision3.ppText = day
                                    txtRtnTimeRevision2.ppText = time
                                    'txtRtnTimeRevision3.ppText = time

                                    txtRtnTime2.ppEnabled = False
                                    'txtRtnTime3.ppEnabled = False
                                    txtRtnDayRevision2.ppEnabled = False
                                    'txtRtnDayRevision3.ppEnabled = False
                                    txtRtnTimeRevision2.ppEnabled = False
                                    'txtRtnTimeRevision3.ppEnabled = False
                                Case txtRtnTime3.ClientID
                                    'txtRtnTime2.ppHourText = hour
                                    txtRtnTime3.ppHourText = hour
                                    'txtRtnTime2.ppMinText = min
                                    txtRtnTime3.ppMinText = min
                                    'txtRtnDayRevision2.ppText = day
                                    txtRtnDayRevision3.ppText = day
                                    'txtRtnTimeRevision2.ppText = time
                                    txtRtnTimeRevision3.ppText = time

                                    'txtRtnTime2.ppEnabled = False
                                    txtRtnTime3.ppEnabled = False
                                    'txtRtnDayRevision2.ppEnabled = False
                                    txtRtnDayRevision3.ppEnabled = False
                                    'txtRtnTimeRevision2.ppEnabled = False
                                    txtRtnTimeRevision3.ppEnabled = False
                            End Select
                        End If
                End Select

            Case WRKCODE_SYUYA
                'Ⅱ.特別営業設定(終夜) 　 IC ： 変更時間  ②精算可能日数、③オフライン運用設定
                '                               戻し時間　②精算可能日数、③オフライン運用設定
                '　　　　　　　　　           　　 上記変更を1回の電文処理で行う為、同一の変更時間を登録とする。
                '　　　　                 LUT： 変更時間  ①運用モード・②精算可能日数、③精算機オフライン時間は同一の変更時間を登録とする
                '                                         ④サンドオフライン時間はユーザの任意登録を許容する。
                '                               戻し時間　②精算可能日数、③精算機オフライン時間は同一の変更時間を登録とする。
                '                                         ①運用モード、④サンドオフライン時間はユーザ任意登録を許容する。
                Select Case ddlSSystemCls.ppSelectedValue
                    Case SYSCODE_IC
                        If isCng Then
                            Select Case timebox.ClientID
                                'Case txtCngTime2.ClientID, txtCngTime3.ClientID
                                Case txtCngTime2.ClientID
                                    txtCngTime2.ppHourText = hour
                                    'txtCngTime3.ppHourText = hour
                                    txtCngTime2.ppMinText = min
                                    'txtCngTime3.ppMinText = min
                                    txtCngDayRevision2.ppText = day
                                    'txtCngDayRevision3.ppText = day
                                    txtCngTimeRevision2.ppText = time
                                    'txtCngTimeRevision3.ppText = time

                                    txtCngTime2.ppEnabled = False
                                    'txtCngTime3.ppEnabled = False
                                    txtCngDayRevision2.ppEnabled = False
                                    'txtCngDayRevision3.ppEnabled = False
                                    txtCngTimeRevision2.ppEnabled = False
                                    'txtCngTimeRevision3.ppEnabled = False
                                Case txtCngTime3.ClientID
                                    'txtCngTime2.ppHourText = hour
                                    txtCngTime3.ppHourText = hour
                                    'txtCngTime2.ppMinText = min
                                    txtCngTime3.ppMinText = min
                                    'txtCngDayRevision2.ppText = day
                                    txtCngDayRevision3.ppText = day
                                    'txtCngTimeRevision2.ppText = time
                                    txtCngTimeRevision3.ppText = time

                                    'txtCngTime2.ppEnabled = False
                                    txtCngTime3.ppEnabled = False
                                    'txtCngDayRevision2.ppEnabled = False
                                    txtCngDayRevision3.ppEnabled = False
                                    'txtCngTimeRevision2.ppEnabled = False
                                    txtCngTimeRevision3.ppEnabled = False
                            End Select

                        Else
                            Select Case timebox.ClientID
                                'Case txtRtnTime2.ClientID, txtRtnTime3.ClientID
                                Case txtRtnTime2.ClientID
                                    txtRtnTime2.ppHourText = hour
                                    'txtRtnTime3.ppHourText = hour
                                    txtRtnTime2.ppMinText = min
                                    'txtRtnTime3.ppMinText = min
                                    txtRtnDayRevision2.ppText = day
                                    'txtRtnDayRevision3.ppText = day
                                    txtRtnTimeRevision2.ppText = time
                                    'txtRtnTimeRevision3.ppText = time

                                    txtRtnTime2.ppEnabled = False
                                    'txtRtnTime3.ppEnabled = False
                                    txtRtnDayRevision2.ppEnabled = False
                                    'txtRtnDayRevision3.ppEnabled = False
                                    txtRtnTimeRevision2.ppEnabled = False
                                    'txtRtnTimeRevision3.ppEnabled = False
                                Case txtRtnTime3.ClientID
                                    'txtRtnTime2.ppHourText = hour
                                    txtRtnTime3.ppHourText = hour
                                    'txtRtnTime2.ppMinText = min
                                    txtRtnTime3.ppMinText = min
                                    'txtRtnDayRevision2.ppText = day
                                    txtRtnDayRevision3.ppText = day
                                    'txtRtnTimeRevision2.ppText = time
                                    txtRtnTimeRevision3.ppText = time

                                    'txtRtnTime2.ppEnabled = False
                                    txtRtnTime3.ppEnabled = False
                                    'txtRtnDayRevision2.ppEnabled = False
                                    txtRtnDayRevision3.ppEnabled = False
                                    'txtRtnTimeRevision2.ppEnabled = False
                                    txtRtnTimeRevision3.ppEnabled = False
                            End Select
                        End If

                    Case SYSCODE_LT
                        If isCng Then
                            Select Case timebox.ClientID
                                'Case txtCngTime1.ClientID, txtCngTime2.ClientID, txtCngTime3.ClientID
                                Case txtCngTime1.ClientID
                                    txtCngTime1.ppHourText = hour
                                    'txtCngTime2.ppHourText = hour
                                    'txtCngTime3.ppHourText = hour
                                    txtCngTime1.ppMinText = min
                                    'txtCngTime2.ppMinText = min
                                    'txtCngTime3.ppMinText = min
                                    txtCngDayRevision1.ppText = day
                                    'txtCngDayRevision2.ppText = day
                                    'txtCngDayRevision3.ppText = day
                                    txtCngTimeRevision1.ppText = time
                                    'txtCngTimeRevision2.ppText = time
                                    'txtCngTimeRevision3.ppText = time

                                    txtCngTime1.ppEnabled = False
                                    'txtCngTime2.ppEnabled = False
                                    'txtCngTime3.ppEnabled = False
                                    txtCngDayRevision1.ppEnabled = False
                                    'txtCngDayRevision2.ppEnabled = False
                                    'txtCngDayRevision3.ppEnabled = False
                                    txtCngTimeRevision1.ppEnabled = False
                                    'txtCngTimeRevision2.ppEnabled = False
                                    'txtCngTimeRevision3.ppEnabled = False
                                Case txtCngTime2.ClientID
                                    'txtCngTime1.ppHourText = hour
                                    txtCngTime2.ppHourText = hour
                                    'txtCngTime3.ppHourText = hour
                                    'txtCngTime1.ppMinText = min
                                    txtCngTime2.ppMinText = min
                                    'txtCngTime3.ppMinText = min
                                    'txtCngDayRevision1.ppText = day
                                    txtCngDayRevision2.ppText = day
                                    'txtCngDayRevision3.ppText = day
                                    'txtCngTimeRevision1.ppText = time
                                    txtCngTimeRevision2.ppText = time
                                    'txtCngTimeRevision3.ppText = time

                                    'txtCngTime1.ppEnabled = False
                                    txtCngTime2.ppEnabled = False
                                    'txtCngTime3.ppEnabled = False
                                    'txtCngDayRevision1.ppEnabled = False
                                    txtCngDayRevision2.ppEnabled = False
                                    'txtCngDayRevision3.ppEnabled = False
                                    'txtCngTimeRevision1.ppEnabled = False
                                    txtCngTimeRevision2.ppEnabled = False
                                    'txtCngTimeRevision3.ppEnabled = False
                                Case txtCngTime3.ClientID
                                    'txtCngTime1.ppHourText = hour
                                    'txtCngTime2.ppHourText = hour
                                    txtCngTime3.ppHourText = hour
                                    'txtCngTime1.ppMinText = min
                                    'txtCngTime2.ppMinText = min
                                    txtCngTime3.ppMinText = min
                                    'txtCngDayRevision1.ppText = day
                                    'txtCngDayRevision2.ppText = day
                                    txtCngDayRevision3.ppText = day
                                    'txtCngTimeRevision1.ppText = time
                                    'txtCngTimeRevision2.ppText = time
                                    txtCngTimeRevision3.ppText = time

                                    'txtCngTime1.ppEnabled = False
                                    'txtCngTime2.ppEnabled = False
                                    txtCngTime3.ppEnabled = False
                                    'txtCngDayRevision1.ppEnabled = False
                                    'txtCngDayRevision2.ppEnabled = False
                                    txtCngDayRevision3.ppEnabled = False
                                    'txtCngTimeRevision1.ppEnabled = False
                                    'txtCngTimeRevision2.ppEnabled = False
                                    txtCngTimeRevision3.ppEnabled = False
                            End Select

                        Else
                            Select Case timebox.ClientID
                                'Case txtRtnTime2.ClientID, txtRtnTime3.ClientID
                                Case txtRtnTime2.ClientID
                                    txtRtnTime2.ppHourText = hour
                                    'txtRtnTime3.ppHourText = hour
                                    txtRtnTime2.ppMinText = min
                                    'txtRtnTime3.ppMinText = min
                                    txtRtnDayRevision2.ppText = day
                                    'txtRtnDayRevision3.ppText = day
                                    txtRtnTimeRevision2.ppText = time
                                    'txtRtnTimeRevision3.ppText = time

                                    txtRtnTime2.ppEnabled = False
                                    'txtRtnTime3.ppEnabled = False
                                    txtRtnDayRevision2.ppEnabled = False
                                    'txtRtnDayRevision3.ppEnabled = False
                                    txtRtnTimeRevision2.ppEnabled = False
                                    'txtRtnTimeRevision3.ppEnabled = False
                                Case txtRtnTime3.ClientID
                                    'txtRtnTime2.ppHourText = hour
                                    txtRtnTime3.ppHourText = hour
                                    'txtRtnTime2.ppMinText = min
                                    txtRtnTime3.ppMinText = min
                                    'txtRtnDayRevision2.ppText = day
                                    txtRtnDayRevision3.ppText = day
                                    'txtRtnTimeRevision2.ppText = time
                                    txtRtnTimeRevision3.ppText = time

                                    'txtRtnTime2.ppEnabled = False
                                    txtRtnTime3.ppEnabled = False
                                    'txtRtnDayRevision2.ppEnabled = False
                                    txtRtnDayRevision3.ppEnabled = False
                                    'txtRtnTimeRevision2.ppEnabled = False
                                    txtRtnTimeRevision3.ppEnabled = False
                            End Select
                        End If
                End Select

            Case WRKCODE_TMTNK
                'Ⅲ.玉単価変更許可設定　　変更時間・戻し時間　①運用モード、②玉単価設定許可は同一の変更時間を登録とする。
                'システム分類は0:共通
                If isCng Then
                    Select Case timebox.ClientID
                        Case txtCngTime1.ClientID, txtCngTime2.ClientID

                            txtCngDayRevision1.ppText = day
                            txtCngDayRevision2.ppText = day
                            txtCngTimeRevision1.ppText = time
                            txtCngTimeRevision2.ppText = time

                            txtCngTime1.ppEnabled = False
                            txtCngTime2.ppEnabled = False
                            txtCngDayRevision1.ppEnabled = False
                            txtCngDayRevision2.ppEnabled = False
                            txtCngTimeRevision1.ppEnabled = False
                            txtCngTimeRevision2.ppEnabled = False
                    End Select
                Else
                    Select Case timebox.ClientID
                        Case txtRtnTime1.ClientID, txtRtnTime2.ClientID

                            txtRtnDayRevision1.ppText = day
                            txtRtnDayRevision2.ppText = day
                            txtRtnTimeRevision1.ppText = time
                            txtRtnTimeRevision2.ppText = time

                            txtRtnTime1.ppEnabled = False
                            txtRtnTime2.ppEnabled = False
                            txtRtnDayRevision1.ppEnabled = False
                            txtRtnDayRevision2.ppEnabled = False
                            txtRtnTimeRevision1.ppEnabled = False
                            txtRtnTimeRevision2.ppEnabled = False
                    End Select
                End If

            Case WRKCODE_KNINK
                'Ⅳ.会員管理会社設定　　　変更時間・戻し時間　①会員サービスコード1、2、②エンコード情報の会員サービスコード1、2は同一の変更時間を登録とする。
                'システム分類は0:共通
                If isCng Then
                    Select Case timebox.ClientID
                        Case txtCngTime1.ClientID, txtCngTime2.ClientID

                            txtCngDayRevision1.ppText = day
                            txtCngDayRevision2.ppText = day
                            txtCngTimeRevision1.ppText = time
                            txtCngTimeRevision2.ppText = time

                            txtCngTime1.ppEnabled = False
                            txtCngTime2.ppEnabled = False
                            txtCngDayRevision1.ppEnabled = False
                            txtCngDayRevision2.ppEnabled = False
                            txtCngTimeRevision1.ppEnabled = False
                            txtCngTimeRevision2.ppEnabled = False
                    End Select
                Else
                    Select Case timebox.ClientID
                        Case txtRtnTime1.ClientID, txtRtnTime2.ClientID

                            txtRtnDayRevision1.ppText = day
                            txtRtnDayRevision2.ppText = day
                            txtRtnTimeRevision1.ppText = time
                            txtRtnTimeRevision2.ppText = time

                            txtRtnTime1.ppEnabled = False
                            txtRtnTime2.ppEnabled = False
                            txtRtnDayRevision1.ppEnabled = False
                            txtRtnDayRevision2.ppEnabled = False
                            txtRtnTimeRevision1.ppEnabled = False
                            txtRtnTimeRevision2.ppEnabled = False
                    End Select
                End If
        End Select

        '変更した元のコントロールは有効化
        timebox.ppEnabled = True
        rdaybox.ppEnabled = True
        rtimbox.ppEnabled = True

    End Sub

    ''' <summary>
    ''' 日時補正値を表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msDispDiffTime()
        lblCng1.Text = mfCalcTime(1, True)
        lblRtn1.Text = mfCalcTime(1, False)
        lblCng2.Text = mfCalcTime(2, True)
        lblRtn2.Text = mfCalcTime(2, False)
        lblCng3.Text = mfCalcTime(3, True)
        lblRtn3.Text = mfCalcTime(3, False)
        lblCng4.Text = mfCalcTime(4, True)
        lblRtn4.Text = mfCalcTime(4, False)
    End Sub

    ''' <summary>
    ''' 日時補正値を計算
    ''' </summary>
    ''' <param name="_intindex"></param>
    ''' <param name="_isCng"></param>
    ''' <returns></returns>
    ''' <remarks>スクリプト側(onblurイベント)とほぼ同じ処理です。</remarks>
    Private Function mfCalcTime(ByVal _intindex As Integer, ByVal _isCng As Boolean) As String
        Dim msecPerMinute As Decimal = 1000 * 60
        Dim msecPerHour As Decimal = msecPerMinute * 60
        Dim msecPerDay As Decimal = msecPerHour * 24
        Dim minusflg As Boolean = False
        Dim minutes As Decimal = 0
        Dim hours As Decimal = 0
        Dim days As Decimal = 0
        Dim time As Decimal = 0
        Dim rtn As String = String.Empty
        Try
            Select Case ddlSWork.ppSelectedValue
                Case WRKCODE_SINYA, WRKCODE_SYUYA
                    Select Case _isCng
                        Case True
                            Select Case _intindex
                                Case 1
                                    If txtCngTime1.ppHourText.Trim <> String.Empty Then
                                        hours = txtCngTime1.ppHourText
                                    End If
                                    If txtCngTime1.ppMinText.Trim <> String.Empty Then
                                        minutes = txtCngTime1.ppMinText
                                    End If
                                    If txtCngDayRevision1.ppText.Trim <> String.Empty Then
                                        days = txtCngDayRevision1.ppText
                                    End If
                                    If txtCngTimeRevision1.ppText.Trim <> String.Empty Then
                                        time = txtCngTimeRevision1.ppText
                                    End If
                                Case 2
                                    If txtCngTime2.ppHourText.Trim <> String.Empty Then
                                        hours = txtCngTime2.ppHourText
                                    End If
                                    If txtCngTime2.ppMinText.Trim <> String.Empty Then
                                        minutes = txtCngTime2.ppMinText
                                    End If
                                    If txtCngDayRevision2.ppText.Trim <> String.Empty Then
                                        days = txtCngDayRevision2.ppText
                                    End If
                                    If txtCngTimeRevision2.ppText.Trim <> String.Empty Then
                                        time = txtCngTimeRevision2.ppText
                                    End If
                                Case 3
                                    If txtCngTime3.ppHourText.Trim <> String.Empty Then
                                        hours = txtCngTime3.ppHourText
                                    End If
                                    If txtCngTime3.ppMinText.Trim <> String.Empty Then
                                        minutes = txtCngTime3.ppMinText
                                    End If
                                    If txtCngDayRevision3.ppText.Trim <> String.Empty Then
                                        days = txtCngDayRevision3.ppText
                                    End If
                                    If txtCngTimeRevision3.ppText.Trim <> String.Empty Then
                                        time = txtCngTimeRevision3.ppText
                                    End If
                                Case 4
                                    If txtCngTime4.ppHourText.Trim <> String.Empty Then
                                        hours = txtCngTime4.ppHourText
                                    End If
                                    If txtCngTime4.ppMinText.Trim <> String.Empty Then
                                        minutes = txtCngTime4.ppMinText
                                    End If
                                    If txtCngDayRevision4.ppText.Trim <> String.Empty Then
                                        days = txtCngDayRevision4.ppText
                                    End If
                                    If txtCngTimeRevision4.ppText.Trim <> String.Empty Then
                                        time = txtCngTimeRevision4.ppText
                                    End If
                            End Select
                        Case False
                            Select Case _intindex
                                Case 1
                                    If txtRtnTime1.ppHourText.Trim <> String.Empty Then
                                        hours = txtRtnTime1.ppHourText
                                    End If
                                    If txtRtnTime1.ppMinText.Trim <> String.Empty Then
                                        minutes = txtRtnTime1.ppMinText
                                    End If
                                    If txtRtnDayRevision1.ppText.Trim <> String.Empty Then
                                        days = txtRtnDayRevision1.ppText
                                    End If
                                    If txtRtnTimeRevision1.ppText.Trim <> String.Empty Then
                                        time = txtRtnTimeRevision1.ppText
                                    End If
                                Case 2
                                    If txtRtnTime2.ppHourText.Trim <> String.Empty Then
                                        hours = txtRtnTime2.ppHourText
                                    End If
                                    If txtRtnTime2.ppMinText.Trim <> String.Empty Then
                                        minutes = txtRtnTime2.ppMinText
                                    End If
                                    If txtRtnDayRevision2.ppText.Trim <> String.Empty Then
                                        days = txtRtnDayRevision2.ppText
                                    End If
                                    If txtRtnTimeRevision2.ppText.Trim <> String.Empty Then
                                        time = txtRtnTimeRevision2.ppText
                                    End If
                                Case 3
                                    If txtRtnTime3.ppHourText.Trim <> String.Empty Then
                                        hours = txtRtnTime3.ppHourText
                                    End If
                                    If txtRtnTime3.ppMinText.Trim <> String.Empty Then
                                        minutes = txtRtnTime3.ppMinText
                                    End If
                                    If txtRtnDayRevision3.ppText.Trim <> String.Empty Then
                                        days = txtRtnDayRevision3.ppText
                                    End If
                                    If txtRtnTimeRevision3.ppText.Trim <> String.Empty Then
                                        time = txtRtnTimeRevision3.ppText
                                    End If
                                Case 4
                                    If txtRtnTime4.ppHourText.Trim <> String.Empty Then
                                        hours = txtRtnTime4.ppHourText
                                    End If
                                    If txtRtnTime4.ppMinText.Trim <> String.Empty Then
                                        minutes = txtRtnTime4.ppMinText
                                    End If
                                    If txtRtnDayRevision4.ppText.Trim <> String.Empty Then
                                        days = txtRtnDayRevision4.ppText
                                    End If
                                    If txtRtnTimeRevision4.ppText.Trim <> String.Empty Then
                                        time = txtRtnTimeRevision4.ppText
                                    End If
                            End Select
                    End Select

                    '基準日(2000/4/1)を作成し、基準日への加算により依頼の作動時刻を取得、元との差分から日数補正値を取得します。
                    '基準日　main：変更を加えない元の基準値保持用　dt：計算用
                    Dim main As New DateTime(2000, 4, 1, hours, minutes, 0)
                    Dim dt As DateTime = main
                    dt = dt.AddDays(days)
                    dt = dt.AddMinutes(time)

                    '日数差分の取得
                    Dim rtnTime As TimeSpan = dt.Date - main.Date
                    days = rtnTime.Days.ToString

                    '作動時刻の取得
                    hours = dt.Hour
                    minutes = dt.Minute

                    If days <> 0 Then
                        If days < 0 Then
                            rtn = Math.Abs(days).ToString + "日前 "
                        Else
                            rtn = Math.Abs(days).ToString + "日後 "
                        End If
                    Else
                        rtn = "当日 "
                    End If
                    rtn = rtn + hours.ToString("00") + ":" + minutes.ToString("00")

                    Return rtn

                Case Else
                    Dim rday As Decimal = 0
                    Dim rtim As Decimal = 0

                    Select Case _isCng
                        Case True
                            Select Case _intindex
                                Case 1
                                    If txtCngDayRevision1.ppText <> String.Empty Then
                                        rday = txtCngDayRevision1.ppText
                                    End If
                                    If txtCngTimeRevision1.ppText <> String.Empty Then
                                        rtim = txtCngTimeRevision1.ppText
                                    End If
                                Case 2
                                    If txtCngDayRevision2.ppText <> String.Empty Then
                                        rday = txtCngDayRevision2.ppText
                                    End If
                                    If txtCngTimeRevision2.ppText <> String.Empty Then
                                        rtim = txtCngTimeRevision2.ppText
                                    End If
                                Case 3
                                    If txtCngDayRevision3.ppText <> String.Empty Then
                                        rday = txtCngDayRevision3.ppText
                                    End If
                                    If txtCngTimeRevision3.ppText <> String.Empty Then
                                        rtim = txtCngTimeRevision3.ppText
                                    End If
                                Case 4
                                    If txtCngDayRevision4.ppText <> String.Empty Then
                                        rday = txtCngDayRevision4.ppText
                                    End If
                                    If txtCngTimeRevision4.ppText <> String.Empty Then
                                        rtim = txtCngTimeRevision4.ppText
                                    End If
                            End Select
                        Case False
                            Select Case _intindex
                                Case 1
                                    If txtRtnDayRevision1.ppText <> String.Empty Then
                                        rday = txtRtnDayRevision1.ppText
                                    End If
                                    If txtRtnTimeRevision1.ppText <> String.Empty Then
                                        rtim = txtRtnTimeRevision1.ppText
                                    End If
                                Case 2
                                    If txtRtnDayRevision2.ppText <> String.Empty Then
                                        rday = txtRtnDayRevision2.ppText
                                    End If
                                    If txtRtnTimeRevision2.ppText <> String.Empty Then
                                        rtim = txtRtnTimeRevision2.ppText
                                    End If
                                Case 3
                                    If txtRtnDayRevision3.ppText <> String.Empty Then
                                        rday = txtRtnDayRevision3.ppText
                                    End If
                                    If txtRtnTimeRevision3.ppText <> String.Empty Then
                                        rtim = txtRtnTimeRevision3.ppText
                                    End If
                                Case 4
                                    If txtRtnDayRevision4.ppText <> String.Empty Then
                                        rday = txtRtnDayRevision4.ppText
                                    End If
                                    If txtRtnTimeRevision4.ppText <> String.Empty Then
                                        rtim = txtRtnTimeRevision4.ppText
                                    End If
                            End Select
                    End Select

                    time = rday * msecPerDay + rtim * msecPerMinute

                    '補正値をミリ秒単位に変換して合算後、再度日時単位に戻します。
                    ' ゼロ対応
                    Dim tmp As Decimal
                    If Decimal.TryParse(time, tmp) = False OrElse tmp = 0 Then
                        Return String.Empty
                    End If

                    ' マイナス対応(日時計算を+で行う為、符号反転しマイナスフラグを保持する)
                    If time < 0 Then
                        minusflg = True
                        time *= -1
                    End If

                    '日時計算 分(合算値/ミリ秒(1000*60)) → 時(分/60) → [日数確定]日(時/24) → [時確定]時 - 日 → [分確定]分 - (日 + 時)
                    '例：　　 1500分 → 25時 → 1日 → 25時 - 1日(24時) = 1時 → 1500分 -( 1日(24時 = 1440分) + 1時(60分)) = 0分
                    '         　⇒ 1日+1時間+0分
                    ' ミリ秒→日時分
                    minutes = Math.Floor(time / msecPerMinute)
                    hours = Math.Floor(minutes / 60)
                    days = Math.Floor(hours / 24)
                    ' 各単位超過分を再計算(例：30時間 → 1日 + 6時間)
                    hours = hours - days * 24
                    minutes = minutes - ((days * 24 + hours) * 60)
                    '日時計算ここまで

                    If days <> 0 Then
                        rtn = days.ToString + "日"
                    End If
                    If hours <> 0 Then
                        rtn = rtn + hours.ToString + "時間"
                    End If
                    If minutes <> 0 Then
                        rtn = rtn + minutes.ToString + "分"
                    End If
                    If rtn <> String.Empty Then
                        If minusflg Then
                            Return rtn + "前"
                        Else
                            Return rtn + "後"
                        End If
                    Else
                        Return rtn
                    End If

            End Select


        Catch ex As Exception
            Return String.Empty
        End Try

    End Function

    ''' <summary>
    ''' エラーサマリー編集
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msEditValid()
        Dim cuv As CustomValidator
        cuv = txtCngTime1.FindControl("pnlErr").FindControl("cuvTimeBox")
        cuv.ErrorMessage = "設定項目１ 変更：" & cuv.ErrorMessage
        cuv = txtCngTime2.FindControl("pnlErr").FindControl("cuvTimeBox")
        cuv.ErrorMessage = "設定項目２ 変更：" & cuv.ErrorMessage
        cuv = txtCngTime3.FindControl("pnlErr").FindControl("cuvTimeBox")
        cuv.ErrorMessage = "設定項目３ 変更：" & cuv.ErrorMessage
        cuv = txtCngTime4.FindControl("pnlErr").FindControl("cuvTimeBox")
        cuv.ErrorMessage = "設定項目４ 変更：" & cuv.ErrorMessage
        cuv = txtRtnTime1.FindControl("pnlErr").FindControl("cuvTimeBox")
        cuv.ErrorMessage = "設定項目１ 戻し：" & cuv.ErrorMessage
        cuv = txtRtnTime2.FindControl("pnlErr").FindControl("cuvTimeBox")
        cuv.ErrorMessage = "設定項目２ 戻し：" & cuv.ErrorMessage
        cuv = txtRtnTime3.FindControl("pnlErr").FindControl("cuvTimeBox")
        cuv.ErrorMessage = "設定項目３ 戻し：" & cuv.ErrorMessage
        cuv = txtRtnTime4.FindControl("pnlErr").FindControl("cuvTimeBox")
        cuv.ErrorMessage = "設定項目４ 戻し：" & cuv.ErrorMessage

        cuv = txtCngDayRevision1.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "設定項目１ 変更：" & cuv.ErrorMessage
        cuv = txtCngDayRevision2.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "設定項目２ 変更：" & cuv.ErrorMessage
        cuv = txtCngDayRevision3.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "設定項目３ 変更：" & cuv.ErrorMessage
        cuv = txtCngDayRevision4.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "設定項目４ 変更：" & cuv.ErrorMessage
        cuv = txtRtnDayRevision1.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "設定項目１ 戻し：" & cuv.ErrorMessage
        cuv = txtRtnDayRevision2.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "設定項目２ 戻し：" & cuv.ErrorMessage
        cuv = txtRtnDayRevision3.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "設定項目３ 戻し：" & cuv.ErrorMessage
        cuv = txtRtnDayRevision4.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "設定項目４ 戻し：" & cuv.ErrorMessage

        cuv = txtCngTimeRevision1.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "設定項目１ 変更：" & cuv.ErrorMessage
        cuv = txtCngTimeRevision2.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "設定項目２ 変更：" & cuv.ErrorMessage
        cuv = txtCngTimeRevision3.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "設定項目３ 変更：" & cuv.ErrorMessage
        cuv = txtCngTimeRevision4.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "設定項目４ 変更：" & cuv.ErrorMessage
        cuv = txtRtnTimeRevision1.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "設定項目１ 戻し：" & cuv.ErrorMessage
        cuv = txtRtnTimeRevision2.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "設定項目２ 戻し：" & cuv.ErrorMessage
        cuv = txtRtnTimeRevision3.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "設定項目３ 戻し：" & cuv.ErrorMessage
        cuv = txtRtnTimeRevision4.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "設定項目４ 戻し：" & cuv.ErrorMessage

    End Sub

    Private Sub msNextCtrl(ByVal _clientid As String, ByRef _sendbox As TextBox, ByRef _nextbox As TextBox)
        Select Case _clientid

            Case txtCngTime1.ppHourBox.ClientID
                _sendbox = txtCngTime1.ppHourBox
                _nextbox = txtCngTime1.ppMinBox
            Case txtCngTime2.ppHourBox.ClientID
                _sendbox = txtCngTime2.ppHourBox
                _nextbox = txtCngTime2.ppMinBox
            Case txtCngTime3.ppHourBox.ClientID
                _sendbox = txtCngTime3.ppHourBox
                _nextbox = txtCngTime3.ppMinBox
            Case txtCngTime4.ppHourBox.ClientID
                _sendbox = txtCngTime4.ppHourBox
                _nextbox = txtCngTime4.ppMinBox
            Case txtRtnTime1.ppHourBox.ClientID
                _sendbox = txtRtnTime1.ppHourBox
                _nextbox = txtRtnTime1.ppMinBox
            Case txtRtnTime2.ppHourBox.ClientID
                _sendbox = txtRtnTime2.ppHourBox
                _nextbox = txtRtnTime2.ppMinBox
            Case txtRtnTime3.ppHourBox.ClientID
                _sendbox = txtRtnTime3.ppHourBox
                _nextbox = txtRtnTime3.ppMinBox
            Case txtRtnTime4.ppHourBox.ClientID
                _sendbox = txtRtnTime4.ppHourBox
                _nextbox = txtRtnTime4.ppMinBox
            Case txtCngTime1.ppMinBox.ClientID
                _sendbox = txtCngTime1.ppMinBox
                _nextbox = txtCngDayRevision1.ppTextBox
            Case txtCngTime2.ppMinBox.ClientID
                _sendbox = txtCngTime2.ppMinBox
                _nextbox = txtCngDayRevision2.ppTextBox
            Case txtCngTime3.ppMinBox.ClientID
                _sendbox = txtCngTime3.ppMinBox
                _nextbox = txtCngDayRevision3.ppTextBox
            Case txtCngTime4.ppMinBox.ClientID
                _sendbox = txtCngTime4.ppMinBox
                _nextbox = txtCngDayRevision4.ppTextBox
            Case txtRtnTime1.ppMinBox.ClientID
                _sendbox = txtRtnTime1.ppMinBox
                _nextbox = txtRtnDayRevision1.ppTextBox
            Case txtRtnTime2.ppMinBox.ClientID
                _sendbox = txtRtnTime2.ppMinBox
                _nextbox = txtRtnDayRevision2.ppTextBox
            Case txtRtnTime3.ppMinBox.ClientID
                _sendbox = txtRtnTime3.ppMinBox
                _nextbox = txtRtnDayRevision3.ppTextBox
            Case txtRtnTime4.ppMinBox.ClientID
                _sendbox = txtRtnTime4.ppMinBox
                _nextbox = txtRtnDayRevision4.ppTextBox
            Case txtCngDayRevision1.ppTextBox.ClientID
                _sendbox = txtCngDayRevision1.ppTextBox
                _nextbox = txtCngTimeRevision1.ppTextBox
            Case txtCngDayRevision2.ppTextBox.ClientID
                _sendbox = txtCngDayRevision2.ppTextBox
                _nextbox = txtCngTimeRevision2.ppTextBox
            Case txtCngDayRevision3.ppTextBox.ClientID
                _sendbox = txtCngDayRevision3.ppTextBox
                _nextbox = txtCngTimeRevision3.ppTextBox
            Case txtCngDayRevision4.ppTextBox.ClientID
                _sendbox = txtCngDayRevision4.ppTextBox
                _nextbox = txtCngTimeRevision4.ppTextBox
            Case txtRtnDayRevision1.ppTextBox.ClientID
                _sendbox = txtRtnDayRevision1.ppTextBox
                _nextbox = txtRtnTimeRevision1.ppTextBox
            Case txtRtnDayRevision2.ppTextBox.ClientID
                _sendbox = txtRtnDayRevision2.ppTextBox
                _nextbox = txtRtnTimeRevision2.ppTextBox
            Case txtRtnDayRevision3.ppTextBox.ClientID
                _sendbox = txtRtnDayRevision3.ppTextBox
                _nextbox = txtRtnTimeRevision3.ppTextBox
            Case txtRtnDayRevision4.ppTextBox.ClientID
                _sendbox = txtRtnDayRevision4.ppTextBox
                _nextbox = txtRtnTimeRevision4.ppTextBox
            Case txtCngTimeRevision1.ppTextBox.ClientID
                _sendbox = txtCngTimeRevision1.ppTextBox
                _nextbox = txtRtnTime1.ppHourBox
            Case txtCngTimeRevision2.ppTextBox.ClientID
                _sendbox = txtCngTimeRevision2.ppTextBox
                _nextbox = txtRtnTime2.ppHourBox
            Case txtCngTimeRevision3.ppTextBox.ClientID
                _sendbox = txtCngTimeRevision3.ppTextBox
                _nextbox = txtRtnTime3.ppHourBox
            Case txtCngTimeRevision4.ppTextBox.ClientID
                _sendbox = txtCngTimeRevision4.ppTextBox
                _nextbox = txtRtnTime4.ppHourBox
            Case txtRtnTimeRevision1.ppTextBox.ClientID
                _sendbox = txtRtnTimeRevision1.ppTextBox
                _nextbox = txtName2.ppTextBox
            Case txtRtnTimeRevision2.ppTextBox.ClientID
                _sendbox = txtRtnTimeRevision2.ppTextBox
                _nextbox = txtName3.ppTextBox
            Case txtRtnTimeRevision3.ppTextBox.ClientID
                _sendbox = txtRtnTimeRevision3.ppTextBox
                _nextbox = txtName4.ppTextBox
            Case txtRtnTimeRevision4.ppTextBox.ClientID
                _sendbox = txtRtnTimeRevision4.ppTextBox
                _nextbox = txtName1.ppTextBox
            Case Else
                'ここは通りません(エディタの警告回避用です)
                _sendbox = txtCngTime1.ppMinBox
                _nextbox = txtCngTime1.ppMinBox
        End Select
        If txtWorkCd.ppText <> WRKCODE_SINYA AndAlso txtWorkCd.ppText <> WRKCODE_SYUYA Then
            Select Case _nextbox.ClientID
                Case txtCngTime1.ppHourBox.ClientID
                    _nextbox = txtCngDayRevision1.ppTextBox
                Case txtRtnTime1.ppHourBox.ClientID
                    _nextbox = txtRtnDayRevision1.ppTextBox
                Case txtCngTime2.ppHourBox.ClientID
                    _nextbox = txtCngDayRevision2.ppTextBox
                Case txtRtnTime2.ppHourBox.ClientID
                    _nextbox = txtRtnDayRevision2.ppTextBox
                Case txtCngTime3.ppHourBox.ClientID
                    _nextbox = txtCngDayRevision3.ppTextBox
                Case txtRtnTime3.ppHourBox.ClientID
                    _nextbox = txtRtnDayRevision3.ppTextBox
                Case txtCngTime4.ppHourBox.ClientID
                    _nextbox = txtCngDayRevision4.ppTextBox
                Case txtRtnTime4.ppHourBox.ClientID
                    _nextbox = txtRtnDayRevision4.ppTextBox
            End Select

        End If

    End Sub



#End Region

End Class
