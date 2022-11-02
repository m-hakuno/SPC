'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　保守対応依頼書
'*　ＰＧＭＩＤ：　CMPSELP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.14　：　ＮＫＣ
'*  更　新　　：　2014.06.17　：　間瀬　レイアウト変更
'*  更　新　　：　2014.06.19　：　間瀬　レイアウト変更
'*  更　新　　：　2014.07.31　：　稲葉　エラーチェック処理変更
'*  更　新　　：　2014.11.13　：　武　　画面遷移パラメータ変更
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'CMPSELP001-001     2015/09/08      加賀　　　輸送元/輸送先に[3:保守拠点][4:NGC]追加。
'CMPSELP001-002     2015/08/19      加賀      ホールマスタ管理からの画面遷移に対応              
'CMPSELP001-003     2015/08/19      加賀      画面遷移[ホールマスタ管理][対応履歴照会]追加  
'CMPSELP001-004     2016/02/15      栗原      代行店の表示/非表示をマスタ管理に変更

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive
#End Region

Public Class CMPSELP001
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
    Const M_MY_DISP_ID = P_FUN_CMP & P_SCR_SEL & P_PAGE & "001"

    '画面ID
    Const M_MNT_DISP_ID = P_FUN_CMP & P_SCR_LST & P_PAGE & "001"    '保守対応依頼書一覧
    Const M_REPR_DISP_ID = P_FUN_REP & P_SCR_UPD & P_PAGE & "002"   '修理依頼書
    Const M_TRBL_DISP_ID = P_FUN_REQ & P_SCR_SEL & P_PAGE & "001"   'トラブル処理票
    Const M_BRNG_DISP_ID = P_FUN_REQ & P_SCR_LST & P_PAGE & "002"   '持参物品一覧
    Const M_SMNT_DISP_ID = P_FUN_CMP & P_SCR_INQ & P_PAGE & "001"   '特別保守費用照会
    Const M_CTI_DISP_ID = P_FUN_CTI & P_SCR_SEL & P_PAGE & "005"    'CTI情報(作業者)
    Const M_CHST_DISP_ID = P_FUN_BRK & P_SCR_INQ & P_PAGE & "001"   '対応履歴照会
    Const M_HALMST_DISP_ID = P_FUN_COM & P_SCR_MEN & P_PAGE & "006" 'ホールマスタ管理

    'ＭＣ
    Const M_MC_DISP_PATH = "~/" & P_MAI & "/" &
            P_FUN_CMP & P_SCR_SEL & P_PAGE & "002" & "/" &
            P_FUN_CMP & P_SCR_SEL & P_PAGE & "002.aspx"

    '修理依頼書ファイルパス
    Const M_REPR_DISP_PATH = "~/" & P_RPE & "/" &
            P_FUN_REP & P_SCR_UPD & P_PAGE & "002" & "/" &
            P_FUN_REP & P_SCR_UPD & P_PAGE & "002.aspx"
    'トラブル処理票ファイルパス
    Const M_TRBL_DISP_PATH = "~/" & P_MAI & "/" &
            P_FUN_REQ & P_SCR_SEL & P_PAGE & "001" & "/" &
            P_FUN_REQ & P_SCR_SEL & P_PAGE & "001.aspx"
    '持参物品一覧ファイルパス
    Const M_BRNG_DISP_PATH = "~/" & P_MAI & "/" &
            P_FUN_REQ & P_SCR_LST & P_PAGE & "002" & "/" &
            P_FUN_REQ & P_SCR_LST & P_PAGE & "002.aspx"
    'ホールマスタ管理画面のパス
    Const M_HMST_DISP_PATH = "~/" & P_COM & "/" &
            P_FUN_COM & P_SCR_MEN & P_PAGE & "006" & "/" &
            P_FUN_COM & P_SCR_MEN & P_PAGE & "006.aspx"
    '対応履歴照会画面のパス
    Const M_CHST_DISP_PATH As String = "~/" & P_FLR & "/" &
            P_FUN_BRK & P_SCR_INQ & P_PAGE & "001" & "/" &
            P_FUN_BRK & P_SCR_INQ & P_PAGE & "001.aspx"

    '作業状況のステータスコード"08"
    Const M_WORK_STSCD = "08"

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
    Dim intEmpFlg As Integer = 0 '空フラグ

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
        pfSet_GridView(Me.grvList, M_MY_DISP_ID, 60, 9)

    End Sub

    ''' <summary>
    ''' ページロード処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim strKeyList As List(Of String) = Nothing 'セッションＫＥＹリスト

        'ボタンアクションの設定
        AddHandler Master.ppLeftButton1.Click, AddressOf btnUpdate_Click
        AddHandler Master.ppLeftButton2.Click, AddressOf btnDelete_Click
        AddHandler Master.ppLeftButton3.Click, AddressOf btnReqPrint_Click
        AddHandler Master.ppLeftButton4.Click, AddressOf btnHistPrint_Click
        AddHandler Master.ppRigthButton1.Click, AddressOf btnRepairReq_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnBringList_Click
        AddHandler Master.ppRigthButton3.Click, AddressOf btnTrblProcess_Click
        AddHandler Master.ppRigthButton4.Click, AddressOf Button1_Click
        'CMPSELP001-003
        AddHandler Master.ppRigthButton8.Click, AddressOf btnHallMstMng_Click
        AddHandler Master.ppRigthButton7.Click, AddressOf btnHallMstMng_Click
        'CMPSELP001-003 END

        'ドロップダウンリストアクションの設定
        AddHandler Me.ddlTransbfCls.ppDropDownList.SelectedIndexChanged, AddressOf ddlTransbfCls_SelectedIndexChanged
        AddHandler Me.ddlTransafCls.ppDropDownList.SelectedIndexChanged, AddressOf ddlTransafCls_SelectedIndexChanged
        Me.ddlTransbfCls.ppDropDownList.AutoPostBack = True
        Me.ddlTransafCls.ppDropDownList.AutoPostBack = True

        'テキストボックスアクションの設定
        AddHandler Me.txtTransbfCd.ppTextBox.TextChanged, AddressOf txtTransbfCd_TextChanged
        AddHandler Me.txtTransafCd.ppTextBox.TextChanged, AddressOf txtTransafCd_TextChanged
        AddHandler Me.txtReqUsr.ppTextBox.TextChanged, AddressOf txtReqUsr_TextChanged
        AddHandler Me.txtTBoxid.ppTextBox.TextChanged, AddressOf ctl_Change
        Me.txtTransbfCd.ppTextBox.AutoPostBack = True
        Me.txtTransafCd.ppTextBox.AutoPostBack = True
        Me.txtReqUsr.ppTextBox.AutoPostBack = True
        Me.txtTBoxid.ppTextBox.AutoPostBack = True


        AddHandler Me.txtMntTm.ppTextBox.TextChanged, AddressOf Calc_TextChanged
        AddHandler Me.txtPsnNum.ppTextBox.TextChanged, AddressOf Calc_TextChanged
        AddHandler Me.txtGbTm.ppTextBox.TextChanged, AddressOf Calc_TextChanged
        AddHandler Me.tmbStartTm.ppHourBox.TextChanged, AddressOf Calc_TextChanged
        AddHandler Me.tmbStartTm.ppMinBox.TextChanged, AddressOf Calc_TextChanged
        AddHandler Me.txtWrkCode.ppTextBox.TextChanged, AddressOf GetEmployee
        AddHandler Me.txtWrkUser.ppTextBox.TextChanged, AddressOf GetEmployee
        AddHandler Me.ddlCmp.ppDropDownList.SelectedIndexChanged, AddressOf sbuDdlChange

        Me.txtMntTm.ppTextBox.AutoPostBack = True
        Me.txtPsnNum.ppTextBox.AutoPostBack = True
        Me.txtGbTm.ppTextBox.AutoPostBack = True
        Me.tmbStartTm.ppHourBox.AutoPostBack = True
        Me.tmbStartTm.ppMinBox.AutoPostBack = True
        Me.txtWrkCode.ppTextBox.AutoPostBack = True
        Me.txtWrkUser.ppTextBox.AutoPostBack = True
        Me.ddlCmp.ppDropDownList.AutoPostBack = True

        Me.txaDealDtl.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txaDealDtl.MaxLength & """);")
        Me.txaDealDtl.Attributes.Add("onChange", "lenCheck(this,""" & Me.txaDealDtl.MaxLength & """);")

        If Not IsPostBack Then  '初回表示のみ

            '★排他情報用のグループ番号保管
            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            '★排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
            If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then

                Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)

            End If

            'プログラムID、画面名設定
            Master.ppProgramID = M_MY_DISP_ID
            Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)

            'パンくずリスト設定
            Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

            '各コマンドボタンの属性設定
            '--登録
            Me.btnEntry.OnClientClick =
                pfGet_OCClickMes("00008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "保守対応依頼書")
            '--追加（対応明細）
            Me.btnDetailInsert.OnClientClick =
                pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "対応内容")
            '--更新（対応明細）
            Me.btnDetailUpdate.OnClientClick =
                pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "選択した対応内容")
            '--削除
            Me.btnDetailDelete.OnClientClick =
                pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "選択した対応内容")
            '--更新
            Master.ppLeftButton1.Text = P_BTN_NM_UPD
            Master.ppLeftButton1.Visible = True
            Master.ppLeftButton1.OnClientClick =
                pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "保守対応依頼書")
            '--削除
            Master.ppLeftButton2.CausesValidation = False
            Master.ppLeftButton2.Text = P_BTN_NM_DEL
            Master.ppLeftButton2.Visible = True
            Master.ppLeftButton2.OnClientClick =
                pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "保守対応依頼書")

            '--依頼書印刷
            Master.ppLeftButton3.CausesValidation = False
            Master.ppLeftButton3.Text = "依頼書印刷"
            Master.ppLeftButton3.Visible = True
            Master.ppLeftButton3.OnClientClick =
                pfGet_OCClickMes("10002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "依頼書")
            '--履歴印刷
            Master.ppLeftButton4.CausesValidation = False
            Master.ppLeftButton4.Text = "履歴印刷"
            Master.ppLeftButton4.Visible = True
            Master.ppLeftButton4.OnClientClick =
                pfGet_OCClickMes("10002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "履歴")
            '--修理依頼書
            Master.ppRigthButton1.CausesValidation = False
            Master.ppRigthButton1.Text = "修理依頼書"
            Master.ppRigthButton1.Visible = True
            '--持参物品一覧
            Master.ppRigthButton2.CausesValidation = False
            Master.ppRigthButton2.Text = "持参物品一覧"
            Master.ppRigthButton2.Visible = True
            '--トラブル処理票
            Master.ppRigthButton3.CausesValidation = False
            Master.ppRigthButton3.Text = "トラブル処理票"
            Master.ppRigthButton3.Visible = True

            '--保守対応故障品
            Master.ppRigthButton4.CausesValidation = False
            Master.ppRigthButton4.Text = "保守対応故障品登録"
            Master.ppRigthButton4.Visible = True

            '--ホールマスタ管理 CMPSELP001-003
            Master.ppRigthButton8.CausesValidation = False
            Master.ppRigthButton8.Text = "ホールマスタ管理"
            Master.ppRigthButton8.Visible = True

            '--対応履歴照会     CMPSELP001-003
            Master.ppRigthButton7.CausesValidation = False
            Master.ppRigthButton7.Text = "対応履歴照会"
            Master.ppRigthButton7.Visible = True

            'ドロップダウンリスト設定
            msSetddlOccurCls()  '区分マスタ（ 0043:保守管理区分 ※発生区分 ）
            msSetddlRptsrc()    '申告元マスタ
            msSetddlStatus()    'ステータスマスタ（69：業務分類コード）
            msSetddlMntFlg()    '特別保守料金マスタ

            Master.ppLeftButton2.BackColor = Drawing.Color.FromArgb(255, 102, 102)          '削除
            btnDetailDelete.BackColor = Drawing.Color.FromArgb(255, 102, 102)

            txtInhCntnt1.ppTextBox.ForeColor = Drawing.Color.FromArgb(255, 255, 0, 0)
            txtInhCntnt2.ppTextBox.ForeColor = Drawing.Color.FromArgb(255, 255, 0, 0)

            '--------------------------------
            '2014/05/21 後藤　ここから
            '--------------------------------
            'msSetddlWrkdtl()    '作業内容マスタ
            '--------------------------------
            '2014/05/21 後藤　ここまで
            '--------------------------------

            '画面初期化
            msInitScreen()

            'セッション情報取得
            If mfGetSession(strKeyList) Then

                '画面遷移条件による設定
                Select Case ViewState(P_SESSION_TERMS)
                    Case ClsComVer.E_遷移条件.参照
                        'データ取得＆表示
                        If mfGetData(strKeyList(0)) Then
                            '申告日へフォーカス
                            Me.dtbRptDt.ppDateBox.Focus()
                        Else
                            '画面を終了する
                            psClose_Window(Me)
                        End If

                    Case ClsComVer.E_遷移条件.更新
                        'データ取得＆表示
                        If mfGetData(strKeyList(0)) Then
                            '対応コード（編集エリア）のデフォルト表示（"GR"+保守管理番号の下3桁）
                            Me.txtDealCd.ppText = "GR" + strKeyList(0).Substring(strKeyList(0).Length - 3, 3)

                            '申告日へフォーカス
                            Me.dtbRptDt.ppDateBox.Focus()

                        Else
                            '画面を終了する
                            '--------------------------------
                            '2014/06/11 後藤　ここから
                            '--------------------------------
                            '排他削除
                            clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                            '--------------------------------
                            '2014/06/11 後藤　ここまで
                            '--------------------------------
                            psClose_Window(Me)
                        End If

                    Case ClsComVer.E_遷移条件.登録
                        Select Case ViewState(P_SESSION_OLDDISP)
                            Case M_TRBL_DISP_ID
                                'ＴＢＯＸＩＤ、トラブル管理番号の表示
                                Me.lblMntNo.Text = strKeyList(0)            '保守管理番号
                                Me.lblTrbNo.Text = strKeyList(1)            'トラブル管理番号
                                Me.txtTBoxid.ppText = strKeyList(2)         'ＴＢＯＸＩＤ

                                'ＴＢＯＸ情報取得
                                If mfGetTboxInfo() Then
                                    '発生区分へフォーカス
                                    Me.ddlOccurCls.Focus()
                                Else
                                    '画面を終了する
                                    psClose_Window(Me)
                                End If

                                Me.dtbRptDt.ppText = strKeyList(3)          '申告日
                                Me.txtRptCharge.ppText = strKeyList(4)      '申告者
                                Me.ddlRptBase.SelectedValue = strKeyList(5) '申告元コード
                                Me.lblRptTel.Text = strKeyList(6)           '申告者ＴＥＬ
                                If strKeyList(7).Length >= 16 Then
                                    Me.dtbRcptDt.ppText = strKeyList(7).Substring(0, 10)        '受付日時（日付）
                                    Me.dtbRcptDt.ppHourText = strKeyList(7).Substring(11, 2)    '受付日時（時）
                                    Me.dtbRcptDt.ppMinText = strKeyList(7).Substring(14, 2)     '受付日時（分）
                                End If
                                Me.txtRcptCharge.ppText = strKeyList(8)     '受付者
                                Me.ddlRpt.SelectedValue = strKeyList(9)     '申告内容コード
                                Me.txtRptDtl1.ppText = strKeyList(10)     '申告内容１
                                Me.txtRptDtl2.ppText = strKeyList(11)     '申告内容２
                            Case M_HALMST_DISP_ID 'CMPSELP001-002 ホールマスタ管理
                                Me.txtTBoxid.ppText = strKeyList(0) 'ＴＢＯＸＩＤ
                                'TBOX情報取得
                                ctl_Change(sender, e)
                                '発生区分へフォーカス
                                Me.ddlOccurCls.Focus()
                            Case Else
                                '発生区分へフォーカス
                                Me.ddlOccurCls.Focus()
                        End Select
                End Select

                If Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 And ddlStatus.SelectedValue = "12" Then
                    msSetActivecontrol(1)
                    Me.pnlMnt3.Enabled = True  'パネル３
                    Me.ddlRpt.Enabled = False
                    Me.cbxImpCls.Enabled = False
                    Me.txtRptDtl1.ppEnabled = False
                    Me.txtRptDtl2.ppEnabled = False
                    Me.cbxBsnsdistCls.Enabled = False
                    Me.cbxScnddistCls.Enabled = False
                    Me.txtInfo1.ppEnabled = False
                    Me.txtInfo2.ppEnabled = False
                    Me.dtbWrkDt.ppEnabled = False
                    Me.dtbDeptDt.ppEnabled = False
                    Me.ddlCmp.ppEnabled = False
                    Me.txtWrkCode.ppEnabled = False
                    Me.txtWrkUser.ppEnabled = False
                    Me.dtbStartDt.ppEnabled = False
                    Me.dtbEndDt.ppEnabled = False
                    Me.ddlAppa.ppEnabled = False
                    Me.txtSttsNotetext.ppEnabled = False
                    Me.ddlRepair.Enabled = False
                    Me.txtRepair.ppEnabled = False
                    Me.txtNotetext1.ppEnabled = False
                    Me.txtNotetext2.ppEnabled = False
                    Me.ddlStatus.Enabled = True
                    Master.ppLeftButton1.Enabled = True     '--更新
                    Master.ppLeftButton2.Enabled = False    '--削除
                    Master.ppLeftButton3.Enabled = True     '--依頼書印刷
                    Master.ppLeftButton4.Enabled = True     '--履歴印刷
                    Master.ppRigthButton1.Enabled = True    '--修理依頼書
                    Master.ppRigthButton2.Enabled = True    '--持参物品一覧
                    Master.ppRigthButton3.Enabled = True    '--トラブル処理票
                    Master.ppRigthButton4.Enabled = False    '--保守対応故障品
                    Me.txtWrkTel.ppEnabled = False
                ElseIf Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 And ddlStatus.SelectedValue = "13" Then
                    msSetActivecontrol(1)
                    Me.pnlMnt3.Enabled = True  'パネル３
                    Me.ddlRpt.Enabled = False
                    Me.cbxImpCls.Enabled = False
                    Me.txtRptDtl1.ppEnabled = False
                    Me.txtRptDtl2.ppEnabled = False
                    Me.cbxBsnsdistCls.Enabled = False
                    Me.cbxScnddistCls.Enabled = False
                    Me.txtInfo1.ppEnabled = False
                    Me.txtInfo2.ppEnabled = False
                    Me.dtbWrkDt.ppEnabled = False
                    Me.dtbDeptDt.ppEnabled = False
                    Me.ddlCmp.ppEnabled = False
                    Me.txtWrkCode.ppEnabled = False
                    Me.txtWrkUser.ppEnabled = False
                    Me.dtbStartDt.ppEnabled = False
                    Me.dtbEndDt.ppEnabled = False
                    Me.ddlAppa.ppEnabled = False
                    Me.txtSttsNotetext.ppEnabled = False
                    Me.ddlRepair.Enabled = False
                    Me.txtRepair.ppEnabled = False
                    Me.txtNotetext1.ppEnabled = False
                    Me.txtNotetext2.ppEnabled = False
                    Me.ddlStatus.Enabled = True
                    Master.ppLeftButton1.Enabled = True     '--更新
                    Master.ppLeftButton2.Enabled = True    '--削除
                    Master.ppLeftButton3.Enabled = True     '--依頼書印刷
                    Master.ppLeftButton4.Enabled = True     '--履歴印刷
                    Master.ppRigthButton1.Enabled = True    '--修理依頼書
                    Master.ppRigthButton2.Enabled = True    '--持参物品一覧
                    Master.ppRigthButton3.Enabled = True    '--トラブル処理票
                    Master.ppRigthButton4.Enabled = False    '--保守対応故障品
                    Me.txtWrkTel.ppEnabled = False
                Else
                    '各コントロールの活性・非活性
                    msSetActivecontrol(ViewState(P_SESSION_TERMS))
                    Master.ppLeftButton2.Enabled = False    '--削除
                End If
            Else
                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                '--------------------------------
                '2014/06/11 後藤　ここから
                '--------------------------------
                '排他削除
                clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                '--------------------------------
                '2014/06/11 後藤　ここまで
                '--------------------------------
                psClose_Window(Me)
            End If

        End If

    End Sub

    Private Sub sbuDdlChange(sender As Object, e As EventArgs)
        Me.txtWrkCode.ppText = ""
        If Me.ddlCmp.ppSelectedValue <> "1" Then
            Me.txtWrkCode.ppTextBox.Enabled = False
        Else
            Me.txtWrkCode.ppTextBox.Enabled = True
        End If
    End Sub

    '---------------------------
    '2014/04/15 武 ここから
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
                ddlOccurCls.Enabled = False
                txtTBoxid.ppEnabled = False
                btnEntry.Enabled = False
                dtbRptDt.ppEnabled = False
                txtRptCharge.ppEnabled = False
                ddlRptBase.Enabled = False
                dtbRcptDt.ppEnabled = False
                txtRcptCharge.ppEnabled = False
                dtbReqDt.ppEnabled = False
                txtReqUsr.ppEnabled = False
                txtHallCharge.ppEnabled = False
                dtbLastDt.ppEnabled = False
                txtInfo1.ppEnabled = False
                txtInfo2.ppEnabled = False
                ddlRpt.Enabled = False
                txtRptDtl1.ppEnabled = False
                txtRptDtl2.ppEnabled = False
                cbxImpCls.Enabled = False
                txtInhCntnt1.ppEnabled = False
                txtInhCntnt2.ppEnabled = False
                cbxBsnsdistCls.Enabled = False
                cbxScnddistCls.Enabled = False
                dtbStartDt.ppEnabled = False
                dtbEndDt.ppEnabled = False
                ddlStatus.Enabled = False
                txtSttsNotetext.ppEnabled = False
                ddlAppa.ppEnabled = False
                ddlRepair.Enabled = False
                txtRepair.ppEnabled = False
                txtNotetext1.ppEnabled = False
                txtNotetext2.ppEnabled = False
                cbxDealAdptCls.Enabled = False
                txtDealCd.ppEnabled = False
                dtbDealDt.ppEnabled = False
                txtDealUsr.ppEnabled = False
                btnDetailInsert.Enabled = False
                btnDetailUpdate.Enabled = False
                btnDetailDelete.Enabled = False
                txaDealDtl.Enabled = False
                ddlMntFlg.Enabled = False
                tmbDeptTm.ppEnabled = False
                tmbStartTm.ppEnabled = False
                tmbEndTm.ppEnabled = False
                ddlWrk.Enabled = False
                txtMntTm.ppEnabled = False
                txtGbTm.ppEnabled = False
                txtPsnNum.ppEnabled = False
                txtReqPrice.ppEnabled = False
                dtbSubmitDt.ppEnabled = False
                ddlRepairCls.ppEnabled = False
                txtNotetext.ppEnabled = False
                ddlDeal.Enabled = False
                txtDeal.ppEnabled = False
                dtbTransDt.ppEnabled = False
                ddlTransbfCls.ppEnabled = False
                txtTransbfCd.ppEnabled = False
                ddlTransafCls.ppEnabled = False
                txtTransafCd.ppEnabled = False
                txtTransItem.ppEnabled = False
                ddlTransRsn.ppEnabled = False
                ddlTransCls.ppEnabled = False
                txtTransPrice.ppEnabled = False
                txtTransComp.ppEnabled = False
                Master.ppLeftButton2.Enabled = False
                Master.ppRigthButton3.Enabled = False
                Master.ppRigthButton4.Enabled = False
                Master.ppRigthButton1.Enabled = False
                Me.txtWrkTel.ppEnabled = False
            Case "NGC"
        End Select

        If lblTrbNo.Text = String.Empty Then
            Master.ppRigthButton3.Enabled = False
        End If

        If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 Then
            Dim dt As DateTime = DateTime.Now
            If dtbRptDt.ppText = String.Empty Then
                dtbRptDt.ppText = dt.ToString("yyyy/MM/dd")
            End If
            If dtbRcptDt.ppText = String.Empty AndAlso dtbRcptDt.ppHourText = String.Empty AndAlso dtbRcptDt.ppMinText = String.Empty Then
                dtbRcptDt.ppText = dt.ToString("yyyy/MM/dd")
                dtbRcptDt.ppHourText = dt.ToString("HH")
                dtbRcptDt.ppMinText = dt.ToString("mm")
            End If
            If intEmpFlg = 0 Then
                If dtbReqDt.ppText = String.Empty AndAlso dtbReqDt.ppHourText = String.Empty AndAlso dtbReqDt.ppMinText = String.Empty Then
                    'dtbReqDt.ppText = dt.ToString("yyyy/MM/dd")
                    'dtbReqDt.ppHourText = dt.ToString("HH")
                    'dtbReqDt.ppMinText = dt.ToString("mm")
                End If
            Else
                dtbReqDt.ppText = String.Empty
                dtbReqDt.ppHourText = String.Empty
                dtbReqDt.ppMinText = String.Empty
            End If
            If dtbDealDt.ppText = String.Empty AndAlso dtbDealDt.ppHourText = String.Empty AndAlso dtbDealDt.ppMinText = String.Empty Then
                dtbDealDt.ppText = dt.ToString("yyyy/MM/dd")
                dtbDealDt.ppHourText = dt.ToString("HH")
                dtbDealDt.ppMinText = dt.ToString("mm")
            End If
        End If

        '保守機ＴＢＯＸＩＤの場合、ボタンを使用不可とする
        Dim blnResult As Boolean = False
        Call sGetMB1Info(blnResult)
        If blnResult = True Then
            '            AddHandler Master.ppLeftButton3.Click, AddressOf btnReqPrint_Click
            '            AddHandler Master.ppLeftButton4.Click, AddressOf btnHistPrint_Click
            '            AddHandler Master.ppRigthButton1.Click, AddressOf btnRepairReq_Click
            Master.ppRigthButton2.Enabled = False
            Master.ppRigthButton3.Enabled = False
            Master.ppRigthButton4.Enabled = False
            Master.ppRigthButton8.Enabled = False
            Master.ppRigthButton7.Enabled = False
        End If


    End Sub

    Private Sub grvList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grvList.RowDataBound

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
                e.Row.Cells(0).Enabled = True
            Case "NGC"
        End Select

    End Sub
    '---------------------------
    '2014/04/14 武 ここまで
    '---------------------------

    ''' <summary>
    ''' 登録ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnEntry_Click(sender As Object, e As EventArgs) Handles btnEntry.Click

        Dim strMntNo As String = String.Empty   '保守管理番号
        Dim drMsg As DataRow = Nothing          '検証メッセージ取得
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        '開始ログ出力
        psLogStart(Me)

        '発生区分必須入力チェック
        If Me.ddlOccurCls.SelectedValue = String.Empty Then
            drMsg = pfGet_ValMes("5003", "発生区分")
            valOccurCls.ErrorMessage = drMsg.Item(P_VALMES_MES)
            valOccurCls.Text = drMsg.Item(P_VALMES_SMES)
            valOccurCls.IsValid = False
            Me.ddlOccurCls.Focus()
        End If

        Dim blnResult As Boolean = False
        Call sGetMB1Info(blnResult)
        If blnResult = True And Me.ddlOccurCls.SelectedValue <> "7" Then
            drMsg = pfGet_ValMes("2010", "7:S前故障（保守のみ）以外", "保守機ＴＢＯＸＩＤ")
            valOccurCls.ErrorMessage = drMsg.Item(P_VALMES_MES)
            valOccurCls.Text = drMsg.Item(P_VALMES_SMES)
            valOccurCls.IsValid = False
            Me.ddlOccurCls.Focus()
        End If

        '入力内容検証
        If (Page.IsValid) Then
            'ＴＢＯＸ情報取得
            If mfGetTboxInfo() Then
                '保守管理番号を採番し保守対応データ（D75_DEAL_MAINTAIN）を登録する
                If mfEntryData(strMntNo) Then
                    ''データ取得＆表示
                    'If mfGetData(strMntNo) Then
                    Me.lblMntNo.Text = strMntNo
                    '対応コード（編集エリア）のデフォルト表示（"GR"+保守管理番号の下3桁）
                    Me.txtDealCd.ppText = "GR" + strMntNo.Substring(strMntNo.Length - 3, 3)

                    '各コントロールの活性・非活性を更新モードにする
                    msSetActivecontrol(2)

                    '遷移条件を更新モードにする
                    ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新

                    'DB接続
                    If Not clsDataConnect.pfOpen_Database(objCn) Then
                        psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    Else
                        Try

                            '--保守対応明細データの取得
                            objCmd = New SqlCommand(M_MY_DISP_ID + "_S2", objCn)
                            With objCmd.Parameters
                                '--パラメータ設定
                                '保守管理番号
                                .Add(pfSet_Param("mntno", SqlDbType.NVarChar, strMntNo))
                                .Add(pfSet_Param("auth", SqlDbType.NVarChar, Session(P_SESSION_AUTH).ToString))
                            End With

                            'データ取得
                            objDs = clsDataConnect.pfGet_DataSet(objCmd)

                            '取得したデータをグリッドに設定
                            Me.grvList.DataSource = objDs.Tables(0)

                            '変更を反映
                            Me.grvList.DataBind()


                            '申告日へフォーカス
                            Me.dtbRptDt.ppDateBox.Focus()
                            'End If

                        Catch ex As Exception
                            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "保守対応依頼書")
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
                                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                            End If
                        End Try
                    End If
                End If
            End If
        End If


        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 更新ボタン（フッター部）クリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        '整合性チェック
        msCheckDealMiantain()

        '特別保守フラグ
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        Else
            Try
                'データ取得
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S1", objCn)
                With objCmd.Parameters
                    .Add(pfSet_Param("mntno", SqlDbType.NVarChar, Me.lblMntNo.Text))
                End With
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "保守対応依頼書")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        End If

        If objDs.Tables.Count = 0 Then
            Exit Sub
        End If

        'If ViewState("APP_UPD_CLS") > "1" AndAlso ViewState("MNT_FLG") <> Me.ddlMntFlg.SelectedValue Then
        If objDs.Tables(0).Rows(0).Item("検収区分").ToString = "1" AndAlso objDs.Tables(0).Rows(0).Item("特別保守フラグ").ToString <> Me.ddlMntFlg.SelectedValue Then
            Me.lblInsApp.Text = objDs.Tables(0).Rows(0).Item("検収承認").ToString
            Me.lblReqApp.Text = objDs.Tables(0).Rows(0).Item("請求承認").ToString
            psMesBox(Me, "20007", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前)
        Else
            '入力内容検証
            If (Page.IsValid) Then
                '更新処理
                mfUpdateData(Me.lblMntNo.Text)

                Me.lblInsApp.Text = objDs.Tables(0).Rows(0).Item("検収承認").ToString
                Me.lblReqApp.Text = objDs.Tables(0).Rows(0).Item("請求承認").ToString

                If ddlStatus.SelectedValue = "12" Then
                    msSetActivecontrol(1)
                    Me.pnlMnt3.Enabled = True  'パネル３
                    Me.ddlRpt.Enabled = False
                    Me.cbxImpCls.Enabled = False
                    Me.txtRptDtl1.ppEnabled = False
                    Me.txtRptDtl2.ppEnabled = False
                    Me.cbxBsnsdistCls.Enabled = False
                    Me.cbxScnddistCls.Enabled = False
                    Me.txtInfo1.ppEnabled = False
                    Me.txtInfo2.ppEnabled = False
                    Me.dtbWrkDt.ppEnabled = False
                    Me.dtbDeptDt.ppEnabled = False
                    Me.ddlCmp.ppEnabled = False
                    Me.txtWrkCode.ppEnabled = False
                    Me.txtWrkUser.ppEnabled = False
                    Me.dtbStartDt.ppEnabled = False
                    Me.dtbEndDt.ppEnabled = False
                    Me.ddlAppa.ppEnabled = False
                    Me.txtSttsNotetext.ppEnabled = False
                    Me.ddlRepair.Enabled = False
                    Me.txtRepair.ppEnabled = False
                    Me.txtNotetext1.ppEnabled = False
                    Me.txtNotetext2.ppEnabled = False
                    Me.ddlStatus.Enabled = True
                    Master.ppLeftButton1.Enabled = True     '--更新
                    Master.ppLeftButton2.Enabled = False    '--削除
                    Master.ppLeftButton3.Enabled = True     '--依頼書印刷
                    Master.ppLeftButton4.Enabled = True     '--履歴印刷
                    Master.ppRigthButton1.Enabled = True    '--修理依頼書
                    Master.ppRigthButton2.Enabled = True    '--持参物品一覧
                    Master.ppRigthButton3.Enabled = True    '--トラブル処理票
                    Master.ppRigthButton4.Enabled = False    '--保守対応故障品
                    Me.txtWrkTel.ppEnabled = False
                ElseIf ddlStatus.SelectedValue = "13" Then
                    msSetActivecontrol(1)
                    Me.pnlMnt3.Enabled = True  'パネル３
                    Me.ddlRpt.Enabled = False
                    Me.cbxImpCls.Enabled = False
                    Me.txtRptDtl1.ppEnabled = False
                    Me.txtRptDtl2.ppEnabled = False
                    Me.cbxBsnsdistCls.Enabled = False
                    Me.cbxScnddistCls.Enabled = False
                    Me.txtInfo1.ppEnabled = False
                    Me.txtInfo2.ppEnabled = False
                    Me.dtbWrkDt.ppEnabled = False
                    Me.dtbDeptDt.ppEnabled = False
                    Me.ddlCmp.ppEnabled = False
                    Me.txtWrkCode.ppEnabled = False
                    Me.txtWrkUser.ppEnabled = False
                    Me.dtbStartDt.ppEnabled = False
                    Me.dtbEndDt.ppEnabled = False
                    Me.ddlAppa.ppEnabled = False
                    Me.txtSttsNotetext.ppEnabled = False
                    Me.ddlRepair.Enabled = False
                    Me.txtRepair.ppEnabled = False
                    Me.txtNotetext1.ppEnabled = False
                    Me.txtNotetext2.ppEnabled = False
                    Me.ddlStatus.Enabled = True
                    Master.ppLeftButton1.Enabled = True     '--更新
                    Master.ppLeftButton2.Enabled = True    '--削除
                    Master.ppLeftButton3.Enabled = True     '--依頼書印刷
                    Master.ppLeftButton4.Enabled = True     '--履歴印刷
                    Master.ppRigthButton1.Enabled = True    '--修理依頼書
                    Master.ppRigthButton2.Enabled = True    '--持参物品一覧
                    Master.ppRigthButton3.Enabled = True    '--トラブル処理票
                    Master.ppRigthButton4.Enabled = False    '--保守対応故障品
                    Me.txtWrkTel.ppEnabled = False
                Else
                    '各コントロールの活性・非活性
                    msSetActivecontrol(ViewState(P_SESSION_TERMS))
                    Master.ppLeftButton2.Enabled = False    '--削除
                    Master.ppRigthButton4.Enabled = True    '--保守対応故障品
                End If
            End If
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 削除ボタン（フッター部）クリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDelete_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        '削除処理
        If mfDeleteData(Me.lblMntNo.Text) Then
            ''各コントロールの活性・非活性を削除モードにする
            'msSetActivecontrol(4)
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 依頼書印刷ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnReqPrint_Click(sender As Object, e As EventArgs)

        Dim objDs As DataSet = Nothing  'データセット
        Dim rpt As Object = Nothing     'ActiveReports用クラス

        '開始ログ出力
        psLogStart(Me)

        'データ取得処理
        objDs = mfGetDataMntReq()

        '保守対応依頼書印刷
        If Not objDs Is Nothing Then
            '帳票出力
            rpt = New REQREP001
            psPrintPDF(Me, rpt, objDs.Tables(0), Master.ppTitle)
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 履歴印刷ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnHistPrint_Click(sender As Object, e As EventArgs)

        Dim objDs As DataSet = Nothing  'データセット
        Dim rpt As Object = Nothing     'ActiveReports用クラス

        '開始ログ出力
        psLogStart(Me)

        'データ取得処理
        objDs = mfGetDataMntHistLst()

        '保守対応状況リスト印刷
        If Not objDs Is Nothing Then
            '帳票出力
            rpt = New REQREP002
            psPrintPDF(Me, rpt, objDs.Tables(0), "保守対応状況リスト")
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 修理依頼書ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnRepairReq_Click(sender As Object, e As EventArgs)

        Dim strKeyList As List(Of String) = Nothing '画面引継ぎ用キー情報

        '開始ログ出力
        psLogStart(Me)

        '画面引継ぎ用キー情報設定
        strKeyList = New List(Of String)
        strKeyList.Add(Me.lblMntNo.Text)    '保守管理番号

        'セッション情報設定																	
        Session(P_SESSION_BCLIST) = Master.ppBcList_Text
        Session(P_SESSION_OLDDISP) = M_MY_DISP_ID
        Session(P_KEY) = strKeyList.ToArray
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)
        Session(P_SESSION_TERMS) = ViewState(P_SESSION_TERMS)

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
                        objStack.GetMethod.Name, M_REPR_DISP_PATH, strPrm, "TRANS")
        '■□■□結合試験時のみ使用予定□■□■
        '--------------------------------
        '2014/04/16 星野　ここまで
        '--------------------------------
        '修理依頼書画面起動
        psOpen_Window(Me, M_REPR_DISP_PATH)

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' トラブル処理票ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnTrblProcess_Click(sender As Object, e As EventArgs)

        Dim strKeyList As List(Of String) = Nothing '画面引継ぎ用キー情報

        '開始ログ出力
        psLogStart(Me)

        '画面引継ぎ用キー情報設定
        strKeyList = New List(Of String)
        strKeyList.Add(Me.lblTrbNo.Text)    '保守管理番号
        strKeyList.Add(Me.txtTBoxid.ppText) 'ＴＢＯＸＩＤ

        'セッション情報設定																	
        Session(P_SESSION_BCLIST) = Master.ppBcList_Text
        Session(P_SESSION_OLDDISP) = M_MY_DISP_ID
        Session(P_KEY) = strKeyList.ToArray
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)
        Session(P_SESSION_TERMS) = ViewState(P_SESSION_TERMS)

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
                        objStack.GetMethod.Name, M_TRBL_DISP_PATH, strPrm, "TRANS")
        '■□■□結合試験時のみ使用予定□■□■
        '--------------------------------
        '2014/04/16 星野　ここまで
        '--------------------------------
        'トラブル処理票画面起動
        psOpen_Window(Me, M_TRBL_DISP_PATH)

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 持参物品一覧ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnBringList_Click(sender As Object, e As EventArgs)

        Dim strKeyList As List(Of String) = Nothing '画面引継ぎ用キー情報
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット


        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        Else
            Try
                ''--保守対応データの取得
                'objCmd = New SqlCommand(M_MY_DISP_ID + "_S1", objCn)
                'With objCmd.Parameters
                '    '--パラメータ設定
                '    '保守管理番号
                '    .Add(pfSet_Param("mntno", SqlDbType.NVarChar, Me.lblMntNo.Text))
                'End With

                ''データ取得
                'objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'If objDs.Tables(0).Rows(0).Item("作業予定日時：日").ToString = "" Then
                '    psMesBox(Me, "30016", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前, "作業予定日時")
                '    Exit Sub
                'End If

                '開始ログ出力
                psLogStart(Me)

                '画面引継ぎ用キー情報設定
                strKeyList = New List(Of String)
                strKeyList.Add(Me.lblMntNo.Text)    '保守管理番号
                strKeyList.Add(Me.txtTBoxid.ppText)    'TBOXID

                'セッション情報設定																	
                Session(P_SESSION_BCLIST) = Master.ppBcList_Text
                Session(P_SESSION_OLDDISP) = M_MY_DISP_ID
                Session(P_KEY) = strKeyList.ToArray
                Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)
                Session(P_SESSION_TERMS) = ViewState(P_SESSION_TERMS)

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
                                objStack.GetMethod.Name, M_BRNG_DISP_PATH, strPrm, "TRANS")
                '■□■□結合試験時のみ使用予定□■□■
                '--------------------------------
                '2014/04/16 星野　ここまで
                '--------------------------------
                '持参物品一覧画面起動
                psOpen_Window(Me, M_BRNG_DISP_PATH)

                '終了ログ出力
                psLogEnd(Me)
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle)
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
    ''' 作業状況ドロップダウンリスト変更時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlStatus.SelectedIndexChanged
        If ddlStatus.SelectedValue <> "12" And ddlStatus.SelectedValue <> "13" Then
            msSetActivecontrol(2)
            Me.ddlRpt.Enabled = True
            Me.cbxImpCls.Enabled = True
            Me.txtRptDtl1.ppEnabled = True
            Me.txtRptDtl2.ppEnabled = True
            Me.cbxBsnsdistCls.Enabled = True
            Me.cbxScnddistCls.Enabled = True
            Me.txtInfo1.ppEnabled = True
            Me.txtInfo2.ppEnabled = True
            Me.dtbWrkDt.ppEnabled = True
            Me.dtbDeptDt.ppEnabled = True
            Me.ddlCmp.ppEnabled = True
            Me.txtWrkCode.ppEnabled = True
            Me.txtWrkUser.ppEnabled = True
            Me.dtbStartDt.ppEnabled = True
            Me.dtbEndDt.ppEnabled = True
            Me.ddlAppa.ppEnabled = True
            Me.txtSttsNotetext.ppEnabled = True
            Me.ddlRepair.Enabled = True
            Me.txtRepair.ppEnabled = True
            Me.txtNotetext1.ppEnabled = True
            Me.txtNotetext2.ppEnabled = True
            Me.ddlStatus.Enabled = True
            Me.cbxDealAdptCls.Enabled = True
            Me.txtDealCd.ppEnabled = True
            Me.dtbDealDt.ppEnabled = True
            Me.txtDealUsr.ppEnabled = True
            Me.txaDealDtl.Enabled = True
            Me.btnDetailInsert.Enabled = True
            Me.btnDetailClear.Enabled = True
            Master.ppLeftButton1.Enabled = True     '--更新
            Master.ppLeftButton2.Enabled = False    '--削除
            Master.ppLeftButton3.Enabled = True     '--依頼書印刷
            Master.ppLeftButton4.Enabled = True     '--履歴印刷
            Master.ppRigthButton1.Enabled = True    '--修理依頼書
            Master.ppRigthButton2.Enabled = True    '--持参物品一覧
            Master.ppRigthButton3.Enabled = True    '--トラブル処理票
            Master.ppRigthButton4.Enabled = True    '--保守対応故障品
            Me.txtWrkTel.ppEnabled = True
        Else
            If Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 Then
                If ddlStatus.SelectedValue = "12" Then
                    msSetActivecontrol(1)
                    Me.pnlMnt3.Enabled = True  'パネル３
                    Me.ddlRpt.Enabled = False
                    Me.cbxImpCls.Enabled = False
                    Me.txtRptDtl1.ppEnabled = False
                    Me.txtRptDtl2.ppEnabled = False
                    Me.cbxBsnsdistCls.Enabled = False
                    Me.cbxScnddistCls.Enabled = False
                    Me.txtInfo1.ppEnabled = False
                    Me.txtInfo2.ppEnabled = False
                    Me.dtbWrkDt.ppEnabled = False
                    Me.dtbDeptDt.ppEnabled = False
                    Me.ddlCmp.ppEnabled = False
                    Me.txtWrkCode.ppEnabled = False
                    Me.txtWrkUser.ppEnabled = False
                    Me.dtbStartDt.ppEnabled = False
                    Me.dtbEndDt.ppEnabled = False
                    Me.ddlAppa.ppEnabled = False
                    Me.txtSttsNotetext.ppEnabled = False
                    Me.ddlRepair.Enabled = False
                    Me.txtRepair.ppEnabled = False
                    Me.txtNotetext1.ppEnabled = False
                    Me.txtNotetext2.ppEnabled = False
                    Me.ddlStatus.Enabled = True
                    Master.ppLeftButton1.Enabled = True     '--更新
                    Master.ppLeftButton2.Enabled = False    '--削除
                    Master.ppLeftButton3.Enabled = True     '--依頼書印刷
                    Master.ppLeftButton4.Enabled = True     '--履歴印刷
                    Master.ppRigthButton1.Enabled = True    '--修理依頼書
                    Master.ppRigthButton2.Enabled = True    '--持参物品一覧
                    Master.ppRigthButton3.Enabled = True    '--トラブル処理票
                    Master.ppRigthButton4.Enabled = False    '--保守対応故障品
                    Me.txtWrkTel.ppEnabled = False
                ElseIf ddlStatus.SelectedValue = "13" Then
                    msSetActivecontrol(1)
                    Me.pnlMnt3.Enabled = True  'パネル３
                    Me.ddlRpt.Enabled = False
                    Me.cbxImpCls.Enabled = False
                    Me.txtRptDtl1.ppEnabled = False
                    Me.txtRptDtl2.ppEnabled = False
                    Me.cbxBsnsdistCls.Enabled = False
                    Me.cbxScnddistCls.Enabled = False
                    Me.txtInfo1.ppEnabled = False
                    Me.txtInfo2.ppEnabled = False
                    Me.dtbWrkDt.ppEnabled = False
                    Me.dtbDeptDt.ppEnabled = False
                    Me.ddlCmp.ppEnabled = False
                    Me.txtWrkCode.ppEnabled = False
                    Me.txtWrkUser.ppEnabled = False
                    Me.dtbStartDt.ppEnabled = False
                    Me.dtbEndDt.ppEnabled = False
                    Me.ddlAppa.ppEnabled = False
                    Me.txtSttsNotetext.ppEnabled = False
                    Me.ddlRepair.Enabled = False
                    Me.txtRepair.ppEnabled = False
                    Me.txtNotetext1.ppEnabled = False
                    Me.txtNotetext2.ppEnabled = False
                    Me.ddlStatus.Enabled = True
                    Master.ppLeftButton1.Enabled = True     '--更新
                    Master.ppLeftButton2.Enabled = True    '--削除
                    Master.ppLeftButton3.Enabled = True     '--依頼書印刷
                    Master.ppLeftButton4.Enabled = True     '--履歴印刷
                    Master.ppRigthButton1.Enabled = True    '--修理依頼書
                    Master.ppRigthButton2.Enabled = True    '--持参物品一覧
                    Master.ppRigthButton3.Enabled = True    '--トラブル処理票
                    Master.ppRigthButton4.Enabled = False    '--保守対応故障品
                    Me.txtWrkTel.ppEnabled = False
                End If
            End If
        End If

        '選択ステータスコード＝"08"の場合は入力可能にする
        If ddlStatus.SelectedValue = M_WORK_STSCD Then
            Me.txtSttsNotetext.ppEnabled = True
        Else
            Me.txtSttsNotetext.ppText = String.Empty
            Me.txtSttsNotetext.ppEnabled = False
        End If

    End Sub

    ''' <summary>
    ''' 作業内容マスタドロップダウンリスト変更時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlWrk_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlWrk.SelectedIndexChanged

        '作業内容マスタ情報の設定
        msSetWrkInfo()
        Calc_TextChanged(sender, e)
    End Sub

    ''' <summary>
    ''' TBOXID入力時の動き
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ctl_Change(ByVal sender As Object, ByVal e As System.Object)

        objStack = New StackFrame

        Try
            '開始ログ出力
            psLogStart(Me)

            '入力がない場合
            If txtTBoxid.ppText.Trim() = "" Then
                'TBOX情報の初期化
                msInitTBoxData()
                Return
            Else
                'TBOXデータの設定
                mfGetTboxInfo()
            End If

        Catch ex As Exception
            'TBOX情報初期化
            msInitTBoxData()
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "TBOX情報の検索")
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
            '終了ログ出力
            psLogEnd(Me)
        End Try
    End Sub

    ''' <summary>
    ''' 対応明細クリアボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDetailClear_Click(sender As Object, e As EventArgs) Handles btnDetailClear.Click

        '開始ログ出力
        psLogStart(Me)

        '編集項目クリア
        msClearDetail()

        '対応コードにフォーカス
        Me.txtDealCd.ppTextBox.Focus()

        '対応コード（編集エリア）のデフォルト表示（"GR"+保守管理番号の下3桁）
        Me.txtDealCd.ppText = "GR" + lblMntNo.Text.Substring(lblMntNo.Text.Length - 3, 3)
        '---------------------------
        '2014/06/24 武 ここから
        '---------------------------
        '参照時には選択したときのみクリアボタンを押下できる
        If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照 Then
            Me.btnDetailClear.Enabled = False
            Me.btnDetailInsert.Enabled = False
            Me.btnDetailUpdate.Enabled = False
            Me.btnDetailDelete.Enabled = False
        End If
        '---------------------------
        '2014/06/24 武 ここから
        '---------------------------
        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 対応明細追加ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDetailInsert_Click(sender As Object, e As EventArgs) Handles btnDetailInsert.Click

        '開始ログ出力
        psLogStart(Me)

        '整合性チェック
        msCheckDealMiantainDtil()

        '入力内容検証
        If (Page.IsValid) Then
            '明細追加処理
            If mfUpdateDataDetail(1) Then
                'データ取得（対応明細）
                msGetDataDetail()

                '編集項目クリア
                msClearDetail()

                '対応コード（編集エリア）のデフォルト表示（"GR"+保守管理番号の下3桁）
                Me.txtDealCd.ppText = "GR" + lblMntNo.Text.Substring(lblMntNo.Text.Length - 3, 3)

                '対応コードにフォーカス
                Me.txtDealCd.ppTextBox.Focus()
            End If
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 対応明細更新ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDetailUpdate_Click(sender As Object, e As EventArgs) Handles btnDetailUpdate.Click

        '開始ログ出力
        psLogStart(Me)

        '整合性チェック
        msCheckDealMiantainDtil()

        '入力内容検証
        If (Page.IsValid) Then
            '明細更新処理
            If mfUpdateDataDetail(2) Then
                'データ取得（対応明細）
                msGetDataDetail()

                '編集項目クリア
                msClearDetail()

                '対応コード（編集エリア）のデフォルト表示（"GR"+保守管理番号の下3桁）
                Me.txtDealCd.ppText = "GR" + lblMntNo.Text.Substring(lblMntNo.Text.Length - 3, 3)

                '対応コードにフォーカス
                Me.txtDealCd.ppTextBox.Focus()
            End If
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 対応明細削除ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDetailDelete_Click(sender As Object, e As EventArgs) Handles btnDetailDelete.Click

        '開始ログ出力
        psLogStart(Me)

        '明細削除処理
        If mfUpdateDataDetail(3) Then

            'データ取得（対応明細）
            msGetDataDetail()

            '編集項目クリア
            msClearDetail()

            '対応コード（編集エリア）のデフォルト表示（"GR"+保守管理番号の下3桁）
            Me.txtDealCd.ppText = "GR" + lblMntNo.Text.Substring(lblMntNo.Text.Length - 3, 3)

            '対応コードにフォーカス
            Me.txtDealCd.ppTextBox.Focus()
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        For Each rowData As GridViewRow In grvList.Rows
            '保守対応依頼書からの遷移で参照モードの場合、選択ボタンを非活性にする
            If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照 Then
                '--------------------------------
                '2014/06/20 武　ここから
                '--------------------------------
                rowData.Cells(0).Enabled = True
                '--------------------------------
                '2014/06/20 武　ここまで
                '--------------------------------
            Else
                ''トラブル明細のデータは、選択ボタンを非活性にする
                'If CType(rowData.FindControl("データ区分"), TextBox).Text = "2" Then
                '    rowData.Cells(0).Enabled = False
                'End If
            End If

            If (rowData.RowIndex Mod 2) = 1 Then
                CType(rowData.FindControl("適応区分／対応コード"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("対応日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("対応者名称"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("対応内容"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
            End If
        Next

    End Sub

    ''' <summary>
    ''' ホールマスタ管理/対応履歴照会ボタンクリック時処理 CMPSELP001-003
    ''' </summary>
    Private Sub btnHallMstMng_Click(sender As Object, e As EventArgs)
        Try
            Dim strKeyList As List(Of String) = Nothing '画面引継ぎ用キー情報
            Dim strDispPath As String = String.Empty
            objStack = New StackFrame

            '開始ログ出力
            psLogStart(Me)

            '画面引継ぎ用キー情報設定
            strKeyList = New List(Of String)
            strKeyList.Add(Me.txtTBoxid.ppText)                 'ＴＢＯＸＩＤ
            strKeyList.Add(Me.lblNLCls.Text.Replace("ＮＧＣ", "N").Replace("ＬＥＣ", "L")) 'NL
            strKeyList.Add(ViewState("SYSTEM_CLS").ToString)    'システム分類
            strKeyList.Add(ViewState("HALL_CD").ToString)       'ホールコード

            'セッション情報設定																	
            Session(P_SESSION_BCLIST) = Master.ppBcList_Text
            Session(P_SESSION_OLDDISP) = M_MY_DISP_ID
            Session(P_KEY) = strKeyList.ToArray
            Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)
            Session(P_SESSION_TERMS) = ViewState(P_SESSION_TERMS)

            '遷移先設定
            Select Case DirectCast(sender, Button).ID
                Case Master.ppRigthButton8.ID
                    'ホールマスタ管理
                    strDispPath = M_HMST_DISP_PATH
                Case Master.ppRigthButton7.ID
                    '対応履歴照会
                    strDispPath = M_CHST_DISP_PATH
            End Select

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
                            objStack.GetMethod.Name, strDispPath, strPrm, "TRANS")
            '■□■□結合試験時のみ使用予定□■□■

            '画面起動
            psOpen_Window(Me, strDispPath)

            '終了ログ出力
            psLogEnd(Me)

        Catch ex As Exception
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
    End Sub

    ''' <summary>
    ''' グリッドの選択ボタン押下時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        Dim rowData As GridViewRow = Nothing        'ボタン押下行

        If e.CommandName <> "btnSelect" Then
            Exit Sub
        End If

        '開始ログ出力
        psLogStart(Me)

        'ボタン押下行の情報を取得
        rowData = grvList.Rows(Convert.ToInt32(e.CommandArgument))

        '保守対応明細データを編集エリアに表示
        '--保守管理連番
        Me.lblDealMntSeq.Text = CType(rowData.FindControl("保守管理連番"), TextBox).Text
        Me.lblRecKbn.Text = CType(rowData.FindControl("データ区分"), TextBox).Text
        '--適応区分
        If CType(rowData.FindControl("適応区分"), TextBox).Text = "1" Then
            Me.cbxDealAdptCls.Checked = True
        Else
            Me.cbxDealAdptCls.Checked = False
        End If
        '--対応コード
        Me.txtDealCd.ppText = CType(rowData.FindControl("対応コード"), TextBox).Text
        '--対応日時：日
        Me.dtbDealDt.ppText = CType(rowData.FindControl("対応日時：日"), TextBox).Text
        '--対応日時：時
        Me.dtbDealDt.ppHourText = CType(rowData.FindControl("対応日時：時"), TextBox).Text
        '--対応日時：分
        Me.dtbDealDt.ppMinText = CType(rowData.FindControl("対応日時：分"), TextBox).Text
        '--対応者名称
        Me.txtDealUsr.ppText = CType(rowData.FindControl("対応者名称"), TextBox).Text
        '--対応内容
        Me.txaDealDtl.Text = CType(rowData.FindControl("対応内容"), TextBox).Text

        'ボタンを活性、非活性
        If ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.参照 Then
            Me.btnDetailInsert.Enabled = False
            Me.btnDetailUpdate.Enabled = True
            Me.btnDetailDelete.Enabled = True
        Else
            '-----------------------------
            '2014/06/24 武　ここから
            '-----------------------------
            Me.btnDetailClear.Enabled = True
            '-----------------------------
            '2014/06/24 武　ここまで
            '-----------------------------
            Me.btnDetailInsert.Enabled = False
            Me.btnDetailUpdate.Enabled = False
            Me.btnDetailDelete.Enabled = False
        End If
        '対応コードにフォーカス
        Me.txtDealCd.ppTextBox.Focus()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 輸送元区分変更時処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ddlTransbfCls_SelectedIndexChanged()

        '輸送元コード、輸送元名クリア
        Me.txtTransbfCd.ppText = String.Empty
        Me.lblTransbfNm.Text = String.Empty
        Me.txtTransbfCd.ppTextBox.Focus()

    End Sub

    ''' <summary>
    ''' 輸送先区分変更時処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ddlTransafCls_SelectedIndexChanged()

        '輸送先コード、輸送先名クリア
        Me.txtTransafCd.ppText = String.Empty
        Me.lblTransafNm.Text = String.Empty
        Me.txtTransafCd.ppTextBox.Focus()

    End Sub

    ''' <summary>
    ''' 輸送元コード変更時処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub txtTransbfCd_TextChanged()

        Dim strbuff As String = String.Empty    '輸送元名取得用

        If Me.txtTransbfCd.ppText <> String.Empty Then
            Select Case Me.ddlTransbfCls.ppSelectedValue
                Case 0  'ホール名取得
                    If mfGetHallInfo(Me.txtTransbfCd.ppText, strbuff) Then
                        Me.lblTransbfNm.Text = strbuff
                        Me.ddlTransafCls.ppDropDownList.Focus()
                    Else
                        Me.lblTransbfNm.Text = String.Empty
                        Me.txtTransbfCd.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
                        Me.txtTransbfCd.ppTextBox.Focus()
                    End If
                Case 1  '営業所名取得
                    If mfGetOfficeInfo("2", Me.txtTransbfCd.ppText, strbuff) Then
                        Me.lblTransbfNm.Text = strbuff
                        Me.ddlTransafCls.ppDropDownList.Focus()
                    Else
                        Me.lblTransbfNm.Text = String.Empty
                        Me.txtTransbfCd.psSet_ErrorNo("2002", "入力した営業所コード")
                        Me.txtTransbfCd.ppTextBox.Focus()
                    End If
                Case 2  '代理店名取得
                    If mfGetOfficeInfo("4", Me.txtTransbfCd.ppText, strbuff) Then
                        Me.lblTransbfNm.Text = strbuff
                        Me.ddlTransafCls.ppDropDownList.Focus()
                    Else
                        Me.lblTransbfNm.Text = String.Empty
                        Me.txtTransbfCd.psSet_ErrorNo("2002", "入力した代理店コード")
                        Me.txtTransbfCd.ppTextBox.Focus()
                    End If
                Case 3  '保守拠点名取得 CMPSELP001-001
                    If mfGetOfficeInfo("3", Me.txtTransbfCd.ppText, strbuff) Then
                        Me.lblTransbfNm.Text = strbuff
                        Me.ddlTransafCls.ppDropDownList.Focus()
                    Else
                        Me.lblTransbfNm.Text = String.Empty
                        Me.txtTransbfCd.psSet_ErrorNo("2002", "入力した保守拠点コード")
                        Me.txtTransbfCd.ppTextBox.Focus()
                    End If
                Case 4  'NGC取得 CMPSELP001-001
                    If mfGetOfficeInfo("1", Me.txtTransbfCd.ppText, strbuff) Then
                        Me.lblTransbfNm.Text = strbuff
                        Me.ddlTransafCls.ppDropDownList.Focus()
                    Else
                        Me.lblTransbfNm.Text = String.Empty
                        Me.txtTransbfCd.psSet_ErrorNo("2002", "入力したNGCコード")
                        Me.txtTransbfCd.ppTextBox.Focus()
                    End If
                Case Else
            End Select
        End If

    End Sub

    ''' <summary>
    ''' 輸送先コード変更時処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub txtTransafCd_TextChanged()

        Dim strbuff As String = String.Empty    '輸送元名取得用

        If Me.txtTransafCd.ppText <> String.Empty Then
            Select Case Me.ddlTransafCls.ppSelectedValue
                Case 0  'ホール名取得
                    If mfGetHallInfo(Me.txtTransafCd.ppText, strbuff) Then
                        Me.lblTransafNm.Text = strbuff
                        Me.txtTransItem.ppTextBox.Focus()
                    Else
                        Me.lblTransafNm.Text = String.Empty
                        Me.txtTransafCd.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
                        Me.txtTransafCd.ppTextBox.Focus()
                    End If
                Case 1  '営業所名取得
                    If mfGetOfficeInfo("2", Me.txtTransafCd.ppText, strbuff) Then
                        Me.lblTransafNm.Text = strbuff
                        Me.txtTransItem.ppTextBox.Focus()
                    Else
                        Me.lblTransafNm.Text = String.Empty
                        Me.txtTransafCd.psSet_ErrorNo("2002", "入力した営業所コード")
                        Me.txtTransafCd.ppTextBox.Focus()
                    End If
                Case 2  '代理店名取得
                    If mfGetOfficeInfo("4", Me.txtTransafCd.ppText, strbuff) Then
                        Me.lblTransafNm.Text = strbuff
                        Me.txtTransItem.ppTextBox.Focus()
                    Else
                        Me.lblTransafNm.Text = String.Empty
                        Me.txtTransafCd.psSet_ErrorNo("2002", "入力した代理店コード")
                        Me.txtTransafCd.ppTextBox.Focus()
                    End If
                Case 3  '保守拠点名取得 CMPSELP001-001
                    If mfGetOfficeInfo("3", Me.txtTransafCd.ppText, strbuff) Then
                        Me.lblTransafNm.Text = strbuff
                        Me.txtTransItem.ppTextBox.Focus()
                    Else
                        Me.lblTransafNm.Text = String.Empty
                        Me.txtTransafCd.psSet_ErrorNo("2002", "入力した保守拠点コード")
                        Me.txtTransafCd.ppTextBox.Focus()
                    End If
                Case 4  'NGC取得 CMPSELP001-001
                    If mfGetOfficeInfo("1", Me.txtTransafCd.ppText, strbuff) Then
                        Me.lblTransafNm.Text = strbuff
                        Me.txtTransItem.ppTextBox.Focus()
                    Else
                        Me.lblTransafNm.Text = String.Empty
                        Me.txtTransafCd.psSet_ErrorNo("2002", "入力したNGCコード")
                        Me.txtTransafCd.ppTextBox.Focus()
                    End If
                Case Else
            End Select
        End If

    End Sub

    ''' <summary>
    ''' 依頼者変更時
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub txtReqUsr_TextChanged()

        If Me.txtReqUsr.ppText = String.Empty Then
            intEmpFlg = 1
        Else
            intEmpFlg = 0
            If Me.dtbReqDt.ppText = String.Empty And _
                Me.dtbReqDt.ppHourText = String.Empty And _
                Me.dtbReqDt.ppMinText = String.Empty Then
                Me.dtbReqDt.ppText = DateTime.Today.ToString("yyyy/MM/dd")       '依頼日時（日付）
                Me.dtbReqDt.ppHourText = DateTime.Now.ToString("HH")             '依頼日時（時間：時）
                Me.dtbReqDt.ppMinText = DateTime.Now.ToString("mm")              '依頼日時（時間：分）
            End If
        End If

    End Sub

#End Region

#Region "そのほかのプロシージャ"
    ''' <summary>
    ''' 画面初期化処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msInitScreen()

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            '発生区分
            Me.ddlOccurCls.SelectedIndex = 0
            'ＴＢＯＸＩＤ
            Me.txtTBoxid.ppText = String.Empty

            'パネル１（管理番号～トラブル管理番号）
            Me.lblMntNo.Text = String.Empty         '管理番号
            Me.lblSpecialMnt.Text = String.Empty    '特別保守
            Me.lblNLCls.Text = String.Empty         'ＮＬ区分
            Me.lblTboxType.Text = String.Empty      'ＴＢＯＸタイプ
            Me.lblTboxVer.Text = String.Empty       'ＶＥＲ
            Me.lblHallNm.Text = String.Empty        'ホール名
            Me.lblEWCls.Text = String.Empty         'ＥＷ区分
            Me.lblAddr1.Text = String.Empty         '住所（上段）
            Me.lblBranchNm.Text = String.Empty      '保担名
            Me.lblTelno.Text = String.Empty         'ＴＥＬ
            Me.lblUnfNm.Text = String.Empty         '統括保担名
            Me.dtbRptDt.ppText = String.Empty       '申告日
            Me.txtRptCharge.ppText = String.Empty   '申告者
            Me.ddlRptBase.SelectedIndex = 0         '申告元
            Me.dtbRcptDt.ppText = String.Empty      '受付日時（日付）
            Me.dtbRcptDt.ppHourText = String.Empty  '受付日時（時間：時）
            Me.dtbRcptDt.ppMinText = String.Empty   '受付日時（時間：時）
            Me.txtRcptCharge.ppText = String.Empty  '受付者
            Me.lblRptTel.Text = String.Empty        '申告元ＴＥＬ
            Me.dtbReqDt.ppText = String.Empty       '依頼日時（日付）
            Me.dtbReqDt.ppHourText = String.Empty   '依頼日時（時間：時）
            Me.dtbReqDt.ppMinText = String.Empty    '依頼日時（時間：分）
            Me.txtReqUsr.ppText = String.Empty      '依頼者
            Me.lblTrbNo.Text = String.Empty         'トラブル管理番号

            'パネル２（ホール担当者～連絡事項（下段））
            Me.txtHallCharge.ppText = String.Empty  'ホール担当者
            Me.dtbLastDt.ppText = String.Empty      '最終集信日
            Me.txtInfo1.ppText = String.Empty       '連絡事項（上段）
            Me.txtInfo2.ppText = String.Empty       '連絡事項（下段）

            'パネル３（申告内容（マスタ）～備考・連絡２）
            Me.ddlRpt.SelectedIndex = 0             '申告内容（マスタ）
            Me.txtRptDtl1.ppText = String.Empty     '申告内容１
            Me.txtRptDtl2.ppText = String.Empty     '申告内容２
            Me.cbxImpCls.Checked = False            '故障重要度
            Me.txtInhCntnt1.ppText = String.Empty   '引継内容１
            Me.txtInhCntnt2.ppText = String.Empty   '引継内容２
            Me.cbxBsnsdistCls.Checked = False       '引継区分（営業支障）
            Me.cbxScnddistCls.Checked = False       '引継区分（２次支障）
            Me.dtbWrkDt.ppText = String.Empty       '作業予定日時（日付）
            Me.dtbWrkDt.ppHourText = String.Empty   '作業予定日時（時間：時）
            Me.dtbWrkDt.ppMinText = String.Empty    '作業予定日時（時間：分）
            Me.txtWrkUser.ppText = String.Empty     '作業者
            Me.ddlCmp.ppDropDownList.SelectedIndex = 0 '会社名
            Me.dtbDeptDt.ppText = String.Empty      '出発日時（日付）
            Me.dtbDeptDt.ppHourText = String.Empty  '出発日時（時間：時）
            Me.dtbDeptDt.ppMinText = String.Empty   '出発日時（時間：分）
            Me.dtbStartDt.ppText = String.Empty     '開始日時（日付）
            Me.dtbStartDt.ppHourText = String.Empty '開始日時（時間：時）
            Me.dtbStartDt.ppMinText = String.Empty  '開始日時（時間：分）
            Me.dtbEndDt.ppText = String.Empty       '終了日時（日付）
            Me.dtbEndDt.ppHourText = String.Empty   '終了日時（時間：時）
            Me.dtbEndDt.ppMinText = String.Empty    '終了日時（時間：分）
            Me.ddlStatus.SelectedIndex = 0          '作業状況（ステータス）
            Me.txtSttsNotetext.ppText = String.Empty    '作業状況備考
            Me.txtSttsNotetext.ppEnabled = False        '作業状況備考を非活性
            Me.ddlAppa.ppDropDownList.SelectedIndex = 0 '故障機器
            Me.ddlRepair.SelectedIndex = 0              '回復内容（マスタ）
            Me.txtRepair.ppText = String.Empty          '回復内容（入力）
            Me.txtNotetext1.ppText = String.Empty       '備考・連絡１
            Me.txtNotetext2.ppText = String.Empty       '備考・連絡２

            '保守対応明細項目クリア（パネル４）
            msClearDetail()

            '保守対応明細グリッド
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

            'パネル５（特別保守フラグ～処置内容（入力））
            Me.ddlMntFlg.SelectedIndex = 0   '特別保守フラグ
            Me.lblInsApp.Text = String.Empty        '検収承認
            Me.lblReqApp.Text = String.Empty        '請求承認
            Me.tmbDeptTm.ppHourText = String.Empty  '出発時間（時）
            Me.tmbDeptTm.ppMinText = String.Empty   '出発時間（分）
            Me.tmbStartTm.ppHourText = String.Empty '開始時間（時）
            Me.tmbStartTm.ppMinText = String.Empty  '開始時間（分）
            Me.tmbEndTm.ppHourText = String.Empty   '終了時間（時）
            Me.tmbEndTm.ppMinText = String.Empty    '終了時間（分）
            If Me.ddlWrk.Items.Count > 0 Then
                Me.ddlWrk.SelectedIndex = 0             '作業内容（マスタ）
            End If
            Me.txtMntTm.ppText = String.Empty       '特別保守作業時間
            Me.txtGbTm.ppText = String.Empty        '特別保守往復時間
            Me.txtPsnNum.ppText = String.Empty      '作業人数
            Me.txtReqPrice.ppText = String.Empty    '請求金額
            Me.dtbSubmitDt.ppText = String.Empty    '提出日
            If Me.ddlRepairCls.ppDropDownList.Items.Count > 0 Then
                Me.ddlRepairCls.ppDropDownList.SelectedIndex = 0    'メーカ修理
            End If
            Me.txtNotetext.ppText = String.Empty    '備考
            If Me.ddlDeal.Items.Count > 0 Then
                Me.ddlDeal.SelectedIndex = 0            '処置内容（マスタ）
            End If
            Me.txtDeal.ppText = String.Empty        '処置内容（入力）

            'パネル６（輸送日～輸送金額）
            Me.dtbTransDt.ppText = String.Empty     '輸送日
            If Me.ddlTransbfCls.ppDropDownList.Items.Count > 0 Then
                Me.ddlTransbfCls.ppDropDownList.SelectedIndex = 0   '輸送元場所区分
            End If
            Me.txtTransbfCd.ppText = String.Empty   '輸送元コード
            Me.lblTransbfNm.Text = String.Empty     '輸送元名
            If Me.ddlTransafCls.ppDropDownList.Items.Count > 0 Then
                Me.ddlTransafCls.ppDropDownList.SelectedIndex = 0   '輸送先場所区分
            End If
            Me.txtTransafCd.ppText = String.Empty   '輸送先コード
            Me.lblTransafNm.Text = String.Empty     '輸送先名
            Me.txtTransItem.ppText = String.Empty   '輸送物品
            If Me.ddlTransRsn.ppDropDownList.Items.Count > 0 Then
                Me.ddlTransRsn.ppDropDownList.SelectedIndex = 0 '輸送理由
            End If
            Me.txtTransComp.ppText = String.Empty   '輸送会社
            If Me.ddlTransCls.ppDropDownList.Items.Count > 0 Then
                Me.ddlTransCls.ppDropDownList.SelectedIndex = 0 '輸送区分
            End If
            Me.txtTransPrice.ppText = String.Empty  '輸送金額

        Catch ex As Exception
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
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
    ''' TBOX情報の初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msInitTBoxData()
        lblNLCls.Text = "" 'ＮＬ区分
        lblEWCls.Text = "" 'ＥＷ区分
        lblHallNm.Text = "" 'ホール名
        lblAddr1.Text = "" 'ホール住所
        lblTelno.Text = "" 'ホールTEL
        lblUnfNm.Text = "" '統括名
        lblBranchNm.Text = "" '保担名
        lblTboxVer.Text = "" 'Ｖｅｒ
        lblTboxType.Text = "" 'ＴＢＯＸタイプ
        lblTwin.Text = ""
        hdnTwinCd.Value = ""
        lblAgc.Text = ""
        hdnAgcCd.Value = ""
        hdnAgcZip.Value = ""
        hdnAgcAddr.Value = ""
        hdnAgcTel.Value = ""
        hdnAgcFax.Value = ""
        lblRep.Text = ""
        hdnRepCd.Value = ""
        hdnRepZip.Value = ""
        hdnRepAddr.Value = ""
        hdnRepTel.Value = ""
        hdnRepChg.Value = ""
        lblNgcOrg.Text = ""
        lblOrgTel.Text = ""
        lblEst.Text = ""
        hdnEstCls.Value = ""
        lblMdn.Text = ""
        hdnMdnCnt.Value = ""
        hdnMdnCd1.Value = ""
        hdnMdnCd2.Value = ""
        hdnMdnCd3.Value = ""
    End Sub

    ''' <summary>
    ''' 保守対応明細編集項目のクリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearDetail()

        Me.lblDealMntSeq.Text = String.Empty    '保守管理連番
        Me.cbxDealAdptCls.Checked = False       '適応区分
        Me.txtDealCd.ppText = String.Empty      '対応コード
        Me.dtbDealDt.ppText = String.Empty      '対応日時（日付）
        Me.dtbDealDt.ppHourText = String.Empty  '対応日時（時間：時）
        Me.dtbDealDt.ppMinText = String.Empty   '対応日時（時間：分）
        Me.txtDealUsr.ppText = String.Empty     '対応担当者
        Me.txaDealDtl.Text = String.Empty     '対応内容
        Me.btnDetailInsert.Enabled = True       '追加ボタン
        Me.btnDetailUpdate.Enabled = False      '更新ボタン
        Me.btnDetailDelete.Enabled = False      '削除ボタン
        '-----------------------------
        '2014/06/24 武　ここから
        '-----------------------------
        '参照時は選択したときのみクリアできる。
        If ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.参照 Then
            '-----------------------------
            '2014/06/24 武　ここまで
            '-----------------------------
            Me.btnDetailClear.Enabled = True        'クリアボタン
            '-----------------------------
            '2014/06/24 武　ここから
            '-----------------------------
        Else
            Me.btnDetailClear.Enabled = False
        End If
        '-----------------------------
        '2014/06/24 武　ここまで
        '-----------------------------
    End Sub

    ''' <summary>
    ''' 各コントロールの活性・非活性
    ''' </summary>
    ''' <param name="intMode">モード（1:参照、2:更新、3:登録、4:削除）</param>
    ''' <remarks></remarks>
    Private Sub msSetActivecontrol(ByVal intMode As Integer)

        Select Case intMode
            Case 1  '参照
                Me.ddlOccurCls.Enabled = False  '発生区分
                Me.txtTBoxid.ppEnabled = False  'ＴＢＯＸＩＤ
                Me.btnEntry.Enabled = False     '登録ボタン
                Me.pnlMnt1.Enabled = False  'パネル１
                Me.pnlMnt2.Enabled = False  'パネル２
                Me.pnlMnt3.Enabled = False  'パネル３
                '---------------------------
                '2014/06/24 武 ここから
                '---------------------------
                'Me.pnlMnt4.Enabled = False  'パネル４
                '---------------------------
                '2014/06/24 武 ここから
                '---------------------------
                Me.pnlMnt5.Enabled = False  'パネル５
                Me.pnlMnt6.Enabled = False  'パネル６
                '---------------------------
                '2014/06/24 武 ここから
                '---------------------------
                Me.cbxDealAdptCls.Enabled = False
                Me.txtDealCd.ppEnabled = False
                Me.dtbDealDt.ppEnabled = False
                Me.txtDealUsr.ppEnabled = False
                Me.txaDealDtl.Enabled = False
                Me.btnDetailInsert.Enabled = False
                Me.btnDetailUpdate.Enabled = False
                Me.btnDetailDelete.Enabled = False
                Me.btnDetailClear.Enabled = False
                '---------------------------
                '2014/06/24 武 ここから
                '---------------------------
                Master.ppLeftButton1.Enabled = False    '更新ボタン
                Master.ppLeftButton2.Enabled = False    '削除ボタン
                Master.ppLeftButton3.Enabled = True     '依頼書印刷ボタン
                Master.ppLeftButton4.Enabled = True     '履歴印刷ボタン
                Master.ppRigthButton1.Enabled = True    '修理依頼書ボタン
                Master.ppRigthButton2.Enabled = True    '持参物品一覧ボタン
                'トラブル処理票ボタン ※トラブル番号が入っている場合は活性化
                'If Me.lblTrbNo.Text <> String.Empty AndAlso
                '   ViewState(P_SESSION_OLDDISP) <> M_TRBL_DISP_ID Then
                If Me.lblTrbNo.Text <> String.Empty Then
                    Master.ppRigthButton3.Enabled = True
                Else
                    Master.ppRigthButton3.Enabled = False
                End If
                Master.ppRigthButton4.Enabled = True
                'CMPSELP001-003
                Master.ppRigthButton8.Enabled = True   'ホールマスタ管理
                Master.ppRigthButton7.Enabled = True   '対応履歴照会
                'CMPSELP001-003 END
            Case 2  '更新
                Me.ddlOccurCls.Enabled = False  '発生区分
                Me.txtTBoxid.ppEnabled = False  'ＴＢＯＸＩＤ
                Me.btnEntry.Enabled = False     '登録ボタン
                Me.pnlMnt1.Enabled = True   'パネル１
                Me.pnlMnt2.Enabled = True   'パネル２
                Me.pnlMnt3.Enabled = True   'パネル３
                Me.pnlMnt4.Enabled = True   'パネル４
                Me.pnlMnt5.Enabled = True   'パネル５
                Me.pnlMnt6.Enabled = True   'パネル６
                Master.ppLeftButton1.Enabled = True     '更新ボタン
                Master.ppLeftButton2.Enabled = False     '削除ボタン
                Master.ppLeftButton3.Enabled = True     '依頼書印刷ボタン
                Master.ppLeftButton4.Enabled = True     '履歴印刷ボタン
                Master.ppRigthButton1.Enabled = True    '修理依頼書ボタン
                Master.ppRigthButton2.Enabled = True    '持参物品一覧ボタン
                'トラブル処理票ボタン ※トラブル番号が入っている場合は活性化
                'If Me.lblTrbNo.Text <> String.Empty AndAlso
                '   ViewState(P_SESSION_OLDDISP) <> M_TRBL_DISP_ID Then
                If Me.lblTrbNo.Text <> String.Empty Then
                    Master.ppRigthButton3.Enabled = True
                Else
                    Master.ppRigthButton3.Enabled = False
                End If
                Master.ppRigthButton4.Enabled = True
                'CMPSELP001-003
                Master.ppRigthButton8.Enabled = True   'ホールマスタ管理
                Master.ppRigthButton7.Enabled = True   '対応履歴照会
                'CMPSELP001-003 END
            Case 3  '登録
                Me.ddlOccurCls.Enabled = True   '発生区分
                'ＴＢＯＸＩＤ
                If ViewState(P_SESSION_OLDDISP) = M_MNT_DISP_ID Then
                    Me.txtTBoxid.ppEnabled = True
                Else
                    Me.txtTBoxid.ppEnabled = False
                End If
                Me.btnEntry.Enabled = True  '登録ボタン
                Me.pnlMnt1.Enabled = False  'パネル１
                Me.pnlMnt2.Enabled = False  'パネル２
                Me.pnlMnt3.Enabled = False  'パネル３
                Me.pnlMnt4.Enabled = False  'パネル４
                Me.pnlMnt5.Enabled = False  'パネル５
                Me.pnlMnt6.Enabled = False  'パネル６
                Master.ppLeftButton1.Enabled = False    '更新ボタン
                Master.ppLeftButton2.Enabled = False    '削除ボタン
                Master.ppLeftButton3.Enabled = False    '依頼書印刷ボタン
                Master.ppLeftButton4.Enabled = False    '履歴印刷ボタン
                Master.ppRigthButton1.Enabled = False   '修理依頼書ボタン
                Master.ppRigthButton2.Enabled = False   '持参物品一覧ボタン
                Master.ppRigthButton3.Enabled = False   'トラブル処理票ボタン
                Master.ppRigthButton4.Enabled = False   'トラブル処理票ボタン
                'CMPSELP001-003
                Master.ppRigthButton8.Enabled = False   'ホールマスタ管理
                Master.ppRigthButton7.Enabled = False   '対応履歴照会
                'CMPSELP001-003 END
            Case 4  '削除
                Me.ddlOccurCls.Enabled = False  '発生区分
                Me.txtTBoxid.ppEnabled = False  'ＴＢＯＸＩＤ
                Me.btnEntry.Enabled = False     '登録ボタン
                Me.pnlMnt1.Enabled = False  'パネル１
                Me.pnlMnt2.Enabled = False  'パネル２
                Me.pnlMnt3.Enabled = False  'パネル３
                Me.pnlMnt4.Enabled = False  'パネル４
                Me.pnlMnt5.Enabled = False  'パネル５
                Me.pnlMnt6.Enabled = False  'パネル６
                Master.ppLeftButton1.Enabled = False    '更新ボタン
                Master.ppLeftButton2.Enabled = False    '削除ボタン
                Master.ppLeftButton3.Enabled = False    '依頼書印刷ボタン
                Master.ppLeftButton4.Enabled = False    '履歴印刷ボタン
                Master.ppRigthButton1.Enabled = False   '修理依頼書ボタン
                Master.ppRigthButton2.Enabled = False   '持参物品一覧ボタン
                Master.ppRigthButton3.Enabled = False   'トラブル処理票ボタン
                Master.ppRigthButton4.Enabled = False   'トラブル処理票ボタン
                'CMPSELP001-003
                Master.ppRigthButton8.Enabled = False   'ホールマスタ管理
                Master.ppRigthButton7.Enabled = False   '対応履歴照会
                'CMPSELP001-003 END
            Case Else

        End Select

    End Sub

    ''' <summary>
    ''' データ取得
    ''' </summary>
    ''' <param name="strMntNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetData(ByVal strMntNo As String) As Boolean

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

        mfGetData = False

        'グリッドの初期化
        Me.grvList.DataSource = New DataTable
        Me.grvList.DataBind()

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        Else
            Try
                '--保守対応データの取得
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S1", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '保守管理番号
                    .Add(pfSet_Param("mntno", SqlDbType.NVarChar, strMntNo))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '取得したデータを表示（テキストボックスに設定）
                If objDs Is Nothing Then
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "保守対応依頼書")
                    Exit Function
                Else
                    dtRow = objDs.Tables(0).Rows(0)

                    msSetddlRptcntt(dtRow("システムコード").ToString)   '申告内容マスタ
                    msSetddlRprdtl(dtRow("システムコード").ToString)    '回復内容マスタ
                    msSetddlDealcntt(dtRow("システムコード").ToString)  '処置内容マスタ

                    'CMPSELP001-003
                    ViewState("SYSTEM_CLS") = dtRow("システムクラス").ToString
                    ViewState("HALL_CD") = dtRow("ホールコード").ToString
                    'CMPSELP001-003 END

                    If dtRow("システムクラス").ToString = "5" _
                    OrElse dtRow("システムクラス").ToString = "3" Then
                        If dtRow("システムコード").ToString = "31" Then
                            Master.ppRigthButton4.Visible = False
                        Else
                            Master.ppRigthButton4.Visible = True
                        End If
                    Else
                        Master.ppRigthButton4.Visible = False
                    End If

                    '発生区分、ＴＢＯＸＩＤ
                    Me.ddlOccurCls.SelectedValue = dtRow("保守管理区分").ToString
                    Me.txtTBoxid.ppText = dtRow("ＴＢＯＸＩＤ").ToString

                    'パネル１（管理番号～トラブル管理番号）
                    Me.lblMntNo.Text = dtRow("管理番号").ToString
                    Me.lblSpecialMnt.Text = dtRow("特別保守").ToString
                    Me.lblNLCls.Text = dtRow("ＮＬ区分").ToString
                    Me.lblTboxType.Text = dtRow("ＴＢＯＸタイプ").ToString
                    Me.lblTboxVer.Text = dtRow("ＶＥＲ").ToString
                    Me.lblHallNm.Text = dtRow("ホール名").ToString
                    Me.lblEWCls.Text = dtRow("ＥＷ区分").ToString
                    Me.lblAddr1.Text = dtRow("住所１").ToString
                    Me.lblBranchNm.Text = dtRow("保担名").ToString
                    Me.lblTelno.Text = dtRow("ＴＥＬ").ToString
                    Me.lblUnfNm.Text = dtRow("統括保担名").ToString
                    Me.dtbRptDt.ppText = dtRow("申告日").ToString
                    Me.txtRptCharge.ppText = dtRow("申告者").ToString
                    Me.ddlRptBase.SelectedValue = dtRow("申告元コード").ToString
                    Me.dtbRcptDt.ppText = dtRow("受付日時：日").ToString
                    Me.dtbRcptDt.ppHourText = dtRow("受付日時：時").ToString
                    Me.dtbRcptDt.ppMinText = dtRow("受付日時：分").ToString
                    Me.txtRcptCharge.ppText = dtRow("受付者").ToString
                    Me.lblRptTel.Text = dtRow("申告元ＴＥＬ").ToString
                    Me.dtbReqDt.ppText = dtRow("依頼日時：日").ToString
                    Me.dtbReqDt.ppHourText = dtRow("依頼時間：時").ToString
                    Me.dtbReqDt.ppMinText = dtRow("依頼時間：分").ToString
                    Me.txtReqUsr.ppText = dtRow("依頼者名称").ToString
                    Me.lblTrbNo.Text = dtRow("トラブル管理番号").ToString

                    'パネル２（ホール担当者～連絡事項（下段））
                    Me.txtHallCharge.ppText = dtRow("ホール担当者").ToString
                    Me.dtbLastDt.ppText = dtRow("最終集信日").ToString
                    Me.txtInfo1.ppText = dtRow("連絡事項１").ToString
                    Me.txtInfo2.ppText = dtRow("連絡事項２").ToString

                    'パネル３（申告内容（マスタ）～備考・連絡２）
                    Me.ddlRpt.SelectedValue = dtRow("申告内容コード").ToString
                    Me.txtRptDtl1.ppText = dtRow("申告内容１").ToString
                    Me.txtRptDtl2.ppText = dtRow("申告内容２").ToString
                    If dtRow("故障重要度区分").ToString = "1" Then
                        Me.cbxImpCls.Checked = True
                    Else
                        Me.cbxImpCls.Checked = False
                    End If
                    Me.txtInhCntnt1.ppText = dtRow("引継内容１").ToString
                    Me.txtInhCntnt2.ppText = dtRow("引継内容２").ToString
                    If dtRow("営業支障区分").ToString = "1" Then
                        Me.cbxBsnsdistCls.Checked = True
                    Else
                        Me.cbxBsnsdistCls.Checked = False
                    End If
                    If dtRow("２次支障区分").ToString = "1" Then
                        Me.cbxScnddistCls.Checked = True
                    Else
                        Me.cbxScnddistCls.Checked = False
                    End If
                    Me.dtbWrkDt.ppText = dtRow("作業予定日時：日").ToString
                    Me.dtbWrkDt.ppHourText = dtRow("作業予定日時：時").ToString
                    Me.dtbWrkDt.ppMinText = dtRow("作業予定日時：分").ToString
                    Me.txtWrkUser.ppText = dtRow("作業者").ToString
                    Me.txtWrkCode.ppText = dtRow("作業者コード").ToString
                    Me.txtWrkTel.ppText = dtRow("作業者ＴＥＬ").ToString
                    Me.ddlCmp.ppSelectedValue = dtRow("会社区分").ToString
                    Me.dtbDeptDt.ppText = dtRow("出発日時：日").ToString
                    Me.dtbDeptDt.ppHourText = dtRow("出発日時：時").ToString
                    Me.dtbDeptDt.ppMinText = dtRow("出発日時：分").ToString
                    Me.dtbDeptDt.ppText = dtRow("出発日時：日").ToString
                    Me.dtbDeptDt.ppHourText = dtRow("出発日時：時").ToString
                    Me.dtbDeptDt.ppMinText = dtRow("出発日時：分").ToString
                    Me.dtbStartDt.ppText = dtRow("開始日時：日").ToString
                    Me.dtbStartDt.ppHourText = dtRow("開始日時：時").ToString
                    Me.dtbStartDt.ppMinText = dtRow("開始日時：分").ToString
                    Me.dtbEndDt.ppText = dtRow("終了日時：日").ToString
                    Me.dtbEndDt.ppHourText = dtRow("終了日時：時").ToString
                    Me.dtbEndDt.ppMinText = dtRow("終了日時：分").ToString
                    Me.ddlStatus.SelectedValue = dtRow("作業状況コード").ToString
                    If dtRow("作業状況コード").ToString = M_WORK_STSCD Then
                        Me.txtSttsNotetext.ppText = dtRow("作業状況備考").ToString
                        Me.txtSttsNotetext.ppEnabled = True
                    Else
                        Me.txtSttsNotetext.ppText = String.Empty
                        Me.txtSttsNotetext.ppEnabled = False
                    End If
                    Me.ddlAppa.ppSelectedValue = dtRow("故障機器コード").ToString
                    Me.ddlRepair.SelectedValue = dtRow("回復内容コード").ToString
                    Me.txtRepair.ppText = dtRow("回復内容").ToString
                    Me.txtNotetext1.ppText = dtRow("備考・連絡１").ToString
                    Me.txtNotetext2.ppText = dtRow("備考・連絡２").ToString

                    'パネル５（特別保守フラグ～処置内容（入力））
                    Me.ddlMntFlg.SelectedValue = dtRow("特別保守フラグ").ToString
                    ViewState("MNT_FLG") = dtRow("特別保守フラグ").ToString
                    Me.lblInsApp.Text = dtRow("検収承認").ToString
                    Me.lblReqApp.Text = dtRow("請求承認").ToString
                    ViewState("APP_UPD_CLS") = (Integer.Parse(dtRow("依頼区分").ToString) + Integer.Parse(dtRow("検収区分").ToString)).ToString
                    Me.tmbDeptTm.ppHourText = dtRow("出発時間：時").ToString
                    Me.tmbDeptTm.ppMinText = dtRow("出発時間：分").ToString
                    Me.tmbStartTm.ppHourText = dtRow("開始時間：時").ToString
                    Me.tmbStartTm.ppMinText = dtRow("開始時間：分").ToString
                    Me.tmbEndTm.ppHourText = dtRow("終了時間：時").ToString
                    Me.tmbEndTm.ppMinText = dtRow("終了時間：分").ToString
                    '--------------------------------
                    '2014/05/21 後藤　ここから
                    '--------------------------------
                    'Me.ddlWrk.SelectedValue = dtRow("作業内容コード").ToString
                    '--------------------------------
                    '2014/05/21 後藤　ここまで
                    '--------------------------------
                    Me.txtMntTm.ppText = dtRow("特別保守作業時間").ToString
                    Me.txtGbTm.ppText = dtRow("特別保守往復時間").ToString
                    Me.txtPsnNum.ppText = dtRow("作業人数").ToString
                    Me.txtReqPrice.ppText = dtRow("請求金額").ToString
                    Me.dtbSubmitDt.ppText = dtRow("提出日").ToString
                    Me.ddlRepairCls.ppSelectedValue = dtRow("メーカ修理区分").ToString
                    Me.txtNotetext.ppText = dtRow("備考").ToString
                    Me.ddlDeal.SelectedValue = dtRow("処置内容コード").ToString
                    Me.txtDeal.ppText = dtRow("処置内容").ToString

                    'パネル６（輸送日～輸送金額）
                    Me.dtbTransDt.ppText = dtRow("輸送日").ToString
                    Me.ddlTransbfCls.ppSelectedValue = dtRow("輸送元区分").ToString
                    Me.txtTransbfCd.ppText = dtRow("輸送元コード").ToString
                    Me.lblTransbfNm.Text = dtRow("輸送元").ToString
                    Me.ddlTransafCls.ppSelectedValue = dtRow("輸送先区分").ToString
                    Me.txtTransafCd.ppText = dtRow("輸送先コード").ToString
                    Me.lblTransafNm.Text = dtRow("輸送先").ToString
                    Me.txtTransItem.ppText = dtRow("輸送物品").ToString
                    Me.ddlTransRsn.ppSelectedValue = dtRow("輸送理由コード").ToString
                    Me.txtTransComp.ppText = dtRow("輸送会社").ToString
                    Me.ddlTransCls.ppSelectedValue = dtRow("輸送区分").ToString
                    Me.txtTransPrice.ppText = dtRow("輸送金額").ToString

                    If dtRow("双子店区分").ToString = "0" Then
                        lblTwin.Text = "単独店"
                        hdnTwinCd.Value = "0"
                    ElseIf dtRow("双子店区分").ToString = "1" Then
                        lblTwin.Text = "双子店"
                        hdnTwinCd.Value = "1"
                    Else
                        lblTwin.Text = ""
                        hdnTwinCd.Value = ""
                    End If
                    lblAgc.Text = dtRow("代理店名").ToString
                    hdnAgcCd.Value = dtRow("代理店ＣＤ").ToString
                    hdnAgcZip.Value = dtRow("代理店郵便番号").ToString
                    hdnAgcAddr.Value = dtRow("代理店住所").ToString
                    hdnAgcTel.Value = dtRow("代理店ＴＥＬ").ToString
                    hdnAgcFax.Value = dtRow("代理店ＦＡＸ").ToString

                    lblRep.Text = dtRow("代行店名").ToString
                    hdnRepCd.Value = dtRow("代行店ＣＤ").ToString
                    hdnRepZip.Value = dtRow("代行店郵便番号").ToString
                    hdnRepAddr.Value = dtRow("代行店住所").ToString
                    hdnRepTel.Value = dtRow("代行店ＴＥＬ").ToString
                    hdnRepChg.Value = dtRow("代行店担当者").ToString
                    'CMPSELP001-004
                    If dtRow("代行店表示フラグ") = "1" Then
                        lblRep.Visible = False
                    Else
                        lblRep.Visible = True
                    End If
                    'CMPSELP001-004 END
                    lblNgcOrg.Text = dtRow("ＮＧＣ担当営業部").ToString
                    lblOrgTel.Text = dtRow("担当営業部ＴＥＬ").ToString
                    lblEst.Text = dtRow("ＭＤＮ設置有無").ToString
                    hdnEstCls.Value = dtRow("ＭＤＮ設置有無ＣＤ").ToString
                    lblMdn.Text = dtRow("ＭＤＮ機器名").ToString
                    hdnMdnCnt.Value = dtRow("ＭＤＮ台数").ToString
                    hdnMdnCd1.Value = dtRow("ＭＤＮＣＤ１").ToString
                    hdnMdnCd2.Value = dtRow("ＭＤＮＣＤ２").ToString
                    hdnMdnCd3.Value = dtRow("ＭＤＮＣＤ３").ToString

                    If dtRow("削除フラグ").ToString > "0" Then
                        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
                        ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
                    End If
                End If


                '--------------------------------
                '2014/05/21 後藤　ここから
                '--------------------------------
                msSetddlWrkdtl(dtRow("システムコード").ToString)    '作業内容マスタ

                ViewState("P_SYSTEM_CD") = dtRow("システムコード").ToString
                Me.ddlWrk.SelectedValue = dtRow("作業内容コード").ToString
                '--------------------------------
                '2014/05/21 後藤　ここまで
                '--------------------------------

                If Me.ddlCmp.ppSelectedValue <> "1" Then
                    Me.txtWrkCode.ppTextBox.Enabled = False
                Else
                    Me.txtWrkCode.ppTextBox.Enabled = True
                End If

                '--保守対応明細データの取得
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S2", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '保守管理番号
                    .Add(pfSet_Param("mntno", SqlDbType.NVarChar, strMntNo))
                    .Add(pfSet_Param("auth", SqlDbType.NVarChar, Session(P_SESSION_AUTH).ToString))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '取得したデータをグリッドに設定
                Me.grvList.DataSource = objDs.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

                mfGetData = True

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "保守対応依頼書")
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
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    mfGetData = False
                End If
            End Try
        End If

    End Function

    ''' <summary>
    ''' データ取得（対応明細）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGetDataDetail()

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

        'グリッドの初期化
        Me.grvList.DataSource = New DataTable
        Me.grvList.DataBind()

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S2", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '保守管理番号
                    .Add(pfSet_Param("mntno", SqlDbType.NVarChar, Me.lblMntNo.Text))
                    .Add(pfSet_Param("auth", SqlDbType.NVarChar, Session(P_SESSION_AUTH).ToString))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '取得したデータをグリッドに設定
                Me.grvList.DataSource = objDs.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守対応依頼書")
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
    ''' ドロップダウンリスト設定（ 0043:保守管理区分 ※発生区分 ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlOccurCls()

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
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S7", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                If ddlOccurCls.Enabled = True Then
                    If objDs.Tables(0).Rows.Count > 11 Then
                        objDs.Tables(0).Rows.RemoveAt(objDs.Tables(0).Rows.Count - 1)
                    End If
                End If
                'ドロップダウンリスト設定
                Me.ddlOccurCls.Items.Clear()
                Me.ddlOccurCls.DataSource = objDs.Tables(0)
                Me.ddlOccurCls.DataTextField = "名称"
                Me.ddlOccurCls.DataValueField = "コード"
                Me.ddlOccurCls.DataBind()
                Me.ddlOccurCls.Items.Insert(0, New ListItem(Nothing, Nothing))  '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "区分マスタ一覧取得")
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
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（申告元マスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlRptsrc()

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
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL017", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                Me.ddlRptBase.Items.Clear()
                Me.ddlRptBase.DataSource = objDs.Tables(0)
                Me.ddlRptBase.DataTextField = "名称"
                Me.ddlRptBase.DataValueField = "申告元コード"
                Me.ddlRptBase.DataBind()
                Me.ddlRptBase.Items.Insert(0, New ListItem(Nothing, Nothing))  '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "申告元マスタ一覧取得")
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
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（申告内容マスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlRptcntt(ByVal strSystemCd As String)

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL018", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '保守管理番号
                    .Add(pfSet_Param("sys_code", SqlDbType.NVarChar, strSystemCd))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                'ドロップダウンリスト設定
                Me.ddlRpt.Items.Clear()
                Me.ddlRpt.DataSource = objDs.Tables(0)
                Me.ddlRpt.DataTextField = "文言"
                Me.ddlRpt.DataValueField = "申告内容コード"
                Me.ddlRpt.DataBind()
                Me.ddlRpt.Items.Insert(0, New ListItem(Nothing, Nothing))  '先頭に空白行を追加
                Me.ddlRpt.SelectedIndex = -1

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "申告内容マスタ一覧取得")
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
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（ステータスマスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlStatus()

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
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL015", objCn)
                With objCmd.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("duty_cd", SqlDbType.NVarChar, "69"))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                Me.ddlStatus.Items.Clear()
                Me.ddlStatus.DataSource = objDs.Tables(0)
                Me.ddlStatus.DataTextField = "進捗ステータス名"
                Me.ddlStatus.DataValueField = "進捗ステータス"
                Me.ddlStatus.DataBind()
                Me.ddlStatus.Items.Insert(0, New ListItem(Nothing, Nothing))  '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "進捗ステータスマスタ一覧取得")
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
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（回復内容マスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlRprdtl(ByVal strSystemCd As String)

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
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL014", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '保守管理番号
                    .Add(pfSet_Param("sys_code", SqlDbType.NVarChar, strSystemCd))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                Me.ddlRepair.Items.Clear()
                Me.ddlRepair.DataSource = objDs.Tables(0)
                Me.ddlRepair.DataTextField = "名称"
                Me.ddlRepair.DataValueField = "回復内容コード"
                Me.ddlRepair.DataBind()
                Me.ddlRepair.Items.Insert(0, New ListItem(Nothing, Nothing))  '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "回復内容マスタ一覧取得")
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
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（作業内容マスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlWrkdtl(ByVal strSystemCd As String)

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL019", objCn)
                '--------------------------------
                '2014/05/21 後藤　ここから
                '--------------------------------
                With objCmd.Parameters
                    .Add(pfSet_Param("systemcd", SqlDbType.NVarChar, strSystemCd))
                End With
                '--------------------------------
                '2014/05/21 後藤　ここまで
                '--------------------------------

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定

                Me.ddlWrk.Items.Clear()
                Me.ddlWrk.DataSource = objDs.Tables(0)
                Me.ddlWrk.DataTextField = "名称"
                Me.ddlWrk.DataValueField = "作業内容コード"
                Me.ddlWrk.DataBind()
                Me.ddlWrk.Items.Insert(0, New ListItem(Nothing, Nothing))  '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "作業内容マスタ一覧取得")
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
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（処置内容マスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlDealcntt(ByVal strSystemCd As String)

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
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL020", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '保守管理番号
                    .Add(pfSet_Param("sys_code", SqlDbType.NVarChar, strSystemCd))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                Me.ddlDeal.Items.Clear()
                Me.ddlDeal.DataSource = objDs.Tables(0)
                Me.ddlDeal.DataTextField = "文言"
                Me.ddlDeal.DataValueField = "処置内容コード"
                Me.ddlDeal.DataBind()
                Me.ddlDeal.Items.Insert(0, New ListItem(Nothing, Nothing))  '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "処置内容マスタ一覧取得")
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
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（特別保守マスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlMntFlg()

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
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL044", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                Me.ddlMntFlg.Items.Clear()
                Me.ddlMntFlg.DataSource = objDs.Tables(0)
                Me.ddlMntFlg.DataTextField = "名称"
                Me.ddlMntFlg.DataValueField = "特別保守料金コード"
                Me.ddlMntFlg.DataBind()
                Me.ddlMntFlg.Items.Insert(0, New ListItem(Nothing, Nothing))  '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "特別保守料金マスタ一覧取得")
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
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' 作業内容マスタの設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetWrkInfo()

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

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S3", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '機器分類コード
                    .Add(pfSet_Param("code", SqlDbType.NVarChar, Me.ddlWrk.SelectedValue))
                    '--------------------------------
                    '2014/05/21 後藤　ここから
                    '--------------------------------
                    .Add(pfSet_Param("systemcd", SqlDbType.NVarChar, ViewState("P_SYSTEM_CD")))
                    '--------------------------------
                    '2014/05/21 後藤　ここまで
                    '--------------------------------
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs Is Nothing Then
                    Exit Sub
                End If

                If objDs.Tables(0).Rows.Count > 0 Then
                    dtRow = objDs.Tables(0).Rows(0)

                    '作業内容テキストボックス設定
                    Me.txtMntTm.ppText = dtRow("作業時間").ToString
                    Me.txtGbTm.ppText = dtRow("往復時間").ToString
                    Me.txtPsnNum.ppText = dtRow("人数").ToString

                End If

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "作業内容マスタ取得")
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
    ''' ＴＢＯＸ情報取得処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetTboxInfo() As Boolean

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

        mfGetTboxInfo = False

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S4", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    'ＴＢＯＸＩＤ
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTBoxid.ppText))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables(0).Rows.Count = 0 Then
                    Me.txtTBoxid.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
                    Me.txtTBoxid.ppTextBox.Focus()
                    msInitTBoxData()
                    Exit Function
                End If

                dtRow = objDs.Tables(0).Rows(0)

                'ＴＢＯＸ情報をラベルに設定
                Me.lblNLCls.Text = dtRow("ＮＬ区分").ToString
                Me.lblTboxType.Text = dtRow("ＴＢＯＸタイプ").ToString
                Me.lblTboxVer.Text = dtRow("ＶＥＲ").ToString
                Me.lblHallNm.Text = dtRow("ホール名").ToString
                Me.lblEWCls.Text = dtRow("ＥＷ区分").ToString
                Me.lblAddr1.Text = dtRow("ホール住所１").ToString
                Me.lblBranchNm.Text = dtRow("保担名").ToString
                Me.lblTelno.Text = dtRow("ホールＴＥＬ").ToString
                Me.lblUnfNm.Text = dtRow("統括保担名").ToString

                If ddlRpt.SelectedValue = "" Then
                    msSetddlRptcntt(dtRow("システムコード").ToString)   '申告内容マスタ
                End If
                msSetddlWrkdtl(dtRow("システムコード").ToString)    '作業内容マスタ
                msSetddlRprdtl(dtRow("システムコード").ToString)    '回復内容マスタ
                msSetddlDealcntt(dtRow("システムコード").ToString)  '処置内容マスタ                mfGetTboxInfo = True
                ViewState("P_SYSTEM_CD") = dtRow("システムコード").ToString
                'CMPSELP001-003
                ViewState("SYSTEM_CLS") = dtRow("システムクラス").ToString
                ViewState("HALL_CD") = dtRow("ホールコード").ToString
                'CMPSELP001-003 END

                If dtRow("システムクラス").ToString = "5" _
                OrElse dtRow("システムクラス").ToString = "3" Then
                    Master.ppRigthButton4.Visible = True
                Else
                    Master.ppRigthButton4.Visible = False
                End If

                If dtRow("双子店区分").ToString = "0" Then
                    lblTwin.Text = "単独店"
                    hdnTwinCd.Value = "0"
                ElseIf dtRow("双子店区分").ToString = "1" Then
                    lblTwin.Text = "双子店"
                    hdnTwinCd.Value = "1"
                Else
                    lblTwin.Text = ""
                    hdnTwinCd.Value = ""
                End If
                lblAgc.Text = dtRow("代理店名").ToString
                hdnAgcCd.Value = dtRow("代理店ＣＤ").ToString
                hdnAgcZip.Value = dtRow("代理店郵便番号").ToString
                hdnAgcAddr.Value = dtRow("代理店住所").ToString
                hdnAgcTel.Value = dtRow("代理店ＴＥＬ").ToString
                hdnAgcFax.Value = dtRow("代理店ＦＡＸ").ToString
                'CMPSELP001-004
                If dtRow("代行店表示フラグ") = "1" Then
                    lblRep.Visible = False
                Else
                    lblRep.Visible = True
                End If
                'CMPSELP001-004
                lblRep.Text = dtRow("代行店名").ToString
                hdnRepCd.Value = dtRow("代行店ＣＤ").ToString
                hdnRepZip.Value = dtRow("代行店郵便番号").ToString
                hdnRepAddr.Value = dtRow("代行店住所").ToString
                hdnRepTel.Value = dtRow("代行店ＴＥＬ").ToString
                hdnRepChg.Value = dtRow("代行店担当者").ToString
                lblNgcOrg.Text = dtRow("ＮＧＣ担当営業部").ToString
                lblOrgTel.Text = dtRow("担当営業部ＴＥＬ").ToString
                lblEst.Text = dtRow("ＭＤＮ設置有無").ToString
                hdnEstCls.Value = dtRow("ＭＤＮ設置有無ＣＤ").ToString
                lblMdn.Text = dtRow("ＭＤＮ機器名").ToString
                hdnMdnCnt.Value = dtRow("ＭＤＮ台数").ToString
                hdnMdnCd1.Value = dtRow("ＭＤＮＣＤ１").ToString
                hdnMdnCd2.Value = dtRow("ＭＤＮＣＤ２").ToString
                hdnMdnCd3.Value = dtRow("ＭＤＮＣＤ３").ToString

                If objDs.Tables(0).Rows(0).Item("運用終了日").ToString.Trim <> Nothing Then
                    If Date.Parse(objDs.Tables(0).Rows(0).Item("運用終了日").ToString.Trim) < DateTime.Now Then
                        Me.txtTBoxid.psSet_ErrorNo("2016", "ホール", "登録")
                        Me.txtTBoxid.ppTextBox.Focus()
                        Return False
                    End If
                End If

                Me.ddlWrk.SelectedIndex = 0
                Me.ddlRepair.SelectedIndex = 0
                Me.ddlDeal.SelectedIndex = 0
                mfGetTboxInfo = True

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "ＴＢＯＸ情報取得")
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
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    mfGetTboxInfo = False
                End If
            End Try
        End If

    End Function

    ''' <summary>
    ''' 保守対応データ登録処理
    ''' </summary>
    ''' <param name="strMntNo">保守管理番号</param>
    ''' <returns>True:正常終了、False:異常終了</returns>
    ''' <remarks></remarks>
    Private Function mfEntryData(ByRef strMntNo As String) As Boolean

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim intRtn As Integer = 0               'ストアド戻り値
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfEntryData = False

        '保守対応データ登録
        If clsDataConnect.pfOpen_Database(objCn) Then
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_U1", objCn)
                With objCmd.Parameters
                    'パラメータ設定
                    '--ＴＢＯＸＩＤ
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTBoxid.ppText))
                    '--管理区分
                    .Add(pfSet_Param("mntcls", SqlDbType.NVarChar, Me.ddlOccurCls.SelectedValue))
                    '--ユーザＩＤ
                    .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))
                    '--保守管理番号
                    .Add(pfSet_Param("mntno", SqlDbType.NVarChar, 12, ParameterDirection.Output))
                    '--トラブル管理番号
                    .Add(pfSet_Param("trblno", SqlDbType.NVarChar, Me.lblTrbNo.Text))
                    '--申告日(yyyy/MM/dd)
                    If Me.dtbRptDt.ppText = String.Empty Then
                        .Add(pfSet_Param("rpt_dt", SqlDbType.NVarChar, DBNull.Value))
                    Else
                        .Add(pfSet_Param("rpt_dt", SqlDbType.NVarChar, Me.dtbRptDt.ppText))
                    End If
                    '--申告者
                    .Add(pfSet_Param("rpt_charge", SqlDbType.NVarChar, Me.txtRptCharge.ppText))
                    '--申告元コード
                    .Add(pfSet_Param("rpt_base", SqlDbType.NVarChar, Me.ddlRptBase.SelectedValue))
                    '--申告元ＴＥＬ
                    .Add(pfSet_Param("rpt_tel", SqlDbType.NVarChar, Me.lblRptTel.Text))
                    '--受付日時(yyyy/MM/dd HH:mm)
                    If Me.dtbRcptDt.ppText = String.Empty AndAlso
                        Me.dtbRcptDt.ppHourText = String.Empty AndAlso
                        Me.dtbRcptDt.ppMinText = String.Empty Then
                        .Add(pfSet_Param("rcpt_dt", SqlDbType.NVarChar, DBNull.Value))
                    Else
                        .Add(pfSet_Param("rcpt_dt", SqlDbType.NVarChar, Me.dtbRcptDt.ppText + " " +
                                            Me.dtbRcptDt.ppHourText + ":" + Me.dtbRcptDt.ppMinText))
                    End If

                    .Add(pfSet_Param("acg_cod", SqlDbType.NVarChar, hdnAgcCd.Value)) '代理店コード
                    .Add(pfSet_Param("agc_nam", SqlDbType.NVarChar, lblAgc.Text)) '代理店名
                    .Add(pfSet_Param("acg_zip", SqlDbType.NVarChar, hdnAgcZip.Value)) '代理店郵便番号
                    .Add(pfSet_Param("acg_ads", SqlDbType.NVarChar, hdnAgcAddr.Value)) '代理店住所
                    .Add(pfSet_Param("acg_tel", SqlDbType.NVarChar, hdnAgcTel.Value)) '代理店ＴＥＬ
                    .Add(pfSet_Param("acg_fax", SqlDbType.NVarChar, hdnAgcFax.Value)) '代理店ＦＡＸ
                    .Add(pfSet_Param("per_cod", SqlDbType.NVarChar, hdnRepCd.Value)) '代行店コード
                    .Add(pfSet_Param("per_nam", SqlDbType.NVarChar, lblRep.Text)) '代行店名
                    .Add(pfSet_Param("per_zip", SqlDbType.NVarChar, hdnRepZip.Value)) '代行店郵便番号
                    .Add(pfSet_Param("per_ads", SqlDbType.NVarChar, hdnRepAddr.Value)) '代行店住所
                    .Add(pfSet_Param("per_tel", SqlDbType.NVarChar, hdnRepTel.Value)) '代行店ＴＥＬ
                    .Add(pfSet_Param("per_stf_cod", SqlDbType.NVarChar, hdnRepChg.Value)) '代行店担当者
                    .Add(pfSet_Param("ngc_bns", SqlDbType.NVarChar, lblNgcOrg.Text)) 'ＮＧＣ担当営業部
                    .Add(pfSet_Param("ngc_tel", SqlDbType.NVarChar, lblOrgTel.Text)) 'ＮＧＣ担当営業部ＴＥＬ
                    .Add(pfSet_Param("mdn_set_dvs", SqlDbType.NVarChar, hdnTwinCd.Value)) '双子店区分
                    .Add(pfSet_Param("mdn_dvs", SqlDbType.NVarChar, hdnEstCls.Value)) 'ＭＤＮ設置区分
                    .Add(pfSet_Param("mdn_suu", SqlDbType.NVarChar, hdnMdnCnt.Value)) 'ＭＤＮ台数
                    .Add(pfSet_Param("mdn_cod1", SqlDbType.NVarChar, hdnMdnCd1.Value)) 'ＭＤＮ機種コード１
                    .Add(pfSet_Param("mdn_cod2", SqlDbType.NVarChar, hdnMdnCd2.Value)) 'ＭＤＮ機種コード２
                    .Add(pfSet_Param("mdn_cod3", SqlDbType.NVarChar, hdnMdnCd3.Value)) 'ＭＤＮ機種コード３

                    '--受付者
                    .Add(pfSet_Param("rcpt_charge", SqlDbType.NVarChar, Me.txtRcptCharge.ppText))
                    '--戻り値
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                End With

                'データ追加
                Using conTrn = objCn.BeginTransaction
                    objCmd.Transaction = conTrn

                    'コマンドタイプ設定(ストアド)
                    objCmd.CommandType = CommandType.StoredProcedure

                    'ストアド実行
                    objCmd.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(objCmd.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, "00008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守対応依頼書", intRtn.ToString)
                        Exit Function
                    End If

                    '保守管理番号の取得
                    strMntNo = objCmd.Parameters("mntno").Value.ToString()

                    Select Case Session(P_SESSION_AUTH)
                        Case "管理者", "SPC", "NGC"
                            '★排他制御用の変数
                            Dim dtExclusiveDate As String = String.Empty
                            Dim arTable_Name As New ArrayList
                            Dim arKey As New ArrayList

                            '★ロック対象テーブル名の登録
                            arTable_Name.Insert(0, "D75_DEAL_MAINTAIN")
                            arTable_Name.Insert(1, "D76_DEALMAINTAIN_DTIL")

                            '★ロックテーブルキー項目の登録(D75_DEAL_MAINTAIN,D76_DEALMAINTAIN_DTIL)
                            arKey.Insert(0, objCmd.Parameters("mntno").Value.ToString()) 'D75_MNT_NO
                            '★排他情報確認処理(更新処理の実行)
                            If clsExc.pfSel_Exclusive(dtExclusiveDate _
                                                , Me _
                                                , Session(P_SESSION_IP) _
                                                , Session(P_SESSION_PLACE) _
                                                , Session(P_SESSION_USERID) _
                                                , Session(P_SESSION_SESSTION_ID) _
                                                , ViewState(P_SESSION_GROUP_NUM) _
                                                , M_MY_DISP_ID _
                                                , arTable_Name _
                                                , arKey) = 0 Then

                                '★登録年月日時刻
                                Me.Master.ppExclusiveDate = dtExclusiveDate
                            End If

                            'コミット
                            conTrn.Commit()
                    End Select

                End Using
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "保守対応依頼書")

                mfEntryData = True

            Catch ex As Exception
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守対応依頼書")
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
                    mfEntryData = False
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Function

    ''' <summary>
    ''' 保守対応データ更新処理
    ''' </summary>
    ''' <param name="strMntNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfUpdateData(ByVal strMntNo As String) As Boolean

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim intRtn As Integer = 0               'ストアド戻り値
        Dim strBuff As String = String.Empty
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfUpdateData = False

        '保守対応データ更新
        If clsDataConnect.pfOpen_Database(objCn) Then
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_U2", objCn)
                With objCmd.Parameters
                    'パラメータ設定
                    '--保守管理番号
                    .Add(pfSet_Param("mntno", SqlDbType.NVarChar, strMntNo))
                    '--ユーザＩＤ
                    .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))
                    '--申告日(yyyy/MM/dd)
                    If Me.dtbRptDt.ppText = String.Empty Then
                        .Add(pfSet_Param("rpt_dt", SqlDbType.NVarChar, DBNull.Value))
                    Else
                        .Add(pfSet_Param("rpt_dt", SqlDbType.NVarChar, Me.dtbRptDt.ppText))
                    End If
                    '--申告者
                    .Add(pfSet_Param("rpt_charge", SqlDbType.NVarChar, Me.txtRptCharge.ppText))
                    '--申告元コード
                    .Add(pfSet_Param("rpt_base", SqlDbType.NVarChar, Me.ddlRptBase.SelectedValue))
                    '--申告元ＴＥＬ
                    .Add(pfSet_Param("rpt_tel", SqlDbType.NVarChar, Me.lblRptTel.Text))
                    '--申告内容コード
                    .Add(pfSet_Param("rpt_cd", SqlDbType.NVarChar, Me.ddlRpt.SelectedValue))
                    '--申告内容１
                    .Add(pfSet_Param("rpt_dtl1", SqlDbType.NVarChar, Me.txtRptDtl1.ppText))
                    '--申告内容２
                    .Add(pfSet_Param("rpt_dtl2", SqlDbType.NVarChar, Me.txtRptDtl2.ppText))
                    '--受付日時(yyyy/MM/dd HH:mm)
                    If Me.dtbRcptDt.ppText = String.Empty AndAlso
                        Me.dtbRcptDt.ppHourText = String.Empty AndAlso
                        Me.dtbRcptDt.ppMinText = String.Empty Then
                        .Add(pfSet_Param("rcpt_dt", SqlDbType.NVarChar, DBNull.Value))
                    Else
                        .Add(pfSet_Param("rcpt_dt", SqlDbType.NVarChar, Me.dtbRcptDt.ppText + " " +
                                            Me.dtbRcptDt.ppHourText + ":" + Me.dtbRcptDt.ppMinText))
                    End If
                    '--受付者
                    .Add(pfSet_Param("rcpt_charge", SqlDbType.NVarChar, Me.txtRcptCharge.ppText))
                    '--依頼日時(yyyy/MM/dd HH:mm)
                    If Me.txtReqUsr.ppText = String.Empty Then
                        intEmpFlg = 1
                        .Add(pfSet_Param("req_dt", SqlDbType.NVarChar, DBNull.Value))
                    Else
                        If Me.dtbReqDt.ppText = String.Empty AndAlso
                            Me.dtbReqDt.ppHourText = String.Empty AndAlso
                            Me.dtbReqDt.ppMinText = String.Empty Then
                            .Add(pfSet_Param("req_dt", SqlDbType.NVarChar, DBNull.Value))
                        Else
                            .Add(pfSet_Param("req_dt", SqlDbType.NVarChar, Me.dtbReqDt.ppText + " " +
                                                Me.dtbReqDt.ppHourText + ":" + Me.dtbReqDt.ppMinText))
                        End If
                    End If
                    '--依頼者名称
                    .Add(pfSet_Param("req_usr", SqlDbType.NVarChar, Me.txtReqUsr.ppText))
                    '--引継内容１
                    .Add(pfSet_Param("inh_cntnt1", SqlDbType.NVarChar, Me.txtInhCntnt1.ppText))
                    '--引継内容２
                    .Add(pfSet_Param("inh_cntnt2", SqlDbType.NVarChar, Me.txtInhCntnt2.ppText))
                    '--作業予定日時(yyyy/MM/dd HH:mm)
                    If Me.dtbWrkDt.ppText = String.Empty AndAlso
                        Me.dtbWrkDt.ppHourText = String.Empty AndAlso
                        Me.dtbWrkDt.ppMinText = String.Empty Then
                        .Add(pfSet_Param("wrk_dt", SqlDbType.NVarChar, DBNull.Value))
                    Else
                        .Add(pfSet_Param("wrk_dt", SqlDbType.NVarChar, Me.dtbWrkDt.ppText + " " +
                                            Me.dtbWrkDt.ppHourText + ":" + Me.dtbWrkDt.ppMinText))
                    End If
                    '--作業者
                    .Add(pfSet_Param("wrk_usr", SqlDbType.NVarChar, Me.txtWrkUser.ppText))
                    .Add(pfSet_Param("cmp_cls", SqlDbType.NVarChar, Me.ddlCmp.ppSelectedValue))

                    '--出発日時(yyyy/MM/dd HH:mm)
                    If Me.dtbDeptDt.ppText = String.Empty AndAlso
                        Me.dtbDeptDt.ppHourText = String.Empty AndAlso
                        Me.dtbDeptDt.ppMinText = String.Empty Then
                        .Add(pfSet_Param("dept_dt", SqlDbType.NVarChar, DBNull.Value))
                    Else
                        .Add(pfSet_Param("dept_dt", SqlDbType.NVarChar, Me.dtbDeptDt.ppText + " " +
                                            Me.dtbDeptDt.ppHourText + ":" + Me.dtbDeptDt.ppMinText))
                    End If
                    '--開始日時(yyyy/MM/dd HH:mm)
                    If Me.dtbStartDt.ppText = String.Empty AndAlso
                        Me.dtbStartDt.ppHourText = String.Empty AndAlso
                        Me.dtbStartDt.ppMinText = String.Empty Then
                        .Add(pfSet_Param("start_dt", SqlDbType.NVarChar, DBNull.Value))
                    Else
                        .Add(pfSet_Param("start_dt", SqlDbType.NVarChar, Me.dtbStartDt.ppText + " " +
                                            Me.dtbStartDt.ppHourText + ":" + Me.dtbStartDt.ppMinText))
                    End If
                    '--終了日時(yyyy/MM/dd HH:mm)
                    If Me.dtbEndDt.ppText = String.Empty AndAlso
                        Me.dtbEndDt.ppHourText = String.Empty AndAlso
                        Me.dtbEndDt.ppMinText = String.Empty Then
                        .Add(pfSet_Param("end_dt", SqlDbType.NVarChar, DBNull.Value))
                    Else
                        .Add(pfSet_Param("end_dt", SqlDbType.NVarChar, Me.dtbEndDt.ppText + " " +
                                            Me.dtbEndDt.ppHourText + ":" + Me.dtbEndDt.ppMinText))
                    End If
                    '--作業状況コード
                    .Add(pfSet_Param("status_cd", SqlDbType.NVarChar, Me.ddlStatus.SelectedValue))
                    '--作業状況備考
                    .Add(pfSet_Param("stts_notetext", SqlDbType.NVarChar, Me.txtSttsNotetext.ppText))
                    '--故障機器コード
                    .Add(pfSet_Param("appa_cd", SqlDbType.NVarChar, Me.ddlAppa.ppSelectedValue))
                    '--回復内容コード
                    .Add(pfSet_Param("repair_cd", SqlDbType.NVarChar, Me.ddlRepair.SelectedValue))
                    '--回復内容
                    .Add(pfSet_Param("repair_cntnt", SqlDbType.NVarChar, Me.txtRepair.ppText))
                    '--備考・連絡１
                    .Add(pfSet_Param("notetext1", SqlDbType.NVarChar, Me.txtNotetext1.ppText))
                    '--備考・連絡２
                    .Add(pfSet_Param("notetext2", SqlDbType.NVarChar, Me.txtNotetext2.ppText))
                    '--故障重要度区分
                    strBuff = "0"
                    If Me.cbxImpCls.Checked Then
                        strBuff = "1"
                    End If
                    .Add(pfSet_Param("imp_cls", SqlDbType.NVarChar, strBuff))
                    '--営業支障区分
                    strBuff = "0"
                    If Me.cbxBsnsdistCls.Checked Then
                        strBuff = "1"
                    End If
                    .Add(pfSet_Param("bsnsdist_cls", SqlDbType.NVarChar, strBuff))
                    '--２次支障区分
                    strBuff = "0"
                    If Me.cbxScnddistCls.Checked Then
                        strBuff = "1"
                    End If
                    .Add(pfSet_Param("scnddist_cls", SqlDbType.NVarChar, strBuff))
                    '--ホール担当者
                    .Add(pfSet_Param("h_charge", SqlDbType.NVarChar, Me.txtHallCharge.ppText))
                    '--最終集信日(yyyy/MM/dd)
                    If Me.dtbLastDt.ppText = String.Empty Then
                        .Add(pfSet_Param("last_dt", SqlDbType.NVarChar, DBNull.Value))
                    Else
                        .Add(pfSet_Param("last_dt", SqlDbType.NVarChar, Me.dtbLastDt.ppText))
                    End If
                    '--特別保守フラグ
                    .Add(pfSet_Param("mnt_flg", SqlDbType.NVarChar, Me.ddlMntFlg.SelectedValue))
                    '--出発時間(HH:mm)
                    If Me.tmbDeptTm.ppHourText = String.Empty AndAlso
                        Me.tmbDeptTm.ppMinText = String.Empty Then
                        .Add(pfSet_Param("dept_tm", SqlDbType.NVarChar, DBNull.Value))
                    Else
                        .Add(pfSet_Param("dept_tm", SqlDbType.NVarChar,
                                            Me.tmbDeptTm.ppHourText + ":" + Me.tmbDeptTm.ppMinText))
                    End If
                    '--開始時間(HH:mm)
                    If Me.tmbStartTm.ppHourText = String.Empty AndAlso
                        Me.tmbStartTm.ppMinText = String.Empty Then
                        .Add(pfSet_Param("start_tm", SqlDbType.NVarChar, DBNull.Value))
                    Else
                        .Add(pfSet_Param("start_tm", SqlDbType.NVarChar,
                                            Me.tmbStartTm.ppHourText + ":" + Me.tmbStartTm.ppMinText))
                    End If
                    '--終了時間(HH:mm)
                    If Me.tmbEndTm.ppHourText = String.Empty AndAlso
                        Me.tmbEndTm.ppMinText = String.Empty Then
                        .Add(pfSet_Param("end_tm", SqlDbType.NVarChar, DBNull.Value))
                    Else
                        .Add(pfSet_Param("end_tm", SqlDbType.NVarChar,
                                            Me.tmbEndTm.ppHourText + ":" + Me.tmbEndTm.ppMinText))
                    End If
                    '--作業内容コード
                    .Add(pfSet_Param("wrk_cd", SqlDbType.NVarChar, Me.ddlWrk.SelectedValue))
                    '--特別保守時間
                    If Me.txtMntTm.ppText = String.Empty Then
                        .Add(pfSet_Param("mnt_tm", SqlDbType.NVarChar, DBNull.Value))
                    Else
                        .Add(pfSet_Param("mnt_tm", SqlDbType.NVarChar, Me.txtMntTm.ppText))
                    End If
                    '--特別保守往復時間
                    If Me.txtGbTm.ppText = String.Empty Then
                        .Add(pfSet_Param("gb_tm", SqlDbType.NVarChar, DBNull.Value))
                    Else
                        .Add(pfSet_Param("gb_tm", SqlDbType.NVarChar, Me.txtGbTm.ppText))
                    End If
                    '--作業人数
                    If Me.txtPsnNum.ppText = String.Empty Then
                        .Add(pfSet_Param("psn_num", SqlDbType.NVarChar, DBNull.Value))
                    Else
                        .Add(pfSet_Param("psn_num", SqlDbType.NVarChar, Me.txtPsnNum.ppText))
                    End If
                    '--請求金額
                    If Me.txtReqPrice.ppText = String.Empty Then
                        .Add(pfSet_Param("req_price", SqlDbType.NVarChar, DBNull.Value))
                    Else
                        .Add(pfSet_Param("req_price", SqlDbType.NVarChar, Me.txtReqPrice.ppText))
                    End If
                    '--提出日(yyyy/MM/dd)
                    If Me.dtbSubmitDt.ppText = String.Empty Then
                        .Add(pfSet_Param("submit_dt", SqlDbType.NVarChar, DBNull.Value))
                    Else
                        .Add(pfSet_Param("submit_dt", SqlDbType.NVarChar, Me.dtbSubmitDt.ppText))
                    End If
                    '--メーカ修理区分
                    .Add(pfSet_Param("repair_cls", SqlDbType.NVarChar, Me.ddlRepairCls.ppSelectedValue))
                    '--備考
                    .Add(pfSet_Param("notetext", SqlDbType.NVarChar, Me.txtNotetext.ppText))
                    '--処置内容コード
                    .Add(pfSet_Param("deal_cd", SqlDbType.NVarChar, Me.ddlDeal.SelectedValue))
                    '--処置内容
                    .Add(pfSet_Param("deal_dtl", SqlDbType.NVarChar, Me.txtDeal.ppText))
                    '--輸送日(yyyy/MM/dd)
                    If Me.dtbTransDt.ppText = String.Empty Then
                        .Add(pfSet_Param("trans_dt", SqlDbType.NVarChar, DBNull.Value))
                    Else
                        .Add(pfSet_Param("trans_dt", SqlDbType.NVarChar, Me.dtbTransDt.ppText))
                    End If
                    '--輸送元コード
                    .Add(pfSet_Param("transbf_cd", SqlDbType.NVarChar, Me.txtTransbfCd.ppText))
                    '--輸送元区分
                    .Add(pfSet_Param("transbf_cls", SqlDbType.NVarChar, Me.ddlTransbfCls.ppSelectedValue))
                    '--輸送先コード
                    .Add(pfSet_Param("transaf_cd", SqlDbType.NVarChar, Me.txtTransafCd.ppText))
                    '--輸送先区分
                    .Add(pfSet_Param("transaf_cls", SqlDbType.NVarChar, Me.ddlTransafCls.ppSelectedValue))
                    '--輸送物品
                    .Add(pfSet_Param("trans_item", SqlDbType.NVarChar, Me.txtTransItem.ppText))
                    '--輸送理由コード
                    .Add(pfSet_Param("transrsn_cd", SqlDbType.NVarChar, Me.ddlTransRsn.ppSelectedValue))
                    '--輸送会社
                    .Add(pfSet_Param("trans_comp", SqlDbType.NVarChar, Me.txtTransComp.ppText))
                    '--輸送区分
                    .Add(pfSet_Param("trans_cls", SqlDbType.NVarChar, Me.ddlTransCls.ppSelectedValue))
                    '--輸送金額
                    If Me.txtTransPrice.ppText = String.Empty Then
                        .Add(pfSet_Param("trans_price", SqlDbType.NVarChar, DBNull.Value))
                    Else
                        .Add(pfSet_Param("trans_price", SqlDbType.NVarChar, Me.txtTransPrice.ppText))
                    End If
                    '--連絡事項１
                    .Add(pfSet_Param("info1", SqlDbType.NVarChar, Me.txtInfo1.ppText))
                    '--連絡事項２
                    .Add(pfSet_Param("info2", SqlDbType.NVarChar, Me.txtInfo2.ppText))
                    '--作業者コード
                    .Add(pfSet_Param("wrk_usr_cd", SqlDbType.NVarChar, Me.txtWrkCode.ppText))
                    '--作業者TEL
                    .Add(pfSet_Param("wrk_usr_tel", SqlDbType.NVarChar, Me.txtWrkTel.ppText))
                    '--戻り値
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                End With

                'データ更新
                Using conTrn = objCn.BeginTransaction
                    objCmd.Transaction = conTrn

                    'コマンドタイプ設定(ストアド)
                    objCmd.CommandType = CommandType.StoredProcedure

                    'ストアド実行
                    objCmd.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(objCmd.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, "00007", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守対応依頼書", intRtn.ToString)
                        Exit Function
                    End If

                    'コミット
                    conTrn.Commit()

                End Using
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "保守対応依頼書")
                mfUpdateData = True

            Catch ex As Exception
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守対応依頼書")
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
                    mfUpdateData = False
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Function

    ''' <summary>
    ''' 保守対応データ削除処理
    ''' </summary>
    ''' <param name="strMntNo">保守管理番号</param>
    ''' <returns>True:正常終了、False:異常終了</returns>
    ''' <remarks></remarks>
    Private Function mfDeleteData(ByVal strMntNo As String) As Boolean
        Dim strKeyList As List(Of String) = Nothing 'セッションＫＥＹリスト
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim intRtn As Integer = 0               'ストアド戻り値
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfDeleteData = False

        '保守対応データ削除
        If clsDataConnect.pfOpen_Database(objCn) Then
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_D1", objCn)
                With objCmd.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("mntno", SqlDbType.NVarChar, strMntNo))            '保守管理番号
                    .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name)) 'ユーザＩＤ
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                   '戻り値
                End With

                'データ削除
                Using conTrn = objCn.BeginTransaction
                    objCmd.Transaction = conTrn

                    'コマンドタイプ設定(ストアド)
                    objCmd.CommandType = CommandType.StoredProcedure

                    'ストアド実行
                    objCmd.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(objCmd.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, "00009", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守対応依頼書", intRtn.ToString)
                        Exit Function
                    End If

                    '保守管理番号の取得
                    strMntNo = objCmd.Parameters("mntno").Value.ToString()

                    'コミット
                    conTrn.Commit()

                End Using

                Me.ddlRpt.Enabled = True
                Me.cbxImpCls.Enabled = True
                Me.txtRptDtl1.ppEnabled = True
                Me.txtRptDtl2.ppEnabled = True
                Me.cbxBsnsdistCls.Enabled = True
                Me.cbxScnddistCls.Enabled = True
                Me.txtInfo1.ppEnabled = True
                Me.txtInfo2.ppEnabled = True
                Me.dtbWrkDt.ppEnabled = True
                Me.dtbDeptDt.ppEnabled = True
                Me.ddlCmp.ppEnabled = True
                Me.txtWrkCode.ppEnabled = True
                Me.txtWrkUser.ppEnabled = True
                Me.dtbStartDt.ppEnabled = True
                Me.dtbEndDt.ppEnabled = True
                Me.ddlAppa.ppEnabled = True
                Me.txtSttsNotetext.ppEnabled = True
                Me.ddlRepair.Enabled = True
                Me.txtRepair.ppEnabled = True
                Me.txtNotetext1.ppEnabled = True
                Me.txtNotetext2.ppEnabled = True
                Me.ddlStatus.Enabled = True
                Me.cbxDealAdptCls.Enabled = True
                Me.txtDealCd.ppEnabled = True
                Me.dtbDealDt.ppEnabled = True
                Me.txtDealUsr.ppEnabled = True
                Me.txaDealDtl.Enabled = True
                Me.btnDetailInsert.Enabled = True
                Me.btnDetailClear.Enabled = True
                Me.txtWrkTel.ppEnabled = False


                '画面初期化
                msInitScreen()
                msInitTBoxData()

                ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録
                Session(P_SESSION_TERMS) = ViewState(P_SESSION_TERMS)

                If mfGetSession(strKeyList) Then
                    If ViewState(P_SESSION_OLDDISP) = M_TRBL_DISP_ID Then
                        'ＴＢＯＸＩＤ、トラブル管理番号の表示
                        Me.lblMntNo.Text = ""                       '保守管理番号
                        Me.lblTrbNo.Text = strKeyList(1)            'トラブル管理番号
                        Me.txtTBoxid.ppText = strKeyList(2)         'ＴＢＯＸＩＤ
                        Me.dtbRptDt.ppText = strKeyList(3)          '申告日
                        Me.txtRptCharge.ppText = strKeyList(4)      '申告者
                        Me.ddlRptBase.SelectedValue = strKeyList(5) '申告元コード
                        Me.lblRptTel.Text = strKeyList(6)           '申告者ＴＥＬ
                        If strKeyList(7).Length >= 16 Then
                            Me.dtbRcptDt.ppText = strKeyList(7).Substring(0, 10)        '受付日時（日付）
                            Me.dtbRcptDt.ppHourText = strKeyList(7).Substring(11, 2)    '受付日時（時）
                            Me.dtbRcptDt.ppMinText = strKeyList(7).Substring(14, 2)     '受付日時（分）
                        End If
                        Me.txtRcptCharge.ppText = strKeyList(8)     '受付者
                        Me.ddlRpt.SelectedValue = strKeyList(9)     '申告内容コード
                        Me.txtRptDtl1.ppText = strKeyList(10)     '申告内容１
                        Me.txtRptDtl2.ppText = strKeyList(11)     '申告内容２

                        'ＴＢＯＸ情報取得
                        If mfGetTboxInfo() Then
                            '発生区分へフォーカス
                            Me.ddlOccurCls.Focus()
                        Else
                            '画面を終了する
                            psClose_Window(Me)
                        End If
                    Else
                        '発生区分へフォーカス
                        Me.ddlOccurCls.Focus()
                    End If
                End If

                '各コントロールの活性・非活性
                msSetActivecontrol(ViewState(P_SESSION_TERMS))

                psMesBox(Me, "00002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "保守対応依頼書")

                mfDeleteData = True

            Catch ex As Exception
                psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守対応依頼書")
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
                    mfDeleteData = False
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Function

    ''' <summary>
    ''' 保守対応明細データ更新処理
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

        '保守対応明細データ登録
        If clsDataConnect.pfOpen_Database(objCn) Then
            Try
                Using conTrn = objCn.BeginTransaction

                    'トラブル処理を更新した場合
                    If Me.lblRecKbn.Text = "2" Then
                        'トラブル処理明細の削除
                        objCmd = New SqlCommand("CMPSELP001_D3", objCn)
                        With objCmd.Parameters
                            'パラメータ設定
                            '--保守管理番号
                            .Add(pfSet_Param("TRBL_NO", SqlDbType.NVarChar, Me.lblTrbNo.Text))
                            '--保守管理連番
                            .Add(pfSet_Param("TRBL_SEQ", SqlDbType.NVarChar, Me.lblDealMntSeq.Text))
                        End With

                        'データ追加・更新・削除
                        objCmd.Transaction = conTrn

                        'コマンドタイプ設定(ストアド)
                        objCmd.CommandType = CommandType.StoredProcedure

                        'ストアド実行
                        objCmd.ExecuteNonQuery()

                        '保守明細を追加で進める
                        If intMode <> 3 Then
                            intMode = 1
                        End If
                    End If

                    '実行ストアド名設定
                    Select Case intMode
                        Case 1  '追加
                            strSpName = M_MY_DISP_ID + "_U3"
                            strErrCode = "00003"
                            strErrCode2 = "00008"
                        Case 2  '更新
                            strSpName = M_MY_DISP_ID + "_U4"
                            strErrCode = "00001"
                            strErrCode2 = "00007"
                        Case 3  '削除
                            strSpName = M_MY_DISP_ID + "_D2"
                            strErrCode = "00002"
                            strErrCode2 = "00009"
                        Case Else
                            Exit Function
                    End Select

                    objCmd = New SqlCommand(strSpName, objCn)
                    With objCmd.Parameters
                        'パラメータ設定
                        '--保守管理番号
                        .Add(pfSet_Param("mnt_no", SqlDbType.NVarChar, Me.lblMntNo.Text))

                        '--保守管理連番
                        If intMode = 2 OrElse intMode = 3 Then
                            .Add(pfSet_Param("mnt_seq", SqlDbType.NVarChar, Me.lblDealMntSeq.Text))
                        End If

                        If intMode = 1 OrElse intMode = 2 Then

                            '--対応日時
                            If Me.dtbDealDt.ppText = String.Empty AndAlso
                                Me.dtbDealDt.ppHourText = String.Empty AndAlso
                                Me.dtbDealDt.ppMinText = String.Empty Then
                                .Add(pfSet_Param("deal_dt", SqlDbType.NVarChar, DBNull.Value))
                            Else
                                .Add(pfSet_Param("deal_dt", SqlDbType.NVarChar, Me.dtbDealDt.ppText + " " +
                                                    Me.dtbDealDt.ppHourText + ":" + Me.dtbDealDt.ppMinText))
                            End If
                            '--対応者名称
                            .Add(pfSet_Param("deal_usr", SqlDbType.NVarChar, Me.txtDealUsr.ppText))
                            '--対応内容
                            .Add(pfSet_Param("deal_dtl", SqlDbType.NVarChar, Me.txaDealDtl.Text))
                            '--対応コード
                            .Add(pfSet_Param("deal_cd", SqlDbType.NVarChar, Me.txtDealCd.ppText))
                            '--適応区分
                            strBuff = "0"
                            If Me.cbxDealAdptCls.Checked Then
                                strBuff = "1"
                            End If
                            .Add(pfSet_Param("adpt_cls", SqlDbType.NVarChar, strBuff))
                            'ユーザＩＤ
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))
                        End If

                        '戻り値
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                    End With

                    'データ追加・更新・削除
                    objCmd.Transaction = conTrn

                    'コマンドタイプ設定(ストアド)
                    objCmd.CommandType = CommandType.StoredProcedure

                    'ストアド実行
                    objCmd.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(objCmd.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, strErrCode2, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守対応依頼書", intRtn.ToString)
                        Exit Function
                    End If

                    'コミット
                    conTrn.Commit()
                End Using


                psMesBox(Me, strErrCode, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "保守対応明細データ")

                mfUpdateDataDetail = True

            Catch ex As Exception
                psMesBox(Me, strErrCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守対応依頼書")
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
                    mfUpdateDataDetail = False
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Function

    ''' <summary>
    ''' 保守対応データ更新時チェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheckDealMiantain()

        Dim strErrCode As String = String.Empty 'エラーコード
        Dim strbuff As String = String.Empty    '輸送元・輸送先名取得用
        Dim dtrErrMes As DataRow
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim dsData As DataSet

        '輸送元コード
        Select Case Me.ddlTransbfCls.ppSelectedValue
            Case 0  'ホール名取得
                If mfGetHallInfo(Me.txtTransbfCd.ppText, strbuff) Then
                    Me.lblTransbfNm.Text = strbuff
                Else
                    Me.lblTransbfNm.Text = String.Empty
                    Me.txtTransbfCd.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
                End If
            Case 1  '営業所名取得
                If mfGetOfficeInfo("2", Me.txtTransbfCd.ppText, strbuff) Then
                    Me.lblTransbfNm.Text = strbuff
                Else
                    Me.lblTransbfNm.Text = String.Empty
                    Me.txtTransbfCd.psSet_ErrorNo("2002", "入力した営業所コード")
                End If
            Case 2  '代理店名取得
                If mfGetOfficeInfo("4", Me.txtTransbfCd.ppText, strbuff) Then
                    Me.lblTransbfNm.Text = strbuff
                Else
                    Me.lblTransbfNm.Text = String.Empty
                    Me.txtTransbfCd.psSet_ErrorNo("2002", "入力した代理店コード")
                End If
            Case 3  '保守拠点名取得 CMPSELP001-001
                If mfGetOfficeInfo("3", Me.txtTransbfCd.ppText, strbuff) Then
                    Me.lblTransbfNm.Text = strbuff
                    Me.ddlTransafCls.ppDropDownList.Focus()
                Else
                    Me.lblTransbfNm.Text = String.Empty
                    Me.txtTransbfCd.psSet_ErrorNo("2002", "入力した保守拠点コード")
                End If
            Case 4  'NGC取得 CMPSELP001-001
                If mfGetOfficeInfo("1", Me.txtTransbfCd.ppText, strbuff) Then
                    Me.lblTransbfNm.Text = strbuff
                    Me.ddlTransafCls.ppDropDownList.Focus()
                Else
                    Me.lblTransbfNm.Text = String.Empty
                    Me.txtTransbfCd.psSet_ErrorNo("2002", "入力したNGCコード")
                End If
            Case Else
                If Me.txtTransbfCd.ppText <> String.Empty Then
                    Me.lblTransbfNm.Text = String.Empty
                    Me.ddlTransbfCls.psSet_ErrorNo("5003", "輸送元の場所区分")
                End If
        End Select

        '輸送先コード
        Select Case Me.ddlTransafCls.ppSelectedValue
            Case 0  'ホール名取得
                If mfGetHallInfo(Me.txtTransafCd.ppText, strbuff) Then
                    Me.lblTransafNm.Text = strbuff
                Else
                    Me.lblTransafNm.Text = String.Empty
                    Me.txtTransafCd.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
                End If
            Case 1  '営業所名取得
                If mfGetOfficeInfo("2", Me.txtTransafCd.ppText, strbuff) Then
                    Me.lblTransafNm.Text = strbuff
                Else
                    Me.lblTransafNm.Text = String.Empty
                    Me.txtTransafCd.psSet_ErrorNo("2002", "入力した営業所コード")
                End If
            Case 2  '代理店名取得
                If mfGetOfficeInfo("4", Me.txtTransafCd.ppText, strbuff) Then
                    Me.lblTransafNm.Text = strbuff
                Else
                    Me.lblTransafNm.Text = String.Empty
                    Me.txtTransafCd.psSet_ErrorNo("2002", "入力した代理店コード")
                End If
            Case 3  '保守拠点名取得 CMPSELP001-001
                If mfGetOfficeInfo("3", Me.txtTransafCd.ppText, strbuff) Then
                    Me.lblTransafNm.Text = strbuff
                    Me.txtTransItem.ppTextBox.Focus()
                Else
                    Me.lblTransafNm.Text = String.Empty
                    Me.txtTransafCd.psSet_ErrorNo("2002", "入力した保守拠点コード")
                End If
            Case 4  'NGC取得 CMPSELP001-001
                If mfGetOfficeInfo("1", Me.txtTransafCd.ppText, strbuff) Then
                    Me.lblTransafNm.Text = strbuff
                    Me.txtTransItem.ppTextBox.Focus()
                Else
                    Me.lblTransafNm.Text = String.Empty
                    Me.txtTransafCd.psSet_ErrorNo("2002", "入力したNGCコード")
                End If
            Case Else
                If Me.txtTransafCd.ppText <> String.Empty Then
                    Me.lblTransafNm.Text = String.Empty
                    Me.ddlTransafCls.psSet_ErrorNo("5003", "輸送先の場所区分")
                End If
        End Select


        If Me.ddlCmp.ppSelectedValue = "1" Then
            If Me.txtWrkCode.ppText <> "" Or Me.txtWrkUser.ppText <> "" Then
                'DB接続
                If Not clsDataConnect.pfOpen_Database(objCn) Then
                    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Else
                    Try
                        objCmd = New SqlCommand("ZCMPSEL048", objCn)
                        With objCmd.Parameters
                            If Me.txtWrkCode.ppText <> "" Then
                                .Add(pfSet_Param("emp_cd", SqlDbType.NVarChar, Me.txtWrkCode.ppText))
                            Else
                                .Add(pfSet_Param("emp_cd", SqlDbType.NVarChar, "エラー"))
                            End If
                            If Me.txtWrkUser.ppText <> "" Then
                                .Add(pfSet_Param("emp_name", SqlDbType.NVarChar, Me.txtWrkUser.ppText))
                            Else
                                .Add(pfSet_Param("emp_name", SqlDbType.NVarChar, "エラー"))
                            End If
                        End With

                        'データ取得
                        dsData = clsDataConnect.pfGet_DataSet(objCmd)
                        If dsData.Tables(0).Rows.Count = 0 Then
                            Me.txtWrkCode.psSet_ErrorNo("2012", "作業者のコードと名称")
                        End If

                    Catch ex As Exception
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle)
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
            End If
        End If

        '作業状況
        If ddlStatus.SelectedIndex = 0 Then
            dtrErrMes = ClsCMCommon.pfGet_ValMes("5003", "作業状況")
            cuvStatus.Text = "未入力エラー"
            cuvStatus.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
            cuvStatus.Enabled = True
            cuvStatus.IsValid = False
            cuvStatus.SetFocusOnError = True
        End If

        '作業員TEL
        Dim lng As Long = 0
        If Not Me.txtWrkTel.ppText = String.Empty And _
         Not Long.TryParse(Me.txtWrkTel.ppText.Replace("-", ""), lng) Then
            Me.txtWrkTel.psSet_ErrorNo("4001", Me.txtWrkTel.ppName, "ハイフン(-)か数字")
        End If

        '依頼者
        If dtbReqDt.ppText <> String.Empty And _
            dtbReqDt.ppHourText <> String.Empty And _
            dtbReqDt.ppMinText <> String.Empty And _
            txtReqUsr.ppText = String.Empty Then
            Me.txtReqUsr.psSet_ErrorNo("5006", Me.txtReqUsr.ppName, "未入力エラー")
        End If

    End Sub

    ''' <summary>
    ''' 保守対応明細データ追加・更新時チェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheckDealMiantainDtil()

        '対応コード
        If Me.txtDealCd.ppText <> "GR" + Me.lblMntNo.Text.Substring(Me.lblMntNo.Text.Length - 3, 3) Then
            Me.txtDealCd.psSet_ErrorNo("4010")
        End If

    End Sub

    ''' <summary>
    ''' ホール情報取得処理
    ''' </summary>
    ''' <param name="strTboxid"></param>
    ''' <param name="strHallName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetHallInfo(ByVal strTboxid As String, ByRef strHallName As String) As Boolean

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

        mfGetHallInfo = False

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL028", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    'ＴＢＯＸＩＤ
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, strTboxid))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables(0).Rows.Count = 0 Then
                    Exit Function
                End If

                dtRow = objDs.Tables(0).Rows(0)

                'ホール名設定
                strHallName = dtRow("ホール名").ToString

                mfGetHallInfo = True

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ホール情報取得")
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
                    mfGetHallInfo = False
                End If
            End Try
        End If

    End Function

    ''' <summary>
    ''' 営業所・代理店情報取得処理
    ''' </summary>
    ''' <param name="strTradercd"></param>
    ''' <param name="strOfficecd"></param>
    ''' <param name="strOfficeName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetOfficeInfo(ByVal strTradercd As String, ByVal strOfficecd As String, ByRef strOfficeName As String) As Boolean

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

        mfGetOfficeInfo = False

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL029", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '業者コード
                    .Add(pfSet_Param("trader_cd", SqlDbType.NVarChar, strTradercd))
                    '営業所・代理店コード
                    .Add(pfSet_Param("office_cd", SqlDbType.NVarChar, strOfficecd))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables(0).Rows.Count = 0 Then
                    Exit Function
                End If

                dtRow = objDs.Tables(0).Rows(0)

                '営業所名設定
                strOfficeName = dtRow("営業所名").ToString

                mfGetOfficeInfo = True

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "営業所・代理店情報取得")
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
                    mfGetOfficeInfo = False
                End If
            End Try
        End If

    End Function

    ''' <summary>
    ''' 保守依頼書印刷用データ取得処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDataMntReq() As DataSet

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGetDataMntReq = Nothing

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S5", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '保守管理番号
                    .Add(pfSet_Param("mntno", SqlDbType.NVarChar, Me.lblMntNo.Text))

                End With

                'データ取得
                mfGetDataMntReq = clsDataConnect.pfGet_DataSet(objCmd)

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle)
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
    ''' 保守対応状況リスト印刷用データ取得処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDataMntHistLst() As DataSet

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGetDataMntHistLst = Nothing

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S6", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '保守管理番号
                    .Add(pfSet_Param("mntno", SqlDbType.NVarChar, Me.lblMntNo.Text))

                End With

                'データ取得
                mfGetDataMntHistLst = clsDataConnect.pfGet_DataSet(objCmd)

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle)
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
    ''' セッション情報取得処理
    ''' </summary>
    ''' <param name="strParamList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetSession(ByRef strParamList As List(Of String))

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        mfGetSession = False

        Try
            '遷移元ＩＤ
            If Session(P_SESSION_OLDDISP) = M_MNT_DISP_ID OrElse
                Session(P_SESSION_OLDDISP) = M_SMNT_DISP_ID OrElse
                Session(P_SESSION_OLDDISP) = M_TRBL_DISP_ID OrElse
                Session(P_SESSION_OLDDISP) = M_CTI_DISP_ID OrElse
                Session(P_SESSION_OLDDISP) = M_CHST_DISP_ID OrElse
                Session(P_SESSION_OLDDISP) = M_HALMST_DISP_ID Then  'CMPSELP001-001 ホールマスタ管理追加
                ViewState(P_SESSION_OLDDISP) = Session(P_SESSION_OLDDISP)
            Else
                Exit Function
            End If

            '遷移条件
            ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)

            '画面間パラメータ
            Dim strPkey() As String = Session(P_KEY)

            If Not strPkey Is Nothing Then

                Select Case ViewState(P_SESSION_OLDDISP)
                    Case M_MNT_DISP_ID, M_SMNT_DISP_ID, M_CTI_DISP_ID, M_CHST_DISP_ID
                        If strPkey.Length <> 1 Then
                            Exit Function
                        Else
                            strParamList = New List(Of String)
                            strParamList.Add(strPkey(0))    '保守管理番号
                        End If
                    Case M_HALMST_DISP_ID   'CMPSELP001-001 ホールマスタ管理追加
                        If strPkey.Length = 0 Then
                            Exit Function
                        Else
                            strParamList = New List(Of String)
                            strParamList.Add(strPkey(0))    'TBOXID
                        End If
                    Case M_TRBL_DISP_ID
                        If strPkey.Length <> 12 Then
                            Exit Function
                        Else
                            strParamList = New List(Of String)
                            strParamList.Add(strPkey(0))    '保守管理番号
                            strParamList.Add(strPkey(1))    'トラブル管理番号
                            strParamList.Add(strPkey(2))    'ＴＢＯＸＩＤ
                            strParamList.Add(strPkey(3))    '申告日
                            strParamList.Add(strPkey(4))    '申告者
                            strParamList.Add(strPkey(5))    '申告元コード
                            strParamList.Add(strPkey(6))    '申告者ＴＥＬ
                            strParamList.Add(strPkey(7))    '受付日時
                            strParamList.Add(strPkey(8))    '受付者
                            strParamList.Add(strPkey(9))    '申告内容
                            strParamList.Add(strPkey(10))    '申告内容備考１
                            strParamList.Add(strPkey(11))    '申告内容備考２
                        End If
                    Case Else
                        Exit Function

                End Select
            Else
                Exit Function
            End If

            mfGetSession = True

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
        Finally
        End Try

    End Function

#End Region

#Region "終了処理プロシージャ"
#End Region

    Protected Sub ddlMntFlg_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMntFlg.SelectedIndexChanged
        Calc_TextChanged(sender, e)
    End Sub

    Private Sub GetEmployee(sender As Object, e As EventArgs)
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim dsData As DataSet

        Me.txtWrkTel.ppText = ""
        If Me.ddlCmp.ppSelectedValue <> "1" Then
            Exit Sub
        End If

        If sender.ClientID = Me.txtWrkCode.ppTextBox.ClientID Then
            If Me.txtWrkCode.ppText = "" Then
                Exit Sub
            End If
        Else
            If Me.txtWrkUser.ppText = "" Then
                Exit Sub
            End If
        End If

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL048", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    If sender.ClientID = Me.txtWrkCode.ppTextBox.ClientID Then
                        .Add(pfSet_Param("emp_cd", SqlDbType.NVarChar, Me.txtWrkCode.ppText))
                        .Add(pfSet_Param("emp_name", SqlDbType.NVarChar, ""))
                    Else
                        .Add(pfSet_Param("emp_cd", SqlDbType.NVarChar, ""))
                        .Add(pfSet_Param("emp_name", SqlDbType.NVarChar, Me.txtWrkUser.ppText))
                    End If
                End With

                'データ取得
                dsData = clsDataConnect.pfGet_DataSet(objCmd)
                If sender.ClientID = Me.txtWrkCode.ppTextBox.ClientID Then
                    If dsData.Tables(0).Rows.Count > 0 Then
                        With dsData.Tables(0).Rows(0)
                            Me.txtWrkCode.ppText = .Item("社員コード").ToString()
                            Me.txtWrkUser.ppText = .Item("姓").ToString() & .Item("名").ToString()
                            Me.txtWrkTel.ppText = .Item("TEL").ToString()
                        End With
                    End If
                Else
                    Select Case dsData.Tables(0).Rows.Count
                        Case 0
                            Me.txtWrkUser.psSet_ErrorNo("2013", "作業者名", "社員マスタ")

                        Case 1
                            With dsData.Tables(0).Rows(0)
                                Me.txtWrkCode.ppText = .Item("社員コード").ToString()
                                Me.txtWrkUser.ppText = .Item("姓").ToString() & .Item("名").ToString()
                                Me.txtWrkTel.ppText = .Item("TEL").ToString()
                            End With

                        Case 2
                            With dsData.Tables(0).Rows(0)
                                Me.txtWrkCode.ppText = .Item("社員コード").ToString()
                                Me.txtWrkUser.ppText = .Item("姓").ToString() & .Item("名").ToString()
                                Me.txtWrkUser.psSet_ErrorNo("2014", .Item("社員コード").ToString(), dsData.Tables(0).Rows(1).Item("社員コード").ToString())
                                Me.txtWrkTel.ppText = .Item("TEL").ToString()
                            End With

                        Case Else
                            With dsData.Tables(0).Rows(0)
                                Me.txtWrkCode.ppText = .Item("社員コード").ToString()
                                Me.txtWrkUser.ppText = .Item("姓").ToString() & .Item("名").ToString()
                                Me.txtWrkUser.psSet_ErrorNo("2015")
                                Me.txtWrkTel.ppText = .Item("TEL").ToString()
                            End With

                    End Select
                End If
                Select Case sender.clientid
                    Case Me.txtWrkCode.ppTextBox.ClientID
                        Me.txtWrkUser.ppTextBox.Focus()

                    Case Me.txtWrkUser.ppTextBox.ClientID
                        Me.dtbStartDt.ppDateBox.Focus()

                End Select

                If Me.txtWrkUser.ppText = "" Then
                    Me.txtWrkCode.psSet_ErrorNo("2013", "作業者コード", "社員マスタ")
                End If
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle)
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

    Private Sub Calc_TextChanged(sender As Object, e As EventArgs)
        Dim tsStTime As TimeSpan
        Dim tsEdTime As TimeSpan
        Dim tsMinTime As TimeSpan
        Dim tsMaxTime As TimeSpan
        Dim tsPrice1StTime As TimeSpan
        Dim tsPrice1EdTime As TimeSpan
        Dim tsPrice2StTime As TimeSpan
        Dim tsPrice2EdTime As TimeSpan
        Dim tsCalcTime As TimeSpan
        Dim decPrice1 As Decimal
        Dim decPrice2 As Decimal
        Dim tsTotal1Time As TimeSpan
        Dim tsTotal2Time As TimeSpan
        Dim decData As Decimal

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then

            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
        End If

        Try
            If Me.ddlMntFlg.SelectedIndex = 0 Or Me.txtMntTm.ppTextBox.Text = "" Or Me.txtGbTm.ppTextBox.Text = "" Or
                                Me.tmbStartTm.ppHourBox.Text = "" Or Me.tmbStartTm.ppMinBox.Text = "" Then
                '入力内容に不足がある場合は抜ける
                Exit Sub
            End If

            '--保守対応データの取得
            objCmd = New SqlCommand("ZCMPSEL045", objCn)
            With objCmd.Parameters
                '--パラメータ設定
                '保守管理番号
                .Add(pfSet_Param("mnt_rate_cls", SqlDbType.NVarChar, Me.ddlMntFlg.SelectedValue))
            End With

            'データ取得
            objDs = clsDataConnect.pfGet_DataSet(objCmd)

            With objDs.Tables(0).Rows(0)
                '計算用のデータを設定する
                If .Item("M96_MNT_START_TM1").ToString.Trim <> "" Then
                    tsPrice1StTime = New TimeSpan(Integer.Parse(.Item("M96_MNT_START_TM1").ToString.Split(":")(0)), Integer.Parse(.Item("M96_MNT_START_TM1").ToString.Split(":")(1)), 0)
                    tsPrice1EdTime = New TimeSpan(Integer.Parse(.Item("M96_MNT_END_TM1").ToString.Split(":")(0)), Integer.Parse(.Item("M96_MNT_END_TM1").ToString.Split(":")(1)), 0)
                Else
                    tsPrice1StTime = New TimeSpan(-1, 0, 0)
                    tsPrice1EdTime = New TimeSpan(-1, 0, 0)
                End If
                If .Item("M96_MNT_START_TM2").ToString.Trim <> "" Then
                    tsPrice2StTime = New TimeSpan(Integer.Parse(.Item("M96_MNT_START_TM2").ToString.Split(":")(0)), Integer.Parse(.Item("M96_MNT_START_TM2").ToString.Split(":")(1)), 0)
                    tsPrice2EdTime = New TimeSpan(Integer.Parse(.Item("M96_MNT_END_TM2").ToString.Split(":")(0)), Integer.Parse(.Item("M96_MNT_END_TM2").ToString.Split(":")(1)), 0)
                Else
                    tsPrice2StTime = New TimeSpan(-1, 0, 0)
                    tsPrice2EdTime = New TimeSpan(-1, 0, 0)
                End If

                tsStTime = New TimeSpan(Integer.Parse(Me.tmbStartTm.ppHourText), Integer.Parse(Me.tmbStartTm.ppMinText), 0)
                tsStTime = tsStTime - New TimeSpan(0, 0, (Integer.Parse(Decimal.Parse(Me.txtGbTm.ppText) * 30)))
                tsEdTime = New TimeSpan(Integer.Parse(Me.tmbStartTm.ppHourText), Integer.Parse(Me.tmbStartTm.ppMinText), 0)
                tsEdTime = tsEdTime + New TimeSpan(0, Integer.Parse(Me.txtMntTm.ppText), 0)
                tsEdTime = tsEdTime + New TimeSpan(0, 0, (Integer.Parse(Decimal.Parse(Me.txtGbTm.ppText) * 30)))

                decData = 0
                tsCalcTime = tsStTime
                decPrice1 = Decimal.Parse(.Item("M96_MNT_PRICE1").ToString)
                decPrice2 = Decimal.Parse(.Item("M96_MNT_PRICE2").ToString)
                tsMinTime = New TimeSpan(0, 0, 0)
                tsMaxTime = New TimeSpan(23, 59, 0)
                tsTotal1Time = New TimeSpan(0, 0, 0)
                tsTotal2Time = New TimeSpan(0, 0, 0)
                If tsCalcTime < New TimeSpan(0, 0, 0) Then
                    tsCalcTime = tsCalcTime + New TimeSpan(24, 0, 0)
                    tsStTime = tsStTime + New TimeSpan(24, 0, 0)
                    tsEdTime = tsEdTime + New TimeSpan(24, 0, 0)
                End If
                '計算
                Do While 1
                    If tsPrice1StTime = New TimeSpan(-1, 0, 0) And tsPrice2StTime = New TimeSpan(-1, 0, 0) Then
                        Exit Do
                    End If

                    If tsPrice1StTime > tsMinTime Then
                        If tsPrice1StTime < tsPrice1EdTime Then
                            If tsCalcTime >= tsPrice1StTime And tsCalcTime < tsPrice1EdTime Then
                                If tsEdTime < tsPrice1EdTime Then
                                    tsTotal1Time = tsTotal1Time + (tsEdTime - tsCalcTime)
                                    Exit Do
                                Else
                                    tsTotal1Time = tsTotal1Time + (tsPrice1EdTime - tsCalcTime)
                                    tsCalcTime = tsPrice1EdTime
                                End If
                            End If
                        Else
                            If tsCalcTime >= tsMinTime And tsCalcTime < tsPrice1EdTime Then
                                If tsEdTime < tsPrice1EdTime Then
                                    tsTotal1Time = tsTotal1Time + (tsEdTime - tsCalcTime)
                                    Exit Do
                                Else
                                    tsTotal1Time = tsTotal1Time + (tsPrice1EdTime - tsCalcTime)
                                    tsCalcTime = tsPrice1EdTime
                                End If
                            ElseIf tsCalcTime >= tsPrice1StTime And tsCalcTime <= tsMaxTime Then
                                If tsEdTime <= tsMaxTime Then
                                    tsTotal1Time = tsTotal1Time + (tsEdTime - tsCalcTime)
                                    Exit Do
                                Else
                                    tsTotal1Time = tsTotal1Time + (tsMaxTime - tsCalcTime)
                                    tsCalcTime = tsMaxTime
                                End If
                            End If
                        End If

                        '日数が増加したら価格用の時間も日を進める
                        If tsCalcTime.Days > tsPrice1StTime.Days Then
                            tsPrice1StTime = tsPrice1StTime + New TimeSpan(24, 0, 0)
                            tsPrice1EdTime = tsPrice1EdTime + New TimeSpan(24, 0, 0)
                        End If

                    ElseIf tsCalcTime < tsPrice2StTime And tsCalcTime >= tsPrice2EdTime Then
                        tsCalcTime = tsPrice2StTime
                    End If

                    '終了時間が過ぎてたら終了
                    If tsEdTime <= tsCalcTime Then
                        Exit Do
                    End If

                    If tsPrice2StTime > tsMinTime Then
                        If tsPrice2StTime < tsPrice2EdTime Then
                            If tsCalcTime >= tsPrice2StTime And tsCalcTime < tsPrice2EdTime Then
                                If tsEdTime < tsPrice2EdTime Then
                                    tsTotal2Time = tsTotal2Time + (tsEdTime - tsCalcTime)
                                    Exit Do
                                Else
                                    tsTotal2Time = tsTotal2Time + (tsPrice2EdTime - tsCalcTime)
                                    tsCalcTime = tsPrice2EdTime
                                End If
                            End If
                        Else
                            If tsCalcTime >= tsMinTime And tsCalcTime < tsPrice2EdTime Then
                                If tsEdTime < tsPrice2EdTime Then
                                    tsTotal2Time = tsTotal2Time + (tsEdTime - tsCalcTime)
                                    Exit Do
                                Else
                                    tsTotal2Time = tsTotal2Time + (tsPrice2EdTime - tsCalcTime)
                                    tsCalcTime = tsPrice2EdTime
                                End If
                            ElseIf tsCalcTime >= tsPrice2StTime And tsCalcTime <= tsMaxTime Then
                                If tsEdTime <= tsMaxTime Then
                                    tsTotal2Time = tsTotal2Time + (tsEdTime - tsCalcTime)
                                    Exit Do
                                Else
                                    tsTotal2Time = tsTotal2Time + (tsMaxTime - tsCalcTime)
                                    tsCalcTime = tsMaxTime
                                End If
                            End If
                        End If

                        '日数が増加したら価格用の時間も日を進める
                        If (tsCalcTime + New TimeSpan(0, 1, 0)).Days > tsPrice2StTime.Days Then
                            tsPrice2StTime = tsPrice2StTime + New TimeSpan(24, 0, 0)
                            tsPrice2EdTime = tsPrice2EdTime + New TimeSpan(24, 0, 0)
                        End If
                    ElseIf tsCalcTime < tsPrice1StTime And tsCalcTime >= tsPrice1EdTime Then
                        tsCalcTime = tsPrice1StTime
                    End If

                    '日数が増加したら境界用の時間も日を進める
                    If tsCalcTime.Days > tsMaxTime.Days Then
                        tsMinTime = tsMinTime + New TimeSpan(24, 0, 0)
                        tsMaxTime = tsMaxTime + New TimeSpan(24, 0, 0)
                    End If

                    '終了時間が過ぎてたら終了
                    If tsEdTime <= tsCalcTime Then
                        Exit Do
                    End If
                Loop
            End With

            If Decimal.TryParse(txtPsnNum.ppText, 0) = True Then
                Me.txtReqPrice.ppText = Math.Floor((((tsTotal1Time.TotalMinutes / 60) * decPrice1) + ((tsTotal2Time.TotalMinutes / 60) * decPrice2)) * Decimal.Parse(txtPsnNum.ppText)).ToString
            End If
        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "特別保守料金情報取得")
        Finally
            Select Case sender.clientid
                Case Me.ddlMntFlg.ClientID : Me.tmbDeptTm.ppHourBox.Focus()
                Case Me.tmbStartTm.ppHourBox.ClientID : Me.tmbStartTm.ppMinBox.Focus()
                Case Me.tmbStartTm.ppMinBox.ClientID : Me.tmbEndTm.ppHourBox.Focus()
                Case Me.ddlWrk.ClientID : Me.txtMntTm.ppTextBox.Focus()
                Case Me.txtMntTm.ppTextBox.ClientID : Me.txtGbTm.ppTextBox.Focus()
                Case Me.txtGbTm.ppTextBox.ClientID : Me.txtPsnNum.ppTextBox.Focus()
                Case Me.txtPsnNum.ppTextBox.ClientID : Me.txtReqPrice.ppTextBox.Focus()
            End Select

            'DB切断
            If Not clsDataConnect.pfClose_Database(objCn) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End Try


    End Sub

    ''' <summary>
    ''' 保守機ＴＢＯＸマスタ　該当ＴＢＯＸＩＤ存在チェック
    ''' </summary>
    ''' <param name="opblnResult">TBOXID</param>
    ''' <remarks></remarks>
    Private Sub sGetMB1Info(ByRef opblnResult As Boolean)

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

        opblnResult = False

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        Else
            Try
                objCmd = New SqlCommand("CMPSELP001_S8", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '保守管理番号
                    .Add(pfSet_Param("prmTBOXID", SqlDbType.NVarChar, Me.txtTBoxid.ppText))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables.Count > 0 Then
                    If objDs.Tables(0).Rows.Count > 0 Then
                        opblnResult = True
                    End If
                End If
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "保守機ＴＢＯＸマスタ取得")
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
                'データセット　破棄
                Call psDisposeDataSet(objDs)
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        End If

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs)

        Dim strKeyList As List(Of String) = Nothing '画面引継ぎ用キー情報

        '開始ログ出力
        psLogStart(Me)

        '画面引継ぎ用キー情報設定
        strKeyList = New List(Of String)
        strKeyList.Add(Me.lblMntNo.Text)    '保守管理番号
        strKeyList.Add(Me.txtTBoxid.ppText) 'ＴＢＯＸＩＤ

        'セッション情報設定																	
        Session(P_SESSION_BCLIST) = Master.ppBcList_Text
        Session(P_SESSION_OLDDISP) = M_MY_DISP_ID
        Session(P_KEY) = strKeyList.ToArray
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)
        Session(P_SESSION_TERMS) = ViewState(P_SESSION_TERMS)

        '修理依頼書画面起動
        psOpen_Window(Me, M_MC_DISP_PATH)

        '終了ログ出力
        psLogEnd(Me)
    End Sub
End Class
