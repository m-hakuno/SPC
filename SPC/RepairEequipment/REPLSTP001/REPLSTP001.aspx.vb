'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜修理・整備業務＞
'*　処理名　　：　進捗一覧（修理・整備）
'*　ＰＧＭＩＤ：　REPLSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.07　：　NKC
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'REPLSTP001-001     2017/04/10      加賀  　  「旧シリアル」「新シリアル」で英字入力を許容

#Region "インポート定義"
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive
#End Region

Public Class REPLSTP001

#Region "継承定義"
    Inherits System.Web.UI.Page
#End Region

#Region "定数定義"
    '修理依頼書 参照／更新 画面のパス
    Private Const M_REPUPDP002 As String = "~/" & P_RPE & "/" &
        P_FUN_REP & P_SCR_UPD & P_PAGE & "002" & "/" &
        P_FUN_REP & P_SCR_UPD & P_PAGE & "002.aspx"

    '整備依頼書 参照／更新 画面のパス
    Private Const M_MNTUPDP001 As String = "~/" & P_RPE & "/" &
        P_FUN_MNT & P_SCR_UPD & P_PAGE & "001" & "/" &
        P_FUN_MNT & P_SCR_UPD & P_PAGE & "001.aspx"

    '画面ID
    Private Const M_DISP_ID = P_FUN_REP & P_SCR_LST & P_PAGE & "001"
    Private Const M_REPUPDP002_ID = P_FUN_REP & P_SCR_UPD & P_PAGE & "002"
    Private Const M_MNTUPDP001_ID = P_FUN_MNT & P_SCR_UPD & P_PAGE & "001"

    '初期表示件数
    Private Const DEFAULT_DISP As String = "1"

    '検索時件数
    Private Const DEFAULT_SEARCH As String = "2"

    ''' <summary>検索エラー</summary>
    Private Const sCnsErr_00004 As String = "00004"
    ''' <summary>接続エラー</summary>
    Private Const sCnsErr_00005 As String = "00005"
    ''' <summary>切断エラー</summary>
    Private Const sCnsErr_00006 As String = "00006"
    ''' <summary>閾値エラー</summary>
    Private Const sCnsErr_30005 As String = "30005"
    ''' <summary>該当データなし</summary>
    Private Const sCnsInfo_00007 As String = "00007"

    ''' <summary>進捗ステータスマスタ一覧取得処理</summary>
    Private Const sCnsSqlid_015 As String = "ZCMPSEL015"
    ''' <summary>業者マスタデータ取得処理</summary>
    Private Const sCnsSqlid_040 As String = "ZCMPSEL040"
    ''' <summary>部品マスタデータ取得処理</summary>
    Private Const sCnsSqlid_027 As String = "ZCMPSEL027"
    ''' <summary>ホールデータ取得処理</summary>
    Private Const sCnsSqlid_028 As String = "ZCMPSEL028"

#End Region

#Region "変数定義"

    Dim objStack As StackFrame
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom
    Dim clsExc As New ClsCMExclusive

#End Region

#Region "イベントプロシージャ"


    ''' <summary>
    ''' Page_Init.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '表設定
        pfSet_GridView(grvList, M_DISP_ID)
    End Sub

    ''' <summary>
    ''' ロード処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click
        AddHandler Master.ppRigthButton4.Click, AddressOf btnMenteAdd_Click
        AddHandler Me.txtTboxId.ppTextBox.TextChanged, AddressOf txtTboxId_TextChanged
        Me.txtTboxId.ppTextBox.AutoPostBack = True
        Master.ppRigthButton2.CausesValidation = False

        If Not IsPostBack Then '初回表示

            '後で追加
            '★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★
            'SPC,NGCの場合での活性制御処理
            '権限によってグリッドまたはコントロールの活性制御が変更される
            '★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★


            'プログラムＩＤ、画面名設定
            Master.Master.ppProgramID = M_DISP_ID
            Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

            '「修理登録」、「整備登録」のボタン活性
            Master.ppRigthButton4.Text = "整備登録"
            Master.ppRigthButton4.Visible = True

            'パンくずリスト設定
            Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

            '排他情報用のグループ番号保管.
            If Not Session(P_SESSION_GROUP_NUM) Is Nothing Then
                ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)
            End If

            '排他情報削除用の登録年月日時刻をマスタページのコントロールに登録.
            If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then
                Me.Master.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)
            End If

            '画面初期化処理.
            Call msInitScreen()

            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

        End If

    End Sub

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
                Master.ppRigthButton4.Enabled = False   '整備登録ボタン
        End Select

    End Sub

    ''' <summary>
    ''' データバインド時、削除フラグがある行の更新を非活性にする。
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound
        Dim strDelF As String
        For Each rowData As GridViewRow In grvList.Rows
            strDelF = CType(rowData.FindControl("削除"), TextBox).Text
            If strDelF = "●" Then   '削除フラグあり
                rowData.Cells(1).Enabled = False
                CType(rowData.FindControl("区分"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("修理先名"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("管理番号"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("枝番"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("ステータス"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("システム"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("機器種別"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("旧シリアル"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("ホール名称"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("対応日"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("機器発送日"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("機器到着日"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("一時診断結果"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("完了発送日"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("請求年月"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("削除"), TextBox).ForeColor = Drawing.Color.Red
            End If
        Next
    End Sub

    Private Sub grvList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grvList.RowDataBound

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
            Case "NGC"
                e.Row.Cells(1).Enabled = False
        End Select

    End Sub

    ''' <summary>
    ''' メーカ区分変更時処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlMakerKubun_TextChanged(sender As Object, e As EventArgs) Handles ddlMakerKubun.TextChanged

        If (Me.ddlMakerKubun.SelectedIndex = "0") Or (chkRepairState.Checked = False And chkMenteState.Checked = True) Then
            Me.ddlWorkNo1.Enabled = False
            Me.ddlWorkNo2.Enabled = False
            Me.ddlPrtsNo1.Enabled = False
            Me.ddlPrtsNo2.Enabled = False
            Me.ddlPrtsNo3.Enabled = False
            Me.ddlPrtsNo4.Enabled = False
            Me.ddlPrtsNo5.Enabled = False
            Me.ddlPrtsNo6.Enabled = False
            Me.ddlPrtsNo1.SelectedIndex = "0"
            Me.ddlPrtsNo2.SelectedIndex = "0"
            Me.ddlPrtsNo3.SelectedIndex = "0"
            Me.ddlPrtsNo4.SelectedIndex = "0"
            Me.ddlPrtsNo5.SelectedIndex = "0"
            Me.ddlPrtsNo6.SelectedIndex = "0"
        Else
            Me.ddlWorkNo1.Enabled = True
            Me.ddlWorkNo2.Enabled = True
            Me.ddlPrtsNo1.Enabled = True
            Me.ddlPrtsNo2.Enabled = True
            Me.ddlPrtsNo3.Enabled = True
            Me.ddlPrtsNo4.Enabled = True
            Me.ddlPrtsNo5.Enabled = True
            Me.ddlPrtsNo6.Enabled = True

            'ドロップダウンリスト再生成(メーカー関連).
            Call mfGet_DropDownListMaker(Me.ddlMakerKubun.SelectedValue)
            SetFocus(txtKanriNo.FindControl("txtTextBox"))
        End If
    End Sub

    Protected Sub chkState_CheckedChanged(sender As Object, e As EventArgs) Handles chkRepairState.CheckedChanged, chkMenteState.CheckedChanged

        If chkRepairState.Checked = False And chkMenteState.Checked = True Then
            Me.ddlWorkNo1.Enabled = False
            Me.ddlWorkNo2.Enabled = False
            Me.ddlPrtsNo1.Enabled = False
            Me.ddlPrtsNo2.Enabled = False
            Me.ddlPrtsNo3.Enabled = False
            Me.ddlPrtsNo4.Enabled = False
            Me.ddlPrtsNo5.Enabled = False
            Me.ddlPrtsNo6.Enabled = False
            Me.ddlPrtsNo1.SelectedIndex = "0"
            Me.ddlPrtsNo2.SelectedIndex = "0"
            Me.ddlPrtsNo3.SelectedIndex = "0"
            Me.ddlPrtsNo4.SelectedIndex = "0"
            Me.ddlPrtsNo5.SelectedIndex = "0"
            Me.ddlPrtsNo6.SelectedIndex = "0"

            Me.txtTboxId.ppEnabled = False
            Me.dftAppaSendDt.ppEnabled = False
            Me.dftTroubleDt.ppEnabled = False
            Me.dftArrivalDt.ppEnabled = False
            Me.txtOldSerial.ppEnabled = False
            Me.txtNewSerial.ppEnabled = False
            Me.txtContent.ppEnabled = False
            Me.txtRptDtl.ppEnabled = False
            Me.txtRepairContent.ppEnabled = False
            Me.txtRptDtl.ppEnabled = False
            Me.txtRptDtl.ppEnabled = False
            Me.txtRptDtl.ppEnabled = False
            Me.txtRptDtl.ppEnabled = False

            Me.txtTboxId.ppText = ""
            Me.dftAppaSendDt.ppFromText = ""
            Me.dftAppaSendDt.ppToText = ""
            Me.dftTroubleDt.ppFromText = ""
            Me.dftTroubleDt.ppToText = ""
            Me.dftArrivalDt.ppFromText = ""
            Me.dftArrivalDt.ppToText = ""
            Me.txtOldSerial.ppText = ""
            Me.txtNewSerial.ppText = ""
            Me.txtContent.ppText = ""
            Me.txtRptDtl.ppText = ""
            Me.txtRepairContent.ppText = ""
        Else
            If Me.ddlMakerKubun.SelectedIndex = "0" Then
                Me.ddlWorkNo1.Enabled = False
                Me.ddlWorkNo2.Enabled = False
                Me.ddlPrtsNo1.Enabled = False
                Me.ddlPrtsNo2.Enabled = False
                Me.ddlPrtsNo3.Enabled = False
                Me.ddlPrtsNo4.Enabled = False
                Me.ddlPrtsNo5.Enabled = False
                Me.ddlPrtsNo6.Enabled = False
                Me.ddlPrtsNo1.SelectedIndex = "0"
                Me.ddlPrtsNo2.SelectedIndex = "0"
                Me.ddlPrtsNo3.SelectedIndex = "0"
                Me.ddlPrtsNo4.SelectedIndex = "0"
                Me.ddlPrtsNo5.SelectedIndex = "0"
                Me.ddlPrtsNo6.SelectedIndex = "0"
            Else
                Me.ddlWorkNo1.Enabled = True
                Me.ddlWorkNo2.Enabled = True
                Me.ddlPrtsNo1.Enabled = True
                Me.ddlPrtsNo2.Enabled = True
                Me.ddlPrtsNo3.Enabled = True
                Me.ddlPrtsNo4.Enabled = True
                Me.ddlPrtsNo5.Enabled = True
                Me.ddlPrtsNo6.Enabled = True
            End If

            Me.txtTboxId.ppEnabled = True
            Me.dftAppaSendDt.ppEnabled = True
            Me.dftTroubleDt.ppEnabled = True
            Me.dftArrivalDt.ppEnabled = True
            Me.txtOldSerial.ppEnabled = True
            Me.txtNewSerial.ppEnabled = True
            Me.txtContent.ppEnabled = True
            Me.txtRptDtl.ppEnabled = True
            Me.txtRepairContent.ppEnabled = True
            Me.txtRptDtl.ppEnabled = True
            Me.txtRptDtl.ppEnabled = True
            Me.txtRptDtl.ppEnabled = True
            Me.txtRptDtl.ppEnabled = True
        End If
    End Sub

    ''' <summary>
    ''' TBOXID変更時処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtTboxId_TextChanged(sender As Object, e As EventArgs)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet        '表示情報のデータセット
        Dim maxVal As Integer = 0

        objStack = New StackFrame

        'TBOXIDが空白の時は処理をしない.
        If Me.txtTboxId.ppTextBox.Text = String.Empty Then
            'ホール名のラベルを初期化
            Me.lblHallName2.Text = String.Empty
            Exit Sub
        End If

        Try

            '開始ログ出力
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, sCnsErr_00005, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            End If

            'リストデータ取得
            cmdDB = New SqlCommand(sCnsSqlid_028, conDB)
            cmdDB.Parameters.Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxId.ppTextBox.Text))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            '該当データなしの場合.
            If dstOrders.Tables(0).Rows.Count = 0 Then
                'ホール名のラベルを初期化
                Me.lblHallName2.Text = String.Empty
                txtTboxId.psSet_ErrorNo("2002", "入力されたTBOXID")
                txtTboxId.ppTextBox.Focus()
                Exit Sub
            End If

            Me.lblHallName2.Text = dstOrders.Tables(0).Rows(0).Item("ホール名").ToString()
            SetFocus(dftAppaSendDt.FindControl("txtDateBoxFrom"))

        Catch ex As Exception

            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ情報取得")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, sCnsErr_00006, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub

    ''' <summary>
    ''' 検索クリアボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)

        '入力項目クリア処理.
        Call msClearScreen()

    End Sub

    ''' <summary>
    ''' 検索ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        '個別エラーチェック.
        Call msCheck_Error()

        If (Page.IsValid) Then
            Call msGet_Data(Me.ddlMakerKubun.SelectedValue,
                            Me.txtKanriNo.ppText,
                            Me.chkRepairState.Checked,
                            Me.chkMenteState.Checked,
                            Me.txtTboxId.ppText,
                            Me.dftAppaSendDt.ppFromText,
                            Me.dftAppaSendDt.ppToText,
                            Me.dftAppaArvDt.ppFromText,
                            Me.dftAppaArvDt.ppToText,
                            Me.dftTroubleDt.ppFromText,
                            Me.dftTroubleDt.ppToText,
                            Me.dftArrivalDt.ppFromText,
                            Me.dftArrivalDt.ppToText,
                            Me.dftCmpSndDt.ppFromText,
                            Me.dftCmpSndDt.ppToText,
                            Me.txtOldSerial.ppText,
                            Me.txtNewSerial.ppText,
                            Me.txtContent.ppText,
                            Me.txtRptDtl.ppText,
                            Me.txtRepairContent.ppText,
                            Me.ddlWorkNo1.SelectedValue,
                            Me.ddlWorkNo2.SelectedValue,
                            Me.ddlPrtsNo1.SelectedValue,
                            Me.ddlPrtsNo2.SelectedValue,
                            Me.ddlPrtsNo3.SelectedValue,
                            Me.ddlPrtsNo4.SelectedValue,
                            Me.ddlPrtsNo5.SelectedValue,
                            Me.ddlPrtsNo6.SelectedValue,
                            Me.ddlTmpResult.ppSelectedValue.Trim,
                            Me.ddlStatusCd.SelectedValue,
                            Me.tftReqDt.ppFromText,
                            Me.tftReqDt.ppToText,
                            DEFAULT_SEARCH)
        End If
    End Sub
    ''' <summary>
    ''' 整備登録ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnMenteAdd_Click(sender As Object, e As EventArgs)

        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text     'パンくずリスト
        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録                  '画面遷移条件
        Session(P_SESSION_OLDDISP) = M_DISP_ID                      '遷移元の画面ＩＤ

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
                        objStack.GetMethod.Name, M_MNTUPDP001, strPrm, "TRANS")

        psOpen_Window(Me, M_MNTUPDP001)

    End Sub
    ''' <summary>
    ''' 一覧の更新／参照ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        If e.CommandName.Trim <> "btnReference" And e.CommandName.Trim <> "btnUpdate" Then
            Exit Sub
        End If

        Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    ' ボタン押下行のIndex
        Dim rowData As GridViewRow = grvList.Rows(intIndex)             ' ボタン押下行
        Dim strKeyList As List(Of String) = Nothing                     '次画面引継ぎ用キー情報
        Dim strKubunNo As String = String.Empty

        '排他制御用の変数定義.
        Dim strExclusiveDate As String = String.Empty
        Dim arTable_Name As New ArrayList
        Dim arKey As New ArrayList
        Dim strBtnFlag As String = String.Empty

        '次画面引継ぎ用キー情報設定.
        strKeyList = New List(Of String)
        strKeyList.Add(CType(rowData.FindControl("管理番号"), TextBox).Text)
        strKeyList.Add(CType(rowData.FindControl("枝番"), TextBox).Text)

        'セッション情報設定
        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text 'パンくずリスト
        Session(P_KEY) = strKeyList.ToArray                             'キー項目(管理番号)
        Session(P_SESSION_OLDDISP) = M_DISP_ID                  '遷移元の画面ＩＤ
        Select Case e.CommandName
            Case "btnReference"     ' 参照
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
            Case "btnUpdate"        ' 更新
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
        End Select

        '画面遷移.
        strKubunNo = CType(rowData.FindControl("区分"), TextBox).Text
        Select Case strKubunNo
            Case "修理" '修理依頼書

                '遷移条件取得.
                strBtnFlag = Session(P_SESSION_TERMS)

                'セッション情報設定.
                If strBtnFlag = ClsComVer.E_遷移条件.更新 Then

                    'ロック対象テーブル名の登録.
                    'arTable_Name.Insert(0, "D80_REPAIR_DTIL")
                    arTable_Name.Insert(0, "D29_REPAIR")

                    'ロックテーブルキー項目の登録.
                    arKey.Insert(0, CType(rowData.FindControl("管理番号"), TextBox).Text)
                    arKey.Insert(1, CType(rowData.FindControl("枝番"), TextBox).Text)

                    '排他情報確認処理(更新処理の実行).
                    If clsExc.pfSel_Exclusive(strExclusiveDate _
                                     , Me _
                                     , Session(P_SESSION_IP) _
                                     , Session(P_SESSION_PLACE) _
                                     , Session(P_SESSION_USERID) _
                                     , Session(P_SESSION_SESSTION_ID) _
                                     , ViewState(P_SESSION_GROUP_NUM) _
                                     , M_REPUPDP002_ID _
                                     , arTable_Name _
                                     , arKey) = 0 Then

                        '登録年月日時刻.
                        Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate
                    Else

                        '排他ロック中.
                        Exit Sub

                    End If

                End If

                '排他情報のグループ番号をセッション変数に設定.
                Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

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
                                objStack.GetMethod.Name, M_REPUPDP002, strPrm, "TRANS")
                '■□■□結合試験時のみ使用予定□■□■
                '--------------------------------
                '2014/04/16 星野　ここまで
                '--------------------------------
                '別ブラウザ起動
                psOpen_Window(Me, M_REPUPDP002)

            Case "整備" '整備依頼書

                '遷移条件取得.
                strBtnFlag = Session(P_SESSION_TERMS)

                'セッション情報設定.
                If strBtnFlag = ClsComVer.E_遷移条件.更新 Then

                    'ロック対象テーブル名の登録.
                    arTable_Name.Insert(0, "D23_MENTE_REQUEST")
                    'arTable_Name.Insert(1, "D232_MENTE_REQUEST_DTL")
                    'arTable_Name.Insert(2, "D82_MENTEPARTS_DTIL")

                    'ロックテーブルキー項目の登録.
                    arKey.Insert(0, CType(rowData.FindControl("管理番号"), TextBox).Text)
                    arKey.Insert(1, CType(rowData.FindControl("枝番"), TextBox).Text)

                    '排他情報確認処理(更新処理の実行).
                    If clsExc.pfSel_Exclusive(strExclusiveDate _
                                     , Me _
                                     , Session(P_SESSION_IP) _
                                     , Session(P_SESSION_PLACE) _
                                     , Session(P_SESSION_USERID) _
                                     , Session(P_SESSION_SESSTION_ID) _
                                     , ViewState(P_SESSION_GROUP_NUM) _
                                     , M_MNTUPDP001_ID _
                                     , arTable_Name _
                                     , arKey) = 0 Then


                        '登録年月日時刻.
                        Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate
                    Else

                        '排他ロック中.
                        Exit Sub

                    End If

                End If

                '排他情報のグループ番号をセッション変数に設定.
                Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

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
                                objStack.GetMethod.Name, M_MNTUPDP001, strPrm, "TRANS")
                '■□■□結合試験時のみ使用予定□■□■
                '--------------------------------
                '2014/04/16 星野　ここまで
                '--------------------------------
                '別ブラウザ起動
                psOpen_Window(Me, M_MNTUPDP001)

        End Select


    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' ドロップダウンリスト生成処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfGet_DropDownList() As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet        '表示情報のデータセット.
        Dim strMessage As String = String.Empty 'エラーメッセージ.

        objStack = New StackFrame

        Try

            '開始ログ出力.
            psLogStart(Me)

            '接続.
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, sCnsErr_00005, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                mfGet_DropDownList = False
            End If

            'メーカー区分ドロップダウンリスト生成.
            strMessage = "業者マスタ一覧取得"
            cmdDB = New SqlCommand(sCnsSqlid_040, conDB)
            cmdDB.Parameters.Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, ""))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlMakerKubun.DataSource = dstOrders.Tables(0)
            Me.ddlMakerKubun.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlMakerKubun.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlMakerKubun.DataBind()
            Me.ddlMakerKubun.Items.Insert(0, New ListItem(Nothing, Nothing))

            'ステータスドロップダウンリスト生成.
            strMessage = "進捗状況ステータスマスタ一覧取得"
            cmdDB = New SqlCommand(M_DISP_ID & "_S2", conDB)
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlStatusCd.DataSource = dstOrders.Tables(0)
            Me.ddlStatusCd.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlStatusCd.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlStatusCd.DataBind()
            Me.ddlStatusCd.Items.Insert(0, New ListItem(Nothing, Nothing))

            '正常終了.
            mfGet_DropDownList = True

        Catch ex As Exception

            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMessage)

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            mfGet_DropDownList = False
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, sCnsErr_00006, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                mfGet_DropDownList = False
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function
    ''' <summary>
    ''' ドロップダウンリスト生成処理(メーカー関連).
    ''' </summary>
    ''' <param name="ipstrMkr_cd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGet_DropDownListMaker(ByVal ipstrMkr_cd As String) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet        '表示情報のデータセット.
        Dim strMessage As String = String.Empty 'エラーメッセージ.

        objStack = New StackFrame

        Try

            '開始ログ出力.
            psLogStart(Me)

            '接続.
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, sCnsErr_00005, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                mfGet_DropDownListMaker = False
            End If

            '作業項番１ドロップダウンリスト生成.
            strMessage = "部品マスタ一覧取得"
            cmdDB = New SqlCommand(sCnsSqlid_027, conDB)
            cmdDB.Parameters.Add(pfSet_Param("maker_cd", SqlDbType.NVarChar, ipstrMkr_cd))
            cmdDB.Parameters.Add(pfSet_Param("wrk_cls", SqlDbType.NVarChar, "1"))
            cmdDB.Parameters.Add(pfSet_Param("part_cd", SqlDbType.NVarChar, ""))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlWorkNo1.DataSource = dstOrders.Tables(0)
            Me.ddlWorkNo1.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlWorkNo1.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlWorkNo1.DataBind()
            '--------------------------------
            '2014/05/15 後藤　ここから
            '--------------------------------
            'Me.ddlWorkNo1.Items.Insert(0, " ")
            Me.ddlWorkNo1.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/15 後藤　ここまで
            '--------------------------------

            '作業項番２ドロップダウンリスト生成.
            '--------------------------------
            '2014/05/13 後藤　ここから
            '--------------------------------
            'strMessage = "部品マスタ一覧取得"
            '--------------------------------
            '2014/05/13 後藤　ここまで
            '--------------------------------
            cmdDB = New SqlCommand(sCnsSqlid_027, conDB)
            cmdDB.Parameters.Add(pfSet_Param("maker_cd", SqlDbType.NVarChar, ipstrMkr_cd))
            cmdDB.Parameters.Add(pfSet_Param("wrk_cls", SqlDbType.NVarChar, "2"))
            cmdDB.Parameters.Add(pfSet_Param("part_cd", SqlDbType.NVarChar, ""))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlWorkNo2.DataSource = dstOrders.Tables(0)
            Me.ddlWorkNo2.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlWorkNo2.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlWorkNo2.DataBind()
            '--------------------------------
            '2014/05/15 後藤　ここから
            '--------------------------------
            'Me.ddlWorkNo2.Items.Insert(0, " ")
            Me.ddlWorkNo2.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/15 後藤　ここまで
            '--------------------------------

            '部品項番１ドロップダウンリスト生成.
            '--------------------------------
            '2014/05/13 後藤　ここから
            '--------------------------------
            'strMessage = "部品マスタ一覧取得"
            '--------------------------------
            '2014/05/13 後藤　ここまで
            '--------------------------------
            cmdDB = New SqlCommand(sCnsSqlid_027, conDB)
            cmdDB.Parameters.Add(pfSet_Param("maker_cd", SqlDbType.NVarChar, ipstrMkr_cd))
            cmdDB.Parameters.Add(pfSet_Param("wrk_cls", SqlDbType.NVarChar, "3"))
            cmdDB.Parameters.Add(pfSet_Param("part_cd", SqlDbType.NVarChar, ""))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlPrtsNo1.DataSource = dstOrders.Tables(0)
            Me.ddlPrtsNo1.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlPrtsNo1.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlPrtsNo1.DataBind()
            '--------------------------------
            '2014/05/15 後藤　ここから
            '--------------------------------
            'Me.ddlPrtsNo1.Items.Insert(0, " ")
            Me.ddlPrtsNo1.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/15 後藤　ここまで
            '--------------------------------

            '部品項番２ドロップダウンリスト生成.
            '--------------------------------
            '2014/05/13 後藤　ここから
            '--------------------------------
            'strMessage = "部品マスタ一覧取得"
            '--------------------------------
            '2014/05/13 後藤　ここまで
            '--------------------------------
            cmdDB = New SqlCommand(sCnsSqlid_027, conDB)
            cmdDB.Parameters.Add(pfSet_Param("maker_cd", SqlDbType.NVarChar, ipstrMkr_cd))
            cmdDB.Parameters.Add(pfSet_Param("wrk_cls", SqlDbType.NVarChar, "4"))
            cmdDB.Parameters.Add(pfSet_Param("part_cd", SqlDbType.NVarChar, ""))
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.ddlPrtsNo2.DataSource = dstOrders.Tables(0)
            Me.ddlPrtsNo3.DataSource = dstOrders.Tables(0)
            Me.ddlPrtsNo4.DataSource = dstOrders.Tables(0)
            Me.ddlPrtsNo5.DataSource = dstOrders.Tables(0)
            Me.ddlPrtsNo6.DataSource = dstOrders.Tables(0)
            Me.ddlPrtsNo2.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlPrtsNo2.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlPrtsNo3.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlPrtsNo3.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlPrtsNo4.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlPrtsNo4.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlPrtsNo5.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlPrtsNo5.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlPrtsNo6.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlPrtsNo6.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlPrtsNo2.DataBind()
            Me.ddlPrtsNo3.DataBind()
            Me.ddlPrtsNo4.DataBind()
            Me.ddlPrtsNo5.DataBind()
            Me.ddlPrtsNo6.DataBind()
            '--------------------------------
            '2014/05/15 後藤　ここから
            '--------------------------------
            'Me.ddlPrtsNo2.Items.Insert(0, " ")
            'Me.ddlPrtsNo3.Items.Insert(0, " ")
            'Me.ddlPrtsNo4.Items.Insert(0, " ")
            'Me.ddlPrtsNo5.Items.Insert(0, " ")
            'Me.ddlPrtsNo6.Items.Insert(0, " ")
            Me.ddlPrtsNo2.Items.Insert(0, New ListItem(Nothing, Nothing))
            Me.ddlPrtsNo3.Items.Insert(0, New ListItem(Nothing, Nothing))
            Me.ddlPrtsNo4.Items.Insert(0, New ListItem(Nothing, Nothing))
            Me.ddlPrtsNo5.Items.Insert(0, New ListItem(Nothing, Nothing))
            Me.ddlPrtsNo6.Items.Insert(0, New ListItem(Nothing, Nothing))
            '--------------------------------
            '2014/05/15 後藤　ここまで
            '--------------------------------

            '正常終了.
            mfGet_DropDownListMaker = True

        Catch ex As Exception

            '--------------------------------
            '2014/05/13 後藤　ここから
            '--------------------------------
            'psMesBox(Me, sCnsErr_00004, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, strMessage)
            psMesBox(Me, "30015", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, strMessage)
            '--------------------------------
            '2014/05/13 後藤　ここまで
            '--------------------------------

            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            mfGet_DropDownListMaker = False
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, sCnsErr_00006, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                mfGet_DropDownListMaker = False
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function
    ''' <summary>
    ''' 画面初期化処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msInitScreen()

        Me.txtKanriNo.ppText = String.Empty
        Me.chkRepairState.Checked = False
        Me.chkMenteState.Checked = False
        Me.txtTboxId.ppText = String.Empty
        Me.lblHallName2.Text = String.Empty
        Me.dftAppaSendDt.ppFromText = String.Empty
        Me.dftAppaSendDt.ppToText = String.Empty
        Me.dftAppaArvDt.ppFromText = String.Empty
        Me.dftAppaArvDt.ppToText = String.Empty
        Me.dftTroubleDt.ppFromText = String.Empty
        Me.dftTroubleDt.ppToText = String.Empty
        Me.dftArrivalDt.ppFromText = String.Empty
        Me.dftArrivalDt.ppToText = String.Empty
        Me.dftCmpSndDt.ppFromText = String.Empty
        Me.dftCmpSndDt.ppToText = String.Empty
        Me.txtOldSerial.ppText = String.Empty
        Me.txtNewSerial.ppText = String.Empty
        Me.txtContent.ppText = String.Empty
        Me.txtRptDtl.ppText = String.Empty
        Me.txtRepairContent.ppText = String.Empty
        Me.tftReqDt.ppFromText = String.Empty
        Me.tftReqDt.ppToText = String.Empty
        Me.grvList.DataSource = New Object() {}
        Master.ppCount = "0"

        '初期表示.
        Call msGet_Data(Me.ddlMakerKubun.SelectedValue,
                    Me.txtKanriNo.ppText,
                    Me.chkRepairState.Checked,
                    Me.chkMenteState.Checked,
                    Me.txtTboxId.ppText,
                    Me.dftAppaSendDt.ppFromText,
                    Me.dftAppaSendDt.ppToText,
                    Me.dftAppaArvDt.ppFromText,
                    Me.dftAppaArvDt.ppToText,
                    Me.dftTroubleDt.ppFromText,
                    Me.dftTroubleDt.ppToText,
                    Me.dftArrivalDt.ppFromText,
                    Me.dftArrivalDt.ppToText,
                    Me.dftCmpSndDt.ppFromText,
                    Me.dftCmpSndDt.ppToText,
                    Me.txtOldSerial.ppText,
                    Me.txtNewSerial.ppText,
                    Me.txtContent.ppText,
                    Me.txtRptDtl.ppText,
                    Me.txtRepairContent.ppText,
                    Me.ddlWorkNo1.SelectedValue,
                    Me.ddlWorkNo2.SelectedValue,
                    Me.ddlPrtsNo1.SelectedValue,
                    Me.ddlPrtsNo2.SelectedValue,
                    Me.ddlPrtsNo3.SelectedValue,
                    Me.ddlPrtsNo4.SelectedValue,
                    Me.ddlPrtsNo5.SelectedValue,
                    Me.ddlPrtsNo6.SelectedValue,
                    Me.ddlTmpResult.ppSelectedValue,
                    Me.ddlStatusCd.SelectedValue,
                    Me.tftReqDt.ppFromText,
                    Me.tftReqDt.ppToText,
                    DEFAULT_DISP)

        '変更を反映.
        Me.grvList.DataBind()

        'ドロップダウンリスト生成.
        If Not mfGet_DropDownList() Then
            '画面を終了
            psClose_Window(Me)
        End If

        'ドロップダウンリスト生成(メーカー関連).
        If Not mfGet_DropDownListMaker(Me.ddlMakerKubun.SelectedValue) Then
            '画面を終了
            psClose_Window(Me)
        End If

    End Sub
    ''' <summary>
    ''' 入力項目クリア処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()

        Me.ddlMakerKubun.SelectedIndex = 0
        Me.txtKanriNo.ppText = String.Empty
        Me.chkRepairState.Checked = False
        Me.chkMenteState.Checked = False
        Me.txtTboxId.ppText = String.Empty
        Me.lblHallName2.Text = String.Empty
        Me.dftAppaSendDt.ppFromText = String.Empty
        Me.dftAppaSendDt.ppToText = String.Empty
        Me.dftAppaArvDt.ppFromText = String.Empty
        Me.dftAppaArvDt.ppToText = String.Empty
        Me.dftTroubleDt.ppFromText = String.Empty
        Me.dftTroubleDt.ppToText = String.Empty
        Me.dftArrivalDt.ppFromText = String.Empty
        Me.dftArrivalDt.ppToText = String.Empty
        Me.dftCmpSndDt.ppFromText = String.Empty
        Me.dftCmpSndDt.ppToText = String.Empty
        Me.txtOldSerial.ppText = String.Empty
        Me.txtNewSerial.ppText = String.Empty
        Me.txtContent.ppText = String.Empty
        Me.txtRptDtl.ppText = String.Empty
        Me.txtRepairContent.ppText = String.Empty
        Me.ddlWorkNo1.SelectedIndex = 0
        Me.ddlWorkNo2.SelectedIndex = 0
        Me.ddlPrtsNo1.SelectedIndex = 0
        Me.ddlPrtsNo2.SelectedIndex = 0
        Me.ddlPrtsNo3.SelectedIndex = 0
        Me.ddlPrtsNo4.SelectedIndex = 0
        Me.ddlPrtsNo5.SelectedIndex = 0
        Me.ddlPrtsNo6.SelectedIndex = 0
        Me.ddlTmpResult.ppDropDownList.SelectedIndex = 0
        Me.ddlStatusCd.SelectedIndex = 0
        Me.tftReqDt.ppFromText = String.Empty
        Me.tftReqDt.ppToText = String.Empty

        Me.ddlWorkNo1.Enabled = False
        Me.ddlWorkNo2.Enabled = False
        Me.ddlPrtsNo1.Enabled = False
        Me.ddlPrtsNo2.Enabled = False
        Me.ddlPrtsNo3.Enabled = False
        Me.ddlPrtsNo4.Enabled = False
        Me.ddlPrtsNo5.Enabled = False
        Me.ddlPrtsNo6.Enabled = False

    End Sub
    ''' <summary>
    ''' 検索処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data(ByVal ipstrMkr_cd As String,
                           ByVal ipstrKanrino As String,
                           ByVal ipstrRepairstate As Boolean,
                           ByVal ipstrMantestate As Boolean,
                           ByVal ipstrTboxid As String,
                           ByVal ipstrAppasend_dt_f As String,
                           ByVal ipstrAppasend_dt_t As String,
                           ByVal ipstrAppaarv_dt_f As String,
                           ByVal ipstrAppaarv_dt_t As String,
                           ByVal ipstrTrouble_dt_f As String,
                           ByVal ipstrTrouble_dt_t As String,
                           ByVal ipstrArrival_dt_f As String,
                           ByVal ipstrArrival_dt_t As String,
                           ByVal ipstrCmpsnd_dt_f As String,
                           ByVal ipstrCmpsnd_dt_t As String,
                           ByVal ipstrOld_serial As String,
                           ByVal ipstrNew_serial As String,
                           ByVal ipstrContent As String,
                           ByVal ipstrRpt_Dtl As String,
                           ByVal ipstrRepair_content As String,
                           ByVal ipstrWrk_no1 As String,
                           ByVal ipstrWrk_no2 As String,
                           ByVal ipstrParts_no1 As String,
                           ByVal ipstrParts_no21 As String,
                           ByVal ipstrParts_no22 As String,
                           ByVal ipstrParts_no23 As String,
                           ByVal ipstrParts_no24 As String,
                           ByVal ipstrParts_no25 As String,
                           ByVal ipstrTmp_result As String,
                           ByVal ipstrStatus_cd As String,
                           ByVal ipstrReq_dt_f As String,
                           ByVal ipstrReq_dt_t As String,
                           ByVal ipstrdefault As String)

        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim intRepairState As Integer
        Dim intManteState As Integer
        Dim strDispFlg As String = String.Empty
        Dim dstOrders As New DataSet
        Dim strSearchFlg As String = String.Empty
        Dim strYear_f As String = String.Empty
        Dim strMonth_f As String = String.Empty
        Dim strYear_t As String = String.Empty
        Dim strMonth_t As String = String.Empty

        objStack = New StackFrame

        '初期化
        conDB = Nothing

        'チェックの判定.
        Select Case ipstrRepairstate
            Case True
                intRepairState = 1
                strDispFlg = "1"
            Case Else
                intRepairState = 0
        End Select

        Select Case ipstrMantestate
            Case True
                intManteState = 2
                strDispFlg = "2"
            Case Else
                intManteState = 0
        End Select

        Select Case intRepairState + intManteState
            Case 0
                strDispFlg = "3"
            Case 3
                strDispFlg = "3"
        End Select

        '部品項番2入力チェック.
        If ipstrParts_no21.Trim = String.Empty And
           ipstrParts_no22.Trim = String.Empty And
           ipstrParts_no23.Trim = String.Empty And
           ipstrParts_no24.Trim = String.Empty And
           ipstrParts_no25.Trim = String.Empty Then
            strSearchFlg = "0"
        Else

            strSearchFlg = "1"
        End If

        '請求年月編集.
        If ipstrReq_dt_f.Trim <> String.Empty Then
            strYear_f = ipstrReq_dt_f.Substring(0, 4)
            strMonth_f = ipstrReq_dt_f.Substring(4, 2)
            ipstrReq_dt_f = strYear_f & "/" & strMonth_f
        End If

        If ipstrReq_dt_t.Trim <> String.Empty Then
            strYear_t = ipstrReq_dt_t.Substring(0, 4)
            strMonth_t = ipstrReq_dt_t.Substring(4, 2)
            ipstrReq_dt_t = strYear_t & "/" & strMonth_t
        End If

        '接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, sCnsErr_00005, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"
        Else
            Try
                cmdDB = New SqlCommand(M_DISP_ID & "_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("mkr_cd", SqlDbType.NVarChar, ipstrMkr_cd))
                    .Add(pfSet_Param("repair_no", SqlDbType.NVarChar, ipstrKanrino))
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, ipstrTboxid))
                    .Add(pfSet_Param("appasend_dt_f", SqlDbType.NVarChar, ipstrAppasend_dt_f))
                    .Add(pfSet_Param("appasend_dt_t", SqlDbType.NVarChar, ipstrAppasend_dt_t))
                    .Add(pfSet_Param("appaarv_dt_f", SqlDbType.NVarChar, ipstrAppaarv_dt_f))
                    .Add(pfSet_Param("appaarv_dt_t", SqlDbType.NVarChar, ipstrAppaarv_dt_t))
                    .Add(pfSet_Param("trouble_dt_f", SqlDbType.NVarChar, ipstrTrouble_dt_f))
                    .Add(pfSet_Param("trouble_dt_t", SqlDbType.NVarChar, ipstrTrouble_dt_t))
                    .Add(pfSet_Param("arrival_dt_f", SqlDbType.NVarChar, ipstrArrival_dt_f))
                    .Add(pfSet_Param("arrival_dt_t", SqlDbType.NVarChar, ipstrArrival_dt_t))
                    .Add(pfSet_Param("cmpsnd_dt_f", SqlDbType.NVarChar, ipstrCmpsnd_dt_f))
                    .Add(pfSet_Param("cmpsnd_dt_t", SqlDbType.NVarChar, ipstrCmpsnd_dt_t))
                    .Add(pfSet_Param("serial", SqlDbType.NVarChar, ipstrOld_serial))
                    .Add(pfSet_Param("new_serial", SqlDbType.NVarChar, ipstrNew_serial))
                    .Add(pfSet_Param("content", SqlDbType.NVarChar, ipstrContent))
                    .Add(pfSet_Param("rpt_dtl", SqlDbType.NVarChar, ipstrRpt_Dtl))
                    .Add(pfSet_Param("repair_content", SqlDbType.NVarChar, ipstrRepair_content))
                    .Add(pfSet_Param("wrk_no1", SqlDbType.NVarChar, ipstrWrk_no1))
                    .Add(pfSet_Param("wrk_no2", SqlDbType.NVarChar, ipstrWrk_no2))
                    .Add(pfSet_Param("parts_no11", SqlDbType.NVarChar, ipstrParts_no1))
                    .Add(pfSet_Param("parts_no21", SqlDbType.NVarChar, ipstrParts_no21))
                    .Add(pfSet_Param("parts_no22", SqlDbType.NVarChar, ipstrParts_no22))
                    .Add(pfSet_Param("parts_no23", SqlDbType.NVarChar, ipstrParts_no23))
                    .Add(pfSet_Param("parts_no24", SqlDbType.NVarChar, ipstrParts_no24))
                    .Add(pfSet_Param("parts_no25", SqlDbType.NVarChar, ipstrParts_no25))
                    .Add(pfSet_Param("tmp_result", SqlDbType.NVarChar, ipstrTmp_result))
                    .Add(pfSet_Param("status_cd", SqlDbType.NVarChar, ipstrStatus_cd))
                    .Add(pfSet_Param("req_dt_f", SqlDbType.NVarChar, ipstrReq_dt_f))
                    .Add(pfSet_Param("req_dt_t", SqlDbType.NVarChar, ipstrReq_dt_t))
                    .Add(pfSet_Param("disp_id", SqlDbType.NVarChar, M_DISP_ID))
                    .Add(pfSet_Param("default", SqlDbType.NVarChar, ipstrdefault))
                    .Add(pfSet_Param("dispflg", SqlDbType.NVarChar, strDispFlg))
                    .Add(pfSet_Param("searchflg", SqlDbType.NVarChar, strSearchFlg))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                Select Case strDispFlg

                    Case "1"

                        '該当データなしの場合.
                        If dstOrders.Tables(0).Rows.Count = 0 Then
                            '--------------------------------
                            '2014/05/12 後藤　ここから
                            '--------------------------------
                            'psMesBox(Me, sCnsInfo_00007, clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後)
                            psMesBox(Me, "00004", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後)
                            '--------------------------------
                            '2014/05/12 後藤　ここまで
                            '--------------------------------
                            Master.ppCount = "0"
                        Else
                            '閾値を超えた場合はメッセージを表示
                            If dstOrders.Tables(0).Rows(0)("MAXROW") > dstOrders.Tables(0).Rows.Count Then
                                psMesBox(Me, sCnsErr_30005, clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後,
                                    dstOrders.Tables(0).Rows(0)("MAXROW") & "(修理)", dstOrders.Tables(0).Rows.Count.ToString)
                            End If

                            '件数を設定
                            Master.ppCount = dstOrders.Tables(0).Rows(0)("MAXROW")
                        End If


                    Case "2"

                        '該当データなしの場合.
                        If dstOrders.Tables(0).Rows.Count = 0 Then
                            '--------------------------------
                            '2014/05/12 後藤　ここから
                            '--------------------------------
                            'psMesBox(Me, sCnsInfo_00007, clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後)
                            psMesBox(Me, "00004", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後)
                            '--------------------------------
                            '2014/05/12 後藤　ここまで
                            '--------------------------------
                            Master.ppCount = "0"
                        Else
                            '閾値を超えた場合はメッセージを表示
                            If dstOrders.Tables(0).Rows(0)("MAXROW") > dstOrders.Tables(0).Rows.Count Then
                                psMesBox(Me, sCnsErr_30005, clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後,
                                    dstOrders.Tables(0).Rows(0)("MAXROW") & "(整備)", dstOrders.Tables(0).Rows.Count.ToString)
                            End If

                            '件数を設定
                            Master.ppCount = dstOrders.Tables(0).Rows(0)("MAXROW")
                        End If

                    Case "3"

                        '該当データなしの場合.
                        If dstOrders.Tables(0).Rows.Count = 0 Then
                            '--------------------------------
                            '2014/05/12 後藤　ここから
                            '--------------------------------
                            'psMesBox(Me, sCnsInfo_00007, clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後)
                            psMesBox(Me, "00004", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後)
                            '--------------------------------
                            '2014/05/12 後藤　ここまで
                            '--------------------------------
                            Master.ppCount = "0"
                        Else

                            '閾値を超えた場合はメッセージを表示(修理のみ).
                            If dstOrders.Tables(0).Rows(0)("M_ROW_COUNT").ToString = String.Empty Then
                                If dstOrders.Tables(0).Rows(0)("MAX_COUNT") < dstOrders.Tables(0).Rows(0)("R_ROW_COUNT") Then
                                    psMesBox(Me, sCnsErr_30005, clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後,
                                        dstOrders.Tables(0).Rows(0)("R_ROW_COUNT") & "(修理)", dstOrders.Tables(0).Rows(0)("MAX_COUNT"))
                                End If
                            End If

                            '閾値を超えた場合はメッセージを表示(整備のみ).
                            If dstOrders.Tables(0).Rows(0)("R_ROW_COUNT").ToString = String.Empty Then
                                If dstOrders.Tables(0).Rows(0)("MAX_COUNT") < dstOrders.Tables(0).Rows(0)("M_ROW_COUNT") Then
                                    psMesBox(Me, sCnsErr_30005, clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後,
                                        dstOrders.Tables(0).Rows(0)("M_ROW_COUNT") & "(整備)", dstOrders.Tables(0).Rows(0)("MAX_COUNT"))
                                End If
                            End If

                            '閾値を超えた場合はメッセージを表示(修理・整備).
                            If dstOrders.Tables(0).Rows(0)("R_ROW_COUNT").ToString <> String.Empty And _
                                dstOrders.Tables(0).Rows(0)("M_ROW_COUNT").ToString <> String.Empty Then




                                If dstOrders.Tables(0).Rows(0)("MAX_COUNT") < dstOrders.Tables(0).Rows(0)("M_ROW_COUNT") And _
                                   dstOrders.Tables(0).Rows(0)("MAX_COUNT") < dstOrders.Tables(0).Rows(0)("R_ROW_COUNT") Then
                                    psMesBox(Me, sCnsErr_30005, clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後,
                                        dstOrders.Tables(0).Rows(0)("R_ROW_COUNT") &
                                        "(修理) " & "、" & "データ件数" & "：" & dstOrders.Tables(0).Rows(0)("M_ROW_COUNT") &
                                        "(整備)", dstOrders.Tables(0).Rows(0)("MAX_COUNT"))

                                    '--------------------------------
                                    '2014/05/16 後藤　ここから
                                    '--------------------------------
                                    'End If

                                ElseIf dstOrders.Tables(0).Rows(0)("MAX_COUNT") < dstOrders.Tables(0).Rows(0)("M_ROW_COUNT") Then
                                    psMesBox(Me, sCnsErr_30005, clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後,
                                        dstOrders.Tables(0).Rows(0)("M_ROW_COUNT") & "(整備)", dstOrders.Tables(0).Rows(0)("MAX_COUNT"))


                                ElseIf dstOrders.Tables(0).Rows(0)("MAX_COUNT") < dstOrders.Tables(0).Rows(0)("R_ROW_COUNT") Then
                                    psMesBox(Me, sCnsErr_30005, clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後,
                                        dstOrders.Tables(0).Rows(0)("R_ROW_COUNT") & "(修理)", dstOrders.Tables(0).Rows(0)("MAX_COUNT"))

                                End If
                                '--------------------------------
                                '2014/05/16 後藤　ここまで
                                '--------------------------------
                            End If

                            '件数を設定
                            Master.ppCount = dstOrders.Tables(0).Rows(0)("MAXROW")

                        End If

                End Select

                '取得したデータをリストに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                psMesBox(Me, sCnsErr_00004, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "進捗一覧")
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
                    psMesBox(Me, sCnsErr_00006, clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                End If
            End Try
        End If
    End Sub
    ''' <summary>
    ''' 個別エラーチェック.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheck_Error()

        Dim strErr As String

        '請求年月From桁数チェック.
        strErr = pfCheck_TxtErr(Me.tftReqDt.ppTextBoxFrom.Text, False, True, False, True, 6, String.Empty, False)
        If strErr <> String.Empty Then
            Me.tftReqDt.psSet_ErrorNo(strErr, "請求年月", "6")
        End If

        '請求年月To桁数チェック.
        strErr = pfCheck_TxtErr(Me.tftReqDt.ppTextBoxTo.Text, False, True, False, True, 6, String.Empty, False)
        If strErr <> String.Empty Then
            Me.tftReqDt.psSet_ErrorNo(strErr, "請求年月", "6")
        End If

        '請求年月From整合性チェック.
        strErr = pfCheck_TxtErr(Me.tftReqDt.ppTextBoxFrom.Text, False, True, True, True, 6, "[0-9][0-9][0-9][0-9]([0][1-9]|[1][0-2])", False)
        If strErr <> "" Then
            Me.tftReqDt.psSet_ErrorNo(strErr, "請求年月", "年月")
        End If

        '請求年月To整合性チェック.
        strErr = pfCheck_TxtErr(Me.tftReqDt.ppTextBoxTo.Text, False, True, True, True, 6, "[0-9][0-9][0-9][0-9]([0][1-9]|[1][0-2])", False)
        If strErr <> "" Then
            Me.tftReqDt.psSet_ErrorNo(strErr, "請求年月", "年月")
        End If

    End Sub

#End Region
End Class
