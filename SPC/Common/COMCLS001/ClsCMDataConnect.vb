'********************************************************************************************************************************
'*　システム　：　サポートセンタステム
'*　処理名　　：　データベース接続クラス
'*　ＰＧＭＩＤ：　ClsCMDataConnect
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.10.21　：　土岐
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号               |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'ClsCMDataConnect-001   2016/08/02      加賀　　　ストアド実行関数追加

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports System.Data
Imports System
Imports System.Configuration
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax

#End Region

Public Class ClsCMDataConnect

    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================

    '============================================================================================================================
    '=　定数定義
    '============================================================================================================================

    '============================================================================================================================
    '=　構造体・列挙体定義
    '============================================================================================================================

    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
#Region "変数定義"
    '--------------------------------
    '2014/04/14 星野　ここから
    '--------------------------------
    Private Shared objStack As StackFrame
    '--------------------------------
    '2014/04/14 星野　ここまで
    '--------------------------------
#End Region

    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================

    '============================================================================================================================
    '=　 プロシージャ定義
    '============================================================================================================================

#Region "そのほかのプロシージャ"
    ''' <summary>
    ''' データベースを開く。
    ''' </summary>
    ''' <param name="opconDB"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function pfOpen_Database(ByRef opconDB As SqlConnection) As Boolean
        Dim ctsSetting As ConnectionStringSettings
        ctsSetting = ConfigurationManager.ConnectionStrings("SPCDB")
        opconDB = New SqlConnection(ctsSetting.ConnectionString)
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            opconDB.Open()
            pfOpen_Database = True
        Catch ex As Exception
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", USER_NAME, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", "接続文字列：" & ctsSetting.ToString & ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            pfOpen_Database = False
        End Try

    End Function

    ''' <summary>
    ''' データベースを閉じる。
    ''' </summary>
    ''' <param name="opconDB"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function pfClose_Database(ByRef opconDB As SqlConnection) As Boolean
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            opconDB.Close()
            opconDB.Dispose()
            pfClose_Database = True
        Catch ex As Exception
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", USER_NAME, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            pfClose_Database = False
        End Try

    End Function

    ''' <summary>
    ''' インプットパラメータを設定する。
    ''' </summary>
    ''' <param name="ipstrName">パラメータ名</param>
    ''' <param name="ipsdtDBType">DBパラメータタイプ</param>
    ''' <param name="ipobjVal">パラメータの値</param>
    ''' <returns>設定したパラメータ</returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function pfSet_Param(ByVal ipstrName As String,
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
    Public Overloads Shared Function pfSet_Param(ByVal ipstrName As String,
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
    Public Overloads Shared Function pfSet_Param(ByVal ipstrName As String,
                                       ByVal ipsdtType As SqlDbType) As SqlParameter
        Dim prmRtn As New SqlParameter(ipstrName, ipsdtType)
        prmRtn.Direction = ParameterDirection.ReturnValue

        pfSet_Param = prmRtn
    End Function

    ''' <summary>
    ''' ストアドプロシージャからデータセットを取得する。
    ''' </summary>
    ''' <param name="ipcmdSQL">コマンド</param>
    ''' <returns>取得したデータセット</returns>
    ''' <remarks></remarks>
    Public Function pfGet_DataSet(ByVal ipcmdSQL As SqlCommand) As DataSet
        Dim dapGetOrders As New SqlDataAdapter(ipcmdSQL)
        Dim dstOrders As New DataSet()
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'コマンドタイプ設定(ストアド)
            ipcmdSQL.CommandType = CommandType.StoredProcedure
            dapGetOrders.Fill(dstOrders)
            pfGet_DataSet = dstOrders

        Catch ex As SqlException

            If Not dstOrders Is Nothing Then
                Call psDisposeDataSet(dstOrders)
'                dstOrders.Dispose()
            End If

            'ログ出力
            psLogWrite("", USER_NAME, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Throw ex

        Catch ex As Exception

            If Not dstOrders Is Nothing Then
                Call psDisposeDataSet(dstOrders)
'                dstOrders.Dispose()
            End If

            'ログ出力
            psLogWrite("", USER_NAME, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Throw ex

        Finally
            If Not dapGetOrders Is Nothing Then
                dapGetOrders.Dispose()
            End If
        End Try
    End Function

    'ClsCMDataConnect-001
    ''' <summary>
    ''' ストアド実行(SELECT)
    ''' </summary>
    ''' <param name="pageCalled">呼び出し元</param>
    ''' <param name="strDataName">取得データ名(エラーメッセージに使用)</param>
    ''' <param name="cmdSQL">実行するSQLコマンド(Connectionが未設定の場合は新たに接続します)</param>
    ''' <param name="dstSelect">実行結果を保存するDataSet</param>
    ''' <returns>True:成功 False:失敗</returns>
    Public Overloads Shared Function pfExec_StoredProcedure(ByVal pageCalled As Page, ByVal strDataName As String, ByRef cmdSQL As SqlCommand, ByRef dstSelect As DataSet) As Boolean

        pfExec_StoredProcedure = False
        objStack = New StackFrame

        'cmdSQLにConnectionが未設定の場合、Connectionを設定
        Dim conDB As SqlConnection = Nothing
        If cmdSQL.Connection Is Nothing Then
            Try
                'WebConfigから接続文字列取得 & 設定
                conDB = New SqlConnection(ConfigurationManager.ConnectionStrings("SPCDB").ConnectionString)

                '接続
                conDB.Open()

                'SQLコマンドにConnection設定
                cmdSQL.Connection = conDB
            Catch ex As Exception

                'ログ出力
                psLogWrite("", USER_NAME, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")

                'メッセージ表示
                psMesBox(pageCalled, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

                Return False

            End Try
        End If

        'SQLコマンド実行
        Try
            'SQLコマンドをストアドとして設定
            cmdSQL.CommandType = CommandType.StoredProcedure

            'データ取得
            dstSelect = New DataSet
            Using dapSelect As SqlDataAdapter = New SqlDataAdapter(cmdSQL)
                dapSelect.Fill(dstSelect)
            End Using

            Return True

        Catch ex As SqlException

            'エラー判別
            If ex.Number = -2 Then
                'SQLタイムアウト
                psMesBox(pageCalled, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【SQLタイムアウト】" + strDataName + "の取得") '{0}の取得に失敗しました。
            Else
                'SQLエラー
                psMesBox(pageCalled, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【SQLエラー】" + strDataName + "の取得")       '{0}の取得に失敗しました。
            End If

            'ログ出力
            psLogWrite("", pageCalled.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

            Return False

        Catch ex As Exception

            'エラー
            psMesBox(pageCalled, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strDataName + "の取得")       '{0}の取得に失敗しました。

            'ログ出力
            psLogWrite("", pageCalled.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

            Return False

        Finally
            'DB接続クローズ
            If conDB IsNot Nothing Then
                conDB.Close()
                conDB.Dispose()
            End If
        End Try

    End Function

    'ClsCMDataConnect-001
    ''' <summary>
    ''' ストアド実行(INSERT/UPDATE)
    ''' </summary>
    ''' <param name="pageCalled">呼び出し元</param>
    ''' <param name="strProcName">取得データ名(エラーメッセージに使用)</param>
    ''' <param name="cmdSQL">実行するSQLコマンド(Connectionが未設定の場合は新たに接続します)</param>
    ''' <returns>True:成功 False:失敗</returns>
    Public Overloads Shared Function pfExec_StoredProcedure(ByVal pageCalled As Page, ByVal strProcName As String, ByRef cmdSQL As SqlCommand) As Boolean

        pfExec_StoredProcedure = False
        objStack = New StackFrame

        'cmdSQLにConnectionが未設定の場合、Connectionを設定
        Dim conDB As SqlConnection = Nothing

        If cmdSQL.Connection Is Nothing Then
            Try
                'WebConfigから接続文字列取得 & 設定
                conDB = New SqlConnection(ConfigurationManager.ConnectionStrings("SPCDB").ConnectionString)

                '接続
                conDB.Open()

                'SQLコマンドにConnection設定
                cmdSQL.Connection = conDB

            Catch ex As Exception

                'ログ出力
                psLogWrite("", USER_NAME, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                'メッセージ表示
                psMesBox(pageCalled, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

                Return False

            End Try
        End If

        'SQLコマンド実行
        Try
            'SQLコマンドをストアドとして設定
            cmdSQL.CommandType = CommandType.StoredProcedure

            'SQLコマンド実行
            cmdSQL.ExecuteNonQuery()

            Return True

        Catch ex As SqlException

            'エラー判別
            If ex.Number = -2 Then
                'SQLタイムアウト
                psMesBox(pageCalled, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【SQLタイムアウト】" + strProcName) '{0}に失敗しました。
            Else
                'SQLエラー
                psMesBox(pageCalled, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【SQLエラー】" + strProcName)       '{0}に失敗しました。
            End If

            'ログ出力
            psLogWrite("", pageCalled.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

            Return False

        Catch ex As Exception

            'エラー
            psMesBox(pageCalled, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strProcName + "の取得")       '{0}の取得に失敗しました。

            'ログ出力
            psLogWrite("", pageCalled.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

            Return False

        Finally
            'DB接続クローズ
            If conDB IsNot Nothing Then
                conDB.Close()
                conDB.Dispose()
            End If
        End Try

    End Function

    ''' <summary>
    ''' ストアドまとめて実行(INSERT/UPDATE)
    ''' </summary>
    ''' <param name="pageCalled">呼び出し元</param>
    ''' <param name="cmdSQL">実行するSQLコマンド(Connectionは設定不要です)</param>
    ''' <param name="intCmdIndex">最後に実行したコマンドのIndex(-1:接続失敗)</param>
    ''' <returns>True:成功 False:失敗</returns>
    Public Overloads Shared Function pfExec_StoredProcedure(ByVal pageCalled As Page, ByRef cmdSQL() As SqlCommand, Optional ByRef intCmdIndex As Integer = 0) As Boolean

        pfExec_StoredProcedure = False
        objStack = New StackFrame

        Dim conDB As SqlConnection = Nothing

        '接続
        Try
            'WebConfigから接続文字列取得 & 設定
            conDB = New SqlConnection(ConfigurationManager.ConnectionStrings("SPCDB").ConnectionString)

            '接続
            conDB.Open()

        Catch ex As Exception

            intCmdIndex = -1

            'ログ出力
            psLogWrite("", USER_NAME, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Return False

        End Try


        'トランザクション開始
        Using trnDB As SqlTransaction = conDB.BeginTransaction

            Try
                'コマンド実行
                For i As Integer = 0 To cmdSQL.Count - 1

                    '実行中コマンドのIndex
                    intCmdIndex = i

                    'コマンドが無い場合次へ
                    If cmdSQL(i) Is Nothing Then
                        Continue For
                    End If

                    'Connection設定
                    cmdSQL(i).Connection = conDB

                    'トランザクション設定
                    cmdSQL(i).Transaction = trnDB

                    'SQLコマンドをストアドとして設定
                    cmdSQL(i).CommandType = CommandType.StoredProcedure

                    'SQLコマンド実行
                    cmdSQL(i).ExecuteNonQuery()

                Next

                'コミット
                trnDB.Commit()

                '正常終了
                Return True

            Catch ex As Exception

                'RollBack
                trnDB.Rollback()

                'ログ出力
                psLogWrite("", pageCalled.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

                Return False

            Finally

                'DB接続クローズ
                If conDB IsNot Nothing Then
                    conDB.Close()
                    conDB.Dispose()
                End If

            End Try

        End Using

    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　ＤＡＴＡＳＥＴ廃棄　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ１：廃棄対象データセットオブジェクト　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' データセットの破棄を行います
    ''' </summary>
    ''' <param name="cpobjDS">破棄するデータセット</param>
    ''' <remarks></remarks>
    Public Shared Sub psDisposeDataSet(ByRef cpobjDS As DataSet)

        '変数定義
        Dim zz As Integer
        If cpobjDS Is Nothing Then Exit Sub
        Try

            'データセット内のテーブル数分ループ
            If cpobjDS.Tables.Count > 0 Then
                For zz = cpobjDS.Tables.Count - 1 To 0 Step -1
                    If Not (cpobjDS.Tables(zz) Is Nothing) Then
                        'テーブル削除
                        cpobjDS.Tables.RemoveAt(zz)
                    End If

                    'テーブル廃棄
                    '                    cpobjDS.Tables(zz).Dispose()
                Next
                'テーブル削除
                cpobjDS.Tables.Clear()
            End If
        Catch ex As Exception
            'ログ出力
            psLogWrite("", USER_NAME, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

        Finally
            'データセットクリア
            cpobjDS.Clear()
            'データセット廃棄
            cpobjDS.Dispose()
        End Try

    End Sub

#End Region


End Class
