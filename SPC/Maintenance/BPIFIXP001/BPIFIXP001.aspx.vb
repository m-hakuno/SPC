'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　貸玉数　設定情報差異確認
'*　ＰＧＭＩＤ：　BPIFIXP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.17　：　ＸＸＸ
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'BPIFIXP001-001     2016/08/08      加賀　　　[e-basカスタマイズ]

#Region "インポート定義"

Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax

#End Region

Public Class BPIFIXP001

#Region "継承定義"
    Inherits System.Web.UI.Page
#End Region

#Region "定数定義"
    'プログラムID
    Const M_MY_DISP_ID = P_FUN_BPI & P_SCR_FIX & P_PAGE & "001"
#End Region

#Region "変数定義"
    Dim objStack As StackFrame
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom

#End Region


#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page_Initイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, M_MY_DISP_ID, M_MY_DISP_ID + "_Header", Me.DivOut)

    End Sub

    ''' <summary>
    ''' Page_Loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        objStack = New StackFrame

        Try
            If Not IsPostBack Then  '初回表示のみ

                '検索ボタンと検索条件クリアボタンを非表示
                Master.ppRigthButton1.Visible = False
                Master.ppRigthButton2.Visible = False

                '画面設定
                Master.Master.ppProgramID = M_MY_DISP_ID
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)
                Master.Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる

                'パンくずリスト設定
                Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                ''セッション変数「グループナンバー」「キー情報」が存在しない場合、画面を閉じる
                'If Session(P_SESSION_GROUP_NUM) Is Nothing Or Session(P_KEY) Is Nothing Then
                '    psMesBox(Me, "20004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画前)
                '    psClose_Window(Me)
                '    Return
                'End If
                'セッション変数「キー情報」が存在しない場合、画面を閉じる
                If Session(P_KEY) Is Nothing Then
                    psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    psClose_Window(Me)
                    Return
                End If

                'ViewStateに「グループナンバー」「キー情報」を保存
                ViewState(P_KEY) = Session(P_KEY)
                ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                '画面クリア
                msClear_Screen()

                'TBOXデータ取得
                If msGet_TboxData() = False Then
                    Exit Sub
                End If

                '一覧データ取得
                msGet_ListData()

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
    ''' データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        Dim strDifference As String

        Try
            For Each rowData As GridViewRow In grvList.Rows
                strDifference = CType(rowData.FindControl("差異有無"), TextBox).Text
                If strDifference = "有" Then
                    'BPIFIXP001-001
                    For Each cell As TableCell In rowData.Cells
                        If TypeOf cell.Controls(0) Is TextBox Then
                            DirectCast(cell.Controls(0), TextBox).ForeColor = Drawing.Color.Red '赤字設定
                        End If
                    Next
                    'CType(rowData.FindControl("運用機番"), TextBox).ForeColor = Drawing.Color.Red
                    'CType(rowData.FindControl("ＪＢ番号"), TextBox).ForeColor = Drawing.Color.Red
                    'CType(rowData.FindControl("機器"), TextBox).ForeColor = Drawing.Color.Red
                    'CType(rowData.FindControl("差異有無"), TextBox).ForeColor = Drawing.Color.Red
                    'CType(rowData.FindControl("貸玉数"), TextBox).ForeColor = Drawing.Color.Red
                    'CType(rowData.FindControl("玉単価"), TextBox).ForeColor = Drawing.Color.Red
                    'CType(rowData.FindControl("貸玉数_照会"), TextBox).ForeColor = Drawing.Color.Red
                    'CType(rowData.FindControl("玉単価_照会"), TextBox).ForeColor = Drawing.Color.Red
                    'CType(rowData.FindControl("貸メダル数"), TextBox).ForeColor = Drawing.Color.Red
                    'CType(rowData.FindControl("メダル単価"), TextBox).ForeColor = Drawing.Color.Red
                    'CType(rowData.FindControl("貸メダル数_照会"), TextBox).ForeColor = Drawing.Color.Red
                    'CType(rowData.FindControl("メダル単価_照会"), TextBox).ForeColor = Drawing.Color.Red
                    'BPIFIXP001-001 END
                End If
            Next
        Catch ex As Exception
            'メッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "一覧の生成")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub


#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClear_Screen()

        Me.lblTboxId2.Text = String.Empty       'ＴＢＯＸＩＤ
        Me.lblHallNm2.Text = String.Empty       'ホール名
        Me.lblVer2.Text = String.Empty          'ＶＥＲ
        Master.ppCount = "0"
        Me.lblCountY.Text = "0"                 '差異有
        Me.lblCountN.Text = "0"                 '差異無
        Me.grvList.DataSource = New DataTable
        Me.grvList.DataBind()

    End Sub

    ''' <summary>
    ''' TBOXデータ取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msGet_TboxData() As Boolean

        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット
        Dim dtRow As DataRow = Nothing              'DataRow
        Dim strKey() As String = Nothing
        objStack = New StackFrame

        msGet_TboxData = False

        'ビューステート項目取得
        strKey = ViewState(P_KEY)

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then

            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_S2", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, strKey(0)))      'ＴＢＯＸＩＤ
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ表示
                If dstOrders.Tables(0).Rows.Count = 0 Then
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "ＴＢＯＸ情報")
                    Exit Try
                Else
                    dtRow = dstOrders.Tables(0).Rows(0)

                    Me.lblTboxId2.Text = strKey(0)
                    Me.lblHallNm2.Text = dtRow("ホール名").ToString
                    Me.lblVer2.Text = dtRow("ＶＥＲ").ToString

                End If

                msGet_TboxData = True

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ情報")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Function

    ''' <summary>
    ''' 一覧データ取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_ListData()

        Dim dstOrders As DataSet = Nothing          'データセット
        Dim strKey() As String = Nothing
        Dim dtCSV As New DataTable
        objStack = New StackFrame

        'BPIFIXP001-001
        Try
            'キー項目取得
            strKey = ViewState(P_KEY)

            '貸玉CSVファイルパス取得
            Using cmdSQL As SqlCommand = New SqlCommand("BPIFIXP001_S3")
                'パラメータ設定
                With cmdSQL.Parameters
                    .Add(pfSet_Param("@tboxid", SqlDbType.NVarChar, strKey(0)))
                End With

                'データ取得実行
                If pfExec_StoredProcedure(Me, "CSVファイルパス", cmdSQL, dstOrders) = False Then
                    Exit Sub
                End If
            End Using

            'カラム設定
            With dtCSV.Columns
                .Add("運用機番")
                .Add("JB番号")
                .Add("貸玉数")
                .Add("貸玉単価")
                .Add("BB種別コード")
            End With

            'CSVデータ取得
            If dstOrders.Tables(0).Rows.Count > 0 Then
                If Get_CSVData(strKey(0), dstOrders.Tables(0).Rows(0)("フォルダ").ToString, dstOrders.Tables(0).Rows(0)("ファイル名").ToString, dtCSV) = False Then
                    Exit Sub
                End If
            Else
                dtCSV.Rows.Add(dtCSV.NewRow)
            End If

            '一覧データ取得
            Using cmdSQL As SqlCommand = New SqlCommand("BPIFIXP001_S1")
                'パラメータ設定
                With cmdSQL.Parameters
                    .Add(pfSet_Param("CTRL_NO", SqlDbType.NVarChar, strKey(1)))     '照会管理番号
                    .Add(pfSet_Param("EDA_SEQ", SqlDbType.Char, strKey(2)))         '照会通番
                    .Add(pfSet_Param("KASHITAMA_CSV", SqlDbType.Structured, dtCSV)) '貸玉CSV
                End With

                'データ取得実行
                If pfExec_StoredProcedure(Me, "一覧データ", cmdSQL, dstOrders) = False Then
                    Exit Sub
                End If
            End Using

            'データ件数確認
            With dstOrders.Tables(0)
                If .Rows.Count = 0 Then
                    '0件
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    Master.ppCount = "0"
                    Me.lblCountY.Text = "0"
                    Me.lblCountN.Text = "0"
                Else
                    '閾値を超えた場合はメッセージを表示
                    If .Rows(0)("件数") > .Rows.Count Then
                        psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                            .Rows(0)("件数").ToString, .Rows.Count.ToString)
                    End If

                    '件数を設定
                    Master.ppCount = .Rows(0)("件数").ToString
                    Me.lblCountY.Text = .Rows(0)("差異有件数").ToString
                    Me.lblCountN.Text = .Rows(0)("差異無件数").ToString
                End If
            End With

            '取得したデータをグリッドに設定
            Me.grvList.DataSource = dstOrders.Tables(0)
            Me.grvList.DataBind()

        Catch ex As Exception
            'データ取得エラー
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "貸玉数　設定情報差異確認")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try


        ''DB接続
        'If clsDataConnect.pfOpen_Database(conDB) Then

        '    Try

        '        cmdDB = New SqlCommand(M_MY_DISP_ID + "_S1", conDB)
        '        'パラメータ設定
        '        With cmdDB.Parameters
        '            .Add(pfSet_Param("ctrl_no", SqlDbType.NVarChar, strKey(0).ToString))    '照会管理番号
        '            .Add(pfSet_Param("shokaiseq", SqlDbType.Char, strKey(1).ToString))      '照会通番
        '            .Add(pfSet_Param("edaseq", SqlDbType.Char, strKey(2).ToString))         '枝番
        '            .Add(pfSet_Param("fksdate", SqlDbType.Char, strKey(3).ToString))        '監視サーバ運用日付
        '            .Add(pfSet_Param("dataseq", SqlDbType.Char, strKey(4).ToString))        '連番
        '            .Add(pfSet_Param("tboxid", SqlDbType.Char, strKey(5).ToString))         'ＴＢＯＸＩＤ
        '            .Add(pfSet_Param("disp_id", SqlDbType.NVarChar, M_MY_DISP_ID))          '画面ＩＤ
        '            .Add(pfSet_Param("default", SqlDbType.NVarChar, "1"))                   '初期表示フラグ
        '        End With

        '        'データ取得
        '        dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

        '        'データ有無確認
        '        If dstOrders.Tables(0).Rows.Count = 0 Then
        '            '0件
        '            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
        '            'psClose_Window(Me)
        '            'Return
        '        Else
        '            '閾値を超えた場合はメッセージを表示
        '            If dstOrders.Tables(0).Rows(0)("データ件数") > dstOrders.Tables(0).Rows.Count Then
        '                psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
        '                    dstOrders.Tables(0).Rows(0)("データ件数").ToString, dstOrders.Tables(0).Rows.Count.ToString)
        '            End If

        '            '件数を設定
        '            Master.ppCount = dstOrders.Tables(0).Rows(0)("データ件数").ToString
        '            Me.lblCountY.Text = dstOrders.Tables(0).Rows(0)("差異有件数").ToString
        '            Me.lblCountN.Text = dstOrders.Tables(0).Rows(0)("差異無件数").ToString
        '        End If

        '        '取得したデータをグリッドに設定
        '        Me.grvList.DataSource = dstOrders.Tables(0)

        '        '変更を反映
        '        Me.grvList.DataBind()

        '    Catch ex As Exception
        '        'データ取得エラー
        '        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "貸玉数　設定情報差異確認")
        '        '--------------------------------
        '        '2014/04/14 星野　ここから
        '        '--------------------------------
        '        'ログ出力
        '        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
        '                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
        '        '--------------------------------
        '        '2014/04/14 星野　ここまで
        '        '--------------------------------
        '        Me.grvList.DataSource = New DataTable
        '        Me.grvList.DataBind()
        '        Master.ppCount = "0"
        '    Finally
        '        'DB切断
        '        If Not clsDataConnect.pfClose_Database(conDB) Then
        '            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        '        End If
        '    End Try
        'Else
        '    psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
        '    Me.grvList.DataSource = New DataTable
        '    Me.grvList.DataBind()
        '    Master.ppCount = "0"
        'End If
        'BPIFIXP001-001 END

    End Sub

    ''' <summary>
    ''' CSVファイル取得 'BPIFIXP001-001
    ''' </summary>
    ''' <param name="strTBOXID">TBOXID</param>
    ''' <param name="strFTP_DirPath">CSVファイルの保存先パス</param>
    ''' <param name="strFTP_FileName">CSVファイル名</param>
    ''' <param name="dtCSV">取得したCSVデータを格納するDataTable</param>
    ''' <remarks>FTPでGETしたCSVをLISTに格納します</remarks>
    Private Function Get_CSVData(ByVal strTBOXID As String, ByVal strFTP_DirPath As String, ByVal strFTP_FileName As String, ByRef dtCSV As DataTable) As Boolean

        Get_CSVData = False
        objStack = New StackFrame

        Dim DBFTP As New DBFTP.ClsDBFTP_Main
        Dim strLocal_FilePath As String = "DOWNLOAD\KASHITAMA_" & strTBOXID & "_" & Date.Now.ToString("yyyyMMddHHmmss")
        Dim lstCSV As New List(Of String())

        'CSVファイルをListで取得
        Try
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
            If IO.File.Exists("C:\" & strLocal_FilePath) Then
                IO.File.Delete("C:\" & strLocal_FilePath)
            End If

        Catch ex As Exception
            'エラー
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSVファイルの取得")       '{0}に失敗しました。
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Exit Function
        End Try


        'List→DataTbale 変換
        Try
            'DataTable作成
            Dim drTmp As DataRow

            'List→DataTable変換
            For Each strCSV() As String In lstCSV
                'レコード作成
                drTmp = dtCSV.NewRow
                With drTmp
                    .Item("運用機番") = strCSV(15)
                    .Item("JB番号") = strCSV(14)
                    .Item("貸玉数") = strCSV(34).TrimStart("0")
                    .Item("貸玉単価") = strCSV(35).TrimStart("0") & "." & strCSV(36).ToString
                    .Item("BB種別コード") = strCSV(16)
                End With

                '作成したレコードをDataTableに追加
                dtCSV.Rows.Add(drTmp)
            Next

        Catch ex As Exception
            'エラー
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSVファイルの変換")       '{0}に失敗しました。
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Exit Function
        End Try


        '正常終了
        Get_CSVData = True

    End Function


#End Region


End Class
