'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　特別保守費用照会 印刷画面
'*　ＰＧＭＩＤ：　CMPOUTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.11.28　：　ＮＫＣ
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'CMPOUTP001-001     2016/03/23      栗原　　　特別保守料金一覧csvファイルの項目に「保守担当者名」、「作業者」を追加　CMPOUTP001_S1
'                   2016/03/28      栗原      「保守担当者名」→「会社名」に変更

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
#End Region

Public Class CMPOUTP001
#Region "継承定義"
    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
    Inherits System.Web.UI.Page
#End Region

#Region "定数定義"
    '============================================================================================================================
    '=　定数定義
    '============================================================================================================================
    'プログラムID
    Const M_MY_DISP_ID = P_FUN_CMP & P_SCR_OUT & P_PAGE & "001"
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
    Dim clsCMDBC As New ClsCMDBCom


#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' ページロード処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then  '初回表示のみ

            'プログラムID、画面名設定
            Master.ppProgramID = M_MY_DISP_ID
            Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)

            'パンくずリスト設定
            Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

            '画面クリア
            msClearScreen()

        End If

    End Sub

    ''' <summary>
    ''' 印刷条件クリアボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click

        '開始ログ出力
        psLogStart(Me)

        '画面クリア
        msClearScreen()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 印刷ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click

        Dim objDs As DataSet        'データセット
        Dim objDt(1) As DataTable   'データテーブル
        Dim rpt(1) As Object        'ActiveReports用クラス
        Dim strRptName(1) As String 'レポート名   

        '開始ログ出力
        psLogStart(Me)

        '入力内容検証
        If (Page.IsValid) Then

            If Me.dtbSupport.ppText = String.Empty AndAlso Me.dttStDtFrom.ppText = String.Empty _
               AndAlso Me.dtbSubmitDt.ppFromText = String.Empty Then
                psMesBox(Me, "20005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                Exit Sub
            End If

            If Me.dttStDtFrom.ppText <> String.Empty AndAlso Me.dttStDtFrom.ppDate > Me.dttStDtTo.ppDate Then
                dttStDtFrom.psSet_ErrorNo("2001", "対応開始日時")
                Exit Sub
            End If

            '(1)特別保守費用一覧印刷処理
            'データ取得
            objDs = mfGetData(M_MY_DISP_ID + "_S1")
            If Not objDs Is Nothing Then
                rpt(0) = New CMPREP003
                objDt(0) = objDs.Tables(0)
                strRptName(0) = "特別保守費用一覧"
                If objDt(0).Rows.Count = 0 Then
                    psMesBox(Me, "00007", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)
                    Exit Sub
                End If
            End If

            '(2)特別保守作業集計印刷処理
            'データ取得
            objDs = mfGetData(M_MY_DISP_ID + "_S2")
            If Not objDs Is Nothing Then
                rpt(1) = New CMPREP004
                objDt(1) = objDs.Tables(0)
                strRptName(1) = "特別保守作業集計"
            End If

            '帳票出力
            psPrintPDF(Me, rpt, objDt, strRptName)
        End If

    End Sub


    Protected Sub btnCsv_Click(sender As Object, e As EventArgs) Handles btnCsv.Click

        Dim objDs As DataSet        'データセット
        Dim objDt As New DataTable   'データテーブル
        Dim objDr As DataRow
        Dim intIndex As Integer

        '開始ログ出力
        psLogStart(Me)

        Try

            '入力内容検証
            If (Page.IsValid) Then

                If Me.dtbSupport.ppText = String.Empty AndAlso Me.dttStDtFrom.ppText = String.Empty _
                   AndAlso Me.dtbSubmitDt.ppFromText = String.Empty Then
                    psMesBox(Me, "20005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    Exit Sub
                End If

                If Me.dttStDtFrom.ppText <> String.Empty AndAlso Me.dttStDtFrom.ppDate > Me.dttStDtTo.ppDate Then
                    dttStDtFrom.psSet_ErrorNo("2001", "対応開始日時")
                    Exit Sub
                End If

                '(1)特別保守費用一覧印刷処理
                'データ取得
                objDs = mfGetData(M_MY_DISP_ID + "_S1")
                If Not objDs Is Nothing Then
                    If objDs.Tables(0).Rows.Count = 0 Then
                        psMesBox(Me, "00007", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)
                        Exit Sub
                    End If

                    objDt.Columns.Add("対応年月")
                    objDt.Columns.Add("エリア")
                    objDt.Columns.Add("依頼日付")
                    objDt.Columns.Add("依頼時刻")
                    objDt.Columns.Add("対応日付")
                    objDt.Columns.Add("対応出発時刻")
                    objDt.Columns.Add("対応開始時刻")
                    objDt.Columns.Add("対応終了時刻")
                    objDt.Columns.Add("担当SC")
                    objDt.Columns.Add("保守管理No")
                    objDt.Columns.Add("TBOXID")
                    objDt.Columns.Add("ホール名")
                    objDt.Columns.Add("TBOXタイプ")
                    objDt.Columns.Add("申告内容/処置内容")
                    objDt.Columns.Add("作業")
                    objDt.Columns.Add("往復")
                    objDt.Columns.Add("作業人数")
                    objDt.Columns.Add("請求額")
                    objDt.Columns.Add("提出日")
                    'CMPOUTP001-001
                    objDt.Columns.Add("会社名")
                    objDt.Columns.Add("作業者")
                    'CMPOUTP001-001 END

                    For intIndex = 0 To objDs.Tables(0).Rows.Count - 1
                        With objDs.Tables(0).Rows(intIndex)
                            objDr = objDt.NewRow
                            If .Item("対応年月").ToString = "1900/01/01 0:00:00" Then
                                objDr("対応年月") = ""
                            Else
                                objDr("対応年月") = Date.Parse(.Item("対応年月").ToString).ToString("yyyy/MM")
                            End If
                            objDr("エリア") = .Item("エリア").ToString
                            If .Item("依頼日時").ToString <> "" Then
                                objDr("依頼日付") = Date.Parse(.Item("依頼日時").ToString).ToString("yyyy/MM/dd")
                                objDr("依頼時刻") = Date.Parse(.Item("依頼日時").ToString).ToString("HH:mm")
                            End If
                            If .Item("対応日付").ToString <> "" Then
                                objDr("対応日付") = Date.Parse(.Item("対応日付").ToString).ToString("yyyy/MM/dd")
                            End If
                            If .Item("出発時刻").ToString <> "" Then
                                objDr("対応出発時刻") = Date.Parse(.Item("出発時刻").ToString).ToString("HH:mm")
                            End If
                            If .Item("開始時刻").ToString <> "" Then
                                objDr("対応開始時刻") = Date.Parse(.Item("開始時刻").ToString).ToString("HH:mm")
                            End If
                            If .Item("終了時刻").ToString <> "" Then
                                objDr("対応終了時刻") = Date.Parse(.Item("終了時刻").ToString).ToString("HH:mm")
                            End If
                            objDr("担当SC") = .Item("担当SC").ToString
                            objDr("保守管理No") = .Item("保守管理No").ToString
                            objDr("TBOXID") = .Item("TBOXID").ToString
                            objDr("ホール名") = .Item("ホール名").ToString
                            objDr("TBOXタイプ") = .Item("TBOXタイプ").ToString
                            objDr("申告内容/処置内容") = .Item("故障状況対応内容").ToString
                            objDr("作業") = .Item("作業").ToString
                            objDr("往復") = .Item("往復").ToString
                            objDr("作業人数") = .Item("作業人数").ToString
                            If .Item("請求額").ToString <> "" Then
                                objDr("請求額") = Decimal.Parse(.Item("請求額").ToString).ToString("#0")
                            End If
                            If .Item("提出日").ToString <> "" Then
                                objDr("提出日") = Date.Parse(.Item("提出日").ToString).ToString("yyyy/MM/dd")
                            End If
                            'CMPOUTP001-001
                            objDr("会社名") = .Item("会社名").ToString
                            objDr("作業者") = .Item("作業者").ToString
                            'CMPOUTP001-001 END
                            objDt.Rows.Add(objDr)
                        End With
                    Next

                    'CSVファイルダウンロード
                    If pfDLCsvFile("特別保守費用一覧" & Me.dtbSupport.ppText.Replace("/", "") & "_" & DateTime.Now.ToString("yyyyMMddHHmmss") & ".csv", objDt, True, Me) <> 0 Then
                        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
                    End If
                Else
                End If

            End If

        Catch ex As Threading.ThreadAbortException

        Catch ex As Exception
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

    End Sub


    Protected Sub btnTCsv_Click(sender As Object, e As EventArgs) Handles btnTCsv.Click

        Dim objDs As DataSet        'データセット
        Dim objDt As New DataTable   'データテーブル
        Dim objDr As DataRow
        Dim intIndex As Integer

        '開始ログ出力
        psLogStart(Me)

        Try

            '入力内容検証
            If (Page.IsValid) Then

                If Me.dtbSupport.ppText = String.Empty AndAlso Me.dttStDtFrom.ppText = String.Empty _
                   AndAlso Me.dtbSubmitDt.ppFromText = String.Empty Then
                    psMesBox(Me, "20005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    Exit Sub
                End If

                If Me.dttStDtFrom.ppText <> String.Empty AndAlso Me.dttStDtFrom.ppDate > Me.dttStDtTo.ppDate Then
                    dttStDtFrom.psSet_ErrorNo("2001", "対応開始日時")
                    Exit Sub
                End If

                '(2)特別保守作業集計印刷処理
                objDs = Nothing
                objDs = New DataSet
                'データ取得
                objDs = mfGetData(M_MY_DISP_ID + "_S2")
                If Not objDs Is Nothing Then
                    If objDs.Tables(0).Rows.Count = 0 Then
                        psMesBox(Me, "00007", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)
                        Exit Sub
                    End If

                    objDt = Nothing
                    objDt = New DataTable
                    objDt.Columns.Add("対応年月")
                    objDt.Columns.Add("磁気 無線_件数")
                    objDt.Columns.Add("磁気 無線_金額")
                    objDt.Columns.Add("磁気 有線_件数")
                    objDt.Columns.Add("磁気 有線_金額")
                    objDt.Columns.Add("IC_件数")
                    objDt.Columns.Add("IC_金額")
                    objDt.Columns.Add("LUTERNA_件数")
                    objDt.Columns.Add("LUTERNA_金額")
                    objDt.Columns.Add("合計_件数")
                    objDt.Columns.Add("合計_金額")

                    For intIndex = 0 To objDs.Tables(0).Rows.Count - 1
                        With objDs.Tables(0).Rows(intIndex)
                            objDr = objDt.NewRow
                            If .Item("対応年月").ToString = "1900/01/01 0:00:00" Then
                                objDr("対応年月") = ""
                            Else
                                objDr("対応年月") = Date.Parse(.Item("対応年月").ToString).ToString("yyyy/MM")
                            End If
                            objDr("磁気 無線_件数") = .Item("磁気無線系件数").ToString
                            objDr("磁気 無線_金額") = .Item("磁気無線系金額").ToString
                            objDr("磁気 有線_件数") = .Item("磁気有線系件数").ToString
                            objDr("磁気 有線_金額") = .Item("磁気有線系金額").ToString
                            objDr("IC_件数") = .Item("ＩＣ件数").ToString
                            objDr("IC_金額") = .Item("ＩＣ金額").ToString
                            objDr("LUTERNA_件数") = .Item("ＬＵＴＥＲＮＡ件数").ToString
                            objDr("LUTERNA_金額") = .Item("ＬＵＴＥＲＮＡ金額").ToString
                            objDr("合計_件数") = .Item("合計件数").ToString
                            objDr("合計_金額") = .Item("合計金額").ToString
                            objDt.Rows.Add(objDr)
                        End With
                    Next

                    'CSVファイルダウンロード
                    If pfDLCsvFile("特別保守作業集計" & Me.dtbSupport.ppText.Replace("/", "") & "_" & DateTime.Now.ToString("yyyyMMddHHmmss") & ".csv", objDt, True, Me) <> 0 Then
                        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
                    End If
                End If

            End If

        Catch ex As Threading.ThreadAbortException

        Catch ex As Exception
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()

        Me.dtbSupport.ppText = String.Empty '対応年月
        Me.dttStDtFrom.ppText = String.Empty
        Me.dttStDtTo.ppText = String.Empty
        Me.dttStDtFrom.ppHourText = String.Empty
        Me.dttStDtFrom.ppMinText = String.Empty
        Me.dttStDtTo.ppHourText = String.Empty
        Me.dttStDtTo.ppMinText = String.Empty
        Me.dtbSubmitDt.ppFromText = String.Empty
        Me.dtbSubmitDt.ppToText = String.Empty
        Me.dtbSupport.ppDateBox.Focus()

    End Sub

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <param name="strProc"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetData(ByVal strProc As String) As DataSet

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGetData = Nothing

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(strProc, objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '対応年月(yyyy/MM)
                    If Me.dtbSupport.ppText = "" Then
                        .Add(pfSet_Param("supportdt", SqlDbType.NVarChar, ""))
                    Else
                        .Add(pfSet_Param("supportdt", SqlDbType.NVarChar, Me.dtbSupport.ppText.ToString))
                    End If
                    If Me.dttStDtFrom.ppDateBox.Text <> "" Then
                        If Me.dttStDtFrom.ppHourText = "" Then
                            Me.dttStDtFrom.ppHourText = "00"
                            Me.dttStDtFrom.ppMinText = "00"
                        End If
                    End If
                    If Me.dttStDtFrom.ppText = "" Then
                        .Add(pfSet_Param("stdtfrom", SqlDbType.NVarChar, ""))
                    Else
                        .Add(pfSet_Param("stdtfrom", SqlDbType.NVarChar, Me.dttStDtFrom.ppDate.ToString))
                    End If

                    If Me.dttStDtTo.ppDateBox.Text <> "" Then
                        If Me.dttStDtTo.ppHourText = "" Then
                            Me.dttStDtTo.ppHourText = "23"
                            Me.dttStDtTo.ppMinText = "59"
                        End If
                    End If
                    If Me.dttStDtTo.ppText = "" Then
                        .Add(pfSet_Param("stdtto", SqlDbType.NVarChar, ""))
                    Else
                        .Add(pfSet_Param("stdtto", SqlDbType.NVarChar, Me.dttStDtTo.ppDate.ToString))
                    End If
                    If Me.dtbSubmitDt.ppFromText = "" Then
                        .Add(pfSet_Param("submitfrom", SqlDbType.NVarChar, ""))
                    Else
                        .Add(pfSet_Param("submitfrom", SqlDbType.NVarChar, Me.dtbSubmitDt.ppFromDate.ToString))
                    End If
                    If Me.dtbSubmitDt.ppToText = "" Then
                        .Add(pfSet_Param("submitto", SqlDbType.NVarChar, ""))
                    Else
                        .Add(pfSet_Param("submitto", SqlDbType.NVarChar, Me.dtbSubmitDt.ppToDate.ToString))
                    End If
                End With

                'データ取得
                mfGetData = clsDataConnect.pfGet_DataSet(objCmd)

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "特別保守費用照会　印刷画面")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Function

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
