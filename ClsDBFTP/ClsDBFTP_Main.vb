'********************************************************************************************************************************
'*　システム　：　サポートセンタステム
'*　処理名　　：　アップロード・ダウンロード制御　メイン
'*　ＰＧＭＩＤ：　ClsLogwrite
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.06.17　：　伯野
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Xml
Imports System.Xml.Serialization
Imports System.IO
Imports System.Net
Imports System.Object
Imports System.Text
Imports DBFTP.ClsDBFTP_Main
Imports DBFTP.ClsFTPCnfg
'Imports DBFTP.clsLogwrite
Imports DBFTP.ClsSQLSvrDB


Public Class ClsDBFTP_Main

    '================================================================================================================================
    '=　定数定義
    '================================================================================================================================

    '================================================================================================================================
    '=　変数定義
    '================================================================================================================================
    Dim clsFTP_Svr As New ClsFTPCnfg
    '--------------------------------
    '2014/04/14 星野　ここから
    '--------------------------------
    Dim objStack As StackFrame
    '--------------------------------
    '2014/04/14 星野　ここまで
    '--------------------------------
    '================================================================================================================================
    '=　プロパティ定義
    '================================================================================================================================

    '================================================================================================================================
    '=　イベントプロシージャ定義
    '================================================================================================================================
    '----------------------------------------------------------------------------------------------------------------------
    '-　インスタンス作成時
    '----------------------------------------------------------------------------------------------------------------------
    Public Sub New()

        Dim reader As XmlSerializer = New XmlSerializer(GetType(ClsFTPCnfg), New XmlRootAttribute("FTP_CONNECT"))
        Dim file As StreamReader = New StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + "\CNFG\FTP_Cnfg.xml")
        clsFTP_Svr = CType(reader.Deserialize(file), ClsFTPCnfg)

    End Sub

    '================================================================================================================================
    '=　プロシージャ定義
    '================================================================================================================================

    '----------------------------------------------------------------------------------------------------------------------
    '-　ディレクトリ存在チェック
    '----------------------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' ディレクトリ存在チェック
    ''' </summary>
    ''' <param name="ipstrDirName">検索対象フォルダ</param>
    ''' <param name="opblnResult">検索結果　True:有／False:無</param>
    ''' <returns>関数の成功失敗　True:成功／False:失敗</returns>
    ''' <remarks></remarks>
    Public Function pfFtpDir_Exists(ByVal ipstrDirName As String, ByRef opblnResult As Boolean, Optional ByVal opCls As String = "") As Boolean

        Dim zz As Integer = 0
        Dim arylist As New ArrayList
        Dim splDir As String() = Nothing
        Dim strDirName As String = Nothing
psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "変数定義", "Catch")
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        pfFtpDir_Exists = False
        opblnResult = False

        Try
psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "opCls：" & opCls, "Catch")
            Select Case opCls
                Case "1"
                    splDir = ipstrDirName.Split("\")
                    strDirName = splDir(0)
psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "フォルダ確認：" & ipstrDirName, "Catch")
                    For spl As Integer = 0 To splDir.Count - 3
                        'フォルダを1つずつ確認するためのパスを作成
                        strDirName &= "\" & splDir(spl + 1)
                        'GetListの実行
                        If fFtpDirExists(strDirName) = True Then
                            opblnResult = True
                            pfFtpDir_Exists = True
                        Else
                            opblnResult = False
                            pfFtpDir_Exists = False
                            Exit For
                        End If
                    Next
                Case Else
psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "フォルダ確認：" & ipstrDirName, "Catch")
                    'If fFtpDirExists(strDirName) = True Then
                    If fFtpDirExists(ipstrDirName) = True Then
                        opblnResult = True
                        pfFtpDir_Exists = True
                    End If
            End Select
            Return opblnResult

        Catch ex As Exception
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
        End Try

    End Function

    '----------------------------------------------------------------------------------------------------------------------
    '-　ファイル存在チェック
    '----------------------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' ファイル存在チェック
    ''' </summary>
    ''' <param name="ipstrDirName">検索対象フォルダ</param>
    ''' <param name="ipstrFileName">検索対象ファイル名</param>
    ''' <param name="opblnResult">検索結果　True:有／False:無</param>
    ''' <returns>関数の成功失敗　True:成功／False:失敗</returns>
    ''' <remarks></remarks>
    Public Function pfFtpFile_Exists(ByVal ipstrDirName As String, ByVal ipstrFileName As String, ByRef opblnResult As Boolean) As Boolean

        Dim zz As Integer = 0
        Dim arylist As New ArrayList
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        pfFtpFile_Exists = False
        opblnResult = False

        Try
psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "ipstrDirName：" & ipstrDirName, "Catch")

            'GetListの実行
            If fFtpGetList(ipstrDirName, arylist) = True Then
                For zz = 0 To arylist.Count - 1
                    If arylist(zz).ToString = ipstrFileName Then
                        opblnResult = True
                        pfFtpFile_Exists = True
                        Exit For
                    End If
                Next
            End If

        Catch ex As Exception
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
        End Try

    End Function

    '----------------------------------------------------------------------------------------------------------------------
    '-　アップロード
    '----------------------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' アップ／ダウンロード
    ''' </summary>
    ''' <param name="ipstrProc">処理区分：PUT/GET</param>
    ''' <param name="ipstrDirName">アップロード対象フォルダ</param>
    ''' <param name="ipstrFileName">アップロード対象ファイル名</param>
    ''' <param name="opblnResult">処理結果　True:有／False:無</param>
    ''' <returns>関数の成功失敗　True:成功／False:失敗</returns>
    ''' <remarks></remarks>
    Public Function pfFtpFile_Copy(ByVal ipstrProc As String, ByVal ipstrDirName As String, ByVal ipstrFileName As String, ByRef opblnResult As Boolean, ByVal strLocalPath As String) As Boolean
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        pfFtpFile_Copy = False

        Try

            Select Case ipstrProc
                Case "GET"
                    'PutFileの実行
                    If fFtpGetFile(ipstrDirName, ipstrFileName, strLocalPath) = True Then
                        pfFtpFile_Copy = True
                    End If
                Case "PUT"
                    'PutFileの実行
                    If fFtpPutFile(ipstrDirName, ipstrFileName, strLocalPath) = True Then
                        pfFtpFile_Copy = True
                    End If
            End Select

        Catch ex As Exception
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
        End Try

    End Function

    ''' <summary>
    ''' アップ／ダウンロード
    ''' </summary>
    ''' <param name="ipstrProc">処理区分：PUT/GET</param>
    ''' <param name="ipstrDirName">アップロード対象フォルダ</param>
    ''' <param name="ipstrMakeDirName">アップロード対象作成フォルダ名</param>
    ''' <param name="ipstrFileName">アップロード対象ファイル名</param>
    ''' <param name="opblnResult">処理結果　True:有／False:無</param>
    ''' <returns>関数の成功失敗　True:成功／False:失敗</returns>
    ''' <remarks></remarks>
    Public Function pfFtpFile_Copy(ByVal ipstrProc As String, ByVal ipstrDirName As String, ByVal ipstrMakeDirName As String, ByVal ipstrFileName As String, ByRef opblnResult As Boolean, ByVal strLocalPath As String) As Boolean
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        pfFtpFile_Copy = False

        Try

            Select Case ipstrProc
                Case "GET"
                    pfFtpFile_Copy = False
                Case "PUT"
                    'PutFileの実行
                    If fFtpPutFile(ipstrDirName, ipstrMakeDirName, ipstrFileName, strLocalPath) = True Then
                        pfFtpFile_Copy = True
                    End If
            End Select

        Catch ex As Exception
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
        End Try

    End Function

    '----------------------------------------------------------------------------------------------------------------------
    '-　ファイル移動
    '----------------------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' ファイル移動
    ''' </summary>
    ''' <param name="ipstrSurDirName">移動元対象フォルダ</param>
    ''' <param name="ipstrDstDirName">移動先対象フォルダ</param>
    ''' <param name="ipstrFileName">アップロード対象ファイル名</param>
    ''' <param name="opblnResult">処理結果　True:有／False:無</param>
    ''' <returns>関数の成功失敗　True:成功／False:失敗</returns>
    ''' <remarks></remarks>
    Public Function pfFtpFile_Move(ByVal ipstrSurDirName As String, ByVal ipstrDstDirName As String, ByVal ipstrFileName As String, ByRef opblnResult As Boolean, ByVal strLocalPath As String) As Boolean
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        pfFtpFile_Move = False

        Try

            'GetFileの実行
            If fFtpGetFile(ipstrSurDirName, ipstrFileName, strLocalPath) = True Then
                'PutFileの実行
                If fFtpPutFile(ipstrDstDirName, ipstrFileName, strLocalPath) = True Then
                    'DelFileの実行
                    If fFtpDelFile(ipstrSurDirName, ipstrFileName) Then
                        pfFtpFile_Move = True
                    End If
                End If

            End If

            pfFtpFile_Move = True

        Catch ex As Exception
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
        End Try

    End Function

    '----------------------------------------------------------------------------------------------------------------------
    '-　デリート
    '----------------------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' デリート
    ''' </summary>
    ''' <param name="ipstrDirName">デリート対象フォルダ</param>
    ''' <param name="ipstrFileName">デリート対象ファイル名</param>
    ''' <param name="opblnResult">処理結果　True:有／False:無</param>
    ''' <returns>関数の成功失敗　True:成功／False:失敗</returns>
    ''' <remarks></remarks>
    Public Function pfFtpFile_Delete(ByVal ipstrDirName As String, ByVal ipstrFileName As String, ByRef opblnResult As Boolean) As Boolean

        pfFtpFile_Delete = False
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try

            'PutFileの実行
            If fFtpDelFile(ipstrDirName, ipstrFileName) = True Then
                pfFtpFile_Delete = True
            End If

        Catch ex As Exception
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
        End Try

    End Function

    '----------------------------------------------------------------------------------------------------------------------
    '-　ＦＴＰ　ＰＵＴ実行
    '----------------------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' ＦＴＰ送信
    ''' </summary>
    ''' <param name="ipstrPutPath">ＰＵＴするパス</param>
    ''' <param name="ipstrPutFile">ＰＵＴするファイル名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function fFtpPutFile(ByVal ipstrPutPath As String, ByVal ipstrPutFile As String, ByVal strLocalPath As String) As Boolean

        'Dim webClient As New System.Net.WebClient()
        Dim ftpCredential As NetworkCredential = New NetworkCredential(clsFTP_Svr.UID, clsFTP_Svr.PWD)
        Dim strFileName As String = String.Empty
        Dim strUrl As String = "ftp://" & clsFTP_Svr.FTP_SERVER & "/" & ipstrPutPath & "/"     'IPアドレス
        Dim fwqFTPObj As FtpWebRequest
        Dim fwrFTPObj As FtpWebResponse = Nothing
        Dim aryDum As String()
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try

            aryDum = ipstrPutFile.Split("\")
            strFileName = aryDum(aryDum.Length - 1)

            'FtpWebRequestの作成
            fwqFTPObj = System.Net.WebRequest.Create(strUrl + ipstrPutFile)

            'ログインユーザー名とパスワードを設定
            fwqFTPObj.Credentials = ftpCredential
            'MethodにWebRequestMethods.Ftp.UploadFile("STOR")を設定
            fwqFTPObj.Method = System.Net.WebRequestMethods.Ftp.UploadFile
            '要求の接続終了でログアウトする
            fwqFTPObj.KeepAlive = False
            'Binaryモードで転送する
            fwqFTPObj.UseBinary = True
            'PASSIVEモードを無効にする
            fwqFTPObj.UsePassive = False

            'ファイルをアップロードするためのStreamを取得
            Dim objFTPreqStrm As System.IO.Stream = fwqFTPObj.GetRequestStream
            'アップロードするファイルを開く
            Dim fsmUpFile As FileStream = New System.IO.FileStream(strLocalPath, System.IO.FileMode.Open, System.IO.FileAccess.Read)
            'アップロードStreamに書き込む
            Dim buffer(fsmUpFile.Length) As Byte
            While True
                Dim readSize As Integer = fsmUpFile.Read(buffer, 0, buffer.Length)
                If readSize = 0 Then
                    Exit While
                End If
                objFTPreqStrm.Write(buffer, 0, readSize)
            End While
            fsmUpFile.Close()
            objFTPreqStrm.Close()

            'FtpWebResponseを取得
            fwrFTPObj = fwqFTPObj.GetResponse()

            'FTP送信ログ出力
            '                msSeijyouLog(strProcCd, strFileName, strSql, strFilePath)

            '閉じる
            fwrFTPObj.Close()

            Return True

        Catch ex As WebException
            '            strErrBunrui = "ＦＴＰ接続エラー"
            '            strMesod = "mfFtpPutSqlFile"
            '            strTable = ""
            '            strData = ""
            '            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Return False
        Catch ex As Exception
            '            strErrBunrui = "その他処理エラー"
            '            strMesod = "mfFtpPutSqlFile"
            '            strTable = ""
            '            strData = ""
            '            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Return False
        Finally
            '解放する
            'WebClient.Dispose()
            'FTPサーバ切断
            If fwrFTPObj Is Nothing Then
            Else
                fwrFTPObj.Close()
            End If
        End Try

    End Function

    ''' <summary>
    ''' ＦＴＰ送信
    ''' </summary>
    ''' <param name="ipstrPutPath">ＰＵＴするパス</param>
    ''' <param name="ipstrMakDirName">ＰＵＴするパス配下に作成するパス名</param>
    ''' <param name="ipstrPutFile">ＰＵＴするファイル名</param>
    ''' <returns></returns>
    ''' <remarks>作成できる階層はＰＵＴパスの１階層したまで</remarks>
    Private Function fFtpPutFile(ByVal ipstrPutPath As String, ByVal ipstrMakDirName As String, ByVal ipstrPutFile As String, ByVal strLocalPath As String) As Boolean

        'Dim webClient As New System.Net.WebClient()
        Dim ftpCredential As NetworkCredential = New NetworkCredential(clsFTP_Svr.UID, clsFTP_Svr.PWD)
        Dim strFileName As String = String.Empty
        Dim strUrl As String = "ftp://" & clsFTP_Svr.FTP_SERVER & "/" & ipstrPutPath
        '& ipstrMakDirName      'IPアドレス
        Dim fwqFTPObj As FtpWebRequest
        Dim fwrFTPObj As FtpWebResponse = Nothing
        Dim aryDum As String()
        Dim chkDirPath As String = Nothing
        Dim splPath As String() = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try

            aryDum = ipstrPutFile.Split("\")
            strFileName = aryDum(aryDum.Length - 1)

            splPath = ipstrPutPath.Split("\")
            chkDirPath = splPath(0)
            For spl As Integer = 0 To splPath.Count - 3

                'フォルダを1つずつ確認するためのパスを作成
                chkDirPath &= "\" & splPath(spl + 1)
                'GetListの実行
                If fFtpDirExists(chkDirPath) = False Then
                    fwqFTPObj = System.Net.WebRequest.Create("ftp://" & clsFTP_Svr.FTP_SERVER & "/" & chkDirPath & "/")
                    'ログインユーザー名とパスワードを設定
                    fwqFTPObj.Credentials = New System.Net.NetworkCredential(clsFTP_Svr.UID, clsFTP_Svr.PWD)
                    'MethodにWebRequestMethods.Ftp.DeleteFile(DELE)を設定
                    fwqFTPObj.Method = System.Net.WebRequestMethods.Ftp.MakeDirectory
                    '要求の完了後に接続を閉じる
                    fwqFTPObj.KeepAlive = False
                    'ASCIIモードで転送する
                    fwqFTPObj.UseBinary = False
                    'PASSIVEモードを無効にする
                    fwqFTPObj.UsePassive = False
                    'FtpWebResponseを取得
                    fwrFTPObj = fwqFTPObj.GetResponse()
                    '閉じる
                    fwrFTPObj.Close()
                End If

            Next

            'FtpWebRequestの作成
            fwqFTPObj = System.Net.WebRequest.Create(strUrl + strFileName)
            'ログインユーザー名とパスワードを設定
            fwqFTPObj.Credentials = ftpCredential
            'MethodにWebRequestMethods.Ftp.UploadFile("STOR")を設定
            fwqFTPObj.Method = System.Net.WebRequestMethods.Ftp.UploadFile
            '要求の接続終了でログアウトする
            fwqFTPObj.KeepAlive = False
            'Binaryモードで転送する
            fwqFTPObj.UseBinary = True
            'PASSIVEモードを無効にする
            fwqFTPObj.UsePassive = False

            'ファイルをアップロードするためのStreamを取得
            Dim objFTPreqStrm As System.IO.Stream = fwqFTPObj.GetRequestStream
            'アップロードするファイルを開く
            Dim fsmUpFile As FileStream = New System.IO.FileStream(strLocalPath, System.IO.FileMode.Open, System.IO.FileAccess.Read)
            'アップロードStreamに書き込む
            Dim buffer(fsmUpFile.Length) As Byte
            While True
                Dim readSize As Integer = fsmUpFile.Read(buffer, 0, buffer.Length)
                If readSize = 0 Then
                    Exit While
                End If
                objFTPreqStrm.Write(buffer, 0, readSize)
            End While
            fsmUpFile.Close()
            objFTPreqStrm.Close()

            'FtpWebResponseを取得
            fwrFTPObj = fwqFTPObj.GetResponse()

            'FTP送信ログ出力
            '                msSeijyouLog(strProcCd, strFileName, strSql, strFilePath)

            '閉じる
            fwrFTPObj.Close()

            Return True

        Catch ex As WebException
            '            strErrBunrui = "ＦＴＰ接続エラー"
            '            strMesod = "mfFtpPutSqlFile"
            '            strTable = ""
            '            strData = ""
            '            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Return False
        Catch ex As Exception
            '            strErrBunrui = "その他処理エラー"
            '            strMesod = "mfFtpPutSqlFile"
            '            strTable = ""
            '            strData = ""
            '            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Return False
        Finally
            '解放する
            'WebClient.Dispose()
            'FTPサーバ切断
            If fwrFTPObj Is Nothing Then
            Else
                fwrFTPObj.Close()
            End If
        End Try

    End Function

    '----------------------------------------------------------------------------------------------------------------------
    '-　ＦＴＰ　一覧取得
    '----------------------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' ＦＴＰ受信ファイル名取得
    ''' </summary>
    ''' <param name="ipstrGetPath">一覧を取得するパス</param>
    ''' <param name="oparyGetList">取得した一覧</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function fFtpGetList(ByVal ipstrGetPath As String, ByRef oparyGetList As ArrayList) As Boolean

        Dim strUrl As String = "ftp://" & clsFTP_Svr.FTP_SERVER & "/" & ipstrGetPath     'IPアドレス
        Dim fwqFTPObj As FtpWebRequest
        Dim fwrFTPObj As FtpWebResponse = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try

            fwqFTPObj = System.Net.WebRequest.Create(strUrl)

            psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "strUrl:" & strUrl, "Catch")
            'ログインユーザー名とパスワードを設定
            fwqFTPObj.Credentials = New System.Net.NetworkCredential(clsFTP_Svr.UID, clsFTP_Svr.PWD)
            'MethodにWebRequestMethods.Ftp.ListDirectoryDetails("LIST")を設定
            fwqFTPObj.Method = System.Net.WebRequestMethods.Ftp.ListDirectory
            '要求の完了後に接続を閉じる
            fwqFTPObj.KeepAlive = False
            'PASSIVEモードを無効にする
            fwqFTPObj.UsePassive = False
            'FtpWebResponseを取得
            fwrFTPObj = fwqFTPObj.GetResponse()
            'FTPサーバーから送信されたデータを取得
            Dim ftpStr As StreamReader = New StreamReader(fwrFTPObj.GetResponseStream())
            Dim strRes As String = ftpStr.ReadToEnd()
            Dim strGetName = strRes.ToString.Replace(Environment.NewLine, ",")
            '前後の","を削除する
            strGetName = strGetName.Trim(","c)
            ftpStr.Close()

            Dim strGetNameList As String()
            Dim aryGetNameList As New ArrayList
            strGetNameList = strGetName.ToString.Split(",")
            Dim zz As Integer = 0
            For i = 0 To strGetNameList.Length - 1
                'ファイルの一覧を作成
                aryGetNameList.Add(strGetNameList(i))
            Next

            'FTPサーバーから送信されたステータスを表示
            ''Console.WriteLine("{0}: {1}", ftpRes.StatusCode, ftpRes.StatusDescription)
            '閉じる
            fwrFTPObj.Close()

            'ログ出力
            psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", aryGetNameList(0).ToString, "Catch")

            oparyGetList = aryGetNameList

            Return True

        Catch ex As WebException
            'strErrBunrui = "ＦＴＰ接続エラー"
            'strMesod = "mfGetFtpFileName"
            'strTable = ""
            'strData = ""
            'msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "WEBEX:" & ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Return False
        Catch ex As Exception
            'strErrBunrui = "その他処理エラー"
            'strMesod = "mfGetFtpFileName"
            'strTable = ""
            'strData = ""
            'msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Return False
        Finally
            If fwrFTPObj Is Nothing Then
            Else
                fwrFTPObj.Close()
            End If
        End Try
        'ログ出力
        psLogWrite2("END", "FTP", objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "", "Catch")

    End Function

    '----------------------------------------------------------------------------------------------------------------------
    '-　ＦＴＰ受信
    '----------------------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' ＦＴＰファイル受信
    ''' </summary>
    ''' <param name="ipstrGetFilePath">取得するファイルパス</param>
    ''' <param name="ipstrFileName">取得するファイル名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function fFtpGetFile(ByVal ipstrGetFilePath As String, ByVal ipstrFileName As String, ByVal strLocalName As String) As Boolean
        'ログ出力
        psLogWrite2("START", "FTP", objStack.GetMethod.DeclaringType.Name,
                        objStack.GetMethod.Name, "", "", "Catch")
        Dim strUrl As String = "ftp://" & clsFTP_Svr.FTP_SERVER & "/" & ipstrGetFilePath     'IPアドレス
        Dim fwqFTPObj As FtpWebRequest
        Dim fwrFTPObj As FtpWebResponse = Nothing
        Dim strFileName As String
        Dim aryDum As String()
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try

            aryDum = ipstrFileName.Split("\")
            strFileName = aryDum(aryDum.Length - 1)

            'FtpWebRequestの作成
            fwqFTPObj = System.Net.WebRequest.Create(strUrl & strFileName)
            'ログインユーザー名とパスワードを設定
            fwqFTPObj.Credentials = New System.Net.NetworkCredential(clsFTP_Svr.UID, clsFTP_Svr.PWD)
            'MethodにWebRequestMethods.Ftp.DownloadFile("RETR")を設定
            fwqFTPObj.Method = System.Net.WebRequestMethods.Ftp.DownloadFile
            '要求の完了後に接続を閉じる
            fwqFTPObj.KeepAlive = False
            '--------------------------------
            '2014/05/21　ここから　高松
            '--------------------------------
            ''ASCIIモードで転送する
            'ftpReq.UseBinary = False
            ''PASSIVEモードを無効にする
            'ftpReq.UsePassive = False
            'Binaryモードで転送する
            fwqFTPObj.UseBinary = True
            'PASSIVEモードを有効にする
            fwqFTPObj.UsePassive = True
            '--------------------------------
            '2014/05/21　ここまで　高松
            '--------------------------------

            'FtpWebResponseを取得
            fwrFTPObj = fwqFTPObj.GetResponse()
            'ファイルをダウンロードするためのStreamを取得
            '            strFileName = ipstrGetFilePath & "\" & ipstrFileName
            Dim resStrm As System.IO.Stream = fwrFTPObj.GetResponseStream()
            'ダウンロードしたファイルを書き込むためのFileStreamを作成
            Dim fs As New System.IO.FileStream("C:" & "\" & strLocalName, System.IO.FileMode.Create, System.IO.FileAccess.Write)


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
            fwrFTPObj.Close()

            'FTP受信ログ出力
            'msSeijyouLog("700", strGetSyoriCdFileName, strFilePath)

            Return True

        Catch ex As WebException
            'strErrBunrui = "ＦＴＰ接続エラー"
            'strMesod = "mfFtpGetCsvFile"
            'strTable = "strFileName"
            'strData = ""
            'msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Return False
        Catch ex As Exception
            'strErrBunrui = "その他処理エラー"
            'strMesod = "mfFtpGetCsvFile"
            'strTable = "strFileName"
            'strData = ""
            'msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Return False
        Finally
            If fwrFTPObj Is Nothing Then
            Else
                fwrFTPObj.Close()
            End If
        End Try
        'ログ出力
        psLogWrite2("END", "FTP", objStack.GetMethod.DeclaringType.Name,
                        objStack.GetMethod.Name, "", "", "Catch")
    End Function

    '----------------------------------------------------------------------------------------------------------------------
    '-　ＦＴＰファイル削除
    '----------------------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' ＦＴＰファイル削除
    ''' </summary>
    ''' <param name="ipstrDelFilePath">削除するファイルパス</param>
    ''' <param name="ipstrFileName">削除するファイル名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function fFtpDelFile(ByVal ipstrDelFilePath As String, ByRef ipstrFileName As String) As Boolean

        Dim strUrl As String = "ftp://" & clsFTP_Svr.FTP_SERVER & "/" & ipstrDelFilePath & "/"     'IPアドレス
        Dim fwqFTPObj As FtpWebRequest
        Dim fwrFTPObj As FtpWebResponse = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            '取り込んだファイルをFTP上から削除する

            fwqFTPObj = System.Net.WebRequest.Create(strUrl & ipstrFileName)

            'ログインユーザー名とパスワードを設定
            fwqFTPObj.Credentials = New System.Net.NetworkCredential(clsFTP_Svr.UID, clsFTP_Svr.PWD)
            'MethodにWebRequestMethods.Ftp.DeleteFile(DELE)を設定
            fwqFTPObj.Method = System.Net.WebRequestMethods.Ftp.DeleteFile
            '要求の完了後に接続を閉じる
            fwqFTPObj.KeepAlive = False
            'ASCIIモードで転送する
            fwqFTPObj.UseBinary = False
            'PASSIVEモードを無効にする
            fwqFTPObj.UsePassive = False
            'FtpWebResponseを取得
            fwrFTPObj = fwqFTPObj.GetResponse()
            '閉じる
            fwrFTPObj.Close()
            ''FTP受信ログ出力
            'msSeijyouLog(objDataRowSyoriCd(0), strGetSyoriCdFileName, strFilePath)
            Return True
        Catch ex As WebException
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Return False
        Catch ex As Exception
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Return False
        Finally
            If fwrFTPObj Is Nothing Then
            Else
                fwrFTPObj.Close()
            End If
        End Try

    End Function

    Private Function fFtpDirExists(ByVal ipstrDelFilePath As String) As Boolean

        Dim strUrl As String = "ftp://" & clsFTP_Svr.FTP_SERVER & "/" & ipstrDelFilePath    'IPアドレス
        Dim fwqFTPObj As FtpWebRequest
        Dim fwrFTPObj As FtpWebResponse = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            fwqFTPObj = System.Net.WebRequest.Create(strUrl)

            'ログインユーザー名とパスワードを設定
            fwqFTPObj.Credentials = New System.Net.NetworkCredential(clsFTP_Svr.UID, clsFTP_Svr.PWD)
            'MethodにWebRequestMethods.Ftp.DeleteFile(DELE)を設定
            fwqFTPObj.Method = System.Net.WebRequestMethods.Ftp.ListDirectory
            fwqFTPObj.Proxy = Nothing ' 接続の高速化
            '要求の完了後に接続を閉じる
            fwqFTPObj.KeepAlive = False
            'ASCIIモードで転送する
            fwqFTPObj.UseBinary = False
            'PASSIVEモードを無効にする
            fwqFTPObj.UsePassive = False
            'FtpWebResponseを取得
            fwrFTPObj = fwqFTPObj.GetResponse()
            '閉じる
            fwrFTPObj.Close()
            Return True
        Catch e As WebException
            If e.Status = WebExceptionStatus.ProtocolError Then
                Dim r As FtpWebResponse = CType(e.Response, FtpWebResponse)
                If r.StatusCode = FtpStatusCode.ActionNotTakenFileUnavailable Then
                    Return False
                End If
            End If
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite2("", "FTP", objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", e.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Throw ' ファイル関連以外の例外は再スロー
        Finally
            If Not fwrFTPObj Is Nothing Then
                fwrFTPObj.Close()
            End If
        End Try

        Return True

    End Function


    ''----------------------------------------------------------------------------------------------------------------------
    ''-　ＦＴＰファイル削除
    ''----------------------------------------------------------------------------------------------------------------------
    ' ''' <summary>
    ' ''' ＦＴＰファイル削除
    ' ''' </summary>
    ' ''' <param name="ipstrDelFilePath">削除するファイルパス</param>
    ' ''' <param name="ipstrFileName">削除するファイル名</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function fFtpDelFile(ByVal ipstrDelFilePath As String, ByRef ipstrFileName As String) As Boolean

    '    Dim strUrl As String = "ftp://" & clsFTP_Svr.FTP_SERVER & "/" & ipstrDelFilePath & "/"     'IPアドレス
    '    Dim fwqFTPObj As FtpWebRequest
    '    Dim fwrFTPObj As FtpWebResponse

    '    Try
    '        '取り込んだファイルをFTP上から削除する

    '        fwqFTPObj = System.Net.WebRequest.Create(strUrl & ipstrFileName)

    '        'ログインユーザー名とパスワードを設定
    '        fwqFTPObj.Credentials = New System.Net.NetworkCredential(clsFTP_Svr.UID, clsFTP_Svr.PWD)
    '        'MethodにWebRequestMethods.Ftp.DeleteFile(DELE)を設定
    '        fwqFTPObj.Method = System.Net.WebRequestMethods.Ftp.DeleteFile
    '        '要求の完了後に接続を閉じる
    '        fwqFTPObj.KeepAlive = False
    '        'ASCIIモードで転送する
    '        fwqFTPObj.UseBinary = False
    '        'PASSIVEモードを無効にする
    '        fwqFTPObj.UsePassive = False
    '        'FtpWebResponseを取得
    '        fwrFTPObj = fwqFTPObj.GetResponse()
    '        '閉じる
    '        fwrFTPObj.Close()
    '        ''FTP受信ログ出力
    '        'msSeijyouLog(objDataRowSyoriCd(0), strGetSyoriCdFileName, strFilePath)
    '        Return True
    '    Catch ex As WebException
    '        Return False
    '    Catch ex As Exception
    '        Return False
    '    Finally
    '        If fwrFTPObj Is Nothing Then
    '        Else
    '            fwrFTPObj.Close()
    '        End If
    '    End Try

    'End Function

    '--------------------------------
    '2014/04/14 星野　ここから
    '--------------------------------
    ''' <summary>
    ''' ログファイル出力
    ''' </summary>
    ''' <param name="strSessionID">セッションID</param>
    ''' <param name="strUserID">ログインID</param>
    ''' <param name="strClass">対象クラス</param>
    ''' <param name="strMethod">対象メソッド</param>
    ''' <param name="strMessageType">メッセージタイプ</param>
    ''' <param name="strMessage">メッセージ</param>
    ''' <param name="strLogType">ログタイプ</param>
    ''' <remarks></remarks>
    Public Shared Sub psLogWrite2(Optional ByVal strSessionID As String = "",
                                  Optional ByVal strUserID As String = "",
                                  Optional ByVal strClass As String = "",
                                  Optional ByVal strMethod As String = "",
                                  Optional ByVal strMessageType As String = "",
                                  Optional ByVal strMessage As String = "",
                                  Optional ByVal strLogType As String = "")

        Dim logDir As String = System.AppDomain.CurrentDomain.BaseDirectory & "Log"

        If strLogType = "Catch" Then
            logDir = System.AppDomain.CurrentDomain.BaseDirectory & "Log\" & DateTime.Now.ToString("yyyyMM")
            '--------------------------------
            '2014/04/16 星野　ここから
            '--------------------------------
            '■□■□結合試験時のみ使用予定□■□■
        ElseIf strLogType = "TRANS" Then
            logDir = System.AppDomain.CurrentDomain.BaseDirectory & "Log\" & DateTime.Now.ToString("yyyyMM") & "\TRANS"
            '■□■□結合試験時のみ使用予定□■□■
            '--------------------------------
            '2014/04/16 星野　ここまで
            '--------------------------------
        Else
            logDir = System.AppDomain.CurrentDomain.BaseDirectory & "Log"
        End If

        'ログフォルダ名作成
        System.IO.Directory.CreateDirectory(logDir)

        'ログの書き込み処理（書き込み失敗時は0.25秒置きに1000回リトライ）
        For zz As Integer = 0 To 1000
            Try
                'ログファイルをロックしてオープン
                Using objFs As New FileStream(logDir + "\" + DateTime.Now.ToString("yyyyMMdd") + ".log",
                                              FileMode.Append, FileAccess.Write, FileShare.None)

                    Dim objBuff As New StringBuilder    'StringBuilderクラス

                    '日時
                    objBuff.Append(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))
                    objBuff.Append(Microsoft.VisualBasic.ControlChars.Tab)
                    'セッションID
                    If strLogType <> "Catch" Then
                        objBuff.Append(strSessionID)
                        objBuff.Append(Microsoft.VisualBasic.ControlChars.Tab)
                    End If
                    'ログインID
                    objBuff.Append(strUserID)
                    objBuff.Append(Microsoft.VisualBasic.ControlChars.Tab)
                    '対象クラス
                    objBuff.Append(strClass)
                    objBuff.Append(Microsoft.VisualBasic.ControlChars.Tab)
                    '対象メソッド
                    objBuff.Append(strMethod)
                    objBuff.Append(Microsoft.VisualBasic.ControlChars.Tab)
                    'メッセージタイプ
                    If strLogType <> "Catch" Then
                        objBuff.Append(strMessageType)
                        objBuff.Append(Microsoft.VisualBasic.ControlChars.Tab)
                    End If
                    'メッセージ
                    objBuff.AppendLine(strMessage.Replace("\n", ""))

                    'ログ出力
                    objFs.Write(Encoding.GetEncoding("shift-jis").GetBytes(objBuff.ToString), 0,
                                Encoding.GetEncoding("shift-jis").GetByteCount(objBuff.ToString))

                    'ファイルクローズ
                    objFs.Close()

                End Using

                Exit Sub

            Catch ex As Exception
                System.Threading.Thread.Sleep(250)
            Finally
            End Try
        Next

    End Sub
    '--------------------------------
    '2014/04/14 星野　ここまで
    '--------------------------------

End Class
