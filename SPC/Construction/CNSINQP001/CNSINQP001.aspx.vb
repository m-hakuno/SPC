'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　工事料金明細書
'*　ＰＧＭＩＤ：　CNSINQP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.10　：　土岐
'*  作　成　　：　2014.06.17　：　間瀬　明細登録時の初期パターン適用、工事完了登録を行って無い場合の確認の制御
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'CNSINQP001-001     2015/11/25      加賀       送信フラグの更新処理の修正
'CNSINQP001-002     2015/11/25      加賀       PDF作成状況表示に対応
'CNSINQP001-003     2015/12/03      加賀       [参照モード]時に明細の選択ボタン非活性化
'CNSINQP001-004     2015/12/07      加賀       検収済の締日の場合、確認を非活性化 
'CNSINQP001-005     2016/05/20      稲葉       工事区分ドロップダウンリスト取得


#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
Imports SPC.ClsCMExclusive
#End Region

Public Class CNSINQP001

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
    Private Const M_DISP_ID = P_FUN_CNS & P_SCR_INQ & P_PAGE & "001"    '工事料金明細書 画面ＩＤ
    Private Const M_VIEW_SEL = "選択データ"                             'ViewStateの明細選択行キー
    Private Const M_VIEW_EXTRA = "割増区分初期値"                       'ViewStateの割増区分初期値キー
    Private Const M_VIEW_CNFM_CHARGE = "確認済"                         'ViewStateの確認済フラグキー
    Private Const M_VIEW_SEND_STTS = "送信状況区分"                     'ViewStateの送信状況区分コードキー
    Private Const M_VIEW_EMREQ_CLS = "緊急依頼区分"                     'ViewStateの緊急依頼区分コードキー
    Private Const M_VIEW_TBOXCLS_CD = "ＴＢＯＸシステムコード"         'ViewStateのＴＢＯＸシステムコードキー
    Private Const M_CNSUPDP006 = P_FUN_CNS & P_SCR_UPD & P_PAGE & "006" '工事完了登録報告書 画面ＩＤ
    Private Const M_ITEM_CODE1 = "01"                                   '大項目コード１
    Private Const M_ITEM_CODE2 = "02"                                   '大項目コード２
    Private Const M_ITEM_CODE3 = "03"                                   '大項目コード３
    Private Const M_ITEM_CODE4 = "04"                                   '大項目コード４
    Private Const M_ITEM_CODE5 = "05"                                   '大項目コード５
    Private Const M_ITEM_CODE6 = "06"                                   '大項目コード６
    Private Const M_ITEM_CODE7 = "07"                                   '大項目コード７
    Private Const M_ITEM_CODE8 = "08"                                   '大項目コード８

    Private Const M_ITEM_CODEA = "A"                                   '大項目コードＡ

    Private Const M_DETAIL_NM = "工事完了報告明細"                      '送信帳票名（ＣＳＶ）１
    Private Const M_FEE_NM = "工事料金明細"                             '送信帳票名（ＣＳＶ）２
    'データ上の項目名
    Private Const M_L_CLS = "大区分"                                    '大区分
    Private Const M_M_CLS = "中区分"                                    '中区分
    Private Const M_S_CLS = "小区分"                                    '小区分
    Private Const M_ITEM_NM = "項目名"                                  '項目名
    Private Const M_QUANTITY = "数量"                                   '数量
    Private Const M_PRICE = "金額"                                      '金額
    'データ上の項目名　明細
    Private Const M_SEQ = "連番"                                        '連番
    Private Const M_CNST_CD = "コード"                                  '工事項目コード
    Private Const M_CNST_NM = "工事名"                                  '工事項目名
    Private Const M_UNIT_PRICE = "単価"                                 '単価
    Private Const M_EXTRA_CLS_NM = "割増区分"                           '割増区分名
    Private Const M_EXTRA_CLS = "割増区分コード"                        '割増区分コード
    Private Const M_SYM_CD = "振分コード"                               '振分コード
    Private Const M_INSERT_DT = "登録日時"                              '登録日時
    Private Const M_INSERT_USR = "登録者"                               '登録者
    Private Const M_UPDATE_DT = "更新日時"                              '更新日時
    Private Const M_UPDATE_USR = "更新者"                               '更新者
    Private Const M_VIEW_NEWEST = "最新締日"                            '最新締日

    '社員マスタ検索条件
    Private ReadOnly M_CNFRM = {"8", "3", Nothing}                      'ＳＰＣ，管理者以上
#End Region

#Region "構造体・列挙体定義"
    '============================================================================================================================
    '=　構造体・列挙体定義
    '============================================================================================================================
#End Region

#Region "変数定義"
    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
    Dim objStack As StackFrame
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom
    Dim clsExc As New ClsCMExclusive

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' ページ初期化
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        '表設定
        pfSet_GridView(Me.grvList, "CNSINQP001_1")
        pfSet_GridView_S_Off(Me.grvData3, "CNSINQP001_2")
        pfSet_GridView_S_Off(Me.grvData4, "CNSINQP001_2")
        pfSet_GridView_S_Off(Me.grvData5, "CNSINQP001_2")
        pfSet_GridView_S_Off(Me.grvData6, "CNSINQP001_2")
        pfSet_GridView_S_Off(Me.grvData7, "CNSINQP001_3")
        pfSet_GridView_S_Off(Me.grvData8, "CNSINQP001_2")

    End Sub

    ''' <summary>
    ''' ページロード
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'フッダーボタンのイベント設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnCSV_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnPDF_Click
        AddHandler Master.ppRigthButton3.Click, AddressOf btnPrint_Click
        AddHandler Master.ppRigthButton4.Click, AddressOf btnUpdate_Click
        AddHandler Master.ppLeftButton1.Click, AddressOf btnTrn_Click
        AddHandler Master.ppLeftButton2.Click, AddressOf btnCnstBreakDtl_Click
        AddHandler Me.txtCnsrCD.ppTextBox.TextChanged, AddressOf txtCnsrCD_TextChanged
        AddHandler Me.ddlExtraCls.ppDropDownList.TextChanged, AddressOf ddlExtraCls_TextChanged
        AddHandler Me.ddlCnstCls.ppDropDownList.SelectedIndexChanged, AddressOf ddlCnstCls_SelectedIndexChanged
        AddHandler Me.txtCloseDtym.ppTextBox.TextChanged, AddressOf txtCloseDtym_TextChanged

        If Not IsPostBack Then  '初回表示
            Dim intHyphen As Integer

            'プログラムＩＤ、画面名設定
            Master.ppProgramID = M_DISP_ID
            Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)
            '            Master.ppLogout_Mode = Global_asax. ClsComVer.E_ログアウトモード.閉じる
            Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる

            'パンくずリスト設定
            Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

            'フッターのボタン設定
            Master.ppRigthButton1.Text = "ＣＳＶ"
            Master.ppRigthButton1.OnClientClick =
                pfGet_OCClickMes("10001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "ＣＳＶ")
            Master.ppRigthButton2.Text = P_BTN_NM_PDF
            Master.ppRigthButton2.OnClientClick =
                pfGet_OCClickMes("10001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, P_BTN_NM_PDF)
            Master.ppRigthButton3.Text = P_BTN_NM_PRI
            Master.ppRigthButton3.OnClientClick =
                pfGet_OCClickMes("10001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事料金明細書")
            Master.ppRigthButton4.Text = P_BTN_NM_UPD
            Master.ppRigthButton4.OnClientClick =
                pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事料金明細書")
            Master.ppRigthButton4.ValidationGroup = "Update"
            Master.ppLeftButton1.Text = P_BTN_NM_TRA
            Master.ppLeftButton1.OnClientClick =
                pfGet_OCClickMes("20001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, M_FEE_NM & "、" & M_DETAIL_NM)
            Master.ppLeftButton2.Text = "工事完了登録"
            Master.ppLeftButton2.OnClientClick =
                pfGet_OCClickMes("30004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, clsCMDBC.pfGet_DispNm(M_CNSUPDP006))

            'ボタンの確認メッセージ設定
            Me.btnAmoAdd.OnClientClick =
                pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "明細")
            Me.btnAmoUpdate.OnClientClick =
                pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "選択中の明細")
            Me.btnAmoDel.OnClientClick =
                pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "選択中の明細")
            Me.btnCnfrm.OnClientClick =
                pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "確認")

            '明細選択行コード、割増区分、工事区分ポストバックイベント設定
            Me.txtCnsrCD.ppTextBox.AutoPostBack = True
            Me.ddlExtraCls.ppDropDownList.AutoPostBack = True
            Me.ddlCnstCls.ppDropDownList.AutoPostBack = True
            Me.txtCloseDtym.ppTextBox.AutoPostBack = True

            'セッション変数「遷移条件」「キー情報」が存在しない場合、画面を閉じる
            If Session(P_SESSION_TERMS) Is Nothing Or Session(P_KEY) Is Nothing Then
                psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                '排他削除
                clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                psClose_Window(Me)
                Return
            End If

            'ViewStateに「遷移条件」「遷移元ＩＤ」を保存
            ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)
            ViewState(P_KEY) = Session(P_KEY)(0)
            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            '★排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
            If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then

                Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)

            End If

            'ログインユーザーの権限確認
            If mfGet_UserAuth(User.Identity.Name) >= 3 Then   '確認権限あり
                Me.pnlCnfrm.Visible = True
            End If

            '確認者の参照条件設定（ＳＰＣ，管理者以上）
            Me.txtCnfrm.ppKeyCode = M_CNFRM

            'クリア
            msClearScreen()

            'キー情報検索条件に設定
            intHyphen = ViewState(P_KEY).ToString.IndexOf("-")
            If intHyphen = -1 Then  'ハイフンなし
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "工事料金")

                '排他削除
                clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                psClose_Window(Me)
                Exit Sub
            Else
                txtCntlNo.ppTextOne = ViewState(P_KEY).ToString.Substring(0, intHyphen)
                txtCntlNo.ppTextTwo = ViewState(P_KEY).ToString.Substring(intHyphen + 1)
            End If

            '検索
            If Not mfGet_Data(ViewState(P_KEY), True) Then
                '排他削除
                clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                psClose_Window(Me)
                Exit Sub
            End If


            '-----------------------------
            '2014/07/30 星野　ここから
            '-----------------------------
            '緊急依頼区分取得
            If Not mfJg_Emergency(ViewState(P_KEY)) Then
                '排他削除
                clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                psClose_Window(Me)
                Exit Sub
            End If
            '-----------------------------
            '2014/07/30 星野　ここまで
            '-----------------------------

            msSet_Cnst_Cls()

            '締日取得   'CNSINQP001-004
            msGet_Newest()

            '活性／非活性設定
            msSet_Mode(ViewState(P_SESSION_TERMS))

            'CNSINQP001-001
            '-----------------------------
            '2014/07/17 星野　ここから
            '-----------------------------
            '送信状況設定
            'If ViewState(M_VIEW_SEND_STTS) = "1" Then
            '    Me.lblSendSttsV.Text = "送信済"
            'ElseIf ViewState(M_VIEW_SEND_STTS) = "0" Then
            '    Me.lblSendSttsV.Text = "未送信"
            'End If
            '-----------------------------
            '2014/07/17 星野　ここまで
            '-----------------------------
            'CNSINQP001-001 END

            Me.txtCntlNo.ppTextBoxOne.Focus()
        End If
        '-----------------------------
        '2014/07/04 星野　ここから
        '-----------------------------
        '締日取得
        msGet_Newest()
        '-----------------------------
        '2014/07/04 星野　ここまで
        '-----------------------------

        '明細項目7番入力可否設定
        msSet_Grid7()
    End Sub

    ''' <summary>
    ''' グリッドデータバインド時 CNSINQP001-003
    ''' </summary>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        Try
            Select Case ViewState(P_SESSION_TERMS)
                Case ClsComVer.E_遷移条件.更新
                    '確認状況
                    If ViewState(M_VIEW_CNFM_CHARGE) Then
                        '確認済
                        msSet_GridBtnEnbl(False)
                    Else
                        '未確認
                        msSet_GridBtnEnbl(True)
                    End If
                Case ClsComVer.E_遷移条件.参照
                    msSet_GridBtnEnbl(False)
                Case Else
                    msSet_GridBtnEnbl(False)
            End Select
        Catch ex As Exception
            'メッセージ表示
            psMesBox(Me, "30005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後, "一覧の生成")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
    End Sub

    '---------------------------
    '2014/04/25 武 ここから
    '---------------------------
    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
                btnAmoClear.Enabled = False
                btnAmoAdd.Enabled = False
                btnAmoUpdate.Enabled = False
                btnAmoDel.Enabled = False
                Master.ppLeftButton2.Enabled = False
            Case "NGC"
                btnAmoClear.Enabled = False
                btnAmoAdd.Enabled = False
                btnAmoUpdate.Enabled = False
                btnAmoDel.Enabled = False
                Master.ppLeftButton2.Enabled = False
        End Select

    End Sub
    '---------------------------
    '2014/04/25 武 ここまで
    '---------------------------

    ''' <summary>
    ''' 検索ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        'ログ出力開始
        psLogStart(Me)
        If (Page.IsValid) Then

            '-----------------------------
            '2014/07/03 星野　ここから
            '-----------------------------
            msClearSelectAmount()
            '-----------------------------
            '2014/07/03 星野　ここまで
            '-----------------------------

            If Not mfGet_Data(Me.txtCntlNo.ppTextOne & "-" & Me.txtCntlNo.ppTextTwo, False) Then
                Exit Sub
            End If

            '-----------------------------
            '2014/07/30 星野　ここから
            '-----------------------------
            '緊急依頼区分取得
            If Not mfJg_Emergency(Me.txtCntlNo.ppTextOne & "-" & Me.txtCntlNo.ppTextTwo) Then
                Exit Sub
            End If
            '-----------------------------
            '2014/07/30 星野　ここまで
            '-----------------------------

            '活性／非活性設定
            msSet_Mode(ViewState(P_SESSION_TERMS))

            '工事区分活性／非活性判定
            msSet_Cnst_Cls()

            'CNSINQP001-001
            ''-----------------------------
            ''2014/07/17 星野　ここから
            ''-----------------------------
            ''送信状況設定
            'If ViewState(M_VIEW_SEND_STTS) = "1" Then
            '    Me.lblSendSttsV.Text = "送信済"
            'ElseIf ViewState(M_VIEW_SEND_STTS) = "0" Then
            '    Me.lblSendSttsV.Text = "未送信"
            'End If
            ''-----------------------------
            ''2014/07/17 星野　ここまで
            ''-----------------------------
            'CNSINQP001-001 END

            msSet_Grid7()
        End If
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 検索条件クリア
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click

        'ログ出力開始
        psLogStart(Me)
        msClearSearch()
        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 再計算ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnRecalculation_Click(sender As Object, e As EventArgs) Handles btnRecalculation.Click

        'ログ出力開始
        psLogStart(Me)
        msSet_ScreenPrice()
        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 明細クリアボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAmoClear_Click(sender As Object, e As EventArgs) Handles btnAmoClear.Click

        'ログ出力開始
        psLogStart(Me)
        msClearSelectAmount()
        msSet_DtlMode("1")
        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 明細追加ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAmoAdd_Click(sender As Object, e As EventArgs) Handles btnAmoAdd.Click

        Dim strSelRowSeqNo As String = Nothing
        Dim dtsConstData As DataSet = Nothing
        Dim drData() As DataRow

        'ログ出力開始
        psLogStart(Me)

        '工事項目取得
        If Not mfGet_Construct(Me.txtCnsrCD.ppText, Me.ddlExtraCls.ppSelectedValue, dtsConstData) Then  '整合性エラー
            Me.txtCnsrCD.psSet_ErrorNo("2002", "コード")
        End If

        '数量チェック
        If Me.txtQuantity.ppText.Trim = "0" Or Me.txtQuantity.ppText.Trim = "" Then
            Me.txtQuantity.psSet_ErrorNo("4001", Me.txtQuantity.ppName, "1以上の数値")
        End If

        If (Page.IsValid) Then  'エラーなし
            drData = dtsConstData.Tables(0).Select("コード = '" & Me.txtCnsrCD.ppText & "'")
            If mfAddSelectAmount(Me.txtCnsrCD.ppText,
                                     drData(0).Item(M_CNST_NM),
                                     drData(0).Item(M_UNIT_PRICE),
                                     Me.txtQuantity.ppText,
                                     Me.ddlExtraCls.ppSelectedTextOnly,
                                     Me.ddlExtraCls.ppSelectedValue,
                                     drData(0).Item(M_SYM_CD)) Then
                '工事区分活性／非活性設定
                msSet_Cnst_Cls()
                msSet_Grid7()
                '選択明細クリア
                msClearSelectAmount()

                msSet_DtlMode("1")

            Else   '追加失敗
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細")
            End If

        End If
        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 明細更新ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAmoUpdate_Click(sender As Object, e As EventArgs) Handles btnAmoUpdate.Click

        Dim strSelRowSeqNo As String = Nothing
        Dim decQuantity As Decimal = Nothing
        Dim dtsConstData As DataSet = Nothing
        Dim strCnstNM As String = Me.lblCnstNM.Text
        Dim strUnitPrice As String = Me.lblUnitPrice.Text
        Dim drData() As DataRow

        'ログ出力開始
        psLogStart(Me)

        '工事項目取得
        If Not mfGet_Construct(Me.txtCnsrCD.ppText, Me.ddlExtraCls.ppSelectedValue, dtsConstData) Then  '整合性エラー
            Me.txtCnsrCD.psSet_ErrorNo("2002", "コード")
        End If

        '数量チェック
        If Me.txtQuantity.ppText.Trim = "0" Or Me.txtQuantity.ppText.Trim = "" Then
            Me.txtQuantity.psSet_ErrorNo("4001", Me.txtQuantity.ppName, "1以上の数値")
        End If

        If (Page.IsValid) Then  'エラーなし
            '選択情報取得
            strSelRowSeqNo = TryCast(ViewState(M_VIEW_SEL), String)
            If strSelRowSeqNo Is Nothing Then   '選択行なし
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細")
                Exit Sub
            End If

            '数量を計算用に変換
            Decimal.TryParse(Me.txtQuantity.ppText, decQuantity)

            drData = dtsConstData.Tables(0).Select("コード = '" & Me.txtCnsrCD.ppText & "'")
            If mfUpdSelectAmount(strSelRowSeqNo,
                     Me.txtCnsrCD.ppText,
                     strCnstNM,
                     strUnitPrice,
                     decQuantity,
                     Me.ddlExtraCls.ppSelectedTextOnly,
                     Me.ddlExtraCls.ppSelectedValue,
                     drData(0).Item(M_SYM_CD)) Then

                '活性／非活性設定
                msSet_Grid7()
                '選択明細クリア
                msClearSelectAmount()

                msSet_DtlMode("1")

            Else    '更新失敗
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細")
            End If
            '-----------------------------
            '2014/04/11 土岐　ここまで
            '-----------------------------
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 明細削除ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAmoDel_Click(sender As Object, e As EventArgs) Handles btnAmoDel.Click

        Dim strSelRowSeqNo As String = Nothing
        'ログ出力開始
        psLogStart(Me)

        strSelRowSeqNo = TryCast(ViewState(M_VIEW_SEL), String)
        If strSelRowSeqNo Is Nothing Then   '選択行なし
            psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細")
            Exit Sub
        End If

        '選択行削除
        If mfDelSelectAmount(strSelRowSeqNo) Then
            '活性／非活性設定
            msSet_Cnst_Cls()
            msSet_Grid7()
            '選択明細クリア
            msClearSelectAmount()

            msSet_DtlMode("1")

        Else    '削除失敗
            psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細")

        End If
        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 明細ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        Dim intIndex As Integer = Nothing
        Dim rowData As GridViewRow = Nothing

        If e.CommandName <> "btnSelect" Then
            Exit Sub
        End If

        'ログ出力開始
        psLogStart(Me)
        intIndex = Convert.ToInt32(e.CommandArgument)   'ボタン押下行のIndex
        rowData = grvList.Rows(intIndex)                'ボタン押下行

        Me.txtCnsrCD.ppText = CType(rowData.FindControl(M_CNST_CD), TextBox).Text
        Me.txtQuantity.ppText = CType(rowData.FindControl(M_QUANTITY), TextBox).Text
        Me.ddlExtraCls.ppSelectedValue = CType(rowData.FindControl(M_EXTRA_CLS), TextBox).Text
        '--------------------------------
        '2014/07/04 星野　ここから
        '--------------------------------
        'msSet_Construct()
        Me.lblCnstNM.Text = CType(rowData.FindControl(M_CNST_NM), TextBox).Text
        Me.lblUnitPrice.Text = CType(rowData.FindControl(M_UNIT_PRICE), TextBox).Text
        '--------------------------------
        '2014/07/04 星野　ここまで
        '--------------------------------
        ViewState(M_VIEW_SEL) = CType(rowData.FindControl(M_SEQ), TextBox).Text
        msSet_DtlMode("2")
        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 工事完了登録
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCnstBreakDtl_Click(sender As Object, e As EventArgs)

        '工事完了報告書 画面のパス
        Const CNSUPDP006 As String = "~/" & P_CNS & "/" &
            P_FUN_CNS & P_SCR_UPD & P_PAGE & "006" & "/" &
            P_FUN_CNS & P_SCR_UPD & P_PAGE & "006.aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報設定
        Session(P_KEY) = {ViewState(P_KEY)}                         'キー情報

        '確認済の場合は変更不可のため参照とする
        If ViewState(M_VIEW_CNFM_CHARGE) Then   '確認済
            Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照              '遷移条件
        Else
            Dim strExclusiveDate As String = Nothing
            Dim arTable_Name As New ArrayList
            Dim arKey As New ArrayList

            If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 Then
                '★ロック対象テーブル名の登録
                arTable_Name.Insert(0, "D87_CNSTBREAK")

                '★ロックテーブルキー項目の登録(D87_CNSTBREAK)
                arKey.Insert(0, ViewState(P_KEY))       'D87_CONST_NO

                '★排他情報確認処理(更新画面へ遷移)
                If clsExc.pfSel_Exclusive(strExclusiveDate _
                                 , Me _
                                 , Session(P_SESSION_IP) _
                                 , Session(P_SESSION_PLACE) _
                                 , Session(P_SESSION_USERID) _
                                 , Session(P_SESSION_SESSTION_ID) _
                                 , ViewState(P_SESSION_GROUP_NUM) _
                                 , P_FUN_CNS & P_SCR_UPD & P_PAGE & "006" _
                                 , arTable_Name _
                                 , arKey) = 0 Then

                    '★登録年月日時刻
                    Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate

                Else

                    '排他ロック中
                    Exit Sub

                End If

            End If
            Session(P_SESSION_TERMS) = ViewState(P_SESSION_TERMS)   '遷移条件
        End If

        Session(P_SESSION_BCLIST) = Master.ppBcList_Text        'パンくずリスト
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)               'グループ番号

        Dim objStack As New StackFrame
        Dim strPrm As String = ""
        strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
        Dim tmp As Object() = Session(P_KEY)
        If Not tmp Is Nothing Then
            For zz = 0 To tmp.Length - 1
                If zz <> tmp.Length - 1 Then
                    strPrm &= tmp(zz).ToString & ","
                Else
                    strPrm &= tmp(zz).ToString
                End If
            Next
        End If

        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                        objStack.GetMethod.Name, CNSUPDP006, strPrm, "TRANS")

        '別ブラウザ起動
        psOpen_Window(Me, CNSUPDP006)
        'ログ出力終了
        psLogEnd(Me)

    End Sub
    '-----------------------------
    '2014/07/18 星野　ここから
    '-----------------------------
    ' ''' <summary>
    ' ''' 工事区分変更時
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub txtCnstCls_TextChanged(sender As Object, e As EventArgs)
    '    '-----------------------------
    '    '2014/04/11 土岐　ここから
    '    '-----------------------------
    '    Dim dstOrders As DataSet = Nothing
    '    '-----------------------------
    '    '2014/04/14 土岐　ここから
    '    '-----------------------------
    '    '-----------------------------
    '    '2014/04/18 土岐　ここから
    '    '-----------------------------
    '    If mfGet_Amount_Default(ViewState(P_KEY),
    '                            txtCnstCls.ppText,
    '                            ViewState(M_VIEW_EMREQ_CLS),
    '                            ViewState(M_VIEW_EXTRA),
    '                            ViewState(M_VIEW_TBOXCLS_CD),
    '                            dstOrders) Then
    '        '-----------------------------
    '        '2014/04/18 土岐　ここまで
    '        '-----------------------------
    '        '-----------------------------
    '        '2014/04/14 土岐　ここまで
    '        '-----------------------------
    '        If mfSet_Amount_Default(dstOrders) Then
    '            '活性／非活性設定
    '            msSet_Cnst_Cls()
    '            msSet_Grid7()

    '        End If
    '    End If
    '    '-----------------------------
    '    '2014/04/11 土岐　ここまで
    '    '-----------------------------
    'End Sub

    ''' <summary>
    ''' 工事区分変更時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlCnstCls_SelectedIndexChanged(sender As Object, e As EventArgs)

        Dim dstOrders As DataSet = Nothing

        If mfGet_Amount_Default(ViewState(P_KEY),
                                ddlCnstCls.ppSelectedValue,
                                ViewState(M_VIEW_EMREQ_CLS),
                                ViewState(M_VIEW_EXTRA),
                                ViewState(M_VIEW_TBOXCLS_CD),
                                dstOrders) Then
            If mfSet_Amount_Default(dstOrders) Then
                '活性／非活性設定
                msSet_Cnst_Cls()
                msSet_Grid7()

            End If
        End If

    End Sub
    '-----------------------------
    '2014/07/18 星野　ここまで
    '-----------------------------

    ''' <summary>
    ''' 明細コード変更時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtCnsrCD_TextChanged(sender As Object, e As EventArgs)

        ddlExtraCls.ppSelectedValue = ViewState(M_VIEW_EXTRA)
        msSet_Construct()
        Me.btnAmoAdd.Enabled = True
        Me.btnAmoClear.Enabled = True
        Me.btnAmoUpdate.Enabled = False
        Me.btnAmoDel.Enabled = False

    End Sub

    ''' <summary>
    ''' 割増区分変更時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlExtraCls_TextChanged(sender As Object, e As EventArgs)

        '-----------------------------
        '2014/07/04 星野　ここから
        '-----------------------------
        'msSet_Construct()
        Dim strQuantity As String = Me.txtQuantity.ppText
        msSet_Construct()
        Me.txtQuantity.ppText = strQuantity
        '-----------------------------
        '2014/07/04 星野　ここまで
        '-----------------------------

    End Sub

    '-----------------------------
    '2014/07/03 星野　ここから
    '-----------------------------
    ''' <summary>
    ''' 締日変更時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtCloseDtym_TextChanged(sender As Object, e As EventArgs)

        Dim txtName As TextBox = TryCast(Me.grvData7.Rows(0).FindControl(M_ITEM_NM), TextBox)
        Dim txtPrice As TextBox = TryCast(Me.grvData7.Rows(0).FindControl(M_PRICE), TextBox)

        psLogStart(Me)

        Page.Validate()

        '入力チェック
        msCheck_CloseDtymError()

        If (Page.IsValid) Then
            If Me.btnAmoDel.Enabled = False Then
                Me.btnAmoUpdate.Enabled = False
            End If

            '活性制御
            msSet_Cnst_Cls()
            'CNSINQP001-004
            Me.txtCnsrCD.ppEnabled = True
            Me.txtQuantity.ppEnabled = True
            Me.ddlExtraCls.ppEnabled = True
            Me.txtNotetext.ppEnabled = True
            Me.btnAmoAdd.Enabled = True
            Me.btnAmoClear.Enabled = True
            If Me.btnAmoDel.Enabled = True Then
                Me.btnAmoUpdate.Enabled = True
                Me.btnAmoAdd.Enabled = False
            End If
            Master.ppRigthButton4.Enabled = True
            If txtPrice.Text = String.Empty Then
                txtName.ReadOnly = True
            Else
                txtName.ReadOnly = False
            End If
        Else
            Me.ddlCnstCls.ppEnabled = False
            Me.txtCnsrCD.ppEnabled = False
            Me.txtQuantity.ppEnabled = False
            Me.ddlExtraCls.ppEnabled = False
            Me.txtNotetext.ppEnabled = False
            Me.btnAmoAdd.Enabled = False
            If Me.btnAmoAdd.Enabled = False And Me.btnAmoDel.Enabled = False Then
                Me.btnAmoClear.Enabled = False
            End If
            Me.btnAmoUpdate.Enabled = False
            Master.ppRigthButton4.Enabled = False
            txtName.ReadOnly = True
            'CNSINQP001-004 END
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub
    '-----------------------------
    '2014/07/03 星野　ここまで
    '-----------------------------

    ''' <summary>
    ''' 確認ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCnfrm_Click(sender As Object, e As EventArgs) Handles btnCnfrm.Click

        Dim txtName As TextBox
        Dim conDB As SqlConnection = Nothing
        Dim cmdCnstAmount As SqlCommand = Nothing
        Dim cmdCnstAmountDTL As SqlCommand = Nothing
        Dim intRtn As Integer
        objStack = New StackFrame

        'ログ出力開始
        psLogStart(Me)

        '確認者の個別チェック
        msCheck_CnfrmError()
        '締日の個別チェック
        msCheck_CloseDtymError()
        '工事区分の個別チェック
        msCheck_CnstClsDtymError()

        If (Page.IsValid) Then  'エラーなし
            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    '工事料金更新
                    cmdCnstAmount = New SqlCommand("CNSINQP001_U2", conDB)
                    With cmdCnstAmount.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("cntl_no", SqlDbType.NVarChar, ViewState(P_KEY)))          '工事依頼番号
                        '.Add(pfSet_Param("send_stts", SqlDbType.NVarChar,
                        'ViewState(M_VIEW_SEND_STTS)))                              '送信状況区分 'CNSINQP001-001 コメントアウト
                        .Add(pfSet_Param("emreq_cls", SqlDbType.NVarChar,
                                         ViewState(M_VIEW_EMREQ_CLS)))                              '緊急依頼区分
                        .Add(pfSet_Param("close_dt", SqlDbType.NVarChar,
                                         Me.txtCloseDtym.ppText & Me.ddlCloseDt.ppSelectedValue))   '締日（年月）
                        '-----------------------------
                        '2014/07/18 星野　ここから
                        '-----------------------------
                        '.Add(pfSet_Param("cnst_cls", SqlDbType.NVarChar, Me.txtCnstCls.ppText))     '工事区分
                        .Add(pfSet_Param("cnst_cls", SqlDbType.NVarChar, Me.ddlCnstCls.ppSelectedValue))     '工事区分
                        '-----------------------------
                        '2014/07/18 星野　ここまで
                        '-----------------------------
                        .Add(pfSet_Param("tbox_tcst", SqlDbType.Money, Me.lblPrice1.Text))          'ＴＢＯＸ機器費計
                        .Add(pfSet_Param("lan_tcst", SqlDbType.Money, Me.lblPrice2.Text))           'ＬＡＮ機器費計
                        .Add(pfSet_Param("base_tcst", SqlDbType.Money, Me.lblPrice3.Text))          '基本料金計
                        .Add(pfSet_Param("cnst_tcst", SqlDbType.Money, Me.lblPrice4.Text))          '工事費計
                        .Add(pfSet_Param("mtrl_tcst", SqlDbType.Money, Me.lblPrice5.Text))          '材料費計
                        .Add(pfSet_Param("oth_tcst", SqlDbType.Money, Me.lblPrice6.Text))           'その他費用計
                        .Add(pfSet_Param("adj_tcst", SqlDbType.Money, Me.lblPrice7.Text))           'その他調整費計
                        .Add(pfSet_Param("subtotal", SqlDbType.Money, Me.lblPrice3to7.Text))        '小計
                        .Add(pfSet_Param("card_tcst", SqlDbType.Money, Me.lblPrice8.Text))          'カード受付機関連計
                        .Add(pfSet_Param("total", SqlDbType.Money, Me.lblPriceSum.Text))            '合計
                        txtName = TryCast(Me.grvData7.Rows(0).FindControl(M_ITEM_NM), TextBox)
                        .Add(pfSet_Param("oth_item", SqlDbType.NVarChar, txtName.Text))             'その他調整金項目
                        .Add(pfSet_Param("notetext", SqlDbType.NVarChar, Me.txtNotetext.ppText))    '備考
                        .Add(pfSet_Param("cnfm_charge", SqlDbType.NVarChar, Me.txtCnfrm.ppText))    '確認者
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))         'ユーザーＩＤ
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
                    End With

                    '明細全件削除
                    cmdCnstAmountDTL = New SqlCommand("CNSINQP001_D1", conDB)
                    With cmdCnstAmountDTL.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("cntl_no", SqlDbType.NVarChar, ViewState(P_KEY)))  '工事依頼番号
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                   '戻り値
                    End With

                    'データ更新
                    Using conTrn = conDB.BeginTransaction
                        cmdCnstAmount.Transaction = conTrn
                        cmdCnstAmountDTL.Transaction = conTrn

                        '工事料金更新
                        cmdCnstAmount.CommandType = CommandType.StoredProcedure
                        cmdCnstAmount.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdCnstAmount.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事料金")
                            Exit Sub
                        End If

                        '明細全件削除
                        cmdCnstAmountDTL.CommandType = CommandType.StoredProcedure
                        cmdCnstAmountDTL.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdCnstAmountDTL.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事料金明細")
                            Exit Sub
                        End If

                        '明細登録
                        For Each rowData As GridViewRow In grvList.Rows
                            intRtn = mfUpdateDTL(conDB,
                                                 conTrn,
                                                 ViewState(P_KEY),
                                                 CType(rowData.FindControl("コード"), TextBox).Text,
                                                 CType(rowData.FindControl("工事名"), TextBox).Text,
                                                 CType(rowData.FindControl("単価"), TextBox).Text,
                                                 CType(rowData.FindControl("数量"), TextBox).Text,
                                                 CType(rowData.FindControl("割増区分コード"), TextBox).Text,
                                                 CType(rowData.FindControl("振分コード"), TextBox).Text,
                                                 CType(rowData.FindControl("金額"), TextBox).Text,
                                                 CType(rowData.FindControl("登録日時"), TextBox).Text,
                                                 CType(rowData.FindControl("登録者"), TextBox).Text,
                                                 CType(rowData.FindControl("更新日時"), TextBox).Text,
                                                 CType(rowData.FindControl("更新者"), TextBox).Text)

                            'ストアド戻り値チェック
                            If intRtn <> 0 Then
                                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事料金明細")
                                Exit Sub
                            End If
                        Next

                        'コミット
                        conTrn.Commit()
                    End Using

                    'CNSINQP001-001
                    '再取得 & 再表示
                    If Not mfGet_Data(ViewState(P_KEY), False) Then
                        psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事料金")
                        Exit Try
                    End If
                    'CNSINQP001-001 END

                    '完了メッセージ表示
                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "工事料金明細")

                    '確認済フラグ設定
                    If txtCnfrm.ppText = String.Empty Then
                        '確認解除
                        ViewState(M_VIEW_CNFM_CHARGE) = Nothing
                    Else
                        '確認済
                        ViewState(M_VIEW_CNFM_CHARGE) = True
                        btnAmoClear_Click(sender, e)    'CNSINQP001-004
                    End If

                    '活性／非活性設定
                    msSet_Mode(ViewState(P_SESSION_TERMS))

                    'CNSINQP001-001
                    ''--------------------------------
                    ''2014/07/07 星野　ここから
                    ''--------------------------------
                    ''送信状況設定
                    'If ViewState(M_VIEW_SEND_STTS) = "1" Then
                    '    Me.lblSendSttsV.Text = "送信済"
                    'ElseIf ViewState(M_VIEW_SEND_STTS) = "0" Then
                    '    Me.lblSendSttsV.Text = "未送信"
                    'End If
                    ''--------------------------------
                    ''2014/07/07 星野　ここまで
                    ''--------------------------------
                    'CNSINQP001-001 END

                    msSet_Grid7()
                Catch ex As Exception
                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事料金")
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
        End If

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

        Dim txtName As TextBox
        Dim conDB As SqlConnection = Nothing
        Dim cmdCnstAmount As SqlCommand = Nothing
        Dim cmdCnstAmountDTL As SqlCommand = Nothing
        Dim intRtn As Integer
        objStack = New StackFrame

        'ログ出力開始
        psLogStart(Me)

        '締日の個別チェック
        msCheck_CloseDtymError()
        '工事区分の個別チェック
        msCheck_CnstClsDtymError()

        If (Page.IsValid) Then  'エラーなし

            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    '工事料金更新
                    cmdCnstAmount = New SqlCommand("CNSINQP001_U1", conDB)
                    With cmdCnstAmount.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("cntl_no", SqlDbType.NVarChar, ViewState(P_KEY)))          '工事依頼番号
                        '.Add(pfSet_Param("send_stts", SqlDbType.NVarChar,
                        '                 ViewState(M_VIEW_SEND_STTS)))                              '送信状況区分 'CNSINQP001-001 コメントアウト
                        .Add(pfSet_Param("emreq_cls", SqlDbType.NVarChar,
                                         ViewState(M_VIEW_EMREQ_CLS)))                              '緊急依頼区分
                        .Add(pfSet_Param("close_dt", SqlDbType.NVarChar,
                                         Me.txtCloseDtym.ppText & Me.ddlCloseDt.ppSelectedValue))   '締日（年月）
                        '-----------------------------
                        '2014/07/18 星野　ここから
                        '-----------------------------
                        '.Add(pfSet_Param("cnst_cls", SqlDbType.NVarChar, Me.txtCnstCls.ppText))     '工事区分
                        .Add(pfSet_Param("cnst_cls", SqlDbType.NVarChar, Me.ddlCnstCls.ppSelectedValue))     '工事区分
                        '-----------------------------
                        '2014/07/18 星野　ここまで
                        '-----------------------------
                        .Add(pfSet_Param("tbox_tcst", SqlDbType.Money, Me.lblPrice1.Text))          'ＴＢＯＸ機器費計
                        .Add(pfSet_Param("lan_tcst", SqlDbType.Money, Me.lblPrice2.Text))           'ＬＡＮ機器費計
                        .Add(pfSet_Param("base_tcst", SqlDbType.Money, Me.lblPrice3.Text))          '基本料金計
                        .Add(pfSet_Param("cnst_tcst", SqlDbType.Money, Me.lblPrice4.Text))          '工事費計
                        .Add(pfSet_Param("mtrl_tcst", SqlDbType.Money, Me.lblPrice5.Text))          '材料費計
                        .Add(pfSet_Param("oth_tcst", SqlDbType.Money, Me.lblPrice6.Text))           'その他費用計
                        .Add(pfSet_Param("adj_tcst", SqlDbType.Money, Me.lblPrice7.Text))           'その他調整費計
                        .Add(pfSet_Param("subtotal", SqlDbType.Money, Me.lblPrice3to7.Text))        '小計
                        .Add(pfSet_Param("card_tcst", SqlDbType.Money, Me.lblPrice8.Text))          'カード受付機関連計
                        .Add(pfSet_Param("total", SqlDbType.Money, Me.lblPriceSum.Text))            '合計
                        txtName = TryCast(Me.grvData7.Rows(0).FindControl(M_ITEM_NM), TextBox)
                        .Add(pfSet_Param("oth_item", SqlDbType.NVarChar, txtName.Text))             'その他調整金項目
                        .Add(pfSet_Param("notetext", SqlDbType.NVarChar, Me.txtNotetext.ppText))    '備考
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))         'ユーザーＩＤ
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
                    End With

                    '明細全件削除
                    cmdCnstAmountDTL = New SqlCommand("CNSINQP001_D1", conDB)
                    With cmdCnstAmountDTL.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("cntl_no", SqlDbType.NVarChar, ViewState(P_KEY)))  '工事依頼番号
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                   '戻り値
                    End With

                    'データ更新
                    Using conTrn = conDB.BeginTransaction
                        cmdCnstAmount.Transaction = conTrn
                        cmdCnstAmountDTL.Transaction = conTrn

                        '工事料金更新
                        cmdCnstAmount.CommandType = CommandType.StoredProcedure
                        cmdCnstAmount.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdCnstAmount.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事料金")
                            Exit Sub
                        End If

                        '明細全件削除
                        cmdCnstAmountDTL.CommandType = CommandType.StoredProcedure
                        cmdCnstAmountDTL.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdCnstAmountDTL.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事料金明細")
                            Exit Sub
                        End If

                        '明細登録
                        For Each rowData As GridViewRow In grvList.Rows
                            intRtn = mfUpdateDTL(conDB,
                                                 conTrn,
                                                 ViewState(P_KEY),
                                                 CType(rowData.FindControl("コード"), TextBox).Text,
                                                 CType(rowData.FindControl("工事名"), TextBox).Text,
                                                 CType(rowData.FindControl("単価"), TextBox).Text,
                                                 CType(rowData.FindControl("数量"), TextBox).Text,
                                                 CType(rowData.FindControl("割増区分コード"), TextBox).Text,
                                                 CType(rowData.FindControl("振分コード"), TextBox).Text,
                                                 CType(rowData.FindControl("金額"), TextBox).Text,
                                                 CType(rowData.FindControl("登録日時"), TextBox).Text,
                                                 CType(rowData.FindControl("登録者"), TextBox).Text,
                                                 CType(rowData.FindControl("更新日時"), TextBox).Text,
                                                 CType(rowData.FindControl("更新者"), TextBox).Text)

                            'ストアド戻り値チェック
                            If intRtn <> 0 Then
                                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事料金明細")
                                Exit Sub
                            End If
                        Next

                        'コミット
                        conTrn.Commit()
                    End Using

                    '活性／非活性設定
                    msSet_Mode(ViewState(P_SESSION_TERMS))

                    'CNSINQP001-001
                    ''--------------------------------
                    ''2014/07/07 星野　ここから
                    ''--------------------------------
                    ''送信状況設定
                    'If ViewState(M_VIEW_SEND_STTS) = "1" Then
                    '    Me.lblSendSttsV.Text = "送信済"
                    'ElseIf ViewState(M_VIEW_SEND_STTS) = "0" Then
                    '    Me.lblSendSttsV.Text = "未送信"
                    'End If
                    ''--------------------------------
                    ''2014/07/07 星野　ここまで
                    ''--------------------------------
                    'CNSINQP001-001 END

                    msSet_Grid7()
                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "工事料金明細")
                Catch ex As Exception
                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事料金")
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
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' ＣＳＶ出力処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCSV_Click(sender As Object, e As EventArgs)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        objStack = New StackFrame

        'ログ出力開始
        psLogStart(Me)

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                '工事料金更新
                cmdDB = New SqlCommand("CNSINQP001_S5", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("cntl_no", SqlDbType.NVarChar, ViewState(P_KEY)))          '工事依頼番号
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器代金及び工事料金明細書")
                    Return
                End If

                pfDLCSV("機器代金及び工事料金明細書", ViewState(P_KEY), dstOrders.Tables(0), False, Me)

            Catch ex As Threading.ThreadAbortException

            Catch ex As Exception
                psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器代金及び工事料金明細書")
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
    ''' ＰＤＦ出力処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPDF_Click(sender As Object, e As EventArgs)

        Dim strServerAddress As String = Nothing
        Dim strFolderNM As String = Nothing
        Dim datCreateDate As DateTime = Nothing
        Dim strFileNm As String = Nothing
        Dim intRtn As Integer
        Dim dttMain As DataTable = Nothing
        Dim dttData1 As DataTable = Nothing
        Dim dttData2 As DataTable = Nothing

        'ログ出力開始
        psLogStart(Me)

        'データ取得
        intRtn = mfGet_PDFData(ViewState(P_KEY), dttMain, dttData1, dttData2)

        'データ取得エラー	
        If intRtn <> 0 Then
            'エラー処理
            psMesBox(Me, "10002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器代金及び工事料金明細書_1")
            Exit Sub
        End If

        psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name, objStackFrame.GetMethod.Name, "", "前　フォルダ名：" & strFolderNM, "Catch")
        'ファイル出力
        '--------------------------------
        '2014/07/18 星野　ここから
        '--------------------------------
        'intRtn = pfPDF("0271PN",
        '               "機器代金及び工事料金明細書",
        '               ViewState(P_KEY),
        '               New CNSREP009(dttData1, dttData2),
        '               dttMain,
        '               strServerAddress,
        '               strFolderNM,
        '               datCreateDate,
        '               strFileNm)
        intRtn = pfPDF("0271PN",
                       lblHallNmV.Text,
                       ViewState(P_KEY),
                       New CNSREP009(dttData1, dttData2),
                       dttMain,
                       strServerAddress,
                       strFolderNM,
                       datCreateDate,
                       strFileNm,
                       Session.SessionID,
                       True)
        '--------------------------------
        '2014/07/18 星野　ここまで
        '--------------------------------
        psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name, objStackFrame.GetMethod.Name, "", "後　フォルダ名：" & strFolderNM, "Catch")

        'ファイル出力エラー	
        If intRtn <> 0 Then
            'エラー処理
            psMesBox(Me, "10002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器代金及び工事料金明細書_2")
            Exit Sub
        End If

        'ダウンロードファイル（T07)にレコードを追加		
        intRtn = pfSetDwnldFile(ViewState(P_KEY), "0271PN", "工事料金明細書", strFileNm, "機器代金及び工事料金明細書",
                                strServerAddress, strFolderNM, datCreateDate, User.Identity.Name)

        'ダウンロードファイル（T07)にレコードを追加エラー		
        If intRtn <> 0 Then
            'エラー処理	
            psMesBox(Me, "10002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器代金及び工事料金明細書_3")
            Exit Sub
        End If

        'CNSINQP001-002
        '再取得 & 再表示
        If Not mfGet_Data(ViewState(P_KEY), False) Then
            'エラー処理	
            psMesBox(Me, "10002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器代金及び工事料金明細書_4")
            Exit Sub
        End If
        'CNSINQP001-002 END

        '処理完了ポップアップ表示
        psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "PDF出力")

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 印刷出力処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPrint_Click(sender As Object, e As EventArgs)

        Dim intRtn As Integer
        Dim dttMain As DataTable = Nothing
        Dim dttData1 As DataTable = Nothing
        Dim dttData2 As DataTable = Nothing

        'ログ出力開始
        psLogStart(Me)

        'データ取得
        intRtn = mfGet_PDFData(ViewState(P_KEY), dttMain, dttData1, dttData2)

        'データ取得エラー	
        If intRtn <> 0 Then
            'エラー処理
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器代金及び工事料金明細書")
            Exit Sub
        End If

        '印刷処理
        psPrintPDF(Me, New CNSREP009(dttData1, dttData2), dttMain, "機器代金及び工事料金明細書")

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 送信処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnTrn_Click(sender As Object, e As EventArgs)

        'Const DETAIL_FNM = "FT_DetailReport_"
        'Const FEE_FNM = "FT_FeeReport_"
        Dim conDB As SqlConnection = Nothing
        Dim dstOrders As New DataSet
        Dim strDetailNo As String = Nothing
        Dim strFeeNo As String = Nothing
        Dim intRtn As Integer
        Dim strWorkAdd As String = Nothing
        Dim strWorkFolder As String = Nothing
        'Dim strWorkPath As String
        Dim dttDate As DateTime
        Dim strYears As String
        Dim strDay As String
        'Dim strDetailFileNm As String   '工事完了報告明細
        'Dim strDetailBKFileNm As String '工事完了報告明細バックアップ
        'Dim strFeeFileNm As String      '工事料金明細
        'Dim strFeeBKFileNm As String    '工事料金明細バックアップ
        objStack = New StackFrame

        'ログ出力開始
        psLogStart(Me)

        '作成日取得
        dttDate = DateTime.Now
        strDay = dttDate.ToString("dd")
        strYears = dttDate.ToString("yyyyMM")

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try

                '工事料金明細(D28)、工事完了報告(D87)の登録確認
                Dim cmdCheck As SqlCommand = New SqlCommand("CNSINQP001_S10", conDB)
                With cmdCheck.Parameters
                    .Add(pfSet_Param("const_no", SqlDbType.NVarChar, ViewState(P_KEY)))         '工事依頼番号
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
                End With

                cmdCheck.CommandType = CommandType.StoredProcedure
                cmdCheck.ExecuteNonQuery()

                'ストアド戻り値チェック
                intRtn = Integer.Parse(cmdCheck.Parameters("retvalue").Value.ToString)
                If intRtn = -1 Then         '工事料金明細（未登録）
                    psMesBox(Me, "40001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事料金")
                    Exit Sub
                ElseIf intRtn = -2 Then     '工事完了報告（未登録）
                    psMesBox(Me, "40002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事料金")
                    Exit Sub
                End If

                'データ更新
                Using conTrn = conDB.BeginTransaction

                    '工事完了報告明細(TOMAS)追加／更新
                    intRtn = mfSet_Cnstbreak(conDB, conTrn, ViewState(P_KEY))
                    If intRtn <> 0 Then
                        psMesBox(Me, "20001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)
                        Exit Sub
                    End If

                    '工事料金明細(TOMAS)追加／更新
                    intRtn = mfSet_Cnstamnt(conDB, conTrn, ViewState(P_KEY))
                    If intRtn <> 0 Then
                        psMesBox(Me, "20001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)
                        Exit Sub
                    End If

                    '----------------------------------------------------------------
                    ' 工事依頼書兼仕様書の案件ステータスを工事料金登録済みに更新する
                    '----------------------------------------------------------------
                    intRtn = mfSet_Cnstrecspec(conDB, conTrn, ViewState(P_KEY))
                    If intRtn <> 0 Then
                        psMesBox(Me, "20001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)
                        Exit Sub
                    End If

                    ''工事完了報告明細(TOMAS)送信回数取得
                    'intRtn = mfSet_SndFilrNo(conDB, conTrn, "3", strDay, strDetailNo)
                    'If intRtn <> 0 Then
                    '    psMesBox(Me, "20001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)
                    '    Exit Sub
                    'End If

                    ''工事料金明細(TOMAS)送信回数取得
                    'intRtn = mfSet_SndFilrNo(conDB, conTrn, "4", strDay, strFeeNo)
                    'If intRtn <> 0 Then
                    '    psMesBox(Me, "20001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)
                    '    Exit Sub
                    'End If

                    ''ファイル名生成
                    'strDetailFileNm = DETAIL_FNM & strDay & strDetailNo & ".csv"
                    'strDetailBKFileNm = DETAIL_FNM & strYears & strDay & strDetailNo & ".csv"
                    'strFeeFileNm = FEE_FNM & strDay & strFeeNo & ".csv"
                    'strFeeBKFileNm = FEE_FNM & strYears & strDay & strFeeNo & ".csv"

                    ''工事完了報告明細(SPC)送信更新
                    'intRtn = mfSet_TrnSPCCnstbreak(conDB, conTrn, ViewState(P_KEY), strDetailFileNm)
                    'If intRtn <> 0 Then
                    '    psMesBox(Me, "20001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)
                    '    Exit Sub
                    'End If

                    ''工事完了報告明細(TOMAS)送信更新
                    'intRtn = mfSet_TrnCnstbreak(conDB, conTrn, ViewState(P_KEY), strDetailFileNm)
                    'If intRtn <> 0 Then
                    '    psMesBox(Me, "20001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)
                    '    Exit Sub
                    'End If

                    ''工事料金明細(SPC)送信状況更新 'CNSINQP001-001 有効化
                    intRtn = mfSet_TrnSPCCnstamnt(conDB, conTrn, ViewState(P_KEY))
                    If intRtn <> 0 Then
                        psMesBox(Me, "20001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)
                        Exit Sub
                    End If

                    ''工事料金明細(TOMAS)送信更新
                    'intRtn = mfSet_TrnCnstamnt(conDB, conTrn, ViewState(P_KEY), strFeeFileNm)
                    'If intRtn <> 0 Then
                    '    psMesBox(Me, "20001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)
                    '    Exit Sub
                    'End If

                    ''作業領域取得
                    'If pfGetPreservePlace("1031FT", strWorkAdd, strWorkFolder) <> 0 Then
                    '    psMesBox(Me, "20001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)
                    '    Exit Sub
                    'End If
                    ''strWorkPath = "\\" & strWorkAdd & "\" & strWorkFolder & "\" & Session.SessionID

                    ''工事完了報告明細(TOMAS)データ取得
                    'intRtn = mfGet_Cnstbreak(conDB, conTrn, ViewState(P_KEY), dstOrders)
                    'If intRtn <> 0 Then
                    '    psMesBox(Me, "20001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)
                    '    Exit Sub
                    'End If

                    ''作業領域にＣＳＶ出力
                    'intRtn = pfCreateCsvFile(strWorkPath, strDetailFileNm, dstOrders, False)
                    'If intRtn <> 0 Then
                    '    psMesBox(Me, "20001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)
                    '    Exit Sub
                    'End If

                    ''工事料金明細(TOMAS)データ取得
                    'intRtn = mfGet_Cnstamnt(conDB, conTrn, ViewState(P_KEY), dstOrders)
                    'If intRtn <> 0 Then
                    '    psMesBox(Me, "20001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)
                    '    Exit Sub
                    'End If

                    ''作業領域にＣＳＶ出力
                    'intRtn = pfCreateCsvFile(strWorkPath, strFeeFileNm, dstOrders, False)
                    'If intRtn <> 0 Then
                    '    psMesBox(Me, "20001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)
                    '    Exit Sub
                    'End If

                    ''ファイル移動
                    'intRtn = mfFile(strDetailFileNm, strDetailBKFileNm, strWorkAdd, strWorkFolder,
                    '       strFeeFileNm, strFeeBKFileNm, strWorkAdd, strWorkFolder)
                    'If intRtn <> 0 Then
                    '    psMesBox(Me, "20001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)
                    '    Exit Sub
                    'End If

                    'コミット
                    conTrn.Commit()

                    'Master.ppLeftButton1.Enabled = False    'CNSINQP001-001

                End Using

                'CNSINQP001-001
                '再取得 & 再表示
                If Not mfGet_Data(ViewState(P_KEY), False) Then
                    psMesBox(Me, "20001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)
                    Exit Try
                End If
                'CNSINQP001-001 END

                '完了メッセージ表示
                psMesBox(Me, "20009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)

            Catch ex As Exception
                psMesBox(Me, "20001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)
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

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <param name="ipstrCnstNo">工事依頼番号</param>
    ''' <param name="ipblnInit">初期表示フラグ（True：初期 False：検索）</param>
    ''' <returns>検索成功：True, 検索失敗：False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_Data(ByVal ipstrCnstNo As String, ByVal ipblnInit As Boolean) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        objStack = New StackFrame

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("CNSINQP001_S3", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("cntl_no", SqlDbType.NVarChar, ipstrCnstNo))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If ipblnInit Then
                    '明細項目設定
                    msSet_Data_Nm(conDB, ipstrCnstNo)
                Else
                    If dstOrders.Tables(0).Rows.Count = 0 Then
                        '0件
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前)
                        Return False
                    End If

                    '-----------------------------
                    '2014/05/23 間瀬　ここから
                    '-----------------------------
                    '参照時に検索を行った際に排他処理を行わないようにする
                    If Session(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.参照 Then
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
                                Return False
                            End If
                        End If

                        '★ロック対象テーブル名の登録
                        arTable_Name.Insert(0, "D27_CNST_AMOUNT")
                        arTable_Name.Insert(1, "D28_CNST_AMOUNT_DTL")

                        '★ロックテーブルキー項目の登録(D27_CNST_AMOUNT, D28_CNST_AMOUNT_DTL)
                        arKey.Insert(0, ipstrCnstNo)        'D27_CNTL_NO
                        arKey.Insert(1, ipstrCnstNo)        'D28_CNTL_NO

                        '★排他情報確認処理(更新画面へ遷移)
                        If clsExc.pfSel_Exclusive(strExclusiveDate _
                                         , Me _
                                         , Session(P_SESSION_IP) _
                                         , Session(P_SESSION_PLACE) _
                                         , Session(P_SESSION_USERID) _
                                         , Session(P_SESSION_SESSTION_ID) _
                                         , ViewState(P_SESSION_GROUP_NUM) _
                                         , M_DISP_ID _
                                         , arTable_Name _
                                         , arKey) = 0 Then

                            '★登録年月日時刻
                            Me.Master.ppExclusiveDate = strExclusiveDate

                        Else

                            '排他ロック中
                            Return False

                        End If
                    End If
                    '-----------------------------
                    '2014/05/23 間瀬　ここから
                    '-----------------------------
                End If

                '取得したデータを画面に設定
                msSet_MainData(dstOrders)

                '明細データ設定
                msSet_Amount_Data(conDB, ipstrCnstNo)

                '明細金額、数量設定
                msSet_GridPrice()

                ViewState(P_KEY) = ipstrCnstNo

                mfGet_Data = True
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "工事料金")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Return False
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            Return False
        End If

    End Function

    ''' <summary>
    ''' データ設定処理
    ''' </summary>
    ''' <param name="ipdstData">設定データ</param>
    ''' <remarks></remarks>
    Private Sub msSet_MainData(ByVal ipdstData As DataSet)

        objStack = New StackFrame

        Try
            '取得したデータを設定
            With ipdstData.Tables(0).Rows(0)
                Me.lblSendSttsV.Text = .Item("送信状況").ToString
                Me.lblPDFSttsV.Text = .Item("PDF作成状況").ToString 'CNSINQP001-002
                Me.lblTboxIDV.Text = .Item("ＴＢＯＸＩＤ").ToString
                Me.lblHallNmV.Text = .Item("ホール名").ToString
                Me.lblCnstDtV.Text = .Item("工事日時").ToString
                Me.lblDatarcvDtV.Text = .Item("依頼受信日時").ToString
                Me.lblEmreqCls.Text = .Item("緊急依頼").ToString
                Me.lblTboxclsNmV.Text = .Item("システム").ToString

                If .Item("T03_TBOXCLASS_CD").ToString.Substring(0, 3) = "TPC" Then
                    Me.lblTPCTITLE.Text = "登録システム"
                    Me.lblTPCNAME.Text = .Item("T03_TBOXCLASS_CD").ToString
                    Me.lblTPCVER.Text = "VER:" & .Item("T03_VERSION").ToString
                Else
                    Me.lblTPCTITLE.Text = ""
                    Me.lblTPCNAME.Text = ""
                    Me.lblTPCVER.Text = ""
                End If

                Me.lblNew_2.Text = .Item("工事種別_新規").ToString
                Me.lblExpansion_2.Text = .Item("工事種別_増設").ToString
                Me.lblSomeRemoval_2.Text = .Item("工事種別_一部撤去").ToString
                Me.lblShopRelocation_2.Text = .Item("工事種別_店内移設").ToString
                Me.lblAllRemoval_2.Text = .Item("工事種別_全撤去").ToString
                Me.lblOnceRemoval_2.Text = .Item("工事種別_一時撤去").ToString
                Me.lblConChange_2.Text = .Item("工事種別_構成変更").ToString
                Me.lblConDelivery_2.Text = .Item("工事種別_構成配信").ToString
                Me.lblReInstallation_2.Text = .Item("工事種別_再設置").ToString
                Me.lblVup_2.Text = .Item("工事種別_ＶＵＰ").ToString
                Me.lblOther_2.Text = .Item("工事種別_その他").ToString
                Me.lblOtherDTL_2.Text = .Item("工事種別_その他内容").ToString
                Me.lblTmpsetDtV.Text = .Item("仮設置日").ToString
                Me.lblStestDtV.Text = .Item("総合テスト日").ToString
                Me.txtCnfrm.ppText = .Item("確認者名").ToString
                If .Item("確認者名").ToString = String.Empty Then
                    ViewState(M_VIEW_CNFM_CHARGE) = False
                Else
                    ViewState(M_VIEW_CNFM_CHARGE) = True
                End If
                Me.txtCloseDtym.ppText = .Item("締日（前半）").ToString
                If .Item("締日（後半）").ToString = String.Empty Then
                    Me.ddlCloseDt.ppDropDownList.SelectedIndex = -1
                Else
                    Me.ddlCloseDt.ppSelectedValue = .Item("締日（後半）").ToString
                End If

                Me.lblPrice1.Text = CType(.Item("ＴＢＯＸ機器費計"), Decimal).ToString("#,0")
                Me.lblPrice2.Text = CType(.Item("ＬＡＮ機器費計"), Decimal).ToString("#,0")
                Me.lblPrice3.Text = CType(.Item("基本料金計"), Decimal).ToString("#,0")
                Me.lblPrice4.Text = CType(.Item("工事費計"), Decimal).ToString("#,0")
                Me.lblPrice5.Text = CType(.Item("材料費計"), Decimal).ToString("#,0")
                Me.lblPrice6.Text = CType(.Item("その他費用計"), Decimal).ToString("#,0")
                Me.lblPrice7.Text = CType(.Item("その他調整費計"), Decimal).ToString("#,0")
                Me.lblPrice3to7.Text = CType(.Item("小計"), Decimal).ToString("#,0")
                Me.lblPrice8.Text = CType(.Item("カード受付機関連計"), Decimal).ToString("#,0")
                Me.lblPriceSum.Text = CType(.Item("合計"), Decimal).ToString("#,0")
                TryCast(Me.grvData7.Rows(0).FindControl(M_ITEM_NM), TextBox).Text =
                    .Item("その他調整金項目").ToString

                '工事区分ドロップダウンリスト生成　　'CNSINQP001-005
                msSetddlCnstCls(.Item("ＴＢＯＸシステムコード").ToString)
                '-----------------------------
                '2014/07/18 星野　ここから
                '-----------------------------
                'Me.txtCnstCls.ppText = .Item("工事区分").ToString
                Me.ddlCnstCls.ppSelectedValue = .Item("工事区分").ToString
                '-----------------------------
                '2014/07/18 星野　ここまで
                '-----------------------------

                Me.txtNotetext.ppText = .Item("備考").ToString

                ViewState(M_VIEW_EXTRA) = .Item("割増区分初期値").ToString
                ViewState(M_VIEW_SEND_STTS) = .Item("送信状況コード").ToString
                ViewState(M_VIEW_EMREQ_CLS) = .Item("緊急依頼コード").ToString
                ViewState(M_VIEW_TBOXCLS_CD) = .Item("ＴＢＯＸシステムコード").ToString
            End With
        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Throw ex
        End Try

    End Sub

    '--------------------------------
    '2014/07/30 星野　ここから
    '--------------------------------
    ''' <summary>
    ''' 緊急依頼判断処理
    ''' </summary>
    ''' <param name="ipstrCnstNo">工事依頼番号</param>
    ''' <returns>検索成功：True, 検索失敗：False</returns>
    ''' <remarks></remarks>
    Private Function mfJg_Emergency(ByVal ipstrCnstNo As String) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet

        objStack = New StackFrame

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try

                cmdDB = New SqlCommand("CNSINQP001_S13", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrCnstNo))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得したデータを設定
                With dstOrders.Tables(0)
                    If .Rows.Count > 0 Then
                        Me.lblEmreqCls.Text = .Rows(0).Item("M29_NAME").ToString
                        ViewState(M_VIEW_EMREQ_CLS) = .Rows(0).Item("M29_CODE").ToString
                    End If
                End With


                mfJg_Emergency = True
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "工事料金")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Return False
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            Return False
        End If

    End Function
    '--------------------------------
    '2014/07/30 星野　ここまで
    '--------------------------------

    ''' <summary>
    ''' 明細データ処理
    ''' </summary>
    ''' <param name="ipstrCntlNo">工事依頼番号</param>
    ''' <remarks></remarks>
    Private Sub msSet_Amount_Data(ByVal ipconDB As SqlConnection, ByVal ipstrCntlNo As String)

        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim strCd As String = String.Empty
        Dim strNm As String = String.Empty
        Dim intPr As Integer = Nothing

        objStack = New StackFrame

        Try
            cmdDB = New SqlCommand("CNSINQP001_S2", ipconDB)
            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("cntl_no", SqlDbType.NVarChar, ipstrCntlNo))
            End With

            'リストデータ取得
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            '取得したデータをリストに設定
            Me.grvList.DataSource = dstOrders.Tables(0)

            '変更を反映
            Me.grvList.DataBind()

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事料金明細")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' 初期パターン設定処理
    ''' </summary>
    ''' <param name="ipdstOrders">設定するデータ</param>
    ''' <returns>設定成功：True, 失敗：False</returns>
    ''' <remarks></remarks>
    Private Function mfSet_Amount_Default(ByVal ipdstOrders As DataSet) As Boolean

        mfSet_Amount_Default = False

        Try

            For Each rowData As DataRow In ipdstOrders.Tables(0).Rows
                If Not mfAddSelectAmount(rowData.Item("コード").ToString,
                                         rowData.Item("工事名").ToString,
                                         rowData.Item("単価").ToString,
                                         rowData.Item("数量").ToString,
                                         rowData.Item("割増区分").ToString,
                                         rowData.Item("割増区分コード").ToString,
                                         rowData.Item("振分コード").ToString) Then
                    Return False
                End If
            Next

            Return True

        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Function

    ''' <summary>
    ''' 初期パターン取得処理
    ''' </summary>
    ''' <param name="ipstrConstNo">工事依頼番号</param>
    ''' <param name="ipstrCnstCls">工事区分</param>
    ''' <param name="ipstrEmgncyFlg">緊急依頼フラグ</param>
    ''' <param name="ipstrExtraCls">初期割増区分</param>
    ''' <param name="ipstrTboxclsCd">ＴＢＯＸシステムコード</param>
    ''' <param name="opdstOrders">取得データ</param>
    ''' <returns>該当あり：True, 取得失敗／該当なし：False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_Amount_Default(ByVal ipstrConstNo As String,
                                          ByVal ipstrCnstCls As String,
                                          ByVal ipstrEmgncyFlg As String,
                                          ByVal ipstrExtraCls As String,
                                          ByVal ipstrTboxclsCd As String,
                                          ByRef opdstOrders As DataSet) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet
        objStack = New StackFrame

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("CNSINQP001_S8", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("const_no", SqlDbType.NVarChar, ipstrConstNo))
                    .Add(pfSet_Param("cnst_cls", SqlDbType.NVarChar, ipstrCnstCls))
                    .Add(pfSet_Param("emgncy_flg", SqlDbType.NVarChar, ipstrEmgncyFlg))
                    .Add(pfSet_Param("extra_cls", SqlDbType.NVarChar, ipstrExtraCls))
                    .Add(pfSet_Param("tboxcls_cd", SqlDbType.NVarChar, ipstrTboxclsCd))
                    .Add(pfSet_Param("close_ym", SqlDbType.NVarChar, If(ViewState(M_VIEW_NEWEST) Is Nothing, "", ViewState(M_VIEW_NEWEST).ToString)))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                opdstOrders = dstOrders

                If dstOrders.Tables(0).Rows.Count > 0 Then  '該当あり
                    Return True
                Else
                    Return False
                End If

            Catch ex As Exception
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Return False
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Return False
        End If

    End Function

    ''' <summary>
    ''' 項目名処理
    ''' </summary>
    ''' <param name="ipstrCntlNo">工事依頼番号</param>
    ''' <remarks></remarks>
    Private Sub msSet_Data_Nm(ByVal ipconDB As SqlConnection, ByVal ipstrCntlNo As String)

        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        '大項目名設定代区分コード
        Dim strCd() As String = {M_ITEM_CODE3,
                                 M_ITEM_CODE4,
                                 M_ITEM_CODE5,
                                 M_ITEM_CODE6,
                                 M_ITEM_CODE7,
                                 M_ITEM_CODE8}

        objStack = New StackFrame

        Try
            cmdDB = New SqlCommand("CNSINQP001_S1", ipconDB)
            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("screen_id", SqlDbType.NVarChar, M_DISP_ID))
            End With

            'リストデータ取得
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            '画面上部の表項目名設定
            msSet_Data_Nm(M_ITEM_CODEA, dstOrders.Tables(0))

            '画面下部表セット
            For zz As Integer = 0 To strCd.Length - 1
                '大項目設定
                msSet_Data_L_Nm(strCd(zz), dstOrders.Tables(0))
                '中項目名設定
                msSet_Data_M_Nm(strCd(zz), dstOrders.Tables(0))
            Next

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "項目名")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' 中項目名設定
    ''' </summary>
    ''' <param name="ipstrCd">大項目コード</param>
    ''' <param name="ipdttData">項目名データ</param>
    ''' <remarks></remarks>
    Private Sub msSet_Data_M_Nm(ByVal ipstrCd As String,
                                ByVal ipdttData As DataTable)

        Dim dttData As DataTable
        Dim dtrSetRow As DataRow
        Dim strSearch As StringBuilder

        strSearch = New StringBuilder
        strSearch.Append(String.Format("{0} = '{1}'", M_L_CLS, ipstrCd))
        strSearch.Append(String.Format(" AND '01' <= {0} AND {0} <= '98'", M_M_CLS))
        strSearch.Append(String.Format(" AND {0} = '999'", M_S_CLS))

        dttData = New DataTable
        dttData.Columns.Add(M_ITEM_NM)
        dttData.Columns.Add(M_QUANTITY)
        dttData.Columns.Add(M_PRICE)
        dttData.Columns.Add(M_L_CLS)
        dttData.Columns.Add(M_M_CLS)
        For Each dtrGetRow As DataRow In ipdttData.Select(strSearch.ToString, "表示順")
            dtrSetRow = dttData.NewRow
            dtrSetRow.Item(M_ITEM_NM) = dtrGetRow.Item(M_ITEM_NM)
            dtrSetRow.Item(M_L_CLS) = dtrGetRow.Item(M_L_CLS)
            dtrSetRow.Item(M_M_CLS) = dtrGetRow.Item(M_M_CLS)
            dttData.Rows.Add(dtrSetRow)
        Next

        'リスト設定
        Select Case ipstrCd
            Case M_ITEM_CODE3
                Me.grvData3.DataSource = dttData
                Me.grvData3.DataBind()
            Case M_ITEM_CODE4
                Me.grvData4.DataSource = dttData
                Me.grvData4.DataBind()
            Case M_ITEM_CODE5
                Me.grvData5.DataSource = dttData
                Me.grvData5.DataBind()
            Case M_ITEM_CODE6
                Me.grvData6.DataSource = dttData
                Me.grvData6.DataBind()
            Case M_ITEM_CODE8
                Me.grvData8.DataSource = dttData
                Me.grvData8.DataBind()
        End Select

    End Sub

    ''' <summary>
    ''' 大項目名設定
    ''' </summary>
    ''' <param name="ipstrCd">大項目コード</param>
    ''' <param name="ipdttData">項目名データ</param>
    ''' <remarks></remarks>
    Private Sub msSet_Data_L_Nm(ByVal ipstrCd As String,
                                ByVal ipdttData As DataTable)
        Dim dtrGetNm() As DataRow
        Dim strSearch As StringBuilder

        strSearch = New StringBuilder
        strSearch.Append(String.Format("{0} = '{1}'", M_L_CLS, ipstrCd))
        strSearch.Append(String.Format(" AND {0} = '99'", M_M_CLS))
        strSearch.Append(String.Format(" AND {0} = '999'", M_S_CLS))

        dtrGetNm = ipdttData.Select(strSearch.ToString, "表示順")
        Select Case ipstrCd
            Case M_ITEM_CODE3
                Me.lblListLName3.Text = dtrGetNm(0).Item(M_ITEM_NM)
            Case M_ITEM_CODE4
                Me.lblListLName4.Text = dtrGetNm(0).Item(M_ITEM_NM)
            Case M_ITEM_CODE5
                Me.lblListLName5.Text = dtrGetNm(0).Item(M_ITEM_NM)
            Case M_ITEM_CODE6
                Me.lblListLName6.Text = dtrGetNm(0).Item(M_ITEM_NM)
            Case M_ITEM_CODE7
                Me.lblListLName7.Text = dtrGetNm(0).Item(M_ITEM_NM)
            Case M_ITEM_CODE8
                Me.lblListLName8.Text = dtrGetNm(0).Item(M_ITEM_NM)
        End Select

    End Sub

    ''' <summary>
    ''' 画面上部の表の項目名設定
    ''' </summary>
    ''' <param name="ipstrCd">大項目コード</param>
    ''' <param name="ipdttData">項目名データ</param>
    ''' <remarks></remarks>
    Private Sub msSet_Data_Nm(ByVal ipstrCd As String,
                              ByVal ipdttData As DataTable)

        Dim dtrGetNm() As DataRow
        Dim strSearch As StringBuilder

        strSearch = New StringBuilder
        strSearch.Append(String.Format("{0} = '{1}'", M_L_CLS, ipstrCd))

        dtrGetNm = ipdttData.Select(strSearch.ToString, "表示順")
        For Each dtrData As DataRow In dtrGetNm
            Select Case dtrData.Item(M_M_CLS)
                Case "01"
                    Me.lblLName1.Text = dtrData.Item(M_ITEM_NM)
                Case "02"
                    Me.lblLName2.Text = dtrData.Item(M_ITEM_NM)
                Case "03"
                    Me.lblLName3.Text = dtrData.Item(M_ITEM_NM)
                Case "04"
                    Me.lblLName4.Text = dtrData.Item(M_ITEM_NM)
                Case "05"
                    Me.lblLName5.Text = dtrData.Item(M_ITEM_NM)
                Case "06"
                    Me.lblLName6.Text = dtrData.Item(M_ITEM_NM)
                Case "07"
                    Me.lblLName7.Text = dtrData.Item(M_ITEM_NM)
                Case "08"
                    Me.lblLName3to7.Text = dtrData.Item(M_ITEM_NM)
                Case "09"
                    Me.lblLName8.Text = dtrData.Item(M_ITEM_NM)
                Case "10"
                    Me.lblLNameSum.Text = dtrData.Item(M_ITEM_NM)
            End Select
        Next

    End Sub

    ''' <summary>
    ''' 工事区分活性／非活性判定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_Cnst_Cls()

        '参照、確認済は非活性
        If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照 _
            OrElse ViewState(M_VIEW_CNFM_CHARGE) Then
            '非活性
            '-----------------------------
            '2014/07/18 星野　ここから
            '-----------------------------
            'Me.txtCnstCls.ppEnabled = False
            Me.ddlCnstCls.ppEnabled = False
            '-----------------------------
            '2014/07/18 星野　ここまで
            '-----------------------------
            Exit Sub
        End If
        If mfGet_Amount_Default(ViewState(P_KEY),
                                ddlCnstCls.ppSelectedValue,
                                ViewState(M_VIEW_EMREQ_CLS),
                                ViewState(M_VIEW_EXTRA),
                                ViewState(M_VIEW_TBOXCLS_CD),
                                Nothing) Then
            If Me.grvList.Rows.Count = 0 Then  '明細が0件の場合、工事区分を活性
                Me.ddlCnstCls.ppEnabled = True
            Else
                Me.ddlCnstCls.ppEnabled = False
            End If
        Else    '工事区分マスタ該当なし
            If Me.txtCloseDtym.ppText <> "" Then
                Me.ddlCnstCls.ppEnabled = True
            End If
        End If

    End Sub

    '-----------------------------
    '2014/07/03 星野　ここから
    '-----------------------------
    ''' <summary>
    ''' 締め日による活性／非活性判定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_Close_YM()

        Dim txtName As TextBox = TryCast(Me.grvData7.Rows(0).FindControl(M_ITEM_NM), TextBox)

        '締め日が空の場合各入力項目等を非活性
        If Me.txtCloseDtym.ppText = "" Then
            '-----------------------------
            '2014/07/18 星野　ここから
            '-----------------------------
            'Me.txtCnstCls.ppEnabled = False
            Me.ddlCnstCls.ppEnabled = False
            '-----------------------------
            '2014/07/18 星野　ここまで
            '-----------------------------
            Me.txtCnsrCD.ppEnabled = False
            Me.txtQuantity.ppEnabled = False
            Me.ddlExtraCls.ppEnabled = False
            Me.txtNotetext.ppEnabled = False
            Me.btnAmoAdd.Enabled = False
            Me.btnAmoClear.Enabled = False
            Master.ppRigthButton4.Enabled = False
            txtName.ReadOnly = True
        End If

    End Sub
    '-----------------------------
    '2014/07/03 星野　ここまで
    '-----------------------------

    ''' <summary>
    ''' 遷移条件によって活性／非活性を設定する。
    ''' </summary>
    ''' <param name="ipshtMode">遷移条件</param>
    ''' <remarks></remarks>
    Private Sub msSet_Mode(ByVal ipshtMode As ClsComVer.E_遷移条件)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim intRtn As Integer

        Select Case ipshtMode
            Case ClsComVer.E_遷移条件.更新
                Me.txtCnfrm.ppEnabled = False            '確認者
                Me.btnCnfrm.Enabled = False              '確認ボタン

                '接続
                If clsDataConnect.pfOpen_Database(conDB) Then
                    Try
                        '工事料金明細(D28)、工事完了報告(D87)の登録確認
                        Dim cmdCheck As SqlCommand = New SqlCommand("CNSINQP001_S10", conDB)
                        With cmdCheck.Parameters
                            .Add(pfSet_Param("const_no", SqlDbType.NVarChar, ViewState(P_KEY)))         '工事依頼番号
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
                        End With

                        cmdCheck.CommandType = CommandType.StoredProcedure
                        cmdCheck.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdCheck.Parameters("retvalue").Value.ToString)
                        If intRtn = 0 Then         '工事料金明細（未登録）
                            cmdDB = New SqlCommand("CNSINQP001_S11", conDB)
                            With cmdDB.Parameters
                                'パラメータ設定
                                .Add(pfSet_Param("const_no", SqlDbType.NVarChar, ViewState(P_KEY)))
                            End With

                            'データ取得
                            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                            '締日が検収済でない場合 'CNSINQP001-004
                            If dstOrders.Tables(1).Rows(0).Item("締日").ToString >= ViewState(M_VIEW_NEWEST) Then
                                '工事のステータスが10：工事料金登録済 or 11：工事料金送信済のデータのみ確認可能
                                If dstOrders.Tables(0).Rows(0).Item("ステータス").ToString = "10" Or
                                    dstOrders.Tables(0).Rows(0).Item("ステータス").ToString = "11" Then
                                    Me.txtCnfrm.ppEnabled = True            '確認者
                                    Me.btnCnfrm.Enabled = True              '確認ボタン
                                End If
                            End If
                        End If


                    Catch ex As Exception
                        psMesBox(Me, "30006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    Finally
                        'DB切断
                        If Not clsDataConnect.pfClose_Database(conDB) Then
                            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        End If
                    End Try
                End If

                'フッターボタン
                Master.ppRigthButton1.Visible = True    'ＣＳＶ
                Master.ppRigthButton2.Visible = True    'ＰＤＦ
                Master.ppRigthButton3.Visible = True    '印刷
                Master.ppRigthButton4.Visible = True    '更新
                Master.ppLeftButton1.Visible = True     '送信
                Master.ppLeftButton2.Visible = True     '工事完了登録

                '確認済の場合は項目の変更不可
                If ViewState(M_VIEW_CNFM_CHARGE) Then   '確認済
                    Me.txtCloseDtym.ppEnabled = False       '締日（前半）
                    Me.ddlCloseDt.ppEnabled = False         '締日（後半）

                    '-----------------------------
                    '2014/07/18 星野　ここから
                    '-----------------------------
                    'Me.txtCnstCls.ppEnabled = False         '工事区分
                    Me.ddlCnstCls.ppEnabled = False         '工事区分
                    '-----------------------------
                    '2014/07/18 星野　ここまで
                    '-----------------------------

                    Me.txtCnsrCD.ppEnabled = False          'コード
                    Me.txtQuantity.ppEnabled = False        '数量
                    Me.ddlExtraCls.ppEnabled = False        '割増区分

                    Me.btnAmoClear.Enabled = False          '明細 クリアボタン
                    Me.btnAmoAdd.Enabled = False            '明細 追加ボタン
                    Me.btnAmoUpdate.Enabled = False         '明細 更新ボタン
                    Me.btnAmoDel.Enabled = False            '明細 削除ボタン 
                    'Me.grvList.Enabled = False              '明細 一覧 'CNSINQP001-003
                    Me.btnRecalculation.Enabled = False     '再計算ボタン
                    Me.txtNotetext.ppEnabled = False        '備考

                    'フッターボタン
                    Master.ppRigthButton1.Enabled = True    'ＣＳＶ
                    Master.ppRigthButton2.Enabled = True    'ＰＤＦ
                    Master.ppRigthButton4.Enabled = False   '更新

                    '送信ボタン活性制御
                    'If txtCntlNo.ppText.Substring(0, 5).ToString = "N0010" Then
                    If ViewState(P_KEY).ToString.Substring(0, 5).ToString = "N0010" Then
                        Master.ppLeftButton1.Enabled = True
                    Else
                        Master.ppLeftButton1.Enabled = False
                    End If

                Else                                    '未確認
                    Me.txtCloseDtym.ppEnabled = True        '締日（前半）
                    'If txtCntlNo.ppText.Substring(0, 5).ToString = "N0010" Then
                    If ViewState(P_KEY).ToString.Substring(0, 5).ToString = "N0010" Then
                        Me.ddlCloseDt.ppEnabled = True          '締日（後半）
                    Else
                        Me.ddlCloseDt.ppDropDownList.SelectedValue = "3"
                        Me.ddlCloseDt.ppEnabled = False         '締日（後半）
                    End If

                    Me.txtCnsrCD.ppEnabled = True           'コード
                    Me.txtQuantity.ppEnabled = True         '数量
                    Me.ddlExtraCls.ppEnabled = True         '割増区分

                    Me.btnAmoClear.Enabled = True           '明細 クリアボタン
                    Me.btnAmoAdd.Enabled = True             '明細 追加ボタン
                    Me.btnAmoUpdate.Enabled = True          '明細 更新ボタン
                    Me.btnAmoDel.Enabled = True             '明細 削除ボタン
                    'Me.grvList.Enabled = True               '明細 一覧 'CNSINQP001-003

                    Me.btnRecalculation.Enabled = True      '再計算ボタン

                    Me.txtNotetext.ppEnabled = True         '備考

                    'フッターボタン
                    Master.ppRigthButton1.Enabled = True   'ＣＳＶ
                    Master.ppRigthButton2.Enabled = False   'ＰＤＦ
                    Master.ppRigthButton4.Enabled = True    '更新
                    Master.ppLeftButton1.Enabled = False    '送信
                    '-----------------------------
                    '2014/07/07 星野　ここから
                    '-----------------------------
                    '-----------------------------
                    '2014/06/17 武　ここから
                    '-----------------------------
                    'msSet_DtlMode("1")
                    msSet_DtlMode("3")
                    '-----------------------------
                    '2014/06/17 武　ここまで
                    '-----------------------------
                    '-----------------------------
                    '2014/07/07 星野　ここまで
                    '-----------------------------
                End If

            Case ClsComVer.E_遷移条件.参照
                Me.txtCnfrm.ppEnabled = False           '確認者
                Me.btnCnfrm.Enabled = False             '確認ボタン

                Me.txtCloseDtym.ppEnabled = False       '締日（前半）
                Me.ddlCloseDt.ppEnabled = False         '締日（後半）

                '-----------------------------
                '2014/07/18 星野　ここから
                '-----------------------------
                'Me.txtCnstCls.ppEnabled = False         '工事区分
                Me.ddlCnstCls.ppEnabled = False         '工事区分
                '-----------------------------
                '2014/07/18 星野　ここまで
                '-----------------------------

                Me.txtCnsrCD.ppEnabled = False          'コード
                Me.txtQuantity.ppEnabled = False        '数量
                Me.ddlExtraCls.ppEnabled = False        '割増区分

                Me.btnAmoClear.Enabled = False          '明細 クリアボタン
                Me.btnAmoAdd.Enabled = False            '明細 追加ボタン
                Me.btnAmoUpdate.Enabled = False         '明細 更新ボタン
                Me.btnAmoDel.Enabled = False            '明細 削除ボタン
                Me.btnRecalculation.Enabled = False     '再計算ボタン

                Me.txtNotetext.ppEnabled = False        '備考

                'フッターボタン
                Master.ppRigthButton1.Visible = True    'ＣＳＶ
                Master.ppRigthButton2.Visible = True    'ＰＤＦ
                Master.ppRigthButton3.Visible = True    '印刷
                Master.ppRigthButton4.Visible = True    '更新
                Master.ppLeftButton1.Visible = True     '送信
                Master.ppLeftButton2.Visible = True     '工事完了登録

                Master.ppRigthButton1.Enabled = False   'ＣＳＶ
                Master.ppRigthButton2.Enabled = False   'ＰＤＦ
                Master.ppRigthButton4.Enabled = False   '更新
                Master.ppLeftButton1.Enabled = False    '送信
        End Select

    End Sub

    ''' <summary>
    ''' 明細データのボタン制御
    ''' </summary>
    ''' <param name="ipstrMode"></param>
    ''' <remarks></remarks>
    Private Sub msSet_DtlMode(ByVal ipstrMode As String)

        '--------------------------------
        '2014/06/23 武　ここから
        '--------------------------------
        If ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.参照 Then
            '--------------------------------
            '2014/06/23 武　ここまで
            '--------------------------------
            Select Case ipstrMode

                Case "1"
                    Me.btnAmoAdd.Enabled = True                 '追加(明細)ボタン
                    '--------------------------------
                    '2014/07/03 星野　ここから
                    '--------------------------------
                    Call msCheck_CloseDtymError()
                    '--------------------------------
                    '2014/07/03 星野　ここまで
                    '--------------------------------
                    Me.btnAmoUpdate.Enabled = False             '更新(明細)ボタン
                    Me.btnAmoDel.Enabled = False                '削除(明細)ボタン
                    If Me.btnAmoAdd.Enabled = True Then
                        Me.btnAmoClear.Enabled = True               'クリア(明細)ボタン
                    Else
                        Me.btnAmoClear.Enabled = False
                    End If
                Case "2"
                    Me.btnAmoUpdate.Enabled = True              '更新(明細)ボタン
                    Me.btnAmoDel.Enabled = True                 '削除(明細)ボタン
                    If IsPostBack Then
                        '--------------------------------
                        '2014/07/03 星野　ここから
                        '--------------------------------
                        Call msCheck_CloseDtymError()
                        '--------------------------------
                        '2014/07/03 星野　ここまで
                        '--------------------------------
                    End If
                    Me.btnAmoClear.Enabled = True               'クリア(明細)ボタン
                    Me.btnAmoAdd.Enabled = False                '追加(明細)ボタン
                Case "3"
                    Dim txtName As TextBox = TryCast(Me.grvData7.Rows(0).FindControl(M_ITEM_NM), TextBox)
                    Dim txtPrice As TextBox = TryCast(Me.grvData7.Rows(0).FindControl(M_PRICE), TextBox)
                    If Me.txtCloseDtym.ppText = "" Then
                        '-----------------------------
                        '2014/07/18 星野　ここから
                        '-----------------------------
                        'Me.txtCnstCls.ppEnabled = False
                        Me.ddlCnstCls.ppEnabled = False
                        '-----------------------------
                        '2014/07/18 星野　ここまで
                        '-----------------------------
                        Me.txtCnsrCD.ppEnabled = False
                        Me.txtQuantity.ppEnabled = False
                        Me.ddlExtraCls.ppEnabled = False
                        Me.txtNotetext.ppEnabled = False
                        Me.btnAmoAdd.Enabled = False
                        Me.btnAmoClear.Enabled = False
                        Me.btnAmoUpdate.Enabled = False
                        Me.btnAmoDel.Enabled = False
                        Master.ppRigthButton4.Enabled = False
                        txtName.ReadOnly = True
                    Else
                        '--------------------------------
                        '2014/07/18 星野　ここから
                        '--------------------------------
                        '--------------------------------
                        '2014/07/17 星野　ここから
                        '--------------------------------
                        'If grvList.Rows.Count > 0 Then
                        '    If Me.txtCnstCls.ppText <> "" Then
                        '        Me.txtCnstCls.ppEnabled = False
                        '    End If
                        'Else
                        '    Me.txtCnstCls.ppEnabled = True
                        'End If
                        If grvList.Rows.Count > 0 Then
                            If Me.ddlCnstCls.ppSelectedValue <> "" Then
                                Me.ddlCnstCls.ppEnabled = False
                            End If
                        Else
                            Me.ddlCnstCls.ppEnabled = True
                        End If
                        '--------------------------------
                        '2014/07/17 星野　ここまで
                        '--------------------------------
                        '--------------------------------
                        '2014/07/18 星野　ここまで
                        '--------------------------------
                        Me.txtCnsrCD.ppEnabled = True
                        Me.txtQuantity.ppEnabled = True
                        Me.ddlExtraCls.ppEnabled = True
                        Me.txtNotetext.ppEnabled = True
                        Me.btnAmoAdd.Enabled = True
                        Me.btnAmoClear.Enabled = True
                        Me.btnAmoUpdate.Enabled = False
                        Me.btnAmoDel.Enabled = False
                        Master.ppRigthButton4.Enabled = True
                        txtName.ReadOnly = False
                    End If
            End Select
            '--------------------------------
            '2014/06/23 武　ここから
            '--------------------------------
        Else
            Me.btnAmoClear.Enabled = True               'クリア(明細)ボタン
            Me.btnAmoAdd.Enabled = False                '追加(明細)ボタン
            Me.btnAmoUpdate.Enabled = False              '更新(明細)ボタン
            Me.btnAmoDel.Enabled = False                 '削除(明細)ボタン
        End If
        '--------------------------------
        '2014/06/23 武　ここまで
        '--------------------------------

    End Sub

    ''' <summary>
    ''' クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()

        Dim dttGrid7 As DataTable = Nothing
        Dim dtrGrid7 As DataRow = Nothing

        Me.lblSendSttsV.Text = String.Empty
        Me.lblPDFSttsV.Text = String.Empty  'CNSINQP001-002
        Me.lblTboxIDV.Text = String.Empty
        Me.lblHallNmV.Text = String.Empty
        Me.lblCnstDtV.Text = String.Empty
        Me.lblDatarcvDtV.Text = String.Empty
        Me.lblEmreqCls.Text = String.Empty
        Me.lblTboxclsNmV.Text = String.Empty
        Me.lblNew_2.Text = String.Empty
        Me.lblExpansion_2.Text = String.Empty
        Me.lblSomeRemoval_2.Text = String.Empty
        Me.lblShopRelocation_2.Text = String.Empty
        Me.lblAllRemoval_2.Text = String.Empty
        Me.lblOnceRemoval_2.Text = String.Empty
        Me.lblConChange_2.Text = String.Empty
        Me.lblConDelivery_2.Text = String.Empty
        Me.lblReInstallation_2.Text = String.Empty
        Me.lblVup_2.Text = String.Empty
        Me.lblOther_2.Text = String.Empty
        Me.lblTmpsetDtV.Text = String.Empty
        Me.lblStestDtV.Text = String.Empty
        Me.txtCnfrm.ppText = String.Empty
        Me.txtCloseDtym.ppText = String.Empty
        Me.ddlCloseDt.ppSelectedValue = String.Empty
        '-----------------------------
        '2014/07/18 星野　ここから
        '-----------------------------
        'Me.txtCnstCls.ppText = String.Empty
        'CNSINQP001-005 comment out
        'Me.ddlCnstCls.ppSelectedValue = String.Empty
        'CNSINQP001-005 End
        '-----------------------------
        '2014/07/18 星野　ここまで
        '-----------------------------
        Me.txtNotetext.ppText = String.Empty
        ViewState(M_VIEW_CNFM_CHARGE) = Nothing
        ViewState(M_VIEW_EXTRA) = Nothing
        ViewState(M_VIEW_SEND_STTS) = Nothing
        ViewState(M_VIEW_EMREQ_CLS) = Nothing

        '検索条件クリア
        msClearSearch()

        '選択明細行クリア
        msClearSelectAmount()

        '明細項目7番設定
        dttGrid7 = pfParse_DataTable(Me.grvData7)
        dtrGrid7 = dttGrid7.NewRow()
        dtrGrid7.Item(M_L_CLS) = M_ITEM_CODE7
        dtrGrid7.Item(M_M_CLS) = "01"
        dttGrid7.Rows.Add(dtrGrid7)

        Me.grvData7.DataSource = dttGrid7
        Me.grvData7.DataBind()

        '明細設定
        Me.grvList.DataSource = New Object() {}
        Me.grvList.DataBind()

        '合計金額初期化
        Me.lblPrice1.Text = "0"
        Me.lblPrice2.Text = "0"
        Me.lblPrice3.Text = "0"
        Me.lblPrice4.Text = "0"
        Me.lblPrice5.Text = "0"
        Me.lblPrice6.Text = "0"
        Me.lblPrice7.Text = "0"
        Me.lblPrice8.Text = "0"
        Me.lblPrice3to7.Text = "0"
        Me.lblPriceSum.Text = "0"

        'GridView金額、数量クリア
        msClear_GridPrm(Me.grvData3)
        msClear_GridPrm(Me.grvData4)
        msClear_GridPrm(Me.grvData5)
        msClear_GridPrm(Me.grvData6)
        msClear_GridPrm(Me.grvData7)
        msClear_GridPrm(Me.grvData8)

    End Sub

    ''' <summary>
    ''' 金額、数量を初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearPrice()
        '合計金額初期化
        Me.lblPrice1.Text = "0"
        Me.lblPrice2.Text = "0"
        Me.lblPrice3.Text = "0"
        Me.lblPrice4.Text = "0"
        Me.lblPrice5.Text = "0"
        Me.lblPrice6.Text = "0"
        Me.lblPrice7.Text = "0"
        Me.lblPrice8.Text = "0"
        Me.lblPrice3to7.Text = "0"
        Me.lblPriceSum.Text = "0"

        'GridView金額、数量クリア
        msClear_GridPrm(Me.grvData3)
        msClear_GridPrm(Me.grvData4)
        msClear_GridPrm(Me.grvData5)
        msClear_GridPrm(Me.grvData6)
        msClear_GridPrm(Me.grvData7)
        msClear_GridPrm(Me.grvData8)
    End Sub

    ''' <summary>
    ''' 画面明細から合計金額を設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_ScreenPrice()

        Dim strSymCD As String
        Dim strPrice As String
        Dim strQuantity As String
        objStack = New StackFrame

        Try
            '金額、数量初期化
            msClearPrice()

            '金額、数量設定
            For Each rowData In grvList.Rows
                strSymCD = CType(rowData.FindControl(M_SYM_CD), TextBox).Text
                strPrice = CType(rowData.FindControl(M_PRICE), TextBox).Text
                strQuantity = CType(rowData.FindControl(M_QUANTITY), TextBox).Text
                msAdd_Price(strSymCD, strPrice)
                msAdd_GridPrm(strSymCD, strPrice, strQuantity)
            Next

        Catch ex As Exception
            psMesBox(Me, "30006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' 画面明細から明細金額を設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_GridPrice()

        Dim strSymCD As String
        Dim strPrice As String
        Dim strQuantity As String
        objStack = New StackFrame

        Try
            '金額、数量初期化
            msClear_GridPrm(Me.grvData3)
            msClear_GridPrm(Me.grvData4)
            msClear_GridPrm(Me.grvData5)
            msClear_GridPrm(Me.grvData6)
            msClear_GridPrm(Me.grvData7)
            msClear_GridPrm(Me.grvData8)

            '金額、数量設定
            For Each rowData In grvList.Rows
                strSymCD = CType(rowData.FindControl(M_SYM_CD), TextBox).Text
                strPrice = CType(rowData.FindControl(M_PRICE), TextBox).Text
                strQuantity = CType(rowData.FindControl(M_QUANTITY), TextBox).Text
                msAdd_GridPrm(strSymCD, strPrice, strQuantity)
            Next

        Catch ex As Exception
            psMesBox(Me, "30006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' 項目合計の金額に加算
    ''' </summary>
    ''' <param name="ipstrSymCD">加算する項目の振分コード</param>
    ''' <param name="ipstrPrice">加算金額</param>
    ''' <remarks></remarks>
    Private Sub msAdd_Price(ByVal ipstrSymCD As String, ByVal ipstrPrice As String)

        Dim lblSetLabel As Label = Nothing
        Dim decPrice As Decimal = Nothing
        Dim decAddPrice As Decimal = Nothing
        Dim blnSyouF As Boolean = False
        objStack = New StackFrame

        If ipstrPrice = String.Empty Then
            Exit Sub
        End If
        Try
            If ipstrSymCD.Length < 1 Then
                Exit Sub
            End If
            Select Case ipstrSymCD.Substring(0, 1).PadLeft(2, "0"c)
                Case M_ITEM_CODE1
                    lblSetLabel = Me.lblPrice1
                Case M_ITEM_CODE2
                    lblSetLabel = Me.lblPrice2
                Case M_ITEM_CODE3
                    blnSyouF = True
                    lblSetLabel = Me.lblPrice3
                Case M_ITEM_CODE4
                    blnSyouF = True
                    lblSetLabel = Me.lblPrice4
                Case M_ITEM_CODE5
                    blnSyouF = True
                    lblSetLabel = Me.lblPrice5
                Case M_ITEM_CODE6
                    blnSyouF = True
                    lblSetLabel = Me.lblPrice6
                Case M_ITEM_CODE7
                    blnSyouF = True
                    lblSetLabel = Me.lblPrice7
                Case M_ITEM_CODE8
                    lblSetLabel = Me.lblPrice8
            End Select

            If lblSetLabel IsNot Nothing Then
                '金額に加算
                Decimal.TryParse(lblSetLabel.Text, decPrice)
                decAddPrice = Decimal.Parse(ipstrPrice)

                decPrice = decPrice + decAddPrice
                lblSetLabel.Text = decPrice.ToString("#,0")

                '小計に加算
                If blnSyouF Then
                    Decimal.TryParse(Me.lblPrice3to7.Text, decPrice)

                    decPrice = decPrice + decAddPrice
                    Me.lblPrice3to7.Text = decPrice.ToString("#,0")
                End If

                '合計に加算
                Decimal.TryParse(Me.lblPriceSum.Text, decPrice)

                decPrice = decPrice + decAddPrice
                Me.lblPriceSum.Text = decPrice.ToString("#,0")
            End If

        Catch ex As Exception
            psMesBox(Me, "30006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' GridView上の金額、数量に加算
    ''' </summary>
    ''' <param name="ipstrSymCD">加算する項目の振分コード</param>
    ''' <param name="ipstrPrice">加算金額</param>
    ''' <param name="ipstrQuantity">加算数量</param>
    ''' <remarks></remarks>
    Private Sub msAdd_GridPrm(ByVal ipstrSymCD As String, ByVal ipstrPrice As String, ByVal ipstrQuantity As String)

        Dim grvSetGrid As GridView = Nothing
        Dim strGridLCls As String = Nothing
        Dim strGridMCls As String = Nothing
        Dim strSetLCls As String = Nothing
        Dim strSetMCls As String = Nothing
        Dim decPrice As Decimal = Nothing
        Dim decQuantity As Decimal = Nothing
        Dim decAddPrice As Decimal = Nothing
        Dim decAddQuantity As Decimal = Nothing

        objStack = New StackFrame

        Try
            If ipstrSymCD = String.Empty Then
                Exit Sub
            End If
            '振分コードを大区分、中区分に分解
            strSetLCls = ipstrSymCD.Substring(0, 1).PadLeft(2, "0"c)        '大区分
            strSetMCls = ipstrSymCD.Substring(1, 2)                         '中区分
            '設定するGrid設定
            Select Case strSetLCls
                Case M_ITEM_CODE3
                    grvSetGrid = Me.grvData3
                Case M_ITEM_CODE4
                    grvSetGrid = Me.grvData4
                Case M_ITEM_CODE5
                    grvSetGrid = Me.grvData5
                Case M_ITEM_CODE6
                    grvSetGrid = Me.grvData6
                Case M_ITEM_CODE7
                    grvSetGrid = Me.grvData7
                Case M_ITEM_CODE8
                    grvSetGrid = Me.grvData8
            End Select

            If grvSetGrid IsNot Nothing Then
                For Each rowData In grvSetGrid.Rows
                    strGridMCls = CType(rowData.FindControl(M_M_CLS), TextBox).Text
                    If strGridMCls = strSetMCls Then   '振分コード一致
                        If ipstrPrice <> String.Empty Then
                            '金額に加算
                            Decimal.TryParse(CType(rowData.FindControl(M_PRICE), TextBox).Text, decPrice)
                            decAddPrice = Decimal.Parse(ipstrPrice)
                            decPrice = decPrice + decAddPrice
                            If decPrice = 0 Then    '0の場合は空白とする
                                CType(rowData.FindControl(M_PRICE), TextBox).Text = String.Empty
                            Else
                                CType(rowData.FindControl(M_PRICE), TextBox).Text = decPrice.ToString("#,0")
                            End If
                        End If

                        '明細項目7番は項目に数量がないため設定しない
                        If grvSetGrid.ID <> Me.grvData7.ID Then
                            If ipstrQuantity <> String.Empty Then
                                '数量に加算
                                Decimal.TryParse(CType(rowData.FindControl(M_QUANTITY), TextBox).Text, decQuantity)
                                decAddQuantity = Decimal.Parse(ipstrQuantity)
                                decQuantity = decQuantity + decAddQuantity
                                If decQuantity = 0 Then
                                    CType(rowData.FindControl(M_QUANTITY), TextBox).Text = String.Empty
                                Else
                                    CType(rowData.FindControl(M_QUANTITY), TextBox).Text = decQuantity.ToString("#,0")
                                End If
                            End If
                        End If
                    End If
                Next
            End If

        Catch ex As Exception
            psMesBox(Me, "30006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' GridView上の金額、数量クリア
    ''' </summary>
    ''' <param name="ipgrvGrid"></param>
    ''' <remarks></remarks>
    Private Sub msClear_GridPrm(ByVal ipgrvGrid As GridView)

        objStack = New StackFrame

        Try
            If ipgrvGrid IsNot Nothing Then
                For Each rowData In ipgrvGrid.Rows
                    '金額クリア
                    CType(rowData.FindControl(M_PRICE), TextBox).Text = String.Empty

                    '明細項目7番は項目に数量がないため設定しない
                    If ipgrvGrid.ID <> Me.grvData7.ID Then
                        '数量クリア
                        CType(rowData.FindControl(M_QUANTITY), TextBox).Text = String.Empty
                    End If
                Next
            End If

        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' ユーザー権限取得処理
    ''' </summary>
    ''' <param name="ipstrUserID">ユーザーＩＤ</param>
    ''' <remarks></remarks>
    Private Function mfGet_UserAuth(ByVal ipstrUserID As String) As String

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim strRet As String = String.Empty

        objStack = New StackFrame

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("ZCMPSEL021", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("userid", SqlDbType.NVarChar, ipstrUserID))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ユーザー権限")
                Else
                    strRet = dstOrders.Tables(0).Rows(0).Item("権限").ToString()
                End If

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ユーザー権限")
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

        mfGet_UserAuth = strRet

    End Function

    ''' <summary>
    ''' 明細項目7番の入力可否設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_Grid7()

        Dim txtName As TextBox
        Dim txtPrice As TextBox

        '明細項目7番未設定
        If Me.grvData7.Rows.Count <= 0 Then
            Exit Sub
        End If

        txtName = TryCast(Me.grvData7.Rows(0).FindControl(M_ITEM_NM), TextBox)
        '項目名取得失敗
        If txtName Is Nothing Then
            Exit Sub
        End If
        txtName.ReadOnly = True

        txtPrice = TryCast(Me.grvData7.Rows(0).FindControl(M_PRICE), TextBox)
        '遷移条件取得失敗
        If ViewState(P_SESSION_TERMS) Is Nothing Then
            Exit Sub
        End If

        '遷移条件が参照
        If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照 Then
            Exit Sub
        End If

        '確認済
        If ViewState(M_VIEW_CNFM_CHARGE) Then
            Exit Sub
        End If

        '金額取得失敗
        If txtPrice Is Nothing Then
            Exit Sub
        End If

        '金額なし
        If txtPrice.Text = "0" OrElse txtPrice.Text = String.Empty Then
            txtName.Text = String.Empty
            Exit Sub
        End If

        txtName.ReadOnly = False
        txtName.MaxLength = 30

    End Sub

    ''' <summary>
    ''' 明細選択行クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearSelectAmount()

        Me.txtCnsrCD.ppText = String.Empty
        Me.lblCnstNM.Text = String.Empty
        Me.lblUnitPrice.Text = String.Empty
        Me.txtQuantity.ppText = String.Empty

        Me.ddlExtraCls.ppDropDownList.SelectedIndex = -1

        ViewState(M_VIEW_SEL) = Nothing

    End Sub

    ''' <summary>
    ''' 検索条件クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearSearch()

        Me.txtCntlNo.ppText = String.Empty

    End Sub

    ''' <summary>
    ''' 引数の連番を持つ行を削除
    ''' </summary>
    ''' <param name="ipstrSeqNo">削除行の連番</param>
    ''' <returns>削除成功：True, 削除失敗/削除行なし：False</returns>
    ''' <remarks></remarks>
    Private Function mfDelSelectAmount(ByVal ipstrSeqNo As String) As Boolean

        Dim dttData As DataTable = Nothing
        Dim decOldPrice As Decimal
        Dim decOldQuantity As Decimal

        mfDelSelectAmount = False

        'GridViewから取得
        dttData = pfParse_DataTable(Me.grvList)

        '該当行削除
        For Each rowSel As DataRow In dttData.Select(M_SEQ & " = '" & ipstrSeqNo & "'")
            '該当行の減算
            Decimal.TryParse(rowSel.Item(M_PRICE).ToString, decOldPrice)
            Decimal.TryParse(rowSel.Item(M_QUANTITY).ToString, decOldQuantity)
            msAdd_Price(rowSel.Item(M_SYM_CD), (-decOldPrice).ToString)
            msAdd_GridPrm(rowSel.Item(M_SYM_CD), (-decOldPrice).ToString, (-decOldQuantity).ToString)

            '工事区分活性／非活性
            msSet_Cnst_Cls()
            '該当行削除処理
            rowSel.Delete()
            mfDelSelectAmount = True
        Next

        'データ再設定
        Me.grvList.DataSource = dttData
        Me.grvList.DataBind()

        msSet_Cnst_Cls()

    End Function

    ''' <summary>
    ''' 引数の連番を持つ行を更新
    ''' </summary>
    ''' <param name="ipstrSeqNo">更新行の連番</param>
    ''' <param name="ipstrCnsrCD">工事項目コード</param>
    ''' <param name="ipstrCnstNM">工事項目名</param>
    ''' <param name="ipdecUnitPrice">単価</param>
    ''' <param name="ipdectQuantity">数量</param>
    ''' <param name="ipstrExtraClsNM">割増区分名</param>
    ''' <param name="ipstrExtraClsCD">割増区分コード</param>
    ''' <param name="ipstrSysCD">振分区分コード</param>
    ''' <returns>更新成功：True, 更新失敗/更新行なし：False</returns>
    ''' <remarks></remarks>
    Private Function mfUpdSelectAmount(ByVal ipstrSeqNo As String,
                                       ByVal ipstrCnsrCD As String,
                                       ByVal ipstrCnstNM As String,
                                       ByVal ipdecUnitPrice As Decimal,
                                       ByVal ipdectQuantity As Decimal,
                                       ByVal ipstrExtraClsNM As String,
                                       ByVal ipstrExtraClsCD As String,
                                       ByVal ipstrSysCD As String) As Boolean

        Dim dttData As DataTable = Nothing
        Dim decPrice As Decimal = Nothing
        Dim decOldPrice As Decimal
        Dim decOldQuantity As Decimal

        objStack = New StackFrame

        mfUpdSelectAmount = False

        Try
            'GridViewから取得
            dttData = pfParse_DataTable(Me.grvList)

            '金額を計算
            decPrice = ipdecUnitPrice * ipdectQuantity

            '該当行更新
            For Each rowSel As DataRow In dttData.Select(M_SEQ & " = '" & ipstrSeqNo & "'")
                '該当行（更新前）の減算
                Decimal.TryParse(rowSel.Item(M_PRICE).ToString, decOldPrice)
                Decimal.TryParse(rowSel.Item(M_QUANTITY).ToString, decOldQuantity)
                msAdd_Price(rowSel.Item(M_SYM_CD), (-decOldPrice).ToString)
                msAdd_GridPrm(rowSel.Item(M_SYM_CD), (-decOldPrice).ToString, (-decOldQuantity).ToString)
                '該当行（更新後）の加算
                msAdd_Price(ipstrSysCD, decPrice)
                msAdd_GridPrm(ipstrSysCD, decPrice, ipdectQuantity)
                '該当行更新
                rowSel.Item(M_CNST_CD) = ipstrCnsrCD
                rowSel.Item(M_CNST_NM) = ipstrCnstNM
                rowSel.Item(M_UNIT_PRICE) = ipdecUnitPrice
                rowSel.Item(M_QUANTITY) = ipdectQuantity
                rowSel.Item(M_PRICE) = decPrice
                rowSel.Item(M_EXTRA_CLS_NM) = ipstrExtraClsNM
                rowSel.Item(M_EXTRA_CLS) = ipstrExtraClsCD
                rowSel.Item(M_SYM_CD) = ipstrSysCD
                rowSel.Item(M_UPDATE_DT) = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                rowSel.Item(M_UPDATE_USR) = User.Identity.Name
                mfUpdSelectAmount = True
            Next

            'データ再設定
            Me.grvList.DataSource = dttData
            Me.grvList.DataBind()
        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            mfUpdSelectAmount = False
        End Try

    End Function

    ''' <summary>
    ''' 明細行を追加
    ''' </summary>
    ''' <param name="ipstrCnsrCD">工事項目コード</param>
    ''' <param name="ipstrCnstNM">工事項目名</param>
    ''' <param name="ipstrUnitPrice">単価</param>
    ''' <param name="ipstrQuantity">数量</param>
    ''' <param name="ipstrExtraClsNM">割増区分名</param>
    ''' <param name="ipstrExtraClsCD">割増区分コード</param>
    ''' <param name="ipstrSysCD">振分コード</param>
    ''' <returns>追加成功：True, 追加失敗：False</returns>
    ''' <remarks></remarks>
    Private Function mfAddSelectAmount(ByVal ipstrCnsrCD As String,
                                       ByVal ipstrCnstNM As String,
                                       ByVal ipstrUnitPrice As String,
                                       ByVal ipstrQuantity As String,
                                       ByVal ipstrExtraClsNM As String,
                                       ByVal ipstrExtraClsCD As String,
                                       ByVal ipstrSysCD As String) As Boolean

        Dim decUnitPrice As Decimal
        Dim decQuantity As Decimal
        Dim blnPrice As Boolean
        Dim blnUnitPrice As Boolean = 0
        Dim blnQuantity As Boolean = 0
        Dim dttData As DataTable = Nothing
        Dim decPrice As Decimal = Nothing
        Dim dtrNew As DataRow = Nothing
        Dim decSeq As Decimal = Nothing
        Dim dtrMaxSeq() As DataRow
        Dim strSeq As String = Nothing
        objStack = New StackFrame

        Try
            'GridViewから取得
            dttData = pfParse_DataTable(Me.grvList)

            '単価変換
            blnUnitPrice = Decimal.TryParse(ipstrUnitPrice, decUnitPrice)

            '数量変換
            '-----------------------------
            '2014/07/22 星野　ここから
            '-----------------------------
            If ipstrCnsrCD = "4101" And lblConDelivery_2.Text = "●" Then
                ipstrQuantity = 0
            End If
            '-----------------------------
            '2014/07/22 星野　ここまで
            '-----------------------------

            blnQuantity = Decimal.TryParse(ipstrQuantity, decQuantity)

            '金額設定
            If blnUnitPrice And blnQuantity Then    '単価、数量が数値として有効
                blnPrice = True
                '金額を計算
                decPrice = decUnitPrice * decQuantity
            Else     '単価、数量数値以外
                blnPrice = False
                decPrice = 0
            End If
            '連番max + 1
            dtrMaxSeq = dttData.Select(M_SEQ & " = MAX(" & M_SEQ & ")")
            If dtrMaxSeq.Count > 0 Then
                Decimal.TryParse(dtrMaxSeq(0).Item(M_SEQ), decSeq)
            Else
                decSeq = 0
            End If
            strSeq = (decSeq + 1)

            If ipstrCnsrCD = "4106" And lblConChange_2.Text = "●" Then
                ipstrSysCD = "303"
            End If


            '新規行設定
            dtrNew = dttData.NewRow()
            dtrNew.Item(M_CNST_CD) = ipstrCnsrCD
            dtrNew.Item(M_CNST_NM) = ipstrCnstNM
            If blnUnitPrice Then
                dtrNew.Item(M_UNIT_PRICE) = decUnitPrice
            Else
                dtrNew.Item(M_UNIT_PRICE) = 0
            End If
            If blnQuantity Then
                dtrNew.Item(M_QUANTITY) = decQuantity
            Else
                dtrNew.Item(M_QUANTITY) = 0
            End If
            If blnPrice Then
                dtrNew.Item(M_PRICE) = decPrice
            Else
                dtrNew.Item(M_PRICE) = 0
            End If
            dtrNew.Item(M_EXTRA_CLS_NM) = ipstrExtraClsNM
            dtrNew.Item(M_EXTRA_CLS) = ipstrExtraClsCD
            dtrNew.Item(M_SYM_CD) = ipstrSysCD
            dtrNew.Item(M_SEQ) = strSeq
            dtrNew.Item(M_INSERT_DT) = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
            dtrNew.Item(M_INSERT_USR) = User.Identity.Name
            dtrNew.Item(M_UPDATE_DT) = DBNull.Value
            dtrNew.Item(M_UPDATE_USR) = DBNull.Value

            '行追加
            dttData.Rows.Add(dtrNew)

            '追加行の加算
            msAdd_Price(ipstrSysCD, decPrice)
            msAdd_GridPrm(ipstrSysCD, decPrice, decQuantity)

            'データ再設定
            Me.grvList.DataSource = dttData
            Me.grvList.DataBind()

            mfAddSelectAmount = True
        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            mfAddSelectAmount = False
        End Try

    End Function

    ''' <summary>
    ''' 工事項目取得
    ''' </summary>
    ''' <param name="ipstrCode">工事項目コード</param>
    ''' <param name="ipstrExtra_Cls">割増区分コード</param>
    ''' <param name="opdstData">工事項目</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_Construct(ByVal ipstrCode As String,
                                     ByVal ipstrExtra_Cls As String,
                                     ByRef opdstData As DataSet)
        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim drData() As DataRow
        Dim strOKNG As String

        objStack = New StackFrame

        '初期化
        conDB = Nothing
        strOKNG = Nothing
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("CNSINQP001_S8", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("const_no", SqlDbType.NVarChar, ViewState(P_KEY)))
                    .Add(pfSet_Param("cnst_cls", SqlDbType.NVarChar, "01"))
                    .Add(pfSet_Param("emgncy_flg", SqlDbType.NVarChar, ViewState(M_VIEW_EMREQ_CLS)))
                    .Add(pfSet_Param("extra_cls", SqlDbType.NVarChar, ViewState(M_VIEW_EXTRA)))
                    .Add(pfSet_Param("tboxcls_cd", SqlDbType.NVarChar, ViewState(M_VIEW_TBOXCLS_CD)))
                    '--------------------------------
                    '2014/07/04 星野　ここから
                    '--------------------------------
                    '                    .Add(pfSet_Param("close_ym", SqlDbType.NVarChar, ViewState(M_VIEW_NEWEST)))
                    .Add(pfSet_Param("close_ym", SqlDbType.NVarChar, txtCloseDtym.ppText))
                    '--------------------------------
                    '2014/07/04 星野　ここまで
                    '--------------------------------
                End With

                'リストデータ取得
                opdstData = clsDataConnect.pfGet_DataSet(cmdDB)

                drData = opdstData.Tables(0).Select("コード = '" & ipstrCode & "'")

                cmdDB = New SqlCommand("CNSINQP001_S4", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("code", SqlDbType.NVarChar, ipstrCode))
                    .Add(pfSet_Param("extra_cls", SqlDbType.NVarChar, ipstrExtra_Cls))
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                    '--------------------------------
                    '2014/07/04 星野　ここから
                    '--------------------------------
                    '                    .Add(pfSet_Param("close_ym", SqlDbType.NVarChar, ViewState(M_VIEW_NEWEST)))
                    .Add(pfSet_Param("close_ym", SqlDbType.NVarChar, txtCloseDtym.ppText))
                    '--------------------------------
                    '2014/07/04 星野　ここまで
                    '--------------------------------
                End With

                'データ取得
                opdstData = clsDataConnect.pfGet_DataSet(cmdDB)

                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "1"
                        '整合性OK()
                        mfGet_Construct = True
                    Case Else
                        '整合性エラー
                        mfGet_Construct = False
                End Select
            Catch ex As Exception
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                mfGet_Construct = False
            Finally
                'DB切断
                clsDataConnect.pfOpen_Database(conDB)
            End Try
        Else
            mfGet_Construct = False
        End If

    End Function

    ''' <summary>
    ''' 選択明細の工事名、単価更新
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_Construct()

        Dim dtsConstData As DataSet = Nothing
        Dim decUnitPrice As Decimal = Nothing
        Dim dtRow() As DataRow

        '初期化
        Me.lblCnstNM.Text = String.Empty
        Me.lblUnitPrice.Text = String.Empty
        Me.txtQuantity.ppText = String.Empty
        '工事項目取得
        If mfGet_Construct(Me.txtCnsrCD.ppText, Me.ddlExtraCls.ppSelectedValue, dtsConstData) Then
            dtRow = dtsConstData.Tables(0).Select("コード = '" & Me.txtCnsrCD.ppText & "'")

            If dtRow.Length > 0 Then
                Me.lblCnstNM.Text = dtRow(0).Item(M_CNST_NM).ToString
                Me.txtQuantity.ppText = dtRow(0).Item("数量").ToString
                If dtRow(0).Item(M_UNIT_PRICE) IsNot Nothing Then
                    Decimal.TryParse(dtRow(0).Item(M_UNIT_PRICE), decUnitPrice)
                    Me.lblUnitPrice.Text = decUnitPrice.ToString("#,0")
                Else
                    Me.lblUnitPrice.Text = String.Empty
                End If
            End If
        End If

    End Sub

    ''' <summary>
    ''' 明細項目7番のエラーチェック
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvGrid7_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvGrid7.ServerValidate

        Dim txtName As TextBox
        Dim strErrNo As String
        Dim dtrErrMes As DataRow

        args.IsValid = False

        '遷移条件が参照
        If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照 Then
            Exit Sub
        End If

        '明細項目7番未設定
        If Me.grvData7.Rows.Count <= 0 Then
            Exit Sub
        End If

        txtName = TryCast(Me.grvData7.Rows(0).FindControl(M_ITEM_NM), TextBox)
        '項目名取得失敗
        If txtName Is Nothing Then
            Exit Sub
        End If

        'エラーチェック
        strErrNo = pfCheck_TxtErr(txtName.Text, False, False, False, False, 50, String.Empty, False)
        If strErrNo <> String.Empty Then
            'エラー
            dtrErrMes = pfGet_ValMes(strErrNo, "")
            cuvGrid7.Text = dtrErrMes.Item(P_VALMES_SMES)
            cuvGrid7.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
            Exit Sub
        End If

        args.IsValid = True

    End Sub

    ''' <summary>
    ''' 明細更新処理
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ</param>
    ''' <param name="iptrnDB">トランザクション</param>
    ''' <param name="ipstrCntlNo">工事依頼番号</param>
    ''' <param name="ipstrCnstCd">工事項目コード</param>
    ''' <param name="ipstrCnstNm">工事項目名</param>
    ''' <param name="ipstrUnitPrice">単価</param>
    ''' <param name="ipstrQuantity">数量</param>
    ''' <param name="ipstrExtraCls">割増区分</param>
    ''' <param name="ipstrSymCd">振分コード</param>
    ''' <param name="ipstrPrice">金額</param>
    ''' <param name="ipstrInsertDt">登録日時</param>
    ''' <param name="ipstrInsertUsr">登録者</param>
    ''' <param name="ipstrUpdateDt">更新日時</param>
    ''' <param name="ipstrUpdateUsr">更新者</param>
    ''' <returns>ＳＱＬ実行結果</returns>
    ''' <remarks></remarks>
    Private Function mfUpdateDTL(ByVal ipconDB As SqlConnection,
                                 ByVal iptrnDB As SqlTransaction,
                                 ByVal ipstrCntlNo As String,
                                 ByVal ipstrCnstCd As String,
                                 ByVal ipstrCnstNm As String,
                                 ByVal ipstrUnitPrice As String,
                                 ByVal ipstrQuantity As String,
                                 ByVal ipstrExtraCls As String,
                                 ByVal ipstrSymCd As String,
                                 ByVal ipstrPrice As String,
                                 ByVal ipstrInsertDt As String,
                                 ByVal ipstrInsertUsr As String,
                                 ByVal ipstrUpdateDt As String,
                                 ByVal ipstrUpdateUsr As String
                                 ) As Integer

        Dim cmdDB As SqlCommand
        objStack = New StackFrame

        Try
            cmdDB = New SqlCommand("CNSINQP001_U3", ipconDB)
            cmdDB.Transaction = iptrnDB
            With cmdDB.Parameters
                .Add(pfSet_Param("cntl_no", SqlDbType.NVarChar, ipstrCntlNo))           '工事依頼番号
                .Add(pfSet_Param("cnst_cd", SqlDbType.NVarChar, ipstrCnstCd))           '工事項目コード
                .Add(pfSet_Param("cnst_nm", SqlDbType.NVarChar, ipstrCnstNm))           '工事項目名
                If ipstrUnitPrice = String.Empty Then
                    .Add(pfSet_Param("unit_price", SqlDbType.Money, DBNull.Value))      '単価
                Else
                    .Add(pfSet_Param("unit_price", SqlDbType.Money, ipstrUnitPrice))    '単価
                End If
                If ipstrQuantity = String.Empty Then
                    .Add(pfSet_Param("quantity", SqlDbType.Decimal, DBNull.Value))      '数量
                Else
                    .Add(pfSet_Param("quantity", SqlDbType.Decimal, ipstrQuantity))     '数量
                End If
                .Add(pfSet_Param("extra_cls", SqlDbType.NVarChar, ipstrExtraCls))       '割増区分
                .Add(pfSet_Param("sym_cd", SqlDbType.NVarChar, ipstrSymCd))             '振分コード
                If ipstrPrice = String.Empty Then
                    .Add(pfSet_Param("price", SqlDbType.Money, DBNull.Value))           '金額
                Else
                    .Add(pfSet_Param("price", SqlDbType.Money, ipstrPrice))             '金額
                End If

                If ipstrInsertDt = String.Empty Then
                    .Add(pfSet_Param("insert_dt", SqlDbType.NVarChar, DBNull.Value))        '登録日時
                Else
                    .Add(pfSet_Param("insert_dt", SqlDbType.NVarChar, ipstrInsertDt))       '登録日時
                End If
                If ipstrInsertUsr = String.Empty Then
                    .Add(pfSet_Param("insert_usr", SqlDbType.NVarChar, DBNull.Value))       '登録者
                Else
                    .Add(pfSet_Param("insert_usr", SqlDbType.NVarChar, ipstrInsertUsr))     '登録者
                End If
                If ipstrUpdateDt = String.Empty Then
                    .Add(pfSet_Param("update_dt", SqlDbType.NVarChar, DBNull.Value))        '更新日時
                Else
                    .Add(pfSet_Param("update_dt", SqlDbType.NVarChar, ipstrUpdateDt))       '更新日時
                End If
                If ipstrUpdateUsr = String.Empty Then
                    .Add(pfSet_Param("update_usr", SqlDbType.NVarChar, DBNull.Value))       '更新者
                Else
                    .Add(pfSet_Param("update_usr", SqlDbType.NVarChar, ipstrUpdateUsr))     '更新者
                End If
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

    '-----------------------------
    '2014/07/04 星野　ここから
    '-----------------------------
    ''' <summary>
    ''' 最新締日取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Newest()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim dstData As DataSet
        objStack = New StackFrame

        Try
            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Exit Try
            End If

            cmdDB = New SqlCommand("CNSINQP001_S12", conDB)

            'データ取得
            dstData = clsDataConnect.pfGet_DataSet(cmdDB)


            With dstData.Tables(0)
                If .Rows.Count > 0 Then
                    If .Rows(0).Item("締め年月").ToString <> "" Then
                        ViewState(M_VIEW_NEWEST) = .Rows(0).Item("締め年月").ToString
                    End If
                End If
            End With

        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            clsDataConnect.pfClose_Database(conDB)
        End Try
    End Sub
    '-----------------------------
    '2014/07/04 星野　ここまで
    '-----------------------------

    ''' <summary>
    ''' 工事区分整合性チェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheck_CnstClsDtymError()
        If ddlCnstCls.ppSelectedValue <> String.Empty _
           AndAlso Not mfGet_Amount_Default(ViewState(P_KEY),
                                    ddlCnstCls.ppSelectedValue,
                                    ViewState(M_VIEW_EMREQ_CLS),
                                    ViewState(M_VIEW_EXTRA),
                                    ViewState(M_VIEW_TBOXCLS_CD),
                                    Nothing) Then
            ddlCnstCls.psSet_ErrorNo("2002", ddlCnstCls.ppName)
        End If
    End Sub

    ''' <summary>
    ''' 締日項目のエラーチェックを行う。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheck_CloseDtymError()

        Dim conDB As SqlConnection = Nothing
        Dim txtName As TextBox = TryCast(Me.grvData7.Rows(0).FindControl(M_ITEM_NM), TextBox)
        Dim txtPrice As TextBox = TryCast(Me.grvData7.Rows(0).FindControl(M_PRICE), TextBox)

        objStack = New StackFrame

        'Page.Validate()

        '月チェック
        If txtCloseDtym.ppText.Length >= 4 _
            AndAlso (txtCloseDtym.ppText.Substring(2, 2) < "01" _
                Or txtCloseDtym.ppText.Substring(2, 2) > "12") Then
            txtCloseDtym.psSet_ErrorNo("4001", txtCloseDtym.ppName, "年月")
            '-----------------------------
            '2014/07/03 星野　ここから
            '-----------------------------
        Else

            '最新の締め年月より前の場合はエラー
            '最新の締め年月が取得できていない場合は当月より前の場合はエラー
            If txtCloseDtym.ppText < ViewState(M_VIEW_NEWEST) Then
                txtCloseDtym.psSet_ErrorNo("4001", txtCloseDtym.ppName, ViewState(M_VIEW_NEWEST) & "以降")
            End If

        End If

        'CNSINQP001-004
        'エラーがある場合は各入力項目を非活性
        'If (Page.IsValid) Then
        '    msSet_Cnst_Cls()
        '    Me.txtCnsrCD.ppEnabled = True
        '    Me.txtQuantity.ppEnabled = True
        '    Me.ddlExtraCls.ppEnabled = True
        '    Me.txtNotetext.ppEnabled = True
        '    Me.btnAmoAdd.Enabled = True
        '    Me.btnAmoClear.Enabled = True
        '    If Me.btnAmoDel.Enabled = True Then
        '        Me.btnAmoUpdate.Enabled = True
        '        Me.btnAmoAdd.Enabled = False
        '    End If
        '    Master.ppRigthButton4.Enabled = True
        '    If txtPrice.Text = String.Empty Then
        '        txtName.ReadOnly = True
        '    Else
        '        txtName.ReadOnly = False
        '    End If
        'Else
        '    '-----------------------------
        '    '2014/07/18 星野　ここから
        '    '-----------------------------
        '    'Me.txtCnstCls.ppEnabled = False
        '    Me.ddlCnstCls.ppEnabled = False
        '    '-----------------------------
        '    '2014/07/18 星野　ここまで
        '    '-----------------------------
        '    Me.txtCnsrCD.ppEnabled = False
        '    Me.txtQuantity.ppEnabled = False
        '    Me.ddlExtraCls.ppEnabled = False
        '    Me.txtNotetext.ppEnabled = False
        '    Me.btnAmoAdd.Enabled = False
        '    If Me.btnAmoAdd.Enabled = False And Me.btnAmoDel.Enabled = False Then
        '        Me.btnAmoClear.Enabled = False
        '    End If
        '    Me.btnAmoUpdate.Enabled = False
        '    Master.ppRigthButton4.Enabled = False
        '    txtName.ReadOnly = True
        'End If
        '-----------------------------
        '2014/07/03 星野　ここまで
        '-----------------------------
        'CNSINQP001-004 END

    End Sub

    ''' <summary>
    ''' 確認者項目のエラーチェックを行う。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheck_CnfrmError()

        If Not ViewState(M_VIEW_CNFM_CHARGE) Then   '未確認
            '確認者未入力チェック
            If txtCnfrm.ppText = String.Empty Then
                txtCnfrm.psSet_ErrorNo("5001", txtCnfrm.ppName)
            End If
        End If

        If txtCnfrm.ppText <> String.Empty Then  '確認解除以外
            '整合性チェック
            If Not mfCheck_Employee(Me.txtCnfrm.ppText) Then
                txtCnfrm.psSet_ErrorNo("2002", txtCnfrm.ppName)
            End If
        End If

    End Sub

    ''' <summary>
    ''' 社員名の整合性チェック
    ''' </summary>
    ''' <param name="ipstrName">チェックする社員名</param>
    ''' <returns>True:OK, False:NG</returns>
    ''' <remarks></remarks>
    Private Function mfCheck_Employee(ByVal ipstrName As String) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim dstData As DataSet

        objStack = New StackFrame

        Try
            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                mfCheck_Employee = False
                Exit Try
            End If

            cmdDB = New SqlCommand("ZCMPSEL031", conDB)
            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("TRADER_CD", SqlDbType.NVarChar, M_CNFRM(0)))              '業者コード
                If M_CNFRM(1) = Nothing Then
                    .Add(pfSet_Param("AUTH_CLS_FROM", SqlDbType.NVarChar, "0"))             '権限区分
                Else
                    .Add(pfSet_Param("AUTH_CLS_FROM", SqlDbType.NVarChar, M_CNFRM(1)))      '権限区分
                End If
                If M_CNFRM(2) = Nothing Then
                    .Add(pfSet_Param("AUTH_CLS_TO", SqlDbType.NVarChar, "9"))               '権限区分
                Else
                    .Add(pfSet_Param("AUTH_CLS_TO", SqlDbType.NVarChar, M_CNFRM(2)))        '権限区分
                End If
            End With

            'データ取得
            dstData = clsDataConnect.pfGet_DataSet(cmdDB)

            If dstData.Tables(0).Select(String.Format("社員名 = '{0}'", ipstrName)).Count > 0 Then
                mfCheck_Employee = True
            Else
                mfCheck_Employee = False
            End If

        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            mfCheck_Employee = False
        Finally
            'DB切断
            clsDataConnect.pfClose_Database(conDB)
        End Try

    End Function

    ''' <summary>
    ''' 工事完了報告明細(TOMAS)追加／更新
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ</param>
    ''' <param name="iptrnDB">トランザクション</param>
    ''' <param name="ipstrCntlNo">工事依頼番号</param>
    ''' <returns>ＳＱＬ実行結果</returns>
    ''' <remarks></remarks>
    Private Function mfSet_Cnstbreak(ByVal ipconDB As SqlConnection,
                                     ByVal iptrnDB As SqlTransaction,
                                     ByVal ipstrCntlNo As String) As Integer

        Dim cmdDB As SqlCommand = Nothing

        objStack = New StackFrame

        Try
            '工事完了報告明細(TOMAS)追加／更新
            cmdDB = New SqlCommand("CNSINQP001_U4", ipconDB)
            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("cntl_no", SqlDbType.NVarChar, ipstrCntlNo))               '工事依頼番号
                .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))         'ユーザーＩＤ
                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
            End With

            cmdDB.Transaction = iptrnDB

            '工事完了報告明細(TOMAS)
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
    ''' 工事料金明細(TOMAS)追加／更新
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ</param>
    ''' <param name="iptrnDB">トランザクション</param>
    ''' <param name="ipstrCntlNo">工事依頼番号</param>
    ''' <returns>ＳＱＬ実行結果</returns>
    ''' <remarks></remarks>
    Private Function mfSet_Cnstamnt(ByVal ipconDB As SqlConnection,
                                    ByVal iptrnDB As SqlTransaction,
                                    ByVal ipstrCntlNo As String) As Integer

        Dim cmdDB As SqlCommand = Nothing

        objStack = New StackFrame

        Try
            '工事料金明細(TOMAS)追加／更新
            cmdDB = New SqlCommand("CNSINQP001_U5", ipconDB)
            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("cntl_no", SqlDbType.NVarChar, ipstrCntlNo))               '工事依頼番号
                .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))         'ユーザーＩＤ
                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
            End With

            cmdDB.Transaction = iptrnDB

            '工事料金明細(TOMAS)
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
    ''' 工事料金明細ＣＳＶデータ取得
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ</param>
    ''' <param name="iptrnDB">トランザクション</param>
    ''' <param name="ipstrCntlNo">工事依頼番号</param>
    ''' <returns>ＳＱＬ実行結果</returns>
    ''' <remarks></remarks>
    Private Function mfGet_Cnstamnt(ByVal ipconDB As SqlConnection,
                                    ByVal iptrnDB As SqlTransaction,
                                    ByVal ipstrCntlNo As String,
                                    ByVal opData As DataSet) As Integer

        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet

        objStack = New StackFrame

        Try
            '工事完了報告明細(TOMAS)取得
            cmdDB = New SqlCommand("CNSINQP001_S5", ipconDB)
            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("cntl_no", SqlDbType.NVarChar, ipstrCntlNo))               '工事依頼番号
            End With

            cmdDB.Transaction = iptrnDB

            '工事完了報告明細(TOMAS)
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
    ''' 送信フォルダ、バックアップフォルダにファイルを移動
    ''' </summary>
    ''' <param name="ipstrDetailNm">工事完了報告明細ファイル名</param>
    ''' <param name="ipstrDetailBKNm">工事完了報告明細バックアップファイル名</param>
    ''' <param name="ipstrDetailAdd">工事完了報告明細コピー元のサーバアドレス</param>
    ''' <param name="ipstrDetailFolder">工事完了報告明細コピー元のフォルダ</param>
    ''' <param name="ipstrFeeNm">工事料金明細ファイル名</param>
    ''' <param name="ipstrFeeBKNm">工事料金明細バックアップファイル名</param>
    ''' <param name="ipstrFeeAdd">工事料金明細コピー元のサーバアドレス</param>
    ''' <param name="ipstrFeeFolder">工事料金明細コピー元のフォルダ</param>
    ''' <returns>1:正常 1以外:エラー</returns>
    ''' <remarks>失敗時は全ファイル削除</remarks>
    Private Function mfFile(ByVal ipstrDetailNm As String,
                            ByVal ipstrDetailBKNm As String,
                            ByVal ipstrDetailAdd As String,
                            ByVal ipstrDetailFolder As String,
                            ByVal ipstrFeeNm As String,
                            ByVal ipstrFeeBKNm As String,
                            ByVal ipstrFeeAdd As String,
                            ByVal ipstrFeeFolder As String) As Integer

        Dim strDetailAdd As String = Nothing
        Dim strDetailFolder As String = Nothing
        Dim strDetailBkAdd As String = Nothing
        Dim strDetailBkFolder As String = Nothing
        Dim strFeeAdd As String = Nothing
        Dim strFeeFolder As String = Nothing
        Dim strFeeBkAdd As String = Nothing
        Dim strFeeBkFolder As String = Nothing

        objStack = New StackFrame

        '工事完了報告明細送信先取得
        If pfGetPreservePlace("0273CS", strDetailAdd, strDetailFolder) <> 0 Then
            psMesBox(Me, "20001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)
            Return -1
        End If

        '工事完了報告明細バックアップ先取得
        If pfGetPreservePlace("0273CB", strDetailBkAdd, strDetailBkFolder) <> 0 Then
            psMesBox(Me, "20001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)
            Return -1
        End If

        '工事料金明細送信先取得
        If pfGetPreservePlace("0272CS", strFeeAdd, strFeeFolder) <> 0 Then
            psMesBox(Me, "20001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)
            Return -1
        End If

        '工事料金明細バックアップ先取得
        If pfGetPreservePlace("0272CB", strFeeBkAdd, strFeeBkFolder) <> 0 Then
            psMesBox(Me, "20001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_FEE_NM & "、" & M_DETAIL_NM)
            Return -1
        End If

        Try
            '工事完了報告明細送信フォルダにコピー
            System.IO.File.Copy("\\" & ipstrDetailAdd & "\" & ipstrDetailFolder & "\" & ipstrDetailNm,
                                "\\" & strDetailAdd & "\" & strDetailFolder & "\" & ipstrDetailNm)
            '工事完了報告明細バックアップフォルダに移動
            System.IO.File.Move("\\" & ipstrDetailAdd & "\" & ipstrDetailFolder & "\" & ipstrDetailNm,
                                "\\" & strDetailBkAdd & "\" & strDetailBkFolder & "\" & ipstrDetailBKNm)


            '工事料金明細送信フォルダにコピー
            System.IO.File.Copy("\\" & ipstrFeeAdd & "\" & ipstrFeeFolder & "\" & ipstrFeeNm,
                                "\\" & strFeeAdd & "\" & strFeeFolder & "\" & ipstrFeeNm)
            '工事料金明細バックアップフォルダに移動
            System.IO.File.Move("\\" & ipstrFeeAdd & "\" & ipstrFeeFolder & "\" & ipstrFeeNm,
                                "\\" & strFeeBkAdd & "\" & strFeeBkFolder & "\" & ipstrFeeBKNm)

            '正常終了
            Return 0
        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            'エラー時ファイル削除
            pfDeleteFile(strDetailAdd, strDetailFolder, ipstrDetailNm)          '工事完了報告明細送信フォルダ
            pfDeleteFile(strDetailBkAdd, strDetailBkFolder, ipstrDetailBKNm)    '工事完了報告明細バックアップフォルダ
            pfDeleteFile(strFeeAdd, strFeeFolder, ipstrFeeNm)                   '工事料金明細送信フォルダ
            pfDeleteFile(strFeeBkAdd, strFeeBkFolder, ipstrFeeBKNm)             '工事料金明細バックアップフォルダ
            pfDeleteFile(ipstrDetailAdd, ipstrDetailFolder, ipstrDetailNm)      '工事完了報告明細作業領域
            pfDeleteFile(ipstrFeeAdd, ipstrFeeFolder, ipstrFeeNm)               '工事料金明細作業領域

            Return -1
        End Try

    End Function

    '-----------------------------
    '2014/04/28 土岐　ここから
    '-----------------------------
    ' ''' <summary>
    ' ''' 工事完了報告明細(SPCDB)送信更新
    ' ''' </summary>
    ' ''' <param name="ipconDB">ＤＢ</param>
    ' ''' <param name="iptrnDB">トランザクション</param>
    ' ''' <param name="ipstrCntlNo">工事依頼番号</param>
    ' ''' <param name="ipstrFileNm">ファイル名</param>
    ' ''' <returns>ＳＱＬ実行結果</returns>
    ' ''' <remarks></remarks>
    'Private Function mfSet_TrnSPCCnstbreak(ByVal ipconDB As SqlConnection,
    '                                       ByVal iptrnDB As SqlTransaction,
    '                                       ByVal ipstrCntlNo As String,
    '                                       ByVal ipstrFileNm As String) As Integer
    '    Dim cmdDB As SqlCommand = Nothing
    '    '--------------------------------
    '    '2014/04/14 星野　ここから
    '    '--------------------------------
    '    objStack = New StackFrame
    '    '--------------------------------
    '    '2014/04/14 星野　ここまで
    '    '--------------------------------
    '    Try
    '        '工事完了報告明細(TOMAS)追加／更新
    '        cmdDB = New SqlCommand("CNSINQP001_U6", ipconDB)
    '        With cmdDB.Parameters
    '            'パラメータ設定
    '            .Add(pfSet_Param("cntl_no", SqlDbType.NVarChar, ipstrCntlNo))               '工事依頼番号
    '            .Add(pfSet_Param("file_nm", SqlDbType.NVarChar, ipstrCntlNo))               'ファイル名
    '            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))         'ユーザーＩＤ
    '            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
    '        End With

    '        cmdDB.Transaction = iptrnDB

    '        '工事完了報告明細(TOMAS)
    '        cmdDB.CommandType = CommandType.StoredProcedure
    '        cmdDB.ExecuteNonQuery()

    '        'ストアド戻り値
    '        Return Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
    '    Catch ex As Exception
    '        '--------------------------------
    '        '2014/04/14 星野　ここから
    '        '--------------------------------
    '        'ログ出力
    '        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
    '                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
    '        '--------------------------------
    '        '2014/04/14 星野　ここまで
    '        '--------------------------------
    '        Return -1
    '    End Try

    'End Function

    ''' <summary>
    ''' 工事料金明細(SPCDB)送信更新 'CNSINQP001-001 有効化
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ</param>
    ''' <param name="iptrnDB">トランザクション</param>
    ''' <param name="ipstrCntlNo">工事依頼番号</param>
    ''' <returns>ＳＱＬ実行結果</returns>
    ''' <remarks></remarks>
    Private Function mfSet_TrnSPCCnstamnt(ByVal ipconDB As SqlConnection,
                                          ByVal iptrnDB As SqlTransaction,
                                          ByVal ipstrCntlNo As String) As Integer

        Dim cmdDB As SqlCommand = Nothing

        objStack = New StackFrame

        Try
            '工事料金明細(SPCDB)送信更新
            cmdDB = New SqlCommand("CNSINQP001_U7", ipconDB)
            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("cntl_no", SqlDbType.NVarChar, ipstrCntlNo))               '工事依頼番号
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

    ' ''' <summary>
    ' ''' 工事完了報告明細(TOMAS)送信更新
    ' ''' </summary>
    ' ''' <param name="ipconDB">ＤＢ</param>
    ' ''' <param name="iptrnDB">トランザクション</param>
    ' ''' <param name="ipstrCntlNo">工事依頼番号</param>
    ' ''' <param name="ipstrFileNm">ファイル名</param>
    ' ''' <returns>ＳＱＬ実行結果</returns>
    ' ''' <remarks></remarks>
    'Private Function mfSet_TrnCnstbreak(ByVal ipconDB As SqlConnection,
    '                                    ByVal iptrnDB As SqlTransaction,
    '                                    ByVal ipstrCntlNo As String,
    '                                    ByVal ipstrFileNm As String) As Integer
    '    Dim cmdDB As SqlCommand = Nothing
    '    '--------------------------------
    '    '2014/04/14 星野　ここから
    '    '--------------------------------
    '    objStack = New StackFrame
    '    '--------------------------------
    '    '2014/04/14 星野　ここまで
    '    '--------------------------------
    '    Try
    '        '工事完了報告明細(TOMAS)追加／更新
    '        cmdDB = New SqlCommand("CNSINQP001_U8", ipconDB)
    '        With cmdDB.Parameters
    '            'パラメータ設定
    '            .Add(pfSet_Param("cntl_no", SqlDbType.NVarChar, ipstrCntlNo))               '工事依頼番号
    '            .Add(pfSet_Param("file_nm", SqlDbType.NVarChar, ipstrCntlNo))               'ファイル名
    '            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))         'ユーザーＩＤ
    '            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
    '        End With

    '        cmdDB.Transaction = iptrnDB

    '        '工事完了報告明細(TOMAS)
    '        cmdDB.CommandType = CommandType.StoredProcedure
    '        cmdDB.ExecuteNonQuery()

    '        'ストアド戻り値
    '        Return Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
    '    Catch ex As Exception
    '        '--------------------------------
    '        '2014/04/14 星野　ここから
    '        '--------------------------------
    '        'ログ出力
    '        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
    '                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
    '        '--------------------------------
    '        '2014/04/14 星野　ここまで
    '        '--------------------------------
    '        Return -1
    '    End Try

    'End Function

    ' ''' <summary>
    ' ''' 工事料金明細(TOMAS)送信更新
    ' ''' </summary>
    ' ''' <param name="ipconDB">ＤＢ</param>
    ' ''' <param name="iptrnDB">トランザクション</param>
    ' ''' <param name="ipstrCntlNo">工事依頼番号</param>
    ' ''' <param name="ipstrFileNm">ファイル名</param>
    ' ''' <returns>ＳＱＬ実行結果</returns>
    ' ''' <remarks></remarks>
    'Private Function mfSet_TrnCnstamnt(ByVal ipconDB As SqlConnection,
    '                                   ByVal iptrnDB As SqlTransaction,
    '                                   ByVal ipstrCntlNo As String,
    '                                   ByVal ipstrFileNm As String) As Integer
    '    Dim cmdDB As SqlCommand = Nothing
    '    '--------------------------------
    '    '2014/04/14 星野　ここから
    '    '--------------------------------
    '    objStack = New StackFrame
    '    '--------------------------------
    '    '2014/04/14 星野　ここまで
    '    '--------------------------------
    '    Try
    '        '工事料金明細(TOMAS)追加／更新
    '        cmdDB = New SqlCommand("CNSINQP001_U9", ipconDB)
    '        With cmdDB.Parameters
    '            'パラメータ設定
    '            .Add(pfSet_Param("cntl_no", SqlDbType.NVarChar, ipstrCntlNo))               '工事依頼番号
    '            .Add(pfSet_Param("file_nm", SqlDbType.NVarChar, ipstrCntlNo))               'ファイル名
    '            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))         'ユーザーＩＤ
    '            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
    '        End With

    '        cmdDB.Transaction = iptrnDB

    '        '工事料金明細(TOMAS)
    '        cmdDB.CommandType = CommandType.StoredProcedure
    '        cmdDB.ExecuteNonQuery()

    '        'ストアド戻り値
    '        Return Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
    '    Catch ex As Exception
    '        '--------------------------------
    '        '2014/04/14 星野　ここから
    '        '--------------------------------
    '        'ログ出力
    '        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
    '                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
    '        '--------------------------------
    '        '2014/04/14 星野　ここまで
    '        '--------------------------------
    '        Return -1
    '    End Try

    'End Function
    '-----------------------------
    '2014/04/28 土岐　ここまで
    '-----------------------------

    ''' <summary>
    ''' 帳票用のデータを取得
    ''' </summary>
    ''' <param name="ipstrCntlNo">工事依頼番号</param>
    ''' <param name="opdttMainDara">メイン</param>
    ''' <param name="opdttData1">内訳</param>
    ''' <param name="opdttData2">店内設置台数</param>
    ''' <returns>1:正常 1以外:エラー</returns>
    ''' <remarks></remarks>
    Private Function mfGet_PDFData(ByVal ipstrCntlNo As String,
                                   ByRef opdttMainDara As DataTable,
                                   ByRef opdttData1 As DataTable,
                                   ByRef opdttData2 As DataTable) As Integer

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dtvData As DataView
        Dim dstOrders As New DataSet
        Dim counter As Integer = 0

        objStack = New StackFrame

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try

                cmdDB = New SqlCommand("CNSINQP001_S9", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("cntl_no", SqlDbType.NVarChar, ipstrCntlNo))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'データセット
                opdttMainDara = dstOrders.Tables(0)
                opdttData1 = dstOrders.Tables(1).Clone()
                dtvData = New DataView(dstOrders.Tables(1))
                dtvData.Sort() = "M45_DISP_ORDER ASC"
                For Each drv As DataRowView In dtvData
                    counter = counter + 1
                    If counter = 36 Then
                        '空行を入れます"XX"はレポート内の表示制御用ダミーデータ
                        Dim rowData As DataRow = opdttData1.NewRow
                        rowData("M45_DISP_ORDER") = 99
                        rowData("M45_M_CLS") = "XX"
                        rowData("M45_CODE") = ""
                        rowData("名称") = ""
                        rowData("数量") = 0
                        rowData("金額") = 0
                        rowData("名称") = ""
                        rowData("工事依頼番号") = ""
                        opdttData1.Rows.Add(rowData)
                    End If
                    opdttData1.ImportRow(drv.Row)
                Next

                counter = 0
                opdttData2 = dstOrders.Tables(2).Clone()
                dtvData = New DataView(dstOrders.Tables(2))
                dtvData.Sort() = "M45_DISP_ORDER ASC"
                For Each drv As DataRowView In dtvData
                    counter = counter + 1
                    If counter = 25 Then
                        '空行を入れます"XX"はレポート内の表示制御用ダミーデータ
                        Dim rowData As DataRow = opdttData2.NewRow
                        rowData("M45_DISP_ORDER") = 99
                        rowData("工事依頼番号") = ""
                        rowData("名称") = ""
                        rowData("工事前") = 0
                        rowData("移設") = 0
                        rowData("撤去") = 0
                        rowData("新設") = 0
                        rowData("撤去設") = 0
                        rowData("工事後") = 0
                        opdttData2.Rows.Add(rowData)
                    End If
                    opdttData2.ImportRow(drv.Row)
                Next

                Return 0
            Catch ex As Exception
                psMesBox(Me, "10002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器代金及び工事料金明細書")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Return -1
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Return -1
        End If

    End Function

    ''' <summary>
    ''' 工事依頼書兼仕様書 更新
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ</param>
    ''' <param name="iptrnDB">トランザクション</param>
    ''' <param name="ipstrCntlNo">工事依頼番号</param>
    ''' <returns>ＳＱＬ実行結果</returns>
    ''' <remarks></remarks>
    Private Function mfSet_Cnstrecspec(ByVal ipconDB As SqlConnection,
                                    ByVal iptrnDB As SqlTransaction,
                                    ByVal ipstrCntlNo As String) As Integer

        Dim cmdDB As SqlCommand = Nothing

        objStack = New StackFrame

        Try
            '工事依頼書兼仕様書 更新
            cmdDB = New SqlCommand("CNSINQP001_U10", ipconDB)
            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrCntlNo))               '工事依頼番号
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
    ''' 明細グリッドのボタン活性制御 CNSINQP001-003
    ''' </summary>
    Private Sub msSet_GridBtnEnbl(ByVal blnEnbl As Boolean)

        '行ループ
        For Each grvRow As GridViewRow In Me.grvList.Rows
            '最初のセルのコントロール参照
            If TypeOf grvRow.Cells(0).Controls(0) Is Button Then
                'ボタン活性制御
                DirectCast(grvRow.Cells(0).Controls(0), Button).Enabled = blnEnbl
            End If
        Next

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（工事区分マスタ） CNSINQP001-005
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlCnstCls(ByVal strSystemCd As String)

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        Else
            Try
                objCmd = New SqlCommand("CNSINQP001_S14", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '保守管理番号
                    .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, strSystemCd))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                Me.ddlCnstCls.ppDropDownList.Items.Clear()
                Me.ddlCnstCls.ppDropDownList.DataSource = objDs.Tables(0)
                Me.ddlCnstCls.ppDropDownList.DataTextField = "項目名"
                Me.ddlCnstCls.ppDropDownList.DataValueField = "工事区分コード"
                Me.ddlCnstCls.ppDropDownList.DataBind()
                Me.ddlCnstCls.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing))  '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "工事区分マスタ一覧取得")
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
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        End If

    End Sub

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
