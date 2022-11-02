'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　
'*　ＰＧＭＩＤ：　
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　.02.18　：　ＸＸＸ
'*  更　新　　：　2014.07.03　：　間瀬      NL区分でNを指定しているものに対してJも参照するように変更
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
Imports System.IO
Imports System.Net
Imports System.String
#End Region

' この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://localhost:49741/SystemMaintenance/SMTSKF001/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SMTSKF001

#Region "継承定義"
    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
    Inherits System.Web.Services.WebService
#End Region

#Region "定数定義"
    '============================================================================================================================
    '=　定数定義
    '============================================================================================================================

    ''' <summary>
    ''' 出力ファイル名（先頭文字列共通）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strLOGFILE As String = "FTPRCV_"

    ''' <summary>
    ''' 出力ファイル名（拡張子）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strEXT_LOG As String = ".log"

    ''' <summary>
    ''' 出力ファイル名（連結文字列（エラー））
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strERR As String = "_ERR"

    ''' <summary>
    ''' エラーログ出力先（DB接続時、保存場所取得時）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strERRLOGPATH As String = "C:\NGC\FTP\LOG"

    ''' <summary>
    ''' 送信済み
    ''' </summary>
    ''' <remarks></remarks>
    Dim strSEND As String = "1"

    ''' <summary>
    ''' 受信済み
    ''' </summary>
    ''' <remarks></remarks>
    Dim strRECV As String = "2"

    ''' <summary>
    ''' JIS
    ''' </summary>
    ''' <remarks></remarks>
    Dim intENCODE As Integer = 50220

    ''' <summary>
    ''' SQL送信ファイル名（情報抽出）
    ''' </summary>
    ''' <remarks></remarks>
    Dim strSQLFILECHUSHUTU As String = "SPC_jchi_"

    ''' <summary>
    ''' SQL送信ファイル名（照会要求）
    ''' </summary>
    ''' <remarks></remarks>
    Dim strSQLFILEYOUKYU As String = "SPC_shyi_"

    ''' <summary>
    ''' SQL結果ファイル名（情報抽出）
    ''' </summary>
    ''' <remarks></remarks>
    Dim strSQLFILECHUSHUTUKEKA As String = "SPC_jchk_"

    ''' <summary>
    ''' SQL結果ファイル名（照会要求）
    ''' </summary>
    ''' <remarks></remarks>
    Dim strSQLFILEYOUKYUKEKA As String = "SPC_shyo_"

    ''' <summary>
    ''' SQL送信ファイル名（拡張子）
    ''' </summary>
    ''' <remarks></remarks>
    Dim strSQLFILENAMEKAKUCHOSHI As String = ".sql"

    ''' <summary>
    ''' SQL結果ファイル名（拡張子）
    ''' </summary>
    ''' <remarks></remarks>
    Dim strSQLKEKAFILENAMEKAKUCHOSHI As String = ".csv"

    ''' <summary>
    ''' SQL結果ファイル名（拡張子）
    ''' </summary>
    ''' <remarks></remarks>
    Dim strSQLFINAL As String = ".finish"

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

    ''' <summary>
    ''' エラー分類
    ''' </summary>
    ''' <remarks></remarks>
    Dim strErrBunrui As String = ""

    ''' <summary>
    ''' メソッド名称
    ''' </summary>
    ''' <remarks></remarks>
    Dim strMesod As String = ""

    ''' <summary>
    ''' テーブル名称
    ''' </summary>
    ''' <remarks></remarks>
    Dim strTable As String = ""

    ''' <summary>
    ''' データ（実行したSQL文等）
    ''' </summary>
    ''' <remarks></remarks>
    Dim strData As String = ""

    ''' <summary>
    ''' 内容
    ''' </summary>
    ''' <remarks></remarks>
    Dim strNaiyo As String = ""

    Dim clsDataConnect As New ClsCMDataConnect

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' ＦＴＰ受信
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function pfRecevFtp() As Boolean
        ' ＤＢ接続変数
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As New SqlCommand
        Dim strFilePath As String = ""
        Dim strRcvFilePath As String = ""
        Dim strBackUpFilePath As String = ""
        Dim strErrKbn As String = "0"
        Dim objDataSet As DataSet = Nothing
        Dim objDtSetConnect As DataSet = Nothing
        'URL
        Dim strUrlL As String = ""
        Dim strUrlN As String = ""
        'USERID
        Dim strUserIdL As String = ""
        Dim strUserIdN As String = ""
        'PassWord
        Dim strPassWordL As String = ""
        Dim strPassWordN As String = ""

        Try
            'ＤＢ接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                'トランザクション
                cmdDB.Connection = conDB
            Else
                strFilePath = strERRLOGPATH
                strErrBunrui = "ＤＢ接続"
                strMesod = "pfSendFtp"
                strTable = ""
                strData = ""
                Throw New Exception("データベースに接続できません")
            End If

            '保存場所取得処理（ログ出力先）
            If mfGetHozon(cmdDB, conDB, strFilePath) = False Then
                '取得できない場合は異常終了
                Return False
            End If

            '保存場所取得処理（受信ファイル一時作成場所）
            If mfGetHozonRcv(cmdDB, conDB, strRcvFilePath, strFilePath) = False Then
                '取得できない場合は異常終了
                Return False
            End If

            '保存場所取得処理（結果ファイルバックアップ場所）
            If mfGetHozonBackUp(cmdDB, conDB, strBackUpFilePath, strFilePath) = False Then
                '取得できない場合は異常終了
                Return False
            End If

            '照会要求データ取得処理
            If mfGetReferenceData(cmdDB, conDB, objDataSet, strFilePath, strErrKbn) = False Then
                '取得できない場合
                If strErrKbn = "0" Then
                    '照会要求データ取得処理で該当データがないため正常終了
                    Return True
                Else
                    '照会要求データ取得処理でCatchされたため異常終了
                    Return False
                End If
            End If

            '接続先情報取得処理
            If mfGetAccessPoint(cmdDB, conDB, objDtSetConnect, strFilePath) = False Then
                '取得できない場合
                Return False
            End If
            '取得できた内容を設定する
            Dim dtTableConnect As DataTable
            Dim objDataRowConnect As DataRow
            dtTableConnect = objDtSetConnect.Tables(0)
            For i = 0 To dtTableConnect.Rows.Count - 1
                objDataRowConnect = dtTableConnect.Rows(i)
                Select Case objDataRowConnect(3)
                    Case "L"
                        strUrlL = "ftp://" & objDataRowConnect(0) & "/"     'IPアドレス
                        strUserIdL = objDataRowConnect(1)   'ユーザID
                        strPassWordL = objDataRowConnect(2) 'パスワード
                    Case "N", "J"
                        strUrlN = "ftp://" & objDataRowConnect(0) & "/"     'IPアドレス
                        strUserIdN = objDataRowConnect(1)   'ユーザID
                        strPassWordN = objDataRowConnect(2) 'パスワード
                End Select

            Next

            Dim strUrl As String
            Dim strUrlUserId As String
            Dim strUrlPassWord As String

            For l = 0 To dtTableConnect.Rows.Count - 1
                If l = 0 Then
                    strUrl = strUrlL
                    strUrlUserId = strUserIdL
                    strUrlPassWord = strPassWordL
                Else
                    strUrl = strUrlN
                    strUrlUserId = strUserIdN
                    strUrlPassWord = strPassWordN
                End If
                'ＦＴＰ受信ファイル名取得
                Dim strGetNameListFinal As String() = Nothing
                If mfGetFtpFileName(strUrl, strUrlUserId, strUrlPassWord, strGetNameListFinal, strFilePath) = False Then
                    Return False
                End If

                Dim intGetCount As Integer = 0
                'ＦＴＰ受信
                If mfFtpGetCsvFile(cmdDB, conDB, strUrl, strUrlUserId, strUrlPassWord, strGetNameListFinal, intGetCount, _
                                   strRcvFilePath, strFilePath) = False Then
                    'FTP受信でエラーのため異常終了
                    If intGetCount = 0 Then
                        '最初の受信でエラーとなったため異常終了
                        '１件でも取得できた場合は処理を継続する
                        Return False
                    End If
                End If

                'ファイル名の取得
                Dim strFiles As String() = System.IO.Directory.GetFiles(strRcvFilePath, "*.csv", _
                                                                     System.IO.SearchOption.AllDirectories)
                Array.Sort(strFiles)

                '-----------------
                '各種ＤＢ登録処理
                '-----------------
                mfInsertDB(cmdDB, conDB, strFiles, strRcvFilePath, strFilePath)

                '-----------------------
                'CSVファイルバックアップ
                '-----------------------
                mfBackUpCsvFile(strRcvFilePath, strBackUpFilePath, strFilePath)
                'If mfInsertDB(cmdDB, conDB, strFiles, strRcvFilePath, strFilePath) = False Then
                'Else
                'End If

            Next
            'オブジェクトの破棄を保証する
            cmdDB.Dispose()

            Return True

        Catch ex As DBConcurrencyException
            Return False
        Catch ex As Exception
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        Finally
            'DB切断
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If

        End Try

    End Function

    ''' <summary>
    ''' ファイルバックアップ
    ''' </summary>
    ''' <param name="strRcvFilePath"></param>
    ''' <param name="strBackUpFilePath"></param>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfBackUpCsvFile(ByVal strRcvFilePath As String, ByVal strBackUpFilePath As String, _
                                     ByVal strFilePath As String) As Boolean
        Dim strName As String = Nothing
        Try
            Dim strFiles As String() = System.IO.Directory.GetFiles(strRcvFilePath, "*.*", _
                                                                 System.IO.SearchOption.AllDirectories)
            For i = 0 To strFiles.Count - 1
                strName = Path.GetFileName(strFiles(i))
                System.IO.File.Move(strRcvFilePath & "\" & strName, _
                                    strBackUpFilePath & "\" & strName)
            Next
        Catch ex As Exception
            strErrBunrui = "ファイルバックアップ"
            strTable = ""
            strData = strName
            strMesod = "mfBackUpCsvFile"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try
        Return True
    End Function

    ''' <summary>
    ''' 各種ＤＢ登録処理
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="strFiles"></param>
    ''' <param name="strRcvFilePath"></param>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertDB(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                ByVal strFiles() As String, ByVal strRcvFilePath As String, _
                                ByVal strFilePath As String) As Boolean
        Dim trans1 As SqlTransaction = Nothing
        Dim strFileNm As String = Nothing
        Try
            '取得したファイル数分繰り返す
            Dim objDatasetD182 As DataSet = Nothing
            For i = 0 To strFiles.Length - 1

                trans1 = conDB.BeginTransaction
                '情報抽出／要求照会ファイル発番抽出
                strFileNm = strFiles(i).Replace(strRcvFilePath & "\", "")
                'If mfGetD182ExtractIssuenum(cmdDB, conDB, trans1, objDatasetD182, _
                '                            strFiles(i).Replace(strRcvFilePath & "\", ""), strFilePath) = False Then
                If mfGetD182ExtractIssuenum(cmdDB, conDB, trans1, objDatasetD182, _
                                            strFileNm, strFilePath) = False Then
                    'ＤＢ検索で異常
                    '次のファイルを行う。
                    trans1.Rollback()
                    Continue For
                End If
                '情報抽出／要求照会ファイル発番抽出結果取得
                Dim dtTableD182 As DataTable
                Dim objDataRowD182 As DataRow
                Dim strD182TableName As String
                Dim strD182CtrlNo As String
                Dim strD182TboxId As String
                dtTableD182 = objDatasetD182.Tables(0)
                objDataRowD182 = dtTableD182.Rows(0)
                strD182TableName = objDataRowD182(0) '対象テーブル名
                strD182CtrlNo = objDataRowD182(1)    '照会管理番号
                strD182TboxId = objDataRowD182(2)    'TBOXID

                Dim objDatasetD83 As DataSet = Nothing
                '照会要求データ、処理コード取得
                If mfGetD83ReferenceData(cmdDB, conDB, trans1, objDatasetD83, strD182CtrlNo, strFilePath, strFileNm) = False Then
                    'ＤＢ検索で異常
                    '次のファイルを行う
                    trans1.Rollback()
                    Continue For
                End If
                Dim dtTableD83 As DataTable
                Dim objDataRowD83 As DataRow
                Dim strD83ProcCd As String
                dtTableD83 = objDatasetD83.Tables(0)
                objDataRowD83 = dtTableD83.Rows(0)
                strD83ProcCd = objDataRowD83(0)     '処理コード

                '照会要求依頼の有無判定
                Dim strSyoukaiiraiUmuFlg As String = "0"
                Select Case strD83ProcCd
                    Case "200", "301", "302", "100"
                        strSyoukaiiraiUmuFlg = "1"
                    Case "401", "402", "901", "902", "501", "502", "503", "504", "505", "506", "507", "508"
                        strSyoukaiiraiUmuFlg = "0"
                End Select

                'CSVファイルの内容をデータテーブルに書き込む
                Dim dtCsvTable As New DataTable
                If mfSetCsvToDataTable(strFiles(i), dtCsvTable, strRcvFilePath, strFilePath, strFileNm) = False Then
                    'データテーブルへの書き込みで異常
                    '次のファイルを行う
                    trans1.Rollback()
                    Continue For
                End If

                Dim strCtrlNo As String
                Dim strSeq As String
                Dim strNlCls As String
                Dim strIdIcCls As String
                Dim strNewCtrlNo As String
                Dim objSabun As DataSet = Nothing
                If strSyoukaiiraiUmuFlg = "1" Then
                    '照会要求依頼ありの場合
                    Select Case strD182TableName
                        Case "D101_INQRECEPT"
                            Dim objDataRowD101 As DataRow
                            Dim strD101Result As String
                            Dim strD101ResultNo As String
                            Dim strD101ReqSeq As String
                            Dim strD101ReqDate As String
                            objDataRowD101 = dtCsvTable.Rows(0)
                            strD101Result = objDataRowD101(0)    '受付結果
                            strD101ResultNo = objDataRowD101(1)  '受付エラー以来番号
                            strD101ReqSeq = objDataRowD101(2)    '要求日付
                            strD101ReqDate = objDataRowD101(3)   '要求通番
                            If mfUpdateD101Inqrecept(cmdDB, conDB, trans1, strD182CtrlNo, strD101Result, _
                                                     strD101ResultNo, strD101ReqSeq, strD101ReqDate, strFilePath, strFileNm) = False Then
                                'D101テーブルへの更新で異常
                                '次のファイルを行う
                                trans1.Rollback()
                                Continue For
                            End If
                        Case Else
                            Dim dtTableD83TboxId As DataTable
                            Dim objDataRowD83TboxId As DataRow
                            '照会要求受付ＤＢ検索
                            If mfGetD101InqreceptSql(cmdDB, conDB, trans1, strD182CtrlNo, objSabun, strFilePath, strFileNm) = False Then
                                'D101テーブルの検索で異常
                                '次のファイルを行う
                                trans1.Rollback()
                                Continue For
                            End If
                            dtTableD83TboxId = objSabun.Tables(0)
                            objDataRowD83TboxId = dtTableD83TboxId.Rows(0)
                            strCtrlNo = objDataRowD83TboxId(0)
                            strSeq = objDataRowD83TboxId(1)
                            strNlCls = objDataRowD83TboxId(2)
                            strIdIcCls = objDataRowD83TboxId(3)
                            If objDataRowD83TboxId(4) Is DBNull.Value Then
                                strNewCtrlNo = ""
                            Else
                                strNewCtrlNo = objDataRowD83TboxId(4)
                            End If

                            Select Case strD182TableName
                                Case "D129_REQFSE"
                                    If mfInsertD129Reqfse(cmdDB, conDB, trans1, strCtrlNo, strSeq, strNlCls, _
                                                          strIdIcCls, strNewCtrlNo, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D129テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                                Case "D130_REQFSH"
                                    If mfInsertD130Reqfsh(cmdDB, conDB, trans1, strCtrlNo, strSeq, strNlCls, _
                                                          strIdIcCls, strNewCtrlNo, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D130テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                                Case "D135_BBKIBAN"
                                    If mfInsertD135Bbkiban(cmdDB, conDB, trans1, strCtrlNo, strSeq, strNlCls, _
                                                           strIdIcCls, strNewCtrlNo, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D135テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                                Case "D162_BBKIBAN2"
                                    If mfInsertD162Bbkiban2(cmdDB, conDB, trans1, strCtrlNo, strSeq, strNlCls, _
                                                            strIdIcCls, strNewCtrlNo, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D162テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                                Case "D133_SOUTIKOSEI"
                                    If mfInsertD133Soutikosei(cmdDB, conDB, trans1, strCtrlNo, strSeq, strNlCls, _
                                                              strIdIcCls, strNewCtrlNo, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D133テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                                Case "D147_SAIKENMEISAI"
                                    If mfInsertD147Saikenmeisai(cmdDB, conDB, trans1, strCtrlNo, strSeq, strNlCls, _
                                                                strIdIcCls, strNewCtrlNo, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D147テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                                Case "D148_SAIMUMEISAI"
                                    If mfInsertD148Saimumeisai(cmdDB, conDB, trans1, strCtrlNo, strSeq, strNlCls, _
                                                               strIdIcCls, strNewCtrlNo, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D148テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                                Case "D150_SEISANMEISAI"
                                    If mfInsertD150Seisanmeisai(cmdDB, conDB, trans1, strCtrlNo, strSeq, strNlCls, _
                                                                strIdIcCls, strNewCtrlNo, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D150テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                                Case "D131_SIYOUCHUCARD"
                                    If mfInsertD131Siyouchucard(cmdDB, conDB, trans1, strCtrlNo, strSeq, strNlCls, _
                                                                strIdIcCls, strNewCtrlNo, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D131テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                                Case "D132_KESSAISHOUKAI"
                                    If mfInsertD132Kessaishoukai(cmdDB, conDB, trans1, strCtrlNo, strSeq, strNlCls, _
                                                                 strIdIcCls, strNewCtrlNo, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D132テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                                Case "D136_BBSOUTI"
                                    If mfInsertD136Bbsouti(cmdDB, conDB, trans1, strCtrlNo, strSeq, strNlCls, _
                                                           strIdIcCls, strNewCtrlNo, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D136テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                                Case "D143_TBOXSOUSALOG"
                                    If mfInsertD143Tboxsousalog(cmdDB, conDB, trans1, strCtrlNo, strSeq, strNlCls, _
                                                                strIdIcCls, strNewCtrlNo, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D143テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                                Case "D144_TBOXERRLOG"
                                    If mfInsertD144Tboxerrlog(cmdDB, conDB, trans1, strCtrlNo, strSeq, strNlCls, _
                                                              strIdIcCls, strNewCtrlNo, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D144テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                                Case "D156_SIYOUCHUCARD2"
                                    If mfInsertD156Siyouchucard2(cmdDB, conDB, trans1, strCtrlNo, strSeq, strNlCls, _
                                                                 strIdIcCls, strNewCtrlNo, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D156テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                            End Select
                    End Select
                Else
                    '照会要求依頼なしの場合
                    'TBOXIDを取得
                    If mfGetTboxId(cmdDB, conDB, trans1, strD182CtrlNo, objSabun, strFilePath, strFileNm) = False Then
                        'TBOXの取得で異常
                        '次のファイルを行う
                        trans1.Rollback()
                        Continue For
                    End If
                    Dim dtTableD83TboxId As DataTable
                    Dim objDataRowD83TboxId As DataRow
                    Dim strD83TboxId As String
                    Dim strT03NL As String
                    Dim strT03SystemBunrui As String
                    dtTableD83TboxId = objSabun.Tables(0)
                    objDataRowD83TboxId = dtTableD83TboxId.Rows(0)
                    strD83TboxId = objDataRowD83TboxId(0)     'TBOXID
                    'NL区分、システム分類を取得
                    If mfGetNlAndSystemBunrui(cmdDB, conDB, trans1, strD83TboxId, objSabun, strFilePath, strFileNm) = False Then
                        'NL区分、システム分類の取得で異常
                        '次のファイルを行う
                        trans1.Rollback()
                        Continue For
                    End If
                    dtTableD83TboxId = objSabun.Tables(0)
                    objDataRowD83TboxId = dtTableD83TboxId.Rows(0)
                    strT03NL = objDataRowD83TboxId(0)
                    strT03SystemBunrui = objDataRowD83TboxId(1)

                    strCtrlNo = strD182CtrlNo
                    strSeq = "1"
                    strNlCls = strT03NL
                    If strT03SystemBunrui = "1" Then
                        strIdIcCls = "01"
                    Else
                        strIdIcCls = "02"
                    End If

                    Select Case strD83ProcCd
                        Case "401", "402"
                            If mfInsertD103TmHisnKnr(cmdDB, conDB, trans1, strCtrlNo, strSeq, _
                                                     strNlCls, strIdIcCls, dtCsvTable, strFilePath, strFileNm) = False Then
                                'D103テーブルへの登録で異常
                                '次のファイルを行う
                                trans1.Rollback()
                                Continue For
                            End If
                        Case "901", "902"
                            If mfInsertD104TmShsnKnr(cmdDB, conDB, trans1, strCtrlNo, strSeq, _
                                                     strNlCls, strIdIcCls, dtCsvTable, strFilePath, strFileNm) = False Then
                                'D104テーブルへの登録で異常
                                '次のファイルを行う
                                trans1.Rollback()
                                Continue For
                            End If
                        Case "501", "502", "503", "504", "505", "506", "507", "508"
                            Select Case strD182TableName
                                Case "D108_TT_HJT_DB_TBXSJ"
                                    If mfInsertD108TtHjtDbTbxsj(cmdDB, conDB, trans1, strCtrlNo, strSeq, _
                                                                strNlCls, strIdIcCls, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D108テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                                Case "D110_TT_HJT_DB_BBUJ"
                                    If mfInsertD110TtHjtDbBbuj(cmdDB, conDB, trans1, strCtrlNo, strSeq, _
                                                               strNlCls, strIdIcCls, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D110テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                                Case "D114_TT_HJT_DB_BBSJKNBINYKNK"
                                    If mfInsertD114TtHjtDbBbsjknbinyknk(cmdDB, conDB, trans1, strCtrlNo, strSeq, _
                                                                        strNlCls, strIdIcCls, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D114テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                                Case "D116_TT_HJT_DB_BBSJSNDSISNK"
                                    If mfInsertD116TtHjtDbBbsjsndsisnk(cmdDB, conDB, trans1, strCtrlNo, strSeq, _
                                                                       strNlCls, strIdIcCls, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D116テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                                Case "D118_TT_HJT_DB_BBSJTURKUKTSKK"
                                    If mfInsertD118TtHjtDbBbsjturkuktskk(cmdDB, conDB, trans1, strCtrlNo, strSeq, _
                                                                         strNlCls, strIdIcCls, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D118テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                                Case "D112_TT_HJT_DB_BBUJNVC"
                                    If mfInsertD112TtHjtDbBbujnvc(cmdDB, conDB, trans1, strCtrlNo, strSeq, _
                                                                  strNlCls, strIdIcCls, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D112テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                                Case "D122_TT_HJT_DB_BBSJSNDNVC"
                                    If mfInsertD122TtHjtDbBbsjsndnvc(cmdDB, conDB, trans1, strCtrlNo, strSeq, _
                                                                     strNlCls, strIdIcCls, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D122テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                                Case "D120_TT_HJT_DB_BBSJSISNKNVC"
                                    If mfInsertD120TtHjtDbBbsjsisnknvc(cmdDB, conDB, trans1, strCtrlNo, strSeq, _
                                                                       strNlCls, strIdIcCls, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D120テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                                Case "D103_TM_HISN_KNR"
                                    If mfInsertD103TmHisnKnr(cmdDB, conDB, trans1, strCtrlNo, strSeq, _
                                                             strNlCls, strIdIcCls, dtCsvTable, strFilePath, strFileNm) = False Then
                                        'D103テーブルへの登録で異常
                                        '次のファイルを行う
                                        trans1.Rollback()
                                        Continue For
                                    End If
                            End Select
                    End Select
                End If

                '------------------------------
                '対象データ種別取得完了判定処理
                '------------------------------
                Dim bltKanryoFlg As Boolean
                '対象テーブルがD103_TM_HISN_KNR、D104_TM_SHSN_KNRの場合
                Select Case strD182TableName
                    Case "D103_TM_HISN_KNR", "D104_TM_SHSN_KNR"
                        Dim objDataSetKanryouHantei As DataSet = Nothing
                        Dim dtTable As DataTable
                        Dim dtTableMoto As DataTable
                        Dim dtTableSaki As DataTable
                        Dim objDataRow As DataRow
                        Dim objDataRowSaki As DataRow
                        Dim objDataRowMoto As DataRow
                        Dim strDataSyubetu As String
                        Dim strTboxSyubetuVer As String
                        Dim strTaishouDataSyubetu As String
                        Dim intMatchCount As Integer = 0

                        '集信（配信）結果取得済データ種別取得
                        If strD182TableName = "D103_TM_HISN_KNR" Then
                            If mfGetDataSyubetu(cmdDB, conDB, trans1, "0", strD182CtrlNo, objDataSetKanryouHantei, strFilePath, strFileNm) = False Then
                                trans1.Rollback()
                                Return False
                            End If
                        Else
                            If mfGetDataSyubetu(cmdDB, conDB, trans1, "1", strD182CtrlNo, objDataSetKanryouHantei, strFilePath, strFileNm) = False Then
                                trans1.Rollback()
                                Return False
                            End If
                        End If
                        dtTableSaki = objDataSetKanryouHantei.Tables(0)

                        'ＴＢＯＸ種別（Ｖｅｒ込）取得
                        If mgGetTboxSyubetuVer(cmdDB, conDB, trans1, strD182TboxId, objDataSetKanryouHantei, strFilePath, strFileNm) = False Then
                            trans1.Rollback()
                            Return False
                        End If
                        dtTable = objDataSetKanryouHantei.Tables(0)
                        objDataRow = dtTable.Rows(0)
                        strTboxSyubetuVer = objDataRow(0) 'ＴＢＯＸ種別（Ｖｅｒ込）

                        '集信（配信）対象データ種別取得
                        If strD182TableName = "D103_TM_HISN_KNR" Then
                            If mfGetTaishoDataSyubetu(cmdDB, conDB, trans1, "0", strTboxSyubetuVer, objDataSetKanryouHantei, strFilePath, strFileNm) = False Then
                                trans1.Rollback()
                                Return False
                            End If
                        Else
                            If mfGetTaishoDataSyubetu(cmdDB, conDB, trans1, "1", strTboxSyubetuVer, objDataSetKanryouHantei, strFilePath, strFileNm) = False Then
                                trans1.Rollback()
                                Return False
                            End If
                        End If
                        dtTableMoto = objDataSetKanryouHantei.Tables(0)
                        'objDataRowMoto = dtTable.Rows(0)

                        '対象データ種別を元に結果取得済データ種別を比較する
                        For j = 0 To dtTableMoto.Rows.Count - 1
                            objDataRowMoto = dtTableMoto.Rows(j)
                            strTaishouDataSyubetu = objDataRowMoto(0)
                            For k = 0 To dtTableSaki.Rows.Count - 1
                                objDataRowSaki = dtTableSaki.Rows(k)
                                strDataSyubetu = objDataRowSaki(0)
                                '一致しているか確認
                                If strTaishouDataSyubetu = strDataSyubetu Then
                                    '一致している場合は件数をカウント
                                    intMatchCount = intMatchCount + 1
                                    Exit For
                                End If
                            Next
                        Next
                        bltKanryoFlg = False
                        If dtTableMoto.Rows.Count = intMatchCount Then
                            'すべて一致
                            bltKanryoFlg = True
                        End If
                End Select

                'ＴＢＯＸ随時照会（処理コードが100、200、301、302）の場合
                Select Case strD83ProcCd
                    Case "100", "200", "301", "302"
                        Dim objDataSetKanryouHantei As DataSet = Nothing
                        Dim dtTableMoto As DataTable
                        Dim dtTableSaki As DataTable
                        Dim objDataRowSaki As DataRow
                        Dim objDataRowMoto As DataRow
                        Dim strDataSyubetu As String
                        Dim strTaishouDataSyubetu As String
                        Dim intMatchCount As Integer = 0
                        '照会要求ＤＢ取得ＡＬＬ
                        If mfGetD101InqreceptSqlAll(cmdDB, conDB, trans1, strD182CtrlNo, objDataSetKanryouHantei, strFilePath, strFileNm) = False Then
                            trans1.Rollback()
                            Return False
                        End If
                        dtTableMoto = objDataSetKanryouHantei.Tables(0)

                        '照会要求親、照会要求明細データ取得
                        If mfGetYoukyuOyaMeisai(cmdDB, conDB, trans1, strD182CtrlNo, objDataSetKanryouHantei, strFilePath, strFileNm) = False Then
                            trans1.Rollback()
                            Return False
                        End If
                        dtTableSaki = objDataSetKanryouHantei.Tables(0)

                        '対象データ種別を元に結果取得済データ種別を比較する
                        For j = 0 To dtTableMoto.Rows.Count - 1
                            objDataRowMoto = dtTableMoto.Rows(j)
                            strTaishouDataSyubetu = objDataRowMoto(0)
                            For k = 0 To dtTableSaki.Rows.Count - 1
                                objDataRowSaki = dtTableSaki.Rows(k)
                                strDataSyubetu = objDataRowSaki(0)
                                '一致しているか確認
                                If strTaishouDataSyubetu = strDataSyubetu Then
                                    '一致している場合は件数をカウント
                                    intMatchCount = intMatchCount + 1
                                    dtTableSaki.Rows(k).Delete()
                                    dtTableSaki.AcceptChanges()
                                    Exit For
                                End If
                            Next
                        Next
                        bltKanryoFlg = False
                        If dtTableMoto.Rows.Count = intMatchCount Then
                            'すべて一致
                            bltKanryoFlg = True
                        End If
                End Select

                '------------------------------
                '照会要求データ更新処理（完了）
                '------------------------------
                Dim objDataSetKoushin As DataSet = Nothing
                Dim dtTableKanryou As DataTable
                Dim objDataRowKanryou As DataRow
                Dim dtmReqDt As DateTime
                Dim strReqSeq As String
                Dim strMoveCls As String
                If mfGetD83ReferenceDataKanryou(cmdDB, conDB, trans1, strD182CtrlNo, objDataSetKoushin, strFilePath, strFileNm) = False Then
                    trans1.Rollback()
                    Return False
                End If
                dtTableKanryou = objDataSetKoushin.Tables(0)
                objDataRowKanryou = dtTableKanryou.Rows(0)
                dtmReqDt = objDataRowKanryou(0)
                strReqSeq = objDataRowKanryou(1)
                strMoveCls = objDataRowKanryou(2)

                If bltKanryoFlg = True Then
                    '判定結果が完了の場合
                    If mfUpdateD83ReferenceData(cmdDB, conDB, trans1, "1", dtmReqDt, strReqSeq, strMoveCls, strFilePath, strFileNm) = False Then
                        trans1.Rollback()
                        Return False
                    End If
                Else
                    '判定結果が未完了の場合
                    If mfUpdateD83ReferenceData(cmdDB, conDB, trans1, "0", dtmReqDt, strReqSeq, strMoveCls, strFilePath, strFileNm) = False Then
                        trans1.Rollback()
                        Return False
                    End If
                End If

                '----------------------
                '照会要求データ登録処理
                '----------------------
                If bltKanryoFlg = False Then
                    If strSyoukaiiraiUmuFlg = "1" Then
                        If strD182TableName <> "D101_INQRECEPT" Then
                            If mfSelectInsert(cmdDB, conDB, trans1, dtmReqDt, strReqSeq, strMoveCls, strFilePath, strFileNm) = False Then
                                trans1.Rollback()
                                Return False
                            End If
                        End If
                    End If
                End If

                trans1.Commit()
            Next
            Return True
        Catch ex As Exception
            trans1.Rollback()
            strErrBunrui = "その他エラー"
            strMesod = "mfSelectInsert"
            strTable = ""
            strData = strFileNm
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 照会要求データ取得、登録情報
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="dtmReqDt"></param>
    ''' <param name="strReqSeq"></param>
    ''' <param name="strMoveCls"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfSelectInsert(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                    ByVal trans1 As SqlTransaction, ByVal dtmReqDt As DateTime, ByVal strReqSeq As String, _
                                    ByVal strMoveCls As String, ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I25", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmREQ_DT", SqlDbType.DateTime, dtmReqDt))
                .Add(pfSet_Param("prmREQ_SEQ", SqlDbType.NVarChar, strReqSeq))
                .Add(pfSet_Param("prmMOVE_CLS", SqlDbType.NVarChar, strMoveCls))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D83_REFERENCE_DATA"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I25"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfSelectInsert"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 照会要求データ更新
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strKbn"></param>
    ''' <param name="dtmReqDt"></param>
    ''' <param name="strReqSeq"></param>
    ''' <param name="strMoveCls"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfUpdateD83ReferenceData(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                              ByVal trans1 As SqlTransaction, ByVal strKbn As String, ByVal dtmReqDt As DateTime, ByVal strReqSeq As String, _
                                              ByVal strMoveCls As String, ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try
            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_U2", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmKbn", SqlDbType.NVarChar, strKbn))
                .Add(pfSet_Param("prmD83_REQ_DT", SqlDbType.DateTime, dtmReqDt))
                .Add(pfSet_Param("prmD83_REQ_SEQ", SqlDbType.NVarChar, strReqSeq))
                .Add(pfSet_Param("prmD83_MOVE_CLS", SqlDbType.NVarChar, strMoveCls))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D83_REFERENCE_DATA"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_U2"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfUpdateD83ReferenceData"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 照会要求データ取得（完了）
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="objDataSetKanryouHantei"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetD83ReferenceDataKanryou(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                                  ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByRef objDataSetKanryouHantei As DataSet, _
                                                  ByVal strFilePath As String, ByVal strFileNm As String) As Boolean
        Try
            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_S17", conDB)

            cmdDB.Parameters.Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
            cmdDB.Parameters.Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            'トランザクション
            cmdDB.Transaction = trans1

            '照会要求データ取得処理
            objDataSetKanryouHantei = clsDataConnect.pfGet_DataSet(cmdDB)
            Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

            If (Not objDataSetKanryouHantei Is Nothing) And objDataSetKanryouHantei.Tables.Count > 0 And strReturn <> "0" Then
                '該当データありの場合は既にDataSetに格納されているため何もしない。
            Else
                '取得データなし
                Return False
            End If
            '11111            End Using
            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetD83ReferenceDataKanryou"
            strTable = "D83_REFERENCE_DATA"
            strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_S17"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 照会要求親、照会要求明細データ取得
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="objDataSetKanryouHantei"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetYoukyuOyaMeisai(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                          ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByRef objDataSetKanryouHantei As DataSet, _
                                          ByVal strFilePath As String, ByVal strFileNm As String) As Boolean
        Try
            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_S16", conDB)

            cmdDB.Parameters.Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
            cmdDB.Parameters.Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            'トランザクション
            cmdDB.Transaction = trans1

            '照会要求データ取得処理
            objDataSetKanryouHantei = clsDataConnect.pfGet_DataSet(cmdDB)
            Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

            If (Not objDataSetKanryouHantei Is Nothing) And objDataSetKanryouHantei.Tables.Count > 0 And strReturn <> "0" Then
                '該当データありの場合は既にDataSetに格納されているため何もしない。
            Else
                '取得データなし
                Return False
            End If
            '11111            End Using
            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetYoukyuOyaMeisai"
            strTable = "D129_REQFSE, D130_REQFSH"
            strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_S16"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 照会要求受付DB全取得
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="objDataSetKanryouHantei"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetD101InqreceptSqlAll(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                              ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByRef objDataSetKanryouHantei As DataSet, _
                                              ByVal strFilePath As String, ByVal strFileNm As String) As Boolean
        Try
            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_S15", conDB)

            cmdDB.Parameters.Add(pfSet_Param("prmD101_CTRL_NO", SqlDbType.NVarChar, strCtrlNo))
            cmdDB.Parameters.Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            'トランザクション
            cmdDB.Transaction = trans1

            '照会要求データ取得処理
            objDataSetKanryouHantei = clsDataConnect.pfGet_DataSet(cmdDB)
            Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

            If (Not objDataSetKanryouHantei Is Nothing) And objDataSetKanryouHantei.Tables.Count > 0 And strReturn <> "0" Then
                '該当データありの場合は既にDataSetに格納されているため何もしない。
            Else
                '取得データなし
                Return False
            End If
            '11111            End Using
            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetD101InqreceptSqlAll"
            strTable = "D101_INQRECEPT"
            strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_S15"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 集信（配信）対象データ種別取得
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strKbn"></param>
    ''' <param name="strTboxverCls"></param>
    ''' <param name="objData"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetTaishoDataSyubetu(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                            ByVal trans1 As SqlTransaction, ByVal strKbn As String, ByVal strTboxverCls As String, _
                                            ByRef objData As DataSet, ByVal strFilePath As String, ByVal strFileNm As String) As Boolean
        Try
            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_S14", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("@prmKbn", SqlDbType.NVarChar, strKbn))
                .Add(pfSet_Param("@prmTBOXVER_CLS", SqlDbType.NVarChar, strTboxverCls))
                .Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
            End With
            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            'トランザクション
            cmdDB.Transaction = trans1

            objData = clsDataConnect.pfGet_DataSet(cmdDB)
            Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

            If (Not objData Is Nothing) And objData.Tables.Count > 0 And strReturn <> "0" Then
                '該当データありの場合は既にDataSetに格納されているため何もしない。
            Else
                Throw New Exception("ＴＢＯＸ種別（Ｖｅｒ込）取得エラー")
            End If
            '11111            End Using

            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetTaishoDataSyubetu"
            strTable = "M94_TBOXVER_COLLINQU"
            strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_S14"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' ＴＢＯＸ種別（Ｖｅｒ込）取得
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strTboxId"></param>
    ''' <param name="objData"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mgGetTboxSyubetuVer(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                         ByVal trans1 As SqlTransaction, ByVal strTboxId As String, ByRef objData As DataSet, _
                                         ByVal strFilePath As String, ByVal strFileNm As String) As Boolean
        Try
            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_S13", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("@prmT03_TBOXID", SqlDbType.NVarChar, strTboxId))
                .Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
            End With
            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            'トランザクション
            cmdDB.Transaction = trans1

            objData = clsDataConnect.pfGet_DataSet(cmdDB)
            Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

            If (Not objData Is Nothing) And objData.Tables.Count > 0 And strReturn <> "0" Then
                '該当データありの場合は既にDataSetに格納されているため何もしない。
            Else
                Throw New Exception("ＴＢＯＸ種別（Ｖｅｒ込）取得エラー")
            End If
            '11111            End Using

            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetDataSyubetu"
            strTable = "T03_TBOX, M03_TBOX"
            strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_S13"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' データ種別取得
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strKbn"></param>
    ''' <param name="strD182CtrlNo"></param>
    ''' <param name="objData"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDataSyubetu(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                      ByVal trans1 As SqlTransaction, ByVal strKbn As String, ByVal strD182CtrlNo As String, ByRef objData As DataSet, _
                                      ByVal strFilePath As String, ByVal strFileNm As String) As Boolean
        Try
            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_S12", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("@prmKbn", SqlDbType.NVarChar, strKbn))
                .Add(pfSet_Param("@prmCTRL_NO", SqlDbType.NVarChar, strD182CtrlNo))
                .Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
            End With
            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            'トランザクション
            cmdDB.Transaction = trans1

            objData = clsDataConnect.pfGet_DataSet(cmdDB)
            Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

            If (Not objData Is Nothing) And objData.Tables.Count > 0 And strReturn <> "0" Then
                '該当データありの場合は既にDataSetに格納されているため何もしない。
            Else
                Throw New Exception("データ種別取得エラー")
            End If
            '11111            End Using

            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetDataSyubetu"
            If strKbn = "0" Then
                strTable = "ファイル名：" & strFileNm & " " & "D103_TM_HISN_KNR"
            Else
                strTable = "ファイル名：" & strFileNm & " " & "D104_TM_SHSN_KNR"
            End If
            strData = "SMTSKF001_S12"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 照会要求受付DB更新
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strD182CtrlNo"></param>
    ''' <param name="strD101Result"></param>
    ''' <param name="strD101ResultNo"></param>
    ''' <param name="strD101ReqSeq"></param>
    ''' <param name="strD101ReqDate"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfUpdateD101Inqrecept(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                           ByVal trans1 As SqlTransaction, ByVal strD182CtrlNo As String, ByVal strD101Result As String, _
                                           ByVal strD101ResultNo As String, ByVal strD101ReqSeq As String, _
                                           ByVal strD101ReqDate As String, ByVal strFilePath As String, _
                                           ByVal strFileNm As String) As Boolean

        Try
            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_U1", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmD101_CTRL_NO", SqlDbType.NVarChar, strD182CtrlNo))
                .Add(pfSet_Param("prmD101_RESULT", SqlDbType.Char, strD101Result))
                .Add(pfSet_Param("prmD101_RESULT_NO", SqlDbType.Char, strD101ResultNo))
                .Add(pfSet_Param("prmD101_REQSEQ", SqlDbType.Char, strD101ReqSeq))
                .Add(pfSet_Param("prmD101_REQDATE", SqlDbType.Char, strD101ReqDate))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                    trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D101_INQRECEPT"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_U1"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                    trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfUpdateD101Inqrecept"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 使用中カードＤＢ２（NVIC)
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="strNewCtrlNo"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD156Siyouchucard2(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                               ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                               ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                               ByVal strNewCtrlNo As String, ByVal dtCsvTable As DataTable, _
                                               ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I24", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmNEW_CTRL_NO", SqlDbType.NVarChar, strNewCtrlNo))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D156_SIYOUCHUCARD2"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I24"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD156Siyouchucard2"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' T-BOXエラーログ
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="strNewCtrlNo"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD144Tboxerrlog(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                            ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                            ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                            ByVal strNewCtrlNo As String, ByVal dtCsvTable As DataTable, _
                                            ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I23", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmNEW_CTRL_NO", SqlDbType.NVarChar, strNewCtrlNo))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D144_TBOXERRLOG"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I23"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD144Tboxerrlog"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' T-BOX操作ログ
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="strNewCtrlNo"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD143Tboxsousalog(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                              ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                              ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                              ByVal strNewCtrlNo As String, ByVal dtCsvTable As DataTable, _
                                              ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I22", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmNEW_CTRL_NO", SqlDbType.NVarChar, strNewCtrlNo))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D143_TBOXSOUSALOG"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I22"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD143Tboxsousalog"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' BB装置情報
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="strNewCtrlNo"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD136Bbsouti(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                         ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                         ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                         ByVal strNewCtrlNo As String, ByVal dtCsvTable As DataTable, _
                                         ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I21", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmNEW_CTRL_NO", SqlDbType.NVarChar, strNewCtrlNo))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D136_BBSOUTI"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I21"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD136Bbsouti"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 決済照会情報
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="strNewCtrlNo"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD132Kessaishoukai(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                               ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                               ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                               ByVal strNewCtrlNo As String, ByVal dtCsvTable As DataTable, _
                                               ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I20", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmNEW_CTRL_NO", SqlDbType.NVarChar, strNewCtrlNo))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D132_KESSAISHOUKAI"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I20"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD132Kessaishoukai"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 使用中カードＤＢ情報
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="strNewCtrlNo"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD131Siyouchucard(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                              ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                              ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                              ByVal strNewCtrlNo As String, ByVal dtCsvTable As DataTable, _
                                              ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I19", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmNEW_CTRL_NO", SqlDbType.NVarChar, strNewCtrlNo))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D131_SIYOUCHUCARD"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I19"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD131Siyouchucard"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 精算明細情報ＤＢ
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="strNewCtrlNo"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD150Seisanmeisai(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                              ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                              ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                              ByVal strNewCtrlNo As String, ByVal dtCsvTable As DataTable, _
                                              ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I18", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmNEW_CTRL_NO", SqlDbType.NVarChar, strNewCtrlNo))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D150_SEISANMEISAI"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I18"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD150Seisanmeisai"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 債務明細情報ＤＢ
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="strNewCtrlNo"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD148Saimumeisai(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                             ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                             ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                             ByVal strNewCtrlNo As String, ByVal dtCsvTable As DataTable, _
                                             ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I17", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmNEW_CTRL_NO", SqlDbType.NVarChar, strNewCtrlNo))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D148_SAIMUMEISAI"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I17"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD148Saimumeisai"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 債権明細情報ＤＢ
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="strNewCtrlNo"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD147Saikenmeisai(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                              ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                              ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                              ByVal strNewCtrlNo As String, ByVal dtCsvTable As DataTable, _
                                              ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I16", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmNEW_CTRL_NO", SqlDbType.NVarChar, strNewCtrlNo))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D147_SAIKENMEISAI"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I16"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD147Saikenmeisai"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 店内装置構成表
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="strNewCtrlNo"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD133Soutikosei(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                            ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                            ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                            ByVal strNewCtrlNo As String, ByVal dtCsvTable As DataTable, _
                                            ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I15", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmNEW_CTRL_NO", SqlDbType.NVarChar, strNewCtrlNo))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D133_SOUTIKOSEI"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I15"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD133Soutikosei"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' ＢＢ基盤情報２
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="strNewCtrlNo"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD162Bbkiban2(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                          ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                          ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                          ByVal strNewCtrlNo As String, ByVal dtCsvTable As DataTable, _
                                          ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I14", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmNEW_CTRL_NO", SqlDbType.NVarChar, strNewCtrlNo))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D162_BBKIBAN2"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I14"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD162Bbkiban2"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' ＢＢ基盤情報
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="strNewCtrlNo"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD135Bbkiban(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                         ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                         ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                         ByVal strNewCtrlNo As String, ByVal dtCsvTable As DataTable, _
                                         ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I13", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmNEW_CTRL_NO", SqlDbType.NVarChar, strNewCtrlNo))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D135_BBKIBAN"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I13"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD135Bbkiban"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 照会要求明細データ
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="strNewCtrlNo"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD130Reqfsh(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                        ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                        ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                        ByVal strNewCtrlNo As String, ByVal dtCsvTable As DataTable, _
                                        ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I12", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmNEW_CTRL_NO", SqlDbType.NVarChar, strNewCtrlNo))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D130_REQFSH"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I12"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD130Reqfsh"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 照会要求親データ
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="strNewCtrlNo"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD129Reqfse(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                        ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                        ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                        ByVal strNewCtrlNo As String, ByVal dtCsvTable As DataTable, _
                                        ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I11", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmNEW_CTRL_NO", SqlDbType.NVarChar, strNewCtrlNo))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D129_REQFSE"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I11"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD129Reqfse"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 照会要求受付DB取得
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="objSabun"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetD101InqreceptSql(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                           ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByRef objSabun As DataSet, _
                                           ByVal strFilePath As String, ByVal strFileNm As String) As Boolean
        Try
            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_S11", conDB)

            cmdDB.Parameters.Add(pfSet_Param("prmD101_CTRL_NO", SqlDbType.NVarChar, strCtrlNo))
            cmdDB.Parameters.Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            'トランザクション
            cmdDB.Transaction = trans1

            '照会要求データ取得処理
            objSabun = clsDataConnect.pfGet_DataSet(cmdDB)
            Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

            If (Not objSabun Is Nothing) And objSabun.Tables.Count > 0 And strReturn <> "0" Then
                '該当データありの場合は既にDataSetに格納されているため何もしない。
            Else
                '取得データなし
                Return False
            End If
            '11111            End Using
            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetD101InqreceptSql"
            strTable = "D101_INQRECEPT"
            strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_S11"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 配信情報登録DB（BB制御情報（精算機）（NVIC））
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD120TtHjtDbBbsjsisnknvc(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                                     ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                                     ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                                     ByVal dtCsvTable As DataTable, ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I10", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D120_TT_HJT_DB_BBSJSISNKNVC"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I10"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD120TtHjtDbBbsjsisnknvc"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 配信情報登録DB（BB制御情報（サンド）（NVIC））
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD122TtHjtDbBbsjsndnvc(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                                   ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                                   ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                                   ByVal dtCsvTable As DataTable, ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I9", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D122_TT_HJT_DB_BBSJSNDNVC"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I9"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD122TtHjtDbBbsjsndnvc"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 配信情報登録DB（BB運用情報（NVIC））
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD112TtHjtDbBbujnvc(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                                ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                                ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                                ByVal dtCsvTable As DataTable, ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I8", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D112_TT_HJT_DB_BBUJNVC"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I8"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD112TtHjtDbBbujnvc"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 配信情報登録DB（BB制御情報（登録受付機））
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD118TtHjtDbBbsjturkuktskk(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                                       ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                                       ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                                       ByVal dtCsvTable As DataTable, ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I7", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D118_TT_HJT_DB_BBSJTURKUKTSKK"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I7"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD118TtHjtDbBbsjturkuktskk"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 配信情報登録DB（BB制御情報（サンド）（精算機））
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD116TtHjtDbBbsjsndsisnk(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                                     ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                                     ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                                     ByVal dtCsvTable As DataTable, ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I6", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D116_TT_HJT_DB_BBSJSNDSISNK"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I6"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD116TtHjtDbBbsjsndsisnk"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 配信情報登録DB（BB制御情報（券売入金機））登録
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD114TtHjtDbBbsjknbinyknk(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                                      ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                                      ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                                      ByVal dtCsvTable As DataTable, ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I5", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D114_TT_HJT_DB_BBSJKNBINYKNK"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I5"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD114TtHjtDbBbsjknbinyknk"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 配信情報登録ＤＢ（T-BOX制御情報）登録
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD110TtHjtDbBbuj(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                             ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                             ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                             ByVal dtCsvTable As DataTable, ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I4", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D110_TT_HJT_DB_BBUJ"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I4"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD110TtHjtDbBbuj"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 配信情報登録ＤＢ（T-BOX制御情報）登録
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD108TtHjtDbTbxsj(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                              ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                              ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                              ByVal dtCsvTable As DataTable, ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I3", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D108_TT_HJT_DB_TBXSJ"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I3"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD108TtHjtDbTbxsj"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 集信管理ＤＢ登録
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD104TmShsnKnr(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                           ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                           ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                           ByVal dtCsvTable As DataTable, ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I2", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D104_TM_SHSN_KNR"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I2"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD104TmShsnKnr"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 配信管理ＤＢ登録
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strCtrlNo"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="strNlCls"></param>
    ''' <param name="strIdIcCls"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsertD103TmHisnKnr(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                           ByVal trans1 As SqlTransaction, ByVal strCtrlNo As String, ByVal strSeq As String, _
                                           ByVal strNlCls As String, ByVal strIdIcCls As String, _
                                           ByVal dtCsvTable As DataTable, ByVal strFilePath As String, ByVal strFileNm As String) As Boolean

        Try

            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_I1", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmCTRL_NO", SqlDbType.NVarChar, strCtrlNo))
                .Add(pfSet_Param("prmSEQ", SqlDbType.Int, Integer.Parse(strSeq)))
                .Add(pfSet_Param("prmNL_CLS", SqlDbType.NVarChar, strNlCls))
                .Add(pfSet_Param("prmIDIC_CLS", SqlDbType.NVarChar, strIdIcCls))
                .Add(pfSet_Param("prmDataTbl", SqlDbType.Structured, dtCsvTable))
                .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            End With

            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            cmdDB.Transaction = trans1
            '実行
            cmdDB.ExecuteNonQuery()
            Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
            If intReturn <> 0 Then
                'エラーメッセージ項目設定
                '11111                trans1.Rollback()
                strErrBunrui = "ＳＱＬ実行"
                strTable = "D103_TM_HISN_KNR"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_I1"
                Throw New Exception("ストアドプロシージャ：" & intReturn)
            Else
                'コミッテッド
                '11111                trans1.Commit()
            End If

            '11111            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfInsertD103TmHisnKnr"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' ＮＬ区分、システム分類区分取得
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strD83TboxId"></param>
    ''' <param name="objSabun"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetNlAndSystemBunrui(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                            ByVal trans1 As SqlTransaction, ByVal strD83TboxId As String, ByRef objSabun As DataSet, _
                                            ByVal strFilePath As String, ByVal strFileNm As String) As Boolean
        Try
            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_S10", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("@prmT03_TBOXID", SqlDbType.NVarChar, strD83TboxId))
                .Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
            End With
            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            'トランザクション
            cmdDB.Transaction = trans1

            objSabun = clsDataConnect.pfGet_DataSet(cmdDB)
            Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

            If (Not objSabun Is Nothing) And objSabun.Tables.Count > 0 And strReturn <> "0" Then
                '該当データありの場合は既にDataSetに格納されているため何もしない。
            Else
                Throw New Exception("NL区分、システム分類取得エラー")
            End If
            '11111            End Using

            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetNlAndSystemBunrui"
            strTable = "T03_TBOX"
            strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_S10"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' ＴＢＯＸＩＤ取得
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="strD182CtrlNo"></param>
    ''' <param name="objSabun"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetTboxId(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                 ByVal trans1 As SqlTransaction, ByVal strD182CtrlNo As String, ByRef objSabun As DataSet, _
                                 ByRef strFilePath As String, ByVal strFileNm As String) As Boolean
        Try
            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_S9", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("@prmD83_REQMNG_NO", SqlDbType.NVarChar, strD182CtrlNo))
                .Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
            End With
            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            'トランザクション
            cmdDB.Transaction = trans1

            objSabun = clsDataConnect.pfGet_DataSet(cmdDB)
            Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

            If (Not objSabun Is Nothing) And objSabun.Tables.Count > 0 And strReturn <> "0" Then
                '該当データありの場合は既にDataSetに格納されているため何もしない。
            Else
                Throw New Exception("TBOXID取得エラー")
            End If
            '11111            End Using

            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetTboxId"
            strTable = "D83_REFERENCE_DATA"
            strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_S9"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' ＣＳＶファイル取得（データセットに登録）
    ''' </summary>
    ''' <param name="strFiles"></param>
    ''' <param name="dtCsvTable"></param>
    ''' <param name="strRcvFilePath"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfSetCsvToDataTable(ByVal strFiles As String, ByRef dtCsvTable As DataTable, _
                                         ByVal strRcvFilePath As String, ByVal strFilePath As String, _
                                         ByVal strFileNm As String) As Boolean
        Try
            'CSVファイルを読み込む（エラー検索のため）
            Dim objFileCsv As List(Of String()) = pfReadCsvFile(strFiles)
            If objFileCsv(0)(0).IndexOf("ファイルエラー") >= 0 Then
                msDBErrLog(strFilePath, "その他エラー", "mfSetCsvToDataTable", "", objFileCsv(0)(0), "ファイルエラー")
                Return False
            ElseIf objFileCsv(0)(0).IndexOf("ＳＱＬ文エラー") >= 0 Then
                msDBErrLog(strFilePath, "その他エラー", "mfSetCsvToDataTable", "", objFileCsv(0)(0), "ＳＱＬ文エラー")
                Return False
            ElseIf objFileCsv(0)(0).IndexOf("ORA") >= 0 Then
                msDBErrLog(strFilePath, "その他エラー", "mfSetCsvToDataTable", "", objFileCsv(0)(0), "ＳＱＬ文エラー")
                Return False
            End If

            For i = 0 To objFileCsv(0).Length - 1
                dtCsvTable.Columns.Add((i).ToString("D"))
            Next

            For i = 0 To objFileCsv.Count - 1
                dtCsvTable.Rows.Add(objFileCsv(i))
            Next

            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfSetCsvToDataTable"
            strTable = ""
            strData = "ファイル名：" & strFileNm
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 保存場所取得処理（ログファイル）
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetHozon(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, ByRef strFilePath As String) As Boolean
        Try
            Using trans1 As SqlTransaction = conDB.BeginTransaction
                'プロシージャ設定
                cmdDB = New SqlCommand("SMTSKF001_S1", conDB)

                cmdDB.Parameters.Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                'ストアドプロシージャで実行
                cmdDB.CommandType = CommandType.StoredProcedure

                'トランザクション
                cmdDB.Transaction = trans1

                '保存先場所取得
                Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)
                Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

                If (Not objDs Is Nothing) And objDs.Tables.Count > 0 And strReturn <> "0" Then
                    Dim dtTable As DataTable
                    Dim objDataRow As DataRow
                    dtTable = objDs.Tables(0)
                    objDataRow = dtTable.Rows(0)
                    '保存先場所
                    strFilePath = "\\" & objDataRow(0) & "\" & objDataRow(1)
                Else
                    Throw New Exception("保存場所取得エラー")
                End If
            End Using

            Return True
        Catch ex As Exception
            strFilePath = strERRLOGPATH
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetHozon"
            strTable = "M78_PRESERVE_PLACE"
            strData = "SMTSKF001_S1"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 保存場所取得処理（受信ファイル一時作成場所）
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="strRcvFilePath"></param>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetHozonRcv(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                   ByRef strRcvFilePath As String, ByVal strFilePath As String) As Boolean
        Try
            Using trans1 As SqlTransaction = conDB.BeginTransaction
                'プロシージャ設定
                cmdDB = New SqlCommand("SMTSKF001_S4", conDB)

                cmdDB.Parameters.Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                'ストアドプロシージャで実行
                cmdDB.CommandType = CommandType.StoredProcedure

                'トランザクション
                cmdDB.Transaction = trans1

                '保存先場所取得
                Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)
                Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

                If (Not objDs Is Nothing) And objDs.Tables.Count > 0 And strReturn <> "0" Then
                    Dim dtTable As DataTable
                    Dim objDataRow As DataRow
                    dtTable = objDs.Tables(0)
                    objDataRow = dtTable.Rows(0)
                    '保存先場所
                    strRcvFilePath = "\\" & objDataRow(0) & "\" & objDataRow(1)
                Else
                    Throw New Exception("受信ファイル一時作成場所取得エラー")
                End If
            End Using

            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetHozonRcv"
            strTable = "M78_PRESERVE_PLACE"
            strData = "SMTSKF001_S4"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 保存場所取得処理（受信ファイル一時作成場所）
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="strBackUpFilePath"></param>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetHozonBackUp(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                      ByRef strBackUpFilePath As String, ByVal strFilePath As String) As Boolean
        Try
            Using trans1 As SqlTransaction = conDB.BeginTransaction
                'プロシージャ設定
                cmdDB = New SqlCommand("SMTSKF001_S5", conDB)

                cmdDB.Parameters.Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                'ストアドプロシージャで実行
                cmdDB.CommandType = CommandType.StoredProcedure

                'トランザクション
                cmdDB.Transaction = trans1

                '保存先場所取得
                Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)
                Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

                If (Not objDs Is Nothing) And objDs.Tables.Count > 0 And strReturn <> "0" Then
                    Dim dtTable As DataTable
                    Dim objDataRow As DataRow
                    dtTable = objDs.Tables(0)
                    objDataRow = dtTable.Rows(0)
                    '保存先場所
                    strBackUpFilePath = "\\" & objDataRow(0) & "\" & objDataRow(1)
                Else
                    Throw New Exception("結果ファイルバックアップ場所取得エラー")
                End If
            End Using

            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetHozonBackUp"
            strTable = "M78_PRESERVE_PLACE"
            strData = "SMTSKF001_S5"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 照会要求データ取得処理
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="objDataSet"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strErrKbn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetReferenceData(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, ByRef objDataSet As DataSet, ByVal strFilePath As String, ByRef strErrKbn As String) As Boolean
        Try
            'エラー区分を設定（戻った際にCatchされたか、データ無しかの判定に使用）
            strErrKbn = "0"

            Using trans1 As SqlTransaction = conDB.BeginTransaction
                'プロシージャ設定
                cmdDB = New SqlCommand("SMTSKF001_S2", conDB)

                cmdDB.Parameters.Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                'ストアドプロシージャで実行
                cmdDB.CommandType = CommandType.StoredProcedure

                'トランザクション
                cmdDB.Transaction = trans1

                '照会要求データ取得処理
                objDataSet = clsDataConnect.pfGet_DataSet(cmdDB)
                Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

                If (Not objDataSet Is Nothing) And objDataSet.Tables.Count > 0 And strReturn <> "0" Then
                    '該当データありの場合は既にDataSetに格納されているため何もしない。
                Else
                    '取得データなし
                    Return False
                End If
            End Using
            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetReferenceData"
            strTable = "D83_PRESERVE_PLACE"
            strData = "SMTSKF001_S2"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            strErrKbn = "1"
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 接続先情報取得処理
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="objDtSetConnect"></param>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetAccessPoint(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                      ByRef objDtSetConnect As DataSet, ByVal strFilePath As String) As Boolean
        'エラー区分を設定（戻った際にCatchされたか、データ無しかの判定に使用）
        Try
            Using trans1 As SqlTransaction = conDB.BeginTransaction
                'プロシージャ設定
                cmdDB = New SqlCommand("SMTSKF001_S3", conDB)

                With cmdDB.Parameters
                    .Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                End With
                'ストアドプロシージャで実行
                cmdDB.CommandType = CommandType.StoredProcedure

                'トランザクション
                cmdDB.Transaction = trans1

                '照会要求データ取得処理
                objDtSetConnect = clsDataConnect.pfGet_DataSet(cmdDB)
                Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

                If (Not objDtSetConnect Is Nothing) And objDtSetConnect.Tables.Count > 0 And strReturn <> "0" Then
                    '該当データありの場合は既にDataSetに格納されているため何もしない。
                Else
                    '取得データなし
                    strErrBunrui = "その他処理エラー"
                    strMesod = "mfGetAccessPoint"
                    strTable = "M81_ACCESS_POINT"
                    strData = "SMTSKF001_S3"
                    msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, "該当レコードなし")
                    Return False
                End If
            End Using
            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetAccessPoint"
            strTable = "M81_ACCESS_POINT"
            strData = "SMTSKF001_S3"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' ＦＴＰ受信ファイル名取得
    ''' </summary>
    ''' <param name="strUrl"></param>
    ''' <param name="strUserId"></param>
    ''' <param name="strPassWord"></param>
    ''' <param name="strGetNameListFinal"></param>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetFtpFileName(ByVal strUrl As String, ByVal strUserId As String, _
                                      ByVal strPassWord As String, ByRef strGetNameListFinal As String(), _
                                      ByVal strFilePath As String) As Boolean
        Try
            Dim ftpReq As System.Net.FtpWebRequest = _
                         CType(System.Net.WebRequest.Create(strUrl), System.Net.FtpWebRequest)
            'ログインユーザー名とパスワードを設定
            ftpReq.Credentials = New System.Net.NetworkCredential(strUserId, strPassWord)
            'MethodにWebRequestMethods.Ftp.ListDirectoryDetails("LIST")を設定
            '                ftpReq.Method = System.Net.WebRequestMethods.Ftp.ListDirectoryDetails
            ftpReq.Method = System.Net.WebRequestMethods.Ftp.ListDirectory
            '要求の完了後に接続を閉じる
            ftpReq.KeepAlive = False
            'PASSIVEモードを無効にする
            ftpReq.UsePassive = False

            'FtpWebResponseを取得
            Dim ftpRes As System.Net.FtpWebResponse = _
             CType(ftpReq.GetResponse(), System.Net.FtpWebResponse)
            'FTPサーバーから送信されたデータを取得
            Dim ftpStr As New System.IO.StreamReader(ftpRes.GetResponseStream())
            Dim strRes As String = ftpStr.ReadToEnd()

            Dim strGetName = strRes.ToString.Replace(Environment.NewLine, ",")
            '前後の","を削除する
            strGetName = strGetName.Trim(","c)

            Dim strGetNameList() As String
            strGetNameList = strGetName.ToString.Split(",")
            Dim j As Integer = 0
            ReDim strGetNameListFinal(j)
            For i = 0 To strGetNameList.Length - 1
                '.finishファイルの一覧を作成
                If strGetNameList(i).IndexOf(strSQLFINAL) > 0 Then
                    strGetNameListFinal(j) = String.Copy(strGetNameList(i))
                    j = j + 1
                    ReDim Preserve strGetNameListFinal(j)
                End If
            Next
            '最後に増やした配列を削除
            ReDim Preserve strGetNameListFinal(j - 1)

            ftpStr.Close()

            'FTPサーバーから送信されたステータスを表示
            ''Console.WriteLine("{0}: {1}", ftpRes.StatusCode, ftpRes.StatusDescription)
            '閉じる
            ftpRes.Close()

            Return True
        Catch ex As WebException
            strErrBunrui = "ＦＴＰ接続エラー"
            strMesod = "mfGetFtpFileName"
            strTable = ""
            strData = ""
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetFtpFileName"
            strTable = ""
            strData = ""
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' ＦＴＰファイル受信
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="strUrl"></param>
    ''' <param name="strUserId"></param>
    ''' <param name="strPassWord"></param>
    ''' <param name="strGetNameListFinal"></param>
    ''' <param name="intGetCount"></param>
    ''' <param name="strRcvFilePath"></param>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfFtpGetCsvFile(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                     ByVal strUrl As String, ByVal strUserId As String, _
                                     ByVal strPassWord As String, ByVal strGetNameListFinal() As String,
                                     ByRef intGetCount As String, ByVal strRcvFilePath As String, _
                                     ByVal strFilePath As String) As Boolean
        Try

            Dim ftpReq As System.Net.FtpWebRequest
            Dim ftpRes As System.Net.FtpWebResponse
            Dim objDtSetSyoriCd As DataSet = Nothing
            For i = 0 To strGetNameListFinal.Length - 1
                For j = 0 To 1
                    'FtpWebRequestの作成
                    If j = 0 Then
                        ftpReq = CType(System.Net.WebRequest.Create(strUrl & strGetNameListFinal(i)), System.Net.FtpWebRequest)
                    Else
                        ftpReq = CType(System.Net.WebRequest.Create(strUrl & _
                                                                    strGetNameListFinal(i).Replace(strSQLFINAL, strSQLKEKAFILENAMEKAKUCHOSHI)), System.Net.FtpWebRequest)
                    End If
                    'ログインユーザー名とパスワードを設定
                    ftpReq.Credentials = New System.Net.NetworkCredential(strUserId, strPassWord)
                    'MethodにWebRequestMethods.Ftp.DownloadFile("RETR")を設定
                    ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile
                    '要求の完了後に接続を閉じる
                    ftpReq.KeepAlive = False
                    'ASCIIモードで転送する
                    ftpReq.UseBinary = False
                    'PASSIVEモードを無効にする
                    ftpReq.UsePassive = False

                    'FtpWebResponseを取得
                    ftpRes = CType(ftpReq.GetResponse(), System.Net.FtpWebResponse)
                    'ファイルをダウンロードするためのStreamを取得
                    Dim strFileName As String
                    Dim strGetSyoriCdFileName As String
                    If j = 0 Then
                        strFileName = strRcvFilePath & "\" & strGetNameListFinal(i)
                        strGetSyoriCdFileName = strGetNameListFinal(i)
                    Else
                        strFileName = strRcvFilePath & "\" & strGetNameListFinal(i).Replace(strSQLFINAL, strSQLKEKAFILENAMEKAKUCHOSHI)
                        strGetSyoriCdFileName = strGetNameListFinal(i).Replace(strSQLFINAL, strSQLKEKAFILENAMEKAKUCHOSHI)
                    End If
                    Dim resStrm As System.IO.Stream = ftpRes.GetResponseStream()
                    'ダウンロードしたファイルを書き込むためのFileStreamを作成
                    Dim fs As New System.IO.FileStream( _
                        strFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write)
                    'ダウンロードしたデータを書き込む
                    Dim buffer(1023) As Byte
                    While True
                        Dim readSize As Integer = resStrm.Read(buffer, 0, buffer.Length)
                        If readSize = 0 Then
                            Exit While
                        End If
                        fs.Write(buffer, 0, readSize)
                    End While
                    fs.Close()
                    resStrm.Close()

                    'FTPサーバーから送信されたステータスを表示
                    'Console.WriteLine("{0}: {1}", ftpRes.StatusCode, ftpRes.StatusDescription)
                    '閉じる
                    ftpRes.Close()

                    '処理コード取得
                    If mfGetSyoriCd(cmdDB, conDB, objDtSetSyoriCd, j, strGetSyoriCdFileName, strFilePath) = False Then
                        Return False
                    End If
                    Dim dtTableSyoriCd As DataTable
                    Dim objDataRowSyoriCd As DataRow
                    dtTableSyoriCd = objDtSetSyoriCd.Tables(0)
                    objDataRowSyoriCd = dtTableSyoriCd.Rows(0)

                    'FTP受信ログ出力
                    msSeijyouLog(objDataRowSyoriCd(0), strGetSyoriCdFileName, strFilePath)
                Next


                intGetCount = intGetCount + 1
            Next
            Return True
        Catch ex As WebException
            strErrBunrui = "ＦＴＰ接続エラー"
            strMesod = "mfFtpGetCsvFile"
            strTable = "strFileName"
            strData = ""
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfFtpGetCsvFile"
            strTable = "strFileName"
            strData = ""
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 処理コード取得
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="objDtSetSyoriCd"></param>
    ''' <param name="intKbn"></param>
    ''' <param name="strFileName"></param>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetSyoriCd(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                  ByRef objDtSetSyoriCd As DataSet, _
                                  ByVal intKbn As Integer, ByVal strFileName As String, _
                                  ByVal strFilePath As String) As Boolean
        Try
            Using trans1 As SqlTransaction = conDB.BeginTransaction
                'プロシージャ設定
                cmdDB = New SqlCommand("SMTSKF001_S6", conDB)

                With cmdDB.Parameters
                    .Add(pfSet_Param("@pramKbn", SqlDbType.Int, intKbn))
                    .Add(pfSet_Param("@prmD182_FILENAME", SqlDbType.NVarChar, strFileName))
                    .Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                End With
                'ストアドプロシージャで実行
                cmdDB.CommandType = CommandType.StoredProcedure

                'トランザクション
                cmdDB.Transaction = trans1

                '照会要求データ取得処理
                objDtSetSyoriCd = clsDataConnect.pfGet_DataSet(cmdDB)
                Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

                If (Not objDtSetSyoriCd Is Nothing) And objDtSetSyoriCd.Tables.Count > 0 And strReturn <> "0" Then
                    '該当データありの場合は既にDataSetに格納されているため何もしない。
                Else
                    '取得データなし
                    strErrBunrui = "その他処理エラー"
                    strMesod = "mfGetSyoriCd"
                    strTable = "D182_EXTRACT_ISSUENUM"
                    strData = "SMTSKF001_S6"
                    msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, "該当レコードなし")
                    Return False
                End If
            End Using
            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetSyoriCd"
            strTable = "D182_EXTRACT_ISSUENUM"
            strData = "SMTSKF001_S6"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 情報抽出／要求照会ファイル発番抽出
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="objDatasetD182"></param>
    ''' <param name="strFiles"></param>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetD182ExtractIssuenum(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                              ByVal trans1 As SqlTransaction, ByRef objDatasetD182 As DataSet, ByVal strFiles As String, _
                                              ByVal strFilePath As String) As Boolean
        Try
            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_S7", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmD182_RSLT_FILE_NM", SqlDbType.NVarChar, strFiles))
                .Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
            End With
            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            'トランザクション
            cmdDB.Transaction = trans1

            '照会要求データ取得処理
            objDatasetD182 = clsDataConnect.pfGet_DataSet(cmdDB)
            Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

            If (Not objDatasetD182 Is Nothing) And objDatasetD182.Tables.Count > 0 And strReturn <> "0" Then
                '該当データありの場合は既にDataSetに格納されているため何もしない。
            Else
                '取得データなし
                strErrBunrui = "その他処理エラー"
                strMesod = "mfGetD182ExtractIssuenum"
                strTable = "D182_EXTRACT_ISSUENUM"
                strData = "SMTSKF001_S7"
                msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, "該当レコードなし")
                Return False
            End If
            '11111            End Using
            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetD182ExtractIssuenum"
            strTable = "D182_EXTRACT_ISSUENUM"
            strData = "SMTSKF001_S7"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 照会要求データ処理コード取得
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="trans1"></param>
    ''' <param name="objDatasetD83"></param>
    ''' <param name="strD182CtrlNo"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetD83ReferenceData(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                           ByVal trans1 As SqlTransaction, ByRef objDatasetD83 As DataSet, ByVal strD182CtrlNo As String, _
                                           ByVal strFilePath As String, ByVal strFileNm As String) As Boolean
        Try
            '11111            Using trans1 As SqlTransaction = conDB.BeginTransaction
            'プロシージャ設定
            cmdDB = New SqlCommand("SMTSKF001_S8", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prmD83_REQMNG_NO", SqlDbType.NVarChar, strD182CtrlNo))
                .Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
            End With
            'ストアドプロシージャで実行
            cmdDB.CommandType = CommandType.StoredProcedure

            'トランザクション
            cmdDB.Transaction = trans1

            '照会要求データ取得処理
            objDatasetD83 = clsDataConnect.pfGet_DataSet(cmdDB)
            Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

            If (Not objDatasetD83 Is Nothing) And objDatasetD83.Tables.Count > 0 And strReturn <> "0" Then
                '該当データありの場合は既にDataSetに格納されているため何もしない。
            Else
                '取得データなし
                strErrBunrui = "その他処理エラー"
                strMesod = "mfGetD83ReferenceData"
                strTable = "D83_REFERENCE_DATA"
                strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_S8"
                msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, "該当レコードなし")
                Return False
            End If
            '11111            End Using
            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetD83ReferenceData"
            strTable = "D83_REFERENCE_DATA"
            strData = "ファイル名：" & strFileNm & " " & "SMTSKF001_S8"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try


    End Function

    ''' <summary>
    ''' エラーログ出力
    ''' </summary>
    ''' <param name="strPath"></param>
    ''' <param name="strErrBunrui"></param>
    ''' <param name="strMesod"></param>
    ''' <param name="strTable"></param>
    ''' <param name="strData"></param>
    ''' <param name="strNaiyo"></param>
    ''' <remarks></remarks>
    Private Sub msDBErrLog(ByVal strPath As String, ByVal strErrBunrui As String, ByVal strMesod As String, _
                           ByVal strTable As String, ByVal strData As String, Optional ByVal strNaiyo As String = Nothing)
        Dim writer As System.IO.StreamWriter
        Dim dteSysDate As DateTime = DateTime.Now()
        Dim strErrLog As String = ""

        'Shift-JISのテキストファイルを作成します。
        '第２パラメータは既存ファイルが存在する場合の振る舞いを示します。
        'false：上書き、true：追記
        strErrLog = dteSysDate.ToString("yyyy/MM/dd HH:mm:ss") & " " & _
             " " & strErrBunrui & " " & strMesod & " " & strTable & " " & strData & " " & strNaiyo

        writer = New System.IO.StreamWriter(strPath & "\" & strLOGFILE & dteSysDate.ToString("yyyyMMdd") & strERR & strEXT_LOG, _
                                            True, System.Text.Encoding.Default)
        writer.WriteLine(strErrLog)        'エラーログ出力

        writer.Close()

    End Sub

    ''' <summary>
    ''' ログ出力
    ''' </summary>
    ''' <param name="strSyoriCd"></param>
    ''' <param name="strSoushinFileName"></param>
    ''' <param name="strFilePath"></param>
    ''' <remarks></remarks>
    Private Sub msSeijyouLog(ByVal strSyoriCd As String, ByVal strSoushinFileName As String, ByVal strFilePath As String)
        Dim writer As System.IO.StreamWriter
        Dim dteSysDate As DateTime = DateTime.Now()
        Dim strSyoriCode As String = ""
        Dim strLog As String = ""

        Select Case strSyoriCd
            Case "901"
                strSyoriCode = "901：即時集信（集信結果）"
            Case "902"
                strSyoriCode = "902：即時集信（画面検索）"
            Case "401"
                strSyoriCode = "401：構成配信（配信依頼結果）"
            Case "402"
                strSyoriCode = "402：構成配信（反映指示結果）"
            Case "200"
                strSyoriCode = "200：貸玉数設定情報"
            Case "301"
                strSyoriCode = "301：ＩＣカード履歴"
            Case "302"
                strSyoriCode = "301：ＩＣカード履歴（使用中カードログ２）"
            Case "100"
                strSyoriCode = "100：ＴＢＯＸ随時照会"
            Case "501"
                strSyoriCode = "501：設定データ更新（ＴＢＯＸ制御情報）"
            Case "502"
                strSyoriCode = "502：設定データ更新（ＢＢ運用情報）"
            Case "503"
                strSyoriCode = "503：設定データ更新（ＢＢ制御情報（券売入金機））"
            Case "504"
                strSyoriCode = "504：設定データ更新（ＢＢ制御情報（サンド）（精算機））"
            Case "505"
                strSyoriCode = "505：設定データ更新（ＢＢ制御情報（登録受付機））"
            Case "506"
                strSyoriCode = "506：設定データ更新（ＢＢ制御情報（NVIC））"
            Case "507"
                strSyoriCode = "507：設定データ更新（ＢＢ制御情報（サンド）（NVIC））"
            Case "508"
                strSyoriCode = "508：設定データ更新（ＢＢ制御情報（精算機）（NVIC））"
        End Select
        'Shift-JISのテキストファイルを作成します。
        '第２パラメータは既存ファイルが存在する場合の振る舞いを示します。
        'false：上書き、true：追記
        strLog = dteSysDate.ToString("yyyy/MM/dd HH:mm:ss") & " " & _
             " " & strSyoriCode & " " & strSoushinFileName

        writer = New System.IO.StreamWriter(strFilePath & "\" & strLOGFILE & dteSysDate.ToString("yyyyMMdd") & strEXT_LOG, _
                                            True, System.Text.Encoding.Default)
        writer.WriteLine(strLog)        'ログ出力

        writer.Close()

    End Sub

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
