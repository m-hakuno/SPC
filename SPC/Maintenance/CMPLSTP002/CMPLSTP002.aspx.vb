'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　検収書確認リスト
'*　ＰＧＭＩＤ：　CMPLSTP002
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.31　：　ＸＸＸ
'********************************************************************************************************************************
'★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★
'★　■2014/03/28 修正ポイント(レポートが出ないことに対する修正) Hamamoto Edit
'★　　1：REPORTCLASS(MNTREP003)のTBOXIDのフィールドが「TBOX ID」とスペースがとられていたので修正。
'★　　2：REPORTCLASS(MNTREP003)のフィールドの合計値の計算の際のデータのINDEXが間違っていたので修正。
'★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'CMPLSTP002-001     2016/10/13      栗原       合計件数・合計金額の表示を追加

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax

#End Region

Public Class CMPLSTP002
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
    Const M_MY_DISP_ID = P_FUN_CMP & P_SCR_LST & P_PAGE & "002"

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
    ''' Page_Initイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        'グリッドXML定義ファイル取得
        pfSet_GridView_S_Off(Me.grvList, M_MY_DISP_ID, "L")

    End Sub

    ''' <summary>
    ''' Page_Loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click
        AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnPrint_Click
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            If Not IsPostBack Then  '初回表示のみ

                '印刷ボタンを活性化
                Master.Master.ppRigthButton1.Visible = True
                Master.Master.ppRigthButton1.Text = P_BTN_NM_PRI

                '検索条件クリアボタンと印刷ボタン押下時の検証を無効
                Master.ppRigthButton2.CausesValidation = False
                Master.Master.ppRigthButton1.CausesValidation = False

                '画面設定
                Master.Master.ppProgramID = M_MY_DISP_ID
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)
                Master.Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる

                '-----------------------------
                '2014/04/25 小守　ここから
                '-----------------------------
                '該当件数非表示
                Master.ppCount_Visible = False
                '-----------------------------
                '2014/04/25 小守　ここまで
                '-----------------------------

                'パンくずリスト設定
                Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                ''セッション変数「グループナンバー」が存在しない場合、画面を閉じる
                'If Session(P_SESSION_GROUP_NUM) Is Nothing Then
                '    psMesBox(Me, "20004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画前)
                '    psClose_Window(Me)
                '    Return
                'End If

                'ViewStateに「グループナンバー」を保存
                ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                'ドロップダウンリスト設定
                msSet_ddlMaker()

                '画面クリア
                msClear_Screen()

                ''データ取得
                'msGet_Data(String.Empty _
                '         , String.Empty _
                '         , String.Empty)
                'Else
                '    'ViewState「グループナンバー」が存在しない場合、画面を閉じる
                '    If ViewState(P_SESSION_GROUP_NUM) Is Nothing Then
                '        psMesBox(Me, "20004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画前)
                '        psClose_Window(Me)
                '        Return
                '    End If
            End If

        Catch ex As Exception
            '画面初期化エラー
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
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

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
            Case "NGC"
        End Select

    End Sub
    '---------------------------
    '2014/04/21 武 ここまで
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
            msGet_Data(Me.ddlMaker.SelectedValue _
                     , Me.dftReqDt.ppFromText _
                     , Me.dftReqDt.ppToText)
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
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)
        '開始ログ出力
        psLogStart(Me)

        '画面クリア
        msClear_Screen()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 印刷ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPrint_Click(sender As Object, e As EventArgs)
        '---------------------------
        '2014/4/11 小守　ここから
        '---------------------------
        Dim objRpt As MNTREP003                     'レポートクラス
        'Dim dtsRepData As DataSet = Nothing         'データセット
        Dim dttData As DataTable = Nothing          'データテーブル

        'ログ出力開始
        psLogStart(Me)

        objRpt = New MNTREP003

        If Master.ppCount = 0 Then
            psMesBox(Me, "00007", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
        Else
            'グリッドからデータ取得
            dttData = pfParse_DataTable(Me.grvList)

            'ファイル出力
            psPrintPDF(Me, objRpt, dttData, "メーカ修理費用")
        End If

        'If mfGet_RepairData(Me.ddlMaker.SelectedValue.ToString _
        '                   , Me.dftReqDt.ppFromText.ToString _
        '                   , Me.dftReqDt.ppToText.ToString _
        '                   , dtsRepData) Then
        '    'ＰＤＦデータ取得処理
        '    dttData = dtsRepData.Tables(0)
        '    objRpt = New MNTREP003

        '    'ファイル出力
        '    psPrintPDF(Me, objRpt, dttData, "メーカ修理費用")
        'End If

        'ログ出力終了
        psLogEnd(Me)
        '---------------------------
        '2014/4/11 小守　ここまで
        '---------------------------
    End Sub

    'CMPLSTP002-001
    ''' <summary>
    ''' 一覧データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        '合計件数・合計金額の計算と表示
        Me.lblTotalSum.Visible = True
        Me.lblTotalAmount.Visible = True
        Dim decAmount As Decimal = 0
        For zz As Integer = 0 To grvList.Rows.Count - 1
            Dim row As GridViewRow = grvList.Rows(zz)
            'Dim strMny As String = DirectCast(row.FindControl("合計"), TextBox).Text
            Dim strMny As String = DirectCast(row.FindControl("合計"), Label).Text
            If strMny <> String.Empty Then
                decAmount = decAmount + Decimal.Parse(strMny)
            End If
        Next
        Me.lblTotalSum.Text = "合計件数： " + grvList.Rows.Count.ToString("#,##0")
        Me.lblTotalAmount.Text = "合計金額： " + decAmount.ToString("#,##0")

    End Sub
    'CMPLSTP002-001 END

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClear_Screen()
        Me.ddlMaker.SelectedValue = String.Empty         'メーカ
        Me.dftReqDt.ppFromText = String.Empty           '請求年月
        Me.dftReqDt.ppToText = String.Empty
        Me.grvList.DataSource = New DataTable
        Master.ppCount = "0"
        Me.grvList.DataBind()
        Me.ddlMaker.Focus()

        'CMPLSTP002-001
        Me.lblTotalAmount.Text = String.Empty
        Me.lblTotalSum.Text = String.Empty
        Me.lblTotalAmount.Visible = False
        Me.lblTotalSum.Visible = False
        'CMPLSTP002-001 END

    End Sub

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <param name="ipstrMkrCd">メーカ区分</param>
    ''' <param name="ipstrReqDtF">請求年月From</param>
    ''' <param name="ipstrReqDtT">請求年月To</param>
    ''' <remarks></remarks>
    Private Sub msGet_Data(ByVal ipstrMkrCd As String _
                         , ByVal ipstrReqDtF As String _
                         , ByVal ipstrReqDtT As String _
                         )
        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_S1", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("mkr_cd", SqlDbType.NVarChar, ipstrMkrCd))         'メーカ
                    .Add(pfSet_Param("req_dt_f", SqlDbType.NVarChar, ipstrReqDtF))      '請求年月From
                    .Add(pfSet_Param("req_dt_t", SqlDbType.NVarChar, ipstrReqDtT))      '請求年月To
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ有無確認
                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                End If

                '件数を設定
                Master.ppCount = dstOrders.Tables(0).Rows.Count.ToString

                '取得したデータをグリッドに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "検収書確認リスト")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
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
    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（業者マスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_ddlMaker()
        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("ZCMPSEL016", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("trader_cd", SqlDbType.NVarChar, "5"))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'ドロップダウンリスト設定
                Me.ddlMaker.Items.Clear()
                Me.ddlMaker.DataSource = dstOrders.Tables(0)
                Me.ddlMaker.DataTextField = "会社"
                Me.ddlMaker.DataValueField = "会社コード"
                Me.ddlMaker.DataBind()
                Me.ddlMaker.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "メーカ区分一覧")
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
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If
    End Sub

    '---------------------------
    '2014/4/11 小守　ここから
    '---------------------------
    ' ''' <summary>
    ' ''' メーカ修理費用取得処理
    ' ''' </summary>
    ' ''' <param name="ipstrMkrCd">メーカ区分</param>
    ' ''' <param name="ipstrReqDtF">請求年月From</param>
    ' ''' <param name="ipstrReqDtT">請求年月To</param>
    ' ''' <param name="opdstData">貸玉数　設定情報項目</param>
    ' ''' <returns>整合性OK：True, NG:False</returns>
    ' ''' <remarks></remarks>
    'Private Function mfGet_RepairData(ByVal ipstrMkrCd As String _
    '                     , ByVal ipstrReqDtF As String _
    '                     , ByVal ipstrReqDtT As String _
    '                     , ByRef opdstData As DataSet) As Boolean
    '    Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
    '    Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
    ''--------------------------------
    ''2014/04/14 星野　ここから
    ''--------------------------------
    '    objStack = New StackFrame
    ''--------------------------------
    ''2014/04/14 星野　ここまで
    ''--------------------------------

    '    mfGet_RepairData = False

    '    'DB接続
    '    If pfOpen_Database(conDB) Then
    '        Try
    '            cmdDB = New SqlCommand(M_MY_DISP_ID + "_S2", conDB)
    '            'パラメータ設定
    '            With cmdDB.Parameters
    '                .Add(pfSet_Param("mkr_cd", SqlDbType.NVarChar, ipstrMkrCd))         'メーカ
    '                .Add(pfSet_Param("req_dt_f", SqlDbType.NVarChar, ipstrReqDtF))      '請求年月From
    '                .Add(pfSet_Param("req_dt_t", SqlDbType.NVarChar, ipstrReqDtT))      '請求年月To
    '            End With

    '            'データ取得
    '            opdstData = pfGet_DataSet(cmdDB)

    '            'データ有無確認
    '            If opdstData.Tables(0).Rows.Count = 0 Then
    '                psMesBox(Me, "00004", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後, "貸玉数　設定情報")
    '                Exit Function
    '            End If

    '            mfGet_RepairData = True

    '        Catch ex As Exception
    '            'データ取得エラー
    '            psMesBox(Me, "00004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "貸玉数　設定情報取得")
    ''--------------------------------
    ''2014/04/14 星野　ここから
    ''--------------------------------
    ''ログ出力
    '            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
    '                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
    ''--------------------------------
    ''2014/04/14 星野　ここまで
    ''--------------------------------
    '        Finally
    '            'DB切断
    '            If Not pfClose_Database(conDB) Then
    '                psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
    '            End If
    '        End Try
    '    Else
    '        psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
    '    End If
    'End Function
    '---------------------------
    '2014/4/11 小守　ここまで
    '---------------------------

#End Region

End Class
