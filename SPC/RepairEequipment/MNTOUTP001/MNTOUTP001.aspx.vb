'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜修理・整備業務＞
'*　処理名　　：　整備検収書
'*　ＰＧＭＩＤ：　MNTOUTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014/01/29　：　後藤
'*  更　新　　：　2017/07/27　：　伯野  通常整備料金の集計キーを管理番号に変更
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'MNTOUTP001-000     2014.07.01      間瀬　　　年月度FromToを一つに変更、初期の締め処理時のバグ修正、画面の挙動の修正
'MNTOUTP001-001     2015/08/06      栗原　　　帳票フッター用項目を追加　MNTOUTP001_S4　MNTOUTP001_S5
'MNTOUTP001-002     2016/01/25      栗原　　　帳票印刷時の会社情報をマスタ取得に変更
'MNTOUTP001-003     2017/07/27      伯野　　　通常整備料金の集計キーを管理番号に修正
'　　　　　　　　　　　　　　　　　　　　　　 　　同月内に注文番号と管理番号が１：ｎになる場合の対応
'MNTOUTP001-004     2017/08/04      伯野　　　整備検収書の集計処理を追加

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax
Imports System.Globalization
Imports System
Imports System.IO
Imports System.String
Imports SPC.ClsCMExclusive

#End Region

Public Class MNTOUTP001
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
    ''' <summary>画面ID</summary>
    Private Const M_DISP_ID = P_FUN_MNT & P_SCR_OUT & P_PAGE & "001"

    '-----------------------------
    '2014/04/11 Hamamoto　ここから
    '-----------------------------
    ''' <summary>
    ''' 整備完了品一覧表帳票最大行カウント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAX_ROW_CNT_FINISH As Integer = 20

    ''' <summary>
    ''' TBOX通常整備料金について帳票最大行カウント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAX_ROW_CNT_TBOX As Integer = 20
    '-----------------------------
    '2014/04/11 Hamamoto　ここまで
    '-----------------------------
    '-----------------------------
    '2014/04/14 Hamamoto　ここから
    '-----------------------------
    ''' <summary>
    ''' 情報機器整備の報告書兼検収書帳票最大行カウント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAX_ROW_CNT_HOKOKUKENSYU As Integer = 15
    '-----------------------------
    '2014/04/14 Hamamoto　ここまで
    '-----------------------------

    '-----------------------------
    '2014/04/15 太和田　ここから
    '-----------------------------
    'ボタン名
    Const sCnsShukei As String = "当月集計"
    '-----------------------------
    '2014/04/15 太和田　ここまで
    '-----------------------------

#End Region

#Region "構造体・列挙体定義"
    '============================================================================================================================
    '=　構造体・列挙体定義
    '============================================================================================================================
    '-----------------------------
    '2014/04/11 Hamamoto　ここから
    '-----------------------------
    ''' <summary>
    ''' 整備完了品一覧CSV列名
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum eColKanryoCsv
        帳票出力日 = 0
        送付先名
        納入日
        送付元名
        作業内容
        バージョン
        注文番号
        品名
        区分
        シリアルNo
        備考
        数量
        合計数量
        区切り
    End Enum

    ''' <summary>
    ''' 「TBOX通常整備料金について」のCSV列名
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum eColTujoCsv
        帳票出力日 = 0
        宛先会社名
        宛先部署名
        発行元会社名
        発行元住所
        検収年月
        システム種別
        作業種別
        台数
        注文番号
        部品１
        項目１
        単価１
        数量１
        金額１
        合計金額
        添付資料枚数
        空行フラグ
    End Enum

    '-----------------------------
    '2014/04/11 Hamamoto　ここまで
    '-----------------------------

    '-----------------------------
    '2014/04/14 Hamamoto　ここから
    '-----------------------------
    ''' <summary>
    ''' 「情報機器整備の報告書兼検収書」のCSV列名
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum eColHokokuKensyuCsv
        帳票出力日 = 0
        宛先会社名
        宛先部署名
        発行元会社名
        発行元住所
        検収年月
        項目
        注文番号
        金額
        備考
        小計
        出精値引き
        課税対象料金金額
        消費税相当額
        合計金額
        確認後宛先会社名
        確認後宛先部署名
        確認後宛先担当者
        確認後宛先TEL
        確認後宛先FAX
        検収者会社名
        検収者部署名
        検収者担当者
        値引率
        空行フラグ
    End Enum
    '-----------------------------
    '2014/04/14 Hamamoto　ここから
    '-----------------------------

#End Region

#Region "変数定義"
    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
    Dim objStack As StackFrame
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
    ''' Page_Init.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '表設定
        pfSet_GridView(grvList, M_DISP_ID)
    End Sub

    ''' <summary>
    ''' ロード処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then '初回表示

            '開始ログ出力
            psLogStart(Me)

            'ボタン活性
            Master.Master.ppRigthButton2.Visible = True '締め／解除
            Master.Master.ppRigthButton2.Enabled = False
            Master.Master.ppRigthButton1.Visible = True '当月集計
            Master.Master.ppRigthButton1.Enabled = False

            '表示設定
            Master.Master.ppRigthButton1.Text = sCnsShukei

            'プログラムＩＤ、画面名設定
            Master.Master.ppProgramID = M_DISP_ID
            Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

            'パンくずリスト設定
            Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)
            '20140604　件数チェックがあると初回起動ができない
            ''整備検収書データの件数チェック
            'If mfChkDataCnt() = False Then
            '    psMesBox(Me, "30015", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画前, "整備検収書データが存在しません。画面初期化")

            '    '読み込み失敗は画面を閉じる
            '    psClose_Window(Me)

            '    Return
            'End If

            '画面初期化処理
            Call msClearScreen()

            'ボタン活性制御.
            Call msSetEnable()

            Master.Master.ppRigthButton2.Enabled = False
            Master.Master.ppRigthButton1.Enabled = False

            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            '終了ログ出力
            psLogEnd(Me)

        End If

        'ボタンアクションの設定.
        Call msSet_ButtonAction()

    End Sub

    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
            Case "NGC"
        End Select

    End Sub

    ''' <summary>
    ''' 検索クリアボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        Dim blnClearFlg As Boolean = True
        Call msClearScreen(blnClearFlg)

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 検索ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        '検証チェック.
        If (Page.IsValid) Then

            '検索処理.
            Call msSearch_Data(Me.txtNendo.ppText.Replace("/", ""),
                               Me.txtNendo.ppText.Replace("/", ""),
                               Me.tftOrder_No.ppFromText,
                               Me.tftOrder_No.ppToText,
                               Me.dftDeliv_D.ppFromText,
                               Me.dftDeliv_D.ppToText,
                               Me.tftMente_No.ppFromText,
                               Me.tftMente_No.ppToText)
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 一覧の更新／参照ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        If e.CommandName <> "btnPrint" And e.CommandName <> "btnCsv" Then
            Exit Sub
        End If

        Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    ' ボタン押下行のIndex
        Dim rowData As GridViewRow = grvList.Rows(intIndex)             ' ボタン押下行
        Dim dtRowData As DataTable = Nothing
        Dim dstOrders As DataSet = Nothing
        dtRowData = pfParse_DataTable(grvList)
        Dim CsvData As New ArrayList
        Dim Cnt As Integer = 0
        Dim dt As New DataTable
        Dim rptDOCREP005 As New DOCREP005 '報告書兼検収書.
        Dim rptDOCREP006 As New DOCREP006 '通常整備料金.
        Dim rptMNTREP004 As New MNTREP004 '整備完了品一覧表.
        Dim strArrayList As String() = Nothing

        'ログ出力開始
        psLogStart(Me)

        'ビューに退避.
        ViewState("dt") = dtRowData
        ViewState("index") = intIndex

        Select Case e.CommandName
            Case "btnPrint"      '印刷.
                Select Case CType(rowData.FindControl("検収書"), TextBox).Text
                    Case "報告書兼検収書"
                        'CSVファイル取り込み.
                        'If mf_GetCSVData("0892CL", "情報機器整備の報告書兼検収書", CType(rowData.FindControl("検収年月"), TextBox).Text.ToString.Replace("/", ""), CsvData, Cnt, strArrayList) = False Then
                        '    Exit Sub
                        'End If
                        'CSVファイル取り込み.
                        If mf_GetCSVData("0892CL", "報告書兼検収書", CType(rowData.FindControl("検収年月"), TextBox).Text.ToString.Replace("/", ""), CsvData, Cnt, strArrayList) = False Then
                            Exit Sub
                        End If
                        ''PDFファイル作成.
                        'ms_MakePdf("情報機器整備の報告書兼検収書", CType(rowData.FindControl("検収年月"), TextBox).Text.ToString.Replace("/", ""), CsvData, Cnt, rptDOCREP005, strArrayList)
                        'PDFファイル作成.
                        ms_MakePdf("報告書兼検収書", CType(rowData.FindControl("検収年月"), TextBox).Text.ToString.Replace("/", ""), CsvData, Cnt, rptDOCREP005, strArrayList)


                    Case "通常整備料金"
                        'CNSOUTP001-003
                        'CSVファイル取り込み.
                        If CType(rowData.FindControl("検収年月"), TextBox).Text > "2017/06" Then
                            If mf_GetCSVData("0892CL", "TBOX通常整備料金", CType(rowData.FindControl("注文番号"), TextBox).Text & "_" & CType(rowData.FindControl("管理番号"), TextBox).Text, CsvData, Cnt, strArrayList) = False Then
                                Exit Sub
                            End If
                        Else
                            If mf_GetCSVData("0892CL", "TBOX通常整備料金", CType(rowData.FindControl("注文番号"), TextBox).Text, CsvData, Cnt, strArrayList) = False Then
                                Exit Sub
                            End If
                        End If
                        '                        If mf_GetCSVData("0892CL", "TBOX通常整備料金", CType(rowData.FindControl("注文番号"), TextBox).Text, CsvData, Cnt, strArrayList) = False Then
                        '                            Exit Sub
                        '                        End If
                        'PDFファイル作成.
                        If CType(rowData.FindControl("検収年月"), TextBox).Text > "2017/06" Then
                            ms_MakePdf("TBOX通常整備料金について", CType(rowData.FindControl("注文番号"), TextBox).Text, CsvData, Cnt, rptDOCREP006, strArrayList)
                        Else
                            ms_MakePdf("TBOX通常整備料金について", CType(rowData.FindControl("注文番号"), TextBox).Text, CsvData, Cnt, rptDOCREP006, strArrayList)
                        End If
'                        ms_MakePdf("TBOX通常整備料金について", CType(rowData.FindControl("注文番号"), TextBox).Text, CsvData, Cnt, rptDOCREP006, strArrayList)
'CNSOUTP001-003
                    Case "整備完了品一覧表"
                        'CSVファイル取り込み.
                        If mf_GetCSVData("0891CL", "整備完了品一覧表", CType(rowData.FindControl("注文番号"), TextBox).Text, CsvData, Cnt, strArrayList, CType(rowData.FindControl("納入日"), TextBox).Text) = False Then
                            Exit Sub
                        End If
                        'PDFファイル作成.
                        ms_MakePdf("整備完了品一覧表", CType(rowData.FindControl("注文番号"), TextBox).Text, CsvData, Cnt, rptMNTREP004, strArrayList)

                End Select


            Case "btnCsv"        'CSV出力.

                Select Case CType(rowData.FindControl("検収書"), TextBox).Text
                    Case "報告書兼検収書"
                        'If mfCsvDownload("0892CL", "情報機器整備の報告書兼検収書", CType(rowData.FindControl("検収年月"), TextBox).Text.ToString.Replace("/", "")) = False Then
                        '    psMesBox(Me, "10001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "CSV")
                        'End If
                        If mfCsvDownload("0892CL", "報告書兼検収書", CType(rowData.FindControl("検収年月"), TextBox).Text.ToString.Replace("/", "")) = False Then
                            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                        End If
                    Case "通常整備料金"
                        'CNSOUTP001-003
                        If CType(rowData.FindControl("検収年月"), TextBox).Text > "2017/06" Then
                            If mfCsvDownload("0892CL", "TBOX通常整備料金について", CType(rowData.FindControl("注文番号"), TextBox).Text & "_" & CType(rowData.FindControl("管理番号"), TextBox).Text) = False Then
                                psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                            End If
                        Else
                            If mfCsvDownload("0892CL", "TBOX通常整備料金について", CType(rowData.FindControl("注文番号"), TextBox).Text) = False Then
                                psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                            End If
                        End If
'                        If mfCsvDownload("0892CL", "TBOX通常整備料金について", CType(rowData.FindControl("注文番号"), TextBox).Text) = False Then
'                            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
'                        End If
'CNSOUTP001-003
                    Case "整備完了品一覧表"

                        If mfCsvDownload("0891CL", "整備完了品一覧表", CType(rowData.FindControl("注文番号"), TextBox).Text, CType(rowData.FindControl("納入日"), TextBox).Text) = False Then
                            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                        End If
                End Select


        End Select

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 当月集計ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCount_Click(sender As Object, e As EventArgs)

        If mfCheckClose(sender) = False Then
            'psMesBox(Me, "30005", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後)
            Exit Sub
        End If

        'ログ出力開始
        psLogStart(Me)

        Call msCount()

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 締め/締め解除ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClose_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        Select Case Master.Master.ppRigthButton2.Text
            Case "締め"

                If mfCheckClose(sender) = False Then
                    'psMesBox(Me, "30005", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後)
                    Exit Sub
                End If

                '案件終了フラグ更新
                If mfUpdateEndFlg("1", "1") = False Then
                    Exit Sub
                End If

                '締め処理.
                If mfUpdateClose("1", "0", "1") = False Then
                    Exit Sub
                End If

                Master.Master.ppRigthButton2.Text = "締め解除"

                '確認メッセージ再設定.
                Master.Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton2.Text).Replace(";", "") + "&&dispWait('unclose');"

            Case "締め解除"

                If mfCheckClose(sender) = False Then
                    'psMesBox(Me, "30005", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後)
                    Exit Sub
                End If

                '案件終了フラグ更新
                If mfUpdateEndFlg("0", "2") = False Then
                    Exit Sub
                End If

                '締め処理.
                'Call msUpdateClose("0", "1", "0")
                If mfUpdateClose("0", "1", "0") = False Then
                    Exit Sub
                End If

                Master.Master.ppRigthButton2.Text = "締め"

                '確認メッセージ再設定.
                Master.Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton2.Text).Replace(";", "") + "&&dispWait('close');"

        End Select

        'ログ出力終了
        psLogEnd(Me)

    End Sub

#End Region

#Region "そのほかのプロシージャ"
    ''' <summary>
    ''' 整備検収書データ件数のチェック
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfChkDataCnt() As Boolean
        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet
        mfChkDataCnt = False

        '初期化
        conDB = Nothing

        '接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                cmdDB = New SqlCommand(M_DISP_ID & "_S8", conDB)
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    mfChkDataCnt = False
                    Exit Function
                End If

                mfChkDataCnt = True

            Catch ex As Exception
                mfChkDataCnt = False
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    mfChkDataCnt = False
                End If
            End Try
        End If

    End Function

    ''' <summary>
    ''' 初期化処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen(Optional ByVal blnClearFlg = False)

        'Me.chkDocument.SelectedValue = String.Empty
        Me.chkDocument.SelectedIndex = -1
        Me.txtNendo.ppText = String.Empty
        Me.tftOrder_No.ppFromText = String.Empty
        Me.tftOrder_No.ppToText = String.Empty
        Me.dftDeliv_D.ppFromText = String.Empty
        Me.dftDeliv_D.ppToText = String.Empty
        Me.tftMente_No.ppFromText = String.Empty
        Me.tftMente_No.ppToText = String.Empty

        '初回のみ初期化.
        If blnClearFlg = False Then
            Me.grvList.DataSource = New Object() {}
            Master.ppCount = "0"
            Me.grvList.DataBind()
        End If

    End Sub

    ''' <summary>
    ''' ボタン活性制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetEnable()

        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet
        Dim intIns As Integer = 0
        Dim intReq As Integer = 0

        objStack = New StackFrame

        '初期化
        conDB = Nothing

        '接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                cmdDB = New SqlCommand(M_DISP_ID & "_S8", conDB)
                cmdDB.Parameters.Add(pfSet_Param("degree", SqlDbType.NVarChar, txtNendo.ppText.Replace("/", "")))
                cmdDB.Parameters.Add(pfSet_Param("search_flg", SqlDbType.NVarChar, "0"))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count > 0 Then
                    intIns = Integer.Parse(dstOrders.Tables(0).Rows(0).Item("検収締め区分"))
                    intReq = Integer.Parse(dstOrders.Tables(0).Rows(0).Item("請求締め区分"))

                    Select Case intIns + intReq
                        Case 0
                            cmdDB = New SqlCommand(M_DISP_ID & "_S8", conDB)
                            cmdDB.Parameters.Add(pfSet_Param("degree", SqlDbType.NVarChar, Date.Parse(Me.txtNendo.ppText).AddMonths(-1).ToString("yyyyMM")))
                            cmdDB.Parameters.Add(pfSet_Param("search_flg", SqlDbType.NVarChar, "0"))
                            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                            Master.Master.ppRigthButton2.Enabled = False
                            '前月の情報が無い、または前月が締め状態であれば締め処理が可能
                            If dstOrders.Tables(0).Rows.Count = 0 Then
                                Master.Master.ppRigthButton2.Enabled = True '締め/解除
                            ElseIf dstOrders.Tables(0).Rows(0).Item("請求締め区分") = "1" Then
                                Master.Master.ppRigthButton2.Enabled = True '締め/解除
                            End If

                            Master.Master.ppRigthButton1.Enabled = True
                            Master.Master.ppRigthButton2.Text = "締め"
                        Case 1
                            Master.Master.ppRigthButton2.Enabled = True
                            Master.Master.ppRigthButton1.Enabled = False
                            Master.Master.ppRigthButton2.Text = "締め解除"
                        Case 2
                            Master.Master.ppRigthButton1.Enabled = False
                            Master.Master.ppRigthButton2.Enabled = False
                            Master.Master.ppRigthButton2.Text = "締め"
                    End Select
                Else
                    Master.Master.ppRigthButton2.Enabled = False
                    Master.Master.ppRigthButton1.Enabled = True
                    Master.Master.ppRigthButton2.Text = "締め"
                End If

            Catch ex As Exception
                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ボタンアクションの設定.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_ButtonAction()

        'イベント設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click
        AddHandler Master.Master.ppRigthButton2.Click, AddressOf btnClose_Click   '締め／解除
        AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnCount_Click  '当月集計

        Master.ppRigthButton1.Attributes("onClick") = "dispWait('search');"


        '検証可否設定
        Master.ppRigthButton2.CausesValidation = False
        Master.Master.ppRigthButton1.CausesValidation = False
        Master.Master.ppRigthButton2.CausesValidation = False

        '確認メッセージ設定.
        If Master.Master.ppRigthButton2.Text = "締め" Then
            Master.Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton2.Text).Replace(";", "") + "&&dispWait('close');" '締め確認
        Else
            Master.Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton2.Text).Replace(";", "") + "&&dispWait('unclose');" '締め確認
        End If
        Master.Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton1.Text).Replace(";", "") + "&&dispWait('count');" '当月集計確認

    End Sub

    ''' <summary>
    ''' 検索処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSearch_Data(ByVal ipstrReq_f As String,
                              ByVal ipstrReq_t As String,
                              ByVal ipstrOrder_No_f As String,
                              ByVal ipstrOrder_No_t As String,
                              ByVal ipstrDeliv_D_f As String,
                              ByVal ipstrDeliv_D_t As String,
                              ByVal ipstrMente_No_f As String,
                              ByVal ipstrMente_No_t As String,
                              Optional ipstrSeach As String = "")

        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet
        Dim dtOrders As New DataTable
        Dim strDocument1 As String = String.Empty
        Dim strDocument2 As String = String.Empty
        Dim strDocument3 As String = String.Empty

        objStack = New StackFrame

        'チェックボックス判定.
        If ipstrSeach = "" Then

            If Me.chkDocument.Items(0).Selected = False Then
                strDocument1 = String.Empty
            Else
                strDocument1 = Me.chkDocument.Items(0).Value
            End If

            If Me.chkDocument.Items(1).Selected = False Then
                strDocument2 = String.Empty
            Else
                strDocument2 = Me.chkDocument.Items(1).Value
            End If

            If Me.chkDocument.Items(2).Selected = False Then
                strDocument3 = String.Empty
            Else
                strDocument3 = Me.chkDocument.Items(2).Value
            End If

        End If


        '初期化
        conDB = Nothing

        '接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"
        Else
            Try
                cmdDB = New SqlCommand(M_DISP_ID & "_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("document1", SqlDbType.NVarChar, mfGetDBNull(strDocument1)))
                    .Add(pfSet_Param("document2", SqlDbType.NVarChar, mfGetDBNull(strDocument2)))
                    .Add(pfSet_Param("document3", SqlDbType.NVarChar, mfGetDBNull(strDocument3)))
                    .Add(pfSet_Param("req_dt_f", SqlDbType.NVarChar, ipstrReq_f))
                    .Add(pfSet_Param("req_dt_t", SqlDbType.NVarChar, ipstrReq_t))
                    .Add(pfSet_Param("order_no_f", SqlDbType.NVarChar, ipstrOrder_No_f))
                    .Add(pfSet_Param("order_no_t", SqlDbType.NVarChar, ipstrOrder_No_t))
                    .Add(pfSet_Param("receipt_dt_f", SqlDbType.NVarChar, ipstrDeliv_D_f))
                    .Add(pfSet_Param("receipt_dt_t", SqlDbType.NVarChar, ipstrDeliv_D_t))
                    .Add(pfSet_Param("mente_no_f", SqlDbType.NVarChar, ipstrMente_No_f))
                    .Add(pfSet_Param("mente_no_t", SqlDbType.NVarChar, ipstrMente_No_t))
                    .Add(pfSet_Param("default", SqlDbType.NVarChar, "2"))
                    .Add(pfSet_Param("disp_id", SqlDbType.NVarChar, M_DISP_ID))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '件数を設定
                If dstOrders.Tables(0).Rows.Count = 0 Then
                    Master.ppCount = "0"
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)

                Else
                    '閾値を超えた場合はメッセージを表示
                    If dstOrders.Tables(0).Rows(0)("MAXROW") > dstOrders.Tables(0).Rows.Count Then
                        psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                            dstOrders.Tables(0).Rows(0)("MAXROW").ToString, dstOrders.Tables(0).Rows.Count.ToString)
                    End If
                    Master.ppCount = dstOrders.Tables(0).Rows(0)("MAXROW")
                End If

                '取得したデータをリストに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

                'CSVを非活性.
                dtOrders = pfParse_DataTable(grvList)

                For intCnt As Integer = 0 To dtOrders.Rows.Count - 1
                    'For intCnt As Integer = 1 To dtOrders.Rows.Count - 1
                    Dim grvrow As GridViewRow = grvList.Rows(intCnt)

                    '通常整備料金のCSVボタン非活性.
                    If CType(grvrow.FindControl("検収書"), TextBox).Text = "通常整備料金" Then
                        grvrow.Cells.Item(1).Enabled = False
                    End If

                    '検収締めをした月の印刷・CSVボタン非活性.
                    'If CType(grvrow.FindControl("検収締め区分"), TextBox).Text = "1" Then
                    '    grvrow.Cells.Item(0).Enabled = False
                    '    grvrow.Cells.Item(1).Enabled = False
                    'End If

                    '請求締めをした月の印刷・CSVボタン非活性.
                    'If CType(grvrow.FindControl("請求締め区分"), TextBox).Text = "1" Then
                    '    grvrow.Cells.Item(0).Enabled = False
                    '    grvrow.Cells.Item(1).Enabled = False

                    'End If

                Next

                If ipstrReq_f <> "" And ipstrOrder_No_f = "" And ipstrOrder_No_t = "" And ipstrDeliv_D_f = "" _
                                    And ipstrDeliv_D_t = "" And ipstrMente_No_f = "" And ipstrMente_No_t = "" Then
                    msSetEnable()
                Else
                    Master.Master.ppRigthButton2.Enabled = False
                    Master.Master.ppRigthButton1.Enabled = False
                End If

                'ビューに退避.
                ViewState("ds") = dstOrders

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "整備検収書")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()
                Master.ppCount = "0"
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If
    End Sub

    ''' <summary>
    ''' 当月集計処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCount()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstDegree As New DataSet '検収月用.
        Dim dstOrder As New DataSet '注文番号用.
        Dim dstCsv1 As New DataSet '整備完了品一覧CSV作成用.
        Dim dstCsv2 As New DataSet 'TBOX通常整備料金CSV作成用.
        Dim dstCsv3 As New DataSet '報告書兼検収書CSV作成用.
        Dim intSeq As Integer = 0
        Dim strError As String = String.Empty
        Dim dtInsert As New DataTable '登録・更新用
        Dim intTotal As Integer = 0
        Dim strSubTotal As New ArrayList

        objStack = New StackFrame

        dtInsert.Columns.Add("年月度", GetType(String))
        dtInsert.Columns.Add("連番", GetType(String))
        dtInsert.Columns.Add("検収書区分", GetType(String))
        dtInsert.Columns.Add("納入日", GetType(String))
        dtInsert.Columns.Add("注文番号", GetType(String))
        dtInsert.Columns.Add("管理番号", GetType(String))
        dtInsert.Columns.Add("合計金額", GetType(String))

        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "接続", "Catch")
        '接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try

                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "当月（検収月）算出処理", "Catch")
                '当月（検収月）算出処理.
                cmdDB = New SqlCommand(M_DISP_ID & "_S2", conDB)
                dstDegree = clsDataConnect.pfGet_DataSet(cmdDB)

                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "検収月の算出処理.", "Catch")
                ''検収月の算出処理.
                Dim strDegree As String = String.Empty
                'If dstDegree.Tables(0).Rows(0).Item("条件") = "未締め分" Then
                '    strDegree = dstDegree.Tables(0).Rows(0).Item("年月")
                'Else
                '    strDegree = dstDegree.Tables(0).Rows(0).Item("年月")
                'End If

                strDegree = Me.txtNendo.ppText.Replace("/", "")

                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "検収月の情報取得", "Catch")
                '検収月の情報取得.
                cmdDB = New SqlCommand(M_DISP_ID & "_S7", conDB)
                cmdDB.Parameters.Add(pfSet_Param("req_dt", SqlDbType.NVarChar, strDegree))
                dstOrder = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrder.Tables(0).Rows.Count = 0 Then
                    '    Master.ppCount = "0"
                    '    psMesBox(Me, "00005", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後)
                    '    Exit Sub
                End If

                Dim strOrder As String = ""

                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "CSV作成", "Catch")
                'CSV作成.
                For i As Integer = 0 To dstOrder.Tables(i).Rows.Count - 1

                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "整備完了品一覧CSV作成処理", "Catch")
                    '整備完了品一覧CSV作成処理.
                    cmdDB = New SqlCommand(M_DISP_ID & "_S3", conDB)
                    cmdDB.Parameters.Add(pfSet_Param("req_dt", SqlDbType.NVarChar, strDegree))
                    cmdDB.Parameters.Add(pfSet_Param("order_no", SqlDbType.NVarChar, dstOrder.Tables(0).Rows(i).Item("注文番号")))
                    cmdDB.Parameters.Add(pfSet_Param("rec_dt", SqlDbType.NVarChar, dstOrder.Tables(0).Rows(i).Item("納入日")))
                    dstCsv1 = clsDataConnect.pfGet_DataSet(cmdDB)

                    If dstCsv1 Is Nothing OrElse dstCsv1.Tables.Count <= 0 Then
                        Throw New Exception
                    End If

                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "日付項目を和暦変換", "Catch")
                    '日付項目を和暦変換
                    For Each objRow As DataRow In dstCsv1.Tables(0).Rows
                        'ヘッダ行は避ける
                        If objRow.Equals(dstCsv1.Tables(0).Rows(0)) Then
                            Continue For
                        End If
                        'msSetWareki(objRow, "帳票出力日", 0)
                        'msSetWareki(objRow, "納入日", 0)
                    Next

                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "CSV作成 整備完了品一覧表", "Catch")
                    'CSV作成.
                    If mfMakeCsv("0891CL", "整備完了品一覧表", dstOrder.Tables(0).Rows(i).Item("注文番号"), dstCsv1) = False Then
                        Exit Sub
                    End If

                    'CNSOUTP001-003
                    '                    If strOrder <> dstOrder.Tables(0).Rows(i).Item("注文番号") Then
                    '                        strOrder = dstOrder.Tables(0).Rows(i).Item("注文番号")
                    If strOrder <> dstOrder.Tables(0).Rows(i).Item("管理番号") Then
                        strOrder = dstOrder.Tables(0).Rows(i).Item("管理番号")
                        'CNSOUTP001-003

                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "TBOX通常整備料金CSV作成用", "Catch")
                        'TBOX通常整備料金CSV作成用.
                        cmdDB = New SqlCommand(M_DISP_ID & "_S4", conDB)
                        cmdDB.Parameters.Add(pfSet_Param("req_dt", SqlDbType.NVarChar, strDegree))
                        'CNSOUTP001-003
                        '                        cmdDB.Parameters.Add(pfSet_Param("order_no", SqlDbType.NVarChar, dstOrder.Tables(0).Rows(i).Item("注文番号")))
                        cmdDB.Parameters.Add(pfSet_Param("order_no", SqlDbType.NVarChar, dstOrder.Tables(0).Rows(i).Item("管理番号")))
                        'CNSOUTP001-003
                        dstCsv2 = clsDataConnect.pfGet_DataSet(cmdDB)

                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "管理毎の金額取得", "Catch")
                        '×注文番号ごとの金額取得.
                        '管理毎の金額取得
                        If dstCsv2.Tables(0).Rows.Count > 1 Then
                            strSubTotal.Add(dstCsv2.Tables(0).Rows(1).Item("合計金額"))
                        Else
                            strSubTotal.Add(0)
                        End If

                        If dstCsv2 Is Nothing OrElse dstCsv2.Tables.Count <= 0 Then
                            Throw New Exception()
                        End If

                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "日付項目を和暦変換", "Catch")
                        '日付項目を和暦変換
                        For Each objRow As DataRow In dstCsv2.Tables(0).Rows
                            'ヘッダ行は避ける
                            If objRow.Equals(dstCsv2.Tables(0).Rows(0)) Then
                                Continue For
                            End If
                            'msSetWareki(objRow, "帳票出力日", 0)
                            'msSetWareki(objRow, "検収年月", 1)
                        Next

                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "CSV作成 TBOX通常整備料金", "Catch")
                        'CSV作成.
                        'CNSOUTP001-003
                        If mfMakeCsv("0893CL", "TBOX通常整備料金", dstOrder.Tables(0).Rows(i).Item("注文番号") & "_" & dstOrder.Tables(0).Rows(i).Item("管理番号"), dstCsv2) = False Then
                            '                        If mfMakeCsv("0893CL", "TBOX通常整備料金", dstOrder.Tables(0).Rows(i).Item("注文番号"), dstCsv2) = False Then
                            'CNSOUTP001-003
                            Exit Sub
                        End If
                    End If
                Next

                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "合計金額計算", "Catch")
                '合計金額計算.
                For i As Integer = 0 To strSubTotal.Count - 1
                    intTotal += strSubTotal.Item(i)
                Next

                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "登録/更新用のテーブル作成", "Catch")
                '登録/更新用のテーブル作成.
                intSeq += 1

                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "報告書兼検収書", "Catch")
                '報告書兼検収書.
                dtInsert.Rows.Add(strDegree, intSeq, "0", "", "", "", intTotal)

                strOrder = ""
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "通常整備料金", "Catch")
                '通常整備料金.
                For i As Integer = 0 To dstOrder.Tables(0).Rows.Count - 1
                    'CNSOUTP001-003
                    '                    If strOrder <> dstOrder.Tables(0).Rows(i).Item("注文番号") Then
                    '                        strOrder = dstOrder.Tables(0).Rows(i).Item("注文番号")
                    If strOrder <> dstOrder.Tables(0).Rows(i).Item("管理番号") Then
                        strOrder = dstOrder.Tables(0).Rows(i).Item("管理番号")
                        'CNSOUTP001-003
                        intSeq += 1
                        dtInsert.Rows.Add(strDegree,
                                          intSeq,
                                          "1",
                                          "",
                                          dstOrder.Tables(0).Rows(i).Item("注文番号"),
                                          dstOrder.Tables(0).Rows(i).Item("管理番号"),
                                          strSubTotal(intSeq - 2))

                    End If
                Next

                '整備完了品一覧.
                For i As Integer = 0 To dstOrder.Tables(0).Rows.Count - 1
                    intSeq += 1

                    Dim strNonyu As String = ""
                    Dim dtVal As DateTime = Nothing
                    If dstOrder.Tables(0).Rows(i).Item("納入日") Is DBNull.Value Then
                        strNonyu = ""
                    Else
                        strNonyu = dstOrder.Tables(0).Rows(i).Item("納入日")
                    End If

                    If DateTime.TryParse(strNonyu, dtVal) Then
                        strNonyu = String.Format("{0:yyyy/MM/dd}", dtVal)
                    End If

                    If strNonyu.Trim() = "" Then
                        dtInsert.Rows.Add(strDegree,
                                      intSeq,
                                      "2",
                                      DBNull.Value,
                                      dstOrder.Tables(0).Rows(i).Item("注文番号"),
                                      dstOrder.Tables(0).Rows(i).Item("管理番号"),
                                      "")
                    Else
                        dtInsert.Rows.Add(strDegree,
                                      intSeq,
                                      "2",
                                      dstOrder.Tables(0).Rows(i).Item("納入日"),
                                      dstOrder.Tables(0).Rows(i).Item("注文番号"),
                                      dstOrder.Tables(0).Rows(i).Item("管理番号"),
                                      "")
                    End If
                Next

                '登録/更新処理.
                'If grvList.Rows.Count = 0 Then
                If mfInsert(dtInsert) = False Then
                    Exit Sub
                End If
                'Else
                '    If mfUpdate(dtInsert) = False Then
                '        Exit Sub
                '    End If
                'End If

                '報告書兼検収書CSV作成用.
                cmdDB = New SqlCommand(M_DISP_ID & "_S5", conDB)
                cmdDB.Parameters.Add(pfSet_Param("req_dt", SqlDbType.NVarChar, strDegree))
                dstCsv3 = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstCsv3 Is Nothing OrElse dstCsv3.Tables.Count <= 0 Then
                    Throw New Exception()
                End If

                Dim strKensyu As String = ""
                '日付項目を和暦変換
                For Each objRow As DataRow In dstCsv3.Tables(0).Rows
                    'ヘッダ行は避ける
                    If objRow.Equals(dstCsv3.Tables(0).Rows(0)) Then
                        Continue For
                    ElseIf objRow.Equals(dstCsv3.Tables(0).Rows(1)) Then
                        'ファイル名用検収年月保存
                        strKensyu = dstCsv3.Tables(0).Rows(1).Item("検収年月").ToString.Replace("/", "")
                    End If
                    msSetWareki(objRow, "帳票出力日", 0)
                    msSetWareki(objRow, "検収年月", 1)
                Next
                If strKensyu = "" Then
                    strKensyu = strDegree
                End If

                If mfMakeCsv("0892CL", "報告書兼検収書", strKensyu, dstCsv3) = False Then
                    Exit Sub
                End If

                '完了メッセージ.
                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, Master.Master.ppRigthButton1.Text)

                '再検索
                msSearch_Data(strDegree, "", "", "", "", "", "", "", "1")

                '画面状態更新
                msSetEnable()

            Catch ex As Exception
                'psMesBox(Me, "30015", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, Me.btnCount.Text)
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.Master.ppRigthButton1.Text)

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally

                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' 和暦セット
    ''' </summary>
    ''' <param name="objRow"></param>
    ''' <param name="strColNm"></param>
    ''' <remarks></remarks>
    Private Sub msSetWareki(ByRef objRow As DataRow, ByVal strColNm As String, ByVal intMode As Integer)
        Dim strVal1 As String = objRow(strColNm)

        If objRow(strColNm) Is DBNull.Value Then
            objRow(strColNm) = ""
            Return
        Else
            strVal1 = objRow(strColNm)
            objRow(strColNm) = mfChange_Jp(strVal1, intMode)

        End If

    End Sub

    ''' <summary>
    ''' 登録処理.
    ''' </summary>
    ''' <param name="dt">登録用テータテーブル</param>
    ''' <remarks></remarks>
    Private Function mfInsert(ByVal dt As DataTable) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing

        objStack = New StackFrame

        Try

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else

                'トランザクション.
                Using conTrn = conDB.BeginTransaction

                    For i As Integer = 0 To dt.Rows.Count - 1

                        cmdDB = New SqlCommand(M_DISP_ID & "_I1", conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = conTrn
                        With cmdDB.Parameters
                            .Add(pfSet_Param("degree", SqlDbType.NVarChar, dt.Rows(i).Item("年月度").ToString))
                            .Add(pfSet_Param("seq", SqlDbType.NVarChar, dt.Rows(i).Item("連番").ToString))
                            .Add(pfSet_Param("inspection_cls", SqlDbType.NVarChar, dt.Rows(i).Item("検収書区分").ToString))
                            .Add(pfSet_Param("dlv_dt", SqlDbType.NVarChar, mfGetDBNull(dt.Rows(i).Item("納入日").ToString)))
                            .Add(pfSet_Param("order_no", SqlDbType.NVarChar, mfGetDBNull(dt.Rows(i).Item("注文番号").ToString)))
                            .Add(pfSet_Param("mng_no", SqlDbType.NVarChar, mfGetDBNull(dt.Rows(i).Item("管理番号").ToString)))
                            .Add(pfSet_Param("price", SqlDbType.NVarChar, mfGetDBNull(dt.Rows(i).Item("合計金額").ToString)))
                            .Add(pfSet_Param("insert_user", SqlDbType.NVarChar, (Session(P_SESSION_USERID))))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With

                        '実行
                        cmdDB.ExecuteNonQuery()

                    Next

                    'コミット
                    conTrn.Commit()

                End Using

            End If
        Catch ex As Exception
            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "整備検収書")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Return False

        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

        End Try

        Return True

    End Function

    ''' <summary>
    ''' 更新処理.
    ''' </summary>
    ''' <param name="dt">更新用テータテーブル</param>
    ''' <remarks></remarks>
    Private Function mfUpdate(ByVal dt As DataTable) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing

        objStack = New StackFrame

        Try

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else

                'トランザクション.
                Using conTrn = conDB.BeginTransaction

                    For i As Integer = 0 To dt.Rows.Count - 1

                        cmdDB = New SqlCommand(M_DISP_ID & "_U1", conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = conTrn
                        With cmdDB.Parameters
                            .Add(pfSet_Param("degree", SqlDbType.NVarChar, dt.Rows(i).Item("年月度").ToString))
                            .Add(pfSet_Param("seq", SqlDbType.NVarChar, dt.Rows(i).Item("連番").ToString))
                            .Add(pfSet_Param("inspection_cls", SqlDbType.NVarChar, dt.Rows(i).Item("検収書区分").ToString))
                            .Add(pfSet_Param("dlv_dt", SqlDbType.NVarChar, mfGetDBNull(dt.Rows(i).Item("納入日").ToString)))
                            .Add(pfSet_Param("order_no", SqlDbType.NVarChar, mfGetDBNull(dt.Rows(i).Item("注文番号").ToString)))
                            .Add(pfSet_Param("mng_no", SqlDbType.NVarChar, mfGetDBNull(dt.Rows(i).Item("管理番号").ToString)))
                            .Add(pfSet_Param("price", SqlDbType.NVarChar, mfGetDBNull(dt.Rows(i).Item("合計金額").ToString)))
                            .Add(pfSet_Param("update_user", SqlDbType.NVarChar, (Session(P_SESSION_USERID))))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With

                        '実行
                        cmdDB.ExecuteNonQuery()

                    Next

                    'コミット
                    conTrn.Commit()

                End Using

            End If
        Catch ex As Exception
            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "整備検収書")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Return False
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

        End Try

        Return True

    End Function

    ''' <summary>
    ''' 案件終了フラグ更新処理
    ''' </summary>
    ''' <param name="strEnd_Flg"></param>
    ''' <param name="strClose_Flg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfUpdateEndFlg(ByVal strEnd_Flg As String, ByVal strClose_Flg As String) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim intRtn As Integer
        mfUpdateEndFlg = False

        objStack = New StackFrame

        Try
            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else

                'トランザクション.
                Using conTrn = conDB.BeginTransaction
                    cmdDB = New SqlCommand(M_DISP_ID & "_U3", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = conTrn
                    With cmdDB.Parameters
                        .Add(pfSet_Param("USER_ID", SqlDbType.NVarChar, (Session(P_SESSION_USERID))))
                        .Add(pfSet_Param("MENTE_END_FLG", SqlDbType.NVarChar, strEnd_Flg))
                        .Add(pfSet_Param("CLOSE_FLG", SqlDbType.NVarChar, strClose_Flg))
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                    End With

                    '実行
                    cmdDB.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.Master.ppRigthButton2.Text)

                        'ロールバック
                        conTrn.Rollback()
                        Exit Function
                    End If

                    'コミット
                    conTrn.Commit()

                End Using

                mfUpdateEndFlg = True

            End If
        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.Master.ppRigthButton2.Text)
            mfUpdateEndFlg = False
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                mfUpdateEndFlg = False
            End If

        End Try
    End Function

    ''' <summary>
    ''' 締め・締め解除処理.
    ''' </summary>
    ''' <param name="ipstrIns_cls_s"></param>
    ''' <param name="ipstrIns_cls_w"></param>
    ''' <remarks></remarks>
    Private Function mfUpdateClose(ByVal ipstrIns_cls_s As String,
                                   ByVal ipstrIns_cls_w As String,
                                   ByVal ipstrIns_cls_flg As String) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim intRtn As Integer
        Dim strDegreeYm As String = String.Empty
        mfUpdateClose = False

        objStack = New StackFrame

        Try

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else

                'トランザクション.
                Using conTrn = conDB.BeginTransaction
                    cmdDB = New SqlCommand(M_DISP_ID & "_U2", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = conTrn
                    With cmdDB.Parameters
                        .Add(pfSet_Param("ins_cls_s", SqlDbType.NVarChar, ipstrIns_cls_s))
                        .Add(pfSet_Param("ins_cls_w", SqlDbType.NVarChar, ipstrIns_cls_w))
                        .Add(pfSet_Param("ins_cls_flg", SqlDbType.NVarChar, ipstrIns_cls_flg))
                        .Add(pfSet_Param("update_user", SqlDbType.NVarChar, (Session(P_SESSION_USERID))))
                        .Add(pfSet_Param("degree", SqlDbType.NVarChar, Me.txtNendo.ppText.Replace("/", "")))
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                    End With

                    '実行
                    cmdDB.ExecuteNonQuery()

                    '年月度取得
                    strDegreeYm = Me.txtNendo.ppText.Replace("/", "")

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        'psMesBox(Me, "30015", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, Me.btnClose.Text)
                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.Master.ppRigthButton2.Text)

                        'ロールバック
                        conTrn.Rollback()

                        Exit Function
                    End If

                    'コミット
                    conTrn.Commit()

                    '完了メッセージ.
                    'psMesBox(Me, "30008", clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後, Me.btnClose.Text)
                    psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, Master.Master.ppRigthButton2.Text)

                End Using

                '再検索
                msSearch_Data(strDegreeYm, "", "", "", "", "", "", "", "1")

                '画面状態更新
                msSetEnable()

                'msSearch_Data("", "", "", "", "", "", "", "")
                mfUpdateClose = True

            End If
        Catch ex As Exception
            'psMesBox(Me, "30015", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, Me.btnClose.Text)
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.Master.ppRigthButton2.Text)
            mfUpdateClose = False

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

                mfUpdateClose = False

            End If

        End Try
    End Function

    '-----------------------------
    '2014/04/14 Hamamoto　ここから
    '-----------------------------
    ''' <summary>
    ''' CSVファイル取り込み.
    ''' </summary>
    ''' <param name="ipstrFileclassCD"></param>
    ''' <param name="ipstrReportName"></param>
    ''' <param name="ipstrOrderNo"></param>
    ''' <param name="CSVData"></param>
    ''' <param name="Cnt"></param>
    ''' <remarks></remarks>
    Private Function mf_GetCSVData(ByVal ipstrFileclassCD As String,
                              ByVal ipstrReportName As String,
                              ByVal ipstrOrderNo As String,
                              ByRef CSVData As ArrayList,
                              ByRef Cnt As Integer,
                              ByRef strArrayList As String(),
                              Optional ipstrRecDt As String = "") As Boolean
        ' Private Sub ms_GetCSVData(ByVal ipstrFileclassCD As String,        
        'ByVal ipstrReportName As String,
        '                      ByVal ipstrOrderNo As String,
        '                      ByRef CSVData As ArrayList,
        '                      ByRef Cnt As Integer,
        '                      ByRef strArrayList As String())
        Dim strServerAddress As String = String.Empty    'サーバアドレス
        Dim strFolderNM As String = String.Empty         'フォルダ名
        Dim strFilePath As String = String.Empty         'ファイルパス
        Dim strDownloadFileName As String = String.Empty '出力パス
        Dim strData As String = String.Empty
        Dim utf As Encoding = Encoding.UTF8
        Dim sjis As Encoding = Encoding.GetEncoding("utf-8")
        Dim strFileName As String = String.Empty
        Dim intCnt As Integer
        Dim opblnResult As Boolean = False
        Dim DBFTP As New DBFTP.ClsDBFTP_Main

        'サーバアドレス取得.
        If pfGetPreservePlace(ipstrFileclassCD, strServerAddress, strFolderNM) <> 0 Then
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
            Return False
        End If

        'パス生成.
        'strFilePath = "\\" & strServerAddress & "\" & strFolderNM & "\" & ipstrReportName & "_[" & ipstrOrderNo & "]" & ".csv"

        intCnt = 1
        If ipstrReportName = "整備完了品一覧表" Then
            Do While 1
                If ipstrRecDt = "" Then
                    strFileName = ipstrReportName & "_" & Me.txtNendo.ppText.Replace("/", "") & "_" & "[" & ipstrOrderNo & "]_納入日なし_" & intCnt.ToString("00") & ".csv"
                Else
                    strFileName = ipstrReportName & "_" & Me.txtNendo.ppText.Replace("/", "") & "_" & "[" & ipstrOrderNo & "]_" & Date.Parse(ipstrRecDt).ToString("yyyyMMdd") & "_" & intCnt.ToString("00") & ".csv"
                End If

                If DBFTP.pfFtpFile_Exists(strFolderNM & "\", strFileName, opblnResult) = False Then
                    Exit Do
                End If
                strData = strFileName
                'strFilePath = "\\" & strServerAddress & "\" & strFolderNM & "\" & strFileName
                intCnt += 1
            Loop
        Else
            If ipstrReportName = "TBOX通常整備料金" Then
                Do While 1
                    strFileName = ipstrReportName & "_" & "[" & ipstrOrderNo & "]_" & intCnt.ToString("00") & ".csv"

                    If DBFTP.pfFtpFile_Exists(strFolderNM & "\", strFileName, opblnResult) = False Then
                        Exit Do
                    End If
                    strData = strFileName
                    'strFilePath = "\\" & strServerAddress & "\" & strFolderNM & "\" & strFileName
                    intCnt += 1
                Loop
            Else
                'strFilePath = "\\" & strServerAddress & "\" & strFolderNM & "\" & ipstrReportName & "_" & "[" & ipstrOrderNo & "]" & ".csv"
                strData = ipstrReportName & "_" & "[" & ipstrOrderNo & "]" & ".csv"
            End If
        End If

        strFilePath = pfFile_Download(strFolderNM, strData)
        If strFilePath = "" Then
            psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
            Return False
        End If

        ''ファイル存在確認
        'If System.IO.File.Exists(strFilePath) = False Then
        '    psMesBox(Me, "30002", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "CSV")
        '    Return False
        'End If

        Dim sr As StreamReader = New StreamReader(strFilePath, System.Text.Encoding.Default)  'ファイルの読み込み
        Dim errMsg As String = "ＣＳＶファイル"                                            'エラーメッセージ

        objStack = New StackFrame

        Try
            Dim strHead As String = sr.ReadLine                                                'ヘッダ行の読み込み

            Dim strHeader As String = strHead.Replace("""", String.Empty)

            strArrayList = strHeader.ToString.Split(",")

            Dim strRep As String = strHead.Replace(",", String.Empty)                          'カンマを削除
            Dim strLin As String = Nothing                                                     'CSVデータ(一行)
            Dim tmpLin As String = Nothing                                                     'CSVデータ(一時保管)
            Dim loopFlg As Boolean = False                                                     'ループ間のフラグ
            Dim loopCnt As Integer = 0                                                         'ループ回数
            Dim fstqt As Boolean = False                                                       '囲み開始フラグ

            Cnt = strHead.Length - strRep.Length                                               'カンマの数を算出

            Dim num As Integer = 0                                                             'カンマ数

            'CSVファイル内の整形開始
            '一行づつ読み込む
            Do Until sr.EndOfStream = True

                Dim strMoji As String = Nothing                                                '一文字格納


                strLin = sr.ReadLine '一行読み込み

                'CSVファイル読み込みカウントアップ
                loopCnt = loopCnt + 1

                'カンマの数を調べる
                '文字数分カウント
                For i As Integer = 0 To strLin.Length - 1

                    '一文字ずつ抽出
                    strMoji = strLin.Substring(i, 1)

                    If strMoji = """" And fstqt = False Then '先頭の囲み

                        fstqt = True

                    ElseIf strMoji = "," And fstqt = True Then 'カンマが""で囲まれている

                        'カンマを別文字に置き換える
                        tmpLin = tmpLin + "‥"

                    ElseIf fstqt = False Then 'カンマが""で囲まれていない

                        '文字を連結
                        tmpLin = tmpLin + strMoji
                        'カンマ数をカウント
                        num = num + 1

                    ElseIf strMoji <> """" And fstqt = True Then '""で囲まれている文字

                        '文字を連結
                        tmpLin = tmpLin + strMoji

                    ElseIf strMoji = """" And fstqt = True Then '囲み終了

                        fstqt = False

                    End If

                Next

                If fstqt = False Then

                    'カンマの数が間違っている場合
                    If num <> Cnt Then

                        Throw New Exception

                    End If

                    CSVData.Add(tmpLin)         '保存
                    tmpLin = Nothing
                    num = 0
                End If

            Loop

            Return True
        Catch ex As Exception

            'システムエラー
            psMesBox(Me, "30002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, errMsg)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Throw ex
            Return False
        Finally

            sr.Close()                 'ファイルクローズ

        End Try

    End Function

    ''' <summary>
    ''' PDF作成.
    ''' </summary>
    ''' <param name="CsvDate"></param>
    ''' <param name="Cnt"></param>
    ''' <remarks></remarks>
    Private Sub ms_MakePdf(ByVal ipstrReportName As String, ipstrOrderNo As String, CsvDate As ArrayList, ByVal Cnt As Integer, ByVal rpt As Object, ByVal strArrayList As String())
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
        Dim dt As New DataTable

        If ipstrReportName = "報告書兼検収書" Then
            ipstrReportName = "情報機器整備の報告書兼検収書"
        End If

        Dim strFNm As String = ipstrReportName & "_" & ipstrOrderNo & "_" & Date.Today.ToString("yyyyMMddHHmmss")  '帳票名称

        'データテーブルカラム作成(ヘッダ)
        For i As Integer = 0 To strArrayList.Length - 1
            'dt.Columns.Add((i).ToString("D"))
            dt.Columns.Add(strArrayList(i))
        Next

        Dim strNow As String = String.Format("{0:yyyy/MM/dd}", DateTime.Now)

        Select Case ipstrReportName
            Case "整備完了品一覧表"
                dt.Columns.Add("区切り")
            Case "TBOX通常整備料金について"
                dt.Columns.Add("空行フラグ")
            Case "情報機器整備の報告書兼検収書"
                dt.Columns.Add("空行フラグ")
        End Select

        If CsvDate.Count <= 0 Then
            'システムエラー
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
            Return
        End If

        '最初の行の注文番号を取得
        Dim strOrderNo As String = ""
        strOrderNo = CsvDate(0).ToString.Split(",")(eColKanryoCsv.注文番号)

        '行カウンター
        Dim intCnt As Integer = 0

        '区分初期値
        Dim strKbnVal As String = CsvDate(0).ToString.Split(",")(eColKanryoCsv.区分)
        Select Case ipstrReportName
            Case "整備完了品一覧表"
                msSetMNTREP004RptData(dt, CsvDate, strNow, rpt)
            Case "TBOX通常整備料金について"
                msSetDOCREP006RptData(dt, CsvDate, strNow, rpt)
            Case "情報機器整備の報告書兼検収書"
                msSetDOCREP005RptData(dt, CsvDate, strNow, rpt)
            Case Else
                Return
        End Select

        'PDF出力処理.
        psPrintPDF(Me, rpt, dt, strFNm)
    End Sub

    ''' <summary>
    ''' データセット(整備完了品一覧表)
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <param name="CsvDate"></param>
    ''' <param name="strNow"></param>
    ''' <remarks></remarks>
    Private Sub msSetMNTREP004RptData(ByRef dt As DataTable, ByVal CsvDate As ArrayList, ByVal strNow As String, ByRef rpt As MNTREP004)
        Dim strCsv() As String
        'CNSOUTP001-002 
        Dim dtr As DataRow = mfGetTraderInfo("3")
        dt.Columns.Add("自社名")
        dt.Columns("自社名").DefaultValue = dtr.Item("M40_SEND_FNM")
        'CNSOUTP001-002 END

        '最初の行の注文番号を取得
        Dim strOrderNo As String = ""
        strOrderNo = CsvDate(0).ToString.Split(",")(eColKanryoCsv.注文番号)

        '行カウンター
        Dim intCnt As Integer = 0

        '区分初期値
        Dim strKbnVal As String = CsvDate(0).ToString.Split(",")(eColKanryoCsv.区分)
        For i As Integer = 0 To CsvDate.Count - 1
            'カンマ毎に分割
            Dim strCsvVal As String = CsvDate(i).ToString & ",0"
            strCsv = strCsvVal.Split(",")

            '注文番号が変わったか？
            If strOrderNo <> strCsv(eColKanryoCsv.注文番号) Then
                '必要な空行を作成
                msSetKanryoEmptyRow((CsvDate(i - 1).ToString & ",1").Split(","), MAX_ROW_CNT_FINISH - intCnt, dt)

                '改ページのためカウントリセット
                intCnt = 0

                '区分リセット
                strKbnVal = "ＯＫ"

                '新注文番号記憶
                strOrderNo = strCsv(eColKanryoCsv.注文番号)
            Else
                '区切り必要？
                If strKbnVal = "ＯＫ" AndAlso strKbnVal <> strCsv(eColKanryoCsv.区分).ToString _
                              AndAlso intCnt <> MAX_ROW_CNT_FINISH Then

                    '区分変更
                    strKbnVal = "ＮＧ"

                    If intCnt = 0 Then
                    Else
                        '空行追加
                        msSetKanryoEmptyRow((CsvDate(i - 1).ToString & ",1").Split(","), 1, dt)
                    End If
                    '空行追加
                    'msSetKanryoEmptyRow((CsvDate(i - 1).ToString & ",1").Split(","), 1, dt)

                    '行カウント追加
                    intCnt += 1

                    '20件になったか？
                    If intCnt = MAX_ROW_CNT_FINISH Then
                        'カウントリセット
                        intCnt = 0

                    End If

                End If
            End If

            dt.Rows.Add(strCsv)


            '行カウント追加
            intCnt += 1

            'データが終わったら、のこりの空行をセット
            If i = CsvDate.Count - 1 Then
                'msSetKanryoEmptyRow((CsvDate(i - 1).ToString & ",1").Split(","), MAX_ROW_CNT_FINISH - intCnt, dt)
                msSetKanryoEmptyRow((CsvDate(0).ToString & ",1").Split(","), MAX_ROW_CNT_FINISH - intCnt, dt)
            End If

            '20件になったか？
            If intCnt = MAX_ROW_CNT_FINISH Then
                'カウントリセット
                intCnt = 0
            End If

        Next


        rpt.RepDataCount = dt.Rows.Count

    End Sub

    ''' <summary>
    ''' データセット(TBOX通常整備料金について)
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <param name="CsvDate"></param>
    ''' <param name="strNow"></param>
    ''' <param name="rpt"></param>
    ''' <remarks></remarks>
    Private Sub msSetDOCREP006RptData(ByRef dt As DataTable, ByVal CsvDate As ArrayList, ByVal strNow As String, ByRef rpt As DOCREP006)
        Dim strCsv() As String

        '最初の行の注文番号を取得
        Dim strOrderNo As String = ""
        strOrderNo = CsvDate(0).ToString.Split(",")(eColTujoCsv.注文番号)

        '行カウンター
        Dim intCnt As Integer = 0

        For i As Integer = 0 To CsvDate.Count - 1
            'カンマ毎に分割
            Dim strCsvVal As String = CsvDate(i).ToString & ",0"
            strCsv = strCsvVal.Split(",")

            '注文番号が変わったか？
            If strOrderNo <> strCsv(eColTujoCsv.注文番号) Then
                '必要な空行を作成
                msSetTboxEmptyRow((CsvDate(i - 1).ToString & ",1").Split(","), MAX_ROW_CNT_TBOX - intCnt, dt)

                '改ページのためカウントリセット
                intCnt = 0

                '新注文番号記憶
                strOrderNo = strCsv(eColTujoCsv.注文番号)
            Else

                '20件になったか？
                If intCnt = MAX_ROW_CNT_FINISH Then
                    'カウントリセット
                    intCnt = 0

                End If

            End If

            dt.Rows.Add(strCsv)

            '行カウント追加
            intCnt += 1

            'データが終わったら、のこりの空行をセット
            If i = CsvDate.Count - 1 Then
                'msSetTboxEmptyRow((CsvDate(i - 1).ToString & ",1").Split(","), MAX_ROW_CNT_TBOX - intCnt, dt)
                msSetTboxEmptyRow((CsvDate(0).ToString & ",1").Split(","), MAX_ROW_CNT_TBOX - intCnt, dt)
            End If

            '20件になったか？
            If intCnt = MAX_ROW_CNT_FINISH Then
                'カウントリセット
                intCnt = 0
            End If

        Next
        'CNSOUTP001-002 
        Dim dtrSend As DataRow = mfGetTraderInfo("7")
        Dim dtrRcv As DataRow = mfGetTraderInfo("8")
        For Each dtr As DataRow In dt.Rows
            dtr.Item("宛先会社名") = dtrRcv.Item("M40_SEND_NM1")
            dtr.Item("宛先部署名") = dtrRcv.Item("M40_SEND_NM1_2")
            dtr.Item("発行元住所") = dtrSend.Item("M40_ADDR1")
        Next
        'CNSOUTP001-002 END
        'データ数記憶
        rpt.RepDataCount = dt.Rows.Count

    End Sub

    ''' <summary>
    ''' データセット(情報機器整備の報告書兼検収書)
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <param name="CsvDate"></param>
    ''' <param name="strNow"></param>
    ''' <param name="rpt"></param>
    ''' <remarks></remarks>
    Private Sub msSetDOCREP005RptData(ByRef dt As DataTable, ByVal CsvDate As ArrayList, ByVal strNow As String, ByRef rpt As DOCREP005)

        Dim strCsv() As String
        'MNTOUTP001-004
        Dim strKeepOrdNo As String = ""
        Dim decTotal As Decimal = 0 '金額
        Dim strBuff As String = ""
        Dim aryBuff As String()
        Dim strKeep As String
        Dim arlTotal As New ArrayList
        'MNTOUTP001-004
        '行カウンター
        Dim intCnt As Integer = 0

        'MNTOUTP001-004
        '注文番号毎に集計
        If CsvDate.Count > 0 Then
            strBuff = CsvDate(0).ToString
            strKeep = CsvDate(0).ToString
            aryBuff = strBuff.Split(",")
            strKeepOrdNo = aryBuff(eColHokokuKensyuCsv.注文番号)

            For zz As Integer = 0 To CsvDate.Count - 1
                Dim strCsvVal As String = CsvDate(zz).ToString
                strCsv = strCsvVal.Split(",")
                If strKeepOrdNo <> strCsv(eColHokokuKensyuCsv.注文番号) Then
                    strCsv = strKeep.Split(",")
                    '金額入れ替え
                    strCsv(eColHokokuKensyuCsv.金額) = decTotal
                    '新規配列に書き出し
                    arlTotal.Add(String.Join(",", strCsv))
                    '現在の情報を読み直し
                    strCsv = strCsvVal.Split(",")
                    '新キーを保管
                    strKeepOrdNo = strCsv(eColHokokuKensyuCsv.注文番号)
                    '金額を保管
                    decTotal = strCsv(eColHokokuKensyuCsv.金額)
                    '各項目を保管
                    strKeep = CsvDate(zz).ToString
                Else
                    decTotal += strCsv(eColHokokuKensyuCsv.金額)
                End If
            Next
            '最終行を出力
            If decTotal > 0 Then
                strCsv = strKeep.Split(",")
                '金額入れ替え
                strCsv(eColHokokuKensyuCsv.金額) = decTotal
                '新規配列に書き出し
                arlTotal.Add(String.Join(",", strCsv))
            End If
            For i As Integer = 0 To arlTotal.Count - 1
                'カンマ毎に分割
                Dim strCsvVal As String = arlTotal(i).ToString & ",0"
                strCsv = strCsvVal.Split(",")

                '金額データの編集
                msSetZero(strCsv, eColHokokuKensyuCsv.金額)
                msSetZero(strCsv, eColHokokuKensyuCsv.合計金額)
                msSetZero(strCsv, eColHokokuKensyuCsv.課税対象料金金額)
                msSetZero(strCsv, eColHokokuKensyuCsv.出精値引き)
                msSetZero(strCsv, eColHokokuKensyuCsv.小計)
                msSetZero(strCsv, eColHokokuKensyuCsv.消費税相当額)
                msSetZero(strCsv, eColHokokuKensyuCsv.値引率)

                dt.Rows.Add(strCsv)

                '行カウント追加
                intCnt += 1

                If i = arlTotal.Count - 1 AndAlso intCnt < MAX_ROW_CNT_HOKOKUKENSYU Then
                    '空行追加
                    msSetHokokuEmptyRow(strCsv, MAX_ROW_CNT_HOKOKUKENSYU - intCnt, dt)
                End If

            Next
        Else
            For i As Integer = 0 To CsvDate.Count - 1
                'カンマ毎に分割
                Dim strCsvVal As String = CsvDate(i).ToString & ",0"
                strCsv = strCsvVal.Split(",")

                '金額データの編集
                msSetZero(strCsv, eColHokokuKensyuCsv.金額)
                msSetZero(strCsv, eColHokokuKensyuCsv.合計金額)
                msSetZero(strCsv, eColHokokuKensyuCsv.課税対象料金金額)
                msSetZero(strCsv, eColHokokuKensyuCsv.出精値引き)
                msSetZero(strCsv, eColHokokuKensyuCsv.小計)
                msSetZero(strCsv, eColHokokuKensyuCsv.消費税相当額)
                msSetZero(strCsv, eColHokokuKensyuCsv.値引率)

                dt.Rows.Add(strCsv)

                '行カウント追加
                intCnt += 1

                If i = CsvDate.Count - 1 AndAlso intCnt < MAX_ROW_CNT_HOKOKUKENSYU Then
                    '空行追加
                    msSetHokokuEmptyRow(strCsv, MAX_ROW_CNT_HOKOKUKENSYU - intCnt, dt)
                End If

            Next
        End If


        'For i As Integer = 0 To CsvDate.Count - 1
        '    'カンマ毎に分割
        '    Dim strCsvVal As String = CsvDate(i).ToString & ",0"
        '    strCsv = strCsvVal.Split(",")

        '    '金額データの編集
        '    msSetZero(strCsv, eColHokokuKensyuCsv.金額)
        '    msSetZero(strCsv, eColHokokuKensyuCsv.合計金額)
        '    msSetZero(strCsv, eColHokokuKensyuCsv.課税対象料金金額)
        '    msSetZero(strCsv, eColHokokuKensyuCsv.出精値引き)
        '    msSetZero(strCsv, eColHokokuKensyuCsv.小計)
        '    msSetZero(strCsv, eColHokokuKensyuCsv.消費税相当額)
        '    msSetZero(strCsv, eColHokokuKensyuCsv.値引率)

        '    dt.Rows.Add(strCsv)

        '    '行カウント追加
        '    intCnt += 1

        '    If i = CsvDate.Count - 1 AndAlso intCnt < MAX_ROW_CNT_HOKOKUKENSYU Then
        '        '空行追加
        '        msSetHokokuEmptyRow(strCsv, MAX_ROW_CNT_HOKOKUKENSYU - intCnt, dt)
        '    End If

        'Next
        'MNTOUTP001-004

        'CNSOUTP001-002 
        Dim dtrSend As DataRow = mfGetTraderInfo("7")
        Dim dtrRcv As DataRow = mfGetTraderInfo("8")
        For Each dtr As DataRow In dt.Rows
            dtr.Item("宛先会社名") = dtrRcv.Item("M40_SEND_NM1")
            dtr.Item("宛先部署名") = dtrRcv.Item("M40_SEND_NM1_2")
            dtr.Item("確認後宛先会社名") = dtrSend.Item("M40_SEND_NM1")
            dtr.Item("確認後宛先部署名") = dtrSend.Item("M40_SEND_NM1_2") & "　" & dtrSend.Item("M40_SEND_NM1_3")
            dtr.Item("発行元住所") = dtrSend.Item("M40_ADDR1")
            dtr.Item("確認後宛先TEL") = dtrSend.Item("M40_TELNO1")
            dtr.Item("確認後宛先FAX") = dtrSend.Item("M40_FAXNO1")
            dtr.Item("検収者会社名") = dtrRcv.Item("M40_SEND_NM1")
            dtr.Item("検収者部署名") = dtrRcv.Item("M40_SEND_NM1_3")
        Next
        'CNSOUTP001-002 END

    End Sub

    ''' <summary>
    ''' 0円設定
    ''' </summary>
    ''' <param name="strCsv"></param>
    ''' <param name="intCol"></param>
    ''' <remarks></remarks>
    Private Sub msSetZero(ByRef strCsv As String(), ByVal intCol As Integer)
        If strCsv(intCol) Is Nothing OrElse strCsv(intCol).Trim() = "" Then
            strCsv(intCol) = "0"
        End If
    End Sub

    ''' <summary>
    ''' 空行セット
    ''' </summary>
    ''' <param name="strCsvRow"></param>
    ''' <param name="intAddRowCnt"></param>
    ''' <param name="dt"></param>
    ''' <remarks></remarks>
    Private Sub msSetKanryoEmptyRow(ByVal strCsvRow() As String, ByVal intAddRowCnt As Integer, ByRef dt As DataTable)
        'カウント数分から行を追加する
        For i As Integer = 0 To intAddRowCnt - 1
            Dim objRow As DataRow = dt.NewRow()
            objRow(eColKanryoCsv.帳票出力日) = strCsvRow(eColKanryoCsv.帳票出力日)
            objRow(eColKanryoCsv.送付先名) = strCsvRow(eColKanryoCsv.送付先名)
            objRow(eColKanryoCsv.納入日) = strCsvRow(eColKanryoCsv.納入日)
            objRow(eColKanryoCsv.送付元名) = strCsvRow(eColKanryoCsv.送付元名)
            objRow(eColKanryoCsv.作業内容) = strCsvRow(eColKanryoCsv.作業内容)
            objRow(eColKanryoCsv.バージョン) = strCsvRow(eColKanryoCsv.バージョン)
            objRow(eColKanryoCsv.注文番号) = strCsvRow(eColKanryoCsv.注文番号)
            objRow(eColKanryoCsv.品名) = ""
            objRow(eColKanryoCsv.区分) = ""
            objRow(eColKanryoCsv.シリアルNo) = ""
            objRow(eColKanryoCsv.備考) = ""
            objRow(eColKanryoCsv.数量) = "0"
            objRow(eColKanryoCsv.合計数量) = strCsvRow(eColKanryoCsv.合計数量)
            objRow(eColKanryoCsv.区切り) = "1"

            dt.Rows.Add(objRow)
        Next
    End Sub

    ''' <summary>
    ''' 空行セット(TBOX通常整備料金について)
    ''' </summary>
    ''' <param name="strCsvRow"></param>
    ''' <param name="intAddRowCnt"></param>
    ''' <param name="dt"></param>
    ''' <remarks></remarks>
    Private Sub msSetTboxEmptyRow(ByVal strCsvRow() As String, ByVal intAddRowCnt As Integer, ByRef dt As DataTable)
        'カウント数分から行を追加する
        For i As Integer = 0 To intAddRowCnt - 1
            Dim objRow As DataRow = dt.NewRow()
            objRow(eColTujoCsv.帳票出力日) = strCsvRow(eColTujoCsv.帳票出力日)
            objRow(eColTujoCsv.宛先会社名) = strCsvRow(eColTujoCsv.宛先会社名)
            objRow(eColTujoCsv.宛先部署名) = strCsvRow(eColTujoCsv.宛先部署名)
            objRow(eColTujoCsv.発行元会社名) = strCsvRow(eColTujoCsv.発行元会社名)
            objRow(eColTujoCsv.発行元住所) = strCsvRow(eColTujoCsv.発行元住所)
            objRow(eColTujoCsv.検収年月) = strCsvRow(eColTujoCsv.検収年月)
            objRow(eColTujoCsv.システム種別) = strCsvRow(eColTujoCsv.システム種別)
            objRow(eColTujoCsv.作業種別) = strCsvRow(eColTujoCsv.作業種別)
            objRow(eColTujoCsv.台数) = strCsvRow(eColTujoCsv.台数)
            objRow(eColTujoCsv.注文番号) = strCsvRow(eColTujoCsv.注文番号)
            objRow(eColTujoCsv.部品１) = ""
            objRow(eColTujoCsv.項目１) = ""
            objRow(eColTujoCsv.単価１) = "0"
            objRow(eColTujoCsv.数量１) = "0"
            objRow(eColTujoCsv.金額１) = "0"
            objRow(eColTujoCsv.合計金額) = "0"
            objRow(eColTujoCsv.添付資料枚数) = strCsvRow(eColTujoCsv.添付資料枚数)
            objRow(eColTujoCsv.空行フラグ) = "1"
            dt.Rows.Add(objRow)
        Next
    End Sub

    ''' <summary>
    ''' 空行セット
    ''' </summary>
    ''' <param name="strCsvRow"></param>
    ''' <param name="intAddRowCnt"></param>
    ''' <param name="dt"></param>
    ''' <remarks></remarks>
    Private Sub msSetHokokuEmptyRow(ByVal strCsvRow() As String, ByVal intAddRowCnt As Integer, ByRef dt As DataTable)
        'カウント数分から行を追加する
        For i As Integer = 0 To intAddRowCnt - 1
            Dim objRow As DataRow = dt.NewRow()
            objRow(eColHokokuKensyuCsv.帳票出力日) = strCsvRow(eColHokokuKensyuCsv.帳票出力日)
            objRow(eColHokokuKensyuCsv.宛先会社名) = strCsvRow(eColHokokuKensyuCsv.宛先会社名)
            objRow(eColHokokuKensyuCsv.宛先部署名) = strCsvRow(eColHokokuKensyuCsv.宛先部署名)
            objRow(eColHokokuKensyuCsv.発行元会社名) = strCsvRow(eColHokokuKensyuCsv.発行元会社名)
            objRow(eColHokokuKensyuCsv.発行元住所) = strCsvRow(eColHokokuKensyuCsv.発行元住所)
            objRow(eColHokokuKensyuCsv.検収年月) = strCsvRow(eColHokokuKensyuCsv.検収年月)
            objRow(eColHokokuKensyuCsv.項目) = strCsvRow(eColHokokuKensyuCsv.項目)
            objRow(eColHokokuKensyuCsv.注文番号) = strCsvRow(eColHokokuKensyuCsv.注文番号)
            objRow(eColHokokuKensyuCsv.金額) = "0"
            objRow(eColHokokuKensyuCsv.備考) = ""
            objRow(eColHokokuKensyuCsv.小計) = "0"
            objRow(eColHokokuKensyuCsv.出精値引き) = "0"
            objRow(eColHokokuKensyuCsv.課税対象料金金額) = "0"
            objRow(eColHokokuKensyuCsv.消費税相当額) = strCsvRow(eColHokokuKensyuCsv.消費税相当額)
            objRow(eColHokokuKensyuCsv.合計金額) = "0"
            objRow(eColHokokuKensyuCsv.確認後宛先会社名) = strCsvRow(eColHokokuKensyuCsv.確認後宛先会社名)
            objRow(eColHokokuKensyuCsv.確認後宛先部署名) = strCsvRow(eColHokokuKensyuCsv.確認後宛先部署名)
            objRow(eColHokokuKensyuCsv.確認後宛先担当者) = strCsvRow(eColHokokuKensyuCsv.確認後宛先担当者)
            objRow(eColHokokuKensyuCsv.確認後宛先TEL) = strCsvRow(eColHokokuKensyuCsv.確認後宛先TEL)
            objRow(eColHokokuKensyuCsv.確認後宛先FAX) = strCsvRow(eColHokokuKensyuCsv.確認後宛先FAX)
            objRow(eColHokokuKensyuCsv.検収者会社名) = strCsvRow(eColHokokuKensyuCsv.検収者会社名)
            objRow(eColHokokuKensyuCsv.検収者部署名) = strCsvRow(eColHokokuKensyuCsv.検収者部署名)
            objRow(eColHokokuKensyuCsv.検収者担当者) = strCsvRow(eColHokokuKensyuCsv.検収者担当者)
            objRow(eColHokokuKensyuCsv.値引率) = strCsvRow(eColHokokuKensyuCsv.値引率)
            objRow(eColHokokuKensyuCsv.空行フラグ) = "1"

            dt.Rows.Add(objRow)
        Next
    End Sub

    ''' <summary>
    ''' ＣＳＶ作成処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfMakeCsv(ByVal ipstrFileclassCD As String,
                               ByVal ipstrReportName As String,
                               ByVal ipstrOrderNo As String,
                               ByVal ds As DataSet) As Boolean

        Dim strServerAddress As String = String.Empty 'サーバアドレス
        Dim strFolderNM As String = String.Empty 'フォルダ名
        Dim strWorkPath As String = String.Empty '出力パス
        Dim strFileName As String = String.Empty
        Dim strlocalpath As String = "C:\UPLOAD\"
        Dim localFileName As String = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString
        Dim opblnResult As Boolean = False
        Dim intCnt As Integer = 1

        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "サーバアドレス取得", "Catch")
        'サーバアドレス取得.
        If pfGetPreservePlace(ipstrFileclassCD, strServerAddress, strFolderNM) <> 0 Then
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
            Return False
        End If
        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "ipstrFileclassCD:" & ipstrFileclassCD, "Catch")
        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "strServerAddress:" & strServerAddress, "Catch")
        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "strFolderNM:" & strFolderNM, "Catch")

        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "出力パス生成", "Catch")
        '出力パス生成.
        strWorkPath = strlocalpath

        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "CSV出力", "Catch")
        'CSV出力.
        If ipstrReportName = "整備完了品一覧表" Then
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "整備完了品一覧表", "Catch")
            Do While 1
                If ds.Tables(0).Rows(1).Item("納入日").ToString = "" Then
                    strFileName = ipstrReportName & "_" & Me.txtNendo.ppText.Replace("/", "") & "_" & "[" & ipstrOrderNo & "]_納入日なし_" & intCnt.ToString("00") & ".csv"
                Else
                    strFileName = ipstrReportName & "_" & Me.txtNendo.ppText.Replace("/", "") & "_" & "[" & ipstrOrderNo & "]_" & Date.Parse(ds.Tables(0).Rows(1).Item("納入日").ToString).ToString("yyyyMMdd") & "_" & intCnt.ToString("00") & ".csv"
                End If

                If System.IO.File.Exists(strWorkPath & "\" & localFileName & ".csv") = False Then
                    Exit Do
                End If
                intCnt += 1
            Loop
        Else
            If ipstrReportName = "TBOX通常整備料金" Then
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "TBOX通常整備料金", "Catch")
                Do While 1
                    strFileName = ipstrReportName & "_" & "[" & ipstrOrderNo & "]_" & intCnt.ToString("00") & ".csv"

                    If System.IO.File.Exists(strWorkPath & "\" & localFileName & ".csv") = False Then
                        Exit Do
                    End If
                    intCnt += 1
                Loop
            Else
                strFileName = ipstrReportName & "_" & "[" & ipstrOrderNo & "]" & ".csv"
            End If
        End If

        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "pfCreateCsvFile", "Catch")
        If pfCreateCsvFile(strWorkPath, localFileName & ".csv", ds, False) <> 0 Then
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
            Return False
        End If


        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "pfFile_Upload", "Catch")
        If pfFile_Upload(strFolderNM, strFileName, localFileName) = False Then
            Return False
        End If

        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "END", "Catch")
        Return True

    End Function

    ''' <summary>
    ''' CSVダウンロード.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfCsvDownload(ByVal ipstrFileclassCD As String,
                                   ByVal ipstrReportName As String,
                                   ByVal ipstrOrderNo As String,
                                   Optional ipstrRecDt As String = "") As Boolean

        Dim strServerAddress As String = String.Empty    'サーバアドレス
        Dim strFolderNM As String = String.Empty         'フォルダ名
        Dim strFilePath As String = String.Empty         'ファイルパス
        Dim strDownloadFileName As String = String.Empty '出力パス
        Dim strData As String = String.Empty
        Dim strFileName As String = String.Empty
        Dim utf As Encoding = Encoding.UTF8
        Dim sjis As Encoding = Encoding.GetEncoding("utf-8")
        Dim strLocalPath As String
        Dim intCnt As Integer
        Dim opblnResult As Boolean = False
        Dim DBFTP As New DBFTP.ClsDBFTP_Main

        mfCsvDownload = False

        'サーバアドレス取得.
        If pfGetPreservePlace(ipstrFileclassCD, strServerAddress, strFolderNM) <> 0 Then
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
            Return False
        End If

        'パス生成.
        'strFilePath = "\\" & strServerAddress & "\" & strFolderNM & "\" & ipstrReportName & "_[" & ipstrOrderNo & "]" & ".csv"
        'strData = ipstrReportName & "_[" & ipstrOrderNo & "]" & ".csv"


        intCnt = 1
        If ipstrReportName = "整備完了品一覧表" Then
            Do While 1
                If ipstrRecDt = "" Then
                    strFileName = ipstrReportName & "_" & Me.txtNendo.ppText.Replace("/", "") & "_" & "[" & ipstrOrderNo & "]_納入日なし_" & intCnt.ToString("00") & ".csv"
                Else
                    strFileName = ipstrReportName & "_" & Me.txtNendo.ppText.Replace("/", "") & "_" & "[" & ipstrOrderNo & "]_" & Date.Parse(ipstrRecDt).ToString("yyyyMMdd") & "_" & intCnt.ToString("00") & ".csv"
                End If

                If DBFTP.pfFtpFile_Exists(strFolderNM & "\", strFileName, opblnResult) = False Then
                    Exit Do
                End If
                strData = strFileName
                'strFilePath = "\\" & strServerAddress & "\" & strFolderNM & "\" & strData
                intCnt += 1
            Loop
        Else
            If ipstrReportName = "TBOX通常整備料金" Then
                Do While 1
                    strFileName = ipstrReportName & "_" & "[" & ipstrOrderNo & "]_" & intCnt.ToString("00") & ".csv"

                    If DBFTP.pfFtpFile_Exists(strFolderNM & "\", strFileName, opblnResult) = False Then
                        Exit Do
                    End If
                    strData = strFileName
                    'strFilePath = "\\" & strServerAddress & "\" & strFolderNM & "\" & strData
                    intCnt += 1
                Loop
            Else
                'strFilePath = "\\" & strServerAddress & "\" & strFolderNM & "\" & ipstrReportName & "_" & "[" & ipstrOrderNo & "]" & ".csv"
                strData = ipstrReportName & "_[" & ipstrOrderNo & "]" & ".csv"
            End If
        End If


        strLocalPath = pfFile_Download(strFolderNM, strData)
        If strLocalPath = "" Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
            Return True
        End If


        Response.Redirect("~/Common/COMLSTP099/COMLSTP099.ashx?path=" & strLocalPath & "&filename=" & HttpUtility.UrlEncode(strData), False)


        'Dim uBytes = Encoding.UTF8.GetBytes(strData)
        'strDownloadFileName = HttpUtility.UrlEncode(strData)
        ''--------------------------------
        ''2014/04/16 星野　ここから
        ''--------------------------------
        ''■□■□結合試験時のみ使用予定□■□■
        'Dim objStack As New StackFrame
        'Dim strPrm As String = ""
        'strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
        'Dim tmp As Object() = Session(P_KEY)
        'If Not tmp Is Nothing Then
        '    For zz = 0 To tmp.Length - 1
        '        If zz <> tmp.Length - 1 Then
        '            strPrm &= tmp(zz).ToString & ","
        '        Else
        '            strPrm &= tmp(zz).ToString
        '        End If
        '    Next
        'End If

        'psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
        '                objStack.GetMethod.Name, "~/Common/COMLSTP099/COMLSTP099.ashx?path=" & strFilePath & "&filename=" & strDownloadFileName, strPrm, "TRANS")
        ''■□■□結合試験時のみ使用予定□■□■
        ''--------------------------------
        ''2014/04/16 星野　ここまで
        ''--------------------------------
        'Response.Redirect("~/Common/COMLSTP099/COMLSTP099.ashx?path=" & strFilePath & "&filename=" & strDownloadFileName, False)

        Return True

    End Function
    ''' <summary>
    ''' 請求書締めチェック処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfCheckClose(ByVal sender As System.Object) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet

        objStack = New StackFrame

        mfCheckClose = False

        Try

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Exit Function
            Else
                '入力チェック
                If Me.txtNendo.ppText = "" Or Me.tftOrder_No.ppFromText <> "" Or Me.tftOrder_No.ppToText <> "" Or Me.dftDeliv_D.ppFromText <> "" _
                    Or Me.dftDeliv_D.ppToText <> "" Or Me.tftMente_No.ppFromText <> "" Or Me.tftMente_No.ppToText <> "" Then
                    psMesBox(Me, "10005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "締め、または当月集計", "\n年月度のみ入力")
                    Exit Function
                End If

                'トランザクション.
                cmdDB = New SqlCommand(M_DISP_ID & "_S8", conDB)
                cmdDB.Parameters.Add(pfSet_Param("degree", SqlDbType.NVarChar, Me.txtNendo.ppText.Replace("/", "")))
                cmdDB.Parameters.Add(pfSet_Param("search_flg", SqlDbType.NVarChar, "0"))

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    mfCheckClose = True
                    Exit Function
                End If

                Select Case dstOrders.Tables(0).Rows(0).Item("検収締め区分") & dstOrders.Tables(0).Rows(0).Item("請求締め区分")
                    Case "00"
                        Select Case sender.ID
                            Case "btnRigth10"        '締め／解除ボタン押下
                                If Master.Master.ppRigthButton2.Text = "締め解除" Then
                                    psMesBox(Me, "30011", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "検収書の締め解除")
                                    Exit Function
                                End If
                            Case "btnRigth9"        '当月集計処理

                        End Select

                    Case "10"
                        Select Case sender.ID
                            Case "btnRigth10"        '締め／解除ボタン押下
                                If Master.Master.ppRigthButton2.Text = "締め" Then
                                    psMesBox(Me, "30011", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "検収書の締め処理")
                                    Exit Function
                                End If
                            Case "btnRigth9"        '当月集計処理
                                psMesBox(Me, "30005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "検収書の締め処理", "当月集計")
                                Exit Function
                        End Select

                    Case "11"
                        Select Case sender.ID
                            Case "btnRigth10"        '締め／解除ボタン押下
                                If Master.Master.ppRigthButton2.Text = "締め解除" Then
                                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "請求書の締め処理", "締め解除")
                                    Exit Function
                                End If
                                If Master.Master.ppRigthButton2.Text = "締め" Then
                                    psMesBox(Me, "30011", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "請求書の締め処理")
                                    Exit Function
                                End If
                            Case "btnRigth9"        '当月集計処理

                        End Select

                End Select

            End If

            mfCheckClose = True

        Catch ex As Exception
            'psMesBox(Me, "30015", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, Me.btnClose.Text)
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.Master.ppRigthButton2.Text)

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

        End Try

    End Function

    ''' <summary>
    ''' オブジェクト取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDBNull(ByVal strVal As String) As Object
        If strVal.Trim() = "" Then
            Return DBNull.Value
        End If
        Return strVal
    End Function

    ''' <summary>
    ''' 日付変換処理.
    ''' </summary>
    ''' <param name="strReport_D">日付</param>
    ''' <param name="intMode">1:検収年月の変換　0:それ以外</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfChange_Jp(ByVal strReport_D As String, ByVal intMode As Integer) As String

        Dim strYear As String = String.Empty
        Dim strMouth As String = String.Empty
        Dim strDay As String = String.Empty
        Dim dtTarget As DateTime = Nothing
        Dim culture As CultureInfo = New CultureInfo("ja-JP", True)
        culture.DateTimeFormat.Calendar = New JapaneseCalendar()

        objStack = New StackFrame

        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "変数定義", "Catch")
        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "strReport_D：" & strReport_D, "Catch")
        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "intMode:" & intMode.ToString, "Catch")

        Try

            Dim dtVal As DateTime = Nothing
            If DateTime.TryParse(strReport_D, dtVal) Then
                If intMode = 0 Then
                    '年・月・日に分割.
                    'Return dtVal.ToString("ggyy年M月d日", culture)
                    Return dtVal.ToString("yyyy年M月d日")
                Else
                    '年・月・日に分割.
                    'Return dtVal.ToString("ggyy年M月", culture)
                    Return dtVal.ToString("yyyy年M月")
                End If
            Else
                Return ""
            End If

        Catch ex As Exception
            ''psMesBox(Me, "30015", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, Me.btnCount.Text)
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "日付の変換")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Return ""
        End Try

    End Function

    'CNSOUTP001-002
    ''' <summary>
    ''' 自社名取得 
    ''' </summary>
    ''' <param name="strKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetTraderInfo(ByVal strKey As String) As DataRow
        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            mfGetTraderInfo = Nothing
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL059", objCn)
                objCmd.Parameters.Add(pfSet_Param("PRC_DVS", SqlDbType.NVarChar, strKey))
                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                If objDs.Tables(0).Rows.Count > 0 Then
                    mfGetTraderInfo = objDs.Tables(0).Rows(0)
                Else
                    mfGetTraderInfo = Nothing
                End If

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "自社名の取得")
                mfGetTraderInfo = Nothing
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Function
    'CNSOUTP001-002 END

#End Region

End Class