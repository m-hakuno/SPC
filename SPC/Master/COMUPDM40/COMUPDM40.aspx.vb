'********************************************************************************************************************************
'*　システム　：　サポートセンタシステム　＜共通＞
'*　処理名　　：　倉庫マスタ
'*　ＰＧＭＩＤ：　COMUPDM40
'*                                                                                                  CopyRight SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.04.10  ：　栗原
'********************************************************************************************************************************

'---------------------------------------------------------------------------------------------------------------------------------
'番号　　　　　｜　日付　　　　　｜　名前　　｜　備考
'---------------------------------------------------------------------------------------------------------------------------------
'COMUPDM40-001　　2015/05/14　　　　杉山　　　　画面レイアウト変更　XMLファイル追加　COMUPDM40_Header.xml
'COMUPDM40-002　　2015/05/15　　　　杉山　　　　画面レイアウト変更　XMLファイル変更　COMUPDM40_Header.xml　COMUPDM40.xml
'COMUPDM40-003　　2015/05/18　　　　杉山　　　　

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive
#End Region

Public Class COMUPDM40
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
    Const DispCode As String = "COMUPDM40"                  '画面ID
    Const MasterName As String = "倉庫マスタ"               '画面名
    Const TableName As String = "M40_WAREHOUSE"          'テーブル名
#End Region

#Region "変数定義"
    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsExc As New ClsCMExclusive
    Dim clsMst As New ClsMSTCommon
    Dim objStack As StackFrame
    Dim strMode As String = Nothing
    Dim stb As New StringBuilder
    Dim strDeleteFlg As String = String.Empty


    'Dim valflg As Boolean = False


#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page_Init
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        'Dim intHeadCol As Integer() = New Integer() {5, 7}  'TBOXコードとTBOX種別、印刷コードと印刷区分を結合する
        'Dim intColSpan As Integer() = New Integer() {2, 2}
        'pfSet_GridView(Me.grvList, DispCode, intHeadCol, intColSpan, 28, 9) 'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, DispCode, DispCode + "_Header", Me.DivOut, 28, 9)
    End Sub

    ''' <summary>
    ''' Page_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        'ボタンアクション設定
        AddHandler Master.ppBtnSearch.Click, AddressOf btnSearch_Click          '検索
        AddHandler Master.ppBtnSrcClear.Click, AddressOf btnSearchClear_Click   '検索条件クリア
        AddHandler Master.ppBtnInsert.Command, AddressOf btn_Click              '登録
        AddHandler Master.ppBtnUpdate.Command, AddressOf btn_Click              '更新
        AddHandler Master.ppBtnDelete.Command, AddressOf btn_Click              '削除
        AddHandler Master.ppBtnClear.Click, AddressOf btnClear_Click            'クリア
        AddHandler btnPrint.Click, AddressOf btnPrint_Click

        'ドロップダウンリストのイベント設定
        AddHandler ddlNL.SelectedIndexChanged, AddressOf ddlNL_SelectedIndexChanged     '編集エリアNL区分
        AddHandler ddlTbox.SelectedIndexChanged, AddressOf ddlTbox_SelectedIndexChanged '編集エリアTBOX区分
        ddlNL.AutoPostBack = True
        ddlTbox.AutoPostBack = True

        'プレビューボタン活性制御の為のにポストバックトリガーにkey項目を追加
        Dim scm As ScriptManager
        scm = Master.FindControl("tsmManager")
        scm.RegisterPostBackControl(ddlNL)
        scm.RegisterPostBackControl(ddlTbox)
        scm.RegisterPostBackControl(btnPrint)

        'マルチラインテキストBOXの文字制御の為のイベント設定
        Me.txtAddr1.ppTextBox.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txtAddr1.ppMaxLength & """);")      '送付先１住所
        Me.txtAddr1.ppTextBox.Attributes.Add("onChange", "lenCheck(this,""" & Me.txtAddr1.ppMaxLength & """);")
        Me.txtAddr2.ppTextBox.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txtAddr2.ppMaxLength & """);")      '送付先２住所
        Me.txtAddr2.ppTextBox.Attributes.Add("onChange", "lenCheck(this,""" & Me.txtAddr2.ppMaxLength & """);")
        Me.txtDetail1.ppTextBox.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txtDetail1.ppMaxLength & """);")  '送付先１気付
        Me.txtDetail1.ppTextBox.Attributes.Add("onChange", "lenCheck(this,""" & Me.txtDetail1.ppMaxLength & """);")
        Me.txtDetail2.ppTextBox.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txtDetail2.ppMaxLength & """);")  '送付先２気付
        Me.txtDetail2.ppTextBox.Attributes.Add("onChange", "lenCheck(this,""" & Me.txtDetail2.ppMaxLength & """);")
        Me.txtRemark.ppTextBox.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txtRemark.ppMaxLength & """);")    '備考欄
        Me.txtRemark.ppTextBox.Attributes.Add("onChange", "lenCheck(this,""" & Me.txtRemark.ppMaxLength & """);")
        Me.txtDetail3.ppTextBox.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txtDetail3.ppMaxLength & """);")    '送付先３気付
        Me.txtDetail3.ppTextBox.Attributes.Add("onChange", "lenCheck(this,""" & Me.txtDetail3.ppMaxLength & """);")

        If Not IsPostBack Then
            'プログラムＩＤ、画面名、件数設定
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"
            ViewState("strDeleteFlg") = String.Empty

            'パンくずリスト設定
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            'ドロップダウンリスト設定
            msSetddlNL()
            msSetddlTbox()
            msSetddlprint()

            'グリッドの初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            msGet_Data()



            'フォーカス設定
            SetFocus(ddlSNL.ClientID)

            strMode = "Default"
        End If

    End Sub

    ''' <summary>
    ''' Page_PreRender
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        'グリッド編集
        gridedit()
        'バリデーター編集
        EditValid()
        'スクロール調整
        MaintainScrollPositionOnPostBack = True
        'JACKY含むのラベル制御
        If ddlNL.SelectedValue = "N" Then
            lblJac.Visible = True
        Else
            lblJac.Visible = False
        End If

        Select Case strMode

            Case "Default"                          '初期値
                msSet_Enabled(False)
                ddlNL.Enabled = True
                ddlTbox.Enabled = True

                Master.ppBtnInsert.Enabled = False     '追加
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Text = "削除"
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnClear.Enabled = True       'クリア


                btnClear1.Enabled = False   'クリアボタン１
                btnClear2.Enabled = False   'クリアボタン２
                btnClear3.Enabled = False   'クリアボタン３

            Case "Insert"                           '登録時

                msSet_Enabled(True)

                ddlNL.Enabled = False
                ddlTbox.Enabled = False

                Master.ppBtnInsert.Enabled = True      '追加
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Text = "削除"
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnClear.Enabled = True       'クリア



            Case "Select"                           '編集時

                msSet_Enabled(True)

                ddlNL.Enabled = False
                ddlTbox.Enabled = False

                Master.ppBtnInsert.Enabled = False     '追加
                If Master.ppBtnDelete.Text = "削除取消" Then
                    Master.ppBtnUpdate.Enabled = False
                    Master.ppMainEnabled = False
                    btnPrint.Enabled = True
                Else
                    Master.ppBtnUpdate.Enabled = True      '更新
                    Master.ppMainEnabled = True
                End If
                Master.ppBtnDelete.Enabled = True      '削除
                Master.ppBtnClear.Enabled = True       'クリア

        End Select

    End Sub

    ''' <summary>
    ''' 検索ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)
        'ログ出力開始
        psLogStart(Me)

        'データ取得
        If (Page.IsValid) Then
            msGet_Data()
            'フォーカス設定
            SetFocus(ddlSNL.ClientID)
        End If

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 検索条件クリアボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearchClear_Click(sender As Object, e As EventArgs)
        'ログ出力開始
        psLogStart(Me)

        '検索ドロップダウンリストの選択項目をリセット
        ddlSNL.SelectedIndex = -1
        ddlSTbox.SelectedIndex = -1
        ddlSprint.SelectedIndex = -1
        ddldel.ppDropDownList.SelectedIndex = -1
        'フォーカス設定
        SetFocus(ddlSNL.ClientID)

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 追加/更新/削除ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_Click(sender As Object, e As CommandEventArgs)
        'ログ出力開始
        psLogStart(Me)
        If (txtName1.ppTextBox.Text & txtName2.ppTextBox.Text & txtName3.ppTextBox.Text & _
            txtZipNo1.ppTextBox.Text & txtZipNo2.ppTextBox.Text & txtZipNo3.ppTextBox.Text & _
            txtAddr1.ppTextBox.Text & txtAddr2.ppTextBox.Text & txtAddr3.ppTextBox.Text & _
        txtDetail1.ppTextBox.Text & txtDetail2.ppTextBox.Text & txtDetail3.ppTextBox.Text & _
        txtTelNo1.ppTextBox.Text & txtTelNo2.ppTextBox.Text & txtTelNo3.ppTextBox.Text & _
        txtFaxNo1.ppTextBox.Text & txtFaxNo2.ppTextBox.Text & txtFaxNo3.ppTextBox.Text & _
        txtInfo1_1.ppTextBox.Text & txtInfo2_1.ppTextBox.Text & txtInfo3_1.ppTextBox.Text & _
        txtInfo1_2.ppTextBox.Text & txtInfo2_2.ppTextBox.Text & txtInfo3_2.ppTextBox.Text & _
        txtRemark.ppTextBox.Text).ToString.Trim = "" Then

            clsMst.psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "入力内容が空欄です\n入力内容の確認")
            'ログ出力終了
            psLogEnd(Me)
            SetFocus(txtName1.ppTextBox.ClientID)
            Exit Sub
        End If




        Page.Validate("val")
        If (Page.IsValid) Then
            ViewState("valflg") = "False"
            msEditData(e.CommandName)
            'フォーカス設定
            SetFocus(ddlSNL.ClientID)
        Else
            ViewState("valflg") = "True"
        End If

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' クリアボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click()
        'ログ出力開始
        psLogStart(Me)

        '排他情報削除
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

        '編集エリア初期化
        ddlNL.SelectedIndex = -1
        ddlTbox.SelectedIndex = -1
        ddlprint.ppDropDownList.SelectedIndex = -1
        lblTrun.Text = String.Empty
        txtName1.ppText = String.Empty
        txtName2.ppText = String.Empty
        txtZipNo1.ppText = String.Empty
        txtZipNo2.ppText = String.Empty
        txtAddr1.ppText = String.Empty
        txtAddr2.ppText = String.Empty
        txtDetail1.ppText = String.Empty
        txtDetail2.ppText = String.Empty
        txtTelNo1.ppText = String.Empty
        txtTelNo2.ppText = String.Empty
        txtFaxNo1.ppText = String.Empty
        txtFaxNo2.ppText = String.Empty
        txtInfo1_1.ppText = String.Empty
        txtInfo1_2.ppText = String.Empty
        txtInfo2_1.ppText = String.Empty
        txtInfo2_2.ppText = String.Empty
        txtRemark.ppText = String.Empty
        txtName3.ppText = String.Empty
        txtZipNo3.ppText = String.Empty
        txtAddr3.ppText = String.Empty
        txtDetail3.ppText = String.Empty
        txtTelNo3.ppText = String.Empty
        txtFaxNo3.ppText = String.Empty
        txtInfo3_1.ppText = String.Empty
        txtInfo3_2.ppText = String.Empty
        Master.ppMainEnabled = True

        ViewState("valflg") = "False"

        'フォーカス設定
        SetFocus(ddlNL.ClientID)

        strMode = "Default"

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' クリア1ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub btnClear1_Click(sender As Object, e As EventArgs) Handles btnClear1.Click


        'ログ出力開始
        psLogStart(Me)

        '排他情報削除
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

        '編集エリア初期化
        'ddlNL.SelectedIndex = -1
        'ddlTbox.SelectedIndex = -1
        'ddlprint.ppDropDownList.SelectedIndex = -1
        'txtTrun.ppText = String.Empty
        txtName1.ppText = String.Empty
        'txtName2.ppText = String.Empty
        txtZipNo1.ppText = String.Empty
        'txtZipNo2.ppText = String.Empty
        txtAddr1.ppText = String.Empty
        'txtAddr2.ppText = String.Empty
        txtDetail1.ppText = String.Empty
        'txtDetail2.ppText = String.Empty
        txtTelNo1.ppText = String.Empty
        'txtTelNo2.ppText = String.Empty
        txtFaxNo1.ppText = String.Empty
        'txtFaxNo2.ppText = String.Empty
        txtInfo1_1.ppText = String.Empty
        txtInfo1_2.ppText = String.Empty
        'txtInfo2_1.ppText = String.Empty
        'txtInfo2_2.ppText = String.Empty

        Master.ppMainEnabled = True

        'フォーカス設定
        'SetFocus(ddlNL.ClientID)

        SetFocus(txtName1.ppTextBox.ClientID)

        'strMode = "Default"

        'ログ出力終了
        psLogEnd(Me)

        If ViewState("valflg") = "True" Then
            Page.Validate()
        End If
    End Sub

    ''' <summary>
    ''' クリア2ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub btnClear2_Click(sender As Object, e As EventArgs) Handles btnClear2.Click


        'ログ出力開始
        psLogStart(Me)

        '排他情報削除
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

        '編集エリア初期化
        txtName2.ppText = String.Empty
        txtZipNo2.ppText = String.Empty
        txtAddr2.ppText = String.Empty
        txtDetail2.ppText = String.Empty
        txtTelNo2.ppText = String.Empty
        txtFaxNo2.ppText = String.Empty
        txtInfo2_1.ppText = String.Empty
        txtInfo2_2.ppText = String.Empty
        Master.ppMainEnabled = True
        SetFocus(txtName2.ppTextBox.ClientID)

        'ログ出力終了
        psLogEnd(Me)

        If ViewState("valflg") = "True" Then
            Page.Validate()
        End If


    End Sub

    ''' <summary>
    ''' クリア3ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub btnClear3_Click(sender As Object, e As EventArgs) Handles btnClear3.Click

        'ログ出力開始
        psLogStart(Me)

        '排他情報削除
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

        '編集エリア初期化
        txtName3.ppText = String.Empty
        txtZipNo3.ppText = String.Empty
        txtAddr3.ppText = String.Empty
        txtDetail3.ppText = String.Empty
        txtTelNo3.ppText = String.Empty
        txtFaxNo3.ppText = String.Empty
        txtInfo3_1.ppText = String.Empty
        txtInfo3_2.ppText = String.Empty

        Master.ppMainEnabled = True

        'フォーカス設定
        SetFocus(txtName3.ppTextBox.ClientID)

        'ログ出力終了
        psLogEnd(Me)
        If ViewState("valflg") = "True" Then
            Page.Validate()
        End If
    End Sub

    ''' <summary>
    ''' NL区分変更
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlNL_SelectedIndexChanged()
        'ログ出力開始
        psLogStart(Me)

        msGet_ExistData("NL")

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' TBOX種別変更
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlTbox_SelectedIndexChanged()
        'ログ出力開始
        psLogStart(Me)

        msGet_ExistData("TBOX")

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' プレビューボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPrint_Click(sender As Object, e As EventArgs)
        ''''プレビューボタンを押された時の処理
        Dim objDs As DataSet   'データセット
        Dim rpt As Object = Nothing     'ActiveReports用クラス
        '開始ログ出力
        psLogStart(Me)
        objStack = New StackFrame
        objDs = Nothing

        Try
            Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
            Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス


            'DB接続
            If Not clsDataConnect.pfOpen_Database(objCn) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else
                Try
                    objCmd = New SqlCommand(DispCode + "_S3", objCn)
                    With objCmd.Parameters
                        '--パラメータ設定
                        .Add(pfSet_Param("nl", SqlDbType.NVarChar, ""))
                        .Add(pfSet_Param("tbox", SqlDbType.NVarChar, ""))
                        .Add(pfSet_Param("trun", SqlDbType.NVarChar, ""))
                    End With

                    'データ取得
                    objDs = clsDataConnect.pfGet_DataSet(objCmd)

                Catch ex As Exception
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle)

                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")

                Finally
                    'DB切断
                    If Not clsDataConnect.pfClose_Database(objCn) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                End Try
            End If



            objDs.Tables(0).Rows(0).Item("TBOX種別") = ddlTbox.SelectedItem.ToString.Split(":")(1)
            objDs.Tables(0).Rows(0).Item("XNL区分") = ddlNL.SelectedItem.ToString.Split(":")(1)
            objDs.Tables(0).Rows(0).Item("送付先１名称") = txtName1.ppText
            objDs.Tables(0).Rows(0).Item("送付先２名称") = txtName2.ppText
            objDs.Tables(0).Rows(0).Item("送付先３名称") = txtName3.ppText
            If Regex.IsMatch(txtZipNo1.ppText, "(^\d+-\d+$)|(^\d+$)") = False Then
                objDs.Tables(0).Rows(0).Item("送付先１郵便番号") = ""
            Else
                objDs.Tables(0).Rows(0).Item("送付先１郵便番号") = txtZipNo1.ppText
            End If
            If Regex.IsMatch(txtZipNo2.ppText, "(^\d+-\d+$)|(^\d+$)") = False Then
                objDs.Tables(0).Rows(0).Item("送付先２郵便番号") = ""
            Else
                objDs.Tables(0).Rows(0).Item("送付先２郵便番号") = txtZipNo2.ppText
            End If
            If Regex.IsMatch(txtZipNo3.ppText, "(^\d+-\d+$)|(^\d+$)") = False Then
                objDs.Tables(0).Rows(0).Item("送付先３郵便番号") = ""
            Else
                objDs.Tables(0).Rows(0).Item("送付先３郵便番号") = txtZipNo3.ppText
            End If
            objDs.Tables(0).Rows(0).Item("送付先１住所") = txtAddr1.ppText
            objDs.Tables(0).Rows(0).Item("送付先２住所") = txtAddr2.ppText
            objDs.Tables(0).Rows(0).Item("送付先３住所") = txtAddr3.ppText

            objDs.Tables(0).Rows(0).Item("送付先１気付") = txtDetail1.ppText
            objDs.Tables(0).Rows(0).Item("送付先２気付") = txtDetail2.ppText
            objDs.Tables(0).Rows(0).Item("送付先３気付") = txtDetail3.ppText

            If Regex.IsMatch(txtTelNo1.ppText, "(^\d+-\d+-\d+$)|(^\d+$)|(^\d+-\d+$)") = False Then
                objDs.Tables(0).Rows(0).Item("送付先１電話番号") = ""
            Else
                objDs.Tables(0).Rows(0).Item("送付先１電話番号") = txtTelNo1.ppText
            End If
            If Regex.IsMatch(txtTelNo2.ppText, "(^\d+-\d+-\d+$)|(^\d+$)|(^\d+-\d+$)") = False Then
                objDs.Tables(0).Rows(0).Item("送付先２電話番号") = ""
            Else
                objDs.Tables(0).Rows(0).Item("送付先２電話番号") = txtTelNo2.ppText
            End If
            If Regex.IsMatch(txtTelNo3.ppText, "(^\d+-\d+-\d+$)|(^\d+$)|(^\d+-\d+$)") = False Then
                objDs.Tables(0).Rows(0).Item("送付先３電話番号") = ""
            Else
                objDs.Tables(0).Rows(0).Item("送付先３電話番号") = txtTelNo3.ppText
            End If

            If Regex.IsMatch(txtFaxNo1.ppText, "(^\d+-\d+-\d+$)|(^\d+$)|(^\d+-\d+$)") = False Then
                objDs.Tables(0).Rows(0).Item("送付先１FAX番号") = ""
            Else
                objDs.Tables(0).Rows(0).Item("送付先１FAX番号") = txtFaxNo1.ppText
            End If
            If Regex.IsMatch(txtFaxNo2.ppText, "(^\d+-\d+-\d+$)|(^\d+$)|(^\d+-\d+$)") = False Then
                objDs.Tables(0).Rows(0).Item("送付先２FAX番号") = ""
            Else
                objDs.Tables(0).Rows(0).Item("送付先２FAX番号") = txtFaxNo2.ppText
            End If
            If Regex.IsMatch(txtFaxNo3.ppText, "(^\d+-\d+-\d+$)|(^\d+$)|(^\d+-\d+$)") = False Then
                objDs.Tables(0).Rows(0).Item("送付先３FAX番号") = ""
            Else
                objDs.Tables(0).Rows(0).Item("送付先３FAX番号") = txtFaxNo3.ppText
            End If

            objDs.Tables(0).Rows(0).Item("送付先１機器情報１") = txtInfo1_1.ppText
            objDs.Tables(0).Rows(0).Item("送付先１機器情報２") = txtInfo1_2.ppText
            objDs.Tables(0).Rows(0).Item("送付先２機器情報１") = txtInfo2_1.ppText
            objDs.Tables(0).Rows(0).Item("送付先２機器情報２") = txtInfo2_2.ppText
            objDs.Tables(0).Rows(0).Item("送付先３機器情報１") = txtInfo3_1.ppText
            objDs.Tables(0).Rows(0).Item("送付先３機器情報２") = txtInfo3_2.ppText
            objDs.Tables(0).Rows(0).Item("備考") = txtRemark.ppText

            '印刷
            If Not objDs Is Nothing Then
                '帳票出力
                ' rpt = New MSTREP040
                rpt = New MSTREP001
            End If
            psPrintPDF(Me, rpt, objDs.Tables(0), Master.ppTitle)


        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle)

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
        End Try


    End Sub

    '============================================================================================================================
    '==   GRID関係
    '============================================================================================================================
    ''' <summary>
    ''' Grid_Rowコマンド
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_RowCommand(sender As Object, e As CommandEventArgs) Handles grvList.RowCommand

        If e.CommandName = "Select" Then
            Dim conDB As SqlConnection = Nothing
            Dim cmdDB As SqlCommand = Nothing
            Dim dstOrders As DataSet = Nothing
            objStack = New StackFrame
            Dim rowData As GridViewRow = grvList.Rows(Convert.ToInt32(e.CommandArgument))
            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    cmdDB = New SqlCommand(DispCode & "_S2", conDB)
                    With cmdDB.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("nl", SqlDbType.NVarChar, CType(rowData.FindControl("XNL区分コード"), TextBox).Text.Trim))
                        .Add(pfSet_Param("tbox", SqlDbType.NVarChar, CType(rowData.FindControl("TBOX種別コード"), TextBox).Text.Trim))
                        .Add(pfSet_Param("trun", SqlDbType.NVarChar, trun_str(CType(rowData.FindControl("連番"), TextBox).Text.Trim)))
                    End With

                    'リストデータ取得
                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                    '排他制御処理
                    Dim strExclusiveDate As String = Nothing
                    Dim arTable_Name As New ArrayList
                    Dim arKey As New ArrayList

                    '排他情報削除
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

                    'ロック対象テーブル名の登録
                    arTable_Name.Insert(0, TableName)

                    'ロックテーブルキー項目の登録
                    arKey.Insert(0, CType(rowData.FindControl("XNL区分コード"), TextBox).Text.Trim)
                    arKey.Insert(0, CType(rowData.FindControl("TBOX種別コード"), TextBox).Text.Trim)
                    arKey.Insert(0, CType(rowData.FindControl("連番"), TextBox).Text.Trim)

                    '排他情報確認処理(更新画面へ遷移)
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

                        '登録年月日時刻取得
                        Me.Master.ppExclusiveDate = strExclusiveDate

                    Else
                        '排他ロック中
                        'ログ出力終了
                        psLogEnd(Me)
                        Exit Sub
                    End If

                    '下段ドロップリスト(TBOX)内に選択項目が含まれているかチェック
                    Dim list_check_FLG As Integer = 0
                    For i As Integer = 0 To Me.ddlTbox.Items.Count - 1
                        If (CType(rowData.FindControl("TBOX種別コード"), TextBox).Text = Me.ddlTbox.Items(i).Value) Then
                            list_check_FLG = 1
                            Exit For
                        End If
                    Next
                    '編集エリアに値を設定
                    ddlNL.SelectedValue = CType(rowData.FindControl("XNL区分コード"), TextBox).Text.Trim
                    If list_check_FLG = 1 Then
                        ddlTbox.SelectedValue = CType(rowData.FindControl("TBOX種別コード"), TextBox).Text.Trim
                    Else    '選択されたTBOX種別が削除済みデータだった場合、ドロップダウンリストは空欄に設定
                        ddlTbox.SelectedIndex = -1
                    End If
                    lblTrun.Text = CType(CType(rowData.FindControl("連番"), TextBox).Text.Trim, Integer)
                    ddlprint.ppDropDownList.SelectedValue = dstOrders.Tables(0).Rows(0).Item("印刷区分コード").ToString
                    txtName1.ppText = dstOrders.Tables(0).Rows(0).Item("送付先１名称").ToString.Replace(Environment.NewLine, "")
                    txtName2.ppText = dstOrders.Tables(0).Rows(0).Item("送付先２名称").ToString.Replace(Environment.NewLine, "")
                    txtName3.ppText = dstOrders.Tables(0).Rows(0).Item("送付先３名称").ToString.Replace(Environment.NewLine, "")
                    txtZipNo1.ppText = dstOrders.Tables(0).Rows(0).Item("送付先１郵便番号").ToString
                    txtZipNo2.ppText = dstOrders.Tables(0).Rows(0).Item("送付先２郵便番号").ToString
                    txtZipNo3.ppText = dstOrders.Tables(0).Rows(0).Item("送付先３郵便番号").ToString
                    txtAddr1.ppText = dstOrders.Tables(0).Rows(0).Item("送付先１住所").ToString.Replace(Environment.NewLine, "")
                    txtAddr2.ppText = dstOrders.Tables(0).Rows(0).Item("送付先２住所").ToString.Replace(Environment.NewLine, "")
                    txtAddr3.ppText = dstOrders.Tables(0).Rows(0).Item("送付先３住所").ToString.Replace(Environment.NewLine, "")
                    txtDetail1.ppText = dstOrders.Tables(0).Rows(0).Item("送付先１気付").ToString.Replace(Environment.NewLine, "")
                    txtDetail2.ppText = dstOrders.Tables(0).Rows(0).Item("送付先２気付").ToString.Replace(Environment.NewLine, "")
                    txtDetail3.ppText = dstOrders.Tables(0).Rows(0).Item("送付先３気付").ToString.Replace(Environment.NewLine, "")
                    txtTelNo1.ppText = dstOrders.Tables(0).Rows(0).Item("送付先１電話番号").ToString
                    txtTelNo2.ppText = dstOrders.Tables(0).Rows(0).Item("送付先２電話番号").ToString
                    txtTelNo3.ppText = dstOrders.Tables(0).Rows(0).Item("送付先３電話番号").ToString
                    txtFaxNo1.ppText = dstOrders.Tables(0).Rows(0).Item("送付先１FAX番号").ToString
                    txtFaxNo2.ppText = dstOrders.Tables(0).Rows(0).Item("送付先２FAX番号").ToString
                    txtFaxNo3.ppText = dstOrders.Tables(0).Rows(0).Item("送付先３FAX番号").ToString
                    txtInfo1_1.ppText = dstOrders.Tables(0).Rows(0).Item("送付先１機器情報１").ToString.Replace(Environment.NewLine, "")
                    txtInfo1_2.ppText = dstOrders.Tables(0).Rows(0).Item("送付先１機器情報２").ToString.Replace(Environment.NewLine, "")
                    txtInfo2_1.ppText = dstOrders.Tables(0).Rows(0).Item("送付先２機器情報１").ToString.Replace(Environment.NewLine, "")
                    txtInfo2_2.ppText = dstOrders.Tables(0).Rows(0).Item("送付先２機器情報２").ToString.Replace(Environment.NewLine, "")
                    txtInfo3_1.ppText = dstOrders.Tables(0).Rows(0).Item("送付先３機器情報１").ToString.Replace(Environment.NewLine, "")
                    txtInfo3_2.ppText = dstOrders.Tables(0).Rows(0).Item("送付先３機器情報２").ToString.Replace(Environment.NewLine, "")
                    txtRemark.ppText = dstOrders.Tables(0).Rows(0).Item("備考").ToString.Replace(Environment.NewLine, "")

                    If dstOrders.Tables(0).Rows(0).Item("削除") <> "1" Then
                        Master.ppBtnDelete.Text = "削除"
                        Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '削除
                        strMode = "Select"
                    Else
                        Master.ppBtnDelete.Text = "削除取消"
                        Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
                        strMode = "Select"
                    End If

                Catch ex As Exception
                    clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Finally
                    'DB切断
                    If Not clsDataConnect.pfClose_Database(conDB) Then
                        clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                End Try
            Else
                clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End If

        ''''印刷ボタンを押された時の処理
        If e.CommandName = "Print" Then
            Dim objDs As DataSet = Nothing  'データセット
            Dim rpt As Object = Nothing     'ActiveReports用クラス
            Dim rowData As GridViewRow = grvList.Rows(Convert.ToInt32(e.CommandArgument))
            '開始ログ出力
            psLogStart(Me)
            Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
            Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
            objStack = New StackFrame
            objDs = Nothing

            'DB接続
            If Not clsDataConnect.pfOpen_Database(objCn) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else
                Try
                    objCmd = New SqlCommand(DispCode + "_S3", objCn)
                    With objCmd.Parameters
                        '--パラメータ設定
                        .Add(pfSet_Param("nl", SqlDbType.NVarChar, CType(rowData.FindControl("XNL区分コード"), TextBox).Text.Trim))
                        .Add(pfSet_Param("tbox", SqlDbType.NVarChar, CType(rowData.FindControl("TBOX種別コード"), TextBox).Text.Trim))
                        .Add(pfSet_Param("trun", SqlDbType.NVarChar, trun_str(CType(rowData.FindControl("連番"), TextBox).Text.Trim)))
                    End With

                    'データ取得
                    objDs = clsDataConnect.pfGet_DataSet(objCmd)

                Catch ex As Exception
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle)

                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")

                Finally
                    'DB切断
                    If Not clsDataConnect.pfClose_Database(objCn) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                End Try
            End If
            '印刷
            If Not objDs Is Nothing Then
                '帳票出力
                rpt = New MSTREP001
                psPrintPDF(Me, rpt, objDs.Tables(0), Master.ppTitle)
            End If
            '終了ログ出力
            psLogEnd(Me)
        End If
    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 活性制御
    ''' </summary>
    ''' <param name="bool"></param>
    ''' <remarks></remarks>
    Private Sub msSet_Enabled(ByVal bool As Boolean)
        '編集エリア全項目一斉活性制御
        ddlNL.Enabled = bool
        ddlTbox.Enabled = bool
        ddlprint.ppDropDownList.Enabled = bool
        btnClear1.Enabled = bool
        btnClear2.Enabled = bool
        btnClear3.Enabled = bool
        btnPrint.Enabled = bool

        txtName1.ppEnabled = bool
        txtName2.ppEnabled = bool
        txtName3.ppEnabled = bool
        txtZipNo1.ppEnabled = bool
        txtZipNo2.ppEnabled = bool
        txtZipNo3.ppEnabled = bool
        txtAddr1.ppEnabled = bool
        txtAddr2.ppEnabled = bool
        txtAddr3.ppEnabled = bool
        txtDetail1.ppEnabled = bool
        txtDetail2.ppEnabled = bool
        txtDetail3.ppEnabled = bool
        txtTelNo1.ppEnabled = bool
        txtTelNo2.ppEnabled = bool
        txtTelNo3.ppEnabled = bool
        txtFaxNo1.ppEnabled = bool
        txtFaxNo2.ppEnabled = bool
        txtFaxNo3.ppEnabled = bool
        txtInfo1_1.ppEnabled = bool
        txtInfo1_2.ppEnabled = bool
        txtInfo2_1.ppEnabled = bool
        txtInfo2_2.ppEnabled = bool
        txtInfo3_1.ppEnabled = bool
        txtInfo3_2.ppEnabled = bool
        txtRemark.ppEnabled = bool

    End Sub

    ''' <summary>
    ''' 検索処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim dispFlg As String = String.Empty
        objStack = New StackFrame

        If Me.IsPostBack Then 'ポストバックかどうか判定
            dispFlg = "0"
        Else
            dispFlg = "1"
        End If

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(DispCode & "_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("nl", SqlDbType.NVarChar, ddlSNL.SelectedValue))
                    .Add(pfSet_Param("tbox", SqlDbType.NVarChar, ddlSTbox.SelectedValue))
                    .Add(pfSet_Param("print", SqlDbType.NVarChar, ddlSprint.SelectedValue))
                    .Add(pfSet_Param("prog_id", SqlDbType.NVarChar, DispCode))
                    .Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, dispFlg))
                    .Add(pfSet_Param("DeleteFlg", SqlDbType.NVarChar, ddldel.ppDropDownList.SelectedValue))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    Master.ppCount = "0"
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)

                    '総件数とデータセット内の件数(閾値制限)の比較
                ElseIf CType(dstOrders.Tables(0).Rows(0).Item("総件数").ToString, Integer) > dstOrders.Tables(0).Rows.Count Then
                    '閾値オーバー
                    Master.ppCount = dstOrders.Tables(0).Rows(0).Item("総件数").ToString
                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                                        dstOrders.Tables(0).Rows(0).Item("総件数").ToString, dstOrders.Tables(0).Rows.Count)
                Else
                    Master.ppCount = dstOrders.Tables(0).Rows(0).Item("総件数").ToString
                End If

                '取得したデータをリストに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()
                Master.ppCount = "0"
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
            Master.ppCount = "0"
        End If

    End Sub

    ''' <summary>
    ''' 既存データ取得処理
    ''' </summary>
    ''' <param name="strKey"></param>
    ''' <remarks></remarks>
    Private Sub msGet_ExistData(ByVal strKey As String)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        objStack = New StackFrame
        Dim strtrun As String = String.Empty



        '全キー項目に数値が入力されているかチェック
        '連番空欄時　連番自動発番
        If ddlNL.SelectedIndex > 0 AndAlso ddlTbox.SelectedIndex > 0 AndAlso lblTrun.Text = String.Empty Then

            Page.Validate("key")
            Dim i As Integer = 99 '連番用'99番のデータ有無を最初にチェック
            If (Page.IsValid) Then
                '接続
                If clsDataConnect.pfOpen_Database(conDB) Then
                    Try
                        cmdDB = New SqlCommand(DispCode & "_S2", conDB)

                        With cmdDB.Parameters
                            'キーカラム
                            'パラメータ設定
                            .Add(pfSet_Param("nl", SqlDbType.NVarChar, ddlNL.SelectedValue))
                            .Add(pfSet_Param("tbox", SqlDbType.NVarChar, ddlTbox.SelectedValue))
                            .Add(pfSet_Param("trun", SqlDbType.NVarChar, i))
                        End With
                        'リストデータ取得
                        dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                        '連番99にデータがあるか確認
                        '連番99にデータがあった場合は空いている番号の最小値を取得
                        If dstOrders.Tables(0).Rows.Count > 0 Then
                            i = 1
                            strtrun = trun_str(i)
                            '排他制御処理
                            Dim strExclusiveDate As String = Nothing
                            Dim arTable_Name As New ArrayList
                            Dim arKey As New ArrayList

                            '排他情報削除
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

                            'ロック対象テーブル名の登録
                            arTable_Name.Insert(0, TableName)

                            'ロックテーブルキー項目の登録
                            arKey.Insert(0, ddlNL.SelectedValue)
                            arKey.Insert(0, ddlTbox.SelectedValue)
                            arKey.Insert(0, strtrun)

                            '排他情報確認処理(更新画面へ遷移)
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

                                '登録年月日時刻取得
                                Me.Master.ppExclusiveDate = strExclusiveDate

                            Else
                                '排他ロック中
                                clsMst.psMesBox(Me, "30010", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)
                                'ログ出力終了
                                psLogEnd(Me)
                                Exit Sub
                            End If

                            '検索用にパラメーターを一旦削除
                            cmdDB.Parameters.Clear()
                            With cmdDB.Parameters
                                'パラメータ設定をやりなおして判定
                                .Add(pfSet_Param("nl", SqlDbType.NVarChar, ddlNL.SelectedValue))
                                .Add(pfSet_Param("tbox", SqlDbType.NVarChar, ddlTbox.SelectedValue))
                                .Add(pfSet_Param("trun", SqlDbType.NVarChar, strtrun))
                            End With
                            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                            'i行目にデータが存在しなくなるまで繰り返し
                            While dstOrders.Tables(0).Rows.Count > 0
                                i = i + 1
                                strtrun = trun_str(i)
                                If i = 100 Then '連番100までで空き番号が見つからなければエラー

                                    cmdDB.Parameters.Clear()
                                    With cmdDB.Parameters
                                        'パラメータ設定をやりなおして判定
                                        .Add(pfSet_Param("nl", SqlDbType.NVarChar, ddlNL.SelectedValue))
                                        .Add(pfSet_Param("tbox", SqlDbType.NVarChar, ddlTbox.SelectedValue))
                                        .Add(pfSet_Param("trun", SqlDbType.NVarChar, strtrun))
                                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                                    End With
                                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)


                                    If Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString) = 1 Then '該当NL区分システムに工事依頼書用データが含まれています。(エラー処理は同じ)
                                        clsMst.psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "上限件数に達しました、データの整理")
                                        strMode = "Default"
                                        btnClear_Click()
                                        Exit Sub
                                    End If

                                    clsMst.psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "上限件数に達しました、データの整理")
                                    strMode = "Default"
                                    btnClear_Click()
                                    Exit Sub
                                End If
                                'パラメーターを一旦削除
                                cmdDB.Parameters.Clear()
                                With cmdDB.Parameters
                                    'パラメータ設定をやりなおして判定
                                    .Add(pfSet_Param("nl", SqlDbType.NVarChar, ddlNL.SelectedValue))
                                    .Add(pfSet_Param("tbox", SqlDbType.NVarChar, ddlTbox.SelectedValue))
                                    .Add(pfSet_Param("trun", SqlDbType.NVarChar, strtrun))
                                End With
                                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                            End While
                            '値を挿入
                            lblTrun.Text = CType(strtrun, Integer)  '空き番号を挿入
                            strMode = "Insert"

                            '連番99にデータがなかった場合は登録済み連番の最大値を取得
                        Else
                            Dim j As String = "100"  '登録済み連番の最大値保存用変数
                            For i = 1 To 99 '連番1から連番99まで探索
                                strtrun = trun_str(i)
                                'パラメーターを一旦削除
                                cmdDB.Parameters.Clear()
                                With cmdDB.Parameters
                                    'パラメータ設定をやりなおして判定
                                    .Add(pfSet_Param("nl", SqlDbType.NVarChar, ddlNL.SelectedValue))
                                    .Add(pfSet_Param("tbox", SqlDbType.NVarChar, ddlTbox.SelectedValue))
                                    .Add(pfSet_Param("trun", SqlDbType.NVarChar, strtrun))
                                End With
                                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                                '排他制御処理
                                Dim strExclusiveDate As String = Nothing
                                Dim arTable_Name As New ArrayList
                                Dim arKey As New ArrayList

                                '排他情報削除
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

                                'ロック対象テーブル名の登録
                                arTable_Name.Insert(0, TableName)

                                'ロックテーブルキー項目の登録
                                arKey.Insert(0, ddlNL.SelectedValue)
                                arKey.Insert(0, ddlTbox.SelectedValue)
                                arKey.Insert(0, strtrun)

                                '排他情報確認処理(更新画面へ遷移)
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

                                    '登録年月日時刻取得
                                    Me.Master.ppExclusiveDate = strExclusiveDate

                                Else
                                    '排他ロック中
                                    clsMst.psMesBox(Me, "30010", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)
                                    'ログ出力終了
                                    psLogEnd(Me)
                                    Exit Sub
                                End If

                                'データがあった場合、その連番+1の行にデータがあるか判定
                                If dstOrders.Tables(0).Rows.Count > 0 AndAlso i < 99 Then
                                    strtrun = trun_str(i + 1)
                                    'パラメーターを一旦削除
                                    cmdDB.Parameters.Clear()
                                    'パラメータ設定をやりなおして次の行を判定
                                    With cmdDB.Parameters
                                        .Add(pfSet_Param("nl", SqlDbType.NVarChar, ddlNL.SelectedValue))
                                        .Add(pfSet_Param("tbox", SqlDbType.NVarChar, ddlTbox.SelectedValue))
                                        .Add(pfSet_Param("trun", SqlDbType.NVarChar, strtrun))
                                    End With
                                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                                    '排他制御処理
                                    '排他情報削除
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

                                    'ロック対象テーブル名の登録
                                    arTable_Name.Insert(0, TableName)

                                    'ロックテーブルキー項目の登録
                                    arKey.Insert(0, ddlNL.SelectedValue)
                                    arKey.Insert(0, ddlTbox.SelectedValue)
                                    arKey.Insert(0, strtrun)

                                    '排他情報確認処理(更新画面へ遷移)
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

                                        '登録年月日時刻取得
                                        Me.Master.ppExclusiveDate = strExclusiveDate

                                    Else
                                        '排他ロック中
                                        clsMst.psMesBox(Me, "30010", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)
                                        'ログ出力終了
                                        psLogEnd(Me)
                                        Exit Sub
                                    End If
                                    '空欄なら候補番号として保存
                                    If dstOrders.Tables(0).Rows.Count = 0 Then
                                        j = i + 1
                                    End If
                                End If
                            Next

                            '値を挿入
                            If j = "100" Then j = "1"
                            lblTrun.Text = CType(j, Integer)
                            strMode = "Insert"
                        End If
                    Catch ex As Exception
                        clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    Finally
                        'DB切断
                        If Not clsDataConnect.pfClose_Database(conDB) Then
                            clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        End If
                    End Try
                Else
                    clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End If
        Else
            strMode = "Default"
        End If

        'フォーカス処理
        Dim afFocusID As String = String.Empty
        If strMode = "Select" Then
            afFocusID = txtName1.ppTextBox.ClientID
        Else
            If ddlNL.SelectedIndex > 0 AndAlso ddlTbox.SelectedIndex > 0 AndAlso lblTrun.Text.Trim <> String.Empty Then
                afFocusID = ddlprint.ppDropDownList.ClientID
            Else
                If strKey = "NL" Then
                    afFocusID = ddlNL.ClientID
                ElseIf strKey = "TBOX" Then
                    afFocusID = ddlTbox.ClientID
                ElseIf strKey = "Trun" Then
                    afFocusID = Master.ppBtnClear.ClientID
                End If
            End If
        End If
        Master.ppBtnDmy.Visible = True
        Master.ppBtnDmy.Attributes("onFocus") = "focusChange(" + Master.ppBtnDmy.ClientID + "," + afFocusID + ");"
        SetFocus(Master.ppBtnDmy.ClientID)

    End Sub

    ''' <summary>
    ''' 追加/更新/削除処理
    ''' </summary>
    ''' <param name="ipstrMode"></param>
    ''' <remarks></remarks>
    Private Sub msEditData(ByVal ipstrMode As String)


        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim intRtn As Integer
        Dim MesCode As String = String.Empty
        Dim procCls As String = String.Empty
        Dim strStored As String = String.Empty
        Dim dttGrid As New DataTable
        Dim drData As DataRow
        objStack = New StackFrame
        Dim getFlg As String = "0"
        Dim strErrorMsg As String = String.Empty

        Select Case ipstrMode
            Case "INSERT"
                MesCode = "00003"
                procCls = "0"
                strStored = DispCode & "_U1"
                strErrorMsg = "登録"

            Case "UPDATE"
                MesCode = "00001"
                procCls = "1"
                strStored = DispCode & "_U1"
                strErrorMsg = "更新"

            Case "DELETE"
                Select Case Master.ppBtnDelete.Text
                    Case "削除"
                        MesCode = "00002"
                        strStored = DispCode & "_D1"
                        getFlg = "1"
                    Case "削除取消"
                        MesCode = "00001"
                        strStored = DispCode & "_D1"
                        getFlg = "0"
                        strErrorMsg = "削除取消"
                End Select

        End Select
        '入力チェック

        If (Page.IsValid) OrElse ipstrMode = "DELETE" Then
            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    cmdDB = New SqlCommand(strStored, conDB)
                    Select Case ipstrMode
                        Case "INSERT", "UPDATE"
                            With cmdDB.Parameters
                                'パラメータ設定
                                .Add(pfSet_Param("nl", SqlDbType.NVarChar, ddlNL.SelectedValue))                                'NL区分
                                .Add(pfSet_Param("tbox", SqlDbType.NVarChar, ddlTbox.SelectedValue))                            'TBOX種別コード
                                .Add(pfSet_Param("tbox_nm", SqlDbType.NVarChar, ddlTbox.SelectedItem.ToString.Split(":")(1)))   'TBOX種別
                                .Add(pfSet_Param("trun", SqlDbType.NVarChar, trun_str(lblTrun.Text.Trim)))                    '連番
                                .Add(pfSet_Param("print", SqlDbType.NVarChar, ddlprint.ppDropDownList.SelectedValue))                          '印刷区分
                                .Add(pfSet_Param("name1", SqlDbType.NVarChar, txtName1.ppText.Trim))                            '送付先1名称
                                .Add(pfSet_Param("name2", SqlDbType.NVarChar, txtName2.ppText.Trim))                            '送付先2名称
                                .Add(pfSet_Param("name3", SqlDbType.NVarChar, txtName3.ppText.Trim))                            '送付先3名称
                                .Add(pfSet_Param("zipno1", SqlDbType.NVarChar, txtZipNo1.ppText.Trim))                          '送付先1郵便番号
                                .Add(pfSet_Param("zipno2", SqlDbType.NVarChar, txtZipNo2.ppText.Trim))                          '送付先2郵便番号
                                .Add(pfSet_Param("zipno3", SqlDbType.NVarChar, txtZipNo3.ppText.Trim))                          '送付先3郵便番号
                                .Add(pfSet_Param("addr1", SqlDbType.NVarChar, txtAddr1.ppText.Trim))                            '送付先1住所
                                .Add(pfSet_Param("addr2", SqlDbType.NVarChar, txtAddr2.ppText.Trim))                            '送付先2住所
                                .Add(pfSet_Param("addr3", SqlDbType.NVarChar, txtAddr3.ppText.Trim))                            '送付先3住所
                                .Add(pfSet_Param("detail1", SqlDbType.NVarChar, txtDetail1.ppText.Trim))                        '送付先1気付
                                .Add(pfSet_Param("detail2", SqlDbType.NVarChar, txtDetail2.ppText.Trim))                          '送付先2気付
                                .Add(pfSet_Param("detail3", SqlDbType.NVarChar, txtDetail3.ppText.Trim))                          '送付先3気付
                                .Add(pfSet_Param("tel1", SqlDbType.NVarChar, txtTelNo1.ppText.Trim))                            '送付先1電話番号
                                .Add(pfSet_Param("tel2", SqlDbType.NVarChar, txtTelNo2.ppText.Trim))                            '送付先2電話番号
                                .Add(pfSet_Param("tel3", SqlDbType.NVarChar, txtTelNo3.ppText.Trim))                            '送付先3電話番号
                                .Add(pfSet_Param("fax1", SqlDbType.NVarChar, txtFaxNo1.ppText.Trim))                            '送付先1FAX
                                .Add(pfSet_Param("fax2", SqlDbType.NVarChar, txtFaxNo2.ppText.Trim))                             '送付先2FAX
                                .Add(pfSet_Param("fax3", SqlDbType.NVarChar, txtFaxNo3.ppText.Trim))                             '送付先3FAX
                                .Add(pfSet_Param("info1_1", SqlDbType.NVarChar, txtInfo1_1.ppText.Trim))                        '送付先1機器情報1_1
                                .Add(pfSet_Param("info1_2", SqlDbType.NVarChar, txtInfo1_2.ppText.Trim))                        '送付先1機器情報1_2
                                .Add(pfSet_Param("info2_1", SqlDbType.NVarChar, txtInfo2_1.ppText.Trim))                        '送付先2機器情報2_1
                                .Add(pfSet_Param("info2_2", SqlDbType.NVarChar, txtInfo2_2.ppText.Trim))                        '送付先2機器情報2_2
                                .Add(pfSet_Param("info3_1", SqlDbType.NVarChar, txtInfo3_1.ppText.Trim))                        '送付先3機器情報3_1
                                .Add(pfSet_Param("info3_2", SqlDbType.NVarChar, txtInfo3_2.ppText.Trim))                        '送付先3機器情報3_2
                                .Add(pfSet_Param("remark", SqlDbType.NVarChar, txtRemark.ppText.Trim))                          '備考
                                .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))                                      '処理区分
                                .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                             'ユーザーＩＤ
                                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                               '戻り値
                            End With

                        Case "DELETE"
                            With cmdDB.Parameters
                                'パラメータ設定
                                .Add(pfSet_Param("nl", SqlDbType.NVarChar, ddlNL.SelectedValue))                                'NL区分
                                .Add(pfSet_Param("tbox", SqlDbType.NVarChar, ddlTbox.SelectedValue))                            'TBOX種別
                                .Add(pfSet_Param("trun", SqlDbType.NVarChar, trun_str(lblTrun.Text.Trim)))                    '連番
                                .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                             'ユーザーＩＤ
                                .Add(pfSet_Param("print", SqlDbType.NVarChar, ddlprint.ppDropDownList.SelectedValue))                          '印刷区分
                                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                               '戻り値
                            End With

                    End Select

                    'データ登録/更新/削除
                    'トランザクションの開始
                    Using conTrn = conDB.BeginTransaction
                        cmdDB.Transaction = conTrn
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)

                        If intRtn = 1 Then
                            'ロールバック
                            conTrn.Rollback()
                            clsMst.psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "既に印刷対象データがある為、" & strErrorMsg & "出来ません。\n印刷区分の確認")
                            Exit Sub
                        End If
                        If intRtn <> 0 Then
                            'ロールバック
                            conTrn.Rollback()
                            psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                            Exit Sub
                        End If
                        'コミット
                        conTrn.Commit()
                    End Using

                    '追加、更新の場合は対象のレコードのみグリッドに表示
                    If ipstrMode = "INSERT" OrElse ipstrMode = "UPDATE" Or getFlg = "0" Then
                        'データテーブルにフィールド名を設定
                        dttGrid.Columns.Add("XNL区分コード")
                        dttGrid.Columns.Add("XNL区分")
                        dttGrid.Columns.Add("TBOX種別コード")
                        dttGrid.Columns.Add("TBOX種別")
                        dttGrid.Columns.Add("印刷区分コード")
                        dttGrid.Columns.Add("印刷区分")
                        dttGrid.Columns.Add("連番")
                        dttGrid.Columns.Add("送付先１名称")
                        dttGrid.Columns.Add("送付先１郵便番号")
                        dttGrid.Columns.Add("送付先１住所")
                        dttGrid.Columns.Add("送付先１気付")
                        dttGrid.Columns.Add("送付先１電話番号")
                        dttGrid.Columns.Add("送付先１FAX番号")
                        dttGrid.Columns.Add("送付先１機器情報1")
                        dttGrid.Columns.Add("送付先１機器情報２")
                        dttGrid.Columns.Add("送付先２名称")
                        dttGrid.Columns.Add("送付先２郵便番号")
                        dttGrid.Columns.Add("送付先２住所")
                        dttGrid.Columns.Add("送付先２気付")
                        dttGrid.Columns.Add("送付先２電話番号")
                        dttGrid.Columns.Add("送付先２FAX番号")
                        dttGrid.Columns.Add("送付先２機器情報１")
                        dttGrid.Columns.Add("送付先２機器情報２")
                        dttGrid.Columns.Add("送付先３名称")
                        dttGrid.Columns.Add("送付先３郵便番号")
                        dttGrid.Columns.Add("送付先３住所")
                        dttGrid.Columns.Add("送付先３気付")
                        dttGrid.Columns.Add("送付先３電話番号")
                        dttGrid.Columns.Add("送付先３FAX番号")
                        dttGrid.Columns.Add("送付先３機器情報1")
                        dttGrid.Columns.Add("送付先３機器情報２")
                        dttGrid.Columns.Add("備考")
                        dttGrid.Columns.Add("削除")

                        '行に編集エリアから値を設定
                        drData = dttGrid.NewRow()
                        drData.Item("XNL区分コード") = ddlNL.SelectedValue
                        drData.Item("XNL区分") = ddlNL.SelectedItem.ToString.Split(":")(1)
                        drData.Item("TBOX種別コード") = ddlTbox.SelectedValue
                        drData.Item("TBOX種別") = ddlTbox.SelectedItem.ToString.Split(":")(1)
                        drData.Item("印刷区分コード") = ddlprint.ppDropDownList.SelectedValue
                        drData.Item("印刷区分") = ddlprint.ppDropDownList.SelectedItem.ToString.Split(":")(1)
                        drData.Item("連番") = CType(lblTrun.Text.Trim, Integer)
                        drData.Item("送付先１名称") = txtName1.ppText.Trim
                        drData.Item("送付先１郵便番号") = txtZipNo1.ppText.Trim
                        drData.Item("送付先１住所") = txtAddr1.ppText.Trim
                        drData.Item("送付先１気付") = txtDetail1.ppText.Trim
                        drData.Item("送付先１電話番号") = txtTelNo1.ppText.Trim
                        drData.Item("送付先１FAX番号") = txtFaxNo1.ppText.Trim
                        drData.Item("送付先１機器情報1") = txtInfo1_1.ppText.Trim
                        drData.Item("送付先１機器情報２") = txtInfo1_2.ppText.Trim
                        drData.Item("送付先２名称") = txtName2.ppText.Trim
                        drData.Item("送付先２郵便番号") = txtZipNo2.ppText.Trim
                        drData.Item("送付先２住所") = txtAddr2.ppText.Trim
                        drData.Item("送付先２気付") = txtDetail2.ppText.Trim
                        drData.Item("送付先２電話番号") = txtTelNo2.ppText.Trim
                        drData.Item("送付先２FAX番号") = txtFaxNo2.ppText.Trim
                        drData.Item("送付先２機器情報１") = txtInfo2_1.ppText.Trim
                        drData.Item("送付先２機器情報２") = txtInfo2_2.ppText.Trim
                        drData.Item("送付先３名称") = txtName3.ppText.Trim
                        drData.Item("送付先３郵便番号") = txtZipNo3.ppText.Trim
                        drData.Item("送付先３住所") = txtAddr3.ppText.Trim
                        drData.Item("送付先３気付") = txtDetail3.ppText.Trim
                        drData.Item("送付先３電話番号") = txtTelNo3.ppText.Trim
                        drData.Item("送付先３FAX番号") = txtFaxNo3.ppText.Trim
                        drData.Item("送付先３機器情報１") = txtInfo3_1.ppText.Trim
                        drData.Item("送付先３機器情報２") = txtInfo3_2.ppText.Trim
                        drData.Item("備考") = txtRemark.ppText.Trim
                        drData.Item("削除") = ""
                        'データテーブルに追加
                        dttGrid.Rows.Add(drData)

                    Else
                        '削除の場合はテーブルを初期化
                        dttGrid = New DataTable
                    End If

                    'データをセット
                    dttGrid.AcceptChanges()
                    grvList.DataSource = dttGrid
                    grvList.DataBind()

                    '件数表示の設定
                    Master.ppCount = dttGrid.Rows.Count
                    psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName)
                    'フォーカスチェンジと編集エリア初期化
                    btnClear_Click()

                Catch ex As Exception
                    psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Finally
                    'DB切断
                    If Not clsDataConnect.pfClose_Database(conDB) Then
                        psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                End Try
            Else
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End If
    End Sub

    ''' <summary>
    ''' NL区分ドロップダウンリスト作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlNL()

        'ドロップダウンリスト設定
        '検索側
        Me.ddlSNL.Items.Clear()
        Me.ddlSNL.DataTextField = "XNL区分"
        Me.ddlSNL.DataValueField = "XNL区分コード"
        Me.ddlSNL.Items.Insert(0, "N:ＮＧＣ")
        Me.ddlSNL.Items(0).Value = "N"
        Me.ddlSNL.Items.Insert(1, "L:ＬＥＣ")
        Me.ddlSNL.Items(1).Value = "L"
        Me.ddlSNL.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

        '編集側
        Me.ddlNL.Items.Clear()
        Me.ddlNL.DataTextField = "XNL区分"
        Me.ddlNL.DataValueField = "XNL区分コード"
        Me.ddlNL.Items.Insert(0, "N:ＮＧＣ")
        Me.ddlNL.Items(0).Value = "N"
        Me.ddlNL.Items.Insert(1, "L:ＬＥＣ")
        Me.ddlNL.Items(1).Value = "L"
        Me.ddlNL.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
    End Sub

    ''' <summary>
    ''' 印刷区分ドロップダウンリスト作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlprint()

        'ドロップダウンリスト設定
        '検索側
        Me.ddlSprint.Items.Clear()
        Me.ddlSprint.DataTextField = "印刷区分"
        Me.ddlSprint.DataValueField = "印刷区分コード"
        Me.ddlSprint.Items.Insert(0, "0:印刷対象")
        Me.ddlSprint.Items(0).Value = "0"
        Me.ddlSprint.Items.Insert(1, "1:印刷対象外")
        Me.ddlSprint.Items(1).Value = "1"
        Me.ddlSprint.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

        '編集側
        Me.ddlprint.ppDropDownList.Items.Clear()
        Me.ddlprint.ppDropDownList.DataTextField = "印刷区分"
        Me.ddlprint.ppDropDownList.DataValueField = "印刷区分コード"
        Me.ddlprint.ppDropDownList.Items.Insert(0, "0:印刷対象")
        Me.ddlprint.ppDropDownList.Items(0).Value = "0"
        Me.ddlprint.ppDropDownList.Items.Insert(1, "1:印刷対象外")
        Me.ddlprint.ppDropDownList.Items(1).Value = "1"
        Me.ddlprint.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
    End Sub

    ''' <summary>
    ''' TBOX種別ドロップダウンリスト作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlTbox()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZMSTSEL004", objCn)
                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                '検索条件(削除済みデータ含む)
                Me.ddlSTbox.Items.Clear()
                Me.ddlSTbox.DataSource = objDs.Tables(0)
                Me.ddlSTbox.DataTextField = "ＴＢＯＸリスト"
                Me.ddlSTbox.DataValueField = "ＴＢＯＸシステムコード"
                Me.ddlSTbox.DataBind()
                Me.ddlSTbox.Items.Insert(0, "**:ダミー") 'ダミーを追加
                Me.ddlSTbox.Items(0).Value = "**"
                Me.ddlSTbox.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

                '編集エリア
                Me.ddlTbox.Items.Clear()
                Me.ddlTbox.DataSource = objDs.Tables(1)
                Me.ddlTbox.DataTextField = "ＴＢＯＸリスト"
                Me.ddlTbox.DataValueField = "ＴＢＯＸシステムコード"
                Me.ddlTbox.DataBind()
                Me.ddlTbox.Items.Insert(0, "**:ダミー") 'ダミーを追加
                Me.ddlTbox.Items(0).Value = "**"
                Me.ddlTbox.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

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
    ''' グリッド編集
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function gridedit()
        'ヘッダテキスト設定
        'COMUPDM40-002
        Dim strHeader As String() = New String() {"選択", "印刷", "XNL区分コード", "XNL区分", "TBOX種別コード", "TBOX種別", "印刷区分コード", "印刷区分", "連番",
                                                  "送付先１名称", "送付先１郵便番号", "送付先１住所", "送付先１気付", "送付先１電話番号", "送付先１FAX番号",
                                                  "送付先１機器情報１", "送付先１機器情報２",
                                                  "送付先２名称", "送付先２郵便番号", "送付先２住所", "送付先２気付", "送付先２電話番号", "送付先２FAX番号",
                                                  "送付先２機器情報１", "送付先２機器情報２",
                                                  "送付先３名称", "送付先３郵便番号", "送付先３住所", "送付先３気付", "送付先３電話番号", "送付先３FAX番号",
                                                  "送付先３機器情報１", "送付先３機器情報２", "備考", "削除"}
        'COMUPDM40-002
        Try
            If Not IsPostBack Then

                'COMUPDM40-002
                'For clm As Integer = 2 To 26
                For clm As Integer = 2 To 33
                    grvList.Columns(clm).HeaderText = strHeader(clm)
                Next
                'COMUPDM40-002

            End If

            For Each rowData As GridViewRow In grvList.Rows
                '    '奇数行背景調整
                If rowData.RowIndex Mod 2 <> 0 Then
                    CType(rowData.FindControl("XNL区分コード"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("XNL区分"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("TBOX種別コード"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("TBOX種別"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("印刷区分コード"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("印刷区分"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("連番"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先１名称"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先１郵便番号"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先１住所"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先１気付"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先１電話番号"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先１FAX番号"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先１機器情報１"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先１機器情報２"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先２名称"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先２郵便番号"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先２住所"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先２気付"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先２電話番号"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先２FAX番号"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先２機器情報１"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先２機器情報２"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("備考"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)


                    CType(rowData.FindControl("送付先３名称"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先３郵便番号"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先３住所"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先３気付"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先３電話番号"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先３FAX番号"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先３機器情報１"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                    CType(rowData.FindControl("送付先３機器情報２"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)


                Else
                    CType(rowData.FindControl("XNL区分コード"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("XNL区分"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("TBOX種別コード"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("TBOX種別"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("印刷区分コード"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("印刷区分"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("連番"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先１名称"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先１郵便番号"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先１住所"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先１気付"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先１電話番号"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先１FAX番号"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先１機器情報１"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先１機器情報２"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先２名称"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先２郵便番号"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先２住所"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先２気付"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先２電話番号"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先２FAX番号"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先２機器情報１"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先２機器情報２"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("備考"), TextBox).BackColor = Drawing.Color.White


                    CType(rowData.FindControl("送付先３名称"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先３郵便番号"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先３住所"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先３気付"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先３電話番号"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先３FAX番号"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先３機器情報１"), TextBox).BackColor = Drawing.Color.White
                    CType(rowData.FindControl("送付先３機器情報２"), TextBox).BackColor = Drawing.Color.White



                End If

                '印刷対象外青字化
                If CType(rowData.FindControl(strHeader(6)), TextBox).Text.Trim = "1" AndAlso CType(rowData.FindControl(strHeader(34)), TextBox).Text.Trim <> "●" Then
                    CType(rowData.FindControl("XNL区分コード"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("XNL区分"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("TBOX種別コード"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("TBOX種別"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("印刷区分コード"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("印刷区分"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("連番"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先１名称"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先１郵便番号"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先１住所"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先１気付"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先１電話番号"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先１FAX番号"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先１機器情報１"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先１機器情報２"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先２名称"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先２郵便番号"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先２住所"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先２気付"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先２電話番号"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先２FAX番号"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先２機器情報１"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先２機器情報２"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("備考"), TextBox).ForeColor = Drawing.Color.Blue

                    CType(rowData.FindControl("送付先３名称"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先３郵便番号"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先３住所"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先３気付"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先３電話番号"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先３FAX番号"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先３機器情報１"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("送付先３機器情報２"), TextBox).ForeColor = Drawing.Color.Blue

                End If



                '削除フラグ１の時該当行赤字化
                If CType(rowData.FindControl(strHeader(34)), TextBox).Text.Trim = "●" Then
                    CType(rowData.FindControl("XNL区分コード"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("XNL区分"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("TBOX種別コード"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("TBOX種別"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("印刷区分コード"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("印刷区分"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("連番"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先１名称"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先１郵便番号"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先１住所"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先１気付"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先１電話番号"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先１FAX番号"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先１機器情報１"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先１機器情報２"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先２名称"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先２郵便番号"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先２住所"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先２気付"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先２電話番号"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先２FAX番号"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先２機器情報１"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先２機器情報２"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("備考"), TextBox).ForeColor = Drawing.Color.Red

                    CType(rowData.FindControl("送付先３名称"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先３郵便番号"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先３住所"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先３気付"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先３電話番号"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先３FAX番号"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先３機器情報１"), TextBox).ForeColor = Drawing.Color.Red
                    CType(rowData.FindControl("送付先３機器情報２"), TextBox).ForeColor = Drawing.Color.Red

                End If
            Next
        Catch ex As Exception
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
        Return True
    End Function

    ''' <summary>
    ''' 連番１桁時先頭に０を追加
    ''' </summary>
    ''' <param name="num">trun</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Private Function trun_str(ByVal num) As String
        If num.ToString.Length = 1 Then
            num = "0" & num
        End If
        Return num
    End Function

    ''' <summary>
    ''' エラーサマリー編集
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EditValid()
        Dim cuv As CustomValidator
        cuv = txtZipNo1.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "故障機器送付先１" & cuv.ErrorMessage
        cuv = txtTelNo1.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "故障機器送付先１" & cuv.ErrorMessage
        cuv = txtFaxNo1.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "故障機器送付先１" & cuv.ErrorMessage

        cuv = txtZipNo2.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "故障機器送付先２" & cuv.ErrorMessage
        cuv = txtTelNo2.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "故障機器送付先２" & cuv.ErrorMessage
        cuv = txtFaxNo2.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "故障機器送付先２" & cuv.ErrorMessage

        cuv = txtZipNo3.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "故障機器送付先３" & cuv.ErrorMessage
        cuv = txtTelNo3.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "故障機器送付先３" & cuv.ErrorMessage
        cuv = txtFaxNo3.FindControl("pnlErr").FindControl("cuvTextBox")
        cuv.ErrorMessage = "故障機器送付先３" & cuv.ErrorMessage

    End Sub
#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
