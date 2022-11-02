'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　データ削除
'*　ＰＧＭＩＤ：　SMTDLE001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　14.02.06　：　(NKC)浜本
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
'Imports SPC.Global_asax
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports System.Xml

#End Region

' この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://localhost:49741/SystemMaintenance/SMTDLE001/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SMTDLE001
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
    ''' データ削除
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function pfTask() As Boolean

        '削除DB情報テーブル取得
        Dim objDDBTbl As DataTable = mfGetDeleteDBDatatbl()
        If objDDBTbl Is Nothing Then
            Return False
        End If

        'テーブルデータ削除処理
        If mfDeleteDBTblData(objDDBTbl) = False Then
            Return False
        End If

        '削除ファイルデータ取得
        Dim objDFileTbl As DataTable = mfGetDeleteFileDatatbl()
        If objDFileTbl Is Nothing Then
            '取得エラー
            Return False
        End If

        'ファイル削除処理
        If mfDeleteFile(objDFileTbl) = False Then
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' 削除DB情報取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDeleteDBDatatbl() As DataTable
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        mfGetDeleteDBDatatbl = Nothing

        Try
            'DB接続
            If clsDataConnect.pfOpen_Database(objCon) = False Then
                Throw New Exception
            End If

            'ストアド設定
            objCmd = New SqlCommand("SMTDLE001_S2")
            objCmd.Connection = objCon
            objCmd.CommandType = CommandType.StoredProcedure

            'SQL実行
            Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

            '取得エラー
            If objDs Is Nothing OrElse objDs.Tables.Count = 0 Then
                Throw New Exception
            End If

            mfGetDeleteDBDatatbl = objDs.Tables(0)

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
            mfGetDeleteDBDatatbl = Nothing
        Finally
            'DB切断
            If clsDataConnect.pfClose_Database(objCon) = False Then
                mfGetDeleteDBDatatbl = Nothing
            End If
        End Try
    End Function

    ''' <summary>
    ''' 不要になったテーブルデータの削除処理
    ''' </summary>
    ''' <param name="objTbl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfDeleteDBTblData(ByVal objTbl As DataTable) As Boolean
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        Dim objTran As SqlTransaction = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        mfDeleteDBTblData = False

        Try
            'DB接続
            If clsDataConnect.pfOpen_Database(objCon) = False Then
                Throw New Exception()
            End If

            'トランザクション開始
            objTran = objCon.BeginTransaction()

            For Each objRow As DataRow In objTbl.Rows
                'テーブル、スキーマ、DBのどれかの名前がない、また、期間が設定されていない場合は処理しない
                If objRow("M26_DB_NM") = "" OrElse objRow("M26_SCHEMA_NM") = "" OrElse objRow("M26_TABLE_NM") = "" OrElse objRow("M24_KEPPSPAN") <= 0 Then
                    Continue For
                End If

                'ストアド設定
                objCmd = New SqlCommand("SMTDLE001_U1")
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Connection = objCon
                objCmd.Transaction = objTran

                'パラメータ設定
                With objCmd.Parameters
                    .Add(pfSet_Param("prmM24_KEPPSPAN", SqlDbType.NVarChar, objRow("M24_KEPPSPAN")))
                    .Add(pfSet_Param("prmM26_DB_NM", SqlDbType.NVarChar, objRow("M26_DB_NM")))
                    .Add(pfSet_Param("prmM26_SCHEMA_NM", SqlDbType.NVarChar, objRow("M26_SCHEMA_NM")))
                    .Add(pfSet_Param("prmM26_TABLE_NM", SqlDbType.NVarChar, objRow("M26_TABLE_NM")))
                    .Add(pfSet_Param("prmM26_TBL_KEY1", SqlDbType.NVarChar, objRow("M26_TBL_KEY1")))
                    .Add(pfSet_Param("prmM26_TBL_KEY2", SqlDbType.NVarChar, objRow("M26_TBL_KEY2")))
                    .Add(pfSet_Param("prmM26_TBL_KEY3", SqlDbType.NVarChar, objRow("M26_TBL_KEY3")))
                    .Add(pfSet_Param("prmM26_TBL_KEY4", SqlDbType.NVarChar, objRow("M26_TBL_KEY4")))
                    .Add(pfSet_Param("prmM26_TBL_KEY5", SqlDbType.NVarChar, objRow("M26_TBL_KEY5")))
                End With

                objCmd.ExecuteNonQuery()

            Next

            'コミット
            objTran.Commit()

            mfDeleteDBTblData = True

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
            'ロールバック
            If Not objTran Is Nothing Then
                objTran.Rollback()
            End If
            mfDeleteDBTblData = False

        Finally
            'DB接続
            If clsDataConnect.pfOpen_Database(objCon) = False Then
                mfDeleteDBTblData = False
            End If
        End Try
    End Function

    ''' <summary>
    ''' 削除ファイル情報取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDeleteFileDatatbl() As DataTable
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        mfGetDeleteFileDatatbl = Nothing

        Try
            'DB接続
            If clsDataConnect.pfOpen_Database(objCon) = False Then
                Throw New Exception
            End If

            'ストアド設定
            objCmd = New SqlCommand("SMTDLE001_S1")
            objCmd.Connection = objCon
            objCmd.CommandType = CommandType.StoredProcedure

            'SQL実行
            Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

            '取得エラー
            If objDs Is Nothing OrElse objDs.Tables.Count = 0 Then
                Throw New Exception
            End If

            mfGetDeleteFileDatatbl = objDs.Tables(0)

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
            mfGetDeleteFileDatatbl = Nothing
        Finally
            'DB切断
            If clsDataConnect.pfClose_Database(objCon) = False Then
                mfGetDeleteFileDatatbl = Nothing
            End If
        End Try
    End Function

    ''' <summary>
    ''' ファイル削除
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfDeleteFile(ByVal objTbl As DataTable) As Boolean

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            '現在日付取得
            Dim dteNow As DateTime = DateTime.Now

            For Each objRow As DataRow In objTbl.Rows
                'フォルダ取得
                Dim strDir As String = objRow("M26_DIR")

                'ファイル,またはフォルダが空で、ディレクトリが存在しない場合は次の行へ
                If (objRow("M26_FILE_NM") = "" OrElse objRow("M26_DIR") = "") OrElse System.IO.Directory.Exists(objRow("M26_DIR")) = False Then
                    Continue For
                End If

                'フォルダ内ファイル取得
                Dim strFiles As String() = System.IO.Directory.GetFiles(strDir)

                '各ファイルをチェック
                For Each strFile As String In strFiles
                    If System.IO.Path.GetFileName(strFile) = objRow("M26_FILE_NM") Then
                        If objRow("M24_KEPPSPAN") > 0 Then
                            '作成日時より所定の期間が過ぎているか？
                            If dteNow > System.IO.File.GetCreationTime(strFile).AddMonths(objRow("M24_KEPPSPAN")) Then
                                '削除
                                System.IO.File.Delete(strFile)
                            End If
                        End If
                        Exit For
                    End If
                Next
            Next

            '正常終了
            Return True
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
            '異常終了
            Return False
        End Try
    End Function

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
