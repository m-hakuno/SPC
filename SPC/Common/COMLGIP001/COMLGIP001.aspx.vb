'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　ログイン
'*　ＰＧＭＩＤ：　COMLGIP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.10.22　：　土岐
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'COMLGIP001-001     2017.03.28      加賀       ユーザーの管理者判定用セッション変数追加
'COMLGIP001-002     2017.10.10      伯野       各プロシージャにデータセットのdisposeを記述

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
'Imports SPC.Global_asax
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports System.Data.SqlClient

#End Region

Public Class COMLGIP001
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
    Const M_DISP_ID = P_FUN_COM & P_SCR_LGI & P_PAGE & "001"
    Const M_NGC_CD = "1"
    Const M_WKB_CD = "2"
    Const M_FS_CD = "7"
    Const M_SPC_CD = "8"
    Const M_VIEW_RE_COUNT = "再ログイン回数"
    Const M_VIEW_RE_ID = "再ログインID"
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

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strTitle As String

        Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.非表示
        If Not IsPostBack Then
            Master.ppProgramID = M_DISP_ID

            'タイトル設定
            strTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)
            Master.ppTitle = strTitle

            txtUserNm.ppText = String.Empty
            txtPass.ppText = String.Empty
            txtUserNm.ppTextBox.Focus()

            If System.IO.Path.GetFileName(FormsAuthentication.GetRedirectUrl("ExcDel", False)) = "ExculsiveList.aspx" Then
                FormsAuthentication.RedirectFromLoginPage("", False)
            End If

        End If

    End Sub

    ''' <summary>
    ''' ログインボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If (Page.IsValid) Then
            Dim strCnfAddress As String
            Dim conDB As SqlConnection = Nothing
            Dim datLoginDT As DateTime
            Dim intRtn As Integer
            Dim strIPAddress As String
            Dim strAuthority As String = ""

            objStack = New StackFrame


            'ログ出力開始
            psLogStart(Me)

            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    '拠点情報取得
                    strCnfAddress = ConfigurationManager.AppSettings("Address")

                    'IPアドレス取得(SPC以外は空文字)
                    Select Case strCnfAddress
                        Case P_ADD_SPC
                            strIPAddress = Request.ServerVariables("REMOTE_ADDR").ToString()
                        Case Else
                            strIPAddress = String.Empty
                    End Select

                    'ユーザ、パスワードチェック
                    intRtn = mfCheck_User(conDB, strCnfAddress)
                    Select Case intRtn
                        Case -1 '該当ユーザなし
                            'ポップアップメッセージ表示
                            psMesBox(Me, "30001", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)

                            'フォーカスをユーザIDへ
                            txtUserNm.ppTextBox.Focus()
                            Exit Sub
                        Case -2 '妥当性エラー
                            'ポップアップメッセージ表示
                            psMesBox(Me, "30014", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

                            'フォーカスをユーザIDへ
                            txtUserNm.ppTextBox.Focus()
                            Exit Sub
                    End Select


                    'ログインチェック
                    intRtn = mfCheck_Login(conDB, strCnfAddress, strIPAddress)

                    'データ更新
                    Using conTrn = conDB.BeginTransaction
                        If intRtn = -1 Then '該当ユーザ有
                            Select Case strCnfAddress
                                Case P_ADD_SPC
                                    'ポップアップメッセージ表示
                                    psMesBox(Me, "30002", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)

                                    'フォーカスをユーザIDへ
                                    txtUserNm.ppTextBox.Focus()
                                    Exit Sub

                                Case P_ADD_NGC, P_ADD_WKB
                                    If ViewState(M_VIEW_RE_COUNT) = 1 _
                                        AndAlso ViewState(M_VIEW_RE_ID) = Me.txtUserNm.ppText Then

                                        'ログイン状態削除
                                        intRtn = mfDelLogin(conDB, conTrn, strIPAddress, datLoginDT)
                                        psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ログイン状態")
                                        If intRtn <> 0 Then
                                            Exit Sub
                                        End If
                                        '画面情報登録
                                        ViewState(M_VIEW_RE_COUNT) = 0
                                    Else
                                        '画面情報登録
                                        ViewState(M_VIEW_RE_COUNT) = 1
                                        ViewState(M_VIEW_RE_ID) = Me.txtUserNm.ppText

                                        'ポップアップメッセージ表示
                                        psMesBox(Me, "30002", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                                        psMesBox(Me, "30007", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)

                                        '画面クリア
                                        msClearScreen()

                                        Exit Sub
                                    End If
                            End Select
                        End If

                        'ログイン状態追加
                        intRtn = mfAddLogin_Situ(conDB, conTrn, strIPAddress, datLoginDT)
                        If intRtn <> 0 Then
                            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ログイン状態")
                            Exit Sub
                        End If

                        '排他制御削除
                        intRtn = mfDelExclusive(conDB, conTrn, strCnfAddress, strIPAddress)
                        If intRtn <> 0 Then
                            psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "排他制御情報")
                            Exit Sub
                        End If

                        'コミット
                        conTrn.Commit()
                    End Using

                Catch ex As Exception
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ユーザ")
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    Exit Sub
                Finally
                    'DB切断
                    If Not clsDataConnect.pfClose_Database(conDB) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        intRtn = -1
                    End If
                End Try

                If intRtn <> 0 Then
                    Exit Sub
                End If

                '権限区分
                Select Case strCnfAddress
                    Case "SPC"
                        Select Case Session(P_SESSION_AUTH)
                            Case "1"
                                strAuthority = "SPC"
                            Case "2"
                                strAuthority = "管理者"
                        End Select
                    Case "NGC"
                        strAuthority = "NGC"
                    Case "WKB"
                        strAuthority = "営業所"
                End Select

                'セッション情報設定
                Session(P_SESSION_USERID) = Me.txtUserNm.ppText
                Session(P_SESSION_LOGIN_DATE) = datLoginDT
                Session(P_SESSION_PLACE) = strCnfAddress
                Session(P_SESSION_SESSTION_ID) = Session.SessionID
                Session(P_SESSION_IP) = strIPAddress
                Session(P_SESSION_ADMIN) = Session(P_SESSION_AUTH)  'COMLGIP001-001 '1:一般 2:管理者
                Session(P_SESSION_AUTH) = strAuthority

                USER_NAME = Me.txtUserNm.ppText

                'ログイン
                FormsAuthentication.RedirectFromLoginPage(txtUserNm.ppText, False)
            Else
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End If

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' クリアボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        'ログ出力開始
        psLogStart(Me)
        msClearScreen()
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

        Me.txtPass.ppText = String.Empty
        Me.txtUserNm.ppText = String.Empty
        txtUserNm.ppTextBox.Focus()

    End Sub

    ''' <summary>
    ''' ユーザとパスワードのチェックを行う
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ</param>
    ''' <param name="ipstrCnfAddress">起動場所</param>
    ''' <returns>0:正常　-1:該当ユーザなし　-2:妥当性エラー</returns>
    ''' <remarks></remarks>
    Private Function mfCheck_User(ByVal ipconDB As SqlConnection,
                                  ByVal ipstrCnfAddress As String) As Integer
        Dim cmdUser As SqlCommand
        Dim dstUser As DataSet
        Dim strCheckAdd As String

        cmdUser = New SqlCommand("COMLGIP001_S1", ipconDB)
        With cmdUser
            'コマンドタイプ設定(ストアド)
            .CommandType = CommandType.StoredProcedure
            'パラメータ設定
            .Parameters.Add(pfSet_Param("userid", SqlDbType.NVarChar, Me.txtUserNm.ppText))
            .Parameters.Add(pfSet_Param("pass", SqlDbType.NVarChar, Me.txtPass.ppText))
            .Parameters.Add(pfSet_Param("delf", SqlDbType.NVarChar, "0"))
        End With

        'ユーザーマスタからデータ取得
        dstUser = clsDataConnect.pfGet_DataSet(cmdUser)

        If dstUser.Tables(0).Rows.Count > 0 Then   'ユーザ該当あり
            '妥当性チェック
            Select Case dstUser.Tables(0).Rows(0).Item("業者コード")
                Case M_NGC_CD
                    strCheckAdd = P_ADD_NGC
                Case M_WKB_CD
                    strCheckAdd = P_ADD_WKB
                Case M_SPC_CD, M_FS_CD
                    strCheckAdd = P_ADD_SPC
                Case Else
                    '妥当性エラー
                    Return -2
            End Select
            '---------------------------
            '2014/04/11 武 ここから
            '---------------------------
            '権限取得
            Select Case dstUser.Tables(0).Rows(0).Item("権限区分")
                Case 1
                    Session(P_SESSION_AUTH) = "1"
                Case Else
                    Session(P_SESSION_AUTH) = "2"
            End Select
            '---------------------------
            '2014/04/11 武 ここまで
            '---------------------------

            If ipstrCnfAddress <> strCheckAdd Then

                'データセットの破棄
                Call psDisposeDataSet(dstUser)

                '妥当性エラー
                Return -2
            End If
        Else
            'データセットの破棄
            Call psDisposeDataSet(dstUser)
            'ユーザ該当なし
            Return -1
        End If

        'データセットの破棄
        Call psDisposeDataSet(dstUser)

        '正常終了
        Return 0

    End Function

    ''' <summary>
    ''' ログインユーザのチェックを行う
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ</param>
    ''' <param name="ipstrAddress">起動場所</param>
    ''' <param name="ipstrIPAddress">ＩＰアドレス</param>
    ''' <returns>0:正常　-1:該当ユーザ有</returns>
    ''' <remarks></remarks>
    Private Function mfCheck_Login(ByVal ipconDB As SqlConnection,
                                   ByVal ipstrAddress As String,
                                   ByVal ipstrIPAddress As String) As Integer
        Dim cmdLogin As SqlCommand
        Dim dstLogin As DataSet

        'ログイン状態からデータ取得
        cmdLogin = New SqlCommand("COMLGIP001_S2", ipconDB)

        With cmdLogin
            'コマンドタイプ設定(ストアド)
            .CommandType = CommandType.StoredProcedure
            'パラメータ設定
            .Parameters.Add(pfSet_Param("userid", SqlDbType.NVarChar, Me.txtUserNm.ppText))
            If ipstrAddress = P_ADD_SPC Then
                .Parameters.Add(pfSet_Param("session_id", SqlDbType.NVarChar, Session.SessionID))
                .Parameters.Add(pfSet_Param("ipaddress", SqlDbType.NVarChar, ipstrIPAddress))
            End If
        End With
        dstLogin = clsDataConnect.pfGet_DataSet(cmdLogin)

        If dstLogin.Tables(0).Rows.Count > 0 Then   'ログイン状態該当あり

            'データセットの破棄
            Call psDisposeDataSet(dstLogin)

            '同一ログイン
            Return -1
        End If

        'データセットの破棄
        Call psDisposeDataSet(dstLogin)

        '正常終了
        Return 0

    End Function

    ''' <summary>
    ''' ログイン状態追加
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ</param>
    ''' <param name="ipconTrn">トランザクション</param>
    ''' <param name="ipstrIPAddress">ＩＰアドレス</param>
    ''' <param name="opdatLoginDT">登録時のログイン日時</param>
    ''' <returns>ＳＱＬ実行結果</returns>
    ''' <remarks></remarks>
    Private Function mfAddLogin_Situ(ByVal ipconDB As SqlConnection,
                                     ByVal ipconTrn As SqlTransaction,
                                     ByVal ipstrIPAddress As String,
                                     ByVal opdatLoginDT As DateTime)

        Dim cmdLogin_Situ As SqlCommand
        Dim dstLogin_Situ As DataSet
        cmdLogin_Situ = New SqlCommand("COMLGIP001_U1", ipconDB)

        With cmdLogin_Situ
            'コマンドタイプ設定(ストアド)
            .CommandType = CommandType.StoredProcedure
            'パラメータ設定
            .Parameters.Add(pfSet_Param("sessionid", SqlDbType.VarChar, Session.SessionID))
            .Parameters.Add(pfSet_Param("userid", SqlDbType.VarChar, Me.txtUserNm.ppText))
            .Parameters.Add(pfSet_Param("ipaddress", SqlDbType.VarChar, ipstrIPAddress))
            .Parameters.Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                        '戻り値
        End With

        cmdLogin_Situ.Transaction = ipconTrn
        dstLogin_Situ = clsDataConnect.pfGet_DataSet(cmdLogin_Situ)

        If dstLogin_Situ.Tables.Count > 0 Then
            opdatLoginDT = dstLogin_Situ.Tables(0).Rows(0).Item("ログイン日時")
        End If

        'データセット破棄
        Call psDisposeDataSet(dstLogin_Situ)

        'ストアド戻り値チェック
        Return Integer.Parse(cmdLogin_Situ.Parameters("retvalue").Value.ToString)

    End Function

    ''' <summary>
    ''' ログイン状態情報削除
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ</param>
    ''' <param name="ipconTrn">トランザクション</param>
    ''' <param name="ipstrCnfAddress">起動場所</param>
    ''' <param name="ipstrIPAddress">ＩＰアドレス</param>
    ''' <returns>ＳＱＬ実行結果</returns>
    ''' <remarks></remarks>
    Private Function mfDelLogin(ByVal ipconDB As SqlConnection,
                                ByVal ipconTrn As SqlTransaction,
                                ByVal ipstrCnfAddress As String,
                                ByVal ipstrIPAddress As String) As Integer

        Dim cmdLogin_Situ As SqlCommand
        cmdLogin_Situ = New SqlCommand("ZCMPDEL002", ipconDB)

        With cmdLogin_Situ
            'コマンドタイプ設定(ストアド)
            .CommandType = CommandType.StoredProcedure
            'パラメータ設定
            .Parameters.Add(pfSet_Param("userid", SqlDbType.VarChar, Me.txtUserNm.ppText))
            If ipstrCnfAddress = P_ADD_SPC Then
                .Parameters.Add(pfSet_Param("ipaddress", SqlDbType.VarChar, ipstrIPAddress))
            End If

            .Parameters.Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                        '戻り値
        End With

        cmdLogin_Situ.Transaction = ipconTrn
        'コマンドタイプ設定(ストアド)
        cmdLogin_Situ.CommandType = CommandType.StoredProcedure
        cmdLogin_Situ.ExecuteNonQuery()

        'ストアド戻り値チェック
        Return Integer.Parse(cmdLogin_Situ.Parameters("retvalue").Value.ToString)
    End Function

    ''' <summary>
    ''' 排他制御情報削除
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ</param>
    ''' <param name="ipconTrn">トランザクション</param>
    ''' <param name="ipstrCnfAddress">起動場所</param>
    ''' <param name="ipstrIPAddress">ＩＰアドレス</param>
    ''' <returns>ＳＱＬ実行結果</returns>
    ''' <remarks></remarks>
    Private Function mfDelExclusive(ByVal ipconDB As SqlConnection,
                                    ByVal ipconTrn As SqlTransaction,
                                    ByVal ipstrCnfAddress As String,
                                    ByVal ipstrIPAddress As String) As Integer

        Dim cmdLogin_Situ As SqlCommand
        cmdLogin_Situ = New SqlCommand("ZCMPDEL003", ipconDB)

        With cmdLogin_Situ
            'コマンドタイプ設定(ストアド)
            .CommandType = CommandType.StoredProcedure
            'パラメータ設定
            .Parameters.Add(pfSet_Param("userid", SqlDbType.VarChar, Me.txtUserNm.ppText))
            If ipstrCnfAddress = P_ADD_SPC Then
                .Parameters.Add(pfSet_Param("ipaddress", SqlDbType.VarChar, ipstrIPAddress))
            End If

            .Parameters.Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                        '戻り値
        End With

        cmdLogin_Situ.Transaction = ipconTrn
        'コマンドタイプ設定(ストアド)
        cmdLogin_Situ.CommandType = CommandType.StoredProcedure
        cmdLogin_Situ.ExecuteNonQuery()

        'ストアド戻り値チェック
        Return Integer.Parse(cmdLogin_Situ.Parameters("retvalue").Value.ToString)
    End Function
#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
