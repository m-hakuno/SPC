'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　品質会議資料
'*　ＰＧＭＩＤ：　QUAOUTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.15　：　酒井
'********************************************************************************************************************************
'★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★
'   酒井さんへ メモ(2014/03/26)
'     中村さんより「ボタン機能を有効にし、一覧表示ができるように修正してほしい」との依頼を受け
'     ボタン制御の箇所、および、検索処理の箇所を修正しました(「Hamamoto Edit」の箇所です。)。
'     酒井さんのソースはコメントアウトして残しています。
'     ストアド、一覧用XMLについても同様です。
'★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================

Imports System.Text
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax
Imports Microsoft.Office.Interop

#End Region

Public Class QUAOUTP001
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

    'エラーコード
    Const sCnsErr_0001 As String = "0001"
    Const sCnsErr_0002 As String = "0002"
    Const sCnsErr_0003 As String = "0003"
    Const sCnsErr_0004 As String = "0004"
    Const sCnsErr_0005 As String = "0005"

    Const sCnsErr_00004 As String = "00004"
    Const sCnsErr_00005 As String = "00005"
    Const sCnsErr_00006 As String = "00006"
    Const sCnsErr_10001 As String = "10001"
    Const sCnsErr_10002 As String = "10002"
    Const sCnsErr_30008 As String = "30008"

    '品質会議資料 画面のパス
    Const sCnsQUAOUTP001 As String = "~/" & P_CNS & "/" &
                                        P_FUN_QUA & P_SCR_OUT & P_PAGE & "001.aspx"

    Const sCnsProgid1 As String = "QUAOUTP001_1"
    Const sCnsSqlid11 As String = "QUAOUTP001_S11"

    Const sCnsProgid As String = "QUAOUTP001"
    Const sCnsSqlid1 As String = "QUAOUTP001_S1"
    Const sCnsSqlid2 As String = "QUAOUTP001_S2"
    Const sCnsCsvButon As String = "ＣＳＶ"
    '-----------------------------
    '2014/04/25 土岐　ここから
    '-----------------------------
    'Const sCnsPdfButon As String = "ＰＤＦ"
    '帳票用タイトル年月キー
    Const sCnsTitleDate As String = "タイトル年月"
    '-----------------------------
    '2014/04/25 土岐　ここまで
    '-----------------------------

    ''CSVファイル出力先ディレクトリ
    'Const M_CSV_OUTPUT_DIR = "C:\Users\NGC_USER02\Desktop"

#End Region

#Region "構造体・列挙体定義"
    '============================================================================================================================
    '=　構造体・列挙体定義
    '============================================================================================================================
    '★2014/03/26 Hamamoto Edit Start
    Private Enum E_QUAOUTP001_Col
        部位 = 0
        目標値
        数量
        '月１
        '月２
        '月３
        '月４
        '月５
        '月６
        '合計
        YYYYMM1
        YYYYMM2
        YYYYMM3
        YYYYMM4
        YYYYMM5
        YYYYMM6
        YYYYMMT
        CD
    End Enum
    '★2014/03/26 Hamamoto Edit   End
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

#Region "ページ初期処理"
    ' ''' <summary>
    ' ''' ページ初期処理
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    '    ''表設定
    '    'pfSet_GridView(grvList, sCnsProgid1)
    '    'pfSet_GridView(grvList1, sCnsProgid1)

    '    ''表設定
    '    'pfSet_GridView(grvListControl, sCnsProgid)
    '    'pfSet_GridView(grvListParts, sCnsProgid)

    'End Sub

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

            'ボタンアクションの設定
            AddHandler Master.ppRigthButton1.Click, AddressOf Button_Click              '検索
            AddHandler Master.ppRigthButton2.Click, AddressOf Button_Click              'クリア
            AddHandler Master.Master.ppRigthButton1.Click, AddressOf Button_Click       'ＰＤＦ
            AddHandler Master.Master.ppRigthButton2.Click, AddressOf Button_Click       'ＣＳＶ

            'AddHandler Master.Master.ppRigthButton9.Click, AddressOf Button_Click  'ＣＳＶ
            'AddHandler Master.Master.ppRigthButton10.Click, AddressOf Button_Click  'ＰＤＦ

            If Not IsPostBack Then      '初回表示
                'ボタンの設定
                '-----------------------------
                '2014/04/25 土岐　ここから
                '-----------------------------
                Master.Master.ppRigthButton1.Text = P_BTN_NM_PRI
                '-----------------------------
                '2014/04/25 土岐　ここまで
                '-----------------------------
                Master.Master.ppRigthButton1.Visible = True
                Master.Master.ppRigthButton1.Enabled = False
                Master.Master.ppRigthButton2.Text = sCnsCsvButon
                Master.Master.ppRigthButton2.Visible = True
                Master.Master.ppRigthButton2.Enabled = False

                'ボタン押下時の検証を無効
                Master.ppRigthButton2.CausesValidation = False
                Master.Master.ppRigthButton1.CausesValidation = False
                Master.Master.ppRigthButton2.CausesValidation = False

                '-----------------------------
                '2014/04/25 土岐　ここから
                '-----------------------------
                Master.ppCount_Visible = False
                '-----------------------------
                '2014/04/25 土岐　ここまで
                '-----------------------------

                ''ボタン表示設定
                'Master.Master.ppRigthButton9.Text = sCnsCsvButon
                'Master.Master.ppRigthButton10.Text = sCnsPdfButon
                ''ボタン非活性
                'Master.Master.ppRigthButton9.Visible = False
                'Master.Master.ppRigthButton10.Visible = False

                'プログラムＩＤ、画面名設定
                Master.Master.ppProgramID = sCnsProgid
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(sCnsProgid)

                'パンくずリスト設定
                Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                'セッション変数「グループナンバー」が存在しない場合、画面を閉じる
                If Session(P_SESSION_GROUP_NUM) Is Nothing Then
                    psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    psClose_Window(Me)
                    Return
                End If

                'ViewStateに「グループナンバー」を保存
                ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                'ボタン押下時のメッセージ設定
                Master.Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes(sCnsErr_10002, ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "品質会議資料")
                Master.Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes(sCnsErr_10001, ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "品質会議資料")

                '画面の初期化
                msClear_Screen()

            Else
                'ViewState「グループナンバー」が存在しない場合、画面を閉じる
                If ViewState(P_SESSION_GROUP_NUM) Is Nothing Then
                    psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    psClose_Window(Me)
                    Return
                End If

            End If

        Catch ex As Exception
            '画面初期化エラー
            psMesBox(Me, sCnsErr_30008, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
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

    '---------------------------
    '2014/04/16 武 ここから
    '---------------------------
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
    '---------------------------
    '2014/04/16 武 ここまで
    '---------------------------

#Region "ボタン押下処理"

    ''' <summary>
    ''' ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Button_Click(sender As System.Object, e As System.EventArgs)

        'Dim rptETCREP020 As New ETCREP020
        'Dim dtRowDataCo As DataTable = Nothing
        'Dim dtRowDataPa As DataTable = Nothing
        'dtRowDataCo = pfParse_DataTable(grvListControl)
        'dtRowDataPa = pfParse_DataTable(grvListParts)

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        ''★2014/03/26 Hamamoto Edit Start
        'Try
        Select Case sender.ID
            Case "btnSearchRigth1"        '検索ボタン押下
                '対応年月必須チェック
                msCheck_Error()

                If (Page.IsValid) Then
                    ''★2014/03/26 Hamamoto Edit Start
                    ''From必須チェック
                    'If Me.dftSupportDt.ppFromText.Trim() = "" Then
                    '    Me.dftSupportDt.psSet_ErrorNo(5001, "開始対応年月")
                    '    Me.dftSupportDt.ppDateBoxFrom.Focus()
                    '    Return
                    'End If
                    ''★2014/03/26 Hamamoto Edit   End

                    ''ToがFromから6ヶ月以内かどうかチェック
                    'If Me.dftStartDt.ppToText <> "" Then        'Toが空白でない
                    '    If Me.dftStartDt.ppFromDate.AddMonths(6) <= Me.dftStartDt.ppToDate Then
                    '        Me.dftStartDt.psSet_ErrorNo(6002, "終了対応年月", "開始対応年月の先6ヶ月以内")
                    '        Me.dftStartDt.ppDateBoxTo.Focus()
                    '        Return
                    '    End If
                    'Else
                    '    If Date.Parse("9999/12").AddMonths(-5) < dftStartDt.ppFromDate Then
                    '        Me.dftStartDt.ppToText = "9999/12"
                    '    End If
                    'End If

                    ''種別選択必須チェック
                    'If Me.ddlClass.ppSelectedValue = String.Empty Then
                    '    Me.ddlClass.psSet_ErrorNo(5003, "種別")
                    '    Me.ddlClass.ppDropDownList.Focus()
                    '    Return
                    'End If

                    '条件検索取得
                    msGet_Data()
                End If
            Case "btnSearchRigth2"        '検索クリアボタン押下
                '--------------------------------
                '2014/04/16 星野　ここから
                '--------------------------------
                '■□■□結合試験時のみ使用予定□■□■
                Dim objStack As New StackFrame
                Dim strPrm As String = ""
                strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
                Dim tmp As Object() = Session(P_KEY)
                If Not tmp Is Nothing Then
                    For zz = 0 To tmp.Length - 1
                        If zz <> tmp.Length - 1 Then
                            strPrm &= tmp(zz).ToString & ","
                        Else
                            strPrm &= tmp(zz).ToString
                        End If
                    Next
                End If

                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, Request.Url.ToString, strPrm, "TRANS")
                '■□■□結合試験時のみ使用予定□■□■
                '--------------------------------
                '2014/04/16 星野　ここまで
                '--------------------------------

                msClear_Screen()

                ''再表示
                'Response.Redirect(Request.Url.ToString)
            Case "btnRigth2"        'ＣＳＶボタン押下
                'ＣＳＶ作成処理
                msCsv()
                '-----------------------------
                '2014/04/25 土岐　ここから
                '-----------------------------
                'Case "btnRigth1"        'ＰＤＦボタン押下
                '    Dim strServerAddress As String = Nothing        'サーバアドレス
                '    Dim strFolderNM As String = Nothing             '保管フォルダ
                '    Dim datCreateDate As DateTime = Nothing         '日付
                '    Dim strFileNm As String = Nothing               'ファイル名
                '    Dim intRtn As Integer                           '戻り値
                '    Dim dttData As DataTable = Nothing              'データテーブル
                '    Dim rptETCREP020 As New ETCREP020

                '    '-----------------------------
                '    '2014/04/24 土岐　ここから
                '    '-----------------------------
                '    'ＰＤＦデータ取得処理
                '    dttData = mfGetPrintDate()

                '    'ファイル出力
                '    intRtn = pfPDF("0791PN" _
                '                 , "ホール機器品質管理表" _
                '                 , Nothing _
                '                 , rptETCREP020 _
                '                 , dttData _
                '                 , strServerAddress _
                '                 , strFolderNM _
                '                 , datCreateDate _
                '                 , strFileNm)

                '    'ファイル出力エラー
                '    If intRtn <> 0 Then
                '        psMesBox(Me, sCnsErr_10002, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "ホール機器品質管理表")
                '        Exit Sub
                '    End If

                '    'ダウンロードファイル（T07)にレコードを追加
                '    intRtn = pfSetDwnldFile(Nothing _
                '                          , "0791PN" _
                '                          , "確認中(2014/04/24)" _
                '                          , strFileNm _
                '                          , "ホール機器品質管理表" _
                '                          , strServerAddress _
                '                          , strFolderNM _
                '                          , datCreateDate _
                '                          , User.Identity.Name)

                '    'ダウンロードファイル（T07)にレコードを追加エラー
                '    If intRtn <> 0 Then
                '        psMesBox(Me, sCnsErr_10002, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "ホール機器品質管理表")
                '        Exit Sub
                '    End If
                '    '-----------------------------
                '    '2014/04/24 土岐　ここまで
                '    '-----------------------------

                '    'TODO　ＰＤＦ 未処理
                '    'レポートクラス(ETCREP020)


                '    '2014/03/28 GOTO Start

                '    ''PDF出力処理.
                '    'psPrintPDF(Me, rptETCREP020, dttData, "ホール機器品質管理表")

                '    '2014/03/28 GOTO End
            Case "btnRigth1"        'ＰＤＦボタン押下
                'ログ出力開始
                psLogStart(Me)

                Try
                    psPrintPDF(Me,
                               New ETCREP020,
                               mfGetPrintDate(),
                               "ホール機器品質管理表")
                Catch ex As Exception
                    psMesBox(Me, sCnsErr_10001, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ホール機器品質管理表")
                End Try

                'ログ出力終了
                psLogEnd(Me)
                '-----------------------------
                '2014/04/25 土岐　ここまで
                '-----------------------------

        End Select

        'Catch ex As Exception

        '    psMesBox(Me, sCnsErr_0003, clscomver.E_Mタイプ.エラー, clscomver.E_Mモード.OK, clscomver.E_S実行.描画後)
        '    '--------------------------------
        '    '2014/04/14 星野　ここから
        '    '--------------------------------
        '    'ログ出力
        '    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
        '                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
        '    '--------------------------------
        '    '2014/04/14 星野　ここまで
        '    '--------------------------------

        'End Try

        'Try
        '    Select Case sender.Name
        '        Case "ppRigthButton1"        '検索ボタン押下
        '            If (Page.IsValid) Then
        '                '条件検索取得
        '                msGet_Data()
        '            End If
        '        Case "ppRigthButton2"        '検索クリアボタン押下
        '            '再表示
        '            Response.Redirect(Request.Url.ToString)
        '        Case "ppRigthButton9"        'ＣＳＶボタン押下
        '            'ＣＳＶ作成処理
        '            msCsv()
        '        Case "ppRigthButton10"        'ＰＤＦボタン押下
        '            'TODO　ＰＤＦ 未処理
        '    End Select

        'Catch ex As Exception

        '    psMesBox(Me, sCnsErr_0003, clscomver.E_Mタイプ.エラー, clscomver.E_Mモード.OK, clscomver.E_S実行.描画後)
        ''--------------------------------
        ''2014/04/14 星野　ここから
        ''--------------------------------
        ''ログ出力
        'psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
        '                objStack.GetMethod.Name, "", ex.ToString, "Catch")
        ''--------------------------------
        ''2014/04/14 星野　ここまで
        ''--------------------------------

        'End Try

        '★2014/03/26 Hamamoto Edit   End

    End Sub

#End Region

#Region "ヘッダ年月 設定処理"

    ' ''' <summary>
    ' ''' ヘッダ年月 設定処理
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Sub productsGridViewCo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grvListControl.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        ' ヘッダの年月 に設定
    '        Me.grvListControl.HeaderRow.Cells(3).Text = Me.dftStartDt.ppFromDate.Month.ToString + "月"
    '        Me.grvListControl.HeaderRow.Cells(4).Text = Me.dftStartDt.ppFromDate.AddMonths(1).Month.ToString + "月"
    '        Me.grvListControl.HeaderRow.Cells(5).Text = Me.dftStartDt.ppFromDate.AddMonths(2).Month.ToString + "月"
    '        Me.grvListControl.HeaderRow.Cells(6).Text = Me.dftStartDt.ppFromDate.AddMonths(3).Month.ToString + "月"
    '        Me.grvListControl.HeaderRow.Cells(7).Text = Me.dftStartDt.ppFromDate.AddMonths(4).Month.ToString + "月"
    '        Me.grvListControl.HeaderRow.Cells(8).Text = Me.dftStartDt.ppFromDate.AddMonths(5).Month.ToString + "月"

    '    End If

    'End Sub
    'Sub productsGridViewPa_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grvListParts.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        ' ヘッダの年月 に設定
    '        Me.grvListParts.HeaderRow.Cells(3).Text = Me.dftStartDt.ppFromDate.Month.ToString + "月"
    '        Me.grvListParts.HeaderRow.Cells(4).Text = Me.dftStartDt.ppFromDate.AddMonths(1).Month.ToString + "月"
    '        Me.grvListParts.HeaderRow.Cells(5).Text = Me.dftStartDt.ppFromDate.AddMonths(2).Month.ToString + "月"
    '        Me.grvListParts.HeaderRow.Cells(6).Text = Me.dftStartDt.ppFromDate.AddMonths(3).Month.ToString + "月"
    '        Me.grvListParts.HeaderRow.Cells(7).Text = Me.dftStartDt.ppFromDate.AddMonths(4).Month.ToString + "月"
    '        Me.grvListParts.HeaderRow.Cells(8).Text = Me.dftStartDt.ppFromDate.AddMonths(5).Month.ToString + "月"

    '    End If

    'End Sub

    'Sub productsGridView_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grvList.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        ' ヘッダの年月 に設定
    '        grvList.Columns.Item(3).HeaderText = Date.Now.AddMonths(-5).ToString("yyyy/MM")
    '        grvList.Columns.Item(4).HeaderText = Date.Now.AddMonths(-4).ToString("yyyy/MM")
    '        grvList.Columns.Item(5).HeaderText = Date.Now.AddMonths(-3).ToString("yyyy/MM")
    '        grvList.Columns.Item(6).HeaderText = Date.Now.AddMonths(-2).ToString("yyyy/MM")
    '        grvList.Columns.Item(7).HeaderText = Date.Now.AddMonths(-1).ToString("yyyy/MM")
    '        grvList.Columns.Item(8).HeaderText = Date.Now.ToString("yyyy/MM")
    '        grvList.Columns.Item(9).HeaderText = "合計"

    '    End If

    'End Sub
    'Sub productsGridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grvList1.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        ' ヘッダの年月 に設定
    '        grvList1.Columns.Item(3).HeaderText = Date.Now.AddMonths(-5).ToString("yyyy/MM")
    '        grvList1.Columns.Item(4).HeaderText = Date.Now.AddMonths(-4).ToString("yyyy/MM")
    '        grvList1.Columns.Item(5).HeaderText = Date.Now.AddMonths(-3).ToString("yyyy/MM")
    '        grvList1.Columns.Item(6).HeaderText = Date.Now.AddMonths(-2).ToString("yyyy/MM")
    '        grvList1.Columns.Item(7).HeaderText = Date.Now.AddMonths(-1).ToString("yyyy/MM")
    '        grvList1.Columns.Item(8).HeaderText = Date.Now.ToString("yyyy/MM")
    '        grvList1.Columns.Item(9).HeaderText = "合計"

    '    End If

    'End Sub

#End Region

#End Region

#Region "そのほかのプロシージャ"

#Region "画面クリア処理"

    ''' <summary>
    ''' 画面クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClear_Screen()
        Dim intMONTHCount As Integer = 6        '検索条件月数

        '検索項目初期化
        Me.dftStartDt.ppFromText = String.Empty
        Me.dftStartDt.ppToText = String.Empty
        Me.ddlClass.ppSelectedValue = String.Empty

        Me.lblClass2.Text = String.Empty

        '明細部初期化
        For zz As Integer = 1 To intMONTHCount - 1
            Me.grvListControl.Columns(3 + zz).Visible = True
            Me.grvListParts.Columns(3 + zz).Visible = True
        Next
        Me.grvListControl.DataSource = New DataTable
        Me.grvListParts.DataSource = New DataTable
        Me.grvListControl.DataBind()
        Me.grvListParts.DataBind()

        'ＣＳＶ、ＰＤＦボタン非活性
        Master.Master.ppRigthButton1.Enabled = False
        Master.Master.ppRigthButton2.Enabled = False

        Me.dftStartDt.ppDateBoxFrom.Focus()

    End Sub

#End Region

#Region "条件検索取得処理"

    ''' <summary>
    ''' 条件検索取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data()
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders1 As New DataSet
        Dim dstOrders2 As New DataSet
        Const intMONTHCount As Integer = 6        '検索条件月数

        'グリッドの初期化
        For zz As Integer = 1 To intMONTHCount - 1
            Me.grvListControl.Columns(3 + zz).Visible = True
            Me.grvListParts.Columns(3 + zz).Visible = True
        Next
        Me.grvListControl.DataSource = New DataTable
        Me.grvListParts.DataSource = New DataTable
        Me.grvListControl.DataBind()
        Me.grvListParts.DataBind()

        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(sCnsSqlid1, conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    '対応年月From
                    .Add(pfSet_Param("start_dt_f", SqlDbType.NVarChar, Me.dftStartDt.ppFromText))
                    '対応年月To
                    .Add(pfSet_Param("start_dt_t", SqlDbType.NVarChar, Me.dftStartDt.ppToText))
                    '種別
                    .Add(pfSet_Param("group_cls", SqlDbType.NVarChar, Me.ddlClass.ppSelectedValue))
                End With

                'データ取得
                dstOrders1 = clsDataConnect.pfGet_DataSet(cmdDB)

                '故障率算出
                If dstOrders1 IsNot Nothing Then
                    msSet_GrvList(dstOrders1.Tables(0), New DataTable, 0)
                End If

                'データ有無確認
                If dstOrders1.Tables(0).Rows.Count = 0 Then
                    '0件
                    psMesBox(Me, sCnsErr_00004, ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                End If

                '取得したデータをグリッドに設定
                Me.grvListControl.DataSource = dstOrders1.Tables(0)

                '変更を反映
                Me.grvListControl.DataBind()


                ' ヘッダの年月設定
                For zz As Integer = 0 To intMONTHCount - 1
                    '-----------------------------
                    '2014/05/02 土岐　ここから
                    '-----------------------------
                    If Me.dftStartDt.ppFromText = String.Empty Then
                        Me.grvListControl.HeaderRow.Cells(3 + zz).Text =
                            Me.dftStartDt.ppToDate.AddMonths(-(intMONTHCount - 1 - zz)).Month.ToString + "月"
                    Else
                        Me.grvListControl.HeaderRow.Cells(3 + zz).Text = Me.dftStartDt.ppFromDate.AddMonths(zz).Month.ToString + "月"
                    End If
                    '-----------------------------
                    '2014/05/02 土岐　ここまで
                    '-----------------------------
                Next

                Try
                    cmdDB = New SqlCommand(sCnsSqlid2, conDB)
                    'パラメータ設定
                    With cmdDB.Parameters
                        '対応年月From
                        .Add(pfSet_Param("start_dt_f", SqlDbType.NVarChar, Me.dftStartDt.ppFromText))
                        '対応年月To
                        .Add(pfSet_Param("start_dt_t", SqlDbType.NVarChar, Me.dftStartDt.ppToText))
                        '種別
                        .Add(pfSet_Param("group_cls", SqlDbType.NVarChar, Me.ddlClass.ppSelectedValue))
                    End With

                    'データ取得
                    dstOrders2 = clsDataConnect.pfGet_DataSet(cmdDB)

                    '故障率算出
                    If dstOrders2 IsNot Nothing Then
                        msSet_GrvList(dstOrders1.Tables(0), dstOrders2.Tables(0), 1)
                    End If

                    'データ有無確認
                    If dstOrders2.Tables(0).Rows.Count = 0 Then
                        '0件
                        psMesBox(Me, sCnsErr_00004, ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    End If

                    '取得したデータをグリッドに設定
                    Me.grvListParts.DataSource = dstOrders2.Tables(0)

                    '変更を反映
                    Me.grvListParts.DataBind()

                    ' ヘッダの年月設定
                    For zz As Integer = 0 To intMONTHCount - 1
                        '-----------------------------
                        '2014/05/02 土岐　ここから
                        '-----------------------------
                        If Me.dftStartDt.ppFromText = String.Empty Then
                            Me.grvListParts.HeaderRow.Cells(3 + zz).Text =
                                Me.dftStartDt.ppToDate.AddMonths(-(intMONTHCount - 1 - zz)).Month.ToString + "月"
                        Else
                            Me.grvListParts.HeaderRow.Cells(3 + zz).Text = Me.dftStartDt.ppFromDate.AddMonths(zz).Month.ToString + "月"
                        End If
                        '-----------------------------
                        '2014/05/02 土岐　ここまで
                        '-----------------------------
                    Next

                    'グリッドのセルの結合
                    msComb_GrvList(dstOrders2.Tables(0), 1)

                    '列の非表示
                    For zz As Integer = 0 To intMONTHCount - 1
                        If Me.dftStartDt.ppFromDate.AddMonths(zz) = Me.dftStartDt.ppToDate Then
                            For yy As Integer = 1 + zz To intMONTHCount - 1
                                Me.grvListParts.Columns(3 + yy).Visible = False
                            Next
                        End If
                    Next

                    'グリッド非表示列をデータテーブルから削除
                    msDel_Column(Me.dftStartDt.ppFromDate, Me.dftStartDt.ppToDate, dstOrders2)

                    'ビューに退避
                    Dim addedDataSet2 As DataSet = dstOrders2
                    ViewState("ds2") = addedDataSet2

                Catch ex As Exception
                    'データ取得エラー
                    psMesBox(Me, sCnsErr_00004, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "品質会議資料(プリンタ/ディスプレイ/操作盤)")
                    '--------------------------------
                    '2014/04/14 星野　ここから
                    '--------------------------------
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    '--------------------------------
                    '2014/04/14 星野　ここまで
                    '--------------------------------
                    For zz As Integer = 1 To intMONTHCount - 1
                        Me.grvListControl.Columns(3 + zz).Visible = True
                        Me.grvListParts.Columns(3 + zz).Visible = True
                    Next
                    Me.grvListControl.DataSource = New DataTable
                    Me.grvListControl.DataBind()
                    Me.grvListParts.DataSource = New DataTable
                    Me.grvListParts.DataBind()

                    Exit Sub
                End Try

                '種別名設定
                Select Case dstOrders1.Tables(0).Rows(0).Item("システム区分")
                    Case "3"
                        Me.lblClass2.Text = dstOrders1.Tables(0).Rows(0).Item("システム名称")
                    Case "5"
                        Me.lblClass2.Text = dstOrders1.Tables(0).Rows(0).Item("システム区分名称")
                End Select

                'グリッドのセルの結合
                msComb_GrvList(dstOrders1.Tables(0), 0)

                '列の非表示
                For zz As Integer = 0 To intMONTHCount - 1
                    If Me.dftStartDt.ppFromDate.AddMonths(zz) = Me.dftStartDt.ppToDate Then
                        For yy As Integer = 1 + zz To intMONTHCount - 1
                            Me.grvListControl.Columns(3 + yy).Visible = False
                        Next
                    End If
                Next

                'グリッド非表示列をデータテーブルから削除
                msDel_Column(Me.dftStartDt.ppFromDate, Me.dftStartDt.ppToDate, dstOrders1)

                'ビューに退避
                Dim addedDataSet1 As DataSet = dstOrders1
                ViewState("ds1") = addedDataSet1

                'ＣＳＶ、ＰＤＦボタンを活性化
                Master.Master.ppRigthButton1.Enabled = True
                Master.Master.ppRigthButton2.Enabled = True

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, sCnsErr_00004, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "品質会議資料(制御部)")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                For zz As Integer = 1 To intMONTHCount - 1
                    Me.grvListControl.Columns(3 + zz).Visible = True
                    Me.grvListParts.Columns(3 + zz).Visible = True
                Next
                Me.grvListControl.DataSource = New DataTable
                Me.grvListControl.DataBind()
                Me.grvListParts.DataSource = New DataTable
                Me.grvListParts.DataBind()
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, sCnsErr_00006, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, sCnsErr_00005, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            For zz As Integer = 1 To intMONTHCount - 1
                Me.grvListControl.Columns(3 + zz).Visible = True
                Me.grvListParts.Columns(3 + zz).Visible = True
            Next
            Me.grvListControl.DataSource = New DataTable
            Me.grvListControl.DataBind()
            Me.grvListParts.DataSource = New DataTable
            Me.grvListParts.DataBind()

        End If


        ''★2014/03/26 Hamamoto Edit Start
        'Dim conDB As New SqlConnection
        'Dim cmdDB As New SqlCommand
        'Dim dstOrders As New DataSet
        'Dim dtKikanDt As New Date
        'Dim strDtFm As String = String.Format("{0:yyyy/MM/dd}", Me.dftSupportDt.ppFromDate)
        'Dim strDtFm2 As String = String.Format("{0:yyyyMM}", Me.dftSupportDt.ppFromDate)
        'Dim strDtTo As String = Me.dftSupportDt.ppToText
        'Dim strDtTo2 As String = ""
        ''--------------------------------
        ''2014/04/14 星野　ここから
        ''--------------------------------
        'objStack = New StackFrame
        ''--------------------------------
        ''2014/04/14 星野　ここまで
        ''--------------------------------
        'Try
        '    If Me.dftStartDt.ppToText <> "" Then
        '        'Toが空白でない
        '        'ToがFromから6ヶ月以内かどうかチェック
        '        If Me.dftStartDt.ppFromDate.AddMonths(6).AddDays(-1) < Me.dftStartDt.ppToDate Then
        '            Me.dftStartDt.psSet_ErrorNo(6002, "終了対応年月", "開始対応年月の先6ヶ月以内")
        '            Me.dftStartDt.ppDateBoxTo.Focus()
        '            Return
        '        Else
        '            strDtTo = String.Format("{0:yyyy/MM/dd}", dftSupportDt.ppToDate)
        '            strDtTo2 = String.Format("{0:yyyyMM}", dftSupportDt.ppToDate)
        '        End If
        '    Else
        '        If CDate("9999/12/31").AddMonths(-6) < dftSupportDt.ppFromDate Then
        '            strDtTo = "9999/12/31"
        '            strDtTo2 = "999912"
        '        Else
        '            'Toが空白
        '            strDtTo = String.Format("{0:yyyy/MM/dd}", Me.dftSupportDt.ppFromDate.AddMonths(6).AddDays(-1))
        '            strDtTo2 = String.Format("{0:yyyyMM}", Me.dftSupportDt.ppFromDate.AddMonths(6).AddDays(-1))
        '        End If

        '    End If

        '    If strDtTo <> "" Then
        '        'Toが空白でない
        '        'ToがFromから6ヶ月以内かどうかチェック
        '        If Me.dftSupportDt.ppFromDate.AddMonths(6).AddDays(-1) < Me.dftSupportDt.ppToDate Then
        '            Me.dftSupportDt.psSet_ErrorNo(6002, "終了対応年月", "開始対応年月の先6ヶ月以内")
        '            Me.dftSupportDt.ppDateBoxTo.Focus()
        '            Return
        '        Else
        '            strDtTo = String.Format("{0:yyyy/MM/dd}", dftSupportDt.ppToDate)
        '            strDtTo2 = String.Format("{0:yyyyMM}", dftSupportDt.ppToDate)
        '        End If
        '    Else
        '        If CDate("9999/12/31").AddMonths(-6) < dftSupportDt.ppFromDate Then
        '            strDtTo = "9999/12/31"
        '            strDtTo2 = "999912"
        '        Else
        '            'Toが空白
        '            strDtTo = String.Format("{0:yyyy/MM/dd}", Me.dftSupportDt.ppFromDate.AddMonths(6).AddDays(-1))
        '            strDtTo2 = String.Format("{0:yyyyMM}", Me.dftSupportDt.ppFromDate.AddMonths(6).AddDays(-1))
        '        End If

        '    End If

        '    '画面ページ表示初期化
        '    Master.ppCount = "0"
        '    Me.grvList.DataSource = Nothing

        '    'ＤＢ接続
        '    If Not pfOpen_Database(conDB) Then
        '        Throw New Exception("")
        '    End If

        '    'パラメータ設定
        '    cmdDB = New SqlCommand(sCnsSqlid11, conDB)
        '    cmdDB.CommandType = CommandType.StoredProcedure

        '    With cmdDB.Parameters
        '        '.Add(pfSet_Param("start_dt_f", SqlDbType.NVarChar, Me.dftStartDt.ppFromText))   '対応年月
        '        '.Add(pfSet_Param("start_dt_t", SqlDbType.NVarChar, Me.dftStartDt.ppToText))
        '        '.Add(pfSet_Param("group_cls", SqlDbType.NVarChar, Me.ddlClass.ppSelectedValue))
        '        .Add(pfSet_Param("START_DT_From", SqlDbType.NVarChar, strDtFm))   '対応年月
        '        .Add(pfSet_Param("START_DT_To", SqlDbType.NVarChar, strDtTo))
        '        .Add(pfSet_Param("START_DT_From2", SqlDbType.NVarChar, strDtFm2))   '対応年月
        '        .Add(pfSet_Param("START_DT_To2", SqlDbType.NVarChar, strDtTo2))
        '        .Add(pfSet_Param("TBOX_TYPE", SqlDbType.NVarChar, Me.ddlTboxType.ppSelectedValue))    '種別
        '    End With

        '    'データ取得およびデータをリストに設定
        '    dstOrders = pfGet_DataSet(cmdDB)
        '    If dstOrders IsNot Nothing AndAlso dstOrders.Tables.Count > 0 Then
        '        grvList.DataSource = dstOrders.Tables(0)
        '        Me.grvList.DataBind()
        '        msSet_GrvList(dstOrders.Tables(0), 0)
        '        Me.grvList.Columns(6).Visible = False
        '    End If

        '    'パラメータ設定
        '    cmdDB = New SqlCommand(sCnsSqlid2, conDB)
        '    cmdDB.CommandType = CommandType.StoredProcedure

        '    With cmdDB.Parameters
        '        .Add(pfSet_Param("START_DT_From", SqlDbType.NVarChar, strDtFm))   '対応年月
        '        .Add(pfSet_Param("START_DT_To", SqlDbType.NVarChar, strDtTo))
        '        .Add(pfSet_Param("START_DT_From2", SqlDbType.NVarChar, strDtFm2))   '対応年月
        '        .Add(pfSet_Param("START_DT_To2", SqlDbType.NVarChar, strDtTo2))
        '        .Add(pfSet_Param("TBOX_TYPE", SqlDbType.NVarChar, Me.ddlTboxType.ppSelectedValue))    '種別
        '    End With

        '    'データ取得およびデータをリストに設定
        '    Dim changeDataSet As DataSet = dstOrders.GetChanges()

        '    dstOrders = New DataSet
        '    dstOrders = pfGet_DataSet(cmdDB)
        '    If dstOrders IsNot Nothing AndAlso dstOrders.Tables.Count > 0 Then
        '        grvList1.DataSource = dstOrders.Tables(0)
        '        Me.grvList1.DataBind()
        '        msSet_GrvList(dstOrders.Tables(0), 1)
        '    End If

        '    '種別表示
        '    lblClass2.Text = Me.ddlTboxType.ppSelectedTextOnly


        '    '件数を設定
        '    Master.ppCount = Me.grvList.Rows.Count

        '    'マルチビューに検索エリアを表示する
        '    Master.ppMultiView.ActiveViewIndex =  ClsComVer.E_照会マルチビュー.一覧表示

        '    '変更を反映
        '    Me.grvList.DataBind()
        '    Me.grvList1.DataBind()

        '    'If grvList.Rows.Count > 0 Then
        '    '    Master.Master.ppRigthButton9.Visible = True
        '    '    Master.Master.ppRigthButton10.Visible = True
        '    'End If

        '    'ビューに退避
        '    Dim addedDataSet As DataSet = dstOrders
        '    ViewState("ds") = addedDataSet

        '    '' ＤＢ接続変数
        '    'Dim conDB As New SqlConnection
        '    'Dim cmdDB As New SqlCommand
        '    'Dim dstOrders As New DataSet
        '    'Dim dtKikanDt As New Date

        '    'Dim strCNST_CLS_Chk As StringBuilder = New StringBuilder
        '    'Try
        '    '    'チェック
        '    '    If Not Me.dftSupportDt.ppToDate.Equals(String.Empty) Then
        '    '        dtKikanDt = Me.dftSupportDt.ppToDate.AddMonths(-5)
        '    '        If Me.dftSupportDt.ppFromDate.ToString("yyyyMMdd") < dtKikanDt.ToString("yyyyMMdd") Then
        '    '            Exit Sub
        '    '        End If
        '    '    End If

        '    '    '画面ページ表示初期化
        '    '    Master.ppCount = "0"
        '    '    Me.grvList.DataSource = Nothing

        '    '    'ＤＢ接続
        '    '    If Not pfOpen_Database(conDB) Then
        '    '        Throw New Exception("")
        '    '    End If

        '    '    'パラメータ設定
        '    '    cmdDB = New SqlCommand(sCnsSqlid, conDB)
        '    '    With cmdDB.Parameters
        '    '        If Me.dftSupportDt.ppFromDate.Equals(String.Empty) _
        '    '           And Me.dftSupportDt.ppToDate.Equals(String.Empty) Then
        '    '            dtKikanDt = Date.Now.AddMonths(-5)
        '    '            .Add(pfSet_Param("START_DT_From", SqlDbType.NVarChar, Date.Now.ToString("yyyy/MM")))   '対応年月
        '    '            .Add(pfSet_Param("START_DT_To", SqlDbType.NVarChar, Date.Now.ToString("yyyy/MM")))
        '    '        Else
        '    '            If Me.dftSupportDt.ppToDate.Equals(String.Empty) Then
        '    '                dtKikanDt = Me.dftSupportDt.ppToDate.AddMonths(-5)
        '    '                .Add(pfSet_Param("START_DT_From", SqlDbType.NVarChar, Me.dftSupportDt.ppFromDate.ToString("yyyy/MM")))    '対応年月
        '    '                .Add(pfSet_Param("START_DT_To", SqlDbType.NVarChar, dtKikanDt.ToString("yyyy/MM")))
        '    '            Else
        '    '                .Add(pfSet_Param("START_DT_From", SqlDbType.NVarChar, Me.dftSupportDt.ppFromDate.ToString("yyyy/MM")))    '対応年月
        '    '                .Add(pfSet_Param("START_DT_To", SqlDbType.NVarChar, Me.dftSupportDt.ppFromDate.ToString("yyyy/MM")))
        '    '            End If
        '    '        End If
        '    '        .Add(pfSet_Param("TBOX_TYPE", SqlDbType.NVarChar, Me.ddlTboxType.ppSelectedValue))    '種別
        '    '    End With

        '    '    'データ取得およびデータをリストに設定
        '    '    dstOrders = pfGet_DataSet(cmdDB)
        '    '    If dstOrders IsNot Nothing Then
        '    '        msSet_GrvList(dstOrders.Tables(0), 0)
        '    '    End If

        '    '    'パラメータ設定
        '    '    cmdDB = New SqlCommand(sCnsSqlid2, conDB)
        '    '    With cmdDB.Parameters
        '    '        If Me.dftSupportDt.ppFromDate.Equals(String.Empty) _
        '    '           And Me.dftSupportDt.ppToDate.Equals(String.Empty) Then
        '    '            dtKikanDt = Date.Now.AddMonths(-5)
        '    '            .Add(pfSet_Param("START_DT_From", SqlDbType.NVarChar, Date.Now.ToString("yyyy/MM")))   '対応年月
        '    '            .Add(pfSet_Param("START_DT_To", SqlDbType.NVarChar, Date.Now.ToString("yyyy/MM")))
        '    '        Else
        '    '            If Me.dftSupportDt.ppToDate.Equals(String.Empty) Then
        '    '                dtKikanDt = Me.dftSupportDt.ppToDate.AddMonths(-5)
        '    '                .Add(pfSet_Param("START_DT_From", SqlDbType.NVarChar, Me.dftSupportDt.ppFromDate.ToString("yyyy/MM")))    '対応年月
        '    '                .Add(pfSet_Param("START_DT_To", SqlDbType.NVarChar, dtKikanDt.ToString("yyyy/MM")))
        '    '            Else
        '    '                .Add(pfSet_Param("START_DT_From", SqlDbType.NVarChar, Me.dftSupportDt.ppFromDate.ToString("yyyy/MM")))    '対応年月
        '    '                .Add(pfSet_Param("START_DT_To", SqlDbType.NVarChar, Me.dftSupportDt.ppFromDate.ToString("yyyy/MM")))
        '    '            End If
        '    '        End If
        '    '        .Add(pfSet_Param("TBOX_TYPE", SqlDbType.NVarChar, Me.ddlTboxType.ppSelectedValue))    '種別
        '    '    End With

        '    '    'データ取得およびデータをリストに設定
        '    '    Dim changeDataSet As DataSet = dstOrders.GetChanges()

        '    '    dstOrders = New DataSet
        '    '    dstOrders = pfGet_DataSet(cmdDB)
        '    '    If dstOrders IsNot Nothing Then
        '    '        msSet_GrvList(dstOrders.Tables(0), 1)
        '    '    End If

        '    '    '種別表示
        '    '    lblClass.Text = Me.ddlTboxType.ppSelectedTextOnly


        '    '    '件数を設定
        '    '    Master.ppCount = Me.grvList.Controls.Count.ToString

        '    '    'マルチビューに検索エリアを表示する
        '    '    Master.ppMultiView.ActiveViewIndex =  ClsComVer.E_照会マルチビュー.一覧表示

        '    '    '変更を反映
        '    '    Me.grvList.DataBind()
        '    '    Me.grvList1.DataBind()

        '    '    'ビューに退避
        '    '    Dim addedDataSet As DataSet = changeDataSet.GetChanges(DataRowState.Added)
        '    '    ViewState("ds") = addedDataSet
        '    'Catch ex As DBConcurrencyException
        '    '    psMesBox(Me, "0005", clscomver.E_Mタイプ.エラー, clscomver.E_Mモード.OK, clscomver.E_S実行.描画後)
        '    ''--------------------------------
        '    ''2014/04/14 星野　ここから
        '    ''--------------------------------
        '    ''ログ出力
        '    'psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
        '    '                objStack.GetMethod.Name, "", ex.ToString, "Catch")
        '    ''--------------------------------
        '    ''2014/04/14 星野　ここまで
        '    ''--------------------------------
        '    'Catch ex As Exception
        '    '    psMesBox(Me, "0003", clscomver.E_Mタイプ.エラー, clscomver.E_Mモード.OK, clscomver.E_S実行.描画後)
        '    ''--------------------------------
        '    ''2014/04/14 星野　ここから
        '    ''--------------------------------
        '    ''ログ出力
        '    'psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
        '    '                objStack.GetMethod.Name, "", ex.ToString, "Catch")
        '    ''--------------------------------
        '    ''2014/04/14 星野　ここまで
        '    ''--------------------------------
        '    '    Me.grvList.DataBind()
        '    '    Me.grvList1.DataBind()
        '    '    Throw
        '    'Finally
        '    '    'DB切断
        '    '    pfClose_Database(conDB)
        '    'End Try

        'Catch ex As DBConcurrencyException
        '    psMesBox(Me, "0005", clscomver.E_Mタイプ.エラー, clscomver.E_Mモード.OK, clscomver.E_S実行.描画後)
        '    '--------------------------------
        '    '2014/04/14 星野　ここから
        '    '--------------------------------
        '    'ログ出力
        '    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
        '                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
        '    '--------------------------------
        '    '2014/04/14 星野　ここまで
        '    '--------------------------------
        '    'grvList.DataSource = New DataTable()
        '    'grvList1.DataSource = New DataTable()
        '    'Me.grvList.DataBind()
        '    'Me.grvList1.DataBind()
        'Catch ex As Exception
        '    psMesBox(Me, "0003", clscomver.E_Mタイプ.エラー, clscomver.E_Mモード.OK, clscomver.E_S実行.描画後)
        '    '--------------------------------
        '    '2014/04/14 星野　ここから
        '    '--------------------------------
        '    'ログ出力
        '    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
        '                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
        '    '--------------------------------
        '    '2014/04/14 星野　ここまで
        '    '--------------------------------
        '    'grvList.DataSource = New DataTable()
        '    'grvList1.DataSource = New DataTable()
        '    'Me.grvList.DataBind()
        '    'Me.grvList1.DataBind()
        '    Throw
        'Finally
        '    'DB切断
        '    pfClose_Database(conDB)
        'End Try
        ''★2014/03/26 Hamamoto Edit   End

    End Sub

#End Region

#Region "故障率算出処理"

    ''' <summary>
    ''' 故障率算出処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_GrvList(ByRef dttOrders1 As DataTable, ByRef dttOrders2 As DataTable, ByVal intShori As Integer)
        '★2014/03/27 Hamamoto Edit Start
        Dim intCnt1 As Integer = 0
        Dim intCnt2 As Integer = 0
        Dim numRow1 As Integer = 0
        Dim numRow2 As Integer = 0

        Dim intMONTHCount As Integer = 6        '検索条件月数

        Dim strKeisan As String = String.Empty

        'Dim xlApp As New Excel.Application

        Dim numKoshouritsu As Double

        numRow1 = dttOrders1.Rows.Count
        If Me.ddlClass.ppSelectedValue = "3" Then
            intCnt1 = 8
        Else
            intCnt1 = 7
        End If

        numRow2 = dttOrders2.Rows.Count
        intCnt2 = 5

        If intShori.Equals(0) Then
            '制御部
            For zz As Integer = 0 To numRow1 - 1 Step intCnt1
                For yy As Integer = 1 To intMONTHCount + 1
                    strKeisan = dttOrders1.Rows(zz).Item("故障率").ToString.Trim
                    'If 0 <= strKeisan.IndexOf("対応件数") Then
                    '    strKeisan = strKeisan.Replace("対応件数", Integer.Parse(dttOrders1.Rows(zz)(2 + yy).ToString))
                    'End If
                    'If 0 <= strKeisan.IndexOf("調整中台数") Then
                    '    strKeisan = strKeisan.Replace("調整中台数", Integer.Parse(dttOrders1.Rows(1 + zz)(2 + yy).ToString))
                    'End If
                    If 0 <= strKeisan.IndexOf("制御部故障台数") Then
                        strKeisan = strKeisan.Replace("制御部故障台数", Integer.Parse(dttOrders1.Rows(2 + zz)(2 + yy).ToString.Replace(",", "")))
                    End If
                    If 0 <= strKeisan.IndexOf("ＨＤＤ故障台数") Then
                        strKeisan = strKeisan.Replace("ＨＤＤ故障台数", Integer.Parse(dttOrders1.Rows(3 + zz)(2 + yy).ToString.Replace(",", "")))
                    End If
                    If 0 <= strKeisan.IndexOf("HDD故障台数") Then
                        strKeisan = strKeisan.Replace("HDD故障台数", Integer.Parse(dttOrders1.Rows(3 + zz)(2 + yy).ToString.Replace(",", "")))
                    End If

                    If Me.ddlClass.ppSelectedValue = "3" Then
                        If 0 <= strKeisan.IndexOf("ＭＣ故障台数") Then
                            strKeisan = strKeisan.Replace("ＭＣ故障台数", Integer.Parse(dttOrders1.Rows(4 + zz)(2 + yy).ToString.Replace(",", "")))
                        End If

                        If 0 <= strKeisan.IndexOf("MC故障台数") Then
                            strKeisan = strKeisan.Replace("MC故障台数", Integer.Parse(dttOrders1.Rows(4 + zz)(2 + yy).ToString.Replace(",", "")))
                        End If
                        'If 0 <= strKeisan.IndexOf("ＴＯＫ台数") Then
                        '    strKeisan = strKeisan.Replace("ＴＯＫ台数", Integer.Parse(dttOrders1.Rows(5 + zz)(2 + yy).ToString))
                        'End If
                        If 0 <= strKeisan.IndexOf("設置台数") Then
                            strKeisan = strKeisan.Replace("設置台数", Integer.Parse(dttOrders1.Rows(7 + zz)(2 + yy).ToString.Replace(",", "")))
                        End If

                        If pfRPN(strKeisan, numKoshouritsu) Then
                            dttOrders1.Rows(6 + zz)(2 + yy) = String.Format("{0:0.00000}", numKoshouritsu)
                        Else
                            dttOrders1.Rows(6 + zz)(2 + yy) = "0.00000"
                        End If
                    Else
                        If 0 <= strKeisan.IndexOf("設置台数") Then
                            strKeisan = strKeisan.Replace("設置台数", Integer.Parse(dttOrders1.Rows(6 + zz)(2 + yy).ToString.Replace(",", "")))
                        End If

                        If pfRPN(strKeisan, numKoshouritsu) Then
                            dttOrders1.Rows(5 + zz)(2 + yy) = String.Format("{0:0.00000}", numKoshouritsu)
                        Else
                            dttOrders1.Rows(5 + zz)(2 + yy) = "0.00000"
                        End If
                    End If

                    'If dttOrders1.Rows(7 + zz)(2 + yy).ToString <> "0" Then
                    '    dttOrders1.Rows(6 + zz)(2 + yy) = String.Format("{0:0.00000}", xlApp.Evaluate(strKeisan))
                    'Else
                    '    dttOrders1.Rows(6 + zz)(2 + yy) = "0.00000"
                    'End If
                Next
            Next
        Else
            'プリンタ/ディスプレイ/操作盤
            For zz As Integer = 0 To numRow2 - 1 Step intCnt2
                For yy As Integer = 1 To intMONTHCount + 1
                    strKeisan = dttOrders2.Rows(zz).Item("故障率").ToString.Trim
                    'If 0 <= strKeisan.IndexOf("対応件数") Then
                    '    dttOrders2.Rows(0).Item("").ToString()
                    '    strKeisan = strKeisan.Replace("対応件数", Integer.Parse(dttOrders2.Rows(zz)(2 + yy).ToString))
                    'End If
                    'If 0 <= strKeisan.IndexOf("調整中台数") Then
                    '    strKeisan = strKeisan.Replace("調整中台数", Integer.Parse(dttOrders2.Rows(1 + zz)(2 + yy).ToString))
                    'End If
                    If 0 <= strKeisan.IndexOf("故障台数") Then
                        strKeisan = strKeisan.Replace("故障台数", Integer.Parse(dttOrders2.Rows(2 + zz)(2 + yy).ToString.Replace(",", "")))
                    End If
                    'If 0 <= strKeisan.IndexOf("ＴＯＫ台数") Then
                    '    strKeisan = strKeisan.Replace("ＴＯＫ台数", Integer.Parse(dttOrders2.Rows(3 + zz)(2 + yy).ToString))
                    'End If
                    Select Case Me.ddlClass.ppSelectedValue
                        Case "1"
                            If 0 <= strKeisan.IndexOf("設置台数") Then
                                strKeisan = strKeisan.Replace("設置台数", Integer.Parse(dttOrders1.Rows(6)(2 + yy).ToString.Replace(",", "")))
                            End If
                        Case "2"
                            If 0 <= strKeisan.IndexOf("IT130S設置台数") Then
                                strKeisan = strKeisan.Replace("IT130S設置台数", Integer.Parse(dttOrders1.Rows(6)(2 + yy).ToString.Replace(",", "")))
                            End If
                            If 0 <= strKeisan.IndexOf("IT135S設置台数") AndAlso numRow1 >= intCnt1 * 2 Then
                                strKeisan = strKeisan.Replace("IT135S設置台数", Integer.Parse(dttOrders1.Rows(13)(2 + yy).ToString.Replace(",", "")))
                            End If
                        Case "3"
                            If 0 <= strKeisan.IndexOf("NVC100設置台数") Then
                                strKeisan = strKeisan.Replace("NVC100設置台数", Integer.Parse(dttOrders1.Rows(7)(2 + yy).ToString.Replace(",", "")))
                            End If
                            If 0 <= strKeisan.IndexOf("NVC100S設置台数") AndAlso numRow1 >= intCnt1 * 2 Then
                                strKeisan = strKeisan.Replace("NVC100S設置台数", Integer.Parse(dttOrders1.Rows(15)(2 + yy).ToString.Replace(",", "")))
                            End If
                            If 0 <= strKeisan.IndexOf("NVC130設置台数") AndAlso numRow1 >= intCnt1 * 3 Then
                                strKeisan = strKeisan.Replace("NVC130設置台数", Integer.Parse(dttOrders1.Rows(23)(2 + yy).ToString.Replace(",", "")))
                            End If
                            If 0 <= strKeisan.IndexOf("NVC300設置台数") AndAlso numRow1 >= intCnt1 * 4 Then
                                strKeisan = strKeisan.Replace("NVC300設置台数", Integer.Parse(dttOrders1.Rows(31)(2 + yy).ToString.Replace(",", "")))
                            End If
                    End Select
                    If pfRPN(strKeisan, numKoshouritsu) Then
                        dttOrders2.Rows(4 + zz)(2 + yy) = String.Format("{0:0.00000}", numKoshouritsu)
                    Else
                        dttOrders2.Rows(4 + zz)(2 + yy) = "0.00000"
                    End If
                Next
            Next
        End If

        'For i As Integer = 0 To numRow - 1
        '    For s As Integer = 0 To intCnt
        '        If Me.grvList.Rows(i + s).Cells(E_QUAOUTP001_Col.部位).RowSpan = 0 Then
        '            Me.grvList.Rows(i + s).Cells(E_QUAOUTP001_Col.部位).RowSpan = 2
        '            Me.grvList.Rows(i + s).Cells(E_QUAOUTP001_Col.目標値).RowSpan = 2
        '        Else
        '            Me.grvList.Rows(i + s).Cells(E_QUAOUTP001_Col.部位).RowSpan = Me.grvList.Rows(i + s).Cells(E_QUAOUTP001_Col.部位).RowSpan + 1
        '            Me.grvList.Rows(i + s).Cells(E_QUAOUTP001_Col.目標値).RowSpan = Me.grvList.Rows(i + s).Cells(E_QUAOUTP001_Col.目標値).RowSpan + 1
        '        End If
        '        'Me.grvList.Rows(i + s).Cells(E_QUAOUTP001_Col.部位).Visible = False
        '        'Me.grvList.Rows(i + s).Cells(E_QUAOUTP001_Col.目標値).Visible = False
        '        strKesan = dttOrders.Rows(i + s).Item("計算式").ToString.Trim
        '        If 0 <= strKesan.IndexOf("対応件数") Then
        '            dttOrders.Rows(0).Item("").ToString()
        '            strKesan = strKesan.Replace("対応件数", CInt(dttOrders.Rows(i + s).Item("対応件数").ToString))
        '        End If
        '        If 0 <= strKesan.IndexOf("調整中台数") Then
        '            strKesan = strKesan.Replace("調整中台数", CInt(dttOrders.Rows(i + s).Item("調整中台数").ToString))
        '        End If
        '        If 0 <= strKesan.IndexOf("制御部故障台数") Then
        '            strKesan = strKesan.Replace("制御部故障台数", CInt(dttOrders.Rows(i + s).Item("制御部故障台数").ToString))
        '        End If
        '        If 0 <= strKesan.IndexOf("ＨＤＤ故障対数") Then
        '            strKesan = strKesan.Replace("ＨＤＤ故障対数", CInt(dttOrders.Rows(i + s).Item("ＨＤＤ故障対数").ToString))
        '        End If
        '        If 0 <= strKesan.IndexOf("ＭＣ故障台数") Then
        '            strKesan = strKesan.Replace("ＭＣ故障台数", CInt(dttOrders.Rows(i + s).Item("ＭＣ故障台数").ToString))
        '        End If
        '        If 0 <= strKesan.IndexOf("ＴＯＫ台数") Then
        '            strKesan = strKesan.Replace("ＴＯＫ台数", CInt(dttOrders.Rows(i + s).Item("ＴＯＫ台数").ToString))
        '        End If
        '        If 0 <= strKesan.IndexOf("設置台数") Then
        '            strKesan = strKesan.Replace("設置台数", CInt(dttOrders.Rows(i + s).Item("設置台数").ToString))
        '        End If
        '        'TODO 計算列例　i1 & " + " & i2 & " + 5"
        '        dttOrders.Rows(i + s).Item("故障率") = xlApp.Evaluate(strKesan)
        '    Next

        '    i += intCnt

        'Next

        ''Dim intCnt As Integer = 0
        ''Dim numRow As Integer = 0

        ''If intShori.Equals(0) Then
        ''    '制御部
        ''    numRow = dttOrders.Rows.Count
        ''    intCnt = 7
        ''Else
        ''    'プリンタ・ディスプレイ
        ''    numRow = Me.grvList1.Rows.Count
        ''    intCnt = 5
        ''End If
        ''Dim strKesan As String = String.Empty

        ''Dim baseCell_1 As TableCell = New TableCell
        ''Dim baseCell_2 As TableCell = New TableCell

        ''Dim xlApp As New Excel.Application

        ''For i As Integer = 0 To numRow - 1
        ''    For s As Integer = 0 To intCnt

        ''        baseCell_1 = Me.grvList.Rows(i + s).Cells("部位")
        ''        baseCell_2 = Me.grvList.Rows(i + s).Cells("目標値")
        ''        If baseCell_1.RowSpan = 0 Then
        ''            baseCell_1.RowSpan = 2
        ''            baseCell_2.RowSpan = 2
        ''        Else
        ''            baseCell_1.RowSpan = baseCell_1.RowSpan + 1
        ''            baseCell_2.RowSpan = baseCell_2.RowSpan + 1
        ''        End If
        ''        Me.grvList.Rows(i + s).Cells("部位").Visible = False
        ''        Me.grvList.Rows(i + s).Cells("目標値").Visible = False
        ''        strKesan = Me.grvList.Rows(i + s).Cells("計算式").Text.Trim
        ''        If 0 <= strKesan.IndexOf("対応件数") Then
        ''            dttOrders.Rows(0).Item("").ToString()
        ''            strKesan = strKesan.Replace("対応件数", CInt(dttOrders.Rows(i + s).Item("対応件数").ToString))
        ''        End If
        ''        If 0 <= strKesan.IndexOf("調整中台数") Then
        ''            strKesan = strKesan.Replace("調整中台数", CInt(dttOrders.Rows(i + s).Item("調整中台数").ToString))
        ''        End If
        ''        If 0 <= strKesan.IndexOf("制御部故障台数") Then
        ''            strKesan = strKesan.Replace("制御部故障台数", CInt(dttOrders.Rows(i + s).Item("制御部故障台数").ToString))
        ''        End If
        ''        If 0 <= strKesan.IndexOf("ＨＤＤ故障対数") Then
        ''            strKesan = strKesan.Replace("ＨＤＤ故障対数", CInt(dttOrders.Rows(i + s).Item("ＨＤＤ故障対数").ToString))
        ''        End If
        ''        If 0 <= strKesan.IndexOf("ＭＣ故障台数") Then
        ''            strKesan = strKesan.Replace("ＭＣ故障台数", CInt(dttOrders.Rows(i + s).Item("ＭＣ故障台数").ToString))
        ''        End If
        ''        If 0 <= strKesan.IndexOf("ＴＯＫ台数") Then
        ''            strKesan = strKesan.Replace("ＴＯＫ台数", CInt(dttOrders.Rows(i + s).Item("ＴＯＫ台数").ToString))
        ''        End If
        ''        If 0 <= strKesan.IndexOf("設置台数") Then
        ''            strKesan = strKesan.Replace("設置台数", CInt(dttOrders.Rows(i + s).Item("設置台数").ToString))
        ''        End If
        ''        'TODO 計算列例　i1 & " + " & i2 & " + 5"
        ''        dttOrders.Rows(i + s).Item("故障率") = xlApp.Evaluate(strKesan)
        ''    Next

        ''    i += intCnt

        ''Next

        ''★2014/03/27 Hamamoto Edit   End
    End Sub

#End Region

#Region "グリッドビューセル結合"
    ''' <summary>
    ''' グリッドビューセル結合
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msComb_GrvList(ByVal dttOrders As DataTable, ByVal intShori As Integer)
        Dim numRow As Integer = 0
        Dim Bnum As Integer = 0
        Dim Nnum As Integer = 0

        Dim baseCell1 As TableCell = New TableCell
        Dim baseCell2 As TableCell = New TableCell
        Dim nextCell1 As TableCell = New TableCell
        Dim nextCell2 As TableCell = New TableCell

        numRow = dttOrders.Rows.Count

        If intShori.Equals(0) Then
            '制御部
            While Bnum < numRow - 1
                Nnum = Bnum + 1
                baseCell1 = grvListControl.Rows(Bnum).Cells(0)
                baseCell2 = grvListControl.Rows(Bnum).Cells(1)

                While Nnum < numRow
                    nextCell1 = grvListControl.Rows(Nnum).Cells(0)
                    nextCell2 = grvListControl.Rows(Nnum).Cells(1)
                    If baseCell1.Text = nextCell1.Text AndAlso baseCell2.Text = baseCell2.Text Then
                        If baseCell1.RowSpan = 0 Then
                            baseCell1.RowSpan = 2
                            baseCell2.RowSpan = 2
                        Else
                            baseCell1.RowSpan = baseCell1.RowSpan + 1
                            baseCell2.RowSpan = baseCell2.RowSpan + 1
                        End If
                        'grvListControl.Rows(Nnum).Cells.Remove(nextCell)
                        grvListControl.Rows(Nnum).Cells(0).Visible = False
                        grvListControl.Rows(Nnum).Cells(1).Visible = False
                        Nnum = Nnum + 1
                    Else
                        Exit While
                    End If
                End While
                Bnum = Nnum
            End While
            Bnum = 0
            Nnum = 0
        Else
            'プリンタ/ディスプレイ/操作盤
            While Bnum < numRow - 1
                Nnum = Bnum + 1
                baseCell1 = grvListParts.Rows(Bnum).Cells(0)
                baseCell2 = grvListParts.Rows(Bnum).Cells(1)

                While Nnum < numRow
                    nextCell1 = grvListParts.Rows(Nnum).Cells(0)
                    nextCell2 = grvListParts.Rows(Nnum).Cells(1)
                    If baseCell1.Text = nextCell1.Text AndAlso baseCell2.Text = baseCell2.Text Then
                        If baseCell1.RowSpan = 0 Then
                            baseCell1.RowSpan = 2
                            baseCell2.RowSpan = 2
                        Else
                            baseCell1.RowSpan = baseCell1.RowSpan + 1
                            baseCell2.RowSpan = baseCell2.RowSpan + 1
                        End If
                        'grvListControl.Rows(Nnum).Cells.Remove(nextCell)
                        grvListParts.Rows(Nnum).Cells(0).Visible = False
                        grvListParts.Rows(Nnum).Cells(1).Visible = False
                        Nnum = Nnum + 1
                    Else
                        Exit While
                    End If
                End While
                Bnum = Nnum
            End While
            Bnum = 0
            Nnum = 0
        End If

    End Sub

#End Region

    '-----------------------------
    '2014/04/24 土岐　ここから
    '-----------------------------
#Region "印刷データ作成"
    Private Function mfGetPrintDate() As DataTable
        Const MAXGRID = 1
        Dim dttGridData(MAXGRID) As DataTable
        Dim grvGridView(MAXGRID) As GridView
        Dim dttPrint As DataTable
        Dim dtrData As DataRow
        Dim datDate As DateTime

        '表示データ設定
        dttGridData(0) = CType(ViewState("ds1"), DataSet).Tables(0)
        grvGridView(0) = Me.grvListControl
        dttGridData(1) = CType(ViewState("ds2"), DataSet).Tables(0)
        grvGridView(1) = Me.grvListParts

        dttPrint = New DataTable

        dttPrint.Columns.Add("タイトル")
        dttPrint.Columns.Add("タイトル年月")
        dttPrint.Columns.Add("日付")
        dttPrint.Columns.Add("会社名")
        dttPrint.Columns.Add("部位")
        dttPrint.Columns.Add("目標値")
        dttPrint.Columns.Add("数量名称")
        dttPrint.Columns.Add("項目タイトル1")
        dttPrint.Columns.Add("数量1")
        dttPrint.Columns.Add("項目タイトル2")
        dttPrint.Columns.Add("数量2")
        dttPrint.Columns.Add("項目タイトル3")
        dttPrint.Columns.Add("数量3")
        dttPrint.Columns.Add("項目タイトル4")
        dttPrint.Columns.Add("数量4")
        dttPrint.Columns.Add("項目タイトル5")
        dttPrint.Columns.Add("数量5")
        dttPrint.Columns.Add("項目タイトル6")
        dttPrint.Columns.Add("数量6")
        dttPrint.Columns.Add("項目タイトル7")
        dttPrint.Columns.Add("数量7")

        datDate = DateTime.Now

        'データ設定
        For zz As Integer = 0 To MAXGRID
            For Each dtrRow As DataRow In dttGridData(zz).Rows
                dtrData = dttPrint.NewRow
                dtrData.Item("タイトル") = Me.lblClass2.Text
                '-----------------------------
                '2014/04/25 土岐　ここから
                '-----------------------------
                '表示中の末月表示
                dtrData.Item("タイトル年月") = ViewState(sCnsTitleDate)
                '-----------------------------
                '2014/04/25 土岐　ここまで
                '-----------------------------
                dtrData.Item("日付") = datDate.ToString("yyyy年MM月dd日")
                dtrData.Item("会社名") = dtrRow.Item("会社名")
                dtrData.Item("部位") = dtrRow.Item("部位")
                dtrData.Item("目標値") = dtrRow.Item("目標値")
                dtrData.Item("数量名称") = dtrRow.Item("数量名称")
                '表示項目以外は空白を設定
                For yy As Integer = 1 To 7
                    If Me.grvListControl.Columns(3 + (yy - 1)).Visible Then
                        dtrData.Item("項目タイトル" & yy.ToString) =
                            Me.grvListControl.HeaderRow.Cells(3 + (yy - 1)).Text
                        dtrData.Item("数量" & yy.ToString) = dtrRow.Item("数量" & yy.ToString)
                    Else
                        dtrData.Item("項目タイトル" & yy.ToString) = String.Empty
                        dtrData.Item("数量" & yy.ToString) = String.Empty
                    End If
                Next
                dttPrint.Rows.Add(dtrData)
            Next

        Next

        Return dttPrint
    End Function
#End Region
    '-----------------------------
    '2014/04/24 土岐　ここまで
    '-----------------------------

#Region "ＣＳＶ作成処理"

    ''' <summary>
    ''' ＣＳＶ作成処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub msCsv()
        Dim intRtn As Integer               '戻り値
        Dim dttData As DataTable            'ＣＳＶデータ

        '開始ログ出力
        psLogStart(Me)

        'ＣＳＶデータ取得処理
        dttData = DirectCast(ViewState("ds1"), DataSet).Tables(0)
        dttData.Merge(DirectCast(ViewState("ds2"), DataSet).Tables(0))

        '出力不必要列削除
        dttData.Columns.Remove("会社名")

        'ヘッダー名設定
        For zz As Integer = 0 To dttData.Columns.Count - 1
            If Me.grvListControl.Columns(zz).Visible = True Then
                dttData.Columns(zz).ColumnName = Me.grvListControl.HeaderRow.Cells(zz).Text
            End If
        Next
        dttData.Columns(dttData.Columns.Count - 1).ColumnName = Me.grvListParts.HeaderRow.Cells(Me.grvListControl.Columns.Count - 1).Text

        'ファイル出力
        intRtn = pfDLCSV("ホール機器品質管理表", "", dttData, True, Me)

        'ファイル出力エラー
        If intRtn <> 0 Then
            psMesBox(Me, sCnsErr_10001, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
            Exit Sub
        End If

        '終了ログ出力
        psLogEnd(Me)

        'Dim dt As DataTable = Nothing

        ''開始ログ出力
        'psLogStart(Me)

        'If Master.ppCount = "0" Then
        '    psMesBox(Me, "00005", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後)
        'Else
        '    '★2014/03/27 Hamamoto Edit Start
        '    If pfDLCSV(sCnsProgid + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"), "", DirectCast(ViewState("ds"), DataSet).Tables(0), False, Me) <> 0 Then
        '        psMesBox(Me, "10001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, Master.Master.ppLeftButton2.Text)
        '    End If
        '    'If pfCreateCsvFile(M_CSV_OUTPUT_DIR,
        '    '                   sCnsProgid + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv",
        '    '                   DirectCast(ViewState("ds"), DataSet), True) <> 0 Then
        '    '    psMesBox(Me, "10001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, Master.Master.ppLeftButton2.Text)
        '    'End If
        '    '★2014/03/27 Hamamoto Edit   End
        'End If

        ''終了ログ出力
        'psLogEnd(Me)

    End Sub

#End Region

#Region "グリッド非表示列削除処理"

    ''' <summary>
    ''' グリッド非表示列削除処理
    ''' </summary>
    ''' <param name="ipdtFromDate">対応年月From</param>
    ''' <param name="ipdtToDate">対応年月To</param>
    ''' <param name="opdstData">品質会議資料データ</param>
    ''' <remarks></remarks>
    Protected Sub msDel_Column(ByVal ipdtFromDate As Date, ByVal ipdtToDate As Date, ByRef opdstData As DataSet)
        opdstData.Tables(0).Columns.Remove("故障率")
        opdstData.Tables(0).Columns.Remove("コード")
        opdstData.Tables(0).Columns.Remove("システム区分")
        opdstData.Tables(0).Columns.Remove("システム区分名称")
        opdstData.Tables(0).Columns.Remove("システム名称")
        opdstData.Tables(0).Columns.Remove("番号")

        If ipdtToDate = Nothing Then
            '-----------------------------
            '2014/04/25 土岐　ここから
            '-----------------------------
            'To条件がない場合は6ヶ月表示
            ViewState(sCnsTitleDate) = ipdtFromDate.AddMonths(5)
            '-----------------------------
            '2014/04/25 土岐　ここまで
            '-----------------------------
            Exit Sub
        End If

        '-----------------------------
        '2014/04/25 土岐　ここから
        '-----------------------------
        'To条件の月表示（To条件がFrom条件の6ヶ月以降の場合は入力エラー）
        ViewState(sCnsTitleDate) = ipdtToDate
        '-----------------------------
        '2014/04/25 土岐　ここまで
        '-----------------------------
        If ipdtToDate < ipdtFromDate.AddMonths(5) Then
            opdstData.Tables(0).Columns.Remove("数量6")
        Else
            Exit Sub
        End If

        If ipdtToDate < ipdtFromDate.AddMonths(4) Then
            opdstData.Tables(0).Columns.Remove("数量5")
        Else
            Exit Sub
        End If

        If ipdtToDate < ipdtFromDate.AddMonths(3) Then
            opdstData.Tables(0).Columns.Remove("数量4")
        Else
            Exit Sub
        End If

        If ipdtToDate < ipdtFromDate.AddMonths(2) Then
            opdstData.Tables(0).Columns.Remove("数量3")
        Else
            Exit Sub
        End If

        If ipdtToDate < ipdtFromDate.AddMonths(1) Then
            opdstData.Tables(0).Columns.Remove("数量2")
        Else
            Exit Sub
        End If

    End Sub

#End Region

#Region "エラーチェック"
    ''' <summary>
    ''' 入力項目のエラーチェックを行う。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheck_Error()
        '対応年月必須チェック
        If Me.dftStartDt.ppFromText = "" And Me.dftStartDt.ppToText = "" Then
            Me.dftStartDt.psSet_ErrorNo(5001, "対応年月")
            Me.dftStartDt.ppDateBoxFrom.Focus()
            Return
        ElseIf Me.dftStartDt.ppFromText <> "" And Me.dftStartDt.ppToText <> "" Then
            If Me.dftStartDt.ppFromDate.AddMonths(6) <= Me.dftStartDt.ppToDate Then
                Me.dftStartDt.psSet_ErrorNo(6002, "終了対応年月", "開始対応年月から6ヶ月以内")
                Me.dftStartDt.ppDateBoxTo.Focus()
                Return
            End If
        ElseIf Me.dftStartDt.ppFromText <> "" And Me.dftStartDt.ppToText = "" Then
            If Date.Parse("9999/12").AddMonths(-5) < dftStartDt.ppFromDate Then
                Me.dftStartDt.ppToText = "9999/12"
            End If
        End If

    End Sub
#End Region

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
