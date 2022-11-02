'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　業者マスタ
'*　ＰＧＭＩＤ：　COMUPDM44
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.06.11　：　栗原
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'COMUPDM44-001     2015/11/12      栗原　　　業者基本マスタの導入に伴い、レイアウト、検索条件等を変更する。

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

Public Class COMUPDM44

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
    Private Const DispCode As String = "COMUPDM44"                  '画面ID
    Private Const MasterName As String = "業者マスタ"           '画面名
    Private Const TableName As String = "M44_TRADER"        'テーブル名

    '業者基本マスタ画面パス
    Private Const M_MST_DISP_PATH_MA1 = "~/" & P_MST & "/" &
            P_FUN_COM & P_SCR_UPD & "M" & "A1" & "/" &
            P_FUN_COM & P_SCR_UPD & "M" & "A1.aspx"

    '保守担当者マスタ画面パス
    Private Const M_MST_DISP_PATH_M41 = "~/" & P_MST & "/" &
            P_FUN_COM & P_SCR_UPD & "M" & "41" & "/" &
            P_FUN_COM & P_SCR_UPD & "M" & "41.aspx"


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
    Dim strSQL As New StringBuilder
    Dim strMode As String = Nothing
    Dim strTrdCd As String = String.Empty
    Dim strStateCdF As String = String.Empty
    Dim strStateCdT As String = String.Empty
    Dim strCompCdF As String = String.Empty
    Dim strCompCdT As String = String.Empty
    Dim strOfficeCdF As String = String.Empty
    Dim strOfficeCdT As String = String.Empty
    Dim strSeqNoF As String = String.Empty
    Dim strSeqNoT As String = String.Empty
    Dim strDeleteFlg As String = String.Empty
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
        Dim intHeadCol As Integer() = New Integer() {2, 4, 6, 8, 11, 17}
        Dim intColSpan As Integer() = New Integer() {2, 2, 2, 2, 2, 2}
        pfSet_GridView(Me.grvList, DispCode, intHeadCol, intColSpan)
    End Sub

    ''' <summary>
    ''' Page Load
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        '遷移ボタン(業者マスタ)設定
        Dim btnFtMA1 As Button = DirectCast(Master.FindControl("btnLeft1"), Button)
        Dim btnFtM41 As Button = DirectCast(Master.FindControl("btnLeft2"), Button)
        btnFtMA1.Visible = True
        btnFtM41.Visible = True
        btnFtMA1.Text = "業者基本マスタ"
        btnFtM41.Text = "保守担当者マスタ"
        '*** TODO 導入時他マスタが導入されるならTrueにする ***
        'btnFtMA1.Enabled = False
        'btnFtM41.Enabled = False
        '*****************************************************

        'ボタンアクション設定
        AddHandler Master.ppBtnSearch.Click, AddressOf btnSearch_Click          '検索
        AddHandler Master.ppBtnSrcClear.Click, AddressOf btnSearchClear_Click   '検索条件クリア
        AddHandler Master.ppBtnInsert.Command, AddressOf btn_Click              '登録
        AddHandler Master.ppBtnUpdate.Command, AddressOf btn_Click              '更新
        AddHandler Master.ppBtnDelete.Command, AddressOf btn_Click              '削除
        AddHandler Master.ppBtnClear.Click, AddressOf btnClear_Click            'クリア
        AddHandler Me.btnGetSeq.Click, AddressOf btnGetSeq_Click
        AddHandler btnFtMA1.Click, AddressOf btnFtMA1_Click            '画面遷移(業者基本マスタ)
        AddHandler btnFtM41.Click, AddressOf btnFtM41_Click            '画面遷移(保守担当マスタ)

        'テキストチェンジ
        AddHandler Me.txtSeqNo.ppTextBox.TextChanged, AddressOf GetData
        AddHandler Me.ddlTrd1.ppDropDownList.SelectedIndexChanged, AddressOf GetComp
        AddHandler Me.ddlCompCd.ppDropDownList.SelectedIndexChanged, AddressOf GetEmployee
        AddHandler Me.ddlOfficeCd.ppDropDownList.SelectedIndexChanged, AddressOf ddlOffice_SelectedIndexChanged
        AddHandler Me.tftCompCd.ppTextBoxFrom.TextChanged, AddressOf tftCd_TextChanged
        AddHandler Me.tftCompCd.ppTextBoxTo.TextChanged, AddressOf tftCd_TextChanged
        AddHandler Me.tftOfficeCd.ppTextBoxFrom.TextChanged, AddressOf tftCd_TextChanged
        AddHandler Me.tftOfficeCd.ppTextBoxTo.TextChanged, AddressOf tftCd_TextChanged
        Me.txtSeqNo.ppTextBox.AutoPostBack = True
        Me.ddlTrd1.ppDropDownList.AutoPostBack = True
        Me.ddlCompCd.ppDropDownList.AutoPostBack = True
        Me.ddlOfficeCd.ppDropDownList.AutoPostBack = True
        Me.tftCompCd.ppTextBoxFrom.AutoPostBack = True
        Me.tftCompCd.ppTextBoxTo.AutoPostBack = True
        Me.tftOfficeCd.ppTextBoxFrom.AutoPostBack = True
        Me.tftOfficeCd.ppTextBoxTo.AutoPostBack = True

        'Me.ddlIntgrtCd.ppDropDownList.AutoPostBack = True
        'Me.ddlOfficeCd.ppDropDownList.AutoPostBack = True

        '削除ボタン非表示
        Master.ppBtnDelete.Visible = False

        Dim scm As ScriptManager
        scm = Master.FindControl("tsmManager")
        scm.RegisterPostBackControl(txtSeqNo)
        scm.RegisterPostBackControl(btnGetSeq)

        '検索項目の入力チェックの為にバリデーターのインスタンスと検証イベント生成
        Dim cuv_Comp As CustomValidator
        Dim cuv_Office As CustomValidator
        Dim cuv_Seq As CustomValidator

        cuv_Comp = tftCompCd.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv_Comp.EnableClientScript = True
        cuv_Office = tftOfficeCd.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv_Office.EnableClientScript = True
        cuv_Seq = tftSeq.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv_Seq.EnableClientScript = True

        AddHandler cuv_Comp.ServerValidate, AddressOf cuvComp_ServerValidate
        AddHandler cuv_Office.ServerValidate, AddressOf cuvOffice_ServerValidate
        AddHandler cuv_Seq.ServerValidate, AddressOf cuvSeq_ServerValidate
        AddHandler cuvState.ServerValidate, AddressOf cuvState_ServerValidate

        'エラーサマリー活性制御の為のUpdatePanelのUPdateMode調整
        Dim pnl As UpdatePanel
        pnl = Master.FindControl("UpdPanelMain")
        pnl.UpdateMode = UpdatePanelUpdateMode.Always

        If Not IsPostBack Then
            'プログラムＩＤ、画面名設定
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"

            'ビューステートの初期化
            ViewState("strSeqNoF") = String.Empty
            ViewState("strSeqNoT") = String.Empty
            ViewState("strTrdCd") = String.Empty
            ViewState("strStateCdF") = String.Empty
            ViewState("strStateCdT") = String.Empty
            ViewState("strCompCdF") = String.Empty
            ViewState("strCompCdT") = String.Empty
            ViewState("strOfficeCdF") = String.Empty
            ViewState("strOfficeCdT") = String.Empty
            ViewState("strDeleteFlg") = String.Empty

            'パンくずリスト設定
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            'ボタン押下時のメッセージ設定
            Master.ppBtnInsert.OnClientClick = pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '追加
            Master.ppBtnUpdate.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '更新

            'ドロップダウンリスト設定
            msSetddlSystem()
            msClearddl()

            'グリッドの初期化
            Me.grvList.DataSource = New DataTable
            grvList.DataBind()

            strMode = "Default"

        End If
        Me.ddlTrd.ppDropDownList.Focus()

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
                Master.ppMainEnabled = True
                Master.ppBtnDelete.Text = "削除"
                Me.txtSeqNo.ppTextBox.Enabled = True
                Me.btnGetSeq.Enabled = True
                Me.ddlTrd1.ppEnabled = False
                Me.ddlCompCd.ppEnabled = False
                Me.ddlOfficeCd.ppEnabled = False
                Me.ddlArea.ppEnabled = False
            Case "Select"
                Master.ppBtnInsert.Enabled = False     '追加
                Master.ppBtnUpdate.Enabled = True      '更新
                Master.ppBtnDelete.Enabled = True      '削除
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppMainEnabled = True
                '業者マスタ上からは削除を許容しない為、削除データ如何による活性制御は無視するが、
                '業者基本マスタの削除データは削除データとして扱い、グリッドの赤字表示のみ実施する。
                'If Master.ppBtnDelete.Text = "削除取消" Then
                '    Master.ppBtnUpdate.Enabled = False
                '    Master.ppMainEnabled = False
                'Else
                '    Master.ppBtnUpdate.Enabled = True      '更新
                '    Master.ppMainEnabled = True
                'End If
                Master.ppBtnUpdate.Enabled = True      '更新
                Master.ppMainEnabled = True
                Me.txtSeqNo.ppTextBox.Enabled = False
                Me.btnGetSeq.Enabled = False
                Me.ddlTrd1.ppEnabled = True
                Me.ddlCompCd.ppEnabled = True
                Me.ddlOfficeCd.ppEnabled = True
                Me.ddlArea.ppEnabled = True
            Case "Insert"
                Master.ppBtnInsert.Enabled = True     '追加
                Master.ppBtnDelete.Enabled = False      '削除
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppMainEnabled = True
                Master.ppBtnDelete.Text = "削除"
                Me.txtSeqNo.ppTextBox.Enabled = False
                Me.btnGetSeq.Enabled = False
                Me.ddlTrd1.ppEnabled = True
                Me.ddlCompCd.ppEnabled = True
                Me.ddlOfficeCd.ppEnabled = True
                Me.ddlArea.ppEnabled = True
        End Select

        '*******連番207のデータはサポートセンタ情報の為、一切の編集を不可にする。*******
        If txtSeqNo.ppText = "207" Then
            Master.ppBtnInsert.Enabled = False      '追加
            Master.ppBtnUpdate.Enabled = False     '更新
            Master.ppBtnDelete.Enabled = False     '削除
            Master.ppBtnClear.Enabled = True       'クリア
            Master.ppMainEnabled = True
            Master.ppBtnDelete.Text = "削除"
            Me.txtSeqNo.ppTextBox.Enabled = False
            Me.btnGetSeq.Enabled = False
            Me.ddlTrd1.ppEnabled = False
            Me.ddlCompCd.ppEnabled = False
            Me.ddlOfficeCd.ppEnabled = False
            Me.ddlArea.ppEnabled = False
        End If

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

            ViewState("strTrdCd") = ddlTrd.ppSelectedValue.ToString.Trim
            ViewState("strStateCdF") = ddlPrefectureFm.SelectedValue.ToString.Trim
            ViewState("strStateCdT") = ddlPrefectureTo.SelectedValue.ToString.Trim
            ViewState("strCompCdF") = tftCompCd.ppFromText.ToString.Trim
            ViewState("strCompCdT") = tftCompCd.ppToText.ToString.Trim
            ViewState("strOfficeCdF") = tftOfficeCd.ppFromText.ToString.Trim
            ViewState("strOfficeCdT") = tftOfficeCd.ppToText.ToString.Trim
            ViewState("strSeqNoF") = tftSeq.ppFromText.ToString.Trim
            ViewState("strSeqNoT") = tftSeq.ppToText.ToString.Trim
            ViewState("strDeleteFlg") = ddldel.ppSelectedValue

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

        ddlTrd.ppDropDownList.SelectedIndex = 0
        tftCompCd.ppTextBoxFrom.Text = ""
        tftCompCd.ppTextBoxTo.Text = ""
        tftOfficeCd.ppTextBoxTo.Text = ""
        tftOfficeCd.ppTextBoxFrom.Text = ""
        tftSeq.ppTextBoxFrom.Text = ""
        tftSeq.ppTextBoxTo.Text = ""
        ddlPrefectureFm.SelectedIndex = 0
        ddlPrefectureTo.SelectedIndex = 0
        ddldel.ppDropDownList.SelectedIndex = 0

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

        If Not sender.Equals(Master.ppBtnDelete) Then
            ' -1:NGC -2:営業所 -3:保守拠点 -8:SPC-
            If ddlTrd1.ppDropDownList.SelectedValue = Nothing OrElse _
                ddlTrd1.ppDropDownList.SelectedValue.ToString = "1" OrElse _
                ddlTrd1.ppDropDownList.SelectedValue.ToString = "2" OrElse _
                ddlTrd1.ppDropDownList.SelectedValue.ToString = "3" OrElse _
                ddlTrd1.ppDropDownList.SelectedValue.ToString = "8" Then

                '2:営業所 or 3:保守拠点 なら料金エリアと営業所の選択を必須とする
                If ddlTrd1.ppDropDownList.SelectedValue.ToString = "2" OrElse _
                   ddlTrd1.ppDropDownList.SelectedValue.ToString = "3" Then
                    ddlOfficeCd.ppRequiredField = True
                    ddlArea.ppRequiredField = True
                Else
                    ddlOfficeCd.ppRequiredField = False
                    ddlArea.ppRequiredField = False
                End If
            End If
            Page.Validate("val")
        End If

        If sender.Equals(Master.ppBtnDelete) OrElse (Page.IsValid) Then
            If mfIsCheck_Data() Then
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

        txtSeqNo.ppText = String.Empty
        ddlTrd1.ppDropDownList.SelectedIndex = 0
        ddlCompCd.ppDropDownList.SelectedIndex = -1
        ddlOfficeCd.ppDropDownList.SelectedIndex = -1
        lblIntgrtCd.Text = String.Empty
        lblZipNo.Text = String.Empty
        lblState.Text = String.Empty
        lblAddr.Text = String.Empty
        lblTelNo.Text = String.Empty
        lblFaxNo.Text = String.Empty
        lblEmTelNo.Text = String.Empty
        ddlArea.ppDropDownList.SelectedIndex = -1

        msClearddl()

        txtSeqNo.ppTextBox.Focus()
        strMode = "Default"

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 営業所変更処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ddlOffice_SelectedIndexChanged()
        msGet_BaseData(True)
    End Sub

    ''' <summary>
    ''' 画面遷移処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnFtMA1_Click()

        '開始ログ出力
        psLogStart(Me)

        'パンクズリストの設定
        Session(P_SESSION_BCLIST) = Master.ppBCList

        '業者マスタに遷移
        psOpen_Window(Me, M_MST_DISP_PATH_MA1)

        '終了ログ出力
        psLogEnd(Me)

    End Sub
    Protected Sub btnFtM41_Click()

        '開始ログ出力
        psLogStart(Me)

        'パンクズリストの設定
        Session(P_SESSION_BCLIST) = Master.ppBCList

        '業者マスタに遷移
        psOpen_Window(Me, M_MST_DISP_PATH_M41)

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 検索欄テキストチェンジ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tftCd_TextChanged(sender As Object, e As System.EventArgs)
        Dim strFocusId As String
        sender.text = mfGetNarrow(sender.text)

        Select Case sender.ClientID
            Case tftCompCd.ppTextBoxFrom.ClientID
                strFocusId = tftCompCd.ppTextBoxTo.ClientID
            Case tftCompCd.ppTextBoxTo.ClientID
                strFocusId = tftOfficeCd.ppTextBoxFrom.ClientID
            Case tftOfficeCd.ppTextBoxFrom.ClientID
                strFocusId = tftOfficeCd.ppTextBoxTo.ClientID
            Case tftOfficeCd.ppTextBoxTo.ClientID
                strFocusId = tftSeq.ppTextBoxFrom.ClientID
            Case Else
                strFocusId = ddlTrd.ppDropDownList.ClientID
        End Select
        Master.ppBtnDmy.Visible = True
        Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + strFocusId + ");"
        SetFocus(Master.ppBtnDmy.ClientID)
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

                '編集エリアに値を設定
                txtSeqNo.ppText = CType(rowData.FindControl("連番"), TextBox).Text
                ddlTrd1.ppDropDownList.SelectedValue = CType(rowData.FindControl("業者コード"), TextBox).Text
                GetComp()
                Dim lstItem As ListItem = ddlCompCd.ppDropDownList.Items.FindByValue(CType(rowData.FindControl("会社コード"), TextBox).Text)
                If lstItem Is Nothing Then
                    ddlCompCd.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                    ddlOfficeCd.ppDropDownList.Items.Add(New ListItem(Nothing, Nothing))
                    ddlCompCd.ppDropDownList.Items(1).Value = CType(rowData.FindControl("会社コード"), TextBox).Text
                    ddlCompCd.ppDropDownList.Items(1).Text = CType(rowData.FindControl("会社コード"), TextBox).Text & ":" & CType(rowData.FindControl("会社名"), TextBox).Text
                End If
                ddlCompCd.ppDropDownList.SelectedValue = CType(rowData.FindControl("会社コード"), TextBox).Text

                'ddlCompCd.ppSelectedValue = CType(rowData.FindControl("会社コード"), TextBox).Text
                GetEmployee()
                lstItem = ddlOfficeCd.ppDropDownList.Items.FindByValue(CType(rowData.FindControl("営業所コード"), TextBox).Text)
                If lstItem Is Nothing Then
                    ddlOfficeCd.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                    ddlOfficeCd.ppDropDownList.Items.Add(New ListItem(Nothing, Nothing))
                    ddlOfficeCd.ppDropDownList.Items(1).Value = CType(rowData.FindControl("営業所コード"), TextBox).Text
                    ddlOfficeCd.ppDropDownList.Items(1).Text = CType(rowData.FindControl("営業所コード"), TextBox).Text & ":" & CType(rowData.FindControl("営業所名"), TextBox).Text
                End If
                ddlOfficeCd.ppDropDownList.SelectedValue = CType(rowData.FindControl("営業所コード"), TextBox).Text

                'lblIntgrtCd.Text = mfGetMainte(CType(rowData.FindControl("営業所コード"), TextBox).Text)
                lblIntgrtCd.Text = CType(rowData.FindControl("統括保担コード"), TextBox).Text & ":" & CType(rowData.FindControl("統括保担名"), TextBox).Text
                If lblIntgrtCd.Text = ":" Then
                    lblIntgrtCd.Text = String.Empty
                End If

                msGet_BaseData()
                lblZipNo.Text = CType(rowData.FindControl("郵便番号"), TextBox).Text
                lblState.Text = CType(rowData.FindControl("都道府県コード"), TextBox).Text + ":" + CType(rowData.FindControl("都道府県"), TextBox).Text
                If lblState.Text = ":" Then
                    lblState.Text = String.Empty
                End If
                lblAddr.Text = CType(rowData.FindControl("住所"), TextBox).Text
                lblTelNo.Text = CType(rowData.FindControl("代表電話番号"), TextBox).Text
                lblFaxNo.Text = CType(rowData.FindControl("FAX番号"), TextBox).Text
                lblEmTelNo.Text = CType(rowData.FindControl("連絡電話番号"), TextBox).Text
                For ddlcnt As Integer = 0 To ddlArea.ppDropDownList.Items.Count
                    If ddlcnt < ddlArea.ppDropDownList.Items.Count Then
                        If ddlArea.ppDropDownList.Items(ddlcnt).Value = CType(rowData.FindControl("エリアコード"), TextBox).Text Then
                            ddlArea.ppDropDownList.SelectedValue = CType(rowData.FindControl("エリアコード"), TextBox).Text
                            Exit For
                        End If
                    Else
                        Me.ddlArea.ppDropDownList.Items(0).Value = CType(rowData.FindControl("エリアコード"), TextBox).Text
                        Me.ddlArea.ppDropDownList.Items(0).Text = CType(rowData.FindControl("エリアコード"), TextBox).Text + ":" + CType(rowData.FindControl("エリア"), TextBox).Text
                    End If
                Next
                'If CType(rowData.FindControl("削除"), TextBox).Text = "" Then
                '    Master.ppBtnDelete.Text = "削除"
                '    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '削除
                'Else
                '    Master.ppBtnDelete.Text = "削除取消"
                '    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
                'End If
                strMode = "Select"
            End If

            ddlTrd1.ppDropDownList.Focus()

        Catch ex As Exception
            'エラー
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
            objStack = New StackFrame
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
        setVal("OFF")
        'ログ出力終了
        psLogEnd(Me)
    End Sub
    Private Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound
        For i = 0 To grvList.Rows.Count - 1
            If CType(grvList.Rows(i).FindControl("削除会社"), TextBox).Text = 1 Then
                'For j As Integer = 2 To grvList.Rows(i).Cells.Count - 1
                '    DirectCast(grvList.Rows(i).Cells(j).Controls(0), TextBox).ForeColor = Drawing.Color.Red
                'Next
                'CType(grvList.Rows(i).FindControl("連番"), TextBox).ForeColor = Drawing.Color.Red
                'CType(grvList.Rows(i).FindControl("業者コード"), TextBox).ForeColor = Drawing.Color.Red
                'CType(grvList.Rows(i).FindControl("業者名"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(i).FindControl("会社コード"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(i).FindControl("会社名"), TextBox).ForeColor = Drawing.Color.Red
                'CType(grvList.Rows(i).FindControl("営業所コード"), TextBox).ForeColor = Drawing.Color.Red
                'CType(grvList.Rows(i).FindControl("営業所名"), TextBox).ForeColor = Drawing.Color.Red
                'CType(grvList.Rows(i).FindControl("統括保担コード"), TextBox).ForeColor = Drawing.Color.Red
                'CType(grvList.Rows(i).FindControl("統括保担名"), TextBox).ForeColor = Drawing.Color.Red
                'CType(grvList.Rows(i).FindControl("郵便番号"), TextBox).ForeColor = Drawing.Color.Red
                'CType(grvList.Rows(i).FindControl("都道府県コード"), TextBox).ForeColor = Drawing.Color.Red
                'CType(grvList.Rows(i).FindControl("都道府県"), TextBox).ForeColor = Drawing.Color.Red
                'CType(grvList.Rows(i).FindControl("住所"), TextBox).ForeColor = Drawing.Color.Red
                'CType(grvList.Rows(i).FindControl("代表電話番号"), TextBox).ForeColor = Drawing.Color.Red
                'CType(grvList.Rows(i).FindControl("FAX番号"), TextBox).ForeColor = Drawing.Color.Red
                'CType(grvList.Rows(i).FindControl("連絡電話番号"), TextBox).ForeColor = Drawing.Color.Red
                'CType(grvList.Rows(i).FindControl("エリアコード"), TextBox).ForeColor = Drawing.Color.Red
                'CType(grvList.Rows(i).FindControl("エリア"), TextBox).ForeColor = Drawing.Color.Red
            End If
            If CType(grvList.Rows(i).FindControl("削除営業所"), TextBox).Text = 1 Then
                'CType(grvList.Rows(i).FindControl("連番"), TextBox).ForeColor = Drawing.Color.Red
                'CType(grvList.Rows(i).FindControl("業者コード"), TextBox).ForeColor = Drawing.Color.Red
                'CType(grvList.Rows(i).FindControl("業者名"), TextBox).ForeColor = Drawing.Color.Red
                'CType(grvList.Rows(i).FindControl("会社コード"), TextBox).ForeColor = Drawing.Color.Red
                'CType(grvList.Rows(i).FindControl("会社名"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(i).FindControl("営業所コード"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(i).FindControl("営業所名"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(i).FindControl("統括保担コード"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(i).FindControl("統括保担名"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(i).FindControl("郵便番号"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(i).FindControl("都道府県コード"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(i).FindControl("都道府県"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(i).FindControl("住所"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(i).FindControl("代表電話番号"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(i).FindControl("FAX番号"), TextBox).ForeColor = Drawing.Color.Red
                CType(grvList.Rows(i).FindControl("連絡電話番号"), TextBox).ForeColor = Drawing.Color.Red
                'CType(grvList.Rows(i).FindControl("エリアコード"), TextBox).ForeColor = Drawing.Color.Red
                'CType(grvList.Rows(i).FindControl("エリア"), TextBox).ForeColor = Drawing.Color.Red
            End If
        Next
    End Sub

#End Region

    ''' <summary>
    ''' 検索欄桁数チェック(会社コード)
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuvComp_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Dim cuv As CustomValidator
        'If tftCompCd.ppFromText.Trim <> String.Empty AndAlso tftCompCd.ppToText.Trim <> String.Empty _
        '    AndAlso tftCompCd.ppFromText.Length > tftCompCd.ppToText.Length Then

        '    cuv = tftCompCd.FindControl("pnlErr").FindControl("cuvTextBox")
        '    cuv.Text = ClsCMCommon.pfGet_ValMes("2001", tftCompCd.ppName).Item(P_VALMES_SMES).ToString
        '    cuv.ErrorMessage = ClsCMCommon.pfGet_ValMes("2001", tftCompCd.ppName).Item(P_VALMES_MES).ToString
        '    args.IsValid = False
        'End If
    End Sub

    ''' <summary>
    ''' 検索欄桁数チェック(営業所コード)
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuvOffice_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Dim cuv As CustomValidator
        'If tftOfficeCd.ppFromText.Trim <> String.Empty AndAlso tftOfficeCd.ppToText.Trim <> String.Empty _
        '  AndAlso tftOfficeCd.ppFromText.Length > tftOfficeCd.ppToText.Length Then
        '    cuv = tftOfficeCd.FindControl("pnlErr").FindControl("cuvTextBox")
        '    cuv.Text = ClsCMCommon.pfGet_ValMes("2001", tftOfficeCd.ppName).Item(P_VALMES_SMES).ToString
        '    cuv.ErrorMessage = ClsCMCommon.pfGet_ValMes("2001", tftOfficeCd.ppName).Item(P_VALMES_MES).ToString
        '    args.IsValid = False
        'End If
    End Sub

    ''' <summary>
    ''' 検索欄桁数チェック(連番)
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuvSeq_ServerValidate(source As Object, args As ServerValidateEventArgs)

        Dim cuv As CustomValidator
        If tftSeq.ppFromText.Trim <> String.Empty AndAlso tftSeq.ppToText.Trim <> String.Empty _
          AndAlso tftSeq.ppFromText.Length > tftSeq.ppToText.Length Then
            cuv = tftSeq.FindControl("pnlErr").FindControl("cuvTextBox")
            cuv.Text = ClsCMCommon.pfGet_ValMes("2001", tftSeq.ppName).Item(P_VALMES_SMES).ToString
            cuv.ErrorMessage = ClsCMCommon.pfGet_ValMes("2001", tftSeq.ppName).Item(P_VALMES_MES).ToString
            args.IsValid = False

        End If
    End Sub

    ''' <summary>
    ''' 検索欄整合性チェック(県コード)
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuvState_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If ddlPrefectureFm.SelectedIndex <> 0 AndAlso ddlPrefectureTo.SelectedIndex <> 0 _
          AndAlso ddlPrefectureFm.SelectedValue > ddlPrefectureTo.SelectedValue Then
            cuvState.Text = ClsCMCommon.pfGet_ValMes("2021", LabelState.Text).Item(P_VALMES_SMES).ToString
            cuvState.ErrorMessage = ClsCMCommon.pfGet_ValMes("2021", LabelState.Text).Item(P_VALMES_MES).ToString
            args.IsValid = False
        End If
    End Sub

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
                .Add(pfSet_Param("TrdCd", SqlDbType.NVarChar, ViewState("strTrdCd")))
                .Add(pfSet_Param("StateCdF", SqlDbType.NVarChar, ViewState("strStateCdF")))
                .Add(pfSet_Param("StateCdT", SqlDbType.NVarChar, ViewState("strStateCdT")))
                .Add(pfSet_Param("CompCdF", SqlDbType.NVarChar, ViewState("strCompCdF")))
                .Add(pfSet_Param("CompCdT", SqlDbType.NVarChar, ViewState("strCompCdT")))
                .Add(pfSet_Param("OfficeCdF", SqlDbType.NVarChar, ViewState("strOfficeCdF")))
                .Add(pfSet_Param("OfficeCdT", SqlDbType.NVarChar, ViewState("strOfficeCdT")))
                .Add(pfSet_Param("SeqNoF", SqlDbType.NVarChar, ViewState("strSeqNoF")))
                .Add(pfSet_Param("SeqNoT", SqlDbType.NVarChar, ViewState("strSeqNoT")))
                .Add(pfSet_Param("Progid", SqlDbType.NVarChar, DispCode))                               '画面ID
                .Add(pfSet_Param("DeleteFlg", SqlDbType.NVarChar, ViewState("strDeleteFlg")))
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
                            .Add(pfSet_Param("Seqno", SqlDbType.NVarChar, Me.txtSeqNo.ppText))            '連番
                            .Add(pfSet_Param("UserId", SqlDbType.NVarChar, Session(P_SESSION_USERID)))          'ユーザーID
                        End With
                        If getFlg = "0" Then
                            ViewState("strSeqNoF") = Me.txtSeqNo.ppText.Trim
                            ViewState("strSeqNoT") = String.Empty
                            ViewState("strTrdCd") = String.Empty
                            ViewState("strStateCdF") = String.Empty
                            ViewState("strStateCdT") = String.Empty
                            ViewState("strCompCdF") = String.Empty
                            ViewState("strCompCdT") = String.Empty
                            ViewState("strOfficeCdF") = String.Empty
                            ViewState("strOfficeCdT") = String.Empty
                            ViewState("strDeleteFlg") = String.Empty
                        End If
                    Case Else
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("SeqNo", SqlDbType.NVarChar, Me.txtSeqNo.ppText.Trim))                         '連番
                            .Add(pfSet_Param("TrdCd", SqlDbType.NVarChar, Me.ddlTrd1.ppDropDownList.SelectedValue))         '業者コード
                            .Add(pfSet_Param("CompCd", SqlDbType.NVarChar, mfSepColon(Me.ddlCompCd.ppSelectedText, 0)))     '会社コード
                            .Add(pfSet_Param("CompNm", SqlDbType.NVarChar, mfSepColon(Me.ddlCompCd.ppSelectedText, 1)))     '会社名
                            '.Add(pfSet_Param("IntGrtCd", SqlDbType.NVarChar, mfSepColon(Me.lblIntgrtCd.Text, 0)))           '統括コード
                            .Add(pfSet_Param("OfficeCd", SqlDbType.NVarChar, mfSepColon(Me.ddlOfficeCd.ppSelectedText, 0))) '営業所コード
                            .Add(pfSet_Param("OfficeNm", SqlDbType.NVarChar, mfSepColon(Me.ddlOfficeCd.ppSelectedText, 1))) '営業所名
                            '.Add(pfSet_Param("ZipNo", SqlDbType.NVarChar, Me.lblZipNo.Text.Trim))                           '郵便番号
                            '.Add(pfSet_Param("StateCd", SqlDbType.NVarChar, mfSepColon(lblState.Text, 0)))                  '都道府県コード
                            '.Add(pfSet_Param("Addr", SqlDbType.NVarChar, Me.lblAddr.Text))                                  '住所
                            '.Add(pfSet_Param("DeptTelNo", SqlDbType.NVarChar, Me.lblTelNo.Text))                            '代表電話番号
                            '.Add(pfSet_Param("FaxNo", SqlDbType.NVarChar, Me.lblFaxNo.Text))                                'FAX番号
                            '.Add(pfSet_Param("TelNo", SqlDbType.NVarChar, Me.lblEmTelNo.Text))                              '連絡電話番号
                            .Add(pfSet_Param("AreaCd", SqlDbType.NVarChar, Me.ddlArea.ppDropDownList.SelectedValue))        'エリアコード
                            .Add(pfSet_Param("UserId", SqlDbType.NVarChar, Session(P_SESSION_USERID)))                      'ユーザーID
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                               '戻り値
                        End With
                End Select

                ViewState("strSeqNoF") = Me.txtSeqNo.ppText.Trim
                ViewState("strSeqNoT") = String.Empty
                ViewState("strTrdCd") = String.Empty
                ViewState("strStateCdF") = String.Empty
                ViewState("strStateCdT") = String.Empty
                ViewState("strCompCdF") = String.Empty
                ViewState("strCompCdT") = String.Empty
                ViewState("strOfficeCdF") = String.Empty
                ViewState("strOfficeCdT") = String.Empty
                ViewState("strDeleteFlg") = String.Empty
                cmdDB.ExecuteNonQuery()

                'ストアド戻り値チェック
                intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)

                If intRtn = 1 Then
                    'If ddlIntgrtCd.ppSelectedValue = String.Empty Then
                    '    psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "統括保守担当が保守担当者マスタには登録されています。\n保守担当者マスタの確認")
                    'Else
                    '    psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "統括保守担当が保守担当者マスタと業者マスタで違います。\n保守担当者マスタも変更")
                    'End If
                End If
                trans.Commit()

                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName)
                If getFlg = "0" Then
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
    ''' 業者基本マスタ取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_BaseData(Optional ByVal blnDispInfo As Boolean = False)

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet
        objStack = New StackFrame

        Try

            '画面初期化
            msClearInfo()
            If ddlOfficeCd.ppSelectedValue = String.Empty Then
                SetFocus(ddlOfficeCd.ppDropDownList.ClientID)
                Exit Sub
            End If

            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Throw New Exception("")
            End If

            'パラメータ設定
            cmdDB = New SqlCommand(DispCode & "_S4", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("TrdCd", SqlDbType.NVarChar, ddlTrd1.ppSelectedValue))
                .Add(pfSet_Param("CmpCd", SqlDbType.NVarChar, "1"))
                .Add(pfSet_Param("Cd", SqlDbType.NVarChar, ddlOfficeCd.ppSelectedValue))
            End With

            'データ取得
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            If dstOrders.Tables(0).Rows.Count = 0 Then
            Else
                If blnDispInfo = True Then
                    '編集エリアに値を設定
                    lblZipNo.Text = dstOrders.Tables(0).Rows(0).Item("郵便番号1").ToString & "-" & dstOrders.Tables(0).Rows(0).Item("郵便番号2").ToString
                    If lblZipNo.Text = "-" Then
                        lblZipNo.Text = String.Empty
                    End If
                    lblState.Text = dstOrders.Tables(0).Rows(0).Item("都道府県コード").ToString & "：" & dstOrders.Tables(0).Rows(0).Item("都道府県").ToString
                    If lblState.Text = "：" Then
                        lblState.Text = String.Empty
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
                    lblEmTelNo.Text = dstOrders.Tables(0).Rows(0).Item("連絡TEL1").ToString & "-" & dstOrders.Tables(0).Rows(0).Item("連絡TEL2").ToString & "-" & dstOrders.Tables(0).Rows(0).Item("連絡TEL3").ToString
                    If lblEmTelNo.Text = "--" Then
                        lblEmTelNo.Text = String.Empty
                    End If

                    lblIntgrtCd.Text = mfGetMainte(ddlOfficeCd.ppSelectedValue)
                End If

                SetFocus(ddlArea.ppDropDownList.ClientID)
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
    ''' ドロップダウンリスト設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlSystem()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                '業者設定
                objCmd = New SqlCommand("ZMSTSEL005", objCn)
                With objCmd.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("Class", SqlDbType.NVarChar, "0069"))            '区分クラス
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                Me.ddlTrd.ppDropDownList.Items.Clear()
                Me.ddlTrd.ppDropDownList.DataSource = objDs.Tables(0)
                Me.ddlTrd.ppDropDownList.DataTextField = "リスト用"
                Me.ddlTrd.ppDropDownList.DataValueField = "コード"
                Me.ddlTrd.ppDropDownList.DataBind()
                Me.ddlTrd.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

                Me.ddlTrd1.ppDropDownList.Items.Clear()
                Me.ddlTrd1.ppDropDownList.DataSource = objDs.Tables(0)
                Me.ddlTrd1.ppDropDownList.DataTextField = "リスト用"
                Me.ddlTrd1.ppDropDownList.DataValueField = "コード"
                Me.ddlTrd1.ppDropDownList.DataBind()
                Me.ddlTrd1.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

                '都道府県設定
                objCmd = New SqlCommand("ZMSTSEL002", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                Me.ddlPrefectureFm.Items.Clear()
                Me.ddlPrefectureFm.DataSource = objDs.Tables(0)
                Me.ddlPrefectureFm.DataTextField = "項目名"
                Me.ddlPrefectureFm.DataValueField = "都道府県コード"
                Me.ddlPrefectureFm.DataBind()
                Me.ddlPrefectureFm.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

                Me.ddlPrefectureTo.Items.Clear()
                Me.ddlPrefectureTo.DataSource = objDs.Tables(0)
                Me.ddlPrefectureTo.DataTextField = "項目名"
                Me.ddlPrefectureTo.DataValueField = "都道府県コード"
                Me.ddlPrefectureTo.DataBind()
                Me.ddlPrefectureTo.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

                'Me.ddlState.ppDropDownList.Items.Clear()
                'Me.ddlState.ppDropDownList.DataSource = objDs.Tables(0)
                'Me.ddlState.ppDropDownList.DataTextField = "項目名"
                'Me.ddlState.ppDropDownList.DataValueField = "都道府県コード"
                'Me.ddlState.ppDropDownList.DataBind()
                'Me.ddlState.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

                'エリアコード設定
                objCmd = New SqlCommand("ZMSTSEL005", objCn)
                With objCmd.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("Class", SqlDbType.NVarChar, "0080"))            '区分クラス
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                Me.ddlArea.ppDropDownList.Items.Clear()
                Me.ddlArea.ppDropDownList.DataSource = objDs.Tables(0)
                Me.ddlArea.ppDropDownList.DataTextField = "リスト用"
                Me.ddlArea.ppDropDownList.DataValueField = "コード"
                Me.ddlArea.ppDropDownList.DataBind()
                Me.ddlArea.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ機種マスタ一覧取得")
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
    ''' ドロップダウンリスト初期化(会社・営業所・保担)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearddl()
        Me.ddlOfficeCd.ppDropDownList.Items.Clear()
        Me.ddlCompCd.ppDropDownList.Items.Clear()
    End Sub

    ''' <summary>
    ''' 情報欄(住所、TEL等)初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearInfo()
        'lblIntgrtCd.Text = String.Empty
        lblZipNo.Text = String.Empty
        lblState.Text = String.Empty
        lblAddr.Text = String.Empty
        lblTelNo.Text = String.Empty
        lblFaxNo.Text = String.Empty
        lblEmTelNo.Text = String.Empty
    End Sub

    ''' <summary>
    ''' 営業所/部署取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetEmployee()
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim dsData As DataSet
        Dim strOfcFlg As String = Nothing

        msClearInfo()
        If ddlCompCd.ppSelectedValue = String.Empty Then
            Me.ddlOfficeCd.ppDropDownList.Items.Clear()
            SetFocus(ddlCompCd.ppDropDownList.ClientID)
            Exit Sub
        End If

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            '業者基本マスタから内容を取得
            Try
                '会社が決まってから営業所を表示する。
                If ddlCompCd.ppSelectedValue <> String.Empty Then

                    objCmd = New SqlCommand(DispCode & "_S3", objCn)
                    With objCmd.Parameters
                        '--パラメータ設定
                        .Add(pfSet_Param("trd_dvs", SqlDbType.NVarChar, ddlTrd1.ppSelectedValue))
                        .Add(pfSet_Param("cmp_dvs", SqlDbType.NVarChar, "1"))
                    End With

                    'データ取得
                    dsData = clsDataConnect.pfGet_DataSet(objCmd)

                    Me.ddlOfficeCd.ppDropDownList.Items.Clear()
                    Me.ddlOfficeCd.ppDropDownList.DataSource = dsData.Tables(0)
                    Me.ddlOfficeCd.ppDropDownList.DataTextField = "項目名"
                    Me.ddlOfficeCd.ppDropDownList.DataValueField = "コード"
                    Me.ddlOfficeCd.ppDropDownList.DataBind()
                    Me.ddlOfficeCd.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                    SetFocus(ddlOfficeCd.ppDropDownList.ClientID)
                End If

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle)
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' 会社取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetComp()
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim dsData As DataSet
        Dim strOfcFlg As String = Nothing

        msClearInfo()
        Me.ddlOfficeCd.ppDropDownList.Items.Clear()
        If ddlTrd1.ppSelectedValue = String.Empty Then
            msClearddl()
            SetFocus(ddlTrd1.ppDropDownList.ClientID)
            Exit Sub
        End If

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            '業者基本マスタから内容を取得
            Try
                objCmd = New SqlCommand(DispCode & "_S3", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    .Add(pfSet_Param("trd_dvs", SqlDbType.NVarChar, ddlTrd1.ppSelectedValue))
                    .Add(pfSet_Param("cmp_dvs", SqlDbType.NVarChar, "0"))
                End With

                'データ取得
                dsData = clsDataConnect.pfGet_DataSet(objCmd)

                Me.ddlCompCd.ppDropDownList.Items.Clear()
                Me.ddlCompCd.ppDropDownList.DataSource = dsData.Tables(0)
                Me.ddlCompCd.ppDropDownList.DataTextField = "項目名"
                Me.ddlCompCd.ppDropDownList.DataValueField = "コード"
                Me.ddlCompCd.ppDropDownList.DataBind()
                Me.ddlCompCd.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                SetFocus(ddlCompCd.ppDropDownList.ClientID)
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle)
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' 保守担当取得
    ''' </summary>
    ''' <param name="strCompCd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetMainte(ByVal strCompCd As String) As String
        mfGetMainte = ""
        If ddlTrd1.ppSelectedValue <> "2" AndAlso ddlTrd1.ppSelectedValue <> "3" Then
            '2:営業所と3:保守拠点以外なら統括保守担当を取得しない
            Exit Function
        End If

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim dsData As DataSet
        Dim strOfcFlg As String = Nothing

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            '保守担当者マスタから内容を取得
            Try
                objCmd = New SqlCommand(DispCode & "_S5", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    .Add(pfSet_Param("MainteCd", SqlDbType.NVarChar, strCompCd))
                End With

                'データ取得
                dsData = clsDataConnect.pfGet_DataSet(objCmd)

                If dsData.Tables(0).Rows.Count < 1 Then
                    mfGetMainte = ""
                Else
                    mfGetMainte = dsData.Tables(0).Rows(0).Item("統括保担").ToString
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
    End Function

    ''' <summary>
    ''' データ取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetData(sender As Object, e As EventArgs)
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim dsData As DataSet
        Dim strOfcFlg As String = Nothing


        'テキストチェンジ採番処理が競合した時用の時間稼ぎ
        '競合時はJavaScript側で動作をキャンセル(採番処理を優先)
        System.Threading.Thread.Sleep(50)

        setVal("ON")
        Page.Validate("key")
        If Not Page.IsValid Then
            txtSeqNo.ppTextBox.Focus()
            Exit Sub
        End If

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            '業者マスタから内容を取得
            Try
                If Not txtSeqNo.ppText = String.Empty Then
                    Dim seqno As Integer
                    If Integer.TryParse(txtSeqNo.ppText.Trim, seqno) Then
                        txtSeqNo.ppText = seqno
                    End If

                    objCmd = New SqlCommand(DispCode & "_S1", objCn)
                    With objCmd.Parameters
                        '--パラメータ設定
                        .Add(pfSet_Param("TrdCd", SqlDbType.NVarChar, String.Empty))
                        .Add(pfSet_Param("StateCdF", SqlDbType.NVarChar, String.Empty))
                        .Add(pfSet_Param("StateCdT", SqlDbType.NVarChar, String.Empty))
                        .Add(pfSet_Param("CompCdF", SqlDbType.NVarChar, String.Empty))
                        .Add(pfSet_Param("CompCdT", SqlDbType.NVarChar, String.Empty))
                        .Add(pfSet_Param("OfficeCdF", SqlDbType.NVarChar, String.Empty))
                        .Add(pfSet_Param("OfficeCdT", SqlDbType.NVarChar, String.Empty))
                        .Add(pfSet_Param("SeqNoF", SqlDbType.NVarChar, Me.txtSeqNo.ppText))
                        .Add(pfSet_Param("SeqNoT", SqlDbType.NVarChar, String.Empty))
                        .Add(pfSet_Param("Progid", SqlDbType.NVarChar, DispCode))                               '画面ID
                        .Add(pfSet_Param("DeleteFlg", SqlDbType.NVarChar, ""))
                    End With

                    'データ取得
                    dsData = clsDataConnect.pfGet_DataSet(objCmd)

                    If dsData.Tables(0).Rows.Count > 0 Then
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
                        arKey.Insert(0, dsData.Tables(0).Rows(0).Item("連番").ToString)

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
                        ddlTrd1.ppDropDownList.SelectedValue = dsData.Tables(0).Rows(0).Item("業者コード").ToString
                        GetComp()
                        Dim lstItem As ListItem = ddlCompCd.ppDropDownList.Items.FindByValue(dsData.Tables(0).Rows(0).Item("会社コード").ToString)
                        If lstItem Is Nothing Then
                            ddlCompCd.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                            ddlOfficeCd.ppDropDownList.Items.Add(New ListItem(Nothing, Nothing))
                            ddlCompCd.ppDropDownList.Items(1).Value = dsData.Tables(0).Rows(0).Item("会社コード").ToString
                            ddlCompCd.ppDropDownList.Items(1).Text = dsData.Tables(0).Rows(0).Item("会社コード").ToString & ":" & dsData.Tables(0).Rows(0).Item("会社名").ToString
                            'ddlCompCd.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                        End If
                        ddlCompCd.ppSelectedValue = dsData.Tables(0).Rows(0).Item("会社コード").ToString

                        GetEmployee()
                        lstItem = ddlOfficeCd.ppDropDownList.Items.FindByValue(dsData.Tables(0).Rows(0).Item("営業所コード").ToString)
                        If lstItem Is Nothing Then
                            ddlOfficeCd.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                            ddlOfficeCd.ppDropDownList.Items.Add(New ListItem(Nothing, Nothing))
                            ddlOfficeCd.ppDropDownList.Items(1).Value = dsData.Tables(0).Rows(0).Item("営業所コード").ToString
                            ddlOfficeCd.ppDropDownList.Items(1).Text = dsData.Tables(0).Rows(0).Item("営業所コード").ToString & ":" & dsData.Tables(0).Rows(0).Item("営業所名").ToString
                            'ddlOfficeCd.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                        End If
                        ddlOfficeCd.ppDropDownList.SelectedValue = dsData.Tables(0).Rows(0).Item("営業所コード").ToString

                        'lblIntgrtCd.Text = mfGetMainte(ddlOfficeCd.ppSelectedValue)
                        lblIntgrtCd.Text = dsData.Tables(0).Rows(0).Item("統括保担コード").ToString & ":" & dsData.Tables(0).Rows(0).Item("統括保担名").ToString
                        If lblIntgrtCd.Text = ":" Then
                            lblIntgrtCd.Text = String.Empty
                        End If
                        msGet_BaseData()
                        lblZipNo.Text = dsData.Tables(0).Rows(0).Item("郵便番号").ToString
                        lblState.Text = dsData.Tables(0).Rows(0).Item("都道府県コード").ToString & "：" & dsData.Tables(0).Rows(0).Item("都道府県").ToString
                        If lblState.Text = "：" Then
                            lblState.Text = String.Empty
                        End If
                        lblAddr.Text = dsData.Tables(0).Rows(0).Item("住所").ToString
                        lblTelNo.Text = dsData.Tables(0).Rows(0).Item("代表電話番号").ToString
                        lblFaxNo.Text = dsData.Tables(0).Rows(0).Item("FAX番号").ToString
                        lblEmTelNo.Text = dsData.Tables(0).Rows(0).Item("連絡電話番号").ToString
                        For ddlcnt As Integer = 0 To ddlArea.ppDropDownList.Items.Count
                            If ddlcnt < ddlArea.ppDropDownList.Items.Count Then
                                If ddlArea.ppDropDownList.Items(ddlcnt).Value = dsData.Tables(0).Rows(0).Item("エリアコード").ToString Then
                                    ddlArea.ppDropDownList.SelectedValue = dsData.Tables(0).Rows(0).Item("エリアコード").ToString
                                    Exit For
                                End If
                            Else
                                Me.ddlArea.ppDropDownList.Items(0).Value = dsData.Tables(0).Rows(0).Item("エリアコード").ToString
                                Me.ddlArea.ppDropDownList.Items(0).Text = dsData.Tables(0).Rows(0).Item("エリアコード").ToString + ":" + dsData.Tables(0).Rows(0).Item("エリア").ToString
                            End If
                        Next
                        'If dsData.Tables(0).Rows(0).Item("削除").ToString = "" Then
                        '    Master.ppBtnDelete.Text = "削除"
                        '    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '削除
                        'Else
                        '    Master.ppBtnDelete.Text = "削除取消"
                        '    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
                        'End If

                        strMode = "Select"
                    Else
                        strMode = "Insert"
                    End If

                    'フォーカスを移動
                    If Not txtSeqNo.ppText = String.Empty Then
                        If Master.ppBtnDelete.Text = "削除" Then
                            ddlTrd1.ppDropDownList.Focus()
                        Else
                            Master.ppBtnClear.Focus()
                        End If
                    Else
                        txtSeqNo.ppTextBox.Focus()
                    End If

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
    ''' 採番処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub btnGetSeq_Click()
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim dsData As DataSet

        '入力欄を初期化(連番テキストチェンジとの競合を避ける)
        btnClear_Click()

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            '番号を採番する
            Try

                objCmd = New SqlCommand(DispCode & "_S2", objCn)

                'データ取得
                dsData = clsDataConnect.pfGet_DataSet(objCmd)

                If dsData.Tables(0).Rows.Count > 0 Then
                    With dsData.Tables(0).Rows(0)
                        Me.txtSeqNo.ppText = .Item("NUM").ToString()
                        ViewState("seq") = .Item("NUM").ToString()
                    End With
                End If
                'フォーカスを移動
                'クリアボタンで表示されたダミーボタンを消す
                Master.ppBtnDmy.Visible = False
                ddlTrd1.ppDropDownList.Focus()
                strMode = "Insert"

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
        setVal("OFF")
    End Sub

    ''' <summary>
    '''フォーカス移動
    ''' </summary>
    ''' <param name="TxtBoxF"></param>
    ''' <param name="TxtBoxT"></param>
    ''' <remarks></remarks>
    Private Sub FocusChange(ByVal TxtBoxF As TextBox, ByVal TxtBoxT As Object)

        Dim strBtnDmy As String = Master.ppBtnDmy.ClientID
        Master.ppBtnDmy.Visible = True

        Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + TxtBoxT.ClientID + ");"
        SetFocus(Master.ppBtnDmy.ClientID)

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
    ''' 業者基本マスタとの整合性チェック
    ''' </summary>
    ''' <returns>True:検証OK　False:検証NG</returns>
    ''' <remarks></remarks>
    Private Function mfIsCheck_Data() As Boolean
        Dim strCheckResult As String = String.Empty
        strCheckResult = mfCheck_DeleteData()
        Select Case strCheckResult
            Case "True"
                mfIsCheck_Data = True
            Case "CmpDel"
                '会社情報削除済
                psMesBox(Me, "30011", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "会社の業者基本マスタ")
                mfIsCheck_Data = False
            Case "OfcDel"
                '営業所/部署情報削除済
                psMesBox(Me, "30011", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "営業所/部署の業者基本マスタ")
                mfIsCheck_Data = False
            Case "DblDel"
                '会社、営業所/部署情報共に削除済
                psMesBox(Me, "30011", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "業者基本マスタ")
                mfIsCheck_Data = False
            Case Else
                'その他例外エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "業者基本マスタ")
                mfIsCheck_Data = False
        End Select
    End Function

    ''' <summary>
    ''' 業者基本マスタの削除フラグチェック
    ''' </summary>
    ''' <returns>"True":削除フラグ0　その他：削除フラグ1</returns>
    ''' <remarks>戻り値はString型</remarks>
    Private Function mfCheck_DeleteData() As String
        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders1 As New DataSet
        Dim dstOrders2 As New DataSet
        objStack = New StackFrame
        mfCheck_DeleteData = "NoCheck"
        Try

            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Throw New Exception("")
            End If
            'パラメータ設定
            cmdDB = New SqlCommand(DispCode & "_S4", conDB)
            With cmdDB.Parameters
                .Add(pfSet_Param("TrdCd", SqlDbType.NVarChar, ddlTrd1.ppSelectedValue))
                .Add(pfSet_Param("CmpCd", SqlDbType.NVarChar, "0"))
                .Add(pfSet_Param("Cd", SqlDbType.NVarChar, ddlCompCd.ppSelectedValue))
                .Add(pfSet_Param("DelFlg", SqlDbType.NVarChar, "1"))
            End With
            'データ取得
            dstOrders1 = clsDataConnect.pfGet_DataSet(cmdDB)

            With cmdDB.Parameters
                .Clear()
                .Add(pfSet_Param("TrdCd", SqlDbType.NVarChar, ddlTrd1.ppSelectedValue))
                .Add(pfSet_Param("CmpCd", SqlDbType.NVarChar, "1"))
                .Add(pfSet_Param("Cd", SqlDbType.NVarChar, ddlOfficeCd.ppSelectedValue))
                .Add(pfSet_Param("DelFlg", SqlDbType.NVarChar, "1"))
            End With
            'データ取得
            dstOrders2 = clsDataConnect.pfGet_DataSet(cmdDB)

            If dstOrders1.Tables(0).Rows.Count <> 0 AndAlso dstOrders2.Tables(0).Rows.Count <> 0 Then
                mfCheck_DeleteData = "DblDel"
            ElseIf dstOrders1.Tables(0).Rows.Count <> 0 Then
                mfCheck_DeleteData = "CmpDel"
            ElseIf dstOrders2.Tables(0).Rows.Count <> 0 Then
                mfCheck_DeleteData = "OfcDel"
            Else
                mfCheck_DeleteData = "True"
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

    Private Sub setVal(ByVal OnOff As String)
        Dim cuv_Seq As CustomValidator
        Dim sum_Seq As ValidationSummary
        cuv_Seq = txtSeqNo.FindControl("pnlErr").FindControl("cuvTextBox")
        sum_Seq = Master.FindControl("UpdPanelMain").FindControl("ValidSumKey")

        Select Case OnOff
            Case "ON"
                cuv_Seq.Visible = True
                sum_Seq.Visible = True

            Case "OFF"
                cuv_Seq.Visible = False
                sum_Seq.Visible = False

        End Select
    End Sub

#End Region

#Region "終了処理プロシージャ"

#End Region
End Class
