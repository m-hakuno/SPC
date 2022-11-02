'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　マスタ管理メニュー
'*　ＰＧＭＩＤ：　COMMENP009
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作成：2015.02.03：星野
'*  変更：2017/10/12：伯野
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'COMMENP009-001     2015/07/07      稲葉       TBOX機種マスタ、TBOXマスタ、型式マスタのリンク追加
'COMMENP009-002     2015/07/07      稲葉       営業所の管理者ユーザー用に各種マスタ画面のリンクを活性化する制御追加
'COMMENP009-003     2016/08/25      栗原       リンク追加、レイアウトの変更
'COMMENP009-004     2017/06/27      伯野       リンク追加、保守機ＴＢＯＸマスタ
'COMMENP009-005     2017/10/12      伯野       各プロシージャにデータセット破棄を記述

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports System.Data.SqlClient

#End Region

Public Class COMMENP009
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
    Const M_DISP_ID = P_FUN_COM & P_SCR_MEN & P_PAGE & "009"
    Const M_LOGIN = "~/" & P_COM & "/" & P_FUN_COM & P_SCR_LGI & P_PAGE & "001/" & P_FUN_COM & P_SCR_LGI & P_PAGE & "001.aspx"
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
    Private mstrPrm(2) As String
    Dim objStack As StackFrame
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

        Dim blnFocus As Boolean = False
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim dstAuth As New DataSet
        Dim strMenDispCD() As String = Nothing
        Dim strCmdDispCD As String = String.Empty
        Dim strAdd As String = Nothing
        Dim lkbControls() As LinkButton = Nothing
        objStack = New StackFrame

        'ログアウト設定
        Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる

        '接続場所取得
        strAdd = ConfigurationManager.AppSettings("Address")

        If Not IsPostBack Then  '初回表示
            '画面設定
            Master.ppProgramID = M_DISP_ID
            Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

            'ユーザーマスタ
            '社員マスタ
            '他社マスタ
            '機器種別マスタ
            '機器マスタ
            '保守料金マスタ
            '特殊店舗マスタ
            '事象マスタ
            strMenDispCD = {P_FUN_COM & P_SCR_UPD & P_PAGE_M & "01" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "02" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "02" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "06" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "07" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "40" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "23" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "03" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "66" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "31" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "54" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "77" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "75" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "53" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "73" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "90" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "87" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "47" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "95" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "92" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "48" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "45" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "06" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "09" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "80" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "89" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "68" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "A1" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "44" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "41" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "07" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "69" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "70" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "71" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "96" _
                          , P_FUN_REP & P_SCR_UPD & P_PAGE & "001" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "36" _
                          , P_FUN_COM & P_SCR_UPD & P_PAGE_M & "B1" _
                           }

            lkbControls = {lkbMenu1 _
                          , lkbMenu2 _
                          , lkbMenu3 _
                          , lkbMenu6 _
                          , lkbMenu7 _
                          , lkbMenu12 _
                          , lkbMenu13 _
                          , lkbMenu14 _
                          , lkbMenu15 _
                          , lkbMenu16 _
                          , lkbMenu17 _
                          , lkbMenu18 _
                          , lkbMenu19 _
                          , lkbMenu20 _
                          , lkbMenu21 _
                          , lkbMenu22 _
                          , lkbMenu23 _
                          , lkbMenu24 _
                          , lkbMenu25 _
                          , lkbMenu26 _
                          , lkbMenu27 _
                          , lkbMenu28 _
                          , lkbMenu29 _
                          , lkbMenu30 _
                          , lkbMenu31 _
                          , lkbMenu33 _
                          , lkbMenu34 _
                          , lkbMenu35 _
                          , lkbMenu36 _
                          , lkbMenu37 _
                          , lkbMenu38 _
                          , lkbMenu40 _
                          , lkbMenu41 _
                          , lkbMenu42 _
                          , lkbMenu43 _
                          , lkbMenu44 _
                          , lkbMenu45 _
                          , lkbMenuB1 _
                          }

            For Each lkbCont As LinkButton In lkbControls
                lkbCont.Enabled = False
            Next

            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    cmdDB = New SqlCommand("ZCMPSEL008", conDB)

                    '画面ＩＤのコマンドパラメータとリンクパス作成
                    For zz As Integer = 0 To (strMenDispCD.Length - 1)
                        If strCmdDispCD = String.Empty Then
                            strCmdDispCD &= strMenDispCD(zz)
                        Else
                            strCmdDispCD &= "," & strMenDispCD(zz)
                        End If
                    Next

                    With cmdDB.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))
                        .Add(pfSet_Param("dispcd", SqlDbType.NVarChar, strCmdDispCD))
                    End With

                    'リストデータ取得
                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                    If dstOrders.Tables(0).Rows.Count = 0 Then
                        '0件
                        '認証クリア(セッション終了時にＤＢクリア)
                        FormsAuthentication.SignOut()
                        Session.Abandon()

                        psMesBox(Me, "30001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                        psNext_Page(Me, M_LOGIN, ClsComVer.E_S実行.描画前)
                    End If

                    '取得したデータにて遷移先設定
                    For zz As Integer = 0 To strMenDispCD.Length - 1
                        For Each rowData As System.Data.DataRow In dstOrders.Tables(0).Rows
                            If rowData.Item("M18_DISP_CD").ToString = strMenDispCD(zz) Then
                                lkbControls(zz).Enabled = True
                                If blnFocus = False Then
                                    lkbControls(zz).Focus()
                                    blnFocus = True
                                End If
                            End If
                        Next
                    Next


                    'COMMENP009-002
                    'ユーザーの権限を取得。ViewStateに保管
                    cmdDB = New SqlCommand("ZCMPSEL056", conDB)
                    With cmdDB.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("user_id", SqlDbType.NVarChar, User.Identity.Name))
                    End With
                    dstAuth = clsDataConnect.pfGet_DataSet(cmdDB)
                    ViewState("user_auth") = dstAuth.Tables(0).Rows(0).Item("M02_AUTH_CLS").ToString
                    'COMMENP009-002 END

                Catch ex As Exception
                    '認証クリア
                    FormsAuthentication.SignOut()
                    Session.Abandon()
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "権限")
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    psNext_Page(Me, M_LOGIN, ClsComVer.E_S実行.描画前)
                Finally
                    'データセット破棄
                    Call psDisposeDataSet(dstOrders)
                    Call psDisposeDataSet(dstAuth)
                    'DB切断
                    If Not clsDataConnect.pfClose_Database(conDB) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    End If
                End Try
            Else
                '認証クリア
                FormsAuthentication.SignOut()
                Session.Abandon()
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                psNext_Page(Me, M_LOGIN, ClsComVer.E_S実行.描画前)
            End If

        End If
    End Sub

    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
                'lkbMenu35.Enabled = True
            Case Else
                'COMMENP009-002
                If ViewState("user_auth") = "3" Or ViewState("user_auth") = "6" Then
                    lkbMenu35.Enabled = True
                    Panel1.Enabled = True
                Else
                    lkbMenu35.Enabled = False
                    Panel1.Enabled = False
                End If
                'Panel1.Enabled = False
                'COMMENP009-002 END

        End Select

    End Sub

#Region "マスタ"
    ''' <summary>
    ''' ユーザーマスタ遷移
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu1_Click(sender As Object, e As EventArgs) Handles lkbMenu1.Click
        'ユーザーマスタ
        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "01"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 社員マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu2_Click(sender As Object, e As EventArgs) Handles lkbMenu2.Click
        '社員マスタ
        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "02"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx?page=1"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 他社マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu3_Click(sender As Object, e As EventArgs) Handles lkbMenu3.Click
        '他社マスタ
        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "02"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx?page=2"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 業者基本マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu35_Click(sender As Object, e As EventArgs) Handles lkbMenu35.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "A1"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 業者マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu36_Click(sender As Object, e As EventArgs) Handles lkbMenu36.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "44"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' ＴＢＯＸタイプマスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu13_Click(sender As Object, e As EventArgs) Handles lkbMenu13.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "23"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' ＴＢＯＸバージョンマスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu14_Click(sender As Object, e As EventArgs) Handles lkbMenu14.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "03"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 機器分類マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu21_Click(sender As Object, e As EventArgs) Handles lkbMenu21.Click
        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "73"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 機器種別マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu29_Click(sender As Object, e As EventArgs) Handles lkbMenu29.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "06"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 電話番号マスタ
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub lkbMenu47_Click(sender As Object, e As EventArgs) Handles lkbMenu47.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "19"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 電話番号別文言マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu16_Click(sender As Object, e As EventArgs) Handles lkbMenu16.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "31"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 出精値引マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu19_Click(sender As Object, e As EventArgs) Handles lkbMenu19.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "75"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 休日マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu45_Click(sender As Object, e As EventArgs) Handles lkbMenu45.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "36"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

#End Region

#Region "工事"
    ''' <summary>
    ''' 工事区分マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu26_Click(sender As Object, e As EventArgs) Handles lkbMenu26.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "92"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 工事名マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu27_Click(sender As Object, e As EventArgs) Handles lkbMenu27.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "48"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 機器マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu38_Click(sender As Object, e As EventArgs) Handles lkbMenu38.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "07"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 型式マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu15_Click(sender As Object, e As EventArgs) Handles lkbMenu15.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "66"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' MDN機種マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu23_Click(sender As Object, e As EventArgs) Handles lkbMenu23.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "87"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 梱包材マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu24_Click(sender As Object, e As EventArgs) Handles lkbMenu24.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "47"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 移動理由マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu22_Click(sender As Object, e As EventArgs) Handles lkbMenu22.Click
        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "90"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' シリアル特殊条件マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu46_Click(sender As Object, e As EventArgs) Handles lkbMenu46.Click
        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "97"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub


#End Region

#Region "監視"

    ''' <summary>
    ''' ヘルスチェックエラーマスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu20_Click(sender As Object, e As EventArgs) Handles lkbMenu20.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "53"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 表示項目マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu28_Click(sender As Object, e As EventArgs) Handles lkbMenu28.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "45"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 未集信マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu39_Click(sender As Object, e As EventArgs) Handles lkbMenu39.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "85"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' DLL設定変更依頼内容マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenuA7_Click(sender As Object, e As EventArgs) Handles lkbMenuA7.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "A7"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

#End Region

#Region "保守"
    ''' <summary>
    ''' 保守料金マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu6_Click(sender As Object, e As EventArgs) Handles lkbMenu6.Click
        '保守料金マスタ
        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "37"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub
    ''' <summary>
    ''' 特別保守料金マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu43_Click(sender As Object, e As EventArgs) Handles lkbMenu43.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "96"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub
    ''' <summary>
    ''' 保守担当者マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu37_Click(sender As Object, e As EventArgs) Handles lkbMenu37.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "41"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub
    ''' <summary>
    ''' 特殊店舗マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu7_Click(sender As Object, e As EventArgs) Handles lkbMenu7.Click
        '特殊店舗マスタ
        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "38"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub
    ''' <summary>
    ''' 持参物品セットマスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu25_Click(sender As Object, e As EventArgs) Handles lkbMenu25.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "95"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 持参物品選択マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu18_Click(sender As Object, e As EventArgs) Handles lkbMenu18.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "77"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 倉庫マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu12_Click(sender As Object, e As EventArgs) Handles lkbMenu12.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "40"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 修理・有償部品費用マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu44_Click(sender As Object, e As EventArgs) Handles lkbMenu44.Click

        Dim strDispID As String = P_FUN_REP & P_SCR_UPD & P_PAGE & "001"
        Dim strPath As String = "~/" & P_RPE & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 持参物品マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu17_Click(sender As Object, e As EventArgs) Handles lkbMenu17.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "54"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 回復内容マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu31_Click(sender As Object, e As EventArgs) Handles lkbMenu31.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "80"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 作業内容マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu33_Click(sender As Object, e As EventArgs) Handles lkbMenu33.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "89"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 事象マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>2015/12/11 リリース中止</remarks>
    Protected Sub lkbMenu34_Click(sender As Object, e As EventArgs) Handles lkbMenu34.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "68"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 申告元マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu40_Click(sender As Object, e As EventArgs) Handles lkbMenu40.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "69"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 申告内容マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu41_Click(sender As Object, e As EventArgs) Handles lkbMenu41.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "70"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 処置内容マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbMenu42_Click(sender As Object, e As EventArgs) Handles lkbMenu42.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "71"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 保守機ＴＢＯＸマスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lkbMenuB1_Click(sender As Object, e As EventArgs) Handles lkbMenuB1.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "B1"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)

    End Sub

#End Region

    ''' <summary>
    ''' 工事区分別使用機器マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>2015/12/11 リリース中止</remarks>
    Protected Sub lkbMenu30_Click(sender As Object, e As EventArgs) Handles lkbMenu30.Click

        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "09"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

#End Region

#Region "そのほかのプロシージャ"
    ''' <summary>
    ''' 画面遷移時の共通セッション設定
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub msSet_Session()
        Dim intGroupNum As Integer

        'キー、遷移条件、パンくずリスト、排他登録年月日クリア
        Session(P_KEY) = Nothing
        Session(P_SESSION_TERMS) = Nothing
        Session(P_SESSION_BCLIST) = Nothing
        Session(P_SESSION_EXCLUSIV_DATE) = Nothing
        'グループ番号
        clsExc.pfGet_GroupNum(intGroupNum, Me)
        Session(P_SESSION_GROUP_NUM) = intGroupNum
    End Sub

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
