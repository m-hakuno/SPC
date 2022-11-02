'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　品質会議資料明細
'*　ＰＧＭＩＤ：　QUAUPDP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.20　：　酒井
'*  更　新　　：　2014.11.13　：　武
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'QUAUPDP001-001     2016/01/22      武   　   事象マスタ削除対策              

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================

Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive

#End Region

Public Class QUAUPDP001

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
    Const sCnsProgId As String = "QUAUPDP001"
    Const sCnsProgNm As String = "品質会議資料明細"
    Const sCnsCsvButon As String = "ＣＳＶ"
    '修理機器一覧ファイルパス
    Const M_METDTIL_DISP_PATH = "~/" & P_MAI & "/" &
                                P_FUN_QUA & P_SCR_UPD & P_PAGE & "002" & "/" &
                                P_FUN_QUA & P_SCR_UPD & P_PAGE & "002.aspx"
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
    Dim clsExc As New ClsCMExclusive
    Dim dsCsv As New DataSet

    'エラーチェック時に、追加・更新ボタン押下か行追加ボタン押下かを判定する
    Dim mstrBtnFlg As String = String.Empty '（"UPD":追加・更新ボタン、"ROW":行追加ボタン）

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

#Region "ページ初期処理"
    ''' <summary>
    ''' ページ初期処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        '表設定
        pfSet_GridView(grvList, sCnsProgId, 85, 9)

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

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click       '検索
        AddHandler Master.ppRigthButton2.Click, AddressOf btnSearchClear_Click  '検索条件クリア
        AddHandler Master.Master.ppLeftButton1.Click, AddressOf btnEntry_Click  '登録
        AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnPrint_Click '印刷
        AddHandler Master.Master.ppRigthButton2.Click, AddressOf btnCsv_Click   'ＣＳＶ

        'テキストボックスアクションの設定
        '--管理番号
        AddHandler txtKanriNo.ppTextBox.TextChanged, AddressOf txtKanriNo_TextChanged
        txtKanriNo.ppTextBox.AutoPostBack = True

        Me.txtChousaJyokyou1.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txtChousaJyokyou1.MaxLength & """);")
        Me.txtChousaJyokyou2.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txtChousaJyokyou2.MaxLength & """);")
        Me.txtChousaJyokyou3.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txtChousaJyokyou3.MaxLength & """);")
        Me.txtChousaJyokyou4.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txtChousaJyokyou4.MaxLength & """);")
        Me.txtChousaJyokyou5.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txtChousaJyokyou5.MaxLength & """);")
        Me.txtChousaJyokyou1.Attributes.Add("onChange", "lenCheck(this,""" & Me.txtChousaJyokyou1.MaxLength & """);")
        Me.txtChousaJyokyou2.Attributes.Add("onChange", "lenCheck(this,""" & Me.txtChousaJyokyou2.MaxLength & """);")
        Me.txtChousaJyokyou3.Attributes.Add("onChange", "lenCheck(this,""" & Me.txtChousaJyokyou3.MaxLength & """);")
        Me.txtChousaJyokyou4.Attributes.Add("onChange", "lenCheck(this,""" & Me.txtChousaJyokyou4.MaxLength & """);")
        Me.txtChousaJyokyou5.Attributes.Add("onChange", "lenCheck(this,""" & Me.txtChousaJyokyou5.MaxLength & """);")

        Me.rdbEdaban1.AutoPostBack = True
        Me.rdbEdaban2.AutoPostBack = True
        Me.rdbEdaban3.AutoPostBack = True
        Me.rdbEdaban4.AutoPostBack = True
        Me.rdbEdaban5.AutoPostBack = True

        If Not IsPostBack Then  '初回表示

            '★排他情報用のグループ番号保管
            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            'プログラムID、画面名設定
            Master.Master.ppProgramID = sCnsProgId
            Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(sCnsProgId)

            'パンくずリスト設定
            Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

            '各コマンドボタンの属性設定
            '--検索条件クリア
            Master.ppRigthButton2.CausesValidation = False
            '--登録
            Master.Master.ppLeftButton1.Text = P_BTN_NM_ADD
            Master.Master.ppLeftButton1.Visible = True
            Master.Master.ppLeftButton1.CausesValidation = False
            Me.Master.Master.ppLeftButton1.OnClientClick =
                pfGet_OCClickMes("00008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, sCnsProgNm)
            '--印刷
            Master.Master.ppRigthButton1.Text = P_BTN_NM_PRI
            Master.Master.ppRigthButton1.Visible = True
            Master.Master.ppRigthButton1.CausesValidation = False
            Me.Master.Master.ppRigthButton1.OnClientClick =
                pfGet_OCClickMes("10002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, sCnsProgNm)
            '--ＣＳＶ
            Master.Master.ppRigthButton2.Text = sCnsCsvButon
            Master.Master.ppRigthButton2.Visible = True
            Master.Master.ppRigthButton2.CausesValidation = False
            '--追加
            Me.btnInsert.OnClientClick = pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, sCnsProgNm)
            '--更新
            Me.btnUpdate.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, sCnsProgNm)
            '--削除
            Me.btnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, sCnsProgNm)
            Me.btnDelete.CausesValidation = False
            Me.btnDelete.BackColor = Drawing.Color.FromArgb(255, 102, 102)
            '--クリア
            Me.btnClear.CausesValidation = False

            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            'ドロップダウンリスト生成
            If Not mfGet_DropDownList_Sel() Then
                '排他削除
                clsExc.psExclusive_Del_Chk(Me, Me.Master.Master.ppExclusiveDate, Me.Master.Master.ppExclusiveDateDtl)

                '画面を終了
                psClose_Window(Me)
            End If

            '画面クリア
            msClearScreen()

            '明細コントロール設定
            mfCtrlDetail()

        End If

    End Sub

#End Region

#Region "ユーザー権限"
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
                Me.btnInsert.Enabled = False       '追加ボタン
                Me.btnUpdate.Enabled = False       '更新ボタン
                Me.btnDelete.Enabled = False       '削除ボタン
                Me.btnClear.Enabled = False        'クリアボタン

                Me.txtKanriNo.ppEnabled = False
                Me.dtbUketukeDt.ppEnabled = False
                Me.dtbKaifukuDt.ppEnabled = False
                Me.txtStopTime.ppEnabled = False
                Me.ddlReport.Enabled = False
                Me.txtReportCon.ppEnabled = False
                Me.ddlEvent.Enabled = False
                Me.ddlRstSubmsnCls.ppEnabled = False
                Me.txtBuiKoshou.ppEnabled = False
                Me.dtbChousaEnd.ppEnabled = False
                Me.ddlStatusCd.Enabled = False

                Me.txtRepect.ppEnabled = False
                Me.txtRepect2.ppEnabled = False
                Me.txtRepect3.ppEnabled = False
                Me.txtRepect4.ppEnabled = False
                Me.txtRepect5.ppEnabled = False
                Me.txtRepect6.ppEnabled = False

                Me.rdbEdaban1.Enabled = False
                Me.rdbEdaban2.Enabled = False
                Me.rdbEdaban3.Enabled = False
                Me.rdbEdaban4.Enabled = False
                Me.rdbEdaban5.Enabled = False
                Me.ddlKosyoBui1.Enabled = False
                Me.ddlKosyoBui2.Enabled = False
                Me.ddlKosyoBui3.Enabled = False
                Me.ddlKosyoBui4.Enabled = False
                Me.ddlKosyoBui5.Enabled = False
                Me.ddlIchijiShindan1.Enabled = False
                Me.ddlIchijiShindan2.Enabled = False
                Me.ddlIchijiShindan3.Enabled = False
                Me.ddlIchijiShindan4.Enabled = False
                Me.ddlIchijiShindan5.Enabled = False
                Me.txtChousaJyokyou1.Enabled = False
                Me.txtChousaJyokyou2.Enabled = False
                Me.txtChousaJyokyou3.Enabled = False
                Me.txtChousaJyokyou4.Enabled = False
                Me.txtChousaJyokyou5.Enabled = False
                Me.btnGetRep.Enabled = False
                Me.btnRowAdd.Enabled = False
                Me.btnRowDel.Enabled = False
                Me.btnRowUp.Enabled = False
                Me.btnRowDn.Enabled = False

                Master.Master.ppLeftButton1.Enabled = False '登録ボタン
        End Select

    End Sub

#End Region

#Region "ボタン押下処理"
    ''' <summary>
    ''' 検索ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        Dim objDs As DataSet = Nothing  'データセット

        '開始ログ出力
        psLogStart(Me)

        'データ取得
        If (mfCheckSearchCondition() AndAlso Page.IsValid) Then

            '明細クリア
            msClearDetail()

            Dim objDsDtil As New DataSet

            'データ取得処理
            objDs = mfGetData(objDsDtil)

            If Not objDs Is Nothing Then

                '画面表示
                msDispData(objDs, objDsDtil)

                '取得データのうち、１個でも修理テーブルからのデータがあった場合は、フッタの登録ボタンを活性化
                Master.Master.ppLeftButton1.Enabled = False
                For Each dr As DataRow In objDs.Tables(0).Rows
                    If dr("データ種別").ToString = "2" Then
                        Master.Master.ppLeftButton1.Enabled = True
                        Exit For
                    End If
                Next

                Me.lblMode.Text = "0"

                '明細コントロール設定
                mfCtrlDetail()

                '管理番号へフォーカス
                txtKanriNo.ppTextBox.Focus()
            End If

        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 検索条件クリアボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearchClear_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        '検索条件クリア
        msClearSearchCondition()

        Me.dtbReportDt.ppDateBoxFrom.Focus()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 登録ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnEntry_Click(sender As Object, e As EventArgs)

        Dim ds As DataSet = Nothing     'データセット
        Dim dt As DataTable = Nothing   'データテーブル

        '開始ログ出力
        psLogStart(Me)

        If mfEntryData() Then

            'グリッド内容をビューステートに退避
            ViewState("ds") = mfGetGridData()
            'CSV内容をビューステートに退避
            ViewState("dsCsv") = mfGetDataCsv()

            '明細クリア
            msClearDetail()

            '管理番号へフォーカス
            txtKanriNo.ppTextBox.Focus()

            '自分自身を非活性にする
            DirectCast(sender, Button).Enabled = False
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' ＣＳＶボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCsv_Click(sender As Object, e As EventArgs)

        Dim ds As DataSet = Nothing
        Dim dsDtil As DataSet = Nothing
        Dim dsWrk As DataSet = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '開始ログ出力
        psLogStart(Me)

        Try
            'データセット格納
            ds = New DataSet
            ds = ViewState("dsCsv")

            'データセットを加工
            Dim strDataList(6) As String
            Dim strData As String = String.Empty

            dsDtil = New DataSet
            dsDtil = ViewState("QUAUPDP001_dsDtil")

            dsWrk = New DataSet
            dsWrk = ds.Copy

            Dim counter As Integer = 0

            'データセットを加工
            For Each dr As DataRow In dsWrk.Tables(0).Rows
                For Each drDtil As DataRow In dsDtil.Tables(0).Rows
                    If dr.Item("管理番号").ToString = drDtil.Item("管理番号").ToString Then
                        dr.Item("ホール名") = dr.Item("ホール名").ToString.Replace(Environment.NewLine, "").Trim
                        dr.Item("ホール住所") = dr.Item("ホール住所").ToString.Replace(Environment.NewLine, "").Trim
                        dr.Item("申告内容") = dr.Item("申告内容").ToString.Replace(Environment.NewLine, "")

                        dr.Item("一時診断結果" & (counter + 1).ToString) = drDtil.Item("一時診断結果").ToString
                        dr.Item("調査及び調査状況" & (counter + 1).ToString) = "・" & drDtil.Item("故障部位").ToString & "・・・" & drDtil.Item("調査及び調査状況").ToString

                        counter = counter + 1
                    End If

                    '５明細以外は印字しない
                    If counter = 4 Then
                        Exit For
                    End If
                Next
                'risetto
                counter = 0
            Next

            'CSVファイルダウンロード
            If pfDLCsvFile("品質会議資料明細_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv",
                           dsWrk.Tables(0), True, Me) <> 0 Then
                psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
            End If

        Catch ex As Exception
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 印刷ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPrint_Click(sender As Object, e As EventArgs)

        Dim ds As DataSet = Nothing
        Dim dsDtil As DataSet = Nothing
        Dim dsWrk As DataSet = Nothing
        Dim rpt As Object = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '開始ログ出力
        psLogStart(Me)

        Try
            '対応年月設定
            Session("SupportFrom") = ViewState("SupportFrom")
            Session("SupportTo") = ViewState("SupportTo")

            'データセット格納
            ds = New DataSet
            ds = ViewState("ds")

            'データセットを加工
            Dim strDataList() As String
            Dim strData As String = String.Empty
            For Each rowData As GridViewRow In grvList.Rows
                For Each dr As DataRow In ds.Tables(0).Rows
                    If DirectCast(rowData.FindControl("管理番号"), TextBox).Text = dr.Item("管理番号").ToString Then
                        strDataList = CType(rowData.FindControl("対応内容"), TextBox).Text.Split(Environment.NewLine)
                        If strDataList(0).Trim <> String.Empty Then
                            strData = strDataList(0)
                        End If
                        If strDataList(1).Trim <> String.Empty Then
                            strData = strData & strDataList(1)
                        End If
                        If strDataList(2).Trim <> String.Empty Then
                            strData = strData & strDataList(2)
                        End If
                        If strDataList(3).Trim <> String.Empty Then
                            strData = strData & strDataList(3)
                        End If
                        If strDataList(4).Trim <> String.Empty Then
                            strData = strData & strDataList(4)
                        End If
                        If strDataList(5).Trim <> String.Empty Then
                            strData = strData & strDataList(5)
                        End If
                        dr.Item("対応内容") = strData
                        strData = String.Empty
                    End If
                Next
            Next

            dsDtil = New DataSet
            dsDtil = ViewState("QUAUPDP001_dsDtil")

            dsWrk = New DataSet
            dsWrk = ds.Copy

            Dim counter As Integer = 0

            'データセットを加工
            For Each dr As DataRow In dsWrk.Tables(0).Rows
                For Each drDtil As DataRow In dsDtil.Tables(0).Rows
                    If dr.Item("管理番号").ToString = drDtil.Item("管理番号").ToString Then
                        dr.Item("ホール名") = dr.Item("ホール名").ToString.Replace(Environment.NewLine, "").Trim
                        dr.Item("ホール住所") = dr.Item("ホール住所").ToString.Replace(Environment.NewLine, "").Trim
                        dr.Item("申告内容") = dr.Item("申告内容").ToString.Replace(Environment.NewLine, "")

                        If counter = 0 Then
                            dr.Item("調査及び調査状況") = String.Empty
                        End If
                        If dr.Item("調査及び調査状況").ToString <> String.Empty Then
                            dr.Item("調査及び調査状況") = dr.Item("調査及び調査状況").ToString & Environment.NewLine
                        End If
                        dr.Item("調査及び調査状況") = dr.Item("調査及び調査状況").ToString _
                                                    & "・" _
                                                    & drDtil.Item("故障部位").ToString & "・・・" _
                                                    & drDtil.Item("調査及び調査状況").ToString.Replace(Environment.NewLine, "")

                        If counter = 0 Then
                            dr.Item("一時診断結果") = String.Empty
                        End If
                        drDtil.Item("一時診断結果").ToString.Replace(Environment.NewLine, "")
                        If dr.Item("一時診断結果").ToString <> String.Empty Then
                            dr.Item("一時診断結果") = dr.Item("一時診断結果").ToString & Environment.NewLine
                        End If
                        dr.Item("一時診断結果") = dr.Item("一時診断結果").ToString _
                                                & drDtil.Item("一時診断結果").ToString

                        counter = counter + 1
                    End If

                    '５明細以外は印字しない
                    If counter = 5 Then
                        Exit For
                    End If
                Next
                'risetto
                counter = 0
            Next

            '印刷処理
            rpt = New ETCREP021

            '印刷（ＰＤＦ表示）
            psPrintPDF(Me, rpt, dsWrk.Tables(0), sCnsProgNm)

        Catch ex As Exception
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsProgNm)
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

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 明細追加処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnInsert_Click(sender As Object, e As EventArgs) Handles btnInsert.Click

        '開始ログ出力
        psLogStart(Me)

        '整合性チェック
        msCheckUpdateDetail()

        '入力内容検証
        If (Page.IsValid) Then

            '明細追加処理
            If mfUpdateDataDetail(1) Then

                'グリッドへデータ追加
                msAddGridData()

                'グリッド内容をビューステートに退避
                ViewState("ds") = mfGetGridData()
                'CSV内容をビューステートに退避
                ViewState("dsCsv") = mfGetDataCsv()

                '明細クリア
                msClearDetail()

                '管理番号へフォーカス
                txtKanriNo.ppTextBox.Focus()

                Me.lblMode.Text = "0"

                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "品質会議資料明細")

            End If

        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 明細更新処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click

        '開始ログ出力
        psLogStart(Me)

        '整合性チェック
        msCheckUpdateDetail()

        '入力内容検証
        If (Page.IsValid) Then

            '明細更新処理
            If mfUpdateDataDetail(2) Then

                'グリッドデータ更新
                msUpdGridData()

                'グリッド内容をビューステートに退避
                ViewState("ds") = mfGetGridData()
                'CSV内容をビューステートに退避
                ViewState("dsCsv") = mfGetDataCsv()

                '明細クリア
                msClearDetail()

                '管理番号へフォーカス
                txtKanriNo.ppTextBox.Focus()

                Me.lblMode.Text = "0"

                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "品質会議資料明細")

            End If

        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 明細削除処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        '開始ログ出力
        psLogStart(Me)

        '明細削除処理
        If mfUpdateDataDetail(3) Then

            'グリッドデータ削除
            msDelGridData()

            'グリッド内容をビューステートに退避
            ViewState("ds") = mfGetGridData()
            'CSV内容をビューステートに退避
            ViewState("dsCsv") = mfGetDataCsv()

            '明細クリア
            msClearDetail()

            '管理番号へフォーカス
            txtKanriNo.ppTextBox.Focus()

            Me.lblMode.Text = "0"

            psMesBox(Me, "00002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "品質会議資料明細")

        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' クリア処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click

        '開始ログ出力
        psLogStart(Me)

        '明細クリア処理
        msClearDetail()

        Me.lblMode.Text = "0"

        '管理番号へフォーカス
        txtKanriNo.ppTextBox.Focus()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 一覧（グリッド）クリア処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnListClear_Click(sender As Object, e As EventArgs) Handles btnListClear.Click

        '開始ログ出力
        psLogStart(Me)

        '検索結果の種別クリア
        lblClass.Text = String.Empty

        'グリッド初期化
        Me.grvList.DataSource = New DataTable
        Master.ppCount = "0"
        Me.grvList.DataBind()

        '登録、ＣＳＶ、印刷ボタン非活性
        Master.Master.ppLeftButton1.Enabled = False
        Master.Master.ppRigthButton2.Enabled = False
        Master.Master.ppRigthButton1.Enabled = False

        'ビューステート初期化
        ViewState("ds") = Nothing
        ViewState("dsDtil") = Nothing
        ViewState("QUAUPDP001_dsDtil") = Nothing

        '終了ログ出力
        psLogEnd(Me)

    End Sub

#End Region

#Region "データバインド時処理"
    ''' <summary>
    ''' データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        Dim strDataCls As String = Nothing  'データ種別（ 1:品質会議資料データからの取得、2:保守対応データからの取得 ）
        Dim strEntryFlg As String = Nothing '登録フラグ（ 0:未登録、1:登録 ） 

        For Each rowData As GridViewRow In grvList.Rows

            'データ種別が"2:保守対応データからの取得"の場合、選択ボタンを非活性にする
            strDataCls = CType(rowData.FindControl("データ種別"), TextBox).Text
            If strDataCls = "2" Then
                rowData.Cells(0).Enabled = False
            End If

            '登録したデータは文字を赤くする
            strEntryFlg = CType(rowData.FindControl("登録フラグ"), TextBox).Text
            If strEntryFlg = "1" Then
                For i = 1 To rowData.Cells.Count - 1
                    CType(rowData.Cells(i).Controls.Item(0), TextBox).ForeColor = Drawing.Color.Red
                Next
            End If

            If (rowData.RowIndex Mod 2) = 1 Then
                'CType(rowData.FindControl("選択"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("項番"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("データ種別"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("管理番号"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("枝番"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("種別コード"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("種別名称"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("ＮＬ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("ＮＬ区分"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("ＶＥＲ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("ホールコード"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("ホール名"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("ホール住所"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("受付日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("マスタ開始日"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("回復日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("分"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("サンド入金停止時間"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("申告元コード"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("申告元"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("申告内容"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("事象コード"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("事象"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("一時診断結果"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("故障部位"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("調査完了日"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("調査及び調査状況"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("ステータスコード"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("ステータス"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("提出"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("提出区分"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("対応内容"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("開始日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("登録フラグ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("データ件数"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
            Else
                CType(rowData.FindControl("項番"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("データ種別"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("管理番号"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("枝番"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("種別コード"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("種別名称"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("ＮＬ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("ＮＬ区分"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("ＶＥＲ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("ホールコード"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("ホール名"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("ホール住所"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("受付日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("マスタ開始日"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("回復日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("分"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("サンド入金停止時間"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("申告元コード"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("申告元"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("申告内容"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("事象コード"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("事象"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("一時診断結果"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("故障部位"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("調査完了日"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("調査及び調査状況"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("ステータスコード"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("ステータス"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("提出"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("提出区分"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("対応内容"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("開始日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("登録フラグ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("データ件数"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
            End If

        Next

    End Sub
#End Region

#Region "一覧の選択ボタン押下処理"
    ''' <summary>
    ''' 一覧の選択ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        '行追加ボタンの使用可不可を判定するフラグ
        Dim blnEnableBtnAdd As Boolean = True

        Dim rowData As GridViewRow = Nothing    'ボタン押下行
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        If e.CommandName <> "btnSelect" Then
            Exit Sub
        End If

        '開始ログ出力
        psLogStart(Me)

        Try
            '明細クリア
            msClearDetail()

            'ボタン押下行の情報を取得
            rowData = grvList.Rows(Convert.ToInt32(e.CommandArgument))

            '★排他制御用の変数
            Dim strExclusiveDate As String = String.Empty
            Dim arTable_Name As New ArrayList
            Dim arKey As New ArrayList

            '★排他情報削除
            If Not Me.Master.Master.ppExclusiveDateDtl = String.Empty Then

                clsExc.pfDel_Exclusive(Me _
                              , Session(P_SESSION_SESSTION_ID) _
                              , Me.Master.Master.ppExclusiveDateDtl)

                Me.Master.Master.ppExclusiveDateDtl = String.Empty

            End If

            Dim intExFlg As Integer = 0     '排他判断用フラグ

            Select Case Session(P_SESSION_AUTH)
                Case "管理者", "SPC"
                    '★ロック対象テーブル名の登録
                    arTable_Name.Insert(0, "D86_MEETING")

                    '★ロックテーブルキー項目の登録(D86_MEETING)
                    arKey.Insert(0, CType(rowData.FindControl("項番"), TextBox).Text)     'D86_NO
                    arKey.Insert(1, CType(rowData.FindControl("管理番号"), TextBox).Text) 'D86_MEETING_NO
                    arKey.Insert(2, CType(rowData.FindControl("枝番"), TextBox).Text)     'D86_BRANCH

                    '★排他情報確認処理(更新処理の実行)
                    If clsExc.pfSel_Exclusive(strExclusiveDate _
                                     , Me _
                                     , Session(P_SESSION_IP) _
                                     , Session(P_SESSION_PLACE) _
                                     , Session(P_SESSION_USERID) _
                                     , Session(P_SESSION_SESSTION_ID) _
                                     , ViewState(P_SESSION_GROUP_NUM) _
                                     , sCnsProgId _
                                     , arTable_Name _
                                     , arKey) = 0 Then

                        '登録年月日時刻(明細)に登録.
                        Me.Master.Master.ppExclusiveDateDtl = strExclusiveDate
                    Else
                        '排他ロック中
                        intExFlg = 1
                    End If
            End Select

            If intExFlg = 0 Then

                '明細の各項目に対してデータを設定
                lblNo.Text = CType(rowData.FindControl("項番"), TextBox).Text                               '項番
                txtKanriNo.ppText = CType(rowData.FindControl("管理番号"), TextBox).Text                    '管理番号
                lblTboxId.Text = CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text                   'ＴＢＯＸＩＤ
                lblVer.Text = CType(rowData.FindControl("ＶＥＲ"), TextBox).Text                            'ＶＥＲ
                lblHallNm.Text = CType(rowData.FindControl("ホール名"), TextBox).Text                       'ホール名

                'lblUketukeDt.Text = CType(rowData.FindControl("受付日時"), TextBox).Text                    '受付日時
                If CType(rowData.FindControl("受付日時"), TextBox).Text <> String.Empty Then
                    Dim strWrkDate As String = CType(rowData.FindControl("受付日時"), TextBox).Text.Replace(Environment.NewLine, "")
                    dtbUketukeDt.ppDateBox.Text = strWrkDate.Substring(0, 10)
                    dtbUketukeDt.ppHourBox.Text = strWrkDate.Substring(11, 2)
                    dtbUketukeDt.ppMinBox.Text = strWrkDate.Substring(14, 2)
                Else
                    dtbUketukeDt.ppDateBox.Text = String.Empty
                    dtbUketukeDt.ppHourBox.Text = String.Empty
                    dtbUketukeDt.ppMinBox.Text = String.Empty
                End If

                lblStartDt.Text = CType(rowData.FindControl("マスタ開始日"), TextBox).Text                  'マスタ開始日
                If CType(rowData.FindControl("回復日時"), TextBox).Text.Length > 0 Then
                    dtbKaifukuDt.ppText = CType(rowData.FindControl("回復日時"), TextBox).Text.Substring(0, 10)
                Else
                    dtbKaifukuDt.ppText = String.Empty
                End If
                dtbKaifukuDt.ppHourText = CType(rowData.FindControl("時"), TextBox).Text                    '回復日時
                dtbKaifukuDt.ppMinText = CType(rowData.FindControl("分"), TextBox).Text                     '回復日分

                txtStopTime.ppText = CType(rowData.FindControl("サンド入金停止時間"), TextBox).Text         'サンド入金停止時間
                ddlReport.SelectedValue = CType(rowData.FindControl("申告元コード"), TextBox).Text.Trim          '申告元
                txtReportCon.ppText = CType(rowData.FindControl("申告内容"), TextBox).Text                  '申告内容


                'ドロップダウンの内容書き換え
                'QUAUPDP001-001
                'If ddlEvent.SelectedIndex <> 0 Then
                If CType(rowData.FindControl("事象コード"), TextBox).Text = "" Then
                    ddlEvent.SelectedIndex = 0
                Else
                    If ddlEvent.Items.FindByValue(CType(rowData.FindControl("事象コード"), TextBox).Text) Is Nothing Then
                        Me.ddlEvent.Items.Insert(0, New ListItem(Nothing, Nothing))
                        ddlEvent.SelectedIndex = 1
                        ddlEvent.Items.Item(1).Value = "00"
                        ddlEvent.Items.Item(1).Text = CType(rowData.FindControl("事象コード"), TextBox).Text & ":" & CType(rowData.FindControl("事象"), TextBox).Text.Replace(Environment.NewLine, "")
                        'ddlEvent.Items.FindByValue(CType(rowData.FindControl("事象コード"), TextBox).Text).Text _
                        '    = CType(rowData.FindControl("事象コード"), TextBox).Text & ":" & CType(rowData.FindControl("事象"), TextBox).Text
                    Else
                        If Not ddlEvent.Items.FindByValue(CType(rowData.FindControl("事象コード"), TextBox).Text).ToString = CType(rowData.FindControl("事象コード"), TextBox).Text & ":" & CType(rowData.FindControl("事象"), TextBox).Text.Replace(Environment.NewLine, "") Then
                            Me.ddlEvent.Items.Insert(0, New ListItem(Nothing, Nothing))
                            ddlEvent.SelectedIndex = 1
                            ddlEvent.Items.Item(1).Value = "00"
                            ddlEvent.Items.Item(1).Text = CType(rowData.FindControl("事象コード"), TextBox).Text & ":" & CType(rowData.FindControl("事象"), TextBox).Text.Replace(Environment.NewLine, "")
                        Else
                            ddlEvent.SelectedValue = CType(rowData.FindControl("事象コード"), TextBox).Text             '事象
                        End If
                    End If
                End If
                'QUAUPDP001-001 END

                txtBuiKoshou.ppText = CType(rowData.FindControl("故障部位"), TextBox).Text                  '部位故障
                dtbChousaEnd.ppText = CType(rowData.FindControl("調査完了日"), TextBox).Text                '調査完了日


                Dim dsDtil As DataSet = Nothing
                dsDtil = ViewState("QUAUPDP001_dsDtil")
                Dim drWrk() As DataRow
                drWrk = dsDtil.Tables(0).Select("管理番号='" & CType(rowData.FindControl("管理番号"), TextBox).Text & "'")

                If drWrk.Count <> 0 Then

                    Dim objhdnSeq As HiddenField        '連番用
                    Dim objhdnEdaban As HiddenField     '管理番号枝番用
                    Dim objddlKosyou As DropDownList    '故障部位用
                    Dim objddlIchiji As DropDownList    '一時診断結果用
                    Dim objtxtChousa As TextBox         '調査状況用


                    For zz As Integer = 0 To drWrk.Count - 1
                        objhdnSeq = Nothing
                        objhdnEdaban = Nothing
                        objddlKosyou = Nothing
                        objddlIchiji = Nothing
                        objtxtChousa = Nothing

                        Select Case zz
                            Case 0
                                objhdnSeq = hdnSeq1
                                objhdnEdaban = hdnEdaban1
                                objddlKosyou = ddlKosyoBui1
                                objddlIchiji = ddlIchijiShindan1
                                objtxtChousa = txtChousaJyokyou1
                            Case 1
                                objhdnSeq = hdnSeq2
                                objhdnEdaban = hdnEdaban2
                                objddlKosyou = ddlKosyoBui2
                                objddlIchiji = ddlIchijiShindan2
                                objtxtChousa = txtChousaJyokyou2
                            Case 2
                                objhdnSeq = hdnSeq3
                                objhdnEdaban = hdnEdaban3
                                objddlKosyou = ddlKosyoBui3
                                objddlIchiji = ddlIchijiShindan3
                                objtxtChousa = txtChousaJyokyou3
                            Case 3
                                objhdnSeq = hdnSeq4
                                objhdnEdaban = hdnEdaban4
                                objddlKosyou = ddlKosyoBui4
                                objddlIchiji = ddlIchijiShindan4
                                objtxtChousa = txtChousaJyokyou4
                            Case 4
                                objhdnSeq = hdnSeq5
                                objhdnEdaban = hdnEdaban5
                                objddlKosyou = ddlKosyoBui5
                                objddlIchiji = ddlIchijiShindan5
                                objtxtChousa = txtChousaJyokyou5
                                blnEnableBtnAdd = False
                        End Select

                        objhdnSeq.Value = drWrk(zz).Item("枝番").ToString
                        objhdnEdaban.Value = drWrk(zz).Item("管理番号枝番").ToString
                        If drWrk(zz).Item("故障部位コード").ToString <> String.Empty Then
                            objddlKosyou.SelectedValue = drWrk(zz).Item("故障部位コード").ToString
                        Else
                            objddlKosyou.SelectedIndex = 0
                        End If
                        If drWrk(zz).Item("一時診断コード").ToString <> String.Empty Then
                            objddlIchiji.SelectedValue = drWrk(zz).Item("一時診断コード").ToString
                        Else
                            objddlIchiji.SelectedIndex = 0
                        End If
                        objtxtChousa.Text = drWrk(zz).Item("調査及び調査状況").ToString
                    Next
                End If


                ddlStatusCd.SelectedValue = CType(rowData.FindControl("ステータスコード"), TextBox).Text    'ステータス
                lblNLClsNm.Text = CType(rowData.FindControl("ＮＬ区分"), TextBox).Text                      'ＮＬ区分（名称）
                ddlRstSubmsnCls.ppDropDownList.SelectedValue = CType(rowData.FindControl("提出"), TextBox).Text '提出区分

                Dim strData(6) As String
                strData = CType(rowData.FindControl("対応内容"), TextBox).Text.Split(Environment.NewLine)
                txtRepect.ppText = strData(0)
                txtRepect2.ppText = strData(1)
                txtRepect3.ppText = strData(2)
                txtRepect4.ppText = strData(3)
                txtRepect5.ppText = strData(4)
                txtRepect6.ppText = strData(5)

                '非表示項目
                lblClassCd.Text = CType(rowData.FindControl("種別コード"), TextBox).Text                    '種別コード
                lblClassNm.Text = CType(rowData.FindControl("種別名称"), TextBox).Text                      '種別名称
                lblNLClsCd.Text = CType(rowData.FindControl("ＮＬ"), TextBox).Text                          'ＮＬ区分（コード）
                lblHallCd.Text = CType(rowData.FindControl("ホールコード"), TextBox).Text                   'ホールコード
                lblHallAdr.Text = CType(rowData.FindControl("ホール住所"), TextBox).Text                    'ホール住所
                lblDealDt.Text = CType(rowData.FindControl("開始日時"), TextBox).Text.Replace(Environment.NewLine, "")                       '対応日（開始日時）

                '管理番号・枝番の非活性
                Me.txtKanriNo.ppEnabled = False

                'ボタン活性・非活性
                Me.btnInsert.Enabled = False
                Me.btnUpdate.Enabled = True
                Me.btnDelete.Enabled = True
                Me.btnListClear.Enabled = False

                ''修理データに存在するかを判定する
                'Dim blnIsExistRp As Boolean = False
                'Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
                'Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
                'If Not clsDataConnect.pfOpen_Database(objCn) Then
                '    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                'Else
                '    objCmd = New SqlCommand(sCnsProgId + "_S9", objCn)
                '    With objCmd.Parameters '--パラメータ設定
                '        .Add(pfSet_Param("meeting_no", SqlDbType.NVarChar, txtKanriNo.ppText))  '管理番号
                '    End With

                '    'データ取得
                '    Dim ds As DataSet = Nothing
                '    ds = clsDataConnect.pfGet_DataSet(objCmd)
                '    If ds.Tables(0).Rows.Count <> 0 Then
                '        Me.lblExistsRp.Text = "1"
                '    Else
                '        Me.lblExistsRp.Text = "0"
                '    End If
                'End If

                Me.lblMode.Text = "0"

                '明細コントロール設定
                mfCtrlDetail()

                'ラジオボタンチェック(１番上かチェックなし)
                If Me.rdbEdaban1.Enabled = True Then
                    Me.rdbEdaban1.Checked = True
                End If

                'ボタン制御
                mfBtnDetail()

                ''ボタン制御(5行目まで活性化されていたらボタン非活性)
                'Me.btnRowAdd.Enabled = blnEnableBtnAdd
            End If


        Catch ex As Exception
            psMesBox(Me, "30009", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "選択",
                     ex.ToString.Replace(Environment.NewLine, "").Replace("'", ""))
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

        '終了ログ出力
        psLogEnd(Me)

    End Sub
#End Region

#Region "管理番号変更時処理"
    ''' <summary>
    ''' 管理番号変更時処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub txtKanriNo_TextChanged()

        '行追加ボタンの使用可不可を判定するフラグ
        Dim blnEnableBtnAdd As Boolean = True

        Dim objDs As DataSet = Nothing  'データセット
        Dim objDsDtil As DataSet = Nothing

        '表示項目クリア
        lblNo.Text = String.Empty
        lblTboxId.Text = String.Empty
        lblVer.Text = String.Empty
        lblHallCd.Text = String.Empty
        lblHallNm.Text = String.Empty
        lblHallAdr.Text = String.Empty
        dtbUketukeDt.ppDateBox.Text = String.Empty
        dtbUketukeDt.ppHourBox.Text = String.Empty
        dtbUketukeDt.ppMinBox.Text = String.Empty
        lblDealDt.Text = String.Empty
        lblStartDt.Text = String.Empty
        dtbKaifukuDt.ppDateBox.Text = String.Empty
        dtbKaifukuDt.ppHourBox.Text = String.Empty
        dtbKaifukuDt.ppMinBox.Text = String.Empty
        lblClassCd.Text = String.Empty
        lblClassNm.Text = String.Empty
        lblNLClsCd.Text = String.Empty
        lblNLClsNm.Text = String.Empty
        ddlReport.SelectedIndex = 0
        txtReportCon.ppText = String.Empty
        txtBuiKoshou.ppText = String.Empty
        txtRepect.ppText = String.Empty
        txtRepect2.ppText = String.Empty
        txtRepect3.ppText = String.Empty
        txtRepect4.ppText = String.Empty
        txtRepect5.ppText = String.Empty
        txtRepect6.ppText = String.Empty

        '-- 製品名
        '枝番
        hdnSeq1.Value = String.Empty
        hdnSeq2.Value = String.Empty
        hdnSeq3.Value = String.Empty
        hdnSeq4.Value = String.Empty
        hdnSeq5.Value = String.Empty
        '管理番号枝番
        hdnEdaban1.Value = String.Empty
        hdnEdaban2.Value = String.Empty
        hdnEdaban3.Value = String.Empty
        hdnEdaban4.Value = String.Empty
        hdnEdaban5.Value = String.Empty
        '故障部位
        ddlKosyoBui1.SelectedIndex = 0
        ddlKosyoBui2.SelectedIndex = 0
        ddlKosyoBui3.SelectedIndex = 0
        ddlKosyoBui4.SelectedIndex = 0
        ddlKosyoBui5.SelectedIndex = 0

        '-- 一時診断結果
        ddlIchijiShindan1.SelectedIndex = 0
        ddlIchijiShindan2.SelectedIndex = 0
        ddlIchijiShindan3.SelectedIndex = 0
        ddlIchijiShindan4.SelectedIndex = 0
        ddlIchijiShindan5.SelectedIndex = 0

        '-- 調査結果及び調査状況
        txtChousaJyokyou1.Text = String.Empty
        txtChousaJyokyou2.Text = String.Empty
        txtChousaJyokyou3.Text = String.Empty
        txtChousaJyokyou4.Text = String.Empty
        txtChousaJyokyou5.Text = String.Empty

        rdbEdaban1.Enabled = False
        rdbEdaban2.Enabled = False
        rdbEdaban3.Enabled = False
        rdbEdaban4.Enabled = False
        rdbEdaban5.Enabled = False
        ddlKosyoBui1.Enabled = False
        ddlKosyoBui2.Enabled = False
        ddlKosyoBui3.Enabled = False
        ddlKosyoBui4.Enabled = False
        ddlKosyoBui5.Enabled = False
        ddlIchijiShindan1.Enabled = False
        ddlIchijiShindan2.Enabled = False
        ddlIchijiShindan3.Enabled = False
        ddlIchijiShindan4.Enabled = False
        ddlIchijiShindan5.Enabled = False
        txtChousaJyokyou1.Enabled = False
        txtChousaJyokyou2.Enabled = False
        txtChousaJyokyou3.Enabled = False
        txtChousaJyokyou4.Enabled = False
        txtChousaJyokyou5.Enabled = False

        '提出区分の初期値を設定する
        ddlRstSubmsnCls.ppDropDownList.SelectedIndex = 2 '提出区分

        If Me.txtKanriNo.ppText <> String.Empty Then

            '保守対応情報取得処理
            objDs = mfGetMntInfo(objDsDtil)

            If Not objDs Is Nothing Then
                If objDs.Tables(0).Rows.Count = 0 Then
                    txtKanriNo.psSet_ErrorNo("2002", "入力した管理番号")
                    txtKanriNo.ppTextBox.Focus()
                Else
                    With objDs.Tables(0).Rows(0)

                        Dim dsWrk As New DataSet
                        dsWrk = mfGetMeetingMstInfo(.Item("管理番号").ToString)
                        If dsWrk.Tables(0).Rows.Count = 0 Then
                            txtKanriNo.psSet_ErrorNo("2013", "入力した管理番号のシステムコード", "品質会議マスタ")
                            txtKanriNo.ppTextBox.Focus()
                            Exit Sub
                        End If

                        If .Item("開始日時").ToString = String.Empty Then
                            txtKanriNo.psSet_ErrorNo("2018")
                            txtKanriNo.ppTextBox.Focus()
                            Exit Sub
                        End If

                        If .Item("D86_管理番号").ToString <> String.Empty Then
                            txtKanriNo.psSet_ErrorNo("2006", "入力した管理番号")
                            txtKanriNo.ppTextBox.Focus()
                        Else
                            '保守対応情報をラベルに設定
                            lblTboxId.Text = .Item("ＴＢＯＸＩＤ").ToString
                            lblVer.Text = .Item("ＶＥＲ").ToString
                            lblHallCd.Text = .Item("ホールコード").ToString
                            lblHallNm.Text = .Item("ホール名").ToString
                            lblHallAdr.Text = .Item("ホール住所").ToString

                            If .Item("受付日時").ToString <> String.Empty Then
                                dtbUketukeDt.ppDateBox.Text = .Item("受付日時").ToString.Substring(0, 10).Replace("-", "/")
                                dtbUketukeDt.ppHourBox.Text = .Item("受付日時").ToString.Substring(11, 2)
                                dtbUketukeDt.ppMinBox.Text = .Item("受付日時").ToString.Substring(14, 2)
                            End If

                            lblDealDt.Text = .Item("開始日時").ToString
                            lblStartDt.Text = .Item("運用開始日").ToString
                            lblClassCd.Text = .Item("種別コード").ToString
                            lblClassNm.Text = .Item("種別名称").ToString
                            lblNLClsCd.Text = .Item("ＮＬ").ToString
                            lblNLClsNm.Text = .Item("ＮＬ区分").ToString
                            If .Item("申告元コード").ToString.Trim <> String.Empty Then
                                ddlReport.SelectedValue = .Item("申告元コード").ToString
                            End If
                            txtReportCon.ppText = .Item("申告内容").ToString

                            If .Item("回復日時").ToString <> String.Empty Then
                                dtbKaifukuDt.ppDateBox.Text = .Item("回復日時").ToString.Substring(0, 10).Replace("-", "/")
                                dtbKaifukuDt.ppHourBox.Text = .Item("回復日時").ToString.Substring(11, 2)
                                dtbKaifukuDt.ppMinBox.Text = .Item("回復日時").ToString.Substring(14, 2)
                            End If

                            If objDsDtil.Tables(0).Rows.Count <> 0 Then

                                Dim objhdnSeq As HiddenField        '枝番用
                                Dim objhdnEdaban As HiddenField     '管理番号枝番用
                                Dim objddlKosyou As DropDownList    '故障部位用
                                Dim objddlIchiji As DropDownList    '一時診断結果用
                                Dim objtxtChousa As TextBox         '調査状況用

                                For zz As Integer = 0 To objDsDtil.Tables(0).Rows.Count - 1
                                    '５行分処理したら抜ける
                                    If zz = 5 Then
                                        Exit For
                                    End If

                                    objhdnSeq = Nothing
                                    objhdnEdaban = Nothing
                                    objddlKosyou = Nothing
                                    objddlIchiji = Nothing
                                    objtxtChousa = Nothing

                                    Select Case zz
                                        Case 0
                                            objhdnSeq = hdnSeq1
                                            objhdnEdaban = hdnEdaban1
                                            objddlKosyou = ddlKosyoBui1
                                            objddlIchiji = ddlIchijiShindan1
                                            objtxtChousa = txtChousaJyokyou1
                                        Case 1
                                            objhdnSeq = hdnSeq2
                                            objhdnEdaban = hdnEdaban2
                                            objddlKosyou = ddlKosyoBui2
                                            objddlIchiji = ddlIchijiShindan2
                                            objtxtChousa = txtChousaJyokyou2
                                        Case 2
                                            objhdnSeq = hdnSeq3
                                            objhdnEdaban = hdnEdaban3
                                            objddlKosyou = ddlKosyoBui3
                                            objddlIchiji = ddlIchijiShindan3
                                            objtxtChousa = txtChousaJyokyou3
                                        Case 3
                                            objhdnSeq = hdnSeq4
                                            objhdnEdaban = hdnEdaban4
                                            objddlKosyou = ddlKosyoBui4
                                            objddlIchiji = ddlIchijiShindan4
                                            objtxtChousa = txtChousaJyokyou4
                                        Case 4
                                            objhdnSeq = hdnSeq5
                                            objhdnEdaban = hdnEdaban5
                                            objddlKosyou = ddlKosyoBui5
                                            objddlIchiji = ddlIchijiShindan5
                                            objtxtChousa = txtChousaJyokyou5
                                            blnEnableBtnAdd = False
                                    End Select

                                    objhdnSeq.Value = objDsDtil.Tables(0).Rows(zz).Item("枝番").ToString
                                    objhdnEdaban.Value = objDsDtil.Tables(0).Rows(zz).Item("管理番号枝番").ToString
                                    If objDsDtil.Tables(0).Rows(zz).Item("故障部位コード").ToString <> String.Empty Then
                                        objddlKosyou.SelectedValue = objDsDtil.Tables(0).Rows(zz).Item("故障部位コード").ToString
                                    Else
                                        objddlKosyou.SelectedIndex = 0
                                    End If
                                    If objDsDtil.Tables(0).Rows(zz).Item("一時診断コード").ToString <> String.Empty Then
                                        objddlIchiji.SelectedValue = objDsDtil.Tables(0).Rows(zz).Item("一時診断コード").ToString
                                    Else
                                        objddlIchiji.SelectedIndex = 0
                                    End If
                                    objtxtChousa.Text = objDsDtil.Tables(0).Rows(zz).Item("調査及び調査状況").ToString
                                Next
                            End If

                            txtBuiKoshou.ppText = ""
                            txtRepect.ppText = .Item("処置内容").ToString
                            txtRepect2.ppText = .Item("処置内容２").ToString
                            txtRepect3.ppText = .Item("処置内容３").ToString
                            txtRepect4.ppText = .Item("処置内容４").ToString
                            txtRepect5.ppText = .Item("処置内容５").ToString
                            txtRepect6.ppText = .Item("処置内容６").ToString
                        End If
                    End With

                    ''修理データに存在するかを判定する
                    'Dim blnIsExistRp As Boolean = False
                    'Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
                    'Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
                    'If Not clsDataConnect.pfOpen_Database(objCn) Then
                    '    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    'Else
                    '    objCmd = New SqlCommand(sCnsProgId + "_S9", objCn)
                    '    With objCmd.Parameters '--パラメータ設定
                    '        .Add(pfSet_Param("meeting_no", SqlDbType.NVarChar, txtKanriNo.ppText))  '管理番号
                    '    End With

                    '    'データ取得
                    '    Dim ds As DataSet = Nothing
                    '    ds = clsDataConnect.pfGet_DataSet(objCmd)
                    '    If ds.Tables(0).Rows.Count <> 0 Then
                    '        Me.lblExistsRp.Text = "1"
                    '    Else
                    '        Me.lblExistsRp.Text = "0"
                    '    End If
                    'End If

                    Me.lblMode.Text = "0"

                    '明細コントロール設定
                    mfCtrlDetail()

                    'ラジオボタンチェック(１番上かチェックなし)
                    If Me.rdbEdaban1.Enabled = True Then
                        Me.rdbEdaban1.Checked = True
                    End If

                    'ボタン制御
                    mfBtnDetail()

                    ''ボタン制御(5行目まで活性化されていたらボタン非活性)
                    'Me.btnRowAdd.Enabled = blnEnableBtnAdd
                End If
            End If

        End If

    End Sub
#End Region

#End Region

#Region "そのほかのプロシージャ"

#Region "画面クリア処理"
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
            '検索条件クリア処理
            msClearSearchCondition()

            '検索結果の種別クリア
            lblClass.Text = String.Empty

            'グリッド初期化
            Me.grvList.DataSource = New DataTable
            Master.ppCount = "0"
            Me.grvList.DataBind()

            '明細編集項目初期化処理
            msClearDetail()

            ''ボタン制御
            'mfBtnDetail()

            '登録、ＣＳＶ、印刷ボタン非活性
            Master.Master.ppLeftButton1.Enabled = False
            Master.Master.ppRigthButton2.Enabled = False
            Master.Master.ppRigthButton1.Enabled = False

            Me.dtbReportDt.ppDateBoxFrom.Focus()

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
#End Region

#Region "検索条件クリア処理"
    ''' <summary>
    ''' 検索条件クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearSearchCondition()

        '検索条件クリア
        Me.dtbReportDt.ppFromText = String.Empty
        Me.dtbReportDt.ppToText = String.Empty
        Me.ddlTboxType.ppDropDownList.SelectedIndex = 0
        Me.ddlCdnSubmsnCls.ppDropDownList.SelectedIndex = 0

    End Sub
#End Region

#Region "データ取得処理"
    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetData(ByRef ds As DataSet) As DataSet

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objCsvCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing          'データセット
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
                objCmd = New SqlCommand(sCnsProgId + "_S1", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    .Add(pfSet_Param("reportdt_f", SqlDbType.NVarChar, Me.dtbReportDt.ppFromText))          '報告日From
                    .Add(pfSet_Param("reportdt_t", SqlDbType.NVarChar, Me.dtbReportDt.ppToText))            '報告日To
                    .Add(pfSet_Param("class", SqlDbType.NVarChar, Me.ddlTboxType.ppSelectedValue))          '種別
                    .Add(pfSet_Param("submsncls", SqlDbType.NVarChar, Me.ddlCdnSubmsnCls.ppSelectedValue))  '提出区分
                    If Session(P_SESSION_AUTH) = "NGC" Then
                        .Add(pfSet_Param("ngc", SqlDbType.NVarChar, "1"))  '提出区分
                    Else
                        .Add(pfSet_Param("ngc", SqlDbType.NVarChar, "0"))  '提出区分
                    End If
                End With

                'データ取得
                mfGetData = clsDataConnect.pfGet_DataSet(objCmd)

                objCmd = New SqlCommand(sCnsProgId + "_S4", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    .Add(pfSet_Param("reportdt_f", SqlDbType.NVarChar, Me.dtbReportDt.ppFromText))          '報告日From
                    .Add(pfSet_Param("reportdt_t", SqlDbType.NVarChar, Me.dtbReportDt.ppToText))            '報告日To
                    .Add(pfSet_Param("class", SqlDbType.NVarChar, Me.ddlTboxType.ppSelectedValue))          '種別
                    .Add(pfSet_Param("submsncls", SqlDbType.NVarChar, Me.ddlCdnSubmsnCls.ppSelectedValue))  '提出区分
                    If Session(P_SESSION_AUTH) = "NGC" Then
                        .Add(pfSet_Param("ngc", SqlDbType.NVarChar, "1"))  '提出区分
                    Else
                        .Add(pfSet_Param("ngc", SqlDbType.NVarChar, "0"))  '提出区分
                    End If
                End With

                ds = clsDataConnect.pfGet_DataSet(objCmd)

                objCsvCmd = New SqlCommand("QUAUPDP001_S8", objCn)
                With objCsvCmd.Parameters
                    '--パラメータ設定
                    .Add(pfSet_Param("reportdt_f", SqlDbType.NVarChar, Me.dtbReportDt.ppFromText))          '報告日From
                    .Add(pfSet_Param("reportdt_t", SqlDbType.NVarChar, Me.dtbReportDt.ppToText))            '報告日To
                    .Add(pfSet_Param("class", SqlDbType.NVarChar, Me.ddlTboxType.ppSelectedValue))          '種別
                    .Add(pfSet_Param("submsncls", SqlDbType.NVarChar, Me.ddlCdnSubmsnCls.ppSelectedValue))  '提出区分
                    If Session(P_SESSION_AUTH) = "NGC" Then
                        .Add(pfSet_Param("ngc", SqlDbType.NVarChar, "1"))  '提出区分
                    Else
                        .Add(pfSet_Param("ngc", SqlDbType.NVarChar, "0"))  '提出区分
                    End If
                End With

                'データ取得
                dsCsv = clsDataConnect.pfGet_DataSet(objCsvCmd)

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsProgNm)
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

    Private Function mfGetDataCsv() As DataSet

        mfGetDataCsv = Nothing

        objStack = New StackFrame
        Dim ds As DataSet
        Dim dsDtil As DataSet
        Try
            'CSVデータセットのクローンを格納
            'mfGetDataCsv = DirectCast(ViewState("dsCsv"), DataSet).Clone
            If ViewState("dsCsv") IsNot Nothing Then
                mfGetDataCsv = DirectCast(ViewState("dsCsv"), DataSet).Clone
            Else
                Dim dsWrk As New DataSet
                mfGetData(dsWrk)
                mfGetDataCsv = dsCsv.Clone
            End If


            'CSVデータセットを加工していきます
            'メインデータ部
            ds = New DataSet
            ds = ViewState("ds")

            'Dim strDataList(6) As String
            'Dim strData As String = String.Empty

            '明細データ部
            dsDtil = New DataSet
            dsDtil = ViewState("QUAUPDP001_dsDtil")

            Dim dr As DataRow
            For zz As Integer = 0 To ds.Tables(0).Rows.Count - 1
                dr = mfGetDataCsv.Tables(0).NewRow
                With dr
                    .Item("項番") = ds.Tables(0).Rows(zz).Item("項番").ToString
                    .Item("ＴＢＯＸＩＤ") = ds.Tables(0).Rows(zz).Item("ＴＢＯＸＩＤ").ToString
                    .Item("ホールコード") = ds.Tables(0).Rows(zz).Item("ホールコード").ToString
                    .Item("ホール名") = ds.Tables(0).Rows(zz).Item("ホール名").ToString
                    .Item("ホール住所") = ds.Tables(0).Rows(zz).Item("ホール住所").ToString
                    .Item("マスタ開始日") = ds.Tables(0).Rows(zz).Item("マスタ開始日").ToString
                    .Item("ＶＥＲ") = ds.Tables(0).Rows(zz).Item("ＶＥＲ").ToString
                    .Item("受付日時") = ds.Tables(0).Rows(zz).Item("受付日時").ToString
                    .Item("管理番号") = ds.Tables(0).Rows(zz).Item("管理番号").ToString
                    .Item("回復日時") = ds.Tables(0).Rows(zz).Item("回復日時").ToString
                    .Item("サンド入金停止時間") = ds.Tables(0).Rows(zz).Item("サンド入金停止時間").ToString
                    .Item("申告元コード") = ds.Tables(0).Rows(zz).Item("申告元コード").ToString
                    .Item("申告元") = ds.Tables(0).Rows(zz).Item("申告元").ToString
                    .Item("申告内容") = ds.Tables(0).Rows(zz).Item("申告内容").ToString
                    .Item("事象コード") = ds.Tables(0).Rows(zz).Item("事象コード").ToString
                    .Item("事象") = ds.Tables(0).Rows(zz).Item("事象").ToString
                    Dim strList() As String = ds.Tables(0).Rows(zz).Item("対応内容").ToString.Split(Environment.NewLine)
                    Dim strData As String
                    For yy As Integer = 0 To strList.Length - 1
                        strData = String.Empty
                        strData = strList(yy).ToString.Replace(Environment.NewLine, "").Trim
                        Select Case yy
                            Case 0
                                .Item("対応内容１") = strData
                            Case 1
                                .Item("対応内容２") = strData
                            Case 2
                                .Item("対応内容３") = strData
                            Case 3
                                .Item("対応内容４") = strData
                            Case 4
                                .Item("対応内容５") = strData
                            Case 5
                                .Item("対応内容６") = strData
                        End Select
                    Next
                    .Item("調査完了日") = ds.Tables(0).Rows(zz).Item("調査完了日").ToString
                    .Item("故障部位") = ds.Tables(0).Rows(zz).Item("故障部位").ToString
                    .Item("ステータスコード") = ds.Tables(0).Rows(zz).Item("ステータスコード").ToString
                    .Item("ステータス") = ds.Tables(0).Rows(zz).Item("ステータス").ToString
                    .Item("提出") = ds.Tables(0).Rows(zz).Item("提出").ToString
                    .Item("提出区分") = ds.Tables(0).Rows(zz).Item("提出区分").ToString

                    For yy As Integer = 0 To dsDtil.Tables(0).Rows.Count - 1
                        If ds.Tables(0).Rows(zz).Item("管理番号").ToString = dsDtil.Tables(0).Rows(yy).Item("管理番号").ToString Then
                            .Item("一時診断結果" & (yy + 1).ToString) = dsDtil.Tables(0).Rows(yy).Item("一時診断結果").ToString
                            .Item("調査及び調査状況" & (yy + 1).ToString) = "・" & dsDtil.Tables(0).Rows(yy).Item("故障部位").ToString _
                                                                          & "・・・" & dsDtil.Tables(0).Rows(yy).Item("調査及び調査状況").ToString
                        End If
                        '５明細以外は印字しない
                        If yy = 4 Then
                            Exit For
                        End If
                    Next
                End With
                mfGetDataCsv.Tables(0).Rows.Add(dr)
            Next

        Catch ex As Exception
            psMesBox(Me, "30010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後,
                     ex.ToString.Replace(Environment.NewLine, "").Replace("'", ""))
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
    End Function
#End Region

#Region "データ表示処理"
    ''' <summary>
    ''' データ表示処理
    ''' </summary>
    ''' <param name="objDs"></param>
    ''' <remarks></remarks>
    Private Sub msDispData(objDs As DataSet, objDsDtil As DataSet)

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            'グリッド及び件数の初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"

            '種別表示
            lblClass.Text = Me.ddlTboxType.ppSelectedTextOnly

            '件数を設定
            If objDs.Tables(0).Rows.Count = 0 Then
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                Master.ppCount = "0"
                Master.Master.ppLeftButton1.Enabled = False
                Master.Master.ppRigthButton1.Enabled = False
                Master.Master.ppRigthButton2.Enabled = False
            Else
                '閾値を超えた場合はメッセージを表示
                If objDs.Tables(0).Rows(0)("データ件数") > objDs.Tables(0).Rows.Count Then
                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                        objDs.Tables(0).Rows(0)("データ件数").ToString, objDs.Tables(0).Rows.Count.ToString)
                End If
                Master.ppCount = objDs.Tables(0).Rows(0)("データ件数").ToString
                Master.Master.ppLeftButton1.Enabled = True
                Master.Master.ppRigthButton1.Enabled = True
                Master.Master.ppRigthButton2.Enabled = True
            End If

            '取得したデータをグリッドに設定
            Me.grvList.DataSource = objDs.Tables(0)

            Dim intMaxLine As Integer   'MAXの表示行をカウントする
            Dim strData As String = String.Empty
            Dim strData2 As String = String.Empty
            Dim strData3 As String = String.Empty
            For zz As Integer = 0 To objDs.Tables(0).Rows.Count - 1
                strData3 = objDs.Tables(0).Rows(zz).Item("対応内容").ToString & Environment.NewLine & _
                           objDs.Tables(0).Rows(zz).Item("対応内容２").ToString & Environment.NewLine & _
                           objDs.Tables(0).Rows(zz).Item("対応内容３").ToString & Environment.NewLine & _
                           objDs.Tables(0).Rows(zz).Item("対応内容４").ToString & Environment.NewLine & _
                           objDs.Tables(0).Rows(zz).Item("対応内容５").ToString & Environment.NewLine & _
                           objDs.Tables(0).Rows(zz).Item("対応内容６").ToString
                objDs.Tables(0).Rows(zz).Item("対応内容") = strData3
                strData3 = String.Empty

                'カウント初期化
                intMaxLine = 0
                For yy As Integer = 0 To objDsDtil.Tables(0).Rows.Count - 1
                    'データテーブルから６行目以降を削除する
                    If intMaxLine > 4 Then
                        If objDs.Tables(0).Rows(zz).Item("管理番号").ToString = objDsDtil.Tables(0).Rows(yy).Item("管理番号").ToString Then
                            objDsDtil.Tables(0).Rows(yy).Delete()
                        End If
                    Else
                        If strData <> String.Empty Then
                            strData = strData & Environment.NewLine
                        End If
                        If objDs.Tables(0).Rows(zz).Item("管理番号").ToString = objDsDtil.Tables(0).Rows(yy).Item("管理番号").ToString Then
                            strData = strData & "・" _
                                    & objDsDtil.Tables(0).Rows(yy).Item("故障部位").ToString & "・・・" _
                                    & objDsDtil.Tables(0).Rows(yy).Item("調査及び調査状況").ToString

                            If strData2 <> String.Empty Then
                                strData2 = strData2 & Environment.NewLine
                            End If
                            strData2 = strData2 & objDsDtil.Tables(0).Rows(yy).Item("一時診断結果").ToString

                            'カウント加算
                            intMaxLine = intMaxLine + 1
                        End If
                    End If
                Next
                objDsDtil.Tables(0).AcceptChanges()

                objDs.Tables(0).Rows(zz).Item("調査及び調査状況") = strData
                strData = String.Empty

                objDs.Tables(0).Rows(zz).Item("一時診断結果") = strData2
                strData2 = String.Empty
            Next

            '変更を反映
            Me.grvList.DataBind()

            'ビューステートに退避
            If dtbReportDt.ppFromText = String.Empty Then
                ViewState("SupportFrom") = String.Empty
            Else
                ViewState("SupportFrom") = dtbReportDt.ppFromDate.ToString("yyyy年M月d日")
            End If
            If dtbReportDt.ppToText = String.Empty Then
                ViewState("SupportTo") = String.Empty
            Else
                ViewState("SupportTo") = dtbReportDt.ppToDate.ToString("yyyy年M月d日")
            End If
            ViewState("ds") = objDs
            ViewState("dsCsv") = dsCsv
            ViewState("QUAUPDP001_dsDtil") = objDsDtil

        Catch ex As Exception
            psMesBox(Me, "30010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後,
                     ex.ToString.Replace(Environment.NewLine, "").Replace("'", ""))
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
#End Region

#Region "品質会議資料データ登録処理"
    ''' <summary>
    ''' 品質会議資料データ登録処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfEntryData() As Boolean

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objTrn As SqlTransaction = Nothing  'SqlTransactionクラス
        Dim intRtn As Integer = 0               'ストアド戻り値
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfEntryData = False

        If clsDataConnect.pfOpen_Database(objCn) Then
            Try
                'トランザクション開始
                objTrn = objCn.BeginTransaction()

                Dim dsDtil As New DataSet
                dsDtil = ViewState("QUAUPDP001_dsDtil")
                Dim drWrk() As DataRow

                For Each rowData As GridViewRow In grvList.Rows

                    'データ種別が"2:保守対応データからの取得"の場合、品質会議資料データに登録
                    If CType(rowData.FindControl("データ種別"), TextBox).Text = "2" Then

                        objCmd = New SqlCommand(sCnsProgId + "_U1", objCn)

                        'コマンドタイプ設定(ストアド)
                        objCmd.CommandType = CommandType.StoredProcedure

                        'パラメータ設定
                        With objCmd.Parameters

                            '--管理番号
                            .Add(pfSet_Param("meeting_no", SqlDbType.NVarChar,
                                             CType(rowData.FindControl("管理番号"), TextBox).Text))
                            '--枝番
                            .Add(pfSet_Param("branch", SqlDbType.NVarChar, "1"))
                            '--種別コード
                            .Add(pfSet_Param("class", SqlDbType.NVarChar,
                                             CType(rowData.FindControl("種別コード"), TextBox).Text))
                            '--ＴＢＯＸＩＤ
                            .Add(pfSet_Param("tboxid", SqlDbType.NVarChar,
                                             CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text))
                            '--ＶＥＲ
                            .Add(pfSet_Param("tbox_ver", SqlDbType.NVarChar,
                                             CType(rowData.FindControl("ＶＥＲ"), TextBox).Text))
                            '--ホールコード
                            .Add(pfSet_Param("hall_cd", SqlDbType.NVarChar,
                                             CType(rowData.FindControl("ホールコード"), TextBox).Text))
                            '--ホール名
                            .Add(pfSet_Param("hall_nm", SqlDbType.NVarChar,
                                             CType(rowData.FindControl("ホール名"), TextBox).Text))
                            '--受付日時
                            If CType(rowData.FindControl("受付日時"), TextBox).Text = String.Empty Then
                                .Add(pfSet_Param("receive_dt", SqlDbType.NVarChar, DBNull.Value))
                            Else
                                .Add(pfSet_Param("receive_dt", SqlDbType.NVarChar,
                                                 CType(rowData.FindControl("受付日時"), TextBox).Text.Replace(Environment.NewLine, "")))
                            End If
                            '--マスタ開始日(yyyy/MM/dd)
                            If CType(rowData.FindControl("マスタ開始日"), TextBox).Text = String.Empty Then
                                .Add(pfSet_Param("tbox_str_dt", SqlDbType.NVarChar, DBNull.Value))
                            Else
                                .Add(pfSet_Param("tbox_str_dt", SqlDbType.NVarChar,
                                                 CType(rowData.FindControl("マスタ開始日"), TextBox).Text.Replace(Environment.NewLine, "")))
                            End If
                            '--回復日時(yyyy/MM/dd HH:mm)
                            If CType(rowData.FindControl("回復日時"), TextBox).Text = String.Empty Then
                                .Add(pfSet_Param("recovery_dt", SqlDbType.NVarChar, DBNull.Value))
                            Else
                                .Add(pfSet_Param("recovery_dt", SqlDbType.NVarChar,
                                                 CType(rowData.FindControl("回復日時"), TextBox).Text.Replace(Environment.NewLine, "")))
                            End If
                            '--サンド入金停止時間
                            .Add(pfSet_Param("sand_stop", SqlDbType.NVarChar,
                                             CType(rowData.FindControl("サンド入金停止時間"), TextBox).Text))
                            '--申告元コード
                            .Add(pfSet_Param("report_cd", SqlDbType.NVarChar,
                                             CType(rowData.FindControl("申告元コード"), TextBox).Text))
                            '--申告元
                            .Add(pfSet_Param("report_nm", SqlDbType.NVarChar,
                                             CType(rowData.FindControl("申告元"), TextBox).Text))
                            '--申告内容
                            .Add(pfSet_Param("report_con", SqlDbType.NVarChar,
                                             CType(rowData.FindControl("申告内容"), TextBox).Text))
                            '--事象コード
                            .Add(pfSet_Param("event_cd", SqlDbType.NVarChar,
                                             CType(rowData.FindControl("事象コード"), TextBox).Text))
                            '--事象
                            .Add(pfSet_Param("event", SqlDbType.NVarChar,
                                             CType(rowData.FindControl("事象"), TextBox).Text))
                            '--一時診断結果
                            .Add(pfSet_Param("diagnosis_res", SqlDbType.NVarChar,
                                             CType(rowData.FindControl("一時診断結果"), TextBox).Text))
                            '--故障部位
                            .Add(pfSet_Param("faultsite_nm", SqlDbType.NVarChar,
                                             CType(rowData.FindControl("故障部位"), TextBox).Text))
                            '--調査完了日(yyyy/MM/dd)
                            If CType(rowData.FindControl("調査完了日"), TextBox).Text = String.Empty Then
                                .Add(pfSet_Param("surveycmp_dt", SqlDbType.NVarChar, DBNull.Value))
                            Else
                                .Add(pfSet_Param("surveycmp_dt", SqlDbType.NVarChar,
                                                 CType(rowData.FindControl("調査完了日"), TextBox).Text.Replace(Environment.NewLine, "")))
                            End If
                            '--調査及び調査状況（※明細にデータを保持することになったので、空文字入れる）
                            .Add(pfSet_Param("survey_sts", SqlDbType.NVarChar, ""))
                            '--ステータスコード
                            .Add(pfSet_Param("status_cd", SqlDbType.NVarChar,
                                             CType(rowData.FindControl("ステータスコード"), TextBox).Text))
                            '--ステータス
                            .Add(pfSet_Param("status_nm", SqlDbType.NVarChar,
                                             CType(rowData.FindControl("ステータス"), TextBox).Text))
                            '--対応日（開始日時）
                            If CType(rowData.FindControl("開始日時"), TextBox).Text = String.Empty Then
                                .Add(pfSet_Param("deal_dt", SqlDbType.NVarChar, DBNull.Value))
                            Else
                                .Add(pfSet_Param("deal_dt", SqlDbType.NVarChar,
                                                 CType(rowData.FindControl("開始日時"), TextBox).Text.Replace(Environment.NewLine, "")))
                            End If
                            '--提出区分
                            .Add(pfSet_Param("sub_miss_cls", SqlDbType.NVarChar,
                                             CType(rowData.FindControl("提出"), TextBox).Text))
                            '--センタ区分（ＮＬ区分）
                            .Add(pfSet_Param("nl_cls", SqlDbType.NVarChar,
                                             CType(rowData.FindControl("ＮＬ"), TextBox).Text))
                            '--ユーザＩＤ
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))
                            '--項番
                            .Add(pfSet_Param("kouban", SqlDbType.NVarChar, 7, ParameterDirection.Output))
                            '--戻り値
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                            '--対応内容シリーズ
                            Dim strData(6) As String
                            strData = CType(rowData.FindControl("対応内容"), TextBox).Text.Split(Environment.NewLine)
                            '--対応内容１
                            .Add(pfSet_Param("repect1", SqlDbType.NVarChar, strData(0)))
                            '--対応内容２
                            .Add(pfSet_Param("repect2", SqlDbType.NVarChar, strData(1)))
                            '--対応内容３
                            .Add(pfSet_Param("repect3", SqlDbType.NVarChar, strData(2)))
                            '--対応内容４
                            .Add(pfSet_Param("repect4", SqlDbType.NVarChar, strData(3)))
                            '--対応内容５
                            .Add(pfSet_Param("repect5", SqlDbType.NVarChar, strData(4)))
                            '--対応内容６
                            .Add(pfSet_Param("repect6", SqlDbType.NVarChar, strData(5)))
                        End With

                        'トランザクション設定
                        objCmd.Transaction = objTrn

                        'ストアド実行
                        objCmd.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(objCmd.Parameters("retvalue").Value.ToString)
                        Select Case intRtn
                            Case 0  '正常終了
                                '項番の取得
                                CType(rowData.FindControl("項番"), TextBox).Text = objCmd.Parameters("kouban").Value.ToString()

                                'データ種別を"1:品質会議資料データからの取得"に変更する
                                CType(rowData.FindControl("データ種別"), TextBox).Text = "1"

                                '選択ボタンを活性にし、文字を赤くする
                                rowData.Cells(0).Enabled = True
                                For i = 1 To rowData.Cells.Count - 1
                                    CType(rowData.Cells(i).Controls.Item(0), TextBox).ForeColor = Drawing.Color.Red
                                Next

                                '登録フラグを"1:登録"に変更する
                                CType(rowData.FindControl("登録フラグ"), TextBox).Text = "1"

                            Case 2627 'キー重複エラーはスキップ
                                '登録フラグを"0:未登録"にする
                                CType(rowData.FindControl("登録フラグ"), TextBox).Text = "0"

                            Case Else
                                psMesBox(Me, "00007", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsProgNm, intRtn.ToString)
                                Exit Function
                        End Select


                        '品質会議資料明細詳細情報登録処理
                        Dim intSeq As Integer = 0

                        drWrk = dsDtil.Tables(0).Select("管理番号='" & CType(rowData.FindControl("管理番号"), TextBox).Text & "'")

                        For zz As Integer = 0 To drWrk.Count - 1

                            '５明細までしか登録しない
                            If zz = 5 Then
                                Exit For
                            End If

                            objCmd = New SqlCommand(sCnsProgId + "_U3", objCn)

                            'コマンドタイプ設定(ストアド)
                            objCmd.CommandType = CommandType.StoredProcedure

                            '枝番を自動採番する
                            intSeq = intSeq + 1

                            'パラメータ設定
                            With objCmd.Parameters
                                .Add(pfSet_Param("meeting_no", SqlDbType.NVarChar, drWrk(zz).Item("管理番号").ToString))

                                '.Add(pfSet_Param("branch", SqlDbType.NVarChar, drWrk(zz).Item("枝番").ToString))
                                '採番した番号を入れる
                                .Add(pfSet_Param("branch", SqlDbType.NVarChar, intSeq.ToString))

                                .Add(pfSet_Param("app_cod", SqlDbType.NVarChar, drWrk(zz).Item("故障部位コード").ToString))
                                .Add(pfSet_Param("faultsite_nm", SqlDbType.NVarChar, drWrk(zz).Item("故障部位").ToString))
                                .Add(pfSet_Param("tmp_cod", SqlDbType.NVarChar, drWrk(zz).Item("一時診断コード").ToString))
                                .Add(pfSet_Param("survey_sts", SqlDbType.NVarChar, drWrk(zz).Item("調査及び調査状況").ToString))
                                .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))
                                If drWrk(zz).Item("管理番号枝番").ToString.ToString = String.Empty Then
                                    .Add(pfSet_Param("@rep_brc", SqlDbType.NVarChar, DBNull.Value))
                                Else
                                    .Add(pfSet_Param("@rep_brc", SqlDbType.NVarChar, drWrk(zz).Item("管理番号枝番").ToString))
                                End If
                                .Add(pfSet_Param("@seq_no", SqlDbType.NVarChar, zz + 1))
                                '--戻り値
                                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))

                            End With

                            'トランザクション設定
                            objCmd.Transaction = objTrn

                            'ストアド実行
                            objCmd.ExecuteNonQuery()

                            'ストアド戻り値チェック
                            intRtn = Integer.Parse(objCmd.Parameters("retvalue").Value.ToString)
                            Select Case intRtn
                                Case 0  '正常終了

                                Case 2627 'キー重複エラーはスキップ
                                    '登録フラグを"0:未登録"にする
                                    CType(rowData.FindControl("登録フラグ"), TextBox).Text = "0"

                                Case Else
                                    psMesBox(Me, "00007", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsProgNm, intRtn.ToString)
                                    Exit Function
                            End Select
                        Next
                    End If
                Next

                psMesBox(Me, "00009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "品質会議資料明細")

                'コミット
                objTrn.Commit()

                mfEntryData = True

            Catch ex As Exception
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsProgNm)
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
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Function

#End Region

#Region "品質会議資料データ更新処理"
    ''' <summary>
    ''' 品質会議資料データ更新処理
    ''' </summary>
    ''' <param name="intMode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfUpdateDataDetail(intMode As Integer) As Boolean

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objTrn As SqlTransaction = Nothing  'SqlTransactionクラス
        Dim intRtn As Integer = 0               'ストアド戻り値
        Dim strSpName As String = String.Empty  'ストアド名
        Dim strErrCode As String = String.Empty 'エラーコード
        Dim strErrCode2 As String = String.Empty 'エラーコード２
        Dim strBuff As String = String.Empty
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfUpdateDataDetail = False

        '実行ストアド名設定
        Select Case intMode
            Case 1  '追加
                strSpName = sCnsProgId + "_U1"
                strErrCode = "00003"
                strErrCode2 = "00008"
            Case 2  '更新
                strSpName = sCnsProgId + "_U2"
                strErrCode = "00001"
                strErrCode2 = "00007"
            Case 3  '削除
                strSpName = sCnsProgId + "_D1"
                strErrCode = "00002"
                strErrCode2 = "00009"
            Case Else
                Exit Function
        End Select

        'シリアルデータ更新処理
        If clsDataConnect.pfOpen_Database(objCn) Then
            Try
                objTrn = objCn.BeginTransaction()
                objCmd = New SqlCommand(strSpName, objCn)
                'コマンドタイプ設定(ストアド)
                objCmd.CommandType = CommandType.StoredProcedure

                With objCmd.Parameters

                    If intMode = 2 OrElse intMode = 3 Then
                        '--項番
                        .Add(pfSet_Param("kouban", SqlDbType.NVarChar, lblNo.Text))
                    End If

                    '--管理番号
                    .Add(pfSet_Param("meeting_no", SqlDbType.NVarChar, txtKanriNo.ppText))

                    '--枝番
                    '.Add(pfSet_Param("branch", SqlDbType.NVarChar, txtEdaban.ppText))
                    .Add(pfSet_Param("branch", SqlDbType.NVarChar, 1))

                    If intMode = 1 OrElse intMode = 2 Then

                        '追加時に基本情報がすべてそろっていなければ再取得にいきましょう
                        If intMode = 1 Then
                            If lblClassCd.Text = String.Empty AndAlso lblClassNm.Text = String.Empty AndAlso lblNLClsCd.Text = String.Empty _
                                AndAlso lblHallCd.Text = String.Empty AndAlso lblHallAdr.Text = String.Empty AndAlso lblDealDt.Text = String.Empty Then
                                Call txtKanriNo_TextChanged()
                            End If
                        End If

                        '--種別コード
                        .Add(pfSet_Param("class", SqlDbType.NVarChar, lblClassCd.Text))
                        '--ＴＢＯＸＩＤ
                        .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, lblTboxId.Text))
                        '--ＶＥＲ
                        .Add(pfSet_Param("tbox_ver", SqlDbType.NVarChar, lblVer.Text))
                        '--ホールコード
                        .Add(pfSet_Param("hall_cd", SqlDbType.NVarChar, lblHallCd.Text))
                        '--ホール名
                        .Add(pfSet_Param("hall_nm", SqlDbType.NVarChar, lblHallNm.Text))
                        '--受付日時
                        If dtbUketukeDt.ppText = String.Empty AndAlso
                           dtbUketukeDt.ppHourText = String.Empty AndAlso
                           dtbUketukeDt.ppMinText = String.Empty Then
                            .Add(pfSet_Param("receive_dt", SqlDbType.NVarChar, DBNull.Value))
                        Else
                            .Add(pfSet_Param("receive_dt", SqlDbType.NVarChar, dtbUketukeDt.ppText + " " +
                                             dtbUketukeDt.ppHourText + ":" + dtbUketukeDt.ppMinText))
                        End If
                        '--マスタ開始日(yyyy/MM/dd)
                        If lblStartDt.Text = String.Empty Then
                            .Add(pfSet_Param("tbox_str_dt", SqlDbType.NVarChar, DBNull.Value))
                        Else
                            .Add(pfSet_Param("tbox_str_dt", SqlDbType.NVarChar, lblStartDt.Text))
                        End If
                        '--回復日時(yyyy/MM/dd HH:mm)
                        If dtbKaifukuDt.ppText = String.Empty AndAlso
                           dtbKaifukuDt.ppHourText = String.Empty AndAlso
                           dtbKaifukuDt.ppMinText = String.Empty Then
                            .Add(pfSet_Param("recovery_dt", SqlDbType.NVarChar, DBNull.Value))
                        Else
                            .Add(pfSet_Param("recovery_dt", SqlDbType.NVarChar, dtbKaifukuDt.ppText + " " +
                                             dtbKaifukuDt.ppHourText + ":" + dtbKaifukuDt.ppMinText))
                        End If
                        '--サンド入金停止時間
                        .Add(pfSet_Param("sand_stop", SqlDbType.NVarChar, txtStopTime.ppText))
                        '--申告元コード
                        .Add(pfSet_Param("report_cd", SqlDbType.NVarChar, ddlReport.SelectedValue))
                        '--申告元
                        .Add(pfSet_Param("report_nm", SqlDbType.NVarChar, mfGetNamePart(ddlReport.SelectedItem.Text)))
                        '--申告内容
                        .Add(pfSet_Param("report_con", SqlDbType.NVarChar, txtReportCon.ppText))
                        '--事象コード
                        'QUAUPDP001-001
                        If ddlEvent.SelectedIndex = 0 Then
                            .Add(pfSet_Param("event_cd", SqlDbType.NVarChar, ddlEvent.SelectedValue))
                        Else
                            .Add(pfSet_Param("event_cd", SqlDbType.NVarChar, ddlEvent.SelectedItem.Text.Remove(2)))
                        End If
                        'QUAUPDP001-001 END
                        '--事象
                        .Add(pfSet_Param("event", SqlDbType.NVarChar, mfGetNamePart(ddlEvent.SelectedItem.Text)))
                        '--一時診断結果（※明細にデータを保持することになったので、空文字入れる）
                        .Add(pfSet_Param("diagnosis_res", SqlDbType.NVarChar, ""))
                        '--故障部位
                        .Add(pfSet_Param("faultsite_nm", SqlDbType.NVarChar, txtBuiKoshou.ppText))
                        '--調査完了日(yyyy/MM/dd)
                        If dtbChousaEnd.ppText = String.Empty Then
                            .Add(pfSet_Param("surveycmp_dt", SqlDbType.NVarChar, DBNull.Value))
                        Else
                            .Add(pfSet_Param("surveycmp_dt", SqlDbType.NVarChar, dtbChousaEnd.ppText))
                        End If
                        '--調査及び調査状況（※明細にデータを保持することになったので、空文字入れる）
                        .Add(pfSet_Param("survey_sts", SqlDbType.NVarChar, ""))
                        '--ステータスコード
                        .Add(pfSet_Param("status_cd", SqlDbType.NVarChar, ddlStatusCd.SelectedValue))
                        '--ステータス
                        .Add(pfSet_Param("status_nm", SqlDbType.NVarChar, mfGetNamePart(ddlStatusCd.SelectedItem.Text)))
                        '--対応日（開始日時）
                        If lblDealDt.Text = String.Empty Then
                            .Add(pfSet_Param("deal_dt", SqlDbType.NVarChar, DBNull.Value))
                        Else
                            .Add(pfSet_Param("deal_dt", SqlDbType.NVarChar, lblDealDt.Text))
                        End If
                        '--提出区分
                        .Add(pfSet_Param("sub_miss_cls", SqlDbType.NVarChar, ddlRstSubmsnCls.ppSelectedValue))
                        '--対応内容１
                        .Add(pfSet_Param("repect1", SqlDbType.NVarChar, txtRepect.ppText))
                        '--対応内容２
                        .Add(pfSet_Param("repect2", SqlDbType.NVarChar, txtRepect2.ppText))
                        '--対応内容３
                        .Add(pfSet_Param("repect3", SqlDbType.NVarChar, txtRepect3.ppText))
                        '--対応内容４
                        .Add(pfSet_Param("repect4", SqlDbType.NVarChar, txtRepect4.ppText))
                        '--対応内容５
                        .Add(pfSet_Param("repect5", SqlDbType.NVarChar, txtRepect5.ppText))
                        '--対応内容６
                        .Add(pfSet_Param("repect6", SqlDbType.NVarChar, txtRepect6.ppText))
                        '--センタ区分（ＮＬ区分）
                        .Add(pfSet_Param("nl_cls", SqlDbType.NVarChar, lblNLClsCd.Text))
                        '--ユーザＩＤ
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))
                    End If

                    If intMode = 1 Then
                        .Add(pfSet_Param("kouban", SqlDbType.NVarChar, 7, ParameterDirection.Output))   '項番
                    End If

                    '--戻り値
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                End With

                objCmd.Transaction = objTrn
                'ストアド実行
                objCmd.ExecuteNonQuery()
                'ストアド戻り値チェック
                intRtn = Integer.Parse(objCmd.Parameters("retvalue").Value.ToString)

                If intRtn <> 0 Then
                    psMesBox(Me, strErrCode2, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsProgNm, intRtn.ToString)
                    Exit Function
                End If

                '項番の取得
                If intMode = 1 Then
                    lblNo.Text = objCmd.Parameters("kouban").Value.ToString()
                End If

                'ここから明細の処理
                '品質会議資料明細詳細情報登録処理(最大５明細までしか登録しない)
                Select Case intMode
                    Case 1, 2
                        strSpName = sCnsProgId + "_U4"
                        strErrCode = "00001"
                        strErrCode2 = "00007"
                    Case 3
                        'コミット
                        objTrn.Commit()
                        mfUpdateDataDetail = True
                        Exit Function
                End Select

                'まず現状の枝番最大値を取得
                Dim strMaxSeq As String = String.Empty
                Dim intMaxSeq As Integer
                Dim objCn2 As SqlConnection = Nothing    'SqlConnectionクラス
                Dim objCmd2 As SqlCommand = Nothing      'SqlCommandクラス
                If clsDataConnect.pfOpen_Database(objCn2) Then
                    Try
                        objCmd2 = New SqlCommand(strSpName, objCn2)
                        objCmd2.CommandType = CommandType.StoredProcedure

                        objCmd2 = New SqlCommand(sCnsProgId + "_S7", objCn2)
                        With objCmd2.Parameters '--パラメータ設定
                            .Add(pfSet_Param("meeting_no", SqlDbType.NVarChar, txtKanriNo.ppText))  '管理番号
                        End With
                        Dim ds As DataSet = Nothing
                        ds = clsDataConnect.pfGet_DataSet(objCmd2)

                        strMaxSeq = ds.Tables(0).Rows(0).Item("最大枝番").ToString()
                        If strMaxSeq = String.Empty Then
                            intMaxSeq = 0
                        Else
                            intMaxSeq = Integer.Parse(strMaxSeq)
                        End If
                    Catch ex As Exception
                        objTrn.Rollback()
                        psMesBox(Me, strErrCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsProgNm)
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    Finally
                        'DB切断
                        If Not clsDataConnect.pfClose_Database(objCn2) Then
                            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        End If
                    End Try
                End If

                '明細追加、更新処理開始
                Dim strSeqList As String = String.Empty '追加、更新した枝番保存用(カンマでつなげた形式を保存xx,xx,xx)
                Dim strFlg_Delete As String         '削除判定用フラグ
                Dim objhdnSeq As HiddenField        '枝番用
                Dim objhdnEdaban As HiddenField     '管理番号枝番用
                Dim objddlKosyou As DropDownList    '故障部位用
                Dim objddlIchiji As DropDownList    '一時診断結果用
                Dim objtxtChousa As TextBox         '調査状況用

                Dim strSeq As String = String.Empty
                For zz As Integer = 0 To 4
                    '変数初期化
                    strFlg_Delete = "0"
                    objhdnSeq = Nothing
                    objhdnEdaban = Nothing
                    objddlKosyou = Nothing
                    objddlIchiji = Nothing
                    objtxtChousa = Nothing

                    'ストアドコマンド設定
                    objCmd = New SqlCommand(strSpName, objCn)
                    objCmd.CommandType = CommandType.StoredProcedure

                    'パラメータ設定
                    With objCmd.Parameters

                        Select Case zz
                            Case 0
                                objhdnSeq = Me.hdnSeq1
                                objhdnEdaban = Me.hdnEdaban1
                                objddlKosyou = Me.ddlKosyoBui1
                                objddlIchiji = Me.ddlIchijiShindan1
                                objtxtChousa = Me.txtChousaJyokyou1
                            Case 1
                                objhdnSeq = Me.hdnSeq2
                                objhdnEdaban = Me.hdnEdaban2
                                objddlKosyou = Me.ddlKosyoBui2
                                objddlIchiji = Me.ddlIchijiShindan2
                                objtxtChousa = Me.txtChousaJyokyou2
                            Case 2
                                objhdnSeq = Me.hdnSeq3
                                objhdnEdaban = Me.hdnEdaban3
                                objddlKosyou = Me.ddlKosyoBui3
                                objddlIchiji = Me.ddlIchijiShindan3
                                objtxtChousa = Me.txtChousaJyokyou3
                            Case 3
                                objhdnSeq = Me.hdnSeq4
                                objhdnEdaban = Me.hdnEdaban4
                                objddlKosyou = Me.ddlKosyoBui4
                                objddlIchiji = Me.ddlIchijiShindan4
                                objtxtChousa = Me.txtChousaJyokyou4
                            Case 4
                                objhdnSeq = Me.hdnSeq5
                                objhdnEdaban = Me.hdnEdaban5
                                objddlKosyou = Me.ddlKosyoBui5
                                objddlIchiji = Me.ddlIchijiShindan5
                                objtxtChousa = Me.txtChousaJyokyou5
                        End Select

                        '初めて非活性行が検出されたら処理しないのでループ出る
                        If objddlKosyou.Enabled = False Then
                            Exit For
                        End If

                        '枝番がないもの（新規）に対して枝番を振る
                        If objhdnSeq.Value = String.Empty Then
                            intMaxSeq = intMaxSeq + 1
                            strSeq = intMaxSeq.ToString()
                        Else
                            strSeq = objhdnSeq.Value
                        End If

                        .Add(pfSet_Param("meeting_no", SqlDbType.NVarChar, txtKanriNo.ppText))
                        .Add(pfSet_Param("branch", SqlDbType.NVarChar, strSeq))
                        .Add(pfSet_Param("app_cod", SqlDbType.NVarChar, objddlKosyou.SelectedValue))
                        .Add(pfSet_Param("faultsite_nm", SqlDbType.NVarChar, ""))
                        .Add(pfSet_Param("tmp_cod", SqlDbType.NVarChar, objddlIchiji.SelectedValue))
                        .Add(pfSet_Param("survey_sts", SqlDbType.NVarChar, objtxtChousa.Text))
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))
                        If objhdnEdaban.Value = String.Empty Then
                            .Add(pfSet_Param("rep_branch", SqlDbType.NVarChar, DBNull.Value))
                        Else
                            .Add(pfSet_Param("rep_branch", SqlDbType.NVarChar, objhdnEdaban.Value))
                        End If

                        .Add(pfSet_Param("seq_no", SqlDbType.SmallInt, zz + 1))
                        .Add(pfSet_Param("flg_delete", SqlDbType.NVarChar, strFlg_Delete))
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar)) '戻り値

                        '削除用の枝番リストを追加していく
                        If zz <> 0 Then
                            strSeqList = strSeqList & ","
                        End If
                        strSeqList = strSeqList & strSeq

                    End With

                    'トランザクション設定
                    objCmd.Transaction = objTrn
                    'ストアド実行
                    objCmd.ExecuteNonQuery()
                    'ストアド戻り値チェック
                    If Integer.Parse(objCmd.Parameters("retvalue").Value.ToString) <> 0 Then
                        objTrn.Rollback()
                        psMesBox(Me, strErrCode2, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsProgNm, intRtn.ToString)
                        Exit Function
                    End If
                Next

                '明細削除処理開始（追加、更新後、処理されなかった枝番のデータを削除する）
                'ストアドコマンド設定
                objCmd = New SqlCommand(sCnsProgId + "_D2", objCn)
                objCmd.CommandType = CommandType.StoredProcedure
                'パラメータ設定
                With objCmd.Parameters
                    .Add(pfSet_Param("meeting_no", SqlDbType.NVarChar, txtKanriNo.ppText))
                    .Add(pfSet_Param("seq_list", SqlDbType.NVarChar, strSeqList))
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar)) '戻り値
                End With

                'トランザクション設定
                objCmd.Transaction = objTrn
                'ストアド実行
                objCmd.ExecuteNonQuery()
                'ストアド戻り値チェック
                If Integer.Parse(objCmd.Parameters("retvalue").Value.ToString) <> 0 Then
                    objTrn.Rollback()
                    psMesBox(Me, strErrCode2, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsProgNm, intRtn.ToString)
                    Exit Function
                End If

                'コミット
                objTrn.Commit()

                mfUpdateDataDetail = True

            Catch ex As Exception

                objTrn.Rollback()
                psMesBox(Me, strErrCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsProgNm)
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
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Function
#End Region

#Region "明細クリア処理"
    ''' <summary>
    ''' 明細クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearDetail()

        'ドロップダウンリスト生成
        mfGet_DropDownList_Sel()

        lblNo.Text = String.Empty                        '項番
        txtKanriNo.ppText = String.Empty                 '管理番号
        lblTboxId.Text = String.Empty                    'ＴＢＯＸＩＤ
        lblVer.Text = String.Empty                       'ＶＥＲ
        lblHallNm.Text = String.Empty                    'ホール名
        dtbUketukeDt.ppDateBox.Text = String.Empty       '受付日
        dtbUketukeDt.ppHourBox.Text = String.Empty       '受付日時   
        dtbUketukeDt.ppMinBox.Text = String.Empty        '受付日分
        lblStartDt.Text = String.Empty                   'マスタ開始日
        dtbKaifukuDt.ppText = String.Empty               '回復日
        dtbKaifukuDt.ppHourText = String.Empty           '回復日時
        dtbKaifukuDt.ppMinText = String.Empty            '回復日分
        txtStopTime.ppText = String.Empty                'サンド入金停止時間
        ddlReport.SelectedIndex = 0                      '申告元
        txtReportCon.ppText = String.Empty               '申告内容
        ddlEvent.SelectedIndex = 0                       '事象
        txtBuiKoshou.ppText = String.Empty               '部位故障
        dtbChousaEnd.ppText = String.Empty               '調査完了日

        rdbEdaban1.Checked = False
        rdbEdaban2.Checked = False
        rdbEdaban3.Checked = False
        rdbEdaban4.Checked = False
        rdbEdaban5.Checked = False
        hdnSeq1.Value = String.Empty
        hdnSeq2.Value = String.Empty
        hdnSeq3.Value = String.Empty
        hdnSeq4.Value = String.Empty
        hdnSeq5.Value = String.Empty
        hdnEdaban1.Value = String.Empty
        hdnEdaban2.Value = String.Empty
        hdnEdaban3.Value = String.Empty
        hdnEdaban4.Value = String.Empty
        hdnEdaban5.Value = String.Empty
        ddlKosyoBui1.SelectedIndex = 0
        ddlKosyoBui2.SelectedIndex = 0
        ddlKosyoBui3.SelectedIndex = 0
        ddlKosyoBui4.SelectedIndex = 0
        ddlKosyoBui5.SelectedIndex = 0
        ddlIchijiShindan1.SelectedIndex = 0
        ddlIchijiShindan2.SelectedIndex = 0
        ddlIchijiShindan3.SelectedIndex = 0
        ddlIchijiShindan4.SelectedIndex = 0
        ddlIchijiShindan5.SelectedIndex = 0
        txtChousaJyokyou1.Text = String.Empty           '調査及び調査状況1
        txtChousaJyokyou2.Text = String.Empty           '調査及び調査状況2
        txtChousaJyokyou3.Text = String.Empty           '調査及び調査状況3
        txtChousaJyokyou4.Text = String.Empty           '調査及び調査状況4
        txtChousaJyokyou5.Text = String.Empty           '調査及び調査状況5

        ddlStatusCd.SelectedIndex = 0                    'ステータス
        lblNLClsNm.Text = String.Empty                   'ＮＬ区分（名称）
        ddlRstSubmsnCls.ppDropDownList.SelectedIndex = 0 '提出区分

        txtRepect.ppText = String.Empty                  '対応内容１
        txtRepect2.ppText = String.Empty                  '対応内容２
        txtRepect3.ppText = String.Empty                  '対応内容３
        txtRepect4.ppText = String.Empty                  '対応内容４
        txtRepect5.ppText = String.Empty                  '対応内容５
        txtRepect6.ppText = String.Empty                  '対応内容６

        '非表示項目
        hdnChildWindow.Value = "0"
        hdnKanriNo.Value = String.Empty
        hdnCheckRow.Value = String.Empty

        lblClassCd.Text = String.Empty                   '種別コード
        lblClassNm.Text = String.Empty                   '種別名称
        lblNLClsCd.Text = String.Empty                   'ＮＬ区分（コード）
        lblHallCd.Text = String.Empty                    'ホールコード
        lblHallAdr.Text = String.Empty                   'ホール住所
        lblDealDt.Text = String.Empty                    '対応日（開始日時）

        '管理番号・枝番活性
        txtKanriNo.ppEnabled = True

        'ボタン活性・非活性
        btnClear.Enabled = True     'クリアボタン
        btnInsert.Enabled = True    '追加ボタン
        btnUpdate.Enabled = False   '更新ボタン
        btnDelete.Enabled = False   '削除ボタン
        btnListClear.Enabled = True '明細クリアボタン

        '明細選択ラジオボタン活性・非活性
        rdbEdaban1.Enabled = False
        rdbEdaban2.Enabled = False
        rdbEdaban3.Enabled = False
        rdbEdaban4.Enabled = False
        rdbEdaban5.Enabled = False

        '故障部位活性・非活性
        ddlKosyoBui1.Enabled = False
        ddlKosyoBui2.Enabled = False
        ddlKosyoBui3.Enabled = False
        ddlKosyoBui4.Enabled = False
        ddlKosyoBui5.Enabled = False

        '一時診断活性・非活性
        ddlIchijiShindan1.Enabled = False
        ddlIchijiShindan2.Enabled = False
        ddlIchijiShindan3.Enabled = False
        ddlIchijiShindan4.Enabled = False
        ddlIchijiShindan5.Enabled = False

        '調査及び調査状況活性・非活性
        txtChousaJyokyou1.Enabled = False
        txtChousaJyokyou2.Enabled = False
        txtChousaJyokyou3.Enabled = False
        txtChousaJyokyou4.Enabled = False
        txtChousaJyokyou5.Enabled = False

        '明細部分のボタン
        btnGetRep.Enabled = False
        btnRowAdd.Enabled = False
        btnRowDel.Enabled = False
        btnRowUp.Enabled = False
        btnRowDn.Enabled = False

    End Sub
#End Region

#Region "ドロップダウンリスト生成処理"
    ''' <summary>
    ''' ドロップダウンリスト生成処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfGet_DropDownList_Sel() As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet        '表示情報のデータセット
        Dim maxVal As Integer = 0
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            '開始ログ出力
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                mfGet_DropDownList_Sel = False
            End If

            cmdDB = New SqlCommand(sCnsProgId & "_S3", conDB)
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            '申告元ドロップダウンリスト生成(明細)
            Me.ddlReport.DataSource = dstOrders.Tables(0)
            Me.ddlReport.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlReport.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlReport.DataBind()
            Me.ddlReport.Items.Insert(0, New ListItem(Nothing, Nothing))

            'ステータスドロップダウンリスト生成
            Me.ddlStatusCd.DataSource = dstOrders.Tables(1)
            Me.ddlStatusCd.DataTextField = dstOrders.Tables(1).Columns(0).ColumnName.ToString
            Me.ddlStatusCd.DataValueField = dstOrders.Tables(1).Columns(1).ColumnName.ToString
            Me.ddlStatusCd.DataBind()
            Me.ddlStatusCd.Items.Insert(0, New ListItem(Nothing, Nothing))

            '事象ドロップダウンリスト生成
            Me.ddlEvent.DataSource = dstOrders.Tables(2)
            Me.ddlEvent.DataTextField = dstOrders.Tables(2).Columns(0).ColumnName.ToString
            Me.ddlEvent.DataValueField = dstOrders.Tables(2).Columns(1).ColumnName.ToString
            Me.ddlEvent.DataBind()
            Me.ddlEvent.Items.Insert(0, New ListItem(Nothing, Nothing))



            '一時診断結果ドロップダウンリスト生成
            Me.ddlIchijiShindan1.DataSource = dstOrders.Tables(3)
            Me.ddlIchijiShindan1.DataTextField = dstOrders.Tables(3).Columns(0).ColumnName.ToString
            Me.ddlIchijiShindan1.DataValueField = dstOrders.Tables(3).Columns(1).ColumnName.ToString
            Me.ddlIchijiShindan1.DataBind()
            Me.ddlIchijiShindan1.Items.Insert(0, New ListItem(Nothing, Nothing))

            Me.ddlIchijiShindan2.DataSource = dstOrders.Tables(3)
            Me.ddlIchijiShindan2.DataTextField = dstOrders.Tables(3).Columns(0).ColumnName.ToString
            Me.ddlIchijiShindan2.DataValueField = dstOrders.Tables(3).Columns(1).ColumnName.ToString
            Me.ddlIchijiShindan2.DataBind()
            Me.ddlIchijiShindan2.Items.Insert(0, New ListItem(Nothing, Nothing))

            Me.ddlIchijiShindan3.DataSource = dstOrders.Tables(3)
            Me.ddlIchijiShindan3.DataTextField = dstOrders.Tables(3).Columns(0).ColumnName.ToString
            Me.ddlIchijiShindan3.DataValueField = dstOrders.Tables(3).Columns(1).ColumnName.ToString
            Me.ddlIchijiShindan3.DataBind()
            Me.ddlIchijiShindan3.Items.Insert(0, New ListItem(Nothing, Nothing))

            Me.ddlIchijiShindan4.DataSource = dstOrders.Tables(3)
            Me.ddlIchijiShindan4.DataTextField = dstOrders.Tables(3).Columns(0).ColumnName.ToString
            Me.ddlIchijiShindan4.DataValueField = dstOrders.Tables(3).Columns(1).ColumnName.ToString
            Me.ddlIchijiShindan4.DataBind()
            Me.ddlIchijiShindan4.Items.Insert(0, New ListItem(Nothing, Nothing))

            Me.ddlIchijiShindan5.DataSource = dstOrders.Tables(3)
            Me.ddlIchijiShindan5.DataTextField = dstOrders.Tables(3).Columns(0).ColumnName.ToString
            Me.ddlIchijiShindan5.DataValueField = dstOrders.Tables(3).Columns(1).ColumnName.ToString
            Me.ddlIchijiShindan5.DataBind()
            Me.ddlIchijiShindan5.Items.Insert(0, New ListItem(Nothing, Nothing))

            '製品名
            Me.ddlKosyoBui1.DataSource = dstOrders.Tables(4)
            Me.ddlKosyoBui1.DataTextField = dstOrders.Tables(4).Columns(0).ColumnName.ToString
            Me.ddlKosyoBui1.DataValueField = dstOrders.Tables(4).Columns(1).ColumnName.ToString
            Me.ddlKosyoBui1.DataBind()
            Me.ddlKosyoBui1.Items.Insert(0, New ListItem(Nothing, Nothing))

            Me.ddlKosyoBui2.DataSource = dstOrders.Tables(4)
            Me.ddlKosyoBui2.DataTextField = dstOrders.Tables(4).Columns(0).ColumnName.ToString
            Me.ddlKosyoBui2.DataValueField = dstOrders.Tables(4).Columns(1).ColumnName.ToString
            Me.ddlKosyoBui2.DataBind()
            Me.ddlKosyoBui2.Items.Insert(0, New ListItem(Nothing, Nothing))

            Me.ddlKosyoBui3.DataSource = dstOrders.Tables(4)
            Me.ddlKosyoBui3.DataTextField = dstOrders.Tables(4).Columns(0).ColumnName.ToString
            Me.ddlKosyoBui3.DataValueField = dstOrders.Tables(4).Columns(1).ColumnName.ToString
            Me.ddlKosyoBui3.DataBind()
            Me.ddlKosyoBui3.Items.Insert(0, New ListItem(Nothing, Nothing))

            Me.ddlKosyoBui4.DataSource = dstOrders.Tables(4)
            Me.ddlKosyoBui4.DataTextField = dstOrders.Tables(4).Columns(0).ColumnName.ToString
            Me.ddlKosyoBui4.DataValueField = dstOrders.Tables(4).Columns(1).ColumnName.ToString
            Me.ddlKosyoBui4.DataBind()
            Me.ddlKosyoBui4.Items.Insert(0, New ListItem(Nothing, Nothing))

            Me.ddlKosyoBui5.DataSource = dstOrders.Tables(4)
            Me.ddlKosyoBui5.DataTextField = dstOrders.Tables(4).Columns(0).ColumnName.ToString
            Me.ddlKosyoBui5.DataValueField = dstOrders.Tables(4).Columns(1).ColumnName.ToString
            Me.ddlKosyoBui5.DataBind()
            Me.ddlKosyoBui5.Items.Insert(0, New ListItem(Nothing, Nothing))

            '正常終了
            mfGet_DropDownList_Sel = True

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
            mfGet_DropDownList_Sel = False
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                mfGet_DropDownList_Sel = False
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function
#End Region

#Region "明細追加（更新）時チェック"
    ''' <summary>
    ''' 明細追加（更新）時チェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheckUpdateDetail()

        Dim objDs As DataSet = Nothing  'データセット
        Dim objDsDtil As DataSet = Nothing

        '保守対応データ存在チェック
        If txtKanriNo.ppEnabled Then 'データ追加時のみチェックする
            '保守対応情報取得処理
            objDs = mfGetMntInfo(objDsDtil)
            If Not objDs Is Nothing Then
                If objDs.Tables(0).Rows.Count = 0 Then
                    txtKanriNo.psSet_ErrorNo("2002", "入力した管理番号")
                    txtKanriNo.ppTextBox.Focus()
                Else
                    Dim dsWrk As New DataSet
                    dsWrk = mfGetMeetingMstInfo(objDs.Tables(0).Rows(0).Item("管理番号").ToString)
                    If dsWrk.Tables(0).Rows.Count = 0 Then
                        txtKanriNo.psSet_ErrorNo("2013", "入力した管理番号のシステムコード", "品質会議マスタ")
                        txtKanriNo.ppTextBox.Focus()
                        Exit Sub
                    End If

                    If objDs.Tables(0).Rows(0).Item("開始日時").ToString = String.Empty Then
                        txtKanriNo.psSet_ErrorNo("2018")
                        txtKanriNo.ppTextBox.Focus()
                        Exit Sub
                    End If

                    If objDs.Tables(0).Rows(0).Item("D86_管理番号").ToString <> String.Empty Then
                        txtKanriNo.psSet_ErrorNo("2006", "入力した管理番号")
                        txtKanriNo.ppTextBox.Focus()
                    Else
                        'グリッド内にデータが存在するか確認
                        Dim dr As DataRow() = pfParse_DataTable(Me.grvList).Select(
                            "管理番号 = '" & objDs.Tables(0).Rows(0).Item("管理番号").ToString & "' AND" &
                            "    枝番 = '" & objDs.Tables(0).Rows(0).Item("枝番").ToString & "'")
                        If dr.Count > 0 Then
                            txtKanriNo.psSet_ErrorNo("2007", "入力した管理番号")
                            txtKanriNo.ppTextBox.Focus()
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' 故障部位エラーチェック
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvKosyoBui_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvKosyoBui1.ServerValidate _
                                                                                                      , cuvKosyoBui2.ServerValidate _
                                                                                                      , cuvKosyoBui3.ServerValidate _
                                                                                                      , cuvKosyoBui4.ServerValidate _
                                                                                                      , cuvKosyoBui5.ServerValidate _
                                                                                                      , cuvKosyoBui11.ServerValidate _
                                                                                                      , cuvKosyoBui22.ServerValidate _
                                                                                                      , cuvKosyoBui33.ServerValidate _
                                                                                                      , cuvKosyoBui44.ServerValidate _
                                                                                                      , cuvKosyoBui55.ServerValidate


        Dim strErrNo As String = String.Empty
        Dim dtrMes As DataRow

        Dim strErrBui As String = String.Empty

        Dim objddlIchijiShindan As New DropDownList
        Dim objtxtChousaJyokyou As New TextBox
        Dim objddlKosyou As DropDownList = Nothing    '故障部位用

        Select Case DirectCast(source, CustomValidator).ID
            Case cuvKosyoBui1.ID, cuvKosyoBui11.ID
                objddlIchijiShindan = ddlIchijiShindan1
                objtxtChousaJyokyou = txtChousaJyokyou1
                objddlKosyou = ddlKosyoBui1
                strErrBui = "製品名１"

            Case cuvKosyoBui2.ID, cuvKosyoBui22.ID
                objddlIchijiShindan = ddlIchijiShindan2
                objtxtChousaJyokyou = txtChousaJyokyou2
                objddlKosyou = ddlKosyoBui2
                strErrBui = "製品名２"

            Case cuvKosyoBui3.ID, cuvKosyoBui33.ID
                objddlIchijiShindan = ddlIchijiShindan3
                objtxtChousaJyokyou = txtChousaJyokyou3
                objddlKosyou = ddlKosyoBui3
                strErrBui = "製品名３"

            Case cuvKosyoBui4.ID, cuvKosyoBui44.ID
                objddlIchijiShindan = ddlIchijiShindan4
                objtxtChousaJyokyou = txtChousaJyokyou4
                objddlKosyou = ddlKosyoBui4
                strErrBui = "製品名４"

            Case cuvKosyoBui5.ID, cuvKosyoBui55.ID
                objddlIchijiShindan = ddlIchijiShindan5
                objtxtChousaJyokyou = txtChousaJyokyou5
                objddlKosyou = ddlKosyoBui5
                strErrBui = "製品名５"

        End Select

        'エラーチェック
        If objddlIchijiShindan.SelectedIndex <> 0 Or objtxtChousaJyokyou.Text <> String.Empty Then
            strErrNo = pfCheck_ListErr(args.Value, True)
        Else
            If objddlKosyou.Enabled = True AndAlso objddlKosyou.SelectedIndex = 0 Then
                strErrNo = pfCheck_ListErr(args.Value, True)
            End If
        End If

        If strErrNo = "5003" Then
            Select Case DirectCast(source, CustomValidator).ID
                Case cuvKosyoBui1.ID, cuvKosyoBui2.ID, cuvKosyoBui3.ID, cuvKosyoBui4.ID, cuvKosyoBui5.ID
                    strErrNo = "5500"
                Case Else
                    strErrNo = "5501"
            End Select
        End If

        If strErrNo <> String.Empty Then
            'エラー
            dtrMes = pfGet_ValMes(strErrNo, strErrBui)
            DirectCast(source, CustomValidator).Text = dtrMes.Item(P_VALMES_SMES)
            DirectCast(source, CustomValidator).ErrorMessage = dtrMes.Item(P_VALMES_MES)
            args.IsValid = False
            Exit Sub
        End If
    End Sub
#End Region

#Region "グリッドデータ追加処理"
    ''' <summary>
    ''' グリッドへデータ追加
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msAddGridData()

        Dim dtData As DataTable = Nothing
        Dim drNewData As DataRow = Nothing
        Dim dsDtil As New DataSet
        Dim dsWrk As New DataSet
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'グリッドから取得
            dtData = pfParse_DataTable(Me.grvList)
            '新規行設定
            drNewData = dtData.NewRow()

            '明細取得
            dsWrk = ViewState("QUAUPDP001_dsDtil")
            If dsWrk Is Nothing Then
                mfGetData(dsWrk)
                If Me.grvList.Rows.Count = 0 Then
                    For Each dr As DataRow In dsWrk.Tables(0).Rows
                        dr.Delete()
                    Next
                    dsWrk.Tables(0).AcceptChanges()
                End If
            End If

            'グリッド行データ設定
            msSetGridRowData(drNewData, dsDtil)
            For zz As Integer = 0 To dsDtil.Tables(0).Rows.Count - 1
                Dim dr As DataRow = dsWrk.Tables(0).NewRow
                dr("管理番号") = dsDtil.Tables(0).Rows(zz).Item("管理番号").ToString
                dr("枝番") = dsDtil.Tables(0).Rows(zz).Item("枝番").ToString
                dr("故障部位コード") = dsDtil.Tables(0).Rows(zz).Item("故障部位コード").ToString
                dr("故障部位") = dsDtil.Tables(0).Rows(zz).Item("故障部位").ToString
                dr("一時診断コード") = dsDtil.Tables(0).Rows(zz).Item("一時診断コード").ToString
                dr("調査及び調査状況") = dsDtil.Tables(0).Rows(zz).Item("調査及び調査状況").ToString
                dr("一時診断結果") = dsDtil.Tables(0).Rows(zz).Item("一時診断結果").ToString
                dr("データ取得先") = dsDtil.Tables(0).Rows(zz).Item("データ取得先").ToString
                If dsDtil.Tables(0).Rows(zz).Item("管理番号枝番").ToString = String.Empty Then
                    dr("管理番号枝番") = DBNull.Value
                Else
                    dr("管理番号枝番") = dsDtil.Tables(0).Rows(zz).Item("管理番号枝番").ToString
                End If
                dr("表示順") = dsDtil.Tables(0).Rows(zz).Item("表示順").ToString
                dsWrk.Tables(0).Rows.Add(dr)
            Next

            ViewState("QUAUPDP001_dsDtil") = dsWrk

            '行追加
            dtData.Rows.Add(drNewData)

            'データ再設定
            Me.grvList.DataSource = dtData
            Me.grvList.DataBind()

        Catch ex As Exception
            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsProgNm)
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

#Region "グリッドデータ更新処理"
    ''' <summary>
    ''' グリッドデータ更新
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msUpdGridData()

        Dim dtData As DataTable = Nothing
        Dim dsDtil As New DataSet
        Dim dsWrk As New DataSet
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'グリッドから取得
            dtData = pfParse_DataTable(Me.grvList)

            '全明細取得
            dsWrk = ViewState("QUAUPDP001_dsDtil")
            If dsWrk Is Nothing Then
                mfGetData(dsWrk)
            End If

            Dim dr As DataRow = dtData.Select("項番 = '" & lblNo.Text & "'" & "AND 管理番号 = '" & txtKanriNo.ppText & "'")(0)
            msSetGridRowData(dr, dsDtil)

            '行削除
            For Each dr2 As DataRow
                In dsWrk.Tables(0).Select("管理番号 = '" & txtKanriNo.ppText & "'")
                dr2.Delete()
            Next
            dsWrk.Tables(0).AcceptChanges()

            '行を追加する
            For zz As Integer = 0 To dsDtil.Tables(0).Rows.Count - 1
                Dim dr3 As DataRow = dsWrk.Tables(0).NewRow
                dr3("管理番号") = dsDtil.Tables(0).Rows(zz).Item("管理番号").ToString
                dr3("枝番") = dsDtil.Tables(0).Rows(zz).Item("枝番").ToString
                dr3("故障部位コード") = dsDtil.Tables(0).Rows(zz).Item("故障部位コード").ToString
                dr3("故障部位") = dsDtil.Tables(0).Rows(zz).Item("故障部位").ToString
                dr3("一時診断コード") = dsDtil.Tables(0).Rows(zz).Item("一時診断コード").ToString
                dr3("調査及び調査状況") = dsDtil.Tables(0).Rows(zz).Item("調査及び調査状況").ToString
                dr3("一時診断結果") = dsDtil.Tables(0).Rows(zz).Item("一時診断結果").ToString
                dr3("データ取得先") = dsDtil.Tables(0).Rows(zz).Item("データ取得先").ToString
                If dsDtil.Tables(0).Rows(zz).Item("管理番号枝番").ToString = String.Empty Then
                    dr3("管理番号枝番") = DBNull.Value
                Else
                    dr3("管理番号枝番") = dsDtil.Tables(0).Rows(zz).Item("管理番号枝番").ToString
                End If
                dr3("表示順") = dsDtil.Tables(0).Rows(zz).Item("表示順").ToString
                dsWrk.Tables(0).Rows.Add(dr3)
            Next

            ViewState("QUAUPDP001_dsDtil") = dsWrk

            '行更新
            dtData.AcceptChanges()

            'データ再設定
            Me.grvList.DataSource = dtData
            Me.grvList.DataBind()

        Catch ex As Exception
            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsProgNm)
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

#Region "グリッドデータ削除処理"
    ''' <summary>
    ''' グリッドデータ削除
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msDelGridData()

        Dim dtData As DataTable = Nothing
        Dim drNew As DataRow = Nothing
        Dim dsWrk As New DataSet
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Dim dsDtil As New DataSet
        Try
            'グリッドから取得
            dtData = pfParse_DataTable(Me.grvList)

            '全明細取得
            dsWrk = ViewState("QUAUPDP001_dsDtil")
            If dsWrk Is Nothing Then
                mfGetData(dsWrk)
            End If

            '行削除
            For Each dr As DataRow
                In dtData.Select("項番     = '" & lblNo.Text & "'" &
                             "AND 管理番号 = '" & txtKanriNo.ppText & "'")
                dr.Delete()
            Next
            dtData.AcceptChanges()

            '明細行削除
            For Each dr As DataRow
                In dsWrk.Tables(0).Select("管理番号 = '" & txtKanriNo.ppText & "'")
                dr.Delete()
            Next
            dsWrk.Tables(0).AcceptChanges()

            ViewState("QUAUPDP001_dsDtil") = dsWrk

            ''明細取得
            'mfGetData(dsDtil)

            'ViewState("QUAUPDP001_dsDtil") = dsDtil

            'データ再設定
            Me.grvList.DataSource = dtData
            Me.grvList.DataBind()

        Catch ex As Exception
            psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsProgNm)
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

#Region "グリッド行データ設定"
    ''' <summary>
    ''' グリッド行データ設定
    ''' </summary>
    ''' <param name="drData"></param>
    ''' <remarks></remarks>
    Private Sub msSetGridRowData(ByRef drData As DataRow, ByRef dsDtil As DataSet)

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objTrn As SqlTransaction = Nothing  'SqlTransactionクラス
        If Not clsDataConnect.pfOpen_Database(objCn) Then
        Else
            drData.Item("データ種別") = "1"
            drData.Item("管理番号") = txtKanriNo.ppText
            drData.Item("項番") = lblNo.Text
            drData.Item("種別コード") = lblClassCd.Text
            drData.Item("種別名称") = lblClassNm.Text
            drData.Item("ＮＬ") = lblNLClsCd.Text
            drData.Item("ＮＬ区分") = lblNLClsNm.Text
            drData.Item("ＴＢＯＸＩＤ") = lblTboxId.Text
            drData.Item("ＶＥＲ") = lblVer.Text
            drData.Item("ホールコード") = lblHallCd.Text
            drData.Item("ホール名") = lblHallNm.Text
            drData.Item("ホール住所") = lblHallAdr.Text

            'drData.Item("受付日時") = lblUketukeDt.Text
            If dtbUketukeDt.ppText = String.Empty AndAlso
               dtbUketukeDt.ppHourText = String.Empty AndAlso
               dtbUketukeDt.ppMinText = String.Empty Then
                drData.Item("受付日時") = String.Empty
            Else
                drData.Item("受付日時") = dtbUketukeDt.ppText + " " + dtbUketukeDt.ppHourText + ":" + dtbUketukeDt.ppMinText
            End If

            drData.Item("マスタ開始日") = lblStartDt.Text

            If dtbKaifukuDt.ppText = String.Empty AndAlso
               dtbKaifukuDt.ppHourText = String.Empty AndAlso
               dtbKaifukuDt.ppMinText = String.Empty Then
                drData.Item("回復日時") = String.Empty
                drData.Item("時") = String.Empty
                drData.Item("分") = String.Empty
            Else
                drData.Item("回復日時") = dtbKaifukuDt.ppText + " " + dtbKaifukuDt.ppHourText + ":" + dtbKaifukuDt.ppMinText
                drData.Item("時") = dtbKaifukuDt.ppHourText
                drData.Item("分") = dtbKaifukuDt.ppMinText
            End If
            drData.Item("サンド入金停止時間") = txtStopTime.ppText
            drData.Item("申告元コード") = ddlReport.SelectedValue
            If ddlReport.SelectedValue = String.Empty Then
                drData.Item("申告元") = String.Empty
            Else
                objCmd = New SqlCommand("ZCMPSEL053", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    .Add(pfSet_Param("code", SqlDbType.NVarChar, ddlReport.SelectedValue))
                End With
                Dim ds053 As New DataSet
                ds053 = clsDataConnect.pfGet_DataSet(objCmd)
                drData.Item("申告元") = ds053.Tables(0).Rows(0).Item("印刷名称").ToString
            End If
            'drData.Item("申告元") = mfGetNamePart(ddlReport.SelectedItem.Text)

            drData.Item("申告内容") = txtReportCon.ppText
            'QUAUPDP001-001
            If ddlEvent.SelectedIndex = 0 Then
                drData.Item("事象コード") = ddlEvent.SelectedValue
            Else
                drData.Item("事象コード") = ddlEvent.SelectedItem.Text.Remove(2)
            End If
            'QUAUPDP001-001 END
            drData.Item("事象") = mfGetNamePart(ddlEvent.SelectedItem.Text)
            'drData.Item("一時診断結果") = txtIchijiShindan.ppText
            drData.Item("故障部位") = txtBuiKoshou.ppText
            drData.Item("調査完了日") = dtbChousaEnd.ppText
            'drData.Item("調査及び調査状況") = txtChousaJyokyou.ppText
            drData.Item("ステータスコード") = ddlStatusCd.SelectedValue
            drData.Item("ステータス") = mfGetNamePart(ddlStatusCd.SelectedItem.Text)
            drData.Item("提出") = ddlRstSubmsnCls.ppSelectedValue
            drData.Item("提出区分") = ddlRstSubmsnCls.ppSelectedTextOnly

            drData.Item("対応内容") = txtRepect.ppText & Environment.NewLine & _
                                      txtRepect2.ppText & Environment.NewLine & _
                                      txtRepect3.ppText & Environment.NewLine & _
                                      txtRepect4.ppText & Environment.NewLine & _
                                      txtRepect5.ppText & Environment.NewLine & _
                                      txtRepect6.ppText
            'drData.Item("対応内容") = txtRepect.ppText

            drData.Item("開始日時") = lblDealDt.Text


            objCmd = New SqlCommand(sCnsProgId + "_S5", objCn)
            With objCmd.Parameters
                '--パラメータ設定
                .Add(pfSet_Param("meeting_no", SqlDbType.NVarChar, txtKanriNo.ppText))
            End With
            dsDtil = clsDataConnect.pfGet_DataSet(objCmd)
            drData.Item("調査及び調査状況") = String.Empty
            drData.Item("一時診断結果") = String.Empty
            If dsDtil.Tables(0).Rows.Count <> 0 Then
                For zz As Integer = 0 To dsDtil.Tables(0).Rows.Count - 1

                    If drData.Item("調査及び調査状況").ToString <> String.Empty Then
                        drData.Item("調査及び調査状況") = drData.Item("調査及び調査状況").ToString & Environment.NewLine
                    End If
                    drData.Item("調査及び調査状況") = drData.Item("調査及び調査状況").ToString _
                                                    & "・" _
                                                    & dsDtil.Tables(0).Rows(zz).Item("故障部位").ToString & "・・・" _
                                                    & dsDtil.Tables(0).Rows(zz).Item("調査及び調査状況").ToString
                    If drData.Item("一時診断結果").ToString <> String.Empty Then
                        drData.Item("一時診断結果") = drData.Item("一時診断結果").ToString & Environment.NewLine
                    End If
                    drData.Item("一時診断結果") = drData.Item("一時診断結果").ToString _
                                                & dsDtil.Tables(0).Rows(zz).Item("一時診断結果").ToString

                Next
            End If
        End If

    End Sub
#End Region

#Region "ドロップダウンリスト名称取得処理"
    ''' <summary>
    ''' ドロップダウンリストの名称部分を取得　※コロン（：）で区切られた文字列の２番目を返す
    ''' </summary>
    ''' <param name="strText"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetNamePart(ByVal strText As String) As String

        Dim strArray As String() = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            If strText = String.Empty Then
                Return strText
            End If

            strArray = strText.Split(":")

            Return strArray(1)

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
            Return String.Empty
        End Try
    End Function
#End Region

#Region "保守対応情報取得処理"
    ''' <summary>
    ''' 保守対応情報取得処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetMntInfo(ByRef objDsDtil As DataSet) As DataSet

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGetMntInfo = Nothing

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(sCnsProgId + "_S2", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '管理番号
                    .Add(pfSet_Param("meeting_no", SqlDbType.NVarChar, txtKanriNo.ppText))
                    '枝番
                    .Add(pfSet_Param("branch", SqlDbType.NVarChar, ""))

                End With

                'データ取得
                mfGetMntInfo = clsDataConnect.pfGet_DataSet(objCmd)


                'objCmd = New SqlCommand(sCnsProgId + "_S5", objCn)
                objCmd = New SqlCommand(sCnsProgId + "_S6", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '管理番号
                    .Add(pfSet_Param("meeting_no", SqlDbType.NVarChar, txtKanriNo.ppText))

                End With

                'データ取得
                objDsDtil = clsDataConnect.pfGet_DataSet(objCmd)


            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守対応情報取得")
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

#Region "グリッド内容の取得処理"
    ''' <summary>
    ''' グリッド内容の取得処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetGridData() As DataSet

        Dim dt As DataTable
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGetGridData = Nothing

        Try
            dt = pfParse_DataTable(Me.grvList)
            mfGetGridData = New DataSet
            mfGetGridData.Tables.Add(dt)

        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "グリッド情報の取得")
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

    End Function
#End Region

#Region "検索時項目チェック"
    ''' <summary>
    ''' 検索時項目チェック
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfCheckSearchCondition() As Boolean

        '報告日From必須入力チェック
        If Me.dtbReportDt.ppFromText = String.Empty Then
            Me.dtbReportDt.psSet_ErrorNo("5001", "報告日FROM")
            Me.dtbReportDt.ppDateBoxFrom.Focus()
            Return False
        End If

        Return True

    End Function
#End Region

#End Region

    ''' <summary>
    ''' 品質会議資料マスタ取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetMeetingMstInfo(ByVal strMntNo As String) As DataSet

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGetMeetingMstInfo = Nothing

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(sCnsProgId + "_S10", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '管理番号
                    .Add(pfSet_Param("mnt_no", SqlDbType.NVarChar, strMntNo))

                End With

                'データ取得
                mfGetMeetingMstInfo = clsDataConnect.pfGet_DataSet(objCmd)

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守対応情報取得")
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

    ''' <summary>
    ''' 明細部分のコントロール制御処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub mfCtrlDetail(Optional ByVal intFlg As Integer = 0)

        'Enabled制御
        Me.rdbEdaban1.Enabled = False
        Me.ddlKosyoBui1.Enabled = False
        Me.ddlIchijiShindan1.Enabled = False
        Me.txtChousaJyokyou1.Enabled = False
        Me.rdbEdaban2.Enabled = False
        Me.ddlKosyoBui2.Enabled = False
        Me.ddlIchijiShindan2.Enabled = False
        Me.txtChousaJyokyou2.Enabled = False
        Me.rdbEdaban3.Enabled = False
        Me.ddlKosyoBui3.Enabled = False
        Me.ddlIchijiShindan3.Enabled = False
        Me.txtChousaJyokyou3.Enabled = False
        Me.rdbEdaban4.Enabled = False
        Me.ddlKosyoBui4.Enabled = False
        Me.ddlIchijiShindan4.Enabled = False
        Me.txtChousaJyokyou4.Enabled = False
        Me.rdbEdaban5.Enabled = False
        Me.ddlKosyoBui5.Enabled = False
        Me.ddlIchijiShindan5.Enabled = False
        Me.txtChousaJyokyou5.Enabled = False

        Select Case intFlg
            Case 0 '通常

                '何行目まで開いてるか判断する変数
                Dim intOpenDtil As Integer = 0

                If Me.ddlKosyoBui1.SelectedIndex <> 0 Then
                    Me.rdbEdaban1.Enabled = True
                    Me.ddlKosyoBui1.Enabled = True
                    Me.ddlIchijiShindan1.Enabled = True
                    Me.txtChousaJyokyou1.Enabled = True
                    intOpenDtil = 1
                End If
                If Me.ddlKosyoBui2.SelectedIndex <> 0 Then
                    Me.rdbEdaban2.Enabled = True
                    Me.ddlKosyoBui2.Enabled = True
                    Me.ddlIchijiShindan2.Enabled = True
                    Me.txtChousaJyokyou2.Enabled = True
                    intOpenDtil = 2
                End If
                If Me.ddlKosyoBui3.SelectedIndex <> 0 Then
                    Me.rdbEdaban3.Enabled = True
                    Me.ddlKosyoBui3.Enabled = True
                    Me.ddlIchijiShindan3.Enabled = True
                    Me.txtChousaJyokyou3.Enabled = True
                    intOpenDtil = 3
                End If
                If Me.ddlKosyoBui4.SelectedIndex <> 0 Then
                    Me.rdbEdaban4.Enabled = True
                    Me.ddlKosyoBui4.Enabled = True
                    Me.ddlIchijiShindan4.Enabled = True
                    Me.txtChousaJyokyou4.Enabled = True
                    intOpenDtil = 4
                End If
                If Me.ddlKosyoBui5.SelectedIndex <> 0 Then
                    Me.rdbEdaban5.Enabled = True
                    Me.ddlKosyoBui5.Enabled = True
                    Me.ddlIchijiShindan5.Enabled = True
                    Me.txtChousaJyokyou5.Enabled = True
                    intOpenDtil = 5
                End If

                If Me.lblMode.Text = "1" Then
                    '新規モードだった場合は、最後に開いてる行の次を活性化
                    Select Case intOpenDtil
                        Case 0
                            Me.rdbEdaban1.Enabled = True
                            Me.ddlKosyoBui1.Enabled = True
                            Me.ddlIchijiShindan1.Enabled = True
                            Me.txtChousaJyokyou1.Enabled = True
                        Case 1
                            Me.rdbEdaban2.Enabled = True
                            Me.ddlKosyoBui2.Enabled = True
                            Me.ddlIchijiShindan2.Enabled = True
                            Me.txtChousaJyokyou2.Enabled = True
                        Case 2
                            Me.rdbEdaban3.Enabled = True
                            Me.ddlKosyoBui3.Enabled = True
                            Me.ddlIchijiShindan3.Enabled = True
                            Me.txtChousaJyokyou3.Enabled = True
                        Case 3
                            Me.rdbEdaban4.Enabled = True
                            Me.ddlKosyoBui4.Enabled = True
                            Me.ddlIchijiShindan4.Enabled = True
                            Me.txtChousaJyokyou4.Enabled = True
                        Case 4
                            Me.rdbEdaban5.Enabled = True
                            Me.ddlKosyoBui5.Enabled = True
                            Me.ddlIchijiShindan5.Enabled = True
                            Me.txtChousaJyokyou5.Enabled = True
                    End Select
                End If

            Case Else '削除時
                Dim objrdbEdaban As RadioButton
                Dim objddlKosyou As DropDownList
                Dim objddlIchiji As DropDownList
                Dim objtxtChousa As TextBox

                For zz As Integer = 1 To intFlg
                    objrdbEdaban = Nothing
                    objddlKosyou = Nothing
                    objddlIchiji = Nothing
                    objtxtChousa = Nothing

                    Select Case zz
                        Case 1
                            objrdbEdaban = Me.rdbEdaban1
                            objddlKosyou = Me.ddlKosyoBui1
                            objddlIchiji = Me.ddlIchijiShindan1
                            objtxtChousa = Me.txtChousaJyokyou1
                        Case 2
                            objrdbEdaban = Me.rdbEdaban2
                            objddlKosyou = Me.ddlKosyoBui2
                            objddlIchiji = Me.ddlIchijiShindan2
                            objtxtChousa = Me.txtChousaJyokyou2
                        Case 3
                            objrdbEdaban = Me.rdbEdaban3
                            objddlKosyou = Me.ddlKosyoBui3
                            objddlIchiji = Me.ddlIchijiShindan3
                            objtxtChousa = Me.txtChousaJyokyou3
                        Case 4
                            objrdbEdaban = Me.rdbEdaban4
                            objddlKosyou = Me.ddlKosyoBui4
                            objddlIchiji = Me.ddlIchijiShindan4
                            objtxtChousa = Me.txtChousaJyokyou4
                        Case 5
                            objrdbEdaban = Me.rdbEdaban5
                            objddlKosyou = Me.ddlKosyoBui5
                            objddlIchiji = Me.ddlIchijiShindan5
                            objtxtChousa = Me.txtChousaJyokyou5
                    End Select
                    objrdbEdaban.Enabled = True
                    objddlKosyou.Enabled = True
                    objddlIchiji.Enabled = True
                    objtxtChousa.Enabled = True
                Next
        End Select
    End Sub

    Private Sub mfRdoDetail()

    End Sub

    Private Sub mfBtnDetail()

        Try
            '-----------------------------------
            '- 修理データ取得ボタン
            '-----------------------------------
            '修理データに存在するかを判定して制御
            Dim blnIsExistRp As Boolean = False
            Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
            Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
            If Not clsDataConnect.pfOpen_Database(objCn) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Exit Sub
            Else
                objCmd = New SqlCommand(sCnsProgId + "_S9", objCn)
                With objCmd.Parameters '--パラメータ設定
                    .Add(pfSet_Param("meeting_no", SqlDbType.NVarChar, txtKanriNo.ppText))  '管理番号
                End With

                'データ取得
                Dim ds As DataSet = Nothing
                ds = clsDataConnect.pfGet_DataSet(objCmd)
                If ds.Tables(0).Rows.Count <> 0 Then
                    Me.btnGetRep.Enabled = True
                Else
                    Me.btnGetRep.Enabled = False
                End If
            End If

            '-----------------------------------
            '- 行追加ボタン(５行目まで活性化していたら非活性)
            '-----------------------------------
            If Me.rdbEdaban5.Enabled = True Then
                Me.btnRowAdd.Enabled = False
            Else
                Me.btnRowAdd.Enabled = True
            End If

            '-----------------------------------
            '- 行削除ボタン(明細が１つでも活性化されていれば活性)
            '-----------------------------------
            If Me.rdbEdaban1.Enabled = True Then
                Me.btnRowDel.Enabled = True
            Else
                Me.btnRowDel.Enabled = False
            End If

            '-----------------------------------
            '- ▲ボタン(チェックが１行目または明細の項目が全部非活性だったら非活性)
            '-----------------------------------
            If rdbEdaban1.Enabled = True Then
                If rdbEdaban1.Checked = True Then
                    Me.btnRowUp.Enabled = False
                Else
                    Me.btnRowUp.Enabled = True
                End If
            Else
                Me.btnRowUp.Enabled = False
            End If

            '-----------------------------------
            '- ▼ボタン(チェックが５行目または最後に活性化されている行だったら非活性)
            '-----------------------------------
            Dim rdbEdaban As RadioButton
            Dim rdbEdaban_last As RadioButton = Nothing
            For zz As Integer = 1 To 5
                rdbEdaban = Nothing
                Select Case zz
                    Case 1
                        rdbEdaban = Me.rdbEdaban1
                    Case 2
                        rdbEdaban = Me.rdbEdaban2
                    Case 3
                        rdbEdaban = Me.rdbEdaban3
                    Case 4
                        rdbEdaban = Me.rdbEdaban4
                    Case 5
                        rdbEdaban = Me.rdbEdaban5
                End Select

                'チェックついてなかったら次
                If rdbEdaban.Checked = False Then
                    Continue For
                End If

                For yy As Integer = 1 To 5
                    rdbEdaban_last = Nothing
                    Select Case True
                        Case Me.rdbEdaban5.Enabled
                            rdbEdaban_last = Me.rdbEdaban5
                            Exit For
                        Case Me.rdbEdaban4.Enabled
                            rdbEdaban_last = Me.rdbEdaban4
                            Exit For
                        Case Me.rdbEdaban3.Enabled
                            rdbEdaban_last = Me.rdbEdaban3
                            Exit For
                        Case Me.rdbEdaban2.Enabled
                            rdbEdaban_last = Me.rdbEdaban2
                            Exit For
                        Case Me.rdbEdaban1.Enabled
                            rdbEdaban_last = Me.rdbEdaban1
                            Exit For
                    End Select
                Next
                If rdbEdaban.ID = Me.rdbEdaban5.ID Then
                    Me.btnRowDn.Enabled = False
                Else
                    If rdbEdaban.ID = rdbEdaban_last.ID Then
                        Me.btnRowDn.Enabled = False
                    Else
                        Me.btnRowDn.Enabled = True
                    End If
                End If
                Exit For
            Next

        Catch ex As Exception
            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsProgNm)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
    End Sub

    ''' <summary>
    ''' ラジオボタン変更
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub rdbEdaban_CheckedChanged(sender As Object, e As EventArgs) Handles rdbEdaban1.CheckedChanged _
                                                                                   , rdbEdaban2.CheckedChanged _
                                                                                   , rdbEdaban3.CheckedChanged _
                                                                                   , rdbEdaban4.CheckedChanged _
                                                                                   , rdbEdaban5.CheckedChanged

        'ボタン制御
        mfBtnDetail()
        'Dim rdbEdaban As RadioButton = Nothing
        'rdbEdaban = DirectCast(sender, RadioButton)
        'If rdbEdaban.Checked = True Then
        '    '修理データ取得ボタン
        '    If Me.lblExistsRp.Text = "0" Then
        '        Me.btnGetRep.Enabled = False
        '    Else
        '        Me.btnGetRep.Enabled = True
        '    End If

        '    '行削除ボタン
        '    Me.btnRowDel.Enabled = True

        '    '▲ボタン(１行目だったら非活性)
        '    If rdbEdaban.ID = Me.rdbEdaban1.ID Then
        '        Me.btnRowUp.Enabled = False
        '    Else
        '        Me.btnRowUp.Enabled = True
        '    End If

        '    '▼ボタン(５行目または最後に活性化されている行だったら非活性)
        '    Dim rdbEdaban_last As RadioButton = Nothing
        '    For zz As Integer = 1 To 5
        '        Select Case True
        '            Case Me.rdbEdaban5.Enabled
        '                rdbEdaban_last = Me.rdbEdaban5
        '                Exit For
        '            Case Me.rdbEdaban4.Enabled
        '                rdbEdaban_last = Me.rdbEdaban4
        '                Exit For
        '            Case Me.rdbEdaban3.Enabled
        '                rdbEdaban_last = Me.rdbEdaban3
        '                Exit For
        '            Case Me.rdbEdaban2.Enabled
        '                rdbEdaban_last = Me.rdbEdaban2
        '                Exit For
        '            Case Me.rdbEdaban1.Enabled
        '                rdbEdaban_last = Me.rdbEdaban1
        '                Exit For
        '        End Select
        '    Next
        '    If rdbEdaban.ID = Me.rdbEdaban5.ID Then
        '        Me.btnRowDn.Enabled = False
        '    Else
        '        If rdbEdaban.ID = rdbEdaban_last.ID Then
        '            Me.btnRowDn.Enabled = False
        '        Else
        '            Me.btnRowDn.Enabled = True
        '        End If
        '    End If
        'End If
    End Sub

    ''' <summary>
    ''' 修理データ取得ボタン押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGetRep_Click(sender As Object, e As EventArgs) Handles btnGetRep.Click

        Dim objStack As New StackFrame

        Try
            '修理情報存在チェック
            Dim blnIsExistRp As Boolean = False
            Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
            Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
            If Not clsDataConnect.pfOpen_Database(objCn) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else
                objCmd = New SqlCommand(sCnsProgId + "_S9", objCn)
                With objCmd.Parameters '--パラメータ設定
                    .Add(pfSet_Param("meeting_no", SqlDbType.NVarChar, txtKanriNo.ppText))  '管理番号
                End With

                'データ取得
                Dim ds As DataSet = Nothing
                ds = clsDataConnect.pfGet_DataSet(objCmd)

                If ds.Tables(0).Rows.Count <> 0 Then

                    '修理があります
                    '画面を開きましょう
                    Dim strKeyList As List(Of String) = Nothing '画面引継ぎ用キー情報

                    '開始ログ出力
                    psLogStart(Me)

                    Dim objhdnSeq As HiddenField = Nothing
                    Dim objhdnEdaban As HiddenField = Nothing
                    Dim objddlKosyou As DropDownList = Nothing
                    Dim objddlIchiji As DropDownList = Nothing
                    Dim objtxtChousa As TextBox = Nothing

                    Select Case True
                        Case Me.rdbEdaban1.Checked
                            objhdnSeq = Me.hdnSeq1
                            objhdnEdaban = Me.hdnEdaban1
                            objddlKosyou = Me.ddlKosyoBui1
                            objddlIchiji = Me.ddlIchijiShindan1
                            objtxtChousa = Me.txtChousaJyokyou1
                            '子画面を開いたときに必要な情報を保管
                            Me.hdnChildWindow.Value = "1"
                            Me.hdnKanriNo.Value = Me.txtKanriNo.ppText
                            Me.hdnCheckRow.Value = "1"
                        Case Me.rdbEdaban2.Checked
                            objhdnSeq = Me.hdnSeq2
                            objhdnEdaban = Me.hdnEdaban2
                            objddlKosyou = Me.ddlKosyoBui2
                            objddlIchiji = Me.ddlIchijiShindan2
                            objtxtChousa = Me.txtChousaJyokyou2
                            '子画面を開いたときに必要な情報を保管
                            Me.hdnChildWindow.Value = "1"
                            Me.hdnKanriNo.Value = Me.txtKanriNo.ppText
                            Me.hdnCheckRow.Value = "2"
                        Case Me.rdbEdaban3.Checked
                            objhdnSeq = Me.hdnSeq3
                            objhdnEdaban = Me.hdnEdaban3
                            objddlKosyou = Me.ddlKosyoBui3
                            objddlIchiji = Me.ddlIchijiShindan3
                            objtxtChousa = Me.txtChousaJyokyou3
                            '子画面を開いたときに必要な情報を保管
                            Me.hdnChildWindow.Value = "1"
                            Me.hdnKanriNo.Value = Me.txtKanriNo.ppText
                            Me.hdnCheckRow.Value = "3"
                        Case Me.rdbEdaban4.Checked
                            objhdnSeq = Me.hdnSeq4
                            objhdnEdaban = Me.hdnEdaban4
                            objddlKosyou = Me.ddlKosyoBui4
                            objddlIchiji = Me.ddlIchijiShindan4
                            objtxtChousa = Me.txtChousaJyokyou4
                            '子画面を開いたときに必要な情報を保管
                            Me.hdnChildWindow.Value = "1"
                            Me.hdnKanriNo.Value = Me.txtKanriNo.ppText
                            Me.hdnCheckRow.Value = "4"
                        Case Me.rdbEdaban5.Checked
                            objhdnSeq = Me.hdnSeq5
                            objhdnEdaban = Me.hdnEdaban5
                            objddlKosyou = Me.ddlKosyoBui5
                            objddlIchiji = Me.ddlIchijiShindan5
                            objtxtChousa = Me.txtChousaJyokyou5
                            '子画面を開いたときに必要な情報を保管
                            Me.hdnChildWindow.Value = "1"
                            Me.hdnKanriNo.Value = Me.txtKanriNo.ppText
                            Me.hdnCheckRow.Value = "5"
                    End Select

                    '画面引継ぎ用キー情報設定
                    strKeyList = New List(Of String)
                    strKeyList.Add(Me.txtKanriNo.ppText)    '保守管理番号
                    strKeyList.Add(objhdnSeq.ClientID)      '枝番ラベル
                    strKeyList.Add(objhdnEdaban.ClientID)   '管理番号枝番ラベル
                    strKeyList.Add(objddlKosyou.ClientID)   '故障部位ドロップダウン
                    strKeyList.Add(objddlIchiji.ClientID)   '一時診断結果ドロップダウン
                    strKeyList.Add(objtxtChousa.ClientID)   '調査状況ドロップダウン
                    strKeyList.Add(objhdnEdaban.Value)       '管理番号枝番

                    'セッション情報設定																	
                    Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
                    Session(P_SESSION_OLDDISP) = sCnsProgId
                    Session(P_KEY) = strKeyList.ToArray
                    Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)
                    Session(P_SESSION_TERMS) = ViewState(P_SESSION_TERMS)

                    '--------------------------------
                    '2014/04/16 星野　ここから
                    '--------------------------------
                    '■□■□結合試験時のみ使用予定□■□■

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
                                    objStack.GetMethod.Name, M_METDTIL_DISP_PATH, strPrm, "TRANS")
                    '■□■□結合試験時のみ使用予定□■□■
                    '--------------------------------
                    '2014/04/16 星野　ここまで
                    '--------------------------------

                    '修理機器一覧画面起動
                    psOpen_Window(Me, M_METDTIL_DISP_PATH)

                    Me.lblExistsRp.Text = "1"
                Else
                    '修理がありません
                    psMesBox(Me, "30022", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "")

                    Me.lblExistsRp.Text = "0"
                    Me.btnGetRep.Enabled = False
                End If
            End If

            '終了ログ出力
            psLogEnd(Me)

        Catch ex As Exception
            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsProgNm)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' 行追加ボタン押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnRowAdd_Click(sender As Object, e As EventArgs) Handles btnRowAdd.Click

        '行追加ボタンの使用可不可を判定するフラグ
        Dim blnEnableBtnAdd As Boolean = True

        If (Page.IsValid) Then
            '明細コントロール設定（新規モード）
            Me.lblMode.Text = "1"
            mfCtrlDetail()

            'ラジオボタンのチェック処理
            'チェック解除
            Me.rdbEdaban1.Checked = False
            Me.rdbEdaban2.Checked = False
            Me.rdbEdaban3.Checked = False
            Me.rdbEdaban4.Checked = False
            Me.rdbEdaban5.Checked = False

            Dim objrdbEdaban As RadioButton
            For zz As Integer = 5 To 1 Step -1
                objrdbEdaban = Nothing
                Select Case zz
                    Case 5
                        objrdbEdaban = Me.rdbEdaban5
                    Case 4
                        objrdbEdaban = Me.rdbEdaban4
                    Case 3
                        objrdbEdaban = Me.rdbEdaban3
                    Case 2
                        objrdbEdaban = Me.rdbEdaban2
                    Case 1
                        objrdbEdaban = Me.rdbEdaban1
                End Select

                '活性化していたらチェック付けて抜ける
                If objrdbEdaban.Enabled = True Then
                    objrdbEdaban.Checked = True
                    If objrdbEdaban.ID = Me.rdbEdaban5.ID Then
                        blnEnableBtnAdd = False
                    End If
                    Exit For
                End If
            Next

            'ボタン制御
            mfBtnDetail()

            ''修理データに存在するかを判定する
            'Dim blnIsExistRp As Boolean = False
            'Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
            'Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
            'If Not clsDataConnect.pfOpen_Database(objCn) Then
            '    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'Else
            '    objCmd = New SqlCommand(sCnsProgId + "_S9", objCn)
            '    With objCmd.Parameters '--パラメータ設定
            '        .Add(pfSet_Param("meeting_no", SqlDbType.NVarChar, txtKanriNo.ppText))  '管理番号
            '    End With

            '    'データ取得
            '    Dim ds As DataSet = Nothing
            '    Try
            '        ds = clsDataConnect.pfGet_DataSet(objCmd)
            '        If ds.Tables(0).Rows.Count <> 0 Then
            '            Me.lblExistsRp.Text = "1"
            '        Else
            '            Me.lblExistsRp.Text = "0"
            '        End If
            '    Catch ex As Exception
            '        psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsProgNm)
            '        'ログ出力
            '        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
            '                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '    End Try
            'End If

            ''ボタン初期化
            'If Me.lblExistsRp.Text = "1" Then
            '    Me.btnGetRep.Enabled = True
            'Else
            '    Me.btnGetRep.Enabled = False
            'End If
            ''行追加ボタン(5行すべて活性化していたらボタン使用不可) 
            'Me.btnRowAdd.Enabled = blnEnableBtnAdd
            'Me.btnRowDel.Enabled = True
            'Me.btnRowUp.Enabled = False
            'Me.btnRowDn.Enabled = False
        End If








        ''行追加ボタンの使用可不可を判定するフラグ
        'Dim blnEnableBtnAdd As Boolean = True

        ''エラーチェック
        'Dim blnIsError As Boolean = True 'エラーチェック OK:True, NG:False
        'Dim objrdbEdaban As RadioButton
        'Dim objddlKosyou As DropDownList
        'For zz As Integer = 1 To 5
        '    objrdbEdaban = Nothing
        '    objddlKosyou = Nothing

        '    Select Case zz
        '        Case 1
        '            objrdbEdaban = Me.rdbEdaban1
        '            objddlKosyou = Me.ddlKosyoBui1
        '        Case 2
        '            objrdbEdaban = Me.rdbEdaban2
        '            objddlKosyou = Me.ddlKosyoBui2
        '        Case 3
        '            objrdbEdaban = Me.rdbEdaban3
        '            objddlKosyou = Me.ddlKosyoBui3
        '        Case 4
        '            objrdbEdaban = Me.rdbEdaban4
        '            objddlKosyou = Me.ddlKosyoBui4
        '        Case 5
        '            objrdbEdaban = Me.rdbEdaban5
        '            objddlKosyou = Me.ddlKosyoBui5
        '            blnEnableBtnAdd = False
        '    End Select

        '    'ラジオボタンのチェック解除
        '    objrdbEdaban.Checked = False

        '    '非活性行が初めて出た時点で、以降はチェックする必要なし
        '    If objrdbEdaban.Enabled = False Then
        '        Exit For
        '    End If
        '    '活性化している行で、空の製品名があった場合は、メッセージ出力
        '    If objddlKosyou.SelectedIndex = 0 Then
        '        psMesBox(Me, "30021", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "")
        '        blnIsError = False
        '    End If
        'Next

        ''明細コントロール設定（新規モード）
        'If blnIsError = True Then
        '    Me.lblMode.Text = "1"
        '    mfCtrlDetail()
        'End If

        ''ボタン初期化
        'Me.btnGetRep.Enabled = False
        ''行追加ボタン(5行すべて活性化していたらボタン使用不可) 
        'Me.btnRowAdd.Enabled = blnEnableBtnAdd
        'Me.btnRowDel.Enabled = False
        'Me.btnRowUp.Enabled = False
        'Me.btnRowDn.Enabled = False

    End Sub

    ''' <summary>
    ''' 行削除ボタン押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnRowDel_Click(sender As Object, e As EventArgs) Handles btnRowDel.Click

        Dim dt As DataTable = New DataTable
        Dim objrdbEdaban As RadioButton
        Dim objhdnSeq As HiddenField
        Dim objhdnEdaban As HiddenField
        Dim objddlKosyou As DropDownList
        Dim objddlIchiji As DropDownList
        Dim objtxtChousa As TextBox

        Try
            dt.Columns.Add("Seq", Type.GetType("System.String"))
            dt.Columns.Add("Edaban", Type.GetType("System.String"))
            dt.Columns.Add("Kosyou", Type.GetType("System.String"))
            dt.Columns.Add("Ichiji", Type.GetType("System.String"))
            dt.Columns.Add("Chousa", Type.GetType("System.String"))

            Dim intLastRow As Integer = 0   '明細の活性化している最後の行
            Dim intChkRow As Integer = 0    '明細のチェックが入っている行

            'まず現在のデータを格納する
            Dim dr As DataRow
            For zz As Integer = 0 To 4

                objrdbEdaban = Nothing
                objhdnSeq = Nothing
                objhdnEdaban = Nothing
                objddlKosyou = Nothing
                objddlIchiji = Nothing
                objtxtChousa = Nothing

                Select Case zz
                    Case 0
                        objrdbEdaban = Me.rdbEdaban1
                        objhdnSeq = Me.hdnSeq1
                        objhdnEdaban = Me.hdnEdaban1
                        objddlKosyou = Me.ddlKosyoBui1
                        objddlIchiji = Me.ddlIchijiShindan1
                        objtxtChousa = Me.txtChousaJyokyou1
                    Case 1
                        objrdbEdaban = Me.rdbEdaban2
                        objhdnSeq = Me.hdnSeq2
                        objhdnEdaban = Me.hdnEdaban2
                        objddlKosyou = Me.ddlKosyoBui2
                        objddlIchiji = Me.ddlIchijiShindan2
                        objtxtChousa = Me.txtChousaJyokyou2
                    Case 2
                        objrdbEdaban = Me.rdbEdaban3
                        objhdnSeq = Me.hdnSeq3
                        objhdnEdaban = Me.hdnEdaban3
                        objddlKosyou = Me.ddlKosyoBui3
                        objddlIchiji = Me.ddlIchijiShindan3
                        objtxtChousa = Me.txtChousaJyokyou3
                    Case 3
                        objrdbEdaban = Me.rdbEdaban4
                        objhdnSeq = Me.hdnSeq4
                        objhdnEdaban = Me.hdnEdaban4
                        objddlKosyou = Me.ddlKosyoBui4
                        objddlIchiji = Me.ddlIchijiShindan4
                        objtxtChousa = Me.txtChousaJyokyou4
                    Case 4
                        objrdbEdaban = Me.rdbEdaban5
                        objhdnSeq = Me.hdnSeq5
                        objhdnEdaban = Me.hdnEdaban5
                        objddlKosyou = Me.ddlKosyoBui5
                        objddlIchiji = Me.ddlIchijiShindan5
                        objtxtChousa = Me.txtChousaJyokyou5
                End Select

                '非活性行があらわれたら脱For
                If objrdbEdaban.Enabled = False Then
                    Exit For
                End If

                '何行目が編集行か
                If objrdbEdaban.Checked = True Then
                    intChkRow = zz + 1
                End If
                '何行目まで活性化されているか
                intLastRow = zz + 1

                dr = dt.NewRow
                dr("Seq") = objhdnSeq.Value
                dr("Edaban") = objhdnEdaban.Value
                dr("Kosyou") = objddlKosyou.SelectedValue
                dr("Ichiji") = objddlIchiji.SelectedValue
                dr("Chousa") = objtxtChousa.Text
                dt.Rows.Add(dr)
            Next

            'データ削除
            dt.Rows(intChkRow - 1).Delete()

            '削除した結果を明細に反映する
            'まず部品明細初期化
            msInitAppDetail()

            For zz As Integer = 0 To dt.Rows.Count - 1

                objrdbEdaban = Nothing
                objhdnSeq = Nothing
                objhdnEdaban = Nothing
                objddlKosyou = Nothing
                objddlIchiji = Nothing
                objtxtChousa = Nothing

                Select Case zz
                    Case 0
                        objhdnSeq = Me.hdnSeq1
                        objhdnEdaban = Me.hdnEdaban1
                        objddlKosyou = Me.ddlKosyoBui1
                        objddlIchiji = Me.ddlIchijiShindan1
                        objtxtChousa = Me.txtChousaJyokyou1
                    Case 1
                        objhdnSeq = Me.hdnSeq2
                        objhdnEdaban = Me.hdnEdaban2
                        objddlKosyou = Me.ddlKosyoBui2
                        objddlIchiji = Me.ddlIchijiShindan2
                        objtxtChousa = Me.txtChousaJyokyou2
                    Case 2
                        objhdnSeq = Me.hdnSeq3
                        objhdnEdaban = Me.hdnEdaban3
                        objddlKosyou = Me.ddlKosyoBui3
                        objddlIchiji = Me.ddlIchijiShindan3
                        objtxtChousa = Me.txtChousaJyokyou3
                    Case 3
                        objhdnSeq = Me.hdnSeq4
                        objhdnEdaban = Me.hdnEdaban4
                        objddlKosyou = Me.ddlKosyoBui4
                        objddlIchiji = Me.ddlIchijiShindan4
                        objtxtChousa = Me.txtChousaJyokyou4
                    Case 4
                        objhdnSeq = Me.hdnSeq5
                        objhdnEdaban = Me.hdnEdaban5
                        objddlKosyou = Me.ddlKosyoBui5
                        objddlIchiji = Me.ddlIchijiShindan5
                        objtxtChousa = Me.txtChousaJyokyou5
                End Select

                objhdnSeq.Value = dt.Rows(zz).Item("Seq").ToString
                objhdnEdaban.Value = dt.Rows(zz).Item("Edaban").ToString
                objddlKosyou.SelectedValue = dt.Rows(zz).Item("Kosyou").ToString
                objddlIchiji.SelectedValue = dt.Rows(zz).Item("Ichiji").ToString
                objtxtChousa.Text = dt.Rows(zz).Item("Chousa").ToString
            Next

            '明細コントロール設定
            'データテーブルにデータがなければ新規モード
            If dt.Rows.Count = 0 Then
                Me.lblMode.Text = "0"
            End If

            '削除前に活性化していた行よりも１少ない分活性化する
            mfCtrlDetail(intLastRow - 1)

            ''修理データに存在するかを判定する
            'Dim blnIsExistRp As Boolean = False
            'Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
            'Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
            'If Not clsDataConnect.pfOpen_Database(objCn) Then
            '    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'Else
            '    objCmd = New SqlCommand(sCnsProgId + "_S9", objCn)
            '    With objCmd.Parameters '--パラメータ設定
            '        .Add(pfSet_Param("meeting_no", SqlDbType.NVarChar, txtKanriNo.ppText))  '管理番号
            '    End With

            '    'データ取得
            '    Dim ds As DataSet = Nothing

            '    ds = clsDataConnect.pfGet_DataSet(objCmd)
            '    If ds.Tables(0).Rows.Count <> 0 Then
            '        Me.lblExistsRp.Text = "1"
            '    Else
            '        Me.lblExistsRp.Text = "0"
            '    End If
            'End If

            ''ボタン初期化
            'If Me.lblExistsRp.Text = "1" Then
            '    Me.btnGetRep.Enabled = True
            'Else
            '    Me.btnGetRep.Enabled = False
            'End If
            'Me.btnRowAdd.Enabled = True
            'Me.btnRowDel.Enabled = True
            'Me.btnRowUp.Enabled = False
            'Me.btnRowDn.Enabled = False

            'ラジオボタンチェック(１番上かチェックなし)
            If Me.rdbEdaban1.Enabled = True Then
                Me.rdbEdaban1.Checked = True
            End If

            'ボタン制御
            mfBtnDetail()

        Catch ex As Exception
            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsProgNm)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
    End Sub

    ''' <summary>
    ''' 上ボタン押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnRowUp_Click(sender As Object, e As EventArgs) Handles btnRowUp.Click
        ''子画面が開いていたら処理しない
        'If Me.hdnChildWindow.Value = "1" Then
        '    psMesBox(Me, "30023", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "")
        '    Exit Sub
        'Else
        '    Me.hdnKanriNo.Value = String.Empty
        '    Me.hdnCheckRow.Value = String.Empty
        'End If

        msUpDown("UP")
    End Sub

    ''' <summary>
    ''' 下ボタン押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnRowDn_Click(sender As Object, e As EventArgs) Handles btnRowDn.Click
        ''子画面が開いていたら処理しない
        'If Me.hdnChildWindow.Value = "1" Then
        '    psMesBox(Me, "30023", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "")
        '    Exit Sub
        'Else
        '    Me.hdnKanriNo.Value = String.Empty
        '    Me.hdnCheckRow.Value = String.Empty
        'End If

        msUpDown("DOWN")
    End Sub

    ''' <summary>
    ''' 上下処理
    ''' </summary>
    ''' <param name="strUpOrDown"></param>
    ''' <remarks></remarks>
    Private Sub msUpDown(ByVal strUpOrDown As String)
        Dim dt As New DataTable
        Dim objrdbEdaban As RadioButton
        Dim objhdnSeq As HiddenField
        Dim objhdnEdaban As HiddenField
        Dim objddlKosyou As DropDownList
        Dim objddlIchiji As DropDownList
        Dim objtxtChousa As TextBox

        Try
            dt.Columns.Add("Seq", Type.GetType("System.String"))
            dt.Columns.Add("Edaban", Type.GetType("System.String"))
            dt.Columns.Add("Kosyou", Type.GetType("System.String"))
            dt.Columns.Add("Ichiji", Type.GetType("System.String"))
            dt.Columns.Add("Chousa", Type.GetType("System.String"))

            Dim intChkRow As Integer = 0   '編集中の行数を格納する
            Dim intRowCnt As Integer = 0   '活性化されている行数を格納する

            'まず現在のデータを格納する
            Dim dr As DataRow
            For zz As Integer = 0 To 4

                objrdbEdaban = Nothing
                objhdnSeq = Nothing
                objhdnEdaban = Nothing
                objddlKosyou = Nothing
                objddlIchiji = Nothing
                objtxtChousa = Nothing

                Select Case zz
                    Case 0
                        objrdbEdaban = Me.rdbEdaban1
                        objhdnSeq = Me.hdnSeq1
                        objhdnEdaban = Me.hdnEdaban1
                        objddlKosyou = Me.ddlKosyoBui1
                        objddlIchiji = Me.ddlIchijiShindan1
                        objtxtChousa = Me.txtChousaJyokyou1
                    Case 1
                        objrdbEdaban = Me.rdbEdaban2
                        objhdnSeq = Me.hdnSeq2
                        objhdnEdaban = Me.hdnEdaban2
                        objddlKosyou = Me.ddlKosyoBui2
                        objddlIchiji = Me.ddlIchijiShindan2
                        objtxtChousa = Me.txtChousaJyokyou2
                    Case 2
                        objrdbEdaban = Me.rdbEdaban3
                        objhdnSeq = Me.hdnSeq3
                        objhdnEdaban = Me.hdnEdaban3
                        objddlKosyou = Me.ddlKosyoBui3
                        objddlIchiji = Me.ddlIchijiShindan3
                        objtxtChousa = Me.txtChousaJyokyou3
                    Case 3
                        objrdbEdaban = Me.rdbEdaban4
                        objhdnSeq = Me.hdnSeq4
                        objhdnEdaban = Me.hdnEdaban4
                        objddlKosyou = Me.ddlKosyoBui4
                        objddlIchiji = Me.ddlIchijiShindan4
                        objtxtChousa = Me.txtChousaJyokyou4
                    Case 4
                        objrdbEdaban = Me.rdbEdaban5
                        objhdnSeq = Me.hdnSeq5
                        objhdnEdaban = Me.hdnEdaban5
                        objddlKosyou = Me.ddlKosyoBui5
                        objddlIchiji = Me.ddlIchijiShindan5
                        objtxtChousa = Me.txtChousaJyokyou5
                End Select

                If objrdbEdaban.Enabled = False Then
                    Exit For
                Else
                    intRowCnt = intRowCnt + 1
                End If

                '対象行がどこかを特定
                If objrdbEdaban.Checked = True Then
                    intChkRow = zz + 1
                End If

                dr = dt.NewRow
                dr("Seq") = objhdnSeq.Value
                dr("Edaban") = objhdnEdaban.Value
                dr("Kosyou") = objddlKosyou.SelectedValue
                dr("Ichiji") = objddlIchiji.SelectedValue
                dr("Chousa") = objtxtChousa.Text
                dt.Rows.Add(dr)
            Next

            '入れ替え処理（一旦退避して、削除して追加する）
            Dim strSeq As String = dt.Rows(intChkRow - 1).Item("Seq").ToString
            Dim strEdaban As String = dt.Rows(intChkRow - 1).Item("Edaban").ToString
            Dim strKosyou As String = dt.Rows(intChkRow - 1).Item("Kosyou").ToString
            Dim strIchiji As String = dt.Rows(intChkRow - 1).Item("Ichiji").ToString
            Dim strChousa As String = dt.Rows(intChkRow - 1).Item("Chousa").ToString

            'データ削除
            dt.Rows(intChkRow - 1).Delete()
            'データ追加
            Dim intRow_Idx As Integer = 0    '動いた先の行数を格納
            Select Case strUpOrDown
                Case "UP"
                    intRow_Idx = intChkRow - 2
                Case "DOWN"
                    intRow_Idx = intChkRow
            End Select
            dr = dt.NewRow
            dr("Seq") = strSeq
            dr("Edaban") = strEdaban
            dr("Kosyou") = strKosyou
            dr("Ichiji") = strIchiji
            dr("Chousa") = strChousa
            dt.Rows.InsertAt(dr, intRow_Idx)


            '結果を明細に反映する
            'まず部品明細初期化
            msInitAppDetail()

            For zz As Integer = 0 To dt.Rows.Count - 1

                objrdbEdaban = Nothing
                objhdnSeq = Nothing
                objhdnEdaban = Nothing
                objddlKosyou = Nothing
                objddlIchiji = Nothing
                objtxtChousa = Nothing

                Select Case zz
                    Case 0
                        objrdbEdaban = Me.rdbEdaban1
                        objhdnSeq = Me.hdnSeq1
                        objhdnEdaban = Me.hdnEdaban1
                        objddlKosyou = Me.ddlKosyoBui1
                        objddlIchiji = Me.ddlIchijiShindan1
                        objtxtChousa = Me.txtChousaJyokyou1
                    Case 1
                        objrdbEdaban = Me.rdbEdaban2
                        objhdnSeq = Me.hdnSeq2
                        objhdnEdaban = Me.hdnEdaban2
                        objddlKosyou = Me.ddlKosyoBui2
                        objddlIchiji = Me.ddlIchijiShindan2
                        objtxtChousa = Me.txtChousaJyokyou2
                    Case 2
                        objrdbEdaban = Me.rdbEdaban3
                        objhdnSeq = Me.hdnSeq3
                        objhdnEdaban = Me.hdnEdaban3
                        objddlKosyou = Me.ddlKosyoBui3
                        objddlIchiji = Me.ddlIchijiShindan3
                        objtxtChousa = Me.txtChousaJyokyou3
                    Case 3
                        objrdbEdaban = Me.rdbEdaban4
                        objhdnSeq = Me.hdnSeq4
                        objhdnEdaban = Me.hdnEdaban4
                        objddlKosyou = Me.ddlKosyoBui4
                        objddlIchiji = Me.ddlIchijiShindan4
                        objtxtChousa = Me.txtChousaJyokyou4
                    Case 4
                        objrdbEdaban = Me.rdbEdaban5
                        objhdnSeq = Me.hdnSeq5
                        objhdnEdaban = Me.hdnEdaban5
                        objddlKosyou = Me.ddlKosyoBui5
                        objddlIchiji = Me.ddlIchijiShindan5
                        objtxtChousa = Me.txtChousaJyokyou5
                End Select

                If zz = intRow_Idx Then
                    objrdbEdaban.Checked = True
                End If
                objhdnSeq.Value = dt.Rows(zz).Item("Seq").ToString
                objhdnEdaban.Value = dt.Rows(zz).Item("Edaban").ToString
                objddlKosyou.SelectedValue = dt.Rows(zz).Item("Kosyou").ToString
                objddlIchiji.SelectedValue = dt.Rows(zz).Item("Ichiji").ToString
                objtxtChousa.Text = dt.Rows(zz).Item("Chousa").ToString
            Next

            '明細コントロール設定
            mfCtrlDetail(intRowCnt)

            'ボタン制御
            mfBtnDetail()

            ''ボタン制御
            'Dim rdbEdaban As RadioButton = Nothing
            'Dim rdbEdaban_last As RadioButton = Nothing

            'Select Case intRow_Idx
            '    Case 0
            '        rdbEdaban = Me.rdbEdaban1
            '    Case 1
            '        rdbEdaban = Me.rdbEdaban2
            '    Case 2
            '        rdbEdaban = Me.rdbEdaban3
            '    Case 3
            '        rdbEdaban = Me.rdbEdaban4
            '    Case 4
            '        rdbEdaban = Me.rdbEdaban5
            'End Select

            ''行追加ボタン(5行すべて活性化していたらボタン使用不可) 
            'If intRowCnt = 5 Then
            '    Me.btnRowAdd.Enabled = False
            'Else
            '    Me.btnRowAdd.Enabled = True
            'End If

            ''▲ボタン(１行目だったら非活性)
            'If rdbEdaban.ID = Me.rdbEdaban1.ID Then
            '    Me.btnRowUp.Enabled = False
            'Else
            '    Me.btnRowUp.Enabled = True
            'End If

            ''▼ボタン(５行目または最後に活性化されている行だったら非活性)
            'For zz As Integer = 1 To 5
            '    Select Case True
            '        Case Me.rdbEdaban5.Enabled
            '            rdbEdaban_last = Me.rdbEdaban5
            '            Exit For
            '        Case Me.rdbEdaban4.Enabled
            '            rdbEdaban_last = Me.rdbEdaban4
            '            Exit For
            '        Case Me.rdbEdaban3.Enabled
            '            rdbEdaban_last = Me.rdbEdaban3
            '            Exit For
            '        Case Me.rdbEdaban2.Enabled
            '            rdbEdaban_last = Me.rdbEdaban2
            '            Exit For
            '        Case Me.rdbEdaban1.Enabled
            '            rdbEdaban_last = Me.rdbEdaban1
            '            Exit For
            '    End Select
            'Next
            'If rdbEdaban.ID = Me.rdbEdaban5.ID Then
            '    Me.btnRowDn.Enabled = False
            'Else
            '    If rdbEdaban.ID = rdbEdaban_last.ID Then
            '        Me.btnRowDn.Enabled = False
            '    Else
            '        Me.btnRowDn.Enabled = True
            '    End If
            'End If

        Catch ex As Exception
            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, sCnsProgNm)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
    End Sub

    ''' <summary>
    ''' 部品明細初期化処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msInitAppDetail()
        Me.rdbEdaban1.Checked = False
        Me.rdbEdaban2.Checked = False
        Me.rdbEdaban3.Checked = False
        Me.rdbEdaban4.Checked = False
        Me.rdbEdaban5.Checked = False
        Me.hdnSeq1.Value = String.Empty
        Me.hdnSeq2.Value = String.Empty
        Me.hdnSeq3.Value = String.Empty
        Me.hdnSeq4.Value = String.Empty
        Me.hdnSeq5.Value = String.Empty
        Me.hdnEdaban1.Value = String.Empty
        Me.hdnEdaban2.Value = String.Empty
        Me.hdnEdaban3.Value = String.Empty
        Me.hdnEdaban4.Value = String.Empty
        Me.hdnEdaban5.Value = String.Empty
        Me.ddlKosyoBui1.SelectedIndex = 0
        Me.ddlKosyoBui2.SelectedIndex = 0
        Me.ddlKosyoBui3.SelectedIndex = 0
        Me.ddlKosyoBui4.SelectedIndex = 0
        Me.ddlKosyoBui5.SelectedIndex = 0
        Me.ddlIchijiShindan1.SelectedIndex = 0
        Me.ddlIchijiShindan2.SelectedIndex = 0
        Me.ddlIchijiShindan3.SelectedIndex = 0
        Me.ddlIchijiShindan4.SelectedIndex = 0
        Me.ddlIchijiShindan5.SelectedIndex = 0
        Me.txtChousaJyokyou1.Text = String.Empty
        Me.txtChousaJyokyou2.Text = String.Empty
        Me.txtChousaJyokyou3.Text = String.Empty
        Me.txtChousaJyokyou4.Text = String.Empty
        Me.txtChousaJyokyou5.Text = String.Empty
    End Sub

End Class
