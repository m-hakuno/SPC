'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　工事連絡票　参照／更新
'*　ＰＧＭＩＤ：　CNSUPDP004
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.2　：　ＸＸＸ
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'CNSUPDP004-001     2015/09/09      加賀　　　NGCユーザーの場合、一覧選択ボタンの非活性
'CNSUPDP004-002     2015/11/16      加賀　　　NGC回答の文字数制限変更 50→150
'CNSUPDP004-003     2015/12/07      加賀　　　システムの入力をtextbox→dropdownに変更
'CNSUPDP004-004     2015/12/08      加賀　　　明細追加時に入力チェック追加
'CNSUPDP004-005     2016/06/21      栗原　　　レイアウト、発番方法の変更

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
'--------------------------------
'2014/06/11 後藤　ここから
'--------------------------------
Imports SPC.ClsCMExclusive
'--------------------------------
'2014/06/11 後藤　ここまで
'--------------------------------

#End Region

Public Class CNSUPDP004
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
    Const M_MY_DISP_ID = P_FUN_CNS & P_SCR_UPD & P_PAGE & "004"

    'ViewStateの明細選択行キー
    Private Const M_VIEW_SEL = "選択データ"

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
    '接続場所
    Dim strAdd As String = ConfigurationManager.AppSettings("Address")
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
    Dim strDeleteFlg As String = "0"
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
        If strAdd <> P_ADD_NGC Then
            pfSet_GridView(Me.grvList, M_MY_DISP_ID & "_1", 30, 10)
        Else
            pfSet_GridView(Me.grvList, M_MY_DISP_ID & "_2", 30, 10)
        End If
    End Sub

    ''' <summary>
    ''' Page_Loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'ボタンアクションの設定
        AddHandler Me.btnClear.Click, AddressOf btnClear_Click
        AddHandler Me.btnAddition.Click, AddressOf btnAddition_Click
        AddHandler Me.btnUpdateDtl.Click, AddressOf btnWHUpdate_Click
        AddHandler Me.btnDelete.Click, AddressOf btnDelete_Click
        'AddHandler Me.btnUpdate.Click, AddressOf btnUpdate_Click
        'AddHandler Me.btnDelete2.Click, AddressOf btnDelete2_Click
        AddHandler Me.Master.ppRigthButton1.Click, AddressOf btnDelete2_Click   '削除
        AddHandler Me.Master.ppRigthButton2.Click, AddressOf btnUpdate_Click    'SPC更新
        AddHandler Me.Master.ppRigthButton3.Click, AddressOf btnUpdateNGC_Click    'NGC更新

        '工事連絡票情報取得設定
        AddHandler Me.txtCnstNo.ppTextBox.TextChanged, AddressOf txtCnstNo_TextChanged
        Me.txtCnstNo.ppTextBox.AutoPostBack = True
        txtCnstNo.ValidateRequestMode = UI.ValidateRequestMode.Enabled
        dttConstD.ppDateBox.ValidateRequestMode = UI.ValidateRequestMode.Enabled
        AddHandler Me.txtCnstNo.ppTextBox.TextChanged, AddressOf mstxtCnsNoVal
        AddHandler Me.dttConstD.ppDateBox.TextChanged, AddressOf msdttConstDVal
        AddHandler Me.txtCnstH.ppTextBox.TextChanged, AddressOf msTimeVal
        AddHandler Me.txtCnstM.ppTextBox.TextChanged, AddressOf msTimeVal
        Me.txtCnstNo.ppTextBox.AutoPostBack = True
        Me.dttConstD.ppDateBox.AutoPostBack = True
        Me.txtCnstH.ppTextBox.AutoPostBack = True
        Me.txtCnstM.ppTextBox.AutoPostBack = True


        '社員参照条件設定
        'CNSUPDP004-005
        'Me.tbpCnfrm.ppKeyCode = {"8", "3", Nothing}
        'Me.tbpComm.ppKeyCode = {"8", "0", Nothing}
        'CNSUPDP004-005
        Me.txtContent.ppTextBox.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txtContent.ppMaxLength & """);")
        Me.txtContent.ppTextBox.Attributes.Add("onChange", "lenCheck(this,""" & Me.txtContent.ppMaxLength & """);")
        'Me.txtAnswer.ppTextBox.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txtAnswer.ppMaxLength & """);")   'CNSUPDP004-002
        'Me.txtAnswer.ppTextBox.Attributes.Add("onChange", "lenCheck(this,""" & Me.txtAnswer.ppMaxLength & """);")
        Me.txtAnswer.ppTextBox.Attributes.Add("onKeyUp", "lenCheck(this,""" & 150 & """);")
        Me.txtAnswer.ppTextBox.Attributes.Add("onChange", "lenCheck(this,""" & 150 & """);")
        'Me.txtAnswer.ppTextBox.Attributes.Add("maxlength", "170")

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            If Not IsPostBack Then  '初回表示のみ
                '★排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
                If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then
                    Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)
                End If

                '画面設定
                Master.ppProgramID = M_MY_DISP_ID
                Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)
                Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる

                Me.Master.ppRigthButton1.Visible = True
                Me.Master.ppRigthButton1.Enabled = True
                Me.Master.ppRigthButton1.Text = "削除"
                Me.Master.ppRigthButton1.ValidationGroup = "ValMain"
                'Me.Master.ppRigthButton1.BackColor = System.Drawing.color.Fro
                Me.Master.ppRigthButton1.BackColor = Drawing.Color.FromArgb(255, 102, 102)
                Me.Master.ppRigthButton2.Visible = True
                Me.Master.ppRigthButton2.Enabled = True
                Me.Master.ppRigthButton2.Text = "更新"
                Me.Master.ppRigthButton2.ValidationGroup = "ValMain"
                Me.Master.ppRigthButton3.Visible = True
                Me.Master.ppRigthButton3.Enabled = True
                Me.Master.ppRigthButton3.Text = "更新"
                Me.Master.ppRigthButton3.ValidationGroup = "ValNGC"

                Master.ppRigthButton2.Style.Add("margin-right", "600px")
                Master.ppRigthButton3.Style.Add("margin-right", "600px")
                '検索条件クリアボタンと新規作成ボタン押下時の検証を無効
                Me.btnClear.CausesValidation = False

                'パンくずリスト設定
                Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

                'セッション変数「グループナンバー」「遷移条件」「遷移元ＩＤ」が存在しない場合、画面を閉じる
                If Session(P_SESSION_GROUP_NUM) Is Nothing Or Session(P_SESSION_TERMS) Is Nothing Or Session(P_SESSION_OLDDISP) Is Nothing Then
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

                'ViewStateに「グループナンバー」「遷移条件」「遷移元ＩＤ」「キー情報」を保存
                ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)
                ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)
                ViewState(P_SESSION_OLDDISP) = Session(P_SESSION_OLDDISP)
                ViewState(P_KEY) = Session(P_KEY)

                'ドロップダウンリスト設定
                msSet_ddlNGCStatus()
                msSet_ddlSetCls()
                msSetddlSystem()    'CNSUPDP004-003
                msSetddlUser()      'CNSUPDP004-005

                '画面クリア
                msClear_Screen()
                'CNSUPDP004-005
                msClear_Screen_DTL()
                'CNSUPDP004-005 END

                'データ取得
                If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録 Then
                    If ViewState(P_SESSION_OLDDISP) <> "CNSLSTP005" Then
                        msSet_CnstData()
                        msGet_Data_DTL(Me.txtCnstNo.ppText)
                    Else
                        Dim cnst_no As String() = ViewState(P_KEY)
                        If cnst_no.Length > 0 Then
                            If cnst_no(0) <> String.Empty Then
                                Me.txtCnstNo.ppText = cnst_no(0)
                                Call txtCnstNo_TextChanged(Nothing, Nothing)
                            End If
                        End If
                    End If
                Else
                    msSet_CommData()
                    msGet_Data_DTL(Me.txtCnstNo.ppText)
                End If


                If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 Then
                    If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then
                        Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)
                    End If
                End If

                '活性／非活性設定
                msSet_Mode(ViewState(P_SESSION_TERMS))

                If strAdd = "SPC" Then
                    'システム項目にフォーカス設定
                    ddlSystem.ppDropDownList.Focus()
                    btnUpdateNGC.Visible = False
                    'btnUpdate.Visible = True
                    Me.Master.ppRigthButton2.Visible = True
                    Me.Master.ppRigthButton3.Visible = False
                Else
                    '回答日
                    dttAnswerD.ppDateBox.Focus()
                    'btnUpdateNGC.Visible = True
                    'btnUpdate.Visible = False
                    Me.Master.ppRigthButton2.Visible = False
                    Me.Master.ppRigthButton3.Visible = True
                End If

            Else
                'ViewState「遷移条件」が存在しない場合、画面を閉じる
                If ViewState(P_SESSION_TERMS) Is Nothing Then
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

#Region "ユーザー権限"
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

        '管理者のみ仕様可能
        ddlCnfrm.ppEnabled = False
        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
                btnUpdateNGC.Enabled = False
                dttAnswerD.ppEnabled = False
                txtAnswerCharge.ppEnabled = False
                ddlNgcStatus.Enabled = False
                txtAnswer.ppEnabled = False
                '---------------------------
                '2014/06/23 武 ここから
                '---------------------------
                If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照 Then
                    '--------------------------------
                    '2014/06/24 武　ここから
                    '--------------------------------
                    'btnClear.Enabled = True
                    '--------------------------------
                    '2014/06/24 武　ここまで
                    '--------------------------------
                    btnAddition.Enabled = False
                    btnUpdateDtl.Enabled = False
                    btnDelete.Enabled = False
                End If
                '---------------------------
                '2014/06/23 武 ここまで
                '---------------------------
                'CNSUPDP004-005 削除ボタンの活性有無条件追加
                'NGC返答有ならFalse　無しならTrue
                Me.Master.ppRigthButton1.Enabled = Not mfExistNgcAnswer()

                If Me.Master.ppRigthButton2.Text = "登録" Then
                    ddlCnfrm.ppEnabled = False
                Else
                    If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 Then
                        ddlCnfrm.ppEnabled = True
                    End If
                End If
                'CNSUPDP004-005 END
                Me.Master.ppRigthButton3.Enabled = False
            Case "SPC"
                btnUpdateNGC.Enabled = False
                Me.Master.ppRigthButton3.Enabled = False
                dttAnswerD.ppEnabled = False
                txtAnswerCharge.ppEnabled = False
                ddlNgcStatus.Enabled = False
                txtAnswer.ppEnabled = False
                '---------------------------
                '2014/06/23 武 ここから
                '---------------------------
                If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照 Then
                    '--------------------------------
                    '2014/06/24 武　ここから
                    '--------------------------------
                    'btnClear.Enabled = True
                    '--------------------------------
                    '2014/06/24 武　ここまで
                    '--------------------------------
                    btnAddition.Enabled = False
                    btnUpdateDtl.Enabled = False
                    btnDelete.Enabled = False
                End If
                '---------------------------
                '2014/06/23 武 ここまで
                '---------------------------
                'CNSUPDP004-005 削除ボタンの活性有無条件追加
                'NGC返答有ならFalse　無しならTrue
                Me.Master.ppRigthButton1.Enabled = Not mfExistNgcAnswer()
                'tbpCnfrm.ppEnabled = False
                ddlCnfrm.ppEnabled = False
                'CNSUPDP004-005 END
            Case "営業所"
                btnUpdateNGC.Enabled = False
                Me.Master.ppRigthButton3.Enabled = False
                dttAnswerD.ppEnabled = False
                txtAnswerCharge.ppEnabled = False
                ddlNgcStatus.Enabled = False
                txtAnswer.ppEnabled = False
                '---------------------------
                '2014/06/23 武 ここから
                '---------------------------
                If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照 Then
                    '--------------------------------
                    '2014/06/24 武　ここから
                    '--------------------------------
                    'btnClear.Enabled = True
                    '--------------------------------
                    '2014/06/24 武　ここまで
                    '--------------------------------
                    btnAddition.Enabled = False
                    btnUpdateDtl.Enabled = False
                    btnDelete.Enabled = False
                End If
                '---------------------------
                '2014/06/23 武 ここまで
                '---------------------------
                btnDelete2.Visible = False
                'CNSUPDP004-005
                'tbpCnfrm.ppEnabled = False
                ddlCnfrm.ppEnabled = False
                'CNSUPDP004-005 END
                Master.ppRigthButton1.Visible = False
            Case "NGC"
                '---------------------------
                '2014/06/20 武 ここから
                '---------------------------
                'btnUpdate.Enabled = False
                'btnUpdateNGC.Enabled = true
                '---------------------------
                '2014/06/20 武 ここまで
                '---------------------------
                ddlSystem.ppDropDownList.Enabled = False
                ddlCreateUser.ppDropDownList.Enabled = False
                txtCnstNo.ppEnabled = False
                dttConstD.ppEnabled = False
                txtCnstH.ppEnabled = False
                txtCnstM.ppEnabled = False
                'CNSUPDP004-005
                'txtFSCharge.ppEnabled = False
                ddlFSCharge.ppEnabled = False
                'CNSUPDP004-005 END
                txtHallCharge.ppEnabled = False
                txtAgcyCharge.ppEnabled = False
                'CNSUPDP004-005
                ddlCnfrm.ppEnabled = False
                'tbpCnfrm.ppEnabled = False
                'tbpComm.ppEnabled = False
                'CNSUPDP004-005 END
                dtbDt.ppEnabled = False
                'CNSUPDP004-005
                ddlChargeDtl.ppEnabled = False
                'txtChargeDtl.ppEnabled = False
                'CNSUPDP004-005 END
                txtContent.ppEnabled = False
                '---------------------------
                '2014/06/23 武 ここから
                '---------------------------
                'btnClear.Enabled = False
                '---------------------------
                '2014/06/23 武 ここまで
                '---------------------------
                btnAddition.Enabled = False
                btnUpdateDtl.Enabled = False
                btnDelete.Enabled = False
                '---------------------------
                '2014/06/23 武 ここから
                '---------------------------
                If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照 Then
                    '--------------------------------
                    '2014/06/24 武　ここから
                    '--------------------------------
                    'btnClear.Enabled = True
                    '--------------------------------
                    '2014/06/24 武　ここまで
                    '--------------------------------
                    btnAddition.Enabled = False
                    btnUpdateDtl.Enabled = False
                    btnDelete.Enabled = False
                End If
                '---------------------------
                '2014/06/23 武 ここまで
                '---------------------------
                If ViewState(P_SESSION_OLDDISP) = "CNSUPDP003" Then
                    Me.btnUpdateNGC.Enabled = False
                    Me.Master.ppRigthButton3.Enabled = False
                End If
                btnDelete2.Visible = False
                Master.ppRigthButton1.Visible = False
        End Select

        If strDeleteFlg = "1" Then
            psClose_Window(Me)
            strDeleteFlg = "0"
        End If

        If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録 AndAlso Not IsPostBack Then
            'CNSUPDP004-005
            'ddlSystem.ppDropDownList.SelectedValue = String.Empty
            'txtCnstT.ppText = String.Empty
            'CNSUPDP004-005 END
            txtHallCharge.ppText = String.Empty
            txtAgcyCharge.ppText = String.Empty
        Else
            'If btnUpdate.Text <> "登録" Then
            '    ddlSetCls.Enabled = False
            'End If
            If Me.Master.ppRigthButton2.Text <> "登録" Then
                ddlSetCls.Enabled = False
            End If
        End If


    End Sub
    '---------------------------
    '2014/04/23 武 ここまで
    '---------------------------
    Private Sub grvList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grvList.RowDataBound
        'CNSUPDP004-001
        Select Case Session(P_SESSION_AUTH)
            Case "NGC"
                e.Row.Cells(0).Enabled = False  '選択ボタン
        End Select

    End Sub

#End Region

    ''' <summary>
    ''' クリアボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)
        'ログ出力開始
        psLogStart(Me)

        '明細入力項目クリア
        msClear_Screen_DTL()
        '--------------------------------
        '2014/06/24 武　ここから
        '--------------------------------
        If ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.参照 Then
            '--------------------------------
            '2014/06/24 武　ここまで
            '--------------------------------
            btnAddition.Enabled = True
            btnUpdateDtl.Enabled = False
            btnDelete.Enabled = False
            If Session(P_SESSION_AUTH) = "NGC" Then
                btnClear.Enabled = False
            Else
                btnClear.Enabled = True
            End If
            '--------------------------------
            '2014/06/24 武　ここから
            '--------------------------------
        Else
            btnClear.Enabled = False
            btnAddition.Enabled = False
            btnUpdateDtl.Enabled = False
            btnDelete.Enabled = False
        End If
        '--------------------------------
        '2014/06/24 武　ここまで
        '--------------------------------
        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 追加ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAddition_Click(sender As Object, e As EventArgs)
        '開始ログ出力
        psLogStart(Me)

        '内容の改行コードを取り除く
        Me.txtContent.ppText = Me.txtContent.ppText.Replace(Environment.NewLine, "")

        'CNSUPDP004-004
        If dtbDt.ppDateBox.Text = String.Empty Then
            dtbDt.ppRequiredField = True
            dtbDt.psSet_ErrorNo("5001", lblDt.Text)
        End If
        If dtbDt.ppHourBox.Text = String.Empty Then
            dtbDt.ppRequiredField = True
            dtbDt.psSet_ErrorNo("5001", lblDt.Text)
        End If
        If dtbDt.ppMinBox.Text = String.Empty Then
            dtbDt.ppRequiredField = True
            dtbDt.psSet_ErrorNo("5001", lblDt.Text)
        End If
        'CNSUPDP004-005
        'If txtChargeDtl.ppText = String.Empty Then
        '    txtChargeDtl.ppRequiredField = True
        '    txtChargeDtl.psSet_ErrorNo("5001", lblChargeDtl.Text)
        'End If
        'CNSUPDP004-005 END
        If txtContent.ppText = String.Empty Then
            txtContent.ppRequiredField = True
            txtContent.psSet_ErrorNo("5001", lblContent.Text)
        End If
        'CNSUPDP004-004 END

        If (Page.IsValid) Then
            Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
            Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
            Dim dstOrders As DataSet = Nothing          'データセット
            Dim intRtn As Integer
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
                    cmdDB = New SqlCommand(M_MY_DISP_ID + "_I1", conDB)
                    'パラメータ設定
                    With cmdDB.Parameters
                        .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, Me.txtCnstNo.ppText))           '工事依頼番号
                        .Add(pfSet_Param("dt_d", SqlDbType.NVarChar, Me.dtbDt.ppText))                  '日時（日）
                        .Add(pfSet_Param("dt_t_h", SqlDbType.NVarChar, Me.dtbDt.ppHourText))            '日時（時）
                        .Add(pfSet_Param("dt_t_m", SqlDbType.NVarChar, Me.dtbDt.ppMinText))             '日時（分）
                        'CNSUPDP004-005
                        .Add(pfSet_Param("charge", SqlDbType.NVarChar, Me.ddlChargeDtl.ppSelectedText))         '担当者
                        '.Add(pfSet_Param("charge", SqlDbType.NVarChar, Me.txtChargeDtl.ppText))         '担当者
                        'CNSUPDP004-005 END
                        .Add(pfSet_Param("content", SqlDbType.NVarChar, Me.txtContent.ppText))          '内容
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))             'ユーザーＩＤ
                        .Add(pfSet_Param("set_cls", SqlDbType.NVarChar, Me.ddlSetCls.SelectedValue))    '設置区分
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                               '戻り値
                    End With

                    'データ追加
                    Using conTrn = conDB.BeginTransaction
                        cmdDB.Transaction = conTrn
                        dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, "00008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事連絡票明細", intRtn.ToString)
                            Exit Sub
                        End If

                        'コミット
                        conTrn.Commit()

                    End Using

                    '完了メッセージ
                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "工事連絡票明細")

                    '明細入力項目クリア
                    msClear_Screen_DTL()

                    '明細データ再取得
                    msGet_Data_DTL(Me.txtCnstNo.ppText)

                Catch ex As Exception
                    'データ追加処理エラー
                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事連絡票明細")
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
    ''' 明細更新処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnWHUpdate_Click(sender As Object, e As EventArgs)
        Dim strSelRowSeqNo As String = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '選択情報取得
        strSelRowSeqNo = TryCast(ViewState(M_VIEW_SEL), String)
        If strSelRowSeqNo Is Nothing Then       '選択行なし
            Exit Sub
        End If

        '開始ログ出力
        psLogStart(Me)

        '内容の改行コードを取り除く
        Me.txtContent.ppText = Me.txtContent.ppText.Replace(Environment.NewLine, "")

        If dtbDt.ppDateBox.Text = String.Empty Then
            dtbDt.ppRequiredField = True
            dtbDt.psSet_ErrorNo("5001", lblDt.Text)
        End If
        If dtbDt.ppHourBox.Text = String.Empty Then
            dtbDt.ppRequiredField = True
            dtbDt.psSet_ErrorNo("5001", lblDt.Text)
        End If
        If dtbDt.ppMinBox.Text = String.Empty Then
            dtbDt.ppRequiredField = True
            dtbDt.psSet_ErrorNo("5001", lblDt.Text)
        End If
        'CNSUPDP004-005
        'If txtChargeDtl.ppText = String.Empty Then
        '    txtChargeDtl.ppRequiredField = True
        '    txtChargeDtl.psSet_ErrorNo("5001", lblChargeDtl.Text)
        'End If
        'CNSUPDP004-005 END
        If txtContent.ppText = String.Empty Then
            txtContent.ppRequiredField = True
            txtContent.psSet_ErrorNo("5001", lblContent.Text)
        End If

        If (Page.IsValid) Then
            Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
            Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
            Dim dstOrders As DataSet = Nothing          'データセット
            Dim intRtn As Integer

            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    cmdDB = New SqlCommand(M_MY_DISP_ID + "_U1", conDB)
                    'パラメータ設定
                    With cmdDB.Parameters
                        .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, Me.txtCnstNo.ppText))            '工事依頼番号
                        .Add(pfSet_Param("dt_d", SqlDbType.NVarChar, Me.dtbDt.ppText))                   '日時（日）
                        .Add(pfSet_Param("dt_t_h", SqlDbType.NVarChar, Me.dtbDt.ppHourText))             '日時（時）
                        .Add(pfSet_Param("dt_t_m", SqlDbType.NVarChar, Me.dtbDt.ppMinText))              '日時（分）
                        'CNSUPDP004-005
                        .Add(pfSet_Param("charge", SqlDbType.NVarChar, Me.ddlChargeDtl.ppSelectedText))  '担当者
                        '.Add(pfSet_Param("charge", SqlDbType.NVarChar, Me.txtChargeDtl.ppText))         '担当者
                        'CNSUPDP004-005 END
                        .Add(pfSet_Param("content", SqlDbType.NVarChar, Me.txtContent.ppText))           '内容
                        .Add(pfSet_Param("seqno", SqlDbType.NVarChar, strSelRowSeqNo))                   '連番
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))              'ユーザーＩＤ
                        .Add(pfSet_Param("set_cls", SqlDbType.NVarChar, Me.ddlSetCls.SelectedValue))     '設置区分
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                '戻り値
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
                            psMesBox(Me, "00007", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事連絡票明細", intRtn.ToString)
                            Exit Sub
                        End If

                        'コミット
                        conTrn.Commit()

                    End Using

                    '完了メッセージ
                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "工事連絡票明細")

                    'フォーカス設定
                    Me.dtbDt.ppDateBox.Focus()

                    '明細データ再取得
                    msGet_Data_DTL(Me.txtCnstNo.ppText)

                    '明細入力項目クリア
                    msClear_Screen_DTL()

                Catch ex As Exception
                    'データ更新処理エラー
                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事連絡票明細")
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
    ''' 削除ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDelete_Click(sender As Object, e As EventArgs)
        Dim strSelRowSeqNo As String = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '選択情報取得
        strSelRowSeqNo = TryCast(ViewState(M_VIEW_SEL), String)
        If strSelRowSeqNo Is Nothing Then   '選択行なし
            Exit Sub
        End If

        '開始ログ出力
        psLogStart(Me)

        If dtbDt.ppDateBox.Text = String.Empty Then
            dtbDt.ppRequiredField = True
            dtbDt.psSet_ErrorNo("5001", lblDt.Text)
        End If
        If dtbDt.ppHourBox.Text = String.Empty Then
            dtbDt.ppRequiredField = True
            dtbDt.psSet_ErrorNo("5001", lblDt.Text)
        End If
        If dtbDt.ppMinBox.Text = String.Empty Then
            dtbDt.ppRequiredField = True
            dtbDt.psSet_ErrorNo("5001", lblDt.Text)
        End If
        'CNSUPDP004-005
        'If txtChargeDtl.ppText = String.Empty Then
        '    txtChargeDtl.ppRequiredField = True
        '    txtChargeDtl.psSet_ErrorNo("5001", lblChargeDtl.Text)
        'End If
        'CNSUPDP004-005 END
        If txtContent.ppText = String.Empty Then
            txtContent.ppRequiredField = True
            txtContent.psSet_ErrorNo("5001", lblContent.Text)
        End If

        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim intRtn As Integer

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_D1", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, Me.txtCnstNo.ppText))           '工事依頼番号
                    .Add(pfSet_Param("dt_d", SqlDbType.NVarChar, Me.dtbDt.ppText))                  '日時（日）
                    .Add(pfSet_Param("dt_t_h", SqlDbType.NVarChar, Me.dtbDt.ppHourText))            '日時（時）
                    .Add(pfSet_Param("dt_t_m", SqlDbType.NVarChar, Me.dtbDt.ppMinText))             '日時（分）
                    'CNSUPDP004-005
                    .Add(pfSet_Param("charge", SqlDbType.NVarChar, Me.ddlChargeDtl.ppSelectedText))         '担当者
                    '.Add(pfSet_Param("charge", SqlDbType.NVarChar, Me.txtChargeDtl.ppText))         '担当者
                    'CNSUPDP004-005 END
                    .Add(pfSet_Param("content", SqlDbType.NVarChar, Me.txtContent.ppText))          '内容
                    .Add(pfSet_Param("seqno", SqlDbType.NVarChar, ViewState(M_VIEW_SEL)))           '連番
                    .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))             'ユーザーＩＤ
                    .Add(pfSet_Param("set_cls", SqlDbType.NVarChar, Me.ddlSetCls.SelectedValue))    '設置区分
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                               '戻り値
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
                        psMesBox(Me, "00009", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事連絡票明細", intRtn.ToString)
                        Exit Sub
                    End If

                    'コミット
                    conTrn.Commit()

                End Using

                '完了メッセージ
                psMesBox(Me, "00002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "工事連絡票明細")

                '明細入力項目クリア
                msClear_Screen_DTL()

                '明細データ再取得
                msGet_Data_DTL(Me.txtCnstNo.ppText)

                btnClear.Enabled = True
                btnAddition.Enabled = True
                btnUpdateDtl.Enabled = False
                btnDelete.Enabled = False

            Catch ex As Exception
                'データ削除処理エラー
                psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事連絡票明細")
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

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 更新ボタンクリック(NGCモード)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnUpdateNGC_Click(ByVal sender As Object, ByVal e As System.Object) Handles btnUpdateNGC.Click

        '開始ログ出力
        psLogStart(Me)

        '入力チェック
        msCheck_Error("NGC")

        '更新処理
        msUpdateMethod()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 登録／更新処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs)
        Dim dtsCnsNoData As DataSet = Nothing
        '開始ログ出力
        psLogStart(Me)

        '個別整合性チェック
        If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録 Then
            msCheck_Error("SPC")
        End If

        If Me.Master.ppRigthButton2.Text = "登録" Then
            mfGet_CnstNo(Me.txtCnstNo.ppText)
            mfGet_CnsNoInfo(Me.txtCnstNo.ppText, dtsCnsNoData)
        End If

        msErrCheck()

        '更新処理
        msUpdateMethod()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 削除ボタンクリック(全体)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDelete2_Click(ByVal sender As Object, ByVal e As System.Object) Handles btnDelete2.Click

        '開始ログ出力
        psLogStart(Me)

        '削除処理
        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim intRtn As Integer

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_D2", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, Me.txtCnstNo.ppText))           '工事依頼番号
                    .Add(pfSet_Param("set_cls", SqlDbType.NVarChar, Me.ddlSetCls.SelectedValue))    '設置区分
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                               '戻り値
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
                        psMesBox(Me, "00009", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事連絡票", intRtn.ToString)
                        Exit Sub
                    End If

                    'コミット
                    conTrn.Commit()

                End Using

                '完了メッセージ
                psMesBox(Me, "10003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画前, "工事連絡票")

                '明細入力項目クリア
                msClear_Screen_DTL()

                '明細データ再取得
                msGet_Data_DTL(Me.txtCnstNo.ppText)

                btnClear.Enabled = True
                btnAddition.Enabled = True
                btnUpdateDtl.Enabled = False
                btnDelete.Enabled = False
                strDeleteFlg = "1"

            Catch ex As Exception
                'データ削除処理エラー
                psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事連絡票")
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

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 工事依頼番号変更時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtCnstNo_TextChanged(sender As Object, e As EventArgs)
        '--------------------------------
        '2014/04/14 小守　ここから
        '--------------------------------
        'Dim intRtn As Integer

        '開始ログ出力
        psLogStart(Me)

        'intRtn = mfGet_CnstNo(Me.txtCnstNo.ppText)
        'If intRtn <> 0 Then
        '    Exit Sub
        'End If

        msSet_CnsNoInfo()

        '終了ログ出力
        psLogEnd(Me)
        '--------------------------------
        '2014/04/14 小守　ここまで
        '--------------------------------
    End Sub

    ''' <summary>
    ''' グリッドのボタン押下時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand
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
            Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    'ボタン押下行のIndex
            Dim rowData As GridViewRow = grvList.Rows(intIndex)             'ボタン押下行

            dtbDt.ppText = CType(rowData.FindControl("日時"), TextBox).Text.Replace(Environment.NewLine, "").Substring(0, 10)
            dtbDt.ppHourText = CType(rowData.FindControl("日時"), TextBox).Text.Replace(Environment.NewLine, "").Substring(11, 2)
            dtbDt.ppMinText = CType(rowData.FindControl("日時"), TextBox).Text.Replace(Environment.NewLine, "").Substring(14, 2)
            'CNSUPDP004-005
            If ddlChargeDtl.ppDropDownList.Items.FindByText(CType(rowData.FindControl("担当者"), TextBox).Text) Is Nothing Then
                Me.ddlChargeDtl.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing))
                ddlChargeDtl.ppDropDownList.SelectedIndex = 1
                ddlChargeDtl.ppDropDownList.Items.Item(1).Value = "00"
                ddlChargeDtl.ppDropDownList.Items.Item(1).Text = CType(rowData.FindControl("担当者"), TextBox).Text
            Else
                Me.ddlChargeDtl.ppSelectedValue = ddlChargeDtl.ppDropDownList.Items.FindByText(CType(rowData.FindControl("担当者"), TextBox).Text).Value
            End If
            'txtChargeDtl.ppText = CType(rowData.FindControl("担当者"), TextBox).Text
            'CNSUPDP004-005 END
            txtContent.ppText = CType(rowData.FindControl("内容"), TextBox).Text.Replace(Environment.NewLine, "")
            ViewState(M_VIEW_SEL) = CType(rowData.FindControl("連番"), TextBox).Text

            btnClear.Enabled = True
            btnAddition.Enabled = False
            If CType(rowData.FindControl("削除フラグ"), TextBox).Text.Trim = "●" Then
                btnDelete.Enabled = False
                btnUpdateDtl.Enabled = False
            Else
                btnDelete.Enabled = True
                btnUpdateDtl.Enabled = True
            End If

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
        End Try
        '--------------------------------
        '2014/06/24 武　ここから
        '--------------------------------
        If Session(P_SESSION_AUTH) = "NGC" Then
            Me.btnClear.Enabled = True
        End If
        '--------------------------------
        '2014/06/24 武　ここまで
        '--------------------------------
        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound
        Dim strDelF As String

        If strAdd <> P_ADD_NGC Then
            For Each rowData As GridViewRow In grvList.Rows
                If (rowData.RowIndex Mod 2) = 1 Then
                    CType(rowData.FindControl("日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("担当者"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("内容"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("削除フラグ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("連番"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                Else
                    CType(rowData.FindControl("日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                    CType(rowData.FindControl("担当者"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                    CType(rowData.FindControl("内容"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                    CType(rowData.FindControl("削除フラグ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                    CType(rowData.FindControl("連番"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                End If
            Next
        Else
            For Each rowData As GridViewRow In grvList.Rows
                If (rowData.RowIndex Mod 2) = 1 Then
                    CType(rowData.FindControl("日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("担当者"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("内容"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                Else
                    CType(rowData.FindControl("日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                    CType(rowData.FindControl("担当者"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                    CType(rowData.FindControl("内容"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                End If
            Next
        End If

        If strAdd = P_ADD_SPC Then
            For Each rowData As GridViewRow In grvList.Rows
                strDelF = CType(rowData.FindControl("削除フラグ"), TextBox).Text.Trim
                If strDelF = "●" Then   '削除フラグあり
                    rowData.Cells(0).Enabled = False
                End If
            Next
        End If
    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClear_Screen()
        Me.lblCommNo2.Text = String.Empty                   '連絡票管理番号
        Me.ddlSystem.ppDropDownList.SelectedValue = String.Empty                  'システム
        'CNSUPDP004-005 
        Me.ddlCreateUser.ppDropDownList.SelectedValue = String.Empty
        'Me.txtCharge.ppText = String.Empty                  '作成者
        'CNSUPDP004-005 END
        Me.lblTboxId2.Text = String.Empty                   'ＴＢＯＸＩＤ
        Me.lblHallNm2.Text = String.Empty                   'ホール名
        Me.txtCnstNo.ppText = String.Empty                  '工事依頼番号
        Me.dttConstD.ppText = String.Empty                  '作業実施日
        Me.txtCnstH.ppText = String.Empty                   '作業実施時間
        Me.txtCnstM.ppText = String.Empty                   '作業実施時間
        Me.lblNew2.Text = String.Empty                      '工事種別　新規
        Me.lblAdd2.Text = String.Empty                      '　　　　　増設
        Me.lblReset2.Text = String.Empty                    '　　　　　再設置
        Me.lblRelocate2.Text = String.Empty                 '　　　　　店内移設
        Me.lblPrtRemove2.Text = String.Empty                '　　　　　一部撤去
        Me.lblAllRemove2.Text = String.Empty                '　　　　　全撤去
        Me.lblTmpRemove2.Text = String.Empty                '　　　　　一時撤去
        Me.lblChngOrgnz2.Text = String.Empty                '　　　　　構成変更
        Me.lblDlvOrgnz2.Text = String.Empty                 '　　　　　構成配信
        Me.lblVup2.Text = String.Empty                      '　　　　　バージョンアップ
        Me.lblOth2.Text = String.Empty                      '　　　　　その他
        Me.lblOthDtl.Text = String.Empty                    'その他工事内容
        'CNSUPDP004-005
        'Me.txtFSCharge.ppText = String.Empty                'ＦＳ担当者
        Me.ddlFSCharge.ppSelectedValue = String.Empty
        'CNSUPDP004-005 END
        Me.txtHallCharge.ppText = String.Empty              'ホール担当者
        Me.txtAgcyCharge.ppText = String.Empty              '代理店担当者
        'CNSUPDP004-005
        Me.ddlCnfrm.ppSelectedValue = String.Empty
        'Me.tbpCnfrm.ppText = String.Empty                   '確認者
        'CNSUPDP004-005 END
        'Me.tbpComm.ppText = String.Empty                    '連絡先'CNSUPDP004-005
        Me.dtbDt.ppText = String.Empty                      '日時（日）
        Me.dtbDt.ppHourText = String.Empty                  '日時（時）
        Me.dtbDt.ppMinText = String.Empty                   '日時（分）
        'CNSUPDP004-005
        Me.ddlChargeDtl.ppSelectedValue = String.Empty
        'Me.txtChargeDtl.ppText = String.Empty               '担当者
        'CNSUPDP004-005 END
        Me.txtContent.ppText = String.Empty                 '内容
        Me.dttAnswerD.ppText = String.Empty                 '回答日
        Me.txtAnswerCharge.ppText = String.Empty            '回答者
        Me.ddlNgcStatus.SelectedValue = String.Empty        '発生状況
        Me.txtAnswer.ppText = String.Empty                  '回答
        Me.ddlSetCls.SelectedIndex = 0                      '設置区分

        Me.grvList.DataSource = New DataTable
        Me.grvList.DataBind()

    End Sub

    ''' <summary>
    ''' 画面明細クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClear_Screen_DTL()
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try

            Me.dtbDt.ppText = String.Empty              '日時（日）
            Me.dtbDt.ppHourText = String.Empty          '日時（時）
            Me.dtbDt.ppMinText = String.Empty           '日時（分）
            'CNSUPDP004-005
            Me.ddlChargeDtl.ppSelectedValue = String.Empty
            'Me.txtChargeDtl.ppText = String.Empty       '担当者
            'CNSUPDP004-005 END
            Me.txtContent.ppText = String.Empty         '内容
            ViewState(M_VIEW_SEL) = Nothing

            'CNSUPDP004-005
            '明細時刻セット
            Dim strDate As String = DateTime.Now.ToString("yyyy/MM/ddHHmm") '現在日時

            If strDate.Length < 14 Then
                Me.dtbDt.ppText = String.Empty
                Me.dtbDt.ppHourText = String.Empty
                Me.dtbDt.ppMinText = String.Empty
            Else
                Me.dtbDt.ppText = strDate.Substring(0, 10)
                Me.dtbDt.ppHourText = strDate.Substring(10, 2)
                Me.dtbDt.ppMinText = strDate.Substring(12, 2)
            End If
            'CNSUPDP004-005 END

            Me.dtbDt.ppDateBox.Focus()

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
    End Sub

    ''' <summary>
    ''' データ取得処理(連絡票管理番号)
    ''' </summary>
    ''' <param name="ipstrKey">キー情報</param>
    ''' <param name="opdstCommData">工事連絡票項目</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_CommData(ByVal ipstrKey As String, ByRef opdstCommData As DataSet) As Boolean
        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGet_CommData = False

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_S1", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    '連絡票管理番号
                    .Add(pfSet_Param("comm_no", SqlDbType.NVarChar, ipstrKey))
                End With

                'データ取得
                opdstCommData = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ有無確認
                If opdstCommData.Tables(0).Rows.Count = 0 Then
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事連絡票")
                    Exit Function
                End If

                mfGet_CommData = True

            Catch ex As Exception
                'データ取得処理エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事連絡票情報取得")
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
    ''' 連絡票管理番号情報の設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_CommData()
        Dim dtsCommData As DataSet = Nothing
        Dim dtRow As DataRow = Nothing              'DataRow
        Dim strKey() As String = Nothing

        'キー項目取得
        strKey = ViewState(P_KEY)

        '連絡票管理番号情報取得
        If mfGet_CommData(strKey(0).ToString, dtsCommData) Then

            dtRow = dtsCommData.Tables(0).Rows(0)

            '連絡票管理番号情報設定
            Me.lblCommNo2.Text = strKey(0).ToString
            Me.ddlSystem.ppDropDownList.SelectedValue = dtRow("システム").ToString
            'CNSUPDP004-005 

            If ddlCreateUser.ppDropDownList.Items.FindByText(dtRow("作成者").ToString) Is Nothing Then
                Me.ddlCreateUser.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing))
                ddlCreateUser.ppDropDownList.SelectedIndex = 1
                ddlCreateUser.ppDropDownList.Items.Item(1).Value = "00"
                ddlCreateUser.ppDropDownList.Items.Item(1).Text = dtRow("作成者").ToString
            Else
                Me.ddlCreateUser.ppDropDownList.SelectedValue = ddlCreateUser.ppDropDownList.Items.FindByText(dtRow("作成者").ToString).Value
            End If
            'Me.txtCharge.ppText = dtRow("作成者").ToString
            'CNSUPDP004-005 END
            Me.lblTboxId2.Text = dtRow("ＴＢＯＸＩＤ").ToString
            Me.lblHallNm2.Text = dtRow("ホール名").ToString
            Me.txtCnstNo.ppText = dtRow("工事依頼番号").ToString
            Me.dttConstD.ppText = dtRow("作業実施日").ToString
            If dtRow("作業実施時間").ToString.Trim <> String.Empty Then
                Me.txtCnstH.ppText = dtRow("作業実施時間").ToString.Split(":")(0).Trim
                Me.txtCnstM.ppText = dtRow("作業実施時間").ToString.Split(":")(1).Trim
            Else
                Me.txtCnstH.ppText = String.Empty
                Me.txtCnstM.ppText = String.Empty
            End If

            Me.lblNew2.Text = dtRow("新規").ToString
            Me.lblAdd2.Text = dtRow("増設").ToString
            Me.lblReset2.Text = dtRow("再設置").ToString
            Me.lblRelocate2.Text = dtRow("店内移設").ToString
            Me.lblPrtRemove2.Text = dtRow("一部撤去").ToString
            Me.lblAllRemove2.Text = dtRow("全撤去").ToString
            Me.lblTmpRemove2.Text = dtRow("一時撤去").ToString
            Me.lblChngOrgnz2.Text = dtRow("構成変更").ToString
            Me.lblDlvOrgnz2.Text = dtRow("構成配信").ToString
            Me.lblVup2.Text = dtRow("ＶＵＰ").ToString
            Me.lblOth2.Text = dtRow("その他").ToString
            Me.lblOthDtl.Text = dtRow("その他工事内容").ToString
            'CNSUPDP004-005
            'Me.txtFSCharge.ppText = dtRow("ＦＳ担当者").ToString
            If ddlFSCharge.ppDropDownList.Items.FindByText(dtRow("ＦＳ担当者").ToString) Is Nothing Then
                Me.ddlFSCharge.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing))
                ddlFSCharge.ppDropDownList.SelectedIndex = 1
                ddlFSCharge.ppDropDownList.Items.Item(1).Value = "00"
                ddlFSCharge.ppDropDownList.Items.Item(1).Text = dtRow("ＦＳ担当者").ToString
            Else
                Me.ddlFSCharge.ppSelectedValue = ddlFSCharge.ppDropDownList.Items.FindByText(dtRow("ＦＳ担当者").ToString).Value
            End If
            'CNSUPDP004-005 END
            Me.txtHallCharge.ppText = dtRow("ホール担当者").ToString
            Me.txtAgcyCharge.ppText = dtRow("代理店担当者").ToString
            'CNSUPDP004-005
            If ddlCnfrm.ppDropDownList.Items.FindByText(dtRow("確認者").ToString) Is Nothing Then
                Me.ddlCnfrm.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing))
                ddlCnfrm.ppDropDownList.SelectedIndex = 1
                ddlCnfrm.ppDropDownList.Items.Item(1).Value = "00"
                ddlCnfrm.ppDropDownList.Items.Item(1).Text = dtRow("確認者").ToString
            Else
                Me.ddlCnfrm.ppSelectedValue = ddlCnfrm.ppDropDownList.Items.FindByText(dtRow("確認者").ToString).Value
            End If
            'Me.tbpCnfrm.ppText = dtRow("確認者").ToString
            'Me.tbpComm.ppText = dtRow("連絡者").ToString
            'CNSUPDP004-005 END
            Me.dttAnswerD.ppText = dtRow("回答日").ToString
            Me.txtAnswerCharge.ppText = dtRow("回答者").ToString
            Me.ddlNgcStatus.SelectedValue = dtRow("進捗状況コード").ToString
            Me.txtAnswer.ppText = dtRow("回答").ToString
            Me.ddlSetCls.SelectedValue = dtRow("設置区分").ToString

            Me.ddlSystem.ppDropDownList.Focus()

        End If
    End Sub

    ''' <summary>
    ''' データ取得処理(工事依頼票番号)
    ''' </summary>
    ''' <param name="ipstrKey">キー情報</param>
    ''' <param name="opdstConstData">工事依頼番号項目</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_CnstData(ByVal ipstrKey As String, ByVal ipstrKey2 As String, ByRef opdstConstData As DataSet) As Boolean
        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGet_CnstData = False

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_S4", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrKey))     '工事依頼番号
                    .Add(pfSet_Param("set_cls", SqlDbType.NVarChar, ipstrKey2))    '設置区分
                End With

                'データ取得
                opdstConstData = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ表示
                If opdstConstData.Tables(0).Rows.Count = 0 Then
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "工事連絡票")

                End If

                mfGet_CnstData = True

            Catch ex As Exception
                'データ取得処理エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事連絡票")
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
    ''' 工事依頼番号情報の設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_CnstData()
        Dim dtsConstData As DataSet = Nothing
        Dim dtRow As DataRow = Nothing              'DataRow
        Dim strKey() As String = Nothing

        'キー項目取得
        strKey = ViewState(P_KEY)

        '工事依頼番号情報取得
        If mfGet_CnstData(strKey(0).ToString, strKey(1).ToString, dtsConstData) Then

            dtRow = dtsConstData.Tables(0).Rows(0)

            '工事依頼番号情報設定
            'CNSUPDP004-003
            'Dim index As Integer
            'If dtRow("システム").ToString.Contains("-") Then
            '    index = dtRow("システム").ToString.IndexOf("-")
            '    Me.txtSystem.ppText = dtRow("システム").ToString.Substring(0, index)
            'Else
            '    Me.txtSystem.ppText = dtRow("システム").ToString
            'End If
            Me.ddlSystem.ppDropDownList.SelectedValue = dtRow("システム").ToString
            'CNSUPDP004-003 END

            Me.lblTboxId2.Text = dtRow("ＴＢＯＸＩＤ").ToString
            Me.lblHallNm2.Text = dtRow("ホール名").ToString
            Me.txtCnstNo.ppText = strKey(0).ToString
            Me.dttConstD.ppText = dtRow("作業実施日").ToString
            If dtRow("作業実施時間").ToString.Trim <> String.Empty Then
                Me.txtCnstH.ppText = dtRow("作業実施時間").ToString.Split(":")(0).Trim
                Me.txtCnstM.ppText = dtRow("作業実施時間").ToString.Split(":")(1).Trim
            Else
                Me.txtCnstH.ppText = String.Empty
                Me.txtCnstM.ppText = String.Empty
            End If

            Me.lblNew2.Text = dtRow("新規").ToString
            Me.lblAdd2.Text = dtRow("増設").ToString
            Me.lblPrtRemove2.Text = dtRow("一部撤去").ToString
            Me.lblRelocate2.Text = dtRow("店内移設").ToString
            Me.lblAllRemove2.Text = dtRow("全撤去").ToString
            Me.lblTmpRemove2.Text = dtRow("一時撤去").ToString
            Me.lblReset2.Text = dtRow("再設置").ToString
            Me.lblChngOrgnz2.Text = dtRow("構成変更").ToString
            Me.lblDlvOrgnz2.Text = dtRow("構成配信").ToString
            Me.lblVup2.Text = dtRow("ＶＵＰ").ToString
            Me.lblOth2.Text = dtRow("その他").ToString
            Me.lblOthDtl.Text = dtRow("その他工事内容").ToString
            'CNSUPDP004-005
            If ddlFSCharge.ppDropDownList.Items.FindByText(dtRow("ＦＳ担当者").ToString) Is Nothing Then
                Me.ddlFSCharge.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing))
                ddlFSCharge.ppDropDownList.SelectedIndex = 1
                ddlFSCharge.ppDropDownList.Items.Item(1).Value = "00"
                ddlFSCharge.ppDropDownList.Items.Item(1).Text = dtRow("ＦＳ担当者").ToString
            Else
                Me.ddlFSCharge.ppDropDownList.SelectedValue = ddlFSCharge.ppDropDownList.Items.FindByText(dtRow("ＦＳ担当者").ToString).Value
            End If
            'Me.ddlFSCharge.ppDropDownList.Text = dtRow("ＦＳ担当者").ToString
            'Me.txtFSCharge.ppText = dtRow("ＦＳ担当者").ToString
            'If ddlCreateUser.ppDropDownList.Items.FindByText(dtRow("登録者").ToString) Is Nothing Then
            '    Me.ddlCreateUser.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing))
            '    ddlCreateUser.ppDropDownList.SelectedIndex = 1
            '    ddlCreateUser.ppDropDownList.Items.Item(1).Value = "00"
            '    ddlCreateUser.ppDropDownList.Items.Item(1).Text = dtRow("登録者").ToString
            'Else
            '    Me.ddlCreateUser.ppDropDownList.SelectedValue = ddlCreateUser.ppDropDownList.Items.FindByText(dtRow("登録者").ToString).Value
            'End If
            'CNSUPDP004-005
            Me.txtHallCharge.ppText = dtRow("ホール担当者").ToString
            Me.txtAgcyCharge.ppText = dtRow("代理店担当者").ToString
            Me.ddlSetCls.SelectedValue = strKey(1)

            If dtRow("TPCNAME").ToString.Substring(0, 3) = "TPC" Then
                Me.lblTPCtitle.Text = "登録ｼｽﾃﾑ"
                Me.lblTPCName.Text = dtRow("TPCNAME").ToString
                Me.lblTPCVer.Text = dtRow("TPCVER").ToString
            Else
                Me.lblTPCtitle.Text = ""
                Me.lblTPCName.Text = ""
                Me.lblTPCVer.Text = ""
            End If

            Me.ddlSystem.ppDropDownList.Focus()
            ViewState(P_KEY) = Me.txtCnstNo.ppText

            '--------------------------------
            '2014/04/14 小守　ここから
            '--------------------------------
            '工事依頼番号非活性
            Me.txtCnstNo.ppTextBox.Enabled = False
            '--------------------------------
            '2014/04/14 小守　ここまで
            '--------------------------------

        End If
    End Sub

    ''' <summary>
    ''' 明細データ取得処理
    ''' </summary>
    ''' <param name="ipCnstNo">工事依頼番号</param>
    ''' <remarks></remarks>
    Private Sub msGet_Data_DTL(ByVal ipCnstNo As String)
        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット
        Dim dtRow As DataRow = Nothing              'DataRow
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
                'ＳＰＣ側とＮＧＣ側でデータの取得条件を変える
                If strAdd <> P_ADD_NGC Then
                    cmdDB = New SqlCommand(M_MY_DISP_ID + "_S2", conDB)
                Else
                    cmdDB = New SqlCommand(M_MY_DISP_ID + "_S3", conDB)
                End If
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipCnstNo))     '工事依頼番号
                    .Add(pfSet_Param("set_cls", SqlDbType.NVarChar, Me.ddlSetCls.SelectedValue))     '設置区分
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得したデータをグリッドに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                'データ取得処理エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事連絡票明細")
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
        End If
    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（進捗ステータスマスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_ddlNGCStatus()
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
                cmdDB = New SqlCommand("ZCMPSEL015", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("duty_cd", SqlDbType.NVarChar, "24"))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'ドロップダウンリスト設定
                Me.ddlNgcStatus.Items.Clear()
                Me.ddlNgcStatus.DataSource = dstOrders.Tables(0)
                Me.ddlNgcStatus.DataTextField = "進捗ステータス名"
                Me.ddlNgcStatus.DataValueField = "進捗ステータス"
                Me.ddlNgcStatus.DataBind()
                Me.ddlNgcStatus.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加

            Catch ex As Exception
                'データ取得処理エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "進捗ステータスマスタ一覧取得")
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

    ''' <summary>
    ''' ドロップダウンリスト設定（設置区分）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_ddlSetCls()
        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット
        objStack = New StackFrame

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("ZCMPSEL002", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("classcd", SqlDbType.NVarChar, "0056"))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'ドロップダウンリスト設定
                Me.ddlSetCls.Items.Clear()
                Me.ddlSetCls.DataSource = dstOrders.Tables(0)
                Me.ddlSetCls.DataTextField = "M29_NAME"
                Me.ddlSetCls.DataValueField = "M29_CODE"
                Me.ddlSetCls.DataBind()
                Me.ddlSetCls.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加

            Catch ex As Exception
                'データ取得処理エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "設置区分")
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
    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定(システム) 'CNSUPDP004-003
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlSystem()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        Dim clsDataConnect As New ClsCMDataConnect
        Dim clsMst As New ClsMSTCommon

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZMSTSEL004", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                objDs.Tables(0).DefaultView.Sort = "ＴＢＯＸシステムコード"

                'ドロップダウンリスト設定(TBOX機種マスタ)(削除済みデータも取得)
                Me.ddlSystem.ppDropDownList.Items.Clear()
                Me.ddlSystem.ppDropDownList.DataSource = objDs.Tables(0)
                Me.ddlSystem.ppDropDownList.DataTextField = "ＴＢＯＸリスト"
                Me.ddlSystem.ppDropDownList.DataValueField = "ＴＢＯＸシステムコード"
                Me.ddlSystem.ppDropDownList.DataBind()
                Me.ddlSystem.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ機種マスタ一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
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
        Dim strOldDispId As String = Nothing        '遷移元画面情報

        '遷移元画面情報設定
        strOldDispId = ViewState(P_SESSION_OLDDISP).ToString

        Select Case ipshtMode
            Case ClsComVer.E_遷移条件.参照
                '---------------------------
                '2014/06/23 武 ここから
                '---------------------------
                'pnlSPC.Enabled = False
                Me.ddlSystem.ppDropDownList.Enabled = False
                'CNSUPDP004-005
                Me.ddlCreateUser.ppDropDownList.Enabled = False
                'Me.txtCharge.ppEnabled = False
                'CNSUPDP004-005 END
                Me.txtCnstNo.ppEnabled = False
                Me.dttConstD.ppEnabled = False
                Me.txtCnstH.ppEnabled = False
                Me.txtCnstM.ppEnabled = False
                'CNSUPDP004-005
                Me.ddlFSCharge.ppEnabled = False
                'Me.txtFSCharge.ppEnabled = False
                'CNSUPDP004-005
                Me.txtHallCharge.ppEnabled = False
                Me.txtAgcyCharge.ppEnabled = False
                'CNSUPDP004-005
                Me.ddlCnfrm.ppEnabled = False
                'Me.tbpCnfrm.ppEnabled = False
                'Me.tbpComm.ppEnabled = False
                'CNSUPDP004-005 END
                Me.dtbDt.ppEnabled = False
                'CNSUPDP004-005
                Me.ddlChargeDtl.ppEnabled = False
                'Me.txtChargeDtl.ppEnabled = False
                'CNSUPDP004-005 END
                Me.txtContent.ppEnabled = False
                Me.btnAddition.Enabled = False
                Me.btnUpdateDtl.Enabled = False
                Me.btnDelete.Enabled = False
                Me.pnlNGC.Enabled = False
                'Me.btnUpdate.Enabled = False
                Me.btnUpdateNGC.Enabled = False
                Me.Master.ppRigthButton3.Enabled = False
                Me.btnClear.Enabled = False
                Me.btnDelete2.Visible = False
                '---------------------------
                '2014/06/23 武 ここまで
                '---------------------------
                Master.ppRigthButton1.Visible = False
                Me.Master.ppRigthButton2.Enabled = False
            Case ClsComVer.E_遷移条件.更新
                If strAdd = P_ADD_SPC Then
                    Me.txtCnstNo.ppEnabled = False
                    pnlNGC.Enabled = False

                    '更新時は明細登録を許可
                    SetEnable(ClsComVer.E_遷移条件.更新, True)

                    Me.ddlSystem.ppDropDownList.Focus()
                Else
                    '---------------------------
                    '2014/06/23 武 ここから
                    '---------------------------
                    'pnlSPC.Enabled = False
                    Me.ddlSystem.ppDropDownList.Enabled = False
                    'CNSUPDP004-005
                    Me.ddlCreateUser.ppDropDownList.Enabled = False
                    'Me.txtCharge.ppEnabled = False
                    'CNSUPDP004-005 END
                    Me.txtCnstNo.ppEnabled = False
                    Me.dttConstD.ppEnabled = False
                    Me.txtCnstH.ppEnabled = False
                    Me.txtCnstM.ppEnabled = False
                    'CNSUPDP004-005
                    Me.ddlFSCharge.ppEnabled = False
                    'Me.txtFSCharge.ppEnabled = False
                    'CNSUPDP004-005 END
                    Me.txtHallCharge.ppEnabled = False
                    Me.txtAgcyCharge.ppEnabled = False
                    'CNSUPDP004-005
                    Me.ddlCnfrm.ppEnabled = False
                    'Me.tbpCnfrm.ppEnabled = False
                    'Me.tbpComm.ppEnabled = False
                    'CNSUPDP004-005 END
                    Me.dtbDt.ppEnabled = False
                    'CNSUPDP004-005
                    Me.ddlChargeDtl.ppEnabled = False
                    'Me.txtChargeDtl.ppEnabled = False
                    'CNSUPDP004-005 END
                    Me.txtContent.ppEnabled = False
                    Me.btnAddition.Enabled = False
                    Me.btnUpdateDtl.Enabled = False
                    Me.btnDelete.Enabled = False
                    'Me.pnlNGC.Enabled = False
                    'Me.btnUpdate.Enabled = False
                    'Me.btnUpdateNGC.Enabled = False
                    Me.btnClear.Enabled = False
                    '---------------------------
                    '2014/06/23 武 ここまで
                    '---------------------------
                    Me.dttAnswerD.ppDateBox.Focus()
                End If
                '確認処理付与
                Me.btnAddition.OnClientClick =
                    pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事連絡票明細")
                Me.btnUpdateDtl.OnClientClick =
                    pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事連絡票明細")
                Me.btnDelete.OnClientClick =
                    pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事連絡票明細")
                'Me.btnUpdate.OnClientClick =
                '    pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事連絡票")
                Me.btnUpdateNGC.OnClientClick =
                    pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事連絡票")
                Me.btnDelete2.OnClientClick =
                    pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事連絡票")
                Master.ppRigthButton1.OnClientClick =
                    pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事連絡票")
                Me.Master.ppRigthButton2.OnClientClick =
                    pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事連絡票")
                Me.Master.ppRigthButton3.OnClientClick =
                pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事連絡票")
            Case ClsComVer.E_遷移条件.登録
                pnlNGC.Enabled = False

                '登録時は明細登録をおこなわせない
                SetEnable(ClsComVer.E_遷移条件.登録, False)

                If strOldDispId = "CNSLSTP005" Then
                    txtCnstNo.ppEnabled = True
                    Me.txtCnstNo.ppTextBox.Focus()
                Else
                    Me.ddlSystem.ppDropDownList.Focus()
                End If
                'Me.btnUpdate.Text = P_BTN_NM_ADD
                Me.Master.ppRigthButton2.Text = P_BTN_NM_ADD
                '確認処理付与
                Me.btnAddition.OnClientClick =
                    pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事連絡票明細")
                Me.btnUpdateDtl.OnClientClick =
                    pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事連絡票明細")
                Me.btnDelete.OnClientClick =
                    pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事連絡票明細")
                'Me.btnUpdate.OnClientClick =
                '    pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事連絡票")
                Me.btnDelete2.Visible = False
                Me.Master.ppRigthButton2.OnClientClick =
                pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "工事連絡票")
                Master.ppRigthButton1.Visible = False
        End Select
    End Sub

    ''' <summary>
    ''' ENABLEモードセット
    ''' </summary>
    ''' <param name="blVal"></param>
    ''' <remarks></remarks>
    Private Sub SetEnable(ByVal mode As ClsComVer.E_遷移条件, ByVal blVal As Boolean)

        If mode = ClsComVer.E_遷移条件.更新 Then
            dtbDt.ppEnabled = blVal
            'CNSUPDP004-005
            ddlChargeDtl.ppEnabled = blVal
            'txtChargeDtl.ppEnabled = blVal
            'CNSUPDP004-005 END
            txtContent.ppEnabled = blVal
            btnClear.Enabled = blVal
            btnAddition.Enabled = blVal
            btnUpdateDtl.Enabled = False
            btnDelete.Enabled = False
            grvList.Enabled = blVal
        Else
            dtbDt.ppEnabled = blVal
            'CNSUPDP004-005
            ddlChargeDtl.ppEnabled = blVal
            'txtChargeDtl.ppEnabled = blVal
            'CNSUPDP004-005 END
            txtContent.ppEnabled = blVal
            btnClear.Enabled = blVal
            btnAddition.Enabled = blVal
            btnUpdateDtl.Enabled = blVal
            btnDelete.Enabled = blVal
            grvList.Enabled = blVal
        End If
    End Sub

    ''' <summary>
    ''' 工事依頼番号情報取得処理
    ''' </summary>
    ''' <param name="ipstrCnstNo">工事依頼番号</param>
    ''' <param name="opdstData">工事依頼番号項目</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_CnsNoInfo(ByVal ipstrCnstNo As String, ByRef opdstData As DataSet) As Boolean
        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGet_CnsNoInfo = False

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_S4", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrCnstNo))       '工事依頼番号
                    .Add(pfSet_Param("set_cls", SqlDbType.NVarChar, ddlSetCls.SelectedValue))       '設置区分
                End With

                'データ取得
                opdstData = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ有無確認
                If opdstData.Tables(0).Rows.Count = 0 Then
                    'msClear_Screen()
                    Me.txtCnstNo.psSet_ErrorNo("2023", "工事受付したデータ")
                    Me.txtCnstNo.ppTextBox.Focus()
                    Exit Function
                End If

                mfGet_CnsNoInfo = True

            Catch ex As Exception
                'データ取得処理エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事連絡票情報取得")
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
    ''' 工事依頼番号情報の設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_CnsNoInfo()
        Dim dtsCnsNoData As DataSet = Nothing
        Dim dtRow As DataRow = Nothing              'DataRow

        '工事依頼番号情報取得
        If mfGet_CnsNoInfo(Me.txtCnstNo.ppText, dtsCnsNoData) Then

            dtRow = dtsCnsNoData.Tables(0).Rows(0)

            '工事依頼番号情報設定
            'CNSUPDP004-003
            'Dim index As Integer
            'If dtRow("システム").ToString.Contains("-") Then
            '    index = dtRow("システム").ToString.IndexOf("-")
            '    Me.txtSystem.ppText = dtRow("システム").ToString.Substring(0, index)
            'Else
            '    Me.txtSystem.ppText = dtRow("システム").ToString
            'End If
            Me.ddlSystem.ppDropDownList.SelectedValue = dtRow("システム").ToString
            'CNSUPDP004-003 END
            Me.lblTboxId2.Text = dtRow("ＴＢＯＸＩＤ").ToString
            Me.lblHallNm2.Text = dtRow("ホール名").ToString
            Me.dttConstD.ppText = dtRow("作業実施日").ToString
            If dtRow("作業実施時間").ToString.Trim <> String.Empty Then
                Me.txtCnstH.ppText = dtRow("作業実施時間").ToString.Split(":")(0).Trim
                Me.txtCnstM.ppText = dtRow("作業実施時間").ToString.Split(":")(1).Trim
            Else
                Me.txtCnstH.ppText = String.Empty
                Me.txtCnstM.ppText = String.Empty
            End If
            Me.lblNew2.Text = dtRow("新規").ToString
            Me.lblAdd2.Text = dtRow("増設").ToString
            Me.lblPrtRemove2.Text = dtRow("一部撤去").ToString
            Me.lblRelocate2.Text = dtRow("店内移設").ToString
            Me.lblAllRemove2.Text = dtRow("全撤去").ToString
            Me.lblTmpRemove2.Text = dtRow("一時撤去").ToString
            Me.lblReset2.Text = dtRow("再設置").ToString
            Me.lblChngOrgnz2.Text = dtRow("構成変更").ToString
            Me.lblDlvOrgnz2.Text = dtRow("構成配信").ToString
            Me.lblVup2.Text = dtRow("ＶＵＰ").ToString
            Me.lblOth2.Text = dtRow("その他").ToString
            Me.lblOthDtl.Text = dtRow("その他工事内容").ToString
            'CNSUPDP004-005
            If ddlFSCharge.ppDropDownList.Items.FindByText(dtRow("ＦＳ担当者").ToString) Is Nothing Then
                Me.ddlFSCharge.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing))
                ddlFSCharge.ppDropDownList.SelectedIndex = 1
                ddlFSCharge.ppDropDownList.Items.Item(1).Value = "00"
                ddlFSCharge.ppDropDownList.Items.Item(1).Text = dtRow("ＦＳ担当者").ToString
            Else
                Me.ddlFSCharge.ppDropDownList.SelectedValue = ddlFSCharge.ppDropDownList.Items.FindByText(dtRow("ＦＳ担当者").ToString).Value
            End If
            'Me.txtFSCharge.ppText = dtRow("ＦＳ担当者").ToString
            'If ddlCreateUser.ppDropDownList.Items.FindByText(dtRow("登録者").ToString) Is Nothing Then
            '    Me.ddlCreateUser.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing))
            '    ddlCreateUser.ppDropDownList.SelectedIndex = 1
            '    ddlCreateUser.ppDropDownList.Items.Item(1).Value = "00"
            '    ddlCreateUser.ppDropDownList.Items.Item(1).Text = dtRow("登録者").ToString
            'Else
            '    Me.ddlCreateUser.ppDropDownList.SelectedValue = ddlCreateUser.ppDropDownList.Items.FindByText(dtRow("登録者").ToString).Value
            'End If
            ddlSetCls.SelectedValue = dtRow("設置区分").ToString
            'Me.ddlSetCls.SelectedIndex = 0
            'CNSUPDP004-005 END
            Me.txtHallCharge.ppText = dtRow("ホール担当者").ToString
            Me.txtAgcyCharge.ppText = dtRow("代理店担当者").ToString


            Me.ddlSystem.ppDropDownList.Focus()

        Else
            'msClear_Screen()
            Me.txtCnstNo.ppTextBox.Focus()

        End If
    End Sub

    ''' <summary>
    ''' 工事依頼番号検索
    ''' </summary>
    ''' <param name="ipstrCnstNo">工事依頼番号</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGet_CnstNo(ByVal ipstrCnstNo As String) As Integer
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

        mfGet_CnstNo = 1

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_S5", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    '工事依頼番号
                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrCnstNo))
                    .Add(pfSet_Param("set_cls", SqlDbType.NVarChar, ddlSetCls.SelectedValue))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ有無確認
                If dstOrders.Tables(0).Rows.Count = 0 Then
                    mfGet_CnstNo = 0
                    Exit Function
                End If

                Me.txtCnstNo.psSet_ErrorNo("2006", "入力した工事依頼番号、設置区分")
                Me.txtCnstNo.ppTextBox.Focus()

            Catch ex As Exception
                'データ取得処理エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事依頼番号")
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

    'CNSUPDP004-003
    ' ''' <summary>
    ' ''' ＴＢＯＸ機種名、整合性チェック
    ' ''' </summary>
    ' ''' <param name="ipstrSystem">システム</param>
    ' ''' <param name="opdstTboxcls">ＴＢＯＸ機種コード</param>
    ' ''' <returns>整合性OK：True, NG:False</returns>
    ' ''' <remarks></remarks>
    'Private Function mfGet_Tboxcls(ByVal ipstrSystem As String, ByRef opdstTboxcls As DataSet) As Boolean
    '    Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
    '    Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
    '    '--------------------------------
    '    '2014/04/14 星野　ここから
    '    '--------------------------------
    '    objStack = New StackFrame
    '    '--------------------------------
    '    '2014/04/14 星野　ここまで
    '    '--------------------------------

    '    mfGet_Tboxcls = False

    '    'DB接続
    '    If clsDataConnect.pfOpen_Database(conDB) Then
    '        Try
    '            cmdDB = New SqlCommand(M_MY_DISP_ID + "_S6", conDB)
    '            'パラメータ設定
    '            With cmdDB.Parameters
    '                'システム
    '                .Add(pfSet_Param("system", SqlDbType.NVarChar, ipstrSystem))
    '            End With

    '            'データ取得
    '            opdstTboxcls = clsDataConnect.pfGet_DataSet(cmdDB)

    '            'データ有無確認
    '            If opdstTboxcls.Tables(0).Rows.Count = 0 Then
    '                Me.txtSystem.ppText = String.Empty
    '                Me.txtSystem.psSet_ErrorNo("2002", "入力したＴＢＯＸ機種名")
    '                Exit Function
    '            End If

    '            mfGet_Tboxcls = True

    '        Catch ex As Exception
    '            'データ取得処理エラー
    '            psMesBox(Me, "00004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "ＴＢＯＸ機種コード")
    '            '--------------------------------
    '            '2014/04/14 星野　ここから
    '            '--------------------------------
    '            'ログ出力
    '            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
    '                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
    '            '--------------------------------
    '            '2014/04/14 星野　ここまで
    '            '--------------------------------
    '        Finally
    '            'DB切断
    '            If Not clsDataConnect.pfClose_Database(conDB) Then
    '                psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
    '            End If
    '        End Try
    '    Else
    '        psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
    '    End If
    'End Function
    'CNSUPDP004-003 END

    ''' <summary>
    ''' 入力項目のエラーチェックを行う。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheck_Error(ByVal strMode As String)

        Select Case strMode
            Case "SPC"
                'CNSUPDP004-003
                ''--------------------------------
                ''2014/04/14 小守　ここから
                ''--------------------------------
                'Dim dtsTboxcls As DataSet = Nothing

                ''ＴＢＯＸ種別コード取得
                'If Not mfGet_Tboxcls(Me.txtSystem.ppText, dtsTboxcls) Then
                '    Me.ddlSystem.ppDropDownList.Focus()
                'End If
                ''--------------------------------
                ''2014/04/14 小守　ここまで
                ''--------------------------------
                'CNSUPDP004-003 END
            Case "NGC"
                '回答内容
                If txtAnswer.ppText.Replace(Environment.NewLine, String.Empty).Length > 150 Then
                    '改行を除く文字列数 > 150
                    txtAnswer.psSet_ErrorNo("3002", txtAnswer.ppName, "150")
                Else
                    If txtAnswer.ppText.Length > 170 Then
                        '改行多すぎ
                        txtAnswer.psSet_ErrorNo("4014", txtAnswer.ppName & "の入力")
                    End If
                End If
        End Select
    End Sub

    ''' <summary>
    ''' 更新処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msUpdateMethod()


        If (Page.IsValid) Then
            Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
            Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
            Dim dstOrders As DataSet = Nothing          'データセット
            Dim intRtn As Integer
            Dim strMes1 As String = Nothing
            Dim strMes2 As String = Nothing
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            objStack = New StackFrame
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------

            Select Case ViewState(P_SESSION_TERMS)
                Case ClsComVer.E_遷移条件.登録
                    strMes1 = "00003"
                    strMes2 = "00008"
                Case ClsComVer.E_遷移条件.更新
                    strMes1 = "00001"
                    strMes2 = "00007"
            End Select

            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 Then
                        cmdDB = New SqlCommand(M_MY_DISP_ID + "_U2", conDB)
                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("comm_no", SqlDbType.NVarChar, Me.lblCommNo2.Text))                        '連絡票管理番号
                            .Add(pfSet_Param("system", SqlDbType.NVarChar, Me.ddlSystem.ppDropDownList.SelectedValue))                 'システム
                            'CNSUPDP004-005
                            .Add(pfSet_Param("charge", SqlDbType.NVarChar, Me.ddlCreateUser.ppDropDownList.SelectedItem.Text))
                            '.Add(pfSet_Param("charge", SqlDbType.NVarChar, Me.txtCharge.ppText))                        '作成者
                            'CNSUPDP004-005 END
                            .Add(pfSet_Param("const_d", SqlDbType.NVarChar, Me.dttConstD.ppText.Replace("/", "")))      '作業実施日
                            .Add(pfSet_Param("cnst_t", SqlDbType.NVarChar, Me.txtCnstH.ppText.Trim & ":" & txtCnstM.ppText.Trim))             '作業実施時間
                            'CNSUPDP004-005
                            .Add(pfSet_Param("stest_charge", SqlDbType.NVarChar, Me.ddlFSCharge.ppSelectedText))                'ＦＳ担当者
                            '.Add(pfSet_Param("stest_charge", SqlDbType.NVarChar, Me.txtFSCharge.ppText))                'ＦＳ担当者
                            'CNSUPDP004-005 END
                            .Add(pfSet_Param("hall_charge", SqlDbType.NVarChar, Me.txtHallCharge.ppText))               'ホール担当者
                            .Add(pfSet_Param("agcy_charge", SqlDbType.NVarChar, Me.txtAgcyCharge.ppText))               '代理店担当者
                            'CNSUPDP004-005
                            '.Add(pfSet_Param("cnfrm", SqlDbType.NVarChar, Me.tbpCnfrm.ppText))                          '確認者
                            .Add(pfSet_Param("cnfrm", SqlDbType.NVarChar, Me.ddlCnfrm.ppSelectedText))              '確認者
                            '.Add(pfSet_Param("comm", SqlDbType.NVarChar, Me.tbpComm.ppText))                            '連絡者
                            .Add(pfSet_Param("comm", SqlDbType.NVarChar, "")) '空文字登録に変更
                            'CNSUPDP004-005 END
                            .Add(pfSet_Param("answer_d", SqlDbType.NVarChar, mfGetDBNull(Me.dttAnswerD.ppText)))        '回答日
                            .Add(pfSet_Param("answer_charge", SqlDbType.NVarChar, Me.txtAnswerCharge.ppText))           '回答者
                            .Add(pfSet_Param("ngc_status_cd", SqlDbType.NVarChar, Me.ddlNgcStatus.SelectedValue))       '進捗状況
                            .Add(pfSet_Param("answer", SqlDbType.NVarChar, Me.txtAnswer.ppText))                        '回答
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                         'ユーザーＩＤ
                            .Add(pfSet_Param("stradd", SqlDbType.NVarChar, strAdd))                                     'ユーザー拠点
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                           '戻り値
                        End With
                    Else
                        cmdDB = New SqlCommand(M_MY_DISP_ID + "_I2", conDB)
                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("system", SqlDbType.NVarChar, Me.ddlSystem.ppDropDownList.SelectedValue))                'システム
                            'CNSUPDP004-005
                            .Add(pfSet_Param("charge", SqlDbType.NVarChar, Me.ddlCreateUser.ppDropDownList.SelectedItem.Text))
                            '.Add(pfSet_Param("charge", SqlDbType.NVarChar, Me.txtCharge.ppText))                        '作成者
                            'CNSUPDP004-005 END
                            .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.lblTboxId2.Text))                         'ＴＢＯＸＩＤ
                            .Add(pfSet_Param("hall_nm", SqlDbType.NVarChar, Me.lblHallNm2.Text))                        'ホール名
                            .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, Me.txtCnstNo.ppText))                       '工事依頼番号
                            .Add(pfSet_Param("const_d", SqlDbType.NVarChar, Me.dttConstD.ppText.Replace("/", "")))      '作業実施日
                            .Add(pfSet_Param("cnst_t", SqlDbType.NVarChar, Me.txtCnstH.ppText.Trim & ":" & txtCnstM.ppText.Trim))             '作業実施時間
                            .Add(pfSet_Param("h_new", SqlDbType.NVarChar, Me.lblNew2.Text))                             '工事種別（新規）
                            .Add(pfSet_Param("h_add", SqlDbType.NVarChar, Me.lblAdd2.Text))                             '工事種別（増設）
                            .Add(pfSet_Param("h_prt_remove", SqlDbType.NVarChar, Me.lblPrtRemove2.Text))                '工事種別（一部撤去）
                            .Add(pfSet_Param("h_relocate", SqlDbType.NVarChar, Me.lblRelocate2.Text))                   '工事種別（店内移設）
                            .Add(pfSet_Param("h_all_remove", SqlDbType.NVarChar, Me.lblAllRemove2.Text))                '工事種別（全撤去）
                            .Add(pfSet_Param("h_tmp_remove", SqlDbType.NVarChar, Me.lblTmpRemove2.Text))                '工事種別（一時撤去）
                            .Add(pfSet_Param("h_reset", SqlDbType.NVarChar, Me.lblReset2.Text))                         '工事種別（再設置）
                            .Add(pfSet_Param("h_chng_orgnz", SqlDbType.NVarChar, Me.lblChngOrgnz2.Text))                '工事種別（構成変更）
                            .Add(pfSet_Param("h_dlv_orgnz", SqlDbType.NVarChar, Me.lblDlvOrgnz2.Text))                  '工事種別（構成配信）
                            .Add(pfSet_Param("h_oth", SqlDbType.NVarChar, Me.lblOth2.Text))                             '工事種別（その他）
                            .Add(pfSet_Param("oth_dtl", SqlDbType.NVarChar, Me.lblOthDtl.Text))                         'その他工事内容
                            'CNSUPDP004-005
                            .Add(pfSet_Param("stest_charge", SqlDbType.NVarChar, Me.ddlFSCharge.ppSelectedText))                'ＦＳ担当者
                            '.Add(pfSet_Param("stest_charge", SqlDbType.NVarChar, Me.txtFSCharge.ppText))                'ＦＳ担当者
                            'CNSUPDP004-005 END
                            .Add(pfSet_Param("hall_charge", SqlDbType.NVarChar, Me.txtHallCharge.ppText))               'ホール担当者
                            .Add(pfSet_Param("agcy_charge", SqlDbType.NVarChar, Me.txtAgcyCharge.ppText))               '代理店担当者
                            'CNSUPDP004-005
                            '.Add(pfSet_Param("cnfrm", SqlDbType.NVarChar, Me.tbpCnfrm.ppText))                          '確認者
                            .Add(pfSet_Param("cnfrm", SqlDbType.NVarChar, Me.ddlCnfrm.ppSelectedText))              '確認者
                            '.Add(pfSet_Param("comm", SqlDbType.NVarChar, Me.tbpComm.ppText))                            '連絡者
                            .Add(pfSet_Param("comm", SqlDbType.NVarChar, "")) '空文字登録に変更
                            'CNSUPDP004-005 END
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                         'ユーザーＩＤ
                            .Add(pfSet_Param("set_cls", SqlDbType.NVarChar, Me.ddlSetCls.SelectedValue))                '設置区分
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                           '戻り値
                        End With
                    End If

                    'データ登録／更新
                    Using conTrn = conDB.BeginTransaction
                        cmdDB.Transaction = conTrn

                        Select Case ViewState(P_SESSION_TERMS)
                            Case ClsComVer.E_遷移条件.登録
                                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                            Case ClsComVer.E_遷移条件.更新
                                'コマンドタイプ設定(ストアド)
                                cmdDB.CommandType = CommandType.StoredProcedure
                                cmdDB.ExecuteNonQuery()
                        End Select

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, strMes2, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事連絡票", intRtn.ToString)
                            Exit Sub
                        End If

                        'コミット
                        conTrn.Commit()

                    End Using

                    If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録 Then
                        '連絡票管理番号を設定
                        Me.lblCommNo2.Text = dstOrders.Tables(0).Rows(0).Item("連絡票管理番号").ToString

                        '更新画面に変更
                        ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
                        Me.btnUpdateNGC.Text = "更新"
                        'Me.btnUpdate.Text = "更新"
                        Me.btnDelete2.Visible = False
                        Me.Master.ppRigthButton2.Text = "更新"
                        Master.ppRigthButton1.Visible = True
                        '活性／非活性設定
                        msSet_Mode(ViewState(P_SESSION_TERMS))
                        '排他制御
                        msExclusive(Me.lblCommNo2.Text.Trim)
                    End If

                    '完了メッセージ
                    psMesBox(Me, strMes1, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "工事連絡票（" + Me.lblCommNo2.Text + "）")

                Catch ex As Exception
                    'データ登録／更新処理エラー
                    psMesBox(Me, strMes1, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事連絡票")
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

    End Sub

    ''' <summary>
    ''' オブジェクト取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDBNull(ByVal strVal As String) As Object
        If strVal.Trim() = "" Then
            Return DBNull.Value
        End If
        Return strVal

    End Function

    'CNSUPDP004-005
    ''' <summary>
    ''' 作業者等のドロップダウンリスト作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlUser()
        Dim clsMst As New ClsMSTCommon
        Dim dsTmp As New DataSet

        Try
            'データ取得
            dsTmp = mfGet_EmployeeList("0", "0") 'SPCユーザー全員
            'ドロップダウンリスト作成者設定
            Me.ddlCreateUser.ppDropDownList.Items.Clear()
            Me.ddlCreateUser.ppDropDownList.DataSource = dsTmp.Tables(0)
            Me.ddlCreateUser.ppDropDownList.DataTextField = "社員名"
            Me.ddlCreateUser.ppDropDownList.DataValueField = "社員コード"
            Me.ddlCreateUser.ppDropDownList.DataBind()
            Me.ddlCreateUser.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            'ドロップダウンリスト作成者設定
            Me.ddlChargeDtl.ppDropDownList.Items.Clear()
            Me.ddlChargeDtl.ppDropDownList.DataSource = dsTmp.Tables(0)
            Me.ddlChargeDtl.ppDropDownList.DataTextField = "社員名"
            Me.ddlChargeDtl.ppDropDownList.DataValueField = "社員コード"
            Me.ddlChargeDtl.ppDropDownList.DataBind()
            Me.ddlChargeDtl.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            dsTmp = mfGet_EmployeeList("0", "1") 'SPC管理者ユーザー
            'ドロップダウンリスト確認者設定
            Me.ddlCnfrm.ppDropDownList.Items.Clear()
            Me.ddlCnfrm.ppDropDownList.DataSource = dsTmp.Tables(0)
            Me.ddlCnfrm.ppDropDownList.DataTextField = "社員名"
            Me.ddlCnfrm.ppDropDownList.DataValueField = "社員コード"
            Me.ddlCnfrm.ppDropDownList.DataBind()
            Me.ddlCnfrm.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            dsTmp = mfGet_EmployeeList("1", "0") '営業所ユーザー
            'ドロップダウンリストFS担当者設定
            Me.ddlFSCharge.ppDropDownList.Items.Clear()
            Me.ddlFSCharge.ppDropDownList.DataSource = dsTmp.Tables(0)
            Me.ddlFSCharge.ppDropDownList.DataTextField = "社員名"
            Me.ddlFSCharge.ppDropDownList.DataValueField = "社員コード"
            Me.ddlFSCharge.ppDropDownList.DataBind()
            Me.ddlFSCharge.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加


        Catch ex As Exception
            clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "社員情報")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト用社員リスト取得処理
    ''' </summary>
    ''' <param name="ipstrOfficeFlg">営業所フラグ　0:SPC　1:営業所</param>
    ''' <param name="ipstrAuthFlg">管理者フラグ　0:全ユーザ　1:管理者以上　</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGet_EmployeeList(ByVal ipstrOfficeFlg As String, ByVal ipstrAuthFlg As String) As DataSet
        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim dsTmp As New DataSet
        Dim strOKNG As String
        objStack = New StackFrame

        '初期化
        conDB = Nothing
        strOKNG = String.Empty
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("CNSUPDP004_S7", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("office_flg", SqlDbType.NVarChar, ipstrOfficeFlg))
                    .Add(pfSet_Param("auth_flg", SqlDbType.NVarChar, ipstrAuthFlg))
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                End With

                'データ取得
                dsTmp = clsDataConnect.pfGet_DataSet(cmdDB)

                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "1"
                        '整合性OK
                        mfGet_EmployeeList = dsTmp
                    Case Else
                        '整合性エラー
                        mfGet_EmployeeList = Nothing
                End Select

            Catch ex As Exception
                'ログ出力
                psLogWrite("", USER_NAME, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                mfGet_EmployeeList = Nothing
            Finally
                'DB切断
                clsDataConnect.pfOpen_Database(conDB)
            End Try
        Else
            mfGet_EmployeeList = Nothing
        End If

    End Function

    ''' <summary>
    ''' NGCの返答有無チェック
    ''' </summary>
    ''' <returns>True:返答有り False:返答無し</returns>
    ''' <remarks></remarks>
    Private Function mfExistNgcAnswer() As Boolean
        If dttAnswerD.ppText.Trim = String.Empty AndAlso _
           txtAnswerCharge.ppText.Trim = String.Empty AndAlso _
           ddlNgcStatus.Text.Trim = String.Empty AndAlso _
           txtAnswer.ppText.Trim = String.Empty Then
            Return False
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' 工事依頼書のデータを丸ごと返します。
    ''' </summary>
    ''' <param name="ipstrCnstNo"></param>
    ''' <returns></returns>
    ''' <remarks>同様のプロシージャが複数ありますが、エラーチェック用として使います。</remarks>
    Private Function mfGet_CnsData(ByVal ipstrCnstNo As String) As DataSet
        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dsRtn As DataSet
        objStack = New StackFrame

        mfGet_CnsData = Nothing

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_S4", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrCnstNo))       '工事依頼番号
                    .Add(pfSet_Param("set_cls", SqlDbType.NVarChar, ddlSetCls.SelectedValue))       '設置区分
                End With

                'データ取得
                dsRtn = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ有無確認
                If dsRtn.Tables(0).Rows.Count = 0 Then
                    mfGet_CnsData = Nothing
                End If

                mfGet_CnsData = dsRtn

            Catch ex As Exception
                'データ取得処理エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事連絡票情報取得")
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
    ''' 入力項目のエラーチェックを行う。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msErrCheck()
        Dim drCns As DataRow = mfGet_CnsData(Me.txtCnstNo.ppText.Trim).Tables(0).Rows(0)

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
                'NGC返答有なら確認者必須
                If mfExistNgcAnswer() = True Then
                    If ddlCnfrm.ppDropDownList.SelectedValue = String.Empty Then
                        Me.ddlCnfrm.psSet_ErrorNo("5003", ddlCnfrm.ppName)
                    End If
                End If
        End Select

        '依頼書にシステムコードが2種類あるので、どっちか合えばOK
        If drCns.Item("システム").ToString.Trim <> ddlSystem.ppDropDownList.SelectedValue.ToString.Trim AndAlso drCns.Item("システム予備").ToString.Trim <> ddlSystem.ppDropDownList.SelectedValue.ToString.Trim Then
            Me.ddlSystem.psSet_ErrorNo("2012", "工事依頼書と" & ddlSystem.ppName)
        End If

        '作業実施日時の未入力チェック
        If dttConstD.ppText.Trim = String.Empty OrElse txtCnstH.ppText.Trim = String.Empty OrElse txtCnstM.ppText.Trim = String.Empty Then
            Me.dttConstD.psSet_ErrorNo("5006", dttConstD.ppName)
        End If

        'FS稼働有(設置区分未選択以外)時は整合性チェックも実施
        If ddlSetCls.SelectedValue <> String.Empty Then
            If drCns.Item("実施日確認").ToString.Trim <> dttConstD.ppText.Trim & edit_num(txtCnstH.ppText.Trim.ToString()) & edit_num(txtCnstM.ppText.Trim.ToString) Then
                Dim strErrms As String
                If ddlSetCls.SelectedValue = "1" Then
                    strErrms = "仮設置日時"      '仮設置なので仮設置日を入力して下さいメッセージ
                Else
                    strErrms = "総合テスト日時"  '本設置なので総合テスト日を入力して下さいメッセージ
                End If
                Me.dttConstD.psSet_ErrorNo("2011", dttConstD.ppName, strErrms)
            End If
        End If

    End Sub

    Private Sub mstxtCnsNoVal(sender As Object, e As EventArgs)
        If txtCnstNo.ppText.Trim = String.Empty Then
            Me.txtCnstNo.psSet_ErrorNo("5006", txtCnstNo.ppName)
            Me.txtCnstNo.ppTextBox.Focus()
            Exit Sub
        End If
        If Regex.IsMatch(txtCnstNo.ppText.Trim, "N00[1,9]0-\d{8}") = False Then
            Me.txtCnstNo.psSet_ErrorNo("4014", txtCnstNo.ppName)
            Me.txtCnstNo.ppTextBox.Focus()
            Exit Sub
        End If

    End Sub
    Private Sub msdttConstDVal(sender As Object, e As EventArgs)
        If dttConstD.ppText.Trim = String.Empty Then
            Me.dttConstD.psSet_ErrorNo("5006", dttConstD.ppName)
            Me.dttConstD.ppDateBox.Focus()
            Exit Sub
        End If
        If Regex.IsMatch(dttConstD.ppText.Trim, "\d{1,4}/\d{2}/\d{2}") = False Then
            Me.dttConstD.psSet_ErrorNo("4014", dttConstD.ppName)
            Me.dttConstD.ppDateBox.Focus()
            Exit Sub
        End If
    End Sub
    Private Sub msTimeVal(sender As Object, e As EventArgs)
        If sender.Text.Trim = String.Empty Then
            Me.txtCnstH.psSet_ErrorNo("5006", txtCnstH.ppName)
            Me.txtCnstH.ppTextBox.Focus()
            Exit Sub
        End If
        If Regex.IsMatch(sender.Text.Trim, "\d{1,2}") = False Then
            Me.txtCnstH.psSet_ErrorNo("4001", txtCnstH.ppName, "正しい形式")
            Me.txtCnstH.ppTextBox.Focus()
            Exit Sub
        End If
        sender.Text = edit_num(sender.Text)
    End Sub

    ''' <summary>
    ''' 2桁に0埋め
    ''' </summary>
    ''' <param name="num"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function edit_num(ByVal num) As String
        If num.ToString.Length = 1 Then
            num = "0" & num
        End If
        Return num
    End Function
    Private Sub msExclusive(ByVal strComNo As String)
        Dim strExclusiveDate As String = String.Empty
        Dim arTable_Name As New ArrayList
        Dim arKey As New ArrayList
        'ロック対象テーブル名の登録.
        arTable_Name.Insert(0, "D25_CNST_COMM")

        'ロックテーブルキー項目の登録.
        arKey.Insert(0, strComNo)

        '排他情報確認処理(更新処理の実行).
        If clsExc.pfSel_Exclusive(strExclusiveDate _
                         , Me _
                         , Session(P_SESSION_IP) _
                         , Session(P_SESSION_PLACE) _
                         , Session(P_SESSION_USERID) _
                         , Session(P_SESSION_SESSTION_ID) _
                         , ViewState(P_SESSION_GROUP_NUM) _
                         , "CNSUPDP004" _
                         , arTable_Name _
                         , arKey) = 0 Then

            '登録年月日時刻.
            Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate
        End If

        '排他情報のグループ番号をセッション変数に設定.
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)
    End Sub
    'CNSUPDP004-005 END

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
