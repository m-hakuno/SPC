'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　マスタ管理データ
'*　ＰＧＭＩＤ：　COMMENP006
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作成：2013.12.03：高松
'*  変更：2017/10/12：伯野
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'COMMENP006-001     2015/05/25      加賀      [ホールTEL用途]の表示形式をドロップダウンに変更              
'COMMENP006-002     2015/05/25      加賀      [保担]、[総括]、[代理店]の表示形式をドロップダウンに変更、[住所][担当営業所]活性化              
'COMMENP006-003     2015/06/03      加賀      ホールTEL情報更新バグ修正              
'COMMENP006-004     2015/06/04      加賀      入力項目[郵便番号]追加、住所項目活性化
'COMMENP006-005     2015/07/08      加賀      更新後に画面更新処理追加
'COMMENP006-006     2015/07/28      加賀      入力項目[Ver]追加、整合性チェック追加
'COMMENP006-007     2015/08/19      加賀      画面遷移ボタン[保守対応依頼書][トラブル処理票][ミニ処理票]追加 ※現状PreRenderにて非活性化
'COMMENP006-008     2015/08/19      加賀      ホールTEL１～５の重複チェック追加　※コメントアウト
'COMMENP006-009     2015/08/19      加賀      親子ホールの全MDN機器名を表示するように変更
'COMMENP006-010     2015/09/01      加賀      親子全MDN台数の初期表示を「0」に変更
'COMMENP006-011     2016/01/19      加賀      システム分類がIDの場合、「運用状況」「運用終了日」を表示する(管理者のみ)
'COMMENP006-012     2016/01/19      加賀      [保担]、[総括]、[代理店]ドロップダウンの項目追加処理を変更
'COMMENP006-013     2016/02/02      加賀      ※保留※[ホールコード]、[ホール名]を変更可能に変更(管理者のみ)
'COMMENP006-014     2016/02/12      加賀      「運用終了日」の入力チェック変更
'COMMENP006-015     2016/11/21      加賀      電話番号マスタ(M19)同期機能追加
'COMMEMP006-016     2016/12/20      栗原      CRS対応（同期処理共通化）
'COMMEMP006-017     2017/10/12      伯野      各プロシージャにデータセット破棄を記述

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax
Imports System.Data
Imports System.IO
Imports System.Web.Hosting
'--------------------------------
'2014/06/11 後藤　ここから
'--------------------------------
Imports SPC.ClsCMExclusive
'--------------------------------
'2014/06/11 後藤　ここまで
'--------------------------------

#End Region

Public Class COMMENP006
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
    '対応履歴照会画面のパス
    Const M_BRKINQP001 As String = "~/" & P_FLR & "/" &
        P_FUN_BRK & P_SCR_INQ & P_PAGE & "001" & "/" &
        P_FUN_BRK & P_SCR_INQ & P_PAGE & "001.aspx"
    'ミニ処理票
    Const P_BRKUPDP001 As String = "~/" & P_FLR & "/" &
        P_FUN_BRK & P_SCR_UPD & P_PAGE & "001" & "/" &
        P_FUN_BRK & P_SCR_UPD & P_PAGE & "001.aspx"
    'トラブル処理票
    Const P_REQSELP001 As String = "~/" & P_MAI & "/" &
            P_FUN_REQ & P_SCR_SEL & P_PAGE & "001" & "/" &
            P_FUN_REQ & P_SCR_SEL & P_PAGE & "001.aspx"
    '保守対応依頼書
    Const P_CMPSELP001 As String = "~/" & P_MAI & "/" &
            P_FUN_CMP & P_SCR_SEL & P_PAGE & "001" & "/" &
            P_FUN_CMP & P_SCR_SEL & P_PAGE & "001.aspx"

    Const M_DISP_ID As String = P_FUN_COM & P_SCR_MEN & P_PAGE & "006"
    Const M_SCRIPT_PASS As String = "~/Scripts/EnableChange"
    Const P_NUM As String = "連番"
    Const P_HALLINF As String = "ホール情報"
    Const P_HALLPRINT As String = "ホール詳細データ"
    Const P_BTN_MINI As String = "ミニ処理票"
    Const P_BTN_TRBL As String = "トラブル処理票"
    Const P_BTN_MNTE As String = "保守対応依頼書"
    Const P_BTN_HSTR As String = "対応履歴照会"
    Const P_BTN_SYNC As String = "同期"   'COMMENP006-015

    Const DDL_VALUE As String = "XXXXXX"


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
    Dim clsSqlDbSvr As New ClsSQLSvrDB
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom
    Dim clsExc As New ClsCMExclusive
    Dim strBtnSts As String = String.Empty

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        objStack = New StackFrame

        Try
            AddHandler Master.ppLeftButton1.Command, AddressOf btnMove_Click
            AddHandler Master.ppLeftButton2.Command, AddressOf btnMove_Click
            AddHandler Master.ppLeftButton3.Command, AddressOf btnMove_Click
            AddHandler Master.ppRigthButton1.Click, AddressOf btnUpd_Click
            AddHandler Master.ppRigthButton2.Click, AddressOf btnPrint_Click
            AddHandler Master.ppRigthButton3.Click, AddressOf btnReset_Click
            AddHandler Master.ppRigthButton4.Command, AddressOf btnMove_Click
            AddHandler Master.ppRigthButton5.Click, AddressOf btnSync_Click   'COMMENP006-015
            'COMMENP006-002
            AddHandler ddlSecChaSC.ppDropDownList.SelectedIndexChanged, AddressOf msDropdown_Chenged
            AddHandler ddlReviewSC.ppDropDownList.SelectedIndexChanged, AddressOf msDropdown_Chenged
            AddHandler ddlAgency.ppDropDownList.SelectedIndexChanged, AddressOf msDropdown_Chenged
            ddlSecChaSC.ppDropDownList.AutoPostBack = True
            ddlReviewSC.ppDropDownList.AutoPostBack = True
            ddlAgency.ppDropDownList.AutoPostBack = True
            'COMMENP006-002 END
            'COMMENP006-004
            'AddHandler txtZip.ppTextBox.TextChanged, AddressOf msZipno_Chaged
            'txtZip.ppTextBox.AutoPostBack = True
            'COMMENP006-004 END

            Me.txtNotes.ppTextBox.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txtNotes.ppMaxLength & """);")
            Me.txtNotes.ppTextBox.Attributes.Add("onChange", "lenCheck(this,""" & Me.txtNotes.ppMaxLength & """);")


            If Not IsPostBack Then  '初回表示

                '変数
                Dim strResult As String = Nothing                   '処理結果
                Dim strUser As String = Nothing                     'ユーザ権限
                Dim strTerms As String = Nothing                    '遷移ボタンの権限
                Dim enableScript As String = Nothing                '用途内容制御用

                '開始ログ出力
                psLogStart(Me)

                If Session(P_SESSION_USERID) = Nothing Then
                    '排他削除
                    clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                    '閉じる
                    psClose_Window(Me)
                End If

                If Session(P_SESSION_TERMS) = Nothing Then
                    '排他削除
                    clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                    '閉じる
                    psClose_Window(Me)
                End If

                'セッションからデータの取得
                strTerms = Session(P_SESSION_TERMS)
                strUser = Session(P_SESSION_USERID)

                'COMMENP006-001
                ''用途コードによる用途内容の入力可否制御
                'enableScript = "ddlChange('" + DDLUseCd1.ClientID + "','" + txtUseCtt1.ppTextBox.ClientID + "')"
                'DDLUseCd1.Attributes.Add("onChange", enableScript)
                'enableScript = "ddlChange('" + DDLUseCd2.ClientID + "','" + txtUseCtt2.ppTextBox.ClientID + "')"
                'DDLUseCd2.Attributes.Add("onChange", enableScript)
                'enableScript = "ddlChange('" + DDLUseCd3.ClientID + "','" + txtUseCtt3.ppTextBox.ClientID + "')"
                'DDLUseCd3.Attributes.Add("onChange", enableScript)
                'enableScript = "ddlChange('" + DDLUseCd4.ClientID + "','" + txtUseCtt4.ppTextBox.ClientID + "')"
                'DDLUseCd4.Attributes.Add("onChange", enableScript)
                'COMMENP006-001 END

                'todo 権限の取得 0:使用不可 1:使用可 2:参照可
                'strResult = authority()

                '各コントロールの制御・初期化
                display_Control(strUser, strTerms)

                'プログラムＩＤ、画面名設定
                Master.ppProgramID = M_DISP_ID
                Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

                'パンくずリスト設定
                Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

                'ビューステートに値を設定
                Me.ViewState.Add(P_KEY, Session(P_KEY))

                'todo 排他制御処理を実行(画面更新が可能な場合)
                'If strTerms = "1" Then
                'strResult = Lock()
                'End If

                '詳細情報検索
                msGet_Data_Sel("0")

                'システム分類がIC,LUTEの場合、「運用状況」「運用終了日」を非表示 COMMENP006-011
                If Session(P_SESSION_AUTH) <> "管理者" OrElse ViewState(P_KEY)(2) <> "1" Then
                    '[Display:none]を追加
                    trHdn.Style.Add("display", "none")
                    ddlPerCls.ppEnabled = False
                    txtWrkEndDate.ppEnabled = False
                End If
                'COMMENP006-011 END

                'COMMENP006-015
                'If ddlPerCls.ppSelectedValue = "99" Then
                '    Master.ppRigthButton5.Enabled = False   '同期ボタン 
                'End If
                'COMMENP006-015 END


                '★排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
                If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then

                    Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)

                End If

                'todo セッション変数の表示画面数の追加更新を行う ※あとで追加する 

                '初期フォーカスの設定
                Select Case strTerms
                    Case ClsComVer.E_遷移条件.更新
                        Me.txtHoleTel2.ppTextBox.Focus()
                    Case ClsComVer.E_遷移条件.参照
                        Master.ppRigthButton4.Focus()
                End Select

                '終了ログ出力
                psLogEnd(Me)

            End If

        Catch ex As SqlException

            '検索取得エラー
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '排他削除
            clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)

            psClose_Window(Me)

        Catch ex As Exception

            'システムエラー
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '排他削除
            clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)

            psClose_Window(Me)

        End Try

    End Sub

    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Dim strUser As String = Nothing
        Dim strTerms As String = Nothing
        Dim strTellNum As String = String.Empty
        Dim js_exe As String = String.Empty


        Select Case Session(P_SESSION_AUTH)
            Case "管理者", "SPC"
                If Not IsPostBack OrElse strBtnSts = "1" OrElse strBtnSts = "2" Then
                    Me.btnTell1.Visible = True
                    Me.btnTell2.Visible = True
                    Me.btnTell3.Visible = True
                    Me.btnTell4.Visible = True
                    Me.btnTell5.Visible = True
                    If txtHoleTel1.ppText.Trim <> String.Empty Then
                        strTellNum = txtHoleTel1.ppText.Trim
                        js_exe = "exe_Cti(" + """" + strTellNum.Replace("-", "") + """" + ");"
                        btnTell1.OnClientClick = pfGet_OCClickMes("40001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, strTellNum).Replace(";", "") + "&&" + js_exe
                        btnTell1.Enabled = True
                    Else
                        btnTell1.Enabled = False
                    End If
                    If txtHoleTel2.ppText.Trim <> String.Empty Then
                        strTellNum = txtHoleTel2.ppText.Trim
                        js_exe = "exe_Cti(" + """" + strTellNum.Replace("-", "") + """" + ");"
                        btnTell2.OnClientClick = pfGet_OCClickMes("40001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, strTellNum).Replace(";", "") + "&&" + js_exe
                        btnTell2.Enabled = True
                    Else
                        btnTell2.Enabled = False
                    End If
                    If txtHoleTel3.ppText.Trim <> String.Empty Then
                        strTellNum = txtHoleTel3.ppText.Trim
                        js_exe = "exe_Cti(" + """" + strTellNum.Replace("-", "") + """" + ");"
                        btnTell3.OnClientClick = pfGet_OCClickMes("40001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, strTellNum).Replace(";", "") + "&&" + js_exe
                        btnTell3.Enabled = True
                    Else
                        btnTell3.Enabled = False
                    End If
                    If txtHoleTel4.ppText.Trim <> String.Empty Then
                        strTellNum = txtHoleTel4.ppText.Trim
                        js_exe = "exe_Cti(" + """" + strTellNum.Replace("-", "") + """" + ");"
                        btnTell4.OnClientClick = pfGet_OCClickMes("40001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, strTellNum).Replace(";", "") + "&&" + js_exe
                        btnTell4.Enabled = True
                    Else
                        btnTell4.Enabled = False
                    End If
                    If txtHoleTel5.ppText.Trim <> String.Empty Then
                        strTellNum = txtHoleTel5.ppText.Trim
                        js_exe = "exe_Cti(" + """" + strTellNum.Replace("-", "") + """" + ");"
                        btnTell5.OnClientClick = pfGet_OCClickMes("40001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, strTellNum).Replace(";", "") + "&&" + js_exe
                        btnTell5.Enabled = True
                    Else
                        btnTell5.Enabled = False
                    End If
                End If
            Case "営業所"
                '---------------------------
                '2014/06/17 武 ここから
                '---------------------------
                strTerms = ClsComVer.E_遷移条件.参照
                strUser = Session(P_SESSION_USERID)
                'display_Control(strUser, strTerms)
                '---------------------------
                '2014/06/17 武 ここまで
                '---------------------------
                Master.ppRigthButton1.Enabled = False   '更新ボタン
                Master.ppRigthButton4.Enabled = False   '対応履歴ボタン
                Master.ppRigthButton5.Enabled = False   '同期ボタン COMMENP006-015
                Master.ppLeftButton1.Enabled = False
                Master.ppLeftButton2.Enabled = False
                Master.ppLeftButton3.Enabled = False
            Case "NGC"
                '---------------------------
                '2014/06/17 武 ここから
                '---------------------------
                strTerms = ClsComVer.E_遷移条件.参照
                strUser = Session(P_SESSION_USERID)
                display_Control(strUser, strTerms)
                Master.ppRigthButton1.Enabled = False   '更新ボタン
                Master.ppRigthButton4.Enabled = False   '対応履歴ボタン
                Master.ppRigthButton5.Enabled = False   '同期ボタン COMMENP006-015
                '---------------------------
                '2014/06/17 武 ここまで
                '---------------------------
                Master.ppLeftButton1.Enabled = False
                Master.ppLeftButton2.Enabled = False
                Master.ppLeftButton3.Enabled = False
        End Select

        '管理者活性制御 'COMMENP006-011 'COMMENP006-013
        If Session(P_SESSION_AUTH) = "管理者" Then
            txtHoleCd.ppEnabled = True      'ホールコード
            txtHoleNm.ppEnabled = True      'ホール名
            cbxHallCd.Enabled = True        'ホールチェックボックス
            ddlPerCls.ppEnabled = True      '運用状況
            txtWrkEndDate.ppEnabled = True  '運用終了日
        Else
            ddlPerCls.ppEnabled = False     '運用状況
            txtWrkEndDate.ppEnabled = False '運用終了日
        End If
        txtHoleCd.ppEnabled = False     'ホールコード
        txtHoleNm.ppEnabled = False     'ホール名
        cbxHallCd.Enabled = False       'ホールチェックボックス
        cbxHallCd.Visible = False       'ホールチェックボックス
        'COMMENP006-013 END

    End Sub

    ''' <summary>
    ''' [保守対応依頼書][トラブル処理票][ミニ処理票]ボタン押下処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnMove_Click(sender As Object, e As CommandEventArgs)

        objStack = New StackFrame

        Try
            Dim strKye() As String            'キー項目
            Dim strView() As String = Me.ViewState(P_KEY)
            Dim strTrnaPath As String = String.Empty
            '開始ログ出力
            psLogStart(Me)

            'ボタンコマンド分岐
            Select Case e.CommandName
                Case P_BTN_MINI 'ミニ処理票
                    strTrnaPath = P_BRKUPDP001  'パス設定
                    strKye = {strView(0).ToString}       'TBOXID
                Case P_BTN_TRBL 'トラブル処理票
                    strTrnaPath = P_REQSELP001  'パス設定
                    strKye = {strView(0).ToString}       'TBOXID
                Case P_BTN_MNTE '保守対応依頼書
                    strTrnaPath = P_CMPSELP001  'パス設定
                    'キー項目の設定
                    strKye = {strView(0).ToString}       'TBOXID
                Case P_BTN_HSTR '対応履歴照会
                    strTrnaPath = M_BRKINQP001  'パス設定
                    'キー項目の設定
                    ReDim strKye(2)
                    strKye(0) = strView(0).ToString     'TBOXID
                    strKye(1) = strView(1).ToString     'NL区分
                    strKye(2) = strView(2).ToString     'システム分類
                Case Else
                    Exit Sub
            End Select

            'セッション情報設定
            Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録
            Session(P_SESSION_BCLIST) = Master.ppBcList_Text
            Session(P_SESSION_OLDDISP) = M_DISP_ID
            Session(P_KEY) = strKye

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
                            objStack.GetMethod.Name, M_BRKINQP001, strPrm, "TRANS")

            '別ブラウザ起動
            psOpen_Window(Me, strTrnaPath)

            '終了ログ出力
            psLogEnd(Me)

        Catch ex As Exception

            'システムエラー
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "対応履歴一覧画面")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' 元に戻すボタン押下処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnReset_Click()

        objStack = New StackFrame

        Try

            '開始ログ出力
            psLogStart(Me)

            '画面の再読み込み
            msGet_Data_Sel("1")

            strBtnSts = "2"

            '終了ログ出力
            psLogEnd(Me)

        Catch ex As SqlException

            '検索取得エラー
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, P_HALLINF)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Catch ex As Exception

            'システムエラー
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, P_HALLINF)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' 印刷ボタン押下処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnPrint_Click()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dtPrint As New DataTable                     '帳票用データセット
        Dim dtRow As DataRow = dtPrint.NewRow()          'データテーブルの行定義
        Dim dstOrders1 As New DataSet                    '表示情報のデータセット
        Dim strView() As String = Nothing                'ビューステート登録用の情報
        Dim rpt As MNTREP005
        Dim strFNm As String = "ホール詳細データ_" & Date.Today.ToString("yyyyMMddHHmmss")  '帳票名称

        objStack = New StackFrame

        Try

            '開始ログ出力
            psLogStart(Me)

            'データテーブルの項目名セット       
            dtPrint.Columns.Add("ＴＢＯＸＩＤ")
            dtPrint.Columns.Add("ＮＬ区分")
            dtPrint.Columns.Add("システム")
            dtPrint.Columns.Add("ＶＥＲ")
            dtPrint.Columns.Add("ＴＢＯＸＴＥＬ")
            dtPrint.Columns.Add("運用開始日")
            dtPrint.Columns.Add("ホール名")
            dtPrint.Columns.Add("ホールコード")
            dtPrint.Columns.Add("ＭＤＮ設置")
            dtPrint.Columns.Add("集信日")
            dtPrint.Columns.Add("住所")
            dtPrint.Columns.Add("ホールＴＥＬ1")
            dtPrint.Columns.Add("ＥＷ区分")
            dtPrint.Columns.Add("ＦＡＸ")
            dtPrint.Columns.Add("ホールＴＥＬ2")
            dtPrint.Columns.Add("用途内容1")
            dtPrint.Columns.Add("ホールＴＥＬ3")
            dtPrint.Columns.Add("用途内容2")
            dtPrint.Columns.Add("ホールＴＥＬ4")
            dtPrint.Columns.Add("用途内容3")
            dtPrint.Columns.Add("ホールＴＥＬ5")
            dtPrint.Columns.Add("用途内容4")
            dtPrint.Columns.Add("定型文1")
            dtPrint.Columns.Add("定型文2")
            dtPrint.Columns.Add("定型文3")
            dtPrint.Columns.Add("定型文4")
            dtPrint.Columns.Add("注意事項")
            dtPrint.Columns.Add("店舗種別")
            dtPrint.Columns.Add("ＭＤＮ台数")
            dtPrint.Columns.Add("担当営業部")
            dtPrint.Columns.Add("ＴＥＬ(担当営業部)")
            dtPrint.Columns.Add("親ＭＤＮ台数")
            dtPrint.Columns.Add("子1ＭＤＮ台数")
            dtPrint.Columns.Add("子2ＭＤＮ台数")
            dtPrint.Columns.Add("ＭＤＮ機器名")
            dtPrint.Columns.Add("ＴＢＯＸシリアル")
            dtPrint.Columns.Add("操作盤シリアル")
            dtPrint.Columns.Add("ＵＰＳシリアル")
            dtPrint.Columns.Add("プリンタシリアル")
            dtPrint.Columns.Add("ＣＲＴシリアル")
            dtPrint.Columns.Add("ＨＤＤ1シリアル")
            dtPrint.Columns.Add("ＨＤＤ2シリアル")
            dtPrint.Columns.Add("ＨＤＤ3シリアル")
            dtPrint.Columns.Add("ＨＤＤ4シリアル")
            dtPrint.Columns.Add("ＳＣ")
            dtPrint.Columns.Add("ＣＣ")
            dtPrint.Columns.Add("サンド")
            dtPrint.Columns.Add("券売機")
            dtPrint.Columns.Add("精算機")
            dtPrint.Columns.Add("保担ＳＣ")
            dtPrint.Columns.Add("ＬＡＮ担当ＩＤ")
            dtPrint.Columns.Add("ＬＡＮ担当情報")
            dtPrint.Columns.Add("住所(保担ＳＣ)")
            dtPrint.Columns.Add("ＴＥＬ(保担ＳＣ)")
            dtPrint.Columns.Add("ＦＡＸ(保担ＳＣ)")
            dtPrint.Columns.Add("統括ＳＣ")
            dtPrint.Columns.Add("支店名(統括ＳＣ)")
            dtPrint.Columns.Add("住所(統括ＳＣ)")
            dtPrint.Columns.Add("ＴＥＬ(統括ＳＣ)")
            dtPrint.Columns.Add("ＦＡＸ(統括ＳＣ)")
            dtPrint.Columns.Add("代理店")
            dtPrint.Columns.Add("住所(代理店)")
            dtPrint.Columns.Add("ＴＥＬ(代理店)")
            dtPrint.Columns.Add("ＦＡＸ(代理店)")
            dtPrint.Columns.Add("出力年月日")

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Throw New Exception
            End If

            'ViewStateから情報を取得
            strView = Me.ViewState(P_KEY)

            cmdDB = New SqlCommand(M_DISP_ID & "_S1", conDB) 'すべての表示情報を取得
            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("prm_tboxid", SqlDbType.NVarChar, strView(0).ToString))      'TBOXID
                .Add(pfSet_Param("prm_nl", SqlDbType.NVarChar, strView(1).ToString))          'NL区分
                .Add(pfSet_Param("prm_system", SqlDbType.NVarChar, strView(2).ToString))      'システム分類
                .Add(pfSet_Param("prm_hallcd", SqlDbType.NVarChar, strView(3).ToString))      'ホールコード
                .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))   '結果取得用
            End With

            'リストデータ取得
            dstOrders1 = clsDataConnect.pfGet_DataSet(cmdDB)

            'データテーブルの列に値を設定
            dtRow("ＴＢＯＸＩＤ") = Me.txtTboxId.ppText
            dtRow("ＮＬ区分") = Me.tbxNLSec.Text
            dtRow("システム") = Me.txtSystem.ppText
            dtRow("ＶＥＲ") = Me.txtVER.ppText
            dtRow("ＴＢＯＸＴＥＬ") = Me.txtTboxTel.ppText
            dtRow("運用開始日") = Me.txtOptSttDate.ppText
            dtRow("ホール名") = Me.txtHoleNm.ppText
            dtRow("ホールコード") = Me.txtHoleCd.ppText
            dtRow("集信日") = Me.txtCcrtDate.ppText
            dtRow("住所") = Me.txtHoleAdd.ppText
            dtRow("ホールＴＥＬ1") = Me.txtHoleTel1.ppText
            dtRow("ＥＷ区分") = Me.txtEWSec.ppText
            dtRow("ＦＡＸ") = Me.txtFaxNo.ppText

            For i As Integer = 0 To dstOrders1.Tables(0).Rows.Count - 1
                Select Case dstOrders1.Tables(0).Rows(i).Item("分類").ToString
                    Case "2"
                        dtRow("注意事項") = dstOrders1.Tables(0).Rows(i).Item("注意事項").ToString                          '注意事項
                    Case "7"
                        Select Case dstOrders1.Tables(0).Rows(i).Item("用途連番").ToString
                            Case "1"
                                dtRow("ホールＴＥＬ2") = dstOrders1.Tables(0).Rows(i).Item("ホールTEL").ToString                        'ホールTEL2
                                dtRow("用途内容1") = dstOrders1.Tables(0).Rows(i).Item("用途内容").ToString                         '用途内容1
                            Case "2"
                                dtRow("ホールＴＥＬ3") = dstOrders1.Tables(0).Rows(i).Item("ホールTEL").ToString                        'ホールTEL3
                                dtRow("用途内容2") = dstOrders1.Tables(0).Rows(i).Item("用途内容").ToString                         '用途内容2
                            Case "3"
                                dtRow("ホールＴＥＬ4") = dstOrders1.Tables(0).Rows(i).Item("ホールTEL").ToString                        'ホールTEL4
                                dtRow("用途内容3") = dstOrders1.Tables(0).Rows(i).Item("用途内容").ToString                         '用途内容3
                            Case "4"
                                dtRow("ホールＴＥＬ5") = dstOrders1.Tables(0).Rows(i).Item("ホールTEL").ToString                        'ホールTEL5
                                dtRow("用途内容4") = dstOrders1.Tables(0).Rows(i).Item("用途内容").ToString                         '用途内容4
                        End Select
                End Select
            Next

            dtRow("定型文1") = Me.txtNotes1.ppText
            dtRow("定型文2") = Me.txtNotes2.ppText
            dtRow("定型文3") = Me.txtNotes3.ppText
            dtRow("定型文4") = Me.txtNotes4.ppText
            dtRow("店舗種別") = Me.tbxStoreType.Text
            dtRow("ＭＤＮ設置") = Me.txtMDNInstallation.ppText
            dtRow("ＭＤＮ台数") = Me.txtNumberOfMDN.ppText                '2014/04/16 中川　修正
            dtRow("親ＭＤＮ台数") = Me.txtPrtNumberOfMDN.ppText           '2014/04/16 中川　修正
            dtRow("子1ＭＤＮ台数") = Me.txtChd1NumberOfMDN.ppText
            dtRow("子2ＭＤＮ台数") = Me.txtChd2NumberOfMDN.ppText
            dtRow("ＭＤＮ機器名") = Me.txtMDN.ppText
            dtRow("ＴＢＯＸシリアル") = Me.txtTboxSerial.ppText
            dtRow("操作盤シリアル") = Me.txtOppSerial.ppText
            dtRow("ＵＰＳシリアル") = Me.txtUPSSerial.ppText
            dtRow("プリンタシリアル") = Me.txtPrtSerial.ppText
            dtRow("ＣＲＴシリアル") = Me.txtCRTSerial.ppText
            dtRow("ＨＤＤ1シリアル") = Me.txtHDD1Serial.ppText
            dtRow("ＨＤＤ2シリアル") = Me.txtHDD2Serial.ppText
            dtRow("ＨＤＤ3シリアル") = Me.txtHDD3Serial.ppText
            dtRow("ＨＤＤ4シリアル") = Me.txtHDD4Serial.ppText
            dtRow("ＳＣ") = Me.txtSC.ppText
            dtRow("ＣＣ") = Me.txtCC.ppText
            dtRow("サンド") = Me.txtSand.ppText
            dtRow("券売機") = Me.txtTVMachine.ppText
            dtRow("精算機") = Me.txtAmMachine.ppText
            dtRow("担当営業部") = Me.txtConSalDep.ppText
            dtRow("ＴＥＬ(担当営業部)") = Me.txtSdTel.ppText
            'COMMENP006-002
            'dtRow("保担ＳＣ") = Me.txtSecChaSC.ppText
            If ddlSecChaSC.ppSelectedValue = String.Empty Then
                dtRow("保担ＳＣ") = String.Empty
            Else
                'コード部分を除いた文言を設定
                'dtRow("保担ＳＣ") = ddlSecChaSC.ppDropDownList.SelectedItem.ToString.Substring(4)
                dtRow("保担ＳＣ") = dstOrders1.Tables(0).Rows(0).Item("保守担当").ToString
            End If

            'dtRow("代理店") = Me.txtAgency.ppText
            If ddlAgency.ppSelectedValue = String.Empty Then
                dtRow("代理店") = String.Empty
            Else
                'dtRow("代理店") = ddlAgency.ppDropDownList.SelectedItem.ToString.Substring(ddlAgency.ppDropDownList.SelectedItem.ToString.IndexOf("：") + 1)
                dtRow("代理店") = dstOrders1.Tables(0).Rows(0).Item("代理店名").ToString
            End If

            'dtRow("統括ＳＣ") = Me.txtReviewSC.ppText
            If ddlReviewSC.ppSelectedValue = String.Empty Then
                dtRow("統括ＳＣ") = String.Empty
            Else
                'dtRow("統括ＳＣ") = ddlReviewSC.ppDropDownList.SelectedItem.ToString.Substring(4)
                dtRow("統括ＳＣ") = dstOrders1.Tables(0).Rows(0).Item("統括名").ToString
            End If
            'COMMENP006-002 END
            dtRow("ＬＡＮ担当ＩＤ") = Me.txtLANResp.ppText
            dtRow("ＬＡＮ担当情報") = Me.txtLANName.Text
            dtRow("住所(保担ＳＣ)") = Me.txtScAdd.ppText
            dtRow("ＴＥＬ(保担ＳＣ)") = Me.txtSecChaTel.ppText
            dtRow("ＦＡＸ(保担ＳＣ)") = Me.txtScFax.ppText
            dtRow("支店名(統括ＳＣ)") = Me.txtBranchNm.ppText
            dtRow("住所(統括ＳＣ)") = Me.txtReviewAdd.ppText
            dtRow("ＴＥＬ(統括ＳＣ)") = Me.txtReviewTel.ppText
            dtRow("ＦＡＸ(統括ＳＣ)") = Me.txtReviewFax.ppText
            dtRow("住所(代理店)") = Me.txtAgencyAdd.ppText
            dtRow("ＴＥＬ(代理店)") = Me.txtAgencyTel.ppText
            dtRow("ＦＡＸ(代理店)") = Me.txtAgencyFax.ppText
            dtRow("出力年月日") = DateTime.Now.ToString

            'データテーブルに作成したデータを設定
            dtPrint.Rows.Add(dtRow)

            'Active Reports(帳票 ホール管理データ)の起動
            Try

                rpt = New MNTREP005
                psPrintPDF(Me, rpt, dtPrint, strFNm)

            Catch ex As Exception
                '帳票の出力に失敗
                psMesBox(Me, "00008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            End Try

        Catch ex As SqlException

            '帳票情報の取得に失敗
            psMesBox(Me, "00008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Catch ex As Exception

            'システムエラー
            psMesBox(Me, "00008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally

            '終了ログ出力
            psLogEnd(Me)

            'データセット、データテーブルの破棄
            '            dtPrint.Clear()
            '            dtPrint.Dispose()
            Call psDisposeDataSet(dstOrders1)

            'DB切断
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If

        End Try

    End Sub

    ''' <summary>
    ''' 更新ボタン押下処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnUpd_Click()

        Dim conDB As New SqlConnection                    '接続
        Dim cmdDB As New SqlCommand
        Dim trans As SqlClient.SqlTransaction             'トランザクション
        Dim strView() As String = Nothing                 'ビューステートの情報
        Dim strNum() As String = Nothing                  'ビューステートの情報(連番)
        Dim strDdluse(3) As String

        objStack = New StackFrame

        Try

            '開始ログ出力
            psLogStart(Me)

            'COMMENP006-001
            If Me.DDLUseCd1.SelectedIndex = 0 Then
                strDdluse(0) = String.Empty
            Else
                strDdluse(0) = DDLUseCd1.SelectedItem.ToString.Substring(5)
            End If

            If Me.DDLUseCd2.SelectedIndex = 0 Then
                strDdluse(1) = String.Empty
            Else
                strDdluse(1) = DDLUseCd2.SelectedItem.ToString.Substring(5)
            End If

            If Me.DDLUseCd3.SelectedIndex = 0 Then
                strDdluse(2) = String.Empty
            Else
                strDdluse(2) = DDLUseCd3.SelectedItem.ToString.Substring(5)
            End If

            If Me.DDLUseCd4.SelectedIndex = 0 Then
                strDdluse(3) = String.Empty
            Else
                strDdluse(3) = DDLUseCd4.SelectedItem.ToString.Substring(5)
            End If
            'COMMENP006-001

            '用途内容とホールTEL、注意情報の整合性チェック
            If mfCheck_Integrity(strDdluse(0) _
                               , strDdluse(1) _
                               , strDdluse(2) _
                               , strDdluse(3)) = False Then
                '入力値チェックに失敗
                Exit Sub
            End If

            '入力チェックがすべて正常である場合、確認ポップアップを表示
            If (Page.IsValid) Then
                'DB接続
                If Not clsDataConnect.pfOpen_Database(conDB) Then

                    Throw New Exception

                End If

                'トランザクションの設定
                trans = conDB.BeginTransaction

                Try

                    'ViewStateから情報を取得
                    strView = Me.ViewState(P_KEY)

                    'ViewStateから情報を取得(連番)
                    'strNum = Me.ViewState(P_NUM) 
                    strNum = {"1", "2", "3,", "4"} 'COMMENP006-003


                    'TBOX・ホール情報の更新
                    cmdDB = New SqlCommand(M_DISP_ID & "_U2", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = trans
                    If mfUpd_Data_Main(cmdDB _
                                     , strView _
                                     , conDB _
                                     , trans) = False Then
                        Exit Sub
                    End If

                    '用途内容、コードの更新/追加(用途1)
                    cmdDB = New SqlCommand(M_DISP_ID & "_U1", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = trans
                    '--2014/04/21 中川　ここから
                    If mfUpd_Data_yoto(strView(0) _
                                    , strView(3) _
                                    , Me.txtHoleTel2.ppText.Trim _
                                    , strDdluse(0) _
                                    , Me.DDLUseCd1.SelectedValue.ToString.Trim _
                                    , strNum(0) _
                                    , cmdDB _
                                    , conDB _
                                    , trans) = False Then '[引数4:Me.DDLUseCd1.SelectedItem.ToString.Trim]を変更 'COMMENP006-001
                        Exit Sub
                    End If
                    '--2014/04/21 中川　ここまで

                    '用途内容、コードの更新/追加(用途2)
                    cmdDB = New SqlCommand(M_DISP_ID & "_U1", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = trans
                    '--2014/04/21 中川　ここから
                    If mfUpd_Data_yoto(strView(0) _
                                   , strView(3) _
                                   , Me.txtHoleTel3.ppText.Trim _
                                   , strDdluse(1) _
                                   , Me.DDLUseCd2.SelectedItem.ToString.Trim _
                                   , strNum(1) _
                                   , cmdDB _
                                   , conDB _
                                   , trans) = False Then
                        Exit Sub
                    End If
                    '--2014/04/21 中川　ここまで

                    '用途内容、コードの更新/追加(用途3)
                    cmdDB = New SqlCommand(M_DISP_ID & "_U1", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = trans
                    If mfUpd_Data_yoto(strView(0) _
                                  , strView(3) _
                                  , Me.txtHoleTel4.ppText.Trim _
                                  , strDdluse(2) _
                                  , Me.DDLUseCd3.SelectedItem.ToString.Trim _
                                  , strNum(2) _
                                  , cmdDB _
                                  , conDB _
                                  , trans) = False Then
                        Exit Sub
                    End If

                    '用途内容、コードの更新/追加(用途4)
                    cmdDB = New SqlCommand(M_DISP_ID & "_U1", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = trans
                    '--2014/04/21 中川　ここから
                    If mfUpd_Data_yoto(strView(0) _
                                  , strView(3) _
                                  , Me.txtHoleTel5.ppText.Trim _
                                  , strDdluse(3) _
                                  , Me.DDLUseCd4.SelectedItem.ToString _
                                  , strNum(3) _
                                  , cmdDB _
                                  , conDB _
                                  , trans) = False Then
                        Exit Sub
                    End If
                    '--2014/04/21 中川　ここまで

                    'COMMENP006-002 DropDownListの初期化
                    If ddlSecChaSC.ppDropDownList.SelectedIndex <> 0 Then
                        Me.ddlSecChaSC.ppDropDownList.Items.Item(0).Value = String.Empty
                        Me.ddlSecChaSC.ppDropDownList.Items.Item(0).Text = String.Empty
                        ViewState("保担") = Nothing
                    End If
                    If ddlReviewSC.ppDropDownList.SelectedIndex <> 0 Then
                        Me.ddlReviewSC.ppDropDownList.Items.Item(0).Value = String.Empty
                        Me.ddlReviewSC.ppDropDownList.Items.Item(0).Text = String.Empty
                        ViewState("統括") = Nothing
                    End If
                    If ddlAgency.ppDropDownList.SelectedIndex <> 0 Then
                        Me.ddlAgency.ppDropDownList.Items.Item(0).Value = String.Empty
                        Me.ddlAgency.ppDropDownList.Items.Item(0).Text = String.Empty
                        ViewState("代理店") = Nothing
                    End If
                    'COMMENP006-002 END

                    'コミットの発行
                    trans.Commit()
                    '破棄
                    trans.Dispose()
                    cmdDB.Dispose()

                    'Viewstateのホールコード更新 'COMMENP006-013
                    'ViewState(P_KEY)(3) = txtHoleCd.ppText

                    '正常終了メッセージ
                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, P_HALLINF)

                    strBtnSts = "1"

                    '画面更新 COMMENP006-005
                    msGet_Data_Sel("1")

                Catch ex As SqlException

                    '更新に失敗
                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, P_HALLINF)
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")

                    If Not conDB Is Nothing Then

                        trans.Rollback()

                    End If

                Catch ex As Exception

                    'システムエラー
                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, P_HALLINF)
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                End Try
            End If
        Catch ex As Exception

            'システムエラー
            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, P_HALLINF)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally

            'DB切断
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If

            ''ドロップダウンリストの値設定
            'Me.DDLUseCd1.SelectedIndex = Me.DDLUseCd1.SelectedIndex
            'Me.DDLUseCd2.SelectedIndex = Me.DDLUseCd2.SelectedIndex
            'Me.DDLUseCd3.SelectedIndex = Me.DDLUseCd3.SelectedIndex
            'Me.DDLUseCd4.SelectedIndex = Me.DDLUseCd4.SelectedIndex

            'COMMENP006-001
            ''用途内容コントロールの制御
            'If Me.DDLUseCd1.SelectedItem.ToString <> "99" Then
            '    Me.txtUseCtt1.ppText = strDdluse(0)
            '    Me.txtUseCtt1.ppEnabled = False
            'Else
            '    Me.txtUseCtt1.ppText = strDdluse(0)
            '    Me.txtUseCtt1.ppEnabled = True
            'End If

            'If Me.DDLUseCd2.SelectedItem.ToString <> "99" Then
            '    Me.txtUseCtt2.ppText = strDdluse(1)
            '    Me.txtUseCtt2.ppEnabled = False
            'Else
            '    Me.txtUseCtt2.ppText = strDdluse(1)
            '    Me.txtUseCtt2.ppEnabled = True
            'End If

            'If Me.DDLUseCd3.SelectedItem.ToString <> "99" Then
            '    Me.txtUseCtt3.ppText = strDdluse(2)
            '    Me.txtUseCtt3.ppEnabled = False
            'Else
            '    Me.txtUseCtt3.ppText = strDdluse(2)
            '    Me.txtUseCtt3.ppEnabled = True
            'End If

            'If Me.DDLUseCd4.SelectedItem.ToString <> "99" Then
            '    Me.txtUseCtt4.ppText = strDdluse(3)
            '    Me.txtUseCtt4.ppEnabled = False
            'Else
            '    Me.txtUseCtt4.ppText = strDdluse(3)
            '    Me.txtUseCtt4.ppEnabled = True
            'End If
            'COMMENP006-001 END

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub

    ''' <summary>
    ''' 情報取得/設定処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data_Sel(ByVal strFlag As String)
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders1 As New DataSet        '表示情報のデータセット
        Dim dstOrders2 As New DataSet        '用途テーブルのデータセット
        Dim strView() As String = Nothing    'ビューステート登録用の情報
        Dim strNum(4 - 1) As String          'ビューステート登録用の情報(連番)
        Dim strOKNG As String = Nothing
        Dim maxVal As Integer = 0
        Dim HDDcnt As Integer = 0            'HDD1～4のカウンタ
        Dim strMDNs As New List(Of String)   'MDN機器名格納

        objStack = New StackFrame

        Try

            '開始ログ出力
            psLogStart(Me)

            'strNumの初期化
            For i As Integer = 0 To strNum.Length - 1

                strNum(i) = "0"

            Next

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Throw New Exception
            End If

            'ViewStateから情報を取得
            strView = Me.ViewState(P_KEY)

            cmdDB = New SqlCommand(M_DISP_ID & "_S1", conDB) 'すべての表示情報を取得
            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("prm_tboxid", SqlDbType.NVarChar, strView(0).ToString))             'TBOXID
                .Add(pfSet_Param("prm_nl", SqlDbType.NVarChar, strView(1).ToString))                 'NL区分
                .Add(pfSet_Param("prm_system", SqlDbType.NVarChar, strView(2).ToString))             'システム分類
                .Add(pfSet_Param("prm_hallcd", SqlDbType.NVarChar, strView(3).ToString))             'ホールコード
                .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))   '結果取得用
            End With

            'リストデータ取得
            dstOrders1 = clsDataConnect.pfGet_DataSet(cmdDB)

            '結果情報を取得
            strOKNG = cmdDB.Parameters("data_exist").Value.ToString

            'データがない場合
            If strOKNG = "0" Then
                'Throw New Exception
                Me.Master.ppRigthButton2.Enabled = False
                Me.Master.ppRigthButton4.Enabled = False
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前)
                Exit Sub
            End If

            '用途マスタが存在しないため保留
            cmdDB = New SqlCommand(M_DISP_ID & "_S2", conDB) '用途コード

            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))   '結果取得用
            End With

            'データがない場合
            If strOKNG = "0" Then
                'Throw New Exception
                Me.Master.ppRigthButton2.Enabled = False
                Me.Master.ppRigthButton4.Enabled = False
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前)
                Exit Sub
            End If

            'リストデータ取得
            dstOrders2 = clsDataConnect.pfGet_DataSet(cmdDB)

            '項目にデータ設定
            '取得したデータをリストに設定
            Me.DDLUseCd1.DataSource = dstOrders2.Tables(0)
            Me.DDLUseCd2.DataSource = dstOrders2.Tables(0)
            Me.DDLUseCd3.DataSource = dstOrders2.Tables(0)
            Me.DDLUseCd4.DataSource = dstOrders2.Tables(0)

            '用途コードの設定(ドロップダウンリストに表示する)
            Me.DDLUseCd1.DataTextField = dstOrders2.Tables(0).Columns(1).ColumnName.ToString
            Me.DDLUseCd2.DataTextField = dstOrders2.Tables(0).Columns(1).ColumnName.ToString
            Me.DDLUseCd3.DataTextField = dstOrders2.Tables(0).Columns(1).ColumnName.ToString
            Me.DDLUseCd4.DataTextField = dstOrders2.Tables(0).Columns(1).ColumnName.ToString

            '用途内容の設定(テキストボックスに表示する)
            Me.DDLUseCd1.DataValueField = dstOrders2.Tables(0).Columns(0).ColumnName.ToString
            Me.DDLUseCd2.DataValueField = dstOrders2.Tables(0).Columns(0).ColumnName.ToString
            Me.DDLUseCd3.DataValueField = dstOrders2.Tables(0).Columns(0).ColumnName.ToString
            Me.DDLUseCd4.DataValueField = dstOrders2.Tables(0).Columns(0).ColumnName.ToString

            'ドロップダウンリストにデータをバインド
            Me.DDLUseCd1.DataBind()
            Me.DDLUseCd2.DataBind()
            Me.DDLUseCd3.DataBind()
            Me.DDLUseCd4.DataBind()

            Me.DDLUseCd1.Items.Insert(0, "")
            Me.DDLUseCd2.Items.Insert(0, "")
            Me.DDLUseCd3.Items.Insert(0, "")
            Me.DDLUseCd4.Items.Insert(0, "")

            Me.DDLUseCd1.SelectedIndex = 0
            Me.DDLUseCd2.SelectedIndex = 0
            Me.DDLUseCd3.SelectedIndex = 0
            Me.DDLUseCd4.SelectedIndex = 0

            '--2014/04/21 中川　ここから
            'ホール連絡先情報を初期化
            Me.txtHoleTel2.ppText = Nothing
            Me.txtHoleTel3.ppText = Nothing
            Me.txtHoleTel4.ppText = Nothing
            Me.txtHoleTel5.ppText = Nothing
            '--2014/04/21 中川　ここまで

            'COMMENP006-2
            cmdDB = New SqlCommand("COMMENP006_S3", conDB)
            'パラメータ設定
            cmdDB.Parameters.Add(pfSet_Param("mnt_cd", SqlDbType.NVarChar, dstOrders1.Tables(0).Rows(0).Item("保守担当コード").ToString)) '保担コード
            cmdDB.Parameters.Add(pfSet_Param("retvalue", SqlDbType.NVarChar)) '戻り値

            Using dstOrders3 As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)
                '保担
                ddlSecChaSC.ppDropDownList.DataSource = dstOrders3.Tables(0)
                ddlSecChaSC.ppDropDownList.DataTextField = "営業所"
                ddlSecChaSC.ppDropDownList.DataValueField = "営業所コード"
                ddlSecChaSC.ppDropDownList.DataBind()
                ddlSecChaSC.ppDropDownList.Items.Insert(0, "")
                ddlSecChaSC.ppDropDownList.SelectedIndex = -1
                '統括
                ddlReviewSC.ppDropDownList.DataSource = dstOrders3.Tables(1)
                ddlReviewSC.ppDropDownList.DataTextField = "営業所"
                ddlReviewSC.ppDropDownList.DataValueField = "営業所コード"
                ddlReviewSC.ppDropDownList.DataBind()
                ddlReviewSC.ppDropDownList.Items.Insert(0, "")
                ddlReviewSC.ppDropDownList.SelectedIndex = -1
                '代行店
                ddlAgency.ppDropDownList.DataSource = dstOrders3.Tables(2)
                ddlAgency.ppDropDownList.DataTextField = "代理店"
                ddlAgency.ppDropDownList.DataValueField = "代理店コード"
                ddlAgency.ppDropDownList.DataBind()
                ddlAgency.ppDropDownList.Items.Insert(0, "")
                ddlAgency.ppDropDownList.SelectedIndex = -1
            End Using
            'COMMENP006-002 END


            '取得データを各コントロールに設定する
            For i As Integer = 0 To dstOrders1.Tables(0).Rows.Count - 1
                Select Case dstOrders1.Tables(0).Rows(i).Item("分類").ToString
                    Case "1"
                        msSetDropDown(dstOrders1.Tables(0).Rows(i)) 'COMMENP006-002 ドロップダウン設定
                        Me.txtTboxId.ppText = dstOrders1.Tables(0).Rows(i).Item("TBOXID").ToString
                        Me.tbxNLSec.Text = dstOrders1.Tables(0).Rows(i).Item("NL区分").ToString
                        Me.txtSystem.ppText = dstOrders1.Tables(0).Rows(i).Item("システム").ToString
                        Me.txtVER.ppText = dstOrders1.Tables(0).Rows(i).Item("VER").ToString
                        Me.hdnSystem.Value = dstOrders1.Tables(0).Rows(i).Item("ホール名").ToString  'COMMENP006-006 システムコード(修正の都合上カラムがホール名)
                        Me.txtTboxTel.ppText = dstOrders1.Tables(0).Rows(i).Item("TBOXTEL").ToString
                        Me.txtOptSttDate.ppText = dstOrders1.Tables(0).Rows(i).Item("運用開始日").ToString
                        Me.txtMDNInstallation.ppText = dstOrders1.Tables(0).Rows(i).Item("MDN設置").ToString
                        Me.txtScAdd.ppText = dstOrders1.Tables(0).Rows(i).Item("保守担当住所").ToString
                        Me.txtScFax.ppText = dstOrders1.Tables(0).Rows(i).Item("保守担当ＦＡＸ番号").ToString
                        Me.txtLANResp.ppText = dstOrders1.Tables(0).Rows(i).Item("LAN担当コード").ToString
                        Me.txtLANName.Text = dstOrders1.Tables(0).Rows(i).Item("LAN担当名").ToString
                        Me.txtSecChaTel.ppText = dstOrders1.Tables(0).Rows(i).Item("保守担当電話番号").ToString
                        Me.txtBranchNm.ppText = dstOrders1.Tables(0).Rows(i).Item("統括支店名").ToString
                        Me.txtReviewAdd.ppText = dstOrders1.Tables(0).Rows(i).Item("統括住所").ToString
                        Me.txtReviewTel.ppText = dstOrders1.Tables(0).Rows(i).Item("統括TEL").ToString
                        Me.txtReviewFax.ppText = dstOrders1.Tables(0).Rows(i).Item("統括FAX").ToString
                        Me.txtAgencyAdd.ppText = dstOrders1.Tables(0).Rows(i).Item("代理店住所").ToString
                        Me.txtAgencyTel.ppText = dstOrders1.Tables(0).Rows(i).Item("代理店TEL").ToString
                        Me.txtAgencyFax.ppText = dstOrders1.Tables(0).Rows(i).Item("代理店FAX").ToString
                        Me.txtConSalDep.ppText = dstOrders1.Tables(0).Rows(i).Item("担当営業部").ToString
                        Me.txtSdTel.ppText = dstOrders1.Tables(0).Rows(i).Item("担当営業部TEL").ToString

                        Me.txtWrkEndDate.ppText = dstOrders1.Tables(0).Rows(i).Item("集信日").ToString     'COMMENP006-011 運用終了日
                        Me.hdnWrkEndDate.Value = dstOrders1.Tables(0).Rows(i).Item("集信日").ToString      'COMMENP006-011 運用終了日
                        msSetPerCls(dstOrders1.Tables(0).Rows(i).Item("ZIPNO").ToString)                   'COMMENP006-011 運用状況
                        hdnPerCls.Value = dstOrders1.Tables(0).Rows(i).Item("ZIPNO").ToString              'COMMENP006-011 運用状況(Hidden)
                    Case "2"
                        Me.txtHoleNm.ppText = dstOrders1.Tables(0).Rows(i).Item("ホール名").ToString
                        Me.txtHoleCd.ppText = dstOrders1.Tables(0).Rows(i).Item("ホールコード").ToString
                        Me.hdnHallCd.Value = dstOrders1.Tables(0).Rows(i).Item("ホールコード").ToString
                        Me.txtCcrtDate.ppText = dstOrders1.Tables(0).Rows(i).Item("集信日").ToString
                        Me.txtNotes.ppText = dstOrders1.Tables(0).Rows(i).Item("注意事項").ToString
                        Me.txtZip.ppText = dstOrders1.Tables(0).Rows(i).Item("ZIPNO").ToString 'COMMENP006-004
                        Me.txtHoleAdd.ppText = dstOrders1.Tables(0).Rows(i).Item("住所").ToString.Trim
                        Me.txtHoleTel1.ppText = dstOrders1.Tables(0).Rows(i).Item("ホールTEL").ToString
                        Me.txtEWSec.ppText = dstOrders1.Tables(0).Rows(i).Item("東西フラグ").ToString
                        Me.txtFaxNo.ppText = dstOrders1.Tables(0).Rows(i).Item("FAX").ToString
                        Me.tbxStoreType.Text = dstOrders1.Tables(0).Rows(i).Item("店舗種別").ToString
                        '--------------------------------
                        '2014/05/23 稲葉　ここから
                        '--------------------------------
                        'If Me.txtMDNInstallation.ppText = "有" Then
                        '    'Me.txtPrtNumberOfMDN.ppText = dstOrders1.Tables(0).Rows(i).Item("MDN台数").ToString
                        'Me.txtNumberOfMDN.ppText = dstOrders1.Tables(0).Rows(i).Item("MDN台数").ToString
                        'Else
                        '    'Me.txtPrtNumberOfMDN.ppText = "0"
                        '    Me.txtNumberOfMDN.ppText = "0"
                        'End If
                        '--------------------------------
                        '2014/05/23 稲葉　ここまで
                        '--------------------------------
                    Case "3"
                        '--------------------------------
                        '2014/05/23 稲葉　ここから
                        '--------------------------------
                        'Select Case dstOrders1.Tables(0).Rows(i).Item("親子連番").ToString
                        '    Case "0" '親
                        '        Me.txtPrtNumberOfMDN.ppText = dstOrders1.Tables(0).Rows(i).Item("親MDN台数").ToString
                        '    Case "1" '子
                        '        Me.txtChd1NumberOfMDN.ppText = dstOrders1.Tables(0).Rows(i).Item("子MDN台数").ToString
                        '    Case "2" 'ここは通らない
                        '        Me.txtChd2NumberOfMDN.ppText = dstOrders1.Tables(0).Rows(i).Item("子MDN台数").ToString
                        'End Select
                        'If dstOrders1.Tables(0).Rows(i).Item("ホールコード").ToString = txtHoleCd.ppText Then
                        '    Me.txtNumberOfMDN.ppText = dstOrders1.Tables(0).Rows(i).Item("子MDN台数").ToString
                        'End If
                        Select Case dstOrders1.Tables(0).Rows(i).Item("親子連番").ToString
                            Case "1"
                                Me.txtNumberOfMDN.ppText = dstOrders1.Tables(0).Rows(i).Item("MDN台数").ToString
                                Me.txtPrtNumberOfMDN.ppText = dstOrders1.Tables(0).Rows(i).Item("親MDN台数").ToString
                                'COMMENP006-010 初期値0設定
                                Me.txtChd1NumberOfMDN.ppText = "0"
                                Me.txtChd2NumberOfMDN.ppText = "0"
                                'COMMENP006-010 END
                            Case "2"
                                Me.txtChd1NumberOfMDN.ppText = dstOrders1.Tables(0).Rows(i).Item("子MDN台数").ToString
                            Case "3"
                                Me.txtChd2NumberOfMDN.ppText = dstOrders1.Tables(0).Rows(i).Item("子MDN台数").ToString
                        End Select
                        '--------------------------------
                        '2014/05/23 稲葉　ここまで
                        '--------------------------------
                    Case "4"
                        Select Case dstOrders1.Tables(0).Rows(i).Item("機器").ToString
                            Case "01" '制御部'
                                Me.txtTboxSerial.ppText = dstOrders1.Tables(0).Rows(i).Item("機器種別").ToString
                            Case "03" '操作盤'
                                Me.txtOppSerial.ppText = dstOrders1.Tables(0).Rows(i).Item("機器種別").ToString
                            Case "06" 'UPS'
                                Me.txtUPSSerial.ppText = dstOrders1.Tables(0).Rows(i).Item("機器種別").ToString
                            Case "04" 'プリンタ'
                                Me.txtPrtSerial.ppText = dstOrders1.Tables(0).Rows(i).Item("機器種別").ToString
                            Case "02" 'CRT'
                                Me.txtCRTSerial.ppText = dstOrders1.Tables(0).Rows(i).Item("機器種別").ToString
                            Case "09" 'HDD'
                                HDDcnt += 1
                                Select Case HDDcnt
                                    Case 1
                                        Me.txtHDD1Serial.ppText = dstOrders1.Tables(0).Rows(i).Item("機器種別").ToString
                                    Case 2
                                        Me.txtHDD2Serial.ppText = dstOrders1.Tables(0).Rows(i).Item("機器種別").ToString
                                    Case 3
                                        Me.txtHDD3Serial.ppText = dstOrders1.Tables(0).Rows(i).Item("機器種別").ToString
                                    Case 4
                                        Me.txtHDD4Serial.ppText = dstOrders1.Tables(0).Rows(i).Item("機器種別").ToString
                                End Select
                        End Select
                    Case "5"
                        '--------------------------------
                        '2014/05/23 稲葉　ここから
                        '--------------------------------
                        Select Case dstOrders1.Tables(0).Rows(i).Item("機器").ToString
                            Case "12" 'SC
                                Me.txtSC.ppText = dstOrders1.Tables(0).Rows(i).Item("機器数").ToString
                            Case "13" 'CC
                                Me.txtCC.ppText = dstOrders1.Tables(0).Rows(i).Item("機器数").ToString
                            Case "21" 'サンド
                                Me.txtSand.ppText = dstOrders1.Tables(0).Rows(i).Item("機器数").ToString
                            Case "22" '券売機
                                Me.txtTVMachine.ppText = dstOrders1.Tables(0).Rows(i).Item("機器数").ToString
                            Case "23" '精算機
                                Me.txtAmMachine.ppText = dstOrders1.Tables(0).Rows(i).Item("機器数").ToString
                        End Select
                        'Select Case dstOrders1.Tables(0).Rows(i).Item("機器").ToString
                        '    Case "SC"
                        '        Me.txtSC.ppText = dstOrders1.Tables(0).Rows(i).Item("機器数").ToString
                        '    Case "CC"
                        '        Me.txtCC.ppText = dstOrders1.Tables(0).Rows(i).Item("機器数").ToString
                        '    Case "サンド"
                        '        Me.txtSand.ppText = dstOrders1.Tables(0).Rows(i).Item("機器数").ToString
                        '    Case "券売機"
                        '        Me.txtTVMachine.ppText = dstOrders1.Tables(0).Rows(i).Item("機器数").ToString
                        '    Case "精算機"
                        '        Me.txtAmMachine.ppText = dstOrders1.Tables(0).Rows(i).Item("機器数").ToString
                        'End Select
                        '--------------------------------
                        '2014/05/23 稲葉　ここまで
                        '--------------------------------
                    Case "6"
                        Select Case dstOrders1.Tables(0).Rows(i).Item("定型文連番").ToString
                            Case "1"
                                Me.txtNotes1.ppText = dstOrders1.Tables(0).Rows(i).Item("定型文").ToString
                            Case "2"
                                Me.txtNotes2.ppText = dstOrders1.Tables(0).Rows(i).Item("定型文").ToString
                            Case "3"
                                Me.txtNotes3.ppText = dstOrders1.Tables(0).Rows(i).Item("定型文").ToString
                            Case "4"
                                Me.txtNotes4.ppText = dstOrders1.Tables(0).Rows(i).Item("定型文").ToString
                        End Select
                    Case "7"
                        'Select Case dstOrders1.Tables(0).Rows(i).Item("用途連番").ToString 'COMMENP006-003
                        Select Case dstOrders1.Tables(0).Rows(i).Item("連番").ToString
                            Case "1"
                                Me.txtHoleTel2.ppText = dstOrders1.Tables(0).Rows(i).Item("ホールTEL").ToString
                                'COMMENP006-001
                                'Me.txtUseCtt1.ppText = dstOrders1.Tables(0).Rows(i).Item("用途内容").ToString 
                                'Me.DDLUseCd1.SelectedIndex = Index_Sel(Me.DDLUseCd1, dstOrders1.Tables(0).Rows(i).Item("用途内容").ToString)
                                Me.DDLUseCd1.SelectedValue = dstOrders1.Tables(0).Rows(i).Item("用途コード").ToString
                                'COMMENP006-001 END
                                strNum(0) = dstOrders1.Tables(0).Rows(i).Item("連番").ToString
                            Case "2"
                                Me.txtHoleTel3.ppText = dstOrders1.Tables(0).Rows(i).Item("ホールTEL").ToString
                                'COMMENP006-001
                                'Me.txtUseCtt2.ppText = dstOrders1.Tables(0).Rows(i).Item("用途内容").ToString
                                'Me.DDLUseCd2.SelectedIndex = Index_Sel(Me.DDLUseCd1, dstOrders1.Tables(0).Rows(i).Item("用途コード").ToString)
                                Me.DDLUseCd2.SelectedValue = dstOrders1.Tables(0).Rows(i).Item("用途コード").ToString
                                'COMMENP006-001 END
                                strNum(1) = dstOrders1.Tables(0).Rows(i).Item("連番").ToString
                            Case "3"
                                Me.txtHoleTel4.ppText = dstOrders1.Tables(0).Rows(i).Item("ホールTEL").ToString
                                'COMMENP006-001
                                'Me.txtUseCtt3.ppText = dstOrders1.Tables(0).Rows(i).Item("用途内容").ToString 
                                'Me.DDLUseCd3.SelectedIndex = Index_Sel(Me.DDLUseCd1, dstOrders1.Tables(0).Rows(i).Item("用途コード").ToString)
                                Me.DDLUseCd3.SelectedValue = dstOrders1.Tables(0).Rows(i).Item("用途コード").ToString
                                'COMMENP006-001 END
                                strNum(2) = dstOrders1.Tables(0).Rows(i).Item("連番").ToString
                            Case "4"
                                Me.txtHoleTel5.ppText = dstOrders1.Tables(0).Rows(i).Item("ホールTEL").ToString
                                'COMMENP006-001
                                'Me.txtUseCtt4.ppText = dstOrders1.Tables(0).Rows(i).Item("用途内容").ToString
                                'Me.DDLUseCd4.SelectedIndex = Index_Sel(Me.DDLUseCd1, dstOrders1.Tables(0).Rows(i).Item("用途コード").ToString)
                                Me.DDLUseCd4.SelectedValue = dstOrders1.Tables(0).Rows(i).Item("用途コード").ToString
                                'COMMENP006-001 END
                                strNum(3) = dstOrders1.Tables(0).Rows(i).Item("連番").ToString
                        End Select
                    Case "8"
                        'COMMENP006-009
                        'MDN機器名を配列に保存
                        If dstOrders1.Tables(0).Rows(i).Item("ＭＤＮ機器名").ToString <> String.Empty Then
                            strMDNs.Add(dstOrders1.Tables(0).Rows(i).Item("ＭＤＮ機器名").ToString)
                            strMDNs.Add(", ")
                        End If
                        'COMMENP006-009 END
                End Select
            Next
            'COMMENP006-009
            'MDN機器名初期化
            Me.txtMDN.ppText = String.Empty
            'MDN機器名を表示
            If strMDNs.Count > 0 Then
                strMDNs.RemoveAt(strMDNs.Count - 1)
                For Each MDNname As String In strMDNs
                    txtMDN.ppText &= MDNname
                Next
            End If
            'COMMENP006-009 END

            'COMMENP006-001
            ''用途内容コントロールの制御
            'If Not Me.DDLUseCd1.SelectedItem.ToString = "99" Then
            '    Me.txtUseCtt1.ppEnabled = False
            'End If

            'If Not Me.DDLUseCd2.SelectedItem.ToString = "99" Then
            '    Me.txtUseCtt2.ppEnabled = False
            'End If

            'If Not Me.DDLUseCd3.SelectedItem.ToString = "99" Then
            '    Me.txtUseCtt3.ppEnabled = False
            'End If

            'If Not Me.DDLUseCd4.SelectedItem.ToString = "99" Then
            '    Me.txtUseCtt4.ppEnabled = False
            'End If
            'COMMENP006-001 END

        Catch ex As SqlException

            '--2014/04/21 中川　ここから
            If strFlag = "0" Then   '初期表示の場合
                '検索取得エラー
                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            End If

            If strFlag = "1" Then   '元に戻すの場合
                '検索取得エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, P_HALLINF)
            End If
            '--2014/04/21 中川　ここまで

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

            'DB切断
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If

            If strFlag = "0" Then
                '排他削除
                clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                psClose_Window(Me)
            End If

        Catch ex As Exception
            '--2014/04/21 中川　ここから
            If strFlag = "0" Then   '初期表示の場合
                '検索取得エラー
                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            End If

            If strFlag = "1" Then   '元に戻すの場合
                '検索取得エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, P_HALLINF)
            End If
            '--2014/04/21 中川　ここまで

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            'DB切断
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If

            '--2014/04/21 中川　ここから
            If strFlag = "0" Then
                psClose_Window(Me)
            End If

            'システムエラー
            'Throw ex
            '--2014/04/21 中川　ここまで

        Finally
            'Dispose
            cmdDB.Dispose()
            clsSqlDbSvr.psDisposeDataSet(dstOrders1)
            clsSqlDbSvr.psDisposeDataSet(dstOrders2)

            '連番の設定
            Me.ViewState.Add(P_NUM, strNum)

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub

    ''' <summary>
    ''' 用途内容更新
    ''' </summary>
    ''' <param name="strHoleCD"></param>
    ''' <param name="strHoleTel"></param>
    ''' <param name="strUseCtt"></param>
    ''' <param name="strDDLUseCd"></param>
    ''' <param name="strSeq"></param>
    ''' <param name="cmdDB"></param>
    ''' <remarks></remarks>
    Private Function mfUpd_Data_yoto(ByVal strTboxId As String _
                                    , ByVal strHoleCD As String _
                                    , ByVal strHoleTel As String _
                                    , ByVal strUseCtt As String _
                                    , ByVal strDDLUseCd As String _
                                    , ByVal strSeq As String _
                                    , ByVal cmdDB As SqlCommand _
                                    , ByVal conDB As SqlConnection _
                                    , ByVal trans As SqlTransaction) As Boolean

        objStack = New StackFrame

        Try

            '開始ログ出力
            psLogStart(Me)

            'すべての項目がNothingの場合、処理を終了する
            If strHoleTel Is Nothing And strUseCtt Is Nothing And strSeq Is Nothing Then
                Return True
            End If

            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("prm_tboxid", SqlDbType.NVarChar, strTboxId))
                .Add(pfSet_Param("prm_hallcd", SqlDbType.NVarChar, strHoleCD))
                .Add(pfSet_Param("prm_seq", SqlDbType.Int, CInt(strSeq)))                      '連番
                .Add(pfSet_Param("prm_hallTel", SqlDbType.NVarChar, strHoleTel))               'ホールＴＥＬ
                .Add(pfSet_Param("prm_useCtt", SqlDbType.NVarChar, strUseCtt))                 '用途内容
                .Add(pfSet_Param("prm_UseCd", SqlDbType.NVarChar, strDDLUseCd))                '用途コード
                .Add(pfSet_Param("prm_userID", SqlDbType.NVarChar, Session(P_SESSION_USERID).ToString)) 'ユーザID
            End With

            'コマンドタイプ設定(ストアド)
            cmdDB.ExecuteNonQuery()

            Return True

        Catch ex As Exception

            '--2014/04/21 中川　ここから
            '更新に失敗
            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, P_HALLINF)
            '--2014/04/21 中川　ここまで

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

            '--2014/04/21 中川　ここから
            If Not conDB Is Nothing Then

                trans.Rollback()

            End If

            Return False

            'Throw ex
            '--2014/04/21 中川　ここまで

        Finally

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function

    ''' <summary>
    ''' ホール・TBOX更新
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfUpd_Data_Main(ByVal cmdDB As SqlCommand _
                                    , ByVal strView As String() _
                                    , ByVal conDB As SqlConnection _
                                    , ByVal trans As SqlTransaction) As Boolean
        objStack = New StackFrame

        Try

            '開始ログ出力
            psLogStart(Me)

            With cmdDB.Parameters   'パラメータ設定
                'COMMENP006-002
                '.Add(pfSet_Param("prm_wrncontent", SqlDbType.NVarChar, Me.txtNotes.ppText))        '注意事項
                '.Add(pfSet_Param("prm_userID", SqlDbType.NVarChar, Session(P_SESSION_USERID)))     'ユーザID
                '.Add(pfSet_Param("prm_hallcode", SqlDbType.NVarChar, HallCD))                      'ホールコード
                If Session(P_SESSION_AUTH) = "管理者" Then
                    .Add(pfSet_Param("IsUpdTbox", SqlDbType.SmallInt, 1))                           'TBOX情報更新有無
                    'IDの場合
                    If strView(2) = "1" Then
                        .Add(pfSet_Param("wrkend_dt", SqlDbType.NVarChar, txtWrkEndDate.ppText))        'COMMENP006-011 運用終了日
                        If ddlPerCls.ppSelectedValue <> String.Empty Then
                            .Add(pfSet_Param("per_cls", SqlDbType.NVarChar, ddlPerCls.ppSelectedValue)) 'COMMENP006-011 運用状況
                        End If
                    End If
                End If
                'ホールコードが変更されている場合
                If txtHoleCd.ppText <> hdnHallCd.Value Then                                     'COMMENP006-013
                    '.Add(pfSet_Param("@IsCngHall", SqlDbType.SmallInt, 1))                      'ホールコード更新有無
                End If
                .Add(pfSet_Param("userID", SqlDbType.NVarChar, Session(P_SESSION_USERID)))      'ユーザID
                .Add(pfSet_Param("tbox_id", SqlDbType.NVarChar, strView(0)))                    'TBOXID
                .Add(pfSet_Param("nl_cls", SqlDbType.NVarChar, strView(1)))                     'NL
                .Add(pfSet_Param("system_cls", SqlDbType.NVarChar, strView(2)))                 'システム分類
                .Add(pfSet_Param("Ver", SqlDbType.NVarChar, Me.txtVER.ppText))                  'Ver
                .Add(pfSet_Param("hall_name", SqlDbType.NVarChar, Me.txtHoleNm.ppText))         'ホール名
                .Add(pfSet_Param("hall_cd", SqlDbType.NVarChar, txtHoleCd.ppText))              'ホールコード
                .Add(pfSet_Param("hall_cd_old", SqlDbType.NVarChar, hdnHallCd.Value))           '旧ホールコード
                .Add(pfSet_Param("zip_no", SqlDbType.NVarChar, Me.txtZip.ppText))               '郵便番号
                .Add(pfSet_Param("address", SqlDbType.NVarChar, Me.txtHoleAdd.ppText))          '住所
                .Add(pfSet_Param("wrncontent", SqlDbType.NVarChar, Me.txtNotes.ppText))         '注意事項
                .Add(pfSet_Param("ngc_org", SqlDbType.NVarChar, Me.txtConSalDep.ppText))        'NGC担当
                .Add(pfSet_Param("ngc_tell", SqlDbType.NVarChar, Me.txtSdTel.ppText))           'NGC担当TEL
                If Me.ddlSecChaSC.ppSelectedValue <> DDL_VALUE Then
                    .Add(pfSet_Param("mnt_cd", SqlDbType.NVarChar, Me.ddlSecChaSC.ppSelectedValue)) '保担コード
                End If
                If Me.ddlReviewSC.ppSelectedValue <> DDL_VALUE Then
                    .Add(pfSet_Param("unf_cd", SqlDbType.NVarChar, Me.ddlReviewSC.ppSelectedValue)) '統括コード
                End If
                If Me.ddlAgency.ppSelectedValue <> DDL_VALUE Then
                    .Add(pfSet_Param("agc_cd", SqlDbType.NVarChar, Me.ddlAgency.ppSelectedValue))   '代理店コード
                End If
                'If ddlAgency.ppDropDownList.SelectedIndex = 0 AndAlso Not ViewState("代理店") Is Nothing Then
                '    .Add(pfSet_Param("agc_cd", SqlDbType.NVarChar, String.Empty))                   '代理店コード
                'Else
                '    .Add(pfSet_Param("agc_cd", SqlDbType.NVarChar, Me.ddlAgency.ppSelectedValue))   '代理店コード
                'End If
                'COMMENP006-002 END
            End With
            'コマンドタイプ設定(ストアド)
            cmdDB.ExecuteNonQuery()

            Return True

        Catch ex As Exception

            '--2014/04/21 中川　ここから
            'システムエラー
            'psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "注意事項") COMMENP006-002 
            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, P_HALLINF)  'COMMENP006-002 END
            '--2014/04/21 中川　ここまで

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

            '--2014/04/21 中川　ここから
            If Not conDB Is Nothing Then

                trans.Rollback()

            End If

            Return False
            '--2014/04/21 中川　ここまで
        Finally

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function

    ''' <summary>
    ''' 同期ボタン押下処理 'COMMENP006-015
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub btnSync_Click()

        '電話番号マスタ同期処理
        mfSyncTelNoMaster()

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面項目初期化・制御
    ''' </summary>
    ''' <param name="strUser"></param>
    ''' <param name="strTerms"></param>
    ''' <remarks></remarks>
    Protected Sub display_Control(strUser As String, strTerms As String)

        objStack = New StackFrame
        Try
            '全コントロールの初期化
            Me.txtTboxId.ppText = Nothing
            Me.tbxNLSec.Text = Nothing
            Me.txtSystem.ppText = Nothing
            Me.txtVER.ppText = Nothing
            Me.txtTboxTel.ppText = Nothing
            Me.txtOptSttDate.ppText = Nothing
            Me.txtNotes.ppText = Nothing
            Me.txtMDNInstallation.ppText = Nothing
            Me.ddlSecChaSC.ppDropDownList.SelectedIndex = Nothing 'Me.txtSecChaSC.ppText = Nothing 'COMMENP006-002
            Me.txtScAdd.ppText = Nothing
            Me.txtScFax.ppText = Nothing
            Me.txtLANName.Text = Nothing
            Me.txtLANResp.ppText = Nothing
            Me.txtSecChaTel.ppText = Nothing
            Me.ddlReviewSC.ppDropDownList.SelectedIndex = Nothing 'Me.txtReviewSC.ppText = Nothing 'COMMENP006-002
            Me.txtBranchNm.ppText = Nothing
            Me.txtReviewAdd.ppText = Nothing
            Me.txtReviewTel.ppText = Nothing
            Me.txtReviewFax.ppText = Nothing
            Me.ddlAgency.ppDropDownList.SelectedIndex = Nothing 'Me.txtAgency.ppText = Nothing 'COMMENP006-002
            Me.txtAgencyAdd.ppText = Nothing
            Me.txtAgencyTel.ppText = Nothing
            Me.txtAgencyFax.ppText = Nothing
            Me.txtHoleNm.ppText = Nothing
            Me.txtHoleCd.ppText = Nothing
            Me.txtCcrtDate.ppText = Nothing
            Me.txtHoleAdd.ppText = Nothing
            Me.txtHoleTel1.ppText = Nothing
            Me.txtEWSec.ppText = Nothing
            Me.txtFaxNo.ppText = Nothing
            Me.tbxStoreType.Text = Nothing
            Me.txtPrtNumberOfMDN.ppText = Nothing
            Me.txtConSalDep.ppText = Nothing
            Me.txtSdTel.ppText = Nothing
            Me.txtNumberOfMDN.ppText = Nothing
            Me.txtChd1NumberOfMDN.ppText = Nothing
            Me.txtTboxSerial.ppText = Nothing
            Me.txtOppSerial.ppText = Nothing
            Me.txtUPSSerial.ppText = Nothing
            Me.txtPrtSerial.ppText = Nothing
            Me.txtCRTSerial.ppText = Nothing
            Me.txtHDD1Serial.ppText = Nothing
            Me.txtHDD2Serial.ppText = Nothing
            Me.txtHDD3Serial.ppText = Nothing
            Me.txtHDD4Serial.ppText = Nothing
            Me.txtSC.ppText = Nothing
            Me.txtCC.ppText = Nothing
            Me.txtSand.ppText = Nothing
            Me.txtTVMachine.ppText = Nothing
            Me.txtAmMachine.ppText = Nothing
            Me.txtNotes1.ppText = Nothing
            Me.txtNotes2.ppText = Nothing
            Me.txtNotes3.ppText = Nothing
            Me.txtNotes4.ppText = Nothing
            Me.txtHoleTel2.ppText = Nothing
            'Me.txtUseCtt1.ppText = Nothing 'COMMENP006-001
            Me.DDLUseCd1.SelectedIndex = Nothing
            Me.txtHoleTel3.ppText = Nothing
            'Me.txtUseCtt2.ppText = Nothing 'COMMENP006-001
            Me.DDLUseCd2.SelectedIndex = Nothing
            Me.txtHoleTel4.ppText = Nothing
            'Me.txtUseCtt3.ppText = Nothing 'COMMENP006-001
            Me.DDLUseCd3.SelectedIndex = Nothing
            Me.txtHoleTel5.ppText = Nothing
            'Me.txtUseCtt4.ppText = Nothing 'COMMENP006-001
            Me.DDLUseCd4.SelectedIndex = Nothing

            'ボタンの表示名を設定
            Master.ppLeftButton1.Text = P_BTN_MINI
            Master.ppLeftButton2.Text = P_BTN_TRBL
            Master.ppLeftButton3.Text = P_BTN_MNTE
            Master.ppRigthButton1.Text = P_BTN_NM_UPD
            Master.ppRigthButton2.Text = P_BTN_NM_PRI
            Master.ppRigthButton3.Text = "元に戻す"
            Master.ppRigthButton4.Text = P_BTN_HSTR
            Master.ppRigthButton5.Text = P_BTN_SYNC 'COMMENP006-015

            '権限による「初期化」、「更新」のボタン活性振り分け
            If strTerms = ClsComVer.E_遷移条件.更新 Then                  '更新の場合

                Master.ppRigthButton1.Visible = True
                Master.ppRigthButton3.Visible = True
                Master.ppRigthButton3.CausesValidation = False
            Else                                    '参照の場合
                Master.ppRigthButton3.Visible = False
                Master.ppRigthButton1.Visible = False
                Me.txtNotes.ppTextBox.ReadOnly = True
                Me.txtNotes.ppTextBox.Enabled = False
            End If
            Master.ppLeftButton1.Visible = True
            Master.ppLeftButton2.Visible = True
            Master.ppLeftButton3.Visible = True
            'コマンド設定
            Master.ppLeftButton1.CommandName = P_BTN_MINI
            Master.ppLeftButton2.CommandName = P_BTN_TRBL
            Master.ppLeftButton3.CommandName = P_BTN_MNTE
            Master.ppRigthButton4.CommandName = P_BTN_HSTR

            '「進捗履歴一覧」、「印刷」のボタン活性
            Master.ppRigthButton2.Visible = True
            Master.ppRigthButton4.Visible = True
            Master.ppRigthButton5.Visible = True 'COMMENP006-015
            Master.ppRigthButton2.CausesValidation = False
            Master.ppRigthButton4.CausesValidation = False
            Master.ppRigthButton5.CausesValidation = False 'COMMENP006-015
            Master.ppLeftButton1.CausesValidation = False
            Master.ppLeftButton2.CausesValidation = False
            Master.ppLeftButton3.CausesValidation = False

            '各コントロールの使用可・不可の設定
            Me.PnlEnabled1.Enabled = False
            Me.PnlEnabled3.Enabled = False
            'Me.PnlEnabled4.Enabled = False      '注意事項の入力不可
            'Me.PnlEnabled5.Enabled = False
            Me.PnlEnabled6.Enabled = False
            Me.PnlEnabled7.Enabled = False
            '2015/07/07
            'Me.PnlEnabled8.Enabled = False
            'Me.PnlEnabled9.Enabled = False
            'Me.PnlEnabled10.Enabled = False
            Me.txtMDNInstallation.ppEnabled = False
            Me.txtNumberOfMDN.ppEnabled = False
            Me.txtPrtNumberOfMDN.ppEnabled = False
            Me.txtChd1NumberOfMDN.ppEnabled = False
            Me.txtChd2NumberOfMDN.ppEnabled = False
            Me.txtMDN.ppEnabled = False
            '2015/07/07 END
            Me.txtFaxNo.ppEnabled = False
            Me.txtEWSec.ppEnabled = False
            Me.txtHoleTel1.ppEnabled = False
            Me.txtHoleTel2.ppEnabled = False
            Me.txtHoleTel3.ppEnabled = False
            Me.txtHoleTel4.ppEnabled = False
            Me.txtHoleTel5.ppEnabled = False
            'COMMENP006-001
            'Me.txtUseCtt1.ppEnabled = False
            'Me.txtUseCtt2.ppEnabled = False
            'Me.txtUseCtt3.ppEnabled = False
            'Me.txtUseCtt4.ppEnabled = False
            'COMMENP006-001END
            Me.DDLUseCd1.Enabled = False
            Me.DDLUseCd2.Enabled = False
            Me.DDLUseCd3.Enabled = False
            Me.DDLUseCd4.Enabled = False
            Me.txtMDN.ppEnabled = False

            If strTerms = ClsComVer.E_遷移条件.更新 Then               '更新の場合
                Me.PnlEnabled4.Enabled = True       '注意事項の入力可
                Me.txtHoleTel2.ppEnabled = True
                Me.txtHoleTel3.ppEnabled = True
                Me.txtHoleTel4.ppEnabled = True
                Me.txtHoleTel5.ppEnabled = True
                'COMMENP006-001
                'Me.txtUseCtt1.ppEnabled = True
                'Me.txtUseCtt2.ppEnabled = True
                'Me.txtUseCtt3.ppEnabled = True
                'Me.txtUseCtt4.ppEnabled = True
                'COMMENP006-001 END
                Me.DDLUseCd1.Enabled = True
                Me.DDLUseCd2.Enabled = True
                Me.DDLUseCd3.Enabled = True
                Me.DDLUseCd4.Enabled = True
            End If

            '「発信」のボタン活性
            Me.btnTell1.Enabled = False
            Me.btnTell2.Enabled = False
            Me.btnTell3.Enabled = False
            Me.btnTell4.Enabled = False
            Me.btnTell5.Enabled = False
            Me.btnTell1.Visible = False
            Me.btnTell2.Visible = False
            Me.btnTell3.Visible = False
            Me.btnTell4.Visible = False
            Me.btnTell5.Visible = False

            '「発信」の色設定
            Me.btnTell1.BackColor = Drawing.Color.FromArgb(0, 0, 0)
            Me.btnTell1.ForeColor = Drawing.Color.FromArgb(255, 255, 255)
            Me.btnTell2.BackColor = Drawing.Color.FromArgb(0, 0, 0)
            Me.btnTell2.ForeColor = Drawing.Color.FromArgb(255, 255, 255)
            Me.btnTell3.BackColor = Drawing.Color.FromArgb(0, 0, 0)
            Me.btnTell3.ForeColor = Drawing.Color.FromArgb(255, 255, 255)
            Me.btnTell4.BackColor = Drawing.Color.FromArgb(0, 0, 0)
            Me.btnTell4.ForeColor = Drawing.Color.FromArgb(255, 255, 255)
            Me.btnTell5.BackColor = Drawing.Color.FromArgb(0, 0, 0)
            Me.btnTell5.ForeColor = Drawing.Color.FromArgb(255, 255, 255)

            '検証エラーチェックの設定
            Master.ppRigthButton1.ValidationGroup = "1"

            'ボタン押下時のエラーメッセージ設定
            Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, P_HALLINF)
            Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("10001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, P_HALLPRINT)
            Master.ppRigthButton3.OnClientClick = pfGet_OCClickMes("00002", ClsComVer.E_Mタイプ.警告, ClsComVer.E_Mモード.OKCancel, P_HALLINF)
            Master.ppRigthButton5.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "電話番号マスタ") 'COMMENP006-015

            '管理者のみ一部活性　COMMENP006-001
            If Session(P_SESSION_AUTH) = "管理者" AndAlso strTerms = ClsComVer.E_遷移条件.更新 Then
                PnlEnabled1.Enabled = True

                txtTboxId.ppEnabled = False
                tbxNLSec.Enabled = False
                txtSystem.ppEnabled = False
                'txtVER.ppEnabled = False
                txtTboxTel.ppEnabled = False
                txtOptSttDate.ppEnabled = False
                txtHoleNm.ppEnabled = False
                txtHoleCd.ppEnabled = False
                txtCcrtDate.ppEnabled = False
                'txtHoleAdd.ppEnabled = False

                PnlEnabled7.Enabled = True
                txtSdTel.ppEnabled = True
                txtLANResp.ppEnabled = False
                txtLANName.Enabled = False
                txtSecChaTel.ppEnabled = False
                txtScAdd.ppEnabled = False
                txtScFax.ppEnabled = False
                txtBranchNm.ppEnabled = False
                txtReviewTel.ppEnabled = False
                txtReviewAdd.ppEnabled = False
                txtReviewFax.ppEnabled = False
                txtAgencyTel.ppEnabled = False
                txtAgencyAdd.ppEnabled = False
                txtAgencyFax.ppEnabled = False

            End If 'COMMENP006-001 END

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Throw ex
        End Try

    End Sub

    ''' <summary>
    ''' ドロップダウンリストの表示設定
    ''' </summary>
    ''' <param name="DDLitem"></param>
    ''' <param name="yoto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function Index_Sel(DDLitem As Object, yoto As String) As Integer

        Dim idx As Integer = 0
        Dim flag As Boolean = False

        For Each item As ListItem In DDLitem.Items

            ' value が 用途コードと一致する
            If (item.Value = yoto) Then
                flag = True
                Exit For
            End If

            idx += 1
        Next

        'FOR文をすべて抜けた場合、-1を行う
        If flag = False Then
            idx = idx - 1
        End If

        Return idx

    End Function

    ''' <summary>
    ''' 入力項目の整合性チェックを行う。
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfCheck_Integrity(ByVal UseCtt1 As String _
                                 , ByVal UseCtt2 As String _
                                 , ByVal UseCtt3 As String _
                                 , ByVal UseCtt4 As String) As Boolean
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim lng As Long = 0
        objStack = New StackFrame

        mfCheck_Integrity = False

        Try
            '郵便番号の形式チェック
            If txtZip.ppText <> String.Empty Then
                If Regex.IsMatch(txtZip.ppText, "^(\d{3}-\d{4}|\d{7})$") = False Then
                    'txtZip.psSet_ErrorNo("4001", txtZip.ppName, "半角数字又はハイフン")
                    txtZip.psSet_ErrorNo("4001", txtZip.ppName, "正しい形式")
                End If
            End If

            'ホールTELの重複チェック(文字列比較)
            Dim TxtBoxes As SPC.ClsCMTextBox() = {New ClsCMTextBox, txtHoleTel1, txtHoleTel2, txtHoleTel3, txtHoleTel4, txtHoleTel5}
            Dim SlctIndexes As Integer() = {0, 0, DDLUseCd1.SelectedIndex, DDLUseCd2.SelectedIndex, DDLUseCd3.SelectedIndex, DDLUseCd4.SelectedIndex}
            Dim HallTells As String() = {String.Empty, txtHoleTel1.ppText, txtHoleTel2.ppText, txtHoleTel3.ppText, txtHoleTel4.ppText, txtHoleTel5.ppText}

            For i As Integer = 2 To 5
                If HallTells(i) = String.Empty Then
                    If SlctIndexes(i) <> 0 Then
                        '未入力エラー(用途選択時)
                        TxtBoxes(i).psSet_ErrorNo("5001", TxtBoxes(i).ppName)
                    End If
                Else
                    If Long.TryParse(HallTells(i).Replace("-", ""), lng) = False Then
                        '形式エラー
                        TxtBoxes(i).psSet_ErrorNo("4001", TxtBoxes(i).ppName, "ハイフン(-)又は数字")
                    Else
                        'COMMENP006-008
                        'If Array.IndexOf(HallTells, HallTells(i)) <> Array.LastIndexOf(HallTells, HallTells(i)) Then
                        '    '重複エラー
                        '    TxtBoxes(i).psSet_ErrorNo("2011", "ホールTEL１～５", "異なる番号")
                        'End If
                        'COMMENP006-008 END
                    End If
                End If
            Next

            '注意事項の長さ確認
            Dim strlen As String = Me.txtNotes.ppText
            If strlen.Length > 500 Then
                Me.txtNotes.psSet_ErrorNo("3002", Me.txtHoleTel5.ppName, "注意事項", Me.txtNotes.ppMaxLength)
            End If

            'COMMENP006-002
            '担当営業所
            If txtSdTel.ppText = String.Empty Then
                If txtConSalDep.ppText <> String.Empty Then
                    txtSdTel.psSet_ErrorNo("5006", txtSdTel.ppName)  'TEL 未入力エラー
                End If
            Else
                If Long.TryParse(txtSdTel.ppText.Replace("-", ""), lng) = False Then
                    txtSdTel.psSet_ErrorNo("4001", "担当営業部 TEL", "ハイフン(-)又は数字")  'TEL 形式エラー
                End If

                If txtConSalDep.ppText = String.Empty Then
                    txtConSalDep.psSet_ErrorNo("5006", txtConSalDep.ppName)  '担当営業所 未入力エラー
                End If
            End If

            '担当営業部情報
            If txtConSalDep.ppText = String.Empty AndAlso txtSdTel.ppText <> String.Empty Then
                txtConSalDep.psSet_ErrorNo("5006", txtConSalDep.ppName)
            End If
            If txtConSalDep.ppText <> String.Empty AndAlso txtSdTel.ppText = String.Empty Then
                txtSdTel.psSet_ErrorNo("5006", txtSdTel.ppName)
            End If
            'COMMENP006-002 END

            'Ver COMMENP006-006
            If txtVER.ppText.Trim <> String.Empty Then

                'Ver情報取得
                If mfGetVerData(dstOrders, ViewState(P_KEY)(2).ToString, hdnSystem.Value, txtVER.ppText.Trim) = False Then
                    Exit Try
                End If

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '整合性エラー
                    Select Case ViewState(P_KEY)(2).ToString
                        Case "1"
                            txtVER.psSet_ErrorNo("2011", txtVER.ppName, txtSystem.ppText & "(ID)のバージョン") '{0} は{1}を入力してください。
                        Case "3"
                            txtVER.psSet_ErrorNo("2011", txtVER.ppName, txtSystem.ppText & "(IC)のバージョン") '{0} は{1}を入力してください。
                        Case "5"
                            txtVER.psSet_ErrorNo("2011", txtVER.ppName, txtSystem.ppText & "(LUTERNA)のバージョン") '{0} は{1}を入力してください。
                        Case Else
                            txtVER.psSet_ErrorNo("2012", "システムと入力された" & txtVER.ppName)
                    End Select
                End If
            End If
            'COMMENP006-006 END

            If ddlReviewSC.ppSelectedValue = String.Empty AndAlso ddlSecChaSC.ppSelectedValue <> String.Empty Then
                ddlReviewSC.psSet_ErrorNo("5003", ddlReviewSC.ppName)
            End If

            'ホールコード　'COMMENP006-013
            If txtHoleCd.ppEnabled = True AndAlso txtHoleCd.ppText <> String.Empty Then
                'ホールコードが変更されている場合
                If txtHoleCd.ppText <> hdnHallCd.Value Then

                    'ホール情報取得
                    If mfGetHallData(dstOrders, txtHoleCd.ppText) = False Then
                        Exit Try
                    End If

                    '整合性チェック
                    If dstOrders.Tables.Count = 0 OrElse dstOrders.Tables(0).Rows.Count = 0 Then
                        'ホールコード無
                        'txtHoleCd.psSet_ErrorNo("2021", txtHoleCd.ppName, txtOptSttDate.ppName)        '{0}は存在しません。{1}を確認して下さい。
                        txtHoleCd.psSet_ErrorNo("2002", txtHoleCd.ppName, txtOptSttDate.ppName)        '{0} は存在しません。再度確認し、入力してください。
                    Else
                        'ホールコード有
                        If cbxHallCd.Checked = True Then
                            '※チェックボックスのチェックONの時は、実質ホールコードの変更は出来ない。(ホールコード有/無いずれも整合性エラーになる)
                            If dstOrders.Tables(0).Rows(0)("TBOXID").ToString = txtTboxId.ppText Then
                                'ホールコードが自分の双子店の場合
                                txtHoleCd.psSet_ErrorNo("2011", txtHoleCd.ppName & "：" & txtHoleCd.ppText, "双子店です。別のホールコード") '{0} は{1}を入力してください。
                            Else
                                txtHoleCd.psSet_ErrorNo("2006", txtHoleCd.ppName & "：" & txtHoleCd.ppText) '{0}は既に登録されています。
                            End If
                        End If
                    End If
                End If
            End If

            '運用終了日付 '運用状況
            If Session(P_SESSION_AUTH) = "管理者" Then
                If ViewState(P_KEY)(2).ToString = "1" Then
                    '運用終了日未入力
                    If txtWrkEndDate.ppText.Trim = String.Empty Then
                        'COMMENP006-014
                        If hdnPerCls.Value <> ddlPerCls.ppSelectedValue Then
                            '運用状況変更
                            Select Case hdnPerCls.Value '変更前
                                Case "02", "99"
                                    '02-99間の変更
                                    Select Case ddlPerCls.ppSelectedValue '変更後
                                        Case "02", "99"
                                            If hdnWrkEndDate.Value.Trim <> String.Empty Then
                                                '未入力への変更は不許可
                                                txtWrkEndDate.psSet_ErrorNo("5001", txtWrkEndDate.ppName)
                                            End If
                                    End Select
                                Case Else
                                    Select Case ddlPerCls.ppSelectedValue '変更後
                                        Case "02", "99"
                                            txtWrkEndDate.psSet_ErrorNo("5001", txtWrkEndDate.ppName)
                                        Case Else
                                            '00,01は未入力許可
                                    End Select
                            End Select
                        Else
                            ''運用状況変更無し
                            If hdnWrkEndDate.Value.Trim <> String.Empty Then
                                '運用終了日変更
                                Select Case hdnPerCls.Value
                                    Case "02", "99"
                                        txtWrkEndDate.psSet_ErrorNo("5001", txtWrkEndDate.ppName)
                                End Select
                            End If
                        End If
                        'COMMENP006-014 END
                    Else
                        If DateTime.TryParse(txtWrkEndDate.ppText, New DateTime) Then
                            If txtOptSttDate.ppText > txtWrkEndDate.ppText Then
                                '運用開始日以前
                                txtWrkEndDate.psSet_ErrorNo("1006", txtWrkEndDate.ppName, txtOptSttDate.ppName) '「{0}」は「{1}」以降の日時で入力してください。
                            End If
                        Else
                            '形式エラー
                            txtWrkEndDate.psSet_ErrorNo("4001", txtWrkEndDate.ppName, "日付")
                        End If
                    End If
                End If

                If hdnPerCls.Value <> ddlPerCls.ppSelectedValue AndAlso ddlPerCls.ppSelectedValue.Trim = String.Empty Then
                    '未入力に変更した場合 未入力エラー
                    ddlPerCls.psSet_ErrorNo("5001", ddlPerCls.ppName)
                End If
            End If

            mfCheck_Integrity = True

        Catch ex As Exception

            'メッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "入力値の検証") '{0}に失敗しました。
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DataSet破棄
            clsSqlDbSvr.psDisposeDataSet(dstOrders)
            'Connection破棄
            clsDataConnect.pfClose_Database(conDB)
        End Try

    End Function

    ''' <summary>
    ''' [保担][総括][代理店]変更時 COMMENP006-2
    ''' </summary>
    Private Sub msDropdown_Chenged(sender As Object, e As EventArgs)
        Dim objddl As DropDownList
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstSC As New DataSet
        'Dim objItem As ListItem

        Try
            objStack = New StackFrame
            objddl = DirectCast(sender, DropDownList)       '呼び出し元のドロップダウンを格納
            'objItem = ddlReviewSC.ppDropDownList.Items(0)   '統括保担の先頭Item保存

            If clsDataConnect.pfOpen_Database(conDB) Then

                cmdDB = New SqlCommand("COMMENP006_S4", conDB)
                cmdDB.Parameters.Add(pfSet_Param("code", SqlDbType.NVarChar, objddl.SelectedValue.ToString))

                Select Case DirectCast(sender, DropDownList).UniqueID
                    Case ddlSecChaSC.ppDropDownList.UniqueID
                        '保担
                        Dim strMntCd As String = ddlSecChaSC.ppSelectedValue.ToString 'COMMENP006-012
                        'If ddlSecChaSC.ppDropDownList.SelectedIndex = 1 AndAlso Not ViewState("保担") Is Nothing Then
                        If ddlSecChaSC.ppSelectedValue = DDL_VALUE Then
                            '先頭項目にデータを表示している場合
                            txtScAdd.ppText = ViewState("保担")(0)
                            txtSecChaTel.ppText = ViewState("保担")(1)
                            txtScFax.ppText = ViewState("保担")(2)
                            strMntCd = (ddlSecChaSC.ppSelectedText & "###").Substring(0, 3) 'COMMENP006-012
                        Else
                            cmdDB.Parameters.Add(pfSet_Param("cd_cls", SqlDbType.NVarChar, "営業所"))
                            dstSC = clsDataConnect.pfGet_DataSet(cmdDB)
                            txtScAdd.ppText = dstSC.Tables(0)(0)("住所").ToString
                            txtSecChaTel.ppText = dstSC.Tables(0)(0)("TEL").ToString
                            txtScFax.ppText = dstSC.Tables(0)(0)("FAX").ToString
                        End If

                        '統括保担ドロップダウンの更新
                        cmdDB = New SqlCommand("COMMENP006_S3", conDB)
                        cmdDB.Parameters.Add(pfSet_Param("mnt_cd", SqlDbType.NVarChar, strMntCd)) 'COMMENP006-012
                        dstSC = clsDataConnect.pfGet_DataSet(cmdDB)
                        ddlReviewSC.ppDropDownList.DataSource = dstSC.Tables(1)
                        ddlReviewSC.ppDropDownList.DataTextField = "営業所"
                        ddlReviewSC.ppDropDownList.DataValueField = "営業所コード"
                        ddlReviewSC.ppDropDownList.DataBind()
                        ddlReviewSC.ppDropDownList.Items.Insert(0, "") '統括保担の先頭Item
                        ddlReviewSC.ppDropDownList.SelectedIndex = -1

                        '統括保担情報のクリア
                        txtReviewAdd.ppText = String.Empty
                        txtReviewTel.ppText = String.Empty
                        txtReviewFax.ppText = String.Empty
                        txtBranchNm.ppText = String.Empty

                    Case ddlReviewSC.ppDropDownList.UniqueID
                        '統括保担
                        If ddlReviewSC.ppSelectedValue = DDL_VALUE Then
                            '先頭項目にデータを表示している場合
                            txtReviewAdd.ppText = ViewState("統括")(0)
                            txtReviewTel.ppText = ViewState("統括")(1)
                            txtReviewFax.ppText = ViewState("統括")(2)
                            txtBranchNm.ppText = ViewState("統括")(3)
                        Else
                            cmdDB.Parameters.Add(pfSet_Param("cd_cls", SqlDbType.NVarChar, "営業所"))
                            dstSC = clsDataConnect.pfGet_DataSet(cmdDB)
                            txtReviewAdd.ppText = dstSC.Tables(0)(0)("住所").ToString
                            txtReviewFax.ppText = dstSC.Tables(0)(0)("FAX").ToString
                            txtBranchNm.ppText = dstSC.Tables(0)(0)("支店名").ToString
                            txtReviewTel.ppText = dstSC.Tables(0)(0)("TEL").ToString
                        End If

                    Case ddlAgency.ppDropDownList.UniqueID
                        '代理店
                        If ddlAgency.ppSelectedValue = DDL_VALUE Then
                            '先頭項目にデータを表示している場合
                            txtAgencyAdd.ppText = ViewState("代理店")(0)
                            txtAgencyTel.ppText = ViewState("代理店")(1)
                            txtAgencyFax.ppText = ViewState("代理店")(2)
                        Else
                            cmdDB.Parameters.Add(pfSet_Param("cd_cls", SqlDbType.NVarChar, "会社"))
                            dstSC = clsDataConnect.pfGet_DataSet(cmdDB)
                            txtAgencyAdd.ppText = dstSC.Tables(0)(0)("住所").ToString
                            txtAgencyTel.ppText = dstSC.Tables(0)(0)("TEL").ToString
                            txtAgencyFax.ppText = dstSC.Tables(0)(0)("FAX").ToString
                        End If
                End Select

            Else
                '接続失敗
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Exit Try
            End If

        Catch ex As Exception
            'システムエラー
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保担情報の取得・表示")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DataSet破棄
            clsSqlDbSvr.psDisposeDataSet(dstSC)
            'Command破棄
            cmdDB.Dispose()
            'Connection破棄
            clsDataConnect.pfClose_Database(conDB)
        End Try

    End Sub

    ''' <summary>
    ''' [保担][総括][代理店]ドロップダウン項目追加処理 COMMENP006-2
    ''' </summary>
    Private Sub msSetDropDown(ByVal objRow As DataRow)

        Dim ddlists As DropDownList() = {ddlSecChaSC.ppDropDownList, ddlReviewSC.ppDropDownList, ddlAgency.ppDropDownList}
        Dim Text_n_Value(,) As String = {{objRow.Item("保守担当コード").ToString, objRow.Item("保守担当").ToString} _
                                       , {objRow.Item("統括コード").ToString, objRow.Item("統括名").ToString} _
                                       , {objRow.Item("代理店コード").ToString, objRow.Item("代理店名").ToString}}
        '初期化
        ViewState("保担") = Nothing
        ViewState("統括") = Nothing
        ViewState("代理店") = Nothing

        'ドロップダウン設定
        For i As Integer = 0 To 2
            'COMMENP006-012
            'If ddlists(i).Items.FindByValue(Text_n_Value(i, 0)) Is Nothing Then
            If ddlists(i).Items.FindByValue(Text_n_Value(i, 0)) Is Nothing OrElse _
              (Text_n_Value(i, 0) <> String.Empty AndAlso ddlists(i).Items.FindByText(Text_n_Value(i, 0) & "：" & Text_n_Value(i, 1)) Is Nothing) Then
                'COMMENP006-012 END

                '新規項目追加
                ddlists(i).Items.Item(0).Text = Text_n_Value(i, 0) & "：" & Text_n_Value(i, 1)
                Text_n_Value(i, 0) = DDL_VALUE
                ddlists(i).Items.Item(0).Value = Text_n_Value(i, 0)
                ddlists(i).Items.Insert(0, "")

                '関連情報保存
                Select Case i
                    Case 0
                        ViewState("保担") = {objRow.Item("保守担当住所").ToString _
                                           , objRow.Item("保守担当電話番号").ToString _
                                           , objRow.Item("保守担当ＦＡＸ番号").ToString}
                    Case 1
                        ViewState("統括") = {objRow.Item("統括住所").ToString _
                                           , objRow.Item("統括TEL").ToString() _
                                           , objRow.Item("統括FAX").ToString() _
                                           , objRow.Item("統括支店名").ToString}
                    Case 2
                        ViewState("代理店") = {objRow.Item("代理店住所").ToString _
                                             , objRow.Item("代理店TEL").ToString() _
                                             , objRow.Item("代理店FAX").ToString()}
                End Select
            End If

            '項目選択
            ddlists(i).SelectedValue = Text_n_Value(i, 0)
        Next

    End Sub

    ''' <summary>
    ''' [運用状況]ドロップダウン項目追加処理 'COMMENP006-011
    ''' </summary>
    Private Sub msSetPerCls(ByVal strValue As String)

        'リストに無い場合追加
        If ddlPerCls.ppDropDownList.Items.FindByValue(strValue) Is Nothing Then
            ddlPerCls.ppDropDownList.Items.Insert(0, strValue)
        Else
            ddlPerCls.ppSelectedValue = strValue
        End If

    End Sub

    ''' <summary>
    ''' ホール情報取得処理 'COMMENP006-013
    ''' </summary>
    Private Function mfGetHallData(ByRef dstHall As DataSet, ByVal strHallCd As String)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing

        mfGetHallData = False

        Try
            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                cmdDB = New SqlCommand("ZCMPSEL058", conDB)
                cmdDB.Parameters.Add(pfSet_Param("Hall_cd", SqlDbType.NVarChar, strHallCd))

                '取得
                dstHall = clsDataConnect.pfGet_DataSet(cmdDB)
            Else
                '接続失敗
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Exit Try
            End If

            '成功
            mfGetHallData = True

        Catch ex As Exception
            'メッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "入力値の検証") '{0}に失敗しました。
        Finally
            'Command破棄
            cmdDB.Dispose()
            'close
            conDB.Close()
        End Try

    End Function

    ''' <summary>
    ''' Ver情報取得処理 'COMMENP006-006
    ''' </summary>
    Private Function mfGetVerData(ByRef dstVer As DataSet, ByVal strSysCls As String, ByVal strSysCd As String, ByVal strVer As String) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing

        mfGetVerData = False

        Try
            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                cmdDB = New SqlCommand("COMUPDM03_S2", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("system_cls", SqlDbType.NVarChar, strSysCls))
                    .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, strSysCd))
                    .Add(pfSet_Param("ver", SqlDbType.NVarChar, strVer))
                End With

                '取得
                dstVer = clsDataConnect.pfGet_DataSet(cmdDB)
            Else
                '接続失敗
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Exit Try
            End If

            '成功
            mfGetVerData = True

        Catch ex As Exception
            'メッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "入力値の検証") '{0}に失敗しました。
        Finally
            'Command破棄
            cmdDB.Dispose()
            'close
            conDB.Close()
        End Try

    End Function
    ' ''' <summary>
    ' ''' [郵便番号]変更時 COMMENP006-004
    ' ''' </summary>
    'Private Sub msZipno_Chaged()

    '    If Regex.IsMatch(txtZip.ppText.Replace("-", ""), "\d{7}") Then
    '        Dim conDB As SqlConnection = Nothing

    '        Try
    '            If clsDataConnect.pfOpen_Database(conDB) Then
    '                Using cmdDB As SqlCommand = New SqlCommand("ZCMPSEL054", conDB)
    '                    cmdDB.CommandType = CommandType.StoredProcedure
    '                    cmdDB.Parameters.Add(pfSet_Param("zipno", SqlDbType.NVarChar, txtZip.ppText))
    '                    Dim objData As Object = cmdDB.ExecuteScalar()  '住所データ取得
    '                    If objData Is Nothing Then
    '                        txtHoleAdd.ppText = String.Empty
    '                    Else
    '                        txtHoleAdd.ppText = objData.ToString
    '                    End If
    '                End Using
    '            End If
    '        Catch ex As Exception
    '            '失敗
    '            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "住所情報の取得")
    '            'ログ出力
    '            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
    '                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
    '        Finally
    '            If Not conDB Is Nothing Then
    '                'DB切断
    '                clsDataConnect.pfClose_Database(conDB)
    '            End If
    '        End Try
    '    Else
    '        txtHoleAdd.ppText = String.Empty
    '    End If

    'End Sub

    ''' <summary>
    ''' 同期処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfSyncTelNoMaster() As Boolean
        Const strStored As String = "ZCMPINS004"
        Dim blnSync As Boolean = False
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim trans As SqlClient.SqlTransaction
        mfSyncTelNoMaster = False
        Dim strTel As String = txtHoleTel1.ppText.Trim

        'トランザクションの設定

        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                trans = conDB.BeginTransaction
                cmdDB = New SqlCommand(strStored, conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.Transaction = trans
                If strTel <> String.Empty Then
                    With cmdDB.Parameters
                        .Clear()
                        .Add(pfSet_Param("telno", SqlDbType.NVarChar, strTel))
                        .Add(pfSet_Param("code", SqlDbType.NVarChar, txtHoleCd.ppText.Trim))
                        .Add(pfSet_Param("decision", SqlDbType.NVarChar, "0"))
                        .Add(pfSet_Param("usrid", SqlDbType.NVarChar, User.Identity.Name))
                    End With
                    Try
                        cmdDB.ExecuteNonQuery()
                    Catch ex As Exception
                        trans.Rollback()
                        Throw ex
                    End Try
                    blnSync = True
                End If
                trans.Commit()
                If blnSync Then
                    '終了メッセージ
                    msSyncTelCompleteMsg(conDB)
                End If
            Catch ex As Exception
                'エラーメッセージ表示
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "電話番号同期")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            End Try
        End If
    End Function

    Private Sub msSyncTelCompleteMsg(ByVal _con As SqlConnection)

        Dim ds As New DataSet
        Dim cmd As New SqlCommand("ZMSTSEL009", _con)

        Try
            cmd.CommandType = CommandType.StoredProcedure

            With cmd.Parameters
                .Add(pfSet_Param("tel", SqlDbType.NVarChar, txtHoleTel1.ppText.Replace("-", "")))
                .Add(pfSet_Param("cod", SqlDbType.NVarChar, txtHoleCd.ppText.Trim))
                .Add(pfSet_Param("dvs", SqlDbType.NVarChar, "0"))
            End With

            ds = clsDataConnect.pfGet_DataSet(cmd)

            If ds.Tables(0).Rows.Count = 0 Then
                '正常終了メッセージ
                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "電話番号の同期")
                Return
            End If

            Dim infomsg As String = "電話番号の同期が完了しました。\n以下の電話番号を電話番号マスタで変更又は削除してください。"
            For Each dr As DataRow In ds.Tables(0).Rows
                infomsg &= "\n" & dr.Item("TEL").ToString.Trim
            Next

            psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, infomsg)
        Catch ex As Exception
            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "電話番号同期")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally

            Call psDisposeDataSet(ds)

        End Try

    End Sub

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
