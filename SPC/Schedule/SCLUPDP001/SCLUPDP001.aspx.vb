'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　スケジュール明細
'*　ＰＧＭＩＤ：　SCLLSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2016.11.28　：　加賀
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'SCLLSTP001-001     2016/11/28      加賀     XXXX
'SCLLSTP001-002     2017/01/20      栗原     文言変更[訪問先コード]→[TBOXID]、TBOXIDの表示制御(保守又は工事のみ表示)、作業人数表示の追加、明細削除時のバグ修正
'SCLUPDP001_003     2017/01/23      武       エラーチェック処理追加
'SCLUPDP001_004     2017/01/23      武       親データと子データ同時更新する

Imports System.Data.SqlClient
Imports SPC.ClsCMCommon

Public Class SCLUPDP001

#Region "継承定義"
    Inherits System.Web.UI.Page
#End Region

#Region "定数定義"

    Const MY_DISP_ID As String = "SCLUPDP001"
    Const SCL_COMMON_S01 As String = "SCLCOMMON_S01"
    Const VS_TERMS As String = "VS_TERMS"
    Const VS_PRMS As String = "VS_PRMS"
    Const VS_DETAIL As String = "VS_DETAIL"
    Const FilterDel As String = "削除 = '0'"
    Const EditMes As String = "\nスケジュールを確定して下さい。"
    'SCLUPDP001_004
    Const VS_MAILMST As String = "VS_MAILMST"
    Const VS_MAIN As String = "VS_MAIN"
    'SCLUPDP001_004 END

    ''' <summary>
    ''' 明細編集エリア制御用
    ''' </summary>
    Private Enum EditMode As Short
        未使用 = 0
        登録 = 1
        更新 = 2
    End Enum

    ''' <summary>
    ''' 明細データ区分
    ''' </summary>
    Private Enum DataCls As Short
        既存 = 0
        新規 = 1
        更新 = 2
    End Enum


#End Region

#Region "変数定義"

    Dim objStack As New StackFrame

#End Region


#Region "イベントプロシージャ"

    ''' <summary>
    ''' ページ初期処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, MY_DISP_ID)

    End Sub

    ''' <summary>
    ''' ページロード処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンイベント処理設定
        AddHandler Master.ppLeftButton1.Click, AddressOf btnNew_Click            '新規
        AddHandler Master.ppRigthButton1.Click, AddressOf btnDelete_Click        '削除
        AddHandler Master.ppRigthButton2.Click, AddressOf btnUpdate_Click        '確定

        AddHandler ddlVstCls.ppDropDownList.SelectedIndexChanged, AddressOf ddlVstCls_SelectedIndexChanged
        'AddHandler lblVstCd.TextBox.TextChanged, AddressOf txtVstCd_TextChanged
        AddHandler txtWkmCd.ppTextBox.TextChanged, AddressOf txtWkmCd_TextChanged

        Try

            If Not IsPostBack Then  '初回表示のみ

                Dim clsCMDBC As New ClsCMDBCom  '画面名取得用

                'セッション情報→Viewstate格納
                ViewState(VS_TERMS) = Session(P_SESSION_TERMS)
                ViewState(VS_PRMS) = Session(P_KEY)

                'プログラムID、画面名設定
                Master.ppProgramID = MY_DISP_ID
                Master.ppTitle = clsCMDBC.pfGet_DispNm(MY_DISP_ID)

                'パンくずリスト設定
                Master.ppBcList_Text = ClsCMCommon.pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

                'SCLUPDP001_004
                'メール送信設定マスタ取得
                ViewState(VS_MAILMST) = psGetMailMaster()
                'SCLUPDP001_004 END

                '登録
                btnInsert.OnClientClick = pfGet_OCClickMes("00008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "スケジュール")

                '新規
                Master.ppLeftButton1.Visible = True
                Master.ppLeftButton1.Text = "新規"
                Master.ppLeftButton1.CausesValidation = False
                Master.ppLeftButton1.OnClientClick = pfGet_OCClickMes("00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "画面を初期化して新規登録モードに変更します。\nよろしいですか？")

                '削除
                Master.ppRigthButton1.Visible = True
                Master.ppRigthButton1.Text = "削除"
                Master.ppRigthButton1.CausesValidation = False
                Master.ppRigthButton1.BackColor = Drawing.Color.FromArgb(255, 102, 102)
                Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "スケジュール")

                '確定
                Master.ppRigthButton2.Visible = True
                Master.ppRigthButton2.Text = "確定"
                Master.ppRigthButton2.CausesValidation = False
                Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "スケジュールの確定")   '{0}を行います。\nよろしいですか？

                '明細削除
                btnDeleteDtl.BackColor = Drawing.Color.FromArgb(255, 102, 102)

                'AutoPostBack!
                ddlVstCls.ppDropDownList.AutoPostBack = True
                'lblVstCd.TextBox.AutoPostBack = True
                txtWkmCd.ppTextBox.AutoPostBack = True

                'ﾄﾞﾛｯﾌﾟﾀﾞｳﾝ設定
                setDropDownList()

                'データ取得
                Select Case ViewState(VS_TERMS)
                    Case ClsComVer.E_遷移条件.参照

                        'データ取得
                        getData(ViewState(VS_PRMS))

                        '活性制御
                        setEnable(ClsComVer.E_遷移条件.参照)

                    Case ClsComVer.E_遷移条件.登録

                        '活性制御
                        setEnable(ClsComVer.E_遷移条件.登録)

                        '一覧表示
                        grvList.DataSource = New DataTable
                        grvList.DataBind()

                        'フォーカス
                        ddlVstCls.ppDropDownList.Focus()

                    Case ClsComVer.E_遷移条件.更新

                        'データ取得
                        getData(ViewState(VS_PRMS))

                        '活性制御
                        setEnable(ClsComVer.E_遷移条件.更新, EditMode.登録)

                        '排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
                        If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then

                            Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)

                        End If

                        'フォーカス
                        txtWkmCd.ppTextBox.Focus()

                End Select


            End If



        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "画面の初期化")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

    End Sub

    'SCLLSTP001-002 
    Private Sub Page_Prerender() Handles Me.PreRender
        msDispWorkerCount()
    End Sub
    'SCLLSTP001-002 END

    ''' <summary>
    ''' 訪問種別　変更時処理
    ''' </summary>
    Protected Sub ddlVstCls_SelectedIndexChanged(sender As Object, e As EventArgs)

        Dim strVstCls As String

        '訪問種別取得
        strVstCls = ddlVstCls.ppSelectedValue

        '活性制御
        setEnable(ViewState(VS_TERMS), EditMode.登録)

        If strVstCls = "01" OrElse strVstCls = "02" Then

            'txtVstNm.ppEnabled = False

            ''フォーカス
            'SetFocus(txtCtrlNo.ppTextBox)

        Else

            'txtVstCd.ppEnabled = False
            'txtCtrlNo.ppEnabled = False

            'フォーカス
            SetFocus(txtWrkDt.ppDateBox)

        End If

    End Sub

    ' ''' <summary>
    ' ''' 訪問先コード　変更時処理
    ' ''' </summary>
    'Protected Sub txtVstCd_TextChanged(sender As Object, e As EventArgs)

    '    'フォーカス
    '    SetFocus(lblVstCd.TextBox)

    '    '未入力
    '    If lblVstCd.Text.Trim = String.Empty Then

    '        txtVstNm.ppText = String.Empty

    '        Exit Sub

    '    End If

    '    Dim strDataName As String
    '    Dim strValGrp As String = String.Empty
    '    Dim strSQLPrm(1) As String
    '    Dim strReturn As String = Nothing

    '    Try

    '        'ValidationGroupを退避
    '        strValGrp = txtVstCd.ppValidationGroup

    '        'ValidationGroupを一時的に変更
    '        txtVstCd.ppValidationGroup = strValGrp & "_VstCD"

    '        '入力値検証
    '        Me.Validate(strValGrp & "_VstCD")

    '        If Me.IsValid() = False Then
    '            Exit Sub
    '        End If

    '        'パラメータ設定
    '        strDataName = "ホール"
    '        strSQLPrm(0) = "T01_HALL"
    '        strSQLPrm(1) = lblVstCd.Text.Trim

    '        '取得
    '        If getName(strDataName, strSQLPrm, strReturn) Then

    '            txtVstNm.ppText = strReturn.Trim

    '        End If

    '        '存在確認
    '        If strReturn.Trim = String.Empty Then

    '            'TBOXIDは存在しません
    '            txtVstCd.psSet_ErrorNo("2002", "入力したTBOXID")  '{0} は存在しません。再度確認し、入力してください。

    '        End If

    '    Catch ex As Exception

    '        'エラーメッセージ表示
    '        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "作業者情報の取得")

    '        'ログ出力
    '        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
    '                        objStack.GetMethod.Name, "", ex.ToString, "Catch")

    '    Finally

    '        'ValidationGroupを戻す
    '        txtVstCd.ppValidationGroup = strValGrp

    '    End Try

    'End Sub

    ''' <summary>
    ''' 作業者コード　変更時処理
    ''' </summary>
    Protected Sub txtWkmCd_TextChanged(sender As Object, e As EventArgs)

        'Trimfa
        txtWkmCd.ppText = txtWkmCd.ppText.Trim

        '未入力はチェックしない
        If txtWkmCd.ppText = String.Empty Then

            '作業者情報クリア
            txtWkmCd.ppText = String.Empty
            lblWkmLNm.Text = String.Empty
            lblWkmFNm.Text = String.Empty
            lblMntNm.Text = String.Empty
            hdnMntCd.Value = String.Empty
            lblArea.Text = String.Empty
            hdnAreaCd.Value = String.Empty
            lblAdm1.Text = String.Empty
            hdnAdm1Cd.Value = String.Empty
            hdnAdm1LNm.Value = String.Empty
            hdnAdm1FNm.Value = String.Empty
            lblAdm2.Text = String.Empty
            hdnAdm2Cd.Value = String.Empty
            hdnAdm2LNm.Value = String.Empty
            hdnAdm2FNm.Value = String.Empty

            '出発・到着日時 クリア
            txtDptDt.ppText = String.Empty
            txtDptDt.ppHourText = String.Empty
            txtDptDt.ppMinText = String.Empty
            lblArrDt.Text = String.Empty

            '活性制御
            If hdnSEQ.Value = String.Empty Then
                txtDptDt.ppEnabled = False
                btnInsertDtl.Enabled = False
            Else
                txtDptDt.ppEnabled = False
                btnUpdateDtl.Enabled = False
            End If

            'フォーカス
            SetFocus(txtWkmCd.ppTextBox)

            Exit Sub

        End If

        Dim strPrmAry As String
        Dim strValGrp As String = String.Empty
        Dim dtWrkInf As New DataTable

        Try
            '作業コード
            strPrmAry = txtWkmCd.ppText

            'ValidationGroupを退避
            strValGrp = txtWkmCd.ppValidationGroup

            'ValidationGroupを一時的に変更
            txtWkmCd.ppValidationGroup = strValGrp & "_WrkCD"

            '入力値検証
            Me.Validate(strValGrp & "_WrkCD")

            If Me.IsValid() = False Then

                'フォーカス
                SetFocus(txtWkmCd.ppTextBox)

                Exit Sub

            End If

            '整合性チェック
            'SCLUPDP001_004(パラメータ追加)
            If CheckWkmCd(0, dtWrkInf) = False Then
                'SCLUPDP001_004 END
                'フォーカス
                SetFocus(txtWkmCd.ppTextBox)

            Else

                'フォーカス
                SetFocus(txtDptDt.ppDateBox)

                '活性制御
                If hdnSEQ.Value = String.Empty Then
                    txtDptDt.ppEnabled = True
                    btnInsertDtl.Enabled = True
                Else
                    txtDptDt.ppEnabled = True
                    btnUpdateDtl.Enabled = True
                End If

            End If

            '画面反映
            Select Case dtWrkInf.Rows.Count
                Case 0

                    '該当データ無し 作業者情報クリア
                    lblWkmLNm.Text = String.Empty
                    lblWkmFNm.Text = String.Empty
                    lblMntNm.Text = String.Empty
                    hdnMntCd.Value = String.Empty
                    lblArea.Text = String.Empty
                    hdnAreaCd.Value = String.Empty
                    lblAdm1.Text = String.Empty
                    hdnAdm1Cd.Value = String.Empty
                    hdnAdm1LNm.Value = String.Empty
                    hdnAdm1FNm.Value = String.Empty
                    lblAdm2.Text = String.Empty
                    hdnAdm2Cd.Value = String.Empty
                    hdnAdm2LNm.Value = String.Empty
                    hdnAdm2FNm.Value = String.Empty

                    '出発・到着日時 クリア
                    lblArrDt.Text = String.Empty

                Case 1

                    With dtWrkInf.Rows(0)

                        lblWkmLNm.Text = .Item("作業者姓").ToString
                        lblWkmFNm.Text = .Item("作業者名").ToString
                        hdnMntCd.Value = .Item("営業所コード").ToString
                        lblMntNm.Text = .Item("営業所").ToString
                        hdnAreaCd.Value = .Item("範囲コード").ToString
                        lblArea.Text = .Item("エリア").ToString
                        lblAdm1.Text = .Item("責任者").ToString
                        hdnAdm1Cd.Value = .Item("責任者コード").ToString
                        hdnAdm1LNm.Value = .Item("責任者姓").ToString
                        hdnAdm1FNm.Value = .Item("責任者名").ToString
                        lblAdm2.Text = String.Empty
                        hdnAdm2Cd.Value = String.Empty
                        hdnAdm2LNm.Value = String.Empty
                        hdnAdm2FNm.Value = String.Empty

                        '到着日時
                        lblArrDt.Text = txtWrkDt.ppDate.ToString("yyyy/MM/dd HH:mm")

                    End With

                Case Else

                    With dtWrkInf

                        lblWkmLNm.Text = .Rows(0).Item("作業者姓").ToString
                        lblWkmFNm.Text = .Rows(0).Item("作業者名").ToString
                        hdnMntCd.Value = .Rows(0).Item("営業所コード").ToString
                        lblMntNm.Text = .Rows(0).Item("営業所").ToString
                        lblArea.Text = .Rows(0).Item("エリア").ToString
                        hdnAreaCd.Value = .Rows(0).Item("範囲コード").ToString
                        lblAdm1.Text = .Rows(0).Item("責任者").ToString
                        hdnAdm1Cd.Value = .Rows(0).Item("責任者コード").ToString
                        hdnAdm1LNm.Value = .Rows(0).Item("責任者姓").ToString
                        hdnAdm1FNm.Value = .Rows(0).Item("責任者名").ToString
                        lblAdm2.Text = .Rows(1).Item("責任者").ToString
                        hdnAdm2Cd.Value = .Rows(1).Item("責任者コード").ToString
                        hdnAdm2LNm.Value = .Rows(1).Item("責任者姓").ToString
                        hdnAdm2FNm.Value = .Rows(1).Item("責任者名").ToString

                        '到着日時
                        lblArrDt.Text = txtWrkDt.ppDate.ToString("yyyy/MM/dd HH:mm")

                    End With

            End Select

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "作業者情報の取得")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        Finally

            'ValidationGroupを戻す
            txtWkmCd.ppValidationGroup = strValGrp

            'Dispose
            If dtWrkInf IsNot Nothing Then
                dtWrkInf.Dispose()
            End If

        End Try

    End Sub

    ''' <summary>
    ''' 登録ボタン押下時処理
    ''' </summary>
    Protected Sub btnInsert_Click(sender As Object, e As EventArgs) Handles btnInsert.Click

        '作業日時
        If txtWrkDt.ppDate < DateTime.Now Then

            '過去日登録
            txtWrkDt.psSet_ErrorNo("1006", txtWrkDt.ppName, "現在日時")    '「{0}」は「{1}」以降の日時で入力してください。

        End If

        '入力値検証
        If Me.IsValid() = False Then
            Exit Sub
        End If

        'SCLUPDP001_004
        '整合性チェック
        If CheckWkmCd(4) = False Then
            Exit Sub
        End If
        'SCLUPDP001_004 END

        ''工事・保守管理番号確認
        'If ddlVstCls.ppSelectedValue = "01" OrElse ddlVstCls.ppSelectedValue = "02" Then

        '    If checkCtrlNo(ddlVstCls.ppSelectedValue, txtCtrlNo.ppText) = False Then
        '        Exit Sub
        '    End If

        'End If

        '開始ログ出力
        psLogStart(Me)

        Dim strKey(5) As String
        Dim strPrmAry(6) As String
        Dim arySQLCmd(1) As SqlCommand
        Dim cmdSQL As SqlCommand
        Dim intReturn As Integer

        Try
            '登録内容 格納
            strPrmAry(0) = String.Empty      'txtCtrlNo.ppText
            strPrmAry(1) = ddlVstCls.ppSelectedValue
            strPrmAry(2) = txtWrkDt.ppText.Replace("/", "")
            strPrmAry(3) = txtWrkDt.ppHourText & txtWrkDt.ppMinText & "00"
            strPrmAry(4) = lblVstCd.Text
            strPrmAry(5) = txtVstNm.ppText.Trim
            strPrmAry(6) = Session(P_SESSION_USERID).ToString

            'SQLコマンド設定

            '訪問種別が 01:工事 02:保守以外の場合、訪問先更新
            If strPrmAry(1) <> "01" AndAlso strPrmAry(1) <> "02" Then

                '訪問先更新
                cmdSQL = New SqlCommand(MY_DISP_ID + "_U03")
                cmdSQL.Parameters.AddRange({New SqlParameter("@vst_nm", strPrmAry(5)) _
                                          , New SqlParameter("@usr_id", strPrmAry(6)) _
                                          , New SqlParameter("@vst_cd", SqlDbType.VarChar, 8) _
                                           })

                'OUTPUT
                cmdSQL.Parameters("@vst_cd").Direction = ParameterDirection.Output

                '配列に追加
                arySQLCmd(0) = cmdSQL.Clone

            End If

            'SCLUPDP001_004
            ViewState(VS_MAIN) = strPrmAry
            'SCLUPDP001_004 END

            'SCLUPDP001_004
            ''スケジュール登録
            'cmdSQL = New SqlCommand(MY_DISP_ID + "_I01")
            'cmdSQL.Parameters.AddRange({New SqlParameter("@ctrl_no", strPrmAry(0)) _
            '                          , New SqlParameter("@vst_cls", strPrmAry(1)) _
            '                          , New SqlParameter("@wrk_day", strPrmAry(2)) _
            '                          , New SqlParameter("@wrk_tim", strPrmAry(3)) _
            '                          , New SqlParameter("@vst_cd", strPrmAry(4)) _
            '                          , New SqlParameter("@vst_nm", strPrmAry(5)) _
            '                          , New SqlParameter("@usr_id", strPrmAry(6)) _
            '                          , New SqlParameter("@mail_no", SqlDbType.VarChar, 13) _
            '                           })
            ''OUTPUT
            'cmdSQL.Parameters("@mail_no").Direction = ParameterDirection.Output

            ''配列に追加
            'arySQLCmd(1) = cmdSQL.Clone
            'SCLUPDP001_004 END

            '登録・更新実行
            If ClsCMDataConnect.pfExec_StoredProcedure(Me, arySQLCmd, intReturn) = False Then

                'メッセージ表示
                Select Case intReturn
                    Case -1
                        psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    Case 0
                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【SQLエラー】" + "訪問先更新")       '{0}に失敗しました。
                    Case 1
                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【SQLエラー】" + "スケジュール登録")       '{0}に失敗しました。
                End Select

                Exit Sub

            End If

            '訪問先コード反映
            'SCLUPDP001_004
            'lblVstCd.Text = arySQLCmd(0).Parameters("@vst_cd").Value.ToString
            'SCLUPDP001_004 END

            'キーを変更
            'SCLUPDP001_004
            'Array.Copy(arySQLCmd(1).Parameters("@mail_no").Value.ToString.Split("-"), strKey, 3)
            'SCLUPDP001_004 END
            Array.Copy(strPrmAry, 1, strKey, 3, 3)

            'SCLUPDP001_004
            'メール管理番号を空にする
            strKey(0) = String.Empty
            strKey(1) = String.Empty
            strKey(2) = String.Empty
            'SCLUPDP001_004 END

            'SCLUPDP001_004
            'lblMailNo.Text = arySQLCmd(1).Parameters("@mail_no").Value.ToString
            'SCLUPDP001_004 END

            '制御情報を変更
            ViewState(VS_TERMS) = ClsComVer.E_遷移条件.更新   '遷移条件
            ViewState(VS_PRMS) = strKey                       'キー

            'SCLUPDP001_004
            ''排他情報確認処理(更新処理の実行)
            'Dim clsExc As New ClsCMExclusive
            'Dim strExclusiveDate As String = String.Empty

            'If clsExc.pfSel_Exclusive(strExclusiveDate _
            '                 , Me _
            '                 , Session(P_SESSION_IP) _
            '                 , Session(P_SESSION_PLACE) _
            '                 , Session(P_SESSION_USERID) _
            '                 , Session(P_SESSION_SESSTION_ID) _
            '                 , ViewState(P_SESSION_GROUP_NUM) _
            '                 , Session(P_SESSION_OLDDISP) _
            '                 , New ArrayList({"SCL_TRN_DTIL"}) _
            '                 , New ArrayList(strKey)) = 0 Then

            '    '排他情報のグループ番号をセッション変数に設定
            '    Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

            '    '排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
            '    Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate
            '    Me.Master.ppExclusiveDate = strExclusiveDate

            'End If
            'SCLUPDP001_004 END

            'データ取得
            getData(ViewState(VS_PRMS))

            '活性制御
            setEnable(ViewState(VS_TERMS), EditMode.登録)

            '正常終了メッセージ
            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "スケジュール")

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "スケジュールの登録")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' クリアボタン押下時処理
    ''' </summary>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click

        '開始ログ出力
        psLogStart(Me)

        Try
            'クリア
            ddlVstCls.ppDropDownList.SelectedIndex = -1
            lblCtrlNo.Text = String.Empty
            txtWrkDt.ppText = String.Empty
            txtWrkDt.ppHourText = String.Empty
            txtWrkDt.ppMinText = String.Empty
            lblVstCd.Text = String.Empty
            txtVstNm.ppText = String.Empty

            '活性制御
            setEnable(ClsComVer.E_遷移条件.登録)

            'フォーカス
            ddlVstCls.ppDropDownList.Focus()

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "スケジュール情報のクリア")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

        '終了ログ出力
        psLogEnd(Me)



    End Sub

    ''' <summary>
    ''' 新規ボタン押下時処理
    ''' </summary>
    Protected Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnClear.Click

        '開始ログ出力
        psLogStart(Me)

        Try

            'ヘッダ項目クリア
            ddlVstCls.ppDropDownList.SelectedIndex = -1
            lblMailNo.Text = String.Empty
            lblCtrlNo.Text = String.Empty
            txtWrkDt.ppText = String.Empty
            txtWrkDt.ppHourText = String.Empty
            txtWrkDt.ppMinText = String.Empty
            lblVstCd.Text = String.Empty
            txtVstNm.ppText = String.Empty

            '明細クリア
            ClearDtl()

            '一覧表示
            grvList.DataSource = New DataTable
            grvList.DataBind()

            '登録モードに変更
            ViewState(VS_TERMS) = ClsComVer.E_遷移条件.登録
            ViewState(VS_PRMS) = Nothing
            ViewState(VS_DETAIL) = Nothing

            'DropDownList設定
            setDropDownList()

            '活性制御
            setEnable(ViewState(VS_TERMS))

            'フォーカス
            ddlVstCls.ppDropDownList.Focus()

            '正常終了メッセージ
            'psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "画面を初期化しました。")
            psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "新規スケジュールを登録して下さい。")

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "スケジュール情報のクリア")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

        '終了ログ出力
        psLogEnd(Me)



    End Sub

    ''' <summary>
    ''' 確定ボタン(フッター)　押下時処理
    ''' </summary>
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        'SCLUPDP001_004
        Dim strPrmAry(6) As String
        Dim strKeyAry(6) As String
        'SCLUPDP001_004 END

        Try

            'SCLUPDP001_004
            '登録内容 格納
            strKeyAry = ViewState(VS_MAIN)
            strPrmAry = ViewState(VS_PRMS)
            strPrmAry = strPrmAry.Concat({Session(P_SESSION_USERID).ToString}).ToArray

            Select Case lblMailNo.Text
                Case String.Empty
                    '整合性チェック
                    'If CheckWkmCd(0) = False Then
                    '    Exit Sub
                    'End If

                    Using dtDetail As DataTable = DirectCast(ViewState(VS_DETAIL), DataTable).Copy

                        ''明細が0件の場合、削除
                        'If grvList.Rows.Count = 0 Then

                        '    '削除処理
                        '    DeleteSchedule()

                        '    Exit Sub

                        'End If
                        Dim cmdSQL As SqlCommand
                        Dim arySQLCmd(1) As SqlCommand
                        Dim intReturn As Integer

                        'メール管理番号を発行する
                        cmdSQL = New SqlCommand(MY_DISP_ID + "_S04")
                        cmdSQL.Parameters.AddRange({ _
                                    New SqlParameter("@vst_cls", strKeyAry(1)) _
                                  , New SqlParameter("@mail_no", SqlDbType.VarChar, 13) _
                                   })
                        'OUTPUT
                        cmdSQL.Parameters("@mail_no").Direction = ParameterDirection.Output

                        '配列に追加
                        arySQLCmd(0) = cmdSQL.Clone

                        '登録・更新実行
                        If ClsCMDataConnect.pfExec_StoredProcedure(Me, arySQLCmd, intReturn) = False Then

                            'メッセージ表示
                            Select Case intReturn
                                Case -1
                                    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                                Case 0
                                    psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【SQLエラー】" + "訪問先更新")       '{0}に失敗しました。
                                Case 1
                                    psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【SQLエラー】" + "スケジュール登録")       '{0}に失敗しました。
                            End Select

                            Exit Sub

                        End If

                        Array.Copy(arySQLCmd(0).Parameters("@mail_no").Value.ToString.Split("-"), strPrmAry, 3)

                        ViewState(VS_PRMS) = strPrmAry

                        'スケジュール登録
                        cmdSQL = New SqlCommand(MY_DISP_ID + "_I01")
                        cmdSQL.Parameters.AddRange({New SqlParameter("@ctrl_no", strKeyAry(0)) _
                                                  , New SqlParameter("@vst_cls", strKeyAry(1)) _
                                                  , New SqlParameter("@wrk_day", strKeyAry(2)) _
                                                  , New SqlParameter("@wrk_tim", strKeyAry(3)) _
                                                  , New SqlParameter("@vst_cd", strKeyAry(4)) _
                                                  , New SqlParameter("@vst_nm", strKeyAry(5)) _
                                                  , New SqlParameter("@usr_id", strKeyAry(6)) _
                                                  , New SqlParameter("@mail_no1", strPrmAry(0)) _
                                                  , New SqlParameter("@mail_no2", strPrmAry(1)) _
                                                  , New SqlParameter("@mail_no3", strPrmAry(2)) _
                                                   })

                        '配列に追加
                        arySQLCmd(0) = cmdSQL.Clone

                        '不要項目を削除
                        With dtDetail.Columns
                            .Remove("作業者")
                            .Remove("営業所")
                            .Remove("エリア")
                            .Remove("責任者1")
                            .Remove("責任者2")
                            .Remove("出発日時")
                            .Remove("出発連絡")
                            .Remove("到着日時")
                            .Remove("到着連絡")
                            .Remove("出発送信回数")
                            .Remove("到着送信回数")
                            .Remove("変更不可")
                        End With

                        '変更を反映
                        dtDetail.AcceptChanges()

                        'SQLコマンド設定
                        'Using cmdSQL As New SqlCommand(MY_DISP_ID & "_U02")
                        cmdSQL = New SqlCommand(MY_DISP_ID + "_U02")

                        'パラメータ設定
                        cmdSQL.Parameters.AddRange({New SqlParameter("@mail_no1", strPrmAry(0)) _
                                                  , New SqlParameter("@mail_no2", strPrmAry(1)) _
                                                  , New SqlParameter("@mail_no3", strPrmAry(2)) _
                                                  , New SqlParameter("@usr_id", strPrmAry(6)) _
                                                  , New SqlParameter("@detail", dtDetail) _
                                                   })

                        cmdSQL.Parameters("@detail").SqlDbType = SqlDbType.Structured

                        arySQLCmd(1) = cmdSQL.Clone

                        ''データ更新
                        'If ClsCMDataConnect.pfExec_StoredProcedure(Me, "スケジュールの更新", cmdSQL) = False Then

                        '    Exit Sub

                        'End If

                        '登録・更新実行
                        If ClsCMDataConnect.pfExec_StoredProcedure(Me, arySQLCmd, intReturn) = False Then

                            'メッセージ表示
                            Select Case intReturn
                                Case -1
                                    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                                Case 0
                                    psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【SQLエラー】" + "スケジュール登録")       '{0}に失敗しました。
                                Case 1
                                    psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【SQLエラー】" + "スケジュール明細登録")       '{0}に失敗しました。
                            End Select

                            Exit Sub

                        End If


                        'End Using

                    End Using

                Case Else

                    Using dtDetail As DataTable = DirectCast(ViewState(VS_DETAIL), DataTable).Copy


                        '不要項目を削除
                        With dtDetail.Columns
                            .Remove("作業者")
                            .Remove("営業所")
                            .Remove("エリア")
                            .Remove("責任者1")
                            .Remove("責任者2")
                            .Remove("出発日時")
                            .Remove("出発連絡")
                            .Remove("到着日時")
                            .Remove("到着連絡")
                            .Remove("出発送信回数")
                            .Remove("到着送信回数")
                            .Remove("変更不可")
                        End With

                        '変更を反映
                        dtDetail.AcceptChanges()

                        'SQLコマンド設定
                        Using cmdSQL As New SqlCommand(MY_DISP_ID & "_U02")
                            'cmdSQL = New SqlCommand(MY_DISP_ID + "_U02")

                            'パラメータ設定
                            cmdSQL.Parameters.AddRange({New SqlParameter("@mail_no1", strPrmAry(0)) _
                                                      , New SqlParameter("@mail_no2", strPrmAry(1)) _
                                                      , New SqlParameter("@mail_no3", strPrmAry(2)) _
                                                      , New SqlParameter("@usr_id", strPrmAry(6)) _
                                                      , New SqlParameter("@detail", dtDetail) _
                                                       })

                            cmdSQL.Parameters("@detail").SqlDbType = SqlDbType.Structured

                            'データ更新
                            If ClsCMDataConnect.pfExec_StoredProcedure(Me, "スケジュールの確定", cmdSQL) = False Then

                                Exit Sub

                            End If

                        End Using

                    End Using

            End Select

            '明細クリア
            ClearDtl()

            '活性制御
            setEnable(ViewState(VS_TERMS), EditMode.登録)

            'データ取得
            getData(ViewState(VS_PRMS))

            '正常終了メッセージ
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "スケジュールの確定")
            'SCLUPDP001_004 END

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "スケジュールの確定")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 削除ボタン(フッター)　押下時処理
    ''' </summary>
    Protected Sub btnDelete_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        'SCLUPDP001_004
        '整合性チェック
        If CheckWkmCd(3) = False Then
            'エラーメッセージ表示
            psMesBox(Me, "00000", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "出発連絡の期間が開始している為、削除できません。")
            Exit Sub
        End If
        'SCLUPDP001_004 END

        '削除処理
        DeleteSchedule()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 明細クリアボタン　押下時処理
    ''' </summary>
    Protected Sub btnClearDtl_Click(sender As Object, e As EventArgs) Handles btnClearDtl.Click

        '開始ログ出力
        psLogStart(Me)

        Try

            '明細クリア
            ClearDtl()

            '活性制御
            setEnable(ViewState(VS_TERMS), EditMode.登録)

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細のクリア")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 明細登録ボタン押下時処理
    ''' </summary>
    Protected Sub btnInsertDtl_Click(sender As Object, e As EventArgs) Handles btnInsertDtl.Click


        '入力値検証
        If Me.IsValid() = False Then
            Exit Sub
        End If

        '整合性チェック
        If CheckWkmCd(0) = False Then
            Exit Sub
        End If

        '開始ログ出力
        psLogStart(Me)

        Dim drwNew As DataRow

        Try
            Using dtDetail As DataTable = DirectCast(ViewState(VS_DETAIL), DataTable)

                '新規登録行
                drwNew = dtDetail.NewRow

                'SEQ ここで振ったSEQがDBに登録されるとは限りません(ストアドで振ります)
                With dtDetail.Select("SEQ = MAX(SEQ)")
                    If .Count > 0 Then
                        drwNew("SEQ") = .ElementAt(0)("SEQ") + 1
                    Else
                        drwNew("SEQ") = 0
                    End If
                End With

                '作業者情報
                drwNew("作業者コード") = txtWkmCd.ppText
                drwNew("作業者") = txtWkmCd.ppText & ":" & lblWkmLNm.Text & " " & lblWkmFNm.Text
                drwNew("作業者姓") = lblWkmLNm.Text
                drwNew("作業者名") = lblWkmFNm.Text
                drwNew("営業所コード") = hdnMntCd.Value
                drwNew("営業所") = lblMntNm.Text
                drwNew("エリア") = lblArea.Text
                drwNew("範囲コード") = hdnAreaCd.Value
                drwNew("責任者1") = lblAdm1.Text
                drwNew("責任者1コード") = hdnAdm1Cd.Value
                drwNew("責任者1姓") = hdnAdm1LNm.Value
                drwNew("責任者1名") = hdnAdm1FNm.Value
                drwNew("責任者2") = lblAdm2.Text
                drwNew("責任者2コード") = hdnAdm2Cd.Value
                drwNew("責任者2姓") = hdnAdm2LNm.Value
                drwNew("責任者2名") = hdnAdm2FNm.Value

                '出発日時
                drwNew("出発日時") = txtDptDt.ppText & " " & txtDptDt.ppHourText & ":" & txtDptDt.ppMinText
                drwNew("TRN_DEP_DAY") = txtDptDt.ppText.Replace("/", "")
                drwNew("TRN_DEP_TIM") = txtDptDt.ppHourText & txtDptDt.ppMinText & "00"
                drwNew("出発連絡") = "無"

                '到着日時
                drwNew("到着日時") = lblArrDt.Text
                drwNew("TRN_ARR_DAY") = lblArrDt.Text.Substring(0, 10).Replace("/", "")
                drwNew("TRN_ARR_TIM") = lblArrDt.Text.Substring(11, 5).Replace(":", "") & "00"
                drwNew("到着連絡") = "無"

                '送信回数
                drwNew("出発送信回数") = "0回"
                drwNew("到着送信回数") = "0回"

                '削除フラグ
                drwNew("削除") = "0"

                '停止区分
                drwNew("停止") = "0"

                'データ区分
                drwNew("データ区分") = DataCls.新規   '(0:既存 1:新規 2:更新)

                '変更不可
                drwNew("変更不可") = "0"

                '明細に追加
                dtDetail.Rows.Add(drwNew)

                '変更を反映
                dtDetail.AcceptChanges()

                '編集用に保存
                ViewState(VS_DETAIL) = dtDetail.Copy

                '一覧にバインド
                grvList.DataSource = dtDetail.Select(FilterDel)
                grvList.DataBind()

            End Using

            '明細クリア
            ClearDtl()

            '活性制御
            setEnable(ViewState(VS_TERMS), EditMode.登録)

            '正常終了メッセージ
            psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "スケジュール明細を登録しました。" & EditMes)

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細の登録")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 明細更新ボタン　押下時処理
    ''' </summary>
    Protected Sub btnUpdateDtl_Click(sender As Object, e As EventArgs) Handles btnUpdateDtl.Click

        '入力値検証
        If Me.IsValid() = False Then
            Exit Sub
        End If

        '整合性チェック
        If CheckWkmCd(0) = False Then
            Exit Sub
        End If

        '開始ログ出力
        psLogStart(Me)

        Dim strFilter As String
        Dim strSEQNO As String

        Try
            '連番取得
            strSEQNO = hdnSEQ.Value

            '連番指定
            strFilter = String.Format("SEQ = {0}", strSEQNO)

            Using dtDetail As DataTable = DirectCast(ViewState(VS_DETAIL), DataTable)

                With dtDetail.Select(strFilter)(0)

                    '作業者情報
                    .Item("作業者コード") = txtWkmCd.ppText
                    .Item("作業者") = txtWkmCd.ppText & ":" & lblWkmLNm.Text & " " & lblWkmFNm.Text
                    .Item("作業者姓") = lblWkmLNm.Text
                    .Item("作業者名") = lblWkmFNm.Text
                    .Item("営業所コード") = hdnMntCd.Value
                    .Item("営業所") = lblMntNm.Text
                    .Item("エリア") = lblArea.Text
                    .Item("範囲コード") = hdnAreaCd.Value
                    .Item("責任者1") = lblAdm1.Text
                    .Item("責任者1コード") = hdnAdm1Cd.Value
                    .Item("責任者1姓") = hdnAdm1LNm.Value
                    .Item("責任者1名") = hdnAdm1FNm.Value
                    .Item("責任者2") = lblAdm2.Text
                    .Item("責任者2コード") = hdnAdm2Cd.Value
                    .Item("責任者2姓") = hdnAdm2LNm.Value
                    .Item("責任者2名") = hdnAdm2FNm.Value

                    '出発日時
                    .Item("出発日時") = txtDptDt.ppText & " " & txtDptDt.ppHourText & ":" & txtDptDt.ppMinText
                    .Item("TRN_DEP_DAY") = txtDptDt.ppText.Replace("/", "")
                    .Item("TRN_DEP_TIM") = txtDptDt.ppHourText & txtDptDt.ppMinText & "00"
                    '.Item("出発連絡") = "無"

                    '到着日時
                    .Item("到着日時") = lblArrDt.Text
                    .Item("TRN_ARR_DAY") = lblArrDt.Text.Substring(0, 10).Replace("/", "")
                    .Item("TRN_ARR_TIM") = lblArrDt.Text.Substring(11, 5).Replace(":", "") & "00"
                    '.Item("到着連絡") = "無"

                    '削除フラグ
                    .Item("削除") = "0"

                    'データ区分
                    If .Item("データ区分") = DataCls.既存 Then
                        .Item("データ区分") = DataCls.更新    '(0:既存 1:新規 2:更新)
                    End If

                End With

                '変更を反映
                dtDetail.AcceptChanges()

                '編集用に保存
                ViewState(VS_DETAIL) = dtDetail.Copy

                '一覧にバインド
                grvList.DataSource = dtDetail.Select(FilterDel)
                grvList.DataBind()

            End Using

            '明細クリア
            ClearDtl()

            '活性制御
            setEnable(ViewState(VS_TERMS), EditMode.登録)

            '正常終了メッセージ
            psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "スケジュール明細を更新しました。" & EditMes)

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細の登録")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 明細削除ボタン　押下時処理
    ''' </summary>
    Protected Sub btnDeleteDtl_Click(sender As Object, e As EventArgs) Handles btnDeleteDtl.Click

        '開始ログ出力
        psLogStart(Me)

        Dim strFilter As String
        Dim strSEQNO As String

        Try
            '連番取得
            strSEQNO = hdnSEQ.Value

            '連番指定
            strFilter = String.Format("SEQ = {0}", strSEQNO)

            '整合性チェック
            If CheckWkmCd(1) = False Then
                Exit Sub
            End If

            Using dtDetail As DataTable = DirectCast(ViewState(VS_DETAIL), DataTable)

                '対象の行を削除
                With dtDetail.Select(strFilter)(0)

                    'SCLLSTP001-002 [.Item("データ区分") <> DataCls.既存] ⇒ [.Item("データ区分") = DataCls.新規]に変更
                    If ViewState(VS_TERMS) = ClsComVer.E_遷移条件.登録 OrElse .Item("データ区分") = DataCls.新規 Then

                        '新規作成データの場合、DELETE
                        .Delete()

                    Else

                        '既存データの場合、論理削除
                        .Item("削除") = "1"
                        .Item("データ区分") = DataCls.更新

                    End If

                End With

                '変更を反映
                dtDetail.AcceptChanges()

                '編集用に保存
                ViewState(VS_DETAIL) = dtDetail.Copy

                '一覧にバインド
                grvList.DataSource = dtDetail.Select(FilterDel)
                grvList.DataBind()

            End Using

            '明細クリア
            ClearDtl()

            '活性制御
            setEnable(ViewState(VS_TERMS), EditMode.登録)

            '正常終了メッセージ
            If grvList.Rows.Count = 0 Then
                psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "スケジュール明細を削除しました。\nスケジュールを削除するかスケジュール明細を追加して下さい。")
            Else
                psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "スケジュール明細を削除しました。" & EditMes)
            End If

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細の登録")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' グリッドのボタン　押下時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        Dim rowData As GridViewRow = Nothing        'ボタン押下行
        Dim strFilter As String
        Dim strSEQNO As String

        If e.CommandName <> "btnSelect" AndAlso e.CommandName <> "btnStop" Then
            Exit Sub
        End If

        '開始ログ出力
        psLogStart(Me)

        'フォーカス
        txtWkmCd.ppTextBox.Focus()

        'ボタン押下行の情報を取得
        rowData = grvList.Rows(Convert.ToInt32(e.CommandArgument))

        '連番取得
        strSEQNO = DirectCast(rowData.FindControl("SEQ"), TextBox).Text

        '連番指定
        strFilter = String.Format("SEQ = {0}", strSEQNO)

        Try
            Using dtDetail As DataTable = DirectCast(ViewState(VS_DETAIL), DataTable)

                Select Case e.CommandName
                    Case "btnStop"  '停止/解除

                        Dim btnStop As Button = DirectCast(rowData.Cells(0).Controls(0), Button)

                        '
                        With dtDetail.Select(strFilter)(0)

                            If .Item("停止").ToString = "0" Then

                                '停止
                                .Item("停止") = "1"

                                If .Item("データ区分") = DataCls.既存 Then
                                    .Item("データ区分") = DataCls.更新
                                End If

                                'ボタン変更
                                btnStop.BackColor = Drawing.Color.FromArgb(255, 102, 102)
                                rowData.Cells(1).Enabled = False

                                '編集中のレコードを停止した場合
                                If hdnSEQ.Value = strSEQNO Then

                                    '明細クリア
                                    ClearDtl()

                                    '活性制御
                                    setEnable(ViewState(VS_TERMS), EditMode.登録)

                                End If

                                '正常終了メッセージ
                                psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "スケジュールを停止対象に変更しました" & EditMes)

                            Else

                                '解除
                                .Item("停止") = "0"

                                If .Item("データ区分") = DataCls.既存 Then
                                    .Item("データ区分") = DataCls.更新
                                End If

                                'ボタン変更
                                btnStop.BackColor = Drawing.Color.Empty

                                If .Item("変更不可") = "0" Then
                                    rowData.Cells(1).Enabled = True
                                End If

                                '正常終了メッセージ
                                psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "スケジュールを稼働対象に変更しました" & EditMes)

                            End If

                        End With

                        '変更を反映
                        dtDetail.AcceptChanges()

                        '編集用に保存
                        ViewState(VS_DETAIL) = dtDetail.Copy


                    Case "btnSelect"    '選択

                        With dtDetail.Select(strFilter)(0)

                            'SEQ
                            hdnSEQ.Value = .Item("SEQ").ToString

                            '作業者情報
                            txtWkmCd.ppText = .Item("作業者コード").ToString
                            lblWkmLNm.Text = .Item("作業者姓").ToString
                            lblWkmFNm.Text = .Item("作業者名").ToString
                            hdnMntCd.Value = .Item("営業所コード").ToString
                            lblMntNm.Text = .Item("営業所").ToString
                            lblArea.Text = .Item("エリア").ToString
                            hdnAreaCd.Value = .Item("範囲コード").ToString
                            lblAdm1.Text = .Item("責任者1").ToString
                            hdnAdm1Cd.Value = .Item("責任者1コード").ToString
                            hdnAdm1LNm.Value = .Item("責任者1姓").ToString
                            hdnAdm1FNm.Value = .Item("責任者1名").ToString
                            lblAdm2.Text = .Item("責任者2").ToString
                            hdnAdm2Cd.Value = .Item("責任者2コード").ToString
                            hdnAdm2LNm.Value = .Item("責任者2姓").ToString
                            hdnAdm2FNm.Value = .Item("責任者2名").ToString

                            '出発日時
                            txtDptDt.ppDate = Date.Parse(.Item("出発日時"))

                            '到着日時
                            lblArrDt.Text = Date.Parse(.Item("到着日時")).ToString("yyyy/MM/dd HH:mm")

                        End With

                        '活性制御
                        setEnable(ViewState(VS_TERMS), EditMode.更新)

                End Select

            End Using

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細の登録")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' GridViewデータバインド
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        Try

            ''確定ボタンメッセージ設定
            'If grvList.Rows.Count = 0 Then
            '    Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "スケジュールのみを確定します。\nよろしいですか？")
            'Else
            '    Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "スケジュール")
            'End If

            '行ループ
            For Each grvrow As GridViewRow In grvList.Rows

                'ボタン編集
                If DirectCast(grvrow.FindControl("停止"), TextBox).Text = "1" Then

                    '.Text = "解除"
                    DirectCast(grvrow.Cells(0).Controls(0), Button).BackColor = Drawing.Color.FromArgb(255, 102, 102)

                    '選択ボタン非活性
                    grvrow.Cells(1).Enabled = False

                End If

                '変更不可レコード
                If DirectCast(grvrow.FindControl("変更不可"), TextBox).Text = "1" Then

                    '選択ボタン非活性
                    grvrow.Cells(1).Enabled = False

                End If

                '到着連絡
                If DirectCast(grvrow.FindControl("到着連絡"), TextBox).Text = "有" Then

                    '停止ボタン非活性
                    grvrow.Cells(0).Enabled = False

                End If

            Next

        Catch ex As Exception

            'メッセージ出力
            psMesBox(Me, "00000", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "一覧の表示編集処理に失敗しました。")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub


#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' データ取得
    ''' </summary>
    Private Sub getData(ByVal strPrmAry() As String)

        Dim dstSelect As New DataSet

        'SQLコマンド設定
        Using cmdSQL As New SqlCommand(MY_DISP_ID & "_S01")

            'パラメータ設定
            cmdSQL.Parameters.AddRange({New SqlParameter("@mail_no1", strPrmAry(0)) _
                                      , New SqlParameter("@mail_no2", strPrmAry(1)) _
                                      , New SqlParameter("@mail_no3", strPrmAry(2)) _
                                      , New SqlParameter("@vst_cls", strPrmAry(3)) _
                                      , New SqlParameter("@wrk_day", strPrmAry(4)) _
                                      , New SqlParameter("@wrk_tim", strPrmAry(5)) _
                                       })

            'データ更新
            If ClsCMDataConnect.pfExec_StoredProcedure(Me, "スケジュール明細", cmdSQL, dstSelect) = False Then

                Exit Sub

            End If

        End Using

        '該当件数表示
        If dstSelect.Tables(0).Rows.Count = 0 Then

            'SCLUPDP001_004
            '該当件数
            'Master.ppCount = "0"
            '該当データ無し
            'psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)

            'Exit Sub
            'SCLUPDP001_004 END

        Else
            'SCLUPDP001_004
            '画面反映
            lblMailNo.Text = strPrmAry(0) & "-" & strPrmAry(1) & "-" & strPrmAry(2)
            With dstSelect.Tables(0).Rows(0)
                ddlVstCls.ppSelectedValue = .Item("訪問種別").ToString
                lblCtrlNo.Text = .Item("管理番号").ToString
                txtWrkDt.ppDate = Date.Parse(.Item("作業日").ToString & " " & .Item("作業時間").ToString)
                lblVstCd.Text = .Item("訪問先コード").ToString
                txtVstNm.ppText = .Item("訪問先名称").ToString
            End With
            'SCLLSTP001-002
            '工事・保守以外は訪問先コード非表示
            If ddlVstCls.ppSelectedValue = "01" OrElse ddlVstCls.ppSelectedValue = "02" Then
                lblVstCd.Visible = True
            Else
                lblVstCd.Visible = False
            End If
            'SCLLSTP001-002 END
            'SCLUPDP001_004 END
        End If

        '編集用に保存
        ViewState(VS_DETAIL) = dstSelect.Tables(1).Copy

        '一覧にバインド
        grvList.DataSource = dstSelect.Tables(1)
        grvList.DataBind()

        'Dispose
        dstSelect.Dispose()

    End Sub

    ''' <summary>
    ''' スケジュール削除
    ''' </summary>
    ''' <remarks>Main-Detailを削除し、画面を登録モードに変更</remarks>
    Private Sub DeleteSchedule()

        Dim strPrmAry(6) As String

        Try

            '登録内容 格納
            strPrmAry = ViewState(VS_PRMS)
            strPrmAry = strPrmAry.Concat({Session(P_SESSION_USERID).ToString}).ToArray

            'SQLコマンド設定
            Using cmdSQL As New SqlCommand(MY_DISP_ID & "_D01")

                'パラメータ設定
                cmdSQL.Parameters.AddRange({New SqlParameter("@mail_no1", strPrmAry(0)) _
                                          , New SqlParameter("@mail_no2", strPrmAry(1)) _
                                          , New SqlParameter("@mail_no3", strPrmAry(2)) _
                                          , New SqlParameter("@vst_cls", strPrmAry(3)) _
                                          , New SqlParameter("@wrk_day", strPrmAry(4)) _
                                          , New SqlParameter("@wrk_tim", strPrmAry(5)) _
                                          , New SqlParameter("@usr_id", strPrmAry(6)) _
                                           })

                'データ更新
                If ClsCMDataConnect.pfExec_StoredProcedure(Me, "スケジュールの削除", cmdSQL) = False Then

                    Exit Sub

                End If

            End Using

            'ヘッダ項目クリア
            ddlVstCls.ppDropDownList.SelectedIndex = -1
            lblMailNo.Text = String.Empty
            lblCtrlNo.Text = String.Empty
            txtWrkDt.ppText = String.Empty
            txtWrkDt.ppHourText = String.Empty
            txtWrkDt.ppMinText = String.Empty
            lblVstCd.Text = String.Empty
            txtVstNm.ppText = String.Empty

            '明細クリア
            ClearDtl()

            '一覧表示
            grvList.DataSource = New DataTable
            grvList.DataBind()

            '登録モードに変更
            ViewState(VS_TERMS) = ClsComVer.E_遷移条件.登録
            ViewState(VS_PRMS) = Nothing
            ViewState(VS_DETAIL) = Nothing

            'DropDownList設定
            setDropDownList()

            '活性制御
            setEnable(ViewState(VS_TERMS))

            'フォーカス
            ddlVstCls.ppDropDownList.Focus()

            '正常終了メッセージ
            psMesBox(Me, "00002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "スケジュール")

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "スケジュールの削除")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

    End Sub

    ''' <summary>
    ''' DropDownList設定
    ''' </summary>
    Private Sub setDropDownList()

        Dim dstSelect As New DataSet

        'SQLコマンド設定
        Using cmdSQL As New SqlCommand(SCL_COMMON_S01)

            'パラメータ設定
            cmdSQL.Parameters.Add(New SqlParameter("@data_cls", "SCL_MST_VSC"))

            'データ取得
            If ClsCMDataConnect.pfExec_StoredProcedure(Me, "訪問種別マスタ", cmdSQL, dstSelect) = False Then

                Exit Sub

            End If

        End Using

        '訪問先種別
        If ViewState(VS_TERMS) = ClsComVer.E_遷移条件.登録 Then
            ddlVstCls.ppDropDownList.DataSource = dstSelect.Tables(0).Select("コード <> '01' AND コード <> '02'").CopyToDataTable
        Else
            ddlVstCls.ppDropDownList.DataSource = dstSelect.Tables(0)
        End If
        ddlVstCls.ppDropDownList.DataValueField = "コード"
        ddlVstCls.ppDropDownList.DataTextField = "コード名称"
        ddlVstCls.ppDropDownList.DataBind()
        ddlVstCls.ppDropDownList.Items.Insert(0, String.Empty)

        'Dispose
        dstSelect.Dispose()

    End Sub

    ''' <summary>
    ''' 活性制御
    ''' </summary>
    Private Sub setEnable(ByVal Term As ClsComVer.E_遷移条件, Optional ByVal intEditMode As EditMode = 0)

        'SCLUPDP001_004
        Dim dtDtl As DataTable = ViewState(VS_DETAIL)
        Dim dsMailMst As DataSet = ViewState(VS_MAILMST)
        Dim intRowCnt As Integer = 0
        'SCLUPDP001_004 END

        Select Case Term
            Case ClsComVer.E_遷移条件.参照

                ddlVstCls.ppEnabled = False
                txtWrkDt.ppEnabled = False
                'txtVstCd.ppEnabled = False
                txtVstNm.ppEnabled = False
                btnInsert.Enabled = False
                btnClear.Enabled = False

                'Detali
                txtWkmCd.ppEnabled = False
                txtDptDt.ppEnabled = False
                btnClearDtl.Enabled = False
                btnInsertDtl.Enabled = False
                btnUpdateDtl.Enabled = False
                btnDeleteDtl.Enabled = False

                'フッター
                Master.ppLeftButton1.Enabled = False
                Master.ppRigthButton1.Enabled = False
                Master.ppRigthButton2.Enabled = False

                '一覧
                grvList.Enabled = False

            Case ClsComVer.E_遷移条件.登録

                'Main
                Select Case intEditMode
                    Case EditMode.登録
                        ddlVstCls.ppEnabled = False
                        'txtCtrlNo.ppEnabled = True
                        txtWrkDt.ppEnabled = True
                        'txtVstCd.ppEnabled = True
                        txtVstNm.ppEnabled = True

                        btnInsert.Enabled = True
                        btnClear.Enabled = True
                    Case Else
                        ddlVstCls.ppEnabled = True
                        'txtCtrlNo.ppEnabled = False
                        txtWrkDt.ppEnabled = False
                        'txtVstCd.ppEnabled = False
                        txtVstNm.ppEnabled = False

                        btnInsert.Enabled = False
                        btnClear.Enabled = False
                End Select

                'txtCtrlNo.ppEnabled = False

                'Detali
                txtWkmCd.ppEnabled = False
                txtDptDt.ppEnabled = False

                btnClearDtl.Enabled = False
                btnInsertDtl.Enabled = False
                btnUpdateDtl.Enabled = False
                btnDeleteDtl.Enabled = False

                'フッター
                Master.ppLeftButton1.Enabled = False
                Master.ppRigthButton1.Enabled = False
                Master.ppRigthButton2.Enabled = False

            Case ClsComVer.E_遷移条件.更新

                'Main
                ddlVstCls.ppEnabled = False
                'txtCtrlNo.ppEnabled = False
                txtWrkDt.ppEnabled = False
                'txtVstCd.ppEnabled = False
                txtVstNm.ppEnabled = False

                btnInsert.Enabled = False
                btnClear.Enabled = False

                'Detali
                txtWkmCd.ppEnabled = True
                'txtDptDt.ppEnabled = False
                'txtArrDt.ppEnabled = True

                Select Case intEditMode
                    Case EditMode.登録
                        txtDptDt.ppEnabled = False
                        btnClearDtl.Enabled = True
                        btnInsertDtl.Enabled = False
                        btnUpdateDtl.Enabled = False
                        btnDeleteDtl.Enabled = False
                    Case EditMode.更新
                        txtDptDt.ppEnabled = True
                        btnClearDtl.Enabled = True
                        btnInsertDtl.Enabled = False
                        btnUpdateDtl.Enabled = True
                        btnDeleteDtl.Enabled = True
                    Case Else
                        txtDptDt.ppEnabled = False
                        btnClearDtl.Enabled = False
                        btnInsertDtl.Enabled = False
                        btnUpdateDtl.Enabled = False
                        btnDeleteDtl.Enabled = False
                End Select

                'フッター
                Master.ppLeftButton1.Enabled = True
                Master.ppRigthButton1.Enabled = True

                '削除ボタンの制御
                intRowCnt = 0
                If dtDtl.Rows.Count > 0 Then
                    For intCnt As Integer = 0 To dtDtl.Rows.Count - 1
                        If Date.Parse(dtDtl.Rows(intCnt).Item("出発日時")).AddMinutes(-1 * dsMailMst.Tables(0).Rows(0).Item("MSA_DEP_MIN")) < DateTime.Now Then
                            intRowCnt += 1
                        End If
                    Next
                End If

                If intRowCnt > 0 Then
                    Master.ppRigthButton1.Enabled = False
                Else
                    Master.ppRigthButton1.Enabled = True
                End If

                'SCLUPDP001_004
                '削除=1のデータを除く、明細数が1件以上の場合のみ活性する
                intRowCnt = 0
                If dtDtl.Rows.Count > 0 Then
                    For intCnt As Integer = 0 To dtDtl.Rows.Count - 1
                        If dtDtl.Rows(intCnt).Item("削除") = 0 Then
                            intRowCnt += 1
                        End If
                    Next
                End If
                'SCLUPDP001_004 END

                If intRowCnt > 0 Then
                    Master.ppRigthButton2.Enabled = True
                Else
                    Master.ppRigthButton2.Enabled = False
                End If

        End Select



    End Sub

    ''' <summary>
    ''' 名称取得
    ''' </summary>
    Private Function getName(ByVal strDataName As String, ByVal strSQLPrm() As String, ByRef strReturn As String) As Boolean

        getName = False

        Dim dstSelect As New DataSet

        Try
            'SQLコマンド設定
            Using cmdSQL As New SqlCommand(SCL_COMMON_S01)

                'パラメータ設定
                cmdSQL.Parameters.AddRange({New SqlParameter("@data_cls", strSQLPrm(0)) _
                                          , New SqlParameter("@key_01", strSQLPrm(1)) _
                                           })

                'データ取得
                If ClsCMDataConnect.pfExec_StoredProcedure(Me, strDataName, cmdSQL, dstSelect) = False Then

                    Exit Function

                End If

            End Using

            '取得値を反映
            If dstSelect.Tables(0).Rows.Count = 0 Then
                strReturn = String.Empty
            Else
                strReturn = dstSelect.Tables(0).Rows(0)("名称").ToString
            End If

            'Dispose
            dstSelect.Dispose()

            Return True

        Catch ex As Exception

            'エラー
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strDataName + "の取得")       '{0}の取得に失敗しました。

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

    End Function

    ''' <summary>
    ''' 明細クリア
    ''' </summary>
    Private Sub ClearDtl()

        'SEQ
        hdnSEQ.Value = String.Empty

        '作業者情報
        txtWkmCd.ppText = String.Empty
        lblWkmLNm.Text = String.Empty
        lblWkmFNm.Text = String.Empty
        lblMntNm.Text = String.Empty
        hdnMntCd.Value = String.Empty
        lblArea.Text = String.Empty
        hdnAreaCd.Value = String.Empty
        lblAdm1.Text = String.Empty
        hdnAdm1Cd.Value = String.Empty
        hdnAdm1LNm.Value = String.Empty
        hdnAdm1FNm.Value = String.Empty
        lblAdm2.Text = String.Empty
        hdnAdm2Cd.Value = String.Empty
        hdnAdm2LNm.Value = String.Empty
        hdnAdm2FNm.Value = String.Empty

        '出発日時
        txtDptDt.ppText = String.Empty
        txtDptDt.ppHourText = String.Empty
        txtDptDt.ppMinText = String.Empty

        '到着日時
        lblArrDt.Text = String.Empty

        'フォーカス
        txtWkmCd.ppTextBox.Focus()

    End Sub

    ''' <summary>
    ''' 整合性チェック
    ''' </summary>
    Private Function CheckWkmCd(ByVal intCls As Integer, Optional ByRef dtWrkInf As DataTable = Nothing) As Boolean

        'SCLUPDP001_004
        Dim dsMailMst As DataSet = ViewState(VS_MAILMST)
        Dim dtDetail As DataTable = ViewState(VS_DETAIL)
        CheckWkmCd = True

        Select Case intCls
            Case 0      '明細登録、明細更新

                '出発日時
                If dtWrkInf Is Nothing Then

                    '到着日時
                    'If txtDptDt.ppDate < DateTime.Now Then
                    If Date.Parse(lblArrDt.Text).AddMinutes(-1 * dsMailMst.Tables(0).Rows(0).Item("MSA_ARR_MIN")) < DateTime.Now Then
                        '過去日登録
                        'txtDptDt.psSet_ErrorNo("1006", txtDptDt.ppName, "現在日時")    '「{0}」は「{1}」以降の日時で入力してください。
                        txtDptDt.psSet_ErrorNo("1007", "到着連絡の期間")    '「{0}」は「{1}」以降の日時で入力してください。

                        CheckWkmCd = False

                    ElseIf txtDptDt.ppDate > Date.Parse(lblArrDt.Text) Then

                        '到着以前
                        txtDptDt.psSet_ErrorNo("1008", txtDptDt.ppName, lblArrDtNm.Text)    '「{0}」は「{1}」以内の日付で入力してください。

                        CheckWkmCd = False

                    ElseIf Date.Parse(lblArrDt.Text).AddHours(-24) > txtDptDt.ppDate Then

                        '到着日時の24時間以上前のデータは登録不可
                        txtDptDt.psSet_ErrorNo("1009", txtDptDt.ppName, lblArrDtNm.Text)    '「{0}」 は「{1}」 の24時間以内の日時を入力してください。

                        CheckWkmCd = False

                    End If

                End If


                Dim dstSelect As New DataSet

                'SQLコマンド設定
                Using cmdSQL As New SqlCommand(MY_DISP_ID & "_S02")

                    'パラメータ設定
                    cmdSQL.Parameters.Add(New SqlParameter("@wkm_cd", txtWkmCd.ppText))

                    'データ更新
                    If ClsCMDataConnect.pfExec_StoredProcedure(Me, "作業者情報", cmdSQL, dstSelect) = False Then

                        Return False

                    End If

                End Using

                '作業者情報
                dtWrkInf = dstSelect.Tables(0).Copy

                If dtWrkInf.Rows.Count = 0 Then

                    '作業者コードは存在しません
                    txtWkmCd.psSet_ErrorNo("2002", "入力した作業者コード")  '{0} は存在しません。再度確認し、入力してください。

                    CheckWkmCd = False

                Else

                    With dtWrkInf.Rows(0)

                        'If .Item("範囲管理区分").ToString = "9" Then

                        '    txtWkmCd.psSet_ErrorNo("2000", "入力した作業者は、スケジュール管理対象外です。社員マスタの範囲管理区分を確認して下さい。")

                        '    CheckWkmCd = False

                        'Elseif
                        If .Item("携帯電話番号").ToString = "" Then

                            txtWkmCd.psSet_ErrorNo("2000", "入力した作業者は、携帯電話番号が登録されていません。")

                            CheckWkmCd = False

                        ElseIf .Item("責任者コード").ToString = "" Then

                            txtWkmCd.psSet_ErrorNo("2000", "入力した作業者は、責任者が存在しません。")

                            CheckWkmCd = False

                        Else
                            '同一作業者
                            If ViewState(VS_DETAIL) IsNot Nothing Then

                                Dim strSEQ As String

                                If hdnSEQ.Value = String.Empty Then
                                    strSEQ = "-1"
                                Else
                                    strSEQ = hdnSEQ.Value
                                End If

                                If DirectCast(ViewState(VS_DETAIL), DataTable).Select("作業者コード = '" & txtWkmCd.ppText.Trim & "' AND 削除 = 0 AND SEQ <> " & strSEQ).Count > 0 Then

                                    txtWkmCd.psSet_ErrorNo("2006", "入力した作業者")

                                    CheckWkmCd = False

                                End If

                            End If

                        End If

                    End With

                    '破棄
                    dstSelect.Dispose()

                End If

            Case 1      '明細削除
                '出発日時
                If txtDptDt.ppDate.AddMinutes(-1 * dsMailMst.Tables(0).Rows(0).Item("MSA_DEP_MIN")) < DateTime.Now Then
                    '過去日削除
                    txtDptDt.psSet_ErrorNo("1010", "出発連絡の期間")    '{0} を過ぎている為、削除できません。

                    CheckWkmCd = False


                End If

            Case 2      '確定

            Case 3      '削除
                '明細の出発日時
                If dtDetail.Rows.Count > 0 Then
                    For intRowCnt As Integer = 0 To dtDetail.Rows.Count - 1
                        If Date.Parse(dtDetail.Rows(intRowCnt).Item("出発日時")).AddMinutes(-1 * dsMailMst.Tables(0).Rows(0).Item("MSA_DEP_MIN")) < DateTime.Now Then
                            CheckWkmCd = False
                        End If
                    Next
                End If

            Case 4      '本登録

                If txtWrkDt.ppDate.AddMinutes(-1 * dsMailMst.Tables(0).Rows(0).Item("MSA_ARR_MIN")) < DateTime.Now Then
                    '過去日登録
                    'txtDptDt.psSet_ErrorNo("1006", txtDptDt.ppName, "現在日時")    '「{0}」は「{1}」以降の日時で入力してください。
                    txtWrkDt.psSet_ErrorNo("1007", "到着連絡の期間")    '{0} を過ぎている為、削除できません。

                    CheckWkmCd = False
                End If

        End Select
        'SCLUPDP001_004 END

    End Function

    'SCLLSTP001-002
    ''' <summary>
    ''' 作業人数の表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msDispWorkerCount()
        '作業人数の表示
        lblCount.Text = grvList.Rows.Count.ToString
    End Sub
    'SCLLSTP001-002 END

    ' ''' <summary>
    ' ''' 依頼番号チェック
    ' ''' </summary>
    'Private Function checkCtrlNo(ByVal strVstCls As String, ByVal strCtrlNo As String) As Boolean

    '    checkCtrlNo = False

    '    Dim dstSelect As New DataSet

    '    Try
    '        'SQLコマンド設定
    '        Using cmdSQL As New SqlCommand(MY_DISP_ID & "_S03")

    '            'パラメータ設定
    '            cmdSQL.Parameters.AddRange({New SqlParameter("@vst_cls", strVstCls) _
    '                                      , New SqlParameter("@ctrl_no", strCtrlNo) _
    '                                       })

    '            'データ取得
    '            If ClsCMDataConnect.pfExec_StoredProcedure(Me, strVstCls, cmdSQL, dstSelect) = False Then

    '                Exit Function

    '            End If

    '        End Using

    '        '結果を返す
    '        If dstSelect.Tables(0).Rows.Count = 0 Then
    '            txtCtrlNo.psSet_ErrorNo("2002", "入力した依頼番号")  '{0} は存在しません。再度確認し、入力してください。
    '            Return False
    '        Else
    '            Return True
    '        End If


    '    Catch ex As Exception

    '        'エラー
    '        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strVstCls + "の取得")       '{0}の取得に失敗しました。

    '        'ログ出力
    '        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

    '    End Try

    'End Function

    'SCLUPDP001_004
    ''' <summary>
    ''' メール送信設定マスタ取得
    ''' </summary>
    Private Function psGetMailMaster() As DataSet

        Dim dstSelect As New DataSet
        Dim dtImport As New DataTable

        Try

            'SQLコマンド設定
            Using cmdSQL As New SqlCommand(MY_DISP_ID & "_S03")

                'パラメータ設定
                cmdSQL.Parameters.AddRange({})

                'SQL実行
                If ClsCMDataConnect.pfExec_StoredProcedure(Me, "スケジュール", cmdSQL, dstSelect) = False Then

                    psGetMailMaster = Nothing

                    Exit Function

                End If

            End Using

            '該当件数表示
            If dstSelect.Tables(0).Rows.Count = 0 Then
            Else
            End If

            '取得結果を戻り値に設定
            psGetMailMaster = dstSelect

        Catch ex As Exception

            '一覧にバインド
            grvList.DataSource = New DataTable
            grvList.DataBind()

            psGetMailMaster = Nothing

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "マスタデータの取得")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        Finally

            'Dispose
            If dstSelect IsNot Nothing Then
                dstSelect.Dispose()
            End If

        End Try

    End Function
    'SCLUPDP001_004 END

#End Region

End Class
