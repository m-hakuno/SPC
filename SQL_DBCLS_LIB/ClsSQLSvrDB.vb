'****************************************************************************************************
'*　システム　：－－－－－－－－－－－－－－－－－－－－　　　　　　　　　　　　　　　　　Ver 0.0.1 *
'*  プログラム：ＤＢ関連クラス　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　*
'*　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　*
'*　　　　　　　　　　　　　　　　　　作成日付　2008.12.02　　作成時間　00：00　　作成者　伯野　　  *
'*　　　　　　　　　　　　　　　　　　更新日付　2009.04.15　　更新時間　16：54　　更新者　伯野　　  *
'*　　　　　　　　　　　　　　　　　　更新日付　2011.11.09　　更新時間　11：05　　更新者　伯野　　  *
'*　　　　　　　　　　　　　　　　　　更新日付　2012.01.19　　更新時間　12：00　　更新者　伯野　　  *
'*　　　　　　　　　　　　　　　　　　検証日付　----.--.--　　検証時間　--：--　　検証者　－－－　  *
'*　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　*
'*　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　Copyright By SANCOSMOS Co.,LTD. *
'****************************************************************************************************

'====================================================================================================
'=　参照追加　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　=
'====================================================================================================

Imports System.Data.SqlClient
Imports SQL_COMCLS_LIB
'Imports System.Web.UI.HtmlControls
'Imports RP_LIB.ClsRPComVar


Public Class ClsSQLSvrDB

    '====================================================================================================
    '=　継承定義　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　=
    '====================================================================================================
    Implements IDisposable 'インラインＤｉｓｐｏｓｅの追加
    Private disposedValue As Boolean = False ' 重複する呼び出しを検出するには

    '====================================================================================================
    '=　定数定義　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　=
    '====================================================================================================
    'いけてる文字列 Provider=IBMDA400.DataSource.1;Data Source=nnn.nnn.nnn.nnn;Persist Security Info=True;User ID=ISHIZEKI;Initial Catalog=S105257G
    Structure typConStr
        Public Provider As String   'プロバイダー名
        Public Driver As String     'ＯＤＢＣドライバー名
        Public DataSource As String 'サーバー名（ＩＰアドレス）
        Public Serv As String       'サーバー名（未使用）
        Public System As String     'サーバー名
        Public DSN As String        'データソース名（未使用）
        Public UserID As String     'ユーザーＩＤ
        Public Password As String   'パスワード
        Public Collect As String    'スキーマ名
        Public Port As String       'ポート番号（未使用）
        Public Dbq As String        '？？ＤＢＱ（？？ＡＳ／４００固有？？）
        Public DftPkgLib As String  '？？ＬＩＢ（？？ＡＳ／４００固有？？）
        Public LanguageID As String '？？コードページ（？？ＡＳ／４００固有？？）
        Public Pkg As String        '？？パッケージ（？？ＡＳ／４００固有？？）
        Public Schema As String     'スキーマ（接続には未使用）
    End Structure

    Public Enum enmDirect
        dInput = ParameterDirection.Input
        dInOutput = ParameterDirection.InputOutput
        dOutput = ParameterDirection.Output
        dRtnValue = ParameterDirection.ReturnValue
    End Enum

    Public Enum enmLogSwt
        OutLog = 1          'ログ出力あり
        NoLog = 2           'ログ出力なし
    End Enum

    '====================================================================================================
    '=　変数定義　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　=
    '====================================================================================================
    '--------------------------------------------------------------------------------------------------------
    '-　オブジェクト定義　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    Dim mobjDBX As SqlConnection        '単独接続用ＤＢ接続オブジェクト
    Public mobjDB As SqlConnection        'クラス全体用ＤＢ接続オブジェクト（ＧＲＡＮＤＩＴ対応で外にさらす）
    Dim mobjDBDA As SqlDataAdapter     'データアダプター
    Dim mobjDBCB As SqlCommandBuilder      'コマンドビルダー
    Public mobjDBCM As SqlCommand         'コマンド
    Dim mobjDBDR As SqlDataReader          'データリーダー
    Dim mclsCom As Object                   '関数ＤＬＬ
    'Dim mclsLog As New ClsA4LogOut                    '実行ログ出力ＤＬＬ

    '--------------------------------------------------------------------------------------------------------
    '-　クラス共有変数定義　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    Dim menmLog As enmLogSwt = enmLogSwt.OutLog '実行ログ出力フラグ
    Dim mstrLog As String = ""                  'フルパスログファイル名
    Dim mtypConStr As typConStr                 '接続文字列群構造体
    Dim mstrXML As String = ""                  'ＸＭＬファイル名
    Dim mstrErr() As String                     '発生エラー保持配列
    Dim mlngErrCnt As Integer                      '発生エラーカウンター
    Dim mblnConState As Boolean = False         'ＤＢ接続状態ステータス
    Dim mstrLogDir As String = ""               '実行ログ出力フォルダ
    Public mstrConString As String = ""               '実行ログ出力フォルダ
    Dim mstrLogName As String = ""              '実行ログ出力ファイル名
    Dim mclsLog As New SQL_COMCLS_LIB.ClsSQLLogOut 'ログ出力クラスのインスタンス

    '====================================================================================================
    '=　プロパティ定義　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　=
    '====================================================================================================
    Property ppProvider() As String
        Get
            Return mtypConStr.Provider
        End Get
        Set(ByVal value As String)
            mtypConStr.Provider = value
        End Set
    End Property

    Property ppDriver() As String
        Get
            Return mtypConStr.Driver
        End Get
        Set(ByVal value As String)
            mtypConStr.Driver = value
        End Set
    End Property

    Property ppDataSource() As String
        Get
            Return mtypConStr.DataSource
        End Get
        Set(ByVal value As String)
            mtypConStr.DataSource = value
        End Set
    End Property

    Property ppServer() As String
        Get
            Return mtypConStr.Serv
        End Get
        Set(ByVal value As String)
            mtypConStr.Serv = value
        End Set
    End Property

    Property ppDSN() As String
        Get
            Return mtypConStr.DSN
        End Get
        Set(ByVal value As String)
            mtypConStr.DSN = value
        End Set
    End Property

    Property ppUserID() As String
        Get
            Return mtypConStr.UserID
        End Get
        Set(ByVal value As String)
            mtypConStr.UserID = value
        End Set
    End Property

    Property ppPassword() As String
        Get
            Return mtypConStr.Password
        End Get
        Set(ByVal value As String)
            mtypConStr.Password = value
        End Set
    End Property

    Property ppCollect() As String
        Get
            Return mtypConStr.Collect
        End Get
        Set(ByVal value As String)
            mtypConStr.Collect = value
        End Set
    End Property

    Property ppPort() As String
        Get
            Return mtypConStr.Port
        End Get
        Set(ByVal value As String)
            mtypConStr.Port = value
        End Set
    End Property

    Property ppLogOut() As enmLogSwt
        Get
            Return menmLog
        End Get
        Set(ByVal value As enmLogSwt)
            menmLog = value
        End Set
    End Property

    Property ppErrMsg() As String()
        Get
            Return mstrErr
            Call sInitErrMsg()
        End Get
        Set(ByVal value() As String)
        End Set
    End Property

    Property ppConnState() As Boolean
        Get
            If mobjDB.State = ConnectionState.Broken Then
                mblnConState = False
            ElseIf mobjDB.State = ConnectionState.Closed Then
                mblnConState = False
            Else
                mblnConState = True
            End If
            Return mblnConState
        End Get
        Set(ByVal value As Boolean)
        End Set
    End Property

    Property ppDir() As String
        Get
            Return mstrLogDir
        End Get
        Set(ByVal value As String)
            mstrLogDir = value
            'mclsLog.ppDir = mstrLogDir
        End Set
    End Property

    Property ppProduct() As String
        Get
            Return mstrLogName
        End Get
        Set(ByVal value As String)
            mstrLogName = value
            'mclsLog.ppProduct = mstrLogName
        End Set
    End Property

    Property ppDbq() As String
        Get
            Return mtypConStr.Dbq
        End Get
        Set(ByVal value As String)
            mtypConStr.Dbq = value
        End Set
    End Property

    Property ppDftPkgLib() As String
        Get
            Return mtypConStr.DftPkgLib
        End Get
        Set(ByVal value As String)
            mtypConStr.DftPkgLib = value
        End Set
    End Property

    Property ppLanguageID() As String
        Get
            Return mtypConStr.LanguageID
        End Get
        Set(ByVal value As String)
            mtypConStr.LanguageID = value
        End Set
    End Property

    Property ppPkg() As String
        Get
            Return mtypConStr.Pkg
        End Get
        Set(ByVal value As String)
            mtypConStr.Pkg = value
        End Set
    End Property

    Property ppSystem() As String
        Get
            Return mtypConStr.System
        End Get
        Set(ByVal value As String)
            mtypConStr.System = value
        End Set
    End Property

    Property ppSchema() As String
        Get
            Return mtypConStr.Schema
        End Get
        Set(ByVal value As String)
            mtypConStr.Schema = value
        End Set
    End Property

    Property ppXMLFile() As String
        Get
            Return mstrXML
        End Get
        Set(ByVal value As String)
            mstrXML = value
        End Set
    End Property

    '---------------------------------------------------------------------------------------------------
    '-　トランザクション継続確認
    '---------------------------------------------------------------------------------------------------
    Public Property ppKeep_Tran() As Boolean
        Get
            If mobjDBCM.Transaction Is Nothing Then
                Return False
            Else
                If mobjDBCM.Transaction.Connection Is Nothing Then
                    Return False
                Else
                    Return True
                End If
            End If
        End Get
        Set(ByVal value As Boolean)
        End Set
    End Property

    '====================================================================================================
    '=　プロシージャ定義　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　=
    '====================================================================================================
    '--------------------------------------------------------------------------------------------------------
    '-　インスタンス生成　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ１：接続文字列　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="ipstrDir"></param>
    ''' <param name="ipstrProduct"></param>
    ''' <remarks></remarks>
    <System.Diagnostics.DebuggerStepThrough()> _
    Public Sub New(Optional ByVal ipstrDir As String = "", Optional ByVal ipstrProduct As String = "")

        'ＮＥＷされた時にＤＢオブジェクトを作成しておかないと接続ステータスの確認がとれないので、記述を追加
        'コネクションを張る時はＮＥＷでインスタンスが作成されるので状況はリセットされるはず
        mobjDB = New SqlClient.SqlConnection    'クラス全体用ＤＢ接続オブジェクト
        mobjDBCM = New SqlClient.SqlCommand     'ＳＱＬＣＯＭＭＡＮＤの新規インスタンス

    End Sub

    '--------------------------------------------------------------------------------------------------------
    '-　ＤＢ接続　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ１：接続文字列　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' データベースへの接続・オープン
    ''' </summary>
    ''' <param name="ipConnectString">接続文字列</param>
    ''' <returns>BOOLEAN 成功：TRUE 失敗：FALSE</returns>
    ''' <remarks></remarks>
    Public Function pfDB_Connect(Optional ByVal ipConnectString As String = "") As Boolean

        Dim strDBConStr As String = ""

        pfDB_Connect = False

        Try
            If ipConnectString = "" Then
                If mstrConString = "" Then
                    If mstrXML = "" Then
                        Call sAddErrMsg("接続失敗　接続文字列設定ＸＭＬファイルが未設定")
                        Exit Function
                    Else
                        If pfXML_Read(mstrXML) = False Then
                            Call sAddErrMsg("接続失敗　接続文字列設定ＸＭＬファイルが正しくないか存在しない")
                            Exit Function
                        End If
                    End If
                    If fCreateConnectstring(strDBConStr) = False Then
                        Call sAddErrMsg("接続失敗　接続文字列生成の失敗")
                        Exit Function
                    Else
                        mstrConString = strDBConStr
                    End If
                End If
            Else
                strDBConStr = ipConnectString
            End If
            'ＤＢの新しいインスタンスを生成する (接続文字列を指定)
            If strDBConStr = "" Then
                mobjDB = New SqlConnection(mstrConString)
            Else
                mobjDB = New SqlConnection(strDBConStr)
            End If
            ' データベース接続を開く
            mobjDB.Open()
            pfDB_Connect = True
        Catch ex As Exception
            Dim strErrMsg As String = "接続失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            mobjDB.Dispose()
            Call mclsLog.wrtLog(strErrMsg & ".エラー発生時の接続文字列：" & strDBConStr, "DB0001", "DBCLASS", "DBCLASS")
        End Try

    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　ＤＡＴＡＳＥＴ作成　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ１：保管データセットオブジェクト　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　２：ＳＱＬ文　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　３：テーブル名　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' データセットの作成　コネクションが事前に作成されていることが条件になります
    ''' </summary>
    ''' <param name="cpobjDS">結果を格納するデータセット　事前にインスタンスを作成しておくこと</param>
    ''' <param name="ipstrSQL">ＳＱＬ文</param>
    ''' <param name="ipstrDSName">データテーブルに名前をつける場合は指定して下さい</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function pfDB_CreateDataSetNoCon(ByRef cpobjDS As DataSet, ByVal ipstrSQL As String, _
                                                        Optional ByVal ipstrDSName As String = "") As Boolean

        '戻り値　設定（失敗）
        pfDB_CreateDataSetNoCon = False

        Try
            'ＤＢコネクトが壊れていなくてかつ閉じていないとき
            If mobjDB.State <> ConnectionState.Broken And mobjDB.State <> ConnectionState.Closed Then
                Try
                    'データセットの作成
                    mobjDBDA = New SqlDataAdapter(ipstrSQL, mobjDB)
                    If mobjDB Is Nothing Then
                    Else
                        If mobjDBCM Is Nothing Then
                        Else
                            mobjDBDA.SelectCommand.Transaction = mobjDBCM.Transaction
                        End If
                    End If
                    If ipstrDSName <> "" Then
                        mobjDBDA.Fill(cpobjDS, ipstrDSName)
                    Else
                        mobjDBDA.Fill(cpobjDS)
                    End If
                    '戻り値　設定（成功）
                    pfDB_CreateDataSetNoCon = True
                Catch ex As Exception
                    'エラー時の処理
                    'Call mclsLog.wrtLog(ex.Message & ".データセット作成失敗：" & ipstrSQL)
                Finally
                    'データアダプターの廃棄
                    mobjDBDA.Dispose()
                End Try
            Else
                Call sAddErrMsg("ＤＢに接続していないので生成できません")
            End If

        Catch ex As Exception
            Dim strErrMsg As String = "ＤＳ生成失敗[" & ex.Source & "]" & ex.Message
            'エラー時の処理
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & " SQL=" & ipstrSQL, "DB0002", "DBCLASS", "DBCLASS")
        End Try

    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　ＤＡＴＡＳＥＴ作成　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ１：保管データセットオブジェクト　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　２：ＳＱＬ文　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　３：テーブル名　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' データテーブルの作成　コネクションが事前に作成されていることが条件になります
    ''' </summary>
    ''' <param name="cpobjDT">結果を格納するデータテーブル　事前にインスタンスを作成しておくこと</param>
    ''' <param name="ipstrSQL">ＳＱＬ文</param>
    ''' <returns>戻り値：関数の処理結果 True:成功 / False:失敗</returns>
    ''' <remarks></remarks>
    Public Function pfDB_CreateDataSetNoCon(ByRef cpobjDT As DataTable, ByVal ipstrSQL As String) As Boolean

        '戻り値　設定（失敗）
        pfDB_CreateDataSetNoCon = False

        Try
            'ＤＢコネクトが壊れていなくてかつ閉じていないとき
            If mobjDB.State <> ConnectionState.Broken And mobjDB.State <> ConnectionState.Closed Then
                Try
                    'データセットの作成
                    mobjDBDA = New SqlDataAdapter(ipstrSQL, mobjDB)
                    If mobjDB Is Nothing Then
                    Else
                        If mobjDBCM Is Nothing Then
                        Else
                            mobjDBDA.SelectCommand.Transaction = mobjDBCM.Transaction
                        End If
                    End If
                    'データテーブル作成
                    mobjDBDA.Fill(cpobjDT)
                    '戻り値　設定（成功）
                    pfDB_CreateDataSetNoCon = True
                Catch ex As Exception
                    'エラー時の処理
                    'Call mclsLog.wrtLog(ex.Message & ".データセット作成失敗：" & ipstrSQL)
                Finally
                    'データアダプターの廃棄
                    mobjDBDA.Dispose()
                End Try
            Else
                Call sAddErrMsg("ＤＢに接続していないので生成できません")
            End If

        Catch ex As Exception
            'エラー時の処理
            Dim strErrMsg As String = "ＤＳ生成失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & " SQL=" & ipstrSQL, "DB0003", "DBCLASS", "DBCLASS")
        End Try

    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　ＤＡＴＡＳＥＴ作成　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ１：接続文字列　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　２：保管データセットオブジェクト　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　３：ＳＱＬ文　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　４：テーブル名　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' データセットの作成　コネクションの接続も行いますので通常はこちらを使用します
    ''' </summary>
    ''' <param name="cpobjDS">結果を格納するデータセット　事前にインスタンスを作成しておくこと</param>
    ''' <param name="ipstrSQL">ＳＱＬ文</param>
    ''' <param name="ipstrDSName">データテーブルに名前をつける場合は指定して下さい</param>
    ''' <param name="ipstrConnect">接続文字列　省略した場合は生成します</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function pfDB_CreateDataSet(ByRef cpobjDS As DataSet, ByVal ipstrSQL As String, _
                    Optional ByVal ipstrDSName As String = "", Optional ByVal ipstrConnect As String = "") As Boolean

        Dim strDBConStr As String = ""

        '戻り値　設定（失敗）
        pfDB_CreateDataSet = False
        cpobjDS = New DataSet
        Try
            'ＤＢ接続
            If ipstrConnect = "" Then
                If mstrConString = "" Then
                    If fCreateConnectstring(strDBConStr) = False Then
                        Call sAddErrMsg("接続失敗　接続文字列生成の失敗")
                        Exit Function
                    Else
                        mstrConString = strDBConStr
                    End If
                End If
            Else
                strDBConStr = ipstrConnect
            End If

            If mobjDBX Is Nothing Then
                If strDBConStr = "" Then
                    mobjDBX = New SqlConnection(mstrConString)
                Else
                    mobjDBX = New SqlConnection(strDBConStr)
                End If
            End If
            If mobjDBX.State <> ConnectionState.Open Then
                mobjDBX.Open()
            End If

            Try
                'データセットの作成
                mobjDBDA = New SqlDataAdapter(ipstrSQL, mobjDBX)
                'Dim cmd As New SQLCommand(ipstrSQL, mobjDBX)
                'mobjDBDA = New SQLDataAdapter(cmd)
                If ipstrDSName <> "" Then
                    mobjDBDA.Fill(cpobjDS, ipstrDSName)
                Else
                    mobjDBDA.Fill(cpobjDS)
                End If
                '戻り値　設定（成功）
                pfDB_CreateDataSet = True
            Catch ex As Exception
                'エラー時の処理
                Dim strErrMsg As String = "ＤＳ生成失敗[" & ex.Source & "]" & ex.Message
                Call sAddErrMsg(strErrMsg)
                Call mclsLog.wrtLog(strErrMsg & " SQL=" & ipstrSQL, "DB0004", "DBCLASS", "DBCLASS")
            Finally
                'データアダプターの廃棄
                mobjDBDA.Dispose()
            End Try

        Catch ex As Exception
            'エラー時の処理
            Dim strErrMsg As String = "ＤＢ接続失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & "：" & ipstrConnect, "DB0005", "DBCLASS", "DBCLASS")
        Finally
            ''DATABASEが開いていればクローズ
            'If mobjDBX.State = ConnectionState.Open Then
            '    'データベース接続を閉じる (正しくは オブジェクトの破棄を保証する を参照)
            '    mobjDBX.Close()
            '    mobjDBX.Dispose()
            'End If
        End Try

    End Function

    ''' <summary>
    ''' ストアドプロシージャからデータセットを取得する。
    ''' </summary>
    ''' <param name="ipcmdSQL">ストアド名、パラメータを指定済のＳＱＬＣｏｍｍａｎｄ</param>
    ''' <param name="opobjwkDS">結果のＤａｔａｓｅｔ</param>
    ''' <returns>true:成功/False:失敗</returns>
    ''' <remarks></remarks>
    Public Function pfGet_DataSet(ByVal ipcmdSQL As SqlCommand, ByRef opobjwkDS As DataSet) As Boolean

        Dim objwkDA As New SqlDataAdapter(ipcmdSQL)
        Dim objwkDS As New DataSet()

        pfGet_DataSet = False

        Try
            'コマンドタイプ設定(ストアド)
            ipcmdSQL.CommandType = CommandType.StoredProcedure
            objwkDA.Fill(objwkDS)
            opobjwkDS = objwkDS.Copy

            pfGet_DataSet = True

        Catch ex As Exception
            'エラー時の処理
            Dim strErrMsg As String = "ストアドプロシージャ実行失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & " SQL=" & ipcmdSQL.CommandText, "DB0004", "DBCLASS", "DBCLASS")
        Finally

            If objwkDS Is Nothing Then
            Else
                Dim zz As Integer = 0
                For zz = 0 To objwkDS.Tables.Count - 1
                    objwkDS.Tables(0).Clear()
                    objwkDS.Tables(0).Dispose()
                Next
                objwkDS.Clear()
            End If

        End Try

    End Function

    ''' <summary>
    ''' ストアドのＥＸＥＣＵＴＥ実行
    ''' </summary>
    ''' <param name="cpobjSQLCmd">設定済のＳＱＬＣｏｍｍａｎｄ</param>
    ''' <returns>True：成功／False:失敗</returns>
    ''' <remarks></remarks>
    Public Function pfDB_ProcStored(ByRef cpobjSQLCmd As SqlCommand) As Boolean

        pfDB_ProcStored = False

        Try
            cpobjSQLCmd.CommandType = CommandType.StoredProcedure
            cpobjSQLCmd.ExecuteNonQuery()

            pfDB_ProcStored = True

        Catch ex As Exception
            'エラー時の処理
            Dim strErrMsg As String = "ストアドプロシージャ実行失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & " SQL=" & cpobjSQLCmd.CommandText, "DB0004", "DBCLASS", "DBCLASS")
        End Try

    End Function

    ''' <summary>
    ''' ストアドのＥＸＥＣＵＴＥ実行
    ''' </summary>
    ''' <param name="cpobjSQLCmd">設定済のＳＱＬＣｏｍｍａｎｄ</param>
    ''' <param name="opintRetCD">処理結果　ストアドの戻り値</param>
    ''' <returns>True：成功／False:失敗</returns>
    ''' <remarks></remarks>
    Public Function pfDB_ProcStored(ByRef cpobjSQLCmd As SqlCommand, ByRef opintRetCD As Integer) As Boolean

        pfDB_ProcStored = False

        Try
            cpobjSQLCmd.CommandType = CommandType.StoredProcedure
            cpobjSQLCmd.ExecuteNonQuery()

            'ストアド戻り値
            opintRetCD = Integer.Parse(cpobjSQLCmd.Parameters("retvalue").Value.ToString)

            pfDB_ProcStored = True

        Catch ex As Exception
            'エラー時の処理
            Dim strErrMsg As String = "ストアドプロシージャ実行失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & " SQL=" & cpobjSQLCmd.CommandText, "DB0004", "DBCLASS", "DBCLASS")
        End Try

    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　コマンドビルダーによるＤＢ更新　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ１：データアダプタ　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ２：データセット　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' コマンドビルダーによるＤＢ更新
    ''' </summary>
    ''' <param name="ipstrSQL">SQL文</param>
    ''' <param name="cpobjDS">更新に使用するデータセット</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function pfDB_ComBldUpdate(ByVal ipstrSQL As String, ByRef cpobjDS As DataSet) As Boolean

        '戻り値　設定（失敗）
        pfDB_ComBldUpdate = False

        Try
            mobjDBDA = New SqlDataAdapter(ipstrSQL, mobjDB)
            'コマンドビルダー
            mobjDBCB = New SqlCommandBuilder(mobjDBDA)
            'データセット更新
            mobjDBDA.Update(cpobjDS)
            '戻り値　設定（成功）
            pfDB_ComBldUpdate = True

        Catch ex As Exception
            'エラー時の処理
            Dim strErrMsg As String = "コマンドビルダー実行失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & " SQL=" & ipstrSQL, "DB0006", "DBCLASS", "DBCLASS")
        End Try

    End Function

    Public Function pfDB_CreateDynaset(ByVal ipstrSQL As String, ByRef cpobjDA As SqlDataAdapter) As Boolean

        pfDB_CreateDynaset = False

        Try
            cpobjDA = New SqlDataAdapter(ipstrSQL, mobjDB)
            pfDB_CreateDynaset = True
        Catch ex As Exception
            'エラー時の処理
            Dim strErrMsg As String = "データアダプター作成失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & " SQL=" & ipstrSQL, "DB0007", "DBCLASS", "DBCLASS")
        End Try

    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　データリーダ生成　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ１：ＳＱＬ文　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ２：データリーダー　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' 読み込み処理用データリーダ作成
    ''' </summary>
    ''' <param name="ipstrSQL">SQL文</param>
    ''' <param name="cpobjDR">読み込みに使用するデータリーダー</param>
    ''' <returns>関数の成否 True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    Public Function pfDB_CreateSnapShot(ByVal ipstrSQL As String, ByRef cpobjDR As SqlDataReader) As Boolean

        pfDB_CreateSnapShot = False

        Dim objDBCM As SqlCommand

        Try

            objDBCM = New SqlCommand(ipstrSQL, mobjDB)
            If mobjDBCM.Transaction Is Nothing Then
            Else
                objDBCM.Transaction = mobjDBCM.Transaction
            End If
            cpobjDR = objDBCM.ExecuteReader
            pfDB_CreateSnapShot = True

        Catch ex As Exception
            'エラー時の処理
            Dim strErrMsg As String = "データリーダー作成失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & " SQL=" & ipstrSQL, "DB0008", "DBCLASS", "DBCLASS")
        End Try

    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　データリーダ生成　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ１：ＳＱＬ文　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ２：データリーダー　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' トランザクション開始後用読み込み処理用データリーダ作成（使用不可）
    ''' </summary>
    ''' <param name="ipstrSQL">SQL文</param>
    ''' <param name="cpobjDR">読み込みに使用するデータリーダー</param>
    ''' <returns>関数の成否 True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    Public Function pfDB_CreateSnapShot_Trans(ByVal ipstrSQL As String, ByRef cpobjDR As SqlDataReader) As Boolean

        pfDB_CreateSnapShot_Trans = False

        Dim objDBCM As SqlCommand

        Try

            objDBCM = New SqlCommand(ipstrSQL, mobjDB)
            objDBCM.Transaction = mobjDBCM.Transaction
            cpobjDR = objDBCM.ExecuteReader
            pfDB_CreateSnapShot_Trans = True

        Catch ex As Exception
            'エラー時の処理
            Dim strErrMsg As String = "データリーダー作成失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & " SQL=" & ipstrSQL, "DB0009", "DBCLASS", "DBCLASS")
        End Try

    End Function

    ''' <summary>
    ''' トランザクション開始後用読み込み処理用データリーダ作成
    ''' </summary>
    ''' <param name="ipstrSQL">SQL文</param>
    ''' <param name="cpobjDR">読み込みに使用するデータリーダー</param>
    ''' <param name="cpobjTran">対象ＤＢのトランザクション</param>
    ''' <returns>関数の成否 True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    Public Function pfDB_CreateSnapShot_Trans(ByVal ipstrSQL As String, ByRef cpobjDR As SqlDataReader, _
                                                                ByRef cpobjTran As SqlClient.SqlTransaction) As Boolean

        pfDB_CreateSnapShot_Trans = False

        Dim sqlTran As SqlTransaction = cpobjTran           'Start a local transaction.
'        Dim objDBCM As SqlCommand  = mobjDB.CreateCommand    'Enlist the command in the current transaction.
        Dim objDBCM As SqlCommand = New SqlCommand(ipstrSQL, mobjDB)    'Enlist the command in the current transaction.

        Try
'            objDBCM = New SqlCommand(ipstrSQL, mobjDB)
            objDBCM.Transaction = sqlTran
'            objDBCM.Transaction = mobjDBCM.Transaction
            cpobjDR = objDBCM.ExecuteReader
            pfDB_CreateSnapShot_Trans = True

        Catch ex As Exception
            'エラー時の処理
            Dim strErrMsg As String = "データリーダー作成失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & " SQL=" & ipstrSQL, "DB0010", "DBCLASS", "DBCLASS")
        End Try

    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　データリーダ作成
    '--------------------------------------------------------------------------------------------------------
    Public Function pfDB_ReadDatabase(ByVal ipstrSQL As String, ByVal ipcolName As String) As String

        pfDB_ReadDatabase = String.Empty

        Dim objDBCM As SqlCommand
        Dim dr As SqlDataReader
        Dim blnOrgConn As Boolean = False

        Try
            If mobjDB.State = ConnectionState.Closed Or mobjDB.State = ConnectionState.Broken Then
                Call pfDB_Connect()
                blnOrgConn = True
            End If
            objDBCM = New SqlCommand(ipstrSQL, mobjDB)
            dr = objDBCM.ExecuteReader

            If dr.HasRows = False Then Exit Function
            dr.Read()
            pfDB_ReadDatabase = dr.Item(ipcolName).ToString
            dr.Close()
        Catch ex As Exception
            'エラー時の処理
            Dim strErrMsg As String = "データリーダー作成失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & " SQL=" & ipstrSQL, "DB0011", "DBCLASS", "DBCLASS")
        Finally
            If mobjDB.State = ConnectionState.Open Then
                If blnOrgConn Then
                    mobjDB.Close()
                End If
            End If
        End Try

    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　データリーダ作成
    '--------------------------------------------------------------------------------------------------------
    Public Function pfDB_Readase(ByVal ipstrSQL As String) As DataTable

        pfDB_Readase = Nothing

        Dim objDBCM As SqlCommand
        Dim dt As New DataTable
        Dim dr As SqlDataReader
        Dim blnOrgConn As Boolean = False

        Try
            If mobjDB.State = ConnectionState.Closed Or mobjDB.State = ConnectionState.Broken Then
                Call pfDB_Connect()
                blnOrgConn = True
            End If
            objDBCM = New SqlCommand(ipstrSQL, mobjDB)
            dr = objDBCM.ExecuteReader

            If dr.HasRows = False Then Exit Function
            Using dr
                For zz As Integer = 0 To dr.FieldCount - 1
                    dt.Columns.Add(zz.ToString)
                Next

                While dr.Read
                    Dim ss(dr.FieldCount - 1) As Object
                    dr.GetValues(ss)
                    dt.Rows.Add(ss)
                End While
            End Using
            'dr.Read()
            'pfDB_ReadDatabase = dr.Item(ipcolName).ToString
            dr.Close()
            Return dt
        Catch ex As Exception
            'エラー時の処理
            Dim strErrMsg As String = "データリーダー作成失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & " SQL=" & ipstrSQL, "DB0012", "DBCLASS", "DBCLASS")
        Finally
            If mobjDB.State = ConnectionState.Open Then
                If blnOrgConn Then
                    mobjDB.Close()
                End If
            End If
        End Try

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
    Public Sub psDisposeDataSet(ByRef cpobjDS As DataSet)

        'エラーが起きても次の処理へ
        '        On Error Resume Next

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
            'エラー時の処理
            Dim strErrMsg As String = "ＤＡＴＡＳＥＴ廃棄失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & " DataSet名=" & cpobjDS.DataSetName, "DB0013", "DBCLASS", "DBCLASS")
        Finally
            'データセットクリア
            cpobjDS.Clear()
            'データセット廃棄
            cpobjDS.Dispose()
        End Try

    End Sub

    '--------------------------------------------------------------------------------------------------------
    '-　ＤＡＴＡＳＥＴクリア　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ１：対象データセットオブジェクト　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' データセットのクリアを行います
    ''' </summary>
    ''' <param name="cpobjDS">クリアするデータセット</param>
    ''' <remarks></remarks>
    Public Sub psClearDataSet(ByRef cpobjDS As DataSet)

        'エラーが起きても次の処理へ
        '        On Error Resume Next

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
            'エラー時の処理
            Dim strErrMsg As String = "ＤＡＴＡＳＥＴクリア失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & " DataSet名=" & cpobjDS.DataSetName, "DB0013", "DBCLASS", "DBCLASS")
        Finally
            'データセットクリア
            cpobjDS.Clear()
        End Try

    End Sub

    '--------------------------------------------------------------------------------------------------------
    '-　トランザクション開始　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' トランザクションの開始
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function pfDB_BeginTrans() As Boolean

        '戻り値　設定（失敗）
        pfDB_BeginTrans = False
        Try
            If mobjDB.State = ConnectionState.Open Then
                'トランザクション開始
                mobjDBCM = mobjDB.CreateCommand
                '                mobjDBCM.Transaction = mobjDB.BeginTransaction
                '                mobjDBCM.Transaction = mobjDB.BeginTransaction(IsolationLevel.Chaos)
                '                mobjDBCM.Transaction = mobjDB.BeginTransaction(IsolationLevel.ReadCommitted)
                '                mobjDBCM.Transaction = mobjDB.BeginTransaction(IsolationLevel.ReadUncommitted)
                mobjDBCM.Transaction = mobjDB.BeginTransaction(IsolationLevel.RepeatableRead)
                '                mobjDBCM.Transaction = mobjDB.BeginTransaction(IsolationLevel.Unspecified)
                '戻り値　設定（成功）
                pfDB_BeginTrans = True
            End If
        Catch ex As Exception
            'エラー時の処理
            Dim strErrMsg As String = "トランザクション失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg, "DB0014", "DBCLASS", "DBCLASS")
        End Try

    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　ＳＱＬ文　実行　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ１：実行ＳＱＬ文　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' ＳＱＬ文を実行します　事前にコネクション・トランザクションの確認をしてください
    ''' </summary>
    ''' <param name="ipstrSQL">実行するＳＱＬ文</param>
    ''' <param name="cpstrMessageCode">エラーコード</param>
    ''' <returns>処理件数が帰る　失敗した場合は処理件数は-1</returns>
    ''' <remarks></remarks>
    Public Function pfDB_ExecuteSQL(ByVal ipstrSQL As String, Optional ByRef cpstrMessageCode As String = "") As Integer

        '変数定義
        Dim intRetCnt As Integer '処理件数

        '戻り値　設定
        pfDB_ExecuteSQL = -1
        'エラートラップ　開始
        Try
            'ＳＱＬコマンド発行
            'Dim i As Integer = Integer.Parse("dddd")
            mobjDBCM.CommandText = ipstrSQL
            'ＳＱＬコマンド　実行（処理件数　保管）
            intRetCnt = mobjDBCM.ExecuteNonQuery()
            pfDB_ExecuteSQL = intRetCnt
        Catch ex As SqlException
            'エラー発生時の処理
            Dim strErrMsg As String = "ＳＱＬ実行失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & " SQL=" & ipstrSQL, "DB0015", "DBCLASS", "DBCLASS")
            Return -1
        Catch ex As Exception
            'エラー発生時の処理
            Dim strErrMsg As String = "ＳＱＬ実行失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & " SQL=" & ipstrSQL, "DB0016", "DBCLASS", "DBCLASS")
            Return -1
        End Try

    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　トランザクション　コミット　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' トランザクションをコミットします
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function pfDB_CommitTrans() As Boolean

        '戻り値　設定（失敗）
        pfDB_CommitTrans = False
        'エラートラップ開始
        Try
            'コミット発行
            mobjDBCM.Transaction.Commit()
            '戻り値　設定（成功）
            pfDB_CommitTrans = True
        Catch ex As Exception
            'エラー発生時の処理
            Dim strErrMsg As String = "コミット失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg, "DB0017", "DBCLASS", "DBCLASS")
            'ロールバック発行
            Call psDB_Rollback()
        End Try

    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　トランザクション　ロールバック　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' トランザクションをロールバックします
    ''' </summary>
    ''' <remarks></remarks>

    Public Sub psDB_Rollback()

        '        On Error Resume Next
        Try
            mobjDBCM.Transaction.Rollback()
        Catch ex As Exception
            'エラー発生時の処理
            Dim strErrMsg As String = "ロールバック失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg, "DB0018", "DBCLASS", "DBCLASS")
        End Try

    End Sub

    '--------------------------------------------------------------------------------------------------------
    '-　ＤＢ切り離し　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ１：実行ＳＱＬ文　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　修正　接続してないのにＣＬＯＳＥするとワーニングが表示されるので接続の判断を追加　ＢＹ　伯野　　　　-
    '-　　　　2009/04/15　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' データベースのクローズ・破棄を行います
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub psDB_Close()

        'On Error Resume Next

        Try
            If mobjDB Is Nothing Then
            Else
                If mobjDB.State = ConnectionState.Open Then
                    ' データベース接続を閉じる (正しくは オブジェクトの破棄を保証する を参照)
                    mobjDB.Close()
                End If
                mobjDB.Dispose()
            End If
            'mclsCom = Nothing
            'mclsLog = Nothing
        Catch ex As Exception
            'エラー発生時の処理
            Dim strErrMsg As String = "ＤＢクローズ失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg, "DB0019", "DBCLASS", "DBCLASS")
        End Try

    End Sub

    '--------------------------------------------------------------------------------------------------------
    '-　接続文字列生成　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ１：接続文字列　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    '<System.Diagnostics.DebuggerStepThrough()> _
    Private Function fCreateConnectstring(ByRef opstrString As String) As Boolean

        Dim strDBConStr As String = ""

        fCreateConnectstring = False
        '"Data Source=WIN-762HDJPIIDA\NGCSPCDB;Initial Catalog=HANOCOMDAT;User ID=BBB"
        Try
            With mtypConStr
                'If String.IsNullOrEmpty(.Provider) = False Then
                '    strDBConStr &= "Provider=" & .Provider & ";"
                'End If
                'If String.IsNullOrEmpty(.Driver) = False Then
                '    strDBConStr &= "Driver = " & .Driver & ";"
                'End If
                If String.IsNullOrEmpty(.DataSource) = False Then
                    strDBConStr &= "Data Source=" & .DataSource & ";"
                End If
                'If String.IsNullOrEmpty(.Serv) = False Then
                '    strDBConStr &= "Server=" & .Serv & ";"
                'End If
                'If String.IsNullOrEmpty(.System) = False Then
                '    strDBConStr &= "System=" & .System & ";"
                'End If
                'If String.IsNullOrEmpty(.DSN) = False Then
                '    strDBConStr &= "DSN = " & .DSN & ";"
                'End If
                If String.IsNullOrEmpty(.UserID) = False Then
                    strDBConStr &= "User ID=" & .UserID & ";"
                End If
                If String.IsNullOrEmpty(.Password) = False Then
                    strDBConStr &= "Password=" & .Password & ";"
                End If
                'If String.IsNullOrEmpty(.Collect) = False Then
                '    strDBConStr &= "Initial Catalog=" & .Collect & ";"
                'End If
                'If String.IsNullOrEmpty(.Port) = False Then
                '    strDBConStr &= "Port = " & .Port & ";"
                'End If
                'If String.IsNullOrEmpty(.Dbq) = False Then
                '    strDBConStr &= "dbq= " & .Dbq & ";"
                'End If
                'If String.IsNullOrEmpty(.DftPkgLib) = False Then
                '    strDBConStr &= "dftpkglib= " & .DftPkgLib & ";"
                'End If
                'If String.IsNullOrEmpty(.LanguageID) = False Then
                '    strDBConStr &= "languageid= " & .LanguageID & ";"
                'End If
                'If String.IsNullOrEmpty(.Pkg) = False Then
                '    strDBConStr &= "pkg= " & .Pkg & ";"
                'End If

            End With

            opstrString = strDBConStr

            If opstrString = "" Then
                fCreateConnectstring = False
            Else
                fCreateConnectstring = True
            End If
        Catch ex As Exception
            Dim strErrMsg As String = "接続文字列生成　失敗[" & ex.Source & "]" & ex.Message
            Call mclsLog.wrtLog(strErrMsg & " 生成された文字列=" & strDBConStr, "DB0020", "DBCLASS", "DBCLASS")
            opstrString = ""
        End Try

    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　ＤＢ接続文字列用ＸＭＬファイル読込み　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　本来は別クラスにするべきだが、データセット形式で読み書きするため、ここに記述　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ１：入出力ＸＭＬファイル名　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------

    ''' <summary>
    ''' 設定ファイルを読み込みます　ＤＢクラスを使用する場合は、この処理を事前に行うこと
    ''' </summary>
    ''' <param name="ipstrXML">設定ファイルパス（絶対パス）</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Diagnostics.DebuggerStepThrough()> _
    Public Function pfXML_Read(ByRef ipstrXML As String) As Boolean

        '変数定義
        Dim strDirName As String = ""
        Dim strFileName As String = ""
        Dim mobjXMLDS As DataSet                '接続文字列文構造体保管用データセット

        '戻り値初期化
        pfXML_Read = False

        Try
            'ファイル名を保管
            mstrXML = ipstrXML
            mobjXMLDS = New DataSet
            'フルパスファイル名をフォルダ名とファイル名に分割
            'Call mclsCom.SplitFF(mstrXML, strDirName, strFileName)

            '処理開始
            'ＸＭＬファイルが存在するとき
            If System.IO.File.Exists(mstrXML) = True Then
                'ＸＭＬファイルをデータセットに保管
                mobjXMLDS.ReadXml(mstrXML)
                'ＸＭＬファイルが存在しないとき
            Else
                'データテーブルを生成

                Dim objWKDT As DataTable
                objWKDT = mobjXMLDS.Tables.Add("0")
                objWKDT.Columns.Add("Provider", Type.GetType("System.String"))
                objWKDT.Columns.Add("Driver", Type.GetType("System.String"))
                objWKDT.Columns.Add("DataSource", Type.GetType("System.String"))
                objWKDT.Columns.Add("Serv", Type.GetType("System.String"))
                objWKDT.Columns.Add("DSN", Type.GetType("System.String"))
                objWKDT.Columns.Add("UserID", Type.GetType("System.String"))
                objWKDT.Columns.Add("Password", Type.GetType("System.String"))
                objWKDT.Columns.Add("Collect", Type.GetType("System.String"))
                objWKDT.Columns.Add("Port", Type.GetType("System.String"))
                objWKDT.Columns.Add("System", Type.GetType("System.String"))
                objWKDT.Columns.Add("Dbq", Type.GetType("System.String"))
                objWKDT.Columns.Add("DftPkgLib", Type.GetType("System.String"))
                objWKDT.Columns.Add("LanguageID", Type.GetType("System.String"))
                objWKDT.Columns.Add("Pkg", Type.GetType("System.String"))
                objWKDT.Columns.Add("Schema", Type.GetType("System.String"))
                '新規レコードを生成
                Dim objWKROW As DataRow
                objWKROW = objWKDT.NewRow
                objWKROW("Provider") = ""
                objWKROW("Driver") = ""
                objWKROW("DataSource") = ""
                objWKROW("Serv") = ""
                objWKROW("DSN") = ""
                objWKROW("UserID") = ""
                objWKROW("Password") = ""
                objWKROW("Collect") = ""
                objWKROW("Port") = ""
                objWKROW("System") = ""
                objWKROW("Dbq") = ""
                objWKROW("DftPkgLib") = ""
                objWKROW("LanguageID") = ""
                objWKROW("Pkg") = ""
                objWKROW("Schema") = ""
                'データテーブルにレコード追加
                objWKDT.Rows.Add(objWKROW)
            End If

            'データセットにレコードが存在するとき
            If mobjXMLDS.Tables(0).Rows.Count > 0 Then
                'プロパティに個々の値を設定
                With mtypConStr
                    .Provider = mobjXMLDS.Tables(0).Rows(0)(0).ToString
                    .Driver = mobjXMLDS.Tables(0).Rows(0)(1).ToString
                    .DataSource = mobjXMLDS.Tables(0).Rows(0)(2).ToString
                    .Serv = mobjXMLDS.Tables(0).Rows(0)(3).ToString
                    .DSN = mobjXMLDS.Tables(0).Rows(0)(4).ToString
                    .UserID = mobjXMLDS.Tables(0).Rows(0)(5).ToString
                    .Password = mobjXMLDS.Tables(0).Rows(0)(6).ToString
                    .Collect = mobjXMLDS.Tables(0).Rows(0)(7).ToString
                    .Port = mobjXMLDS.Tables(0).Rows(0)(8).ToString
                    .System = mobjXMLDS.Tables(0).Rows(0)(9).ToString
                    .Dbq = mobjXMLDS.Tables(0).Rows(0)(10).ToString
                    .DftPkgLib = mobjXMLDS.Tables(0).Rows(0)(11).ToString
                    .LanguageID = mobjXMLDS.Tables(0).Rows(0)(12).ToString
                    .Pkg = mobjXMLDS.Tables(0).Rows(0)(13).ToString
                    .Schema = mobjXMLDS.Tables(0).Rows(0)(14).ToString
                End With
            End If

            'データセットを破棄
            Call psDisposeDataSet(mobjXMLDS)

            pfXML_Read = True

        Catch ex As Exception
            'エラー発生時の処理
            Dim strErrMsg As String = "ＸＭＬファイル読込み失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & " XMLFILE=" & ipstrXML, "DB0021", "DBCLASS", "DBCLASS")
        End Try


    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　ＤＢ接続文字列用ＸＭＬファイル出力　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　本来は別クラスにするべきだが、データセット形式で読み書きするため、ここに記述　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' 設定ファイルの書き込みを行います
    ''' </summary>
    ''' <param name="ipstrXML">設定ファイルのパス（絶対パス）</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function pfXML_Write(ByRef ipstrXML As String) As Boolean

        Dim objSW As IO.StreamWriter
        Dim strDirName As String = ""
        Dim strFileName As String = ""
        Dim mobjXMLDS As DataSet                '接続文字列文構造体保管用データセット

        '戻り値　設定
        pfXML_Write = True

        'ファイル名を保管
        mstrXML = ipstrXML

        'Call mclsCom.SplitFF(mstrXML, strDirName, strFileName)

        Try
            If IO.Directory.Exists(mstrLogDir & "\CNFG") = False Then
                'フォルダの新規作成
                My.Computer.FileSystem.CreateDirectory(mstrLogDir & "\CNFG")
            End If
        Catch ex As Exception
            'Call mclsLog.wrtLog(ex.Message & ex.StackTrace)
            Return False
        End Try

        Try

            mobjXMLDS = New DataSet
            'フルパスファイル名をフォルダ名とファイル名に分割
            'Call mclsCom.SplitFF(mstrXML, strDirName, strFileName)

            '処理開始
            'ＸＭＬファイルが存在するとき
            If System.IO.File.Exists(mstrXML) = True Then
                'ＸＭＬファイルをデータセットに保管
                mobjXMLDS.ReadXml(mstrXML)
                'ＸＭＬファイルが存在しないとき
            Else
                'データテーブルを生成
                Dim objWKDT As DataTable
                objWKDT = mobjXMLDS.Tables.Add("0")
                objWKDT.Columns.Add("Provider", Type.GetType("System.String"))
                objWKDT.Columns.Add("Driver", Type.GetType("System.String"))
                objWKDT.Columns.Add("DataSource", Type.GetType("System.String"))
                objWKDT.Columns.Add("Serv", Type.GetType("System.String"))
                objWKDT.Columns.Add("DSN", Type.GetType("System.String"))
                objWKDT.Columns.Add("UserID", Type.GetType("System.String"))
                objWKDT.Columns.Add("Password", Type.GetType("System.String"))
                objWKDT.Columns.Add("Collect", Type.GetType("System.String"))
                objWKDT.Columns.Add("Port", Type.GetType("System.String"))
                objWKDT.Columns.Add("System", Type.GetType("System.String"))
                objWKDT.Columns.Add("Dbq", Type.GetType("System.String"))
                objWKDT.Columns.Add("DftPkgLib", Type.GetType("System.String"))
                objWKDT.Columns.Add("LanguageID", Type.GetType("System.String"))
                objWKDT.Columns.Add("Pkg", Type.GetType("System.String"))
                objWKDT.Columns.Add("Schema", Type.GetType("System.String"))
                '新規レコードを生成
                Dim objWKROW As DataRow
                objWKROW = objWKDT.NewRow
                objWKROW("Provider") = ""
                objWKROW("Driver") = ""
                objWKROW("DataSource") = ""
                objWKROW("Serv") = ""
                objWKROW("DSN") = ""
                objWKROW("UserID") = ""
                objWKROW("Password") = ""
                objWKROW("Collect") = ""
                objWKROW("Port") = ""
                objWKROW("System") = ""
                objWKROW("Dbq") = ""
                objWKROW("DftPkgLib") = ""
                objWKROW("LanguageID") = ""
                objWKROW("Pkg") = ""
                objWKROW("Schema") = ""
                'データテーブルに空レコード追加
                objWKDT.Rows.Add(objWKROW)
            End If

            '１レコード目に差新の情報を保管
            If mobjXMLDS.Tables(0).Rows.Count > 0 Then
                With mtypConStr
                    mobjXMLDS.Tables(0).Rows(0)(0) = .Provider
                    mobjXMLDS.Tables(0).Rows(0)(1) = .Driver
                    mobjXMLDS.Tables(0).Rows(0)(2) = .DataSource
                    mobjXMLDS.Tables(0).Rows(0)(3) = .Serv
                    mobjXMLDS.Tables(0).Rows(0)(4) = .DSN
                    mobjXMLDS.Tables(0).Rows(0)(5) = .UserID
                    mobjXMLDS.Tables(0).Rows(0)(6) = .Password
                    mobjXMLDS.Tables(0).Rows(0)(7) = .Collect
                    mobjXMLDS.Tables(0).Rows(0)(8) = .Port
                    mobjXMLDS.Tables(0).Rows(0)(9) = .System
                    mobjXMLDS.Tables(0).Rows(0)(10) = .Dbq
                    mobjXMLDS.Tables(0).Rows(0)(11) = .DftPkgLib
                    mobjXMLDS.Tables(0).Rows(0)(12) = .LanguageID
                    mobjXMLDS.Tables(0).Rows(0)(13) = .Pkg
                    mobjXMLDS.Tables(0).Rows(0)(14) = .Schema
                End With
            End If

            'ＸＭＬファイルをして出力
            objSW = New IO.StreamWriter(mstrXML, False, System.Text.Encoding.Default)
            mobjXMLDS.WriteXml(objSW)
            objSW.Close()
            objSW.Dispose()

            Call psDisposeDataSet(mobjXMLDS)

        Catch ex As Exception
            'エラー発生時の処理
            Dim strErrMsg As String = "ＸＭＬファイル書出し失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & " XMLFILE=" & mstrXML, "DB0021", "DBCLASS", "DBCLASS")
        End Try

    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　エラーメッセージ保管配列の初期化　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    Private Sub sInitErrMsg()

        Try
            mlngErrCnt = 0
            ReDim mstrErr(mlngErrCnt)
            mstrErr(0) = "Dummy"
        Catch ex As Exception
            'エラー発生時の処理
            Call sAddErrMsg("エラーメッセージクリア失敗[" & ex.Source & "]" & ex.Message)
        End Try

    End Sub

    '--------------------------------------------------------------------------------------------------------
    '-　要求されたテーブルに該当するデータがあるかどうか
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' 要求されたテーブルに該当するデータがあるかどうか
    ''' </summary>
    ''' <param name="strSQL">データを検索するＳＱＬ分</param>
    ''' <returns>データあり：True データなし:False </returns>
    ''' <remarks></remarks>
    Public Function pfExistData(ByVal strSQL As String) As Boolean
        Dim ds As New DataSet
        Try
            If pfDB_CreateDataSet(ds, strSQL) = False Then
                Return False
            Else
                If ds.Tables(0).Rows.Count = 0 Then
                    Return False
                Else
                    Return True
                End If
            End If
        Catch ex As Exception
            Dim strErrMsg As String = "データ存在判定　失敗[" & ex.Source & "]" & ex.Message
            Call mclsLog.wrtLog(strErrMsg & " SQL=" & strSQL, "DB0022", "DBCLASS", "DBCLASS")
            Return False
        Finally
            psDisposeDataSet(ds)
        End Try
    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　指定したＳＱＬでデータがあるかどうか確認同時に値取得
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' 指定したＳＱＬでデータがあるかどうか確認同時に値取得 取得する値はＳＱＬ内の先頭のカラムです
    ''' </summary>
    ''' <param name="ipstrSQL">データをチェックするＳＱＬ</param>
    ''' <param name="cpblnExist">データが存在するどうかのチェック</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function pfDB_ReadName(ByVal ipstrSQL As String, ByRef cpblnExist As Boolean) As String

        pfDB_ReadName = String.Empty

        Dim objDBCM As SqlCommand
        '2012/09/08修正
        Dim dr As SqlDataReader = Nothing
        Dim blnOrgConn As Boolean = False

        Try
            If mobjDB.State = ConnectionState.Broken Or mobjDB.State = ConnectionState.Closed Then
                Call pfDB_Connect()
                blnOrgConn = True
            End If
            objDBCM = New SqlCommand(ipstrSQL, mobjDB)
            dr = objDBCM.ExecuteReader

            If dr.HasRows = False Then
                cpblnExist = False
                Exit Function
            End If
            cpblnExist = True
            dr.Read()
            pfDB_ReadName = dr.Item(0).ToString
            'dr.Close()
        Catch ex As Exception
            'エラー時の処理
            Dim strErrMsg As String = "データリーダー作成失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & " SQL=" & ipstrSQL, "DB0023", "DBCLASS", "DBCLASS")
        Finally
            '2012/09/07追加
            If dr Is Nothing Then
            Else
                If dr.IsClosed = True Then
                Else
                    dr.Close()
                End If
            End If
            If mobjDB.State = ConnectionState.Open Then
                If blnOrgConn Then
                    mobjDB.Close()
                End If
            End If
        End Try

    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　指定したＳＱＬでデータがあるかどうか確認同時に値取得
    '--------------------------------------------------------------------------------------------------------
    Public Function pfDB_CreateDataRow(ByVal ipstrSQL As String) As Object()

        pfDB_CreateDataRow = Nothing
        Dim objDBCM As SqlCommand
        Dim dr As SqlDataReader
        Try
            Call pfDB_Connect()
            objDBCM = New SqlCommand(ipstrSQL, mobjDB)
            dr = objDBCM.ExecuteReader(CommandBehavior.KeyInfo)

            If dr.HasRows = False Then
                pfDB_CreateDataRow = Nothing
                Exit Function
            End If
            Using dr
                dr.Read()
                Dim reader(dr.FieldCount - 1) As Object
                dr.GetValues(reader)
                Return reader
            End Using
            'cpblnExist = True
            'dr.Read()
            'pfDB_ReadName = dr.Item(0).ToString
            dr.Close()
        Catch ex As Exception
            'エラー時の処理
            Dim strErrMsg As String = "データリーダー作成失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & " SQL=" & ipstrSQL, "DB0024", "DBCLASS", "DBCLASS")
        Finally
            If mobjDB.State = ConnectionState.Open Then
                mobjDB.Close()
            End If
        End Try

    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　データ存在チェック
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' データの存在をチェックします
    ''' </summary>
    ''' <param name="ipstrSQL">検索するＳＱＬ</param>
    ''' <returns>成功（データがある）：True　失敗（データがない）：False </returns>
    ''' <remarks></remarks>
    Public Function pfDB_CheckExistDat(ByVal ipstrSQL As String) As Boolean

        Dim objDBCM As SqlCommand
        Dim dr As SqlDataReader
        Try
            Call pfDB_Connect()
            objDBCM = New SqlCommand(ipstrSQL, mobjDB)
            dr = objDBCM.ExecuteReader

            If dr.HasRows = False Then
                pfDB_CheckExistDat = False
            Else
                pfDB_CheckExistDat = True
            End If
            dr.Close()
        Catch ex As Exception
            'エラー時の処理
            Dim strErrMsg As String = "データリーダー作成失敗[" & ex.Source & "]" & ex.Message
            Call sAddErrMsg(strErrMsg)
            Call mclsLog.wrtLog(strErrMsg & " SQL=" & ipstrSQL, "DB0025", "DBCLASS", "DBCLASS")
        Finally
            If mobjDB.State = ConnectionState.Open Then
                mobjDB.Close()
            End If
        End Try
    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　エラーメッセージ保管配列にデータを追加　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    Private Sub sAddErrMsg(ByVal ipstrErrMsg As String)

        Try
            mlngErrCnt += 1
            ReDim Preserve mstrErr(mlngErrCnt)
            mstrErr(mlngErrCnt) = ipstrErrMsg
            'mclsLog.wrtLog(ControlChars.Tab & ipstrErrMsg.Trim)
        Catch ex As Exception
            'エラー発生時の処理
            '            Call sAddErrMsg("エラーメッセージ追加失敗[" & ex.Source & "]" & ex.Message)
            Dim log As New EventLog("OLEDB DLL", My.Computer.Name, "sAddErrMsg")
            log.WriteEntry("エラーメッセージ追加失敗[" & ex.Source & "]" & ex.Message, EventLogEntryType.Warning, 123)
        End Try

    End Sub

    '--------------------------------------------------------------------------------------------------------
    '-　インラインＤｉｓｐｏｓｅの内部処理　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　修正　接続してないのにＣＬＯＳＥするとワーニングが表示されるので接続の判断を追加　ＢＹ　伯野　　　　-
    '-　　　　2009/04/15　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing = True Then
                ' TODO: 明示的に呼び出されたときにマネージ リソースを解放します
            End If
            ' TODO: 共有のアンマネージ リソースを解放します
            Try
                If mobjDB Is Nothing Then
                Else
                    If mobjDB.State = ConnectionState.Open Then
                        mobjDB.Close()
                    End If
                    mobjDB.Dispose()
                End If
            Catch ex As Exception
                'ＤＢ接続を破棄中にエラーが起こるということはまともに繋がっていないので失敗しても問題Ｎｏｔｈｉｎｇ
                'したがって、あえて何もしない
            End Try
            Try
                If mobjDBX Is Nothing Then
                Else
                    If mobjDBX.State = ConnectionState.Open Then
                        mobjDBX.Close()
                    End If
                    mobjDBX.Dispose()
                End If
            Catch ex As Exception
                'ＤＢ接続を破棄中にエラーが起こるということはまともに繋がっていないので失敗しても問題Ｎｏｔｈｉｎｇ
                'したがって、あえて何もしない
            End Try
            Try
                mclsCom = Nothing '共通関数？ＤＬＬ
            Catch ex As Exception

            End Try
        End If
        Me.disposedValue = True
    End Sub



#Region "スキーマ名 構造体"
    '--------------------------------------------------------------------
    '-スキーマ名  
    '--------------------------------------------------------------------
    Structure SchemaName
        ''' <summary>
        ''' GRANDIT
        ''' </summary>
        ''' <remarks></remarks>
        Public DB_BI As String
        ''' <summary>
        ''' GRANDIT
        ''' </summary>
        ''' <remarks></remarks>
        Public DB_CM As String
        ''' <summary>
        ''' GRANDIT
        ''' </summary>
        ''' <remarks></remarks>
        Public DB_ET As String
        ''' <summary>
        ''' GRANDIT
        ''' </summary>
        ''' <remarks></remarks>
        Public DB_FI As String
        ''' <summary>
        ''' GRANDIT
        ''' </summary>
        ''' <remarks></remarks>
        Public DB_HR As String
        ''' <summary>
        ''' GRANDIT
        ''' </summary>
        ''' <remarks></remarks>
        Public DB_SC As String
        ''' <summary>
        ''' 修理
        ''' </summary>
        ''' <remarks></remarks>
        Public DB_RP As String
    End Structure

    ''' <summary>
    ''' NEQにおけるスキーマ名
    ''' </summary>
    ''' <remarks></remarks>
    Public DBName As New SchemaName  'NEQスキーマ名

    ''' <summary>
    ''' DBスキーマ名取得
    ''' </summary>
    ''' <param name="strConfig">設定ファイルのマップパス</param>
    ''' <remarks></remarks>
    ''' <returns>成功＝True、失敗=False</returns>
    Public Function msComFnc_Init(ByVal strConfig As String) As Boolean
        msComFnc_Init = False
        Try
            Dim strDummFileNM As String = ""
            'Dim clsDb As New SQL_DBCLS_LIB.ClsSQLSvrDB
            ' Dim clsPage As New System.Web.UI.Page

            Using wrkDs As New DataSet
                If IO.File.Exists(strConfig) = True Then
                    wrkDs.ReadXml(strConfig)
                Else
                    Exit Function
                End If

                If wrkDs.Tables("SchemaName").Rows.Count <> 0 Then
                    With wrkDs.Tables("SchemaName").Rows(0)
                        DBName.DB_BI = .Item("BIDB").ToString()
                        DBName.DB_CM = .Item("CMDB").ToString()
                        DBName.DB_ET = .Item("ETDB").ToString()
                        DBName.DB_FI = .Item("FIDB").ToString()
                        DBName.DB_HR = .Item("HRDB").ToString()
                        DBName.DB_SC = .Item("SCDB").ToString()
                        DBName.DB_RP = .Item("RPDB").ToString()
                    End With
                Else
                    Exit Function
                End If
            End Using

            'clsPage.Session("Config") = strConfig
            msComFnc_Init = True
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region


    'ここから　先はさわってはいけません。
    '--------------------------------------------------------------------------------------------------------
    '-　インラインＤｉｓｐｏｓｅの定義　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　触ると不幸になります。　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
#Region " IDisposable Support "

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose

        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)

    End Sub

#End Region

End Class
