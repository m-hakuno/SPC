'********************************************************************************************************************************
'*　システム　：　サポートセンタステム
'*　処理名　　：　データベース接続クラス
'*　ＰＧＭＩＤ：　ClsCMDataConnect
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.10.21　：　土岐
'********************************************************************************************************************************

'============================================================================================================================
'=　インポート定義
'============================================================================================================================
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO

Public Class ClsCMDBCom

'============================================================================================================================
'=　変数定義
'============================================================================================================================
    '--------------------------------
    '2014/04/14 星野　ここから
    '--------------------------------
    Dim objStackFrame As StackFrame
'    Dim USER_NAME As String
    '--------------------------------
    '2014/04/14 星野　ここまで
    '--------------------------------
    Dim clsDBDC As New ClsCMDataConnect


'============================================================================================================================
'=　イベントプロシージャ定義
'============================================================================================================================


'============================================================================================================================
'=　プロシージャ定義
'============================================================================================================================

    ''' <summary>
    ''' データベースから画面名を取得する。
    ''' </summary>
    ''' <param name="ipstrDispCD">画面ＩＤ(プログラムＩＤ)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function pfGet_DispNm(ByVal ipstrDispCD As String) As String


        Dim conDB As SqlClient.SqlConnection
        Dim cmdDB As SqlClient.SqlCommand
        Dim dstOrders As New DataSet
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '初期化
        conDB = Nothing
        pfGet_DispNm = ""

        '接続
        If clsDBDC.pfOpen_Database(conDB) Then

            Try
                cmdDB = New SqlClient.SqlCommand("ZCMPSEL001", conDB)
                With cmdDB
                    'コマンドタイプ設定(ストアド)
                    .CommandType = CommandType.StoredProcedure
                    'パラメータ設定
                    .Parameters.Add(pfSet_Param("dispcd", SqlDbType.VarChar, ipstrDispCD))
                End With

                'ユーザーマスタからデータ取得
                dstOrders = clsDBDC.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count > 0 Then
                    pfGet_DispNm = dstOrders.Tables(0).Rows(0).Item(0).ToString()
                End If

            Catch ex As Exception
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
                                objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                pfGet_DispNm = ""
            Finally
                'DB切断
                clsDBDC.pfClose_Database(conDB)
            End Try
        Else
            pfGet_DispNm = ""
        End If

    End Function

    ''' <summary>
    ''' インプットパラメータを設定する。
    ''' </summary>
    ''' <param name="ipstrName">パラメータ名</param>
    ''' <param name="ipsdtDBType">DBパラメータタイプ</param>
    ''' <param name="ipobjVal">パラメータの値</param>
    ''' <returns>設定したパラメータ</returns>
    ''' <remarks></remarks>
    Private Function pfSet_Param(ByVal ipstrName As String,
                                       ByVal ipsdtDBType As SqlDbType,
                                       ByVal ipobjVal As Object) As SqlParameter
        Dim prmRtn As New SqlParameter(ipstrName, ipsdtDBType)
        prmRtn.Direction = ParameterDirection.Input
        prmRtn.Value = ipobjVal

        pfSet_Param = prmRtn
    End Function

    ''' <summary>
    ''' パラメータを設定する。
    ''' </summary>
    ''' <param name="ipstrName">パラメータ名</param>
    ''' <param name="ipsdtDBType">DBパラメータタイプ</param>
    ''' <param name="ipintSize">パラメータサイズ</param>
    ''' <param name="ippdnType">パラメータタイプ</param>
    ''' <returns>設定したパラメータ</returns>
    ''' <remarks></remarks>
    Private Function pfSet_Param(ByVal ipstrName As String,
                                       ByVal ipsdtDBType As SqlDbType,
                                       ByVal ipintSize As Integer,
                                       ByVal ippdnType As ParameterDirection) As SqlParameter
        Dim prmRtn As New SqlParameter(ipstrName, ipsdtDBType)
        prmRtn.Direction = ippdnType
        prmRtn.Size = ipintSize

        pfSet_Param = prmRtn
    End Function

    ''' <summary>
    ''' 戻り値のパラメータを設定する。
    ''' </summary>
    ''' <param name="ipstrName">パラメータ名</param>
    ''' <param name="ipsdtType">DBパラメータタイプ</param>
    ''' <returns>設定したパラメータ</returns>
    ''' <remarks></remarks>
    Private Function pfSet_Param(ByVal ipstrName As String,
                                       ByVal ipsdtType As SqlDbType) As SqlParameter
        Dim prmRtn As New SqlParameter(ipstrName, ipsdtType)
        prmRtn.Direction = ParameterDirection.ReturnValue

        pfSet_Param = prmRtn
    End Function

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
    Private Sub psLogWrite(Optional ByVal strSessionID As String = "",
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

    ''' <summary>
    ''' 個別の日付ボックスの入力値をチェックする。
    ''' </summary>
    ''' <param name="ipstrData">チェックする文字列</param>
    ''' <param name="ipblnReqFieldVal">必須チェックの要不要</param>
    ''' <param name="ipdafDateFormat">日付の形式</param>
    ''' <returns>検証メッセージNo.(正常の場合は空白)</returns>
    ''' <remarks></remarks>
    Public Function pfCheck_DateErr(ByVal ipstrData As String,
                                           ByVal ipblnReqFieldVal As Boolean,
                                           ByVal ipdafDateFormat As Object) As String
'                                           ByVal ipdafDateFormat As  ClsComVer.E_日付形式) As String
        Dim strData As String   'チェックする日付
        Dim dteVal As Date = Nothing
        Dim strMinDt As String = "1753/01/01"
        Dim strMaxDt As String = "9999/12/31"

        pfCheck_DateErr = String.Empty

        '未入力チェック
        If ipblnReqFieldVal Then
            If ipstrData = String.Empty Then
                pfCheck_DateErr = "5001"
                Exit Function
            End If
        End If

        '入力がある項目のみチェックする
        If ipstrData <> String.Empty Then
            '日付チェック
            Select Case ipdafDateFormat
                Case  ClsComVer.E_日付形式.年月
                    '年月の場合、日付チェックを行うため"/01"を加えてチェックを行う
                    strData = ipstrData & "/01"
                Case Else
                    strData = ipstrData
            End Select
            If DateTime.TryParse(strData, Nothing) = False Or strData.Length <> 10 Then
                pfCheck_DateErr = "4001"
                Exit Function
            End If

            '境界値チェック
            If Date.TryParse(strData, dteVal) Then
                If DateTime.Parse(strMinDt) > dteVal And _
                    dteVal < DateTime.Parse(strMaxDt) Then
                    pfCheck_DateErr = "6002"
                    Exit Function
                End If
            End If

        End If

    End Function


End Class
