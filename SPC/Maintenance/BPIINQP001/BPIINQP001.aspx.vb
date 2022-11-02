'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　貸玉数　設定情報
'*　ＰＧＭＩＤ：　BPIINQP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.17　：　ＸＸＸ
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'BPIINQP001-001     2016/08/01      加賀　　　[e-basカスタマイズ]


#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax

#End Region

Public Class BPIINQP001

#Region "継承定義"
    Inherits System.Web.UI.Page
#End Region

#Region "定数定義"
    'プログラムID
    Const M_MY_DISP_ID = P_FUN_BPI & P_SCR_INQ & P_PAGE & "001"
    '次画面ファイルパス
    Const M_NEXT_DISP_PATH = "~/" & P_MAI & "/" &
            P_FUN_BPI & P_SCR_FIX & P_PAGE & "001" & "/" &
            P_FUN_BPI & P_SCR_FIX & P_PAGE & "001.aspx"

#End Region

#Region "変数定義"
    Dim objStack As StackFrame
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom

#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page_Initイベント
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, M_MY_DISP_ID)
    End Sub

    ''' <summary>
    ''' Page_Loadイベント
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click
        AddHandler Master.ppRigthButton3.Click, AddressOf btnInquiry_Click
        objStack = New StackFrame

        Try
            If Not IsPostBack Then  '初回表示のみ

                '照会ボタンを活性化
                Master.ppRigthButton3.Visible = True
                Master.ppRigthButton3.Text = "照会"

                '検索条件クリアボタン押下時の検証を無効
                Master.ppRigthButton2.CausesValidation = False

                '確認メッセージ設定 'BPIINQP001-001
                Master.ppRigthButton3.OnClientClick =
                pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "貸玉数　設定情報の照会")  '{0}を行います。\nよろしいですか？

                '画面設定
                Master.Master.ppProgramID = M_MY_DISP_ID
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)
                Master.Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる

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

                '画面クリア
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
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
    End Sub

    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
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
    ''' 検索ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        Try
            If (Page.IsValid) Then

                'BPIINQP001-001 ?
                ''SQLコマンド設定
                'Using cmdSQL As SqlCommand = New SqlCommand(M_MY_DISP_ID + "_S2")
                '    'パラメータ設定
                '    cmdSQL.Parameters.Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxId.ppText))    'TBOXID

                '    'データ取得実行
                '    If pfExec_StoredProcedure(Me, "印刷データ", cmdSQL, dstTbox) = False Then
                '        Exit Sub
                '    End If
                'End Using

                ''データ有無確認
                'If dstTbox.Tables(0).Rows.Count = 0 Then
                '    Me.txtTboxId.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
                '    Me.txtTboxId.ppTextBox.Focus()
                '    Exit Sub
                'End If

                ''取得情報表示
                'With dstTbox.Tables(0).Rows(0)
                '    Me.lblHallNm2.Text = .Item("ホール名").ToString
                '    Me.lblVER2.Text = .Item("ＶＥＲ").ToString
                'End With

                ''一覧データ取得
                'msGet_Data("0")
                'BPIINQP001-001

                If Not txtTboxId.ppText = String.Empty Then
                    '整合性チェック
                    If Not mfGet_TboxData(1, Me.txtTboxId.ppText) Then
                        Exit Sub
                    End If
                End If

                'データ取得
                msGet_Data("0")

                'DataSet破棄
                'dstTbox.Dispose()

            End If

        Catch ex As Exception
            'データ取得エラー
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "貸玉数　設定情報")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 検索条件クリアボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)
        '開始ログ出力
        psLogStart(Me)

        '画面クリア
        'BPIINQP001-001
        'msClear_Screen()
        Me.txtTboxId.ppText = String.Empty          'ＴＢＯＸＩＤ
        Me.lblHallNm2.Text = String.Empty           'ホール名
        Me.lblVER2.Text = String.Empty              'ＶＥＲ
        Me.txtTboxId.ppTextBox.Focus()
        'BPIINQP001-001 END

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 照会ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnInquiry_Click(sender As Object, e As EventArgs)

        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット
        Dim dtRow As DataRow = Nothing              'DataRow
        Dim intRtn As Integer
        objStack = New StackFrame

        '必須入力チェック
        If Me.txtTboxId.ppText = String.Empty Then
            Me.txtTboxId.psSet_ErrorNo("5001", "ＴＢＯＸＩＤ")
            Me.txtTboxId.ppTextBox.Focus()
            Exit Sub
        End If

        '開始ログ出力
        psLogStart(Me)

        If (Page.IsValid) Then

            '整合性チェック
            If Not mfGet_TboxData(2, Me.txtTboxId.ppText) Then
                Exit Sub
            End If

            '照会中データ確認
            If mfGet_RefData(Me.txtTboxId.ppText, User.Identity.Name, Request.ServerVariables("REMOTE_ADDR")) Then
                psMesBox(Me, "20003", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                Exit Sub
            End If

            'DB接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    cmdDB = New SqlCommand(M_MY_DISP_ID + "_I1", conDB)
                    'パラメータ設定
                    With cmdDB.Parameters
                        .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxId.ppText))                        'ＴＢＯＸＩＤ
                        .Add(pfSet_Param("user_id", SqlDbType.NVarChar, User.Identity.Name))                        'ユーザーＩＤ
                        .Add(pfSet_Param("term_nm", SqlDbType.NVarChar, Request.ServerVariables("REMOTE_ADDR")))    '端末情報
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                           '戻り値
                    End With

                    'データ追加
                    Using conTrn = conDB.BeginTransaction
                        cmdDB.Transaction = conTrn
                        dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn = 1 Then
                            conTrn.Rollback()
                            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "照会要求データ")
                            Exit Try

                        ElseIf intRtn = 2 Then
                            conTrn.Rollback()
                            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "照会要求受付DB")
                            Exit Try
                        End If

                        'コミット
                        conTrn.Commit()

                        'データ再取得
                        msGet_Data("0")

                    End Using

                Catch ex As Exception
                    'データ追加エラー
                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "照会情報")
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    'Me.grvList.DataSource = New DataTable
                    'Me.grvList.DataBind()
                    'Master.ppCount = "0"
                Finally
                    'DB切断
                    If Not clsDataConnect.pfClose_Database(conDB) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                End Try
            Else
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()
                Master.ppCount = "0"
            End If
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' グリッドのボタン押下時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        If e.CommandName <> "btnPrint" And e.CommandName <> "btnDifCon" Then
            Exit Sub
        End If

        '開始ログ出力
        psLogStart(Me)

        Dim intIndex As Integer = Nothing
        Dim rowData As GridViewRow = Nothing
        Dim lstKey As New List(Of String)

        intIndex = Convert.ToInt32(e.CommandArgument)   'ボタン押下行のIndex
        rowData = grvList.Rows(intIndex)                'ボタン押下行

        Select Case e.CommandName
            Case "btnPrint"     '印刷
                Try
                    Dim clsReport As New BPIREP002          'レポートクラス
                    Dim dstReport As New DataSet            'データセット
                    'Dim dttData As DataTable = Nothing      'データテーブル

                    'BPIINQP001-001
                    lstKey.Add(CType(rowData.FindControl("照会管理番号"), TextBox).Text)   '照会管理番号
                    lstKey.Add(CType(rowData.FindControl("要求枝番"), TextBox).Text)   '要求枝番

                    'SQLコマンド設定
                    Using cmdSQL As SqlCommand = New SqlCommand("SPCDB.dbo." + "BPIINQP001_S3")
                        'パラメータ設定
                        With cmdSQL.Parameters
                            .Add(pfSet_Param("@CTRL_NO", SqlDbType.NVarChar, lstKey(0)))
                            .Add(pfSet_Param("@EDA_SEQ", SqlDbType.NVarChar, lstKey(1)))
                        End With

                        'データ取得実行
                        If pfExec_StoredProcedure(Me, "印刷データ", cmdSQL, dstReport) = False Then
                            Exit Sub
                        End If
                    End Using

                    '印刷対象データ確認
                    If dstReport.Tables(0).Rows.Count = 0 Then
                        '印刷対象データなし
                        psMesBox(Me, "00007", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)  '印刷対象データがありません。
                    Else
                        '帳票出力
                        psPrintPDF(Me, clsReport, dstReport.Tables(0), "貸玉数　設定情報一覧")
                    End If


                    'lstKey = New List(Of String)
                    ''パラメータ設定
                    'lstKey.Add(CType(rowData.FindControl("依頼日時"), TextBox).Text)               '依頼日時
                    'Select Case CType(rowData.FindControl("照会種別"), TextBox).Text                'データ種別
                    '    Case "BB機番情報"
                    '        lstKey.Add("29")
                    '    Case "BB機番情報2"
                    '        lstKey.Add("86")
                    'End Select
                    'lstKey.Add(CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text)           'ＴＢＯＸＩＤ

                    'If mfGet_BPIInfo(lstKey(0), lstKey(1), lstKey(2), dstReport) Then
                    '    'ＰＤＦデータ取得処理
                    '    dttData = dstReport.Tables(0)
                    '    clsReport = New BPIREP002

                    '    'ファイル出力
                    '    psPrintPDF(Me, clsReport, dttData, "貸玉数　設定情報一覧")
                    'End If

                    'BPIINQP001-001 END

                Catch ex As Exception
                    'エラー
                    psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "貸玉単価設定確認リスト")       '{0}の出力に失敗しました。
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
                End Try

            Case "btnDifCon"    '差異確認

                '次画面引継ぎ用キー情報設定
                lstKey = New List(Of String)
                'BPIINQP001-001
                lstKey.Add(CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text)
                lstKey.Add(CType(rowData.FindControl("照会管理番号"), TextBox).Text)
                lstKey.Add(CType(rowData.FindControl("要求枝番"), TextBox).Text)
                'lstKey.Add(CType(rowData.FindControl("照会管理番号"), TextBox).Text)
                'lstKey.Add(CType(rowData.FindControl("照会通番"), TextBox).Text)
                'lstKey.Add("01")
                'lstKey.Add(CType(rowData.FindControl("監視サーバ運用日付"), TextBox).Text)
                'lstKey.Add(CType(rowData.FindControl("連番"), TextBox).Text)
                'lstKey.Add(CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text)
                'BPIINQP001-001 END

                'セッション情報設定
                Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
                Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)
                Session(P_KEY) = lstKey.ToArray

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
                                objStack.GetMethod.Name, M_NEXT_DISP_PATH, strPrm, "TRANS")

                '貸玉数　設定情報差異確認画面起動
                psOpen_Window(Me, M_NEXT_DISP_PATH)
        End Select
    End Sub

    ''' <summary>
    ''' データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        Dim strStatus As String
        Dim strResult As String
        Dim strDataCls As String    'BPIINQP001-001

        Try
            '照会状況、照会結果によりボタンや文字色を設定
            For Each rowData As GridViewRow In grvList.Rows
                strStatus = CType(rowData.FindControl("照会状況"), TextBox).Text
                strResult = CType(rowData.FindControl("照会結果"), TextBox).Text
                strDataCls = CType(rowData.FindControl("照会種別コード"), TextBox).Text    'BPIINQP001-001

                'If strResult <> "正常終了" Or strStatus <> "照会完了" Then 'BPIINQP001-001
                If strResult <> "正常終了" OrElse strStatus <> "照会完了" OrElse strDataCls <> "88" Then
                    rowData.Cells(0).Enabled = False
                    rowData.Cells(1).Enabled = False
                End If

                If strResult <> "正常終了" And strStatus <> "照会中" Then
                    CType(rowData.FindControl("照会種別"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("ホール名"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("照会状況"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("照会結果"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("照会日時"), TextBox).ForeColor = Drawing.Color.Red
                End If
            Next

        Catch ex As Exception
            'メッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "一覧の生成")
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
    Private Sub msClear_Screen()

        Me.txtTboxId.ppText = String.Empty          'ＴＢＯＸＩＤ
        Me.lblHallNm2.Text = String.Empty           'ホール名
        Me.lblVER2.Text = String.Empty              'ＶＥＲ
        Me.grvList.DataSource = New DataTable
        Master.ppCount = "0"
        Me.grvList.DataBind()
        Me.txtTboxId.ppTextBox.Focus()

    End Sub

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <param name="ipstrDefault">初期表示フラグ（1：初期　1以外：初期以外）</param>
    ''' <remarks></remarks>
    Private Sub msGet_Data(ByRef ipstrDefault As String)

        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット
        objStack = New StackFrame

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then

            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_S1", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxId.ppText))        'ＴＢＯＸＩＤ
                    .Add(pfSet_Param("disp_id", SqlDbType.NVarChar, M_MY_DISP_ID))              '画面ＩＤ
                    .Add(pfSet_Param("default", SqlDbType.NVarChar, ipstrDefault))              '初期表示フラグ
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ有無確認
                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    Master.ppCount = dstOrders.Tables(0).Rows.Count.ToString
                Else
                    '閾値を超えた場合はメッセージを表示
                    If dstOrders.Tables(0).Rows(0)("データ件数") > dstOrders.Tables(0).Rows.Count Then
                        psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                            dstOrders.Tables(0).Rows(0)("データ件数").ToString, dstOrders.Tables(0).Rows.Count.ToString)
                    End If
                    Master.ppCount = dstOrders.Tables(0).Rows(0)("データ件数").ToString
                End If

                '取得したデータをグリッドに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "貸玉数　設定情報")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()
                Master.ppCount = "0"
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"
        End If
    End Sub

    ''' <summary>
    ''' 貸玉数　設定情報取得処理
    ''' </summary>
    ''' <param name="ipstrReqDateSPC">依頼日時</param>
    ''' <param name="ipstrDataClass">データ種別</param>
    ''' <param name="ipstrTboxId">ＴＢＯＸＩＤ</param>
    ''' <param name="opdstData">貸玉数　設定情報項目</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_BPIInfo(ByVal ipstrReqDateSPC As String _
                                 , ByVal ipstrDataClass As String _
                                 , ByVal ipstrTboxId As String _
                                 , ByRef opdstData As DataSet) As Boolean

        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        objStack = New StackFrame

        mfGet_BPIInfo = False

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_S3", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("reqdate_spc", SqlDbType.NVarChar, ipstrReqDateSPC))       '依頼日時
                    .Add(pfSet_Param("dataclass", SqlDbType.NVarChar, ipstrDataClass))          'データ種別
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, ipstrTboxId))                'ＴＢＯＸＩＤ
                End With

                'データ取得
                opdstData = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ有無確認
                If opdstData.Tables(0).Rows.Count = 0 Then
                    psMesBox(Me, "00007", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "貸玉数　設定情報")
                    Exit Function
                End If

                mfGet_BPIInfo = True

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "貸玉数　設定情報取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If
    End Function

    ''' <summary>
    ''' ＴＢＯＸ情報取得処理
    ''' </summary>
    ''' <param name="ipstrTboxId">ＴＢＯＸＩＤ</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_TboxData(ByVal ipstrProc As Integer, ByVal ipstrTboxId As String) As Boolean

        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstTbox As DataSet = Nothing
        objStack = New StackFrame

        mfGet_TboxData = False

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then

            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_S2", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, ipstrTboxId))        'ＴＢＯＸＩＤ
                End With

                'データ取得
                dstTbox = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ有無確認
                If dstTbox.Tables(0).Rows.Count = 0 Then
                    Me.txtTboxId.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
                    Me.txtTboxId.ppTextBox.Focus()
                    Exit Function
                Else
                    'BPIINQP001-001
                    With dstTbox.Tables(0).Rows(0)
                        Me.lblHallNm2.Text = .Item("ホール名").ToString
                        Me.lblVER2.Text = .Item("ＶＥＲ").ToString
                    End With

                    If ipstrProc = 2 AndAlso DirectCast(dstTbox.Tables(0).Rows(0).Item("照会可否"), Integer) = 0 Then
                        Me.txtTboxId.psSet_ErrorNo("7003") 'TBOXIDのVERが対象外です。
                        Me.txtTboxId.ppTextBox.Focus()
                        Exit Function
                    End If
                End If

                'With dstTbox.Tables(0).Rows(0)
                '    Me.lblHallNm2.Text = .Item("ホール名").ToString
                '    Me.lblVER2.Text = .Item("ＶＥＲ").ToString
                'End With

                dstTbox.Dispose()

                mfGet_TboxData = True

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ情報")
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
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"
        End If
    End Function


    ''' <summary>
    ''' 照会中データ取得処理
    ''' </summary>
    ''' <param name="ipstrTboxId">ＴＢＯＸＩＤ</param>
    ''' <param name="ipstrUserId">ユーザＩＤ</param>
    ''' <param name="ipstrTermNm">端末情報</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_RefData(ByVal ipstrTboxId As String _
                                 , ByVal ipstrUserId As String _
                                 , ByVal ipstrTermNm As String) As Boolean
        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット

        objStack = New StackFrame

        mfGet_RefData = False

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_S4", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, ipstrTboxId))        'ＴＢＯＸＩＤ
                    .Add(pfSet_Param("user_id", SqlDbType.NVarChar, ipstrUserId))       'ユーザーＩＤ
                    .Add(pfSet_Param("term_nm", SqlDbType.NVarChar, ipstrTermNm))       '端末情報
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ有無確認
                If dstOrders.Tables(0).Rows.Count = 0 Then
                    Exit Function
                End If

                mfGet_RefData = True

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "貸玉数　設定情報取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If
    End Function

#End Region

End Class
