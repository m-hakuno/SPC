'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　マスタ管理メニュー
'*　ＰＧＭＩＤ：　ComMstMenu
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.08.18　：　伯野
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
'Imports SPC.Global_asax
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SQL_DBCLS_LIB


Public Class WebForm1

    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
    Inherits System.Web.UI.Page


    '============================================================================================================================
    '=　定数定義
    '============================================================================================================================
    Const M_DISP_ID = "ComMstMenu"
    Const M_LOGIN = "~/" & P_COM & "/" & P_FUN_COM & P_SCR_LGI & P_PAGE & "001/" & P_FUN_COM & P_SCR_LGI & P_PAGE & "001.aspx"


    '============================================================================================================================
    '=　構造体・列挙体定義
    '============================================================================================================================


    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
    Private mstrPrm(2) As String
    Dim objStack As StackFrame
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom
    Dim clsExc As New ClsCMExclusive
    Dim mclsDB As New ClsSQLSvrDB
    Dim mstrDBConn = ConfigurationManager.ConnectionStrings("SPCDB").ToString
    Dim objWKDT As New DataSet
    Dim stbSQL As New StringBuilder


    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
    '============================================================================================================================
    '=　イベントプロシージャ定義
    '============================================================================================================================

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim zz As Integer = 0
        Dim strAuth As String = ""

        objStack = New StackFrame

        'プログラムＩＤ、画面名設定
        Master.ppProgramID = M_DISP_ID
        Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)
        'ログアウト設定
        Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる

        If Not IsPostBack Then '初回表示
            'パンくずリスト設定
            Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)
            ''セッション変数「遷移条件」「キー情報」が存在しない場合、画面を閉じる
            'If Session(P_SESSION_TERMS) Is Nothing Or Session(P_KEY) Is Nothing Then
            '    psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            '    '--------------------------------
            '    '2014/06/11 後藤　ここから
            '    '--------------------------------
            '    '排他削除
            '    clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
            '    '--------------------------------
            '    '2014/06/11 後藤　ここまで
            '    '--------------------------------
            '    psClose_Window(Me)
            '    Return
            'End If
            ''ViewStateに「遷移条件」「遷移元ＩＤ」を保存
            'ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)
            'ViewState(P_KEY) = Session(P_KEY)(0)
            'ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            'ログインユーザーの権限確認
            If mfGet_UserAuth(User.Identity.Name, strAuth) Then
                If strAuth >= "3" Then
                    '管理者
                Else
                    psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    Exit Sub
                End If
            End If

        End If

        stbSQL.Clear()
        stbSQL.Append("SELECT M17_DISP_NM,M17_DISP_CD")
        stbSQL.Append("  FROM SPCDB.dbo.M17_PROCESS")
        stbSQL.Append(" WHERE (M17_DISP_CD LIKE '%COMUPDM%'")
        stbSQL.Append("    OR M17_DISP_CD LIKE '%COMUPDT%')")
        stbSQL.Append("   AND M17_DELETE_CD <> '1'")

        'リストデータ取得
        If mclsDB.pfDB_CreateDataSet(objWKDT, stbSQL.ToString, "", mstrDBConn) Then
            If objWKDT Is Nothing Then
            Else
                If objWKDT.Tables.Count > 0 Then
                    bltMenu.DataSource = objWKDT.Tables(0)
                    bltMenu.DataBind()
                    'For zz = 0 To objWKDT.Tables(0).Rows.Count - 1

                    'Next
                End If
            End If
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            psNext_Page(Me, M_LOGIN, ClsComVer.E_S実行.描画前)
        End If

    End Sub

    Private Sub bltMenu_Click(sender As Object, e As BulletedListEventArgs) Handles bltMenu.Click

        'マスタ管理業務
        Dim strDispID As String = bltMenu.Items(e.Index).Value
        Dim strPath As String = "~/Master/" & bltMenu.Items(e.Index).Value & "/" & bltMenu.Items(e.Index).Value & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        'セッション情報共通部設定
        msSet_Session()

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)

    End Sub

    '============================================================================================================================
    '=　プロシージャ定義
    '============================================================================================================================

    '-------------------------------------------------------------------------------------------------------------------
    '　ユーザー権限取得
    '-------------------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' ユーザー権限取得処理
    ''' </summary>
    ''' <param name="ipstrUserID">ユーザーＩＤ</param>
    ''' <param name="opstrAuth">権限コード</param>
    ''' <remarks></remarks>
    Private Function mfGet_UserAuth(ByVal ipstrUserID As String, ByRef opstrAuth As String) As Boolean

        Dim strRet As String = String.Empty

        mfGet_UserAuth = False
        objStack = New StackFrame
        opstrAuth = ""

        Try
            'データ取得
            Call mclsDB.psDisposeDataSet(objWKDT)
            stbSQL.Clear()
            stbSQL.Append("	SELECT M02_AUTH_CLS ")
            stbSQL.Append("   FROM SPCDB.dbo.M01_USER")
            stbSQL.Append("   LEFT JOIN M02_EMPLOYEE ON M02_EMPLOYEE_CD = M01_EMPLY_CD")
            stbSQL.Append("  WHERE M01_USERID = '" & ipstrUserID & "'")
            stbSQL.Append("    AND M01_DELETE_FLG <> '1'")
            stbSQL.Append("    AND M02_DELETE_FLG <> '1'")
            If mclsDB.pfDB_CreateDataSet(objWKDT, stbSQL.ToString, "", mstrDBConn) Then
                If objWKDT Is Nothing Then
                Else
                    If objWKDT.Tables.Count > 0 Then
                        If objWKDT.Tables(0).Rows.Count > 0 Then
                            If objWKDT.Tables(0).Rows(0).Item("M02_AUTH_CLS") Is DBNull.Value Then
                            Else
                                opstrAuth = objWKDT.Tables(0).Rows(0).Item("M02_AUTH_CLS")
                            End If
                        Else
                            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ユーザー権限")
                        End If
                    End If
                End If
            Else
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

            mfGet_UserAuth = True

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ユーザー権限")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Function

    '-------------------------------------------------------------------------------------------------------------------
    '　セッション変数設定
    '-------------------------------------------------------------------------------------------------------------------
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

End Class