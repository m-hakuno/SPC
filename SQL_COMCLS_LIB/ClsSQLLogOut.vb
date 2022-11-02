'********************************************************************************************************************************
'*  システム　：　MOP基幹システム（修理）
'*　処理名　　：　ログ出力クラス
'*  ＰＧＭＩＤ：　ClsSQLLogOut
'* 
'*                                                                                                 CopyRight SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  更　新　　：　11.07.11 09：00　湧川
'********************************************************************************************************************************
'<System.Diagnostics.DebuggerStepThrough()> _


Public Class ClsSQLLogOut


    'Sharedは、WEBアプリで誤作動を起こす可能性があるのでSessionに変更する
    'Public Shared blnLogEnabled As Boolean = True
    Dim _blnLogEnable As Boolean

    '========================================================================================
    '=コンストラクタ、ログ出力初期値の設定
    '========================================================================================
    'Sub New()
    '    Dim clsPage As New System.Web.UI.Page
    '    LogEnable = clsPage.Session("Config")
    'End Sub

    '========================================================================================================
    '=　実行ログを書き込む        　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　=
    '========================================================================================================
    ''' <summary>
    ''' 実行ログ出力
    ''' </summary>
    ''' <param name="ipstrLogContent">内容</param>
    ''' <param name="ipstrLogNo">画面ログ出力連番</param>
    ''' <param name="ipstrID">PGID</param>
    ''' <param name="ipUser">ログインユーザー名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function wrtLog(ByVal ipstrLogContent As String, ByVal ipstrLogNo As String, ByVal ipstrID As String, ByVal ipUser As String) As Boolean

        If LogEnable = False Then
        End If

        'Return True
        Dim strlogFilePATH As String = Application.StartupPath           '日付
        Dim strLogDirectory As String = Date.Now.ToString("yyyMM")
        'フォルダの存在確認
        Try
            'なければ作成
            If IO.Directory.Exists(strlogFilePATH + "\LOG") = False Then IO.Directory.CreateDirectory(strlogFilePATH & "\LOG")
            If IO.Directory.Exists(strlogFilePATH + "\LOG\" & strLogDirectory) = False Then IO.Directory.CreateDirectory(strlogFilePATH & "\LOG\" & strLogDirectory)
        Catch ex As Exception
            '作成失敗
            'MessageBox.Show(pstrlogFilePATH & vbCrLf & "ログフォルダの作成に失敗しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, _
            '                Nothing, MessageBoxOptions.DefaultDesktopOnly)
            Return False

        End Try

        Try
            'Winサービスからログ出力する場合、ログファイル末尾に任意の接尾辞追記。また、電源監視はマルチスレッド処理でおこなうため、任意のプロセスからの読み書きを許可する(2007/08/22 武田追加)
            'If My.Application.Info.AssemblyName = pstrPwrSvc Then
            '    FileOpen(intFileNo, pstrlogFilePATH + "\LOG\" & strLogDirectory & "\" + Format(logTime, "yyyMMdd") + "_P.log", OpenMode.Append, OpenAccess.Default, OpenShare.Shared)
            'ElseIf My.Application.Info.AssemblyName = pstrWngSvc Then
            '    FileOpen(intFileNo, pstrlogFilePATH + "\LOG\" & strLogDirectory & "\" + Format(logTime, "yyyMMdd") + "_W.log", OpenMode.Append)
            'ElseIf ipstrID <> "" Then
            '    FileOpen(intFileNo, pstrlogFilePATH + "\LOG\" & strLogDirectory & "\" + Format(logTime, "yyyMMdd") + "_" + ipstrID + ".log", OpenMode.Append)
            'Else
            '    FileOpen(intFileNo, pstrlogFilePATH + "\LOG\" & strLogDirectory & "\" + Format(logTime, "yyyMMdd") + ".log", OpenMode.Append)
            'End If

            '生成 追加by湧川
            Dim strLog As String = ipstrID & "：" & ipUser & " [" & ipstrLogNo & "]" & "：" & ipstrLogContent

            '日付込みのフォーマットでログを書き込む
            Dim strOutTime As String = Date.Now.ToString("yyyy/MM/dd HH:mm:ss : ")
            My.Computer.FileSystem.WriteAllText(strlogFilePATH + "\LOG\" & strLogDirectory & "\" + Format(Date.Now, "yyyMMdd") + ".log", strOutTime & strLog & vbCrLf, True)
            Return True
        Catch
            'ログファイルすら作れなかった場合の重症時
            MessageBox.Show(strlogFilePATH & vbCrLf & "ログの書込みに失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, _
                            Nothing, MessageBoxOptions.DefaultDesktopOnly)
            Return False

        End Try
    End Function

    '========================================================================================
    '=ログ出力をするＯｒしないプロパティ
    '========================================================================================
    Public Property LogEnable() As Boolean
        Get
            Return _blnLogEnable
        End Get
        Set(ByVal value As Boolean)
            _blnLogEnable = value
        End Set
    End Property

End Class

