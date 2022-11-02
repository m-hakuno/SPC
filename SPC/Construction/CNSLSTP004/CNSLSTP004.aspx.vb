'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　進捗工事一覧
'*　ＰＧＭＩＤ：　CNSLSTP004
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.11.27　：　土岐
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'CNSLSTP004-001     2015/11/27      加賀　　　一覧表示項目、レイアウト変更
'CNSLSTP004-002     2015/11/27      加賀　　　該当件数表示位置の変更
'CNSLSTP004-003     2015/11/27      加賀　　　検索項目[設置区分][システム][工事種別]追加。ホールコード削除
'CNSLSTP004-004     2016/04/04      加賀　　　随時一覧の排他をかけないよう修正(親子同時工事が開けないくなる為)
'CNSLSTP004-005     2016/07/15      栗原      随時一覧の排他を復活、排他キーに双子店フラグを追加

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
Imports System.Data.SqlClient
'-----------------------------
'2014/04/21 土岐　ここから
'-----------------------------
'排他制御用
Imports SPC.ClsCMExclusive
'-----------------------------
'2014/04/21 土岐　ここまで
'-----------------------------
#End Region

Public Class CNSLSTP004
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
    Private Const M_DISP_ID = P_FUN_CNS & P_SCR_LST & P_PAGE & "004"
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
    ''' ページ初期化
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '表設定
        pfSet_GridView(Me.grvList, "CNSLSTP004", "CNSLSTP004_Header", Me.DivOut, 40, 10)

    End Sub

    ''' <summary>
    ''' ページロード
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            'ボタンアクションの設定
            AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
            AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click
            AddHandler btnReload.Click, AddressOf btnNow_Click

            If Not IsPostBack Then  '初回表示

                'プログラムＩＤ、画面名設定
                Master.Master.ppProgramID = M_DISP_ID
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

                'パンくずリスト設定
                Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                '「リロード」「クリア」ボタン押下時の検証を無効
                Master.ppRigthButton2.CausesValidation = False
                btnReload.CausesValidation = False

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                '都道府県、状況、システムドロップダウンリスト設定()
                msSetddlPre()
                msSetddlStatus()
                msSetddlSystem()    'CNSLSTP004-001

                ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

                '右上の該当件数非表示
                Master.ppCount_Visible = False

                '検索（初期表示）
                msGet_Data("0", "0",
                           DateTime.Today.ToString("yyyy/MM/dd"),
                           String.Empty,
                           String.Empty,
                           String.Empty,
                           String.Empty,
                           String.Empty,
                           String.Empty,
                           String.Empty,
                           String.Empty,
                           String.Empty,
                           "0",
                           "0",
                           "0",
                           "0",
                           "0",
                           "0",
                           "0",
                           "0",
                           "0",
                           "0",
                           "0",
                           "0",
                           "0",
                           "0",
                           "0",
                           "0",
                           "0",
                           "0",
                           "0",
                           "1",
                           M_DISP_ID,
                           "1",
                           "1")
            End If

        Catch ex As Exception

            'メッセージ出力
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    '---------------------------
    '2014/04/21 武 ここから
    '---------------------------
    ' ''' <summary>
    ' ''' ユーザー権限
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

    '    Select Case Session(P_SESSION_AUTH)
    '        Case "管理者"
    '        Case "SPC"
    '        Case "営業所"
    '        Case "NGC"
    '    End Select

    'End Sub
    '---------------------------
    '2014/04/21 武 ここまで
    '---------------------------

    ''' <summary>
    ''' グリッド データバインド時処理
    ''' </summary>
    ''' <remarks>ユーザー権限によるボタン制御、対象行の赤字表示処理</remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        Dim objdate As DateTime '[実施日時]格納用
        Dim strStts As String   '[最大進捗]格納用

        Try
            '行ループ
            For Each rowData As GridViewRow In grvList.Rows

                'ユーザー権限によるボタン制御
                Select Case Session(P_SESSION_AUTH)
                    Case "管理者"
                    Case "SPC"
                    Case "営業所"
                        rowData.Cells(1).Enabled = False
                    Case "NGC"
                        rowData.Cells(1).Enabled = False
                End Select

                '対象行の赤字表示処理
                If DateTime.TryParse(CType(rowData.FindControl("実施日時"), TextBox).Text, objdate) = True Then
                    '判定対象の値を設定＆格納
                    If CType(rowData.FindControl("工事種別_構成配信"), TextBox).Text.Trim = "" Then
                        '構成配信でない場合、実施日時を31分減算して判定
                        objdate = objdate.AddMinutes(-31)
                    End If
                    strStts = CType(rowData.FindControl("最大進捗"), TextBox).Text.Trim

                    '赤字表示判定
                    If objdate < DateTime.Now Then
                        Select Case strStts
                            Case "", "00", "01"
                                '全セル参照し、赤字表示
                                For Each cell As TableCell In rowData.Cells
                                    If TypeOf cell.Controls(0) Is TextBox Then
                                        DirectCast(cell.Controls(0), TextBox).ForeColor = Drawing.Color.Red '赤字設定
                                    End If
                                Next
                        End Select
                    End If
                End If
            Next

        Catch ex As Exception
            'メッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "一覧の生成")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try


    End Sub


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
        msClearSearch()
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

        If (Page.IsValid) Then
            '検索
            msGet_Data(msConvBool(cbxEstTmp.Checked), msConvBool(cbxEstSet.Checked),
                       Me.dftImplementationDt.ppFromText,
                       Me.dftImplementationDt.ppToText,
                       Me.tftTboxId.ppFromText,
                       Me.tftTboxId.ppToText,
                       Me.tftConsReqNo.ppFromText,
                       Me.tftConsReqNo.ppToText,
                       Me.ddlPreFrom.SelectedValue.ToString,
                       Me.ddlPreTo.SelectedValue.ToString,
                       Me.ddlSituation.SelectedValue,
                       Me.ddlSystem.SelectedValue,
                       msConvBool(cbxNew.Checked),
                       msConvBool(cbxExpansion.Checked),
                       msConvBool(cbxSomeRemoval.Checked),
                       msConvBool(cbxShopRelocation.Checked),
                       msConvBool(cbxAllRemoval.Checked),
                       msConvBool(cbxOnceRemoval.Checked),
                       msConvBool(cbxReInstallation.Checked),
                       msConvBool(cbxConChange.Checked),
                       msConvBool(cbxConDelivery.Checked),
                       msConvBool(cbxOther.Checked),
                       msConvBool(cbxVup.Checked),
                       msConvBool(cbxAll.Checked),
                       msConvBool(cbxDelay.Checked),
                       msConvBool(cbxSpare.Checked),
                       msConvBool(cbxVain.Checked),
                       msConvBool(cbxIns.Checked),
                       msConvBool(cbxTel.Checked),
                       msConvBool(cbxOth.Checked),
                       msConvBool(cbxOrgnz.Checked),
                       "0",
                       M_DISP_ID,
                       "0",
                       Me.ddlOutputOrder.SelectedValue)
        End If
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' リロードボタン押下処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnNow_Click(sender As Object, e As EventArgs)
        'ログ出力開始
        psLogStart(Me)
        '検索
        msGet_Data("0", "0",
                   DateTime.Today.ToString("yyyy/MM/dd"),
                   String.Empty,
                   String.Empty,
                   String.Empty,
                   String.Empty,
                   String.Empty,
                   String.Empty,
                   String.Empty,
                   String.Empty,
                   String.Empty,
                   "0",
                   "0",
                   "0",
                   "0",
                   "0",
                   "0",
                   "0",
                   "0",
                   "0",
                   "0",
                   "0",
                   "0",
                   "0",
                   "0",
                   "0",
                   "0",
                   "0",
                   "0",
                   "0",
                   "1",
                   M_DISP_ID,
                   "1",
                   "1")
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
        '工事進捗 参照更新 画面のパス
        Const CNSUPDP003 As String = "~/" & P_CNS & "/" &
            P_FUN_CNS & P_SCR_UPD & P_PAGE & "003" & "/" &
            P_FUN_CNS & P_SCR_UPD & P_PAGE & "003.aspx"
        Dim intIndex As Integer
        Dim rowData As GridViewRow
        Dim strKey(1) As String

        If e.CommandName <> "btnReference" And e.CommandName <> "btnUpdate" Then
            Exit Sub
        End If

        'ログ出力開始
        psLogStart(Me)

        intIndex = Convert.ToInt32(e.CommandArgument)    ' ボタン押下行のIndex
        rowData = grvList.Rows(intIndex)             ' ボタン押下行

        'セッション情報設定
        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text                     'パンくずリスト
        strKey(0) = CType(rowData.FindControl("工事依頼番号"), TextBox).Text        '詳細キー項目(工事依頼番号)
        strKey(1) = CType(rowData.FindControl("設置区分コード"), TextBox).Text      '詳細キー項目(設置区分)
        Session(P_KEY) = strKey
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)               'グループ番号

        Select Case e.CommandName
            Case "btnReference"     ' 参照
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
            Case "btnUpdate"        ' 更新
                '-----------------------------
                '2014/04/21 土岐　ここから
                '-----------------------------
                Dim strExclusiveDate As String = Nothing
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList

                '★ロック対象テーブル名の登録
                arTable_Name.Insert(0, "D24_CNST_SITU_DTL")
                arTable_Name.Insert(1, "D39_CNSTREQSPEC")
                '★ロックテーブルキー項目の登録(D39_CNST_NO)
                arKey.Insert(0, strKey(0))      '工事依頼番号


                'CNSLSTP004-005 双子/三つ子工事に限り排他制御
                If CType(rowData.FindControl("双子区分"), TextBox).Text.Trim = "1" OrElse CType(rowData.FindControl("双子区分"), TextBox).Text.Trim = "2" Then
                    arTable_Name.Insert(2, "D84_ANYTIME_LIST") 'CNSLSTP004-004
                    'CNSLSTP004-004
                    arKey.Insert(1, CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text)   'ＴＢＯＸＩＤ
                    arKey.Insert(2, CType(rowData.FindControl("実施日時"), TextBox).Text)       '工事日
                    'CNSLSTP004-004 END
                End If
                'CNSLSTP004-005 END


                '2014/05/30 間瀬 工事進捗でも随時集信のデータを操作しているのでデータがなくてもロックをかける
                ''本設置で全撤去または一時撤去の場合、随時一覧もロックする
                'If strKey(1) = "2" _
                '    And (CType(rowData.FindControl("工事種別_全撤去"), TextBox).Text = "●" _
                '        Or CType(rowData.FindControl("工事種別_一時撤去"), TextBox).Text = "●") Then
                '    '★ロック対象テーブル名の登録
                '    arTable_Name.Insert(1, "D84_ANYTIME_LIST")
                '★ロックテーブルキー項目の登録(D84_TBOXID, D84_CNST_DT)

                'End If


                '★排他情報確認処理(更新画面へ遷移)
                If clsExc.pfSel_Exclusive(strExclusiveDate _
                                 , Me _
                                 , Session(P_SESSION_IP) _
                                 , Session(P_SESSION_PLACE) _
                                 , Session(P_SESSION_USERID) _
                                 , Session(P_SESSION_SESSTION_ID) _
                                 , ViewState(P_SESSION_GROUP_NUM) _
                                 , P_FUN_CNS & P_SCR_UPD & P_PAGE & "003" _
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
                        objStack.GetMethod.Name, CNSUPDP003, strPrm, "TRANS")
        '■□■□結合試験時のみ使用予定□■□■
        '--------------------------------
        '2014/04/16 星野　ここまで
        '--------------------------------
        '別ブラウザ起動
        psOpen_Window(Me, CNSUPDP003)

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
        msClearSearch()

        Me.grvList.DataSource = New Object() {}
        Me.grvList.DataBind()
    End Sub

    ''' <summary>
    ''' 検索条件クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearSearch()

        '設置区分
        Me.cbxEstSet.Checked = False
        Me.cbxEstTmp.Checked = False

        Me.dftImplementationDt.ppFromText = String.Empty
        Me.dftImplementationDt.ppToText = String.Empty
        Me.tftTboxId.ppFromText = String.Empty
        Me.tftTboxId.ppToText = String.Empty
        Me.tftConsReqNo.ppFromText = String.Empty
        Me.tftConsReqNo.ppToText = String.Empty
        Me.ddlSystem.SelectedIndex = -1                     'システム

        Me.ddlPreFrom.SelectedValue = ""
        Me.ddlPreTo.SelectedValue = ""
        Me.ddlSituation.SelectedValue = ""
        Me.ddlOutputOrder.SelectedValue = "1"

        '工事種別
        Me.cbxNew.Checked = False              '工事種別新規
        Me.cbxSomeRemoval.Checked = False      '工事種別一部撤去
        Me.cbxAllRemoval.Checked = False       '工事種別全撤去
        Me.cbxReInstallation.Checked = False   '工事種別再設置
        Me.cbxConDelivery.Checked = False      '工事種別構成配信
        Me.cbxExpansion.Checked = False        '工事種別増設
        Me.cbxShopRelocation.Checked = False   '工事種別店舗移設
        Me.cbxOnceRemoval.Checked = False      '工事種別一時撤去
        Me.cbxConChange.Checked = False        '工事種別構成変更
        Me.cbxVup.Checked = False              'ＶＵＰ
        Me.cbxOther.Checked = False            '工事種別その他

        'トラブル関連
        Me.cbxAll.Checked = False       '発生工事全
        Me.cbxDelay.Checked = False     '工事遅延
        Me.cbxSpare.Checked = False     '予備機使用
        Me.cbxVain.Checked = False      '空振り工事
        Me.cbxIns.Checked = False       'INS
        Me.cbxTel.Checked = False       'TEL
        Me.cbxOth.Checked = False       'その他
        Me.cbxOrgnz.Checked = False     '構成

    End Sub

    ''' <summary>
    ''' 検索処理
    ''' </summary>
    ''' <param name="ipstrCnst_dt_f">実施日（開始）</param>
    ''' <param name="ipstrCnst_dt_t">実施日（終了）</param>
    ''' <param name="ipstrTboxid_f">ＴＢＯＸＩＤ（開始）</param>
    ''' <param name="ipstrTboxid_t">ＴＢＯＸＩＤ（終了）</param>
    ''' <param name="ipstrConst_no_f">工事依頼番号（開始）</param>
    ''' <param name="ipstrConst_no_t">工事依頼番号（終了）</param>
    ''' <param name="ipstrStare_cd_f">都道府県コード（開始）</param>
    ''' <param name="ipstrStare_cd_t">都道府県コード（終了）</param>
    ''' <param name="ipstrStatus_cd">進捗ステータスコード</param>
    ''' <param name="ipstrToday">初期表示/リロードフラグ（1：初期表示/リロード　1以外：通常）</param>
    ''' <param name="ipstrDisp_id">画面ＩＤ</param>
    ''' <param name="ipstrDefault">初期表示フラグ（1：初期　1以外：初期以外）</param>
    ''' <param name="ipstrSort">ソート順</param>
    ''' <remarks></remarks>
    Private Sub msGet_Data(ByVal ipstrEstTmp As String,
                           ByVal ipstrEstSet As String,
                           ByVal ipstrCnst_dt_f As String,
                           ByVal ipstrCnst_dt_t As String,
                           ByVal ipstrTboxid_f As String,
                           ByVal ipstrTboxid_t As String,
                           ByVal ipstrConst_no_f As String,
                           ByVal ipstrConst_no_t As String,
                           ByVal ipstrStare_cd_f As String,
                           ByVal ipstrStare_cd_t As String,
                           ByVal ipstrStatus_cd As String,
                           ByVal ipstrSystem_cd As String,
                           ByVal ipstrCnst_New As String,
                           ByVal ipstrCnst_Add As String,
                           ByVal ipstrCnst_PrtRmv As String,
                           ByVal ipstrCnst_ReLocate As String,
                           ByVal ipstrCnst_AllRmv As String,
                           ByVal ipstrCnst_TmpRmv As String,
                           ByVal ipstrCnst_ReSet As String,
                           ByVal ipstrCnst_ChngOrgnz As String,
                           ByVal ipstrCnst_DlvOrgnz As String,
                           ByVal ipstrCnst_Other As String,
                           ByVal ipstrCnst_Vup As String,
                           ByVal ipstrTrbl_All As String,
                           ByVal ipstrTrbl_Delay As String,
                           ByVal ipstrTrbl_Spare As String,
                           ByVal ipstrTrbl_Vain As String,
                           ByVal ipstrTrbl_Ins As String,
                           ByVal ipstrTrbl_Tel As String,
                           ByVal ipstrTrbl_Other As String,
                           ByVal ipstrTrbl_Orgnz As String,
                           ByVal ipstrToday As String,
                           ByVal ipstrDisp_id As String,
                           ByVal ipstrDefault As String,
                           ByVal ipstrSort As String
                           )
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing

        objStack = New StackFrame

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("CNSLSTP004_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("cnst_dt_f", SqlDbType.NVarChar, ipstrCnst_dt_f))
                    .Add(pfSet_Param("cnst_dt_t", SqlDbType.NVarChar, ipstrCnst_dt_t))
                    .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, ipstrTboxid_f))
                    .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, ipstrTboxid_t))
                    .Add(pfSet_Param("const_no_f", SqlDbType.NVarChar, ipstrConst_no_f))
                    .Add(pfSet_Param("const_no_t", SqlDbType.NVarChar, ipstrConst_no_t))
                    '.Add(pfSet_Param("hall_cd_f", SqlDbType.NVarChar, ipstrHall_cd_f))
                    '.Add(pfSet_Param("hall_cd_t", SqlDbType.NVarChar, ipstrHall_cd_t))
                    .Add(pfSet_Param("stare_cd_f", SqlDbType.NVarChar, ipstrStare_cd_f))
                    .Add(pfSet_Param("stare_cd_t", SqlDbType.NVarChar, ipstrStare_cd_t))
                    .Add(pfSet_Param("status_cd", SqlDbType.NVarChar, ipstrStatus_cd))
                    .Add(pfSet_Param("today", SqlDbType.NVarChar, ipstrToday))
                    .Add(pfSet_Param("disp_id", SqlDbType.NVarChar, ipstrDisp_id))
                    .Add(pfSet_Param("default", SqlDbType.NVarChar, ipstrDefault))
                    .Add(pfSet_Param("sort", SqlDbType.NVarChar, ipstrSort))
                    'CNSLSTP004-003
                    .Add(pfSet_Param("EstTmp", SqlDbType.NVarChar, ipstrEstTmp))        '仮設置
                    .Add(pfSet_Param("EstSet", SqlDbType.NVarChar, ipstrEstSet))        '本設置
                    .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ipstrSystem_cd))  'システム
                    .Add(pfSet_Param("H_NEW", SqlDbType.NVarChar, ipstrCnst_New))       '工事種別↓
                    .Add(pfSet_Param("H_PRT_REMOVE", SqlDbType.NVarChar, ipstrCnst_PrtRmv))
                    .Add(pfSet_Param("H_ALL_REMOVE", SqlDbType.NVarChar, ipstrCnst_AllRmv))
                    .Add(pfSet_Param("H_RESET", SqlDbType.NVarChar, ipstrCnst_ReSet))
                    .Add(pfSet_Param("H_DLV_ORGNZ", SqlDbType.NVarChar, ipstrCnst_DlvOrgnz))
                    .Add(pfSet_Param("H_ADD", SqlDbType.NVarChar, ipstrCnst_Add))
                    .Add(pfSet_Param("H_RELOCATE", SqlDbType.NVarChar, ipstrCnst_ReLocate))
                    .Add(pfSet_Param("H_TMP_REMOVE", SqlDbType.NVarChar, ipstrCnst_TmpRmv))
                    .Add(pfSet_Param("H_CHNG_ORGNZ", SqlDbType.NVarChar, ipstrCnst_ChngOrgnz))
                    .Add(pfSet_Param("VUP", SqlDbType.NVarChar, ipstrCnst_Vup))
                    .Add(pfSet_Param("H_OTH", SqlDbType.NVarChar, ipstrCnst_Other))
                    'CNSLSTP004-003 END

                    'トラブル関連
                    .Add(pfSet_Param("All", SqlDbType.NVarChar, ipstrTrbl_All))     '発生工事全
                    .Add(pfSet_Param("Delay", SqlDbType.NVarChar, ipstrTrbl_Delay)) '工事遅延
                    .Add(pfSet_Param("Spare", SqlDbType.NVarChar, ipstrTrbl_Spare)) '予備機使用
                    .Add(pfSet_Param("Vain", SqlDbType.NVarChar, ipstrTrbl_Vain))   '空振り工事
                    .Add(pfSet_Param("Ins", SqlDbType.NVarChar, ipstrTrbl_Ins))     'INS
                    .Add(pfSet_Param("Tel", SqlDbType.NVarChar, ipstrTrbl_Tel))     'TEL
                    .Add(pfSet_Param("Oth", SqlDbType.NVarChar, ipstrTrbl_Other))   'その他
                    .Add(pfSet_Param("Orgnz", SqlDbType.NVarChar, ipstrTrbl_Orgnz)) '構成
                End With

                'リストデータ取得
                '                cmdDB.CommandTimeout = 120
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    If ipstrToday = "1" Then
                        psMesBox(Me, "00010", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, ipstrCnst_dt_f)
                        'Master.ppCount = dstOrders.Tables(0).Rows.Count.ToString CNSLSTP004-002
                        Me.lblCount.Text = "0" 'CNSLSTP004-002
                    Else
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        'Master.ppCount = dstOrders.Tables(0).Rows.Count.ToString CNSLSTP004-002
                        Me.lblCount.Text = "0" 'CNSLSTP004-002
                    End If
                Else
                    '閾値を超えた場合はメッセージを表示
                    If dstOrders.Tables(0).Rows(0)("データ件数") > dstOrders.Tables(0).Rows.Count Then
                        psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                            dstOrders.Tables(0).Rows(0)("データ件数").ToString, dstOrders.Tables(0).Rows.Count.ToString)
                    End If
                    'Master.ppCount = dstOrders.Tables(0).Rows(0)("データ件数").ToString CNSLSTP004-002
                    Me.lblCount.Text = dstOrders.Tables(0).Rows(0)("データ件数").ToString 'CNSLSTP004-002
                End If

                '取得したデータをリストに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

                'グリッド編集処理
                'msGet_GRIDEdit() 'CNSLSTP004-001 DataBoundイベントに移動

            Catch ex As SqlException
                'SQLタイムアウト
                'メッセージ出力
                If ex.Number = -2 Then
                    'SQLタイムアウト
                    psMesBox(Me, "30010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【SQLタイムアウト】データ件数が多過ぎます。") 'データの表示に失敗しました。\nエラー内容：{0}
                Else
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "進捗工事一覧")
                End If

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Catch ex As Exception

                'メッセージ出力
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "進捗工事一覧")

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
            'メッセージ出力
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"
        End If
    End Sub

    ''' <summary>
    ''' ブール値→(0 or 1)変換 'CNSLSTP004-003
    ''' </summary>
    ''' <param name="ipBool">変換するブール値(True:1,False:0)</param>
    Private Function msConvBool(ByVal ipBool As Boolean) As Integer

        If ipBool = Nothing Then
            msConvBool = Nothing
        Else
            If ipBool = True Then
                msConvBool = "1"
            Else
                msConvBool = "0"
            End If
        End If
    End Function


    ''' <summary>
    ''' ドロップダウンリスト設定（進捗ステータスマスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlStatus()

        Dim conDB As SqlConnection = Nothing    'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing      'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'DB接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                cmdDB = New SqlCommand("ZCMPSEL015", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("duty_cd", SqlDbType.NVarChar, "21"))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'ドロップダウンリスト設定
                Me.ddlSituation.Items.Clear()
                Me.ddlSituation.DataSource = dstOrders.Tables(0)
                Me.ddlSituation.DataTextField = "進捗ステータス名"
                Me.ddlSituation.DataValueField = "進捗ステータス"
                Me.ddlSituation.DataBind()
                Me.ddlSituation.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "進捗ステータスマスタ一覧取得")
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
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（都道府県）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlPre()

        Dim conDB As SqlConnection = Nothing    'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing      'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'DB接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                cmdDB = New SqlCommand("ZCMPSEL052", conDB)

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'ドロップダウンリスト設定
                Me.ddlPreFrom.Items.Clear()
                Me.ddlPreFrom.DataSource = dstOrders.Tables(0)
                Me.ddlPreFrom.DataTextField = "都道府県名"
                Me.ddlPreFrom.DataValueField = "都道府県コード"
                Me.ddlPreFrom.DataBind()
                Me.ddlPreTo.Items.Clear()
                Me.ddlPreTo.DataSource = dstOrders.Tables(0)
                Me.ddlPreTo.DataTextField = "都道府県名"
                Me.ddlPreTo.DataValueField = "都道府県コード"
                Me.ddlPreTo.DataBind()

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "進捗ステータスマスタ一覧取得")
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
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定(システム) CNSLSTP004-001
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlSystem()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        Dim clsDataConnect As New ClsCMDataConnect
        Dim clsMst As New ClsMSTCommon

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZMSTSEL004", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                objDs.Tables(0).DefaultView.Sort = "ＴＢＯＸシステムコード"

                'ドロップダウンリスト設定(TBOX機種マスタ)(削除済みデータも取得)
                Me.ddlSystem.Items.Clear()
                Me.ddlSystem.DataSource = objDs.Tables(0)
                Me.ddlSystem.DataTextField = "ＴＢＯＸリスト"
                Me.ddlSystem.DataValueField = "ＴＢＯＸシステムコード"
                Me.ddlSystem.DataBind()
                Me.ddlSystem.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ機種マスタ一覧取得")
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

    ' ''' <summary>
    ' ''' データバインド時処理 'CNSLSTP004-001 CSSにより設定される為不要
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

    '    For Each rowData As GridViewRow In grvList.Rows
    '        If (rowData.RowIndex Mod 2) = 1 Then
    '            CType(rowData.FindControl("設置区分"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
    '            CType(rowData.FindControl("実施日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
    '            CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
    '            CType(rowData.FindControl("工事依頼番号"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
    '            CType(rowData.FindControl("ホール名"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
    '            CType(rowData.FindControl("状況"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
    '            CType(rowData.FindControl("都道府県コード"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
    '            CType(rowData.FindControl("都道府県名"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
    '            CType(rowData.FindControl("ＴＢＯＸ機種"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
    '            CType(rowData.FindControl("工事種別_新規"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
    '            CType(rowData.FindControl("工事種別_増設"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
    '            CType(rowData.FindControl("工事種別_一部撤去"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
    '            CType(rowData.FindControl("工事種別_移設"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
    '            CType(rowData.FindControl("工事種別_全撤去"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
    '            CType(rowData.FindControl("工事種別_一時撤去"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
    '            CType(rowData.FindControl("工事種別_再設置"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
    '            CType(rowData.FindControl("工事種別_構成変更"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
    '            CType(rowData.FindControl("工事種別_構成配信"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
    '            CType(rowData.FindControl("VERUP"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
    '            CType(rowData.FindControl("工事種別_その他"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
    '            'CType(rowData.FindControl("削除有無"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
    '        Else
    '            CType(rowData.FindControl("設置区分"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
    '            CType(rowData.FindControl("実施日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
    '            CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
    '            CType(rowData.FindControl("工事依頼番号"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
    '            CType(rowData.FindControl("ホール名"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
    '            CType(rowData.FindControl("状況"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
    '            CType(rowData.FindControl("都道府県コード"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
    '            CType(rowData.FindControl("都道府県名"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
    '            CType(rowData.FindControl("ＴＢＯＸ機種"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
    '            CType(rowData.FindControl("工事種別_新規"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
    '            CType(rowData.FindControl("工事種別_増設"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
    '            CType(rowData.FindControl("工事種別_一部撤去"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
    '            CType(rowData.FindControl("工事種別_移設"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
    '            CType(rowData.FindControl("工事種別_全撤去"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
    '            CType(rowData.FindControl("工事種別_一時撤去"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
    '            CType(rowData.FindControl("工事種別_再設置"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
    '            CType(rowData.FindControl("工事種別_構成変更"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
    '            CType(rowData.FindControl("工事種別_構成配信"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
    '            CType(rowData.FindControl("VERUP"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
    '            CType(rowData.FindControl("工事種別_その他"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
    '            'CType(rowData.FindControl("削除有無"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
    '        End If
    '    Next

    'End Sub

    'CNSLSTP004-001
    ' ''' <summary>
    ' ''' グリッド編集処理
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Function msGet_GRIDEdit()
    '    'ヘッダーの値を設定
    '    Dim strHeadNm As String() = New String() _
    '                                {"参照", "更新", "設置区分", "実施日時", "ＴＢＯＸＩＤ", "工事依頼番号", _
    '                                 "ホール名", "状況", "都道府県名", "ＴＢＯＸ機種", "工事種別_新規", _
    '                                 "工事種別_増設", "工事種別_一部撤去", "工事種別_移設", "工事種別_全撤去", "工事種別_一時撤去", _
    '                                 "工事種別_再設置", "工事種別_構成変更", "工事種別_構成配信", "VERUP", "工事種別_その他", "削除有無"}

    '    Try
    '        If Not IsPostBack Then
    '            For clm As Integer = 2 To 21
    '                grvList.Columns(clm).HeaderText = strHeadNm(clm)
    '            Next
    '        End If

    '        For Each rowData As GridViewRow In grvList.Rows
    '            If CType(rowData.FindControl(strHeadNm(18)), TextBox).Text.Trim = "●" Then
    '                If DateTime.TryParse(CType(rowData.FindControl(strHeadNm(3)), TextBox).Text, "1900/01/01") = True Then
    '                    If DateTime.Parse(CType(rowData.FindControl(strHeadNm(3)), TextBox).Text) _
    '                    < DateTime.Now And _
    '                    (CType(rowData.FindControl("最新進捗"), TextBox).Text.Trim = "" OrElse _
    '                    CType(rowData.FindControl("最新進捗"), TextBox).Text.Trim = "00" OrElse _
    '                    CType(rowData.FindControl("最新進捗"), TextBox).Text.Trim = "01") Then
    '                        For clmCnt As Integer = 2 To 21
    '                            CType(rowData.FindControl(grvList.Columns(clmCnt).HeaderText), TextBox).ForeColor = Drawing.Color.Red
    '                        Next
    '                    End If
    '                End If
    '            Else
    '                If DateTime.TryParse(CType(rowData.FindControl(strHeadNm(3)), TextBox).Text, "1900/01/01") = True Then
    '                    If DateTime.Parse(CType(rowData.FindControl(strHeadNm(3)), TextBox).Text).AddMinutes(-31) _
    '                    < DateTime.Now And _
    '                    (CType(rowData.FindControl("最新進捗"), TextBox).Text.Trim = "" OrElse _
    '                    CType(rowData.FindControl("最新進捗"), TextBox).Text.Trim = "00" OrElse _
    '                    CType(rowData.FindControl("最新進捗"), TextBox).Text.Trim = "01") Then
    '                        For clmCnt As Integer = 2 To 21
    '                            CType(rowData.FindControl(grvList.Columns(clmCnt).HeaderText), TextBox).ForeColor = Drawing.Color.Red
    '                        Next
    '                    End If
    '                End If
    '            End If
    '        Next

    '    Catch ex As Exception

    '        psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
    '        '--------------------------------
    '        '2014/04/14 星野　ここから
    '        '--------------------------------
    '        'ログ出力
    '        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
    '                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
    '        '--------------------------------
    '        '2014/04/14 星野　ここまで
    '        '--------------------------------

    '    End Try
    '    Return True

    'End Function
    'CNSLSTP004-001 END

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
