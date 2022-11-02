'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜監視＞
'*　処理名　　：　ＴＢＯＸ結果表示　店内通信
'*　ＰＧＭＩＤ：　WATOUTP026
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.29　：　中川
'*  更　新　　：　2017.07.06　：　加賀  ラジオボタンのpostbackイベント削除
'********************************************************************************************************************************

Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect

Public Class WATOUTP026

#Region "継承定義"
    Inherits System.Web.UI.Page
#End Region

#Region "定数定義"

    'プログラムＩＤ
    Const M_DISP_ID = P_FUN_WAT & P_SCR_OUT & P_PAGE & "026"

#End Region

#Region "変数定義"

    Dim objStack As StackFrame
    Dim clsCMDBC As New ClsCMDBCom
    Dim clsDataConnect As New ClsCMDataConnect

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

                'セッションから情報を取得
                ViewState(P_KEY) = Session(P_KEY)   '0：照会管理番号  1：連番  2：ＮＬ区分  3：ＩＤＩＣ区分  4：運用日付  5：要求通番  6：枝番
                If Not Session(P_SESSION_GROUP_NUM) Is Nothing Then                 'グループ番号
                    ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)
                End If
                If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then             '登録用年月日
                    Master.ppExclusiveDateDtl = Session(P_SESSION_EXCLUSIV_DATE)
                End If

                '自動ポストバックを有効にする
                'Me.rblSlctPrnt.AutoPostBack = True  '2017.07.06

                '画面情報設定
                Master.ppProgramID = M_DISP_ID
                Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)
                Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる

                'ボタン設定
                Master.ppRigthButton10.Text = "印刷"
                Master.ppRigthButton10.Visible = True
                Master.ppRigthButton10.Enabled = True

                'パンくずリスト設定
                Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

                '初期選択 '2017.07.06
                rblSlctPrnt.SelectedIndex = 0

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

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        'Dim trans As SqlTransaction = Nothing
        Dim dstOrders As New DataSet
        Dim rpt As New TBRREP012                'レポートクラス
        Dim strSelect As String = String.Empty
        Dim strFileName As String = String.Empty
        Dim strKeyList() As String = ViewState(P_KEY)
        objStack = New StackFrame

        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                strSelect = rblSlctPrnt.SelectedValue

                Select Case strSelect
                    Case "1"
                        strFileName = "店内通信"

                    Case "2"
                        strFileName = "店内通信（接続分）"

                    Case "3"
                        strFileName = "店内通信（未接続分）"

                End Select

                cmdDB = New SqlCommand("TBRREP012_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("ctrl_no", SqlDbType.NVarChar, strKeyList(0)))
                    .Add(pfSet_Param("seq", SqlDbType.NVarChar, strKeyList(1)))
                    .Add(pfSet_Param("nl_cls", SqlDbType.NVarChar, strKeyList(2)))
                    .Add(pfSet_Param("idic_cls", SqlDbType.NVarChar, strKeyList(3)))
                    .Add(pfSet_Param("unyo_date", SqlDbType.NVarChar, strKeyList(4)))
                    .Add(pfSet_Param("yokyu_seq", SqlDbType.NVarChar, strKeyList(5)))
                    .Add(pfSet_Param("eda_seq", SqlDbType.NVarChar, strKeyList(6)))
                    .Add(pfSet_Param("select_flg", SqlDbType.NVarChar, strSelect))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'テーブルに該当レコードがない場合はポップアップを表示
                If dstOrders Is Nothing OrElse dstOrders.Tables(0).Rows.Count = 0 Then
                    psMesBox(Me, "00007", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)
                Else
                    '帳票を出力する
                    psPrintPDF(Me, rpt, dstOrders.Tables(0), strFileName)
                End If

            Catch ex As Exception
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Throw
            Finally
                'DB切断
                If Not conDB Is Nothing Then
                    clsDataConnect.pfClose_Database(conDB)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Sub

    '2017.07.06
    ' ''' <summary>
    ' ''' 印刷対象ラジオボタン選択時のイベント
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub rblSlctPrnt_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblSlctPrnt.SelectedIndexChanged

    '    '--------------------------------
    '    '2014/04/14 星野　ここから
    '    '--------------------------------
    '    objStack = New StackFrame
    '    '--------------------------------
    '    '2014/04/14 星野　ここまで
    '    '--------------------------------
    '    Try
    '        '印刷ボタンを活性にする
    '        Master.ppRigthButton10.Enabled = True

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
    '    End Try

    'End Sub

#End Region

#Region "そのほかのプロシージャ"
#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
