'********************************************************************************************************************************
'*　システム　：　サポートセンタステム
'*　処理名　　：　アップロード・ダウンロード制御　ログ出力
'*　ＰＧＭＩＤ：　ClsLogwrite
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.06.17　：　伯野
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.IO
Imports System.Text

Public Class clsLogwrite
'================================================================================================================================
'=　定数定義
'================================================================================================================================

'================================================================================================================================
'=　変数定義
'================================================================================================================================

'================================================================================================================================
'=　プロパティ定義
'================================================================================================================================

'================================================================================================================================
'=　イベントプロシージャ定義
'================================================================================================================================

'================================================================================================================================
'=　プロシージャ定義
'================================================================================================================================

    '--------------------------------------------------------------------------------------------------------------
    '-　実行ログ出力
    '--------------------------------------------------------------------------------------------------------------
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
    Public Shared Sub psLogWrite(Optional ByVal strSessionID As String = "",
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

End Class
