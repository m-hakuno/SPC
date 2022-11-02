'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜監視＞
'*　処理名　　：　DLL変更依頼一覧
'*　ＰＧＭＩＤ：　DLCLSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.01.25　：　高松
'*  修　正　　：　2017.11.17　：　伯野　データセットの後始末を追加
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'DLCLSTP001-001     2015/05/13      武       画面レイアウト変更（備考追加） DLCLSTP001_U1 DLCLSTP001_I1   
'DLCLSTP001-002     2015/05/27      武       工事データ選択時ドロップダウンリストに項目を設定し、非活性にする処理を追加
'DLCLSTP001_003     2015/06/09      武       クリア処理追加
'DLCLSTP001_004     2015/08/20      武       検索時クリア処理を行わないよう修正
'DLCLSTP001_005     2016/01/22      伯野     検索条件にリトライを追加
'DLCLSTP001_006     2016/02/22      伯野     検索条件の日付範囲を他の画面と合わせる(設定日時、戻し日時、登録日付、更新日付)
'DLCLSTP001_007     2016/02/26      伯野     削除案件を編集エリアに表示可能とする
'DLCLSTP001_008     2016/03/16      伯野     削除ボタン押下時に備考を保存する
'DLCLSTP001_009     2016/09/09　　　武       選択ボタン押下時にクリア処理追加
'DLCLSTP001_010     2016/10/12      武       自動リトライ機能制御ボタン追加
'DLCLSTP001_011     2016/10/11　　　加賀     一覧に履歴ボタンを追加
'DLCLSTP001_012     2016/10/20　　　武       初期表示に検索処理を追加
'DLCLSTP001_013     2016/10/20　　　加賀     帳票の表示順を変更,工事PASS、深夜PASS、深夜終夜除外
'DLCLSTP001_014     2016/10/26　　　武　     設定依頼内容が会員管理会社設定の時、戻し日時を非活性にする。
'DLCLSTP001_015     2016/10/27　　　加賀     設定状態、戻し状態の表示整形
'DLCLSTP001_016     2016/10/27　　　加賀     設定依頼の整合性チェック追加
'DLCLSTP001_017     2016/11/04　　　加賀     ﾘﾄﾗｲのリロードバグ修正
'DLCLSTP001_018     2016/11/04　　　加賀     登録内容初期化処理修正
'DLCLSTP001_019     2016/11/07　　　加賀     排他処理修正
'DLCLSTP001_020     2016/11/07　　　加賀     設定/戻し情報がソート時に崩れるバグを修正
'DLCLSTP001_021     2016/11/08　　　加賀     特別運用モードSW選択時のバグを修正
'DLCLSTP001_022     2017/11/17　　　伯野     データセットの後始末を追加

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive      '排他制御用
#End Region

Public Class DLCLSTP001

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
    Private Const M_DISP_ID = P_FUN_DLC & P_SCR_LST & P_PAGE & "001"
    Private Const M_SETSTART = "0"  '設定始まり
    Private Const M_RETSTART = "4"  '戻し始まり
    Private Const M_SETCDSTART = "0"  '設定始まり
    Private Const M_SETCDEND = "1"  '設定完了
    Private Const M_RETCDSTART = "0"  '戻し始まり
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
    Dim strRetSetf As String   '設定、戻しの判断フラグ
    Dim strSetcd As String     '設定コード
    Dim strRetcd As String     '戻しコード

    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom
    Dim clsExc As New ClsCMExclusive
    Dim clsCmm As New ClsCMCommon

    Dim mstbRetryNo As New StringBuilder

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page_Init
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        '表設定
        '        ClsCMCommon.pfSet_GridView(Me.grvList, M_DISP_ID, "DLCLSTP001_Header", Me.DivOut, 40, 10)
        ClsCMCommon.pfSet_GridView(Me.grvList, M_DISP_ID, 40, 10)

    End Sub

    ''' <summary>
    ''' Page_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        objStack = New StackFrame

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btn_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btn_Click
        AddHandler Master.ppRigthButton3.Click, AddressOf btn_Click
        AddHandler Master.Master.ppLeftButton1.Click, AddressOf btn_Click
        AddHandler Master.Master.ppLeftButton2.Click, AddressOf btn_Click
        'DLCLSTP001_010
        AddHandler Master.Master.ppLeftButton3.Click, AddressOf btn_Click
        'DLCLSTP001_010 END
        AddHandler btnInsert.Click, AddressOf btn_Click
        AddHandler btnUpdate.Click, AddressOf btn_Click
        AddHandler btnDelete.Click, AddressOf btn_Click
        AddHandler btnClear.Click, AddressOf btn_Click
        'TBOXIDを入力した場合
        AddHandler Me.txtTboxID_Input.ppTextBox.TextChanged, AddressOf ctl_Change
        AddHandler Me.btnReload.Click, AddressOf btn_Click
        Me.txtTboxID_Input.ppTextBox.AutoPostBack = True
        AddHandler Me.ddlSettei_input.SelectedIndexChanged, AddressOf dll_Change
        Me.ddlSettei_input.AutoPostBack = True

        ViewState("txtInsDay_From") = txtInsDay_From.ppText & txtInsTim_From.ppHourText & txtInsTim_From.ppMinText
        ViewState("txtInsDay_To") = txtInsDay_To.ppText & txtInsTim_To.ppHourText & txtInsTim_To.ppMinText            '戻し日時(TO)
        ViewState("txtUpdDay_From") = txtUpdDay_From.ppText & txtUpdTim_From.ppHourText & txtUpdTim_From.ppMinText           '戻し日時(TO)
        ViewState("txtUpdDay_To") = txtUpdDay_To.ppText & txtUpdTim_To.ppHourText & txtUpdTim_To.ppMinText          '戻し日時(TO)
        ViewState("ddlDutyCD") = ddlDutyCD.SelectedValue             '戻し日時(TO)
        'DLCLSTP001-005
        'ViewState("ddlRetrySel") = ddlRetrySel.SelectedValue
        If ViewState("mstbRetryNo") Is Nothing Then
            ViewState("mstbRetryNo") = mstbRetryNo
        Else
            mstbRetryNo = ViewState("mstbRetryNo")
        End If
        'Me.btnReload.Enabled = False
        'Me.btnReload.Visible = False
        'DLCLSTP001-005
        'DLCLSTP001-001
        Me.txtNotes.ppTextBox.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txtNotes.ppMaxLength & """);")
        Me.txtNotes.ppTextBox.Attributes.Add("onChange", "lenCheck(this,""" & Me.txtNotes.ppMaxLength & """);")

        'Me.txtTboxID_Input.ppTextBox.ReadOnly = False
        'txtTboxID_Input.ppTextBox.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF")


        If Not IsPostBack Then  '初回表示

            Try

                'プログラムＩＤ、画面名設定
                Master.Master.ppProgramID = M_DISP_ID
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

                'セッション変数有り無し
                If Session(P_SESSION_BCLIST) Is Nothing Then

                    'システムエラー
                    Throw New Exception

                End If

                'パンくずリスト設定
                Master.Master.ppBcList_Text = ClsCMCommon.pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                '画面初期化処理
                msClearScreen()
                ddlSettei_input.Enabled = True

                'ドロップダウンリストの初期設定を取得
                If Not msGetDLLData(0) Then

                    'システムエラー
                    Throw New Exception

                End If

                '検索項目の設定
                ms_SetView_st()

                'セッション変数の取得/明細情報検索
                If Not ms_GetSession() Then

                    'システムエラー
                    Throw New Exception

                End If

                'DLCLSTP001_012

                ViewState("ddlRetrySel") = ddlRetrySel.SelectedValue

                ViewState("btnreload") = "1"

                '検索条件の保管
                ms_SetView_st()
                'DLL変更依頼一覧の取得
                ms_GetDLLhenko("Reload")
                'DLCLSTP001_012 END

                'リトライボタン更新
                msGetRetryBtn()

            Catch ex As Exception

                'システムエラーメッセージ
                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                ClsCMCommon.psClose_Window(Me)
                Exit Sub

            End Try

        End If

    End Sub


#End Region

    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        If ViewState("txtInsDay_From") Is Nothing Then
        Else
            If ViewState("txtInsDay_From").ToString.Length > 9 Then
                txtInsDay_From.ppText = ViewState("txtInsDay_From").ToString.Substring(0, 10)
            End If
            If ViewState("txtInsDay_From").ToString.Length > 10 Then
                txtInsTim_From.ppHourText = ViewState("txtInsDay_From").ToString.Substring(10, 2)
            End If
            If ViewState("txtInsDay_From").ToString.Length > 12 Then
                txtInsTim_From.ppMinText = ViewState("txtInsDay_From").ToString.Substring(12, 2)
            End If
        End If
        If ViewState("txtInsDay_To") Is Nothing Then
        Else
            If ViewState("txtInsDay_To").ToString.Length > 9 Then
                txtInsDay_To.ppText = ViewState("txtInsDay_To").ToString.Substring(0, 10)
            End If
            If ViewState("txtInsDay_To").ToString.Length > 10 Then
                txtInsTim_To.ppHourText = ViewState("txtInsDay_To").ToString.Substring(10, 2)
            End If
            If ViewState("txtInsDay_To").ToString.Length > 12 Then
                txtInsTim_To.ppMinText = ViewState("txtInsDay_To").ToString.Substring(12, 2)
            End If
        End If
        If ViewState("txtUpdDay_From") Is Nothing Then
        Else
            If ViewState("txtUpdDay_From").ToString.Length > 9 Then
                txtUpdDay_From.ppText = ViewState("txtUpdDay_From").ToString.Substring(0, 10)
            End If
            If ViewState("txtUpdDay_From").ToString.Length > 10 Then
                txtUpdTim_From.ppHourText = ViewState("txtUpdDay_From").ToString.Substring(10, 2)
            End If
            If ViewState("txtUpdDay_From").ToString.Length > 12 Then
                txtUpdTim_From.ppMinText = ViewState("txtUpdDay_From").ToString.Substring(12, 2)
            End If
        End If
        If ViewState("txtUpdDay_To") Is Nothing Then
        Else
            If ViewState("txtUpdDay_To").ToString.Length > 9 Then
                txtUpdDay_To.ppText = ViewState("txtUpdDay_To").ToString.Substring(0, 10)
            End If
            If ViewState("txtUpdDay_To").ToString.Length > 10 Then
                txtUpdTim_To.ppHourText = ViewState("txtUpdDay_To").ToString.Substring(10, 2)
            End If
            If ViewState("txtUpdDay_To").ToString.Length > 12 Then
                txtUpdTim_To.ppMinText = ViewState("txtUpdDay_To").ToString.Substring(12, 2)
            End If
        End If

        'If ViewState("ddlRetrySel") Is Nothing Then
        'Else
        '    ddlDutyCD.SelectedValue = ViewState("ddlDutyCD")
        'End If

        'DLCLSTP001-005
        If ViewState("ddlRetrySel") Is Nothing Then
        Else
            ddlRetrySel.SelectedValue = ViewState("ddlRetrySel")
            If Me.ddlRetrySel.SelectedValue = "1" OrElse Me.ddlRetrySel.SelectedValue = "2" Then
                Master.Master.ppLeftButton2.Enabled = True
                '                Me.btnReload.Enabled = False
            Else
                Master.Master.ppLeftButton2.Enabled = False
                Me.btnReload.Enabled = True
            End If
        End If
        '        Me.btnReload.Enabled = True

        ViewState("mstbRetryNo") = mstbRetryNo
        'DLCLSTP001-005

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
            Case "NGC"
        End Select

    End Sub

#Region "ボタンクリックイベント"

    ''' <summary>
    ''' ボタンクリック
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub btn_Click(sender As Object, e As EventArgs)

        objStack = New StackFrame

        Try

            Select Case sender.text

                Case "検索条件クリア"

                    '入力項目の初期化
                    Me.txtTboxID.ppFromText = Nothing
                    Me.txtTboxID.ppToText = Nothing
                    Me.ddlCenterCls.SelectedIndex = 0
                    Me.ddlSystem.SelectedIndex = 0
                    Me.txtHallName.ppText = Nothing
                    Me.ddlSetteiirai.SelectedIndex = 0
                    Me.txtSetteiDayFrom.ppText = Nothing
                    Me.txtSetteiTimeFrom.ppHourText = Nothing
                    Me.txtSetteiTimeFrom.ppMinText = Nothing
                    Me.txtSetteiDayTo.ppText = Nothing
                    Me.txtSetteiTimeTo.ppHourText = Nothing
                    Me.txtSetteiTimeTo.ppMinText = Nothing
                    Me.txtReturnDayFrom.ppText = Nothing
                    Me.txtReturnTimeFrom.ppHourText = Nothing
                    Me.txtReturnTimeFrom.ppMinText = Nothing
                    Me.txtReturnDayTO.ppText = Nothing
                    Me.txtReturnTimeTO.ppHourText = Nothing
                    Me.txtReturnTimeTO.ppMinText = Nothing
                    Me.txtInsDay_From.ppText = ""
                    Me.txtInsTim_From.ppHourText = ""
                    Me.txtInsTim_From.ppMinText = ""
                    ViewState("txtInsDay_From") = ""
                    Me.txtInsDay_To.ppText = ""
                    Me.txtInsTim_To.ppHourText = ""
                    Me.txtInsTim_To.ppMinText = ""
                    ViewState("txtInsDay_To") = ""
                    Me.txtUpdDay_From.ppText = ""
                    Me.txtUpdTim_From.ppHourText = ""
                    Me.txtUpdTim_From.ppMinText = ""
                    ViewState("txtUpdDay_From") = ""
                    Me.txtUpdDay_To.ppText = ""
                    Me.txtUpdTim_To.ppHourText = ""
                    Me.txtUpdTim_To.ppMinText = ""
                    ViewState("txtUpdDay_To") = ""
                    Me.ddlDutyCD.SelectedIndex = 0 '.SelectedValue = ""
                    ViewState("ddlDutyCD") = ""
                    'DLCLSTP001-005
                    Me.ddlRetrySel.SelectedIndex = 0
                    ViewState("ddlRetrySel") = ""
                    ViewState("mstbRetryNo") = ""
                    If mstbRetryNo Is Nothing Then
                    Else
                        mstbRetryNo.Clear()
                    End If
                    'DLCLSTP001-005
                    'ボタンの活性化を行う
                    Me.btnUpdate.Enabled = False
                    Me.btnDelete.Enabled = False
                    Me.btnInsert.Enabled = True
                    'DLCLSTP001-002
                    ''ドロップダウンリストの初期設定を取得
                    'If Not msGetDLLData() Then

                    '    'システムエラー
                    '    Throw New Exception

                    'End If
                    'DLCLSTP001-002 END
                Case "検索"

                    Try
                        '設定日時、戻し日時の検証チェック
                        ms_cheackVlid()

                        If (Page.IsValid) Then

                            'DLCLSTP001-005
                            If mstbRetryNo Is Nothing Then
                            Else
                                mstbRetryNo.Clear()
                            End If
                            'DLCLSTP001-005

                            '検索条件の保管
                            ms_SetView_st()

                            'DLL変更依頼一覧の取得
                            ViewState("btnreload") = "0"
                            ms_GetDLLhenko()

                            'リトライボタン更新
                            msGetRetryBtn()
                        End If

                    Catch ex As Exception

                        'グリッドビューの初期化
                        Me.grvList.DataSource = New DataTable
                        Me.grvList.DataBind()

                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")

                        '処理終了
                        Exit Sub

                    End Try

                Case "リトライのリロード"

                    Try
                        '設定日時、戻し日時の検証チェック
                        '                        ms_cheackVlid()

                        '検索条件の保管
                        '                        ms_SetView_st()

                        'DLL変更依頼一覧の取得
                        ms_GetDLLhenko("RetryReload")

                        'リトライボタン更新
                        msGetRetryBtn()

                    Catch ex As Exception

                        'グリッドビューの初期化
                        Me.grvList.DataSource = New DataTable
                        Me.grvList.DataBind()
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")

                        '処理終了
                        Exit Sub

                    End Try

                Case "印刷"

                    Dim dirPath As String = ms_GetDirpath()
                    'Dim strView_st_out() As String = Me.ViewState(P_KEY)           'キー項目の設定 
                    Dim dtPDF As DataTable = New DataTable                         'PDF用データテーブル
                    Dim rpt As New DLCREP001

                    If Not dirPath Is Nothing Then

                        Dim fileName As String = Nothing
                        Dim CrateDate As String = Date.Now.ToString("yyyyMMddHHmmss")

                        'ファイル名作成
                        fileName = "運用モード変更一覧_" + CrateDate

                        '帳票のDataTable編集
                        ms_GetPdfDatatable(dtPDF)

                        '出力データ件数確認
                        If dtPDF.Rows.Count = 0 Then
                            '該当するデータが存在しません。
                            psMesBox(Me, "00007", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                            Exit Sub
                        End If

                        'Active Reports(帳票 運用モード変更一覧)の起動
                        Try

                            ClsCMCommon.psPrintPDF(Me, rpt, dtPDF, fileName)

                        Catch ex As Exception

                            '帳票の出力に失敗
                            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "運用モード変更一覧")
                            'ログ出力
                            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
                            '処理終了
                            Exit Sub

                        End Try

                    Else

                        '処理終了
                        Exit Sub

                    End If

                Case "登録"

                    If Me.lblHallName_Input.Text = String.Empty Then

                        'ＴＢＯＸＩＤが間違ってる
                        Me.txtTboxID_Input.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")

                    End If


                    '設定時間、戻し時間の設定
                    If Not mfChkRetSet() Then
                        '確認時に異常終了
                        Exit Sub
                    End If

                    '会員サービスコードの入力チェック
                    If ddlSettei_input.SelectedValue.Split(":")(0).ToString = "13" Then
                        msInput_Check()
                    End If

                    If (Page.IsValid) Then

                        Try
                            'DLCLSTP001_016
                            If check_SetCd() = False Then
                                Exit Sub
                            End If
                            'DLCLSTP001_016 END

                            '同一TBOXIDで設定変更依頼中のデータ有無確認
                            If Not mfJudgeTboxid() Then
                                Exit Select
                            End If

                            '設定の開始判断(戻り開始、設定開始)
                            msJudgeRetSet()

                            '登録処理
                            ms_InsDLLhenko()

                            '再描画
                            ViewState("btnreload") = "1"
                            ms_GetDLLhenko("Reload")

                            'リトライボタン更新
                            msGetRetryBtn()

                            ''項目の初期化
                            'DLCLSTP001-003
                            msClear()

                            '排他情報削除
                            If Not Me.Master.Master.ppExclusiveDate = String.Empty Then

                                clsExc.pfDel_Exclusive(Me,
                                                Session(P_SESSION_SESSTION_ID),
                                                Me.Master.Master.ppExclusiveDate)

                                Me.Master.Master.ppExclusiveDate = String.Empty

                            End If
                            '--2014/04/15 中川　ここまで

                        Catch ex As Exception

                            'ログ出力
                            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
                            '処理終了
                            Exit Sub

                        End Try

                    End If

                Case "変更"

                    If Me.lblHallName_Input.Text = String.Empty Then

                        'ＴＢＯＸＩＤが間違ってる
                        Me.txtTboxID_Input.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")

                    End If

                    '設定時間、戻し時間の設定
                    If Not mfChkRetSet() Then
                        '確認時に異常終了
                        Exit Sub
                    End If

                    '会員サービスコードの入力チェック
                    If ddlSettei_input.SelectedValue.Split(":")(0).ToString = "13" Then
                        msInput_Check()
                    End If

                    If (Page.IsValid) Then

                        Try
                            '同一TBOXIDで設定変更依頼中のデータ有無確認
                            If Not mfJudgeTboxid("Update") Then
                                Exit Select
                            End If

                            '変更処理
                            ms_UpdDLLhenko()

                            '再描画
                            ViewState("btnreload") = "1"
                            ms_GetDLLhenko("Reload")

                            'リトライボタン更新
                            msGetRetryBtn()

                            '更新項目の初期化
                            'DLCLSTP001-003
                            msClear()

                            '排他情報削除 'DLCLSTP001_019
                            If Not Me.Master.Master.ppExclusiveDate = String.Empty Then

                                clsExc.pfDel_Exclusive(Me,
                                                Session(P_SESSION_SESSTION_ID),
                                                Me.Master.Master.ppExclusiveDate)

                                Me.Master.Master.ppExclusiveDate = String.Empty

                            End If

                        Catch ex As Exception
                            'ログ出力
                            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
                        End Try

                    End If

                Case "削除"

                    If (Page.IsValid) Then

                        Try

                            '削除処理
                            ms_DelDLLhenko()

                            '更新項目の初期化
                            'DLCLSTP001-003
                            msClear()

                            '排他情報削除
                            If Not Me.Master.Master.ppExclusiveDate = String.Empty Then

                                clsExc.pfDel_Exclusive(Me,
                                                Session(P_SESSION_SESSTION_ID),
                                                Me.Master.Master.ppExclusiveDate)

                                Me.Master.Master.ppExclusiveDate = String.Empty

                            End If
                            '--2014/04/15 中川　ここまで

                            '再描画
                            ViewState("btnreload") = "1"
                            ms_GetDLLhenko("Reload")

                            'リトライボタン更新
                            msGetRetryBtn()

                        Catch ex As Exception
                            'ログ出力
                            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
                        End Try

                    End If
                Case "クリア"
                    'DLCLSTP001_003
                    msClear()

                    '排他情報削除 'DLCLSTP001_019
                    If Not Me.Master.Master.ppExclusiveDate = String.Empty Then

                        clsExc.pfDel_Exclusive(Me,
                                        Session(P_SESSION_SESSTION_ID),
                                        Me.Master.Master.ppExclusiveDate)

                        Me.Master.Master.ppExclusiveDate = String.Empty

                    End If

                Case "リトライ"

                    msbatchRetry()

                Case "リロード"

                    Try
                        ViewState("btnreload") = "1"

                        'DLL変更依頼一覧の取得
                        ms_GetDLLhenko("Reload")

                        'リトライボタン更新
                        msGetRetryBtn()

                    Catch ex As Exception

                        'グリッドビューの初期化
                        Me.grvList.DataSource = New DataTable
                        Me.grvList.DataBind()

                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")

                        '処理終了
                        Exit Sub

                    End Try


                    'DLCLSTP001_010
                Case ViewState("RetryBtn")

                    msAutoRetrySW()
                    'DLCLSTP001_010 END

            End Select

        Catch ex As Exception

            'システムエラー
            psMesBox(Me, "99999", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '処理終了
            Exit Sub

        End Try

    End Sub

    ' ''' <summary>
    ' ''' リロードボタン押下時
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Private Sub BtnReload_Click(sender As Object, e As EventArgs) Handles BtnReload.Click

    '    Try
    '        Dim strSettei_dt As String = Nothing
    '        Dim strReturn_dt As String = Nothing

    '        '設定日時、戻し日時の検証チェック
    '        ms_cheackVlid()

    '        If (Page.IsValid) Then

    '            'DLCLSTP001-003
    '            'DLCLSTP001_004
    '            'msClear()
    '            'DLCLSTP001_004 END
    '            ''--------------------------------
    '            ''2014/04/14 高松　ここから
    '            ''--------------------------------
    '            ''更新項目の初期化
    '            'Me.txtTboxID_Input.ppText = Nothing
    '            'Me.lblCenterCls_Input.Text = String.Empty
    '            'Me.lblSystem_Input.Text = String.Empty
    '            'Me.lblHallName_Input.Text = String.Empty
    '            'Me.ddlSettei_input.SelectedIndex = 0
    '            'Me.txtStteiDay_input.ppText = Nothing
    '            'Me.txtStteiTime_input.ppHourText = Nothing
    '            'Me.txtStteiTime_input.ppMinText = Nothing
    '            'Me.txtReturnDay_Input.ppText = Nothing
    '            'Me.txtReturnTime_Input.ppHourText = Nothing
    '            'Me.txtReturnTime_Input.ppMinText = Nothing
    '            'Me.txtIraisya.ppText = String.Empty
    '            'Me.txtSrvCd1.ppText = String.Empty
    '            'Me.txtSrvCd2.ppText = String.Empty
    '            ''DLCLSTP001-001
    '            'Me.txtNotes.ppText = String.Empty
    '            ''DLCLSTP001-001 END

    '            ''更新、削除ボタンの非活性化
    '            'Me.btnUpdate.Enabled = False
    '            'Me.btnDelete.Enabled = False
    '            'Me.btnInsert.Enabled = True
    '            'Me.btnClear.Enabled = True
    '            ''--------------------------------
    '            ''2014/04/14 高松　ここまで
    '            ''--------------------------------
    '            ''DLCLSTP001-002
    '            ''ドロップダウンリストの初期設定を取得
    '            'If Not msGetDLLData() Then

    '            '    'システムエラー
    '            '    Throw New Exception

    '            'End If
    '            ''DLCLSTP001-002 END
    '            'DLCLSTP001-003 END
    '            '検索条件の保管
    '            ms_SetView_st()

    '            'DLL変更依頼一覧の取得
    '            ms_GetDLLhenko()

    '        End If

    '    Catch ex As Exception

    '        'グリッドビューの初期化
    '        Me.grvList.DataSource = New DataTable
    '        Me.grvList.DataBind()
    '        '--------------------------------
    '        '2014/04/14 星野　ここから
    '        '--------------------------------
    '        'ログ出力
    '        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
    '                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
    '        '--------------------------------
    '        '2014/04/14 星野　ここまで
    '        '--------------------------------

    '        '処理終了
    '        Exit Sub

    '    End Try
    'End Sub

    ''' <summary>
    ''' グリッドビュー選択イベント
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        Select Case e.CommandName
            Case "btnSelect"
            Case "btnRetry"
            Case "btnHistory"   'DLCLSTP001_011
            Case Else
                Exit Sub
        End Select

        Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    'ボタン押下行のIndex
        Dim rowData As GridViewRow = grvList.Rows(intIndex)             'ボタン押下行
        Dim strView_st_in(10 - 1) As String                             'ビューステート登録用
        Dim result As Boolean = False
        objStack = New StackFrame

        Select Case e.CommandName

            Case "btnSelect"     '選択

                Try
                    '--2014/04/15 中川　ここから
                    '排他制御用変数
                    Dim strExclusiveDate As String = Nothing
                    Dim arTable_Name As New ArrayList
                    Dim arKey As New ArrayList

                    '排他情報削除
                    If Not Me.Master.Master.ppExclusiveDateDtl = String.Empty Then

                        clsExc.pfDel_Exclusive(Me,
                                        Session(P_SESSION_SESSTION_ID),
                                        Me.Master.Master.ppExclusiveDateDtl)

                        Me.Master.Master.ppExclusiveDateDtl = String.Empty

                    End If

                    'ロック対象テーブル名の登録
                    arTable_Name.Insert(0, "D47_DLLSEND")

                    'ロックテーブルキー項目の登録
                    arKey.Insert(0, CType(rowData.FindControl("依頼番号"), TextBox).Text)   '依頼番号
                    arKey.Insert(1, CType(rowData.FindControl("明細番号"), TextBox).Text)   '明細番号

                    '排他情報確認処理
                    If clsExc.pfSel_Exclusive(strExclusiveDate,
                                       Me,
                                       Session(P_SESSION_IP),
                                       Session(P_SESSION_PLACE),
                                       Session(P_SESSION_USERID),
                                       Session(P_SESSION_SESSTION_ID),
                                       ViewState(P_SESSION_GROUP_NUM),
                                       M_DISP_ID,
                                       arTable_Name,
                                       arKey) = 0 Then

                        '登録年月日時刻(明細)に登録
                        Me.Master.Master.ppExclusiveDate = strExclusiveDate

                    Else
                        '排他ロック中
                        Exit Sub

                    End If
                    '--2014/04/15 中川　ここまで

                    'DLCLSTP001_009
                    'クリア処理
                    msClear()
                    'DLCLSTP001_009

                    '更新項目へ設定
                    Me.txtTboxID_Input.ppText = CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text
                    Me.lblCenterCls_Input.Text = CType(rowData.FindControl("センタ区分名"), TextBox).Text
                    Me.lblSystem_Input.Text = CType(rowData.FindControl("システム"), TextBox).Text
                    Me.lblHallName_Input.Text = CType(rowData.FindControl("ホール名"), TextBox).Text
                    If CType(rowData.FindControl("設定日時"), TextBox).Text <> "" Then
                        Me.txtStteiDay_input.ppText = Date.Parse(CType(rowData.FindControl("設定日時"), TextBox).Text).ToString("yyyy/MM/dd")
                        Me.txtStteiTime_input.ppHourText = Date.Parse(CType(rowData.FindControl("設定日時"), TextBox).Text).ToString("HH")
                        Me.txtStteiTime_input.ppMinText = Date.Parse(CType(rowData.FindControl("設定日時"), TextBox).Text).ToString("mm")
                    Else
                        Me.txtStteiDay_input.ppText = ""
                        Me.txtStteiTime_input.ppHourText = ""
                        Me.txtStteiTime_input.ppMinText = ""
                    End If
                    If CType(rowData.FindControl("戻し日時"), TextBox).Text <> "" Then
                        Me.txtReturnDay_Input.ppText = Date.Parse(CType(rowData.FindControl("戻し日時"), TextBox).Text).ToString("yyyy/MM/dd")
                        Me.txtReturnTime_Input.ppHourText = Date.Parse(CType(rowData.FindControl("戻し日時"), TextBox).Text).ToString("HH")
                        Me.txtReturnTime_Input.ppMinText = Date.Parse(CType(rowData.FindControl("戻し日時"), TextBox).Text).ToString("mm")
                    Else
                        Me.txtReturnDay_Input.ppText = ""
                        Me.txtReturnTime_Input.ppHourText = ""
                        Me.txtReturnTime_Input.ppMinText = ""
                    End If
                    Me.txtIraisya.ppText = CType(rowData.FindControl("設定依頼者"), TextBox).Text
                    'DLCLSTP001-002
                    'ドロップダウンリストの設定
                    Dim intSelIndex As Integer = 0
                    'ドロップダウンリストの初期設定を取得
                    If Not msGetDLLData(1) Then

                        'システムエラー
                        Throw New Exception

                    End If
                    intSelIndex = ms_GetSelectIndex(CType(rowData.FindControl("設定依頼内容"), TextBox).Text.Replace(Environment.NewLine, String.Empty).Replace(Environment.NewLine, String.Empty))
                    If Not intSelIndex > Me.ddlSettei_input.Items.Count Then
                        Me.ddlSettei_input.SelectedIndex = intSelIndex
                        Me.ddlSettei_input.Enabled = True
                    Else
                        intSelIndex = 0
                        For intcnt As Integer = 0 To ViewState("ddlSetteiirai").rows.count - 1
                            '削除データ
                            If ViewState("ddlSetteiirai").rows(intcnt).item("M62_WRK_NM").ToString = CType(rowData.FindControl("設定依頼内容"), TextBox).Text.Replace(Environment.NewLine, String.Empty) Then
                                Me.ddlSettei_input.Items(0).Text = ViewState("ddlSetteiirai").rows(intcnt).item("M62_WRK_NM").ToString
                                Me.ddlSettei_input.Items(0).Value = ViewState("ddlSetteiirai").rows(intcnt).item("Column1").ToString
                                Me.ddlSettei_input.SelectedIndex = 0
                                Me.ddlSettei_input.Enabled = False
                                Exit For
                            Else
                                intSelIndex += 1
                            End If
                        Next
                        If intSelIndex = ViewState("ddlSetteiirai").rows.count Then
                            '工事データ
                            Me.ddlSettei_input.Items(0).Text = CType(rowData.FindControl("設定依頼内容"), TextBox).Text.Replace(Environment.NewLine, "").Replace(Environment.NewLine, String.Empty)
                            Me.ddlSettei_input.Items(0).Value = ("99:" + CType(rowData.FindControl("設定依頼内容"), TextBox).Text.Replace(Environment.NewLine, "").Replace(Environment.NewLine, String.Empty) + ":通常運用　→　特別運用:特別運用　→　通常運用")
                            Me.ddlSettei_input.SelectedIndex = 0
                            Me.ddlSettei_input.Enabled = False
                        End If
                    End If
                    'DLCLSTP001-002 END
                    If Me.ddlSettei_input.SelectedValue.Split(":")(0).ToString = "13" Then
                        Me.txtSrvCd1.ppText = CType(rowData.FindControl("設定情報"), TextBox).Text.Split("：")(1)
                        Me.txtSrvCd2.ppText = CType(rowData.FindControl("戻し情報"), TextBox).Text.Split("：")(1)
                        Me.ddlSettei_input.Enabled = True
                    End If
                    '--------------------------------
                    '2014/04/14 高松　ここから
                    '--------------------------------
                    'ドロップダウンリストの値で活性非活性を切り替える
                    Dim split() As String = Me.ddlSettei_input.SelectedValue.ToString.Split(":")

                    If split Is Nothing Then
                        Exit Sub
                    End If
                    'チェックボックスリストの設定
                    'If CType(rowData.FindControl("精算機変更"), TextBox).Text = "1" Then

                    '    Me.ChkSeisanki.Items(0).Selected = True

                    'End If

                    'If CType(rowData.FindControl("精算機戻し"), TextBox).Text = "1" Then

                    '    Me.ChkSeisanki.Items(1).Selected = True

                    'End If
                    'DLCLSTP001-001
                    Me.txtNotes.ppText = CType(rowData.FindControl("エフエス向け特記事項"), TextBox).Text.Replace(Environment.NewLine, String.Empty)
                    'DLCLSTP001-001 END

                    Select Case split(0)
                        Case "02", "03"  '深夜営業、終夜営業

                            Me.ChkSeisanki.Enabled = True

                            'チェックボックスリストの設定
                            If CType(rowData.FindControl("精算機変更"), TextBox).Text = "1" Then

                                Me.ChkSeisanki.Items(0).Selected = True

                            End If

                            If CType(rowData.FindControl("精算機戻し"), TextBox).Text = "1" Then

                                Me.ChkSeisanki.Items(1).Selected = True

                            End If

                        Case Else

                            Me.ChkSeisanki.Enabled = False
                            Me.ChkSeisanki.Items(0).Selected = False
                            Me.ChkSeisanki.Items(1).Selected = False

                    End Select

                    'DLCLSTP001_014
                    If split(0) = "13" Then
                        txtSrvCd1.ppEnabled = True
                        txtSrvCd2.ppEnabled = True
                        txtReturnDay_Input.ppEnabled = False
                        txtReturnTime_Input.ppEnabled = False
                    Else
                        txtSrvCd1.ppText = String.Empty
                        txtSrvCd2.ppText = String.Empty
                        txtSrvCd1.ppEnabled = False
                        txtSrvCd2.ppEnabled = False
                        txtReturnDay_Input.ppEnabled = True
                        txtReturnTime_Input.ppEnabled = True
                    End If
                    'DLCLSTP001_014 END


                    '更新、削除に必要な情報を設定
                    strView_st_in(0) = CType(rowData.FindControl("依頼番号"), TextBox).Text
                    strView_st_in(1) = CType(rowData.FindControl("ホールコード"), TextBox).Text
                    strView_st_in(2) = CType(rowData.FindControl("センタ区分名"), TextBox).Text
                    strView_st_in(3) = CType(rowData.FindControl("ＴＢＯＸ機種コード"), TextBox).Text
                    strView_st_in(4) = CType(rowData.FindControl("現状モード"), TextBox).Text
                    strView_st_in(5) = CType(rowData.FindControl("エフエス向け特記事項"), TextBox).Text

                    'ビューステートに設定
                    Me.ViewState.Add("変更削除情報", strView_st_in)

                    'Me.txtTboxID_Input.ppTextBox.ReadOnly = True
                    'txtTboxID_Input.ppTextBox.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC")
                    Me.txtTboxID_Input.ppEnabled = False

                    ddlSettei_input.Enabled = False

                    '更新、削除ボタンの活性化
                    Me.btnUpdate.Enabled = True
                    Me.btnDelete.Enabled = True

                    '登録ボタンの非活性化
                    Me.btnInsert.Enabled = False                        '更新、削除ボタンの非活性化
                    Me.btnClear.Enabled = True

                    'DLCLSTP001_007
                    If CType(rowData.FindControl("削除"), TextBox).Text.Trim = "●" Then
                        Me.btnInsert.Enabled = False
                        Me.btnUpdate.Enabled = False
                        Me.btnDelete.Enabled = False
                    End If
                    'DLCLSTP001_007

                    'DLCLSTP001_010
                    ViewState("SEQ_NO") = CType(rowData.FindControl("明細番号"), TextBox).Text
                    'DLCLSTP001_010 END
                Catch ex As Exception

                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    '処理終了
                    Exit Sub

                End Try

            Case "btnRetry"     'リトライ

                Try
                    '排他制御用変数
                    Dim strExclusiveDate As String = Nothing
                    Dim arTable_Name As New ArrayList
                    Dim arKey As New ArrayList

                    '排他情報削除
                    If Not Me.Master.Master.ppExclusiveDateDtl = String.Empty Then

                        clsExc.pfDel_Exclusive(Me,
                                        Session(P_SESSION_SESSTION_ID),
                                        Me.Master.Master.ppExclusiveDateDtl)

                        Me.Master.Master.ppExclusiveDateDtl = String.Empty

                    End If

                    'ロック対象テーブル名の登録
                    arTable_Name.Insert(0, "D47_DLLSEND")

                    'ロックテーブルキー項目の登録
                    arKey.Insert(0, CType(rowData.FindControl("依頼番号"), TextBox).Text)   '依頼番号
                    arKey.Insert(1, CType(rowData.FindControl("明細番号"), TextBox).Text)   '明細番号

                    '排他情報確認処理
                    If clsExc.pfSel_Exclusive(strExclusiveDate,
                                       Me,
                                       Session(P_SESSION_IP),
                                       Session(P_SESSION_PLACE),
                                       Session(P_SESSION_USERID),
                                       Session(P_SESSION_SESSTION_ID),
                                       ViewState(P_SESSION_GROUP_NUM),
                                       M_DISP_ID,
                                       arTable_Name,
                                       arKey) = 0 Then

                        '登録年月日時刻(明細)に登録
                        Me.Master.Master.ppExclusiveDate = strExclusiveDate

                    Else
                        '排他ロック中
                        Exit Sub

                    End If
                    '--2014/04/15 中川　ここまで

                    '登録データの取得
                    Dim strCNTL_NO As String = Nothing
                    Dim strSEQNO As String = Nothing
                    Dim conDB As SqlConnection = Nothing
                    Dim sqlCmd As SqlCommand = Nothing
                    Dim objDS As New DataSet
                    Dim objTrans As SqlTransaction
                    Dim blnRetIns As Boolean = False
                    strCNTL_NO = CType(rowData.FindControl("依頼番号"), TextBox).Text.Replace(Environment.NewLine, "")   '依頼番号
                    strSEQNO = CType(rowData.FindControl("明細番号"), TextBox).Text   '明細番号

                    If clsDataConnect.pfOpen_Database(conDB) Then

                        objTrans = conDB.BeginTransaction

                        sqlCmd = New SqlCommand("DLCLSTP001_S7", conDB)
                        'DLL変更依頼　取得
                        With sqlCmd.Parameters
                            'パラメータ設定
                            .Add(ClsCMDataConnect.pfSet_Param("prm_CNTL_NO", SqlDbType.NVarChar, strCNTL_NO))                'TBOXID(FROM)
                            .Add(ClsCMDataConnect.pfSet_Param("prm_SEQNO", SqlDbType.NVarChar, strSEQNO))                'TBOXID(TO)
                        End With
                        'データ取得
                        sqlCmd.Transaction = objTrans
                        objDS = clsDataConnect.pfGet_DataSet(sqlCmd)

                        If objDS.Tables.Count > 0 Then
                            If objDS.Tables(0).Rows.Count > 0 Then
                                '新規でＤＬＬ変更情報を追加する                               電文エラー　　　　　　　　　　　　　　　　　　　　　　　　　　　　　正常終了　　　　　　　　　　　　　　　　　　　　　　　　　テスト待ち　　　　　　　　　　　　　　　　　　　　　　　　　未処理
                                If objDS.Tables(0).Rows(0).Item("CNG_RET_DENBUN").ToString = "DNBN_NG" OrElse (objDS.Tables(0).Rows(0).Item("CNG_STTS").ToString <> "0" And objDS.Tables(0).Rows(0).Item("CNG_STTS").ToString <> "4" And objDS.Tables(0).Rows(0).Item("CNG_STTS").ToString <> "Z") Then
                                    '変更時に失敗していたら変更でリトライ
                                    'ＩＮＳ処理
                                    Try
                                        Dim strRetryCnt As String = ""
                                        If objDS.Tables(0).Rows(0).Item("D47_RETCNT") Is DBNull.Value Then
                                            strRetryCnt = "01"
                                        Else
                                            strRetryCnt = (Integer.Parse(objDS.Tables(0).Rows(0).Item("D47_RETCNT")) + 1).ToString("D2")
                                        End If
                                        Dim strNewCntl_No As String = ""
                                        If objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Length = 10 Then
                                            strNewCntl_No = objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Substring(0, 7)
                                        ElseIf objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Length = 15 Then
                                            strNewCntl_No = objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Substring(0, 12)
                                        Else
                                            strNewCntl_No = objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString
                                        End If
                                        sqlCmd = Nothing
                                        sqlCmd = New SqlCommand("DLCLSTP001_I2", conDB)
                                        With sqlCmd.Parameters
                                            'パラメータ設定
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_dllsend_no", SqlDbType.NVarChar, strNewCntl_No & "R" & strRetryCnt)) '管理番号
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_seqno", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SEQNO").ToString))       '明細番号
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_nl_flg", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NL_FLG").ToString))     'ＮＬ区分
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_req", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_REQ").ToString))           '依頼方法
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_set_cd", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SET_CD").ToString))     '作業内容コード
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_set_nm", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SET_NM").ToString))     '作業内容
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_tboxid", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_TBOXID").ToString))     'ＴＢＯＸＩＤ
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_hall_cd", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_HALL_CD").ToString))    'ホールコード
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_hall_nm", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_HALL_NM").ToString))    'ホール名
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_tboxclass_cd", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_TBOXCLASS_CD").ToString)) 'ＴＢＯＸ機種コード
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_tboxclass_nm", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_TBOXCLASS_NM").ToString)) 'ＴＢＯＸ機種
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_spmodest_dt", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SPMODEST_DT").ToString)) '特別運用モード変更日時
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_spmodest_cd", SqlDbType.NVarChar, "0")) '特別運用変更内容コード
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_nmmodest_dt", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NMMODEST_DT").ToString)) '通常運用モード戻し日時
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_nmmodest_cd", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NMMODEST_CD").ToString)) '通常運用変更内容コード
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_setreq_nm", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SETREQ_NM").ToString))  '設定依頼者
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_set_info", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SET_INFO").ToString))   '設定情報
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_ret_info", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_RET_INFO").ToString))   '戻し情報
                                            If Me.ChkSeisanki.Enabled Then
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_adjmachine_chg", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_ADJMACHINE_CHG").ToString)) '精算機変更
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_adjmachine_rtn", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_ADJMACHINE_RTN").ToString)) '精算機戻し
                                            Else
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_adjmachine_chg", SqlDbType.NVarChar, "0"))                                '精算機変更
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_adjmachine_rtn", SqlDbType.NVarChar, "0"))                                '精算機戻し
                                            End If
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_sendend_flg", SqlDbType.NVarChar, "2"))                                       '案件終了フラグ
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_insert_usr", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_INSERT_USR").ToString)) '登録者
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_proctime_cls", SqlDbType.NVarChar, "0")) '処理時間区分
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_RETRY_CNT", SqlDbType.NVarChar, strRetryCnt)) 'リトライ回数
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_MODE_CD", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_MODE_CD").ToString)) 'モードコード
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_SETRET_FLG", SqlDbType.NVarChar, "")) '入り戻し依頼フラグ
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_CNST_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_CNST_FLG").ToString)) '工事有無
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_DNIGHT_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_DNIGHT_FLG").ToString)) '深夜営業有無
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_CNST_NM", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_CNST_NM").ToString)) '工事区分名
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_CNST_CLS", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_CNST_CLS").ToString)) '工事区分
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_SET_MSG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SET_MSG").ToString)) '設定依頼メッセージ
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_NOTETEXT", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NOTETEXT").ToString)) '備考
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_SPMODESND_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SPMODESND_FLG").ToString)) '特別運用送信フラグ
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_NMMODESND_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NMMODESND_FLG").ToString)) '通常運用送信フラグ
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_SPMODERCV_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SPMODERCV_FLG").ToString)) '特別運用結果フラグ
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_NMMODERCV_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NMMODERCV_FLG").ToString)) '通常運用結果フラグ
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_STATUS_CD", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_STATUS_CD").ToString)) '進捗状況
                                        End With
                                        sqlCmd.Transaction = objTrans
                                        'コマンドタイプ設定(ストアド)
                                        sqlCmd.CommandType = CommandType.StoredProcedure
                                        sqlCmd.ExecuteNonQuery()
                                        blnRetIns = True
                                        'コミット
                                        mstbRetryNo.Append("'" & strNewCntl_No & "',")
                                        '                                        trans.Commit()
                                        '登録が正常終了
                                        '                                        psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼一覧")
                                    Catch ex As SqlException
                                        '登録に失敗
                                        psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼　リトライ登録")
                                        'ログ出力
                                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
                                        'ロールバック
                                        objTrans.Rollback()
                                        '処理終了
                                        Throw ex
                                    Catch ex As Exception
                                        'システムエラー
                                        psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼　リトライ登録")
                                        'ログ出力
                                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
                                        'ロールバック
                                        objTrans.Rollback()
                                        '処理終了
                                        Throw ex
                                    End Try
                                Else
                                    '変更済でＤＬＬ変更情報を追加する                             電文エラー　　　　　　　　　　　　　　　　　　　　　　　　　　　　　正常終了　　　　　　　　　　　　　　　　　　　　　　　　　テスト待ち　　　　　　　　　　　　　　　　　　　　　　　　　未処理
                                    If objDS.Tables(0).Rows(0).Item("RTN_RET_DENBUN").ToString = "DNBN_NG" OrElse (objDS.Tables(0).Rows(0).Item("RTN_STTS").ToString <> "0" And objDS.Tables(0).Rows(0).Item("RTN_STTS").ToString <> "4" And objDS.Tables(0).Rows(0).Item("RTN_STTS").ToString <> "Z") Then
                                        '戻し時に失敗していたら戻しでリトライ
                                        Try 'ＩＮＳ処理
                                            Dim strRetryCnt As String = ""
                                            If objDS.Tables(0).Rows(0).Item("D47_RETCNT") Is DBNull.Value Then
                                                strRetryCnt = "01"
                                            Else
                                                strRetryCnt = (Integer.Parse(objDS.Tables(0).Rows(0).Item("D47_RETCNT")) + 1).ToString("D2")
                                            End If
                                            Dim strNewCntl_No As String = ""
                                            If objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Length = 10 Then
                                                strNewCntl_No = objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Substring(0, 7)
                                            ElseIf objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Length = 15 Then
                                                strNewCntl_No = objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Substring(0, 12)
                                            Else
                                                strNewCntl_No = objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString
                                            End If
                                            sqlCmd = Nothing
                                            sqlCmd = New SqlCommand("DLCLSTP001_I2", conDB)
                                            With sqlCmd.Parameters
                                                'パラメータ設定
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_dllsend_no", SqlDbType.NVarChar, strNewCntl_No & "R" & strRetryCnt)) '管理番号
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_seqno", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SEQNO").ToString))       '明細番号
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_nl_flg", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NL_FLG").ToString))     'ＮＬ区分
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_req", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_REQ").ToString))           '依頼方法
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_set_cd", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SET_CD").ToString))     '作業内容コード
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_set_nm", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SET_NM").ToString))     '作業内容
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_tboxid", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_TBOXID").ToString))     'ＴＢＯＸＩＤ
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_hall_cd", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_HALL_CD").ToString))    'ホールコード
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_hall_nm", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_HALL_NM").ToString))    'ホール名
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_tboxclass_cd", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_TBOXCLASS_CD").ToString)) 'ＴＢＯＸ機種コード
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_tboxclass_nm", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_TBOXCLASS_NM").ToString)) 'ＴＢＯＸ機種
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_spmodest_dt", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SPMODEST_DT").ToString)) '特別運用モード変更日時
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_spmodest_cd", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SPMODEST_CD").ToString)) '特別運用変更内容コード
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_nmmodest_dt", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NMMODEST_DT").ToString)) '通常運用モード戻し日時
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_nmmodest_cd", SqlDbType.NVarChar, "0")) '通常運用変更内容コード
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_setreq_nm", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SETREQ_NM").ToString))  '設定依頼者
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_set_info", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SET_INFO").ToString))   '設定情報
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_ret_info", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_RET_INFO").ToString))   '戻し情報
                                                If Me.ChkSeisanki.Enabled Then
                                                    .Add(ClsCMDataConnect.pfSet_Param("prm_adjmachine_chg", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_ADJMACHINE_CHG").ToString)) '精算機変更
                                                    .Add(ClsCMDataConnect.pfSet_Param("prm_adjmachine_rtn", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_ADJMACHINE_RTN").ToString)) '精算機戻し
                                                Else
                                                    .Add(ClsCMDataConnect.pfSet_Param("prm_adjmachine_chg", SqlDbType.NVarChar, "0"))                                '精算機変更
                                                    .Add(ClsCMDataConnect.pfSet_Param("prm_adjmachine_rtn", SqlDbType.NVarChar, "0"))                                '精算機戻し
                                                End If
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_sendend_flg", SqlDbType.NVarChar, "02"))                                       '案件終了フラグ
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_insert_usr", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_INSERT_USR").ToString)) '登録者
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_proctime_cls", SqlDbType.NVarChar, "4")) '処理時間区分
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_RETRY_CNT", SqlDbType.NVarChar, strRetryCnt)) 'リトライ回数
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_MODE_CD", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_MODE_CD").ToString)) 'モードコード
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_SETRET_FLG", SqlDbType.NVarChar, "0")) '入り戻し依頼フラグ
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_CNST_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_CNST_FLG").ToString)) '工事有無
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_DNIGHT_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_DNIGHT_FLG").ToString)) '深夜営業有無
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_CNST_NM", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_CNST_NM").ToString)) '工事区分名
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_CNST_CLS", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_CNST_CLS").ToString)) '工事区分
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_SET_MSG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SET_MSG").ToString)) '設定依頼メッセージ
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_NOTETEXT", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NOTETEXT").ToString)) '備考
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_SPMODESND_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SPMODESND_FLG").ToString)) '特別運用送信フラグ
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_NMMODESND_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NMMODESND_FLG").ToString)) '通常運用送信フラグ
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_SPMODERCV_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SPMODERCV_FLG").ToString)) '特別運用結果フラグ
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_NMMODERCV_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NMMODERCV_FLG").ToString)) '通常運用結果フラグ
                                                .Add(ClsCMDataConnect.pfSet_Param("prm_STATUS_CD", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_STATUS_CD").ToString)) '進捗状況
                                            End With
                                            sqlCmd.Transaction = objTrans
                                            'コマンドタイプ設定(ストアド)
                                            sqlCmd.CommandType = CommandType.StoredProcedure
                                            sqlCmd.ExecuteNonQuery()
                                            blnRetIns = True
                                            'コミット
                                            '                                        trans.Commit()
                                            '登録が正常終了
                                            '                                        psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼一覧")
                                        Catch ex As SqlException
                                            '登録に失敗
                                            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼　リトライ登録")
                                            'ログ出力
                                            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
                                            'ロールバック
                                            objTrans.Rollback()
                                            '処理終了
                                            Throw ex
                                        Catch ex As Exception
                                            'システムエラー
                                            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼　リトライ登録")
                                            'ログ出力
                                            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
                                            'ロールバック
                                            objTrans.Rollback()
                                            '処理終了
                                            Throw ex
                                        End Try
                                    End If
                                End If

                                '既存の情報を削除扱いにする（非表示になるはず）
                                '更新するときＤＥＬＥＴＥフラグを０、１以外で更新
                                'ＵＰＤＡＴＥ処理
                                If blnRetIns = True Then
                                    Try
                                        sqlCmd = Nothing
                                        sqlCmd = New SqlCommand("DLCLSTP001_D1", conDB)
                                        With sqlCmd.Parameters
                                            'パラメータ設定
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_dllsend_no", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Replace(Environment.NewLine, ""))) '管理番号
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_seqno", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SEQNO").ToString)) '明細番号
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_notetext", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NOTETEXT").ToString))              '更新者
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_update_usr", SqlDbType.NVarChar, Session(P_SESSION_USERID)))              '更新者
                                            .Add(ClsCMDataConnect.pfSet_Param("prm_DELETE_FLG", SqlDbType.NVarChar, "2"))              '更新値
                                        End With

                                        'データ追加／更新
                                        sqlCmd.Transaction = objTrans
                                        'コマンドタイプ設定(ストアド)
                                        sqlCmd.CommandType = CommandType.StoredProcedure
                                        sqlCmd.ExecuteNonQuery()
                                        'コミット
                                        objTrans.Commit()
                                        '更新が正常終了
                                        psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼　リトライ登録")
                                    Catch ex As SqlException
                                        objTrans.Rollback()
                                        '更新に失敗
                                        psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼リトライ登録")
                                        'ログ出力
                                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
                                        Throw ex
                                    Catch ex As Exception
                                        objTrans.Rollback()
                                        'システムエラー
                                        psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼　リトライ登録")
                                        'ログ出力
                                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
                                        Throw ex
                                    Finally
                                    End Try
                                Else
                                    objTrans.Rollback()
                                End If
                            End If
                        End If
                        'DB切断
                        If Not clsDataConnect.pfClose_Database(conDB) Then
                            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        End If

                    Else

                        '検索失敗
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼")
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "ＤＢ接続失敗", "Catch")
                        'DB切断
                        If Not clsDataConnect.pfClose_Database(conDB) Then
                            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        End If
                    End If

                    'データセットの破棄
                    Call ClsCMDataConnect.psDisposeDataSet(objDS)
                    'DB切断
                    If Not clsDataConnect.pfClose_Database(conDB) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If

                Catch ex As Exception

                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    '処理終了
                    Exit Sub

                End Try

                '再描画
                ms_GetDLLhenko()

                'リトライボタン更新
                msGetRetryBtn()

            Case "btnHistory"   'DLCLSTP001_011

                Dim strCtrlNo As String
                Dim strSeqNo As String
                Dim strWrkCD As String
                Dim intRtryIndx As Integer
                Dim strLogPrm As String = String.Empty

                Try
                    '遷移用Keyの取得
                    strCtrlNo = CType(rowData.FindControl("依頼番号"), TextBox).Text.Replace(Environment.NewLine, "") '依頼番号
                    strSeqNo = CType(rowData.FindControl("明細番号"), TextBox).Text.Replace(Environment.NewLine, "") '依頼番号
                    strWrkCD = CType(rowData.FindControl("設定依頼内容コード"), TextBox).Text                        '作業コード

                    '依頼番号整形(ﾘﾄﾗｲ回数(Rnn)を除去)
                    intRtryIndx = strCtrlNo.IndexOf("R")
                    If intRtryIndx >= 0 Then
                        strCtrlNo = strCtrlNo.Substring(0, intRtryIndx)
                    End If

                    '遷移先画面PATH
                    Dim strPathDLLHSTP001 As String = "~/" & P_WAT & "/" & "DLLHSTP001/DLLHSTP001.aspx?WrkCD=" & strWrkCD

                    '遷移用引数
                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
                    Session(P_KEY) = {strCtrlNo, strSeqNo}

                    '画面遷移 DLL設定変更依頼履歴
                    psOpen_Window(Me, strPathDLLHSTP001)

                    'ログ出力内容編集
                    strLogPrm &= Session(P_SESSION_TERMS).ToString() & ","
                    strLogPrm &= strCtrlNo

                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, strPathDLLHSTP001, strLogPrm, "TRANS")

                Catch ex As Exception

                    'エラーメッセージ表示
                    psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "履歴画面への遷移")    '{0}に失敗しました。

                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

                End Try

                'DLCLSTP001_011 END
        End Select

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面項目の初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()

        '検索項目の初期化
        Me.txtTboxID.ppFromText = Nothing
        Me.txtTboxID.ppTextBoxFrom.Focus()
        Me.txtTboxID.ppToText = Nothing
        Me.ddlCenterCls.Items.Clear()
        Me.ddlSystem.Items.Clear()
        Me.txtHallName.ppText = Nothing
        Me.ddlSetteiirai.Items.Clear()
        Me.txtSetteiDayFrom.ppText = Nothing
        Me.txtSetteiTimeFrom.ppHourText = Nothing
        Me.txtSetteiTimeFrom.ppMinText = Nothing
        Me.txtSetteiDayTo.ppText = Nothing
        Me.txtSetteiTimeTo.ppHourText = Nothing
        Me.txtSetteiTimeTo.ppMinText = Nothing
        Me.txtReturnDayFrom.ppText = Nothing
        Me.txtReturnTimeFrom.ppHourText = Nothing
        Me.txtReturnTimeFrom.ppMinText = Nothing
        Me.txtReturnDayTO.ppText = Nothing
        Me.txtReturnTimeTO.ppHourText = Nothing
        Me.txtReturnTimeTO.ppMinText = Nothing

        Me.ddlRetrySel.Items.Clear()
        Me.ddlDutyCD.Items.Clear()

        '検索ボタンの初期化
        Me.Master.ppRigthButton1.CausesValidation = True
        Me.Master.ppRigthButton2.CausesValidation = False
        Me.Master.ppRigthButton3.CausesValidation = False
        Me.Master.ppRigthButton3.Text = "印刷"
        Me.Master.ppRigthButton3.Visible = True
        Me.Master.ppRigthButton3.Enabled = False

        Me.Master.ppRigthButton3.OnClientClick = pfGet_OCClickMes("10001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "運用モード変更一覧")

        '一括リトライボタンの初期化
        Me.Master.Master.ppLeftButton1.CausesValidation = False
        Me.Master.Master.ppLeftButton1.Text = "リトライ"
        Me.Master.Master.ppLeftButton1.Visible = True
        Me.Master.Master.ppLeftButton1.Enabled = False

        'リロードの初期化
        Me.Master.Master.ppLeftButton2.CausesValidation = False
        Me.Master.Master.ppLeftButton2.Text = "リトライのリロード"
        Me.Master.Master.ppLeftButton2.Visible = True
        'Me.Master.Master.ppLeftButton2.Enabled = False

        'DLCLSTP001_010
        '自動リトライボタンの初期化
        Me.Master.Master.ppLeftButton3.CausesValidation = False
        Me.Master.Master.ppLeftButton3.Text = ""
        Me.Master.Master.ppLeftButton3.Visible = True
        Me.Master.Master.ppLeftButton3.Enabled = True
        'DLCLSTP001_010 END

        '更新項目の初期化
        Me.txtTboxID_Input.ppText = Nothing
        Me.lblCenterCls_Input.Text = String.Empty
        Me.lblSystem_Input.Text = String.Empty
        Me.lblHallName_Input.Text = String.Empty
        Me.ddlSettei_input.Items.Clear()
        Me.txtStteiDay_input.ppText = Nothing
        Me.txtStteiTime_input.ppHourText = Nothing
        Me.txtStteiTime_input.ppMinText = Nothing
        Me.txtReturnDay_Input.ppText = Nothing
        Me.txtReturnTime_Input.ppHourText = Nothing
        Me.txtReturnTime_Input.ppMinText = Nothing
        Me.ChkSeisanki.Enabled = False
        'DLCLSTP001_014
        Me.txtSrvCd1.ppEnabled = False
        Me.txtSrvCd2.ppEnabled = False
        'DLCLSTP001_014 END
        Me.txtSrvCd1.ppText = String.Empty
        Me.txtSrvCd2.ppText = String.Empty
        '更新項目ボタンの検証チェック設定
        Me.btnClear.CausesValidation = False
        Me.btnDelete.CausesValidation = True
        Me.btnUpdate.CausesValidation = True
        Me.btnInsert.CausesValidation = True
        Me.btnDelete.ValidationGroup = 2
        Me.btnUpdate.ValidationGroup = 2
        Me.btnInsert.ValidationGroup = 2

        'ボタンの非活性化
        Me.btnDelete.Enabled = False
        Me.btnUpdate.Enabled = False
        Me.btnInsert.Enabled = True
        Me.btnClear.Enabled = True

        'ボタン押下メッセージ設定
        Me.btnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "運用モード変更一覧")
        Me.btnUpdate.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "運用モード変更一覧")
        Me.btnInsert.OnClientClick = pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "運用モード変更一覧")

        'グリッドビュー初期化
        Me.grvList.DataSource = New DataTable
        Me.grvList.DataBind()

        '件数の初期化
        '画面右上該当件数非表示
        Master.ppCount_Visible = False
        Master.ppCount = "0"
        Me.lblCount.Text = "0"

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト初期表示データ取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function msGetDLLData(ByVal intProcCd As Integer) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
        Dim errMsg As String = Nothing
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
                errMsg = "設定依頼内容"

                '設定依頼内容の取得
                cmdDB = New SqlCommand(M_DISP_ID & "_S4", conDB)

                'DLL変更依頼一覧
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(ClsCMDataConnect.pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))                        '結果取得用(0:データなし、1:データあり)
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ格納
                ViewState("ddlSetteiirai") = dstOrders.Tables(1)

                '結果情報を取得
                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "0"         'データ無し

                        '取得失敗
                        Throw New Exception

                    Case Else        'データ有り
                        Dim intSelIdx As Integer = Me.ddlSetteiirai.SelectedIndex
                        Me.ddlSetteiirai.SelectedIndex = 0  'DLCLSTP001_021
                        '検索項目の設定依頼内容
                        Me.ddlSetteiirai.DataSource = dstOrders.Tables(0)
                        Me.ddlSetteiirai.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString  'コード
                        Me.ddlSetteiirai.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString   '表示
                        Me.ddlSetteiirai.DataBind()

                        '先頭行の設定と選択
                        Me.ddlSetteiirai.Items.Insert(ddlSetteiirai.Items.Count, New ListItem("特別運用モードＳＷ", "99"))
                        Me.ddlSetteiirai.Items.Insert(0, "")
                        Me.ddlSetteiirai.SelectedIndex = intSelIdx

                        '更新項目の設定依頼内容
                        Me.ddlSettei_input.DataSource = dstOrders.Tables(0)
                        Me.ddlSettei_input.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString  'コード
                        Me.ddlSettei_input.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString   '表示
                        Me.ddlSettei_input.DataBind()

                End Select
                'DLCLSTP001-002
                Me.ddlSettei_input.Enabled = True
                'DLCLSTP001-002 END

                'データセットの初期化
                dstOrders = New DataSet

                '処理コード0の時のみ続行
                If intProcCd > 0 Then
                    Return True
                End If


                errMsg = "センタ区分名"

                'センタ区分名の取得
                cmdDB = New SqlCommand(M_DISP_ID & "_S2", conDB)

                'DLL変更依頼一覧
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(ClsCMDataConnect.pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))                        '結果取得用(0:データなし、1:データあり)
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '結果情報を取得
                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "0"         'データ無し

                        '取得失敗
                        Throw New Exception

                    Case Else        'データ有り

                        Me.ddlCenterCls.DataSource = dstOrders.Tables(0)
                        Me.ddlCenterCls.DataValueField = dstOrders.Tables(0).Columns(0).ColumnName.ToString  'コード
                        Me.ddlCenterCls.DataTextField = dstOrders.Tables(0).Columns(1).ColumnName.ToString   '表示
                        Me.ddlCenterCls.DataBind()

                        '先頭行の設定と選択
                        Me.ddlCenterCls.Items.Insert(0, "")
                        Me.ddlCenterCls.SelectedIndex = 0

                End Select

                'データセットの初期化
                dstOrders = New DataSet

                errMsg = "システム"

                'システム名の取得
                cmdDB = New SqlCommand(M_DISP_ID & "_S3", conDB)

                'DLL変更依頼一覧
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(ClsCMDataConnect.pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))                        '結果取得用(0:データなし、1:データあり)
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '結果情報を取得
                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "0"         'データ無し

                        '取得失敗
                        Throw New Exception

                    Case Else        'データ有り

                        Me.ddlSystem.DataSource = dstOrders.Tables(0)
                        Me.ddlSystem.DataValueField = dstOrders.Tables(0).Columns(0).ColumnName.ToString  'コード
                        Me.ddlSystem.DataTextField = dstOrders.Tables(0).Columns(1).ColumnName.ToString   '表示
                        Me.ddlSystem.DataBind()

                        '先頭行の設定と選択
                        Me.ddlSystem.Items.Insert(0, "")
                        Me.ddlSystem.SelectedIndex = 0

                End Select

                'データセットの初期化
                dstOrders = New DataSet

                errMsg = "リトライ"

                '設定依頼内容の取得
                cmdDB = New SqlCommand(M_DISP_ID & "_S8", conDB)

                'リトライ条件
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(ClsCMDataConnect.pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))                        '結果取得用(0:データなし、1:データあり)
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '結果情報を取得
                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "0"         'データ無し

                        '取得失敗
                        Throw New Exception

                    Case Else        'データ有り

                        '検索項目の設定依頼内容
                        Me.ddlRetrySel.DataSource = dstOrders.Tables(0)
                        Me.ddlRetrySel.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString  'コード
                        Me.ddlRetrySel.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString   '表示
                        Me.ddlRetrySel.DataBind()

                        '先頭行の設定と選択00
                        Me.ddlRetrySel.Items.Insert(0, "")
                        Me.ddlRetrySel.SelectedIndex = 0

                End Select

                errMsg = "設定状況"

                '設定依頼内容の取得
                cmdDB = New SqlCommand(M_DISP_ID & "_S9", conDB)

                'リトライ条件
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(ClsCMDataConnect.pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))                        '結果取得用(0:データなし、1:データあり)
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '結果情報を取得
                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "0"         'データ無し

                        '取得失敗
                        Throw New Exception

                    Case Else        'データ有り

                        '検索項目の設定依頼内容
                        Me.ddlDutyCD.DataSource = dstOrders.Tables(0)
                        Me.ddlDutyCD.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString  'コード
                        Me.ddlDutyCD.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString   '表示
                        Me.ddlDutyCD.DataBind()

                        '先頭行の設定と選択
                        Me.ddlDutyCD.Items.Insert(0, "")
                        Me.ddlDutyCD.SelectedIndex = 0

                End Select

                'DLCLSTP001_010
                '自動リトライ機能有効無効
                msGetRetryBtn()
                'DLCLSTP001_010 END

                Return True

            Catch ex As SqlException

                '検索失敗
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, errMsg)
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Return False

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, errMsg)
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Return False

            Finally

                'データセットの破棄
                Call ClsCMDataConnect.psDisposeDataSet(dstOrders)
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If

            End Try

        Else

            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            Master.ppCount = "0"
            Me.lblCount.Text = "0"
            Return False

        End If


    End Function

    ''' <summary>
    ''' セッション変数の確認
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ms_GetSession() As Boolean

        '排他情報用のグループ番号保管
        If Not Session(P_SESSION_GROUP_NUM) Is Nothing Then

            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

        End If

        '排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
        If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then

            Me.Master.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)

        End If

        Return True

    End Function

    ''' <summary>
    ''' 設定時間、戻し時間の検証チェック
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfChkRetSet() As Boolean

        Dim Setdata As String = Me.txtStteiDay_input.ppText
        Dim Settime As String = Me.txtStteiTime_input.ppHourText + _
                                Me.txtStteiTime_input.ppMinText
        Dim Retdata As String = Me.txtReturnDay_Input.ppText
        Dim Rettime As String = Me.txtReturnTime_Input.ppHourText + _
                                Me.txtReturnTime_Input.ppMinText

        Dim Setdttime As String = String.Empty
        Dim Retdttime As String = String.Empty

        Try

            '検証エラーチェック開始

            '日付と時間の一方だけ入力している(設定日時)
            If Setdata.Equals(String.Empty) And Not Settime.Equals(String.Empty) Then
                txtStteiDay_input.psSet_ErrorNo("4012", Me.txtStteiDay_input.ppName)
            ElseIf Not Setdata.Equals(String.Empty) And Settime.Equals(String.Empty) Then
                txtStteiTime_input.psSet_ErrorNo("4013", Me.txtStteiTime_input.ppName)
            End If
            '日付と時間の一方だけ入力している(戻し日時)
            If Retdata.Equals(String.Empty) And Not Rettime.Equals(String.Empty) Then
                txtReturnDay_Input.psSet_ErrorNo("4012", Me.txtReturnDay_Input.ppName)
            ElseIf Not Retdata.Equals(String.Empty) And Rettime.Equals(String.Empty) Then
                txtReturnTime_Input.psSet_ErrorNo("4013", Me.txtReturnTime_Input.ppName)
            End If

            '設定日時と戻し日時の両方を入力していない
            If Setdata.Equals(String.Empty) And Retdata.Equals(String.Empty) Then
                If Settime.Equals(String.Empty) And Rettime.Equals(String.Empty) Then
                    txtStteiDay_input.psSet_ErrorNo("2005", Me.txtStteiDay_input.ppName, Me.txtReturnDay_Input.ppName)
                End If
            End If

            '設定日より戻し日が過去日の場合

            If Setdata.Replace("/", "").PadLeft(8, "0") & Settime.Replace(":", "").PadLeft(4, "0") _
                > Retdata.Replace("/", "").PadLeft(8, "9") & Rettime.Replace("/", "").PadLeft(4, "9") Then
                txtReturnDay_Input.psSet_ErrorNo("1003", Me.txtReturnDay_Input.ppName, Me.txtStteiDay_input.ppName)
            End If

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Return False

        End Try

        Return True

    End Function

    ''' <summary>
    ''' 設定開始、戻し開始の判断
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msJudgeRetSet()
        Dim Setdttime As String = Me.txtStteiDay_input.ppText + _
                                  Me.txtStteiTime_input.ppHourText + _
                                  Me.txtStteiTime_input.ppMinText
        Dim Retdttime As String = Me.txtReturnDay_Input.ppText + _
                                  Me.txtReturnTime_Input.ppHourText + _
                                  Me.txtReturnTime_Input.ppMinText

        strRetSetf = String.Empty

        '戻し日時未設定の場合、設定始まり
        If Retdttime = String.Empty Then
            strRetSetf = M_SETSTART
            strSetcd = M_SETCDSTART
            strRetcd = M_SETCDEND
        End If

        '設定日時未設定の場合、戻し始まり
        If Setdttime = String.Empty Then
            strRetSetf = M_RETSTART
            strSetcd = M_SETCDEND
            strRetcd = M_RETCDSTART
        End If

        '設定日、戻し日時が入力されている場合、設定始まり
        If strRetSetf.Equals(String.Empty) Then
            strRetSetf = M_SETSTART
            strSetcd = M_SETCDSTART
            strRetcd = M_RETCDSTART
        End If

    End Sub

    ''' <summary>
    ''' 検索条件の保管
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_SetView_st()

        Dim strView_st_in(10 - 1) As String                       'ビューステート登録用


        strView_st_in(0) = Me.txtTboxID.ppFromText                   'TBOXID(FROM)
        strView_st_in(1) = Me.txtTboxID.ppToText                     'TBOXID(TO)
        strView_st_in(2) = Me.ddlCenterCls.SelectedItem.ToString     'センタ区分(N,L,J)
        strView_st_in(3) = Me.ddlSystem.SelectedItem.ToString        'TBOX機種コード
        strView_st_in(4) = Me.txtHallName.ppText                     'ホール名
        strView_st_in(5) = Me.ddlSetteiirai.SelectedItem.ToString    '設定依頼内容

        '設定日時(FROM)の設定
        If Not Me.txtSetteiDayFrom.ppText = String.Empty Then

            If Not Me.txtSetteiTimeFrom.ppHourText = String.Empty Then
                strView_st_in(6) = Me.txtSetteiDayFrom.ppText.Replace("/", "") _
                                 + Me.txtSetteiTimeFrom.ppHourText _
                                 + Me.txtSetteiTimeFrom.ppMinText
            Else
                If Not Me.txtSetteiDayTo.ppText = String.Empty Then
                    strView_st_in(6) = Me.txtSetteiDayFrom.ppText.Replace("/", "") _
                                     + "0000"
                Else
                    strView_st_in(6) = Me.txtSetteiDayFrom.ppText.Replace("/", "")
                End If
            End If
        Else
            strView_st_in(6) = ""
        End If

        '設定日時(To)の設定
        If Not Me.txtSetteiDayTo.ppText = String.Empty Then

            If Not Me.txtSetteiTimeTo.ppHourText = String.Empty Then
                strView_st_in(7) = Me.txtSetteiDayTo.ppText.Replace("/", "") _
                                 + Me.txtSetteiTimeTo.ppHourText _
                                 + Me.txtSetteiTimeTo.ppMinText
            Else
                strView_st_in(7) = Me.txtSetteiDayTo.ppText.Replace("/", "") _
                                 + "2359"
            End If
        Else

            strView_st_in(7) = ""

        End If

        '戻り日時(FROM)の設定
        If Not Me.txtReturnDayFrom.ppText = String.Empty Then

            If Not Me.txtReturnTimeFrom.ppHourText = String.Empty Then
                strView_st_in(8) = Me.txtReturnDayFrom.ppText.Replace("/", "") _
                                 + Me.txtReturnTimeFrom.ppHourText _
                                 + Me.txtReturnTimeFrom.ppMinText
            Else
                If Not Me.txtReturnDayTO.ppText = String.Empty Then
                    strView_st_in(8) = Me.txtReturnDayFrom.ppText.Replace("/", "") _
                                     + "0000"
                Else
                    strView_st_in(8) = Me.txtReturnDayFrom.ppText.Replace("/", "")
                End If
            End If
        Else

            strView_st_in(8) = ""

        End If

        '戻り日時(To)の設定
        If Not Me.txtReturnDayTO.ppText = String.Empty Then

            If Not Me.txtReturnTimeTO.ppHourText = String.Empty Then
                strView_st_in(9) = Me.txtReturnDayTO.ppText.Replace("/", "") _
                                 + Me.txtReturnTimeTO.ppHourText _
                                 + Me.txtReturnTimeTO.ppMinText
            Else
                strView_st_in(9) = Me.txtReturnDayTO.ppText.Replace("/", "") _
                                 + "2359"
            End If
        Else

            strView_st_in(9) = ""

        End If

        ViewState("ddlRetrySel") = ddlRetrySel.SelectedValue

        'ビューステートに保存
        Me.ViewState.Add("検索条件", strView_st_in)

    End Sub

    ''' <summary>
    ''' DLL変更依頼一覧の取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_GetDLLhenko(Optional ByVal strProcCls As String = "")

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
        Dim strView_st_out() As String = Me.ViewState("検索条件")
        objStack = New StackFrame

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then

            Try

                cmdDB = New SqlCommand(M_DISP_ID & "_S1", conDB)

                'DLL変更依頼一覧
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(ClsCMDataConnect.pfSet_Param("prm_tbox_id_f", SqlDbType.NVarChar, strView_st_out(0)))                'TBOXID(FROM)
                    .Add(ClsCMDataConnect.pfSet_Param("prm_tbox_id_t", SqlDbType.NVarChar, strView_st_out(1)))                'TBOXID(TO)
                    .Add(ClsCMDataConnect.pfSet_Param("prm_nl_flag", SqlDbType.NVarChar, strView_st_out(2)))                  'センタ区分(N,L,J)
                    .Add(ClsCMDataConnect.pfSet_Param("prm_system_cd", SqlDbType.NVarChar, strView_st_out(3)))                'TBOX機種コード
                    .Add(ClsCMDataConnect.pfSet_Param("prm_hall_name", SqlDbType.NVarChar, strView_st_out(4)))                'ホール名
                    .Add(ClsCMDataConnect.pfSet_Param("prm_settei_irai", SqlDbType.NVarChar, strView_st_out(5)))              '設定依頼内容
                    .Add(ClsCMDataConnect.pfSet_Param("prm_spmodest_dt_f", SqlDbType.NVarChar, strView_st_out(6)))            '設定日時(FROM)
                    .Add(ClsCMDataConnect.pfSet_Param("prm_spmodest_dt_t", SqlDbType.NVarChar, strView_st_out(7)))            '設定日時(TO)
                    .Add(ClsCMDataConnect.pfSet_Param("prm_nmmodest_dt_f", SqlDbType.NVarChar, strView_st_out(8)))            '戻し日時(FROM)
                    .Add(ClsCMDataConnect.pfSet_Param("prm_nmmodest_dt_t", SqlDbType.NVarChar, strView_st_out(9)))            '戻し日時(TO)
                    .Add(ClsCMDataConnect.pfSet_Param("prm_insdat_f", SqlDbType.NVarChar, ViewState("txtInsDay_From").ToString.Replace("/", "")))            '戻し日時(TO)
                    .Add(ClsCMDataConnect.pfSet_Param("prm_insdat_t", SqlDbType.NVarChar, ViewState("txtInsDay_To").ToString.Replace("/", "")))            '戻し日時(TO)
                    .Add(ClsCMDataConnect.pfSet_Param("prm_upddat_f", SqlDbType.NVarChar, ViewState("txtUpdDay_From").ToString.Replace("/", "")))            '戻し日時(TO)
                    .Add(ClsCMDataConnect.pfSet_Param("prm_upddat_t", SqlDbType.NVarChar, ViewState("txtUpdDay_To").ToString.Replace("/", "")))            '戻し日時(TO)
                    .Add(ClsCMDataConnect.pfSet_Param("prm_prctime", SqlDbType.NVarChar, ViewState("ddlDutyCD")))            '戻し日時(TO)
                    'DLCLSTP001-005
                    .Add(ClsCMDataConnect.pfSet_Param("prm_retrysel", SqlDbType.NVarChar, ViewState("ddlRetrySel")))
                    .Add(ClsCMDataConnect.pfSet_Param("prm_reload", SqlDbType.NVarChar, ViewState("btnreload")))
                    'If ViewState("mstbRetryNo") Is Nothing Then
                    If mstbRetryNo Is Nothing Then
                        .Add(ClsCMDataConnect.pfSet_Param("prm_CNTL_NO", SqlDbType.NVarChar, ""))
                    Else
                        .Add(ClsCMDataConnect.pfSet_Param("prm_CNTL_NO", SqlDbType.NVarChar, DirectCast(ViewState("mstbRetryNo"), StringBuilder).ToString))
                    End If
                    '@prm_CNTL_NO
                    'DLCLSTP001-005
                    .Add(ClsCMDataConnect.pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))       '結果取得用(0:データなし、1:データあり)
                End With

                'DLCLSTP001_017
                Select Case strProcCls
                    Case "RetryReload"

                        'リトライリロードフラグ設定
                        If ddlRetrySel.SelectedValue = "1" Or ddlRetrySel.SelectedValue = "2" Then
                            cmdDB.Parameters("prm_retrysel").Value = "1"
                            cmdDB.Parameters("prm_reload").Value = "0"
                        Else
                            '検索パラメータクリア
                            For Each cmdprm As SqlClient.SqlParameter In cmdDB.Parameters
                                cmdprm.Value = String.Empty
                            Next
                            cmdDB.Parameters("prm_retrysel").Value = "0"
                            cmdDB.Parameters("prm_reload").Value = "1"
                        End If
                    Case "Reload"
                        '検索パラメータクリア
                        For Each cmdprm As SqlClient.SqlParameter In cmdDB.Parameters
                            cmdprm.Value = String.Empty
                        Next

                        'リトライリロードフラグ設定
                        cmdDB.Parameters("prm_reload").Value = "1"
                End Select
                'DLCLSTP001_017 END


                'データ取得
                cmdDB.CommandTimeout = 120
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '結果情報を取得
                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "0"         'データ無し

                        '件数を設定
                        Master.ppCount = "0"
                        Me.lblCount.Text = "0"

                        'グリッドの初期化
                        Me.grvList.DataSource = New DataTable
                        '変更を反映
                        Me.grvList.DataBind()

                        '0件
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)

                        '印刷ボタンを非活性
                        Master.ppRigthButton3.Enabled = False

                    Case Else        'データ有り

                        '件数を設定
                        Master.ppCount = dstOrders.Tables(0).Rows(0).Item("該当件数").ToString
                        Me.lblCount.Text = dstOrders.Tables(0).Rows(0).Item("該当件数").ToString

                        '検索時、該当件数が表示件数より大きい場合
                        If CInt(Me.lblCount.Text) > CInt(dstOrders.Tables(0).Rows.Count) Then
                            psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, Me.lblCount.Text, dstOrders.Tables(0).Rows.Count.ToString)
                        End If

                        Dim zz As Integer = 0
                        Dim intRetryCnt As Integer = 0
                        Dim blnRetryFlg As Boolean = False
                        For zz = 0 To dstOrders.Tables(0).Rows.Count - 1
                            blnRetryFlg = False
                            If dstOrders.Tables(0).Rows(zz).Item("変更リトライ可能") Is DBNull.Value Then
                            ElseIf dstOrders.Tables(0).Rows(zz).Item("変更リトライ可能").ToString = "RET_NG" Then
                            ElseIf dstOrders.Tables(0).Rows(zz).Item("変更リトライ可能").ToString = "RET_OK" Then
                                blnRetryFlg = True
                            End If
                            If dstOrders.Tables(0).Rows(zz).Item("戻しリトライ可能") Is DBNull.Value Then
                            ElseIf dstOrders.Tables(0).Rows(zz).Item("戻しリトライ可能").ToString = "RET_NG" Then
                            ElseIf dstOrders.Tables(0).Rows(zz).Item("戻しリトライ可能").ToString = "RET_OK" Then
                                blnRetryFlg = True
                            End If
                            If blnRetryFlg = True Then
                                intRetryCnt += 1
                                dstOrders.Tables(0).Rows(zz).Item("リトライ") = intRetryCnt.ToString
                            Else
                                dstOrders.Tables(0).Rows(zz).Item("リトライ") = ""
                            End If

                            'DLCLSTP001_020
                            dstOrders.Tables(0).Rows(zz).Item("設定情報") = dstOrders.Tables(0).Rows(zz).Item("設定情報").ToString.Replace(Environment.NewLine, "").Replace("　", "").Replace("→", "" & Environment.NewLine & "↓" & Environment.NewLine & "")
                            dstOrders.Tables(0).Rows(zz).Item("戻し情報") = dstOrders.Tables(0).Rows(zz).Item("戻し情報").ToString.Replace(Environment.NewLine, "").Replace("　", "").Replace("→", "" & Environment.NewLine & "↓" & Environment.NewLine & "")
                            'DLCLSTP001_020 END
                        Next
                        dstOrders.Tables(0).AcceptChanges()

                        '取得したデータをグリッドに設定
                        Me.grvList.DataSource = dstOrders.Tables(0)

                        '変更を反映
                        Me.grvList.DataBind()

                        If dstOrders.Tables.Count > 1 Then
                            '取得したデータをグリッドに保存
                            '帳票用のデータテーブルの整形
                            msPrintRemake(dstOrders.Tables(1))
                        Else
                            '帳票用のデータテーブルの整形
                            msPrintRemake(dstOrders.Tables(0))
                        End If

                        '印刷ボタンを活性化
                        Master.ppRigthButton3.Enabled = True

                End Select

            Catch ex As SqlException

                '検索失敗
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼一覧")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Throw ex

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼一覧")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Throw ex

            Finally

                Call ClsCMDataConnect.psDisposeDataSet(dstOrders)
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try

        Else

            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Master.ppCount = "0"
            Me.lblCount.Text = "0"

        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリストの選択行参照
    ''' </summary>
    ''' <param name="p1"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ms_GetSelectIndex(p1 As String) As Integer

        Dim selIndex As Integer = 0

        For Each lstItem As ListItem In Me.ddlSettei_input.Items

            ' value が 用途コードと一致する
            If (lstItem.Text = p1) Then
                Return selIndex
                Exit Function
            End If

            selIndex += 1

        Next

        'DLCLSTP001-002
        Return 99
        'DLCLSTP001-002 END
    End Function

    ''' <summary>
    ''' データバインド時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        Dim strDelF As String = Nothing               '削除フラグ
        Dim strInputF As String = Nothing            '手入力フラグ
        Dim intRetCnt As Integer = 0
        For Each rowData As GridViewRow In grvList.Rows
            'DLCLSTP001_020
            'CType(rowData.FindControl("設定情報"), TextBox).Text = CType(rowData.FindControl("設定情報"), TextBox).Text.Replace(Environment.NewLine, "").Replace("　", "").Replace("→", "" & Environment.NewLine & "↓" & Environment.NewLine & "")
            'CType(rowData.FindControl("戻し情報"), TextBox).Text = CType(rowData.FindControl("戻し情報"), TextBox).Text.Replace(Environment.NewLine, "").Replace("　", "").Replace("→", "" & Environment.NewLine & "↓" & Environment.NewLine & "")
            'DLCLSTP001_020 END

            'DLCLSTP001_015
            CType(rowData.FindControl("設定状態_一覧"), TextBox).Text = CType(rowData.FindControl("設定状態_一覧"), TextBox).Text.Replace(Environment.NewLine, "")
            CType(rowData.FindControl("戻し状態_一覧"), TextBox).Text = CType(rowData.FindControl("戻し状態_一覧"), TextBox).Text.Replace(Environment.NewLine, "")
            'DLCLSTP001_015 END

            '削除フラグがある行の選択を非活性にする
            '手入力以外の行の選択を非活性にする
            strDelF = CType(rowData.FindControl("削除"), TextBox).Text
            strInputF = CType(rowData.FindControl("依頼方法"), TextBox).Text

            If strDelF.Trim = "●" Then   '削除フラグあり
                'DLCLSTP001_007
                '                rowData.Cells(0).Enabled = False
                'DLCLSTP001_007
                'DirectCast(rowData.Controls.Item(1).Controls(0), Button).Enabled = False
                DirectCast(rowData.FindControl("一括リトライ"), CheckBox).Enabled = False
            Else
                If CType(rowData.FindControl("変更リトライ可能"), TextBox).Text = "RET_NG" _
                And CType(rowData.FindControl("戻しリトライ可能"), TextBox).Text = "RET_NG" Then
                    'DirectCast(rowData.Controls.Item(1).Controls(0), Button).Enabled = False
                    DirectCast(rowData.FindControl("一括リトライ"), CheckBox).Enabled = False
                ElseIf CType(rowData.FindControl("変更リトライ可能"), TextBox).Text = "RET_OK" _
                And CType(rowData.FindControl("戻しリトライ可能"), TextBox).Text = "RET_NG" Then
                    If CType(rowData.FindControl("戻し電文結果"), TextBox).Text = "" Then
                        'DirectCast(rowData.Controls.Item(1).Controls(0), Button).Enabled = True
                        DirectCast(rowData.FindControl("一括リトライ"), CheckBox).Enabled = True
                        intRetCnt += 1
                    Else
                        '戻しが入っていても変更が失敗していた場合はリトライ可能
                        If CType(rowData.FindControl("設定電文結果"), TextBox).Text <> "NT0060" _
                        And CType(rowData.FindControl("設定電文結果"), TextBox).Text <> "NT0061" Then
                            DirectCast(rowData.FindControl("一括リトライ"), CheckBox).Enabled = True
                            intRetCnt += 1
                        Else
                            DirectCast(rowData.FindControl("一括リトライ"), CheckBox).Enabled = False
                        End If
                        'DirectCast(rowData.Controls.Item(1).Controls(0), Button).Enabled = False
                        '                        DirectCast(rowData.FindControl("一括リトライ"), CheckBox).Enabled = False
                    End If
                Else
                    'DirectCast(rowData.Controls.Item(1).Controls(0), Button).Enabled = True
                    DirectCast(rowData.FindControl("一括リトライ"), CheckBox).Enabled = True
                    intRetCnt += 1
                End If
            End If

            '履歴が無い場合はﾎﾞﾀﾝ非活性 DLCLSTP001_011
            If CType(rowData.FindControl("履歴有無"), TextBox).Text = "0" Then
                rowData.Cells(1).Enabled = False
            Else
                rowData.Cells(1).Enabled = True
            End If
            'DLCLSTP001_011 END
        Next

        '一括リトライボタン制御
        If intRetCnt = 0 Then
            Me.Master.Master.ppLeftButton1.Enabled = False
            'Me.Master.Master.ppLeftButton2.Enabled = False
        Else
            Me.Master.Master.ppLeftButton1.Enabled = True
            'Me.Master.Master.ppLeftButton2.Enabled = True
        End If

    End Sub

    ''' <summary>
    ''' ＤＬＬ変更依頼一覧情報登録
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_InsDLLhenko()

        Dim conDB As SqlConnection = Nothing               'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing                  'SqlCommandクラス
        Dim objDs As DataSet = Nothing                     'データセット
        Dim trans As SqlClient.SqlTransaction              'トランザクション
        Dim cntrol_num As String = Nothing                 '管理番号
        Dim strOKNG As String = Nothing                    '検索結果
        Dim strSettei_dt As String = Nothing               '設定日時
        Dim strReturn_dt As String = Nothing               '戻し日時
        Dim dstOrders As New DataSet
        Dim strView_st_out() As String = Me.ViewState("変更削除情報")
        Dim strddl() As String = Me.ddlSettei_input.SelectedValue.ToString.Split(":")
        Dim strSettei_f As String = "0"
        Dim strRetrun_f As String = "0"
        Dim intNum As Integer
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

                '管理番号の作成
                cntrol_num = Date.Now.ToString("yyyyMMdd")

                '管理番号採番
                'パラメータ設定
                cmdDB = New SqlCommand("ZCMPSEL022", conDB)
                With cmdDB.Parameters
                    'パラメータ設定 
                    .Add(ClsCMDataConnect.pfSet_Param("MNGNO_CLS", SqlDbType.Int, 1))                                    '管理番号
                    .Add(ClsCMDataConnect.pfSet_Param("YMD", SqlDbType.NVarChar, Date.Now.ToString("yyyyMMdd")))         '年月日
                    .Add(ClsCMDataConnect.pfSet_Param("SalesYTD", SqlDbType.Int, 20, ParameterDirection.Output))         '結果取得用
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'cmdDB.Transaction = trans
                'cmdDB.CommandType = CommandType.StoredProcedure
                'cmdDB.ExecuteNonQuery()

                '管理番号を作成する
                intNum = cmdDB.Parameters("SalesYTD").Value
                cntrol_num = cntrol_num + intNum.ToString("0000")

                If Me.txtStteiDay_input.ppText.Equals(String.Empty) Then
                    strSettei_dt = ""
                Else
                    strSettei_dt = Me.txtStteiDay_input.ppText _
                             + " " _
                             + Me.txtStteiTime_input.ppHourText _
                             + ":" _
                             + Me.txtStteiTime_input.ppMinText

                End If

                If Me.txtReturnDay_Input.ppText.Equals(String.Empty) Then
                    strReturn_dt = ""
                Else
                    strReturn_dt = Me.txtReturnDay_Input.ppText _
                            + " " _
                            + Me.txtReturnTime_Input.ppHourText _
                            + ":" _
                            + Me.txtReturnTime_Input.ppMinText
                End If

                'チェックボックスリストのチェック対象確認
                '--------------------------------
                '2014/04/14 高松　ここから
                '--------------------------------
                'If ChkSeisanki.Items(0).Selected Then

                '    strSettei_f = "1"

                'End If

                'If ChkSeisanki.Items(1).Selected Then

                '    strRetrun_f = "1"

                'End If

                If Me.ChkSeisanki.Enabled Then
                    If ChkSeisanki.Items(0).Selected Then

                        strSettei_f = "1"

                    End If

                    If ChkSeisanki.Items(1).Selected Then

                        strRetrun_f = "1"

                    End If
                End If
                '--------------------------------
                '2014/04/14 高松　ここまで
                '--------------------------------
            Catch ex As SqlException
                '登録に失敗
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼一覧")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                '処理終了
                Throw ex

            Catch ex As Exception
                'システムエラー
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼一覧")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                '処理終了
                Throw ex
            End Try

            trans = conDB.BeginTransaction

            Try

                cmdDB = New SqlCommand(M_DISP_ID & "_I1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(ClsCMDataConnect.pfSet_Param("prm_dllsend_no", SqlDbType.NVarChar, cntrol_num))                                 '管理番号
                    If ViewState("SEQ_NO") Is Nothing Then
                        .Add(ClsCMDataConnect.pfSet_Param("prm_seqno", SqlDbType.NVarChar, "1"))                                             '明細番号
                    ElseIf ViewState("SEQ_NO").ToString = "" Then
                        .Add(ClsCMDataConnect.pfSet_Param("prm_seqno", SqlDbType.NVarChar, "1"))                                             '明細番号
                    Else
                        .Add(ClsCMDataConnect.pfSet_Param("prm_seqno", SqlDbType.NVarChar, ViewState("SEQ_NO").ToString))                                             '明細番号
                    End If
                    .Add(ClsCMDataConnect.pfSet_Param("prm_nl_flg", SqlDbType.NVarChar, strView_st_out(2)))                              'ＮＬ区分
                    .Add(ClsCMDataConnect.pfSet_Param("prm_req", SqlDbType.NVarChar, "手入力"))                                          '依頼方法
                    .Add(ClsCMDataConnect.pfSet_Param("prm_set_cd", SqlDbType.NVarChar, strddl(0)))                                      '作業内容コード
                    .Add(ClsCMDataConnect.pfSet_Param("prm_set_nm", SqlDbType.NVarChar, strddl(1)))                                      '作業内容
                    .Add(ClsCMDataConnect.pfSet_Param("prm_tboxid", SqlDbType.NVarChar, Me.txtTboxID_Input.ppText))                      'ＴＢＯＸＩＤ
                    .Add(ClsCMDataConnect.pfSet_Param("prm_hall_cd", SqlDbType.NVarChar, strView_st_out(1)))                             'ホールコード
                    .Add(ClsCMDataConnect.pfSet_Param("prm_hall_nm", SqlDbType.NVarChar, Me.lblHallName_Input.Text))                     'ホール名
                    .Add(ClsCMDataConnect.pfSet_Param("prm_tboxclass_cd", SqlDbType.NVarChar, strView_st_out(3)))                        'ＴＢＯＸ機種コード
                    .Add(ClsCMDataConnect.pfSet_Param("prm_tboxclass_nm", SqlDbType.NVarChar, Me.lblSystem_Input.Text))                  'ＴＢＯＸ機種
                    .Add(ClsCMDataConnect.pfSet_Param("prm_spmodest_dt", SqlDbType.NVarChar, strSettei_dt))                              '特別運用モード変更日時
                    .Add(ClsCMDataConnect.pfSet_Param("prm_spmodest_cd", SqlDbType.NVarChar, strSetcd))                                  '特別運用変更内容コード
                    .Add(ClsCMDataConnect.pfSet_Param("prm_nmmodest_dt", SqlDbType.NVarChar, strReturn_dt))                              '通常運用モード戻し日時
                    .Add(ClsCMDataConnect.pfSet_Param("prm_nmmodest_cd", SqlDbType.NVarChar, strRetcd))                                  '通常運用変更内容コード
                    .Add(ClsCMDataConnect.pfSet_Param("prm_setreq_nm", SqlDbType.NVarChar, Me.txtIraisya.ppText))                        '設定依頼者
                    If strddl(0).ToString = "13" Then
                        .Add(ClsCMDataConnect.pfSet_Param("prm_set_info", SqlDbType.NVarChar, "1：" & txtSrvCd1.ppText))                 '設定情報
                        .Add(ClsCMDataConnect.pfSet_Param("prm_ret_info", SqlDbType.NVarChar, "2：" & txtSrvCd2.ppText))                 '戻し情報
                    Else
                        .Add(ClsCMDataConnect.pfSet_Param("prm_set_info", SqlDbType.NVarChar, strddl(2)))                                '設定情報
                        .Add(ClsCMDataConnect.pfSet_Param("prm_ret_info", SqlDbType.NVarChar, strddl(3)))                                '戻し情報
                    End If
                    '--------------------------------
                    '2014/04/14 高松　ここから
                    '--------------------------------
                    '.Add(clsDataConnect.pfSet_Param("prm_adjmachine_chg", SqlDbType.NVarChar, strSettei_f))                            '精算機変更
                    '.Add(clsDataConnect.pfSet_Param("prm_adjmachine_rtn", SqlDbType.NVarChar, strRetrun_f))                            '精算機戻し
                    If Me.ChkSeisanki.Enabled Then
                        .Add(ClsCMDataConnect.pfSet_Param("prm_adjmachine_chg", SqlDbType.NVarChar, strSettei_f))                        '精算機変更
                        .Add(ClsCMDataConnect.pfSet_Param("prm_adjmachine_rtn", SqlDbType.NVarChar, strRetrun_f))                        '精算機戻し
                    Else
                        .Add(ClsCMDataConnect.pfSet_Param("prm_adjmachine_chg", SqlDbType.NVarChar, "0"))                                '精算機変更
                        .Add(ClsCMDataConnect.pfSet_Param("prm_adjmachine_rtn", SqlDbType.NVarChar, "0"))                                '精算機戻し
                    End If
                    '--------------------------------
                    '2014/04/14 高松　ここまで
                    '--------------------------------
                    .Add(ClsCMDataConnect.pfSet_Param("prm_sendend_flg", SqlDbType.NVarChar, "2"))                                       '案件終了フラグ
                    .Add(ClsCMDataConnect.pfSet_Param("prm_insert_usr", SqlDbType.NVarChar, Session(P_SESSION_USERID)))                  '登録者
                    .Add(ClsCMDataConnect.pfSet_Param("prm_proctime_cls", SqlDbType.NVarChar, strRetSetf))                               '処理時間区分
                    'DLCLSTP001-001
                    .Add(ClsCMDataConnect.pfSet_Param("prm_notetext", SqlDbType.NVarChar, Me.txtNotes.ppText))
                    'DLCLSTP001-001 END
                End With

                cmdDB.Transaction = trans
                'コマンドタイプ設定(ストアド)
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.ExecuteNonQuery()
                'コミット
                trans.Commit()

                '登録が正常終了
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼一覧")

            Catch ex As SqlException
                '登録に失敗
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼一覧")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                'ロールバック
                trans.Rollback()

                '処理終了
                Throw ex

            Catch ex As Exception
                'システムエラー
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼一覧")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                'ロールバック
                trans.Rollback()

                '処理終了
                Throw ex

            Finally

                'データセットの破棄
                Call ClsCMDataConnect.psDisposeDataSet(dstOrders)
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Throw New Exception
        End If

    End Sub

    ''' <summary>
    ''' ＤＬＬ変更依頼一覧情報変更
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_UpdDLLhenko()

        Dim conDB As SqlConnection = Nothing               'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing                  'SqlCommandクラス
        Dim objDs As DataSet = Nothing                     'データセット
        Dim strOKNG As String = Nothing                    '検索結果
        Dim strSettei_dt As String = Nothing               '設定日時
        Dim strReturn_dt As String = Nothing               '戻し日時
        Dim dstOrders As New DataSet
        Dim strView_st_out() As String = Me.ViewState("変更削除情報")
        Dim strddl() As String = Me.ddlSettei_input.SelectedValue.ToString.Split(":")
        Dim strSettei_f As String = "0"
        Dim strRetrun_f As String = "0"
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

                If Me.txtStteiDay_input.ppText.Equals(String.Empty) Then
                    strSettei_dt = ""
                Else
                    strSettei_dt = Me.txtStteiDay_input.ppText _
                             + " " _
                             + Me.txtStteiTime_input.ppHourText _
                             + ":" _
                             + Me.txtStteiTime_input.ppMinText

                End If

                If Me.txtReturnDay_Input.ppText.Equals(String.Empty) Then
                    strReturn_dt = ""
                Else
                    strReturn_dt = Me.txtReturnDay_Input.ppText _
                            + " " _
                            + Me.txtReturnTime_Input.ppHourText _
                            + ":" _
                            + Me.txtReturnTime_Input.ppMinText
                End If

                'チェックボックスリストのチェック対象確認
                If ChkSeisanki.Items(0).Selected Then

                    strSettei_f = "1"

                End If

                If ChkSeisanki.Items(1).Selected Then

                    strRetrun_f = "1"

                End If

                cmdDB = New SqlCommand(M_DISP_ID & "_U1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(ClsCMDataConnect.pfSet_Param("prm_dllsend_no", SqlDbType.NVarChar, strView_st_out(0).Replace(Environment.NewLine, "")))                          '管理番号
                    .Add(ClsCMDataConnect.pfSet_Param("prm_seqno", SqlDbType.NVarChar, ViewState("SEQ_NO").ToString))                                             '明細番号
                    .Add(ClsCMDataConnect.pfSet_Param("prm_nl_flg", SqlDbType.NVarChar, strView_st_out(2)))                              'ＮＬ区分
                    .Add(ClsCMDataConnect.pfSet_Param("prm_req", SqlDbType.NVarChar, "手入力"))                                          '依頼方法
                    .Add(ClsCMDataConnect.pfSet_Param("prm_set_cd", SqlDbType.NVarChar, strddl(0)))                                      '作業内容コード
                    .Add(ClsCMDataConnect.pfSet_Param("prm_set_nm", SqlDbType.NVarChar, strddl(1)))                                      '作業内容
                    .Add(ClsCMDataConnect.pfSet_Param("prm_tboxid", SqlDbType.NVarChar, Me.txtTboxID_Input.ppText))                      'ＴＢＯＸＩＤ
                    .Add(ClsCMDataConnect.pfSet_Param("prm_hall_cd", SqlDbType.NVarChar, strView_st_out(1)))                             'ホールコード
                    .Add(ClsCMDataConnect.pfSet_Param("prm_hall_nm", SqlDbType.NVarChar, Me.lblHallName_Input.Text))                     'ホール名
                    .Add(ClsCMDataConnect.pfSet_Param("prm_tboxclass_cd", SqlDbType.NVarChar, strView_st_out(3)))                        'ＴＢＯＸ機種コード
                    .Add(ClsCMDataConnect.pfSet_Param("prm_tboxclass_nm", SqlDbType.NVarChar, Me.lblSystem_Input.Text))                  'ＴＢＯＸ機種
                    .Add(ClsCMDataConnect.pfSet_Param("prm_spmodest_dt", SqlDbType.NVarChar, strSettei_dt))                              '特別運用モード変更日時
                    .Add(ClsCMDataConnect.pfSet_Param("prm_spmodest_cd", SqlDbType.NVarChar, "0"))                                       '特別運用変更内容コード
                    .Add(ClsCMDataConnect.pfSet_Param("prm_nmmodest_dt", SqlDbType.NVarChar, strReturn_dt))                              '通常運用モード戻し日時
                    .Add(ClsCMDataConnect.pfSet_Param("prm_nmmodest_cd", SqlDbType.NVarChar, "0"))                                       '通常運用変更内容コード
                    .Add(ClsCMDataConnect.pfSet_Param("prm_setreq_nm", SqlDbType.NVarChar, Me.txtIraisya.ppText))                        '設定依頼者
                    If strddl(0).ToString = "13" Then
                        .Add(ClsCMDataConnect.pfSet_Param("prm_set_info", SqlDbType.NVarChar, "1：" & txtSrvCd1.ppText))                 '設定情報
                        .Add(ClsCMDataConnect.pfSet_Param("prm_ret_info", SqlDbType.NVarChar, "2：" & txtSrvCd2.ppText))                 '戻し情報
                    Else
                        .Add(ClsCMDataConnect.pfSet_Param("prm_set_info", SqlDbType.NVarChar, strddl(2)))                                '設定情報
                        .Add(ClsCMDataConnect.pfSet_Param("prm_ret_info", SqlDbType.NVarChar, strddl(3)))                                '戻し情報
                    End If
                    '--------------------------------
                    '2014/04/14 高松　ここから
                    '--------------------------------
                    '.Add(clsDataConnect.pfSet_Param("prm_adjmachine_chg", SqlDbType.NVarChar, strSettei_f))                            '精算機変更
                    '.Add(clsDataConnect.pfSet_Param("prm_adjmachine_rtn", SqlDbType.NVarChar, strRetrun_f))                            '精算機戻し
                    If Me.ChkSeisanki.Enabled Then
                        .Add(ClsCMDataConnect.pfSet_Param("prm_adjmachine_chg", SqlDbType.NVarChar, strSettei_f))                         '精算機変更
                        .Add(ClsCMDataConnect.pfSet_Param("prm_adjmachine_rtn", SqlDbType.NVarChar, strRetrun_f))                         '精算機戻し
                    Else
                        .Add(ClsCMDataConnect.pfSet_Param("prm_adjmachine_chg", SqlDbType.NVarChar, "0"))                       '精算機変更
                        .Add(ClsCMDataConnect.pfSet_Param("prm_adjmachine_rtn", SqlDbType.NVarChar, "0"))                       '精算機戻し
                    End If
                    '--------------------------------
                    '2014/04/14 高松　ここまで
                    '--------------------------------
                    .Add(ClsCMDataConnect.pfSet_Param("prm_sendend_flg", SqlDbType.NVarChar, "2"))                                       '案件終了フラグ
                    .Add(ClsCMDataConnect.pfSet_Param("prm_update_usr", SqlDbType.NVarChar, Session(P_SESSION_USERID)))                  '更新者
                    'DLCLSTP001-001
                    .Add(ClsCMDataConnect.pfSet_Param("prm_notetext", SqlDbType.NVarChar, Me.txtNotes.ppText))
                    'DLCLSTP001-001 END
                End With

                'データ追加／更新
                Using conTrn = conDB.BeginTransaction
                    cmdDB.Transaction = conTrn
                    'コマンドタイプ設定(ストアド)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()
                    'コミット
                    conTrn.Commit()
                End Using

                '更新が正常終了
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼一覧")

            Catch ex As SqlException
                '更新に失敗
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼一覧")
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
            Catch ex As Exception
                'システムエラー
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼一覧")
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
            Finally

                'データセットの破棄
                Call ClsCMDataConnect.psDisposeDataSet(dstOrders)
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Throw New Exception
        End If

    End Sub

    ''' <summary>
    ''' ＤＬＬ変更依頼一覧情報削除
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_DelDLLhenko()

        Dim conDB As SqlConnection = Nothing               'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing                  'SqlCommandクラス
        Dim objDs As DataSet = Nothing                     'データセット
        Dim strOKNG As String = Nothing                    '検索結果
        Dim dstOrders As New DataSet
        Dim strView_st_out() As String = Me.ViewState("変更削除情報")
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

                cmdDB = New SqlCommand(M_DISP_ID & "_D1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    If strView_st_out(0) Is DBNull.Value Then
                        .Add(ClsCMDataConnect.pfSet_Param("prm_dllsend_no", SqlDbType.NVarChar, ""))                      '管理番号
                    ElseIf String.IsNullOrEmpty(strView_st_out(0)) Then
                        .Add(ClsCMDataConnect.pfSet_Param("prm_dllsend_no", SqlDbType.NVarChar, ""))                      '管理番号
                    Else
                        .Add(ClsCMDataConnect.pfSet_Param("prm_dllsend_no", SqlDbType.NVarChar, strView_st_out(0).Replace(Environment.NewLine, ""))) '管理番号
                    End If
                    '                    .Add(clsDataConnect.pfSet_Param("prm_dllsend_no", SqlDbType.NVarChar, strView_st_out(0)))                      '管理番号
                    If Me.txtNotes.ppText Is DBNull.Value Then
                        .Add(ClsCMDataConnect.pfSet_Param("prm_notetext", SqlDbType.NVarChar, ""))
                    Else
                        .Add(ClsCMDataConnect.pfSet_Param("prm_notetext", SqlDbType.NVarChar, Me.txtNotes.ppText))
                    End If
                    .Add(ClsCMDataConnect.pfSet_Param("prm_seqno", SqlDbType.NVarChar, ViewState("SEQ_NO").ToString))                                         '明細番号
                    .Add(ClsCMDataConnect.pfSet_Param("prm_update_usr", SqlDbType.NVarChar, Session(P_SESSION_USERID)))              '更新者
                    .Add(ClsCMDataConnect.pfSet_Param("prm_DELETE_FLG", SqlDbType.NVarChar, "1"))              '更新者
                End With

                'データ追加／更新
                Using conTrn = conDB.BeginTransaction
                    cmdDB.Transaction = conTrn
                    'コマンドタイプ設定(ストアド)
                    cmdDB.CommandTimeout = "5000"
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()
                    'コミット
                    conTrn.Commit()
                End Using

                '更新が正常終了
                psMesBox(Me, "00002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼一覧")

            Catch ex As SqlException
                '更新に失敗
                psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼一覧")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Throw ex
            Catch ex As Exception
                'システムエラー
                psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼一覧")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Throw ex
            Finally
                'データセットの破棄
                Call ClsCMDataConnect.psDisposeDataSet(dstOrders)
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Throw New Exception
        End If

    End Sub

    ''' <summary>
    ''' 更新項目ＴＢＯＸＩＤが変更された場合
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ctl_Change(sender As Object, e As EventArgs)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
        Dim strView_st_in(11 - 1) As String

        objStack = New StackFrame

        If Me.txtTboxID_Input.ppText = String.Empty Then

            '入力が空の場合項目初期化
            Me.lblCenterCls_Input.Text = String.Empty
            Me.lblSystem_Input.Text = String.Empty
            Me.lblHallName_Input.Text = String.Empty

            '処理終了
            Exit Sub

        End If

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then

            Try

                cmdDB = New SqlCommand(M_DISP_ID & "_S5", conDB)

                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(ClsCMDataConnect.pfSet_Param("prm_tbox_id", SqlDbType.NVarChar, Me.txtTboxID_Input.ppText))        'TBOXID
                    .Add(ClsCMDataConnect.pfSet_Param("prm_hallCD", SqlDbType.NVarChar, 20, ParameterDirection.Output))     'ホールコード
                    .Add(ClsCMDataConnect.pfSet_Param("prm_hallname", SqlDbType.NVarChar, 50, ParameterDirection.Output))   'ホール名
                    .Add(ClsCMDataConnect.pfSet_Param("prm_nl", SqlDbType.NVarChar, 20, ParameterDirection.Output))         'NLコード
                    .Add(ClsCMDataConnect.pfSet_Param("prm_nl_name", SqlDbType.NVarChar, 20, ParameterDirection.Output))    'センタ区分名
                    .Add(ClsCMDataConnect.pfSet_Param("prm_system", SqlDbType.NVarChar, 20, ParameterDirection.Output))     'TBOX機種
                    .Add(ClsCMDataConnect.pfSet_Param("prm_systemCD", SqlDbType.NVarChar, 20, ParameterDirection.Output))   'TBOX機種コード
                    .Add(ClsCMDataConnect.pfSet_Param("prm_data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output)) '結果取得用
                    .Add(ClsCMDataConnect.pfSet_Param("prm_systemCls", SqlDbType.NVarChar, 20, ParameterDirection.Output))  'システムクラス 'DLCLSTP001_016
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '結果情報を取得
                strOKNG = cmdDB.Parameters("prm_data_exist").Value.ToString

                Select Case strOKNG
                    Case "0"         'データ無し

                        Me.lblCenterCls_Input.Text = String.Empty
                        Me.lblSystem_Input.Text = String.Empty
                        Me.lblHallName_Input.Text = String.Empty

                        Exit Sub

                    Case Else        'データ有り

                        'システムクラスID弾く 'DLCLSTP001_016
                        If cmdDB.Parameters("prm_systemCls").Value.ToString = "1" Then
                            txtTboxID_Input.psSet_ErrorNo("7001")
                            Exit Sub
                        End If
                        'DLCLSTP001_016 END

                        '--------------------------------
                        '2014/04/21 石橋　ここから
                        '--------------------------------
                        strView_st_in = Me.ViewState("変更削除情報")

                        'ViewStateに値を設定する
                        'strView_st_in(0) = "0"
                        'strView_st_in(1) = cmdDB.Parameters("prm_hallCD").Value.ToString
                        'strView_st_in(2) = cmdDB.Parameters("prm_nl").Value.ToString
                        'strView_st_in(3) = cmdDB.Parameters("prm_systemCD").Value.ToString
                        'strView_st_in(4) = "0"
                        'strView_st_in(5) = "0"
                        If strView_st_in Is Nothing Then
                            strView_st_in = {"0", _
                                             cmdDB.Parameters("prm_hallCD").Value.ToString, _
                                             cmdDB.Parameters("prm_nl").Value.ToString, _
                                             cmdDB.Parameters("prm_systemCD").Value.ToString, _
                                             "0",
                                             "0"}
                        ElseIf strView_st_in(0) = "0" Then
                            strView_st_in(0) = "0"
                            strView_st_in(1) = cmdDB.Parameters("prm_hallCD").Value.ToString
                            strView_st_in(2) = cmdDB.Parameters("prm_nl").Value.ToString
                            strView_st_in(3) = cmdDB.Parameters("prm_systemCD").Value.ToString
                            strView_st_in(4) = "0"
                            strView_st_in(5) = "0"
                        Else
                            strView_st_in(1) = cmdDB.Parameters("prm_hallCD").Value.ToString
                            strView_st_in(2) = cmdDB.Parameters("prm_nl").Value.ToString
                            strView_st_in(3) = cmdDB.Parameters("prm_systemCD").Value.ToString
                        End If
                        '--------------------------------
                        '2014/04/21 石橋　ここまで
                        '--------------------------------

                        Me.ViewState.Add("変更削除情報", strView_st_in)

                        '                        Me.lblCenterCls_Input.Text = cmdDB.Parameters("prm_nl_name").Value.ToString
                        Me.lblCenterCls_Input.Text = cmdDB.Parameters("prm_nl").Value.ToString
                        Me.lblSystem_Input.Text = cmdDB.Parameters("prm_system").Value.ToString
                        Me.lblHallName_Input.Text = cmdDB.Parameters("prm_hallname").Value.ToString

                End Select

            Catch ex As SqlException

                '検索失敗
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼一覧")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼一覧")
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

                'データセットの破棄
                Call ClsCMDataConnect.psDisposeDataSet(dstOrders)
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

                Me.txtTboxID_Input.ppTextBox.Focus()

            End Try
        Else

            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Master.ppCount = "0"
            Me.lblCount.Text = "0"

        End If


    End Sub

    ''' <summary>
    ''' 依頼中のTBOXIDの確認
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfJudgeTboxid(Optional ByVal ipstrMode As String = "") As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
        Dim strView_st_out() As String = Me.ViewState("変更削除情報")
        Dim strDllSendNo As String
        objStack = New StackFrame

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then

            Try

                If ipstrMode = "Update" Then
                    strDllSendNo = strView_st_out(0).Replace(Environment.NewLine, "")
                Else
                    strDllSendNo = String.Empty
                End If

                Dim dtmSTDT As Date = Nothing
                Dim dtmEDDT As Date = Nothing

                dtmSTDT = If(txtStteiDay_input.ppDate.ToString("yyyy/MM/dd") = "0001/01/01", "1900/01/01", txtStteiDay_input.ppDate.ToString("yyyy/MM/dd")) & " " & If(txtStteiTime_input.ppHourText.ToString = "", "00", txtStteiTime_input.ppHourText.ToString) & ":" & If(txtStteiTime_input.ppMinText.ToString = "", "00", txtStteiTime_input.ppMinText.ToString) & ":00"
                dtmEDDT = If(txtReturnDay_Input.ppDate.ToString("yyyy/MM/dd") = "0001/01/01", "1900/01/01", txtReturnDay_Input.ppDate.ToString("yyyy/MM/dd")) & " " & If(txtReturnTime_Input.ppHourText.ToString = "", "00", txtReturnTime_Input.ppHourText.ToString) & ":" & If(txtReturnTime_Input.ppMinText.ToString = "", "00", txtReturnTime_Input.ppMinText.ToString) & ":00"

                cmdDB = New SqlCommand(M_DISP_ID & "_S6", conDB)

                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(ClsCMDataConnect.pfSet_Param("prm_dllsend_no", SqlDbType.NVarChar, strDllSendNo))        '管理番号
                    .Add(ClsCMDataConnect.pfSet_Param("prm_tbox_id", SqlDbType.NVarChar, Me.txtTboxID_Input.ppText))        'TBOXID
                    .Add(ClsCMDataConnect.pfSet_Param("prm_set_cd", SqlDbType.NVarChar, Me.ddlSettei_input.SelectedValue.ToString.Split(":")(0)))    '依頼内容
                    .Add(ClsCMDataConnect.pfSet_Param("prm_ST_DT", SqlDbType.DateTime, dtmSTDT))    '依頼内容
                    .Add(ClsCMDataConnect.pfSet_Param("prm_ED_DT", SqlDbType.DateTime, dtmEDDT))    '依頼内容
                    .Add(ClsCMDataConnect.pfSet_Param("prm_data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output)) '結果取得用
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '結果情報を取得
                strOKNG = cmdDB.Parameters("prm_data_exist").Value.ToString

                Select Case strOKNG
                    Case "0"         'データ無し

                        Return True

                    Case Else        'データ有り

                        '検索失敗
                        psMesBox(Me, "20001", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        '                        'Return False
                        Return False

                End Select

            Catch ex As SqlException

                '検索失敗
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼一覧")

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

                Return False
            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼一覧")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

                Return False
            Finally

                'データセットの破棄
                Call ClsCMDataConnect.psDisposeDataSet(dstOrders)
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
    ''' 設定依頼内容の選択変更
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub dll_Change(sender As Object, e As EventArgs)

        Dim split() As String = Me.ddlSettei_input.SelectedValue.ToString.Split(":")

        If split Is Nothing Then
            Exit Sub
        End If

        Select Case split(0)
            Case "02", "03"  '深夜営業、終夜営業

                Me.ChkSeisanki.Enabled = True

            Case Else

                Me.ChkSeisanki.Enabled = False
                Me.ChkSeisanki.Items(0).Selected = False
                Me.ChkSeisanki.Items(1).Selected = False

        End Select

        'DLCLSTP001_014
        If split(0) = "13" Then
            txtSrvCd1.ppEnabled = True
            txtSrvCd2.ppEnabled = True
            txtReturnDay_Input.ppEnabled = False
            txtReturnTime_Input.ppEnabled = False
        Else
            txtSrvCd1.ppText = String.Empty
            txtSrvCd2.ppText = String.Empty
            txtSrvCd1.ppEnabled = False
            txtSrvCd2.ppEnabled = False
            txtReturnDay_Input.ppEnabled = True
            txtReturnTime_Input.ppEnabled = True
        End If
        'DLCLSTP001_014 END

    End Sub

    ''' <summary>
    ''' ＰＤＦ出力先パス取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ms_GetDirpath() As String
        Dim strPath As String = Nothing
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
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

                cmdDB = New SqlCommand("ZCMPSEL009", conDB)

                'PDF出力先
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(ClsCMDataConnect.pfSet_Param("FILECLASS_CD", SqlDbType.NVarChar, "11"))                              'ファイル種別
                    .Add(ClsCMDataConnect.pfSet_Param("SERVER_ADDRESS", SqlDbType.NVarChar, 20, ParameterDirection.Output))   'アドレス
                    .Add(ClsCMDataConnect.pfSet_Param("FOLDER_NM", SqlDbType.NVarChar, 20, ParameterDirection.Output))        'フォルダ名
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                strPath = cmdDB.Parameters("SERVER_ADDRESS").Value.ToString + "\" + cmdDB.Parameters("FOLDER_NM").Value.ToString

                Return strPath

            Catch ex As SqlException

                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保存先パス")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Return Nothing

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保存先パス")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Return Nothing

            Finally

                'データセット破棄
                Call ClsCMDataConnect.psDisposeDataSet(dstOrders)
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Return Nothing

        End If
    End Function

    ''' <summary>
    ''' PDF出力用のデータテーブル作成
    ''' </summary>
    ''' <param name="dtPDF"></param>
    ''' <remarks></remarks>
    Private Sub ms_GetPdfDatatable(ByRef dtPDF As DataTable)

        Dim dtRow As DataRow
        Dim dtChg As DateTime = Nothing
        Dim dtRtn As DateTime = Nothing

        '項目作成
        dtPDF.Columns.Add("対象日付")
        dtPDF.Columns.Add("区分")
        dtPDF.Columns.Add("依頼方法")
        dtPDF.Columns.Add("作業内容")
        dtPDF.Columns.Add("TBOXID")
        dtPDF.Columns.Add("ホール名")
        dtPDF.Columns.Add("種別")
        dtPDF.Columns.Add("特別運用モード変更日時")
        dtPDF.Columns.Add("通常運用モード戻し日時")
        'DLCLSTP001_013
        'dtPDF.Columns.Add("工事有無")
        'dtPDF.Columns.Add("深夜営業有無")
        dtPDF.Columns.Add("工事PASS")
        dtPDF.Columns.Add("深夜PASS")
        'DLCLSTP001_013 END
        dtPDF.Columns.Add("現状モード")
        dtPDF.Columns.Add("備考")
        dtPDF.Columns.Add("印刷日時")
        dtPDF.Columns.Add("設定状況")
        dtPDF.Columns.Add("戻し状況")
        dtPDF.Columns.Add("作業内容コード")


        For Each rowData As DataRow In ViewState("帳票用").Rows

            dtRow = dtPDF.NewRow()          'データテーブルの行定義

            dtRow("対象日付") = DateTime.Now.ToString("MM月dd日")
            dtRow("区分") = rowData.Item("センタ区分名").ToString.Replace(Environment.NewLine, String.Empty)
            dtRow("依頼方法") = "PC"
            dtRow("作業内容") = rowData.Item("設定依頼内容").ToString().Replace(Environment.NewLine, String.Empty)
            dtRow("TBOXID") = rowData.Item("ＴＢＯＸＩＤ").ToString().Replace(Environment.NewLine, String.Empty)
            dtRow("ホール名") = rowData.Item("ホール名").ToString().Replace(Environment.NewLine, String.Empty)
            dtRow("種別") = rowData.Item("システム区分").ToString().Replace(Environment.NewLine, String.Empty)
            If DateTime.TryParse(rowData.Item("設定日時").ToString().Replace(Environment.NewLine, String.Empty), dtChg) Then
                dtRow("特別運用モード変更日時") = dtChg.ToString("yyyy/MM/dd HH:mm")
            Else
                dtRow("特別運用モード変更日時") = String.Empty
            End If
            If DateTime.TryParse(rowData.Item("戻し日時").ToString().Replace(Environment.NewLine, String.Empty), dtRtn) Then
                dtRow("通常運用モード戻し日時") = dtRtn.ToString("yyyy/MM/dd HH:mm")
            Else
                dtRow("通常運用モード戻し日時") = String.Empty
            End If
            'DLCLSTP001_013
            'dtRow("工事有無") = rowData.Item("工事").ToString().Replace(Environment.NewLine, String.Empty)
            'dtRow("深夜営業有無") = rowData.Item("設定状態").tostring()
            dtRow("工事PASS") = rowData.Item("工事PASS")
            dtRow("深夜PASS") = rowData.Item("深夜PASS")
            'DLCLSTP001_013 END
            dtRow("現状モード") = rowData.Item("進捗状況").ToString().Replace(Environment.NewLine, String.Empty)
            dtRow("備考") = rowData.Item("設定依頼メッセージ").ToString().Replace(Environment.NewLine, String.Empty)
            dtRow("印刷日時") = DateTime.Now.ToString("yyyy/MM/dd HH:mm")
            dtRow("設定状況") = rowData.Item("設定状態").ToString().Replace(Environment.NewLine, String.Empty)
            dtRow("戻し状況") = rowData.Item("戻し状態").ToString().Replace(Environment.NewLine, String.Empty)
            dtRow("作業内容コード") = rowData.Item("SET_CD").ToString().Replace(Environment.NewLine, String.Empty)

            dtPDF.Rows.Add(dtRow)
        Next

    End Sub

    ''' <summary>
    ''' 入力チェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msInput_Check()

        If txtSrvCd1.ppText.Trim = String.Empty Then
            txtSrvCd1.psSet_ErrorNo("5001", "会員サービスコード１")
        Else
            If Not Regex.IsMatch(txtSrvCd1.ppText.Trim, "^01[0-9]{2}9900$") Then
                txtSrvCd1.psSet_ErrorNo("4001", "会員サービスコード１", "01nn9900の形式")
            End If
        End If

        If txtSrvCd2.ppText.Trim = String.Empty Then
            txtSrvCd2.psSet_ErrorNo("5001", "会員サービスコード２")
        Else
            If Not Regex.IsMatch(txtSrvCd2.ppText.Trim, "^02[0-9]{2}9900$") Then
                txtSrvCd2.psSet_ErrorNo("4001", "会員サービスコード２", "02nn9900の形式")
            End If
        End If

    End Sub

    ''' <summary>
    ''' 一括リトライ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msbatchRetry()

        Dim sqlCmd As SqlCommand = Nothing
        Dim conDB As SqlConnection = Nothing
        Dim intNmlFlg As Integer = 0
        Dim intAbNmlFlg As Integer = 0
        Dim intLoopCnt As Integer = 0
        Dim intChkCnt As Integer = 0
        Dim objTrans As SqlTransaction = Nothing
        Dim strLogNo As String = Nothing
        Dim strCNTL_NO As String = Nothing
        Dim strSEQNO As String = Nothing
        Dim objDS As New DataSet
        Dim blnRetIns As Boolean = False
        Dim strRetryCnt As String = ""
        Dim strNewCntl_No As String = ""

        Try

            If clsDataConnect.pfOpen_Database(conDB) Then
                objTrans = conDB.BeginTransaction

                'DLCLSTP001_005
                '既存のキープをクリア
                mstbRetryNo.Clear()
                'DLCLSTP001_005

                For Each rowData As GridViewRow In grvList.Rows
                    'チェックがついた行のみ処理を行う
                    If CType(rowData.FindControl("一括リトライ"), CheckBox).Checked = True Then

                        '排他制御用変数
                        Dim strExclusiveDate As String = Nothing
                        Dim arTable_Name As New ArrayList
                        Dim arKey As New ArrayList

                        '排他情報削除
                        If Not Me.Master.Master.ppExclusiveDateDtl = String.Empty Then

                            clsExc.pfDel_Exclusive(Me,
                                            Session(P_SESSION_SESSTION_ID),
                                            Me.Master.Master.ppExclusiveDateDtl)

                            Me.Master.Master.ppExclusiveDateDtl = String.Empty

                        End If

                        'ロック対象テーブル名の登録
                        arTable_Name.Insert(0, "D47_DLLSEND")

                        'ロックテーブルキー項目の登録
                        arKey.Insert(0, CType(rowData.FindControl("依頼番号"), TextBox).Text)   '依頼番号
                        arKey.Insert(1, CType(rowData.FindControl("明細番号"), TextBox).Text)   '明細番号

                        '排他情報確認処理
                        If clsExc.pfSel_Exclusive(strExclusiveDate,
                                           Me,
                                           Session(P_SESSION_IP),
                                           Session(P_SESSION_PLACE),
                                           Session(P_SESSION_USERID),
                                           Session(P_SESSION_SESSTION_ID),
                                           ViewState(P_SESSION_GROUP_NUM),
                                           M_DISP_ID,
                                           arTable_Name,
                                           arKey) = 0 Then

                            '登録年月日時刻(明細)に登録
                            Me.Master.Master.ppExclusiveDate = strExclusiveDate

                        Else
                            '排他ロック中
                            Exit Sub

                        End If

                        '登録データの取得
                        strCNTL_NO = Nothing
                        strSEQNO = Nothing
                        objDS = Nothing
                        blnRetIns = False
                        strCNTL_NO = CType(rowData.FindControl("依頼番号"), TextBox).Text.Replace(Environment.NewLine, "")   '依頼番号
                        strSEQNO = CType(rowData.FindControl("明細番号"), TextBox).Text   '明細番号
                        strLogNo = strCNTL_NO + " "

                        sqlCmd = New SqlCommand("DLCLSTP001_S7", conDB)
                        sqlCmd.Transaction = objTrans
                        'DLL変更依頼　取得
                        With sqlCmd.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("prm_CNTL_NO", SqlDbType.NVarChar, strCNTL_NO))                'TBOXID(FROM)
                            .Add(pfSet_Param("prm_SEQNO", SqlDbType.NVarChar, strSEQNO))                'TBOXID(TO)
                        End With
                        'データ取得
                        objDS = clsDataConnect.pfGet_DataSet(sqlCmd)

                        If objDS.Tables.Count > 0 Then
                            If objDS.Tables(0).Rows.Count > 0 Then
                                'If objDS.Tables(0).Rows(0).Item("CNG_RET_DENBUN").ToString = "DNBN_NG" OrElse (objDS.Tables(0).Rows(0).Item("CNG_STTS").ToString <> "0" And objDS.Tables(0).Rows(0).Item("CNG_STTS").ToString <> "4" And objDS.Tables(0).Rows(0).Item("CNG_STTS").ToString <> "Z") Then
                                If CType(rowData.FindControl("変更リトライ可能"), TextBox).Text = "RET_OK" Then
                                    '新規でＤＬＬ変更情報を追加する                               電文エラー　　　　　　
                                    '変更時に失敗していたら変更でリトライ
                                    'ＩＮＳ処理
                                    strRetryCnt = ""
                                    If objDS.Tables(0).Rows(0).Item("D47_RETCNT") Is DBNull.Value Then
                                        strRetryCnt = "01"
                                    Else
                                        strRetryCnt = (Integer.Parse(objDS.Tables(0).Rows(0).Item("D47_RETCNT")) + 1).ToString("D2")
                                    End If
                                    strNewCntl_No = ""
                                    If objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Length = 10 Then
                                        strNewCntl_No = objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Substring(0, 7)
                                    ElseIf objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Length = 15 Then
                                        strNewCntl_No = objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Substring(0, 12)
                                    ElseIf objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Length = 17 Then
                                        strNewCntl_No = objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Substring(0, 14)
                                    Else
                                        strNewCntl_No = objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString
                                    End If
                                    sqlCmd = New SqlCommand("DLCLSTP001_I2", conDB)
                                    sqlCmd.CommandType = CommandType.StoredProcedure
                                    sqlCmd.Transaction = objTrans
                                    With sqlCmd.Parameters
                                        'パラメータ設定
                                        .Add(pfSet_Param("prm_dllsend_no", SqlDbType.NVarChar, strNewCntl_No & "R" & strRetryCnt)) '管理番号
                                        .Add(pfSet_Param("prm_seqno", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SEQNO").ToString))       '明細番号
                                        .Add(pfSet_Param("prm_nl_flg", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NL_FLG").ToString))     'ＮＬ区分
                                        .Add(pfSet_Param("prm_req", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_REQ").ToString))           '依頼方法
                                        .Add(pfSet_Param("prm_set_cd", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SET_CD").ToString))     '作業内容コード
                                        .Add(pfSet_Param("prm_set_nm", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SET_NM").ToString))     '作業内容
                                        .Add(pfSet_Param("prm_tboxid", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_TBOXID").ToString))     'ＴＢＯＸＩＤ
                                        .Add(pfSet_Param("prm_hall_cd", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_HALL_CD").ToString))    'ホールコード
                                        .Add(pfSet_Param("prm_hall_nm", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_HALL_NM").ToString))    'ホール名
                                        .Add(pfSet_Param("prm_tboxclass_cd", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_TBOXCLASS_CD").ToString)) 'ＴＢＯＸ機種コード
                                        .Add(pfSet_Param("prm_tboxclass_nm", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_TBOXCLASS_NM").ToString)) 'ＴＢＯＸ機種
                                        .Add(pfSet_Param("prm_spmodest_dt", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SPMODEST_DT").ToString)) '特別運用モード変更日時
                                        .Add(pfSet_Param("prm_spmodest_cd", SqlDbType.NVarChar, "0")) '特別運用変更内容コード
                                        .Add(pfSet_Param("prm_nmmodest_dt", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NMMODEST_DT").ToString)) '通常運用モード戻し日時
                                        .Add(pfSet_Param("prm_nmmodest_cd", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NMMODEST_CD").ToString)) '通常運用変更内容コード
                                        .Add(pfSet_Param("prm_setreq_nm", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SETREQ_NM").ToString))  '設定依頼者
                                        .Add(pfSet_Param("prm_set_info", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SET_INFO").ToString))   '設定情報
                                        .Add(pfSet_Param("prm_ret_info", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_RET_INFO").ToString))   '戻し情報
                                        If Me.ChkSeisanki.Enabled Then
                                            .Add(pfSet_Param("prm_adjmachine_chg", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_ADJMACHINE_CHG").ToString)) '精算機変更
                                            .Add(pfSet_Param("prm_adjmachine_rtn", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_ADJMACHINE_RTN").ToString)) '精算機戻し
                                        Else
                                            .Add(pfSet_Param("prm_adjmachine_chg", SqlDbType.NVarChar, "0"))                                '精算機変更
                                            .Add(pfSet_Param("prm_adjmachine_rtn", SqlDbType.NVarChar, "0"))                                '精算機戻し
                                        End If
                                        .Add(pfSet_Param("prm_sendend_flg", SqlDbType.NVarChar, "2"))                                       '案件終了フラグ
                                        .Add(pfSet_Param("prm_insert_usr", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_INSERT_USR").ToString)) '登録者
                                        .Add(pfSet_Param("prm_proctime_cls", SqlDbType.NVarChar, "0")) '処理時間区分
                                        .Add(pfSet_Param("prm_RETRY_CNT", SqlDbType.NVarChar, strRetryCnt)) 'リトライ回数
                                        .Add(pfSet_Param("prm_MODE_CD", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_MODE_CD").ToString)) 'モードコード
                                        .Add(pfSet_Param("prm_SETRET_FLG", SqlDbType.NVarChar, "")) '入り戻し依頼フラグ
                                        .Add(pfSet_Param("prm_CNST_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_CNST_FLG").ToString)) '工事有無
                                        .Add(pfSet_Param("prm_DNIGHT_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_DNIGHT_FLG").ToString)) '深夜営業有無
                                        .Add(pfSet_Param("prm_CNST_NM", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_CNST_NM").ToString)) '工事区分名
                                        .Add(pfSet_Param("prm_CNST_CLS", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_CNST_CLS").ToString)) '工事区分
                                        .Add(pfSet_Param("prm_SET_MSG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SET_MSG").ToString)) '設定依頼メッセージ
                                        .Add(pfSet_Param("prm_NOTETEXT", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NOTETEXT").ToString)) '備考
                                        .Add(pfSet_Param("prm_SPMODESND_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SPMODESND_FLG").ToString)) '特別運用送信フラグ
                                        .Add(pfSet_Param("prm_NMMODESND_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NMMODESND_FLG").ToString)) '通常運用送信フラグ
                                        .Add(pfSet_Param("prm_SPMODERCV_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SPMODERCV_FLG").ToString)) '特別運用結果フラグ
                                        .Add(pfSet_Param("prm_NMMODERCV_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NMMODERCV_FLG").ToString)) '通常運用結果フラグ
                                        .Add(pfSet_Param("prm_STATUS_CD", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_STATUS_CD").ToString)) '進捗状況
                                    End With

                                    sqlCmd.ExecuteNonQuery()
                                    blnRetIns = True

                                    'ElseIf objDS.Tables(0).Rows(0).Item("RTN_RET_DENBUN").ToString = "DNBN_NG" OrElse (objDS.Tables(0).Rows(0).Item("RTN_STTS").ToString <> "0" And objDS.Tables(0).Rows(0).Item("RTN_STTS").ToString <> "4" And objDS.Tables(0).Rows(0).Item("RTN_STTS").ToString <> "Z") Then
                                ElseIf CType(rowData.FindControl("戻しリトライ可能"), TextBox).Text = "RET_OK" Then

                                    '変更済でＤＬＬ変更情報を追加する                             電文エラー　　　　　　　　　　　　　　　　　　　　　　　　　　　　　正常終了　　　　　　　　　　　　　　　　　　　　　　　　　テスト待ち　　　　　　　　　　　　　　　　　　　　　　　　　未処理
                                    '戻し時に失敗していたら戻しでリトライ
                                    'ＩＮＳ処理
                                    strRetryCnt = ""
                                    If objDS.Tables(0).Rows(0).Item("D47_RETCNT") Is DBNull.Value Then
                                        strRetryCnt = "01"
                                    Else
                                        strRetryCnt = (Integer.Parse(objDS.Tables(0).Rows(0).Item("D47_RETCNT")) + 1).ToString("D2")
                                    End If
                                    strNewCntl_No = ""
                                    If objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Length = 10 Then
                                        strNewCntl_No = objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Substring(0, 7)
                                    ElseIf objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Length = 15 Then
                                        strNewCntl_No = objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Substring(0, 12)
                                    ElseIf objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Length = 17 Then
                                        strNewCntl_No = objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Substring(0, 14)
                                    Else
                                        strNewCntl_No = objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString
                                    End If
                                    sqlCmd = New SqlCommand("DLCLSTP001_I2", conDB)
                                    sqlCmd.CommandType = CommandType.StoredProcedure
                                    sqlCmd.Transaction = objTrans
                                    With sqlCmd.Parameters
                                        'パラメータ設定
                                        .Add(pfSet_Param("prm_dllsend_no", SqlDbType.NVarChar, strNewCntl_No & "R" & strRetryCnt)) '管理番号
                                        .Add(pfSet_Param("prm_seqno", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SEQNO").ToString))       '明細番号
                                        .Add(pfSet_Param("prm_nl_flg", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NL_FLG").ToString))     'ＮＬ区分
                                        .Add(pfSet_Param("prm_req", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_REQ").ToString))           '依頼方法
                                        .Add(pfSet_Param("prm_set_cd", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SET_CD").ToString))     '作業内容コード
                                        .Add(pfSet_Param("prm_set_nm", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SET_NM").ToString))     '作業内容
                                        .Add(pfSet_Param("prm_tboxid", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_TBOXID").ToString))     'ＴＢＯＸＩＤ
                                        .Add(pfSet_Param("prm_hall_cd", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_HALL_CD").ToString))    'ホールコード
                                        .Add(pfSet_Param("prm_hall_nm", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_HALL_NM").ToString))    'ホール名
                                        .Add(pfSet_Param("prm_tboxclass_cd", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_TBOXCLASS_CD").ToString)) 'ＴＢＯＸ機種コード
                                        .Add(pfSet_Param("prm_tboxclass_nm", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_TBOXCLASS_NM").ToString)) 'ＴＢＯＸ機種
                                        .Add(pfSet_Param("prm_spmodest_dt", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SPMODEST_DT").ToString)) '特別運用モード変更日時
                                        .Add(pfSet_Param("prm_spmodest_cd", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SPMODEST_CD").ToString)) '特別運用変更内容コード
                                        .Add(pfSet_Param("prm_nmmodest_dt", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NMMODEST_DT").ToString)) '通常運用モード戻し日時
                                        .Add(pfSet_Param("prm_nmmodest_cd", SqlDbType.NVarChar, "0")) '通常運用変更内容コード
                                        .Add(pfSet_Param("prm_setreq_nm", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SETREQ_NM").ToString))  '設定依頼者
                                        .Add(pfSet_Param("prm_set_info", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SET_INFO").ToString))   '設定情報
                                        .Add(pfSet_Param("prm_ret_info", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_RET_INFO").ToString))   '戻し情報
                                        If Me.ChkSeisanki.Enabled Then
                                            .Add(pfSet_Param("prm_adjmachine_chg", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_ADJMACHINE_CHG").ToString)) '精算機変更
                                            .Add(pfSet_Param("prm_adjmachine_rtn", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_ADJMACHINE_RTN").ToString)) '精算機戻し
                                        Else
                                            .Add(pfSet_Param("prm_adjmachine_chg", SqlDbType.NVarChar, "0"))                                '精算機変更
                                            .Add(pfSet_Param("prm_adjmachine_rtn", SqlDbType.NVarChar, "0"))                                '精算機戻し
                                        End If
                                        .Add(pfSet_Param("prm_sendend_flg", SqlDbType.NVarChar, "02"))                                       '案件終了フラグ
                                        .Add(pfSet_Param("prm_insert_usr", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_INSERT_USR").ToString)) '登録者
                                        .Add(pfSet_Param("prm_proctime_cls", SqlDbType.NVarChar, "4")) '処理時間区分
                                        .Add(pfSet_Param("prm_RETRY_CNT", SqlDbType.NVarChar, strRetryCnt)) 'リトライ回数
                                        .Add(pfSet_Param("prm_MODE_CD", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_MODE_CD").ToString)) 'モードコード
                                        .Add(pfSet_Param("prm_SETRET_FLG", SqlDbType.NVarChar, "0")) '入り戻し依頼フラグ
                                        .Add(pfSet_Param("prm_CNST_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_CNST_FLG").ToString)) '工事有無
                                        .Add(pfSet_Param("prm_DNIGHT_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_DNIGHT_FLG").ToString)) '深夜営業有無
                                        .Add(pfSet_Param("prm_CNST_NM", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_CNST_NM").ToString)) '工事区分名
                                        .Add(pfSet_Param("prm_CNST_CLS", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_CNST_CLS").ToString)) '工事区分
                                        .Add(pfSet_Param("prm_SET_MSG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SET_MSG").ToString)) '設定依頼メッセージ
                                        .Add(pfSet_Param("prm_NOTETEXT", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NOTETEXT").ToString)) '備考
                                        .Add(pfSet_Param("prm_SPMODESND_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SPMODESND_FLG").ToString)) '特別運用送信フラグ
                                        .Add(pfSet_Param("prm_NMMODESND_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NMMODESND_FLG").ToString)) '通常運用送信フラグ
                                        .Add(pfSet_Param("prm_SPMODERCV_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SPMODERCV_FLG").ToString)) '特別運用結果フラグ
                                        .Add(pfSet_Param("prm_NMMODERCV_FLG", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NMMODERCV_FLG").ToString)) '通常運用結果フラグ
                                        .Add(pfSet_Param("prm_STATUS_CD", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_STATUS_CD").ToString)) '進捗状況
                                    End With

                                    sqlCmd.ExecuteNonQuery()
                                    blnRetIns = True

                                    'End If
                                End If
                                '既存の情報を削除扱いにする（非表示になるはず）
                                '更新するときＤＥＬＥＴＥフラグを０、１以外で更新
                                'ＵＰＤＡＴＥ処理
                                If blnRetIns = True Then

                                    sqlCmd = New SqlCommand("DLCLSTP001_D1", conDB)
                                    sqlCmd.CommandType = CommandType.StoredProcedure
                                    sqlCmd.Transaction = objTrans
                                    With sqlCmd.Parameters
                                        'パラメータ設定
                                        .Add(pfSet_Param("prm_dllsend_no", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_DLLSEND_NO").ToString.Replace(Environment.NewLine, ""))) '管理番号
                                        .Add(pfSet_Param("prm_seqno", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_SEQNO").ToString)) '明細番号
                                        .Add(pfSet_Param("prm_notetext", SqlDbType.NVarChar, objDS.Tables(0).Rows(0).Item("D47_NOTETEXT").ToString)) '明細番号
                                        .Add(pfSet_Param("prm_update_usr", SqlDbType.NVarChar, Session(P_SESSION_USERID)))              '更新者
                                        .Add(pfSet_Param("prm_DELETE_FLG", SqlDbType.NVarChar, "2"))              '更新値
                                    End With
                                    'データ追加／更新                                  
                                    sqlCmd.ExecuteNonQuery()
                                    '更新が正常終了
                                    intNmlFlg += 1

                                End If
                            End If
                        End If
                        intChkCnt += 1

                        If mstbRetryNo Is Nothing Then
                            mstbRetryNo.Append("'" & strNewCntl_No & "R" & strRetryCnt & "'")
                        Else
                            If mstbRetryNo.Length > 0 Then
                                mstbRetryNo.Append(",'" & strNewCntl_No & "R" & strRetryCnt & "'")
                            Else
                                mstbRetryNo.Append("'" & strNewCntl_No & "R" & strRetryCnt & "'")
                            End If
                        End If

                    End If
                    intLoopCnt += 1

                Next

                If grvList.Rows.Count = intLoopCnt Then
                    objTrans.Commit()
                Else
                    objTrans.Rollback()
                End If

                '排他情報削除 'DLCLSTP001_019
                If Not Me.Master.Master.ppExclusiveDate = String.Empty Then

                    clsExc.pfDel_Exclusive(Me,
                                    Session(P_SESSION_SESSTION_ID),
                                    Me.Master.Master.ppExclusiveDate)

                    Me.Master.Master.ppExclusiveDate = String.Empty

                End If

            Else

                '検索失敗
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "ＤＢ接続失敗", "Catch")
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End If

        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
            intAbNmlFlg += 1
            objTrans.Rollback()
            '処理終了
            Exit Sub

        Finally

            'エラーチェック
            If intChkCnt <> 0 Then
                If intAbNmlFlg = 0 Then
                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "ＤＬＬ変更依頼　リトライ登録")
                Else
                    If intNmlFlg = 0 Then
                    Else
                        psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "管理番号 " + strLogNo + "でＤＬＬ変更依頼リトライ登録")
                    End If
                End If
            Else
                psMesBox(Me, "00007", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)
            End If

            'データセットの破棄
            Call ClsCMDataConnect.psDisposeDataSet(objDS)
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If


        End Try

        '再描画
        ViewState("btnreload") = "0"

        'なんでかリトライリロードからリロードに変更されてた。きっと朝比奈の適当な指示のせい
        '        ms_GetDLLhenko("Reload")
        ms_GetDLLhenko("RetryReload")

        'リトライボタン更新
        msGetRetryBtn()

    End Sub

    'DLCLSTP001_003
    ''' <summary>
    ''' 登録エリアクリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClear()

        '更新項目の初期化
        Me.txtTboxID_Input.ppText = Nothing
        Me.txtTboxID_Input.ppEnabled = True
        Me.lblCenterCls_Input.Text = String.Empty
        Me.lblSystem_Input.Text = String.Empty
        Me.lblHallName_Input.Text = String.Empty
        Me.ddlSettei_input.SelectedIndex = 0
        Me.txtStteiDay_input.ppText = Nothing
        Me.txtStteiTime_input.ppHourText = Nothing
        Me.txtStteiTime_input.ppMinText = Nothing
        Me.txtReturnDay_Input.ppText = Nothing
        Me.txtReturnTime_Input.ppHourText = Nothing
        Me.txtReturnTime_Input.ppMinText = Nothing
        Me.txtIraisya.ppText = Nothing
        Me.txtSrvCd1.ppText = String.Empty
        Me.txtSrvCd2.ppText = String.Empty
        'DLCLSTP001-001
        Me.txtNotes.ppText = String.Empty
        'DLCLSTP001-001 END
        '--2014/04/15 中川　ここから
        '排他情報削除
        If Not Me.Master.Master.ppExclusiveDateDtl = String.Empty Then

            clsExc.pfDel_Exclusive(Me,
                            Session(P_SESSION_SESSTION_ID),
                            Me.Master.Master.ppExclusiveDateDtl)

            Me.Master.Master.ppExclusiveDateDtl = String.Empty

        End If
        '--2014/04/15 中川　ここまで

        '更新、削除ボタンの非活性化
        Me.btnUpdate.Enabled = False
        Me.btnDelete.Enabled = False
        Me.btnInsert.Enabled = True
        Me.btnClear.Enabled = True
        'DLCLSTP001-002
        'ドロップダウンリストの初期設定を取得
        If Not msGetDLLData(1) Then

            'システムエラー
            Throw New Exception

        End If
        'DLCLSTP001-002 END

        'DLCLSTP001_003
        Me.ChkSeisanki.Items(0).Selected = False
        Me.ChkSeisanki.Items(1).Selected = False
        Me.ChkSeisanki.Enabled = False
        'DLCLSTP001_003 END

        'DLCLSTP001_014
        Me.txtStteiDay_input.ppEnabled = True
        Me.txtStteiTime_input.ppEnabled = True
        Me.txtReturnDay_Input.ppEnabled = True
        Me.txtReturnTime_Input.ppEnabled = True
        'DLCLSTP001_014 END

        'DLCLSTP001_018
        Me.txtSrvCd1.ppEnabled = False
        Me.txtSrvCd2.ppEnabled = False
        'DLCLSTP001_018 END

        'DLCLSTP001_010
        ViewState("SEQ_NO") = Nothing
        'DLCLSTP001_010 END

    End Sub
    'DLCLSTP001_003　END

    ''' <summary>
    ''' 帳票用データテーブル整形処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msPrintRemake(ByVal dtPrint As DataTable)

        '完了した変更依頼レコードの削除
        If dtPrint.Rows.Count > 0 Then
            Dim intdlt As Integer = 0
            For intCnt As Integer = 0 To dtPrint.Rows.Count - 1
                If dtPrint.Rows(intdlt).Item("進捗状況").ToString = "完了" _
                    OrElse dtPrint.Rows(intdlt).Item("設定依頼内容コード").ToString = "02" _
                    OrElse dtPrint.Rows(intdlt).Item("設定依頼内容コード").ToString = "03" Then 'DLCLSTP001_013 深夜終夜除外追加
                    dtPrint.Rows(intdlt).Delete()
                    dtPrint.AcceptChanges()
                    If dtPrint.Rows.Count = intdlt Then
                        Exit For
                    End If
                Else
                    If dtPrint.Rows.Count = intdlt Then
                        Exit For
                    End If
                    intdlt += 1
                End If
            Next
        End If

        'DLCLSTP001_012
        Dim dvRpt As New DataView(dtPrint)
        dvRpt.Sort = "RptSort_1,RptSort_2"

        'ViewState("帳票用") = dtPrint
        ViewState("帳票用") = dvRpt.ToTable
        'DLCLSTP001_012 END

    End Sub

    ''' <summary>
    ''' 設定依頼内容整合性チェック 'DLCLSTP001_016
    ''' </summary>
    ''' <remarks>設定依頼内容とシステム分類(IC/LUT)の整合性チェック</remarks>
    Private Function check_SetCd() As Boolean

        Dim dstSelect As New DataSet

        check_SetCd = False

        Try
            'SQLコマンド設定
            Using cmdSQL As SqlCommand = New SqlCommand("SPCDB.dbo." + M_DISP_ID & "_S11")

                'パラメータ設定
                With cmdSQL.Parameters
                    .Add(ClsCMDataConnect.pfSet_Param("prm_TBOXID", SqlDbType.NVarChar, Me.txtTboxID_Input.ppText))         'TBOXID
                    .Add(ClsCMDataConnect.pfSet_Param("prm_WRKCD", SqlDbType.NVarChar, Me.ddlSettei_input.SelectedValue.Substring(0, 2)))   '作業コード
                End With

                'データ取得実行
                If ClsCMDataConnect.pfExec_StoredProcedure(Me, "設定依頼内容の整合性", cmdSQL, dstSelect) = False Then
                    Exit Function
                End If

                '整合性チェック
                If dstSelect.Tables(0).Rows.Count > 0 Then
                    check_SetCd = True
                Else
                    Me.txtTboxID_Input.psSet_ErrorNo("2012", "設定依頼内容とシステム分類")    '{0}が一致しません。
                    check_SetCd = False
                End If

            End Using

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "設定依頼内容の整合性チェック")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

            'データセットの破棄
            Call ClsCMDataConnect.psDisposeDataSet(dstSelect)

        End Try

    End Function


#End Region

#Region "終了処理プロシージャ"

    ''' <summary>
    ''' 設定日時、戻し日時の検証チェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_cheackVlid()

        'DLCLSTP001_006
        '日付のみの入力は許可、時間のみの入力は不許可
        '--------------------------------
        '2014/04/21 石橋　ここから
        '--------------------------------
        '設定日時(FROM)の年月日のみ入力している場合
        'If Not Me.txtSetteiDayFrom.ppText Is Nothing _
        '    And Me.txtSetteiTimeFrom.ppHourText Is Nothing _
        '    And Me.txtSetteiTimeFrom.ppMinText Is Nothing Then
        '    Me.txtSetteiTimeFrom.psSet_ErrorNo("5001", Me.txtSetteiDayFrom.ppName)
        'End If
        '        If (Me.txtSetteiDayFrom.ppText <> String.Empty) _
        '            And (Me.txtSetteiTimeFrom.ppHourText = String.Empty) _
        '            And (Me.txtSetteiTimeFrom.ppMinText = String.Empty) Then
        '            Me.txtSetteiTimeFrom.psSet_ErrorNo("5001", Me.txtSetteiDayFrom.ppName)
        '        End If

        '設定日時(FROM)の時分のみ入力している場合
        'If Me.txtSetteiDayFrom.ppText Is Nothing _
        '    And Not Me.txtSetteiTimeFrom.ppHourText Is Nothing _
        '    And Not Me.txtSetteiTimeFrom.ppMinText Is Nothing Then
        '    Me.txtSetteiDayFrom.psSet_ErrorNo("5001", Me.txtSetteiDayFrom.ppName)
        'End If
        If (Me.txtSetteiDayFrom.ppText = String.Empty) _
            And (Me.txtSetteiTimeFrom.ppHourText <> String.Empty) _
            And (Me.txtSetteiTimeFrom.ppMinText <> String.Empty) Then
            Me.txtSetteiDayFrom.psSet_ErrorNo("5001", Me.txtSetteiDayFrom.ppName)
        End If

        '設定日時(TO)の年月日のみ入力している場合
        'If Not Me.txtSetteiDayTo.ppText Is Nothing _
        '    And Me.txtSetteiTimeTo.ppHourText Is Nothing _
        '    And Me.txtSetteiTimeTo.ppMinText Is Nothing Then
        '    Me.txtSetteiTimeTo.psSet_ErrorNo("5001", Me.txtSetteiDayFrom.ppName)
        'End If
        '        If (Me.txtSetteiDayTo.ppText <> String.Empty _
        '            And Me.txtSetteiTimeTo.ppHourText = String.Empty _
        '            And Me.txtSetteiTimeTo.ppMinText = String.Empty) Then
        '            Me.txtSetteiTimeTo.psSet_ErrorNo("5001", Me.txtSetteiDayFrom.ppName)
        '        End If

        '設定日時(TO)の時分のみ入力している場合
        'If Me.txtSetteiDayTo.ppText Is Nothing _
        '    And Not Me.txtSetteiTimeTo.ppHourText Is Nothing _
        '    And Not Me.txtSetteiTimeTo.ppMinText Is Nothing Then
        '    Me.txtSetteiDayTo.psSet_ErrorNo("5001", Me.txtSetteiDayFrom.ppName)
        'End If
        If Me.txtSetteiDayTo.ppText = String.Empty _
            And Me.txtSetteiTimeTo.ppHourText <> String.Empty _
            And Me.txtSetteiTimeTo.ppMinText <> String.Empty Then
            Me.txtSetteiDayTo.psSet_ErrorNo("5001", Me.txtSetteiDayTo.ppName)
        End If

        Dim dtSetteiFrom As String
        Dim dtSetteiTo As String
        If Not Me.txtSetteiDayTo.ppText = String.Empty Then
            If Me.txtSetteiTimeFrom.ppHourText Is Nothing Or Me.txtSetteiTimeFrom.ppHourText = "" Then
                dtSetteiFrom = Me.txtSetteiDayFrom.ppText & " 00:00:00"
            Else
                dtSetteiFrom = Me.txtSetteiDayFrom.ppText & " " & Me.txtSetteiTimeFrom.ppHourText & ":" & _
                               Me.txtSetteiTimeFrom.ppMinText & ":" & "00"
            End If
            If Me.txtSetteiTimeTo.ppHourText Is Nothing Or Me.txtSetteiTimeTo.ppHourText = "" Then
                dtSetteiTo = Me.txtSetteiDayTo.ppText & " 23:59:59"
            Else
                dtSetteiTo = Me.txtSetteiDayTo.ppText & " " & Me.txtSetteiTimeTo.ppHourText & ":" & _
                             Me.txtSetteiTimeTo.ppMinText & ":" & "00"
            End If

            '設定日時（From）より設定日時（To）が大きい場合
            If dtSetteiFrom.CompareTo(dtSetteiTo) > 0 Then
                Me.txtSetteiDayFrom.psSet_ErrorNo("2001", Me.txtSetteiDayFrom.ppName)
            End If
        End If

        '戻し日時(FROM)の年月日のみ入力している場合
        'If Not Me.txtReturnDayFrom.ppText Is Nothing _
        '    And Me.txtReturnTimeFrom.ppHourText Is Nothing _
        '    And Me.txtReturnTimeFrom.ppMinText Is Nothing Then
        '    Me.txtReturnTimeFrom.psSet_ErrorNo("5001", Me.txtSetteiDayFrom.ppName)
        'End If
        '        If Me.txtReturnDayFrom.ppText <> String.Empty _
        '            And Me.txtReturnTimeFrom.ppHourText = String.Empty _
        '            And Me.txtReturnTimeFrom.ppMinText = String.Empty Then
        '            Me.txtReturnTimeFrom.psSet_ErrorNo("5001", Me.txtSetteiDayFrom.ppName)
        '        End If

        '戻し日時(FROM)の時分のみ入力している場合
        'If Me.txtReturnDayFrom.ppText Is Nothing _
        '    And Not Me.txtReturnTimeFrom.ppHourText Is Nothing _
        '    And Not Me.txtReturnTimeFrom.ppMinText Is Nothing Then
        '    Me.txtReturnDayFrom.psSet_ErrorNo("5001", Me.txtSetteiDayFrom.ppName)
        'End If
        If Me.txtReturnDayFrom.ppText = String.Empty _
            And Me.txtReturnTimeFrom.ppHourText <> String.Empty _
            And Me.txtReturnTimeFrom.ppMinText <> String.Empty Then
            Me.txtReturnDayFrom.psSet_ErrorNo("5001", Me.txtReturnDayFrom.ppName)
        End If

        '戻し日時(TO)の年月日のみ入力している場合
        'If Not Me.txtReturnDayTO.ppText Is Nothing _
        '    And Me.txtReturnTimeTO.ppHourText Is Nothing _
        '    And Me.txtReturnTimeTO.ppMinText Is Nothing Then
        '    Me.txtReturnTimeTO.psSet_ErrorNo("5001", Me.txtSetteiDayFrom.ppName)
        'End If
        '        If Me.txtReturnDayTO.ppText <> String.Empty _
        '            And Me.txtReturnTimeTO.ppHourText = String.Empty _
        '            And Me.txtReturnTimeTO.ppMinText = String.Empty Then
        '            Me.txtReturnTimeTO.psSet_ErrorNo("5001", Me.txtSetteiDayFrom.ppName)
        '        End If

        '戻し日時(TO)の時分のみ入力している場合
        'If Me.txtReturnDayTO.ppText Is Nothing _
        '    And Not Me.txtReturnTimeTO.ppHourText Is Nothing _
        '    And Not Me.txtReturnTimeTO.ppMinText Is Nothing Then
        '    Me.txtReturnDayTO.psSet_ErrorNo("5001", Me.txtSetteiDayFrom.ppName)
        'End If
        If Me.txtReturnDayTO.ppText = String.Empty _
            And Me.txtReturnTimeTO.ppHourText <> String.Empty _
            And Me.txtReturnTimeTO.ppMinText <> String.Empty Then
            Me.txtReturnDayTO.psSet_ErrorNo("5001", Me.txtReturnDayTO.ppName)
        End If

        '登録日(FROM)のみ登録している場合
        '        If Me.txtInsDay_From.ppText = String.Empty _
        '            And Me.txtInsTim_From.ppHourText <> String.Empty _
        '            And Me.txtInsTim_From.ppMinText <> String.Empty Then
        '            Me.txtReturnDayTO.psSet_ErrorNo("5001", Me.txtInsDay_From.ppName)
        '        End If
        '登録日(FROM)の時間のみ登録している場合
        If Me.txtInsDay_From.ppText = String.Empty _
            And Me.txtInsTim_From.ppHourText <> String.Empty _
            And Me.txtInsTim_From.ppMinText <> String.Empty Then
            Me.txtInsDay_From.psSet_ErrorNo("5001", Me.txtInsDay_From.ppName)
        End If
        '登録日(TO)のみ登録している場合
        '        If Me.txtInsDay_To.ppText = String.Empty _
        '            And Me.txtInsTim_To.ppHourText <> String.Empty _
        '            And Me.txtInsTim_To.ppMinText <> String.Empty Then
        '            Me.txtReturnDayTO.psSet_ErrorNo("5001", Me.txtInsDay_To.ppName)
        '        End If
        '登録日(TO)の時間のみ登録している場合
        If Me.txtInsDay_To.ppText = String.Empty _
            And Me.txtInsTim_To.ppHourText <> String.Empty _
            And Me.txtInsTim_To.ppMinText <> String.Empty Then
            Me.txtInsDay_To.psSet_ErrorNo("5001", Me.txtInsDay_To.ppName)
        End If
        '更新日(FROM)のみ登録している場合
        '        If Me.txtUpdDay_From.ppText = String.Empty _
        '            And Me.txtUpdTim_From.ppHourText <> String.Empty _
        '            And Me.txtUpdTim_From.ppMinText <> String.Empty Then
        '            Me.txtReturnDayTO.psSet_ErrorNo("5001", Me.txtUpdDay_From.ppName)
        '        End If
        '更新日(FROM)の時間のみ登録している場合
        If Me.txtUpdDay_From.ppText = String.Empty _
            And Me.txtUpdTim_From.ppHourText <> String.Empty _
            And Me.txtUpdTim_From.ppMinText <> String.Empty Then
            Me.txtUpdDay_From.psSet_ErrorNo("5001", Me.txtUpdDay_From.ppName)
        End If
        '更新日(TO)のみ登録している場合
        '        If Me.txtUpdDay_To.ppText = String.Empty _
        '            And Me.txtUpdTim_To.ppHourText <> String.Empty _
        '            And Me.txtUpdTim_To.ppMinText <> String.Empty Then
        '            Me.txtReturnDayTO.psSet_ErrorNo("5001", Me.txtUpdDay_To.ppName)
        '        End If
        '更新日(TO)の時間のみ登録している場合
        If Me.txtUpdDay_To.ppText = String.Empty _
            And Me.txtUpdTim_To.ppHourText <> String.Empty _
            And Me.txtUpdTim_To.ppMinText <> String.Empty Then
            Me.txtUpdDay_To.psSet_ErrorNo("5001", Me.txtUpdDay_To.ppName)
        End If
        'DLCLSTP001_006


        Dim dtReturnFrom As String
        Dim dtReturnTo As String
        If Not Me.txtReturnDayTO.ppText = String.Empty Then
            If Me.txtReturnTimeFrom.ppHourText Is Nothing Or Me.txtReturnTimeFrom.ppHourText = "" Then
                dtReturnFrom = Me.txtReturnDayFrom.ppText & " 00:00:00"
            Else
                dtReturnFrom = Me.txtReturnDayFrom.ppText & " " & Me.txtReturnTimeFrom.ppHourText & ":" & _
                               Me.txtReturnTimeFrom.ppMinText & ":" & "00"
            End If
            If Me.txtReturnTimeTO.ppHourText Is Nothing Or Me.txtReturnTimeTO.ppHourText = "" Then
                dtReturnTo = Me.txtReturnDayTO.ppText & " 23:59:59"
            Else
                dtReturnTo = Me.txtReturnDayTO.ppText & " " & Me.txtReturnTimeTO.ppHourText & ":" & _
                             Me.txtReturnTimeTO.ppMinText & ":" & "00"
            End If

            '戻し日時（From）より戻し日時（To）が大きい場合
            If dtReturnFrom.CompareTo(dtReturnTo) > 0 Then
                Me.txtReturnDayFrom.psSet_ErrorNo("2001", Me.txtReturnDayFrom.ppName)
            End If
        End If
        '--------------------------------
        '2014/04/21 石橋　ここまで
        '--------------------------------

    End Sub

    'DLCLSTP001_010
    ''' <summary>
    ''' 自動リトライ有効無効
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msAutoRetrySW()

        Dim conDB As SqlConnection = Nothing               'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing                  'SqlCommandクラス
        Dim objDs As DataSet = Nothing                     'データセット
        Dim dstOrders As New DataSet
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
                cmdDB = New SqlCommand(M_DISP_ID & "_U2", conDB)

                'データ追加／更新
                Using conTrn = conDB.BeginTransaction
                    cmdDB.Transaction = conTrn
                    'コマンドタイプ設定(ストアド)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()
                    'コミット
                    conTrn.Commit()
                End Using

                'リトライボタン更新
                msGetRetryBtn()

                '更新が正常終了
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, ViewState("RetryBtn").ToString)

            Catch ex As SqlException
                '更新に失敗
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "自動リトライ")
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
            Catch ex As Exception
                'システムエラー
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "自動リトライ")
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
            Finally

                'データセットの破棄
                Call ClsCMDataConnect.psDisposeDataSet(dstOrders)
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Throw New Exception
        End If

    End Sub

    ''' <summary>
    ''' 自動リトライボタン表示データ取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function msGetRetryBtn() As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
        Dim errMsg As String = Nothing
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

                errMsg = "自動リトライ"

                '設定依頼内容の取得
                cmdDB = New SqlCommand(M_DISP_ID & "_S10", conDB)

                'リトライ条件
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(ClsCMDataConnect.pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))                        '結果取得用(0:データなし、1:データあり)
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '結果情報を取得
                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "0"         'データ無し

                        '取得失敗
                        Throw New Exception

                    Case Else        'データ有り

                        '検索項目の設定依頼内容
                        Me.Master.Master.ppLeftButton3.Text = dstOrders.Tables(0).Rows(0).Item("M29_NAME").ToString
                        Me.lblAutoRetrySts.Text = dstOrders.Tables(0).Rows(0).Item("M29_SHORT_NM").ToString
                        Me.lblUpdateDt.Text = dstOrders.Tables(0).Rows(0).Item("UPDATEDATE").ToString & ")"
                        ViewState("RetryBtn") = dstOrders.Tables(0).Rows(0).Item("M29_NAME").ToString
                        If dstOrders.Tables(0).Rows(0).Item("M29_CODE").ToString = "0" Then
                            Me.Master.Master.ppLeftButton3.BackColor = Drawing.Color.Pink
                            Me.lblAutoRetrySts.ForeColor = Drawing.Color.Black
                        Else
                            Me.Master.Master.ppLeftButton3.BackColor = Drawing.Color.Gray
                            Me.lblAutoRetrySts.ForeColor = Drawing.Color.Red
                        End If

                End Select

                Return True

            Catch ex As SqlException

                '検索失敗
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, errMsg)
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Return False

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, errMsg)
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Return False

            Finally

                'データセットの破棄
                Call ClsCMDataConnect.psDisposeDataSet(dstOrders)
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If

            End Try

        Else

            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            Master.ppCount = "0"
            Me.lblCount.Text = "0"
            Return False

        End If


    End Function
    'DLCLSTP001_010 END

#End Region

End Class
