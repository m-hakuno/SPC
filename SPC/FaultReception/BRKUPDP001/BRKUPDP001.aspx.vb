'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜故障受付＞
'*　処理名　　：　ミニ処理票
'*　ＰＧＭＩＤ：　BRKLSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.04.07　：　浜本
'*  更　新　　：　2014.06.17　：　間瀬　レイアウト変更
'*  更　新　　：　2015.02.04　：　加賀　Ｖｅｒ表示領域追加
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'BRKLSTP001-001     2015/08/19      加賀      ホールマスタ管理からの画面遷移に対応     
'BRKLSTP001-002     2015/08/25      加賀      画面遷移[ホールマスタ管理][対応履歴照会]追加              
'BRKLSTP001-003     2016/02/15      栗原      代行店の表示/非表示をマスタ取得に変更
'BRKLSTP001-004     2017/03/28      加賀      営業所の管理者から編集可能に変更             

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================

Imports System
Imports System.Globalization
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataLink
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive

#End Region

Public Class BRKUPDP001
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

    ''' <summary>
    ''' 一覧ボタン名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strBTN_SEL_NM As String = "btnSntaku"       '選択

    ''' <summary>
    ''' 印刷ボタンタイトル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strBTN_PRINT_TXT As String = "印刷"

    ''' <summary>
    ''' 登録ボタンタイトル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strBTN_INSERT_TXT As String = "登録"

    ''' <summary>
    ''' 更新ボタンタイトル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strBTN_UPDATE_TXT As String = "更新"

    ''' <summary>
    ''' 削除ボタンタイトル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strBTN_DELETE_TXT As String = "削除"

    ''' <summary>
    ''' クリアボタンタイトル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strBTN_CLEAR_TXT As String = "クリア"

    ''' <summary>
    ''' ホールマスタ管理ボタンタイトル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strBTN_HALLMST_TXT As String = "ホールマスタ管理"

    ''' <summary>
    ''' 対応履歴照会ボタンタイトル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strBTN_REFHST_TXT As String = "対応履歴照会"

    ''' <summary>
    ''' 管理番号種別(採番用)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const intKANRIBANGO_SBT As Integer = 3

    ''' <summary>
    ''' 画面ＩＤ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const M_DISP_ID As String = "BRKUPDP001"
    Const M_CHST_DISP_ID = P_FUN_BRK & P_SCR_INQ & P_PAGE & "001"   '対応履歴照会
    Const M_HALMST_DISP_ID = P_FUN_COM & P_SCR_MEN & P_PAGE & "006" 'ホールマスタ管理

    'ホールマスタ管理画面のパス
    Const M_HMST_DISP_PATH = "~/" & P_COM & "/" &
            P_FUN_COM & P_SCR_MEN & P_PAGE & "006" & "/" &
            P_FUN_COM & P_SCR_MEN & P_PAGE & "006.aspx"

    '対応履歴照会画面のパス
    Const M_CHST_DISP_PATH As String = "~/" & P_FLR & "/" &
            P_FUN_BRK & P_SCR_INQ & P_PAGE & "001" & "/" &
            P_FUN_BRK & P_SCR_INQ & P_PAGE & "001.aspx"

    '作業状況のステータスコード"08"
    Const M_WORK_STSCD = "08"

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

#Region "■ ページ初期処理"

    ''' <summary>
    ''' ページ初期処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '表設定
        pfSet_GridView(grvList, M_DISP_ID, 60, 9)
        pfSet_GridView(grvList2, M_DISP_ID + "_2", 60, 9)

    End Sub

#End Region

#Region "■ Page_Load"

    ''' <summary>
    ''' Page_Load処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        objStack = New StackFrame

        Try
            '開始ログ出力
            psLogStart(Me)

            'BRKLSTP001-004
            If Session(P_SESSION_ADMIN) = "2" Then
                ViewState(P_SESSION_AUTH) = "管理者"
            Else
                ViewState(P_SESSION_AUTH) = Session(P_SESSION_AUTH)
            End If
            'BRKLSTP001-004 END

            If IsPostBack Then
                'ボタン機能セット
                msSetAddHandler(CInt(ViewState(P_SESSION_TERMS)))
            End If

            Me.txaDealDtl.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txaDealDtl.MaxLength & """);")
            Me.txaDealDtl.Attributes.Add("onChange", "lenCheck(this,""" & Me.txaDealDtl.MaxLength & """);")

            txtHikitugiDtl.ppTextBox.ForeColor = Drawing.Color.FromArgb(255, 255, 0, 0)

            If Not IsPostBack Then  '初回表示

                'セッション項目取得
                ViewState(P_KEY) = Session(P_KEY)
                ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)
                ViewState(P_SESSION_USERID) = Session(P_SESSION_USERID)

                'プログラムＩＤ、画面名設定
                Master.ppProgramID = M_DISP_ID
                'Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)
                Master.ppTitle = "コール処理票"

                'パンくずリスト設定
                Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

                'ボタンの設定
                msSetButton()

                'ページ初期化
                msInitPage()

                If ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.登録 Then
                    'データ取得処理(新規登録でない場合のみ)
                    If Not msGet_Data() Then
                        Throw New Exception()
                    End If

                End If

                '排他情報用のグループ番号保管
                If Not Session(P_SESSION_GROUP_NUM) Is Nothing Then

                    ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

                End If
                '★排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
                If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then

                    Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)

                End If

                'Enable制御
                msEdit_Enabled(CBool(ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.参照))

                '初期フォーカス設定
                If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録 Then
                    'AutoPostback設定(Tbox入力検索のため)
                    txtTboxId.ppTextBox.AutoPostBack = True
                    Page.SetFocus(Me.txtTboxId.FindControl("txtTextBox"))
                    'BRKLSTP001-001
                    If Session(P_SESSION_OLDDISP) = M_HALMST_DISP_ID Then
                        Dim strKey() As String = ViewState(P_KEY)
                        'TBOXIDセット
                        txtTboxId.ppText = strKey(0)
                        'TBOX情報取得
                        mfSetTboxData(strKey(0))
                    End If
                    'BRKLSTP001-001 END
                ElseIf ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 Then
                    Page.SetFocus(Me.ddlCallCls.FindControl("ddlList"))
                End If

            End If

        Catch ex As Exception

            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

            '排他削除
            clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)

            psClose_Window(Me)
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub

#End Region

#Region "■ユーザー権限"
    '---------------------------
    '2014/04/18 武 ここから
    '---------------------------
    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        Select Case ViewState(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
                btnInsert.Enabled = False
                btnUpdate.Enabled = False
                btnDelete.Enabled = False
            Case "NGC"
        End Select

        Select Case ViewState(P_SESSION_TERMS)
            Case ClsComVer.E_遷移条件.登録
                Panel1.Enabled = True
                Panel2.Enabled = False
                Panel3.Enabled = False
                pnlRegister.Enabled = False
            Case ClsComVer.E_遷移条件.更新
                Panel1.Enabled = False
                Panel2.Enabled = True
                Panel3.Enabled = True
                pnlRegister.Enabled = True
            Case ClsComVer.E_遷移条件.参照
                Panel1.Enabled = False
                Panel2.Enabled = False
                Panel3.Enabled = True
                pnlRegister.Enabled = True
        End Select

        If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 Then
            Dim dt As DateTime = DateTime.Now
            If dttShinkokuDt.ppText = String.Empty Then
                dttShinkokuDt.ppText = dt.ToString("yyyy/MM/dd")
            End If
            'If txtUketukeDt.ppText = String.Empty AndAlso txtUketukeDt.ppHourText = String.Empty AndAlso txtUketukeDt.ppMinText = String.Empty Then
            '    txtUketukeDt.ppText = dt.ToString("yyyy/MM/dd")
            '    txtUketukeDt.ppHourText = dt.ToString("HH")
            '    txtUketukeDt.ppMinText = dt.ToString("mm")
            'End If
            If txtTaiouDt.ppText = String.Empty AndAlso txtTaiouDt.ppHourText = String.Empty AndAlso txtTaiouDt.ppMinText = String.Empty Then
                txtTaiouDt.ppText = dt.ToString("yyyy/MM/dd")
                txtTaiouDt.ppHourText = dt.ToString("HH")
                txtTaiouDt.ppMinText = dt.ToString("mm")
            End If
        End If

    End Sub
    '---------------------------
    '2014/04/18 武 ここまで
    '---------------------------
#End Region

#Region "■ ボタン押下処理"

    ''' <summary>
    ''' クリアボタン(メインデータ)クリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnGrandClear_Click(ByVal sender As Object, ByVal e As EventArgs)

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

            '再表示
            If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録 Then
                'オールクリア
                msInitKmkMain(1)
                msNotSelectDdl(1)
            Else
                'クリア(上部キー項目、及びラベル部は残す)
                msInitKmkMain(2)
                msNotSelectDdl(2)
            End If
        Catch ex As Exception
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
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
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub

    ''' <summary>
    ''' 登録・更新ボタン(メインデータ)クリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnGrandInsAndUpd_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim lng As Long = 0
        Dim dtrErrMes As DataRow

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

            '電話番号2のチェック
            If Not Me.txtShinkokuTel.ppText = String.Empty And _
                Not Long.TryParse(Me.txtShinkokuTel.ppText.Replace("-", ""), lng) Then
                Me.txtShinkokuTel.psSet_ErrorNo("4001", Me.txtShinkokuTel.ppName, "ハイフン(-)か数字")
                Return
            End If

            If ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.登録 Then
                If ddlStatusCd.SelectedIndex = 0 Then
                    dtrErrMes = ClsCMCommon.pfGet_ValMes("5003", "作業状況")
                    CustomValidator3.Text = "未入力エラー"
                    CustomValidator3.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
                    CustomValidator3.Enabled = True
                    CustomValidator3.IsValid = False
                    CustomValidator3.SetFocusOnError = True
                End If
            End If

            '入力チェック
            If Page.IsValid() = False Then
                Return
            End If

            ''その他チェック
            'If mfSelfChk() = False Then
            '    Return
            'End If

            If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録 Then

                'TBOXID存在チェック
                If mfChkExistsTboxId(txtTboxId.ppText) = False Then
                    txtTboxId.ppTextBox.Focus()

                    Return
                End If

                '登録
                msGrandDataInsert()
            Else
                '更新
                msGrandDataUpdate()
            End If

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
            Return
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub

    ''' <summary>
    ''' 削除ボタン(メインデータ)クリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnGrandDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
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

            '削除
            msGrandDataDelete()

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
            Return
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub

    ''' <summary>
    ''' 印刷ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPdf_Click(ByVal sender As Object, ByVal e As EventArgs)
        'DB接続
        Dim objCon As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '開始ログ出力
        psLogStart(Me)

        'DB接続
        If clsDataConnect.pfOpen_Database(objCon) Then
            Try
                'ストアド設定
                objCmd = New SqlCommand("BRKUPDP001_S5")
                objCmd.Connection = objCon
                objCmd.CommandType = CommandType.StoredProcedure

                With objCmd.Parameters
                    .Add(pfSet_Param("prmD77_MNG_NO", SqlDbType.NVarChar, lblKanriNo.Text))
                End With

                'SQL実行
                Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

                '取得エラー？
                If objDs Is Nothing OrElse objDs.Tables.Count <= 0 Then
                    Throw New Exception
                End If

                '印刷データ存在しない場合
                If objDs.Tables(0).Rows.Count <= 0 Then
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    Return
                End If

                '印刷
                psPrintPDF(Me, New REQREP002, objDs.Tables(0), "ミニ処理票入力リスト")

            Catch ex As Exception
                psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "システムの一覧")
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
                If Not clsDataConnect.pfClose_Database(objCon) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' クリアボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
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

            '項目クリア
            msInitKmkDtl()

            Page.SetFocus(Me.txtTaiouDt.FindControl("txtDateBox"))
            '---------------------------
            '2014/06/24 武 ここから
            '---------------------------
            '参照モードのとき、選択されている時以外はクリアボタンを押下できない
            If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照 Then
                Me.btnClear.Enabled = False
            End If
            '---------------------------
            '2014/06/24 武 ここまで
            '---------------------------
        Catch ex As Exception
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
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
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub

    ''' <summary>
    ''' 追加ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnInsert_Click(sender As Object, e As EventArgs) Handles btnInsert.Click
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

            '項目チェック
            If Page.IsValid() = False Then
                Return
            End If

            'MultiLineテキストのチェック
            If msSelfChkMsi() = False Then
                Return
            End If

            'データ選択中か(キーが取得されているか)をチェック
            If Not hdnMsiKey1.Value Is Nothing AndAlso Not hdnMsiKey2.Value Is Nothing AndAlso hdnMsiKey1.Value <> "" AndAlso hdnMsiKey2.Value <> "" Then
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
            Else
                '登録処理
                msMsiDataInsert()
            End If
            Page.SetFocus(Me.txtTaiouDt.FindControl("txtDateBox"))

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
            Return
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub

    ''' <summary>
    ''' 更新ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
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

            '入力チェック
            If Page.IsValid() = False Then
                Return
            End If

            'MultiLineテキストのチェック
            If msSelfChkMsi() = False Then
                Return
            End If

            'データ選択中か(キーが取得されているか)をチェック
            If Not hdnMsiKey1.Value Is Nothing AndAlso Not hdnMsiKey2.Value Is Nothing AndAlso hdnMsiKey1.Value <> "" AndAlso hdnMsiKey2.Value <> "" Then
                '更新
                msMsiDataUpdate()
            Else
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
            End If
            Page.SetFocus(Me.txtTaiouDt.FindControl("txtDateBox"))

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
            Return
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub

    ''' <summary>
    ''' 削除ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
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

            'データ選択中か(キーが取得されているか)をチェック
            If Not hdnMsiKey1.Value Is Nothing AndAlso Not hdnMsiKey2.Value Is Nothing AndAlso hdnMsiKey1.Value <> "" AndAlso hdnMsiKey2.Value <> "" Then
                msMsiDataDelete()
            Else
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
            End If
            Page.SetFocus(Me.txtTaiouDt.FindControl("txtDateBox"))

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
            Return
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub

    ''' <summary>
    ''' ホールマスタ管理/対応履歴照会ボタンクリック時処理 BRKLSTP001-002
    ''' </summary>
    Private Sub btnHallMstMng_Click(sender As Object, e As EventArgs)
        Try
            Dim strKeyList As List(Of String) = Nothing '画面引継ぎ用キー情報
            Dim strDispPath As String = String.Empty
            objStack = New StackFrame

            '開始ログ出力
            psLogStart(Me)

            '画面引継ぎ用キー情報設定
            strKeyList = New List(Of String)
            strKeyList.Add(Me.txtTboxId.ppText)                 'ＴＢＯＸＩＤ
            strKeyList.Add(hdnNlclsCd.Value)                    'NL
            strKeyList.Add(ViewState("SYSTEM_CLS").ToString)    'システム分類
            strKeyList.Add(hdnHallCd.Value)                     'ホールコード

            'セッション情報設定																	
            Session(P_SESSION_BCLIST) = Master.ppBcList_Text
            Session(P_SESSION_OLDDISP) = M_DISP_ID
            Session(P_KEY) = strKeyList.ToArray
            Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)
            Session(P_SESSION_TERMS) = ViewState(P_SESSION_TERMS)

            '遷移先設定
            Select Case DirectCast(sender, Button).ID
                Case Master.ppRigthButton7.ID
                    '対応履歴照会
                    strDispPath = M_CHST_DISP_PATH
                Case Master.ppRigthButton8.ID
                    'ホールマスタ管理
                    strDispPath = M_HMST_DISP_PATH
            End Select

            '--------------------------------
            '2014/04/16 星野　ここから
            '--------------------------------
            '■□■□結合試験時のみ使用予定□■□■
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
                            objStack.GetMethod.Name, strDispPath, strPrm, "TRANS")
            '■□■□結合試験時のみ使用予定□■□■
            '--------------------------------
            '2014/04/16 星野　ここまで
            '--------------------------------

            '画面起動
            psOpen_Window(Me, strDispPath)

            '終了ログ出力
            psLogEnd(Me)

        Catch ex As Exception
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
    End Sub

    ''' <summary>
    ''' 作業状況ドロップダウンリスト変更時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlStatusCd_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlStatusCd.SelectedIndexChanged
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        '選択ステータスコード＝"08"の場合は入力可能にする
        If ddlStatusCd.SelectedValue = M_WORK_STSCD Then
            'Me.txtSttsNotetext.ppEnabled = True
            'Page.SetFocus(Me.txtSttsNotetext.FindControl("txtTextBox"))

        Else
            'Me.txtSttsNotetext.ppText = String.Empty
            'Me.txtSttsNotetext.ppEnabled = False
            'Page.SetFocus(Me.txtHokokuDt.FindControl("txtDateBox"))
        End If

        If Me.ddlStatusCd.SelectedValue = "13" Then
            Master.ppRigthButton3.Enabled = True           '削除
        Else
            Master.ppRigthButton3.Enabled = False          '削除
        End If

        If ViewState(P_SESSION_AUTH) = "管理者" Then
            If ddlStatusCd.SelectedValue = "12" Then
                'DB接続
                If clsDataConnect.pfOpen_Database(objCn) Then
                    Try
                        objCmd = New SqlCommand("ZCMPSEL051", objCn)

                        With objCmd.Parameters
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, ViewState(P_SESSION_USERID)))
                        End With


                        'データ取得
                        objDs = clsDataConnect.pfGet_DataSet(objCmd)
                        'Me.txtKakuninNm.ppText = objDs.Tables(0).Rows(0).Item("ユーザー名").ToString
                    Catch ex As Exception
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    Finally
                        'DB切断
                        If Not clsDataConnect.pfClose_Database(objCn) Then
                        End If
                    End Try
                Else
                    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
                'Me.txtKakuninDt.ppDate = System.DateTime.Now
            Else
                'Me.txtKakuninNm.ppText = ""
                'Me.txtKakuninDt.ppText = ""
            End If
        End If


    End Sub

    ''' <summary>
    ''' 一覧の選択ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            '選択でないときはなにもしない
            If e.CommandName <> strBTN_SEL_NM Then
                Return
            End If

            '開始ログ出力
            psLogStart(Me)

            Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    ' ボタン押下行のIndex
            Dim rowData As GridViewRow = grvList.Rows(intIndex)             ' ボタン押下行


            Dim dtTaiouDt As DateTime = Nothing

            '日付をチェック
            If DateTime.TryParse(CType(rowData.FindControl("対応日時"), TextBox).Text, dtTaiouDt) Then
                txtTaiouDt.ppText = String.Format("{0:yyyy/MM/dd}", dtTaiouDt)         '対応日時
                txtTaiouDt.ppHourText = String.Format("{0:HH}", dtTaiouDt)
                txtTaiouDt.ppMinText = String.Format("{0:mm}", dtTaiouDt)
            Else
                txtTaiouDt.ppText = ""         '対応日時
                txtTaiouDt.ppHourText = ""
                txtTaiouDt.ppMinText = ""
            End If

            txtTaiouNm.ppText = CType(rowData.FindControl("対応担当"), TextBox).Text        '対応担当者
            txaDealDtl.Text = CType(rowData.FindControl("対応内容"), TextBox).Text          '対応内容
            hdnMsiKey1.Value = lblKanriNo.Text
            'hdnMsiKey1.Value = CType(rowData.FindControl("ミニ処理管理番号"), TextBox).Text
            hdnMsiKey2.Value = CType(rowData.FindControl("ミニ処理管理連番"), TextBox).Text.Replace(Environment.NewLine, "")

            '明細のボタン制御
            msChgDtlBtnMode(1)

            Page.SetFocus(Me.txtTaiouDt.FindControl("txtDateBox"))

        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ミニ処理情報明細データの選択処理")
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
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub

#End Region

#Region "■ TBOXフォーカスオフ処理"

    ''' <summary>
    ''' TBOXID入力時の動き
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ctl_Change(ByVal sender As Object, ByVal e As System.Object)
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

            '入力がない場合
            If txtTboxId.ppText.Trim() = "" Then
                'TBOX情報の初期化
                msInitKmkMainIns()
                msInitDropDownList()
                Return
            Else
                'TBOXデータの設定
                If mfSetTboxData(txtTboxId.ppText.Trim()) = False Then
                    Throw New Exception
                End If
            End If
            msSetDropDownList(ddlRptCd, "BRKUPDP001_S7", "M70_CONTENT", "M70_CODE", "申告内容")             '申告内容
            'msSetDropDownList(ddlDealDtl, "BRKUPDP001_S9", "M71_CONTENT", "M71_CODE", "処置内容")           '処置    

            Page.SetFocus(Me.ddlCallCls.FindControl("ddlList"))

        Catch ex As Exception
            'TBOX情報初期化
            msInitKmkMainIns()
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "TBOX情報の検索")
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
            '終了ログ出力
            psLogEnd(Me)
        End Try
    End Sub

#End Region

#End Region

#Region "そのほかのプロシージャ"

#Region "■ 初期化"

    ''' <summary>
    ''' ハンドラ追加
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetAddHandler(ByVal intMode As Integer)
        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnGrandClear_Click       'クリア
        AddHandler Master.ppRigthButton2.Click, AddressOf btnGrandInsAndUpd_Click   '更新
        AddHandler Master.ppRigthButton3.Click, AddressOf btnGrandDelete_Click      '削除
        AddHandler Master.ppRigthButton4.Click, AddressOf btnPdf_Click              '印刷
        'BRKLSTP001-002
        AddHandler Master.ppRigthButton7.Click, AddressOf btnHallMstMng_Click        'ホールマスタ管理
        AddHandler Master.ppRigthButton8.Click, AddressOf btnHallMstMng_Click        '対応履歴照会
        'BRKLSTP001-002 END
        AddHandler btnAdd.Click, AddressOf btnGrandInsAndUpd_Click                  '登録

        'TBOXIDを入力した場合
        AddHandler Me.txtTboxId.ppTextBox.TextChanged, AddressOf ctl_Change
        Me.txtTboxId.ppTextBox.AutoPostBack = True

        '登録ボタン
        Me.btnAdd.OnClientClick = pfGet_OCClickMes("00008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "表示中のミニ処理票データ")

        If intMode = ClsComVer.E_遷移条件.登録 Then
            '登録ボタン
            Master.ppRigthButton2.OnClientClick =
                pfGet_OCClickMes("00008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "表示中のミニ処理票データ")
        Else
            '更新ボタン
            Master.ppRigthButton2.OnClientClick =
                pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "表示中のミニ処理票データ")
        End If

        '削除ボタン
        Master.ppRigthButton3.OnClientClick =
            pfGet_OCClickMes("00010", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "表示中のミニ処理票データ")

        '印刷ボタン
        Master.ppRigthButton4.OnClientClick =
            pfGet_OCClickMes("10002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "ミニ処理票入力リスト")

        '削除ボタン(明細)
        btnDelete.OnClientClick =
            pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "選択中のミニ処理票明細データ")

        '追加ボタン(明細)
        btnInsert.OnClientClick =
            pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "ミニ処理票明細データ")

        '更新ボタン(明細)
        btnUpdate.OnClientClick =
            pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "選択中のミニ処理票明細データ")

    End Sub

    ''' <summary>
    ''' ボタン制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetButton()

        'ボタン機能セット
        msSetAddHandler(CInt(ViewState(P_SESSION_TERMS)))

        'validationグループの設定
        Master.ppRigthButton1.ValidationGroup = "Detail"
        Master.ppRigthButton2.ValidationGroup = "Detail"
        Master.ppRigthButton3.ValidationGroup = "Detail"
        Master.ppRigthButton4.ValidationGroup = "Detail"

        'ボタン押下時の検証を無効
        Master.ppRigthButton1.CausesValidation = False     'クリア
        Master.ppRigthButton2.CausesValidation = True      '更新
        Master.ppRigthButton3.CausesValidation = False     '削除
        Master.ppRigthButton4.CausesValidation = False     '印刷
        'BRKLSTP001-002
        Master.ppRigthButton7.CausesValidation = False      '対応履歴照会
        Master.ppRigthButton8.CausesValidation = False      'ホールマスタ管理
        'BRKLSTP001-002 END
        Master.ppLeftButton1.CausesValidation = False
        Master.ppLeftButton2.CausesValidation = False

        btnClear.CausesValidation = True
        btnDelete.CausesValidation = False
        btnInsert.CausesValidation = True
        btnUpdate.CausesValidation = True

        Master.ppRigthButton3.BackColor = Drawing.Color.FromArgb(255, 102, 102)          '削除
        btnDelete.BackColor = Drawing.Color.FromArgb(255, 102, 102)

        Master.ppRigthButton1.Visible = True     'クリア
        Master.ppRigthButton2.Visible = True     '更新
        Master.ppRigthButton3.Visible = True     '削除
        Master.ppRigthButton4.Visible = True     '印刷
        'BRKLSTP001-002
        Master.ppRigthButton7.Visible = True      '対応履歴照会
        Master.ppRigthButton8.Visible = True      'ホールマスタ管理
        'BRKLSTP001-002 END
        Master.ppLeftButton1.Visible = True
        Master.ppLeftButton2.Visible = True

        'ボタン名称設定
        Master.ppRigthButton1.Text = strBTN_CLEAR_TXT      'クリア
        Master.ppRigthButton2.Text = strBTN_UPDATE_TXT     '更新
        Master.ppRigthButton3.Text = strBTN_DELETE_TXT     '削除
        Master.ppRigthButton4.Text = strBTN_PRINT_TXT      '印刷
        'BRKLSTP001-002 END
        Master.ppRigthButton7.Text = strBTN_REFHST_TXT     '対応履歴照会
        Master.ppRigthButton8.Text = strBTN_HALLMST_TXT    'ホールマスタ管理
        'BRKLSTP001-002 END
        Master.ppLeftButton1.Text = "ﾒｰﾙ雛形"
        Master.ppLeftButton2.Text = "ﾒｰﾙ雛形"

    End Sub

    ''' <summary>
    ''' フォーム初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msInitPage()
        'メイン項目のクリア
        msInitKmkMain(0)

        'リスト登録用項目クリア
        msInitKmkDtl()

        'ドロップダウンリスト設定
        msInitDropDownList()

        '一覧初期化
        grvList.DataSource = New DataTable()
        grvList.DataBind()

        '一覧初期化
        grvList2.DataSource = New DataTable()
        grvList2.DataBind()
    End Sub

    ''' <summary>
    ''' メイン部クリア
    ''' </summary>
    ''' <remarks>0:ロード 1:登録モードクリアボタン時 2:更新モードクリアボタン時</remarks>
    Private Sub msInitKmkMain(ByVal intMode As Integer)
        If intMode = 0 OrElse intMode = 1 Then
            '登録時入力項目の初期化
            msInitKmkMainIns()
        End If

        '残りの入力項目の初期化
        msInitKmkMainUpd()
    End Sub

    ''' <summary>
    ''' 登録時にクリアする項目のクリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msInitKmkMainIns()
        lblKanriNo.Text = "" '管理番号

        'TBOX情報
        msInitTBoxData()

        Me.txtTboxId.ppText = ""
        Me.LblTboxType.Text = ""
        '--------------------------------
        '2015/02/04 加賀　ここから
        '--------------------------------
        Me.LblTboxVer.Text = ""
        '--------------------------------
        '2015/02/04 加賀　ここまで
        '--------------------------------

    End Sub

    ''' <summary>
    ''' TBOX情報の初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msInitTBoxData()
        'txtTboxId.ppText = "" 'TBOXID
        lblNlcls.Text = "" 'ＮＬ区分
        hdnNlclsCd.Value = "" 'ＮＬ区分ＣＤ
        lblEwcls.Text = "" 'ＥＷ区分
        hdnEwclsCd.Value = "" 'ＥＷ区分ＣＤ
        hdnHallCd.Value = "" 'ホールＣＤ
        lblHallNm.Text = "" 'ホール名
        lblHallAddr.Text = "" 'ホール住所
        lblTel.Text = "" 'ホールTEL
        'lblTokatuNm.Text = "" '統括名
        'hdnTokatuCd.Value = "" '統括名ＣＤ
        lblEigyosyoNm.Text = "" '営業所名
        hdnVersion.Value = "" 'Ｖｅｒ
        hdnTboxType.Value = "" 'ＴＢＯＸタイプ
        lblTwin.Text = ""
        hdnTwinCd.Value = ""
        lblAgc.Text = ""
        hdnAgcCd.Value = ""
        hdnAgcZip.Value = ""
        hdnAgcAddr.Value = ""
        hdnAgcTel.Value = ""
        hdnAgcFax.Value = ""
        'lblRep.Text = ""
        'hdnRepCd.Value = ""
        'hdnRepZip.Value = ""
        'hdnRepAddr.Value = ""
        'hdnRepTel.Value = ""
        'hdnRepChg.Value = ""
        lblNgcOrg.Text = ""
        'lblOrgTel.Text = ""
        'lblEst.Text = ""
        'hdnEstCls.Value = ""
        'lblMdn.Text = ""
        'hdnMdnCnt.Value = ""
        'hdnMdnCd1.Value = ""
        'hdnMdnCd2.Value = ""
        'hdnMdnCd3.Value = ""
    End Sub

    ''' <summary>
    ''' 更新時にクリアする項目のクリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msInitKmkMainUpd()
        'dttShinkokuDt.ppText = "" '申告日
        txtShinkokuNm.ppText = "" '申告者
        txtShinkokuTel.ppText = "" '申告者TEL
        'txtUketukeDt.ppText = "" '受付日時
        'txtUketukeDt.ppHourText = "" '受付日時(時間)
        'txtUketukeDt.ppMinText = "" '受付日時(時間)
        txtUketukeNm.ppText = "" '受付者
        txtShinkokuDtl1.ppText = "" '申告内容１
        'txtShinkokuDtl2.ppText = "" '申告内容２
        txtHikitugiDtl.ppText = "" '引継ぎ内容
        'txtHokokuDt.ppText = "" '報告日時
        'txtHokokuDt.ppHourText = "" '報告日時(時間)
        'txtHokokuDt.ppMinText = "" '報告日時(時間)
        'txtSttsNotetext.ppText = "" '作業状況備考
        'txtSttsNotetext.ppEnabled = False
        'txtSyotiDtl.ppText = "" '処置内容
        'txtKakuninDt.ppText = "" '確認日
        'txtKakuninNm.ppText = "" '確認者
        'txtTroubleNo.ppText = "" 'トラブル管理番号
    End Sub

    ''' <summary>
    ''' リスト登録用項目クリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msInitKmkDtl()
        txtTaiouDt.ppText = "" '対応日時
        txtTaiouDt.ppHourText = "" '対応日時(時間)
        txtTaiouDt.ppMinText = "" '対応日時(時間)
        txtTaiouNm.ppText = "" '対応担当
        txaDealDtl.Text = "" '対応内容
        hdnMsiKey1.Value = "" 'ミニ処理管理番号
        hdnMsiKey2.Value = "" 'ミニ処理管理連番

        '明細のボタン制御
        msChgDtlBtnMode(0)

    End Sub

    ''' <summary>
    ''' 選択・クリアボタンによるボタンの変更
    ''' </summary>
    ''' <param name="intMode">0：明細新規　1：明細更新</param>
    ''' <remarks></remarks>
    Private Sub msChgDtlBtnMode(ByVal intMode As Integer)
        '--------------------------------
        '2014/06/24 武　ここから
        '--------------------------------
        If ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.参照 Then
            '--------------------------------
            '2014/06/24 武　ここまで
            '--------------------------------
            btnClear.Enabled = True
            btnDelete.Enabled = CBool(intMode <> 0)
            btnUpdate.Enabled = CBool(intMode <> 0)
            btnInsert.Enabled = CBool(intMode = 0)
            '--------------------------------
            '2014/06/24 武　ここから
            '--------------------------------
        Else
            btnClear.Enabled = True
            btnDelete.Enabled = False
            btnUpdate.Enabled = False
            btnInsert.Enabled = False
        End If
        '--------------------------------
        '2014/06/24 武　ここまで
        '--------------------------------
    End Sub

    ''' <summary>
    ''' 未選択コンボボックス
    ''' </summary>
    ''' <remarks>0:ロード時 1:登録モードクリアボタン押下時 2:更新モードクリアボタン押下時</remarks>
    Private Sub msNotSelectDdl(ByVal intMode As Integer)
        '各ドロップダウンリストを未選択に
        If intMode = 0 OrElse intMode = 1 Then
            ddlCallCls.ppDropDownList.SelectedIndex = 0
            'ddlBlngCls.ppDropDownList.SelectedIndex = 0
            'ddlAppaCls.ppDropDownList.SelectedIndex = 0
        End If

        'ddlRptBase.SelectedIndex = 0
        ddlRptCd.SelectedIndex = 0
        ddlStatusCd.SelectedIndex = 0
        'ddlDealDtl.SelectedIndex = 0

    End Sub

#End Region

#Region "■ ドロップダウンリスト設定"

    ''' <summary>
    ''' ドロップダウンリスト設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msInitDropDownList()
        'データ設定
        'msSetDropDownList(ddlRptBase, "BRKUPDP001_S6", "M69_NAME", "M69_CODE", "申告者")                '申告者
        msSetDropDownList(ddlStatusCd, "BRKUPDP001_S8", "M27_STATUS_NM", "M27_STATUS_CD", "作業状況")   '作業状況
        msSetDropDownList(ddlRptCd, "BRKUPDP001_S7", "M70_CONTENT", "M70_CODE", "申告内容")             '申告内容
        'msSetDropDownList(ddlDealDtl, "BRKUPDP001_S9", "M71_CONTENT", "M71_CODE", "処置内容")           '処置    

        '各ドロップダウンリストを未選択に
        msNotSelectDdl(0)

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定
    ''' </summary>
    ''' <param name="ddlCtrl">ドロップダウンリスト</param>
    ''' <param name="strStoredNm">ストアド名称</param>
    ''' <param name="strDataTxtKmkNm">DB表示項目名</param>
    ''' <param name="strValKmkNm">DB値項目名</param>
    ''' <param name="strMstNm">マスタ名称(エラーメッセージ用)</param>
    ''' <remarks></remarks>
    Private Sub msSetDropDownList(ByRef ddlCtrl As DropDownList, ByVal strStoredNm As String, ByVal strDataTxtKmkNm As String, ByVal strValKmkNm As String, ByVal strMstNm As String)

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'DB接続
        If clsDataConnect.pfOpen_Database(objCn) Then
            Try
                objCmd = New SqlCommand(strStoredNm, objCn)

                If strStoredNm = "BRKUPDP001_S7" Or strStoredNm = "BRKUPDP001_S9" Then
                    With objCmd.Parameters
                        .Add(pfSet_Param("sys_code", SqlDbType.NVarChar, hdnTboxType.Value))
                    End With
                End If


                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                ddlCtrl.Items.Clear()
                ddlCtrl.DataSource = objDs.Tables(0)
                ddlCtrl.DataTextField = strDataTxtKmkNm
                ddlCtrl.DataValueField = strValKmkNm
                ddlCtrl.DataBind()
                ddlCtrl.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加
                ddlCtrl.SelectedIndex = 0

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMstNm & "マスタ一覧取得")
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
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Sub

#End Region

#Region "■ 項目活性化制御"

    ''' <summary>
    ''' 項目活性化・非活性化処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msEdit_Enabled(ByVal blNotSansyo As Boolean)
        If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録 Then
            Master.ppRigthButton4.Enabled = False '印刷
        Else
            txtTboxId.ppEnabled = False
            Master.ppRigthButton4.Enabled = True '印刷
        End If

        ddlCallCls.ppEnabled = blNotSansyo 'コール区分
        'ddlBlngCls.ppEnabled = blNotSansyo '所属区分
        'ddlAppaCls.ppEnabled = blNotSansyo '機種区分

        dttShinkokuDt.ppEnabled = blNotSansyo '申告日
        'ddlRptBase.Enabled = blNotSansyo '申告元
        txtShinkokuNm.ppEnabled = blNotSansyo '申告者
        txtShinkokuTel.ppEnabled = blNotSansyo '申告者ＴＥＬ
        'txtUketukeDt.ppEnabled = blNotSansyo '受付日時
        'txtUketukeDt.ppEnabled = blNotSansyo '受付日時(時間)
        txtUketukeNm.ppEnabled = blNotSansyo '受付者
        ddlRptCd.Enabled = blNotSansyo '申告内容
        txtShinkokuDtl1.ppEnabled = blNotSansyo '申告内容１
        'txtShinkokuDtl2.ppEnabled = blNotSansyo '申告内容２
        txtHikitugiDtl.ppEnabled = blNotSansyo '引継内容
        ddlStatusCd.Enabled = blNotSansyo '作業状況
        If ddlStatusCd.SelectedValue = M_WORK_STSCD Then
            'txtSttsNotetext.ppEnabled = blNotSansyo '作業状況備考
        Else
            'txtSttsNotetext.ppEnabled = False '作業状況備考
        End If
        'txtHokokuDt.ppEnabled = blNotSansyo 'ＮＧＣ報告日時
        'txtHokokuDt.ppEnabled = blNotSansyo 'ＮＧＣ報告日時(時間)
        'ddlDealDtl.Enabled = blNotSansyo '処置内容
        'txtSyotiDtl.ppEnabled = blNotSansyo '処置内容(txt)
        'txtKakuninDt.ppEnabled = blNotSansyo '確認日
        'txtKakuninNm.ppEnabled = blNotSansyo '確認者
        'txtTroubleNo.ppEnabled = blNotSansyo 'トラブル管理番号
        txtTaiouDt.ppEnabled = blNotSansyo '対応日時
        txtTaiouNm.ppEnabled = blNotSansyo '対応担当
        txaDealDtl.Enabled = blNotSansyo '対応内容

        If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録 Then
            Master.ppRigthButton2.Enabled = False     '更新
            'BRKLSTP001-002
            Master.ppRigthButton7.Enabled = False    '対応履歴照会
            Master.ppRigthButton8.Enabled = False    'ホールマスタ管理
            'BRKLSTP001-002 END
        Else
            Master.ppRigthButton2.Enabled = blNotSansyo     '更新
            'BRKLSTP001-002
            Master.ppRigthButton7.Enabled = True    '対応履歴照会
            Master.ppRigthButton8.Enabled = True    'ホールマスタ管理
            'BRKLSTP001-002 END
        End If

        If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録 Then
            Master.ppRigthButton1.Enabled = False     'クリア
        Else
            Master.ppRigthButton1.Enabled = blNotSansyo     'クリア
        End If

        If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 Then

            '明細のボタン制御
            msChgDtlBtnMode(0)

            If Me.ddlStatusCd.SelectedValue = "13" Then
                Master.ppRigthButton3.Enabled = True           '削除
            Else
                Master.ppRigthButton3.Enabled = False          '削除
            End If
        Else
            btnClear.Enabled = False  'リスト用クリア
            btnInsert.Enabled = False 'リスト用追加
            btnUpdate.Enabled = False 'リスト用更新
            btnDelete.Enabled = False 'リスト用削除
            Master.ppRigthButton3.Enabled = False     '削除
        End If
        txtTboxId.ppEnabled = blNotSansyo
        '--------------------------------
        '2014/06/24 武　ここから
        '--------------------------------
        'grvList.Enabled = blNotSansyo '一覧
        '--------------------------------
        '2014/06/24 武　ここまで
        '--------------------------------
    End Sub

#End Region

#Region "■ データ読み込み"

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msGet_Data() As Boolean

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand

        Dim strKey() As String = Nothing
        Dim strVal As String = lblKanriNo.Text.Trim()
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'ＤＢ接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                'ビューステート項目取得
                strKey = ViewState(P_KEY)

                '画面ページ表示初期化
                Me.grvList.DataSource = New DataTable()

                '***** メインデータ取得 *****

                'ストアド設定
                cmdDB = New SqlCommand("BRKUPDP001_S1", conDB)
                cmdDB.CommandType = CommandType.StoredProcedure

                'パラメータ設定
                With cmdDB.Parameters
                    If strVal = "" Then
                        .Add(pfSet_Param("prmD77_MNG_NO", SqlDbType.NVarChar, strKey(0))) 'ミニ管理番号
                    Else
                        .Add(pfSet_Param("prmD77_MNG_NO", SqlDbType.NVarChar, strVal)) 'ミニ管理番号
                    End If
                End With

                Dim dstOrders_1 As DataSet = Nothing
                dstOrders_1 = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得エラー
                If dstOrders_1 Is Nothing OrElse dstOrders_1.Tables.Count < 0 Then
                    Throw New Exception()
                End If

                'データ設定
                If msSetData(dstOrders_1.Tables(0)) = False Then
                    Throw New Exception()
                End If

                '***** 明細データ取得 *****

                'ストアド設定
                cmdDB = New SqlCommand("BRKUPDP001_S2", conDB)
                cmdDB.CommandType = CommandType.StoredProcedure

                'パラメータ設定
                With cmdDB.Parameters
                    If strVal = "" Then
                        .Add(pfSet_Param("prmD78_MNG_NO", SqlDbType.NVarChar, strKey(0))) 'ミニ管理番号
                    Else
                        .Add(pfSet_Param("prmD78_MNG_NO", SqlDbType.NVarChar, strVal)) 'ミニ管理番号
                    End If
                End With

                Dim dstOrders_2 As DataSet = Nothing
                dstOrders_2 = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得エラー
                If dstOrders_2 Is Nothing OrElse dstOrders_2.Tables.Count < 0 Then
                    Throw New Exception()
                End If

                '一覧設定
                grvList.DataSource = dstOrders_2.Tables(0)
                Me.grvList.DataBind()

                Return True

            Catch ex As Exception
                '--------------------------------
                '2014/04/15 浜本　ここから
                '--------------------------------
                'psMesBox(Me, "00004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "ミニ処理票データ")
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "ミニ処理票データ")
                '--------------------------------
                '2014/04/15 浜本　ここから
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
                Me.grvList.DataBind()

                Return False
            Finally
                'DB切断
                If Not conDB Is Nothing Then
                    If Not clsDataConnect.pfClose_Database(conDB) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Return False
        End If

    End Function

    ''' <summary>
    ''' TBOX情報取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfSetTboxData(ByVal strTboxId As String) As Boolean
        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                '値が空白の場合はエラー
                If strTboxId = "" Then
                    Throw New Exception()
                End If

                'ストアド設定
                cmdDB = New SqlCommand("BRKUPDP001_S3")
                cmdDB.Connection = conDB
                cmdDB.CommandType = CommandType.StoredProcedure

                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("@prmT03_TBOXID", SqlDbType.NVarChar, strTboxId))              'ミニ管理番号
                End With

                'SQL実行
                Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得エラー？
                If objDs Is Nothing OrElse objDs.Tables.Count <= 0 Then
                    Throw New Exception()
                End If

                'データが存在しない
                If objDs.Tables(0).Rows.Count <= 0 Then

                    'TBOX情報の初期化
                    msInitTBoxData()
                    Me.LblTboxType.Text = ""
                    Me.LblTboxVer.Text = ""
                    Return True
                End If

                'TBOX情報設定
                msSetTboxInfo(objDs.Tables(0).Rows(0))

                Return True

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
    ''' 再読み込み
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msReLoad(ByVal strKanriNo As String, ByVal intMode As Integer)
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            'キー値なしはエラー
            If strKanriNo Is Nothing OrElse strKanriNo.Trim = "" Then
                Throw New Exception()
            End If

            '--------------------------------
            '2014/04/15 浜本　ここから
            '--------------------------------
            '更新・参照モードでは、ボタン名を「登録」から「更新」に変更
            Master.ppRigthButton2.Text = strBTN_UPDATE_TXT       '更新
            '--------------------------------
            '2014/04/15 浜本　ここまで
            '--------------------------------

            'キー値設定
            Session(P_KEY) = {strKanriNo}
            ViewState(P_KEY) = {strKanriNo}

            'モード設定
            Session(P_SESSION_TERMS) = intMode
            ViewState(P_SESSION_TERMS) = intMode

            '再読み込み
            msGet_Data()

            'ENABLE制御を更新用に変更
            msEdit_Enabled(CBool(ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.参照))

            'コール区分にフォーカス設定
            Page.SetFocus(Me.ddlCallCls.FindControl("ddlList"))

        Catch ex As Exception
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
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

    End Sub

#End Region

#Region "■ 画面項目セット"

    ''' <summary>
    ''' データ設定
    ''' </summary>
    ''' <param name="objTbl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function msSetData(ByVal objTbl As DataTable) As Boolean

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            'データがない場合はエラー
            If objTbl Is Nothing OrElse objTbl.Rows.Count = 0 Then
                Return False
            End If

            Dim objRow As DataRow = objTbl.Rows(0)

            '各項目に値を設定
            lblKanriNo.Text = objRow("管理番号")

            'TBOX関連設定
            msSetTboxInfo(objRow)

            msSetDropDownList(ddlRptCd, "BRKUPDP001_S7", "M70_CONTENT", "M70_CODE", "申告内容")             '申告内容
            'msSetDropDownList(ddlDealDtl, "BRKUPDP001_S9", "M71_CONTENT", "M71_CODE", "処置内容")           '処置        

            ddlCallCls.ppSelectedValue = objRow("コール区分")
            'ddlBlngCls.ppSelectedValue = objRow("所属区分")
            'ddlAppaCls.ppSelectedValue = objRow("機種区分")
            dttShinkokuDt.ppText = objRow("申告日")
            'ddlRptBase.SelectedValue = objRow("申告元")
            txtShinkokuNm.ppText = objRow("申告者")
            txtShinkokuTel.ppText = objRow("申告者ＴＥＬ")
            'txtUketukeDt.ppText = objRow("受付日時")


            If objRow("受付日時_時間") Is DBNull.Value Then
                'txtUketukeDt.ppHourText = ""
                'txtUketukeDt.ppMinText = ""
            Else
                'txtUketukeDt.ppHourText = String.Format("{0:HH}", CDate(objRow("受付日時_時間")))
                'txtUketukeDt.ppMinText = String.Format("{0:mm}", CDate(objRow("受付日時_時間")))
            End If

            txtUketukeNm.ppText = objRow("受付者")
            ddlRptCd.SelectedValue = objRow("申告内容")
            txtShinkokuDtl1.ppText = objRow("申告内容フリー１")
            'txtShinkokuDtl2.ppText = objRow("申告内容フリー２")
            txtHikitugiDtl.ppText = objRow("引継ぎ内容")
            ddlStatusCd.SelectedValue = objRow("作業状況")
            If ViewState(P_SESSION_AUTH) <> "管理者" Then
                If Me.ddlStatusCd.SelectedValue = "12" Or Me.ddlStatusCd.SelectedValue = "13" Or Me.ddlStatusCd.SelectedValue = "22" Then
                    ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
                    '非活性化処理
                    msEdit_Enabled(False)
                Else
                    For intState As Integer = 0 To ddlStatusCd.Items.Count - 1
                        If Me.ddlStatusCd.Items(intState).Value = "12" Then
                            Me.ddlStatusCd.Items.RemoveAt(intState)
                            Exit For
                        End If
                    Next
                    For intState As Integer = 0 To ddlStatusCd.Items.Count - 1
                        If Me.ddlStatusCd.Items(intState).Value = "13" Then
                            Me.ddlStatusCd.Items.RemoveAt(intState)
                            Exit For
                        End If
                    Next
                    For intState As Integer = 0 To ddlStatusCd.Items.Count - 1
                        If Me.ddlStatusCd.Items(intState).Value = "22" Then
                            Me.ddlStatusCd.Items.RemoveAt(intState)
                            Exit For
                        End If
                    Next
                End If
            End If

            If objRow("作業状況").ToString = M_WORK_STSCD Then
                'Me.txtSttsNotetext.ppText = objRow("作業状況備考").ToString
                'Me.txtSttsNotetext.ppEnabled = True
            Else
                'Me.txtSttsNotetext.ppText = String.Empty
                'Me.txtSttsNotetext.ppEnabled = False
            End If
            'txtHokokuDt.ppText = objRow("ＮＧＣ報告日時")

            If objRow("ＮＧＣ報告日時_時間") Is DBNull.Value Then
                'txtHokokuDt.ppHourText = ""
                'txtHokokuDt.ppMinText = ""
            Else
                'txtHokokuDt.ppHourText = String.Format("{0:HH}", CDate(objRow("ＮＧＣ報告日時_時間")))
                'txtHokokuDt.ppMinText = String.Format("{0:mm}", CDate(objRow("ＮＧＣ報告日時_時間")))
            End If

            'ddlDealDtl.SelectedValue = objRow("処置内容")
            'txtSyotiDtl.ppText = objRow("処置内容フリー")
            'txtKakuninDt.ppText = objRow("確認日")
            'txtKakuninNm.ppText = objRow("確認者")
            'txtTroubleNo.ppText = objRow("トラブル管理番号")

            'キャンセル区分がたっている時は参照モードで表示
            If CStr(objRow("キャンセル区分")) = "1" Then
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
                ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
            End If

            Return True

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
            Return False
        End Try
    End Function

    ''' <summary>
    ''' TBOX情報設定
    ''' </summary>
    ''' <param name="objRow"></param>
    ''' <remarks></remarks>
    Private Sub msSetTboxInfo(ByVal objRow As DataRow)
        txtTboxId.ppText = objRow("ＴＢＯＸＩＤ")
        If Me.txtTboxId.ppText = "99999999" Or Me.txtTboxId.ppText = "99999989" Then
            Me.LblTboxType.Text = ""               'ＴＢＯＸタイプ
            Me.lblNlcls.Text = ""                                                                 'ＮＬ区分
        Else
            Me.LblTboxType.Text = objRow("ＴＢＯＸタイプ名")               'ＴＢＯＸタイプ
            Me.lblNlcls.Text = objRow("ＮＬ区分")                              'ＮＬ区分
        End If

        'lblNlcls.Text = objRow("ＮＬ区分")
        lblEwcls.Text = objRow("ＥＷ区分")
        hdnHallCd.Value = objRow("ホールＣＤ")
        lblHallNm.Text = objRow("ホール名")
        lblHallAddr.Text = objRow("ホール住所")
        lblTel.Text = objRow("ホールＴＥＬ")
        'lblTokatuNm.Text = objRow("統括名")
        lblEigyosyoNm.Text = objRow("営業所名")
        ViewState("SYSTEM_CLS") = objRow("システムクラス") 'BRKLSTP001-002
        hdnNlclsCd.Value = objRow("ＮＬ区分ＣＤ") 'ＮＬ区分ＣＤ
        hdnEwclsCd.Value = objRow("ＥＷ区分ＣＤ") 'ＥＷ区分ＣＤ
        'hdnTokatuCd.Value = objRow("統括名ＣＤ") '統括名ＣＤ
        hdnEigyosyoCd.Value = objRow("営業所名ＣＤ") '営業所ＣＤ
        hdnVersion.Value = objRow("Ｖｅｒ") 'バージョン
        LblTboxVer.Text = objRow("Ｖｅｒ") 'バージョン
        hdnTboxType.Value = objRow("ＴＢＯＸタイプ") 'ＴＢＯＸタイプ
        'LblTboxType.Text = objRow("ＴＢＯＸタイプ名")
        If objRow("双子店区分") = "0" Then
            lblTwin.Text = "単独店"
            hdnTwinCd.Value = "0"
        ElseIf objRow("双子店区分") = "1" Then
            lblTwin.Text = "双子店"
            hdnTwinCd.Value = "1"
        Else
            lblTwin.Text = ""
            hdnTwinCd.Value = ""
        End If
        lblAgc.Text = objRow("代理店名")
        hdnAgcCd.Value = objRow("代理店ＣＤ")
        hdnAgcZip.Value = objRow("代理店郵便番号")
        hdnAgcAddr.Value = objRow("代理店住所")
        hdnAgcTel.Value = objRow("代理店ＴＥＬ")
        hdnAgcFax.Value = objRow("代理店ＦＡＸ")

        'BRKLSTP001-003 
        'lblRep.Text = objRow("代行店名")
        'hdnRepCd.Value = objRow("代行店ＣＤ")
        'hdnRepZip.Value = objRow("代行店郵便番号")
        'hdnRepAddr.Value = objRow("代行店住所")
        'hdnRepTel.Value = objRow("代行店ＴＥＬ")
        'hdnRepChg.Value = objRow("代行店担当者")
        If objRow.Table.Columns.Contains("代行店表示フラグ") AndAlso objRow("代行店表示フラグ").ToString = "1" Then
            'lblRep.Visible = False
        Else
            'lblRep.Visible = True
        End If
        'BRKLSTP001-003 END

        lblNgcOrg.Text = objRow("ＮＧＣ担当営業部")
        'lblOrgTel.Text = objRow("担当営業部ＴＥＬ")
        'lblEst.Text = objRow("ＭＤＮ設置有無")
        'hdnEstCls.Value = objRow("ＭＤＮ設置有無ＣＤ")
        'lblMdn.Text = objRow("ＭＤＮ機器名")
        'hdnMdnCnt.Value = objRow("ＭＤＮ台数")
        'hdnMdnCd1.Value = objRow("ＭＤＮＣＤ１")
        'hdnMdnCd2.Value = objRow("ＭＤＮＣＤ２")
        'hdnMdnCd3.Value = objRow("ＭＤＮＣＤ３")

    End Sub

#End Region


#Region "データバインド時処理"

    ''' <summary>
    ''' データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound, grvList2.DataBound

        '色を変える
        For Each rowData As GridViewRow In grvList.Rows
            If (rowData.RowIndex Mod 2) = 1 Then
                CType(rowData.FindControl("対応日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("対応担当"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("対応内容"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
            End If

        Next

        '色を変える
        For Each rowData As GridViewRow In grvList2.Rows
            If (rowData.RowIndex Mod 2) = 1 Then
                CType(rowData.FindControl("製品名"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("設置台数"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("予備機台数"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("合計台数"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
            End If

        Next
    End Sub
#End Region
#Region "■ 入力チェック"
    '--------------------------------
    '2014/04/14 Hamamoto　ここから
    '--------------------------------

    ' ''' <summary>
    ' ''' メイン登録項目チェック
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function mfSelfChk() As Boolean
    '    '--------------------------------
    '    '2014/04/14 星野　ここから
    '    '--------------------------------
    '    objStack = New StackFrame
    '    '--------------------------------
    '    '2014/04/14 星野　ここまで
    '    '--------------------------------
    '    Try
    '        Dim intErrFlg As Integer = 0

    '        'コール区分(必須)
    '        If ddlCallCls.ppDropDownList.SelectedIndex = 0 Then
    '            ddlCallCls.psSet_ErrorNo("5003", ddlCallCls.ppName)

    '            If intErrFlg = 0 Then
    '                ddlCallCls.ppDropDownList.Focus()
    '            End If

    '            intErrFlg = 1

    '        End If

    '        '所属区分(必須)
    '        If ddlBlngCls.ppDropDownList.SelectedIndex = 0 Then
    '            ddlBlngCls.psSet_ErrorNo("5003", ddlBlngCls.ppName)

    '            If intErrFlg = 0 Then
    '                ddlBlngCls.ppDropDownList.Focus()
    '            End If

    '            intErrFlg = 1
    '        End If

    '        '機種区分(必須)
    '        If ddlAppaCls.ppDropDownList.SelectedIndex = 0 Then
    '            ddlAppaCls.psSet_ErrorNo("5003", ddlAppaCls.ppName)

    '            If intErrFlg = 0 Then
    '                ddlAppaCls.ppDropDownList.Focus()
    '            End If

    '            intErrFlg = 1
    '        End If

    '        'エラーがあったか
    '        If intErrFlg = 1 Then
    '            Return False
    '        Else
    '            Return True
    '        End If

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
    '        Return False
    '    End Try

    'End Function

    '--------------------------------
    '2014/04/14 Hamamoto　ここまで
    '--------------------------------

    ''' <summary>
    ''' 明細登録項目チェック
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function msSelfChkMsi() As Boolean
        'MultiLineテキストボックス長さチェック
        'If txaDealDtl.ppText.Length > txaDealDtl.ppMaxLength Then
        '    txaDealDtl.psSet_ErrorNo("3002", txaDealDtl.ppName, CStr(txaDealDtl.ppMaxLength))
        '    txaDealDtl.ppTextBox.Focus()
        '    Return False
        'End If

        Return True
    End Function

#End Region

#Region "■ TBOXID存在チェック"

    ''' <summary>
    ''' TBOXID存在チェック
    ''' </summary>
    ''' <param name="strTboxId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfChkExistsTboxId(ByVal strTboxId As String) As Boolean
        Dim objCon As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        mfChkExistsTboxId = False

        'TBOXの入力がない場合ははじく
        If strTboxId Is Nothing OrElse strTboxId.Trim() = "" Then
            mfChkExistsTboxId = False
        Else
            'DB接続
            If clsDataConnect.pfOpen_Database(objCon) Then
                Try
                    'ストアド設定
                    objCmd = New SqlCommand("BRKUPDP001_S10")
                    objCmd.Connection = objCon
                    objCmd.CommandType = CommandType.StoredProcedure

                    'パラメータ設定
                    With objCmd.Parameters
                        .Add(pfSet_Param("@prmT03_TBOXID", SqlDbType.NVarChar, strTboxId)) 'TBOXID
                    End With

                    'SQL実行
                    Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

                    '取得エラー
                    If objDs Is Nothing OrElse objDs.Tables.Count <= 0 OrElse objDs.Tables(0).Rows.Count <= 0 Then
                        Throw New Exception()
                    End If

                    '存在確認(1件？)
                    If CInt(objDs.Tables(0).Rows(0)("TBOXCNT")) = 1 Then
                        If objDs.Tables(0).Rows(0)("WRKEND_DT").ToString = "" Then
                            mfChkExistsTboxId = True
                        Else
                            If Date.Parse(objDs.Tables(0).Rows(0)("WRKEND_DT")) >= DateTime.Now Or objDs.Tables(0).Rows(0)("WRKEND_DT") = Nothing Then
                                mfChkExistsTboxId = True
                            Else
                                '警告メッセージ
                                psMesBox(Me, "10008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "ホール", "登録")
                            End If
                        End If
                    Else
                        '警告メッセージ
                        psMesBox(Me, "00003", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "入力されたTBOXIDのデータ")
                    End If


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
                    mfChkExistsTboxId = False
                Finally
                    'DB切断
                    If Not clsDataConnect.pfClose_Database(objCon) Then
                        mfChkExistsTboxId = False
                    End If
                End Try
            Else
                mfChkExistsTboxId = False
            End If
        End If

    End Function

#End Region

#Region "■ メインデータ更新関連"

    ''' <summary>
    ''' メインデータ登録
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGrandDataInsert()
        ' ＤＢ接続変数
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim trans As SqlTransaction = Nothing

        Dim strExclusiveDate As String = Nothing
        Dim arTable_Name As New ArrayList
        Dim arKey As New ArrayList
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'ＤＢ接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                'トランザクション開始
                trans = conDB.BeginTransaction()

                cmdDB = New SqlCommand("BRKUPDP001_I1", conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.Transaction = trans

                'パラメータ設定
                If mfSetGrandParameter(cmdDB, 1) = False Then
                    Throw New Exception()
                End If

                '実行
                cmdDB.ExecuteNonQuery()

                'コミット
                trans.Commit()

                Select Case ViewState(P_SESSION_AUTH)
                    Case "管理者", "SPC", "NGC"


                        '★ロック対象テーブル名の登録
                        arTable_Name.Insert(0, "D77_MINI_MANAGE")

                        'ロックテーブルキー項目の登録.
                        arKey.Insert(0, CStr(cmdDB.Parameters("prmD77_MNG_NO").Value))

                        '★排他情報確認処理(更新処理の実行)
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
                            Exit Sub

                        End If
                End Select

                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "ミニ処理票データ")

                '再読み込み
                msReLoad(CStr(cmdDB.Parameters("prmD77_MNG_NO").Value), ClsComVer.E_遷移条件.更新)

            Catch ex As Exception
                'ロールバック
                If Not trans Is Nothing Then
                    trans.Rollback()
                End If

                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ミニ処理票データ")
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
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Sub

    ''' <summary>
    ''' メインデータ更新
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGrandDataUpdate()
        ' ＤＢ接続変数
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim trans As SqlTransaction = Nothing

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'ＤＢ接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                'トランザクション開始
                trans = conDB.BeginTransaction()

                'ストアド設定
                cmdDB = New SqlCommand("BRKUPDP001_U1", conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.Transaction = trans

                'パラメータ設定
                If mfSetGrandParameter(cmdDB, 2) = False Then
                    Throw New Exception()
                End If

                '実行
                cmdDB.ExecuteNonQuery()

                'コミット
                trans.Commit()

                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "ミニ処理票データ")

                '再読み込み
                msReLoad(CStr(cmdDB.Parameters("prmD77_MNG_NO").Value), ClsComVer.E_遷移条件.更新)

            Catch ex As Exception
                'ロールバック
                If Not trans Is Nothing Then
                    trans.Rollback()
                End If

                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ミニ処理票データ")
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
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If
    End Sub

    ''' <summary>
    ''' メインデータ削除(キャンセル区分を立てる)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGrandDataDelete()
        ' ＤＢ接続変数
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim trans As SqlTransaction = Nothing

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'ＤＢ接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                'トランザクション開始
                trans = conDB.BeginTransaction()

                'ストアド設定
                cmdDB = New SqlCommand("BRKUPDP001_U2")
                cmdDB.Connection = conDB
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.Transaction = trans

                'パラメータ設定
                If mfSetGrandParameter(cmdDB, 3) = False Then
                    Throw New Exception()
                End If

                '実行
                cmdDB.ExecuteNonQuery()

                'コミット
                trans.Commit()

                psMesBox(Me, "00011", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "ミニ処理票データ")

                '再読み込み
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録
                ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録

                'キー値設定
                Session(P_KEY) = {""}
                ViewState(P_KEY) = {""}

                'ボタンの設定
                msSetButton()

                'オールクリア
                txtTboxId.ppText = ""
                msInitKmkMain(1)
                msNotSelectDdl(1)

                'Enable制御
                msEdit_Enabled(CBool(ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.参照))

                'AutoPostback設定(Tbox入力検索のため)
                txtTboxId.ppTextBox.AutoPostBack = True

                Page.SetFocus(Me.txtTboxId.FindControl("txtTextBox"))

                'ボタン機能セット
                msSetAddHandler(CInt(ViewState(P_SESSION_TERMS)))

                ''再読み込み
                'msGet_Data()

                'ENABLE制御を更新用に変更
                msEdit_Enabled(CBool(ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.参照))
                'msReLoad(CStr(cmdDB.Parameters("prmD77_MNG_NO").Value),  ClsComVer.E_遷移条件.参照)


            Catch ex As Exception
                'ロールバック
                If Not trans Is Nothing Then
                    trans.Rollback()
                End If

                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ミニ処理票データのキャンセル")
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
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If
    End Sub

#End Region

#Region "■ パラメータ設定"

    ''' <summary>
    ''' メインデータパラメータ設定
    ''' </summary>
    ''' <param name="objCmd"></param>
    ''' <param name="intMode">1:登録、2:更新、3:削除</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfSetGrandParameter(ByRef objCmd As SqlCommand, ByVal intMode As Integer) As Boolean
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            'SqlComanndがない
            If objCmd Is Nothing Then
                Throw New Exception()
            End If

            'パラメータ設定
            With objCmd.Parameters
                '登録or更新の時
                If intMode = 1 OrElse intMode = 2 Then

                    '登録の時
                    If intMode = 1 Then
                        .Add(pfSet_Param("prmD77_TBOXID", SqlDbType.NVarChar, txtTboxId.ppText)) 'TBOXID
                        .Add(pfSet_Param("prmD77_NL_CLS", SqlDbType.NVarChar, mfChgNlToBrank(hdnNlclsCd))) 'NL区分
                        .Add(pfSet_Param("prmD77_EW_CLS", SqlDbType.NVarChar, mfChgNlToBrank(hdnEwclsCd))) 'EW区分
                        .Add(pfSet_Param("prmD77_TBOX_VER", SqlDbType.NVarChar, mfChgNlToBrank(hdnVersion))) 'TBOXVersion
                        .Add(pfSet_Param("prmD77_TBOX_TYPE", SqlDbType.NVarChar, mfChgNlToBrank(hdnTboxType))) 'TBOXType
                        .Add(pfSet_Param("prmD77_HALL_CD", SqlDbType.NVarChar, mfChgNlToBrank(hdnHallCd))) 'ホールＣＤ
                        .Add(pfSet_Param("prmD77_HALL_NM", SqlDbType.NVarChar, lblHallNm.Text)) 'ホール名
                        .Add(pfSet_Param("prmD77_ADDR", SqlDbType.NVarChar, lblHallAddr.Text)) 'ホール住所
                        .Add(pfSet_Param("prmD77_TELNO", SqlDbType.NVarChar, lblTel.Text)) 'ホールTel
                        '.Add(pfSet_Param("prmD77_UNF_CD", SqlDbType.NVarChar, mfChgNlToBrank(hdnTokatuCd))) '統括コード
                        .Add(pfSet_Param("prmD77_BRANCH_CD", SqlDbType.NVarChar, mfChgNlToBrank(hdnEigyosyoCd))) '営業所コード
                        .Add(pfSet_Param("prmD77_ACG_COD", SqlDbType.NVarChar, mfChgNlToBrank(hdnAgcCd))) '代理店コード
                        .Add(pfSet_Param("prmD77_ACG_NAM", SqlDbType.NVarChar, lblAgc.Text)) '代理店名
                        .Add(pfSet_Param("prmD77_ACG_ZIP", SqlDbType.NVarChar, mfChgNlToBrank(hdnAgcZip))) '代理店郵便番号
                        .Add(pfSet_Param("prmD77_ACG_ADS", SqlDbType.NVarChar, mfChgNlToBrank(hdnAgcAddr))) '代理店住所
                        .Add(pfSet_Param("prmD77_ACG_TEL", SqlDbType.NVarChar, mfChgNlToBrank(hdnAgcTel))) '代理店ＴＥＬ
                        .Add(pfSet_Param("prmD77_ACG_FAX", SqlDbType.NVarChar, mfChgNlToBrank(hdnAgcFax))) '代理店ＦＡＸ
                        '.Add(pfSet_Param("prmD77_PER_COD", SqlDbType.NVarChar, mfChgNlToBrank(hdnRepCd))) '代行店コード
                        '.Add(pfSet_Param("prmD77_PER_NAM", SqlDbType.NVarChar, lblRep.Text)) '代行店名
                        '.Add(pfSet_Param("prmD77_PER_ZIP", SqlDbType.NVarChar, mfChgNlToBrank(hdnRepZip))) '代行店郵便番号
                        '.Add(pfSet_Param("prmD77_PER_ADS", SqlDbType.NVarChar, mfChgNlToBrank(hdnRepAddr))) '代行店住所
                        '.Add(pfSet_Param("prmD77_PER_TEL", SqlDbType.NVarChar, mfChgNlToBrank(hdnRepTel))) '代行店ＴＥＬ
                        '.Add(pfSet_Param("prmD77_PER_STF_COD", SqlDbType.NVarChar, mfChgNlToBrank(hdnRepChg))) '代行店担当者
                        .Add(pfSet_Param("prmD77_NGC_BNS", SqlDbType.NVarChar, lblNgcOrg.Text)) 'ＮＧＣ担当営業部
                        '.Add(pfSet_Param("prmD77_NGC_TEL", SqlDbType.NVarChar, lblOrgTel.Text)) 'ＮＧＣ担当営業部ＴＥＬ
                        .Add(pfSet_Param("prmD77_MDN_SET_DVS", SqlDbType.NVarChar, mfChgNlToBrank(hdnTwinCd))) '双子店区分
                        '.Add(pfSet_Param("prmD77_MDN_DVS", SqlDbType.NVarChar, mfChgNlToBrank(hdnEstCls))) 'ＭＤＮ設置区分
                        '.Add(pfSet_Param("prmD77_MDN_SUU", SqlDbType.NVarChar, mfChgNlToBrank(hdnMdnCnt))) 'ＭＤＮ台数
                        '.Add(pfSet_Param("prmD77_MDN_COD1", SqlDbType.NVarChar, mfChgNlToBrank(hdnMdnCd1))) 'ＭＤＮ機種コード１
                        '.Add(pfSet_Param("prmD77_MDN_COD2", SqlDbType.NVarChar, mfChgNlToBrank(hdnMdnCd2))) 'ＭＤＮ機種コード２
                        '.Add(pfSet_Param("prmD77_MDN_COD3", SqlDbType.NVarChar, mfChgNlToBrank(hdnMdnCd3))) 'ＭＤＮ機種コード３

                    End If

                    .Add(pfSet_Param("prmD77_CALL_CLS", SqlDbType.NVarChar, ddlCallCls.ppSelectedValue)) 'コール区分
                    '.Add(pfSet_Param("prmD77_BLNG_CLS", SqlDbType.NVarChar, ddlBlngCls.ppSelectedValue)) '所属区分
                    '.Add(pfSet_Param("prmD77_APPA_CLS", SqlDbType.NVarChar, ddlAppaCls.ppSelectedValue)) '機種区分
                    '申告日
                    msSetDatePrm(objCmd, "prmD77_RPT_DT", dttShinkokuDt.ppText)

                    '受付日
                    'Dim strHour As String = txtUketukeDt.ppHourText
                    'Dim strMin As String = txtUketukeDt.ppMinText
                    'msSetZero(strHour, strMin)
                    'If txtUketukeDt.ppText <> "" Then
                    '    msSetDatePrm(objCmd, "prmD77_RCPT_DT", txtUketukeDt.ppText & " " & strHour & ":" & strMin & ":00")
                    'Else
                    '    .Add(pfSet_Param("prmD77_RCPT_DT", SqlDbType.DateTime, DBNull.Value))
                    'End If

                    ''NGC報告日時
                    'strHour = txtHokokuDt.ppHourText
                    'strMin = txtHokokuDt.ppMinText
                    'msSetZero(strHour, strMin)
                    'If txtHokokuDt.ppText <> "" Then
                    '    msSetDatePrm(objCmd, "prmD77_REPORT_DT", txtHokokuDt.ppText & " " & strHour & ":" & strMin & ":00")
                    'Else
                    '    .Add(pfSet_Param("prmD77_REPORT_DT", SqlDbType.DateTime, DBNull.Value))
                    'End If

                    '確認日
                    'msSetDatePrm(objCmd, "prmD77_APP_DT", txtKakuninDt.ppText)

                    '.Add(pfSet_Param("prmD77_RPT_BASE", SqlDbType.NVarChar, ddlRptBase.SelectedValue)) '申告元
                    .Add(pfSet_Param("prmD77_RPT_CHARGE", SqlDbType.NVarChar, txtShinkokuNm.ppText)) '申告者
                    .Add(pfSet_Param("prmD77_RPT_TEL", SqlDbType.NVarChar, txtShinkokuTel.ppText)) '申告者TEL
                    .Add(pfSet_Param("prmD77_RCPT_CHARGE", SqlDbType.NVarChar, txtUketukeNm.ppText)) '受付者
                    .Add(pfSet_Param("prmD77_RPT_CD", SqlDbType.NVarChar, ddlRptCd.SelectedValue)) '申告内容
                    .Add(pfSet_Param("prmD77_RPT_DTL1", SqlDbType.NVarChar, txtShinkokuDtl1.ppText)) '申告内容１
                    '.Add(pfSet_Param("prmD77_RPT_DTL2", SqlDbType.NVarChar, txtShinkokuDtl2.ppText)) '申告内容２
                    .Add(pfSet_Param("prmD77_INH_CNTNT", SqlDbType.NVarChar, txtHikitugiDtl.ppText)) '引継内容
                    .Add(pfSet_Param("prmD77_STATUS_CD", SqlDbType.NVarChar, ddlStatusCd.SelectedValue)) '作業状況
                    '.Add(pfSet_Param("prmD77_STTS_NOTETEXT", SqlDbType.NVarChar, txtSttsNotetext.ppText)) '作業状況備考
                    '.Add(pfSet_Param("prmD77_DEAL_CD", SqlDbType.NVarChar, ddlDealDtl.SelectedValue)) '処置内容
                    '.Add(pfSet_Param("prmD77_DEAL_DTL", SqlDbType.NVarChar, txtSyotiDtl.ppText)) '処置内容フリー
                    '.Add(pfSet_Param("prmD77_APP_USR", SqlDbType.NVarChar, txtKakuninNm.ppText)) '確認者
                    '.Add(pfSet_Param("prmD77_TRBL_NO", SqlDbType.NVarChar, txtTroubleNo.ppText)) 'トラブル管理番号

                Else
                    .Add(pfSet_Param("prmD77_CANCEL_CLS", SqlDbType.NVarChar, "1")) 'キャンセルフラグ
                End If

                '管理番号
                Dim strKanriNo As String = ""
                If intMode = 1 Then
                    'T08_管理番号マスタより管理番号を採番
                    strKanriNo = mfGetKanriNo()

                    '取得できていない場合はエラー
                    If strKanriNo.Trim() = "" Then
                        Throw New Exception()
                    End If

                    .Add(pfSet_Param("prmD77_MNG_NO", SqlDbType.NVarChar, strKanriNo))
                Else
                    .Add(pfSet_Param("prmD77_MNG_NO", SqlDbType.NVarChar, lblKanriNo.Text))
                End If

                Dim dtNow As DateTime = DateTime.Now
                '更新or削除の時
                If intMode = 1 Then
                    .Add(pfSet_Param("prmD77_INSERT_DT", SqlDbType.DateTime, dtNow)) '登録日時
                    .Add(pfSet_Param("prmD77_INSERT_USR", SqlDbType.NVarChar, CStr(ViewState(P_SESSION_USERID)))) '登録者
                Else
                    .Add(pfSet_Param("prmD77_UPDATE_DT", SqlDbType.DateTime, dtNow)) '更新日時
                    .Add(pfSet_Param("prmD77_UPDATE_USR", SqlDbType.NVarChar, CStr(ViewState(P_SESSION_USERID)))) '更新者
                End If

            End With

            Return True

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
            Return False
        End Try

    End Function

#End Region

#Region "■ キー項目採番処理"

    ''' <summary>
    ''' 管理番号採番
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetKanriNo() As String
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                'ストアド設定
                cmdDB = New SqlCommand("ZCMPSEL022")
                cmdDB.Connection = conDB
                cmdDB.CommandType = CommandType.StoredProcedure

                Dim dtNow As DateTime = DateTime.Now

                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("MNGNO_CLS", SqlDbType.Int, intKANRIBANGO_SBT)) '管理番号種別
                    .Add(pfSet_Param("YMD", SqlDbType.NVarChar, String.Format("{0:yyyyMMdd}", DateTime.Now))) '年月
                    .Add(pfSet_Param("SalesYTD", SqlDbType.Int, 20, ParameterDirection.Output)) '最新番号
                End With

                Dim intRet As Integer = 0

                intRet = cmdDB.ExecuteNonQuery()

                '処理失敗
                If intRet = 0 OrElse cmdDB.Parameters("SalesYTD").Value Is DBNull.Value Then
                    Throw New Exception()
                End If

                '管理番号を"SPCYYMM-9999"というフォーマットに編集
                Dim strVal As String = "SPC" & String.Format("{0:yyMM}", dtNow) & "-" & CStr(cmdDB.Parameters("SalesYTD").Value).PadLeft(4, "0"c)

                Return strVal

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
                Return ""
            Finally
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try

        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Return ""
        End If

    End Function

    ''' <summary>
    ''' 最大ミニ処理管理連番取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetMaxKanriSeq(ByVal strKanriNo As String) As String
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGetMaxKanriSeq = ""

        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                '入力がない場合はエラー
                If strKanriNo.Trim() = "" Then
                    Throw New Exception()
                End If

                'ストアド設定
                cmdDB = New SqlCommand("BRKUPDP001_S4")
                cmdDB.Connection = conDB
                cmdDB.CommandType = CommandType.StoredProcedure

                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("prmD78_MNG_NO", SqlDbType.NVarChar, strKanriNo)) 'ミニ管理番号
                End With

                'SQL実行
                Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)

                If objDs Is Nothing OrElse objDs.Tables.Count <= 0 Then
                    mfGetMaxKanriSeq = ""
                End If

                If objDs.Tables(0).Rows.Count = 0 Then
                    'データがない場合は1を返す
                    mfGetMaxKanriSeq = "1"
                Else
                    'データがある場合はMAX + 1を返す
                    mfGetMaxKanriSeq = objDs.Tables(0).Rows(0)("MAXCNT")
                End If

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
                mfGetMaxKanriSeq = ""
            Finally
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    mfGetMaxKanriSeq = ""
                End If
            End Try

        Else
            mfGetMaxKanriSeq = ""
        End If
    End Function

#End Region

#Region "■ 明細データ更新関連"

    ''' <summary>
    ''' 明細データ登録
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msMsiDataInsert()
        ' ＤＢ接続変数
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim trans As SqlTransaction = Nothing

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'ＤＢ接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                'トランザクション開始
                trans = conDB.BeginTransaction()

                '最大ミニ管理連番採番
                Dim strKanriSeq As String = mfGetMaxKanriSeq(lblKanriNo.Text)

                '取得エラー
                If strKanriSeq = "" Then
                    Throw New Exception()
                End If

                'ストアド設定
                cmdDB = New SqlCommand("BRKUPDP001_I2")
                cmdDB.Connection = conDB
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.Transaction = trans

                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("prmD78_MNG_NO", SqlDbType.NVarChar, lblKanriNo.Text)) 'ミニ管理番号
                    .Add(pfSet_Param("prmD78_MNG_SEQ", SqlDbType.NVarChar, strKanriSeq))     'ミニ管理連番

                    '対応日時
                    Dim strHour As String = txtTaiouDt.ppHourText
                    Dim strMin As String = txtTaiouDt.ppMinText
                    msSetZero(strHour, strMin)
                    If txtTaiouDt.ppText <> "" Then
                        msSetDatePrm(cmdDB, "prmD78_DEAL_DT", txtTaiouDt.ppText & " " & strHour & ":" & strMin & ":00")
                    Else
                        .Add(pfSet_Param("prmD78_DEAL_DT", SqlDbType.DateTime, DBNull.Value))
                    End If

                    .Add(pfSet_Param("prmD78_DEAL_USR", SqlDbType.NVarChar, txtTaiouNm.ppText)) '対応者
                    .Add(pfSet_Param("prmD78_DEAL_DTL", SqlDbType.NVarChar, txaDealDtl.Text)) '対応内容
                    .Add(pfSet_Param("prmD78_DEAL_CD", SqlDbType.NVarChar, "TE")) '対応コード(「TE」固定)
                    .Add(pfSet_Param("prmD78_DELETE_FLG", SqlDbType.NVarChar, "0")) '削除フラグ
                    .Add(pfSet_Param("prmD78_INSERT_DT", SqlDbType.DateTime, DateTime.Now)) '登録日時
                    .Add(pfSet_Param("prmD78_INSERT_USR", SqlDbType.NVarChar, ViewState(P_SESSION_USERID))) '登録者
                End With

                '実行
                cmdDB.ExecuteNonQuery()

                'コミット
                trans.Commit()

                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "ミニ処理票明細データ")

                '一覧登録項目初期化
                msInitKmkDtl()

                Dim strKey() As String = Nothing
                Dim strVal As String = lblKanriNo.Text.Trim()

                'ＤＢ接続
                If clsDataConnect.pfOpen_Database(conDB) Then
                    'ビューステート項目取得
                    strKey = ViewState(P_KEY)

                    '画面ページ表示初期化
                    Me.grvList.DataSource = New DataTable()

                    'ストアド設定
                    cmdDB = New SqlCommand("BRKUPDP001_S2", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure

                    'パラメータ設定
                    With cmdDB.Parameters
                        If strVal = "" Then
                            .Add(pfSet_Param("prmD78_MNG_NO", SqlDbType.NVarChar, strKey(0))) 'ミニ管理番号
                        Else
                            .Add(pfSet_Param("prmD78_MNG_NO", SqlDbType.NVarChar, strVal)) 'ミニ管理番号
                        End If
                    End With

                    Dim dstOrders_2 As DataSet = Nothing
                    dstOrders_2 = clsDataConnect.pfGet_DataSet(cmdDB)

                    '取得エラー
                    If dstOrders_2 Is Nothing OrElse dstOrders_2.Tables.Count < 0 Then
                        Throw New Exception()
                    End If

                    '一覧設定
                    grvList.DataSource = dstOrders_2.Tables(0)
                    Me.grvList.DataBind()

                Else
                    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

                '再読み込み
                'msReLoad(lblKanriNo.Text,  ClsComVer.E_遷移条件.更新)

            Catch ex As Exception
                'ロールバック
                If Not trans Is Nothing Then
                    trans.Rollback()
                End If

                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ミニ処理票明細データの追加")
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
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Sub

    ''' <summary>
    ''' 明細データ更新
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msMsiDataUpdate()
        ' ＤＢ接続変数
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim trans As SqlTransaction = Nothing

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'ＤＢ接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                'トランザクション開始
                trans = conDB.BeginTransaction()

                'ストアド設定
                cmdDB = New SqlCommand("BRKUPDP001_U3")
                cmdDB.Connection = conDB
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.Transaction = trans

                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("prmD78_MNG_NO", SqlDbType.NVarChar, hdnMsiKey1.Value)) 'ミニ管理番号
                    .Add(pfSet_Param("prmD78_MNG_SEQ", SqlDbType.NVarChar, hdnMsiKey2.Value)) 'ミニ管理連番

                    '対応日時
                    Dim strHour As String = txtTaiouDt.ppHourText
                    Dim strMin As String = txtTaiouDt.ppMinText
                    msSetZero(strHour, strMin)
                    If txtTaiouDt.ppText <> "" Then
                        msSetDatePrm(cmdDB, "prmD78_DEAL_DT", txtTaiouDt.ppText & " " & strHour & ":" & strMin & ":00")
                    Else
                        .Add(pfSet_Param("prmD78_DEAL_DT", SqlDbType.DateTime, DBNull.Value))
                    End If

                    .Add(pfSet_Param("prmD78_DEAL_USR", SqlDbType.NVarChar, txtTaiouNm.ppText)) '対応者
                    .Add(pfSet_Param("prmD78_DEAL_DTL", SqlDbType.NVarChar, txaDealDtl.Text)) '対応内容
                    .Add(pfSet_Param("prmD78_UPDATE_DT", SqlDbType.DateTime, DateTime.Now)) '登録日時
                    .Add(pfSet_Param("prmD78_UPDATE_USR", SqlDbType.NVarChar, ViewState(P_SESSION_USERID))) '登録者

                End With

                '実行
                cmdDB.ExecuteNonQuery()

                'コミット
                trans.Commit()

                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "ミニ処理票明細データ")

                '一覧登録項目初期化
                msInitKmkDtl()

                Dim strKey() As String = Nothing
                Dim strVal As String = lblKanriNo.Text.Trim()

                'ＤＢ接続
                If clsDataConnect.pfOpen_Database(conDB) Then
                    'ビューステート項目取得
                    strKey = ViewState(P_KEY)

                    '画面ページ表示初期化
                    Me.grvList.DataSource = New DataTable()

                    'ストアド設定
                    cmdDB = New SqlCommand("BRKUPDP001_S2", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure

                    'パラメータ設定
                    With cmdDB.Parameters
                        If strVal = "" Then
                            .Add(pfSet_Param("prmD78_MNG_NO", SqlDbType.NVarChar, strKey(0))) 'ミニ管理番号
                        Else
                            .Add(pfSet_Param("prmD78_MNG_NO", SqlDbType.NVarChar, strVal)) 'ミニ管理番号
                        End If
                    End With

                    Dim dstOrders_2 As DataSet = Nothing
                    dstOrders_2 = clsDataConnect.pfGet_DataSet(cmdDB)

                    '取得エラー
                    If dstOrders_2 Is Nothing OrElse dstOrders_2.Tables.Count < 0 Then
                        Throw New Exception()
                    End If

                    '一覧設定
                    grvList.DataSource = dstOrders_2.Tables(0)
                    Me.grvList.DataBind()

                Else
                    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

                '再読み込み
                'msReLoad(lblKanriNo.Text,  ClsComVer.E_遷移条件.更新)

            Catch ex As Exception
                'ロールバック
                If Not trans Is Nothing Then
                    trans.Rollback()
                End If

                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ミニ処理票明細データの更新")
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
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If
    End Sub

    ''' <summary>
    ''' 明細データ削除(物理削除)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msMsiDataDelete()
        ' ＤＢ接続変数
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim trans As SqlTransaction = Nothing

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'ＤＢ接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                'トランザクション開始
                trans = conDB.BeginTransaction()

                'ストアド設定
                cmdDB = New SqlCommand("BRKUPDP001_U4")
                cmdDB.Connection = conDB
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.Transaction = trans

                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("prmD78_MNG_NO", SqlDbType.NVarChar, hdnMsiKey1.Value)) 'ミニ管理番号
                    .Add(pfSet_Param("prmD78_MNG_SEQ", SqlDbType.NVarChar, hdnMsiKey2.Value)) 'ミニ管理連番
                End With

                '実行
                cmdDB.ExecuteNonQuery()

                'コミット
                trans.Commit()

                psMesBox(Me, "00002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "ミニ処理票明細データ")

                '一覧登録項目初期化
                msInitKmkDtl()

                Dim strKey() As String = Nothing
                Dim strVal As String = lblKanriNo.Text.Trim()

                'ＤＢ接続
                If clsDataConnect.pfOpen_Database(conDB) Then
                    'ビューステート項目取得
                    strKey = ViewState(P_KEY)

                    '画面ページ表示初期化
                    Me.grvList.DataSource = New DataTable()

                    'ストアド設定
                    cmdDB = New SqlCommand("BRKUPDP001_S2", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure

                    'パラメータ設定
                    With cmdDB.Parameters
                        If strVal = "" Then
                            .Add(pfSet_Param("prmD78_MNG_NO", SqlDbType.NVarChar, strKey(0))) 'ミニ管理番号
                        Else
                            .Add(pfSet_Param("prmD78_MNG_NO", SqlDbType.NVarChar, strVal)) 'ミニ管理番号
                        End If
                    End With

                    Dim dstOrders_2 As DataSet = Nothing
                    dstOrders_2 = clsDataConnect.pfGet_DataSet(cmdDB)

                    '取得エラー
                    If dstOrders_2 Is Nothing OrElse dstOrders_2.Tables.Count < 0 Then
                        Throw New Exception()
                    End If

                    '一覧設定
                    grvList.DataSource = dstOrders_2.Tables(0)
                    Me.grvList.DataBind()

                Else
                    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

                '再読み込み
                'msReLoad(lblKanriNo.Text,  ClsComVer.E_遷移条件.更新)

            Catch ex As Exception
                'ロールバック
                If Not trans Is Nothing Then
                    trans.Rollback()
                End If

                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ミニ処理票明細データの削除")
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
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If
    End Sub

#End Region

#Region "■ その他"

    ''' <summary>
    ''' 日付項目パラメータセット
    ''' </summary>
    ''' <param name="objCmd"></param>
    ''' <param name="strPrmNm"></param>
    ''' <param name="strDtVal"></param>
    ''' <remarks></remarks>
    Private Sub msSetDatePrm(ByRef objCmd As SqlCommand, ByVal strPrmNm As String, ByVal strDtVal As String)
        With objCmd.Parameters
            Dim dtVal As DateTime = Nothing

            '日付かチェック
            If DateTime.TryParse(strDtVal, dtVal) Then
                .Add(pfSet_Param(strPrmNm, SqlDbType.DateTime, dtVal))
            Else
                .Add(pfSet_Param(strPrmNm, SqlDbType.DateTime, DBNull.Value))
            End If
        End With
    End Sub

    ''' <summary>
    ''' Nothingを空白に変換
    ''' </summary>
    ''' <param name="objCtrl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfChgNlToBrank(ByVal objCtrl As HiddenField)
        Dim strVal As String = ""

        If objCtrl.Value Is Nothing Then
            strVal = ""
        Else
            strVal = CStr(objCtrl.Value)
        End If

        Return strVal

    End Function

    ''' <summary>
    ''' 時分項目設定(空白を00に)
    ''' </summary>
    ''' <param name="strHour"></param>
    ''' <param name="strMin"></param>
    ''' <remarks></remarks>
    Private Sub msSetZero(ByRef strHour As String, ByRef strMin As String)
        If strHour = "" Then
            strHour = "00"
        End If

        If strMin = "" Then
            strMin = "00"
        End If
    End Sub
#End Region

#End Region

#Region "終了処理プロシージャ"
#End Region
End Class
