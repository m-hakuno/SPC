'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　進捗管理
'*　ＰＧＭＩＤ：　COMMENP005
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作成：2013.11.22：ＸＸＸ
'*  作成：2017/10/12：伯野
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'COMMENP005-001     2017.03.28      加賀       管理者はミニ処理票リンクを使用可能に変更
'COMMENP005-002     2017/10/12      伯野       各プロシージャにデータセット破棄を記述

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
'Imports SPC.Global_asax
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports System.Data.SqlClient

#End Region

Public Class COMMENP005
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
    Const M_DISP_ID = P_FUN_COM & P_SCR_MEN & P_PAGE & "005"

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
    '工事進捗一覧
    '保守対応依頼書一覧
    'トラブル処理票一覧
    'ミニ処理表一覧
    '作業予定一覧
    'ＴＢＯＸ随時照会
    '玉単価設定情報一覧
    '設定情報差異確認
    Dim strMenDispCD() As String = {P_FUN_CNS & P_SCR_LST & P_PAGE & "004" _
                                  , P_FUN_CMP & P_SCR_LST & P_PAGE & "001" _
                                  , P_FUN_REQ & P_SCR_LST & P_PAGE & "001" _
                                  , P_FUN_BRK & P_SCR_LST & P_PAGE & "001" _
                                  , P_FUN_CNS & P_SCR_LST & P_PAGE & "007" _
                                  , P_FUN_WAT & P_SCR_INQ & P_PAGE & "001" _
                                  , P_FUN_BPI & P_SCR_LST & P_PAGE & "001" _
                                  , P_FUN_BPI & P_SCR_INQ & P_PAGE & "001" _
                                  }
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

        Dim blnFocus As Boolean
        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet

        Dim lkbControls() As LinkButton = {lkbSubMenu1 _
                                         , lkbSubMenu2 _
                                         , lkbSubMenu3 _
                                         , lkbSubMenu4 _
                                         , lkbSubMenu5 _
                                         , lkbSubMenu6 _
                                         , lkbSubMenu7 _
                                         , lkbSubMenu8 _
                                         }

        Dim strCmdDispCD As String

        objStack = New StackFrame

        If Not IsPostBack Then  '初回表示
            '画面設定
            Master.ppProgramID = M_DISP_ID
            Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)
            '            Master.ppLogout_Mode = Global_asax. ClsComVer.E_ログアウトモード.閉じる
            Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる

            '初期化
            conDB = Nothing
            strCmdDispCD = String.Empty
            blnFocus = False

            'ViewStateに「グループナンバー」を保存
            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            'リンクボタン非活性化
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

                    'パラメータ設定
                    With cmdDB.Parameters
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))
                        .Add(pfSet_Param("dispcd", SqlDbType.NVarChar, strCmdDispCD))
                    End With

                    'リストデータ取得
                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                    If dstOrders.Tables(0).Rows.Count <> 0 Then
                        '取得したデータにて遷移先設定
                        For zz As Integer = 0 To lkbControls.Length - 1
                            For Each rowData As System.Data.DataRow In dstOrders.Tables(0).Rows
                                If rowData.Item("M18_DISP_CD").ToString = strMenDispCD(zz) Then
                                    lkbControls(zz).Enabled = True
                                    If blnFocus = False Then
                                        lkbControls(zz).Focus()
                                        blnFocus = True
                                    End If
                                End If
                            Next
                        Next zz
                    Else
                        '0件
                        psMesBox(Me, "30001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                        psClose_Window(Me)
                        Return
                    End If

                Catch ex As Exception
                    'データ取得エラー
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "権限")
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    psClose_Window(Me)
                Finally
                    Call psDisposeDataSet(dstOrders)
                    'DB切断
                    If Not clsDataConnect.pfClose_Database(conDB) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                End Try
            Else
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                psClose_Window(Me)
            End If
        End If
    End Sub

    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
                'COMMENP005-001
                If Session(P_SESSION_ADMIN) = "2" Then
                    '管理者
                    lkbSubMenu3.Enabled = False
                    lkbSubMenu6.Enabled = False
                    lkbSubMenu8.Enabled = False
                Else
                    '一般
                    lkbSubMenu3.Enabled = False
                    lkbSubMenu4.Enabled = False
                    lkbSubMenu6.Enabled = False
                    lkbSubMenu8.Enabled = False
                End If

            Case "NGC"
        End Select

    End Sub

    Protected Sub lkbSubMenu1_Click(sender As Object, e As EventArgs) Handles lkbSubMenu1.Click
        Dim strPath As String = "~/" & P_CNS & "/" & strMenDispCD(0) & "/" & strMenDispCD(0) & ".aspx"

        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_BCLIST) = Master.ppTitle
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

        Response.Redirect(strPath)

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    Protected Sub lkbSubMenu2_Click(sender As Object, e As EventArgs) Handles lkbSubMenu2.Click
        Dim strPath As String = "~/" & P_MAI & "/" & strMenDispCD(1) & "/" & strMenDispCD(1) & ".aspx"

        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_BCLIST) = Master.ppTitle
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

        Response.Redirect(strPath)

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    Protected Sub lkbSubMenu3_Click(sender As Object, e As EventArgs) Handles lkbSubMenu3.Click
        Dim strPath As String = "~/" & P_MAI & "/" & strMenDispCD(2) & "/" & strMenDispCD(2) & ".aspx"

        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_BCLIST) = Master.ppTitle
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

        Response.Redirect(strPath)

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    Protected Sub lkbSubMenu4_Click(sender As Object, e As EventArgs) Handles lkbSubMenu4.Click
        Dim strPath As String = "~/" & P_FLR & "/" & strMenDispCD(3) & "/" & strMenDispCD(3) & ".aspx"

        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_BCLIST) = Master.ppTitle
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

        Response.Redirect(strPath)

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    Protected Sub lkbSubMenu5_Click(sender As Object, e As EventArgs) Handles lkbSubMenu5.Click
        Dim strPath As String = "~/" & P_CNS & "/" & strMenDispCD(4) & "/" & strMenDispCD(4) & ".aspx"

        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_BCLIST) = Master.ppTitle
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

        Response.Redirect(strPath)

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    Protected Sub lkbSubMenu6_Click(sender As Object, e As EventArgs) Handles lkbSubMenu6.Click
        Dim strPath As String = "~/" & P_WAT & "/" & strMenDispCD(5) & "/" & strMenDispCD(5) & ".aspx"

        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_BCLIST) = Master.ppTitle
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

        Response.Redirect(strPath)

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    Protected Sub lkbSubMenu7_Click(sender As Object, e As EventArgs) Handles lkbSubMenu7.Click
        Dim strPath As String = "~/" & P_MAI & "/" & strMenDispCD(6) & "/" & strMenDispCD(6) & ".aspx"

        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_BCLIST) = Master.ppTitle
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

        Response.Redirect(strPath)

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    Protected Sub lkbSubMenu8_Click(sender As Object, e As EventArgs) Handles lkbSubMenu8.Click
        Dim strPath As String = "~/" & P_MAI & "/" & strMenDispCD(7) & "/" & strMenDispCD(7) & ".aspx"

        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_BCLIST) = Master.ppTitle
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

        Response.Redirect(strPath)

        'ログ出力終了
        psLogEnd(Me)

    End Sub

#End Region

#Region "そのほかのプロシージャ"
#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
