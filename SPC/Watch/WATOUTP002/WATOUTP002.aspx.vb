'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜監視＞
'*　処理名　　：　ＴＢＯＸ結果表示　決済照会情報
'*　ＰＧＭＩＤ：　WATOUTP026
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.28　：　中川
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax

Public Class WATOUTP002

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
    'プログラムＩＤ
    Const M_DISP_ID = P_FUN_WAT & P_SCR_OUT & P_PAGE & "002"

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

        objStack = New StackFrame

        Try
            AddHandler Master.ppRigthButton10.Click, AddressOf btnPrint_Click

            If Not IsPostBack Then  '初回表示

                'セッションのキー項目を取得
                ViewState(P_KEY) = Session(P_KEY)   '0：照会管理番号  1：連番  2：ＮＬ区分  3：ＩＤＩＣ区分  4：運用日付  5：要求通番  6：枝番
                If Not Session(P_SESSION_GROUP_NUM) Is Nothing Then                 'グループ番号
                    ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)
                End If
                If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then             '登録用年月日
                    Master.ppExclusiveDateDtl = Session(P_SESSION_EXCLUSIV_DATE)
                End If

                '自動ポストバックを有効にする
                Me.rblSlctPrnt.AutoPostBack = True

                '画面情報設定
                Master.ppProgramID = M_DISP_ID
                Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)
                '                Master.ppLogout_Mode = Global_asax. ClsComVer.E_ログアウトモード.閉じる
                Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる

                'ボタン設定
                Master.ppRigthButton10.Visible = True
                Master.ppRigthButton10.Text = "印刷"
                Master.ppRigthButton10.Enabled = False

                'パンくずリスト設定
                Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

            End If

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

    End Sub

    ''' <summary>
    ''' 印刷ボタン押下時のイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        objStack = New StackFrame

        Try
            Dim strSelect As String = String.Empty
            Dim rpt As New Object
            Dim strSql As String = String.Empty
            Dim strSqlPrm As String = String.Empty
            Dim strFileName As String = String.Empty
            Dim strKeyList() As String = ViewState(P_KEY)

            Dim conDB As SqlConnection = Nothing
            Dim cmdDB As SqlCommand = Nothing
            Dim dst As New DataSet
            Dim dstDtl As New DataSet
            Dim blnTwin As Boolean = False
            Dim wkDS As New DataSet

            strSelect = rblSlctPrnt.SelectedValue
            strSqlPrm = " '" & strKeyList(0) & "', '" & strKeyList(1) & "', '" &
                            strKeyList(2) & "', '" & strKeyList(3) & "', '" &
                            strKeyList(4) & "', '" & strKeyList(5) & "', '" &
                            strKeyList(6) & "'"


            If clsDataConnect.pfOpen_Database(conDB) Then

                cmdDB = New SqlCommand("TBRREP002_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("ctrl_no", SqlDbType.NVarChar, strKeyList(0)))
                    .Add(pfSet_Param("seq", SqlDbType.NVarChar, strKeyList(1)))
                    .Add(pfSet_Param("nl_cls", SqlDbType.NVarChar, strKeyList(2)))
                    .Add(pfSet_Param("idic_cls", SqlDbType.NVarChar, strKeyList(3)))
                    .Add(pfSet_Param("unyo_date", SqlDbType.NVarChar, strKeyList(4)))
                    .Add(pfSet_Param("yokyu_seq", SqlDbType.NVarChar, strKeyList(5)))
                    .Add(pfSet_Param("eda_seq", SqlDbType.NVarChar, strKeyList(6)))
                End With

                cmdDB.CommandTimeout = 60

                dst = clsDataConnect.pfGet_DataSet(cmdDB)
                dstDtl.Tables.Add(dst.Tables(1).Copy)

                cmdDB = New SqlCommand("ZCMPSEL063", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("TBOXID", SqlDbType.NVarChar, strKeyList(10)))
                End With

                cmdDB.CommandTimeout = 60

                wkDS = clsDataConnect.pfGet_DataSet(cmdDB)
                If wkDS.Tables.Count > 0 Then
                    If wkDS.Tables(0).Rows.Count > 0 Then
                        If wkDS.Tables(0).Rows(0).Item("T01_TWIN_CD").ToString = "0" Then
                        Else
                            blnTwin = True
                        End If
                    End If
                End If
                Call psDisposeDataSet(wkDS)
                clsDataConnect.pfClose_Database(conDB)

            Else
                'DB接続に失敗
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

            End If

            Select Case strSelect
                Case "1"
                    rpt = New TBRREP002(dstDtl)
                    strFileName = "決算照会情報"
                    '帳票を出力する（データテーブル）
                    psPrintPDF(Me, rpt, dst.Tables(0), strFileName)

                    Exit Sub

                Case "2"
                    'If blnTwin = True Then

                    '    If clsDataConnect.pfOpen_Database(conDB) Then

                    '        cmdDB = New SqlCommand("TBRREP013_S2", conDB)
                    '        With cmdDB.Parameters
                    '            'パラメータ設定
                    '            .Add(pfSet_Param("ctrl_no", SqlDbType.NVarChar, strKeyList(0)))
                    '            .Add(pfSet_Param("idic_cls", SqlDbType.NVarChar, strKeyList(3)))
                    '            .Add(pfSet_Param("eda_seq", SqlDbType.NVarChar, strKeyList(6)))
                    '        End With

                    '        cmdDB.CommandTimeout = 60

                    '        dst = clsDataConnect.pfGet_DataSet(cmdDB)

                    '        clsDataConnect.pfClose_Database(conDB)

                    '    Else
                    '        'DB接続に失敗
                    '        psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    '        Exit Sub
                    '    End If

                    '    rpt = New TBRREP013_1(dst.Tables(1))
                    '    strSql = "EXEC TBRREP013_S2" & strSqlPrm
                    '    strFileName = "決算照会情報（機番別合算）"
                    '    Call psPrintPDF(Me, rpt, dst.Tables(0), strFileName)

                    '    Exit Sub

                    'Else
                    '    rpt = New TBRREP013
                    '    strSql = "EXEC TBRREP013_S1" & strSqlPrm
                    '    strFileName = "決算照会情報（機番別合算）"
                    'End If
                    rpt = New TBRREP013
                    strSql = "EXEC TBRREP013_S1" & strSqlPrm
                    strFileName = "決算照会情報（機番別合算）"

                Case "3"
                    '                    If blnTwin = True Then

                    '                        If clsDataConnect.pfOpen_Database(conDB) Then

                    '                            cmdDB = New SqlCommand("TBRREP014_S2", conDB)
                    '                            With cmdDB.Parameters
                    '                                'パラメータ設定
                    '                                .Add(pfSet_Param("ctrl_no", SqlDbType.NVarChar, strKeyList(0)))
                    '                                .Add(pfSet_Param("idic_cls", SqlDbType.NVarChar, strKeyList(3)))
                    '                                .Add(pfSet_Param("eda_seq", SqlDbType.NVarChar, strKeyList(6)))
                    '                            End With

                    '                            cmdDB.CommandTimeout = 60

                    '                            dst = clsDataConnect.pfGet_DataSet(cmdDB)

                    '                            clsDataConnect.pfClose_Database(conDB)

                    '                        Else
                    '                            'DB接続に失敗
                    '                            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    '                            Exit Sub
                    '                        End If

                    '                        rpt = New TBRREP014_1(dst.Tables(1))
                    ''                        rpt = New TBRREP014
                    '                        strSql = "EXEC TBRREP014_S1" & strSqlPrm
                    '                        strFileName = "決算照会情報（島別合算）"
                    '                        Call psPrintPDF(Me, rpt, dst.Tables(0), strFileName)

                    '                        Exit Sub

                    '                    Else
                    '                        rpt = New TBRREP014
                    '                        strSql = "EXEC TBRREP014_S1" & strSqlPrm
                    '                        strFileName = "決算照会情報（島別合算）"
                    '                    End If
                    rpt = New TBRREP014
                    strSql = "EXEC TBRREP014_S1" & strSqlPrm
                    strFileName = "決算照会情報（島別合算）"

            End Select

            '帳票を出力する（SQL）
            psPrintPDF(Me, rpt, strSql, strFileName)

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

    End Sub

    ''' <summary>
    ''' 印刷対象ラジオボタン選択時のイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub rblSlctPrnt_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblSlctPrnt.SelectedIndexChanged

        objStack = New StackFrame

        Try
            '印刷ボタンを活性にする
            Master.ppRigthButton10.Enabled = True

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

    End Sub

#End Region

#Region "そのほかのプロシージャ"
#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
