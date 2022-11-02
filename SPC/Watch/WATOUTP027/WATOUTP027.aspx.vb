'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜監視＞
'*　処理名　　：　ＴＢＯＸ結果表示　ホール機器設定情報
'*　ＰＧＭＩＤ：　WATOUTP027
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2016/06/21　：　加賀　　　[e-basカスタマイズ]e-mon作成分をベースに作成
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'WATOUTP027-000     2016/XX/XX      XXXX　　　XXXX

Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon

Public Class WATOUTP027

#Region "継承定義"

    Inherits System.Web.UI.Page

#End Region

#Region "定数定義"

    'プログラムＩＤ
    Const M_DISP_ID = P_FUN_WAT & P_SCR_OUT & P_PAGE & "027"

#End Region

#Region "構造体・列挙体定義"

    ''' <summary>
    ''' 帳票種別
    ''' </summary>
    Private Enum ReportCls As Short
        TBOX設定情報 = 1
        BB機器情報 = 2
    End Enum

#End Region

#Region "変数定義"

    Dim objStack As StackFrame
    Dim clsCMDBC As New ClsCMDBCom

#End Region


#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page_Loadイベント
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        objStack = New StackFrame

        Try
            'イベント設定
            AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnPrint_Click         '印刷
            AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click               '検索
            AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click                '検索クリア
            AddHandler Me.tftJBNo.ppTextBoxFrom.TextChanged, AddressOf txtJBNo_TextChanged  'JBFrom
            AddHandler Me.tftJBNo.ppTextBoxTo.TextChanged, AddressOf txtJBNo_TextChanged    'JBTo

            '初回表示
            If Not IsPostBack Then

                'セッションから情報を取得
                ViewState(P_KEY) = Session(P_KEY)   '0：照会管理番号  1：連番  2：ＮＬ区分  3：ＩＤＩＣ区分  4：要求通番  5：枝番

                With Master.Master
                    '画面ヘッダ設定
                    .ppProgramID = M_DISP_ID
                    .ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)
                    .ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)    'パンくずリスト設定
                    .ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる
                    Master.ppCount_Visible = False

                    '印刷ボタン設定
                    .ppRigthButton1.Visible = True
                    .ppRigthButton1.Text = "印刷"
                    .ppRigthButton1.Enabled = False
                    .ppRigthButton1.CausesValidation = False
                    .ppRigthButton1.OnClientClick = pfGet_OCClickMes("10002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "帳票")
                End With

                '検索エリア設定
                Me.tftJBNo.ppEnabled = False
                Master.ppRigthButton1.Enabled = False
                Master.ppRigthButton2.Enabled = False
                Master.ppRigthButton2.CausesValidation = False

                'JB番号にAutoPostBack設定
                Me.tftJBNo.ppTextBoxFrom.AutoPostBack = True
                Me.tftJBNo.ppTextBoxTo.AutoPostBack = True

                'TBOX設定情報選択
                rblSlctPrnt.SelectedIndex = 0

                '「検索中」メッセージ表示javascript付与
                Master.ppRigthButton1.OnClientClick = "showMsgDiv()"

                '帳票選択の自動ポストバックを有効
                Me.rblSlctPrnt.AutoPostBack = True

                'データ取得・表示
                setInfo(0, String.Empty, String.Empty)

            End If

        Catch ex As Exception
            'エラーメッセージ表示
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後) '画面の初期化に失敗しました。
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' 検索ボタン押下時
    ''' </summary>
    Protected Sub btnSearch_Click()

        Try
            '入力チェック？
            If Me.IsValid Then

                'データ取得・表示
                setInfo(1, Me.tftJBNo.ppFromText, Me.tftJBNo.ppToText)

                '検索条件保存
                Me.hdnJB_From.Value = Me.tftJBNo.ppFromText
                Me.hdnJB_To.Value = Me.tftJBNo.ppToText
            End If
        Catch ex As Exception
            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "検索")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' 検索条件クリア押下時
    ''' </summary>
    Protected Sub btnClear_Click()

        Try
            'JB番号
            Me.tftJBNo.ppFromText = String.Empty
            Me.tftJBNo.ppToText = String.Empty
        Catch ex As Exception
            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "検索条件クリア")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' 印刷ボタン押下時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim dstOrders As New DataSet
        Dim rpt() As Object = Nothing
        Dim intRptCls As Integer
        Dim strReportName() As String = Nothing
        Dim strKeyList() As String = ViewState(P_KEY)
        Dim strStoredNm As String = String.Empty
        objStack = New StackFrame

        Try
            '選択された帳票を取得
            intRptCls = Integer.Parse(rblSlctPrnt.SelectedValue)

            Select Case intRptCls
                Case ReportCls.TBOX設定情報
                    strReportName = {"ＴＢＯＸ設定情報", "ＴＢＯＸ設定情報"}
                    strStoredNm = "TBRREP015_S1"
                    rpt = {New TBRREP016, New TBRREP015}
                Case ReportCls.BB機器情報
                    strReportName = {"ＢＢ機器情報"}
                    strStoredNm = "TBRREP015_S2"
                    rpt = {New TBRREP017}
            End Select

            'SQLコマンド設定
            Using cmdSQL As SqlCommand = New SqlCommand("SPCDB.dbo." + strStoredNm)
                'パラメータ設定
                With cmdSQL.Parameters
                    .Add(pfSet_Param("ctrl_no", SqlDbType.NVarChar, strKeyList(0)))
                    .Add(pfSet_Param("nl_cls", SqlDbType.NVarChar, strKeyList(2)))
                    .Add(pfSet_Param("eda_seq", SqlDbType.NVarChar, strKeyList(6)))
                    If intRptCls = ReportCls.BB機器情報 Then
                        .Add(pfSet_Param("jbnumberST", SqlDbType.NVarChar, Me.hdnJB_From.Value))
                        .Add(pfSet_Param("jbnumberED", SqlDbType.NVarChar, Me.hdnJB_To.Value))
                    End If
                End With

                'データ取得実行
                If pfExec_StoredProcedure(Me, strReportName(0), cmdSQL, dstOrders) = False Then
                    Exit Sub
                End If
            End Using

            'テーブルに該当レコードがない場合はポップアップを表示
            If dstOrders Is Nothing OrElse dstOrders.Tables(0).Rows.Count = 0 Then
                '該当するデータが存在しません。
                psMesBox(Me, "00007", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)
            Else
                '帳票を出力する
                Select Case intRptCls
                    Case ReportCls.TBOX設定情報
                        psPrintPDF(Me, rpt, {dstOrders.Tables(1), dstOrders.Tables(0)}, strReportName)
                    Case ReportCls.BB機器情報
                        psPrintPDF(Me, rpt(0), dstOrders.Tables(0), strReportName(0))
                End Select
            End If

        Catch ex As Exception
            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "印刷")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'データセット破棄
            dstOrders.Dispose()
        End Try

    End Sub

    ''' <summary>
    ''' 印刷対象ラジオボタン選択時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub rblSlctPrnt_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblSlctPrnt.SelectedIndexChanged

        Dim intRptCls As Integer

        objStack = New StackFrame
        Try
            '印刷ボタンを活性にする
            Master.Master.ppRigthButton1.Enabled = True

            '選択された帳票を取得
            intRptCls = Integer.Parse(rblSlctPrnt.SelectedValue)

            Select Case intRptCls
                Case ReportCls.TBOX設定情報
                    Me.tftJBNo.ppEnabled = False
                    Master.ppRigthButton1.Enabled = False
                    Master.ppRigthButton2.Enabled = False
                    Master.Master.ppRigthButton1.Enabled = True
                Case ReportCls.BB機器情報
                    Me.tftJBNo.ppEnabled = True
                    Master.ppRigthButton1.Enabled = True
                    Master.ppRigthButton2.Enabled = True
                    If lblRecordCount.Text = "0" Then
                        Master.Master.ppRigthButton1.Enabled = False
                    Else
                        Master.Master.ppRigthButton1.Enabled = True
                    End If
                Case Else
                    Me.tftJBNo.ppEnabled = False
                    Master.ppRigthButton1.Enabled = False
                    Master.ppRigthButton2.Enabled = False
                    Master.Master.ppRigthButton1.Enabled = False
            End Select

        Catch ex As Exception
            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "帳票の選択")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' JB番号変更時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub txtJBNo_TextChanged(sender As Object, e As EventArgs)

        '印刷ボタンを非活性にする
        Master.Master.ppRigthButton1.Enabled = False

        'フォーカス設定
        If sender.Equals(tftJBNo.ppTextBoxFrom) Then
            tftJBNo.ppTextBoxTo.Focus()
        Else
            Master.ppRigthButton2.Focus()
        End If

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面情報取得・表示
    ''' </summary>
    ''' <param name="intProcCode">処理コード 0:初回表示 1:検索</param>
    ''' <param name="strJBNo_From">JB番号From</param>
    ''' <param name="strJBNo_To">JB番号To</param>
    Private Sub setInfo(ByVal intProcCode As Integer, ByVal strJBNo_From As String, ByVal strJBNo_To As String)

        Dim strKeyList() As String = ViewState(P_KEY)
        Dim dstOrders As New DataSet
        Dim strStoredNm As String = "WATOUTP027_S1"

        'SQLコマンド設定
        Using cmdSQL As SqlCommand = New SqlCommand("SPCDB.dbo." + strStoredNm)
            'パラメータ設定
            With cmdSQL.Parameters
                .Add(pfSet_Param("ctrl_no", SqlDbType.NVarChar, strKeyList(0)))
                .Add(pfSet_Param("nl_cls", SqlDbType.NVarChar, strKeyList(2)))
                .Add(pfSet_Param("eda_seq", SqlDbType.NVarChar, strKeyList(6)))
                .Add(pfSet_Param("jbnumberST", SqlDbType.NVarChar, strJBNo_From))
                .Add(pfSet_Param("jbnumberED", SqlDbType.NVarChar, strJBNo_To))
            End With

            'データ取得実行
            If pfExec_StoredProcedure(Me, "ホール機器設定情報", cmdSQL, dstOrders) = False Then
                Exit Sub
            End If
        End Using

        '画面反映
        If dstOrders Is Nothing _
        OrElse dstOrders.Tables.Count < 2 _
        OrElse dstOrders.Tables(0).Rows.Count < 1 _
        OrElse dstOrders.Tables(1).Rows.Count < 1 Then
            '該当するデータが存在しません。
            psMesBox(Me, "00007", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)
            '該当件数設定
            Me.lblRecordCount.Text = "0"
            '印刷ボタン非活性
            Master.Master.ppRigthButton1.Enabled = False
            '初回表示
            If intProcCode = 0 Then
                Me.lblTboxId.Text = String.Empty
                Me.lblHallNm.Text = String.Empty
                Me.lblVer.Text = String.Empty
                Me.lblRcvDate.Text = String.Empty
                Me.lblJBNo_From.Text = String.Empty
                Me.lblJBNo_To.Text = String.Empty
                rblSlctPrnt.Enabled = False

                '閉じる
                psClose_Window(Me)
            End If
        Else
            Dim drwSelect() As DataRow = {dstOrders.Tables(0).Rows(0), dstOrders.Tables(1).Rows(0)}

            If intProcCode = 0 Then
                '初回表示
                Me.lblTboxId.Text = drwSelect(0)("TBOXID").ToString
                Me.lblHallNm.Text = drwSelect(0)("ホール名").ToString
                Me.lblVer.Text = drwSelect(0)("TBOXVER").ToString
                Me.lblRcvDate.Text = drwSelect(0)("照会日時").ToString
                Me.lblJBNo_From.Text = drwSelect(1)("JB番号_MIN").ToString
                Me.lblJBNo_To.Text = drwSelect(1)("JB番号_MAX").ToString

                '初期値としてJB番号From-Toに最小値を設定
                Me.tftJBNo.ppFromText = drwSelect(1)("JB番号_MIN").ToString
                Me.tftJBNo.ppToText = drwSelect(1)("JB番号_MIN").ToString
                Me.lblRecordCount.Text = "1"

                '検索条件保存
                Me.hdnJB_From.Value = Me.tftJBNo.ppFromText
                Me.hdnJB_To.Value = Me.tftJBNo.ppToText
            Else
                '検索
                lblRecordCount.Text = drwSelect(1)("JB番号_件数").ToString
            End If

            '印刷ボタン活性
            Master.Master.ppRigthButton1.Enabled = True

        End If

    End Sub

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
