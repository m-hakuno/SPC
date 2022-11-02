'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　工事完了報告書
'*　ＰＧＭＩＤ：　CNSUPDP006
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.25　：　土岐
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect

'Imports SPC.Global_asax
'-----------------------------
'2014/04/21 土岐　ここから
'-----------------------------
'排他制御用
Imports SPC.ClsCMExclusive
'-----------------------------
'2014/04/21 土岐　ここまで
'-----------------------------
'Imports clscomver

#End Region

Public Class CNSUPDP006
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
    'プログラムID
    Const M_DISP_ID = P_FUN_CNS & P_SCR_UPD & P_PAGE & "006"

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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        AddHandler Master.ppRigthButton1.Click, AddressOf btnUpdate_Click

        If Not IsPostBack Then  '初回表示
            'プログラムＩＤ、画面名設定
            Master.ppProgramID = M_DISP_ID
            Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

            'パンくずリスト設定
            Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

            'セッション変数「遷移条件」「キー情報」が存在しない場合、画面を閉じる
            If Session(P_SESSION_TERMS) Is Nothing Or Session(P_KEY) Is Nothing Then
                psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
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

            'ViewStateに「遷移条件」「遷移元ＩＤ」を保存
            ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)
            ViewState(P_KEY) = Session(P_KEY)(0)
            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)
            '-----------------------------
            '2014/04/21 土岐　ここから
            '-----------------------------
            '★排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
            If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then

                Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)

            End If
            '-----------------------------
            '2014/04/21 土岐　ここまで
            '-----------------------------

            msSet_Mode(ViewState(P_SESSION_TERMS))

            '画面クリア処理
            msClearScreen()

            'データ取得
            Me.lblbConstNo2.Text = ViewState(P_KEY)
            msGet_Data(ViewState(P_KEY))

            If Me.grvList1.Rows.Count > 0 Then
                CType(Me.grvList1.Rows(0).FindControl("old_row"), ClsCMTextBox).ppTextBox.Focus()
            End If
        End If

        '合計行を読み取り専用に設定（デザイナ上で設定した場合、値がポストバックされないため）
        For Each rowGrid As GridViewRow In grvList1.Rows
            CType(rowGrid.FindControl("afttot"), TextBox).Attributes.Add("readOnly", "readOnly")
        Next
        For Each rowGrid As GridViewRow In grvList2.Rows
            CType(rowGrid.FindControl("store_afttot"), TextBox).Attributes.Add("readOnly", "readOnly")
        Next
        For Each rowGrid As GridViewRow In grvList3.Rows
            CType(rowGrid.FindControl("new_f"), TextBox).Attributes.Add("readOnly", "readOnly")
            CType(rowGrid.FindControl("div_f"), TextBox).Attributes.Add("readOnly", "readOnly")
            CType(rowGrid.FindControl("aft_f"), TextBox).Attributes.Add("readOnly", "readOnly")
        Next
    End Sub

    '---------------------------
    '2014/04/23 武 ここから
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
            Case "NGC"
        End Select

    End Sub
    '---------------------------
    '2014/04/23 武 ここまで
    '---------------------------

    ''' <summary>
    ''' 登録／更新処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs)
        Const PRA_OLD As String = "old_row"
        Const PRA_MOV As String = "mov_row"
        Const PRA_REM As String = "rem_row"
        Const PRA_NEW As String = "newtot_row"
        Const PRA_DIV As String = "divertot_row"
        Const PRA_STORE As String = "store_"

        Const PRA_STORE_NEW As String = "new_"
        Const PRA_STORE_DIV As String = "div_"
        Const PRA_STORE_AFT As String = "aft_"
        Const PRA_STORE_F1 As String = "f1_row"
        Const PRA_STORE_F2 As String = "f2_row"
        Const PRA_STORE_F3 As String = "f3_row"
        Const PRA_STORE_F4 As String = "f4_row"
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim intRtn As Integer
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'GridViewコントロールＩＤ
        Dim strPra() As String = {PRA_OLD, PRA_MOV, PRA_REM, PRA_NEW, PRA_DIV}
        Dim strPraStore() As String = {PRA_STORE & PRA_OLD, PRA_STORE & PRA_MOV,
                                       PRA_STORE & PRA_REM, PRA_STORE & PRA_NEW}
        Dim strPraF() As String = {PRA_STORE_NEW & PRA_STORE_F1, PRA_STORE_NEW & PRA_STORE_F2,
                                   PRA_STORE_NEW & PRA_STORE_F3, PRA_STORE_NEW & PRA_STORE_F4,
                                   String.Empty,
                                   PRA_STORE_DIV & PRA_STORE_F1, PRA_STORE_DIV & PRA_STORE_F2,
                                   PRA_STORE_DIV & PRA_STORE_F3, PRA_STORE_DIV & PRA_STORE_F4,
                                   String.Empty,
                                   PRA_STORE_AFT & PRA_STORE_F1, PRA_STORE_AFT & PRA_STORE_F2,
                                   PRA_STORE_AFT & PRA_STORE_F3, PRA_STORE_AFT & PRA_STORE_F4}

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'ログ出力開始
        psLogStart(Me)

        If (Page.IsValid) Then
            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    cmdDB = New SqlCommand("CNSUPDP006_U1", conDB)
                    With cmdDB.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("const_no", SqlDbType.NVarChar, Me.lblbConstNo2.Text))                 '工事依頼番号
                        .Add(pfSet_Param("constcom_no", SqlDbType.NVarChar, Me.lblConstcomNo2.Text))            '工事通知番号
                        .Add(pfSet_Param("total_bef", SqlDbType.NVarChar, Me.lblTotalBef2.Text))                '締日
                        .Add(pfSet_Param("fsblty_stdy", SqlDbType.NVarChar, Me.ddlFsbltyStdy.ppSelectedValue))  '事前調査
                        .Add(pfSet_Param("cstmluck_use", SqlDbType.NVarChar, Me.ddlCstmluckUSE.ppSelectedValue)) '改造ラック使用
                        .Add(pfSet_Param("upsincorp", SqlDbType.NVarChar, Me.ddlUpsincorp.ppSelectedValue))     'ＵＰＳ内蔵タイプ
                        .Add(pfSet_Param("tbluck", SqlDbType.NVarChar, Me.ddlTBLuck.ppSelectedValue))           'ＴＢラック
                        .Add(pfSet_Param("tboxto_cls", SqlDbType.NVarChar, Me.ddlTboxToCls.ppSelectedValue))    'ＴＢＯＸ持帰区分
                        .Add(pfSet_Param("imptnt_notice", SqlDbType.NVarChar, Me.txtImptntNotice.ppText))       '特記事項
                        .Add(pfSet_Param("frequency1", SqlDbType.NVarChar, Me.ddlFrequenxy1.ppSelectedValue))   '周波数1
                        .Add(pfSet_Param("frequency2", SqlDbType.NVarChar, Me.ddlFrequenxy2.ppSelectedValue))   '周波数2
                        .Add(pfSet_Param("frequency3", SqlDbType.NVarChar, Me.ddlFrequenxy3.ppSelectedValue))   '周波数3
                        .Add(pfSet_Param("frequency4", SqlDbType.NVarChar, Me.ddlFrequenxy4.ppSelectedValue))   '周波数4
                        .Add(pfSet_Param("frequency5", SqlDbType.NVarChar, Me.ddlFrequenxy5.ppSelectedValue))   '周波数5
                        .Add(pfSet_Param("frequency6", SqlDbType.NVarChar, Me.ddlFrequenxy6.ppSelectedValue))   '周波数6
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                     'ユーザーＩＤ
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                       '戻り値
                    End With
                    '設置工事
                    msSetUpdatePrm(cmdDB.Parameters, strPra, Me.grvList1)
                    '店内設置工事　明細１
                    msSetUpdatePrm(cmdDB.Parameters, strPraStore, Me.grvList2)
                    '店内設置工事　新設,撤去設,工事後明細
                    msSetUpdatePrm(cmdDB.Parameters, strPraF, Me.grvList3)

                    'データ更新
                    Using conTrn = conDB.BeginTransaction
                        cmdDB.Transaction = conTrn
                        'コマンドタイプ設定(ストアド)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            Select Case ViewState(P_SESSION_TERMS)
                                Case ClsComVer.E_遷移条件.登録
                                    psMesBox(Me, "00008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事完了報告書", intRtn.ToString)
                                Case ClsComVer.E_遷移条件.更新
                                    psMesBox(Me, "00007", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事完了報告書", intRtn.ToString)
                            End Select
                            Exit Sub
                        End If

                        'コミット
                        conTrn.Commit()
                    End Using

                    Select Case ViewState(P_SESSION_TERMS)
                        Case ClsComVer.E_遷移条件.登録
                            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "工事完了報告書")
                        Case ClsComVer.E_遷移条件.更新
                            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "工事完了報告書")
                    End Select
                Catch ex As Exception
                    Select Case ViewState(P_SESSION_TERMS)
                        Case ClsComVer.E_遷移条件.登録
                            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事完了報告書")
                        Case ClsComVer.E_遷移条件.更新
                            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事完了報告書")
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
    ''' 設置工事明細、店内設置工事明細１合計チェック
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub sum_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Const NAME As String = "lblName"
        msCheckSum(NAME, ClsComVer.E_VMタイプ.ショート, CType(source, CustomValidator), args)
    End Sub

    ''' <summary>
    ''' 店内設置工事新設明細合計チェック
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvNew_f_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Const NAME As String = "lblNewName"
        msCheckSum(NAME, ClsComVer.E_VMタイプ.ショート, CType(source, CustomValidator), args)

    End Sub

    ''' <summary>
    ''' 店内設置工事撤去設明細合計チェック
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvDiv_f_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Const NAME As String = "lblDivName"
        msCheckSum(NAME, ClsComVer.E_VMタイプ.ショート, CType(source, CustomValidator), args)

    End Sub

    ''' <summary>
    ''' 店内設置工事工事後明細合計チェック
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvAft_f_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Const NAME As String = "lblAftName"
        msCheckSum(NAME, ClsComVer.E_VMタイプ.ショート, CType(source, CustomValidator), args)

    End Sub
#End Region

#Region "そのほかのプロシージャ"
    ''' <summary>
    ''' 検索処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data(ByVal ipstrConstNo As String)
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
                '--工事依頼書兼仕様書取得--
                cmdDB = New SqlCommand("CNSUPDP006_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("const_no", SqlDbType.NVarChar, ipstrConstNo))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得したデータを設定
                msSet_MainData(dstOrders)

                '--設置工事　明細取得--
                cmdDB = New SqlCommand("CNSUPDP006_S2", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("const_no", SqlDbType.NVarChar, ipstrConstNo))
                    .Add(pfSet_Param("screen_id", SqlDbType.NVarChar, M_DISP_ID))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得したデータをリストに設定
                Me.grvList1.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList1.DataBind()

                For zz As Integer = 15 To 17
                    If Me.grvList1.Rows.Count > zz Then
                        CType(Me.grvList1.Rows(zz - 1).FindControl("newtot_row"), ClsCMTextBox).Visible = False
                        CType(Me.grvList1.Rows(zz - 1).FindControl("divertot_row"), ClsCMTextBox).Visible = False
                    End If
                Next

                For zz As Integer = 21 To 22
                    If Me.grvList1.Rows.Count > zz Then
                        CType(Me.grvList1.Rows(zz - 1).FindControl("newtot_row"), ClsCMTextBox).Visible = False
                        CType(Me.grvList1.Rows(zz - 1).FindControl("divertot_row"), ClsCMTextBox).Visible = False
                    End If
                Next

                '--店内設置工事　明細１取得--
                cmdDB = New SqlCommand("CNSUPDP006_S3", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("const_no", SqlDbType.NVarChar, ipstrConstNo))
                    .Add(pfSet_Param("screen_id", SqlDbType.NVarChar, M_DISP_ID))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得したデータをリストに設定
                Me.grvList2.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList2.DataBind()

                '--店内設置工事　新設,撤去設,工事後明細取得--
                cmdDB = New SqlCommand("CNSUPDP006_S4", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("const_no", SqlDbType.NVarChar, ipstrConstNo))
                    .Add(pfSet_Param("screen_id", SqlDbType.NVarChar, M_DISP_ID))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得したデータをリストに設定
                Me.grvList3.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList3.DataBind()

                '工事後の7行目　項目非表示処理
                If Me.grvList3.Rows.Count > 7 Then
                    CType(Me.grvList3.Rows(7 - 1).FindControl("aft_f1_row"), ClsCMTextBox).Visible = False
                    CType(Me.grvList3.Rows(7 - 1).FindControl("aft_f2_row"), ClsCMTextBox).Visible = False
                    CType(Me.grvList3.Rows(7 - 1).FindControl("aft_f3_row"), ClsCMTextBox).Visible = False
                    CType(Me.grvList3.Rows(7 - 1).FindControl("aft_f4_row"), ClsCMTextBox).Visible = False
                    CType(Me.grvList3.Rows(7 - 1).FindControl("aft_f"), TextBox).Visible = False
                End If
            Catch ex As Exception
                '-----------------------------
                '2014/05/19 土岐　ここから
                '-----------------------------
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "工事完了報告書")
                '-----------------------------
                '2014/05/19 土岐　ここまで
                '-----------------------------
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
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
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            '-----------------------------
            '2014/05/19 土岐　ここから
            '-----------------------------
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            '-----------------------------
            '2014/05/19 土岐　ここまで
            '-----------------------------
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
    End Sub

    ''' <summary>
    ''' データ設定処理
    ''' </summary>
    ''' <param name="ipdstData">設定データ</param>
    ''' <remarks></remarks>
    Private Sub msSet_MainData(ByVal ipdstData As DataSet)
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            '取得したデータを設定
            With ipdstData.Tables(0).Rows(0)
                Me.lblConstcomNo2.Text = .Item("工事通知番号").ToString
                Me.lblTotalBef2.Text = .Item("データ締日").ToString
                Me.txtImptntNotice.ppText = .Item("特記事項").ToString
                Me.ddlFsbltyStdy.ppSelectedValue = .Item("事前調査").ToString
                Me.ddlCstmluckUSE.ppSelectedValue = .Item("改造ラック使用").ToString
                Me.ddlUpsincorp.ppSelectedValue = .Item("ＵＰＳ内臓").ToString
                Me.ddlTBLuck.ppSelectedValue = .Item("ＴＢラック").ToString
                Me.ddlFrequenxy1.ppSelectedValue = .Item("周波数１").ToString
                Me.ddlFrequenxy2.ppSelectedValue = .Item("周波数２").ToString
                Me.ddlFrequenxy3.ppSelectedValue = .Item("周波数３").ToString
                Me.ddlFrequenxy4.ppSelectedValue = .Item("周波数４").ToString
                Me.ddlFrequenxy5.ppSelectedValue = .Item("周波数５").ToString
                Me.ddlFrequenxy6.ppSelectedValue = .Item("周波数６").ToString
                Me.ddlTboxToCls.ppSelectedValue = .Item("ＴＢＯＸ持帰区分").ToString
                Me.lblNew2.Text = .Item("工事種別_新規").ToString
                Me.lblAdd2.Text = .Item("工事種別_増設").ToString
                Me.lblReset2.Text = .Item("工事種別_再設置").ToString
                Me.lblRelocate2.Text = .Item("工事種別_店内移設").ToString
                Me.lblPrtRemove2.Text = .Item("工事種別_一部撤去").ToString
                Me.lblAllRemove2.Text = .Item("工事種別_全撤去").ToString
                Me.lblTmpRemove2.Text = .Item("工事種別_一時撤去").ToString
                Me.lblChngOrgnz2.Text = .Item("工事種別_構成変更").ToString
                Me.lblDlvOrgnz2.Text = .Item("工事種別_構成配信").ToString
                Me.lblVup2.Text = .Item("工事種別_ＶＵＰ").ToString
                Me.lblOth2.Text = .Item("工事種別_その他").ToString
                Me.lblOthDtl2.Text = .Item("工事種別_その他内容").ToString
                If .IsNull("工事完了報告書有無") Then
                    '「登録」ボタン設定
                    Master.ppRigthButton1.Text = P_BTN_NM_ADD
                    Master.ppRigthButton1.OnClientClick =
                        pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事完了報告書")
                    ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録
                Else
                    '「更新」ボタン設定
                    Master.ppRigthButton1.Text = P_BTN_NM_UPD
                    Master.ppRigthButton1.OnClientClick =
                        pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事完了報告書")
                    ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
                End If
                Master.ppRigthButton1.Visible = True
            End With
        Catch ex As Exception
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' GridViewに設定された情報をパラメータに登録
    ''' </summary>
    ''' <param name="cpparPra">登録するパラメータ</param>
    ''' <param name="ipstrPraNm">登録するパラメータ名、GridViewの取得コントロール名</param>
    ''' <param name="ipgrvGrid">データを取得するGridview</param>
    ''' <remarks></remarks>
    Private Sub msSetUpdatePrm(ByRef cpparPra As SqlParameterCollection,
                               ByVal ipstrPraNm() As String,
                               ByVal ipgrvGrid As GridView)
        Dim decPra As Decimal
        Dim strPra As String
        For zz As Integer = 0 To (ipgrvGrid.Rows.Count - 1)
            For Each strPraNm As String In ipstrPraNm
                If strPraNm <> String.Empty Then
                    '入力値取得
                    strPra = CType(ipgrvGrid.Rows(zz).FindControl(strPraNm), ClsCMTextBox).ppText
                    If strPra <> String.Empty Then      '入力がある
                        decPra = Decimal.Parse(strPra)
                        cpparPra.Add(pfSet_Param(strPraNm & (zz + 1).ToString, SqlDbType.Decimal, decPra))
                    Else                                '入力がない場合はNullで登録
                        cpparPra.Add(pfSet_Param(strPraNm & (zz + 1).ToString, SqlDbType.Decimal, DBNull.Value))
                    End If
                End If
            Next
        Next

    End Sub

    ''' <summary>
    ''' 画面クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()
        Me.lblbConstNo2.Text = String.Empty     '工事依頼番号
        Me.lblConstcomNo2.Text = String.Empty   '工事通知番号
        Me.lblTotalBef2.Text = String.Empty     'データ締日
        Me.lblNew2.Text = String.Empty          '工事種別_新規
        Me.lblAdd2.Text = String.Empty          '工事種別_増設
        Me.lblReset2.Text = String.Empty        '工事種別_再設置
        Me.lblRelocate2.Text = String.Empty     '工事種別_店内移設
        Me.lblPrtRemove2.Text = String.Empty    '工事種別_一部撤去
        Me.lblAllRemove2.Text = String.Empty    '工事種別_全撤去
        Me.lblTmpRemove2.Text = String.Empty    '工事種別_一時撤去
        Me.lblChngOrgnz2.Text = String.Empty    '工事種別_構成変更
        Me.lblDlvOrgnz2.Text = String.Empty     '工事種別_構成配信
        Me.lblVup2.Text = String.Empty          '工事種別_ＶＵＰ
        Me.lblOth2.Text = String.Empty          '工事種別_その他
        Me.lblOthDtl2.Text = String.Empty       '工事種別_その他内容

        Me.grvList1.DataSource = New Object() {}
        Me.grvList1.DataBind()
        Me.grvList2.DataSource = New Object() {}
        Me.grvList2.DataBind()
        Me.grvList3.DataSource = New Object() {}
        Me.grvList3.DataBind()
    End Sub

    ''' <summary>
    ''' 遷移条件によって活性／非活性を設定する。
    ''' </summary>
    ''' <param name="ipshtMode">遷移条件</param>
    ''' <remarks></remarks>
    Private Sub msSet_Mode(ByVal ipshtMode As ClsComVer.E_遷移条件)
        Select Case ipshtMode
            Case ClsComVer.E_遷移条件.参照
                '非活性設定
                Me.grvList1.Enabled = False
                Me.grvList2.Enabled = False
                Me.grvList3.Enabled = False
                Me.ddlFrequenxy1.ppEnabled = False
                Me.ddlFrequenxy2.ppEnabled = False
                Me.ddlFrequenxy3.ppEnabled = False
                Me.ddlFrequenxy4.ppEnabled = False
                Me.ddlFrequenxy5.ppEnabled = False
                Me.ddlFrequenxy6.ppEnabled = False
                Me.ddlFsbltyStdy.ppEnabled = False
                Me.ddlCstmluckUSE.ppEnabled = False
                Me.ddlUpsincorp.ppEnabled = False
                Me.ddlTBLuck.ppEnabled = False
                Me.ddlTboxToCls.ppEnabled = False
                Me.txtImptntNotice.ppEnabled = False
                Master.ppRigthButton1.Enabled = False
            Case ClsComVer.E_遷移条件.更新, ClsComVer.E_遷移条件.登録
                '活性設定
                Me.grvList1.Enabled = True
                Me.grvList2.Enabled = True
                Me.grvList3.Enabled = True
                Me.ddlFrequenxy1.ppEnabled = True
                Me.ddlFrequenxy2.ppEnabled = True
                Me.ddlFrequenxy3.ppEnabled = True
                Me.ddlFrequenxy4.ppEnabled = True
                Me.ddlFrequenxy5.ppEnabled = True
                Me.ddlFrequenxy6.ppEnabled = True
                Me.ddlFsbltyStdy.ppEnabled = True
                Me.ddlCstmluckUSE.ppEnabled = True
                Me.ddlUpsincorp.ppEnabled = True
                Me.ddlTBLuck.ppEnabled = True
                Me.ddlTboxToCls.ppEnabled = True
                Me.txtImptntNotice.ppEnabled = True
                Master.ppRigthButton1.Enabled = True
        End Select
    End Sub

    ''' <summary>
    ''' 合計チェック処理
    ''' </summary>
    ''' <param name="ipstrNameID">名称コントロールＩＤ</param>
    ''' <param name="ipshtMesType">表示メッセージタイプ</param>
    ''' <param name="cpcuvAfttot">検証コントロール</param>
    ''' <param name="cpArgs">検証イベント</param>
    ''' <remarks></remarks>
    Private Sub msCheckSum(ByVal ipstrNameID As String,
                           ByVal ipshtMesType As ClsComVer.E_VMタイプ,
                           ByRef cpcuvAfttot As CustomValidator,
                           ByRef cpArgs As ServerValidateEventArgs)

        Dim dtrErrMes As DataRow
        Dim strName As String
        Dim intVal As Integer
        Dim strMesType As String = P_VALMES_SMES

        '合計がない場合はチェックを行わない
        If cpArgs.Value = String.Empty Then
            Return
        End If

        '表示メッセージタイプ設定
        Select Case ipshtMesType
            Case ClsComVer.E_VMタイプ.アスタ
                strMesType = P_VALMES_AST
            Case ClsComVer.E_VMタイプ.ショート
                strMesType = P_VALMES_SMES
            Case ClsComVer.E_VMタイプ.メッセージ
                strMesType = P_VALMES_MES
        End Select

        '0以下チェック
        Integer.TryParse(cpArgs.Value, intVal)
        If intVal < 0 Then
            strName = CType(cpcuvAfttot.Parent.FindControl(ipstrNameID), Label).Text
            dtrErrMes = pfGet_ValMes("6002", strName, "0以上")
            cpcuvAfttot.Text = dtrErrMes.Item(strMesType)
            cpcuvAfttot.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
            cpArgs.IsValid = False
            Return
        End If

        '数値チェック
        If Not pfCheck_Num(cpArgs.Value) Then
            strName = CType(cpcuvAfttot.Parent.FindControl(ipstrNameID), Label).Text
            dtrErrMes = pfGet_ValMes("4009", strName)
            cpcuvAfttot.Text = dtrErrMes.Item(strMesType)
            cpcuvAfttot.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
            cpArgs.IsValid = False
            Return
        End If

        '桁数チェック
        If cpArgs.Value.Length > 5 Then
            strName = CType(cpcuvAfttot.Parent.FindControl(ipstrNameID), Label).Text
            dtrErrMes = pfGet_ValMes("3003", strName, "5")
            cpcuvAfttot.Text = dtrErrMes.Item(strMesType)
            cpcuvAfttot.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
            cpArgs.IsValid = False
            Return
        End If

    End Sub
#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
