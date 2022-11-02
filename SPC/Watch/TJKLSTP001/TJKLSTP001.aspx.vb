'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜監視＞
'*　処理名　　：　定時実行　結果確認
'*　ＰＧＭＩＤ：　TJKLSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2016.02.26　：　伯野
'*  更　新　　：　2017.06.06　：　伯野
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'TJKLSTP001-000     2016/03/25      加賀      バグ修正、実行後の一覧リロード処理追加
'TJKLSTP001-001     2017/06/06      伯野      実行判定の追加

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
#Region "インポート定義"

Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive      '排他制御用
Imports SQL_DBCLS_LIB
            Imports Microsoft.VisualBasic

#End Region


'================================================================================================================================
'=　クラス定義
'================================================================================================================================
Public Class TJKLSTP001


    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
    Inherits System.Web.UI.Page


    '============================================================================================================================
    '=　定数定義
    '============================================================================================================================
    Private Const M_DISP_ID = "TJKLSTP001"


    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
    Dim objStack As StackFrame
    Dim strRetSetf As String   '設定、戻しの判断フラグ
    Dim strSetcd As String     '設定コード
    Dim strRetcd As String     '戻しコード

    Dim mclsDataConnect As New ClsCMDataConnect
    Dim mclsCMDBC As New ClsCMDBCom
    Dim mclsExc As New ClsCMExclusive
    Dim mclsCmm As New ClsCMCommon
    Dim mclsDB As New ClsSQLSvrDB
    Dim mclsKSIMST As New clsKessaiMST
    Dim mstbRetryNo As New StringBuilder


    '============================================================================================================================
    '=　イベントプロシージャ定義
    '============================================================================================================================

    '----------------------------------------------------------------------------------------------------------------
    '-　ページ制御
    '----------------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' ページ　初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub TJKLSTP001_Init(sender As Object, e As EventArgs) Handles Me.Init

        ClsCMCommon.pfSet_GridView(Me.grvList1, M_DISP_ID, "L", 30, 10)
        ClsCMCommon.pfSet_GridView(Me.grvList2, M_DISP_ID, "L", 30, 10)
        ClsCMCommon.pfSet_GridView(Me.grvList3, M_DISP_ID, "L", 30, 10)
        ClsCMCommon.pfSet_GridView(Me.grvList4, M_DISP_ID, "L", 30, 10)

    End Sub

    ''' <summary>
    ''' ページ　ロード
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ＤＢ接続
        mclsDB.mstrConString = ConfigurationManager.ConnectionStrings("SPCDB").ToString
        If mclsDB.pfDB_Connect() = False Then
            'ＤＢ接続失敗
            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "データベースへの接続")
            Exit Sub
        End If

        If Not IsPostBack Then  '初回表示

            'プログラムＩＤ、画面名設定
            Master.ppProgramID = M_DISP_ID
            Master.ppTitle = fGet_DispNM(M_DISP_ID)

            'パンくずリスト設定
            Master.ppBcList_Text = ClsCMCommon.pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

            '全ボタン非活性
            Me.btnKSMST.Enabled = False
            Me.btnERLST.Enabled = False
            Me.btnAUTRN.Enabled = False
            Me.btnOVELT.Enabled = False

            'ボタン押下時のメッセージ設定
            Me.btnKSMST.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "決済センタＴＢＯＸマスタ取得")
            Me.btnERLST.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "集信エラー取得")
            Me.btnAUTRN.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "券売機自走取得")
            Me.btnOVELT.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "時間外表示ホール取得")

            'ﾄﾞﾛｯﾌﾟﾀﾞｳﾝ設定
            Me.ddlSElProc.ppDropDownList.Items.Add("")
            Me.ddlSElProc.ppDropDownList.Items.Add(New ListItem("決済センタＴＢＯＸマスタ取得", "1"))
            Me.ddlSElProc.ppDropDownList.Items.Add(New ListItem("集信エラー", "2"))
            Me.ddlSElProc.ppDropDownList.Items.Add(New ListItem("券売機　自走", "3"))
            Me.ddlSElProc.ppDropDownList.Items.Add(New ListItem("時間外消費", "4"))

            '一覧データ取得＆表示
            Call GetData_and_GridBind()

            '再実行判定の初期化
            lblSelProc.Text = ""
            lblJudgeRes.Text = ""
            lblErrDetail.Text = ""
        End If

    End Sub

    ' ''' <summary>
    ' ''' ページ　描画前
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Sub TJKLSTP001_PreLoad(sender As Object, e As EventArgs) Handles Me.PreLoad

    '    If ViewState("ddlSElProc") Is Nothing Then
    '    Else
    '        Me.ddlSElProc.ppSelectedValue = ViewState("ddlSElProc")
    '    End If

    'End Sub

    ''' <summary>
    ''' ページ　描画前
    ''' </summary>
    ''' <remarks>ユーザー権限制御</remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        'ユーザー権限制御
        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case Else
                Me.ddlSElProc.ppEnabled = False
                Me.btnProcSel.Enabled = False
                Me.btnRELOD.Enabled = False
                Me.btnKSMST.Enabled = False
                Me.btnERLST.Enabled = False
                Me.btnAUTRN.Enabled = False
                Me.btnOVELT.Enabled = False
        End Select

        If lblJudgeRes.Text = "×" Then
            Me.btnAUTRN.Enabled = False
            Me.btnERLST.Enabled = False
            Me.btnKSMST.Enabled = False
            Me.btnOVELT.Enabled = False
        End If

    End Sub

    ''' <summary>
    ''' ページ　アンロード
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub TJKLSTP001_Unload(sender As Object, e As EventArgs) Handles Me.Unload

        'DB接続のクローズ&破棄
        Call mclsDB.psDB_Close()

    End Sub

    '----------------------------------------------------------------------------------------------------------------
    '-　ボタン制御
    '----------------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' 選択ボタン　押下
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub btnProcSel_Click(sender As Object, e As EventArgs) Handles btnProcSel.Click

        Me.btnKSMST.Enabled = False
        Me.btnERLST.Enabled = False
        Me.btnAUTRN.Enabled = False
        Me.btnOVELT.Enabled = False

        Select Case Me.ddlSElProc.ppSelectedValue
            Case "1"
                Call sSetErrDetail("1")
                Me.btnKSMST.Enabled = True
            Case "2"
                Call sSetErrDetail("2")
                Me.btnERLST.Enabled = True
            Case "3"
                Call sSetErrDetail("3")
                Me.btnAUTRN.Enabled = True
            Case "4"
                Call sSetErrDetail("4")
                Me.btnOVELT.Enabled = True
            Case Else
                Me.lblSelProc.Text = ""
                Me.lblJudgeRes.Text = ""
                Me.lblErrDetail.Text = ""
        End Select

    End Sub

    ''' <summary>
    ''' リロード　押下
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub btnRELOD_Click(sender As Object, e As EventArgs) Handles btnRELOD.Click

        '一覧データ　再取得()
        Call GetData_and_GridBind()
        '実行判定　再取得
        Select Case Me.ddlSElProc.ppSelectedValue
            Case "1"
                Call sSetErrDetail("1")
            Case "2"
                Call sSetErrDetail("2")
            Case "3"
                Call sSetErrDetail("3")
            Case "4"
                Call sSetErrDetail("4")
            Case Else
                Me.lblSelProc.Text = ""
                Me.lblJudgeRes.Text = ""
                Me.lblErrDetail.Text = ""
        End Select

    End Sub

    ''' <summary>
    ''' 決済センタマスタ　押下
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub btnKSMST_Click(sender As Object, e As EventArgs) Handles btnKSMST.Click

        Dim strProcCD1 As String = "801"
        Dim strIraiSento As String = "TJK4"
        Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")
        Dim strProcTimes As String = "01"
        'Dim intRet As Integer

        Dim objSQLCmd As New SqlCommand
        Dim objWKDS As New DataSet
        Dim objTran As SqlTransaction

        Try
            objTran = mclsDB.mobjDB.BeginTransaction

            objSQLCmd = New SqlClient.SqlCommand("SMTTJK001_S2", mclsDB.mobjDB)
            'パラメータ設定
            objSQLCmd.Parameters.Add(pfSet_Param("prmD83_PROC_CD", SqlDbType.NVarChar, strProcCD1))
            objSQLCmd.Parameters.Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
            'コマンドタイプ設定(ストアド)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Transaction = objTran

            'SQL実行
            If mclsDB.pfGet_DataSet(objSQLCmd, objWKDS) = False Then
                'エラーメッセージ表示
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "決済センタマスタ取得")
                'ロールバック
                objSQLCmd.Transaction.Rollback()
                Exit Sub
            End If

            If objWKDS.Tables.Count > 0 Then
                If objWKDS.Tables(0).Rows.Count > 0 Then
                    strProcTimes = (Integer.Parse(objWKDS.Tables(0).Rows(0).Item(0).ToString.Substring(13, 2)) + 1).ToString.PadLeft(2, "0")
                End If
            End If

            objSQLCmd = New SqlClient.SqlCommand("SMTTJK001_I1", mclsDB.mobjDB)
            objSQLCmd.Transaction = objTran
            With objSQLCmd.Parameters
                .Add(pfSet_Param("prmSento", SqlDbType.NVarChar, strIraiSento))
                .Add(pfSet_Param("prmD83_PROC_CD1", SqlDbType.NVarChar, strProcCD1))
                .Add(pfSet_Param("prmD83_PROC_CD2", SqlDbType.NVarChar, "802"))
                .Add(pfSet_Param("prmToujituKaisuu1", SqlDbType.NVarChar, strProcTimes))
                .Add(pfSet_Param("prmToujituKaisuu2", SqlDbType.NVarChar, "01"))
                .Add(pfSet_Param("retD83_REQMNG_NO1", SqlDbType.NVarChar, 15, ParameterDirection.Output))
                .Add(pfSet_Param("retD83_REQMNG_NO2", SqlDbType.NVarChar, 15, ParameterDirection.Output))
                .Add(pfSet_Param("retD83_REQ_DT_NO1", SqlDbType.NVarChar, 15, ParameterDirection.Output))
                .Add(pfSet_Param("retD83_REQ_DT_NO2", SqlDbType.NVarChar, 15, ParameterDirection.Output))
                .Add(pfSet_Param("retD83_REQ_SEQ_NO1", SqlDbType.NVarChar, 15, ParameterDirection.Output))
                .Add(pfSet_Param("retD83_REQ_SEQ_NO2", SqlDbType.NVarChar, 15, ParameterDirection.Output))
            End With
            'コマンドタイプ設定(ストアド)
            objSQLCmd.CommandType = CommandType.StoredProcedure

            If mclsDB.pfDB_ProcStored(objSQLCmd) = False Then
                'エラーメッセージ表示
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "決済センタマスタ取得")
                'ロールバック
                objSQLCmd.Transaction.Rollback()
                Exit Sub
            End If

            'コミット
            objSQLCmd.Transaction.Commit()

            '一覧データ取得＆表示
            Call GetData_and_GridBind()

            '完了メッセージ
            psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "決済センタマスタ取得を実行しました。")

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "決済センタマスタ取得")

            'ログ出力
            psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
                            objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")

            'ロールバック
            If mclsDB.ppKeep_Tran = True Then
                mclsDB.psDB_Rollback()
            End If
        Finally
            Call mclsDB.psDisposeDataSet(objWKDS)

            If Not objSQLCmd Is Nothing Then
                objSQLCmd.Dispose()
            End If

        End Try

    End Sub

    ''' <summary>
    ''' 集信エラー　押下
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub btnERLST_Click(sender As Object, e As EventArgs) Handles btnERLST.Click

        Dim strProcCd1 As String = "631"
        Dim strProcCd2 As String = "632"
        Dim strIraiSento As String = "TJK2"
        Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")
        Dim strProcTimes As String = "01"
        Dim strReqmngNo1 As String = ""
        Dim strReqmngNo2 As String = ""
        Dim dtReqDt1 As String = ""
        Dim dtReqDt2 As String = ""
        Dim strReqSeq1 As String = ""
        Dim strReqSeq2 As String = ""

        Dim objSQLCmd As New SqlCommand
        Dim objWKDS As New DataSet
        Dim objTran As SqlTransaction

        Try
            objTran = mclsDB.mobjDB.BeginTransaction

            objSQLCmd = New SqlClient.SqlCommand("SMTTJK001_S2", mclsDB.mobjDB)
            'パラメータ設定
            objSQLCmd.Parameters.Add(pfSet_Param("prmD83_PROC_CD", SqlDbType.NVarChar, strProcCD1))
            objSQLCmd.Parameters.Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
            'コマンドタイプ設定(ストアド)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Transaction = objTran

            'SQL実行
            If mclsDB.pfGet_DataSet(objSQLCmd, objWKDS) = False Then
                'エラーメッセージ表示
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "集信エラー取得")
                'ロールバック
                objSQLCmd.Transaction.Rollback()
                Exit Sub
            End If

            If objWKDS.Tables.Count > 0 Then
                If objWKDS.Tables(0).Rows.Count > 0 Then
                    strProcTimes = (Integer.Parse(objWKDS.Tables(0).Rows(0).Item(0).ToString.Substring(13, 2)) + 1).ToString.PadLeft(2, "0")
                End If
            End If

            objSQLCmd = New SqlClient.SqlCommand("SMTTJK001_I1", mclsDB.mobjDB)
            objSQLCmd.Transaction = objTran
            With objSQLCmd.Parameters
                .Add(pfSet_Param("prmSento", SqlDbType.NVarChar, strIraiSento))
                .Add(pfSet_Param("prmD83_PROC_CD1", SqlDbType.NVarChar, strProcCd1))
                .Add(pfSet_Param("prmD83_PROC_CD2", SqlDbType.NVarChar, strProcCd2))
                .Add(pfSet_Param("prmToujituKaisuu1", SqlDbType.NVarChar, strProcTimes))
                .Add(pfSet_Param("prmToujituKaisuu2", SqlDbType.NVarChar, strProcTimes))
                .Add(pfSet_Param("retD83_REQMNG_NO1", SqlDbType.NVarChar, 15, ParameterDirection.Output))
                .Add(pfSet_Param("retD83_REQMNG_NO2", SqlDbType.NVarChar, 15, ParameterDirection.Output))
                .Add(pfSet_Param("retD83_REQ_DT_NO1", SqlDbType.NVarChar, 15, ParameterDirection.Output))
                .Add(pfSet_Param("retD83_REQ_DT_NO2", SqlDbType.NVarChar, 15, ParameterDirection.Output))
                .Add(pfSet_Param("retD83_REQ_SEQ_NO1", SqlDbType.NVarChar, 15, ParameterDirection.Output))
                .Add(pfSet_Param("retD83_REQ_SEQ_NO2", SqlDbType.NVarChar, 15, ParameterDirection.Output))
            End With
            'コマンドタイプ設定(ストアド)
            objSQLCmd.CommandType = CommandType.StoredProcedure

            If mclsDB.pfDB_ProcStored(objSQLCmd) = False Then
                'エラーメッセージ表示
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "集信エラー取得")
                'ロールバック
                objSQLCmd.Transaction.Rollback()
                Exit Sub
            End If
            '            objSQLCmd.Transaction.Commit()

            strReqmngNo1 = objSQLCmd.Parameters("retD83_REQMNG_NO1").Value.ToString
            strReqmngNo2 = objSQLCmd.Parameters("retD83_REQMNG_NO2").Value.ToString
            dtReqDt1 = objSQLCmd.Parameters("retD83_REQ_DT_NO1").Value.ToString
            dtReqDt2 = objSQLCmd.Parameters("retD83_REQ_DT_NO2").Value.ToString
            strReqSeq1 = objSQLCmd.Parameters("retD83_REQ_SEQ_NO1").Value.ToString
            strReqSeq2 = objSQLCmd.Parameters("retD83_REQ_SEQ_NO2").Value.ToString

            'プロシージャ設定
            objSQLCmd = New SqlCommand("SMTTJK001_I2", mclsDB.mobjDB)
            objSQLCmd.Transaction = objTran
            objSQLCmd.Parameters.Add(pfSet_Param("prmProcCd1", SqlDbType.NVarChar, strProcCd1))
            objSQLCmd.Parameters.Add(pfSet_Param("prmTvselfrunNo1", SqlDbType.NVarChar, strReqmngNo1))
            objSQLCmd.Parameters.Add(pfSet_Param("prmTvselfrunNo2", SqlDbType.NVarChar, strReqmngNo2))
            objSQLCmd.Parameters.Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            'ストアドプロシージャで実行
            objSQLCmd.CommandType = CommandType.StoredProcedure

            '実行
            If mclsDB.pfDB_ProcStored(objSQLCmd) = True Then
                If objSQLCmd.Parameters("returnCode").Value <> 0 Then
                    'エラーメッセージ表示
                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "集信エラー取得")
                    'ロールバック
                    objSQLCmd.Transaction.Rollback()
                    Exit Sub
                End If
            Else
                'エラーメッセージ表示
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "集信エラー取得")
                'ロールバック
                objSQLCmd.Transaction.Rollback()
                Exit Sub
            End If

            'コミット
            objSQLCmd.Transaction.Commit()

            '一覧データ取得＆表示
            Call GetData_and_GridBind()

            '完了メッセージ
            psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "集信エラー取得を実行しました。")

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "集信エラー取得")

            'ログ出力
            psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
                            objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
            If mclsDB.ppKeep_Tran = True Then
                mclsDB.psDB_Rollback()
            End If
            If Not objSQLCmd.Transaction Is Nothing Then
                objSQLCmd.Transaction.Rollback()
            End If
        Finally
            Call mclsDB.psDisposeDataSet(objWKDS)
            objSQLCmd.Dispose()
        End Try

    End Sub

    ''' <summary>
    ''' 券売機自走　押下
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub btnAUTRN_Click(sender As Object, e As EventArgs) Handles btnAUTRN.Click

        Dim strProcCd1 As String = "611"
        Dim strProcCd2 As String = "613"
        Dim strIraiSento As String = "TJK0"
        Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")
        Dim strProcTimes As String = "01"
        Dim strReqmngNo1 As String = ""
        Dim strReqmngNo2 As String = ""
        Dim dtReqDt1 As String = ""
        Dim dtReqDt2 As String = ""
        Dim strReqSeq1 As String = ""
        Dim strReqSeq2 As String = ""

        Dim objSQLCmd As New SqlCommand
        Dim objWKDS As New DataSet
        Dim objTran As SqlTransaction

        Try
            objTran = mclsDB.mobjDB.BeginTransaction

            objSQLCmd = New SqlClient.SqlCommand("SMTTJK001_S2", mclsDB.mobjDB)
            'パラメータ設定
            objSQLCmd.Parameters.Add(pfSet_Param("prmD83_PROC_CD", SqlDbType.NVarChar, strProcCd1))
            objSQLCmd.Parameters.Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
            'コマンドタイプ設定(ストアド)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Transaction = objTran

            'ユーザーマスタからデータ取得
            'SQL実行
            If mclsDB.pfGet_DataSet(objSQLCmd, objWKDS) = False Then
                'エラーメッセージ表示
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "券売機自走取得")
                'ロールバック
                objSQLCmd.Transaction.Rollback()
                Exit Sub
            End If

            If objWKDS.Tables.Count > 0 Then
                If objWKDS.Tables(0).Rows.Count > 0 Then
                    strProcTimes = (Integer.Parse(objWKDS.Tables(0).Rows(0).Item(0).ToString.Substring(13, 2)) + 1).ToString.PadLeft(2, "0")
                End If
            End If

            objSQLCmd = New SqlClient.SqlCommand("SMTTJK001_I1", mclsDB.mobjDB)
            objSQLCmd.Transaction = objTran
            With objSQLCmd.Parameters
                .Add(pfSet_Param("prmSento", SqlDbType.NVarChar, strIraiSento))
                .Add(pfSet_Param("prmD83_PROC_CD1", SqlDbType.NVarChar, strProcCd1))
                .Add(pfSet_Param("prmD83_PROC_CD2", SqlDbType.NVarChar, strProcCd2))
                .Add(pfSet_Param("prmToujituKaisuu1", SqlDbType.NVarChar, strProcTimes))
                .Add(pfSet_Param("prmToujituKaisuu2", SqlDbType.NVarChar, strProcTimes))
                .Add(pfSet_Param("retD83_REQMNG_NO1", SqlDbType.NVarChar, 15, ParameterDirection.Output))
                .Add(pfSet_Param("retD83_REQMNG_NO2", SqlDbType.NVarChar, 15, ParameterDirection.Output))
                .Add(pfSet_Param("retD83_REQ_DT_NO1", SqlDbType.NVarChar, 15, ParameterDirection.Output))
                .Add(pfSet_Param("retD83_REQ_DT_NO2", SqlDbType.NVarChar, 15, ParameterDirection.Output))
                .Add(pfSet_Param("retD83_REQ_SEQ_NO1", SqlDbType.NVarChar, 15, ParameterDirection.Output))
                .Add(pfSet_Param("retD83_REQ_SEQ_NO2", SqlDbType.NVarChar, 15, ParameterDirection.Output))
            End With
            'コマンドタイプ設定(ストアド)
            objSQLCmd.CommandType = CommandType.StoredProcedure

            If mclsDB.pfDB_ProcStored(objSQLCmd) = False Then
                'エラーメッセージ表示
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "券売機自走取得")
                'ロールバック
                objSQLCmd.Transaction.Rollback()
                Exit Sub
            End If
            '            objSQLCmd.Transaction.Commit()

            strReqmngNo1 = objSQLCmd.Parameters("retD83_REQMNG_NO1").Value.ToString
            strReqmngNo2 = objSQLCmd.Parameters("retD83_REQMNG_NO2").Value.ToString
            dtReqDt1 = objSQLCmd.Parameters("retD83_REQ_DT_NO1").Value.ToString
            dtReqDt2 = objSQLCmd.Parameters("retD83_REQ_DT_NO2").Value.ToString
            strReqSeq1 = objSQLCmd.Parameters("retD83_REQ_SEQ_NO1").Value.ToString
            strReqSeq2 = objSQLCmd.Parameters("retD83_REQ_SEQ_NO2").Value.ToString

            'プロシージャ設定
            objSQLCmd = New SqlCommand("SMTTJK001_I2", mclsDB.mobjDB)
            objSQLCmd.Transaction = objTran
            objSQLCmd.Parameters.Add(pfSet_Param("prmProcCd1", SqlDbType.NVarChar, strProcCd1))
            objSQLCmd.Parameters.Add(pfSet_Param("prmTvselfrunNo1", SqlDbType.NVarChar, strReqmngNo1))
            objSQLCmd.Parameters.Add(pfSet_Param("prmTvselfrunNo2", SqlDbType.NVarChar, strReqmngNo2))
            objSQLCmd.Parameters.Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            'ストアドプロシージャで実行
            objSQLCmd.CommandType = CommandType.StoredProcedure

            '実行
            If mclsDB.pfDB_ProcStored(objSQLCmd) = True Then
                If objSQLCmd.Parameters("returnCode").Value <> 0 Then
                    'エラーメッセージ表示
                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "券売機自走取得")
                    'ロールバック
                    objSQLCmd.Transaction.Rollback()
                    Exit Sub
                End If
            Else
                'エラーメッセージ表示
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "券売機自走取得")
                'ロールバック
                objSQLCmd.Transaction.Rollback()
                Exit Sub
            End If

            'コミット
            objSQLCmd.Transaction.Commit()

            '一覧データ取得＆表示
            Call GetData_and_GridBind()

            '完了メッセージ
            psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "券売機自走取得を実行しました。")


        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "券売機自走取得")

            'ログ出力
            psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
                            objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
            If mclsDB.ppKeep_Tran = True Then
                mclsDB.psDB_Rollback()
            End If
            If Not objSQLCmd.Transaction Is Nothing Then
                objSQLCmd.Transaction.Rollback()
            End If
        Finally
            Call mclsDB.psDisposeDataSet(objWKDS)
            objSQLCmd.Dispose()
        End Try

    End Sub

    ''' <summary>
    ''' 時間外消費　押下
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub btnOVELT_Click(sender As Object, e As EventArgs) Handles btnOVELT.Click

        Dim strProcCd1 As String = "621"
        Dim strProcCd2 As String = "622"
        Dim strIraiSento As String = "TJK1"
        Dim strToujituhiduke As String = DateTime.Now.ToString("yyyyMMdd")
        Dim strProcTimes As String = "01"
        Dim strReqmngNo1 As String = ""
        Dim strReqmngNo2 As String = ""
        Dim dtReqDt1 As String = ""
        Dim dtReqDt2 As String = ""
        Dim strReqSeq1 As String = ""
        Dim strReqSeq2 As String = ""

        Dim objSQLCmd As New SqlCommand
        Dim objWKDS As New DataSet
        Dim objTran As SqlTransaction

        Try
            objTran = mclsDB.mobjDB.BeginTransaction

            objSQLCmd = New SqlClient.SqlCommand("SMTTJK001_S2", mclsDB.mobjDB)
            'パラメータ設定
            objSQLCmd.Parameters.Add(pfSet_Param("prmD83_PROC_CD", SqlDbType.NVarChar, strProcCd1))
            objSQLCmd.Parameters.Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
            'コマンドタイプ設定(ストアド)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Transaction = objTran

            'ユーザーマスタからデータ取得
            'SQL実行
            If mclsDB.pfGet_DataSet(objSQLCmd, objWKDS) = False Then
                'エラーメッセージ表示
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "時間外表示ホール取得")
                'ロールバック
                objSQLCmd.Transaction.Rollback()
                Exit Sub
            End If

            If objWKDS.Tables.Count > 0 Then
                If objWKDS.Tables(0).Rows.Count > 0 Then
                    strProcTimes = (Integer.Parse(objWKDS.Tables(0).Rows(0).Item(0).ToString.Substring(13, 2)) + 1).ToString.PadLeft(2, "0")
                End If
            End If

            objSQLCmd = New SqlClient.SqlCommand("SMTTJK001_I1", mclsDB.mobjDB)
            objSQLCmd.Transaction = objTran
            With objSQLCmd.Parameters
                .Add(pfSet_Param("prmSento", SqlDbType.NVarChar, strIraiSento))
                .Add(pfSet_Param("prmD83_PROC_CD1", SqlDbType.NVarChar, strProcCd1))
                .Add(pfSet_Param("prmD83_PROC_CD2", SqlDbType.NVarChar, strProcCd2))
                .Add(pfSet_Param("prmToujituKaisuu1", SqlDbType.NVarChar, strProcTimes))
                .Add(pfSet_Param("prmToujituKaisuu2", SqlDbType.NVarChar, strProcTimes))
                .Add(pfSet_Param("retD83_REQMNG_NO1", SqlDbType.NVarChar, 15, ParameterDirection.Output))
                .Add(pfSet_Param("retD83_REQMNG_NO2", SqlDbType.NVarChar, 15, ParameterDirection.Output))
                .Add(pfSet_Param("retD83_REQ_DT_NO1", SqlDbType.NVarChar, 15, ParameterDirection.Output))
                .Add(pfSet_Param("retD83_REQ_DT_NO2", SqlDbType.NVarChar, 15, ParameterDirection.Output))
                .Add(pfSet_Param("retD83_REQ_SEQ_NO1", SqlDbType.NVarChar, 15, ParameterDirection.Output))
                .Add(pfSet_Param("retD83_REQ_SEQ_NO2", SqlDbType.NVarChar, 15, ParameterDirection.Output))
            End With
            'コマンドタイプ設定(ストアド)
            objSQLCmd.CommandType = CommandType.StoredProcedure

            If mclsDB.pfDB_ProcStored(objSQLCmd) = False Then
                'エラーメッセージ表示
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "時間外表示ホール取得")
                'ロールバック
                objSQLCmd.Transaction.Rollback()
                Exit Sub
            End If
            '            objSQLCmd.Transaction.Commit()

            strReqmngNo1 = objSQLCmd.Parameters("retD83_REQMNG_NO1").Value.ToString
            strReqmngNo2 = objSQLCmd.Parameters("retD83_REQMNG_NO2").Value.ToString
            dtReqDt1 = objSQLCmd.Parameters("retD83_REQ_DT_NO1").Value.ToString
            dtReqDt2 = objSQLCmd.Parameters("retD83_REQ_DT_NO2").Value.ToString
            strReqSeq1 = objSQLCmd.Parameters("retD83_REQ_SEQ_NO1").Value.ToString
            strReqSeq2 = objSQLCmd.Parameters("retD83_REQ_SEQ_NO2").Value.ToString

            'プロシージャ設定
            objSQLCmd = New SqlCommand("SMTTJK001_I2", mclsDB.mobjDB)
            objSQLCmd.Transaction = objTran
            objSQLCmd.Parameters.Add(pfSet_Param("prmProcCd1", SqlDbType.NVarChar, strProcCd1))
            objSQLCmd.Parameters.Add(pfSet_Param("prmTvselfrunNo1", SqlDbType.NVarChar, strReqmngNo1))
            objSQLCmd.Parameters.Add(pfSet_Param("prmTvselfrunNo2", SqlDbType.NVarChar, strReqmngNo2))
            objSQLCmd.Parameters.Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
            'ストアドプロシージャで実行
            objSQLCmd.CommandType = CommandType.StoredProcedure

            '実行
            If mclsDB.pfDB_ProcStored(objSQLCmd) = True Then
                If objSQLCmd.Parameters("returnCode").Value <> 0 Then
                    'エラーメッセージ表示
                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "時間外消費ホール取得")
                    'ロールバック
                    objSQLCmd.Transaction.Rollback()
                    Exit Sub
                End If
            Else
                'エラーメッセージ表示
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "時間外消費ホール取得")
                'ロールバック
                objSQLCmd.Transaction.Rollback()
                Exit Sub
            End If

            'コミット
            objSQLCmd.Transaction.Commit()

            '一覧データ取得＆表示
            Call GetData_and_GridBind()

            '完了メッセージ
            psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "時間外消費ホール取得を実行しました。")

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "時間外消費ホール取得")

            'ログ出力
            psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
                            objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
            If mclsDB.ppKeep_Tran = True Then
                mclsDB.psDB_Rollback()
            End If
            If Not objSQLCmd.Transaction Is Nothing Then
                objSQLCmd.Transaction.Rollback()
            End If
        Finally
            Call mclsDB.psDisposeDataSet(objWKDS)
            objSQLCmd.Dispose()
        End Try

    End Sub


    '============================================================================================================================
    '=　プロシージャ定義
    '============================================================================================================================

    ''' <summary>
    ''' データ取得～表示制限～バインド
    ''' </summary>
    ''' <param name="strWhere">SQL 条件文</param>
    Private Sub GetData_and_GridBind(Optional ByVal strWHERE As String = "")

        Dim objSQLCmd As New SqlClient.SqlCommand
        '        Dim objDBTran As SqlClient.SqlTransaction = mclsDB.mobjDB.BeginTransaction
        Dim intRetCD As Integer = 0
        Dim strMesType As String = "00001"
        Dim objWKDS As New DataSet

        Try
            objSQLCmd = New SqlClient.SqlCommand("TJKLSTP001_S1", mclsDB.mobjDB)
            'コマンドタイプ設定(ストアド)
            objSQLCmd.CommandType = CommandType.StoredProcedure

            'データ取得
            'SQL実行
            If mclsDB.pfGet_DataSet(objSQLCmd, objWKDS) = False Then
                'エラー
                'システムエラーメッセージ
                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                objStack = New StackFrame
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "明細情報取得失敗", "Catch")
                Exit Sub
            End If

            If objWKDS.Tables.Count > 0 Then
                Me.grvList1.DataSource = objWKDS.Tables(0)
                Me.grvList1.DataBind()
            End If
            If objWKDS.Tables.Count > 1 Then
                Me.grvList2.DataSource = objWKDS.Tables(1)
                Me.grvList2.DataBind()
            End If
            If objWKDS.Tables.Count > 2 Then
                Me.grvList3.DataSource = objWKDS.Tables(2)
                Me.grvList3.DataBind()
            End If
            If objWKDS.Tables.Count > 3 Then
                Me.grvList4.DataSource = objWKDS.Tables(3)
                Me.grvList4.DataBind()
            End If

        Catch ex As Exception

            'システムエラーメッセージ
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)

            'ログ出力
            psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
                            objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DataSetのDispose
            Call mclsDB.psDisposeDataSet(objWKDS)
        End Try

    End Sub

    ''' <summary>
    ''' 画面タイトル取得
    ''' </summary>
    ''' <param name="ipstrDispID"></param>
    ''' <remarks></remarks>
    Private Function fGet_DispNM(ByVal ipstrDispID As String)

        Dim objSQLCmd As New SqlClient.SqlCommand
        '        Dim objDBTran As SqlClient.SqlTransaction = mclsDB.mobjDB.BeginTransaction
        Dim intRetCD As Integer = 0
        Dim strMesType As String = "00001"
        Dim objWKDS As New DataSet

        fGet_DispNM = ""

        Try
            objSQLCmd = New SqlClient.SqlCommand("ZCMPSEL001", mclsDB.mobjDB)
            'コマンドタイプ設定(ストアド)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            'パラメータ設定
            objSQLCmd.Parameters.Add(pfSet_Param("dispcd", SqlDbType.VarChar, ipstrDispID))

            'ユーザーマスタからデータ取得
            'SQL実行
            If mclsDB.pfGet_DataSet(objSQLCmd, objWKDS) = False Then
                'エラー
                'システムエラーメッセージ
                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                objStack = New StackFrame
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "明細情報取得失敗", "Catch")
                Exit Function
            End If

            If objWKDS.Tables.Count > 0 Then
                If objWKDS.Tables(0).Rows.Count > 0 Then
                    fGet_DispNM = objWKDS.Tables(0).Rows(0).Item(0).ToString
                End If
            End If
        Catch ex As Exception
            'ログ出力
            psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
                            objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
            fGet_DispNM = ""
        Finally
            Call mclsDB.psDisposeDataSet(objWKDS)
        End Try

    End Function

    Private Sub sSetErrDetail(ByVal ipstrProcCD As String)

        Dim objSQLCmd As New SqlClient.SqlCommand
        '        Dim objDBTran As SqlClient.SqlTransaction = mclsDB.mobjDB.BeginTransaction
        Dim intRetCD As Integer = 0
        Dim strMesType As String = "00001"
        Dim objWKDS As DataSet = New DataSet

        Try
            '実行指示の初期化
            Me.lblJudgeRes.Text = ""
            Me.lblErrDetail.Text = ""
            Me.lblSelProc.Text = ""

            objSQLCmd = New SqlClient.SqlCommand("TJKLSTP001_S2", mclsDB.mobjDB)
            'コマンドタイプ設定(ストアド)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(pfSet_Param("prmProcCD", SqlDbType.VarChar, ipstrProcCD))

            'データ取得
            'SQL実行
            If mclsDB.pfGet_DataSet(objSQLCmd, objWKDS) = False Then
                'エラー
                'ストアドがなくてもエラーを出さない(保険)
                'システムエラーメッセージ
                '                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                '                objStack = New StackFrame
                'ログ出力
                '                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "処理情報　取得失敗", "Catch")
                'ストアドがなくてもエラーを出さない(保険)

                Me.lblJudgeRes.Text = ""
                Me.lblErrDetail.Text = ""
                Me.lblSelProc.Text = ""

                Exit Sub
            End If

            'システム的なエラー検知
            If objWKDS.Tables.Count > 3 Then
                If objWKDS.Tables(3).Rows.Count > 0 Then
                    lblJudgeRes.Text = "×"
                    lblJudgeRes.ForeColor = Drawing.Color.Red
                    Select Case objWKDS.Tables(3).Rows(0).Item("DA3_PROC_CD").ToString
                        Case "FTP"
                            If objWKDS.Tables(3).Rows(0).Item("DA3_INSERT_USR") = "SMTSSF001" Then
                                lblJudgeRes.Text = "×"
                                lblJudgeRes.ForeColor = Drawing.Color.Red
                                lblErrDetail.Text = "ＦＴＰ送信サービスでＦＴＰエラーが発生中"
                            End If
                            If objWKDS.Tables(3).Rows(0).Item("DA3_INSERT_USR") = "SMTSKF001" Then
                                lblJudgeRes.Text = "×"
                                lblJudgeRes.ForeColor = Drawing.Color.Red
                                lblErrDetail.Text = "ＦＴＰ受信サービスでＦＴＰエラーが発生中"
                            End If
                        Case "DB"
                            If objWKDS.Tables(3).Rows(0).Item("DA3_INSERT_USR") = "SMTSSF001" Then
                                lblJudgeRes.Text = "×"
                                lblJudgeRes.ForeColor = Drawing.Color.Red
                                lblErrDetail.Text = "ＦＴＰ送信サービスでエラーが発生中 "
                            End If
                            If objWKDS.Tables(3).Rows(0).Item("DA3_INSERT_USR") = "SMTSKF001" Then
                                lblJudgeRes.Text = "×"
                                lblJudgeRes.ForeColor = Drawing.Color.Red
                                lblErrDetail.Text = "ＦＴＰ受信サービスでエラーが発生中"
                            End If
                    End Select
                End If
            End If

            '停滞している情報を検知
            If lblJudgeRes.Text = "" Then
                If objWKDS.Tables.Count > 2 Then
                    If objWKDS.Tables(2).Rows.Count > 0 Then
                        If objWKDS.Tables(2).Rows(0).Item("MAXDT") Is DBNull.Value Then
                            '                            lblJudgeRes.Text = "○"
                            '                            lblJudgeRes.ForeColor = Drawing.Color.Blue
                            '                            lblErrDetail.Text = "実行可能"
                        Else
                            If objWKDS.Tables(2).Rows(0).Item("MAXDT") <= DateAdd(DateInterval.Minute, -30, DateTime.Now) Then
                                lblJudgeRes.Text = "×"
                                lblJudgeRes.ForeColor = Drawing.Color.Yellow
                                lblErrDetail.Text = "処理中、または処理に不具合が発生している可能性がある"
                            ElseIf objWKDS.Tables(2).Rows(0).Item("MAXDT") <= DateAdd(DateInterval.Minute, -120, DateTime.Now) Then
                                lblJudgeRes.Text = "×"
                                lblJudgeRes.ForeColor = Drawing.Color.Red
                                lblErrDetail.Text = "処理に不具合が発生している可能性がある"
                            Else
                                lblJudgeRes.Text = "×"
                                lblJudgeRes.ForeColor = Drawing.Color.Red
                                lblErrDetail.Text = "処理中のため実行不可"
                            End If
                        End If
                    End If
                End If
            End If

            '実行中の情報を検知
            If lblJudgeRes.Text = "" Then
                If objWKDS.Tables.Count > 1 Then
                    If objWKDS.Tables(1).Rows.Count > 0 Then
                        lblJudgeRes.Text = "×"
                        lblJudgeRes.ForeColor = Drawing.Color.Red
                        lblErrDetail.Text = "処理中のため実行不可"
                    Else
                        '                        lblJudgeRes.Text = "○"
                        '                        lblJudgeRes.ForeColor = Drawing.Color.Blue
                        '                        lblErrDetail.Text = "実行可能"
                    End If
                End If
            End If

            '当日実行済みの情報を検知
            If lblJudgeRes.Text = "" Then
                If objWKDS.Tables.Count > 0 Then
                    If objWKDS.Tables(0).Rows.Count > 0 Then
                        lblJudgeRes.Text = "○"
                        lblJudgeRes.ForeColor = Drawing.Color.Blue
                        lblErrDetail.Text = "前回処理は正常に終了している　実行可能"
                    Else
                        lblJudgeRes.Text = "○"
                        lblJudgeRes.ForeColor = Drawing.Color.Blue
                        lblErrDetail.Text = "本日は実行されていない　実行可能"
                    End If
                End If
            End If

            If lblJudgeRes.Text = "" Then
                lblJudgeRes.Text = "○"
                lblJudgeRes.ForeColor = Drawing.Color.Blue
                lblErrDetail.Text = "実行可能"
            End If

            If lblJudgeRes.Text <> "" Then
                Select Case ipstrProcCD
                    Case "1"
                        Me.lblSelProc.Text = "決済センタＴＢＯＸマスタ取得"
                    Case "2"
                        Me.lblSelProc.Text = "集信エラー"
                    Case "3"
                        Me.lblSelProc.Text = "券売機　自走"
                    Case "4"
                        Me.lblSelProc.Text = "時間外消費"
                End Select
            End If

        Catch ex As Exception

            'システムエラーメッセージ
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)

            'ログ出力
            psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
                            objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DataSetのDispose
            Call mclsDB.psDisposeDataSet(objWKDS)
        End Try

    End Sub

End Class

        'Dim objSQLCmd As New SqlClient.SqlCommand
        'Dim objDBTran As SqlClient.SqlTransaction = mclsDB.mobjDB.BeginTransaction
        'Dim intRetCD As Integer = 0
        'Dim strMesType As String = "00001"

        'objSQLCmd.Connection = mclsDB.mobjDB
        'objSQLCmd.CommandText = "COMUPDM92_I1"
        'objSQLCmd.Parameters.Add("prmM92_CNST_CLS", SqlDbType.NVarChar)
        'objSQLCmd.Parameters.Add("prmM92_CNSTCLS_NM", SqlDbType.NVarChar)
        'objSQLCmd.Parameters.Add("prmM92_TBOXCLS_CD", SqlDbType.NVarChar)
        'objSQLCmd.Parameters.Add("prmM92_SEQNO", SqlDbType.NVarChar)
        'objSQLCmd.Parameters.Add("prmM92_CNSTCKS_CD", SqlDbType.NVarChar)
        'objSQLCmd.Parameters.Add("prmM92_EMGNCY_FLG", SqlDbType.NVarChar)
        'objSQLCmd.Parameters.Add("prmM92_INSERT_USR", SqlDbType.NVarChar)

        'Dim strWKBuff As String = ""
        'strWKBuff = ""
        'If Me.txtCnst_Cls.ppText Is DBNull.Value Then
        '    strWKBuff = ""
        'ElseIf Me.txtCnst_Cls.ppText.Trim = "" Then
        '    strWKBuff = ""
        'Else
        '    strWKBuff = Me.txtCnst_Cls.ppText.Trim
        'End If
        'objSQLCmd.Parameters("prmM92_CNST_CLS").Value = strWKBuff

        'strWKBuff = ""
        'If Me.txtCnstCls_NM.ppText Is DBNull.Value Then
        '    strWKBuff = ""
        'ElseIf Me.txtCnstCls_NM.ppText.Trim = "" Then
        '    strWKBuff = ""
        'Else
        '    strWKBuff = Me.txtCnstCls_NM.ppText.Trim
        'End If
        'objSQLCmd.Parameters("prmM92_CNSTCLS_NM").Value = strWKBuff

        'strWKBuff = ""
        'If ViewState("ddlTBOXCLS_CD") Is Nothing Then
        '    strWKBuff = ""
        'Else
        '    strWKBuff = ViewState("ddlTBOXCLS_CD")
        'End If
        'objSQLCmd.Parameters("prmM92_TBOXCLS_CD").Value = strWKBuff

        'strWKBuff = ""
        'If Me.txtSEQNO.ppText Is DBNull.Value Then
        '    strWKBuff = ""
        'ElseIf Me.txtSEQNO.ppText.Trim = "" Then
        '    strWKBuff = ""
        'Else
        '    strWKBuff = Me.txtSEQNO.ppText.Trim
        'End If
        'objSQLCmd.Parameters("prmM92_SEQNO").Value = strWKBuff

        'strWKBuff = ""
        'If ViewState("ddlWork_Cls") Is Nothing Then
        '    strWKBuff = ""
        'Else
        '    strWKBuff = ViewState("ddlWork_Cls")
        'End If
        'objSQLCmd.Parameters("prmM92_CNSTCKS_CD").Value = strWKBuff

        'strWKBuff = ""
        'If Me.ChkEMGNCY_FLG.Checked = False Then
        '    strWKBuff = "0"
        'Else
        '    strWKBuff = "1"
        'End If
        'objSQLCmd.Parameters("prmM92_EMGNCY_FLG").Value = strWKBuff

        'objSQLCmd.Parameters("prmM92_INSERT_USR").Value = User.Identity.Name

        'Try
        '    'トランザクション開始
        '    'If mclsDB.pfDB_BeginTrans = False Then
        '    '    '失敗メッセージ表示
        '    '    psMesBox(Me, strMesType, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
        '    '    mstrDispMode = "UPD"
        '    '    Exit Sub
        '    'End If
        '        If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
        '            'エラー
        '            clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
        '            objStack = New StackFrame
        '            'ログ出力
        '            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "明細情報取得失敗", "Catch")
        '            Exit Sub
        '        End If

        '        Me.grvList.DataSource = objwkDS.Tables(0)
        '        Me.grvList.DataBind()

