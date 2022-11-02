'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　修理・有償部品費用作成
'*　ＰＧＭＩＤ：　CMPUPDP002
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.20　：　酒井
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'CMPUPDP002-001     2016/10/14      栗原　　　明細表示領域の拡張

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================

Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax

#End Region

Public Class CMPUPDP002
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
    Const sCnsErr_00004 As String = "00004"
    Const sCnsErr_00005 As String = "00005"
    Const sCnsErr_00006 As String = "00006"
    Const sCnsErr_00007 As String = "00007"
    Const sCnsErr_10001 As String = "10001"
    Const sCnsErr_10002 As String = "10002"
    Const sCnsErr_20004 As String = "20004"
    Const sCnsErr_30008 As String = "30008"

    ''品質会議資料 画面のパス
    'Const sCnsCMPUPDP002 As String = "~/" & P_CNS & "/" &
    '                                    P_FUN_QUA & P_SCR_OUT & P_PAGE & "001.aspx"

    Const sCnsProgid As String = "CMPUPDP002"
    Const sCnsSqlid_S1 As String = "CMPUPDP002_S1"
    Const sCnsSqlid_S2 As String = "CMPUPDP002_S2"
    Const sCnsSqlid_S3 As String = "CMPUPDP002_S3"
    Const sCnsCsvButon As String = "ＣＳＶ"
    Const sCnsPdfButon As String = "印刷"

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

#Region "ページ初期処理"
    ''' <summary>
    ''' ページ初期処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '表設定
        pfSet_GridView_S_Off(grvList, sCnsProgid)

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
            'ボタンアクションの設定
            AddHandler Master.ppRigthButton1.Click, AddressOf Button_Click
            AddHandler Master.ppRigthButton2.Click, AddressOf Button_Click

            AddHandler Master.Master.ppRigthButton1.Click, AddressOf Button_Click   'ＣＳＶ
            AddHandler Master.Master.ppRigthButton2.Click, AddressOf Button_Click   'ＰＤＦ

            '-----------------------------
            '2014/04/25 小守　ここから
            '-----------------------------
            If Not IsPostBack Then  '初回表示
                'ボタン押下時の検証を無効
                Master.ppRigthButton2.CausesValidation = False
                Master.Master.ppRigthButton1.CausesValidation = False
                Master.Master.ppRigthButton2.CausesValidation = False

                'ボタン表示設定
                Master.Master.ppRigthButton1.Text = sCnsPdfButon
                Master.Master.ppRigthButton2.Text = sCnsCsvButon
                'ボタン非活性
                Master.Master.ppRigthButton1.Visible = True
                Master.Master.ppRigthButton2.Visible = True
                Master.Master.ppRigthButton1.Enabled = False
                Master.Master.ppRigthButton2.Enabled = False

                '該当件数非表示
                Master.ppCount_Visible = False

                'プログラムＩＤ、画面名設定
                Master.Master.ppProgramID = sCnsProgid
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(sCnsProgid)
                'Master.ppCount = "0"

                'パンくずリスト設定
                Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                'セッション変数「グループナンバー」が存在しない場合、画面を閉じる
                If Session(P_SESSION_GROUP_NUM) Is Nothing Then
                    psMesBox(Me, sCnsErr_20004, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    psClose_Window(Me)
                    Return
                End If

                'ViewStateに「グループナンバー」を保存
                ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                'ボタン押下時のメッセージ設定
                'Master.Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes(sCnsErr_10002, clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "修理・有償部品費用")
                Master.Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes(sCnsErr_10001, ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "修理・有償部品費用")

                'ＴＢＯＸタイプドロップダウンリスト設定
                ms_GetTboxType()

                '機器名ドロップダウンリスト設定
                ms_GetAppa()

                '画面の初期化
                msClear_SearchArea()
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()

            Else
                'ViewState「グループナンバー」が存在しない場合、画面を閉じる
                If ViewState(P_SESSION_GROUP_NUM) Is Nothing Then
                    psMesBox(Me, sCnsErr_20004, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
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
        '-----------------------------
        '2014/04/25 小守　ここまで
        '-----------------------------
    End Sub

#End Region

#Region "ユーザー定義"
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
#End Region

#Region "ボタン押下処理"

    ''' <summary>
    ''' ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Button_Click(sender As System.Object, e As System.EventArgs)
        'ボタン押下判定
        Select Case sender.text.ToString()
            Case "検索"
                If (Page.IsValid) Then
                    msGet_Data()                            '条件検索取得
                End If
            Case "検索条件クリア"
                msClear_SearchArea()
            Case sCnsCsvButon
                msCsv()                                     'ＣＳＶ作成処理
            Case sCnsPdfButon
                msPrint()                                   '印刷処理
        End Select

    End Sub

#End Region

#End Region

#Region "そのほかのプロシージャ"

#Region "検索条件クリア処理"

    ''' <summary>
    ''' 検索条件クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClear_SearchArea()

        Me.dttReq_Dt.ppText = String.Empty
        Me.tftTboxId.ppFromText = String.Empty
        Me.tftTboxId.ppToText = String.Empty
        Me.ddlTboxClass.SelectedValue = String.Empty
        Me.ddlAppaClass.SelectedValue = String.Empty

        Me.dttReq_Dt.ppDateBox.Focus()

        Me.lblTotalAmount.Text = String.Empty
        Me.lblTotalSum.Text = String.Empty
        Me.lblTotalAmount.Visible = False
        Me.lblTotalSum.Visible = False

    End Sub

#End Region

#Region "条件検索取得処理"

    ''' <summary>
    ''' 条件検索取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data()
        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'ＤＢ接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(sCnsSqlid_S1, conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    '請求年月
                    .Add(pfSet_Param("req_dt", SqlDbType.NVarChar, Me.dttReq_Dt.ppText))
                    'ＴＢＯＸＩＤFrom
                    If Me.tftTboxId.ppToText = String.Empty Then
                        'ＴＢＯＸＩＤToが空白の場合は「あいまい検索」なのでエスケープする
                        .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar _
                                         , Me.tftTboxId.ppFromText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    Else
                        'ＴＢＯＸＩＤToが空白でないの場合は「範囲検索」なのでエスケープしない
                        .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, Me.tftTboxId.ppFromText))
                    End If
                    'ＴＢＯＸＩＤTo
                    .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, Me.tftTboxId.ppToText))
                    'ＴＢＯＸタイプ
                    .Add(pfSet_Param("tboxcls", SqlDbType.NVarChar, Me.ddlTboxClass.SelectedValue))
                    '機器名
                    .Add(pfSet_Param("appa_cls", SqlDbType.NVarChar, Me.ddlAppaClass.SelectedValue))
                End With

                'データ取得およびデータをリストに設定
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                Me.grvList.DataSource = dstOrders

                '件数を設定
                Master.ppCount = Me.grvList.Controls.Count.ToString
                If dstOrders.Tables(0).Rows.Count = 0 Then
                    'ボタン非活性
                    Master.Master.ppRigthButton1.Enabled = False
                    Master.Master.ppRigthButton2.Enabled = False
                    '0件
                    psMesBox(Me, sCnsErr_00004, ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                Else
                    'ボタン活性
                    Master.Master.ppRigthButton1.Enabled = True
                    Master.Master.ppRigthButton2.Enabled = True
                End If

                '件数を設定
                Master.ppCount = dstOrders.Tables(0).Rows.Count.ToString

                '変更を反映
                Me.grvList.DataBind()

                Me.lblTotalSum.Visible = True
                Me.lblTotalAmount.Visible = True
                Dim decAmount As Decimal = 0
                For zz As Integer = 0 To dstOrders.Tables(0).Rows.Count - 1
                    If dstOrders.Tables(0).Rows(zz).Item("修理費合計").ToString <> String.Empty Then
                        decAmount = decAmount + Decimal.Parse(dstOrders.Tables(0).Rows(zz).Item("修理費合計"))
                    End If
                Next
                Me.lblTotalSum.Text = "合計件数： " + dstOrders.Tables(0).Rows.Count.ToString("#,##0")
                Me.lblTotalAmount.Text = "合計金額： " + decAmount.ToString("#,##0")

                'ビューに退避
                ViewState("ds") = dstOrders

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, sCnsErr_00004, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "修理・有償部品費用")
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
                    psMesBox(Me, sCnsErr_00006, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, sCnsErr_00005, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"
        End If
    End Sub

#End Region


#Region "ＣＳＶ作成処理"

    ''' <summary>
    ''' ＣＳＶ作成処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub msCsv()
        '---------------------------
        '2014/4/21 小守　ここから
        '---------------------------
        Dim intRtn As Integer               '戻り値
        Dim dttData As DataTable            'ＣＳＶデータ

        '開始ログ出力
        psLogStart(Me)

        Try

            'ＣＳＶデータ取得処理
            dttData = pfParse_DataTable(Me.grvList)

            '不必要列削除
            dttData.Columns.Remove("日時")
            dttData.Columns.Remove("会社略称")

            'ファイル出力
            intRtn = pfDLCSV("修理・有償部品費用", "", dttData, True, Me)

            'ファイル出力エラー
            If intRtn <> 0 Then
                psMesBox(Me, sCnsErr_10001, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
                Exit Sub
            End If

            'If Master.ppCount = 0 Then
            '    psMesBox(Me, sCnsErr_00005, clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後)
            'Else
            '    'グリッドから取得
            '    dt = pfParse_DataTable(Me.grvList)

            '    'CSVファイルダウンロード
            '    If pfDLCsvFile(sCnsProgid + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv",
            '                   dt, True, Me) <> 0 Then
            '        psMesBox(Me, sCnsErr_10001, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "ＣＳＶファイル")
            '    End If
            'End If

            '終了ログ出力
            psLogEnd(Me)

            '---------------------------
            '2014/4/21 小守　ここまで
            '---------------------------
        Catch ex As Threading.ThreadAbortException
            '終了ログ出力
            psLogEnd(Me)

        Catch ex As Exception
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

    End Sub

#End Region

#Region "印刷処理"

    ''' <summary>
    ''' 印刷処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub msPrint()
        '---------------------------
        '2014/4/11 小守　ここから
        '---------------------------
        Dim objRpt As REPREP003                     'レポートクラス
        'Dim dtsRepData As DataSet = Nothing         'データセット
        Dim dttData As DataTable = Nothing          'データテーブル
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'ログ出力開始
        psLogStart(Me)

        objRpt = New REPREP003

        If Master.ppCount = 0 Then
            psMesBox(Me, sCnsErr_00007, ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
        Else
            'グリッドからデータ取得
            dttData = pfParse_DataTable(Me.grvList)

            '印刷処理
            psPrintPDF(Me, objRpt, dttData, "修理・有償部品費用")
        End If

        'Try
        '    If mfGet_PrintData(dtsRepData) Then
        '        'ＰＤＦデータ取得処理
        '        dttData = dtsRepData.Tables(0)
        '        objRpt = New REPREP003

        '        'ＰＤＦ出力
        '        psPrintPDF(Me, objRpt, dttData, "修理・有償部品費用")

        '    End If

        'Catch ex As Exception
        '    '印刷エラー
        '    psMesBox(Me, "10002", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "修理・有償部品費用")
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

        'ログ出力終了
        psLogEnd(Me)
        '---------------------------
        '2014/4/11 小守　ここまで
        '---------------------------
    End Sub

#End Region

#Region "印刷データ取得処理"

    '---------------------------
    '2014/4/11 小守　ここから
    '---------------------------
    ' ''' <summary>
    ' ''' 印刷データ取得処理
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Function mfGet_PrintData(ByRef opdstData As DataSet) As Boolean
    '    ' ＤＢ接続変数
    '    Dim conDB As New SqlConnection
    '    Dim cmdDB As New SqlCommand
    '    Dim dstOrders As New DataSet
    ''--------------------------------
    ''2014/04/14 星野　ここから
    ''--------------------------------
    '    objStack = New StackFrame
    ''--------------------------------
    ''2014/04/14 星野　ここまで
    ''--------------------------------

    '    mfGet_PrintData = False

    '    Try
    '        'ＤＢ接続
    '        If Not pfOpen_Database(conDB) Then
    '            Throw New Exception("")
    '        End If

    '        'パラメータ設定
    '        cmdDB = New SqlCommand(sCnsSqlid_S4, conDB)

    '        With cmdDB.Parameters
    '            '請求年月
    '            .Add(pfSet_Param("req_dt", SqlDbType.NVarChar, Me.dttReq_Dt.ppText))
    '            'ＴＢＯＸＩＤFrom
    '            If Me.tftTboxId.ppToText = String.Empty Then
    '                'ＴＢＯＸＩＤToが空白の場合は「あいまい検索」なのでエスケープする
    '                .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar _
    '                                 , Me.tftTboxId.ppFromText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
    '            Else
    '                'ＴＢＯＸＩＤToが空白でないの場合は「範囲検索」なのでエスケープしない
    '                .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, Me.tftTboxId.ppFromText))
    '            End If
    '            'ＴＢＯＸＩＤTo
    '            .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, Me.tftTboxId.ppToText))
    '            'ＴＢＯＸタイプ
    '            .Add(pfSet_Param("tboxcls", SqlDbType.NVarChar, Me.ddlTboxClass.SelectedValue))
    '            '機器名
    '            .Add(pfSet_Param("appa_cls", SqlDbType.NVarChar, Me.ddlAppaClass.SelectedValue))
    '        End With

    '        'データ取得およびデータをリストに設定
    '        dstOrders = pfGet_DataSet(cmdDB)

    '        '件数を設定
    '        If dstOrders.Tables(0).Rows.Count = 0 Then
    '            '0件
    '            psMesBox(Me, "00004", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後)
    '            Exit Function
    '        Else
    '            opdstData = dstOrders
    '        End If

    '    Catch ex As DBConcurrencyException
    '        psMesBox(Me, "0005", clscomver.E_Mタイプ.エラー, clscomver.E_Mモード.OK, clscomver.E_S実行.描画後)
    ''--------------------------------
    ''2014/04/14 星野　ここから
    ''--------------------------------
    ''ログ出力
    '            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
    '                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
    ''--------------------------------
    ''2014/04/14 星野　ここまで
    ''--------------------------------
    '    Catch ex As Exception
    '        psMesBox(Me, "0003", clscomver.E_Mタイプ.エラー, clscomver.E_Mモード.OK, clscomver.E_S実行.描画後)
    ''--------------------------------
    ''2014/04/14 星野　ここから
    ''--------------------------------
    ''ログ出力
    '            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
    '                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
    ''--------------------------------
    ''2014/04/14 星野　ここまで
    ''--------------------------------
    '        Me.grvList.DataBind()
    '        Throw
    '    Finally
    '        'DB切断
    '        pfClose_Database(conDB)
    '        mfGet_PrintData = True
    '    End Try

    'End Function
    '---------------------------
    '2014/4/11 小守　ここまで
    '---------------------------

#End Region

#Region "ＴＢＯＸタイプコンボボックス設定"
    ''' <summary>
    ''' ＴＢＯＸタイプの取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_GetTboxType()
        ' ＤＢ接続変数
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(sCnsSqlid_S2, conDB)

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得データをバインド
                Me.ddlTboxClass.DataSource = dstOrders.Tables(0)

                Me.ddlTboxClass.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
                Me.ddlTboxClass.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
                Me.ddlTboxClass.DataBind()

                '一行目を設定
                Me.ddlTboxClass.Items.Insert(0, "")
                Me.ddlTboxClass.SelectedIndex = 0

            Catch ex As SqlException
                '検索失敗
                psMesBox(Me, sCnsErr_00004, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸタイプ")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                '結果を返す
                Throw ex

            Catch ex As Exception
                'システムエラー
                psMesBox(Me, sCnsErr_00004, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸタイプ")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                '結果を返す
                Throw ex

            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, sCnsErr_00006, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try
        Else
            psMesBox(Me, sCnsErr_00005, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
        End If

    End Sub

#End Region

#Region "機器コンボボックス設定"
    ''' <summary>
    ''' 機器名の取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_GetAppa()
        ' ＤＢ接続変数
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(sCnsSqlid_S3, conDB)

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得データをバインド
                Me.ddlAppaClass.DataSource = dstOrders.Tables(0)

                Me.ddlAppaClass.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
                Me.ddlAppaClass.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
                Me.ddlAppaClass.DataBind()

                '一行目を設定
                Me.ddlAppaClass.Items.Insert(0, "")
                Me.ddlAppaClass.SelectedIndex = 0

            Catch ex As SqlException
                '検索失敗
                psMesBox(Me, sCnsErr_00004, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器名")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                '結果を返す
                Throw ex

            Catch ex As Exception
                'システムエラー
                psMesBox(Me, sCnsErr_00004, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器名")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                '結果を返す
                Throw ex

            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, sCnsErr_00006, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try
        Else
            psMesBox(Me, sCnsErr_00005, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)

        End If

    End Sub
#End Region

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
