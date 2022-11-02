'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　工事区分マスタ管理
'*　ＰＧＭＩＤ：　COMUPDM92
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.11.18　：　伯野
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SQL_DBCLS_LIB
Imports System.Threading


Public Class COMUPDM92

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
    Const DispCode As String = "COMUPDM92"             '画面ID
    'マスタ情報
    Const MasterName As String = "工事区分マスタ"           'マスタ名
    Const TableName As String = "M92_DISPLAY_ITEM"              'テーブル名
    'キー情報
    Const KeyName1 As String = "M92_CNST_CLS"
    Const KeyName2 As String = "M92_TBOXCLS_CD"
    Const KeyName3 As String = "M92_SEQNO"
    Const KeyName4 As String = "M92_CNSTCKS_CD"
    Const KeyName5 As String = ""
    Const intDelCls As Integer = 0         '削除種別 0:削除なし  1:削除フラグ  2:DELETE
    'ソート情報
    Const SortKey As String = " ORDER BY " & KeyName1 & "   "
    '検索条件項目
    Const cstSrchKey_CNSTCLS As String = "ddlsrchCNST_CLS"
    Const cstSrchKey_TBOXCLS As String = "ddlsrchTBOXCLS_CD"
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

        'Response.Write("<div id='mydiv' >")
        'Response.Write("_")
        'Response.Write("</div>")
        'Response.Write("<script>mydiv.innerText = '';</script>")

        'Response.Write("<script language=javascript>;")
        'Response.Write("var dots = 0;var dotmax = 10;function ShowWait()")
        'Response.Write("{var output; output = 'Loading';dots++;if(dots>=dotmax)dots=1;")
        'Response.Write("for(var x = 0;x < dots;x++){output += '.';}mydiv.innerText =  output;}")
        'Response.Write("function StartShowWait(){mydiv.style.visibility = 'visible';window.setInterval('ShowWait()',1000);}")
        'Response.Write("function HideWait(){mydiv.style.visibility = 'hidden';window.clearInterval();}")
        'Response.Write("StartShowWait();</script>")
        'Response.Flush()
        'Thread.Sleep(10000)

        mclsDB.mstrConString = ConfigurationManager.ConnectionStrings("SPCDB").ToString
        If mclsDB.pfDB_Connect() = False Then
            'ＤＢ接続失敗
        End If

        ViewState(cstSrchKey_CNSTCLS) = ddlSrchCNST_CLS.ppSelectedValue
        ViewState(cstSrchKey_TBOXCLS) = ddlSrchTBOXCLS_CD.ppSelectedValue
        ViewState("txtCnst_Cls") = txtCnst_Cls.ppText
        ViewState("txtCnstCls_NM") = txtCnstCls_NM.ppText
        ViewState("ddlTBOXCLS_CD") = ddlTBOXCLS_CD.ppSelectedValue
        ViewState("txtSEQNO") = txtSEQNO.ppText
        ViewState("ddlWork_Cls") = ddlWork_Cls.ppSelectedValue
        ViewState("ChkEMGNCY_FLG") = ChkEMGNCY_FLG.Checked
        ViewState("lblNml_Amount") = lblNml_Amount.Text
        ViewState("lblHld_Amount") = lblHld_Amount.Text
        ViewState("lblMdn_Amount") = lblMdn_Amount.Text

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
            Me.ddlSrchCNST_CLS.Focus()
        End If

        Call ddlSrchCnst_CLS_DataBind()
        Call ddlSrchTBOX_CLS_DataBind()
        Call ddlWork_CD_DataBind()

        'ボタンイベント設定
        AddHandler Master.ppBtnSrcClear.Click, AddressOf BtnSearchClear_Click
        AddHandler Master.ppBtnSearch.Click, AddressOf BtnSearch_Click
        AddHandler Master.ppBtnClear.Click, AddressOf BtnClear_Click
        AddHandler Master.ppBtnInsert.Click, AddressOf BtnInsert_Click
        AddHandler Master.ppBtnUpdate.Click, AddressOf BtnUpdate_Click
        AddHandler Master.ppBtnDelete.Click, AddressOf BtnDelete_Click

        AddHandler txtCnst_Cls.ppTextBox.TextChanged, AddressOf txtCnst_Cls_TextChanged
        AddHandler txtSEQNO.ppTextBox.TextChanged, AddressOf txtSEQNO_TextChanged
        AddHandler ddlWork_Cls.ppDropDownList.TextChanged, AddressOf ddlWork_Cls_SelectedIndexChanged

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
            ddlSrchCNST_CLS.ppSelectedValue = ViewState(cstSrchKey_CNSTCLS)
        End If
        If ViewState(cstSrchKey_TBOXCLS) Is Nothing Then
        Else
            ddlSrchTBOXCLS_CD.ppSelectedValue = ViewState(cstSrchKey_TBOXCLS)
        End If
        If ViewState("txtCnst_Cls") Is Nothing Then
        Else
            txtCnst_Cls.ppText = ViewState("txtCnst_Cls")
        End If
        If ViewState("txtCnstCls_NM") Is Nothing Then
        Else
            txtCnstCls_NM.ppText = ViewState("txtCnstCls_NM")
        End If
        If ViewState("ddlTBOXCLS_CD") Is Nothing Then
        Else
            ddlTBOXCLS_CD.ppSelectedValue = ViewState("ddlTBOXCLS_CD")
        End If
        If ViewState("txtSEQNO") Is Nothing Then
        Else
            txtSEQNO.ppText = ViewState("txtSEQNO")
        End If
        If ViewState("ddlWork_Cls") Is Nothing Then
        Else
            ddlWork_Cls.ppSelectedValue = ViewState("ddlWork_Cls")
        End If
        If ViewState("ChkEMGNCY_FLG") Is Nothing Then
        Else
            ChkEMGNCY_FLG.Checked = ViewState("ChkEMGNCY_FLG")
        End If
        If ViewState("lblNml_Amount") Is Nothing Then
        Else
            lblNml_Amount.Text = ViewState("lblNml_Amount")
        End If
        If ViewState("lblHld_Amount") Is Nothing Then
        Else
            lblHld_Amount.Text = ViewState("lblHld_Amount")
        End If
        If ViewState("lblMdn_Amount") Is Nothing Then
            lblMdn_Amount.Text = ViewState("lblMdn_Amount")
        End If

        '閉じるボタンでEXIT
        If Master.ppCount = "close" OrElse mstrDispMode = "EXIT" Then
            Exit Sub
        End If

        'ボタンの使用制限
        Select Case mstrDispMode
            Case "DEF", "First"
                '初期　追加
                Me.ddlSrchCNST_CLS.ppEnabled = True
                Me.ddlSrchTBOXCLS_CD.ppEnabled = True
                Me.txtCnst_Cls.ppEnabled = True
                Me.txtCnstCls_NM.ppEnabled = True
                Me.ddlTBOXCLS_CD.ppSelectedValue = ""
                Me.ddlTBOXCLS_CD.ppEnabled = True
                Me.txtSEQNO.ppEnabled = True
                Me.ChkEMGNCY_FLG.Enabled = True
                Me.ddlWork_Cls.ppSelectedValue = ""
                Me.ddlWork_Cls.ppEnabled = True
                Me.lblNml_Amount.Text = ""
                Me.lblHld_Amount.Text = ""
                Me.lblMdn_Amount.Text = ""
                Me.lblAmount_CLS.Text = ""
                Master.ppBtnInsert.Enabled = True
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnClear.Enabled = True
                Master.ppBtnDelete.Enabled = False
                Master.ppCount = "0"
            Case "ADD"
                '初期　追加
                Me.ddlSrchCNST_CLS.ppEnabled = False
                Me.ddlSrchTBOXCLS_CD.ppEnabled = False
                Me.txtCnst_Cls.ppEnabled = True
                Me.txtCnstCls_NM.ppEnabled = True
                Me.ddlTBOXCLS_CD.ppEnabled = True
                Me.txtSEQNO.ppEnabled = True
                Me.ddlWork_Cls.ppEnabled = True
                Me.ChkEMGNCY_FLG.Enabled = True
                '                Me.lblNml_Amount.Text = ""
                '                Me.lblHld_Amount.Text = ""
                '                Me.lblMdn_Amount.Text = ""
                '                Me.lblAmount_CLS.Text = ""
                Master.ppBtnClear.Enabled = True
                Master.ppBtnInsert.Enabled = True
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnDelete.Enabled = False
            Case "UPD", "SEL"
                '選択後　更新　削除
                Me.ddlSrchCNST_CLS.ppEnabled = False
                Me.ddlSrchTBOXCLS_CD.ppEnabled = False
                Me.txtCnst_Cls.ppEnabled = True
                Me.txtCnstCls_NM.ppEnabled = True
                Me.ddlTBOXCLS_CD.ppEnabled = True
                Me.txtSEQNO.ppEnabled = True
                Me.ddlWork_Cls.ppEnabled = True
                '                Me.lblNml_Amount.Text = ""
                '                Me.lblHld_Amount.Text = ""
                '                Me.lblMdn_Amount.Text = ""
                '                Me.lblAmount_CLS.Text = ""
                '                'TOMAS名称変更禁止
                '                txtTOMAS.Enabled = False
                'If txtTOMAS.Text.Trim = "" Then
                '    txtTOMAS.Enabled = False
                'End If
                'txtAPPANM.Focus()
                Master.ppBtnInsert.Enabled = False
                Master.ppBtnUpdate.Enabled = True
                Master.ppBtnClear.Enabled = True
                Master.ppBtnDelete.Enabled = True
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
                Me.ddlSrchCNST_CLS.ppEnabled = True
                Me.ddlSrchTBOXCLS_CD.ppEnabled = True
                Me.txtCnst_Cls.ppEnabled = True
                Me.txtCnstCls_NM.ppEnabled = True
                Me.ddlTBOXCLS_CD.ppSelectedValue = ""
                Me.ddlTBOXCLS_CD.ppEnabled = True
                Me.txtSEQNO.ppEnabled = True
                Me.ChkEMGNCY_FLG.Enabled = True
                Me.ddlWork_Cls.ppSelectedValue = ""
                Me.ddlWork_Cls.ppEnabled = True
                Me.lblNml_Amount.Text = ""
                Me.lblHld_Amount.Text = ""
                Me.lblMdn_Amount.Text = ""
                Me.lblAmount_CLS.Text = ""
                Master.ppBtnInsert.Enabled = True
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnClear.Enabled = True
                Master.ppBtnDelete.Enabled = False
        End Select

        Me.ddlSrchCNST_CLS.ppEnabled = True
        Me.ddlSrchTBOXCLS_CD.ppEnabled = True

        Master.ppBtnDelete.Visible = False

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
        Me.txtCnst_Cls.ppText = ""
        Me.txtCnstCls_NM.ppText = ""
        Me.ddlTBOXCLS_CD.ppSelectedValue = ""
        Me.txtSEQNO.ppText = ""
        Me.ddlWork_Cls.ppSelectedValue = ""

        ViewState("txtCnst_Cls") = ""
        ViewState("txtCnstCls_NM") = ""
        ViewState("ddlTBOXCLS_CD") = ""
        ViewState("txtSEQNO") = ""
        ViewState("ddlWork_Cls") = ""
        ViewState("ChkEMGNCY_FLG") = False
        ViewState("lblNml_Amount") = ""
        ViewState("lblHld_Amount") = ""
        ViewState("lblMdn_Amount") = ""

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
        Me.txtCnst_Cls.Focus()

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

        Me.Validate("val")
        If Me.IsValid = False Then
            Exit Sub
        End If

        Dim objSQLCmd As New SqlClient.SqlCommand
        Dim objDBTran As SqlClient.SqlTransaction = mclsDB.mobjDB.BeginTransaction
        Dim intRetCD As Integer = 0
        Dim strMesType As String = "00001"

        objSQLCmd.Connection = mclsDB.mobjDB
        objSQLCmd.CommandText = "COMUPDM92_I1"
        objSQLCmd.Parameters.Add("prmM92_CNST_CLS", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM92_CNSTCLS_NM", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM92_TBOXCLS_CD", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM92_SEQNO", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM92_CNSTCKS_CD", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM92_EMGNCY_FLG", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM92_INSERT_USR", SqlDbType.NVarChar)

        Dim strWKBuff As String = ""
        strWKBuff = ""
        If Me.txtCnst_Cls.ppText Is DBNull.Value Then
            strWKBuff = ""
        ElseIf Me.txtCnst_Cls.ppText.Trim = "" Then
            strWKBuff = ""
        Else
            strWKBuff = Me.txtCnst_Cls.ppText.Trim
        End If
        objSQLCmd.Parameters("prmM92_CNST_CLS").Value = strWKBuff

        strWKBuff = ""
        If Me.txtCnstCls_NM.ppText Is DBNull.Value Then
            strWKBuff = ""
        ElseIf Me.txtCnstCls_NM.ppText.Trim = "" Then
            strWKBuff = ""
        Else
            strWKBuff = Me.txtCnstCls_NM.ppText.Trim
        End If
        objSQLCmd.Parameters("prmM92_CNSTCLS_NM").Value = strWKBuff

        strWKBuff = ""
        If ViewState("ddlTBOXCLS_CD") Is Nothing Then
            strWKBuff = ""
        Else
            strWKBuff = ViewState("ddlTBOXCLS_CD")
        End If
        objSQLCmd.Parameters("prmM92_TBOXCLS_CD").Value = strWKBuff

        strWKBuff = ""
        If Me.txtSEQNO.ppText Is DBNull.Value Then
            strWKBuff = ""
        ElseIf Me.txtSEQNO.ppText.Trim = "" Then
            strWKBuff = ""
        Else
            strWKBuff = Me.txtSEQNO.ppText.Trim
        End If
        objSQLCmd.Parameters("prmM92_SEQNO").Value = strWKBuff

        strWKBuff = ""
        If ViewState("ddlWork_Cls") Is Nothing Then
            strWKBuff = ""
        Else
            strWKBuff = ViewState("ddlWork_Cls")
        End If
        objSQLCmd.Parameters("prmM92_CNSTCKS_CD").Value = strWKBuff

        strWKBuff = ""
        If Me.ChkEMGNCY_FLG.Checked = False Then
            strWKBuff = "0"
        Else
            strWKBuff = "1"
        End If
        objSQLCmd.Parameters("prmM92_EMGNCY_FLG").Value = strWKBuff

        objSQLCmd.Parameters("prmM92_INSERT_USR").Value = User.Identity.Name

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
                objSQLCmd.CommandText = "COMUPDM92_S4"
                objSQLCmd.Parameters.Add("ddlsrchCNST_CLS", SqlDbType.NVarChar, 2)
                objSQLCmd.Parameters.Add("ddlsrchTBOXCLS_CD", SqlDbType.NVarChar, 2)
                objSQLCmd.Parameters.Add("SEQNO", SqlDbType.NVarChar, 2)
                objSQLCmd.Parameters.Add("CNSTCKS_CD", SqlDbType.NVarChar, 4)
                objSQLCmd.Parameters("ddlsrchCNST_CLS").Value = ViewState("txtCnst_Cls")
                objSQLCmd.Parameters("ddlsrchTBOXCLS_CD").Value = ViewState("ddlTBOXCLS_CD")
                objSQLCmd.Parameters("SEQNO").Value = ViewState("txtSEQNO")
                objSQLCmd.Parameters("CNSTCKS_CD").Value = ViewState("ddlWork_Cls")
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
        Me.txtCnst_Cls.Focus()
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

        Me.Validate("val")
        If Me.IsValid = False Then
            Exit Sub
        End If

        Dim objSQLCmd As New SqlClient.SqlCommand
        Dim objDBTran As SqlClient.SqlTransaction = mclsDB.mobjDB.BeginTransaction
        Dim intRetCD As Integer = 0
        Dim strMesType As String = "00001"

        objSQLCmd.Connection = mclsDB.mobjDB
        objSQLCmd.CommandText = "COMUPDM92_U1"
        objSQLCmd.Parameters.Add("prmM92_CNST_CLS", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM92_CNSTCLS_NM", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM92_TBOXCLS_CD", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM92_SEQNO", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM92_CNSTCKS_CD", SqlDbType.Decimal)
        objSQLCmd.Parameters.Add("prmM92_EMGNCY_FLG", SqlDbType.NVarChar)
        objSQLCmd.Parameters.Add("prmM92_UPDATE_USR", SqlDbType.NVarChar)

        Dim strWKBuff As String = ""
        strWKBuff = ""
        If Me.txtCnst_Cls.ppText Is DBNull.Value Then
            strWKBuff = ""
        ElseIf Me.txtCnst_Cls.ppText.Trim = "" Then
            strWKBuff = ""
        Else
            strWKBuff = Me.txtCnst_Cls.ppText.Trim
        End If
        objSQLCmd.Parameters("prmM92_CNST_CLS").Value = strWKBuff

        strWKBuff = ""
        If Me.txtCnstCls_NM.ppText Is DBNull.Value Then
            strWKBuff = ""
        ElseIf Me.txtCnstCls_NM.ppText.Trim = "" Then
            strWKBuff = ""
        Else
            strWKBuff = Me.txtCnstCls_NM.ppText.Trim
        End If
        objSQLCmd.Parameters("prmM92_CNSTCLS_NM").Value = strWKBuff

        strWKBuff = ""
        If ViewState("ddlTBOXCLS_CD") Is Nothing Then
            strWKBuff = ""
        Else
            strWKBuff = ViewState("ddlTBOXCLS_CD")
        End If
        objSQLCmd.Parameters("prmM92_TBOXCLS_CD").Value = strWKBuff

        strWKBuff = ""
        If Me.txtSEQNO.ppText Is DBNull.Value Then
            strWKBuff = ""
        ElseIf Me.txtSEQNO.ppText.Trim = "" Then
            strWKBuff = ""
        Else
            strWKBuff = Me.txtSEQNO.ppText.Trim
        End If
        objSQLCmd.Parameters("prmM92_SEQNO").Value = strWKBuff

        strWKBuff = ""
        If ViewState("ddlWork_Cls") Is Nothing Then
            strWKBuff = ""
        ElseIf ViewState("ddlWork_Cls") = "" Then
            strWKBuff = ""
        Else
            strWKBuff = ViewState("ddlWork_Cls")
        End If
        objSQLCmd.Parameters("prmM92_CNSTCKS_CD").Value = strWKBuff

        strWKBuff = ""
        If Me.ChkEMGNCY_FLG.Checked = False Then
            strWKBuff = "0"
        Else
            strWKBuff = "1"
        End If
        objSQLCmd.Parameters("prmM92_EMGNCY_FLG").Value = strWKBuff

        objSQLCmd.Parameters("prmM92_UPDATE_USR").Value = User.Identity.Name

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
                objSQLCmd.CommandText = "COMUPDM92_S4"
                objSQLCmd.Parameters.Add("ddlsrchCNST_CLS", SqlDbType.NVarChar, 2)
                objSQLCmd.Parameters.Add("ddlsrchTBOXCLS_CD", SqlDbType.NVarChar, 2)
                objSQLCmd.Parameters.Add("SEQNO", SqlDbType.NVarChar, 2)
                objSQLCmd.Parameters.Add("CNSTCKS_CD", SqlDbType.NVarChar, 4)
                objSQLCmd.Parameters("ddlsrchCNST_CLS").Value = Me.txtCnst_Cls.ppText.Trim
                objSQLCmd.Parameters("ddlsrchTBOXCLS_CD").Value = ViewState("ddlTBOXCLS_CD")
                objSQLCmd.Parameters("SEQNO").Value = Me.txtSEQNO.ppText.Trim
                objSQLCmd.Parameters("CNSTCKS_CD").Value = ViewState("ddlWork_Cls")
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
        Me.txtCnst_Cls.Focus()
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
                objSQLCmd.CommandText = "COMUPDM92_D1"
                objSQLCmd.Parameters.Add("prmM92_CNST_CLS", SqlDbType.NVarChar)
                objSQLCmd.Parameters.Add("prmM92_TBOXCLS_CD", SqlDbType.NVarChar)
                objSQLCmd.Parameters.Add("prmM92_SEQNO", SqlDbType.NVarChar)
                objSQLCmd.Parameters.Add("prmM92_CNSTCKS_CD", SqlDbType.Decimal)

                Dim strWKBuff As String = ""
                strWKBuff = ""
                If Me.txtCnst_Cls.ppText Is DBNull.Value Then
                    strWKBuff = ""
                ElseIf Me.txtCnst_Cls.ppText.Trim = "" Then
                    strWKBuff = ""
                Else
                    strWKBuff = Me.txtCnst_Cls.ppText.Trim
                End If
                objSQLCmd.Parameters("prmM92_CNST_CLS").Value = strWKBuff

                strWKBuff = ""
                If ViewState("ddlTBOXCLS_CD") Is Nothing Then
                    strWKBuff = ""
                ElseIf ViewState("ddlTBOXCLS_CD") = "" Then
                    strWKBuff = ""
                Else
                    strWKBuff = ViewState("ddlTBOXCLS_CD")
                End If
                objSQLCmd.Parameters("prmM92_TBOXCLS_CD").Value = strWKBuff

                strWKBuff = ""
                If Me.txtSEQNO.ppText Is DBNull.Value Then
                    strWKBuff = ""
                ElseIf Me.txtSEQNO.ppText.Trim = "" Then
                    strWKBuff = ""
                Else
                    strWKBuff = Me.txtSEQNO.ppText.Trim
                End If
                objSQLCmd.Parameters("prmM92_SEQNO").Value = strWKBuff

                strWKBuff = ""
                If ViewState("ddlWork_Cls") Is Nothing Then
                    strWKBuff = ""
                ElseIf ViewState("ddlWork_Cls") = "" Then
                    strWKBuff = ""
                Else
                    strWKBuff = ViewState("ddlWork_Cls")
                End If
                objSQLCmd.Parameters("prmM92_CNSTCKS_CD").Value = strWKBuff

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
                        objSQLCmd.CommandText = "COMUPDM92_S4"
                        objSQLCmd.Parameters.Add("ddlsrchCNST_CLS", SqlDbType.NVarChar, 2)
                        objSQLCmd.Parameters.Add("ddlsrchTBOXCLS_CD", SqlDbType.NVarChar, 2)
                        objSQLCmd.Parameters.Add("SEQNO", SqlDbType.NVarChar, 2)
                        objSQLCmd.Parameters.Add("CNSTCKS_CD", SqlDbType.NVarChar, 4)
                        objSQLCmd.Parameters("ddlsrchCNST_CLS").Value = Me.txtCnst_Cls.ppText.Trim
                        objSQLCmd.Parameters("ddlsrchTBOXCLS_CD").Value = ViewState("ddlTBOXCLS_CD")
                        objSQLCmd.Parameters("SEQNO").Value = Me.txtSEQNO.ppText.Trim
                        objSQLCmd.Parameters("CNSTCKS_CD").Value = ViewState("ddlWork_Cls")
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

                'データバインド
                Master.ppCount = "0"
                grvList.DataSource = New DataTable
                grvList.DataBind()

                '項目の初期化
                BtnClear_Click()



                'ログ出力終了
                psLogEnd(Me)
                Exit Select
            Case Else
                Select Case Master.ppBtnDelete.Text
                    Case "削除"

                        objSQLCmd.Connection = mclsDB.mobjDB
                        objSQLCmd.CommandText = "COMUPDM92_D1"
                        objSQLCmd.Parameters.Add("prmM92_CNST_CLS", SqlDbType.NVarChar)
                        objSQLCmd.Parameters.Add("prmM92_TBOXCLS_CD", SqlDbType.NVarChar)
                        objSQLCmd.Parameters.Add("prmM92_SEQNO", SqlDbType.NVarChar)
                        objSQLCmd.Parameters.Add("prmM92_CNSTCKS_CD", SqlDbType.Decimal)

                        Dim strWKBuff As String = ""
                        strWKBuff = ""
                        If Me.txtCnst_Cls.ppText Is DBNull.Value Then
                            strWKBuff = ""
                        ElseIf Me.txtCnst_Cls.ppText.Trim = "" Then
                            strWKBuff = ""
                        Else
                            strWKBuff = Me.txtCnst_Cls.ppText.Trim
                        End If
                        objSQLCmd.Parameters("prmM92_CNST_CLS").Value = strWKBuff

                        strWKBuff = ""
                        If Me.ddlTBOXCLS_CD.ppSelectedValue = "" Then
                            strWKBuff = ""
                        Else
                            strWKBuff = Me.ddlTBOXCLS_CD.ppSelectedValue.Trim
                        End If
                        objSQLCmd.Parameters("prmM92_TBOXCLS_CD").Value = strWKBuff

                        strWKBuff = ""
                        If Me.txtSEQNO.ppText Is DBNull.Value Then
                            strWKBuff = ""
                        ElseIf Me.txtSEQNO.ppText.Trim = "" Then
                            strWKBuff = ""
                        Else
                            strWKBuff = Me.txtSEQNO.ppText.Trim
                        End If
                        objSQLCmd.Parameters("prmM92_SEQNO").Value = strWKBuff

                        strWKBuff = ""
                        If Me.ddlWork_Cls.ppSelectedValue Is DBNull.Value Then
                            strWKBuff = ""
                        ElseIf Me.ddlWork_Cls.ppSelectedValue.Trim = "" Then
                            strWKBuff = ""
                        Else
                            strWKBuff = Me.ddlWork_Cls.ppSelectedValue.Trim
                        End If
                        objSQLCmd.Parameters("prmM92_CNSTCKS_CD").Value = strWKBuff

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
                                objSQLCmd.CommandText = "COMUPDM92_S4"
                                objSQLCmd.Parameters.Add("ddlsrchCNST_CLS", SqlDbType.NVarChar, 2)
                                objSQLCmd.Parameters.Add("ddlsrchTBOXCLS_CD", SqlDbType.NVarChar, 2)
                                objSQLCmd.Parameters.Add("SEQNO", SqlDbType.NVarChar, 2)
                                objSQLCmd.Parameters.Add("CNSTCKS_CD", SqlDbType.NVarChar, 4)
                                objSQLCmd.Parameters("ddlsrchCNST_CLS").Value = Me.txtCnst_Cls.ppText.Trim
                                objSQLCmd.Parameters("ddlsrchTBOXCLS_CD").Value = Me.ddlTBOXCLS_CD.ppSelectedValue.ToString.Trim
                                objSQLCmd.Parameters("SEQNO").Value = Me.txtSEQNO.ppText.Trim
                                objSQLCmd.Parameters("CNSTCKS_CD").Value = Me.ddlWork_Cls.ppSelectedValue.ToString.Trim
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

        'フォーカス設定
        txtSEQNO.Focus()

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

        ViewState(cstSrchKey_CNSTCLS) = ddlSrchCNST_CLS.ppSelectedValue
        ViewState(cstSrchKey_TBOXCLS) = ddlSrchTBOXCLS_CD.ppSelectedValue

        'フォーカス設定
        Me.ddlSrchCNST_CLS.Focus()

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
        Me.ddlSrchCNST_CLS.Focus()

        'ログ出力終了
        psLogEnd(Me)

    End Sub
#End Region

    '----------------------------------------------------------------------------------------------------------------------------
    '-　ボタン以外のコントロール制御
    '----------------------------------------------------------------------------------------------------------------------------
#Region "ボタン以外のコントロール制御"

    ''' <summary>
    ''' 工事区分コード変更後
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtCnst_Cls_TextChanged(sender As Object, e As EventArgs)

        Dim strProcFlg As String = ""

        'ログ出力開始
        psLogStart(Me)

        If Me.txtCnst_Cls.ppText Is DBNull.Value Then
            strProcFlg = "CLR"
        ElseIf txtCnst_Cls.ppText.Trim = "" Then
            strProcFlg = "CLR"
        Else
            strProcFlg = "CHK"
        End If

        Select Case strProcFlg
            Case "CLR"
                If Me.txtCnstCls_NM.ppTextBox.ReadOnly = True Then
                    Me.txtCnstCls_NM.ppTextBox.Text = ""
                    Me.txtCnstCls_NM.ppTextBox.ReadOnly = False
                End If
            Case "CHK"
                Dim objLstItem As ListItem
                objLstItem = ddlSrchCNST_CLS.ppDropDownList.Items.FindByValue(Me.txtCnst_Cls.ppText)
                If objLstItem Is Nothing Then
                    Me.txtCnstCls_NM.ppTextBox.ReadOnly = False
                    ddlTBOXCLS_CD.Focus()
                    txtCnstCls_NM.Focus()
                ElseIf objLstItem.Text.Trim = "" Then
                    Me.txtCnstCls_NM.ppTextBox.ReadOnly = False
                    ddlTBOXCLS_CD.Focus()
                    txtCnstCls_NM.Focus()
                Else
                    Me.txtCnstCls_NM.ppText = objLstItem.Text.Substring(3).Trim
                    ViewState("txtCnstCls_NM") = objLstItem.Text.Substring(3).Trim
                    Me.txtCnstCls_NM.ppTextBox.ReadOnly = True
                    ddlTBOXCLS_CD.Focus()
                End If
        End Select

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 連番変更後
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtSEQNO_TextChanged(sender As Object, e As EventArgs)

        Dim strResult As String = ""

        Call sGet_AmountInfo(sender.ID, strResult)

        If strResult = "NASI" Then
            If mstrDispMode = "UPD" Then
            Else
                mstrDispMode = "ADD"
            End If
        ElseIf strResult = "ARI" Then
            mstrDispMode = "UPD"
        ElseIf strResult = "ARI2" Then
            mstrDispMode = "ADD"
        Else
        End If

    End Sub

    ''' <summary>
    ''' 工事名の変更後
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlWork_Cls_SelectedIndexChanged(sender As Object, e As EventArgs)

        Dim strResult As String = ""

        Call sGet_AmountInfo(sender.ID, strResult)

        If strResult = "NASI" Then
            If mstrDispMode = "UPD" Then
            Else
                mstrDispMode = "ADD"
            End If
        ElseIf strResult = "ARI" Then
            mstrDispMode = "UPD"
        ElseIf strResult = "ARI2" Then
            mstrDispMode = "ADD"
        Else
        End If

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
                    objSQLCmd.CommandText = "COMUPDM92_S4"
                    objSQLCmd.Parameters.Add(cstSrchKey_CNSTCLS, SqlDbType.NVarChar, 2)
                    objSQLCmd.Parameters.Add(cstSrchKey_TBOXCLS, SqlDbType.NVarChar, 2)
                    objSQLCmd.Parameters.Add("SEQNO", SqlDbType.NVarChar, 2)
                    objSQLCmd.Parameters.Add("CNSTCKS_CD", SqlDbType.NVarChar, 4)
                    objSQLCmd.Parameters(cstSrchKey_CNSTCLS).Value = CType(rowData.FindControl("CM92_CNST_CLS"), TextBox).Text
                    objSQLCmd.Parameters(cstSrchKey_TBOXCLS).Value = CType(rowData.FindControl("CM92_TBOXCLS_CD"), TextBox).Text
                    objSQLCmd.Parameters("SEQNO").Value = CType(rowData.FindControl("CM92_SEQNO"), TextBox).Text
                    objSQLCmd.Parameters("CNSTCKS_CD").Value = CType(rowData.FindControl("CM92_CNSTCKS_CD"), TextBox).Text
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

                    Me.txtCnst_Cls.ppText = objwkDS.Tables(0).Rows(0).Item("CM92_CNST_CLS").ToString
                    Me.txtCnstCls_NM.ppText = objwkDS.Tables(0).Rows(0).Item("CM92_CNSTCLS_NM").ToString
                    Me.ddlTBOXCLS_CD.ppSelectedValue = objwkDS.Tables(0).Rows(0).Item("CM92_TBOXCLS_CD").ToString
                    Me.txtSEQNO.ppText = objwkDS.Tables(0).Rows(0).Item("CM92_SEQNO").ToString
                    Me.ddlWork_Cls.ppSelectedValue = objwkDS.Tables(0).Rows(0).Item("CM92_CNSTCKS_CD").ToString
                    If objwkDS.Tables(0).Rows(0).Item("CM92_EMGNCY_FLG").ToString = "1" Then
                        Me.ChkEMGNCY_FLG.Checked = True
                    Else
                        Me.ChkEMGNCY_FLG.Checked = False
                    End If
                    If objwkDS.Tables(0).Rows(0).Item("CM48_NML_PRICE") Is DBNull.Value Then
                        Me.lblNml_Amount.Text = cstSpace7
                    Else
                        Call sAmount_Format(objwkDS.Tables(0).Rows(0).Item("CM48_NML_PRICE").ToString, lblNml_Amount)
                    End If
                    If objwkDS.Tables(0).Rows(0).Item("CM48_HLDY_PRICE") Is DBNull.Value Then
                        Me.lblHld_Amount.Text = cstSpace7
                    Else
                        Call sAmount_Format(objwkDS.Tables(0).Rows(0).Item("CM48_HLDY_PRICE").ToString, lblHld_Amount)
                    End If
                    If objwkDS.Tables(0).Rows(0).Item("CM48_NGHT_PRICE") Is DBNull.Value Then
                        Me.lblMdn_Amount.Text = cstSpace7
                    Else
                        Call sAmount_Format(objwkDS.Tables(0).Rows(0).Item("CM48_NGHT_PRICE").ToString, lblMdn_Amount)
                    End If
                    '                    Me.txtAmount_CLS.Text = objwkDS.Tables(0).Rows(0).Item("CM48_CLASS").ToString

                    ViewState("txtCnst_Cls") = objwkDS.Tables(0).Rows(0).Item("CM92_CNST_CLS").ToString
                    ViewState("txtCnstCls_NM") = objwkDS.Tables(0).Rows(0).Item("CM92_CNSTCLS_NM").ToString
                    ViewState("ddlTBOXCLS_CD") = objwkDS.Tables(0).Rows(0).Item("CM92_TBOXCLS_CD").ToString
                    ViewState("txtSEQNO") = objwkDS.Tables(0).Rows(0).Item("CM92_SEQNO").ToString
                    ViewState("ddlWork_Cls") = objwkDS.Tables(0).Rows(0).Item("CM92_CNSTCKS_CD").ToString
                    If objwkDS.Tables(0).Rows(0).Item("CM92_EMGNCY_FLG").ToString = "1" Then
                        ViewState("ChkEMGNCY_FLG") = True
                    Else
                        ViewState("ChkEMGNCY_FLG") = False
                    End If
                    'ViewState("lblNml_Amount") = objwkDS.Tables(0).Rows(0).Item("M48_NML_PRICE").ToString
                    'ViewState("lblHld_Amount") = objwkDS.Tables(0).Rows(0).Item("M48_HLDY_PRICE").ToString
                    'ViewState("lblMdn_Amount") = objwkDS.Tables(0).Rows(0).Item("M48_NGHT_PRICE").ToString
                    ViewState("lblNml_Amount") = Me.lblNml_Amount.Text
                    ViewState("lblHld_Amount") = Me.lblHld_Amount.Text
                    ViewState("lblMdn_Amount") = Me.lblMdn_Amount.Text

                    'ボタンの使用制限
                    mstrDispMode = "SEL"
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

                    Me.txtCnst_Cls.Focus()

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
    '        objwkCHK = DirectCast(rowData.FindControl("CM92_EMGNCY_FLG"), CheckBox)
    '        Dim rowView As DataRowView = CType(rowData.DataItem, DataRowView)
    '        Dim state As String = rowView("CM92_EMGNCY_FLG").ToString()
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
    ''' グリッドビューの行ごとの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grvList.RowDataBound

        Dim strBuff As String = ""
        Dim rowData As GridViewRow

        rowData = e.Row

        If rowData.RowType = DataControlRowType.DataRow Then

            Dim objwkTXT As TextBox
            Dim sglBuff As Single
            '通常料金のカンマ編集
            objwkTXT = Nothing
            objwkTXT = CType(rowData.FindControl("CM48_NML_PRICE"), TextBox)
            strBuff = objwkTXT.Text
            If Single.TryParse(strBuff, sglBuff) Then
                If sglBuff <> 0 Then
                    objwkTXT.Text = Integer.Parse(sglBuff).ToString("#,##0")
                End If
                If Single.Parse(strBuff) = 0 Then
                    objwkTXT.Text = "0"
                End If
            Else
                objwkTXT.Text = ""
            End If
            '休日料金のカンマ編集
            objwkTXT = Nothing
            objwkTXT = DirectCast(rowData.FindControl("CM48_HLDY_PRICE"), TextBox)
            strBuff = objwkTXT.Text
            If Single.TryParse(strBuff, sglBuff) Then
                If sglBuff <> 0 Then
                    objwkTXT.Text = Integer.Parse(sglBuff).ToString("#,##0")
                End If
                If Single.Parse(strBuff) = 0 Then
                    objwkTXT.Text = "0"
                End If
            Else
                objwkTXT.Text = ""
            End If
            '夜間料金のカンマ編集
            objwkTXT = Nothing
            objwkTXT = DirectCast(rowData.FindControl("CM48_NGHT_PRICE"), TextBox)
            strBuff = objwkTXT.Text
            If Single.TryParse(strBuff, sglBuff) Then
                If sglBuff <> 0 Then
                    objwkTXT.Text = Integer.Parse(sglBuff).ToString("#,##0")
                End If
                If Single.Parse(strBuff) = 0 Then
                    objwkTXT.Text = "0"
                End If
            Else
                objwkTXT.Text = ""
            End If
            'チェックボックスの値を設定
            Dim objwkCHK As CheckBox
            objwkCHK = Nothing
            objwkCHK = DirectCast(rowData.FindControl("CM92_EMGNCY_FLG"), CheckBox)
            Dim objrowView As DataRowView = CType(rowData.DataItem, DataRowView)
            Dim strDataVal As String = objrowView("CM92_EMGNCY_FLG").ToString()
            If strDataVal = "1" Then
                objwkCHK.Checked = True
            Else
                objwkCHK.Checked = False
            End If

        End If

    End Sub

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
    ' ''' 工事区分 チェック
    ' ''' </summary>
    ' ''' <param name="source"></param>
    ' ''' <param name="args"></param>
    ' ''' <remarks></remarks>
    'Private Sub CstmVal_txtCnst_Cls_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_txtCnst_Cls.ServerValidate

    '    Dim dtrErrMes As DataRow

    '    Dim strCntlName As String = "工事区分"
    '    source.ControlToValidate = txtCnst_Cls.ID

    '    If ViewState("txtCnst_Cls") Is Nothing Then
    '    ElseIf ViewState("txtCnst_Cls") = String.Empty Then
    '        dtrErrMes = ClsCMCommon.pfGet_ValMes("5001", strCntlName)
    '        CstmVal_txtSEQNO.Text = P_VALMES_AST
    '        CstmVal_txtSEQNO.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
    '        CstmVal_txtSEQNO.IsValid = False
    '        args.IsValid = False
    '        Exit Sub
    '    End If

    '    If Regex.IsMatch(txtSEQNO.Text, "^[0-9]+$") = False Then
    '        dtrErrMes = ClsCMCommon.pfGet_ValMes("4001", strCntlName)
    '        CstmVal_txtSEQNO.Text = P_VALMES_AST
    '        CstmVal_txtSEQNO.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
    '        CstmVal_txtSEQNO.IsValid = False
    '        args.IsValid = False
    '        Exit Sub
    '    End If

    'End Sub

    ' ''' <summary>
    ' ''' 工事名　チェック
    ' ''' </summary>
    ' ''' <param name="source"></param>
    ' ''' <param name="args"></param>
    ' ''' <remarks></remarks>
    'Private Sub CstmVal_txtCnstCls_NM_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_txtCnstCls_NM.ServerValidate

    '    Dim dtrErrMes As DataRow

    '    Dim strCntlName As String = "工事名"
    '    source.ControlToValidate = txtCnst_Cls.ID

    '    If ViewState("txtCnstCls_NM") Is Nothing Then
    '    ElseIf ViewState("txtCnstCls_NM") = String.Empty Then
    '        dtrErrMes = ClsCMCommon.pfGet_ValMes("5001", strCntlName)
    '        CstmVal_txtSEQNO.Text = P_VALMES_AST
    '        CstmVal_txtSEQNO.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
    '        CstmVal_txtSEQNO.IsValid = False
    '        args.IsValid = False
    '        Exit Sub
    '    End If

    'End Sub

    ' ''' <summary>
    ' ''' 連番　チェック
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Protected Sub CstmVal_txtSEQNO_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_txtSEQNO.ServerValidate

    '    Dim dtrErrMes As DataRow

    '    Dim strCntlName As String = "連番"
    '    source.ControlToValidate = txtSEQNO.ID

    '    If txtSEQNO.Text = String.Empty Then
    '        dtrErrMes = ClsCMCommon.pfGet_ValMes("5001", strCntlName)
    '        CstmVal_txtSEQNO.Text = P_VALMES_AST
    '        CstmVal_txtSEQNO.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
    '        CstmVal_txtSEQNO.IsValid = False
    '        args.IsValid = False
    '        Exit Sub
    '    End If

    '    If Regex.IsMatch(txtSEQNO.Text, "^[0-9]+$") = False Then
    '        dtrErrMes = ClsCMCommon.pfGet_ValMes("4001", strCntlName)
    '        CstmVal_txtSEQNO.Text = P_VALMES_AST
    '        CstmVal_txtSEQNO.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
    '        CstmVal_txtSEQNO.IsValid = False
    '        args.IsValid = False
    '        Exit Sub
    '    End If

    'End Sub

    ' ''' <summary>
    ' ''' 工事名　チェック
    ' ''' </summary>
    ' ''' <param name="source"></param>
    ' ''' <param name="args"></param>
    ' ''' <remarks></remarks>
    'Private Sub CstmVal_ddlWork_Cls_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_ddlWork_Cls.ServerValidate

    '    Dim blnErrFlg As Boolean = True
    '    Dim dtrErrMes As DataRow

    '    Dim strCntlName As String = "工事名"
    '    source.ControlToValidate = ddlWork_Cls.ID
    '    If ViewState("ddlWork_Cls") Is Nothing Then
    '    ElseIf ViewState("ddlWork_Cls") = String.Empty Then
    '    ElseIf ViewState("ddlWork_Cls") = "" Then
    '    Else
    '        blnErrFlg = False
    '    End If
    '    If blnErrFlg = True Then
    '        dtrErrMes = ClsCMCommon.pfGet_ValMes("5001", strCntlName)
    '        CstmVal_txtSEQNO.Text = P_VALMES_MES
    '        CstmVal_txtSEQNO.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
    '        CstmVal_txtSEQNO.IsValid = False
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
    ''' 検索条件の工事区分をバインド
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlSrchCnst_CLS_DataBind()

        Dim objSQLCmd As New SqlClient.SqlCommand
        Dim objwkDS As New DataSet

        objSQLCmd.Connection = mclsDB.mobjDB
        objSQLCmd.CommandText = "COMUPDM92_S1"
        If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
            'エラー
            clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "リスト作成")
            objStack = New StackFrame
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "リスト情報取得失敗", "Catch")
            Exit Sub
        End If

        If objwkDS.Tables.Count > 0 Then
            Me.ddlSrchCNST_CLS.ppDropDownList.DataSource = objwkDS.Tables(0)
            Me.ddlSrchCNST_CLS.ppDropDownList.DataTextField = "M92_CNSTCLS_NM_DISP"
            Me.ddlSrchCNST_CLS.ppDropDownList.DataValueField = "M92_CNST_CLS"
            Me.ddlSrchCNST_CLS.ppDropDownList.DataBind()
            Me.ddlSrchCNST_CLS.ppDropDownList.Items.Insert(0, "")
        End If

        Call mclsDB.psDisposeDataSet(objwkDS)

        'ClsSQL.psDropDownDataBind(ddlsAPPACLS, stb.ToString, "CLSNM", "CLS")
        '        ClsSQL.psDropDownDataBind(ddlScreen_ID, stb.ToString, "CLSNM", "CLS")

    End Sub

    ''' <summary>
    ''' 検索条件、編集エリアのＴＢＯＸシステムをバインド
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlSrchTBOX_CLS_DataBind()

        Dim objSQLCmd As New SqlClient.SqlCommand
        Dim objwkDS As New DataSet

        objSQLCmd.Connection = mclsDB.mobjDB
        objSQLCmd.CommandText = "COMUPDM92_S2"
        If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
            'エラー
            clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "リスト作成")
            objStack = New StackFrame
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "リスト情報取得失敗", "Catch")
            Exit Sub
        End If

        If objwkDS.Tables.Count > 0 Then
            '検索条件
            Me.ddlSrchTBOXCLS_CD.ppDropDownList.DataSource = objwkDS.Tables(0)
            Me.ddlSrchTBOXCLS_CD.ppDropDownList.DataTextField = "M23_TBOXCLS_NM_DISP"
            Me.ddlSrchTBOXCLS_CD.ppDropDownList.DataValueField = "M23_TBOXCLS"
            Me.ddlSrchTBOXCLS_CD.ppDropDownList.DataBind()
            Me.ddlSrchTBOXCLS_CD.ppDropDownList.Items.Insert(0, "")
            '編集エリア
            Me.ddlTBOXCLS_CD.ppDropDownList.DataSource = objwkDS.Tables(0)
            Me.ddlTBOXCLS_CD.ppDropDownList.DataTextField = "M23_TBOXCLS_NM_DISP"
            Me.ddlTBOXCLS_CD.ppDropDownList.DataValueField = "M23_TBOXCLS"
            Me.ddlTBOXCLS_CD.ppDropDownList.DataBind()
            Me.ddlTBOXCLS_CD.ppDropDownList.Items.Insert(0, "")
        End If

        Call mclsDB.psDisposeDataSet(objwkDS)

        'ClsSQL.psDropDownDataBind(ddlsAPPACLS, stb.ToString, "CLSNM", "CLS")
        '        ClsSQL.psDropDownDataBind(ddlScreen_ID, stb.ToString, "CLSNM", "CLS")

    End Sub

    Private Sub ddlWork_CD_DataBind()

        Dim objSQLCmd As New SqlClient.SqlCommand
        Dim objwkDS As New DataSet

        objSQLCmd.Connection = mclsDB.mobjDB
        objSQLCmd.CommandText = "COMUPDM92_S5"
        If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
            'エラー
            clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "リスト作成")
            objStack = New StackFrame
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "リスト情報取得失敗", "Catch")
            Exit Sub
        End If

        If objwkDS.Tables.Count > 0 Then
            '編集エリア
            Me.ddlWork_Cls.ppDropDownList.DataSource = objwkDS.Tables(0)
            Me.ddlWork_Cls.ppDropDownList.DataTextField = "M48_CNST_NM_DISP"
            Me.ddlWork_Cls.ppDropDownList.DataValueField = "M48_CODE"
            Me.ddlWork_Cls.ppDropDownList.DataBind()
            Me.ddlWork_Cls.ppDropDownList.Items.Insert(0, "")
        End If

        Call mclsDB.psDisposeDataSet(objwkDS)

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
            objSQLCmd.CommandText = "COMUPDM92_S3"
            objSQLCmd.Parameters.Add(cstSrchKey_CNSTCLS, SqlDbType.NVarChar)
            objSQLCmd.Parameters.Add(cstSrchKey_TBOXCLS, SqlDbType.NVarChar)
            Dim strprmValue As String = ""
            If ViewState(cstSrchKey_CNSTCLS) Is Nothing Then
                strprmValue = ""
            ElseIf ViewState(cstSrchKey_CNSTCLS) = "" Then
                strprmValue = ""
            Else
                strprmValue = ViewState(cstSrchKey_CNSTCLS)
            End If
            objSQLCmd.Parameters(cstSrchKey_CNSTCLS).Value = strprmValue
            strprmValue = ""
            If ViewState(cstSrchKey_TBOXCLS) Is Nothing Then
                strprmValue = ""
            ElseIf ViewState(cstSrchKey_TBOXCLS) = "" Then
                strprmValue = ""
            Else
                strprmValue = ViewState(cstSrchKey_TBOXCLS)
            End If
            objSQLCmd.Parameters(cstSrchKey_TBOXCLS).Value = strprmValue
            If mstrDispMode = "First" Then
                objSQLCmd.Parameters(cstSrchKey_CNSTCLS).Value = "**"
                objSQLCmd.Parameters(cstSrchKey_TBOXCLS).Value = "**"
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
    Private Sub setControl(strCase As String)

        Select Case strCase
            Case "Default", "Search"
                ClsSQL.ControlClear(ddlSrchCNST_CLS, True)
                ClsSQL.ControlClear(ddlSrchTBOXCLS_CD, True)
                ClsSQL.ControlClear(txtCnst_Cls, False)
                ClsSQL.ControlClear(ddlTBOXCLS_CD, False)
                ClsSQL.ControlClear(txtSEQNO, False)
                ClsSQL.ControlClear(lblNml_Amount, False)
                ClsSQL.ControlClear(lblHld_Amount, False)
                ClsSQL.ControlClear(lblMdn_Amount, False)
                ClsSQL.ControlClear(lblAmount_CLS, False)
                ClsSQL.ControlClear(ChkEMGNCY_FLG, False)
                ddlSrchCNST_CLS.ppEnabled = True
                ddlSrchTBOXCLS_CD.ppEnabled = True
            Case "UPD", "ADD", "DEL"
                ClsSQL.ControlClear(ddlSrchCNST_CLS, False)
                ClsSQL.ControlClear(ddlSrchTBOXCLS_CD, False)
                ClsSQL.ControlClear(txtCnst_Cls, True)
                ClsSQL.ControlClear(ddlTBOXCLS_CD, True)
                ClsSQL.ControlClear(txtSEQNO, True)
                ClsSQL.ControlClear(lblNml_Amount, True)
                ClsSQL.ControlClear(lblHld_Amount, True)
                ClsSQL.ControlClear(lblMdn_Amount, True)
                ClsSQL.ControlClear(lblAmount_CLS, True)
                ClsSQL.ControlClear(ChkEMGNCY_FLG, True)
                ddlSrchCNST_CLS.ppEnabled = False
                ddlSrchTBOXCLS_CD.ppEnabled = False
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
    '                'stb.Append("         [M92_APPA_CD]      AS 機器コード ")
    '                'stb.Append("        ,[M92_APPA_NM]		AS 機器名称 ")
    '                'stb.Append("        ,[M92_SHORT_NM]		AS 機器略称 ")
    '                'stb.Append("        ,CASE [M92_APPACLASS_CD] ")
    '                'stb.Append(" 		   WHEN '99' THEN '99 : その他'  ")
    '                'stb.Append(" 		   ELSE ISNULL([M92_APPACLASS_CD] + ' : ' + [M73_APPACLASS_NM], [M92_APPACLASS_CD]) ")
    '                'stb.Append(" 		 END                AS 機器分類 ")
    '                'stb.Append("        ,[M92_APPA_CLS] + ' : ' + [M06_APPACLS_NM]		AS 機器種別 ")
    '                'stb.Append(" 		,CASE [M92_SYSTEM_CD] ")
    '                'stb.Append(" 		   WHEN '11' THEN '11 : 磁気無線' ")
    '                'stb.Append(" 		   WHEN '12' THEN '12 : 磁気有線' ")
    '                'stb.Append(" 		   ELSE ISNULL([M92_SYSTEM_CD] + ' : ' + [M23_TBOXCLS_NM], [M92_SYSTEM_CD] ) ")
    '                'stb.Append(" 		 END					AS  システム ")
    '                'stb.Append(" 	    ,IIF([M92_DELETE_FLG] = '0', '', '●')  ")
    '                'stb.Append(" 	 					    	AS 削除 ")
    '                'stb.Append("        ,FORMAT([M92_INSERT_DT], 'yyyy/MM/dd HH:mm:ss')	AS 登録日時 ")
    '                'stb.Append("   FROM [SPCDB].[dbo].[M92_APPA] ")
    '                'stb.Append("   LEFT JOIN [SPCDB].[dbo].[M73_APPA_GROUPING] ")
    '                'stb.Append("        ON    M92_APPACLASS_CD = M73_APPACLASS_CD ")
    '                'stb.Append("   LEFT JOIN [SPCDB].[dbo].[M06_APPACLASS] ")
    '                'stb.Append("        ON   [M92_APPACLASS_CD] = [M06_APPACLASS_CD] ")
    '                'stb.Append("        AND  [M92_APPA_CLS]     = [M06_APPA_CLS] ")
    '                'stb.Append("   LEFT JOIN [SPCDB].[dbo].[M23_TBOXCLASS] ")
    '                'stb.Append("        ON   M92_SYSTEM_CD = M23_TBOXCLS  ")
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
    '                '            stb.Append(" AND M92_APPA_CD <= " & intTO & "")
    '                '        Else
    '                '            stb.Append(" AND 1 = 2 ")
    '                '        End If
    '                '    End If
    '                'Else
    '                '    If txtsAPPACDto.Text = "" Then
    '                '        ' From-
    '                '        If Integer.TryParse(txtsAPPACDfrom.Text, intFROM) = True Then
    '                '            stb.Append(" AND M92_APPA_CD = " & intFROM & " ")
    '                '        Else
    '                '            stb.Append(" AND 1 = 2 ")
    '                '        End If
    '                '    Else 'From TO
    '                '        If Integer.TryParse(txtsAPPACDfrom.Text, intFROM) = True And Integer.TryParse(txtsAPPACDto.Text, intTO) = True Then
    '                '            stb.Append(" AND " & intFROM & " <= M92_APPA_CD AND  M92_APPA_CD <= " & intTO & "")
    '                '        Else
    '                '            stb.Append(" AND 1 = 2 ")
    '                '        End If
    '                '    End If
    '                'End If
    '                'stb.Append(ClsSQL.GeneStrSearch("[M92_APPA_NM]", txtsAPPANM.Text, "部分"))
    '                'stb.Append(ClsSQL.GeneStrSearch("[M92_SHORT_NM]", txtsSHORT.Text, "部分"))
    '                'stb.Append(ClsSQL.GeneStrSearch("[M92_APPACLASS_CD]", ddlsAPPAGroup.SelectedValue, "完全"))
    '                'stb.Append(ClsSQL.GeneStrSearch("[M92_APPA_CLS]", ddlsAPPACLS.SelectedValue, "完全"))
    '                'stb.Append(ClsSQL.GeneStrSearch("[M92_SYSTEM_CD]", ddlsSYS.SelectedValue, "完全"))
    '                ''登録したカラムからSQL生成
    '                'stb.Append(ClsSQL.GeneSQLCommand("WHERE", TableName, GeneArray(""), GeneArray("更新", "sValue")))
    '                'stb.Replace("WHERE AND", "WHERE")
    '                Return stb.ToString

    '            Case "追加"
    '                'stb.Append(" INSERT INTO [SPCDB].[dbo].[M92_APPA] ")
    '                'stb.Append("            ([M92_PROD_CD] ")
    '                'stb.Append("            ,[M92_APPA_CD] ")
    '                'stb.Append("            ,[M92_APPA_NM] ")
    '                'stb.Append("            ,[M92_SHORT_NM] ")
    '                'stb.Append("            ,[M92_APPA_CLS] ")
    '                'stb.Append("            ,[M92_VERSION] ")
    '                'stb.Append("            ,[M92_MODEL_NO] ")
    '                'stb.Append("            ,[M92_APPACLASS_CD] ")
    '                'stb.Append("            ,[M92_SYSTEM_CD] ")
    '                'stb.Append("            ,[M92_TBOX_VER] ")
    '                'stb.Append("            ,[M92_HDD_NO] ")
    '                'stb.Append("            ,[M92_HDD_CLS] ")
    '                'stb.Append("            ,[M92_CNSTDET_CLS] ")
    '                'stb.Append("            ,[M92_TOMASUSE_NM] ")
    '                'stb.Append("            ,[M92_DELETE_FLG] ")
    '                'stb.Append("            ,[M92_INSERT_DT] ")
    '                'stb.Append("            ,[M92_INSERT_USR] ")
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
    '                'stb.Append(" UPDATE [SPCDB].[dbo].[M92_APPA] ")
    '                'stb.Append("    SET [M92_PROD_CD]      = '" & txtAPPACD.Text & "' ")
    '                'stb.Append("       ,[M92_APPA_CD]      = '" & txtAPPACD.Text & "' ")
    '                'stb.Append("       ,[M92_APPA_NM]      = '" & txtAPPANM.Text & "' ")
    '                'stb.Append("       ,[M92_SHORT_NM]     = '" & txtSHORTNM.Text & "' ")
    '                'If ddlAPPACLS.Visible = True Then
    '                '    stb.Append("       ,[M92_APPA_CLS]     = '" & ddlAPPACLS.SelectedValue & "' ")
    '                'End If
    '                'stb.Append("       ,[M92_VERSION]      = '" & ClsSQL.pfCnvEmpStr(txtVERSION.Text) & "' ")
    '                'stb.Append("       ,[M92_MODEL_NO]     = '" & ClsSQL.pfCnvEmpStr(txtMODELNo.Text) & "' ")
    '                'stb.Append("       ,[M92_APPACLASS_CD] = '" & ddlAPPAGroup.SelectedValue & "' ")
    '                'If ddlSYS.Visible = True Then
    '                '    stb.Append("       ,[M92_SYSTEM_CD]    = '" & ClsSQL.pfCnvEmpStr(ddlSYS.SelectedValue) & "' ")
    '                'End If
    '                'stb.Append("       ,[M92_TBOX_VER]     = '" & ClsSQL.pfCnvEmpStr(ddlVer.SelectedValue) & "' ")
    '                'stb.Append("       ,[M92_HDD_NO]       = '" & ClsSQL.pfCnvEmpStr(ddlHDDNo.SelectedValue) & "' ")
    '                'stb.Append("       ,[M92_HDD_CLS]      = '" & ClsSQL.pfCnvEmpStr(ddlHDDCLS.SelectedValue) & "' ")
    '                'If ddlCNSTDET.SelectedIndex > 0 Then
    '                '    stb.Append("       ,[M92_CNSTDET_CLS]  = '" & ClsSQL.pfCnvEmpStr(ddlCNSTDET.SelectedValue) & "' ")
    '                'End If
    '                'stb.Append("       ,[M92_TOMASUSE_NM]  = '" & ClsSQL.pfCnvEmpStr(txtTOMAS.Text) & "' ")
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
    '        stb.Append(" SELECT [M92_APPA_CD] ")
    '        stb.Append("       ,[M92_APPA_NM] ")
    '        stb.Append("       ,[M92_SHORT_NM] ")
    '        stb.Append("       ,[M92_APPA_CLS] ")
    '        stb.Append("       ,[M92_DELETE_FLG] ")
    '        stb.Append("       ,[M92_VERSION] ")
    '        stb.Append("       ,[M92_MODEL_NO] ")
    '        stb.Append("       ,[M92_APPACLASS_CD] ")
    '        stb.Append("       ,[M92_SYSTEM_CD] ")
    '        stb.Append("       ,[M92_TBOX_VER] ")
    '        stb.Append("       ,[M92_HDD_NO] ")
    '        stb.Append("       ,[M92_HDD_CLS] ")
    '        stb.Append("       ,[M92_CNSTDET_CLS] ")
    '        stb.Append("       ,[M92_TOMASUSE_NM] ")
    '        stb.Append("   FROM [SPCDB].[dbo].[M92_APPA] ")
    '        stb.Append("  WHERE [M92_APPA_CD] = '" & txtAPPACD.Text & "' ")
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
    '            txtAPPACD.Text = dt(0)("M92_APPA_CD").ToString
    '            txtAPPANM.Text = dt(0)("M92_APPA_NM").ToString
    '            txtSHORTNM.Text = dt(0)("M92_SHORT_NM").ToString
    '            If ddlAPPAGroup.Items.FindByValue(dt(0)("M92_APPACLASS_CD").ToString.Trim) Is Nothing Then
    '                ddlAPPAGroup.Items.Item(0).Value = dt(0)("M92_APPACLASS_CD").ToString.Trim
    '                ddlAPPAGroup.Items.Item(0).Text = dt(0)("M92_APPACLASS_CD").ToString.Trim
    '                'ddlAPPAGroup.Items.Item(0).Text = CType(rowData.FindControl("機器分類"), TextBox).Text.Trim
    '                ddlAPPAGroup.SelectedIndex = 0
    '            Else
    '                ddlAPPAGroup.Items.Item(0).Value = ""
    '                ddlAPPAGroup.Items.Item(0).Text = ""
    '                ddlAPPAGroup.SelectedValue = dt(0)("M92_APPACLASS_CD").ToString
    '            End If
    '            ddlAPPAGroup_SelectedIndexChanged()
    '            If ClsSQL.GetRecord("SELECT M23_TBOXCLS FROM M23_TBOXCLASS WHERE M23_TBOXCLS = '" & dt(0)("M92_SYSTEM_CD").ToString & "' AND M23_DELETE_FLG   = '0' ") <= 0 _
    '                                And dt(0)("M92_SYSTEM_CD").ToString <> "11" And dt(0)("M92_SYSTEM_CD").ToString <> "12" Then
    '                '存在しないシステムコードの場合項目追加
    '                ddlSYS.SelectedIndex = 0
    '                ddlSYS.SelectedItem.Text = dt(0)("M92_SYSTEM_CD").ToString
    '                ddlSYS.Items.Item(0).Value = dt(0)("M92_SYSTEM_CD").ToString
    '                ddlSYS_SelectedIndexChanged()
    '            Else
    '                ddlSYS.SelectedValue = dt(0)("M92_SYSTEM_CD").ToString
    '                ddlSYS.Items.Item(0).Text = ""
    '                ddlSYS.Items.Item(0).Value = ""
    '                ddlSYS_SelectedIndexChanged()
    '            End If
    '            stb.Clear()
    '            stb.Append(" SELECT M06_APPACLASS_CD FROM M06_APPACLASS ")
    '            stb.Append("  WHERE M06_APPACLASS_CD = '" & dt(0)("M92_APPACLASS_CD").ToString & "'")
    '            stb.Append("    AND M06_APPA_CLS     = '" & dt(0)("M92_APPA_CLS").ToString & "'")
    '            stb.Append("    AND M06_DELETE_FLG   = '0'")
    '            If ClsSQL.GetRecordCount(stb.ToString) <= 0 Then
    '                ddlAPPACLS.SelectedIndex = 0
    '                ddlAPPACLS.SelectedItem.Text = dt(0)("M92_APPA_CLS").ToString
    '                ddlAPPACLS.Items.Item(0).Value = dt(0)("M92_APPA_CLS").ToString
    '                ddlAPPACLS_SelectedIndexChanged()
    '            Else
    '                ddlAPPACLS.SelectedValue = dt(0)("M92_APPA_CLS").ToString
    '                ddlAPPACLS.Items.Item(0).Text = ""
    '                ddlAPPACLS.Items.Item(0).Value = ""
    '                ddlAPPACLS_SelectedIndexChanged()
    '            End If
    '            txtVERSION.Text = dt(0)("M92_VERSION").ToString
    '            txtMODELNo.Text = dt(0)("M92_MODEL_NO").ToString
    '            If ddlVer.Items.FindByValue(dt(0)("M92_TBOX_VER").ToString.Trim) Is Nothing Then
    '                ddlVer.Items.Item(0).Value = dt(0)("M92_TBOX_VER").ToString.Trim
    '                ddlVer.Items.Item(0).Text = dt(0)("M92_TBOX_VER").ToString.Trim
    '                ddlVer.SelectedIndex = 0
    '            Else
    '                ddlVer.Items.Item(0).Value = ""
    '                ddlVer.Items.Item(0).Text = ""
    '                ddlVer.SelectedValue = dt(0)("M92_TBOX_VER").ToString
    '            End If
    '            If ddlHDDNo.Items.FindByValue(dt(0)("M92_HDD_NO").ToString.Trim) Is Nothing Then
    '                ddlHDDNo.Items.Item(0).Value = dt(0)("M92_HDD_NO").ToString.Trim
    '                ddlHDDNo.Items.Item(0).Text = dt(0)("M92_HDD_NO").ToString.Trim
    '                ddlHDDNo.SelectedIndex = 0
    '            Else
    '                ddlHDDNo.Items.Item(0).Value = ""
    '                ddlHDDNo.Items.Item(0).Text = ""
    '                ddlHDDNo.SelectedValue = dt(0)("M92_HDD_NO").ToString
    '            End If
    '            If ddlHDDCLS.Items.FindByValue(dt(0)("M92_HDD_CLS").ToString.Trim) Is Nothing Then
    '                ddlHDDCLS.Items.Item(0).Value = dt(0)("M92_HDD_CLS").ToString.Trim
    '                ddlHDDCLS.Items.Item(0).Text = dt(0)("M92_HDD_CLS").ToString.Trim
    '                ddlHDDCLS.SelectedIndex = 0
    '            Else
    '                ddlHDDCLS.Items.Item(0).Value = ""
    '                ddlHDDCLS.Items.Item(0).Text = ""
    '                ddlHDDCLS.SelectedValue = dt(0)("M92_HDD_CLS").ToString
    '            End If
    '            txtTOMAS.Text = dt(0)("M92_TOMASUSE_NM").ToString
    '            Dim M29CLASS As String
    '            Select Case hdnSYS.Value
    '                Case "1"
    '                    M29CLASS = "0118"
    '                Case Else
    '                    M29CLASS = "0120"
    '            End Select
    '            If ClsSQL.GetRecordCount("SELECT M29_CODE FROM M29_CLASS WHERE M29_CLASS_CD = '" & M29CLASS & "' AND M29_CODE = '" & dt(0)("M92_CNSTDET_CLS").ToString & "' ") <= 0 Then
    '                ddlCNSTDET.SelectedIndex = 0
    '                ddlCNSTDET.SelectedItem.Text = dt(0)("M92_CNSTDET_CLS").ToString
    '                ddlCNSTDET.Items.Item(0).Value = dt(0)("M92_CNSTDET_CLS").ToString
    '            Else
    '                ddlCNSTDET.SelectedValue = dt(0)("M92_CNSTDET_CLS").ToString
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

    Private Sub sAmount_Format(ByVal ipstrPrice As Single, ByRef cpobjLabel As Label)

        Dim sglbuff As Single
        Dim strBuff As String

        If Single.TryParse(ipstrPrice, sglbuff) Then
            If sglbuff <> 0 Then
                strBuff = Integer.Parse(Single.Parse(ipstrPrice)).ToString("#,##0")
                cpobjLabel.Text = strBuff.PadLeft(7)
            Else
                cpobjLabel.Text = (0).ToString.PadLeft(7)
            End If
        Else
            cpobjLabel.Text = cstSpace7
        End If

    End Sub

    ''' <summary>
    ''' 料金情報　取得
    ''' </summary>
    ''' <param name="ipstrProcCntl">コントロール名</param>
    ''' <param name="opstrRet">取得結果</param>
    ''' <remarks></remarks>
    Private Sub sGet_AmountInfo(ByVal ipstrProcCntl As String, ByRef opstrRet As String)

        Dim strProcFlg As String = ""
        Dim strprmValue As String = ""

        'ログ出力開始
        psLogStart(Me)

        opstrRet = "NASI"

        If Me.txtCnst_Cls.ppText Is DBNull.Value Then
            strProcFlg = "CLR"
        ElseIf txtCnst_Cls.ppText.Trim = "" Then
            strProcFlg = "CLR"
        Else
            strProcFlg = "CHK"
        End If

        Select Case strProcFlg
            Case "CLR"
            Case "CHK"

                Dim objSQLCmd As New SqlClient.SqlCommand
                Dim objwkDS As New DataSet

                objSQLCmd.Connection = mclsDB.mobjDB
                objSQLCmd.CommandText = "COMUPDM92_S6"
                objSQLCmd.Parameters.Add(cstSrchKey_CNSTCLS, SqlDbType.NVarChar)
                objSQLCmd.Parameters.Add(cstSrchKey_TBOXCLS, SqlDbType.NVarChar)
                objSQLCmd.Parameters.Add("SEQNO", SqlDbType.NVarChar)
                strprmValue = ""
                If ViewState("txtCnst_Cls") Is Nothing Then
                    strprmValue = ""
                ElseIf ViewState("txtCnst_Cls") = "" Then
                    strprmValue = ""
                Else
                    strprmValue = ViewState("txtCnst_Cls")
                End If
                objSQLCmd.Parameters(cstSrchKey_CNSTCLS).Value = strprmValue
                strprmValue = ""
                If ViewState("ddlTBOXCLS_CD") Is Nothing Then
                    strprmValue = ""
                ElseIf ViewState("ddlTBOXCLS_CD") = "" Then
                    strprmValue = ""
                Else
                    strprmValue = ViewState("ddlTBOXCLS_CD")
                End If
                objSQLCmd.Parameters(cstSrchKey_TBOXCLS).Value = strprmValue
                strprmValue = ""
                If ViewState("txtSEQNO") Is Nothing Then
                    strprmValue = "0"
                ElseIf ViewState("txtSEQNO") = "" Then
                    strprmValue = "0"
                Else
                    strprmValue = ViewState("txtSEQNO")
                End If
                objSQLCmd.Parameters("SEQNO").Value = strprmValue
                If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
                    'エラー
                    clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "リスト作成")
                    objStack = New StackFrame
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "リスト情報取得失敗", "Catch")
                    Exit Sub
                End If

                If objwkDS.Tables.Count > 0 Then
                    If objwkDS.Tables(0).Rows.Count > 0 Then
                        opstrRet = "ARI"
                        Select Case ipstrProcCntl
                            Case "txtSEQNO"
                                Me.ddlWork_Cls.ppSelectedValue = objwkDS.Tables(0).Rows(0).Item("M92_CNSTCKS_CD").ToString
                                ViewState("ddlWork_Cls") = objwkDS.Tables(0).Rows(0).Item("M92_CNSTCKS_CD").ToString
                            Case "ddlWork_Cls"
                        End Select
                        If objwkDS.Tables(0).Rows(0).Item("M48_NML_PRICE") Is DBNull.Value Then
                            Me.lblNml_Amount.Text = cstSpace7
                        Else
                            Call sAmount_Format(objwkDS.Tables(0).Rows(0).Item("M48_NML_PRICE").ToString, lblNml_Amount)
                            ViewState("lblNml_Amount") = lblNml_Amount.Text
                        End If
                        If objwkDS.Tables(0).Rows(0).Item("M48_HLDY_PRICE") Is DBNull.Value Then
                            Me.lblHld_Amount.Text = cstSpace7
                        Else
                            Call sAmount_Format(objwkDS.Tables(0).Rows(0).Item("M48_HLDY_PRICE").ToString, lblHld_Amount)
                            ViewState("lblHld_Amount") = lblHld_Amount.Text
                        End If
                        If objwkDS.Tables(0).Rows(0).Item("M48_NGHT_PRICE") Is DBNull.Value Then
                            Me.lblMdn_Amount.Text = cstSpace7
                        Else
                            Call sAmount_Format(objwkDS.Tables(0).Rows(0).Item("M48_NGHT_PRICE").ToString, lblMdn_Amount)
                            ViewState("lblMdn_Amount") = lblMdn_Amount.Text
                        End If

                    End If
                End If

                Call mclsDB.psDisposeDataSet(objwkDS)

                If opstrRet = "NASI" Then
                    strprmValue = ""
                    '                    objSQLCmd = Nothing
                    '                    objSQLCmd.Connection = mclsDB.mobjDB
                    Dim zz As Integer = 0
                    For zz = 0 To objSQLCmd.Parameters.Count - 1
                        objSQLCmd.Parameters.RemoveAt(0)
                    Next
                    objSQLCmd.CommandText = "COMUPDM92_S7"
                    objSQLCmd.Parameters.Add("ddlWork_Cls", SqlDbType.NVarChar)
                    strprmValue = ""
                    If ViewState("ddlWork_Cls") Is Nothing Then
                        strprmValue = ""
                    ElseIf ViewState("ddlWork_Cls") = "" Then
                        strprmValue = ""
                    Else
                        strprmValue = ViewState("ddlWork_Cls")
                    End If
                    objSQLCmd.Parameters("ddlWork_Cls").Value = strprmValue
                    If mclsDB.pfGet_DataSet(objSQLCmd, objwkDS) = False Then
                        'エラー
                        clsMst.psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "料金情報取得")
                        objStack = New StackFrame
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", "料金情報取得失敗", "Catch")
                        Exit Sub
                    End If

                    If objwkDS.Tables.Count > 0 Then
                        If objwkDS.Tables(0).Rows.Count > 0 Then
                            opstrRet = "ARI2"
                            Select Case ipstrProcCntl
                                Case "txtSEQNO"
                                Case "ddlWork_Cls"
                            End Select
                            If objwkDS.Tables(0).Rows(0).Item("M48_NML_PRICE") Is DBNull.Value Then
                                Me.lblNml_Amount.Text = cstSpace7
                            Else
                                Call sAmount_Format(objwkDS.Tables(0).Rows(0).Item("M48_NML_PRICE").ToString, lblNml_Amount)
                                ViewState("lblNml_Amount") = lblNml_Amount.Text
                            End If
                            If objwkDS.Tables(0).Rows(0).Item("M48_HLDY_PRICE") Is DBNull.Value Then
                                Me.lblHld_Amount.Text = cstSpace7
                            Else
                                Call sAmount_Format(objwkDS.Tables(0).Rows(0).Item("M48_HLDY_PRICE").ToString, lblHld_Amount)
                                ViewState("lblHld_Amount") = lblHld_Amount.Text
                            End If
                            If objwkDS.Tables(0).Rows(0).Item("M48_NGHT_PRICE") Is DBNull.Value Then
                                Me.lblMdn_Amount.Text = cstSpace7
                            Else
                                Call sAmount_Format(objwkDS.Tables(0).Rows(0).Item("M48_NGHT_PRICE").ToString, lblMdn_Amount)
                                ViewState("lblMdn_Amount") = lblMdn_Amount.Text
                            End If
                        End If
                    End If

                    Call mclsDB.psDisposeDataSet(objwkDS)

                End If

        End Select

        'ログ出力終了
        psLogEnd(Me)

    End Sub


#End Region

#Region "終了処理プロシージャ"

#End Region

End Class

