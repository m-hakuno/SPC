'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　設置環境写真一覧
'*　ＰＧＭＩＤ：　COMLSTP002
'*                                                                                                        CopyRihgt F･S CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2018.04.17　：　小野
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------

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

Public Class COMLSTP002

#Region "継承定義"

    Inherits System.Web.UI.Page

#End Region

#Region "定数定義"

    'プログラムID
    Const M_MY_DISP_ID = P_FUN_COM & P_SCR_LST & P_PAGE & "001"

#End Region

#Region "変数定義"

    Dim objStack As StackFrame
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom
    Dim DBFTP As New DBFTP.ClsDBFTP_Main
    Dim serchcon(4) As String '検索条件保存用

#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page_Initイベント
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, "COMLSTP002", "L")


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

        BtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "選択されたファイル")


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

                '過去一週間分を初期表示
                Dim dt = DateTime.Today
                dftGetDt.ppFromText = dt.AddDays(-7)
                dftGetDt.ppToText = dt
                serchcon.SetValue(Me.tftTboxId.ppFromText, 0)
                serchcon.SetValue(Me.tftTboxId.ppToText, 1)
                serchcon.SetValue(Me.dftGetDt.ppFromText, 2)
                serchcon.SetValue(Me.dftGetDt.ppToText, 3)
                serchcon.SetValue(Me.txtHallNm.ppText, 4)
                ViewState("serch") = serchcon
                ViewState("delete") = "0"
                msGet_DataPic("0")

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
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender


        If Not Session(P_SESSION_AUTH) = "管理者" Then

            BtnDelete.Enabled = False

            For Each row As GridViewRow In grvList.Rows
                Dim cbox As CheckBox = CType(row.Cells(1).Controls(0), CheckBox)
                cbox.Enabled = False
            Next

        End If

    End Sub


    ''' <summary>
    ''' 検索ボタンクリック時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        ViewState("delete") = "0"

        '開始ログ出力
        psLogStart(Me)

        'データ取得
        If (Page.IsValid) Then
            serchcon.SetValue(Me.tftTboxId.ppFromText, 0)
            serchcon.SetValue(Me.tftTboxId.ppToText, 1)
            serchcon.SetValue(Me.dftGetDt.ppFromText, 2)
            serchcon.SetValue(Me.dftGetDt.ppToText, 3)
            serchcon.SetValue(Me.txtHallNm.ppText, 4)
            ViewState("serch") = serchcon
            msGet_DataPic("0")
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

        If Not e.CommandName = "Sort" Then

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
            Dim strKeyList As List(Of String) = Nothing                      '次画面引継ぎ用キー情報

            objStack = New StackFrame


            '開始ログ出力
            psLogStart(Me)

            Try
                'ファイル名,フォルダパス
                strDownLoadfile = CType(rowData.FindControl("ファイル名"), Label).Text
                strDownLoadPath = CType(rowData.FindControl("フォルダパス"), Label).Text & "\" & _
                                  CType(rowData.FindControl("TBOXID"), Label).Text
                strFullPath = "\\" + strDownLoadPath + "\" + strDownLoadfile
                strFullPath = strFullPath.Replace("/", "\")
                splPath = strFullPath.Split("\")
                For spl = 3 To splPath.Count - 2
                    filePath2 &= splPath(spl) & "\"
                Next

                strfileName = splPath(splPath.Count - 1)

                'ファイルの存在確認
                If DBFTP.pfFtpFile_Exists(filePath2, strfileName, opblnResult) = False Then

                    strErrmsg = strDownLoadfile
                    Throw New Exception

                End If


                '選択ボタン押下
                If e.CommandName = "btnSelect" Then

                    '拡張子の取得
                    strExtension = Path.GetExtension(strfileName)
                    localFileName = localFileName & strExtension

                    'ローカルにフォルダがなかった場合、作成する
                    If Directory.Exists(strLocalPath) = False Then
                        System.IO.Directory.CreateDirectory(strLocalPath)
                    End If

                    'ローカルにダウンロードを開始する
                    DBFTP.pfFtpFile_Copy("GET", filePath2, splPath(splPath.Count - 1), opblnResult, localdirName & "/" & localFileName)

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

                End If


                'ダウンロード情報を更新
                Dim conDB As SqlConnection = Nothing
                Dim com As SqlCommand = Nothing
                If clsDataConnect.pfOpen_Database(conDB) Then
                    Using conTrn = conDB.BeginTransaction
                        com = New SqlCommand(M_MY_DISP_ID + "_S2", conDB)
                        com.CommandType = CommandType.StoredProcedure
                        com.Transaction = conTrn
                        With com.Parameters
                            .Add(pfSet_Param("kanrinum", SqlDbType.NVarChar, CType(rowData.FindControl("管理番号"), Label).Text))
                            .Add(pfSet_Param("retvalue", SqlDbType.SmallInt))
                        End With
                        '実行
                        com.ExecuteNonQuery()
                        '戻り値チェック
                        If com.Parameters("retvalue").Value.ToString <> 0 Then
                            'ロールバック
                            conTrn.Rollback()
                        End If
                        'コミット
                        conTrn.Commit()
                    End Using
                    '一覧の更新
                    msGet_DataPic("0")
                End If

            Catch ex As Exception
                'エラー
                psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "設置環境写真")       '{0}の出力に失敗しました。
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
            End Try


            '終了ログ出力
            psLogEnd(Me)

        End If
    End Sub


    ''' <summary>
    ''' 削除ボタン押下処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click


        Dim deldt As New DataTable
        If deldt.Rows.Count <> 0 Then
            deldt.Rows.Clear()
        End If
        Dim rowCount As Integer = Nothing
        Dim opblnResult As Boolean = False
        Dim strFullPath As String = Nothing
        Dim strDeletefile As String = Nothing
        Dim strDeletePath As String = Nothing
        Dim splPath As String() = Nothing
        Dim filePath2 As String = Nothing
        Dim strfileName As String = Nothing

        objStack = New StackFrame

        ViewState("delete") = "1"


        '開始ログ出力
        psLogStart(Me)

        deldt.Columns.Add("管理番号")
        deldt.Columns.Add("ファイル名")
        deldt.Columns.Add("フォルダパス")
        deldt.Columns.Add("TBOXID")


        'チェックされたファイル一覧の取得
        Try

            For Each row As GridViewRow In grvList.Rows

                Dim cbox As CheckBox = CType(row.Cells(1).Controls(0), CheckBox)
                If cbox.Checked = True Then
                    Dim ar(3) As String
                    Dim mngNm As Label = CType(row.Cells(8).Controls(0), Label)
                    Dim fileNm As Label = CType(row.Cells(4).Controls(0), Label)
                    Dim foldPass As Label = CType(row.Cells(7).Controls(0), Label)
                    Dim tboxid As Label = CType(row.Cells(2).Controls(0), Label)
                    ar.SetValue(mngNm.Text, 0)
                    ar.SetValue(fileNm.Text, 1)
                    ar.SetValue(foldPass.Text, 2)
                    ar.SetValue(tboxid.Text, 3)
                    deldt.Rows.Add(ar)
                End If
            Next


            rowCount = deldt.Rows.Count
            If Not rowCount = 0 Then

                Dim x As Integer
                For x = 0 To rowCount - 1

                    strDeletePath = deldt.Rows(x).Item("フォルダパス").ToString & "\" & _
                                               deldt.Rows(x).Item("TBOXID").ToString
                    strDeletefile = deldt.Rows(x).Item("ファイル名").ToString

                    strFullPath = "\\" + strDeletePath + "\" + strDeletefile
                    strFullPath = strFullPath.Replace("/", "\")
                    splPath = strFullPath.Split("\")
                    filePath2 = Nothing
                    For spl = 3 To splPath.Count - 2
                        filePath2 &= "\" & splPath(spl) & "\"
                    Next

                    strfileName = splPath(splPath.Count - 1)

                    'ファイルの存在確認
                    If DBFTP.pfFtpFile_Exists(filePath2, strfileName, opblnResult) = False Then
                        'If DBFTP.pfFtpFile_Exists("\upload\GENBAPIC\" & deldt.Rows(x).Item("TBOXID").ToString & "\", strfileName, opblnResult) = False Then

                        psMesBox(Me, "30003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strfileName)
                        Exit Sub
                    End If
                Next


                Dim i As Integer
                For i = 0 To rowCount - 1
                    strDeletePath = deldt.Rows(i).Item("フォルダパス").ToString & "\" & _
                                              deldt.Rows(i).Item("TBOXID").ToString
                    strDeletefile = deldt.Rows(i).Item("ファイル名").ToString

                    strFullPath = "\\" + strDeletePath + "\" + strDeletefile
                    strFullPath = strFullPath.Replace("/", "\")
                    splPath = strFullPath.Split("\")
                    filePath2 = Nothing
                    For spl = 3 To splPath.Count - 2
                        filePath2 &= splPath(spl) & "\"
                    Next

                    strfileName = splPath(splPath.Count - 1)

                    'ダウンロード情報削除
                    Dim conDB As SqlConnection = Nothing
                    Dim com As SqlCommand = Nothing
                    If clsDataConnect.pfOpen_Database(conDB) Then
                        Using conTrn = conDB.BeginTransaction
                            com = New SqlCommand(M_MY_DISP_ID + "_S3", conDB)
                            com.CommandType = CommandType.StoredProcedure
                            com.Transaction = conTrn
                            With com.Parameters
                                .Add(pfSet_Param("kanrinum", SqlDbType.NVarChar, deldt.Rows(i).Item("管理番号")))
                                .Add(pfSet_Param("filemei", SqlDbType.NVarChar, deldt.Rows(i).Item("ファイル名")))
                                .Add(pfSet_Param("retvalue", SqlDbType.SmallInt))
                            End With
                            '実行
                            com.ExecuteNonQuery()
                            '戻り値チェック
                            If com.Parameters("retvalue").Value.ToString <> 0 Then
                                conTrn.Rollback()
                                Throw New Exception
                            End If

                            'ファイルの削除
                            If DBFTP.pfFtpFile_Delete(filePath2, strfileName, opblnResult) = False Then
                                conTrn.Rollback()
                                Throw New Exception
                            End If

                            'コミット
                            conTrn.Commit()

                        End Using

                    End If
                Next

                '一覧の更新
                msGet_DataPic("0")


                '処理件数の表示
                psMesBox(Me, "00002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, rowCount & "件のファイル")
            Else
                'チェックがないとき
                psMesBox(Me, "30016", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "ファイル")
            End If

        Catch ex As Exception
            'エラー
            psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "設置環境写真")       '{0}の削除に失敗しました。
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

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
        Me.txtHallNm.ppText = String.Empty
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
    Private Sub msGet_DataPic(ByVal ipstrDefault As String)

        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット
        objStack = New StackFrame

        'ViewStateから入力値を取得
        serchcon = ViewState("serch")

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_S1", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    'ＴＢＯＸＩＤFrom
                    .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, serchcon.GetValue(0).ToString))
                    'ＴＢＯＸＩＤTo
                    .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, serchcon.GetValue(1).ToString))
                    '登録日From
                    .Add(pfSet_Param("get_dt_f", SqlDbType.NVarChar, serchcon.GetValue(2).ToString))
                    '登録日To
                    .Add(pfSet_Param("get_dt_t", SqlDbType.NVarChar, serchcon.GetValue(3).ToString))
                    '画面ＩＤ
                    .Add(pfSet_Param("disp_id", SqlDbType.NVarChar, M_MY_DISP_ID))
                    '初期表示フラグ
                    .Add(pfSet_Param("default", SqlDbType.NVarChar, ipstrDefault))
                    'ホール名検索入力値---2018/4/18追加 小野
                    .Add(pfSet_Param("hallname", SqlDbType.NVarChar, serchcon.GetValue(4).ToString))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ有無確認
                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    If ViewState("delete") <> "1" Then
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        '件数を設定
                        Master.ppCount = "0"
                    End If
                Else
                    '閾値を超えた場合はメッセージを表示
                    If dstOrders.Tables(0).Rows(0)("データ件数") > dstOrders.Tables(0).Rows.Count And _
                         ViewState("delete") <> "1" Then
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
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "設置環境写真一覧")
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



#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
