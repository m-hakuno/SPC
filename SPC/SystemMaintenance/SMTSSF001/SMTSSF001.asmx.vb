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
#End Region

' この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://localhost:49741/SystemMaintenance/SMTSSF001/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SMTSSF001

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
    Private Const strLOGFILE As String = "FTPSND_"

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
    ''' 電文処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function pfSendFtp() As Boolean
        ' ＤＢ接続変数
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As New SqlCommand
        Dim strFilePath As String = ""
        Dim strSqlFilePath As String = ""
        Dim strErrKbn As String = "0"
        Dim objDataSet As DataSet = Nothing
        Dim objDtSetConnect As DataSet = Nothing
        'URL
        Dim strUrl As String = ""
        'USERID
        Dim strUserId As String = ""
        'PassWord
        Dim strPassWord As String = ""

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

            '保存場所取得処理（ＳＱＬファイル出力先）
            If mfGetHozonSQL(cmdDB, conDB, strSqlFilePath, strFilePath) = False Then
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
            '取得できた内容をデータテーブルに設定する
            Dim objDtSetNl As DataSet = Nothing
            Dim objDataRow As DataRow
            Dim dtTable As DataTable
            Dim intTblCount As Integer = 0
            Dim dtmYoukyu As DateTime    '要求日付
            Dim strTuban As String       '要求通番
            Dim strMoveCls As String     '遷移区分
            Dim strTboxId As String      'TBOXID
            Dim strProcCd As String      '処理コード
            Dim strReqMngNo As String    '依頼管理番号
            Dim strNLKbn As String       'NL区分
            Dim strTboxSyubetu As String 'TBOX種別
            dtTable = objDataSet.Tables(0)

            '取得テーブルの件数を取得
            intTblCount = dtTable.Rows.Count

            For i = 0 To intTblCount - 1
                objDataRow = dtTable.Rows(i)
                dtmYoukyu = objDataRow(0)      '要求日付（D83_REFERENCE_DATA)
                strTuban = objDataRow(1)       '要求通番（D83_REFERENCE_DATA)
                strMoveCls = objDataRow(2)     '遷移区分（D83_REFERENCE_DATA)
                strTboxId = objDataRow(3)      'TBOXID（D83_REFERENCE_DATA)
                strProcCd = objDataRow(4)      '処理コード（D83_REFERENCE_DATA)
                strReqMngNo = objDataRow(5)    '依頼管理番号（D83_REFERENCE_DATA)
                strNLKbn = objDataRow(6)       'NL区分（T03_TBOX）
                strTboxSyubetu = objDataRow(7) 'TBOX種別（M03_TBOX）

                '接続先情報取得処理
                If mfGetAccessPoint(cmdDB, conDB, strNLKbn, objDtSetConnect, strFilePath) = False Then
                    '取得できない場合
                    Return False
                End If
                '取得できた内容を設定する
                Dim dtTableConnect As DataTable
                Dim objDataRowConnect As DataRow
                dtTableConnect = objDtSetConnect.Tables(0)
                objDataRowConnect = dtTableConnect.Rows(0)
                strUrl = "ftp://" & objDataRowConnect(0) & "/"     'IPアドレス
                strUserId = objDataRowConnect(1)   'ユーザID
                strPassWord = objDataRowConnect(2) 'パスワード

                'ＳＱＬ文作成処理
                If mfMkSqlFile(cmdDB, conDB, strUrl, strUserId, strPassWord, dtmYoukyu, strTuban, strMoveCls, _
                               strTboxId, strProcCd, strReqMngNo, strNLKbn, _
                               strTboxSyubetu, strFilePath, strSqlFilePath) = False Then
                    'ＳＱＬ文生成でエラーのため異常終了
                    Return False
                End If

                '照会要求データ更新処理
                If mfUpdateReferenceData(cmdDB, conDB, dtmYoukyu, strTuban, strMoveCls, strFilePath) = False Then
                    '更新できない場合
                    Return False
                End If

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
                cmdDB = New SqlCommand("SMTSSF001_S1", conDB)

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
            strData = "SMTSSF001_S1"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 保存場所取得処理（ＳＱＬファイル）
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="strSqlFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetHozonSQL(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                   ByRef strSqlFilePath As String, ByVal strFilePath As String) As Boolean
        Try
            Using trans1 As SqlTransaction = conDB.BeginTransaction
                'プロシージャ設定
                cmdDB = New SqlCommand("SMTSSF001_S3", conDB)

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
                    strSqlFilePath = "\\" & objDataRow(0) & "\" & objDataRow(1)
                Else
                    Throw New Exception("送信ファイル一時作成場所取得エラー")
                End If
            End Using

            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetHozonSQL"
            strTable = "M78_PRESERVE_PLACE"
            strData = "SMTSSF001_S3"
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
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetReferenceData(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, ByRef objDataSet As DataSet, ByVal strFilePath As String, ByRef strErrKbn As String) As Boolean
        Try
            'エラー区分を設定（戻った際にCatchされたか、データ無しかの判定に使用）
            strErrKbn = "0"

            Using trans1 As SqlTransaction = conDB.BeginTransaction
                'プロシージャ設定
                cmdDB = New SqlCommand("SMTSSF001_S2", conDB)

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
            strData = "SMTSSF001_S2"
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
    ''' <param name="strNLKbn"></param>
    ''' <param name="objDtSetConnect"></param>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetAccessPoint(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, ByVal strNLKbn As String, _
                                      ByRef objDtSetConnect As DataSet, ByVal strFilePath As String) As Boolean
        'エラー区分を設定（戻った際にCatchされたか、データ無しかの判定に使用）
        Try
            Using trans1 As SqlTransaction = conDB.BeginTransaction
                'プロシージャ設定
                cmdDB = New SqlCommand("SMTSSF001_S7", conDB)

                With cmdDB.Parameters
                    .Add(pfSet_Param("prmM81_NL_CLS", SqlDbType.NVarChar, strNLKbn))
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
                    strData = "SMTSSF001_S7"
                    msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, "該当レコードなし")
                    Return False
                End If
            End Using
            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetAccessPoint"
            strTable = "M81_ACCESS_POINT"
            strData = "SMTSSF001_S7"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try
    End Function


    ''' <summary>
    ''' ＳＱＬファイル作成
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="dtmYoukyu"></param>
    ''' <param name="strTuban"></param>
    ''' <param name="strMoveCls"></param>
    ''' <param name="strTboxId"></param>
    ''' <param name="strProcCd"></param>
    ''' <param name="strReqMngNo"></param>
    ''' <param name="strSqlFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfMkSqlFile(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                 ByVal strUrl As String, ByVal strUserId As String, ByVal strPassWord As String, _
                                 ByVal dtmYoukyu As DateTime, ByVal strTuban As String, ByVal strMoveCls As String, _
                                 ByVal strTboxId As String, ByVal strProcCd As String, ByVal strReqMngNo As String, _
                                 ByVal strNLKbn As String, ByVal strTboxSyubetu As String, _
                                 ByVal strFilePath As String, ByVal strSqlFilePath As String) As Boolean
        Try
            Dim strSetHeading As String = "set HEADING OFF"
            Dim strSetFeedback As String = "set FEEDBACK OFF"
            Dim strSetVerify As String = "set VERIFY OFF"
            Dim strSetLinesize As String = "set LINESIZE 160"
            Dim strExit As String = "exit;"
            Dim strSelect As String = ""
            Dim strInsert As String = ""
            Dim objSelectInsert As DataSet = Nothing

            'Dim objdtGetTuban As DataSet = Nothing
            'Dim strFileTuban As String = ""
            'Dim dtGetTuban As DataTable
            'Dim objGetTubanDataRow As DataRow

            'Dim strSoushinFileName As String = ""
            'Dim strSoushinFileNameFinal As String = ""

            Dim objInsertDataRow As DataRow
            Dim dtInsetTable As DataTable
            Dim strIraiTuban As String
            Dim strIraiNichiji As String
            Dim strKakuchoTbox As String

            Dim strIraiDataClass(15) As String
            Dim strIraiUnyouDate(15) As String
            Dim strIraiKakuchoBB(15) As String
            Dim strIraiCid(15) As String
            Dim strIraiNyukin(15) As String

            Dim strJyoutaiFlg As String
            Dim strYokyuNichiji As String
            Dim strYokyuTuban As String

            Dim dtSqldtTable As DataTable
            Dim intSqldtCount As Integer

            Dim objSqlDataRow As DataRow

            Dim strD130UnyouKanshi As String
            Dim strD130Shikibetu As String
            Dim strD130ShoukaiTuban As String
            Dim strD130EdaBan As String
            Dim strD130Renban As String
            Dim strD130TboxId As String
            Dim strD130DataSyubetu As String = ""

            Dim strWatashiTableName As String = ""

            'SQL文作成
            Select Case strProcCd
                Case "901", "902"
                    strSelect = "SELECT * FROM tm_shsn_knr"
                    If strNLKbn = "N" Or strNLKbn = "J" Then
                        strSelect = strSelect & " WHERE kkchutbxid = " & "'20" & strTboxId & "';"
                    Else
                        strSelect = strSelect & " WHERE kkchutbxid = " & "'10" & strTboxId & "';"
                    End If
                    strWatashiTableName = "D104_TM_SHSN_KNR"
                    '------------------------------------
                    '通番を発行後ファイルを作成しＦＴＰ送信を行う
                    If mfFtpSoushinKanren(cmdDB, conDB, _
                                       strReqMngNo, _
                                       strTboxId, _
                                       strProcCd, _
                                       strD130DataSyubetu, _
                                       "1", _
                                       strUrl, _
                                       strUserId, _
                                       strPassWord, _
                                       strSetHeading, _
                                       strSetFeedback, _
                                       strSetVerify, _
                                       strSetLinesize, _
                                       strExit, _
                                       strSelect, _
                                       strWatashiTableName, _
                                       strFilePath, _
                                       strSqlFilePath) = False Then
                        Return False
                    End If
                    '------------------------------------
                Case "401", "402"
                    strSelect = "SELECT * FROM tm_hisn_knr"
                    If strNLKbn = "N" Or strNLKbn = "J" Then
                        strSelect = strSelect & " WHERE kkchutbxid = " & "'20" & strTboxId & "';"
                    Else
                        strSelect = strSelect & " WHERE kkchutbxid = " & "'10" & strTboxId & "';"
                    End If
                    strSelect = strSelect & "   AND dt_shbt = " & "'47'" & ";"
                    strWatashiTableName = "D103_TM_HISN_KNR"
                    '------------------------------------
                    '通番を発行後ファイルを作成しＦＴＰ送信を行う
                    If mfFtpSoushinKanren(cmdDB, conDB, _
                                       strReqMngNo, _
                                       strTboxId, _
                                       strProcCd, _
                                       strD130DataSyubetu, _
                                       "1", _
                                       strUrl, _
                                       strUserId, _
                                       strPassWord, _
                                       strSetHeading, _
                                       strSetFeedback, _
                                       strSetVerify, _
                                       strSetLinesize, _
                                       strExit, _
                                       strSelect, _
                                       strWatashiTableName, _
                                       strFilePath, _
                                       strSqlFilePath) = False Then
                        Return False
                    End If
                    '------------------------------------
                Case "200"
                    If strMoveCls = "1" Then
                        '照会要求受付ＤＢ取得
                        If mfGetD101InqreceptSql(cmdDB, conDB, strReqMngNo, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If

                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strIraiTuban = objInsertDataRow(0)      '依頼通番（D101_INQRECEPT)
                        strIraiNichiji = objInsertDataRow(1)    '依頼日時（D101_INQRECEPT)
                        strKakuchoTbox = objInsertDataRow(2)    '拡張T-BOX（D101_INQRECEPT)
                        Dim j As Integer = 3
                        For i = 0 To 14
                            '依頼1～15データ種別（D101_INQRECEPT)
                            If objInsertDataRow(j) Is DBNull.Value Then
                                strIraiDataClass(i) = ""
                            Else
                                strIraiDataClass(i) = objInsertDataRow(j)
                            End If
                            '依頼1～15運用日付（D101_INQRECEPT)
                            If objInsertDataRow(j + 1) Is DBNull.Value Then
                                strIraiUnyouDate(i) = ""
                            Else
                                strIraiUnyouDate(i) = objInsertDataRow(j + 1)
                            End If
                            '依頼1～15拡張ＢＢ１シリアルＮｏ（D101_INQRECEPT)
                            If objInsertDataRow(j + 2) Is DBNull.Value Then
                                strIraiKakuchoBB(i) = ""
                            Else
                                strIraiKakuchoBB(i) = objInsertDataRow(j + 2)
                            End If
                            '依頼1～15Ｃ－ＩＤ（D101_INQRECEPT）
                            If objInsertDataRow(j + 3) Is DBNull.Value Then
                                strIraiCid(i) = ""
                            Else
                                strIraiCid(i) = objInsertDataRow(j + 3)
                            End If
                            '依頼1～15入金伝票（D101_INQRECEPT）
                            If objInsertDataRow(j + 4) Is DBNull.Value Then
                                strIraiNyukin(i) = ""
                            Else
                                strIraiNyukin(i) = objInsertDataRow(j + 4)
                            End If
                            j = j + 5
                        Next
                        strInsert = "INSERT INTO tt_shukiyuky_uktsk VALUES ("
                        strInsert = strInsert & "'" & strIraiTuban & "',"
                        strInsert = strInsert & "'" & strIraiNichiji & "',"
                        strInsert = strInsert & "'" & strKakuchoTbox & "',"
                        For i = 0 To 14
                            strInsert = strInsert & "'" & strIraiDataClass(i) & "',"
                            strInsert = strInsert & "'" & strIraiUnyouDate(i) & "',"
                            strInsert = strInsert & "'" & strIraiKakuchoBB(i) & "',"
                            strInsert = strInsert & "'" & strIraiCid(i) & "',"
                            strInsert = strInsert & "'" & strIraiNyukin(i) & "',"
                        Next
                        strInsert = strInsert & "'    ',"
                        strInsert = strInsert & "'  ',"
                        strInsert = strInsert & "'        ',"
                        strInsert = strInsert & "'      '"
                        strInsert = strInsert & ");"
                        strWatashiTableName = "D101_INQRECEPT"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "2", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strInsert, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "2" Then
                        '照会要求受付ＤＢ取得
                        If mfGetD101InqreceptSql(cmdDB, conDB, strReqMngNo, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strIraiTuban = objInsertDataRow(0)      '依頼通番（D101_INQRECEPT)
                        strIraiNichiji = objInsertDataRow(1)    '依頼日時（D101_INQRECEPT)
                        strSelect = "SELECT RESULT, RESULT_NO, REQSEQ, REQDATE FROM tt_shukiyuky_uktsk"
                        strSelect = strSelect & " WHERE REQSEQ_SPC = '" & strIraiTuban & "'"
                        strSelect = strSelect & "   AND REQDATE_SPC = '" & strIraiNichiji & "';"
                        strWatashiTableName = "D101_INQRECEPT"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "3" Then
                        '照会要求受付ＤＢ取得
                        If mfGetD101InqreceptSql(cmdDB, conDB, strReqMngNo, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strYokyuNichiji = objInsertDataRow(80)  '要求日付（D101_INQRECEPT)
                        strYokyuTuban = objInsertDataRow(81)    '要求通番（D101_INQRECEPT)
                        strSelect = "SELECT * FROM T_CT_REQFSE"
                        strSelect = strSelect & " WHERE REQDATE = '" & strYokyuNichiji & "'"
                        strSelect = strSelect & "   AND REQSEQ = '" & strYokyuTuban & "';"
                        strWatashiTableName = "D129_REQFSE"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "4" Then
                        '照会要求受付ＤＢ取得
                        If mfGetD101InqreceptSql(cmdDB, conDB, strReqMngNo, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strYokyuNichiji = objInsertDataRow(80)  '要求日付（D101_INQRECEPT)
                        strYokyuTuban = objInsertDataRow(81)    '要求通番（D101_INQRECEPT)
                        '照会要求親データ取得
                        If mfGetD129Reqfse(cmdDB, conDB, strReqMngNo, strYokyuNichiji, strYokyuTuban, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strYokyuNichiji = objInsertDataRow(11)  '要求日付（D129_REQFSE)
                        strYokyuTuban = objInsertDataRow(12)    '要求日付（D129_REQFSE)
                        strSelect = "SELECT * FROM T_CT_REQFSH"
                        strSelect = strSelect & " WHERE REQDATE =  '" & strYokyuNichiji & "'"
                        strSelect = strSelect & "   AND REQSEQ = '" & strYokyuTuban & "';"
                        strWatashiTableName = "D130_REQFSH"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "5" Then
                        '照会要求受付ＤＢ取得
                        If mfGetD101InqreceptSql(cmdDB, conDB, strReqMngNo, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strYokyuNichiji = objInsertDataRow(80)  '要求日付（D101_INQRECEPT)
                        strYokyuTuban = objInsertDataRow(81)    '要求通番（D101_INQRECEPT)
                        '照会要求親データ取得
                        If mfGetD129Reqfse(cmdDB, conDB, strReqMngNo, strYokyuNichiji, strYokyuTuban, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strJyoutaiFlg = objInsertDataRow(0)     '状態フラグ（D129_REQFSE)
                        strYokyuNichiji = objInsertDataRow(11)  '要求日付（D129_REQFSE)
                        strYokyuTuban = objInsertDataRow(12)    '要求日付（D129_REQFSE)
                        '照会要求明細データ取得
                        If mfGetD130Reqfsh(cmdDB, conDB, strReqMngNo, strYokyuNichiji, strYokyuTuban, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtSqldtTable = objSelectInsert.Tables(0)

                        '取得テーブルの件数を取得
                        intSqldtCount = dtSqldtTable.Rows.Count
                        For i = 0 To intSqldtCount - 1
                            objSqlDataRow = dtSqldtTable.Rows(i)
                            strD130UnyouKanshi = objSqlDataRow(0)  '運用監視サーバ運用日付（D130_REQFSH)
                            strD130Shikibetu = objSqlDataRow(1)    '識別（D130_REQFSH)
                            strD130ShoukaiTuban = objSqlDataRow(2) '照会通番（D130_REQFSH)
                            strD130EdaBan = objSqlDataRow(3)       '枝番号（D130_REQFSH)
                            strD130Renban = objSqlDataRow(4)       '連番（D130_REQFSH)
                            strD130TboxId = objSqlDataRow(5)       'TBOXID（D130_REQFSH)
                            strD130DataSyubetu = objSqlDataRow(6)  'データ種別（D130_REQFSH)
                            'TBOX種別
                            Select Case strTboxSyubetu
                                Case "6", "9", "A", "P", "R", "S", "X"
                                    strSelect = "SELECT * FROM T_ZS_BBKIBAN"
                                    strSelect = strSelect & " WHERE FKSDATE = " & "'" & strD130UnyouKanshi & "'"
                                    strSelect = strSelect & "   AND SVRTYPE = " & "'" & strD130Shikibetu & "'"
                                    strSelect = strSelect & "   AND SHOKAISEQ = " & "'" & strD130ShoukaiTuban & "'"
                                    strSelect = strSelect & "   AND EDASEQ = " & "'" & strD130EdaBan & "'"
                                    strSelect = strSelect & "   AND DATASEQ = " & "'" & strD130Renban & "';"
                                    strWatashiTableName = "D135_BBKIBAN"
                                Case "B", "C"
                                    strSelect = "SELECT * FROM T_ZS_BBKIBAN2"
                                    strSelect = strSelect & " WHERE FKSDATE = " & "'" & strD130UnyouKanshi & "'"
                                    strSelect = strSelect & "   AND SVRTYPE = " & "'" & strD130Shikibetu & "'"
                                    strSelect = strSelect & "   AND SHOKAISEQ = " & "'" & strD130ShoukaiTuban & "'"
                                    strSelect = strSelect & "   AND EDASEQ = " & "'" & strD130EdaBan & "'"
                                    strSelect = strSelect & "   AND DATASEQ = " & "'" & strD130Renban & "';"
                                    strWatashiTableName = "D162_BBKIBAN2"
                            End Select
                            '------------------------------------
                            '通番を発行後ファイルを作成しＦＴＰ送信を行う
                            If mfFtpSoushinKanren(cmdDB, conDB, _
                                               strReqMngNo, _
                                               strTboxId, _
                                               strProcCd, _
                                               strD130DataSyubetu, _
                                               "1", _
                                               strUrl, _
                                               strUserId, _
                                               strPassWord, _
                                               strSetHeading, _
                                               strSetFeedback, _
                                               strSetVerify, _
                                               strSetLinesize, _
                                               strExit, _
                                               strSelect, _
                                               strWatashiTableName, _
                                               strFilePath, _
                                               strSqlFilePath) = False Then
                                Return False
                            End If
                            '------------------------------------
                        Next

                    End If
                Case "301"
                    If strMoveCls = "1" Then
                        '照会要求受付ＤＢ取得
                        If mfGetD101InqreceptSql(cmdDB, conDB, strReqMngNo, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If

                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strIraiTuban = objInsertDataRow(0)      '依頼通番（D101_INQRECEPT)
                        strIraiNichiji = objInsertDataRow(1)    '依頼日時（D101_INQRECEPT)
                        strKakuchoTbox = objInsertDataRow(2)    '拡張T-BOX（D101_INQRECEPT)
                        Dim j As Integer = 3
                        For i = 0 To 14
                            '依頼1～15データ種別（D101_INQRECEPT)
                            If objInsertDataRow(j) Is DBNull.Value Then
                                strIraiDataClass(i) = ""
                            Else
                                strIraiDataClass(i) = objInsertDataRow(j)
                            End If
                            '依頼1～15運用日付（D101_INQRECEPT)
                            If objInsertDataRow(j + 1) Is DBNull.Value Then
                                strIraiUnyouDate(i) = ""
                            Else
                                strIraiUnyouDate(i) = objInsertDataRow(j + 1)
                            End If
                            '依頼1～15拡張ＢＢ１シリアルＮｏ（D101_INQRECEPT)
                            If objInsertDataRow(j + 2) Is DBNull.Value Then
                                strIraiKakuchoBB(i) = ""
                            Else
                                strIraiKakuchoBB(i) = objInsertDataRow(j + 2)
                            End If
                            '依頼1～15Ｃ－ＩＤ（D101_INQRECEPT）
                            If objInsertDataRow(j + 3) Is DBNull.Value Then
                                strIraiCid(i) = ""
                            Else
                                strIraiCid(i) = objInsertDataRow(j + 3)
                            End If
                            '依頼1～15入金伝票（D101_INQRECEPT）
                            If objInsertDataRow(j + 4) Is DBNull.Value Then
                                strIraiNyukin(i) = ""
                            Else
                                strIraiNyukin(i) = objInsertDataRow(j + 4)
                            End If
                            j = j + 5
                        Next
                        strInsert = "INSERT INTO tt_shukiyuky_uktsk VALUES ("
                        strInsert = strInsert & "'" & strIraiTuban & "',"
                        strInsert = strInsert & "'" & strIraiNichiji & "',"
                        strInsert = strInsert & "'" & strKakuchoTbox & "',"
                        For i = 0 To 14
                            strInsert = strInsert & "'" & strIraiDataClass(i) & "',"
                            strInsert = strInsert & "'" & strIraiUnyouDate(i) & "',"
                            strInsert = strInsert & "'" & strIraiKakuchoBB(i) & "',"
                            strInsert = strInsert & "'" & strIraiCid(i) & "',"
                            strInsert = strInsert & "'" & strIraiNyukin(i) & "',"
                        Next
                        strInsert = strInsert & "'    ',"
                        strInsert = strInsert & "'  ',"
                        strInsert = strInsert & "'        ',"
                        strInsert = strInsert & "'      '"
                        strInsert = strInsert & ");"
                        strWatashiTableName = "D101_INQRECEPT"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "2", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strInsert, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "2" Then
                        '照会要求受付ＤＢ取得
                        If mfGetD101InqreceptSql(cmdDB, conDB, strReqMngNo, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strIraiTuban = objInsertDataRow(0)      '依頼通番（D101_INQRECEPT)
                        strIraiNichiji = objInsertDataRow(1)    '依頼日時（D101_INQRECEPT)
                        strSelect = "SELECT RESULT, RESULT_NO, REQSEQ, REQDATE FROM tt_shukiyuky_uktsk"
                        strSelect = strSelect & " WHERE REQSEQ_SPC = '" & strIraiTuban & "'"
                        strSelect = strSelect & "   AND REQDATE_SPC = '" & strIraiNichiji & "';"
                        strWatashiTableName = "D101_INQRECEPT"
                        '------------------------------------
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "3" Then
                        '照会要求受付ＤＢ取得
                        If mfGetD101InqreceptSql(cmdDB, conDB, strReqMngNo, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strYokyuNichiji = objInsertDataRow(80)  '要求日付（D101_INQRECEPT)
                        strYokyuTuban = objInsertDataRow(81)    '要求通番（D101_INQRECEPT)
                        strSelect = "SELECT * FROM T_CT_REQFSE"
                        strSelect = strSelect & " WHERE REQDATE = '" & strYokyuNichiji & "'"
                        strSelect = strSelect & "   AND REQSEQ = '" & strYokyuTuban & "';"
                        strWatashiTableName = "D129_REQFSE"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "4" Then
                        '照会要求受付ＤＢ取得
                        If mfGetD101InqreceptSql(cmdDB, conDB, strReqMngNo, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strYokyuNichiji = objInsertDataRow(80)  '要求日付（D101_INQRECEPT)
                        strYokyuTuban = objInsertDataRow(81)    '要求通番（D101_INQRECEPT)
                        '照会要求親データ取得
                        If mfGetD129Reqfse(cmdDB, conDB, strReqMngNo, strYokyuNichiji, strYokyuTuban, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strYokyuNichiji = objInsertDataRow(11)  '要求日付（D129_REQFSE)
                        strYokyuTuban = objInsertDataRow(12)    '要求日付（D129_REQFSE)
                        strSelect = "SELECT * FROM T_CT_REQFSH"
                        strSelect = strSelect & " WHERE REQDATE =  '" & strYokyuNichiji & "'"
                        strSelect = strSelect & "   AND REQSEQ = '" & strYokyuTuban & "';"
                        strWatashiTableName = "D130_REQFSH"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "5" Then
                        '照会要求受付ＤＢ取得
                        If mfGetD101InqreceptSql(cmdDB, conDB, strReqMngNo, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strYokyuNichiji = objInsertDataRow(80)  '要求日付（D101_INQRECEPT)
                        strYokyuTuban = objInsertDataRow(81)    '要求通番（D101_INQRECEPT)
                        '照会要求親データ取得
                        If mfGetD129Reqfse(cmdDB, conDB, strReqMngNo, strYokyuNichiji, strYokyuTuban, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strJyoutaiFlg = objInsertDataRow(0)     '状態フラグ（D129_REQFSE)
                        strYokyuNichiji = objInsertDataRow(11)  '要求日付（D129_REQFSE)
                        strYokyuTuban = objInsertDataRow(12)    '要求日付（D129_REQFSE)
                        '照会要求明細データ取得
                        If mfGetD130Reqfsh(cmdDB, conDB, strReqMngNo, strYokyuNichiji, strYokyuTuban, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtSqldtTable = objSelectInsert.Tables(0)

                        '取得テーブルの件数を取得
                        intSqldtCount = dtSqldtTable.Rows.Count
                        For i = 0 To intSqldtCount - 1
                            objSqlDataRow = dtSqldtTable.Rows(i)
                            strD130UnyouKanshi = objSqlDataRow(0)  '運用監視サーバ運用日付（D130_REQFSH)
                            strD130Shikibetu = objSqlDataRow(1)    '識別（D130_REQFSH)
                            strD130ShoukaiTuban = objSqlDataRow(2) '照会通番（D130_REQFSH)
                            strD130EdaBan = objSqlDataRow(3)       '枝番号（D130_REQFSH)
                            strD130Renban = objSqlDataRow(4)       '連番（D130_REQFSH)
                            strD130TboxId = objSqlDataRow(5)       'TBOXID（D130_REQFSH)
                            strD130DataSyubetu = objSqlDataRow(6)  'データ種別（D130_REQFSH)

                            'データ種別
                            Select Case strD130DataSyubetu
                                Case "27"
                                    strSelect = "SELECT * FROM T_ZS_SOUTIKOSEI"
                                    strSelect = strSelect & " WHERE FKSDATE = " & "'" & strD130UnyouKanshi & "'"
                                    strSelect = strSelect & "   AND SVRTYPE = " & "'" & strD130Shikibetu & "'"
                                    strSelect = strSelect & "   AND SHOKAISEQ = " & "'" & strD130ShoukaiTuban & "'"
                                    strSelect = strSelect & "   AND EDASEQ = " & "'" & strD130EdaBan & "'"
                                    strSelect = strSelect & "   AND DATASEQ = " & "'" & strD130Renban & "';"
                                    strWatashiTableName = "D133_SOUTIKOSEI"
                                Case "39"
                                    strSelect = "SELECT * FROM T_ZS_SAIKENMEISAI"
                                    strSelect = strSelect & " WHERE FKSDATE = " & "'" & strD130UnyouKanshi & "'"
                                    strSelect = strSelect & "   AND SVRTYPE = " & "'" & strD130Shikibetu & "'"
                                    strSelect = strSelect & "   AND SHOKAISEQ = " & "'" & strD130ShoukaiTuban & "'"
                                    strSelect = strSelect & "   AND EDASEQ = " & "'" & strD130EdaBan & "'"
                                    strSelect = strSelect & "   AND DATASEQ = " & "'" & strD130Renban & "';"
                                    strWatashiTableName = "D147_SAIKENMEISAI"
                                Case "40"
                                    strSelect = "SELECT * FROM T_ZS_SAIMUMEISAI"
                                    strSelect = strSelect & " WHERE FKSDATE = " & "'" & strD130UnyouKanshi & "'"
                                    strSelect = strSelect & "   AND SVRTYPE = " & "'" & strD130Shikibetu & "'"
                                    strSelect = strSelect & "   AND SHOKAISEQ = " & "'" & strD130ShoukaiTuban & "'"
                                    strSelect = strSelect & "   AND EDASEQ = " & "'" & strD130EdaBan & "'"
                                    strSelect = strSelect & "   AND DATASEQ = " & "'" & strD130Renban & "';"
                                    strWatashiTableName = "D148_SAIMUMEISAI"
                                Case "42"
                                    strSelect = "SELECT * FROM T_ZS_SEISANMEISAI"
                                    strSelect = strSelect & " WHERE FKSDATE = " & "'" & strD130UnyouKanshi & "'"
                                    strSelect = strSelect & "   AND SVRTYPE = " & "'" & strD130Shikibetu & "'"
                                    strSelect = strSelect & "   AND SHOKAISEQ = " & "'" & strD130ShoukaiTuban & "'"
                                    strSelect = strSelect & "   AND EDASEQ = " & "'" & strD130EdaBan & "'"
                                    strSelect = strSelect & "   AND DATASEQ = " & "'" & strD130Renban & "';"
                                    strWatashiTableName = "D150_SEISANMEISAI"
                            End Select
                            '------------------------------------
                            '通番を発行後ファイルを作成しＦＴＰ送信を行う
                            If mfFtpSoushinKanren(cmdDB, conDB, _
                                               strReqMngNo, _
                                               strTboxId, _
                                               strProcCd, _
                                               strD130DataSyubetu, _
                                               "1", _
                                               strUrl, _
                                               strUserId, _
                                               strPassWord, _
                                               strSetHeading, _
                                               strSetFeedback, _
                                               strSetVerify, _
                                               strSetLinesize, _
                                               strExit, _
                                               strSelect, _
                                               strWatashiTableName, _
                                               strFilePath, _
                                               strSqlFilePath) = False Then
                                Return False
                            End If
                            '------------------------------------
                        Next

                    End If
                Case "302"
                    If strMoveCls = "1" Then
                        '照会要求受付ＤＢ取得
                        If mfGetD101InqreceptSql(cmdDB, conDB, strReqMngNo, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If

                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strIraiTuban = objInsertDataRow(0)      '依頼通番（D101_INQRECEPT)
                        strIraiNichiji = objInsertDataRow(1)    '依頼日時（D101_INQRECEPT)
                        strKakuchoTbox = objInsertDataRow(2)    '拡張T-BOX（D101_INQRECEPT)
                        Dim j As Integer = 3
                        For i = 0 To 14
                            '依頼1～15データ種別（D101_INQRECEPT)
                            If objInsertDataRow(j) Is DBNull.Value Then
                                strIraiDataClass(i) = ""
                            Else
                                strIraiDataClass(i) = objInsertDataRow(j)
                            End If
                            '依頼1～15運用日付（D101_INQRECEPT)
                            If objInsertDataRow(j + 1) Is DBNull.Value Then
                                strIraiUnyouDate(i) = ""
                            Else
                                strIraiUnyouDate(i) = objInsertDataRow(j + 1)
                            End If
                            '依頼1～15拡張ＢＢ１シリアルＮｏ（D101_INQRECEPT)
                            If objInsertDataRow(j + 2) Is DBNull.Value Then
                                strIraiKakuchoBB(i) = ""
                            Else
                                strIraiKakuchoBB(i) = objInsertDataRow(j + 2)
                            End If
                            '依頼1～15Ｃ－ＩＤ（D101_INQRECEPT）
                            If objInsertDataRow(j + 3) Is DBNull.Value Then
                                strIraiCid(i) = ""
                            Else
                                strIraiCid(i) = objInsertDataRow(j + 3)
                            End If
                            '依頼1～15入金伝票（D101_INQRECEPT）
                            If objInsertDataRow(j + 4) Is DBNull.Value Then
                                strIraiNyukin(i) = ""
                            Else
                                strIraiNyukin(i) = objInsertDataRow(j + 4)
                            End If
                            j = j + 5
                        Next
                        strInsert = "INSERT INTO tt_shukiyuky_uktsk VALUES ("
                        strInsert = strInsert & "'" & strIraiTuban & "',"
                        strInsert = strInsert & "'" & strIraiNichiji & "',"
                        strInsert = strInsert & "'" & strKakuchoTbox & "',"
                        For i = 0 To 14
                            strInsert = strInsert & "'" & strIraiDataClass(i) & "',"
                            strInsert = strInsert & "'" & strIraiUnyouDate(i) & "',"
                            strInsert = strInsert & "'" & strIraiKakuchoBB(i) & "',"
                            strInsert = strInsert & "'" & strIraiCid(i) & "',"
                            strInsert = strInsert & "'" & strIraiNyukin(i) & "',"
                        Next
                        strInsert = strInsert & "'    ',"
                        strInsert = strInsert & "'  ',"
                        strInsert = strInsert & "'        ',"
                        strInsert = strInsert & "'      '"
                        strInsert = strInsert & ");"
                        strWatashiTableName = "D101_INQRECEPT"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "2", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strInsert, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "2" Then
                        '照会要求受付ＤＢ取得
                        If mfGetD101InqreceptSql(cmdDB, conDB, strReqMngNo, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strIraiTuban = objInsertDataRow(0)      '依頼通番（D101_INQRECEPT)
                        strIraiNichiji = objInsertDataRow(1)    '依頼日時（D101_INQRECEPT)
                        strSelect = "SELECT RESULT, RESULT_NO, REQSEQ, REQDATE FROM tt_shukiyuky_uktsk"
                        strSelect = strSelect & " WHERE REQSEQ_SPC = '" & strIraiTuban & "'"
                        strSelect = strSelect & "   AND REQDATE_SPC = '" & strIraiNichiji & "';"
                        strWatashiTableName = "D101_INQRECEPT"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "3" Then
                        '照会要求受付ＤＢ取得
                        If mfGetD101InqreceptSql(cmdDB, conDB, strReqMngNo, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strYokyuNichiji = objInsertDataRow(80)  '要求日付（D101_INQRECEPT)
                        strYokyuTuban = objInsertDataRow(81)    '要求通番（D101_INQRECEPT)
                        strSelect = "SELECT * FROM T_CT_REQFSE"
                        strSelect = strSelect & " WHERE REQDATE = '" & strYokyuNichiji & "'"
                        strSelect = strSelect & "   AND REQSEQ = '" & strYokyuTuban & "';"
                        strWatashiTableName = "D129_REQFSE"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "4" Then
                        '照会要求受付ＤＢ取得
                        If mfGetD101InqreceptSql(cmdDB, conDB, strReqMngNo, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strYokyuNichiji = objInsertDataRow(80)  '要求日付（D101_INQRECEPT)
                        strYokyuTuban = objInsertDataRow(81)    '要求通番（D101_INQRECEPT)
                        '照会要求親データ取得
                        If mfGetD129Reqfse(cmdDB, conDB, strReqMngNo, strYokyuNichiji, strYokyuTuban, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strYokyuNichiji = objInsertDataRow(11)  '要求日付（D129_REQFSE)
                        strYokyuTuban = objInsertDataRow(12)    '要求日付（D129_REQFSE)
                        strSelect = "SELECT * FROM T_CT_REQFSH"
                        strSelect = strSelect & " WHERE REQDATE =  '" & strYokyuNichiji & "'"
                        strSelect = strSelect & "   AND REQSEQ = '" & strYokyuTuban & "';"
                        strWatashiTableName = "D130_REQFSH"
                        '------------------------------------
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "5" Then
                        '照会要求受付ＤＢ取得
                        If mfGetD101InqreceptSql(cmdDB, conDB, strReqMngNo, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strYokyuNichiji = objInsertDataRow(80)  '要求日付（D101_INQRECEPT)
                        strYokyuTuban = objInsertDataRow(81)    '要求通番（D101_INQRECEPT)
                        '照会要求親データ取得
                        If mfGetD129Reqfse(cmdDB, conDB, strReqMngNo, strYokyuNichiji, strYokyuTuban, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strJyoutaiFlg = objInsertDataRow(0)     '状態フラグ（D129_REQFSE)
                        strYokyuNichiji = objInsertDataRow(11)  '要求日付（D129_REQFSE)
                        strYokyuTuban = objInsertDataRow(12)    '要求日付（D129_REQFSE)
                        '照会要求明細データ取得
                        If mfGetD130Reqfsh(cmdDB, conDB, strReqMngNo, strYokyuNichiji, strYokyuTuban, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtSqldtTable = objSelectInsert.Tables(0)

                        '取得テーブルの件数を取得
                        intSqldtCount = dtSqldtTable.Rows.Count
                        For i = 0 To intSqldtCount - 1
                            objSqlDataRow = dtSqldtTable.Rows(i)
                            strD130UnyouKanshi = objSqlDataRow(0)  '運用監視サーバ運用日付（D130_REQFSH)
                            strD130Shikibetu = objSqlDataRow(1)    '識別（D130_REQFSH)
                            strD130ShoukaiTuban = objSqlDataRow(2) '照会通番（D130_REQFSH)
                            strD130EdaBan = objSqlDataRow(3)       '枝番号（D130_REQFSH)
                            strD130Renban = objSqlDataRow(4)       '連番（D130_REQFSH)
                            strD130TboxId = objSqlDataRow(5)       'TBOXID（D130_REQFSH)
                            strD130DataSyubetu = objSqlDataRow(6)  'データ種別（D130_REQFSH)

                            'TBOX種別
                            Select Case strTboxSyubetu
                                Case "6", "9", "A", "B", "C", "H"
                                    strSelect = "SELECT * FROM T_ZS_SIYOUCHUCARD"
                                    strSelect = strSelect & " WHERE FKSDATE = " & "'" & strD130UnyouKanshi & "'"
                                    strSelect = strSelect & "   AND SVRTYPE = " & "'" & strD130Shikibetu & "'"
                                    strSelect = strSelect & "   AND SHOKAISEQ = " & "'" & strD130ShoukaiTuban & "'"
                                    strSelect = strSelect & "   AND EDASEQ = " & "'" & strD130EdaBan & "'"
                                    strSelect = strSelect & "   AND DATASEQ = " & "'" & strD130Renban & "';"
                                    strWatashiTableName = "D131_SIYOUCHUCARD"
                                Case "P", "R", "S", "X"
                                    strSelect = "SELECT * FROM T_ZS_SIYOUCHUCARD2"
                                    strSelect = strSelect & " WHERE FKSDATE = " & "'" & strD130UnyouKanshi & "'"
                                    strSelect = strSelect & "   AND SVRTYPE = " & "'" & strD130Shikibetu & "'"
                                    strSelect = strSelect & "   AND SHOKAISEQ = " & "'" & strD130ShoukaiTuban & "'"
                                    strSelect = strSelect & "   AND EDASEQ = " & "'" & strD130EdaBan & "'"
                                    strSelect = strSelect & "   AND DATASEQ = " & "'" & strD130Renban & "';"
                                    strWatashiTableName = "D131_SIYOUCHUCARD"
                            End Select
                            '------------------------------------
                            '通番を発行後ファイルを作成しＦＴＰ送信を行う
                            If mfFtpSoushinKanren(cmdDB, conDB, _
                                               strReqMngNo, _
                                               strTboxId, _
                                               strProcCd, _
                                               strD130DataSyubetu, _
                                               "1", _
                                               strUrl, _
                                               strUserId, _
                                               strPassWord, _
                                               strSetHeading, _
                                               strSetFeedback, _
                                               strSetVerify, _
                                               strSetLinesize, _
                                               strExit, _
                                               strSelect, _
                                               strWatashiTableName, _
                                               strFilePath, _
                                               strSqlFilePath) = False Then
                                Return False
                            End If
                            '------------------------------------
                        Next
                    End If
                Case "100"
                    If strMoveCls = "1" Then
                        '照会要求受付ＤＢ取得
                        If mfGetD101InqreceptSql(cmdDB, conDB, strReqMngNo, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If

                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strIraiTuban = objInsertDataRow(0)      '依頼通番（D101_INQRECEPT)
                        strIraiNichiji = objInsertDataRow(1)    '依頼日時（D101_INQRECEPT)
                        strKakuchoTbox = objInsertDataRow(2)    '拡張T-BOX（D101_INQRECEPT)
                        Dim j As Integer = 3
                        For i = 0 To 14
                            '依頼1～15データ種別（D101_INQRECEPT)
                            If objInsertDataRow(j) Is DBNull.Value Then
                                strIraiDataClass(i) = ""
                            Else
                                strIraiDataClass(i) = objInsertDataRow(j)
                            End If
                            '依頼1～15運用日付（D101_INQRECEPT)
                            If objInsertDataRow(j + 1) Is DBNull.Value Then
                                strIraiUnyouDate(i) = ""
                            Else
                                strIraiUnyouDate(i) = objInsertDataRow(j + 1)
                            End If
                            '依頼1～15拡張ＢＢ１シリアルＮｏ（D101_INQRECEPT)
                            If objInsertDataRow(j + 2) Is DBNull.Value Then
                                strIraiKakuchoBB(i) = ""
                            Else
                                strIraiKakuchoBB(i) = objInsertDataRow(j + 2)
                            End If
                            '依頼1～15Ｃ－ＩＤ（D101_INQRECEPT）
                            If objInsertDataRow(j + 3) Is DBNull.Value Then
                                strIraiCid(i) = ""
                            Else
                                strIraiCid(i) = objInsertDataRow(j + 3)
                            End If
                            '依頼1～15入金伝票（D101_INQRECEPT）
                            If objInsertDataRow(j + 4) Is DBNull.Value Then
                                strIraiNyukin(i) = ""
                            Else
                                strIraiNyukin(i) = objInsertDataRow(j + 4)
                            End If
                            j = j + 5
                        Next
                        strInsert = "INSERT INTO tt_shukiyuky_uktsk VALUES ("
                        strInsert = strInsert & "'" & strIraiTuban & "',"
                        strInsert = strInsert & "'" & strIraiNichiji & "',"
                        strInsert = strInsert & "'" & strKakuchoTbox & "',"
                        For i = 0 To 14
                            strInsert = strInsert & "'" & strIraiDataClass(i) & "',"
                            strInsert = strInsert & "'" & strIraiUnyouDate(i) & "',"
                            strInsert = strInsert & "'" & strIraiKakuchoBB(i) & "',"
                            strInsert = strInsert & "'" & strIraiCid(i) & "',"
                            strInsert = strInsert & "'" & strIraiNyukin(i) & "',"
                        Next
                        strInsert = strInsert & "'    ',"
                        strInsert = strInsert & "'  ',"
                        strInsert = strInsert & "'        ',"
                        strInsert = strInsert & "'      '"
                        strInsert = strInsert & ");"
                        strWatashiTableName = "D101_INQRECEPT"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "2", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strInsert, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "2" Then
                        '照会要求受付ＤＢ取得
                        If mfGetD101InqreceptSql(cmdDB, conDB, strReqMngNo, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strIraiTuban = objInsertDataRow(0)      '依頼通番（D101_INQRECEPT)
                        strIraiNichiji = objInsertDataRow(1)    '依頼日時（D101_INQRECEPT)
                        strSelect = "SELECT RESULT, RESULT_NO, REQSEQ, REQDATE FROM tt_shukiyuky_uktsk"
                        strSelect = strSelect & " WHERE REQSEQ_SPC = '" & strIraiTuban & "'"
                        strSelect = strSelect & "   AND REQDATE_SPC = '" & strIraiNichiji & "';"
                        strWatashiTableName = "D101_INQRECEPT"
                        '------------------------------------
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "3" Then
                        '照会要求受付ＤＢ取得
                        If mfGetD101InqreceptSql(cmdDB, conDB, strReqMngNo, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strYokyuNichiji = objInsertDataRow(80)  '要求日付（D101_INQRECEPT)
                        strYokyuTuban = objInsertDataRow(81)    '要求通番（D101_INQRECEPT)
                        strSelect = "SELECT * FROM T_CT_REQFSE"
                        strSelect = strSelect & " WHERE REQDATE = '" & strYokyuNichiji & "'"
                        strSelect = strSelect & "   AND REQSEQ = '" & strYokyuTuban & "';"
                        strWatashiTableName = "D129_REQFSE"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "4" Then
                        '照会要求受付ＤＢ取得
                        If mfGetD101InqreceptSql(cmdDB, conDB, strReqMngNo, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strYokyuNichiji = objInsertDataRow(80)  '要求日付（D101_INQRECEPT)
                        strYokyuTuban = objInsertDataRow(81)    '要求通番（D101_INQRECEPT)
                        '照会要求親データ取得
                        If mfGetD129Reqfse(cmdDB, conDB, strReqMngNo, strYokyuNichiji, strYokyuTuban, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strYokyuNichiji = objInsertDataRow(11)  '要求日付（D129_REQFSE)
                        strYokyuTuban = objInsertDataRow(12)    '要求日付（D129_REQFSE)
                        strSelect = "SELECT * FROM T_CT_REQFSH"
                        strSelect = strSelect & " WHERE REQDATE =  '" & strYokyuNichiji & "'"
                        strSelect = strSelect & "   AND REQSEQ = '" & strYokyuTuban & "';"
                        strWatashiTableName = "D130_REQFSH"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "5" Then
                        '照会要求受付ＤＢ取得
                        If mfGetD101InqreceptSql(cmdDB, conDB, strReqMngNo, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strYokyuNichiji = objInsertDataRow(80)  '要求日付（D101_INQRECEPT)
                        strYokyuTuban = objInsertDataRow(81)    '要求通番（D101_INQRECEPT)
                        '照会要求親データ取得
                        If mfGetD129Reqfse(cmdDB, conDB, strReqMngNo, strYokyuNichiji, strYokyuTuban, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtInsetTable = objSelectInsert.Tables(0)
                        objInsertDataRow = dtInsetTable.Rows(0)
                        strJyoutaiFlg = objInsertDataRow(0)     '状態フラグ（D129_REQFSE)
                        strYokyuNichiji = objInsertDataRow(11)  '要求日付（D129_REQFSE)
                        strYokyuTuban = objInsertDataRow(12)    '要求日付（D129_REQFSE)
                        '照会要求明細データ取得
                        If mfGetD130Reqfsh(cmdDB, conDB, strReqMngNo, strYokyuNichiji, strYokyuTuban, objSelectInsert, strFilePath) = False Then
                            Return False
                        End If
                        dtSqldtTable = objSelectInsert.Tables(0)

                        '取得テーブルの件数を取得
                        intSqldtCount = dtSqldtTable.Rows.Count
                        For i = 0 To intSqldtCount - 1
                            objSqlDataRow = dtSqldtTable.Rows(i)
                            strD130UnyouKanshi = objSqlDataRow(0)  '運用監視サーバ運用日付（D130_REQFSH)
                            strD130Shikibetu = objSqlDataRow(1)    '識別（D130_REQFSH)
                            strD130ShoukaiTuban = objSqlDataRow(2) '照会通番（D130_REQFSH)
                            strD130EdaBan = objSqlDataRow(3)       '枝番号（D130_REQFSH)
                            strD130Renban = objSqlDataRow(4)       '連番（D130_REQFSH)
                            strD130TboxId = objSqlDataRow(5)       'TBOXID（D130_REQFSH)
                            strD130DataSyubetu = objSqlDataRow(6)  'データ種別（D130_REQFSH)

                            'データ種別
                            Select Case strD130DataSyubetu
                                Case "25"
                                    strSelect = "SELECT * FROM T_ZS_SIYOUCHUCARD"
                                    strSelect = strSelect & " WHERE FKSDATE = " & "'" & strD130UnyouKanshi & "'"
                                    strSelect = strSelect & "   AND SVRTYPE = " & "'" & strD130Shikibetu & "'"
                                    strSelect = strSelect & "   AND SHOKAISEQ = " & "'" & strD130ShoukaiTuban & "'"
                                    strSelect = strSelect & "   AND EDASEQ = " & "'" & strD130EdaBan & "'"
                                    strSelect = strSelect & "   AND DATASEQ = " & "'" & strD130Renban & "';"
                                    strWatashiTableName = "D131_SIYOUCHUCARD"
                                Case "26"
                                    strSelect = "SELECT * FROM T_ZS_KESSAISHOUKAI"
                                    strSelect = strSelect & " WHERE FKSDATE = " & "'" & strD130UnyouKanshi & "'"
                                    strSelect = strSelect & "   AND SVRTYPE = " & "'" & strD130Shikibetu & "'"
                                    strSelect = strSelect & "   AND SHOKAISEQ = " & "'" & strD130ShoukaiTuban & "'"
                                    strSelect = strSelect & "   AND EDASEQ = " & "'" & strD130EdaBan & "'"
                                    strSelect = strSelect & "   AND DATASEQ = " & "'" & strD130Renban & "';"
                                    strWatashiTableName = "D132_KESSAISHOUKAI"
                                Case "27"
                                    strSelect = "SELECT * FROM T_ZS_SOUTIKOSEI"
                                    strSelect = strSelect & " WHERE FKSDATE = " & "'" & strD130UnyouKanshi & "'"
                                    strSelect = strSelect & "   AND SVRTYPE = " & "'" & strD130Shikibetu & "'"
                                    strSelect = strSelect & "   AND SHOKAISEQ = " & "'" & strD130ShoukaiTuban & "'"
                                    strSelect = strSelect & "   AND EDASEQ = " & "'" & strD130EdaBan & "'"
                                    strSelect = strSelect & "   AND DATASEQ = " & "'" & strD130Renban & "';"
                                    strWatashiTableName = "D133_SOUTIKOSEI"
                                Case "29"
                                    strSelect = "SELECT * FROM T_ZS_BBKIBAN"
                                    strSelect = strSelect & " WHERE FKSDATE = " & "'" & strD130UnyouKanshi & "'"
                                    strSelect = strSelect & "   AND SVRTYPE = " & "'" & strD130Shikibetu & "'"
                                    strSelect = strSelect & "   AND SHOKAISEQ = " & "'" & strD130ShoukaiTuban & "'"
                                    strSelect = strSelect & "   AND EDASEQ = " & "'" & strD130EdaBan & "'"
                                    strSelect = strSelect & "   AND DATASEQ = " & "'" & strD130Renban & "';"
                                    strWatashiTableName = "D135_BBKIBAN"
                                Case "86"
                                    strSelect = "SELECT * FROM T_ZS_BBKIBAN2"
                                    strSelect = strSelect & " WHERE FKSDATE = " & "'" & strD130UnyouKanshi & "'"
                                    strSelect = strSelect & "   AND SVRTYPE = " & "'" & strD130Shikibetu & "'"
                                    strSelect = strSelect & "   AND SHOKAISEQ = " & "'" & strD130ShoukaiTuban & "'"
                                    strSelect = strSelect & "   AND EDASEQ = " & "'" & strD130EdaBan & "'"
                                    strSelect = strSelect & "   AND DATASEQ = " & "'" & strD130Renban & "';"
                                    strWatashiTableName = "D162_BBKIBAN2"
                                Case "30"
                                    strSelect = "SELECT * FROM T_ZS_BBSOUTI"
                                    strSelect = strSelect & " WHERE FKSDATE = " & "'" & strD130UnyouKanshi & "'"
                                    strSelect = strSelect & "   AND SVRTYPE = " & "'" & strD130Shikibetu & "'"
                                    strSelect = strSelect & "   AND SHOKAISEQ = " & "'" & strD130ShoukaiTuban & "'"
                                    strSelect = strSelect & "   AND EDASEQ = " & "'" & strD130EdaBan & "'"
                                    strSelect = strSelect & "   AND DATASEQ = " & "'" & strD130Renban & "';"
                                    strWatashiTableName = "D136_BBSOUTI"
                                Case "35"
                                    strSelect = "SELECT * FROM T_ZS_TBOXSOUSALOG"
                                    strSelect = strSelect & " WHERE FKSDATE = " & "'" & strD130UnyouKanshi & "'"
                                    strSelect = strSelect & "   AND SVRTYPE = " & "'" & strD130Shikibetu & "'"
                                    strSelect = strSelect & "   AND SHOKAISEQ = " & "'" & strD130ShoukaiTuban & "'"
                                    strSelect = strSelect & "   AND EDASEQ = " & "'" & strD130EdaBan & "'"
                                    strSelect = strSelect & "   AND DATASEQ = " & "'" & strD130Renban & "';"
                                    strWatashiTableName = "D143_TBOXSOUSALOG"
                                Case "36"
                                    strSelect = "SELECT * FROM T_ZS_TBOXERRLOG"
                                    strSelect = strSelect & " WHERE FKSDATE = " & "'" & strD130UnyouKanshi & "'"
                                    strSelect = strSelect & "   AND SVRTYPE = " & "'" & strD130Shikibetu & "'"
                                    strSelect = strSelect & "   AND SHOKAISEQ = " & "'" & strD130ShoukaiTuban & "'"
                                    strSelect = strSelect & "   AND EDASEQ = " & "'" & strD130EdaBan & "'"
                                    strSelect = strSelect & "   AND DATASEQ = " & "'" & strD130Renban & "';"
                                    strWatashiTableName = "D144_TBOXERRLOG"
                                Case "39"
                                    strSelect = "SELECT * FROM T_ZS_SAIKENMEISAI"
                                    strSelect = strSelect & " WHERE FKSDATE = " & "'" & strD130UnyouKanshi & "'"
                                    strSelect = strSelect & "   AND SVRTYPE = " & "'" & strD130Shikibetu & "'"
                                    strSelect = strSelect & "   AND SHOKAISEQ = " & "'" & strD130ShoukaiTuban & "'"
                                    strSelect = strSelect & "   AND EDASEQ = " & "'" & strD130EdaBan & "'"
                                    strSelect = strSelect & "   AND DATASEQ = " & "'" & strD130Renban & "';"
                                    strWatashiTableName = "D147_SAIKENMEISAI"
                                Case "40"
                                    strSelect = "SELECT * FROM T_ZS_SAIMUMEISAI"
                                    strSelect = strSelect & " WHERE FKSDATE = " & "'" & strD130UnyouKanshi & "'"
                                    strSelect = strSelect & "   AND SVRTYPE = " & "'" & strD130Shikibetu & "'"
                                    strSelect = strSelect & "   AND SHOKAISEQ = " & "'" & strD130ShoukaiTuban & "'"
                                    strSelect = strSelect & "   AND EDASEQ = " & "'" & strD130EdaBan & "'"
                                    strSelect = strSelect & "   AND DATASEQ = " & "'" & strD130Renban & "';"
                                    strWatashiTableName = "D148_SAIMUMEISAI"
                                Case "42"
                                    strSelect = "SELECT * FROM T_ZS_SEISANMEISAI"
                                    strSelect = strSelect & " WHERE FKSDATE = " & "'" & strD130UnyouKanshi & "'"
                                    strSelect = strSelect & "   AND SVRTYPE = " & "'" & strD130Shikibetu & "'"
                                    strSelect = strSelect & "   AND SHOKAISEQ = " & "'" & strD130ShoukaiTuban & "'"
                                    strSelect = strSelect & "   AND EDASEQ = " & "'" & strD130EdaBan & "'"
                                    strSelect = strSelect & "   AND DATASEQ = " & "'" & strD130Renban & "';"
                                    strWatashiTableName = "D150_SEISANMEISAI"
                                Case "f1"
                                    strSelect = "SELECT * FROM T_ZS_SIYOUCHUCARD2"
                                    strSelect = strSelect & " WHERE FKSDATE = " & "'" & strD130UnyouKanshi & "'"
                                    strSelect = strSelect & "   AND SVRTYPE = " & "'" & strD130Shikibetu & "'"
                                    strSelect = strSelect & "   AND SHOKAISEQ = " & "'" & strD130ShoukaiTuban & "'"
                                    strSelect = strSelect & "   AND EDASEQ = " & "'" & strD130EdaBan & "'"
                                    strSelect = strSelect & "   AND DATASEQ = " & "'" & strD130Renban & "';"
                                    strWatashiTableName = "D156_SIYOUCHUCARD2"
                            End Select
                            '------------------------------------
                            '通番を発行後ファイルを作成しＦＴＰ送信を行う
                            If mfFtpSoushinKanren(cmdDB, conDB, _
                                               strReqMngNo, _
                                               strTboxId, _
                                               strProcCd, _
                                               strD130DataSyubetu, _
                                               "1", _
                                               strUrl, _
                                               strUserId, _
                                               strPassWord, _
                                               strSetHeading, _
                                               strSetFeedback, _
                                               strSetVerify, _
                                               strSetLinesize, _
                                               strExit, _
                                               strSelect, _
                                               strWatashiTableName, _
                                               strFilePath, _
                                               strSqlFilePath) = False Then
                                Return False
                            End If
                            '------------------------------------
                        Next
                    End If
                Case "501"
                    If strMoveCls = "1" Then
                        strSelect = "SELECT * FROM tt_hjt_db_tbxsj"
                        If strNLKbn = "N" Or strNLKbn = "J" Then
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'20" & strTboxId & "';"
                        Else
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'10" & strTboxId & "';"
                        End If
                        strWatashiTableName = "D108_TT_HJT_DB_TBXSJ"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "2" Then
                        strSelect = "SELECT * FROM tm_hisn_knr"
                        If strNLKbn = "N" Or strNLKbn = "J" Then
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'20" & strTboxId & "';"
                        Else
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'10" & strTboxId & "';"
                        End If
                        strSelect = strSelect & "   AND dt_shbt = " & "'50'" & ";"
                        strWatashiTableName = "D103_TM_HISN_KNR"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    End If
                Case "502"
                    If strMoveCls = "1" Then
                        strSelect = "SELECT * FROM tt_hjt_db_bbuj"
                        If strNLKbn = "N" Or strNLKbn = "J" Then
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'20" & strTboxId & "';"
                        Else
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'10" & strTboxId & "';"
                        End If
                        strWatashiTableName = "D110_TT_HJT_DB_BBUJ"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "2" Then
                        strSelect = "SELECT * FROM tm_hisn_knr"
                        If strNLKbn = "N" Or strNLKbn = "J" Then
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'20" & strTboxId & "';"
                        Else
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'10" & strTboxId & "';"
                        End If
                        strSelect = strSelect & "   AND dt_shbt = " & "'49'" & ";"
                        strWatashiTableName = "D103_TM_HISN_KNR"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    End If
                Case "503"
                    If strMoveCls = "1" Then
                        strSelect = "SELECT * FROM tt_hjt_db_bbsjknbinyknk"
                        If strNLKbn = "N" Or strNLKbn = "J" Then
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'20" & strTboxId & "';"
                        Else
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'10" & strTboxId & "';"
                        End If
                        strWatashiTableName = "D114_TT_HJT_DB_BBSJKNBINYKNK"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "2" Then
                        strSelect = "SELECT * FROM tm_hisn_knr"
                        If strNLKbn = "N" Or strNLKbn = "J" Then
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'20" & strTboxId & "';"
                        Else
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'10" & strTboxId & "';"
                        End If
                        strSelect = strSelect & "   AND dt_shbt = " & "'51'" & ";"
                        strWatashiTableName = "D103_TM_HISN_KNR"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    End If
                Case "504"
                    If strMoveCls = "1" Then
                        strSelect = "SELECT * FROM tt_hjt_db_bbsjsndsisnk"
                        If strNLKbn = "N" Or strNLKbn = "J" Then
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'20" & strTboxId & "';"
                        Else
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'10" & strTboxId & "';"
                        End If
                        strWatashiTableName = "D116_TT_HJT_DB_BBSJSNDSISNK"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "2" Then
                        strSelect = "SELECT * FROM tm_hisn_knr"
                        If strNLKbn = "N" Or strNLKbn = "J" Then
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'20" & strTboxId & "';"
                        Else
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'10" & strTboxId & "';"
                        End If
                        strSelect = strSelect & "   AND dt_shbt = " & "'52'" & ";"
                        strWatashiTableName = "D103_TM_HISN_KNR"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    End If
                Case "505"
                    If strMoveCls = "1" Then
                        strSelect = "SELECT * FROM tt_hjt_db_bbsjturkuktskk"
                        If strNLKbn = "N" Or strNLKbn = "J" Then
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'20" & strTboxId & "';"
                        Else
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'10" & strTboxId & "';"
                        End If
                        strWatashiTableName = "D118_TT_HJT_DB_BBSJTURKUKTSKK"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "2" Then
                        strSelect = "SELECT * FROM tm_hisn_knr"
                        If strNLKbn = "N" Or strNLKbn = "J" Then
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'20" & strTboxId & "';"
                        Else
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'10" & strTboxId & "';"
                        End If
                        strSelect = strSelect & "   AND dt_shbt = " & "'62'" & ";"
                        strWatashiTableName = "D103_TM_HISN_KNR"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    End If
                Case "506"
                    If strMoveCls = "1" Then
                        strSelect = "SELECT * FROM tt_hjt_db_bbujnvc"
                        If strNLKbn = "N" Or strNLKbn = "J" Then
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'20" & strTboxId & "';"
                        Else
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'10" & strTboxId & "';"
                        End If
                        strWatashiTableName = "D112_TT_HJT_DB_BBUJNVC"
                        '------------------------------------
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "2" Then
                        strSelect = "SELECT * FROM tm_hisn_knr"
                        If strNLKbn = "N" Or strNLKbn = "J" Then
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'20" & strTboxId & "'"
                        Else
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'10" & strTboxId & "'"
                        End If
                        strSelect = strSelect & "   AND dt_shbt = " & "'e6'" & ";"
                        strWatashiTableName = "D103_TM_HISN_KNR"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    End If
                Case "507"
                    If strMoveCls = "1" Then
                        strSelect = "SELECT * FROM tt_hjt_db_bbsjsndnvc"
                        If strNLKbn = "N" Or strNLKbn = "J" Then
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'20" & strTboxId & "';"
                        Else
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'10" & strTboxId & "';"
                        End If
                        strWatashiTableName = "D122_TT_HJT_DB_BBSJSNDNVC"
                        '------------------------------------
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "2" Then
                        strSelect = "SELECT * FROM tm_hisn_knr"
                        If strNLKbn = "N" Or strNLKbn = "J" Then
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'20" & strTboxId & "'"
                        Else
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'10" & strTboxId & "'"
                        End If
                        strSelect = strSelect & "   AND dt_shbt = " & "'e8'" & ";"
                        strWatashiTableName = "D103_TM_HISN_KNR"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    End If
                Case "508"
                    If strMoveCls = "1" Then
                        strSelect = "SELECT * FROM tt_hjt_db_bbsjsisnknvc"
                        If strNLKbn = "N" Or strNLKbn = "J" Then
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'20" & strTboxId & "';"
                        Else
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'10" & strTboxId & "';"
                        End If
                        strWatashiTableName = "D120_TT_HJT_DB_BBSJSISNKNVC"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    ElseIf strMoveCls = "2" Then
                        strSelect = "SELECT * FROM tm_hisn_knr"
                        If strNLKbn = "N" Or strNLKbn = "J" Then
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'20" & strTboxId & "'"
                        Else
                            strSelect = strSelect & " WHERE kkchutbxid = " & "'10" & strTboxId & "'"
                        End If
                        strSelect = strSelect & "   AND dt_shbt = " & "'e7'" & ";"
                        strWatashiTableName = "D103_TM_HISN_KNR"
                        '------------------------------------
                        '通番を発行後ファイルを作成しＦＴＰ送信を行う
                        If mfFtpSoushinKanren(cmdDB, conDB, _
                                           strReqMngNo, _
                                           strTboxId, _
                                           strProcCd, _
                                           strD130DataSyubetu, _
                                           "1", _
                                           strUrl, _
                                           strUserId, _
                                           strPassWord, _
                                           strSetHeading, _
                                           strSetFeedback, _
                                           strSetVerify, _
                                           strSetLinesize, _
                                           strExit, _
                                           strSelect, _
                                           strWatashiTableName, _
                                           strFilePath, _
                                           strSqlFilePath) = False Then
                            Return False
                        End If
                        '------------------------------------
                    End If
            End Select
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 通番を発行しファイルを作成後にＦＴＰ送信を行う
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="strReqMngNo"></param>
    ''' <param name="strTboxId"></param>
    ''' <param name="strProcCd"></param>
    ''' <param name="strD130DataSyubetu"></param>
    ''' <param name="strJyouhou_SyoukaiKubun"></param>
    ''' <param name="strUrl"></param>
    ''' <param name="strUserId"></param>
    ''' <param name="strPassWord"></param>
    ''' <param name="strSetHeading"></param>
    ''' <param name="strSetFeedback"></param>
    ''' <param name="strSetVerify"></param>
    ''' <param name="strSetLinesize"></param>
    ''' <param name="strExit"></param>
    ''' <param name="strSqlCode"></param>
    ''' <param name="strWatashiTableName"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strSqlFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfFtpSoushinKanren(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                        ByVal strReqMngNo As String, _
                                        ByVal strTboxId As String, _
                                        ByVal strProcCd As String, _
                                        ByVal strD130DataSyubetu As String, _
                                        ByVal strJyouhou_SyoukaiKubun As String, _
                                        ByVal strUrl As String, _
                                        ByVal strUserId As String, _
                                        ByVal strPassWord As String, _
                                        ByVal strSetHeading As String, _
                                        ByVal strSetFeedback As String, _
                                        ByVal strSetVerify As String, _
                                        ByVal strSetLinesize As String, _
                                        ByVal strExit As String, _
                                        ByVal strSqlCode As String, _
                                        ByVal strWatashiTableName As String, _
                                        ByVal strFilePath As String, _
                                        ByVal strSqlFilePath As String) As Boolean
        Dim objdtGetTuban As DataSet = Nothing
        Dim dtGetTuban As DataTable
        Dim objGetTubanDataRow As DataRow
        Dim strFileTuban As String = ""

        Dim strSoushinFileName As String = ""
        Dim strSoushinFileNameFinal As String = ""
        Try
            '------------------------------------
            '通番取得
            If mfGetTuban(cmdDB, conDB, objdtGetTuban, strFilePath) = False Then
                'エラー時
                Return False
            End If
            dtGetTuban = objdtGetTuban.Tables(0)
            objGetTubanDataRow = dtGetTuban.Rows(0)
            'ファイル通番（D182_EXTRACT_ISSUENUM)
            If objGetTubanDataRow(0) Is DBNull.Value Then
                strFileTuban = "000001"
            Else
                strFileTuban = objGetTubanDataRow(0)
                strFileTuban = (Integer.Parse(strFileTuban) + 1).ToString("D6")
            End If
            '情報抽出／照会要求ファイル発番登録
            If mfSetD182ExtractIssuenum(cmdDB, conDB, strReqMngNo, strFileTuban, strTboxId, _
                                        strD130DataSyubetu, strWatashiTableName, strJyouhou_SyoukaiKubun, _
                                        strSoushinFileName, strSoushinFileNameFinal, strFilePath) = False Then
                'エラー時
                Return False
            End If
            'ファイル作成
            If mfMakeFile(strSetHeading, strSetFeedback, strSetVerify, strSetLinesize, _
                          strExit, strSqlCode, strSoushinFileName, _
                          strFilePath, strSqlFilePath) = False Then
                'エラー時
                Return False
            End If

            'ＦＴＰ送信
            If mfFtpPutSqlFile(strUrl, strUserId, strPassWord, strProcCd, strSqlCode, strFilePath, strSqlFilePath) = False Then
                'FTP送信でエラーのため異常終了
                Return False
            End If

            'SQLファイル削除処理
            If mfDelSqlFile(strFilePath, strSqlFilePath) = False Then
                'SQLファイル削除でエラーのため異常終了
                Return False
            End If

            'FINALファイル作成
            If mfMakeFileFinal(strSoushinFileNameFinal, strFilePath, strSqlFilePath) = False Then
                'エラー時
                Return False
            End If

            'ＦＴＰ送信
            If mfFtpPutSqlFile(strUrl, strUserId, strPassWord, strProcCd, "", strFilePath, strSqlFilePath) = False Then
                'FTP送信でエラーのため異常終了
                Return False
            End If

            'SQLファイル削除処理
            If mfDelSqlFile(strFilePath, strSqlFilePath) = False Then
                'SQLファイル削除でエラーのため異常終了
                Return False
            End If
            '------------------------------------
            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfFtpSoushinKanren"
            strTable = ""
            strData = ""
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function
    ''' <summary>
    ''' ファイル通番取得
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="objdtGetTuban"></param>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetTuban(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                ByRef objdtGetTuban As DataSet, ByVal strFilePath As String) As Boolean
        Try
            Using trans1 As SqlTransaction = conDB.BeginTransaction
                'プロシージャ設定
                Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")
                cmdDB = New SqlCommand("SMTSSF001_S8", conDB)

                cmdDB.Parameters.Add(pfSet_Param("prmD182_REQ_DT", SqlDbType.NVarChar, strToujituhiduke))
                cmdDB.Parameters.Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                'ストアドプロシージャで実行
                cmdDB.CommandType = CommandType.StoredProcedure

                'トランザクション
                cmdDB.Transaction = trans1

                '照会要求データ取得処理
                objdtGetTuban = clsDataConnect.pfGet_DataSet(cmdDB)
                Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

                If (Not objdtGetTuban Is Nothing) And objdtGetTuban.Tables.Count > 0 And strReturn <> "0" Then
                    '該当データありの場合は既にDataSetに格納されているため何もしない。
                Else
                    '取得データなし
                    Return False
                End If
            End Using
            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetTuban"
            strTable = "D182_EXTRACT_ISSUENUM"
            strData = "SMTSSF001_S8"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 情報抽出／照会要求ファイル発番登録
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="strReqMngNo"></param>
    ''' <param name="strFileTuban"></param>
    ''' <param name="strTboxId"></param>
    ''' <param name="strD130DataSyubetu"></param>
    ''' <param name="strTblName"></param>
    ''' <param name="strFileKbn"></param>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfSetD182ExtractIssuenum(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                              ByVal strReqMngNo As String, ByVal strFileTuban As String, _
                                              ByVal strTboxId As String, ByVal strD130DataSyubetu As String, _
                                              ByVal strTblName As String, ByVal strFileKbn As String, _
                                              ByRef strSoushinFileName As String, ByRef strSoushinFileNameFinal As String, _
                                              ByVal strFilePath As String) As Boolean
        Try
            Dim strFileName As String
            Dim strFileNamefinal As String
            Dim strKekaFileName As String
            Dim strKekaFileNameFinal As String

            Using trans1 As SqlTransaction = conDB.BeginTransaction
                Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")

                If strFileKbn = "1" Then
                    '情報抽出
                    strFileName = strSQLFILECHUSHUTU & strToujituhiduke & "_" & strFileTuban & strSQLFILENAMEKAKUCHOSHI
                    strFileNamefinal = strSQLFILECHUSHUTU & strToujituhiduke & "_" & strFileTuban & strSQLFINAL
                    strKekaFileName = strSQLFILECHUSHUTUKEKA & strToujituhiduke & "_" & strFileTuban & strSQLKEKAFILENAMEKAKUCHOSHI
                    strKekaFileNameFinal = strSQLFILECHUSHUTUKEKA & strToujituhiduke & "_" & strFileTuban & strSQLFINAL
                Else
                    '照会要求
                    strFileName = strSQLFILEYOUKYU & strToujituhiduke & "_" & strFileTuban & strSQLFILENAMEKAKUCHOSHI
                    strFileNamefinal = strSQLFILEYOUKYU & strToujituhiduke & "_" & strFileTuban & strSQLFINAL
                    strKekaFileName = strSQLFILEYOUKYUKEKA & strToujituhiduke & "_" & strFileTuban & strSQLKEKAFILENAMEKAKUCHOSHI
                    strKekaFileNameFinal = strSQLFILEYOUKYUKEKA & strToujituhiduke & "_" & strFileTuban & strSQLFINAL
                End If
                'プロシージャ設定
                cmdDB = New SqlCommand("SMTSSF001_I1", conDB)

                With cmdDB.Parameters
                    .Add(pfSet_Param("prmD182_CTRL_NO", SqlDbType.NVarChar, strReqMngNo))
                    .Add(pfSet_Param("prmD182_REQ_DT", SqlDbType.NVarChar, strToujituhiduke))
                    .Add(pfSet_Param("prmD182_FILE_SEQ", SqlDbType.NVarChar, strFileTuban))
                    .Add(pfSet_Param("prmD182_TBOXID", SqlDbType.NVarChar, strTboxId))
                    .Add(pfSet_Param("prmD182_DATA_CLS", SqlDbType.NVarChar, strD130DataSyubetu))
                    .Add(pfSet_Param("prmD182_REQ_FILE_NM", SqlDbType.NVarChar, strFileName))
                    .Add(pfSet_Param("prmD182_REQ_CMPFILE_NM", SqlDbType.NVarChar, strFileNamefinal))
                    .Add(pfSet_Param("prmD182_RSLT_FILE_NM", SqlDbType.NVarChar, strKekaFileName))
                    .Add(pfSet_Param("prmD182_RSLT_CMPFILE_NM", SqlDbType.NVarChar, strKekaFileNameFinal))
                    .Add(pfSet_Param("prmD182_TABLE_NM", SqlDbType.NVarChar, strTblName))
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
                    strTable = "D182_EXTRACT_ISSUENUM"
                    strData = "SMTSSF001_I1"
                    Throw New Exception("ストアドプロシージャ：" & intReturn)
                Else
                    'コミッテッド
                    trans1.Commit()
                End If

            End Using
            strSoushinFileName = strFileName
            strSoushinFileNameFinal = strFileNamefinal
            Return True
        Catch ex As Exception
            strMesod = "mfSetD182ExtractIssuenum"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' ファイル作成
    ''' </summary>
    ''' <param name="strSetHeading"></param>
    ''' <param name="strSetFeedback"></param>
    ''' <param name="strSetVerify"></param>
    ''' <param name="strSetLinesize"></param>
    ''' <param name="strExit"></param>
    ''' <param name="strSqlDml"></param>
    ''' <param name="strSoushinFileName"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strSqlFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfMakeFile(ByVal strSetHeading As String, ByVal strSetFeedback As String, _
                                ByVal strSetVerify As String, ByVal strSetLinesize As String, _
                                ByVal strExit As String, ByVal strSqlDml As String, _
                                ByVal strSoushinFileName As String, ByVal strFilePath As String, _
                                ByVal strSqlFilePath As String) As Boolean
        Try
            Dim sw As New System.IO.StreamWriter(strSqlFilePath & "\" & strSoushinFileName, _
                                                 False, System.Text.Encoding.GetEncoding("shift_jis"))
            sw.WriteLine(strSetHeading)
            sw.WriteLine(strSetFeedback)
            sw.WriteLine(strSetVerify)
            sw.WriteLine(strSetLinesize)
            sw.WriteLine(strSqlDml)
            sw.WriteLine(strExit)
            '閉じる
            sw.Close()

            Return True

        Catch ex As Exception
            strErrBunrui = "ファイル作成エラー"
            strMesod = "mfMakeFile"
            strTable = ""
            strData = ""
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' FINALファイル作成
    ''' </summary>
    ''' <param name="strSoushinFileNameFinal"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strSqlFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfMakeFileFinal(ByVal strSoushinFileNameFinal As String, _
                                     ByVal strFilePath As String, _
                                     ByVal strSqlFilePath As String) As Boolean
        Try
            Dim sw As New System.IO.StreamWriter(strSqlFilePath & "\" & strSoushinFileNameFinal, _
                                                 False, System.Text.Encoding.GetEncoding("shift_jis"))
            '閉じる
            sw.Close()

            Return True

        Catch ex As Exception
            strErrBunrui = "ファイル作成エラー"
            strMesod = "mfMakeFileFinal"
            strTable = ""
            strData = ""
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try


    End Function

    ''' <summary>
    ''' 照会要求受付DB取得
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="strReqMngNo"></param>
    ''' <param name="objSelectInsert"></param>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetD101InqreceptSql(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                 ByVal strReqMngNo As String, ByRef objSelectInsert As DataSet, _
                                 ByVal strFilePath As String) As Boolean
        Try
            Using trans1 As SqlTransaction = conDB.BeginTransaction
                'プロシージャ設定
                cmdDB = New SqlCommand("SMTSSF001_S4", conDB)

                cmdDB.Parameters.Add(pfSet_Param("prmD101_CTRL_NO", SqlDbType.NVarChar, strReqMngNo))
                cmdDB.Parameters.Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                'ストアドプロシージャで実行
                cmdDB.CommandType = CommandType.StoredProcedure

                'トランザクション
                cmdDB.Transaction = trans1

                '照会要求データ取得処理
                objSelectInsert = clsDataConnect.pfGet_DataSet(cmdDB)
                Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

                If (Not objSelectInsert Is Nothing) And objSelectInsert.Tables.Count > 0 And strReturn <> "0" Then
                    '該当データありの場合は既にDataSetに格納されているため何もしない。
                Else
                    '取得データなし
                    Return False
                End If
            End Using
            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetD101InqreceptSql"
            strTable = "D101_INQRECEPT"
            strData = "SMTSSF001_S4"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 照会要求親データ取得
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="strReqMngNo"></param>
    ''' <param name="strYokyuNichiji"></param>
    ''' <param name="strYokyuTuban"></param>
    ''' <param name="objSelectInsert"></param>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetD129Reqfse(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                     ByVal strReqMngNo As String, ByVal strYokyuNichiji As String, _
                                     ByVal strYokyuTuban As String, ByRef objSelectInsert As DataSet, _
                                     ByVal strFilePath As String) As Boolean
        Try
            Using trans1 As SqlTransaction = conDB.BeginTransaction
                'プロシージャ設定
                cmdDB = New SqlCommand("SMTSSF001_S5", conDB)

                cmdDB.Parameters.Add(pfSet_Param("prmD129_CTRL_NO", SqlDbType.NVarChar, strReqMngNo))
                cmdDB.Parameters.Add(pfSet_Param("prmD129_REQDATE", SqlDbType.Char, strYokyuNichiji))
                cmdDB.Parameters.Add(pfSet_Param("prmD129_REQSEQ", SqlDbType.Char, strYokyuTuban))
                cmdDB.Parameters.Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                'ストアドプロシージャで実行
                cmdDB.CommandType = CommandType.StoredProcedure

                'トランザクション
                cmdDB.Transaction = trans1

                '照会要求データ取得処理
                objSelectInsert = clsDataConnect.pfGet_DataSet(cmdDB)
                Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

                If (Not objSelectInsert Is Nothing) And objSelectInsert.Tables.Count > 0 And strReturn <> "0" Then
                    '該当データありの場合は既にDataSetに格納されているため何もしない。
                Else
                    '取得データなし
                    Return False
                End If
            End Using
            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetD129Reqfse"
            strTable = "D129_REQFSE"
            strData = "SMTSSF001_S5"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 照会要求明細データ取得
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="strReqMngNo"></param>
    ''' <param name="strYokyuNichiji"></param>
    ''' <param name="strYokyuTuban"></param>
    ''' <param name="objSelectInsert"></param>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetD130Reqfsh(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                     ByVal strReqMngNo As String, ByVal strYokyuNichiji As String, _
                                     ByVal strYokyuTuban As String, ByRef objSelectInsert As DataSet, _
                                     ByVal strFilePath As String) As Boolean
        Try
            Using trans1 As SqlTransaction = conDB.BeginTransaction
                'プロシージャ設定
                cmdDB = New SqlCommand("SMTSSF001_S6", conDB)

                cmdDB.Parameters.Add(pfSet_Param("prmD130_CTRL_NO", SqlDbType.NVarChar, strReqMngNo))
                cmdDB.Parameters.Add(pfSet_Param("prmD130_REQDATE", SqlDbType.Char, strYokyuNichiji))
                cmdDB.Parameters.Add(pfSet_Param("prmD130_REQSEQ", SqlDbType.Char, strYokyuTuban))
                cmdDB.Parameters.Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                'ストアドプロシージャで実行
                cmdDB.CommandType = CommandType.StoredProcedure

                'トランザクション
                cmdDB.Transaction = trans1

                '照会要求データ取得処理
                objSelectInsert = clsDataConnect.pfGet_DataSet(cmdDB)
                Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

                If (Not objSelectInsert Is Nothing) And objSelectInsert.Tables.Count > 0 And strReturn <> "0" Then
                    '該当データありの場合は既にDataSetに格納されているため何もしない。
                Else
                    '取得データなし
                    Return False
                End If
            End Using
            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetD130Reqfsh"
            strTable = "D130_REQFSH"
            strData = "SMTSSF001_S6"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' ＦＴＰ送信
    ''' </summary>
    ''' <param name="strFilePath"></param>
    ''' <param name="strSqlFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfFtpPutSqlFile(ByVal strUrl As String, ByVal strUserId As String, ByVal strPassWord As String, _
                                     ByVal strProcCd As String, ByVal strSql As String, ByVal strFilePath As String, ByVal strSqlFilePath As String) As Boolean
        Dim webClient As New System.Net.WebClient()
        Try
            '作成したSQLファイルをすべて取得する
            Dim strFullPathSqlFileNms() As String = System.IO.Directory.GetFiles(strSqlFilePath)

            Dim strFileName As String = ""

            'ログインユーザー名とパスワードを指定
            webClient.Credentials = New System.Net.NetworkCredential(strUserId, strPassWord)
            'FTPサーバーにアップロード
            For i As Integer = 0 To strFullPathSqlFileNms.Length - 1
                'ファイル名称を取得
                strFileName = System.IO.Path.GetFileName(strFullPathSqlFileNms(i))
                'ファイルアップロード
                webClient.UploadFile(strUrl & strFileName, strFullPathSqlFileNms(i))

                'FTP送信ログ出力
                msSeijyouLog(strProcCd, strFileName, strSql, strFilePath)
            Next

            Return True

        Catch ex As WebException
            strErrBunrui = "ＦＴＰ接続エラー"
            strMesod = "mfFtpPutSqlFile"
            strTable = ""
            strData = ""
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfFtpPutSqlFile"
            strTable = ""
            strData = ""
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        Finally
            '解放する
            webClient.Dispose()
        End Try

    End Function

    ''' <summary>
    ''' 全ファイル削除
    ''' </summary>
    ''' <param name="strFilePath"></param>
    ''' <param name="strSqlFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfDelSqlFile(ByVal strFilePath As String, ByVal strSqlFilePath As String) As Boolean
        Try
            '作成したSQLファイルをすべて取得する
            Dim strFullPathSqlFileNms() As String = System.IO.Directory.GetFiles(strSqlFilePath)

            For i = 0 To strFullPathSqlFileNms.Length - 1
                '作成したSQLファイルをすべて削除する
                System.IO.File.Delete(strFullPathSqlFileNms(i))
            Next

            Return True

        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfDelSqlFile"
            strTable = ""
            strData = ""
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 照会要求データ更新処理
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="dtmYoukyu"></param>
    ''' <param name="strTuban"></param>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfUpdateReferenceData(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                           ByVal dtmYoukyu As DateTime, ByVal strTuban As String, _
                                           ByVal strMoveCls As String, ByVal strFilePath As String) As Boolean
        Try

            'エラーメッセージ項目設定
            strErrBunrui = "その他処理エラー"
            strTable = ""
            strData = ""

            Using trans1 As SqlTransaction = conDB.BeginTransaction
                'プロシージャ設定
                cmdDB = New SqlCommand("SMTSSF001_U1", conDB)

                'ストアドプロシージャで実行
                cmdDB.CommandType = CommandType.StoredProcedure

                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("prmD83_REQ_DT", SqlDbType.DateTime, dtmYoukyu))
                    .Add(pfSet_Param("prmD83_REQ_SEQ", SqlDbType.NVarChar, strTuban))
                    .Add(pfSet_Param("prmD83_MOVE_CLS", SqlDbType.NVarChar, strMoveCls))
                    .Add(pfSet_Param("prmD83_FTP_SND", SqlDbType.NVarChar, strSEND))
                    .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
                End With

                cmdDB.Transaction = trans1
                '実行
                cmdDB.ExecuteNonQuery()
                Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
                If intReturn <> 0 Then
                    'エラーメッセージ項目設定
                    trans1.Rollback()
                    strErrBunrui = "ＳＱＬ実行"
                    strTable = "D83_PRESERVE_PLACE"
                    strData = "SMTSSF001_U1"
                    Throw New Exception("ストアドプロシージャ：" & intReturn)
                Else
                    'コミッテッド
                    trans1.Commit()
                End If

            End Using

            Return True
        Catch ex As Exception
            strMesod = "mfUpdateReferenceData"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' エラーログ出力
    ''' </summary>
    ''' <param name="strErrBunrui"></param>
    ''' <param name="strMesod"></param>
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
    ''' <param name="strProcCd"></param>
    ''' <param name="strSoushinFileName"></param>
    ''' <param name="strSql"></param>
    ''' <param name="strFilePath"></param>
    ''' <remarks></remarks>
    Private Sub msSeijyouLog(ByVal strProcCd As String, ByVal strSoushinFileName As String, ByVal strSql As String, _
                             ByVal strFilePath As String)
        Dim writer As System.IO.StreamWriter
        Dim dteSysDate As DateTime = DateTime.Now()
        Dim strSyoriCode As String = ""
        Dim strLog As String = ""

        Select Case strProcCd
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
             " " & strSyoriCode & " " & strSoushinFileName & " " & strSql

        writer = New System.IO.StreamWriter(strFilePath & "\" & strLOGFILE & dteSysDate.ToString("yyyyMMdd") & strEXT_LOG, _
                                            True, System.Text.Encoding.Default)
        writer.WriteLine(strLog)        'ログ出力

        writer.Close()

    End Sub

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
