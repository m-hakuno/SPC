'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　メインメニュー
'*　ＰＧＭＩＤ：　COMMENP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.10.24　：　土岐
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'COMMENP001-001     2015/07/07      稲葉       営業所の管理者ユーザー用にマスタ管理リンクを活性化する制御追加
'COMMENP001-002     2016/12/16      稲葉       CRS(Contact Reception System)用のスケジュールリンク追加
'COMMENP001-003     2017/02/01      加賀       CRSをWKB,SPCで利用可能に変更
'COMMENP001-004     2017/10/13      伯野       各プロシージャにデータセット破棄を記述

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
'Imports SPC.Global_asax
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports System.Data.SqlClient

#End Region

Public Class COMMENP001
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
    Const M_DISP_ID = P_FUN_COM & P_SCR_MEN & P_PAGE & "001"
    Const M_LOGIN = "~/" & P_COM & "/" & P_FUN_COM & P_SCR_LGI & P_PAGE & "001/" & P_FUN_COM & P_SCR_LGI & P_PAGE & "001.aspx"
    Const VS_CRS_DVS As String = "VS_CRSDVS" 'COMMENP001-003
    Const VS_VSC_DVS As String = "VS_VSCDVS" 'COMMENP001-003
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

        Dim blnFocus As Boolean = False
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim dstAuth As New DataSet
        Dim strMenDispCD() As String = Nothing
        Dim strCmdDispCD As String = String.Empty
        Dim strAdd As String = Nothing
        Dim lkbControls() As LinkButton = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'ログアウト設定
        Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.ログアウト

        '接続場所取得
        strAdd = ConfigurationManager.AppSettings("Address")

        If strAdd = P_ADD_NGC Then
            'ボタンアクションの設定
            AddHandler Master.ppRigthButton1.Click, AddressOf btnUpdate_Click
        End If

        If Not IsPostBack Then  '初回表示
            '画面設定
            Master.ppProgramID = M_DISP_ID
            Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

            '接続場所がNGCの場合メニューを切り替える
            If strAdd = P_ADD_NGC Then
                muvList.ActiveViewIndex = 1
                '物品転送依頼一覧
                'ダウンロードファイル一覧
                '進捗工事一覧
                '保守対応依頼書一覧
                '工事連絡票一覧
                '随時集信一覧状況入力
                'ダウンロードファイル一覧
                'ダウンロードファイル一覧
                '特別保守費用照会
                'シリアル情報一覧
                'ダウンロードファイル一覧
                '監視外対象ホール一覧
                '品質会議資料
                '品質会議資料明細
                '券売入金機自走調査一覧
                '監視報告書兼依頼票一覧
                '玉単価設定情報一覧
                'ＤＳＵ交換対応依頼書一覧
                '進捗一覧（修理・整備）
                '設置環境写真一覧---2018/4/17追加 小野
                strMenDispCD = {P_FUN_CNS & P_SCR_LST & P_PAGE & "002",
                                P_FUN_COM & P_SCR_LST & P_PAGE & "099",
                                P_FUN_CNS & P_SCR_LST & P_PAGE & "004",
                                P_FUN_CMP & P_SCR_LST & P_PAGE & "001",
                                P_FUN_CNS & P_SCR_LST & P_PAGE & "005",
                                P_FUN_TBP & P_SCR_INP & P_PAGE & "001",
                                P_FUN_COM & P_SCR_LST & P_PAGE & "099",
                                P_FUN_COM & P_SCR_LST & P_PAGE & "099",
                                P_FUN_CMP & P_SCR_INQ & P_PAGE & "001",
                                P_FUN_SER & P_SCR_LST & P_PAGE & "001",
                                P_FUN_COM & P_SCR_LST & P_PAGE & "099",
                                P_FUN_WAT & P_SCR_LST & P_PAGE & "002",
                                P_FUN_QUA & P_SCR_OUT & P_PAGE & "001",
                                P_FUN_QUA & P_SCR_UPD & P_PAGE & "001",
                                P_FUN_SLF & P_SCR_LST & P_PAGE & "001",
                                P_FUN_WAT & P_SCR_LST & P_PAGE & "001",
                                P_FUN_BPI & P_SCR_LST & P_PAGE & "001",
                                P_FUN_DSU & P_SCR_LST & P_PAGE & "001",
                                P_FUN_REP & P_SCR_LST & P_PAGE & "001",
                                P_FUN_COM & P_SCR_LST & P_PAGE & "001",
                                P_FUN_DLC & P_SCR_INP & P_PAGE & "002"}

                lkbControls = {lkbNGCMenu1,
                               lkbNGCMenu2,
                               lkbNGCMenu3,
                               lkbNGCMenu4,
                               lkbNGCMenu5,
                               lkbNGCMenu6,
                               lkbNGCMenu7,
                               lkbNGCMenu8,
                               lkbNGCMenu9,
                               lkbNGCMenu10,
                               lkbNGCMenu11,
                               lkbNGCMenu12,
                               lkbNGCMenu13,
                               lkbNGCMenu19,
                               lkbNGCMenu14,
                               lkbNGCMenu15,
                               lkbNGCMenu16,
                               lkbNGCMenu17,
                               lkbNGCMenu18,
                               lkbNGCMenu20,
                               lkbNGCMenu21}

                '更新ボタン設定
                Master.ppRigthButton1.Text = P_BTN_NM_UPD
                Master.ppRigthButton1.Visible = True

            Else
                muvList.ActiveViewIndex = 0
                '工事管理
                '保守管理
                '監視業務
                '進捗管理
                'ヘルスチェック
                '検収/請求
                'ホール参照
                'スケジュール管理　COMMENP001-002
                strMenDispCD = {P_FUN_COM & P_SCR_MEN & P_PAGE & "002",
                                P_FUN_COM & P_SCR_MEN & P_PAGE & "003",
                                P_FUN_COM & P_SCR_MEN & P_PAGE & "004",
                                P_FUN_COM & P_SCR_MEN & P_PAGE & "005",
                                P_FUN_COM & P_SCR_MEN & P_PAGE & "007",
                                P_FUN_COM & P_SCR_MEN & P_PAGE & "008",
                                P_FUN_COM & P_SCR_SEL & P_PAGE & "001",
                                "ComMstMenu",
                                "",
                                P_FUN_SCL & P_SCR_MEN & P_PAGE & "001",
                                P_FUN_COM & P_SCR_MEN & P_PAGE & "010"}

                lkbControls = {lkbMenu1,
                               lkbMenu2,
                               lkbMenu3,
                               lkbMenu4,
                               lkbMenu5,
                               lkbMenu6,
                               lkbMenu7,
                               lkbMenu8,
                               lkbMenu9,
                               lkbMenu10}
            End If

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

                    'COMMENP001-001
                    'ユーザーの権限を取得。ViewStateに保管
                    cmdDB = New SqlCommand("ZCMPSEL056", conDB)
                    With cmdDB.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("user_id", SqlDbType.NVarChar, User.Identity.Name))
                    End With
                    dstAuth = clsDataConnect.pfGet_DataSet(cmdDB)
                    ViewState("user_auth") = dstAuth.Tables(0).Rows(0).Item("M02_AUTH_CLS").ToString
                    'COMMENP001-001 END

                    'COMMENP001-002
                    'ユーザーの権限(メール管理区分、範囲管理区分)を取得。ViewStateに保管
                    cmdDB = New SqlCommand("ZCMPSEL061", conDB)
                    With cmdDB.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("user_id", SqlDbType.NVarChar, User.Identity.Name))
                    End With
                    dstAuth = clsDataConnect.pfGet_DataSet(cmdDB)
                    'COMMENP001-003
                    'ViewState("user_mail_dvs") = dstAuth.Tables(0).Rows(0).Item("メール管理区分").ToString
                    ViewState(VS_VSC_DVS) = dstAuth.Tables(0).Rows(0).Item("範囲管理区分").ToString
                    ViewState(VS_CRS_DVS) = dstAuth.Tables(0).Rows(0).Item("CRS区分").ToString
                    'COMMENP001-003 END
                    'COMMENP001-002 END

                Catch ex As Exception
                    '認証クリア
                    FormsAuthentication.SignOut()
                    Session.Abandon()
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "権限")
                    '--------------------------------
                    '2014/04/14 星野　ここから
                    '--------------------------------
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    '--------------------------------
                    '2014/04/14 星野　ここまで
                    '--------------------------------
                    psNext_Page(Me, M_LOGIN, ClsComVer.E_S実行.描画前)
                Finally
                    'データセットの破棄
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

            If strAdd = P_ADD_NGC Then
                'ＮＧＣの場合未完了件数を検索
                msGet_Unfinished()
            End If
        End If
    End Sub

    '---------------------------
    '2014/04/14 武 ここから
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
                lkbMenu9.Enabled = True

                'COMMENP001-003
                'COMMENP001-002
                ''メニューに表示させない
                'lkbMenu10.Visible = False
                'lkbMenu10.Enabled = False
                ''COMMENP001-002 END
                If ViewState(VS_CRS_DVS) = "0" Then
                    ' 対象
                    lkbMenu10.Visible = True
                    lkbMenu10.Enabled = True
                Else
                    '対象外
                    lkbMenu10.Visible = False
                    lkbMenu10.Enabled = False
                End If
                'COMMENP001-003 END

            Case "SPC"
                lkbMenu6.Enabled = False
                lkbMenu8.Enabled = False
                lkbMenu9.Enabled = False

                'COMMENP001-003
                ''COMMENP001-002
                ''メニューに表示させない
                'lkbMenu10.Visible = False
                'lkbMenu10.Enabled = False
                ''COMMENP001-002 END
                If ViewState(VS_CRS_DVS) = "0" Then
                    ' 対象
                    lkbMenu10.Visible = True
                    lkbMenu10.Enabled = True
                Else
                    '対象外
                    lkbMenu10.Visible = False
                    lkbMenu10.Enabled = False
                End If
                'COMMENP001-003 END

            Case "営業所"
                lkbMenu3.Enabled = False
                lkbMenu5.Enabled = False
                lkbMenu6.Enabled = False
                lkbMenu9.Enabled = False
                'COMMENP001-001
                If ViewState("user_auth") = "3" Or ViewState("user_auth") = "6" Then
                    lkbMenu8.Enabled = True
                Else
                    lkbMenu8.Enabled = False
                End If
                'lkbMenu8.Enabled = False
                'COMMENP001-001 END

                'COMMENP001-003
                ''COMMENP001-002
                ''営業所の責任者・作業者のみメニュー表示
                'If ViewState("user_vsc_dvs") = "1" Or ViewState("user_vsc_dvs") = "0" Then
                '    lkbMenu10.Visible = True
                '    lkbMenu10.Enabled = True
                'Else
                '    lkbMenu10.Visible = False
                '    lkbMenu10.Enabled = False
                'End If
                ''COMMENP001-002 END
                If ViewState(VS_CRS_DVS) = "0" Then
                    ' 対象
                    lkbMenu10.Visible = True
                    lkbMenu10.Enabled = True
                Else
                    '対象外
                    lkbMenu10.Visible = False
                    lkbMenu10.Enabled = False
                End If
                'COMMENP001-003 END


            Case "NGC"
        End Select


        'マスタ管理画面を使用するまでは非表示
        'lkbMenu8.Visible = False

    End Sub
    '---------------------------
    '2014/04/14 武 ここまで
    '---------------------------

    'SPC
    Protected Sub lkbMenu1_Click(sender As Object, e As EventArgs) Handles lkbMenu1.Click
        '工事管理
        Dim strDispID As String = P_FUN_COM & P_SCR_MEN & P_PAGE & "002"
        Dim strPath As String = "~/" & P_COM & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbMenu2_Click(sender As Object, e As EventArgs) Handles lkbMenu2.Click
        '保守管理
        Dim strDispID As String = P_FUN_COM & P_SCR_MEN & P_PAGE & "003"
        Dim strPath As String = "~/" & P_COM & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbMenu3_Click(sender As Object, e As EventArgs) Handles lkbMenu3.Click
        '監視業務
        Dim strDispID As String = P_FUN_COM & P_SCR_MEN & P_PAGE & "004"
        Dim strPath As String = "~/" & P_COM & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbMenu4_Click(sender As Object, e As EventArgs) Handles lkbMenu4.Click
        '進捗管理
        Dim strDispID As String = P_FUN_COM & P_SCR_MEN & P_PAGE & "005"
        Dim strPath As String = "~/" & P_COM & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbMenu5_Click(sender As Object, e As EventArgs) Handles lkbMenu5.Click
        'ヘルスチェック
        Dim strDispID As String = P_FUN_COM & P_SCR_MEN & P_PAGE & "007"
        Dim strPath As String = "~/" & P_COM & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbMenu6_Click(sender As Object, e As EventArgs) Handles lkbMenu6.Click
        '検収/請求
        Dim strDispID As String = P_FUN_COM & P_SCR_MEN & P_PAGE & "008"
        Dim strPath As String = "~/" & P_COM & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbMenu7_Click(sender As Object, e As EventArgs) Handles lkbMenu7.Click
        'ホール参照
        Dim strDispID As String = P_FUN_COM & P_SCR_SEL & P_PAGE & "001"
        Dim strPath As String = "~/" & P_COM & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbMenu8_Click(sender As Object, e As EventArgs) Handles lkbMenu8.Click
        'マスタ管理
        Dim strDispID As String = P_FUN_COM & P_SCR_MEN & P_PAGE & "009"
        Dim strPath As String = "~/" & P_COM & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbMenu9_Click(sender As Object, e As EventArgs) Handles lkbMenu9.Click
        '定時実行　確認
        Dim strDispID As String = "TJKLSTP001"
        Dim strPath As String = "~/Watch/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    'COMMENP001-002
    Protected Sub lkbMenu10_Click(sender As Object, e As EventArgs) Handles lkbMenu10.Click
        'スケジュール管理
        Dim strDispID As String = P_FUN_SCL & P_SCR_MEN & P_PAGE & "001"
        Dim strPath As String = "~/" & P_SCL & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        'COMMENP001-003
        'Session("USR_VSC_DVS") = ViewState("user_vsc_dvs").ToString
        If ViewState(VS_CRS_DVS).ToString = "0" Then
            Select Case ViewState(VS_VSC_DVS).ToString
                Case "0"
                    Session(P_SESSION_CRS_USE) = ClsComVer.E_CRS使用制限.参照
                Case "1"
                    Session(P_SESSION_CRS_USE) = ClsComVer.E_CRS使用制限.更新
                Case "9"
                    Session(P_SESSION_CRS_USE) = ClsComVer.E_CRS使用制限.使用不可
            End Select
        Else
            Session(P_SESSION_CRS_USE) = ClsComVer.E_CRS使用制限.使用不可
        End If
        'COMMENP001-003 END

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub
    'COMMENP001-002 END

    Protected Sub lkbMenu11_Click(sender As Object, e As EventArgs) Handles lkbMenu11.Click
        '周知事項
        Dim strDispID As String = "COMMENP010"
        Dim strPath As String = "~/COMMON/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub


    'NGC
    Protected Sub lkbNGCMenu1_Click(sender As Object, e As EventArgs) Handles lkbNGCMenu1.Click
        '物品転送依頼一覧
        Dim strDispID As String = P_FUN_CNS & P_SCR_LST & P_PAGE & "002"
        Dim strPath As String = "~/" & P_CNS & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbNGCMenu2_Click(sender As Object, e As EventArgs) Handles lkbNGCMenu2.Click
        'ダウンロードファイル一覧
        Dim strDispID As String = P_FUN_COM & P_SCR_LST & P_PAGE & "099"
        Dim strPath As String = "~/" & P_COM & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()
        'NGCダウンロード
        mstrPrm = {sender.Text, "2"}
        Session(P_SESSION_NGC_MEN) = mstrPrm

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbNGCMenu3_Click(sender As Object, e As EventArgs) Handles lkbNGCMenu3.Click
        '進捗工事一覧
        Dim strDispID As String = P_FUN_CNS & P_SCR_LST & P_PAGE & "004"
        Dim strPath As String = "~/" & P_CNS & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbNGCMenu4_Click(sender As Object, e As EventArgs) Handles lkbNGCMenu4.Click
        '保守対応依頼書一覧
        Dim strDispID As String = P_FUN_CMP & P_SCR_LST & P_PAGE & "001"
        Dim strPath As String = "~/" & P_MAI & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbNGCMenu5_Click(sender As Object, e As EventArgs) Handles lkbNGCMenu5.Click
        '工事連絡票一覧
        Dim strDispID As String = P_FUN_CNS & P_SCR_LST & P_PAGE & "005"
        Dim strPath As String = "~/" & P_CNS & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbNGCMenu6_Click(sender As Object, e As EventArgs) Handles lkbNGCMenu6.Click
        '随時集信一覧状況入力
        Dim strDispID As String = P_FUN_TBP & P_SCR_INP & P_PAGE & "001"
        Dim strPath As String = "~/" & P_SPC & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbNGCMenu7_Click(sender As Object, e As EventArgs) Handles lkbNGCMenu7.Click
        'ダウンロードファイル一覧
        Dim strDispID As String = P_FUN_COM & P_SCR_LST & P_PAGE & "099"
        Dim strPath As String = "~/" & P_COM & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()
        'NGCダウンロード
        mstrPrm = {sender.Text, "7"}
        Session(P_SESSION_NGC_MEN) = mstrPrm

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbNGCMenu8_Click(sender As Object, e As EventArgs) Handles lkbNGCMenu8.Click
        'ダウンロードファイル一覧
        Dim strDispID As String = P_FUN_COM & P_SCR_LST & P_PAGE & "099"
        Dim strPath As String = "~/" & P_COM & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()
        'NGCダウンロード
        mstrPrm = {sender.Text, "8"}
        Session(P_SESSION_NGC_MEN) = mstrPrm

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbNGCMenu9_Click(sender As Object, e As EventArgs) Handles lkbNGCMenu9.Click
        '特別保守費用照会
        Dim strDispID As String = P_FUN_CMP & P_SCR_INQ & P_PAGE & "001"
        Dim strPath As String = "~/" & P_MAI & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbNGCMenu10_Click(sender As Object, e As EventArgs) Handles lkbNGCMenu10.Click
        'シリアル情報一覧
        Dim strDispID As String = P_FUN_SER & P_SCR_LST & P_PAGE & "001"
        Dim strPath As String = "~/" & P_CNS & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbNGCMenu11_Click(sender As Object, e As EventArgs) Handles lkbNGCMenu11.Click
        'ダウンロードファイル一覧
        Dim strDispID As String = P_FUN_COM & P_SCR_LST & P_PAGE & "099"
        Dim strPath As String = "~/" & P_COM & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()
        'NGCダウンロード
        mstrPrm = {sender.Text, "11"}
        Session(P_SESSION_NGC_MEN) = mstrPrm

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbNGCMenu12_Click(sender As Object, e As EventArgs) Handles lkbNGCMenu12.Click
        '監視外対象ホール一覧
        Dim strDispID As String = P_FUN_WAT & P_SCR_LST & P_PAGE & "002"
        Dim strPath As String = "~/" & P_WAT & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbNGCMenu13_Click(sender As Object, e As EventArgs) Handles lkbNGCMenu13.Click
        '品質会議資料
        Dim strDispID As String = P_FUN_QUA & P_SCR_OUT & P_PAGE & "001"
        Dim strPath As String = "~/" & P_MAI & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbNGCMenu19_Click(sender As Object, e As EventArgs) Handles lkbNGCMenu19.Click
        '品質会議資料明細
        Dim strDispID As String = P_FUN_QUA & P_SCR_UPD & P_PAGE & "001"
        Dim strPath As String = "~/" & P_MAI & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbNGCMenu14_Click(sender As Object, e As EventArgs) Handles lkbNGCMenu14.Click
        '券売入金機自走調査一覧
        Dim strDispID As String = P_FUN_SLF & P_SCR_LST & P_PAGE & "001"
        Dim strPath As String = "~/" & P_WAT & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbNGCMenu15_Click(sender As Object, e As EventArgs) Handles lkbNGCMenu15.Click
        '監視報告書兼依頼票一覧
        Dim strDispID As String = P_FUN_WAT & P_SCR_LST & P_PAGE & "001"
        Dim strPath As String = "~/" & P_WAT & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbNGCMenu16_Click(sender As Object, e As EventArgs) Handles lkbNGCMenu16.Click
        '玉単価設定情報一覧
        Dim strDispID As String = P_FUN_BPI & P_SCR_LST & P_PAGE & "001"
        Dim strPath As String = "~/" & P_MAI & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbNGCMenu17_Click(sender As Object, e As EventArgs) Handles lkbNGCMenu17.Click
        'ＤＳＵ交換対応依頼書一覧
        Dim strDispID As String = P_FUN_DSU & P_SCR_LST & P_PAGE & "001"
        Dim strPath As String = "~/" & P_WAT & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Protected Sub lkbNGCMenu18_Click(sender As Object, e As EventArgs) Handles lkbNGCMenu18.Click
        '進捗一覧（修理・整備）
        Dim strDispID As String = P_FUN_REP & P_SCR_LST & P_PAGE & "001"
        Dim strPath As String = "~/" & P_RPE & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    '---2018/4/17追加 小野 ここから
    Protected Sub lkbNGCMenu20_Click(sender As Object, e As EventArgs) Handles lkbNGCMenu20.Click
        '設置環境写真一覧
        Dim strDispID As String = P_FUN_COM & P_SCR_LST & P_PAGE & "001"
        Dim strPath As String = "~/" & P_COM & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub
    '---2018/4/17ここまで

    Protected Sub lkbNGCMenu21_Click(sender As Object, e As EventArgs) Handles lkbNGCMenu21.Click
        'ＭＤＮ－Ｈ設置写真一覧
        Dim strDispID As String = P_FUN_DLC & P_SCR_INP & P_PAGE & "002"
        Dim strPath As String = "~/" & P_COM & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 更新ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs)
        'ログ出力開始
        psLogStart(Me)
        '未完了件数取得
        msGet_Unfinished()
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

    ''' <summary>
    ''' 未完了件数取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub msGet_Unfinished()
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

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                '未完了件数取得
                cmdDB = New SqlCommand("COMMENP001_S1", conDB)

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '未完了件数設定
                With dstOrders.Tables(0).Rows(0)
                    If .Item("工事連絡票件数").ToString.Length > 3 Then
                        Me.lblNGCMenu5.Text = "999"
                    Else
                        Me.lblNGCMenu5.Text = .Item("工事連絡票件数").ToString
                    End If

                    If .Item("特別保守費用照会件数").ToString.Length > 3 Then
                        Me.lblNGCMenu9.Text = "999"
                    Else
                        Me.lblNGCMenu9.Text = .Item("特別保守費用照会件数").ToString
                    End If

                    If .Item("監視報告書兼依頼票一覧件数").ToString.Length > 3 Then
                        Me.lblNGCMenu15.Text = "999"
                    Else
                        Me.lblNGCMenu15.Text = .Item("監視報告書兼依頼票一覧件数").ToString
                    End If
                End With

            Catch ex As Exception
                '認証クリア
                FormsAuthentication.SignOut()
                Session.Abandon()
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "未完了件数")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                psNext_Page(Me, M_LOGIN, ClsComVer.E_S実行.描画前)
            Finally
                'データセット破棄
                Call psDisposeDataSet(dstOrders)
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
    End Sub

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
