'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　スケジュールメニュー
'*　ＰＧＭＩＤ：　SCLMENP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2016.12.16　：　稲葉
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'SCLMENP001-001     2017/02/06      加賀      CRSをWKB,SPCで利用可能に変更

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
'Imports SPC.Global_asax
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports System.Data.SqlClient
#End Region

Public Class SCLMENP001
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
    Const M_DISP_ID = P_FUN_SCL & P_SCR_MEN & P_PAGE & "001"

#End Region

#Region "変数定義"
    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
    'スケジュール一覧
    'スケジュール明細
    Dim strMenDispCD() As String = {P_FUN_SCL & P_SCR_LST & P_PAGE & "001", _
                                    P_FUN_SCL & P_SCR_UPD & P_PAGE & "001", _
                                    P_FUN_SCL & P_SCR_INP & P_PAGE & "001" _
                                  }
    Dim objStack As StackFrame
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
        Dim strCmdDispCD As String

        Dim lkbControls() As LinkButton = {lkbSubMenu1, _
                                           lkbSubMenu2, _
                                           lkbSubMenu3 _
                                         }

        objStack = New StackFrame

        If Not IsPostBack Then
            '画面設定
            Master.ppProgramID = M_DISP_ID
            Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)
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

                    'SCLMENP001-001
                    Select Case Session(P_SESSION_CRS_USE)
                        Case ClsComVer.E_CRS使用制限.使用不可
                            lkbSubMenu2.Enabled = False
                            lkbSubMenu3.Enabled = False
                        Case ClsComVer.E_CRS使用制限.参照
                            lkbSubMenu2.Enabled = False
                            lkbSubMenu3.Enabled = False
                        Case ClsComVer.E_CRS使用制限.更新
                            lkbSubMenu2.Enabled = True
                            lkbSubMenu3.Enabled = True
                        Case Else
                            lkbSubMenu2.Enabled = False
                            lkbSubMenu3.Enabled = False
                    End Select

                Catch ex As Exception
                    'データ取得エラー
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "権限")

                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    psClose_Window(Me)
                Finally
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
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
            Case "NGC"
        End Select

    End Sub

    ''' <summary>
    ''' スケジュール一覧クリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbSubMenu1_Click(sender As Object, e As EventArgs) Handles lkbSubMenu1.Click
        Dim strPath As String = "~/" & P_SCL & "/" & strMenDispCD(0) & "/" & strMenDispCD(0) & ".aspx"

        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_OLDDISP) = M_DISP_ID
        Session(P_SESSION_BCLIST) = Master.ppTitle
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

        Response.Redirect(strPath)

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    Protected Sub lkbSubMenu2_Click(sender As Object, e As EventArgs) Handles lkbSubMenu2.Click
        Dim strPath As String = "~/" & P_SCL & "/" & strMenDispCD(1) & "/" & strMenDispCD(1) & ".aspx"

        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_OLDDISP) = M_DISP_ID
        Session(P_SESSION_BCLIST) = Master.ppTitle
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)
        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録

        Response.Redirect(strPath)

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    Protected Sub lkbSubMenu3_Click(sender As Object, e As EventArgs) Handles lkbSubMenu3.Click
        Dim strPath As String = "~/" & P_SCL & "/" & strMenDispCD(2) & "/" & strMenDispCD(2) & ".aspx"

        'ログ出力開始
        psLogStart(Me)

        Session(P_SESSION_OLDDISP) = M_DISP_ID
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
