'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜修理・整備業務＞
'*　処理名　　：　整備進捗明細
'*　ＰＧＭＩＤ：　MNTSELP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.24　：　後藤
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax
'-----------------------------
'2014/06/11 後藤　ここから
'-----------------------------
'排他制御用
Imports SPC.ClsCMExclusive
'-----------------------------
'2014/06/11 後藤　ここまで
'-----------------------------
#End Region

Public Class MNTSELP001
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
    ''' <summary>画面ID</summary>
    Private Const M_DISP_ID = P_FUN_MNT & P_SCR_SEL & P_PAGE & "001"

    ''' <summary>引継情報(枝番)</summary>
    Private Const P_KEY2 As String = "ＫＥＹ２"
    ''' <summary>共有情報(連番)</summary>
    Private Const P_KEY3 As String = "ＫＥＹ３"

    ''' <summary>依頼先会社コード</summary>
    Private Const P_REQCOMP_CD As String = "P_REQCOMP_CD"

    ''' <summary>請求年月</summary>
    Private Const sCnsReqDt As String = "REQDT"

    ''' <summary>ストアドプロシージャ1</summary>
    Const sCnsSqlid_015 As String = "ZCMPSEL015"
    ''' <summary>ストアドプロシージャ2</summary>
    Const sCnsSqlid_027 As String = "ZCMPSEL027"
    ''' <summary>進捗状況ステータスマスタ一覧取得</summary>
    Const sCnsErrorMes_015 As String = "進捗状況ステータスマスタ一覧取得"
    ''' <summary>部品マスタ一覧取得</summary>
    Const sCnsErrorMes_027 As String = "部品マスタ一覧取得"

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

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"
    ''' <summary>
    ''' Page_Init.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '表設定
        pfSet_GridView(Me.grvList, M_DISP_ID)
    End Sub
    ''' <summary>
    ''' ロード処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ''ボタンアクションの設定.
        AddHandler Me.btnClear.Click, AddressOf btnClear_Click
        AddHandler Me.btnInsert.Click, AddressOf btnInsert_Click
        AddHandler Me.btnUpdate.Click, AddressOf btnUpdate_Click
        AddHandler Me.btnDelete.Click, AddressOf btnDelete_Click

        '--------------------------------
        '2014/05/19 後藤　ここから
        '--------------------------------
        'AddHandler Me.btnReset.Click, AddressOf btnReset_Click
        'AddHandler Me.btnAllDelete.Click, AddressOf btnAllDelete_Click
        'AddHandler Me.btnAllUpdate.Click, AddressOf btnAllUpdate_Click
        AddHandler Master.ppRigthButton3.Click, AddressOf btnReset_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnAllDelete_Click
        AddHandler Master.ppRigthButton1.Click, AddressOf btnAllUpdate_Click

        Master.ppRigthButton1.ValidationGroup = "1"

        '--------------------------------
        '2014/05/19 後藤　ここまで
        '--------------------------------

        AddHandler Me.ddlWrkCls.ppDropDownList.TextChanged, AddressOf ddlWrkCls_TextChanged
        Me.ddlWrkCls.ppDropDownList.AutoPostBack = True

        '確認メッセージ設定.
        '--------------------------------
        '2014/05/19 後藤　ここから
        '--------------------------------
        'Me.btnAllDelete.OnClientClick = pfGet_OCClickMes("00005", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "整備進捗明細")
        'Me.btnAllUpdate.OnClientClick = pfGet_OCClickMes("00004", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "整備進捗明細")
        Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("00005", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "整備進捗明細")
        Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("00004", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "整備進捗明細")
        '--------------------------------
        '2014/05/19 後藤　ここまで
        '--------------------------------
        Me.btnInsert.OnClientClick = pfGet_OCClickMes("00006", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "交換部品")
        Me.btnUpdate.OnClientClick = pfGet_OCClickMes("00004", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "交換部品")
        Me.btnDelete.OnClientClick = pfGet_OCClickMes("00005", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "交換部品")

        Dim strKey As String = String.Empty '管理番号
        Dim strKey2 As String = String.Empty '枝番
        Dim strId As String = String.Empty
        Dim strTerms As String = String.Empty


        If Not IsPostBack Then '初回表示.

            If Session(P_SESSION_TERMS) Is Nothing Or Session(P_KEY) Is Nothing Then
                psMesBox(Me, "20004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画前)
                '--------------------------------
                '2014/06/11 後藤　ここから
                '--------------------------------
                '排他削除
                clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                '--------------------------------
                '2014/06/11 後藤　ここまで
                '--------------------------------
                psClose_Window(Me)
                Return
            End If

            'セッション項目取得
            ViewState(P_KEY) = DirectCast(Session(P_KEY), String())(0)
            ViewState(P_KEY2) = DirectCast(Session(P_KEY), String())(1)
            ViewState(P_SESSION_OLDDISP) = Session(P_SESSION_OLDDISP)
            ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)

            'ビューステート項目取得
            strKey = ViewState(P_KEY)
            strKey2 = ViewState(P_KEY2)
            strId = ViewState(P_SESSION_OLDDISP)
            strTerms = ViewState(P_SESSION_TERMS)

            'プログラムＩＤ、画面名設定.
            Master.ppProgramID = M_DISP_ID
            Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

            'パンくずリスト設定.
            Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

            '--------------------------------
            '2014/05/19 後藤　ここから
            '--------------------------------
            Master.ppRigthButton1.Text = "更新"
            Master.ppRigthButton1.Visible = True
            Master.ppRigthButton2.Text = "削除"
            Master.ppRigthButton2.Visible = True
            Master.ppRigthButton3.Text = "元に戻す"
            Master.ppRigthButton3.Visible = True
            '--------------------------------
            '2014/05/19 後藤　ここまで
            '--------------------------------

            '★排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
            If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then

                Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)

            End If

            Select Case strTerms.ToString.Trim
                Case ClsComVer.E_遷移条件.参照

                    'コントロール初期化.
                    Call msClearScreen()

                    '整備進捗明細データ取得処理.
                    If mfGet_Data(strKey, strKey2) = False Then
                        psClose_Window(Me)
                        Return
                    End If

                    '交換部品データ取得処理.
                    If mfSetList_Data(strKey, strKey2) = False Then
                        psClose_Window(Me)
                        Return
                    End If

                    '活性制御
                    Call msEnableScreen(strTerms)

                Case ClsComVer.E_遷移条件.更新

                    'コントロール初期化.
                    Call msClearScreen()

                    '整備進捗明細データ取得処理.
                    If mfGet_Data(strKey, strKey2) = False Then
                        '--------------------------------
                        '2014/06/11 後藤　ここから
                        '--------------------------------
                        '排他削除
                        clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                        '--------------------------------
                        '2014/06/11 後藤　ここまで
                        '--------------------------------
                        psClose_Window(Me)
                        Return
                    End If

                    '交換部品データ取得処理.
                    If mfSetList_Data(strKey, strKey2) = False Then
                        '--------------------------------
                        '2014/06/11 後藤　ここから
                        '--------------------------------
                        '排他削除
                        clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                        '--------------------------------
                        '2014/06/11 後藤　ここまで
                        '--------------------------------
                        psClose_Window(Me)
                        Return
                    End If

                    '活性制御
                    Call msEnableScreen(strTerms)

            End Select



        End If

    End Sub

    '---------------------------
    '2014/04/24 武 ここから
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
                ddlTmptst_Cd.ppEnabled = False
                dtbRsltSend_Dt.ppEnabled = False
                ddlPrgStatus_Cd.Enabled = False
                dtbSend_Dt.ppEnabled = False
                ddlWork.ppEnabled = False
                txtSerial_No.ppEnabled = False
                dtbReceipt_Dt.ppEnabled = False
                ddlClass.ppEnabled = False
                txtReq_Dt.ppEnabled = False
                ddlWrkCls.ppEnabled = False
                ddlPartsNm.Enabled = False
                txtQuantity.ppEnabled = False
                '--------------------------------
                '2014/06/24 星野　ここから
                '--------------------------------
                'Me.btnClear.Enabled = True
                '--------------------------------
                '2014/06/24 星野　ここまで
                '--------------------------------
                btnInsert.Enabled = False
                btnUpdate.Enabled = False
                btnDelete.Enabled = False

                '--------------------------------
                '2014/05/19 後藤　ここから
                '--------------------------------
                'btnReset.Enabled = False
                'btnAllDelete.Enabled = False
                'btnAllUpdate.Enabled = False
                Master.ppRigthButton3.Enabled = False
                Master.ppRigthButton2.Enabled = False
                Master.ppRigthButton1.Enabled = False
                '--------------------------------
                '2014/05/19 後藤　ここまで
                '--------------------------------
            Case "NGC"
                ddlTmptst_Cd.ppEnabled = False
                dtbRsltSend_Dt.ppEnabled = False
                ddlPrgStatus_Cd.Enabled = False
                dtbSend_Dt.ppEnabled = False
                ddlWork.ppEnabled = False
                txtSerial_No.ppEnabled = False
                dtbReceipt_Dt.ppEnabled = False
                ddlClass.ppEnabled = False
                txtReq_Dt.ppEnabled = False
                ddlWrkCls.ppEnabled = False
                ddlPartsNm.Enabled = False
                txtQuantity.ppEnabled = False
                '--------------------------------
                '2014/06/24 星野　ここから
                '--------------------------------
                'Me.btnClear.Enabled = True
                '--------------------------------
                '2014/06/24 星野　ここまで
                '--------------------------------
                btnInsert.Enabled = False
                btnUpdate.Enabled = False
                btnDelete.Enabled = False
                '--------------------------------
                '2014/05/19 後藤　ここから
                '--------------------------------
                'btnReset.Enabled = False
                'btnAllDelete.Enabled = False
                'btnAllUpdate.Enabled = False
                Master.ppRigthButton3.Enabled = False
                Master.ppRigthButton2.Enabled = False
                Master.ppRigthButton1.Enabled = False
                '--------------------------------
                '2014/05/19 後藤　ここまで
                '--------------------------------
        End Select

    End Sub
    '---------------------------
    '2014/04/24 武 ここまで
    '---------------------------
    '---------------------------
    '2014/06/23 武 ここから
    '---------------------------
    ''' <summary>
    ''' ユーザ権限
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grvList.RowDataBound
        '選択ボタンを押下できるようにする。
        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
                e.Row.Cells(0).Enabled = True
            Case "SPC"
                e.Row.Cells(0).Enabled = True
            Case "営業所"
                e.Row.Cells(0).Enabled = True
            Case "NGC"
                e.Row.Cells(0).Enabled = True
                e.Row.Cells(1).Enabled = False
                e.Row.Cells(2).Enabled = False
        End Select

    End Sub
    '---------------------------
    '2014/06/23 武 ここまで
    '---------------------------
    ''' <summary>
    ''' 作業分類変更時処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlWrkCls_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        '空白の場合は処理しない.
        If Me.ddlWrkCls.ppDropDownList.SelectedIndex = "0" Then
            Me.ddlPartsNm.SelectedIndex = 0
            Me.ddlPartsNm.Enabled = False
            Exit Sub
        End If

        Call msWrkCls_TextChanged(sender, "")

    End Sub
    ''' <summary>
    ''' 更新ボタン押下処理．
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAllUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        '入力チェック.
        Call msCheck_Error()

        '検証チェック.
        If (Page.IsValid) Then

            Dim conDB As SqlConnection = Nothing
            Dim cmdDB As SqlCommand = Nothing
            Dim conTrn As SqlTransaction = Nothing
            Dim intRtn As Integer
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            objStack = New StackFrame
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------

            Try

                '開始ログ出力.
                psLogStart(Me)

                '接続
                If Not clsDataConnect.pfOpen_Database(conDB) Then
                    psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                Else

                    'トランザクション.
                    conTrn = conDB.BeginTransaction

                    cmdDB = New SqlCommand(M_DISP_ID & "_U1", conDB)
                    cmdDB.Connection = conDB
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = conTrn

                    With cmdDB.Parameters
                        '--------------------------------
                        '2014/05/20 後藤　ここから
                        '--------------------------------
                        .Add(pfSet_Param("mente_no", SqlDbType.NVarChar, Me.lblMente_No.Text))
                        .Add(pfSet_Param("branch", SqlDbType.NVarChar, ViewState(P_KEY2)))
                        .Add(pfSet_Param("tmptst_cd", SqlDbType.NVarChar, Me.ddlTmptst_Cd.ppSelectedValue))
                        '.Add(pfSet_Param("rsltsend_d", SqlDbType.NVarChar, Me.dtbRsltSend_Dt.ppText))
                        .Add(pfSet_Param("rsltsend_d", SqlDbType.NVarChar, mfGetDBNull(Me.dtbRsltSend_Dt.ppText)))
                        .Add(pfSet_Param("prgstatus_cd", SqlDbType.NVarChar, Me.ddlPrgStatus_Cd.SelectedValue))
                        .Add(pfSet_Param("send_d", SqlDbType.NVarChar, Me.dtbSend_Dt.ppText))
                        '.Add(pfSet_Param("send_d", SqlDbType.NVarChar, mfGetDBNull(Me.dtbSend_Dt.ppText)))
                        .Add(pfSet_Param("wrk_cd", SqlDbType.NVarChar, Me.ddlWork.ppSelectedValue))
                        .Add(pfSet_Param("serial", SqlDbType.NVarChar, Me.txtSerial_No.ppText))
                        .Add(pfSet_Param("receipt_d", SqlDbType.NVarChar, Me.dtbReceipt_Dt.ppText))
                        '.Add(pfSet_Param("receipt_d", SqlDbType.NVarChar, mfGetDBNull(Me.dtbReceipt_Dt.ppText)))
                        .Add(pfSet_Param("result_cls", SqlDbType.NVarChar, Me.ddlClass.ppSelectedValue))
                        .Add(pfSet_Param("req_dt", SqlDbType.NVarChar, ViewState(sCnsReqDt)))
                        '.Add(pfSet_Param("req_dt", SqlDbType.NVarChar, mfGetDBNull(ViewState(sCnsReqDt))))
                        .Add(pfSet_Param("update_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                    End With
                    '--------------------------------
                    '2014/05/20 後藤　ここまで
                    '--------------------------------
                    '実行
                    cmdDB.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, "00001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "整備進捗明細")

                        'ロールバック
                        conTrn.Rollback()

                        Exit Sub
                    End If

                    'コミット
                    conTrn.Commit()

                    msEnableScreen(ClsComVer.E_遷移条件.更新)

                    '完了メッセージ
                    psMesBox(Me, "00001", clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後, "整備進捗明細")

                End If

            Catch ex As Exception

                If Not conTrn Is Nothing Then
                    conTrn.Rollback()
                End If

                psMesBox(Me, "00001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "整備進捗明細")
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
                    psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                End If

                '終了ログ出力
                psLogEnd(Me)

            End Try

        End If


    End Sub
    ''' <summary>
    ''' 削除ボタン押下処理．
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAllDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim conTrn As SqlTransaction = Nothing
        Dim intRtn As Integer
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            '開始ログ出力.
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            Else

                'トランザクション.
                conTrn = conDB.BeginTransaction

                cmdDB = New SqlCommand(M_DISP_ID & "_D1", conDB)
                cmdDB.Connection = conDB
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.Transaction = conTrn
                With cmdDB.Parameters
                    .Add(pfSet_Param("mente_no", SqlDbType.NVarChar, Me.lblMente_No.Text))
                    .Add(pfSet_Param("branch", SqlDbType.NVarChar, ViewState(P_KEY2)))
                    .Add(pfSet_Param("delete_flg", SqlDbType.NVarChar, "1"))
                    .Add(pfSet_Param("update_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                End With

                '実行
                cmdDB.ExecuteNonQuery()

                'ストアド戻り値チェック
                intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                If intRtn <> 0 Then
                    psMesBox(Me, "00002", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "整備進捗明細")

                    'ロールバック
                    conTrn.Rollback()

                    Exit Sub
                End If

                'コミット
                conTrn.Commit()

                '完了メッセージ
                psMesBox(Me, "00012", clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後, "整備進捗明細", "整備依頼書の台数の確認を行い、更新")

                '活性制御.
                Call msEnableScreen("")
            End If

        Catch ex As Exception

            If Not conTrn Is Nothing Then
                conTrn.Rollback()
            End If

            psMesBox(Me, "00002", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "整備進捗明細")
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
                psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub
    ''' <summary>
    ''' 交換部品・追加ボタン押下処理．
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        '検証チェック.
        If (Page.IsValid) Then

            Dim objDt As DataTable = Nothing
            Dim strBrach As String = String.Empty
            Dim conDB As SqlConnection = Nothing
            Dim cmdDB As SqlCommand
            Dim conTrn As SqlTransaction = Nothing
            Dim intRtn As Integer
            Dim intMaxValue As Integer = 0
            Dim dstOrders As New DataSet
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            objStack = New StackFrame
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------

            Try

                '開始ログ出力.
                psLogStart(Me)

                'グリッドの情報取得.
                objDt = pfParse_DataTable(Me.grvList)


                '連番の最大値取得.
                If Not clsDataConnect.pfOpen_Database(conDB) Then
                    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Else
                    cmdDB = New SqlCommand(M_DISP_ID & "_S3", conDB)
                    cmdDB.Parameters.Add(pfSet_Param("repair_no", SqlDbType.NVarChar, Me.lblMente_No.Text))
                    cmdDB.Parameters.Add(pfSet_Param("branch", SqlDbType.NVarChar, ViewState(P_KEY2)))
                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                End If
                '0件の場合,連番=1.
                If dstOrders.Tables(0).Rows.Count = 0 Or dstOrders.Tables(0).Rows(0).Item("連番").ToString = String.Empty Then
                    intMaxValue = 1
                Else
                    intMaxValue = (Integer.Parse(dstOrders.Tables(0).Rows(0).Item("連番").ToString()) + 1)
                End If
                'If objDt.Rows.Count <> 0 Then

                '    '連番の最大値取得.
                '    If Not clsDataConnect.pfOpen_Database(conDB) Then
                '        psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                '    Else
                '        cmdDB = New SqlCommand(M_DISP_ID & "_S3", conDB)
                '        cmdDB.Parameters.Add(pfSet_Param("repair_no", SqlDbType.NVarChar, Me.lblMente_No.Text))
                '        cmdDB.Parameters.Add(pfSet_Param("branch", SqlDbType.NVarChar, ViewState(P_KEY2)))
                '        dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                '    End If

                '    intMaxValue = (Integer.Parse(dstOrders.Tables(0).Rows(0).Item("連番").ToString()) + 1)
                'Else
                '    intMaxValue = 1
                'End If


                '接続
                If Not clsDataConnect.pfOpen_Database(conDB) Then
                    psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                Else

                    'トランザクション.
                    conTrn = conDB.BeginTransaction

                    cmdDB = New SqlCommand(M_DISP_ID & "_I1", conDB)
                    cmdDB.Connection = conDB
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = conTrn
                    With cmdDB.Parameters
                        .Add(pfSet_Param("mente_no", SqlDbType.NVarChar, Me.lblMente_No.Text))
                        .Add(pfSet_Param("branch", SqlDbType.NVarChar, ViewState(P_KEY2)))
                        .Add(pfSet_Param("seq_no", SqlDbType.NVarChar, intMaxValue))
                        .Add(pfSet_Param("parts_cd", SqlDbType.NVarChar, Me.ddlPartsNm.SelectedValue))
                        .Add(pfSet_Param("quantity", SqlDbType.NVarChar, Me.txtQuantity.ppText))
                        .Add(pfSet_Param("insert_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                        .Add(pfSet_Param("wrk_cls", SqlDbType.NVarChar, Me.ddlWrkCls.ppSelectedValue))
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                    End With

                    '実行
                    cmdDB.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, "00003", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "交換部品")

                        'ロールバック
                        conTrn.Rollback()

                        Exit Sub
                    End If

                    'コミット
                    conTrn.Commit()

                    '完了メッセージ
                    psMesBox(Me, "00003", clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後, "交換部品")

                    '交換部品リスト再取得.
                    Call mfSetList_Data(Me.lblMente_No.Text, ViewState(P_KEY2))

                    '交換部品初期化.
                    Me.ddlPartsNm.SelectedIndex = 0
                    Me.txtQuantity.ppText = String.Empty
                    Me.ddlWrkCls.ppDropDownList.SelectedIndex = 0
                    Me.ddlPartsNm.Enabled = False

                    '活性制御.
                    Call msEnableChangeScreen(sender)

                End If

            Catch ex As Exception

                If Not conTrn Is Nothing Then
                    conTrn.Rollback()
                End If

                psMesBox(Me, "00003", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "交換部品")
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
                    psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                End If

                '終了ログ出力
                psLogEnd(Me)

            End Try

        End If

    End Sub
    ''' <summary>
    ''' 交換部品・更新ボタン押下処理．
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        '検証チェック.
        If (Page.IsValid) Then

            Dim objDt As DataTable = Nothing
            Dim conDB As SqlConnection = Nothing
            Dim cmdDB As SqlCommand
            Dim conTrn As SqlTransaction = Nothing
            Dim intRtn As Integer
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            objStack = New StackFrame
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------

            'グリッドの情報取得.
            objDt = pfParse_DataTable(Me.grvList)

            Try

                '開始ログ出力.
                psLogStart(Me)

                '接続
                If Not clsDataConnect.pfOpen_Database(conDB) Then
                    psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                Else

                    'トランザクション.
                    conTrn = conDB.BeginTransaction

                    cmdDB = New SqlCommand(M_DISP_ID & "_U2", conDB)
                    cmdDB.Connection = conDB
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = conTrn
                    With cmdDB.Parameters
                        .Add(pfSet_Param("mente_no", SqlDbType.NVarChar, Me.lblMente_No.Text))
                        .Add(pfSet_Param("branch", SqlDbType.NVarChar, ViewState(P_KEY2)))
                        .Add(pfSet_Param("seq_no", SqlDbType.NVarChar, ViewState(P_KEY3)))
                        .Add(pfSet_Param("parts_cd", SqlDbType.NVarChar, Me.ddlPartsNm.SelectedValue))
                        .Add(pfSet_Param("quantity", SqlDbType.NVarChar, Me.txtQuantity.ppText))
                        .Add(pfSet_Param("update_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                        .Add(pfSet_Param("wrk_cls", SqlDbType.NVarChar, Me.ddlWrkCls.ppSelectedValue))
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                    End With

                    '実行
                    cmdDB.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, "00001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "交換部品")

                        'ロールバック
                        conTrn.Rollback()

                        Exit Sub
                    End If

                    'コミット
                    conTrn.Commit()

                    '完了メッセージ
                    psMesBox(Me, "00001", clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後, "交換部品")

                    '交換部品リスト再取得.
                    Call mfSetList_Data(Me.lblMente_No.Text, ViewState(P_KEY2))

                    '交換部品初期化.
                    Me.ddlPartsNm.SelectedIndex = 0
                    Me.txtQuantity.ppText = String.Empty
                    Me.ddlWrkCls.ppDropDownList.SelectedIndex = 0
                    Me.ddlPartsNm.Enabled = False

                    '活性制御.
                    Call msEnableChangeScreen(sender)

                End If

            Catch ex As Exception

                If Not conTrn Is Nothing Then
                    conTrn.Rollback()
                End If

                psMesBox(Me, "00001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "交換部品")
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
                    psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                End If

                '終了ログ出力
                psLogEnd(Me)

            End Try

        End If

    End Sub
    ''' <summary>
    ''' 交換部品・削除ボタン押下処理．
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        '検証チェック.
        If (Page.IsValid) Then

            Dim objDt As DataTable = Nothing
            Dim conDB As SqlConnection = Nothing
            Dim cmdDB As SqlCommand
            Dim conTrn As SqlTransaction = Nothing
            Dim intRtn As Integer
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            objStack = New StackFrame
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------

            'グリッドの情報取得.
            objDt = pfParse_DataTable(Me.grvList)

            Try

                '開始ログ出力.
                psLogStart(Me)

                '接続
                If Not clsDataConnect.pfOpen_Database(conDB) Then
                    psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                Else

                    'トランザクション.
                    conTrn = conDB.BeginTransaction

                    cmdDB = New SqlCommand(M_DISP_ID & "_D2", conDB)
                    cmdDB.Connection = conDB
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = conTrn
                    With cmdDB.Parameters
                        .Add(pfSet_Param("mente_no", SqlDbType.NVarChar, Me.lblMente_No.Text))
                        .Add(pfSet_Param("branch", SqlDbType.NVarChar, ViewState(P_KEY2)))
                        .Add(pfSet_Param("seq_no", SqlDbType.NVarChar, ViewState(P_KEY3)))
                        .Add(pfSet_Param("delete_flg", SqlDbType.NVarChar, "1"))
                        .Add(pfSet_Param("update_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                    End With

                    '実行
                    cmdDB.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, "00002", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "交換部品")

                        'ロールバック
                        conTrn.Rollback()

                        Exit Sub
                    End If

                    'コミット
                    conTrn.Commit()

                    '完了メッセージ
                    psMesBox(Me, "00002", clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後, "交換部品")

                    '交換部品初期化.
                    Me.ddlPartsNm.SelectedIndex = 0
                    Me.txtQuantity.ppText = String.Empty
                    Me.ddlWrkCls.ppDropDownList.SelectedIndex = 0
                    Me.ddlPartsNm.Enabled = False

                    '活性制御.
                    Call msEnableChangeScreen(sender)

                    '交換部品リスト再取得.
                    Call mfSetList_Data(Me.lblMente_No.Text, ViewState(P_KEY2))

                End If

            Catch ex As Exception

                If Not conTrn Is Nothing Then
                    conTrn.Rollback()
                End If

                psMesBox(Me, "00002", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "交換部品")
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
                    psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                End If

                '終了ログ出力
                psLogEnd(Me)

            End Try

        End If

    End Sub
    ''' <summary>
    ''' 交換部品・クリアボタン押下処理．
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        '交換部品初期化.
        If Me.ddlPartsNm.SelectedIndex > 0 Then
            Me.ddlPartsNm.SelectedIndex = 0
        End If
        Me.txtQuantity.ppText = String.Empty
        '--------------------------------
        '2014/06/24 星野　ここから
        '--------------------------------
        'If Me.ddlPartsNm.SelectedIndex > 0 Then
        '    Me.ddlWrkCls.ppDropDownList.SelectedIndex = 0
        'End If
        If Me.ddlWrkCls.ppDropDownList.SelectedIndex > 0 Then
            Me.ddlWrkCls.ppDropDownList.SelectedIndex = 0
        End If
        '--------------------------------
        '2014/06/24 星野　ここまで
        '--------------------------------

        '活性制御.
        Call msEnableChangeScreen(sender)

    End Sub
    ''' <summary>
    ''' 元に戻すボタン押下処理．
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        '整備進捗明細データ再取得処理.
        Call mfGet_Data(ViewState(P_KEY), ViewState(P_KEY2), False)

    End Sub
    ''' <summary>
    ''' 一覧の選択ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand
        Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    ' ボタン押下行のIndex
        Dim rowData As GridViewRow = grvList.Rows(intIndex)             ' ボタン押下行

        '作業分類変更.
        Me.ddlWrkCls.ppSelectedValue = CType(rowData.FindControl("作業分類"), TextBox).Text

        '作業分類変更時処理.
        Call msWrkCls_TextChanged(sender, CType(rowData.FindControl("交換部品コード"), TextBox).Text)

        'コントロールにセット.
        Me.ddlPartsNm.SelectedValue = CType(rowData.FindControl("交換部品コード"), TextBox).Text
        Me.txtQuantity.ppText = CType(rowData.FindControl("個数"), TextBox).Text
        ViewState(P_KEY3) = CType(rowData.FindControl("連番"), TextBox).Text

        '活性制御.
        Call msEnableChangeScreen(sender)

    End Sub
    ''' <summary>
    ''' 部品名の検証.
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvPartsNm_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvPartsNm.ServerValidate
        Dim dtrMes As DataRow
        args.IsValid = True
        If ddlPartsNm.SelectedIndex = 0 Then '未選択
            dtrMes = pfGet_ValMes("5001", "部品名")
            cuvPartsNm.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            cuvPartsNm.Text = dtrMes.Item(P_VALMES_SMES)
            args.IsValid = False
        End If

        If ddlPartsNm.SelectedIndex = -1 Then '未選択
            dtrMes = pfGet_ValMes("5001", "部品名")
            cuvPartsNm.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            cuvPartsNm.Text = dtrMes.Item(P_VALMES_SMES)
            args.IsValid = False
        End If

    End Sub

#End Region

#Region "そのほかのプロシージャ"
    ''' <summary>
    ''' 初期化処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()

        'コントロールの初期化 .
        Me.lblReqComp_Nm.Text = String.Empty
        Me.lblMente_No.Text = String.Empty
        Me.lblOrder_No.Text = String.Empty
        Me.lblWrkCls_Cd.Text = String.Empty
        Me.lblTboxCls_Cd.Text = String.Empty
        Me.lblVersion_Nm.Text = String.Empty
        Me.lblCeriv_Nm.Text = String.Empty
        Me.lblZip_No.Text = String.Empty
        Me.lblAddr.Text = String.Empty
        Me.lblTel_No.Text = String.Empty
        Me.lblFax_No.Text = String.Empty
        Me.lblAppa_Nm.Text = String.Empty
        Me.dtbRsltSend_Dt.ppText = String.Empty
        Me.dtbSend_Dt.ppText = String.Empty
        Me.txtSerial_No.ppText = String.Empty
        Me.dtbReceipt_Dt.ppText = String.Empty

        'グリッドビューの初期化.
        Me.grvList.DataSource = New DataTable
        Me.grvList.DataBind()

    End Sub
    ''' <summary>
    ''' 作業分類変更時処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Private Sub msWrkCls_TextChanged(ByVal sender As Object, ByVal strPartCd As String)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            '開始ログ出力.
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            Else

                '部品名ドロップダウンリスト生成.
                cmdDB = New SqlCommand(sCnsSqlid_027, conDB)
                cmdDB.Parameters.Add(pfSet_Param("maker_cd", SqlDbType.NVarChar, ViewState(P_REQCOMP_CD)))
                cmdDB.Parameters.Add(pfSet_Param("wrk_cls", SqlDbType.NVarChar, Me.ddlWrkCls.ppSelectedValue))
                cmdDB.Parameters.Add(pfSet_Param("part_cd", SqlDbType.NVarChar, strPartCd))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                Me.ddlPartsNm.DataSource = dstOrders.Tables(0)
                Me.ddlPartsNm.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
                Me.ddlPartsNm.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
                Me.ddlPartsNm.DataBind()
                '--------------------------------
                '2014/05/20 後藤　ここから
                '--------------------------------
                'Me.ddlPartsNm.Items.Insert(0, " ")
                Me.ddlPartsNm.Items.Insert(0, New ListItem(Nothing, Nothing))
                '--------------------------------
                '2014/05/20 後藤　ここまで
                '--------------------------------

                '活性制御.
                Call msEnableChangeScreen(sender)

            End If

        Catch ex As Exception
            psMesBox(Me, "00004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "部品名")
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
                psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub
    ''' <summary>
    ''' ドロップダウンリスト生成処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfGet_DropListData_Sel(ByVal ipstrReqcomp_Cd) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet        '表示情報のデータセット
        Dim strMessage As String = String.Empty
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            '開始ログ出力.
            psLogStart(Me)

            '接続.
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                mfGet_DropListData_Sel = False
                Exit Function
            End If

            'ステータスドロップダウンリスト生成.
            strMessage = sCnsErrorMes_015
            cmdDB = New SqlCommand(sCnsSqlid_015, conDB)
            cmdDB.Parameters.Add(pfSet_Param("duty_cd", SqlDbType.NVarChar, "87"))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlPrgStatus_Cd.DataSource = dstOrders.Tables(0)
            Me.ddlPrgStatus_Cd.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlPrgStatus_Cd.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlPrgStatus_Cd.DataBind()
            '--------------------------------
            '2014/05/20 後藤　ここから
            '--------------------------------
            'Me.ddlPrgStatus_Cd.Items.Insert(0, " ")
            Me.ddlPrgStatus_Cd.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/20 後藤　ここまで
            '--------------------------------

            '正常終了.
            mfGet_DropListData_Sel = True

        Catch ex As Exception
            '--------------------------------
            '2014/04/15 後藤　ここから
            '--------------------------------
            'psMesBox(Me, "00004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画前, strMessage)
            psMesBox(Me, "30015", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画前, strMessage)
            '--------------------------------
            '2014/05/15 後藤　ここまで
            '--------------------------------

            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            mfGet_DropListData_Sel = False
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                mfGet_DropListData_Sel = False
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function
    ''' <summary>
    ''' 整備進捗明細データ取得処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfGet_Data(ByVal ipstrMente_No As String,
                           ByVal ipstrBranch As String,
                           Optional ByVal ipblnSeachFlg As Boolean = True) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet
        Dim cmdDBList As New SqlCommand
        Dim dstOrdersList As New DataSet
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        mfGet_Data = False

        Try

            '開始ログ出力.
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                Exit Function

            Else

                '修正依頼データ取得.
                cmdDB = New SqlCommand(M_DISP_ID & "_S1", conDB)
                cmdDB.Parameters.Add(pfSet_Param("mente_no", SqlDbType.NVarChar, ipstrMente_No))
                cmdDB.Parameters.Add(pfSet_Param("branch", SqlDbType.NVarChar, ipstrBranch))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '初回ロード時のみ.
                If ipblnSeachFlg = True Then
                    'ドロップダウンリスト生成.
                    If Not mfGet_DropListData_Sel(dstOrders.Tables(0).Rows(0).Item("依頼先会社コード").ToString()) Then
                        Exit Function
                    End If
                End If

                '依頼先会社コード保存.
                ViewState(P_REQCOMP_CD) = dstOrders.Tables(0).Rows(0).Item("依頼先会社コード").ToString()

                '取得したデータをコントロールに設定.
                Call msSetDisp_Data(dstOrders)

            End If

            mfGet_Data = True
        Catch ex As Exception
            psMesBox(Me, "00004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画前, "整備進捗明細")
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Exit Function
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                mfGet_Data = False

            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function
    ''' <summary>
    ''' 取得データ設定処理.
    ''' </summary>
    ''' <param name="dstOrders"></param>
    ''' <remarks></remarks>
    Private Sub msSetDisp_Data(ByVal dstOrders As DataSet)

        Me.lblReqComp_Nm.Text = dstOrders.Tables(0).Rows(0).Item("依頼先").ToString()
        Me.lblMente_No.Text = dstOrders.Tables(0).Rows(0).Item("管理番号").ToString()
        Me.lblOrder_No.Text = dstOrders.Tables(0).Rows(0).Item("注文番号").ToString()
        Me.lblWrkCls_Cd.Text = dstOrders.Tables(0).Rows(0).Item("作業種別").ToString()
        Me.lblTboxCls_Cd.Text = dstOrders.Tables(0).Rows(0).Item("システム").ToString()
        Me.lblVersion_Nm.Text = dstOrders.Tables(0).Rows(0).Item("VER設定").ToString()
        Me.lblCeriv_Nm.Text = dstOrders.Tables(0).Rows(0).Item("納入先名").ToString()
        Me.lblZip_No.Text = dstOrders.Tables(0).Rows(0).Item("郵便番号").ToString()
        Me.lblAddr.Text = dstOrders.Tables(0).Rows(0).Item("住所").ToString()
        Me.lblTel_No.Text = dstOrders.Tables(0).Rows(0).Item("TEL").ToString()
        Me.lblFax_No.Text = dstOrders.Tables(0).Rows(0).Item("FAX").ToString()
        Me.lblAppa_Nm.Text = dstOrders.Tables(0).Rows(0).Item("機器名").ToString()
        Me.ddlTmptst_Cd.ppSelectedValue = dstOrders.Tables(0).Rows(0).Item("一時診断結果").ToString()
        Me.dtbRsltSend_Dt.ppText = dstOrders.Tables(0).Rows(0).Item("診断結果送付日").ToString()
        Me.ddlPrgStatus_Cd.SelectedValue = dstOrders.Tables(0).Rows(0).Item("ステータス").ToString()
        Me.dtbSend_Dt.ppText = dstOrders.Tables(0).Rows(0).Item("完了発送日").ToString()
        Me.ddlWork.ppSelectedValue = dstOrders.Tables(0).Rows(0).Item("作業内容").ToString()
        Me.txtSerial_No.ppText = dstOrders.Tables(0).Rows(0).Item("シリアルNo").ToString()
        Me.dtbReceipt_Dt.ppText = dstOrders.Tables(0).Rows(0).Item("受領日").ToString()
        Me.txtReq_Dt.ppText = dstOrders.Tables(0).Rows(0).Item("請求年月").ToString()
        Me.ddlClass.ppSelectedValue = dstOrders.Tables(0).Rows(0).Item("区分").ToString()

    End Sub
    ''' <summary>
    ''' 活性制御.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msEnableScreen(ByVal strTerms As String)

        Select Case strTerms.ToString.Trim

            Case ClsComVer.E_遷移条件.更新
                Me.pnlMente.Enabled = True
                Me.ddlPartsNm.Enabled = False
                Me.btnDelete.Enabled = False
                Me.btnUpdate.Enabled = False
                Me.btnInsert.Enabled = True
                Me.txtQuantity.ppEnabled = True
                Me.ddlWrkCls.ppEnabled = True
                Me.btnClear.Enabled = True
                Master.ppRigthButton1.Enabled = True
                Master.ppRigthButton2.Enabled = True
                Master.ppRigthButton3.Enabled = True

            Case ClsComVer.E_遷移条件.参照

                'Me.pnlMente.Enabled = False
                '--------------------------------
                '2014/06/24 武　ここから
                '--------------------------------
                Me.ddlTmptst_Cd.ppEnabled = False
                Me.dtbRsltSend_Dt.ppEnabled = False
                Me.ddlPrgStatus_Cd.Enabled = False
                Me.dtbSend_Dt.ppEnabled = False
                Me.ddlWork.ppEnabled = False
                Me.txtSerial_No.ppEnabled = False
                Me.dtbReceipt_Dt.ppEnabled = False
                Me.ddlClass.ppEnabled = False
                Me.txtReq_Dt.ppEnabled = False
                Me.ddlWrkCls.ppEnabled = False
                Me.ddlPartsNm.Enabled = False
                Me.txtQuantity.ppEnabled = False
                Me.btnClear.Enabled = False
                Me.btnInsert.Enabled = False
                Me.btnUpdate.Enabled = False
                Me.btnDelete.Enabled = False
                '--------------------------------
                '2014/06/24 武　ここまで
                '--------------------------------
                Master.ppRigthButton1.Enabled = False
                Master.ppRigthButton2.Enabled = False
                Master.ppRigthButton3.Enabled = False

            Case Else

                Me.pnlMente.Enabled = False
                Master.ppRigthButton1.Enabled = False
                Master.ppRigthButton2.Enabled = False
                Master.ppRigthButton3.Enabled = False

        End Select

        If ddlPrgStatus_Cd.SelectedValue = "03" And Me.txtReq_Dt.ppText < System.DateTime.Now.ToString("yyyyMM") And Me.txtReq_Dt.ppText <> "" Then
            Me.btnInsert.Enabled = False
            Me.btnDelete.Enabled = False
            Me.btnUpdate.Enabled = False
            Me.btnClear.Enabled = False
            Me.ddlWrkCls.ppEnabled = False
            Me.ddlPartsNm.Enabled = False
            Me.txtQuantity.ppEnabled = False
            Me.Master.ppRigthButton2.Enabled = False
        End If

    End Sub
    ''' <summary>
    ''' 交換部品活性制御.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Private Sub msEnableChangeScreen(ByVal sender As Object)

        Select Case sender.ID
            Case "btnClear"        'クリア
                '--------------------------------
                '2014/06/24 武　ここから
                '--------------------------------
                If ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.参照 Then
                    '--------------------------------
                    '2014/06/24 武　ここまで
                    '--------------------------------
                    Me.btnInsert.Enabled = True
                    Me.btnDelete.Enabled = False
                    Me.btnUpdate.Enabled = False
                    Me.ddlPartsNm.Enabled = False
                    '--------------------------------
                    '2014/06/24 武　ここから
                    '--------------------------------
                Else
                    Me.btnInsert.Enabled = False
                    Me.btnDelete.Enabled = False
                    Me.btnUpdate.Enabled = False
                    Me.btnClear.Enabled = False
                End If
                '--------------------------------
                '2014/06/24 武　ここまで
                '--------------------------------
            Case "btnInsert"       '登録.
                If ddlPrgStatus_Cd.SelectedValue = "03" And Me.txtReq_Dt.ppText < System.DateTime.Now.ToString("yyyyMM") And Me.txtReq_Dt.ppText <> "" Then
                    Me.ddlWrkCls.ppSelectedValue = ""
                    Me.ddlPartsNm.SelectedValue = ""
                    Me.txtQuantity.ppText = ""
                    Me.btnInsert.Enabled = False
                    Me.btnDelete.Enabled = False
                    Me.btnUpdate.Enabled = False
                    Me.btnClear.Enabled = False
                    Me.ddlWrkCls.ppEnabled = False
                    Me.ddlPartsNm.Enabled = False
                    Me.txtQuantity.ppEnabled = False
                Else
                    If ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.参照 Then
                        Me.ddlWrkCls.ppSelectedValue = ""
                        Me.ddlWrkCls.ppEnabled = True
                        Me.btnClear.Enabled = True
                        Me.btnInsert.Enabled = True
                        Me.btnDelete.Enabled = False
                        Me.btnUpdate.Enabled = False
                        Me.ddlPartsNm.Enabled = False
                    End If
                End If

            Case "btnUpdate"       '更新.
                If ddlPrgStatus_Cd.SelectedValue = "03" And Me.txtReq_Dt.ppText < System.DateTime.Now.ToString("yyyyMM") And Me.txtReq_Dt.ppText <> "" Then
                    Me.ddlWrkCls.ppSelectedValue = ""
                    Me.ddlPartsNm.SelectedValue = ""
                    Me.txtQuantity.ppText = ""
                    Me.btnInsert.Enabled = False
                    Me.btnDelete.Enabled = False
                    Me.btnUpdate.Enabled = False
                    Me.btnClear.Enabled = False
                    Me.ddlWrkCls.ppEnabled = False
                    Me.ddlPartsNm.Enabled = False
                    Me.txtQuantity.ppEnabled = False
                Else
                    If ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.参照 Then
                        Me.ddlWrkCls.ppSelectedValue = ""
                        Me.ddlWrkCls.ppEnabled = True
                        Me.btnClear.Enabled = True
                        Me.btnInsert.Enabled = True
                        Me.btnDelete.Enabled = False
                        Me.btnUpdate.Enabled = False
                        Me.ddlPartsNm.Enabled = False
                    End If
                End If

            Case "btnDelete"       '削除.
                Me.btnInsert.Enabled = True
                Me.btnDelete.Enabled = False
                Me.btnUpdate.Enabled = False
            Case "grvList"         '選択.
                '--------------------------------
                '2014/06/24 武　ここから
                '--------------------------------
                If ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.参照 Then
                    '--------------------------------
                    '2014/06/24 武　ここまで
                    '--------------------------------
                    Me.btnInsert.Enabled = False
                    Me.btnDelete.Enabled = True
                    Me.btnUpdate.Enabled = True
                    '--------------------------------
                    '2014/06/24 武　ここから
                    '--------------------------------
                    Me.ddlPartsNm.Enabled = False
                Else
                    Me.btnInsert.Enabled = False
                    Me.btnDelete.Enabled = False
                    Me.btnUpdate.Enabled = False
                    Me.btnClear.Enabled = True
                End If
                '--------------------------------
                '2014/06/24 武　ここまで
                '--------------------------------
            Case "ddlList"
                Me.ddlPartsNm.Enabled = True
                Me.txtQuantity.ppEnabled = True
                Me.btnClear.Enabled = True
                Me.btnInsert.Enabled = True

            Case Else
                If ddlPrgStatus_Cd.SelectedValue = "03" And Me.txtReq_Dt.ppText < System.DateTime.Now.ToString("yyyyMM") And Me.txtReq_Dt.ppText <> "" Then
                    Me.ddlWrkCls.ppSelectedValue = ""
                    Me.ddlPartsNm.SelectedValue = ""
                    Me.txtQuantity.ppText = ""
                    Me.btnInsert.Enabled = False
                    Me.btnDelete.Enabled = False
                    Me.btnUpdate.Enabled = False
                    Me.btnClear.Enabled = False
                    Me.ddlWrkCls.ppEnabled = False
                    Me.ddlPartsNm.Enabled = False
                    Me.txtQuantity.ppEnabled = False
                End If
        End Select
    End Sub
    ''' <summary>
    ''' 交換部品一覧データ表示処理.
    ''' </summary>
    ''' <param name="ipstrMente_No"></param>
    ''' <param name="ipstrBranch"></param>
    ''' <remarks></remarks>
    Private Function mfSetList_Data(ByVal ipstrMente_No As String, ByVal ipstrBranch As String) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        mfSetList_Data = False

        '接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Exit Function
        Else
            Try
                cmdDB = New SqlCommand(M_DISP_ID & "_S2", conDB)
                cmdDB.Parameters.Add(pfSet_Param("mente_no", SqlDbType.NVarChar, ipstrMente_No))
                cmdDB.Parameters.Add(pfSet_Param("branch", SqlDbType.NVarChar, ipstrBranch))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得したデータをリストに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

                mfSetList_Data = True
            Catch ex As Exception
                psMesBox(Me, "00004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "交換部品一覧")
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
                Exit Function
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                    mfSetList_Data = False
                End If
            End Try
        End If
    End Function
    ''' <summary>
    ''' 入力チェック.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheck_Error()

        Dim strErr As String
        Dim strReqDt As String

        '空白時はチェックしない.
        If Me.txtReq_Dt.ppText <> String.Empty Then

            '請求年月桁数チェック.
            strErr = pfCheck_TxtErr(Me.txtReq_Dt.ppText, False, False, False, True, 6, "", False)
            If strErr <> String.Empty Then
                Me.txtReq_Dt.psSet_ErrorNo(strErr, "請求年月", "6")
                Exit Sub
            End If

            '請求年月整合性チェック.
            strErr = pfCheck_TxtErr(Me.txtReq_Dt.ppText, False, True, True, True, 6, "[0-9][0-9][0-9][0-9]([0][1-9]|[1][0-2])", False)
            If strErr <> "" Then
                Me.txtReq_Dt.psSet_ErrorNo(strErr, "請求年月", "年月")
                Exit Sub
            End If

            '請求年月下限上限チェック.
            strReqDt = Me.txtReq_Dt.ppText.Substring(0, 4) & "/" & Me.txtReq_Dt.ppText.Substring(4, 2) & "/" & "01"
            If Not strReqDt >= "2000/01/01" Then
                Me.txtReq_Dt.psSet_ErrorNo("6003", "請求年月", "2000年")
                Exit Sub
            End If

            '--------------------------------
            '2014/05/16 後藤　ここから
            '--------------------------------
            '更新用.
            ViewState(sCnsReqDt) = strReqDt
            '--------------------------------
            '2014/05/16 後藤　ここまで
            '--------------------------------
        Else
            ViewState(sCnsReqDt) = String.Empty
        End If


    End Sub
#End Region
    '--------------------------------
    '2014/05/20 後藤　ここから
    '--------------------------------
    ''' <summary>
    ''' オブジェクト取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDBNull(ByVal strVal As String) As Object
        If strVal.Trim() = "" Then
            Return DBNull.Value
        End If
        Return strVal

    End Function
    '--------------------------------
    '2014/05/20 後藤　ここまで
    '--------------------------------
End Class