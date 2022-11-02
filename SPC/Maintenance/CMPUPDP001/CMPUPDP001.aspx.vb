'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　保守料金明細作成
'*　ＰＧＭＩＤ：　CMPUPDP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.30　：　ＮＫＣ
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'CMPUPDP001-001     2015/05/22      加賀      料金データの存在しないシステムの一覧表示に対応 
'CMPUPDP001-002     2016/02/19      栗原      システム分類毎の帳票出力に変更　初期表示のレスポンス改善

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
#End Region

Public Class CMPUPDP001
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
    Const M_MY_DISP_ID = P_FUN_CMP & P_SCR_UPD & P_PAGE & "001"

    'ビューステート
    Const M_VIEW_DS_STATE = "ds_state"  '都道府県別データセット
    Const M_VIEW_DS_AREA = "ds_area"    'エリア別データセット
    Const M_VIEW_DS_LAN = "ds_lan"      'LAN単価別データセット
    Const M_VIEW_DS_PRINT = "print"     '印刷用データセット
    Const M_VIEW_AGG_DT = "dtbAggDt"    '集計日（Sessionを利用して帳票側でも使用）
    Const M_VIEW_TYPE_CNT = "TypeCount" 'TBOX種別の件数

    'エリアコード
    Const M_AREA_FS = "0"   'ＦＳエリア
    Const M_AREA_CS = "1"   'ＣＳエリア
    Const M_AREA_OTH = "9"  'その他エリア

    '次画面ファイルパス（保守対応依頼書）
    Const M_MNT_DISP_PATH = "~/" & P_MAI & "/" &
            P_FUN_CMP & P_SCR_LST & P_PAGE & "003" & "/" &
            P_FUN_CMP & P_SCR_LST & P_PAGE & "003.aspx"

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

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click
        AddHandler Master.Master.ppRigthButton2.Click, AddressOf btnCsv_Click
        AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnPrn_Click
        AddHandler Master.Master.ppLeftButton1.Click, AddressOf btnLeft1_Click

        If Not IsPostBack Then  '初回表示のみ

            '「検索条件クリア」押下時の検証を無効
            Master.ppRigthButton2.CausesValidation = False

            'プログラムID、画面名設定
            Master.Master.ppProgramID = M_MY_DISP_ID
            Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)

            'パンくずリスト設定
            Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

            '各コマンドボタンの属性設定
            '--印刷
            Master.Master.ppRigthButton1.CausesValidation = False
            Master.Master.ppRigthButton1.Text = P_BTN_NM_PRI
            Master.Master.ppRigthButton1.Visible = True
            Master.Master.ppRigthButton1.OnClientClick =
                pfGet_OCClickMes("10002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "保守料金明細一覧表")
            '--ＣＳＶ
            Master.Master.ppRigthButton2.CausesValidation = False
            Master.Master.ppRigthButton2.Text = "ＣＳＶ"
            Master.Master.ppRigthButton2.Visible = True
            '--保守料金明細
            Master.Master.ppLeftButton1.CausesValidation = False
            Master.Master.ppLeftButton1.Text = "保守料金明細"
            Master.Master.ppLeftButton1.Visible = True
            Master.Master.ppLeftButton1.Enabled = False

            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            '画面クリア
            msClearScreen()

        End If

    End Sub

    '---------------------------
    '2014/04/22 武 ここから
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
    '2014/04/22 武 ここまで
    '---------------------------

    ''' <summary>
    ''' 検索ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        'データ取得
        If (Page.IsValid) Then
            Session(M_VIEW_AGG_DT) = Me.dtbAggDt.ppText
            msGetData(1)
            '検索データがある場合はフッター部ボタンを活性化
            If Master.ppCount > "0" Then
                Master.Master.ppRigthButton1.Enabled = True
                Master.Master.ppRigthButton2.Enabled = True
                Master.Master.ppLeftButton1.Enabled = True
            Else
                Master.Master.ppRigthButton1.Enabled = False
                Master.Master.ppRigthButton2.Enabled = False
                Master.Master.ppLeftButton1.Enabled = False
            End If
        Else
            '入力された年月日以外を初期化
            ViewState(M_VIEW_AGG_DT) = Me.dtbAggDt.ppText
            msClearScreen()
            Me.dtbAggDt.ppText = ViewState(M_VIEW_AGG_DT)
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 検索条件クリア
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        '画面クリア
        msClearScreen()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' CSVボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCsv_Click(sender As Object, e As EventArgs)

        Dim objds0 As DataSet = Nothing 'データセット（LAN単価）
        Dim objDs1 As DataSet = Nothing 'データセット（都道府県別）
        Dim objDs2 As DataSet = Nothing 'データセット（エリア別）

        '開始ログ出力
        psLogStart(Me)

        If Master.ppCount = "0" Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
        Else
            objDs1 = ViewState(M_VIEW_DS_STATE)
            objDs2 = ViewState(M_VIEW_DS_AREA)
            objds0 = ViewState(M_VIEW_DS_LAN)

            'エリア別のエリアコード列を削除
            objDs2.Tables(0).Columns.RemoveAt(0)
            'LAN単価の合計行を削除
            'objds0.Tables(0).Rows.RemoveAt(objds0.Tables(0).Rows.Count - 1)

            'データテーブルのマージ（都道府県別とエリア別とLAN単価をマージ）
            objDs2.Tables(0).Columns(0).ColumnName = "都道府県名"
            objds0.Tables(0).Columns(0).ColumnName = "都道府県名"
            objDs1.Tables(0).Merge(objDs2.Tables(0))
            objDs1.Tables(0).Merge(objds0.Tables(0))

            'CSVファイルダウンロード
            If pfDLCsvFile(Master.Master.ppTitle + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv",
                           objDs1.Tables(0), True, Me) <> 0 Then
                psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
            End If
        End If

        'データ取得
        msGetData(1)

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 印刷ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPrn_Click(sender As Object, e As EventArgs)
        Dim objds0 As DataSet = Nothing
        Dim objDs1 As DataSet = Nothing 'データセット（都道府県別）
        Dim objDs2 As DataSet = Nothing 'データセット（エリア別）
        Dim FsCnt() As Integer = Nothing
        Dim FsPrice() As Integer = Nothing
        Dim CsCnt() As Integer = Nothing
        Dim CsPrice() As Integer = Nothing
        Dim OthCnt() As Integer = Nothing
        Dim OthPrice() As Integer = Nothing
        Dim intTypeCount As Integer = 0
        Dim WrkTable As DataTable = Nothing 'データテーブル（都道府県別、システム分類別）

        Dim dsPrint As New DataSet
        '開始ログ出力
        psLogStart(Me)

        '保守料金明細一覧表印刷
        '(注)Session変数はレポート側の処理で使用
        If Master.ppCount = "0" Then
            psMesBox(Me, "00007", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
        Else
            objDs1 = ViewState(M_VIEW_DS_STATE)
            objDs2 = ViewState(M_VIEW_DS_AREA)
            objds0 = ViewState(M_VIEW_DS_LAN)

            'セッション情報設定
            '集計日
            'Session(M_VIEW_AGG_DT) = Me.dtbAggDt.ppDate

            '--エリア別件数
            'Session("FsTtlCnt") = Me.lblFsAreaCount.Text  'ＦＳ件数
            'Session("CsTtlCnt") = Me.lblCsAreaCount.Text  'ＣＳ件数
            'Session("OthTtlCnt") = Me.lblOtAreaCount.Text 'その他件数

            'CMPUPDP001-002
            'プリント用データセットの取得
            dsPrint = mfGetPrintData()

            'TBOXタイプ件数設定
            intTypeCount = ViewState(M_VIEW_TYPE_CNT)
            '--エリア・TBOXタイプ別件数、金額
            ReDim FsCnt(intTypeCount - 1)
            ReDim FsPrice(intTypeCount - 1)
            ReDim CsCnt(intTypeCount - 1)
            ReDim CsPrice(intTypeCount - 1)
            ReDim OthCnt(intTypeCount - 1)
            ReDim OthPrice(intTypeCount - 1)
            With objDs2.Tables(0)
                For i = 0 To .Rows.Count - 1
                    For j = 0 To intTypeCount - 1
                        'ＦＳエリア
                        If .Rows(i).Item(0) = M_AREA_FS Then
                            FsCnt(j) = 0
                            FsPrice(j) = 0
                        End If
                        'ＣＳエリア
                        If .Rows(i).Item(0) = M_AREA_CS Then
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

            'システム区分別のTBOXタイプ種類数（総計はintTypeCount）
            '「都道府県名」と「_件数」「_金額」の重複を除いた種類数を保存（帳票側の処理で使用）
            Session("IDTypeCnt") = (dsPrint.Tables(0).Columns.Count - 1) / 2
            Session("ICTypeCnt") = (dsPrint.Tables(1).Columns.Count - 1) / 2
            Session("LTTypeCnt") = (dsPrint.Tables(2).Columns.Count - 1) / 2

            Dim rptID As New CMPREP001
            Dim rptIC As New CMPREP001
            Dim rptLT As New CMPREP001
            rptID.ppTitle = Master.Master.ppTitle + "一覧表　ID"
            rptIC.ppTitle = Master.Master.ppTitle + "一覧表　IC"
            rptLT.ppTitle = Master.Master.ppTitle + "一覧表　LUTERNA"

            'CMPUPDP001-002
            '帳票出力
            psPrintPDF(Me, {rptID, rptIC, rptLT}, {dsPrint.Tables(0), dsPrint.Tables(1), dsPrint.Tables(2)} _
                       , {Master.Master.ppTitle + "一覧表　ID", Master.Master.ppTitle + "一覧表　IC", Master.Master.ppTitle + "一覧表　LUTERNA"})
            'psPrintPDF(Me,  rpt, objDs1.Tables(0),Master.Master.ppTitle + "一覧表")
            'CMPUPDP001-002 END
        End If

        'データ取得
        msGetData(1)

        '終了ログ出力
        psLogEnd(Me)

    End Sub
    'CMPUPDP001-002
    ''' <summary>
    ''' 印刷用データの取得と編集
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetPrintData() As DataSet
        mfGetPrintData = Nothing
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim RtnDs As New DataSet
        Dim dsTbox As New DataSet
        '印刷用テーブル
        Dim dtIDMain As New DataTable
        Dim dtICMain As New DataTable
        Dim dtLUTEMain As New DataTable
        '都道府県別保守料金明細テーブル
        'TBOX毎に配列で分割
        Dim dtIDInfo() As DataTable
        Dim dtICInfo() As DataTable
        Dim dtLUTEInfo() As DataTable

        Dim strAggDt As String = Session(M_VIEW_AGG_DT)
        '取得したTBOXデータテーブルのインデックス番号（×システム分類の区分コード）
        Const intIDTblIndex As Integer = 1
        Const intICTblIndex As Integer = 2
        Const intLUTETblIndex As Integer = 3

        '作業用変数
        Dim drTemp As DataRow
        Dim intIndex As Integer
        Dim intState As Integer

        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S4", objCn)
                '保守料金登録（M37）のあるシステムの一覧取得
                dsTbox = clsDataConnect.pfGet_DataSet(objCmd)

                '***各テーブルに項目（列）の設定***
                dtIDMain.Columns.Add("都道府県名")
                dtICMain.Columns.Add("都道府県名")
                dtLUTEMain.Columns.Add("都道府県名")
                'TBOX毎に「_件数」「_金額」をそれぞれ設定
                For intIndex = 0 To dsTbox.Tables(intIDTblIndex).Rows.Count - 1
                    With dsTbox.Tables(intIDTblIndex).Rows(intIndex)
                        dtIDMain.Columns.Add(.Item("M03_TBOX_NM").ToString() + "_件数")
                        dtIDMain.Columns.Add(.Item("M03_TBOX_NM").ToString() + "_金額")
                    End With
                Next
                For intIndex = 0 To dsTbox.Tables(intICTblIndex).Rows.Count - 1
                    With dsTbox.Tables(intICTblIndex).Rows(intIndex)
                        dtICMain.Columns.Add(.Item("M03_TBOX_NM").ToString() + "_件数")
                        dtICMain.Columns.Add(.Item("M03_TBOX_NM").ToString() + "_金額")
                    End With
                Next
                For intIndex = 0 To dsTbox.Tables(intLUTETblIndex).Rows.Count - 1
                    With dsTbox.Tables(intLUTETblIndex).Rows(intIndex)
                        dtLUTEMain.Columns.Add(.Item("M03_TBOX_NM").ToString() + "_件数")
                        dtLUTEMain.Columns.Add(.Item("M03_TBOX_NM").ToString() + "_金額")
                    End With
                Next

                '集計結果管理用テーブル配列の設定（システムの種類数だけ用意）
                ReDim dtIDInfo(dsTbox.Tables(intIDTblIndex).Rows.Count - 1)
                ReDim dtICInfo(dsTbox.Tables(intICTblIndex).Rows.Count - 1)
                ReDim dtLUTEInfo(dsTbox.Tables(intLUTETblIndex).Rows.Count - 1)

                '集計結果をテーブル配列に挿入
                For intIndex = 0 To dtIDInfo.Count - 1
                    objCmd = New SqlCommand(M_MY_DISP_ID + "_S5", objCn)
                    With objCmd.Parameters  '--パラメータ設定
                        .Add(pfSet_Param("AggDate", SqlDbType.NVarChar, strAggDt.Replace("/", "")))
                        .Add(pfSet_Param("syscd", SqlDbType.NVarChar, dsTbox.Tables(intIDTblIndex).Rows(intIndex).Item("M03_TBOX_CD").ToString))
                    End With
                    dtIDInfo(intIndex) = clsDataConnect.pfGet_DataSet(objCmd).Tables(0)
                Next

                For intIndex = 0 To dtICInfo.Count - 1
                    objCmd = New SqlCommand(M_MY_DISP_ID + "_S5", objCn)
                    With objCmd.Parameters  '--パラメータ設定
                        .Add(pfSet_Param("AggDate", SqlDbType.NVarChar, strAggDt.Replace("/", "")))
                        .Add(pfSet_Param("syscd", SqlDbType.NVarChar, dsTbox.Tables(intICTblIndex).Rows(intIndex).Item("M03_TBOX_CD").ToString))
                    End With
                    dtICInfo(intIndex) = clsDataConnect.pfGet_DataSet(objCmd).Tables(0)
                Next

                For intIndex = 0 To dtLUTEInfo.Count - 1
                    objCmd = New SqlCommand(M_MY_DISP_ID + "_S5", objCn)
                    With objCmd.Parameters  '--パラメータ設定
                        .Add(pfSet_Param("AggDate", SqlDbType.NVarChar, strAggDt.Replace("/", "")))
                        .Add(pfSet_Param("syscd", SqlDbType.NVarChar, dsTbox.Tables(intLUTETblIndex).Rows(intIndex).Item("M03_TBOX_CD").ToString))
                    End With
                    dtLUTEInfo(intIndex) = clsDataConnect.pfGet_DataSet(objCmd).Tables(0)
                Next

                '印刷用のテーブルに集計結果を挿入
                For intIndex = 0 To dtIDInfo(0).Rows.Count - 1
                    drTemp = dtIDMain.NewRow()
                    drTemp("都道府県名") = dtIDInfo(0).Rows(intIndex).Item("都道府県名").ToString
                    For intState = 0 To dsTbox.Tables(intIDTblIndex).Rows.Count - 1
                        If dtIDInfo(intState).Rows.Count = 0 Then
                            drTemp(dsTbox.Tables(intIDTblIndex).Rows(intState).Item("M03_TBOX_NM").ToString() + "_件数") = "0"
                            drTemp(dsTbox.Tables(intIDTblIndex).Rows(intState).Item("M03_TBOX_NM").ToString() + "_金額") = "0"
                        Else
                            drTemp(dsTbox.Tables(intIDTblIndex).Rows(intState).Item("M03_TBOX_NM").ToString() + "_件数") = Decimal.Parse(dtIDInfo(intState).Rows(intIndex).Item("D96_CNT_SUU")).ToString("#,0")
                            drTemp(dsTbox.Tables(intIDTblIndex).Rows(intState).Item("M03_TBOX_NM").ToString() + "_金額") = Decimal.Parse(dtIDInfo(intState).Rows(intIndex).Item("D96_CNT_AMT")).ToString("#,0")
                        End If
                    Next
                    dtIDMain.Rows.Add(drTemp)
                Next
                For intIndex = 0 To dtICInfo(0).Rows.Count - 1
                    drTemp = dtICMain.NewRow()
                    drTemp("都道府県名") = dtICInfo(0).Rows(intIndex).Item("都道府県名").ToString
                    For intState = 0 To dsTbox.Tables(intICTblIndex).Rows.Count - 1
                        If dtICInfo(intState).Rows.Count = 0 Then
                            drTemp(dsTbox.Tables(intICTblIndex).Rows(intState).Item("M03_TBOX_NM").ToString() + "_件数") = "0"
                            drTemp(dsTbox.Tables(intICTblIndex).Rows(intState).Item("M03_TBOX_NM").ToString() + "_金額") = "0"
                        Else
                            drTemp(dsTbox.Tables(intICTblIndex).Rows(intState).Item("M03_TBOX_NM").ToString() + "_件数") = Decimal.Parse(dtICInfo(intState).Rows(intIndex).Item("D96_CNT_SUU")).ToString("#,0")
                            drTemp(dsTbox.Tables(intICTblIndex).Rows(intState).Item("M03_TBOX_NM").ToString() + "_金額") = Decimal.Parse(dtICInfo(intState).Rows(intIndex).Item("D96_CNT_AMT")).ToString("#,0")
                        End If
                    Next
                    dtICMain.Rows.Add(drTemp)
                Next
                For intIndex = 0 To dtLUTEInfo(0).Rows.Count - 1
                    drTemp = dtLUTEMain.NewRow()
                    drTemp("都道府県名") = dtLUTEInfo(0).Rows(intIndex).Item("都道府県名").ToString
                    For intState = 0 To dsTbox.Tables(intLUTETblIndex).Rows.Count - 1
                        If dtLUTEInfo(intState).Rows.Count = 0 Then
                            drTemp(dsTbox.Tables(intLUTETblIndex).Rows(intState).Item("M03_TBOX_NM").ToString() + "_件数") = "0"
                            drTemp(dsTbox.Tables(intLUTETblIndex).Rows(intState).Item("M03_TBOX_NM").ToString() + "_金額") = "0"
                        Else
                            drTemp(dsTbox.Tables(intLUTETblIndex).Rows(intState).Item("M03_TBOX_NM").ToString() + "_件数") = Decimal.Parse(dtLUTEInfo(intState).Rows(intIndex).Item("D96_CNT_SUU")).ToString("#,0")
                            drTemp(dsTbox.Tables(intLUTETblIndex).Rows(intState).Item("M03_TBOX_NM").ToString() + "_金額") = Decimal.Parse(dtLUTEInfo(intState).Rows(intIndex).Item("D96_CNT_AMT")).ToString("#,0")
                        End If
                    Next
                    dtLUTEMain.Rows.Add(drTemp)
                Next

                '戻り値用データセットの作成
                RtnDs.Tables.Add(dtIDMain)
                RtnDs.Tables.Add(dtICMain)
                RtnDs.Tables.Add(dtLUTEMain)
                mfGetPrintData = RtnDs

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守料金明細作成")
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
    'CMPUPDP001-002 END
    ''' <summary>
    ''' 保守料金明細ボタンクリック時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnLeft1_Click(sender As System.Object, e As EventArgs)

        Try
            '画面「保守料金明細」へ遷移
            Dim strDispID As String = "CMPLSTP003"
            Dim strPath As String = "~/Maintenance/" & strDispID & "/" & strDispID & ".aspx"
            Dim strKeyList() As String = {dtbAggDt.ppText}

            'セッション情報共通部設定
            Session(P_KEY) = strKeyList

            psOpen_Window(Me, strPath)

            msGetData(1)

        Catch ex As Exception

            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)

        End Try


    End Sub
#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            Me.dtbAggDt.ppText = String.Empty
            Me.lblFsAreaCount.Text = String.Empty
            Me.lblFsAreaAmount.Text = String.Empty
            Me.lblCsAreaCount.Text = String.Empty
            Me.lblCsAreaAmount.Text = String.Empty
            Me.lblOtAreaCount.Text = String.Empty
            Me.lblOtAreaAmount.Text = String.Empty
            Me.lblTotalAreaCount.Text = String.Empty
            Me.lblTotalAreaAmount.Text = String.Empty
            Master.Master.ppRigthButton1.Enabled = False
            Master.Master.ppRigthButton2.Enabled = False
            Master.Master.ppLeftButton1.Enabled = False
            'CMPUPDM001-002
            'msGetData(0)
            msGridInit()
            'CMPUPDM001-002 END
            Me.dtbAggDt.ppDateBox.Focus()

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
        End Try

    End Sub

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <param name="intMode"></param>
    ''' <remarks></remarks>
    Private Sub msGetData(ByVal intMode As Integer)

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs0 As New DataSet               'データセット（LAN単価用）
        Dim objDs1 As New DataSet               'データセット（都道府県別）
        Dim objDs2 As New DataSet               'データセット（エリア別）
        Dim objDs3 As New DataSet               'データセット（エリア別合計）
        Dim strBuff As String = String.Empty    '文字列バッファ
        Dim intTypeCount As Integer = 0         'TBOXタイプ件数
        'Dim dsState As DataSet
        Dim dsStateDt() As DataSet
        Dim dsStateDt2() As DataSet
        Dim dtData As New DataTable
        Dim dtData2 As New DataTable
        Dim drData As DataRow
        Dim intIndex As Integer
        Dim intState As Integer
        Dim intTtlCnt As Integer
        Dim intTtlAmt As Integer
        Dim strAggDt As String = Session(M_VIEW_AGG_DT)

        objStack = New StackFrame

        'グリッド及び件数の初期化
        Me.grvListState.DataSource = New DataTable
        Me.grvListState.DataBind()
        Me.grvListSummary.DataSource = New DataTable
        Me.grvListSummary.DataBind()
        Me.grvListLan.DataSource = New DataTable
        Me.grvListLan.DataBind()
        Me.lblFsAreaCount.Text = "0"
        Me.lblFsAreaAmount.Text = "0"
        Me.lblCsAreaCount.Text = "0"
        Me.lblCsAreaAmount.Text = "0"
        Me.lblOtAreaCount.Text = "0"
        Me.lblOtAreaAmount.Text = "0"
        Me.lblTotalAreaCount.Text = "0"
        Me.lblTotalAreaAmount.Text = "0"
        Master.ppCount = "0"

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                '*****************************/
                '*  都道府県・TBOXタイプ別   */
                '*****************************/

                '都道府県一覧　取得
                Dim dsState As New DataSet
                Dim dsTBOXLIST As New DataSet
                objCmd = New SqlCommand("CMPINQP002_S27", objCn)
                dsState = clsDataConnect.pfGet_DataSet(objCmd)

                objCmd = New SqlCommand(M_MY_DISP_ID + "_S4", objCn)
                dsTBOXLIST = clsDataConnect.pfGet_DataSet(objCmd)

                dtData.Columns.Add("都道府県名")
                For intIndex = 0 To dsTBOXLIST.Tables(0).Rows.Count - 1
                    With dsTBOXLIST.Tables(0).Rows(intIndex)
                        dtData.Columns.Add(.Item("M03_TBOX_NM").ToString() + "_件数")
                        dtData.Columns.Add(.Item("M03_TBOX_NM").ToString() + "_金額")
                    End With
                Next

                ReDim dsStateDt(dsTBOXLIST.Tables(0).Rows.Count - 1)

                For intIndex = 0 To dsTBOXLIST.Tables(0).Rows.Count - 1
                    With dsTBOXLIST.Tables(0).Rows(intIndex)
                        objCmd = New SqlCommand(M_MY_DISP_ID + "_S5", objCn)
                        With objCmd.Parameters  '--パラメータ設定
                            .Add(pfSet_Param("AggDate", SqlDbType.NVarChar, strAggDt.Replace("/", "")))
                            .Add(pfSet_Param("syscd", SqlDbType.NVarChar, dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_CD").ToString))
                        End With
                        dsStateDt(intIndex) = clsDataConnect.pfGet_DataSet(objCmd)
                    End With
                Next

                '都道府県でループ
                Dim zz As Integer
                Dim yy As Integer = 0

                For zz = 0 To dsState.Tables(0).Rows.Count - 1

                    drData = dtData.NewRow()
                    drData("都道府県名") = dsState.Tables(0).Rows(zz).Item("M12_STATE_NM") 'dsTBOX_PreFuct(0).Tables(0).Rows(intIndex).Item("都道府県名").ToString
                    Debug.WriteLine(dsState.Tables(0).Rows(zz).Item("M12_STATE_NM"))
                    Dim blnMatch As Boolean = False
                    'ＴＢＯＸ種別でループ
                    'For intIndex = 0 To dsTBOX_PreFuct(0).Tables(0).Rows.Count - 1
                    For intIndex = 0 To dsTBOXLIST.Tables(0).Rows.Count - 1

                        blnMatch = False

                        If dsStateDt(intIndex).Tables(0).Rows.Count > 0 Then
                            For yy = 0 To dsStateDt(intIndex).Tables(0).Rows.Count - 1
                                If dsState.Tables(0).Rows(zz).Item("M12_STATE_NM").ToString = dsStateDt(intIndex).Tables(0).Rows(yy).Item("都道府県名").ToString Then
                                    Debug.WriteLine("　" & dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_NM").ToString)
                                    Debug.WriteLine("　　" & dsStateDt(intIndex).Tables(0).Rows(yy).Item("都道府県名").ToString)
                                    drData(dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_NM").ToString() + "_件数") = Decimal.Parse(dsStateDt(intIndex).Tables(0).Rows(yy).Item("D96_CNT_SUU")).ToString("#,0")
                                    drData(dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_NM").ToString() + "_金額") = Decimal.Parse(dsStateDt(intIndex).Tables(0).Rows(yy).Item("D96_SPC_AMT")).ToString("#,0")
                                    blnMatch = True
                                End If
                            Next
                            If blnMatch = False Then
                                Debug.WriteLine("　" & dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_NM").ToString)
                                Debug.WriteLine("　　" & dsState.Tables(0).Rows(zz).Item("M12_STATE_NM").ToString)
                                drData(dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_NM").ToString() + "_件数") = "0"
                                drData(dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_NM").ToString() + "_金額") = "0"
                            End If
                        Else
                            Debug.WriteLine("　" & dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_NM").ToString)
                            Debug.WriteLine("　　" & dsState.Tables(0).Rows(zz).Item("M12_STATE_NM").ToString)
                            drData(dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_NM").ToString() + "_件数") = "0"
                            drData(dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_NM").ToString() + "_金額") = "0"
                        End If
                    Next
                    dtData.Rows.Add(drData)
                Next

                objDs1.Tables.Add(dtData)



                'For intIndex = 0 To dsStateDt(0).Tables(0).Rows.Count - 1
                '    drData = dtData.NewRow()
                '    drData("都道府県名") = dsStateDt(0).Tables(0).Rows(intIndex).Item("都道府県名").ToString
                '    For intState = 0 To dsState.Tables(0).Rows.Count - 1
                '        With dsState.Tables(0).Rows(intState)
                '            If dsStateDt(intState).Tables(0).Rows.Count = 0 Then 'CMPUPDP001-001
                '                drData(.Item("M03_TBOX_NM").ToString() + "_件数") = "0"
                '                drData(.Item("M03_TBOX_NM").ToString() + "_金額") = "0"
                '            Else                                                 'CMPUPDP001-001 END
                '                drData(.Item("M03_TBOX_NM").ToString() + "_件数") = Decimal.Parse(dsStateDt(intState).Tables(0).Rows(intIndex).Item("D96_CNT_SUU")).ToString("#,0")
                '                drData(.Item("M03_TBOX_NM").ToString() + "_金額") = Decimal.Parse(dsStateDt(intState).Tables(0).Rows(intIndex).Item("D96_CNT_AMT")).ToString("#,0")
                '            End If
                '        End With
                '    Next
                '    dtData.Rows.Add(drData)
                'Next
                'objDs1.Tables.Add(dtData)

                'データ件数表示
                Master.ppCount = objDs1.Tables(0).Rows.Count

                'データをグリッドに設定
                Me.grvListState.DataSource = objDs1.Tables(0)

                'ビューステートにデータセットを格納
                ViewState(M_VIEW_DS_STATE) = objDs1

                '変更を反映
                Me.grvListState.DataBind()

                '****************************/
                '*  エリア・TBOXタイプ別    */
                '****************************/
                Dim decSuu As Decimal
                Dim decAmt As Decimal
                Dim decSpcSuu As Decimal
                Dim decSpcAmt As Decimal

                dtData = Nothing
                dtData = New DataTable()

                dtData.Columns.Add("エリアコード")
                dtData.Columns.Add("エリア")
                For intIndex = 0 To dsTBOXLIST.Tables(0).Rows.Count - 1
                    With dsTBOXLIST.Tables(0).Rows(intIndex)
                        dtData.Columns.Add(.Item("M03_TBOX_NM").ToString() + "_件数")
                        dtData.Columns.Add(.Item("M03_TBOX_NM").ToString() + "_金額")
                    End With
                Next

                ReDim dsStateDt(dsTBOXLIST.Tables(0).Rows.Count - 1)
                ReDim dsStateDt2(dsTBOXLIST.Tables(0).Rows.Count - 1)

                For intIndex = 0 To dsTBOXLIST.Tables(0).Rows.Count - 1
                    With dsTBOXLIST.Tables(0).Rows(intIndex)
                        objCmd = New SqlCommand(M_MY_DISP_ID + "_S6", objCn)
                        With objCmd.Parameters  '--パラメータ設定
                            .Add(pfSet_Param("AggDate", SqlDbType.NVarChar, strAggDt.Replace("/", "")))
                            .Add(pfSet_Param("syscd", SqlDbType.NVarChar, dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_CD").ToString))
                        End With
                        dsStateDt(intIndex) = clsDataConnect.pfGet_DataSet(objCmd)
                    End With
                Next

                decSpcSuu = 0
                decSpcAmt = 0
                For intIndex = 0 To dsStateDt(0).Tables(0).Rows.Count - 1
                    drData = dtData.NewRow()
                    decSuu = 0
                    decAmt = 0
                    drData("エリアコード") = dsStateDt(0).Tables(0).Rows(intIndex).Item("D97_ARE_COD").ToString
                    drData("エリア") = dsStateDt(0).Tables(0).Rows(intIndex).Item("エリア名").ToString
                    For intState = 0 To dsTBOXLIST.Tables(0).Rows.Count - 1
                        With dsTBOXLIST.Tables(0).Rows(intState)
                            If dsStateDt(intState).Tables(0).Rows.Count = 0 Then 'CMPUPDP001-001
                                drData(.Item("M03_TBOX_NM").ToString() + "_件数") = "0"
                                drData(.Item("M03_TBOX_NM").ToString() + "_金額") = "0"
                            Else                                                 'CMPUPDP001-001 END
                                drData(.Item("M03_TBOX_NM").ToString() + "_件数") = Decimal.Parse(dsStateDt(intState).Tables(0).Rows(intIndex).Item("D97_CNT_SUU")).ToString("#,0")
                                If Decimal.Parse(dsStateDt(0).Tables(0).Rows(intIndex).Item("D97_ARE_COD").ToString) >= 0 Then
                                    drData(.Item("M03_TBOX_NM").ToString() + "_金額") = Decimal.Parse(dsStateDt(intState).Tables(0).Rows(intIndex).Item("D97_CNT_AMT")).ToString("#,0")
                                Else
                                    drData(.Item("M03_TBOX_NM").ToString() + "_金額") = Decimal.Parse(dsStateDt(intState).Tables(0).Rows(intIndex).Item("D97_SPC_AMT")).ToString("#,0")
                                End If

                                decSuu += Decimal.Parse(dsStateDt(intState).Tables(0).Rows(intIndex).Item("D97_CNT_SUU").ToString)
                                decAmt += Decimal.Parse(dsStateDt(intState).Tables(0).Rows(intIndex).Item("D97_CNT_AMT").ToString)
                                decSpcSuu += Decimal.Parse(dsStateDt(intState).Tables(0).Rows(intIndex).Item("D97_CNT_SUU").ToString)
                                decSpcAmt += Decimal.Parse(dsStateDt(intState).Tables(0).Rows(intIndex).Item("D97_SPC_AMT").ToString)
                            End If
                        End With
                    Next

                    'データ表示
                    Select Case dsStateDt(0).Tables(0).Rows(intIndex).Item("D97_ARE_COD")
                        Case M_AREA_FS  'ＦＳエリア
                            Me.lblFsAreaCount.Text = decSuu.ToString("#,0")
                            Me.lblFsAreaAmount.Text = decAmt.ToString("#,0")
                        Case M_AREA_CS  'ＣＳエリア
                            Me.lblCsAreaCount.Text = decSuu.ToString("#,0")
                            Me.lblCsAreaAmount.Text = decAmt.ToString("#,0")
                        Case M_AREA_OTH 'その他エリア
                            Me.lblOtAreaCount.Text = decSuu.ToString("#,0")
                            Me.lblOtAreaAmount.Text = decAmt.ToString("#,0")
                        Case Else       '総合計
                            Me.lblTotalAreaCount.Text = decSuu.ToString("#,0")
                            Me.lblTotalAreaAmount.Text = decSpcAmt.ToString("#,0")
                    End Select

                    dtData.Rows.Add(drData)
                Next
                objDs2.Tables.Add(dtData)

                'データをグリッドに設定
                objDs2.Tables(0).Columns(1).ColumnName = " "
                Me.grvListSummary.DataSource = objDs2.Tables(0)

                'ビューステートにデータセットを格納
                ViewState(M_VIEW_DS_AREA) = objDs2

                '変更を反映
                Me.grvListSummary.DataBind()

                '****************************/
                '*  LAN単価                 */
                '****************************/

                For intIndex = 0 To dsTBOXLIST.Tables(0).Rows.Count - 1
                    With dsTBOXLIST.Tables(0).Rows(intIndex)
                        objCmd = New SqlCommand(M_MY_DISP_ID + "_S7", objCn)
                        With objCmd.Parameters  '--パラメータ設定
                            .Add(pfSet_Param("AggDate", SqlDbType.NVarChar, strAggDt.Replace("/", "")))
                            .Add(pfSet_Param("syscd", SqlDbType.NVarChar, dsTBOXLIST.Tables(0).Rows(intIndex).Item("M03_TBOX_CD").ToString))
                        End With
                        dsStateDt2(intIndex) = clsDataConnect.pfGet_DataSet(objCmd)
                    End With
                Next

                objCmd = New SqlCommand(M_MY_DISP_ID + "_S4", objCn)
                dsState = clsDataConnect.pfGet_DataSet(objCmd)
                dtData2.Columns.Add("都道府県名")
                For intIndex = 0 To dsTBOXLIST.Tables(0).Rows.Count - 1
                    With dsTBOXLIST.Tables(0).Rows(intIndex)
                        dtData2.Columns.Add(.Item("M03_TBOX_NM").ToString() + "_件数")
                        dtData2.Columns.Add(.Item("M03_TBOX_NM").ToString() + "_金額")
                    End With
                Next

                For intIndex = 0 To dsStateDt2(0).Tables(0).Rows.Count - 1
                    If intIndex = 0 Then  '最初の行に合計行を追加
                        drData = dtData2.NewRow()
                        drData("都道府県名") = "LAN機器総合計"
                        For intState = 0 To dsTBOXLIST.Tables(0).Rows.Count - 1
                            intTtlCnt = 0
                            intTtlAmt = 0
                            For introw As Integer = 0 To dsStateDt2(intState).Tables(0).Rows.Count - 1
                                intTtlCnt += Decimal.Parse(dsStateDt2(intState).Tables(0).Rows(introw).Item("D96_CNT_SUU")).ToString()
                                intTtlAmt += Decimal.Parse(dsStateDt2(intState).Tables(0).Rows(introw).Item("D96_SPC_AMT")).ToString()
                            Next
                            With dsTBOXLIST.Tables(0).Rows(intState)
                                drData(.Item("M03_TBOX_NM").ToString() + "_件数") = intTtlCnt
                                drData(.Item("M03_TBOX_NM").ToString() + "_金額") = intTtlAmt
                            End With
                        Next
                        dtData2.Rows.Add(drData)
                    End If
                    drData = dtData2.NewRow()
                    drData("都道府県名") = dsStateDt2(0).Tables(0).Rows(intIndex).Item("都道府県名").ToString
                    For intState = 0 To dsState.Tables(0).Rows.Count - 1
                        With dsTBOXLIST.Tables(0).Rows(intState)
                            drData(.Item("M03_TBOX_NM").ToString() + "_件数") = Decimal.Parse(dsStateDt2(intState).Tables(0).Rows(intIndex).Item("D96_CNT_SUU")).ToString
                            drData(.Item("M03_TBOX_NM").ToString() + "_金額") = Decimal.Parse(dsStateDt2(intState).Tables(0).Rows(intIndex).Item("D96_SPC_AMT")).ToString
                        End With
                    Next
                    dtData2.Rows.Add(drData)
                Next

                objDs0.Tables.Add(dtData2)

                'データをグリッドに設定
                objDs0.Tables(0).Columns(0).ColumnName = " "
                Me.grvListLan.DataSource = objDs0.Tables(0)

                'ビューステートにデータセットを格納
                ViewState(M_VIEW_DS_LAN) = objDs0

                '変更を反映
                Me.grvListLan.DataBind()



                ''*****************************/
                ''*  都道府県・TBOXタイプ別   */
                ''*****************************/
                'objCmd = New SqlCommand(M_MY_DISP_ID + "_S1", objCn)
                'With objCmd.Parameters
                '    '--パラメータ設定
                '    '集計日
                '    .Add(pfSet_Param("AggDate", SqlDbType.NVarChar, Me.dtbAggDt.ppText))
                '    'データ抽出モード（0:抽出しない、1:抽出する）
                '    .Add(pfSet_Param("Mode", SqlDbType.NVarChar, intMode))
                'End With
                ''データ取得
                'objDs1 = clsDataConnect.pfGet_DataSet(objCmd)

                ''データ件数表示
                'Master.ppCount = objDs1.Tables(0).Rows.Count

                ''データをグリッドに設定
                'Me.grvListState.DataSource = objDs1.Tables(0)

                ''ビューステートにデータセットを格納
                'ViewState(M_VIEW_DS_STATE) = objDs1

                ''変更を反映
                'Me.grvListState.DataBind()

                'CMPUPDP001-002
                'TBOXタイプ件数設定
                intTypeCount = (objDs1.Tables(0).Columns.Count - 1) / 2
                ViewState(M_VIEW_TYPE_CNT) = intTypeCount
                'CMPUPDP001-002 END

                ''****************************/
                ''*  エリア・TBOXタイプ別    */
                ''****************************/
                'objCmd = New SqlCommand(M_MY_DISP_ID + "_S2", objCn)
                'With objCmd.Parameters
                '    '--パラメータ設定
                '    '集計日
                '    .Add(pfSet_Param("AggDate", SqlDbType.NVarChar, Me.dtbAggDt.ppText))
                '    'データ抽出モード（0:抽出しない、1:抽出する）
                '    .Add(pfSet_Param("Mode", SqlDbType.NVarChar, intMode))
                'End With
                ''データ取得
                'objDs2 = clsDataConnect.pfGet_DataSet(objCmd)

                ''データをグリッドに設定
                'objDs2.Tables(0).Columns(1).ColumnName = " "
                'Me.grvListSummary.DataSource = objDs2.Tables(0)

                ''ビューステートにデータセットを格納
                'ViewState(M_VIEW_DS_AREA) = objDs2

                ''変更を反映
                'Me.grvListSummary.DataBind()

                ''******************/
                ''*  エリア別合計  */
                ''******************/
                'objCmd = New SqlCommand(M_MY_DISP_ID + "_S3", objCn)
                'With objCmd.Parameters
                '    '--パラメータ設定
                '    '集計日
                '    .Add(pfSet_Param("AggDate", SqlDbType.NVarChar, Me.dtbAggDt.ppText))
                'End With
                ''データ取得
                'objDs3 = clsDataConnect.pfGet_DataSet(objCmd)
                ''データ表示
                'For i = 0 To objDs3.Tables(0).Rows.Count - 1
                '    Select Case objDs3.Tables(0).Rows(i).Item("エリアコード")
                '        Case M_AREA_FS  'ＦＳエリア
                '            Me.lblFsAreaCount.Text = objDs3.Tables(0).Rows(i).Item("エリア件数")
                '            Me.lblFsAreaAmount.Text = objDs3.Tables(0).Rows(i).Item("エリア金額")
                '        Case M_AREA_CS  'ＣＳエリア
                '            Me.lblCsAreaCount.Text = objDs3.Tables(0).Rows(i).Item("エリア件数")
                '            Me.lblCsAreaAmount.Text = objDs3.Tables(0).Rows(i).Item("エリア金額")
                '        Case M_AREA_OTH 'その他エリア
                '            Me.lblOtAreaCount.Text = objDs3.Tables(0).Rows(i).Item("エリア件数")
                '            Me.lblOtAreaAmount.Text = objDs3.Tables(0).Rows(i).Item("エリア金額")
                '    End Select
                'Next

                'グリッドレイアウト整形
                '--都道府県別
                Me.grvListState.HeaderStyle.Width = 100 + (intTypeCount * 130)
                Me.grvListState.Width = 100 + (intTypeCount * 130)
                Me.grvListState.HeaderRow.Cells(0).Width = 97
                For i = 1 To intTypeCount
                    Me.grvListState.HeaderRow.Cells.RemoveAt(i + 1)
                    Me.grvListState.HeaderRow.Cells(i).ColumnSpan = 2
                    Me.grvListState.HeaderRow.Cells(i).Width = 127
                    Me.grvListState.HeaderRow.Cells(i).Text = Me.grvListState.HeaderRow.Cells(i).Text.Replace("_件数", "")
                Next
                If Me.grvListState.Rows.Count > 0 Then
                    For i = 0 To Me.grvListState.Rows.Count - 1
                        Me.grvListState.Rows(i).Cells(0).Width = 97
                        Me.grvListState.Rows(i).Cells(0).HorizontalAlign = HorizontalAlign.Left
                        For j = 1 To Me.grvListState.Rows(i).Cells.Count - 1 Step 2
                            Me.grvListState.Rows(i).Cells(j).Width = 47
                            Me.grvListState.Rows(i).Cells(j + 1).Width = 77
                            Me.grvListState.Rows(i).Cells(j).HorizontalAlign = HorizontalAlign.Right
                            Me.grvListState.Rows(i).Cells(j + 1).HorizontalAlign = HorizontalAlign.Right
                        Next
                    Next
                End If
                '--エリア別
                Me.grvListSummary.HeaderStyle.Width = 100 + (intTypeCount * 130)
                Me.grvListSummary.Width = 100 + (intTypeCount * 130)
                'エリアコードは非表示
                Me.grvListSummary.HeaderRow.Cells(0).Visible = False
                'エリア名
                Me.grvListSummary.HeaderRow.Cells(1).Width = 97
                'TBOXタイプ毎の台数、金額
                For i = 1 To intTypeCount
                    Me.grvListSummary.HeaderRow.Cells.RemoveAt(i + 2)
                    Me.grvListSummary.HeaderRow.Cells(i + 1).ColumnSpan = 2
                    Me.grvListSummary.HeaderRow.Cells(i + 1).Width = 127
                    Me.grvListSummary.HeaderRow.Cells(i + 1).Text = Me.grvListSummary.HeaderRow.Cells(i + 1).Text.Replace("_件数", "")
                Next
                If Me.grvListSummary.Rows.Count > 0 Then
                    For i = 0 To Me.grvListSummary.Rows.Count - 1
                        'エリアコードは非表示
                        Me.grvListSummary.Rows(i).Cells(0).Visible = False
                        'エリア名
                        Me.grvListSummary.Rows(i).Cells(1).Width = 97
                        Me.grvListSummary.Rows(i).Cells(1).HorizontalAlign = HorizontalAlign.Left
                        'TBOXタイプ毎の台数、金額
                        For j = 2 To Me.grvListSummary.Rows(i).Cells.Count - 2 Step 2
                            Me.grvListSummary.Rows(i).Cells(j).Width = 47
                            Me.grvListSummary.Rows(i).Cells(j + 1).Width = 77
                            Me.grvListSummary.Rows(i).Cells(j).HorizontalAlign = HorizontalAlign.Right
                            Me.grvListSummary.Rows(i).Cells(j + 1).HorizontalAlign = HorizontalAlign.Right
                        Next
                    Next
                End If

                'LAN単価
                Me.grvListLan.HeaderStyle.Width = 100 + (intTypeCount * 130)
                Me.grvListLan.Width = 100 + (intTypeCount * 130)
                Me.grvListLan.HeaderRow.Cells(0).Width = 97
                For i = 1 To intTypeCount
                    Me.grvListLan.HeaderRow.Cells.RemoveAt(i + 1)
                    Me.grvListLan.HeaderRow.Cells(i).ColumnSpan = 2
                    Me.grvListLan.HeaderRow.Cells(i).Width = 127
                    Me.grvListLan.HeaderRow.Cells(i).Text = Me.grvListState.HeaderRow.Cells(i).Text.Replace("_件数", "")
                Next
                If Me.grvListLan.Rows.Count > 0 Then
                    For i = 0 To Me.grvListLan.Rows.Count - 1
                        Me.grvListLan.Rows(i).Cells(0).Width = 97
                        Me.grvListLan.Rows(i).Cells(0).HorizontalAlign = HorizontalAlign.Left
                        For j = 1 To Me.grvListLan.Rows(i).Cells.Count - 1 Step 2
                            Me.grvListLan.Rows(i).Cells(j).Width = 47
                            Me.grvListLan.Rows(i).Cells(j + 1).Width = 77
                            Me.grvListLan.Rows(i).Cells(j).HorizontalAlign = HorizontalAlign.Right
                            Me.grvListLan.Rows(i).Cells(j + 1).HorizontalAlign = HorizontalAlign.Right
                        Next
                    Next
                End If

                If intMode <> 0 Then
                    If objDs2.Tables(0).Rows.Count = 0 Then
                        '0件
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        Master.ppCount = objDs2.Tables(0).Rows.Count.ToString
                    End If
                End If

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守料金明細作成")
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

    End Sub
    'CMPUPDP001-002
    ''' <summary>
    ''' グリッド欄初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGridInit()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim intTypeCount As Integer = 0         'TBOXタイプ件数
        Dim dsState As DataSet                  'データ取得用
        Dim dtData As New DataTable
        Dim intIndex As Integer

        objStack = New StackFrame

        'グリッド及び件数の初期化
        Me.lblFsAreaCount.Text = "0"
        Me.lblFsAreaAmount.Text = "0"
        Me.lblCsAreaCount.Text = "0"
        Me.lblCsAreaAmount.Text = "0"
        Me.lblOtAreaCount.Text = "0"
        Me.lblOtAreaAmount.Text = "0"
        Me.lblTotalAreaCount.Text = "0"
        Me.lblTotalAreaAmount.Text = "0"
        Master.ppCount = "0"

        ViewState(M_VIEW_DS_STATE) = Nothing
        ViewState(M_VIEW_DS_AREA) = Nothing
        ViewState(M_VIEW_DS_LAN) = Nothing
        ViewState(M_VIEW_TYPE_CNT) = Nothing

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                'Tbox情報取得
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S4", objCn)
                dsState = clsDataConnect.pfGet_DataSet(objCmd)

                dtData.Columns.Add("都道府県名")
                For intIndex = 0 To dsState.Tables(0).Rows.Count - 1
                    With dsState.Tables(0).Rows(intIndex)
                        dtData.Columns.Add(.Item("M03_TBOX_NM").ToString() + "_件数")
                        dtData.Columns.Add(.Item("M03_TBOX_NM").ToString() + "_金額")
                    End With
                Next
                '*****************************/
                '*  都道府県・TBOXタイプ別   */
                '*****************************/
                'データをグリッドに設定
                Me.grvListState.DataSource = dtData
                '変更を反映
                Me.grvListState.DataBind()
                'TBOXタイプ件数設定
                intTypeCount = (dtData.Columns.Count - 1) / 2
                '****************************/
                '*  LAN単価                 */
                '****************************/
                'データをグリッドに設定
                dtData.Columns(0).ColumnName = " "
                Me.grvListLan.DataSource = dtData
                '変更を反映
                Me.grvListLan.DataBind()
                '****************************/
                '*  エリア・TBOXタイプ別    */
                '****************************/
                dtData = Nothing
                dtData = New DataTable()
                dtData.Columns.Add("エリアコード")
                dtData.Columns.Add("エリア")
                For intIndex = 0 To dsState.Tables(0).Rows.Count - 1
                    With dsState.Tables(0).Rows(intIndex)
                        dtData.Columns.Add(.Item("M03_TBOX_NM").ToString() + "_件数")
                        dtData.Columns.Add(.Item("M03_TBOX_NM").ToString() + "_金額")
                    End With
                Next
                'データをグリッドに設定
                dtData.Columns(1).ColumnName = " "
                Me.grvListSummary.DataSource = dtData
                '変更を反映
                Me.grvListSummary.DataBind()

                '****************************/
                '* グリッドレイアウト整形   */
                '****************************/
                '--都道府県別
                Me.grvListState.HeaderStyle.Width = 100 + (intTypeCount * 130)
                Me.grvListState.Width = 100 + (intTypeCount * 130)
                Me.grvListState.HeaderRow.Cells(0).Width = 97
                For i = 1 To intTypeCount
                    Me.grvListState.HeaderRow.Cells.RemoveAt(i + 1)
                    Me.grvListState.HeaderRow.Cells(i).ColumnSpan = 2
                    Me.grvListState.HeaderRow.Cells(i).Width = 127
                    Me.grvListState.HeaderRow.Cells(i).Text = Me.grvListState.HeaderRow.Cells(i).Text.Replace("_件数", "")
                Next
                If Me.grvListState.Rows.Count > 0 Then
                    For i = 0 To Me.grvListState.Rows.Count - 1
                        Me.grvListState.Rows(i).Cells(0).Width = 97
                        Me.grvListState.Rows(i).Cells(0).HorizontalAlign = HorizontalAlign.Left
                        For j = 1 To Me.grvListState.Rows(i).Cells.Count - 1 Step 2
                            Me.grvListState.Rows(i).Cells(j).Width = 47
                            Me.grvListState.Rows(i).Cells(j + 1).Width = 77
                            Me.grvListState.Rows(i).Cells(j).HorizontalAlign = HorizontalAlign.Right
                            Me.grvListState.Rows(i).Cells(j + 1).HorizontalAlign = HorizontalAlign.Right
                        Next
                    Next
                End If

                '--エリア別
                Me.grvListSummary.HeaderStyle.Width = 100 + (intTypeCount * 130)
                Me.grvListSummary.Width = 100 + (intTypeCount * 130)
                'エリアコードは非表示
                Me.grvListSummary.HeaderRow.Cells(0).Visible = False
                'エリア名
                Me.grvListSummary.HeaderRow.Cells(1).Width = 97
                'TBOXタイプ毎の台数、金額
                For i = 1 To intTypeCount
                    Me.grvListSummary.HeaderRow.Cells.RemoveAt(i + 2)
                    Me.grvListSummary.HeaderRow.Cells(i + 1).ColumnSpan = 2
                    Me.grvListSummary.HeaderRow.Cells(i + 1).Width = 127
                    Me.grvListSummary.HeaderRow.Cells(i + 1).Text = Me.grvListSummary.HeaderRow.Cells(i + 1).Text.Replace("_件数", "")
                Next
                If Me.grvListSummary.Rows.Count > 0 Then
                    For i = 0 To Me.grvListSummary.Rows.Count - 1
                        'エリアコードは非表示
                        Me.grvListSummary.Rows(i).Cells(0).Visible = False
                        'エリア名
                        Me.grvListSummary.Rows(i).Cells(1).Width = 97
                        Me.grvListSummary.Rows(i).Cells(1).HorizontalAlign = HorizontalAlign.Left
                        'TBOXタイプ毎の台数、金額
                        For j = 2 To Me.grvListSummary.Rows(i).Cells.Count - 2 Step 2
                            Me.grvListSummary.Rows(i).Cells(j).Width = 47
                            Me.grvListSummary.Rows(i).Cells(j + 1).Width = 77
                            Me.grvListSummary.Rows(i).Cells(j).HorizontalAlign = HorizontalAlign.Right
                            Me.grvListSummary.Rows(i).Cells(j + 1).HorizontalAlign = HorizontalAlign.Right
                        Next
                    Next
                End If

                '--LAN単価
                Me.grvListLan.HeaderStyle.Width = 100 + (intTypeCount * 130)
                Me.grvListLan.Width = 100 + (intTypeCount * 130)
                Me.grvListLan.HeaderRow.Cells(0).Width = 97
                For i = 1 To intTypeCount
                    Me.grvListLan.HeaderRow.Cells.RemoveAt(i + 1)
                    Me.grvListLan.HeaderRow.Cells(i).ColumnSpan = 2
                    Me.grvListLan.HeaderRow.Cells(i).Width = 127
                    Me.grvListLan.HeaderRow.Cells(i).Text = Me.grvListState.HeaderRow.Cells(i).Text.Replace("_件数", "")
                Next
                If Me.grvListLan.Rows.Count > 0 Then
                    For i = 0 To Me.grvListLan.Rows.Count - 1
                        Me.grvListLan.Rows(i).Cells(0).Width = 97
                        Me.grvListLan.Rows(i).Cells(0).HorizontalAlign = HorizontalAlign.Left
                        For j = 1 To Me.grvListLan.Rows(i).Cells.Count - 1 Step 2
                            Me.grvListLan.Rows(i).Cells(j).Width = 47
                            Me.grvListLan.Rows(i).Cells(j + 1).Width = 77
                            Me.grvListLan.Rows(i).Cells(j).HorizontalAlign = HorizontalAlign.Right
                            Me.grvListLan.Rows(i).Cells(j + 1).HorizontalAlign = HorizontalAlign.Right
                        Next
                    Next
                End If

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守料金明細作成")
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

    End Sub
    'CMPUPDP001-002 END
#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
