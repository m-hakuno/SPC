'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜監視＞
'*　処理名　　：　ＴＢＯＸ随時照会
'*　ＰＧＭＩＤ：　WATINQP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.1.7　：　中川
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'WATINQP001-001     2015/07/**      武         カスタマイズに伴う修正   
'WATINQP001-002     2015/10/05      加賀       e-basカスタマイズ(既存の事象修正)　CIDのIME制御追加、txtTBOXIDの幅変更 
'WATINQP001-003     2017/07/14      加賀       バグ修正
'WATINQP001-004     2017/08/14      加賀       結果表示遷移先変更(データ絞込み画面)

#Region "インポート定義"

Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive

#End Region

Public Class WATINQP001

#Region "継承定義"
    Inherits System.Web.UI.Page
#End Region

#Region "定数定義"

    'プログラムID
    Const M_DISP_ID = P_FUN_WAT & P_SCR_INQ & P_PAGE & "001"        '自画面
    Const M_COM_DISP_ID = P_FUN_WAT & P_SCR_OUT & P_PAGE & "001"    'TBOX結果表示 情報選択画面
    Const M_KSI_DISP_ID = P_FUN_WAT & P_SCR_OUT & P_PAGE & "002"    'TBOX結果表示 決済照会情報選択画面
    Const M_TNNI_DISP_ID = P_FUN_WAT & P_SCR_OUT & P_PAGE & "026"   'TBOX結果表示 店内通信選択画面
    Const M_HALL_DISP_ID = P_FUN_WAT & P_SCR_OUT & P_PAGE & "027"

#End Region

#Region "構造体・列挙体定義"

#End Region

#Region "変数定義"

    Dim objStack As StackFrame
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom

#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page_Initイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        objStack = New StackFrame

        Try
            psLogStart(Me)

            'グリッドXML定義ファイル取得
            pfSet_GridView_S_Off(Me.grvInquiryList, M_DISP_ID & "_1")
            pfSet_GridView(Me.grvResultList, M_DISP_ID & "_2")

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

        Finally

            psLogEnd(Me)

        End Try

    End Sub

    ''' <summary>
    ''' Page_Loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        AddHandler Master.ppRigthButton7.Click, AddressOf btnGmnUpdate_Click
        AddHandler Me.txtTboxId1.ppTextBox.TextChanged, AddressOf txtTboxId1_LostFucos
        AddHandler ddlDataSbt.ppDropDownList.SelectedIndexChanged, AddressOf ddlDataSbt_TextChanged

        objStack = New StackFrame

        Try
            psLogStart(Me)

            Dim dstOrders_1 As New WATINQP001_DATASET

            If Not IsPostBack Then  '初回表示

                '自動ポストバックを有効にする
                Me.txtTboxId1.ppTextBox.AutoPostBack = True
                Me.ddlDataSbt.ppDropDownList.AutoPostBack = True

                '画面情報設定
                Master.ppProgramID = M_DISP_ID
                Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)
                '                Master.ppLogout_Mode = Global_asax. ClsComVer.E_ログアウトモード.閉じる
                Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる

                'ボタン設定
                Master.ppRigthButton7.Visible = True
                Master.ppRigthButton7.Text = "画面更新"
                Master.ppRigthButton7.ValidationGroup = "2"
                Me.btnAdd.OnClientClick =
                    pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "照会情報")
                Me.btnUpdate.OnClientClick =
                    pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "照会情報")
                Me.btnDelete.OnClientClick =
                    pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "照会情報")
                Me.btnInquiry.OnClientClick =
                    pfGet_OCClickMes("20004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel)

                'パンくずリスト設定
                Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

                'セッション情報を取得する
                ViewState(P_SESSION_USERID) = Session(P_SESSION_USERID)             'ユーザID
                ViewState(P_SESSION_IP) = Session(P_SESSION_IP)                     'IP
                ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)               '遷移条件
                ViewState(P_SESSION_OLDDISP) = Session(P_SESSION_OLDDISP)           '遷移元ID
                If Not Session(P_SESSION_GROUP_NUM) Is Nothing Then                 'グループ番号
                    ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)
                End If

                If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then             '登録用年月日
                    Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)
                End If

                '遷移条件が参照のとき、TBOXIDをセット
                If Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照 Then
                    '工事依頼書兼仕様書、ヘルスチェック
                    If Not Session(P_KEY) Is Nothing Then
                        ViewState(P_KEY) = Session(P_KEY)
                    End If
                    'viewstateの内容確認
                    Dim tboxidh As String()
                    tboxidh = ViewState(P_KEY)
                End If

                '画面初期化
                msClearScreen(ViewState(P_SESSION_TERMS), ViewState(P_SESSION_OLDDISP), ViewState(P_KEY))
                Me.grvInquiryList.DataSource = dstOrders_1.Tables("KeepInquiryDtt")
                Me.grvInquiryList.DataBind()

                '結果データ取得処理
                Call mfGet_ResultData("1")

                'ＴＢＯＸＩＤ入力項目にフォーカスを設定する
                Me.txtTboxId1.ppTextBox.Focus()

            End If

            Me.grvResultList.DataSource = ViewState("結果明細データソース")
            Me.grvResultList.DataBind()

            Call msSet_Requred()

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally

            psLogEnd(Me)

        End Try

    End Sub

    ' ''' <summary>
    ' ''' ユーザー権限
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

    '    'Select Case Session(P_SESSION_AUTH)
    '    '    Case "管理者"
    '    '    Case "SPC"
    '    '    Case "営業所"
    '    '    Case "NGC"
    '    'End Select

    'End Sub

    ''' <summary>
    ''' 結果明細データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvResultList.DataBound
        Dim strResultCd, strRcvState As String
        For Each rowData As GridViewRow In grvResultList.Rows
            'strDelF = DirectCast(rowData.FindControl("状態"), TextBox).Text
            'rowData.Cells(0).Enabled = False
            'If strDelF = "受信完了" Then
            '    rowData.Cells(0).Enabled = True
            'End If
            strRcvState = DirectCast(rowData.FindControl("状態"), TextBox).Text
            strResultCd = DirectCast(rowData.FindControl("結果"), TextBox).Text + "XX"
            strResultCd = strResultCd.Substring(0, 2)
            rowData.Cells(0).Enabled = False
            If strRcvState = "受信完了" AndAlso strResultCd = "00" Then
                rowData.Cells(0).Enabled = True
            End If
        Next
    End Sub

    ''' <summary>
    ''' TBOXIDロストフォーカス時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtTboxId1_LostFucos(ByVal sender As Object, ByVal e As System.EventArgs)

        objStack = New StackFrame

        Try
            psLogStart(Me)

            Dim conDB As SqlConnection = Nothing
            Dim cmdDB As SqlCommand = Nothing
            Dim dstOrders_1 As DataSet
            Dim dstOrders_2 As DataSet
            Dim strTboxVerCd As String = Nothing

            '照会明細をクリア
            Call msClearInqTable()
            Me.grvInquiryList.DataSource = New DataTable
            Me.DataBind()

            '選択行番号クリア
            ViewState.Remove("選択行番号")

            'ボタン制御
            ms_changeBtn("入力消去")

            If Me.txtTboxId1.ppText = String.Empty Then

                Me.lblHallName.Text = String.Empty
                Me.lblNlKubun.Text = String.Empty

                Exit Sub

            End If

            'TBOXID数値チェック
            If pfCheck_Num(txtTboxId1.ppText) = False Then
                txtTboxId1.psSet_ErrorNo("4002", "ＴＢＯＸＩＤ")
            End If

            Try
                'DB接続
                If clsDataConnect.pfOpen_Database(conDB) Then
                    cmdDB = New SqlCommand(M_DISP_ID & "_S1", conDB)
                    With cmdDB.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("tbox_id", SqlDbType.NVarChar, Me.txtTboxId1.ppText))
                    End With

                    dstOrders_1 = clsDataConnect.pfGet_DataSet(cmdDB)

                    If dstOrders_1.Tables(0).Rows.Count = 0 Then
                        '0件
                        Me.txtTboxId1.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
                        Exit Sub
                    End If

                    If dstOrders_1.Tables(0).Rows(0).Item("システム分類") = "1" Then
                        'システム分類が「ID」の場合
                        Me.txtTboxId1.psSet_ErrorNo("7001")

                        Me.btnInquiry.Enabled = False
                        Me.ddlDataSbt.ppEnabled = False
                        Me.ddlDataSbt.ppSelectedValue = Nothing
                        Exit Sub
                    End If

                    'ホール名、NL区分を表示する
                    Me.lblHallName.Text = dstOrders_1.Tables(0).Rows(0).Item("ホール名")
                    Me.lblNlKubun.Text = dstOrders_1.Tables(0).Rows(0).Item("NL区分")

                    'ビューステートに保存
                    ViewState("ＮＬ区分") = dstOrders_1.Tables(0).Rows(0).Item("NL区分")
                    ViewState("システム分類") = dstOrders_1.Tables(0).Rows(0).Item("システム分類")

                    strTboxVerCd = dstOrders_1.Tables(0).Rows(0).Item("ＴＢＯＸ種別")

                    'データ種別を表示する
                    cmdDB = New SqlCommand(M_DISP_ID & "_S3", conDB)
                    With cmdDB.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("tbox_ver", SqlDbType.NVarChar, strTboxVerCd))
                    End With

                    dstOrders_2 = clsDataConnect.pfGet_DataSet(cmdDB)

                    Me.ddlDataSbt.ppEnabled = True
                    Me.ddlDataSbt.ppdropdownlist.DataSource = dstOrders_2.Tables(0)
                    Me.ddlDataSbt.ppdropdownlist.DataTextField = "データ種別"
                    Me.ddlDataSbt.ppdropdownlist.DataValueField = "データ種別コード"
                    Me.ddlDataSbt.DataBind()
                    Me.ddlDataSbt.ppdropdownlist.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加
                    Me.ddlDataSbt.ppdropdownlist.Items.Add(New ListItem("店内通信", "ZZ"))          '末尾に店内通信を追加

                    'ボタン制御
                    ms_changeBtn("入力")

                Else
                    'DB接続に失敗
                    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

                End If

            Catch ex As Exception

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")

            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try

        Finally

            psLogEnd(Me)
            msFocusChange(Me.txtTboxId1)

        End Try

    End Sub

    ''' <summary>
    ''' データ種別変更後の表示／非表示処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="ByRefe"></param>
    ''' <remarks></remarks>
    Private Sub ddlDataSbt_TextChanged(ByVal sender As Object, ByRefe As EventArgs)

        objStack = New StackFrame

        Try
            psLogStart(Me)

            If Not Me.btnUpdate.Enabled Then

                If Me.btnAdd.Enabled Then

                    If Me.ddlDataSbt.ppDropDownList.SelectedIndex = 0 Then

                        'ボタン制御
                        ms_changeBtn("リスト未選択")

                    Else

                        'ボタン制御
                        ms_changeBtn("リスト選択")

                    End If

                Else

                    '追加ボタンを活性
                    Me.btnAdd.Enabled = True
                    'クリアボタンを活性
                    Me.btnClear.Enabled = True

                End If

            End If

            '照会テーブル表示／非表示処理をコールする
            Call msSet_InquiryTable()

            If Me.grvInquiryList.Rows.Count >= 15 Then
                '追加ボタンを使用不可に設定する
                Me.btnAdd.Enabled = False
            End If

            msFocusChange(Me.ddlDataSbt)

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

        Finally
            psLogEnd(Me)

        End Try

    End Sub

    ''' <summary>
    ''' クリアボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click

        objStack = New StackFrame

        Try
            psLogStart(Me)

            'ボタン制御
            ms_changeBtn("クリア")

            '照会テーブルをクリアする
            'Call msClearInqTable()

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

        Finally
            psLogEnd(Me)

        End Try

    End Sub

    ''' <summary>
    ''' 追加ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        objStack = New StackFrame

        Try
            psLogStart(Me)

            '使用中カードＤＢの場合の整合性チェック
            Dim strSlctDataSbt As String = Me.ddlDataSbt.ppdropdownlist.SelectedItem.Value
            If strSlctDataSbt = "25" Then

                ms_chkUseCdDb()

            End If

            'データ種別リスト選択／未選択判定処理
            Call msCheck_List()

            If (Page.IsValid) Then

                Dim dstOrders_1 As New WATINQP001_DATASET
                Dim dtRow As DataRow = dstOrders_1.KeepInquiryDtt.NewRow

                Select Case Me.grvInquiryList.Rows.Count
                    Case 0

                        '照会テーブルの値を保管用照会データテーブルに追加
                        dtRow("データ種別コード") = Me.ddlDataSbt.ppdropdownlist.SelectedItem.Value
                        dtRow("データ種別") = Me.ddlDataSbt.ppdropdownlist.SelectedItem.Text
                        dtRow("TBOX運用日") = Me.dtbTboxUnyoDate.ppText
                        dtRow("BB1シリアル番号") = Me.txtBB1SerialNo.ppText
                        dtRow("CID") = Me.txtCID.ppText
                        dtRow("入金伝票番号") = Me.txtNyukinDenpyoNo.ppText

                        dstOrders_1.KeepInquiryDtt.Rows.Add(dtRow)

                    Case Else
                        Dim dttTable As DataTable = pfParse_DataTable(Me.grvInquiryList)

                        '照会明細のデータを取得する
                        Call msGet_InquiryData(dstOrders_1)

                        dtRow = dstOrders_1.KeepInquiryDtt.NewRow

                        '照会テーブルの値を保管用照会データテーブルに追加
                        dtRow("データ種別コード") = Me.ddlDataSbt.ppdropdownlist.SelectedItem.Value
                        dtRow("データ種別") = Me.ddlDataSbt.ppdropdownlist.SelectedItem.Text
                        dtRow("TBOX運用日") = Me.dtbTboxUnyoDate.ppText
                        dtRow("BB1シリアル番号") = Me.txtBB1SerialNo.ppText
                        dtRow("CID") = Me.txtCID.ppText
                        dtRow("入金伝票番号") = Me.txtNyukinDenpyoNo.ppText

                        '照会情報重複チェック
                        If mfCheack_InqData(dstOrders_1.KeepInquiryDtt, dtRow) = False Then

                            Exit Sub

                        End If

                        '重複がなければ追加する
                        dstOrders_1.KeepInquiryDtt.Rows.Add(dtRow)

                End Select

                '照会テーブルをクリアする
                Call msClearInqTable()

                Me.grvInquiryList.DataSource = dstOrders_1.KeepInquiryDtt
                Me.grvInquiryList.DataBind()

                'ボタン制御
                ms_changeBtn("追加")

                'If Me.grvInquiryList.Rows.Count >= 1 Then
                '    '照会ボタンを使用可に設定する
                '    Me.btnInquiry.Enabled = True
                'Else
                '    '照会ボタンを使用不可に設定する
                '    Me.btnInquiry.Enabled = False
                'End If

                If Me.grvInquiryList.Rows.Count >= 15 Then
                    '追加ボタンを使用不可に設定する
                    Me.btnAdd.Enabled = False
                End If

            End If

            msFocusChange(Me.btnAdd)

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

        Finally
            psLogEnd(Me)

        End Try

    End Sub

    ''' <summary>
    ''' 更新ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

        objStack = New StackFrame

        Try
            psLogStart(Me)

            '使用中カードＤＢの場合の整合性チェック
            Dim strSlctDataSbt As String = Me.ddlDataSbt.ppdropdownlist.SelectedItem.Value
            If strSlctDataSbt = "25" Then

                ms_chkUseCdDb()

            End If

            'データ種別リスト選択／未選択判定処理
            Call msCheck_List()

            If (Page.IsValid) Then

                Dim dstOrders_1 As New WATINQP001_DATASET
                Dim dtRow As DataRow = dstOrders_1.KeepInquiryDtt.NewRow
                Dim intIndex As Integer = ViewState("選択行番号")

                '照会明細のデータを取得する
                Call msGet_InquiryData(dstOrders_1)

                dtRow("データ種別コード") = Me.ddlDataSbt.ppdropdownlist.SelectedItem.Value
                dtRow("データ種別") = Me.ddlDataSbt.ppdropdownlist.SelectedItem.Text
                dtRow("TBOX運用日") = Me.dtbTboxUnyoDate.ppText
                dtRow("BB1シリアル番号") = Me.txtBB1SerialNo.ppText
                dtRow("CID") = Me.txtCID.ppText
                dtRow("入金伝票番号") = Me.txtNyukinDenpyoNo.ppText

                '照会情報重複チェック
                dstOrders_1.KeepInquiryDtt.Rows.RemoveAt(intIndex)
                If mfCheack_InqData(dstOrders_1.KeepInquiryDtt, dtRow) = False Then

                    Exit Sub

                End If

                '重複がなければ選択行のデータを更新する
                If (Page.IsValid) Then

                    'dstOrders_1.KeepInquiryDtt.Rows(intIndex).ItemArray = dtRow.ItemArray
                    dstOrders_1.KeepInquiryDtt.Rows.InsertAt(dtRow, intIndex)

                End If

                Me.grvInquiryList.DataSource = dstOrders_1.KeepInquiryDtt
                Me.grvInquiryList.DataBind()

                '照会テーブル、行選択番号をクリアする
                'Call msClearInqTable()
                ViewState.Remove("選択行番号")

                'ボタン制御
                ms_changeBtn("更新")

                If Me.grvInquiryList.Rows.Count >= 15 Then
                    '追加ボタンを使用不可に設定する
                    Me.btnAdd.Enabled = False
                End If

            End If

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

        Finally
            psLogEnd(Me)

        End Try

    End Sub

    ''' <summary>
    ''' 削除ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click

        objStack = New StackFrame

        Try
            psLogStart(Me)

            Dim dstOrders_1 As New WATINQP001_DATASET
            Dim dtRow As DataRow = dstOrders_1.KeepInquiryDtt.NewRow

            '照会明細のデータを取得する
            Call msGet_InquiryData(dstOrders_1)

            '選択行を削除する
            dtRow = dstOrders_1.KeepInquiryDtt.Rows(ViewState("選択行番号"))
            dstOrders_1.KeepInquiryDtt.RemoveKeepInquiryDttRow(dtRow)

            '照会明細に保管用照会データテーブルのデータを設定する
            Me.grvInquiryList.DataSource = dstOrders_1.KeepInquiryDtt
            Me.grvInquiryList.DataBind()

            '照会テーブル、選択行番号をクリアする
            Call msClearInqTable()
            ViewState("選択行番号") = Nothing

            'ボタン制御
            ms_changeBtn("削除")

            If Me.grvInquiryList.Rows.Count >= 15 Then
                '追加ボタンを使用不可に設定する
                Me.btnAdd.Enabled = False
            End If

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

        Finally

            psLogEnd(Me)

        End Try

    End Sub

    ''' <summary>
    ''' 照会明細の選択ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvInquiryLst_RowCommand(ByVal sender As Object,
                                           e As GridViewCommandEventArgs) Handles grvInquiryList.RowCommand

        objStack = New StackFrame

        Try
            psLogStart(Me)

            Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    'ボタン押下行のIndex
            Dim rowData As GridViewRow = grvInquiryList.Rows(intIndex)      'ボタン押下行

            '選択行の情報を照会テーブルに設定
            Me.ddlDataSbt.ppSelectedValue = CType(rowData.FindControl("データ種別コード"), TextBox).Text
            msSet_InquiryTable()
            Me.dtbTboxUnyoDate.ppText = CType(rowData.FindControl("ＴＢＯＸ運用日"), TextBox).Text
            Me.txtBB1SerialNo.ppText = CType(rowData.FindControl("ＢＢ１シリアル番号"), TextBox).Text
            Me.txtCID.ppText = CType(rowData.FindControl("ＣＩＤ"), TextBox).Text
            Me.txtNyukinDenpyoNo.ppText = CType(rowData.FindControl("入金伝票番号"), TextBox).Text

            'ビューステートに選択行番号を設定する
            ViewState("選択行番号") = intIndex.ToString

            'ボタン制御
            ms_changeBtn("選択")

            If txtCID.ppText <> String.Empty Then
                rbtCID.Checked = True
                txtCID.ppEnabled = True
                rbtNo.Checked = False
                txtNyukinDenpyoNo.ppEnabled = False
                rbtCID.Enabled = True
                rbtNo.Enabled = True
            End If

            If txtNyukinDenpyoNo.ppText <> String.Empty Then
                rbtCID.Checked = False
                txtCID.ppEnabled = False
                rbtNo.Checked = True
                txtNyukinDenpyoNo.ppEnabled = True
                rbtCID.Enabled = True
                rbtNo.Enabled = True
            End If

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

        Finally

            psLogEnd(Me)

        End Try

    End Sub

    ''' <summary>
    ''' 照会ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnInquiry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInquiry.Click

        objStack = New StackFrame

        Try
            psLogStart(Me)

            If (Page.IsValid) Then

                Dim conDB As New SqlConnection
                Dim cmdDB As New SqlCommand
                Dim dstOrders_1 As New WATINQP001_DATASET
                Dim dttTable As New DataTable
                Dim intRtn As Integer

                '照会明細のデータを保管用照会データテーブルに追加
                dttTable = pfParse_DataTable(Me.grvInquiryList)

                Try
                    'DB接続
                    If Not clsDataConnect.pfOpen_Database(conDB) Then
                        '失敗
                        psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    Else

                        'トランザクション
                        Using trans = conDB.BeginTransaction
                            cmdDB = New SqlCommand(M_DISP_ID & "_I1", conDB)
                            cmdDB.CommandType = CommandType.StoredProcedure
                            With cmdDB.Parameters
                                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))       '戻り値
                                .Add(pfSet_Param("term_nm", SqlDbType.NVarChar, ViewState(P_SESSION_IP)))
                                .Add(pfSet_Param("req_cnt", SqlDbType.NVarChar, Me.grvInquiryList.Rows.Count.ToString))
                                .Add(pfSet_Param("nl_cls", SqlDbType.NVarChar, ViewState("ＮＬ区分")))
                                .Add(pfSet_Param("id_ic_cls", SqlDbType.NVarChar, ViewState("システム分類")))
                                .Add(pfSet_Param("tbox_id", SqlDbType.NVarChar, Me.txtTboxId1.ppText))
                                .Add(pfSet_Param("user_id", SqlDbType.NVarChar, ViewState(P_SESSION_USERID)))
                            End With

                            Dim intSeq As Integer = 1
                            For i = 0 To dttTable.Rows.Count - 1
                                With cmdDB.Parameters
                                    .Add(pfSet_Param("dataclass_" + intSeq.ToString, SqlDbType.NVarChar, dttTable.Rows(i).Item(0).ToString))
                                    .Add(pfSet_Param("datadate_" + intSeq.ToString, SqlDbType.NVarChar, dttTable.Rows(i).Item(2).ToString.Replace("/", "")))
                                    .Add(pfSet_Param("bbserialno_" + intSeq.ToString, SqlDbType.NVarChar, dttTable.Rows(i).Item(3).ToString))
                                    .Add(pfSet_Param("cid_" + intSeq.ToString, SqlDbType.NVarChar, dttTable.Rows(i).Item(4)))
                                    .Add(pfSet_Param("denpyono_" + intSeq.ToString, SqlDbType.NVarChar, dttTable.Rows(i).Item(5).ToString))
                                End With

                                intSeq += 1
                            Next

                            cmdDB.Transaction = trans
                            cmdDB.ExecuteNonQuery()

                            intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                            If intRtn = 1 Then
                                '管理番号取得失敗
                                trans.Rollback()
                                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "管理番号取得")
                                Exit Sub

                            ElseIf intRtn = 2 Then
                                '照会要求受付追加失敗
                                trans.Rollback()
                                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "照会要求受付")
                                Exit Sub

                            ElseIf intRtn = 3 Then
                                '照会要求データ追加失敗
                                trans.Rollback()
                                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "照会要求データ")
                                Exit Sub

                            ElseIf intRtn = 4 Then
                                '集信要求受追加失敗
                                trans.Rollback()
                                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "集信要求受")
                                Exit Sub

                            End If

                            'コミット
                            trans.Commit()
                        End Using

                        '結果データ取得
                        If mfGet_ResultData("2") = True Then

                            '照会明細をクリア
                            'Me.grvInquiryList.DataSource = dstOrders_1.KeepInquiryDtt
                            Me.grvInquiryList.DataSource = New DataTable
                            Me.grvInquiryList.DataBind()
                            'Me.btnInquiry.Enabled = False
                            Me.ddlDataSbt.ppdropdownlist.SelectedIndex = 0
                            Me.txtTboxId1.ppTextBox.Focus()

                        End If

                    End If

                Catch ex As Exception

                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

                Finally
                    'DB切断
                    If Not clsDataConnect.pfClose_Database(conDB) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If

                End Try

            End If

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

        Finally

            psLogEnd(Me)

        End Try

    End Sub

    ''' <summary>
    ''' 結果明細の結果表示ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvResultLst_RowCommand(sender As Object,
                                          e As GridViewCommandEventArgs) Handles grvResultList.RowCommand

        objStack = New StackFrame

        Try
            psLogStart(Me)

            If e.CommandName <> "btnResult" Then
                Exit Sub
            End If

            Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)     ' ボタン押下行のIndex
            Dim rowData As GridViewRow = Me.grvResultList.Rows(intIndex)     ' ボタン押下行

            Dim strKeyList As New List(Of String)
            Dim strDataCls As String = Nothing

            'パラメータ取得
            strDataCls = CType(rowData.FindControl("データ種別コード"), TextBox).Text
            strKeyList.Add(CType(rowData.FindControl("照会管理番号"), TextBox).Text)
            strKeyList.Add(CType(rowData.FindControl("連番"), TextBox).Text)
            strKeyList.Add(CType(rowData.FindControl("ＮＬ区分"), TextBox).Text)
            strKeyList.Add(CType(rowData.FindControl("ＩＤＩＣ区分"), TextBox).Text)
            strKeyList.Add(CType(rowData.FindControl("運用日付"), TextBox).Text)
            strKeyList.Add(CType(rowData.FindControl("要求通番"), TextBox).Text)
            strKeyList.Add(CType(rowData.FindControl("要求枝番"), TextBox).Text)
            'WATINQP001-004
            strKeyList.Add(strDataCls)                                                  '7
            strKeyList.Add(CType(rowData.FindControl("データ種別"), TextBox).Text)      '8
            strKeyList.Add(CType(rowData.FindControl("要求日時"), TextBox).Text)        '9
            strKeyList.Add(CType(rowData.FindControl("TBOXID"), TextBox).Text.Trim)     '10
            strKeyList.Add(CType(rowData.FindControl("ホール名"), TextBox).Text.Trim)   '11
            'WATINQP001-004 END

            '帳票出力処理をコール
            Call msPrint_Pdf(strDataCls, strKeyList.ToArray)

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

        Finally

            psLogEnd(Me)

        End Try

    End Sub

    ''' <summary>
    ''' 画面更新ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGmnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        objStack = New StackFrame

        Try
            psLogStart(Me)

            If (Page.IsValid) Then

                '結果データ取得処理
                Call mfGet_ResultData("0")

            End If

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

        Finally

            psLogEnd(Me)

        End Try

    End Sub

    ''' <summary>
    ''' CID,入金伝票番号 選択時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub rbtCID_CheckedChanged(sender As Object, e As EventArgs) Handles rbtCID.CheckedChanged, rbtNo.CheckedChanged

        Me.txtCID.ppText = Nothing
        Me.txtNyukinDenpyoNo.ppText = Nothing

        If rbtCID.Checked = True Then
            txtCID.ppEnabled = True
            txtNyukinDenpyoNo.ppEnabled = False
            txtCID.ppTextBox.Focus()
        ElseIf rbtNo.Checked = True Then
            txtCID.ppEnabled = False
            txtNyukinDenpyoNo.ppEnabled = True
            txtNyukinDenpyoNo.ppTextBox.Focus()
        Else
            txtCID.ppEnabled = False
            txtNyukinDenpyoNo.ppEnabled = False
        End If

    End Sub

    'WATINQP001-003
    Protected Sub grvInquiryList_DataBound(sender As Object, e As EventArgs) Handles grvInquiryList.DataBound

        If grvInquiryList.Rows.Count > 0 Then
            Me.btnInquiry.Enabled = True
        Else
            Me.btnInquiry.Enabled = False
        End If

    End Sub


#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen(ByVal ipshtMode As ClsComVer.E_遷移条件,
                              ByRef ipstrOldDisp As String,
                              ByRef ipstrKeyCode As String())

        objStack = New StackFrame

        Try

            '画面初期化処理
            Select Case ipshtMode
                Case ClsComVer.E_遷移条件.参照
                    Select Case ipstrOldDisp
                        Case P_FUN_CNS & P_SCR_UPD & P_PAGE & "001", P_FUN_HEA & P_SCR_UPD & P_PAGE & "001"     '工事依頼書兼仕様書,ヘルスチェック
                            Me.txtTboxId1.ppText = ipstrKeyCode(0)
                            Me.lblHallName.Text = Nothing
                            Me.lblNlKubun.Text = Nothing
                            'Me.txtTboxId1.ppText = Nothing
                            Me.txtTboxId2.ppText = Nothing
                            Me.dtbTboxUnyoDate.ppText = Nothing
                            Me.txtBB1SerialNo.ppText = Nothing
                            Me.txtCID.ppText = Nothing
                            Me.txtNyukinDenpyoNo.ppText = Nothing
                            Me.dtbInquiryDate.ppText = Nothing
                            Me.grvInquiryList.DataSource = New DataTable
                            Me.grvInquiryList.DataBind()
                            Me.grvResultList.DataSource = New DataTable
                            Me.grvResultList.DataBind()
                            Me.ddlDataSbt.ppSelectedValue = Nothing
                            Me.dtbTboxUnyoDate.ppEnabled = False
                            Me.txtBB1SerialNo.ppEnabled = False
                            Me.txtCID.ppEnabled = False
                            Me.txtNyukinDenpyoNo.ppEnabled = False
                            Me.rbtCID.Enabled = True
                            Me.rbtNo.Enabled = False
                            Me.rbtCID.Checked = True
                            Me.rbtNo.Checked = False
                            psLogStart(Me)

                            Dim conDB As SqlConnection = Nothing
                            Dim cmdDB As SqlCommand = Nothing
                            Dim dstOrders_1 As DataSet
                            Dim dstOrders_2 As DataSet
                            Dim strTboxVerCd As String = Nothing

                            '照会明細をクリア
                            Call msClearInqTable()
                            Me.grvInquiryList.DataSource = New DataTable
                            Me.DataBind()

                            '選択行番号クリア
                            ViewState.Remove("選択行番号")

                            'ボタン制御
                            ms_changeBtn("入力消去")

                            If Me.txtTboxId1.ppText = String.Empty Then

                                Me.lblHallName.Text = String.Empty
                                Me.lblNlKubun.Text = String.Empty

                                Exit Sub

                            End If

                            'TBOXID数値チェック
                            If pfCheck_Num(txtTboxId1.ppText) = False Then
                                txtTboxId1.psSet_ErrorNo("4002", "ＴＢＯＸＩＤ")
                            End If

                            Try
                                'DB接続
                                If clsDataConnect.pfOpen_Database(conDB) Then
                                    cmdDB = New SqlCommand(M_DISP_ID & "_S1", conDB)
                                    With cmdDB.Parameters
                                        'パラメータ設定
                                        .Add(pfSet_Param("tbox_id", SqlDbType.NVarChar, Me.txtTboxId1.ppText))
                                    End With

                                    dstOrders_1 = clsDataConnect.pfGet_DataSet(cmdDB)

                                    If dstOrders_1.Tables(0).Rows.Count = 0 Then
                                        '0件
                                        Me.txtTboxId1.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
                                        Exit Sub
                                    End If

                                    If dstOrders_1.Tables(0).Rows(0).Item("システム分類") = "1" Then
                                        'システム分類が「ID」の場合
                                        Me.txtTboxId1.psSet_ErrorNo("7001")

                                        'Me.btnInquiry.Enabled = False
                                        Me.ddlDataSbt.ppEnabled = False
                                        Me.ddlDataSbt.ppSelectedValue = Nothing
                                        Exit Sub
                                    End If

                                    'ホール名、NL区分を表示する
                                    Me.lblHallName.Text = dstOrders_1.Tables(0).Rows(0).Item("ホール名")
                                    Me.lblNlKubun.Text = dstOrders_1.Tables(0).Rows(0).Item("NL区分")

                                    'ビューステートに保存
                                    ViewState("ＮＬ区分") = dstOrders_1.Tables(0).Rows(0).Item("NL区分")
                                    ViewState("システム分類") = dstOrders_1.Tables(0).Rows(0).Item("システム分類")

                                    strTboxVerCd = dstOrders_1.Tables(0).Rows(0).Item("ＴＢＯＸ種別")

                                    'データ種別を表示する
                                    cmdDB = New SqlCommand(M_DISP_ID & "_S3", conDB)
                                    With cmdDB.Parameters
                                        'パラメータ設定
                                        .Add(pfSet_Param("tbox_ver", SqlDbType.NVarChar, strTboxVerCd))
                                    End With

                                    dstOrders_2 = clsDataConnect.pfGet_DataSet(cmdDB)

                                    Me.ddlDataSbt.ppEnabled = True
                                    Me.ddlDataSbt.ppdropdownlist.DataSource = dstOrders_2.Tables(0)
                                    Me.ddlDataSbt.ppdropdownlist.DataTextField = "データ種別"
                                    Me.ddlDataSbt.ppdropdownlist.DataValueField = "データ種別コード"
                                    Me.ddlDataSbt.DataBind()
                                    Me.ddlDataSbt.ppdropdownlist.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加
                                    Me.ddlDataSbt.ppdropdownlist.Items.Add(New ListItem("店内通信", "ZZ"))          '末尾に店内通信を追加

                                    'ボタン制御
                                    ms_changeBtn("入力")

                                Else
                                    'DB接続に失敗
                                    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

                                End If

                            Catch ex As Exception

                                'ログ出力
                                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

                            End Try
                        Case Else
                            Me.lblHallName.Text = Nothing
                            Me.lblNlKubun.Text = Nothing
                            Me.txtTboxId1.ppText = Nothing
                            Me.txtTboxId2.ppText = Nothing
                            Me.dtbTboxUnyoDate.ppText = Nothing
                            Me.txtBB1SerialNo.ppText = Nothing
                            Me.txtCID.ppText = Nothing
                            Me.txtNyukinDenpyoNo.ppText = Nothing
                            Me.dtbInquiryDate.ppText = Nothing
                            Me.grvInquiryList.DataSource = New DataTable
                            Me.grvInquiryList.DataBind()
                            Me.grvResultList.DataSource = New DataTable
                            Me.grvResultList.DataBind()
                            Me.ddlDataSbt.ppSelectedValue = Nothing
                            Me.dtbTboxUnyoDate.ppEnabled = False
                            Me.txtBB1SerialNo.ppEnabled = False
                            Me.txtCID.ppEnabled = False
                            Me.txtNyukinDenpyoNo.ppEnabled = False
                            Me.rbtCID.Enabled = False
                            Me.rbtNo.Enabled = False
                            Me.rbtCID.Checked = True
                            Me.rbtNo.Checked = False
                            Me.ddlDataSbt.ppEnabled = False
                    End Select
                Case Else
                    Me.lblHallName.Text = Nothing
                    Me.lblNlKubun.Text = Nothing
                    Me.txtTboxId1.ppText = Nothing
                    Me.txtTboxId2.ppText = Nothing
                    Me.dtbTboxUnyoDate.ppText = Nothing
                    Me.txtBB1SerialNo.ppText = Nothing
                    Me.txtCID.ppText = Nothing
                    Me.txtNyukinDenpyoNo.ppText = Nothing
                    Me.dtbInquiryDate.ppText = Nothing
                    Me.grvInquiryList.DataSource = New DataTable
                    Me.grvInquiryList.DataBind()
                    Me.grvResultList.DataSource = New DataTable
                    Me.grvResultList.DataBind()
                    Me.ddlDataSbt.ppSelectedValue = Nothing
                    Me.dtbTboxUnyoDate.ppEnabled = False
                    Me.txtBB1SerialNo.ppEnabled = False
                    Me.txtCID.ppEnabled = False
                    Me.rbtCID.Enabled = False
                    Me.rbtNo.Enabled = False
                    Me.txtNyukinDenpyoNo.ppEnabled = False
                    Me.rbtCID.Checked = True
                    Me.rbtNo.Checked = False
                    Me.ddlDataSbt.ppEnabled = False

            End Select

            'NL区分にフォーカス
            Me.lblNlKubun.Focus()

            'ボタン制御
            ms_changeBtn("初期")

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

    End Sub

    ''' <summary>
    ''' 照会テーブルをクリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearInqTable()

        'データ種別以外の入力値を初期化、非表示
        Me.dtbTboxUnyoDate.ppText = Nothing
        Me.dtbTboxUnyoDate.ppEnabled = False
        Me.txtBB1SerialNo.ppText = Nothing
        Me.txtBB1SerialNo.ppEnabled = False
        Me.txtCID.ppText = Nothing
        Me.txtCID.ppEnabled = False
        Me.rbtCID.Enabled = False
        Me.rbtNo.Enabled = False
        Me.rbtCID.Checked = True
        Me.rbtNo.Checked = False
        Me.txtNyukinDenpyoNo.ppText = Nothing
        Me.txtNyukinDenpyoNo.ppEnabled = False

    End Sub

    ''' <summary>
    ''' 照会明細からのデータ取得処理
    ''' </summary>
    ''' <param name="ipdstOrders_1"></param>
    ''' <remarks></remarks>
    Private Sub msGet_InquiryData(ByRef ipdstOrders_1 As WATINQP001_DATASET)

        Dim dtRow As DataRow

        '照会明細のデータを保管用照会データテーブルに追加
        Dim dttTable As DataTable = pfParse_DataTable(Me.grvInquiryList)
        dtRow = ipdstOrders_1.KeepInquiryDtt.NewRow
        For i = 0 To dttTable.Rows.Count - 1
            dtRow = ipdstOrders_1.KeepInquiryDtt.NewKeepInquiryDttRow
            dtRow("データ種別コード") = dttTable.Rows(i).Item(0)
            dtRow("データ種別") = dttTable.Rows(i).Item(1)
            dtRow("TBOX運用日") = dttTable.Rows(i).Item(2)
            dtRow("BB1シリアル番号") = dttTable.Rows(i).Item(3)
            dtRow("CID") = dttTable.Rows(i).Item(4)
            dtRow("入金伝票番号") = dttTable.Rows(i).Item(5)
            ipdstOrders_1.KeepInquiryDtt.AddKeepInquiryDttRow(dtRow)
        Next

    End Sub

    ''' <summary>
    ''' 結果データ取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfGet_ResultData(ByVal ipstrFlag As String) As Boolean

        objStack = New StackFrame

        Try
            Dim strYokyuDate As String = Nothing
            Dim strTboxId As String = Nothing
            Dim conDB As SqlConnection = Nothing
            Dim cmdDB As SqlCommand = Nothing
            Dim dstOrders_2 As DataSet

            '照会処理からの結果表示の場合
            If ipstrFlag = "2" Then
                '
                Me.txtTboxId2.ppText = Me.txtTboxId1.ppText
                Me.dtbInquiryDate.ppDate = Date.Now

            End If

            strYokyuDate = Me.dtbInquiryDate.ppText.Replace("/", "")
            strTboxId = Me.txtTboxId2.ppText

            Try
                If clsDataConnect.pfOpen_Database(conDB) Then
                    cmdDB = New SqlCommand(M_DISP_ID & "_S2", conDB)
                    With cmdDB.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("yokyu_date", SqlDbType.NVarChar, strYokyuDate))
                        .Add(pfSet_Param("tbox_id", SqlDbType.NVarChar, strTboxId))
                    End With

                    dstOrders_2 = clsDataConnect.pfGet_DataSet(cmdDB)

                    If ipstrFlag <> "1" And dstOrders_2.Tables(0).Rows.Count = 0 Then
                        '0件
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    End If

                    '保管用結果データテーブルを結果明細に設定
                    Me.grvResultList.DataSource = dstOrders_2.Tables(0)
                    Me.grvResultList.DataBind()

                    ViewState("結果明細データソース") = dstOrders_2.Tables(0)

                Else
                    'DB接続に失敗
                    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    Return False
                End If

                Return True

            Catch ex As Exception

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

                Return False

            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

            Return False

        Finally

        End Try

    End Function

    ''' <summary>
    ''' ボタン制御/項目の制御
    ''' </summary>
    ''' <param name="onBtnName"></param>
    ''' <remarks></remarks>
    Private Sub ms_changeBtn(ByVal onBtnName As String)

        Select Case onBtnName

            Case "初期"

                Me.btnClear.Enabled = True
                Me.btnAdd.Enabled = False
                Me.btnUpdate.Enabled = False
                Me.btnDelete.Enabled = False

            Case "クリア"

                Me.btnClear.Enabled = True
                Me.btnAdd.Enabled = False
                Me.btnUpdate.Enabled = False
                Me.btnDelete.Enabled = False

                '全て非表示
                Me.ddlDataSbt.ppDropDownList.SelectedIndex = 0
                Me.ddlDataSbt.ppEnabled = True
                Me.dtbTboxUnyoDate.ppEnabled = False
                Me.dtbTboxUnyoDate.ppText = Nothing
                Me.txtBB1SerialNo.ppEnabled = False
                Me.txtBB1SerialNo.ppText = Nothing
                Me.txtCID.ppEnabled = False
                Me.txtCID.ppText = Nothing
                Me.txtNyukinDenpyoNo.ppEnabled = False
                Me.txtNyukinDenpyoNo.ppText = Nothing
                Me.rbtCID.Enabled = False
                Me.rbtNo.Enabled = False
                Me.rbtCID.Checked = True
                Me.rbtNo.Checked = False

            Case "更新"

                Me.btnClear.Enabled = True
                Me.btnAdd.Enabled = False
                Me.btnUpdate.Enabled = False
                Me.btnDelete.Enabled = False

                '全て非表示
                Me.ddlDataSbt.ppDropDownList.SelectedIndex = 0
                Me.ddlDataSbt.ppEnabled = True
                Me.dtbTboxUnyoDate.ppEnabled = False
                Me.dtbTboxUnyoDate.ppText = Nothing
                Me.txtBB1SerialNo.ppEnabled = False
                Me.txtBB1SerialNo.ppText = Nothing
                Me.txtCID.ppEnabled = False
                Me.txtCID.ppText = Nothing
                Me.txtNyukinDenpyoNo.ppEnabled = False
                Me.txtNyukinDenpyoNo.ppText = Nothing
                Me.rbtCID.Enabled = False
                Me.rbtNo.Enabled = False
                Me.rbtCID.Checked = False
                Me.rbtNo.Checked = False

            Case "追加"

                Me.btnClear.Enabled = True
                Me.btnAdd.Enabled = False
                Me.btnUpdate.Enabled = False
                Me.btnDelete.Enabled = False

                '全て非表示
                Me.ddlDataSbt.ppDropDownList.SelectedIndex = 0
                Me.ddlDataSbt.ppEnabled = True
                Me.dtbTboxUnyoDate.ppEnabled = False
                Me.dtbTboxUnyoDate.ppText = Nothing
                Me.txtBB1SerialNo.ppEnabled = False
                Me.txtBB1SerialNo.ppText = Nothing
                Me.txtCID.ppEnabled = False
                Me.txtCID.ppText = Nothing
                Me.txtNyukinDenpyoNo.ppEnabled = False
                Me.txtNyukinDenpyoNo.ppText = Nothing
                Me.rbtCID.Enabled = False
                Me.rbtNo.Enabled = False
                Me.rbtCID.Checked = True
                Me.rbtNo.Checked = False

            Case "削除"

                Me.btnClear.Enabled = True
                Me.btnAdd.Enabled = False
                Me.btnDelete.Enabled = False
                Me.btnUpdate.Enabled = False

                '全て非表示
                Me.ddlDataSbt.ppDropDownList.SelectedIndex = 0
                Me.ddlDataSbt.ppEnabled = True
                Me.dtbTboxUnyoDate.ppEnabled = False
                Me.dtbTboxUnyoDate.ppText = Nothing
                Me.txtBB1SerialNo.ppEnabled = False
                Me.txtBB1SerialNo.ppText = Nothing
                Me.txtCID.ppEnabled = False
                Me.txtCID.ppText = Nothing
                Me.txtNyukinDenpyoNo.ppEnabled = False
                Me.txtNyukinDenpyoNo.ppText = Nothing
                Me.rbtCID.Enabled = False
                Me.rbtNo.Enabled = False
                Me.rbtCID.Checked = True
                Me.rbtNo.Checked = False

            Case "選択"

                Me.btnClear.Enabled = True
                Me.btnAdd.Enabled = False
                Me.btnUpdate.Enabled = True
                Me.btnDelete.Enabled = True

            Case "入力消去"

                Me.btnClear.Enabled = True
                Me.btnAdd.Enabled = False
                Me.btnUpdate.Enabled = False
                Me.btnDelete.Enabled = False

                '非表示
                Me.ddlDataSbt.ppEnabled = False

            Case "入力"

                Me.btnClear.Enabled = True
                Me.btnAdd.Enabled = False
                Me.btnUpdate.Enabled = False
                Me.btnDelete.Enabled = False

            Case "リスト選択"

                Me.btnClear.Enabled = True
                Me.btnAdd.Enabled = True
                Me.btnUpdate.Enabled = False
                Me.btnDelete.Enabled = False

            Case "リスト未選択"

                Me.btnClear.Enabled = True
                Me.btnAdd.Enabled = False
                Me.btnUpdate.Enabled = False
                Me.btnDelete.Enabled = False

        End Select


        '追加ボタンを非活性
        If Me.grvInquiryList.Rows.Count >= 15 Then
            Me.btnAdd.Enabled = False
        End If


    End Sub

    ''' <summary>
    ''' 照会テーブル表示／非表示処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub msSet_InquiryTable()

        '選択されたデータ種別
        Dim strSlctDataSbt As String = Me.ddlDataSbt.ppDropDownList.SelectedItem.Value

        Select Case strSlctDataSbt

            Case "", "27", "29", "86", "ZZ", "88"
                '全て非表示
                Me.dtbTboxUnyoDate.ppEnabled = False
                Me.dtbTboxUnyoDate.ppText = Nothing
                Me.txtBB1SerialNo.ppEnabled = False
                Me.txtBB1SerialNo.ppText = Nothing
                Me.txtCID.ppEnabled = False
                Me.txtCID.ppText = Nothing
                Me.txtNyukinDenpyoNo.ppEnabled = False
                Me.txtNyukinDenpyoNo.ppText = Nothing
                Me.rbtCID.Enabled = False
                Me.rbtNo.Enabled = False
                Me.rbtCID.Checked = True
                Me.rbtNo.Checked = False

            Case "25"
                'CID、入金伝票番号を表示
                Me.dtbTboxUnyoDate.ppEnabled = False
                Me.dtbTboxUnyoDate.ppText = Nothing
                Me.txtBB1SerialNo.ppEnabled = False
                Me.txtBB1SerialNo.ppText = Nothing
                Me.txtCID.ppEnabled = True
                Me.txtCID.ppText = Nothing
                Me.txtNyukinDenpyoNo.ppEnabled = False
                Me.txtNyukinDenpyoNo.ppText = Nothing
                Me.rbtCID.Enabled = True
                Me.rbtNo.Enabled = True
                Me.rbtCID.Checked = True
                Me.rbtNo.Checked = False

            Case "f1"
                'CIDを表示
                Me.dtbTboxUnyoDate.ppEnabled = False
                Me.dtbTboxUnyoDate.ppText = Nothing
                Me.txtBB1SerialNo.ppEnabled = False
                Me.txtBB1SerialNo.ppText = Nothing
                Me.txtCID.ppEnabled = True
                Me.txtCID.ppText = Nothing
                Me.txtNyukinDenpyoNo.ppEnabled = False
                Me.txtNyukinDenpyoNo.ppText = Nothing
                Me.rbtCID.Enabled = True
                Me.rbtNo.Enabled = False
                Me.rbtCID.Checked = True
                Me.rbtNo.Checked = False

            Case "33" 'BB明細情報
                'TBOX運用日、シリアル番号を表示
                Me.dtbTboxUnyoDate.ppEnabled = True
                Me.dtbTboxUnyoDate.ppText = Nothing
                Me.txtBB1SerialNo.ppEnabled = True
                Me.txtBB1SerialNo.ppText = Nothing
                Me.txtCID.ppEnabled = False
                Me.txtCID.ppText = Nothing
                Me.txtNyukinDenpyoNo.ppEnabled = False
                Me.txtNyukinDenpyoNo.ppText = Nothing
                Me.rbtCID.Enabled = False
                Me.rbtNo.Enabled = False
                Me.rbtCID.Checked = True
                Me.rbtNo.Checked = False

            Case Else
                'TBOX運用日を表示
                Me.dtbTboxUnyoDate.ppEnabled = True
                Me.dtbTboxUnyoDate.ppText = Nothing
                Me.txtBB1SerialNo.ppEnabled = False
                Me.txtBB1SerialNo.ppText = Nothing
                Me.txtCID.ppEnabled = False
                Me.txtCID.ppText = Nothing
                Me.txtNyukinDenpyoNo.ppEnabled = False
                Me.txtNyukinDenpyoNo.ppText = Nothing
                Me.rbtCID.Enabled = False
                Me.rbtNo.Enabled = False
                Me.rbtCID.Checked = True
                Me.rbtNo.Checked = False

        End Select

    End Sub


    Private Sub msSet_Requred()

        '選択されたデータ種別
        Dim strSlctDataSbt As String = Me.ddlDataSbt.ppSelectedValue

        Select Case strSlctDataSbt

            Case "", "27", "29", "86", "ZZ"
                '項目なし
                Me.dtbTboxUnyoDate.ppRequiredField = False
                Me.txtBB1SerialNo.ppRequiredField = False
                Me.txtCID.ppRequiredField = False
                Me.txtNyukinDenpyoNo.ppRequiredField = False

            Case "25"

                '必須なし（CID、入金伝票番号のどちらかに入力）
                Me.dtbTboxUnyoDate.ppRequiredField = False
                Me.txtBB1SerialNo.ppRequiredField = False
                Me.txtCID.ppRequiredField = False
                Me.txtNyukinDenpyoNo.ppRequiredField = False

            Case "f1"

                'CIDが必須
                Me.dtbTboxUnyoDate.ppRequiredField = False
                Me.txtBB1SerialNo.ppRequiredField = False
                Me.txtCID.ppRequiredField = True
                Me.txtNyukinDenpyoNo.ppRequiredField = False

            Case "33" 'BB明細情報

                Me.dtbTboxUnyoDate.ppRequiredField = True
                Me.txtBB1SerialNo.ppRequiredField = True
                Me.txtCID.ppRequiredField = False
                Me.txtNyukinDenpyoNo.ppRequiredField = False

            Case Else
                'TBOX運用日が必須
                Me.dtbTboxUnyoDate.ppRequiredField = True
                Me.txtBB1SerialNo.ppRequiredField = False
                Me.txtCID.ppRequiredField = False
                Me.txtNyukinDenpyoNo.ppRequiredField = False

        End Select

    End Sub

    ''' <summary>
    ''' 使用中カードＤＢの整合性チェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_chkUseCdDb()

        'どちらも未入力の場合
        If Me.txtCID.ppText Is Nothing And Me.txtNyukinDenpyoNo.ppText Is Nothing _
            Or Me.txtCID.ppText = String.Empty And Me.txtNyukinDenpyoNo.ppText = String.Empty Then

            Me.txtCID.psSet_ErrorNo("2005", Me.txtCID.ppName, Me.txtNyukinDenpyoNo.ppName)
            Me.txtNyukinDenpyoNo.psSet_ErrorNo("2005", Me.txtCID.ppName, Me.txtNyukinDenpyoNo.ppName)

        End If

        'どちらも入力されている場合
        If Me.txtCID.ppText <> String.Empty And Me.txtNyukinDenpyoNo.ppText <> String.Empty Then

            Me.txtCID.psSet_ErrorNo("2005", Me.txtCID.ppName, Me.txtNyukinDenpyoNo.ppName)
            Me.txtNyukinDenpyoNo.psSet_ErrorNo("2005", Me.txtCID.ppName, Me.txtNyukinDenpyoNo.ppName)

        End If

    End Sub

    ''' <summary>
    ''' データ種別リスト選択／未選択判定処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheck_List()

        'データ種別が未選択の場合
        If Not pfCheck_ListErr(ddlDataSbt.ppSelectedValue, True) = Nothing Then

            ddlDataSbt.psSet_ErrorNo("5003", ddlDataSbt.ppName)

        End If

    End Sub

    ''' <summary>
    ''' 照会情報重複チェック処理
    ''' </summary>
    ''' <param name="ipdttData"></param>
    ''' <param name="ipdtrNewRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfCheack_InqData(ByVal ipdttData As DataTable, ByVal ipdtrNewRow As DataRow) As Boolean

        Dim dtrMsg As DataRow = Nothing
        Dim strMsgPrm As String = Nothing

        '照会情報重複チェック
        For Each rowData As DataRow In ipdttData.Rows

            If rowData.ItemArray.SequenceEqual(ipdtrNewRow.ItemArray) Then

                'メッセージ埋め込み文言の設定
                strMsgPrm = "データ種別"

                If Me.dtbTboxUnyoDate.ppText <> "" Then

                    strMsgPrm += "、ＴＢＯＸ運用日"

                End If

                If Me.txtBB1SerialNo.ppText <> "" Then

                    strMsgPrm += "、ＢＢ１シリアル番号"

                End If

                If Me.txtCID.ppText <> "" Then

                    strMsgPrm += "、ＣＩＤ"

                End If

                If Me.txtNyukinDenpyoNo.ppText <> "" Then

                    strMsgPrm += "、入金伝票番号"

                End If

                ddlDataSbt.psSet_ErrorNo("2006", ddlDataSbt.ppName)

                Return False

            End If

        Next

        Return True

    End Function

    ''' <summary>
    ''' 帳票出力処理
    ''' </summary>
    ''' <param name="ipstrDataSbtCd"></param>
    ''' <remarks></remarks>
    Private Sub msPrint_Pdf(ByVal ipstrDataSbtCd As String,
                            ByVal ipstrKeyList() As String)

        objStack = New StackFrame

        Try
            Dim rpt As Object = Nothing
            Dim strSql As String = Nothing
            Dim strSqlPrm As String = Nothing
            Dim strFileName As String = Nothing
            strSqlPrm = " '" & ipstrKeyList(0) & "', '" & ipstrKeyList(1) & "', '" & _
                            ipstrKeyList(2) & "', '" & ipstrKeyList(3) & "', '" & _
                            ipstrKeyList(4) & "', '" & ipstrKeyList(5) & "', '" & _
                            ipstrKeyList(6) & "'"

            Select Case ipstrDataSbtCd
                Case "25"
                    rpt = New TBRREP001
                    strSql = "EXEC TBRREP001_S1" & strSqlPrm
                    strFileName = "使用中カードＤＢ"

                Case "26"           '決済照会情報
                    '選択画面を開く
                    Dim strPath As String = "~/" & P_WAT & "/" & M_KSI_DISP_ID & "/" & M_KSI_DISP_ID & ".aspx"
                    Session(P_SESSION_BCLIST) = Master.ppBcList_Text
                    Session(P_KEY) = ipstrKeyList

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

                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, strPath, strPrm, "TRANS")
                    psOpen_Window(Me, strPath)

                    Exit Sub

                Case "27"

                    '双子店判定
                    Dim conDB As New SqlConnection
                    Dim cmdDB As New SqlCommand
                    Dim wkDS As New DataSet
                    Dim blnTwinFlg As Boolean = False

                    If clsDataConnect.pfOpen_Database(conDB) Then
                        cmdDB = New SqlCommand("ZCMPSEL063", conDB)
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("TBOXID", SqlDbType.NVarChar, ipstrKeyList(10)))
                            .Add(pfSet_Param("Delete_flg", SqlDbType.NVarChar, "0"))
                        End With

                        wkDS = clsDataConnect.pfGet_DataSet(cmdDB)

                        If wkDS.Tables(0).Rows.Count = 0 Then
                            '0件
                            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        End If

                        If wkDS.Tables(0).Rows(0).Item("T01_TWIN_CD").ToString = "1" _
                        Or wkDS.Tables(0).Rows(0).Item("T01_TWIN_CD").ToString = "2" Then
                            blnTwinFlg = True
                        End If
                    Else
                        'DB接続に失敗
                        psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        Exit Sub
                    End If

                    If blnTwinFlg = True Then
                        '双子店の場合
                        Dim aryRPT As Object()
                        Dim arySQL As String()
                        Dim aryFile As String()
                        aryRPT = {New TBRREP003, New TBRREP018}
                        arySQL = {"EXEC TBRREP003_S1" & strSqlPrm, "EXEC TBRREP018_S1" & strSqlPrm}
                        aryFile = {"店内装置構成表", "店内装置構成表（島構成）"}
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", strSql & ":File=" & strFileName, "PRT")
                        '帳票を出力する
                        psPrintPDF(Me, aryRPT, arySQL, aryFile)
                        Exit Sub
                    Else
                        '単独店の場合
                        rpt = New TBRREP003
                        strSql = "EXEC TBRREP003_S1" & strSqlPrm
                        strFileName = "店内装置構成表"
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", strSql & ":File=" & strFileName, "PRT")
                        '帳票を出力する
                        psPrintPDF(Me, rpt, strSql, strFileName)
                        Exit Sub
                    End If

'                    rpt = New TBRREP003
'                    strSql = "EXEC TBRREP003_S1" & strSqlPrm
'                    strFileName = "店内装置構成表"

                Case "29", "30", "35", "36", "39", "40", "42", "86", "ZZ"

                    Dim strPath As String = "~/" & P_WAT & "/" & M_COM_DISP_ID & "/" & M_COM_DISP_ID & ".aspx"
                    Session(P_SESSION_BCLIST) = Master.ppBcList_Text
                    Session(P_KEY) = ipstrKeyList

                    psOpen_Window(Me, strPath)

                    Exit Sub

                'Case "30"
                '    rpt = New TBRREP005
                '    strSql = "EXEC TBRREP005_S1" & strSqlPrm
                '    strFileName = "ＢＢ装置情報"

                'Case "35"
                '    rpt = New TBRREP006
                '    strSql = "EXEC TBRREP006_S1" & strSqlPrm
                '    strFileName = "ＴＢＯＸ操作ログ"

                'Case "36"
                '    rpt = New TBRREP007
                '    strSql = "EXEC TBRREP007_S1" & strSqlPrm
                '    strFileName = "ＴＢＯＸエラーログ"

                'Case "39"
                '    rpt = New TBRREP008
                '    strSql = "EXEC TBRREP008_S1" & strSqlPrm
                '    strFileName = "債権明細情報"
                'Case "40"
                '    rpt = New TBRREP009
                '    strSql = "EXEC TBRREP009_S1" & strSqlPrm
                '    strFileName = "債務明細情報"

                'Case "42"
                '    rpt = New TBRREP010
                '    strSql = "EXEC TBRREP010_S1" & strSqlPrm
                '    strFileName = "精算明細情報"

                Case "f1"
                    rpt = New TBRREP011
                    strSql = "EXEC TBRREP011_S1" & strSqlPrm
                    strFileName = "使用中カードログ２"

                Case "ZZ"           '店内通信

                    ''選択画面を開く
                    'Dim strPath As String = "~/" & P_WAT & "/" & M_TNNI_DISP_ID & "/" &
                    '                        M_TNNI_DISP_ID & ".aspx"
                    'Session(P_SESSION_BCLIST) = Master.ppBcList_Text
                    'Session(P_KEY) = ipstrKeyList

                    'Dim objStack As New StackFrame
                    'Dim strPrm As String = ""
                    'strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
                    'Dim tmp As Object() = Session(P_KEY)
                    'If Not tmp Is Nothing Then
                    '    For zz = 0 To tmp.Length - 1
                    '        If zz <> tmp.Length - 1 Then
                    '            strPrm &= tmp(zz).ToString & ","
                    '        Else
                    '            strPrm &= tmp(zz).ToString
                    '        End If
                    '    Next
                    'End If

                    'psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                    '                objStack.GetMethod.Name, strPath, strPrm, "TRANS")

                    'psOpen_Window(Me, strPath)

                    'Exit Sub


                    'Case "86"
                    '    rpt = New TBRREP004
                    '    strSql = "EXEC TBRREP004_S2" & strSqlPrm
                    '    strFileName = "ＢＢ機番情報２"

                Case "88"   'ホール機器設定情報

                    '選択画面を開く
                    Dim strPath As String = "~/" & P_WAT & "/" & M_HALL_DISP_ID & "/" & M_HALL_DISP_ID & ".aspx"

                    Session(P_SESSION_BCLIST) = Master.ppBcList_Text
                    Session(P_KEY) = ipstrKeyList

                    'ログ出力
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

                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, strPath, strPrm, "TRANS")
                    psOpen_Window(Me, strPath)

                    Exit Sub

            End Select

            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", strSql & ":File=" & strFileName, "PRT")
            '帳票を出力する
            psPrintPDF(Me, rpt, strSql, strFileName)

        Catch ex As Exception

            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "帳票")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

    End Sub

    ' ''' <summary>
    ' ''' 帳票出力処理
    ' ''' </summary>
    ' ''' <param name="ipstrDataSbtCd"></param>
    ' ''' <remarks></remarks>
    'Private Sub msPrint_Pdf(ByVal ipstrDataSbtCd As String,
    '                        ByVal ipstrKeyList() As String)

    '    objStack = New StackFrame

    '    Try
    '        Dim rpt As Object = Nothing
    '        Dim strSql As String = Nothing
    '        Dim strSqlPrm As String = Nothing
    '        Dim strFileName As String = Nothing

    '        strSqlPrm = " '" & ipstrKeyList(0) & "', '" & ipstrKeyList(1) & "', '" &
    '                        ipstrKeyList(2) & "', '" & ipstrKeyList(3) & "', '" &
    '                        ipstrKeyList(4) & "', '" & ipstrKeyList(5) & "', '" &
    '                        ipstrKeyList(6) & "'"

    '        Select Case ipstrDataSbtCd
    '            Case "25"
    '                rpt = New TBRREP001
    '                strSql = "EXEC TBRREP001_S1" & strSqlPrm
    '                strFileName = "使用中カードＤＢ"

    '            Case "26"           '決済照会情報
    '                '選択画面を開く
    '                Dim strPath As String = "~/" & P_WAT & "/" & M_KSI_DISP_ID & "/" &
    '                                        M_KSI_DISP_ID & ".aspx"
    '                Session(P_SESSION_BCLIST) = Master.ppBcList_Text
    '                Session(P_KEY) = ipstrKeyList

    '                Dim objStack As New StackFrame
    '                Dim strPrm As String = ""
    '                strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
    '                Dim tmp As Object() = Session(P_KEY)
    '                If Not tmp Is Nothing Then
    '                    For zz = 0 To tmp.Length - 1
    '                        If zz <> tmp.Length - 1 Then
    '                            strPrm &= tmp(zz).ToString & ","
    '                        Else
    '                            strPrm &= tmp(zz).ToString
    '                        End If
    '                    Next
    '                End If

    '                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
    '                                objStack.GetMethod.Name, strPath, strPrm, "TRANS")

    '                psOpen_Window(Me, strPath)

    '                Exit Sub

    '            Case "27"
    '                rpt = New TBRREP003
    '                strSql = "EXEC TBRREP003_S1" & strSqlPrm
    '                strFileName = "店内装置構成表"

    '            Case "29"
    '                rpt = New TBRREP004
    '                strSql = "EXEC TBRREP004_S1" & strSqlPrm
    '                strFileName = "ＢＢ機番情報"

    '            Case "30"
    '                rpt = New TBRREP005
    '                strSql = "EXEC TBRREP005_S1" & strSqlPrm
    '                strFileName = "ＢＢ装置情報"

    '            Case "35"
    '                rpt = New TBRREP006
    '                strSql = "EXEC TBRREP006_S1" & strSqlPrm
    '                strFileName = "ＴＢＯＸ操作ログ"

    '            Case "36"
    '                rpt = New TBRREP007
    '                strSql = "EXEC TBRREP007_S1" & strSqlPrm
    '                strFileName = "ＴＢＯＸエラーログ"

    '            Case "39"
    '                rpt = New TBRREP008
    '                strSql = "EXEC TBRREP008_S1" & strSqlPrm
    '                strFileName = "債権明細情報"
    '            Case "40"
    '                rpt = New TBRREP009
    '                strSql = "EXEC TBRREP009_S1" & strSqlPrm
    '                strFileName = "債務明細情報"

    '            Case "42"
    '                rpt = New TBRREP010
    '                strSql = "EXEC TBRREP010_S1" & strSqlPrm
    '                strFileName = "精算明細情報"

    '            Case "f1"
    '                rpt = New TBRREP011
    '                strSql = "EXEC TBRREP011_S1" & strSqlPrm
    '                strFileName = "使用中カードログ２"

    '            Case "ZZ"           '店内通信
    '                '選択画面を開く
    '                Dim strPath As String = "~/" & P_WAT & "/" & M_TNNI_DISP_ID & "/" &
    '                                        M_TNNI_DISP_ID & ".aspx"
    '                Session(P_SESSION_BCLIST) = Master.ppBcList_Text
    '                Session(P_KEY) = ipstrKeyList

    '                Dim objStack As New StackFrame
    '                Dim strPrm As String = ""
    '                strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
    '                Dim tmp As Object() = Session(P_KEY)
    '                If Not tmp Is Nothing Then
    '                    For zz = 0 To tmp.Length - 1
    '                        If zz <> tmp.Length - 1 Then
    '                            strPrm &= tmp(zz).ToString & ","
    '                        Else
    '                            strPrm &= tmp(zz).ToString
    '                        End If
    '                    Next
    '                End If

    '                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
    '                                objStack.GetMethod.Name, strPath, strPrm, "TRANS")

    '                psOpen_Window(Me, strPath)

    '                Exit Sub

    '            Case "86"
    '                rpt = New TBRREP004
    '                strSql = "EXEC TBRREP004_S2" & strSqlPrm
    '                strFileName = "ＢＢ機番情報２"

    '            Case "88"
    '                '選択画面を開く
    '                Dim strPath As String = "~/" & P_WAT & "/" & M_HALL_DISP_ID & "/" &
    '                                        M_HALL_DISP_ID & ".aspx"

    '                Session(P_SESSION_BCLIST) = Master.ppBcList_Text
    '                Session(P_KEY) = ipstrKeyList

    '                'ログ出力
    '                Dim objStack As New StackFrame
    '                Dim strPrm As String = ""
    '                strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
    '                Dim tmp As Object() = Session(P_KEY)
    '                If Not tmp Is Nothing Then
    '                    For zz = 0 To tmp.Length - 1
    '                        If zz <> tmp.Length - 1 Then
    '                            strPrm &= tmp(zz).ToString & ","
    '                        Else
    '                            strPrm &= tmp(zz).ToString
    '                        End If
    '                    Next
    '                End If

    '                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
    '                                objStack.GetMethod.Name, strPath, strPrm, "TRANS")

    '                psOpen_Window(Me, strPath)

    '                Exit Sub


    '        End Select

    '        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
    '                        objStack.GetMethod.Name, "", strSql & ":File=" & strFileName, "PRT")
    '        '帳票を出力する
    '        psPrintPDF(Me, rpt, strSql, strFileName)

    '    Catch ex As Exception

    '        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "帳票")

    '        'ログ出力
    '        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
    '                        objStack.GetMethod.Name, "", ex.ToString, "Catch")

    '    End Try

    'End Sub

    ''' <summary>
    ''' フォーカス制御
    ''' </summary>
    ''' <param name="ipControl"></param>
    ''' <remarks></remarks>
    Private Sub msFocusChange(ByVal ipControl As Control)

        Select Case ipControl.ID.ToString
            Case "txtTboxId1" '登録エリアTBOXID
                If Me.txtTboxId1.ppText = String.Empty Then
                    Me.txtTboxId2.ppTextBox.Focus()
                Else
                    Me.ddlDataSbt.Focus()
                End If
            Case "ddlDataSbt" '登録エリア照会種別
                For ctlCnt As Integer = 0 To 4
                    Select Case ctlCnt
                        Case 0
                            If Me.dtbTboxUnyoDate.ppEnabled = True Then
                                Me.dtbTboxUnyoDate.ppDateBox.Focus()
                                Exit For
                            End If
                        Case 1
                            If Me.txtBB1SerialNo.ppEnabled = True Then
                                Me.txtBB1SerialNo.ppTextBox.Focus()
                                Exit For
                            End If
                        Case 2
                            If Me.txtCID.ppEnabled = True Then
                                Me.txtCID.ppTextBox.Focus()
                                Exit For
                            End If
                        Case 3
                            If Me.txtNyukinDenpyoNo.ppEnabled = True Then
                                Me.txtNyukinDenpyoNo.ppTextBox.Focus()
                                Exit For
                            End If
                        Case 4
                            Me.btnClear.Focus()
                            Exit For
                    End Select
                Next
            Case "btnAdd" '登録エリア追加ボタン
                btnInquiry.Focus()
        End Select

    End Sub


#End Region

End Class
