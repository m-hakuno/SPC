'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　工事名マスタ管理
'*　ＰＧＭＩＤ：　COMUPDM48
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.11.26　：　伯野
'*  作　成　　：　2016.08.18　：　伯野　開始年月日を常時使用可能とする
'********************************************************************************************************************************
'COMUPDM48-001 2016.08.18 伯野　開始年月日を常時使用可能とする

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SQL_DBCLS_LIB
Imports System.Threading


Public Class COMUPDM48

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
    '画面情報
    Const DispCode As String = "COMUPDM48"             '画面ID
    'マスタ情報
    Const MasterName As String = "工事名マスタ"           'マスタ名
    Const TableName As String = "M48_DISPLAY_ITEM"              'テーブル名
    'キー情報
    Const KeyName1 As String = "M48_CODE"
    Const KeyName2 As String = "M48_SUMSTART_D"
    Const KeyName3 As String = ""
    Const KeyName4 As String = ""
    Const KeyName5 As String = ""
    Const intDelCls As Integer = 0         '削除種別 0:削除なし  1:削除フラグ  2:DELETE
    'ソート情報
    Const SortKey As String = " ORDER BY " & KeyName1 & "   "
    '検索条件項目
    Const cstSrchKey_CNSTCLS As String = "txtSrchCODE"
    Const cstSrchKey_TBOXCLS As String = "txtSrchCNST_NM"
    Const cstSpace7 As String = "       "
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
    '    Dim stb As New StringBuilder
    '    Dim dv As New DataView
    '    Dim dt As New DataTable
    Dim mstrDispMode As String
    '    Dim RcdCase As String = ""

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
    ''' ページ読み込み
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        mclsDB.mstrConString = ConfigurationManager.ConnectionStrings("SPCDB").ToString
        If mclsDB.pfDB_Connect() = False Then
            'ＤＢ接続失敗
        End If

        ViewState(cstSrchKey_CNSTCLS) = txtSrchCODE.ppText
        ViewState(cstSrchKey_TBOXCLS) = txtSrchCNST_NM.ppText
        ViewState("txtCODE") = txtCODE.ppText
        ViewState("txtCNST_NM") = txtCNST_NM.ppText
        ViewState("cdbSUMSTART_D") = cdbSUMSTART_D.ppText
        ViewState("txtNML_PRICE") = txtNML_PRICE.ppText
        ViewState("txtHLDY_PRICE") = txtHLDY_PRICE.ppText
        ViewState("txtNGHT_PRICE") = txtNGHT_PRICE.ppText
        ViewState("ddlL_CLS") = ddlL_CLS.ppDropDownList.SelectedValue
        ViewState("ddlM_CLS") = ddlM_CLS.ppDropDownList.SelectedValue
        If ViewState("mstrDispMode") Is Nothing Then
            mstrDispMode = "DEF"
        Else
            mstrDispMode = ViewState("mstrDispMode")
        End If

        '初回実行
        If Not IsPostBack Then
            mstrDispMode = "First"
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            'キー情報保存用ViewState初期化
            Me.ViewState.Add("KeyName1", "")
            Me.ViewState.Add("KeyName2", "")
            Me.ViewState.Add("KeyName3", "")
            Me.ViewState.Add("KeyName4", "")
            Me.ViewState.Add("KeyName5", "")
            '削除データ検索フラグ初期化
            Me.ViewState.Add("SrchDelFlg", "0")
            'Me.ViewState.Add("strWHERE", "")

            'データ取得
            Select Case intDelCls
                Case 0
                    grvList.DataSource = Nothing
                    grvList.DataBind()
                Case 1
                    Master.chksDelVisible(True)
                    grvList.DataSource = Nothing
                    grvList.DataBind()
                    Master.ppCount = "0"

                    '非削除データ取得
                    'GetData_and_GridBind(" WHERE " & TableName.Substring(0, 4) & "DELETE_FLG = 0 ")
                Case 2
                    '                    GetData_and_GridBind()
            End Select
            Call GetData_and_GridBind()

            'ドロップダウンデータ取得
            '            setDropDownList()

            'コントロールの活性制御
            setControl("Default")
            setControl("Search")

            'フォーカス設定
            Me.txtSrchCODE.Focus()
        End If

        Call ddlL_CLS_DataBind()
        Call ddlM_CLS_DataBind()

        'ボタンイベント設定
        AddHandler Master.ppBtnSrcClear.Click, AddressOf BtnSearchClear_Click
        AddHandler Master.ppBtnSearch.Click, AddressOf BtnSearch_Click
        AddHandler Master.ppBtnClear.Click, AddressOf BtnClear_Click
        AddHandler Master.ppBtnInsert.Click, AddressOf BtnInsert_Click
        AddHandler Master.ppBtnUpdate.Click, AddressOf BtnUpdate_Click
        AddHandler Master.ppBtnDelete.Click, AddressOf BtnDelete_Click
        AddHandler cdbSUMSTART_D.ppDateBox.TextChanged, AddressOf cdbSUMSTART_D_TextChanged
        AddHandler txtCODE.ppTextBox.TextChanged, AddressOf txtCODE_TextChanged
        AddHandler ddlL_CLS.ppDropDownList.TextChanged, AddressOf ddlL_CLS_TextChanged

        ddlL_CLS.ppDropDownList.AutoPostBack = True

        'キー入力イベント設定
        '        AddHandler txtAPPACD.TextChanged, AddressOf inputSelectData

        'フォーカス用スクリプト設定
        Master.ppBtnDmy.Attributes.Add("onfocus", "")

        'grvList.Columns.Item(2).HeaderStyle.CssClass = "GridNoDisp"

    End Sub

    ''' <summary>
    ''' ページ成形前
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        If ViewState(cstSrchKey_CNSTCLS) Is Nothing Then
        Else
            txtSrchCODE.ppText = ViewState(cstSrchKey_CNSTCLS)
        End If
        If ViewState(cstSrchKey_TBOXCLS) Is Nothing Then
        Else
            txtSrchCNST_NM.ppText = ViewState(cstSrchKey_TBOXCLS)
        End If
        If ViewState("txtCODE") Is Nothing Then
        Else
            txtCODE.ppText = ViewState("txtCODE")
        End If
        If ViewState("txtCNST_NM") Is Nothing Then
        Else
            txtCNST_NM.ppText = ViewState("txtCNST_NM")
        End If
        If ViewState("cdbSUMSTART_D") Is Nothing Then
        Else
            cdbSUMSTART_D.ppText = ViewState("cdbSUMSTART_D")
        End If
        If ViewState("txtNML_PRICE") Is Nothing Then
        Else
            txtNML_PRICE.ppText = ViewState("txtNML_PRICE")
        End If
        If ViewState("txtHLDY_PRICE") Is Nothing Then
        Else
            txtHLDY_PRICE.ppText = ViewState("txtHLDY_PRICE")
        End If
        If ViewState("txtNGHT_PRICE") Is Nothing Then
        Else
            txtNGHT_PRICE.ppText = ViewState("txtNGHT_PRICE")
        End If
        If ViewState("ddlL_CLS") Is Nothing Then
        Else
            ddlL_CLS.ppDropDownList.SelectedValue = ViewState("ddlL_CLS")
        End If
        If ViewState("ddlM_CLS") Is Nothing Then
        Else
            ddlM_CLS.ppDropDownList.SelectedValue = ViewState("ddlM_CLS")
        End If

        '閉じるボタンでEXIT
        If Master.ppCount = "close" OrElse mstrDispMode = "EXIT" Then
            Exit Sub
        End If

        'キー項目の読み取り専用を解除
        Me.txtCODE.ppTextBox.ReadOnly = False
        Me.cdbSUMSTART_D.ppDateBox.ReadOnly = False
        Me.txtCODE.ppTextBox.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF")
        Me.cdbSUMSTART_D.ppDateBox.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF")
        'ボタンの使用制限
        Select Case mstrDispMode
            Case "DEF", "First"
                '初期　追加
                txtCODE.ppEnabled = True
                txtCNST_NM.ppEnabled = True
                cdbSUMSTART_D.ppEnabled = True
                txtNML_PRICE.ppEnabled = True
                txtHLDY_PRICE.ppEnabled = True
                txtNGHT_PRICE.ppEnabled = True
                ddlL_CLS.ppEnabled = True
                ddlM_CLS.ppEnabled = True
                txtSrchCODE.ppText = ""
                txtSrchCNST_NM.ppText = ""
                txtCODE.ppText = ""
                txtCNST_NM.ppText = ""
                cdbSUMSTART_D.ppText = ""
                txtNML_PRICE.ppText = ""
                txtHLDY_PRICE.ppText = ""
                txtNGHT_PRICE.ppText = ""
                ddlL_CLS.ppDropDownList.SelectedValue = ""
                ddlM_CLS.ppDropDownList.SelectedValue = ""
                Master.ppBtnInsert.Enabled = True
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnClear.Enabled = True
                Master.ppBtnDelete.Enabled = False
'                Master.ppCount = "0"
            Case "ADD"
                '初期　追加
                txtCODE.ppEnabled = True
                txtCNST_NM.ppEnabled = True
                cdbSUMSTART_D.ppEnabled = True
                txtNML_PRICE.ppEnabled = True
                txtHLDY_PRICE.ppEnabled = True
                txtNGHT_PRICE.ppEnabled = True
                ddlL_CLS.ppEnabled = True
                ddlM_CLS.ppEnabled = True
                Master.ppBtnClear.Enabled = True
                Master.ppBtnInsert.Enabled = True
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnDelete.Enabled = False
            Case "UPD", "SEL"
                '選択後　更新　削除
                txtCODE.ppEnabled = True
                txtCNST_NM.ppEnabled = True
                cdbSUMSTART_D.ppEnabled = True
                txtNML_PRICE.ppEnabled = True
                txtHLDY_PRICE.ppEnabled = True
                txtNGHT_PRICE.ppEnabled = True
                ddlL_CLS.ppEnabled = True
                ddlM_CLS.ppEnabled = True
                Master.ppBtnInsert.Enabled = False
                Master.ppBtnUpdate.Enabled = True
                Master.ppBtnClear.Enabled = True
                Master.ppBtnDelete.Enabled = True
                'キー項目を読み取り専用に設定
                '                Me.txtCODE.ppTextBox.ReadOnly = True
                '                If Me.cdbSUMSTART_D.ppDateBox.Text = "1900/01/01" Then
                '                    Me.cdbSUMSTART_D.ppDateBox.ReadOnly = True
                '                    Me.txtCODE.ppTextBox.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC")
                '                    Me.cdbSUMSTART_D.ppDateBox.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC")
                '                Else
                Me.cdbSUMSTART_D.ppDateBox.ReadOnly = False
                Me.txtCODE.ppTextBox.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF")
                Me.cdbSUMSTART_D.ppDateBox.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF")
                '                End If
                'Case "SEL"
                '    Me.ddlSrchCNST_CLS.Enabled = False
                '    Me.ddlSrchTBOXCLS_CD.Enabled = False
                '    Me.txtCnst_Cls.Enabled = True
                '    Me.txtCnstCls_NM.Enabled = True
                '    Me.ddlTBOXCLS_CD.Enabled = True
                '    Me.txtSEQNO.Enabled = True
                '    Me.ddlWork_Cls.Enabled = True
                '    Master.ppBtnInsert.Enabled = False
                '    Master.ppBtnDelete.Enabled = True
                '    Master.ppBtnUpdate.Enabled = True
                '    Master.ppBtnClear.Enabled = True
                '            Case "DEL"
                '                '削除データ選択時
                '                Master.ppBtnInsert.Enabled = False
                '                Master.ppBtnUpdate.Enabled = False
                '                Master.ppBtnDelete.Enabled = True
                '                Master.ppBtnDmy.Visible = False
            Case Else
                '初期　追加
                txtCODE.ppEnabled = True
                txtCNST_NM.ppEnabled = True
                cdbSUMSTART_D.ppEnabled = True
                txtNML_PRICE.ppEnabled = True
                txtHLDY_PRICE.ppEnabled = True
                txtNGHT_PRICE.ppEnabled = True
                ddlL_CLS.ppEnabled = True
                ddlM_CLS.ppEnabled = True
                txtSrchCODE.ppText = ""
                txtSrchCNST_NM.ppText = ""
                txtCODE.ppText = ""
                txtCNST_NM.ppText = ""
                cdbSUMSTART_D.ppText = ""
                txtNML_PRICE.ppText = ""
                txtHLDY_PRICE.ppText = ""
                txtNGHT_PRICE.ppText = ""
                ddlL_CLS.ppDropDownList.SelectedValue = ""
                ddlM_CLS.ppDropDownList.SelectedValue = ""
                Master.ppBtnInsert.Enabled = True
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnClear.Enabled = True
                Master.ppBtnDelete.Enabled = False
        End Select

        txtSrchCODE.ppEnabled = True
        txtSrchCNST_NM.ppEnabled = True

        ViewState("mstrDispMode") = mstrDispMode

        Master.ppBtnDelete.Visible = False

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
    ''' クリアボタン押下
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BtnClear_Click()

        'ログ出力開始
        psLogStart(Me)

        '★排他情報削除
        Me.Master.ppExclusiveDate = ClsSQL.pfExclDel(Me, DispCode, Me.Master.ppExclusiveDate)

        '選択したキー情報の削除
        ViewState("KeyName1") = ""
        ViewState("KeyName2") = ""
        ViewState("KeyName3") = ""
        ViewState("KeyName4") = ""
        ViewState("KeyName5") = ""

        '項目のクリア
        ClsSQL.ClearControls(Master.ppPlaceHolderMain)
        txtCODE.ppText = ""
        txtCNST_NM.ppText = ""
        cdbSUMSTART_D.ppText = ""
        txtNML_PRICE.ppText = ""
        txtHLDY_PRICE.ppText = ""
        txtNGHT_PRICE.ppText = ""
        ddlL_CLS.ppDropDownList.SelectedValue = ""
        ddlM_CLS.ppDropDownList.SelectedValue = ""

        ViewState("txtCODE") = ""
        ViewState("txtCNST_NM") = ""
        ViewState("cdbSUMSTART_D") = ""
        ViewState("txtNML_PRICE") = ""
        ViewState("txtHLDY_PRICE") = ""
        ViewState("txtNGHT_PRICE") = ""
        ViewState("ddlL_CLS") = ""
        ViewState("ddlM_CLS") = ""

        'コントロールの制御
        setControl("Default")
        Master.ppMainEnabled = True
        mstrDispMode = "DEF"
        '        Master.ppBtnUpdate.Enabled = False

        'ドロップダウンの初期化
        '        Me.ddlSrchScreen_ID.SelectedIndex = 0
        'ddlVer.Items.Item(0).Text = ""
        'ddlVer.Items.Item(0).Value = ""
        'ddlAPPAGroup.Items.Item(0).Text = ""
        'ddlAPPAGroup.Items.Item(0).Value = ""
        'ddlAPPACLS.Items.Item(0).Text = ""
        'ddlAPPACLS.Items.Item(0).Value = ""
        'ddlCNSTDET.Items.Item(0).Text = ""
        'ddlCNSTDET.Items.Item(0).Value = ""
        'ddlHDDCLS.Items.Item(0).Text = ""
        'ddlHDDCLS.Items.Item(0).Value = ""
        'ddlHDDNo.Items.Item(0).Text = ""
        'ddlHDDNo.Items.Item(0).Value = ""

        'フォーカス設定
        Me.txtCODE.ppTextBox.Focus()

        'ボタンの使用制限
        mstrDispMode = "DEF"

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 追加ボタン　押下
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BtnInsert_Click()

        'ログ出力開始
        psLogStart(Me)

        '       Me.Validate("Edit")
        If Me.IsValid = False Then
            Exit Sub
        End If

        Dim objSQLCmd As New SqlClient.SqlCommand
        Dim objDBTran As SqlClient.SqlTransaction = mclsDB.mobjDB.BeginTransaction
        Dim intRetCD As Integer = 0
        Dim strMesType As String = "00001"

        objSQLCmd.Connection = mclsDB.mobjDB
        objSQLCmd.CommandText = "COMUPDM48_I1"
        objSQLCmd.Parameters.Add("prmM48_CODE", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM48_CNST_NM", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM48_SUMSTART_D", SqlDbType.DateTime)
        objSQLCmd.Parameters.Add("prmM48_NML_PRICE", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM48_HLDY_PRICE", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM48_NGHT_PRICE", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM48_CLASS", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM48_INSERT_USR", SqlDbType.NVarChar)

        Dim strWKBuff As String = ""
        strWKBuff = ""
        If Me.txtCODE.ppText Is DBNull.Value Then
            strWKBuff = ""
        ElseIf Me.txtCODE.ppText.Trim = "" Then
            strWKBuff = ""
        Else
            strWKBuff = Me.txtCODE.ppText.Trim
        End If
        objSQLCmd.Parameters("prmM48_CODE").Value = strWKBuff

        strWKBuff = ""
        If Me.txtCNST_NM.ppText Is DBNull.Value Then
            strWKBuff = ""
        ElseIf Me.txtCNST_NM.ppText.Trim = "" Then
            strWKBuff = ""
        Else
            strWKBuff = Me.txtCNST_NM.ppText.Trim
        End If
        objSQLCmd.Parameters("prmM48_CNST_NM").Value = strWKBuff

        strWKBuff = ""
        If cdbSUMSTART_D.ppText Is DBNull.Value Then
            strWKBuff = ""
        ElseIf Me.cdbSUMSTART_D.ppText.Trim = "" Then
            strWKBuff = ""
        Else
            strWKBuff = ViewState("cdbSUMSTART_D")
        End If
        objSQLCmd.Parameters("prmM48_SUMSTART_D").Value = strWKBuff

        strWKBuff = ""
        If Me.txtNML_PRICE.ppText Is DBNull.Value Then
            strWKBuff = ""
        ElseIf Me.txtNML_PRICE.ppText.Trim = "" Then
            strWKBuff = ""
        Else
            strWKBuff = Me.txtNML_PRICE.ppText.Trim
        End If
        objSQLCmd.Parameters("prmM48_NML_PRICE").Value = strWKBuff

        strWKBuff = ""
        If Me.txtHLDY_PRICE.ppText Is DBNull.Value Then
            strWKBuff = ""
        ElseIf Me.txtHLDY_PRICE.ppText.Trim = "" Then
            strWKBuff = ""
        Else
            strWKBuff = Me.txtHLDY_PRICE.ppText.Trim
        End If
        objSQLCmd.Parameters("prmM48_HLDY_PRICE").Value = strWKBuff

        strWKBuff = ""
        If Me.txtNGHT_PRICE.ppText Is DBNull.Value Then
            strWKBuff = ""
        ElseIf Me.txtNGHT_PRICE.ppText.Trim = "" Then
            strWKBuff = ""
        Else
            strWKBuff = Me.txtNGHT_PRICE.ppText.Trim
        End If
        objSQLCmd.Parameters("prmM48_NGHT_PRICE").Value = strWKBuff

        strWKBuff = ""
        If ViewState("ddlL_CLS") Is Nothing Then
            strWKBuff = "9"
        Else
            strWKBuff = ViewState("ddlL_CLS").ToString.Substring(1)
        End If
        If ViewState("ddlM_CLS") Is Nothing Then
            strWKBuff += "99"
        Else
            strWKBuff += ViewState("ddlM_CLS").ToString
        End If
        objSQLCmd.Parameters("prmM48_CLASS").Value = strWKBuff

        'strWKBuff = ""
        'If Me.ChkEMGNCY_FLG.Checked = False Then
        '    strWKBuff = "0"
        'Else
        '    strWKBuff = "1"
        'End If
        'objSQLCmd.Parameters("prmM48_EMGNCY_FLG").Value = strWKBuff

        objSQLCmd.Parameters("prmM48_INSERT_USR").Value = User.Identity.Name

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
                mstrDispMode = "ADD"
                objSQLCmd.Transaction.Rollback()
                Exit Sub
            End If

            'コミット
            objSQLCmd.Transaction.Commit()

            Dim objwkDS As New DataSet
            objSQLCmd = Nothing
            objSQLCmd = New SqlClient.SqlCommand

            Try

                objSQLCmd.Connection = mclsDB.mobjDB
                objSQLCmd.CommandText = "COMUPDM48_S4"
                objSQLCmd.Parameters.Add("txtCODE", SqlDbType.NVarChar, 4)
                objSQLCmd.Parameters.Add("cdbSUMSTART_D", SqlDbType.DateTime)
                objSQLCmd.Parameters("txtCODE").Value = ViewState("txtCODE")
                objSQLCmd.Parameters("cdbSUMSTART_D").Value = ViewState("cdbSUMSTART_D")
                If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
                    'エラー
                    clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
                    objStack = New StackFrame
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "明細情報取得失敗", "Catch")
                    Exit Sub
                End If

                Me.grvList.DataSource = objwkDS.Tables(0)
                Me.grvList.DataBind()

            Catch ex As Exception
                'エラー
                clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
                objStack = New StackFrame
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
                mstrDispMode = "UPD"
                'トランザクションが残ってたらロールバック
                If objSQLCmd.Transaction Is Nothing Then
                Else
                    objSQLCmd.Transaction.Rollback()
                End If
                Exit Sub
            Finally
                Call mclsDB.psDisposeDataSet(objwkDS)
            End Try

        Catch ex As Exception

            '失敗メッセージ表示
            psMesBox(Me, strMesType, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
            mstrDispMode = "ADD"

        End Try

        'トランザクションが残ってたらロールバック
        If objSQLCmd.Transaction Is Nothing Then
        Else
            objSQLCmd.Transaction.Rollback()
        End If


        '完了メッセージ表示
        psMesBox(Me, strMesType, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName, MasterName)

        '        If ClsSQL.Btn_EditDBData(Me, "更新", MasterName, GetStr("更新")) = True Then
        '更新をグリッドに反映
        '            GetData_and_GridBind(GetStr("EditDataKey"))
        '項目の初期化
        BtnClear_Click()
        'ボタンの使用制限
        mstrDispMode = "DEF"
        'フォーカス設定
        Me.txtCODE.Focus()
        '        Else
        '            mstrDispMode = "UPD"
        '        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 更新ボタン　押下
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BtnUpdate_Click()

        'ログ出力開始
        psLogStart(Me)

        '        Me.Validate("Edit")
        If Me.IsValid = False Then
            Exit Sub
        End If

        Dim objSQLCmd As New SqlClient.SqlCommand
        Dim objDBTran As SqlClient.SqlTransaction = mclsDB.mobjDB.BeginTransaction
        Dim intRetCD As Integer = 0
        Dim strMesType As String = "00001"

        objSQLCmd.Connection = mclsDB.mobjDB
        objSQLCmd.CommandText = "COMUPDM48_U1"
        objSQLCmd.Parameters.Add("prmM48_CODE", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM48_CNST_NM", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM48_SUMSTART_D", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM48_NML_PRICE", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM48_HLDY_PRICE", SqlDbType.Decimal)
        objSQLCmd.Parameters.Add("prmM48_NGHT_PRICE", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM48_CLASS", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM48_UPDATE_USR", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM48_SUMSTART_D_OLD", SqlDbType.NVarChar)

        Dim strWKBuff As String = ""
        strWKBuff = ""
        If Me.txtCODE.ppText Is DBNull.Value Then
            strWKBuff = ""
        ElseIf Me.txtCODE.ppText.Trim = "" Then
            strWKBuff = ""
        Else
            strWKBuff = Me.txtCODE.ppText.Trim
        End If
        objSQLCmd.Parameters("prmM48_CODE").Value = strWKBuff

        strWKBuff = ""
        If Me.txtCNST_NM.ppText Is DBNull.Value Then
            strWKBuff = ""
        ElseIf Me.txtCNST_NM.ppText.Trim = "" Then
            strWKBuff = ""
        Else
            strWKBuff = Me.txtCNST_NM.ppText.Trim
        End If
        objSQLCmd.Parameters("prmM48_CNST_NM").Value = strWKBuff

        strWKBuff = ""
        If Me.cdbSUMSTART_D.ppText Is DBNull.Value Then
            strWKBuff = ""
        ElseIf Me.cdbSUMSTART_D.ppText.Trim = "" Then
            strWKBuff = ""
        Else
            strWKBuff = Me.cdbSUMSTART_D.ppText.Trim
        End If
        objSQLCmd.Parameters("prmM48_SUMSTART_D").Value = strWKBuff

        strWKBuff = ""
        If Me.txtNML_PRICE.ppText Is DBNull.Value Then
            strWKBuff = ""
        ElseIf Me.txtNML_PRICE.ppText.Trim = "" Then
            strWKBuff = ""
        Else
            strWKBuff = Me.txtNML_PRICE.ppText.Trim
        End If
        objSQLCmd.Parameters("prmM48_NML_PRICE").Value = strWKBuff

        strWKBuff = ""
        If Me.txtHLDY_PRICE.ppText Is DBNull.Value Then
            strWKBuff = ""
        ElseIf Me.txtHLDY_PRICE.ppText.Trim = "" Then
            strWKBuff = ""
        Else
            strWKBuff = Me.txtHLDY_PRICE.ppText.Trim
        End If
        objSQLCmd.Parameters("prmM48_HLDY_PRICE").Value = strWKBuff

        strWKBuff = ""
        If Me.txtNGHT_PRICE.ppText Is DBNull.Value Then
            strWKBuff = ""
        ElseIf Me.txtNGHT_PRICE.ppText.Trim = "" Then
            strWKBuff = ""
        Else
            strWKBuff = Me.txtNGHT_PRICE.ppText.Trim
        End If
        objSQLCmd.Parameters("prmM48_NGHT_PRICE").Value = strWKBuff

        strWKBuff = ""
        If ViewState("ddlL_CLS") Is Nothing Then
            strWKBuff = "9"
        Else
            strWKBuff = ViewState("ddlL_CLS").ToString.Substring(1)
        End If
        If ViewState("ddlM_CLS") Is Nothing Then
            strWKBuff += "99"
        Else
            strWKBuff += ViewState("ddlM_CLS")
        End If
        objSQLCmd.Parameters("prmM48_CLASS").Value = strWKBuff

        objSQLCmd.Parameters("prmM48_UPDATE_USR").Value = User.Identity.Name


        'ViewState("cdbSUMSTART_D_OLD")

        strWKBuff = ""
        If ViewState("cdbSUMSTART_D_OLD") Is Nothing Then
            strWKBuff = ""
        ElseIf ViewState("cdbSUMSTART_D_OLD") = "" Then
            strWKBuff = ""
        Else
            strWKBuff = ViewState("cdbSUMSTART_D_OLD")
        End If
        objSQLCmd.Parameters("prmM48_SUMSTART_D_OLD").Value = strWKBuff

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
                mstrDispMode = "UPD"
                objSQLCmd.Transaction.Rollback()
                Exit Sub
            End If

            'コミット
            objSQLCmd.Transaction.Commit()

            Dim objwkDS As New DataSet
            objSQLCmd = Nothing
            objSQLCmd = New SqlClient.SqlCommand

            Try

                objSQLCmd.Connection = mclsDB.mobjDB
                objSQLCmd.CommandText = "COMUPDM48_S4"
                objSQLCmd.Parameters.Add("txtCODE", SqlDbType.NVarChar, 4)
                objSQLCmd.Parameters.Add("cdbSUMSTART_D", SqlDbType.DateTime)
                objSQLCmd.Parameters("txtCODE").Value = ViewState("txtCODE")
                objSQLCmd.Parameters("cdbSUMSTART_D").Value = ViewState("cdbSUMSTART_D")
                If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
                    'エラー
                    clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
                    objStack = New StackFrame
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "明細情報取得失敗", "Catch")
                    Exit Sub
                End If

                Me.grvList.DataSource = objwkDS.Tables(0)
                Me.grvList.DataBind()

            Catch ex As Exception
                'エラー
                clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
                objStack = New StackFrame
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
                mstrDispMode = "UPD"
                'トランザクションが残ってたらロールバック
                If objSQLCmd.Transaction Is Nothing Then
                Else
                    objSQLCmd.Transaction.Rollback()
                End If
                Exit Sub
            Finally
                Call mclsDB.psDisposeDataSet(objwkDS)
            End Try

        Catch ex As Exception

            '失敗メッセージ表示
            psMesBox(Me, strMesType, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
            mstrDispMode = "UPD"

        End Try

        'トランザクションが残ってたらロールバック
        If objSQLCmd.Transaction Is Nothing Then
        Else
            objSQLCmd.Transaction.Rollback()
        End If


        '完了メッセージ表示
        psMesBox(Me, strMesType, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName, MasterName)

        '        If ClsSQL.Btn_EditDBData(Me, "更新", MasterName, GetStr("更新")) = True Then
        '更新をグリッドに反映
        '            GetData_and_GridBind(GetStr("EditDataKey"))
        '項目の初期化
        BtnClear_Click()
        'ボタンの使用制限
        mstrDispMode = "DEF"
        'フォーカス設定
        Me.txtCODE.Focus()
        '        Else
        '            mstrDispMode = "UPD"
        '        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 削除ボタン　押下
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BtnDelete_Click()

        'ログ出力開始
        psLogStart(Me)

        Select Case intDelCls
            Case 0
                'ログ出力終了
                psLogEnd(Me)
                Exit Select
        End Select

        Dim objSQLCmd As New SqlClient.SqlCommand
        Dim objDBTran As SqlClient.SqlTransaction = mclsDB.mobjDB.BeginTransaction
        Dim intRetCD As Integer = 0
        Dim strMesType As String = "00001"

        Select Case intDelCls
            Case 0

                objSQLCmd.Connection = mclsDB.mobjDB
                objSQLCmd.CommandText = "COMUPDM48_D1"
                objSQLCmd.Parameters.Add("prmM48_CODE", SqlDbType.NVarChar)
                objSQLCmd.Parameters.Add("prmM48_SUMSTART_D", SqlDbType.DateTime)

                Dim strWKBuff As String = ""
                strWKBuff = ""
                If Me.txtCODE.ppText Is DBNull.Value Then
                    strWKBuff = ""
                ElseIf Me.txtCODE.ppText.Trim = "" Then
                    strWKBuff = ""
                Else
                    strWKBuff = Me.txtCODE.ppText.Trim
                End If
                objSQLCmd.Parameters("prmM48_CODE").Value = strWKBuff

                strWKBuff = ""
                If Me.cdbSUMSTART_D.ppText Is DBNull.Value Then
                    strWKBuff = ""
                ElseIf Me.cdbSUMSTART_D.ppText.Trim = "" Then
                    strWKBuff = ""
                Else
                    strWKBuff = Me.cdbSUMSTART_D.ppText.Trim
                End If
                objSQLCmd.Parameters("prmM48_SUMSTART_D").Value = strWKBuff

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
                        mstrDispMode = "UPD"
                        objSQLCmd.Transaction.Rollback()
                        Exit Sub
                    End If

                    'コミット
                    objSQLCmd.Transaction.Commit()

                    Dim objwkDS As New DataSet
                    objSQLCmd = Nothing
                    objSQLCmd = New SqlClient.SqlCommand

                    Try

                        objSQLCmd.Connection = mclsDB.mobjDB
                        objSQLCmd.CommandText = "COMUPDM48_S4"
                        objSQLCmd.Parameters.Add("txtCODE", SqlDbType.NVarChar, 4)
                        objSQLCmd.Parameters.Add("cdbSUMSTART_D", SqlDbType.DateTime)
                        objSQLCmd.Parameters("txtCODE").Value = ViewState("txtCODE")
                        objSQLCmd.Parameters("cdbSUMSTART_D").Value = ViewState("cdbSUMSTART_D")
                        If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
                            'エラー
                            clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
                            objStack = New StackFrame
                            'ログ出力
                            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "明細情報取得失敗", "Catch")
                            Exit Sub
                        End If

                        Me.grvList.DataSource = objwkDS.Tables(0)
                        Me.grvList.DataBind()

                    Catch ex As Exception
                        'エラー
                        clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
                        objStack = New StackFrame
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
                        mstrDispMode = "UPD"
                        'トランザクションが残ってたらロールバック
                        If objSQLCmd.Transaction Is Nothing Then
                        Else
                            objSQLCmd.Transaction.Rollback()
                        End If
                        Exit Sub
                    Finally
                        Call mclsDB.psDisposeDataSet(objwkDS)
                    End Try

                Catch ex As Exception

                    '失敗メッセージ表示
                    psMesBox(Me, strMesType, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                    mstrDispMode = "UPD"

                End Try

                'データバインド
                Master.ppCount = "0"
                grvList.DataSource = New DataTable
                grvList.DataBind()

                '項目の初期化
                BtnClear_Click()

            Case Else

                Select Case Master.ppBtnDelete.Text
                    Case "削除"

                        objSQLCmd.Connection = mclsDB.mobjDB
                        objSQLCmd.CommandText = "COMUPDM48_D1"
                        objSQLCmd.Parameters.Add("prmM48_CODE", SqlDbType.NVarChar)
                        objSQLCmd.Parameters.Add("prmM48_SUMSTART_D", SqlDbType.DateTime)

                        Dim strWKBuff As String = ""
                        strWKBuff = ""
                        If Me.txtCODE.ppText Is DBNull.Value Then
                            strWKBuff = ""
                        ElseIf Me.txtCODE.ppText.Trim = "" Then
                            strWKBuff = ""
                        Else
                            strWKBuff = Me.txtCODE.ppText.Trim
                        End If
                        objSQLCmd.Parameters("prmM48_CODE").Value = strWKBuff

                        strWKBuff = ""
                        If Me.cdbSUMSTART_D.ppText Is DBNull.Value Then
                            strWKBuff = ""
                        ElseIf Me.cdbSUMSTART_D.ppText.Trim = "" Then
                            strWKBuff = ""
                        Else
                            strWKBuff = Me.cdbSUMSTART_D.ppText.Trim
                        End If
                        objSQLCmd.Parameters("prmM48_SUMSTART_D").Value = strWKBuff

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
                                mstrDispMode = "UPD"
                                objSQLCmd.Transaction.Rollback()
                                Exit Sub
                            End If

                            'コミット
                            objSQLCmd.Transaction.Commit()

                            Dim objwkDS As New DataSet
                            objSQLCmd = Nothing
                            objSQLCmd = New SqlClient.SqlCommand

                            Try

                                objSQLCmd.Connection = mclsDB.mobjDB
                                objSQLCmd.CommandText = "COMUPDM48_S4"
                                objSQLCmd.Parameters.Add("txtCODE", SqlDbType.NVarChar, 4)
                                objSQLCmd.Parameters.Add("cdbSUMSTART_D", SqlDbType.DateTime)
                                objSQLCmd.Parameters("txtCODE").Value = ViewState("txtCODE")
                                objSQLCmd.Parameters("cdbSUMSTART_D").Value = ViewState("cdbSUMSTART_D")
                                If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
                                    'エラー
                                    clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
                                    objStack = New StackFrame
                                    'ログ出力
                                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "明細情報取得失敗", "Catch")
                                    Exit Sub
                                End If

                                Me.grvList.DataSource = objwkDS.Tables(0)
                                Me.grvList.DataBind()

                            Catch ex As Exception
                                'エラー
                                clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
                                objStack = New StackFrame
                                'ログ出力
                                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
                                mstrDispMode = "UPD"
                                'トランザクションが残ってたらロールバック
                                If objSQLCmd.Transaction Is Nothing Then
                                Else
                                    objSQLCmd.Transaction.Rollback()
                                End If
                                Exit Sub
                            Finally
                                Call mclsDB.psDisposeDataSet(objwkDS)
                            End Try

                        Catch ex As Exception

                            '失敗メッセージ表示
                            psMesBox(Me, strMesType, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                            mstrDispMode = "UPD"

                        End Try

                        'データバインド
                        Master.ppCount = "0"
                        grvList.DataSource = New DataTable
                        grvList.DataBind()

                        '項目の初期化
                        BtnClear_Click()

                    Case Else
                        '削除取消
                        'stb.Clear()
                        'stb.Append("UPDATE " & TableName & " SET ")
                        'stb.Append("" & TableName.Substring(0, 4) & "DELETE_FLG = 0, ")
                        'stb.Append("" & TableName.Substring(0, 4) & "UPDATE_DT = '" & DateTime.Now & "', ")
                        'stb.Append("" & TableName.Substring(0, 4) & "UPDATE_USR = '" & User.Identity.Name & "' ")
                        'stb.Append(" WHERE  ")
                        'stb.Append(ClsSQL.GeneStrSearch(KeyName1, ViewState("KeyName1"), "完全", ""))
                        'stb.Append(ClsSQL.GeneStrSearch(KeyName2, ViewState("KeyName2"), "完全"))
                        'stb.Append(ClsSQL.GeneStrSearch(KeyName3, ViewState("KeyName3"), "完全"))
                        'stb.Append(ClsSQL.GeneStrSearch(KeyName4, ViewState("KeyName4"), "完全"))
                        'stb.Append(ClsSQL.GeneStrSearch(KeyName5, ViewState("KeyName5"), "完全"))

                        '                        If ClsSQL.Btn_EditDBData(Me, "更新", MasterName, stb.ToString) = True Then
                        If ClsSQL.Btn_EditDBData(Me, "更新", MasterName, "") = True Then
                            If ViewState("strWHERE") Is Nothing Then
                                '                                GetData_and_GridBind(GetStr("EditDataKey"))
                                GetData_and_GridBind()
                            Else
                                '更新をグリッドに反映
                                'GetData_and_GridBind(DirectCast(ViewState("strWHERE"), String))
                                '                                GetData_and_GridBind(GetStr("EditDataKey"))
                                GetData_and_GridBind()
                            End If

                            '項目の初期化
                            BtnClear_Click()
                        End If
                End Select
        End Select

        '完了メッセージ表示
        psMesBox(Me, strMesType, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName, MasterName)

        'フォーカス設定
        txtCODE.Focus()

        'ログ出力終了
        psLogEnd(Me)

    End Sub
    ''' <summary>
    '''検索条件クリア
    ''' </summary>
    Sub BtnSearchClear_Click()

        'ログ出力開始
        psLogStart(Me)

        '項目のクリア
        ClsSQL.ClearControls(Me.Master.ppPlaceHolderSearch)
        Master.ppchksDel.Checked = False
        'コントロールの活性制御
        setControl("Search")

        ViewState(cstSrchKey_CNSTCLS) = txtSrchCODE.ppText
        ViewState(cstSrchKey_TBOXCLS) = txtSrchCNST_NM.ppText

        'フォーカス設定
        Me.txtSrchCODE.Focus()

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    '''検索ボタン
    ''' </summary>
    Sub BtnSearch_Click()

        If Me.IsValid = False Then
            Exit Sub
        End If

        'ログ出力開始
        psLogStart(Me)

        'コンテナ内を検索し値を収集
        '        stb.Clear()
        '        For Each objCtrl As Control In Master.ppPlaceHolderSearch.Controls
        '            Select Case objCtrl.GetType.Name
        '                Case "DropDownList"
        '                    stb.Append(DirectCast(objCtrl, DropDownList).SelectedValue)
        '                Case "TextBox"
        '                    stb.Append(DirectCast(objCtrl, TextBox).Text)
        '                Case "CheckBox"
        '                    stb.Append(DirectCast(objCtrl, CheckBox).Checked)
        '            End Select
        '        Next

        '        If stb.ToString = "" AndAlso Master.ppchksDel.Checked = False Then
        '            '検索条件初期化
        '            mstrDispMode = "Clear"

        '            '結果をグリッドに反映
        '            GetData_and_GridBind()

        '            'ログ出力終了
        '            psLogEnd(Me)

        '        Else
        '結果をグリッドに反映
        '            GetData_and_GridBind(GetStr("検索"))
        GetData_and_GridBind()
        '        End If

        '検索条件保存
        'Me.ViewState.Add("strWHERE", GetStr("検索"))
        '        Me.ViewState.Add("SrchDelFlg", ClsSQL.GetDltCheckNum(Master.ppchksDel))

        'フォーカス設定
        Me.txtSrchCODE.Focus()

        'ログ出力終了
        psLogEnd(Me)

    End Sub
#End Region

    '----------------------------------------------------------------------------------------------------------------------------
    '-　ボタン以外のコントロール制御
    '----------------------------------------------------------------------------------------------------------------------------
#Region "ボタン以外のコントロール制御"

    ''' <summary>
    ''' コード変更後
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtCODE_TextChanged(sender As Object, e As EventArgs)

        Dim objSQLCmd As New SqlClient.SqlCommand
        Dim objwkDS As New DataSet

        objSQLCmd.Connection = mclsDB.mobjDB
        objSQLCmd.CommandText = "COMUPDM48_S5"
        objSQLCmd.Parameters.Add("txtCODE", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("cdbSUMSTART_D", SqlDbType.DateTime)
        objSQLCmd.Parameters("txtCODE").Value = ViewState("txtCODE")
        If ViewState("cdbSUMSTART_D") = "" Then
            objSQLCmd.Parameters("cdbSUMSTART_D").Value = DBNull.Value
        Else
            objSQLCmd.Parameters("cdbSUMSTART_D").Value = ViewState("cdbSUMSTART_D")
        End If
        '        objSQLCmd.Parameters("cdbSUMSTART_D").Value = ViewState("cdbSUMSTART_D")
        If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
            'エラー
            clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "既存情報取得")
            objStack = New StackFrame
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "リスト情報取得失敗", "Catch")
            Exit Sub
        End If

        If objwkDS.Tables.Count > 0 Then
            If objwkDS.Tables.Count > 0 Then
                If objwkDS.Tables(0).Rows.Count = 1 Then
                    Me.txtCNST_NM.ppText = objwkDS.Tables(0).Rows(0).Item("CM48_CNST_NM").ToString
                    Me.cdbSUMSTART_D.ppText = objwkDS.Tables(0).Rows(0).Item("CM48_SUMSTART_D").ToString
                    Me.ddlL_CLS.ppDropDownList.SelectedValue = objwkDS.Tables(0).Rows(0).Item("CM48_L_CLS").ToString
                    Me.ddlM_CLS.ppDropDownList.SelectedValue = objwkDS.Tables(0).Rows(0).Item("CM48_M_CLS").ToString
                    If objwkDS.Tables(0).Rows(0).Item("CM48_NML_PRICE") Is DBNull.Value Then
                        Me.txtNML_PRICE.ppText = cstSpace7
                    Else
                        Call sAmount_Format(objwkDS.Tables(0).Rows(0).Item("CM48_NML_PRICE").ToString, txtNML_PRICE.ppTextBox)
                    End If
                    If objwkDS.Tables(0).Rows(0).Item("CM48_HLDY_PRICE") Is DBNull.Value Then
                        Me.txtHLDY_PRICE.ppText = cstSpace7
                    Else
                        Call sAmount_Format(objwkDS.Tables(0).Rows(0).Item("CM48_HLDY_PRICE").ToString, txtHLDY_PRICE.ppTextBox)
                    End If
                    If objwkDS.Tables(0).Rows(0).Item("CM48_NGHT_PRICE") Is DBNull.Value Then
                        Me.txtNGHT_PRICE.ppText = cstSpace7
                    Else
                        Call sAmount_Format(objwkDS.Tables(0).Rows(0).Item("CM48_NGHT_PRICE").ToString, txtNGHT_PRICE.ppTextBox)
                    End If
                    ViewState("txtCODE") = objwkDS.Tables(0).Rows(0).Item("CM48_CODE").ToString
                    ViewState("txtCNST_NM") = objwkDS.Tables(0).Rows(0).Item("CM48_CNST_NM").ToString
                    ViewState("cdbSUMSTART_D") = objwkDS.Tables(0).Rows(0).Item("CM48_SUMSTART_D").ToString
                    ViewState("ddlL_CLS") = objwkDS.Tables(0).Rows(0).Item("CM48_L_CLS").ToString
                    ViewState("ddlM_CLS") = objwkDS.Tables(0).Rows(0).Item("CM48_M_CLS").ToString
                    ViewState("txtNML_PRICE") = Me.txtNML_PRICE.ppText
                    ViewState("txtHLDY_PRICE") = Me.txtHLDY_PRICE.ppText
                    ViewState("txtNGHT_PRICE") = Me.txtNGHT_PRICE.ppText
                    mstrDispMode = "UPD"
                    Me.txtCNST_NM.Focus()
                End If
            End If
        End If

        Call mclsDB.psDisposeDataSet(objwkDS)

        'いきなりコードを入力されたら追加モードに設定
        If mstrDispMode Is Nothing Then
            If Me.txtCODE.ppText.Trim = "" Then
            Else
                mstrDispMode = "ADD"
                Me.txtCNST_NM.Focus()
            End If
        ElseIf mstrDispMode = "First" Then
            If Me.txtCODE.ppText.Trim = "" Then
            Else
                mstrDispMode = "ADD"
                Me.txtCNST_NM.Focus()
            End If
        End If

    End Sub

    '    ''' <summary>
    '    ''' 工事名　変更後
    '    ''' </summary>
    '    ''' <param name="sender"></param>
    '    ''' <param name="e"></param>
    '    ''' <remarks></remarks>
    '    Private Sub txtCNST_NM_TextChanged(sender As Object, e As EventArgs) Handles txtCNST_NM.TextChanged

    '        Dim objSQLCmd As New SqlClient.SqlCommand
    '        Dim objwkDS As New DataSet

    '        objSQLCmd.Connection = mclsDB.mobjDB
    '        objSQLCmd.CommandText = "COMUPDM48_S6"
    '        objSQLCmd.Parameters.Add("txtCNST_NM", SqlDbType.NVarChar)
    '        objSQLCmd.Parameters.Add("cdbSUMSTART_D", SqlDbType.DateTime)
    '        objSQLCmd.Parameters("txtCNST_NM").Value = ViewState("txtCNST_NM")
    '        If ViewState("cdbSUMSTART_D") = "" Then
    '            objSQLCmd.Parameters("cdbSUMSTART_D").Value = DBNull.Value
    '        Else
    '            objSQLCmd.Parameters("cdbSUMSTART_D").Value = ViewState("cdbSUMSTART_D")
    '        End If
    ''        objSQLCmd.Parameters("cdbSUMSTART_D").Value = ViewState("cdbSUMSTART_D")
    '        If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
    '            'エラー
    '            clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "リスト作成")
    '            objStack = New StackFrame
    '            'ログ出力
    '            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "リスト情報取得失敗", "Catch")
    '            Exit Sub
    '        End If

    '        If objwkDS.Tables.Count > 0 Then
    '            If objwkDS.Tables.Count > 0 Then
    '                If objwkDS.Tables(0).Rows.Count = 1 Then
    '                    Me.txtCNST_NM.Text = objwkDS.Tables(0).Rows(0).Item("CM48_CNST_NM").ToString
    '                    Me.cdbSUMSTART_D.ppText = objwkDS.Tables(0).Rows(0).Item("CM48_SUMSTART_D").ToString
    '                    Me.ddlL_CLS.SelectedValue = objwkDS.Tables(0).Rows(0).Item("CM48_L_CLS").ToString
    '                    Me.ddlM_CLS.SelectedValue = objwkDS.Tables(0).Rows(0).Item("CM48_M_CLS").ToString
    '                    If objwkDS.Tables(0).Rows(0).Item("CM48_NML_PRICE") Is DBNull.Value Then
    '                        Me.txtNML_PRICE.Text = cstSpace7
    '                    Else
    '                        Call sAmount_Format(objwkDS.Tables(0).Rows(0).Item("CM48_NML_PRICE").ToString, txtNML_PRICE)
    '                    End If
    '                    If objwkDS.Tables(0).Rows(0).Item("CM48_HLDY_PRICE") Is DBNull.Value Then
    '                        Me.txtHLDY_PRICE.Text = cstSpace7
    '                    Else
    '                        Call sAmount_Format(objwkDS.Tables(0).Rows(0).Item("CM48_HLDY_PRICE").ToString, txtHLDY_PRICE)
    '                    End If
    '                    If objwkDS.Tables(0).Rows(0).Item("CM48_NGHT_PRICE") Is DBNull.Value Then
    '                        Me.txtNGHT_PRICE.Text = cstSpace7
    '                    Else
    '                        Call sAmount_Format(objwkDS.Tables(0).Rows(0).Item("CM48_NGHT_PRICE").ToString, txtNGHT_PRICE)
    '                    End If
    '                    ViewState("txtCODE") = objwkDS.Tables(0).Rows(0).Item("CM48_CODE").ToString
    '                    ViewState("txtCNST_NM") = objwkDS.Tables(0).Rows(0).Item("CM48_CNST_NM").ToString
    '                    ViewState("cdbSUMSTART_D") = objwkDS.Tables(0).Rows(0).Item("CM48_SUMSTART_D").ToString
    '                    ViewState("ddlL_CLS") = objwkDS.Tables(0).Rows(0).Item("CM48_L_CLS").ToString
    '                    ViewState("ddlM_CLS") = objwkDS.Tables(0).Rows(0).Item("CM48_M_CLS").ToString
    '                    ViewState("txtNML_PRICE") = Me.txtNML_PRICE.Text
    '                    ViewState("txtHLDY_PRICE") = Me.txtHLDY_PRICE.Text
    '                    ViewState("txtNGHT_PRICE") = Me.txtNGHT_PRICE.Text
    '                    mstrDispMode = "SEL"
    '               End If
    '            End If
    '        End If

    '        Call mclsDB.psDisposeDataSet(objwkDS)

    '        'いきなり日付を入力されたら追加モードに設定
    '        If mstrDispMode Is Nothing Then
    '            If Me.txtCNST_NM.Text.Trim = "" Then
    '            Else
    '                mstrDispMode = "ADD"
    '            End If
    '        End If

    '    End Sub

    ''' <summary>
    ''' 日付テキストボックス　変更後
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub cdbSUMSTART_D_TextChanged()

        Dim objSQLCmd As New SqlClient.SqlCommand
        Dim objwkDS As New DataSet

        objSQLCmd.Connection = mclsDB.mobjDB
        objSQLCmd.CommandText = "COMUPDM48_S5"
        objSQLCmd.Parameters.Add("txtCODE", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("cdbSUMSTART_D", SqlDbType.DateTime)
        objSQLCmd.Parameters("txtCODE").Value = ViewState("txtCODE")
        If ViewState("cdbSUMSTART_D") = "" Then
            objSQLCmd.Parameters("cdbSUMSTART_D").Value = DBNull.Value
        Else
            objSQLCmd.Parameters("cdbSUMSTART_D").Value = ViewState("cdbSUMSTART_D")
        End If
        '        objSQLCmd.Parameters("cdbSUMSTART_D").Value = ViewState("cdbSUMSTART_D")
        If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
            'エラー
            clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "リスト作成")
            objStack = New StackFrame
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "リスト情報取得失敗", "Catch")
            Exit Sub
        End If

        If objwkDS.Tables.Count > 0 Then
            If objwkDS.Tables.Count > 0 Then
                If objwkDS.Tables(0).Rows.Count = 1 Then
                    Me.txtCNST_NM.ppText = objwkDS.Tables(0).Rows(0).Item("CM48_CNST_NM").ToString
                    Me.cdbSUMSTART_D.ppText = objwkDS.Tables(0).Rows(0).Item("CM48_SUMSTART_D").ToString
                    Me.ddlL_CLS.ppDropDownList.SelectedValue = objwkDS.Tables(0).Rows(0).Item("CM48_L_CLS").ToString
                    Me.ddlM_CLS.ppDropDownList.SelectedValue = objwkDS.Tables(0).Rows(0).Item("CM48_M_CLS").ToString
                    If objwkDS.Tables(0).Rows(0).Item("CM48_NML_PRICE") Is DBNull.Value Then
                        Me.txtNML_PRICE.ppText = cstSpace7
                    Else
                        Call sAmount_Format(objwkDS.Tables(0).Rows(0).Item("CM48_NML_PRICE").ToString, txtNML_PRICE.ppTextBox)
                    End If
                    If objwkDS.Tables(0).Rows(0).Item("CM48_HLDY_PRICE") Is DBNull.Value Then
                        Me.txtHLDY_PRICE.ppText = cstSpace7
                    Else
                        Call sAmount_Format(objwkDS.Tables(0).Rows(0).Item("CM48_HLDY_PRICE").ToString, txtHLDY_PRICE.ppTextBox)
                    End If
                    If objwkDS.Tables(0).Rows(0).Item("CM48_NGHT_PRICE") Is DBNull.Value Then
                        Me.txtNGHT_PRICE.ppText = cstSpace7
                    Else
                        Call sAmount_Format(objwkDS.Tables(0).Rows(0).Item("CM48_NGHT_PRICE").ToString, txtNGHT_PRICE.ppTextBox)
                    End If
                    ViewState("txtCODE") = objwkDS.Tables(0).Rows(0).Item("CM48_CODE").ToString
                    ViewState("txtCNST_NM") = objwkDS.Tables(0).Rows(0).Item("CM48_CNST_NM").ToString
                    ViewState("cdbSUMSTART_D") = objwkDS.Tables(0).Rows(0).Item("CM48_SUMSTART_D").ToString
                    ViewState("ddlL_CLS") = objwkDS.Tables(0).Rows(0).Item("CM48_L_CLS").ToString
                    ViewState("ddlM_CLS") = objwkDS.Tables(0).Rows(0).Item("CM48_M_CLS").ToString
                    ViewState("txtNML_PRICE") = Me.txtNML_PRICE.ppText
                    ViewState("txtHLDY_PRICE") = Me.txtHLDY_PRICE.ppText
                    ViewState("txtNGHT_PRICE") = Me.txtNGHT_PRICE.ppText
                    mstrDispMode = "UPD"
                End If
            End If
        End If

        Call mclsDB.psDisposeDataSet(objwkDS)

        'いきなり日付を入力されたら追加モードに設定
        If mstrDispMode Is Nothing Then
            If Me.cdbSUMSTART_D.ppText.Trim = "" Then
            Else
                mstrDispMode = "ADD"
            End If
        End If

    End Sub

    ''' <summary>
    ''' 料金大区分　変更後
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlL_CLS_TextChanged(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        Call ddlM_CLS_DataBind()

        Me.ddlM_CLS.ppDropDownList.SelectedValue = ""
        ViewState("ddlM_CLS") = ""

        'いきなり項目選択をされたら追加モードに設定
        If mstrDispMode Is Nothing Then
            If ViewState("ddlL_CLS") = "" Then
            Else
                mstrDispMode = "ADD"
            End If
        Else
            If ViewState("ddlL_CLS") = "" Then
            Else
                mstrDispMode = "ADD"
            End If
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

#End Region

    '----------------------------------------------------------------------------------------------------------------------------
    '-　グリッドビュー制御
    '----------------------------------------------------------------------------------------------------------------------------
#Region "グリッドビュー制御"

    ''' <summary>
    ''' 明細選択およびヘッダー押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_RowCommand(sender As Object, e As CommandEventArgs) Handles grvList.RowCommand

        'ログ出力開始
        psLogStart(Me)

        Select Case e.CommandName
            Case "Sort"   'ヘッダー押下でソート

                '４桁以上の数字文字列でカンマ表示しない時使用 grvList_DataBoundのコメントアウトも解除

                ''順番号の頭に０付加
                'Dim intTRUN As Int16
                'For Each rowData As GridViewRow In grvList.Rows
                '    intTRUN = Int16.Parse(CType(rowData.FindControl("順番号"), TextBox).Text)
                '    CType(rowData.FindControl("順番号"), TextBox).Text = intTRUN.ToString("000")
                'Next

                Exit Sub
            Case "Select"  '選択

                Dim objwkDS As New DataSet
                Dim objSQLCmd As New SqlClient.SqlCommand
                Dim rowData As GridViewRow = grvList.Rows(Convert.ToInt32(e.CommandArgument))

                Try

                    objSQLCmd.Connection = mclsDB.mobjDB
                    objSQLCmd.CommandText = "COMUPDM48_S4"
                    objSQLCmd.Parameters.Add("txtCODE", SqlDbType.NVarChar, 4)
                    objSQLCmd.Parameters.Add("cdbSUMSTART_D", SqlDbType.DateTime)
                    objSQLCmd.Parameters("txtCODE").Value = CType(rowData.FindControl("CM48_CODE"), TextBox).Text
                    objSQLCmd.Parameters("cdbSUMSTART_D").Value = CType(rowData.FindControl("CM48_SUMSTART_D"), TextBox).Text
                    If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
                        'エラー
                        clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
                        objStack = New StackFrame
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "明細情報取得失敗", "Catch")
                        Exit Sub
                    End If

                    Dim arKey As String() = {KeyName1, KeyName2, KeyName3, KeyName4, KeyName5}
                    Dim strKeyValue As String() = {"", "", "", "", ""}
                    '取得データの存在確認
                    If objwkDS.Tables.Count > 0 Then
                        If objwkDS.Tables(0).Rows.Count > 0 Then
                            '選択したデータのキーの登録&退避
                            For i As Integer = 0 To arKey.Count - 1
                                If arKey(i) = "" Then
                                    Exit For
                                Else
                                    Me.ViewState.Add("KeyName" & i + 1 & "", objwkDS.Tables(0).Rows(0).Item("C" & arKey(i)).ToString)
                                    strKeyValue(i) = objwkDS.Tables(0).Rows(0).Item("C" & arKey(i)).ToString
                                End If
                            Next
                        Else
                            'エラー
                            clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
                            objStack = New StackFrame
                            'ログ出力
                            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "明細情報取得失敗", "Catch")
                            Exit Sub
                        End If
                    Else
                        'エラー
                        clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
                        objStack = New StackFrame
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "明細情報取得失敗", "Catch")
                        Exit Sub
                    End If

                    '排他制御処理
                    If fExclusive(strKeyValue) = False Then
                        'clsMst.psMesBox(Me, "30010", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)
                        Exit Sub
                    End If

                    '編集エリアに値を反映
                    setControl("UPD")

                    Me.txtCODE.ppText = objwkDS.Tables(0).Rows(0).Item("CM48_CODE").ToString
                    Me.txtCNST_NM.ppText = objwkDS.Tables(0).Rows(0).Item("CM48_CNST_NM").ToString
                    Me.cdbSUMSTART_D.ppText = objwkDS.Tables(0).Rows(0).Item("CM48_SUMSTART_D").ToString
                    Me.ddlL_CLS.ppDropDownList.SelectedValue = "0" & objwkDS.Tables(0).Rows(0).Item("CM48_CLASS").ToString.Substring(0, 1)
                    Me.ddlM_CLS.ppDropDownList.SelectedValue = objwkDS.Tables(0).Rows(0).Item("CM48_CLASS").ToString.Substring(1, 2)
                    If objwkDS.Tables(0).Rows(0).Item("CM48_NML_PRICE") Is DBNull.Value Then
                        Me.txtNML_PRICE.ppText = ""
                    Else
                        txtNML_PRICE.ppText = objwkDS.Tables(0).Rows(0).Item("CM48_NML_PRICE").ToString
                    End If
                    If objwkDS.Tables(0).Rows(0).Item("CM48_HLDY_PRICE") Is DBNull.Value Then
                        Me.txtHLDY_PRICE.ppText = ""
                    Else
                        txtHLDY_PRICE.ppText = objwkDS.Tables(0).Rows(0).Item("CM48_HLDY_PRICE").ToString
                    End If
                    If objwkDS.Tables(0).Rows(0).Item("CM48_NGHT_PRICE") Is DBNull.Value Then
                        Me.txtNGHT_PRICE.ppText = ""
                    Else
                        txtNGHT_PRICE.ppText = objwkDS.Tables(0).Rows(0).Item("CM48_NGHT_PRICE").ToString
                    End If

                    ViewState("txtCODE") = objwkDS.Tables(0).Rows(0).Item("CM48_CODE").ToString
                    ViewState("txtCNST_NM") = objwkDS.Tables(0).Rows(0).Item("CM48_CNST_NM").ToString
                    ViewState("cdbSUMSTART_D") = objwkDS.Tables(0).Rows(0).Item("CM48_SUMSTART_D").ToString
                    '                    ViewState("txtNML_PRICE") = objwkDS.Tables(0).Rows(0).Item("CM48_NML_PRICE").ToString
                    '                    ViewState("txtHLDY_PRICE") = objwkDS.Tables(0).Rows(0).Item("CM48_HLDY_PRICE").ToString
                    '                    ViewState("txtNGHT_PRICE") = objwkDS.Tables(0).Rows(0).Item("CM48_NGHT_PRICE").ToString
                    ViewState("ddlL_CLS") = "0" & objwkDS.Tables(0).Rows(0).Item("CM48_CLASS").ToString.Substring(0, 1)
                    ViewState("ddlM_CLS") = objwkDS.Tables(0).Rows(0).Item("CM48_CLASS").ToString.Substring(1, 2)
                    ViewState("txtNML_PRICE") = Me.txtNML_PRICE.ppText.Trim
                    ViewState("txtHLDY_PRICE") = Me.txtHLDY_PRICE.ppText.Trim
                    ViewState("txtNGHT_PRICE") = Me.txtNGHT_PRICE.ppText.Trim
                    ViewState("cdbSUMSTART_D_OLD") = objwkDS.Tables(0).Rows(0).Item("CM48_SUMSTART_D").ToString

                    'ボタンの使用制限
                    mstrDispMode = "UPD"
                    '                    Select Case Master.ppBtnDelete.Text
                    '                        Case "削除"
                    '                            mstrDispMode = "UPD"
                    '                            'フォーカス設定
                    ''                            txtAPPANM.Focus()
                    '                        Case Else
                    '                            mstrDispMode = "DEL"
                    '                            'フォーカス設定
                    '                            Master.ppBtnClear.Focus()
                    '                    End Select

                    Me.txtCODE.Focus()

                Catch ex As Exception
                    'エラー
                    clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
                    objStack = New StackFrame
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Finally
                    Call mclsDB.psDisposeDataSet(objwkDS)
                End Try
        End Select

        'ログ出力終了
        psLogEnd(Me)
    End Sub

#Region "コメントアウト"
    '    ''' <summary>
    '    ''' GRID DataBound
    '    ''' </summary>
    '    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound
    'Dim strBuff As String = ""
    'For Each rowData As GridViewRow In grvList.Rows
    '    If rowData.RowType = DataControlRowType.DataRow Then
    '        Dim objwkTXT As TextBox
    '        Dim sglBuff As Single
    '        '通常料金のカンマ編集
    '        objwkTXT = Nothing
    '        objwkTXT = CType(rowData.FindControl("CM48_NML_PRICE"), TextBox)
    '        strBuff = objwkTXT.Text
    '        If Single.TryParse(strBuff, sglBuff) Then
    '            If sglBuff <> 0 Then
    '                objwkTXT.Text = Integer.Parse(sglBuff).ToString("#,##0")
    '            End If
    '            If Single.Parse(strBuff) = 0 Then
    '                objwkTXT.Text = "0"
    '            End If
    '        Else
    '            objwkTXT.Text = ""
    '        End If
    '        '休日料金のカンマ編集
    '        objwkTXT = Nothing
    '        objwkTXT = DirectCast(rowData.FindControl("CM48_HLDY_PRICE"), TextBox)
    '        strBuff = objwkTXT.Text
    '        If Single.TryParse(strBuff, sglBuff) Then
    '            If sglBuff <> 0 Then
    '                objwkTXT.Text = Integer.Parse(sglBuff).ToString("#,##0")
    '            End If
    '            If Single.Parse(strBuff) = 0 Then
    '                objwkTXT.Text = "0"
    '            End If
    '        Else
    '            objwkTXT.Text = ""
    '        End If
    '        '夜間料金のカンマ編集
    '        objwkTXT = Nothing
    '        objwkTXT = DirectCast(rowData.FindControl("CM48_NGHT_PRICE"), TextBox)
    '        strBuff = objwkTXT.Text
    '        If Single.TryParse(strBuff, sglBuff) Then
    '            If sglBuff <> 0 Then
    '                objwkTXT.Text = Integer.Parse(sglBuff).ToString("#,##0")
    '            End If
    '            If Single.Parse(strBuff) = 0 Then
    '                objwkTXT.Text = "0"
    '            End If
    '        Else
    '            objwkTXT.Text = ""
    '        End If
    '        Dim objwkCHK As CheckBox
    '        objwkCHK = Nothing
    '        objwkCHK = DirectCast(rowData.FindControl("CM48_EMGNCY_FLG"), CheckBox)
    '        Dim rowView As DataRowView = CType(rowData.DataItem, DataRowView)
    '        Dim state As String = rowView("CM48_EMGNCY_FLG").ToString()
    '        If state = "0" Then
    '            objwkCHK.Checked = True
    '        Else
    '            objwkCHK.Checked = False
    '        End If
    '    End If
    'Next
    '    End Sub
#End Region

    ''' <summary>
    ''' データバインド後
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

    End Sub

    '    ''' <summary>
    '    ''' グリッドビューの行ごとの処理
    '    ''' </summary>
    '    ''' <param name="sender"></param>
    '    ''' <param name="e"></param>
    '    ''' <remarks></remarks>
    '    Private Sub grvList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grvList.RowDataBound

    '        Dim strBuff As String = ""
    '        Dim rowData As GridViewRow

    '        rowData = e.Row

    '        If rowData.RowType = DataControlRowType.DataRow Then

    'Dim objwkTXT As TextBox
    'Dim sglBuff As Single
    ''通常料金のカンマ編集
    'objwkTXT = Nothing
    'objwkTXT = CType(rowData.FindControl("CM48_NML_PRICE"), TextBox)
    'strBuff = objwkTXT.Text
    'If Single.TryParse(strBuff, sglBuff) Then
    '    If sglBuff <> 0 Then
    '        objwkTXT.Text = Integer.Parse(sglBuff).ToString("#,##0")
    '    End If
    '    If Single.Parse(strBuff) = 0 Then
    '        objwkTXT.Text = "0"
    '    End If
    'Else
    '    objwkTXT.Text = ""
    'End If
    ''休日料金のカンマ編集
    'objwkTXT = Nothing
    'objwkTXT = DirectCast(rowData.FindControl("CM48_HLDY_PRICE"), TextBox)
    'strBuff = objwkTXT.Text
    'If Single.TryParse(strBuff, sglBuff) Then
    '    If sglBuff <> 0 Then
    '        objwkTXT.Text = Integer.Parse(sglBuff).ToString("#,##0")
    '    End If
    '    If Single.Parse(strBuff) = 0 Then
    '        objwkTXT.Text = "0"
    '    End If
    'Else
    '    objwkTXT.Text = ""
    'End If
    ''夜間料金のカンマ編集
    'objwkTXT = Nothing
    'objwkTXT = DirectCast(rowData.FindControl("CM48_NGHT_PRICE"), TextBox)
    'strBuff = objwkTXT.Text
    'If Single.TryParse(strBuff, sglBuff) Then
    '    If sglBuff <> 0 Then
    '        objwkTXT.Text = Integer.Parse(sglBuff).ToString("#,##0")
    '    End If
    '    If Single.Parse(strBuff) = 0 Then
    '        objwkTXT.Text = "0"
    '    End If
    'Else
    '    objwkTXT.Text = ""
    'End If
    ''開始日付の日付形式
    'objwkTXT = Nothing
    'objwkTXT = DirectCast(rowData.FindControl("CM48_SUMSTART_D"), TextBox)
    'If objwkTXT.Text Is DBNull.Value Then
    '    objwkTXT.Text = ""
    'ElseIf objwkTXT.Text.Trim = "" Then
    '    objwkTXT.Text = ""
    'Else
    '    strBuff = ""
    '    objwkTXT.Text = DateTime.Parse(objwkTXT.Text).ToString("yyyy/MM/dd")
    'End If
    ''チェックボックスの値を設定
    'Dim objwkCHK As CheckBox
    'objwkCHK = Nothing
    'objwkCHK = DirectCast(rowData.FindControl("CM48_EMGNCY_FLG"), CheckBox)
    'Dim objrowView As DataRowView = CType(rowData.DataItem, DataRowView)
    'Dim strDataVal As String = objrowView("CM48_EMGNCY_FLG").ToString()
    'If strDataVal = "1" Then
    '    objwkCHK.Checked = True
    'Else
    '    objwkCHK.Checked = False
    'End If

    '        End If

    '    End Sub

#Region "まとめてコメントアウト"

    '    ''' <summary>
    '    '''検索 機器分類に対応した機器種別をバインド
    '    ''' </summary>
    '    Protected Sub ddlsAPPAGroup_SelectedIndexChanged() Handles ddlsAPPAGroup.SelectedIndexChanged

    '        If ddlsAPPAGroup.SelectedIndex = 0 Then
    '            ddlsAPPACLS.SelectedIndex = 0
    '            ddlsAPPACLS.Enabled = False
    '        Else
    '            ddlsAPPACLS.Enabled = True
    '            stb.Clear()
    '            stb.Append(" SELECT [M06_APPA_CLS]                           AS CLS ")
    '            stb.Append("       ,[M06_APPA_CLS] + ':' + [M06_APPACLS_NM]  AS CLSNM ")
    '            stb.Append("   FROM [SPCDB].[dbo].[M06_APPACLASS] ")
    '            stb.Append("   WHERE M06_APPACLASS_CD = '" & ddlsAPPAGroup.SelectedValue & "' ")
    '            stb.Append("     AND M06_DELETE_FLG   = '0' ")
    '            'ClsSQL.psDropDownDataBind(ddlsAPPACLS, stb.ToString, "CLSNM", "CLS")
    '            ClsSQL.psDropDownDataBind(ddlsAPPACLS, stb.ToString, "CLSNM", "CLS")
    '        End If
    '        'フォーカス設定
    '        ddlsAPPAGroup.Focus()
    '    End Sub

    '    ''' <summary>
    '    '''編集 機器種別：ＨＤＤの時にＨＤＤドロップダウン活性
    '    ''' </summary>
    '    Protected Sub ddlAPPACLS_SelectedIndexChanged() Handles ddlAPPACLS.SelectedIndexChanged

    '        If ddlAPPACLS.SelectedValue = "09" Then
    '            ddlHDDCLS.Enabled = True
    '            ddlHDDNo.Enabled = True
    '        Else
    '            ddlHDDCLS.Enabled = False
    '            ddlHDDNo.Enabled = False
    '            ddlHDDCLS.SelectedIndex = 0
    '            ddlHDDNo.SelectedIndex = 0
    '        End If

    '        'フォーカス設定
    '        ddlAPPACLS.Focus()

    '    End Sub

    ' ''' <summary>
    ' '''編集 SYSTEM_CDに対応した取得先バージョンをバインド
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Protected Sub ddlSYS_SelectedIndexChanged() Handles ddlSYS.SelectedIndexChanged

    '    Select Case ddlSYS.SelectedIndex
    '        Case 0
    '            If ddlVer.Enabled = True Then
    '                ddlVer.SelectedIndex = 0
    '                ClsSQL.ControlClear(ddlVer, False)
    '            End If

    '            hdnSYS.Value = ""
    '            '工事依頼書用区分
    '            ClsSQL.psSetDropDownClassData(ddlCNSTDET, "0120")
    '        Case Else
    '            ddlVer.Enabled = True

    '            stb.Clear()
    '            stb.Append("SELECT M03_TBOX_VER FROM M03_TBOX WHERE M03_DELETE_FLG = 0 AND M03_TBOX_CD = '" & ddlSYS.SelectedValue.ToString & "' ")
    '            Dim Ver As New DataTable
    '            Ver = ClsSQL.getDataSetTable(stb.ToString, "Ver")
    '            'Ver.Rows.Add(Ver.NewRow)
    '            Ver.Rows.InsertAt(Ver.NewRow, 0)
    '            ddlVer.DataSource = Ver
    '            ddlVer.DataTextField = "M03_TBOX_VER"
    '            ddlVer.DataValueField = "M03_TBOX_VER"
    '            ddlVer.DataBind()
    '            'Select Case ddlSYS.SelectedValue
    '            '    Case "11", "12"
    '            '    Case Else
    '            '        ddlVer.Items.Insert(0, "-")
    '            'End Select
    '            'ddlVer.Items.Insert(0, "")

    '            hdnSYS.Value = ClsSQL.GetRecord("SELECT [M23_SYSTEM_CD]  FROM [SPCDB].[dbo].[M23_TBOXCLASS]  WHERE [M23_TBOXCLS] = '" & ddlSYS.SelectedValue & "' ")
    '            '工事依頼書用区分
    '            If hdnSYS.Value = "1" Or ddlSYS.SelectedValue = "11" Or ddlSYS.SelectedValue = "11" Then
    '                'ID/磁気無線/磁気有線
    '                ClsSQL.psSetDropDownClassData(ddlCNSTDET, "0118")
    '            Else
    '                'IC/LUTERUNA
    '                ClsSQL.psSetDropDownClassData(ddlCNSTDET, "0120")
    '            End If

    '    End Select

    '    'フォーカス設定
    '    ddlSYS.Focus()

    'End Sub

#End Region

#End Region

    '----------------------------------------------------------------------------------------------------------------------------
    '   Validation制御
    '----------------------------------------------------------------------------------------------------------------------------
#Region "                             --- Validation ---                                "

#Region "まとめてコメントアウト１"

    ' ''' <summary>
    ' ''' 検索 機器コード
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Protected Sub CstmVal_s01_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_s01.ServerValidate

    '    Dim tb As TextBox = txtsAPPACDfrom
    '    Const Name As String = "機器コード"
    '    source.ControlToValidate = tb.ID

    '    If txtsAPPACDfrom.Text = String.Empty And txtsAPPACDto.Text = String.Empty Then
    '        Exit Sub
    '    End If

    '    If Regex.IsMatch(tb.Text, "^[0-9]{0,8}$") = True And Regex.IsMatch(txtsAPPACDto.Text, "^[0-9]{0,8}$") = True Then
    '    Else
    '        source.ErrorMessage = "" & Name & "は半角数字で入力して下さい。"
    '        source.Text = "形式エラー"
    '        args.IsValid = False
    '        Exit Sub
    '    End If

    '    'If TryCast(txtsAPPACDfrom.Text, String ) is Nothing or TryCast(txtsAPPACDto.Text )= Nothing then

    '    'End If

    '    Dim intTgt As Int32

    '    If Int32.TryParse(txtsAPPACDfrom.Text, intTgt) = True AndAlso Int32.TryParse(txtsAPPACDto.Text, intTgt) = True Then
    '        If Integer.Parse(txtsAPPACDfrom.Text) > Integer.Parse(txtsAPPACDto.Text) Then
    '            source.ErrorMessage = "" & Name & " は開始が終了以下となるよう入力してください。"
    '            source.Text = "形式エラー"
    '            args.IsValid = False
    '            Exit Sub
    '        End If
    '    End If



    'End Sub

    ' ''' <summary>
    ' ''' 機器コード
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Protected Sub CstmVal_APPACD_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_APPACD.ServerValidate

    '    Dim tb As TextBox = DirectCast(Master.FindControl("MainContent").FindControl(source.ControlToValidate), TextBox)
    '    Const Name As String = "機器コード"

    '    If tb.Text = String.Empty Then
    '        source.ErrorMessage = "" & Name & "に値が設定されていません。"
    '        source.Text = "未入力エラー"
    '        args.IsValid = False
    '        Exit Sub
    '    End If

    '    If Regex.IsMatch(tb.Text, "^[0-9]{8}$") Then
    '    Else
    '        source.ErrorMessage = "" & Name & "は半角数字 8 桁で入力して下さい。"
    '        source.Text = "形式エラー"
    '        args.IsValid = False
    '        Exit Sub
    '    End If

    '    '重複チェック
    '    Dim strKey1 As String = txtAPPACD.Text
    '    'Dim strKey2 As String = ddlAPPACLS.SelectedValue.ToString
    '    'Dim strKey3 As String = txtSEQ.Text
    '    'Dim strKey4 As String = txtSEQ.Text
    '    'Dim strKey5 As String = ddlHDDCLS.SelectedValue.ToString

    '    stb.Clear()
    '    stb.Append("SELECT COUNT(*) FROM " & TableName & " ")
    '    stb.Append("WHERE  ")
    '    stb.Append("     " & KeyName1 & " = '" & strKey1 & "' ")
    '    'stb.Append(" AND " & KeyName2 & " = '" & strKey2 & "' ")
    '    'stb.Append(" AND " & KeyName3 & " = '" & strKey3 & "' ")
    '    stb.Append(" AND  NOT (  ")
    '    stb.Append(ClsSQL.GeneStrSearch(KeyName1, ViewState("KeyName1"), "完全", ""))
    '    stb.Append(ClsSQL.GeneStrSearch(KeyName2, ViewState("KeyName2"), "完全"))
    '    stb.Append(ClsSQL.GeneStrSearch(KeyName3, ViewState("KeyName3"), "完全"))
    '    stb.Append(ClsSQL.GeneStrSearch(KeyName4, ViewState("KeyName4"), "完全"))
    '    stb.Append(ClsSQL.GeneStrSearch(KeyName5, ViewState("KeyName5"), "完全"))

    '    'stb.Append("     " & KeyName1 & " = '" & ViewState("KeyName1") & "' ")
    '    'stb.Append(" AND " & KeyName2 & " = '" & ViewState("KeyName2") & "' ")
    '    'stb.Append(" AND " & KeyName3 & " = '" & ViewState("KeyName3") & "' ")
    '    'stb.Append(" AND " & KeyName4 & " = '" & ViewState("KeyName4") & "' ")
    '    'stb.Append(" AND " & KeyName5 & " = '" & ViewState("KeyName5") & "' ")
    '    stb.Append("  ) ")

    '    '変更・追加する場合
    '    Select Case ClsSQL.GetRecord(stb.ToString)
    '        Case -1    'エラー
    '        Case 0     '存在しない
    '        Case Else  '存在する
    '            stb.Clear()
    '            'stb.Append("システムコード:" & strKey1 & " ")
    '            'stb.Append("機器種別:" & strKey2 & " ")
    '            'stb.Append("機器種別:" & strKey3 & " ")
    '            'stb.Append("連番:" & strKey4 & " ")
    '            'stb.Append("に　" & Name & "： " & strKey4 & " は既に登録されています。")
    '            'stb.Append("に　HDDNo：" & ddlHDDNo.SelectedValue.ToString & "")
    '            stb.Append("  機器コード：" & txtAPPACD.Text & "  は既に登録されています。")

    '            source.ErrorMessage = stb.ToString
    '            source.Text = "整合性エラー"
    '            args.IsValid = False
    '    End Select
    'End Sub
#End Region

    ' ''' <summary>
    ' ''' 工事コード検索　入力チェック
    ' ''' </summary>
    ' ''' <param name="source"></param>
    ' ''' <param name="args"></param>
    ' ''' <remarks></remarks>
    'Private Sub CstmVal_txtSrchCODE_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_txtSrchCODE.ServerValidate

    '    Dim dtrErrMes As DataRow

    '    Dim strCntlName As String = "工事コード"
    '    source.ControlToValidate = txtSrchCODE.ID

    '    'If ViewState("txtSrchCODE") Is Nothing Then
    '    'ElseIf ViewState("txtSrchCODE") = String.Empty Then
    '    '    dtrErrMes = ClsCMCommon.pfGet_ValMes("5001", strCntlName)
    '    '    CstmVal_txtSrchCODE.Text = P_VALMES_AST
    '    '    CstmVal_txtSrchCODE.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
    '    '    CstmVal_txtSrchCODE.IsValid = False
    '    '    args.IsValid = False
    '    '    Exit Sub
    '    'End If

    '    If txtSrchCODE.Text Is Nothing Then
    '    Else
    '        If txtSrchCODE.Text.Trim = "" Then
    '        Else
    '            If Regex.IsMatch(txtSrchCODE.Text, "^[0-9a-zA-Z]+$") = False Then
    '                dtrErrMes = ClsCMCommon.pfGet_ValMes("4001", strCntlName, "半角英数字")
    '                CstmVal_txtSrchCODE.Text = P_VALMES_AST
    '                CstmVal_txtSrchCODE.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
    '                CstmVal_txtSrchCODE.IsValid = False
    '                args.IsValid = False
    '                Exit Sub
    '            End If
    '        End If
    '    End If

    'End Sub

    ' ''' <summary>
    ' ''' 工事名検索　入力チェック
    ' ''' </summary>
    ' ''' <param name="source"></param>
    ' ''' <param name="args"></param>
    ' ''' <remarks></remarks>
    'Private Sub CstmVal_txtSrchCNST_NM_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_txtSrchCNST_NM.ServerValidate

    'End Sub

    ' ''' <summary>
    ' ''' 工事コード　入力チェック
    ' ''' </summary>
    ' ''' <param name="source"></param>
    ' ''' <param name="args"></param>
    ' ''' <remarks></remarks>
    'Private Sub CstmVal_txtCODE_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_txtCODE.ServerValidate

    '    Dim dtrErrMes As DataRow

    '    Dim strCntlName As String = "工事コード"
    '    source.ControlToValidate = txtCODE.ID

    '    If ViewState("txtCODE") Is Nothing Then
    '    ElseIf ViewState("txtCODE") = String.Empty Then
    '        dtrErrMes = ClsCMCommon.pfGet_ValMes("5001", strCntlName)
    '        CstmVal_txtCODE.Text = P_VALMES_AST
    '        CstmVal_txtCODE.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
    '        CstmVal_txtCODE.IsValid = False
    '        args.IsValid = False
    '        Exit Sub
    '    End If

    '    If Regex.IsMatch(txtCODE.Text, "^[0-9a-zA-Z]+$") = False Then
    '        dtrErrMes = ClsCMCommon.pfGet_ValMes("4001", strCntlName, "半角英数字")
    '        CstmVal_txtCODE.Text = P_VALMES_AST
    '        CstmVal_txtCODE.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
    '        CstmVal_txtCODE.IsValid = False
    '        args.IsValid = False
    '        Exit Sub
    '    End If

    'End Sub

    ' ''' <summary>
    ' ''' 工事名　入力チェック
    ' ''' </summary>
    ' ''' <param name="source"></param>
    ' ''' <param name="args"></param>
    ' ''' <remarks></remarks>
    'Private Sub CstmVal_txtCNST_NM_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_txtCNST_NM.ServerValidate

    '    Dim dtrErrMes As DataRow

    '    Dim strCntlName As String = "工事名"
    '    source.ControlToValidate = txtCNST_NM.ID

    '    If ViewState("txtCNST_NM") Is Nothing Then
    '    ElseIf ViewState("txtCNST_NM") = String.Empty Then
    '        dtrErrMes = ClsCMCommon.pfGet_ValMes("5001", strCntlName)
    '        CstmVal_txtCNST_NM.Text = P_VALMES_AST
    '        CstmVal_txtCNST_NM.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
    '        CstmVal_txtCNST_NM.IsValid = False
    '        args.IsValid = False
    '        Exit Sub
    '    End If

    'End Sub

    ' ''' <summary>
    ' ''' 適用開始日　入力チェック
    ' ''' </summary>
    ' ''' <param name="source"></param>
    ' ''' <param name="args"></param>
    ' ''' <remarks></remarks>
    'Private Sub CstmVal_cdbSUMSTART_D_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_cdbSUMSTART_D.ServerValidate

    'End Sub

    ' ''' <summary>
    ' ''' 通常料金　入力チェック
    ' ''' </summary>
    ' ''' <param name="source"></param>
    ' ''' <param name="args"></param>
    ' ''' <remarks></remarks>
    'Private Sub CstmVal_txtNML_PRICE_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_txtNML_PRICE.ServerValidate

    '    Dim dtrErrMes As DataRow

    '    Dim strCntlName As String = "通常料金"
    '    source.ControlToValidate = txtNML_PRICE.ID

    '    If ViewState("txtNML_PRICE") Is Nothing Then
    '    ElseIf ViewState("txtNML_PRICE") = String.Empty Then
    '        dtrErrMes = ClsCMCommon.pfGet_ValMes("5001", strCntlName)
    '        CstmVal_txtNML_PRICE.Text = P_VALMES_AST
    '        CstmVal_txtNML_PRICE.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
    '        CstmVal_txtNML_PRICE.IsValid = False
    '        args.IsValid = False
    '        Exit Sub
    '    End If

    '    If Regex.IsMatch(txtNML_PRICE.Text, "^[0-9]+$") = False Then
    '        dtrErrMes = ClsCMCommon.pfGet_ValMes("4001", strCntlName, "半角数字")
    '        CstmVal_txtNML_PRICE.Text = P_VALMES_AST
    '        CstmVal_txtNML_PRICE.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
    '        CstmVal_txtNML_PRICE.IsValid = False
    '        args.IsValid = False
    '        Exit Sub
    '    End If

    'End Sub

    ' ''' <summary>
    ' ''' 休日料金　入力チェック
    ' ''' </summary>
    ' ''' <param name="source"></param>
    ' ''' <param name="args"></param>
    ' ''' <remarks></remarks>
    'Private Sub CstmVal_txtHLDY_PRICE_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_txtHLDY_PRICE.ServerValidate

    '    Dim dtrErrMes As DataRow

    '    Dim strCntlName As String = "休日料金"
    '    source.ControlToValidate = txtHLDY_PRICE.ID

    '    If ViewState("txtHLDY_PRICE") Is Nothing Then
    '    ElseIf ViewState("txtHLDY_PRICE") = String.Empty Then
    '        dtrErrMes = ClsCMCommon.pfGet_ValMes("5001", strCntlName)
    '        CstmVal_txtHLDY_PRICE.Text = P_VALMES_AST
    '        CstmVal_txtHLDY_PRICE.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
    '        CstmVal_txtHLDY_PRICE.IsValid = False
    '        args.IsValid = False
    '        Exit Sub
    '    End If

    '    If Regex.IsMatch(txtHLDY_PRICE.Text, "^[0-9]+$") = False Then
    '        dtrErrMes = ClsCMCommon.pfGet_ValMes("4001", strCntlName, "半角数字")
    '        CstmVal_txtHLDY_PRICE.Text = P_VALMES_AST
    '        CstmVal_txtHLDY_PRICE.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
    '        CstmVal_txtHLDY_PRICE.IsValid = False
    '        args.IsValid = False
    '        Exit Sub
    '    End If

    'End Sub

    ' ''' <summary>
    ' ''' 夜間料金　入力チェック
    ' ''' </summary>
    ' ''' <param name="source"></param>
    ' ''' <param name="args"></param>
    ' ''' <remarks></remarks>
    'Private Sub CstmVal_txtNGHT_PRICE_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_txtNGHT_PRICE.ServerValidate

    '    Dim dtrErrMes As DataRow

    '    Dim strCntlName As String = "夜間料金"
    '    source.ControlToValidate = txtNGHT_PRICE.ID

    '    If ViewState("txtNGHT_PRICE") Is Nothing Then
    '    ElseIf ViewState("txtNGHT_PRICE") = String.Empty Then
    '        dtrErrMes = ClsCMCommon.pfGet_ValMes("5001", strCntlName)
    '        CstmVal_txtNGHT_PRICE.Text = P_VALMES_AST
    '        CstmVal_txtNGHT_PRICE.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
    '        CstmVal_txtNGHT_PRICE.IsValid = False
    '        args.IsValid = False
    '        Exit Sub
    '    End If

    '    If Regex.IsMatch(txtNGHT_PRICE.Text, "^[0-9]+$") = False Then
    '        dtrErrMes = ClsCMCommon.pfGet_ValMes("4001", strCntlName, "半角数字")
    '        CstmVal_txtNGHT_PRICE.Text = P_VALMES_AST
    '        CstmVal_txtNGHT_PRICE.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
    '        CstmVal_txtNGHT_PRICE.IsValid = False
    '        args.IsValid = False
    '        Exit Sub
    '    End If

    'End Sub

    ' ''' <summary>
    ' ''' 料金大分類　入力チェック
    ' ''' </summary>
    ' ''' <param name="source"></param>
    ' ''' <param name="args"></param>
    ' ''' <remarks></remarks>
    'Private Sub CstmVal_ddlL_CLS_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_ddlL_CLS.ServerValidate

    '    Dim dtrErrMes As DataRow

    '    Dim strCntlName As String = "料金大分類"
    '    source.ControlToValidate = ddlL_CLS.ID

    '    If ViewState("ddlL_CLS") Is Nothing Then
    '    ElseIf ViewState("ddlL_CLS") = String.Empty Then
    '        dtrErrMes = ClsCMCommon.pfGet_ValMes("5001", strCntlName)
    '        CstmVal_ddlL_CLS.Text = P_VALMES_AST
    '        CstmVal_ddlL_CLS.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
    '        CstmVal_ddlL_CLS.IsValid = False
    '        args.IsValid = False
    '        Exit Sub
    '    End If

    'End Sub

    ' ''' <summary>
    ' ''' 料金中分類　入力チェック
    ' ''' </summary>
    ' ''' <param name="source"></param>
    ' ''' <param name="args"></param>
    ' ''' <remarks></remarks>
    'Private Sub CstmVal_ddlM_CLS_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_ddlM_CLS.ServerValidate

    '    Dim dtrErrMes As DataRow

    '    Dim strCntlName As String = "料金中分類"
    '    source.ControlToValidate = ddlM_CLS.ID

    '    If ViewState("ddlM_CLS") Is Nothing Then
    '    ElseIf ViewState("ddlM_CLS") = String.Empty Then
    '        dtrErrMes = ClsCMCommon.pfGet_ValMes("5001", strCntlName)
    '        CstmVal_ddlM_CLS.Text = P_VALMES_AST
    '        CstmVal_ddlM_CLS.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
    '        CstmVal_ddlM_CLS.IsValid = False
    '        args.IsValid = False
    '        Exit Sub
    '    End If

    'End Sub

#Region "まとめてコメントアウト２"

    ' ''' <summary>
    ' ''' 機器略称
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Protected Sub CstmVal_Short_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_Short.ServerValidate
    '    Dim tb As TextBox = DirectCast(Master.FindControl("MainContent").FindControl(source.ControlToValidate), TextBox)
    '    Const Name As String = "機器略称"
    '    If tb.Text = String.Empty Then
    '        source.ErrorMessage = "" & Name & "に値が設定されていません。"
    '        source.Text = "未入力エラー"
    '        args.IsValid = False
    '        Exit Sub
    '    End If
    '    If tb.Text.ToUpper = "NULL" Then
    '        source.ErrorMessage = "" & Name & "は不正な値です。"
    '        source.Text = "形式エラー"
    '        args.IsValid = False
    '        Exit Sub
    '    End If
    'End Sub

    '    ''' <summary>
    '    ''' 機器分類
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Protected Sub CstmVal_APPAGroup_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_APPAGroup.ServerValidate
    '        Const Name As String = "機器分類"
    ''        Dim ddl As DropDownList = ddlAPPAGroup
    ''        source.ControlToValidate = ddl.ID
    ''        If ddl.SelectedValue = "" Then
    '            source.ErrorMessage = "" & Name & "に値が設定されていません。"
    '            source.Text = "未入力エラー"
    '            args.IsValid = False
    '            Exit Sub
    ''        End If
    '    End Sub

    '    ''' <summary>
    '    ''' 型番
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    Protected Sub CstmVal_Model_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_Model.ServerValidate
    '        Const Name As String = "型番"
    ''        Dim tb As TextBox = txtMODELNo
    ''        source.ControlToValidate = tb.ID
    '        'If tb.Text = String.Empty Then
    '        '    source.ErrorMessage = "" & Name & "に値が設定されていません。"
    '        '    source.Text = "未入力エラー"
    '        '    args.IsValid = False
    '        '    Exit Sub
    '        'End If
    ''        If Regex.IsMatch(tb.Text, "^[a-zA-Z0-9 -/:-@\[-\`\{-\~]{0,20}$") = False Then
    '            source.ErrorMessage = "" & Name & "は半角英数字記号で入力して下さい。"
    '            source.Text = "形式エラー"
    '            args.IsValid = False
    '            Exit Sub
    ''        End If
    '    End Sub

    ' ''' <summary>
    ' ''' 機器種別
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Protected Sub CstmVal_APPACLS_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_APPACLS.ServerValidate
    '    Const Name As String = "機器種別"
    '    Dim ddl As DropDownList = ddlAPPACLS
    '    source.ControlToValidate = ddl.ID
    '    If ddl.SelectedValue = "" Then
    '        source.ErrorMessage = "" & Name & "に値が設定されていません。"
    '        source.Text = "未入力エラー"
    '        args.IsValid = False
    '        Exit Sub
    '    End If
    'End Sub

    ' ''' <summary>
    ' ''' システムコード
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Protected Sub CstmVal_SYS_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_SYS.ServerValidate
    '    'Const Name As String = "システムコード"
    '    Dim ddl As DropDownList = DirectCast(Master.FindControl("MainContent").FindControl(source.ControlToValidate), DropDownList)
    '    If ddl.Enabled = False Then
    '        Exit Sub
    '    End If
    'End Sub
    ' ''' <summary>
    ' ''' TBOXバージョン
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Protected Sub CstmVal_Ver_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_Ver.ServerValidate
    '    'Const Name As String = "TBOXバージョン"
    '    'Dim ddl As DropDownList = DirectCast(Master.FindControl("MainContent").FindControl(source.ControlToValidate), DropDownList)
    '    'If ddl.Enabled = False Then
    '    '    Exit Sub
    '    'End If
    '    'If ddl.SelectedIndex = 0 Then
    '    '    source.ErrorMessage = "" & Name & "に値が設定されていません。"
    '    '    source.Text = "未入力エラー"
    '    '    args.IsValid = False
    '    '    Exit Sub
    '    'End If
    'End Sub

    ' ''' <summary>
    ' ''' HDD No
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Protected Sub CstmVal_HDDNo_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_HDDNo.ServerValidate
    '    Const Name As String = "HDD No"
    '    Dim ddl As DropDownList = DirectCast(Master.FindControl("MainContent").FindControl(source.ControlToValidate), DropDownList)
    '    If ddl.Enabled = False Then
    '        Exit Sub
    '    End If
    '    If ddl.SelectedIndex = 0 And ddlHDDCLS.SelectedIndex <> 0 Then
    '        source.ErrorMessage = "" & Name & "に値が設定されていません。"
    '        source.Text = "未入力エラー"
    '        args.IsValid = False
    '        Exit Sub
    '    End If
    'End Sub

    ' ''' <summary>
    ' ''' HDD 種別
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Protected Sub CstmVal_HDDCLS_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_HDDCLS.ServerValidate
    '    'Const Name As String = "HDD種別"
    '    Dim ddl As DropDownList = DirectCast(Master.FindControl("MainContent").FindControl(source.ControlToValidate), DropDownList)
    '    If ddl.Enabled = False Then
    '        Exit Sub
    '    End If
    '    If ddl.SelectedIndex = 0 Then
    '        'source.ErrorMessage = "" & Name & "に値が設定されていません。"
    '        'source.Text = "未入力エラー"
    '        'args.IsValid = False
    '        Exit Sub
    '    End If
    '    '重複チェック
    '    'Dim strKey1 As String = ddlSYS.SelectedValue.ToString
    '    'Dim strKey2 As String = ddlAPPACLS.SelectedValue.ToString
    '    'Dim strKey3 As String = txtSEQ.Text
    '    'Dim strKey4 As String = txtSEQ.Text
    '    'Dim strKey5 As String = ddlHDDCLS.SelectedValue.ToString
    '    'stb.Clear()
    '    'stb.Append("SELECT COUNT(*) FROM " & TableName & " ")
    '    'stb.Append("WHERE  ")
    '    'stb.Append("     " & KeyName1 & " = '" & strKey1 & "' ")
    '    'stb.Append(" AND " & KeyName2 & " = '" & strKey2 & "' ")
    '    ''stb.Append(" AND " & KeyName3 & " = '" & strKey3 & "' ")
    '    'stb.Append(" AND  M97_HDD_NO      = '" & ddlHDDNo.SelectedValue.ToString & "' ")
    '    'stb.Append(" AND  M97_HDD_CLS     = '" & ddlHDDCLS.SelectedValue.ToString & "' ")
    '    'stb.Append(" AND  NOT (  ")
    '    'stb.Append(ClsSQL.GeneStrSearch(KeyName1, ViewState("KeyName1"), "完全", ""))
    '    'stb.Append(ClsSQL.GeneStrSearch(KeyName2, ViewState("KeyName2"), "完全"))
    '    'stb.Append(ClsSQL.GeneStrSearch(KeyName3, ViewState("KeyName3"), "完全"))
    '    'stb.Append(ClsSQL.GeneStrSearch(KeyName4, ViewState("KeyName4"), "完全"))
    '    'stb.Append(ClsSQL.GeneStrSearch(KeyName5, ViewState("KeyName5"), "完全"))
    '    'stb.Append("     " & KeyName1 & " = '" & ViewState("KeyName1") & "' ")
    '    'stb.Append(" AND " & KeyName2 & " = '" & ViewState("KeyName2") & "' ")
    '    'stb.Append(" AND " & KeyName3 & " = '" & ViewState("KeyName3") & "' ")
    '    'stb.Append(" AND  " & KeyName4 & " = '" & ViewState("KeyName4") & "' ")
    '    'stb.Append(" AND  " & KeyName5 & " = '" & ViewState("KeyName5") & "' ")
    '    stb.Append("  ) ")
    '    '変更・追加する場合
    '    Select Case ClsSQL.GetRecord(stb.ToString)
    '        Case -1    'エラー
    '        Case 0     '存在しない
    '        Case Else  '存在する
    '            stb.Clear()
    '            'stb.Append("システムコード:" & strKey1 & " ")
    '            'stb.Append("機器種別:" & strKey2 & " ")
    '            'stb.Append("機器種別:" & strKey3 & " ")
    '            'stb.Append("連番:" & strKey4 & " ")
    '            'stb.Append("に　" & Name & "： " & strKey4 & " は既に登録されています。")
    '            stb.Append("に　HDDNo：" & ddlHDDNo.SelectedValue.ToString & "")
    '            stb.Append("  HDD種別：" & ddlHDDCLS.SelectedValue.ToString & "  は既に登録されています。")

    '            source.ErrorMessage = stb.ToString
    '            source.Text = "整合性エラー"
    '            args.IsValid = False
    '    End Select
    'End Sub


#End Region

#End Region
#End Region

    '============================================================================================================================
    '=　プロシージャ
    '============================================================================================================================
#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 料金大分類をバインド
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlL_CLS_DataBind()

        Dim objSQLCmd As New SqlClient.SqlCommand
        Dim objwkDS As New DataSet

        objSQLCmd.Connection = mclsDB.mobjDB
        objSQLCmd.CommandText = "COMUPDM48_S1"
        If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
            'エラー
            clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "リスト作成")
            objStack = New StackFrame
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "リスト情報取得失敗", "Catch")
            Exit Sub
        End If

        If objwkDS.Tables.Count > 0 Then
            Me.ddlL_CLS.ppDropDownList.DataSource = objwkDS.Tables(0)
            Me.ddlL_CLS.ppDropDownList.DataTextField = "M45_L_CLS_DISP"
            Me.ddlL_CLS.ppDropDownList.DataValueField = "M45_L_CLS"
            Me.ddlL_CLS.ppDropDownList.DataBind()
            Me.ddlL_CLS.ppDropDownList.Items.Insert(0, "")
        End If

        Call mclsDB.psDisposeDataSet(objwkDS)

        'ClsSQL.psDropDownDataBind(ddlsAPPACLS, stb.ToString, "CLSNM", "CLS")
        '        ClsSQL.psDropDownDataBind(ddlScreen_ID, stb.ToString, "CLSNM", "CLS")

    End Sub

    ''' <summary>
    ''' 料金中分類をバインド
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlM_CLS_DataBind()

        Dim objSQLCmd As New SqlClient.SqlCommand
        Dim objwkDS As New DataSet

        objSQLCmd.Connection = mclsDB.mobjDB
        objSQLCmd.CommandText = "COMUPDM48_S2"
        objSQLCmd.Parameters.Add("prmL_CLS", SqlDbType.NVarChar)
        objSQLCmd.Parameters("prmL_CLS").Value = ViewState("ddlL_CLS").ToString
        If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
            'エラー
            clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "リスト作成")
            objStack = New StackFrame
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "リスト情報取得失敗", "Catch")
            Exit Sub
        End If

        If objwkDS.Tables.Count > 0 Then
            Me.ddlM_CLS.ppDropDownList.DataSource = objwkDS.Tables(0)
            Me.ddlM_CLS.ppDropDownList.DataTextField = "M45_M_CLS_DISP"
            Me.ddlM_CLS.ppDropDownList.DataValueField = "M45_M_CLS"
            Me.ddlM_CLS.ppDropDownList.DataBind()
            Me.ddlM_CLS.ppDropDownList.Items.Insert(0, "")
        End If

        Call mclsDB.psDisposeDataSet(objwkDS)

        'ClsSQL.psDropDownDataBind(ddlsAPPACLS, stb.ToString, "CLSNM", "CLS")
        '        ClsSQL.psDropDownDataBind(ddlScreen_ID, stb.ToString, "CLSNM", "CLS")

    End Sub

    ''' <summary>
    ''' データ取得～表示制限～バインド
    ''' </summary>
    ''' <param name="strWhere">SQL 条件文</param>
    Sub GetData_and_GridBind(Optional ByVal strWHERE As String = "")

        Dim objSQLCmd As New SqlClient.SqlCommand
        Dim objwkDS As New DataSet

        Try
            '            Dim RecordCount As Integer  '該当件数

            objSQLCmd.Connection = mclsDB.mobjDB
            objSQLCmd.CommandText = "COMUPDM48_S3"
            objSQLCmd.Parameters.Add("txtSrchCODE", SqlDbType.NVarChar)
            objSQLCmd.Parameters.Add("txtSrchCNST_NM", SqlDbType.NVarChar)
            Dim strprmValue As String = ""
            If ViewState("txtSrchCODE") Is Nothing Then
                strprmValue = ""
            ElseIf ViewState("txtSrchCODE") = "" Then
                strprmValue = ""
            Else
                strprmValue = ViewState("txtSrchCODE")
            End If
            objSQLCmd.Parameters("txtSrchCODE").Value = strprmValue
            strprmValue = ""
            If ViewState("txtSrchCNST_NM") Is Nothing Then
                strprmValue = ""
            ElseIf ViewState("txtSrchCNST_NM") = "" Then
                strprmValue = ""
            Else
                strprmValue = ViewState("txtSrchCNST_NM")
            End If
            objSQLCmd.Parameters("txtSrchCNST_NM").Value = strprmValue
            If mstrDispMode = "First" Then
                objSQLCmd.Parameters("txtSrchCODE").Value = ""
                objSQLCmd.Parameters("txtSrchCNST_NM").Value = ""
            End If
            If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
                'エラー
                clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "リスト作成")
                objStack = New StackFrame
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "リスト情報取得失敗", "Catch")
                Exit Sub
            End If

            If objwkDS.Tables.Count > 0 Then
                Me.grvList.DataSource = objwkDS.Tables(0)
                Me.grvList.DataBind()
            Else
                Me.grvList.DataSource = Nothing
                Me.grvList.DataBind()
            End If

            '該当件数表示
            If objwkDS.Tables(0).Rows.Count > 0 Then
                Master.ppCount = objwkDS.Tables(0).Rows(0).Item("CMAXCNT")
            Else
                Master.ppCount = 0
            End If

            '検索条件の保存
            Me.ViewState.Add("strWHERE", strWHERE)

        Catch ex As Exception
            'データの取得に失敗しました
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            'ログ出力
            objStack = New StackFrame
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Exit Sub
        Finally
            Call mclsDB.psDisposeDataSet(objwkDS)
        End Try

    End Sub

    ''' <summary>
    '''排他制御処理
    ''' </summary>
    Function fExclusive(ByVal arKeyValue As String()) As Boolean

        fExclusive = False

        Dim strExclusiveDate As String = Nothing
        Dim arTable_Name As New ArrayList
        Dim arKey As New ArrayList

        '★排他情報削除
        If Not Me.Master.ppExclusiveDate = String.Empty Then

            If clsExc.pfDel_Exclusive(Me _
                               , Session(P_SESSION_SESSTION_ID) _
                               , Me.Master.ppExclusiveDate) = 0 Then
                Me.Master.ppExclusiveDate = String.Empty
            Else
                'ログ出力終了
                psLogEnd(Me)
                Exit Function
            End If
        End If

        '★ロック対象テーブル名の登録
        arTable_Name.Insert(0, TableName)

        '★ロックテーブルキー項目の登録
        For i As Integer = 0 To arKeyValue.Count - 1
            If arKeyValue(i) = "" Then
                Exit For
            Else
                arKey.Insert(i, arKeyValue(i))
            End If
        Next

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
            Exit Function
        End If

        fExclusive = True

    End Function


#Region "まとめてコメントアウト"
    ' ''' <summary>
    ' ''' DropDownList データ取得＆バインド
    ' ''' </summary>
    'Private Sub setDropDownList()

    '    'TBOX機種コード
    '    stb.Clear()
    '    stb.Append(" SELECT [M23_TBOXCLS] ")
    '    stb.Append("       ,(M23_TBOXCLS + ' : ' + M23_TBOXCLS_NM) AS CLSNM ")
    '    stb.Append("   FROM [SPCDB].[dbo].[M23_TBOXCLASS] WHERE M23_DELETE_FLG ='0' ")
    '    stb.Append(" UNION  ")
    '    stb.Append(" SELECT '11', '11 : 磁気無線' ")
    '    stb.Append(" UNION  ")
    '    stb.Append(" SELECT '12', '12 : 磁気有線' ")
    '    'ClsSQL.psDropDownDataBind(ddlSYS, stb.ToString, "CLSNM", "M23_TBOXCLS")
    '    'ClsSQL.psDropDownDataBind(ddlsSYS, stb.ToString, "CLSNM", "M23_TBOXCLS")

    '    'ddlSYS.Items.Item(0).Attributes.Add("style", "color:Red;")
    '    'ddlSYS.Items.Item(8).Attributes.Add("style", "color:Blue;")

    '    '機器分類
    '    stb.Clear()
    '    stb.Append(" SELECT [M73_APPACLASS_CD] AS VALUE ")
    '    stb.Append("       ,[M73_APPACLASS_CD] + ' : ' + [M73_APPACLASS_NM] AS TEXT ")
    '    stb.Append("   FROM [SPCDB].[dbo].[M73_APPA_GROUPING] ")
    '    stb.Append(" UNION  ")
    '    stb.Append(" SELECT '99', '99 : その他' ")
    '    'ClsSQL.psDropDownDataBind(ddlAPPAGroup, stb.ToString, "TEXT", "VALUE")
    '    'ClsSQL.psDropDownDataBind(ddlsAPPAGroup, stb.ToString, "TEXT", "VALUE")

    '    '機器種別
    '    stb.Clear()
    '    stb.Append(" SELECT [M06_APPA_CLS]                           AS CLS ")
    '    stb.Append("       ,[M06_APPA_CLS] + ':' + [M06_APPACLS_NM]  AS CLSNM ")
    '    stb.Append("   FROM [SPCDB].[dbo].[M06_APPACLASS] ")
    '    stb.Append("   WHERE M06_APPACLASS_CD = '01' ")
    '    stb.Append("     AND M06_DELETE_FLG   = '0' ")
    '    'ClsSQL.psDropDownDataBind(ddlsAPPACLS, stb.ToString, "CLSNM", "CLS")
    '    'ClsSQL.psDropDownDataBind(ddlAPPACLS, stb.ToString, "CLSNM", "CLS")

    '    'HDDNo
    '    stb.Clear()
    '    stb.Append(" SELECT DISTINCT [M57_HDD_NO] AS HDDNo ")
    '    stb.Append("   FROM [SPCDB].[dbo].[M57_HDD] ")
    '    'ClsSQL.psDropDownDataBind(ddlHDDNo, stb.ToString, "HDDNo", "HDDNo")

    '    'HDD種別
    '    stb.Clear()
    '    stb.Append(" SELECT DISTINCT [M57_HDD_CLS] AS HDDCLS ")
    '    stb.Append("   FROM [SPCDB].[dbo].[M57_HDD] ")
    '    'ClsSQL.psDropDownDataBind(ddlHDDCLS, stb.ToString, "HDDCLS", "HDDCLS", False)

    '    '工事依頼書用区分
    '    'ClsSQL.psSetDropDownClassData(ddlCNSTDET, "0120")

    'End Sub
#End Region

    ''' <summary>
    ''' コントロール制御
    ''' </summary>
    ''' <param name="strCase"></param>
    ''' <remarks></remarks>
    Private Sub setControl(strCase As String)

        Select Case strCase
            Case "Default", "Search"
                ClsSQL.ControlClear(txtSrchCODE, True)
                ClsSQL.ControlClear(txtSrchCNST_NM, True)
                ClsSQL.ControlClear(txtCODE, True)
                ClsSQL.ControlClear(txtCNST_NM, True)
                ClsSQL.ControlClear(cdbSUMSTART_D, True)
                ClsSQL.ControlClear(txtNML_PRICE, True)
                ClsSQL.ControlClear(txtHLDY_PRICE, True)
                ClsSQL.ControlClear(txtNGHT_PRICE, True)
                ClsSQL.ControlClear(ddlL_CLS, True)
                ClsSQL.ControlClear(ddlM_CLS, True)
                txtSrchCODE.ppEnabled = True
                txtSrchCNST_NM.ppEnabled = True
            Case "UPD", "ADD", "DEL"
                ClsSQL.ControlClear(txtSrchCODE, True)
                ClsSQL.ControlClear(txtSrchCNST_NM, True)
                ClsSQL.ControlClear(txtCODE, True)
                ClsSQL.ControlClear(txtCNST_NM, True)
                ClsSQL.ControlClear(cdbSUMSTART_D, True)
                ClsSQL.ControlClear(txtNML_PRICE, True)
                ClsSQL.ControlClear(txtHLDY_PRICE, True)
                ClsSQL.ControlClear(txtNGHT_PRICE, True)
                ClsSQL.ControlClear(ddlL_CLS, True)
                ClsSQL.ControlClear(ddlM_CLS, True)
                txtSrchCODE.ppEnabled = True
                txtSrchCNST_NM.ppEnabled = True
        End Select

    End Sub

#Region "まとめてコメントアウト３"

    ' ''' <summary>
    ' ''' 文字列を取得
    ' ''' </summary>
    ' ''' <param name="strName">文字列を取り出すキー</param>
    'Public ReadOnly Property GetStr(ByVal strName As String)
    '    Get
    '        Dim stb As New StringBuilder
    '        stb.Clear()

    '        Select Case strName
    '            Case "SELECT"
    '                ''文頭のSELECT不要
    '                'stb.Append("         [M48_APPA_CD]      AS 機器コード ")
    '                'stb.Append("        ,[M48_APPA_NM]		AS 機器名称 ")
    '                'stb.Append("        ,[M48_SHORT_NM]		AS 機器略称 ")
    '                'stb.Append("        ,CASE [M48_APPACLASS_CD] ")
    '                'stb.Append(" 		   WHEN '99' THEN '99 : その他'  ")
    '                'stb.Append(" 		   ELSE ISNULL([M48_APPACLASS_CD] + ' : ' + [M73_APPACLASS_NM], [M48_APPACLASS_CD]) ")
    '                'stb.Append(" 		 END                AS 機器分類 ")
    '                'stb.Append("        ,[M48_APPA_CLS] + ' : ' + [M06_APPACLS_NM]		AS 機器種別 ")
    '                'stb.Append(" 		,CASE [M48_SYSTEM_CD] ")
    '                'stb.Append(" 		   WHEN '11' THEN '11 : 磁気無線' ")
    '                'stb.Append(" 		   WHEN '12' THEN '12 : 磁気有線' ")
    '                'stb.Append(" 		   ELSE ISNULL([M48_SYSTEM_CD] + ' : ' + [M23_TBOXCLS_NM], [M48_SYSTEM_CD] ) ")
    '                'stb.Append(" 		 END					AS  システム ")
    '                'stb.Append(" 	    ,IIF([M48_DELETE_FLG] = '0', '', '●')  ")
    '                'stb.Append(" 	 					    	AS 削除 ")
    '                'stb.Append("        ,FORMAT([M48_INSERT_DT], 'yyyy/MM/dd HH:mm:ss')	AS 登録日時 ")
    '                'stb.Append("   FROM [SPCDB].[dbo].[M48_APPA] ")
    '                'stb.Append("   LEFT JOIN [SPCDB].[dbo].[M73_APPA_GROUPING] ")
    '                'stb.Append("        ON    M48_APPACLASS_CD = M73_APPACLASS_CD ")
    '                'stb.Append("   LEFT JOIN [SPCDB].[dbo].[M06_APPACLASS] ")
    '                'stb.Append("        ON   [M48_APPACLASS_CD] = [M06_APPACLASS_CD] ")
    '                'stb.Append("        AND  [M48_APPA_CLS]     = [M06_APPA_CLS] ")
    '                'stb.Append("   LEFT JOIN [SPCDB].[dbo].[M23_TBOXCLASS] ")
    '                'stb.Append("        ON   M48_SYSTEM_CD = M23_TBOXCLS  ")
    '                Return stb.ToString

    '            Case "検索"
    '                'stb.Append(" WHERE")
    '                'If intDelCls = 1 Then
    '                '    If Master.ppchksDel.Checked = False Then
    '                '        stb.Append(" " & TableName.Substring(0, 4) & "DELETE_FLG = 0 ")
    '                '    Else
    '                '        stb.Append(" " & TableName.Substring(0, 4) & "DELETE_FLG IN ( '0' , '1' ) ")
    '                '    End If
    '                'End If
    '                'TBOXID()
    '                'Dim intFROM As Integer
    '                'Dim intTO As Integer
    '                'If txtsAPPACDfrom.Text = "" Then
    '                '    If txtsAPPACDto.Text = "" Then
    '                '    Else '-To
    '                '        If Integer.TryParse(txtsAPPACDto.Text, intTO) = True Then
    '                '            stb.Append(" AND M48_APPA_CD <= " & intTO & "")
    '                '        Else
    '                '            stb.Append(" AND 1 = 2 ")
    '                '        End If
    '                '    End If
    '                'Else
    '                '    If txtsAPPACDto.Text = "" Then
    '                '        ' From-
    '                '        If Integer.TryParse(txtsAPPACDfrom.Text, intFROM) = True Then
    '                '            stb.Append(" AND M48_APPA_CD = " & intFROM & " ")
    '                '        Else
    '                '            stb.Append(" AND 1 = 2 ")
    '                '        End If
    '                '    Else 'From TO
    '                '        If Integer.TryParse(txtsAPPACDfrom.Text, intFROM) = True And Integer.TryParse(txtsAPPACDto.Text, intTO) = True Then
    '                '            stb.Append(" AND " & intFROM & " <= M48_APPA_CD AND  M48_APPA_CD <= " & intTO & "")
    '                '        Else
    '                '            stb.Append(" AND 1 = 2 ")
    '                '        End If
    '                '    End If
    '                'End If
    '                'stb.Append(ClsSQL.GeneStrSearch("[M48_APPA_NM]", txtsAPPANM.Text, "部分"))
    '                'stb.Append(ClsSQL.GeneStrSearch("[M48_SHORT_NM]", txtsSHORT.Text, "部分"))
    '                'stb.Append(ClsSQL.GeneStrSearch("[M48_APPACLASS_CD]", ddlsAPPAGroup.SelectedValue, "完全"))
    '                'stb.Append(ClsSQL.GeneStrSearch("[M48_APPA_CLS]", ddlsAPPACLS.SelectedValue, "完全"))
    '                'stb.Append(ClsSQL.GeneStrSearch("[M48_SYSTEM_CD]", ddlsSYS.SelectedValue, "完全"))
    '                ''登録したカラムからSQL生成
    '                'stb.Append(ClsSQL.GeneSQLCommand("WHERE", TableName, GeneArray(""), GeneArray("更新", "sValue")))
    '                'stb.Replace("WHERE AND", "WHERE")
    '                Return stb.ToString

    '            Case "追加"
    '                'stb.Append(" INSERT INTO [SPCDB].[dbo].[M48_APPA] ")
    '                'stb.Append("            ([M48_PROD_CD] ")
    '                'stb.Append("            ,[M48_APPA_CD] ")
    '                'stb.Append("            ,[M48_APPA_NM] ")
    '                'stb.Append("            ,[M48_SHORT_NM] ")
    '                'stb.Append("            ,[M48_APPA_CLS] ")
    '                'stb.Append("            ,[M48_VERSION] ")
    '                'stb.Append("            ,[M48_MODEL_NO] ")
    '                'stb.Append("            ,[M48_APPACLASS_CD] ")
    '                'stb.Append("            ,[M48_SYSTEM_CD] ")
    '                'stb.Append("            ,[M48_TBOX_VER] ")
    '                'stb.Append("            ,[M48_HDD_NO] ")
    '                'stb.Append("            ,[M48_HDD_CLS] ")
    '                'stb.Append("            ,[M48_CNSTDET_CLS] ")
    '                'stb.Append("            ,[M48_TOMASUSE_NM] ")
    '                'stb.Append("            ,[M48_DELETE_FLG] ")
    '                'stb.Append("            ,[M48_INSERT_DT] ")
    '                'stb.Append("            ,[M48_INSERT_USR] ")
    '                'stb.Append("            ) VALUES ( ")
    '                'stb.Append(" 		     '" & txtAPPACD.Text & "' ")
    '                'stb.Append("            ,'" & txtAPPACD.Text & "' ")
    '                'stb.Append("            ,'" & txtAPPANM.Text & "' ")
    '                'stb.Append("            ,'" & txtSHORTNM.Text & "' ")
    '                'stb.Append("            ,'" & ddlAPPACLS.SelectedValue & "' ")
    '                'stb.Append("            ,'" & ClsSQL.pfCnvEmpStr(txtVERSION.Text) & "' ")
    '                'stb.Append("            ,'" & ClsSQL.pfCnvEmpStr(txtMODELNo.Text) & "' ")
    '                'stb.Append("            ,'" & ddlAPPAGroup.SelectedValue & "' ")
    '                'stb.Append("            ,'" & ClsSQL.pfCnvEmpStr(ddlSYS.SelectedValue) & "' ")
    '                'stb.Append("            ,'" & ClsSQL.pfCnvEmpStr(ddlVer.SelectedValue) & "' ")
    '                'stb.Append("            ,'" & ClsSQL.pfCnvEmpStr(ddlHDDNo.SelectedValue) & "' ")
    '                'stb.Append("            ,'" & ClsSQL.pfCnvEmpStr(ddlHDDCLS.SelectedValue) & "' ")
    '                'stb.Append("            ,'" & ClsSQL.pfCnvEmpStr(ddlCNSTDET.SelectedValue) & "' ")
    '                'stb.Append("            ,'" & ClsSQL.pfCnvEmpStr(txtTOMAS.Text) & "' ")
    '                'stb.Append("            ,	'0' ")
    '                'stb.Append("            ,	'" & DateTime.Now & "' ")
    '                'stb.Append("            ,	'" & User.Identity.Name & "'  ")
    '                'stb.Append("            ) ")
    '                'stb.Replace("'NULL'", "NULL")
    '                Return stb.ToString

    '            Case "更新"
    '                'stb.Append(" UPDATE [SPCDB].[dbo].[M48_APPA] ")
    '                'stb.Append("    SET [M48_PROD_CD]      = '" & txtAPPACD.Text & "' ")
    '                'stb.Append("       ,[M48_APPA_CD]      = '" & txtAPPACD.Text & "' ")
    '                'stb.Append("       ,[M48_APPA_NM]      = '" & txtAPPANM.Text & "' ")
    '                'stb.Append("       ,[M48_SHORT_NM]     = '" & txtSHORTNM.Text & "' ")
    '                'If ddlAPPACLS.Visible = True Then
    '                '    stb.Append("       ,[M48_APPA_CLS]     = '" & ddlAPPACLS.SelectedValue & "' ")
    '                'End If
    '                'stb.Append("       ,[M48_VERSION]      = '" & ClsSQL.pfCnvEmpStr(txtVERSION.Text) & "' ")
    '                'stb.Append("       ,[M48_MODEL_NO]     = '" & ClsSQL.pfCnvEmpStr(txtMODELNo.Text) & "' ")
    '                'stb.Append("       ,[M48_APPACLASS_CD] = '" & ddlAPPAGroup.SelectedValue & "' ")
    '                'If ddlSYS.Visible = True Then
    '                '    stb.Append("       ,[M48_SYSTEM_CD]    = '" & ClsSQL.pfCnvEmpStr(ddlSYS.SelectedValue) & "' ")
    '                'End If
    '                'stb.Append("       ,[M48_TBOX_VER]     = '" & ClsSQL.pfCnvEmpStr(ddlVer.SelectedValue) & "' ")
    '                'stb.Append("       ,[M48_HDD_NO]       = '" & ClsSQL.pfCnvEmpStr(ddlHDDNo.SelectedValue) & "' ")
    '                'stb.Append("       ,[M48_HDD_CLS]      = '" & ClsSQL.pfCnvEmpStr(ddlHDDCLS.SelectedValue) & "' ")
    '                'If ddlCNSTDET.SelectedIndex > 0 Then
    '                '    stb.Append("       ,[M48_CNSTDET_CLS]  = '" & ClsSQL.pfCnvEmpStr(ddlCNSTDET.SelectedValue) & "' ")
    '                'End If
    '                'stb.Append("       ,[M48_TOMASUSE_NM]  = '" & ClsSQL.pfCnvEmpStr(txtTOMAS.Text) & "' ")
    '                'stb.Append(", " & TableName.Substring(0, 4) & "DELETE_FLG = '" & ClsSQL.GetDltCheckNum(chkDEL) & "'")
    '                'stb.Append(", " & TableName.Substring(0, 4) & "UPDATE_DT  = '" & DateTime.Now & "'")
    '                'stb.Append(", " & TableName.Substring(0, 4) & "UPDATE_USR = '" & User.Identity.Name & "'")
    '                'stb.Append("  WHERE  ")
    '                'stb.Append(ClsSQL.GeneStrSearch(KeyName1, ViewState("KeyName1"), "完全", ""))
    '                'stb.Append(ClsSQL.GeneStrSearch(KeyName2, ViewState("KeyName2"), "完全"))
    '                'stb.Append(ClsSQL.GeneStrSearch(KeyName3, ViewState("KeyName3"), "完全"))
    '                'stb.Append(ClsSQL.GeneStrSearch(KeyName4, ViewState("KeyName4"), "完全"))
    '                'stb.Append(ClsSQL.GeneStrSearch(KeyName5, ViewState("KeyName5"), "完全"))
    '                'stb.Replace("'NULL'", "NULL")
    '                Return stb.ToString

    '            Case "削除"
    '                'Select Case intDelCls
    '                '    Case 0
    '                '        Exit Select
    '                '    Case 1
    '                '        stb.Append("UPDATE " & TableName & " SET ")
    '                '        stb.Append("" & TableName.Substring(0, 4) & "DELETE_FLG = 1, ")
    '                '        stb.Append("" & TableName.Substring(0, 4) & "UPDATE_DT = '" & DateTime.Now & "', ")
    '                '        stb.Append("" & TableName.Substring(0, 4) & "UPDATE_USR = '" & User.Identity.Name & "' ")
    '                '    Case 2
    '                '        stb.Append("DELETE FROM " & TableName & "  ")
    '                'End Select
    '                'stb.Append("WHERE  ")
    '                'stb.Append(ClsSQL.GeneStrSearch(KeyName1, ViewState("KeyName1"), "完全", ""))
    '                'stb.Append(ClsSQL.GeneStrSearch(KeyName2, ViewState("KeyName2"), "完全"))
    '                'stb.Append(ClsSQL.GeneStrSearch(KeyName3, ViewState("KeyName3"), "完全"))
    '                'stb.Append(ClsSQL.GeneStrSearch(KeyName4, ViewState("KeyName4"), "完全"))
    '                'stb.Append(ClsSQL.GeneStrSearch(KeyName5, ViewState("KeyName5"), "完全"))
    '                Return stb.ToString

    '            Case "KEY" '選択したデータのキー情報
    '                'stb.Clear()
    '                'stb.Append("WHERE  ")
    '                'stb.Append(ClsSQL.GeneStrSearch(KeyName1, ViewState("KeyName1"), "完全", ""))
    '                'stb.Append(ClsSQL.GeneStrSearch(KeyName2, ViewState("KeyName2"), "完全"))
    '                'stb.Append(ClsSQL.GeneStrSearch(KeyName3, ViewState("KeyName3"), "完全"))
    '                'stb.Append(ClsSQL.GeneStrSearch(KeyName4, ViewState("KeyName4"), "完全"))
    '                'stb.Append(ClsSQL.GeneStrSearch(KeyName5, ViewState("KeyName5"), "完全"))
    '                Return stb.ToString

    '            Case "EditDataKey" '追加したデータのキー情報
    '                'stb.Clear()
    '                'stb.Append("WHERE  ")
    '                'stb.Append(ClsSQL.GeneStrSearch(KeyName1, txtAPPACD.Text, "完全", ""))
    '                'stb.Append(ClsSQL.GeneStrSearch(KeyName2, "", "完全"))
    '                'stb.Append(ClsSQL.GeneStrSearch(KeyName3, "", "完全"))
    '                'stb.Append(ClsSQL.GeneStrSearch(KeyName4, "", "完全"))
    '                'stb.Append(ClsSQL.GeneStrSearch(KeyName5, "", "完全"))
    '                Return stb.ToString

    '            Case Else
    '                Return -1
    '        End Select
    '    End Get

    'End Property

    ' ''' <summary>
    ' ''' キー入力時制御
    ' ''' </summary>
    'Private Sub inputSelectData(ByVal sender As Control, ByVal e As EventArgs)
    '    'フォーカス設定
    '    sender.Focus()
    '    'キー項目入力制御
    '    If txtAPPACD.Text = "" Then
    '        '未入力キー項目がある場合、キー項目以外を非活性
    '        setControl("Default")
    '    Else
    '        'キー入力値検証
    '        Me.Validate("key")
    '        If Me.IsValid = False Then
    '            Exit Sub
    '        End If
    '        stb.Clear()
    '        stb.Append(" SELECT [M48_APPA_CD] ")
    '        stb.Append("       ,[M48_APPA_NM] ")
    '        stb.Append("       ,[M48_SHORT_NM] ")
    '        stb.Append("       ,[M48_APPA_CLS] ")
    '        stb.Append("       ,[M48_DELETE_FLG] ")
    '        stb.Append("       ,[M48_VERSION] ")
    '        stb.Append("       ,[M48_MODEL_NO] ")
    '        stb.Append("       ,[M48_APPACLASS_CD] ")
    '        stb.Append("       ,[M48_SYSTEM_CD] ")
    '        stb.Append("       ,[M48_TBOX_VER] ")
    '        stb.Append("       ,[M48_HDD_NO] ")
    '        stb.Append("       ,[M48_HDD_CLS] ")
    '        stb.Append("       ,[M48_CNSTDET_CLS] ")
    '        stb.Append("       ,[M48_TOMASUSE_NM] ")
    '        stb.Append("   FROM [SPCDB].[dbo].[M48_APPA] ")
    '        stb.Append("  WHERE [M48_APPA_CD] = '" & txtAPPACD.Text & "' ")
    '        dt = ClsSQL.getDataSetTable(stb.ToString, "SelectedRow")
    '        If dt.Rows.Count = 0 Then
    '            BtnClear_Click()
    '            txtAPPANM.Enabled = True
    '            txtSHORTNM.Enabled = True
    '            ddlAPPAGroup.Enabled = True
    '            ddlAPPACLS.Enabled = False
    '            txtVERSION.Enabled = True
    '            txtMODELNo.Enabled = True
    '            ddlHDDNo.Enabled = False
    '            ddlHDDCLS.Enabled = False
    '            ddlSYS.Enabled = True
    '            ddlVer.Enabled = False
    '            ddlCNSTDET.Enabled = True
    '            txtTOMAS.Enabled = True
    '            mstrDispMode = "ADD"
    '            'フォーカス設定
    '            FocusChange(sender, txtAPPANM)
    '        Else
    '            '選択したデータのキーの登録&退避
    '            Dim arKey As String() = {KeyName1, KeyName2, KeyName3, KeyName4, KeyName5}
    '            Dim strKeyValue As String() = {"", "", "", "", ""}
    '            For i As Integer = 0 To arKey.Count - 1
    '                If arKey(i) = "" Then
    '                    Exit For
    '                Else
    '                    Me.ViewState.Add("KeyName" & i + 1 & "", dt(0)(arKey(i)).ToString)
    '                    strKeyValue(i) = dt(0)(arKey(i)).ToString
    '                End If
    '            Next
    '            '排他制御処理
    '            If fExclusive(strKeyValue) = False Then
    '                clsMst.psMesBox(Me, "30010", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)
    '                mstrDispMode = "DEF"
    '                'BtnClear_Click()
    '                Exit Sub
    '            End If
    '            setControl("UPD")
    '            '編集エリアに値を反映
    '            txtAPPACD.Text = dt(0)("M48_APPA_CD").ToString
    '            txtAPPANM.Text = dt(0)("M48_APPA_NM").ToString
    '            txtSHORTNM.Text = dt(0)("M48_SHORT_NM").ToString
    '            If ddlAPPAGroup.Items.FindByValue(dt(0)("M48_APPACLASS_CD").ToString.Trim) Is Nothing Then
    '                ddlAPPAGroup.Items.Item(0).Value = dt(0)("M48_APPACLASS_CD").ToString.Trim
    '                ddlAPPAGroup.Items.Item(0).Text = dt(0)("M48_APPACLASS_CD").ToString.Trim
    '                'ddlAPPAGroup.Items.Item(0).Text = CType(rowData.FindControl("機器分類"), TextBox).Text.Trim
    '                ddlAPPAGroup.SelectedIndex = 0
    '            Else
    '                ddlAPPAGroup.Items.Item(0).Value = ""
    '                ddlAPPAGroup.Items.Item(0).Text = ""
    '                ddlAPPAGroup.SelectedValue = dt(0)("M48_APPACLASS_CD").ToString
    '            End If
    '            ddlAPPAGroup_SelectedIndexChanged()
    '            If ClsSQL.GetRecord("SELECT M23_TBOXCLS FROM M23_TBOXCLASS WHERE M23_TBOXCLS = '" & dt(0)("M48_SYSTEM_CD").ToString & "' AND M23_DELETE_FLG   = '0' ") <= 0 _
    '                                And dt(0)("M48_SYSTEM_CD").ToString <> "11" And dt(0)("M48_SYSTEM_CD").ToString <> "12" Then
    '                '存在しないシステムコードの場合項目追加
    '                ddlSYS.SelectedIndex = 0
    '                ddlSYS.SelectedItem.Text = dt(0)("M48_SYSTEM_CD").ToString
    '                ddlSYS.Items.Item(0).Value = dt(0)("M48_SYSTEM_CD").ToString
    '                ddlSYS_SelectedIndexChanged()
    '            Else
    '                ddlSYS.SelectedValue = dt(0)("M48_SYSTEM_CD").ToString
    '                ddlSYS.Items.Item(0).Text = ""
    '                ddlSYS.Items.Item(0).Value = ""
    '                ddlSYS_SelectedIndexChanged()
    '            End If
    '            stb.Clear()
    '            stb.Append(" SELECT M06_APPACLASS_CD FROM M06_APPACLASS ")
    '            stb.Append("  WHERE M06_APPACLASS_CD = '" & dt(0)("M48_APPACLASS_CD").ToString & "'")
    '            stb.Append("    AND M06_APPA_CLS     = '" & dt(0)("M48_APPA_CLS").ToString & "'")
    '            stb.Append("    AND M06_DELETE_FLG   = '0'")
    '            If ClsSQL.GetRecordCount(stb.ToString) <= 0 Then
    '                ddlAPPACLS.SelectedIndex = 0
    '                ddlAPPACLS.SelectedItem.Text = dt(0)("M48_APPA_CLS").ToString
    '                ddlAPPACLS.Items.Item(0).Value = dt(0)("M48_APPA_CLS").ToString
    '                ddlAPPACLS_SelectedIndexChanged()
    '            Else
    '                ddlAPPACLS.SelectedValue = dt(0)("M48_APPA_CLS").ToString
    '                ddlAPPACLS.Items.Item(0).Text = ""
    '                ddlAPPACLS.Items.Item(0).Value = ""
    '                ddlAPPACLS_SelectedIndexChanged()
    '            End If
    '            txtVERSION.Text = dt(0)("M48_VERSION").ToString
    '            txtMODELNo.Text = dt(0)("M48_MODEL_NO").ToString
    '            If ddlVer.Items.FindByValue(dt(0)("M48_TBOX_VER").ToString.Trim) Is Nothing Then
    '                ddlVer.Items.Item(0).Value = dt(0)("M48_TBOX_VER").ToString.Trim
    '                ddlVer.Items.Item(0).Text = dt(0)("M48_TBOX_VER").ToString.Trim
    '                ddlVer.SelectedIndex = 0
    '            Else
    '                ddlVer.Items.Item(0).Value = ""
    '                ddlVer.Items.Item(0).Text = ""
    '                ddlVer.SelectedValue = dt(0)("M48_TBOX_VER").ToString
    '            End If
    '            If ddlHDDNo.Items.FindByValue(dt(0)("M48_HDD_NO").ToString.Trim) Is Nothing Then
    '                ddlHDDNo.Items.Item(0).Value = dt(0)("M48_HDD_NO").ToString.Trim
    '                ddlHDDNo.Items.Item(0).Text = dt(0)("M48_HDD_NO").ToString.Trim
    '                ddlHDDNo.SelectedIndex = 0
    '            Else
    '                ddlHDDNo.Items.Item(0).Value = ""
    '                ddlHDDNo.Items.Item(0).Text = ""
    '                ddlHDDNo.SelectedValue = dt(0)("M48_HDD_NO").ToString
    '            End If
    '            If ddlHDDCLS.Items.FindByValue(dt(0)("M48_HDD_CLS").ToString.Trim) Is Nothing Then
    '                ddlHDDCLS.Items.Item(0).Value = dt(0)("M48_HDD_CLS").ToString.Trim
    '                ddlHDDCLS.Items.Item(0).Text = dt(0)("M48_HDD_CLS").ToString.Trim
    '                ddlHDDCLS.SelectedIndex = 0
    '            Else
    '                ddlHDDCLS.Items.Item(0).Value = ""
    '                ddlHDDCLS.Items.Item(0).Text = ""
    '                ddlHDDCLS.SelectedValue = dt(0)("M48_HDD_CLS").ToString
    '            End If
    '            txtTOMAS.Text = dt(0)("M48_TOMASUSE_NM").ToString
    '            Dim M29CLASS As String
    '            Select Case hdnSYS.Value
    '                Case "1"
    '                    M29CLASS = "0118"
    '                Case Else
    '                    M29CLASS = "0120"
    '            End Select
    '            If ClsSQL.GetRecordCount("SELECT M29_CODE FROM M29_CLASS WHERE M29_CLASS_CD = '" & M29CLASS & "' AND M29_CODE = '" & dt(0)("M48_CNSTDET_CLS").ToString & "' ") <= 0 Then
    '                ddlCNSTDET.SelectedIndex = 0
    '                ddlCNSTDET.SelectedItem.Text = dt(0)("M48_CNSTDET_CLS").ToString
    '                ddlCNSTDET.Items.Item(0).Value = dt(0)("M48_CNSTDET_CLS").ToString
    '            Else
    '                ddlCNSTDET.SelectedValue = dt(0)("M48_CNSTDET_CLS").ToString
    '                ddlCNSTDET.Items.Item(0).Text = ""
    '                ddlCNSTDET.Items.Item(0).Value = ""
    '            End If
    '            Select Case intDelCls
    '                Case 1
    '                    Select Case dt(0)("" & TableName.Substring(0, 4) & "DELETE_FLG").ToString
    '                        Case "0"
    '                            Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '削除
    '                            Master.ppBtnDelete.Text = "削除"
    '                            Master.ppMainEnabled = True
    '                        Case "1"
    '                            Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
    '                            Master.ppBtnDelete.Text = "削除取消"
    '                            Master.ppMainEnabled = False
    '                    End Select
    '                Case Else
    '            End Select
    '            'ボタンの使用制限
    '            Select Case Master.ppBtnDelete.Text
    '                Case "削除"
    '                    mstrDispMode = "UPD"
    '                    'フォーカス設定
    '                    'FocusChange(sender, txtAPPANM)
    '                Case Else
    '                    mstrDispMode = "DEL"
    '                    'フォーカス設定
    '                    Master.ppBtnClear.Focus()
    '            End Select
    '        End If
    '    End If
    'End Sub
#End Region

    ''' <summary>
    ''' フォーカス移動
    ''' </summary>
    Private Sub FocusChange(ByVal objCtrlF As Control, ByVal objCtrlT As Control)

        Dim strBtnDmy As String = Master.ppBtnDmy.ClientID
        Master.ppBtnDmy.Visible = True

        Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + objCtrlT.ClientID + ");"
        SetFocus(Master.ppBtnDmy.ClientID)

    End Sub

    Private Sub sAmount_Format(ByVal ipstrPrice As Single, ByRef cpobjLabel As TextBox)

        Dim sglbuff As Single
        Dim strBuff As String

        If Single.TryParse(ipstrPrice, sglbuff) Then
            If sglbuff <> 0 Then
                strBuff = Integer.Parse(Single.Parse(ipstrPrice)).ToString("#,##0")
                '                cpobjLabel.Text = strBuff.PadLeft(7)
                cpobjLabel.Text = strBuff
            Else
                '                cpobjLabel.Text = (0).ToString.PadLeft(7)
                cpobjLabel.Text = "0"
            End If
        Else
            cpobjLabel.Text = cstSpace7
        End If

    End Sub

#End Region

    'txtSrchCODE
    'txtSrchCNST_NM
    'txtCODE
    'txtCNST_NM
    'cdbSUMSTART_D
    'txtNML_PRICE
    'txtHLDY_PRICE
    'txtNGHT_PRICE
    'ddlL_CLS
    'ddlM_CLS


#Region "終了処理プロシージャ"

#End Region

End Class

