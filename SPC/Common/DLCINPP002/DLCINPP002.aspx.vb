'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　アップロード
'*　ＰＧＭＩＤ：　DLCINPP002
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2022.06.23　：　MITS
'********************************************************************************************************************************
#Region "更新履歴"
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'DLCINPP002-001     2015/06/11      加賀      BB1調査報告書のファイル名の変更(ホール名、TBOXIDをファイル名に追加)              
#End Region

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports System.IO
Imports System.String
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive
Imports DBFTP.ClsDBFTP_Main
Imports DBFTP.ClsFTPCnfg
'Imports DBFTP.clsLogwrite
Imports DBFTP.ClsSQLSvrDB
Imports System.Security.AccessControl

#End Region

Public Class DLCINPP002
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
    Const M_DISP_ID = P_FUN_DLC & P_SCR_INP & P_PAGE & "002"    'プログラムID

    Const M_KENSYU_NAME As String = "KENSYUSYO"                 '工事完了報告書兼検収書
    Const M_DDL_NAME As String = "DLLHAISHIN"                   'DLL配信データ
    Const M_TOMAS_NAME As String = "TOMAS_HALL"                 'TOMASホール情報
    Const M_SAGYO_NAME As String = "HOSYUKANRYOU"               '保守完了報告書
    Const M_KASHITAMA_NAME As String = "KASHITAMA"              '貸玉数設定情報
    Const M_BBTYOUSA_NAME As String = "BBCHOUSA"                'BB調査報告書
    Const M_BBKOSEI_NAME As String = "BBKOUSEI"                 'BB構成ファイル
    Const M_MDNH_PIC_NAME As String = "MDNHPIC"                 'ＭＤＮＨ設置写真

    '---2014/02/25 高松　ここから
    Const M_FILE_TYPE1 = "0151FC"                               '工事完了報告書兼検収書
    Const M_FILE_TYPE2 = "0152FC"                               'DLL配信データ
    Const M_FILE_TYPE3 = "0153FC"                               'TOMASホール情報
    Const M_FILE_TYPE4 = "0154FC"                               '保守完了報告書
    Const M_FILE_TYPE5 = "0155FC"                               '貸玉数設定情報
    Const M_FILE_TYPE6 = "0156FC"                               'BB調査報告書
    Const M_FILE_TYPE7 = "0157FC"                               'BB構成ファイル
    Const M_FILE_TYPE8 = "0158FC"                               'BB構成ファイルバックアップ
    '---2018/4/13 小野 ここから
    Const M_FILE_TYPE9 = "0159FC"                               '設置環境写真
    '---2018/4/13 小野 ここまで
    Const M_FILE_TYPE10 = "0160FC"                              'ＭＤＮＨ設置写真

    'Const M_FILE_TYPE1 = "1"                                   '検収書
    'Const M_FILE_TYPE2 = "1"                                   'DLL配信データ
    'Const M_FILE_TYPE3 = "1"                                   'TOMASホール情報
    'Const M_FILE_TYPE4 = "1"                                   '作業報告書
    'Const M_FILE_TYPE5 = "1"                                   '貸玉数設定情報
    'Const M_FILE_TYPE6 = "1"                                   'BB調査報告書
    'Const M_FILE_TYPE7 = "1"                                   'BB構成ファイル
    '---2014/02/25 高松　ここまで

    Const M_ERRMSG1 = "アップロードファイル"                    'エラーメッセージ用
    Const M_ERRMSG2 = "ファイル保管場所"                        'エラーメッセージ用
    Const M_ERRMSG3 = "入力された工事依頼番号"                  'エラーメッセージ用
    Const M_ERRMSG4 = "工事完了報告書兼検収書"                  'エラーメッセージ用
    Const M_ERRMSG5 = "DLL配信データ"                           'エラーメッセージ用
    Const M_ERRMSG6 = "BB構成ファイル"                          'エラーメッセージ用
    '---2018/4/13 小野 ここから
    Const M_ERRMSG7 = "設置環境写真"                            'エラーメッセージ用
    '---2018/4/13 小野 ここまで
    Const M_ERRMSG8 = "ＭＤＮＨ設置写真"                        'エラーメッセージ用

    Const M_STERTLEN As Integer = 10                            '切り取り開始文字数
    Const M_ENDLEN As Integer = 8                               '切り取り文字数
    Const M_CLOMUN_NUM As Integer = 10                          'CSVファイルのTBOXID保管位置

    Const M_REPLACE As String = "‥"                            'CSVファイル内のカンマを置き換える

    Const M_SHINYA_FLAG As String = "特別運用モードＳＷ（深夜営業）" '深夜営業フラグ文字
    Const M_SYUYA_FLAG As String = "特別運用モードＳＷ（終夜営業）" '終夜営業フラグ文字

    Const M_BTN_NM_UPL = "アップロード"                         'ボタン名称

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
    Dim clsExc As New ClsCMExclusive

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'アップロードボタン押下のイベント設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnUpl_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClr_Click

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        If Not IsPostBack Then  '初回表示
            Try

                '開始ログ出力
                psLogStart(Me)

                '排他情報用のグループ番号保管
                If Not Session(P_SESSION_GROUP_NUM) Is Nothing Then

                    ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

                End If

                '変数定義
                Dim enableScript As String = Nothing                'ラジオボタン制御用
                Dim enablestr As String = Nothing                   '共通文字列保管用
                Dim strUser(1) As String                            'ユーザ権限(0),起動場所(1)

                'todo セッション変数の表示画面数の追加更新を行う ※あとで追加する 

                'ラジオボタンのonChangeイベントの設定
                '---2018/4/13 小野 設置環境写真TBOXID追加
                enablestr = "','" + txtPicTboxId.ppTextBox.ClientID + "')"


                '設置環境写真
                enableScript = "rdioBtnChange2('" + "1" + enablestr
                rdbMDNPic.Attributes.Add("onChange", enableScript)

                'セッション変数「遷移条件」「遷移元ＩＤ」が存在しない場合、画面を閉じる
                'If Session(P_SESSION_TERMS) Is Nothing Or Session(P_SESSION_OLDDISP) Is Nothing Then
                '    psMesBox(Me, "0007", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画前)
                '    psClose_Window(Me)
                '    Return
                'End If

                'ユーザの権限、場所を取得
                'strUser(0) = Session(P_SESSION_BCLIST)
                'strUser(1) = Session(P_SESSION_BCLIST)

                '権限、場所をビューステートに格納
                'Me.ViewState.Add(P_KEY, strUser)

                '画面名を表示
                Master.ppProgramID = M_DISP_ID
                Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

                'パンくずリスト
                Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

                'コントロールの初期化
                msClearScreen()

                'ボタン押下時のメッセージ設定
                Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("30002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel)

            Catch ex As Exception

                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
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

                '終了ログ出力
                psLogEnd(Me)

            End Try

        End If

    End Sub

#End Region

#Region "ユーザー権限"
    '---------------------------
    '2014/04/15 武 ここから
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
    '2014/04/15 武 ここまで
    '---------------------------
#End Region

#Region "ボタンクリックイベント"

    ''' <summary>
    ''' アップロードボタンクリック
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnUpl_Click()

        Dim Fullpath As HttpPostedFile = Nothing
        Dim strFileName As String = Nothing
        Dim strExtension As String = Nothing
        Dim strSavePath As String = Nothing
        Dim strLocalPath As String = Nothing
        Dim strLocaldir As String = Nothing
        Dim LocalFileNm As String = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString
        Dim strUpladfile As String = Nothing
        Dim listTBOXID As New List(Of String())
        Dim strTBOXID As String = Nothing
        Dim strErrmsg As String = Nothing
        Dim localDirName As String = Nothing
        Dim strExclusiveDate As String = Nothing
        Dim arTable_Name As New ArrayList
        Dim arKey As New ArrayList
        Dim opblnResult As Boolean = False
        Dim upCls As String = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            '開始ログ出力
            psLogStart(Me)

            'アップロードコントロールの検証チェック
            msCheck_Integrity()

            'TEXTBOXの検証チェック
            If pfCheck_Txtbox(strErrmsg) = False Then
                Throw New Exception
            End If

            If (Page.IsValid) Then

                'ファイル名を取得
                Fullpath = trfUplordFileSelection.PostedFile

                'ファイルの拡張子を取得
                strExtension = Path.GetExtension(Fullpath.FileName)

                'アップロードファイルをローカルに保持
                localDirName = "UPLOAD"
                strLocaldir = "C:"
                strLocalPath = strLocaldir & "/" & localDirName & "/"

                If Not Fullpath.FileName = "" Then

                    'フォルダがなかった場合、作成する
                    If Directory.Exists(strLocalPath) = False Then
                        System.IO.Directory.CreateDirectory(strLocalPath)
                    End If

                    LocalFileNm = LocalFileNm + strExtension
                    strLocalPath = strLocalPath & LocalFileNm
                    'strLocalPath.Replace(":\", "://")

                    'ローカルに保存
                    Fullpath.SaveAs(strLocalPath)

                End If

                '保管場所情報を取得
                strSavePath = msGet_SavePath(True)
                If strSavePath = Nothing Then

                    '取得エラーのため処理終了
                    strErrmsg = M_ERRMSG2
                    Throw New Exception

                End If

                'ファイル名取得/ディレクトリの作成

                '---2018/4/17 小野 ここから
                Dim picFilename As String
                If strExtension = ".jpg" Or strExtension = ".jpeg" Or strExtension = ".JPG" Or strExtension = ".JPEG" _
                   Or strExtension = ".png" Or strExtension = ".PNG" Then
                    picFilename = System.IO.Path.GetFileNameWithoutExtension(Fullpath.FileName)
                Else
                    picFilename = Nothing

                End If
                strFileName = msGet_fileName(strExtension, strSavePath, strTBOXID, strLocalPath, picFilename)
                '---2018/4/17 小野 ここまで

                If strFileName = Nothing Then

                    'ファイルパスの作成を失敗した
                    strErrmsg = Nothing
                    Throw New Exception

                End If

                '保存後処理
                If rdbMDNPic.Checked = True Then              '設置環境写真
                    upCls = "8"
                End If

                psMesBox(Me, "30003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)

            End If

            'ローカルに一時的に作成したファイルを削除
            'ファイルの存在を確認
            If System.IO.File.Exists(strLocalPath) Then
                System.IO.File.Delete(strLocalPath)
            End If

            'コントロールのenableを反映させる
            msChange_ctl()

            '終了ログ出力
            psLogEnd(Me)

            'DLCINPP002-004
        Catch ex As SqlException

            'SQLタイムアウト
            'メッセージ出力
            If ex.Number = -2 Then
                'SQLタイムアウト
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【SQLタイムアウト】アップロード") 'データの表示に失敗しました。\nエラー内容：{0}
            Else
                'システムエラー
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【SQLエラー】アップロード")
            End If

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
            'DLCINPP002-004 END
        Catch ex As Exception

            Select Case strErrmsg
                Case Nothing
                    'システムエラー
                    psMesBox(Me, "30002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "アップロード")
                Case "CSV", "BB構成"
                    psMesBox(Me, "10006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strErrmsg & "ファイルの形式")
                Case "BB構成TBOX"
                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Case Else
                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, strErrmsg)
            End Select

            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------

            '排他ロックの解除
            'アップロードファイル存在確認(保存先)
            If File.Exists(strUpladfile) Then
                'ファイルを削除
                System.IO.File.Delete(strUpladfile)
            End If
            'コントロールのenableを反映させる
            msChange_ctl()

        Finally

            'ローカルに一時的に作成したファイルを削除
            'ファイルの存在を確認
            If System.IO.File.Exists(strLocalPath) Then
                System.IO.File.Delete(strLocalPath)
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try
    End Sub

    ''' <summary>
    ''' クリアボタンクリック
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnClr_Click()
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            '開始ログ出力
            psLogStart(Me)
            'クリア処理
            msClearScreen()
            '終了ログ出力
            psLogEnd(Me)

        Catch ex As Exception

            'システムエラー
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
        Finally
        End Try
    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' コントロールの初期化
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub msClearScreen()

        'Dim strView() As String = Nothing                'ビューステート登録用の情報
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            'ViewStateから情報を取得
            'strView = Me.ViewState(P_KEY)

            '場所、権限で設定を変更する
            'If User = "" Then

            'End If
            'If Prace = "" Then

            'End If

            '入力値の初期化
            txtPicTboxId.ppText = String.Empty

            '表示の設定
            txtPicTboxId.ppEnabled = False
            rdbMDNPic.Checked = False
            trfUplordFileSelection.Enabled = True

            '検証エラー設定
            Master.ppRigthButton1.CausesValidation = True
            Master.ppRigthButton2.CausesValidation = False
            Master.ppRigthButton1.ValidationGroup = "1"

            'ボタンの表示設定
            Master.ppRigthButton1.Visible = True
            Master.ppRigthButton2.Visible = True
            Master.ppRigthButton1.Text = M_BTN_NM_UPL
            Master.ppRigthButton2.Text = P_BTN_NM_CLE

            'エラーサマリの設定
            valfileUpload.ValidationGroup = "1"

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
            Throw ex

        Finally
        End Try

    End Sub

    ''' <summary>
    ''' 保管場所取得
    ''' </summary>
    ''' <param name="flag"> バックアップ判断用 </param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function msGet_SavePath(ByVal flag As Boolean) As String

        Dim strPath As String = Nothing
        Dim result As String = "0"
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

        Try
            '開始ログ出力
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Throw New Exception
            End If

            cmdDB = New SqlCommand(M_DISP_ID & "_S2", conDB)

            '設置環境写真
            If rdbMDNPic.Checked = True Then

                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_file", SqlDbType.NVarChar, M_FILE_TYPE10))      'ファイル種別
                End With


            End If

            'リストデータ取得
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            strPath = dstOrders.Tables(0).Rows(0).Item("パス").ToString

            Return strPath


        Catch ex As SqlException

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

        Finally

            '終了ログ出力
            psLogEnd(Me)

        End Try
    End Function

    ''' <summary>
    ''' ファイル名取得/ディレクトリの作成
    ''' </summary>
    ''' <remarks></remarks>
    ''' 2018/4/17 小野 設置環境写真用に引数を追加
    Private Function msGet_fileName(ByVal Extension As String _
                                  , ByVal filePath As String _
                                  , ByVal TBOXID As String _
                                  , ByVal strLocalPath As String _
                                  , ByVal strPicName As String) As String

        Dim fileName As String = Nothing      'ファイル名
        Dim dirpath As DirectoryInfo
        Dim makeDir As DirectoryInfo
        Dim opblnResult As Boolean
        Dim folderName As String = DateTime.Now.ToString("yyyyMMdd")
        Dim filePath2 As String = Nothing
        Dim splPath As String() = Nothing
        Dim makeDirPath As String = Nothing

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            '開始ログ出力
            psLogStart(Me)

            '設置環境写真
            If rdbMDNPic.Checked = True Then
                '[TBOXID]\[アップロード日(YYYYMMDD)]\ファイル名
                filePath = "\\" + filePath + "\" + Me.txtPicTboxId.ppText
                splPath = filePath.Split("\")

                For spl = 0 To 3
                    If spl <> 3 Then
                        makeDirPath &= splPath(spl) & "\"
                    Else
                        makeDirPath &= "dbftp\"
                    End If
                Next
                For spl = 3 To splPath.Count - 1
                    filePath2 &= splPath(spl) & "\"
                Next

                If Not strPicName = Nothing Then
                    fileName = strPicName + Extension
                Else
                    fileName = M_MDNH_PIC_NAME _
                           + "_" _
                           + txtPicTboxId.ppText _
                           + Extension
                End If
                'アップロード先のパスを作成
                makeDirPath &= filePath2
            End If

            '保存先のフォルダが存在しない場合作成する
            makeDir = New DirectoryInfo(makeDirPath)

            'FTP用のパスを作成
            dirpath = New DirectoryInfo(filePath2)

            '保存先のフォルダの存在有無を確認
            If DBFTP.pfFtpDir_Exists(dirpath.ToString, opblnResult, "1") = False Then
                '存在しなかった場合、フォルダを作成しアップロード
                If DBFTP.pfFtpFile_Copy("PUT", dirpath.ToString, folderName, fileName, opblnResult, strLocalPath) = False Then
                    'アップロードに失敗
                    Throw New Exception
                End If
                'フォルダが存在した場合
            Else
                'アップロード
                If DBFTP.pfFtpFile_Exists(dirpath.ToString, fileName, opblnResult) = True Then
                    If DBFTP.pfFtpFile_Delete(dirpath.ToString, fileName, opblnResult) = False Then
                        'アップロードに失敗
                        Throw New Exception
                    End If
                End If
                If DBFTP.pfFtpFile_Copy("PUT", dirpath.ToString, fileName, opblnResult, strLocalPath) = False Then
                    'アップロードに失敗
                    Throw New Exception
                End If
            End If

            'アップロードの種類によってパスを切り替える
            Dim selectPath As String = 0
            Select Case selectPath
                Case 0
                    Return filePath + "/" + fileName
                Case 1
                    Return makeDirPath + fileName
                Case Else
                    Return String.Empty
            End Select

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
            Return Nothing

        Finally

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function

    ''' <summary>
    ''' TBOXIDの取得処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function msGet_Tboxid(ByVal strName As String _
                                , ByVal Fullpath As String) As String

        Dim objFs As FileStream = System.IO.File.OpenRead(Fullpath)   'FileStreamクラス
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            '開始ログ出力
            psLogStart(Me)

            Dim strTboxID As String = Nothing                  'TBOXID
            Dim fileSize As Integer = CInt(objFs.Length)       ' ファイルのサイズ
            Dim buf(fileSize - 1) As Byte                      ' データ格納用配列
            Dim buf2(fileSize - 1) As String                   ' データ格納用配列
            Dim readSize As Integer                            ' Readメソッドで読み込んだバイト数
            Dim remain As Integer = fileSize                   ' 読み込むべき残りのバイト数
            Dim bufPos As Integer = 0                          ' データ格納用配列内の追加位置

            ' 1024Bytesずつ読み込む(10進数)
            readSize = objFs.Read(buf, bufPos, Math.Min(fileSize, remain))

            '必要なバイト数を読み込む(16進数)
            For i As Integer = 0 To fileSize - 1
                Select Case i
                    Case 0 To 5
                        'TBOXIDの取得
                        buf2(i) = Convert.ToString(buf(i), 2).ToString.PadLeft(8, "0")
                        'BCDに変換
                        strTboxID = strTboxID + Convert.ToInt32(buf2(i).Substring(0, 4), 2).ToString
                        strTboxID = strTboxID + Convert.ToInt32(buf2(i).Substring(4, 4), 2).ToString
                End Select
            Next

            '拡張ＴＢＯＸ-ＩＤのため前2桁は必要なし
            Return strTboxID.Substring(2, 8)

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
            'ファイルオープンエラー
            Return Nothing

        Finally

            If Not objFs Is Nothing Then

                objFs.Close()

            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function

    ''' <summary>
    ''' コントロールのenableを反映させる
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msChange_ctl()

        '---2018/4/13 小野 ここから
        '設置環境写真
        If rdbMDNPic.Checked = True Then

            txtPicTboxId.ppEnabled = True

        End If
        '---2018/4/13 小野 ここまで

    End Sub

    '--2014/04/14 中川　ここから
    ''' <summary>
    ''' ダウンロード情報追加
    ''' </summary>
    ''' <param name="strFlag"></param>
    ''' <param name="strFileInf"></param>
    ''' <remarks></remarks>
    Private Sub ms_InsDownload(ByVal strFlag As String,
                               ByVal strFileInf() As String)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim strKanriNum As String = Nothing                                         '管理番号
        Dim strFileType As String = Nothing                                         'ファイル種別
        Dim strFileName As String = Nothing                                         'ファイル名
        Dim strReportName As String = Nothing                                       '帳票名
        Dim strServerName As String = Nothing                                       'サーバアドレス
        Dim strFilePath As String = Nothing                                         '保存先フォルダ
        Dim datCreateDate As DateTime = DateTime.Now                                '作成日
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        strFileName = strFileInf.Last
        strServerName = strFileInf.GetValue(2)

        '保存先フォルダ
        For i As Integer = 3 To strFileInf.Count - 2

            strFilePath += strFileInf(i)

            If i <> strFileInf.Count - 2 Then

                strFilePath += "\"

            End If

        Next



        '設置環境写真
        If strFlag = "7" Then
            strKanriNum = txtPicTboxId.ppText
            strFileType = M_FILE_TYPE10
            strReportName = rdbMDNPic.Text
            strFileName = System.IO.Path.GetFileName(String.Join("", strFileInf))

        End If

        If clsDataConnect.pfOpen_Database(conDB) Then

            Try
                '開始ログ出力
                psLogStart(Me)

                '---2018/4/18 小野 ここから
                'TBOXIDからホール名と既存画像データ検索
                Dim title As String = ""
                Dim reponame As String = ""
                Dim mngnum As String = ""
                If strFlag = "7" Then
                    title = txtPicTboxId.ppText
                    Dim com As New SqlCommand("DLCINPP002_S4", conDB)
                    Dim com2 As New SqlCommand("DLCINPP002_S5", conDB)
                    With com.Parameters.Add(pfSet_Param("TBOXID", SqlDbType.NVarChar, txtPicTboxId.ppText))
                    End With
                    With com2.Parameters.Add(pfSet_Param("TBOXID", SqlDbType.NVarChar, txtPicTboxId.ppText))
                    End With
                    Using objDst = clsDataConnect.pfGet_DataSet(com)
                        'Dim findrow = objDst.Tables(0).Select("ファイル名 = '" & strFileName & "' And TBOXID = '" & txtPicTboxId.ppText & "'")
                        'If Not findrow Is Nothing Then
                        '    Throw New Exception
                        'End If
                        mngnum = "SP" & txtPicTboxId.ppText & "-" & String.Format("{0:D3}", objDst.Tables(0).Rows.Count + 1)
                    End Using
                    Using objDst = clsDataConnect.pfGet_DataSet(com2)
                        If objDst.Tables(0).Rows.Count > 0 Then
                            reponame = objDst.Tables(0).Rows(0)("ホール名").ToString()
                        End If
                    End Using
                Else
                    title = Master.ppTitle
                    reponame = strReportName
                    mngnum = strKanriNum
                End If
                '---2018/4/18 小野　ここまで

                cmdDB = New SqlCommand("ZCMPINS001", conDB)
                With cmdDB.Parameters
                    .Add(pfSet_Param("MNG_NO", SqlDbType.NVarChar, mngnum))                                 '管理番号
                    .Add(pfSet_Param("FILE_CLS", SqlDbType.NVarChar, strFileType))                          'ファイル種別
                    .Add(pfSet_Param("TITLE", SqlDbType.NVarChar, title))                                   '画面タイトル
                    .Add(pfSet_Param("FILE_NM", SqlDbType.NVarChar, strFileName))                           'ファイル名
                    .Add(pfSet_Param("REPORT_NM", SqlDbType.NVarChar, reponame))                            '帳票名
                    .Add(pfSet_Param("SERVER_ADDRESS", SqlDbType.NVarChar, strServerName))                  'サーバアドレス
                    .Add(pfSet_Param("KEEP_FOLD", SqlDbType.NVarChar, strFilePath))                         '保存先フォルダ
                    .Add(pfSet_Param("CREATE_DT", SqlDbType.DateTime, datCreateDate))                       '作成日
                    .Add(pfSet_Param("INSERT_USR", SqlDbType.NVarChar, Session(P_SESSION_USERID).ToString)) 'ユーザＩＤ
                End With

                'データ追加
                Using conTrn = conDB.BeginTransaction
                    cmdDB.Transaction = conTrn
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()
                    'コミット
                    conTrn.Commit()
                End Using

            Catch ex As SqlException
                '追加に失敗
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ダウンロードファイル")
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
            Catch ex As Exception
                'システムエラー
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ダウンロードファイル")
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

                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

                '終了ログ出力
                psLogEnd(Me)

            End Try

        End If

    End Sub

    '--2014/04/14 中川　ここまで

#End Region

#Region "終了処理プロシージャ"

    ''' <summary>
    ''' TEXTBOXの検証チェック
    ''' </summary>
    ''' <remarks></remarks>
    Protected Function pfCheck_Txtbox(ByRef ErrMsg As String) As Boolean

        Dim strResult As String = Nothing
        Dim strMaxLength As String = Nothing
        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet
        'pfCheck_TxtErrの設定について
        ' <param name="ipstrData">チェックするデータ</param>
        ' <param name="ipblnReqFieldVal">必須チェックの要不要</param>
        ' <param name="ipblnNumF">数値チェックの要不要</param>
        ' <param name="ipblnCheckHan">半角チェックの要不要</param>
        ' <param name="ipblnLengthF">固定桁チェックの要不要</param>
        ' <param name="ipintLength">桁チェックの桁数</param>
        ' <param name="ipstrExpression">正規表現チェック(空の場合はチェックを行わない)</param>
        ' <param name="ipstrExpression">英字の用不要チェック(TRUE:英字のみ)</param>
        ' <returns>検証メッセージNo.(正常の場合は空白)</returns>

        pfCheck_Txtbox = True

        '---2018/4/13 小野 ここから
        '設置環境写真
        If rdbMDNPic.Checked = True Then

            If Regex.IsMatch(txtPicTboxId.ppText, "^[0-9]{8}$") = False Then
                Me.txtPicTboxId.psSet_ErrorNo("4001", txtPicTboxId.ppName, "半角数字８桁")
            Else
                'TBOXID存在確認
                Try
                    'ＤＢ接続
                    If Not clsDataConnect.pfOpen_Database(conDB) Then
                        Throw New Exception
                    End If

                    cmdDB = New SqlCommand("DLCINPP002_S3", conDB)
                    With cmdDB.Parameters
                        .Add(pfSet_Param("Proc_Cd", SqlDbType.SmallInt, 1))
                        .Add(pfSet_Param("Key", SqlDbType.NVarChar, Me.txtPicTboxId.ppText))
                    End With

                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                    If dstOrders.Tables.Count > 0 Then
                        If dstOrders.Tables(0).Rows.Count = 0 Then
                            ErrMsg = "入力されたTBOXID"
                            pfCheck_Txtbox = False
                        End If
                    End If

                Catch ex As Exception
                    psMesBox(Me, "0003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)

                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")

                Finally
                    'DB切断
                    If Not conDB Is Nothing Then
                        clsDataConnect.pfClose_Database(conDB)
                    End If
                End Try
            End If


        End If

        '---2018/4/13 小野 ここまで
    End Function

    ''' <summary>
    ''' ファイルアップロードコントロールの検証チェック
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub msCheck_Integrity()

        Dim Fullpath As HttpPostedFile = Nothing
        Dim strFullpath As String = Nothing
        Dim strFileName As String = Nothing
        Dim strExtension As String = Nothing
        Dim filesize As Long = Nothing
        Dim dtrErrMes As DataRow

        'アップロードファイルの有無
        If Not Me.trfUplordFileSelection.HasFile Then

            dtrErrMes = ClsCMCommon.pfGet_ValMes("5002")

            valfileUpload.Text = "未入力エラー"
            'valfileUpload.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
            valfileUpload.ErrorMessage = "ファイルが選択されていません。" 'DLCINPP002-003
            valfileUpload.Visible = True
            valfileUpload.Enabled = True
            valfileUpload.IsValid = False
            trfUplordFileSelection.Focus()

        Else

            'ファイル名を取得
            strFullpath = trfUplordFileSelection.PostedFile.FileName
            Fullpath = trfUplordFileSelection.PostedFile
            'ファイルの拡張子を取得
            strExtension = Path.GetExtension(Fullpath.FileName)

            '---2018/4/13 小野 ここから
            '設置環境写真
            If rdbMDNPic.Checked = True Then

                If Not strExtension = ".jpeg" And Not strExtension = ".jpg" And Not strExtension = ".JPG" And Not strExtension = ".JPEG" _
                    And Not strExtension = ".png" And Not strExtension = ".PNG" Then
                    dtrErrMes = ClsCMCommon.pfGet_ValMes("4004", strFullpath + "の拡張子")

                    valfileUpload.Text = "形式エラー"
                    valfileUpload.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
                    valfileUpload.Enabled = True
                    valfileUpload.IsValid = False
                End If


                Dim pcname = System.Net.Dns.GetHostName()
                Dim iphe As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(pcname)
                Dim adList As System.Net.IPAddress() = iphe.AddressList
                Dim i As Integer
                Dim ip As String = ""
                For i = 0 To adList.Length - 1
                    If adList(i).AddressFamily = Net.Sockets.AddressFamily.InterNetwork Then
                        ip = adList(i).ToString()
                    End If
                Next

                If Not Fullpath.ContentLength <= 3145728 Then
                    valfileUpload.ErrorMessage = "選択ファイル " & strFullpath & "のファイルサイズが大きすぎます。"

                    valfileUpload.Text = "ファイルサイズエラー"
                    valfileUpload.Enabled = True
                    valfileUpload.IsValid = False
                End If


                If System.IO.Path.GetFileName(strFullpath).Length >= 100 Then
                    valfileUpload.ErrorMessage = "選択ファイル " & strFullpath & "のファイル名が長すぎます。ファイル名は１００文字以内にしてください。"

                    valfileUpload.Text = "ファイル名エラー"
                    valfileUpload.Enabled = True
                    valfileUpload.IsValid = False


                End If
                '---2018/4/13 小野 ここまで

            End If
        End If

    End Sub

#End Region

End Class
