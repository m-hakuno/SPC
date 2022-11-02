'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　情報機器保守検収書
'*　ＰＧＭＩＤ：　CMPINQP002
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.27　：　酒井
'*  更　新　　：　2014.06.23　：　稲葉　Object型に対して"="で比較していたのを修正
'*  更　新　　：　2014.06.25　：　間瀬　締め処理等の制御を修正しました。
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'CMPINQP002-001     2015/08/06      栗原　　　帳票フッター用の項目をCSVに追加　CMPINQP002_S7
'CMPINQP002-002     2016/01/25      栗原　　　検収書印刷時、業者情報を倉庫マスタから取得する変更を実施。
'CMPINQP002-003     2016/03/03      栗原　　　保守料金明細一覧表作成時、システム分類別のcsv、帳票を作成するよう変更。
'                                           　特別保守料金csvファイルの項目に「作業者」を追加　CMPINQP002_S14

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================

Imports System
Imports System.Globalization
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataLink
'Imports SPC.Global_asax
Imports System.IO
Imports System.String
Imports SPC.ClsCMExclusive

#End Region

Public Class CMPINQP002

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

    '--------------------------------
    '2014/04/28 Hamamoto ここから
    '--------------------------------
    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ME_DISP_ID As String = "CMPINQP002"
    'CMPINQP002-003
    Private Const KENSYU_YM As String = "KensyuYM" 'ViewStateでの年月保管用
    'システム分類コード
    Private Const SYSCLS_ID As Integer = 1
    Private Const SYSCLS_IC As Integer = 3
    Private Const SYSCLS_LT As Integer = 5
    'CMPINQP002-003 END
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
    Dim DBFTP As New DBFTP.ClsDBFTP_Main
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

#Region "ページ初期処理"
    Dim CMPINQP002_DISP As DataSet

    ''' <summary>
    ''' ページ初期処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        '表設定
        pfSet_GridView(grvList, ME_DISP_ID)

        '件数を非表設定
        '-----------------------------
        '2014/04/22 Hamamoto ここから
        '-----------------------------
        Master.ppCount = "0"
        '-----------------------------
        '2014/04/22 Hamamoto ここまで
        '-----------------------------
    End Sub

#End Region

#Region "Page_Load"

    ''' <summary>
    ''' Page_Load処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try

            If Not IsPostBack Then  '初回表示

                '-------------------------------
                '2014/04/22 Hamamoto ここから
                '-------------------------------
                '「締め／解除」「当月集計」のボタン表示設定
                Master.Master.ppRigthButton1.Visible = True '当月集計ボタン
                Master.Master.ppRigthButton2.Visible = True '締め/締め解除ボタン
                Master.Master.ppRigthButton1.Enabled = False '当月集計ボタン
                Master.Master.ppRigthButton2.Enabled = False '締め/締め解除ボタン

                '「検索」「検索条件クリア」「締め／解除」「当月集計」のボタン活性
                Master.Master.ppRigthButton1.Text = "当月集計"
                Master.Master.ppRigthButton2.Text = "締め"

                'Master.ppRigthButton3.Visible = True    '締め／解除
                'Master.ppRigthButton3.Enabled = False    '締め／解除
                'Master.ppRigthButton4.Visible = True   '当月集計
                'Master.ppRigthButton4.Enabled = False   '当月集計

                ''「締め／解除」「当月集計」のボタン表示設定
                'Master.ppRigthButton3.Text = sCnsShimeButon '締め／解除
                'Master.ppRigthButton4.Text = sCnsSumButon   '当月集計
                '-------------------------------
                '2014/04/22 Hamamoto ここまで
                '-------------------------------

                'プログラムＩＤ、画面名設定
                Master.Master.ppProgramID = ME_DISP_ID
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(ME_DISP_ID)

                'パンくずリスト設定
                Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                'グリッドの初期化
                Me.grvList.DataSource = New DataTable

                '-------------------------------
                '2014/05/05 後藤 ここから
                '-------------------------------
                '年月度初期化
                Me.txtNendo.ppText = String.Empty
                '-------------------------------
                '2014/05/05 後藤 ここまで
                '-------------------------------

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                '変更を反映
                Me.grvList.DataBind()

                ''情報機器保守検収書データの件数チェック
                'If mfChkDataCnt() = False Then
                '    psMesBox(Me, "30015", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画前, "情報機器保守検収書データが存在しません。画面初期化")

                '    '読み込み失敗は画面を閉じる
                '    psClose_Window(Me)

                '    Return
                'End If

                'ボタン活性制御.
                Call msSetEnable()
                SetFocus(Me.txtNendo.ppDateBox)
            End If

            'ボタンアクションの設定.
            Call msSet_ButtonAction()

        Catch ex As Exception

            '--------------------------------
            ' 2014/04/28 Hamamoto ここから
            '--------------------------------
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "読み込み")
            '--------------------------------
            ' 2014/04/28 Hamamoto ここまで
            '--------------------------------

            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------

        End Try

    End Sub

#End Region

#Region "ユーザー権限"
    '---------------------------
    '2014/04/21 武 ここから
    '---------------------------
    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        '------------------------------
        '2014/05/02 後藤 ここから
        '------------------------------
        Select Case Session(P_SESSION_AUTH)
            Case "管理者"

            Case "SPC"
                Master.Master.ppRigthButton1.Enabled = False
                Master.Master.ppRigthButton2.Enabled = False
            Case "営業所"
                Master.Master.ppRigthButton1.Enabled = False
                Master.Master.ppRigthButton2.Enabled = False
            Case "NGC"
                Master.Master.ppRigthButton1.Enabled = False
                Master.Master.ppRigthButton2.Enabled = False
        End Select
        '------------------------------
        '2014/05/02 後藤 ここまで
        '------------------------------

    End Sub
    '---------------------------
    '2014/04/21 武 ここまで
    '---------------------------
#End Region

#Region "ボタン押下処理"

    ''' <summary>
    ''' ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Button_Click(sender As System.Object, e As System.EventArgs)

        Dim strMessage As String = String.Empty
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            Select Case sender.id
                Case "btnSearchRigth2"        '検索クリアボタン押下

                    strMessage = "検索条件クリア処理"

                    '入力項目初期化.
                    Me.txtNendo.ppText = String.Empty

                Case "btnSearchRigth1"        '検索ボタン押下

                    strMessage = "検索処理"

                    If (Page.IsValid) Then
                        '検収月をViewStateに保存
                        ViewState(KENSYU_YM) = txtNendo.ppText
                        '条件検索取得
                        msGet_Data(0)
                    End If

                Case "btnRigth2"        '締め／解除ボタン押下

                    strMessage = "締め処理"

                    '締め処理.
                    Call msClose_Click()

                Case "btnRigth1"        '当月集計ボタン押下

                    strMessage = "当月集計処理"

                    '状態チェック.
                    If mfChkJotai(3) = False Then
                        Exit Sub
                    End If

                    '条件検索取得
                    msGet_Data(1)

            End Select
            SetFocus(Me.txtNendo.ppDateBox)

        Catch ex As Exception

            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMessage)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------

        End Try

    End Sub

#End Region

#Region "一覧の更新／参照／進捗画面ボタン押下処理"
    ''' <summary>
    ''' 一覧の更新／参照／進捗画面ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        '------------------------------
        '2014/05/20 後藤 ここから
        '------------------------------
        If e.CommandName <> "btnPrint" And e.CommandName <> "btnCsv" And e.CommandName <> "btnSyukei" Then
            Exit Sub
        End If

        Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    ' ボタン押下行のIndex
        Dim rowData As GridViewRow = grvList.Rows(intIndex)             ' ボタン押下行
        Dim dtRowData As DataTable = Nothing
        Dim dstOrders As DataSet = Nothing
        dtRowData = pfParse_DataTable(grvList)
        Dim CsvData As New ArrayList
        Dim Cnt As Integer = 0
        Dim strArrayList As String() = Nothing
        Dim CsvData2 As New ArrayList
        Dim Cnt2 As Integer = 0
        Dim strArrayList2 As String() = Nothing
        Dim dt As New DataTable
        Dim rptDOCREP004 As New DOCREP004 '情報機器保守の報告書兼検収書.
        'Dim rptCMPREP001 As New CMPREP001 '保守料金明細.
        Dim rptCMPREP003 As New CMPREP003 '特別保守費用一覧.
        Dim rptCMPREP004 As New CMPREP004 '特別保守作業_集計.
        Dim rptCMPREP002 As New CMPREP002 '緊急運営輸送費.
        Dim rptREPREP003 As New REPREP003 '修理・有償部品費用.
        Dim rptREPREP058 As New REPREP058 '修理作業・運送費用_有償部品代集計.
        Dim strSeikyuYm As String = ""
        Dim strDegreeYmd As String = ""
        Dim CsvData3 As New ArrayList
        Dim Cnt3 As Integer = 0
        Dim strArrayList3 As String() = Nothing

        '------------------------------
        '2014/05/20 後藤 ここまで
        '------------------------------
        'ログ出力開始
        psLogStart(Me)

        'ビューに退避.
        ViewState("dt") = dtRowData
        ViewState("index") = intIndex

        '------------------------------
        '2014/05/01 後藤 ここから
        '------------------------------
        '検収月を取得
        If Not mfGetKensyuYm(strSeikyuYm, strDegreeYmd) Then
            Throw New Exception()
        End If
        '------------------------------
        '2014/05/01 後藤 ここまで
        '------------------------------

        Select Case e.CommandName
            Case "btnPrint"      '印刷.
                Select Case CType(rowData.FindControl("帳票名"), TextBox).Text
                    Case "情報機器保守の報告書兼検収書"

                        '------------------------------
                        '2014/05/01 後藤 ここから
                        '------------------------------
                        'CSVファイル取り込み.
                        'ms_GetCSVData("0781CL", "情報機器保守の報告書兼検収書", CsvData, Cnt, strArrayList)
                        ms_GetCSVData("0781CL", "情報機器保守の報告書兼検収書_[" & strSeikyuYm.Replace("/", "") & "]", CsvData, Cnt, strArrayList)

                        If Not CsvData Is Nothing AndAlso CsvData.Count > 0 Then
                            'PDFファイル作成.
                            ms_MakePdf("情報機器保守の報告書兼検収書", CsvData, Cnt, rptDOCREP004, strArrayList, 0)
                        Else
                            psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        End If

                        '------------------------------
                        '2014/05/01 後藤 ここまで
                        '------------------------------
                    Case "保守料金明細"
                        'CSVファイル取り込み.
                        'CMPINQP002-003
                        'ms_GetCSVData("0788CL"         , "保守料金明細一覧表_都道府県_2"   , CsvData , Cnt , strArrayList )
                        'ms_GetCSVData("0788CL"         , "保守料金明細一覧表_エリア別"     , CsvData2, Cnt2, strArrayList2)
                        'ms_GetCSVData("0788CL"         , "保守料金明細一覧表_エリア別合計" , CsvData3, Cnt3, strArrayList3)
                        'ms_MakePdf   ("保守料金明細"   , CsvData , Cnt   , rptCMPREP001    , strArrayList  , 1            )

                        'システム分類別の変数
                        Dim CsvData_ID As New ArrayList
                        Dim CsvData_IC As New ArrayList
                        Dim CsvData_LT As New ArrayList
                        Dim Cnt_ID As Integer = 0
                        Dim Cnt_IC As Integer = 0
                        Dim Cnt_LT As Integer = 0
                        Dim strArrayList_ID As String() = Nothing
                        Dim strArrayList_IC As String() = Nothing
                        Dim strArrayList_LT As String() = Nothing
                        'システム分類毎の明細一覧表と総合計取得用csvファイルの取り込み
                        '各システム分類別の一覧表と合計値表示用のエリア別併せて4つのcsvファイルを使用する
                        ms_GetCSVData("0789CL", "保守料金明細一覧表_ID_[" & strSeikyuYm.Replace("/", "") & "]", CsvData_ID, Cnt_ID, strArrayList_ID)
                        ms_GetCSVData("0789CL", "保守料金明細一覧表_IC_[" & strSeikyuYm.Replace("/", "") & "]", CsvData_IC, Cnt_IC, strArrayList_IC)
                        ms_GetCSVData("0789CL", "保守料金明細一覧表_LT_[" & strSeikyuYm.Replace("/", "") & "]", CsvData_LT, Cnt_LT, strArrayList_LT)
                        ms_GetCSVData("0788CL", "保守料金明細一覧表_エリア別_[" & strSeikyuYm.Replace("/", "") & "]", CsvData2, Cnt2, strArrayList2)
                        'ms_GetCSVData("0788CL", "保守料金明細一覧表_エリア別合計_[" & strSeikyuYm.Replace("/", "") & "]", CsvData3, Cnt3, strArrayList3)

                        '必要なcsvファイルが全種類揃っているかチェックする
                        If Not CsvData_ID Is Nothing AndAlso CsvData_ID.Count > 0 AndAlso
                           Not CsvData_IC Is Nothing AndAlso CsvData_IC.Count > 0 AndAlso
                           Not CsvData_LT Is Nothing AndAlso CsvData_LT.Count > 0 AndAlso
                           Not CsvData2 Is Nothing AndAlso CsvData2.Count > 0 Then
                            'AndAlso
                            'Not CsvData3 Is Nothing AndAlso CsvData3.Count > 0 Then
                            'PDFファイル作成
                            mfPDF(CsvData_ID, Cnt_ID, strArrayList_ID,
                                  CsvData_IC, Cnt_IC, strArrayList_IC,
                                  CsvData_LT, Cnt_LT, strArrayList_LT,
                                  CsvData2, Cnt2, strArrayList2)
                            'CsvData3, Cnt3, strArrayList3, "保守料金明細作成一覧表")
                            'mfPDF(CsvData , Cnt , strArrayList ,
                            '      CsvData2, Cnt2, strArrayList2,
                            '      CsvData3, Cnt3, strArrayList3, "保守料金明細作成一覧表")
                            'CMPINQP002-003 END
                        Else
                            If Date.Parse(strSeikyuYm & "/01").CompareTo(New DateTime(2014, 11, 1)) = -1 Then
                                psMesBox(Me, "30025", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "2014年10月以前は保守料金明細一覧表のデータ", "印刷")
                            Else

                                '2016/03までは修正前のcsvファイル（システム分類無し）なら存在するはずなので検索
                                If Date.Parse(strSeikyuYm & "/01").CompareTo(New DateTime(2016, 4, 1)) = -1 Then
                                    ms_GetCSVData("0789CL", "保守料金明細一覧表_[" & strSeikyuYm.Replace("/", "") & "]", CsvData, Cnt, strArrayList)
                                    If Not CsvData Is Nothing AndAlso CsvData.Count > 0 AndAlso
                                       Not CsvData2 Is Nothing AndAlso CsvData2.Count > 0 Then
                                        '修正前の（システム分類毎に分割されていない）帳票を出力
                                        mfPDF(CsvData, Cnt, strArrayList,
                                              Nothing, Nothing, Nothing,
                                              Nothing, Nothing, Nothing,
                                              CsvData2, Cnt2, strArrayList2,
                                              False)
                                    Else
                                        psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                                    End If
                                Else
                                    psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                                End If
                            End If
                        End If

                    Case "特別保守費用一覧"

                        '------------------------------
                        '2014/05/01 後藤 ここから
                        '------------------------------
                        'CSVファイル取り込み.
                        'ms_GetCSVData("0783CL", "特別保守費用一覧", CsvData, Cnt, strArrayList)
                        'ms_GetCSVData("0784CL", "特別保守作業集計", CsvData2, Cnt2, strArrayList2)
                        ms_GetCSVData("0783CL", "特別保守費用一覧_[" & strSeikyuYm.Replace("/", "") & "]", CsvData, Cnt, strArrayList)
                        ms_GetCSVData("0784CL", "特別保守作業集計_[" & strSeikyuYm.Replace("/", "") & "]", CsvData2, Cnt2, strArrayList2)

                        If Not CsvData Is Nothing AndAlso CsvData.Count > 0 AndAlso
                             Not CsvData2 Is Nothing AndAlso CsvData2.Count > 0 Then
                            'PDFファイル作成.
                            ms_MakePdf("特別保守費用一覧", "特別保守作業集計", CsvData, CsvData2, rptCMPREP003, rptCMPREP004, strArrayList, strArrayList2, 0)
                        Else
                            psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        End If
                        '------------------------------
                        '2014/05/01 後藤 ここまで
                        '------------------------------
                    Case "緊急運営輸送費"

                        '------------------------------
                        '2014/05/01 後藤 ここから
                        '------------------------------
                        'CSVファイル取り込み.
                        'ms_GetCSVData("0785CL", "緊急運営輸送費", CsvData, Cnt, strArrayList)
                        ms_GetCSVData("0785CL", "緊急運営輸送費_[" & strSeikyuYm.Replace("/", "") & "]", CsvData, Cnt, strArrayList)

                        If Not CsvData Is Nothing AndAlso CsvData.Count > 0 Then
                            'PDFファイル作成.
                            ms_MakePdf("緊急運営輸送費", CsvData, Cnt, rptCMPREP002, strArrayList, 2)
                        Else
                            psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        End If
                        '------------------------------
                        '2014/05/01 後藤 ここまで
                        '------------------------------
                    Case "有償部品費用"

                        '------------------------------
                        '2014/05/01 後藤 ここから
                        '------------------------------
                        'CSVファイル取り込み.
                        'ms_GetCSVData("0786CL", "有償部品費用", CsvData, Cnt, strArrayList)
                        'ms_GetCSVData("0787CL", "修理・作業運送費用有償部品代集計", CsvData2, Cnt2, strArrayList2)
                        '------------------------------
                        '2014/05/08 後藤 ここから
                        '------------------------------
                        'ms_GetCSVData("0786CL", "有償部品費用_[" & strSeikyuYm.Replace("/", "") & "]", CsvData, Cnt, strArrayList)
                        'ms_GetCSVData("0787CL", "修理_作業運送費用有償部品代集計_[" & strSeikyuYm.Replace("/", "") & "]", CsvData2, Cnt2, strArrayList2)
                        ms_GetCSVData("0786CL", "修理有償部品費用_[" & strSeikyuYm.Replace("/", "") & "]", CsvData, Cnt, strArrayList)
                        ms_GetCSVData("0787CL", "修理作業_運送費用有償部品代集計_[" & strSeikyuYm.Replace("/", "") & "]", CsvData2, Cnt2, strArrayList2)
                        '------------------------------
                        '2014/05/08 後藤 ここまで
                        '------------------------------
                        If Not CsvData Is Nothing AndAlso CsvData.Count > 0 Then
                            'PDFファイル作成.
                            '------------------------------
                            '2014/05/08 後藤 ここから
                            '------------------------------
                            'ms_MakePdf("有償部品費用", "修理・作業運送費用有償部品代集計", CsvData, CsvData2, rptREPREP003, rptREPREP058, strArrayList, strArrayList2, 1)
                            ms_MakePdf("修理有償部品費用", "修理作業_運送費用有償部品代集計", CsvData, CsvData2, rptREPREP003, rptREPREP058, strArrayList, strArrayList2, 1)
                            '------------------------------
                            '2014/05/08 後藤 ここまで
                            '------------------------------
                        Else
                            psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        End If
                        '------------------------------
                        '2014/05/01 後藤 ここまで
                        '------------------------------
                End Select


            Case "btnCsv"        'CSV出力.

                Select Case CType(rowData.FindControl("帳票名"), TextBox).Text
                    Case "情報機器保守の報告書兼検収書"

                        '------------------------------
                        '2014/05/01 後藤 ここから
                        '------------------------------
                        'If mfCsvDownload("0781CL", "情報機器保守の報告書兼検収書") = False Then
                        If mfCsvDownload("0781CL", "情報機器保守の報告書兼検収書_[" & strSeikyuYm.Replace("/", "") & "]") = False Then
                            'psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        End If
                        '------------------------------
                        '2014/05/01 後藤 ここまで
                        '------------------------------

                    Case "保守料金明細"

                        '------------------------------
                        '2014/05/01 後藤 ここから
                        '------------------------------
                        'If mfCsvDownload("0782CL", "保守料金明細") = False Then
                        If mfCsvDownload("0782CL", "保守料金明細一覧表_[" & strSeikyuYm.Replace("/", "") & "]") = False Then
                            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                        End If
                        '------------------------------
                        '2014/05/01 後藤 ここまで
                        '------------------------------

                    Case "特別保守費用一覧"
                        '------------------------------
                        '2014/05/01 後藤 ここから
                        '------------------------------
                        '--------------------------------
                        '2014/04/22 Hamamoto　ここから
                        '--------------------------------
                        'If mfCsvDownload("0783CL", "特別保守費用一覧") = False Then
                        If mfCsvDownload("0783CL", "特別保守費用一覧_[" & strSeikyuYm.Replace("/", "") & "]") = False Then
                            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                        End If
                        '--------------------------------
                        '2014/04/22 Hamamoto　ここまで
                        '--------------------------------
                        '------------------------------
                        '2014/05/01 後藤 ここまで
                        '------------------------------

                    Case "緊急運営輸送費"

                        '------------------------------
                        '2014/05/01 後藤 ここから
                        '------------------------------
                        'If mfCsvDownload("0785CL", "緊急運営輸送費") = False Then
                        If mfCsvDownload("0785CL", "緊急運営輸送費_[" & strSeikyuYm.Replace("/", "") & "]") = False Then
                            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                        End If
                        '------------------------------
                        '2014/05/01 後藤 ここまで
                        '------------------------------

                    Case "有償部品費用"
                        '------------------------------
                        '2014/05/01 後藤 ここから
                        '------------------------------
                        '--------------------------------
                        '2014/04/22 Hamamoto　ここから
                        '--------------------------------
                        'If mfCsvDownload("0786CL", "有償部品費用") = False Then

                        '------------------------------
                        '2014/05/08 後藤 ここから
                        '------------------------------
                        'If mfCsvDownload("0786CL", "有償部品費用_[" & strSeikyuYm.Replace("/", "") & "]") = False Then
                        '    psMesBox(Me, "10001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "CSV")
                        'End If
                        If mfCsvDownload("0786CL", "修理有償部品費用_[" & strSeikyuYm.Replace("/", "") & "]") = False Then
                            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                        End If
                        '------------------------------
                        '2014/05/08 後藤 ここまで
                        '------------------------------
                        '--------------------------------
                        '2014/04/22 Hamamoto　ここまで
                        '--------------------------------
                        '------------------------------
                        '2014/05/01 後藤 ここまで
                        '------------------------------

                    Case "LAN単価"
                        If mfCsvDownload("0782CL", "LAN単価_[" & strSeikyuYm.Replace("/", "") & "]") = False Then
                            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                        End If

                End Select
                '--------------------------------
                '2014/04/22 Hamamoto　ここから
                '--------------------------------
            Case "btnSyukei"
                Select Case CType(rowData.FindControl("帳票名"), TextBox).Text
                    Case "特別保守費用一覧"
                        '------------------------------
                        '2014/05/01 後藤 ここから
                        '------------------------------
                        'If mfCsvDownload("0784CL", "特別保守作業集計") = False Then
                        If mfCsvDownload("0784CL", "特別保守作業集計_[" & strSeikyuYm.Replace("/", "") & "]") = False Then
                            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                        End If
                        '------------------------------
                        '2014/05/01 後藤 ここまで
                        '------------------------------

                    Case "有償部品費用"
                        '------------------------------
                        '2014/05/08 後藤 ここから
                        '------------------------------
                        '------------------------------
                        '2014/05/01 後藤 ここから
                        '------------------------------
                        'If mfCsvDownload("0787CL", "修理・作業運送費用有償部品代集計") = False Then
                        'If mfCsvDownload("0787CL", "修理・作業運送費用有償部品代集計_[" & strSeikyuYm.Replace("/", "") & "]") = False Then
                        '    psMesBox(Me, "10001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "CSV")
                        'End If
                        If mfCsvDownload("0787CL", "修理作業_運送費用有償部品代集計_[" & strSeikyuYm.Replace("/", "") & "]") = False Then
                            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                        End If
                        '------------------------------
                        '2014/05/01 後藤 ここまで
                        '------------------------------
                        '------------------------------
                        '2014/05/08 後藤 ここまで
                        '------------------------------
                End Select
                '--------------------------------
                '2014/04/22 Hamamoto　ここまで
                '--------------------------------
        End Select

        '終了ログ出力
        psLogEnd(Me)

    End Sub

#End Region

#End Region

#Region "そのほかのプロシージャ"

#Region "ボタンアクション"

    ''' <summary>
    ''' ボタンアクションの設定.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_ButtonAction()

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf Button_Click   '検索
        AddHandler Master.ppRigthButton2.Click, AddressOf Button_Click   'クリア

        '-----------------------------
        '2014/04/22 Hamamoto ここから
        '-----------------------------
        'AddHandler Master.ppRigthButton3.Click, AddressOf Button_Click
        'AddHandler Master.ppRigthButton4.Click, AddressOf Button_Click
        AddHandler Master.Master.ppRigthButton1.Click, AddressOf Button_Click '当月集計
        AddHandler Master.Master.ppRigthButton2.Click, AddressOf Button_Click '締め／解除
        '-----------------------------
        '2014/04/22 Hamamoto ここから
        '-----------------------------

        Master.ppRigthButton1.Attributes("onClick") = "dispWait('search');"

        '「クリア」ボタン押下時の検証を無効
        Master.ppRigthButton2.CausesValidation = False

        '------------------------------
        '2014/05/01 後藤 ここから
        '------------------------------
        Master.Master.ppRigthButton1.CausesValidation = False
        Master.Master.ppRigthButton2.CausesValidation = False
        '------------------------------
        '2014/05/01 後藤 ここまで
        '------------------------------

        '確認メッセージ設定.
        '-----------------------------
        '2014/04/22 Hamamoto ここから
        '-----------------------------
        Master.Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton1.Text).Replace(";", "") + "&&dispWait('count');" '当月集計確認
        Master.Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton2.Text).Replace(";", "") + "&&dispWait('close');" '締め確認
        'Master.ppRigthButton3.OnClientClick = pfGet_OCClickMes("30006", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, Master.ppRigthButton3.Text) '締め確認
        'Master.ppRigthButton4.OnClientClick = pfGet_OCClickMes("30006", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, Master.ppRigthButton4.Text) '当月集計確認
        '-----------------------------
        '2014/04/22 Hamamoto ここまで
        '-----------------------------

    End Sub
#End Region

#Region "条件検索取得処理"
    '-----------------------------
    '2014/05/16 後藤　ここから
    '-----------------------------
    ''' <summary>
    ''' 情報機器保守検収書データ件数のチェック
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
                cmdDB = New SqlCommand("CMPINQP002_S9", conDB)
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
    ''' 条件検索取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data(ByVal intShori As Integer)

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet
        Dim dstOrders2 As New DataSet
        Dim dstOrders3 As New DataSet
        Dim strCNST_CLS_Chk As StringBuilder = New StringBuilder
        Dim intIns As Integer = 0 '検収締め区分
        Dim intReq As Integer = 0 '請求締め区分
        Dim decDegree As Decimal = Nothing     '年月度
        Dim decCnst_Cnt As Decimal = Nothing   '検収書件数 
        Dim decCnst_Price As Decimal = Nothing '検収書単価 
        Dim decMnt_Cnt As Decimal = Nothing    '保守料金件数
        Dim decMnt_Price As Decimal = Nothing  '保守料金単価
        Dim decPre_Cnt As Decimal = Nothing    '特別保守件数
        Dim decPre_Price As Decimal = Nothing  '特別保守単価
        Dim decSpc_Cnt As Decimal = Nothing    '運送費件数 
        Dim decSpc_Price As Decimal = Nothing  '運送費単価 
        Dim decOth_Cnt As Decimal = Nothing    '修理有償件数
        Dim decOth_Price As Decimal = Nothing  '修理有償単価
        Dim decLan_Cnt As Decimal = Nothing    'LAN単価件数
        Dim decLan_Price As Decimal = Nothing  'LAN単価
        Dim dec As Decimal = Nothing '変換用
        Dim intCol As Integer = 0 'カラム数
        Dim intRow As Integer = 0 '列数
        Dim intFsCnt As Integer = 0
        Dim intFsMoney As Decimal = Nothing
        Dim intCsCnt As Integer = 0
        Dim intCsMoney As Decimal = Nothing
        Dim intOthCnt As Integer = 0
        Dim intOthMoney As Decimal = Nothing
        Dim strSeikyuYm As String = ""
        Dim strDegreeYmd As String = ""
        Dim strData As String() = Nothing
        Dim intRetCnt As Integer = 0

        objStack = New StackFrame

        Try

            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Throw New Exception("")
            End If

            If intShori = 0 Then    '検索ボタン押下

                '画面ページ表示初期化
                Master.ppCount = ""
                Me.grvList.DataSource = New DataTable

                'パラメータ設定
                cmdDB = New SqlCommand("CMPINQP002_S1", conDB)

                With cmdDB.Parameters
                    .Add(pfSet_Param("Tekiyou_YM", SqlDbType.NVarChar, txtNendo.ppText.Substring(0, 4) & txtNendo.ppText.Substring(5, 2)))          '適用日
                End With

                'データ取得およびデータをリストに設定
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                Me.grvList.DataSource = dstOrders

                '件数を設定
                Master.ppCount = dstOrders.Tables(0).Rows.Count

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                '変更を反映
                Me.grvList.DataBind()

                For i As Integer = 0 To Me.grvList.Rows.Count - 1
                    Dim rowData As GridViewRow = grvList.Rows(i)
                    intIns = CType(rowData.FindControl("検収締め"), TextBox).Text
                    intReq = CType(rowData.FindControl("請求締め"), TextBox).Text

                    '締まっている場合.
                    'If CType(rowData.FindControl("検収締め"), TextBox).Text = "1" Then
                    '    '印刷/ＣＳＶ非活性.
                    '    rowData.Cells.Item(0).Enabled = False
                    '    rowData.Cells.Item(1).Enabled = False
                    'Else '締まっていない場合.
                    '    '印刷/ＣＳＶ活性.
                    '    rowData.Cells.Item(0).Enabled = True
                    '    rowData.Cells.Item(1).Enabled = True
                    'End If

                    '特別保守費用一覧、有償部品費用の行には一覧ボタンと集計ボタンを設定
                    If CType(rowData.Cells(3).Controls(0), TextBox).Text = "特別保守費用一覧" OrElse CType(rowData.Cells(3).Controls(0), TextBox).Text = "有償部品費用" Then
                        CType(rowData.Cells(1).Controls(0), Button).Text = " 一覧 "
                        CType(rowData.Cells(2).Controls(0), Button).Text = "集計"

                        '締めているか？
                        'If CType(rowData.FindControl("検収締め"), TextBox).Text = "1" Then
                        '    rowData.Cells.Item(2).Enabled = False
                        'Else
                        '    rowData.Cells.Item(2).Enabled = True
                        'End If
                    Else
                        '使わないボタンは見出しを空白とし、非活性
                        CType(rowData.Cells(2).Controls(0), Button).Text = "　　"
                        rowData.Cells.Item(2).Enabled = False
                        If CType(rowData.Cells(3).Controls(0), TextBox).Text = "LAN単価" Then
                            CType(rowData.Cells(0).Controls(0), Button).Text = "　　"
                            rowData.Cells.Item(0).Enabled = False
                        End If
                    End If
                Next

                'データがない場合メッセージ.
                If dstOrders.Tables(0).Rows.Count = 0 Then
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                End If

                '再検索
                Call msSeach_Data(txtNendo.ppText)

                Call msSetEnable()

            Else        '当月集計
                Dim strFileclassCD As String = String.Empty
                Dim strReportName As String = String.Empty

                '検収月を取得
                If Not mfGetKensyuYm(strSeikyuYm, strDegreeYmd) Then
                    Throw New Exception()
                End If

                '情報機器保守の報告書兼検収書
                cmdDB = New SqlCommand("CMPINQP002_S7", conDB)

                cmdDB.Parameters.Add(pfSet_Param("prm_date", SqlDbType.NVarChar, strSeikyuYm))
                If txtNendo.ppText = DateTime.Now.AddMonths(1).ToString("yyyy/MM") Then
                    cmdDB.Parameters.Add(pfSet_Param("prm_degree", SqlDbType.NVarChar, DateTime.Now.ToString("yyyy/MM/dd")))
                Else
                    cmdDB.Parameters.Add(pfSet_Param("prm_degree", SqlDbType.NVarChar, Date.Parse(strSeikyuYm & "/01").AddDays(-1).ToString("yyyy/MM/dd")))
                End If
                cmdDB.Parameters.Add(pfSet_Param("outputflg", SqlDbType.NVarChar, "1"))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '検収書料金取得
                dec = Nothing

                If dstOrders.Tables(0).Rows.Count > 0 Then
                    If Decimal.TryParse(dstOrders.Tables(0).Rows(0).Item("保守料金件数合計").ToString, dec) Then
                        decCnst_Cnt += dec
                    End If
                    If Decimal.TryParse(dstOrders.Tables(0).Rows(0).Item("保守料金料金合計").ToString, dec) Then
                        decCnst_Price += dec
                    End If
                End If

                '保守料金明細_都道府県
                cmdDB = New SqlCommand("CMPINQP002_S15", conDB)
                With cmdDB.Parameters
                    .Add(pfSet_Param("AggDate", SqlDbType.NVarChar, Date.Parse(strSeikyuYm & "/01").AddDays(-1).ToString("yyyy/MM/dd")))
                End With

                dstOrders2 = clsDataConnect.pfGet_DataSet(cmdDB)

                'strFileclassCD = "0788CL"
                'strReportName = "保守料金明細一覧表_都道府県"
                'If mfMakeCsv(strFileclassCD, strReportName, dstOrders2) = False Then
                '    Exit Sub
                'End If

                'データ件数表示
                Master.ppCount = dstOrders2.Tables(0).Rows.Count

                '****************************/
                '*  エリア・TBOXタイプ別    */
                '****************************/
                Dim decSuu As Decimal
                Dim decAmt As Decimal
                Dim decSpcSuu As Decimal
                Dim decSpcAmt As Decimal
                Dim decSpcAmt2 As Decimal
                Dim dsTBOXLIST As DataSet
                Dim dtCSVBase As New DataTable
                Dim dtData2 As New DataTable
                Dim objWKDR As DataRow
                Dim dsTBOX_PreFuct() As DataSet
                Dim dsTBOX_PreFuct2() As DataSet
                Dim objDs0 As New DataSet          'データセット（都道府県別CSV用）
                Dim dsCSVBase As New DataSet          'データセット（都道府県別）
                Dim objDs2 As New DataSet          'データセット（エリア別）
                Dim objDs3 As New DataSet          'データセット（エリア別合計）
                'CMPINQP002-003
                Dim objDsID As New DataSet          'データセット（ID）
                Dim objDsIC As New DataSet          'データセット（IC）
                Dim objDsLT As New DataSet          'データセット（LUTE）
                'CMPINQP002-003 END

                '保守料金明細_エリア別.
                cmdDB = New SqlCommand("CMPINQP002_S16", conDB)
                cmdDB.Parameters.Add(pfSet_Param("AggDate", SqlDbType.NVarChar, Date.Parse(strSeikyuYm & "/01").AddDays(-1).ToString("yyyy/MM/dd")))
                dstOrders3 = clsDataConnect.pfGet_DataSet(cmdDB)
                'strFileclassCD = "0788CL"
                'strReportName = "保守料金明細一覧表_エリア別"
                'If mfMakeCsv(strFileclassCD, strReportName, dstOrders3) = False Then
                '    Exit Sub
                'End If

                '保守料金明細_エリア別合計.
                cmdDB = New SqlCommand("CMPINQP002_S17", conDB)
                cmdDB.Parameters.Add(pfSet_Param("AggDate", SqlDbType.NVarChar, Date.Parse(strSeikyuYm & "/01").AddDays(-1).ToString("yyyy/MM/dd")))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                strFileclassCD = "0788CL"
                strReportName = "保守料金明細一覧表_エリア別合計"
                If mfMakeCsv(strFileclassCD, strReportName, dstOrders) = False Then
                    Exit Sub
                End If

                'エリア別のエリアコード列を削除
                dstOrders3.Tables(0).Columns.RemoveAt(0)

                'データテーブルのマージ（都道府県別とエリア別をマージ）
                dstOrders3.Tables(0).Columns(3).ColumnName = "都道府県名"

                'dsCSVBase.Tables(0).Merge(dstOrders3.Tables(0))

                'エリア別合計件数追加.
                intCol = dstOrders2.Tables(0).Columns.Count
                For i As Integer = 1 To dstOrders2.Tables(0).Columns.Count

                    If intCol = i Then
                        dstOrders2.Tables(0).Columns.Add("FSエリア合計件数", Type.GetType("System.String"))
                        dstOrders2.Tables(0).Columns.Add("CSエリア合計件数", Type.GetType("System.String"))
                        dstOrders2.Tables(0).Columns.Add("その他エリア合計件数", Type.GetType("System.String"))
                    End If
                Next

                '件数・金額取得
                For i As Integer = 0 To dstOrders.Tables(0).Rows.Count - 1
                    Select Case dstOrders.Tables(0).Rows(i).Item("エリアコード").ToString
                        Case "0"
                            intFsCnt = dstOrders.Tables(0).Rows(i).Item("エリア件数")
                            intFsMoney = dstOrders.Tables(0).Rows(i).Item("エリア金額")
                            'decMnt_Cnt += dstOrders.Tables(0).Rows(i).Item("エリア件数")
                            decMnt_Price += dstOrders.Tables(0).Rows(i).Item("エリア金額")
                        Case "1"
                            intCsCnt = dstOrders.Tables(0).Rows(i).Item("エリア件数")
                            intCsMoney = dstOrders.Tables(0).Rows(i).Item("エリア金額")
                            'decMnt_Cnt += dstOrders.Tables(0).Rows(i).Item("エリア件数")
                            decMnt_Price += dstOrders.Tables(0).Rows(i).Item("エリア金額")
                        Case "9"
                            intOthCnt = dstOrders.Tables(0).Rows(i).Item("エリア件数")
                            intOthMoney = dstOrders.Tables(0).Rows(i).Item("エリア金額")
                            'decMnt_Cnt += dstOrders.Tables(0).Rows(i).Item("エリア件数")
                            decMnt_Price += dstOrders.Tables(0).Rows(i).Item("エリア金額")
                    End Select
                Next

                'エリア別件数セット.
                For i As Integer = 0 To dstOrders2.Tables(0).Rows.Count - 1
                    If dstOrders.Tables(0).Rows.Count = 0 Then
                        dstOrders2.Tables(0).Rows(i).Item("FSエリア合計件数") = 0
                        dstOrders2.Tables(0).Rows(i).Item("CSエリア合計件数") = 0
                        dstOrders2.Tables(0).Rows(i).Item("その他エリア合計件数") = 0
                    Else
                        dstOrders2.Tables(0).Rows(i).Item("FSエリア合計件数") = intFsCnt
                        dstOrders2.Tables(0).Rows(i).Item("CSエリア合計件数") = intCsCnt
                        dstOrders2.Tables(0).Rows(i).Item("その他エリア合計件数") = intOthCnt
                    End If
                Next

                '都道府県一覧　取得
                Dim dsState As New DataSet
                cmdDB = New SqlCommand("CMPINQP002_S27", conDB)
                dsState = clsDataConnect.pfGet_DataSet(cmdDB)

                'ＴＢＯＸ種別一覧　取得
                cmdDB = New SqlCommand("CMPINQP002_S26", conDB)
                dsTBOXLIST = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ保管用　雛形作成
                dtCSVBase.Columns.Add("帳票出力日")
                dtCSVBase.Columns.Add("発行元名")
                dtCSVBase.Columns.Add("集計日")
                dtCSVBase.Columns.Add("都道府県名")
                For intIndex = 0 To dsTBOXLIST.Tables(0).Rows.Count - 1
                    dtCSVBase.Columns.Add(dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_NM").ToString() + "_件数")
                    dtCSVBase.Columns.Add(dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_NM").ToString() + "_金額")
                Next

                'ＴＢＯＸ種別毎にデータセットの配列作成　？メモリ食いすぎじゃね？
                ReDim dsTBOX_PreFuct(dsTBOXLIST.Tables(0).Rows.Count - 1)
                ReDim dsTBOX_PreFuct2(dsTBOXLIST.Tables(0).Rows.Count - 1)

                'ＴＢＯＸ種別毎　都道府県別件数　取得
                For intIndex = 0 To dsTBOXLIST.Tables(0).Rows.Count - 1
                    With dsTBOXLIST.Tables(0).Rows(intIndex)
                        cmdDB = New SqlCommand("CMPINQP002_S21", conDB)
                        With cmdDB.Parameters  '--パラメータ設定
                            If txtNendo.ppText = DateTime.Now.AddMonths(1).ToString("yyyy/MM") Then
                                .Add(pfSet_Param("AggDate", SqlDbType.NVarChar, DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "")))
                            Else
                                .Add(pfSet_Param("AggDate", SqlDbType.NVarChar, Date.Parse(strSeikyuYm & "/01").AddDays(-1).ToString("yyyy/MM/dd").Replace("/", "")))
                            End If
                            .Add(pfSet_Param("syscd", SqlDbType.NVarChar, dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_CD").ToString))
                        End With
                        dsTBOX_PreFuct(intIndex) = clsDataConnect.pfGet_DataSet(cmdDB)
                    End With
                Next

                '都道府県でループ
                Dim zz As Integer
                Dim yy As Integer = 0

                For zz = 0 To dsState.Tables(0).Rows.Count - 1

                    objWKDR = dtCSVBase.NewRow()
                    objWKDR("都道府県名") = dsState.Tables(0).Rows(zz).Item("M12_STATE_NM") 'dsTBOX_PreFuct(0).Tables(0).Rows(intIndex).Item("都道府県名").ToString
                    Debug.WriteLine(dsState.Tables(0).Rows(zz).Item("M12_STATE_NM"))
                    Dim blnMatch As Boolean = False
                    'ＴＢＯＸ種別でループ
                    'For intIndex = 0 To dsTBOX_PreFuct(0).Tables(0).Rows.Count - 1
                    For intIndex = 0 To dsTBOXLIST.Tables(0).Rows.Count - 1

                        blnMatch = False

                        If dsTBOX_PreFuct(intIndex).Tables(0).Rows.Count > 0 Then
                            For yy = 0 To dsTBOX_PreFuct(intIndex).Tables(0).Rows.Count - 1
                                If dsState.Tables(0).Rows(zz).Item("M12_STATE_NM").ToString = dsTBOX_PreFuct(intIndex).Tables(0).Rows(yy).Item("都道府県名").ToString Then
                                    Debug.WriteLine("　" & dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_NM").ToString)
                                    Debug.WriteLine("　　" & dsTBOX_PreFuct(intIndex).Tables(0).Rows(yy).Item("都道府県名").ToString)
                                    objWKDR("集計日") = dsTBOX_PreFuct(intIndex).Tables(0).Rows(0).Item("集計日").ToString
                                    objWKDR("発行元名") = dsTBOX_PreFuct(intIndex).Tables(0).Rows(0).Item("発行元名").ToString
                                    objWKDR("帳票出力日") = DateTime.Now.ToString("yyyy/MM/dd")
                                    objWKDR(dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_NM").ToString() + "_件数") = Decimal.Parse(dsTBOX_PreFuct(intIndex).Tables(0).Rows(yy).Item("D96_CNT_SUU")).ToString
                                    objWKDR(dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_NM").ToString() + "_金額") = Decimal.Parse(dsTBOX_PreFuct(intIndex).Tables(0).Rows(yy).Item("D96_SPC_AMT")).ToString
                                    blnMatch = True
                                End If
                            Next
                            If blnMatch = False Then
                                Debug.WriteLine("　" & dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_NM").ToString)
                                Debug.WriteLine("　　" & dsState.Tables(0).Rows(zz).Item("M12_STATE_NM").ToString)
                                objWKDR(dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_NM").ToString() + "_件数") = 0
                                objWKDR(dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_NM").ToString() + "_金額") = 0
                            End If
                        Else
                            Debug.WriteLine("　" & dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_NM").ToString)
                            Debug.WriteLine("　　" & dsState.Tables(0).Rows(zz).Item("M12_STATE_NM").ToString)
                            objWKDR(dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_NM").ToString() + "_件数") = 0
                            objWKDR(dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_NM").ToString() + "_金額") = 0
                        End If
                    Next
                    dtCSVBase.Rows.Add(objWKDR)
                Next

                dsCSVBase.Tables.Add(dtCSVBase)

                'If dsTBOX_PreFuct(intState).Tables(0).Rows.Count = 0 Then
                '    With dsTBOXLIST.Tables(0).Rows(intState)
                '        objWKDR(.Item("M03_TBOX_NM").ToString() + "_件数") = "0"
                '        objWKDR(.Item("M03_TBOX_NM").ToString() + "_金額") = "0"
                '    End With
                'Else
                '    With dsTBOXLIST.Tables(0).Rows(intState)
                '        If dsTBOX_PreFuct(intState).Tables(0).Rows.Count > 0 Then
                '            If dsTBOX_PreFuct(intState).Tables(0).Rows(intIndex).Item("D96_CNT_SUU") Is DBNull.Value Then
                '                objWKDR(.Item("M03_TBOX_NM").ToString() + "_件数") = "0"
                '            Else
                '                objWKDR(.Item("M03_TBOX_NM").ToString() + "_件数") = Decimal.Parse(dsTBOX_PreFuct(intState).Tables(0).Rows(intIndex).Item("D96_CNT_SUU")).ToString
                '            End If
                '        Else
                '            objWKDR(.Item("M03_TBOX_NM").ToString() + "_件数") = "0"
                '        End If
                '        If dsTBOX_PreFuct(intState).Tables(0).Rows.Count > 0 Then
                '            If dsTBOX_PreFuct(intState).Tables(0).Rows(intIndex).Item("D96_SPC_AMT") Is DBNull.Value Then
                '                objWKDR(.Item("M03_TBOX_NM").ToString() + "_金額") = "0"
                '            Else
                '                objWKDR(.Item("M03_TBOX_NM").ToString() + "_金額") = Decimal.Parse(dsTBOX_PreFuct(intState).Tables(0).Rows(intIndex).Item("D96_SPC_AMT")).ToString
                '            End If
                '        Else
                '            objWKDR(.Item("M03_TBOX_NM").ToString() + "_金額") = "0"
                '        End If
                '    End With
                'End If



                strFileclassCD = "0782CL"
                strReportName = "保守料金明細一覧表_[" & strSeikyuYm.Replace("/", "") & "]"
                If mfMakeCsv(strFileclassCD, strReportName, dsCSVBase) = False Then
                    Exit Sub
                End If
                strFileclassCD = "0789CL"
                strReportName = "保守料金明細一覧表_都道府県_2"
                If mfMakeCsv(strFileclassCD, strReportName, dsCSVBase) = False Then
                    Exit Sub
                End If

                'CMPINQP002-003     システム分類別の一覧表を取得して保存
                strReportName = "保守料金明細一覧表_ID_[" & strSeikyuYm.Replace("/", "") & "]"
                objDsID.Tables.Add(mfGetSelectColumn(dsCSVBase.Tables(0), SYSCLS_ID))
                If mfMakeCsv(strFileclassCD, strReportName, objDsID) = False Then
                    Exit Sub
                End If

                strReportName = "保守料金明細一覧表_IC_[" & strSeikyuYm.Replace("/", "") & "]"
                objDsIC.Tables.Add(mfGetSelectColumn(dsCSVBase.Tables(0), SYSCLS_IC))
                If mfMakeCsv(strFileclassCD, strReportName, objDsIC) = False Then
                    Exit Sub
                End If

                strReportName = "保守料金明細一覧表_LT_[" & strSeikyuYm.Replace("/", "") & "]"
                objDsLT.Tables.Add(mfGetSelectColumn(dsCSVBase.Tables(0), SYSCLS_LT))
                If mfMakeCsv(strFileclassCD, strReportName, objDsLT) = False Then
                    Exit Sub
                End If
                'CMPINQP002-003 END

                For intIndex = 0 To dsTBOXLIST.Tables(0).Rows.Count - 1
                    With dsTBOXLIST.Tables(0).Rows(intIndex)
                        cmdDB = New SqlCommand("CMPINQP002_S23", conDB)
                        With cmdDB.Parameters  '--パラメータ設定
                            If txtNendo.ppText = DateTime.Now.AddMonths(1).ToString("yyyy/MM") Then
                                .Add(pfSet_Param("AggDate", SqlDbType.NVarChar, DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "")))
                            Else
                                .Add(pfSet_Param("AggDate", SqlDbType.NVarChar, Date.Parse(strSeikyuYm & "/01").AddDays(-1).ToString("yyyy/MM/dd").Replace("/", "")))
                            End If
                            .Add(pfSet_Param("syscd", SqlDbType.NVarChar, dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_CD").ToString))
                        End With
                        dsTBOX_PreFuct2(intIndex) = clsDataConnect.pfGet_DataSet(cmdDB)
                    End With
                Next

                cmdDB = New SqlCommand("CMPINQP002_S26", conDB)
                dsTBOXLIST = clsDataConnect.pfGet_DataSet(cmdDB)
                dtData2.Columns.Add("帳票出力日")
                dtData2.Columns.Add("発行元名")
                dtData2.Columns.Add("集計日")
                dtData2.Columns.Add("都道府県名")
                For intIndex = 0 To dsTBOXLIST.Tables(0).Rows.Count - 1
                    With dsTBOXLIST.Tables(0).Rows(intIndex)
                        dtData2.Columns.Add(.Item("M03_TBOX_NM").ToString() + "_件数")
                        dtData2.Columns.Add(.Item("M03_TBOX_NM").ToString() + "_金額")
                    End With
                Next

                For intIndex = 0 To dsTBOX_PreFuct2(0).Tables(0).Rows.Count - 1
                    objWKDR = dtData2.NewRow()
                    objWKDR("都道府県名") = dsTBOX_PreFuct2(0).Tables(0).Rows(intIndex).Item("都道府県名").ToString
                    objWKDR("集計日") = dsTBOX_PreFuct2(0).Tables(0).Rows(intIndex).Item("集計日").ToString
                    objWKDR("発行元名") = dsTBOX_PreFuct2(0).Tables(0).Rows(intIndex).Item("発行元名").ToString
                    objWKDR("帳票出力日") = DateTime.Now.ToString("yyyy/MM/dd")
                    For intState = 0 To dsTBOXLIST.Tables(0).Rows.Count - 1
                        If dsTBOX_PreFuct2(intState).Tables(0).Rows.Count = 0 Then
                            With dsTBOXLIST.Tables(0).Rows(intState)
                                objWKDR(.Item("M03_TBOX_NM").ToString() + "_件数") = "0"
                                objWKDR(.Item("M03_TBOX_NM").ToString() + "_金額") = "0"
                            End With
                        Else
                            With dsTBOXLIST.Tables(0).Rows(intState)
                                objWKDR(.Item("M03_TBOX_NM").ToString() + "_件数") = Decimal.Parse(dsTBOX_PreFuct2(intState).Tables(0).Rows(intIndex).Item("D96_CNT_SUU")).ToString
                                objWKDR(.Item("M03_TBOX_NM").ToString() + "_金額") = Decimal.Parse(dsTBOX_PreFuct2(intState).Tables(0).Rows(intIndex).Item("D96_SPC_AMT")).ToString
                            End With
                        End If

                    Next
                    dtData2.Rows.Add(objWKDR)
                Next
                objDs0.Tables.Add(dtData2)
                strFileclassCD = "0782CL"
                strReportName = "LAN単価_[" & strSeikyuYm.Replace("/", "") & "]"
                If mfMakeCsv(strFileclassCD, strReportName, objDs0) = False Then
                    Exit Sub
                End If
                strFileclassCD = "0788CL"
                strReportName = "保守料金明細一覧表_都道府県"
                If mfMakeCsv(strFileclassCD, strReportName, objDs0) = False Then
                    Exit Sub
                End If

                dtCSVBase = Nothing
                dtCSVBase = New DataTable()

                dtCSVBase.Columns.Add("エリアコード")
                dtCSVBase.Columns.Add("エリア")
                For intIndex = 0 To dsTBOXLIST.Tables(0).Rows.Count - 1
                    With dsTBOXLIST.Tables(0).Rows(intIndex)
                        dtCSVBase.Columns.Add(.Item("M03_TBOX_NM").ToString() + "_件数")
                        dtCSVBase.Columns.Add(.Item("M03_TBOX_NM").ToString() + "_金額")
                    End With
                Next

                ReDim dsTBOX_PreFuct(dsTBOXLIST.Tables(0).Rows.Count - 1)
                ReDim dsTBOX_PreFuct2(dsTBOXLIST.Tables(0).Rows.Count - 1)
                For intIndex = 0 To dsTBOXLIST.Tables(0).Rows.Count - 1
                    With dsTBOXLIST.Tables(0).Rows(intIndex)
                        cmdDB = New SqlCommand("CMPINQP002_S22", conDB)
                        With cmdDB.Parameters  '--パラメータ設定
                            If txtNendo.ppText = DateTime.Now.AddMonths(1).ToString("yyyy/MM") Then
                                .Add(pfSet_Param("AggDate", SqlDbType.NVarChar, DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "")))
                            Else
                                .Add(pfSet_Param("AggDate", SqlDbType.NVarChar, Date.Parse(strSeikyuYm & "/01").AddDays(-1).ToString("yyyy/MM/dd").Replace("/", "")))
                            End If
                            .Add(pfSet_Param("syscd", SqlDbType.NVarChar, dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_CD").ToString))
                        End With
                        dsTBOX_PreFuct(intIndex) = clsDataConnect.pfGet_DataSet(cmdDB)
                    End With
                    With dsTBOXLIST.Tables(0).Rows(intIndex)
                        cmdDB = New SqlCommand("CMPINQP002_S24", conDB)
                        With cmdDB.Parameters
                            If txtNendo.ppText = DateTime.Now.AddMonths(1).ToString("yyyy/MM") Then
                                .Add(pfSet_Param("AggDate", SqlDbType.NVarChar, DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "")))
                            Else
                                .Add(pfSet_Param("AggDate", SqlDbType.NVarChar, Date.Parse(strSeikyuYm & "/01").AddDays(-1).ToString("yyyy/MM/dd").Replace("/", "")))
                            End If
                            .Add(pfSet_Param("syscd", SqlDbType.NVarChar, dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_CD").ToString))
                        End With
                        dsTBOX_PreFuct2(intIndex) = clsDataConnect.pfGet_DataSet(cmdDB)
                    End With
                Next

                decSpcSuu = 0
                decSpcAmt = 0
                decSpcAmt2 = 0
                For intIndex = 0 To dsTBOX_PreFuct(0).Tables(0).Rows.Count - 1
                    objWKDR = dtCSVBase.NewRow()
                    decSuu = 0
                    decAmt = 0
                    objWKDR("エリアコード") = dsTBOX_PreFuct(0).Tables(0).Rows(intIndex).Item("D97_ARE_COD").ToString
                    objWKDR("エリア") = dsTBOX_PreFuct(0).Tables(0).Rows(intIndex).Item("エリア名").ToString
                    For intState = 0 To dsTBOXLIST.Tables(0).Rows.Count - 1
                        If dsTBOX_PreFuct(intState).Tables(0).Rows.Count = 0 Then
                            With dsTBOXLIST.Tables(0).Rows(intState)
                                objWKDR(.Item("M03_TBOX_NM").ToString() + "_件数") = "0"
                                If Decimal.Parse(dsTBOX_PreFuct(0).Tables(0).Rows(intIndex).Item("D97_ARE_COD").ToString) >= 0 Then
                                    objWKDR(.Item("M03_TBOX_NM").ToString() + "_金額") = "0"
                                Else
                                    objWKDR(.Item("M03_TBOX_NM").ToString() + "_金額") = "0"
                                End If
                            End With
                        Else
                            With dsTBOXLIST.Tables(0).Rows(intState)
                                objWKDR(.Item("M03_TBOX_NM").ToString() + "_件数") = Decimal.Parse(dsTBOX_PreFuct(intState).Tables(0).Rows(intIndex).Item("D97_CNT_SUU")).ToString()
                                If Decimal.Parse(dsTBOX_PreFuct(0).Tables(0).Rows(intIndex).Item("D97_ARE_COD").ToString) >= 0 Then
                                    objWKDR(.Item("M03_TBOX_NM").ToString() + "_金額") = Decimal.Parse(dsTBOX_PreFuct(intState).Tables(0).Rows(intIndex).Item("D97_CNT_AMT")).ToString()
                                Else
                                    objWKDR(.Item("M03_TBOX_NM").ToString() + "_金額") = Decimal.Parse(dsTBOX_PreFuct(intState).Tables(0).Rows(intIndex).Item("D97_SPC_AMT")).ToString()
                                End If
                                decSuu += Decimal.Parse(dsTBOX_PreFuct(intState).Tables(0).Rows(intIndex).Item("D97_CNT_SUU").ToString)
                                decAmt += Decimal.Parse(dsTBOX_PreFuct(intState).Tables(0).Rows(intIndex).Item("D97_CNT_AMT").ToString)
                                decSpcSuu += Decimal.Parse(dsTBOX_PreFuct(intState).Tables(0).Rows(intIndex).Item("D97_CNT_SUU").ToString)
                                decSpcAmt += Decimal.Parse(dsTBOX_PreFuct(intState).Tables(0).Rows(intIndex).Item("D97_SPC_AMT").ToString)
                                decSpcAmt2 += Decimal.Parse(dsTBOX_PreFuct(intState).Tables(0).Rows(intIndex).Item("D97_SPC_AMT2").ToString)
                            End With
                        End If

                    Next

                    'データ表示
                    'Select Case dsTBOX_PreFuct(0).Tables(0).Rows(intIndex).Item("D97_ARE_COD")
                    '    Case M_AREA_FS  'ＦＳエリア
                    '        Me.lblFsAreaCount.Text = decSuu.ToString()
                    '        Me.lblFsAreaAmount.Text = decAmt.ToString()
                    '    Case M_AREA_CS  'ＣＳエリア
                    '        Me.lblCsAreaCount.Text = decSuu.ToString()
                    '        Me.lblCsAreaAmount.Text = decAmt.ToString()
                    '    Case M_AREA_OTH 'その他エリア
                    '        Me.lblOtAreaCount.Text = decSuu.ToString()
                    '        Me.lblOtAreaAmount.Text = decAmt.ToString()
                    '    Case Else       '総合計
                    '        Me.lblTotalAreaCount.Text = decSuu.ToString()
                    '        Me.lblTotalAreaAmount.Text = decSpcAmt.ToString()
                    'End Select

                    dtCSVBase.Rows.Add(objWKDR)
                Next

                decLan_Cnt = 0
                decLan_Price = 0
                For intIndex = 0 To dsTBOX_PreFuct2.Count - 1
                    If dsTBOX_PreFuct2(intIndex).Tables(0).Rows.Count <> 0 Then
                        decLan_Cnt += dsTBOX_PreFuct2(intIndex).Tables(0).Rows(0).Item("件数")
                        decLan_Price += dsTBOX_PreFuct2(intIndex).Tables(0).Rows(0).Item("単価")
                    End If
                Next

                objDs2.Tables.Add(dtCSVBase)
                strFileclassCD = "0788CL"
                strReportName = "保守料金明細一覧表_エリア別_[" & strSeikyuYm.Replace("/", "") & "]"
                If mfMakeCsv(strFileclassCD, strReportName, objDs2) = False Then
                    Exit Sub
                End If

                Dim FsCnt() As Integer = Nothing
                Dim FsPrice() As Integer = Nothing
                Dim CsCnt() As Integer = Nothing
                Dim CsPrice() As Integer = Nothing
                Dim OthCnt() As Integer = Nothing
                Dim OthPrice() As Integer = Nothing
                Dim intTypeCount As Integer = 0

                intTypeCount = (dsCSVBase.Tables(0).Columns.Count - 4) / 2

                '--エリア・TBOXタイプ別件数、金額
                'If intTypeCount > 0 Then
                '    intTypeCount = intTypeCount - 2
                'End If
                ReDim FsCnt(intTypeCount)
                ReDim FsPrice(intTypeCount)
                ReDim CsCnt(intTypeCount)
                ReDim CsPrice(intTypeCount)
                ReDim OthCnt(intTypeCount)
                ReDim OthPrice(intTypeCount)

                With objDs2.Tables(0)
                    For i = 0 To .Rows.Count - 1
                        For j = 0 To intTypeCount - 1
                            'ＦＳエリア
                            If .Rows(i).Item(0) = "0" Then
                                FsCnt(j) = 0
                                FsPrice(j) = 0
                            End If
                            'ＣＳエリア
                            If .Rows(i).Item(0) = "1" Then
                                CsCnt(j) = 0
                                CsPrice(j) = 0
                            End If
                            'その他エリア
                            If Decimal.Parse(.Rows(i).Item(0)) < 0 Then
                                OthCnt(j) = .Rows(i).Item(j * 2 + 2)
                                OthPrice(j) = .Rows(i).Item(j * 2 + 3)
                            End If
                        Next
                    Next
                End With

                decMnt_Cnt = 0
                decMnt_Price = 0
                For intcnt As Integer = 0 To OthCnt.Count - 1
                    decMnt_Cnt += OthCnt(intcnt)
                    'decMnt_Price += OthPrice(intcnt)
                Next

                decMnt_Price = decSpcAmt2

                '特別保守費用一覧.
                cmdDB = New SqlCommand("CMPINQP002_S14", conDB)
                cmdDB.Parameters.Add(pfSet_Param("supportdt", SqlDbType.NVarChar, strSeikyuYm))
                cmdDB.Parameters.Add(pfSet_Param("retcnt", SqlDbType.Int, 10, ParameterDirection.Output))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                intRetCnt = 0
                intRetCnt = cmdDB.Parameters("retcnt").Value
                strFileclassCD = "0783CL"
                strReportName = "特別保守費用一覧_[" & strSeikyuYm.Replace("/", "") & "]"
                If mfMakeCsv(strFileclassCD, strReportName, dstOrders) = False Then
                    Exit Sub
                End If

                '特別保守作業集計.
                cmdDB = New SqlCommand("CMPINQP002_S13", conDB)
                cmdDB.Parameters.Add(pfSet_Param("supportdt", SqlDbType.NVarChar, strSeikyuYm))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                strFileclassCD = "0784CL"
                strReportName = "特別保守作業集計_[" & strSeikyuYm.Replace("/", "") & "]"
                If mfMakeCsv(strFileclassCD, strReportName, dstOrders) = False Then
                    Exit Sub
                End If

                '特別保守料金取得.
                For i As Integer = 0 To dstOrders.Tables(0).Rows.Count - 1
                    dec = Nothing
                    If intRetCnt = 0 Then
                        decPre_Cnt = 0
                        decPre_Price = 0
                    Else
                        If Decimal.TryParse(dstOrders.Tables(0).Rows(i).Item("合計件数").ToString, dec) Then
                            decPre_Cnt = dec
                        End If
                        If Decimal.TryParse(dstOrders.Tables(0).Rows(i).Item("合計金額").ToString, dec) Then
                            decPre_Price = dec
                        End If
                    End If
                Next

                '緊急運営輸送費.
                cmdDB = New SqlCommand("CMPINQP002_S6", conDB)
                cmdDB.Parameters.Add(pfSet_Param("Tekiyou_YM", SqlDbType.NVarChar, strSeikyuYm))
                cmdDB.Parameters.Add(pfSet_Param("Ret_Cnt", SqlDbType.Int, 10, ParameterDirection.Output))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                intRetCnt = 0
                intRetCnt = cmdDB.Parameters("Ret_Cnt").Value
                strFileclassCD = "0785CL"
                strReportName = "緊急運営輸送費_[" & strSeikyuYm.Replace("/", "") & "]"
                If mfMakeCsv(strFileclassCD, strReportName, dstOrders) = False Then
                    Exit Sub
                End If

                dec = Nothing

                If dstOrders.Tables(0).Rows.Count > 0 Then
                    If Decimal.TryParse(dstOrders.Tables(0).Rows(0).Item("合計").ToString, dec) Then
                        decSpc_Price += dec
                    End If
                    If intRetCnt = 0 Then
                        decSpc_Cnt = 0
                    Else
                        decSpc_Cnt = dstOrders.Tables(0).Rows.Count
                    End If
                End If

                '有償部品費用.
                cmdDB = New SqlCommand("CMPINQP002_S10", conDB)

                cmdDB.Parameters.Add(pfSet_Param("req_dt", SqlDbType.NVarChar, strSeikyuYm))
                cmdDB.Parameters.Add(pfSet_Param("retcnt", SqlDbType.Int, 10, ParameterDirection.Output))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                intRetCnt = 0
                intRetCnt = cmdDB.Parameters("retcnt").Value
                strFileclassCD = "0786CL"

                strReportName = "修理有償部品費用_[" & strSeikyuYm.Replace("/", "") & "]"

                If mfMakeCsv(strFileclassCD, strReportName, dstOrders) = False Then
                    Exit Sub
                End If

                '修理作業・運送費用 有償部品代 集計
                cmdDB = New SqlCommand("CMPINQP002_S18", conDB)
                cmdDB.Parameters.Add(pfSet_Param("req_dt", SqlDbType.NVarChar, strSeikyuYm))

                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                strFileclassCD = "0787CL"

                '件数取得
                cmdDB = New SqlCommand("CMPINQP002_S25", conDB)
                cmdDB.Parameters.Add(pfSet_Param("req_dt", SqlDbType.NVarChar, strSeikyuYm))

                dstOrders2 = clsDataConnect.pfGet_DataSet(cmdDB)
                decOth_Cnt = 0
                dec = Nothing
                If dstOrders2.Tables(0).Rows.Count > 0 Then
                    For intcnt As Integer = 0 To dstOrders2.Tables(0).Rows.Count - 1
                        Select Case dstOrders2.Tables(0).Rows(intcnt).Item("SYSTEM_CD").ToString
                            Case 1
                                Select Case dstOrders2.Tables(0).Rows(intcnt).Item("SUM_CLS").ToString
                                    Case 1
                                        dstOrders.Tables(0).Rows(0).Item("修理ID無線件数") += dstOrders2.Tables(0).Rows(intcnt).Item("件数").ToString
                                        dstOrders.Tables(0).Rows(0).Item("部品ID無線件数") += dstOrders2.Tables(0).Rows(intcnt).Item("件数").ToString
                                    Case 2
                                        dstOrders.Tables(0).Rows(0).Item("修理ID有線件数") += dstOrders2.Tables(0).Rows(intcnt).Item("件数").ToString
                                        dstOrders.Tables(0).Rows(0).Item("部品ID有線件数") += dstOrders2.Tables(0).Rows(intcnt).Item("件数").ToString
                                End Select
                            Case 3
                                dstOrders.Tables(0).Rows(0).Item("修理IC件数") += dstOrders2.Tables(0).Rows(intcnt).Item("件数").ToString
                                dstOrders.Tables(0).Rows(0).Item("部品IC件数") += dstOrders2.Tables(0).Rows(intcnt).Item("件数").ToString
                            Case 5
                                dstOrders.Tables(0).Rows(0).Item("修理LUTERNA件数") += dstOrders2.Tables(0).Rows(intcnt).Item("件数").ToString
                                dstOrders.Tables(0).Rows(0).Item("部品LUTERNA件数") += dstOrders2.Tables(0).Rows(intcnt).Item("件数").ToString
                            Case Else
                        End Select
                        If Decimal.TryParse(dstOrders2.Tables(0).Rows(intcnt).Item("件数").ToString, dec) Then
                            decOth_Cnt += dec
                        End If
                    Next
                    dstOrders.Tables(0).Rows(0).Item("合計件数") = decOth_Cnt.ToString
                End If

                strReportName = "修理作業_運送費用有償部品代集計_[" & strSeikyuYm.Replace("/", "") & "]"

                If mfMakeCsv(strFileclassCD, strReportName, dstOrders) = False Then
                    Exit Sub
                End If

                '有償部品代費取得.                
                For i As Integer = 0 To dstOrders.Tables(0).Rows.Count - 1
                    dec = Nothing
                    If Decimal.TryParse(dstOrders.Tables(0).Rows(0).Item("合計金額").ToString, dec) Then
                        decOth_Price = dec
                    End If
                    If Decimal.TryParse(dstOrders.Tables(0).Rows(0).Item("合計件数").ToString, dec) Then
                        'decOth_Cnt = dec
                    End If
                Next

                If mfUpdate(strSeikyuYm.Replace("/", ""),
                            decCnst_Cnt,
                            decCnst_Price,
                            decMnt_Cnt,
                            decMnt_Price,
                            decPre_Cnt,
                            decPre_Price,
                            decSpc_Cnt,
                            decSpc_Price,
                            decOth_Cnt,
                            decOth_Price,
                            decLan_Cnt,
                            decLan_Price) = False Then
                    Throw New Exception
                End If

                '情報機器保守の報告書兼検収書.
                cmdDB = New SqlCommand("CMPINQP002_S7", conDB)
                cmdDB.Parameters.Add(pfSet_Param("prm_date", SqlDbType.NVarChar, strSeikyuYm))
                If txtNendo.ppText = DateTime.Now.AddMonths(1).ToString("yyyy/MM") Then
                    cmdDB.Parameters.Add(pfSet_Param("prm_degree", SqlDbType.NVarChar, DateTime.Now.ToString("yyyy/MM/dd")))
                Else
                    cmdDB.Parameters.Add(pfSet_Param("prm_degree", SqlDbType.NVarChar, Date.Parse(strSeikyuYm & "/01").AddDays(-1).ToString("yyyy/MM/dd")))
                End If
                cmdDB.Parameters.Add(pfSet_Param("outputflg", SqlDbType.NVarChar, "1"))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                If dstOrders.Tables(0).Rows.Count < 4 Then
                    '0件
                    If txtNendo.ppText = DateTime.Now.AddMonths(1).ToString("yyyy/MM") Then
                        psMesBox(Me, "10010", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, DateTime.Now.ToString("yyyy/MM/dd"))
                    Else
                        psMesBox(Me, "00009", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    End If
                    Exit Sub
                End If
                strFileclassCD = "0781CL"
                strReportName = "情報機器保守の報告書兼検収書_[" & strSeikyuYm.Replace("/", "") & "]"
                If mfMakeCsv(strFileclassCD, strReportName, dstOrders) = False Then
                    Exit Sub
                End If

                '完了メッセージ.
                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, Master.Master.ppRigthButton1.Text)

                '再検索
                Call msSeach_Data(strSeikyuYm)

            End If

        Catch ex As Exception

            If intShori = 0 Then
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "情報機器保守検収書")
            End If

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

            grvList.DataSource = New DataTable()

            Me.grvList.DataBind()

            Throw
        Finally
            'DB切断
            clsDataConnect.pfClose_Database(conDB)
        End Try

    End Sub

    ''' <summary>
    ''' 再検索処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSeach_Data(ByVal strDegree As String)

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet
        Dim intIns As Integer = 0 '検収締め区分
        Dim intReq As Integer = 0 '請求締め区分

        objStack = New StackFrame

        Try

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else

                '画面ページ表示初期化
                Master.ppCount = ""
                Me.grvList.DataSource = New DataTable

                'パラメータ設定
                cmdDB = New SqlCommand("CMPINQP002_S1", conDB)

                With cmdDB.Parameters
                    .Add(pfSet_Param("Tekiyou_YM", SqlDbType.NVarChar, strDegree.Replace("/", "")))
                End With

                'データ取得およびデータをリストに設定
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                Me.grvList.DataSource = dstOrders

                '件数を設定
                Master.ppCount = dstOrders.Tables(0).Rows.Count

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                '変更を反映
                Me.grvList.DataBind()

                For i As Integer = 0 To Me.grvList.Rows.Count - 1
                    Dim rowData As GridViewRow = grvList.Rows(i)
                    intIns = CType(rowData.FindControl("検収締め"), TextBox).Text
                    intReq = CType(rowData.FindControl("請求締め"), TextBox).Text

                    '締まっている場合.
                    'If CType(rowData.FindControl("検収締め"), TextBox).Text = "1" Then

                    '    '印刷/ＣＳＶ非活性.
                    '    rowData.Cells.Item(0).Enabled = False
                    '    rowData.Cells.Item(1).Enabled = False


                    'Else '締まっていない場合.

                    '    '印刷/ＣＳＶ活性.
                    '    rowData.Cells.Item(0).Enabled = True
                    '    rowData.Cells.Item(1).Enabled = True
                    'End If

                    '特別保守費用一覧、有償部品費用の行には一覧ボタンと集計ボタンを設定
                    If CType(rowData.Cells(3).Controls(0), TextBox).Text = "特別保守費用一覧" OrElse CType(rowData.Cells(3).Controls(0), TextBox).Text = "有償部品費用" Then
                        CType(rowData.Cells(1).Controls(0), Button).Text = " 一覧 "
                        CType(rowData.Cells(2).Controls(0), Button).Text = "集計"

                        '締めているか？
                        'If CType(rowData.FindControl("検収締め"), TextBox).Text = "1" Then
                        '    rowData.Cells.Item(2).Enabled = False
                        'Else
                        '    rowData.Cells.Item(2).Enabled = True
                        'End If
                    Else
                        '使わないボタンは見出しを空白とし、非活性
                        CType(rowData.Cells(2).Controls(0), Button).Text = "　　"
                        rowData.Cells.Item(2).Enabled = False
                        If CType(rowData.Cells(3).Controls(0), TextBox).Text = "LAN単価" Then
                            CType(rowData.Cells(0).Controls(0), Button).Text = "　　"
                            rowData.Cells.Item(0).Enabled = False
                        End If
                    End If

                Next

                '画面状態更新
                msSetEnable()

                '確認メッセージ再設定.
                Master.Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton1.Text).Replace(";", "") + "&&dispWait('count');" '締め確認

            End If
        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "情報機器保守検収書")
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            grvList.DataSource = New DataTable()
            Me.grvList.DataBind()

            Throw
        Finally
            'DB切断
            clsDataConnect.pfClose_Database(conDB)
        End Try

    End Sub
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

#End Region
    '--------------------------------
    '2014/05/01 後藤　ここから
    '--------------------------------
#Region "■ 検収月取得"

    ''' <summary>
    ''' 検収月の取得
    ''' </summary>
    ''' <returns>
    ''' strSetkyuYm  = 'yyyy/MM'
    ''' strDegreeYmd = 'yyyy/MM/dd'（末日） 
    ''' </returns>
    ''' <remarks></remarks>
    Private Function mfGetKensyuYm(ByRef strSeikyuYm As String, ByRef strDegreeYmd As String) As Boolean
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        strSeikyuYm = ""
        strDegreeYmd = ""
        mfGetKensyuYm = False

        Try
            'ViewStateに保存されている検収月を取得する
            'strDegreeYmdは検収月の末日

            'strSeikyuYm = Me.txtNendo.ppText
            If ViewState(KENSYU_YM) = Nothing Then
                strSeikyuYm = String.Empty
                strDegreeYmd = String.Empty
            Else
                strSeikyuYm = ViewState(KENSYU_YM)
                strDegreeYmd = Date.Parse(strSeikyuYm & "/01").AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd")
            End If

            ''ＤＢ接続
            'If Not pfOpen_Database(conDB) Then
            '    Throw New Exception("")
            'End If

            ''検収月の算出
            'cmdDB = New SqlCommand("CMPINQP002_S11", conDB)
            'With cmdDB.Parameters
            '    .Add(pfSet_Param("prmMAXDATE", SqlDbType.NVarChar, 7, ParameterDirection.Output))
            '    .Add(pfSet_Param("prmDEGREE", SqlDbType.NVarChar, 20, ParameterDirection.Output))
            'End With
            'Dim objDs As DataSet = pfGet_DataSet(cmdDB)

            ''検収月(yyyy/mm)
            'If cmdDB.Parameters("prmMAXDATE").Value Is Nothing OrElse cmdDB.Parameters("prmMAXDATE").Value = "" Then
            '    Throw New Exception()
            'Else
            '    strSeikyuYm = cmdDB.Parameters("prmMAXDATE").Value
            'End If

            ''検収月(yyyy/mm/dd)
            'If cmdDB.Parameters("prmDEGREE").Value Is Nothing OrElse cmdDB.Parameters("prmDEGREE").Value = "" Then
            '    Throw New Exception()
            'Else
            '    strDegreeYmd = cmdDB.Parameters("prmDEGREE").Value
            'End If

            mfGetKensyuYm = True

        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "検収月の取得")
            strSeikyuYm = ""
            strDegreeYmd = ""
            mfGetKensyuYm = False
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                strSeikyuYm = ""
                strDegreeYmd = ""
                mfGetKensyuYm = False
            End If
        End Try
    End Function

#End Region
    '--------------------------------
    '2014/05/01 後藤　ここまで
    '--------------------------------
#Region "CSV処理"
    ''' <summary>
    ''' ＣＳＶ作成処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfMakeCsv(ByVal ipstrFileclassCD As String,
                               ByVal ipstrReportName As String,
                               ByVal ds As DataSet) As Boolean

        Dim strServerAddress As String = String.Empty 'サーバアドレス
        Dim strFolderNM As String = String.Empty 'フォルダ名
        Dim strWorkPath As String = String.Empty '出力パス
        Dim strlocalpath As String = "C:\UPLOAD\"
        Dim localFileName As String = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString
        Dim opblnResult As Boolean = False

        'CSV出力.
        If pfCreateCsvFile(strlocalpath, localFileName & ".csv", ds, True) <> 0 Then
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
            Return False
        End If

        'サーバアドレス取得.
        If pfGetPreservePlace(ipstrFileclassCD, strServerAddress, strFolderNM) <> 0 Then
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
            Return False
        End If

        If pfFile_Upload(strFolderNM, ipstrReportName & ".csv", localFileName) = False Then
            psMesBox(Me, "50003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
            Return False
        End If
        ''出力パス生成.
        'strWorkPath = strFolderNM & "\"

        'dirpath = New DirectoryInfo(strWorkPath)
        ''保存先のフォルダの存在有無を確認
        'If DBFTP.pfFtpDir_Exists(dirpath.ToString, opblnResult, "0") = False Then
        '    '存在しなかった場合、フォルダを作成しアップロード
        '    If DBFTP.pfFtpFile_Copy("PUT", dirpath.ToString, "csv", ipstrReportName & ".csv", opblnResult, strlocalpath & "\" & localFileName & ".csv") = False Then
        '        'アップロードに失敗
        '        Throw New Exception
        '    End If
        '    'フォルダが存在した場合
        'Else
        '    If DBFTP.pfFtpFile_Exists(dirpath.ToString, ipstrReportName & ".csv", opblnResult) = True Then
        '        If DBFTP.pfFtpFile_Delete(dirpath.ToString, ipstrReportName & ".csv", opblnResult) = False Then
        '            'アップロードに失敗
        '            Throw New Exception
        '        End If
        '    End If
        '    If DBFTP.pfFtpFile_Copy("PUT", dirpath.ToString, ipstrReportName & ".csv", opblnResult, strlocalpath & localFileName & ".csv") = False Then
        '        'アップロードに失敗
        '        Throw New Exception
        '    End If
        'End If

        ''ローカルに一時的に作成したファイルを削除
        ''ファイルの存在を確認
        'If System.IO.File.Exists(strlocalpath & localFileName & ".csv") Then
        '    System.IO.File.Delete(strlocalpath & localFileName & ".csv")
        'End If

        Return True

    End Function
    ''' <summary>
    ''' CSVファイル取り込み.
    ''' </summary>
    ''' <param name="ipstrFileclassCD"></param>
    ''' <param name="ipstrReportName"></param>
    ''' <param name="CSVData"></param>
    ''' <param name="Cnt"></param>
    ''' <remarks></remarks>
    Private Sub ms_GetCSVData(ByVal ipstrFileclassCD As String,
                              ByVal ipstrReportName As String,
                              ByRef CSVData As ArrayList,
                              ByRef Cnt As Integer,
                              ByRef strArrayList As String())

        Dim strServerAddress As String = String.Empty    'サーバアドレス
        Dim strFolderNM As String = String.Empty         'フォルダ名
        Dim strFilePath As String = String.Empty         'ファイルパス
        Dim strDownloadFileName As String = String.Empty '出力パス
        Dim strData As String = String.Empty
        Dim utf As Encoding = Encoding.UTF8
        Dim sjis As Encoding = Encoding.GetEncoding("utf-8")

        'サーバアドレス取得.
        If pfGetPreservePlace(ipstrFileclassCD, strServerAddress, strFolderNM) <> 0 Then
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
        End If

        strFilePath = pfFile_Download(strFolderNM, ipstrReportName & ".csv")
        If strFilePath = "" Then
            'psMesBox(Me, "30003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, ipstrReportName)
            Exit Sub
        End If

        'パス生成.
        'strFilePath = "\\" & strServerAddress & "\" & strFolderNM & "\" & ipstrReportName & ".csv"

        Dim sr As StreamReader = New StreamReader(strFilePath, System.Text.Encoding.Default)  'ファイルの読み込み
        Dim errMsg As String = "ＣＳＶファイル"                                            'エラーメッセージ

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

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

        Catch ex As Exception

            'システムエラー
            psMesBox(Me, "30002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, errMsg)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Throw ex

        Finally

            sr.Close()                 'ファイルクローズ

        End Try

    End Sub
    ''' <summary>
    ''' CSVダウンロード.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfCsvDownload(ByVal ipstrFileclassCD As String,
                                   ByVal ipstrReportName As String) As Boolean

        Dim strServerAddress As String = String.Empty    'サーバアドレス
        Dim strFolderNM As String = String.Empty         'フォルダ名
        Dim strFilePath As String = String.Empty         'ファイルパス
        Dim strDownloadFileName As String = String.Empty '出力パス
        Dim strData As String = String.Empty
        Dim utf As Encoding = Encoding.UTF8
        Dim sjis As Encoding = Encoding.GetEncoding("utf-8")
        Dim localFileName As String = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString
        Dim strfileName As String = Nothing
        Dim localdirName As String = "DOWNLOAD"
        Dim localFiledir As String = "C:"
        Dim strLocalPath As String
        Dim filePath2 As String = Nothing
        Dim Fullpath As HttpPostedFile = Nothing
        Dim strExtension As String = Nothing
        Dim opblnResult As Boolean = False

        mfCsvDownload = False

        'サーバアドレス取得.
        If pfGetPreservePlace(ipstrFileclassCD, strServerAddress, strFolderNM) <> 0 Then
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
            Return True
        End If

        strfileName = ipstrReportName & ".csv"

        strLocalPath = pfFile_Download(strFolderNM, strfileName)
        If strLocalPath = "" Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
            Return True
        End If


        Response.Redirect("~/Common/COMLSTP099/COMLSTP099.ashx?path=" & strLocalPath & "&filename=" & HttpUtility.UrlEncode(strfileName), False)

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
        '■□■□結合試験時のみ使用予定□■□■
        '--------------------------------
        '2014/04/16 星野　ここまで
        '--------------------------------
        'Response.Redirect("~/Common/COMLSTP099/COMLSTP099.ashx?path=" & strFilePath & "&filename=" & strDownloadFileName, False)

        Return True

    End Function
    ''' <summary>
    ''' PDF作成.
    ''' </summary>
    ''' <param name="CsvDate"></param>
    ''' <param name="Cnt"></param>
    ''' <remarks></remarks>
    Private Overloads Sub ms_MakePdf(ByVal ipstrReportName As String,
                                     ByVal CsvDate As ArrayList,
                                     ByVal Cnt As Integer,
                                     ByVal rpt As Object,
                                     ByVal strArrayList As String(),
                                     ByVal intPrint As Integer)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
        '--------------------------------
        '2014/05/04 後藤　ここから
        '--------------------------------
        'Dim strCsv() As String
        Dim dt As New DataTable
        Dim dtTime As New DateTime
        Dim dtKenshusyo As New DataTable
        Dim dtYuso As New DataTable

        ''データテーブルカラム作成(ヘッダ)
        'For i As Integer = 0 To strArrayList.Length - 1
        '    dt.Columns.Add(strArrayList(i))
        'Next

        ''行数分ループ
        'For i As Integer = 1 To CsvDate.Count - 1

        '    strCsv = Nothing

        '    'カンマ毎に分割
        '    strCsv = CsvDate(i).ToString.Split(",")

        '    dt.Rows.Add(strCsv)

        'Next
        msCreateTable(strArrayList, CsvDate, dt)
        '--------------------------------
        '2014/05/04 後藤　ここまで
        '--------------------------------

        '特別保守費用、緊急運営輸送費、修理・有償部品費のデータ取得
        'DB接続
        'If Not clsDataConnect.pfOpen_Database(conDB) Then
        '    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        'Else
        '    cmdDB = New SqlCommand("CMPINQP002_S20", conDB)
        '    With cmdDB.Parameters
        '        '--パラメータ設定
        '        .Add(pfSet_Param("req_dt", SqlDbType.NVarChar, (dt.Rows(0).Item("対象日付").ToString).Substring(0, 7)))
        '    End With

        '    'データ取得
        '    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
        'End If

        Select Case intPrint
            Case 0 '情報機器保守の報告書兼検収書.
                dtKenshusyo.Columns.Add("対象日付", GetType(DateTime))
                dtKenshusyo.Columns.Add("出力日付", GetType(DateTime))
                dtKenshusyo.Columns.Add("TBOXID", GetType(String))
                dtKenshusyo.Columns.Add("保守料金", GetType(Integer))
                dtKenshusyo.Columns.Add("磁気無線件数", GetType(String))
                dtKenshusyo.Columns.Add("磁気無線系金額", GetType(String))
                dtKenshusyo.Columns.Add("磁気有線件数", GetType(String))
                dtKenshusyo.Columns.Add("磁気有線系金額", GetType(String))
                dtKenshusyo.Columns.Add("IC件数", GetType(String))
                dtKenshusyo.Columns.Add("IC金額", GetType(String))
                dtKenshusyo.Columns.Add("LUTERNA件数", GetType(String))
                dtKenshusyo.Columns.Add("LUTERNA金額", GetType(String))
                dtKenshusyo.Columns.Add("保守料金件数", GetType(String))
                dtKenshusyo.Columns.Add("保守料金料金", GetType(String))
                dtKenshusyo.Columns.Add("請求先検収会社", GetType(String))
                dtKenshusyo.Columns.Add("請求先検収者", GetType(String))
                dtKenshusyo.Columns.Add("宛先電話番号", GetType(String))
                dtKenshusyo.Columns.Add("宛先FAX", GetType(String))
                dtKenshusyo.Columns.Add("宛先会社名", GetType(String))
                dtKenshusyo.Columns.Add("宛先名", GetType(String))
                dtKenshusyo.Columns.Add("出精値引き率", GetType(String))
                dtKenshusyo.Columns.Add("出精値引き", GetType(String))
                dtKenshusyo.Columns.Add("課税対象料金計", GetType(String))
                dtKenshusyo.Columns.Add("消費税相当額", GetType(String))
                dtKenshusyo.Columns.Add("合計金額", GetType(String))
                '--------------------------------
                '2014/05/08 後藤　ここから
                '--------------------------------
                dtKenshusyo.Columns.Add("発行元住所", GetType(String))
                '--------------------------------
                '2014/05/08 後藤　ここまで
                '--------------------------------
                'CMPINQP002-001
                dtKenshusyo.Columns.Add("請求先検収者フッター", GetType(String))
                'CMPINQP002-001 END

                'dtKenshusyo.Columns.Add("保守料金計", GetType(Integer))



                For i As Integer = 0 To dt.Rows.Count - 1

                    If DateTime.TryParse(dt.Rows(i).Item("対象日付").ToString, dtTime) Then
                        dt.Rows(i).Item("対象日付") = dtTime
                    Else
                        dt.Rows(i).Item("対象日付") = DBNull.Value
                    End If
                    ''データセット編集
                    'Select Case dt.Rows(i).Item("TBOXID")
                    '    Case "特別保守費用"
                    '        dt.Rows(i).Item("磁気無線件数") = dstOrders.Tables(0).Rows(0).Item("無線系件数")
                    '    Case "緊急運営輸送費"
                    '    Case "修理・有償部品費"
                    'End Select

                    If Not dt.Columns.Contains("請求先検収者フッター") Then
                        dtKenshusyo.Rows.Add(dt.Rows(i).Item("対象日付"),
                                        dt.Rows(i).Item("出力日付"),
                                        dt.Rows(i).Item("TBOXID"),
                                        dt.Rows(i).Item("保守料金"),
                                        dt.Rows(i).Item("磁気無線件数"),
                                        dt.Rows(i).Item("磁気無線系金額"),
                                        dt.Rows(i).Item("磁気有線件数"),
                                        dt.Rows(i).Item("磁気有線系金額"),
                                        dt.Rows(i).Item("IC件数"),
                                        dt.Rows(i).Item("IC金額"),
                                        dt.Rows(i).Item("LUTERNA件数"),
                                        dt.Rows(i).Item("LUTERNA金額"),
                                        dt.Rows(i).Item("保守料金件数"),
                                        dt.Rows(i).Item("保守料金料金"),
                                        dt.Rows(0).Item("請求先検収会社"),
                                        dt.Rows(0).Item("請求先検収者"),
                                        dt.Rows(0).Item("宛先電話番号"),
                                        dt.Rows(0).Item("宛先FAX"),
                                        dt.Rows(0).Item("宛先会社名"),
                                        dt.Rows(0).Item("宛先名"),
                                        dt.Rows(0).Item("出精値引き率"),
                                        dt.Rows(0).Item("出精値引き"),
                                        dt.Rows(0).Item("課税対象料金計"),
                                        dt.Rows(0).Item("消費税相当額"),
                                        dt.Rows(0).Item("合計金額"),
                                        dt.Rows(0).Item("発行元住所"))
                    Else
                        'CMPINQP002-001
                        dtKenshusyo.Rows.Add(dt.Rows(i).Item("対象日付"),
                                             dt.Rows(i).Item("出力日付"),
                                             dt.Rows(i).Item("TBOXID"),
                                             dt.Rows(i).Item("保守料金"),
                                             dt.Rows(i).Item("磁気無線件数"),
                                             dt.Rows(i).Item("磁気無線系金額"),
                                             dt.Rows(i).Item("磁気有線件数"),
                                             dt.Rows(i).Item("磁気有線系金額"),
                                             dt.Rows(i).Item("IC件数"),
                                             dt.Rows(i).Item("IC金額"),
                                             dt.Rows(i).Item("LUTERNA件数"),
                                             dt.Rows(i).Item("LUTERNA金額"),
                                             dt.Rows(i).Item("保守料金件数"),
                                             dt.Rows(i).Item("保守料金料金"),
                                             dt.Rows(0).Item("請求先検収会社"),
                                             dt.Rows(0).Item("請求先検収者"),
                                             dt.Rows(0).Item("宛先電話番号"),
                                             dt.Rows(0).Item("宛先FAX"),
                                             dt.Rows(0).Item("宛先会社名"),
                                             dt.Rows(0).Item("宛先名"),
                                             dt.Rows(0).Item("出精値引き率"),
                                             dt.Rows(0).Item("出精値引き"),
                                             dt.Rows(0).Item("課税対象料金計"),
                                             dt.Rows(0).Item("消費税相当額"),
                                             dt.Rows(0).Item("合計金額"),
                                             dt.Rows(0).Item("発行元住所"),
                                             dt.Rows(0).Item("請求先検収者フッター"))
                    End If 'CMPINQP002-001
                Next

                '会社名の取得
                Dim dtrSend As DataRow = mfGetTraderInfo("7")   'サポートセンター情報
                Dim dtrRcv As DataRow = mfGetTraderInfo("8")    'MOP情報

                'ヘッダー＆フッター情報をデータテーブルに追加
                For Each dtr As DataRow In dtKenshusyo.Rows
                    dtr.Item("請求先検収会社") = dtrRcv.Item("M40_SEND_NM1")
                    dtr.Item("請求先検収者") = dtrRcv.Item("M40_SEND_NM1_2")
                    dtr.Item("宛先会社名") = dtrSend.Item("M40_SEND_NM1")
                    dtr.Item("宛先名") = dtrSend.Item("M40_SEND_NM1_2") & "　" & dtrSend.Item("M40_SEND_NM1_3") & "　宛"
                    dtr.Item("発行元住所") = dtrSend.Item("M40_ADDR1")
                    dtr.Item("宛先電話番号") = dtrSend.Item("M40_TELNO1")
                    dtr.Item("宛先FAX") = dtrSend.Item("M40_FAXNO1")
                    If dtKenshusyo.Columns.Contains("請求先検収者フッター") Then
                        dtr.Item("請求先検収者フッター") = dtrRcv.Item("M40_SEND_NM1_3")
                    End If
                Next

                'PDF出力処理.
                psPrintPDF(Me, rpt, dtKenshusyo, ipstrReportName)

                '--------------------------------
                '2014/005/01 後藤　ここから
                '--------------------------------
                '保守料金明細一覧表は別メソッド（mfPDF）で出力する
            Case 1 '保守料金明細.

                'mfPDF(Me, rpt, dt, ipstrReportName)
                'PDF出力処理.
                'psPrintPDF(Me, rpt, dt, ipstrReportName)
                '--------------------------------
            Case 2 '緊急運営輸送費.
                dtYuso.Columns.Add("対象年月", GetType(String))
                dtYuso.Columns.Add("輸送日", GetType(DateTime))
                dtYuso.Columns.Add("輸送元", GetType(String))
                dtYuso.Columns.Add("輸送先", GetType(String))
                dtYuso.Columns.Add("ＴＢＯＸ", GetType(String))
                dtYuso.Columns.Add("ホール", GetType(String))
                dtYuso.Columns.Add("ＴＢＯＸタイプ", GetType(String))
                dtYuso.Columns.Add("管理番号", GetType(String))
                dtYuso.Columns.Add("輸送物品", GetType(String))
                dtYuso.Columns.Add("輸送理由", GetType(String))
                dtYuso.Columns.Add("輸送会社等", GetType(String))
                'dtYuso.Columns.Add("請求金額", GetType(String))
                dtYuso.Columns.Add("請求額", GetType(String))

                For i As Integer = 0 To dt.Rows.Count - 1

                    If DateTime.TryParse(dt.Rows(i).Item("輸送日").ToString, dtTime) Then
                        dt.Rows(i).Item("輸送日") = dtTime
                    Else
                        dt.Rows(i).Item("輸送日") = DBNull.Value
                    End If

                    dtYuso.Rows.Add(dt.Rows(i).Item("対象年月"),
                                    dt.Rows(i).Item("輸送日"),
                                    dt.Rows(i).Item("輸送元"),
                                    dt.Rows(i).Item("輸送先"),
                                    dt.Rows(i).Item("ＴＢＯＸ"),
                                    dt.Rows(i).Item("ホール"),
                                    dt.Rows(i).Item("ＴＢＯＸタイプ"),
                                    dt.Rows(i).Item("管理番号"),
                                    dt.Rows(i).Item("輸送物品"),
                                    dt.Rows(i).Item("輸送理由"),
                                    dt.Rows(i).Item("輸送会社等"),
                                    dt.Rows(i).Item("請求額"))
                Next
                '--------------------------------
                '2014/05/01 後藤　ここまで
                '--------------------------------
                'PDF出力処理.
                psPrintPDF(Me, rpt, dtYuso, ipstrReportName)

        End Select

    End Sub
    ''' <summary>
    ''' PDF作成.
    ''' </summary>
    ''' <param name="ipstrReportName"></param>
    ''' <param name="ipstrReportName2"></param>
    ''' <param name="CsvDate"></param>
    ''' <param name="CsvDate2"></param>
    ''' <param name="rpt"></param>
    ''' <param name="rpt2"></param>
    ''' <param name="strArrayList"></param>
    ''' <param name="strArrayList2"></param>
    ''' <remarks></remarks>
    Private Overloads Sub ms_MakePdf(ByVal ipstrReportName As String,
                                     ByVal ipstrReportName2 As String,
                                     ByVal CsvDate As ArrayList,
                                     ByVal CsvDate2 As ArrayList,
                                     ByVal rpt As Object,
                                     ByVal rpt2 As Object,
                                     ByVal strArrayList As String(),
                                     ByVal strArrayList2 As String(),
                                     ByVal intPrint As Integer)

        Dim strCsv() As String
        Dim strCsv2() As String
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim objTbl1 As DataTable = Nothing
        Dim objTbl2 As DataTable = Nothing
        Dim dtPrint As New DataTable
        Dim dtTime As New DateTime

        'データテーブルカラム作成(ヘッダ)
        For i As Integer = 0 To strArrayList.Length - 1
            dt.Columns.Add(strArrayList(i))
        Next

        For i As Integer = 0 To strArrayList2.Length - 1
            dt2.Columns.Add(strArrayList2(i))
        Next

        '行数分ループ
        For i As Integer = 0 To CsvDate.Count - 1

            strCsv = Nothing

            'カンマ毎に分割
            strCsv = CsvDate(i).ToString.Split(",")

            dt.Rows.Add(strCsv)

        Next

        For i As Integer = 0 To CsvDate2.Count - 1.0F

            strCsv2 = Nothing

            'カンマ毎に分割
            strCsv2 = CsvDate2(i).ToString.Split(",")

            dt2.Rows.Add(strCsv2)

        Next

        '--------------------------------
        '2014/05/08 後藤　ここから
        '--------------------------------
        If intPrint = 0 Then

            'For i As Integer = 0 To CsvDate2.Count - 1.0F

            '    strCsv2 = Nothing

            '    'カンマ毎に分割
            '    strCsv2 = CsvDate2(i).ToString.Split(",")

            '    dt2.Rows.Add(strCsv2)

            'Next


            Dim objDs As New DataSet     'データセット
            objDs.Tables.Add("0")

            Dim objDt(1) As DataTable   'データテーブル
            Dim rept(1) As Object        'ActiveReports用クラス
            Dim strRptName(1) As String 'レポート名   

            '開始ログ出力
            psLogStart(Me)

            rept(0) = New CMPREP003
            objDt(0) = dt
            strRptName(0) = "特別保守費用一覧"
            If objDt(0).Rows.Count = 0 Then
                Dim drNew As DataRow = objDt(0).NewRow()
                drNew("対応年月") = Me.txtNendo.ppText
                objDt(0).Rows.Add(drNew)
            Else

                dtPrint.Columns.Add("エリア", GetType(String))
                dtPrint.Columns.Add("対応年月", GetType(DateTime))
                dtPrint.Columns.Add("依頼日時", GetType(DateTime))
                dtPrint.Columns.Add("対応日付", GetType(DateTime))
                dtPrint.Columns.Add("出発時刻", GetType(DateTime))
                dtPrint.Columns.Add("開始時刻", GetType(DateTime))
                dtPrint.Columns.Add("終了時刻", GetType(DateTime))
                dtPrint.Columns.Add("担当SC", GetType(String))
                dtPrint.Columns.Add("保守管理No", GetType(String))
                dtPrint.Columns.Add("TBOXID", GetType(String))
                dtPrint.Columns.Add("ホール名", GetType(String))
                dtPrint.Columns.Add("TBOXタイプ", GetType(String))
                dtPrint.Columns.Add("故障状況対応内容", GetType(String))
                dtPrint.Columns.Add("作業", GetType(String))
                dtPrint.Columns.Add("往復", GetType(String))
                dtPrint.Columns.Add("作業人数", GetType(String))
                dtPrint.Columns.Add("請求額", GetType(String))
                dtPrint.Columns.Add("提出日", GetType(DateTime))

                For i As Integer = 0 To objDt(0).Rows.Count - 1

                    If DateTime.TryParse(objDt(0).Rows(i).Item("対応年月").ToString, dtTime) Then
                        objDt(0).Rows(i).Item("対応年月") = dtTime
                    Else
                        objDt(0).Rows(i).Item("対応年月") = DBNull.Value
                    End If
                    If DateTime.TryParse(objDt(0).Rows(i).Item("依頼日時").ToString, dtTime) Then
                        objDt(0).Rows(i).Item("依頼日時") = dtTime
                    Else
                        objDt(0).Rows(i).Item("依頼日時") = DBNull.Value
                    End If
                    If DateTime.TryParse(objDt(0).Rows(i).Item("対応日付").ToString, dtTime) Then
                        objDt(0).Rows(i).Item("対応日付") = dtTime
                    Else
                        objDt(0).Rows(i).Item("対応日付") = DBNull.Value
                    End If
                    If DateTime.TryParse(objDt(0).Rows(i).Item("出発時刻").ToString, dtTime) Then
                        objDt(0).Rows(i).Item("出発時刻") = dtTime
                    Else
                        objDt(0).Rows(i).Item("出発時刻") = DBNull.Value
                    End If
                    If DateTime.TryParse(objDt(0).Rows(i).Item("開始時刻").ToString, dtTime) Then
                        objDt(0).Rows(i).Item("開始時刻") = dtTime
                    Else
                        objDt(0).Rows(i).Item("開始時刻") = DBNull.Value
                    End If
                    If DateTime.TryParse(objDt(0).Rows(i).Item("終了時刻").ToString, dtTime) Then
                        objDt(0).Rows(i).Item("終了時刻") = dtTime
                    Else
                        objDt(0).Rows(i).Item("終了時刻") = DBNull.Value
                    End If
                    If DateTime.TryParse(objDt(0).Rows(i).Item("提出日").ToString, dtTime) Then
                        objDt(0).Rows(i).Item("提出日") = dtTime
                    Else
                        objDt(0).Rows(i).Item("提出日") = DBNull.Value
                    End If

                    dtPrint.Rows.Add(objDt(0).Rows(i).Item("エリア"),
                                     objDt(0).Rows(i).Item("対応年月"),
                                     objDt(0).Rows(i).Item("依頼日時"),
                                     objDt(0).Rows(i).Item("対応日付"),
                                     objDt(0).Rows(i).Item("出発時刻"),
                                     objDt(0).Rows(i).Item("開始時刻"),
                                     objDt(0).Rows(i).Item("終了時刻"),
                                     objDt(0).Rows(i).Item("担当SC"),
                                     objDt(0).Rows(i).Item("保守管理No"),
                                     objDt(0).Rows(i).Item("TBOXID"),
                                     objDt(0).Rows(i).Item("ホール名"),
                                     objDt(0).Rows(i).Item("TBOXタイプ"),
                                     objDt(0).Rows(i).Item("故障状況対応内容"),
                                     objDt(0).Rows(i).Item("作業"),
                                     objDt(0).Rows(i).Item("往復"),
                                     objDt(0).Rows(i).Item("作業人数"),
                                     objDt(0).Rows(i).Item("請求額"),
                                     objDt(0).Rows(i).Item("提出日"))
                Next

            End If

            objDt(0) = dtPrint


            '(2)特別保守作業集計印刷処理
            'データ取得

            If Not objDs Is Nothing Then
                rept(1) = New CMPREP004
                objDt(1) = dt2
                strRptName(1) = "特別保守作業集計"
            End If


            '帳票出力
            psPrintPDF(Me, rept, objDt, strRptName)

        Else
            'PDF出力.
            psPrintPDF(Me, {rpt, rpt2}, {dt, dt2}, {ipstrReportName, ipstrReportName2})

        End If

        '--------------------------------
        '2014/05/08 後藤　ここまで
        '--------------------------------
    End Sub

#End Region

#Region "締め処理"

    ''' <summary>
    ''' 締め処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClose_Click()

        'ログ出力開始
        psLogStart(Me)

        '------------------------------
        '2014/05/21 後藤 ここから
        '------------------------------
        Select Case Master.Master.ppRigthButton2.Text
            Case "締め"

                '状態チェック.
                If mfChkJotai(1) = False Then
                    Exit Sub
                End If


                '締め処理.
                If mfUpdateClose("1", "0") = False Then
                Else
                    Master.Master.ppRigthButton2.Text = "締め解除"
                    Master.Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton2.Text).Replace(";", "") + "&&dispWait('unclose');" '締め/締め解除確認
                End If

                'Case "解除"
            Case "締め解除"

                '状態チェック.
                If mfChkJotai(2) = False Then
                    Exit Sub
                End If

                '締め解除処理.
                If mfUpdateClose("0", "1") = False Then
                Else
                    Master.Master.ppRigthButton2.Text = "締め"
                    Master.Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton2.Text).Replace(";", "") + "&&dispWait('close');" '締め/締め解除確認
                End If

        End Select
        '------------------------------
        '2014/05/21 後藤 ここまで
        '------------------------------

        'ログ出力終了
        psLogEnd(Me)

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
        Dim strSeikyuYm As String = ""
        Dim strDegreeYmd As String = ""
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '初期化
        conDB = Nothing

        '接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                '検収月を取得
                If Not mfGetKensyuYm(strSeikyuYm, strDegreeYmd) Then
                    Throw New Exception()
                End If

                cmdDB = New SqlCommand("CMPINQP002_S9", conDB)
                'cmdDB.Parameters.Add(pfSet_Param("degree", SqlDbType.NVarChar, Me.txtNendo.ppText.Replace("/", "")))
                cmdDB.Parameters.Add(pfSet_Param("degree", SqlDbType.NVarChar, strSeikyuYm.Replace("/", "")))
                cmdDB.Parameters.Add(pfSet_Param("search_flg", SqlDbType.NVarChar, "0"))
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count > 0 Then
                    intIns = Integer.Parse(dstOrders.Tables(0).Rows(0).Item("検収締め区分"))
                    intReq = Integer.Parse(dstOrders.Tables(0).Rows(0).Item("請求締め区分"))
                Else
                    intIns = 0
                    intReq = 0
                End If

                '--------------------------------
                '2014/04/22 Hamamoto　ここから
                '--------------------------------
                'ボタン活性制御.
                Select Case intIns + intReq
                    Case 0
                        Master.Master.ppRigthButton1.Enabled = True '当月集計

                        Master.Master.ppRigthButton2.Enabled = False
                        If grvList.Rows.Count > 0 Then
                            '検索月の先月のデータを取得
                            cmdDB = New SqlCommand("CMPINQP002_S9", conDB)
                            'cmdDB.Parameters.Add(pfSet_Param("degree", SqlDbType.NVarChar, Date.Parse(Me.txtNendo.ppText).AddMonths(-1).ToString("yyyyMM")))
                            cmdDB.Parameters.Add(pfSet_Param("degree", SqlDbType.NVarChar, Date.Parse(strSeikyuYm).AddMonths(-1).ToString("yyyyMM")))
                            cmdDB.Parameters.Add(pfSet_Param("search_flg", SqlDbType.NVarChar, "0"))
                            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                            '前月の情報が無い、または前月が締め状態であれば締め処理が可能
                            If dstOrders.Tables(0).Rows.Count = 0 Then
                                Master.Master.ppRigthButton2.Enabled = True '締め/解除
                            ElseIf dstOrders.Tables(0).Rows(0).Item("請求締め区分") = "1" Then
                                Master.Master.ppRigthButton2.Enabled = True '締め/解除
                            End If

                        End If
                        Master.Master.ppRigthButton2.Text = "締め"

                    Case 1
                        Master.Master.ppRigthButton1.Enabled = False '当月集計
                        Master.Master.ppRigthButton2.Enabled = True '締め/解除
                        Master.Master.ppRigthButton2.Text = "締め解除"

                    Case 2
                        Master.Master.ppRigthButton1.Enabled = False '当月集計
                        Master.Master.ppRigthButton2.Enabled = False '締め/解除
                        Master.Master.ppRigthButton2.Text = "締め"
                End Select

                'If Me.txtNendo.ppText = "" Then
                If strSeikyuYm = "" Then
                    Master.Master.ppRigthButton1.Enabled = False '当月集計
                    Master.Master.ppRigthButton2.Enabled = False '締め/解除
                End If

                '--------------------------------
                '2014/04/22 Hamamoto　ここまで
                '--------------------------------
            Catch ex As Exception
                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
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
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub
    ''' <summary>
    ''' 状態チェック
    ''' </summary>
    ''' <param name="intMode">1:締め　2:締め解除　3:当月集計　4:印刷 5:CSV</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfChkJotai(ByVal intMode As Integer) As Boolean
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        mfChkJotai = False

        'DB接続
        If clsDataConnect.pfOpen_Database(objCon) Then
            Try
                'ストアド設定
                objCmd = New SqlCommand("CMPINQP002_S19")
                objCmd.Connection = objCon
                objCmd.CommandType = CommandType.StoredProcedure

                'パラメータ設定
                With objCmd.Parameters
                    .Add(pfSet_Param("prm_degree", SqlDbType.NVarChar, Me.txtNendo.ppText.Replace("/", "")))
                    .Add(pfSet_Param("prmINS", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                    .Add(pfSet_Param("prmREQ", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                    .Add(pfSet_Param("prmErr", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                End With

                'SQL実行
                clsDataConnect.pfGet_DataSet(objCmd)

                '---------------------------
                '2014/06/23 稲葉 ここから
                '---------------------------
                'エラー
                If objCmd.Parameters("prmErr").Value.ToString <> "0" Then
                    Throw New Exception()
                End If

                '締めている書類を取得
                Dim strMesPrm As String = "検収書"
                If objCmd.Parameters("prmREQ").Value.ToString = "1" Then
                    strMesPrm = "請求書"
                End If

                Select Case intMode
                    Case 1 '締め
                        If Not (objCmd.Parameters("prmINS").Value.ToString = "0" AndAlso objCmd.Parameters("prmREQ").Value.ToString = "0") Then
                            psMesBox(Me, "30011", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前, strMesPrm & "の締め処理")
                            mfChkJotai = False
                            Exit Function
                        End If
                    Case 2 '締め解除
                        If Not (objCmd.Parameters("prmINS").Value.ToString = "1" AndAlso objCmd.Parameters("prmREQ").Value.ToString = "0") Then
                            If objCmd.Parameters("prmREQ").Value.ToString = "1" Then
                                psMesBox(Me, "30005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前, strMesPrm & "の締め処理", "締め解除")
                            Else
                                psMesBox(Me, "30011", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前, strMesPrm & "の締め解除")
                            End If

                            mfChkJotai = False
                            Exit Function
                        End If
                    Case 3 '当月集計
                        If objCmd.Parameters("prmINS").Value.ToString <> "" And objCmd.Parameters("prmREQ").Value.ToString <> "" Then
                            If Not (objCmd.Parameters("prmINS").Value.ToString = "0" AndAlso objCmd.Parameters("prmREQ").Value.ToString = "0" _
                                    OrElse objCmd.Parameters("prmINS").Value.ToString = "1" AndAlso objCmd.Parameters("prmREQ").Value.ToString = "1") Then
                                psMesBox(Me, "30005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前, strMesPrm & "の締め処理", "当月集計が")
                                mfChkJotai = False
                                Exit Function
                            End If
                        End If
                End Select
                ''エラー
                'If objCmd.Parameters("prmErr").Value <> "0" Then
                '    Throw New Exception()
                'End If

                ''締めている書類を取得
                'Dim strMesPrm As String = "検収書"
                'If objCmd.Parameters("prmREQ").Value = "1" Then
                '    strMesPrm = "請求書"
                'End If

                'Select Case intMode
                '    Case 1 '締め
                '        If Not (objCmd.Parameters("prmINS").Value = "0" AndAlso objCmd.Parameters("prmREQ").Value = "0") Then
                '            psMesBox(Me, "30011", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画前, strMesPrm & "の締め処理")
                '            mfChkJotai = False
                '            Exit Function
                '        End If
                '    Case 2 '締め解除
                '        If Not (objCmd.Parameters("prmINS").Value = "1" AndAlso objCmd.Parameters("prmREQ").Value = "0") Then
                '            If objCmd.Parameters("prmREQ").Value = "1" Then
                '                psMesBox(Me, "30005", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画前, strMesPrm & "の締め処理", "締め解除")
                '            Else
                '                psMesBox(Me, "30011", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画前, strMesPrm & "の締め解除")
                '            End If

                '            mfChkJotai = False
                '            Exit Function
                '        End If
                '    Case 3 '当月集計
                '        If Not (objCmd.Parameters("prmINS").Value = "0" AndAlso objCmd.Parameters("prmREQ").Value = "0" _
                '                OrElse objCmd.Parameters("prmINS").Value = "1" AndAlso objCmd.Parameters("prmREQ").Value = "1") Then
                '            psMesBox(Me, "30005", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画前, strMesPrm & "の締め処理", "当月集計が")
                '            mfChkJotai = False
                '            Exit Function
                '        End If
                'End Select
                '---------------------------
                '2014/06/23 稲葉 ここまで
                '---------------------------

                mfChkJotai = True
                Exit Function

            Catch ex As Exception
                mfChkJotai = False
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCon) Then
                    mfChkJotai = False
                End If
            End Try
        Else
            mfChkJotai = False
        End If

    End Function
    '--------------------------------
    '2014/05/01 後藤　ここまで
    '--------------------------------
    ''' <summary>
    ''' 締め・締め解除処理.
    ''' </summary>
    ''' <param name="ipstrIns_cls_s"></param>
    ''' <param name="ipstrIns_cls_w"></param>
    ''' <remarks></remarks>
    Private Function mfUpdateClose(ByVal ipstrIns_cls_s As String,
                                   ByVal ipstrIns_cls_w As String) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim intRtn As Integer
        Dim strDegreeYm As String = String.Empty
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        mfUpdateClose = False '初期値

        Try

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else

                '-----------------------------
                '2014/05/21 後藤 ここから
                '-----------------------------

                'トランザクション.
                Using conTrn = conDB.BeginTransaction
                    cmdDB = New SqlCommand("CMPINQP002_U1", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = conTrn
                    With cmdDB.Parameters
                        .Add(pfSet_Param("ins_cls_s", SqlDbType.NVarChar, ipstrIns_cls_s))
                        .Add(pfSet_Param("ins_cls_w", SqlDbType.NVarChar, ipstrIns_cls_w))
                        .Add(pfSet_Param("update_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                        .Add(pfSet_Param("degree", SqlDbType.NVarChar, Me.txtNendo.ppText.Replace("/", "")))
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                    End With

                    '実行
                    cmdDB.ExecuteNonQuery()

                    ''年月度取得
                    'strDegreeYm = cmdDB.Parameters("degree").Value

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

                    '成功.
                    mfUpdateClose = True

                    '完了メッセージ.
                    psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, Master.Master.ppRigthButton2.Text)

                    '再検索
                    Call msSeach_Data(Me.txtNendo.ppText)

                    '画面状態更新
                    msSetEnable()

                End Using

                '-----------------------------
                '2014/05/21 後藤 ここまで
                '-----------------------------
            End If
        Catch ex As Exception
            '-----------------------------
            '2014/04/22 Hamamoto ここから
            '-----------------------------
            'psMesBox(Me, "30015", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, Master.ppRigthButton3.Text)
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.Master.ppRigthButton2.Text)
            '-----------------------------
            '2014/04/22 Hamamoto ここまで
            '-----------------------------
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
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

        End Try
    End Function


#End Region

#Region "登録・更新処理"
    ''' <summary>
    ''' 登録・更新処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfUpdate(ByVal strDegree As String,
                              ByVal decCnst_Cnt As Decimal,
                              ByVal decCnst_Price As Decimal,
                              ByVal decMnt_Cnt As Decimal,
                              ByVal decMnt_Price As Decimal,
                              ByVal decPre_Cnt As Decimal,
                              ByVal decPre_Price As Decimal,
                              ByVal decSpc_Cnt As Decimal,
                              ByVal decSpc_Price As Decimal,
                              ByVal decOth_Cnt As Decimal,
                              ByVal decOth_Price As Decimal,
                              ByVal decLan_Cnt As Decimal,
                              ByVal decLan_Price As Decimal) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        mfUpdate = False

        Try

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Exit Function
            Else

                'トランザクション.
                Using conTrn = conDB.BeginTransaction

                    cmdDB = New SqlCommand("CMPINQP002_U2", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = conTrn

                    With cmdDB.Parameters
                        .Add(pfSet_Param("degree", SqlDbType.NVarChar, strDegree))
                        .Add(pfSet_Param("cnst_cnt", SqlDbType.NVarChar, decCnst_Cnt))
                        .Add(pfSet_Param("cnst_price", SqlDbType.NVarChar, decCnst_Price))
                        .Add(pfSet_Param("mnt_cnt", SqlDbType.NVarChar, decMnt_Cnt))
                        .Add(pfSet_Param("mnt_price", SqlDbType.NVarChar, decMnt_Price))
                        .Add(pfSet_Param("pre_cnt", SqlDbType.NVarChar, decPre_Cnt))
                        .Add(pfSet_Param("pre_price", SqlDbType.NVarChar, decPre_Price))
                        .Add(pfSet_Param("spc_cnt", SqlDbType.NVarChar, decSpc_Cnt))
                        .Add(pfSet_Param("spc_price", SqlDbType.NVarChar, decSpc_Price))
                        .Add(pfSet_Param("oth_cnt", SqlDbType.NVarChar, decOth_Cnt))
                        .Add(pfSet_Param("oth_price", SqlDbType.NVarChar, decOth_Price))
                        .Add(pfSet_Param("lan_cnt", SqlDbType.NVarChar, decLan_Cnt))
                        .Add(pfSet_Param("lan_price", SqlDbType.NVarChar, decLan_Price))
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                    End With

                    '実行
                    cmdDB.ExecuteNonQuery()

                    'コミット
                    conTrn.Commit()

                End Using

            End If

            mfUpdate = True

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
            mfUpdate = False
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                mfUpdate = False
            End If

        End Try

    End Function

#End Region

#End Region

    '--------------------------------
    '2014/05/01 後藤　ここから
    '--------------------------------
    'Private Sub mfPDF(cMPINQP002 As CMPINQP002, rpt As CMPREP001, dt As DataTable, ipstrReportName As String)
    ''' <summary>
    ''' 保守料金明細出力（印刷）処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub mfPDF(ByVal CsvDate_ID As ArrayList, ByVal Cnt_ID As Integer, ByVal strArrayList_ID As String(),
                      ByVal CsvDate_IC As ArrayList, ByVal Cnt_IC As Integer, ByVal strArrayList_IC As String(),
                      ByVal CsvDate_LT As ArrayList, ByVal Cnt_LT As Integer, ByVal strArrayList_LT As String(),
                      ByVal CsvDate2 As ArrayList, ByVal Cnt2 As Integer, ByVal strArrayList2 As String(),
                      Optional ByVal blnCanNewPrint As Boolean = True)
        '             ByVal CsvDate3 As ArrayList, ByVal Cnt3 As Integer, ByVal strArrayList3 As String(),
        '             ByVal ipstrReportName As String)

        Dim FsCnt() As Integer = Nothing
        Dim FsPrice() As Integer = Nothing
        Dim CsCnt() As Integer = Nothing
        Dim CsPrice() As Integer = Nothing
        Dim OthCnt() As Integer = Nothing
        Dim OthPrice() As Integer = Nothing
        Dim intTypeCount As Integer = 0
        Dim dt_ID As New DataTable
        Dim dt_IC As New DataTable
        Dim dt_LT As New DataTable
        Dim dtArea As New DataTable
        'Dim dtAreaTotal As New DataTable
        Dim strSeikyuYm As String = String.Empty
        Dim strDegreeYmd As String = String.Empty

        Try
            'csvファイルからデータテーブル生成（IDテーブルをIf文の外で作成せず2回書いているのは見た目の問題だけです）
            If blnCanNewPrint = True Then
                '新帳票印刷時は、システム分類毎にデータテーブルを分割する
                msCreateTable(strArrayList_ID, CsvDate_ID, dt_ID)
                msCreateTable(strArrayList_IC, CsvDate_IC, dt_IC)
                msCreateTable(strArrayList_LT, CsvDate_LT, dt_LT)
            Else
                '旧帳票印刷時は、システム分類を無視してIDテーブルに集計する。
                msCreateTable(strArrayList_ID, CsvDate_ID, dt_ID)
            End If
            msCreateTable(strArrayList2, CsvDate2, dtArea)      'エリア毎の集計値（合計値計算用）
            'msCreateTable(strArrayList3, CsvDate3, dtAreaTotal)

            '開始ログ出力
            psLogStart(Me)

            '検収月を取得
            If Not mfGetKensyuYm(strSeikyuYm, strDegreeYmd) Then
                Throw New Exception()
            End If

            'セッション情報設定
            '集計日（帳票で使用）
            Session("dtbAggDt") = dt_ID.Rows(0).Item("集計日").ToString

            'Dim strFsAreaCnt As String = ""
            'Dim strFsAreaAmount As String = ""
            'Dim strCsAreaCount As String = ""
            'Dim strCsAreaAmount As String = ""
            'Dim strOthAreaCnt As String = ""
            'Dim strOthAreaAmount As String = ""

            'For i = 0 To dtAreaTotal.Rows.Count - 1
            '    Select Case dtAreaTotal.Rows(i).Item("エリアコード")
            '        Case "0"  'ＦＳエリア
            '            strFsAreaCnt = dtAreaTotal.Rows(i).Item("エリア件数")
            '            strFsAreaAmount = dtAreaTotal.Rows(i).Item("エリア金額")

            '        Case "1"  'ＣＳエリア
            '            strCsAreaCount = dtAreaTotal.Rows(i).Item("エリア件数")
            '            strCsAreaAmount = dtAreaTotal.Rows(i).Item("エリア金額")

            '        Case "9" 'その他エリア
            '            strOthAreaCnt = dtAreaTotal.Rows(i).Item("エリア件数")
            '            strOthAreaAmount = dtAreaTotal.Rows(i).Item("エリア金額")

            '    End Select
            'Next
            'If strFsAreaCnt.Trim() = "" Then
            '    strFsAreaCnt = "0"
            'End If
            'If strFsAreaAmount.Trim() = "" Then
            '    strFsAreaAmount = "0"
            'End If
            'If strCsAreaCount.Trim() = "" Then
            '    strCsAreaCount = "0"
            'End If
            'If strCsAreaAmount.Trim() = "" Then
            '    strCsAreaAmount = "0"
            'End If
            'If strOthAreaCnt.Trim() = "" Then
            '    strOthAreaCnt = "0"
            'End If
            'If strOthAreaAmount.Trim() = "" Then
            '    strOthAreaAmount = "0"
            'End If
            ''--エリア別件数
            'Session("FsTtlCnt") = strFsAreaCnt  'ＦＳ件数
            'Session("CsTtlCnt") = strCsAreaCount  'ＣＳ件数
            'Session("OthTtlCnt") = strOthAreaCnt 'その他件数

            '--エリア・TBOXタイプ別件数、金額
            '全TBOXタイプの種類数総計
            intTypeCount = (dtArea.Columns.Count - 2) / 2

            'If intTypeCount > 0 Then
            '    intTypeCount = intTypeCount - 2
            'End If
            '各エリアの集計件数、金額をシステム毎に配列に代入（実際に使用しているのはその他エリアのみ）
            ReDim FsCnt(intTypeCount - 1)
            ReDim FsPrice(intTypeCount - 1)
            ReDim CsCnt(intTypeCount - 1)
            ReDim CsPrice(intTypeCount - 1)
            ReDim OthCnt(intTypeCount - 1)
            ReDim OthPrice(intTypeCount - 1)

            With dtArea
                For i = 0 To .Rows.Count - 1
                    For j = 0 To intTypeCount - 1
                        'ＦＳエリア
                        If .Rows(i).Item(0) = "0" Then
                            FsCnt(j) = 0
                            FsPrice(j) = 0
                        End If
                        'ＣＳエリア
                        If .Rows(i).Item(0) = "1" Then
                            CsCnt(j) = 0
                            CsPrice(j) = 0
                        End If
                        'その他エリア
                        If Decimal.Parse(.Rows(i).Item(0)) < 0 Then
                            OthCnt(j) = .Rows(i).Item(j * 2 + 2)
                            OthPrice(j) = .Rows(i).Item(j * 2 + 3)
                        End If
                    Next
                Next
            End With

            '帳票側の処理で使用
            Session("FsCnt") = FsCnt
            Session("FsPrice") = FsPrice
            Session("CsCnt") = CsCnt
            Session("CsPrice") = CsPrice
            Session("OthCnt") = OthCnt
            Session("OthPrice") = OthPrice

            '各データテーブルの余分な列を削除（「帳票出力日」「発行元名」「集計日」）
            dt_ID.Columns.RemoveAt(0)
            dt_ID.Columns.RemoveAt(0)
            dt_ID.Columns.RemoveAt(0)
            If blnCanNewPrint = True Then
                dt_IC.Columns.RemoveAt(0)
                dt_IC.Columns.RemoveAt(0)
                dt_IC.Columns.RemoveAt(0)
                dt_LT.Columns.RemoveAt(0)
                dt_LT.Columns.RemoveAt(0)
                dt_LT.Columns.RemoveAt(0)
            End If

            'システム分類毎のTBOXﾀｲﾌﾟ種類数（帳票側の処理で使用）
            '合計の件数はintTypeCount　旧帳票の場合はSession("IDTypeCnt")も同数となる
            Session("IDTypeCnt") = (dt_ID.Columns.Count - 1) / 2
            If blnCanNewPrint = True Then
                Session("ICTypeCnt") = (dt_IC.Columns.Count - 1) / 2
                Session("LTTypeCnt") = (dt_LT.Columns.Count - 1) / 2
            End If

            '帳票出力
            Dim rptID As New CMPREP001
            If blnCanNewPrint = True Then
                Dim rptIC As New CMPREP001
                Dim rptLT As New CMPREP001

                '帳票のタイトル設定
                rptID.ppTitle = "保守料金明細作成一覧表　ID"
                rptIC.ppTitle = "保守料金明細作成一覧表　IC"
                rptLT.ppTitle = "保守料金明細作成一覧表　LUTERNA"

                '帳票出力
                psPrintPDF(Me, {rptID, rptIC, rptLT}, {dt_ID, dt_IC, dt_LT} _
                           , {Master.Master.ppTitle + "一覧表　ID", Master.Master.ppTitle + "一覧表　IC", Master.Master.ppTitle + "一覧表　LUTERNA"})
            Else
                rptID.ppTitle = "保守料金明細作成一覧表"
                psPrintPDF(Me, rptID, dt_ID, Master.Master.ppTitle + "一覧表")
            End If

            'rpt = New CMPREP001
            'rpt.ppTitle = ipstrReportName
            ''For rowint As Integer = 0 To objDt1.Rows.Count - 1
            ''    For colint As Integer = 0 To objDt1.Columns.Count - 1
            ''    Next
            ''Next
            ''psPrintPDF(Me, rpt, objDt1, Master.Master.ppTitle + "一覧表")
            'psPrintPDF(Me, rpt, objDt_ID, ipstrReportName)
        Catch ex As Exception
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守料金明細")
        End Try

    End Sub
    ''' <summary>
    ''' データテーブル作成
    ''' </summary>
    ''' <param name="strArrayList"></param>
    ''' <param name="CsvDate"></param>
    ''' <param name="dt"></param>
    ''' <remarks></remarks>
    Private Sub msCreateTable(ByVal strArrayList() As String,
                                  ByVal CsvDate As ArrayList,
                                  ByRef dt As DataTable)

        Dim strCsv() As String

        For i As Integer = 0 To strArrayList.Length - 1
            dt.Columns.Add(strArrayList(i))
        Next

        '行数分ループ
        For i As Integer = 0 To CsvDate.Count - 1

            strCsv = Nothing

            'カンマ毎に分割
            strCsv = CsvDate(i).ToString.Split(",")

            dt.Rows.Add(strCsv)

        Next
    End Sub
    '--------------------------------
    '2014/05/01 後藤　ここまで
    '--------------------------------

    'DOCMENP001-005
    ''' <summary>
    ''' 会社名取得
    ''' </summary>
    ''' <param name="strKey"></param>
    ''' <returns></returns>
    ''' <remarks>帳票「情報機器保守検収書」作成時に、
    ''' ヘッダフッタ項目の取得で使用する</remarks>
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
                'ログ出力"
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
    'DOCMENP001-005

    ''' <summary>
    ''' 指定したシステム分類の列を抽出
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <param name="intSyscls"></param>
    ''' <remarks>指定したシステム分類以外の列をテーブルから削除</remarks>
    Private Function mfGetSelectColumn(ByVal dt As DataTable, ByVal intSyscls As Integer) As DataTable
        mfGetSelectColumn = Nothing
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim dtRtn As New DataTable               '戻り値用
        Dim dtSysCls As New DataTable

        '戻り値用テーブル作成
        dtRtn = dt.Copy

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                'システム名とシステム区分の取得
                objCmd = New SqlCommand("CMPINQP002_S26", objCn)
                dtSysCls = clsDataConnect.pfGet_DataSet(objCmd).Tables(0)                            '全システムの取得
                dtSysCls = dtSysCls.Select("M23_SYSTEM_CD <>" & intSyscls.ToString).CopyToDataTable  '対象外システム区分の抽出

                For Each dtc As DataColumn In dt.Columns
                    For Each dtrSys As DataRow In dtSysCls.Rows
                        '対象外システム分類のシステムの列（「○○_件数」「○○_金額」）を削除
                        If dtc.ColumnName = dtrSys.Item("M03_TBOX_NM") & "_件数" Then
                            dtRtn.Columns.Remove(dtc.ToString)
                        End If
                        If dtc.ColumnName = dtrSys.Item("M03_TBOX_NM") & "_金額" Then
                            dtRtn.Columns.Remove(dtc.ToString)
                        End If
                    Next
                Next

                mfGetSelectColumn = dtRtn

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "システム区分の取得")
                'ログ出力"
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                mfGetSelectColumn = Nothing

            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Function

End Class
