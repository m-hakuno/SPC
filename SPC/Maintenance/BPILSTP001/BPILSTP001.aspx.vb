'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　玉単価設定情報一覧
'*　ＰＧＭＩＤ：　BPILSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.17　：　ＸＸＸ
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'BPILSTP001-001     2016/06/23      加賀　　　[e-basカスタマイズ]帳票をCSVから出力するように変更
'BPILSTP001-002     2017/09/06      加賀　　　ホール機器設定情報から出力可能に変更　→　一時削除　ていうか削除
'BPILSTP001-003     2017/10/27      伯野　　　ｇｒｉｄｖｉｅｗのコントロールをテキストボックスからラベルに変更

#Region "インポート定義"

Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
'Imports DBFTP.ClsDBFTP_Main    'BPILSTP001-001
'Imports DBFTP.ClsFTPCnfg       'BPILSTP001-001
'Imports DBFTP.clsLogwrite
'Imports DBFTP.ClsSQLSvrDB      'BPILSTP001-001
Imports System.Security.AccessControl
Imports System.IO

#End Region

Public Class BPILSTP001

#Region "継承定義"

    Inherits System.Web.UI.Page

#End Region

#Region "定数定義"

    'プログラムID
    Const M_MY_DISP_ID = P_FUN_BPI & P_SCR_LST & P_PAGE & "001"

#End Region

#Region "変数定義"

    Dim objStack As StackFrame
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom
    Dim DBFTP As New DBFTP.ClsDBFTP_Main

#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page_Initイベント
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, M_MY_DISP_ID, "L")

    End Sub

    ''' <summary>
    ''' Page_Loadイベント
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click
        objStack = New StackFrame

        Try
            If Not IsPostBack Then  '初回表示のみ

                '検索条件クリアボタン押下時の検証を無効
                Master.ppRigthButton2.CausesValidation = False

                '画面設定
                Master.Master.ppProgramID = M_MY_DISP_ID
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)
                Master.Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる

                'パンくずリスト設定
                Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                'セッション変数「グループナンバー」が存在しない場合、画面を閉じる
                If Session(P_SESSION_GROUP_NUM) Is Nothing Then
                    psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    psClose_Window(Me)
                    Return
                End If

                'ViewStateに「グループナンバー」を保存
                ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                '画面クリア
                msClear_Screen()
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()
                Master.ppCount = "0"
            Else
                'ViewState「グループナンバー」が存在しない場合、画面を閉じる
                If ViewState(P_SESSION_GROUP_NUM) Is Nothing Then
                    psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    psClose_Window(Me)
                    Return
                End If
            End If

        Catch ex As Exception
            '画面初期化エラー
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
    End Sub


    ''' <summary>
    ''' 検索ボタンクリック時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        'データ取得
        If (Page.IsValid) Then
            msGet_Data("0")
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 検索条件クリアボタンクリック時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        '画面クリア
        msClear_Screen()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' グリッドのボタン押下時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        If e.CommandName = "Sort" Then
            Exit Sub
        End If

        '開始ログ出力
        psLogStart(Me)

        Try
            Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    'ボタン押下行のIndex
            Dim rowData As GridViewRow = grvList.Rows(intIndex)             'ボタン押下行
            Dim strPrm() As String = {CType(rowData.FindControl("TBOXID"), Label).Text _
                                    , CType(rowData.FindControl("ホール名"), Label).Text _
                                    , CType(rowData.FindControl("取得日"), Label).Text _
                                    , CType(rowData.FindControl("フォルダパス"), Label).Text _
                                    , CType(rowData.FindControl("ファイル名"), Label).Text _
                                    , Path.GetExtension(CType(rowData.FindControl("ファイル名"), Label).Text)}

            'If mfGet_BPIInfo(strPrm(0), strPrm(1), strPrm(2), strPrm(3), strPrm(4), strPrm(5), dtsDSUData) Then
            '    'ＰＤＦデータ取得処理
            '    dttData = dtsDSUData.Tables(0)
            '    objRpt = New BPIREP002

            '    If strPrm(3) = ".csv" Then
            '        'ファイル出力
            '        psPrintPDF(Me, objRpt, dttData, "貸玉数　設定情報一覧")
            '    End If
            'End If

            'BPILSTP001-001
            Select Case strPrm(5)
                Case ".pdf"
                    'ファイルパス整形
                    strPrm(3) = strPrm(3) & "\" & strPrm(4).Split("/")(0) & "\"
                    strPrm(4) = strPrm(4).Split("/")(1)

                    'PDF取得
                    If Get_PDF(strPrm(0), strPrm(3), strPrm(4)) = False Then
                        Exit Sub
                    End If

                Case ".csv"
                    Dim objRpt As New BPIREP002         'レポートクラス
                    Dim lstCSV As New List(Of String()) 'CSVデータ
                    Dim dtsReport As DataSet = Nothing  '印刷用データ

                    'CSVデータ取得
                    If Get_CSVData(strPrm(0), strPrm(3), strPrm(4), lstCSV) = False Then
                        Exit Sub
                    End If

                    '印刷用データ取得
                    If Get_ReportData(lstCSV, dtsReport) = False Then
                        Exit Sub
                    End If

                    '帳票出力
                    If dtsReport.Tables.Count = 0 Then
                        psMesBox(Me, "00007", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後) '印刷対象データがありません。
                        Exit Sub
                    Else
                        psPrintPDF(Me, objRpt, dtsReport.Tables(0), "貸玉数　設定情報一覧")
                    End If

                    'Case Else   ''BPILSTP001-002

                    '    Dim objRpt As New BPIREP002         'レポートクラス
                    '    Dim dtsReport As DataSet = Nothing  '印刷用データ

                    '    'SQLコマンド設定
                    '    Using cmdSQL As SqlCommand = New SqlCommand("SPCDB.dbo.BPILSTP001_S3")

                    '        cmdSQL.Parameters.Add(pfSet_Param("tboxid", SqlDbType.NVarChar, strPrm(0)))

                    '        '印刷用データ取得
                    '        If pfExec_StoredProcedure(Me, "ホール機器設定情報", cmdSQL, dtsReport) = False Then
                    '            Exit Sub
                    '        End If

                    '    End Using

                    '    '帳票出力
                    '    If dtsReport.Tables.Count = 0 OrElse dtsReport.Tables(0).Rows.Count = 0 Then
                    '        psMesBox(Me, "00007", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後) '印刷対象データがありません。
                    '        Exit Sub
                    '    Else
                    '        psPrintPDF(Me, objRpt, dtsReport.Tables(0), "貸玉数　設定情報一覧")
                    '    End If

            End Select
            'BPILSTP001-001 END

        Catch ex As Exception
            'エラー
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "貸玉単価設定確認リスト")       '{0}の出力に失敗しました。
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClear_Screen()

        Me.tftTboxId.ppFromText = String.Empty              'ＴＢＯＸＩＤ
        Me.tftTboxId.ppToText = String.Empty
        Me.dftGetDt.ppFromText = String.Empty              '対応日
        Me.dftGetDt.ppToText = String.Empty
        Me.tftTboxId.ppTextBoxFrom.Focus()
        'Me.grvList.DataSource = New DataTable
        'Me.grvList.DataBind()
        'Master.ppCount = "0"

    End Sub

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <param name="ipstrDefault">初期表示フラグ（1：初期　1以外：初期以外）</param>
    ''' <remarks></remarks>
    Private Sub msGet_Data(ByVal ipstrDefault As String)

        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット
        objStack = New StackFrame

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_S1", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    'ＴＢＯＸＩＤFrom
                    .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, Me.tftTboxId.ppFromText))
                    'ＴＢＯＸＩＤTo
                    .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, Me.tftTboxId.ppToText))
                    '対応日From
                    .Add(pfSet_Param("get_dt_f", SqlDbType.NVarChar, Me.dftGetDt.ppFromText))
                    '対応日To
                    .Add(pfSet_Param("get_dt_t", SqlDbType.NVarChar, Me.dftGetDt.ppToText))
                    '画面ＩＤ
                    .Add(pfSet_Param("disp_id", SqlDbType.NVarChar, M_MY_DISP_ID))
                    '初期表示フラグ
                    .Add(pfSet_Param("default", SqlDbType.NVarChar, ipstrDefault))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ有無確認
                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    '件数を設定
                    Master.ppCount = "0"
                Else
                    '閾値を超えた場合はメッセージを表示
                    If dstOrders.Tables(0).Rows(0)("データ件数") > dstOrders.Tables(0).Rows.Count Then
                        psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                            dstOrders.Tables(0).Rows(0)("データ件数").ToString, dstOrders.Tables(0).Rows.Count.ToString)
                    End If
                    '件数を設定
                    Master.ppCount = dstOrders.Tables(0).Rows(0)("データ件数").ToString
                End If

                '取得したデータをグリッドに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "玉単価設定情報")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()
                Master.ppCount = "0"
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"
        End If

    End Sub

    ''' <summary>
    ''' PDFファイル取得、ダウンロード 'BPILSTP001-001
    ''' </summary>
    ''' <param name="strTBOXID">TBOXID</param>
    ''' <param name="strFTP_DirPath">CSVファイルの保存先パス</param>
    ''' <param name="strFTP_FileName">CSVファイル名</param>
    ''' <remarks>FTPでGETしたPDFをダウンロードします</remarks>
    Private Function Get_PDF(ByVal strTBOXID As String, ByVal strFTP_DirPath As String, ByVal strFTP_FileName As String) As Boolean

        Get_PDF = False
        objStack = New StackFrame

        Try
            Dim DBFTP As New DBFTP.ClsDBFTP_Main
            Dim strLocal_FilePath As String = "DOWNLOAD\KASHITAMA_" & strTBOXID & "_" & Date.Now.ToString("yyyyMMddHHmmss") & ".pdf"

            'ファイルの存在確認
            If DBFTP.pfFtpFile_Exists(strFTP_DirPath, strFTP_FileName, True) = False Then
                'エラーメッセージ表示
                psMesBox(Me, "30003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【FTPエラー】" & strFTP_FileName)   '{0}ファイルは存在しません。
                Exit Function
            End If

            'PDFファイルをFTP-GET 
            If DBFTP.pfFtpFile_Copy("GET", strFTP_DirPath, strFTP_FileName, True, strLocal_FilePath) = False Then
                'エラーメッセージ表示
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【FTPエラー】" & "PDFファイル取得の取得")
                Exit Function
            End If

            'PDFをダウンロード
            Response.Redirect("~/Common/COMLSTP099/COMLSTP099.ashx?path=" & "C:\" & strLocal_FilePath & "&filename=" & HttpUtility.UrlEncode(strFTP_FileName), False)

            'ローカルの一時ファイルを削除
            Dim strFiles() As String = Directory.GetFiles("C:\DOWNLOAD")
            For Each FilePath As String In strFiles
                '一昨日以前のファイルを削除
                If FilePath.IndexOf("KASHITAMA") >= 0 _
                AndAlso FilePath.IndexOf(Date.Now.ToString("yyyyMMdd")) = -1 _
                AndAlso FilePath.IndexOf(Date.Now.AddDays(-1).ToString("yyyyMMdd")) = -1 Then
                    File.Delete(FilePath)
                End If
            Next

            Get_PDF = True

        Catch ex As Exception
            'エラー
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "PDFファイル取得の取得")       '{0}に失敗しました。
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Function

    ''' <summary>
    ''' CSVファイル取得 'BPILSTP001-001
    ''' </summary>
    ''' <param name="strTBOXID">TBOXID</param>
    ''' <param name="strFTP_DirPath">CSVファイルの保存先パス</param>
    ''' <param name="strFTP_FileName">CSVファイル名</param>
    ''' <param name="lstCSV">取得したCSVデータを格納するList</param>
    ''' <remarks>FTPでGETしたCSVをLISTに格納します</remarks>
    Private Function Get_CSVData(ByVal strTBOXID As String, ByVal strFTP_DirPath As String, ByVal strFTP_FileName As String, ByRef lstCSV As List(Of String())) As Boolean

        Get_CSVData = False
        objStack = New StackFrame

        Try
            Dim DBFTP As New DBFTP.ClsDBFTP_Main
            Dim strLocal_FilePath As String = "DOWNLOAD\KASHITAMA_" & strTBOXID & "_" & Date.Now.ToString("yyyyMMddHHmmss")

            'ファイルの存在確認
            If DBFTP.pfFtpFile_Exists(strFTP_DirPath, strFTP_FileName, True) = False Then
                'エラーメッセージ表示
                psMesBox(Me, "30003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【FTPエラー】" & strFTP_FileName)   '{0}ファイルは存在しません。
                Exit Function
            End If

            'CSVファイルをFTP-GET 
            If DBFTP.pfFtpFile_Copy("GET", strFTP_DirPath, strFTP_FileName, True, strLocal_FilePath) = False Then
                'エラーメッセージ表示
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【FTPエラー】" & "CSVファイルの取得")
                Exit Function
            End If

            'CSVファイルをリストに変換
            lstCSV = pfReadCsvFile("C:\" & strLocal_FilePath)
            If lstCSV Is Nothing Then
                'エラーメッセージ表示
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSVファイルの読み込み")
                Exit Function
            End If

            'ローカルの一時ファイルを削除
            If File.Exists("C:\" & strLocal_FilePath) Then
                File.Delete("C:\" & strLocal_FilePath)
            End If

            Get_CSVData = True

        Catch ex As Exception
            'エラー
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSVファイル取得の取得")       '{0}に失敗しました。
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Function

    ''' <summary>
    ''' CSVファイル、DBから印刷用データを作成 'BPILSTP001-001
    ''' </summary>
    ''' <remarks>CSVデータとDBの情報を結合して印刷用DataTableを作成します</remarks>
    Private Function Get_ReportData(ByVal lstCSV As List(Of String()), ByRef dstSelect As DataSet) As Boolean

        Get_ReportData = False
        objStack = New StackFrame

        Try
            'ストアド引数用DataTable作成
            Dim dtTmp As New DataTable
            Dim drTmp As DataRow

            'カラム設定
            With dtTmp.Columns
                .Add("TBOXID")
                .Add("TBOX_VER")
                .Add("運用機番")
                .Add("JB番号")
                .Add("島番号")
                .Add("BB種別コード")
                .Add("貸出金額")
                .Add("貸玉数")
                .Add("貸玉単価")
                .Add("自動払出")
                .Add("貸玉単価表示")
                .Add("ストック玉動作")
                .Add("方式")
            End With

            'List→DataTable変換
            For Each strCSV() As String In lstCSV
                'レコード作成
                drTmp = dtTmp.NewRow
                With drTmp
                    .Item("TBOXID") = strCSV(9)
                    .Item("TBOX_VER") = strCSV(12).ToString & strCSV(13).ToString
                    .Item("運用機番") = strCSV(15)
                    .Item("JB番号") = strCSV(14)
                    .Item("島番号") = strCSV(20)
                    .Item("BB種別コード") = strCSV(16) '?
                    .Item("貸出金額") = strCSV(33)
                    .Item("貸玉数") = strCSV(34)
                    .Item("貸玉単価") = strCSV(35).ToString & "." & strCSV(36).ToString
                    .Item("自動払出") = strCSV(37)
                    .Item("貸玉単価表示") = strCSV(38)
                    .Item("ストック玉動作") = strCSV(39)
                    .Item("方式") = strCSV(40)
                End With

                '作成したレコードをDataTableに追加
                dtTmp.Rows.Add(drTmp)
            Next

            Const strStoredNm As String = "BPILSTP001_S2"
            Using SqlCmd As SqlCommand = New SqlCommand("SPCDB.dbo." + strStoredNm)
                'パラメータ設定
                SqlCmd.Parameters.Add(pfSet_Param("KASHITAMA_CSV", SqlDbType.Structured, dtTmp))

                '実行
                If pfExec_StoredProcedure(Me, "印刷用データ", SqlCmd, dstSelect) = False Then
                    Exit Function
                End If
            End Using

            'DataTable破棄
            dtTmp.Dispose()

            Return True

        Catch ex As Exception
            'エラー
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "印刷用データの取得")       '{0}に失敗しました。
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Function

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
