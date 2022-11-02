'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜サポートセンタ業務＞
'*　処理名　　：　随時集信一覧状況入力
'*　ＰＧＭＩＤ：　TBPINPP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.17　：　ＮＫＣ
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive
'Imports SPC.Global_asax

#End Region

Public Class TBPINPP001
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
    Const M_MY_DISP_ID = P_FUN_TBP & P_SCR_INP & P_PAGE & "001"
    '次画面ファイルパス（即時集信一覧）
    Const M_NEXT_DISP_PATH = "~/" & P_CNS & "/" &
            P_FUN_MST & P_SCR_LST & P_PAGE & "001" & "/" &
            P_FUN_MST & P_SCR_LST & P_PAGE & "001.aspx"
    Const M_MY_PGNAME = "随時集信一覧"
    Const M_MST_REQUEST = "マスタ廃止・停止依頼書"

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

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' ページ初期処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, M_MY_DISP_ID)

    End Sub

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
        AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnPrint_Click
        AddHandler Master.Master.ppRigthButton2.Click, AddressOf btnCsv_Click

        'テキストボックスアクションの設定
        AddHandler Me.txtTboxidDtl.ppTextBox.TextChanged, AddressOf txtTboxidM_TextChanged
        Me.txtTboxidDtl.ppTextBox.AutoPostBack = True

        If Not IsPostBack Then  '初回表示のみ

            '★排他情報用のグループ番号保管
            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            '「検索条件クリア」ボタン押下時の検証を無効
            Master.ppRigthButton2.CausesValidation = False

            'プログラムID、画面名設定
            Master.Master.ppProgramID = M_MY_DISP_ID
            Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)

            'パンくずリスト設定
            Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

            '権限によって変更(仮決め)
            Select Case Session(P_SESSION_TERMS)

                Case ClsComVer.E_遷移条件.更新

                    '遷移情報を取得
                    ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新

                Case ClsComVer.E_遷移条件.参照

                    '遷移情報を取得
                    ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照

            End Select

            '各コマンドボタンの属性設定
            '--追加
            Me.btnDetailInsert.OnClientClick =
                pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, M_MY_PGNAME + "情報")
            '--変更
            Me.btnDetailUpdate.OnClientClick =
                pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, M_MY_PGNAME + "情報")
            '--削除
            Me.btnDetailDelete.OnClientClick =
                pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, M_MY_PGNAME + "情報")
            '--マスタ廃止
            Me.btnDetailPrint.OnClientClick =
                pfGet_OCClickMes("10002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, M_MST_REQUEST)
            '--印刷
            Master.Master.ppRigthButton1.Text = P_BTN_NM_PRI
            Master.Master.ppRigthButton1.Visible = True
            Master.Master.ppRigthButton1.OnClientClick =
                pfGet_OCClickMes("10002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, M_MY_PGNAME)
            '--ＣＳＶ
            Master.Master.ppRigthButton2.Text = "ＣＳＶ"
            Master.Master.ppRigthButton2.Visible = True

            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            'ドロップダウンリスト設定
            msSetddlSystem()        'システム
            msSetddlUnclctReason()  '未集信理由（Ｍ８５）

            '画面クリア
            msClearScreen()

        End If

    End Sub

    '---------------------------
    '2014/04/23 武 ここから
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
                Me.btnDetailInsert.Enabled = False
                Me.btnDetailUpdate.Enabled = False
                Me.btnDetailDelete.Enabled = False
                Me.btnDetailCncnt.Enabled = False
                '---------------------------
                '2014/06/18 武 ここから
                '---------------------------
                Me.txtTboxidDtl.ppEnabled = False
                Me.dtbLastBDtDtl.ppEnabled = False
                Me.dtbLastSDtDtl.ppEnabled = False
                Me.ddlRemoveClsDtl.ppEnabled = False
                Me.ddlUnclctReasonDtl.Enabled = False
                Me.dtbArtclArvDtDtl.ppEnabled = False
                Me.dtbSuckDtDtl.ppEnabled = False
                Me.dtbArtclRetDtDtl.ppEnabled = False
                Me.dtbMstRepFSDtDtl.ppEnabled = False
                Me.dtbMstRepNGCDtDtl.ppEnabled = False
                Me.txtNoteTextDtl.ppEnabled = False
                '---------------------------
                '2014/06/18 武 ここまで
                '---------------------------
                Me.btnDetailPrint.Visible = False
        End Select

    End Sub

    '---------------------------
    '2014/04/23 武 ここまで
    '---------------------------

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

        '整合性チェック
        msCheckSearchCondition()

        'データ取得
        If (Page.IsValid) Then
            '明細クリア
            msClearDetail()

            'データ取得処理
            objDs = mfGetData()

            If Not objDs Is Nothing Then
                '画面表示
                msDispData(objDs)

                'TBOXIDへフォーカス
                Me.txtTboxidDtl.ppTextBox.Focus()
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
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        '画面クリア
        msClearSearchCondition()

        '工事日Fromへフォーカス
        Me.dtbCnstDt.ppDateBoxFrom.Focus()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 印刷ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPrint_Click(sender As Object, e As EventArgs)

        Dim objDs As DataSet = Nothing  'データセット
        Dim rpt As Object = Nothing     'ActiveReports用クラス

        '開始ログ出力
        psLogStart(Me)

        'データ取得
        If (Page.IsValid) Then
            '明細クリア
            msClearDetail()

            'データ取得処理
            objDs = mfGetData()

            If Not objDs Is Nothing Then
                '帳票出力
                rpt = New REQREP003
                psPrintPDF(Me, rpt, objDs.Tables(0), M_MY_PGNAME)
            End If
        End If

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

        Dim objDs As DataSet = Nothing  'データセット

        '開始ログ出力
        psLogStart(Me)

        '整合性チェック
        msCheckSearchCondition()

        'データ取得
        If (Page.IsValid) Then
            '明細クリア
            msClearDetail()

            'データ取得処理
            objDs = mfGetData()

            If Not objDs Is Nothing Then
                'CSVファイル出力
                msCsvFileOutput(objDs)
            End If
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 追加ボタン（明細）クリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDetailInsert_Click(sender As Object, e As EventArgs) Handles btnDetailInsert.Click

        Dim objDs As DataSet = Nothing  'データセット

        '開始ログ出力
        psLogStart(Me)

        '整合性チェック
        msCheckUpdateDetail()

        '入力内容検証
        If (Page.IsValid) Then
            '明細追加処理
            If mfUpdateDataDetail(1) Then
                '明細クリア
                msClearDetail()

                'データ取得処理
                objDs = mfGetData()

                If Not objDs Is Nothing Then
                    'データ表示
                    msDispData(objDs)

                    'TBOXIDへフォーカス
                    Me.txtTboxidDtl.ppTextBox.Focus()
                End If
            End If
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 変更ボタン（明細）クリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDetailUpdate_Click(sender As Object, e As EventArgs) Handles btnDetailUpdate.Click

        Dim objDs As DataSet = Nothing  'データセット

        '開始ログ出力
        psLogStart(Me)

        '整合性チェック
        msCheckUpdateDetail()

        '入力内容検証
        If (Page.IsValid) Then
            '明細更新処理
            If mfUpdateDataDetail(2) Then
                '明細クリア
                msClearDetail()

                'データ取得処理
                objDs = mfGetData()

                If Not objDs Is Nothing Then
                    'データ表示
                    msDispData(objDs)

                    'TBOXIDへフォーカス
                    Me.txtTboxidDtl.ppTextBox.Focus()
                End If
            End If
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 削除ボタン（明細）クリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDetailDelete_Click(sender As Object, e As EventArgs) Handles btnDetailDelete.Click

        Dim objDs As DataSet = Nothing  'データセット

        '開始ログ出力
        psLogStart(Me)

        '明細削除処理
        If mfUpdateDataDetail(3) Then
            '明細クリア
            msClearDetail()

            'データ取得処理
            objDs = mfGetData()

            If Not objDs Is Nothing Then
                'データ表示
                msDispData(objDs)

                'TBOXIDへフォーカス
                Me.txtTboxidDtl.ppTextBox.Focus()
            End If
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 即時集信ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDetailCncnt_Click(sender As Object, e As EventArgs) Handles btnDetailCncnt.Click

        Dim strKeyList As List(Of String) = Nothing '次画面引継ぎ用キー情報

        '開始ログ出力
        psLogStart(Me)

        'TBOXIDの整合チェック
        If Not msGetTboxInfo() Then
            Me.txtTboxidDtl.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
            Me.txtTboxidDtl.ppTextBox.Focus()
        End If

        If (Page.IsValid) Then
            '次画面引継ぎ用キー情報設定
            strKeyList = New List(Of String)
            strKeyList.Add(Me.txtTboxidDtl.ppText)

            'セッション情報設定																	
            Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
            Session(P_SESSION_OLDDISP) = M_MY_DISP_ID
            Session(P_KEY) = strKeyList.ToArray
            Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

            '--------------------------------
            '2014/05/30 後藤　ここから
            '--------------------------------
            ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
            '--------------------------------
            '2014/05/30 後藤　ここまで
            '--------------------------------
            '画面権限情報
            Select Case ViewState(P_SESSION_TERMS)

                Case ClsComVer.E_遷移条件.更新

                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新


                Case ClsComVer.E_遷移条件.参照

                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照

            End Select

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
                            objStack.GetMethod.Name, M_NEXT_DISP_PATH, strPrm, "TRANS")
            '■□■□結合試験時のみ使用予定□■□■
            '--------------------------------
            '2014/04/16 星野　ここまで
            '--------------------------------
            '即時集信一覧画面起動
            psOpen_Window(Me, M_NEXT_DISP_PATH)
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' マスタ廃止ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDetailPrint_Click(sender As Object, e As EventArgs) Handles btnDetailPrint.Click

        Dim objDs As DataSet = Nothing  'データセット
        Dim rpt As Object = Nothing     'ActiveReports用クラス

        '開始ログ出力
        psLogStart(Me)

        'データ取得処理
        objDs = mfGetDataMstReq()

        If Not objDs Is Nothing Then
            '帳票出力
            rpt = New MSTREP002
            psPrintPDF(Me, rpt, objDs.Tables(0), M_MST_REQUEST)
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' グリッド選択ボタン押下時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

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


            '--------------------------------
            '2014/06/19 星野　ここから
            '--------------------------------
            'Select Case Session(P_SESSION_AUTH)
            '    Case "管理者", "SPC", "NGC"

            '        '★ロック対象テーブル名の登録
            '        arTable_Name.Insert(0, "D84_ANYTIME_LIST")

            '        '★ロックテーブルキー項目の登録(D84_ANYTIME_LIST)
            '        arKey.Insert(0, CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text) 'D84_TBOXID
            '        arKey.Insert(1, CType(rowData.FindControl("工事日"), TextBox).Text)       'D84_CNST_DT

            '        '★排他情報確認処理(更新処理の実行)
            '        If clsExc.pfSel_Exclusive(strExclusiveDate _
            '                         , Me _
            '                         , Session(P_SESSION_IP) _
            '                         , Session(P_SESSION_PLACE) _
            '                         , Session(P_SESSION_USERID) _
            '                         , Session(P_SESSION_SESSTION_ID) _
            '                         , ViewState(P_SESSION_GROUP_NUM) _
            '                         , M_MY_DISP_ID _
            '                         , arTable_Name _
            '                         , arKey) = 0 Then

            '            Me.Master.Master.ppExclusiveDateDtl = strExclusiveDate

            '            '明細の各項目に対してデータを設定
            '            Me.lblCnstDtDtl.Text = CType(rowData.FindControl("工事日"), TextBox).Text
            '            Me.txtTboxidDtl.ppText = CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text
            '            Me.lblHallNmDtl.Text = CType(rowData.FindControl("ホール名"), TextBox).Text
            '            Me.lblSystemDtl.Text = CType(rowData.FindControl("システム"), TextBox).Text
            '            Me.lblCnstNoDtl.Text = CType(rowData.FindControl("工事依頼番号"), TextBox).Text
            '            Me.dtbLastBDtDtl.ppText = CType(rowData.FindControl("最終営業日"), TextBox).Text
            '            Me.dtbLastSDtDtl.ppText = CType(rowData.FindControl("最終集信日"), TextBox).Text
            '            Me.ddlRemoveClsDtl.ppSelectedValue = CType(rowData.FindControl("撤去区分"), TextBox).Text
            '            Me.lblMishushinDtl.Text = CType(rowData.FindControl("未集信"), TextBox).Text
            '            Me.ddlUnclctReasonDtl.SelectedValue = CType(rowData.FindControl("未集信理由コード"), TextBox).Text
            '            Me.dtbArtclArvDtDtl.ppText = CType(rowData.FindControl("物品到着日"), TextBox).Text
            '            Me.dtbSuckDtDtl.ppText = CType(rowData.FindControl("吸上げ処理日"), TextBox).Text
            '            Me.dtbArtclRetDtDtl.ppText = CType(rowData.FindControl("物品返却日"), TextBox).Text
            '            Me.dtbMstRepFSDtDtl.ppText = CType(rowData.FindControl("マスタ廃止日（ＦＳ）"), TextBox).Text
            '            Me.dtbMstRepNGCDtDtl.ppText = CType(rowData.FindControl("マスタ廃止日（ＮＧＣ）"), TextBox).Text
            '            Me.txtNoteTextDtl.ppText = CType(rowData.FindControl("備考"), TextBox).Text

            '            '各コントロール活性・非活性
            '            Me.txtTboxidDtl.ppEnabled = False
            '            If Me.lblMishushinDtl.Text = "あり" Then
            '                '未集信ありの場合、物品到着日及び吸上げ処理日を活性
            '                Me.dtbArtclArvDtDtl.ppEnabled = True
            '                Me.dtbSuckDtDtl.ppEnabled = True
            '            Else
            '                '未集信なしの場合、物品到着日及び吸上げ処理日を非活性
            '                Me.dtbArtclArvDtDtl.ppEnabled = False
            '                Me.dtbSuckDtDtl.ppEnabled = False
            '            End If
            '            Me.ddlRemoveClsDtl.ppEnabled = False '撤去区分
            '            Me.btnDetailInsert.Enabled = False
            '            Me.btnDetailUpdate.Enabled = True
            '            If Me.lblCnstNoDtl.Text = String.Empty Then
            '                '工事依頼番号がない場合は削除ボタンを活性
            '                Me.btnDetailDelete.Enabled = True
            '            Else
            '                '工事依頼番号がある場合は削除ボタンを非活性
            '                Me.btnDetailDelete.Enabled = False
            '            End If
            '            Me.btnDetailPrint.Enabled = True

            '        Else

            '            '排他ロック中

            '        End If
            'End Select

            Dim intExFlg As Integer = 0     '排他判断用フラグ

            Select Case Session(P_SESSION_AUTH)
                Case "管理者", "SPC"

                    '★ロック対象テーブル名の登録
                    arTable_Name.Insert(0, "D84_ANYTIME_LIST")

                    '★ロックテーブルキー項目の登録(D84_ANYTIME_LIST)
                    arKey.Insert(0, CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text) 'D84_TBOXID
                    arKey.Insert(1, CType(rowData.FindControl("工事日"), TextBox).Text)       'D84_CNST_DT

                    '★排他情報確認処理(更新処理の実行)
                    If clsExc.pfSel_Exclusive(strExclusiveDate _
                                     , Me _
                                     , Session(P_SESSION_IP) _
                                     , Session(P_SESSION_PLACE) _
                                     , Session(P_SESSION_USERID) _
                                     , Session(P_SESSION_SESSTION_ID) _
                                     , ViewState(P_SESSION_GROUP_NUM) _
                                     , M_MY_DISP_ID _
                                     , arTable_Name _
                                     , arKey) = 0 Then

                        Me.Master.Master.ppExclusiveDateDtl = strExclusiveDate

                    Else
                        '排他ロック中
                        intExFlg = 1
                    End If
            End Select

            If intExFlg = 0 Then

                '明細の各項目に対してデータを設定
                Me.lblCnstDtDtl.Text = CType(rowData.FindControl("工事日"), TextBox).Text
                Me.txtTboxidDtl.ppText = CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text
                Me.lblHallNmDtl.Text = CType(rowData.FindControl("ホール名"), TextBox).Text
                Me.lblSystemDtl.Text = CType(rowData.FindControl("システム"), TextBox).Text
                Me.lblCnstNoDtl.Text = CType(rowData.FindControl("工事依頼番号"), TextBox).Text
                Me.dtbLastBDtDtl.ppText = CType(rowData.FindControl("最終営業日"), TextBox).Text
                Me.dtbLastSDtDtl.ppText = CType(rowData.FindControl("最終集信日"), TextBox).Text
                Me.ddlRemoveClsDtl.ppSelectedValue = CType(rowData.FindControl("撤去区分"), TextBox).Text
                Me.lblMishushinDtl.Text = CType(rowData.FindControl("未集信"), TextBox).Text
                'TBPINPP001-001
                If CType(rowData.FindControl("未集信理由コード"), TextBox).Text = "" Then
                    Me.ddlUnclctReasonDtl.SelectedIndex = 0
                Else
                    If Me.ddlUnclctReasonDtl.Items.FindByValue(CType(rowData.FindControl("未集信理由コード"), TextBox).Text) Is Nothing Then
                        Me.ddlUnclctReasonDtl.Items.Insert(0, New ListItem(Nothing, Nothing))
                        Me.ddlUnclctReasonDtl.SelectedIndex = 1
                        Me.ddlUnclctReasonDtl.Items.Item(1).Value = CType(rowData.FindControl("未集信理由コード"), TextBox).Text
                        Me.ddlUnclctReasonDtl.Items.Item(1).Text = CType(rowData.FindControl("未集信理由コード"), TextBox).Text + ":" + CType(rowData.FindControl("未集信理由"), TextBox).Text
                    Else
                        If Not Me.ddlUnclctReasonDtl.Items.FindByValue(CType(rowData.FindControl("未集信理由コード"), TextBox).Text).ToString = CType(rowData.FindControl("未集信理由コード"), TextBox).Text + ":" + CType(rowData.FindControl("未集信理由"), TextBox).Text Then
                            Me.ddlUnclctReasonDtl.Items.Insert(0, New ListItem(Nothing, Nothing))
                            Me.ddlUnclctReasonDtl.SelectedIndex = 1
                            Me.ddlUnclctReasonDtl.Items.Item(1).Value = "00"
                            Me.ddlUnclctReasonDtl.Items.Item(1).Text = CType(rowData.FindControl("未集信理由コード"), TextBox).Text + ":" + CType(rowData.FindControl("未集信理由"), TextBox).Text
                        Else
                            Me.ddlUnclctReasonDtl.SelectedValue = CType(rowData.FindControl("未集信理由コード"), TextBox).Text
                        End If
                    End If
                End If
                'TBPINPP001-001 END
                ''ドロップダウンの内容書き換え
                ''QUAUPDP001-001
                ''If ddlEvent.SelectedIndex <> 0 Then
                'If CType(rowData.FindControl("事象コード"), TextBox).Text = "" Then
                '    ddlEvent.SelectedIndex = 0
                'Else
                '    If ddlEvent.Items.FindByValue(CType(rowData.FindControl("事象コード"), TextBox).Text) Is Nothing Then
                '        Me.ddlEvent.Items.Insert(0, New ListItem(Nothing, Nothing))
                '        ddlEvent.SelectedIndex = 1
                '        ddlEvent.Items.Item(1).Value = "00"
                '        ddlEvent.Items.Item(1).Text = CType(rowData.FindControl("事象コード"), TextBox).Text & ":" & CType(rowData.FindControl("事象"), TextBox).Text.Replace(Environment.NewLine, "")
                '        'ddlEvent.Items.FindByValue(CType(rowData.FindControl("事象コード"), TextBox).Text).Text _
                '        '    = CType(rowData.FindControl("事象コード"), TextBox).Text & ":" & CType(rowData.FindControl("事象"), TextBox).Text
                '    Else
                '        If Not ddlEvent.Items.FindByValue(CType(rowData.FindControl("事象コード"), TextBox).Text).ToString = CType(rowData.FindControl("事象コード"), TextBox).Text & ":" & CType(rowData.FindControl("事象"), TextBox).Text.Replace(Environment.NewLine, "") Then
                '            Me.ddlEvent.Items.Insert(0, New ListItem(Nothing, Nothing))
                '            ddlEvent.SelectedIndex = 1
                '            ddlEvent.Items.Item(1).Value = "00"
                '            ddlEvent.Items.Item(1).Text = CType(rowData.FindControl("事象コード"), TextBox).Text & ":" & CType(rowData.FindControl("事象"), TextBox).Text.Replace(Environment.NewLine, "")
                '        Else
                '            ddlEvent.SelectedValue = CType(rowData.FindControl("事象コード"), TextBox).Text             '事象
                '        End If
                '    End If
                'End If
                ''QUAUPDP001-001 END
                Me.dtbArtclArvDtDtl.ppText = CType(rowData.FindControl("物品到着日"), TextBox).Text
                Me.dtbSuckDtDtl.ppText = CType(rowData.FindControl("吸上げ処理日"), TextBox).Text
                Me.dtbArtclRetDtDtl.ppText = CType(rowData.FindControl("物品返却日"), TextBox).Text
                Me.dtbMstRepFSDtDtl.ppText = CType(rowData.FindControl("マスタ廃止日（ＦＳ）"), TextBox).Text
                Me.dtbMstRepNGCDtDtl.ppText = CType(rowData.FindControl("マスタ廃止日（ＮＧＣ）"), TextBox).Text
                Me.txtNoteTextDtl.ppText = CType(rowData.FindControl("備考"), TextBox).Text

                Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
                Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
                Dim objDs As DataSet = Nothing          'データセット
                Dim dtRow As DataRow = Nothing          'DataRow


                'DB接続
                If Not clsDataConnect.pfOpen_Database(objCn) Then
                    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Else
                    Try
                        objCmd = New SqlCommand(M_MY_DISP_ID + "_S2", objCn)
                        With objCmd.Parameters
                            '--パラメータ設定
                            'ＴＢＯＸＩＤ
                            .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text))
                        End With

                        'データ取得
                        objDs = clsDataConnect.pfGet_DataSet(objCmd)

                        If objDs.Tables(0).Rows.Count = 0 Then
                            Exit Sub
                        End If

                        dtRow = objDs.Tables(0).Rows(0)
                    Catch ex As Exception
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ情報取得")
                    Finally
                        'DB切断
                        If Not clsDataConnect.pfClose_Database(objCn) Then
                            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        End If
                    End Try
                End If

                'システム分類によって即時集信ボタンを制御
                If dtRow("システム分類").ToString <> "1" Then
                    Me.btnDetailCncnt.Enabled = True
                Else
                    Me.btnDetailCncnt.Enabled = False
                End If

                '各コントロール活性・非活性
                Me.txtTboxidDtl.ppEnabled = False
                If Me.lblMishushinDtl.Text = "あり" Then
                    '未集信ありの場合、物品到着日及び吸上げ処理日を活性
                    Me.dtbArtclArvDtDtl.ppEnabled = True
                    Me.dtbSuckDtDtl.ppEnabled = True
                Else
                    '未集信なしの場合、物品到着日及び吸上げ処理日を非活性
                    Me.dtbArtclArvDtDtl.ppEnabled = False
                    Me.dtbSuckDtDtl.ppEnabled = False
                End If
                '----20140729   武   from
                'Me.ddlRemoveClsDtl.ppEnabled = False '撤去区分
                '----20140729   武   to
                Me.btnDetailInsert.Enabled = False
                Me.btnDetailUpdate.Enabled = True
                If Me.lblCnstNoDtl.Text = String.Empty Then
                    '工事依頼番号がない場合は削除ボタンを活性
                    Me.btnDetailDelete.Enabled = True
                Else
                    '工事依頼番号がある場合は削除ボタンを非活性
                    Me.btnDetailDelete.Enabled = False
                End If
                Me.btnDetailPrint.Enabled = True

            End If

            '--------------------------------
            '2014/06/19 星野　ここまで
            '--------------------------------


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
        Finally
        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' ＴＢＯＸＩＤ変更時処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub txtTboxidM_TextChanged()

        If Me.txtTboxidDtl.ppText <> String.Empty Then
            If Not msGetTboxInfo() Then
                Me.txtTboxidDtl.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
                Me.txtTboxidDtl.ppTextBox.Focus()
            End If
        End If

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
            '検索条件クリア
            msClearSearchCondition()

            '明細クリア
            msClearDetail()

            'グリッド、該当件数
            Me.grvList.DataSource = New DataTable
            Master.ppCount = "0"
            Me.grvList.DataBind()

            '工事日Fromへフォーカス
            Me.dtbCnstDt.ppDateBoxFrom.Focus()

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
    ''' 検索条件クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearSearchCondition()

        Me.dtbCnstDt.ppFromText = String.Empty
        Me.dtbCnstDt.ppToText = String.Empty
        Me.txtTboxid.ppFromText = String.Empty
        Me.txtTboxid.ppToText = String.Empty
        Me.ddlSystemFm.SelectedIndex = 0
        Me.ddlSystemTo.SelectedIndex = 0
        Me.dtbSuckDt.ppFromText = String.Empty
        Me.dtbSuckDt.ppToText = String.Empty
        Me.cbxSuckDt.Checked = False
        Me.cbxMishushinAri.Checked = False
        Me.cbxMishushinNashi.Checked = False

    End Sub

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetData() As DataSet

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim strBuff As String = String.Empty
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
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S1", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '工事日From
                    .Add(pfSet_Param("cnstdt_f", SqlDbType.NVarChar, Me.dtbCnstDt.ppFromText))

                    '工事日To
                    .Add(pfSet_Param("cnstdt_t", SqlDbType.NVarChar, Me.dtbCnstDt.ppToText))

                    'ＴＢＯＸＩＤFrom
                    If Me.txtTboxid.ppToText = String.Empty OrElse
                       Me.txtTboxid.ppToText = Me.txtTboxid.ppFromText Then
                        'ＴＢＯＸＩＤToが空白、またはFromToが一致した場合は「あいまい検索」なのでエスケープする
                        .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar,
                                         Me.txtTboxid.ppFromText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    Else
                        '上記以外は「範囲検索」なのでエスケープしない
                        .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, Me.txtTboxid.ppFromText))
                    End If

                    'ＴＢＯＸＩＤTo
                    .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, Me.txtTboxid.ppToText))

                    'システムFrom
                    .Add(pfSet_Param("system_f", SqlDbType.NVarChar, Me.ddlSystemFm.SelectedValue))

                    'システムTo
                    .Add(pfSet_Param("system_t", SqlDbType.NVarChar, Me.ddlSystemTo.SelectedValue))

                    '吸上げ処理日From
                    .Add(pfSet_Param("suckdt_f", SqlDbType.NVarChar, Me.dtbSuckDt.ppFromText))

                    '吸上げ処理日To
                    .Add(pfSet_Param("suckdt_t", SqlDbType.NVarChar, Me.dtbSuckDt.ppToText))

                    '吸上げ処理日空欄検索チェックボックス
                    If Me.cbxSuckDt.Checked Then
                        .Add(pfSet_Param("suckdt", SqlDbType.NVarChar, "1"))
                    Else
                        .Add(pfSet_Param("suckdt", SqlDbType.NVarChar, "0"))
                    End If

                    '未集信「あり」チェックボックス
                    If Me.cbxMishushinAri.Checked Then
                        .Add(pfSet_Param("MishushinAri", SqlDbType.NVarChar, "1"))
                    Else
                        .Add(pfSet_Param("MishushinAri", SqlDbType.NVarChar, "0"))
                    End If

                    '未集信「なし」チェックボックス
                    If Me.cbxMishushinNashi.Checked Then
                        .Add(pfSet_Param("MishushinNashi", SqlDbType.NVarChar, "1"))
                    Else
                        .Add(pfSet_Param("MishushinNashi", SqlDbType.NVarChar, "0"))
                    End If

                End With

                'データ取得
                mfGetData = clsDataConnect.pfGet_DataSet(objCmd)

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.Master.ppTitle)
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
    ''' データ表示処理
    ''' </summary>
    ''' <param name="objDs"></param>
    ''' <remarks></remarks>
    Private Sub msDispData(objDs As DataSet)

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

            '件数を設定
            If objDs.Tables(0).Rows.Count = 0 Then
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                Master.ppCount = "0"
            Else
                '閾値を超えた場合はメッセージを表示
                If objDs.Tables(0).Rows(0)("データ件数") > objDs.Tables(0).Rows.Count Then
                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                        objDs.Tables(0).Rows(0)("データ件数").ToString, objDs.Tables(0).Rows.Count.ToString)
                End If
                Master.ppCount = objDs.Tables(0).Rows(0)("データ件数").ToString
            End If

            '取得したデータをグリッドに設定
            Me.grvList.DataSource = objDs.Tables(0)

            '変更を反映
            Me.grvList.DataBind()

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

    ''' <summary>
    ''' CSVファイル出力処理
    ''' </summary>
    ''' <param name="objDs"></param>
    ''' <remarks></remarks>
    Private Sub msCsvFileOutput(objDs As DataSet)

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            'CSVファイル出力不要列を削除
            objDs.Tables(0).Columns.Remove("最終集信日")
            objDs.Tables(0).Columns.Remove("撤去区分")
            objDs.Tables(0).Columns.Remove("未集信理由コード")
            objDs.Tables(0).Columns.Remove("データ件数")

            If objDs.Tables(0).Rows.Count = 0 Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
            Else
                'CSVファイルダウンロード
                If pfDLCsvFile(Master.Master.ppTitle + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv",
                               objDs.Tables(0), True, Me) <> 0 Then
                    psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
                End If
            End If

        Catch ex As Threading.ThreadAbortException

        Catch ex As Exception
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
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
    ''' マスタ廃止・停止依頼書印刷用データ取得処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDataMstReq() As DataSet

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim strBuff As String = String.Empty
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGetDataMstReq = Nothing

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S3", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    'ＴＢＯＸＩＤ
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxidDtl.ppText))

                    '工事日
                    .Add(pfSet_Param("cnst_dt", SqlDbType.NVarChar, Me.lblCnstDtDtl.Text))
                End With

                'データ取得
                mfGetDataMstReq = clsDataConnect.pfGet_DataSet(objCmd)

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.Master.ppTitle)
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
    ''' 明細クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearDetail()

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            'ドロップダウンリスト再取得
            msSetddlUnclctReason()

            Me.lblCnstDtDtl.Text = String.Empty
            Me.txtTboxidDtl.ppText = String.Empty
            Me.lblHallNmDtl.Text = String.Empty
            Me.lblSystemDtl.Text = String.Empty
            Me.lblCnstNoDtl.Text = String.Empty
            Me.dtbLastBDtDtl.ppText = String.Empty
            Me.dtbLastSDtDtl.ppText = String.Empty
            Me.ddlRemoveClsDtl.ppDropDownList.SelectedIndex = 0
            Me.lblMishushinDtl.Text = String.Empty
            Me.ddlUnclctReasonDtl.SelectedIndex = 0
            Me.dtbArtclArvDtDtl.ppText = String.Empty
            Me.dtbSuckDtDtl.ppText = String.Empty
            Me.dtbArtclRetDtDtl.ppText = String.Empty
            Me.dtbMstRepFSDtDtl.ppText = String.Empty
            Me.dtbMstRepNGCDtDtl.ppText = String.Empty
            Me.txtNoteTextDtl.ppText = String.Empty

            '各コントロール活性・非活性
            Me.txtTboxidDtl.ppEnabled = True     'TBOXID
            Me.ddlRemoveClsDtl.ppEnabled = True  '撤去区分
            Me.dtbArtclArvDtDtl.ppEnabled = True '物品到着日
            Me.dtbSuckDtDtl.ppEnabled = True     '吸上げ処理日
            Me.btnDetailInsert.Enabled = True    '追加ボタン
            Me.btnDetailUpdate.Enabled = False   '変更ボタン
            Me.btnDetailDelete.Enabled = False   '削除ボタン
            Me.btnDetailCncnt.Enabled = True     '即時集信ボタン
            Me.btnDetailPrint.Enabled = False    '印刷ボタン

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
    ''' ドロップダウンリスト設定（システム）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlSystem()

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL011", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ＴＢＯＸリスト、ＴＢＯＸ機種コードでDistinctする。（重複データをカット）
                Dim dv As DataView = objDs.Tables(0).DefaultView
                Dim dt As DataTable = dv.ToTable("ＴＢＯＸ", True, "ＴＢＯＸリスト", "ＴＢＯＸ機種コード")

                'ドロップダウンリスト設定
                '(1)検索条件
                Me.ddlSystemFm.Items.Clear()
                Me.ddlSystemFm.DataSource = dt
                Me.ddlSystemFm.DataTextField = "ＴＢＯＸリスト"
                Me.ddlSystemFm.DataValueField = "ＴＢＯＸ機種コード"
                Me.ddlSystemFm.DataBind()
                Me.ddlSystemFm.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                '(2)検索結果（明細）
                Me.ddlSystemTo.Items.Clear()
                Me.ddlSystemTo.DataSource = dt
                Me.ddlSystemTo.DataTextField = "ＴＢＯＸリスト"
                Me.ddlSystemTo.DataValueField = "ＴＢＯＸ機種コード"
                Me.ddlSystemTo.DataBind()
                Me.ddlSystemTo.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸマスタ一覧取得")
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

    ''' <summary>
    ''' ドロップダウンリスト設定（未集信理由）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlUnclctReason()

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL024", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                Me.ddlUnclctReasonDtl.Items.Clear()
                Me.ddlUnclctReasonDtl.DataSource = objDs.Tables(0)
                Me.ddlUnclctReasonDtl.DataTextField = "理由"
                Me.ddlUnclctReasonDtl.DataValueField = "コード"
                Me.ddlUnclctReasonDtl.DataBind()
                Me.ddlUnclctReasonDtl.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "未集信マスタ一覧取得")
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

    ''' <summary>
    ''' 整合性チェック（検索ボタン押下時）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheckSearchCondition()

        Dim drMsg As DataRow = Nothing  '検証メッセージ取得

        'システムFromTo大小チェック
        If Me.ddlSystemTo.SelectedValue <> String.Empty AndAlso
           Me.ddlSystemFm.SelectedValue > Me.ddlSystemTo.SelectedValue Then
            drMsg = pfGet_ValMes("2001", "システム")
            Me.valSystem.ErrorMessage = drMsg.Item(P_VALMES_MES)
            Me.valSystem.Text = drMsg.Item(P_VALMES_SMES)
            Me.valSystem.IsValid = False
            Me.ddlSystemFm.Focus()
        End If

    End Sub

    ''' <summary>
    ''' 整合性チェック（明細の追加・変更ボタン押下時）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheckUpdateDetail()

        Dim drMsg As DataRow = Nothing      '検証メッセージ取得

        'TBOXIDの整合チェック
        If Not msGetTboxInfo() Then
            Me.txtTboxidDtl.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
            Me.txtTboxidDtl.ppTextBox.Focus()
        End If

        '物品到着日と吸上げ処理日の日付整合チェック
        If Me.dtbSuckDtDtl.ppText <> String.Empty AndAlso
           Me.dtbArtclArvDtDtl.ppText > Me.dtbSuckDtDtl.ppText Then
            Me.dtbSuckDtDtl.psSet_ErrorNo("1003", Me.dtbSuckDtDtl.ppName, Me.dtbArtclArvDtDtl.ppName)
            Me.dtbSuckDtDtl.ppDateBox.Focus()
        End If

        '吸上げ処理日と物品返却日の日付整合チェック
        If Me.dtbArtclRetDtDtl.ppText <> String.Empty AndAlso
           Me.dtbSuckDtDtl.ppText > Me.dtbArtclRetDtDtl.ppText Then
            Me.dtbArtclRetDtDtl.psSet_ErrorNo("1003", Me.dtbArtclRetDtDtl.ppName, Me.dtbSuckDtDtl.ppName)
            Me.dtbArtclRetDtDtl.ppDateBox.Focus()
        End If

        '物品到着日と物品返却日の日付整合チェック
        If Me.dtbArtclRetDtDtl.ppText <> String.Empty AndAlso
           Me.dtbArtclArvDtDtl.ppText > Me.dtbArtclRetDtDtl.ppText Then
            Me.dtbArtclRetDtDtl.psSet_ErrorNo("1003", Me.dtbArtclRetDtDtl.ppName, Me.dtbArtclArvDtDtl.ppName)
            Me.dtbArtclRetDtDtl.ppDateBox.Focus()
        End If

        '備考の行数チェック
        'If Me.txtNoteTextDtl.ppText.Split(Environment.NewLine).Length > 3 Then
        '    Me.txtNoteTextDtl.psSet_ErrorNo("3004", Me.txtNoteTextDtl.ppName, "3")
        '    Me.txtNoteTextDtl.ppTextBox.Focus()
        'End If

    End Sub

    ''' <summary>
    ''' 随時一覧データ更新処理
    ''' </summary>
    ''' <param name="intMode">1:追加、2:更新、3:削除</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfUpdateDataDetail(intMode As Integer) As Boolean

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
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
                strSpName = M_MY_DISP_ID + "_U1"
                strErrCode = "00003"
                strErrCode2 = "00008"
            Case 2  '更新
                strSpName = M_MY_DISP_ID + "_U2"
                strErrCode = "00001"
                strErrCode2 = "00007"
            Case 3  '削除
                strSpName = M_MY_DISP_ID + "_D1"
                strErrCode = "00002"
                strErrCode2 = "00009"
            Case Else
                Exit Function
        End Select

        '随時一覧データ更新処理
        If clsDataConnect.pfOpen_Database(objCn) Then
            Try
                objCmd = New SqlCommand(strSpName, objCn)
                With objCmd.Parameters
                    'パラメータ設定
                    '--ＴＢＯＸＩＤ
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxidDtl.ppText))

                    '--工事日(yyyy/MM/dd)
                    .Add(pfSet_Param("cnst_dt", SqlDbType.NVarChar, Me.lblCnstDtDtl.Text))

                    '--ユーザーＩＤ
                    .Add(pfSet_Param("userid", SqlDbType.NVarChar, Me.User.Identity.Name))

                    If intMode = 1 Then
                        '--撤去区分
                        .Add(pfSet_Param("remove_cls", SqlDbType.NVarChar, Me.ddlRemoveClsDtl.ppSelectedValue))
                    End If

                    If intMode = 1 OrElse intMode = 2 Then
                        '--最終営業日(yyyy/MM/dd)
                        If Me.dtbLastBDtDtl.ppText = String.Empty Then
                            .Add(pfSet_Param("lstwrk_dt", SqlDbType.NVarChar, DBNull.Value))
                        Else
                            .Add(pfSet_Param("lstwrk_dt", SqlDbType.NVarChar, Me.dtbLastBDtDtl.ppText))
                        End If

                        '--未集信理由
                        If Me.ddlUnclctReasonDtl.SelectedIndex = 0 Then
                            .Add(pfSet_Param("unclct_rsn", SqlDbType.NVarChar, Me.ddlUnclctReasonDtl.SelectedValue.ToString))
                        Else
                            .Add(pfSet_Param("unclct_rsn", SqlDbType.NVarChar, Me.ddlUnclctReasonDtl.SelectedItem.ToString.Remove(2)))
                        End If

                        '--物品到着日(yyyy/MM/dd)
                        If Me.dtbArtclArvDtDtl.ppText = String.Empty Then
                            .Add(pfSet_Param("arrival_dt", SqlDbType.NVarChar, DBNull.Value))
                        Else
                            .Add(pfSet_Param("arrival_dt", SqlDbType.NVarChar, Me.dtbArtclArvDtDtl.ppText))
                        End If

                        '--吸上処理日(yyyy/MM/dd)
                        If Me.dtbSuckDtDtl.ppText = String.Empty Then
                            .Add(pfSet_Param("suck_dt", SqlDbType.NVarChar, DBNull.Value))
                        Else
                            .Add(pfSet_Param("suck_dt", SqlDbType.NVarChar, Me.dtbSuckDtDtl.ppText))
                        End If

                        '--マスタ廃止日（ＦＳ）(yyyy/MM/dd)
                        If Me.dtbMstRepFSDtDtl.ppText = String.Empty Then
                            .Add(pfSet_Param("fs_abol_dt", SqlDbType.NVarChar, DBNull.Value))
                        Else
                            .Add(pfSet_Param("fs_abol_dt", SqlDbType.NVarChar, Me.dtbMstRepFSDtDtl.ppText))
                        End If

                        '--マスタ廃止日（ＮＧＣ）(yyyy/MM/dd)
                        If Me.dtbMstRepNGCDtDtl.ppText = String.Empty Then
                            .Add(pfSet_Param("ngc_abol_dt", SqlDbType.NVarChar, DBNull.Value))
                        Else
                            .Add(pfSet_Param("ngc_abol_dt", SqlDbType.NVarChar, Me.dtbMstRepNGCDtDtl.ppText))
                        End If

                        '--最終集信日(yyyy/MM/dd)
                        If Me.dtbLastSDtDtl.ppText = String.Empty Then
                            .Add(pfSet_Param("lstclct_dt", SqlDbType.NVarChar, DBNull.Value))
                        Else
                            .Add(pfSet_Param("lstclct_dt", SqlDbType.NVarChar, Me.dtbLastSDtDtl.ppText))
                        End If

                        '--返却日(yyyy/MM/dd)
                        If Me.dtbArtclRetDtDtl.ppText = String.Empty Then
                            .Add(pfSet_Param("return_dt", SqlDbType.NVarChar, DBNull.Value))
                        Else
                            .Add(pfSet_Param("return_dt", SqlDbType.NVarChar, Me.dtbArtclRetDtDtl.ppText))
                        End If

                        '--備考
                        .Add(pfSet_Param("notetext", SqlDbType.NVarChar, Me.txtNoteTextDtl.ppText))
                    End If

                    '戻り値
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                End With

                'データ追加・更新・削除
                Using conTrn = objCn.BeginTransaction
                    objCmd.Transaction = conTrn

                    'コマンドタイプ設定(ストアド)
                    objCmd.CommandType = CommandType.StoredProcedure

                    'ストアド実行
                    objCmd.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(objCmd.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, strErrCode2, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "随時一覧", intRtn.ToString)
                        Exit Function
                    End If

                    'コミット
                    conTrn.Commit()

                End Using

                mfUpdateDataDetail = True

                If Not dtbSuckDtDtl.ppText = String.Empty Then
                    objCmd = New SqlCommand("TBPINPP001_U3", objCn)
                    With objCmd.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxidDtl.ppText))
                        .Add(pfSet_Param("cnst_No", SqlDbType.NVarChar, Me.lblCnstNoDtl.Text))
                        .Add(pfSet_Param("lstwrk_dt", SqlDbType.NVarChar, (Me.dtbLastBDtDtl.ppText).Replace("/", "")))
                        If Me.ddlUnclctReasonDtl.SelectedIndex = 0 Then
                            .Add(pfSet_Param("unclct_rsn", SqlDbType.NVarChar, Me.ddlUnclctReasonDtl.SelectedValue.ToString))
                        Else
                            .Add(pfSet_Param("unclct_rsn", SqlDbType.NVarChar, Me.ddlUnclctReasonDtl.SelectedItem.ToString.Remove(2)))
                        End If
                        .Add(pfSet_Param("suck_dt", SqlDbType.NVarChar, (Me.dtbSuckDtDtl.ppText).Replace("/", "")))
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, Me.User.Identity.Name))
                    End With
                End If
                'データ追加・更新・削除
                Using conTrn = objCn.BeginTransaction
                    objCmd.Transaction = conTrn

                    'コマンドタイプ設定(ストアド)
                    objCmd.CommandType = CommandType.StoredProcedure

                    'ストアド実行
                    objCmd.ExecuteNonQuery()

                    'コミット
                    conTrn.Commit()

                End Using
            Catch ex As Exception
                psMesBox(Me, strErrCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "随時一覧")
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

    ''' <summary>
    ''' ＴＢＯＸ情報取得処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function msGetTboxInfo() As Boolean

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim dtRow As DataRow = Nothing          'DataRow
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        msGetTboxInfo = False

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S2", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    'ＴＢＯＸＩＤ
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxidDtl.ppText))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables(0).Rows.Count = 0 Then
                    Exit Function
                End If

                dtRow = objDs.Tables(0).Rows(0)

                'ＴＢＯＸ情報をラベルに設定
                Me.lblHallNmDtl.Text = dtRow("ホール名").ToString
                Me.lblSystemDtl.Text = dtRow("システム").ToString
                '--------------------------------
                '2014/06/16 武　ここから
                '--------------------------------
                'システム分類によって即時集信ボタンを制御
                If dtRow("システム分類").ToString <> "1" Then
                    Me.btnDetailCncnt.Enabled = True
                Else
                    Me.btnDetailCncnt.Enabled = False
                End If
                '--------------------------------
                '2014/06/16 武　ここまで
                '--------------------------------
                msGetTboxInfo = True



            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ情報取得")
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
