'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　工事料金明細書一覧
'*　ＰＧＭＩＤ：　CNSLSTP006
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.3 ：土岐
'*  変　更　　：　2017.03.16：伯野
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'CNSUPDP006-001     2015/05/08      加賀      検索時 締め日形式チェック   修正ストアド:CNSUPDP006_S1           
'CNSUPDP006-002     2017/03/16      伯野      検索条件　ＮＬ区分　工事種別　追加


#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax
'-----------------------------
'2014/04/21 土岐　ここから
'-----------------------------
'排他制御用
Imports SPC.ClsCMExclusive
'-----------------------------
'2014/04/21 土岐　ここまで
'-----------------------------
#End Region

#Region "クラス定義"
Public Class CNSLSTP006
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
    Private Const M_DISP_ID = P_FUN_CNS & P_SCR_LST & P_PAGE & "006"
    Private Const M_DTL_FILE_NM = "機器代金及び工事料金明細一覧（明細）"  '機器代金及び工事料金明細一覧（明細）
    Private Const M_SUM_FILE_NM = "機器代金及び工事料金明細一覧（合計）"  '機器代金及び工事料金明細一覧（合計）
    Private Const M_VIEW_DATE_FROM = "工事日From"
    Private Const M_VIEW_DATE_TO = "工事日To"
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
    '--------------------------------
    '2014/04/14 星野　ここから
    '--------------------------------
    Dim objStack As StackFrame
    '--------------------------------
    '2014/04/14 星野　ここまで
    '--------------------------------
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom
    Dim clsExc As New ClsCMExclusive
    Dim ttlPrice As Integer = 0

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
        If Session(P_SESSION_AUTH) = "管理者" Then
            pfSet_GridView(grvList, "CNSLSTP006")
        Else
            pfSet_GridView(grvList, "CNSLSTP006_2")
        End If

    End Sub

    ''' <summary>
    ''' ページロード
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click

        AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnPDF_Click
        AddHandler Master.Master.ppRigthButton2.Click, AddressOf btnPrint_Click
        AddHandler Master.Master.ppRigthButton3.Click, AddressOf btnCSV_Click

        If Not IsPostBack Then  '初回表示
            'プログラムＩＤ、画面名設定
            Master.Master.ppProgramID = M_DISP_ID
            Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

            'パンくずリスト設定
            Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

            'フッターのボタン設定
            Master.Master.ppRigthButton1.Visible = True
            Master.Master.ppRigthButton1.Text = P_BTN_NM_PDF
            Master.Master.ppRigthButton1.OnClientClick =
                pfGet_OCClickMes("10001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, P_BTN_NM_PDF)
            Master.Master.ppRigthButton2.Visible = True
            Master.Master.ppRigthButton2.Text = P_BTN_NM_PRI
            Master.Master.ppRigthButton2.OnClientClick =
                pfGet_OCClickMes("10001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "機器代金及び工事料金明細一覧")
            Master.Master.ppRigthButton3.Visible = True
            Master.Master.ppRigthButton3.Text = "ＣＳＶ"
            Master.Master.ppRigthButton3.OnClientClick =
                pfGet_OCClickMes("10001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "ＣＳＶ")

            '「クリア」ボタン押下時の検証を無効
            Master.ppRigthButton2.CausesValidation = False

            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            '締日（期間）リスト設定
            msSetcblClosingDt()

            'クリア処理
            msClearScreen()

            '工事依頼番号にフォーカス移動
            Me.txtCnsReqestNo.ppTextBox.Focus()

            '検索項目 幅設定
            tftTboxId.ppTextBoxFrom.Width = 65
            tftTboxId.ppTextBoxTo.Width = 65
            txtClosingDt.ppTextBox.Width = 40

        End If
    End Sub

    ''' <summary>
    ''' ページ描画前
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        If Session(P_SESSION_AUTH) <> "管理者" Then
            Me.lblTotal.Visible = False
            Me.lblTotalPrice.Visible = False
            Master.Master.ppRigthButton3.Visible = False
        End If

    End Sub

    ''' <summary>
    ''' グリッドビューデータ連携時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        If Session(P_SESSION_AUTH) = "管理者" Then
            For zz = 0 To grvList.Rows.Count - 1
                Dim obj As Object = grvList.Rows(zz).Cells(36).Controls(0)
                If obj.Text <> String.Empty Then
                    ttlPrice += obj.Text.ToString.Replace(",", "")
                    obj.Text = "\" + obj.Text
                End If
            Next

            lblTotalPrice.Text = "\" + ttlPrice.ToString("#,##0")
        End If

    End Sub

    ''' <summary>
    ''' グリッドビューソート
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles grvList.Sorting

        If Session(P_SESSION_AUTH) = "管理者" Then
            For zz = 0 To grvList.Rows.Count - 1
                Dim obj As Object = grvList.Rows(zz).Cells(36).Controls(0)
                If obj.Text <> String.Empty Then
                    obj.Text = obj.Text.ToString.Replace("\", "")
                End If
            Next
        End If

    End Sub

    '---------------------------
    '2014/04/14 武 ここから
    '---------------------------
    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grvList.RowDataBound

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
                e.Row.Cells(0).Enabled = False
                e.Row.Cells(1).Enabled = False
            Case "NGC"
                e.Row.Cells(0).Enabled = False
                e.Row.Cells(1).Enabled = False
        End Select

    End Sub
    '---------------------------
    '2014/04/14 武 ここまで
    '---------------------------

    ''' <summary>
    ''' 検索クリアボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)
        'ログ出力開始
        psLogStart(Me)
        'クリア処理
        msClearScreen()
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 検索ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)
        'ログ出力開始
        psLogStart(Me)

        'CNSUPDP006-001 締め日　形式チェック
        If txtClosingDt.ppText = String.Empty OrElse (Regex.IsMatch(txtClosingDt.ppText, "^[0-9]{2}/[0-9]{2}$") _
                                                      AndAlso _
                                                      DateTime.TryParse("20" & txtClosingDt.ppText & "/01", New DateTime)) Then
            'CNSUPDP006-001 END
            If (Page.IsValid) Then
                '検索
                Dim CnstFrom As String = String.Empty
                Dim CnstTo As String = String.Empty
                If Me.dtbCnstFm.ppText = String.Empty AndAlso Me.dtbCnstFm.ppHourText = String.Empty AndAlso Me.dtbCnstFm.ppMinText = String.Empty Then
                Else
                    CnstFrom = Me.dtbCnstFm.ppText + " " + Me.dtbCnstFm.ppHourText + ":" + Me.dtbCnstFm.ppMinText
                End If
                If Me.dtbCnstTo.ppText = String.Empty AndAlso Me.dtbCnstTo.ppHourText = String.Empty AndAlso Me.dtbCnstTo.ppMinText = String.Empty Then
                Else
                    CnstTo = Me.dtbCnstTo.ppText + " " + Me.dtbCnstTo.ppHourText + ":" + Me.dtbCnstTo.ppMinText
                End If
                msGet_Data(Me.tftTboxId.ppFromText, Me.tftTboxId.ppToText, _
                           Me.txtCnsReqestNo.ppText, _
                           Me.txtHoleNm.ppText, _
                           CnstFrom, CnstTo, _
                           Me.txtClosingDt.ppText.Replace("/", ""), mfGet_ClosingDtT, _
                            Me.txtNLCls.ppText, _
                            Me.cbxNew.Checked, Me.cbxExpansion.Checked, Me.cbxSomeRemoval.Checked, _
                            Me.cbxShopRelocation.Checked, Me.cbxAllRemoval.Checked, _
                            Me.cbxOnceRemoval.Checked, Me.cbxReInstallation.Checked, _
                            Me.cbxConChange.Checked, Me.cbxConDelivery.Checked, _
                            Me.cbxOther.Checked, Me.cbxVup.Checked)
            End If
            'CNSUPDP006-001
        Else
            '締め日 形式エラー
            txtClosingDt.psSet_ErrorNo("4001", txtClosingDt.ppName, "正しい年月")
        End If
        'CNSUPDP006-001 END

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 一覧の更新／参照ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand
        '工事料金明細書 画面のパス
        Const CNSINQP001 As String = "~/" & P_CNS & "/" &
            P_FUN_CNS & P_SCR_INQ & P_PAGE & "001" & "/" &
            P_FUN_CNS & P_SCR_INQ & P_PAGE & "001.aspx"

        Dim intIndex As Integer = Nothing
        Dim rowData As GridViewRow = Nothing

        If e.CommandName <> "btnReference" And e.CommandName <> "btnUpdate" Then
            Exit Sub
        End If

        'ログ出力開始
        psLogStart(Me)

        intIndex = Convert.ToInt32(e.CommandArgument)   ' ボタン押下行のIndex
        rowData = grvList.Rows(intIndex)                ' ボタン押下行

        'セッション情報設定
        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text                     'パンくずリスト
        Session(P_KEY) = {CType(rowData.FindControl("工事依頼番号"), TextBox).Text} '詳細キー項目(工事依頼番号)
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号
        Select Case e.CommandName
            Case "btnReference"     ' 参照
                '--------------------------------
                '2015/04/06 加賀　ここから
                '--------------------------------
                ''-----------------------------
                ''2014/05/27 間瀬　ここから
                ''-----------------------------
                'Dim strExclusiveDate As String = Nothing
                'Dim arTable_Name As New ArrayList
                'Dim arKey As New ArrayList

                ''★ロック対象テーブル名の登録
                'arTable_Name.Insert(0, "D05_CNSTBREAK_DTL")
                'arTable_Name.Insert(1, "D06_CNSTAMNT_DTL")

                ''★ロックテーブルキー項目の登録(D27_CNTL_NO, D28_CNST_AMOUNT)
                'arKey.Insert(0, CType(rowData.FindControl("工事依頼番号"), TextBox).Text)

                ''★排他情報確認処理(更新画面へ遷移)
                'If clsExc.pfSel_Exclusive(strExclusiveDate _
                '                 , Me _
                '                 , Session(P_SESSION_IP) _
                '                 , Session(P_SESSION_PLACE) _
                '                 , Session(P_SESSION_USERID) _
                '                 , Session(P_SESSION_SESSTION_ID) _
                '                 , ViewState(P_SESSION_GROUP_NUM) _
                '                 , P_FUN_CNS & P_SCR_INQ & P_PAGE & "001" _
                '                 , arTable_Name _
                '                 , arKey) = 0 Then

                '    '★登録年月日時刻
                '    Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate

                'Else

                '    '排他ロック中
                '    Exit Sub

                'End If
                ''-----------------------------
                ''2014/05/27 間瀬　ここまで
                ''-----------------------------
                '--------------------------------
                '2015/04/06 加賀　ここまで
                '--------------------------------
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
            Case "btnUpdate"        ' 更新
                '-----------------------------
                '2014/04/21 土岐　ここから
                '-----------------------------
                Dim strExclusiveDate As String = Nothing
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList

                '★ロック対象テーブル名の登録
                arTable_Name.Insert(0, "D27_CNST_AMOUNT")
                arTable_Name.Insert(1, "D28_CNST_AMOUNT_DTL")

                '★ロックテーブルキー項目の登録(D27_CNTL_NO, D28_CNST_AMOUNT)
                arKey.Insert(0, CType(rowData.FindControl("工事依頼番号"), TextBox).Text)
                arKey.Insert(1, CType(rowData.FindControl("工事依頼番号"), TextBox).Text)

                '★排他情報確認処理(更新画面へ遷移)
                If clsExc.pfSel_Exclusive(strExclusiveDate _
                                 , Me _
                                 , Session(P_SESSION_IP) _
                                 , Session(P_SESSION_PLACE) _
                                 , Session(P_SESSION_USERID) _
                                 , Session(P_SESSION_SESSTION_ID) _
                                 , ViewState(P_SESSION_GROUP_NUM) _
                                 , P_FUN_CNS & P_SCR_INQ & P_PAGE & "001" _
                                 , arTable_Name _
                                 , arKey) = 0 Then

                    '★登録年月日時刻
                    Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate

                Else

                    '排他ロック中
                    Exit Sub

                End If
                '-----------------------------
                '2014/04/21 土岐　ここまで
                '-----------------------------
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
        End Select

        '--------------------------------
        '2014/04/16 星野　ここから
        '--------------------------------
        '■□■□結合試験時のみ使用予定□■□■
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
                        objStack.GetMethod.Name, CNSINQP001, strPrm, "TRANS")
        '■□■□結合試験時のみ使用予定□■□■
        '--------------------------------
        '2014/04/16 星野　ここまで
        '--------------------------------
        '別ウィンドウ起動
        psOpen_Window(Me, CNSINQP001)

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

        'ログ出力開始
        psLogStart(Me)

        If (Page.IsValid) Then

            'ファイル出力
            '--------------------------------
            '2014/07/04 星野　ここから
            '--------------------------------
            'intRtn = pfPDF("0251PN", M_DTL_FILE_NM, Nothing, New CNSREP017, mfGet_PDFData(),
            '                      strServerAddress, strFolderNM, datCreateDate, strFileNm)
            intRtn = pfPDF("0251PN", M_DTL_FILE_NM, Nothing, New CNSREP017, mfGet_PDFData(),
                                  strServerAddress, strFolderNM, datCreateDate, strFileNm, Session.SessionID, True)
            '--------------------------------
            '2014/07/04 星野　ここまで
            '--------------------------------

            'ファイル出力エラー	
            If intRtn <> 0 Then
                'エラー処理
                psMesBox(Me, "10002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_DTL_FILE_NM)
                Exit Sub
            End If

            'ダウンロードファイル（T07)にレコードを追加		
            intRtn = pfSetDwnldFile(DateTime.Now.ToString("yyyyMMdd"), "0251PN", "工事料金明細書一覧", strFileNm, M_DTL_FILE_NM,
                                    strServerAddress, strFolderNM, datCreateDate, User.Identity.Name)

            'ダウンロードファイル（T07)にレコードを追加エラー		
            If intRtn <> 0 Then
                'エラー処理	
                psMesBox(Me, "10002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_DTL_FILE_NM)
                Exit Sub
            End If


            'ファイル出力
            '--------------------------------
            '2014/07/04 星野　ここから
            '--------------------------------
            'intRtn = pfPDF("0252PN", M_SUM_FILE_NM, Nothing, New CNSREP016, mfGet_SumPDFData(),
            '                      strServerAddress, strFolderNM, datCreateDate, strFileNm)
            intRtn = pfPDF("0252PN", M_SUM_FILE_NM, Nothing, New CNSREP016, mfGet_SumPDFData(),
                                  strServerAddress, strFolderNM, datCreateDate, strFileNm, Session.SessionID, True)
            '--------------------------------
            '2014/07/04 星野　ここまで
            '--------------------------------

            'ファイル出力エラー	
            If intRtn <> 0 Then
                'エラー処理
                psMesBox(Me, "10002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_SUM_FILE_NM)
                Exit Sub
            End If

            'ダウンロードファイル（T07)にレコードを追加		
            intRtn = pfSetDwnldFile(DateTime.Now.ToString("yyyyMMdd"), "0252PN", "工事料金明細書一覧", strFileNm, M_SUM_FILE_NM,
                                    strServerAddress, strFolderNM, datCreateDate, User.Identity.Name)

            'ダウンロードファイル（T07)にレコードを追加エラー		
            If intRtn <> 0 Then
                'エラー処理	
                psMesBox(Me, "10002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_SUM_FILE_NM)
                Exit Sub
            End If

            '--------------------------------
            '2014/07/22 星野　ここから
            '--------------------------------
            '処理完了ポップアップ表示
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "PDF出力")
            '--------------------------------
            '2014/07/22 星野　ここまで
            '--------------------------------

        End If

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

        'ログ出力開始
        psLogStart(Me)

        If (Page.IsValid) Then

            psPrintPDF(Me,
                       {New CNSREP016, New CNSREP017},
                       {mfGet_SumPDFData(), mfGet_PDFData()},
                       {M_DTL_FILE_NM, M_SUM_FILE_NM})

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

        If (Page.IsValid) Then
            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    cmdDB = New SqlCommand("CNSLSTP006_S5", conDB)
                    With cmdDB.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, Me.tftTboxId.ppFromText))
                        .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, Me.tftTboxId.ppToText))
                        .Add(pfSet_Param("const_no", SqlDbType.NVarChar, Me.txtCnsReqestNo.ppText))
                        .Add(pfSet_Param("hall_nm", SqlDbType.NVarChar, Me.txtHoleNm.ppText))
                        If Me.dtbCnstFm.ppText = String.Empty AndAlso
                           Me.dtbCnstFm.ppHourText = String.Empty AndAlso
                           Me.dtbCnstFm.ppMinText = String.Empty Then
                            .Add(pfSet_Param("stest_dt_f", SqlDbType.NVarChar, String.Empty))
                        Else
                            .Add(pfSet_Param("stest_dt_f", SqlDbType.NVarChar, Me.dtbCnstFm.ppText + " " +
                                             Me.dtbCnstFm.ppHourText + ":" + Me.dtbCnstFm.ppMinText))
                        End If
                        If Me.dtbCnstTo.ppText = String.Empty AndAlso
                          Me.dtbCnstTo.ppHourText = String.Empty AndAlso
                          Me.dtbCnstTo.ppMinText = String.Empty Then
                            .Add(pfSet_Param("stest_dt_t", SqlDbType.NVarChar, String.Empty))
                        Else
                            .Add(pfSet_Param("stest_dt_t", SqlDbType.NVarChar, Me.dtbCnstTo.ppText + " " +
                                             Me.dtbCnstTo.ppHourText + ":" + Me.dtbCnstTo.ppMinText))
                        End If
                        .Add(pfSet_Param("close_dt_ym", SqlDbType.NVarChar, Me.txtClosingDt.ppText.Replace("/", "")))
                        .Add(pfSet_Param("close_dt_t", SqlDbType.NVarChar, mfGet_ClosingDtT))
                        .Add(pfSet_Param("NLCls", SqlDbType.NVarChar, Me.txtNLCls.ppText))
                        If cbxNew.Checked = True Then
                            .Add(pfSet_Param("New", SqlDbType.NVarChar, "1"))
                        Else
                            .Add(pfSet_Param("New", SqlDbType.NVarChar, "0"))
                        End If
                        If cbxExpansion.Checked = True Then
                            .Add(pfSet_Param("Expansion", SqlDbType.NVarChar, "1"))
                        Else
                            .Add(pfSet_Param("Expansion", SqlDbType.NVarChar, "0"))
                        End If
                        If cbxSomeRemoval.Checked = True Then
                            .Add(pfSet_Param("SomeRemoval", SqlDbType.NVarChar, "1"))
                        Else
                            .Add(pfSet_Param("SomeRemoval", SqlDbType.NVarChar, "0"))
                        End If
                        If cbxShopRelocation.Checked = True Then
                            .Add(pfSet_Param("ShopRelocation", SqlDbType.NVarChar, "1"))
                        Else
                            .Add(pfSet_Param("ShopRelocation", SqlDbType.NVarChar, "0"))
                        End If
                        If cbxAllRemoval.Checked = True Then
                            .Add(pfSet_Param("AllRemoval", SqlDbType.NVarChar, "1"))
                        Else
                            .Add(pfSet_Param("AllRemoval", SqlDbType.NVarChar, "0"))
                        End If
                        If cbxOnceRemoval.Checked = True Then
                            .Add(pfSet_Param("OnceRemoval", SqlDbType.NVarChar, "1"))
                        Else
                            .Add(pfSet_Param("OnceRemoval", SqlDbType.NVarChar, "0"))
                        End If
                        If cbxReInstallation.Checked = True Then
                            .Add(pfSet_Param("ReInstallation", SqlDbType.NVarChar, "1"))
                        Else
                            .Add(pfSet_Param("ReInstallation", SqlDbType.NVarChar, "0"))
                        End If
                        If cbxConChange.Checked = True Then
                            .Add(pfSet_Param("ConChange", SqlDbType.NVarChar, "1"))
                        Else
                            .Add(pfSet_Param("ConChange", SqlDbType.NVarChar, "0"))
                        End If
                        If cbxConDelivery.Checked = True Then
                            .Add(pfSet_Param("ConDelivery", SqlDbType.NVarChar, "1"))
                        Else
                            .Add(pfSet_Param("ConDelivery", SqlDbType.NVarChar, "0"))
                        End If
                        If cbxOther.Checked = True Then
                            .Add(pfSet_Param("Other", SqlDbType.NVarChar, "1"))
                        Else
                            .Add(pfSet_Param("Other", SqlDbType.NVarChar, "0"))
                        End If
                        If cbxVup.Checked = True Then
                            .Add(pfSet_Param("Vup", SqlDbType.NVarChar, "1"))
                        Else
                            .Add(pfSet_Param("Vup", SqlDbType.NVarChar, "0"))
                        End If
                    End With

                    'リストデータ取得
                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                    If dstOrders.Tables(0).Rows.Count = 0 Then
                        '0件
                        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事料金明細書一覧")
                        Return
                    End If

                    'CSVファイルダウンロード
                    If pfDLCsvFile("工事料金明細書一覧" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv", dstOrders.Tables(0), True, Me) <> 0 Then
                        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
                    End If

                Catch ex As Threading.ThreadAbortException

                Catch ex As Exception
                    psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事料金明細書一覧")
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

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()
        Me.tftTboxId.ppFromText = String.Empty
        Me.tftTboxId.ppToText = String.Empty
        Me.txtCnsReqestNo.ppText = String.Empty
        Me.txtHoleNm.ppText = String.Empty
        Me.dtbCnstFm.ppText = String.Empty
        Me.dtbCnstFm.ppHourText = String.Empty
        Me.dtbCnstFm.ppMinText = String.Empty
        Me.dtbCnstTo.ppText = String.Empty
        Me.dtbCnstTo.ppHourText = String.Empty
        Me.dtbCnstTo.ppMinText = String.Empty
        Me.txtClosingDt.ppText = String.Empty
        Me.cblClosingDt.ClearSelection()

        Me.txtNLCls.ppText = String.Empty
        Me.cbxNew.Checked = False
        Me.cbxExpansion.Checked = False
        Me.cbxSomeRemoval.Checked = False
        Me.cbxShopRelocation.Checked = False
        Me.cbxAllRemoval.Checked = False
        Me.cbxOnceRemoval.Checked = False
        Me.cbxReInstallation.Checked = False
        Me.cbxConChange.Checked = False
        Me.cbxConDelivery.Checked = False
        Me.cbxOther.Checked = False
        Me.cbxVup.Checked = False

        Me.grvList.DataSource = New Object() {}
        Me.grvList.DataBind()
        Master.ppCount = "0"

        '帳票表示用検索条件更新
        ViewState(M_VIEW_DATE_FROM) = Nothing
        ViewState(M_VIEW_DATE_TO) = Nothing
    End Sub

    'Private Sub msGet_Data(ByVal ipstrTboxid_f As String,
    '                       ByVal ipstrTboxid_t As String,
    '                       ByVal ipstrConst_no As String,
    '                       ByVal ipstrHall_nm As String,
    '                       ByVal ipstrStest_dt_f As String,
    '                       ByVal ipstrStest_dt_t As String,
    '                       ByVal ipstrClose_dt_ym As String,
    '                       ByVal ipstrClose_dt_t As String,
    '                       )
    ''' <summary>
    ''' 検索処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data(ByVal ipstrTboxid_f As String, ByVal ipstrTboxid_t As String, _
                           ByVal ipstrConst_no As String, _
                           ByVal ipstrHall_nm As String, _
                           ByVal ipstrStest_dt_f As String, ByVal ipstrStest_dt_t As String, _
                           ByVal ipstrClose_dt_ym As String, ByVal ipstrClose_dt_t As String, _
                           ByVal ipstrNLCls As String, _
                           ByVal ipblnNew As Boolean, ByVal ipblnExpansion As Boolean, ByVal ipblnSomeRemoval As Boolean, _
                           ByVal ipblnShopRelocation As Boolean, ByVal ipblnAllRemoval As Boolean, ByVal ipblnOnceRemoval As Boolean, _
                           ByVal ipblnReInstallation As Boolean, ByVal ipblnConChange As Boolean, ByVal ipblnConDelivery As Boolean, _
                           ByVal ipblnOther As Boolean, ByVal ipblnVup As Boolean
                           )
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("CNSLSTP006_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, ipstrTboxid_f))
                    .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, ipstrTboxid_t))
                    .Add(pfSet_Param("const_no", SqlDbType.NVarChar, ipstrConst_no))
                    .Add(pfSet_Param("hall_nm", SqlDbType.NVarChar, ipstrHall_nm))
                    .Add(pfSet_Param("stest_dt_f", SqlDbType.NVarChar, ipstrStest_dt_f))
                    .Add(pfSet_Param("stest_dt_t", SqlDbType.NVarChar, ipstrStest_dt_t))
                    .Add(pfSet_Param("close_dt_ym", SqlDbType.NVarChar, ipstrClose_dt_ym))
                    .Add(pfSet_Param("close_dt_t", SqlDbType.NVarChar, ipstrClose_dt_t))
                    .Add(pfSet_Param("NLCls", SqlDbType.NVarChar, ipstrNLCls))
                    If ipblnNew = True Then
                        .Add(pfSet_Param("New", SqlDbType.NVarChar, "1"))
                    Else
                        .Add(pfSet_Param("New", SqlDbType.NVarChar, "0"))
                    End If
                    If ipblnExpansion = True Then
                        .Add(pfSet_Param("Expansion", SqlDbType.NVarChar, "1"))
                    Else
                        .Add(pfSet_Param("Expansion", SqlDbType.NVarChar, "0"))
                    End If
                    If ipblnSomeRemoval = True Then
                        .Add(pfSet_Param("SomeRemoval", SqlDbType.NVarChar, "1"))
                    Else
                        .Add(pfSet_Param("SomeRemoval", SqlDbType.NVarChar, "0"))
                    End If
                    If ipblnShopRelocation = True Then
                        .Add(pfSet_Param("ShopRelocation", SqlDbType.NVarChar, "1"))
                    Else
                        .Add(pfSet_Param("ShopRelocation", SqlDbType.NVarChar, "0"))
                    End If
                    If ipblnAllRemoval = True Then
                        .Add(pfSet_Param("AllRemoval", SqlDbType.NVarChar, "1"))
                    Else
                        .Add(pfSet_Param("AllRemoval", SqlDbType.NVarChar, "0"))
                    End If
                    If ipblnOnceRemoval = True Then
                        .Add(pfSet_Param("OnceRemoval", SqlDbType.NVarChar, "1"))
                    Else
                        .Add(pfSet_Param("OnceRemoval", SqlDbType.NVarChar, "0"))
                    End If
                    If ipblnReInstallation = True Then
                        .Add(pfSet_Param("ReInstallation", SqlDbType.NVarChar, "1"))
                    Else
                        .Add(pfSet_Param("ReInstallation", SqlDbType.NVarChar, "0"))
                    End If
                    If ipblnConChange = True Then
                        .Add(pfSet_Param("ConChange", SqlDbType.NVarChar, "1"))
                    Else
                        .Add(pfSet_Param("ConChange", SqlDbType.NVarChar, "0"))
                    End If
                    If ipblnConDelivery = True Then
                        .Add(pfSet_Param("ConDelivery", SqlDbType.NVarChar, "1"))
                    Else
                        .Add(pfSet_Param("ConDelivery", SqlDbType.NVarChar, "0"))
                    End If
                    If ipblnOther = True Then
                        .Add(pfSet_Param("Other", SqlDbType.NVarChar, "1"))
                    Else
                        .Add(pfSet_Param("Other", SqlDbType.NVarChar, "0"))
                    End If
                    If ipblnVup = True Then
                        .Add(pfSet_Param("Vup", SqlDbType.NVarChar, "1"))
                    Else
                        .Add(pfSet_Param("Vup", SqlDbType.NVarChar, "0"))
                    End If
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                End If

                '件数を設定
                Master.ppCount = dstOrders.Tables(0).Rows.Count.ToString

                '取得したデータをリストに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

                '帳票表示用検索条件更新(yy/MM/dd)
                If ipstrStest_dt_f.Length >= 2 Then
                    ViewState(M_VIEW_DATE_FROM) = ipstrStest_dt_f.Substring(2)
                Else
                    ViewState(M_VIEW_DATE_FROM) = ipstrStest_dt_f
                End If
                If ipstrStest_dt_t.Length >= 2 Then
                    ViewState(M_VIEW_DATE_TO) = ipstrStest_dt_t.Substring(2)
                Else
                    ViewState(M_VIEW_DATE_TO) = ipstrStest_dt_t
                End If

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事料金明細書一覧")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
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
    ''' 締日（期間）をカンマ区切りで返す。
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfGet_ClosingDtT() As String
        Dim strVal As String = String.Empty
        For zz As Integer = 0 To (Me.cblClosingDt.Items.Count - 1)
            If Me.cblClosingDt.Items(zz).Selected Then
                If strVal = String.Empty Then
                    strVal = Me.cblClosingDt.Items(zz).Value
                Else
                    strVal = strVal + "," + Me.cblClosingDt.Items(zz).Value
                End If
            End If
        Next
        Return strVal
    End Function

    ''' <summary>
    ''' 締日（期間）リスト設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetcblClosingDt()
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("CNSLSTP006_S2", conDB)

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得したデータをリストに設定
                Me.cblClosingDt.DataSource = dstOrders.Tables(0)
                Me.cblClosingDt.DataValueField = "コード"
                Me.cblClosingDt.DataTextField = "名称"

                '変更を反映
                Me.cblClosingDt.DataBind()

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "締日（期間）リスト")
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
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If
    End Sub

    ''' <summary>
    ''' GridViewから機器代金及び工事料金明細一覧（集計）データ生成
    ''' </summary>
    ''' <returns>生成したデータ</returns>
    ''' <remarks></remarks>
    Private Function mfGet_SumPDFData() As DataTable


        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dtlSum As DataTable = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("CNSLSTP006_S4", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, Me.tftTboxId.ppFromText))
                    .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, Me.tftTboxId.ppToText))
                    .Add(pfSet_Param("const_no", SqlDbType.NVarChar, Me.txtCnsReqestNo.ppText))
                    .Add(pfSet_Param("hall_nm", SqlDbType.NVarChar, Me.txtHoleNm.ppText))
                    If Me.dtbCnstFm.ppText = String.Empty AndAlso
                       Me.dtbCnstFm.ppHourText = String.Empty AndAlso
                       Me.dtbCnstFm.ppMinText = String.Empty Then
                        .Add(pfSet_Param("stest_dt_f", SqlDbType.NVarChar, String.Empty))
                    Else
                        .Add(pfSet_Param("stest_dt_f", SqlDbType.NVarChar, Me.dtbCnstFm.ppText + " " +
                                         Me.dtbCnstFm.ppHourText + ":" + Me.dtbCnstFm.ppMinText))
                    End If
                    If Me.dtbCnstTo.ppText = String.Empty AndAlso
                      Me.dtbCnstTo.ppHourText = String.Empty AndAlso
                      Me.dtbCnstTo.ppMinText = String.Empty Then
                        .Add(pfSet_Param("stest_dt_t", SqlDbType.NVarChar, String.Empty))
                    Else
                        .Add(pfSet_Param("stest_dt_t", SqlDbType.NVarChar, Me.dtbCnstTo.ppText + " " +
                                         Me.dtbCnstTo.ppHourText + ":" + Me.dtbCnstTo.ppMinText))
                    End If
                    .Add(pfSet_Param("close_dt_ym", SqlDbType.NVarChar, Me.txtClosingDt.ppText.Replace("/", "")))
                    .Add(pfSet_Param("close_dt_t", SqlDbType.NVarChar, mfGet_ClosingDtT))
                End With

                'リストデータ取得
                dtlSum = clsDataConnect.pfGet_DataSet(cmdDB).Tables(0)


            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事料金明細書一覧")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
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



        'Dim dtlGrid As DataTable
        'Dim dtvData As DataView
        'Dim dtlData As DataTable
        'Dim dtlSum As DataTable
        'Dim rowSumRow As DataRow
        'Dim strWhere As StringBuilder
        'Dim dttDate As DateTime
        'Dim strSort As String

        'dttDate = DateTime.Now
        'dtlGrid = pfParse_DataTable(Me.grvList)

        ''重複権削除
        'dtvData = New DataView(dtlGrid)
        'dtlData = dtvData.ToTable("DistinctTable",
        '                          True,
        '                          {"東西フラグ",
        '                           "ＩＤ／ＩＣ",
        '                           "工事種別",
        '                           "締月",
        '                           "現行システムコード",
        '                           "現行システム",
        '                           "改ページ"})
        ''集計列生成
        'dtlSum = New DataTable
        'dtlSum.Columns.Add("開始日")
        'dtlSum.Columns.Add("終了日")
        'dtlSum.Columns.Add("東西フラグ")
        'dtlSum.Columns.Add("IDIC")
        'dtlSum.Columns.Add("工事種別")
        'dtlSum.Columns.Add("締月")
        'dtlSum.Columns.Add("システム")
        'dtlSum.Columns.Add("件数")
        'dtlSum.Columns.Add("TBOX機器費")
        'dtlSum.Columns.Add("LAN機器費")
        'dtlSum.Columns.Add("基本料金")
        'dtlSum.Columns.Add("工事費")
        'dtlSum.Columns.Add("材料費")
        'dtlSum.Columns.Add("その他費用")
        'dtlSum.Columns.Add("その他調整費")
        'dtlSum.Columns.Add("カード受付関連費")
        'dtlSum.Columns.Add("合計")
        'dtlSum.Columns.Add("小計")
        'dtlSum.Columns.Add("発行日付")
        'dtlSum.Columns.Add("会社名")
        'dtlSum.Columns.Add("改ページ")

        ''ソート条件
        'strSort = "東西フラグ, 締月, ＩＤ／ＩＣ, 工事種別, 現行システム"

        ''集計データ生成
        'For Each row As DataRow In dtlData.Select(Nothing, strSort)
        '    '条件式生成
        '    strWhere = New StringBuilder
        '    strWhere.Append(String.Format("東西フラグ ='{0}'", row.Item("東西フラグ")))
        '    strWhere.Append(" AND ")
        '    strWhere.Append(String.Format("ＩＤ／ＩＣ ='{0}'", row.Item("ＩＤ／ＩＣ")))
        '    strWhere.Append(" AND ")
        '    strWhere.Append(String.Format("工事種別 ='{0}'", row.Item("工事種別")))
        '    strWhere.Append(" AND ")
        '    strWhere.Append(String.Format("締月 ='{0}'", row.Item("締月")))
        '    strWhere.Append(" AND ")
        '    strWhere.Append(String.Format("現行システムコード ='{0}'", row.Item("現行システムコード")))

        '    rowSumRow = dtlSum.NewRow
        '    rowSumRow.Item("開始日") = ViewState(M_VIEW_DATE_FROM)
        '    rowSumRow.Item("終了日") = ViewState(M_VIEW_DATE_TO)
        '    rowSumRow.Item("東西フラグ") = row.Item("東西フラグ")
        '    rowSumRow.Item("IDIC") = row.Item("ＩＤ／ＩＣ")
        '    rowSumRow.Item("工事種別") = row.Item("工事種別")
        '    rowSumRow.Item("締月") = row.Item("締月")
        '    rowSumRow.Item("システム") = row.Item("現行システム")
        '    rowSumRow.Item("件数") = mfConvertDBNull(dtlGrid.Compute("COUNT(現行システムコード)", strWhere.ToString))
        '    rowSumRow.Item("ＴＢＯＸ機器費") = mfConvertDBNull(dtlGrid.Compute("SUM(ＴＢＯＸ機器費)", strWhere.ToString))
        '    rowSumRow.Item("ＬＡＮ機器費") = mfConvertDBNull(dtlGrid.Compute("SUM(ＬＡＮ機器費)", strWhere.ToString))
        '    rowSumRow.Item("基本料金") = mfConvertDBNull(dtlGrid.Compute("SUM(基本料金)", strWhere.ToString))
        '    rowSumRow.Item("工事費") = mfConvertDBNull(dtlGrid.Compute("SUM(工事費)", strWhere.ToString))
        '    rowSumRow.Item("材料費") = mfConvertDBNull(dtlGrid.Compute("SUM(材料費)", strWhere.ToString))
        '    rowSumRow.Item("その他費用") = mfConvertDBNull(dtlGrid.Compute("SUM(その他費用)", strWhere.ToString))
        '    rowSumRow.Item("その他調整費") = mfConvertDBNull(dtlGrid.Compute("SUM(その他調整費)", strWhere.ToString))
        '    rowSumRow.Item("カード受付関連費") = mfConvertDBNull(dtlGrid.Compute("SUM(カード受付関連費)", strWhere.ToString))
        '    rowSumRow.Item("合計") = mfConvertDBNull(dtlGrid.Compute("SUM(合計)", strWhere.ToString))
        '    rowSumRow.Item("小計") = mfConvertDBNull(dtlGrid.Compute("SUM(小計)", strWhere.ToString))
        '    rowSumRow.Item("発行日付") = dttDate
        '    rowSumRow.Item("会社名") = dtlGrid.Rows(0).Item("会社名")
        '    rowSumRow.Item("改ページ") = row.Item("改ページ")

        '    dtlSum.Rows.Add(rowSumRow)
        'Next

        Return dtlSum
    End Function

    ''' <summary>
    ''' GridViewから機器代金及び工事料金明細一覧（明細）データ生成
    ''' </summary>
    ''' <returns>生成したデータ</returns>
    ''' <remarks></remarks>
    Private Function mfGet_PDFData() As DataTable


        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dtlDtl As DataTable = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("CNSLSTP006_S3", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, Me.tftTboxId.ppFromText))
                    .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, Me.tftTboxId.ppToText))
                    .Add(pfSet_Param("const_no", SqlDbType.NVarChar, Me.txtCnsReqestNo.ppText))
                    .Add(pfSet_Param("hall_nm", SqlDbType.NVarChar, Me.txtHoleNm.ppText))
                    If Me.dtbCnstFm.ppText = String.Empty AndAlso
                       Me.dtbCnstFm.ppHourText = String.Empty AndAlso
                       Me.dtbCnstFm.ppMinText = String.Empty Then
                        .Add(pfSet_Param("stest_dt_f", SqlDbType.NVarChar, String.Empty))
                    Else
                        .Add(pfSet_Param("stest_dt_f", SqlDbType.NVarChar, Me.dtbCnstFm.ppText + " " +
                                         Me.dtbCnstFm.ppHourText + ":" + Me.dtbCnstFm.ppMinText))
                    End If
                    If Me.dtbCnstTo.ppText = String.Empty AndAlso
                      Me.dtbCnstTo.ppHourText = String.Empty AndAlso
                      Me.dtbCnstTo.ppMinText = String.Empty Then
                        .Add(pfSet_Param("stest_dt_t", SqlDbType.NVarChar, String.Empty))
                    Else
                        .Add(pfSet_Param("stest_dt_t", SqlDbType.NVarChar, Me.dtbCnstTo.ppText + " " +
                                         Me.dtbCnstTo.ppHourText + ":" + Me.dtbCnstTo.ppMinText))
                    End If
                    .Add(pfSet_Param("close_dt_ym", SqlDbType.NVarChar, Me.txtClosingDt.ppText.Replace("/", "")))
                    .Add(pfSet_Param("close_dt_t", SqlDbType.NVarChar, mfGet_ClosingDtT))
                End With

                'リストデータ取得
                dtlDtl = clsDataConnect.pfGet_DataSet(cmdDB).Tables(0)


            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事料金明細書一覧")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
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


        'Dim dtlGrid As DataTable
        'Dim dtlDtl As DataTable
        'Dim rowDtlRow As DataRow
        'Dim dttDate As DateTime
        'Dim strSort As String

        'dttDate = DateTime.Now
        'dtlGrid = pfParse_DataTable(Me.grvList)

        ''明細列生成
        'dtlDtl = New DataTable
        'dtlDtl.Columns.Add("開始日")
        'dtlDtl.Columns.Add("終了日")
        'dtlDtl.Columns.Add("東西フラグ")
        'dtlDtl.Columns.Add("IDIC")
        'dtlDtl.Columns.Add("工事種別")
        'dtlDtl.Columns.Add("締月")
        'dtlDtl.Columns.Add("ホール名")
        'dtlDtl.Columns.Add("ホールコード")
        'dtlDtl.Columns.Add("工事依頼番号")
        'dtlDtl.Columns.Add("システム")
        'dtlDtl.Columns.Add("工事日")
        'dtlDtl.Columns.Add("オープン日")
        'dtlDtl.Columns.Add("TBOX機器費")
        'dtlDtl.Columns.Add("LAN機器費")
        'dtlDtl.Columns.Add("基本料金")
        'dtlDtl.Columns.Add("工事費")
        'dtlDtl.Columns.Add("材料費")
        'dtlDtl.Columns.Add("その他費用")
        'dtlDtl.Columns.Add("その他調整費")
        'dtlDtl.Columns.Add("カード受付関連費")
        'dtlDtl.Columns.Add("合計")
        'dtlDtl.Columns.Add("小計")
        'dtlDtl.Columns.Add("発行日付")
        'dtlDtl.Columns.Add("会社名")
        'dtlDtl.Columns.Add("連番")
        'dtlDtl.Columns.Add("改ページ")

        ''ソート条件
        'strSort = "東西フラグ, 締月, ＩＤ／ＩＣ, 工事種別, 改ページ, 工事依頼番号"

        ''明細データ生成
        'For Each row As DataRow In dtlGrid.Select(Nothing, strSort)
        '    rowDtlRow = dtlDtl.NewRow
        '    rowDtlRow.Item("開始日") = ViewState(M_VIEW_DATE_FROM)
        '    rowDtlRow.Item("終了日") = ViewState(M_VIEW_DATE_TO)
        '    rowDtlRow.Item("東西フラグ") = row.Item("東西フラグ")
        '    rowDtlRow.Item("IDIC") = row.Item("ＩＤ／ＩＣ")
        '    rowDtlRow.Item("工事種別") = row.Item("工事種別")
        '    rowDtlRow.Item("締月") = row.Item("締月")
        '    rowDtlRow.Item("ホール名") = row.Item("ホール名")
        '    rowDtlRow.Item("ホールコード") = row.Item("ホールコード")
        '    rowDtlRow.Item("工事依頼番号") = row.Item("工事依頼番号")
        '    rowDtlRow.Item("システム") = row.Item("現行システム")
        '    rowDtlRow.Item("工事日") = row.Item("工事日")
        '    rowDtlRow.Item("オープン日") = row.Item("オープン日")
        '    rowDtlRow.Item("TBOX機器費") = mfConvertDBNull(row.Item("TBOX機器費"))
        '    rowDtlRow.Item("LAN機器費") = mfConvertDBNull(row.Item("LAN機器費"))
        '    rowDtlRow.Item("基本料金") = mfConvertDBNull(row.Item("基本料金"))
        '    rowDtlRow.Item("工事費") = mfConvertDBNull(row.Item("工事費"))
        '    rowDtlRow.Item("材料費") = mfConvertDBNull(row.Item("材料費"))
        '    rowDtlRow.Item("その他費用") = mfConvertDBNull(row.Item("その他費用"))
        '    rowDtlRow.Item("その他調整費") = mfConvertDBNull(row.Item("その他調整費"))
        '    rowDtlRow.Item("カード受付関連費") = mfConvertDBNull(row.Item("カード受付関連費"))
        '    rowDtlRow.Item("合計") = mfConvertDBNull(row.Item("合計"))
        '    rowDtlRow.Item("小計") = mfConvertDBNull(row.Item("小計"))
        '    rowDtlRow.Item("発行日付") = dttDate
        '    rowDtlRow.Item("会社名") = row.Item("会社名")
        '    rowDtlRow.Item("連番") = row.Item("連番")
        '    rowDtlRow.Item("改ページ") = row.Item("改ページ")
        '    dtlDtl.Rows.Add(rowDtlRow)
        'Next

        Return dtlDtl
    End Function

    ''' <summary>
    ''' DBNullをobjRtnに置換して返す
    ''' </summary>
    ''' <param name="objData">チェック値</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfConvertDBNull(ByVal objData As Object) As Decimal
        If DBNull.Value.Equals(objData) Then
            Return 0
        Else
            Return objData
        End If
    End Function

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
#End Region
