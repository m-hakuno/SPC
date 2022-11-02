'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜監視＞
'*　処理名　　：　監視報告書兼依頼票　参照／更新
'*　ＰＧＭＩＤ：　WATUPDP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.07　：　ＸＸＸ
'*  変　更　　：　2017.06.26　：　伯野
'********************************************************************************************************************************
'WATUPDP001-001 案件終了となった案件をＮＧＣ側で編集不可とする データ取得時に案件終了フラグを取得して編集可能判定に使用
'WATUPDP001-002 削除された案件は編集不可とする
'               印刷時にページ検証の判定を追加 
'WATUPDP001-003 案件を削除する場合は、進捗状況を受領済み、案件終了フラグを終了にして登録する


#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive      '排他制御用

#End Region

Public Class WATUPDP001
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
    Const M_MY_DISP_ID = P_FUN_WAT & P_SCR_UPD & P_PAGE & "001"

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
    Dim strAddr As String = ConfigurationManager.AppSettings("Address")
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

    Dim mstrMntrEndFlg As String = "0" 'WATUPDP001-001
    Dim mstrDelFlg As String = "0" 'WATUPDP001-002

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page_Loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'ボタンアクションの設定
        AddHandler btnUpdate.Click, AddressOf btnUpdate_Click
        AddHandler btnAdd.Click, AddressOf btnAdd_Click
        AddHandler btnPrint.Click, AddressOf btnPrint_Click

        '監視報告書兼依頼票票情報取得設定
        AddHandler Me.txtTboxId.ppTextBox.TextChanged, AddressOf txtTboxId_TextChanged
        Me.txtTboxId.ppTextBox.AutoPostBack = True

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            If Not IsPostBack Then  '初回表示のみ
                '画面設定
                Master.ppProgramID = M_MY_DISP_ID
                Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)
                Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる

                'パンくずリスト設定
                Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

                'セッション変数「グループナンバー」「遷移条件」が存在しない場合、画面を閉じる
                If Session(P_SESSION_GROUP_NUM) Is Nothing Or Session(P_SESSION_TERMS) Is Nothing Then
                    psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    '--------------------------------
                    '2014/06/11 後藤　ここから
                    '--------------------------------
                    '排他削除
                    clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                    '--------------------------------
                    '2014/06/11 後藤　ここまで
                    '--------------------------------
                    psClose_Window(Me)
                    Return
                End If

                '排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
                If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then

                    Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)

                End If

                'ViewStateに「グループナンバー」「遷移条件」「遷移元画面ＩＤ」「キー情報」を保存
                ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)
                ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)
                ViewState(P_SESSION_OLDDISP) = Session(P_SESSION_OLDDISP)
                ViewState(P_KEY) = Session(P_KEY)

                '状況ドロップダウンリスト設定
                msSet_ddlStatus()           '進捗状況
                msSet_ddlDealStatus()       'ＮＧＣ対応状況

                '画面クリア
                msClear_Screen()

                '活性／非活性設定
                msSet_Mode(ViewState(P_SESSION_TERMS))

                'データ取得
                '--------------------------------
                '2014/04/15 高松　ここから
                '--------------------------------
                'If ViewState(P_SESSION_OLDDISP) = "WATLSTP001" Then
                '    If ViewState(P_SESSION_TERMS) <>  ClsComVer.E_遷移条件.登録 Then
                '        msSet_WATData()
                '    End If
                'Else
                '    msSet_OVEData()

                'End If
                If ViewState(P_SESSION_OLDDISP) = "WATLSTP001" Then
                    If ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.登録 Then
                        msSet_WATData()
                    End If
                ElseIf ViewState(P_SESSION_OLDDISP) = "SLFLSTP002" Then
                    msSet_SLFData()
                Else
                    msSet_OVEData()
                End If
                '--------------------------------
                '2014/04/15 高松　ここまで
                '--------------------------------

            Else
                'ViewState「グループナンバー」が存在しない場合、画面を閉じる
                If ViewState(P_SESSION_GROUP_NUM) Is Nothing Then
                    psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    '--------------------------------
                    '2014/06/11 後藤　ここから
                    '--------------------------------
                    '排他削除
                    clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                    '--------------------------------
                    '2014/06/11 後藤　ここまで
                    '--------------------------------
                    psClose_Window(Me)
                    Return
                End If
                'WATUPDP001-002 START
                'ポストバック後の判断基準を更新
                If ddlStatus.SelectedValue = "02" Then
                    mstrMntrEndFlg = "1"
                Else
                    mstrMntrEndFlg = "0"
                End If
                If cbxDeleteFlg.Checked = True Then
                    mstrDelFlg = "1"
                Else
                    mstrDelFlg = "0"
                End If
                'WATUPDP001-002 END
            End If
        Catch ex As Exception
            '画面初期化エラー
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画前)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            '--------------------------------
            '2014/06/11 後藤　ここから
            '--------------------------------
            '排他削除
            clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
            '--------------------------------
            '2014/06/11 後藤　ここまで
            '--------------------------------
            psClose_Window(Me)
            Return
        End Try
    End Sub

    '---------------------------
    '2014/04/18 武 ここから
    '---------------------------
    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照 Then
            btnUpdate.Visible = False
            btnAdd.Visible = False
        End If

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
                txtNgcDeal.ppEnabled = False
                txtAnswer.ppEnabled = False
                ddlStatus.Enabled = True
                'WATUPDP001-002 START
                If mstrMntrEndFlg = "1" Then
                    dttOBreakD.ppEnabled = False
                    txtTboxId.ppEnabled = False
                    txtSpcCharge.ppEnabled = False
                    cbxDeleteFlg.Enabled = False
                    '                    ddlStatus.Enabled = False
                    ddlHpnStts.ppEnabled = False
                    txtContent.ppEnabled = False
                Else
                    dttOBreakD.ppEnabled = True
                    txtTboxId.ppEnabled = True
                    txtSpcCharge.ppEnabled = True
                    cbxDeleteFlg.Enabled = True
                    '                    ddlStatus.Enabled = true
                    ddlHpnStts.ppEnabled = True
                    txtContent.ppEnabled = True
                End If
                'WATUPDP001-002 END
            Case "SPC"
                txtNgcDeal.ppEnabled = False
                txtAnswer.ppEnabled = False
                'WATUPDP001-002 START
                If mstrMntrEndFlg = "1" Then
                    pnlSPC.Enabled = False
                Else
                    pnlSPC.Enabled = True
                End If
                'WATUPDP001-002 END
            Case "営業所"
            Case "NGC"
                'WATUPDP001-001 START
                If mstrMntrEndFlg = "1" Then
                    pnlNGC.Enabled = False
                    btnUpdate.Visible = False
                    btnAdd.Visible = False
                End If
                'WATUPDP001-001 END
        End Select

        'WATUPDP001-002 START
        If mstrDelFlg = "1" Then
            pnlSPC.Enabled = False
            pnlNGC.Enabled = False
            btnUpdate.Visible = False
            btnAdd.Visible = False
            btnPrint.Visible = False
            btnUpdate.Visible = False
        End If
        'WATUPDP001-002 END

    End Sub
    '---------------------------
    '2014/04/18 武 ここまで
    '---------------------------

    ''' <summary>
    ''' 更新処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs)
        '開始ログ出力
        psLogStart(Me)

        If (Page.IsValid) Then
            Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
            Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
            Dim dstOrders As DataSet = Nothing          'データセット
            Dim strAddrFlg As String = Nothing          '作業拠点判別フラグ
            Dim intRtn As Integer
            Dim strDeleteFlg As String
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            objStack = New StackFrame
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------

            '場所指定
            If strAddr = P_ADD_NGC Then
                strAddrFlg = "1"
            Else
                strAddrFlg = "0"
            End If

            '値取得
            Select Case Me.cbxDeleteFlg.Checked
                Case False
                    strDeleteFlg = "0"
                Case Else
                    strDeleteFlg = "1"
            End Select

            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    cmdDB = New SqlCommand(M_MY_DISP_ID + "_U1", conDB)
                    'パラメータ設定
                    With cmdDB.Parameters
                        .Add(pfSet_Param("mntrreport_no", SqlDbType.NVarChar, Me.lblMntrReportNo2.Text))            '管理番号
                        .Add(pfSet_Param("obreak_d", SqlDbType.NVarChar, Me.dttOBreakD.ppText))                     '発生日
                        .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxId.ppText))                        'ＴＢＯＸＩＤ
                        .Add(pfSet_Param("hall_nm", SqlDbType.NVarChar, Me.lblHallNm2.Text))                        'ホール名
                        .Add(pfSet_Param("spc_charge", SqlDbType.NVarChar, Me.txtSpcCharge.ppText))                 '報告者
                        .Add(pfSet_Param("delete_flg", SqlDbType.NVarChar, strDeleteFlg))                           '削除
                        .Add(pfSet_Param("status_cd", SqlDbType.NVarChar, Me.ddlStatus.SelectedValue))              '進捗状況
                        .Add(pfSet_Param("hpnstts_cd", SqlDbType.NVarChar, Me.ddlHpnStts.ppSelectedValue))          '発生状況
                        .Add(pfSet_Param("content", SqlDbType.NVarChar, Me.txtContent.ppText))                      '報告内容
                        .Add(pfSet_Param("ngc_deal", SqlDbType.NVarChar, Me.txtNgcDeal.ppText))                     '対応者
                        .Add(pfSet_Param("deal_status_cd", SqlDbType.NVarChar, Me.ddlDealStatus.SelectedValue))     '対応状況
                        .Add(pfSet_Param("answer", SqlDbType.NVarChar, Me.txtAnswer.ppText))                        '対応内容
                        .Add(pfSet_Param("addrflg", SqlDbType.NVarChar, strAddrFlg))                                '拠点判別キー
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                         'ユーザーＩＤ
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                           '戻り値

                    End With

                    'データ更新
                    Using conTrn = conDB.BeginTransaction
                        cmdDB.Transaction = conTrn

                        'コマンドタイプ設定(ストアド)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, "00007", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, intRtn.ToString)
                            Exit Sub
                        End If

                        'コミット
                        conTrn.Commit()

                        If strDeleteFlg <> 0 Then
                            '参照画面に変更
                            ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
                            'WATUPDP001-003
                            Me.ddlStatus.SelectedValue = "02"
                            'WATUPDP001-003
                        End If

                    End Using

                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "監視報告書兼依頼書")

                    '活性／非活性設定
                    msSet_Mode(ViewState(P_SESSION_TERMS))

                Catch ex As Exception
                    'データ更新エラー
                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "監視報告書兼依頼書")
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
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 登録処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs)
        '開始ログ出力
        psLogStart(Me)

        If (Page.IsValid) Then
            Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
            Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
            Dim dstOrders As DataSet = Nothing          'データセット
            Dim intRtn As Integer
            Dim strDeleteFlg As String
            Dim strKey() As String = ViewState(P_KEY)
            Dim strCtrlNo As String
            Dim strSeq As String
            Dim strSeqKey As String
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            objStack = New StackFrame
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------

            '値取得
            Select Case Me.cbxDeleteFlg.Checked
                Case False
                    strDeleteFlg = "0"
                Case Else
                    strDeleteFlg = "1"
            End Select


            If ViewState(P_SESSION_OLDDISP) = "SLFLSTP002" Then
                strCtrlNo = strKey(2)
                strSeq = strKey(6)
                strSeqKey = String.Empty
            ElseIf ViewState(P_SESSION_OLDDISP) = "OVELSTP002" Then
                strCtrlNo = strKey(2)
                strSeq = strKey(3)
                strSeqKey = String.Empty
            Else
                strCtrlNo = String.Empty
                strSeq = String.Empty
                strSeqKey = String.Empty
            End If


            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                '監視報告書兼依頼書登録処理
                Try
                    cmdDB = New SqlCommand(M_MY_DISP_ID + "_I1", conDB)
                    'パラメータ設定
                    With cmdDB.Parameters
                        .Add(pfSet_Param("obreak_d", SqlDbType.NVarChar, Me.dttOBreakD.ppText))                     '発生日
                        .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxId.ppText))                        'ＴＢＯＸＩＤ
                        .Add(pfSet_Param("hall_nm", SqlDbType.NVarChar, Me.lblHallNm2.Text))                        'ホール名
                        .Add(pfSet_Param("spc_charge", SqlDbType.NVarChar, Me.txtSpcCharge.ppText))                 '報告者
                        .Add(pfSet_Param("delete_flg", SqlDbType.NVarChar, strDeleteFlg))                           '削除
                        .Add(pfSet_Param("status_cd", SqlDbType.NVarChar, Me.ddlStatus.SelectedValue))              '進捗状況
                        .Add(pfSet_Param("hpnstts_cd", SqlDbType.NVarChar, Me.ddlHpnStts.ppSelectedValue))          '発生状況
                        .Add(pfSet_Param("content", SqlDbType.NVarChar, Me.txtContent.ppText))                      '報告内容
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                         'ユーザーＩＤ
                        .Add(pfSet_Param("ctrl_no", SqlDbType.NVarChar, strCtrlNo))                                 '元管理番号
                        .Add(pfSet_Param("seq", SqlDbType.NVarChar, strSeq))                                        '元連番（ＪＢＮＯ）
                        .Add(pfSet_Param("seq_key", SqlDbType.NVarChar, strSeqKey))                                 '元管理連番
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                           '戻り値

                    End With

                    'データ登録
                    Using conTrn = conDB.BeginTransaction
                        cmdDB.Transaction = conTrn

                        dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, "00008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, intRtn.ToString)
                            Exit Sub
                        End If

                        '管理番号を設定
                        Me.lblMntrReportNo2.Text = dstOrders.Tables(0).Rows(0).Item("管理番号").ToString

                        'If ViewState(P_SESSION_OLDDISP) <> "WATLSTP001" Then
                        If ViewState(P_SESSION_OLDDISP) = "OVELSTP002" Then

                            '運用時間外仕様情報更新処理
                            Try
                                cmdDB = New SqlCommand(M_MY_DISP_ID + "_U2", conDB)
                                cmdDB.Transaction = conTrn
                                'パラメータ設定
                                With cmdDB.Parameters
                                    .Add(pfSet_Param("ctrl_no", SqlDbType.NVarChar, strKey(2).ToString))                    '管理番号
                                    .Add(pfSet_Param("seq", SqlDbType.NVarChar, strKey(3)))                                 '連番
                                    .Add(pfSet_Param("nl_cls", SqlDbType.NVarChar, strKey(5)))                              'ＮＬ区分
                                    .Add(pfSet_Param("id_ic_cls", SqlDbType.NVarChar, strKey(4)))                           'ＩＤ／ＩＣ区分
                                    .Add(pfSet_Param("recvdatetime", SqlDbType.NVarChar, strKey(6)))                        '検知日時
                                    .Add(pfSet_Param("recvseq", SqlDbType.NVarChar, strKey(7)))                             '受信連番
                                    .Add(pfSet_Param("new_ctrl_no", SqlDbType.NVarChar, Me.lblMntrReportNo2.Text))          '新管理番号
                                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxId.ppText))                    'ＴＢＯＸＩＤ
                                    .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                     'ユーザーＩＤ
                                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                       '戻り値

                                End With

                                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                                'ストアド戻り値チェック
                                intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                                If intRtn <> 0 Then
                                    psMesBox(Me, "00008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, intRtn.ToString)
                                    Exit Sub
                                End If

                                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "運用時間外使用状況")

                            Catch ex As Exception
                                'データ登録エラー
                                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "運用時間外使用状況")
                                '--------------------------------
                                '2014/04/14 星野　ここから
                                '--------------------------------
                                'ログ出力
                                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                                '--------------------------------
                                '2014/04/14 星野　ここまで
                                '--------------------------------
                                Exit Sub
                            End Try
                            '--------------------------------
                            '2014/04/15 高松　ここから
                            '--------------------------------
                        ElseIf ViewState(P_SESSION_OLDDISP) = "SLFLSTP002" Then

                            '運用時間外仕様情報更新処理
                            Try
                                cmdDB = New SqlCommand(M_MY_DISP_ID + "_U3", conDB)
                                cmdDB.Transaction = conTrn
                                'パラメータ設定
                                With cmdDB.Parameters
                                    .Add(pfSet_Param("ctrl_no", SqlDbType.NVarChar, strKey(2).ToString))                    '管理番号
                                    .Add(pfSet_Param("seq", SqlDbType.NVarChar, strKey(3)))                                 '連番
                                    .Add(pfSet_Param("nl_cls", SqlDbType.NVarChar, strKey(5)))                              'ＮＬ区分
                                    .Add(pfSet_Param("id_ic_cls", SqlDbType.NVarChar, strKey(4)))                           'ＩＤ／ＩＣ区分
                                    .Add(pfSet_Param("jb_num", SqlDbType.NVarChar, strKey(6)))                              'ＪＢ番号
                                    .Add(pfSet_Param("new_ctrl_no", SqlDbType.NVarChar, Me.lblMntrReportNo2.Text))          '新管理番号
                                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxId.ppText))                    'ＴＢＯＸＩＤ
                                    .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                     'ユーザーＩＤ
                                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                       '戻り値
                                End With

                                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                                'ストアド戻り値チェック
                                intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                                If intRtn <> 0 Then
                                    psMesBox(Me, "00008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, intRtn.ToString)
                                    Exit Sub
                                End If

                                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "自走中情報")

                            Catch ex As Exception
                                'データ登録エラー
                                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "自走中情報")

                                'ログ出力
                                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                                objStack.GetMethod.Name, "", ex.ToString, "Catch")

                                Exit Sub
                            End Try

                            '--------------------------------
                            '2014/04/15 高松　ここまで
                            '--------------------------------
                        End If
                        'コミット
                        conTrn.Commit()


                        '排他制御用変数
                        Dim strExclusiveDate As String = Nothing
                        Dim arTable_Name As New ArrayList
                        Dim arKey As New ArrayList

                        'ロック対象テーブル名の登録
                        arTable_Name.Insert(0, "D36_MNTRREPORT")

                        'ロックテーブルキー項目の登録
                        arKey.Insert(0, Me.lblMntrReportNo2.Text)

                        '排他情報確認処理
                        If clsExc.pfSel_Exclusive(strExclusiveDate,
                                           Me,
                                           Session(P_SESSION_IP),
                                           Session(P_SESSION_PLACE),
                                           Session(P_SESSION_USERID),
                                           Session(P_SESSION_SESSTION_ID),
                                           ViewState(P_SESSION_GROUP_NUM),
                                           M_MY_DISP_ID,
                                           arTable_Name,
                                           arKey) = 0 Then

                            'セッション情報設定
                            Me.Master.ppExclusiveDate = strExclusiveDate

                        Else
                            '排他ロック中
                            Exit Sub

                        End If
                    End Using

                    If strDeleteFlg <> 0 Then
                        '参照画面に変更
                        ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
                    Else
                        '更新画面に変更
                        ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
                        Me.btnUpdate.Visible = True
                    End If
                    Me.btnPrint.Enabled = True

                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "監視報告書兼依頼書（" + Me.lblMntrReportNo2.Text + "）")

                    '活性／非活性設定
                    msSet_Mode(ViewState(P_SESSION_TERMS))

                Catch ex As Exception
                    'データ登録エラー
                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "監視報告書兼依頼書")
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
        End If

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
        Dim objRpt As WATREP005
        Dim strFNm As String
        Dim strKey As String = Nothing          '監視報告書兼依頼票情報キー
        Dim strSql As StringBuilder

        'WATUPDP001-002 START
        If (Page.IsValid) Then

            'ログ出力開始
            psLogStart(Me)

            strKey = Me.lblMntrReportNo2.Text
            strFNm = "監視報告書兼依頼票"
            strSql = New StringBuilder
            strSql.Append("EXEC WATUPDP001_S3 '")
            strSql.Append(strKey)
            strSql.Append("'")
            objRpt = New WATREP005

            psPrintPDF(Me, objRpt, strSql.ToString, strFNm)

            'ログ出力終了
            psLogEnd(Me)
        End If
        'WATUPDP001-002 END


    End Sub

    ''' <summary>
    ''' ＴＢＯＸＩＤロストフォーカス時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtTboxId_TextChanged(sender As Object, e As EventArgs)
        Dim dtsHallNm As DataSet = Nothing

        '開始ログ出力
        psLogStart(Me)

        If mfGet_HallInfo(Me.txtTboxId.ppText, dtsHallNm) Then
            Me.lblHallNm2.Text = dtsHallNm.Tables(0).Rows(0).Item("ホール名").ToString
            Me.dttOBreakD.ppDateBox.Focus()
        Else
            Me.txtTboxId.ppText = String.Empty
            Me.txtTboxId.ppTextBox.Focus()
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClear_Screen()

        Me.lblMntrReportNo2.Text = String.Empty         '管理番号
        Me.dttOBreakD.ppText = String.Empty             '発生日
        Me.txtTboxId.ppText = String.Empty              'ＴＢＯＸＩＤ
        Me.lblHallNm2.Text = String.Empty               'ホール名
        Me.txtSpcCharge.ppText = String.Empty           '報告者
        Me.cbxDeleteFlg.Checked = False                 '削除
        Me.ddlStatus.SelectedValue = String.Empty       '進捗状況
        Me.ddlHpnStts.ppSelectedValue = String.Empty    '発生状況
        Me.txtContent.ppText = String.Empty             '報告内容
        Me.txtNgcDeal.ppText = String.Empty             '対応者
        Me.ddlDealStatus.SelectedValue = String.Empty   '対応状況
        Me.txtAnswer.ppText = String.Empty              '対応内容

    End Sub

    ''' <summary>
    ''' データ取得処理(遷移元:WATLSTP001)
    ''' </summary>
    ''' <param name="ipstrKey">キー情報</param>
    ''' <param name="opdstData">監視報告書兼依頼書項目</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_WATData(ByVal ipstrKey As String, ByRef opdstData As DataSet) As Boolean
        Dim conDB As SqlConnection = Nothing    'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing      'SqlCommandクラス
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGet_WATData = False

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_S1", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("mntrreport_no", SqlDbType.NVarChar, ipstrKey))
                End With

                'データ取得
                opdstData = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ有無確認
                If opdstData.Tables(0).Rows.Count = 0 Then
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "監視報告書兼依頼票")
                    Exit Function
                End If

                mfGet_WATData = True

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "監視報告書兼依頼票")
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

    End Function

    ''' <summary>
    ''' データ設定処理(遷移元:WATLSTP001)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_WATData()
        Dim dtsData As DataSet = Nothing
        Dim dtRow As DataRow = Nothing              'DataRow
        Dim strKey() As String = Nothing

        'キー項目取得
        strKey = ViewState(P_KEY)

        If mfGet_WATData(strKey(0).ToString, dtsData) Then

            dtRow = dtsData.Tables(0).Rows(0)

            '監視報告書兼依頼票情報設定
            Me.lblMntrReportNo2.Text = strKey(0).ToString
            Me.dttOBreakD.ppText = dtRow("発生日").ToString
            Me.txtTboxId.ppText = dtRow("ＴＢＯＸＩＤ").ToString
            Me.lblHallNm2.Text = dtRow("ホール名").ToString
            Me.txtSpcCharge.ppText = dtRow("報告者").ToString
            If dtRow("削除フラグ").ToString <> "0" Then
                Me.cbxDeleteFlg.Checked = True
            End If
            'WATUPDP001-002 START
            If dtRow("削除フラグ") Is DBNull.Value Then
            Else
                mstrDelFlg = dtRow("削除フラグ").ToString
            End If
            'WATUPDP001-002 END
            Me.ddlStatus.SelectedValue = dtRow("進捗状況").ToString
            '            If dtRow("進捗状況").ToString = "02" Then
            '                Me.ddlStatus.Enabled = False
            '            Else
            '                Me.ddlStatus.Enabled = True
            '            End If
            Me.ddlHpnStts.ppSelectedValue = dtRow("発生状況").ToString
            Me.txtContent.ppText = dtRow("報告内容").ToString
            Me.txtNgcDeal.ppText = dtRow("対応者").ToString
            Me.ddlDealStatus.SelectedValue = dtRow("対応状況").ToString
            Me.txtAnswer.ppText = dtRow("対応内容").ToString
            mstrMntrEndFlg = dtRow("案件終了").ToString 'WATUPDP001-001

        End If

    End Sub

    ''' <summary>
    ''' データ取得処理(遷移元:OVELSTP002)
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfGet_OVEData(ByVal ipstrKey1 As String _
                                 , ByVal ipstrKey2 As String _
                                 , ByVal ipstrKey3 As String _
                                 , ByVal ipstrKey4 As String _
                                 , ByVal ipstrKey5 As String _
                                 , ByVal ipstrKey6 As String _
                                 , ByVal ipstrKey7 As String _
                                 , ByVal ipstrKey8 As String _
                                 , ByVal ipstrKey9 As String _
                                 , ByVal ipstrKey10 As String _
                                 , ByVal ipstrKey11 As String _
                                 , ByVal ipstrKey12 As String _
                                 , ByRef opdstData As DataSet) As Boolean
        Dim conDB As SqlConnection = Nothing    'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing      'SqlCommandクラス
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGet_OVEData = False

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Select Case ipstrKey2
                Case "2"
                    Try
                        cmdDB = New SqlCommand(M_MY_DISP_ID + "_S1", conDB)
                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("mntrreport_no", SqlDbType.NVarChar, ipstrKey9))
                        End With

                        'データ取得
                        opdstData = clsDataConnect.pfGet_DataSet(cmdDB)

                        'データ有無確認
                        If opdstData.Tables(0).Rows.Count = 0 Then
                            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "監視報告書兼依頼票")
                            Exit Function
                        End If

                        mfGet_OVEData = True

                    Catch ex As Exception
                        'データ取得エラー
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "監視報告書兼依頼票")
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

                Case "1"
                    Try
                        cmdDB = New SqlCommand("ZCMPSEL028", conDB)
                        'パラメータ設定
                        With cmdDB.Parameters
                            'ＴＢＯＸＩＤ
                            .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, ipstrKey10))
                        End With

                        'データ取得
                        opdstData = clsDataConnect.pfGet_DataSet(cmdDB)

                        'データ有無確認
                        If opdstData.Tables(0).Rows.Count = 0 Then
                            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "ホール情報")
                            Me.txtTboxId.ppTextBox.Focus()
                            Exit Function
                        End If

                        mfGet_OVEData = True

                    Catch ex As Exception
                        'データ取得エラー
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ホール情報")
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
            End Select
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Function

    ''' <summary>
    ''' データ設定処理(遷移元:OVELSTP002)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_OVEData()
        Dim dtsData As DataSet = Nothing
        Dim dtRow As DataRow = Nothing              'DataRow
        Dim strKey() As String = Nothing

        strKey = ViewState(P_KEY)

        If mfGet_OVEData(strKey(0) _
                       , strKey(1) _
                       , strKey(2) _
                       , strKey(3) _
                       , strKey(4) _
                       , strKey(5) _
                       , strKey(6) _
                       , strKey(7) _
                       , strKey(8) _
                       , strKey(9) _
                       , strKey(10) _
                       , strKey(11) _
                       , dtsData) Then
            Select Case strKey(1)
                Case "2"
                    dtRow = dtsData.Tables(0).Rows(0)

                    '監視報告書兼依頼票情報設定
                    Me.lblMntrReportNo2.Text = strKey(8).ToString
                    Me.dttOBreakD.ppText = dtRow("発生日").ToString
                    Me.txtTboxId.ppText = dtRow("ＴＢＯＸＩＤ").ToString
                    Me.lblHallNm2.Text = dtRow("ホール名").ToString
                    Me.txtSpcCharge.ppText = dtRow("報告者").ToString
                    If dtRow("削除フラグ").ToString <> "0" Then
                        Me.cbxDeleteFlg.Checked = True
                    End If
                    Me.ddlStatus.SelectedValue = dtRow("進捗状況").ToString
                    If dtRow("進捗状況").ToString = "02" Then
                        Me.ddlStatus.Enabled = False
                    Else
                        Me.ddlStatus.Enabled = True
                    End If
                    Me.ddlHpnStts.ppSelectedValue = dtRow("発生状況").ToString
                    Me.txtContent.ppText = dtRow("報告内容").ToString
                    Me.txtNgcDeal.ppText = dtRow("対応者").ToString
                    Me.ddlDealStatus.SelectedValue = dtRow("対応状況").ToString
                    Me.txtAnswer.ppText = dtRow("対応内容").ToString
                    mstrMntrEndFlg = dtRow("案件終了").ToString 'WATUPDP001-001

                Case "1"
                    Me.txtTboxId.ppText = strKey(9).ToString
                    Me.lblHallNm2.Text = dtsData.Tables(0).Rows(0).Item("ホール名").ToString
                    Me.txtSpcCharge.ppText = strKey(10).ToString
                    Me.txtContent.ppText = strKey(11).ToString

                    Me.dttOBreakD.ppDateBox.Focus()
            End Select

        End If
    End Sub

    '--------------------------------
    '2014/04/15 高松　ここから
    '--------------------------------
    ''' <summary>
    ''' データ取得処理(遷移元:SLFLSTP002)
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfGet_SLFData(ByVal ipstrKey1 As String _
                                 , ByVal ipstrKey2 As String _
                                 , ByVal ipstrKey3 As String _
                                 , ByVal ipstrKey4 As String _
                                 , ByVal ipstrKey5 As String _
                                 , ByVal ipstrKey6 As String _
                                 , ByVal ipstrKey7 As String _
                                 , ByVal ipstrKey8 As String _
                                 , ByVal ipstrKey9 As String _
                                 , ByVal ipstrKey10 As String _
                                 , ByRef opdstData As DataSet) As Boolean
        Dim conDB As SqlConnection = Nothing    'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing      'SqlCommandクラス

        objStack = New StackFrame

        mfGet_SLFData = False

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Select Case ipstrKey2
                Case "2"
                    Try
                        cmdDB = New SqlCommand(M_MY_DISP_ID + "_S1", conDB)
                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("mntrreport_no", SqlDbType.NVarChar, ipstrKey8))
                        End With

                        'データ取得
                        opdstData = clsDataConnect.pfGet_DataSet(cmdDB)

                        'データ有無確認
                        If opdstData.Tables(0).Rows.Count = 0 Then
                            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "監視報告書兼依頼票")
                            Exit Function
                        End If

                        mfGet_SLFData = True

                    Catch ex As Exception
                        'データ取得エラー
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "監視報告書兼依頼票")

                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")

                    Finally
                        'DB切断
                        If Not clsDataConnect.pfClose_Database(conDB) Then
                            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        End If
                    End Try

                Case "1"
                    Try
                        cmdDB = New SqlCommand("ZCMPSEL028", conDB)
                        'パラメータ設定
                        With cmdDB.Parameters
                            'ＴＢＯＸＩＤ
                            .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, ipstrKey9))
                        End With

                        'データ取得
                        opdstData = clsDataConnect.pfGet_DataSet(cmdDB)

                        'データ有無確認
                        If opdstData.Tables(0).Rows.Count = 0 Then
                            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "ホール情報")
                            Me.txtTboxId.ppTextBox.Focus()
                            Exit Function
                        End If

                        mfGet_SLFData = True

                    Catch ex As Exception
                        'データ取得エラー
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ホール情報")

                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")

                    Finally
                        'DB切断
                        If Not clsDataConnect.pfClose_Database(conDB) Then
                            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        End If
                    End Try
            End Select
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Function

    ''' <summary>
    ''' データ設定処理(遷移元:SLFLSTP002)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_SLFData()
        Dim dtsData As DataSet = Nothing
        Dim dtRow As DataRow = Nothing              'DataRow
        Dim strKey() As String = Nothing

        strKey = ViewState(P_KEY)

        If mfGet_SLFData(strKey(0) _
                       , strKey(1) _
                       , strKey(2) _
                       , strKey(3) _
                       , strKey(4) _
                       , strKey(5) _
                       , strKey(6) _
                       , strKey(7) _
                       , strKey(8) _
                       , strKey(9) _
                       , dtsData) Then
            Select Case strKey(1)
                Case "2"
                    dtRow = dtsData.Tables(0).Rows(0)

                    '監視報告書兼依頼票情報設定
                    '--------------------------------
                    '2014/06/19 星野　ここから
                    '--------------------------------
                    'Me.lblMntrReportNo2.Text = strKey(8).ToString
                    Me.lblMntrReportNo2.Text = strKey(7).ToString
                    '--------------------------------
                    '2014/06/19 星野　ここまで
                    '--------------------------------
                    Me.dttOBreakD.ppText = dtRow("発生日").ToString
                    Me.txtTboxId.ppText = dtRow("ＴＢＯＸＩＤ").ToString
                    Me.lblHallNm2.Text = dtRow("ホール名").ToString
                    Me.txtSpcCharge.ppText = dtRow("報告者").ToString
                    If dtRow("削除フラグ").ToString <> "0" Then
                        Me.cbxDeleteFlg.Checked = True
                    End If
                    Me.ddlStatus.SelectedValue = dtRow("進捗状況").ToString
                    If dtRow("進捗状況").ToString = "02" Then
                        Me.ddlStatus.Enabled = False
                    Else
                        Me.ddlStatus.Enabled = True
                    End If
                    Me.ddlHpnStts.ppSelectedValue = dtRow("発生状況").ToString
                    Me.txtContent.ppText = dtRow("報告内容").ToString
                    Me.txtNgcDeal.ppText = dtRow("対応者").ToString
                    Me.ddlDealStatus.SelectedValue = dtRow("対応状況").ToString
                    Me.txtAnswer.ppText = dtRow("対応内容").ToString
                    mstrMntrEndFlg = dtRow("案件終了").ToString 'WATUPDP001-001

                Case "1"
                    Me.txtTboxId.ppText = strKey(8).ToString
                    Me.lblHallNm2.Text = dtsData.Tables(0).Rows(0).Item("ホール名").ToString
                    Me.txtContent.ppText = strKey(9).ToString
                    Me.dttOBreakD.ppDateBox.Focus()
            End Select

        End If
    End Sub
    '--------------------------------
    '2014/04/15 高松　ここまで
    '--------------------------------

    ''' <summary>
    ''' ドロップダウンリスト設定（進捗状況）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_ddlStatus()
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
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                cmdDB = New SqlCommand("ZCMPSEL015", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("duty_cd", SqlDbType.NVarChar, "36A"))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'ドロップダウンリスト設定
                Me.ddlStatus.Items.Clear()
                Me.ddlStatus.DataSource = dstOrders.Tables(0)
                Me.ddlStatus.DataTextField = "進捗ステータス名"
                Me.ddlStatus.DataValueField = "進捗ステータス"
                Me.ddlStatus.DataBind()
                Me.ddlStatus.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "進捗状況")
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
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（ＮＧＣ対応状況）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_ddlDealStatus()
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
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                cmdDB = New SqlCommand("ZCMPSEL015", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("duty_cd", SqlDbType.NVarChar, "36B"))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'ドロップダウンリスト設定
                Me.ddlDealStatus.Items.Clear()
                Me.ddlDealStatus.DataSource = dstOrders.Tables(0)
                Me.ddlDealStatus.DataTextField = "進捗ステータス名"
                Me.ddlDealStatus.DataValueField = "進捗ステータス"
                Me.ddlDealStatus.DataBind()
                Me.ddlDealStatus.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＮＧＣ対応状況")
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
        End If
    End Sub

    ''' <summary>
    ''' 遷移条件によって活性／非活性を設定する。
    ''' </summary>
    ''' <param name="ipshtMode"></param>
    ''' <remarks></remarks>
    Private Sub msSet_Mode(ByVal ipshtMode As ClsComVer.E_遷移条件)
        Select Case ipshtMode
            Case ClsComVer.E_遷移条件.参照
                pnlSPC.Enabled = False
                pnlNGC.Enabled = False
                btnUpdate.Enabled = False
                btnAdd.Enabled = False
                ''確認処理付与
                'Me.btnPrint.OnClientClick =
                '    pfGet_OCClickMes("10002", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "監視報告書兼依頼票")

            Case ClsComVer.E_遷移条件.更新
                btnAdd.Visible = False
                If strAddr <> P_ADD_NGC Then
                    pnlNGC.Enabled = False
                    Me.dttOBreakD.ppDateBox.Focus()
                Else
                    pnlSPC.Enabled = False
                    Me.txtNgcDeal.ppTextBox.Focus()
                End If
                '確認処理付与
                Me.btnUpdate.OnClientClick =
                    pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "監視報告書兼依頼票")
                'Me.btnPrint.OnClientClick =
                '    pfGet_OCClickMes("10002", clscomver.E_Mタイプ.情報, clscomver.E_Mモード.OKCancel, "監視報告書兼依頼票")

            Case ClsComVer.E_遷移条件.登録
                pnlNGC.Enabled = False
                btnUpdate.Visible = False
                btnPrint.Enabled = False
                Me.txtTboxId.ppTextBox.Focus()
                '確認処理付与
                Me.btnAdd.OnClientClick =
                    pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "監視報告書兼依頼票")
        End Select
    End Sub

    ''' <summary>
    ''' ホール情報取得処理
    ''' </summary>
    ''' <param name="strTboxid">ＴＢＯＸＩＤ</param>
    ''' <param name="opdstHallNm">ホール情報</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_HallInfo(ByVal strTboxid As String, ByRef opdstHallNm As DataSet) As Boolean
        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGet_HallInfo = False

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("ZCMPSEL028", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    'ＴＢＯＸＩＤ
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, strTboxid))
                End With

                'データ取得
                opdstHallNm = clsDataConnect.pfGet_DataSet(cmdDB)

                If opdstHallNm.Tables(0).Rows.Count = 0 Then
                    Me.txtTboxId.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
                    Exit Function
                End If

                mfGet_HallInfo = True

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ホール情報")
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
    End Function

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
