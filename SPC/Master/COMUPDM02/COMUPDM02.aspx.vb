'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　社員マスタ
'*　ＰＧＭＩＤ：　COMUPDM02
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.02.25　：　星野
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'COMUPDM02-001     2016/11/14      栗原　　　レイアウト変更、進捗連絡機能対応
'COMUPDM02-002     2016/11/25      加賀　　　CRS 
'COMUPDM02-003     2016/12/14      栗原      SES/CRS レイアウト修正、同期処理追加
'COMUPDM02-004     2017/02/01      加賀      CRS区分追加


#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive

#End Region

Public Class COMUPDM02

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
    Const DispCode As String = "COMUPDM02"              '画面ID
    Const MasterName As String = "社員マスタ"           '画面名
    Const TableName As String = "M02_EMPLOYEE"          'テーブル名

#End Region

#Region "変数定義"
    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsExc As New ClsCMExclusive
    Dim clsMst As New ClsMSTCommon
    Dim objStack As StackFrame
    Dim strMode As String = Nothing

#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page Init
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        '社員マスタ/他社マスタ判定
        If Request.QueryString.Get("page").ToString = "1" Then
            ViewState(MasterName) = "社員マスタ"
        Else
            ViewState(MasterName) = "他社マスタ"
        End If

        'グリッドXML定義ファイル取得
        If ViewState(MasterName) = "社員マスタ" Then
            pfSet_GridView(Me.grvList, DispCode, {10, 12, 18, 20}, {2, 2, 2, 2})
        Else
            pfSet_GridView(Me.grvList, DispCode & "_2", {8, 10}, {2, 2})
        End If
    End Sub

    ''' <summary>
    ''' Page Load
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim btnFtSync As Button = DirectCast(Master.FindControl("btnRigth1"), Button)

        'ボタンアクション設定
        AddHandler Master.ppBtnSearch.Click, AddressOf btnSearch_Click          '検索
        AddHandler Master.ppBtnSrcClear.Click, AddressOf btnSearchClear_Click   '検索条件クリア
        AddHandler Master.ppBtnInsert.Command, AddressOf btn_Click              '登録
        AddHandler Master.ppBtnUpdate.Command, AddressOf btn_Click              '更新
        AddHandler Master.ppBtnDelete.Command, AddressOf btn_Click              '削除
        AddHandler Master.ppBtnClear.Click, AddressOf btnClear_Click            'クリア
        AddHandler btnFtSync.Click, AddressOf btnSync_Click                     '同期     'COMUPDM02-002

        'テキストボックスのイベント設定
        AddHandler txtEmpCd.ppTextBox.TextChanged, AddressOf txtEmpCd_TextChanged
        txtEmpCd.ppTextBox.AutoPostBack = True

        'ドロップダウンリストのイベント設定
        AddHandler ddlCrsDvs.ppDropDownList.SelectedIndexChanged, AddressOf ddlCrsDvs_SelectedIndexChanged
        ddlCrsDvs.ppDropDownList.AutoPostBack = True

        DirectCast(Master.FindControl("UpdPanelMain"), UpdatePanel).UpdateMode = UpdatePanelUpdateMode.Always
        DirectCast(Master.FindControl("UpdPanelSearch"), UpdatePanel).UpdateMode = UpdatePanelUpdateMode.Always

        If Not IsPostBack Then

            'プログラムＩＤ、画面名、件数設定
            Master.ppProgramID = DispCode
            Master.ppTitle = ViewState(MasterName)
            Master.ppCount = "0"

            'パンくずリスト設定
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            '削除データを含むを表示
            'Master.ppchksDel.Visible = True

            btnFtSync.Visible = True
            btnFtSync.Text = "同期"
            btnFtSync.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "電話番号マスタ") '同期

            'ドロップダウンリスト設定
            msSetddlComp()
            msSetddlOffice("Search")

            'チェックボックスリスト設定
            msSetchkListAuth()

            'グリッドの初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

            'データ取得
            'msGet_Data()

            'フォーカス設定
            SetFocus(tftSEmpCd.ppTextBoxFrom.ClientID)

            strMode = "Default"

        End If

        ''グリッド調整
        'grvList.Columns(9).HeaderStyle.CssClass = "GridNoDisp"
        'grvList.Columns(8).HeaderStyle.Width = 273
        'grvList.Columns(11).HeaderStyle.CssClass = "GridNoDisp"
        'grvList.Columns(10).HeaderStyle.Width = 273

        'メッセージ非表示
        lblACmp.Visible = False
        lblEmp.Visible = False

    End Sub

    ''' <summary>
    ''' Page PreRender
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        Select Case strMode
            Case "Default"
                Master.ppMainEnabled = True
                msSet_Enabled(False)
                txtEmpCd.ppEnabled = True
                Master.ppBtnInsert.Enabled = False     '追加
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppbtnGeneral.Enabled = False 'COMUPDM02-002
                Master.ppBtnDelete.Text = "削除"
            Case "Insert"
                Master.ppMainEnabled = True
                msSet_Enabled(True)
                txtEmpCd.ppEnabled = False
                Master.ppBtnInsert.Enabled = True      '追加
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppbtnGeneral.Enabled = False 'COMUPDM02-002
                Master.ppBtnDelete.Text = "削除"
            Case "Select"
                If Master.ppBtnDelete.Text = "削除" Then
                    Master.ppMainEnabled = True
                    Master.ppBtnUpdate.Enabled = True  '更新
                    Master.ppbtnGeneral.Enabled = True 'COMUPDM02-002
                Else
                    Master.ppMainEnabled = False
                    Master.ppBtnUpdate.Enabled = False
                    Master.ppbtnGeneral.Enabled = False 'COMUPDM02-002
                End If
                msSet_Enabled(True)
                txtEmpCd.ppEnabled = False
                Master.ppBtnInsert.Enabled = False     '追加
                Master.ppBtnDelete.Enabled = True      '削除
                Master.ppBtnClear.Enabled = True       'クリア
            Case "Enabled"
                Master.ppMainEnabled = False
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnInsert.Enabled = False     '追加
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnClear.Enabled = True       'クリア
                Master.ppbtnGeneral.Enabled = False 'COMUPDM02-002
        End Select

        If ddlComp.SelectedIndex = 0 Then
            ddlOffice.Enabled = False
        Else
            ddlOffice.Enabled = True
        End If

        If ViewState(MasterName) <> "社員マスタ" Then
            Label5.Visible = False
            Label6.Visible = False
            chkListAuth.Visible = False
            ddlAuth.Visible = False
            cuvAuth.Enabled = False
            ddlCrsDvs.ppEnabled = False
        End If

    End Sub

    ''' <summary>
    '''検索ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        'データ取得
        If (Page.IsValid) Then
            msGet_Data()
            'フォーカス設定
            SetFocus(tftSEmpCd.ppTextBoxFrom.ClientID)
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

        tftSEmpCd.ppFromText = String.Empty
        tftSEmpCd.ppToText = String.Empty
        txtSName.ppText = String.Empty
        txtSNameKana.ppText = String.Empty
        ddlSComp.SelectedIndex = -1
        msSetddlOffice("Search")
        ddlSOffice.SelectedIndex = -1
        For zz = 0 To chkListAuth.Items.Count - 1
            chkListAuth.Items(zz).Selected = False
        Next
        ddlsMailDvs.ppDropDownList.SelectedIndex = -1
        ddldel.ppSelectedValue = String.Empty

        'フォーカス設定
        SetFocus(tftSEmpCd.ppTextBoxFrom.ClientID)

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

        checkTELNO()

        checkMailAddress()

        'メール送信対象が範囲内に2件以下の状態を保たれているか確認
        If ddlComp.SelectedValue = "701" AndAlso ddlMailDvs.ppSelectedValue = "1" Then
            If mfMailReceiveEmployeeCount() = False Then
                Exit Sub
            End If
        End If

        If (Page.IsValid) Then
            'COMUPDM02-004
            'msEditData(e.CommandName)
            If msEditData(e.CommandName) Then
                'COMUPDM02-004 END

                '電話番号同期
                If (e.CommandName = "DELETE" AndAlso Master.ppBtnDelete.Text = P_BTN_NM_DEL) Then
                    mfDeleteTelNoMaster()
                Else
                    mfSyncTelNoMaster()
                End If

                'クリア
                btnClear_Click()

                'フォーカス設定
                SetFocus(tftSEmpCd.ppTextBoxFrom.ClientID)

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

        txtEmpCd.ppText = String.Empty
        txtLName.ppText = String.Empty
        txtFName.ppText = String.Empty
        txtLNameK.ppText = String.Empty
        txtFNameK.ppText = String.Empty
        txtSrtName.ppText = String.Empty
        txtTel1.ppText = String.Empty
        txtTel2.ppText = String.Empty
        txtTel3.ppText = String.Empty
        txtFax1.ppText = String.Empty
        txtFax2.ppText = String.Empty
        txtFax3.ppText = String.Empty
        ddlComp.SelectedIndex = -1
        ddlOffice.SelectedIndex = -1
        ddlAuth.SelectedIndex = -1
        'COMUPDM02-001
        ddlMailDvs.ppDropDownList.SelectedIndex = -1
        txtTelSp1_1.ppText = String.Empty
        txtTelSp1_2.ppText = String.Empty
        txtTelSp1_3.ppText = String.Empty
        txtTelSp2_1.ppText = String.Empty
        txtTelSp2_2.ppText = String.Empty
        txtTelSp2_3.ppText = String.Empty
        ddlVscDvs.ppDropDownList.SelectedIndex = -1
        txtMail.ppText = String.Empty
        txtDomain.Text = String.Empty
        'COMUPDM02-001 END
        'COMUPDM02-001
        hdnTelSp1.Value = String.Empty
        hdnTelSp2.Value = String.Empty
        'COMUPDM02-002 END
        ddlCrsDvs.ppDropDownList.Items(0).Enabled = True
        ddlCrsDvs.ppSelectedValue = String.Empty
        lblMailAdmin.Text = String.Empty

        'ドロップダウン設定
        msSetddlComp("Edit")
        ddlOffice.Items.Clear()
        msSetCrsDropDownList()

        'フォーカス設定
        SetFocus(txtEmpCd.ppTextBox.ClientID)

        strMode = "Default"

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 同期ボタン 'COMUPDM02-003
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnSync_Click()

        If mfSyncAllTelNoMaster() = True Then
            If mfIsTelNoIntegration() = False Then
                '不整合データ有
                psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "社員マスタに登録の無い電話番号が登録されています。\n電話番号マスタを確認して下さい。")
            End If
        End If

    End Sub

    ''' <summary>
    ''' 会社変更時（検索条件）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ddlSComp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSComp.SelectedIndexChanged
        Dim strVal As String = ddlSOffice.SelectedValue
        msSetddlOffice("Search")
        If ddlSOffice.Items.FindByValue(strVal) IsNot Nothing Then
            ddlSOffice.SelectedValue = strVal
        End If

    End Sub

    ''' <summary>
    ''' 会社変更時（編集エリア）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ddlComp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlComp.SelectedIndexChanged

        msSetddlOffice()

    End Sub

    ''' <summary>
    ''' 社員コード変更時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub txtEmpCd_TextChanged()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        objStack = New StackFrame

        If txtEmpCd.ppText.Trim <> String.Empty Then
            '入力チェック
            Page.Validate("key")
            If (Page.IsValid) Then
                '接続
                If clsDataConnect.pfOpen_Database(conDB) Then
                    Try
                        'ドロップダウン設定
                        msSetddlComp("Edit")
                        ddlOffice.Items.Clear()

                        cmdDB = New SqlCommand(DispCode & "_S2", conDB)

                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("emply_cd", SqlDbType.NVarChar, txtEmpCd.ppText.Trim))
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
                            arKey.Insert(0, txtEmpCd.ppText.Trim)

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
                                txtEmpCd.ppText = .Item("社員コード").ToString
                                txtLName.ppText = .Item("社員姓").ToString
                                txtFName.ppText = .Item("社員名").ToString
                                txtLNameK.ppText = .Item("社員姓カナ").ToString
                                txtFNameK.ppText = .Item("社員名カナ").ToString
                                txtSrtName.ppText = .Item("社員略称").ToString
                                If .Item("TEL").ToString <> String.Empty Then
                                    txtTel1.ppText = .Item("TEL").ToString.Split("-")(0)
                                    txtTel2.ppText = .Item("TEL").ToString.Split("-")(1)
                                    txtTel3.ppText = .Item("TEL").ToString.Split("-")(2)
                                End If
                                If .Item("FAX").ToString <> String.Empty Then
                                    txtFax1.ppText = .Item("FAX").ToString.Split("-")(0)
                                    txtFax2.ppText = .Item("FAX").ToString.Split("-")(1)
                                    txtFax3.ppText = .Item("FAX").ToString.Split("-")(2)
                                End If

                                If Request.QueryString.Get("page").ToString = .Item("画面区分").ToString Then
                                    ddlComp.SelectedValue = .Item("会社コード").ToString
                                    msSetddlOffice("")
                                    ddlOffice.SelectedValue = .Item("営業所コード").ToString
                                    ddlAuth.SelectedValue = .Item("権限区分").ToString
                                    strMode = "Select"
                                Else
                                    ddlComp.Items.Clear()
                                    ddlComp.Items.Add(.Item("会社"))
                                    ddlOffice.Items.Clear()
                                    ddlOffice.Items.Add(.Item("営業所"))
                                    ddlAuth.SelectedValue = .Item("権限区分").ToString
                                    If ViewState(MasterName) = "社員マスタ" Then
                                        lblACmp.Visible = True
                                        lblEmp.Visible = False
                                    Else
                                        lblACmp.Visible = False
                                        lblEmp.Visible = True
                                    End If
                                    strMode = "Enabled"
                                End If

                                'COMUPDM02-004
                                Dim strTmp As String
                                strTmp = .Item("CRS区分").ToString
                                If strTmp = String.Empty Then
                                    ddlCrsDvs.ppDropDownList.SelectedValue = "0"
                                Else
                                    ddlCrsDvs.ppSelectedValue = strTmp
                                End If

                                ddlCrsDvs.ppDropDownList.Items(0).Enabled = False   '空欄リストの削除
                                ddlCrsDvs_SelectedIndexChanged(Nothing, Nothing)    'CRS管理の活性制御

                                strTmp = .Item("範囲管理区分").ToString
                                If strTmp = String.Empty Then
                                    ddlVscDvs.ppDropDownList.SelectedValue = "9"
                                Else
                                    ddlVscDvs.ppSelectedValue = strTmp
                                End If

                                strTmp = .Item("メール送信区分").ToString
                                If strTmp = String.Empty Then
                                    ddlMailDvs.ppDropDownList.SelectedValue = "9"
                                Else
                                    ddlMailDvs.ppSelectedValue = strTmp
                                End If

                                'COMUPDM02-002
                                'If ddlMailDvs.ppSelectedValue = "9" AndAlso ddlVscDvs.ppSelectedValue = "9" Then
                                '    ddlCrsDvs.ppSelectedValue = "1"
                                'Else
                                '    ddlCrsDvs.ppSelectedValue = "0"
                                'End If
                                'strTmp = .Item("範囲管理区分").ToString
                                'If strTmp = String.Empty Then
                                '    ddlVscDvs.ppDropDownList.SelectedValue = "9"
                                'Else
                                '    ddlVscDvs.ppSelectedValue = strTmp
                                'End If
                                'strTmp = .Item("メール送信区分").ToString
                                'If strTmp = String.Empty Then
                                '    ddlMailDvs.ppDropDownList.SelectedValue = "9"
                                'Else
                                '    ddlMailDvs.ppSelectedValue = strTmp
                                'End If
                                'If ddlMailDvs.ppSelectedValue = "9" AndAlso ddlVscDvs.ppSelectedValue = "9" Then
                                '    ddlCrsDvs.ppSelectedValue = "1"
                                'Else
                                '    ddlCrsDvs.ppSelectedValue = "0"
                                'End If
                                'COMUPDM02-004 END
                                strTmp = .Item("携帯電話番号1").ToString
                                If strTmp <> String.Empty Then
                                    txtTelSp1_1.ppText = strTmp.Split("-")(0)
                                    txtTelSp1_2.ppText = strTmp.Split("-")(1)
                                    txtTelSp1_3.ppText = strTmp.Split("-")(2)
                                    hdnTelSp1.Value = strTmp
                                End If
                                strTmp = .Item("携帯電話番号2").ToString
                                If strTmp <> String.Empty Then
                                    txtTelSp2_1.ppText = strTmp.Split("-")(0)
                                    txtTelSp2_2.ppText = strTmp.Split("-")(1)
                                    txtTelSp2_3.ppText = strTmp.Split("-")(2)
                                    hdnTelSp2.Value = strTmp
                                End If
                                txtMail.ppText = .Item("メールアドレス").ToString
                                txtDomain.Text = .Item("メールドメイン").ToString
                                'COMUPDM02-002 END

                                '削除フラグによってボタンの文言を変更
                                If .Item("削除フラグ").ToString <> "1" Then
                                    Master.ppBtnDelete.Text = "削除"
                                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, ViewState(MasterName))       '削除
                                Else
                                    Master.ppBtnDelete.Text = "削除取消"
                                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, ViewState(MasterName) & "の削除取消")       '削除取消
                                End If

                                'メール送信対象判定
                                If .Item("責任者連番").ToString <> "0" AndAlso .Item("削除フラグ").ToString = "0" Then
                                    lblMailAdmin.ForeColor = Drawing.Color.Red
                                    lblMailAdmin.Text = "責任者" & .Item("責任者連番")
                                Else
                                    lblMailAdmin.Text = String.Empty
                                End If

                            End With

                        Else
                            ddlCrsDvs.ppSelectedValue = "1"                     '1:対象外
                            ddlCrsDvs.ppDropDownList.Items(0).Enabled = False   '空欄リストの削除
                            ddlCrsDvs_SelectedIndexChanged(Nothing, Nothing)    'CRS管理の活性制御
                            strMode = "Insert"
                        End If

                        If Master.ppBtnDelete.Text = "削除" Then
                            Master.ppBtnDmy.Visible = True
                            Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + txtLName.ppTextBox.ClientID + ");"
                            SetFocus(Master.ppBtnDmy.ClientID)
                        Else
                            SetFocus(Master.ppBtnClear.ClientID)
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
            strMode = "Default"
            SetFocus(Master.ppBtnClear.ClientID)
        End If

    End Sub

    ''' <summary>
    ''' CRS管理変更
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlCrsDvs_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim blnEnable As Boolean
        Select Case ddlCrsDvs.ppSelectedValue
            Case "0" '対象
                blnEnable = True
                SetFocus(ddlVscDvs.ppDropDownList.ClientID)
            Case "1" '対象外
                blnEnable = False
                txtMail.ppText = String.Empty
                txtDomain.Text = String.Empty
                SetFocus(txtLName.ppTextBox.ClientID)
        End Select

        ddlVscDvs.ppEnabled = blnEnable
        ddlMailDvs.ppEnabled = blnEnable
        txtMail.ppEnabled = blnEnable
        txtDomain.Enabled = blnEnable

        msSetCrsDropDownList(False, blnEnable)
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
                arKey.Insert(0, CType(rowData.FindControl("社員コード"), TextBox).Text)

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
                txtEmpCd.ppText = CType(rowData.FindControl("社員コード"), TextBox).Text
                txtLName.ppText = CType(rowData.FindControl("社員姓"), TextBox).Text
                txtFName.ppText = CType(rowData.FindControl("社員名"), TextBox).Text
                txtLNameK.ppText = CType(rowData.FindControl("社員姓カナ"), TextBox).Text
                txtFNameK.ppText = CType(rowData.FindControl("社員名カナ"), TextBox).Text
                txtSrtName.ppText = CType(rowData.FindControl("社員略称"), TextBox).Text
                If CType(rowData.FindControl("TEL"), TextBox).Text.Split("-")(0) <> String.Empty Then
                    txtTel1.ppText = CType(rowData.FindControl("TEL"), TextBox).Text.Split("-")(0)
                    txtTel2.ppText = CType(rowData.FindControl("TEL"), TextBox).Text.Split("-")(1)
                    txtTel3.ppText = CType(rowData.FindControl("TEL"), TextBox).Text.Split("-")(2)
                End If
                If CType(rowData.FindControl("FAX"), TextBox).Text.Split("-")(0) <> String.Empty Then
                    txtFax1.ppText = CType(rowData.FindControl("FAX"), TextBox).Text.Split("-")(0)
                    txtFax2.ppText = CType(rowData.FindControl("FAX"), TextBox).Text.Split("-")(1)
                    txtFax3.ppText = CType(rowData.FindControl("FAX"), TextBox).Text.Split("-")(2)
                End If
                ddlComp.SelectedValue = CType(rowData.FindControl("会社コード"), TextBox).Text
                msSetddlOffice("")
                ddlOffice.SelectedValue = CType(rowData.FindControl("営業所コード"), TextBox).Text
                ddlAuth.SelectedValue = CType(rowData.FindControl("権限区分コード"), TextBox).Text

                'COMUPDM02-002
                Dim strTmp As String

                strTmp = CType(rowData.FindControl("CRS区分"), TextBox).Text
                If strTmp = String.Empty Then
                    ddlCrsDvs.ppDropDownList.SelectedValue = "0"
                Else
                    ddlCrsDvs.ppSelectedValue = strTmp
                End If

                ddlCrsDvs.ppDropDownList.Items(0).Enabled = False   '空欄リストの削除
                ddlCrsDvs_SelectedIndexChanged(sender, e)           'CRS管理の活性制御

                strTmp = CType(rowData.FindControl("範囲管理区分コード"), TextBox).Text
                If strTmp = String.Empty Then
                    ddlVscDvs.ppSelectedValue = "9"
                Else
                    ddlVscDvs.ppSelectedValue = strTmp
                End If

                strTmp = CType(rowData.FindControl("メール送信区分コード"), TextBox).Text
                If strTmp = String.Empty Then
                    ddlMailDvs.ppSelectedValue = "9"
                Else
                    ddlMailDvs.ppSelectedValue = strTmp
                End If

                'If ddlMailDvs.ppSelectedValue = "9" AndAlso ddlVscDvs.ppSelectedValue = "9" Then
                '    ddlCrsDvs.ppSelectedValue = "1"
                'Else
                '    ddlCrsDvs.ppSelectedValue = "0"
                'End If

                'strTmp = CType(rowData.FindControl("範囲管理区分コード"), TextBox).Text
                'If strTmp = String.Empty Then
                '    ddlVscDvs.ppSelectedValue = "9"
                'Else
                '    ddlVscDvs.ppSelectedValue = strTmp
                'End If

                'strTmp = CType(rowData.FindControl("メール送信区分コード"), TextBox).Text
                'If strTmp = String.Empty Then
                '    ddlMailDvs.ppSelectedValue = "9"
                'Else
                '    ddlMailDvs.ppSelectedValue = strTmp
                'End If
                'COMUPDM02-004 END

                strTmp = CType(rowData.FindControl("携帯電話番号1"), TextBox).Text
                If strTmp <> String.Empty Then
                    txtTelSp1_1.ppText = strTmp.Split("-")(0)
                    txtTelSp1_2.ppText = strTmp.Split("-")(1)
                    txtTelSp1_3.ppText = strTmp.Split("-")(2)
                    hdnTelSp1.Value = strTmp
                End If
                strTmp = CType(rowData.FindControl("携帯電話番号2"), TextBox).Text
                If strTmp <> String.Empty Then
                    txtTelSp2_1.ppText = strTmp.Split("-")(0)
                    txtTelSp2_2.ppText = strTmp.Split("-")(1)
                    txtTelSp2_3.ppText = strTmp.Split("-")(2)
                    hdnTelSp2.Value = strTmp
                End If
                txtMail.ppText = CType(rowData.FindControl("メールアドレス"), TextBox).Text
                txtDomain.Text = CType(rowData.FindControl("メールドメイン"), TextBox).Text
                'COMUPDM02-002 END

                '削除フラグによってボタンの文言を変更
                If CType(rowData.FindControl("削除"), TextBox).Text.Trim = "0" Then
                    Master.ppBtnDelete.Text = "削除"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, ViewState(MasterName))       '削除
                Else
                    Master.ppBtnDelete.Text = "削除取消"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, ViewState(MasterName) & "の削除取消")       '削除取消
                End If

                'メール送信対象判定
                If CType(rowData.FindControl("責任者連番"), TextBox).Text.Trim <> "0" AndAlso CType(rowData.FindControl("削除"), TextBox).Text.Trim = "0" Then
                    lblMailAdmin.ForeColor = Drawing.Color.Red
                    lblMailAdmin.Text = "責任者" & CType(rowData.FindControl("責任者連番"), TextBox).Text.Trim
                Else
                    lblMailAdmin.Text = String.Empty
                End If

                'フォーカス設定
                If Master.ppBtnDelete.Text = "削除" Then
                    SetFocus(txtLName.ppTextBox.ClientID)
                Else
                    SetFocus(Master.ppBtnClear.ClientID)
                End If

                strMode = "Select"
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

    Private Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        Try
            For Each rowData As GridViewRow In grvList.Rows
                '削除フラグ１の時該当行赤字化
                If CType(rowData.FindControl("削除"), TextBox).Text.Trim = "1" Then
                    For i As Integer = 0 To rowData.Cells.Count - 1
                        If TryCast((rowData.Cells(i).Controls(0)), TextBox) IsNot Nothing Then
                            CType(rowData.Cells(i).Controls(0), TextBox).ForeColor = Drawing.Color.Red
                        End If
                    Next
                End If
                '送信対象は●
                If CType(rowData.FindControl("メール送信区分コード"), TextBox).Text.Trim = "1" Then
                    CType(rowData.FindControl("権限区分"), TextBox).Text = "●" & CType(rowData.FindControl("権限区分"), TextBox).Text
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
    ''' 編集エリア活性制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_Enabled(ByVal bool As Boolean)

        txtEmpCd.ppEnabled = bool
        txtLName.ppEnabled = bool
        txtFName.ppEnabled = bool
        txtSrtName.ppEnabled = bool
        txtLNameK.ppEnabled = bool
        txtFNameK.ppEnabled = bool
        txtTel1.ppEnabled = bool
        txtTel2.ppEnabled = bool
        txtTel3.ppEnabled = bool
        txtFax1.ppEnabled = bool
        txtFax2.ppEnabled = bool
        txtFax3.ppEnabled = bool
        ddlComp.Enabled = bool
        ddlOffice.Enabled = bool
        ddlAuth.Enabled = bool
        'COMUPDM02-001 

        txtTelSp1_1.ppEnabled = bool
        txtTelSp1_2.ppEnabled = bool
        txtTelSp1_3.ppEnabled = bool
        txtTelSp2_1.ppEnabled = bool
        txtTelSp2_2.ppEnabled = bool
        txtTelSp2_3.ppEnabled = bool

        'COMUPDM02-001 END
        ddlCrsDvs.ppEnabled = bool
        'CRS管理項目は非活性化のみ、活性化処理はCRS管理のドロップダウンリストイベントで制御
        If bool = False Then
            ddlVscDvs.ppEnabled = bool
            ddlMailDvs.ppEnabled = bool
            txtMail.ppEnabled = bool
            txtDomain.Enabled = bool
        End If

    End Sub

    ''' <summary>
    ''' 検索処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim dispFlg As String = String.Empty
        Dim dltFlg As String = String.Empty
        Dim authCls As String = String.Empty
        objStack = New StackFrame

        If Me.IsPostBack Then
            dispFlg = "0"
        Else
            dispFlg = "1"
        End If

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                For zz = 0 To chkListAuth.Items.Count - 1
                    If chkListAuth.Items(zz).Selected = True Then
                        If authCls = String.Empty Then
                            authCls = chkListAuth.Items(zz).Value
                        Else
                            authCls &= "," & chkListAuth.Items(zz).Value
                        End If
                    End If
                Next

                cmdDB = New SqlCommand(DispCode & "_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("emply_cd_from", SqlDbType.NVarChar, tftSEmpCd.ppFromText.Trim))
                    .Add(pfSet_Param("emply_cd_to", SqlDbType.NVarChar, tftSEmpCd.ppToText.Trim))
                    .Add(pfSet_Param("comp_cd", SqlDbType.NVarChar, ddlSComp.SelectedValue))
                    .Add(pfSet_Param("office_cd", SqlDbType.NVarChar, ddlSOffice.SelectedValue))
                    .Add(pfSet_Param("name", SqlDbType.NVarChar, txtSName.ppText.Trim))
                    .Add(pfSet_Param("name_kana", SqlDbType.NVarChar, txtSNameKana.ppText.Trim))
                    .Add(pfSet_Param("auth_cls", SqlDbType.NVarChar, authCls))
                    .Add(pfSet_Param("mail_cls", SqlDbType.NVarChar, ddlsMailDvs.ppSelectedValue))  'COMUPDM02-002
                    .Add(pfSet_Param("delete_flg", SqlDbType.NVarChar, ddldel.ppSelectedValue))
                    .Add(pfSet_Param("disp_cls", SqlDbType.NVarChar, Request.QueryString.Get("page").ToString))
                    .Add(pfSet_Param("prog_id", SqlDbType.NVarChar, DispCode))
                    .Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, dispFlg))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    Master.ppCount = "0"
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                ElseIf CType(dstOrders.Tables(0).Rows(0).Item("総件数").ToString, Integer) > dstOrders.Tables(0).Rows.Count Then
                    '上限オーバー
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
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, ViewState(MasterName))
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
    ''' <remarks></remarks>
    Private Function msEditData(ByVal ipstrMode As String) As Boolean

        msEditData = False

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
                        procCls = "0"
                        strStored = DispCode & "_D1"
                    Case "削除取消"
                        MesCode = "00001"
                        procCls = "1"
                        strStored = DispCode & "_D1"
                End Select
        End Select

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(strStored, conDB)
                Select Case ipstrMode
                    Case "INSERT", "UPDATE"
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("emply_cd", SqlDbType.NVarChar, txtEmpCd.ppText.Trim))             '社員コード
                            .Add(pfSet_Param("l_name", SqlDbType.NVarChar, txtLName.ppText.Trim))               '社員姓
                            .Add(pfSet_Param("f_name", SqlDbType.NVarChar, txtFName.ppText.Trim))               '社員名
                            .Add(pfSet_Param("l_name_k", SqlDbType.NVarChar, txtLNameK.ppText.Trim))            '社員姓カナ
                            .Add(pfSet_Param("f_name_k", SqlDbType.NVarChar, txtFNameK.ppText.Trim))            '社員名カナ
                            .Add(pfSet_Param("s_name", SqlDbType.NVarChar, txtSrtName.ppText.Trim))             '社員略称
                            .Add(pfSet_Param("comp_cd", SqlDbType.NVarChar, ddlComp.SelectedValue))             '会社コード
                            .Add(pfSet_Param("office_cd", SqlDbType.NVarChar, ddlOffice.SelectedValue))         '営業所コード
                            .Add(pfSet_Param("tel", SqlDbType.NVarChar, mfConnectHyphen(txtTel1.ppTextBox)))                    'TEL
                            .Add(pfSet_Param("fax", SqlDbType.NVarChar, mfConnectHyphen(txtFax1.ppTextBox)))                    'FAX
                            If ViewState(MasterName) = "他社マスタ" Then
                                .Add(pfSet_Param("auth_cls", SqlDbType.NVarChar, "1"))                          '権限区分
                            Else
                                .Add(pfSet_Param("auth_cls", SqlDbType.NVarChar, ddlAuth.SelectedValue))        '権限区分
                            End If
                            .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))                          '処理区分
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                 'ユーザーＩＤ
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                   '戻り値
                            'COMUPDM02-002
                            .Add(pfSet_Param("@mail_cls", SqlDbType.NVarChar, ddlMailDvs.ppSelectedValue))       'メール送信区分
                            .Add(pfSet_Param("@mail_auth", SqlDbType.NVarChar, ddlVscDvs.ppSelectedValue))       '範囲管理区分
                            .Add(pfSet_Param("@mail_adr", SqlDbType.NVarChar, txtMail.ppText.Trim))              'メールアドレス
                            .Add(pfSet_Param("@mail_dmin", SqlDbType.NVarChar, txtDomain.Text.Trim))             'ドメイン
                            .Add(pfSet_Param("@ph1_no1", SqlDbType.NVarChar, txtTelSp1_1.ppText.Trim))           '携帯電話番号1
                            .Add(pfSet_Param("@ph1_no2", SqlDbType.NVarChar, txtTelSp1_2.ppText.Trim))           '
                            .Add(pfSet_Param("@ph1_no3", SqlDbType.NVarChar, txtTelSp1_3.ppText.Trim))           '
                            .Add(pfSet_Param("@ph2_no1", SqlDbType.NVarChar, txtTelSp2_1.ppText.Trim))           '携帯電話番号2
                            .Add(pfSet_Param("@ph2_no2", SqlDbType.NVarChar, txtTelSp2_2.ppText.Trim))           '
                            .Add(pfSet_Param("@ph2_no3", SqlDbType.NVarChar, txtTelSp2_3.ppText.Trim))           '
                            'COMUPDM02-002 END
                            .Add(pfSet_Param("@crs_dvs", SqlDbType.NVarChar, ddlCrsDvs.ppSelectedValue.ToString)) 'CRS区分  'COMUPDM02-004
                        End With
                    Case "DELETE"
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("emply_cd", SqlDbType.NVarChar, txtEmpCd.ppText.Trim))              '社員コード
                            .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))                           '処理区分
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                  'ユーザーＩＤ
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                    '戻り値
                        End With
                End Select

                'データ登録/更新/削除
                Using conTrn = conDB.BeginTransaction
                    cmdDB.Transaction = conTrn

                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        conTrn.Rollback()
                        psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, ViewState(MasterName))
                        Exit Function
                    End If

                    'コミット
                    conTrn.Commit()
                End Using

                '追加、更新の場合は対象のレコードのみグリッドに表示
                'If ipstrMode = "INSERT" OrElse ipstrMode = "UPDATE" Then
                If ipstrMode = "INSERT" OrElse ipstrMode = "UPDATE" OrElse Master.ppBtnDelete.Text = "削除取消" Then

                    'COMUPDM02-002
                    cmdDB = New SqlCommand(DispCode & "_S1", conDB)
                    With cmdDB.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("emply_cd_from", SqlDbType.NVarChar, txtEmpCd.ppText))
                        .Add(pfSet_Param("emply_cd_to", SqlDbType.NVarChar, String.Empty))
                        .Add(pfSet_Param("comp_cd", SqlDbType.NVarChar, String.Empty))
                        .Add(pfSet_Param("office_cd", SqlDbType.NVarChar, String.Empty))
                        .Add(pfSet_Param("name", SqlDbType.NVarChar, String.Empty))
                        .Add(pfSet_Param("name_kana", SqlDbType.NVarChar, String.Empty))
                        .Add(pfSet_Param("auth_cls", SqlDbType.NVarChar, String.Empty))
                        .Add(pfSet_Param("mail_cls", SqlDbType.NVarChar, String.Empty))
                        .Add(pfSet_Param("delete_flg", SqlDbType.NVarChar, String.Empty))
                        .Add(pfSet_Param("disp_cls", SqlDbType.NVarChar, Request.QueryString.Get("page").ToString))
                        .Add(pfSet_Param("prog_id", SqlDbType.NVarChar, DispCode))
                        .Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, "0"))
                    End With

                    'リストデータ取得
                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                    dttGrid = dstOrders.Tables(0)

                    'dttGrid.Columns.Add("社員コード")
                    'dttGrid.Columns.Add("社員姓")
                    'dttGrid.Columns.Add("社員名")
                    'dttGrid.Columns.Add("社員姓カナ")
                    'dttGrid.Columns.Add("社員名カナ")
                    'dttGrid.Columns.Add("社員略称")
                    'dttGrid.Columns.Add("TEL")
                    'dttGrid.Columns.Add("FAX")
                    'dttGrid.Columns.Add("会社コード")
                    'dttGrid.Columns.Add("会社")
                    'dttGrid.Columns.Add("営業所コード")
                    'dttGrid.Columns.Add("営業所")
                    'dttGrid.Columns.Add("権限区分")
                    'dttGrid.Columns.Add("権限区分コード")
                    'dttGrid.Columns.Add("削除")
                    'drData = dttGrid.NewRow()
                    'drData.Item("社員コード") = txtEmpCd.ppText.Trim
                    'drData.Item("社員姓") = txtLName.ppText.Trim
                    'drData.Item("社員名") = txtFName.ppText.Trim
                    'drData.Item("社員姓カナ") = txtLNameK.ppText.Trim
                    'drData.Item("社員名カナ") = txtFNameK.ppText.Trim
                    'drData.Item("社員略称") = txtSrtName.ppText.Trim
                    'drData.Item("TEL") = mfConnectHyphen(txtTel1.ppTextBox)
                    'drData.Item("FAX") = mfConnectHyphen(txtFax1.ppTextBox)
                    'drData.Item("会社コード") = ddlComp.SelectedValue
                    'drData.Item("会社") = ddlComp.SelectedItem.ToString.Split(":")(1)
                    'drData.Item("営業所コード") = ddlOffice.SelectedValue
                    'drData.Item("営業所") = ddlOffice.SelectedItem.ToString.Split(":")(1)
                    'drData.Item("権限区分") = ddlAuth.SelectedItem.ToString
                    'drData.Item("権限区分コード") = ddlAuth.SelectedValue
                    'drData.Item("削除") = String.Empty
                    'dttGrid.Rows.Add(drData)
                    'COMUPDM02-002 END
                Else
                    dttGrid = New DataTable
                End If

                'データをセット
                'dttGrid.AcceptChanges() 'COMUPDM02-002
                grvList.DataSource = dttGrid
                grvList.DataBind()

                Master.ppCount = dttGrid.Rows.Count

                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, ViewState(MasterName))

                Return True

            Catch ex As Exception
                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, ViewState(MasterName))
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

    End Function

    ''' <summary>
    ''' ドロップダウンリスト設定（会社）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlComp(Optional ByVal mode As String = "All")

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(DispCode & "_S3", objCn)

                'パラメータ設定
                objCmd.Parameters.Add(pfSet_Param("disp_cls", SqlDbType.NVarChar, Request.QueryString.Get("page").ToString))         '画面区分（1:社員マスタ 2:他社マスタ）

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                If mode = "All" OrElse mode = "Search" Then
                    '検索条件
                    Me.ddlSComp.Items.Clear()
                    Me.ddlSComp.DataSource = objDs.Tables(0)
                    Me.ddlSComp.DataTextField = "会社名"
                    Me.ddlSComp.DataValueField = "連番"
                    Me.ddlSComp.DataBind()
                    Me.ddlSComp.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                End If

                If mode = "All" OrElse mode = "Edit" Then
                    '検索結果（明細）
                    Me.ddlComp.Items.Clear()
                    Me.ddlComp.DataSource = objDs.Tables(0)
                    Me.ddlComp.DataTextField = "会社名"
                    Me.ddlComp.DataValueField = "連番"
                    Me.ddlComp.DataBind()
                    Me.ddlComp.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                End If

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "業者マスタ取得")
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
    ''' ドロップダウンリスト設定（営業所）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlOffice(Optional ByVal ipstrArea As String = "")

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(DispCode & "_S4", objCn)

                With objCmd.Parameters
                    'パラメータ設定
                    If ipstrArea = "Search" Then
                        .Add(pfSet_Param("comp_cd", SqlDbType.NVarChar, ddlSComp.SelectedValue))             '会社コード
                    Else
                        .Add(pfSet_Param("comp_cd", SqlDbType.NVarChar, ddlComp.SelectedValue))              '会社コード
                    End If
                    .Add(pfSet_Param("disp_cls", SqlDbType.NVarChar, Request.QueryString.Get("page").ToString))         '画面区分（1:社員マスタ 2:他社マスタ）
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                If ipstrArea = "Search" Then
                    '検索条件
                    Me.ddlSOffice.Items.Clear()
                    Me.ddlSOffice.DataSource = objDs.Tables(0)
                    Me.ddlSOffice.DataTextField = "営業所名"
                    Me.ddlSOffice.DataValueField = "営業所コード"
                    Me.ddlSOffice.DataBind()
                    Me.ddlSOffice.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                Else
                    '編集エリア
                    Me.ddlOffice.Items.Clear()
                    Me.ddlOffice.DataSource = objDs.Tables(0)
                    Me.ddlOffice.DataTextField = "営業所名"
                    Me.ddlOffice.DataValueField = "営業所コード"
                    Me.ddlOffice.DataBind()
                    Me.ddlOffice.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                End If

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "営業所一覧取得")
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
    ''' ドロップダウンリスト/チェックボックスリスト設定（権限区分）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetchkListAuth()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(DispCode & "_S5", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                Me.ddlAuth.Items.Clear()
                Me.ddlAuth.DataSource = objDs.Tables(0)
                Me.ddlAuth.DataTextField = "権限区分"
                Me.ddlAuth.DataValueField = "区分コード"
                Me.ddlAuth.DataBind()
                Me.ddlAuth.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                'チェックボックスリスト設定
                Me.chkListAuth.Items.Clear()
                Me.chkListAuth.DataSource = objDs.Tables(0)
                Me.chkListAuth.DataTextField = "権限区分"
                Me.chkListAuth.DataValueField = "区分コード"
                Me.chkListAuth.DataBind()

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "権限区分取得")
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
    ''' 入力チェック（会社）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub cuvComp_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvComp.ServerValidate
        Dim strErrNo As String
        Dim dtrMes As DataRow
        'エラーチェック
        strErrNo = pfCheck_ListErr(args.Value, True)

        If strErrNo <> String.Empty Then
            'エラー
            dtrMes = pfGet_ValMes(strErrNo, "会社")
            cuvComp.Text = dtrMes.Item(P_VALMES_SMES)
            cuvComp.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            args.IsValid = False
            Exit Sub
        End If

    End Sub

    ''' <summary>
    ''' 入力チェック（営業所）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub cuvOffice_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvOffice.ServerValidate
        Dim strErrNo As String
        Dim dtrMes As DataRow
        'エラーチェック
        strErrNo = pfCheck_ListErr(args.Value, True)

        If strErrNo <> String.Empty Then
            'エラー
            dtrMes = pfGet_ValMes(strErrNo, "営業所")
            cuvOffice.Text = dtrMes.Item(P_VALMES_SMES)
            cuvOffice.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            args.IsValid = False
            Exit Sub
        End If

    End Sub

    ''' <summary>
    ''' 入力チェック（権限区分）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub cuvAuth_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvAuth.ServerValidate
        Dim strErrNo As String
        Dim dtrMes As DataRow
        'エラーチェック
        strErrNo = pfCheck_ListErr(args.Value, True)

        If strErrNo <> String.Empty Then
            'エラー
            dtrMes = pfGet_ValMes(strErrNo, "権限区分")
            cuvAuth.Text = dtrMes.Item(P_VALMES_SMES)
            cuvAuth.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            args.IsValid = False
            Exit Sub
        End If

    End Sub

    ''' <summary>
    ''' 入力チェック（電話番号系）'COMUPDM02-002
    ''' </summary>
    ''' <remarks></remarks>
    Private Function checkTELNO() As Boolean

        checkTELNO = True

        Dim uctbxTelNo(3) As ClsCMTextBox
        Dim strTelNo(3)() As String

        uctbxTelNo = {txtTel1, txtFax1, txtTelSp1_1, txtTelSp2_1}
        strTelNo(0) = {txtTel1.ppText.Trim, txtTel2.ppText.Trim, txtTel3.ppText.Trim}
        strTelNo(1) = {txtFax1.ppText.Trim, txtFax2.ppText.Trim, txtFax3.ppText.Trim}
        strTelNo(2) = {txtTelSp1_1.ppText.Trim, txtTelSp1_2.ppText.Trim, txtTelSp1_3.ppText.Trim}
        strTelNo(3) = {txtTelSp2_1.ppText.Trim, txtTelSp2_2.ppText.Trim, txtTelSp2_3.ppText.Trim}

        For i As Integer = 0 To 3

            If strTelNo(i)(0) & strTelNo(i)(1) & strTelNo(i)(2) = String.Empty Then
                '全て未入力 入力値検証終了
                Continue For
            Else
                If strTelNo(i)(0) = String.Empty OrElse strTelNo(i)(1) = String.Empty OrElse strTelNo(i)(2) = String.Empty Then
                    'いずれかが未入力
                    uctbxTelNo(i).psSet_ErrorNo("4014", uctbxTelNo(i).ppName)
                    'X
                    checkTELNO = False
                End If
            End If

            '携帯番号2のみの入力禁止
            If i = 3 Then
                '携帯番号1未入力
                If strTelNo(i - 1)(0) = String.Empty OrElse strTelNo(i - 1)(1) = String.Empty OrElse strTelNo(i - 1)(2) = String.Empty Then
                    '携帯番号2入力
                    If strTelNo(i)(0) <> String.Empty OrElse strTelNo(i)(1) <> String.Empty OrElse strTelNo(i)(2) <> String.Empty Then
                        'いずれかが未入力
                        uctbxTelNo(i - 1).psSet_ErrorNo("2011", "携帯電話番号2の登録", "携帯電話番号1")
                        'X
                        checkTELNO = False
                    End If
                Else
                    If strTelNo(i - 1)(0) & strTelNo(i - 1)(1) & strTelNo(i - 1)(2) = strTelNo(i)(0) & strTelNo(i)(1) & strTelNo(i)(2) Then
                        '携帯１と携帯２が同番号
                        uctbxTelNo(i - 1).psSet_ErrorNo("4001", "携帯電話番号の登録", "1と2それぞれ別の番号")
                        'X
                        checkTELNO = False
                    End If
                End If
            End If
        Next

    End Function

    ''' <summary>
    ''' 入力チェック（メールアドレス）'COMUPDM02-002
    ''' </summary>
    ''' <remarks></remarks>
    Private Function checkMailAddress() As Boolean

        Dim strMail As String = txtMail.ppText.Trim
        Dim strDomain As String = txtDomain.Text.Trim

        If strMail = String.Empty AndAlso strDomain = String.Empty Then
            If ddlMailDvs.ppSelectedValue = "1" Then
                txtMail.psSet_ErrorNo("5006", txtMail.ppName)  '{0} が入力されていません。
                txtMail.psSet_ErrorNo("5006", "ドメイン")  '{0} が入力されていません。
                Return False
            Else
                Return True
            End If
        End If

        If strMail = String.Empty Then
            txtMail.psSet_ErrorNo("5006", txtMail.ppName)  '{0} が入力されていません。
            Return False
        End If

        If strDomain = String.Empty Then
            txtMail.psSet_ErrorNo("5006", "ドメイン")  '{0} が入力されていません。
            Return False
        End If

        If Regex.IsMatch(strDomain, "^[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$") = False Then
            txtMail.psSet_ErrorNo("4001", txtMail.ppName, "正しい形式")
            Return False
        End If

        checkMailAddress = True

    End Function


    'COMUPDM02-001 
    Private Function mfConnectHyphen(ByVal _txtbox As TextBox) As String
        Select Case _txtbox.ClientID
            Case txtTel1.ppTextBox.ClientID
                If txtTel1.ppText.Trim = String.Empty Then
                    Return String.Empty 'COMUPDM02-002
                Else
                    Return txtTel1.ppText.Trim & "-" & txtTel2.ppText.Trim & "-" & txtTel3.ppText.Trim
                End If
            Case txtFax1.ppTextBox.ClientID
                If txtFax1.ppText.Trim = String.Empty Then
                    Return String.Empty 'COMUPDM02-002
                Else
                    Return txtFax1.ppText.Trim & "-" & txtFax2.ppText.Trim & "-" & txtFax3.ppText.Trim
                End If
            Case txtTelSp1_1.ppTextBox.ClientID
                If txtTelSp1_1.ppText.Trim = String.Empty Then
                    Return String.Empty 'COMUPDM02-002
                Else
                    Return txtTelSp1_1.ppText.Trim & "-" & txtTelSp1_2.ppText.Trim & "-" & txtTelSp1_3.ppText.Trim
                End If
            Case txtTelSp2_1.ppTextBox.ClientID
                If txtTelSp2_1.ppText.Trim = String.Empty Then
                    Return String.Empty 'COMUPDM02-002
                Else
                    Return txtTelSp2_1.ppText.Trim & "-" & txtTelSp2_2.ppText.Trim & "-" & txtTelSp2_3.ppText.Trim
                End If
            Case Else
                Return String.Empty
        End Select

    End Function

    'COMUPDM02-001 END

    'COMUPDM02-003 
    Private Function mfSyncAllTelNoMaster() As Boolean
        Const strStored As String = "ZCMPINS003"
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim trans As SqlClient.SqlTransaction
        objStack = New StackFrame
        mfSyncAllTelNoMaster = False

        'トランザクションの設定
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                trans = conDB.BeginTransaction
                cmdDB = New SqlCommand(strStored, conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.Transaction = trans
                With cmdDB.Parameters
                    .Clear()
                    .Add(pfSet_Param("tblid", SqlDbType.NVarChar, "M02_EMPLOYEE"))
                    .Add(pfSet_Param("usrid", SqlDbType.NVarChar, User.Identity.Name))
                End With
                Try
                    cmdDB.ExecuteNonQuery()
                Catch ex As Exception
                    trans.Rollback()
                    Throw
                End Try
                trans.Commit()
                mfSyncAllTelNoMaster = True
                '正常終了メッセージ
                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "電話番号の同期")
            Catch ex As Exception
                mfSyncAllTelNoMaster = False
                'エラーメッセージ表示
                psMesBox(Me, "00000", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "電話番号の同期に失敗しました。\nシステム管理者に問い合わせてください。")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                  objStack.GetMethod.Name, "", ex.ToString, "Catch")
            End Try
        End If
    End Function
    Private Function mfSyncTelNoMaster() As Boolean
        Const strStored As String = "ZCMPINS004"
        Dim blnSync As Boolean = False
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim trans As SqlClient.SqlTransaction
        objStack = New StackFrame
        mfSyncTelNoMaster = False

        Dim strtelsp1 As String = txtTelSp1_1.ppText.Trim + txtTelSp1_2.ppText.Trim + txtTelSp1_3.ppText.Trim
        Dim strtelsp2 As String = txtTelSp2_1.ppText.Trim + txtTelSp2_2.ppText.Trim + txtTelSp2_3.ppText.Trim

        'トランザクションの設定

        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                trans = conDB.BeginTransaction
                cmdDB = New SqlCommand(strStored, conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.Transaction = trans
                If strtelsp1 <> String.Empty Then
                    With cmdDB.Parameters
                        .Clear()
                        .Add(pfSet_Param("telno", SqlDbType.NVarChar, strtelsp1))
                        .Add(pfSet_Param("code", SqlDbType.NVarChar, txtEmpCd.ppText.Trim))
                        .Add(pfSet_Param("decision", SqlDbType.NVarChar, "1"))
                        .Add(pfSet_Param("usrid", SqlDbType.NVarChar, User.Identity.Name))
                    End With
                    Try
                        cmdDB.ExecuteNonQuery()
                    Catch ex As Exception
                        trans.Rollback()
                        Throw ex
                    End Try
                    blnSync = True
                End If
                If strtelsp2 <> String.Empty Then
                    With cmdDB.Parameters
                        .Clear()
                        .Add(pfSet_Param("telno", SqlDbType.NVarChar, strtelsp2))
                        .Add(pfSet_Param("code", SqlDbType.NVarChar, txtEmpCd.ppText.Trim))
                        .Add(pfSet_Param("decision", SqlDbType.NVarChar, "1"))
                        .Add(pfSet_Param("usrid", SqlDbType.NVarChar, User.Identity.Name))
                    End With
                    Try
                        cmdDB.ExecuteNonQuery()
                    Catch ex As Exception
                        trans.Rollback()
                        Throw ex
                    End Try
                    blnSync = True
                End If
                trans.Commit()
                If blnSync Then
                    '終了メッセージ
                    msSyncTelCompleteMsg(conDB)
                End If
            Catch ex As Exception
                'エラーメッセージ表示
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "電話番号同期")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            End Try
        End If
    End Function
    Private Function mfDeleteTelNoMaster() As Boolean
        Const strStored As String = "ZCMPDEL004"
        Dim blnSync As Boolean = False
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim trans As SqlClient.SqlTransaction
        mfDeleteTelNoMaster = False

        Dim strtelsp1 As String = txtTelSp1_1.ppText.Trim + txtTelSp1_2.ppText.Trim + txtTelSp1_3.ppText.Trim
        Dim strtelsp2 As String = txtTelSp2_1.ppText.Trim + txtTelSp2_2.ppText.Trim + txtTelSp2_3.ppText.Trim

        'トランザクションの設定

        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                trans = conDB.BeginTransaction
                cmdDB = New SqlCommand(strStored, conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.Transaction = trans
                If strtelsp1 <> String.Empty Then
                    With cmdDB.Parameters
                        .Clear()
                        .Add(pfSet_Param("code", SqlDbType.NVarChar, txtEmpCd.ppText.Trim))
                    End With
                    Try
                        cmdDB.ExecuteNonQuery()
                    Catch ex As Exception
                        trans.Rollback()
                        Throw ex
                    End Try
                    blnSync = True
                End If
                If strtelsp2 <> String.Empty Then
                    With cmdDB.Parameters
                        .Clear()
                        .Add(pfSet_Param("code", SqlDbType.NVarChar, txtEmpCd.ppText.Trim))
                    End With
                    Try
                        cmdDB.ExecuteNonQuery()
                    Catch ex As Exception
                        trans.Rollback()
                        Throw ex
                    End Try
                    blnSync = True
                End If
                trans.Commit()
                If blnSync Then
                    '正常終了メッセージ
                    psMesBox(Me, "00002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "電話番号マスタ")
                End If
            Catch ex As Exception
                'エラーメッセージ表示
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "電話番号同期")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            End Try
        End If
    End Function
    Private Sub msSyncTelCompleteMsg(ByVal _con As SqlConnection)
        Dim ds As New DataSet
        Dim cmd As New SqlCommand("ZMSTSEL008", _con)
        cmd.CommandType = CommandType.StoredProcedure
        Dim strtelsp1 As String = txtTelSp1_1.ppText.Trim + txtTelSp1_2.ppText.Trim + txtTelSp1_3.ppText.Trim
        Dim strtelsp2 As String = txtTelSp2_1.ppText.Trim + txtTelSp2_2.ppText.Trim + txtTelSp2_3.ppText.Trim

        With cmd.Parameters
            .Add(pfSet_Param("tel1", SqlDbType.NVarChar, strtelsp1))
            If strtelsp2 <> String.Empty Then
                .Add(pfSet_Param("tel2", SqlDbType.NVarChar, strtelsp2))
            End If
            .Add(pfSet_Param("cod", SqlDbType.NVarChar, txtEmpCd.ppText.Trim))
            .Add(pfSet_Param("dvs", SqlDbType.NVarChar, "1"))
        End With

        ds = clsDataConnect.pfGet_DataSet(cmd)

        If ds.Tables(0).Rows.Count = 0 Then
            '正常終了メッセージ
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "電話番号の同期")
            Return
        End If

        Dim infomsg As String = "電話番号の同期が完了しました。\n以下の社員の携帯番号を変更又は削除してください。"

        For Each dr As DataRow In ds.Tables(0).Rows
            infomsg &= "\n" & dr.Item("社員コード").ToString.Trim & ":" & dr.Item("社員名").ToString.Trim
        Next

        psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, infomsg)

    End Sub
    Private Function mfIsTelNoIntegration() As Boolean
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        mfIsTelNoIntegration = False
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try

                cmdDB = New SqlCommand("ZMSTSEL007", conDB)
                cmdDB.Parameters.Add(pfSet_Param("dvs", SqlDbType.NVarChar, "1"))

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    mfIsTelNoIntegration = True
                Else
                    mfIsTelNoIntegration = False
                End If

            Catch ex As Exception
                mfIsTelNoIntegration = False
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Function

    Private Sub msSetCrsDropDownList(Optional ByVal _blnIsinitializeOnly As Boolean = True, Optional ByVal _blnCrsDvs As Boolean = False)

        For Each Item As ListItem In ddlVscDvs.ppDropDownList.Items
            Item.Enabled = True
        Next
        For Each Item As ListItem In ddlMailDvs.ppDropDownList.Items
            Item.Enabled = True
        Next

        If _blnIsinitializeOnly Then
            Exit Sub
        End If

        'アイテム毎のインデックス値を取得
        Dim intIndexOfVsc0 As Integer = ddlVscDvs.ppDropDownList.Items.IndexOf(ddlVscDvs.ppDropDownList.Items.FindByValue("0"))
        Dim intIndexOfVsc1 As Integer = ddlVscDvs.ppDropDownList.Items.IndexOf(ddlVscDvs.ppDropDownList.Items.FindByValue("1"))
        Dim intIndexOfVsc9 As Integer = ddlVscDvs.ppDropDownList.Items.IndexOf(ddlVscDvs.ppDropDownList.Items.FindByValue("9"))
        Dim intIndexOfMail0 As Integer = ddlMailDvs.ppDropDownList.Items.IndexOf(ddlMailDvs.ppDropDownList.Items.FindByValue("0"))
        Dim intIndexOfMail1 As Integer = ddlMailDvs.ppDropDownList.Items.IndexOf(ddlMailDvs.ppDropDownList.Items.FindByValue("1"))
        Dim intIndexOfMail9 As Integer = ddlMailDvs.ppDropDownList.Items.IndexOf(ddlMailDvs.ppDropDownList.Items.FindByValue("9"))

        If _blnCrsDvs Then
            ddlVscDvs.ppDropDownList.Items.Item(0).Enabled = False
            ddlVscDvs.ppDropDownList.Items.Item(intIndexOfVsc9).Enabled = False
            ddlVscDvs.ppSelectedValue = "0"

            ddlMailDvs.ppDropDownList.Items.Item(0).Enabled = False
            ddlMailDvs.ppDropDownList.Items.Item(intIndexOfMail9).Enabled = False
            ddlMailDvs.ppSelectedValue = "0"
        Else
            ddlVscDvs.ppDropDownList.Items.Item(0).Enabled = False
            ddlVscDvs.ppDropDownList.Items.Item(intIndexOfVsc0).Enabled = False
            ddlVscDvs.ppDropDownList.Items.Item(intIndexOfVsc1).Enabled = False
            ddlVscDvs.ppSelectedValue = "9"

            ddlMailDvs.ppDropDownList.Items.Item(0).Enabled = False
            ddlMailDvs.ppDropDownList.Items.Item(intIndexOfMail0).Enabled = False
            ddlMailDvs.ppDropDownList.Items.Item(intIndexOfMail1).Enabled = False
            ddlMailDvs.ppSelectedValue = "9"
        End If

    End Sub

    Private Function mfMailReceiveEmployeeCount() As Boolean
        mfMailReceiveEmployeeCount = False
        Dim ds As New DataSet
        Dim con As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing


        'トランザクションの設定
        If clsDataConnect.pfOpen_Database(con) Then
            Try
                cmd = New SqlCommand("COMUPDM02_S6", con)
                cmd.CommandType = CommandType.StoredProcedure
                With cmd.Parameters
                    .Add(pfSet_Param("empcd", SqlDbType.NVarChar, txtEmpCd.ppText.Trim))
                    .Add(pfSet_Param("ofccd", SqlDbType.NVarChar, ddlOffice.SelectedValue))
                End With
                ds = clsDataConnect.pfGet_DataSet(cmd)

                If ds.Tables(1).Rows.Count = 0 OrElse ds.Tables(1).Rows(0).Item("送信最大件数") Is Nothing Then
                    'エラーメッセージ表示
                    psMesBox(Me, "00000", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "メール送信対象の取得処理に失敗しました。")
                    Return False
                End If
                If ds.Tables(0).Rows.Count = 0 OrElse ds.Tables(0).Rows(0).Item("送信対象数") < ds.Tables(1).Rows(0).Item("送信最大件数") Then
                    mfMailReceiveEmployeeCount = True
                Else
                    If Master.ppBtnDelete.Text = "削除取消" Then
                        Dim infomsg As String = "メール送信対象の責任者は" & ds.Tables(1).Rows(0).Item("送信最大件数").ToString & "名までです。\n社員マスタの内容を確認してください。"

                        For Each dr As DataRow In ds.Tables(2).Rows
                            infomsg &= "\n" & dr.Item("送信対象者").ToString.Trim
                        Next

                        psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, infomsg)
                    Else
                        ddlMailDvs.psSet_ErrorNo("2024", "メール送信対象", "責任者" & ds.Tables(1).Rows(0).Item("送信最大件数").ToString & "名まで")
                    End If
                    Return False
                End If
            Catch ex As Exception
                mfMailReceiveEmployeeCount = False
                'エラーメッセージ表示
                psMesBox(Me, "00000", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "メール送信対象の取得処理に失敗しました。")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                  objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(con) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If
    End Function
    'COMUPDM02-003 END
#End Region

#Region "終了処理プロシージャ"

#End Region

End Class
