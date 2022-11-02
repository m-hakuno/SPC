'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　アップロード
'*　ＰＧＭＩＤ：　DLCINPP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.17　：　NKC
'********************************************************************************************************************************
#Region "更新履歴"
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'DLCINPP001-001     2015/06/11      加賀      BB1調査報告書のファイル名の変更(ホール名、TBOXIDをファイル名に追加)              
'DLCINPP001-002     2015/06/15      加賀      入力エラーメッセージ表示バグ修正
'DLCINPP001-003     2015/06/16      加賀      各依頼番号の形式・存在チェックの追加・修正   
'DLCINPP001-004     2016/05/06      加賀      SQLタイムアウトをエラーメッセージとして表示   
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

Public Class DLCINPP001
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
    Const M_DISP_ID = P_FUN_DLC & P_SCR_INP & P_PAGE & "001"    'プログラムID

    Const M_KENSYU_NAME As String = "KENSYUSYO"                 '工事完了報告書兼検収書
    Const M_DDL_NAME As String = "DLLHAISHIN"                   'DLL配信データ
    Const M_TOMAS_NAME As String = "TOMAS_HALL"                 'TOMASホール情報
    Const M_SAGYO_NAME As String = "HOSYUKANRYOU"               '保守完了報告書
    Const M_KASHITAMA_NAME As String = "KASHITAMA"              '貸玉数設定情報
    Const M_BBTYOUSA_NAME As String = "BBCHOUSA"                'BB調査報告書
    Const M_BBKOSEI_NAME As String = "BBKOUSEI"                 'BB構成ファイル
    '---2018/4/13 小野 ここから
    Const M_GENBAPIC_NAME As String = "GENBAPIC"                '設置環境写真
    '---2018/4/13 小野 ここまで

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
                enablestr = "','" + txtWorkRequestNo.ppTextBoxOne.ClientID + _
                                             "','" + txtWorkRequestNo.ppTextBoxTwo.ClientID + _
                                             "','" + txtRequestNo.ppTextBox.ClientID + _
                                             "','" + txtTboxId.ppTextBox.ClientID + _
                                             "','" + txtControlNo.ppTextBox.ClientID + _
                                             "','" + txtBBTboxId.ppTextBox.ClientID + _
                                             "','" + txtPicTboxId.ppTextBox.ClientID + _
                                              "')"

                '検収書
                enableScript = "rdioBtnChange('" + "1" + enablestr
                rdbAcceptanceCertificate.Attributes.Add("onChange", enableScript)

                'DLL配信データ
                enableScript = "rdioBtnChange('" + "2" + enablestr
                rdbDllDistributionDt.Attributes.Add("onChange", enableScript)

                'TOMASホール情報
                enableScript = "rdioBtnChange('" + "3" + enablestr
                rdbTomasHoleInfo.Attributes.Add("onChange", enableScript)

                '作業報告書
                enableScript = "rdioBtnChange('" + "4" + enablestr
                rdbWorkReport.Attributes.Add("onChange", enableScript)

                '貸玉数設定情報
                enableScript = "rdioBtnChange('" + "5" + enablestr
                rdbANOBallsSettingInfo.Attributes.Add("onChange", enableScript)

                'BB調査報告書
                enableScript = "rdioBtnChange('" + "6" + enablestr
                rdbBbInvestigativeReport.Attributes.Add("onChange", enableScript)

                'BB構成ファイル
                enableScript = "rdioBtnChange('" + "7" + enablestr
                rdbConstitutionFile.Attributes.Add("onChange", enableScript)

                '---2018/4/13 小野 ここから
                '設置環境写真
                enableScript = "rdioBtnChange('" + "8" + enablestr
                rdbGenbaPic.Attributes.Add("onChange", enableScript)
                '---2018/4/13 小野 ここまで

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

                '--------------------------------
                '2014/06/12 後藤　ここから
                '--------------------------------
                If Not Session(P_KEY) Is Nothing Then
                    Dim strKey() As String
                    ViewState(P_KEY) = Session(P_KEY)
                    strKey = ViewState(P_KEY)
                    strKey = strKey(0).Split("-")
                    txtWorkRequestNo.ppTextOne = strKey(0)
                    txtWorkRequestNo.ppTextTwo = strKey(1)
                End If
                '--------------------------------
                '2014/06/12 後藤　ここまで
                '--------------------------------

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
                rdbDllDistributionDt.Enabled = False
                rdbTomasHoleInfo.Enabled = False
                rdbBbInvestigativeReport.Enabled = False
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

                '検収書の工事依頼書存在確認チェック
                If rdbAcceptanceCertificate.Checked = True Then

                    '工事依頼書の存在確認
                    Select Case msGet_KojiCount()
                        Case "0" '工事依頼書が存在する

                            '★ロック対象テーブル名の登録
                            arTable_Name.Insert(0, "D39_CNSTREQSPEC")

                            '★ロックテーブルキー項目の登録(D39_CNSTREQSPEC)
                            arKey.Insert(0, Me.txtWorkRequestNo.ppTextOne + Me.txtWorkRequestNo.ppTextTwo)

                            '★排他情報確認処理
                            If clsExc.pfSel_Exclusive(strExclusiveDate _
                                             , Me _
                                             , Session(P_SESSION_IP) _
                                             , Session(P_SESSION_PLACE) _
                                             , Session(P_SESSION_USERID) _
                                             , Session(P_SESSION_SESSTION_ID) _
                                             , ViewState(P_SESSION_GROUP_NUM) _
                                             , M_DISP_ID _
                                             , arTable_Name _
                                             , arKey) = 0 Then

                                '★登録年月日時刻
                                Me.Master.ppExclusiveDate = strExclusiveDate

                            Else

                                '排他ロック中
                                Exit Sub

                            End If
                        Case "1"

                            '工事依頼書が存在しない
                            strErrmsg = M_ERRMSG3
                            Me.txtWorkRequestNo.Focus()
                            Throw New Exception

                        Case "2"

                            'エラーのため処理を終了
                            strErrmsg = Nothing
                            Me.txtWorkRequestNo.Focus()
                            Throw New Exception

                        Case "3"

                            'DB接続失敗
                            strErrmsg = Nothing
                            Me.txtWorkRequestNo.Focus()
                            Throw New Exception

                        Case "4"

                            'DB切断
                            strErrmsg = Nothing
                            Me.txtWorkRequestNo.Focus()
                            Throw New Exception

                    End Select

                End If

                If rdbTomasHoleInfo.Checked = True Then
                    If strExtension = ".txt" Then
                        strExtension = ".csv"
                    End If
                End If


                'アップロードファイルをローカルに保持
                localDirName = "UPLOAD"
                strLocaldir = "C:"
                strLocalPath = strLocaldir & "/" & localDirName & "/"

                If Not Fullpath.FileName = "" Then

                    'Try
                    '    Dim DirInfo As New System.IO.DirectoryInfo(strLocalPath)
                    '    Dim DirSec As DirectorySecurity

                    '    DirSec = DirInfo.GetAccessControl()

                    '    'アクセス権限を指定

                    '    'Everyoneに対し、フルコントロールの許可

                    '    '（サブフォルダ、及び、ファイルにも適用）

                    '    Dim AccessRule As New FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow)

                    '    'アクセス権限を追加

                    '    DirSec.AddAccessRule(AccessRule)

                    '    DirInfo.SetAccessControl(DirSec)

                    'Catch ex As Exception

                    '    Debug.WriteLine(ex.Message)

                    'End Try
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

                'BB構成ファイル(ファイルのバックアップ実行)
                If rdbConstitutionFile.Checked = True Then

                    Dim strFileNum() As String

                    If File.Exists("\\" + strSavePath + "\" + Me.txtBBTboxId.ppText) Then

                        strFileNum = Directory.GetFiles("\\" + strSavePath + "\" + Me.txtBBTboxId.ppText)


                        'ファイル数が0の場合
                        If strFileNum.Length <> 0 Then

                            Dim strCpPath = msGet_SavePath(False)
                            If strCpPath = Nothing Then

                                '取得エラーのため処理終了
                                strErrmsg = M_ERRMSG2
                                Throw New Exception

                            End If

                            'ファイルの移動を行う
                            For i As Integer = 0 To strFileNum.Length - 1

                                'バックアップ先ファイルの確認
                                If File.Exists("\\" + strCpPath + "\" + System.IO.Path.GetFileName(strFileNum(i))) Then

                                    File.Delete("\\" + strCpPath + "\" + System.IO.Path.GetFileName(strFileNum(i)))

                                End If

                                File.Move(strFileNum(i), "\\" + strCpPath + "\" + System.IO.Path.GetFileName(strFileNum(i)))

                            Next

                        End If

                    End If

                End If

                '----20140725 武 from
                'BB構成ファイル
                If rdbConstitutionFile.Checked = True Then

                    'TBOXIDの取得処理
                    strTBOXID = ms_MakeTempFile(strLocalPath)

                    If strTBOXID = Nothing Then

                        'strErrmsg = M_ERRMSG6
                        strErrmsg = "BB構成" 'DLCINPP001-003
                        Throw New Exception

                    End If

                    'TBOXIDが間違っている
                    If Not strTBOXID = Me.txtBBTboxId.ppText Then

                        'strErrmsg = M_ERRMSG6
                        strErrmsg = "BB構成TBOX"
                        Throw New Exception

                    End If

                End If
                '----20140725 武 to

                'DLCINPP001-002 移動
                '---2014/02/25 高松　ここから
                'DLL配信データ
                If rdbDllDistributionDt.Checked = True Then

                    'If ms_MakeTempFile(strFileName) Is Nothing Then
                    If ms_MakeTempFile(strLocalPath) Is Nothing Then
                        strErrmsg = "CSV"
                        Throw New Exception

                    End If

                End If

                'TOMASホール情報
                If rdbTomasHoleInfo.Checked = True Then

                    'If ms_MakeTempFile(strFileName) Is Nothing Then
                    If ms_MakeTempFile(strLocalPath) Is Nothing Then
                        strErrmsg = "CSV"
                        Throw New Exception

                    End If

                End If
                '---2014/02/25 高松　ここまで
                'DLCINPP001-002 END



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

                'アップロード
                'strUpladfile = strFileName
                'Fullpath.SaveAs(strUpladfile)

                'アップロードファイル存在確認(保存先)
                'If Not File.Exists(strUpladfile) Then

                '    'ファイルが存在しない
                '    strErrmsg = M_ERRMSG1
                '    Throw New Exception

                'End If

                'DLCINPP001-002 移動前

                ''BB構成ファイル
                'If rdbConstitutionFile.Checked = True Then

                '    'TBOXIDの取得処理
                '    strTBOXID = ms_MakeTempFile(strFileName)

                '    If strTBOXID = Nothing Then

                '        strErrmsg = M_ERRMSG6
                '        Throw New Exception

                '    End If

                '    'TBOXIDが間違っている
                '    If Not strTBOXID = Me.txtBBTboxId.ppText Then

                '        strErrmsg = M_ERRMSG6
                '        Throw New Exception


                '    End If

                'End If

                '保存後処理
                If rdbAcceptanceCertificate.Checked = True Then     '工事完了報告書兼検収書
                    upCls = "0"
                ElseIf rdbDllDistributionDt.Checked = True Then     'ＤＬＬ配信データ
                    upCls = "1"
                ElseIf rdbTomasHoleInfo.Checked = True Then         'ＴＯＭＡＳホール情報
                    upCls = "2"
                ElseIf rdbWorkReport.Checked = True Then            '保守完了報告書
                    upCls = "3"
                ElseIf rdbANOBallsSettingInfo.Checked = True Then   '貸玉数設定情報
                    upCls = "4"
                ElseIf rdbBbInvestigativeReport.Checked = True Then 'ＢＢ調査報告書
                    upCls = "5"
                ElseIf rdbConstitutionFile.Checked = True Then      'ＢＢ構成ファイル
                    upCls = "6"
                    '---2018/4/13 小野 ここから
                ElseIf rdbGenbaPic.Checked = True Then              '設置環境写真
                    upCls = "7"
                    '---2018/4/13 小野 ここまで
                End If


                '保存後処理(検収書)
                If upCls = "0" Then
                    '工事依頼書の検収書アップロードフラグを更新
                    strSavePath = "\\" & strSavePath
                    Select Case msUpd_Koji()
                        Case "0"

                            'ダウンロード情報の追加
                            '--2014/04/14 中川　ここから
                            ms_InsDownload(upCls, strFileName.Split("\"))
                            '--2014/04/14 中川　ここまで

                        Case "1"
                            '保存先のファイルを削除
                            System.IO.File.Delete(strFileName)

                            'コントロールのenableを反映させる
                            msChange_ctl()

                            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

                            Exit Sub

                        Case "2"
                            '保存先のファイルを削除
                            System.IO.File.Delete(strFileName)

                            'コントロールのenableを反映させる
                            msChange_ctl()

                            '排他ロックの解除

                            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

                            Exit Sub

                        Case "3"

                            '保存先のファイルを削除
                            System.IO.File.Delete(strUpladfile)

                            'コントロールのenableを反映させる
                            msChange_ctl()

                            '工事依頼書の更新失敗
                            strErrmsg = M_ERRMSG4
                            Throw New Exception("00001")

                    End Select

                    '保存後処理(TOMASホール情報)
                    If rdbTomasHoleInfo.Checked = True Then
                        'ホール情報更新処理の起動

                    End If

                Else
                    'ダウンロード情報の追加
                    '--2014/04/14 中川　ここから
                    Select Case upCls
                        Case "0", "3", "4", "5", "6", "7"
                            ms_InsDownload(upCls, strFileName.Split("\"))
                    End Select

                    '--2014/04/14 中川　ここまで
                End If

                ''保守完了報告書
                'If rdbWorkReport.Checked = True Or
                '    rdbANOBallsSettingInfo.Checked Then

                '    'ダウンロード情報の追加
                '    '--2014/04/14 中川　ここから
                '    ms_InsDownload("1", strFileName.Split("\"))
                '    '--2014/04/14 中川　ここまで

                'End If

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

            'DLCINPP001-004
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
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            'DLCINPP001-004 END
        Catch ex As Exception

            'DLCINPP001-003
            'If strErrmsg = Nothing Then
            '    'システムエラー
            '    psMesBox(Me, "30002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "アップロード")
            'Else
            '    'その他エラー
            '    'psMesBox(Me, "30011", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strErrmsg)    
            '    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, strErrmsg)
            'End If
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
            'DLCINPP001-003 END


            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
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
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
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
            txtWorkRequestNo.ppTextOne = "N0010"
            txtWorkRequestNo.ppTextTwo = String.Empty
            txtRequestNo.ppText = String.Empty
            txtTboxId.ppText = String.Empty
            txtControlNo.ppText = String.Empty
            txtBBTboxId.ppText = String.Empty
            '---2018/4/13 小野 ここから
            txtPicTboxId.ppText = String.Empty
            '---2018/4/13 小野 ここまで

            '表示の設定
            rdbAcceptanceCertificate.Checked = True
            rdbDllDistributionDt.Checked = False
            rdbTomasHoleInfo.Checked = False
            rdbWorkReport.Checked = False
            rdbANOBallsSettingInfo.Checked = False
            rdbBbInvestigativeReport.Checked = False
            rdbConstitutionFile.Checked = False
            txtWorkRequestNo.ppEnabled = True
            txtRequestNo.ppEnabled = False
            txtTboxId.ppEnabled = False
            txtControlNo.ppEnabled = False
            txtBBTboxId.ppEnabled = False
            '---2018/4/13 小野 ここから
            txtPicTboxId.ppEnabled = False
            rdbGenbaPic.Checked = False
            '---2018/4/13 小野 ここまで
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
    ''' 工事依頼番号の存在確認
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msGet_KojiCount() As String

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

            Dim strKojiNum As String = txtWorkRequestNo.ppTextOne + "-" + txtWorkRequestNo.ppTextTwo '工事依頼番号

            '開始ログ出力
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then

                Return "3"

            End If

            cmdDB = New SqlCommand(M_DISP_ID & "_S1", conDB) 'すべての表示情報を取得
            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("prm_kojinum", SqlDbType.NVarChar, strKojiNum))      '工事依頼番号
            End With

            'リストデータ取得
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            '取得件数の確認
            If CInt(dstOrders.Tables(0).Rows(0).Item("件数")) < 1 Then

                '工事依頼書が存在しない
                result = "1"

            End If

            '切断
            clsDataConnect.pfClose_Database(conDB)

            '終了ログ出力
            psLogEnd(Me)

            Return result

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
            '切断
            If Not conDB Is Nothing Then
                If Not clsDataConnect.pfClose_Database(conDB) Then

                    Return "4"

                End If
            End If

            '工事依頼番号の取得に失敗
            Return "2"

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
            '切断
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If

            'システムエラー
            Return "2"

        Finally
        End Try

    End Function

    ''' <summary>
    ''' 工事依頼書の更新
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msUpd_Koji() As String

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim trans As SqlClient.SqlTransaction
        Dim dstOrders As New DataSet
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then

            Return "1"

        End If

        trans = conDB.BeginTransaction            'トランザクション

        Try

            '工事依頼番号の設定
            Dim strKojiNum As String = txtWorkRequestNo.ppTextOne + "-" + txtWorkRequestNo.ppTextTwo

            '開始ログ出力
            psLogStart(Me)

            cmdDB = New SqlCommand(M_DISP_ID & "_U1", conDB)
            cmdDB.CommandType = CommandType.StoredProcedure
            cmdDB.Transaction = trans

            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("prm_kojinum", SqlDbType.NVarChar, strKojiNum))  '工事依頼番号
            End With

            'コマンドタイプ設定(ストアド)
            cmdDB.ExecuteNonQuery()

            'コミット
            trans.Commit()

            '切断
            clsDataConnect.pfClose_Database(conDB)

            Return "0"

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
            If Not conDB Is Nothing Then

                'ロールバック
                trans.Rollback()

                '切断
                If clsDataConnect.pfClose_Database(conDB) Then

                    Return "2"

                End If

            End If

            Return "3"

        Finally

            '★排他情報の削除

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function

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

            '検収書
            If rdbAcceptanceCertificate.Checked = True Then

                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_file", SqlDbType.NVarChar, M_FILE_TYPE1))      'ファイル種別
                End With


            End If

            'DLL配信データ
            If rdbDllDistributionDt.Checked = True Then

                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_file", SqlDbType.NVarChar, M_FILE_TYPE2))      'ファイル種別
                End With


            End If

            'TOMASホール情報
            If rdbTomasHoleInfo.Checked = True Then

                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_file", SqlDbType.NVarChar, M_FILE_TYPE3))      'ファイル種別
                End With


            End If

            '作業報告書
            If rdbWorkReport.Checked = True Then

                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_file", SqlDbType.NVarChar, M_FILE_TYPE4))      'ファイル種別
                End With


            End If

            '貸玉数設定情報
            If rdbANOBallsSettingInfo.Checked = True Then

                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_file", SqlDbType.NVarChar, M_FILE_TYPE5))      'ファイル種別
                End With


            End If

            'BB調査報告書
            If rdbBbInvestigativeReport.Checked = True Then

                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_file", SqlDbType.NVarChar, M_FILE_TYPE6))      'ファイル種別
                End With


            End If

            'BB構成ファイル
            If rdbConstitutionFile.Checked = True Then

                If flag = False Then

                    With cmdDB.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("prm_file", SqlDbType.NVarChar, M_FILE_TYPE8))      'ファイル種別
                    End With

                Else

                    With cmdDB.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("prm_file", SqlDbType.NVarChar, M_FILE_TYPE7))      'ファイル種別
                    End With

                End If

            End If

            '---2018/4/13 小野　ここから
            '設置環境写真
            If rdbGenbaPic.Checked = True Then

                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_file", SqlDbType.NVarChar, M_FILE_TYPE9))      'ファイル種別
                End With


            End If
            '---2018/4/13 小野　ここまで


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

            '検収書
            If rdbAcceptanceCertificate.Checked = True Then

                '[アップロード日(YYYYMMDD)]\ファイル名_[工事依頼番号1][工事依頼番号2].pdf
                filePath = "\\" + filePath + "\" + folderName
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

                fileName = M_KENSYU_NAME _
                           + "_" _
                           + txtWorkRequestNo.ppTextOne + txtWorkRequestNo.ppTextTwo _
                           + Extension
                'アップロード先のパスを作成
                makeDirPath &= filePath2

            End If

            'DLL配信データ
            If rdbDllDistributionDt.Checked = True Then

                '[TBOXID]\[アップロード日(YYYYMMDD)]\ファイル名.csv
                filePath = "\\" + filePath + "\" + TBOXID + folderName
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
                fileName = M_DDL_NAME _
                           + Extension
                'アップロード先のパスを作成
                makeDirPath &= filePath2
            End If

            'TOMASホール情報
            If rdbTomasHoleInfo.Checked = True Then

                '[アップロード日(YYYYMMDD)]\ファイル名.txt
                filePath = "\\" + filePath + "\" + folderName
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
                fileName = M_TOMAS_NAME _
                           + Extension
                'アップロード先のパスを作成
                makeDirPath &= filePath2
            End If

            '作業報告書
            If rdbWorkReport.Checked = True Then

                '[アップロード日(YYYYMMDD)]\ファイル名_[依頼番号].txt
                filePath = "\\" + filePath + "\" + folderName
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

                fileName = M_SAGYO_NAME _
                           + "_" _
                           + txtRequestNo.ppText _
                           + Extension
                'アップロード先のパスを作成
                makeDirPath &= filePath2

            End If

            '貸玉数設定情報
            If rdbANOBallsSettingInfo.Checked = True Then

                '[TBOXID]\[アップロード日(YYYYMMDD)]\ファイル名_[TBOXID].txt
                'filePath = "\\" + filePath + "\" + TBOXID + "\" + folderName
                filePath = "\\" + filePath + "\" + Me.txtTboxId.ppText
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

                fileName = M_KASHITAMA_NAME _
                           + "_" _
                           + txtTboxId.ppText _
                           + Extension
                'アップロード先のパスを作成
                makeDirPath &= filePath2
            End If

            'BB調査報告書
            If rdbBbInvestigativeReport.Checked = True Then
                'DLCINPP001-001
                Dim conDB As SqlConnection = Nothing
                Dim cmdDB As SqlCommand = Nothing
                Dim strHallNm As String = String.Empty
                Dim strTboxId As String = String.Empty
                If clsDataConnect.pfOpen_Database(conDB) Then
                    cmdDB = New SqlCommand("DLCINPP001_S3", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    With cmdDB.Parameters   'パラメータ設定
                        .Add(pfSet_Param("Proc_Cd", SqlDbType.SmallInt, 3))
                        .Add(pfSet_Param("Key", SqlDbType.NVarChar, txtControlNo.ppText))
                    End With
                    Using objDst = clsDataConnect.pfGet_DataSet(cmdDB)
                        If objDst.Tables(0).Rows.Count > 0 Then
                            strHallNm = objDst.Tables(0).Rows(0)("ホール名").ToString.Trim
                            strTboxId = objDst.Tables(0).Rows(0)("TBOXID").ToString
                        End If
                    End Using
                Else
                    Throw New Exception
                End If
                'DLCINPP001-001 END

                '[アップロード日(YYYYMMDD)]\ファイル名_[管理番号].txt_ホール名(TBOXID)
                filePath = "\\" + filePath + "\" + folderName
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

                fileName = M_BBTYOUSA_NAME _
                           + "_" _
                           + txtControlNo.ppText _
                           + "_" & strHallNm & "(" & strTboxId & ")" _
                           + Extension      'DLCINPP001-001 ホール名(TBOXID)追加
                'アップロード先のパスを作成
                makeDirPath &= filePath2
            End If

            'BB構成ファイル
            If rdbConstitutionFile.Checked = True Then

                '[TBOXID]\[アップロード日(YYYYMMDD)]\ファイル名
                filePath = "\\" + filePath + "\" + Me.txtBBTboxId.ppText
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

                fileName = folderName + "_" + M_BBKOSEI_NAME
                'アップロード先のパスを作成
                makeDirPath &= filePath2
            End If


            '---2018/4/13 小野 ここから
            '設置環境写真
            If rdbGenbaPic.Checked = True Then
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
                    fileName = M_GENBAPIC_NAME _
                           + "_" _
                           + txtPicTboxId.ppText _
                           + Extension
                End If
                'アップロード先のパスを作成
                makeDirPath &= filePath2
            End If
            '---2018/4/13 小野 ここまで


            '保存先のフォルダが存在しない場合作成する
            makeDir = New DirectoryInfo(makeDirPath)
            'If Not dirpath.Exists Then
            '    dirpath.Create()
            'End If

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
            If rdbDllDistributionDt.Checked = True Then
                selectPath = 1
            ElseIf rdbTomasHoleInfo.Checked = True Then
                selectPath = 1
            ElseIf rdbConstitutionFile.Checked = True Then
                selectPath = 1
            End If
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
    ''' 一時保存場所に移動する
    ''' </summary>
    ''' <param name="Fullpath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ms_MakeTempFile(ByVal Fullpath As String) As String

        Dim strTempPath As String = Nothing
        'Dim dirpath As DirectoryInfo
        Dim strTBOXID As String = Nothing
        Dim CsvDate As New ArrayList
        Dim Cnt As Integer = 0
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

            'BB構成情報
            If rdbConstitutionFile.Checked = True Then

                strTBOXID = msGet_Tboxid(M_DDL_NAME, Fullpath)
                If strTBOXID = Nothing Then

                    '取得エラーのため処理終了
                    Throw New Exception

                End If

                Return strTBOXID

            End If

            'DLL配信データ
            If rdbDllDistributionDt.Checked = True Then

                'CSVファイル取り込み
                ms_GetCSVDate(Fullpath, CsvDate, Cnt)

                '運用モード変更設定テーブルの更新
                ms_InsDllSend(CsvDate, Cnt)

                Return "0"

            End If

            'TOMASホール情報
            If rdbTomasHoleInfo.Checked = True Then

                Dim dt As New DataTable

                'CSVファイル取り込み
                ms_GetCSVDate(Fullpath, CsvDate, Cnt)

                '運用モード変更設定テーブルの更新
                ms_UpdTomasInfo(CsvDate, Cnt)

                Return "0"

            End If

            Return Nothing

            'DLCINPP001-004 SQL例外の時は呼び出し元に投げます
        Catch ex As SqlException

            Throw ex

            'DLCINPP001-004 END

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
            'アップロードファイル存在確認(一時保存先)
            If File.Exists(Fullpath) Then

                'ファイルを削除
                System.IO.File.Delete(Fullpath)

            End If

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

        '検収書
        If rdbAcceptanceCertificate.Checked = True Then

            txtWorkRequestNo.ppEnabled = True
            txtRequestNo.ppEnabled = False
            txtTboxId.ppEnabled = False
            txtControlNo.ppEnabled = False
            '---2014/03/02 高松　ここから
            txtBBTboxId.ppEnabled = False
            '---2014/03/02 高松　ここまで
            '---2018/4/13 小野 ここから
            txtPicTboxId.ppEnabled = False
            '---2018/4/13 小野 ここまで

        End If

        'DLL配信データ
        If rdbDllDistributionDt.Checked = True Then

            txtWorkRequestNo.ppEnabled = False
            txtRequestNo.ppEnabled = False
            txtTboxId.ppEnabled = False
            txtControlNo.ppEnabled = False
            '---2014/03/02 高松　ここから
            txtBBTboxId.ppEnabled = False
            '---2014/03/02 高松　ここまで
            '---2018/4/13 小野 ここから
            txtPicTboxId.ppEnabled = False
            '---2018/4/13 小野 ここまで

        End If

        'TOMASホール情報
        If rdbTomasHoleInfo.Checked = True Then

            txtWorkRequestNo.ppEnabled = False
            txtRequestNo.ppEnabled = False
            txtTboxId.ppEnabled = False
            txtControlNo.ppEnabled = False
            '---2014/03/02 高松　ここから
            txtBBTboxId.ppEnabled = False
            '---2014/03/02 高松　ここまで
            '---2018/4/13 小野 ここから
            txtPicTboxId.ppEnabled = False
            '---2018/4/13 小野 ここまで

        End If

        '作業報告書
        If rdbWorkReport.Checked = True Then

            txtWorkRequestNo.ppEnabled = False
            txtRequestNo.ppEnabled = True
            txtTboxId.ppEnabled = False
            txtControlNo.ppEnabled = False
            '---2014/03/02 高松　ここから
            txtBBTboxId.ppEnabled = False
            '---2014/03/02 高松　ここまで
            '---2018/4/13 小野 ここから
            txtPicTboxId.ppEnabled = False
            '---2018/4/13 小野 ここまで

        End If

        '貸玉数設定情報
        If rdbANOBallsSettingInfo.Checked = True Then

            txtWorkRequestNo.ppEnabled = False
            txtRequestNo.ppEnabled = False
            txtTboxId.ppEnabled = True
            txtControlNo.ppEnabled = False
            '---2014/03/02 高松　ここから
            txtBBTboxId.ppEnabled = False
            '---2014/03/02 高松　ここまで
            '---2018/4/13 小野 ここから
            txtPicTboxId.ppEnabled = False
            '---2018/4/13 小野 ここまで

        End If

        'BB調査報告書
        If rdbBbInvestigativeReport.Checked = True Then

            txtWorkRequestNo.ppEnabled = False
            txtRequestNo.ppEnabled = False
            txtTboxId.ppEnabled = False
            txtControlNo.ppEnabled = True
            '---2014/03/02 高松　ここから
            txtBBTboxId.ppEnabled = False
            '---2014/03/02 高松　ここまで
            '---2018/4/13 小野 ここから
            txtPicTboxId.ppEnabled = False
            '---2018/4/13 小野 ここまで

        End If

        'BB構成ファイル
        If rdbConstitutionFile.Checked = True Then

            txtWorkRequestNo.ppEnabled = False
            txtRequestNo.ppEnabled = False
            txtTboxId.ppEnabled = False
            txtControlNo.ppEnabled = False
            '---2014/03/02 高松　ここから
            txtBBTboxId.ppEnabled = True
            '---2014/03/02 高松　ここまで
            '---2018/4/13 小野 ここから
            txtPicTboxId.ppEnabled = False
            '---2018/4/13 小野 ここまで

        End If

        '---2018/4/13 小野 ここから
        '設置環境写真
        If rdbGenbaPic.Checked = True Then

            txtWorkRequestNo.ppEnabled = False
            txtRequestNo.ppEnabled = False
            txtTboxId.ppEnabled = False
            txtControlNo.ppEnabled = False
            txtBBTboxId.ppEnabled = False
            txtPicTboxId.ppEnabled = True

        End If
        '---2018/4/13 小野 ここまで

    End Sub

    ''' <summary>
    ''' CSVファイル取り込み
    ''' </summary>
    ''' <param name="Fullpath"></param>
    ''' <remarks></remarks>
    Private Sub ms_GetCSVDate(ByVal Fullpath As String, ByRef CSVDate As ArrayList, ByRef Cnt As Integer)

        Dim sr As StreamReader = New StreamReader(Fullpath, System.Text.Encoding.Default)  'ファイルの読み込み
        Dim errMsg As String = "ＣＳＶファイル"                                            'エラーメッセージ
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

            Dim strHead As String = sr.ReadLine                                                'ヘッダ行の読み込み
            Dim strRep As String = strHead.Replace(",", String.Empty)                          'カンマを削除
            Dim strLin As String = Nothing                                                     'CSVデータ(一行)
            Dim tmpLin As String = Nothing                                                     'CSVデータ(一時保管)
            Dim loopFlg As Boolean = False                                                     'ループ間のフラグ
            Dim loopCnt As Integer = 0                                                         'ループ回数
            Dim fstqt As Boolean = False                                                       '囲み開始フラグ

            Cnt = strHead.Length - strRep.Length                                               'カンマの数を算出

            Dim num As Integer = 0                                                             'カンマ数

            'CSVファイル内の整形開始
            '一行づつ読み込む
            Do Until sr.EndOfStream = True

                Dim strMoji As String = Nothing                                                '一文字格納

                strLin = sr.ReadLine '一行読み込み

                'CSVファイル読み込みカウントアップ
                loopCnt = loopCnt + 1

                'カンマの数を調べる
                '文字数分カウント
                For i As Integer = 0 To strLin.Length - 1
                    'For i As Integer = 0 To Cnt
                    '一文字ずつ抽出
                    strMoji = strLin.Substring(i, 1)

                    If strMoji = """" And fstqt = False Then '先頭の囲み

                        fstqt = True

                    ElseIf strMoji = "," And fstqt = True Then 'カンマが""で囲まれている

                        'カンマを別文字に置き換える
                        tmpLin = tmpLin + "‥"

                    ElseIf fstqt = False Then 'カンマが""で囲まれていない

                        '文字を連結
                        tmpLin = tmpLin + strMoji
                        'カンマ数をカウント
                        num = num + 1

                    ElseIf strMoji <> """" And fstqt = True Then '""で囲まれている文字

                        '文字を連結
                        tmpLin = tmpLin + strMoji

                    ElseIf strMoji = """" And fstqt = True Then '囲み終了

                        fstqt = False

                    End If

                Next

                If fstqt = False Then

                    'カンマの数が間違っている場合
                    If num <> Cnt Then

                        Throw New Exception

                    End If

                    CSVDate.Add(tmpLin)         '保存
                    tmpLin = Nothing
                    num = 0
                End If

            Loop

        Catch ex As Exception

            'システムエラー
            psMesBox(Me, "30002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後, errMsg)
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

            sr.Close()                 'ファイルクローズ

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub

    ''' <summary>
    ''' 運用モード設定変更テーブル追加
    ''' </summary>
    ''' <param name="CsvDate"></param>
    ''' <remarks></remarks>
    Private Sub ms_InsDllSend(CsvDate As ArrayList, ByVal Cnt As Integer)

        Dim conDB As SqlConnection = Nothing               'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing                  'SqlCommandクラス
        Dim objDs As DataSet = Nothing                     'データセット
        Dim strOKNG As String = Nothing                    '検索結果
        Dim strCsv() As String
        Dim dt As New DataTable
        Dim dstOrder As New DataSet
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------


        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try

                '開始ログ出力
                psLogStart(Me)

                'データ追加／更新
                Using conTrn = conDB.BeginTransaction

                    'データテーブルカラム作成(ヘッダ)
                    For i As Integer = 0 To Cnt + 1
                        dt.Columns.Add((i).ToString("D"))
                    Next

                    '行数分ループ
                    For i As Integer = 0 To CsvDate.Count - 1

                        strCsv = Nothing

                        'カンマ毎に分割
                        strCsv = CsvDate(i).ToString.Split(",")

                        '深夜営業確認
                        '--------------------------------
                        '2014/04/15 高松　ここから
                        '--------------------------------
                        'If strCsv(2) = M_SHINYA_FLAG then
                        If strCsv(2) = M_SHINYA_FLAG _
                            Or strCsv(2) = M_SYUYA_FLAG Then

                            ReDim Preserve strCsv(strCsv.Count)
                            strCsv(strCsv.Count - 1) = "1"

                        Else

                            ReDim Preserve strCsv(strCsv.Count)
                            strCsv(strCsv.Count - 1) = "0"

                        End If
                        '--------------------------------
                        '2014/04/15 高松　ここまで
                        '--------------------------------
                        dt.Rows.Add(strCsv)

                    Next

                    cmdDB = New SqlCommand(M_DISP_ID & "_I1", conDB)
                    With cmdDB.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("prm_dataTbl", SqlDbType.Structured, dt))                        'データセット
                        .Add(pfSet_Param("prm_UserId", SqlDbType.NVarChar, Session(P_SESSION_USERID)))  'ユーザID
                    End With

                    cmdDB.Transaction = conTrn
                    'コマンドタイプ設定(ストアド)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()
                    'コミット
                    conTrn.Commit()

                End Using

                '更新が正常終了
                'psMesBox(Me, "00003", clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後, "ＤＬＬ配信データ") 'DLCINPP001-002

            Catch ex As SqlException
                '更新に失敗
                'psMesBox(Me, "00001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "ＤＬＬ配信データ") 'DLCINPP001-002
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
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＬＬ配信データ")
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
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Throw New Exception
        End If

    End Sub

    ''' <summary>
    ''' TOMASホール情報の更新
    ''' </summary>
    ''' <param name="CsvDate"></param>
    ''' <param name="Cnt"></param>
    ''' <remarks></remarks>
    Private Sub ms_UpdTomasInfo(CsvDate As ArrayList, ByVal Cnt As Integer)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
        Dim strCsv() As String
        Dim dt As New DataTable
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then

            Try

                '開始ログ出力
                psLogStart(Me)

                'データ追加／更新
                Using conTrn = conDB.BeginTransaction

                    'データテーブルカラム作成(ヘッダ)
                    For i As Integer = 0 To Cnt
                        dt.Columns.Add((i).ToString("D"))
                    Next

                    '行数分ループ
                    For i As Integer = 0 To CsvDate.Count - 1

                        strCsv = Nothing

                        'カンマ毎に分割
                        strCsv = CsvDate(i).ToString.Split(",")

                        dt.Rows.Add(strCsv)

                    Next

                    cmdDB = New SqlCommand(M_DISP_ID & "_I2", conDB)
                    With cmdDB.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("prm_dataTbl", SqlDbType.Structured, dt))                            'データセット
                        .Add(pfSet_Param("prm_UserId", SqlDbType.NVarChar, Session(P_SESSION_USERID)))        'ユーザID
                    End With

                    cmdDB.Transaction = conTrn
                    'コマンドタイプ設定(ストアド)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()
                    'コミット
                    conTrn.Commit()

                End Using

                '更新が正常終了
                'psMesBox(Me, "00003", clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後, "ＴＯＭＡＳホール情報") 'DLCINPP001-002

            Catch ex As SqlException
                '更新に失敗
                'psMesBox(Me, "00001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "ＴＯＭＡＳホール情報") 'DLCINPP001-002
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
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＯＭＡＳホール情報")
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

        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)

        End If


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


        '工事依頼書(検収書)
        If strFlag = "0" Then

            strKanriNum = txtWorkRequestNo.ppTextOne + "-" + txtWorkRequestNo.ppTextTwo
            strFileType = M_FILE_TYPE1
            strReportName = rdbAcceptanceCertificate.Text
        End If

        'ＤＬＬ配信データ
        If strFlag = "1" Then
            strKanriNum = txtRequestNo.ppText
            strFileType = M_FILE_TYPE2
            strReportName = rdbWorkReport.Text
        End If

        'ＴＯＭＡＳホール情報
        If strFlag = "2" Then
            strKanriNum = txtRequestNo.ppText
            strFileType = M_FILE_TYPE3
            strReportName = rdbWorkReport.Text
        End If

        '保守完了報告書
        If strFlag = "3" Then
            strKanriNum = txtRequestNo.ppText
            strFileType = M_FILE_TYPE4
            strReportName = rdbWorkReport.Text
        End If

        '貸玉数設定情報
        If strFlag = "4" Then
            strKanriNum = txtTboxId.ppText
            strFileType = M_FILE_TYPE5
            strReportName = "玉単価設定情報"
        End If

        'ＢＢ調査報告書
        If strFlag = "5" Then
            strKanriNum = txtControlNo.ppText
            strFileType = "0903CL"
            strReportName = rdbBbInvestigativeReport.Text
        End If

        'ＢＢ構成ファイル
        If strFlag = "6" Then
            strKanriNum = txtBBTboxId.ppText
            strFileType = M_FILE_TYPE7
            strReportName = txtBBTboxId.ppText
        End If

        '設置環境写真
        If strFlag = "7" Then
            strKanriNum = txtPicTboxId.ppText
            strFileType = M_FILE_TYPE9
            strReportName = rdbGenbaPic.Text
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
                    Dim com As New SqlCommand("DLCINPP001_S4", conDB)
                    Dim com2 As New SqlCommand("DLCINPP001_S5", conDB)
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

        '工事完了報告書兼検収書
        If rdbAcceptanceCertificate.Checked = True Then

            strMaxLength = (txtWorkRequestNo.ppMaxLengthOne + txtWorkRequestNo.ppMaxLengthTwo).ToString

            '工事依頼番号1のチェック
            strResult = pfCheck_TxtErr(Me.txtWorkRequestNo.ppTextOne, True, False, True, True, txtWorkRequestNo.ppMaxLengthOne, String.Empty, False)
            If Not strResult = String.Empty Then

                Me.txtWorkRequestNo.psSet_ErrorNo(strResult, txtWorkRequestNo.ppName, strMaxLength)

            End If

            '初期化
            strResult = Nothing

            '工事依頼番号2のチェック
            strResult = pfCheck_TxtErr(Me.txtWorkRequestNo.ppTextTwo, True, True, True, True, txtWorkRequestNo.ppMaxLengthTwo, String.Empty, False)
            If Not strResult = String.Empty Then

                Me.txtWorkRequestNo.psSet_ErrorNo(strResult, txtWorkRequestNo.ppName, strMaxLength)

            End If


        End If

        '保守完了報告書
        If rdbWorkReport.Checked = True Then

            '依頼書番号のチェック
            strResult = pfCheck_TxtErr(Me.txtRequestNo.ppText, True, False, True, True, txtRequestNo.ppMaxLength, String.Empty, False)
            If Not strResult = String.Empty Then

                Me.txtRequestNo.psSet_ErrorNo(strResult, txtRequestNo.ppName, txtRequestNo.ppMaxLength.ToString)
            Else    'DLCINPP001-003
                '依頼番号存在チェック
                Try
                    'ＤＢ接続
                    If Not clsDataConnect.pfOpen_Database(conDB) Then
                        Throw New Exception
                    End If

                    cmdDB = New SqlCommand("DLCINPP001_S3", conDB)
                    With cmdDB.Parameters
                        .Add(pfSet_Param("Proc_Cd", SqlDbType.SmallInt, 2))
                        .Add(pfSet_Param("Key", SqlDbType.NVarChar, Me.txtRequestNo.ppText))
                    End With

                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                    If dstOrders.Tables.Count > 0 Then
                        If dstOrders.Tables(0).Rows.Count = 0 Then
                            ErrMsg = "入力された保守管理番号"
                            pfCheck_Txtbox = False
                        End If
                    End If

                Catch ex As Exception
                    psMesBox(Me, "0003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
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
                    'DB切断
                    If Not conDB Is Nothing Then
                        clsDataConnect.pfClose_Database(conDB)
                    End If
                End Try



            End If  'DLCINPP001-003 END

        End If

        '貸玉数設定情報
        If rdbANOBallsSettingInfo.Checked = True Then

            'TBOXIDのチェック
            'DLCINPP001-003
            'strResult = pfCheck_TxtErr(Me.txtTboxId.ppText, True, True, True, True, txtTboxId.ppMaxLength, String.Empty, False)
            'If Not strResult = String.Empty Then

            '    Me.txtTboxId.psSet_ErrorNo(strResult, txtTboxId.ppName, txtTboxId.ppMaxLength.ToString)
            If Regex.IsMatch(txtTboxId.ppText, "^[0-9]{8}$") = False Then
                Me.txtTboxId.psSet_ErrorNo("4001", txtTboxId.ppName, "半角数字８桁")
                'DLCINPP001-003 END
            Else
                'TBOXID存在確認
                Try
                    'ＤＢ接続
                    If Not clsDataConnect.pfOpen_Database(conDB) Then
                        Throw New Exception
                    End If

                    cmdDB = New SqlCommand("DLCINPP001_S3", conDB)
                    With cmdDB.Parameters
                        .Add(pfSet_Param("Proc_Cd", SqlDbType.SmallInt, 1))
                        .Add(pfSet_Param("Key", SqlDbType.NVarChar, Me.txtTboxId.ppText))
                    End With

                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                    If dstOrders.Tables.Count > 0 Then
                        If dstOrders.Tables(0).Rows.Count = 0 Then
                            'Me.txtTboxId.psSet_ErrorNo("2002", txtTboxId.ppName & " : " & txtTboxId.ppText, txtTboxId.ppMaxLength.ToString)
                            ErrMsg = "入力されたTBOXID"
                            pfCheck_Txtbox = False
                        End If
                    End If

                Catch ex As Exception
                    psMesBox(Me, "0003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
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
                    'DB切断
                    If Not conDB Is Nothing Then
                        clsDataConnect.pfClose_Database(conDB)
                    End If
                End Try
            End If

        End If

        'BB調査報告書
        If rdbBbInvestigativeReport.Checked = True Then

            '管理番号のチェック
            'DLCINPP001-003
            'strResult = pfCheck_TxtErr(Me.txtControlNo.ppText, True, False, True, True, txtControlNo.ppMaxLength, String.Empty, False)
            'If Not strResult = String.Empty Then

            '    Me.txtControlNo.psSet_ErrorNo(strResult, txtControlNo.ppName, txtControlNo.ppMaxLength.ToString)
            If Regex.IsMatch(txtControlNo.ppText, "^[a-zA-z0-9\-]{8}$") = False Then
                Me.txtControlNo.psSet_ErrorNo("4001", txtControlNo.ppName, "８桁の半角英数字 又は ハイフン'-'")
            Else
                '存在確認
                Try
                    'ＤＢ接続
                    If Not clsDataConnect.pfOpen_Database(conDB) Then
                        Throw New Exception
                    End If

                    cmdDB = New SqlCommand("DLCINPP001_S3", conDB)
                    With cmdDB.Parameters   'パラメータ設定
                        .Add(pfSet_Param("Proc_Cd", SqlDbType.SmallInt, 3))
                        .Add(pfSet_Param("Key", SqlDbType.NVarChar, txtControlNo.ppText))
                    End With

                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                    If dstOrders.Tables.Count > 0 Then
                        If dstOrders.Tables(0).Rows.Count = 0 Then
                            ErrMsg = "入力されたBB調査管理番号"
                            pfCheck_Txtbox = False
                        End If
                    End If

                Catch ex As Exception
                    psMesBox(Me, "0003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
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
                    'DB切断
                    If Not conDB Is Nothing Then
                        clsDataConnect.pfClose_Database(conDB)
                    End If
                End Try
                'DLCINPP001-003 END
            End If
        End If

        'BB構成ファイル
        If rdbConstitutionFile.Checked = True Then

            'TBOXIDのチェック
            'DLCINPP001-003
            'strResult = pfCheck_TxtErr(Me.txtBBTboxId.ppText, True, True, True, True, txtBBTboxId.ppMaxLength, String.Empty, False)
            'If Not strResult = String.Empty Then
            '    Me.txtBBTboxId.psSet_ErrorNo(strResult, txtBBTboxId.ppName, txtBBTboxId.ppMaxLength.ToString)
            'End If
            If Regex.IsMatch(txtBBTboxId.ppText, "^[0-9]{8}$") = False Then
                Me.txtBBTboxId.psSet_ErrorNo("4001", txtBBTboxId.ppName, "半角数字８桁")
            End If
            'DLCINPP001-003 END

        End If

        '---2018/4/13 小野 ここから
        '設置環境写真
        If rdbGenbaPic.Checked = True Then

            If Regex.IsMatch(txtPicTboxId.ppText, "^[0-9]{8}$") = False Then
                Me.txtPicTboxId.psSet_ErrorNo("4001", txtPicTboxId.ppName, "半角数字８桁")
            Else
                'TBOXID存在確認
                Try
                    'ＤＢ接続
                    If Not clsDataConnect.pfOpen_Database(conDB) Then
                        Throw New Exception
                    End If

                    cmdDB = New SqlCommand("DLCINPP001_S3", conDB)
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
            valfileUpload.ErrorMessage = "ファイルが選択されていません。" 'DLCINPP001-003
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

            '検収書
            If rdbAcceptanceCertificate.Checked = True Then

                If Not strExtension = ".pdf" Then

                    dtrErrMes = ClsCMCommon.pfGet_ValMes("4004", strFullpath + "の拡張子")

                    valfileUpload.Text = "形式エラー"
                    valfileUpload.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
                    valfileUpload.Enabled = True
                    valfileUpload.IsValid = False
                End If

            End If

            'DLL配信データ
            If rdbDllDistributionDt.Checked = True Then

                If Not strExtension = ".csv" And Not strExtension = ".txt" Then
                    dtrErrMes = ClsCMCommon.pfGet_ValMes("4004", strFullpath + "の拡張子")

                    valfileUpload.Text = "形式エラー"
                    valfileUpload.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
                    valfileUpload.Enabled = True
                    valfileUpload.IsValid = False
                End If

            End If

            'TOMASホール情報
            If rdbTomasHoleInfo.Checked = True Then
                If strExtension = ".txt" Then
                    strExtension = ".csv"
                End If
                If Not strExtension = ".csv" Then
                    dtrErrMes = ClsCMCommon.pfGet_ValMes("4004", strFullpath + "の拡張子")

                    valfileUpload.Text = "形式エラー"
                    valfileUpload.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
                    valfileUpload.Enabled = True
                    valfileUpload.IsValid = False
                End If

            End If

            '作業報告書
            If rdbWorkReport.Checked = True Then

                If Not strExtension = ".pdf" Then
                    dtrErrMes = ClsCMCommon.pfGet_ValMes("4004", strFullpath + "の拡張子")

                    valfileUpload.Text = "形式エラー"
                    valfileUpload.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
                    valfileUpload.Enabled = True
                    valfileUpload.IsValid = False
                End If

            End If

            '貸玉数設定情報
            If rdbANOBallsSettingInfo.Checked = True Then

                If Not strExtension = ".pdf" Then
                    dtrErrMes = ClsCMCommon.pfGet_ValMes("4004", strFullpath + "の拡張子")

                    valfileUpload.Text = "形式エラー"
                    valfileUpload.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
                    valfileUpload.Enabled = True
                    valfileUpload.IsValid = False
                End If

            End If

            'BB調査報告書
            If rdbBbInvestigativeReport.Checked = True Then

                If Not strExtension = ".pdf" Then
                    dtrErrMes = ClsCMCommon.pfGet_ValMes("4004", strFullpath + "の拡張子")

                    valfileUpload.Text = "形式エラー"
                    valfileUpload.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
                    valfileUpload.Enabled = True
                    valfileUpload.IsValid = False
                End If

            End If

            'BB構成ファイル
            If rdbConstitutionFile.Checked = True Then

                If Not strExtension = String.Empty Then
                    dtrErrMes = ClsCMCommon.pfGet_ValMes("4004", strFullpath + "の拡張子")

                    valfileUpload.Text = "形式エラー"
                    valfileUpload.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
                    valfileUpload.Enabled = True
                    valfileUpload.IsValid = False
                End If

            End If

            '---2018/4/13 小野 ここから
            '設置環境写真
            If rdbGenbaPic.Checked = True Then

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
