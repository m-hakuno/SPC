'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜修理・整備業務＞
'*　処理名　　：　整備依頼書
'*　ＰＧＭＩＤ：　MNTUPDP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.27　：　後藤
'*  作　成　　：　2014.06.30　：　間瀬　郵便番号等のエラー制御修正
'*  作　成　　：　2014.07.01　：　間瀬　明細追加処理バグ修正
'*  更　新　　：　2015.01.09　：　稲葉　管理番号の年を年度で取得する処理追加
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'MNTUPDP002-001     2016/01/22      栗原      帳票の印字項目送付先、送付元、納入先をマスタ参照するよう変更

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive
Imports System.Globalization
#End Region

Public Class MNTUPDP001
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
    Private Const M_DISP_ID = P_FUN_MNT & P_SCR_UPD & P_PAGE & "001"

    '整備進捗明細 参照／更新 画面のパス
    Private Const M_MNTSELP001 As String = "~/" & P_RPE & "/" &
        P_FUN_MNT & P_SCR_SEL & P_PAGE & "001" & "/" &
        P_FUN_MNT & P_SCR_SEL & P_PAGE & "001.aspx"

    ''' <summary>機器種別台数</summary>
    Private Const P_APPA_CNT As String = "APPA_CNT"
    Private Const P_REC_CNT As String = "REC_CNT"

    '--------------------------------
    '2014/05/14 後藤　ここから
    '--------------------------------
    ''' <summary>注文番号</summary>
    Private Const P_ORDER_NO As String = "ORDER_NO"
    '--------------------------------
    '2014/05/14 後藤　ここまで
    '--------------------------------

    ''' <summary>納入先の情報取得</summary>
    Private Const sCnsSqlid_032 As String = "ZCMPSEL032"
    'MNTUPDP002-001 過去帳票の納入先とマスタ情報結合用
    Private Const OLD_FS_COD As String = "90-01"
    Private Const NEW_FS_COD As String = "00-02"
    Private Const OLD_BW_COD As String = "90-02"
    Private Const NEW_BW_COD As String = "00-06"
    'MNTUPDP002-001 END




    ''' <summary>TBOX機種マスタ情報取得</summary>
    Private Const sCnsSqlid_038 As String = "ZCMPSEL038"
    ''' <summary>倉庫マスタ情報取得</summary>
    Private Const sCnsSqlid_039 As String = "ZCMPSEL039"
    ''' <summary>業者マスタ情報取得</summary>
    Private Const sCnsSqlid_040 As String = "ZCMPSEL040"

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
    Dim intDel As Integer = 0
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

        Dim strKey As String = String.Empty '管理番号
        Dim strId As String = String.Empty
        Dim strTerms As String = String.Empty
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstReq As New DataSet

        If Not IsPostBack Then '初回表示.

            If Session(P_SESSION_TERMS) Is Nothing Then
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

            ViewState("CheckCode") = ""

            'セッション項目取得
            ViewState(P_SESSION_OLDDISP) = Session(P_SESSION_OLDDISP)
            ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)

            'ビューステート項目取得
            strId = ViewState(P_SESSION_OLDDISP)
            strTerms = ViewState(P_SESSION_TERMS)

            'プログラムＩＤ、画面名設定.
            Master.ppProgramID = M_DISP_ID
            Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

            'パンくずリスト設定.
            Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

            '排他情報用のグループ番号保管.
            If Not Session(P_SESSION_GROUP_NUM) Is Nothing Then
                ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)
            End If

            '排他情報削除用の登録年月日時刻をマスタページのコントロールに登録.
            If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then
                Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)
            End If

            '--------------------------------
            '2014/05/14 後藤　ここから
            '--------------------------------
            'ボタン活性
            Master.ppRigthButton1.Text = "登録"
            Master.ppRigthButton1.Visible = True
            Master.ppRigthButton2.Text = "削除"
            Master.ppRigthButton2.Visible = True
            Master.ppRigthButton3.Text = "クリア"
            Master.ppRigthButton3.Visible = True
            Master.ppLeftButton1.Text = "印刷"
            Master.ppLeftButton1.Visible = True

            Master.ppRigthButton1.ValidationGroup = 1
            '--------------------------------
            '2014/05/14 後藤　ここまで
            '--------------------------------

            Select Case strTerms.ToString.Trim

                Case ClsComVer.E_遷移条件.参照

                    ViewState(P_KEY) = DirectCast(Session(P_KEY), String())(0)
                    strKey = ViewState(P_KEY)

                    'コントロール初期化.
                    Call msClearScreen()

                    '整備依頼書データ取得処理.
                    Call msGet_Data(strKey)

                    '整備依頼書（整備機器一覧）データ取得処理.
                    Call msSetList_Data(strKey)

                    '活性制御
                    Call msEnableScreen(strTerms)

                Case ClsComVer.E_遷移条件.更新

                    ViewState(P_KEY) = DirectCast(Session(P_KEY), String())(0)
                    strKey = ViewState(P_KEY)

                    'コントロール初期化.
                    Call msClearScreen()

                    '整備依頼書データ取得処理.
                    Call msGet_Data(strKey)

                    '整備依頼書（整備機器一覧）データ取得処理.
                    Call msSetList_Data(strKey)

                    '活性制御
                    Call msEnableScreen(strTerms)

                    '更新前の機器種別台数保管.
                    ViewState(P_APPA_CNT) = Me.txtAppa_Cnt.ppText

                    '---------------------------
                    '2014/05/14 後藤 ここから
                    '---------------------------
                    '更新前の注文番号保管.
                    ViewState(P_ORDER_NO) = Me.txtOrder_No.ppText & Me.txtOrder_Seq.ppText
                    '---------------------------
                    '2014/05/14 後藤 ここまで
                    '---------------------------


                Case ClsComVer.E_遷移条件.登録

                    'コントロール初期化.
                    Call msClearScreen()

                    Me.txtOrder_No.ppText = "NGC整"

                    Me.txtOrder_Seq.ppText = ""
                    ''接続
                    'If Not clsDataConnect.pfOpen_Database(conDB) Then
                    '    psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                    'Else
                    '    Try
                    '        '注文番号存在チェック.
                    '        cmdDB = New SqlCommand("ZCMPSEL043", conDB)
                    '        cmdDB.Parameters.Add(pfSet_Param("MNGNO_CLS", SqlDbType.Int, 4))
                    '        cmdDB.Parameters.Add(pfSet_Param("YMD", SqlDbType.NVarChar, DateTime.Now.ToString("yyyy0101")))
                    '        cmdDB.Parameters.Add(pfSet_Param("SalesYTD", SqlDbType.Int, 4, ParameterDirection.Output))
                    '        dstReq = clsDataConnect.pfGet_DataSet(cmdDB)
                    '        Me.txtOrder_Seq.ppText = DateTime.Now.ToString("yy") & "-" & Integer.Parse(dstReq.Tables(0).Rows(0).Item("連番").ToString).ToString("000")
                    '    Catch ex As Exception
                    '        psMesBox(Me, "00004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "整備依頼番号情報")
                    '    Finally
                    '        'DB切断
                    '        If Not clsDataConnect.pfClose_Database(conDB) Then
                    '            psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                    '        End If
                    '    End Try
                    'End If

                    '活性制御
                    Call msEnableScreen(strTerms)

            End Select

        End If

        'ボタンアクションの設定.
        Call msSet_ButtonAction()

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
            Case "営業所", "NGC"
                ddlReqcomp_Cd.Enabled = False
                txtOrder_No.ppEnabled = False
                txtOrder_Seq.ppEnabled = False
                ddlRstAppaDiv.Enabled = False
                ddlAppa_Nm.Enabled = False
                ddlSystem.Enabled = False
                ddlRstAppaModel.Enabled = False
                txtAppa_Cnt.ppEnabled = False
                dtbArrval_D.ppEnabled = False
                txtSend_Nm.ppEnabled = False
                txtSend_NoteText.ppEnabled = False
                ddlWrkcls_cd.ppEnabled = False
                txtVersion.ppEnabled = False
                txtVersion.ppEnabled = False
                rdlMemCls.Enabled = False
                dtbDeliv_D.ppEnabled = False
                txtDeliv.ppEnabled = False
                ddlDeliv_Cd.Enabled = False
                txtZipNo.Enabled = False
                txtAddr.Enabled = False
                txtTel.Enabled = False
                txtFAX.Enabled = False
                txtCeriv_NoteText.ppEnabled = False
                '--------------------------------
                '2014/05/14 後藤　ここから
                '--------------------------------
                'Button1.Enabled = False
                'Button2.Enabled = False
                'btnClear.Enabled = False
                'btnDelete.Enabled = False
                'btnUpdate.Enabled = False

                Master.ppRigthButton1.Enabled = False
                Master.ppRigthButton2.Enabled = False
                Master.ppRigthButton3.Enabled = False
                Master.ppLeftButton1.Enabled = False
                '--------------------------------
                '2014/05/14 後藤　ここまで
                '--------------------------------

                'Case "NGC"
                '    ddlReqcomp_Cd.Enabled = False
                '    txtOrder_No.ppEnabled = False
                '    ddlSystem.Enabled = False
                '    ddlAppa_Nm.Enabled = False
                '    txtAppa_Cnt.ppEnabled = False
                '    dtbArrval_D.ppEnabled = False
                '    txtSend_Nm.ppEnabled = False
                '    txtSend_NoteText.ppEnabled = False
                '    ddlWrkcls_cd.ppEnabled = False
                '    txtVersion.ppEnabled = False
                '    txtVersion.ppEnabled = False
                '    rdlMemCls.Enabled = False
                '    dtbDeliv_D.ppEnabled = False
                '    txtDeliv.ppEnabled = False
                '    ddlDeliv_Cd.Enabled = False
                '    txtZipNo.Enabled = False
                '    txtAddr.Enabled = False
                '    txtTel.Enabled = False
                '    txtFAX.Enabled = False
                '    txtCeriv_NoteText.ppEnabled = False
                '    '--------------------------------
                '    '2014/05/14 後藤　ここから
                '    '--------------------------------
                '    'Button1.Enabled = False
                '    'Button2.Enabled = False
                '    'btnClear.Enabled = False
                '    'btnDelete.Enabled = False
                '    'btnUpdate.Enabled = False

                '    Master.ppRigthButton1.Enabled = False
                '    Master.ppRigthButton2.Enabled = False
                '    Master.ppRigthButton3.Enabled = False
                '    '--------------------------------
                '    '2014/05/14 後藤　ここまで
                '    '--------------------------------
        End Select

    End Sub
    '---------------------------
    '2014/04/24 武 ここまで
    '---------------------------

    ''' <summary>
    ''' 依頼先変更時処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlReqcomp_Cd_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet
        Dim intSeq As Integer = 0
        Dim strSeq As String = String.Empty
        Dim strMaker As String = String.Empty
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '空白の場合は処理しない.
        If Me.ddlReqcomp_Cd.SelectedIndex = "0" Then
            Exit Sub
        End If

        Try

            '開始ログ出力.
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            Else

                ''メーカータイプ取得.
                'cmdDB = New SqlCommand(M_DISP_ID & "_S5", conDB)
                'cmdDB.Parameters.Add(pfSet_Param("seach_flg", SqlDbType.NVarChar, "1"))
                'cmdDB.Parameters.Add(pfSet_Param("maker_cd", SqlDbType.NVarChar, Me.ddlReqcomp_Cd.SelectedValue))
                'dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                'strMaker = dstOrders.Tables(0).Rows(0).Item("メーカータイプ").ToString()

                ''依頼番号取得.
                'cmdDB = New SqlCommand(M_DISP_ID & "_S5", conDB)
                'cmdDB.Parameters.Add(pfSet_Param("seach_flg", SqlDbType.NVarChar, "2"))
                'cmdDB.Parameters.Add(pfSet_Param("maker_cd", SqlDbType.NVarChar, ""))
                'dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                ''整備依頼番号採番.
                'If dstOrders.Tables(0).Rows.Count = 0 OrElse String.IsNullOrEmpty(dstOrders.Tables(0).Rows(0).Item("依頼番号").ToString) Then
                '    strSeq = strMaker & String.Format("{0:00}", DateTime.Now.Month) & "-001"
                'Else
                '    strSeq = dstOrders.Tables(0).Rows(0).Item("依頼番号").ToString()
                '    intSeq = dstOrders.Tables(0).Rows(0).Item("依頼番号").ToString().Substring(3, 3)
                '    intSeq = intSeq + 1
                '    strSeq = strMaker & strSeq.Substring(0, 2) & "-" & String.Format("{0:000}", intSeq)
                'End If

                ''取得したデータをコントロールに設定
                'Me.lblMente_No.Text = strSeq


                '年度を取得する
                '（管理番号に付く年は3月末を締めとして設定する）
                '（例：2014/04～2015/03はYY=14、2015/04～2016/03はYY=15）
                Dim strFisYear As String = String.Empty
                If DateTime.Now.ToString("MM") <= "03" Then
                    strFisYear = DateTime.Now.AddYears(-1).ToString("yy")
                Else
                    strFisYear = DateTime.Now.ToString("yy")
                End If

                '依頼番号取得.
                cmdDB = New SqlCommand(M_DISP_ID & "_S5", conDB)
                cmdDB.Parameters.Add(pfSet_Param("seach_flg", SqlDbType.NVarChar, "2"))
                cmdDB.Parameters.Add(pfSet_Param("maker_cd", SqlDbType.NVarChar, Me.ddlReqcomp_Cd.SelectedValue))
                'cmdDB.Parameters.Add(pfSet_Param("YY", SqlDbType.NVarChar, DateTime.Now.ToString("yy")))
                cmdDB.Parameters.Add(pfSet_Param("YY", SqlDbType.NVarChar, strFisYear))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    'メーカータイプ取得.
                    cmdDB = New SqlCommand(M_DISP_ID & "_S5", conDB)
                    cmdDB.Parameters.Add(pfSet_Param("seach_flg", SqlDbType.NVarChar, "1"))
                    cmdDB.Parameters.Add(pfSet_Param("maker_cd", SqlDbType.NVarChar, Me.ddlReqcomp_Cd.SelectedValue))
                    cmdDB.Parameters.Add(pfSet_Param("YY", SqlDbType.NVarChar, ""))
                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                    If dstOrders.Tables(0).Rows.Count = 0 Then
                        psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "選択した依頼先は、整備管理番号の情報がない為選択できません。")
                        ddlReqcomp_Cd.SelectedIndex = -1
                        strSeq = ""
                    Else
                        strMaker = dstOrders.Tables(0).Rows(0).Item("メーカータイプ").ToString()
                        '依頼番号生成
                        strSeq = strMaker & String.Format("{0:00}", strFisYear) & "-001"
                        'If dstOrders.Tables(0).Rows.Count = 0 OrElse String.IsNullOrEmpty(dstOrders.Tables(0).Rows(0).Item("依頼番号").ToString) Then
                        '    strSeq = strMaker & String.Format("{0:00}", DateTime.Now.Year.ToString("yy")) & "-001"
                        'End If
                    End If
                Else
                    '取得したデータをコントロールに設定
                    strSeq = dstOrders.Tables(0).Rows(0).Item("依頼番号").ToString()
                End If
                Me.lblMente_No.Text = strSeq
            End If

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "整備依頼番号情報")
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

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub
    ''' <summary>
    ''' システム変更時処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlSystem_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        '空白の場合は検索しない.
        If Me.ddlSystem.SelectedIndex = "0" Then
            Exit Sub
        End If

        Dim strArrayList As String() = Nothing
        strArrayList = Me.ddlSystem.SelectedItem.ToString.Split(":")

        '選択値をシステムに設定.
        Me.lblSystem.Text = strArrayList(1).ToString

    End Sub
    ''' <summary>
    ''' 台数変更時処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtAppa_Cnt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        '空白の場合は検索しない.
        If Me.txtAppa_Cnt.ppText = String.Empty Then
            Exit Sub
        End If

        '選択値を台数に設定.
        Me.lblArrval_Cnt.Text = Me.txtAppa_Cnt.ppText
        Me.lblReq_Cnt.Text = Me.txtAppa_Cnt.ppText

    End Sub
    ''' <summary>
    ''' 納入先名変更時処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlDeliv_Cd_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
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

        '空白の場合は検索しない.
        If Me.ddlDeliv_Cd.SelectedIndex = "0" Then
            Exit Sub
        End If

        Try

            '開始ログ出力.
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            Else

                cmdDB = New SqlCommand(sCnsSqlid_032, conDB)
                cmdDB.Parameters.Add(pfSet_Param("code", SqlDbType.NVarChar, Me.ddlDeliv_Cd.SelectedValue))

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得したデータをコントロールに設定
                Me.txtZipNo.Text = dstOrders.Tables(0).Rows(0).Item("郵便番号").ToString()
                Me.txtAddr.Text = dstOrders.Tables(0).Rows(0).Item("住所").ToString()
                Me.txtTel.Text = dstOrders.Tables(0).Rows(0).Item("TEL").ToString()
                Me.txtFAX.Text = dstOrders.Tables(0).Rows(0).Item("FAX").ToString()
            End If

        Catch ex As Exception

            '--------------------------------
            '2014/05/13 後藤　ここから
            '--------------------------------
            'psMesBox(Me, "00004", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後, "納入先情報")
            psMesBox(Me, "00004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "納入先情報")
            '--------------------------------
            '2014/05/13 後藤　ここまで
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
    ''' 更新ボタン押下処理．
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        '排他制御用の変数定義.
        Dim strExclusiveDate As String = String.Empty
        Dim arTable_Name As New ArrayList
        Dim arKey As New ArrayList
        Dim strBtnFlag As String = String.Empty

        '個別エラーチェック.
        Call msCheck_Error()

        If mfCheck_Order() = False Then
            Exit Sub
        End If

        '検証チェック.
        If (Page.IsValid) Then

            Dim conDB As SqlConnection = Nothing
            Dim cmdDB As SqlCommand = Nothing
            Dim cmdDB_M As SqlCommand = Nothing
            Dim cmdDB_S As SqlCommand = Nothing
            Dim cmdDB_T As SqlCommand = Nothing
            Dim conTrn As SqlTransaction = Nothing
            Dim strMessage As String = String.Empty
            Dim intAppaCnt As Integer = 0
            Dim intOldAppa_Cnt As Integer = 0
            Dim intNewAppa_Cnt As Integer = 0
            Dim intRtn As Integer
            Dim strArrayList As String() = Nothing
            strArrayList = Me.ddlReqcomp_Cd.SelectedItem.ToString.Split(":")
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

                    '--------------------------------
                    '2014/05/14 後藤　ここから
                    '--------------------------------
                    'If Me.btnUpdate.Text = "登録" Then
                    If Master.ppRigthButton1.Text = "登録" Then
                        '--------------------------------
                        '2014/05/14 後藤　ここまで
                        '--------------------------------


                        '整備作業依頼テーブル登録処理.
                        cmdDB = New SqlCommand(M_DISP_ID & "_I1", conDB)
                        cmdDB.Connection = conDB
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = conTrn
                        strMessage = "登録処理"

                        With cmdDB.Parameters
                            .Add(pfSet_Param("mente_no", SqlDbType.NVarChar, Me.lblMente_No.Text))
                            .Add(pfSet_Param("reqcomp_cd", SqlDbType.NVarChar, Me.ddlReqcomp_Cd.SelectedValue))
                            .Add(pfSet_Param("reqcomp_nm", SqlDbType.NVarChar, strArrayList(1).ToString))
                            .Add(pfSet_Param("appa_div", SqlDbType.NVarChar, Me.ddlRstAppaDiv.SelectedValue))
                            .Add(pfSet_Param("appa_cd", SqlDbType.NVarChar, Me.ddlAppa_Nm.SelectedValue))
                            .Add(pfSet_Param("appa_model", SqlDbType.NVarChar, Me.ddlRstAppaModel.SelectedValue))
                            .Add(pfSet_Param("appa_cnt", SqlDbType.NVarChar, Me.txtAppa_Cnt.ppText))
                            .Add(pfSet_Param("arrival_d", SqlDbType.NVarChar, Me.dtbArrval_D.ppText))
                            .Add(pfSet_Param("arrival_cnt", SqlDbType.NVarChar, Me.lblArrval_Cnt.Text))
                            .Add(pfSet_Param("send_nm", SqlDbType.NVarChar, Me.txtSend_Nm.ppText))
                            .Add(pfSet_Param("send_notetext", SqlDbType.NVarChar, Me.txtSend_NoteText.ppText))
                            .Add(pfSet_Param("wrkcls_cd", SqlDbType.NVarChar, Me.ddlWrkcls_cd.ppSelectedValue))
                            .Add(pfSet_Param("tboxclass_cd", SqlDbType.NVarChar, Me.ddlSystem.SelectedValue))
                            .Add(pfSet_Param("version", SqlDbType.NVarChar, Me.txtVersion.ppText))
                            .Add(pfSet_Param("req_cnt", SqlDbType.NVarChar, Me.lblReq_Cnt.Text))
                            .Add(pfSet_Param("work_notetext", SqlDbType.NVarChar, Me.txtWork_NoteText.ppText))
                            .Add(pfSet_Param("memcls", SqlDbType.NVarChar, Me.rdlMemCls.SelectedValue))
                            .Add(pfSet_Param("deliv", SqlDbType.NVarChar, Me.txtDeliv.ppText))
                            .Add(pfSet_Param("deliv_d", SqlDbType.NVarChar, mfGetDBNull(Me.dtbDeliv_D.ppText)))
                            .Add(pfSet_Param("deliv_cd", SqlDbType.NVarChar, Me.ddlDeliv_Cd.SelectedValue))
                            .Add(pfSet_Param("zip_no", SqlDbType.NVarChar, Me.txtZipNo.Text))
                            .Add(pfSet_Param("addr", SqlDbType.NVarChar, Me.txtAddr.Text))
                            .Add(pfSet_Param("telno", SqlDbType.NVarChar, Me.txtTel.Text))
                            .Add(pfSet_Param("faxno", SqlDbType.NVarChar, Me.txtFAX.Text))
                            .Add(pfSet_Param("ceriv_notetext", SqlDbType.NVarChar, Me.txtCeriv_NoteText.ppText))
                            .Add(pfSet_Param("mente_end_flg", SqlDbType.NVarChar, "0"))
                            .Add(pfSet_Param("order_no", SqlDbType.NVarChar, Me.txtOrder_No.ppText & Me.txtOrder_Seq.ppText))
                            .Add(pfSet_Param("delete_flg", SqlDbType.NVarChar, "0"))
                            .Add(pfSet_Param("insert_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With

                        cmdDB.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            Select Case strMessage
                                Case "登録処理"
                                    psMesBox(Me, "00003", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "整備依頼書")
                                Case "更新処理"
                                    psMesBox(Me, "00001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "整備依頼書")
                            End Select

                            'ロールバック
                            conTrn.Rollback()

                            Exit Sub
                        End If

                        '整備作業依頼明細テーブル登録処理.
                        intAppaCnt = Integer.Parse(Me.txtAppa_Cnt.ppText)
                        For intCnt As Integer = 1 To intAppaCnt
                            cmdDB_M = New SqlCommand(M_DISP_ID & "_I2", conDB)
                            cmdDB_M.Connection = conDB
                            cmdDB_M.CommandType = CommandType.StoredProcedure
                            cmdDB_M.Transaction = conTrn
                            With cmdDB_M.Parameters
                                .Add(pfSet_Param("mente_no", SqlDbType.NVarChar, Me.lblMente_No.Text))
                                .Add(pfSet_Param("branch", SqlDbType.NVarChar, intCnt))
                                .Add(pfSet_Param("reqcomp_cd", SqlDbType.NVarChar, Me.ddlReqcomp_Cd.SelectedValue))
                                .Add(pfSet_Param("reqcomp_nm", SqlDbType.NVarChar, strArrayList(1).ToString))
                                .Add(pfSet_Param("appa_cd", SqlDbType.NVarChar, Me.ddlAppa_Nm.SelectedValue))
                                .Add(pfSet_Param("arrival_d", SqlDbType.NVarChar, Me.dtbArrval_D.ppText))
                                .Add(pfSet_Param("arrival_cnt", SqlDbType.NVarChar, Me.lblArrval_Cnt.Text))
                                .Add(pfSet_Param("send_nm", SqlDbType.NVarChar, Me.txtSend_Nm.ppText))
                                .Add(pfSet_Param("send_notetext", SqlDbType.NVarChar, Me.txtSend_NoteText.ppText))
                                .Add(pfSet_Param("wrkcls_cd", SqlDbType.NVarChar, Me.ddlWrkcls_cd.ppSelectedValue))
                                .Add(pfSet_Param("tboxclass_cd", SqlDbType.NVarChar, Me.ddlSystem.SelectedValue))
                                .Add(pfSet_Param("version", SqlDbType.NVarChar, Me.txtVersion.ppText))
                                .Add(pfSet_Param("req_cnt", SqlDbType.NVarChar, Me.lblReq_Cnt.Text))
                                .Add(pfSet_Param("work_notetext", SqlDbType.NVarChar, Me.txtWork_NoteText.ppText))
                                .Add(pfSet_Param("memcls", SqlDbType.NVarChar, Me.rdlMemCls.SelectedValue))
                                .Add(pfSet_Param("deliv", SqlDbType.NVarChar, Me.txtDeliv.ppText))
                                .Add(pfSet_Param("deliv_d", SqlDbType.NVarChar, mfGetDBNull(Me.dtbDeliv_D.ppText)))
                                .Add(pfSet_Param("mente_end_flg", SqlDbType.NVarChar, "0"))
                                .Add(pfSet_Param("insert_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                            End With

                            cmdDB_M.ExecuteNonQuery()

                            'ストアド戻り値チェック
                            intRtn = Integer.Parse(cmdDB_M.Parameters("retvalue").Value.ToString)
                            If intRtn <> 0 Then
                                Select Case strMessage
                                    Case "登録処理"
                                        psMesBox(Me, "00003", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "整備依頼書")
                                    Case "更新処理"
                                        psMesBox(Me, "00001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "整備依頼書")
                                End Select

                                'ロールバック
                                conTrn.Rollback()

                                Exit Sub
                            End If
                        Next


                        '整備依頼番号登録／更新処理

                        '年度を取得する
                        '（管理番号に付く年は3月末を締めとして設定する）
                        '（例：2014/04～2015/03はYY=14、2015/04～2016/03はYY=15）
                        Dim strFisYear As String = String.Empty
                        If DateTime.Now.ToString("MM") <= "03" Then
                            strFisYear = DateTime.Now.AddYears(-1).ToString("yy")
                        Else
                            strFisYear = DateTime.Now.ToString("yy")
                        End If

                        'メーカータイプ取得.
                        Dim dsMaker As New DataSet
                        Dim strMaker As String
                        cmdDB_T = New SqlCommand(M_DISP_ID & "_S5", conDB)
                        cmdDB_T.Connection = conDB
                        cmdDB_T.CommandType = CommandType.StoredProcedure
                        cmdDB_T.Transaction = conTrn
                        cmdDB_T.Parameters.Add(pfSet_Param("seach_flg", SqlDbType.NVarChar, "1"))
                        cmdDB_T.Parameters.Add(pfSet_Param("maker_cd", SqlDbType.NVarChar, Me.ddlReqcomp_Cd.SelectedValue))
                        cmdDB_T.Parameters.Add(pfSet_Param("YY", SqlDbType.NVarChar, ""))
                        dsMaker = clsDataConnect.pfGet_DataSet(cmdDB_T)
                        strMaker = dsMaker.Tables(0).Rows(0).Item("メーカータイプ").ToString()

                        '依頼番号取得.
                        Dim dsNo As New DataSet
                        cmdDB_T = New SqlCommand(M_DISP_ID & "_S5", conDB)
                        cmdDB_T.Connection = conDB
                        cmdDB_T.CommandType = CommandType.StoredProcedure
                        cmdDB_T.Transaction = conTrn
                        cmdDB_T.Parameters.Add(pfSet_Param("seach_flg", SqlDbType.NVarChar, "2"))
                        cmdDB_T.Parameters.Add(pfSet_Param("maker_cd", SqlDbType.NVarChar, Me.ddlReqcomp_Cd.SelectedValue))
                        'cmdDB_T.Parameters.Add(pfSet_Param("YY", SqlDbType.NVarChar, DateTime.Now.ToString("yy")))
                        cmdDB_T.Parameters.Add(pfSet_Param("YY", SqlDbType.NVarChar, strFisYear))
                        dsNo = clsDataConnect.pfGet_DataSet(cmdDB_T)

                        '整備作業依頼テーブル登録処理.
                        cmdDB_T = New SqlCommand(M_DISP_ID & "_I3", conDB)
                        cmdDB_T.Connection = conDB
                        cmdDB_T.CommandType = CommandType.StoredProcedure
                        cmdDB_T.Transaction = conTrn
                        strMessage = "登録処理"

                        With cmdDB_T.Parameters
                            .Add(pfSet_Param("mnt_cls", SqlDbType.NVarChar, Me.ddlReqcomp_Cd.SelectedValue))
                            .Add(pfSet_Param("mngno_cls", SqlDbType.NVarChar, strMaker))
                            '.Add(pfSet_Param("yy", SqlDbType.NVarChar, DateTime.Now.ToString("yy")))
                            .Add(pfSet_Param("yy", SqlDbType.NVarChar, strFisYear))
                            .Add(pfSet_Param("insert_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                            If dsNo.Tables(0).Rows.Count = 0 Then
                                .Add(pfSet_Param("list_no", SqlDbType.NVarChar, "2"))
                                .Add(pfSet_Param("branch_flg", SqlDbType.NVarChar, "0"))
                            Else
                                .Add(pfSet_Param("list_no", SqlDbType.NVarChar, Me.ddlRstAppaDiv.SelectedValue))
                                .Add(pfSet_Param("branch_flg", SqlDbType.NVarChar, "1"))
                            End If
                        End With

                        cmdDB_T.ExecuteNonQuery()


                        '注文番号存在チェック.
                        cmdDB_S = New SqlCommand("ZCMPSEL022", conDB)
                        cmdDB_S.Connection = conDB
                        cmdDB_S.CommandType = CommandType.StoredProcedure
                        cmdDB_S.Transaction = conTrn
                        cmdDB_S.Parameters.Add(pfSet_Param("MNGNO_CLS", SqlDbType.Int, 4))
                        cmdDB_S.Parameters.Add(pfSet_Param("YMD", SqlDbType.NVarChar, DateTime.Now.ToString("yyyy0101")))
                        cmdDB_S.Parameters.Add(pfSet_Param("SalesYTD", SqlDbType.Int, 4, ParameterDirection.Output))
                        cmdDB_S.ExecuteNonQuery()
                        If cmdDB_S.Parameters("SalesYTD").Value = Nothing Then
                            Select Case strMessage
                                Case "登録処理"
                                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "整備依頼書")
                                Case "更新処理"
                                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "整備依頼書")
                            End Select

                            'ロールバック
                            conTrn.Rollback()

                            Exit Sub
                        End If

                        'ロック対象テーブル名の登録.
                        arTable_Name.Insert(0, "D23_MENTE_REQUEST")

                        'ロックテーブルキー項目の登録.
                        arKey.Insert(0, Me.lblMente_No.Text)

                        '排他情報確認処理(更新処理の実行).
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


                            '登録年月日時刻.
                            Me.Master.ppExclusiveDate = strExclusiveDate
                        Else

                            '排他ロック中.
                            Exit Sub

                        End If

                        '--------------------------------
                        '2014/05/14 後藤　ここから
                        '--------------------------------
                        'ElseIf Me.btnUpdate.Text = "更新" Then
                    ElseIf Master.ppRigthButton1.Text = "更新" Then
                        '--------------------------------
                        '2014/05/14 後藤　ここまで
                        '--------------------------------

                        '整備作業依頼テーブル更新処理.
                        cmdDB = New SqlCommand(M_DISP_ID & "_U1", conDB)
                        cmdDB.Connection = conDB
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = conTrn
                        strMessage = "更新処理"
                        With cmdDB.Parameters
                            .Add(pfSet_Param("mente_no", SqlDbType.NVarChar, Me.lblMente_No.Text))
                            .Add(pfSet_Param("reqcomp_cd", SqlDbType.NVarChar, Me.ddlReqcomp_Cd.SelectedValue))
                            .Add(pfSet_Param("reqcomp_nm", SqlDbType.NVarChar, strArrayList(1).ToString))
                            .Add(pfSet_Param("appa_div", SqlDbType.NVarChar, Me.ddlRstAppaDiv.SelectedValue))
                            .Add(pfSet_Param("appa_cd", SqlDbType.NVarChar, Me.ddlAppa_Nm.SelectedValue))
                            .Add(pfSet_Param("appa_model", SqlDbType.NVarChar, Me.ddlRstAppaModel.SelectedValue))
                            .Add(pfSet_Param("appa_cnt", SqlDbType.NVarChar, Me.txtAppa_Cnt.ppText))
                            .Add(pfSet_Param("arrival_d", SqlDbType.NVarChar, Me.dtbArrval_D.ppText))
                            .Add(pfSet_Param("arrival_cnt", SqlDbType.NVarChar, Me.lblArrval_Cnt.Text))
                            .Add(pfSet_Param("send_nm", SqlDbType.NVarChar, Me.txtSend_Nm.ppText))
                            .Add(pfSet_Param("send_notetext", SqlDbType.NVarChar, Me.txtSend_NoteText.ppText))
                            .Add(pfSet_Param("wrkcls_cd", SqlDbType.NVarChar, Me.ddlWrkcls_cd.ppSelectedValue))
                            .Add(pfSet_Param("tboxclass_cd", SqlDbType.NVarChar, Me.ddlSystem.SelectedValue))
                            .Add(pfSet_Param("version", SqlDbType.NVarChar, Me.txtVersion.ppText))
                            .Add(pfSet_Param("req_cnt", SqlDbType.NVarChar, Me.lblReq_Cnt.Text))
                            .Add(pfSet_Param("work_notetext", SqlDbType.NVarChar, Me.txtWork_NoteText.ppText))
                            .Add(pfSet_Param("memcls", SqlDbType.NVarChar, Me.rdlMemCls.SelectedValue))
                            .Add(pfSet_Param("deliv", SqlDbType.NVarChar, Me.txtDeliv.ppText))
                            .Add(pfSet_Param("deliv_d", SqlDbType.NVarChar, mfGetDBNull(Me.dtbDeliv_D.ppText)))
                            .Add(pfSet_Param("deliv_cd", SqlDbType.NVarChar, Me.ddlDeliv_Cd.SelectedValue))
                            .Add(pfSet_Param("zip_no", SqlDbType.NVarChar, Me.txtZipNo.Text))
                            .Add(pfSet_Param("addr", SqlDbType.NVarChar, Me.txtAddr.Text))
                            .Add(pfSet_Param("telno", SqlDbType.NVarChar, Me.txtTel.Text))
                            .Add(pfSet_Param("faxno", SqlDbType.NVarChar, Me.txtFAX.Text))
                            .Add(pfSet_Param("ceriv_notetext", SqlDbType.NVarChar, Me.txtCeriv_NoteText.ppText))
                            .Add(pfSet_Param("mente_end_flg", SqlDbType.NVarChar, "0"))
                            .Add(pfSet_Param("order_no", SqlDbType.NVarChar, Me.txtOrder_No.ppText & Me.txtOrder_Seq.ppText))
                            .Add(pfSet_Param("delete_flg", SqlDbType.NVarChar, "0"))
                            .Add(pfSet_Param("update_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With

                        cmdDB.ExecuteNonQuery()
                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            Select Case strMessage
                                Case "登録処理"
                                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "整備依頼書")
                                Case "更新処理"
                                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "整備依頼書")
                            End Select

                            'ロールバック
                            conTrn.Rollback()

                            Exit Sub
                        End If

                        '整備作業依頼明細テーブル更新処理.
                        intOldAppa_Cnt = Integer.Parse(ViewState(P_REC_CNT))
                        intNewAppa_Cnt = Integer.Parse(Me.txtAppa_Cnt.ppText)
                        For intCnt As Integer = 1 To grvList.Rows.Count
                            cmdDB_M = New SqlCommand(M_DISP_ID & "_U2", conDB)
                            cmdDB_M.Connection = conDB
                            cmdDB_M.CommandType = CommandType.StoredProcedure
                            cmdDB_M.Transaction = conTrn
                            With cmdDB_M.Parameters
                                .Add(pfSet_Param("mente_no", SqlDbType.NVarChar, Me.lblMente_No.Text))
                                .Add(pfSet_Param("branch", SqlDbType.NVarChar, intCnt))
                                .Add(pfSet_Param("reqcomp_cd", SqlDbType.NVarChar, Me.ddlReqcomp_Cd.SelectedValue))
                                .Add(pfSet_Param("reqcomp_nm", SqlDbType.NVarChar, strArrayList(1).ToString))
                                .Add(pfSet_Param("appa_cd", SqlDbType.NVarChar, Me.ddlAppa_Nm.SelectedValue))
                                .Add(pfSet_Param("arrival_d", SqlDbType.NVarChar, Me.dtbArrval_D.ppText))
                                .Add(pfSet_Param("arrival_cnt", SqlDbType.NVarChar, Me.lblArrval_Cnt.Text))
                                .Add(pfSet_Param("send_nm", SqlDbType.NVarChar, Me.txtSend_Nm.ppText))
                                .Add(pfSet_Param("send_notetext", SqlDbType.NVarChar, Me.txtSend_NoteText.ppText))
                                .Add(pfSet_Param("wrkcls_cd", SqlDbType.NVarChar, Me.ddlWrkcls_cd.ppSelectedValue))
                                .Add(pfSet_Param("tboxclass_cd", SqlDbType.NVarChar, Me.ddlSystem.SelectedValue))
                                .Add(pfSet_Param("version", SqlDbType.NVarChar, Me.txtVersion.ppText))
                                .Add(pfSet_Param("req_cnt", SqlDbType.NVarChar, Me.lblReq_Cnt.Text))
                                .Add(pfSet_Param("work_notetext", SqlDbType.NVarChar, Me.txtWork_NoteText.ppText))
                                .Add(pfSet_Param("memcls", SqlDbType.NVarChar, Me.rdlMemCls.SelectedValue))
                                .Add(pfSet_Param("deliv", SqlDbType.NVarChar, Me.txtDeliv.ppText))
                                .Add(pfSet_Param("deliv_d", SqlDbType.NVarChar, mfGetDBNull(Me.dtbDeliv_D.ppText)))
                                .Add(pfSet_Param("update_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                            End With

                            cmdDB_M.ExecuteNonQuery()


                            'ストアド戻り値チェック
                            intRtn = Integer.Parse(cmdDB_M.Parameters("retvalue").Value.ToString)
                            If intRtn <> 0 Then
                                Select Case strMessage
                                    Case "登録処理"
                                        psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "整備依頼書")
                                    Case "更新処理"
                                        psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "整備依頼書")
                                End Select

                                'ロールバック
                                conTrn.Rollback()

                                Exit Sub
                            End If
                        Next


                        '整備作業依頼明細テーブル登録処理.
                        For intCnt As Integer = 1 To (intNewAppa_Cnt - intOldAppa_Cnt)
                            cmdDB_M = New SqlCommand(M_DISP_ID & "_I2", conDB)
                            cmdDB_M.Connection = conDB
                            cmdDB_M.CommandType = CommandType.StoredProcedure
                            cmdDB_M.Transaction = conTrn
                            With cmdDB_M.Parameters
                                .Add(pfSet_Param("mente_no", SqlDbType.NVarChar, Me.lblMente_No.Text))
                                .Add(pfSet_Param("branch", SqlDbType.NVarChar, Integer.Parse(grvList.Rows.Count) + intCnt))
                                .Add(pfSet_Param("reqcomp_cd", SqlDbType.NVarChar, Me.ddlReqcomp_Cd.SelectedValue))
                                .Add(pfSet_Param("reqcomp_nm", SqlDbType.NVarChar, strArrayList(1).ToString))
                                .Add(pfSet_Param("appa_cd", SqlDbType.NVarChar, Me.ddlAppa_Nm.SelectedValue))
                                .Add(pfSet_Param("arrival_d", SqlDbType.NVarChar, Me.dtbArrval_D.ppText))
                                .Add(pfSet_Param("arrival_cnt", SqlDbType.NVarChar, Me.lblArrval_Cnt.Text))
                                .Add(pfSet_Param("send_nm", SqlDbType.NVarChar, Me.txtSend_Nm.ppText))
                                .Add(pfSet_Param("send_notetext", SqlDbType.NVarChar, Me.txtSend_NoteText.ppText))
                                .Add(pfSet_Param("wrkcls_cd", SqlDbType.NVarChar, Me.ddlWrkcls_cd.ppSelectedValue))
                                .Add(pfSet_Param("tboxclass_cd", SqlDbType.NVarChar, Me.ddlSystem.SelectedValue))
                                .Add(pfSet_Param("version", SqlDbType.NVarChar, Me.txtVersion.ppText))
                                .Add(pfSet_Param("req_cnt", SqlDbType.NVarChar, Me.lblReq_Cnt.Text))
                                .Add(pfSet_Param("work_notetext", SqlDbType.NVarChar, Me.txtWork_NoteText.ppText))
                                .Add(pfSet_Param("memcls", SqlDbType.NVarChar, Me.rdlMemCls.SelectedValue))
                                .Add(pfSet_Param("deliv", SqlDbType.NVarChar, Me.txtDeliv.ppText))
                                .Add(pfSet_Param("deliv_d", SqlDbType.NVarChar, mfGetDBNull(Me.dtbDeliv_D.ppText)))
                                .Add(pfSet_Param("mente_end_flg", SqlDbType.NVarChar, "0"))
                                .Add(pfSet_Param("insert_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                            End With

                            cmdDB_M.ExecuteNonQuery()


                            'ストアド戻り値チェック
                            intRtn = Integer.Parse(cmdDB_M.Parameters("retvalue").Value.ToString)
                            If intRtn <> 0 Then
                                Select Case strMessage
                                    Case "登録処理"
                                        psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "整備依頼書")
                                    Case "更新処理"
                                        psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "整備依頼書")
                                End Select

                                'ロールバック
                                conTrn.Rollback()

                                Exit Sub
                            End If

                        Next

                    End If

                    'コミット
                    conTrn.Commit()

                    '完了メッセージ
                    Select Case strMessage
                        Case "登録処理"
                            psMesBox(Me, "00003", clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後, "整備依頼書")
                        Case "更新処理"
                            psMesBox(Me, "00001", clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後, "整備依頼書")
                    End Select

                    '更新前の機器種別台数保管.
                    ViewState(P_APPA_CNT) = Me.txtAppa_Cnt.ppText

                    '更新前の注文番号保管.
                    ViewState(P_ORDER_NO) = Me.txtOrder_No.ppText & Me.txtOrder_Seq.ppText

                    Me.ddlRstAppaDiv.Enabled = False
                    Me.ddlAppa_Nm.Enabled = False
                    Me.ddlSystem.Enabled = False
                    Me.ddlRstAppaModel.Enabled = False

                    '画面状態更新.
                    Call msEnableScreen(ClsComVer.E_遷移条件.更新)

                    '再検索.
                    Call msSetList_Data(Me.lblMente_No.Text)

                    '再設定
                    Call msSet_ButtonAction()

                End If

            Catch ex As Exception

                If Not conTrn Is Nothing Then
                    conTrn.Rollback()
                End If

                Select Case strMessage
                    Case "登録処理"
                        psMesBox(Me, "00003", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "整備依頼書")
                    Case "更新処理"
                        psMesBox(Me, "00001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "整備依頼書")
                End Select
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
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim cmdDB_M As SqlCommand = Nothing
        Dim cmdDB_BM As SqlCommand = Nothing
        Dim dsCheck As DataSet
        Dim drCheck() As DataRow
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
                cmdDB = New SqlCommand(M_DISP_ID & "_S3", conDB)
                cmdDB.Parameters.Add(pfSet_Param("mente_no", SqlDbType.NVarChar, Me.lblMente_No.Text))
                dsCheck = clsDataConnect.pfGet_DataSet(cmdDB)
                drCheck = dsCheck.Tables(0).Select("D232_PRGSTATUS_CD = '03' AND 請求日付 < '" & System.DateTime.Now.ToString("yyyyMM") & "' ")
                If drCheck.Length > 0 Then
                    psMesBox(Me, "30010", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後, "既に完了した明細", "整備依頼書の削除を行うこと")
                    Exit Sub
                End If



                'トランザクション.
                conTrn = conDB.BeginTransaction

                '整備作業依頼テーブル削除処理.
                cmdDB = New SqlCommand(M_DISP_ID & "_D1", conDB)
                cmdDB.Connection = conDB
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.Transaction = conTrn
                With cmdDB.Parameters
                    .Add(pfSet_Param("mente_no", SqlDbType.NVarChar, Me.lblMente_No.Text))
                    .Add(pfSet_Param("delete_flg", SqlDbType.NVarChar, "1"))
                    .Add(pfSet_Param("update_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                End With

                cmdDB.ExecuteNonQuery()

                'ストアド戻り値チェック.
                intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                If intRtn <> 0 Then
                    psMesBox(Me, "00002", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "整備依頼書")
                    conTrn.Rollback()
                    Exit Sub
                End If

                '整備作業依頼明細テーブル削除処理.
                cmdDB_M = New SqlCommand(M_DISP_ID & "_D2", conDB)
                cmdDB_M.Connection = conDB
                cmdDB_M.CommandType = CommandType.StoredProcedure
                cmdDB_M.Transaction = conTrn
                With cmdDB_M.Parameters
                    .Add(pfSet_Param("mente_no", SqlDbType.NVarChar, Me.lblMente_No.Text))
                    .Add(pfSet_Param("delete_flg", SqlDbType.NVarChar, "1"))
                    .Add(pfSet_Param("update_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                End With

                cmdDB_M.ExecuteNonQuery()

                'ストアド戻り値チェック.
                intRtn = Integer.Parse(cmdDB_M.Parameters("retvalue").Value.ToString)
                If intRtn <> 0 Then
                    psMesBox(Me, "00002", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "整備依頼書")
                    conTrn.Rollback()
                    Exit Sub
                End If


                '整備作業依頼部品明細テーブル削除処理.
                cmdDB_BM = New SqlCommand(M_DISP_ID & "_D3", conDB)
                cmdDB_BM.Connection = conDB
                cmdDB_BM.CommandType = CommandType.StoredProcedure
                cmdDB_BM.Transaction = conTrn
                With cmdDB_BM.Parameters
                    .Add(pfSet_Param("mente_no", SqlDbType.NVarChar, Me.lblMente_No.Text))
                    .Add(pfSet_Param("delete_flg", SqlDbType.NVarChar, "1"))
                    .Add(pfSet_Param("update_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                End With

                cmdDB_BM.ExecuteNonQuery()

                'ストアド戻り値チェック.
                intRtn = Integer.Parse(cmdDB_BM.Parameters("retvalue").Value.ToString)
                If intRtn <> 0 Then
                    psMesBox(Me, "00002", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "整備依頼書")
                    conTrn.Rollback()
                    Exit Sub
                End If

                'コミット
                conTrn.Commit()

                '完了メッセージ.
                psMesBox(Me, "00002", clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後, "整備依頼書")

                '活性制御.
                '--------------------------------
                '2014/05/14 後藤　ここから
                '--------------------------------
                'Me.pnlMente.Enabled = False
                'Me.btnPrint.Enabled = False
                'Me.btnClear.Enabled = False
                'Me.btnDelete.Enabled = False
                'Me.btnUpdate.Enabled = False


                '--------------------------------
                '2014/05/14 後藤　ここまで
                '--------------------------------

                'コントロール初期化.
                Call msClearScreen()

                ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録
                '活性制御
                Call msEnableScreen(ClsComVer.E_遷移条件.登録)
            End If

        Catch ex As Exception

            If Not conTrn Is Nothing Then
                conTrn.Rollback()
            End If

            psMesBox(Me, "00002", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "整備依頼書")
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
    ''' クリアボタン押下処理．
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim strTerms As String
        Dim blnClearFlg As Boolean = False
        strTerms = ViewState(P_SESSION_TERMS)

        'Select Case strTerms.ToString.Trim
        '    Case ClsComVer.E_遷移条件.更新

        '        '整備依頼書データ再取得処理.
        '        Call msGet_Data(Me.lblMente_No.Text)

        '    Case ClsComVer.E_遷移条件.登録

        '        'コントロール初期化.
        '        Call msClearScreen(blnClearFlg)

        'End Select
        Select Case sender.text.ToString()
            Case "元に戻す"

                '整備依頼書データ再取得処理.
                Call msGet_Data(Me.lblMente_No.Text)

            Case "クリア"
                'コントロール初期化.
                Call msClearScreen(blnClearFlg)

        End Select

    End Sub
    ''' <summary>
    ''' 印刷ボタン押下処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim dstDestination As New DataSet
        Dim dtPrint As New DataTable                     '帳票用データセット
        Dim dtRow As DataRow = dtPrint.NewRow()          'データテーブルの行定義
        Dim strMessage As String = String.Empty
        Dim strUserNm As String = String.Empty
        Dim strOfficeNm As String = String.Empty
        Dim strDeliv As String = String.Empty

        Dim rpt As MNTREP001
        Dim strRptNm As String = "MNTREP001"
        Dim intRetValue As Integer = 0

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'データテーブルの項目名セット       
        dtPrint.Columns.Add("発行日")
        dtPrint.Columns.Add("担当者")
        dtPrint.Columns.Add("送付先会社名")
        dtPrint.Columns.Add("送付先事業部")
        dtPrint.Columns.Add("管理番号")
        dtPrint.Columns.Add("機種名")
        dtPrint.Columns.Add("台数")
        dtPrint.Columns.Add("到着日")
        dtPrint.Columns.Add("到着台数")
        dtPrint.Columns.Add("送付先")
        dtPrint.Columns.Add("送付先備考")
        dtPrint.Columns.Add("作業種別")
        dtPrint.Columns.Add("システム")
        dtPrint.Columns.Add("バージョン設定")
        dtPrint.Columns.Add("依頼台数")
        dtPrint.Columns.Add("作業依頼備考")
        dtPrint.Columns.Add("故障区分1")
        dtPrint.Columns.Add("故障区分2")
        dtPrint.Columns.Add("故障区分3")
        dtPrint.Columns.Add("納入希望日")
        dtPrint.Columns.Add("納入先郵便番号")
        dtPrint.Columns.Add("納入先住所")
        dtPrint.Columns.Add("納入先名")
        dtPrint.Columns.Add("納入先TEL")
        dtPrint.Columns.Add("納入先FAX")
        dtPrint.Columns.Add("納入先備考")
        'MNTUPDP002-001
        dtPrint.Columns.Add("送付元会社")
        dtPrint.Columns.Add("送付元部署")
        'MNTUPDP002-001 END
        Try
            '開始ログ出力
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            Else
                '整備依頼データ取得.
                cmdDB = New SqlCommand(M_DISP_ID & "_S4", conDB)
                cmdDB.Parameters.Add(pfSet_Param("mente_no", SqlDbType.NVarChar, lblMente_No.Text))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'データがない場合は印刷しない
                If dstOrders.Tables(0).Rows.Count = 0 Then
                    psMesBox(Me, "00008", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後)
                    Return
                End If
            End If

            '納入希望日設定.

            If dstOrders.Tables(0).Rows(0).Item("納入希望").ToString() <> String.Empty Then
                strDeliv = dstOrders.Tables(0).Rows(0).Item("納入希望").ToString()
            Else
                strDeliv = mfChange_Jp(dstOrders.Tables(0).Rows(0).Item("納入希望日").ToString(), "2")
            End If


            'データテーブルの列に値を設定
            dtRow("発行日") = mfChange_Jp(DateTime.Now.ToString("yyyy/MM/dd"), "1")
            dtRow("担当者") = dstOrders.Tables(0).Rows(0).Item("担当者").ToString()
            dtRow("送付先会社名") = dstOrders.Tables(0).Rows(0).Item("依頼先").ToString()
            dtRow("送付先事業部") = dstOrders.Tables(0).Rows(0).Item("送付先事業部").ToString()
            dtRow("管理番号") = dstOrders.Tables(0).Rows(0).Item("管理番号").ToString()
            dtRow("機種名") = dstOrders.Tables(0).Rows(0).Item("機種名").ToString()
            dtRow("台数") = dstOrders.Tables(0).Rows(0).Item("台数").ToString()
            dtRow("到着日") = mfChange_Jp(dstOrders.Tables(0).Rows(0).Item("到着日").ToString, "2")
            dtRow("到着台数") = dstOrders.Tables(0).Rows(0).Item("到着台数").ToString()
            dtRow("送付先") = dstOrders.Tables(0).Rows(0).Item("送付先").ToString()
            dtRow("送付先備考") = dstOrders.Tables(0).Rows(0).Item("送付先備考").ToString()
            dtRow("作業種別") = dstOrders.Tables(0).Rows(0).Item("作業種別").ToString()
            dtRow("システム") = dstOrders.Tables(0).Rows(0).Item("システム").ToString()
            dtRow("バージョン設定") = dstOrders.Tables(0).Rows(0).Item("バージョン設定").ToString()
            dtRow("依頼台数") = dstOrders.Tables(0).Rows(0).Item("依頼台数").ToString()
            dtRow("作業依頼備考") = dstOrders.Tables(0).Rows(0).Item("作業依頼備考").ToString()
            dtRow("故障区分1") = dstOrders.Tables(0).Rows(0).Item("故障区分1").ToString()
            dtRow("故障区分2") = dstOrders.Tables(0).Rows(0).Item("故障区分2").ToString()
            dtRow("故障区分3") = dstOrders.Tables(0).Rows(0).Item("故障区分3").ToString()
            dtRow("納入希望日") = strDeliv
            dtRow("納入先郵便番号") = dstOrders.Tables(0).Rows(0).Item("納入先郵便番号").ToString()
            dtRow("納入先住所") = dstOrders.Tables(0).Rows(0).Item("納入先住所").ToString()
            dtRow("納入先名") = dstOrders.Tables(0).Rows(0).Item("納入先名").ToString()
            dtRow("納入先TEL") = dstOrders.Tables(0).Rows(0).Item("納入先TEL").ToString()
            dtRow("納入先FAX") = dstOrders.Tables(0).Rows(0).Item("納入先FAX").ToString()
            dtRow("納入先備考") = dstOrders.Tables(0).Rows(0).Item("納入先備考").ToString()

            'MNTUPDP002-001
            dtRow("送付元会社") = dstOrders.Tables(0).Rows(0).Item("送付元会社").ToString()
            dtRow("送付元部署") = dstOrders.Tables(0).Rows(0).Item("送付元部署").ToString()
            'MNTUPDP002-001 END

            'データテーブルに作成したデータを設定
            dtPrint.Rows.Add(dtRow)

            'Active Reports帳票の起動

            '--------------------------------
            '2014/05/14 後藤　ここから
            '--------------------------------

            'Try
            rpt = New MNTREP001
            psPrintPDF(Me, rpt, dtPrint, "整備依頼書")

        Catch ex As SqlException
            '帳票情報の取得に失敗
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "整備依頼書")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Catch ex As Exception
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "整備依頼書")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub
    ''' <summary>
    ''' 一覧の選択ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet

        '参照・更新ボタン以外は処理しない.
        If e.CommandName.Trim <> "btnReference" And e.CommandName.Trim <> "btnUpdate" Then
            Exit Sub
        End If

        Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    ' ボタン押下行のIndex
        Dim rowData As GridViewRow = grvList.Rows(intIndex)             ' ボタン押下行
        Dim strKeyList As List(Of String) = Nothing                     '次画面引継ぎ用キー情報

        '排他制御用の変数定義.
        Dim strExclusiveDate As String = String.Empty
        Dim arTable_Name As New ArrayList
        Dim arKey As New ArrayList
        Dim strBtnFlag As String = String.Empty


        '接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
        Else
            Try
                If e.CommandName = "btnUpdate" Then
                    '修正依頼データ取得.
                    cmdDB = New SqlCommand(P_FUN_MNT & P_SCR_SEL & P_PAGE & "001_S1", conDB)
                    cmdDB.Parameters.Add(pfSet_Param("mente_no", SqlDbType.NVarChar, Me.lblMente_No.Text))
                    cmdDB.Parameters.Add(pfSet_Param("branch", SqlDbType.NVarChar, CType(rowData.FindControl("枝番"), TextBox).Text))
                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                    If dstOrders.Tables(0).Rows.Count = 0 Then
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        Exit Sub
                    ElseIf dstOrders.Tables(0).Rows(0).Item("削除フラグ").ToString = "1" Then
                        psMesBox(Me, "30011", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "削除")
                        Exit Sub
                    End If
                End If

                '次画面引継ぎ用キー情報設定.
                strKeyList = New List(Of String)
                strKeyList.Add(Me.lblMente_No.Text)
                strKeyList.Add(CType(rowData.FindControl("枝番"), TextBox).Text)

                'セッション情報設定
                Session(P_SESSION_BCLIST) = Master.ppBcList_Text 'パンくずリスト
                Session(P_KEY) = strKeyList.ToArray                     'キー項目(管理番号,枝番)
                Session(P_SESSION_OLDDISP) = M_DISP_ID                  '遷移元の画面ＩＤ
                Select Case e.CommandName
                    Case "btnReference"     ' 参照
                        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
                    Case "btnUpdate"        ' 更新
                        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
                End Select

                '遷移条件取得.
                strBtnFlag = Session(P_SESSION_TERMS)

                'セッション情報設定.
                If strBtnFlag = ClsComVer.E_遷移条件.更新 Then

                    'ロック対象テーブル名の登録.
                    arTable_Name.Insert(0, "D232_MENTE_REQUEST_DTL")
                    arTable_Name.Insert(1, "D82_MENTEPARTS_DTIL")

                    'ロックテーブルキー項目の登録.
                    arKey.Insert(0, Me.lblMente_No.Text)
                    arKey.Insert(1, CType(rowData.FindControl("枝番"), TextBox).Text)
                    arKey.Insert(2, Me.lblMente_No.Text)
                    arKey.Insert(3, CType(rowData.FindControl("枝番"), TextBox).Text)

                    '排他情報確認処理(更新処理の実行).
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
                        Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate
                    Else

                        '排他ロック中.
                        Exit Sub

                    End If

                End If

                '排他情報のグループ番号をセッション変数に設定.
                Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)


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
                                objStack.GetMethod.Name, M_MNTSELP001, strPrm, "TRANS")

                '別ブラウザ起動
                psOpen_Window(Me, M_MNTSELP001)
            Catch ex As Exception
                '帳票の出力に失敗
                psMesBox(Me, "10001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "整備依頼書")

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
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
    ''' 依頼先の検証(必須入力チェック).
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvReqcomp_Cd_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvReqcomp_Cd.ServerValidate
        Dim dtrMes As DataRow
        args.IsValid = True
        If ddlReqcomp_Cd.SelectedIndex = 0 Then '未選択
            dtrMes = pfGet_ValMes("5001", "依頼先")
            cuvReqcomp_Cd.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            cuvReqcomp_Cd.Text = dtrMes.Item(P_VALMES_SMES)
            args.IsValid = False
        End If
    End Sub

    ''' <summary>
    ''' 機種名の検証(必須入力チェック).
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvRstAppaDiv_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles valRstAppaDiv.ServerValidate
        Dim dtrMes As DataRow
        args.IsValid = True
        If ddlRstAppaDiv.Enabled = False Then
            Exit Sub
        End If
        If ddlRstAppaDiv.SelectedIndex = 0 Then '未選択
            dtrMes = pfGet_ValMes("5001", "機種名")
            valRstAppaDiv.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            valRstAppaDiv.Text = dtrMes.Item(P_VALMES_SMES)
            args.IsValid = False
        End If
    End Sub

    ''' <summary>
    ''' 機種名の検証(必須入力チェック).
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub valRstAppaModel_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles valRstAppaModel.ServerValidate
        Dim dtrMes As DataRow
        args.IsValid = True
        If ddlRstAppaModel.Enabled = False Then
            Exit Sub
        End If
        If ddlRstAppaModel.SelectedIndex = 0 Then '未選択
            dtrMes = pfGet_ValMes("5001", "機種名")
            valRstAppaModel.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            valRstAppaModel.Text = dtrMes.Item(P_VALMES_SMES)
            args.IsValid = False
        End If
    End Sub

    ''' <summary>
    ''' システムの検証(必須入力チェック).
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvSystem_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvSystem.ServerValidate
        Dim dtrMes As DataRow
        args.IsValid = True
        If ddlSystem.Enabled = False Then
            Exit Sub
        End If
        If ddlSystem.SelectedIndex = 0 Then '未選択
            dtrMes = pfGet_ValMes("5001", "システム")
            cuvSystem.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            cuvSystem.Text = dtrMes.Item(P_VALMES_SMES)
            args.IsValid = False
        End If
    End Sub
    ''' <summary>
    ''' 機種名の検証(必須入力チェック).
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvAppa_Nm_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvAppa_Nm.ServerValidate
        Dim dtrMes As DataRow
        args.IsValid = True
        If ddlAppa_Nm.Enabled = False Then
            Exit Sub
        End If
        If ddlAppa_Nm.SelectedIndex = 0 Then '未選択
            dtrMes = pfGet_ValMes("5001", "機種名")
            cuvAppa_Nm.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            cuvAppa_Nm.Text = dtrMes.Item(P_VALMES_SMES)
            args.IsValid = False
        End If
    End Sub
    ''' <summary>
    ''' 対応方法の検証(必須入力チェック).
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    ''' 
    Protected Sub cuvMemCls_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvMemCls.ServerValidate
        Dim dtrMes As DataRow
        args.IsValid = True
        If rdlMemCls.SelectedValue = "" Then '未選択
            dtrMes = pfGet_ValMes("5001", "対応方法")
            cuvMemCls.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            cuvMemCls.Text = dtrMes.Item(P_VALMES_SMES)
            args.IsValid = False
        End If
    End Sub
    ''' <summary>
    ''' 納入先の検証(必須入力チェック).
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvCeriv_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvDeliv_Cd.ServerValidate
        Dim dtrMes As DataRow
        args.IsValid = True
        If ddlDeliv_Cd.SelectedIndex = 0 Then '未選択
            dtrMes = pfGet_ValMes("5001", "納入先")
            cuvDeliv_Cd.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            cuvDeliv_Cd.Text = dtrMes.Item(P_VALMES_SMES)
            args.IsValid = False
        End If
    End Sub


#End Region

#Region "そのほかのプロシージャ"
    ''' <summary>
    ''' 初期化処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen(Optional ByVal blnClearFlg As Boolean = True)

        'コントロールの初期化 .
        Me.ddlReqcomp_Cd.SelectedIndex = -1
        Me.lblMente_No.Text = String.Empty
        Me.txtOrder_No.ppText = String.Empty
        Me.txtOrder_Seq.ppText = String.Empty
        Me.ddlRstAppaDiv.SelectedIndex = -1
        Me.ddlAppa_Nm.SelectedIndex = -1
        Me.ddlSystem.SelectedIndex = -1
        Me.ddlRstAppaModel.SelectedIndex = -1
        Me.txtAppa_Cnt.ppText = String.Empty
        Me.dtbArrval_D.ppText = String.Empty
        Me.lblArrval_Cnt.Text = String.Empty
        Me.txtSend_Nm.ppText = String.Empty
        Me.txtSend_NoteText.ppText = String.Empty
        Me.ddlWrkcls_cd.ppDropDownList.SelectedIndex = -1
        Me.lblSystem.Text = String.Empty
        Me.txtVersion.ppText = String.Empty
        Me.lblReq_Cnt.Text = String.Empty
        Me.txtWork_NoteText.ppText = String.Empty
        Me.rdlMemCls.SelectedIndex = -1
        Me.dtbDeliv_D.ppText = String.Empty
        Me.txtDeliv.ppText = String.Empty
        Me.ddlDeliv_Cd.SelectedIndex = -1
        Me.txtZipNo.Text = String.Empty
        Me.txtAddr.Text = String.Empty
        Me.txtTel.Text = String.Empty
        Me.txtFAX.Text = String.Empty
        Me.txtCeriv_NoteText.ppText = String.Empty

        '初回のみ初期化.
        If blnClearFlg = True Then
            'グリッドビューの初期化.
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

            'ドロップダウンリスト生成.
            If Not mfGet_DropListData_Sel() Then
                '画面を終了
                psMesBox(Me, "30001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画前)
                '--------------------------------
                '2014/06/11 後藤　ここから
                '--------------------------------
                '排他削除
                clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                '--------------------------------
                '2014/06/11 後藤　ここまで
                '--------------------------------
                psClose_Window(Me)
            End If
        End If

    End Sub
    ''' <summary>
    ''' ボタンアクションの設定.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_ButtonAction()

        '--------------------------------
        '2014/05/14 後藤　ここから
        '--------------------------------
        'AddHandler Me.btnPrint.Click, AddressOf btnPrint_Click
        'AddHandler Me.btnClear.Click, AddressOf btnClear_Click
        'AddHandler Me.btnUpdate.Click, AddressOf btnUpdate_Click
        'AddHandler Me.btnDelete.Click, AddressOf btnDelete_Click
        AddHandler Master.ppRigthButton1.Click, AddressOf btnUpdate_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnDelete_Click
        AddHandler Master.ppRigthButton3.Click, AddressOf btnClear_Click
        AddHandler Master.ppLeftButton1.Click, AddressOf btnPrint_Click
        '--------------------------------
        '2014/05/14 後藤　ここまで
        '--------------------------------

        AddHandler Me.ddlReqcomp_Cd.TextChanged, AddressOf ddlReqcomp_Cd_TextChanged
        AddHandler Me.ddlDeliv_Cd.TextChanged, AddressOf ddlDeliv_Cd_TextChanged
        AddHandler Me.ddlSystem.TextChanged, AddressOf ddlSystem_TextChanged
        AddHandler Me.txtAppa_Cnt.ppTextBox.TextChanged, AddressOf txtAppa_Cnt_TextChanged
        Me.ddlReqcomp_Cd.AutoPostBack = True
        Me.ddlDeliv_Cd.AutoPostBack = True
        Me.ddlSystem.AutoPostBack = True
        Me.txtAppa_Cnt.ppTextBox.AutoPostBack = True

        '確認メッセージ設定.
        '--------------------------------
        '2014/05/14 後藤　ここから
        '--------------------------------
        'Me.btnPrint.OnClientClick = pfGet_OCClickMes("10002", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "整備依頼書")
        'Me.btnDelete.OnClientClick = pfGet_OCClickMes("00005", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "整備依頼書")

        'If Me.btnUpdate.Text = "更新" Then
        '    Me.btnUpdate.OnClientClick = pfGet_OCClickMes("00004", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "整備依頼書")
        'Else
        '    Me.btnUpdate.OnClientClick = pfGet_OCClickMes("00006", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "整備依頼書")
        'End If

        Master.ppLeftButton1.OnClientClick = pfGet_OCClickMes("10002", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "整備依頼書")
        Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("00005", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "整備依頼書")

        If Master.ppRigthButton1.Text = "更新" Then
            Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("00004", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "整備依頼書")
        Else
            Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("00006", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "整備依頼書")
        End If
        '--------------------------------
        '2014/05/14 後藤　ここまで
        '--------------------------------
    End Sub
    ''' <summary>
    ''' 整備依頼書取得処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data(ByVal ipstrMente_No As String)

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

        Try

            '開始ログ出力.
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            Else

                '整備依頼書取得.
                cmdDB = New SqlCommand(M_DISP_ID & "_S2", conDB)
                cmdDB.Parameters.Add(pfSet_Param("mente_no", SqlDbType.NVarChar, ipstrMente_No))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得したデータをコントロールに設定.
                Call msSetDisp_Data(dstOrders)
                Me.ddlRstAppaDiv.Enabled = False
                Me.ddlAppa_Nm.Enabled = False
                Me.ddlSystem.Enabled = False
                Me.ddlRstAppaModel.Enabled = False

            End If

        Catch ex As Exception
            psMesBox(Me, "00004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "整備依頼書データ")
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
    ''' 取得データ設定処理.
    ''' </summary>
    ''' <param name="dstOrders"></param>
    ''' <remarks></remarks>
    Private Sub msSetDisp_Data(ByVal dstOrders As DataSet)

        Me.ddlReqcomp_Cd.SelectedValue = dstOrders.Tables(0).Rows(0).Item("依頼先会社コード").ToString()
        Me.lblMente_No.Text = dstOrders.Tables(0).Rows(0).Item("管理番号").ToString()
        Me.txtOrder_No.ppText = dstOrders.Tables(0).Rows(0).Item("注文番号").ToString.Substring(0, 4)
        Me.txtOrder_Seq.ppText = dstOrders.Tables(0).Rows(0).Item("注文番号").ToString.Substring(4)
        Me.ddlRstAppaDiv.SelectedValue = dstOrders.Tables(0).Rows(0).Item("機器分類").ToString()
        Me.ddlAppa_Nm.SelectedValue = dstOrders.Tables(0).Rows(0).Item("機種コード").ToString()
        Me.txtAppa_Cnt.ppText = dstOrders.Tables(0).Rows(0).Item("台数").ToString()
        Me.dtbArrval_D.ppText = dstOrders.Tables(0).Rows(0).Item("到着日").ToString()
        Me.lblArrval_Cnt.Text = dstOrders.Tables(0).Rows(0).Item("到着台数").ToString()
        Me.txtSend_Nm.ppText = dstOrders.Tables(0).Rows(0).Item("送付先").ToString()
        Me.txtSend_NoteText.ppText = dstOrders.Tables(0).Rows(0).Item("備考").ToString()
        Me.ddlWrkcls_cd.ppSelectedValue = dstOrders.Tables(0).Rows(0).Item("作業種別").ToString()
        Me.ddlSystem.SelectedValue = dstOrders.Tables(0).Rows(0).Item("システム").ToString()

        Me.lblSystem.Text = dstOrders.Tables(0).Rows(0).Item("TBOX機種名").ToString()

        Me.txtVersion.ppText = dstOrders.Tables(0).Rows(0).Item("VER設定").ToString()
        Me.lblReq_Cnt.Text = dstOrders.Tables(0).Rows(0).Item("依頼台数").ToString()
        Me.txtWork_NoteText.ppText = dstOrders.Tables(0).Rows(0).Item("作業依頼備考").ToString()
        Me.rdlMemCls.SelectedValue = dstOrders.Tables(0).Rows(0).Item("対応方法").ToString()
        Me.dtbDeliv_D.ppText = dstOrders.Tables(0).Rows(0).Item("納入希望日").ToString()
        Me.txtDeliv.ppText = dstOrders.Tables(0).Rows(0).Item("納入希望").ToString()
        'MNTUPDP002-001 
        'Me.ddlDeliv_Cd.SelectedValue = dstOrders.Tables(0).Rows(0).Item("納入先コード").ToString()
        Select Case dstOrders.Tables(0).Rows(0).Item("納入先コード").ToString()
            Case OLD_BW_COD
                Me.ddlDeliv_Cd.SelectedValue = NEW_BW_COD
            Case OLD_FS_COD
                Me.ddlDeliv_Cd.SelectedValue = NEW_FS_COD
            Case Else
                Me.ddlDeliv_Cd.SelectedValue = dstOrders.Tables(0).Rows(0).Item("納入先コード").ToString()
        End Select
        'MNTUPDP002-001 END

        Me.txtZipNo.Text = dstOrders.Tables(0).Rows(0).Item("郵便番号").ToString()
        Me.txtAddr.Text = dstOrders.Tables(0).Rows(0).Item("住所").ToString()
        Me.txtTel.Text = dstOrders.Tables(0).Rows(0).Item("TEL").ToString()
        Me.txtFAX.Text = dstOrders.Tables(0).Rows(0).Item("FAX").ToString()
        Me.txtCeriv_NoteText.ppText = dstOrders.Tables(0).Rows(0).Item("納入備考").ToString()
        If dstOrders.Tables(0).Rows(0).Item("削除").ToString() <> "" Then
            intDel = Integer.Parse(dstOrders.Tables(0).Rows(0).Item("削除").ToString())
        End If

        msSetddlAppamodel()
        Me.ddlRstAppaModel.SelectedValue = dstOrders.Tables(0).Rows(0).Item("型式機器").ToString()

    End Sub
    ''' <summary>
    ''' 活性制御.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msEnableScreen(ByVal strTerms As String)


        Select Case strTerms.ToString.Trim
            Case ClsComVer.E_遷移条件.更新

                Me.pnlMente.Enabled = True
                Me.ddlReqcomp_Cd.Enabled = False
                '--------------------------------
                '2014/05/14 後藤　ここから
                '--------------------------------
                'Me.btnClear.Text = "元に戻す"
                'Me.btnUpdate.Text = "更新"
                'Me.btnPrint.Enabled = True
                'Me.btnClear.Enabled = True
                'Me.btnDelete.Enabled = True
                'Me.btnUpdate.Enabled = True

                Master.ppRigthButton1.Text = "更新"
                Master.ppRigthButton3.Text = "元に戻す"
                Master.ppRigthButton1.Enabled = True
                Master.ppRigthButton2.Enabled = True
                Master.ppRigthButton3.Enabled = True
                Master.ppLeftButton1.Enabled = True
                '--------------------------------
                '2014/05/14 後藤　ここまで
                '--------------------------------

            Case ClsComVer.E_遷移条件.参照

                Me.pnlMente.Enabled = False
                Me.ddlReqcomp_Cd.Enabled = False

                '--------------------------------
                '2014/05/14 後藤　ここから
                '--------------------------------
                'Me.btnClear.Text = "元に戻す"
                'Me.btnUpdate.Text = "更新"
                'Me.btnPrint.Enabled = True
                'Me.btnClear.Enabled = False
                'Me.btnDelete.Enabled = False
                'Me.btnUpdate.Enabled = False

                Master.ppRigthButton1.Text = "更新"
                Master.ppRigthButton3.Text = "元に戻す"
                Master.ppRigthButton1.Enabled = False
                Master.ppRigthButton2.Enabled = False
                Master.ppRigthButton3.Enabled = False
                Master.ppLeftButton1.Enabled = True
                If intDel = 1 Then
                    Master.ppLeftButton1.Enabled = False
                End If
                '--------------------------------
                '2014/05/14 後藤　ここまで
                '--------------------------------
                '--------------------------------
                '2014/05/15 後藤　ここから
                '--------------------------------
                '更新ボタン非活性
                For Each objRow As GridViewRow In grvList.Rows
                    objRow.Cells(1).Enabled = False
                Next
                '--------------------------------
                '2014/05/15 後藤　ここまで
                '--------------------------------

            Case ClsComVer.E_遷移条件.登録

                Me.pnlMente.Enabled = True
                Me.ddlReqcomp_Cd.Enabled = True

                '--------------------------------
                '2014/05/14 後藤　ここから
                '--------------------------------
                'Me.btnClear.Text = "クリア"
                'Me.btnUpdate.Text = "登録"
                'Me.btnPrint.Enabled = False
                'Me.btnClear.Enabled = True
                'Me.btnDelete.Enabled = False
                'Me.btnUpdate.Enabled = True

                Master.ppRigthButton1.Text = "登録"
                Master.ppRigthButton3.Text = "クリア"
                Master.ppRigthButton1.Enabled = True
                Master.ppRigthButton2.Enabled = False
                Master.ppRigthButton3.Enabled = True
                Master.ppLeftButton1.Enabled = False
                '--------------------------------
                '2014/05/14 後藤　ここまで
                '--------------------------------

            Case "削除"
                Me.pnlMente.Enabled = False
                Master.ppRigthButton1.Enabled = False
                Master.ppRigthButton2.Enabled = False
                Master.ppRigthButton3.Enabled = False
                Master.ppLeftButton1.Enabled = False

        End Select

    End Sub
    ''' <summary>
    ''' 整備依頼書（整備機器一覧）データ取得処理.
    ''' </summary>
    ''' <param name="ipstrMente_No"></param>
    ''' <remarks></remarks>
    Private Sub msSetList_Data(ByVal ipstrMente_No As String)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet
        Dim drOrders() As DataRow
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
        Else
            Try
                cmdDB = New SqlCommand(M_DISP_ID & "_S3", conDB)
                cmdDB.Parameters.Add(pfSet_Param("mente_no", SqlDbType.NVarChar, ipstrMente_No))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得したデータをリストに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

                drOrders = dstOrders.Tables(0).Select("削除区分 <> '●'")
                ViewState(P_REC_CNT) = drOrders.Length

            Catch ex As Exception
                psMesBox(Me, "00004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "整備依頼書データ")
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
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                End If
            End Try
        End If
    End Sub

    ''' <summary>
    ''' データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        For Each rowData As GridViewRow In grvList.Rows
            If CType(rowData.FindControl("削除区分"), TextBox).Text = "●" Then
                rowData.Cells(1).Enabled = False
                CType(rowData.FindControl("機器名"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("一時診断結果"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("診断結果送付日"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("ステータス"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("完了発送日"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("シリアルNo"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("受領日"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("削除区分"), TextBox).ForeColor = Drawing.Color.Red
            End If
        Next
    End Sub

    ''' <summary>
    ''' 個別エラーチェック.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheck_Error()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim drOrders() As DataRow
        Dim dstReq As New DataSet
        Dim strErr As String
        Dim strTerms As String
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
            Else
                'チェック時に明細の再取得を行う
                cmdDB = New SqlCommand(M_DISP_ID & "_S3", conDB)
                cmdDB.Parameters.Add(pfSet_Param("mente_no", SqlDbType.NVarChar, Me.lblMente_No.Text))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得したデータをリストに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

                drOrders = dstOrders.Tables(0).Select("削除区分 <> '●'")
                ViewState(P_REC_CNT) = drOrders.Length

                '機器種別台数整合性チェック.
                If Integer.TryParse(Me.txtAppa_Cnt.ppText, 0) = True Then
                    If Me.txtAppa_Cnt.ppText < ViewState(P_REC_CNT) Then
                        'If Me.txtAppa_Cnt.ppText = ViewState(P_APPA_CNT) OrElse Me.txtAppa_Cnt.ppText < ViewState(P_APPA_CNT) Then
                        Me.txtAppa_Cnt.psSet_ErrorNo("2011", "機器種別台数", "現在の明細数の" & Integer.Parse(ViewState(P_REC_CNT)) & "以上の数値")
                    End If
                Else
                    Me.txtAppa_Cnt.psSet_ErrorNo("2011", "機器種別台数", "現在の明細数の" & Integer.Parse(ViewState(P_REC_CNT)) & "以上の数値")
                End If

                '納入希望／納入希望日入力チェック.
                If Me.dtbDeliv_D.ppText <> String.Empty And Me.txtDeliv.ppText <> String.Empty Then
                    Me.dtbDeliv_D.psSet_ErrorNo("5004", "納入希望", "納入希望日")
                End If

                '納入希望／納入希望日入力チェック.
                If Me.dtbDeliv_D.ppText = String.Empty And Me.txtDeliv.ppText = String.Empty Then
                    Me.dtbDeliv_D.psSet_ErrorNo("5004", "納入希望", "納入希望日")
                End If

                strTerms = ViewState(P_SESSION_TERMS)
                'If strTerms =  ClsComVer.E_遷移条件.登録 Then
                '    '納入希望日下限上限チェック(入力時のみチェック).
                '    If Me.dtbDeliv_D.ppText <> String.Empty Then
                '        If Not Me.dtbDeliv_D.ppText >= DateTime.Now.ToString("yyyy/MM/dd") Then
                '            Me.dtbDeliv_D.psSet_ErrorNo("6003", "納入希望日", "システム日付")
                '        End If
                '    End If

                '    '到着日下限上限チェック.
                '    If Not Me.dtbArrval_D.ppText >= DateTime.Now.ToString("yyyy/MM/dd") Then
                '        Me.dtbArrval_D.psSet_ErrorNo("6003", "到着日", "システム日付")
                '    End If
                'End If

                '注文番号桁数チェック.
                If Me.txtOrder_No.ppText.Length + txtOrder_Seq.ppText.Length <> "10" Then
                    Me.txtOrder_No.psSet_ErrorNo("3001", "注文番号", "10")
                End If

                '注文番号形式チェック.
                strErr = pfCheck_TxtErr(Me.txtOrder_No.ppText & Me.txtOrder_Seq.ppText, False, False, False, False, 10, "[N][G][C][整][0-9][0-9][-][0-9][0-9][0-9]", False)
                If strErr <> "" Then
                    Me.txtOrder_No.psSet_ErrorNo("4014", "注文番号の入力形式")
                    Exit Sub
                End If

                'If Master.ppRigthButton1.Text = "登録" Then
                '    '注文番号存在チェック.
                '    cmdDB = New SqlCommand("ZCMPSEL043", conDB)
                '    cmdDB.Parameters.Add(pfSet_Param("MNGNO_CLS", SqlDbType.Int, 4))
                '    cmdDB.Parameters.Add(pfSet_Param("YMD", SqlDbType.NVarChar, DateTime.Now.ToString("yyyy0101")))
                '    cmdDB.Parameters.Add(pfSet_Param("SalesYTD", SqlDbType.Int, 4, ParameterDirection.Output))
                '    dstReq = clsDataConnect.pfGet_DataSet(cmdDB)
                '    If Decimal.Parse(Me.txtOrder_Seq.ppText.Substring(3)) <> Decimal.Parse(dstReq.Tables(0).Rows(0).Item("連番").ToString) Then
                '        Me.txtOrder_No.psSet_ErrorNo("4014", "注文番号の連番")
                '        Exit Sub
                '    End If
                'End If

                ''注文番号存在チェック.
                'cmdDB = New SqlCommand(M_DISP_ID & "_S8", conDB)
                'cmdDB.Parameters.Add(pfSet_Param("order_no", SqlDbType.NVarChar, Me.txtOrder_No.ppText))
                'cmdDB.Parameters.Add(pfSet_Param("mente_no", SqlDbType.NVarChar, Me.lblMente_No.Text))
                'dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '郵便番号
                If Not Me.txtZipNo.Text = String.Empty And _
                    Not Long.TryParse(Me.txtZipNo.Text.Replace("-", ""), 0) Then
                    Me.cuvZipNo.ErrorMessage = pfGet_ValMes("4001", "郵便番号", "ハイフン(-)か数字").Item(P_VALMES_MES)
                    Me.cuvZipNo.Text = "形式エラー"
                    cuvZipNo.IsValid = False
                End If

                'ＴＥＬ
                If Not Me.txtTel.Text = String.Empty And _
                    Not Long.TryParse(Me.txtTel.Text.Replace("-", ""), 0) Then
                    Me.cuvTel.ErrorMessage = pfGet_ValMes("4001", "ＴＥＬ", "ハイフン(-)か数字").Item(P_VALMES_MES)
                    Me.cuvTel.Text = "形式エラー"
                    cuvTel.IsValid = False
                End If

                'ＦＡＸ
                If Not Me.txtFAX.Text = String.Empty And _
                    Not Long.TryParse(Me.txtFAX.Text.Replace("-", ""), 0) Then
                    Me.cuvFAX.ErrorMessage = pfGet_ValMes("4001", "ＦＡＸ", "ハイフン(-)か数字").Item(P_VALMES_MES)
                    Me.cuvFAX.Text = "形式エラー"
                    cuvFAX.IsValid = False
                End If

                '---------------------------
                '2014/05/14 後藤 ここから
                '---------------------------
                'If dstOrders.Tables(0).Rows(0).Item("件数").ToString() <> "0" Then
                '    Me.txtOrder_No.psSet_ErrorNo("4001", "注文番号", "すでに登録されています。別の番号")
                'End If

                'Select Case Master.ppRigthButton1.Text

                '    Case "登録"
                '        If dstOrders.Tables(0).Rows(0).Item("件数").ToString() <> "0" Then
                '            Me.txtOrder_No.psSet_ErrorNo("4001", "注文番号", "すでに登録されています。別の番号")
                '        End If
                '    Case "更新"
                '        '更新前と値が違う場合にチェック.
                '        If ViewState(P_ORDER_NO) <> Me.txtOrder_No.ppText & Me.txtOrder_Seq.ppText Then
                '            If dstOrders.Tables(0).Rows(0).Item("件数").ToString() <> "0" Then
                '                Me.txtOrder_No.psSet_ErrorNo("4001", "注文番号", "すでに登録されています。別の番号")
                '            End If
                '        End If
                'End Select
                '---------------------------
                '2014/05/14 後藤 ここまで
                '---------------------------

            End If



        Catch ex As Exception
            psMesBox(Me, "00004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "注文番号")
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

    Function mfCheck_Order()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstReq As New DataSet

        mfCheck_Order = False

        Try
            '開始ログ出力.
            psLogStart(Me)

            '接続.
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else
                '注文番号存在チェック.
                cmdDB = New SqlCommand(M_DISP_ID & "_S8", conDB)
                cmdDB.Parameters.Add(pfSet_Param("order_no", SqlDbType.NVarChar, Me.txtOrder_No.ppText & Me.txtOrder_Seq.ppText))
                cmdDB.Parameters.Add(pfSet_Param("mente_no", SqlDbType.NVarChar, Me.lblMente_No.Text))
                dstReq = clsDataConnect.pfGet_DataSet(cmdDB)
                If dstReq.Tables(0).Rows(0).Item("件数").ToString > "0" Then
                    If ViewState("CheckCode") = Me.txtOrder_No.ppText & Me.txtOrder_Seq.ppText Then
                        mfCheck_Order = True
                    Else
                        ViewState("CheckCode") = Me.txtOrder_No.ppText & Me.txtOrder_Seq.ppText
                        Me.txtOrder_No.psSet_ErrorNo("2017")
                        mfCheck_Order = False
                    End If
                Else
                    mfCheck_Order = True
                End If
            End If
        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "注文番号")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Function

    ''' <summary>
    ''' ドロップダウンリスト生成処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfGet_DropListData_Sel() As Boolean

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
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                mfGet_DropListData_Sel = False
            Else

                '依頼先ドロップダウンリスト生成.
                strMessage = "業者マスタ一覧取得"
                cmdDB = New SqlCommand(sCnsSqlid_040, conDB)
                cmdDB.Parameters.Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, ""))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                Me.ddlReqcomp_Cd.DataSource = dstOrders.Tables(0)
                Me.ddlReqcomp_Cd.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
                Me.ddlReqcomp_Cd.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
                Me.ddlReqcomp_Cd.DataBind()
                Me.ddlReqcomp_Cd.Items.Insert(0, " ")
                Me.ddlReqcomp_Cd.SelectedIndex = 0

                '機器分類ドロップダウンリスト生成.
                strMessage = "機器分類マスタ一覧取得"
                cmdDB = New SqlCommand("ZCMPSEL012", conDB)
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                Me.ddlRstAppaDiv.DataSource = dstOrders.Tables(0)
                Me.ddlRstAppaDiv.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
                Me.ddlRstAppaDiv.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
                Me.ddlRstAppaDiv.DataBind()
                Me.ddlRstAppaDiv.Items.Insert(0, " ")
                Me.ddlRstAppaDiv.SelectedIndex = 0

                '機器種別ドロップダウンリスト生成.
                strMessage = "機器種別マスタ一覧取得"
                cmdDB = New SqlCommand("ZCMPSEL013", conDB)
                cmdDB.Parameters().Add(pfSet_Param("appadivcd", SqlDbType.NVarChar, ""))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                Me.ddlAppa_Nm.DataSource = dstOrders.Tables(0)
                Me.ddlAppa_Nm.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
                Me.ddlAppa_Nm.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
                Me.ddlAppa_Nm.DataBind()
                Me.ddlAppa_Nm.Items.Insert(0, " ")
                Me.ddlAppa_Nm.SelectedIndex = 0

                'システムコードドロップダウンリスト生成.
                strMessage = "システムコードマスタ一覧取得"
                cmdDB = New SqlCommand(sCnsSqlid_038, conDB)
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                Me.ddlSystem.DataSource = dstOrders.Tables(0)
                Me.ddlSystem.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
                Me.ddlSystem.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
                Me.ddlSystem.DataBind()
                Me.ddlSystem.Items.Insert(0, " ")
                Me.ddlSystem.SelectedIndex = 0

                '型式/機器ドロップダウンリスト生成.
                Me.ddlRstAppaModel.Items.Clear()    'データが無い場合は初期化
                Me.ddlRstAppaModel.Enabled = False  'データが無い場合は非活性

                '納入先ドロップダウンリスト生成.
                strMessage = "業者マスタ一覧取得"
                cmdDB = New SqlCommand(sCnsSqlid_039, conDB)
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                Me.ddlDeliv_Cd.DataSource = dstOrders.Tables(0)
                Me.ddlDeliv_Cd.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
                Me.ddlDeliv_Cd.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
                Me.ddlDeliv_Cd.DataBind()
                Me.ddlDeliv_Cd.Items.Insert(0, " ")
                Me.ddlDeliv_Cd.SelectedIndex = 0

                '正常終了.
                mfGet_DropListData_Sel = True

            End If

        Catch ex As Exception
            '--------------------------------
            '2014/05/13 後藤　ここから
            '--------------------------------
            'psMesBox(Me, "00004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, strMessage)
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMessage)
            '--------------------------------
            '2014/05/13 後藤　ここまで
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
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                mfGet_DropListData_Sel = False
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function


    Protected Sub ddlRstAppaDiv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRstAppaDiv.SelectedIndexChanged

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet        '表示情報のデータセット

        Try
            '接続.
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else
                '機器種別ドロップダウンリスト生成.
                cmdDB = New SqlCommand("ZCMPSEL013", conDB)
                cmdDB.Parameters().Add(pfSet_Param("appadivcd", SqlDbType.NVarChar, Me.ddlRstAppaDiv.SelectedValue))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                Me.ddlAppa_Nm.DataSource = dstOrders.Tables(0)
                Me.ddlAppa_Nm.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
                Me.ddlAppa_Nm.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
                Me.ddlAppa_Nm.DataBind()
                Me.ddlAppa_Nm.Items.Insert(0, " ")
                Me.ddlAppa_Nm.SelectedIndex = 0
            End If

            msSetddlSystem()
            msSetddlAppamodel()

        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, " 機器種別マスタ一覧取得")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
            '終了ログ出力
            psLogEnd(Me)
        End Try
    End Sub


    ''' <summary>
    ''' ドロップダウンリスト設定（システム）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlSystem()       '---- 変更 2014/06/11 ----

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("SERUPDP001_S5", objCn)

                'パラメータ設定
                With objCmd.Parameters
                    '機器分類コード
                    .Add(pfSet_Param("appaclass_cd", SqlDbType.NVarChar, Me.ddlRstAppaDiv.SelectedValue))
                    '機器種別コード
                    .Add(pfSet_Param("appa_cls", SqlDbType.NVarChar, Me.ddlSystem.SelectedValue))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables(0).Rows.Count = 0 Or Me.ddlRstAppaDiv.SelectedValue <> "01" Then
                    '＜検索結果（明細）＞
                    Me.ddlSystem.Items.Clear()
                    Me.ddlSystem.Enabled = False
                Else
                    '検索結果（明細）
                    Me.ddlSystem.Items.Clear()
                    Me.ddlSystem.DataSource = objDs.Tables(0)
                    Me.ddlSystem.DataTextField = "システム"
                    Me.ddlSystem.DataValueField = "システムコード"
                    Me.ddlSystem.DataBind()
                    Me.ddlSystem.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                    Me.ddlSystem.Enabled = True
                End If

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


    Protected Sub ddlAppa_Nm_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAppa_Nm.SelectedIndexChanged, ddlSystem.SelectedIndexChanged
        msSetddlAppamodel()
    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（機器マスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlAppamodel()

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("MNTUPDP001_S9", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '機器分類コード
                    .Add(pfSet_Param("appaclass_cd", SqlDbType.NVarChar, Me.ddlRstAppaDiv.SelectedValue))
                    '機器種別コード
                    .Add(pfSet_Param("appa_cls", SqlDbType.NVarChar, Me.ddlAppa_Nm.SelectedValue))
                    'システムコード
                    .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, Me.ddlSystem.SelectedValue))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables(0).Rows.Count = 0 Then
                    Me.ddlRstAppaModel.Items.Clear()    'データが無い場合は初期化
                    Me.ddlRstAppaModel.Enabled = False  'データが無い場合は非活性
                Else
                    'ドロップダウンリスト設定
                    Me.ddlRstAppaModel.Items.Clear()
                    Me.ddlRstAppaModel.DataSource = objDs.Tables(0)
                    Me.ddlRstAppaModel.DataTextField = "型式機器"
                    Me.ddlRstAppaModel.DataValueField = "型式機器コード"
                    Me.ddlRstAppaModel.DataBind()
                    Me.ddlRstAppaModel.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加
                    Me.ddlRstAppaModel.Enabled = True
                End If
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器マスタ一覧取得")
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
    ''' オブジェクト取得.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDBNull(ByVal strVal As String) As Object
        If String.IsNullOrEmpty(strVal) = True Then
            Return DBNull.Value
        End If
        Return strVal

    End Function
    ''' <summary>
    ''' 日付変換処理.
    ''' </summary>
    ''' <param name="strReport_D">日付</param>
    ''' <param name="strType">1:和暦、2:月日(曜日)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfChange_Jp(ByVal strReport_D As String, ByVal strType As String) As String

        Dim strYear As String = String.Empty
        Dim strMouth As String = String.Empty
        Dim strDay As String = String.Empty
        Dim dtTarget As DateTime = Nothing
        Dim culture As CultureInfo = New CultureInfo("ja-JP", True)
        culture.DateTimeFormat.Calendar = New JapaneseCalendar()
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '--------------------------------
        '2014/05/14 後藤　ここから
        '--------------------------------
        If strReport_D.Length = 0 Then
            Return String.Empty
        End If
        '--------------------------------
        '2014/05/14 後藤　ここまで
        '--------------------------------
        Try
            '年・月・日に分割.
            strYear = strReport_D.Substring(0, 4)
            strMouth = strReport_D.Substring(5, 2)
            strDay = strReport_D.Substring(8, 2)
            dtTarget = New DateTime(strYear, strMouth, strDay)

        Catch ex As Exception


            '--------------------------------
            '2014/05/13 後藤　ここから
            '--------------------------------
            'psMesBox(Me, "", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "日付の変換")
            '--------------------------------
            '2014/05/13 後藤　ここまで
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
        End Try

        '戻り値
        If strType = "1" Then
            'Return dtTarget.ToString("ggyy年M月d日", culture)
            Return dtTarget.ToString("yyyy年M月d日")
        Else
            Return dtTarget.ToString("MM月dd日(ddd)")
        End If

    End Function

#End Region

End Class