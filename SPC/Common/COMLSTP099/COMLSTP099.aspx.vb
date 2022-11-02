'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　ダウンロード
'*　ＰＧＭＩＤ：　COMLSTP099
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.20　：　ＮＫＣ
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'COMLSTP099-001     2015/06/10      加賀      DLチェック更新ボタン、更新処理追加              
'COMLSTP099-002     2017/10/10      伯野      各プロシージャにデータセット破棄を記述              

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
Imports DBFTP.ClsDBFTP_Main
Imports DBFTP.ClsFTPCnfg
Imports DBFTP.ClsSQLSvrDB
Imports System.Security.AccessControl
'Imports clscomver

#End Region

Public Class COMLSTP099

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
    Const M_DISP_ID = P_FUN_COM & P_SCR_LST & P_PAGE & "099"    'プログラムID

    Const M_FIRST = "1"
    Const M_RESTART = "2"

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
    Dim DBFTP As New DBFTP.ClsDBFTP_Main
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
    ''' Page_Init
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, M_DISP_ID)

    End Sub

    ''' <summary>
    ''' Page_Load
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

            '更新ボタンクリックイベント設定
            AddHandler Master.ppRigthButton1.Click, AddressOf btnReStart_Click
            AddHandler Master.ppRigthButton2.Click, AddressOf btnUpdCheck_Click 'COMLSTP099-001

            If Not IsPostBack Then  '初回表示

                Dim strUser(2) As String                            '0:ユーザ権限(0),1: 起動場所
                Dim strSession(2 - 1) As String                     '0:画面名 1:値

                '開始ログ出力
                psLogStart(Me)

                'ユーザの権限、場所を取得
                strUser(0) = Session(P_SESSION_BCLIST)
                strUser(1) = Session(P_SESSION_BCLIST)

                '権限、場所をビューステートに格納
                'Me.ViewState.Add(P_KENGEN, strUser)

                '引継ぎ値を設定
                strSession = Session(P_SESSION_NGC_MEN)
                Me.ViewState.Add(P_SESSION_NGC_MEN, strSession)
                '画面名を表示
                Master.ppProgramID = M_DISP_ID
                Master.ppTitle = strSession(0) & clsCMDBC.pfGet_DispNm(M_DISP_ID)

                'パンくずリスト
                Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

                'コントロールの初期化
                msClearScreen()

                '一覧表示処理
                ms_GetData()

                '終了ログ出力
                psLogEnd(Me)

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
            'システムエラー
            psClose_Window(Me)

        End Try

    End Sub

    ''' <summary>
    ''' Grid_DataBound COMLSTP099-001
    ''' </summary>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        'DLチェックボックスにデータ反映
        For Each rowData As GridViewRow In grvList.Rows
            If CType(rowData.FindControl("DLフラグ"), TextBox).Text = "1" Then
                CType(rowData.FindControl("CHECK_BOX"), CheckBox).Checked = True
            End If
        Next

    End Sub

#End Region

#Region "ユーザー権限"
    '---------------------------
    '2014/04/21 武 ここから
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
            Case "SPC"
            Case "営業所"
            Case "NGC"
        End Select

    End Sub
    '---------------------------
    '2014/04/21 武 ここまで
    '---------------------------
#End Region

#Region "ボタンクリックイベント"

    ''' <summary>
    ''' 更新ボタンクリック
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnReStart_Click()

        '開始ログ出力
        psLogStart(Me)

        '検索処理開始
        ms_GetData()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 更新ボタンクリック COMLSTP099-001
    ''' </summary>
    Protected Sub btnUpdCheck_Click()
        '開始ログ出力
        psLogStart(Me)

        Dim aryKey As ArrayList = New ArrayList 'キー情報保存配列
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing

        Try
            'チェック有無、キー情報を収集
            For Each rowData As GridViewRow In grvList.Rows
                aryKey.Add({(CType(CType(rowData.FindControl("CHECK_BOX"), CheckBox).Checked, Integer) * -1).ToString, _
                             CType(rowData.FindControl("管理番号"), TextBox).Text, _
                             CType(rowData.FindControl("ファイル種別"), TextBox).Text})
            Next
        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '失敗メッセージ表示
            'psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "チェック内容")
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "更新")
            Exit Sub
        End Try

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Using conTrn = conDB.BeginTransaction
                Try
                    'UPDATE
                    For Each strKey() As String In aryKey
                        cmdDB = New SqlCommand("COMLSTP099_U1", conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = conTrn
                        With cmdDB.Parameters   'パラメータ設定
                            .Add(pfSet_Param("DL_flg", SqlDbType.NVarChar, strKey(0)))              'DLフラグ
                            .Add(pfSet_Param("mng_no", SqlDbType.NVarChar, strKey(1)))              '管理番号
                            .Add(pfSet_Param("file_cls", SqlDbType.NVarChar, strKey(2)))            'ファイル種別
                            .Add(pfSet_Param("user_id", SqlDbType.NVarChar, User.Identity.Name))    'ユーザー
                            .Add(pfSet_Param("disp_dt", SqlDbType.DateTime, ViewState("GetDate")))  '取得日時
                            .Add(pfSet_Param("retvalue", SqlDbType.SmallInt))                       '戻り値
                        End With
                        '実行
                        cmdDB.ExecuteNonQuery()
                        '戻り値チェック
                        If cmdDB.Parameters("retvalue").Value.ToString <> 0 Then
                            'ロールバック
                            conTrn.Rollback()
                            '失敗メッセージ表示
                            'psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "チェック内容")
                            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "更新")
                        End If
                    Next
                    '完了メッセージ表示
                    'psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "チェック内容", "チェック内容")
                    psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "更新", "更新")
                    'コミット
                    conTrn.Commit()
                Catch ex As Exception
                    'ロールバック
                    conTrn.Rollback()
                    '失敗メッセージ表示
                    'psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "チェック内容")
                    psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "更新")
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                End Try
            End Using
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

        '検索処理開始
        ms_GetData()

        '終了ログ出力
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' ダウンロードボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        If e.CommandName <> "Sort" Then

            Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    'ボタン押下行のIndex
            Dim rowData As GridViewRow = grvList.Rows(intIndex)             'ボタン押下行
            Dim strErrmsg As String = Nothing                               '置き換えエラーメッセージ
            Dim strDownLoadfile As String = Nothing                         'ダウンロード対象ファイル名
            Dim strDownLoadPath As String = Nothing                         '保管場所
            Dim strFullPath As String = Nothing                             'フルパス
            Dim opblnResult As Boolean = False
            Dim splPath As String() = Nothing
            Dim localFileName As String = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString
            Dim strfileName As String = Nothing
            Dim localdirName As String = "DOWNLOAD"
            Dim localFiledir As String = "C:"
            Dim strLocalPath As String = localFiledir & "/" & localdirName & "/"
            Dim filePath2 As String = Nothing
            Dim Fullpath As HttpPostedFile = Nothing
            Dim strExtension As String = Nothing
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            objStack = New StackFrame
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------

            '開始ログ出力
            psLogStart(Me)

            Try
                'ファイル名、保管場所を取得
                strDownLoadfile = CType(rowData.FindControl("ファイル名"), TextBox).Text
                strDownLoadPath = CType(rowData.FindControl("保管フォルダ"), TextBox).Text
                strFullPath = "\\" + strDownLoadPath + "\" + strDownLoadfile
                strFullPath = strFullPath.Replace("/", "\")
                splPath = strFullPath.Split("\")
                For spl = 3 To splPath.Count - 2
                    filePath2 &= splPath(spl) & "\"
                Next

                strfileName = splPath(splPath.Count - 1)

                'splPath(0) = splPath(3) & "/" & splPath(4) & "/"
                'ファイルの存在確認
                If DBFTP.pfFtpFile_Exists(filePath2, strfileName, opblnResult) = False Then

                    'ファイルが存在しない
                    strErrmsg = strDownLoadfile
                    Throw New Exception

                End If

                '拡張子の取得
                strExtension = Path.GetExtension(strfileName)
                localFileName = localFileName & strExtension
                'ローカルにフォルダがなかった場合、作成する
                If Directory.Exists(strLocalPath) = False Then
                    System.IO.Directory.CreateDirectory(strLocalPath)
                End If

                'ローカルにダウンロードを開始する
                DBFTP.pfFtpFile_Copy("GET", filePath2, splPath(splPath.Count - 1), opblnResult, localdirName & "/" & localFileName)
                'btnDownload_Start(strDownLoadfile)

                'ダウンロードファイル存在確認(保存先)
                If Not File.Exists(strLocalPath & localFileName) Then
                    'ファイルが存在しない
                    psMesBox(Me, "30003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strfileName)
                    Exit Sub
                End If

                'パスの再設定
                strLocalPath = strLocalPath & localFileName

                'ダウンロード
                Response.Redirect("~/Common/COMLSTP099/COMLSTP099.ashx?path=" & strLocalPath & "&filename=" & HttpUtility.UrlEncode(strfileName), False)

                'ローカルに一時的に作成したファイルを削除
                ''ファイルの存在を確認
                'If System.IO.File.Exists(strLocalPath) Then
                '    System.IO.File.Delete(strLocalPath)
                'End If

            Catch ex As Exception

                If strErrmsg = Nothing Then

                    'システムエラー
                    psMesBox(Me, "30010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, ex.ToString)

                Else

                    'その他エラー
                    psMesBox(Me, "30003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strErrmsg)

                End If
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------

            Finally
                'ローカルに一時的に作成したファイルを削除
                ''ファイルの存在を確認
                'If System.IO.File.Exists(strLocalPath) Then
                '    System.IO.File.Delete(strLocalPath)
                'End If
                '終了ログ出力
                psLogEnd(Me)

            End Try

        End If

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' コントロールの初期化
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub msClearScreen()

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '開始ログ出力
        psLogStart(Me)

        Try

            'ボタンのプロパティ設定
            Master.ppRigthButton1.Enabled = True
            'Master.ppRigthButton1.Text = P_BTN_NM_UPD
            Master.ppRigthButton1.Text = "再表示"  'COMLSTP099-001
            'Master.ppRigthButton1.Visible = True
            'COMLSTP099-001
            Master.ppRigthButton2.Enabled = True
            Master.ppRigthButton2.Text = "更新"
            Master.ppRigthButton2.Visible = True
            'COMLSTP099-001 END

            'グリッドの初期化
            Me.grvList.DataSource = New DataTable
            '変更を反映
            Me.grvList.DataBind()

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
            Throw ex

        Finally

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub

    ''' <summary>
    ''' 一覧表示処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ms_GetData()

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim strOKNG As String = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '開始ログ出力
        psLogStart(Me)

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else

            Try
                Dim strSerch As String = Nothing
                Dim strPrintList As String = Nothing
                Dim strView() As String = Me.ViewState(P_SESSION_NGC_MEN)

                'ファイル種別の切り替え
                strPrintList = ms_changePrintList(strView(1))
                If strPrintList Is Nothing Then

                    Throw New Exception

                End If

                objCmd = New SqlCommand(M_DISP_ID + "_S1", objCn)
                With objCmd.Parameters

                    '--パラメータ設定
                    .Add(pfSet_Param("prm_pageID", SqlDbType.NVarChar, M_DISP_ID))                      '画面ID
                    .Add(pfSet_Param("prm_fileNum", SqlDbType.NVarChar, strPrintList))                  'ファイル種別
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))  '結果取得用(0:データなし、1:データあり)
                    .Add(pfSet_Param("Get_Date", SqlDbType.DateTime, 50, ParameterDirection.Output))    '取得日時

                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '取得日時
                ViewState("GetDate") = objCmd.Parameters("Get_Date").Value

                '結果情報を取得
                strOKNG = objCmd.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "0"         'データ無し

                        'グリッドの初期化
                        Me.grvList.DataSource = New DataTable
                        '変更を反映
                        Me.grvList.DataBind()

                    Case Else        'データ有り

                        '取得したデータをグリッドに設定
                        Me.grvList.DataSource = objDs.Tables(0)

                        '変更を反映
                        Me.grvList.DataBind()

                End Select

            Catch ex As SqlException

                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ダウンロードファイル")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------

            Catch ex As Exception

                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ダウンロードファイル")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------

            Finally

                'データセット破棄
                Call ClsCMDataConnect.psDisposeDataSet(objDs)

                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

                '終了ログ出力
                psLogEnd(Me)

            End Try

        End If
    End Sub

    ''' <summary>
    ''' ファイル種別切り替え
    ''' </summary>
    ''' <param name="strSelect"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function ms_changePrintList(ByVal strSelect As String)

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            Dim strResult As String = Nothing

            '遷移元の判断 検索条件を変更

            Select Case strSelect

                Case "2"     '返却品一覧

                    strResult = "0336PN,0333PN,0334PN"   '返却品一覧表,ＳＣ・ＣＣ・ＲＳＣ返却品集計表

                Case "7"     '工事料金明細

                    strResult = "0271PN"          '機器代金及び工事料金明細書

                Case "8"     '工事料金明細一覧

                    strResult = "0251PN,0252PN"   '機器代金及び工事料金明細書一覧（明細）,機器代金及び工事料金明細書一覧（合計）

                Case "11"    'ＢＢ１調査依頼一覧

                    strResult = "0903CL"          'IC2次ﾌﾞﾗｯｸﾎﾞｯｸｽ調査報告書

                Case "30"    '工事完了報告書兼検収書

                    strResult = "0151FC"          '工事完了報告書兼検収書

                Case "40"    '保守完了報告書

                    strResult = "0154FC"          '保守完了報告書

            End Select

            Return strResult

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
            Return Nothing

        End Try

    End Function

    ''' <summary>
    ''' ダウンロード
    ''' </summary>
    ''' <param name="fileName"></param>
    ''' <param name="failepath"></param>
    ''' <remarks></remarks>
    Protected Sub btnDownload_Start(ByVal fileName As String, ByVal failepath As String)

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '開始ログ出力
        psLogStart(Me)
        Try
            'If fileName <> "" Then
            '    Response.ContentType = "application/octet-stream"
            '    Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName))
            '    Response.Flush()
            '    Response.WriteFile(failepath)
            '    Response.End()
            'End If

            Response.Redirect("~/Common/COMLSTP099/COMLSTP099.ashx?path=" & failepath & "&filename=" & HttpUtility.UrlEncode(fileName), False)

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
            Throw ex

        Finally

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
