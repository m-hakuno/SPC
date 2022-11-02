'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　ＣＴＩ情報
'*　ＰＧＭＩＤ：　CTISELP019
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.04.8　：　中川
'*  更　新　　：　2016.12.07 ：　伯野
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'CTISELP019-001     2016/08/10      栗原      電話番号に紐付くホール情報が複数あった際、ホール参照画面へ遷移するよう変更
'CTISELP019-002     2016/12/07      伯野　　　自動返信機能対応　電話番号に紐付くスケジュールを更新する

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax

#End Region

Public Class CTISELP019

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
    Const M_MY_DISP_ID As String = P_FUN_CTI & P_SCR_SEL & P_PAGE & "019"           '自画面ＩＤ
    Const M_COMMENP001_PATH As String = "~/Common/COMMENP001/COMMENP001.aspx"       'メインメニューのパス
    Const M_COMMENP006_PATH As String = "~/Common/COMMENP006/COMMENP006.aspx"       'ホールマスタ管理のパス
    Const M_CTISELP005_PATH As String = "~/Maintenance/CTISELP005/CTISELP005.aspx"  'ＣＴＩ情報（作業者）のパス
    'CTISELP019-001
    Const M_COMSELP001_PATH As String = "~/Common/COMSELP001/COMSELP001.aspx"       'ホール参照のパス
    'CTISELP019-001 END

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

    ''' <summary>
    ''' Page_Loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            If Not IsPostBack Then  '初回表示

                Dim strTel As String = Nothing
                'CTISELP019-002
                '                Dim strPC_Name As String = Nothing
                Dim strCliantIP As String = Nothing
                'CTISELP019-002

                '画面情報設定
                Master.ppProgramID = M_MY_DISP_ID
                Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)

                'セッション設定
                Session(P_SESSION_BCLIST) = "ＣＴＩ情報"
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照

                'URL変数からＣＴＩ電話番号を取得
                strTel = Request.QueryString.Get("TEL").ToString
                'CTISELP019-002
                '                strPC_Name = Request.QueryString.Get("PCNAME").ToString
                '                strExte_No = Request.QueryString.Get("EXTENSION_NO").ToString
                '                strCliantIP = Request.ServerVariables("REMOTE_ADDR")
                strCliantIP = Server.HtmlEncode(Request.UserHostAddress)

                'CTISELP019-002
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", "電話番号=" + strTel + "　ＩＰアドレス=" + strCliantIP, "")

                Select Case strTel

                    Case Nothing, "非通知", "公衆電話", "表示圏外", "受信エラー", "相手先不明"

                        '電話番号以外の場合

                        If strTel Is Nothing Or strTel = String.Empty Then
                            strTel = "相手先不明"
                        End If

                        psMesBox(Me, "20008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, strTel)
                        psClose_Window(Me)
                        Exit Sub

                    Case Else

                        ViewState("電話番号") = strTel
                        'CTISELP019-002
                        'スケジュール情報を更新
                        '                        ViewState("ＰＣ名") = strPC_Name
                        ViewState("着信端末") = strCliantIP
                        ' Web.config 内に設定した値を取得する
                        '                        Dim strObjPC_NAME As String = ConfigurationManager.AppSettings("PCNAME") 'ＰＣ名
                        Dim strObjCliantIP As String = ConfigurationManager.AppSettings("ReceiptIP") '内線番号
                        '                        ViewState("対象ＰＣ名") = strObjPC_NAME
                        ViewState("対象端末") = strObjCliantIP
                        '                        If strPC_Name = strObjPC_NAME And strExte_No = strObjExtension_NO Then
                        If strCliantIP = strObjCliantIP Then
                            '                            Call sschedule_Update(strPC_Name, strExte_No, strTel)
                            Call sschedule_Update(strCliantIP, strTel)
                        End If
                        'CTISELP019-002

                End Select

                'ログイン判断処理
                If mfCheck_Login() = False Then

                    If ViewState("ログイン") = "0" Then

                        'ログアウト（セッション削除）
                        FormsAuthentication.SignOut()
                        Session.Abandon()

                    End If
                    psClose_Window(Me)
                    Exit Sub

                End If

                '遷移画面判断処理
                If mfCheck_Gamen() = False Then

                    If ViewState("ログイン") = "0" Then

                        'ログアウト（セッション削除）
                        FormsAuthentication.SignOut()
                        Session.Abandon()

                    End If
                    psClose_Window(Me)
                    Exit Sub

                End If

                '画面遷移処理
                Call msTrans_Gamen()

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
        End Try

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' ログイン判断処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfCheck_Login() As Boolean

        Dim strIPAddress As String = Nothing
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'IPアドレス取得
        strIPAddress = Request.ServerVariables("REMOTE_ADDR")

        Try
            'DB接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then

                '失敗
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                psClose_Window(Me)
                Return False

            Else
                'ログイン状態取得
                cmdDB = New SqlCommand(M_MY_DISP_ID & "_S1", conDB)

                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("session_id", SqlDbType.NVarChar, Session.SessionID))   'ＩＰアドレス
                End With

                '結果取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)



                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                objStack.GetMethod.Name, "", "ログイン判断処理", "Catch")



                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '件数が0の場合
                    ViewState(P_SESSION_IP) = strIPAddress
                    ViewState("ログイン") = "0"



                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                    objStack.GetMethod.Name, "", "ログイン処理前", "Catch")




                    'ログイン処理
                    If mfLogin() = False Then
                        Return False
                    End If

                Else
                    '0件以外の場合
                    ViewState("ログイン") = "1"     'ログイン済み

                End If

            End If

            Return True

        Catch ex As Exception
            '検索失敗
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ログイン状態")
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            psClose_Window(Me)
            Return False

        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                psClose_Window(Me)
                mfCheck_Login = False
            End If

        End Try

    End Function

    ''' <summary>
    ''' ログイン処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfLogin() As Boolean

        Dim strUserId As String = Nothing
        Dim datLoginDate As DateTime = Nothing
        Dim strIPAddress As String = Nothing
        Dim strReturn As String = Nothing
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'セッションＩＤ取得
        Session(P_SESSION_SESSTION_ID) = Session.SessionID
        ViewState(P_SESSION_SESSTION_ID) = Session(P_SESSION_SESSTION_ID)
        'ＩＰアドレス取得
        strIPAddress = ViewState(P_SESSION_IP)

        Try
            'DB接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then

                '失敗
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                psClose_Window(Me)
                Return False

            Else
                'ＣＴＩ用ユーザー情報取得
                cmdDB = New SqlCommand(M_MY_DISP_ID & "_S2", conDB)

                With cmdDB.Parameters
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))   '戻り値
                End With

                '結果取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                strReturn = cmdDB.Parameters("retvalue").Value.ToString

                If strReturn <> "0" Then
                    '検索失敗
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ログイン状態")
                    psClose_Window(Me)
                    Return False
                End If

                strUserId = dstOrders.Tables(0).Rows(0).Item("ユーザーＩＤ")

                'ログイン状態を登録
                Using trans = conDB.BeginTransaction
                    cmdDB = New SqlCommand(M_MY_DISP_ID & "_I1", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure

                    '現在日時を取得
                    datLoginDate = DateTime.Now

                    'パラメータ設定
                    With cmdDB.Parameters
                        .Add(pfSet_Param("session_id", SqlDbType.NVarChar, ViewState(P_SESSION_SESSTION_ID)))   'セッションＩＤ
                        .Add(pfSet_Param("user_id", SqlDbType.NVarChar, strUserId))                             'ユーザーＩＤ
                        .Add(pfSet_Param("ip_address", SqlDbType.NVarChar, ViewState(P_SESSION_IP)))            'ＩＰアドレス
                        .Add(pfSet_Param("login_date", SqlDbType.NVarChar, datLoginDate))                       'ログイン日時
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                       '戻り値
                    End With

                    cmdDB.Transaction = trans
                    cmdDB.ExecuteNonQuery()

                    '戻り値取得
                    strReturn = cmdDB.Parameters("retvalue").Value.ToString

                    If strReturn <> "0" Then
                        '登録失敗
                        psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ログイン状態")
                        psClose_Window(Me)
                        Return False
                    End If

                    'コミット
                    trans.Commit()
                End Using

            End If

            'セッション設定
            Session(P_SESSION_USERID) = strUserId                               'ユーザーＩＤ
            Session(P_SESSION_LOGIN_DATE) = datLoginDate                        'ログイン日時
            Session(P_SESSION_IP) = strIPAddress                                'ＩＰアドレス
            Session(P_SESSION_PLACE) = "SPC"                                    '場所
            Session(P_SESSION_SESSTION_ID) = ViewState(P_SESSION_SESSTION_ID)   'セッションＩＤ
            Session(P_SESSION_AUTH) = "SPC"                                     '権限区分

            'ログイン
            FormsAuthentication.SetAuthCookie(strUserId, False)

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
                psClose_Window(Me)
                mfLogin = False
            End If

        End Try

    End Function

    ''' <summary>
    ''' 遷移画面判断処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfCheck_Gamen() As Boolean

        Dim strKeyList As New List(Of String)
        Dim strReturn As String = Nothing
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'DB接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then

                '失敗
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                psClose_Window(Me)
                Return False

            Else
                cmdDB = New SqlCommand(M_MY_DISP_ID & "_S3", conDB)

                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("tel_no", SqlDbType.NVarChar, ViewState("電話番号")))
                End With

                '結果取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '件数が0件の場合
                    ViewState("マスタ情報") = "0"

                Else
                    'CTISELP019-001
                    'ホールかつホール情報の重複有
                    If dstOrders.Tables(0).Rows(0).Item("判定区分") = "0" AndAlso dstOrders.Tables(1).Rows.Count > 1 Then
                        'ホール情報が2件以上の場合
                        '1件の場合
                        With strKeyList
                            .Add(ViewState("電話番号"))
                        End With

                        ViewState(P_KEY) = strKeyList.ToArray
                        ViewState("マスタ情報") = "2"
                    Else
                        '1件の場合
                        With strKeyList
                            .Add(dstOrders.Tables(0).Rows(0).Item("電話番号"))
                            .Add(dstOrders.Tables(0).Rows(0).Item("会社コード"))
                            .Add(dstOrders.Tables(0).Rows(0).Item("ホールコード"))
                            .Add(dstOrders.Tables(0).Rows(0).Item("社員コード"))
                            .Add(dstOrders.Tables(0).Rows(0).Item("判定区分"))
                            .Add(dstOrders.Tables(0).Rows(0).Item("ＴＢＯＸＩＤ"))
                            .Add(dstOrders.Tables(0).Rows(0).Item("ＮＬ区分"))
                            .Add(dstOrders.Tables(0).Rows(0).Item("システム分類"))
                        End With

                        ViewState(P_KEY) = strKeyList.ToArray
                        ViewState("マスタ情報") = "1"

                    End If
                    'CTISELP019-001 END
                End If

            End If

            Return True

        Catch ex As Exception
            '検索失敗
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "電話番号マスタ")
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            psClose_Window(Me)
            Return False

        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                psClose_Window(Me)
                mfCheck_Gamen = False
            End If

        End Try

    End Function

    ''' <summary>
    ''' 画面遷移処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msTrans_Gamen()

        Dim strKeyList() As String = Nothing
        Dim strSessionKey As New List(Of String)
        Dim strTelNo As String = Nothing
        Dim strMstrInf As String = Nothing
        Dim strLogin As String = Nothing

        'ビューステートから情報取得
        strTelNo = ViewState("電話番号")
        strMstrInf = ViewState("マスタ情報")
        strLogin = ViewState("ログイン")

        'CTISELP019-002
        '        Dim strPC_Name As String = ""
        Dim strCliantIP As String = ""
        '        strPC_Name = ViewState("ＰＣ名") '受信ＰＣ名
        strCliantIP = ViewState("着信端末") '受信内線番号
        '        Dim strObjPC_NAME As String = ConfigurationManager.AppSettings("PCNAME") '専用ＰＣ名
        Dim strObjCliantIP As String = ConfigurationManager.AppSettings("ReceiptIP") '専用内線番号
        'CTISELP019-002

        If strMstrInf = "0" Then

            'CTISELP019-002
            'マスタ情報がない場合
            'マスタ情報がなくても受信専用端末の場合はメッセージを出さない
            '            If strPC_Name = strObjPC_NAME And strExte_No = strObjExtension_NO Then
            If strCliantIP = strObjCliantIP Then
            Else
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前)
            End If
            '            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前)
            'CTISELP019-002
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
                            objStack.GetMethod.Name, M_COMMENP001_PATH, strPrm, "TRANS")
            '■□■□結合試験時のみ使用予定□■□■
            '--------------------------------
            '2014/04/16 星野　ここまで
            '--------------------------------
            ''未ログインだった場合、メインメニューを表示
            'If strLogin = "0" Then
            '    msOpen_Window(Me, M_COMMENP001_PATH)
            'End If
            psClose_Window(Me)
            Exit Sub

            'CTISELP019-001　ホール情報の重複有
        ElseIf strMstrInf = "2" Then
            'キー情報を取得
            strKeyList = ViewState(P_KEY)
            'キー情報設定
            With strSessionKey
                .Add(strKeyList(0))     '電話番号
            End With

            'セッション設定
            Session(P_KEY) = strSessionKey.ToArray
            Session("遷移元") = M_MY_DISP_ID

            'ホールマスタ参照画面に遷移
            'CTISELP019-002
            If strCliantIP = strObjCliantIP Then
            Else
                msOpen_Window(Me, M_COMSELP001_PATH)
            End If
            '            msOpen_Window(Me, M_COMSELP001_PATH)
            'CTISELP019-002

            '自画面を終了する
            psClose_Window(Me)
            'CTISELP019-001 END

        Else
            'マスタ情報が1の場合

            If strLogin = "0" Then
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
                                objStack.GetMethod.Name, M_COMMENP001_PATH, strPrm, "TRANS")
                '■□■□結合試験時のみ使用予定□■□■
                '--------------------------------
                '2014/04/16 星野　ここまで
                '--------------------------------
                ''未ログインだった場合、メインメニューを表示
                'msOpen_Window(Me, M_COMMENP001_PATH)

            End If

            'キー情報を取得
            strKeyList = ViewState(P_KEY)

            Select Case strKeyList(4)

                Case "0"    'ホール

                    'キー情報設定
                    With strSessionKey
                        .Add(strKeyList(5))     'ＴＢＯＸＩＤ
                        .Add(strKeyList(6))     'ＮＬ区分
                        .Add(strKeyList(7))     'システム分類
                        .Add(strKeyList(2))     'ホールコード
                    End With

                    'セッション設定
                    Session(P_KEY) = strSessionKey.ToArray

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
                                    objStack.GetMethod.Name, M_COMMENP006_PATH, strPrm, "TRANS")
                    '■□■□結合試験時のみ使用予定□■□■
                    '--------------------------------
                    '2014/04/16 星野　ここまで
                    '--------------------------------
                    'ホールマスタ管理画面に遷移
                    'CTISELP019-002
                    If strCliantIP = strObjCliantIP Then
                    Else
                        msOpen_Window(Me, M_COMMENP006_PATH)
                    End If
'                    msOpen_Window(Me, M_COMMENP006_PATH)
'CTISELP019-002

                Case "1", "2"   '1:社員　2:保守担当

                    'キー情報設定
                    With strSessionKey
                        .Add(strTelNo)          '電話番号
                        .Add(strKeyList(1))     '会社コード
                        .Add(strKeyList(2))     'ホールコード
                        .Add(strKeyList(3))     '社員コード
                        .Add(strKeyList(4))     '判定区分
                    End With

                    'セッション設定
                    Session(P_KEY) = strSessionKey.ToArray

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
                                    objStack.GetMethod.Name, M_CTISELP005_PATH, strPrm, "TRANS")
                    '■□■□結合試験時のみ使用予定□■□■
                    '--------------------------------
                    '2014/04/16 星野　ここまで
                    '--------------------------------
                    'ＣＴＩ情報（作業者）に遷移
                    'CTISELP019-002
                    If strCliantIP = strObjCliantIP Then
                    Else
                        msOpen_Window(Me, M_CTISELP005_PATH)
                    End If
                    '                    msOpen_Window(Me, M_CTISELP005_PATH)
                    'CTISELP019-002

            End Select

            '自画面を終了する
            psClose_Window(Me)

        End If

    End Sub

    'CTISELP019-002
    '    Private Sub sschedule_Update(ByVal ipstrPC_Name As String, ByVal ipstrExte_No As String, ByVal ipstrTELNO As String)
    '    ''' <param name="ipstrPC_Name">ＰＣ名</param>
    ''' <summary>
    ''' 自動返信機能用スケジュール情報更新
    ''' </summary>
    ''' <param name="ipstrCliantIP">着信端末ＩＰ</param>
    ''' <param name="ipstrTELNO">電話番号</param>
    ''' <remarks></remarks>
    Private Sub sschedule_Update(ByVal ipstrCliantIP As String, ByVal ipstrTELNO As String)

        Dim strKeyList As New List(Of String)
        Dim strReturn As String = Nothing
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet

        Try

            'DB接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then

                '失敗
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                '                psClose_Window(Me)

            Else
                'ログイン状態取得


                '結果取得
                'ログイン状態を登録
                Using trans = conDB.BeginTransaction
                    cmdDB = New SqlCommand(M_MY_DISP_ID & "_U1", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure

                    'パラメータ設定
                    With cmdDB.Parameters
                        '                        .Add(pfSet_Param("PC_NAME", SqlDbType.NVarChar, ipstrPC_Name))   'ＰＣ名
                        '                        .Add(pfSet_Param("PC_NAME", SqlDbType.NVarChar, ""))   'ＰＣ名
                        '                        .Add(pfSet_Param("EXTE_NO", SqlDbType.NVarChar, ipstrCliantIP))   '着信端末ＩＰ
                        .Add(pfSet_Param("TEL_NO", SqlDbType.NVarChar, ipstrTELNO))   '電話番号
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))   '戻り値
                    End With

                    cmdDB.Transaction = trans
                    cmdDB.ExecuteNonQuery()

                    '戻り値取得
                    strReturn = cmdDB.Parameters("retvalue").Value.ToString

                    If strReturn <> "0" Then
                        '登録失敗
                        trans.Rollback()
                        psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "スケジュール更新")
                        '                        psClose_Window(Me)
                    End If

                    'コミット
                    trans.Commit()
                End Using

            End If

        Catch ex As Exception
            '検索失敗
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "スケジュール更新")
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            '            psClose_Window(Me)

        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                '                psClose_Window(Me)
            End If
        End Try

    End Sub
    'CTISELP019-002

#End Region

#Region "終了処理プロシージャ"

    ''' <summary>
    ''' 新規ウィンドウで開く
    ''' </summary>
    ''' <param name="ippagPage">開き元のページコントロール</param>
    ''' <param name="ipstrPath">開き先の仮想パス</param>
    ''' <remarks></remarks>
    Public Shared Sub msOpen_Window(ByVal ippagPage As Page, ByVal ipstrPath As String)
        Dim strKey As String = "subwindow"
        Dim strScript As String
        Dim intKeyNo = 0

        '使用していないスクリプトのキー設定
        Do Until Not ippagPage.ClientScript.IsClientScriptBlockRegistered(ippagPage.GetType, strKey)
            strKey = "subwindow" & intKeyNo.ToString
            intKeyNo = intKeyNo + 1
        Loop

        'strKey = pfGet_ScriptKey(ippagPage, ippagPage.GetType, "subwindow")
        strScript = "window.open('" & VirtualPathUtility.ToAbsolute(ipstrPath) & "', '_blank'," _
            & "'dependent=yes, resizable=yes, scrollbars=yes, menubar=yes, width=1065px, height=600px');"
        ippagPage.ClientScript.RegisterClientScriptBlock(ippagPage.GetType, strKey, strScript, True)
    End Sub

#End Region

End Class
