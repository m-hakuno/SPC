'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　配送機器一覧表
'*　ＰＧＭＩＤ：　EQULSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.28　：　土岐
'*  作　成　　：　2014.06.23　：　エラーチェックと画面制御の修正
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
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

Public Class EQULSTP001
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
    Private Const M_DISP_ID As String = P_FUN_EQU & P_SCR_LST & P_PAGE & "001"
    Private Const M_VIEW_MNT_DT_F As String = "保守対応日From"
    Private Const M_VIEW_MNT_DT_T As String = "保守対応日To"
    Private Const M_VIEW_MNT_NO_F As String = "保守管理番号From"
    Private Const M_VIEW_MNT_NO_T As String = "保守管理番号To"
    Private Const M_VIEW_SITUATION As String = "進捗状況"
    Private Const M_VIEW_SEARCH_F As String = "検索フラグ"
    Private Const M_VIEW_HDD_T As String = "HDDテーブル"
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
    Dim strflg As String = "Default"

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '表設定
        pfSet_GridView(grvList, "EQULSTP001")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnSearchClear_Click

        'テキストボックスアクションの設定
        AddHandler txtDelivery.ppTextBox.TextChanged, AddressOf txtDelivery_TextChanged
        AddHandler txtPoint.ppTextBox.TextChanged, AddressOf txtPoint_TextChanged
        AddHandler txtSPoint.ppTextBox.TextChanged, AddressOf txtSPoint_TextChanged
        Me.txtNotes.ppTextBox.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txtNotes.ppMaxLength & """);")
        Me.txtNotes.ppTextBox.Attributes.Add("onChange", "lenCheck(this,""" & Me.txtNotes.ppMaxLength & """);")
        Me.txtDelivery.ppTextBox.AutoPostBack = True
        Me.txtPoint.ppTextBox.AutoPostBack = True
        Me.txtSPoint.ppTextBox.AutoPostBack = True

        'ドロップダウンリストアクションの設定
        AddHandler ddlDelivery.ppDropDownList.SelectedIndexChanged, AddressOf ddlDelivery_SelectedIndexChanged
        AddHandler ddlPoint.ppDropDownList.SelectedIndexChanged, AddressOf ddlPoint_SelectedIndexChanged
        AddHandler ddlSPoint.ppDropDownList.SelectedIndexChanged, AddressOf ddlSPoint_SelectedIndexChanged
        Me.ddlDelivery.ppDropDownList.AutoPostBack = True
        Me.ddlPoint.ppDropDownList.AutoPostBack = True
        Me.ddlSPoint.ppDropDownList.AutoPostBack = True

        If Not IsPostBack Then  '初回表示

            'プログラムＩＤ、画面名設定
            Master.Master.ppProgramID = M_DISP_ID
            Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

            'パンくずリスト設定
            Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

            '「クリア」ボタン押下時の検証を無効
            Master.ppRigthButton2.CausesValidation = False

            'ボタンメッセージ設定
            Me.btnDel.OnClientClick =
                pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "配送機器")

            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            'ドロップダウンリスト設定
            msSetddlStatus()            '進捗ステータス
            msSetddlAppaclass()         '機器分類
            msSetddlTboxType()          'ＴＢＯＸタイプ
            msSetHDD()                  'ＨＤＤ

            '画面クリア
            msClearScreen()

            '初期表示
            msGet_Data("",
                       "",
                       "",
                       "",
                       "",
                       "",
                       "",
                       "",
                       "",
                       M_DISP_ID,
                       True)

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
                Me.dtbDeliveryDT.ppEnabled = False
                Me.dtbArrivePlanDT.ppEnabled = False
            Case "NGC"
        End Select

        If strflg <> "Through" Then
            If strflg = "Default" OrElse Me.ddlStatus.SelectedValue = "01" Then
                Me.ddlAppaClass.Enabled = True
                Me.ddlAppaClassCD.Enabled = True
                Me.ddlTboxType.Enabled = True
                Me.cuvTboxType.Enabled = True
                Me.TxtTboxVer.ppEnabled = True
                Me.ddlAppaNM.Enabled = True
                If Me.ddlAppaClass.SelectedValue = "01" AndAlso Me.ddlAppaClassCD.SelectedValue = "09" Then
                    Me.ddlHDDNo.Enabled = True
                    Me.ddlHDDCls.Enabled = True
                Else
                    Me.ddlHDDNo.Enabled = False
                    Me.ddlHDDCls.Enabled = False
                    Me.ddlHDDNo.SelectedIndex = -1
                    Me.ddlHDDCls.SelectedIndex = -1
                End If
                Me.txtSerialNo.ppEnabled = True
            Else
                Me.ddlAppaClass.Enabled = False
                Me.ddlAppaClassCD.Enabled = False
                Me.ddlTboxType.Enabled = False
                Me.cuvTboxType.Enabled = False
                Me.TxtTboxVer.ppEnabled = False
                Me.ddlAppaNM.Enabled = False
                Me.ddlHDDNo.Enabled = False
                Me.ddlHDDCls.Enabled = False
                If Me.ddlStatus.SelectedValue = "04" Then
                    Me.txtSerialNo.ppEnabled = False
                Else
                    Me.txtSerialNo.ppEnabled = True
                End If
            End If
        End If

    End Sub
    '---------------------------
    '2014/04/15 武 ここまで
    '---------------------------

    Private Sub grvList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grvList.RowDataBound

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
                e.Row.Cells(1).Enabled = False
            Case "NGC"
        End Select

    End Sub

    ''' <summary>
    ''' 明細登録ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim intRtn As String
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim intIndex As Integer = 0
        Dim drData As DataRow

        objStack = New StackFrame

        'ログ出力開始
        psLogStart(Me)

        msCheck_Error("Edit")

        If (Page.IsValid) Then
            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    'データ追加／更新
                    Using conTrn = conDB.BeginTransaction
                        'ストアド戻り値チェック
                        '-----------------------------
                        '2014/05/19 土岐　ここから
                        '-----------------------------
                        intRtn = mfUpdateDTL(conDB,
                                             conTrn,
                                             "1")
                        '-----------------------------
                        '2014/05/19 土岐　ここまで
                        '-----------------------------
                        If intRtn.IndexOf("正常") < 0 Then
                            psMesBox(Me, "00008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "配送機器", intRtn.ToString)
                            Exit Sub
                        End If

                        'コミット
                        conTrn.Commit()
                    End Using

                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "配送機器")

                    Dim dttGrid As DataTable = Nothing

                    'GridViewからテーブル取得し結合
                    dttGrid = pfParse_DataTable(Me.grvList)
                    drData = dttGrid.NewRow()

                    drData.Item("管理番号") = Me.txtMntNo.ppText
                    drData.Item("シリアルＮＯ") = Me.txtSerialNo.ppText
                    drData.Item("進捗状況コード") = Me.ddlStatus.SelectedValue
                    drData.Item("進捗状況") = Me.ddlStatus.SelectedItem.ToString.Split(":")(1)
                    If Me.ddlAppaClass.SelectedIndex > 0 Then
                        drData.Item("機器分類コード") = Me.ddlAppaClass.SelectedValue
                        drData.Item("機器分類") = Me.ddlAppaClass.SelectedItem.ToString.Split(":")(1)
                    Else
                        drData.Item("機器分類コード") = ""
                        drData.Item("機器分類") = ""
                    End If
                    If Me.ddlAppaClassCD.SelectedIndex > 0 Then
                        drData.Item("機器種別コード") = Me.ddlAppaClassCD.SelectedValue
                        drData.Item("機器種別") = Me.ddlAppaClassCD.SelectedItem.ToString.Split(":")(1)
                    Else
                        drData.Item("機器種別コード") = ""
                        drData.Item("機器種別") = ""
                    End If
                    If Me.ddlTboxType.SelectedIndex > 0 Then
                        drData.Item("ＴＢＯＸタイプコード") = Me.ddlTboxType.SelectedValue
                        drData.Item("ＴＢＯＸタイプ") = Me.ddlTboxType.SelectedItem.ToString.Split(":")(1)
                    Else
                        drData.Item("ＴＢＯＸタイプコード") = ""
                        drData.Item("ＴＢＯＸタイプ") = ""
                    End If

                    drData.Item("ＶＥＲ") = Me.TxtTboxVer.ppText
                    drData.Item("型式機器") = Me.ddlAppaNM.SelectedValue
                    drData.Item("ＨＤＤＮＯ") = Me.ddlHDDNo.SelectedValue
                    drData.Item("ＨＤＤ種別") = Me.ddlHDDCls.SelectedValue
                    If Me.ddlDelivery.ppDropDownList.SelectedIndex > 0 Then
                        drData.Item("配送元区分") = Me.ddlDelivery.ppDropDownList.SelectedValue
                    Else
                        drData.Item("配送元区分") = ""
                    End If
                    drData.Item("配送元") = Me.lblDelivery.Text
                    drData.Item("配送元コード") = Me.txtDelivery.ppText
                    If Me.ddlPoint.ppDropDownList.SelectedIndex > 0 Then
                        drData.Item("配送先区分") = Me.ddlPoint.ppDropDownList.SelectedValue
                    Else
                        drData.Item("配送先区分") = ""
                    End If
                    drData.Item("配送先名") = Me.lblPoint.Text
                    drData.Item("配送先コード") = Me.txtPoint.ppText

                    drData.Item("発送日") = Me.dtbDeliveryDT.ppText
                    drData.Item("致着予定日") = Me.dtbArrivePlanDT.ppText
                    drData.Item("致着日") = Me.dtbArriveDT.ppText
                    drData.Item("備考") = Me.txtNotes.ppText
                    drData.Item("発行番号") = intRtn.Replace("正常", "")

                    dttGrid.Rows.Add(drData)

                    '結合したデータをセット
                    dttGrid.AcceptChanges()
                    Me.grvList.DataSource = dttGrid
                    Me.grvList.DataBind()

                    Master.ppCount = dttGrid.Rows.Count

                    '明細クリア
                    msClearSelect()

                Catch ex As Exception
                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "配送機器")
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
        Else
            strflg = "Through"
        End If
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 明細更新ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim intRtn As String
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'ログ出力開始
        psLogStart(Me)

        msCheck_Error("Edit")

        If (Page.IsValid) Then
            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    'データ追加／更新
                    Using conTrn = conDB.BeginTransaction
                        'ストアド戻り値チェック
                        '-----------------------------
                        '2014/05/19 土岐　ここから
                        '-----------------------------
                        intRtn = mfUpdateDTL(conDB,
                                             conTrn,
                                             "2")
                        '-----------------------------
                        '2014/05/19 土岐　ここまで
                        '-----------------------------
                        If intRtn.IndexOf("正常") < 0 Then
                            psMesBox(Me, "00007", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "配送機器", intRtn.ToString)
                            Exit Sub
                        End If

                        'コミット
                        conTrn.Commit()
                    End Using

                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "配送機器")

                    Dim dttGrid As DataTable = Nothing

                    'GridViewからテーブル取得し結合
                    dttGrid = pfParse_DataTable(Me.grvList)
                    For intIndex = 0 To dttGrid.Rows.Count - 1
                        If dttGrid.Rows(intIndex).Item("発行番号").ToString = Me.lblPbrnNo.Text Then
                            dttGrid.Rows(intIndex).Item("管理番号") = Me.txtMntNo.ppText
                            dttGrid.Rows(intIndex).Item("シリアルＮＯ") = Me.txtSerialNo.ppText
                            dttGrid.Rows(intIndex).Item("進捗状況コード") = Me.ddlStatus.SelectedValue
                            dttGrid.Rows(intIndex).Item("進捗状況") = Me.ddlStatus.SelectedItem.ToString.Split(":")(1)
                            If Me.ddlAppaClass.SelectedIndex > 0 Then
                                dttGrid.Rows(intIndex).Item("機器分類コード") = Me.ddlAppaClass.SelectedValue
                                dttGrid.Rows(intIndex).Item("機器分類") = Me.ddlAppaClass.SelectedItem.ToString.Split(":")(1)
                            Else
                                dttGrid.Rows(intIndex).Item("機器分類コード") = ""
                                dttGrid.Rows(intIndex).Item("機器分類") = ""
                            End If
                            If Me.ddlAppaClassCD.SelectedIndex > 0 Then
                                dttGrid.Rows(intIndex).Item("機器種別コード") = Me.ddlAppaClassCD.SelectedValue
                                dttGrid.Rows(intIndex).Item("機器種別") = Me.ddlAppaClassCD.SelectedItem.ToString.Split(":")(1)
                            Else
                                dttGrid.Rows(intIndex).Item("機器種別コード") = ""
                                dttGrid.Rows(intIndex).Item("機器種別") = ""
                            End If
                            If Me.ddlTboxType.SelectedIndex > 0 Then
                                dttGrid.Rows(intIndex).Item("ＴＢＯＸタイプコード") = Me.ddlTboxType.SelectedValue
                                dttGrid.Rows(intIndex).Item("ＴＢＯＸタイプ") = Me.ddlTboxType.SelectedItem.ToString.Split(":")(1)
                            Else
                                dttGrid.Rows(intIndex).Item("ＴＢＯＸタイプコード") = ""
                                dttGrid.Rows(intIndex).Item("ＴＢＯＸタイプ") = ""
                            End If

                            dttGrid.Rows(intIndex).Item("ＶＥＲ") = Me.TxtTboxVer.ppText
                            dttGrid.Rows(intIndex).Item("型式機器") = Me.ddlAppaNM.SelectedValue
                            dttGrid.Rows(intIndex).Item("ＨＤＤＮＯ") = Me.ddlHDDNo.SelectedValue
                            dttGrid.Rows(intIndex).Item("ＨＤＤ種別") = Me.ddlHDDCls.SelectedValue
                            If Me.ddlDelivery.ppDropDownList.SelectedIndex > 0 Then
                                dttGrid.Rows(intIndex).Item("配送元区分") = Me.ddlDelivery.ppDropDownList.SelectedValue
                            Else
                                dttGrid.Rows(intIndex).Item("配送元区分") = ""
                            End If
                            dttGrid.Rows(intIndex).Item("配送元") = Me.lblDelivery.Text
                            dttGrid.Rows(intIndex).Item("配送元コード") = Me.txtDelivery.ppText
                            If Me.ddlPoint.ppDropDownList.SelectedIndex > 0 Then
                                dttGrid.Rows(intIndex).Item("配送先区分") = Me.ddlPoint.ppDropDownList.SelectedValue
                            Else
                                dttGrid.Rows(intIndex).Item("配送先区分") = ""
                            End If
                            dttGrid.Rows(intIndex).Item("配送先名") = Me.lblPoint.Text
                            dttGrid.Rows(intIndex).Item("配送先コード") = Me.txtPoint.ppText

                            dttGrid.Rows(intIndex).Item("発送日") = Me.dtbDeliveryDT.ppText
                            dttGrid.Rows(intIndex).Item("致着予定日") = Me.dtbArrivePlanDT.ppText
                            dttGrid.Rows(intIndex).Item("致着日") = Me.dtbArriveDT.ppText
                            dttGrid.Rows(intIndex).Item("備考") = Me.txtNotes.ppText

                            Exit For
                        End If
                    Next

                    '結合したデータをセット
                    dttGrid.AcceptChanges()
                    Me.grvList.DataSource = dttGrid
                    Me.grvList.DataBind()

                    '明細クリア
                    msClearSelect()

                Catch ex As Exception
                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "配送機器")
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
        Else
            strflg = "Through"
        End If
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 明細削除ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDel_Click(sender As Object, e As EventArgs) Handles btnDel.Click
        Dim intRtn As Integer
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'ログ出力開始
        psLogStart(Me)
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                'データ削除
                Using conTrn = conDB.BeginTransaction
                    'ストアド戻り値チェック
                    intRtn = mfDeleteDTL(conDB,
                                         conTrn,
                                         Me.lblPbrnNo.Text)
                    If intRtn <> 0 Then
                        psMesBox(Me, "00009", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "配送機器", intRtn.ToString)
                        Exit Sub
                    End If

                    'コミット
                    conTrn.Commit()
                End Using

                psMesBox(Me, "00002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "配送機器")

                Dim dttGrid As DataTable = Nothing

                'GridViewからテーブル取得し結合
                dttGrid = pfParse_DataTable(Me.grvList)
                For intIndex = 0 To dttGrid.Rows.Count - 1
                    If dttGrid.Rows(intIndex).Item("発行番号").ToString = Me.lblPbrnNo.Text Then
                        dttGrid.Rows(intIndex).Delete()
                        Exit For
                    End If
                Next
                '結合したデータをセット
                dttGrid.AcceptChanges()
                Me.grvList.DataSource = dttGrid
                Me.grvList.DataBind()

                Master.ppCount = dttGrid.Rows.Count

                '明細クリア
                msClearSelect()

            Catch ex As Exception
                psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "配送機器")
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
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 明細クリアボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        'ログ出力開始
        psLogStart(Me)
        msClearSelect()
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 明細ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand
        Const CNSUPDP002 = "~/" & P_CNS & "/" &
            P_FUN_CNS & P_SCR_UPD & P_PAGE & "002/" &
            P_FUN_CNS & P_SCR_UPD & P_PAGE & "002.aspx"
        Dim intIndex As Integer = Nothing
        Dim rowData As GridViewRow = Nothing

        If e.CommandName <> "btnSelect" And e.CommandName <> "btnCNSUPDP002" Then
            Exit Sub
        End If

        'ログ出力開始
        psLogStart(Me)
        intIndex = Convert.ToInt32(e.CommandArgument)   'ボタン押下行のIndex
        rowData = grvList.Rows(intIndex)                'ボタン押下行

        Select Case e.CommandName
            '選択ボタン
            Case "btnSelect"
                '-----------------------------
                '2014/04/21 土岐　ここから
                '-----------------------------
                '★排他制御処理
                Dim strExclusiveDate As String = Nothing
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList
                'Select Case ViewState(P_SESSION_TERMS)
                '    Case  ClsComVer.E_遷移条件.更新
                Select Case Session(P_SESSION_AUTH)
                    Case "管理者", "SPC", "営業所"

                        '★排他情報削除
                        If Not Me.Master.Master.ppExclusiveDateDtl = String.Empty Then

                            If clsExc.pfDel_Exclusive(Me _
                                               , Session(P_SESSION_SESSTION_ID) _
                                               , Me.Master.Master.ppExclusiveDateDtl) = 0 Then
                                Me.Master.Master.ppExclusiveDateDtl = String.Empty
                            Else
                                Exit Sub
                            End If
                        End If

                        '★ロック対象テーブル名の登録
                        arTable_Name.Insert(0, "D79_DELIVERY_EQUIP")

                        '★ロックテーブルキー項目の登録(D79_DELIVERY_EQUIP)
                        arKey.Insert(0, CType(rowData.FindControl("発行番号"), TextBox).Text)       'D79_PBRN_NO

                        '★排他情報確認処理(更新画面へ遷移)
                        If clsExc.pfSel_Exclusive(strExclusiveDate _
                                         , Me _
                                         , Session(P_SESSION_IP) _
                                         , Session(P_SESSION_PLACE) _
                                         , Session(P_SESSION_USERID) _
                                         , Session(P_SESSION_SESSTION_ID) _
                                         , ViewState(P_SESSION_GROUP_NUM) _
                                         , M_DISP_ID _
                                         , arTable_Name _
                                         , arKey) = 0 Then

                            '★登録年月日時刻(明細)
                            Me.Master.Master.ppExclusiveDateDtl = strExclusiveDate

                        Else

                            '排他ロック中
                            Exit Sub

                        End If
                        '-----------------------------
                        '2014/06/20 武　ここから
                        '-----------------------------
                    Case Else
                        '★登録年月日時刻(明細)
                        Me.Master.Master.ppExclusiveDateDtl = strExclusiveDate
                        '-----------------------------
                        '2014/06/20 武　ここまで
                        '-----------------------------
                End Select
                '-----------------------------
                '2014/04/21 土岐　ここまで
                '-----------------------------
                Me.ddlAppaClass.SelectedValue = CType(rowData.FindControl("機器分類コード"), TextBox).Text
                '機器種別リスト設定
                msSetddlAppaGrouping(Me.ddlAppaClass.SelectedValue)
                Me.ddlAppaClassCD.SelectedValue = CType(rowData.FindControl("機器種別コード"), TextBox).Text
                Me.ddlTboxType.SelectedValue = CType(rowData.FindControl("ＴＢＯＸタイプコード"), TextBox).Text
                Me.TxtTboxVer.ppText = CType(rowData.FindControl("ＶＥＲ"), TextBox).Text
                msSetAppaData()
                For zz = 0 To ddlAppaNM.Items.Count - 1
                    If Me.ddlAppaNM.Items(zz).Text = CType(rowData.FindControl("型式機器"), TextBox).Text Then
                        Me.ddlAppaNM.SelectedValue = CType(rowData.FindControl("型式機器"), TextBox).Text
                        Exit For
                    Else
                        Me.ddlAppaNM.SelectedIndex = -1
                    End If
                Next
                Me.ddlHDDNo.SelectedValue = CType(rowData.FindControl("ＨＤＤＮＯ"), TextBox).Text
                Me.ddlHDDCls.SelectedValue = CType(rowData.FindControl("ＨＤＤ種別"), TextBox).Text
                Me.txtSerialNo.ppText = CType(rowData.FindControl("シリアルＮＯ"), TextBox).Text
                Me.ddlDelivery.ppDropDownList.SelectedValue = CType(rowData.FindControl("配送元区分"), TextBox).Text
                Me.txtDelivery.ppText = CType(rowData.FindControl("配送元コード"), TextBox).Text
                Me.lblDelivery.Text = CType(rowData.FindControl("配送元"), TextBox).Text
                Me.dtbDeliveryDT.ppText = CType(rowData.FindControl("発送日"), TextBox).Text
                Me.ddlPoint.ppDropDownList.SelectedValue = CType(rowData.FindControl("配送先区分"), TextBox).Text
                Me.txtPoint.ppText = CType(rowData.FindControl("配送先コード"), TextBox).Text
                Me.lblPoint.Text = CType(rowData.FindControl("配送先名"), TextBox).Text
                Me.dtbArrivePlanDT.ppText = CType(rowData.FindControl("致着予定日"), TextBox).Text
                Me.dtbArriveDT.ppText = CType(rowData.FindControl("致着日"), TextBox).Text
                Me.txtMntNo.ppText = CType(rowData.FindControl("管理番号"), TextBox).Text
                '--- 追加 2014/05/26 START ----
                Me.ddlStatus.SelectedValue = CType(rowData.FindControl("進捗状況コード"), TextBox).Text
                '--- 追加 2014/05/26 END   ----
                Me.lblPbrnNo.Text = CType(rowData.FindControl("発行番号"), TextBox).Text
                Me.txtNotes.ppText = CType(rowData.FindControl("備考"), TextBox).Text

                Me.btnAdd.Enabled = False           '登録ボタン
                Me.btnUpdate.Enabled = True         '更新ボタン
                Me.btnDel.Enabled = True            '削除ボタン

                strflg = "Select"

                '物品転送ボタン
            Case "btnCNSUPDP002"
                Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
                Session(P_KEY) = {CType(rowData.FindControl("管理番号"), TextBox).Text}
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録
                Session(P_SESSION_OLDDISP) = M_DISP_ID
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
                                objStack.GetMethod.Name, CNSUPDP002, strPrm, "TRANS")
                '■□■□結合試験時のみ使用予定□■□■
                '--------------------------------
                '2014/04/16 星野　ここまで
                '--------------------------------
                psOpen_Window(Me, CNSUPDP002)

                strflg = "Through"

        End Select
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 検索ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)
        'ログ出力開始
        psLogStart(Me)

        msCheck_Error("Search")

        If (Page.IsValid) Then
            '検索
            msGet_Data(Me.dftMntDT.ppFromText,
                       Me.dftMntDT.ppToText,
                       Me.dftArvDT.ppFromText,
                       Me.dftArvDT.ppToText,
                       Me.tftMntNo.ppFromText,
                       Me.tftMntNo.ppToText,
                       Me.ddlSituation.SelectedValue,
                       Me.ddlSPoint.ppSelectedValue,
                       Me.txtSPoint.ppText,
                       M_DISP_ID,
                       False)
            '-----------------------------
            '2014/05/12 土岐　ここから
            '-----------------------------
        End If
        '-----------------------------
        '2014/05/12 土岐　ここまで
        '-----------------------------
        '明細クリア
        msClearSelect()

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 検索条件クリアボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearchClear_Click(sender As Object, e As EventArgs)
        'ログ出力開始
        psLogStart(Me)

        'クリア処理
        msClearSearch()

        strflg = "Through"

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 機器分類変更時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlAppaClass_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAppaClass.SelectedIndexChanged
        msSetddlAppaGrouping(Me.ddlAppaClass.SelectedValue)

        Me.ddlAppaClassCD.SelectedIndex = -1
        Me.ddlTboxType.SelectedIndex = -1
        Me.ddlAppaNM.Items.Clear()
        Me.ddlHDDNo.SelectedIndex = -1
        Me.ddlHDDCls.SelectedIndex = -1
    End Sub

    ''' <summary>
    ''' 機器種別変更時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlAppaClassCD_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAppaClassCD.SelectedIndexChanged
        msSetAppaData()
    End Sub

    ''' <summary>
    ''' ＴＢＯＸタイプ指定
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlTboxType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTboxType.SelectedIndexChanged
        msSetAppaData()
    End Sub

    ''' <summary>
    ''' 配送元区分変更時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlDelivery_SelectedIndexChanged()

        Dim strbuff As String = String.Empty

        If Me.ddlDelivery.ppSelectedValue <> String.Empty AndAlso Me.txtDelivery.ppText <> String.Empty Then
            Select Case Me.ddlDelivery.ppSelectedValue
                Case 1  '営業所名、または保守拠点名取得
                    If mfGetOfficeInfo(Me.txtDelivery.ppText, strbuff, "2,3") Then
                        Me.lblDelivery.Text = strbuff
                    Else
                        Me.lblDelivery.Text = String.Empty
                        Me.txtDelivery.psSet_ErrorNo("2002", "入力した配送元コード")
                        Me.txtDelivery.ppTextBox.Focus()
                    End If
                Case 3  '倉庫名取得
                    If mfGetOfficeInfo(Me.txtDelivery.ppText, strbuff, "6") Then
                        Me.lblDelivery.Text = strbuff
                    Else
                        Me.lblDelivery.Text = String.Empty
                        Me.txtDelivery.psSet_ErrorNo("2002", "入力した配送元コード")
                        Me.txtDelivery.ppTextBox.Focus()
                    End If
                Case Else
                    Me.lblDelivery.Text = String.Empty
            End Select
        Else
            Me.lblDelivery.Text = String.Empty
        End If

        strflg = "Through"

    End Sub

    ''' <summary>
    ''' 配送先区分(検索条件)変更時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlSPoint_SelectedIndexChanged()

        Dim strbuff As String = String.Empty

        If Me.txtSPoint.ppText <> String.Empty AndAlso Me.ddlSPoint.ppSelectedValue <> String.Empty Then
            Select Case Me.ddlSPoint.ppSelectedValue
                Case 1  '営業所名、または保守拠点名取得
                    If mfGetOfficeInfo(Me.txtSPoint.ppText, strbuff, "2,3") Then
                        Me.lblPointNm.Text = strbuff
                    Else
                        Me.lblPointNm.Text = String.Empty
                        Me.txtSPoint.psSet_ErrorNo("2002", "入力した配送先コード")
                        Me.txtSPoint.ppTextBox.Focus()
                    End If
                Case 3  '倉庫名取得
                    If mfGetOfficeInfo(Me.txtSPoint.ppText, strbuff, "6") Then
                        Me.lblPointNm.Text = strbuff
                    Else
                        Me.lblPointNm.Text = String.Empty
                        Me.txtSPoint.psSet_ErrorNo("2002", "入力した配送元コード")
                        Me.txtSPoint.ppTextBox.Focus()
                    End If
                Case Else
                    Me.lblPointNm.Text = String.Empty
            End Select
        Else
            Me.lblPointNm.Text = String.Empty
        End If

        strflg = "Through"

    End Sub

    ''' <summary>
    ''' 配送先区分変更時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlPoint_SelectedIndexChanged()

        Dim strbuff As String = String.Empty

        If Me.txtPoint.ppText <> String.Empty AndAlso Me.ddlPoint.ppSelectedValue <> String.Empty Then
            Select Case Me.ddlPoint.ppSelectedValue
                Case 1  '営業所名、または保守拠点名取得
                    If mfGetOfficeInfo(Me.txtPoint.ppText, strbuff, "2,3") Then
                        Me.lblPoint.Text = strbuff
                    Else
                        Me.lblPoint.Text = String.Empty
                        Me.txtPoint.psSet_ErrorNo("2002", "入力した配送先コード")
                        Me.txtPoint.ppTextBox.Focus()
                    End If
                Case 3  '倉庫名取得
                    If mfGetOfficeInfo(Me.txtPoint.ppText, strbuff, "6") Then
                        Me.lblPoint.Text = strbuff
                    Else
                        Me.lblPoint.Text = String.Empty
                        Me.txtPoint.psSet_ErrorNo("2002", "入力した配送元コード")
                        Me.txtPoint.ppTextBox.Focus()
                    End If
                Case Else
                    Me.lblPoint.Text = String.Empty
            End Select
        Else
            Me.lblPoint.Text = String.Empty
        End If

        strflg = "Through"

    End Sub

    ''' <summary>
    ''' 配送元コード変更時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub txtDelivery_TextChanged()

        Dim strbuff As String = String.Empty

        If Me.ddlDelivery.ppSelectedValue <> String.Empty AndAlso Me.txtDelivery.ppText <> String.Empty Then
            Select Case Me.ddlDelivery.ppSelectedValue
                Case 1  '営業所名、または保守拠点名取得
                    If mfGetOfficeInfo(Me.txtDelivery.ppText, strbuff, "2,3") Then
                        Me.lblDelivery.Text = strbuff
                    Else
                        Me.lblDelivery.Text = String.Empty
                        Me.txtDelivery.psSet_ErrorNo("2002", "入力した配送元コード")
                        Me.txtDelivery.ppTextBox.Focus()
                    End If
                Case 3  '倉庫名取得
                    If mfGetOfficeInfo(Me.txtDelivery.ppText, strbuff, "6") Then
                        Me.lblDelivery.Text = strbuff
                    Else
                        Me.lblDelivery.Text = String.Empty
                        Me.txtDelivery.psSet_ErrorNo("2002", "入力した配送元コード")
                        Me.txtDelivery.ppTextBox.Focus()
                    End If
                Case Else
                    Me.lblDelivery.Text = String.Empty
            End Select
        Else
            Me.lblDelivery.Text = String.Empty
        End If

        strflg = "Through"

    End Sub

    ''' <summary>
    ''' 配送先コード(検索条件)変更時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub txtSPoint_TextChanged()

        Dim strbuff As String = String.Empty

        If Me.txtSPoint.ppText <> String.Empty AndAlso Me.ddlSPoint.ppSelectedValue <> String.Empty Then
            Select Case Me.ddlSPoint.ppSelectedValue
                Case 1  '営業所名、または保守拠点名取得
                    If mfGetOfficeInfo(Me.txtSPoint.ppText, strbuff, "2,3") Then
                        Me.lblPointNm.Text = strbuff
                    Else
                        Me.lblPointNm.Text = String.Empty
                        Me.txtSPoint.psSet_ErrorNo("2002", "入力した配送先コード")
                        Me.txtSPoint.ppTextBox.Focus()
                    End If
                Case 3  '倉庫名取得
                    If mfGetOfficeInfo(Me.txtSPoint.ppText, strbuff, "6") Then
                        Me.lblPointNm.Text = strbuff
                    Else
                        Me.lblPointNm.Text = String.Empty
                        Me.txtSPoint.psSet_ErrorNo("2002", "入力した配送元コード")
                        Me.txtSPoint.ppTextBox.Focus()
                    End If
                Case Else
                    Me.lblPointNm.Text = String.Empty
            End Select
        Else
            Me.lblPointNm.Text = String.Empty
        End If

        strflg = "Through"

    End Sub

    ''' <summary>
    ''' 配送先コード変更時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub txtPoint_TextChanged()

        Dim strbuff As String = String.Empty

        If Me.txtPoint.ppText <> String.Empty AndAlso Me.ddlPoint.ppSelectedValue <> String.Empty Then
            Select Case Me.ddlPoint.ppSelectedValue
                Case 1  '営業所名、または保守拠点名取得
                    If mfGetOfficeInfo(Me.txtPoint.ppText, strbuff, "2,3") Then
                        Me.lblPoint.Text = strbuff
                    Else
                        Me.lblPoint.Text = String.Empty
                        Me.txtPoint.psSet_ErrorNo("2002", "入力した配送先コード")
                        Me.txtPoint.ppTextBox.Focus()
                    End If
                Case 3  '倉庫名取得
                    If mfGetOfficeInfo(Me.txtPoint.ppText, strbuff, "6") Then
                        Me.lblPoint.Text = strbuff
                    Else
                        Me.lblPoint.Text = String.Empty
                        Me.txtPoint.psSet_ErrorNo("2002", "入力した配送元コード")
                        Me.txtPoint.ppTextBox.Focus()
                    End If
                Case Else
                    Me.lblPoint.Text = String.Empty
            End Select
        Else
            Me.lblPoint.Text = String.Empty
        End If

        strflg = "Through"

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 検索処理
    ''' </summary>
    ''' <param name="ipstrShi_ymd_f">発送日（開始）</param>
    ''' <param name="ipstrShi_ymd_t">発送日（終了）</param>
    ''' <param name="ipstrDep_ymd_f">致着日（開始）</param>
    ''' <param name="ipstrDep_ymd_t">致着日（終了）</param>
    ''' <param name="ipstrMgnt_no_f">管理番号（開始）</param>
    ''' <param name="ipstrMgnt_no_t">管理番号（終了）</param>
    ''' <param name="ipstrStatus_cd">進捗ステータスコード</param>
    ''' <param name="ipstrDly_dvs">配送先区分</param>
    ''' <param name="ipstrDly_cd">配送先コード</param>
    ''' <param name="ipstrDisp_id">画面ＩＤ</param>
    ''' <param name="ipblnReSearch">検索ボタン以外の再検索（True:その他の再検索, False:検索ボタン）</param>
    ''' <remarks></remarks>
    Private Sub msGet_Data(ByVal ipstrShi_ymd_f As String,
                           ByVal ipstrShi_ymd_t As String,
                           ByVal ipstrDep_ymd_f As String,
                           ByVal ipstrDep_ymd_t As String,
                           ByVal ipstrMgnt_no_f As String,
                           ByVal ipstrMgnt_no_t As String,
                           ByVal ipstrStatus_cd As String,
                           ByVal ipstrDly_dvs As String,
                           ByVal ipstrDly_cd As String,
                           ByVal ipstrDisp_id As String,
                           ByVal ipblnReSearch As Boolean)
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
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
                cmdDB = New SqlCommand("EQULSTP001_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("shi_ymd_f", SqlDbType.NVarChar, ipstrShi_ymd_f))
                    .Add(pfSet_Param("shi_ymd_t", SqlDbType.NVarChar, ipstrShi_ymd_t))
                    .Add(pfSet_Param("dep_ymd_f", SqlDbType.NVarChar, ipstrDep_ymd_f))
                    .Add(pfSet_Param("dep_ymd_t", SqlDbType.NVarChar, ipstrDep_ymd_t))
                    .Add(pfSet_Param("mgnt_no_f", SqlDbType.NVarChar, ipstrMgnt_no_f))
                    .Add(pfSet_Param("mgnt_no_t", SqlDbType.NVarChar, ipstrMgnt_no_t))
                    .Add(pfSet_Param("status_cd", SqlDbType.NVarChar, ipstrStatus_cd))
                    .Add(pfSet_Param("dly_dvs", SqlDbType.NVarChar, ipstrDly_dvs))
                    .Add(pfSet_Param("dly_cd", SqlDbType.NVarChar, ipstrDly_cd))
                    .Add(pfSet_Param("disp_id", SqlDbType.NVarChar, ipstrDisp_id))
                    If ipblnReSearch = True Then
                        .Add(pfSet_Param("initial_flg", SqlDbType.NVarChar, "1"))
                    Else
                        .Add(pfSet_Param("initial_flg", SqlDbType.NVarChar, "0"))
                    End If
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    Master.ppCount = dstOrders.Tables(0).Rows.Count.ToString
                Else
                    '閾値を超えた場合はメッセージを表示
                    If dstOrders.Tables(0).Rows(0)("データ件数") > dstOrders.Tables(0).Rows.Count Then
                        psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                            dstOrders.Tables(0).Rows(0)("データ件数").ToString, dstOrders.Tables(0).Rows.Count.ToString)
                    End If
                    Master.ppCount = dstOrders.Tables(0).Rows(0)("データ件数").ToString
                End If

                '取得したデータをリストに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "配送機器一覧")
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
                'データセット破棄
                Call ClsCMDataConnect.psDisposeDataSet(dstOrders)
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
    ''' クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()

        msClearSearch()
        msClearSelect()                             '選択クリア

        Me.grvList.DataSource = New DataTable()
        Me.grvList.DataBind()

        Master.ppCount = "0"

    End Sub

    '-----------------------------
    '2014/05/23 土岐　ここから
    '-----------------------------
    ''' <summary>
    ''' 検索エリアのクリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearSearch()
        Me.dftMntDT.ppFromText = String.Empty       '発送日From
        Me.dftMntDT.ppToText = String.Empty         '発送日To
        Me.dftArvDT.ppFromText = String.Empty       '致着日From
        Me.dftArvDT.ppToText = String.Empty         '致着日To
        Me.tftMntNo.ppFromText = String.Empty       '管理番号From
        Me.tftMntNo.ppToText = String.Empty         '管理番号To
        Me.ddlSituation.SelectedIndex = -1          '進捗状況
        Me.ddlSPoint.ppDropDownList.SelectedIndex = -1  '配送先区分
        Me.txtSPoint.ppText = String.Empty          '配送先コード
        Me.lblPointNm.Text = String.Empty           '配送先拠点名
    End Sub
    '-----------------------------
    '2014/05/23 土岐　ここまで
    '-----------------------------

    ''' <summary>
    ''' 選択クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearSelect()
        '-----------------------------
        '2014/04/21 土岐　ここから
        '-----------------------------
        '★排他情報削除
        If Not Me.Master.Master.ppExclusiveDateDtl = String.Empty Then

            If clsExc.pfDel_Exclusive(Me _
                               , Session(P_SESSION_SESSTION_ID) _
                               , Me.Master.Master.ppExclusiveDateDtl) = 0 Then
                Me.Master.Master.ppExclusiveDateDtl = String.Empty
            Else
                Exit Sub
            End If
        End If
        '-----------------------------
        '2014/04/21 土岐　ここまで
        '-----------------------------
        Me.ddlAppaClass.SelectedIndex = -1          '機器分類
        Me.ddlAppaClassCD.Items.Clear()             '機器種別
        Me.ddlTboxType.SelectedIndex = -1           'ＴＢＯＸタイプ
        Me.TxtTboxVer.ppText = String.Empty         'ＶＥＲ
        Me.ddlAppaNM.SelectedIndex = -1             '型式／機器
        Me.ddlAppaNM.Items.Clear()
        Me.ddlAppaNM.Items.Insert(0, New ListItem(Nothing, Nothing))
        Me.ddlHDDNo.SelectedIndex = -1              'ＨＤＤＮＯ
        Me.ddlHDDCls.SelectedIndex = -1             'ＨＤＤ種別
        Me.txtSerialNo.ppText = String.Empty        'シリアルＮＯ
        Me.ddlDelivery.ppDropDownList.SelectedIndex = -1           '配送元区分
        Me.txtDelivery.ppText = String.Empty        '配送元コード
        Me.lblDelivery.Text = String.Empty          '配送元
        Me.dtbDeliveryDT.ppText = String.Empty      '発送日
        Me.ddlPoint.ppDropDownList.SelectedIndex = -1              '配送先区分
        Me.txtPoint.ppText = String.Empty           '配送先区分
        Me.lblPoint.Text = String.Empty             '配送先
        Me.dtbArrivePlanDT.ppText = String.Empty    '発着予定日
        Me.dtbArriveDT.ppText = String.Empty        '発着日
        Me.txtMntNo.ppText = String.Empty           '管理番号
        '--- 追加 2014/05/26 START ----------------------------------
        Me.ddlStatus.SelectedIndex = -1             '進捗状況
        '--- 追加 2014/05/26 END ------------------------------------
        Me.lblPbrnNo.Text = String.Empty            '発行番号
        Me.txtNotes.ppText = String.Empty           '備考

        '活性
        Me.ddlTboxType.Enabled = True               'ＴＢＯＸタイプ
        Me.cuvTboxType.Enabled = True               'ＴＢＯＸタイプ_エラー
        Me.TxtTboxVer.ppEnabled = True                'ＶＥＲ
        Me.txtSerialNo.ppEnabled = True             'シリアルＮＯ．
        Me.txtMntNo.ppEnabled = True                '管理番号
        Me.btnAdd.Enabled = True                    '登録ボタン

        '非活性
        Me.btnDel.Enabled = False                   '削除ボタン
        Me.btnUpdate.Enabled = False                '更新ボタン

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（進捗ステータスマスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlStatus()

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
                With cmdDB.Parameters
                    'パラメータ設定
                    '-----------------------------
                    '2014/05/15 土岐　ここから
                    '-----------------------------
                    .Add(pfSet_Param("duty_cd", SqlDbType.NVarChar, "71"))
                    '-----------------------------
                    '2014/05/15 土岐　ここまで
                    '-----------------------------
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'ドロップダウンリスト設定
                Me.ddlSituation.Items.Clear()
                Me.ddlSituation.DataSource = dstOrders.Tables(0)
                Me.ddlSituation.DataTextField = "進捗ステータス名"
                Me.ddlSituation.DataValueField = "進捗ステータス"
                Me.ddlSituation.DataBind()

                Me.ddlSituation.Items.Insert(0, New ListItem(Nothing, Nothing))

                '--- 追加 2014/05/26 START ----------------------------------
                'ドロップダウンリスト設定（更新部）
                Me.ddlStatus.Items.Clear()
                Me.ddlStatus.DataSource = dstOrders.Tables(0)
                Me.ddlStatus.DataTextField = "進捗ステータス名"
                Me.ddlStatus.DataValueField = "進捗ステータス"
                Me.ddlStatus.DataBind()
                Me.ddlStatus.Items.Insert(0, New ListItem(Nothing, Nothing))
                '--- 追加 2014/05/26 END ------------------------------------
            Catch ex As Exception
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
                'データセット破棄
                Call ClsCMDataConnect.psDisposeDataSet(dstOrders)
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（機器分類マスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlAppaclass()

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
                cmdDB = New SqlCommand("ZCMPSEL012", conDB)

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'ドロップダウンリスト設定
                Me.ddlAppaClass.Items.Clear()
                Me.ddlAppaClass.DataSource = dstOrders.Tables(0)
                Me.ddlAppaClass.DataTextField = "名称"
                Me.ddlAppaClass.DataValueField = "機器分類コード"
                Me.ddlAppaClass.DataBind()

                Me.ddlAppaClass.Items.Insert(0, New ListItem(Nothing, Nothing))
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器分類マスタ一覧取得")
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
                'データセット破棄
                Call ClsCMDataConnect.psDisposeDataSet(dstOrders)
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（機器種別マスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlAppaGrouping(ByVal ipstrSelAppaclassCD)

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

        '機器種別がない場合リストを空にする
        If ipstrSelAppaclassCD = String.Empty Then
            ddlAppaClassCD.Items.Clear()
            Exit Sub
        End If

        'DB接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                cmdDB = New SqlCommand("ZCMPSEL013", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("appadivcd", SqlDbType.NVarChar, ipstrSelAppaclassCD))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'ドロップダウンリスト設定
                Me.ddlAppaClassCD.Items.Clear()
                Me.ddlAppaClassCD.DataSource = dstOrders.Tables(0)
                Me.ddlAppaClassCD.DataTextField = "機器種別名"
                Me.ddlAppaClassCD.DataValueField = "機器種別コード"
                Me.ddlAppaClassCD.DataBind()

                Me.ddlAppaClassCD.Items.Insert(0, New ListItem(Nothing, Nothing))
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器種別マスタ一覧取得")
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
                'データセット破棄
                Call ClsCMDataConnect.psDisposeDataSet(dstOrders)
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（ＴＢＯＸタイプ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlTboxType()

        Dim conDB As SqlConnection = Nothing    'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing       'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
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
                cmdDB = New SqlCommand("ZCMPSEL038", conDB)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(cmdDB)

                'ドロップダウンリスト設定
                Me.ddlTboxType.Items.Clear()
                Me.ddlTboxType.DataSource = objDs.Tables(0)
                Me.ddlTboxType.DataTextField = "ＴＢＯＸリスト"
                Me.ddlTboxType.DataValueField = "ＴＢＯＸシステムコード"
                Me.ddlTboxType.DataBind()

                '先頭に空白行を追加
                Me.ddlTboxType.Items.Insert(0, New ListItem(Nothing, Nothing))

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸマスタ一覧取得")
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
                'データセット破棄
                Call ClsCMDataConnect.psDisposeDataSet(objDs)
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（ＨＤＤ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetHDD()
        '-----------------------------
        '2014/05/12 土岐　ここから
        '-----------------------------
        Dim dtrDel() As DataRow
        '-----------------------------
        '2014/05/12 土岐　ここまで
        '-----------------------------

        'ＨＤＤリスト取得
        msGetHDD()

        If ViewState(M_VIEW_HDD_T) Is Nothing Then
            Exit Sub
        End If

        'ＨＤＤＮＯ（重複データをカット）
        Dim davHDDNo As DataView = CType(ViewState(M_VIEW_HDD_T), DataTable).DefaultView
        Dim dttHDDNo As DataTable = davHDDNo.ToTable("ＨＤＤＮｏ", True, "ＨＤＤＮｏ")
        'ドロップダウンリスト設定（ＨＤＤＮＯ）
        Me.ddlHDDNo.Items.Clear()
        '-----------------------------
        '2014/05/12 土岐　ここから
        '-----------------------------
        dtrDel = dttHDDNo.Select("[ＨＤＤＮｏ] IS NULL OR [ＨＤＤＮｏ] = ''")
        For Each dtrdata As DataRow In dtrDel
            dtrdata.Delete()
        Next
        Me.ddlHDDNo.DataSource = dttHDDNo
        '-----------------------------
        '2014/05/12 土岐　ここまで
        '-----------------------------
        Me.ddlHDDNo.DataValueField = "ＨＤＤＮｏ"
        Me.ddlHDDNo.DataBind()
        '先頭に空白行を追加
        Me.ddlHDDNo.Items.Insert(0, New ListItem(Nothing, Nothing))

        'ＨＤＤ種別（重複データをカット）
        Dim davHDDCls As DataView = CType(ViewState(M_VIEW_HDD_T), DataTable).DefaultView
        Dim dttHDDCls As DataTable = davHDDCls.ToTable("ＨＤＤ種別", True, "ＨＤＤ種別")
        'ドロップダウンリスト設定（ＨＤＤ種別）
        Me.ddlHDDCls.Items.Clear()
        '-----------------------------
        '2014/05/12 土岐　ここから
        '-----------------------------
        dtrDel = dttHDDCls.Select("[ＨＤＤ種別] IS NULL OR [ＨＤＤ種別] = ''")
        For Each dtrdata As DataRow In dtrDel
            dtrdata.Delete()
        Next
        Me.ddlHDDCls.DataSource = dttHDDCls
        '-----------------------------
        '2014/05/12 土岐　ここまで
        '-----------------------------
        Me.ddlHDDCls.DataValueField = "ＨＤＤ種別"
        Me.ddlHDDCls.DataBind()
        '先頭に空白行を追加
        Me.ddlHDDCls.Items.Insert(0, New ListItem(Nothing, Nothing))

    End Sub

    ''' <summary>
    ''' 型式設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetAppaData()

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("SERUPDP001_S4", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '機器分類コード
                    .Add(pfSet_Param("appaclass_cd", SqlDbType.NVarChar, Me.ddlAppaClass.SelectedValue))
                    '機器種別コード
                    .Add(pfSet_Param("appa_cls", SqlDbType.NVarChar, Me.ddlAppaClassCD.SelectedValue))
                    'システムコード
                    .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, Me.ddlTboxType.SelectedValue))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables(0).Rows.Count = 0 Then
                    Me.ddlAppaNM.Items.Clear()    'データが無い場合は初期化
                    Me.ddlAppaNM.Enabled = False  'データが無い場合は非活性
                Else
                    'ドロップダウンリスト設定
                    Me.ddlAppaNM.Items.Clear()
                    Me.ddlAppaNM.DataSource = objDs.Tables(0)
                    Me.ddlAppaNM.DataTextField = "型式機器"
                    Me.ddlAppaNM.DataValueField = "型式機器"
                    Me.ddlAppaNM.DataBind()
                    Me.ddlAppaNM.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加
                    Me.ddlAppaNM.Enabled = True
                End If

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器マスタ一覧取得")
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
                'データセット破棄
                Call ClsCMDataConnect.psDisposeDataSet(objDs)
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ＨＤＤデータ取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGetHDD()

        Dim conDB As SqlConnection = Nothing    'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing       'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        If ViewState(M_VIEW_HDD_T) IsNot Nothing Then
            Exit Sub
        End If

        'DB接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                cmdDB = New SqlCommand("ZCMPSEL033", conDB)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(cmdDB)

                'ViewStateにＨＤＤ情報を格納
                ViewState(M_VIEW_HDD_T) = objDs.Tables(0)

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＨＤＤマスタ一覧取得")
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
                'データセット破棄
                Call ClsCMDataConnect.psDisposeDataSet(objDs)
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If
    End Sub

    ''' <summary>
    ''' 営業所情報取得処理
    ''' </summary>
    ''' <param name="strOfficeCd"></param>
    ''' <param name="strOfficeName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetOfficeInfo(ByVal strOfficeCd As String, ByRef strOfficeName As String, ByVal strTraderCd As String) As Boolean

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim dtRow As DataRow = Nothing          'DataRow
        objStack = New StackFrame

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
                    .Add(pfSet_Param("trader_cd", SqlDbType.NVarChar, strTraderCd))
                    '営業所コード
                    .Add(pfSet_Param("office_cd", SqlDbType.NVarChar, strOfficeCd))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables(0).Rows.Count = 0 Then
                    Exit Function
                End If

                dtRow = objDs.Tables(0).Rows(0)

                '会社名または営業所名設定
                If dtRow("業者コード").ToString = "2" OrElse
                   dtRow("業者コード").ToString = "3" OrElse
                   dtRow("業者コード").ToString = "4" Then
                    strOfficeName = dtRow("営業所名").ToString
                Else
                    strOfficeName = dtRow("会社名").ToString
                End If

                mfGetOfficeInfo = True

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "営業所情報取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'データセット破棄
                Call ClsCMDataConnect.psDisposeDataSet(objDs)
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Function

    ''' <summary>
    ''' 明細行更新処理
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ</param>
    ''' <param name="iptrnDB">トランザクション</param>
    ''' <param name="ipstrProcCls">処理区分（1:新規登録、2:更新）</param>
    ''' <returns>ＳＱＬ実行結果</returns>
    ''' <remarks></remarks>
    Private Function mfUpdateDTL(ByVal ipconDB As SqlConnection,
                                 ByVal iptrnDB As SqlTransaction,
                                 ByVal ipstrProcCls As String) As String
        Dim cmdDB As SqlCommand
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            cmdDB = New SqlCommand("EQULSTP001_U1", ipconDB)
            cmdDB.Transaction = iptrnDB
            With cmdDB.Parameters
                .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, ipstrProcCls))               '処理区分
                If Me.lblPbrnNo.Text = String.Empty Then                                      '発行番号
                    .Add(pfSet_Param("pbrn_no", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("pbrn_no", SqlDbType.NVarChar, Me.lblPbrnNo.Text))
                End If
                If Me.txtSerialNo.ppText = String.Empty Then                                  'シリアルNo.
                    .Add(pfSet_Param("seri_no", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("seri_no", SqlDbType.NVarChar, Me.txtSerialNo.ppText))
                End If
                If Me.ddlTboxType.SelectedIndex < 1 Then                                      'システムコード、システム名称
                    .Add(pfSet_Param("sys_cod", SqlDbType.NVarChar, DBNull.Value))
                    .Add(pfSet_Param("sys_nam", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("sys_cod", SqlDbType.NVarChar, Me.ddlTboxType.SelectedValue))
                    .Add(pfSet_Param("sys_nam", SqlDbType.NVarChar, Me.ddlTboxType.SelectedItem.ToString.Split(":")(1)))
                End If
                If Me.TxtTboxVer.ppText = String.Empty Then                                  'TBOXVER
                    .Add(pfSet_Param("tboxver", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("tboxver", SqlDbType.NVarChar, Me.TxtTboxVer.ppText))
                End If
                If Me.ddlAppaClass.SelectedIndex < 1 Then                                    '機器分類コード、機器分類名称
                    .Add(pfSet_Param("apa_cod", SqlDbType.NVarChar, DBNull.Value))
                    .Add(pfSet_Param("apa_nam", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("apa_cod", SqlDbType.NVarChar, Me.ddlAppaClass.SelectedValue))
                    .Add(pfSet_Param("apa_nam", SqlDbType.NVarChar, Me.ddlAppaClass.SelectedItem.ToString.Split(":")(1)))
                End If
                If Me.ddlAppaClassCD.SelectedIndex < 1 Then                                  '機器種別コード
                    .Add(pfSet_Param("apc_cod", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("apc_cod", SqlDbType.NVarChar, Me.ddlAppaClassCD.SelectedValue))
                End If
                If Me.ddlAppaNM.SelectedIndex < 1 Then                                       '型式/機器
                    .Add(pfSet_Param("apc_nam", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("apc_nam", SqlDbType.NVarChar, Me.ddlAppaNM.SelectedValue))
                End If
                If Me.ddlHDDNo.SelectedIndex < 1 Then                                        'HDDNo.
                    .Add(pfSet_Param("hdd_no", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("hdd_no", SqlDbType.NVarChar, Me.ddlHDDNo.SelectedValue))
                End If
                If Me.ddlHDDCls.SelectedIndex < 1 Then                                       'HDD種別
                    .Add(pfSet_Param("hdd_dvs", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("hdd_dvs", SqlDbType.NVarChar, Me.ddlHDDCls.SelectedValue))
                End If
                If Me.ddlDelivery.ppDropDownList.SelectedIndex < 1 Then                      '配送元区分  
                    .Add(pfSet_Param("ord_dvs", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("ord_dvs", SqlDbType.NVarChar, Me.ddlDelivery.ppDropDownList.SelectedValue))
                End If
                If Me.txtDelivery.ppText = String.Empty AndAlso lblDelivery.Text = String.Empty Then  '配送元コード、配送元名称
                    .Add(pfSet_Param("ord_cod", SqlDbType.NVarChar, DBNull.Value))
                    .Add(pfSet_Param("ord_nam", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("ord_cod", SqlDbType.NVarChar, Me.txtDelivery.ppText))
                    .Add(pfSet_Param("ord_nam", SqlDbType.NVarChar, Me.lblDelivery.Text))
                End If
                If Me.dtbDeliveryDT.ppText = String.Empty Then                               '発送日
                    .Add(pfSet_Param("shi_ymd", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("shi_ymd", SqlDbType.NVarChar, Me.dtbDeliveryDT.ppText.Replace("/", "")))
                End If
                If Me.ddlPoint.ppDropDownList.SelectedIndex < 1 Then                         '配送先区分
                    .Add(pfSet_Param("dly_dvs", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("dly_dvs", SqlDbType.NVarChar, Me.ddlPoint.ppDropDownList.SelectedValue))
                End If
                If Me.txtPoint.ppText = String.Empty AndAlso lblPoint.Text = String.Empty Then        '配送先コード、配送先名称
                    .Add(pfSet_Param("dly_cod", SqlDbType.NVarChar, DBNull.Value))
                    .Add(pfSet_Param("dly_nam", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("dly_cod", SqlDbType.NVarChar, Me.txtPoint.ppText))
                    .Add(pfSet_Param("dly_nam", SqlDbType.NVarChar, Me.lblPoint.Text))
                End If
                If Me.dtbArrivePlanDT.ppText = String.Empty Then                             '致着予定日
                    .Add(pfSet_Param("pln_ymd", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("pln_ymd", SqlDbType.NVarChar, Me.dtbArrivePlanDT.ppText.Replace("/", "")))
                End If
                If Me.dtbArriveDT.ppText = String.Empty Then                                 '致着日
                    .Add(pfSet_Param("dep_ymd", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("dep_ymd", SqlDbType.NVarChar, Me.dtbArriveDT.ppText.Replace("/", "")))
                End If
                If Me.txtMntNo.ppText = String.Empty Then                                    '管理番号
                    .Add(pfSet_Param("mgnt_no", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("mgnt_no", SqlDbType.NVarChar, Me.txtMntNo.ppText))
                End If
                If Me.ddlStatus.SelectedIndex < 1 Then                                       '進捗状況
                    .Add(pfSet_Param("sts_cod", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("sts_cod", SqlDbType.NVarChar, Me.ddlStatus.SelectedValue))
                End If
                If Me.txtNotes.ppText = String.Empty Then                                    '備考
                    .Add(pfSet_Param("notetext", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("notetext", SqlDbType.NVarChar, Me.txtNotes.ppText))
                End If
                .Add(pfSet_Param("usr_id", SqlDbType.NVarChar, User.Identity.Name))          '登録者/更新者
                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                            '戻り値（処理結果）
                .Add(New SqlParameter("return", SqlDbType.NVarChar))                         '戻り値（発行番号）
            End With
            cmdDB.Parameters("return").Direction = ParameterDirection.Output
            cmdDB.Parameters("return").Size = -1

            'コマンドタイプ設定(ストアド)
            cmdDB.CommandType = CommandType.StoredProcedure
            cmdDB.ExecuteNonQuery()

            'ストアド戻り値チェック
            If cmdDB.Parameters("return").Value.ToString = String.Empty Then
                mfUpdateDTL = cmdDB.Parameters("retvalue").Value.ToString
            Else
                mfUpdateDTL = "正常" + cmdDB.Parameters("return").Value.ToString
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
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 明細行削除処理
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ</param>
    ''' <param name="iptrnDB">トランザクション</param>
    ''' <param name="ipstrPbrlNo">発行番号</param>
    ''' <returns>ＳＱＬ実行結果</returns>
    ''' <remarks></remarks>
    Private Function mfDeleteDTL(ByVal ipconDB As SqlConnection,
                                 ByVal iptrnDB As SqlTransaction,
                                 ByVal ipstrPbrlNo As String) As Integer
        Dim cmdDB As SqlCommand
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            cmdDB = New SqlCommand("EQULSTP001_D1", ipconDB)
            cmdDB.Transaction = iptrnDB
            With cmdDB.Parameters
                .Add(pfSet_Param("pbrn_no", SqlDbType.NVarChar, ipstrPbrlNo))               '発行番号
                .Add(pfSet_Param("update_user", SqlDbType.NVarChar, User.Identity.Name))    '更新者
                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
            End With

            'コマンドタイプ設定(ストアド)
            cmdDB.CommandType = CommandType.StoredProcedure
            cmdDB.ExecuteNonQuery()

            'ストアド戻り値チェック
            mfDeleteDTL = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
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
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 入力項目のエラーチェックを行う。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheck_Error(ByVal ipstrMode As String)

        Dim strbuff As String = String.Empty

        Select Case ipstrMode
            Case "Search"
                '管理番号
                If Me.tftMntNo.ppFromText <> String.Empty AndAlso Me.tftMntNo.ppToText <> String.Empty Then
                    If Me.tftMntNo.ppFromText.Length <> Me.tftMntNo.ppToText.Length Then
                        Me.tftMntNo.psSet_ErrorNo("3005", "管理番号")
                    End If
                End If

                '配送先
                If Me.ddlSPoint.ppDropDownList.SelectedIndex > 0 OrElse Me.txtSPoint.ppText <> String.Empty Then
                    If Me.ddlSPoint.ppDropDownList.SelectedIndex > 0 AndAlso Me.txtSPoint.ppText <> String.Empty Then
                        Select Case Me.ddlSPoint.ppSelectedValue
                            Case 1  '営業所名、または保守拠点名取得
                                If mfGetOfficeInfo(Me.txtSPoint.ppText, strbuff, "2,3") Then
                                    Me.lblPointNm.Text = strbuff
                                Else
                                    Me.lblPointNm.Text = String.Empty
                                    Me.txtSPoint.psSet_ErrorNo("2002", "入力した配送先コード")
                                    Me.txtSPoint.ppTextBox.Focus()
                                End If
                            Case 3  '倉庫名取得
                                If mfGetOfficeInfo(Me.txtSPoint.ppText, strbuff, "6") Then
                                    Me.lblPointNm.Text = strbuff
                                Else
                                    Me.lblPointNm.Text = String.Empty
                                    Me.txtSPoint.psSet_ErrorNo("2002", "入力した配送元コード")
                                    Me.txtSPoint.ppTextBox.Focus()
                                End If
                            Case Else
                        End Select
                    End If
                End If

            Case "Edit"
                '配送元
                If Me.ddlDelivery.ppDropDownList.SelectedIndex > 0 OrElse Me.txtDelivery.ppText <> String.Empty Then
                    If Me.ddlDelivery.ppDropDownList.SelectedIndex > 0 AndAlso Me.txtDelivery.ppText <> String.Empty Then
                        Select Case Me.ddlDelivery.ppSelectedValue
                            Case 1  '営業所名、または保守拠点名取得
                                If mfGetOfficeInfo(Me.txtDelivery.ppText, strbuff, "2,3") Then
                                    Me.lblDelivery.Text = strbuff
                                Else
                                    Me.lblDelivery.Text = String.Empty
                                    Me.txtDelivery.psSet_ErrorNo("2002", "入力した配送元コード")
                                    Me.txtDelivery.ppTextBox.Focus()
                                End If
                            Case 3  '倉庫名取得
                                If mfGetOfficeInfo(Me.txtDelivery.ppText, strbuff, "6") Then
                                    Me.lblDelivery.Text = strbuff
                                Else
                                    Me.lblDelivery.Text = String.Empty
                                    Me.txtDelivery.psSet_ErrorNo("2002", "入力した配送元コード")
                                    Me.txtDelivery.ppTextBox.Focus()
                                End If
                            Case Else
                        End Select
                    Else
                        If Me.ddlDelivery.ppDropDownList.SelectedIndex > 0 And Me.txtDelivery.ppText = String.Empty Then
                            Me.lblDelivery.Text = String.Empty
                            Me.txtDelivery.psSet_ErrorNo("5001", "配送元コード")
                            Me.txtDelivery.ppTextBox.Focus()
                        End If
                        If Me.ddlDelivery.ppDropDownList.SelectedIndex < 1 And Me.txtDelivery.ppText <> String.Empty Then
                            Me.lblDelivery.Text = String.Empty
                            Me.ddlDelivery.psSet_ErrorNo("5003", "配送元区分")
                            Me.ddlDelivery.ppDropDownList.Focus()
                        End If
                    End If
                End If

                '配送先
                If Me.ddlPoint.ppDropDownList.SelectedIndex > 0 OrElse Me.txtPoint.ppText <> String.Empty Then
                    If Me.ddlPoint.ppDropDownList.SelectedIndex > 0 AndAlso Me.txtPoint.ppText <> String.Empty Then
                        Select Case Me.ddlPoint.ppSelectedValue
                            Case 1  '営業所名、または保守拠点名取得
                                If mfGetOfficeInfo(Me.txtPoint.ppText, strbuff, "2,3") Then
                                    Me.lblPoint.Text = strbuff
                                Else
                                    Me.lblPoint.Text = String.Empty
                                    Me.txtPoint.psSet_ErrorNo("2002", "入力した配送先コード")
                                    Me.txtPoint.ppTextBox.Focus()
                                End If
                            Case 3  '倉庫名取得
                                If mfGetOfficeInfo(Me.txtPoint.ppText, strbuff, "6") Then
                                    Me.lblPoint.Text = strbuff
                                Else
                                    Me.lblPoint.Text = String.Empty
                                    Me.txtPoint.psSet_ErrorNo("2002", "入力した配送元コード")
                                    Me.txtPoint.ppTextBox.Focus()
                                End If
                            Case Else
                        End Select
                    Else
                        If Me.ddlPoint.ppDropDownList.SelectedIndex > 0 And Me.txtPoint.ppText = String.Empty Then
                            Me.lblPoint.Text = String.Empty
                            Me.txtPoint.psSet_ErrorNo("5001", "配送先コード")
                            Me.txtPoint.ppTextBox.Focus()
                        End If
                        If Me.ddlPoint.ppDropDownList.SelectedIndex < 1 And Me.txtPoint.ppText <> String.Empty Then
                            Me.lblPoint.Text = String.Empty
                            Me.ddlPoint.psSet_ErrorNo("5003", "配送先区分")
                            Me.ddlPoint.ppDropDownList.Focus()
                        End If
                    End If
                End If
        End Select


    End Sub

    ''' <summary>
    ''' ＴＢＯＸタイプエラーチェック
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvTboxType_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvTboxType.ServerValidate
        Dim strErrNo As String
        Dim dtrMes As DataRow
        'エラーチェック
        strErrNo = pfCheck_ListErr(args.Value, True)

        If strErrNo <> String.Empty Then
            'エラー
            dtrMes = pfGet_ValMes(strErrNo, "ＴＢＯＸ種別")
            cuvTboxType.Text = dtrMes.Item(P_VALMES_SMES)
            cuvTboxType.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            args.IsValid = False
            Exit Sub
        End If
    End Sub

    ''' <summary>
    ''' 進捗状況エラーチェック
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cuvStatus_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvStatus.ServerValidate
        Dim strErrNo As String
        Dim dtrMes As DataRow
        'エラーチェック
        strErrNo = pfCheck_ListErr(args.Value, True)

        If strErrNo <> String.Empty Then
            'エラー
            dtrMes = pfGet_ValMes(strErrNo, "進捗状況")
            cuvStatus.Text = dtrMes.Item(P_VALMES_SMES)
            cuvStatus.ErrorMessage = dtrMes.Item(P_VALMES_MES)
            args.IsValid = False
            Exit Sub
        End If
    End Sub

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
