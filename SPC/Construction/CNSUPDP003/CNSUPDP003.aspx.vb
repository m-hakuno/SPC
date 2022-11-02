'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　工事進捗　参照／更新
'*　ＰＧＭＩＤ：　CNSUPDP003
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.11.27　：　土岐
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'CNSUPDP003-001     2015/04/08      加賀　　　明細の排他制御削除
'CNSUPDP003-002     2016/02/25      加賀　　　レイアウト変更
'CNSUPDP003-003     2016/03/28      加賀　　　明細の更新処理を変更(集信、作業者の更新を削除)
'CNSUPDP003-004     2016/03/28      加賀　　　撤去関連の活性制御変更
'CNSUPDP003-005     2016/04/01      加賀　　　画面更新処理修正
'CNSUPDP003-006     2016/04/01      加賀　　　明細の活性制御修正
'CNSUPDP003-007     2016/04/01      加賀　　　ボタンのテキストを修正
'CNSUPDP003-008     2016/04/11      加賀　　　明細の対応日時を初期化時に現在日時をセット
'CNSUPDP003-009     2016/04/11      加賀　　　対応者の入力をText→Dropdownに変更
'CNSUPDP003-010     2016/07/19      栗原　　　双子区分、MDN機器情報の更新処理を追加。撤去情報の抽出条件変更。撤去フラグ有の時のみ撤去情報に値をセットする。

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataLink
'Imports SPC.Global_asax
'-----------------------------
'2014/04/21 土岐　ここから
'-----------------------------
'排他制御用
Imports SPC.ClsCMExclusive
'-----------------------------
'2014/04/21 土岐　ここまで
'-----------------------------
#End Region

Public Class CNSUPDP003
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
    Private Const M_DISP_ID As String = P_FUN_CNS & P_SCR_UPD & P_PAGE & "003"
    Private Const M_VIEW_CONST_NO As String = "工事依頼番号"
    Private Const M_VIEW_EST_CLS As String = "設置区分"
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
    'Dim strBtn As String = "登録" 'CNSUPDP003-007
    'Dim strTrBtn As String = "登録"'CNSUPDP003-007

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '表設定
        pfSet_GridView(grvList, "CNSUPDP003", "CNSUPDP003_Header", Me.DivOut, 40, 10)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strKey(1) As String

        'ボタンのイベント設定
        AddHandler Me.btnUpdWrk.Click, AddressOf btnUpdWrk_Click            '作業者更新
        AddHandler Me.btnUpdTrbl.Click, AddressOf btnUpdTrbl_Click          'トラブル関連更新
        AddHandler btnDetailUpdate.Click, AddressOf btnDetailUpdate_Click   '明細更新
        AddHandler btnDetailInsert.Click, AddressOf btnDetailInsert_Click   '明細追加
        AddHandler btnDetailDelete.Click, AddressOf btnDetailDelete_Click   '明細削除
        AddHandler btnDetailClear.Click, AddressOf msClearSelect
        'フッターボタンのイベント設定                                       
        AddHandler Master.ppLeftButton1.Click, AddressOf btnComm_Click    '工事連絡票
        AddHandler Master.ppLeftButton2.Click, AddressOf btnCnst_Click    '工事依頼書 <<不使用>>
        AddHandler Master.ppLeftButton3.Click, AddressOf btnConst_Click   '構成配信
        AddHandler Master.ppLeftButton4.Click, AddressOf btnAnytime_Click '随時集信一覧
        AddHandler Master.ppRigthButton2.Click, AddressOf btnPrt_Click    '印刷
        AddHandler Master.ppRigthButton1.Click, AddressOf btnTrn_Click    '送信
        '対応者コード変更のイベント設定                                       
        'AddHandler Me.txtlblRespondersCd.ppTextBox.TextChanged, AddressOf txtlblRespondersCd_TextChanged 'CNSUPDP003-009
        '対応内容JavaScript設定(文字切)
        Me.txtContent.ppTextBox.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txtContent.ppMaxLength & """);")
        Me.txtContent.ppTextBox.Attributes.Add("onChange", "lenCheck(this,""" & Me.txtContent.ppMaxLength & """);")

        If Not IsPostBack Then  '初回表示
            Try
                'セッション変数「遷移条件」「キー情報」が存在しない場合、画面を閉じる
                If Session(P_SESSION_TERMS) Is Nothing Or Session(P_KEY) Is Nothing Then
                    'メッセージ表示
                    psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    '排他削除
                    clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                    '画面を閉じる
                    psClose_Window(Me)
                    Return
                End If

                'プログラムＩＤ、画面名設定
                Master.ppProgramID = M_DISP_ID
                Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

                'パンくずリスト設定
                Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

                'ボタン設定
                msSet_Button()

                '作業員名、連絡先を非表示にする
                Me.txtWorkerNm.Visible = False
                Me.txtCntactInfo.Visible = False
                Me.btnUpdWrk.Visible = False

                '対応者コードAutoPostBack設定
                'Me.txtlblRespondersCd.ppTextBox.AutoPostBack = True 'CNSUPDP003-009

                'ViewStateに「キー情報」「遷移条件」「遷移元ＩＤ」を保存
                strKey = Session(P_KEY)
                ViewState(M_VIEW_CONST_NO) = strKey(0)
                ViewState(M_VIEW_EST_CLS) = strKey(1)
                ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)
                ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

                '★排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
                If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then

                    Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)

                End If

                msSetddlResponser()         'ﾄﾞﾛｯﾌﾟﾀﾞｳﾝ設定 対応者
                msSetddlUncollect()         'ﾄﾞﾛｯﾌﾟﾀﾞｳﾝ設定 未集信理由
                msGet_Data(strKey(0), strKey(1)) '画面初期化処理
                msSetddlStatus()            'ﾄﾞﾛｯﾌﾟﾀﾞｳﾝ設定進捗ステータス

                '明細初期化
                msClearSelect()

                'CNSUPDP003-007 END
                ''作業員のボタン設定
                'If Not Me.lblICOBranchOffice_2.Text = String.Empty Then
                '    If Me.lblICOBranchOffice_2.Text = "990" Then
                '        If txtWorkerNm.ppText = String.Empty _
                '        And txtCntactInfo.ppText = String.Empty Then
                '            ViewState(strBtn) = "登録"
                '        Else
                '            ViewState(strBtn) = "変更"
                '        End If
                '    End If
                'End If

                ''トラブル関連のボタン設定
                'Dim bolTrbls() As Boolean = {Me.cbxAllCnst.Checked _
                '                           , Me.cbxCnstDelay.Checked _
                '                           , Me.cbxSpareMachineUse.Checked _
                '                           , Me.cbxMisCalcuCnst.Checked _
                '                           , Me.cbxIns.Checked _
                '                           , Me.cbxTel.Checked _
                '                           , Me.cbxOther.Checked _
                '                           , Me.cbxConstitution.Checked}
                'If bolTrbls.Contains(True) Then
                '    ViewState(strTrBtn) = "変更"  'トラブル登録済
                'Else
                '    ViewState(strTrBtn) = "登録"  'トラブル未登録
                'End If
                'CNSUPDP003-007 END

                '活性制御
                msSet_Mode(ViewState(P_SESSION_TERMS), ViewState(M_VIEW_EST_CLS), Me.grvList, 0)

            Catch ex As Exception
                'メッセージ表示
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "画面の初期表示")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            End Try
        End If


    End Sub

    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        Try
            Select Case Session(P_SESSION_AUTH)
                Case "管理者"
                Case "SPC"
                Case "営業所"
                    msSet_Mode(ClsComVer.E_遷移条件.参照, ViewState(M_VIEW_EST_CLS), Me.grvList, 0)
                    Master.ppLeftButton1.Enabled = False
                    Master.ppLeftButton3.Enabled = False
                    Master.ppLeftButton4.Enabled = False
                    btnDetailUpdate.Enabled = False
                    btnDetailInsert.Enabled = False
                    btnDetailDelete.Enabled = False
                    Master.ppRigthButton1.Enabled = False
                Case "NGC"
                    msSet_Mode(ClsComVer.E_遷移条件.参照, ViewState(M_VIEW_EST_CLS), Me.grvList, 0)
                    btnDetailUpdate.Enabled = False
                    btnDetailInsert.Enabled = False
                    btnDetailDelete.Enabled = False
                    'Master.ppLeftButton1.Enabled = True
                    Master.ppLeftButton2.Enabled = False
                    Master.ppLeftButton3.Enabled = False
                    Master.ppLeftButton4.Enabled = False
            End Select

            If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照 Then
                btnDetailUpdate.Enabled = False   '更新ボタン
                btnDetailDelete.Enabled = False   '削除ボタン
                btnDetailInsert.Enabled = False   '追加ボタン
                btnDetailClear.Enabled = False    'クリアボタン
                Me.txtWorkerNm.ppEnabled = False
                Me.txtCntactInfo.ppEnabled = False
                Me.btnUpdWrk.Enabled = False
                Me.btnUpdTrbl.Enabled = False
            End If

            '担当支社コードが990のとき活性にする
            If Me.lblICOBranchOffice_2.Text = "990" Then
                'Me.trWorker.Style.Clear()
                Me.lblWorkerNm_1.Visible = False
                Me.lblWorkerNm_2.Visible = False
                Me.lblContactInfo_1.Visible = False
                Me.lblContactInfo_2.Visible = False
                Me.txtWorkerNm.Visible = True
                Me.txtCntactInfo.Visible = True
                Me.btnUpdWrk.Visible = True
                'Me.btnUpdWrk.Text = ViewState(strBtn) 'CNSUPDP003-007 END
            Else
                'Me.trWorker.Style.Add("Display", "none")
                Me.lblWorkerNm_1.Visible = True
                Me.lblWorkerNm_2.Visible = True
                Me.lblContactInfo_1.Visible = True
                Me.lblContactInfo_2.Visible = True
                Me.txtWorkerNm.Visible = False
                Me.txtCntactInfo.Visible = False
                Me.btnUpdWrk.Visible = False
            End If
            'Me.btnUpdTrbl.Text = ViewState(strTrBtn) 'CNSUPDP003-007 END

            '更新モードの時、工事依頼書兼仕様書を表示しない
            If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 Then
                Master.ppLeftButton2.Visible = False
            End If
        Catch ex As Exception
            'メッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "画面の活性制御(ユーザー権限)")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub


#Region "対応明細"

    ''' <summary>
    ''' 一覧の選択ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        Dim intIndex As Integer
        Dim rowData As GridViewRow
        Dim strKey(1) As String
        Dim strTime As String

        If e.CommandName <> "btnSelect" Then
            Exit Sub
        End If

        'ログ出力開始
        psLogStart(Me)

        Try
            intIndex = Convert.ToInt32(e.CommandArgument)   'ボタン押下行のIndex
            rowData = grvList.Rows(intIndex)                'ボタン押下行
            '--------------------------------
            '2015/04/08 加賀　ここから　CNSUPDP003-001
            '--------------------------------
            ''-----------------------------
            ''2014/04/21 土岐　ここから
            ''-----------------------------
            ''★排他制御処理
            'Dim strExclusiveDate As String = Nothing
            'Dim arTable_Name As New ArrayList
            'Dim arKey As New ArrayList
            'Select Case ViewState(P_SESSION_TERMS)
            '    Case  ClsComVer.E_遷移条件.更新

            '        '★排他情報削除
            '        If Not Me.Master.ppExclusiveDateDtl = String.Empty Then

            '            If clsExc.pfDel_Exclusive(Me _
            '                               , Session(P_SESSION_SESSTION_ID) _
            '                               , Me.Master.ppExclusiveDateDtl) = 0 Then
            '                Me.Master.ppExclusiveDateDtl = String.Empty
            '            Else
            '                Exit Sub
            '            End If
            '        End If

            '        '★ロック対象テーブル名の登録
            '        arTable_Name.Insert(0, "D24_CNST_SITU_DTL")

            '        '★ロックテーブルキー項目の登録(D20_ARTCLTRNS_DTL)
            '        '-----------------------------
            '        '2014/05/26 土岐　ここから
            '        '-----------------------------
            '        arKey.Insert(0, (ViewState(M_VIEW_CONST_NO)))                         'D24_CONST_NO
            '        '-----------------------------
            '        '2014/05/26 土岐　ここまで
            '        '-----------------------------
            '        arKey.Insert(1, (ViewState(M_VIEW_EST_CLS)))                          'D24_EST_CLS
            '        arKey.Insert(2, (CType(rowData.FindControl("連番"), TextBox).Text))   'D24_SEQNO

            '        '★排他情報確認処理(更新画面へ遷移)
            '        If clsExc.pfSel_Exclusive(strExclusiveDate _
            '                         , Me _
            '                         , Session(P_SESSION_IP) _
            '                         , Session(P_SESSION_PLACE) _
            '                         , Session(P_SESSION_USERID) _
            '                         , Session(P_SESSION_SESSTION_ID) _
            '                         , ViewState(P_SESSION_GROUP_NUM) _
            '                         , M_DISP_ID _
            '                         , arTable_Name _
            '                         , arKey) = 0 Then

            '            '★登録年月日時刻(明細)
            '            Me.Master.ppExclusiveDateDtl = strExclusiveDate

            '        Else

            '            '排他ロック中
            '            Exit Sub

            '        End If
            'End Select
            ''-----------------------------
            ''2014/04/21 土岐　ここまで
            ''-----------------------------
            '--------------------------------
            '2015/04/08 加賀　ここまで
            '--------------------------------

            'Me.cbxAllConstraction.Checked = mfGet_CheckBoxCheck(CType(rowData.FindControl("発生工事全"), TextBox).Text)
            'Me.cbxConstractionDelay.Checked = mfGet_CheckBoxCheck(CType(rowData.FindControl("工事遅延"), TextBox).Text)
            'Me.cbxSpareMachineUse.Checked = mfGet_CheckBoxCheck(CType(rowData.FindControl("予備機使用"), TextBox).Text)
            'Me.cbxMiscalculationCons.Checked = mfGet_CheckBoxCheck(CType(rowData.FindControl("空振り工事"), TextBox).Text)
            'Me.cbxIns.Checked = mfGet_CheckBoxCheck(CType(rowData.FindControl("ＩＮＳ"), TextBox).Text)
            'Me.cbxTel.Checked = mfGet_CheckBoxCheck(CType(rowData.FindControl("ＴＥＬ"), TextBox).Text)
            'Me.cbxOther.Checked = mfGet_CheckBoxCheck(CType(rowData.FindControl("その他"), TextBox).Text)
            'Me.cbxConstitution.Checked = mfGet_CheckBoxCheck(CType(rowData.FindControl("構成"), TextBox).Text)
            Me.lblSerialNo_2.Text = CType(rowData.FindControl("連番"), TextBox).Text
            Me.datlblSupportDt.ppText = CType(rowData.FindControl("対応日"), TextBox).Text
            strTime = CType(rowData.FindControl("対応時間"), TextBox).Text
            If strTime.Length >= 2 Then
                Me.datlblSupportDt.ppHourText = strTime.Substring(0, 2)
            Else
                Me.datlblSupportDt.ppHourText = strTime
            End If
            If strTime.Length >= 5 Then
                Me.datlblSupportDt.ppMinText = strTime.Substring(3, 2)
            ElseIf strTime.Length >= 3 Then
                Me.datlblSupportDt.ppMinText = strTime.Substring(3)
            End If

            'CNSUPDP003-009
            'Me.txtlblRespondersCd.ppText = CType(rowData.FindControl("対応者コード"), TextBox).Text
            'Me.lblRespondersNm_2.Text = CType(rowData.FindControl("対応者名"), TextBox).Text
            Me.ddlResponser.ppSelectedValue = CType(rowData.FindControl("対応者コード"), TextBox).Text.Replace(Environment.NewLine, String.Empty)
            'CNSUPDP003-009 END
            Me.ddlSituation.SelectedValue = CType(rowData.FindControl("進捗コード"), TextBox).Text
            Me.txtContent.ppText = CType(rowData.FindControl("内容"), TextBox).Text

            btnDetailInsert.Enabled = False  '追加ボタン
            btnDetailUpdate.Enabled = True   '更新ボタン
            btnDetailDelete.Enabled = True   '削除ボタン

        Catch ex As Exception
            'メッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細の選択")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 明細 更新ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDetailUpdate_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        Try
            '個別エラーチェック
            'msCheck_DetailError() 'CNSUPDP003-009

            'エラーチェック
            If Page.IsValid Then
                '登録用日付作成
                Dim strSupportDt As String = Me.datlblSupportDt.ppText.Replace("/", "") _
                    + Me.datlblSupportDt.ppHourText _
                    + Me.datlblSupportDt.ppMinText _
                    + "00"
                '更新処理
                If mfUpdateDetail(ClsComVer.E_遷移条件.更新,
                            Me.lblConReqNo_2.Text,
                            ViewState(M_VIEW_EST_CLS),
                            Me.lblSerialNo_2.Text,
                            Me.ddlSituation.SelectedValue,
                            strSupportDt,
                            Me.ddlResponser.ppSelectedValue,
                            Me.txtContent.ppText,
                            mfGet_CheckBoxText(Me.cbxAllCnst.Checked),
                            mfGet_CheckBoxText(Me.cbxCnstDelay.Checked),
                            mfGet_CheckBoxText(Me.cbxSpareMachineUse.Checked),
                            mfGet_CheckBoxText(Me.cbxMisCalcuCnst.Checked),
                            mfGet_CheckBoxText(Me.cbxIns.Checked),
                            mfGet_CheckBoxText(Me.cbxTel.Checked),
                            mfGet_CheckBoxText(Me.cbxOther.Checked),
                            mfGet_CheckBoxText(Me.cbxConstitution.Checked),
                            User.Identity.Name,
                            Me.ddlAATConSituation.ppSelectedValue,
                            Me.ddlAATCUReason.SelectedValue,
                            Me.dttAATLstwrkDt.ppDate) Then
                    '選択データクリア
                    msClearSelect()
                    'CNSUPDP003-005
                    '画面更新
                    'msGet_Data(Me.lblConReqNo_2.Text, ViewState(M_VIEW_EST_CLS)) 
                    '明細更新
                    msGet_DataDtl(ViewState(M_VIEW_CONST_NO), ViewState(M_VIEW_EST_CLS))
                    'CNSUPDP003-005 END
                    '活性制御
                    'msSet_Mode(ViewState(P_SESSION_TERMS), ViewState(M_VIEW_EST_CLS), Me.grvList, 0) 'CNSUPDP003-006
                End If
            End If
        Catch ex As Exception
            'メッセージ表示
            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事進捗")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 明細 追加ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDetailInsert_Click(sender As Object, e As EventArgs)
        Dim strSupportDt As String

        'ログ出力開始
        psLogStart(Me)

        Try
            '個別エラーチェック
            'msCheck_DetailError() 'CNSUPDP003-009

            'エラーチェック
            If Page.IsValid Then
                '登録用日付作成
                strSupportDt = Me.datlblSupportDt.ppText.Replace("/", "") _
                    + Me.datlblSupportDt.ppHourText _
                    + Me.datlblSupportDt.ppMinText _
                    + "00"
                '登録処理
                If mfUpdateDetail(ClsComVer.E_遷移条件.登録,
                            Me.lblConReqNo_2.Text,
                            ViewState(M_VIEW_EST_CLS),
                            Me.lblSerialNo_2.Text,
                            Me.ddlSituation.SelectedValue,
                            strSupportDt,
                            Me.ddlResponser.ppSelectedValue,
                            Me.txtContent.ppText,
                            mfGet_CheckBoxText(Me.cbxAllCnst.Checked),
                            mfGet_CheckBoxText(Me.cbxCnstDelay.Checked),
                            mfGet_CheckBoxText(Me.cbxSpareMachineUse.Checked),
                            mfGet_CheckBoxText(Me.cbxMisCalcuCnst.Checked),
                            mfGet_CheckBoxText(Me.cbxIns.Checked),
                            mfGet_CheckBoxText(Me.cbxTel.Checked),
                            mfGet_CheckBoxText(Me.cbxOther.Checked),
                            mfGet_CheckBoxText(Me.cbxConstitution.Checked),
                            User.Identity.Name,
                            Me.ddlAATConSituation.ppSelectedValue,
                            Me.ddlAATCUReason.SelectedValue,
                            Me.dttAATLstwrkDt.ppDate) Then



                    '選択データクリア
                    msClearSelect()
                    'CNSUPDP003-005
                    '画面更新
                    'msGet_Data(Me.lblConReqNo_2.Text, ViewState(M_VIEW_EST_CLS)) 
                    '明細更新
                    msGet_DataDtl(ViewState(M_VIEW_CONST_NO), ViewState(M_VIEW_EST_CLS))
                    'CNSUPDP003-005 END
                    '活性制御
                    'msSet_Mode(ViewState(P_SESSION_TERMS), ViewState(M_VIEW_EST_CLS), Me.grvList, 0) 'CNSUPDP003-006



                End If
            End If

        Catch ex As Exception
            'メッセージ表示
            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事進捗")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 明細　削除ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDetailDelete_Click(sender As Object, e As EventArgs)
        'ログ出力開始
        psLogStart(Me)

        Try
            If mfDeleteDetail(Me.lblConReqNo_2.Text,
                        ViewState(M_VIEW_EST_CLS),
                        Me.lblSerialNo_2.Text,
                        User.Identity.Name) Then
                '選択データクリア
                msClearSelect()
                'CNSUPDP003-005
                '画面更新
                'msGet_Data(Me.lblConReqNo_2.Text, ViewState(M_VIEW_EST_CLS)) 
                '明細更新
                msGet_DataDtl(ViewState(M_VIEW_CONST_NO), ViewState(M_VIEW_EST_CLS))
                'CNSUPDP003-005 END
                '活性制御
                'msSet_Mode(ViewState(P_SESSION_TERMS), ViewState(M_VIEW_EST_CLS), Me.grvList, 0) 'CNSUPDP003-006

            End If

        Catch ex As Exception
            'メッセージ表示
            psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事進捗")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 明細 クリアボタン押下処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnDetailClear_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        Try
            'クリア
            msClearSelect()

        Catch ex As Exception
            'メッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細のクリア")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 選択クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearSelect()

        '★排他情報削除
        If Not Me.Master.ppExclusiveDateDtl = String.Empty Then

            If clsExc.pfDel_Exclusive(Me _
                               , Session(P_SESSION_SESSTION_ID) _
                               , Me.Master.ppExclusiveDateDtl) = 0 Then
                Me.Master.ppExclusiveDateDtl = String.Empty
            Else
                Exit Sub
            End If
        End If

        'トラブルチェック 不要？
        'Me.cbxAllCnst.Checked = False
        'Me.cbxCnstDelay.Checked = False
        'Me.cbxSpareMachineUse.Checked = False
        'Me.cbxMisCalcuCnst.Checked = False
        'Me.cbxIns.Checked = False
        'Me.cbxTel.Checked = False
        'Me.cbxOther.Checked = False
        'Me.cbxConstitution.Checked = False

        '明細情報
        Dim strDate As String = DateTime.Now.ToString("yyyy/MM/ddHHmm") '現在日時

        Me.lblSerialNo_2.Text = String.Empty
        'CNSUPDP003-008
        If strDate.Length < 14 Then
            Me.datlblSupportDt.ppText = String.Empty
            Me.datlblSupportDt.ppHourText = String.Empty
            Me.datlblSupportDt.ppMinText = String.Empty
        Else
            Me.datlblSupportDt.ppText = strDate.Substring(0, 10)
            Me.datlblSupportDt.ppHourText = strDate.Substring(10, 2)
            Me.datlblSupportDt.ppMinText = strDate.Substring(12, 2)
        End If
        'CNSUPDP003-008 END

        'CNSUPDP003-009
        'Me.txtlblRespondersCd.ppText = String.Empty
        'Me.lblRespondersNm_2.Text = String.Empty
        Me.ddlResponser.ppSelectedValue = String.Empty
        'CNSUPDP003-009 END
        Me.ddlSituation.SelectedIndex = 0
        Me.txtContent.ppText = String.Empty

        '活性制御
        btnDetailInsert.Enabled = True            '追加
        btnDetailUpdate.Enabled = False           '更新
        btnDetailDelete.Enabled = False           '削除

    End Sub

    'CNSUPDP003-009
    ' ''' <summary>
    ' ''' 明細 対応者コード変更時
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub txtlblRespondersCd_TextChanged(sender As Object, e As EventArgs)
    '    If Me.txtlblRespondersCd.ppText <> String.Empty Then
    '        SetResponders(Me.txtlblRespondersCd.ppText)
    '    End If
    'End Sub
    'CNSUPDP003-009 END

    'CNSUPDP003-009
    ' ''' <summary>
    ' ''' 明細 対応者名をセット
    ' ''' </summary>
    ' ''' <param name="ipstrCharge_CD">担当者コード</param>
    ' ''' <remarks></remarks>
    'Private Sub SetResponders(ByVal ipstrCharge_CD As String)
    '    Dim dtsChargeData As DataSet = Nothing

    '    '担当者名のリスト設定
    '    If pfGet_Employee(ipstrCharge_CD, "701", "701", dtsChargeData) Then
    '        lblRespondersNm_2.Text = dtsChargeData.Tables(0).Rows(0).Item("社員姓")
    '    Else
    '        lblRespondersNm_2.Text = String.Empty
    '    End If

    'End Sub
    'CNSUPDP003-009 END

    'CNSUPDP003-009
    ' ''' <summary>
    ' ''' 明細 入力項目のエラーチェックを行う。
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Sub msCheck_DetailError()
    '    '対応者コード整合性チェック
    '    If Not pfGet_Employee(Me.txtlblRespondersCd.ppText, "701", "701", Nothing) Then
    '        Me.txtlblRespondersCd.psSet_ErrorNo("2002", "該当する対応者")
    '    End If
    'End Sub
    'CNSUPDP003-009 END

    ''' <summary>
    ''' 明細 登録/更新処理
    ''' </summary>
    ''' <param name="ipshtProc_cls">処理区分（1:登録 ,2:更新）</param>
    ''' <param name="ipstrCnst_No">工事依頼番号</param>
    ''' <param name="ipstrEst_cls">設置区分</param>
    ''' <param name="ipstrSeqNo">連番</param>
    ''' <param name="ipstrStatus_CD">進捗ステータスコード</param>
    ''' <param name="ipstrCrrsp_DT">対応日時</param>
    ''' <param name="ipstrCharge_CD">担当者コード</param>
    ''' <param name="ipstrDetail">内容</param>
    ''' <param name="ipstrCnst_all">発生工事全</param>
    ''' <param name="ipstrCnst_delay">工事遅延</param>
    ''' <param name="ipstrUse_spare">予備機使用</param>
    ''' <param name="ipstrVain">空振り</param>
    ''' <param name="ipstrIns">ＩＮＳ</param>
    ''' <param name="ipstrTel">ＴＥＬ</param>
    ''' <param name="ipstrEtc">その他</param>
    ''' <param name="ipstrOrgnz">構成</param>
    ''' <param name="ipstrUserid">ユーザＩＤ</param>
    ''' <param name="ipstrClct_stts">随時集信状況</param>
    ''' <param name="ipstrUnclct_rsn">未集信理由</param>
    ''' <param name="ipdatLstwrk_dt">最終営業日</param>
    ''' <returns>成功：True, 失敗：False</returns>
    ''' <remarks></remarks>
    Private Function mfUpdateDetail(ByVal ipshtProc_cls As ClsComVer.E_遷移条件,
                              ByVal ipstrCnst_No As String,
                              ByVal ipstrEst_cls As String,
                              ByVal ipstrSeqNo As String,
                              ByVal ipstrStatus_CD As String,
                              ByVal ipstrCrrsp_DT As String,
                              ByVal ipstrCharge_CD As String,
                              ByVal ipstrDetail As String,
                              ByVal ipstrCnst_all As String,
                              ByVal ipstrCnst_delay As String,
                              ByVal ipstrUse_spare As String,
                              ByVal ipstrVain As String,
                              ByVal ipstrIns As String,
                              ByVal ipstrTel As String,
                              ByVal ipstrEtc As String,
                              ByVal ipstrOrgnz As String,
                              ByVal ipstrUserid As String,
                              ByVal ipstrClct_stts As String,
                              ByVal ipstrUnclct_rsn As String,
                              ByVal ipdatLstwrk_dt As DateTime) As Boolean
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim cmdCnst As SqlCommand = Nothing
        Dim cmdCnstDTL As SqlCommand = Nothing
        'Dim cmdAnyt As SqlCommand = Nothing     'CNSUPDP003-003
        Dim cmdCnstSend As SqlCommand = Nothing
        'Dim cmdCnstEmp As SqlCommand = Nothing 'CNSUPDP003-003
        Dim cmdTboxUp As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim intRtn As Integer
        'Dim brPC_flg As Boolean = True

        objStack = New StackFrame
        mfUpdateDetail = False

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                Using conTrn = conDB.BeginTransaction

                    '工事状況明細 登録/更新
                    cmdCnst = New SqlCommand("CNSUPDP003_U2", conDB)
                    cmdCnst.Transaction = conTrn
                    With cmdCnst.Parameters
                        'パラメータ設定
                        Select Case ipshtProc_cls   '処理区分
                            Case ClsComVer.E_遷移条件.登録
                                .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, "1"))
                            Case ClsComVer.E_遷移条件.更新
                                .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, "2"))
                                .Add(pfSet_Param("seqno", SqlDbType.Decimal, Decimal.Parse(ipstrSeqNo))) '連番
                        End Select
                        .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrCnst_No))          '工事依頼番号
                        .Add(pfSet_Param("est_cls", SqlDbType.NVarChar, ipstrEst_cls))          '設置区分
                        .Add(pfSet_Param("status_cd", SqlDbType.NVarChar, ipstrStatus_CD))      '進捗ステータスコード
                        .Add(pfSet_Param("crrsp_dt", SqlDbType.NVarChar, ipstrCrrsp_DT))        '対応日時
                        .Add(pfSet_Param("charge_cd", SqlDbType.NVarChar, ipstrCharge_CD))      '担当者コード
                        .Add(pfSet_Param("detail", SqlDbType.NVarChar, ipstrDetail))            '内容
                        .Add(pfSet_Param("cnst_all", SqlDbType.NVarChar, ipstrCnst_all))        '発生工事全
                        .Add(pfSet_Param("cnst_delay", SqlDbType.NVarChar, ipstrCnst_delay))    '工事遅延
                        .Add(pfSet_Param("use_spare", SqlDbType.NVarChar, ipstrUse_spare))      '予備機使用
                        .Add(pfSet_Param("vain", SqlDbType.NVarChar, ipstrVain))                '空振り
                        .Add(pfSet_Param("ins", SqlDbType.NVarChar, ipstrIns))                  'ＩＮＳ
                        .Add(pfSet_Param("tel", SqlDbType.NVarChar, ipstrTel))                  'ＴＥＬ
                        .Add(pfSet_Param("etc", SqlDbType.NVarChar, ipstrEtc))                  'その他
                        .Add(pfSet_Param("orgnz", SqlDbType.NVarChar, ipstrOrgnz))              '構成
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, ipstrUserid))            'ユーザＩＤ
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                       '戻り値

                    End With

                    '工事依頼書兼仕様書 更新
                    cmdCnstDTL = New SqlCommand("CNSUPDP003_U1", conDB)
                    cmdCnstDTL.Transaction = conTrn
                    With cmdCnstDTL.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrCnst_No))          '工事依頼番号
                        .Add(pfSet_Param("est_cls", SqlDbType.NVarChar, ipstrEst_cls))          '設置区分
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, ipstrUserid))            'ユーザＩＤ
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                       '戻り値

                    End With

                    'CNSUPDP003-003
                    ''随時一覧 登録/更新
                    ''親子確認
                    'cmdDB = New SqlCommand("CNSUPDP003_S4", conDB)
                    'With cmdDB.Parameters
                    '    'パラメータ設定
                    '    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrCnst_No))          '工事依頼番号
                    'End With
                    'dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                    'If dstOrders.Tables.Count <> 0 Then
                    '    If dstOrders.Tables(0).Rows(0).Item("親子フラグ").ToString = "2" Then
                    '        cmdDB = New SqlCommand("CNSUPDP003_S5", conDB)
                    '        With cmdDB.Parameters
                    '            'パラメータ設定
                    '            .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, dstOrders.Tables(0).Rows(0).Item("TBOXID").ToString))          'TBOXID
                    '            .Add(pfSet_Param("cnstdt", SqlDbType.NVarChar, dstOrders.Tables(0).Rows(0).Item("工事日").ToString))
                    '        End With
                    '        dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                    '        If dstOrders.Tables.Count <> 0 Then
                    '            If dstOrders.Tables(0).Rows.Count <> 0 Then
                    '                brPC_flg = False
                    '            End If
                    '        End If
                    '    End If
                    'End If
                    'If brPC_flg = True Then
                    '    cmdAnyt = New SqlCommand("CNSUPDP003_U3", conDB)
                    '    With cmdAnyt.Parameters
                    '        'パラメータ設定
                    '        .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrCnst_No))          '工事依頼番号
                    '        .Add(pfSet_Param("status_cd", SqlDbType.NVarChar, ipstrStatus_CD))      '進捗ステータスコード
                    '        .Add(pfSet_Param("clct_stts", SqlDbType.NVarChar, ipstrClct_stts))      '随時集信状況
                    '        .Add(pfSet_Param("unclct_rsn", SqlDbType.NVarChar, ipstrUnclct_rsn))    '未集信理由
                    '        If ipdatLstwrk_dt = Nothing Then
                    '            .Add(pfSet_Param("lstwrk_dt", SqlDbType.DateTime, DBNull.Value))    '最終営業日
                    '        Else
                    '            .Add(pfSet_Param("lstwrk_dt", SqlDbType.DateTime, ipdatLstwrk_dt))  '最終営業日
                    '        End If
                    '        .Add(pfSet_Param("userid", SqlDbType.NVarChar, ipstrUserid))            'ユーザＩＤ
                    '        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                       '戻り値
                    '    End With
                    'End If
                    'CNSUPDP003-003 END

                    If ipstrEst_cls = "2" Then
                        If ddlSituation.Text = "07" OrElse ddlSituation.Text = "08" Then
                            '工事状況明細 登録
                            If ddlSituation.Text = "08" Then
                                If ipstrCnst_No.Substring(0, 5) <> "N0090" Then
                                    cmdCnstSend = New SqlCommand("CNSUPDP003_U4", conDB)
                                    With cmdCnstSend.Parameters
                                        'パラメータ設定
                                        .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrCnst_No))          '工事依頼番号
                                        .Add(pfSet_Param("est_cls", SqlDbType.NVarChar, ipstrEst_cls))          '設置区分
                                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, ipstrUserid))            'ユーザＩＤ
                                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                       '戻り値
                                    End With
                                End If
                            End If

                            If lblNew_2.Text = "●" _
                            OrElse lblAllRmv_2.Text = "●" _
                            OrElse lblOncRmv_2.Text = "●" _
                            OrElse lblReInst_2.Text = "●" _
                            Then
                                'TBOX情報更新
                                cmdTboxUp = New SqlCommand("CNSUPDP003_U6", conDB)
                                With cmdTboxUp.Parameters
                                    'パラメータ設定
                                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrCnst_No))          '工事依頼番号
                                    .Add(pfSet_Param("est_cls", SqlDbType.NVarChar, ipstrEst_cls))          '設置区分
                                    .Add(pfSet_Param("userid", SqlDbType.NVarChar, ipstrUserid))            'ユーザＩＤ
                                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                       '戻り値
                                End With
                            End If
                        End If
                    End If

                    'CNSUPDP003-003
                    'If Not Me.lblICOBranchOffice_2.Text = String.Empty Then
                    '    If Me.lblICOBranchOffice_2.Text = "990" Then
                    '        '作業員、連絡先 登録
                    '        cmdCnstEmp = New SqlCommand("CNSUPDP003_U5", conDB)
                    '        With cmdCnstEmp.Parameters
                    '            'パラメータ設定
                    '            .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrCnst_No))              '工事依頼番号
                    '            .Add(pfSet_Param("employee", SqlDbType.NVarChar, Me.txtWorkerNm.ppText))    '作業員名
                    '            .Add(pfSet_Param("tell", SqlDbType.NVarChar, Me.txtCntactInfo.ppText))      '連絡先
                    '            .Add(pfSet_Param("userid", SqlDbType.NVarChar, ipstrUserid))                'ユーザＩＤ
                    '            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
                    '        End With
                    '    End If
                    'End If
                    'CNSUPDP003-003 END 

                    'データ登録/更新

                    'CNSUPDP003-003
                    'If brPC_flg = True Then
                    '    cmdAnyt.Transaction = conTrn
                    'End If
                    'CNSUPDP003-003 END
                    If ipstrEst_cls = "2" Then
                        If ddlSituation.Text = "07" OrElse ddlSituation.Text = "08" Then
                            If ddlSituation.Text = "08" Then
                                If ipstrCnst_No.Substring(0, 5) <> "N0090" Then
                                    cmdCnstSend.Transaction = conTrn
                                End If
                            End If
                            If lblNew_2.Text = "●" _
                            OrElse lblAllRmv_2.Text = "●" _
                            OrElse lblOncRmv_2.Text = "●" _
                            OrElse lblReInst_2.Text = "●" _
                            Then
                                cmdTboxUp.Transaction = conTrn
                            End If
                        End If
                    End If
                    'CNSUPDP003-003
                    'If Not Me.lblICOBranchOffice_2.Text = String.Empty Then
                    '    If Me.lblICOBranchOffice_2.Text = "990" Then
                    '        cmdCnstEmp.Transaction = conTrn
                    '    End If
                    'End If
                    'CNSUPDP003-003 END

                    '工事状況明細 登録/更新
                    cmdCnst.CommandType = CommandType.StoredProcedure
                    cmdCnst.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdCnst.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        Select Case ipshtProc_cls
                            Case ClsComVer.E_遷移条件.登録
                                psMesBox(Me, "00008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事進捗", intRtn.ToString)
                            Case ClsComVer.E_遷移条件.更新
                                psMesBox(Me, "00007", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事進捗", intRtn.ToString)
                        End Select
                        Exit Function
                    End If

                    '工事依頼書兼仕様書 更新
                    cmdCnstDTL.CommandType = CommandType.StoredProcedure
                    cmdCnstDTL.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdCnstDTL.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        Select Case ipshtProc_cls
                            Case ClsComVer.E_遷移条件.登録
                                psMesBox(Me, "00008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事進捗", intRtn.ToString)
                            Case ClsComVer.E_遷移条件.更新
                                psMesBox(Me, "00007", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事進捗", intRtn.ToString)
                        End Select
                        Exit Function
                    End If

                    'CNSUPDP003-003
                    ''随時一覧 登録/更新
                    'If brPC_flg = True Then
                    '    cmdAnyt.CommandType = CommandType.StoredProcedure
                    '    cmdAnyt.ExecuteNonQuery()

                    '    'ストアド戻り値チェック
                    '    intRtn = Integer.Parse(cmdAnyt.Parameters("retvalue").Value.ToString)
                    '    If intRtn <> 0 Then
                    '        Select Case ipshtProc_cls
                    '            Case ClsComVer.E_遷移条件.登録
                    '                psMesBox(Me, "00008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事進捗", intRtn.ToString)
                    '            Case ClsComVer.E_遷移条件.更新
                    '                psMesBox(Me, "00007", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事進捗", intRtn.ToString)
                    '        End Select
                    '        Exit Function
                    '    End If
                    'End If
                    'CNSUPDP003-003 END

                    '送信、TBOX情報更新
                    If ipstrEst_cls = "2" Then
                        If ddlSituation.Text = "07" OrElse ddlSituation.Text = "08" Then
                            If ddlSituation.Text = "08" AndAlso Me.lblConReqNo_2.Text.Substring(0, 5).ToString <> "N0090" Then
                                cmdCnstSend.CommandType = CommandType.StoredProcedure
                                cmdCnstSend.ExecuteNonQuery()
                                'ストアド戻り値チェック
                                intRtn = Integer.Parse(cmdCnst.Parameters("retvalue").Value.ToString)
                                If intRtn <> 0 Then
                                    psMesBox(Me, "20001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事完了速報", intRtn.ToString)
                                    conTrn.Rollback()
                                    Exit Function
                                End If
                            End If

                            If lblNew_2.Text = "●" _
                            OrElse lblAllRmv_2.Text = "●" _
                            OrElse lblOncRmv_2.Text = "●" _
                            OrElse lblReInst_2.Text = "●" _
                            Then
                                cmdTboxUp.CommandType = CommandType.StoredProcedure
                                cmdTboxUp.ExecuteNonQuery()

                                'ストアド戻り値チェック
                                intRtn = Integer.Parse(cmdCnst.Parameters("retvalue").Value.ToString)
                                If intRtn <> 0 Then
                                    psMesBox(Me, "20001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事完了速報", intRtn.ToString)
                                    conTrn.Rollback()
                                    Exit Function
                                End If
                            End If
                        End If
                    End If

                    'CNSUPDP003-003
                    ''作業員 連絡先 登録
                    'If Not Me.lblICOBranchOffice_2.Text = String.Empty Then
                    '    If Me.lblICOBranchOffice_2.Text = "990" Then
                    '        cmdCnstEmp.CommandType = CommandType.StoredProcedure
                    '        cmdCnstEmp.ExecuteNonQuery()

                    '        'ストアド戻り値チェック
                    '        intRtn = Integer.Parse(cmdCnst.Parameters("retvalue").Value.ToString)
                    '        If intRtn <> 0 Then
                    '            psMesBox(Me, "20001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "作業員", intRtn.ToString)
                    '            conTrn.Rollback()
                    '            Exit Function
                    '        End If
                    '        '作業員の「登録」ボタン設定
                    '        If txtWorkerNm.ppText = String.Empty _
                    '                And txtCntactInfo.ppText = String.Empty Then
                    '            ViewState(strBtn) = "登録"
                    '        Else
                    '            ViewState(strBtn) = "変更"
                    '        End If
                    '    End If
                    'End If
                    'CNSUPDP003-003 END

                    'CNSUPDP003_010
                    'TBOX、ホール情報の「双子店区分」「MDN機器」を更新
                    '最新工事依頼か判定
                    Dim objWKDS As New DataSet
                    cmdDB = New SqlCommand("CNSUPDP003_S11", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Parameters.Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, ipstrCnst_No))
                    cmdDB.Transaction = conTrn

                    objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)
                    'データセット取得チェック
                    If objWKDS.Tables.Count > 0 Then
                        If objWKDS.Tables(0).Rows.Count > 0 Then
                        Else
                            '最新工事依頼の場合、ＭＤＮ、双子店情報の更新
                            If mfUpdataTbox(conTrn, conDB, "更新") = False Then
                                Throw New Exception
                            End If
                            'CNSUPDP003_010 END
                        End If
                    End If
                    Call psDisposeDataSet(objWKDS)
                    '最新工事依頼でない場合、
                    'コミット
                    conTrn.Commit()
                End Using

                '完了メッセージ
                Select Case ipshtProc_cls
                    Case ClsComVer.E_遷移条件.登録
                        psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "工事進捗", intRtn.ToString)
                    Case ClsComVer.E_遷移条件.更新
                        psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "工事進捗", intRtn.ToString)
                End Select

                'CNSUPDP003-010
                msGet_Data(ipstrCnst_No, ipstrEst_cls)
                'CNSUPDP003-010 END

                '正常終了
                mfUpdateDetail = True
            Catch ex As Exception
                'メッセージ表示
                Select Case ipshtProc_cls
                    Case ClsComVer.E_遷移条件.登録
                        psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事進捗")
                    Case ClsComVer.E_遷移条件.更新
                        psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事進捗")
                End Select
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
            'メッセージ表示
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If
    End Function

    ''' <summary>
    ''' 明細 削除処理
    ''' </summary>
    ''' <param name="ipstrCnst_No">工事依頼番号</param>
    ''' <param name="ipstrEst_cls">設置区分</param>
    ''' <param name="ipstrSeqNo">連番</param>
    ''' <param name="ipstrUserid">ユーザＩＤ</param>
    ''' <returns>成功：True, 失敗：False</returns>
    ''' <remarks></remarks>
    Private Function mfDeleteDetail(ByVal ipstrCnst_No As String,
                              ByVal ipstrEst_cls As String,
                              ByVal ipstrSeqNo As String,
                              ByVal ipstrUserid As String) As Boolean
        Dim conDB As SqlConnection = Nothing
        Dim cmdCnst As SqlCommand = Nothing
        Dim cmdCnstDTL As SqlCommand = Nothing
        Dim cmdAnyt As SqlCommand = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim intRtn As Integer

        objStack = New StackFrame
        mfDeleteDetail = False

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                '工事状況明細 登録/更新
                cmdCnst = New SqlCommand("CNSUPDP003_D1", conDB)
                With cmdCnst.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrCnst_No))          '工事依頼番号
                    .Add(pfSet_Param("est_cls", SqlDbType.NVarChar, ipstrEst_cls))          '設置区分
                    .Add(pfSet_Param("seqno", SqlDbType.Decimal, ipstrSeqNo))               '連番
                    .Add(pfSet_Param("userid", SqlDbType.NVarChar, ipstrUserid))            'ユーザＩＤ
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                       '戻り値

                End With

                '工事依頼書兼仕様書 更新
                cmdCnstDTL = New SqlCommand("CNSUPDP003_U1", conDB)
                With cmdCnstDTL.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrCnst_No))          '工事依頼番号
                    .Add(pfSet_Param("est_cls", SqlDbType.NVarChar, ipstrEst_cls))          '設置区分
                    .Add(pfSet_Param("userid", SqlDbType.NVarChar, ipstrUserid))            'ユーザＩＤ
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                       '戻り値

                End With

                'データ登録/更新
                Using conTrn = conDB.BeginTransaction
                    cmdCnst.Transaction = conTrn
                    cmdCnstDTL.Transaction = conTrn

                    '工事状況明細 登録/更新
                    cmdCnst.CommandType = CommandType.StoredProcedure
                    cmdCnst.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdCnst.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, "00009", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事進捗", intRtn.ToString)
                        Exit Function
                    End If

                    '工事依頼書兼仕様書 更新
                    cmdCnstDTL.CommandType = CommandType.StoredProcedure
                    cmdCnstDTL.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdCnstDTL.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, "00009", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事進捗", intRtn.ToString)
                        Exit Function
                    End If

                    'CNSUPDP003_010  
                    'TBOX、ホール情報の「双子店区分」「MDN機器」を更新
                    Dim objWKDS As New DataSet
                    cmdDB = New SqlCommand("CNSUPDP003_S11", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Parameters.Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, ipstrCnst_No))
                    cmdDB.Transaction = conTrn
                    objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)
                    'データセット取得チェック
                    If objWKDS.Tables.Count > 0 Then
                        If objWKDS.Tables(0).Rows.Count > 0 Then
                        Else
                            '最新工事依頼の場合、ＭＤＮ、双子店情報の更新
                            If mfUpdataTbox(conTrn, conDB, "更新") = False Then
                                Throw New Exception
                            End If
                            'CNSUPDP003_010 END
                        End If
                    End If
                    Call psDisposeDataSet(objWKDS)
                    'If mfUpdataTbox(conTrn, conDB, "削除") = False Then
                    '    Throw New Exception
                    'End If
                    'CNSUPDP003_010 END

                    'コミット
                    conTrn.Commit()
                End Using

                psMesBox(Me, "00002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "工事進捗")

                'CNSUPDP003-010
                msGet_Data(ipstrCnst_No, ipstrEst_cls)
                'CNSUPDP003-010 END

                '正常終了
                mfDeleteDetail = True
            Catch ex As Exception
                'メッセージ表示
                psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事進捗")
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

#Region "画面遷移"

    ''' <summary>
    ''' 工事連絡ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnComm_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        objStack = New StackFrame

        Try
            '工事連絡票 画面のパス
            Const CNSUPDP004 As String = "~/" & P_CNS & "/" &
                P_FUN_CNS & P_SCR_UPD & P_PAGE & "004" & "/" &
                P_FUN_CNS & P_SCR_UPD & P_PAGE & "004.aspx"
            Dim dstOrders As DataSet = Nothing
            Dim estCls As String = String.Empty
            If lblEstCls_2.Text = "仮設置" Then
                estCls = "1"
            ElseIf lblEstCls_2.Text = "本設置" Then
                estCls = "2"
            End If
            Dim intRtn As Integer
            intRtn = mfGet_DataCnst_Comm(Me.lblConReqNo_2.Text, estCls, dstOrders)
            If intRtn <> 0 Then
                Exit Sub
            End If
            Session(P_SESSION_BCLIST) = Master.ppBcList_Text 'パンくずリスト
            Session(P_SESSION_OLDDISP) = M_DISP_ID              '遷移元の画面ＩＤ
            Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号 

            If dstOrders Is Nothing _
                OrElse dstOrders.Tables(0).Rows.Count <= 0 Then
                Session(P_KEY) = {Me.lblConReqNo_2.Text, estCls}                                        '工事依頼番号
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録                                      '登録
            Else
                Dim strExclusiveDate As String = Nothing
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList

                '★ロック対象テーブル名の登録
                arTable_Name.Insert(0, "D25_CNST_COMM")

                '★ロックテーブルキー項目の登録(D25_CNST_COMM)
                arKey.Insert(0, dstOrders.Tables(0).Rows(0).Item("連絡票管理番号").ToString)

                '★排他情報確認処理(更新画面へ遷移)
                If clsExc.pfSel_Exclusive(strExclusiveDate _
                                 , Me _
                                 , Session(P_SESSION_IP) _
                                 , Session(P_SESSION_PLACE) _
                                 , Session(P_SESSION_USERID) _
                                 , Session(P_SESSION_SESSTION_ID) _
                                 , ViewState(P_SESSION_GROUP_NUM) _
                                 , P_FUN_CNS & P_SCR_UPD & P_PAGE & "004" _
                                 , arTable_Name _
                                 , arKey) = 0 Then
                    '★登録年月日時刻
                    Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate
                Else
                    '排他ロック中
                    Exit Sub
                End If

                Session(P_KEY) = {dstOrders.Tables(0).Rows(0).Item("連絡票管理番号").ToString}  '連絡票管理番号
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新                                      '更新
            End If

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
                            objStack.GetMethod.Name, CNSUPDP004, strPrm, "TRANS")

            '別ブラウザ起動
            psOpen_Window(Me, CNSUPDP004)

        Catch ex As Exception
            'メッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "画面遷移")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 工事依頼書兼仕様書ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCnst_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        objStack = New StackFrame

        Try
            '工事依頼書兼仕様書 画面のパス
            Const CNSUPDP001 As String = "~/" & P_CNS & "/" &
                P_FUN_CNS & P_SCR_UPD & P_PAGE & "001" & "/" &
                P_FUN_CNS & P_SCR_UPD & P_PAGE & "001.aspx"
            Dim dstOrders As DataSet = Nothing
            Dim strExclusiveDate As String = Nothing
            Dim arTable_Name As New ArrayList
            Dim arKey As New ArrayList

            '-----------------------------
            '2014/05/30 後藤　ここから
            '-----------------------------
            '★ロック対象テーブル名の登録
            'arTable_Name.Insert(0, "D39_CNST_NO")
            'arTable_Name.Insert(0, "D39_CNSTREQSPEC")
            ''-----------------------------
            ''2014/05/30 後藤　ここまで
            ''-----------------------------

            ''★ロックテーブルキー項目の登録(D39_CNST_NO)
            'arKey.Insert(0, Me.lblConReqNo_2.Text)

            ''★排他情報確認処理(更新画面へ遷移)
            'If clsExc.pfSel_Exclusive(strExclusiveDate _
            '                 , Me _
            '                 , Session(P_SESSION_IP) _
            '                 , Session(P_SESSION_PLACE) _
            '                 , Session(P_SESSION_USERID) _
            '                 , Session(P_SESSION_SESSTION_ID) _
            '                 , ViewState(P_SESSION_GROUP_NUM) _
            '                 , P_FUN_CNS & P_SCR_UPD & P_PAGE & "001" _
            '                 , arTable_Name _
            '                 , arKey) = 0 Then

            '    '★登録年月日時刻
            '    Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate
            'Else
            '    '排他ロック中
            '    Exit Sub
            'End If
            Session(P_KEY) = {Me.lblConReqNo_2.Text, "0", lblPrgSituation_2.Text.Remove(0, 3)}           '工事依頼番号
            Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照   '遷移条件
            Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号
            Session(P_SESSION_BCLIST) = Master.ppBcList_Text 'パンくずリスト

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
                            objStack.GetMethod.Name, CNSUPDP001, strPrm, "TRANS")
            '別ブラウザ起動
            psOpen_Window(Me, CNSUPDP001)

        Catch ex As Exception
            'メッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "画面遷移")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 構成配信ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnConst_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        objStack = New StackFrame

        Try
            '構成配信 画面のパス
            Const DLLSELP001 As String = "~/" & P_CNS & "/" &
                P_FUN_DLL & P_SCR_SEL & P_PAGE & "001" & "/" &
                P_FUN_DLL & P_SCR_SEL & P_PAGE & "001.aspx"
            Dim dstOrders As DataSet = Nothing

            Session(P_KEY) = {Me.lblTboxId_2.Text}                'ＴＢＯＸＩＤ
            Session(P_SESSION_TERMS) = ViewState(P_SESSION_TERMS)   '遷移条件
            Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号
            Session(P_SESSION_BCLIST) = Master.ppBcList_Text 'パンくずリスト

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
                            objStack.GetMethod.Name, DLLSELP001, strPrm, "TRANS")

            '別ブラウザ起動
            psOpen_Window(Me, DLLSELP001)

        Catch ex As Exception
            'メッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "画面遷移")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 即時集信一覧ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAnytime_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        objStack = New StackFrame

        Try
            '即時集信一覧 画面のパス
            Const MSTLSTP001 As String = "~/" & P_CNS & "/" &
                P_FUN_MST & P_SCR_LST & P_PAGE & "001" & "/" &
                P_FUN_MST & P_SCR_LST & P_PAGE & "001.aspx"
            Dim dstOrders As DataSet = Nothing

            Session(P_KEY) = {Me.lblTboxId_2.Text}                  'ＴＢＯＸＩＤ
            Session(P_SESSION_TERMS) = ViewState(P_SESSION_TERMS)   '遷移条件
            Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号
            Session(P_SESSION_BCLIST) = Master.ppBcList_Text 'パンくずリスト

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
                            objStack.GetMethod.Name, MSTLSTP001, strPrm, "TRANS")

            '別ブラウザ起動
            psOpen_Window(Me, MSTLSTP001)

        Catch ex As Exception
            'メッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "画面遷移")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        'ログ出力終了
        psLogEnd(Me)
    End Sub


#End Region

#Region "印刷"

    ''' <summary>
    ''' 印刷ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPrt_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        '印刷
        msSet_Print()

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 工事対応状況詳細一覧表
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msSet_Print()

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders_1 As New DataSet
        Dim rpt() As Object = Nothing
        Dim strSql() As String = Nothing
        Dim strFileName() As String = Nothing
        Dim strEstCls As String = ""

        objStack = New StackFrame

        Try

            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Return False
            End If
            cmdDB = New SqlCommand("CNSUPDP003_S6", conDB)
            With cmdDB.Parameters
                .Add(pfSet_Param("const_no", SqlDbType.NVarChar, Me.lblConReqNo_2.Text))
                Select Case lblEstCls_2.Text
                    Case "仮設置"
                        .Add(pfSet_Param("est_cls", SqlDbType.NVarChar, "1"))
                    Case "本設置"
                        .Add(pfSet_Param("est_cls", SqlDbType.NVarChar, "2"))
                End Select
            End With
            dstOrders_1 = clsDataConnect.pfGet_DataSet(cmdDB)

            rpt = {New CNSREP010}

            strFileName = {"工事対応状況詳細一覧表"}

            '帳票を出力する
            psPrintPDF(Me, rpt(0), dstOrders_1.Tables(0), strFileName(0))

        Catch ex As DBConcurrencyException
            'メッセージ表示
            psMesBox(Me, "0005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Catch ex As Exception
            'メッセージ表示
            psMesBox(Me, "0003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Me.grvList.DataBind()
        Finally
            'DB切断
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If
        End Try

        Return True

    End Function

#End Region

#Region "送信"

    ''' <summary>
    ''' 送信ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnTrn_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        '送信処理
        If mfTransmission(Me.lblConReqNo_2.Text,
                          ViewState(M_VIEW_EST_CLS),
                          User.Identity.Name) Then
            '画面更新
            msGet_Data(Me.lblConReqNo_2.Text, ViewState(M_VIEW_EST_CLS))
            '活性制御
            msSet_Mode(ViewState(P_SESSION_TERMS), ViewState(M_VIEW_EST_CLS), Me.grvList, 0)
        End If

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 送信処理
    ''' </summary>
    ''' <param name="ipstrCnst_No">工事依頼番号</param>
    ''' <param name="ipstrEst_cls">設置区分</param>
    ''' <param name="ipstrUserid">ユーザＩＤ</param>
    ''' <returns>成功：True, 失敗：False</returns>
    ''' <remarks></remarks>
    Private Function mfTransmission(ByVal ipstrCnst_No As String,
                              ByVal ipstrEst_cls As String,
                              ByVal ipstrUserid As String) As Boolean
        Dim conDB As SqlConnection = Nothing
        Dim cmdCnst As SqlCommand = Nothing
        Dim intRtn As Integer

        mfTransmission = False
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                '工事状況明細 登録
                cmdCnst = New SqlCommand("CNSUPDP003_U4", conDB)
                With cmdCnst.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrCnst_No))          '工事依頼番号
                    .Add(pfSet_Param("est_cls", SqlDbType.NVarChar, ipstrEst_cls))          '設置区分
                    .Add(pfSet_Param("userid", SqlDbType.NVarChar, ipstrUserid))            'ユーザＩＤ
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                       '戻り値

                End With

                'データ登録
                Using conTrn = conDB.BeginTransaction
                    cmdCnst.Transaction = conTrn

                    '
                    cmdCnst.CommandType = CommandType.StoredProcedure
                    cmdCnst.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdCnst.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, "20001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事完了速報", intRtn.ToString)
                        Exit Function
                    End If

                    'コミット
                    conTrn.Commit()
                End Using

                'psMesBox(Me, "20009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "工事完了速報")

                '正常終了
                mfTransmission = True
            Catch ex As Exception
                psMesBox(Me, "20001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事完了速報")
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

#Region "作業員"

    ''' <summary>
    ''' 作業員登録ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpdWrk_Click(sender As Object, e As EventArgs)
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim cmdCnstEmp As SqlCommand = Nothing
        Dim intRtn As Integer

        'ログ出力開始
        psLogStart(Me)

        objStack = New StackFrame

        '作業員登録の時は必須項目にしない
        datlblSupportDt.ppRequiredField = False
        'txtlblRespondersCd.ppRequiredField = False 'CNSUPDP003-009
        txtContent.ppRequiredField = False

        'TEL
        If txtCntactInfo.ppText <> String.Empty Then
            If Long.TryParse(txtCntactInfo.ppText.Replace("-", ""), New Long) = False Then
                '形式エラー
                txtCntactInfo.psSet_ErrorNo("4001", txtCntactInfo.ppName, "ハイフン(-)又は数字")
            Else
                If Long.TryParse(txtCntactInfo.ppText.Substring(0, 1), New Long) = False Then
                    '形式エラー
                    txtCntactInfo.psSet_ErrorNo("4001", txtCntactInfo.ppName, "正しい形式")
                End If
            End If
        End If

        'エラーチェック
        If Page.IsValid Then

            '作業員 連絡先 登録
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    '入力チェック
                    If Me.txtWorkerNm.ppText = String.Empty And Me.txtCntactInfo.ppText = String.Empty _
                        And btnUpdWrk.Text = "登録" Then
                        Me.txtWorkerNm.psSet_ErrorNo("5001", Me.txtWorkerNm.ppName)
                        Me.txtCntactInfo.psSet_ErrorNo("5001", Me.txtCntactInfo.ppName)
                    Else
                        'データ登録/更新
                        If Not Me.lblICOBranchOffice_2.Text = String.Empty Then
                            If Me.lblICOBranchOffice_2.Text = "990" Then
                                '作業員、連絡先 登録
                                cmdCnstEmp = New SqlCommand("CNSUPDP003_U5", conDB)
                                With cmdCnstEmp.Parameters
                                    'パラメータ設定
                                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, Me.lblConReqNo_2.Text))     '工事依頼番号
                                    .Add(pfSet_Param("employee", SqlDbType.NVarChar, Me.txtWorkerNm.ppText))    '作業員名
                                    .Add(pfSet_Param("tell", SqlDbType.NVarChar, Me.txtCntactInfo.ppText))      '連絡先
                                    .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))         'ユーザＩＤ
                                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
                                End With
                            End If

                            Using conTrn = conDB.BeginTransaction
                                cmdCnstEmp.Transaction = conTrn
                                cmdCnstEmp.CommandType = CommandType.StoredProcedure
                                cmdCnstEmp.ExecuteNonQuery()

                                'ストアド戻り値チェック
                                intRtn = Integer.Parse(cmdCnstEmp.Parameters("retvalue").Value.ToString)
                                If intRtn <> 0 Then
                                    psMesBox(Me, "20001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "作業員", intRtn.ToString)
                                    conTrn.Rollback()
                                    Exit Sub
                                End If
                                'コミット
                                conTrn.Commit()
                            End Using

                            'CNSUPDP003-007
                            ''作業員の「登録」ボタン設定
                            'If txtWorkerNm.ppText = String.Empty _
                            '        And txtCntactInfo.ppText = String.Empty Then
                            '    ViewState(strBtn) = "登録"
                            'Else
                            '    ViewState(strBtn) = "変更"
                            'End If
                            'CNSUPDP003-007 END


                            '完了メッセージ
                            Select Case btnUpdWrk.Text.ToString
                                Case "登録"
                                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "作業員情報", intRtn.ToString)
                                Case "変更"
                                    psMesBox(Me, "00009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "作業員情報", intRtn.ToString)
                            End Select
                        End If
                    End If
                Catch ex As Exception
                    Select Case btnUpdWrk.Text
                        Case "登録"
                            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "作業員情報")
                        Case "変更"
                            psMesBox(Me, "00010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "作業員情報")
                    End Select
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
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

#End Region

#Region "トラブル & 撤去関連"

    ''' <summary>
    ''' トラブル登録ボタン押下処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnUpdTrbl_Click(sender As Object, e As EventArgs)
        Dim conDB As SqlConnection = Nothing
        Dim cmdTrbl As SqlCommand = Nothing
        Dim cmdAnyt As SqlCommand = Nothing
        Dim intRtn As Integer

        'ログ出力開始
        psLogStart(Me)

        'エラーチェック
        If Page.IsValid Then

            'トラブル内容登録
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    'トラブル関連 登録/更新コマンド
                    cmdTrbl = New SqlCommand("CNSUPDP003_U7", conDB)
                    cmdTrbl.CommandType = CommandType.StoredProcedure
                    With cmdTrbl.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, Me.lblConReqNo_2.Text))                             '工事依頼番号
                        .Add(pfSet_Param("All", SqlDbType.NVarChar, mfGet_CheckBoxText(Me.cbxAllCnst.Checked)))             '発生工事全
                        .Add(pfSet_Param("Delay", SqlDbType.NVarChar, mfGet_CheckBoxText(Me.cbxCnstDelay.Checked)))         '工事遅延
                        .Add(pfSet_Param("Use", SqlDbType.NVarChar, mfGet_CheckBoxText(Me.cbxSpareMachineUse.Checked)))     '予備機仕様
                        .Add(pfSet_Param("Miscal", SqlDbType.NVarChar, mfGet_CheckBoxText(Me.cbxMisCalcuCnst.Checked)))     '空振り工事
                        .Add(pfSet_Param("Ins", SqlDbType.NVarChar, mfGet_CheckBoxText(Me.cbxIns.Checked)))                 'INS
                        .Add(pfSet_Param("Tel", SqlDbType.NVarChar, mfGet_CheckBoxText(Me.cbxTel.Checked)))                 'TEL
                        .Add(pfSet_Param("Oth", SqlDbType.NVarChar, mfGet_CheckBoxText(Me.cbxOther.Checked)))               'その他
                        .Add(pfSet_Param("Stitution", SqlDbType.NVarChar, mfGet_CheckBoxText(Me.cbxConstitution.Checked)))  '構成
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                                 'ユーザＩＤ
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                                   '戻り値
                    End With


                    '撤去関連 登録/更新コマンド 'CNSUPDP003-004
                    If hdnRmv.Value = "1" Then
                        cmdAnyt = New SqlCommand("CNSUPDP003_U3", conDB)
                        cmdAnyt.CommandType = CommandType.StoredProcedure
                        With cmdAnyt.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, lblConReqNo_2.Text))                    '工事依頼番号
                            .Add(pfSet_Param("clct_stts", SqlDbType.NVarChar, ddlAATConSituation.ppSelectedValue))  '随時集信状況
                            .Add(pfSet_Param("unclct_rsn", SqlDbType.NVarChar, ddlAATCUReason.SelectedValue))       '未集信理由
                            If dttAATLstwrkDt.ppDate = Nothing Then
                                .Add(pfSet_Param("lstwrk_dt", SqlDbType.DateTime, DBNull.Value))                    '最終営業日
                            Else
                                .Add(pfSet_Param("lstwrk_dt", SqlDbType.DateTime, dttAATLstwrkDt.ppDate))           '最終営業日
                            End If
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, Session(P_SESSION_USERID)))              'ユーザＩＤ
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                       '戻り値
                        End With
                    End If
                    'CNSUPDP003-004 END


                    Using conTrn = conDB.BeginTransaction

                        'トラブル関連
                        cmdTrbl.Transaction = conTrn     'トランザクション設定
                        cmdTrbl.ExecuteNonQuery()        '実行

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdTrbl.Parameters("retvalue").Value)
                        If intRtn <> 0 Then
                            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "トラブル関連", intRtn.ToString)
                            conTrn.Rollback()
                            Exit Sub
                        End If

                        '撤去関連 CNSUPDP003-004
                        If hdnRmv.Value = "1" Then
                            cmdAnyt.Transaction = conTrn 'トランザクション設定
                            cmdAnyt.ExecuteNonQuery()    '実行

                            'ストアド戻り値チェック
                            intRtn = Integer.Parse(cmdAnyt.Parameters("retvalue").Value)
                            If intRtn <> 0 Then
                                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "撤去関連", intRtn.ToString)
                                conTrn.Rollback()
                                Exit Sub
                            End If
                        End If
                        'CNSUPDP003-004 END

                        'コミット
                        conTrn.Commit()
                    End Using

                    'CNSUPDP003-007
                    ''ボタンの文言設定
                    'If Integer.Parse(mfGet_CheckBoxText(Me.cbxAllCnst.Checked)) + _
                    '   Integer.Parse(mfGet_CheckBoxText(Me.cbxCnstDelay.Checked)) + _
                    '   Integer.Parse(mfGet_CheckBoxText(Me.cbxSpareMachineUse.Checked)) + _
                    '   Integer.Parse(mfGet_CheckBoxText(Me.cbxMisCalcuCnst.Checked)) + _
                    '   Integer.Parse(mfGet_CheckBoxText(Me.cbxIns.Checked)) + _
                    '   Integer.Parse(mfGet_CheckBoxText(Me.cbxTel.Checked)) + _
                    '   Integer.Parse(mfGet_CheckBoxText(Me.cbxOther.Checked)) + _
                    '   Integer.Parse(mfGet_CheckBoxText(Me.cbxConstitution.Checked)) = 0 Then
                    '    ViewState(strTrBtn) = "登録"
                    'Else
                    '    ViewState(strTrBtn) = "変更"
                    'End If
                    'CNSUPDP003-007 END

                    '完了メッセージ
                    Select Case btnUpdTrbl.Text
                        Case "登録"
                            psMesBox(Me, "00009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "トラブル関連＆撤去関連", intRtn.ToString)
                        Case "変更"
                            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "トラブル関連＆撤去関連", intRtn.ToString)
                    End Select
                Catch ex As Exception
                    'エラーメッセージ
                    Select Case btnUpdTrbl.Text
                        Case "登録"
                            psMesBox(Me, "00010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "トラブル関連＆撤去関連")
                        Case "変更"
                            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "トラブル関連＆撤去関連")
                    End Select

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
                'DB接続エラーメッセージメッセージ
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

        End If
        'ログ出力終了
        psLogEnd(Me)

    End Sub

#End Region

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' ボタンの設定
    ''' </summary>
    Private Sub msSet_Button()

        '「更新」「追加」「削除」ボタン設定
        btnDetailInsert.Text = P_BTN_NM_UPD
        btnDetailInsert.Text = "追加"
        btnDetailDelete.Text = P_BTN_NM_DEL
        btnDetailDelete.Visible = True

        '「印刷」ボタン設定
        Master.ppRigthButton2.Text = P_BTN_NM_PRI
        Master.ppRigthButton2.Visible = True
        Master.ppRigthButton2.CausesValidation = False

        '「送信」ボタン設定
        Master.ppRigthButton1.Text = P_BTN_NM_TRA
        Master.ppRigthButton1.Visible = False
        Master.ppRigthButton1.OnClientClick =
            pfGet_OCClickMes("20001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事完了速報")

        '画面遷移ボタン設定
        Master.ppLeftButton1.Text = "工事連絡票"
        Master.ppLeftButton2.Text = "工事依頼書兼仕様書"
        Master.ppLeftButton3.Text = "構成配信"
        Master.ppLeftButton4.Text = "即時集信一覧"
        Master.ppLeftButton1.Visible = True
        Master.ppLeftButton2.Visible = True
        Master.ppLeftButton3.Visible = True
        Master.ppLeftButton4.Visible = True

        '「戻る」「印刷」「明細削除」、画面遷移ボタンの検証無効化
        btnDetailDelete.CausesValidation = False
        Master.ppRigthButton1.CausesValidation = False
        Master.ppRigthButton2.CausesValidation = False
        Master.ppLeftButton1.CausesValidation = False
        Master.ppLeftButton2.CausesValidation = False
        Master.ppLeftButton3.CausesValidation = False
        Master.ppLeftButton4.CausesValidation = False

        '確認ダイアログ設定
        btnUpdWrk.OnClientClick =
            pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "作業員情報")
        btnUpdTrbl.OnClientClick =
            pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "トラブル関連＆撤去関連情報")
        btnDetailUpdate.OnClientClick =
            pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事進捗")
        btnDetailInsert.OnClientClick =
            pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事進捗")
        btnDetailDelete.OnClientClick =
            pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事進捗")
        Master.ppRigthButton2.OnClientClick =
            pfGet_OCClickMes("10002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事対応状況詳細一覧表")

    End Sub

    ''' <summary>
    ''' 検索処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data(ByVal ipstrCnst_no As String,
                           ByVal ipstrEst_cls As String)
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing

        objStack = New StackFrame

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("CNSUPDP003_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrCnst_no))
                    .Add(pfSet_Param("est_cls", SqlDbType.NVarChar, ipstrEst_cls))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得したデータを画面に設定
                msSet_MainData(dstOrders)

                '明細検索
                msGet_DataDtl(ipstrCnst_no, ipstrEst_cls)

            Catch ex As Exception
                'メッセージ表示
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "工事進捗")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '排他削除
                clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                'CloseOpen
                psClose_Window(Me)
                Return
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            '排他削除
            clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
            'CloseOpen
            psClose_Window(Me)
            Return
        End If
    End Sub

    ''' <summary>
    ''' 明細検索処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_DataDtl(ByVal ipstrCnst_no As String,
                           ByVal ipstrEst_cls As String)
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing

        objStack = New StackFrame

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("CNSUPDP003_S2", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("const_no", SqlDbType.NVarChar, ipstrCnst_no))
                    .Add(pfSet_Param("est_cls", SqlDbType.NVarChar, ipstrEst_cls))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '件数を設定
                'Master.ppCount = dstOrders.Tables(0).Rows.Count.ToString
                lblCount.Text = dstOrders.Tables(0).Rows.Count.ToString

                '取得したデータをリストに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                'メッセージ表示
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "工事進捗明細")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '排他削除
                clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                '画面閉じる
                psClose_Window(Me)
                Return
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        Else
            'メッセージ表示
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            '排他削除
            clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
            '画面閉じる
            psClose_Window(Me)
            Return
        End If
    End Sub

    ''' <summary>
    ''' データ設定処理
    ''' </summary>
    ''' <param name="ipdstData">設定データ</param>
    ''' <remarks></remarks>
    Private Sub msSet_MainData(ByVal ipdstData As DataSet)

        objStack = New StackFrame

        Try
            '取得したデータを設定
            With ipdstData.Tables(0).Rows(0)
                Me.lblEstCls_2.Text = .Item("設置区分").ToString
                Me.lblConReqNo_2.Text = .Item("工事依頼番号").ToString
                Me.lblCnstDt_2.Text = .Item("工事開始日").ToString       'CNSUPDP003-001 追加
                Me.lblPrgSituation_2.Text = .Item("設置区分").ToString
                Me.lblPrgSituation_2.Text = mfEditColon(.Item("進捗状況").ToString)
                Me.lblEwSection_2.Text = .Item("ＥＷ区分").ToString
                Me.lblNjSection_2.Text = .Item("ＮＬ区分").ToString
                Me.lblTboxId_2.Text = .Item("ＴＢＯＸＩＤ").ToString
                Me.lblHoleNm_2.Text = .Item("ホール名").ToString
                Me.lblTboxSystem_2.Text = .Item("ＴＢＯＸシステム").ToString
                Me.lblVer_2.Text = .Item("ＶＥＲ").ToString
                Me.lblAddress_2.Text = .Item("ホール住所").ToString
                Me.lblTel_2.Text = .Item("ＴＥＬ").ToString
                Me.lblAgency_2.Text = .Item("代理店").ToString
                Me.lblAgencyshop_2.Text = .Item("代行店").ToString
                Me.lblNew_2.Text = .Item("工事区分_新規").ToString
                Me.lblExpns_2.Text = .Item("工事区分_増設").ToString
                Me.lblSmRmv_2.Text = .Item("工事区分_一部撤去").ToString
                Me.lblShpRelocat_2.Text = .Item("工事区分_移設").ToString
                Me.lblAllRmv_2.Text = .Item("工事区分_全撤去").ToString
                Me.lblOncRmv_2.Text = .Item("工事区分_一時撤去").ToString
                Me.lblReInst_2.Text = .Item("工事区分_再設置").ToString
                Me.lblCnChng_2.Text = .Item("工事区分_構成変更").ToString
                Me.lblCnDlvry_2.Text = .Item("工事区分_構成配信").ToString
                Me.lblOther_2.Text = .Item("工事区分_その他").ToString
                Me.lblVup_2.Text = .Item("VERUP日付区分").ToString
                'CNSUPDP003-004
                'If .Item("工事区分_全撤去").ToString() <> "●" _
                '    And .Item("工事区分_一時撤去").ToString() <> "●" Then '随時一覧更新なし
                '    msSetAnttime(False)
                'End If
                'CNSUPDP003-004 END
                Me.lblOtherContent_2.Text = .Item("その他内容").ToString
                Me.lblWorkerNm_2.Text = mfEditColon(.Item("作業員名").ToString)
                Me.lblContactInfo_2.Text = .Item("連絡先").ToString
                Me.lblICOBranchOffice_2.Text = mfEditColon(.Item("担当支社").ToString)

                'CNSUPDP003-010 
                If .Item("撤去フラグ").ToString = "0" Then
                    Me.ddlAATConSituation.ppSelectedValue = "0" 'なし
                    Me.ddlAATCUReason.SelectedValue = String.Empty
                    Me.dttAATLstwrkDt.ppText = String.Empty
                Else
                    If .Item("随時集信状況") Is DBNull.Value Then
                        Me.ddlAATConSituation.ppSelectedValue = "0" 'なし
                    Else
                        Me.ddlAATConSituation.ppSelectedValue = .Item("随時集信状況").ToString
                    End If
                    Me.ddlAATCUReason.SelectedValue = .Item("随時集信未実施理由").ToString
                    Me.dttAATLstwrkDt.ppText = .Item("最終営業日").ToString
                End If
                'CNSUPDP003-010 END

                Me.txtWorkerNm.ppText = .Item("作業員名2").ToString
                Me.txtCntactInfo.ppText = .Item("連絡先2").ToString
                Me.cbxAllCnst.Checked = .Item("発生工事全").ToString
                Me.cbxCnstDelay.Checked = .Item("工事遅延").ToString
                Me.cbxSpareMachineUse.Checked = .Item("予備機使用").ToString
                Me.cbxMisCalcuCnst.Checked = .Item("空振り工事").ToString
                Me.cbxIns.Checked = .Item("INS").ToString
                Me.cbxTel.Checked = .Item("TEL").ToString
                Me.cbxOther.Checked = .Item("その他").ToString
                Me.cbxConstitution.Checked = .Item("構成").ToString
            End With

            '撤去関連編集可否 'CNSUPDP003-004
            With ipdstData
                'If ViewState(M_VIEW_EST_CLS) = "1" Then
                '    hdnRmv.Value = "0"
                'Else
                '    If .Tables(0).Rows(0).Item("撤去フラグ").ToString = "0" Then
                '        hdnRmv.Value = "0"
                '    Else
                '        If .Tables(0).Rows(0).Item("親子フラグ").ToString = "0" Then
                '            hdnRmv.Value = "1"
                '        Else
                '            If .Tables(1).Rows(0)(0).ToString = "1" Then '親子同時工事数
                '                hdnRmv.Value = "1"
                '            Else
                '                hdnRmv.Value = "0"
                '            End If
                '        End If
                '    End If
                'End If
                If .Tables(0).Rows(0).Item("撤去フラグ").ToString = "0" Then
                    hdnRmv.Value = "0"
                Else
                    hdnRmv.Value = "1"
                End If
            End With
            'CNSUPDP003-004 END

        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' コロン編集
    ''' </summary>
    ''' <param name="ipstrData"></param>
    ''' <remarks></remarks>
    Private Function mfEditColon(ByVal ipstrData As String) As String

        If ipstrData.Trim = ":" Then
            ipstrData = String.Empty
        End If

        If Microsoft.VisualBasic.Strings.Right(ipstrData.Trim, 1) = ":" Or
                    Microsoft.VisualBasic.Strings.Left(ipstrData.Trim, 1) = ":" Then
            ipstrData = ipstrData.Replace(":", "")
        End If

        Return ipstrData
    End Function

    ''' <summary>
    ''' 随時一覧項目の入力可否設定
    ''' </summary>
    ''' <param name="ipblnOn">入力可否（True：ＯＮ False：ＯＦＦ）</param>
    ''' <remarks></remarks>
    Private Sub msSetAnttime(ByVal ipblnOn As Boolean)
        ddlAATConSituation.ppEnabled = ipblnOn  '随時集信状況
        ddlAATCUReason.Enabled = ipblnOn        '随時集信未実施理由
        dttAATLstwrkDt.ppEnabled = ipblnOn      '最終営業日
    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（進捗ステータスマスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlStatus()

        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット

        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                cmdDB = New SqlCommand("ZCMPSEL015", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("duty_cd", SqlDbType.NVarChar, "21"))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'ドロップダウンリスト設定
                '仮登録の依頼は終了を消す
                Me.ddlSituation.Items.Clear()
                If Me.lblPrgSituation_2.Text.Substring(0, 2).ToString Is {"03", "04", "05"} Then
                    For int As Integer = 0 To dstOrders.Tables(0).Rows.Count - 1
                        Select Case dstOrders.Tables(0).Rows(int).Item("進捗ステータス").ToString
                            Case "08"
                                dstOrders.Tables(0).Rows.RemoveAt(int)
                                int -= 1
                        End Select
                        If int = dstOrders.Tables(0).Rows.Count - 1 Then
                            Exit For
                        End If
                    Next
                End If
                Me.ddlSituation.DataSource = dstOrders.Tables(0)
                Me.ddlSituation.DataTextField = "進捗ステータス名"
                Me.ddlSituation.DataValueField = "進捗ステータス"
                Me.ddlSituation.DataBind()

            Catch ex As Exception
                'メッセージ表示
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "進捗ステータスマスタ一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（未集信）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlUncollect()

        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット

        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                cmdDB = New SqlCommand("ZCMPSEL024", conDB)

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'ドロップダウンリスト設定
                'Me.ddlAATCUReason.Items.Clear()
                Me.ddlAATCUReason.DataSource = dstOrders.Tables(0)
                Me.ddlAATCUReason.DataTextField = "理由"
                Me.ddlAATCUReason.DataValueField = "コード"
                Me.ddlAATCUReason.DataBind()
                Me.ddlAATCUReason.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加

            Catch ex As Exception
                'メッセージ表示
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "未集信マスタ一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（対応者）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlResponser()

        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット

        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                cmdDB = New SqlCommand("ZCMPSEL046", conDB)
                cmdDB.Parameters.Add(pfSet_Param("comp_cd", SqlDbType.NVarChar, "701"))
                cmdDB.Parameters.Add(pfSet_Param("office_cd", SqlDbType.NVarChar, "701"))
                cmdDB.Parameters.Add(pfSet_Param("data_exist", SqlDbType.Int, 1, ParameterDirection.Output))

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'ドロップダウンリスト設定
                Me.ddlResponser.ppDropDownList.SelectedIndex = -1
                Me.ddlResponser.ppDropDownList.DataSource = dstOrders.Tables(0)
                Me.ddlResponser.ppDropDownList.DataTextField = "表示名"
                Me.ddlResponser.ppDropDownList.DataValueField = "社員コード"
                Me.ddlResponser.ppDropDownList.DataBind()
                Me.ddlResponser.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加

            Catch ex As Exception
                'メッセージ表示
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "社員情報の取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' 連絡票検索処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfGet_DataCnst_Comm(ByVal ipstrCnst_no As String,
                                         ByVal ipstrEst_Cls As String,
                                         ByRef opdstOrders As DataSet) As Integer
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing

        objStack = New StackFrame

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("CNSUPDP003_S3", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("const_no", SqlDbType.NVarChar, ipstrCnst_no))
                    .Add(pfSet_Param("est_cls", SqlDbType.NVarChar, ipstrEst_Cls))
                End With

                'リストデータ取得
                opdstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '正常終了
                mfGet_DataCnst_Comm = 0
            Catch ex As Exception
                '異常終了
                mfGet_DataCnst_Comm = -1
                'メッセージ表示
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事連絡票")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    '異常終了
                    mfGet_DataCnst_Comm = -1
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            '異常終了
            mfGet_DataCnst_Comm = -1
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If
    End Function

    ''' <summary>
    ''' チェックボックスの更新値取得
    ''' </summary>
    ''' <param name="ipblnChecked">チェック状態</param>
    ''' <returns>チェックあり："1", チェックなし:"0"</returns>
    ''' <remarks></remarks>
    Private Function mfGet_CheckBoxText(ByVal ipblnChecked As Boolean) As String
        If ipblnChecked Then
            mfGet_CheckBoxText = "1"
        Else
            mfGet_CheckBoxText = "0"
        End If
    End Function

    ''' <summary>
    ''' 文字列からチェックボックスのチェック状態取得
    ''' </summary>
    ''' <param name="ipstrChecked">チェック状態</param>
    ''' <returns>"1":True, 以外:False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_CheckBoxCheck(ByVal ipstrChecked As String) As Boolean
        If ipstrChecked = "1" Then
            mfGet_CheckBoxCheck = True
        Else
            mfGet_CheckBoxCheck = False
        End If
    End Function

    ''' <summary>
    ''' 遷移条件によって活性／非活性を設定する。
    ''' </summary>
    ''' <param name="ipshtMode">遷移条件</param>
    ''' <param name="ipEstCls">設置区分</param>
    ''' <param name="ipgrvList">一覧</param>
    ''' <remarks></remarks>
    Private Sub msSet_Mode(ByVal ipshtMode As ClsComVer.E_遷移条件,
                           ByVal ipEstCls As String,
                           ByVal ipgrvList As GridView,
                           ByVal ibutton As Integer)

        Select Case ipshtMode
            Case ClsComVer.E_遷移条件.更新
                Master.ppLeftButton1.Enabled = True          '工事連絡票
                If ibutton = 0 Then
                    btnDetailUpdate.Enabled = False           '更新
                    btnDetailInsert.Enabled = True            '追加
                    btnDetailDelete.Enabled = False           '削除
                Else
                    btnDetailUpdate.Enabled = True            '更新
                    btnDetailInsert.Enabled = False           '追加
                    btnDetailDelete.Enabled = True            '削除
                End If
                Master.ppRigthButton1.Enabled = False        '送信

                '本設置のみ活性判定を行う
                If ipEstCls = "2" Then
                    '一覧に進捗ステータス完了がある場合に送信ボタンを活性とする
                    For Each rowData As GridViewRow In ipgrvList.Rows
                        If CType(rowData.FindControl("進捗コード"), TextBox).Text = "08" Then
                            Master.ppRigthButton1.Enabled = True        '送信
                        End If
                    Next
                End If

                Me.cbxAllCnst.Enabled = True                'トラブル関連_発生工事全
                Me.cbxCnstDelay.Enabled = True              'トラブル関連_工事遅延
                Me.cbxMisCalcuCnst.Enabled = True           'トラブル関連_予備機使用
                Me.cbxIns.Enabled = True                    'トラブル関連_空振り工事
                Me.cbxTel.Enabled = True                    'トラブル関連_ＩＮＳ
                Me.cbxOther.Enabled = True                  'トラブル関連_その他
                Me.cbxConstitution.Enabled = True           'トラブル関連_構成
                Me.datlblSupportDt.ppEnabled = True         '対応日時
                'CNSUPDP003-009
                'Me.txtlblRespondersCd.ppEnabled = True      '対応者コード
                Me.ddlResponser.ppEnabled = True            '対応者コード
                'CNSUPDP003-009 END
                Me.ddlSituation.Enabled = True              '進捗状況
                Me.txtContent.ppEnabled = True              '内容
                Me.grvList.Enabled = True                   '一覧

                '撤去関連の活性制御 'CNSUPDP003-004
                If hdnRmv.Value = "0" Then
                    msSetAnttime(False)
                Else
                    msSetAnttime(True)
                End If
                'CNSUPDP003-004 END

            Case ClsComVer.E_遷移条件.参照
                Master.ppLeftButton1.Enabled = False      '工事連絡票
                Master.ppRigthButton1.Enabled = False     '送信
                btnDetailUpdate.Enabled = False             '更新
                btnDetailInsert.Enabled = False             '追加
                btnDetailDelete.Enabled = False             '削除
                Me.cbxAllCnst.Enabled = False               'トラブル関連_発生工事全
                Me.cbxCnstDelay.Enabled = False             'トラブル関連_工事遅延
                Me.cbxSpareMachineUse.Enabled = False       'トラブル関連_予備機使用
                Me.cbxMisCalcuCnst.Enabled = False          'トラブル関連_空振り工事
                Me.cbxIns.Enabled = False                   'トラブル関連_ＩＮＳ
                Me.cbxTel.Enabled = False                   'トラブル関連_ＴＥＬ
                Me.cbxOther.Enabled = False                 'トラブル関連_その他
                Me.cbxConstitution.Enabled = False          'トラブル関連_構成
                msSetAnttime(False)                         '随時集信状況、随時集信未実施理由、最終営業日
                Me.datlblSupportDt.ppEnabled = False        '対応日時
                'CNSUPDP003-009
                'Me.txtlblRespondersCd.ppEnabled = False     '対応者コード
                Me.ddlResponser.ppEnabled = False            '対応者コード
                'CNSUPDP003-009 END
                Me.ddlSituation.Enabled = False             '進捗状況
                Me.txtContent.ppEnabled = False             '内容
                Me.grvList.Enabled = True                   '一覧
                '-----------------------------
                '2014/06/23 武　ここから
                '-----------------------------
                '-----------------------------
                '2014/05/26 土岐　ここから
                '-----------------------------
                'Me.grvList.Enabled = False                 '一覧
                '-----------------------------
                '2014/05/26 土岐　ここまで
                '-----------------------------
                '-----------------------------
                '2014/06/23 武　ここまで
                '-----------------------------
        End Select

    End Sub

    'CNSUPDP003-010
    Private Function mfUpdataTbox(ByVal tran As SqlTransaction, ByVal conDB As SqlConnection, ByVal strMode As String) As Boolean
        Try
            Dim drCnstInfo As DataRow = Nothing
            Dim strPrcCd As String = mfCheckUpdTbox(drCnstInfo, tran, conDB, strMode)
            mfUpdataTbox = False

            Select Case strPrcCd
                Case "0"
                    '処理無し
                    mfUpdataTbox = True
                Case "1" '双子、MDN更新
                    '双子店更新
                    If mfUpdTwin(drCnstInfo.Item("D39_HALL_CD").ToString.Trim, drCnstInfo.Item("D39_TWIN_CD").ToString.Trim, tran, conDB) = False Then
                    End If
                    'MDN機器更新
                    If mfUpdMDN(drCnstInfo, tran, conDB) = False Then
                    End If
                    mfUpdataTbox = True
                Case "3" 'MDN機器戻し
                    If mfUpdMDNRev(drCnstInfo, tran, conDB) = False Then
                    End If
                    mfUpdataTbox = True
                Case Else
                    'エラー
                    psMesBox(Me, "30029", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事依頼書兼仕様書データを取得できませんでした。", "データが正しいか")

            End Select
        Catch ex As Exception
            mfUpdataTbox = False
        End Try
    End Function

    ''' <summary>
    ''' 双子店区分、MDN機器の更新有無をチェックする。
    ''' </summary>
    ''' <param name="drCnstInfo">工事依頼書の情報（更新前）を上書きして返す</param>
    ''' <returns>0:更新不要　1:MDN、双子店更新　2:MDNのみ更新　3:MDN戻し
    ''' </returns>
    ''' <remarks></remarks>
    Private Function mfCheckUpdTbox(ByRef drCnstInfo As DataRow, ByVal trans As SqlTransaction, ByVal conDB As SqlConnection, ByVal strMode As String) As String

        Dim cmdDB As SqlCommand = Nothing
        Dim intRtn As Integer = 0
        Dim dsCnstInfo As New DataSet

        objStack = New StackFrame

        '接続
        Try
            '工事データ取得
            cmdDB = New SqlCommand("CNSUPDP003_S10", conDB)
            cmdDB.CommandType = CommandType.StoredProcedure
            cmdDB.Transaction = trans

            With cmdDB.Parameters
                .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, lblConReqNo_2.Text.Trim))
            End With
            dsCnstInfo = clsDataConnect.pfGet_DataSet(cmdDB)
            'データセット取得チェック
            If dsCnstInfo Is Nothing OrElse dsCnstInfo.Tables(0).Rows().Count = 0 Then
                Return "9" 'エラー
                Exit Function
            End If

            drCnstInfo = dsCnstInfo.Tables(0).Rows(0)

            'FS稼働チェック(FS稼働有のみかつ本設置のみ更新処理対象) 
            If drCnstInfo.Item("D39_FSWRK_CLS").ToString.Trim <> "1" AndAlso drCnstInfo.Item("D39_STEST_DT").ToString.Trim <> String.Empty Then
                '2:本設置以外
                If ViewState(M_VIEW_EST_CLS) <> "2" Then
                    Return "0"
                End If
            Else
                '仮設置・FS稼働無しは無視
                'FS稼働無しは「工事依頼書兼仕様書」又は「定時実行タスク」での更新処理を実施する
                Return "0"
            End If

            If drCnstInfo.Item("D39_CNST_NO").ToString.Substring(0, 5) = "N0090" Then
                Return "0"
            End If

            '連絡区分チェック
            'If drCnstInfo.Item("D39_TELL_CLS").ToString.Trim = "3" Then '3:キャンセル
            '    'キャンセルなので処理無し
            '    Return "0" '0:処理無し
            '    Exit Function
            'End If

            '進捗状況チェック依頼書「07:現場終了」以上かつ進捗「08:終了」の場合のみ更新する。
            Dim strTwinCd As String = drCnstInfo.Item("D39_TWIN_CD").ToString               '双子区分
            Dim strStatus_Dtl As String = ddlSituation.SelectedValue.ToString.Trim          '明細進捗
            Dim intStatus_Main As Integer = drCnstInfo.Item("D39_MTR_STATUS_CD").ToString   '依頼書進捗

            '[双子区分]未入力は0として扱います
            If strTwinCd = String.Empty Then
                strTwinCd = "0"
            End If

            If intStatus_Main < 7 Then
                '入力値：現場未終了
                '3:MDN戻し処理
                Return "3"
            Else
                If strMode = "更新" Then
                    If strStatus_Dtl = "08" Then
                        '1:双子、MDN更新
                        Return "1"
                    Else
                        Return "0"
                    End If
                Else  '削除処理時
                    If intStatus_Main = 7 Then
                        Return "1"
                    Else
                        Return "0"
                    End If
                End If

            End If

        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Throw
        Finally
        End Try
        Return "0"
    End Function

    ''' <summary>
    ''' 双子店区分の更新処理
    ''' </summary>
    ''' <param name="strHallcd">工事依頼書のホールコード</param>
    ''' <returns>True:成功　False:失敗</returns>
    ''' <remarks></remarks>
    Private Function mfUpdTwin(ByVal strHallcd As String, ByVal strTwincd As String, ByVal trans As SqlTransaction, ByVal conDB As SqlConnection) As Boolean
        Dim cmdDB As SqlCommand = Nothing
        Dim intRtn As Integer = 0
        Dim dsHALLInfo As New DataSet
        objStack = New StackFrame

        Try
            'ホール情報取得
            cmdDB = New SqlCommand("CNSUPDP003_S7", conDB)
            cmdDB.CommandType = CommandType.StoredProcedure
            With cmdDB.Parameters
                .Add(pfSet_Param("HALL_CD", SqlDbType.NVarChar, strHallcd))
            End With
            cmdDB.Transaction = trans
            dsHALLInfo = clsDataConnect.pfGet_DataSet(cmdDB)

            'データセット取得チェック
            If dsHALLInfo Is Nothing OrElse dsHALLInfo.Tables(0).Rows().Count = 0 Then
                mfUpdTwin = False 'エラー
                psMesBox(Me, "30029", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ホールが存在しません。", "ホール情報")
                Throw New Exception
                Exit Function
            End If
            Dim strTwinData As String = dsHALLInfo.Tables(0).Rows(0).Item("T01_TWIN_CD").ToString.Trim
            Dim strTwinInput As String = strTwincd.Trim

            '双子区分空欄(又はNULL)は0として扱う
            If strTwinInput = "" Then
                strTwinInput = "0"
            End If
            If strTwinData = "" Then
                strTwinData = "0"
            End If

            '双子店区分に差異があれば更新する。
            If strTwinData <> strTwinInput Then
                cmdDB = New SqlCommand("CNSUPDP003_U8", conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                With cmdDB.Parameters
                    .Add(pfSet_Param("HALL_CD", SqlDbType.NVarChar, strHallcd))
                    .Add(pfSet_Param("TWIN_CD", SqlDbType.NVarChar, strTwinInput))
                    .Add(pfSet_Param("USER_ID", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                End With
                cmdDB.Transaction = trans
                cmdDB.ExecuteNonQuery()
                'ストアド戻り値チェック
                intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                If intRtn <> 0 Then
                    psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "双子店区分更新処理")
                    'ロールバック
                    trans.Rollback()
                    Return False
                End If
                mfUpdTwin = True
            Else
                mfUpdTwin = True
            End If
        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "双子店区分更新処理")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            trans.Rollback()
            Return False
        Finally
        End Try
    End Function

    ''' <summary>
    ''' MDN機器の更新処理
    ''' </summary>
    ''' <param name="drCnstInfo">工事依頼の情報</param>
    ''' <returns>True:成功　False:失敗</returns>
    ''' <remarks></remarks>
    Private Function mfUpdMDN(ByVal drCnstInfo As DataRow, ByVal trans As SqlTransaction, ByVal conDB As SqlConnection) As Boolean

        Dim cmdDB As SqlCommand = Nothing
        Dim intRtn As Integer = 0
        Dim dsMDN As New DataSet
        Dim intMDNCnt As Integer = 0
        Dim strMDNCd As String = ""

        objStack = New StackFrame

        '接続
        Try
            '明細（D40）のMDN明細のみ取得
            cmdDB = New SqlCommand("CNSUPDP003_S8", conDB)
            cmdDB.CommandType = CommandType.StoredProcedure
            With cmdDB.Parameters
                .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, drCnstInfo.Item("D39_CNST_NO").ToString.Trim))
            End With
            cmdDB.Transaction = trans
            dsMDN = clsDataConnect.pfGet_DataSet(cmdDB)

            'データセット取得チェック
            If dsMDN.Tables(0).Rows().Count = 0 Then
                'MDN機器無しなので0台で登録
            Else
                intMDNCnt = dsMDN.Tables(0).Rows(0).Item("MDN台数")
                strMDNCd = dsMDN.Tables(0).Rows(0).Item("MDN機種コード")
            End If

            cmdDB = New SqlCommand("CNSUPDP003_U9", conDB)
            cmdDB.CommandType = CommandType.StoredProcedure
            With cmdDB.Parameters
                .Add(pfSet_Param("TBOX_ID", SqlDbType.NVarChar, drCnstInfo.Item("D39_TBOXID").ToString.Trim))
                .Add(pfSet_Param("HALL_CD", SqlDbType.NVarChar, drCnstInfo.Item("D39_HALL_CD").ToString.Trim))
                .Add(pfSet_Param("MDN_CD", SqlDbType.NVarChar, strMDNCd))
                .Add(pfSet_Param("MDN_CNT", SqlDbType.Int, intMDNCnt))
                .Add(pfSet_Param("USER_ID", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
            End With
            cmdDB.Transaction = trans
            cmdDB.ExecuteNonQuery()
            'ストアド戻り値チェック
            intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
            If intRtn <> 0 Then
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "MDN機種情報更新処理")
                'ロールバック
                trans.Rollback()
                Return False
            End If

            mfUpdMDN = True

        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "MDN機種情報更新処理")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            trans.Rollback()
            Return False
        Finally

        End Try

    End Function

    ''' <summary>
    ''' MDN機器の戻し処理
    ''' </summary>
    ''' <param name="drCnstInfo">工事依頼の情報</param>
    ''' <returns>True:成功　False:失敗</returns>
    ''' <remarks>前回工事終了時の状態にMDN情報を戻します。
    ''' 処理無し（前回工事データ無し）でもTrueを返します。</remarks>
    Private Function mfUpdMDNRev(ByVal drCnstInfo As DataRow, ByVal trans As SqlTransaction, ByVal conDB As SqlConnection) As Boolean

        Dim cmdDB As SqlCommand = Nothing
        Dim intRtn As Integer = 0
        Dim dsTmp As New DataSet
        Dim strCnstNoBefore As String
        Dim dsMDN As New DataSet
        Dim intMDNCnt As Integer = 0
        Dim strMDNCd As String = ""

        objStack = New StackFrame

        '接続
        Try
            '前回の工事依頼番号を取得する。
            cmdDB = New SqlCommand("CNSUPDP003_S9", conDB)
            cmdDB.CommandType = CommandType.StoredProcedure
            With cmdDB.Parameters
                .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, drCnstInfo.Item("D39_CNST_NO").ToString.Trim))
                .Add(pfSet_Param("TBOX_ID", SqlDbType.NVarChar, drCnstInfo.Item("D39_TBOXID").ToString.Trim))
                .Add(pfSet_Param("HALL_CD", SqlDbType.NVarChar, drCnstInfo.Item("D39_HALL_CD").ToString.Trim))
            End With
            cmdDB.Transaction = trans
            dsTmp = clsDataConnect.pfGet_DataSet(cmdDB)
            If dsTmp Is Nothing OrElse dsTmp.Tables(0).Rows().Count = 0 Then
                '前回工事が存在しない為、処理終了
                Return True
            End If
            strCnstNoBefore = dsTmp.Tables(0).Rows(0).Item("前回工事依頼番号")

            '前回工事の明細（D40）のMDN明細のみ取得
            cmdDB = New SqlCommand("CNSUPDP003_S8", conDB)
            cmdDB.CommandType = CommandType.StoredProcedure
            With cmdDB.Parameters
                .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, strCnstNoBefore))
            End With
            cmdDB.Transaction = trans
            dsMDN = clsDataConnect.pfGet_DataSet(cmdDB)

            'データセット取得チェック
            If dsMDN.Tables(0).Rows().Count = 0 Then
                'MDN機器無しなので0台で登録
            Else
                intMDNCnt = dsMDN.Tables(0).Rows(0).Item("MDN台数")
                strMDNCd = dsMDN.Tables(0).Rows(0).Item("MDN機種コード")
            End If

            cmdDB = New SqlCommand("CNSUPDP003_U9", conDB)
            cmdDB.CommandType = CommandType.StoredProcedure
            With cmdDB.Parameters
                .Add(pfSet_Param("TBOX_ID", SqlDbType.NVarChar, drCnstInfo.Item("D39_TBOXID").ToString.Trim))
                .Add(pfSet_Param("HALL_CD", SqlDbType.NVarChar, drCnstInfo.Item("D39_HALL_CD").ToString.Trim))
                .Add(pfSet_Param("MDN_CD", SqlDbType.NVarChar, strMDNCd))
                .Add(pfSet_Param("MDN_CNT", SqlDbType.Int, intMDNCnt))
                .Add(pfSet_Param("USER_ID", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
            End With
            cmdDB.Transaction = trans
            cmdDB.ExecuteNonQuery()
            'ストアド戻り値チェック
            intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)

            If intRtn <> 0 Then
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "MDN機種情報更新処理")
                'ロールバック
                trans.Rollback()
                Return False
            End If
            mfUpdMDNRev = True

        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "MDN機種情報更新処理")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            trans.Rollback()
            Return False
        Finally
            'DB切断
        End Try
    End Function
    'CNSUPDP003-010 END

#End Region

End Class
