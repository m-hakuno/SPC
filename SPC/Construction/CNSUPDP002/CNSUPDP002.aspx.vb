'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　物品転送依頼書 参照／更新
'*　ＰＧＭＩＤ：　CNSUPDP002
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.11.07　：　土岐
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'CNSUPDP002-001     2015/05/18      加賀      担当者ドロップダウンリストWidth設定              
'CNSUPDP002-002     2015/05/18      加賀      依頼番号変更時の画面更新処理追加              
'CNSUPDP002-003     2015/05/18      加賀      依頼番号-TBOXID 整合性チェック              
'CNSUPDP002-004     2015/05/19      加賀      営業所コード、会社コード変更時フォーカス制御              
'CNSUPDP002-005     2015/05/19      加賀      物品未登録時のエラー設定              
'CNSUPDP002-006     2015/06/09      加賀      更新モード時：部材の[登録/更新/削除]押下時にデータ更新を行わず、グリッド反映のみ            
'CNSUPDP002-007     2015/06/09      加賀      更新モード時：明細データ更新処理を追加      
'CNSUPDP002-008     2015/06/10      加賀      依頼番号変更時の物品機材データ取得処理追加(工事用の場合のみ)
'CNSUPDP002-009     2015/06/23      加賀      担当者ドロップダウンのM02削除社員対策
'CNSUPDP002-010     2015/06/24      加賀      工事・保守の判別を遷移元画面から依頼番号に変更
'CNSUPDP002-011     2015/06/24      加賀      工事種別の初期化、再取得処理の追加
'CNSUPDP002-012     2015/07/03      加賀      部材追加更新処理変更 
'CNSUPDP002-013     2015/07/03      加賀      部材情報の変更を更新していない場合、印刷押下時にエラーメッセージを表示 
'CNSUPDP002-014     2015/07/13      加賀      工事用データの出荷種別又は依頼番号が変更された場合、更新時にエラーメッセージを表示 
'CNSUPDP002-015     2015/07/13      加賀      依頼番号時に形式チェック、重複チェックを追加 
'CNSUPDP002-016     2017/04/20      加賀      梱包材に対応 
'CNSUPDP002-017     2017/04/26      加賀      明細クリアボタン追加
'CNSUPDP002-018     2017/05/31      伯野      印刷時の明細保存チェックを外す


#Region "インポート定義"

Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMDataLink
Imports System.Data.SqlClient
Imports SPC.ClsCMExclusive

#End Region

Public Class CNSUPDP002

#Region "継承定義"
    Inherits System.Web.UI.Page
#End Region

#Region "定数定義"

    Private Const M_VIEW_SEL = "選択データ"                          'ViewStateの明細選択行キー
    Private Const M_VIEW_SEL_DIPS_SEQ = "選択表示順"                 'ViewStateの明細選択表示順
    Private Const M_VIEW_UPDATE = "更新フラグ"                       'ViewStateの更新フラグ
    Private Const M_VIEW_DELFLH = "削除フラグ"                       'ViewStateの削除フラグ
    Private Const M_TRN_FILE_NM = "物品転送依頼書"                   '送信ファイル名
    Private Const M_REP_PAC = "1"                                    '依頼区分：物品転送依頼
    Private Const M_REP_ART = "2"                                    '依頼区分：梱包箱出荷依頼
    Private Const M_DISP_ID = P_FUN_CNS & P_SCR_UPD & P_PAGE & "002"
    Const MES_NO_DEL = "00010"

#End Region

#Region "構造体・列挙体定義"

#End Region

#Region "変数定義"

    Dim objStack As StackFrame
    Dim clsSqlDbSvr As New ClsSQLSvrDB
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom
    Dim clsExc As New ClsCMExclusive
    Dim strBaseDuty As String = String.Empty

#End Region

#Region "イベントプロシージャ"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        '表設定
        pfSet_GridView(grvArtcltrnsDTL, "CNSUPDP002")

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンアクションの設定
        AddHandler Master.ppLeftButton1.Click, AddressOf btnUpdate_Click
        AddHandler Master.ppLeftButton2.Click, AddressOf btnDEL_Click
        AddHandler Master.ppLeftButton3.Click, AddressOf btnClear_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnPrint_Click

        '基本情報取得
        AddHandler Me.txtTboxID.ppTextBox.TextChanged, AddressOf txtTboxID_TextChanged
        AddHandler Me.txtRequestNo.ppTextBox.TextChanged, AddressOf txtRequestNo_TextChanged  'CNSUPDP002-002
        '業者情報取得設定
        AddHandler Me.txtCompCD.ppTextBox.TextChanged, AddressOf txtCompCD_TextChanged
        AddHandler Me.txtOfficCD.ppTextBox.TextChanged, AddressOf txtOfficCD_TextChanged
        AddHandler Me.txtAppaCD.ppTextBox.TextChanged, AddressOf txtAppaCD_TextChanged

        Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる

        'Select Case ddlBasedutyCD.ppSelectedValue
        '    Case "01"
        '        strBaseDuty = "工事依頼番号"
        '    Case "03"
        '        strBaseDuty = "保守管理番号"
        '    Case Else
        '        strBaseDuty = "依頼番号"
        'End Select

        ''削除
        'Master.ppRigthButton1.Visible = True
        'Master.ppRigthButton1.Text = "削除"
        'Master.ppRigthButton1.CausesValidation = False
        'Master.ppRigthButton1.BackColor = Drawing.Color.FromArgb(255, 102, 102)
        'Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "スケジュール")


        If Not IsPostBack Then

            '削除ボタン
            btnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "選択した" & lblArtcltrnsDTL.Text)   '削除

            '業者情報参照ボタンのリンク先設定
            btnTrader.OnClientClick =
                "return window_open('" & VirtualPathUtility.ToAbsolute("~/Common/COMSELP002/COMSELP002.aspx") & "')"

            'プログラムＩＤ、画面名設定
            Master.ppProgramID = M_DISP_ID
            Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

            'パンくずリスト設定
            Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

            'セッション変数「遷移条件」「遷移元ＩＤ」が存在しない場合、画面を閉じる
            If Session(P_SESSION_TERMS) Is Nothing Or Session(P_SESSION_OLDDISP) Is Nothing Then
                psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                '排他削除
                clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                psClose_Window(Me)
                Return
            End If

            'ViewStateに「遷移条件」「遷移元ＩＤ」を保存
            ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)
            ViewState(P_SESSION_OLDDISP) = Session(P_SESSION_OLDDISP)
            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            '★排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
            If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then

                Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)

            End If

            '登録以外は指示Ｎｏ．を設定
            If Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録 Then
                If ViewState(P_SESSION_OLDDISP) = P_FUN_CNS & P_SCR_LST & P_PAGE & "002" Then
                    '物品転送依頼一覧
                    ViewState(P_KEY) = Nothing
                Else
                    '工事依頼書兼仕様書,配送機器一覧
                    ViewState(P_KEY) = Session(P_KEY)(0)
                End If
            Else
                ViewState(P_KEY) = Session(P_KEY)(0)
                Me.msSetArtclNo(ViewState(P_KEY))
            End If

            'フッターボタン表示設定
            Master.ppLeftButton1.Visible = True         '登録／更新ボタン
            Master.ppLeftButton3.Visible = True         'クリアボタン
            Master.ppRigthButton2.Visible = True        '印刷ボタン

            'ボタン名設定
            Master.ppLeftButton1.Text = P_BTN_NM_UPD
            Master.ppLeftButton2.Text = "キャンセル"
            Master.ppLeftButton3.Text = P_BTN_NM_CLE
            Master.ppRigthButton2.Text = P_BTN_NM_PRI

            Master.ppLeftButton2.OnClientClick =
                pfGet_OCClickMes(MES_NO_DEL, ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, M_TRN_FILE_NM)
            Master.ppLeftButton3.OnClientClick =
                pfGet_OCClickMes("00002", ClsComVer.E_Mタイプ.警告, ClsComVer.E_Mモード.OKCancel)

            '検証グループ設定
            'Master.ppLeftButton1.ValidationGroup = "Artcltrns" 'CNSUPDP002-003

            '押下時の検証を無効
            Master.ppLeftButton2.CausesValidation = False
            Master.ppLeftButton3.CausesValidation = False
            Master.ppRigthButton2.CausesValidation = False

            'ポストバック設定
            Me.txtCompCD.ppTextBox.AutoPostBack = True
            Me.txtOfficCD.ppTextBox.AutoPostBack = True
            Me.txtAppaCD.ppTextBox.AutoPostBack = True
            Me.txtTboxID.ppTextBox.AutoPostBack = True
            Me.txtRequestNo.ppTextBox.AutoPostBack = True 'CNSUPDP002-002

            '画面設定
            msClearScreen(ViewState(P_SESSION_TERMS), ViewState(P_SESSION_OLDDISP), ViewState(P_KEY))

        End If

        'ViewState「遷移条件」が存在しない場合、画面を閉じる
        If ViewState(P_SESSION_TERMS) Is Nothing Then
            psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            '排他削除
            clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
            psClose_Window(Me)
            Return
        End If

        '活性／非活性設定
        msSet_Mode(ViewState(P_SESSION_TERMS))

    End Sub

    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
                txtArtclNo1.ppEnabled = False
                txtArtclNoT2.Enabled = False
                txtArtclNoT3.Enabled = False
                txtArtclNoT4.Enabled = False
                ddlBasedutyCD.ppEnabled = False
                txtRequestNo.ppEnabled = False
                ttxCommNo.ppEnabled = False
                txtTboxID.ppEnabled = False
                Master.ppLeftButton2.Enabled = False
                Master.ppLeftButton1.Enabled = False
            Case "NGC"
                Master.ppLeftButton1.Enabled = False
                Master.ppLeftButton2.Enabled = False
                Master.ppLeftButton3.Enabled = False
                btnTrader.Enabled = False
                btnAdd.Enabled = False
                btnUpdate.Enabled = False
                btnDelete.Enabled = False
        End Select
    End Sub

    ''' <summary>
    ''' 依頼区分の検証
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvRepuestCls_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvRepuestCls.ServerValidate
        Dim dtrMes As DataRow
        args.IsValid = True
        If rblrdoRepuestClsV.SelectedIndex < 0 Then '未選択
            dtrMes = pfGet_ValMes("5003", "依頼区分")
            cuvRepuestCls.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            cuvRepuestCls.Text = dtrMes.Item(P_VALMES_SMES)

            args.IsValid = False
        End If
    End Sub

    ''' <summary>
    ''' 物品機材登録一覧選択処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvArtcltrnsDTL_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvArtcltrnsDTL.RowCommand

        'ログ出力開始
        psLogStart(Me)

        Select Case e.CommandName
            Case "btnSelect"
                Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    ' ボタン押下行のIndex
                Dim rowData As GridViewRow = grvArtcltrnsDTL.Rows(intIndex)             ' ボタン押下行

                '★排他制御処理
                Dim strExclusiveDate As String = Nothing
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList
                Select Case ViewState(P_SESSION_TERMS)
                    Case ClsComVer.E_遷移条件.更新

                        '★排他情報削除
                        If Not Me.Master.ppExclusiveDateDtl = String.Empty Then

                            If clsExc.pfDel_Exclusive(Me _
                                               , Session(P_SESSION_SESSTION_ID) _
                                               , Me.Master.ppExclusiveDateDtl) = 0 Then
                                Me.Master.ppExclusiveDateDtl = String.Empty
                            Else
                                Exit Sub
                            End If
                        End If

                End Select

                Me.txtAppaCD.ppText = CType(rowData.FindControl("物品／部材コード"), TextBox).Text
                Me.lblAppaNm.Text = CType(rowData.FindControl("物品／部材名"), TextBox).Text
                Me.txtQuantity.ppText = CType(rowData.FindControl("数量"), TextBox).Text
                Me.ddlAppaCnds.ppSelectedValue = CType(rowData.FindControl("物品状況コード"), TextBox).Text
                Me.txtNotetext.ppText = CType(rowData.FindControl("備考"), TextBox).Text
                ViewState(M_VIEW_SEL) = CType(rowData.FindControl("Ｎｏ"), TextBox).Text
                ViewState(M_VIEW_SEL_DIPS_SEQ) = CType(rowData.FindControl("表示順"), TextBox).Text

                msSet_DtlMode("2")

        End Select
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 登録／更新処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)
        'CNSUPDP002-001
        ''工事依頼番号チェック（仮依頼番号は不可）
        'If Me.txtRequestNo.ppText.Contains("N0090") Then
        '    psMesBox(Me, "50001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "物品転送依頼書")
        '    Exit Sub
        'End If
        'CNSUPDP002-001 END

        Me.Validate("Artcltrns") 'CNSUPDP002-003

        '個別エラーチェック
        msCheck_Error()

        If Me.txtCompCD.ppText <> String.Empty AndAlso Me.txtOfficCD.ppText <> String.Empty Then
            Dim dtsTRaderData As DataSet = Nothing
            If mfGet_Trader(Me.txtCompCD.ppText, Me.txtOfficCD.ppText, dtsTRaderData) Then
            Else
                Me.txtOfficCD.psSet_ErrorNo("2009", txtCompCD.ppName, txtOfficCD.ppName)
            End If
        End If

        'If (Page.IsValid) AndAlso grvArtcltrnsDTL.Rows.Count > 0 Then 'CNSUPDP002-005
        If (Page.IsValid) Then
            '物品登録確認 CNSUPDP002-005
            If grvArtcltrnsDTL.Rows.Count = 0 Then
                '物品未登録エラー
                psMesBox(Me, "30021", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "部品/部材が登録されていません。登録")
                Exit Sub
            End If
            'CNSUPDP002-005 END

            Dim conDB As SqlConnection = Nothing
            Dim cmdDB As SqlCommand = Nothing
            Dim dstOrders As DataSet = Nothing
            Dim intRtn As Integer
            Dim strArtcl_no As String

            objStack = New StackFrame

            '指示Ｎｏ
            Select Case ViewState(P_SESSION_TERMS)
                Case ClsComVer.E_遷移条件.登録
                    strArtcl_no = Me.txtArtclNo1.ppText
                Case ClsComVer.E_遷移条件.更新
                    strArtcl_no = Me.txtArtclNo1.ppText & Me.txtArtclNoT2.Text & Me.txtArtclNoT3.Text & Me.txtArtclNoT4.Text
                Case Else
                    strArtcl_no = Nothing
            End Select

            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    '依頼番号重複チェック
                    If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録 Then
                        If Me.txtRequestNo.ppText <> "" Then
                            cmdDB = New SqlCommand("CNSUPDP002_S11", conDB)

                            With cmdDB.Parameters
                                'パラメータ設定
                                .Add(pfSet_Param("request_no", SqlDbType.NVarChar, Me.txtRequestNo.ppText))                 '依頼番号
                            End With

                            'データ取得
                            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                            If dstOrders.Tables(0).Rows.Count > 0 Then
                                'psMesBox(Me, "30010", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "同一の依頼番号", "登録")
                                psMesBox(Me, "30023", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "同一の依頼番号が登録されています。")
                                Exit Sub
                            End If
                        End If
                    Else
                        'CNSUPDP002-014
                        cmdDB = New SqlCommand("CNSUPDP002_S1", conDB)
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("artcl_no", SqlDbType.NVarChar, strArtcl_no))
                        End With

                        '物品転送依頼リストデータ取得
                        dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                        If dstOrders.Tables(0).Rows(0)("出荷種別").ToString = "01" Then
                            If ddlBasedutyCD.ppSelectedValue.ToString <> "01" _
                            OrElse dstOrders.Tables(0).Rows(0)("依頼番号").ToString <> txtRequestNo.ppText Then
                                psMesBox(Me, "30024", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "物品転送の基本情報", "更新")
                                Exit Sub
                            End If
                        End If
                        'CNSUPDP002-014 END
                    End If

                    cmdDB = New SqlCommand("CNSUPDP002_U1", conDB)
                    With cmdDB.Parameters
                        'パラメータ設定
                        '物品転送管理番号
                        .Add(pfSet_Param("artcl_no", SqlDbType.NVarChar, strArtcl_no))
                        .Add(pfSet_Param("ship_cls", SqlDbType.NVarChar, Me.ddlBasedutyCD.ppSelectedValue))         '出荷種別
                        .Add(pfSet_Param("repuest_cls", SqlDbType.NVarChar, Me.rblrdoRepuestClsV.SelectedValue))    '依頼区分
                        .Add(pfSet_Param("request_no", SqlDbType.NVarChar, Me.txtRequestNo.ppText))                 '依頼番号
                        .Add(pfSet_Param("comm_no", SqlDbType.NVarChar, Me.ttxCommNo.ppText))                       '通知番号
                        .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxID.ppText))                        'ＴＢＯＸＩＤ
                        .Add(pfSet_Param("send_cd", SqlDbType.NVarChar, Me.txtCompCD.ppText))                       '送付先コード
                        .Add(pfSet_Param("send_branch_cd", SqlDbType.NVarChar, Me.txtOfficCD.ppText))               '営業所コード
                        .Add(pfSet_Param("deliv_dt", SqlDbType.NVarChar, Me.dtbDelivDT.ppText))                     '納期日
                        .Add(pfSet_Param("send_charge_cd", SqlDbType.NVarChar, Me.ddlChargeV.SelectedValue))        '担当者コード
                        .Add(pfSet_Param("sp_notetext", SqlDbType.NVarChar, Me.txtSPNotetext.ppText))               '特記事項
                        .Add(pfSet_Param("artcltrans_d", SqlDbType.NVarChar, Me.dtbArtcltransD.ppText))             '物品転送依頼日
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                         'ユーザーＩＤ
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                           '戻り値
                        Select Case ViewState(P_SESSION_TERMS)
                            Case ClsComVer.E_遷移条件.登録
                                .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, "1")) '処理区分
                            Case ClsComVer.E_遷移条件.更新
                                .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, "2")) '処理区分
                        End Select
                        'CNSUPDP002-010
                        'Select Case ViewState(P_SESSION_OLDDISP)
                        '    Case P_FUN_CNS & P_SCR_UPD & P_PAGE & "001"     '工事依頼書兼仕様書
                        '        .Add(pfSet_Param("concnst_no", SqlDbType.NVarChar, ViewState(P_KEY)))   '工事依頼番号
                        '        '-----------------------------
                        '        '2014/04/15 土岐　ここから
                        '        '-----------------------------
                        '    Case P_FUN_EQU & P_SCR_LST & P_PAGE & "001"     '配送機器一覧
                        '        '-----------------------------
                        '        '2014/04/15 土岐　ここまで
                        '        '-----------------------------
                        '        .Add(pfSet_Param("conment_no", SqlDbType.NVarChar, ViewState(P_KEY)))   '保守管理番号
                        'End Select
                        If txtRequestNo.ppText.IndexOf("N0010") >= 0 OrElse txtRequestNo.ppText.IndexOf("N0090") >= 0 Then
                            .Add(pfSet_Param("concnst_no", SqlDbType.NVarChar, txtRequestNo.ppText))   '工事依頼番号
                        Else
                            .Add(pfSet_Param("conment_no", SqlDbType.NVarChar, txtRequestNo.ppText))   '保守管理番号
                        End If
                        'Select Case ddlBasedutyCD.ppSelectedValue
                        '    Case "01"
                        '        .Add(pfSet_Param("concnst_no", SqlDbType.NVarChar, txtRequestNo.ppText))   '工事依頼番号
                        '    Case "03"
                        '        .Add(pfSet_Param("conment_no", SqlDbType.NVarChar, txtRequestNo.ppText))   '保守管理番号
                        'End Select
                        'CNSUPDP002-010 END

                    End With

                    'データ追加／更新
                    Using conTrn = conDB.BeginTransaction
                        cmdDB.Transaction = conTrn

                        '更新
                        Select Case ViewState(P_SESSION_TERMS)
                            Case ClsComVer.E_遷移条件.登録
                                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                                'ストアド戻り値チェック
                                intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                                If intRtn <> 0 Then
                                    psMesBox(Me, "00008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "物品転送依頼書", intRtn)
                                    Exit Sub
                                End If

                                '明細登録
                                For Each rowData As GridViewRow In grvArtcltrnsDTL.Rows

                                    'ストアド戻り値チェック
                                    intRtn = mfUpdateDTL(conDB,
                                                         conTrn,
                                                         "1",
                                                         dstOrders.Tables(0).Rows(0).Item("指示Ｎｏ").ToString,
                                                         0,
                                                         Me.rblrdoRepuestClsV.SelectedValue,
                                                         CType(rowData.FindControl("表示順"), TextBox).Text,
                                                         CType(rowData.FindControl("Ｎｏ"), TextBox).Text,
                                                         CType(rowData.FindControl("物品／部材コード"), TextBox).Text,
                                                         CType(rowData.FindControl("物品／部材名"), TextBox).Text,
                                                         CType(rowData.FindControl("物品状況コード"), TextBox).Text,
                                                         Decimal.Parse(CType(rowData.FindControl("数量"), TextBox).Text),
                                                         CType(rowData.FindControl("備考"), TextBox).Text)

                                    If intRtn <> 0 Then
                                        psMesBox(Me,
                                                 "00008",
                                                 ClsComVer.E_Mタイプ.エラー,
                                                 ClsComVer.E_S実行.描画後,
                                                 "物品転送依頼書",
                                                 intRtn.ToString)
                                        Exit Sub
                                    End If
                                Next

                            Case ClsComVer.E_遷移条件.更新
                                'コマンドタイプ設定(ストアド)
                                cmdDB.CommandType = CommandType.StoredProcedure
                                cmdDB.ExecuteNonQuery()

                                'ストアド戻り値チェック
                                intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                                If intRtn <> 0 Then
                                    psMesBox(Me, "00007", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "物品転送依頼書", intRtn.ToString)
                                    Exit Sub
                                End If

                                'CNSUPDP002-007 明細登録
                                ''明細数確認
                                'cmdDB = New SqlCommand("CNSUPDP002_S2", conDB)
                                'cmdDB.Transaction = conTrn
                                'With cmdDB.Parameters 'パラメータ設定
                                '    .Add(pfSet_Param("artcl_no", SqlDbType.NVarChar, strArtcl_no))  '物品転送管理番号
                                '    .Add(pfSet_Param("delete_flg", SqlDbType.SmallInt, 0))          '削除区分
                                'End With
                                'dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                                '削除
                                cmdDB = New SqlCommand("CNSUPDP002_D2", conDB)
                                cmdDB.Transaction = conTrn
                                With cmdDB.Parameters 'パラメータ設定
                                    .Add(pfSet_Param("artcl_no", SqlDbType.NVarChar, strArtcl_no))      '物品転送管理番号
                                    '.Add(pfSet_Param("seqno", SqlDbType.Decimal, 1))                    '連番
                                    .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name)) 'ユーザーＩＤ
                                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                   '戻り値
                                End With

                                'コマンドタイプ設定(ストアド)
                                cmdDB.CommandType = CommandType.StoredProcedure
                                cmdDB.ExecuteNonQuery()

                                'ストアド戻り値チェック
                                If 0 <> Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString) Then
                                    psMesBox(Me, "00007", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "物品転送依頼書", intRtn.ToString)
                                    Exit Sub
                                End If

                                '登録/更新処理
                                Dim dt_grv As DataTable = pfParse_DataTable(Me.grvArtcltrnsDTL)
                                For Each objrow As DataRow In dt_grv.Rows
                                    If objrow.Item("連番").ToString = String.Empty Then
                                        'INSERT
                                        intRtn = mfUpdateDTL(conDB,
                                                         conTrn,
                                                         "1",
                                                         strArtcl_no,
                                                         0,
                                                         Me.rblrdoRepuestClsV.SelectedValue,
                                                         objrow.Item("表示順").ToString,
                                                         objrow.Item("Ｎｏ").ToString,
                                                         objrow.Item("物品／部材コード").ToString,
                                                         objrow.Item("物品／部材名").ToString,
                                                         objrow.Item("物品状況コード").ToString,
                                                         Decimal.Parse(objrow.Item("数量").ToString),
                                                         objrow.Item("備考").ToString)
                                        If intRtn <> 0 Then
                                            psMesBox(Me, "00007", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "物品転送依頼書", intRtn.ToString)
                                            Exit Sub
                                        End If
                                    Else
                                        'UPDATE
                                        intRtn = mfUpdateDTL(conDB,
                                                         conTrn,
                                                         "2",
                                                         strArtcl_no,
                                                         Decimal.Parse(objrow.Item("連番").ToString),
                                                         Me.rblrdoRepuestClsV.SelectedValue,
                                                         objrow.Item("表示順").ToString,
                                                         objrow.Item("Ｎｏ").ToString,
                                                         objrow.Item("物品／部材コード").ToString,
                                                         objrow.Item("物品／部材名").ToString,
                                                         objrow.Item("物品状況コード").ToString,
                                                         Decimal.Parse(objrow.Item("数量").ToString),
                                                         objrow.Item("備考").ToString)
                                        If intRtn <> 0 Then
                                            psMesBox(Me, "00007", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "物品転送依頼書", intRtn.ToString)
                                            Exit Sub
                                        End If
                                    End If
                                Next
                                'CNSUPDP002-007 END

                                '更新フラグがなければ変更回数を更新
                                If ViewState(M_VIEW_UPDATE) = Nothing Then
                                    mfUpdateCount(conDB, conTrn, strArtcl_no)
                                End If
                        End Select

                        'コミット
                        conTrn.Commit()

                        Select Case ViewState(P_SESSION_TERMS)
                            Case ClsComVer.E_遷移条件.登録
                                psMesBox(Me, "00009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "物品転送依頼書")
                            Case ClsComVer.E_遷移条件.更新
                                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "物品転送依頼書")
                        End Select
                        Select Case ViewState(P_SESSION_TERMS)
                            Case ClsComVer.E_遷移条件.登録
                                '登録時の指示Ｎｏを設定
                                strArtcl_no = dstOrders.Tables(0).Rows(0).Item("指示Ｎｏ").ToString
                                msSetArtclNo(strArtcl_no)

                                Dim strExclusiveDate As String = Nothing
                                Dim arTable_Name As New ArrayList
                                Dim arKey As New ArrayList

                                '★ロック対象テーブル名の登録
                                arTable_Name.Insert(0, "D19_ARTCLTRNS")
                                '★ロックテーブルキー項目の登録(D39_CNSTREQSPEC)
                                arKey.Insert(0, dstOrders.Tables(0).Rows(0).Item("指示Ｎｏ").ToString)

                                '★排他情報確認処理(更新画面へ遷移)
                                If clsExc.pfSel_Exclusive(strExclusiveDate _
                                                 , Me _
                                                 , Session(P_SESSION_IP) _
                                                 , Session(P_SESSION_PLACE) _
                                                 , Session(P_SESSION_USERID) _
                                                 , Session(P_SESSION_SESSTION_ID) _
                                                 , ViewState(P_SESSION_GROUP_NUM) _
                                                 , P_FUN_CNS & P_SCR_UPD & P_PAGE & "002" _
                                                 , arTable_Name _
                                                 , arKey) = 0 Then

                                    '★登録年月日時刻
                                    Me.Master.ppExclusiveDate = strExclusiveDate

                                Else

                                    '排他ロック中
                                    Exit Sub

                                End If

                                '更新画面に変更
                                ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
                                ViewState(P_KEY) = strArtcl_no

                            Case ClsComVer.E_遷移条件.更新
                                '更新フラグＯＮ
                                ViewState(M_VIEW_UPDATE) = True
                        End Select
                    End Using

                    '画面クリア
                    msClearScreen(ViewState(P_SESSION_TERMS), ViewState(P_SESSION_OLDDISP), strArtcl_no)
                    '活性／非活性設定
                    msSet_Mode(ViewState(P_SESSION_TERMS))

                Catch ex As Exception
                    Select Case ViewState(P_SESSION_TERMS)
                        Case ClsComVer.E_遷移条件.登録
                            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "物品転送依頼書")
                        Case ClsComVer.E_遷移条件.更新
                            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "物品転送依頼書")
                    End Select

                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Finally
                    'DB切断
                    If Not clsDataConnect.pfClose_Database(conDB) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                    'Dispose
                    clsSqlDbSvr.psDisposeDataSet(dstOrders)
                End Try
            Else
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End If

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 削除処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDEL_Click(sender As Object, e As EventArgs)

        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet
        Dim intRtn As Integer
        Dim strArtcl_no As String

        objStack = New StackFrame

        'ログ出力開始
        psLogStart(Me)

        '初期化
        conDB = Nothing

        '指示Ｎｏ
        strArtcl_no = txtArtclNo1.ppText & txtArtclNoT2.Text & txtArtclNoT3.Text & txtArtclNoT4.Text

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("CNSUPDP002_D1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    '物品転送管理番号
                    .Add(pfSet_Param("artcl_no", SqlDbType.NVarChar, strArtcl_no))
                    .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))     'ユーザーＩＤ
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                       '戻り値
                End With

                'データ更新
                Using conTrn = conDB.BeginTransaction
                    cmdDB.Transaction = conTrn

                    'コマンドタイプ設定(ストアド)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, "00009", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "物品転送依頼書", intRtn.ToString)
                        Exit Sub
                    End If

                    '更新フラグがなければ変更回数を更新
                    If ViewState(M_VIEW_UPDATE) = Nothing Then
                        mfUpdateCount(conDB, conTrn, strArtcl_no)
                    End If

                    'コミット
                    conTrn.Commit()

                    psMesBox(Me, "00011", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "物品転送依頼書")

                    '更新フラグＯＮ
                    ViewState(M_VIEW_UPDATE) = True

                    '画面クリア
                    msClearScreen(ViewState(P_SESSION_TERMS), ViewState(P_SESSION_OLDDISP), strArtcl_no)
                    '活性／非活性設定
                    msSet_Mode(ViewState(P_SESSION_TERMS))
                End Using
            Catch ex As Exception
                psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "物品転送依頼書")

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' クリアボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        'イベント発生元
        If sender.Equals(txtRequestNo.ppTextBox) Then 'CNSUPDP002-02
            '依頼番号
            msClearScreen(ViewState(P_SESSION_TERMS), ViewState(P_SESSION_OLDDISP), txtRequestNo.ppText)
        Else
            'クリアボタン
            msClearScreen(ViewState(P_SESSION_TERMS), ViewState(P_SESSION_OLDDISP), ViewState(P_KEY))
        End If

        '活性／非活性設定
        msSet_Mode(ViewState(P_SESSION_TERMS))

        msSet_DtlMode("1")

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 印刷ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPrint_Click(sender As Object, e As EventArgs)

        Dim dtOld As DataTable = DirectCast(ViewState("ArtcltrnsDTL"), DataTable)
        Dim dtNew As DataTable = pfParse_DataTable(Me.grvArtcltrnsDTL)

        objStack = New StackFrame

        Using dvNew As New DataView(dtNew)
            dvNew.Sort = "Ｎｏ"
            dtNew = dvNew.ToTable
        End Using

        'CNSUPDP002-018
        '        If dtOld.AsEnumerable().SequenceEqual(dtNew.AsEnumerable(), DataRowComparer.Default) = False Then
        '            psMesBox(Me, "30021", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "部品/部材が変更されています。更新")
        '            Exit Sub
        '        End If
        'CNSUPDP002-018

        Dim rpt As CNSREP002
        Dim strFNm As String
        Dim strArtcl_no As String
        Dim strSql As StringBuilder

        'ログ出力開始
        psLogStart(Me)

        '指示Ｎｏ
        strArtcl_no = txtArtclNo1.ppText & txtArtclNoT2.Text & txtArtclNoT3.Text & txtArtclNoT4.Text
        strFNm = M_TRN_FILE_NM
        strSql = New StringBuilder
        strSql.Append("EXEC CNSUPDP002_S3 '")
        strSql.Append(strArtcl_no)
        strSql.Append("'")
        rpt = New CNSREP002
        psPrintPDF(Me, rpt, strSql.ToString, strFNm)

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 依頼番号変更時
    ''' </summary>
    Protected Sub txtRequestNo_TextChanged(sender As Object, e As EventArgs)
        'CNSUPDP002-002

        objStack = New StackFrame

        Dim dstArtcltrns As DataSet = Nothing

        '選択明細クリア
        msClearSelect()

        If Me.txtRequestNo.ppText = String.Empty Then
            '未入力
            Me.ttxCommNo.ppText = String.Empty              '通知番号
            Me.txtTboxID.ppText = String.Empty              'ＴＢＯＸＩＤ
            Me.lblHallNmV.Text = String.Empty               'ホール名
            Me.lblNlClsV.Text = String.Empty                'ＮＬ区分
            Me.dtbArtcltransD.ppText = DateTime.Now.ToString("yyyy/MM/dd")
            Dim US As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-US")
            Me.dtbArtcltransD.ppYobiText = DateTime.Now.ToString("ddd", US).ToUpper
            Me.dtbDelivDT.ppText = String.Empty             '納期
            Me.dtbDelivDT.ppYobiText = String.Empty         '納期(曜日)
            Me.txtCompCD.ppText = String.Empty              '会社コード
            Me.txtOfficCD.ppText = String.Empty             '営業所コード
            Me.lblSendNmV.Text = String.Empty               '送付先名
            Me.lblBranchV.Text = String.Empty               '営業所名
            Me.ddlChargeV.Items.Clear()                     '担当者名
            Me.ddlChargeV.Items.Add("")
            Me.ddlChargeV.Width = 143
            Me.lblZipnoV.Text = String.Empty                '郵便番号
            Me.lblPrefV.Text = String.Empty                 '県コード
            Me.lblAddrV.Text = String.Empty                 '住所
            Me.lblTelV.Text = String.Empty                  'ＴＥＬ
            Me.lblFaxV.Text = String.Empty                  'Ｆａｘ
            Me.txtSPNotetext.ppText = String.Empty          '特記事項

            'CNSUPDP002-011　工事種別
            Me.lblCnstClsV1.Text = "□"
            Me.lblCnstClsV2.Text = "□"
            Me.lblCnstClsV3.Text = "□"
            Me.lblCnstClsV4.Text = "□"
            Me.lblCnstClsV5.Text = "□"
            Me.lblCnstClsV6.Text = "□"
            Me.lblCnstClsV7.Text = "□"
            Me.lblCnstClsV8.Text = "□"
            Me.lblCnstClsV9.Text = "□"
            Me.lblCnstClsV10.Text = "□"
            Me.lblCnstClsV11.Text = "□"
            'CNSUPDP002-011

            Me.grvArtcltrnsDTL.DataSource = New Object() {}
            Me.grvArtcltrnsDTL.DataBind()
        Else
            Dim conDB As SqlConnection = Nothing
            Dim cmdArtcltrns As SqlCommand = Nothing
            Dim strFlg As String = ddlBasedutyCD.ppSelectedValue.ToString

            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    'CNSUPDP002-015
                    '工事用の場合
                    If ddlBasedutyCD.ppSelectedValue = "01" Then
                        '形式チェック
                        If txtRequestNo.ppText.Length = 14 _
                        AndAlso txtRequestNo.ppText.Replace("N0090-", "").Replace("N0010-", "").Length = 8 _
                        AndAlso Regex.IsMatch(txtRequestNo.ppText.Replace("N0090-", "").Replace("N0010-", ""), "^[0-9]+$") Then
                            '形式ＯＫ
                        Else
                            '形式エラー
                            txtRequestNo.psSet_ErrorNo("4001", strBaseDuty, "正しい形式")
                            Exit Sub
                        End If
                    End If

                    '依頼番号重複チェック
                    cmdArtcltrns = New SqlCommand("CNSUPDP002_S11", conDB)

                    With cmdArtcltrns.Parameters    'パラメータ設定
                        .Add(pfSet_Param("request_no", SqlDbType.NVarChar, Me.txtRequestNo.ppText)) '依頼番号
                        If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 Then
                            .Add(pfSet_Param("artcl_no", SqlDbType.NVarChar, _
                            Me.txtArtclNo1.ppText & Me.txtArtclNoT2.Text & Me.txtArtclNoT3.Text & Me.txtArtclNoT4.Text))    '指示No
                        End If
                    End With

                    'データ取得
                    dstArtcltrns = clsDataConnect.pfGet_DataSet(cmdArtcltrns)

                    If dstArtcltrns.Tables(0).Rows.Count > 0 Then
                        '整合性エラー
                        If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 Then
                            txtRequestNo.psSet_ErrorNo("2006", strBaseDuty & " : " & txtRequestNo.ppText)
                        Else
                            txtRequestNo.psSet_ErrorNo("2020", strBaseDuty & " : " & txtRequestNo.ppText)
                        End If
                        Exit Sub
                    End If
                    'CNSUPDP002-015 END

                    '6/10
                    Me.grvArtcltrnsDTL.DataSource = New Object() {}
                    Me.grvArtcltrnsDTL.DataBind()
                    '6/10--

                    '工事
                    cmdArtcltrns = New SqlCommand("CNSUPDP002_S7", conDB)
                    With cmdArtcltrns.Parameters 'パラメータ設定
                        .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, txtRequestNo.ppText))
                    End With

                    'データ取得
                    dstArtcltrns = clsDataConnect.pfGet_DataSet(cmdArtcltrns)

                    If dstArtcltrns.Tables(0).Rows.Count = 0 Then
                        'CNSUPDP002-011　工事種別
                        Me.lblCnstClsV1.Text = "□"
                        Me.lblCnstClsV2.Text = "□"
                        Me.lblCnstClsV3.Text = "□"
                        Me.lblCnstClsV4.Text = "□"
                        Me.lblCnstClsV5.Text = "□"
                        Me.lblCnstClsV6.Text = "□"
                        Me.lblCnstClsV7.Text = "□"
                        Me.lblCnstClsV8.Text = "□"
                        Me.lblCnstClsV9.Text = "□"
                        Me.lblCnstClsV10.Text = "□"
                        Me.lblCnstClsV11.Text = "□"
                        'CNSUPDP002-011

                        '保守
                        cmdArtcltrns = New SqlCommand("CNSUPDP002_S9", conDB)
                        With cmdArtcltrns.Parameters 'パラメータ設定
                            .Add(pfSet_Param("trouble_no", SqlDbType.NVarChar, txtRequestNo.ppText))
                        End With

                        'データ取得
                        dstArtcltrns = clsDataConnect.pfGet_DataSet(cmdArtcltrns)
                    End If

                    '依頼番号存在確認
                    If dstArtcltrns.Tables(0).Rows.Count = 0 Then
                        '該当データ無し
                        'txtRequestNo.psSet_ErrorNo("2002", txtRequestNo.ppName & " : " & txtRequestNo.ppText)

                        'Me.ddlBasedutyCD.ppSelectedValue = Nothing      '出荷種別
                        Me.ttxCommNo.ppText = String.Empty              '通知番号
                        Me.txtTboxID.ppText = String.Empty              'ＴＢＯＸＩＤ
                        Me.lblHallNmV.Text = String.Empty               'ホール名
                        Me.lblNlClsV.Text = String.Empty                'ＮＬ区分
                        Me.dtbArtcltransD.ppText = DateTime.Now.ToString("yyyy/MM/dd")
                        Dim US As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-US")
                        Me.dtbArtcltransD.ppYobiText = DateTime.Now.ToString("ddd", US).ToUpper
                        Me.dtbDelivDT.ppText = String.Empty             '納期
                        Me.dtbDelivDT.ppYobiText = String.Empty         '納期(曜日)
                        Me.txtCompCD.ppText = String.Empty              '会社コード
                        Me.txtOfficCD.ppText = String.Empty             '営業所コード
                        Me.lblSendNmV.Text = String.Empty               '送付先名
                        Me.lblBranchV.Text = String.Empty               '営業所名
                        Me.ddlChargeV.Items.Clear()                     '担当者名
                        Me.ddlChargeV.Items.Add("")
                        Me.ddlChargeV.Width = 143
                        Me.lblZipnoV.Text = String.Empty                '郵便番号
                        Me.lblPrefV.Text = String.Empty                 '県コード
                        Me.lblAddrV.Text = String.Empty                 '住所
                        Me.lblTelV.Text = String.Empty                  'ＴＥＬ
                        Me.lblFaxV.Text = String.Empty                  'Ｆａｘ
                        Me.txtSPNotetext.ppText = String.Empty          '特記事項

                        Me.grvArtcltrnsDTL.DataSource = New Object() {}
                        Me.grvArtcltrnsDTL.DataBind()

                    Else
                        '該当データあり 項目に値セット

                        '取得したデータを設定
                        With dstArtcltrns.Tables(0).Rows(0)
                            '指示Ｎｏ
                            'Me.ddlBasedutyCD.ppSelectedValue = .Item("出荷種別").ToString
                            Me.txtRequestNo.ppText = .Item("依頼番号").ToString
                            Me.ttxCommNo.ppText = .Item("通知番号").ToString

                            '工事種別
                            Me.lblCnstClsV1.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.新規)
                            Me.lblCnstClsV2.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.増設)
                            Me.lblCnstClsV3.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.再配置)
                            Me.lblCnstClsV4.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.移設)
                            Me.lblCnstClsV5.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.一部撤去)
                            Me.lblCnstClsV6.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.全撤去)
                            Me.lblCnstClsV7.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.一時撤去)
                            Me.lblCnstClsV8.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.機種変更)
                            Me.lblCnstClsV9.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.構成配信)
                            Me.lblCnstClsV10.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.その他)
                            Me.lblCnstClsV11.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.ＶＵＰ)

                            Me.lblCnstNotetextV.Text = .Item("その他").ToString
                            Me.txtTboxID.ppText = .Item("ＴＢＯＸＩＤ").ToString
                            Me.lblHallNmV.Text = .Item("ホール名").ToString
                            Me.lblNlClsV.Text = .Item("ＮＬ区分").ToString
                            Me.dtbArtcltransD.ppText = .Item("依頼日").ToString
                            Me.dtbArtcltransD.ppYobiText = .Item("依頼日曜日").ToString
                            Me.dtbDelivDT.ppText = .Item("納期").ToString
                            Me.dtbDelivDT.ppYobiText = .Item("納期曜日").ToString
                            Me.lblTestDtV.Text = .Item("総合試験日").ToString

                            '送付先情報
                            Me.txtCompCD.ppText = .Item("送付先コード").ToString
                            Me.txtOfficCD.ppText = .Item("営業所コード").ToString
                            Me.lblSendNmV.Text = .Item("送付先名").ToString
                            Me.lblBranchV.Text = .Item("営業所名").ToString
                            '担当者名のリスト設定
                            If msSetCharge(.Item("送付先コード").ToString, .Item("営業所コード").ToString) Then
                            End If

                            Me.ddlChargeV.SelectedValue = .Item("担当者コード").ToString
                            Me.lblZipnoV.Text = .Item("郵便番号").ToString
                            Me.lblPrefV.Text = .Item("県コード").ToString
                            Me.lblAddrV.Text = .Item("住所").ToString
                            Me.lblTelV.Text = .Item("ＴＥＬ").ToString
                            Me.lblFaxV.Text = .Item("ＦＡＸ").ToString
                            Me.txtSPNotetext.ppText = .Item("特記事項").ToString
                            ViewState(M_VIEW_DELFLH) = .Item("削除フラグ").ToString
                            If .Item("削除フラグ").ToString <> "0" Then   '削除フラグが有効以外は活性状態変更
                                '活性／非活性設定
                                msSet_Mode(ViewState(P_SESSION_TERMS))
                            End If
                        End With

                        'CNSUPDP002-008 工事用の場合のみ  物品機材データ取得 
                        If ddlBasedutyCD.ppSelectedText.IndexOf("工事用") > 0 Then
                            If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録 Then
                                '登録モード
                                cmdArtcltrns = New SqlCommand("CNSUPDP002_S8", conDB)       '物品機材登録一覧データ取得
                                With cmdArtcltrns.Parameters                                'パラメータ設定
                                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, txtRequestNo.ppText))
                                End With

                                dstArtcltrns = clsDataConnect.pfGet_DataSet(cmdArtcltrns)   '物品機材登録一覧データ取得
                                grvArtcltrnsDTL.DataSource = dstArtcltrns                   '物品機材登録一覧データ設定
                                grvArtcltrnsDTL.DataBind()

                            Else
                                '更新モード
                                cmdArtcltrns = New SqlCommand("CNSUPDP002_S1", conDB)       '物品転送依頼取得
                                With cmdArtcltrns.Parameters                                'パラメータ設定
                                    .Add(pfSet_Param("artcl_no", SqlDbType.NVarChar, ViewState(P_KEY)))
                                End With

                                dstArtcltrns = clsDataConnect.pfGet_DataSet(cmdArtcltrns)   '物品転送依頼リストデータ取得

                                '物品転送番号と依頼番号の一致確認
                                If dstArtcltrns.Tables(0).Rows(0)("依頼番号").ToString = txtRequestNo.ppText Then
                                    '一致
                                    cmdArtcltrns = New SqlCommand("CNSUPDP002_S2", conDB)   '物品機材登録一覧
                                    With cmdArtcltrns.Parameters                            'パラメータ設定
                                        .Add(pfSet_Param("artcl_no", SqlDbType.NVarChar, ViewState(P_KEY)))
                                    End With

                                    dstArtcltrns = clsDataConnect.pfGet_DataSet(cmdArtcltrns)   '物品機材登録一覧データ取得
                                    grvArtcltrnsDTL.DataSource = dstArtcltrns                   '物品機材登録一覧データ設定
                                    grvArtcltrnsDTL.DataBind()
                                Else
                                    '不一致
                                    cmdArtcltrns = New SqlCommand("CNSUPDP002_S8", conDB)       '物品機材登録一覧データ取得
                                    With cmdArtcltrns.Parameters                                'パラメータ設定
                                        .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, txtRequestNo.ppText))
                                    End With

                                    dstArtcltrns = clsDataConnect.pfGet_DataSet(cmdArtcltrns)   '物品機材登録一覧データ取得
                                    grvArtcltrnsDTL.DataSource = dstArtcltrns                   '物品機材登録一覧データ設定
                                    grvArtcltrnsDTL.DataBind()
                                End If
                            End If
                        End If
                        'CNSUPDP002-008 END

                    End If

                Catch ex As Exception
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "依頼番号")
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Finally
                    'DB切断
                    clsDataConnect.pfClose_Database(conDB)
                    'Dispose
                    clsSqlDbSvr.psDisposeDataSet(dstArtcltrns)
                End Try
            End If
        End If

        'CNSUPDP002-002 END
    End Sub

    ''' <summary>
    ''' 会社コード変更時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtCompCD_TextChanged(sender As Object, e As EventArgs)

        '業者情報を取得
        If msSetTrader() Then
        Else
            If Me.txtCompCD.ppText <> String.Empty AndAlso Me.txtOfficCD.ppText <> String.Empty Then
                Me.txtOfficCD.psSet_ErrorNo("2009", txtCompCD.ppName, txtOfficCD.ppName)
            End If
        End If

        txtOfficCD.ppTextBox.Focus() '営業所コードへフォーカス設定

    End Sub

    ''' <summary>
    ''' 営業所コード変更時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtOfficCD_TextChanged(sender As Object, e As EventArgs)


        If Me.txtCompCD.ppText <> String.Empty AndAlso Me.txtOfficCD.ppText <> String.Empty Then

            '担当者名のリストを設定
            If msSetCharge(Me.txtCompCD.ppText, Me.txtOfficCD.ppText) Then
                '業者情報を取得
                If msSetTrader() Then
                Else
                    txtOfficCD.psSet_ErrorNo("2009", txtCompCD.ppName, txtOfficCD.ppName)
                End If
            Else
                txtOfficCD.psSet_ErrorNo("2008", txtCompCD.ppName, "業者マスタ")
            End If
        End If

        'CNSUPDP002-004
        'Me.txtOfficCD.ppTextBox.Focus()
        btnTrader.Focus() '業者情報へフォーカス設定
        'CNSUPDP002-004 END
    End Sub

    ''' <summary>
    ''' ＴＢＯＸＩＤ変更時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtTboxID_TextChanged(sender As Object, e As EventArgs)

        'ＴＢＯＸに紐づく情報取得
        msSetTbox(Me.txtTboxID.ppText)

        Me.txtTboxID.ppTextBox.Focus()

    End Sub

    ''' <summary>
    ''' 物品／部材コード変更時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtAppaCD_TextChanged(sender As Object, e As EventArgs)

        Dim dtsAppaData As DataSet = Nothing

        '物品／部材名取得
        Select Case Me.rblrdoRepuestClsV.SelectedValue
            Case M_REP_PAC
                If mfGet_Appa(Me.txtAppaCD.ppText, dtsAppaData) Then
                    Me.lblAppaNm.Text = dtsAppaData.Tables(0).Rows(0).Item("機器名").ToString
                Else
                    Me.lblAppaNm.Text = String.Empty
                End If
            Case M_REP_ART
                If mfGet_Packing(Me.txtAppaCD.ppText, dtsAppaData) Then
                    Me.lblAppaNm.Text = dtsAppaData.Tables(0).Rows(0).Item("商品名").ToString
                Else
                    Me.lblAppaNm.Text = String.Empty
                End If
            Case Else
                Me.lblAppaNm.Text = String.Empty
        End Select

        Me.txtAppaCD.ppTextBox.Focus()

    End Sub

    ''' <summary>
    ''' 物品／部材　追加ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        Dim dtsAppaData As DataSet = Nothing
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim strAppaNm As String

        objStack = New StackFrame

        'ログ出力開始
        psLogStart(Me)

        dtsAppaData = msCheck_ErrorDTL()

        If (Page.IsValid) Then  'エラーなし
            Select Case Me.rblrdoRepuestClsV.SelectedValue
                Case M_REP_PAC
                    strAppaNm = dtsAppaData.Tables(0).Rows(0).Item("機器名")
                Case M_REP_ART
                    strAppaNm = dtsAppaData.Tables(0).Rows(0).Item("商品名")
                Case Else
                    strAppaNm = String.Empty
            End Select
            Select Case ViewState(P_SESSION_TERMS)
                Case ClsComVer.E_遷移条件.登録, _
                     ClsComVer.E_遷移条件.更新 'CNSUPDP002-006 追加
                    If Not msAddSelectDtl(Me.txtAppaCD.ppText,
                                          strAppaNm,
                                          Me.txtQuantity.ppText,
                                          Me.ddlAppaCnds.ppSelectedTextOnly,
                                          Me.txtNotetext.ppText,
                                          Me.ddlAppaCnds.ppSelectedValue) Then
                        psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細")
                        Exit Sub
                    End If
                    '選択明細クリア
                    msClearSelect()

            End Select

        End If
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 物品／部材　変更ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpdate_Click1(sender As Object, e As EventArgs) Handles btnUpdate.Click

        Dim strSelRowSeqNo As String
        Dim dtsAppaData As DataSet = Nothing
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim strAppaNm As String     'CNSUPDP002-016

        'CNSUPDP002-006
        'Dim strArtcl_no As String
        'Dim intRtn As Integer
        'CNSUPDP002-006 END

        objStack = New StackFrame

        'ログ出力開始
        psLogStart(Me)

        dtsAppaData = msCheck_ErrorDTL()

        If (Page.IsValid) Then  'エラーなし
            '選択情報取得
            strSelRowSeqNo = TryCast(ViewState(M_VIEW_SEL), String)
            If strSelRowSeqNo Is Nothing Then   '選択行なし
                Exit Sub
            End If
            Select Case ViewState(P_SESSION_TERMS)
                Case ClsComVer.E_遷移条件.登録, _
                     ClsComVer.E_遷移条件.更新 'CNSUPDP002-006 追加

                    'CNSUPDP002-016
                    Select Case Me.rblrdoRepuestClsV.SelectedValue
                        Case M_REP_PAC
                            strAppaNm = dtsAppaData.Tables(0).Rows(0).Item("機器名")
                        Case M_REP_ART
                            strAppaNm = dtsAppaData.Tables(0).Rows(0).Item("商品名")
                        Case Else
                            strAppaNm = String.Empty
                    End Select

                    '更新処理
                    If Not msUpdSelectDtl(ViewState(M_VIEW_SEL),
                                          Me.txtAppaCD.ppText,
                                          strAppaNm,
                                          Me.txtQuantity.ppText,
                                          Me.ddlAppaCnds.ppSelectedTextOnly,
                                          Me.txtNotetext.ppText,
                                          Me.ddlAppaCnds.ppSelectedValue) Then
                        psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細")
                        Exit Sub
                    End If
                    '選択明細クリア
                    msClearSelect()

            End Select

        End If

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 物品／部材　削除ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing

        objStack = New StackFrame

        'ログ出力開始
        psLogStart(Me)
        Select Case ViewState(P_SESSION_TERMS)
            Case ClsComVer.E_遷移条件.登録, _
                 ClsComVer.E_遷移条件.更新 'CNSUPDP002-006 追加
                If Not msDelSelectDtl(ViewState(M_VIEW_SEL)) Then
                    psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細")
                End If
                msClearSelect() 'CNSUPDP002-006 追加選択明細クリア

        End Select

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 物品／部材　クリアボタン押下処理 'CNSUPDP002-017 
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnDtlClear_Click(sender As Object, e As EventArgs) Handles btnDtlClear.Click

        msClearSelect()

    End Sub

    ''' <summary>
    ''' 依頼区分変更
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub rblrdoRepuestClsV_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblrdoRepuestClsV.SelectedIndexChanged

        '明細部クリア
        msClearSelect()
        Me.grvArtcltrnsDTL.DataSource = New Object() {}
        Me.grvArtcltrnsDTL.DataBind()
        '通知番号クリア
        Me.ttxCommNo.ppText = String.Empty
        '工事種別クリア
        mfGet_CONSTClsValue("0", ClsComVer.E_工事種別.新規)
        mfGet_CONSTClsValue("0", ClsComVer.E_工事種別.増設)
        mfGet_CONSTClsValue("0", ClsComVer.E_工事種別.再配置)
        mfGet_CONSTClsValue("0", ClsComVer.E_工事種別.移設)
        mfGet_CONSTClsValue("0", ClsComVer.E_工事種別.一部撤去)
        mfGet_CONSTClsValue("0", ClsComVer.E_工事種別.全撤去)
        mfGet_CONSTClsValue("0", ClsComVer.E_工事種別.一時撤去)
        mfGet_CONSTClsValue("0", ClsComVer.E_工事種別.機種変更)
        mfGet_CONSTClsValue("0", ClsComVer.E_工事種別.構成配信)
        mfGet_CONSTClsValue("0", ClsComVer.E_工事種別.その他)
        mfGet_CONSTClsValue("0", ClsComVer.E_工事種別.ＶＵＰ)
        '総合試験日クリア
        Me.lblTestDtV.Text = String.Empty

        '活性／非活性設定
        msSet_Mode(ViewState(P_SESSION_TERMS))

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 工事種別の表示する値を返す。
    ''' </summary>
    ''' <param name="ipstrData">判別するデータ</param>
    ''' <param name="ipshtCount">判定する工事種別</param>
    ''' <returns>0以外:■, 他:□</returns>
    ''' <remarks></remarks>
    Private Function mfGet_CONSTClsValue(ByVal ipstrData As String, ByVal ipshtCount As ClsComVer.E_工事種別) As String

        If (ipstrData.Length >= ipshtCount) Then
            If (ipstrData.Substring(ipshtCount - 1, 1) <> "0") Then
                mfGet_CONSTClsValue = "■"
            Else
                mfGet_CONSTClsValue = "□"
            End If
        Else
            mfGet_CONSTClsValue = "□"
        End If

    End Function

    ''' <summary>
    ''' データ設定処理
    ''' </summary>
    ''' <param name="ipdstArtcltrns"></param>
    ''' <remarks></remarks>
    Private Sub msSet_Data_Artcltrns(ByVal ipdstArtcltrns As DataSet)

        objStack = New StackFrame

        Try
            '取得したデータを設定
            With ipdstArtcltrns.Tables(0).Rows(0)
                '指示Ｎｏ
                Me.msSetArtclNo(.Item("指示Ｎｏ").ToString)

                Me.lblChangeV.Text = .Item("変更回数").ToString
                Me.lblProcClsV.Text = .Item("処理区分").ToString
                If .Item("依頼区分").ToString = String.Empty Then
                    Me.rblrdoRepuestClsV.SelectedValue = "1"
                Else
                    Me.rblrdoRepuestClsV.SelectedValue = .Item("依頼区分").ToString
                End If
                Me.lblSendDTV.Text = .Item("送信日時").ToString
                If Me.lblSendDTV.Text = "" Then
                    ViewState(M_VIEW_UPDATE) = True
                End If
                Me.lblSendCNTV.Text = .Item("送信回数").ToString
                Me.ddlBasedutyCD.ppSelectedValue = .Item("出荷種別").ToString
                Me.txtRequestNo.ppText = .Item("依頼番号").ToString
                Me.ttxCommNo.ppText = .Item("通知番号").ToString

                '工事種別
                Me.lblCnstClsV1.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.新規)
                Me.lblCnstClsV2.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.増設)
                Me.lblCnstClsV3.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.再配置)
                Me.lblCnstClsV4.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.移設)
                Me.lblCnstClsV5.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.一部撤去)
                Me.lblCnstClsV6.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.全撤去)
                Me.lblCnstClsV7.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.一時撤去)
                Me.lblCnstClsV8.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.機種変更)
                Me.lblCnstClsV9.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.構成配信)
                Me.lblCnstClsV10.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.その他)
                Me.lblCnstClsV11.Text = mfGet_CONSTClsValue(.Item("工事種別").ToString, ClsComVer.E_工事種別.ＶＵＰ)

                Me.lblCnstNotetextV.Text = .Item("その他").ToString
                Me.txtTboxID.ppText = .Item("ＴＢＯＸＩＤ").ToString
                Me.lblHallNmV.Text = .Item("ホール名").ToString
                Me.lblNlClsV.Text = .Item("ＮＬ区分").ToString
                Me.dtbArtcltransD.ppText = .Item("依頼日").ToString
                Me.dtbArtcltransD.ppYobiText = .Item("依頼日曜日").ToString
                Me.dtbDelivDT.ppText = .Item("納期").ToString
                Me.dtbDelivDT.ppYobiText = .Item("納期曜日").ToString
                Me.lblTestDtV.Text = .Item("総合試験日").ToString

                '送付先情報
                Me.txtCompCD.ppText = .Item("送付先コード").ToString
                Me.txtOfficCD.ppText = .Item("営業所コード").ToString
                Me.lblSendNmV.Text = .Item("送付先名").ToString
                Me.lblBranchV.Text = .Item("営業所名").ToString

                '担当者名のリスト設定
                msSetCharge(.Item("送付先コード").ToString, .Item("営業所コード").ToString)

                'CNSUPDP002-009 削除社員対策
                If ddlChargeV.Items.FindByValue(.Item("担当者コード").ToString) Is Nothing Then
                    ddlChargeV.Items.Item(0).Value = .Item("担当者コード").ToString
                    ddlChargeV.Items.Item(0).Text = .Item("担当者コード").ToString & ":" & .Item("担当者").ToString
                    ddlChargeV.SelectedIndex = 0
                Else
                    Me.ddlChargeV.SelectedValue = .Item("担当者コード").ToString
                End If
                'CNSUPDP002-009　END

                Me.lblZipnoV.Text = .Item("郵便番号").ToString
                Me.lblPrefV.Text = .Item("県コード").ToString
                Me.lblAddrV.Text = .Item("住所").ToString
                Me.lblTelV.Text = .Item("ＴＥＬ").ToString
                Me.lblFaxV.Text = .Item("ＦＡＸ").ToString
                Me.txtSPNotetext.ppText = .Item("特記事項").ToString
                ViewState(M_VIEW_DELFLH) = .Item("削除フラグ").ToString

                If .Item("削除フラグ").ToString <> "0" Then   '削除フラグが有効以外は活性状態変更
                    '活性／非活性設定
                    msSet_Mode(ViewState(P_SESSION_TERMS))
                End If

            End With
        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

            Throw ex

        End Try
    End Sub

    ''' <summary>
    ''' 遷移条件によって活性／非活性を設定する。
    ''' </summary>
    ''' <param name="ipshtMode">遷移条件</param>
    ''' <remarks></remarks>
    Private Sub msSet_Mode(ByVal ipshtMode As ClsComVer.E_遷移条件)
        Const MES_NO_ADD = "00008"
        Const MES_NO_UPD = "00004"
        Select Case ipshtMode
            Case ClsComVer.E_遷移条件.参照
                '非活性設定
                Me.txtArtclNo1.ppEnabled = False            '指示Ｎｏ
                Me.pnlRepuestCls.Enabled = False            '依頼区分
                Me.ddlBasedutyCD.ppEnabled = False          '出荷種別
                Me.txtRequestNo.ppEnabled = False           '依頼番号
                Me.ttxCommNo.ppEnabled = False              '通知番号
                Me.txtTboxID.ppEnabled = False              'ＴＢＯＸＩＤ
                Me.dtbArtcltransD.ppEnabled = False         '依頼日
                Me.dtbDelivDT.ppEnabled = False             '納期
                Me.txtCompCD.ppEnabled = False              '会社コード
                Me.txtOfficCD.ppEnabled = False             '営業所コード
                Me.ddlChargeV.Enabled = False               '担当者名
                Me.txtSPNotetext.ppEnabled = False          '特記事項
                Me.txtAppaCD.ppEnabled = False              '物品／部材コード
                Me.txtQuantity.ppEnabled = False            '数量
                Me.ddlAppaCnds.ppEnabled = False            '物品状況
                Me.txtNotetext.ppEnabled = False            '備考
                Me.btnTrader.Enabled = False                '参照ボタン
                Me.btnAdd.Enabled = False                   '追加(明細)ボタン
                Me.btnUpdate.Enabled = False                '更新(明細)ボタン
                Me.btnDelete.Enabled = False                '削除(明細)ボタン

                'ボタン名設定
                Master.ppLeftButton1.Text = P_BTN_NM_UPD    '更新

                Master.ppLeftButton2.Visible = True         'キャンセルボタン
                Master.ppLeftButton1.Enabled = False        '更新ボタン_非活性
                Master.ppLeftButton2.Enabled = False        'キャンセルボタン_非活性
                Master.ppLeftButton3.Enabled = False        'クリアボタン_非活性
                Master.ppRigthButton2.Enabled = True        '印刷ボタン活性

            Case ClsComVer.E_遷移条件.更新

                If ViewState(M_VIEW_DELFLH) = 0 Then    '有効
                    Me.txtArtclNo1.ppEnabled = False            '指示Ｎｏ
                    Me.pnlRepuestCls.Enabled = False            '依頼区分
                    Me.txtRequestNo.ppEnabled = True            '依頼番号
                    Me.txtTboxID.ppEnabled = True               'ＴＢＯＸＩＤ
                    Me.dtbArtcltransD.ppEnabled = True          '依頼日
                    Me.dtbDelivDT.ppEnabled = True              '納期
                    Me.txtCompCD.ppEnabled = True               '会社コード
                    Me.txtOfficCD.ppEnabled = True              '営業所コード
                    Me.ddlChargeV.Enabled = True                '担当者名
                    Me.txtSPNotetext.ppEnabled = True           '特記事項

                    Me.grvArtcltrnsDTL.Enabled = True           '物品機材登録一覧
                    Me.txtAppaCD.ppEnabled = True               '物品／部材コード
                    Me.txtQuantity.ppEnabled = True             '数量
                    Me.ddlAppaCnds.ppEnabled = True             '物品状況
                    Me.txtNotetext.ppEnabled = True             '備考
                    Me.btnTrader.Enabled = True                 '参照ボタン
                    Me.btnAdd.Enabled = True                    '追加(明細)ボタン
                    Me.btnUpdate.Enabled = True                 '更新(明細)ボタン
                    Me.btnDelete.Enabled = True                 '削除(明細)ボタン

                    Select Case Me.rblrdoRepuestClsV.SelectedValue
                        Case "1"         '物品転送依頼
                            Me.ddlBasedutyCD.ppEnabled = True       '出荷種別
                            Me.ttxCommNo.ppEnabled = True           '通知番号
                            Me.txtAppaCD.ppMaxLength = 8            '物品／部材コード:入力桁数８桁
                        Case "2"         '梱包箱出荷
                            Me.ddlBasedutyCD.ppEnabled = False      '出荷種別
                            Me.ddlBasedutyCD.ppSelectedValue = "99" '出荷種別：99その他
                            Me.ttxCommNo.ppEnabled = False          '通知番号
                            Me.txtAppaCD.ppMaxLength = 11           '物品／部材コード:入力桁数１１桁
                    End Select


                    Master.ppLeftButton1.Enabled = True         '更新ボタン_活性
                    Master.ppLeftButton2.Enabled = True         'キャンセルボタン_活性
                    Master.ppLeftButton3.Enabled = True         'クリアボタン_活性
                    Master.ppRigthButton2.Enabled = True        '印刷ボタン活性
                    'CNSUPDP002-012
                    If ViewState(M_VIEW_SEL) = Nothing Then
                        msSet_DtlMode("1")
                    Else
                        msSet_DtlMode("2")
                    End If
                    'CNSUPDP002-012 END
                Else                                    '削除
                    '非活性設定
                    Me.txtArtclNo1.ppEnabled = False            '指示Ｎｏ
                    Me.pnlRepuestCls.Enabled = False            '依頼区分
                    Me.ddlBasedutyCD.ppEnabled = False          '出荷種別
                    Me.txtRequestNo.ppEnabled = False           '依頼番号
                    Me.ttxCommNo.ppEnabled = False              '通知番号
                    Me.txtTboxID.ppEnabled = False              'ＴＢＯＸＩＤ
                    Me.dtbArtcltransD.ppEnabled = False         '依頼日
                    Me.dtbDelivDT.ppEnabled = False             '納期
                    Me.txtCompCD.ppEnabled = False              '会社コード
                    Me.txtOfficCD.ppEnabled = False             '営業所コード
                    Me.ddlChargeV.Enabled = False               '担当者名
                    Me.txtSPNotetext.ppEnabled = False          '特記事項

                    Me.grvArtcltrnsDTL.Enabled = False          '物品機材登録一覧
                    Me.txtAppaCD.ppEnabled = False              '物品／部材コード
                    Me.txtQuantity.ppEnabled = False            '数量
                    Me.ddlAppaCnds.ppEnabled = False            '物品状況
                    Me.txtNotetext.ppEnabled = False            '備考
                    Me.btnTrader.Enabled = False                '参照ボタン
                    Me.btnAdd.Enabled = False                   '追加(明細)ボタン
                    Me.btnUpdate.Enabled = False                '更新(明細)ボタン
                    Me.btnDelete.Enabled = False                '削除(明細)ボタン

                    Master.ppLeftButton1.Enabled = False        '更新ボタン_非活性
                    Master.ppLeftButton2.Enabled = False        'キャンセルボタン_非活性
                    Master.ppLeftButton3.Enabled = False        'クリアボタン_非活性
                End If

                'ボタン名設定
                Master.ppLeftButton1.Text = P_BTN_NM_UPD

                Master.ppLeftButton2.Visible = True         'キャンセルボタン

                '確認処理付与(更新の確認,送信の確認)
                Master.ppLeftButton1.OnClientClick =
                    pfGet_OCClickMes(MES_NO_UPD, ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "物品転送依頼書")
            Case ClsComVer.E_遷移条件.登録
                Me.txtArtclNo1.ppEnabled = True             '指示Ｎｏ
                Me.pnlRepuestCls.Enabled = True             '依頼区分
                Me.txtRequestNo.ppEnabled = True            '依頼番号
                Me.txtTboxID.ppEnabled = True               'ＴＢＯＸＩＤ
                Me.dtbArtcltransD.ppEnabled = True          '依頼日
                Me.dtbDelivDT.ppEnabled = True              '納期
                Me.txtCompCD.ppEnabled = True               '会社コード
                Me.txtOfficCD.ppEnabled = True              '営業所コード
                Me.ddlChargeV.Enabled = True                '担当者名
                Me.txtSPNotetext.ppEnabled = True           '特記事項

                Me.grvArtcltrnsDTL.Enabled = True           '物品機材登録一覧
                Me.txtAppaCD.ppEnabled = True               '物品／部材コード
                Me.txtQuantity.ppEnabled = True             '数量
                Me.ddlAppaCnds.ppEnabled = True             '物品状況
                Me.txtNotetext.ppEnabled = True             '備考
                Me.btnTrader.Enabled = True                 '参照ボタン
                Me.btnAdd.Enabled = True                    '追加(明細)ボタン
                Me.btnUpdate.Enabled = True                 '更新(明細)ボタン
                Me.btnDelete.Enabled = True                 '削除(明細)ボタン
                'CNSUPDP002-012
                If ViewState(M_VIEW_SEL) = Nothing Then
                    msSet_DtlMode("1")
                Else
                    msSet_DtlMode("2")
                End If
                'CNSUPDP002-012 END
                Select Case Me.rblrdoRepuestClsV.SelectedValue
                    Case "1"         '物品転送依頼
                        Me.ddlBasedutyCD.ppEnabled = True           '出荷種別
                        Me.ttxCommNo.ppEnabled = True               '通知番号
                        Me.txtAppaCD.ppMaxLength = 8                '物品／部材コード:入力桁数８桁
                    Case "2"         '梱包箱出荷
                        Me.ddlBasedutyCD.ppEnabled = False          '出荷種別
                        Me.ddlBasedutyCD.ppSelectedValue = "99"     '出荷種別：99その他
                        Me.ttxCommNo.ppEnabled = False              '通知番号
                        Me.txtAppaCD.ppMaxLength = 11               '物品／部材コード:入力桁数１１桁
                End Select

                'ボタン名設定
                Master.ppLeftButton1.Text = P_BTN_NM_ADD            '登録

                Master.ppLeftButton2.Visible = False        'キャンセルボタン
                Master.ppLeftButton1.Enabled = True         '登録ボタン_活性
                Master.ppLeftButton2.Enabled = True         'キャンセルボタン_活性
                Master.ppLeftButton3.Enabled = True         'クリアボタン_活性
                Master.ppRigthButton2.Enabled = False       '印刷ボタン非活性

                '確認処理付与(更新の確認,送信の確認)
                Master.ppLeftButton1.OnClientClick =
                    pfGet_OCClickMes(MES_NO_ADD, ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "物品転送依頼書")

        End Select
    End Sub

    ''' <summary>
    ''' 物品機材登録一覧のボタン制御
    ''' </summary>
    ''' <param name="ipstrMode"></param>
    ''' <remarks></remarks>
    Private Sub msSet_DtlMode(ByVal ipstrMode As String)
        If ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.参照 Then
            Select Case ipstrMode

                Case "1"
                    Me.btnAdd.Enabled = True                    '追加(明細)ボタン
                    Me.btnUpdate.Enabled = False                '更新(明細)ボタン
                    Me.btnDelete.Enabled = False                '削除(明細)ボタン
                Case "2"
                    Me.btnAdd.Enabled = False                   '追加(明細)ボタン
                    Me.btnUpdate.Enabled = True                 '更新(明細)ボタン
                    Me.btnDelete.Enabled = True                 '削除(明細)ボタン
            End Select

        Else
            Me.btnAdd.Enabled = False                   '追加(明細)ボタン
            Me.btnUpdate.Enabled = False                 '更新(明細)ボタン
            Me.btnDelete.Enabled = False                 '削除(明細)ボタン
        End If

    End Sub

    ''' <summary>
    ''' 指示Ｎｏ設定
    ''' </summary>
    ''' <param name="ipstrArtclNo">設定する指示Ｎｏ</param>
    ''' <remarks></remarks>
    Private Sub msSetArtclNo(ByVal ipstrArtclNo As String)
        If ipstrArtclNo.Length >= 0 Then
            If ipstrArtclNo.Length >= 1 Then
                txtArtclNo1.ppText = ipstrArtclNo.Substring(0, 1)
            Else
                txtArtclNo1.ppText = ipstrArtclNo
            End If
        Else
            txtArtclNo1.ppText = String.Empty
        End If
        If ipstrArtclNo.Length >= 1 Then
            If ipstrArtclNo.Length >= 5 Then
                txtArtclNoT2.Text = ipstrArtclNo.Substring(1, 4)
            Else
                txtArtclNoT2.Text = ipstrArtclNo.Substring(1)
            End If
        Else
            txtArtclNoT2.Text = String.Empty
        End If
        If ipstrArtclNo.Length >= 5 Then
            If ipstrArtclNo.Length >= 7 Then
                txtArtclNoT3.Text = ipstrArtclNo.Substring(5, 2)
            Else
                txtArtclNoT3.Text = ipstrArtclNo.Substring(5)
            End If
        Else
            txtArtclNoT3.Text = String.Empty
        End If
        If ipstrArtclNo.Length >= 7 Then
            txtArtclNoT4.Text = ipstrArtclNo.Substring(7)
        Else
            txtArtclNoT4.Text = String.Empty
        End If
    End Sub

    ''' <summary>
    ''' 入力項目のエラーチェックを行う。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheck_Error()
        Dim dtrErrMes As DataRow

        '指示Ｎｏ（業務）チェック
        If Me.txtArtclNo1.ppEnabled _
            AndAlso Me.txtArtclNo1.ppText <> "G" And Me.txtArtclNo1.ppText <> "R" Then
            Me.txtArtclNo1.psSet_ErrorNo("4001", "指示Ｎｏ", "G（LAN）またはR（LAN機器以外）")
        End If

        '担当者コード必須チェック
        If Me.ddlChargeV.Enabled Then
            Dim strErrNo As String
            strErrNo = pfCheck_ListErr(ddlChargeV.SelectedValue, True)
            If strErrNo <> String.Empty Then
                'エラー
                dtrErrMes = pfGet_ValMes(strErrNo, "担当者")
                Me.cuvCharge.IsValid = False
                Me.cuvCharge.Text = dtrErrMes.Item(P_VALMES_SMES)
                Me.cuvCharge.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
            End If
        End If

        'TBOXIDは出荷種別が工事用または保守用の場合のみ必須
        If ddlBasedutyCD.ppSelectedText.IndexOf("工事用") > 0 Or ddlBasedutyCD.ppSelectedText.IndexOf("保守用") > 0 Then

            'TBOXIDチェック
            If Me.txtTboxID.ppText = String.Empty Then
                '未入力
                Me.txtTboxID.psSet_ErrorNo("5001", txtTboxID.ppName)
            End If

            'CNSUPDP002-003
            If ddlBasedutyCD.ppSelectedText.IndexOf("工事用") > 0 Then
                '形式チェック
                If txtRequestNo.ppText.Length <> 14 _
                OrElse txtRequestNo.ppText.Replace("N0090-", "").Replace("N0010-", "").Length <> 8 _
                OrElse Regex.IsMatch(txtRequestNo.ppText.Replace("N0090-", "").Replace("N0010-", ""), "^[0-9]+$") = False Then
                    '形式エラー
                    txtRequestNo.psSet_ErrorNo("4001", strBaseDuty, "正しい形式")
                    Exit Sub
                End If
                Dim conDB As SqlConnection = Nothing
                Dim cmdArtcltrns As SqlCommand = Nothing
                Dim dstArtcltrns As DataSet = Nothing
                Dim strFlg As String = ""
                '接続
                If clsDataConnect.pfOpen_Database(conDB) Then
                    Try
                        '工事
                        cmdArtcltrns = New SqlCommand("CNSUPDP002_S7", conDB)
                        With cmdArtcltrns.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, txtRequestNo.ppText))
                        End With

                        'データ取得
                        dstArtcltrns = clsDataConnect.pfGet_DataSet(cmdArtcltrns)

                        '依頼番号存在確認
                        If dstArtcltrns.Tables(0).Rows.Count = 0 Then
                            'エラー
                            txtRequestNo.psSet_ErrorNo("2002", strBaseDuty & " : " & txtRequestNo.ppText)
                        Else
                            '依頼番号-TBOXID 整合性
                            If txtTboxID.ppText <> dstArtcltrns.Tables(0).Rows(0)("ＴＢＯＸＩＤ").ToString Then
                                '整合性エラー
                                strFlg = "整合性エラー"
                            End If
                        End If
                        cmdArtcltrns = New SqlCommand("CMPSELP001_S4", conDB)
                        With cmdArtcltrns.Parameters
                            '--パラメータ設定 ＴＢＯＸＩＤ
                            .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxID.ppText))
                        End With

                        'データ取得
                        dstArtcltrns = clsDataConnect.pfGet_DataSet(cmdArtcltrns)

                        '存在チェック
                        If dstArtcltrns.Tables(0).Rows.Count = 0 Then
                            Me.txtTboxID.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
                        Else
                            If Me.txtTboxID.ppText = String.Empty Then
                                If strFlg = "整合性エラー" Then
                                    txtTboxID.psSet_ErrorNo("2012", strBaseDuty & "と" & txtTboxID.ppName)
                                End If
                            End If
                        End If

                    Catch ex As Exception
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事依頼書兼仕様書")
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    Finally
                        'DB切断
                        clsDataConnect.pfClose_Database(conDB)
                    End Try
                End If

            End If
            'CNSUPDP002-003 END

        End If

    End Sub

    ''' <summary>
    ''' 入力項目(物品／部材)のエラーチェックを行う。
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msCheck_ErrorDTL() As DataSet
        Dim dtsAppaData As DataSet = Nothing
        Dim intQuantity As Integer

        '数量チェック
        Integer.TryParse(Me.txtQuantity.ppText, intQuantity)
        If intQuantity < 1 Then
            Me.txtQuantity.psSet_ErrorNo("4001", "数量", "1以上")
            Me.txtQuantity.ppTextBox.Focus()
        End If

        '物品／部材名取得
        Select Case Me.rblrdoRepuestClsV.SelectedValue
            Case M_REP_PAC
                If Not mfGet_Appa(Me.txtAppaCD.ppText, dtsAppaData) Then    '整合性エラー
                    Me.txtAppaCD.psSet_ErrorNo("2002", "物品／部材コード")
                    Me.txtAppaCD.ppTextBox.Focus()
                End If
            Case M_REP_ART
                If Not mfGet_Packing(Me.txtAppaCD.ppText, dtsAppaData) Then '整合性エラー
                    Me.txtAppaCD.psSet_ErrorNo("2002", "物品／部材コード")
                    Me.txtAppaCD.ppTextBox.Focus()
                End If
            Case Else
                Me.txtAppaCD.psSet_ErrorNo("2002", "物品／部材コード")
                Me.txtAppaCD.ppTextBox.Focus()
        End Select

        msCheck_ErrorDTL = dtsAppaData
    End Function

    ''' <summary>
    ''' 会社コード、営業所コードに対応する業者を設定する。
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msSetTrader() As Boolean

        Dim dtsTRaderData As DataSet = Nothing
        '業者情報取得
        If mfGet_Trader(Me.txtCompCD.ppText,
                        Me.txtOfficCD.ppText,
                        dtsTRaderData) Then
            '取得したデータを設定
            With dtsTRaderData.Tables(0).Rows(0)
                If CType(Me.txtCompCD.ppText, Integer) > 0 And CType(Me.txtCompCD.ppText, Integer) < 11 Then
                    Me.lblSendNmV.Text = "NTTデータカスタマサービス株式会社"
                Else
                    Me.lblSendNmV.Text = .Item("送付先名").ToString
                End If
                Me.lblBranchV.Text = .Item("営業所名").ToString
                Me.lblZipnoV.Text = .Item("郵便番号").ToString
                Me.lblPrefV.Text = .Item("県コード").ToString
                Me.lblAddrV.Text = .Item("住所").ToString
                Me.lblTelV.Text = .Item("ＴＥＬ").ToString
                Me.lblFaxV.Text = .Item("ＦＡＸ").ToString
            End With
            msSetTrader = True
        Else
            Me.lblSendNmV.Text = String.Empty
            Me.lblBranchV.Text = String.Empty
            Me.lblZipnoV.Text = String.Empty
            Me.lblPrefV.Text = String.Empty
            Me.lblAddrV.Text = String.Empty
            Me.lblTelV.Text = String.Empty
            Me.lblFaxV.Text = String.Empty
            msSetTrader = False
        End If
    End Function

    ''' <summary>
    ''' 業者情報取得
    ''' </summary>
    ''' <param name="ipstrCompCD">会社コード</param>
    ''' <param name="ipstrOfficeCD">営業所コード</param>
    ''' <param name="opdsTrader">業者情報</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_Trader(ByVal ipstrCompCD As String,
                                  ByVal ipstrOfficeCD As String,
                                  ByRef opdsTrader As DataSet)

        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim strOKNG As String

        objStack = New StackFrame

        '初期化
        conDB = Nothing
        strOKNG = String.Empty
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("CNSUPDP002_S6", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("comp_cd", SqlDbType.NVarChar, ipstrCompCD))
                    .Add(pfSet_Param("office_cd", SqlDbType.NVarChar, ipstrOfficeCD))
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                End With

                'データ取得
                opdsTrader = clsDataConnect.pfGet_DataSet(cmdDB)

                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "1"
                        '整合性OK
                        mfGet_Trader = True
                    Case Else
                        '整合性エラー
                        mfGet_Trader = False
                End Select

            Catch ex As Exception

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")

                mfGet_Trader = False

            Finally
                'DB切断
                clsDataConnect.pfClose_Database(conDB)
            End Try
        Else
            mfGet_Trader = False
        End If
    End Function

    ''' <summary>
    ''' 会社コード、営業所コードに対応する担当者名のリストを設定する。
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msSetCharge(ByVal ipstrCompCD As String, ByVal ipstrOfficeCD As String) As Boolean

        Dim dtsChargeData As DataSet = Nothing

        '担当者名のリスト設定
        If pfGet_EmployeeList(ipstrCompCD, ipstrOfficeCD, dtsChargeData) Then
            Me.ddlChargeV.DataSource = dtsChargeData
            Me.ddlChargeV.DataTextField = "表示名"
            Me.ddlChargeV.DataValueField = "社員コード"
            Me.ddlChargeV.DataBind()
            msSetCharge = True
            Me.ddlChargeV.Width = Nothing     'CNSUPDP002-001
            Me.ddlChargeV.Items.Insert(0, "") 'CNSUPDP002-001
        Else
            Me.ddlChargeV.Items.Clear()
            msSetCharge = False
            Me.ddlChargeV.Items.Add("")   'CNSUPDP002-001
            Me.ddlChargeV.Width = 143     'CNSUPDP002-001
        End If

    End Function

    ''' <summary>
    ''' ＴＢＯＸＩＤに対応するホール名、ＮＬ区分を設定する。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetTbox(ByVal ipstrCompCD As String)
        Dim dtsTboxData As DataSet = Nothing
        '担当者名のリスト設定
        If mfGet_Tbox(ipstrCompCD, dtsTboxData) Then
            '取得したデータを設定
            With dtsTboxData.Tables(0).Rows(0)
                Me.lblHallNmV.Text = .Item("ホール名").ToString
                Me.lblNlClsV.Text = .Item("ＮＬ区分").ToString
            End With
        Else
            Me.lblHallNmV.Text = String.Empty
            Me.lblNlClsV.Text = String.Empty
        End If
    End Sub

    ''' <summary>
    ''' ＴＢＯＸ情報取得
    ''' </summary>
    ''' <param name="ipstrTboxID">ＴＢＯＸＩＤ</param>
    ''' <param name="opdsTrader">ＴＢＯＸ情報</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_Tbox(ByVal ipstrTboxID As String,
                                ByRef opdsTrader As DataSet)

        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim strOKNG As String

        objStack = New StackFrame

        '初期化
        conDB = Nothing
        strOKNG = String.Empty
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("CNSUPDP002_S10", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, ipstrTboxID))
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                End With

                'データ取得
                opdsTrader = clsDataConnect.pfGet_DataSet(cmdDB)

                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "1"
                        '整合性OK
                        mfGet_Tbox = True
                    Case Else
                        '整合性エラー
                        mfGet_Tbox = False
                End Select

            Catch ex As Exception

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")

                mfGet_Tbox = False

            Finally
                'DB切断
                clsDataConnect.pfClose_Database(conDB)
            End Try
        Else
            mfGet_Tbox = False
        End If

    End Function

    ''' <summary>
    ''' クリア処理
    ''' </summary>
    ''' <param name="ipshtMode">遷移条件</param>
    ''' <param name="ipstrOldDisp">遷移元ＩＤ</param>
    ''' <param name="ipstrKeyCode">詳細検索キー</param>
    ''' <remarks></remarks>
    Private Sub msClearScreen(ByVal ipshtMode As ClsComVer.E_遷移条件,
                              ByRef ipstrOldDisp As String,
                              ByRef ipstrKeyCode As String,
                              Optional ipstrSeachMode As String = "0")

        Dim conDB As SqlConnection = Nothing
        Dim cmdArtcltrns As SqlCommand = Nothing
        Dim dstArtcltrns As DataSet = Nothing
        Dim cmdArtcltrnsDTL As SqlCommand = Nothing
        Dim dstArtcltrnsDTL As DataSet = Nothing
        Dim strArtcl_no As String = String.Empty

        objStack = New StackFrame

        '明細選択行クリア
        msClearSelect()

        '画面クリア処理
        Select Case ipshtMode
            Case ClsComVer.E_遷移条件.登録
                Select Case ipstrOldDisp
                    Case P_FUN_CNS & P_SCR_UPD & P_PAGE & "001"     '工事依頼書兼仕様書
                        '接続
                        If clsDataConnect.pfOpen_Database(conDB) Then
                            Try
                                '物品転送依頼
                                cmdArtcltrns = New SqlCommand("CNSUPDP002_S7", conDB)
                                With cmdArtcltrns.Parameters
                                    'パラメータ設定
                                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrKeyCode))
                                End With

                                '物品転送依頼リストデータ取得
                                dstArtcltrns = clsDataConnect.pfGet_DataSet(cmdArtcltrns)

                                '物品機材登録一覧
                                cmdArtcltrnsDTL = New SqlCommand("CNSUPDP002_S8", conDB)
                                With cmdArtcltrnsDTL.Parameters
                                    'パラメータ設定
                                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrKeyCode))
                                End With

                                '物品機材登録一覧データ取得
                                dstArtcltrnsDTL = clsDataConnect.pfGet_DataSet(cmdArtcltrnsDTL)

                                '取得データ設定
                                If ipstrSeachMode = "0" Then
                                    msSet_Data_Artcltrns(dstArtcltrns)
                                End If

                                grvArtcltrnsDTL.DataSource = dstArtcltrnsDTL
                                grvArtcltrnsDTL.DataBind()

                                ddlBasedutyCD.ppSelectedValue = "01"

                                Me.dtbArtcltransD.ppText = DateTime.Now.ToString("yyyy/MM/dd")              '依頼日
                                Dim US As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-US")
                                Me.dtbArtcltransD.ppYobiText = DateTime.Now.ToString("ddd", US).ToUpper     '依頼日（曜日）

                            Catch ex As Exception
                                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事依頼書兼仕様書")

                                'ログ出力
                                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                            Finally
                                'DB切断
                                clsDataConnect.pfClose_Database(conDB)
                            End Try
                        Else
                            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        End If

                    Case P_FUN_EQU & P_SCR_LST & P_PAGE & "001"     '配送機器一覧
                        '接続
                        If clsDataConnect.pfOpen_Database(conDB) Then
                            Try
                                '物品転送依頼
                                cmdArtcltrns = New SqlCommand("CNSUPDP002_S9", conDB)
                                With cmdArtcltrns.Parameters
                                    'パラメータ設定
                                    .Add(pfSet_Param("trouble_no", SqlDbType.NVarChar, ipstrKeyCode))
                                End With

                                '物品転送依頼リストデータ取得
                                dstArtcltrns = clsDataConnect.pfGet_DataSet(cmdArtcltrns)

                                If dstArtcltrns Is Nothing OrElse dstArtcltrns.Tables(0).Rows.Count = 0 Then
                                    psMesBox(Me, "20006", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                                    Exit Sub
                                End If

                                '取得データ設定
                                If ipstrSeachMode = "0" Then
                                    msSet_Data_Artcltrns(dstArtcltrns)
                                End If

                                grvArtcltrnsDTL.DataSource = New Object() {}
                                grvArtcltrnsDTL.DataBind()

                                If ipstrKeyCode.IndexOf("N0010-") >= 0 OrElse ipstrKeyCode.IndexOf("N0090-") >= 0 Then
                                    ddlBasedutyCD.ppSelectedValue = "01"
                                Else
                                    ddlBasedutyCD.ppSelectedValue = "03"
                                End If

                                Me.dtbArtcltransD.ppText = DateTime.Now.ToString("yyyy/MM/dd")              '依頼日
                                Dim US As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-US")
                                Me.dtbArtcltransD.ppYobiText = DateTime.Now.ToString("ddd", US).ToUpper     '依頼日（曜日）

                            Catch ex As Exception

                                If ipstrKeyCode.IndexOf("N0010-") >= 0 OrElse ipstrKeyCode.IndexOf("N0090-") >= 0 Then
                                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事依頼書兼仕様書")
                                Else
                                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守対応依頼書")
                                End If

                                'ログ出力
                                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                            Finally
                                'DB切断
                                clsDataConnect.pfClose_Database(conDB)
                            End Try
                        Else
                            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        End If

                    Case Else
                        Me.msSetArtclNo(String.Empty)                   '指示Ｎｏ．

                        Me.lblChangeV.Text = "0"                        '変更回数
                        Me.lblProcClsV.Text = "新規"                    '処理区分
                        Me.lblSendDTV.Text = String.Empty               '送信日時
                        Me.lblSendCNTV.Text = "0"                       '送信回数
                        Me.rblrdoRepuestClsV.SelectedIndex = Nothing    '依頼区分
                        Me.ddlBasedutyCD.ppSelectedValue = Nothing      '出荷種別
                        Me.txtRequestNo.ppText = String.Empty           '依頼番号
                        Me.ttxCommNo.ppText = String.Empty              '通知番号
                        Me.txtTboxID.ppText = String.Empty              'ＴＢＯＸＩＤ
                        Me.lblHallNmV.Text = String.Empty               'ホール名
                        Me.lblNlClsV.Text = String.Empty                'ＮＬ区分
                        Me.dtbArtcltransD.ppText = DateTime.Now.ToString("yyyy/MM/dd")
                        Dim US As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-US")
                        Me.dtbArtcltransD.ppYobiText = DateTime.Now.ToString("ddd", US).ToUpper
                        Me.dtbDelivDT.ppText = String.Empty             '納期
                        Me.dtbDelivDT.ppYobiText = String.Empty         '納期(曜日)
                        Me.txtCompCD.ppText = String.Empty              '会社コード
                        Me.txtOfficCD.ppText = String.Empty             '営業所コード
                        Me.lblSendNmV.Text = String.Empty               '送付先名
                        Me.lblBranchV.Text = String.Empty               '営業所名
                        Me.ddlChargeV.Items.Clear()                     '担当者名
                        Me.lblZipnoV.Text = String.Empty                '郵便番号
                        Me.lblPrefV.Text = String.Empty                 '県コード
                        Me.lblAddrV.Text = String.Empty                 '住所
                        Me.lblTelV.Text = String.Empty                  'ＴＥＬ
                        Me.lblFaxV.Text = String.Empty                  'Ｆａｘ
                        Me.txtSPNotetext.ppText = String.Empty          '特記事項

                        'CNSUPDP002-011　工事種別
                        Me.lblCnstClsV1.Text = "□"
                        Me.lblCnstClsV2.Text = "□"
                        Me.lblCnstClsV3.Text = "□"
                        Me.lblCnstClsV4.Text = "□"
                        Me.lblCnstClsV5.Text = "□"
                        Me.lblCnstClsV6.Text = "□"
                        Me.lblCnstClsV7.Text = "□"
                        Me.lblCnstClsV8.Text = "□"
                        Me.lblCnstClsV9.Text = "□"
                        Me.lblCnstClsV10.Text = "□"
                        Me.lblCnstClsV11.Text = "□"
                        'CNSUPDP002-011

                        Me.grvArtcltrnsDTL.DataSource = New Object() {}
                        Me.grvArtcltrnsDTL.DataBind()

                End Select
            Case Else

                '指示Ｎｏ
                strArtcl_no = txtArtclNo1.ppText & txtArtclNoT2.Text & txtArtclNoT3.Text & txtArtclNoT4.Text

                '接続
                If clsDataConnect.pfOpen_Database(conDB) Then
                    Try
                        '物品転送依頼
                        cmdArtcltrns = New SqlCommand("CNSUPDP002_S1", conDB)
                        With cmdArtcltrns.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("artcl_no", SqlDbType.NVarChar, strArtcl_no))
                        End With

                        '物品転送依頼リストデータ取得
                        dstArtcltrns = clsDataConnect.pfGet_DataSet(cmdArtcltrns)


                        '物品機材登録一覧
                        cmdArtcltrnsDTL = New SqlCommand("CNSUPDP002_S2", conDB)
                        With cmdArtcltrnsDTL.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("artcl_no",
                                                        SqlDbType.NVarChar,
                                                        ViewState(P_KEY)))
                        End With

                        '物品機材登録一覧データ取得
                        dstArtcltrnsDTL = clsDataConnect.pfGet_DataSet(cmdArtcltrnsDTL)

                        '取得データ設定
                        If ipstrSeachMode = "0" Then
                            msSet_Data_Artcltrns(dstArtcltrns)

                        End If

                        grvArtcltrnsDTL.DataSource = dstArtcltrnsDTL
                        grvArtcltrnsDTL.DataBind()
                        ViewState("ArtcltrnsDTL") = pfParse_DataTable(Me.grvArtcltrnsDTL) 'CNSUPDP002-012

                    Catch ex As Exception

                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "物品転送依頼書")

                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    Finally
                        'DB切断
                        If Not clsDataConnect.pfClose_Database(conDB) Then
                            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        End If
                    End Try
                Else
                    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
        End Select
    End Sub

    ''' <summary>
    ''' 明細選択行クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearSelect()

        '★排他情報削除
        If Not Me.Master.ppExclusiveDateDtl = String.Empty Then

            If clsExc.pfDel_Exclusive(Me _
                               , Session(P_SESSION_SESSTION_ID) _
                               , Me.Master.ppExclusiveDateDtl) = 0 Then
                Me.Master.ppExclusiveDateDtl = String.Empty
            Else
                Exit Sub
            End If
        End If

        Me.txtAppaCD.ppText = String.Empty
        Me.lblAppaNm.Text = String.Empty
        Me.txtQuantity.ppText = String.Empty
        Me.ddlAppaCnds.ppDropDownList.SelectedIndex = 0
        Me.txtNotetext.ppText = String.Empty
        'CNSUPDP002-012
        msSet_DtlMode("1")
        'CNSUPDP002-012 END

        ViewState(M_VIEW_SEL) = Nothing
        ViewState(M_VIEW_SEL_DIPS_SEQ) = Nothing

    End Sub

    ''' <summary>
    ''' 機器名取得
    ''' </summary>
    ''' <param name="ipstrCode">機器コード</param>
    ''' <param name="opdstData">機器名</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_Appa(ByVal ipstrCode As String,
                                ByRef opdstData As DataSet)
        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim strOKNG As String

        objStack = New StackFrame

        '初期化
        conDB = Nothing
        strOKNG = Nothing
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("ZCMPSEL007", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("appa_cd", SqlDbType.NVarChar, ipstrCode))
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                End With

                'データ取得
                opdstData = clsDataConnect.pfGet_DataSet(cmdDB)

                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "1"
                        '整合性OK
                        mfGet_Appa = True
                    Case Else
                        '整合性エラー
                        mfGet_Appa = False
                End Select
            Catch ex As Exception

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")

                mfGet_Appa = False

            Finally

                'DB切断
                clsDataConnect.pfClose_Database(conDB)

            End Try
        Else
            mfGet_Appa = False
        End If
    End Function

    ''' <summary>
    ''' 梱包財名取得
    ''' </summary>
    ''' <param name="ipstrCode">商品コード</param>
    ''' <param name="opdstData">商品名</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_Packing(ByVal ipstrCode As String,
                                   ByRef opdstData As DataSet)
        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim strOKNG As String

        objStack = New StackFrame

        '初期化
        conDB = Nothing
        strOKNG = Nothing
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("ZCMPSEL037", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prod_cd", SqlDbType.NVarChar, ipstrCode))
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                End With

                'データ取得
                opdstData = clsDataConnect.pfGet_DataSet(cmdDB)

                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "1"
                        '整合性OK
                        mfGet_Packing = True
                    Case Else
                        '整合性エラー
                        mfGet_Packing = False
                End Select
            Catch ex As Exception

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")

                mfGet_Packing = False
            Finally
                'DB切断
                clsDataConnect.pfClose_Database(conDB)
            End Try
        Else
            mfGet_Packing = False
        End If
    End Function

    ''' <summary>
    ''' 新規登録の引数の連番を持つ行を更新
    ''' </summary>
    ''' <param name="ipstrSeqNo">更新行の連番</param>
    ''' <param name="ipstrAppaCD">物品／部材コード</param>
    ''' <param name="ipstrAppaNM">物品／部材名</param>
    ''' <param name="ipdectQuantity">数量</param>
    ''' <param name="ipstrAppaCndsNM">物品状況</param>
    ''' <param name="ipstrNotetext">備考</param>
    ''' <param name="ipstrAppaCndsCD">物品状況コード</param>
    ''' <returns>更新成功：True, 更新失敗/更新行なし：False</returns>
    ''' <remarks></remarks>
    Private Function msUpdSelectDtl(ByVal ipstrSeqNo As String,
                                    ByVal ipstrAppaCD As String,
                                    ByVal ipstrAppaNM As String,
                                    ByVal ipdectQuantity As Decimal,
                                    ByVal ipstrAppaCndsNM As String,
                                    ByVal ipstrNotetext As String,
                                    ByVal ipstrAppaCndsCD As String) As Boolean
        Dim dttData As DataTable = Nothing

        objStack = New StackFrame

        msUpdSelectDtl = False
        Try
            'GridViewから取得
            dttData = pfParse_DataTable(Me.grvArtcltrnsDTL)

            '該当行更新
            'For Each rowSel As DataRow In dttData.Select("連番 = " & ipstrSeqNo)　'CNSUPDP002-012
            For Each rowSel As DataRow In dttData.Select("Ｎｏ = '" & ipstrSeqNo & "'")

                '物品／部材コード（機器コード）が変わった場合、表示順を変更
                If rowSel.Item("物品／部材コード") <> ipstrAppaCD Then
                    rowSel.Item("物品／部材コード") = ipstrAppaCD
                    'rowSel.Item("表示順") = 99 'CNSUPDP002-012
                    rowSel.Item("表示順") = Integer.Parse(dttData.Rows(dttData.Rows.Count - 1)("表示順")) + 1
                End If
                rowSel.Item("物品／部材名") = ipstrAppaNM
                rowSel.Item("数量") = ipdectQuantity
                rowSel.Item("物品状況") = ipstrAppaCndsNM
                rowSel.Item("備考") = ipstrNotetext
                rowSel.Item("物品状況コード") = ipstrAppaCndsCD
                msUpdSelectDtl = True

                ''
                'Dim rowdt As DataRow = dttData.NewRow
                'rowdt.Item("物品／部材コード") = ipstrAppaCD
                'rowdt.Item("物品／部材名") = ipstrAppaNM
                'rowdt.Item("数量") = ipdectQuantity
                'rowdt.Item("物品状況") = ipstrAppaCndsNM
                'rowdt.Item("備考") = ipstrNotetext
                'rowdt.Item("物品状況コード") = ipstrAppaCndsCD

                'ソート
                dttData.DefaultView.Sort = "表示順,Ｎｏ ASC" 'CNSUPDP002-012
                'dttData.DefaultView.Sort = "Ｎｏ ASC"
                'msSetDataNo(dttData) 'CNSUPDP002-012
            Next

            'データ再設定
            Me.grvArtcltrnsDTL.DataSource = dttData
            Me.grvArtcltrnsDTL.DataBind()
        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

            msUpdSelectDtl = False

        End Try

    End Function

    ''' <summary>
    ''' 新規登録の明細行を追加
    ''' </summary>
    ''' <param name="ipstrAppaCD">物品／部材コード</param>
    ''' <param name="ipstrAppaNM">物品／部材名</param>
    ''' <param name="ipdectQuantity">数量</param>
    ''' <param name="ipstrAppaCndsNM">物品状況</param>
    ''' <param name="ipstrNotetext">備考</param>
    ''' <param name="ipstrAppaCndsCD">物品状況コード</param>
    ''' <returns>更新成功：True, 更新失敗：False</returns>
    ''' <remarks></remarks>
    Private Function msAddSelectDtl(ByVal ipstrAppaCD As String,
                                    ByVal ipstrAppaNM As String,
                                    ByVal ipdectQuantity As Decimal,
                                    ByVal ipstrAppaCndsNM As String,
                                    ByVal ipstrNotetext As String,
                                    ByVal ipstrAppaCndsCD As String) As Boolean
        Dim dttData As DataTable = Nothing
        Dim dtrNew As DataRow = Nothing
        Dim intSeq As Integer = Nothing
        Dim intNo As Integer = Nothing

        Dim strNo As String = Nothing
        Dim strSeq As String = Nothing

        objStack = New StackFrame

        Try
            'GridViewから取得
            dttData = pfParse_DataTable(Me.grvArtcltrnsDTL)

            '新規行設定
            dtrNew = dttData.NewRow()
            dtrNew.Item("物品／部材コード") = ipstrAppaCD
            dtrNew.Item("物品／部材名") = ipstrAppaNM
            dtrNew.Item("数量") = ipdectQuantity
            dtrNew.Item("物品状況") = ipstrAppaCndsNM
            dtrNew.Item("備考") = ipstrNotetext
            dtrNew.Item("物品状況コード") = ipstrAppaCndsCD
            'dtrNew.Item("表示順") = 99
            If dttData.Rows.Count = 0 Then
                dtrNew.Item("表示順") = 0
            Else
                dtrNew.Item("表示順") = Integer.Parse(dttData.Rows(dttData.Rows.Count - 1)("表示順")) + 1
            End If
            dtrNew.Item("Ｎｏ") = dttData.Rows.Count + 1


            '行追加
            dttData.Rows.Add(dtrNew)

            'ソート
            dttData.DefaultView.Sort = "表示順,Ｎｏ ASC"
            'msSetDataNo(dttData) 'CNSUPDP002-012


            'データ再設定
            Me.grvArtcltrnsDTL.DataSource = dttData
            Me.grvArtcltrnsDTL.DataBind()

            msAddSelectDtl = True
        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

            msAddSelectDtl = False

        End Try
    End Function

    ''' <summary>
    ''' 新規登録の引数の連番を持つ行を削除
    ''' </summary>
    ''' <param name="ipstrSeqNo">更新行の連番</param>
    ''' <returns>更新成功：True, 更新失敗/更新行なし：False</returns>
    ''' <remarks></remarks>
    Private Function msDelSelectDtl(ByVal ipstrSeqNo As String) As Boolean
        Dim dttData As DataTable = Nothing

        objStack = New StackFrame

        msDelSelectDtl = False
        Try
            'GridViewから取得
            dttData = pfParse_DataTable(Me.grvArtcltrnsDTL)

            '該当行更新
            'For Each rowSel As DataRow In dttData.Select("連番 = '" & ipstrSeqNo & "'")　'CNSUPDP002-012
            For Each rowSel As DataRow In dttData.Select("Ｎｏ = '" & ipstrSeqNo & "'")
                rowSel.Delete()
                msDelSelectDtl = True

                'ソート
                'dttData.DefaultView.Sort = "表示順,Ｎｏ ASC"
                'msSetDataNo(dttData) 'CNSUPDP002-012
                For Each rowData As DataRow In dttData.Rows
                    If rowData.Item("Ｎｏ") > Integer.Parse(ipstrSeqNo) Then
                        rowData.Item("Ｎｏ") -= 1
                    End If
                Next
                'CNSUPDP002-012 END
            Next

            'データ再設定
            Me.grvArtcltrnsDTL.DataSource = dttData
            Me.grvArtcltrnsDTL.DataBind()
        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

            msDelSelectDtl = False

        End Try

    End Function

    ''' <summary>
    ''' 新規登録の表示番号と連番を採番
    ''' </summary>
    ''' <param name="cpdttData">採番するテーブル</param>
    ''' <remarks></remarks>
    Private Sub msSetDataNo(ByRef cpdttData As DataTable)
        Dim zz As Integer = 1
        For Each rowData As DataRow In cpdttData.Rows
            rowData.Item("Ｎｏ") = zz
            'rowData.Item("連番") = zz 'CNSUPDP002-012
            zz = zz + 1
        Next
    End Sub

    ''' <summary>
    ''' 明細行更新処理
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ</param>
    ''' <param name="iptrnDB">トランザクション</param>
    ''' <param name="ipstrProcCls">処理区分（1:新規登録、2:更新）</param>
    ''' <param name="ipstrArtclNo">物品転送管理番号</param>
    ''' <param name="ipdecSeqNo">連番</param>
    ''' <param name="ipstrDispSeq">表示順</param>
    ''' <param name="ipstrRepuestCls">依頼区分</param>
    ''' <param name="ipstrAppaCd">機器コード</param>
    ''' <param name="ipstrAppaNm">機器名</param>
    ''' <param name="ipstrAppaCnds">状況</param>
    ''' <param name="ipdecQuantity">数量</param>
    ''' <param name="ipstrNotetext">備考</param>
    ''' <returns>ＳＱＬ実行結果</returns>
    ''' <remarks></remarks>
    Private Function mfUpdateDTL(ByVal ipconDB As SqlConnection,
                                 ByVal iptrnDB As SqlTransaction,
                                 ByVal ipstrProcCls As String,
                                 ByVal ipstrArtclNo As String,
                                 ByVal ipdecSeqNo As Decimal,
                                 ByVal ipstrRepuestCls As String,
                                 ByVal ipstrDispSeq As String,
                                 ByVal ipstrDispNo As String,
                                 ByVal ipstrAppaCd As String,
                                 ByVal ipstrAppaNm As String,
                                 ByVal ipstrAppaCnds As String,
                                 ByVal ipdecQuantity As Decimal,
                                 ByVal ipstrNotetext As String) As Integer
        Dim cmdDB As SqlCommand

        objStack = New StackFrame

        Try
            cmdDB = New SqlCommand("CNSUPDP002_U2", ipconDB)
            cmdDB.Transaction = iptrnDB
            With cmdDB.Parameters
                .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, ipstrProcCls))         '処理区分
                .Add(pfSet_Param("artcl_no", SqlDbType.NVarChar, ipstrArtclNo))         '物品転送管理番号
                .Add(pfSet_Param("repuest_cls", SqlDbType.NVarChar, ipstrRepuestCls))   '依頼区分
                .Add(pfSet_Param("seqno", SqlDbType.Decimal, ipdecSeqNo))               '連番
                .Add(pfSet_Param("disp_seq", SqlDbType.NVarChar, ipstrDispSeq))         '表示順
                .Add(pfSet_Param("disp_no", SqlDbType.NVarChar, ipstrDispNo))          '表示No
                .Add(pfSet_Param("appa_cd", SqlDbType.NVarChar, ipstrAppaCd))           '機器コード
                .Add(pfSet_Param("appa_nm", SqlDbType.NVarChar, ipstrAppaNm))           '機器名
                .Add(pfSet_Param("appa_cnds", SqlDbType.NVarChar, ipstrAppaCnds))       '状況
                .Add(pfSet_Param("quantity", SqlDbType.Decimal, ipdecQuantity))         '数量
                .Add(pfSet_Param("notetext", SqlDbType.NVarChar, ipstrNotetext))        '備考
                .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))     'ユーザーＩＤ
                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                       '戻り値
            End With

            'コマンドタイプ設定(ストアド)
            cmdDB.CommandType = CommandType.StoredProcedure
            cmdDB.ExecuteNonQuery()

            'ストアド戻り値チェック
            mfUpdateDTL = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 明細表示番号行更新処理
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ</param>
    ''' <param name="iptrnDB">トランザクション</param>
    ''' <param name="ipstrArtclNo">物品転送管理番号</param>
    ''' <returns>ＳＱＬ実行結果</returns>
    ''' <remarks></remarks>
    Private Function mfAddDTL(ByVal ipconDB As SqlConnection,
                              ByVal iptrnDB As SqlTransaction,
                              ByVal ipstrArtclNo As String) As Integer
        Dim cmdDB As SqlCommand

        objStack = New StackFrame

        Try
            cmdDB = New SqlCommand("CNSUPDP002_U3", ipconDB)
            cmdDB.Transaction = iptrnDB
            With cmdDB.Parameters
                .Add(pfSet_Param("artcl_no", SqlDbType.NVarChar, ipstrArtclNo))     '物品転送管理番号
                .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name)) 'ユーザーＩＤ
                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                   '戻り値
            End With

            'コマンドタイプ設定(ストアド)
            cmdDB.CommandType = CommandType.StoredProcedure
            cmdDB.ExecuteNonQuery()

            'ストアド戻り値チェック
            mfAddDTL = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 明細行削除更新処理
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ</param>
    ''' <param name="iptrnDB">トランザクション</param>
    ''' <param name="ipstrArtclNo">物品転送管理番号</param>
    ''' <param name="ipdecSeqNo">連番</param>
    ''' <returns>ＳＱＬ実行結果</returns>
    ''' <remarks></remarks>
    Private Function mfDelDTL(ByVal ipconDB As SqlConnection,
                              ByVal iptrnDB As SqlTransaction,
                              ByVal ipstrArtclNo As String,
                              ByVal ipdecSeqNo As Decimal) As Integer
        Dim cmdDB As SqlCommand

        objStack = New StackFrame

        Try
            cmdDB = New SqlCommand("CNSUPDP002_D2", ipconDB)
            cmdDB.Transaction = iptrnDB
            With cmdDB.Parameters
                .Add(pfSet_Param("artcl_no", SqlDbType.NVarChar, ipstrArtclNo))     '物品転送管理番号
                .Add(pfSet_Param("seqno", SqlDbType.Decimal, ipdecSeqNo))          '連番
                .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name)) 'ユーザーＩＤ
                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                   '戻り値
            End With

            'コマンドタイプ設定(ストアド)
            cmdDB.CommandType = CommandType.StoredProcedure
            cmdDB.ExecuteNonQuery()

            'ストアド戻り値チェック
            mfDelDTL = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 変更回数更新処理
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ</param>
    ''' <param name="iptrnDB">トランザクション</param>
    ''' <param name="ipstrArtclNo">物品転送管理番号</param>
    ''' <returns>ＳＱＬ実行結果</returns>
    ''' <remarks></remarks>
    Private Function mfUpdateCount(ByVal ipconDB As SqlConnection,
                                   ByVal iptrnDB As SqlTransaction,
                                   ByVal ipstrArtclNo As String) As Integer
        Dim cmdDB As SqlCommand

        objStack = New StackFrame

        Try
            cmdDB = New SqlCommand("CNSUPDP002_U4", ipconDB)
            cmdDB.Transaction = iptrnDB
            With cmdDB.Parameters
                .Add(pfSet_Param("artcl_no", SqlDbType.NVarChar, ipstrArtclNo))     '物品転送管理番号
                .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name)) 'ユーザーＩＤ
                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                   '戻り値
            End With

            'コマンドタイプ設定(ストアド)
            cmdDB.CommandType = CommandType.StoredProcedure
            cmdDB.ExecuteNonQuery()

            'ストアド戻り値チェック
            mfUpdateCount = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 物品転送依頼ＮＧＣ送信データ（明細）取得
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ</param>
    ''' <param name="iptrnDB">トランザクション</param>
    ''' <param name="ipstrArtclNo">物品転送管理番号</param>
    ''' <returns>ＳＱＬ実行結果</returns>
    ''' <remarks></remarks>
    Private Function mfGet_Artcltrns_D(ByVal ipconDB As SqlConnection,
                                       ByVal iptrnDB As SqlTransaction,
                                       ByVal ipstrArtclNo As String,
                                       ByRef opData As DataSet) As Integer
        Dim cmdDB As SqlCommand = Nothing

        objStack = New StackFrame

        Try
            '工事完了報告明細(TOMAS)取得4
            cmdDB = New SqlCommand("CNSUPDP002_S5", ipconDB)
            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("artcl_no", SqlDbType.NVarChar, ipstrArtclNo))     '物品転送管理番号
            End With

            cmdDB.Transaction = iptrnDB

            '物品転送依頼ＮＧＣ送信データ
            opData = clsDataConnect.pfGet_DataSet(cmdDB)

            '正常
            Return 0
        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

            Return -1

        End Try

    End Function

    ''' <summary>
    ''' 物品転送依頼ＮＧＣ送信データ（ヘッダ）取得
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ</param>
    ''' <param name="iptrnDB">トランザクション</param>
    ''' <param name="ipstrArtclNo">物品転送管理番号</param>
    ''' <returns>ＳＱＬ実行結果</returns>
    ''' <remarks></remarks>
    Private Function mfGet_Artcltrns_H(ByVal ipconDB As SqlConnection,
                                       ByVal iptrnDB As SqlTransaction,
                                       ByVal ipstrArtclNo As String,
                                       ByRef opData As DataSet) As Integer
        Dim cmdDB As SqlCommand = Nothing

        objStack = New StackFrame

        Try
            '工事完了報告明細(TOMAS)取得4
            cmdDB = New SqlCommand("CNSUPDP002_S5", ipconDB)
            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("artcl_no", SqlDbType.NVarChar, ipstrArtclNo))     '物品転送管理番号
            End With

            cmdDB.Transaction = iptrnDB

            '物品転送依頼ＮＧＣ送信ヘッダデータ
            opData = clsDataConnect.pfGet_DataSet(cmdDB)

            '正常
            Return 0
        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

            Return -1
        End Try

    End Function

    ''' <summary>
    ''' 物品転送依頼送信更新
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ</param>
    ''' <param name="iptrnDB">トランザクション</param>
    ''' <param name="ipstrArtclNo">工事依頼番号</param>
    ''' <param name="ipdttSenddt">送信日時</param>
    ''' <returns>ＳＱＬ実行結果</returns>
    ''' <remarks></remarks>
    Private Function mfSet_TrnSPCCnstamnt(ByVal ipconDB As SqlConnection,
                                          ByVal iptrnDB As SqlTransaction,
                                          ByVal ipstrArtclNo As String,
                                          ByVal ipdttSenddt As DateTime) As Integer
        Dim cmdDB As SqlCommand = Nothing

        objStack = New StackFrame

        Try
            '物品転送依頼送信更新
            cmdDB = New SqlCommand("CNSUPDP002_U5", ipconDB)
            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("artcl_no", SqlDbType.NVarChar, ipstrArtclNo))             '工事依頼番号
                .Add(pfSet_Param("send_dt", SqlDbType.DateTime, ipdttSenddt))               '送信日時
                .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))         'ユーザーＩＤ
                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
            End With

            cmdDB.Transaction = iptrnDB

            cmdDB.CommandType = CommandType.StoredProcedure
            cmdDB.ExecuteNonQuery()

            'ストアド戻り値
            Return Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

            Return -1
        End Try

    End Function

    ''' <summary>
    ''' 送信フォルダ、バックアップフォルダにファイルを移動
    ''' </summary>
    ''' <param name="ipstrDataNm">物品転送依頼（明細）ファイル名</param>
    ''' <param name="ipstrDataBKNm">物品転送依頼（明細）バックアップファイル名</param>
    ''' <param name="ipstrDataAdd">物品転送依頼（明細）コピー元のサーバアドレス</param>
    ''' <param name="ipstrDataFolder">物品転送依頼（明細）コピー元のフォルダ</param>
    ''' <param name="ipstrFeeNm">物品転送依頼（ヘッダ）ファイル名</param>
    ''' <param name="ipstrFeeBKNm">物品転送依頼（ヘッダ）バックアップファイル名</param>
    ''' <param name="ipstrFeeAdd">物品転送依頼（ヘッダ）コピー元のサーバアドレス</param>
    ''' <param name="ipstrFeeFolder">物品転送依頼（ヘッダ）コピー元のフォルダ</param>
    ''' <returns>1:正常 1以外:エラー</returns>
    ''' <remarks>失敗時は全ファイル削除</remarks>
    Private Function mfFile(ByVal ipstrDataNm As String,
                            ByVal ipstrDataBKNm As String,
                            ByVal ipstrDataAdd As String,
                            ByVal ipstrDataFolder As String,
                            ByVal ipstrFeeNm As String,
                            ByVal ipstrFeeBKNm As String,
                            ByVal ipstrFeeAdd As String,
                            ByVal ipstrFeeFolder As String) As Integer
        Dim strDataAdd As String = Nothing
        Dim strDataFolder As String = Nothing
        Dim strDataBkAdd As String = Nothing
        Dim strDataBkFolder As String = Nothing
        Dim strFeeAdd As String = Nothing
        Dim strFeeFolder As String = Nothing
        Dim strFeeBkAdd As String = Nothing
        Dim strFeeBkFolder As String = Nothing

        objStack = New StackFrame

        '物品転送依頼（明細）送信先取得
        If pfGetPreservePlace("0233CS", strDataAdd, strDataFolder) <> 0 Then
            psMesBox(Me, "20001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_TRN_FILE_NM)
            Return -1
        End If

        '物品転送依頼（明細）バックアップ先取得
        If pfGetPreservePlace("0203CB", strDataBkAdd, strDataBkFolder) <> 0 Then
            psMesBox(Me, "20001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_TRN_FILE_NM)
            Return -1
        End If

        '物品転送依頼（ヘッダ）送信先取得
        If pfGetPreservePlace("0212CS", strFeeAdd, strFeeFolder) <> 0 Then
            psMesBox(Me, "20001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_TRN_FILE_NM)
            Return -1
        End If

        '物品転送依頼（ヘッダ）バックアップ先取得
        If pfGetPreservePlace("0222CB", strFeeBkAdd, strFeeBkFolder) <> 0 Then
            psMesBox(Me, "20001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_TRN_FILE_NM)
            Return -1
        End If

        Try
            '物品転送依頼（明細）送信フォルダにコピー
            System.IO.File.Copy("\\" & ipstrDataAdd & "\" & ipstrDataFolder & "\" & ipstrDataNm,
                                "\\" & strDataAdd & "\" & strDataFolder & "\" & ipstrDataNm)
            '物品転送依頼（明細）バックアップフォルダに移動
            System.IO.File.Move("\\" & ipstrDataAdd & "\" & ipstrDataFolder & "\" & ipstrDataNm,
                                "\\" & strDataBkAdd & "\" & strDataBkFolder & "\" & ipstrDataBKNm)


            '物品転送依頼（ヘッダ）送信フォルダにコピー
            System.IO.File.Copy("\\" & ipstrFeeAdd & "\" & ipstrFeeFolder & "\" & ipstrFeeNm,
                                "\\" & strFeeAdd & "\" & strFeeFolder & "\" & ipstrFeeNm)
            '物品転送依頼（ヘッダ）バックアップフォルダに移動
            System.IO.File.Move("\\" & ipstrFeeAdd & "\" & ipstrFeeFolder & "\" & ipstrFeeNm,
                                "\\" & strFeeBkAdd & "\" & strFeeBkFolder & "\" & ipstrFeeBKNm)

            '正常終了
            Return 0
        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

            'エラー時ファイル削除
            pfDeleteFile(strDataAdd, strDataFolder, ipstrDataNm)        '工事完了報告明細送信フォルダ
            pfDeleteFile(strDataBkAdd, strDataBkFolder, ipstrDataBKNm)  '工事完了報告明細バックアップフォルダ
            pfDeleteFile(strFeeAdd, strFeeFolder, ipstrFeeNm)           '物品転送依頼（ヘッダ）送信フォルダ
            pfDeleteFile(strFeeBkAdd, strFeeBkFolder, ipstrFeeBKNm)     '物品転送依頼（ヘッダ）バックアップフォルダ
            pfDeleteFile(ipstrDataAdd, ipstrDataFolder, ipstrDataNm)    '工事完了報告明細作業領域
            pfDeleteFile(ipstrFeeAdd, ipstrFeeFolder, ipstrFeeNm)       '工事料金明細作業領域

            Return -1
        End Try

    End Function

#End Region

End Class
