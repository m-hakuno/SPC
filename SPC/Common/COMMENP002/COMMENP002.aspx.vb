'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　工事管理
'*　ＰＧＭＩＤ：　COMMENP002
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.10.25　：　土岐
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'COMMENP002-001     2015/07/06      稲葉       工事連絡票一覧、構成配信/結果参照へのリンク追加
'COMMENP002-002     2017/10/12      伯野       各プロシージャにデータセット破棄を記述
'COMMENP002-003     2018/04/16      小野       設置環境写真一覧へのリンクを追加


#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
'Imports SPC.Global_asax
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports System.Data.SqlClient

#End Region

Public Class COMMENP002
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
        Const M_DISP_ID = P_FUN_COM & P_SCR_MEN & P_PAGE & "002"


        Dim blnFocus As Boolean
        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet
        '工事受付一覧
        '請求資料一覧
        '作業予定一覧
        '物品転送一覧
        '工事料金明細書一覧
        '工事連絡票一覧
        'シリアル登録
        'シリアル情報一覧
        '随時集信一覧状況入力
        '業者情報
        '社員情報(非活性固定)
        '機器参照
        '設置環境写真一覧---COMMENP002-003
        Dim strMenDispCD() As String = {P_FUN_CNS & P_SCR_LST & P_PAGE & "001",
                                        P_FUN_CNS & P_SCR_UPD & P_PAGE & "005",
                                        P_FUN_CNS & P_SCR_LST & P_PAGE & "007",
                                        P_FUN_CNS & P_SCR_LST & P_PAGE & "002",
                                        P_FUN_CNS & P_SCR_LST & P_PAGE & "006",
                                        P_FUN_SER & P_SCR_UPD & P_PAGE & "001",
                                        P_FUN_SER & P_SCR_LST & P_PAGE & "001",
                                        P_FUN_DLC & P_SCR_LST & P_PAGE & "001",
                                        P_FUN_TBP & P_SCR_INP & P_PAGE & "001",
                                        P_FUN_COM & P_SCR_SEL & P_PAGE & "002",
                                        P_FUN_CTI & P_SCR_SEL & P_PAGE & "005",
                                        P_FUN_COM & P_SCR_SEL & P_PAGE & "004",
                                        P_FUN_COM & P_SCR_LST & P_PAGE & "099",
                                        P_FUN_CNS & P_SCR_LST & P_PAGE & "005",
                                        P_FUN_DLL & P_SCR_SEL & P_PAGE & "001",
                                        P_FUN_COM & P_SCR_LST & P_PAGE & "001"}
        Dim lkbControls() As LinkButton = {lkbMenu1,
                                           lkbMenu2,
                                           lkbMenu3,
                                           lkbMenu4,
                                           lkbMenu7,
                                           lkbMenu8,
                                           lkbMenu9,
                                           lkbMenu10,
                                           lkbMenu11,
                                           lkbMenu12,
                                           lkbMenu13,
                                           lkbMenu14,
                                           lkbMenu15,
                                           lkbMenu16,
                                           lkbMenu17,
                                           lkbmenu18}
        Dim strCmdDispCD As String
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

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

            'グループ番号取得
            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            For Each lkbCont As LinkButton In lkbControls
                lkbCont.Enabled = False
            Next

            'プログラムＩＤ、画面名設定
            Master.ppProgramID = M_DISP_ID
            Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

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
                        psMesBox(Me, "30001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                        psClose_Window(Me)
                        Return
                    End If

                    '取得したデータにて遷移先設定
                    For zz As Integer = 0 To strMenDispCD.Length - 1
                        For Each rowData As System.Data.DataRow In dstOrders.Tables(0).Rows
                            '社員情報(13番)は非活性固定
                            If rowData.Item("M18_DISP_CD").ToString = strMenDispCD(zz) _
                                AndAlso lkbControls(zz).ID <> lkbMenu13.ID Then
                                lkbControls(zz).Enabled = True
                                If blnFocus = False Then
                                    lkbControls(zz).Focus()
                                    blnFocus = True
                                End If
                            End If
                        Next
                    Next


                    Dim dsCnstCnt As New DataSet

                    cmdDB = New SqlCommand("COMMENP002_S1", conDB)

                    'リストデータ取得
                    dsCnstCnt = clsDataConnect.pfGet_DataSet(cmdDB)

                    If Not dsCnstCnt Is Nothing AndAlso dsCnstCnt.Tables(0).Rows.Count > 0 Then
                        lblCnstCnt.Text = DateTime.Now.ToString("MM月dd日") + "受信分：" + dsCnstCnt.Tables(0).Rows(0).Item("T08_LTST_NO").ToString + "件"
                    Else
                        lblCnstCnt.Text = DateTime.Now.ToString("MM月dd日") + "受信分：0件"
                    End If

                    Call psDisposeDataSet(dsCnstCnt)

                Catch ex As Exception
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "権限")
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

    '---------------------------
    '2014/04/17 武 ここから
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
                lkbMenu7.Enabled = True
            Case "SPC"
            Case "営業所"
                lkbMenu7.Enabled = False
                lkbMenu10.Enabled = False
                lkbMenu11.Enabled = False
                lkbMenu12.Enabled = False
                lkbMenu14.Enabled = False
                lkbMenu16.Enabled = False
                lkbMenu17.Enabled = False
            Case "NGC"
        End Select

    End Sub
    '---------------------------
    '2014/04/17 武 ここまで
    '---------------------------

    Protected Sub lkbMenu1_Click(sender As Object, e As EventArgs) Handles lkbMenu1.Click
        Dim strDispID As String = P_FUN_CNS & P_SCR_LST & P_PAGE & "001"
        Dim strPath As String = "~/" & P_CNS & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号
        Session(P_SESSION_BCLIST) = Master.ppTitle                      'パンくずリスト

        Response.Redirect(strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbMenu2_Click(sender As Object, e As EventArgs) Handles lkbMenu2.Click
        Dim strDispID As String = P_FUN_CNS & P_SCR_UPD & P_PAGE & "005"
        Dim strPath As String = "~/" & P_CNS & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号
        Session(P_SESSION_BCLIST) = Master.ppTitle                      'パンくずリスト

        Response.Redirect(strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbMenu3_Click(sender As Object, e As EventArgs) Handles lkbMenu3.Click
        Dim strDispID As String = P_FUN_CNS & P_SCR_LST & P_PAGE & "007"
        Dim strPath As String = "~/" & P_CNS & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号
        Session(P_SESSION_BCLIST) = Master.ppTitle                      'パンくずリスト

        Response.Redirect(strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbMenu4_Click(sender As Object, e As EventArgs) Handles lkbMenu4.Click
        Dim strDispID As String = P_FUN_CNS & P_SCR_LST & P_PAGE & "002"
        Dim strPath As String = "~/" & P_CNS & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号
        Session(P_SESSION_BCLIST) = Master.ppTitle                      'パンくずリスト

        Response.Redirect(strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbMenu7_Click(sender As Object, e As EventArgs) Handles lkbMenu7.Click
        Dim strDispID As String = P_FUN_CNS & P_SCR_LST & P_PAGE & "006"
        Dim strPath As String = "~/" & P_CNS & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号
        Session(P_SESSION_BCLIST) = Master.ppTitle                      'パンくずリスト

        Response.Redirect(strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbMenu8_Click(sender As Object, e As EventArgs) Handles lkbMenu8.Click
        Dim strDispID As String = P_FUN_SER & P_SCR_UPD & P_PAGE & "001"
        Dim strPath As String = "~/" & P_CNS & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号
        Session(P_SESSION_BCLIST) = Master.ppTitle                      'パンくずリスト

        Response.Redirect(strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbMenu9_Click(sender As Object, e As EventArgs) Handles lkbMenu9.Click
        Dim strDispID As String = P_FUN_SER & P_SCR_LST & P_PAGE & "001"
        Dim strPath As String = "~/" & P_CNS & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号
        Session(P_SESSION_BCLIST) = Master.ppTitle                      'パンくずリスト

        Response.Redirect(strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbMenu10_Click(sender As Object, e As EventArgs) Handles lkbMenu10.Click
        Dim strDispID As String = P_FUN_DLC & P_SCR_LST & P_PAGE & "001"
        Dim strPath As String = "~/" & P_WAT & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号
        Session(P_SESSION_BCLIST) = Master.ppTitle                      'パンくずリスト

        Response.Redirect(strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbMenu11_Click(sender As Object, e As EventArgs) Handles lkbMenu11.Click
        Dim strDispID As String = P_FUN_TBP & P_SCR_INP & P_PAGE & "001"
        Dim strPath As String = "~/" & P_SPC & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号
        Session(P_SESSION_BCLIST) = Master.ppTitle                      'パンくずリスト

        Response.Redirect(strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbMenu12_Click(sender As Object, e As EventArgs) Handles lkbMenu12.Click
        Dim strDispID As String = P_FUN_COM & P_SCR_SEL & P_PAGE & "002"
        Dim strPath As String = "~/" & P_COM & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号
        Session(P_SESSION_BCLIST) = Master.ppTitle                      'パンくずリスト

        Response.Redirect(strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    '社員情報は非活性固定となったためコメント化
    'Protected Sub lkbMenu13_Click(sender As Object, e As EventArgs) Handles lkbMenu13.Click
    '    Dim strDispID As String = P_FUN_CTI & P_SCR_SEL & P_PAGE & "005"
    '    Dim strPath As String = "~/" & P_MAI & "/" & strDispID & "/" & strDispID & ".aspx"
    '    'ログ出力開始
    '    psLogStart(Me)

    '    Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号
    '    Session(P_SESSION_BCLIST) = Master.ppTitle                      'パンくずリスト
    '    Response.Redirect(strPath)
    '    'ログ出力終了
    '    psLogEnd(Me)
    'End Sub

    Protected Sub lkbMenu14_Click(sender As Object, e As EventArgs) Handles lkbMenu14.Click
        Dim strDispID As String = P_FUN_COM & P_SCR_SEL & P_PAGE & "004"
        Dim strPath As String = "~/" & P_COM & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号
        Session(P_SESSION_BCLIST) = Master.ppTitle                      'パンくずリスト

        Response.Redirect(strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbMenu15_Click(sender As Object, e As EventArgs) Handles lkbMenu15.Click
        Dim strDispID As String = P_FUN_COM & P_SCR_LST & P_PAGE & "099"
        Dim strPath As String = "~/" & P_COM & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号
        Session(P_SESSION_BCLIST) = Master.ppTitle                      'パンくずリスト
        Session(P_SESSION_NGC_MEN) = {"工事完了報告書兼検収書", "30"}

        Response.Redirect(strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    'COMMENP002-003
    Protected Sub lbkMenu18_Click(sender As Object, e As EventArgs) Handles lkbmenu18.Click
        Dim strDispID As String = P_FUN_COM & P_SCR_LST & P_PAGE & "001"
        Dim strpath As String = "~/" & P_COM & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)  'グループ番号
        Session(P_SESSION_BCLIST) = Master.ppTitle

        Response.Redirect(strpath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    'COMMENP002-001
    Protected Sub lkbMenu16_Click(sender As Object, e As EventArgs) Handles lkbMenu16.Click
        Dim strDispID As String = P_FUN_CNS & P_SCR_LST & P_PAGE & "005"
        Dim strPath As String = "~/" & P_CNS & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号
        Session(P_SESSION_BCLIST) = Master.ppTitle                      'パンくずリスト

        Response.Redirect(strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbMenu17_Click(sender As Object, e As EventArgs) Handles lkbMenu17.Click
        Dim strDispID As String = P_FUN_DLL & P_SCR_SEL & P_PAGE & "001"
        Dim strPath As String = "~/" & P_CNS & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号
        Session(P_SESSION_BCLIST) = Master.ppTitle                      'パンくずリスト

        Response.Redirect(strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub
    'COMMENP002-001 END

#End Region

#Region "そのほかのプロシージャ"
#End Region

#Region "終了処理プロシージャ"
#End Region


End Class
