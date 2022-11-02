'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜監視＞
'*　処理名　　：　DLL設定変更履歴
'*　ＰＧＭＩＤ：　DLLHSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2016/10/11　：　加賀　　　XXX
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'DLLHSTP001-000     2016/XX/XX      XXXX　　　XXXX

#Region "インポート定義"

Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon

#End Region

Public Class DLLHSTP001

#Region "継承定義"

    Inherits System.Web.UI.Page

#End Region

#Region "定数定義"

    'プログラムＩＤ
    Const M_DISP_ID = "DLLHSTP001"

#End Region

#Region "変数定義"

    Dim objStack As StackFrame
    Dim clsCMDBC As New ClsCMDBCom

#End Region


#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page_Initイベント
    ''' </summary>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim strWrkCD As String = Request.QueryString.Get("WrkCD")
        Dim intGrvRowHeight As Integer

        '作業コード別にグリッド行の高さを変更
        Select Case strWrkCD
            Case "08"       '受入禁止券種設定
                intGrvRowHeight = 86
            Case "03", "12" '終夜, 他店取込
                intGrvRowHeight = 43
            Case Else
                intGrvRowHeight = 28
        End Select

        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, M_DISP_ID, intGrvRowHeight, 11)

    End Sub

    ''' <summary>
    ''' Page_Loadイベント
    ''' </summary>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        objStack = New StackFrame

        Try
            '初回表示
            If Not IsPostBack Then

                'セッションから情報を取得
                ViewState(P_KEY) = Session(P_KEY)   '0：DLL管理番号  1：作業コード  2：履歴ﾎﾞﾀﾝ押下時刻

                '画面ヘッダ設定
                With Master
                    .ppProgramID = M_DISP_ID
                    .ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)
                    .ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), .ppTitle)    'パンくずリスト設定
                    .ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる
                End With

                '設定内容取得・表示
                getDLLInfo()

                '一覧データ取得・表示
                getListData()

            End If

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後) '画面の初期化に失敗しました。

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' リロードボタン押下時
    ''' </summary>
    Protected Sub btnReload_Click(sender As Object, e As EventArgs) Handles btnReload.Click

        Try

            '一覧データ取得・表示
            getListData()

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "リロード")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 設定内容 取得・表示
    ''' </summary>
    Private Sub getDLLInfo()

        Dim strKey() As String = ViewState(P_KEY)
        Dim dstSelect As New DataSet
        Dim strStoredNm As String = "DLLHSTP001_S01"

        '設定内容の取得
        Try
            'SQLコマンド設定
            Using cmdSQL As SqlCommand = New SqlCommand("SPCDB.dbo." + strStoredNm)

                'パラメータ設定
                cmdSQL.Parameters.Add(pfSet_Param("prm_DLLSEND_NO", SqlDbType.NVarChar, strKey(0)))
                cmdSQL.Parameters.Add(pfSet_Param("prm_SEQ_NO", SqlDbType.NVarChar, strKey(1)))

                'データ取得実行
                If ClsCMDataConnect.pfExec_StoredProcedure(Me, "DLL設定内容", cmdSQL, dstSelect) = False Then
                    Exit Sub
                End If

            End Using

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "DLL設定内容の取得")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try


        '設定内容の表示
        Try
            If dstSelect Is Nothing _
            OrElse dstSelect.Tables.Count < 1 _
            OrElse dstSelect.Tables(0).Rows.Count < 1 Then

                '該当するデータが存在しません。
                psMesBox(Me, "00007", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)

            Else

                Dim drwSelect As DataRow = dstSelect.Tables(0).Rows(0)

                Me.lbl_TboxId.Text = drwSelect("TBOXID").ToString
                Me.lbl_System.Text = drwSelect("システム").ToString
                Me.lbl_NLCls.Text = drwSelect("ＮＬ区分").ToString
                Me.lbl_HallNm.Text = drwSelect("ホール名").ToString
                Me.lbl_DLLWRK.Text = drwSelect("設定依頼内容").ToString
                Me.hdn_NLCls.Value = drwSelect("CLS_NL").ToString

                'Dispose
                dstSelect.Dispose()

            End If

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "DLL設定内容の表示")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

    End Sub

    ''' <summary>
    ''' 一覧情報 取得・表示
    ''' </summary>
    Private Sub getListData()

        Dim strKey() As String = ViewState(P_KEY)
        Dim dstSelect As New DataSet
        Dim strStoredNm As String = "DLLHSTP001_S02"

        '一覧情報の取得
        Try

            'SQLコマンド設定
            Using cmdSQL As SqlCommand = New SqlCommand("SPCDB.dbo." + strStoredNm)

                'パラメータ設定
                cmdSQL.Parameters.Add(pfSet_Param("prm_DLLSEND_NO", SqlDbType.NVarChar, strKey(0)))
                cmdSQL.Parameters.Add(pfSet_Param("prm_SEQ_NO", SqlDbType.NVarChar, strKey(1)))
                cmdSQL.Parameters.Add(pfSet_Param("prm_NL_CLS", SqlDbType.NVarChar, hdn_NLCls.Value))

                'データ取得実行
                If ClsCMDataConnect.pfExec_StoredProcedure(Me, "履歴情報一覧", cmdSQL, dstSelect) = False Then

                    'エラーメッセージ表示
                    psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "履歴情報一覧の取得")

                    Exit Sub

                End If

            End Using

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "履歴情報一覧の取得")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        '一覧情報の表示
        Try
            If dstSelect Is Nothing _
            OrElse dstSelect.Tables.Count < 1 _
            OrElse dstSelect.Tables(0).Rows.Count < 1 Then

                '一覧にバインド
                grvList.DataSource = New DataTable
                grvList.DataBind()

                '該当件数を表示
                Me.lblCount.Text = "0"

                '該当するデータが存在しません。
                psMesBox(Me, "00007", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)

            Else

                '[<][>]文字化け対策
                For Each drw As DataRow In dstSelect.Tables(0).Rows
                    drw.Item("配信設定内容") = drw.Item("配信設定内容").ToString.Replace("&lt;", "＜").Replace("&gt;", "＞")
                Next

                '変更をコミット
                dstSelect.Tables(0).AcceptChanges()

                '一覧にバインド
                grvList.DataSource = dstSelect.Tables(0)
                grvList.DataBind()

                '該当件数、表示件数を表示
                Dim strRecordCnt, strDispCnt As String
                strRecordCnt = dstSelect.Tables(0).Rows(0)("該当件数").ToString
                strDispCnt = dstSelect.Tables(0).Rows(0)("表示件数").ToString

                '該当件数を表示
                Me.lblCount.Text = strRecordCnt

                '閾値を超えた場合はメッセージを表示
                If Integer.Parse(strRecordCnt) > Integer.Parse(strDispCnt) Then
                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, strRecordCnt, strDispCnt)
                End If

                'Dispose
                dstSelect.Dispose()

            End If

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "履歴情報一覧の表示")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

#End Region

End Class
