'********************************************************************************************************************************
'*　システム　：　サポートセンタステム
'*　処理名　　：　データベース接続クラス
'*　ＰＧＭＩＤ：　ClsCMDataLink
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.11.13　：　土岐
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
Imports System.Data.SqlClient
Imports System.Data

#End Region

Public Class ClsCMDataLink
#Region "継承定義"
    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
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
    Private Shared objStack As StackFrame
    '--------------------------------
    '2014/04/14 星野　ここまで
    '--------------------------------
    Private Shared clsDataConnect As New ClsCMDataConnect

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
    ''' 営業所名取得
    ''' </summary>
    ''' <param name="ipstrBranchCD">営業所コード</param>
    ''' <returns>営業所名</returns>
    ''' <remarks></remarks>
    Public Shared Function pfGet_BranchNm(ByVal ipstrBranchCD As String) As String
        Dim dstOrders As DataSet = Nothing
        pfGet_Branch(ipstrBranchCD, dstOrders)
        If dstOrders.Tables.Count > 0 Then
            pfGet_BranchNm = dstOrders.Tables(0).Rows(0).Item("営業所名").ToString()
        Else
            pfGet_BranchNm = String.Empty
        End If
    End Function

    ''' <summary>
    ''' 営業所整合性チェック
    ''' </summary>
    ''' <param name="ipstrBranchCD">営業所コード</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Public Shared Function pfCheck_Branch(ByVal ipstrBranchCD As String) As Boolean
        Return pfGet_Branch(ipstrBranchCD, Nothing)
    End Function

    ''' <summary>
    ''' 会社名取得
    ''' </summary>
    ''' <param name="ipstrCompCD">会社コード</param>
    ''' <returns>会社名</returns>
    ''' <remarks></remarks>
    Public Shared Function pfGet_CompNm(ByVal ipstrCompCD As String) As String
        Dim dstOrders As DataSet = Nothing
        pfGet_Branch(ipstrCompCD, dstOrders)
        If dstOrders.Tables.Count > 0 Then
            pfGet_CompNm = dstOrders.Tables(0).Rows(0).Item("会社名").ToString()
        Else
            pfGet_CompNm = String.Empty
        End If
    End Function

    ''' <summary>
    ''' 会社略称取得
    ''' </summary>
    ''' <param name="ipstrCompCD">会社コード</param>
    ''' <returns>会社略称</returns>
    ''' <remarks></remarks>
    Public Shared Function pfGet_CompShortNm(ByVal ipstrCompCD As String) As String
        Dim dstOrders As DataSet = Nothing
        pfGet_Branch(ipstrCompCD, dstOrders)
        If dstOrders.Tables.Count > 0 Then
            pfGet_CompShortNm = dstOrders.Tables(0).Rows(0).Item("会社略称").ToString()
        Else
            pfGet_CompShortNm = String.Empty
        End If
    End Function

    ''' <summary>
    ''' 会社整合性チェック
    ''' </summary>
    ''' <param name="ipstrCompCD">会社コード</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Public Shared Function pfCheck_Comp(ByVal ipstrCompCD As String) As Boolean
        Return pfGet_Comp(ipstrCompCD, Nothing)
    End Function

    ''' <summary>
    ''' 代理店名取得
    ''' </summary>
    ''' <param name="ipstrAgencyCD">代理店コード</param>
    ''' <returns>代理店名</returns>
    ''' <remarks></remarks>
    Public Shared Function pfGet_AgencyNm(ByVal ipstrAgencyCD As String) As String
        Dim dstOrders As DataSet = Nothing
        pfGet_Branch(ipstrAgencyCD, dstOrders)
        If dstOrders.Tables.Count > 0 Then
            pfGet_AgencyNm = dstOrders.Tables(0).Rows(0).Item("代理店名").ToString()
        Else
            pfGet_AgencyNm = String.Empty
        End If
    End Function

    ''' <summary>
    ''' 代理略称取得
    ''' </summary>
    ''' <param name="ipstrAgencyCD">代理店コード</param>
    ''' <returns>代理店略称</returns>
    ''' <remarks></remarks>
    Public Shared Function pfGet_AgencyShortNm(ByVal ipstrAgencyCD As String) As String
        Dim dstOrders As DataSet = Nothing
        pfGet_Agency(ipstrAgencyCD, dstOrders)
        If dstOrders.Tables.Count > 0 Then
            pfGet_AgencyShortNm = dstOrders.Tables(0).Rows(0).Item("代理店略称").ToString()
        Else
            pfGet_AgencyShortNm = String.Empty
        End If
    End Function

    ''' <summary>
    ''' 代理店整合性チェック
    ''' </summary>
    ''' <param name="ipstrAgencyCD">代理店コード</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Public Shared Function pfCheck_pfGet_Agency(ByVal ipstrAgencyCD As String) As Boolean
        Return pfGet_Agency(ipstrAgencyCD, Nothing)
    End Function

    ''' <summary>
    ''' 社員姓取得
    ''' </summary>
    ''' <param name="ipstrEmployeeCD">社員コード</param>
    ''' <returns>社員姓</returns>
    ''' <remarks></remarks>
    Public Shared Function pfGet_LastName(ByVal ipstrEmployeeCD As String) As String
        Dim dstOrders As DataSet = Nothing
        pfGet_Employee(ipstrEmployeeCD, dstOrders)
        If dstOrders.Tables.Count > 0 Then
            pfGet_LastName = dstOrders.Tables(0).Rows(0).Item("社員姓").ToString()
        Else
            pfGet_LastName = String.Empty
        End If

    End Function

    ''' <summary>
    ''' 社員名取得
    ''' </summary>
    ''' <param name="ipstrEmployeeCD">社員コード</param>
    ''' <returns>社員名</returns>
    ''' <remarks></remarks>
    Public Shared Function pfGet_FirstName(ByVal ipstrEmployeeCD As String) As String
        Dim dstOrders As DataSet = Nothing
        pfGet_Employee(ipstrEmployeeCD, dstOrders)
        If dstOrders.Tables.Count > 0 Then
            pfGet_FirstName = dstOrders.Tables(0).Rows(0).Item("社員名").ToString()
        Else
            pfGet_FirstName = String.Empty
        End If

    End Function

    ''' <summary>
    ''' 社員整合性チェック
    ''' </summary>
    ''' <param name="ipstrEmployeeCD">社員コード</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Public Shared Function pfCheck_Employee(ByVal ipstrEmployeeCD As String) As Boolean
        Return pfGet_Employee(ipstrEmployeeCD, Nothing)
    End Function

    ''' <summary>
    ''' 営業所名取得、整合性チェック
    ''' </summary>
    ''' <param name="ipstrBranchCD">営業所コード</param>
    ''' <param name="opdstBranchNM">営業所名</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Public Shared Function pfGet_Branch(ByVal ipstrBranchCD As String, ByRef opdstBranchNM As DataSet) As Boolean
        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim strOKNG As String
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '初期化
        conDB = Nothing
        strOKNG = Nothing
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("ZCMPSEL003", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("branch_cd", SqlDbType.NVarChar, ipstrBranchCD))
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                End With

                'データ取得
                opdstBranchNM = clsDataConnect.pfGet_DataSet(cmdDB)

                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "1"
                        '整合性OK
                        pfGet_Branch = True
                    Case Else
                        '整合性エラー
                        pfGet_Branch = False
                End Select
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
                pfGet_Branch = False
            Finally
                'DB切断
                clsDataConnect.pfOpen_Database(conDB)
            End Try
        Else
            pfGet_Branch = False
        End If

    End Function

    ''' <summary>
    ''' 会社名取得、整合性チェック
    ''' </summary>
    ''' <param name="ipstrCompCD">会社コード</param>
    ''' <param name="opdstCompNM">会社名, 会社略称</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Public Shared Function pfGet_Comp(ByVal ipstrCompCD As String, ByRef opdstCompNM As DataSet) As Boolean
        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim strOKNG As String
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '初期化
        conDB = Nothing
        strOKNG = Nothing
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("ZCMPSEL004", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("comp_cd", SqlDbType.NVarChar, ipstrCompCD))
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                End With

                'データ取得
                opdstCompNM = clsDataConnect.pfGet_DataSet(cmdDB)

                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "1"
                        '整合性OK
                        pfGet_Comp = True
                    Case Else
                        '整合性エラー
                        pfGet_Comp = False
                End Select
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
                pfGet_Comp = False
            Finally
                'DB切断
                clsDataConnect.pfOpen_Database(conDB)
            End Try
        Else
            pfGet_Comp = False
        End If

    End Function

    ''' <summary>
    ''' 代理店名取得、整合性チェック
    ''' </summary>
    ''' <param name="ipstrCompCD">代理店コード</param>
    ''' <param name="opdstCompNM">代理店名, 代理店略称</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Public Shared Function pfGet_Agency(ByVal ipstrCompCD As String, ByRef opdstCompNM As DataSet) As Boolean
        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim strOKNG As String
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '初期化
        conDB = Nothing
        strOKNG = Nothing
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("ZCMPSEL005", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("agency_cd", SqlDbType.NVarChar, ipstrCompCD))
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                End With

                'データ取得
                opdstCompNM = clsDataConnect.pfGet_DataSet(cmdDB)

                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "1"
                        '整合性OK
                        pfGet_Agency = True
                    Case Else
                        '整合性エラー
                        pfGet_Agency = False
                End Select
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
                pfGet_Agency = False
            Finally
                'DB切断
                clsDataConnect.pfOpen_Database(conDB)
            End Try
        Else
            pfGet_Agency = False
        End If

    End Function

    ''' <summary>
    ''' 社員名取得、整合性チェック
    ''' </summary>
    ''' <param name="ipstrEmployeeCD">社員コード</param>
    ''' <param name="opdstEmployeeNM">社員姓, 社員名</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Public Shared Function pfGet_Employee(ByVal ipstrEmployeeCD As String, ByRef opdstEmployeeNM As DataSet) As Boolean
        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim strOKNG As String
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '初期化
        conDB = Nothing
        strOKNG = String.Empty
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("ZCMPSEL006", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("employee_cd", SqlDbType.NVarChar, ipstrEmployeeCD))
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                End With

                'データ取得
                opdstEmployeeNM = clsDataConnect.pfGet_DataSet(cmdDB)

                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "1"
                        '整合性OK
                        pfGet_Employee = True
                    Case Else
                        '整合性エラー
                        pfGet_Employee = False
                End Select
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
                pfGet_Employee = False
            Finally
                'DB切断
                clsDataConnect.pfOpen_Database(conDB)
            End Try
        Else
            pfGet_Employee = False
        End If

    End Function

    '--------------------------------
    '2014/07/24 星野　ここから
    '--------------------------------
    ''' <summary>
    ''' 社員名取得、整合性チェック
    ''' </summary>
    ''' <param name="ipstrEmployeeCD">社員コード</param>
    ''' <param name="ipstrCompCD">会社コード</param>
    ''' <param name="ipstrOfficeCD">営業所コード</param>
    ''' <param name="opdstEmployeeNM">社員姓, 社員名</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Public Shared Function pfGet_Employee(ByVal ipstrEmployeeCD As String, ByVal ipstrCompCD As String, ByVal ipstrOfficeCD As String, ByRef opdstEmployeeNM As DataSet) As Boolean
        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim strOKNG As String
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '初期化
        conDB = Nothing
        strOKNG = String.Empty
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("ZCMPSEL047", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("employee_cd", SqlDbType.NVarChar, ipstrEmployeeCD))
                    .Add(pfSet_Param("comp_cd", SqlDbType.NVarChar, ipstrCompCD))
                    .Add(pfSet_Param("office_cd", SqlDbType.NVarChar, ipstrOfficeCD))
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                End With

                'データ取得
                opdstEmployeeNM = clsDataConnect.pfGet_DataSet(cmdDB)

                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "1"
                        '整合性OK
                        pfGet_Employee = True
                    Case Else
                        '整合性エラー
                        pfGet_Employee = False
                End Select
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
                pfGet_Employee = False
            Finally
                'DB切断
                clsDataConnect.pfOpen_Database(conDB)
            End Try
        Else
            pfGet_Employee = False
        End If

    End Function
    '--------------------------------
    '2014/07/24 星野　ここまで
    '--------------------------------

    ''' <summary>
    ''' 社員名取得、整合性チェック
    ''' </summary>
    ''' <param name="ipstrCompCD">会社コード</param>
    ''' <param name="opdstEmployeeNM">社員コード, 社員姓, 社員名</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Public Shared Function pfGet_EmployeeList(ByVal ipstrCompCD As String, ByRef opdstEmployeeNM As DataSet) As Boolean
        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim strOKNG As String
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '初期化
        conDB = Nothing
        strOKNG = String.Empty
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("ZCMPSEL010", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("comp_cd", SqlDbType.NVarChar, ipstrCompCD))
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                End With

                'データ取得
                opdstEmployeeNM = clsDataConnect.pfGet_DataSet(cmdDB)

                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "1"
                        '整合性OK
                        pfGet_EmployeeList = True
                    Case Else
                        '整合性エラー
                        pfGet_EmployeeList = False
                End Select
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
                pfGet_EmployeeList = False
            Finally
                'DB切断
                clsDataConnect.pfOpen_Database(conDB)
            End Try
        Else
            pfGet_EmployeeList = False
        End If

    End Function

    '--------------------------------
    '2014/07/23 星野　ここから
    '--------------------------------
    ''' <summary>
    ''' 社員名取得、整合性チェック
    ''' </summary>
    ''' <param name="ipstrCompCD">会社コード</param>
    ''' <param name="ipstrOfficeCD">営業所コード</param>
    ''' <param name="opdstEmployeeNM">社員コード, 社員姓, 社員名</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Public Shared Function pfGet_EmployeeList(ByVal ipstrCompCD As String, ByVal ipstrOfficeCD As String, ByRef opdstEmployeeNM As DataSet) As Boolean
        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim strOKNG As String
        objStack = New StackFrame

        '初期化
        conDB = Nothing
        strOKNG = String.Empty
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("ZCMPSEL046", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("comp_cd", SqlDbType.NVarChar, ipstrCompCD))
                    .Add(pfSet_Param("office_cd", SqlDbType.NVarChar, ipstrOfficeCD))
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                End With

                'データ取得
                opdstEmployeeNM = clsDataConnect.pfGet_DataSet(cmdDB)

                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "1"
                        '整合性OK
                        pfGet_EmployeeList = True
                    Case Else
                        '整合性エラー
                        pfGet_EmployeeList = False
                End Select
            Catch ex As Exception
                'ログ出力
                psLogWrite("", USER_NAME, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                pfGet_EmployeeList = False
            Finally
                'DB切断
                clsDataConnect.pfOpen_Database(conDB)
            End Try
        Else
            pfGet_EmployeeList = False
        End If

    End Function
    '--------------------------------
    '2014/07/23 星野　ここまで
    '--------------------------------

#End Region

End Class
