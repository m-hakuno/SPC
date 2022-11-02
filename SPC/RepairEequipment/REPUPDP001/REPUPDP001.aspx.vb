'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜修理・整備業務＞
'*　処理名　　：　修理有償部品費用
'*　ＰＧＭＩＤ：　REPUPDP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.14　：　後藤
'*  更　新　　：　2014.06.23　：　間瀬
'*  更　新　　：　2014.07.01　：　間瀬
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive
#End Region

Public Class REPUPDP001
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
    '画面ID.
    Private Const M_DISP_ID = P_FUN_REP & P_SCR_UPD & P_PAGE & "001"

    ''' <summary>更新エラー</summary>
    Private Const sCnsErr_00001 As String = "00001"
    ''' <summary>削除エラー</summary>
    Private Const sCnsErr_00002 As String = "00002"
    ''' <summary>追加エラー</summary>
    Private Const sCnsErr_00003 As String = "00003"
    ''' <summary>検索エラー</summary>
    Private Const sCnsErr_00004 As String = "00004"
    ''' <summary>接続エラー</summary>
    Private Const sCnsErr_00005 As String = "00005"
    ''' <summary>切断エラー</summary>
    Private Const sCnsErr_00006 As String = "00006"
    ''' <summary>更新完了</summary>
    Private Const sCnsInfo_00001 As String = "00001"
    ''' <summary>削除完了</summary>
    Private Const sCnsInfo_00002 As String = "00002"
    ''' <summary>追加完了</summary>
    Private Const sCnsInfo_00003 As String = "00003"
    ''' <summary>更新確認</summary>
    Private Const sCnsInfo_00004 As String = "00004"
    ''' <summary>削除確認</summary>
    Private Const sCnsInfo_00005 As String = "00005"
    ''' <summary>追加確認</summary>
    Private Const sCnsInfo_00006 As String = "00006"
    ''' <summary>該当データなし</summary>
    Private Const sCnsInfo_00007 As String = "00007"
    ''' <summary>閾値エラー</summary>
    Private Const sCnsErr_30005 As String = "30005"
    ''' <summary>初期化エラー</summary>
    Private Const sCnsErr_30008 As String = "30008"
    ''' <summary>表示項目エラー</summary>
    Private Const sCnsErr_30001 As String = "30001"



    ''' <summary>業務マスタデータ取得処理</summary>
    Private Const sCnsSqlid_016 As String = "ZCMPSEL016"

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
        pfSet_GridView(Me.grvList, M_DISP_ID, M_DISP_ID & "_Header", Me.DivOut)
    End Sub

    ''' <summary>
    ''' ロード処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンアクションの設定.
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnSearchClear_Click
        Master.ppRigthButton2.CausesValidation = False
        Me.btnClear.CausesValidation = False

        '確認メッセージ設定.
        Me.btnInsert.OnClientClick = pfGet_OCClickMes(sCnsInfo_00006, clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "部品マスタ詳細")
        Me.btnUpdate.OnClientClick = pfGet_OCClickMes(sCnsInfo_00004, clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "部品マスタ詳細")
        Me.btnDelete.OnClientClick = pfGet_OCClickMes(sCnsInfo_00005, clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "部品マスタ詳細")

        AddHandler Me.btnClear.Click, AddressOf btnClear_Click
        AddHandler Me.btnInsert.Click, AddressOf btnInsert_Click
        AddHandler Me.btnUpdate.Click, AddressOf btnUpdate_Click
        AddHandler Me.btnDelete.Click, AddressOf btnDelete_Click

        If Not IsPostBack Then '初回表示.

            'プログラムＩＤ、画面名設定.
            Master.Master.ppProgramID = M_DISP_ID
            Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

            'パンくずリスト設定.
            Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

            '画面初期化処理.
            Call msInitScreen()

            Me.btnClear.Enabled = True
            Me.btnInsert.Enabled = True
            Me.btnDelete.Enabled = False
            Me.btnUpdate.Enabled = False

            '排他情報用のグループ番号保管.
            If Not Session(P_SESSION_GROUP_NUM) Is Nothing Then
                ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)
            End If

            '排他情報削除用の登録年月日時刻をマスタページのコントロールに登録.
            If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then
                Me.Master.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)
            End If

            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示
        End If

    End Sub

    '---------------------------
    '2014/04/22 武 ここから
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
    '2014/04/22 武 ここまで
    '---------------------------

    ''' <summary>
    ''' 検索クリアボタン押下処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearchClear_Click(sender As Object, e As EventArgs)

        'クリア処理.
        Call msClearScreen()

    End Sub

    ''' <summary>
    ''' 検索ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        If (Page.IsValid) Then
            Call msGet_Data(Me.ddlMakerKubun.SelectedValue,
                            Me.ddlWrkCls.ppSelectedValue,
                            Me.tftPartsCd.ppFromText,
                            Me.tftPartsCd.ppToText,
                            Me.txtPartsNm.ppText)

            '---------------------------
            '2014/04/22 武 ここまで
            '---------------------------
            '部品マスタ詳細クリア.
            'Call msPartsClearScreen()

            '活性制御.
            'Call msEnableChangeScreen(sender)

        End If
    End Sub
    ''' <summary>
    ''' 選択ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        If e.CommandName.Trim <> "btnSelect" Then
            Exit Sub
        End If

        Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    ' ボタン押下行のIndex
        Dim rowData As GridViewRow = grvList.Rows(intIndex)             ' ボタン押下行

        '排他制御用の変数.
        Dim strExclusiveDate As String = String.Empty
        Dim arTable_Name As New ArrayList
        Dim arKey As New ArrayList

        '排他情報削除.
        If Not Me.Master.Master.ppExclusiveDateDtl = String.Empty Then

            clsExc.pfDel_Exclusive(Me _
                          , Session(P_SESSION_SESSTION_ID) _
                          , Me.Master.Master.ppExclusiveDateDtl)

            Me.Master.Master.ppExclusiveDateDtl = String.Empty

        End If

        'ロック対象テーブル名の登録.
        arTable_Name.Insert(0, "M59_PARTS")

        'ロックテーブルキー項目の登録(M59_PARTS).
        arKey.Insert(0, CType(rowData.FindControl("会社コード"), TextBox).Text)      'M59_MAKER.
        arKey.Insert(1, CType(rowData.FindControl("作業分類コード"), TextBox).Text)  'M59_WRK_CLS.
        arKey.Insert(2, CType(rowData.FindControl("コード"), TextBox).Text)          'M59_PARTS_CD.

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        If ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.参照 Then
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

                Try

                Catch ex As Exception

                    'マスタ情報が存在しない.
                    psMesBox(Me, sCnsErr_30001, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                    '--------------------------------
                    '2014/04/14 星野　ここから
                    '--------------------------------
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    '--------------------------------
                    '2014/04/14 星野　ここまで
                    '--------------------------------
                    '排他情報解除.
                    clsExc.pfDel_Exclusive(Me _
                                  , Session(P_SESSION_SESSTION_ID) _
                                  , Me.Master.Master.ppExclusiveDateDtl)

                    Me.Master.Master.ppExclusiveDateDtl = String.Empty
                    Exit Sub

                End Try

                '画面に表示.
                Me.ddlMakerKubun_S.SelectedValue = DirectCast(rowData.FindControl("会社コード"), TextBox).Text
                Me.ddlWrkCls_S.ppSelectedValue = DirectCast(rowData.FindControl("作業分類コード"), TextBox).Text
                Me.txtPartsCd_S.ppText = DirectCast(rowData.FindControl("コード"), TextBox).Text
                Me.txtPartsNm_S.ppText = DirectCast(rowData.FindControl("部品名"), TextBox).Text
                Me.dtbStartDt1_S.ppText = DirectCast(rowData.FindControl("適応開始日1"), TextBox).Text
                Me.txtPrice11_S.ppText = DirectCast(rowData.FindControl("作業単価11"), TextBox).Text.Replace(",", "")
                Me.txtPrice12_S.ppText = DirectCast(rowData.FindControl("作業単価12"), TextBox).Text.Replace(",", "")
                Me.dtbStartDt2_S.ppText = DirectCast(rowData.FindControl("適応開始日2"), TextBox).Text
                Me.txtPrice21_S.ppText = DirectCast(rowData.FindControl("作業単価21"), TextBox).Text.Replace(",", "")
                Me.txtPrice22_S.ppText = DirectCast(rowData.FindControl("作業単価22"), TextBox).Text.Replace(",", "")
                Me.dtbStartDt3_S.ppText = DirectCast(rowData.FindControl("適応開始日3"), TextBox).Text
                Me.txtPrice31_S.ppText = DirectCast(rowData.FindControl("作業単価31"), TextBox).Text.Replace(",", "")
                Me.txtPrice32_S.ppText = DirectCast(rowData.FindControl("作業単価32"), TextBox).Text.Replace(",", "")
                Me.txtMakerWrkNm_S.ppText = DirectCast(rowData.FindControl("メーカー作業名"), TextBox).Text
                Me.txtNgcWrkNm_S.ppText = DirectCast(rowData.FindControl("ＮＧＣ作業名"), TextBox).Text
                Me.ddlSystem_S.SelectedValue = DirectCast(rowData.FindControl("システムコード"), TextBox).Text
                Me.ddlAppaNm_S.SelectedValue = DirectCast(rowData.FindControl("機器種別コード"), TextBox).Text
                Me.lblInsertDt_S.Text = DirectCast(rowData.FindControl("登録日時"), TextBox).Text
                Me.lblUpdateDt_S.Text = DirectCast(rowData.FindControl("更新日時"), TextBox).Text

                '活性制御.
                Call msEnableChangeScreen(sender)

                '登録年月日時刻(明細)に登録.
                Me.Master.Master.ppExclusiveDateDtl = strExclusiveDate

            Else

                '排他ロック中
                Exit Sub

            End If
        Else
            '画面に表示.
            Me.ddlMakerKubun_S.SelectedValue = DirectCast(rowData.FindControl("会社コード"), TextBox).Text
            Me.ddlWrkCls_S.ppSelectedValue = DirectCast(rowData.FindControl("作業分類コード"), TextBox).Text
            Me.txtPartsCd_S.ppText = DirectCast(rowData.FindControl("コード"), TextBox).Text
            Me.txtPartsNm_S.ppText = DirectCast(rowData.FindControl("部品名"), TextBox).Text
            Me.dtbStartDt1_S.ppText = DirectCast(rowData.FindControl("適応開始日1"), TextBox).Text
            Me.txtPrice11_S.ppText = DirectCast(rowData.FindControl("作業単価11"), TextBox).Text.Replace(",", "")
            Me.txtPrice12_S.ppText = DirectCast(rowData.FindControl("作業単価12"), TextBox).Text.Replace(",", "")
            Me.dtbStartDt2_S.ppText = DirectCast(rowData.FindControl("適応開始日2"), TextBox).Text
            Me.txtPrice21_S.ppText = DirectCast(rowData.FindControl("作業単価21"), TextBox).Text.Replace(",", "")
            Me.txtPrice22_S.ppText = DirectCast(rowData.FindControl("作業単価22"), TextBox).Text.Replace(",", "")
            Me.dtbStartDt3_S.ppText = DirectCast(rowData.FindControl("適応開始日3"), TextBox).Text
            Me.txtPrice31_S.ppText = DirectCast(rowData.FindControl("作業単価31"), TextBox).Text.Replace(",", "")
            Me.txtPrice32_S.ppText = DirectCast(rowData.FindControl("作業単価32"), TextBox).Text.Replace(",", "")
            Me.txtMakerWrkNm_S.ppText = DirectCast(rowData.FindControl("メーカー作業名"), TextBox).Text
            Me.txtNgcWrkNm_S.ppText = DirectCast(rowData.FindControl("ＮＧＣ作業名"), TextBox).Text
            Me.ddlSystem_S.SelectedValue = DirectCast(rowData.FindControl("システムコード"), TextBox).Text
            Me.ddlAppaNm_S.SelectedValue = DirectCast(rowData.FindControl("機器種別コード"), TextBox).Text
            Me.lblInsertDt_S.Text = DirectCast(rowData.FindControl("登録日時"), TextBox).Text
            Me.lblUpdateDt_S.Text = DirectCast(rowData.FindControl("更新日時"), TextBox).Text

            '活性制御.
            Call msEnableChangeScreen(sender)

            '登録年月日時刻(明細)に登録.
            Me.Master.Master.ppExclusiveDateDtl = strExclusiveDate
        End If
    End Sub
    ''' <summary>
    ''' クリアボタン押下処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)

        '部品マスタ詳細クリア処理.
        Call msPartsClearScreen()

        '活性制御.
        Call msEnableChangeScreen(sender)

    End Sub
    ''' <summary>
    ''' 追加ボタン押下処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnInsert_Click(sender As Object, e As EventArgs)
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet        '表示情報のデータセット

        '開始ログ出力.
        psLogStart(Me)
        If Not clsDataConnect.pfOpen_Database(conDB) Then

            psMesBox(Me, sCnsErr_00005, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"
            Exit Sub
        End If
        Try
            '個別チェック.
            Call msCheck_Error()

            If (Page.IsValid) Then
                cmdDB = New SqlCommand(M_DISP_ID & "_S4", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("mkr_cd", SqlDbType.NVarChar, Me.ddlMakerKubun_S.SelectedValue))
                    .Add(pfSet_Param("wrk_cls", SqlDbType.NVarChar, Me.ddlWrkCls_S.ppSelectedValue))
                    .Add(pfSet_Param("parts_cd", SqlDbType.NVarChar, Me.txtPartsCd_S.ppText))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count > 0 Then
                    If dstOrders.Tables(0).Rows(0).Item("削除フラグ").ToString = "1" Then
                        psMesBox(Me, "30010", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後, "破棄された機器データ", "追加処理")
                        'Me.grvList.DataSource = New DataTable
                        'Me.grvList.DataBind()
                        'Master.ppCount = "0"
                        Exit Sub
                    Else
                        psMesBox(Me, "30008", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後)
                        'Me.grvList.DataSource = New DataTable
                        'Me.grvList.DataBind()
                        'Master.ppCount = "0"
                        Exit Sub
                    End If
                End If

                '追加処理.
                Call msInsert_Data()

                '部品マスタ詳細クリア処理.
                Call msPartsClearScreen()

                '再検索.
                Call msGet_Data(Me.ddlMakerKubun.SelectedValue,
                                Me.ddlWrkCls.ppSelectedValue,
                                Me.tftPartsCd.ppFromText,
                                Me.tftPartsCd.ppToText,
                                Me.txtPartsNm.ppText)

                '活性制御.
                Call msEnableChangeScreen(sender)
            End If
        Catch ex As Exception
            psMesBox(Me, sCnsErr_00002, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "部品マスタ詳細")
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
                psMesBox(Me, sCnsErr_00006, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            End If
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub
    ''' <summary>
    ''' 更新ボタン押下処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs)

        '個別チェック.
        Call msCheck_Error()

        If (Page.IsValid) Then

            '更新処理.
            Call msUpdate_Data()

            '画面初期化処理.
            Call msPartsClearScreen()

            '再検索.
            Call msGet_Data(Me.ddlMakerKubun.SelectedValue,
                            Me.ddlWrkCls.ppSelectedValue,
                            Me.tftPartsCd.ppFromText,
                            Me.tftPartsCd.ppToText,
                            Me.txtPartsNm.ppText)

            '活性制御.
            Call msEnableChangeScreen(sender)
        End If
    End Sub
    ''' <summary>
    ''' 削除ボタン押下処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDelete_Click(sender As Object, e As EventArgs)

        '削除処理.
        Call msDelete_Data()

        '画面初期化処理.
        Call msPartsClearScreen()

        '再検索.
        Call msGet_Data(Me.ddlMakerKubun.SelectedValue,
                        Me.ddlWrkCls.ppSelectedValue,
                        Me.tftPartsCd.ppFromText,
                        Me.tftPartsCd.ppToText,
                        Me.txtPartsNm.ppText)

        '部品マスタ詳細クリア.
        Call msPartsClearScreen()

        '活性制御.
        Call msEnableChangeScreen(sender)

    End Sub
    ''' <summary>
    ''' メーカ名の検証.
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvRepuestCls_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvDropDownList.ServerValidate
        Dim dtrMes As DataRow
        args.IsValid = True
        If ddlMakerKubun_S.SelectedIndex = 0 Then '未選択
            dtrMes = pfGet_ValMes("5001", "メーカ名")
            cuvDropDownList.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            cuvDropDownList.Text = dtrMes.Item(P_VALMES_SMES)
            args.IsValid = False
        End If
    End Sub

#End Region

#Region "そのほかのプロシージャ"
    ''' <summary>
    ''' 画面初期化処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msInitScreen()

        Me.tftPartsCd.ppFromText = String.Empty
        Me.tftPartsCd.ppToText = String.Empty
        Me.txtPartsNm.ppText = String.Empty

        Me.txtPartsCd_S.ppText = String.Empty
        Me.txtPartsNm_S.ppText = String.Empty
        Me.txtMakerWrkNm_S.ppText = String.Empty
        Me.txtNgcWrkNm_S.ppText = String.Empty
        Me.dtbStartDt1_S.ppText = String.Empty
        Me.txtPrice11_S.ppText = String.Empty
        Me.txtPrice12_S.ppText = String.Empty
        Me.dtbStartDt2_S.ppText = String.Empty
        Me.txtPrice21_S.ppText = String.Empty
        Me.txtPrice22_S.ppText = String.Empty
        Me.dtbStartDt3_S.ppText = String.Empty
        Me.txtPrice31_S.ppText = String.Empty
        Me.txtPrice32_S.ppText = String.Empty
        Me.lblInsertDt_S.Text = String.Empty
        Me.lblUpdateDt_S.Text = String.Empty

        Me.grvList.DataSource = New Object() {}
        Master.ppCount = "0"
        Me.grvList.DataBind()

        '★排他情報用コントロールの初期化
        Me.Master.Master.ppExclusiveDate = String.Empty
        Me.Master.Master.ppExclusiveDateDtl = String.Empty

        'ドロップダウンリスト生成.
        If Not mfGet_DropDownList_Sel() Then
            '画面を終了.
            psClose_Window(Me)
        End If

    End Sub
    ''' <summary>
    ''' ドロップダウンリスト生成処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfGet_DropDownList_Sel() As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet        '表示情報のデータセット
        Dim maxVal As Integer = 0
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
                psMesBox(Me, sCnsErr_00005, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                mfGet_DropDownList_Sel = False
            End If

            'メーカ名ドロップダウンリスト生成.
            cmdDB = New SqlCommand(sCnsSqlid_016, conDB)
            cmdDB.Parameters.Add(pfSet_Param("trader_cd", SqlDbType.NVarChar, "5"))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlMakerKubun.DataSource = dstOrders.Tables(0)
            Me.ddlMakerKubun.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlMakerKubun.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlMakerKubun.DataBind()
            '--------------------------------
            '2014/05/15 後藤　ここから
            '--------------------------------
            'Me.ddlMakerKubun.Items.Insert(0, " ")
            Me.ddlMakerKubun.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/15 後藤　ここまで
            '--------------------------------

            'メーカ名ドロップダウンリスト生成(明細).
            Me.ddlMakerKubun_S.DataSource = dstOrders.Tables(0)
            Me.ddlMakerKubun_S.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlMakerKubun_S.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlMakerKubun_S.DataBind()
            '--------------------------------
            '2014/05/15 後藤　ここから
            '--------------------------------
            'Me.ddlMakerKubun_S.Items.Insert(0, " ")
            Me.ddlMakerKubun_S.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/15 後藤　ここまで
            '--------------------------------

            'システム名称ドロップダウンリスト生成.
            cmdDB = New SqlCommand(M_DISP_ID & "_S2", conDB)
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlSystem_S.DataSource = dstOrders.Tables(0)
            Me.ddlSystem_S.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlSystem_S.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlSystem_S.DataBind()
            '--------------------------------
            '2014/05/15 後藤　ここから
            '--------------------------------
            'Me.ddlSystem_S.Items.Insert(0, " ")
            Me.ddlSystem_S.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/15 後藤　ここまで
            '--------------------------------

            '機器名称ドロップダウンリスト生成.
            cmdDB = New SqlCommand(M_DISP_ID & "_S3", conDB)
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlAppaNm_S.DataSource = dstOrders.Tables(0)
            Me.ddlAppaNm_S.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlAppaNm_S.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlAppaNm_S.DataBind()
            '--------------------------------
            '2014/05/15 後藤　ここから
            '--------------------------------
            'Me.ddlAppaNm_S.Items.Insert(0, " ")
            Me.ddlAppaNm_S.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/15 後藤　ここまで
            '--------------------------------

            '正常終了.
            mfGet_DropDownList_Sel = True

        Catch ex As Exception
            psMesBox(Me, sCnsErr_30008, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            mfGet_DropDownList_Sel = False
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, sCnsErr_00006, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                mfGet_DropDownList_Sel = False
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function
    ''' <summary>
    ''' 検索条件クリア処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()

        Me.ddlMakerKubun.SelectedIndex = 0
        Me.ddlWrkCls.ppDropDownList.SelectedIndex = 0
        Me.tftPartsCd.ppFromText = String.Empty
        Me.tftPartsCd.ppToText = String.Empty
        Me.txtPartsNm.ppText = String.Empty

    End Sub
    ''' <summary>
    ''' 検索処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data(ByVal ipstrMkr_cd As String,
                           ByVal ipstrWrk_Cls As String,
                           ByVal ipstrParts_Cd_f As String,
                           ByVal ipstrParts_Cd_t As String,
                           ByVal ipstrParts_Nm As String)

        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
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
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, sCnsErr_00005, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()
                Master.ppCount = "0"
            Else
                cmdDB = New SqlCommand(M_DISP_ID & "_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("mkr_cd", SqlDbType.NVarChar, ipstrMkr_cd))
                    .Add(pfSet_Param("wrk_cls", SqlDbType.NVarChar, ipstrWrk_Cls))
                    .Add(pfSet_Param("parts_cd_f", SqlDbType.NVarChar, ipstrParts_Cd_f))
                    .Add(pfSet_Param("parts_cd_t", SqlDbType.NVarChar, ipstrParts_Cd_t))
                    .Add(pfSet_Param("parts_nm", SqlDbType.NVarChar, ipstrParts_Nm))
                    .Add(pfSet_Param("disp_id", SqlDbType.NVarChar, M_DISP_ID))
                    .Add(pfSet_Param("default", SqlDbType.NVarChar, "2"))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '該当データなしの場合.
                    '--------------------------------
                    '2014/05/12 後藤　ここから
                    '--------------------------------
                    'psMesBox(Me, sCnsInfo_00007, clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後)
                    psMesBox(Me, "00004", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後)
                    '--------------------------------
                    '2014/05/12 後藤　ここまで
                    '--------------------------------
                    Master.ppCount = "0"
                Else
                    '閾値を超えた場合はメッセージを表示
                    If dstOrders.Tables(0).Rows(0)("MAXROW") > dstOrders.Tables(0).Rows.Count Then
                        psMesBox(Me, sCnsErr_30005, clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後,
                           dstOrders.Tables(0).Rows(0)("MAXROW"), dstOrders.Tables(0).Rows.Count.ToString)
                    End If
                    Master.ppCount = dstOrders.Tables(0).Rows(0)("MAXROW")
                End If

                '取得したデータをリストに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            End If

        Catch ex As Exception
            psMesBox(Me, sCnsErr_00004, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "部品マスタ詳細")
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
                psMesBox(Me, sCnsErr_00006, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub
    ''' <summary>
    ''' 追加処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msInsert_Data()

        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet
        Dim intRtn As Integer = 0
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            '開始ログ出力
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, sCnsErr_00005, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()
                Master.ppCount = "0"
            Else
                'トランザクション.
                Using conTrn = conDB.BeginTransaction
                    cmdDB = New SqlCommand(M_DISP_ID & "_I1", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = conTrn
                    With cmdDB.Parameters
                        .Add(pfSet_Param("mkr_cd", SqlDbType.NVarChar, Me.ddlMakerKubun_S.SelectedValue))              '会社コード
                        .Add(pfSet_Param("wrk_cls", SqlDbType.NVarChar, Me.ddlWrkCls_S.ppSelectedValue))               '作業分類
                        .Add(pfSet_Param("parts_cd", SqlDbType.NVarChar, Me.txtPartsCd_S.ppText))                      '部品コード        
                        .Add(pfSet_Param("parts_nm", SqlDbType.NVarChar, Me.txtPartsNm_S.ppText))                      '表示名称
                        .Add(pfSet_Param("mkrwrk_nm", SqlDbType.NVarChar, Me.txtMakerWrkNm_S.ppText))                  'メーカ作業名称
                        .Add(pfSet_Param("ngcwrk_nm", SqlDbType.NVarChar, Me.txtNgcWrkNm_S.ppText))                    'NGC作業名称
                        .Add(pfSet_Param("price11", SqlDbType.NVarChar, mfGetDBNull(Me.txtPrice11_S.ppText)))          '適用①・作業単価１
                        .Add(pfSet_Param("price12", SqlDbType.NVarChar, mfGetDBNull(Me.txtPrice12_S.ppText)))          '適用①・作業単価２
                        .Add(pfSet_Param("start_dt1", SqlDbType.NVarChar, mfGetDBNull(Me.dtbStartDt1_S.ppText)))       '適用①・適用開始日
                        .Add(pfSet_Param("price21", SqlDbType.NVarChar, mfGetDBNull(Me.txtPrice21_S.ppText)))          '適用②・作業単価１
                        .Add(pfSet_Param("price22", SqlDbType.NVarChar, mfGetDBNull(Me.txtPrice22_S.ppText)))          '適用②・作業単価２
                        .Add(pfSet_Param("start_dt2", SqlDbType.NVarChar, mfGetDBNull(Me.dtbStartDt2_S.ppText)))       '適用②・適用開始日
                        .Add(pfSet_Param("price31", SqlDbType.NVarChar, mfGetDBNull(Me.txtPrice31_S.ppText)))          '適用③・作業単価１
                        .Add(pfSet_Param("price32", SqlDbType.NVarChar, mfGetDBNull(Me.txtPrice32_S.ppText)))          '適用③・作業単価２
                        .Add(pfSet_Param("start_dt3", SqlDbType.NVarChar, mfGetDBNull(Me.dtbStartDt3_S.ppText)))       '適用③・適用開始日
                        .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, Me.ddlSystem_S.SelectedValue))               'システム名称
                        .Add(pfSet_Param("appa_cd", SqlDbType.NVarChar, Me.ddlAppaNm_S.SelectedValue))                 '機器名称
                        .Add(pfSet_Param("insert_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))                '登録者
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                    End With

                    '実行
                    cmdDB.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)

                    If intRtn <> 0 Then
                        psMesBox(Me, sCnsErr_00003, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "部品マスタ詳細")

                        'ロールバック
                        conTrn.Rollback()

                        Exit Sub
                    End If

                    'コミット
                    conTrn.Commit()

                End Using

                '完了メッセージ
                psMesBox(Me, sCnsInfo_00003, clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後, "部品マスタ詳細")

            End If

        Catch ex As Exception
            psMesBox(Me, sCnsErr_00003, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "部品マスタ詳細")
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
                psMesBox(Me, sCnsErr_00006, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            End If
            '終了ログ出力
            psLogEnd(Me)
        End Try
    End Sub
    ''' <summary>
    ''' 更新処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msUpdate_Data()

        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet
        Dim intRtn As Integer = 0
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            '開始ログ出力
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, sCnsErr_00005, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()
                Master.ppCount = "0"
            Else
                'トランザクション.
                Using conTrn = conDB.BeginTransaction
                    cmdDB = New SqlCommand(M_DISP_ID & "_U1", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = conTrn
                    With cmdDB.Parameters
                        .Add(pfSet_Param("mkr_cd", SqlDbType.NVarChar, Me.ddlMakerKubun_S.SelectedValue))              '会社コード
                        .Add(pfSet_Param("wrk_cls", SqlDbType.NVarChar, Me.ddlWrkCls_S.ppSelectedValue))               '作業分類
                        .Add(pfSet_Param("parts_cd", SqlDbType.NVarChar, Me.txtPartsCd_S.ppText))                      '部品コード        
                        .Add(pfSet_Param("parts_nm", SqlDbType.NVarChar, Me.txtPartsNm_S.ppText))                      '表示名称
                        .Add(pfSet_Param("mkrwrk_nm", SqlDbType.NVarChar, Me.txtMakerWrkNm_S.ppText))                  'メーカ作業名称
                        .Add(pfSet_Param("ngcwrk_nm", SqlDbType.NVarChar, Me.txtNgcWrkNm_S.ppText))                    'NGC作業名称
                        .Add(pfSet_Param("price11", SqlDbType.NVarChar, mfGetDBNull(Me.txtPrice11_S.ppText)))          '適用①・作業単価１
                        .Add(pfSet_Param("price12", SqlDbType.NVarChar, mfGetDBNull(Me.txtPrice12_S.ppText)))          '適用①・作業単価２
                        .Add(pfSet_Param("start_dt1", SqlDbType.NVarChar, mfGetDBNull(Me.dtbStartDt1_S.ppText)))       '適用①・適用開始日
                        .Add(pfSet_Param("price21", SqlDbType.NVarChar, mfGetDBNull(Me.txtPrice21_S.ppText)))          '適用②・作業単価１
                        .Add(pfSet_Param("price22", SqlDbType.NVarChar, mfGetDBNull(Me.txtPrice22_S.ppText)))          '適用②・作業単価２
                        .Add(pfSet_Param("start_dt2", SqlDbType.NVarChar, mfGetDBNull(Me.dtbStartDt2_S.ppText)))       '適用②・適用開始日
                        .Add(pfSet_Param("price31", SqlDbType.NVarChar, mfGetDBNull(Me.txtPrice31_S.ppText)))          '適用③・作業単価１
                        .Add(pfSet_Param("price32", SqlDbType.NVarChar, mfGetDBNull(Me.txtPrice32_S.ppText)))          '適用③・作業単価２
                        .Add(pfSet_Param("start_dt3", SqlDbType.NVarChar, mfGetDBNull(Me.dtbStartDt3_S.ppText)))       '適用③・適用開始日
                        .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, Me.ddlSystem_S.SelectedValue))               'システム名称
                        .Add(pfSet_Param("appa_cd", SqlDbType.NVarChar, Me.ddlAppaNm_S.SelectedValue))                 '機器名称
                        .Add(pfSet_Param("update_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))                '更新者
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                    End With

                    '実行
                    cmdDB.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, sCnsErr_00001, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "部品マスタ詳細")

                        'ロールバック
                        conTrn.Rollback()

                        Exit Sub
                    End If

                    'コミット
                    conTrn.Commit()

                End Using

                '完了メッセージ
                psMesBox(Me, sCnsInfo_00001, clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後, "部品マスタ詳細")

            End If

        Catch ex As Exception
            psMesBox(Me, sCnsErr_00001, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "部品マスタ詳細")
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
                psMesBox(Me, sCnsErr_00006, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            End If
            '終了ログ出力
            psLogEnd(Me)
        End Try
    End Sub
    ''' <summary>
    ''' 削除処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msDelete_Data()

        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet
        Dim intRtn As Integer = 0
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            '開始ログ出力
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, sCnsErr_00005, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()
                Master.ppCount = "0"
            Else
                'トランザクション.
                Using conTrn = conDB.BeginTransaction
                    cmdDB = New SqlCommand(M_DISP_ID & "_D1", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = conTrn
                    With cmdDB.Parameters
                        .Add(pfSet_Param("mkr_cd", SqlDbType.NVarChar, Me.ddlMakerKubun_S.SelectedValue)) '会社コード
                        .Add(pfSet_Param("wrk_cls", SqlDbType.NVarChar, Me.ddlWrkCls_S.ppSelectedValue))  '作業分類
                        .Add(pfSet_Param("parts_cd", SqlDbType.NVarChar, Me.txtPartsCd_S.ppText))         '部品コード        
                        .Add(pfSet_Param("update_userid", SqlDbType.NVarChar, Session(P_SESSION_USERID)))   '更新日
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                    End With

                    '実行
                    cmdDB.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, sCnsErr_00002, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "部品マスタ詳細")

                        'ロールバック
                        conTrn.Rollback()

                        Exit Sub
                    End If

                    'コミット
                    conTrn.Commit()

                End Using

                '完了メッセージ
                psMesBox(Me, sCnsInfo_00002, clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後, "部品マスタ詳細")

            End If

        Catch ex As Exception
            psMesBox(Me, sCnsErr_00002, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "部品マスタ詳細")
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
                psMesBox(Me, sCnsErr_00006, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            End If
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub
    ''' <summary>
    ''' 部品マスタ詳細クリア処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msPartsClearScreen()

        Me.ddlMakerKubun_S.SelectedIndex = 0
        Me.ddlWrkCls_S.ppDropDownList.SelectedIndex = 0
        Me.txtPartsCd_S.ppText = String.Empty
        Me.txtPartsNm_S.ppText = String.Empty
        Me.txtMakerWrkNm_S.ppText = String.Empty
        Me.txtNgcWrkNm_S.ppText = String.Empty
        Me.dtbStartDt1_S.ppText = String.Empty
        Me.txtPrice11_S.ppText = String.Empty
        Me.txtPrice12_S.ppText = String.Empty
        Me.dtbStartDt2_S.ppText = String.Empty
        Me.txtPrice21_S.ppText = String.Empty
        Me.txtPrice22_S.ppText = String.Empty
        Me.dtbStartDt3_S.ppText = String.Empty
        Me.txtPrice31_S.ppText = String.Empty
        Me.txtPrice32_S.ppText = String.Empty
        Me.ddlSystem_S.SelectedIndex = 0
        Me.ddlAppaNm_S.SelectedIndex = 0
        Me.lblInsertDt_S.Text = String.Empty
        Me.lblUpdateDt_S.Text = String.Empty

    End Sub
    ''' <summary>
    ''' 活性制御.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Private Sub msEnableChangeScreen(ByVal sender As Object)

        Select Case sender.ID
            Case "btnSearchRigth1" '検索.
                Me.ddlMakerKubun_S.Enabled = True
                Me.ddlWrkCls_S.ppEnabled = True
                Me.txtPartsCd_S.ppEnabled = True
                Me.btnClear.Enabled = True
                Me.btnInsert.Enabled = True
                Me.btnDelete.Enabled = False
                Me.btnUpdate.Enabled = False
            Case "btnClear"        'クリア
                Me.ddlMakerKubun_S.Enabled = True
                Me.ddlWrkCls_S.ppEnabled = True
                Me.txtPartsCd_S.ppEnabled = True
                Me.btnClear.Enabled = True
                Me.btnInsert.Enabled = True
                Me.btnDelete.Enabled = False
                Me.btnUpdate.Enabled = False
            Case "btnInsert"       '追加.
                Me.ddlMakerKubun_S.Enabled = True
                Me.ddlWrkCls_S.ppEnabled = True
                Me.txtPartsCd_S.ppEnabled = True
                Me.btnClear.Enabled = True
                Me.btnInsert.Enabled = True
                Me.btnDelete.Enabled = False
                Me.btnUpdate.Enabled = False
            Case "btnUpdate"       '更新.
                Me.ddlMakerKubun_S.Enabled = True
                Me.ddlWrkCls_S.ppEnabled = True
                Me.txtPartsCd_S.ppEnabled = True
                Me.btnClear.Enabled = True
                Me.btnInsert.Enabled = True
                Me.btnDelete.Enabled = False
                Me.btnUpdate.Enabled = False
            Case "btnDelete"       '削除.
                Me.ddlMakerKubun_S.Enabled = True
                Me.ddlWrkCls_S.ppEnabled = True
                Me.txtPartsCd_S.ppEnabled = True
                Me.btnClear.Enabled = True
                Me.btnInsert.Enabled = True
                Me.btnDelete.Enabled = False
                Me.btnUpdate.Enabled = False
            Case "grvList"          '選択.
                Me.ddlMakerKubun_S.Enabled = False
                Me.ddlWrkCls_S.ppEnabled = False
                Me.txtPartsCd_S.ppEnabled = False
                Me.btnClear.Enabled = True
                Me.btnInsert.Enabled = False
                Me.btnDelete.Enabled = True
                Me.btnUpdate.Enabled = True
        End Select


    End Sub
    ''' <summary>
    ''' 個別チェック.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheck_Error()

        '適応開始日の重複チェック.
        If Me.dtbStartDt1_S.ppText = Me.dtbStartDt2_S.ppText OrElse Me.dtbStartDt1_S.ppText = Me.dtbStartDt3_S.ppText Then
            Me.dtbStartDt1_S.psSet_ErrorNo("4001", "適応①・適応開始日", "適応開始日①～③の入力値と重複しない日付")
        End If


        If Me.dtbStartDt2_S.ppText <> String.Empty Then
            If Me.dtbStartDt2_S.ppText = Me.dtbStartDt3_S.ppText Then
                Me.dtbStartDt2_S.psSet_ErrorNo("4001", "適応②・適応開始日", "適応開始日①～③の入力値と重複しない日付")
            End If
        End If

    End Sub
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
#End Region

End Class