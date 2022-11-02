'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　保守担当者マスタ
'*　ＰＧＭＩＤ：　COMUPDM06
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.12.01　：　伯野
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive
Imports SQL_DBCLS_LIB


Public Class COMUPDM06

    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
#Region "継承定義"

    Inherits System.Web.UI.Page

#End Region

    '============================================================================================================================
    '=　定数定義
    '============================================================================================================================
#Region "定数定義"

    Const DispCode As String = "COMUPDM06"                  '画面ID
    Const MasterName As String = "機器種別マスタ"           '画面名
    Const DBSName As String = "SPCDB.dbo."                  'DB名.スキーマ名
    Const TableName As String = "M06_APPACLASS"        'テーブル名

#End Region

    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
#Region "変数定義"

    Dim mclsDB As New ClsSQLSvrDB
    Dim ClsSQL As New ClsSQL
    Dim clsExc As New ClsCMExclusive
    Dim clsMst As New ClsMSTCommon
    Dim mclsCMC As New ClsCMCommon
    Dim objStack As StackFrame

    Dim mstrDispMode As String = "DEFAULT"
    Dim strDeleteFlg As String = String.Empty
    Dim strFocusFlg As String = String.Empty

#End Region

    '============================================================================================================================
    '=　イベントプロシージャ
    '============================================================================================================================
#Region "イベントプロシージャ"

    '----------------------------------------------------------------------------------------------------------------------------
    '=　ページ制御
    '----------------------------------------------------------------------------------------------------------------------------
#Region "ページ制御"

    ''' <summary>
    ''' ページ初期化
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, DispCode)

    End Sub

    ''' <summary>
    ''' ページロード
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        'ＤＢ接続
        mclsDB.mstrConString = ConfigurationManager.ConnectionStrings("SPCDB").ToString
        If mclsDB.pfDB_Connect() = False Then
            'ＤＢ接続失敗
        End If

        ViewState("ddlSrchAPPACLASS_CD") = ddlSrchAPPACLASS_CD.ppSelectedValue
        ViewState("txtSrchAPPACLS_NM") = txtSrchAPPACLS_NM.ppText
        ViewState("ddlAPPACLASS_CD") = ddlAPPACLASS_CD.ppSelectedValue
        ViewState("txtAPPA_CLS") = txtAPPA_CLS.ppText
        ViewState("txtAPPACLS_NM") = txtAPPACLS_NM.ppText
        ViewState("txtAPPACLS_SNM") = txtAPPACLS_SNM.ppText
        ViewState("htxtDELETE") = htxtDELETE.Value
        ViewState("ddlSEARIAL_CLS") = ddlSEARIAL_CLS.ppSelectedValue
        ViewState("ppchksDel") = Master.ppchksDel.Checked

        mstrDispMode = ViewState("mstrDispMode")

        'ボタンアクション設定
        AddHandler Master.ppBtnSearch.Click, AddressOf btnSearch_Click          '検索
        AddHandler Master.ppBtnSrcClear.Click, AddressOf btnSearchClear_Click   '検索条件クリア
        AddHandler Master.ppBtnInsert.Command, AddressOf btn_Click              '登録
        AddHandler Master.ppBtnUpdate.Command, AddressOf btn_Click              '更新
        AddHandler Master.ppBtnDelete.Command, AddressOf btn_Click              '削除
        AddHandler Master.ppBtnClear.Click, AddressOf btnClear_Click            'クリア

        ddlAPPACLASS_CD.ppDropDownList.AutoPostBack = True
        txtAPPA_CLS.ppTextBox.AutoPostBack = True
        txtAPPACLS_NM.ppTextBox.AutoPostBack = True
        txtAPPACLS_SNM.ppTextBox.AutoPostBack = True
        ddlSEARIAL_CLS.ppDropDownList.AutoPostBack = True

        AddHandler ddlAPPACLASS_CD.ppDropDownList.TextChanged, AddressOf ddlAPPACLASS_CD_TextChanged
        AddHandler ddlSEARIAL_CLS.ppDropDownList.TextChanged, AddressOf ddlSEARIAL_CLS_TextChanged
        AddHandler txtAPPA_CLS.ppTextBox.TextChanged, AddressOf txtAPPA_CLS_TextChanged
        AddHandler txtAPPACLS_NM.ppTextBox.TextChanged, AddressOf txtAPPACLS_NM_TextChanged
        AddHandler txtAPPACLS_SNM.ppTextBox.TextChanged, AddressOf txtAPPACLS_SNM_TextChanged


        Master.ppBtnDmy.Visible = False

        If Not IsPostBack Then
            'プログラムＩＤ、画面名設定
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"

            '削除データを含むを活性化
            Master.ppchksDel.Visible = True

            'パンくずリスト設定
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            'ボタン押下時のメッセージ設定
            Master.ppBtnInsert.OnClientClick = pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '追加
            Master.ppBtnUpdate.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '更新

            'ValidationGroup設定
            Master.ppBtnInsert.ValidationGroup = "Edit"
            Master.ppBtnUpdate.ValidationGroup = "Edit"

            'ドロップダウンリスト設定
            Call sSetddlAPPACLASS()
            Call sSetddlSEARIAL_CLS()

            'グリッドの初期化
            Me.grvList.DataSource = New DataTable
            grvList.DataBind()

            'データ取得
            msGet_Data()

            mstrDispMode = "DEFAULT"
            ViewState("mstrDispMode") = mstrDispMode

            Me.ddlSrchAPPACLASS_CD.ppDropDownList.Focus()

        End If

    End Sub

    ''' <summary>
    ''' ページ成形前
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        If ViewState("ddlSrchAPPACLASS_CD") Is Nothing Then
            ddlSrchAPPACLASS_CD.ppSelectedValue = ""
        Else
            ddlSrchAPPACLASS_CD.ppSelectedValue = ViewState("ddlSrchAPPACLASS_CD")
        End If
        If ViewState("txtSrchAPPACLS_NM") Is Nothing Then
            txtSrchAPPACLS_NM.ppText = ""
        Else
            txtSrchAPPACLS_NM.ppText = ViewState("txtSrchAPPACLS_NM")
        End If
        If ViewState("ddlAPPACLASS_CD") Is Nothing Then
            ddlAPPACLASS_CD.ppSelectedValue = ""
        Else
            ddlAPPACLASS_CD.ppSelectedValue = ViewState("ddlAPPACLASS_CD")
        End If
        If ViewState("txtAPPA_CLS") Is Nothing Then
            txtAPPA_CLS.ppText = ""
        Else
            txtAPPA_CLS.ppText = ViewState("txtAPPA_CLS")
        End If
        If ViewState("txtAPPACLS_NM") Is Nothing Then
            txtAPPACLS_NM.ppText = ""
        Else
            txtAPPACLS_NM.ppText = ViewState("txtAPPACLS_NM")
        End If
        If ViewState("txtAPPACLS_SNM") Is Nothing Then
            txtAPPACLS_SNM.ppText = ""
        Else
            txtAPPACLS_SNM.ppText = ViewState("txtAPPACLS_SNM")
        End If
        If ViewState("htxtDELETE") Is Nothing Then
            htxtDELETE.Value = ""
        Else
            htxtDELETE.Value = ViewState("htxtDELETE")
        End If
        If ViewState("ddlSEARIAL_CLS") Is Nothing Then
            ddlSEARIAL_CLS.ppSelectedValue = ""
        Else
            ddlSEARIAL_CLS.ppSelectedValue = ViewState("ddlSEARIAL_CLS")
        End If
        If ViewState("ppchksDel") Is Nothing Then
            Master.ppchksDel.Checked = False
        Else
            If ViewState("ppchksDel") = True Then
                Master.ppchksDel.Checked = True
            Else
                Master.ppchksDel.Checked = False
            End If
        End If

        Me.ddlAPPACLASS_CD.ppDropDownList.Enabled = True
        Me.txtAPPA_CLS.ppTextBox.ReadOnly = False
        Me.txtAPPA_CLS.ppTextBox.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF")

        Select Case mstrDispMode
            Case "DEFAULT"
                Master.ppBtnInsert.Enabled = False      '追加
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnClear.Enabled = True       'クリア
                Me.ddlSrchAPPACLASS_CD.ppEnabled = True
                Me.txtSrchAPPACLS_NM.ppEnabled = True
                Me.ddlAPPACLASS_CD.ppEnabled = True
                Me.txtAPPA_CLS.ppEnabled = True
                Me.txtAPPACLS_NM.ppEnabled = True
                Me.txtAPPACLS_SNM.ppEnabled = True
                Master.ppBtnDelete.Text = "削除"
                Master.ppMainEnabled = True
            Case "SELECT"
                Master.ppBtnInsert.Enabled = False     '追加
                If Master.ppBtnDelete.Text = "削除取消" Then
                    Master.ppBtnUpdate.Enabled = False
                    Master.ppMainEnabled = False
                Else
                    Master.ppBtnUpdate.Enabled = True      '更新
                    Master.ppMainEnabled = True
                End If
                Master.ppBtnDelete.Enabled = True      '削除
                Master.ppBtnClear.Enabled = True       'クリア
                Me.ddlSrchAPPACLASS_CD.ppEnabled = True
                Me.txtSrchAPPACLS_NM.ppEnabled = True
                Me.ddlAPPACLASS_CD.ppEnabled = True
                Me.ddlAPPACLASS_CD.ppDropDownList.Enabled = False
                Me.txtAPPA_CLS.ppEnabled = True
                Me.txtAPPA_CLS.ppTextBox.ReadOnly = True
                Me.txtAPPA_CLS.ppTextBox.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC")
                Me.txtAPPACLS_NM.ppEnabled = True
                Me.txtAPPACLS_SNM.ppEnabled = True
            Case "INSERT"
                Master.ppBtnInsert.Enabled = True     '追加
                Master.ppBtnDelete.Text = "削除"
                Master.ppBtnDelete.Enabled = False      '削除
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnClear.Enabled = True       'クリア
                Me.ddlSrchAPPACLASS_CD.ppEnabled = True
                Me.txtSrchAPPACLS_NM.ppEnabled = True
                Me.ddlAPPACLASS_CD.ppEnabled = True
                Me.txtAPPA_CLS.ppEnabled = True
                Me.txtAPPACLS_NM.ppEnabled = True
                Me.txtAPPACLS_SNM.ppEnabled = True
        End Select

        ViewState("mstrDispMode") = mstrDispMode

    End Sub

    ''' <summary>
    ''' ページ作成終了
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Unload(sender As Object, e As EventArgs) Handles Me.Unload

        'ＤＢクローズ
        Call mclsDB.psDB_Close()

    End Sub

#End Region

    '----------------------------------------------------------------------------------------------------------------------------
    '-　ボタン制御
    '----------------------------------------------------------------------------------------------------------------------------
#Region "ボタン制御"

    ''' <summary>
    ''' 検索ボタン　押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        'データ取得
        If (Page.IsValid) Then

            '画面項目クリア
            '            Me.ddlSrchAPPACLASS_CD.ppSelectedValue = ""
            '            Me.txtSrchAPPACLS_NM.ppText = ""
            Me.ddlAPPACLASS_CD.ppSelectedValue = ""
            Me.txtAPPA_CLS.ppText = ""
            Me.txtAPPACLS_NM.ppText = ""
            Me.txtAPPACLS_SNM.ppText = ""

            'ＶＩＥＷＳＴＡＴＥクリア
            ViewState("ddlAPPACLASS_CD") = ddlAPPACLASS_CD.ppSelectedValue
            ViewState("txtAPPA_CLS") = txtAPPA_CLS.ppText
            ViewState("txtAPPACLS_NM") = txtAPPACLS_NM.ppText
            ViewState("txtAPPACLS_SNM") = txtAPPACLS_SNM.ppText
            ViewState("htxtDELETE") = htxtDELETE.Value

            'コントロールの制御
            Master.ppMainEnabled = True
            mstrDispMode = "DEFAULT"
            ViewState("mstrDispMode") = mstrDispMode

            If Master.ppchksDel.Checked.Equals(True) Then                                           '削除
                ViewState("strDeleteFlg") = "1"
            Else
                ViewState("strDeleteFlg") = String.Empty
            End If

            msGet_Data()

        End If

        'フォーカス移動
        Me.ddlAPPACLASS_CD.Focus()

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 検索条件クリア　押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearchClear_Click(sender As Object, e As EventArgs)
        'ログ出力開始
        psLogStart(Me)

        '画面項目クリア
        Me.ddlSrchAPPACLASS_CD.ppSelectedValue = ""
        Me.txtSrchAPPACLS_NM.ppText = ""
        'ＶＩＥＷＳＴＡＴＥクリア
        ViewState("ddlSrchAPPACLASS_CD") = ddlSrchAPPACLASS_CD.ppSelectedValue
        ViewState("txtSrchAPPACLS_NM") = txtSrchAPPACLS_NM.ppText
        Master.ppchksDel.Checked = False

        'フォーカス移動
        Me.ddlSrchAPPACLASS_CD.Focus()

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 追加／更新／削除ボタン　押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_Click(sender As Object, e As CommandEventArgs)

        'ログ出力開始
        psLogStart(Me)

        If (Page.IsValid) Then
            msEditData(e.CommandName)
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' クリアボタン　押下
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click()

        'ログ出力開始
        psLogStart(Me)

        '★排他情報削除
        If Not Me.Master.ppExclusiveDate = String.Empty Then
            If clsExc.pfDel_Exclusive(Me _
                               , Session(P_SESSION_SESSTION_ID) _
                               , Me.Master.ppExclusiveDate) = 0 Then
                Me.Master.ppExclusiveDate = String.Empty
            Else
                'ログ出力終了
                psLogEnd(Me)
                Exit Sub
            End If
        End If

        '画面項目クリア
        Me.ddlSrchAPPACLASS_CD.ppSelectedValue = ""
        Me.txtSrchAPPACLS_NM.ppText = ""
        Me.ddlAPPACLASS_CD.ppSelectedValue = ""
        Me.txtAPPA_CLS.ppText = ""
        Me.txtAPPACLS_NM.ppText = ""
        Me.txtAPPACLS_SNM.ppText = ""
        Me.ddlSEARIAL_CLS.ppSelectedValue = ""
        'ＶＩＥＷＳＴＡＴＥクリア
        ViewState("ddlAPPACLASS_CD") = ddlAPPACLASS_CD.ppSelectedValue
        ViewState("txtAPPA_CLS") = txtAPPA_CLS.ppText
        ViewState("txtAPPACLS_NM") = txtAPPACLS_NM.ppText
        ViewState("txtAPPACLS_SNM") = txtAPPACLS_SNM.ppText
        ViewState("ddlSEARIAL_CLS") = ddlSEARIAL_CLS.ppSelectedValue
        ViewState("htxtDELETE") = htxtDELETE.Value
        mstrDispMode = "DEFAULT"
        ViewState("mstrDispMode") = mstrDispMode
        Me.ddlAPPACLASS_CD.ppSelectedValue = ""

        'ログ出力終了
        psLogEnd(Me)

    End Sub

#End Region

    '----------------------------------------------------------------------------------------------------------------------------
    '-　ボタン以外のコントロール制御
    '----------------------------------------------------------------------------------------------------------------------------
#Region "ボタン以外のコントロール制御"

    Private Sub ddlAPPACLASS_CD_TextChanged()

        If mstrDispMode = "SELECT" Then
        ElseIf mstrDispMode = "INSERT" Then
        ElseIf mstrDispMode = "DELETE" Then
        ElseIf mstrDispMode = "UPDATE" Then
        Else
            mstrDispMode = "INSERT"
            ViewState("mstrDispMode") = mstrDispMode
        End If
        Me.txtAPPA_CLS.ppTextBox.Focus()

    End Sub

    Private Sub txtAPPA_CLS_TextChanged()

        If mstrDispMode = "SELECT" Then
        ElseIf mstrDispMode = "INSERT" Then
        ElseIf mstrDispMode = "DELETE" Then
        ElseIf mstrDispMode = "UPDATE" Then
        Else
            mstrDispMode = "INSERT"
            ViewState("mstrDispMode") = mstrDispMode
        End If
        Me.txtAPPACLS_NM.ppTextBox.Focus()

    End Sub

    Private Sub txtAPPACLS_NM_TextChanged()

        If mstrDispMode = "SELECT" Then
        ElseIf mstrDispMode = "INSERT" Then
        ElseIf mstrDispMode = "DELETE" Then
        ElseIf mstrDispMode = "UPDATE" Then
        Else
            mstrDispMode = "INSERT"
            ViewState("mstrDispMode") = mstrDispMode
        End If
        Me.txtAPPACLS_SNM.ppTextBox.Focus()

    End Sub

    Private Sub txtAPPACLS_SNM_TextChanged()

        If mstrDispMode = "SELECT" Then
        ElseIf mstrDispMode = "INSERT" Then
        ElseIf mstrDispMode = "DELETE" Then
        ElseIf mstrDispMode = "UPDATE" Then
        Else
            mstrDispMode = "INSERT"
            ViewState("mstrDispMode") = mstrDispMode
        End If
        Me.ddlSEARIAL_CLS.ppDropDownList.Focus()

    End Sub

    Private Sub ddlSEARIAL_CLS_TextChanged()

        If mstrDispMode = "SELECT" Then
        ElseIf mstrDispMode = "INSERT" Then
        ElseIf mstrDispMode = "DELETE" Then
        ElseIf mstrDispMode = "UPDATE" Then
        Else
            mstrDispMode = "INSERT"
            ViewState("mstrDispMode") = mstrDispMode
        End If
        '        Me.txtAPPACLS_NM.ppTextBox.Focus()
        Master.ppBtnClear.Focus()

    End Sub

#End Region

    '----------------------------------------------------------------------------------------------------------------------------
    '-　グリッドビュー制御
    '----------------------------------------------------------------------------------------------------------------------------
#Region "グリッドビュー制御"

    Private Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        For Each rowData As GridViewRow In grvList.Rows
            If CType(rowData.FindControl("CM06_DELETE_FLG"), TextBox).Text = "1" Then
                If rowData.HasControls = True Then
                    Dim zz As Integer = 0
                    For zz = 0 To rowData.Cells.Count - 1
                        If rowData.Cells(zz).HasControls Then
                            For yy = 0 To rowData.Cells(zz).Controls.Count - 1
                                If rowData.Cells(zz).Controls(yy).GetType.ToString = "System.Web.UI.WebControls.TextBox" Then
                                    DirectCast(rowData.Cells(zz).Controls(0), TextBox).ForeColor = Drawing.Color.Red
                                End If
                            Next
                        End If
                    Next
                End If
            End If
        Next

    End Sub

    ''' <summary>
    ''' GRID RowCommand
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub grvList_RowCommand(sender As Object, e As CommandEventArgs) Handles grvList.RowCommand

        Try
            If e.CommandName = "Select" Then

                'ログ出力開始
                psLogStart(Me)

                '★排他制御処理
                Dim strExclusiveDate As String = Nothing
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList
                Dim rowData As GridViewRow = grvList.Rows(Convert.ToInt32(e.CommandArgument))

                '★排他情報削除
                If Not Me.Master.ppExclusiveDate = String.Empty Then
                    If clsExc.pfDel_Exclusive(Me _
                                       , Session(P_SESSION_SESSTION_ID) _
                                       , Me.Master.ppExclusiveDate) = 0 Then
                        Me.Master.ppExclusiveDate = String.Empty
                    Else
                        'ログ出力終了
                        psLogEnd(Me)
                        Exit Sub
                    End If
                End If

                '★ロック対象テーブル名の登録
                arTable_Name.Insert(0, TableName)

                '★ロックテーブルキー項目の登録
                arKey.Insert(0, CType(rowData.FindControl("CM06_APPACLASS_CD"), TextBox).Text)
                arKey.Insert(0, CType(rowData.FindControl("CM06_APPA_CLS"), TextBox).Text)

                '★排他情報確認処理(更新画面へ遷移)
                If clsExc.pfSel_Exclusive(strExclusiveDate _
                                 , Me _
                                 , Session(P_SESSION_IP) _
                                 , Session(P_SESSION_PLACE) _
                                 , Session(P_SESSION_USERID) _
                                 , Session(P_SESSION_SESSTION_ID) _
                                 , ViewState(P_SESSION_GROUP_NUM) _
                                 , DispCode _
                                 , arTable_Name _
                                 , arKey) = 0 Then

                    '★登録年月日時刻(明細)
                    Me.Master.ppExclusiveDate = strExclusiveDate

                Else
                    '排他ロック中
                    'ログ出力終了
                    psLogEnd(Me)
                    Exit Sub
                End If

                '編集エリアに値を設定
                Me.ddlAPPACLASS_CD.ppSelectedValue = CType(rowData.FindControl("CM06_APPACLASS_CD"), TextBox).Text
                Me.txtAPPA_CLS.ppText = CType(rowData.FindControl("CM06_APPA_CLS"), TextBox).Text
                Me.txtAPPACLS_NM.ppText = CType(rowData.FindControl("CM06_APPACLS_NM"), TextBox).Text
                Me.txtAPPACLS_SNM.ppText = CType(rowData.FindControl("CM06_SHORT_NM"), TextBox).Text
                Me.ddlSEARIAL_CLS.ppDropDownList.SelectedValue = CType(rowData.FindControl("CM06_SEARIAL_CNTL_CLS"), TextBox).Text
                Me.htxtDELETE.Value = CType(rowData.FindControl("CM06_DELETE_FLG"), TextBox).Text
                If CType(rowData.FindControl("CM06_DELETE_FLG"), TextBox).Text = "0" Then
                    Master.ppBtnDelete.Text = "削除"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '削除
                Else
                    Master.ppBtnDelete.Text = "削除取消"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
                    'Master.ppMainEnabled = False
                End If
                ViewState("ddlAPPACLASS_CD") = CType(rowData.FindControl("CM06_APPACLASS_CD"), TextBox).Text
                ViewState("txtAPPA_CLS") = CType(rowData.FindControl("CM06_APPA_CLS"), TextBox).Text
                ViewState("txtAPPACLS_NM") = CType(rowData.FindControl("CM06_APPACLS_NM"), TextBox).Text
                ViewState("txtAPPACLS_SNM") = CType(rowData.FindControl("CM06_SHORT_NM"), TextBox).Text
                ViewState("ddlSEARIAL_CLS") = CType(rowData.FindControl("CM06_SEARIAL_CNTL_CLS"), TextBox).Text
                ViewState("htxtDELETE") = CType(rowData.FindControl("CM06_DELETE_FLG"), TextBox).Text

                mstrDispMode = "SELECT"
                ViewState("mstrDispMode") = mstrDispMode
            End If

            Me.ddlAPPACLASS_CD.Focus()

        Catch ex As Exception
            'エラー
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
            objStack = New StackFrame
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
        'ログ出力終了
        psLogEnd(Me)
    End Sub


#End Region

#End Region

    '============================================================================================================================
    '=　プロシージャ
    '============================================================================================================================
#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' データ取得およびバインド
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data()

        Dim objSQLCmd As New SqlClient.SqlCommand
        Dim objwkDS As New DataSet
        objStack = New StackFrame

        Try
            '画面ページ表示初期化
            Master.ppCount = "0"
            Me.grvList.DataSource = Nothing
            grvList.DataBind()

            objSQLCmd.Connection = mclsDB.mobjDB
            objSQLCmd.CommandText = "COMUPDM06_S1"
            objSQLCmd.Parameters.Add("prmAPPACLASS_CD", SqlDbType.NVarChar)
            objSQLCmd.Parameters.Add("prmAPPACLS_NM", SqlDbType.NVarChar)
            objSQLCmd.Parameters.Add("prmppchksDel", SqlDbType.NVarChar)
            Dim strprmValue As String = ""
            If ViewState("ddlSrchAPPACLASS_CD") Is Nothing Then
                strprmValue = ""
            ElseIf ViewState("ddlSrchAPPACLASS_CD") = "" Then
                strprmValue = ""
            Else
                strprmValue = ViewState("ddlSrchAPPACLASS_CD")
            End If
            objSQLCmd.Parameters("prmAPPACLASS_CD").Value = strprmValue
            strprmValue = ""
            If ViewState("txtSrchAPPACLS_NM") Is Nothing Then
                strprmValue = ""
            ElseIf ViewState("txtSrchAPPACLS_NM") = "" Then
                strprmValue = ""
            Else
                strprmValue = ViewState("txtSrchAPPACLS_NM")
            End If
            objSQLCmd.Parameters("prmAPPACLS_NM").Value = strprmValue
            If mstrDispMode = "FIRST" Then
                objSQLCmd.Parameters("prmAPPACLASS_CD").Value = ""
                objSQLCmd.Parameters("prmAPPACLS_NM").Value = ""
            End If
            If ViewState("ppchksDel") = True Then
                objSQLCmd.Parameters("prmppchksDel").Value = "1"
            ElseIf ViewState("ppchksDel") = False Then
                objSQLCmd.Parameters("prmppchksDel").Value = "0"
            Else
                objSQLCmd.Parameters("prmppchksDel").Value = "0"
            End If

            If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
                'エラー
                clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "リスト作成")
                objStack = New StackFrame
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "リスト情報取得失敗", "Catch")
                Exit Sub
            End If

            Master.ppCount = 0

            If objwkDS.Tables.Count > 0 Then
                If objwkDS.Tables(0).Rows.Count > 0 Then
                    Me.grvList.DataSource = objwkDS.Tables(0)
                    grvList.DataBind()
                    '件数を設定
                    Master.ppCount = objwkDS.Tables(0).Rows(0).Item("CMAXCNT").ToString
                    '閾値を超えた場合はメッセージを表示
                    If objwkDS.Tables(0).Rows(0).Item("CMAXCNT") > objwkDS.Tables(0).Rows(0).Item("CLIMITCNT") Then
                        psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, objwkDS.Tables(0).Rows(0).Item("CLIMITCNT").value.ToString.Trim, objwkDS.Tables(0).Rows(0).Item("CMAXCNT").value.ToString.Trim)
                    End If
                Else
                    '0件
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    Master.ppCount = 0
                End If
            End If

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器種別マスタ")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ipmstrDispMode"></param>
    ''' <remarks></remarks>
    Private Sub msEditData(ByVal ipmstrDispMode As String)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim MesCode As String = String.Empty
        Dim dttGrid As New DataTable
        Dim strStored As String = String.Empty
        Dim getFlg As String = "0"
        Dim objDBTran As SqlClient.SqlTransaction = mclsDB.mobjDB.BeginTransaction
        objStack = New StackFrame

        Select Case ipmstrDispMode
            Case "INSERT"
                MesCode = "00003"
                strStored = "COMUPDM06_I1"
            Case "UPDATE"
                MesCode = "00001"
                strStored = "COMUPDM06_U1"
            Case "DELETE"
                Select Case Master.ppBtnDelete.Text
                    Case "削除"
                        MesCode = "00002"
                        strStored = "COMUPDM06_U2"
                        getFlg = "1"
                    Case "削除取消"
                        MesCode = "00001"
                        strStored = "COMUPDM06_U2"
                        getFlg = "0"
                End Select
        End Select

        Try
            ''INSERTの場合、更新するかのチェックを行う
            'If ipmstrDispMode = "INSERT" Then
            '    cmdDB = New SqlCommand(DispCode & "_S3", conDB)
            '    cmdDB.Parameters.Add(pfSet_Param("MainteCd", SqlDbType.NVarChar, Me.txtMainteCd.ppText))       '保担コード
            '    'データ取得およびデータをリストに設定
            '    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            '    If dstOrders.Tables(0).Rows.Count > 0 Then
            '        'psMesBox(Me, "30008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
            '        'Exit Sub
            '    End If
            'End If

            Dim objSQLCmd As New SqlClient.SqlCommand
            Dim intRetCD As Integer = 0
            Dim strMesType As String = "00001"
            Dim strwkBuff As String = ""

            objSQLCmd.Connection = mclsDB.mobjDB
            objSQLCmd.CommandText = strStored

            objSQLCmd.Parameters.Add("prmM06_APPACLASS_CD", SqlDbType.NVarChar)
            objSQLCmd.Parameters.Add("prmM06_APPA_CLS", SqlDbType.NVarChar)

            strwkBuff = ""
            If ViewState("ddlAPPACLASS_CD") Is Nothing Then
                strwkBuff = ""
            Else
                strwkBuff = ViewState("ddlAPPACLASS_CD").ToString.Trim
            End If
            objSQLCmd.Parameters("prmM06_APPACLASS_CD").Value = strwkBuff

            strwkBuff = ""
            If ViewState("txtAPPA_CLS") Is Nothing Then
                strwkBuff = ""
            Else
                strwkBuff = ViewState("txtAPPA_CLS").ToString.Trim
            End If
            objSQLCmd.Parameters("prmM06_APPA_CLS").Value = strwkBuff

            Select Case ipmstrDispMode
                Case "INSERT"
                    objSQLCmd.Parameters.Add("prmM06_APPACLS_NM", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM06_SHORT_NM", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM06_DELETE_FLG", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM06_INSERT_USR", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM06_SEARIAL_CNTL_CLS", SqlDbType.NVarChar)

                    strwkBuff = ""
                    If ViewState("txtAPPACLS_NM") Is Nothing Then
                        strwkBuff = ""
                    Else
                        strwkBuff = ViewState("txtAPPACLS_NM").ToString.Trim
                    End If
                    objSQLCmd.Parameters("prmM06_APPACLS_NM").Value = strwkBuff

                    strwkBuff = ""
                    If ViewState("txtAPPACLS_SNM") Is Nothing Then
                        strwkBuff = ""
                    Else
                        strwkBuff = ViewState("txtAPPACLS_SNM").ToString.Trim
                    End If
                    objSQLCmd.Parameters("prmM06_SHORT_NM").Value = strwkBuff

                    objSQLCmd.Parameters("prmM06_DELETE_FLG").Value = "0"
                    objSQLCmd.Parameters("prmM06_INSERT_USR").Value = User.Identity.Name

                    strwkBuff = ""
                    If ViewState("ddlSEARIAL_CLS") Is Nothing Then
                        strwkBuff = ""
                    Else
                        strwkBuff = ViewState("ddlSEARIAL_CLS").ToString.Trim
                    End If
                    objSQLCmd.Parameters("prmM06_SEARIAL_CNTL_CLS").Value = strwkBuff

                Case "UPDATE"
                    objSQLCmd.Parameters.Add("prmM06_APPACLS_NM", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM06_SHORT_NM", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM06_DELETE_FLG", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM06_UPDATE_USR", SqlDbType.NVarChar)
                    objSQLCmd.Parameters.Add("prmM06_SEARIAL_CNTL_CLS", SqlDbType.NVarChar)

                    strwkBuff = ""
                    If ViewState("txtAPPACLS_NM") Is Nothing Then
                        strwkBuff = ""
                    Else
                        strwkBuff = ViewState("txtAPPACLS_NM").ToString.Trim
                    End If
                    objSQLCmd.Parameters("prmM06_APPACLS_NM").Value = strwkBuff

                    strwkBuff = ""
                    If ViewState("txtAPPACLS_SNM") Is Nothing Then
                        strwkBuff = ""
                    Else
                        strwkBuff = ViewState("txtAPPACLS_SNM").ToString.Trim
                    End If
                    objSQLCmd.Parameters("prmM06_SHORT_NM").Value = strwkBuff

                    objSQLCmd.Parameters("prmM06_DELETE_FLG").Value = "0"
                    objSQLCmd.Parameters("prmM06_UPDATE_USR").Value = User.Identity.Name

                    strwkBuff = ""
                    If ViewState("ddlSEARIAL_CLS") Is Nothing Then
                        strwkBuff = ""
                    Else
                        strwkBuff = ViewState("ddlSEARIAL_CLS").ToString.Trim
                    End If
                    objSQLCmd.Parameters("prmM06_SEARIAL_CNTL_CLS").Value = strwkBuff

                Case "DELETE"
                    Select Case Master.ppBtnDelete.Text
                        Case "削除"
                            objSQLCmd.Parameters.Add("prmM06_DELETE_FLG", SqlDbType.NVarChar)
                            objSQLCmd.Parameters.Add("prmM06_UPDATE_USR", SqlDbType.NVarChar)

                            objSQLCmd.Parameters("prmM06_DELETE_FLG").Value = "1"
                            objSQLCmd.Parameters("prmM06_UPDATE_USR").Value = User.Identity.Name

                        Case "削除取消"
                            objSQLCmd.Parameters.Add("prmM06_DELETE_FLG", SqlDbType.NVarChar)
                            objSQLCmd.Parameters.Add("prmM06_UPDATE_USR", SqlDbType.NVarChar)

                            objSQLCmd.Parameters("prmM06_DELETE_FLG").Value = "0"
                            objSQLCmd.Parameters("prmM06_UPDATE_USR").Value = User.Identity.Name

                    End Select
            End Select

            'objSQLCmd.Parameters.Add("prmM06_APPACLASS_CD", SqlDbType.NVarChar)
            'objSQLCmd.Parameters.Add("prmM06_APPA_CLS", SqlDbType.NVarChar)
            'objSQLCmd.Parameters.Add("prmM06_APPACLS_NM", SqlDbType.NVarChar)
            'objSQLCmd.Parameters.Add("prmM06_SHORT_NM", SqlDbType.NVarChar)
            'objSQLCmd.Parameters.Add("prmM06_DELETE_FLG", SqlDbType.NVarChar)
            'objSQLCmd.Parameters.Add("prmM06_INSERT_USR", SqlDbType.NVarChar)
            'objSQLCmd.Parameters.Add("prmM06_UPDATE_USR", SqlDbType.NVarChar)
            'objSQLCmd.Parameters.Add("prmM06_SEARIAL_CNTL_CLS", SqlDbType.NVarChar)

            Try
                'トランザクション開始
                'If mclsDB.pfDB_BeginTrans = False Then
                '    '失敗メッセージ表示
                '    psMesBox(Me, strMesType, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                '    mstrDispMode = "UPD"
                '    Exit Sub
                'End If
                objSQLCmd.Transaction = objDBTran

                'SQL実行
                If mclsDB.pfDB_ProcStored(objSQLCmd) Then
                Else
                    '失敗メッセージ表示
                    psMesBox(Me, strMesType, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                    '                    mstrDispMode = "UPD"
                    objSQLCmd.Transaction.Rollback()
                    Exit Sub
                End If

                'コミット
                objSQLCmd.Transaction.Commit()

                Dim objwkDS As New DataSet
                objSQLCmd = Nothing
                objSQLCmd = New SqlClient.SqlCommand

                Call msGet_Data()

                'Try

                '    objSQLCmd.Connection = mclsDB.mobjDB
                '    objSQLCmd.CommandText = "COMUPDM06_S4"
                '    objSQLCmd.Parameters.Add("prmM06_APPACLASS_CD", SqlDbType.NVarChar)
                '    objSQLCmd.Parameters.Add("prmM06_APPA_CLS", SqlDbType.NVarChar)

                '    strwkBuff = ""
                '    If ViewState("ddlAPPACLASS_CD") Is Nothing Then
                '        strwkBuff = ""
                '    Else
                '        strwkBuff = ViewState("ddlAPPACLASS_CD").ToString.Trim
                '    End If
                '    objSQLCmd.Parameters("prmM06_APPACLASS_CD").Value = strwkBuff

                '    strwkBuff = ""
                '    If ViewState("txtAPPA_CLS") Is Nothing Then
                '        strwkBuff = ""
                '    Else
                '        strwkBuff = ViewState("txtAPPA_CLS").ToString.Trim
                '    End If
                '    objSQLCmd.Parameters("prmM06_APPA_CLS").Value = strwkBuff

                '    If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
                '        'エラー
                '        clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
                '        objStack = New StackFrame
                '        'ログ出力
                '        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "明細情報取得失敗", "Catch")
                '        Exit Sub
                '    End If

                '    Me.grvList.DataSource = objwkDS.Tables(0)
                '    Me.grvList.DataBind()

                'Catch ex As Exception
                '    'エラー
                '    clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
                '    objStack = New StackFrame
                '    'ログ出力
                '    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '    mstrDispMode = "UPDATE"
                '    'トランザクションが残ってたらロールバック
                '    If objSQLCmd.Transaction Is Nothing Then
                '    Else
                '        objSQLCmd.Transaction.Rollback()
                '    End If
                '    Exit Sub
                'Finally
                '    Call mclsDB.psDisposeDataSet(objwkDS)
                'End Try

            Catch ex As Exception

                '失敗メッセージ表示
                psMesBox(Me, strMesType, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                '                mstrDispMode = "UPD"

            End Try

            'トランザクションが残ってたらロールバック
            If objSQLCmd.Transaction Is Nothing Then
            Else
                objSQLCmd.Transaction.Rollback()
            End If

            psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName)
            'If getFlg = "0" Then
            '    msGet_Data()
            'Else
            '    'グリッドの初期化
            '    Me.grvList.DataSource = New DataTable
            '    grvList.DataBind()
            'End If
            Call btnClear_Click()

        Catch ex As Exception
            psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
        End Try

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（機器分類）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub sSetddlAPPACLASS()

        Dim objSQLCmd As New SqlClient.SqlCommand
        Dim objwkDS As New DataSet

        objSQLCmd.Connection = mclsDB.mobjDB
        objSQLCmd.CommandText = "COMUPDM06_S2"
        If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
            'エラー
            clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "リスト作成")
            objStack = New StackFrame
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "リスト情報取得失敗", "Catch")
            Exit Sub
        End If

        If objwkDS.Tables.Count > 0 Then
            '検索用項目
            Me.ddlSrchAPPACLASS_CD.ppDropDownList.DataSource = objwkDS.Tables(0)
            Me.ddlSrchAPPACLASS_CD.ppDropDownList.DataTextField = "M73_APPACLASS_NM_DISP"
            Me.ddlSrchAPPACLASS_CD.ppDropDownList.DataValueField = "M73_APPACLASS_CD"
            Me.ddlSrchAPPACLASS_CD.DataBind()
            Me.ddlSrchAPPACLASS_CD.ppDropDownList.Items.Insert(0, "")
            '入力エリア項目
            Me.ddlAPPACLASS_CD.ppDropDownList.DataSource = objwkDS.Tables(0)
            Me.ddlAPPACLASS_CD.ppDropDownList.DataTextField = "M73_APPACLASS_NM_DISP"
            Me.ddlAPPACLASS_CD.ppDropDownList.DataValueField = "M73_APPACLASS_CD"
            Me.ddlAPPACLASS_CD.DataBind()
            Me.ddlAPPACLASS_CD.ppDropDownList.Items.Insert(0, "")
        End If

        Call mclsDB.psDisposeDataSet(objwkDS)

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定　シリアル管理区分
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub sSetddlSEARIAL_CLS()

        Dim objSQLCmd As New SqlClient.SqlCommand
        Dim objwkDS As New DataSet

        objSQLCmd.Connection = mclsDB.mobjDB
        objSQLCmd.CommandText = "COMUPDM06_S3"
        If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
            'エラー
            clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "リスト作成")
            objStack = New StackFrame
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "リスト情報取得失敗", "Catch")
            Exit Sub
        End If

        If objwkDS.Tables.Count > 0 Then
            '入力エリア項目
            Me.ddlSEARIAL_CLS.ppDropDownList.DataSource = objwkDS.Tables(0)
            Me.ddlSEARIAL_CLS.ppDropDownList.DataTextField = "M29_NAME_DISP"
            Me.ddlSEARIAL_CLS.ppDropDownList.DataValueField = "M29_CODE"
            Me.ddlSEARIAL_CLS.DataBind()
            Me.ddlSEARIAL_CLS.ppDropDownList.Items.Insert(0, "")
        End If

        Call mclsDB.psDisposeDataSet(objwkDS)

    End Sub

    'フォーカス移動
    Private Sub FocusChange(ByVal TxtBoxF As TextBox, ByVal TxtBoxT As TextBox)

        Dim strBtnDmy As String = Master.ppBtnDmy.ClientID
        Master.ppBtnDmy.Visible = True

        Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + TxtBoxT.ClientID + ");"
        SetFocus(Master.ppBtnDmy.ClientID)

    End Sub

#End Region

#Region "終了処理プロシージャ"

#End Region

End Class























