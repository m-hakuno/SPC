'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜サポートセンタ業務＞
'*　処理名　　：　ブラックボックス調査報告書
'*　ＰＧＭＩＤ：　BBPUPDP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.02.03　：　後藤
'********************************************************************************************************************************
'---------------------------------------------------------------------------------------------------------------------------------
'番号　　　　　｜　日付　　　　　｜　名前　　｜　備考
'---------------------------------------------------------------------------------------------------------------------------------
'BBPUPDP001-001　　2015/07/17　　　　栗原　　　TBOXID変更時、型式番号のドロップダウンリストを再設定するよう変更　
'BBPUPDP001_002    2016/01/25        武         FS移転に伴う帳票修正
'BBPUPDP001_003    2017/04/10        加賀       修理依頼Noの入力形式を変更(先頭数字を許容)

#Region "インポート定義"
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
Imports System.Globalization
'排他制御用
Imports SPC.ClsCMExclusive
#End Region

Public Class BBPUPDP001

#Region "継承定義"
    Inherits System.Web.UI.Page
#End Region

#Region "定数定義"
    ''' <summary>画面ID</summary>
    Private Const M_DISP_ID = P_FUN_BBP & P_SCR_UPD & P_PAGE & "001"

    ''' <summary>共有情報(ＴＢＯＸＩＤ)</summary>
    Private Const P_TBOXID As String = "ＴＢＯＸＩＤ"
    ''' <summary>共有情報(ＳＹＳＴＥＭ)</summary>
    Private Const P_System As String = "ＳＹＳＴＥＭ"
    ''' <summary>画面名</summary>
    Private Const sCnsDispNm As String = "ブラックボックス調査報告書"
    ''' <summary>更新エラー</summary>
    Private Const sCnsErr_00001 As String = "00001"
    ''' <summary>削除エラー</summary>
    Private Const sCnsErr_00002 As String = "00002"
    ''' <summary>検索エラー</summary>
    Private Const sCnsErr_00004 As String = "00004"
    ''' <summary>接続エラー</summary>
    Private Const sCnsErr_00005 As String = "00005"
    ''' <summary>切断エラー</summary>
    Private Const sCnsErr_00006 As String = "00006"
    ''' <summary>印刷エラー</summary>
    Private Const sCnsErr_10001 As String = "10001"
    ''' <summary>セッションエラー</summary>
    Private Const sCnsErr_20004 As String = "20004"
    ''' <summary>更新完了</summary>
    Private Const sCnsInfo_00001 As String = "00001"
    ''' <summary>削除完了</summary>
    Private Const sCnsInfo_00002 As String = "00002"
    ''' <summary>更新確認</summary>
    Private Const sCnsInfo_00004 As String = "00004"
    ''' <summary>削除確認</summary>
    Private Const sCnsInfo_00005 As String = "00005"
    ''' <summary>印刷確認</summary>
    Private Const sCnsInfo_10002 As String = "10002"
    ''' <summary>進捗ステータスマスタ取得</summary>
    Private Const sCnsSql_id015 As String = "ZCMPSEL015"
    ''' <summary>コントロールID''' </summary>
    Private Const sCnsOrgn_Lno As String = "cphMainContent_txtOrgn_Lno_txtTextBox"
    Private Const sCnsOrgn_Rno As String = "cphMainContent_txtOrgn_Rno_txtTextBox"
    Private Const sCnsOrgn_Pay As String = "cphMainContent_txtOrgn_Pay_txtTextBox"
    Private Const sCnsOrgn_Consumer As String = "cphMainContent_txtOrgn_Consumer_txtTextBox"
    Private Const sCnsDpl_Lno As String = "cphMainContent_txtDpl_Lno_txtTextBox"
    Private Const sCnsDpl_Rno As String = "cphMainContent_txtDpl_Rno_txtTextBox"
    Private Const sCnsDpl_Pay As String = "cphMainContent_txtDpl_Pay_txtTextBox"
    Private Const sCnsDpl_Consumer As String = "cphMainContent_txtDpl_Consumer_txtTextBox"
    Private Const sCnsTbox_Bb As String = "cphMainContent_txtTbox_Bb_txtTextBox"
    Private Const sCnsTbox_Receipt As String = "cphMainContent_txtTbox_Receipt_txtTextBox"
    Private Const sCnsTbox_Cnsmp As String = "cphMainContent_txtTbox_Cnsmp_txtTextBox"
#End Region

#Region "変数定義"
    Dim objStack As StackFrame
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom
    Dim clsExc As New ClsCMExclusive

#End Region

#Region "イベントプロシージャ"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'イベント設定.
        Call msSet_Action()

        If Not IsPostBack Then '初回表示.

            '確認メッセージ設定.
            Master.ppRigthButton3.OnClientClick = pfGet_OCClickMes(sCnsInfo_10002, ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, sCnsDispNm)
            Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes(sCnsInfo_00005, ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, sCnsDispNm)
            Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes(sCnsInfo_00004, ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, sCnsDispNm)

            Dim strKey As String = String.Empty    'ＢＢ調報Ｎｏ.
            Dim strId As String = String.Empty
            Dim strTerms As String = String.Empty

            'セッション情報取得チェック.
            If Session(P_SESSION_TERMS) Is Nothing Or Session(P_KEY) Is Nothing Then
                psMesBox(Me, sCnsErr_20004, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                '排他削除
                clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                psClose_Window(Me)
                Return
            End If

            'セッション項目取得
            ViewState(P_KEY) = DirectCast(Session(P_KEY), String())(0)
            ViewState(P_SESSION_OLDDISP) = Session(P_SESSION_OLDDISP)
            ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)

            'ビューステート項目取得
            strKey = ViewState(P_KEY)
            strId = ViewState(P_SESSION_OLDDISP)
            strTerms = ViewState(P_SESSION_TERMS)

            'プログラムＩＤ、画面名設定.
            Master.ppProgramID = M_DISP_ID
            Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

            'パンくずリスト設定.
            Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

            '排他情報削除用の登録年月日時刻をマスタページのコントロールに登録.
            If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then
                Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)
            End If

            Select Case strTerms.ToString.Trim
                Case ClsComVer.E_遷移条件.参照

                    'コントロール初期化.
                    Call msClearScreen()

                    'ドロップダウンリスト生成.
                    If mfSet_DropDownList() = False Then
                        psClose_Window(Me)
                        Return
                    End If

                    'ブラックボックス調査報告書情報取得処理.
                    If mfGet_Data(strKey, 0) = False Then
                        psClose_Window(Me)
                        Return
                    End If
                    'BBPUPDP001-001
                    setDDLModel()
                    Me.ddlMente_No.SelectedValue = ViewState("katasiki")
                    'BBPUPDP001-001
                    '活性制御.
                    msEnableScreen(strTerms)


                Case ClsComVer.E_遷移条件.更新

                    'コントロール初期化.
                    Call msClearScreen()

                    'ドロップダウンリスト生成.
                    If mfSet_DropDownList() = False Then
                        '排他削除
                        clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                        psClose_Window(Me)
                        Return
                    End If

                    'ブラックボックス調査報告書情報取得処理.
                    If mfGet_Data(strKey, 0) = False Then
                        '排他削除
                        clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                        psClose_Window(Me)
                        Return
                    End If

                    'BBPUPDP001-001
                    setDDLModel()
                    Me.ddlMente_No.SelectedValue = ViewState("katasiki")
                    'BBPUPDP001-001

                    '活性制御.
                    msEnableScreen(strTerms)

                Case ClsComVer.E_遷移条件.登録

                    'コントロール初期化.
                    Call msClearScreen()

                    'ドロップダウンリスト生成.
                    If mfSet_DropDownList() = False Then
                        psClose_Window(Me)
                        Return
                    End If
                    'BBPUPDP001-001
                    setDDLModel()
                    Me.ddlMente_No.SelectedValue = ViewState("katasiki")
                    'BBPUPDP001-001
                    '活性制御.
                    msEnableScreen(strTerms)

            End Select

        End If



    End Sub

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
                Master.ppRigthButton1.Enabled = False
                Master.ppRigthButton2.Enabled = False
            Case "NGC"
        End Select

    End Sub

    ''' <summary>
    ''' 元に戻すボタン押下時処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnReset_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        'ブラックボックス調査報告書情報取得処理.
        Call mfGet_Data(Me.lblBbinvst_No.Text, 1)

        'BBPUPDP001-001
        Me.ddlMente_No.SelectedValue = ""
        setDDLModel()
        Me.ddlMente_No.SelectedValue = ViewState("katasiki")
        'BBPUPDP001-001 END

        '終了ログ出力
        psLogEnd(Me)

    End Sub
    ''' <summary>
    ''' ＴＢＯＸＩＤ変更時処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtTboxId_TextChanged(sender As Object, e As EventArgs)

        'BBPUPDP001-001
        Me.ddlMente_No.SelectedValue = ""
        setDDLModel()
        'BBPUPDP001-001 END

        '空白の場合は処理しない.
        If Me.txtTbox_Id.ppText = String.Empty Then
            Exit Sub
        End If

        Dim strErr As String = String.Empty
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet               '表示情報のデータセット.
        Dim strSystem_Cls As String = String.Empty 'システム分類.

        objStack = New StackFrame

        strErr = pfCheck_TxtErr(Me.txtTbox_Id.ppText, False, True, False, False, 8, "", False)
        If strErr <> "" Then
            Me.txtTbox_Id.psSet_ErrorNo(strErr, "TBOXID")
            Exit Sub
        End If


        Try

            '開始ログ出力
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, sCnsErr_00005, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else
                'リストデータ取得
                cmdDB = New SqlCommand(M_DISP_ID & "_S2", conDB)
                cmdDB.Parameters.Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTbox_Id.ppText))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    txtTbox_Id.psSet_ErrorNo("2002", "入力されたTBOXID")
                    txtTbox_Id.ppTextBox.Focus()
                    Me.lblNl_Cls.Text = String.Empty
                    Me.lblEw_Flg.Text = String.Empty
                    Me.txtHall_Nm.ppText = String.Empty
                    Me.lblTboxClass_Cd.Text = String.Empty
                    Exit Sub
                End If

                'コントロールにセット.
                Me.lblNl_Cls.Text = dstOrders.Tables(0).Rows(0).Item("ＮＬ区分").ToString()
                Me.lblEw_Flg.Text = dstOrders.Tables(0).Rows(0).Item("ＥＷ").ToString()
                Me.txtHall_Nm.ppText = dstOrders.Tables(0).Rows(0).Item("ホール名").ToString()
                Me.lblTboxClass_Cd.Text = dstOrders.Tables(0).Rows(0).Item("ＴＢＯＸ種別").ToString()
                strSystem_Cls = dstOrders.Tables(0).Rows(0).Item("システム分類").ToString()
                ViewState("TboxCls") = dstOrders.Tables(0).Rows(0).Item("TBOX種別コード").ToString()

                '店内集信結果ドロップダウン設定.
                cmdDB = New SqlCommand(M_DISP_ID & "_S5", conDB)
                Select Case strSystem_Cls
                    Case "1"
                        cmdDB.Parameters.Add(pfSet_Param("class_cd", SqlDbType.NVarChar, "0077"))
                    Case Else
                        cmdDB.Parameters.Add(pfSet_Param("class_cd", SqlDbType.NVarChar, "0076"))
                End Select

                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                Me.ddlResult.DataSource = dstOrders.Tables(0)
                Me.ddlResult.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
                Me.ddlResult.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
                Me.ddlResult.DataBind()
                Me.ddlResult.Items.Insert(0, "")

            End If

        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ情報取得")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, sCnsErr_00006, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub
    ''' <summary>
    ''' 正/副機番件数変更時処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ctlJbAdd_TextChanged(sender As Object, e As EventArgs)

        Dim dec As Decimal = Nothing

        Select Case sender.ClientID

            Case sCnsOrgn_Lno

                '正機番左金額計算.
                Me.lblOrgn_Lm.Text = mfSet_Money(Me.txtOrgn_Lno.ppText)

            Case sCnsOrgn_Rno

                '正機番右金額計算.
                Me.lblOrgn_Rm.Text = mfSet_Money(Me.txtOrgn_Rno.ppText)

            Case sCnsDpl_Lno

                '副機番左金額計算.
                Me.lblDpl_Lm.Text = mfSet_Money(Me.txtDpl_Lno.ppText)

            Case sCnsDpl_Rno

                '副機番右金額計算.
                Me.lblDpl_Rm.Text = mfSet_Money(Me.txtDpl_Rno.ppText)

        End Select

        'BB累計金額計算処理.
        Me.lblBbTotal.Text = mfSet_BbPrice(Me.lblOrgn_Lm.Text,
                                           Me.lblOrgn_Rm.Text,
                                           Me.lblDpl_Lm.Text,
                                           Me.lblDpl_Rm.Text)

        If Me.chkOlddt_Flg.Checked = True Then
            If Decimal.TryParse(Me.lblBbTotal.Text, dec) = True And dec < 1 And Me.txtTbox_Bb.ppText = "TBOX照会不可" Then
                Me.txtTbox_Bb.ppText = "0"
            ElseIf Decimal.TryParse(Me.lblBbTotal.Text, dec) = True And dec > 0 Then
                Me.txtTbox_Bb.ppText = "TBOX照会不可"
                Me.lblComMoney.Text = String.Empty
            End If
        End If

        '補償金額計算.
        Me.lblComMoney.Text = mfSet_Money(Me.lblBbTotal.Text,
                                          Me.txtTbox_Bb.ppText,
                                          False)

    End Sub
    ''' <summary>
    ''' 正/副機番予備金額変更時処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ctlPay_TextChanged(sender As Object, e As EventArgs)

        Dim dec As Decimal = Nothing

        Select Case sender.ClientID

            Case sCnsOrgn_Pay, sCnsDpl_Pay  '入金額.
                'ミニカード入金額計算.
                Me.lblCardPay.Text = mfSet_Money(Me.txtOrgn_Pay.ppText,
                                                 Me.txtDpl_Pay.ppText,
                                                 True)

            Case sCnsOrgn_Consumer, sCnsDpl_Consumer  '消費額.
                'ミニカード消費額計算.
                Me.lblCardComsumer.Text = mfSet_Money(Me.txtOrgn_Consumer.ppText,
                                                      Me.txtDpl_Consumer.ppText,
                                                      True)
        End Select

        If Me.chkOlddt_Flg.Checked = True Then
            If Decimal.TryParse(Me.lblCardPay.Text, dec) = True And dec < 1 And Me.txtTbox_Receipt.ppText = "TBOX照会不可" Then
                Me.txtTbox_Receipt.ppText = "0"
            ElseIf Decimal.TryParse(Me.lblCardPay.Text, dec) = True And dec > 0 Then
                Me.txtTbox_Receipt.ppText = "TBOX照会不可"
                Me.lblNot_Col1.Text = String.Empty
            End If

            If Decimal.TryParse(Me.lblCardComsumer.Text, dec) = True And dec < 1 And Me.txtTbox_Cnsmp.ppText = "TBOX照会不可" Then
                Me.txtTbox_Cnsmp.ppText = "0"
            ElseIf Decimal.TryParse(Me.lblCardComsumer.Text, dec) = True And dec > 0 Then
                Me.txtTbox_Cnsmp.ppText = "TBOX照会不可"
                Me.lblNot_Col2.Text = String.Empty
            End If
        End If

        '未集信額計算.
        Me.lblNot_Col1.Text = mfSet_Money(Me.lblCardPay.Text,
                                          Me.txtTbox_Receipt.ppText,
                                          False)

        '未集信額計算.
        Me.lblNot_Col2.Text = mfSet_Money(Me.lblCardComsumer.Text,
                                          Me.txtTbox_Cnsmp.ppText,
                                          False)


    End Sub
    ''' <summary>
    ''' ＴＢＯＸ集信分変更時処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ctlTbox_TextChanged(sender As Object, e As EventArgs)

        Dim dec As Decimal = Nothing

        Select Case sender.ClientID

            Case sCnsTbox_Bb

                If Me.chkOlddt_Flg.Checked = True And Decimal.TryParse(Me.lblBbTotal.Text, dec) = True And dec > 0 Then
                    Me.txtTbox_Bb.ppText = "TBOX照会不可"
                    Me.lblComMoney.Text = String.Empty
                    Exit Sub
                End If

                '補償金額計算.
                Me.lblComMoney.Text = mfSet_Money(Me.lblBbTotal.Text,
                                                  Me.txtTbox_Bb.ppText,
                                                  False)

            Case sCnsTbox_Receipt

                If Me.chkOlddt_Flg.Checked = True And Decimal.TryParse(Me.lblCardPay.Text, dec) = True And dec > 0 Then
                    Me.txtTbox_Receipt.ppText = "TBOX照会不可"
                    Me.lblNot_Col1.Text = String.Empty
                    Exit Sub
                End If

                '未集信額計算.
                Me.lblNot_Col1.Text = mfSet_Money(Me.lblCardPay.Text,
                                                  Me.txtTbox_Receipt.ppText,
                                                  False)

            Case sCnsTbox_Cnsmp

                If Me.chkOlddt_Flg.Checked = True And Decimal.TryParse(Me.lblCardComsumer.Text, dec) = True And dec > 0 Then
                    Me.txtTbox_Cnsmp.ppText = "TBOX照会不可"
                    Me.lblNot_Col2.Text = String.Empty
                    Exit Sub
                End If

                '未集信額計算.
                Me.lblNot_Col2.Text = mfSet_Money(Me.lblCardComsumer.Text,
                                                  Me.txtTbox_Cnsmp.ppText,
                                                  False)

        End Select

    End Sub
    ''' <summary>
    ''' 日付古いチェック処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub chkOlddt_FlgCheckedChanged(sender As Object, e As EventArgs)

        Dim dec As Decimal = Nothing

        If Me.chkOlddt_Flg.Checked = True Then
            If Decimal.TryParse(Me.lblBbTotal.Text, dec) = True And dec > 0 Then
                Me.txtTbox_Bb.ppText = "TBOX照会不可"
                Me.lblComMoney.Text = String.Empty
            End If

            If Decimal.TryParse(Me.lblCardPay.Text, dec) = True And dec > 0 Then
                Me.txtTbox_Receipt.ppText = "TBOX照会不可"
                Me.lblNot_Col1.Text = String.Empty
            End If

            If Decimal.TryParse(Me.lblCardComsumer.Text, dec) = True And dec > 0 Then
                Me.txtTbox_Cnsmp.ppText = "TBOX照会不可"
                Me.lblNot_Col2.Text = String.Empty
            End If
        Else
            Me.txtTbox_Bb.ppText = "0"
            Me.txtTbox_Receipt.ppText = "0"
            Me.txtTbox_Cnsmp.ppText = "0"
        End If
    End Sub

    ''' <summary>
    ''' 更新ボタン押下時処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs)

        '個別エラーチェック.
        Call msCheck_Error()

        '検証チェック.
        If (Page.IsValid) Then

            Dim conDB As SqlConnection = Nothing
            Dim cmdDB As SqlCommand = Nothing
            Dim intRtn As Integer

            objStack = New StackFrame

            Try

                '開始ログ出力.
                psLogStart(Me)

                '接続
                If Not clsDataConnect.pfOpen_Database(conDB) Then
                    psMesBox(Me, sCnsErr_00005, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Else

                    'トランザクション.
                    Using conTrn = conDB.BeginTransaction
                        cmdDB = New SqlCommand(M_DISP_ID & "_U1", conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = conTrn
                        With cmdDB.Parameters
                            .Add(pfSet_Param("bbrep_no", SqlDbType.NVarChar, Me.lblBbinvst_No.Text))
                            .Add(pfSet_Param("repreq_no", SqlDbType.NVarChar, Me.txtRepair_No.ppText))
                            .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTbox_Id.ppText))
                            .Add(pfSet_Param("nl_flg", SqlDbType.NVarChar, Me.lblNl_Cls.Text))
                            Select Case Me.lblEw_Flg.Text
                                Case "西日本"
                                    .Add(pfSet_Param("ew_flg", SqlDbType.NVarChar, "W"))
                                Case "東日本"
                                    .Add(pfSet_Param("ew_flg", SqlDbType.NVarChar, "E"))
                            End Select
                            .Add(pfSet_Param("hall_nm", SqlDbType.NVarChar, Me.txtHall_Nm.ppText))
                            .Add(pfSet_Param("tbox_cls", SqlDbType.NVarChar, ViewState("TboxCls")))
                            .Add(pfSet_Param("model_no", SqlDbType.NVarChar, Me.ddlMente_No.SelectedValue))
                            .Add(pfSet_Param("serial", SqlDbType.NVarChar, Me.txtSerial.ppText))
                            .Add(pfSet_Param("status_cd", SqlDbType.NVarChar, Me.ddlStatus.SelectedValue))
                            .Add(pfSet_Param("rcv_dt", SqlDbType.NVarChar, Me.dtbReceipt_D.ppText))
                            .Add(pfSet_Param("wrk_dt", SqlDbType.NVarChar, Me.dtbWrk_D.ppText))
                            .Add(pfSet_Param("rep_dt", SqlDbType.NVarChar, Me.dtbReport_D.ppText))
                            .Add(pfSet_Param("bb1send_dt", SqlDbType.NVarChar, Me.dtbBb1_send_D.ppText))
                            .Add(pfSet_Param("ins_mt", SqlDbType.NVarChar, Me.txtInspect_M.ppText))
                            .Add(pfSet_Param("acc_dt", SqlDbType.NVarChar, Me.dtbAccDt.ppText))
                            .Add(pfSet_Param("bb_cls", SqlDbType.NVarChar, Me.txtBb_Cls.ppText))
                            .Add(pfSet_Param("jb_no1", SqlDbType.NVarChar, Me.txtJb_No1.ppText))
                            .Add(pfSet_Param("jb_no2", SqlDbType.NVarChar, Me.txtJb_No2.ppText))
                            .Add(pfSet_Param("jb_no3", SqlDbType.NVarChar, Me.txtJb_No3.ppText))
                            .Add(pfSet_Param("checker", SqlDbType.NVarChar, Me.txtChecker.ppText))
                            .Add(pfSet_Param("led_flg", SqlDbType.NVarChar, Me.ddlLed_Flg.ppSelectedValue))
                            .Add(pfSet_Param("dst_flg", SqlDbType.NVarChar, Me.ddlDst_Flg.ppSelectedValue))
                            .Add(pfSet_Param("clct_rslt", SqlDbType.NVarChar, Me.ddlResult.SelectedValue))

                            Select Case Me.chkOlddt_Flg.Checked
                                Case False
                                    .Add(pfSet_Param("olddt_flg", SqlDbType.NVarChar, "0"))
                                Case True
                                    .Add(pfSet_Param("olddt_flg", SqlDbType.NVarChar, "1"))
                            End Select

                            Select Case Me.chkCrpt_Flg.Checked
                                Case False
                                    .Add(pfSet_Param("crpt_flg", SqlDbType.NVarChar, "0"))

                                Case True
                                    .Add(pfSet_Param("crpt_flg", SqlDbType.NVarChar, "1"))
                            End Select

                            Select Case Me.chkBb1brk_Flg.Checked
                                Case False
                                    .Add(pfSet_Param("bb1brk_flg", SqlDbType.NVarChar, "0"))
                                Case True
                                    .Add(pfSet_Param("bb1brk_flg", SqlDbType.NVarChar, "1"))
                            End Select

                            .Add(pfSet_Param("brk_cntnt", SqlDbType.NVarChar, Me.txtBrkCntnt.ppText))
                            .Add(pfSet_Param("oth_cd", SqlDbType.NVarChar, Me.ddlOth_Cd.SelectedValue))
                            .Add(pfSet_Param("oth_free", SqlDbType.NVarChar, Me.txtOth_Free.ppText))
                            .Add(pfSet_Param("rep_req", SqlDbType.NVarChar, Me.ddlRep_Req.ppSelectedValue))
                            .Add(pfSet_Param("bf_crct", SqlDbType.NVarChar, mfGetDBNull(Me.txtBf_Crct.ppText)))
                            .Add(pfSet_Param("af_crct", SqlDbType.NVarChar, mfGetDBNull(Me.txtAf_Crct.ppText)))
                            .Add(pfSet_Param("fsi", SqlDbType.NVarChar, mfGetDBNull(Me.txtFsi.ppText)))
                            .Add(pfSet_Param("sim", SqlDbType.NVarChar, mfGetDBNull(Me.txtSim.ppText)))
                            .Add(pfSet_Param("mng_dt", SqlDbType.NVarChar, Me.dtbMng_Dt.ppText))
                            .Add(pfSet_Param("bb_no", SqlDbType.NVarChar, Me.txtBb_No.ppText))
                            .Add(pfSet_Param("orgn_no", SqlDbType.NVarChar, Me.txtOrgn_No.ppText))
                            .Add(pfSet_Param("dpl_no", SqlDbType.NVarChar, Me.txtDpl_No.ppText))
                            .Add(pfSet_Param("orgn_lno", SqlDbType.NVarChar, Me.txtOrgn_Lno.ppText))
                            .Add(pfSet_Param("orgn_rno", SqlDbType.NVarChar, Me.txtOrgn_Rno.ppText))
                            .Add(pfSet_Param("dpl_lno", SqlDbType.NVarChar, Me.txtDpl_Lno.ppText))
                            .Add(pfSet_Param("dpl_rno", SqlDbType.NVarChar, Me.txtDpl_Rno.ppText))
                            .Add(pfSet_Param("orgn_deposit", SqlDbType.NVarChar, mfGetDBNull(Me.txtOrgn_Pay.ppText)))
                            .Add(pfSet_Param("orgn_expend", SqlDbType.NVarChar, mfGetDBNull(Me.txtOrgn_Consumer.ppText)))
                            .Add(pfSet_Param("dpl_deposit", SqlDbType.NVarChar, mfGetDBNull(Me.txtDpl_Pay.ppText)))
                            .Add(pfSet_Param("dpl_expend", SqlDbType.NVarChar, mfGetDBNull(Me.txtDpl_Consumer.ppText)))
                            .Add(pfSet_Param("tbox_bb", SqlDbType.NVarChar, mfGetDBNull(Me.txtTbox_Bb.ppText)))
                            .Add(pfSet_Param("tbox_receipt", SqlDbType.NVarChar, mfGetDBNull(Me.txtTbox_Receipt.ppText)))
                            .Add(pfSet_Param("tbox_cnsmp", SqlDbType.NVarChar, mfGetDBNull(Me.txtTbox_Cnsmp.ppText)))
                            .Add(pfSet_Param("update_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With

                        '実行
                        cmdDB.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, sCnsErr_00001, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsDispNm)

                            'ロールバック
                            conTrn.Rollback()

                            Exit Sub
                        End If

                        'コミット
                        conTrn.Commit()

                    End Using

                    '完了メッセージ
                    psMesBox(Me, sCnsInfo_00001, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, sCnsDispNm)
                End If
            Catch ex As Exception
                psMesBox(Me, sCnsErr_00001, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsDispNm)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, sCnsErr_00006, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

                '終了ログ出力
                psLogEnd(Me)

            End Try
        End If
    End Sub
    ''' <summary>
    ''' 削除ボタン押下時処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDelete_Click(sender As Object, e As EventArgs)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim intRtn As Integer
        objStack = New StackFrame

        Try

            '開始ログ出力.
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, sCnsErr_00005, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else

                'トランザクション.
                Using conTrn = conDB.BeginTransaction

                    cmdDB = New SqlCommand(M_DISP_ID & "_D1", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = conTrn
                    cmdDB.Parameters.Add(pfSet_Param("bbrep_no", SqlDbType.NVarChar, Me.lblBbinvst_No.Text))
                    cmdDB.Parameters.Add(pfSet_Param("retvalue", SqlDbType.NVarChar))

                    '実行
                    cmdDB.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, sCnsErr_00002, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsDispNm)

                        'ロールバック
                        conTrn.Rollback()

                        Exit Sub
                    End If

                    'コミット
                    conTrn.Commit()

                End Using

                '完了メッセージ
                psMesBox(Me, sCnsInfo_00002, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, sCnsDispNm)

                '削除後画面非活性
                Call msEnableScreen(ClsComVer.E_遷移条件.参照)
            End If
        Catch ex As Exception
            psMesBox(Me, sCnsErr_00002, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsDispNm)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, sCnsErr_00006, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try
    End Sub
    ''' <summary>
    ''' 印刷ボタン押下時処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPrint_Click(sender As Object, e As EventArgs)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet                     'ブラックボックス調査報告書用.
        Dim dstTbox As New DataSet                       'TBOX情報データ取得用.
        Dim dstDisplay_Item As New DataSet               '表示項目マスタ取得用.
        Dim dtPrint As New DataTable                     '帳票用データテーブル.
        Dim dtRow As DataRow = dtPrint.NewRow()          'データテーブルの行定義.
        Dim strBbTotal As String = String.Empty          'BB蓄積額.
        Dim strCardPay As String = String.Empty          'ミニカード入金額.
        Dim strCardConsumer As String = String.Empty     'ミニカード消費額.
        Dim strTboxClass As String = String.Empty        'ＴＢＯＸ種別.
        Dim strL_Cls As String = String.Empty            '大項目.
        Dim strBiko6 As String = String.Empty            '備考６文言.
        Dim dtBiko As New DataTable                      '備考用データテーブル.
        Dim intCnt As Integer = 0                        'ループカウント用.
        Dim strFNm As String = "ブラックボックス調査報告書_" & Me.lblBbinvst_No.Text & "_" & Date.Today.ToString("yyyyMMddHHmmss")  '帳票名称
        Dim rptWATREP002 As New WATREP002
        Dim rptWATREP004 As New WATREP004
        Dim rpt As New Object

        objStack = New StackFrame

        Try
            '開始ログ出力
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, sCnsErr_00005, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else

                'ブラックボックス調査報告書データ再取得.
                cmdDB = New SqlCommand(M_DISP_ID & "_S1", conDB)
                cmdDB.Parameters.Add(pfSet_Param("bbrep_no", SqlDbType.NVarChar, Me.lblBbinvst_No.Text))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'TBOX情報データ取得
                cmdDB = New SqlCommand(M_DISP_ID & "_S2", conDB)
                cmdDB.Parameters.Add(pfSet_Param("tboxid", SqlDbType.NVarChar, dstOrders.Tables(0).Rows(0).Item("TBOXID").ToString()))
                dstTbox = clsDataConnect.pfGet_DataSet(cmdDB)

                'データがない場合は印刷しない
                If dstOrders.Tables(0).Rows.Count = 0 OrElse dstTbox.Tables(0).Rows.Count = 0 Then
                    psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    Return
                End If

                Select Case dstTbox.Tables(0).Rows(0).Item("システム分類").ToString()
                    Case "1" 'ID系
                        strL_Cls = "02"
                    Case Else
                        strL_Cls = "01"
                End Select


                Select Case strL_Cls
                    Case "02" 'ID系
                        rpt = rptWATREP004
                        '帳票用データテーブルの行設定.
                        dtPrint.Columns.Add("エリア")
                        dtPrint.Columns.Add("ＢＢ調報告ＮＯ")
                        dtPrint.Columns.Add("和暦")
                        dtPrint.Columns.Add("店舗名")
                        dtPrint.Columns.Add("ＴＢＯＸＩＤ")
                        dtPrint.Columns.Add("管理日付")
                        dtPrint.Columns.Add("ＢＢ_ＮＯ")
                        dtPrint.Columns.Add("正機番")
                        dtPrint.Columns.Add("正機番左件数")
                        dtPrint.Columns.Add("正機番右件数")
                        dtPrint.Columns.Add("正機番左金額")
                        dtPrint.Columns.Add("正機番右金額")
                        dtPrint.Columns.Add("正機番ミニカード入金")
                        dtPrint.Columns.Add("正機番ミニカード消費")
                        dtPrint.Columns.Add("副機番")
                        dtPrint.Columns.Add("副機番左件数")
                        dtPrint.Columns.Add("副機番右件数")
                        dtPrint.Columns.Add("副機番左金額")
                        dtPrint.Columns.Add("副機番右金額")
                        dtPrint.Columns.Add("副機番ミニカード入金")
                        dtPrint.Columns.Add("副機番ミニカード消費")
                        dtPrint.Columns.Add("保持ランプ点灯有無")
                        dtPrint.Columns.Add("ＢＢ累計額")
                        dtPrint.Columns.Add("ＴＢＯＸ集信分ＢＢ")
                        dtPrint.Columns.Add("補償金額")
                        dtPrint.Columns.Add("ミニカード入金額")
                        dtPrint.Columns.Add("ＴＢＯＸ集信分入金")
                        dtPrint.Columns.Add("未集信額１")
                        dtPrint.Columns.Add("ミニカード消費額")
                        dtPrint.Columns.Add("ＴＢＯＸ集信分消費")
                        dtPrint.Columns.Add("未集信額２")
                        dtPrint.Columns.Add("修理依頼票管理Ｎｏ")
                        dtPrint.Columns.Add("ＳＮｏ")
                        dtPrint.Columns.Add("事故発生日")
                        dtPrint.Columns.Add("故障機型式番号")
                        dtPrint.Columns.Add("装置状態")
                        dtPrint.Columns.Add("データ破棄完了")
                        dtPrint.Columns.Add("処理不可")
                        dtPrint.Columns.Add("メーカー")
                        dtPrint.Columns.Add("備考１")
                        dtPrint.Columns.Add("備考２")
                        dtPrint.Columns.Add("備考３")
                        dtPrint.Columns.Add("備考３_１")
                        dtPrint.Columns.Add("備考５")
                        dtPrint.Columns.Add("備考５_１")
                        dtPrint.Columns.Add("備考５_２")
                        dtPrint.Columns.Add("備考６")
                        dtPrint.Columns.Add("備考６_１")
                        dtPrint.Columns.Add("HALL_OR_TBOXID")
                        dtPrint.Columns.Add("受領日")
                        dtPrint.Columns.Add("BB１区分")
                        'BBPUPDP001_002
                        dtPrint.Columns.Add("送り元会社名")
                        dtPrint.Columns.Add("送り元名")
                        'BBPUPDP001_002 END
                        '帳票用データセット設定.
                        dtRow("エリア") = dstOrders.Tables(0).Rows(0).Item("EW区分").ToString()
                        dtRow("ＢＢ調報告ＮＯ") = dstOrders.Tables(0).Rows(0).Item("BB調報No").ToString()
                        dtRow("和暦") = mfChange_Jp(dstOrders.Tables(0).Rows(0).Item("報告日").ToString())
                        dtRow("店舗名") = dstOrders.Tables(0).Rows(0).Item("ホール名").ToString()
                        If dstOrders.Tables(0).Rows(0).Item("BB1故障").ToString() = "1" Then
                            dtRow("ＴＢＯＸＩＤ") = String.Empty
                        Else
                            dtRow("ＴＢＯＸＩＤ") = dstOrders.Tables(0).Rows(0).Item("TBOXID").ToString()
                        End If
                        dtRow("管理日付") = dstOrders.Tables(0).Rows(0).Item("ID系管理日付").ToString()
                        dtRow("ＢＢ_ＮＯ") = dstOrders.Tables(0).Rows(0).Item("ID系BBNo").ToString()
                        dtRow("正機番") = dstOrders.Tables(0).Rows(0).Item("正機番").ToString()
                        dtRow("正機番左件数") = dstOrders.Tables(0).Rows(0).Item("正機番左件数").ToString()
                        dtRow("正機番右件数") = dstOrders.Tables(0).Rows(0).Item("正機番右件数").ToString()
                        dtRow("正機番左金額") = mfSet_Money(dstOrders.Tables(0).Rows(0).Item("正機番左件数").ToString())
                        dtRow("正機番右金額") = mfSet_Money(dstOrders.Tables(0).Rows(0).Item("正機番右件数").ToString())
                        dtRow("正機番ミニカード入金") = dstOrders.Tables(0).Rows(0).Item("正機番ミニカード入金金額").ToString()
                        dtRow("正機番ミニカード消費") = dstOrders.Tables(0).Rows(0).Item("正機番ミニカード消費金額").ToString()
                        dtRow("副機番") = dstOrders.Tables(0).Rows(0).Item("副機番").ToString()
                        dtRow("副機番左件数") = dstOrders.Tables(0).Rows(0).Item("副機番左件数").ToString()
                        dtRow("副機番右件数") = dstOrders.Tables(0).Rows(0).Item("副機番右件数").ToString()
                        dtRow("副機番左金額") = mfSet_Money(dstOrders.Tables(0).Rows(0).Item("副機番左件数").ToString())
                        dtRow("副機番右金額") = mfSet_Money(dstOrders.Tables(0).Rows(0).Item("副機番右件数").ToString())
                        dtRow("副機番ミニカード入金") = dstOrders.Tables(0).Rows(0).Item("副機番ミニカード入金金額").ToString()
                        dtRow("副機番ミニカード消費") = dstOrders.Tables(0).Rows(0).Item("副機番ミニカード消費金額").ToString()
                        dtRow("保持ランプ点灯有無") = dstOrders.Tables(0).Rows(0).Item("LED点灯表示用").ToString()
                        strBbTotal = mfSet_BbPrice(mfSet_Money(dstOrders.Tables(0).Rows(0).Item("正機番左件数").ToString()),
                                              mfSet_Money(dstOrders.Tables(0).Rows(0).Item("正機番右件数").ToString()),
                                              mfSet_Money(dstOrders.Tables(0).Rows(0).Item("副機番左件数").ToString()),
                                              mfSet_Money(dstOrders.Tables(0).Rows(0).Item("副機番右件数").ToString()))
                        dtRow("ＢＢ累計額") = strBbTotal
                        If dstOrders.Tables(0).Rows(0).Item("日付古い").ToString() = "1" Then
                            If strBbTotal < 1 Then
                                dtRow("ＴＢＯＸ集信分ＢＢ") = dstOrders.Tables(0).Rows(0).Item("TBOX集信分1").ToString()
                                dtRow("補償金額") = mfSet_Money(strBbTotal, dstOrders.Tables(0).Rows(0).Item("TBOX集信分1").ToString(), False)
                            Else
                                dtRow("ＴＢＯＸ集信分ＢＢ") = "ＴＢＯＸ照会不可"
                                dtRow("補償金額") = ""
                            End If
                        Else
                            dtRow("ＴＢＯＸ集信分ＢＢ") = dstOrders.Tables(0).Rows(0).Item("TBOX集信分1").ToString()
                            dtRow("補償金額") = mfSet_Money(strBbTotal, dstOrders.Tables(0).Rows(0).Item("TBOX集信分1").ToString(), False)
                        End If
                        strCardPay = mfSet_Money(dstOrders.Tables(0).Rows(0).Item("正機番ミニカード入金金額").ToString(),
                                                                dstOrders.Tables(0).Rows(0).Item("副機番ミニカード入金金額").ToString(),
                                                                True)
                        If strCardPay <> "" Then
                            dtRow("ミニカード入金額") = (Decimal.Parse(strCardPay) * 100).ToString("#,##0")
                        Else
                            dtRow("ミニカード入金額") = Nothing
                        End If

                        If dstOrders.Tables(0).Rows(0).Item("日付古い").ToString() = "1" Then
                            If strCardPay < 1 Then
                                dtRow("ＴＢＯＸ集信分入金") = (Decimal.Parse(dstOrders.Tables(0).Rows(0).Item("TBOX集信分2").ToString()) * 100).ToString("#,##0")
                                dtRow("未集信額１") = (Decimal.Parse(mfSet_Money(strCardPay, dstOrders.Tables(0).Rows(0).Item("TBOX集信分2").ToString(), False)) * 100).ToString("#,##0")
                            Else
                                dtRow("ＴＢＯＸ集信分入金") = "ＴＢＯＸ照会不可"
                                dtRow("未集信額１") = ""
                            End If
                        Else
                            If dstOrders.Tables(0).Rows(0).Item("TBOX集信分2").ToString <> "" Then
                                dtRow("ＴＢＯＸ集信分入金") = (Decimal.Parse(dstOrders.Tables(0).Rows(0).Item("TBOX集信分2").ToString()) * 100).ToString("#,##0")
                                dtRow("未集信額１") = (Decimal.Parse(mfSet_Money(strCardPay, dstOrders.Tables(0).Rows(0).Item("TBOX集信分2").ToString(), False)) * 100).ToString("#,##0")
                            Else
                                dtRow("ＴＢＯＸ集信分入金") = Nothing
                                dtRow("未集信額１") = Nothing
                            End If
                        End If
                        strCardConsumer = mfSet_Money(dstOrders.Tables(0).Rows(0).Item("正機番ミニカード消費金額").ToString(),
                                                                dstOrders.Tables(0).Rows(0).Item("副機番ミニカード消費金額").ToString(),
                                                                True)
                        If strCardConsumer <> "" Then
                            dtRow("ミニカード消費額") = (Decimal.Parse(strCardConsumer) * 100).ToString("#,##0")
                        Else
                            dtRow("ミニカード消費額") = Nothing
                        End If

                        If dstOrders.Tables(0).Rows(0).Item("日付古い").ToString() = "1" Then
                            If strCardConsumer < 1 Then
                                dtRow("ＴＢＯＸ集信分消費") = (Decimal.Parse(dstOrders.Tables(0).Rows(0).Item("TBOX集信分3").ToString()) * 100).ToString("#,##0")
                                dtRow("未集信額２") = (Decimal.Parse(mfSet_Money(strCardConsumer, dstOrders.Tables(0).Rows(0).Item("TBOX集信分3").ToString(), False)) * 100).ToString("#,##0")
                            Else
                                dtRow("ＴＢＯＸ集信分消費") = "ＴＢＯＸ照会不可"
                                dtRow("未集信額２") = ""
                            End If
                        Else
                            If dstOrders.Tables(0).Rows(0).Item("TBOX集信分3").ToString <> "" Then
                                dtRow("ＴＢＯＸ集信分消費") = (Decimal.Parse(dstOrders.Tables(0).Rows(0).Item("TBOX集信分3").ToString()) * 100).ToString("#,##0")
                                dtRow("未集信額２") = (Decimal.Parse(mfSet_Money(strCardConsumer, dstOrders.Tables(0).Rows(0).Item("TBOX集信分3").ToString(), False)) * 100).ToString("#,##0")
                            Else
                                dtRow("ＴＢＯＸ集信分消費") = Nothing
                                dtRow("未集信額２") = Nothing
                            End If
                        End If


                        If dstOrders.Tables(0).Rows(0).Item("日付古い").ToString() = "1" Then
                            dtRow("備考１") = "①"
                        Else
                            dtRow("備考１") = "１"
                        End If

                        If dstOrders.Tables(0).Rows(0).Item("データ化け").ToString() = "1" Then
                            dtRow("備考２") = "②"
                        Else
                            dtRow("備考２") = "２"
                        End If

                        If dstOrders.Tables(0).Rows(0).Item("BB1故障").ToString() = "1" Then
                            dtRow("備考３") = "③"
                            dtRow("備考３_１") = dstOrders.Tables(0).Rows(0).Item("故障内容").ToString()
                        Else
                            dtRow("備考３") = "３"
                            dtRow("備考３_１") = ""
                        End If

                        If dstOrders.Tables(0).Rows(0).Item("訂正前").ToString() <> String.Empty And dstOrders.Tables(0).Rows(0).Item("訂正後").ToString() <> String.Empty Then
                            dtRow("備考５") = "⑤"
                            dtRow("備考５_１") = dstOrders.Tables(0).Rows(0).Item("訂正前").ToString()
                            dtRow("備考５_２") = dstOrders.Tables(0).Rows(0).Item("訂正後").ToString()
                        Else
                            dtRow("備考５") = "５"
                            dtRow("備考５_１") = ""
                            dtRow("備考５_２") = ""
                        End If

                        If dstOrders.Tables(0).Rows(0).Item("その他定型").ToString() <> String.Empty And dstOrders.Tables(0).Rows(0).Item("その他フリー").ToString() <> String.Empty Then
                            dtRow("備考６") = "⑥"
                            dtRow("備考６_１") = dstOrders.Tables(0).Rows(0).Item("その他定型").ToString() & dstOrders.Tables(0).Rows(0).Item("その他フリー").ToString()
                            dtRow("備考６_１") = dstOrders.Tables(0).Rows(0).Item("その他フリー").ToString()
                        Else
                            dtRow("備考６") = "６"
                            dtRow("備考６_１") = ""
                        End If

                        dtRow("修理依頼票管理Ｎｏ") = dstOrders.Tables(0).Rows(0).Item("修理依頼No").ToString()
                        dtRow("ＳＮｏ") = dstOrders.Tables(0).Rows(0).Item("シリアルNo").ToString()
                        dtRow("事故発生日") = dstOrders.Tables(0).Rows(0).Item("事故発生日").ToString()
                        dtRow("故障機型式番号") = dstOrders.Tables(0).Rows(0).Item("故障機型式番号").ToString()
                        dtRow("装置状態") = dstOrders.Tables(0).Rows(0).Item("店内集信結果ID表示用").ToString()
                        dtRow("データ破棄完了") = dstOrders.Tables(0).Rows(0).Item("データ破棄完了表示用").ToString()
                        dtRow("処理不可") = dstOrders.Tables(0).Rows(0).Item("故障内容").ToString()
                        dtRow("メーカー") = String.Empty
                        Select Case dstOrders.Tables(0).Rows(0).Item("修理依頼票").ToString()
                            Case "1"
                                dtRow("HALL_OR_TBOXID") = "TBOXID"
                            Case "2"
                                dtRow("HALL_OR_TBOXID") = "ホール名"
                            Case Else
                                dtRow("HALL_OR_TBOXID") = ""
                        End Select

                        dtRow("受領日") = dstOrders.Tables(0).Rows(0).Item("受領日表示用").ToString
                        dtRow("BB１区分") = dstOrders.Tables(0).Rows(0).Item("TBOX種別").ToString()
                        'BBPUPDP001_002
                        dtRow("送り元会社名") = dstTbox.Tables(0).Rows(0).Item("送り元会社名").ToString
                        dtRow("送り元名") = dstTbox.Tables(0).Rows(0).Item("送り元名").ToString
                        'BBPUPDP001_002 END
                    Case Else 'IC系

                        rpt = rptWATREP002

                        '帳票用データテーブルの行設定.
                        dtPrint.Columns.Add("エリア")
                        dtPrint.Columns.Add("ＢＢ調報告ＮＯ")
                        dtPrint.Columns.Add("和暦")
                        dtPrint.Columns.Add("タイトル")
                        dtPrint.Columns.Add("店舗名")
                        dtPrint.Columns.Add("ＴＢＯＸＩＤ")
                        dtPrint.Columns.Add("ＢＢ種別名")
                        dtPrint.Columns.Add("処理年月日")
                        dtPrint.Columns.Add("型式番号")
                        dtPrint.Columns.Add("ＢＢシリアル番号")
                        dtPrint.Columns.Add("吸上明細書_債権合計")
                        dtPrint.Columns.Add("吸上明細書_負債合計")
                        dtPrint.Columns.Add("保持ランプ点灯有無")
                        dtPrint.Columns.Add("備考１")
                        dtPrint.Columns.Add("備考１_１")
                        dtPrint.Columns.Add("備考２")
                        dtPrint.Columns.Add("備考２_１")
                        dtPrint.Columns.Add("備考２_２")
                        dtPrint.Columns.Add("備考３")
                        dtPrint.Columns.Add("備考３_１")
                        dtPrint.Columns.Add("修正依頼票管理Ｎｏ")
                        dtPrint.Columns.Add("事故発生日")
                        dtPrint.Columns.Add("ＬＥＤ点灯")
                        dtPrint.Columns.Add("装置状態")
                        dtPrint.Columns.Add("データ破棄完了")
                        dtPrint.Columns.Add("処理不可")
                        dtPrint.Columns.Add("メーカー")
                        dtPrint.Columns.Add("HALL_OR_TBOXID")
                        'BBPUPDP001_002
                        dtPrint.Columns.Add("送り元会社名")
                        dtPrint.Columns.Add("送り元名")
                        'BBPUPDP001_002 END
                        dtPrint.Columns.Add("受領日")
                        '帳票用データセット設定.
                        dtRow("エリア") = dstOrders.Tables(0).Rows(0).Item("EW区分").ToString()
                        dtRow("ＢＢ調報告ＮＯ") = dstOrders.Tables(0).Rows(0).Item("BB調報No").ToString()
                        dtRow("和暦") = mfChange_Jp(dstOrders.Tables(0).Rows(0).Item("報告日").ToString())
                        strTboxClass = dstTbox.Tables(0).Rows(0).Item("ＴＢＯＸ種別").ToString()

                        Select Case strTboxClass.Length > 3
                            Case True
                                If strTboxClass.Substring(0, 3) = "NVC" Or strTboxClass.Substring(0, 3) = "HTB" Then
                                    dtRow("タイトル") = strTboxClass
                                Else
                                    dtRow("タイトル") = "ＩＣ２次"
                                End If
                            Case False
                                dtRow("タイトル") = "ＩＣ２次"
                        End Select

                        dtRow("店舗名") = dstOrders.Tables(0).Rows(0).Item("ホール名").ToString()

                        If dstOrders.Tables(0).Rows(0).Item("データ破棄完了表示用").ToString() = "×" Or dstOrders.Tables(0).Rows(0).Item("BB1故障").ToString = "1" Then
                            dtRow("ＴＢＯＸＩＤ") = String.Empty
                        Else
                            dtRow("ＴＢＯＸＩＤ") = dstOrders.Tables(0).Rows(0).Item("TBOXID").ToString()
                        End If
                        dtRow("ＢＢ種別名") = dstOrders.Tables(0).Rows(0).Item("BB種別名").ToString()
                        dtRow("処理年月日") = dstOrders.Tables(0).Rows(0).Item("作業日").ToString()
                        dtRow("型式番号") = dstOrders.Tables(0).Rows(0).Item("故障機型式番号").ToString()
                        dtRow("ＢＢシリアル番号") = dstOrders.Tables(0).Rows(0).Item("シリアルNo").ToString()
                        dtRow("吸上明細書_債権合計") = dstOrders.Tables(0).Rows(0).Item("IC系BB負債額").ToString()
                        dtRow("吸上明細書_負債合計") = dstOrders.Tables(0).Rows(0).Item("IC系BB債務額").ToString()
                        dtRow("保持ランプ点灯有無") = dstOrders.Tables(0).Rows(0).Item("LED点灯表示用").ToString()
                        dtRow("修正依頼票管理Ｎｏ") = dstOrders.Tables(0).Rows(0).Item("修理依頼No").ToString()
                        dtRow("事故発生日") = dstOrders.Tables(0).Rows(0).Item("事故発生日").ToString()
                        dtRow("ＬＥＤ点灯") = dstOrders.Tables(0).Rows(0).Item("LED点灯表示用").ToString()
                        dtRow("装置状態") = dstOrders.Tables(0).Rows(0).Item("店内集信結果IC表示用").ToString()
                        dtRow("データ破棄完了") = dstOrders.Tables(0).Rows(0).Item("データ破棄完了表示用").ToString()
                        dtRow("処理不可") = dstOrders.Tables(0).Rows(0).Item("故障内容").ToString()
                        dtRow("メーカー") = String.Empty
                        dtRow("備考１") = String.Empty
                        dtRow("備考２") = String.Empty
                        dtRow("備考３") = String.Empty

                        If dstOrders.Tables(0).Rows(0).Item("BB1故障").ToString() = "1" Then
                            dtRow("備考１") = "①"
                            dtRow("備考１_１") = dstOrders.Tables(0).Rows(0).Item("故障内容").ToString()
                        Else
                            dtRow("備考１") = "１"
                            dtRow("備考１_１") = ""
                        End If

                        If dstOrders.Tables(0).Rows(0).Item("訂正前").ToString() <> String.Empty And dstOrders.Tables(0).Rows(0).Item("訂正後").ToString() <> String.Empty Then
                            dtRow("備考２") = "②"
                            dtRow("備考２_１") = dstOrders.Tables(0).Rows(0).Item("訂正前").ToString()
                            dtRow("備考２_２") = dstOrders.Tables(0).Rows(0).Item("訂正後").ToString()
                        Else
                            dtRow("備考２") = "２"
                            dtRow("備考２_１") = ""
                            dtRow("備考２_２") = ""
                        End If

                        If dstOrders.Tables(0).Rows(0).Item("その他定型").ToString() <> String.Empty And dstOrders.Tables(0).Rows(0).Item("その他フリー").ToString() <> String.Empty Then
                            dtRow("備考３") = "③"
                            dtRow("備考３_１") = dstOrders.Tables(0).Rows(0).Item("その他フリー").ToString()
                        Else
                            dtRow("備考３") = "３"
                            dtRow("備考３_１") = ""
                        End If

                        Select Case dstOrders.Tables(0).Rows(0).Item("修理依頼票").ToString()
                            Case "1"
                                dtRow("HALL_OR_TBOXID") = "TBIXID"
                            Case "2"
                                dtRow("HALL_OR_TBOXID") = "ホール名"
                            Case Else
                                dtRow("HALL_OR_TBOXID") = ""
                        End Select
                        dtRow("受領日") = dstOrders.Tables(0).Rows(0).Item("受領日表示用").ToString
                        'BBPUPDP001_002
                        dtRow("送り元会社名") = dstTbox.Tables(0).Rows(0).Item("送り元会社名").ToString
                        dtRow("送り元名") = dstTbox.Tables(0).Rows(0).Item("送り元名").ToString
                        'BBPUPDP001_002 END
                End Select

            End If

            'データテーブルに作成したデータを設定
            dtPrint.Rows.Add(dtRow)

            Try
                'Active Reports帳票の起動
                psPrintPDF(Me, rpt, dtPrint, strFNm)

            Catch ex As Exception
                '帳票の出力に失敗
                psMesBox(Me, sCnsErr_10001, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsDispNm)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            End Try

        Catch ex As SqlException
            '帳票情報の取得に失敗
            psMesBox(Me, sCnsErr_00004, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "帳票情報")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Catch ex As Exception
            '帳票の出力に失敗
            psMesBox(Me, sCnsErr_10001, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsDispNm)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, sCnsErr_00006, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
            '終了ログ出力
            psLogEnd(Me)
        End Try
    End Sub
#End Region

#Region "そのほかのプロシージャ"
    ''' <summary>
    ''' イベント設定.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_Action()

        'ボタン活性
        Master.ppRigthButton1.Text = "更新"
        Master.ppRigthButton1.Visible = True
        Master.ppRigthButton2.Text = "削除"
        Master.ppRigthButton2.Visible = True
        Master.ppRigthButton3.Text = "印刷"
        Master.ppRigthButton3.Visible = True
        Master.ppRigthButton4.Text = "元に戻す"
        Master.ppRigthButton4.Visible = True

        'イベントとプロシージャを関連付け.
        AddHandler Me.txtTbox_Id.ppTextBox.TextChanged, AddressOf txtTboxId_TextChanged
        AddHandler Me.txtOrgn_Lno.ppTextBox.TextChanged, AddressOf ctlJbAdd_TextChanged
        AddHandler Me.txtOrgn_Rno.ppTextBox.TextChanged, AddressOf ctlJbAdd_TextChanged
        AddHandler Me.txtOrgn_Pay.ppTextBox.TextChanged, AddressOf ctlPay_TextChanged
        AddHandler Me.txtOrgn_Consumer.ppTextBox.TextChanged, AddressOf ctlPay_TextChanged
        AddHandler Me.txtDpl_Lno.ppTextBox.TextChanged, AddressOf ctlJbAdd_TextChanged
        AddHandler Me.txtDpl_Rno.ppTextBox.TextChanged, AddressOf ctlJbAdd_TextChanged
        AddHandler Me.txtDpl_Pay.ppTextBox.TextChanged, AddressOf ctlPay_TextChanged
        AddHandler Me.txtDpl_Consumer.ppTextBox.TextChanged, AddressOf ctlPay_TextChanged
        AddHandler Me.txtTbox_Bb.ppTextBox.TextChanged, AddressOf ctlTbox_TextChanged
        AddHandler Me.txtTbox_Receipt.ppTextBox.TextChanged, AddressOf ctlTbox_TextChanged
        AddHandler Me.txtTbox_Cnsmp.ppTextBox.TextChanged, AddressOf ctlTbox_TextChanged

        'ボタンアクションの設定.
        AddHandler Master.ppRigthButton1.Click, AddressOf btnUpdate_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnDelete_Click
        AddHandler Master.ppRigthButton3.Click, AddressOf btnPrint_Click
        AddHandler Master.ppRigthButton4.Click, AddressOf btnReset_Click

        'ポストバック設定.
        Me.txtTbox_Id.ppTextBox.AutoPostBack = True
        Me.txtOrgn_Lno.ppTextBox.AutoPostBack = True
        Me.txtOrgn_Rno.ppTextBox.AutoPostBack = True
        Me.txtDpl_Lno.ppTextBox.AutoPostBack = True
        Me.txtDpl_Rno.ppTextBox.AutoPostBack = True
        Me.txtOrgn_Pay.ppTextBox.AutoPostBack = True
        Me.txtOrgn_Consumer.ppTextBox.AutoPostBack = True
        Me.txtDpl_Pay.ppTextBox.AutoPostBack = True
        Me.txtDpl_Consumer.ppTextBox.AutoPostBack = True
        Me.txtTbox_Bb.ppTextBox.AutoPostBack = True
        Me.txtTbox_Receipt.ppTextBox.AutoPostBack = True
        Me.txtTbox_Cnsmp.ppTextBox.AutoPostBack = True

        Master.ppRigthButton4.CausesValidation = False

        Master.ppRigthButton1.ValidationGroup = 1
        AddHandler Me.chkOlddt_Flg.CheckedChanged, AddressOf chkOlddt_FlgCheckedChanged
        Me.chkOlddt_Flg.AutoPostBack = True

    End Sub
    ''' <summary>
    ''' 初期化処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen(Optional ByVal blnClearFlg As Boolean = True)

        'コントロールの初期化 .
        Me.lblBbinvst_No.Text = String.Empty
        Me.lblNl_Cls.Text = String.Empty
        Me.lblEw_Flg.Text = String.Empty
        Me.txtHall_Nm.ppText = String.Empty
        Me.lblTboxClass_Cd.Text = String.Empty
        Me.lblBbTotal.Text = String.Empty
        Me.lblOrgn_Lm.Text = String.Empty
        Me.lblOrgn_Rm.Text = String.Empty
        Me.lblDpl_Lm.Text = String.Empty
        Me.lblDpl_Rm.Text = String.Empty
        Me.lblCardPay.Text = String.Empty
        Me.lblCardComsumer.Text = String.Empty
        Me.lblComMoney.Text = String.Empty
        Me.lblNot_Col1.Text = String.Empty
        Me.lblNot_Col2.Text = String.Empty

        Me.txtRepair_No.ppText = String.Empty
        Me.txtTbox_Id.ppText = String.Empty
        Me.txtSerial.ppText = String.Empty
        Me.txtInspect_M.ppText = String.Empty
        Me.txtBb_Cls.ppText = String.Empty
        Me.txtJb_No1.ppText = String.Empty
        Me.txtJb_No2.ppText = String.Empty
        Me.txtJb_No3.ppText = String.Empty
        Me.txtChecker.ppText = String.Empty
        Me.txtBrkCntnt.ppText = String.Empty
        Me.txtOth_Free.ppText = String.Empty
        Me.txtBf_Crct.ppText = String.Empty
        Me.txtAf_Crct.ppText = String.Empty
        Me.txtFsi.ppText = String.Empty
        Me.txtSim.ppText = String.Empty
        Me.txtBb_No.ppText = String.Empty
        Me.txtOrgn_No.ppText = String.Empty
        Me.txtDpl_No.ppText = String.Empty
        Me.txtOrgn_Lno.ppText = String.Empty
        Me.txtOrgn_Rno.ppText = String.Empty
        Me.txtDpl_Lno.ppText = String.Empty
        Me.txtDpl_Rno.ppText = String.Empty
        Me.txtOrgn_Pay.ppText = String.Empty
        Me.txtDpl_Pay.ppText = String.Empty
        Me.txtOrgn_Consumer.ppText = String.Empty
        Me.txtDpl_Consumer.ppText = String.Empty
        Me.txtTbox_Bb.ppText = String.Empty
        Me.txtTbox_Receipt.ppText = String.Empty
        Me.txtTbox_Cnsmp.ppText = String.Empty

        Me.dtbReceipt_D.ppText = String.Empty
        Me.dtbWrk_D.ppText = String.Empty
        Me.dtbReport_D.ppText = String.Empty
        Me.dtbBb1_send_D.ppText = String.Empty
        Me.dtbAccDt.ppText = String.Empty
        Me.dtbMng_Dt.ppText = String.Empty

        Me.chkOlddt_Flg.Checked = False
        Me.chkCrpt_Flg.Checked = False
        Me.chkBb1brk_Flg.Checked = False

        '修理依頼Noにフォーカス.
        Me.txtRepair_No.ppTextBox.Focus()

    End Sub
    ''' <summary>
    ''' 取得データ設定処理.
    ''' </summary>
    ''' <param name="dstOrders"></param>
    ''' <remarks></remarks>
    Private Sub msSetDisp_Data(ByVal dstOrders As DataSet)

        Dim int As Integer = 0
        Dim dec As Decimal = Nothing

        Me.lblBbinvst_No.Text = dstOrders.Tables(0).Rows(0).Item("BB調報No").ToString()
        Me.txtRepair_No.ppText = dstOrders.Tables(0).Rows(0).Item("修理依頼No").ToString()
        Me.txtTbox_Id.ppText = dstOrders.Tables(0).Rows(0).Item("TBOXID").ToString()
        Me.lblNl_Cls.Text = dstOrders.Tables(0).Rows(0).Item("NL区分").ToString()
        Me.lblEw_Flg.Text = dstOrders.Tables(0).Rows(0).Item("EW区分").ToString()
        Me.txtHall_Nm.ppText = dstOrders.Tables(0).Rows(0).Item("ホール名").ToString()
        ViewState("TboxCls") = dstOrders.Tables(0).Rows(0).Item("TBOX種別コード").ToString()
        Me.lblTboxClass_Cd.Text = dstOrders.Tables(0).Rows(0).Item("TBOX種別").ToString()
        'BBPUPDP001-001
        'Me.ddlMente_No.SelectedValue = dstOrders.Tables(0).Rows(0).Item("故障機型式番号").ToString()
        ViewState("katasiki") = dstOrders.Tables(0).Rows(0).Item("故障機型式番号").ToString()
        'BBPUPDP001-001　END
        Me.txtSerial.ppText = dstOrders.Tables(0).Rows(0).Item("シリアルNo").ToString()
        Me.ddlStatus.SelectedValue = dstOrders.Tables(0).Rows(0).Item("進捗状況").ToString()
        Me.dtbReceipt_D.ppText = dstOrders.Tables(0).Rows(0).Item("受領日").ToString()
        Me.dtbWrk_D.ppText = dstOrders.Tables(0).Rows(0).Item("作業日").ToString()
        Me.dtbReport_D.ppText = dstOrders.Tables(0).Rows(0).Item("報告日").ToString()
        Me.dtbBb1_send_D.ppText = dstOrders.Tables(0).Rows(0).Item("BB1送付日").ToString()
        Me.txtInspect_M.ppText = dstOrders.Tables(0).Rows(0).Item("検収月").ToString()
        Me.dtbAccDt.ppText = dstOrders.Tables(0).Rows(0).Item("事故発生日").ToString()
        Me.txtBb_Cls.ppText = dstOrders.Tables(0).Rows(0).Item("BB種別名").ToString()
        Me.txtJb_No1.ppText = dstOrders.Tables(0).Rows(0).Item("読出JJB番号1回目").ToString()
        Me.txtJb_No2.ppText = dstOrders.Tables(0).Rows(0).Item("読出JJB番号2回目").ToString()
        Me.txtJb_No3.ppText = dstOrders.Tables(0).Rows(0).Item("読出JJB番号3回目").ToString()
        Me.txtChecker.ppText = dstOrders.Tables(0).Rows(0).Item("CHECKER").ToString()
        Me.ddlLed_Flg.ppSelectedValue = dstOrders.Tables(0).Rows(0).Item("LED点灯").ToString()
        Me.ddlDst_Flg.ppSelectedValue = dstOrders.Tables(0).Rows(0).Item("データ破棄完了").ToString()
        Me.ddlResult.SelectedValue = dstOrders.Tables(0).Rows(0).Item("店内集信結果").ToString()

        Select Case dstOrders.Tables(0).Rows(0).Item("データ化け").ToString()
            Case "0"
                Me.chkCrpt_Flg.Checked = False
            Case "1"
                Me.chkCrpt_Flg.Checked = True
        End Select

        Select Case dstOrders.Tables(0).Rows(0).Item("BB1故障").ToString()
            Case "0"
                Me.chkBb1brk_Flg.Checked = False
            Case "1"
                Me.chkBb1brk_Flg.Checked = True
        End Select

        Me.txtBrkCntnt.ppText = dstOrders.Tables(0).Rows(0).Item("故障内容").ToString()
        Me.ddlOth_Cd.SelectedValue = dstOrders.Tables(0).Rows(0).Item("その他定型コード").ToString()
        Me.txtOth_Free.ppText = dstOrders.Tables(0).Rows(0).Item("その他フリー").ToString()
        Me.txtBf_Crct.ppText = dstOrders.Tables(0).Rows(0).Item("訂正前").ToString()
        Me.txtAf_Crct.ppText = dstOrders.Tables(0).Rows(0).Item("訂正後").ToString()
        Me.ddlRep_Req.ppSelectedValue = dstOrders.Tables(0).Rows(0).Item("修理依頼票").ToString()
        Me.txtFsi.ppText = dstOrders.Tables(0).Rows(0).Item("IC系BB負債額").ToString()
        Me.txtSim.ppText = dstOrders.Tables(0).Rows(0).Item("IC系BB債務額").ToString()
        Me.dtbMng_Dt.ppText = dstOrders.Tables(0).Rows(0).Item("ID系管理日付").ToString()
        Me.txtBb_No.ppText = dstOrders.Tables(0).Rows(0).Item("ID系BBNo").ToString()
        Me.txtOrgn_No.ppText = dstOrders.Tables(0).Rows(0).Item("正機番").ToString()
        Me.txtOrgn_Lno.ppText = dstOrders.Tables(0).Rows(0).Item("正機番左件数").ToString()
        Me.txtOrgn_Rno.ppText = dstOrders.Tables(0).Rows(0).Item("正機番右件数").ToString()
        Me.txtOrgn_Pay.ppText = dstOrders.Tables(0).Rows(0).Item("正機番ミニカード入金金額").ToString()
        Me.txtOrgn_Consumer.ppText = dstOrders.Tables(0).Rows(0).Item("正機番ミニカード消費金額").ToString()
        Me.txtDpl_No.ppText = dstOrders.Tables(0).Rows(0).Item("副機番").ToString()
        Me.txtDpl_Lno.ppText = dstOrders.Tables(0).Rows(0).Item("副機番左件数").ToString()
        Me.txtDpl_Rno.ppText = dstOrders.Tables(0).Rows(0).Item("副機番右件数").ToString()
        Me.lblOrgn_Lm.Text = mfSet_Money(dstOrders.Tables(0).Rows(0).Item("正機番左件数").ToString())
        Me.lblOrgn_Rm.Text = mfSet_Money(dstOrders.Tables(0).Rows(0).Item("正機番右件数").ToString())
        Me.lblDpl_Lm.Text = mfSet_Money(dstOrders.Tables(0).Rows(0).Item("副機番左件数").ToString())
        Me.lblDpl_Rm.Text = mfSet_Money(dstOrders.Tables(0).Rows(0).Item("副機番右件数").ToString())
        Me.txtDpl_Pay.ppText = dstOrders.Tables(0).Rows(0).Item("副機番ミニカード入金金額").ToString()
        Me.txtDpl_Consumer.ppText = dstOrders.Tables(0).Rows(0).Item("副機番ミニカード消費金額").ToString()
        Me.lblBbTotal.Text = mfSet_BbPrice(mfSet_Money(dstOrders.Tables(0).Rows(0).Item("正機番左件数").ToString()),
                                           mfSet_Money(dstOrders.Tables(0).Rows(0).Item("正機番右件数").ToString()),
                                           mfSet_Money(dstOrders.Tables(0).Rows(0).Item("副機番左件数").ToString()),
                                           mfSet_Money(dstOrders.Tables(0).Rows(0).Item("副機番右件数").ToString()))
        Me.lblCardPay.Text = mfSet_Money(dstOrders.Tables(0).Rows(0).Item("正機番ミニカード入金金額").ToString(),
                                         dstOrders.Tables(0).Rows(0).Item("副機番ミニカード入金金額").ToString(),
                                         True)
        Me.lblCardComsumer.Text = mfSet_Money(dstOrders.Tables(0).Rows(0).Item("正機番ミニカード消費金額").ToString(),
                                              dstOrders.Tables(0).Rows(0).Item("副機番ミニカード消費金額").ToString(),
                                              True)
        Me.lblComMoney.Text = mfSet_Money(Me.lblBbTotal.Text, dstOrders.Tables(0).Rows(0).Item("TBOX集信分1").ToString(), False)
        Me.lblNot_Col1.Text = mfSet_Money(Me.lblCardPay.Text, dstOrders.Tables(0).Rows(0).Item("TBOX集信分2").ToString(), False)
        Me.lblNot_Col2.Text = mfSet_Money(Me.lblCardComsumer.Text, dstOrders.Tables(0).Rows(0).Item("TBOX集信分3").ToString(), False)

        Me.txtTbox_Bb.ppText = dstOrders.Tables(0).Rows(0).Item("TBOX集信分1").ToString()
        Me.txtTbox_Receipt.ppText = dstOrders.Tables(0).Rows(0).Item("TBOX集信分2").ToString()
        Me.txtTbox_Cnsmp.ppText = dstOrders.Tables(0).Rows(0).Item("TBOX集信分3").ToString()

        Select Case dstOrders.Tables(0).Rows(0).Item("日付古い").ToString()
            Case "0"
                Me.chkOlddt_Flg.Checked = False
            Case "1"
                Me.chkOlddt_Flg.Checked = True
                If Decimal.TryParse(Me.lblBbTotal.Text, dec) = True And dec > 0 Then
                    Me.txtTbox_Bb.ppText = "TBOX照会不可"
                    Me.lblComMoney.Text = String.Empty
                End If

                If Decimal.TryParse(Me.lblCardPay.Text, dec) = True And dec > 0 Then
                    Me.txtTbox_Receipt.ppText = "TBOX照会不可"
                    Me.lblNot_Col1.Text = String.Empty
                End If

                If Decimal.TryParse(Me.lblCardComsumer.Text, dec) = True And dec > 0 Then
                    Me.txtTbox_Cnsmp.ppText = "TBOX照会不可"
                    Me.lblNot_Col2.Text = String.Empty
                End If
        End Select

    End Sub
    ''' <summary>
    ''' 活性制御.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msEnableScreen(ByVal strTerms As String)

        Select Case strTerms.ToString.Trim

            Case ClsComVer.E_遷移条件.参照

                Me.txtRepair_No.ppEnabled = False
                Me.txtTbox_Id.ppEnabled = False
                Me.ddlMente_No.Enabled = False
                Me.txtSerial.ppEnabled = False
                Me.ddlStatus.Enabled = False
                Me.dtbReceipt_D.ppEnabled = False
                Me.dtbWrk_D.ppEnabled = False
                Me.dtbReport_D.ppEnabled = False
                Me.dtbBb1_send_D.ppEnabled = False
                Me.txtInspect_M.ppEnabled = False
                Me.dtbAccDt.ppEnabled = False
                Me.txtBb_Cls.ppEnabled = False
                Me.txtJb_No1.ppEnabled = False
                Me.txtJb_No2.ppEnabled = False
                Me.txtJb_No3.ppEnabled = False
                Me.txtChecker.ppEnabled = False
                Me.ddlLed_Flg.ppEnabled = False
                Me.ddlDst_Flg.ppEnabled = False
                Me.ddlResult.Enabled = False
                Me.txtBrkCntnt.ppEnabled = False
                Me.ddlOth_Cd.Enabled = False
                Me.txtOth_Free.ppEnabled = False
                Me.txtBf_Crct.ppEnabled = False
                Me.txtAf_Crct.ppEnabled = False
                Me.ddlRep_Req.ppEnabled = False
                Me.txtFsi.ppEnabled = False
                Me.txtSim.ppEnabled = False
                Me.dtbMng_Dt.ppEnabled = False
                Me.txtBb_No.ppEnabled = False
                Me.txtOrgn_No.ppEnabled = False
                Me.txtOrgn_Lno.ppEnabled = False
                Me.txtOrgn_Rno.ppEnabled = False
                Me.txtOrgn_Pay.ppEnabled = False
                Me.txtOrgn_Consumer.ppEnabled = False
                Me.txtDpl_No.ppEnabled = False
                Me.txtDpl_Lno.ppEnabled = False
                Me.txtDpl_Rno.ppEnabled = False
                Me.txtDpl_Pay.ppEnabled = False
                Me.txtDpl_Consumer.ppEnabled = False
                Me.txtTbox_Bb.ppEnabled = False
                Me.txtTbox_Receipt.ppEnabled = False
                Me.txtTbox_Cnsmp.ppEnabled = False
                Master.ppRigthButton1.Enabled = False
                Master.ppRigthButton2.Enabled = False
                Master.ppRigthButton3.Enabled = False
                Master.ppRigthButton4.Enabled = False

                Me.chkBb1brk_Flg.Enabled = False
                Me.chkCrpt_Flg.Enabled = False
                Me.chkOlddt_Flg.Enabled = False

            Case ClsComVer.E_遷移条件.更新

                Me.txtRepair_No.ppEnabled = True
                Me.txtTbox_Id.ppEnabled = True
                Me.ddlMente_No.Enabled = True
                Me.txtSerial.ppEnabled = True
                Me.ddlStatus.Enabled = True
                Me.dtbReceipt_D.ppEnabled = True
                Me.dtbWrk_D.ppEnabled = True
                Me.dtbReport_D.ppEnabled = True
                Me.dtbBb1_send_D.ppEnabled = True
                Me.txtInspect_M.ppEnabled = True
                Me.dtbAccDt.ppEnabled = True
                Me.txtBb_Cls.ppEnabled = True
                Me.txtJb_No1.ppEnabled = True
                Me.txtJb_No2.ppEnabled = True
                Me.txtJb_No3.ppEnabled = True
                Me.txtChecker.ppEnabled = True
                Me.ddlLed_Flg.ppEnabled = True
                Me.ddlDst_Flg.ppEnabled = True
                Me.ddlResult.Enabled = True
                Me.txtBrkCntnt.ppEnabled = True
                Me.ddlOth_Cd.Enabled = True
                Me.txtOth_Free.ppEnabled = True
                Me.txtBf_Crct.ppEnabled = True
                Me.txtAf_Crct.ppEnabled = True
                Me.ddlRep_Req.ppEnabled = True
                Me.txtFsi.ppEnabled = True
                Me.txtSim.ppEnabled = True
                Me.dtbMng_Dt.ppEnabled = True
                Me.txtBb_No.ppEnabled = True
                Me.txtOrgn_No.ppEnabled = True
                Me.txtOrgn_Lno.ppEnabled = True
                Me.txtOrgn_Rno.ppEnabled = True
                Me.txtOrgn_Pay.ppEnabled = True
                Me.txtOrgn_Consumer.ppEnabled = True
                Me.txtDpl_No.ppEnabled = True
                Me.txtDpl_Lno.ppEnabled = True
                Me.txtDpl_Rno.ppEnabled = True
                Me.txtDpl_Pay.ppEnabled = True
                Me.txtDpl_Consumer.ppEnabled = True
                Me.txtTbox_Bb.ppEnabled = True
                Me.txtTbox_Receipt.ppEnabled = True
                Me.txtTbox_Cnsmp.ppEnabled = True

                Master.ppRigthButton1.Enabled = True
                Master.ppRigthButton2.Enabled = True
                Master.ppRigthButton3.Enabled = True
                Master.ppRigthButton4.Enabled = True

                Me.chkBb1brk_Flg.Enabled = True
                Me.chkCrpt_Flg.Enabled = True
                Me.chkOlddt_Flg.Enabled = True

            Case ClsComVer.E_遷移条件.登録

                Me.txtRepair_No.ppEnabled = True
                Me.txtTbox_Id.ppEnabled = True
                Me.ddlMente_No.Enabled = True
                Me.txtSerial.ppEnabled = True
                Me.ddlStatus.Enabled = True
                Me.dtbReceipt_D.ppEnabled = True
                Me.dtbWrk_D.ppEnabled = True
                Me.dtbReport_D.ppEnabled = True
                Me.dtbBb1_send_D.ppEnabled = True
                Me.txtInspect_M.ppEnabled = True
                Me.dtbAccDt.ppEnabled = True
                Me.txtBb_Cls.ppEnabled = True
                Me.txtJb_No1.ppEnabled = True
                Me.txtJb_No2.ppEnabled = True
                Me.txtJb_No3.ppEnabled = True
                Me.txtChecker.ppEnabled = True
                Me.ddlLed_Flg.ppEnabled = True
                Me.ddlDst_Flg.ppEnabled = True
                Me.ddlResult.Enabled = True
                Me.txtBrkCntnt.ppEnabled = True
                Me.ddlOth_Cd.Enabled = True
                Me.txtOth_Free.ppEnabled = True
                Me.txtBf_Crct.ppEnabled = True
                Me.txtAf_Crct.ppEnabled = True
                Me.ddlRep_Req.ppEnabled = True
                Me.txtFsi.ppEnabled = True
                Me.txtSim.ppEnabled = True
                Me.dtbMng_Dt.ppEnabled = True
                Me.txtBb_No.ppEnabled = True
                Me.txtOrgn_No.ppEnabled = True
                Me.txtOrgn_Lno.ppEnabled = True
                Me.txtOrgn_Rno.ppEnabled = True
                Me.txtOrgn_Pay.ppEnabled = True
                Me.txtOrgn_Consumer.ppEnabled = True
                Me.txtDpl_No.ppEnabled = True
                Me.txtDpl_Lno.ppEnabled = True
                Me.txtDpl_Rno.ppEnabled = True
                Me.txtDpl_Pay.ppEnabled = True
                Me.txtDpl_Consumer.ppEnabled = True
                Me.txtTbox_Bb.ppEnabled = True
                Me.txtTbox_Receipt.ppEnabled = True
                Me.txtTbox_Cnsmp.ppEnabled = True

                Master.ppRigthButton1.Enabled = True
                Master.ppRigthButton2.Enabled = True
                Master.ppRigthButton3.Enabled = True
                Master.ppRigthButton4.Enabled = True

                Me.chkBb1brk_Flg.Enabled = True
                Me.chkCrpt_Flg.Enabled = True
                Me.chkOlddt_Flg.Enabled = True
        End Select

    End Sub
    ''' <summary>
    ''' 個別エラーチェック.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheck_Error()

        Dim strErr As String = String.Empty
        Dim intNum As Integer = 0

        ''修理依頼No整合性チェック.
        'strErr = pfCheck_TxtErr(Me.txtRepair_No.ppText, False, False, False, False, 8, "[a-zA-Z][0-9][0-9][0-9][0-9][0-9][0-9][0-9]", False)    'BBPUPDP001_003
        'strErr = pfCheck_TxtErr(Me.txtRepair_No.ppText, False, False, False, False, 8, "[0-9a-zA-Z][0-9][0-9][0-9][0-9][0-9][0-9][0-9]", False)    'BBPUPDP001_003
        'If strErr <> String.Empty Then
        '    Me.txtRepair_No.psSet_ErrorNo(strErr, "先頭1桁", "英字")
        'End If

        '検収月整合性チェック.
        strErr = pfCheck_TxtErr(Me.txtInspect_M.ppText, False, False, False, True, 4, String.Empty, False)
        If strErr <> String.Empty Then
            Me.txtInspect_M.psSet_ErrorNo(strErr, "検収月", "4桁")
        End If

        '検収月整合性チェック.
        strErr = pfCheck_TxtErr(txtInspect_M.ppText, False, True, True, True, 4, "[0-9][0-9]([0][1-9]|[1][0-2])", False)
        If strErr <> "" Then
            Me.txtInspect_M.psSet_ErrorNo(strErr, "検収月", "年月")
        End If

        '読出JB番号1回目入力チェック.
        strErr = pfCheck_TxtErr(txtJb_No1.ppText, False, True, False, False, 4, "", False)
        If strErr <> "" Then
            Me.txtJb_No1.psSet_ErrorNo(strErr, "読出JB番号1回目", "数字")
        End If

        '読出JB番号2回目入力チェック.
        strErr = pfCheck_TxtErr(txtJb_No2.ppText, False, True, False, False, 4, "", False)
        If strErr <> "" Then
            Me.txtJb_No2.psSet_ErrorNo(strErr, "読出JB番号2回目", "数字")
        End If

        '読出JB番号3回目入力チェック.
        strErr = pfCheck_TxtErr(txtJb_No3.ppText, False, True, False, False, 4, "", False)
        If strErr <> "" Then
            Me.txtJb_No3.psSet_ErrorNo(strErr, "読出JB番号3回目", "数字")
        End If

        'BB負債額.
        strErr = pfCheck_TxtErr(txtFsi.ppText, False, True, False, False, 6, "", False)
        If strErr <> "" Then
            Me.txtFsi.psSet_ErrorNo(strErr, "BB負債額", "数字")
        End If

        'BB債務額.
        strErr = pfCheck_TxtErr(txtSim.ppText, False, True, False, False, 6, "", False)
        If strErr <> "" Then
            Me.txtSim.psSet_ErrorNo(strErr, "BB債務額", "数字")
        End If

        'BBNo.
        strErr = pfCheck_TxtErr(txtBb_No.ppText, False, True, False, False, 4, "", False)
        If strErr <> "" Then
            Me.txtBb_No.psSet_ErrorNo(strErr, "BBNo", "数字")
        End If

        '読出JB番号1回目.
        If Me.txtJb_No1.ppText > "9999" Then
            Me.txtJb_No1.psSet_ErrorNo("6001", "読出JB番号1回目", "9999")
        End If

        '読出JB番号2回目.
        If Me.txtJb_No2.ppText > "9999" Then
            Me.txtJb_No2.psSet_ErrorNo("6001", "読出JB番号2回目", "9999")
        End If

        '読出JB番号3回目.
        If Me.txtJb_No3.ppText > "9999" Then
            Me.txtJb_No3.psSet_ErrorNo("6001", "読出JB番号3回目", "9999")
        End If

        'BB負債額.
        If Me.txtFsi.ppText > "999999" Then
            Me.txtFsi.psSet_ErrorNo("6001", "BB負債額", "999999")
        End If

        'BB債務額.
        If Me.txtSim.ppText > "999999" Then
            Me.txtSim.psSet_ErrorNo("6001", "BB債務額", "999999")
        End If

        'BBNo.
        If Me.txtBb_No.ppText > "9999" Then
            Me.txtBb_No.psSet_ErrorNo("6001", "BBNo", "9999")
        End If

        'TBOX集信分.
        strErr = pfCheck_TxtErr(Me.txtTbox_Bb.ppText, False, True, False, False, 8, "", False)
        If strErr <> "" Then
            If Me.txtTbox_Bb.ppText <> "TBOX照会不可" Then
                Me.txtTbox_Bb.psSet_ErrorNo(strErr, "TBOX集信分")
            End If
        End If

        'TBOX集信分.
        strErr = pfCheck_TxtErr(Me.txtTbox_Receipt.ppText, False, True, False, False, 8, "", False)
        If strErr <> "" Then
            If Me.txtTbox_Receipt.ppText <> "TBOX照会不可" Then
                Me.txtTbox_Receipt.psSet_ErrorNo(strErr, "TBOX集信分")
            End If
        End If

        'TBOX集信分.
        strErr = pfCheck_TxtErr(Me.txtTbox_Cnsmp.ppText, False, True, False, False, 8, "", False)
        If strErr <> "" Then
            If Me.txtTbox_Cnsmp.ppText <> "TBOX照会不可" Then
                Me.txtTbox_Cnsmp.psSet_ErrorNo(strErr, "TBOX集信分")
            End If
        End If

        '--------------------------------
        '2014/04/19 後藤　ここから
        '--------------------------------
        Select Case Me.ddlRep_Req.ppSelectedValue.Trim
            Case String.Empty
                '訂正前.
                If Me.txtBf_Crct.ppText = String.Empty And Me.txtAf_Crct.ppText = String.Empty Then
                Else
                    Me.ddlRep_Req.psSet_ErrorNo("5001", "修理依頼表")
                End If

            Case 1
                strErr = pfCheck_TxtErr(Me.txtBf_Crct.ppText, False, True, False, False, 8, "", False)
                If strErr <> "" Then
                    Me.txtBf_Crct.psSet_ErrorNo(strErr, "訂正前", "8")
                End If

                strErr = pfCheck_TxtErr(Me.txtAf_Crct.ppText, False, True, False, False, 8, "", False)
                If strErr <> "" Then
                    Me.txtAf_Crct.psSet_ErrorNo(strErr, "訂正後", "8")
                End If
            Case 2
                '訂正前.
                If Me.txtBf_Crct.ppText.Length > 40 Then
                    Me.txtBf_Crct.psSet_ErrorNo("3002", "訂正前", "40")
                End If

                '訂正後.
                If Me.txtAf_Crct.ppText.Length > 40 Then
                    Me.txtAf_Crct.psSet_ErrorNo("3002", "訂正後", "40")
                End If
        End Select
        '--------------------------------
        '2014/04/19 後藤　ここまで
        '--------------------------------

        '--------------------------------
        '2014/05/13 後藤　ここから
        '--------------------------------
        If mfChk_Tboxid() = False Then
            txtTbox_Id.psSet_ErrorNo("2002", "入力されたTBOXID")
        End If
        '--------------------------------
        '2014/05/13 後藤　ここまで
        '--------------------------------

    End Sub
    ''' <summary>
    ''' ブラックボックス調査報告書情報取得処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfGet_Data(ByVal ipstrBbrep_No As String, ByVal intSeachFlg As Integer) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet
        Dim cmdDBList As New SqlCommand
        Dim dstOrdersList As New DataSet
        Dim strMessage As String = String.Empty
        mfGet_Data = False
        Dim shtWork As Short = 0

        If intSeachFlg = 0 Then
            shtWork = ClsComVer.E_S実行.描画前
        Else
            shtWork = ClsComVer.E_S実行.描画後
        End If

        objStack = New StackFrame

        Try

            '開始ログ出力.
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, sCnsErr_00005, ClsComVer.E_Mタイプ.エラー, shtWork)
                Exit Function
            Else

                'ブラックボックス調査報告書データ取得.
                cmdDB = New SqlCommand(M_DISP_ID & "_S1", conDB)
                cmdDB.Parameters.Add(pfSet_Param("bbrep_no", SqlDbType.NVarChar, ipstrBbrep_No))
                dstOrdersList = clsDataConnect.pfGet_DataSet(cmdDB)

                'TBOXマスタデータ取得
                cmdDB = New SqlCommand(M_DISP_ID & "_S2", conDB)
                cmdDB.Parameters.Add(pfSet_Param("tboxid", SqlDbType.NVarChar, dstOrdersList.Tables(0).Rows(0).Item("TBOXID").ToString()))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '店内集信結果ドロップダウン設定.
                cmdDB = New SqlCommand(M_DISP_ID & "_S5", conDB)
                Select Case dstOrders.Tables(0).Rows(0).Item("システム分類").ToString()
                    Case "1"
                        cmdDB.Parameters.Add(pfSet_Param("class_cd", SqlDbType.NVarChar, "0077"))
                    Case Else
                        cmdDB.Parameters.Add(pfSet_Param("class_cd", SqlDbType.NVarChar, "0076"))
                End Select

                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                Me.ddlResult.DataSource = dstOrders.Tables(0)
                Me.ddlResult.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
                Me.ddlResult.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
                Me.ddlResult.DataBind()
                Me.ddlResult.Items.Insert(0, "")

            End If

            '取得したデータをコントロールに設定.
            Call msSetDisp_Data(dstOrdersList)

            '正常.
            mfGet_Data = True

        Catch ex As Exception
            psMesBox(Me, sCnsErr_00004, ClsComVer.E_Mタイプ.エラー, shtWork, sCnsDispNm)

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Exit Function
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, sCnsErr_00006, ClsComVer.E_Mタイプ.エラー, shtWork)
                mfGet_Data = False
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function
    ''' <summary>
    ''' ドロップダウンリスト生成.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfSet_DropDownList() As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet        '表示情報のデータセット
        Dim strMessage As String = String.Empty

        objStack = New StackFrame
        mfSet_DropDownList = False
        Dim strSetval As String

        Try

            '開始ログ出力.
            psLogStart(Me)

            '接続.
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, sCnsErr_00005, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                Exit Function
            End If

            cmdDB = New SqlCommand(M_DISP_ID & "_S3", conDB)
            strMessage = "型式マスタ"
            strSetval = Me.ddlMente_No.SelectedValue
            'BBPUPDP001-001
            cmdDB.Parameters.Add(pfSet_Param("tboxid", SqlDbType.NVarChar, ""))
            'BBPUPDP001-001 END
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlMente_No.DataSource = dstOrders.Tables(0)
            Me.ddlMente_No.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlMente_No.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlMente_No.DataBind()
            Me.ddlMente_No.Items.Insert(0, "")
            Me.ddlMente_No.SelectedValue = strSetval

            cmdDB = New SqlCommand(M_DISP_ID & "_S4", conDB)
            strMessage = "表示項目マスタ"
            strSetval = Me.ddlOth_Cd.SelectedValue
            cmdDB.Parameters.Add(pfSet_Param("dispid", SqlDbType.NVarChar, M_DISP_ID))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            Me.ddlOth_Cd.Items.Clear()
            Me.ddlOth_Cd.DataSource = dstOrders.Tables(0)
            Me.ddlOth_Cd.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlOth_Cd.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlOth_Cd.DataBind()
            Me.ddlOth_Cd.Items.Insert(0, "")
            Me.ddlOth_Cd.Items(0).Value = ""
            Me.ddlOth_Cd.SelectedValue = strSetval

            cmdDB = New SqlCommand(sCnsSql_id015, conDB)

            strMessage = "進捗ステータスマスタ"
            strSetval = Me.ddlStatus.SelectedValue
            cmdDB.Parameters.Add(pfSet_Param("duty_cd", SqlDbType.NVarChar, "91"))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlStatus.DataSource = dstOrders.Tables(0)
            Me.ddlStatus.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlStatus.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlStatus.DataBind()

            Me.ddlStatus.Items.Insert(0, "")
            Me.ddlStatus.SelectedValue = strSetval
            '正常.
            mfSet_DropDownList = True

        Catch ex As Exception
            psMesBox(Me, sCnsErr_00004, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, strMessage)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Exit Function
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, sCnsErr_00006, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                mfSet_DropDownList = False
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function
    ''' <summary>
    ''' ＢＢ累計金額計算処理.
    ''' </summary>
    ''' <param name="strPriceOlm">正機番左金額</param>
    ''' <param name="strPriceOrm">正機番右金額</param>
    ''' <param name="strPriceDlm">副機番左金額</param>
    ''' <param name="strPriceDrm">副機番右金額</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfSet_BbPrice(ByVal strPriceOlm As String,
                                   ByVal strPriceOrm As String,
                                   ByVal strPriceDlm As String,
                                   ByVal strPriceDrm As String) As String

        Dim decSum As Decimal = 0
        Dim decPriceOlm As Decimal
        Dim decPriceOrm As Decimal
        Dim decPriceDlm As Decimal
        Dim decPriceDrm As Decimal

        objStack = New StackFrame

        Try
            If strPriceOlm <> "" OrElse strPriceOrm <> "" OrElse strPriceDlm <> "" OrElse strPriceDrm <> "" Then
                Decimal.TryParse(strPriceOlm, decPriceOlm)
                Decimal.TryParse(strPriceOrm, decPriceOrm)
                Decimal.TryParse(strPriceDlm, decPriceDlm)
                Decimal.TryParse(strPriceDrm, decPriceDrm)

                decSum = decPriceOlm + decPriceOrm + decPriceDlm + decPriceDrm

                Me.lblBbTotal.Text = decSum.ToString
            End If

        Catch ex As Exception
            psMesBox(Me, "30006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        If strPriceOlm <> "" OrElse strPriceOrm <> "" OrElse strPriceDlm <> "" OrElse strPriceDrm <> "" Then
            Return decSum.ToString("#,##0")
        Else
            Return Nothing
        End If


    End Function
    ''' <summary>
    ''' 日付変換処理.
    ''' </summary>
    ''' <param name="strReport_D">日付</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfChange_Jp(ByVal strReport_D As String) As String

        Dim strYear As String = String.Empty
        Dim strMouth As String = String.Empty
        Dim strDay As String = String.Empty
        Dim dtTarget As DateTime = Nothing
        Dim culture As CultureInfo = New CultureInfo("ja-JP", True)
        culture.DateTimeFormat.Calendar = New JapaneseCalendar()
        objStack = New StackFrame

        If strReport_D.Length = 0 Then
            Return String.Empty
        End If

        Try
            '年・月・日に分割.
            strYear = strReport_D.Substring(0, 4)
            strMouth = strReport_D.Substring(5, 2)
            strDay = strReport_D.Substring(8, 2)
            dtTarget = New DateTime(strYear, strMouth, strDay)

        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "日付の変換")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        'Return dtTarget.ToString("ggyy年M月d日", culture)
        Return dtTarget.ToString("yyyy年M月d日")

    End Function
    ''' <summary>
    ''' 金額計算(掛け算).
    ''' </summary>
    ''' <param name="strCount">件数</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Overloads Function mfSet_Money(ByVal strCount As String) As String

        Dim decResult As Decimal = Nothing
        Dim decPrice As Decimal = Nothing

        objStack = New StackFrame

        Try
            If strCount <> "" Then
                Decimal.TryParse(strCount, decResult)
                decPrice = decResult * 100
            End If

        Catch ex As Exception
            psMesBox(Me, "30006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        If strCount <> "" Then
            Return decPrice.ToString("#,##0")
        Else
            Return Nothing
        End If

    End Function
    ''' <summary>
    ''' 金額計算(足し算・引き算).
    ''' </summary>
    ''' <param name="strNo"></param>
    ''' <param name="strAdd"></param>
    ''' <param name="blnAddition_flg">True=足し算,False=引き算</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Overloads Function mfSet_Money(ByVal strNo As String,
                                           ByVal strAdd As String,
                                           ByVal blnAddition_flg As Boolean) As String

        Dim decPriceOl As Decimal = Nothing
        Dim decPriceDl As Decimal = Nothing
        Dim decSum As Decimal = Nothing

        objStack = New StackFrame

        Try
            If strNo <> "" OrElse strAdd <> "" Then
                Decimal.TryParse(strNo, decPriceOl)
                Decimal.TryParse(strAdd, decPriceDl)

                If blnAddition_flg = True Then
                    decSum = decPriceOl + decPriceDl
                Else
                    decSum = decPriceOl - decPriceDl
                End If
            End If

        Catch ex As Exception
            psMesBox(Me, "30006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        If strNo <> "" OrElse strAdd <> "" Then
            Return decSum.ToString("#,##0")
        Else
            Return Nothing
        End If

    End Function
    ''' <summary>
    ''' DBNULL取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDBNull(ByVal strVal As String) As Object
        If strVal.Trim() = "" OrElse strVal.Trim() = "TBOX照会不可" Then
            Return DBNull.Value
        End If
        Return strVal

    End Function
    ''' <summary>
    ''' オブジェクト取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetZero(ByVal strVal As String) As String
        If strVal.Trim() = "TBOX照会不可" Then
            Return "0"
        End If
        Return strVal

    End Function
    ''' <summary>
    ''' TBOXID存在チェック
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfChk_Tboxid() As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        mfChk_Tboxid = False

        objStack = New StackFrame

        Try

            '開始ログ出力
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, sCnsErr_00005, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else

                'リストデータ取得
                cmdDB = New SqlCommand(M_DISP_ID & "_S2", conDB)
                cmdDB.Parameters.Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTbox_Id.ppText))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'チェック結果判定
                If dstOrders.Tables(0).Rows.Count = 0 Then
                Else
                    mfChk_Tboxid = True
                End If

            End If

        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ情報取得")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, sCnsErr_00006, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try
    End Function
    ''' <summary>
    ''' ドロップダウンリスト生成(型式マスタ用) 
    ''' </summary>
    ''' <remarks></remarks>
    Private Function setDDLModel() As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet        '表示情報のデータセット
        Dim strMessage As String = String.Empty
        objStack = New StackFrame
        setDDLModel = False
        Dim strSetval As String

        Try

            '開始ログ出力.
            psLogStart(Me)

            '接続.
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, sCnsErr_00005, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                Exit Function
            End If

            cmdDB = New SqlCommand(M_DISP_ID & "_S3", conDB)

            strMessage = "型式マスタ"

            strSetval = Me.ddlMente_No.SelectedValue
            cmdDB.Parameters.Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTbox_Id.ppText))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            'データセットに直接空白データ挿入(選択リスト未選択でデータバインド時のエラー回避)
            Dim drw As DataRow = dstOrders.Tables(0).NewRow
            drw(dstOrders.Tables(0).Columns(0).ColumnName.ToString) = ""
            drw(dstOrders.Tables(0).Columns(1).ColumnName.ToString) = ""
            dstOrders.Tables(0).Rows.InsertAt(drw, 0)

            Me.ddlMente_No.DataSource = dstOrders.Tables(0)
            Me.ddlMente_No.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlMente_No.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlMente_No.DataBind()
            Me.ddlMente_No.SelectedValue = strSetval
            setDDLModel = True

        Catch ex As Exception
            psMesBox(Me, sCnsErr_00004, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, strMessage)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Exit Function

        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, sCnsErr_00006, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                setDDLModel = False
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function
#End Region

End Class